using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReleaseZero.Api.Infrastructure;
using ReleaseZero.Api.Models;

namespace ReleaseZero.Api.Controllers
{
    /// <summary>
    /// Controller for testing purposes. Doesn't do anything terribly useful.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/foo")]
    //[Route("api/foo")]
    public class FooController : Controller
    {
        private readonly ILogger<FooController> _logger;

        private readonly FooContext _context;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReleaseZero.Api.Controllers.FooController"/> class.
		/// </summary>
		/// <param name="logger">Logger to use, provided by constructor injection</param>
		/// <param name="context">Entity Framework context to use, provided by constructor injection</param>
		public FooController(ILogger<FooController> logger, FooContext context)
        {
            _logger = logger;
            _context = context;
        }

		/// <summary>
		/// Gets all foo instances (NATO Phonetic alphabet)
		/// </summary>
		/// <returns>All 26 Foo instances</returns>
		/// <response code="200">Returns all 26 Foo instances</response>
		/// <response code="400">An error occurred</response>
		/// <response code="404">If there are no Foo instances available</response>
		[HttpGet]
        [ProducesResponseType(typeof(List<Foo>), 200)]
        [ProducesResponseType(typeof(List<Foo>), 400)]
        [ProducesResponseType(typeof(List<Foo>), 404)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var foos = await _context.Foos.ToListAsync();

                if (foos.Any())
                {
                    return Ok(foos);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, ex.Message);

                return BadRequest();
            }
        }

		/// <summary>
		/// Gets a specific letter in the collection
		/// </summary>
		/// <returns>The Foo instance requested</returns>
		/// <param name="id">The ID value of the requested Foo instance</param>
		/// <response code="200">The specified Foo object</response>
		/// <response code="400">An error occurred</response>
		/// <response code="404">The requested Foo instance was not found</response>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(IEnumerable<Foo>), 200)]
		[ProducesResponseType(typeof(IEnumerable<Foo>), 400)]
		[ProducesResponseType(typeof(IEnumerable<Foo>), 404)]
        public async Task<IActionResult> Get(int id)
        {
			try
			{
                var foo = await _context.Foos.FirstOrDefaultAsync(x => x.Id == id);

                if (foo != null){
                    return Ok(foo);
                }

				return NotFound();
			}
			catch (Exception ex)
			{
				_logger.LogError(0, ex, ex.Message);

				return BadRequest();
			}
        }

		/// <summary>
		/// Updates an existing Foo object
		/// </summary>
		/// <returns>The updated Foo object</returns>
		/// <param name="id">The ID value of the requested Foo instance</param>
		/// <param name="fooDocument">JsonPatchDocument to apply</param>
		/// <response code="200">Returns the updated Foo object</response>
		/// <response code="400">An error occurred</response>
		/// <response code="404">The requested Foo object was not found</response>
		[HttpPatch("{id}")]
        [ProducesResponseType(typeof(IEnumerable<Foo>), 200)]
        [ProducesResponseType(typeof(IEnumerable<Foo>), 400)]
		[ProducesResponseType(typeof(IEnumerable<Foo>), 404)]
        public async Task<IActionResult> Patch(int id, [FromBody]JsonPatchDocument<Foo> fooDocument)
        {
            try
            {
                var fooToUpdate = await _context.Foos.SingleOrDefaultAsync(x => x.Id == id);

                if (fooToUpdate != null)
                {
                    fooDocument.ApplyTo(fooToUpdate);

                    _context.SaveChanges();

                    return Ok(fooToUpdate);
                }

                return NotFound();
            }
			catch (Exception ex)
			{
				_logger.LogError(0, ex, ex.Message);

				return BadRequest();
			}
        }

        /// <summary>
        /// Creates a new Foo
        /// </summary>
        /// <returns>The post.</returns>
        /// <param name="foo">Foo.</param>
        [HttpPost]
        [ProducesResponseType(typeof(Foo), 201)]
        [ProducesResponseType(typeof(IEnumerable<Foo>), 400)]
        public async Task<IActionResult> Post([FromBody]Foo foo)
        {
            try
            {
                if (foo.Id < 1 || foo.Id > 26)
                {
                    return Res
                }
            }
			catch (Exception ex)
			{
				_logger.LogError(0, ex, ex.Message);

				return BadRequest();
			}
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var fooToDelete = await _context.Foos.FirstOrDefaultAsync(x => x.Id == id);

                if (fooToDelete != null)
                {
                    _context.Remove(fooToDelete);
                    _context.SaveChanges();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, ex.Message);

                return BadRequest();
            }
        }
    }
}
