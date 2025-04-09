using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.DbContexts;
using WebApi.Contracts.Requests.Companies;
using WebApi.Contracts.Responses.Companies;

namespace WebApiTest.Tests
{
    public class AuthorizeControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
        private const string RequestUri = "/api/authorize";

        public AuthorizeControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            EnsureRootUserSeeded();
            _client = factory.CreateClient();
        }
        
        private void EnsureRootUserSeeded()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
            var userExists = dbContext.Users.Any(u => u.Name == "root");
            if (!userExists)
            {
                throw new InvalidOperationException("Seeding failed: 'root' user must be present for AuthorizeControllerTests.");
            }
        }


        [Fact]
        public async Task Authorize_WithValidCredentials_ReturnsOkAndToken()
        {
            // Arrange
            var request = new AuthUserRequest
            {
                Login = "root",
                Password = "Root000!"
            };

            // Act
            var response = await _client.PostAsJsonAsync(RequestUri, request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var authResponse = await response.Content.ReadFromJsonAsync<AuthUserResponse>();
            authResponse.Should().NotBeNull();
            authResponse!.Token.Data.Should().NotBeNullOrEmpty();
            authResponse.Token.Data.Should().Contain(".");
            authResponse.UserId.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Authorize_WithInvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            var request = new AuthUserRequest
            {
                Login = "root",
                Password = "WrongPassword123!" 
            };

            // Act && Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                await _client.PostAsJsonAsync(RequestUri, request);
            });
        }

        [Fact]
        public async Task Authorize_WithNonExistentUser_ReturnsUnauthorized()
        {
            // Arrange
            var request = new AuthUserRequest
            {
                Login = "nonexistentuser",
                Password = "SomePassword123!"
            };

            // Act && Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                await _client.PostAsJsonAsync(RequestUri, request);
            });
        }

        [Fact]
        public async Task Authorize_WithMissingUsername_ReturnsBadRequest()
        {
            // Arrange
            var request = new AuthUserRequest
            {
                Login = null,
                Password = "Root000!"
            };

            // Act
            var response = await _client.PostAsJsonAsync(RequestUri, request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Authorize_WithMissingPassword_ReturnsBadRequest()
        {
            // Arrange
            var request = new AuthUserRequest
            {
                Login = "root",
                Password = null
            };

            // Act
            var response = await _client.PostAsJsonAsync(RequestUri, request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

         [Fact]
        public async Task Authorize_WithEmptyBody_ReturnsBadRequest()
        {
            // Arrange
             var content = JsonContent.Create(new {});

            // Act
            var response = await _client.PostAsync(RequestUri, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}