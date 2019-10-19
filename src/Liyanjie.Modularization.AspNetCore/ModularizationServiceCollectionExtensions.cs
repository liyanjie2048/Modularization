using System;
using System.Threading.Tasks;

using Liyanjie.Modularization.AspNetCore;

using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModularizationServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="deserializeFromRequest"></param>
        /// <param name="serializeToResponse"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddModularization(this IServiceCollection services,
            Func<HttpRequest, Type, Task<object>> deserializeFromRequest,
            Func<HttpResponse, object, Task> serializeToResponse)
        {
            ModularizationDefaults.DeserializeFromRequestAsync = deserializeFromRequest ?? throw new ArgumentNullException(nameof(deserializeFromRequest));
            ModularizationDefaults.SerializeToResponseAsync = serializeToResponse ?? throw new ArgumentNullException(nameof(serializeToResponse));

            var moduleTable = new ModularizationModuleTable(services);
            services.AddSingleton(moduleTable);

            return moduleTable;
        }
    }
}
