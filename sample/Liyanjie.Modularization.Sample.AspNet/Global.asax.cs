using System;
using System.Threading.Tasks;
using System.Web;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

namespace Liyanjie.Modularization.Sample.AspNet
{
    public class Global : System.Web.HttpApplication
    {
        static readonly IServiceCollection services = new ServiceCollection();

        protected void Application_Start(object sender, EventArgs e)
        {
            #region Use DI
            static void registerServiceType(
                Type type,
                string lifeTime)
                => _ = lifeTime.ToLower() switch
                {
                    "singleton" => services.AddSingleton(type),
                    "scoped" => services.AddScoped(type),
                    "transient" => services.AddTransient(type),
                    _ => services,
                };
            static void registerServiceInstance(
                Type type, 
                Func<IServiceProvider, object> registerServiceImplementationFactory,
                string lifeTime)
                => _ = lifeTime.ToLower() switch
                {
                    "singleton" => services.AddSingleton(type, registerServiceImplementationFactory),
                    "scoped" => services.AddScoped(type, registerServiceImplementationFactory),
                    "transient" => services.AddTransient(type, registerServiceImplementationFactory),
                    _ => services,
                };
            this.AddModularization(registerServiceType, registerServiceImplementationFactory)
                //.AddModule<TModule,TModuleOptions>()
                ;
            #endregion
            #region Static
            //this.AddModularization(deserializeFromRequest, serializeToResponse)
            //    //.AddModule<TModule,TModuleOptions>()
            //    ;
            #endregion
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            #region Use DI
            this.UseModularization(services.BuildServiceProvider());
            #endregion
            #region Static
            //this.UseModularization();
            #endregion
        }
    }
}