using System.Threading.Tasks;

using Liyanjie.Modularization.AspNet;

namespace System.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModularizationHttpApplicationExtensions
    {
        static ModularizationModuleTable moduleTable;

        /// <summary>
        /// Add in Global.Application_Start
        /// </summary>
        /// <param name="app"></param>
        /// <param name="deserializeFromRequest"></param>
        /// <param name="serializeToResponse"></param>
        /// <returns></returns>
        /// <example>
        /// protected void Application_Start(object sender, EventArgs e)
        /// {
        ///    this.AddModularization(serializeToResponse, deserializeFromRequest);
        /// }
        /// </example>
        public static ModularizationModuleTable AddModularization(this HttpApplication app,
            Func<HttpRequest, Type, Task<object>> deserializeFromRequest,
            Func<HttpResponse, object, Task> serializeToResponse)
        {
            ModularizationDefaults.DeserializeFromRequestAsync = deserializeFromRequest ?? throw new ArgumentNullException(nameof(deserializeFromRequest));
            ModularizationDefaults.SerializeToResponseAsync = serializeToResponse ?? throw new ArgumentNullException(nameof(serializeToResponse));

            moduleTable = new ModularizationModuleTable();

            return moduleTable;
        }

        /// <summary>
        /// Add in Global.Application_BeginRequest
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        /// <example>
        /// protected void Application_BeginRequest(object sender, EventArgs e)
        /// {
        ///    this.UseModularization();
        /// }
        /// </example>
        public static HttpApplication UseModularization(this HttpApplication app)
        {
            new ModularizationMiddleware(moduleTable).Invoke(app.Context).Wait();
            
            return app;
        }
    }
}
