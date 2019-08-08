using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace DotNetCoreLoggingDemoAPI.Scenario1.Middleware
{

    /// <summary>
    /// Middleware converting uncaught exceptions in responses with status code 500.
    /// </summary>
    /// <remarks>
    /// As I try to follow the seperation of concerns this handler middleware just produces the responses but does not do logging.
    /// </remarks>
    public class ExceptionHandlerMiddleware
    {

        /// <summary>
        /// The next middleware in the pipeline.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Indicates that the error response should be empty (e.g. for production).
        /// </summary>
        private readonly bool _emptyResponse;

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="emptyResponse">Indicates that the error reponse should be empty.</param>
        public ExceptionHandlerMiddleware(RequestDelegate next, bool emptyResponse)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _emptyResponse = emptyResponse;
        }

        /// <summary>
        /// It's pipeline activity.
        /// </summary>
        /// <param name="httpContext">The HTTP context to be used.</param>
        /// <returns>Returns a task.</returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {

            try
            {

                await _next(httpContext);

            }
            catch (Exception ex)
            {

                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                if (_emptyResponse)
                {
                    httpContext.Response.ContentType = null; // clear the content type
                    await httpContext.Response.WriteAsync(string.Empty);
                }
                else
                {
                    httpContext.Response.ContentType = "text/plain";
                    await httpContext.Response.WriteAsync(ex.ToString());
                }

            }

        }

    }

}
