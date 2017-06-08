using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ReleaseZero.Api.Infrastructure
{
    /// <summary>
    /// Error handling middleware.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
		private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ReleaseZero.Api.Infrastructure.ErrorHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">Next.</param>
        /// <param name="logger">Logger.</param>
		public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
		{
			_next = next;
            _logger = logger;
		}

        /// <summary>
        /// Invoke the specified context.
        /// </summary>
        /// <returns>The invoke.</returns>
        /// <param name="context">Context.</param>
		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
                _logger.LogError(new EventId(0), ex, ex.Message);

				await HandleExceptionAsync(context, ex);
			}
		}

        /// <summary>
        /// Handles the exception async.
        /// </summary>
        /// <returns>The exception async.</returns>
        /// <param name="context">Context.</param>
        /// <param name="exception">Exception.</param>
		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			var code = HttpStatusCode.InternalServerError; // 500 if unexpected

			//if (exception is MyNotFoundException) code = HttpStatusCode.NotFound;
			//else if (exception is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
			//else if (exception is MyException) code = HttpStatusCode.BadRequest;

			var result = JsonConvert.SerializeObject(new { error = exception.Message });
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)code;
			return context.Response.WriteAsync(result);
		}
    }
}
