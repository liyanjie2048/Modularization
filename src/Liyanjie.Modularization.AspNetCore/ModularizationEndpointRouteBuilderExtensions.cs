#if NETCOREAPP3_0
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
                    var pipeline = endpoints.CreateApplicationBuilder().UseMiddleware(middleware.Value).Build();
                    endpoints.Map(middleware.Key, pipeline).WithDisplayName($"Modularization-{module.Key}-{middleware.Key}");
                }
            }

            return endpoints;
        }
    }
}
#endif
