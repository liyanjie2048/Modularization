using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class ModularizationModuleTable
    {
        readonly IDictionary<Type, object> modules = new Dictionary<Type, object>();

        internal ReadOnlyDictionary<Type, object> Modules => new ReadOnlyDictionary<Type, object>(modules);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <typeparam name="TModuleOptions"></typeparam>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public ModularizationModuleTable AddModule<TModule, TModuleOptions>(Action<TModuleOptions> configureOptions)
            where TModule : class, IModularizationModule
            where TModuleOptions : class
        {
            var options = (TModuleOptions)Activator.CreateInstance(typeof(TModuleOptions));
            configureOptions?.Invoke(options);
            modules.Add(typeof(TModule), options);

            return this;
        }
    }
}
