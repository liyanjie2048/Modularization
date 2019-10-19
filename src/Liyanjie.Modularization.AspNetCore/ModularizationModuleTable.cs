using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ModularizationModuleTable
    {
        readonly IServiceCollection services;
        readonly IList<Type> moduleTypes = new List<Type>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public ModularizationModuleTable(IServiceCollection services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<Type> ModuleTypes => new ReadOnlyCollection<Type>(moduleTypes);

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
            where TModuleOptions : class
        {
            if (configureOptions != null)
                services.Configure(configureOptions);

            moduleTypes.Add(typeof(TModule));

            return this;
        }
    }
}
