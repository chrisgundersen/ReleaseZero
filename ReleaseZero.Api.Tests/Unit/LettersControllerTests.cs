using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ReleaseZero.Api.Controllers;
using ReleaseZero.Api.Infrastructure;
using ReleaseZero.Api.Models;
using Xunit;

namespace ReleaseZero.Api.Tests.Unit
{
    public class FooControllerTests
    {
        private readonly ILogger<LettersController> _logger = new Mock<ILogger<LettersController>>().Object;

        [Fact(DisplayName = "Get() method in FooController returns phonetic alphabet")]
        public async Task GetWithoutParamsReturnsAllLetters()
        {
            using (var context = GetFooContextWithData())
            {
                using (var controller = new LettersController(_logger, context))
                {
                    var result = await controller.Get() as OkObjectResult;

                    Assert.NotNull(result);

                    var list = result.Value as List<Letter>;

                    Assert.NotNull(list);

                    Assert.Equal(26, list.Count);
                }
            }
        }

        [Theory(DisplayName = "Get() given a valid id returns a letter")]
        [InlineData(1, "Alpha")]
        [InlineData(5, "Echo")]
        [InlineData(13, "Mancy")]
        [InlineData(19, "Sierra")]
        [InlineData(26, "Zulu")]
        public async Task GetWithValidParamReturnsOneLetter(int id, string name)
        {
            using (var context = GetFooContextWithData())
            {
                using (var controller = new LettersController(_logger, context))
                {
					var result = await controller.Get(id) as OkObjectResult;

					Assert.NotNull(result);

					var foo = result.Value as Letter;

					Assert.NotNull(foo);

					Assert.Equal(name, foo.Telephony);
                }
            }
        }

		[Theory(DisplayName = "Get() given an invalid id returns NotFound")]
		[InlineData(-1)]
		[InlineData(0)]
		[InlineData(27)]
		[InlineData(42)]
		[InlineData(Int32.MinValue)]
        [InlineData(Int32.MaxValue)]
		public async Task GetWithInvalidParamReturnsNotFound(int id)
		{
			using (var context = GetFooContextWithData())
			{
				using (var controller = new LettersController(_logger, context))
				{
					var result = await controller.Get(id) as NotFoundResult;

					Assert.NotNull(result);
				}
			}
		}

        private LettersContext GetFooContextWithData()
        {
            var options = new DbContextOptionsBuilder<LettersContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LettersContext(options);

            context.Letters.AddRange(Helpers.GetLetterArray());

            context.SaveChanges();

            return context;
        }
    }
}
