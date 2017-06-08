using Newtonsoft.Json;

namespace ReleaseZero.Api.Models
{
    /// <summary>
    /// Validation error.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <value>The field.</value>
		[JsonProperty("field", NullValueHandling = NullValueHandling.Ignore)]
		public string Field { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        [JsonProperty("message")]
		public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ReleaseZero.Api.Models.ValidationError"/> class.
        /// </summary>
        /// <param name="field">Field.</param>
        /// <param name="message">Message.</param>
		public ValidationError(string field, string message)
		{
			Field = field != string.Empty ? field : null;
			Message = message;
		}
    }
}
