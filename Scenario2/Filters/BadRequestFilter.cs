using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetCoreLoggingDemoAPI.Scenario2.Filters
{

    /// <summary>
    /// Filter checking the model state and returning a bad request if the model state is not valid.
    /// </summary>
    /// <remarks>
    /// When using the [ApiController] we would not need that.
    /// But then GlobalLogFilter does not log the model bound data causing the bad request.
    /// 
    /// In the Stattup we can configure this to not return the BadRequest with 
    /// <code>
    ///     apiBehaviorOptions.SuppressModelStateInvalidFilter = true;
    /// </code>
    /// 
    /// This leaves us with checking the model state again in each controller method.
    /// 
    /// Or using this dedicated filter doing the same thing but after the GlobalLogFilter had it's chance to grab the data.
    /// <code>
    ///     mvcOptions.Filters.Add(typeof(Filters.GlobalLoggingFilter), 1);
    ///     mvcOptions.Filters.Add(typeof(Filters.BadRequestFilter), 2);
    /// </code>
    /// 
    /// </remarks>
    public class BadRequestFilter : IActionFilter
    {

        ///<inheritdoc/>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        ///<inheritdoc/>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // intentionally not used for now
        }
        
    }

}
