using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

namespace Liyanjie.Modularization.Sample.AspNetCore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            static async Task<object> deserializeFromRequest(HttpRequest request, Type modelType)
            {
                using var streamReader = new System.IO.StreamReader(request.Body);
                var _request = await streamReader.ReadToEndAsync();
                return JsonConvert.DeserializeObject(_request, modelType);
            }
            static async Task serializeToResponse(HttpResponse response, object content)
            {
                response.StatusCode = 200;
                response.ContentType = "application/json";
                await response.WriteAsync(JsonConvert.SerializeObject(content));
            }

            services.AddModularization(deserializeFromRequest, serializeToResponse)
                //.AddModule<TModule,TModuleOptions>()
                ;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseModularization();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
