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
        readonly Action<Type, Func<IServiceProvider, object>, string> registerServiceImplementationFactory;
        readonly Dictionary<string, ModularizationModuleMiddleware[]> modules = new();
        readonly Dictionary<string, object> moduleOptions = new();

        /// <summary>
        /// 
        /// </summary>
        public ModularizationModuleTable() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registerServiceType"></param>
        /// <param name="registerServiceImplementationFactory"></param>
        public ModularizationModuleTable(
            Action<Type, string> registerServiceType,
            Action<Type, Func<IServiceProvider, object>, string> registerServiceImplementationFactory)
        {
            this.registerServiceType = registerServiceType;
            this.registerServiceImplementationFactory = registerServiceImplementationFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<Type, string> RegisterServiceType => registerServiceType;

        /// <summary>
        /// 
        /// </summary>
        public Action<Type, Func<IServiceProvider, object>, string> RegisterServiceImplementationFactory => registerServiceImplementationFactory;

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

                if (RegisterServiceImplementationFactory != null)
                    RegisterServiceImplementationFactory.Invoke(typeof(TModuleOptions), sp => options, "Singleton");
                else
                    moduleOptions[moduleName] = options;
            }
            return this;
        }
    }
}
