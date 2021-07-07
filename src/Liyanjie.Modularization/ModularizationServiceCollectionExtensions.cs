using Liyanjie.Modularization;

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
        /// <returns></returns>
        public static ModuleTable AddModularization(this IServiceCollection services)
        {
            var moduleTable = new ModuleTable(services);
            services.AddSingleton(moduleTable);

            return moduleTable;
        }
    }
}
