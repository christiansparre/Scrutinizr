using System.Collections.Generic;
using System.IO;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace Scrutinizr
{
    public class Startup
    {
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment hostingEnvironment, IApplicationEnvironment appEnvironment)
        {
            var dataDirectory = Path.Combine(appEnvironment.ApplicationBasePath, "data");
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            var dataPath = Path.Combine(dataDirectory, "data.db");
            
            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(new[] { new KeyValuePair<string, string>("Database", dataPath) })
                .Build();

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(s => _config);

            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseIISPlatformHandler();
            app.UseDeveloperExceptionPage();
            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
