namespace Microsoft.Extensions.DependencyInjection;

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
    public static ModularizationModuleTable AddModularization(this IServiceCollection services)
    {
        var moduleTable = new ModularizationModuleTable(services);
        services.AddSingleton(moduleTable);

        return moduleTable;
    }
}
