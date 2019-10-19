using System;
using System.Threading.Tasks;
using System.Web;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class ModularizationMiddleware
    {
        readonly IServiceProvider serviceProvider;
        readonly ModularizationModuleTable moduleTable;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ModularizationMiddleware(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.moduleTable = serviceProvider.GetService(typeof(ModularizationModuleTable)) as ModularizationModuleTable ?? throw new ArgumentNullException(nameof(moduleTable));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleTable"></param>
        public ModularizationMiddleware(ModularizationModuleTable moduleTable)
        {
            this.moduleTable = moduleTable ?? throw new ArgumentNullException(nameof(moduleTable));
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
                var module = serviceProvider == null
                    ? moduleTable.TryGetOptions(moduleType, out var options)
                        ? Activator.CreateInstance(moduleType, options)
                        : Activator.CreateInstance(moduleType)
                    : serviceProvider.GetServiceOrCreateInstance(moduleType);
                if (module is IModularizationModule _module)
                {
                    if (await _module.TryMatchRequestingAsync(httpContext))
                    {
                        await _module.HandleResponsingAsync(httpContext);
                        httpContext.Response.End();
                    }
                }
            }
        }
    }
}
