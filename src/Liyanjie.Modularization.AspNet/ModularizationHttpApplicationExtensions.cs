using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Routing;

using Liyanjie.Modularization;
using Liyanjie.TemplateMatching;

using Microsoft.Extensions.DependencyInjection;

namespace System.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModularizationHttpApplicationExtensions
    {
        /// <summary>
        /// Add in Global.Application_BeginRequest
        /// </summary>
        /// <param name="app"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static HttpApplication UseModularization(this HttpApplication app
            , IServiceProvider serviceProvider)
        {
            var moduleTable = serviceProvider.GetRequiredService<ModularizationModuleTable>();
            foreach (var module in moduleTable.Modules)
            {
                foreach (var middleware in module.Value)
                {
                    if (false
                        || middleware.HttpMethods == null
                        || middleware.HttpMethods.Contains(app.Context.Request.HttpMethod))
                    {
                        var routeValues = new RouteValueDictionary();
                        var templateMatcher = new TemplateMatcher(TemplateParser.Parse(middleware.RouteTemplate), routeValues);
                        if (templateMatcher.TryMatch(app.Context.Request.Path, routeValues))
                        {
                            var _middleware = ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, middleware.HandlerType);
                            if (_middleware == null)
                                throw new NotSupportedException($"未能成功创建 {middleware.HandlerType.Name} 的实例");

                            var method = middleware.HandlerType.GetMethod("InvokeAsync", BindingFlags.Public | BindingFlags.Instance);
                            if (method == null)
                                throw new NotSupportedException($"在类型 {middleware.HandlerType.Name} 中未找到匹配的 InvokeAsync 方法");

                            var task = (method.GetParameters().Length switch
                            {
                                0 => method.Invoke(_middleware, null) as Task,
                                1 => method.Invoke(_middleware, new[] { app.Context }) as Task,
                                2 => method.Invoke(_middleware, new object[] { app.Context, routeValues }) as Task,
                                _ => throw new NotSupportedException($"未找到匹配的 InvokeAsync 方法"),
                            });
                            task.Wait();
                        }
                    }
                }
            }

            return app;
        }
    }
}
