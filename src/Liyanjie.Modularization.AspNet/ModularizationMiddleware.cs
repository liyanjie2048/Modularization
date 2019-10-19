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
        readonly ModularizationModuleTable moduleTable;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleTable"></param>
        public ModularizationMiddleware(ModularizationModuleTable moduleTable)
        {
            this.moduleTable = moduleTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            foreach (var moduleType in moduleTable.Modules.Keys)
            {
                if (Activator.CreateInstance(moduleType, moduleTable.Modules[moduleType]) is IModularizationModule module)
                {
                    if (await module.TryMatchRequestingAsync(httpContext))
                    {
                        await module.HandleResponsingAsync(httpContext);
                        httpContext.Response.End();
                    }
                }
            }
        }
    }
}
