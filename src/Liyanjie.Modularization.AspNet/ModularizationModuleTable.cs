using System;
using System.Collections.Generic;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ModularizationModuleTable
    {
        readonly Action<object, string> serviceRegister;
        readonly Dictionary<string, IDictionary<string, Type>> modules = new Dictionary<string, IDictionary<string, Type>>();
        readonly Dictionary<string, object> moduleOptions = new Dictionary<string, object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceRegister"></param>
        public ModularizationModuleTable(Action<object, string> serviceRegister = null)
        {
            this.serviceRegister = serviceRegister;
        }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyDictionary<string, IDictionary<string, Type>> Modules => modules;

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
            IDictionary<string, Type> moduleMiddlewares,
            Action<TModuleOptions> configureModuleOptions = null)
            where TModuleOptions : class, new()
        {
            modules[moduleName] = moduleMiddlewares;
            if (configureModuleOptions != null)
            {
                var options = new TModuleOptions();
                configureModuleOptions.Invoke(options);

                if (serviceRegister != null)
                    serviceRegister.Invoke(options, "Singleton");
                else
                    moduleOptions[moduleName] = options;
            }
            return this;
        }
    }
}
