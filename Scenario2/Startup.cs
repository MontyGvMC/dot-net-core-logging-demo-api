using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

                        //https://docs.microsoft.com/en-us/aspnet/core/web-api/index?view=aspnetcore-2.2#customize-badrequest-response
                        //apiBehaviorOptions.InvalidModelStateResponseFactory = context =>
                        //{

                        //    // log bad requests in here???

                        //    var problemDetails = new ValidationProblemDetails(context.ModelState)
                        //    {
                        //        Type = "https://contoso.com/probs/modelvalidation",
                        //        Title = "One or more model validation errors occurred.",
                        //        Status = Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest,
                        //        Detail = "See the errors property for details.",
                        //        Instance = context.HttpContext.Request.Path
                        //    };

                        //    return new BadRequestObjectResult(problemDetails)
                        //    {
                        //        ContentTypes = { "application/problem+json" }
                        //    };
                        //};
                    }
                )
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwashbuckle();

        }
        /// <inheritdoc/>        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
