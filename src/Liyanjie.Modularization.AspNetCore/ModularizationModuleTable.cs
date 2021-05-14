using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ModularizationModuleTable
    {
        readonly IServiceCollection services;
        readonly Dictionary<string, ModularizationModuleMiddleware[]> modules = new();

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
        public IServiceCollection Services => services;

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyDictionary<string, ModularizationModuleMiddleware[]> Modules => modules;

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
            where TModuleOptions : class
        {
            modules[moduleName] = moduleMiddlewares;

            if (configureModuleOptions != null)
                services.Configure(configureModuleOptions);

            return this;
        }
    }
}
