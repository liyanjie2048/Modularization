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
    {
        readonly RequestDelegate next;
        readonly IServiceProvider serviceProvider;
        readonly ModularizationModuleTable moduleTable;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="serviceProvider"></param>
        public ModularizationMiddleware(
            RequestDelegate next,
            IServiceProvider serviceProvider)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            this.moduleTable = serviceProvider.GetRequiredService<ModularizationModuleTable>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            foreach (var moduleType in moduleTable.ModuleTypes)
            {
                if (ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, moduleType) is IModularizationModule module)
                {
                    if (await module.TryMatchRequestingAsync(httpContext))
                    {
                        await module.HandleResponsingAsync(httpContext);
                        return;
                    }
                }
            }

            await next(httpContext);
        }
    }
}
