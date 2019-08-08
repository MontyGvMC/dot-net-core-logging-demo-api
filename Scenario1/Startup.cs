using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCoreLoggingDemoAPI.Scenario1
{

    /// <summary>
    /// Our startup class.
    /// </summary>
    public class Startup
    {

        /// <summary>
        /// The configuration to be used.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="configuration">The configuration to be used.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <inheritdoc/>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwashbuckle();

        }
        /// <inheritdoc/>        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            // otherwise Swagger UI will display the HTML code of developer page in error cases
            //if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }

            app.UseSwashbuckle();

            app.UseMvc();
        }

    }

}
