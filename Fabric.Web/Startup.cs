using Fabric.Web.Hubs;
using Fabric.Web.OnStart;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fabric.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.ConfigureMyAppServices(Configuration);
            services.AddMvc();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"))
                         .AddDebug();

            app.UseDeveloperExceptionPage();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
            }

            app.UseDefaultFiles()
               .UseStaticFiles()
               .UseSignalR(routes =>
               {
                   routes.MapHub<OrleansHub>("/hub");
               })
               .UseMvc(routes =>
               {
                   routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
               });
        }
    }
}
