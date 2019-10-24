using System;
using System.Collections.Generic;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public interface IModularizationModule
    {
        /// <summary>
        /// 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        IReadOnlyDictionary<string, Type> Middlewares { get; }
    }
}
