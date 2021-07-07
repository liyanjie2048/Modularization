using System;
using System.Web;

using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Modularization.Sample.AspNet
{
    public class Global : System.Web.HttpApplication
    {
        readonly IServiceCollection services = new ServiceCollection();
        IServiceProvider serviceProvider;
        protected void Application_Start(object sender, EventArgs e)
        {
            services.AddModularization()
                //.AddModule<>()
                ;

            serviceProvider = services.BuildServiceProvider();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var serviceProvider = this.serviceProvider.CreateScope().ServiceProvider;
            this.UseModularization(serviceProvider);
        }
    }
}