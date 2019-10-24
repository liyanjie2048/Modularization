using System;
using System.Collections.Generic;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ModularizationModuleTable
    {
        readonly Action<Type, string> registerServiceType;
        readonly Action<Func<IServiceProvider, object>, string> registerServiceInstance;
        readonly Dictionary<string, ModularizationModuleMiddleware[]> modules = new Dictionary<string, ModularizationModuleMiddleware[]>();
        readonly Dictionary<string, object> moduleOptions = new Dictionary<string, object>();

        /// <summary>
        /// 
        /// </summary>
        public ModularizationModuleTable() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registerServiceType"></param>
        /// <param name="registerServiceInstance"></param>
        public ModularizationModuleTable(
            Action<Type, string> registerServiceType,
            Action<Func<IServiceProvider, object>, string> registerServiceInstance)
        {
            this.registerServiceType = registerServiceType;
            this.registerServiceInstance = registerServiceInstance;
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<Type, string> RegisterServiceType => registerServiceType;

        /// <summary>
        /// 
        /// </summary>
        public Action<Func<IServiceProvider, object>, string> RegisterServiceInstance => registerServiceInstance;

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyDictionary<string, ModularizationModuleMiddleware[]> Modules => modules;

        /// <summary>
        /// 
        /// </summary>
        internal IReadOnlyDictionary<string, object> ModuleOptions => moduleOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="moduleMiddlewares"></param>
        /// <param name="configureModuleOptions"></param>
        /// <returns></returns>
        public ModularizationModuleTable AddModule<TModuleOptions>(
            string moduleName,
            ModularizationModuleMiddleware[] moduleMiddlewares,
            Action<TModuleOptions> configureModuleOptions = null)
            where TModuleOptions : class, new()
        {
            modules[moduleName] = moduleMiddlewares;
            if (configureModuleOptions != null)
            {
                var options = new TModuleOptions();
                configureModuleOptions.Invoke(options);

                if (registerServiceInstance != null)
                    registerServiceInstance.Invoke(sp => options, "Singleton");
                else
                    moduleOptions[moduleName] = options;
            }
            return this;
        }
    }
}
