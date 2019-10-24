using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

using Liyanjie.TemplateMatching;

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
            foreach (var module in moduleTable.Modules)
            {
                foreach (var middleware in module.Value)
                {
                    var routeValues = new RouteValueDictionary();
                    var templateMatcher = new TemplateMatcher(TemplateParser.Parse(middleware.Key), routeValues);
                    if (templateMatcher.TryMatch(httpContext.Request.Path, routeValues))
                    {
                        var _middleware = serviceProvider == null
                            ? Activator.CreateInstance(middleware.Value)
                            : serviceProvider.GetServiceOrCreateInstance(middleware.Value);

                        await (middleware.Value.GetMethod("HandleAsync").Invoke(_middleware, new[] { httpContext }) as Task);
                    }
                }
            }
        }
    }
}
