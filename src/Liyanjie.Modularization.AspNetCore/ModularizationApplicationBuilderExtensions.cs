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
                        var routeValues = new RouteValueDictionary();
                        var templateMatcher = new TemplateMatcher(TemplateParser.Parse(middleware.Key), routeValues);
                        return templateMatcher.TryMatch(httpContext.Request.Path, routeValues);
                    }, _app => _app.UseMiddleware(middleware.Value));
                }
            }

            return app;
        }
    }
}
