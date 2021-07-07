using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Modularization
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ModuleTable
    {
        readonly IServiceCollection services;
        readonly Dictionary<string, ModuleMiddleware[]> modules = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public ModuleTable(IServiceCollection services)
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
        public IReadOnlyDictionary<string, ModuleMiddleware[]> Modules => modules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="moduleMiddlewares"></param>
        /// <param name="configureModuleOptions"></param>
        /// <returns></returns>
        public ModuleTable AddModule<TModuleOptions>(
            string moduleName,
            ModuleMiddleware[] moduleMiddlewares,
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
