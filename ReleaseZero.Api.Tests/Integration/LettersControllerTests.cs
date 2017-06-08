using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace ReleaseZero.Api.Tests.Integration
{
    public class LettersControllerTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public LettersControllerTests()
        {
            _server = new TestServer(new WebHostBuilder()
                                     .UseStartup<Startup>());

            _client = _server.CreateClient();
        }

        [Fact(DisplayName = "Get() with no version returns v1")]
		public async Task ReturnLetterCollection_noversion()
		{
			var response = await _client.GetAsync("/api/letters");

			response.EnsureSuccessStatusCode();

            Assert.True(response.Headers.Contains("api-supported-versions"));

            var apiSupportedVersions = response.Headers.GetValues("api-supported-versions");

            Assert.Equal(1, apiSupportedVersions.Count());
            Assert.Equal("1.0", apiSupportedVersions.First());

			var responseString = await response.Content.ReadAsStringAsync();

			Assert.NotNull(responseString);
		}

        [Fact(DisplayName = "Get() with version in url returns v1")]
        public async Task ReturnLetterCollection_v1()
        {
            var response = await _client.GetAsync("/api/v1.0/letters");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.NotNull(responseString);
        }

		[Fact(DisplayName = "Get() with version 1 in querystring returns v1")]
		public async Task ReturnLetterCollection_v1_querystring()
		{
			var response = await _client.GetAsync("/api/letters?X-LK-API-VERSION=1");

			response.EnsureSuccessStatusCode();

			var responseString = await response.Content.ReadAsStringAsync();

			Assert.NotNull(responseString);
		}

		[Fact(DisplayName = "Get() with bad version returns BadRequest")]
		public async Task ReturnLetterCollection_v2()
		{
			var response = await _client.GetAsync("/api/v2.0/letters");

			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact(DisplayName = "Get() with version v2 in querystring returns BadRequest")]
		public async Task ReturnLetterCollection_v2_querystring()
		{
			var response = await _client.GetAsync("/api/letters?X-LK-API-VERSION=2");

			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}
    }
}
