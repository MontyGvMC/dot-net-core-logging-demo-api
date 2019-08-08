using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// Extension methods for integrating Swashbuckle.
    /// </summary>
    public static class SwashbuckleIntegration
    {

        /// <summary>
        /// Adds the Swashbuckle related services (add Swashbuckle after MVC).
        /// </summary>
        /// <param name="services">The service collection to be extended.</param>
        /// <returns>Returns the extended service collection.</returns>
        public static IServiceCollection AddSwashbuckle(this IServiceCollection services)
        {

            // in the project properties -> Build -> Output -> check Build XML documentation
            string xmlPath = Path.Combine(AppContext.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name + ".xml");

            if (!File.Exists(xmlPath)) throw new FileNotFoundException("XML documentation not found (in the project properties -> build -> enable XML documentation)", xmlPath);

            services.AddSwaggerGen
            (
                options =>
                {

                    options.IncludeXmlComments(xmlPath, true);

                    options.DescribeAllEnumsAsStrings();

                    options.SwaggerDoc
                    (
                        "v1",
                        new Info
                        {
                            Version = "v1",
                            Title = ".NET Core Logging Demo API: Scenario 0",
                            Description = "A playground for .NET Core logging experiments."
                        }
                    );
                }
            );

            return services;
        }

        /// <summary>
        /// Adds the Swashbuckle related services to the pipeline (use Swashbuckle before MVC).
        /// </summary>
        /// <param name="app">The application builder to be extended.</param>
        /// <returns>Returns the extended application builder.</returns>
        public static IApplicationBuilder UseSwashbuckle(this IApplicationBuilder app)
        {

            app.UseSwagger();

            // in the project properties -> Debug -> Check launch browser and type in "swagger" instead of "api/values"
            // which will automatically start with the Swagger UI
            app.UseSwaggerUI
            (
                options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                }
            );

            return app;
        }

    }

}
