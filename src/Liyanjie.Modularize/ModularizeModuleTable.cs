namespace Liyanjie.Modularize;

/// <summary>
/// 
/// </summary>
public sealed class ModularizeModuleTable(IServiceCollection services)
{
    readonly Dictionary<string, ModularizeModuleMiddleware[]> modules = [];

    /// <summary>
    /// 
    /// </summary>
    public IServiceCollection Services => services;

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyDictionary<string, ModularizeModuleMiddleware[]> Modules => modules;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="moduleName"></param>
    /// <param name="moduleMiddlewares"></param>
    /// <param name="configureModuleOptions"></param>
    /// <returns></returns>
    public ModularizeModuleTable AddModule<TModuleOptions>(
        string moduleName,
        ModularizeModuleMiddleware[] moduleMiddlewares,
        Action<TModuleOptions>? configureModuleOptions = default)
        where TModuleOptions : class
    {
        modules[moduleName] = moduleMiddlewares;

        services.Configure(configureModuleOptions ?? (options => { }));

        return this;
    }
}
