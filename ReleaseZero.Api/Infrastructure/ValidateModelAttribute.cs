using Microsoft.AspNetCore.Mvc.Filters;

namespace ReleaseZero.Api.Infrastructure
{
    /// <summary>
    /// Validate model attribute.
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Ons the action executing.
        /// </summary>
        /// <param name="context">Context.</param>
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
				context.Result = new ValidationFailedResult(context.ModelState);
			}
		}
    }
}
