# Modularization

- #### Liyanjie.Modularization
  - ModularizationModuleMiddleware
    ```csharp
    string[] HttpMethods { get; set; }
    string RouteTemplate { get; set; }
    HandlerType { get; set; }
    ```
  - ModularizationModuleTable
    ```csharp
    ModuleTable AddModule<TModuleOptions>(
        string moduleName,
        ModuleMiddleware[] moduleMiddlewares,
        Action<TModuleOptions> configureModuleOptions = null)
        where TModuleOptions : class
    ```
  - Usage
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
  - Usage
    ```csharp
    //app is HttpApplication
    app.UseModularization(IServiceProvider serviceProvider);
    ```
- #### Liyanjie.Modularization.AspNetCore
  - Usage
    ```csharp
    //endpoints is IEndpointRouteBuilder
    endpoints.MapModularization();
    ```