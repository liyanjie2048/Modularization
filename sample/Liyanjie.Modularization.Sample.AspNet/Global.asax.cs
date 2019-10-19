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
            static async Task<object> deserializeFromRequest(HttpRequest request, Type modelType)
            {
                await Task.FromResult(0);
                using var streamReader = new System.IO.StreamReader(request.InputStream);
                var _request = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject(_request, modelType);
            }
            static async Task serializeToResponse(HttpResponse response, object content)
            {
                await Task.FromResult(0);
                response.StatusCode = 200;
                response.ContentType = "application/json";
                response.Write(JsonConvert.SerializeObject(content));
            }

            #region Use DI
            void serviceRegister(object instance, string lifeTime)
                => _ = lifeTime.ToLower() switch
                {
                    "singleton" => services.AddSingleton(instance.GetType(), sp => instance),
                    "scoped" => services.AddScoped(instance.GetType(), sp => instance),
                    "transient" => services.AddTransient(instance.GetType(), sp => instance),
                    _ => services,
                };
            this.AddModularization(serviceRegister, deserializeFromRequest, serializeToResponse)
                //.AddModule<TModule,TModuleOptions>()
                ;
            #endregion
            #region Static
            this.AddModularization(deserializeFromRequest, serializeToResponse)
                //.AddModule<TModule,TModuleOptions>()
                ;
            #endregion
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            #region Use DI
            this.UseModularization(services.BuildServiceProvider());
            #endregion
            #region Static
            this.UseModularization();
            #endregion
        }
    }
}