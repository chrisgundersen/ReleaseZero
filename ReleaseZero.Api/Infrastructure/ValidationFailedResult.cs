using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ReleaseZero.Api.Models;

namespace ReleaseZero.Api.Infrastructure
{
    /// <summary>
    /// Validation failed result.
    /// </summary>
    public class ValidationFailedResult : ObjectResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ReleaseZero.Api.Infrastructure.ValidationFailedResult"/> class.
        /// </summary>
        /// <param name="modelState">Model state.</param>
		public ValidationFailedResult(ModelStateDictionary modelState)
		: base(new ValidationResultModel(modelState))
		{
			StatusCode = StatusCodes.Status422UnprocessableEntity;
		}
    }
}
