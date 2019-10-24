using System;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public struct ModularizationModuleMiddleware
    {
        /// <summary>
        /// 
        /// </summary>
        public string[] HttpMethods { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RouteTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Type Type { get; set; }
    }
}
