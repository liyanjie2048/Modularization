using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class ModularizationMiddleware
#if NETSTANDARD2_0
        : IMiddleware
#endif
    {
#if !NETSTANDARD2_0
        readonly RequestDelegate next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public ModularizationMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            await InvokeAsync(httpContext, next);
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            var moduleTable = httpContext.RequestServices.GetRequiredService<ModularizationModuleTable>();

            foreach (var moduleType in moduleTable.ModuleTypes)
            {
                if (ActivatorUtilities.GetServiceOrCreateInstance(httpContext.RequestServices, moduleType) is IModularizationModule module)
                {
                    if (await module.TryMatchRequestingAsync(httpContext))
                    {
                        var authorized = ModularizationDefaults.AuthorizeAsync == null
                            ? true
                            : await ModularizationDefaults.AuthorizeAsync.Invoke(httpContext, module.Name);
                        if (authorized)
                        {
                            await module.HandleResponsingAsync(httpContext);
                        }
                        else
                        {
                            if (ModularizationDefaults.HandleUnauthorizeAsync != null)
                                await ModularizationDefaults.HandleUnauthorizeAsync.Invoke(httpContext, module.Name);
                        }
                        return;
                    }
                }
            }

            await next(httpContext);
        }
    }
}
