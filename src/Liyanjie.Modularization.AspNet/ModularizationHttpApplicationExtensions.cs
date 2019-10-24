using System.Threading.Tasks;

using Liyanjie.Modularization.AspNet;

namespace System.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModularizationHttpApplicationExtensions
    {
        /// <summary>
        /// Add in Global.Application_Start.(Use DI)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="serviceRegister"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddModularization(this HttpApplication app,
            Action<object, string> serviceRegister)
        {

            var moduleTable = new ModularizationModuleTable(serviceRegister);
            serviceRegister.Invoke(moduleTable, "Singleton");

            return moduleTable;
        }

        /// <summary>
        /// Add in Global.Application_BeginRequest.(Use DI)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static HttpApplication UseModularization(this HttpApplication app,
            IServiceProvider serviceProvider)
        {
            new ModularizationMiddleware(serviceProvider).Invoke(app.Context).Wait();

            return app;
        }

        #region Static ModuleTable Mode

        static ModularizationModuleTable moduleTable;

        /// <summary>
        /// Add in Global.Application_Start.(Static ModuleTable)
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddModularization(this HttpApplication app)
        {
            moduleTable = new ModularizationModuleTable();

            return moduleTable;
        }

        /// <summary>
        /// Add in Global.Application_BeginRequest.(Static ModuleTable)
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static HttpApplication UseModularization(this HttpApplication app)
        {
            new ModularizationMiddleware(moduleTable).Invoke(app.Context).Wait();

            return app;
        }

        #endregion
    }
}
