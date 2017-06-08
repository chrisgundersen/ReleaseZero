using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace ReleaseZero.Api.Models
{
    /// <summary>
    /// Validation result model.
    /// </summary>
    public class ValidationResultModel
    {
        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        [JsonProperty("message")]
		public string Message { get; }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>The errors.</value>
        [JsonProperty("errors")]
		public List<ValidationError> Errors { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ReleaseZero.Api.Models.ValidationResultModel"/> class.
        /// </summary>
        /// <param name="modelState">Model state.</param>
		public ValidationResultModel(ModelStateDictionary modelState)
		{
			Message = "Validation Failed";
			Errors = modelState.Keys
					.SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
					.ToList();
		}
    }
}
