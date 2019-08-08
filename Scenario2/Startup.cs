using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;

namespace DotNetCoreLoggingDemoAPI.Scenario2
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

            //this is the service configuration for the solution with the two global filters in the correct order
            services
                .AddMvc
                (
                    mvcOptions =>
                    {
                        mvcOptions.Filters.Add<Filters.GlobalLoggingFilter>(1);
                        mvcOptions.Filters.Add<Filters.BadRequestFilter>(2);
                    }
                )
                .ConfigureApiBehaviorOptions
                (
                    apiBehaviorOptions =>
                    {
                        apiBehaviorOptions.SuppressModelStateInvalidFilter = true;
                    }
                )
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // this is the service configuration for the approach of logging within the InvalidModelStateResponseFactory
            //services
            //    .AddMvc
            //    (
            //        mvcOptions =>
            //        {
            //            mvcOptions.Filters.Add<Filters.GlobalLoggingFilter>();
            //        }
            //    )
            //    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //services.PostConfigure<ApiBehaviorOptions>(options =>
            //{
            //    //TODO does this respect the settings regarding the configured response model?
            //    //     seems to be different between the two approaches

            //    var builtInFactory = options.InvalidModelStateResponseFactory;

            //    options.InvalidModelStateResponseFactory = context =>
            //    {

            //        var logger = context.HttpContext.RequestServices
            //            .GetRequiredService<ILoggerFactory>()
            //            .CreateLogger(context.ActionDescriptor.DisplayName.Split(" ")[0]);

            //        logger.LogWarning
            //        (
            //            StatusCodes.Status400BadRequest,
            //            JsonConvert.SerializeObject
            //            (
            //                new
            //                {
            //                    Arguments = "null", //TODO still need to figure out how to get the model data causing the problems
            //                    ModelStateErrors = context.ModelState
            //                        .Where(kvp => kvp.Value.Errors.Count > 0)
            //                        .ToDictionary
            //                        (
            //                            kvp => kvp.Key,
            //                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            //                        )
            //                },
            //                Formatting.Indented
            //            )
            //        );

            //        return builtInFactory(context);
            //    };
            //});

            services.AddSwashbuckle();

        }

        /// <inheritdoc/>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            // otherwise Swagger UI will display the HTML code of developer page in error cases
            //if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }

            app.UseMiddleware<Middleware.ExceptionHandlerMiddleware>(!env.IsDevelopment());
            app.UseMiddleware<Middleware.ExceptionLoggingMiddleware>();

            app.UseSwashbuckle();

            app.UseMvc();
        }

    }

}
