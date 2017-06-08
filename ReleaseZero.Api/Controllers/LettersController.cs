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
    [Route("api/v{version:apiVersion}/letters")]
    [Route("api/letters")]
    [ValidateModel]
    public class LettersController : Controller
    {
        private readonly ILogger<LettersController> _logger;

        private readonly LettersContext _context;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReleaseZero.Api.Controllers.LetterController"/> class.
		/// </summary>
		/// <param name="logger">Logger to use, provided by constructor injection</param>
		/// <param name="context">Entity Framework context to use, provided by constructor injection</param>
		public LettersController(ILogger<LettersController> logger, LettersContext context)
        {
            _logger = logger;
            _context = context;
        }

		/// <summary>
		/// Gets all letter instances (NATO Phonetic alphabet)
		/// </summary>
		/// <returns>All 26 letter instances</returns>
		/// <response code="200">Returns all 26 letter instances</response>
		/// <response code="404">If there are no letter instances available</response>
		[HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Letter>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> Get()
        {
			var letters = await _context.Letters.ToListAsync();

			if (letters.Any())
			{
				return Ok(letters);
			}

			return NotFound();
        }

		/// <summary>
		/// Gets a specific letter in the collection
		/// </summary>
		/// <returns>The letter instance requested</returns>
		/// <param name="character">The character value of the requested letter instance</param>
		/// <response code="200">The specified letter object</response>
		/// <response code="404">The requested letter instance was not found</response>
		[HttpGet("{character}")]
		[ProducesResponseType(typeof(Letter), 200)]
		[ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> Get(char character)
        {
            var letter = await _context.Letters.FirstOrDefaultAsync(x => x.Character.Equals(character));

			if (letter != null)
			{
				return Ok(letter);
			}

			return NotFound();
        }

		/// <summary>
		/// Updates an existing letter object
		/// </summary>
		/// <returns>The updated letter object</returns>
		/// <param name="character">The character value of the requested letter instance</param>
		/// <param name="letterDocument">JsonPatchDocument to apply</param>
		/// <response code="200">Returns the updated letter object</response>
		/// <response code="404">The requested letter object was not found</response>
		[HttpPatch("{character}")]
        [ProducesResponseType(typeof(Letter), 200)]
		[ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> Patch(char character, [FromBody]JsonPatchDocument<Letter> letterDocument)
        {
			var letterToUpdate = await _context.Letters.SingleOrDefaultAsync(x => x.Character.Equals(character));

			if (letterToUpdate != null)
			{
				letterDocument.ApplyTo(letterToUpdate);

				_context.SaveChanges();

				return Ok(letterToUpdate);
			}

			return NotFound();
        }

        /// <summary>
        /// Creates a new letter
        /// </summary>
        /// <returns>The post.</returns>
        /// <param name="letter">letter.</param>
        /// <exception cref="T:System.NotImplementedException"></exception>
        [HttpPost]
        [ProducesResponseType(typeof(Letter), 201)]
        [ProducesResponseType(typeof(IEnumerable<Letter>), 400)]
        public async Task<IActionResult> Post([FromBody]Letter letter)
        {
            try
            {
                var newLetter = await _context.Letters.AddAsync(letter);

                _context.SaveChanges();

                return Ok(newLetter.Entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0), ex, ex.Message);

                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Put the specified id and value.
        /// </summary>
        /// <returns>The put.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="value">Value.</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        /// Delete the specified character.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="character">Character.</param>
        [HttpDelete("{character}")]
        public async Task<IActionResult> Delete(char character)
        {
			var letterToDelete = await _context.Letters.FirstOrDefaultAsync(x => x.Character.Equals(character));

			if (letterToDelete != null)
			{
				_context.Remove(letterToDelete);
				_context.SaveChanges();
			}

			return Ok();
        }
    }
}
