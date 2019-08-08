using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace DotNetCoreLoggingDemoAPI.Scenario1.Filters
{

    /// <summary>
    /// A filter producing logs in a centralized place. Can be applied globally.
    /// </summary>
    /// <remarks>
    /// The filter is created once per request (as far as I can figure that out) so caching works.
    /// </remarks>
    public class GlobalLoggingFilter : IActionFilter, IResultFilter, IExceptionFilter
    {

        /// <summary>
        /// The logger factory used to create the loggers.
        /// </summary>
        private readonly ILoggerFactory _loggerFactory;

        // evil work around (caching the may large action arguments)
        private object _actionArguments = null;

        /// <summary>
        /// Constructs an new instance.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public GlobalLoggingFilter(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Gets the name of the called action (Namespace(s).ControllerName.ActionName)
        /// </summary>
        /// <param name="actionDescriptor">The action descriptor holding the data.</param>
        /// <returns>Returns the log source matching the currently called action.</returns>
        private string LogSource(ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.DisplayName.Split(" ")[0];
        }

        /// <summary>
        /// Creates a logger for the currently called action.
        /// </summary>
        /// <param name="actionDescriptor">The action descriptor holding the data.</param>
        /// <returns>Returns a logger for the currently called action.</returns>
        private ILogger CreateDataLogger(ActionDescriptor actionDescriptor)
        {
            return _loggerFactory.CreateLogger(LogSource(actionDescriptor));
        }

        ///<inheritdoc/>
        public void OnActionExecuting(ActionExecutingContext context)
        {

            //_loggerFactory.CreateLogger<GlobalLoggingFilter>().LogDebug(nameof(OnActionExecuting));
            //CreateDataLogger(context.ActionDescriptor).LogDebug(nameof(OnActionExecuting));

            // here is the only place where is access to the action arguments
            // but we do not know if we need them later so let's cache them (yep: evil if large)
            _actionArguments = (context.ActionArguments.Count > 0) ? context.ActionArguments : null;
        }

        ///<inheritdoc/>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //_loggerFactory.CreateLogger<GlobalLoggingFilter>().LogDebug(nameof(OnActionExecuted));
            //CreateDataLogger(context.ActionDescriptor).LogDebug(nameof(OnActionExecuted));

            // checking for bad requests only works with context.ModelState.IsValid but not status codes
        }

        ///<inheritdoc/>
        public void OnResultExecuting(ResultExecutingContext context)
        {
            //_loggerFactory.CreateLogger<GlobalLoggingFilter>().LogDebug(nameof(OnResultExecuting));
            //CreateDataLogger(context.ActionDescriptor).LogDebug(nameof(OnResultExecuting));

            // if bad requests should be handled check with the model state here
            // the response is not yet executed and may not yet hold the status code 400
        }

        ///<inheritdoc/>
        public void OnResultExecuted(ResultExecutedContext context)
        {
            //_loggerFactory.CreateLogger<GlobalLoggingFilter>().LogDebug(nameof(OnResultExecuted));
            //CreateDataLogger(context.ActionDescriptor).LogDebug(nameof(OnResultExecuted));

            // if bad request results should be handled OnResultExecuted is the first place having the status code 400

            ////if (!context.ModelState.IsValid)
            //if (context.HttpContext.Response.StatusCode == 400)
            //{
            //}

            int statusCode = context.HttpContext.Response.StatusCode;

            LogLevel logLevel = LogLevel.None;
            string statusMessage = "";
            
            if (statusCode >= 200 && statusCode <= 299)
            {
                statusMessage = "success";
                logLevel = LogLevel.Information;
            }
            else if (statusCode >= 400 && statusCode <= 499)
            {
                statusMessage = "client error";
                logLevel = LogLevel.Warning;
            }
            else if (statusCode >= 500 && statusCode <= 599)
            {
                // developers can still catch exceptions on their own
                // and return status code 500 results
                statusMessage = "server error";
                logLevel = LogLevel.Error;
            }
            else
            {
                // well, do not log anything for now
                return;
            }


            if (_actionArguments != null)
            {
                statusMessage = statusMessage + Environment.NewLine;
                statusMessage = statusMessage + JsonConvert.SerializeObject(_actionArguments);
            }

            CreateDataLogger(context.ActionDescriptor)
                .Log(logLevel, statusCode, statusMessage);

        }

        ///<inheritdoc/>
        public void OnException(ExceptionContext context)
        {

            //_loggerFactory.CreateLogger<GlobalLoggingFilter>().LogDebug(nameof(OnException));
            //CreateDataLogger(context.ActionDescriptor).LogDebug(nameof(OnException));

            // if we have no data to log do not log anything
            if (_actionArguments != null)
            {
                CreateDataLogger(context.ActionDescriptor)
                .LogError
                (
                    //context.Exception, // there is the ExceptionLoggingMiddleware producing the stack trace logs
                    StatusCodes.Status500InternalServerError, // used for the event id
                    "an error occured with the data:"
                    + Environment.NewLine
                    + JsonConvert.SerializeObject(_actionArguments, Formatting.Indented)
                );

            }

        }

    }

}
