# Modularization

- #### Liyanjie.Modularization
  - ModuleMiddleware
    ```csharp
    string[] HttpMethods { get; set; }
    string RouteTemplate { get; set; }
    HandlerType { get; set; }
    ```
  - ModuleTable
    ```csharp
    ModuleTable AddModule<TModuleOptions>(
        string moduleName,
        ModuleMiddleware[] moduleMiddlewares,
        Action<TModuleOptions> configureModuleOptions = null)
        where TModuleOptions : class
    ```
  - ExtendMethods
    ```csharp
    //services is IServiceCollection
    services.AddModularization()
        .AddModule<TModuleOptions>(
            string moduleName,
            ModuleMiddleware[] moduleMiddlewares,
            Action<TModuleOptions> configureModuleOptions = null)
            where TModuleOptions : class;
    ```
- #### Liyanjie.Modularization.AspNet
  - ExtendMethods
    ```csharp
    //app is HttpApplication
    app.UseModularization(IServiceProvider serviceProvider);
    ```
- #### Liyanjie.Modularization.AspNetCore
  - ExtendMethods
    ```csharp
    //endpoints is IEndpointRouteBuilder
    endpoints.MapModularization();
    ```