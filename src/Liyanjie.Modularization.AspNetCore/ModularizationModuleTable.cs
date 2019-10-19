using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class ModularizationModuleTable
    {
        internal readonly IList<Type> ModuleTypes = new List<Type>();
        readonly IServiceCollection services;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public ModularizationModuleTable(IServiceCollection services)
        {
            this.services = services;
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
            ModuleTypes.Add(typeof(TModule));

            return this;
        }
    }
}
