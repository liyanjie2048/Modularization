using System;

namespace Liyanjie.Modularization
{
    /// <summary>
    /// 
    /// </summary>
    public struct ModuleMiddleware
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
        public Type HandlerType { get; set; }
    }
}
