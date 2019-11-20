using Liyanjie.Modularization.AspNet;

namespace System.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModularizationHttpApplicationExtensions
    {
        /// <summary>
        /// Add in Global.Application_Start (Use DI)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="registerServiceType"></param>
        /// <param name="registerServiceImplementationFactory"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddModularization(this HttpApplication app,
            Action<Type, string> registerServiceType,
            Action<Type, Func<IServiceProvider, object>, string> registerServiceImplementationFactory)
        {
            var moduleTable = new ModularizationModuleTable(registerServiceType, registerServiceImplementationFactory);
            registerServiceImplementationFactory.Invoke(typeof(ModularizationModuleTable), serviceProvider => moduleTable, "Singleton");
            registerServiceImplementationFactory.Invoke(typeof(ModularizationMiddleware), serviceProvider => new ModularizationMiddleware(serviceProvider), "Singleton");

            return moduleTable;
        }

        /// <summary>
        /// Add in Global.Application_BeginRequest (Use DI)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static HttpApplication UseModularization(this HttpApplication app,
            IServiceProvider serviceProvider)
        {
            var middleware = serviceProvider.GetService(typeof(ModularizationMiddleware));
            (middleware as ModularizationMiddleware)
                ?.InvokeAsync(app.Context)
                ?.Wait();

            return app;
        }

        #region Static ModuleTable Mode

        static ModularizationModuleTable moduleTable;
        static ModularizationMiddleware middleware;

        /// <summary>
        /// Add in Global.Application_Start (Static Mode)
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddModularization(this HttpApplication app)
        {
            moduleTable = new ModularizationModuleTable();
            middleware = new ModularizationMiddleware(moduleTable);

            return moduleTable;
        }

        /// <summary>
        /// Add in Global.Application_BeginRequest (Static Mode)
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static HttpApplication UseModularization(this HttpApplication app)
        {
            middleware.InvokeAsync(app.Context).Wait();

            return app;
        }

        #endregion
    }
}
