using System;
using System.Collections.Generic;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ModularizationModuleTable
    {
        readonly Dictionary<string, IDictionary<string, Type>> modules = new Dictionary<string, IDictionary<string, Type>>();

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyDictionary<string, IDictionary<string, Type>> Modules => modules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="middlewares"></param>
        /// <returns></returns>
        public ModularizationModuleTable AddModule(string name, IDictionary<string, Type> middlewares)
        {
            modules[name] = middlewares;

            return this;
        }
    }
}
