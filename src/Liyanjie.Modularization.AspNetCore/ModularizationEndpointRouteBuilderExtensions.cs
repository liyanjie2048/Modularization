#if NETCOREAPP3_0
using System;

using Liyanjie.Modularization.AspNetCore;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModularizationEndpointRouteBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoints"></param>
        /// <returns></returns>
        public static IEndpointRouteBuilder MapModularization(this IEndpointRouteBuilder endpoints)
        {
            var moduleTable = endpoints.ServiceProvider.GetService<ModularizationModuleTable>();

            foreach (var module in moduleTable.Modules)
            {
                foreach (var middleware in module.Value)
                {
                    var pipeline = endpoints.CreateApplicationBuilder().UseMiddleware(middleware.Type).Build();
                    var displayName = $"Modularization-{module.Key}-{middleware.RouteTemplate}";
                    if (middleware.HttpMethods == null)
                        endpoints.Map(middleware.RouteTemplate, pipeline).WithDisplayName(displayName);
                    else
                        endpoints.MapMethods(middleware.RouteTemplate, middleware.HttpMethods, pipeline).WithDisplayName($"{displayName}-{string.Join('|', middleware.HttpMethods)}");
                }
            }

            return endpoints;
        }
    }
}
#endif
