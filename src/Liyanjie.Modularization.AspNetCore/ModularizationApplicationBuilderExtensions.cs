using Liyanjie.Modularization.AspNetCore;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModularizationApplicationBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseModularization(this IApplicationBuilder app)
        {
            app.UseMiddleware<ModularizationMiddleware>();

            return app;
        }
    }
}
