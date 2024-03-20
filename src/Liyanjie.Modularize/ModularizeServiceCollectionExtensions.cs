namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 
/// </summary>
public static class ModularizeServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static ModularizeModuleTable AddModularize(this IServiceCollection services)
    {
        var moduleTable = new ModularizeModuleTable(services);
        services.AddSingleton(moduleTable);

        return moduleTable;
    }
}
