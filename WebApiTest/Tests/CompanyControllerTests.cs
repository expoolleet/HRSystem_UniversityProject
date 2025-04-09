using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using AutoFixture;
using WebApi.Contracts.Dto.Companies;
using WebApi.Services;

namespace WebApiTest.Tests
{
    public class CompanyControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
        private readonly TokenService _tokenService;
        private readonly IFixture _fixture;
        
        public CompanyControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _fixture = new Fixture();
            _client = factory.CreateClient();
            _tokenService = new TokenService();
            EnsureSeedData();
        }

        private void EnsureSeedData()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
            if (!dbContext.Users.Any(u => u.Name == "root")) throw new InvalidOperationException("Seeding failed: 'root' user must be present.");
            if (!dbContext.Companies.Any(c => c.Name == "RootCompany")) throw new InvalidOperationException("Seeding failed: 'RootCompany' must be present.");
            if (!dbContext.Roles.Any(r => r.Name == "Admin")) throw new InvalidOperationException("Seeding failed: 'Admin' role must be present.");
        }

        private async Task<string> GetRootUserTokenAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Name == "root");
            var role = await dbContext.Roles.FirstOrDefaultAsync(r => r.Id == user!.RoleId);
            var tokenResult = _tokenService.GenerateToken(user!, role!);
            return tokenResult.Data!;
        }

        private async Task<(Guid UserId, string UserName)> GetExistingUserInfoAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Name == "root");
            if (user == null) throw new InvalidOperationException("Seeding failed: 'root' user not found.");
            return (user.Id, user.Name);
        }

         private async Task<Guid> GetExistingCompanyIdAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
            var company = await dbContext.Companies.FirstOrDefaultAsync(c => c.Name == "RootCompany");
            if (company == null) throw new InvalidOperationException("Seeding failed: 'RootCompany' not found.");
            return company.Id;
        }

        private async Task<Guid> GetExistingRoleIdAsync(string roleName = "Admin")
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
            var role = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
             if (role == null) throw new InvalidOperationException($"Seeding failed: '{roleName}' role not found.");
            return role.Id;
        }


        [Fact]
        public async Task GetUserById_WhenAuthorized_ReturnsOkAndUser()
        {
            var token = await GetRootUserTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var (userId, _) = await GetExistingUserInfoAsync();

            var response = await _client.GetAsync($"/api/Company/users/{userId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var userDto = await response.Content.ReadFromJsonAsync<UserDto>();
            userDto.Should().NotBeNull();
            userDto!.Id.Should().Be(userId);

            _client.DefaultRequestHeaders.Authorization = null;
        }

        [Fact]
        public async Task GetUserById_WhenUnauthorized_ReturnsUnauthorized()
        {
             var (userId, _) = await GetExistingUserInfoAsync();
             _client.DefaultRequestHeaders.Authorization = null;

            var response = await _client.GetAsync($"/api/Company/users/{userId}");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }


        [Fact]
        public async Task GetUserByName_WhenAuthorized_ReturnsOkAndUser()
        {
            var token = await GetRootUserTokenAsync();
             _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var (_, userName) = await GetExistingUserInfoAsync();

            var response = await _client.GetAsync($"/api/Company/users/{userName}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var userDto = await response.Content.ReadFromJsonAsync<UserDto>();
            userDto.Should().NotBeNull();
            userDto!.Name.Should().Be(userName);

            _client.DefaultRequestHeaders.Authorization = null;
        }

         [Fact]
        public async Task GetUserByName_WhenUnauthorized_ReturnsUnauthorized()
        {
            var (_, userName) = await GetExistingUserInfoAsync();
             _client.DefaultRequestHeaders.Authorization = null;

            var response = await _client.GetAsync($"/api/company/users/{userName}");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CreateCompany_WhenAuthorizedAsAdmin_ReturnsOkAndCompany()
        {
            var token = await GetRootUserTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var companyName = _fixture.Create<string>();
            
            var response = await _client.PostAsync($"/api/company/{companyName}", null);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            _client.DefaultRequestHeaders.Authorization = null;
        }

         [Fact]
        public async Task CreateCompany_WhenUnauthorized_ReturnsUnauthorized()
        {
             _client.DefaultRequestHeaders.Authorization = null;
            var companyName = _fixture.Create<string>();

            var response = await _client.PostAsync($"/api/Company/{companyName}", null);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CreateRole_WhenAuthorizedAsAdmin_ReturnsOkAndRole()
        {
            var token = await GetRootUserTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var roleName = $"TestRole_{_fixture.Create<string>().Substring(0, 10)}";
            
            var response = await _client.PostAsync($"/api/Company/roles/{roleName}", null);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            
            _client.DefaultRequestHeaders.Authorization = null;
        }

        [Fact]
        public async Task CreateRole_WhenUnauthorized_ReturnsUnauthorized()
        {
            _client.DefaultRequestHeaders.Authorization = null;
            var roleName = _fixture.Create<string>();

            var response = await _client.PostAsync($"/api/Company/roles/{roleName}", null);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}