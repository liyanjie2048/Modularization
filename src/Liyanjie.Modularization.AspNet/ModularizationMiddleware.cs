using System;
using System.Linq;
using System.Reflection;
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
        public async Task InvokeAsync(HttpContext httpContext)
        {
            foreach (var module in moduleTable.Modules)
            {
                foreach (var middleware in module.Value)
                {
                    if (false
                        || middleware.HttpMethods == null
                        || middleware.HttpMethods.Contains(httpContext.Request.HttpMethod))
                    {
                        var routeValues = new RouteValueDictionary();
                        var templateMatcher = new TemplateMatcher(TemplateParser.Parse(middleware.RouteTemplate), routeValues);
                        if (templateMatcher.TryMatch(httpContext.Request.Path, routeValues))
                        {
                            var _middleware = serviceProvider == null
                                ? moduleTable.ModuleOptions.TryGetValue(module.Key, out var moduleOptions)
                                    ? Activator.CreateInstance(middleware.HandlerType, moduleOptions)
                                    : Activator.CreateInstance(middleware.HandlerType)
                                : serviceProvider.GetServiceOrCreateInstance(middleware.HandlerType);

                            var method = middleware.HandlerType.GetMethod("HandleAsync", BindingFlags.Public | BindingFlags.Instance);
                            await (method.GetParameters().Length switch
                            {
                                0 => method.Invoke(_middleware, null) as Task,
                                1 => method.Invoke(_middleware, new[] { httpContext }) as Task,
                                2 => method.Invoke(_middleware, new object[] { httpContext, routeValues }) as Task,
                                _ => throw new NotSupportedException($"未找到匹配的 HandleAsync 方法"),
                            });
                            
                            return;
                        }
                    }
                }
            }
        }
    }
}
