using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ModularizationModuleTable
    {
        readonly Action<object, string> serviceRegister;
        readonly IDictionary<Type, object> moduleTypes = new Dictionary<Type, object>();

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
        public IReadOnlyList<Type> ModuleTypes => new ReadOnlyCollection<Type>(moduleTypes.Keys.ToList());

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <returns></returns>
        public ModularizationModuleTable AddModule<TModule>()
            where TModule : class, IModularizationModule
        {
            AddModule<TModule, object>(null);

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <typeparam name="TModuleOptions"></typeparam>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public ModularizationModuleTable AddModule<TModule, TModuleOptions>(Action<TModuleOptions> configureOptions)
            where TModule : class, IModularizationModule
            where TModuleOptions : class, new()
        {
            TModuleOptions options = null;
            if (configureOptions != null)
            {
                options = new TModuleOptions();
                configureOptions.Invoke(options);
            }

            if (serviceRegister == null)
                moduleTypes.Add(typeof(TModule), options);
            else
            {
                serviceRegister.Invoke(options, "Singleton");
                moduleTypes.Add(typeof(TModule), null);
            }

            return this;
        }

        internal bool TryGetOptions(Type moduleType, out object options)
            => moduleTypes.TryGetValue(moduleType, out options);
    }
}
