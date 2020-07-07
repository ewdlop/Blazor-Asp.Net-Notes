using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Xunit;

namespace BlazorServerApp.xUnitTests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient httpClient;

        public IntegrationTests(WebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateClient();
        }

        [Fact]
        public async System.Threading.Tasks.Task TestAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/Monsters");

            request.Content = new StringContent(JsonSerializer.Serialize(new {
                term = "MFA",
                definition = "An authentication process that considers multiple factors."
            }), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "An invalid token");

            // Act
            var response = await httpClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }


}
