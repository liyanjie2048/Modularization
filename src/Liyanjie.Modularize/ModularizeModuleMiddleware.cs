namespace Liyanjie.Modularize;

/// <summary>
/// 
/// </summary>
public struct ModularizeModuleMiddleware
{
    /// <summary>
    /// 
    /// </summary>
    public string[] HttpMethods { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string RouteTemplate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Type HandlerType { get; set; }
}
