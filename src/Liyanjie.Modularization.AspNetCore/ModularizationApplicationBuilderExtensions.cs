using System;
using System.Linq;

using Liyanjie.Modularization.AspNetCore;

using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModularizationApplicationBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseModularization(this IApplicationBuilder app)
        {
            var moduleTable = app.ApplicationServices.GetService<ModularizationModuleTable>();

            foreach (var module in moduleTable.Modules)
            {
                foreach (var middleware in module.Value)
                {
                    app.MapWhen(httpContext =>
                    {
                        if (false
                            || middleware.HttpMethods == null
                            || middleware.HttpMethods.Contains(httpContext.Request.Method))
                        {
                            var routeValues = new RouteValueDictionary();
                            var templateMatcher = new TemplateMatcher(TemplateParser.Parse(middleware.RouteTemplate), routeValues);
                            return templateMatcher.TryMatch(httpContext.Request.Path, routeValues);
                        }
                        return false;
                    }, _app => _app.UseMiddleware(middleware.Type));
                }
            }

            return app;
        }
    }
}
