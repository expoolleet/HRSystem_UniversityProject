using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using AutoFixture;
using Domain.Vacancies;
using FluentAssertions;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Contracts.Dto.Vacancies;
using WebApi.Contracts.Requests.Vacancies;
using WebApi.Services;
using WebApiTest.Builders;

namespace WebApiTest.Tests
{
    public class VacanciesControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
        private readonly TokenService _tokenService;
        private readonly IFixture _fixture;

        public VacanciesControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _fixture = new Fixture();
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions());
            _tokenService = new TokenService();
        }

        private async Task<string> GetRootUserTokenAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Name == "root");
            if (user == null) throw new InvalidOperationException("Seeding failed: 'root' user not found.");

            var role = await dbContext.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId);
            if (role == null) throw new InvalidOperationException($"Seeding failed: Role for 'root' user not found (RoleId: {user.RoleId}).");

            var tokenResult = _tokenService.GenerateToken(user, role);
            return tokenResult.Data!;
        }

        [Fact]
        public async Task GetVacancies_WhenCalled_ReturnsOkAndListOfShortVacancies()
        {
            var response = await _client.GetAsync("/api/vacancy");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var vacancies = await response.Content.ReadFromJsonAsync<List<VacancyShortDto>>();
            vacancies.Should().NotBeNull();
             vacancies!.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetVacancyById_WhenCalled_ReturnsOkAndShortVacancy()
        {
            Guid vacancyId;
            using (var scope = _factory.Services.CreateScope())
            {
                var scopeProvider = scope.ServiceProvider;
                var dbContext = scopeProvider.GetRequiredService<MainDbContext>();
                var vacancy = await dbContext.Vacancies.FirstAsync();
                vacancyId = vacancy.Id;
            }

            var response = await _client.GetAsync($"/api/vacancy/{vacancyId}");

            var vacancyDto = await response.Content.ReadFromJsonAsync<VacancyShortDto>();
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            vacancyDto.Should().NotBeNull();
            vacancyDto!.Id.Should().Be(vacancyId);
        }

        [Fact]
        public async Task GetVacanciesByFilter_WhenCalled_ReturnsOneVacancy()
        {
            var token = await GetRootUserTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var vacancyTitleFilter = "Test Vacancy 2";

            var response = await _client.GetAsync($"/api/vacancy?title={vacancyTitleFilter}");

            var vacancyDto = await response.Content.ReadFromJsonAsync<List<VacancyDto>>();
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            vacancyDto.Should().NotBeNull();
            vacancyDto!.Count.Should().Be(1);
            vacancyDto.First().Workflow.Should().NotBeNull();

             _client.DefaultRequestHeaders.Authorization = null;
        }

        [Fact]
        public async Task GetVacancyById_WhenCalled_ReturnsOkAndFullVacancy()
        {
            Guid vacancyId;
            using (var scope = _factory.Services.CreateScope())
            {
                var scopeProvider = scope.ServiceProvider;
                var dbContext = scopeProvider.GetRequiredService<MainDbContext>();
                var vacancy = await dbContext.Vacancies.FirstAsync();
                vacancyId = vacancy.Id;
            }

            var token = await GetRootUserTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"/api/vacancy/{vacancyId}");

            var vacancyDto = await response.Content.ReadFromJsonAsync<VacancyDto>();
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            vacancyDto.Should().NotBeNull();
            vacancyDto!.Id.Should().Be(vacancyId);
            vacancyDto.Workflow.Should().NotBeNull();

             _client.DefaultRequestHeaders.Authorization = null;
        }

        [Fact]
        public async Task CreateVacancy_WhenCalled_ReturnsCreatedAtActionResult()
        {
            Guid companyId;
            using (var scope = _factory.Services.CreateScope())
            {
                var scopeProvider = scope.ServiceProvider;
                var dbContext = scopeProvider.GetRequiredService<MainDbContext>();
                var company = await dbContext.Companies.FirstAsync();
                companyId = company.Id;
            }

            _fixture.Customize<VacancyWorkflowDto>(_ => new VacancyWorkflowDtoBuilder(2));
            var request = new CreateVacancyRequest
            {
                CompanyId = companyId,
                Description = _fixture.Create<string>(),
                Workflow = _fixture.Create<VacancyWorkflowDto>()
            };

            var token = await GetRootUserTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsJsonAsync($"/api/vacancy", request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

             _client.DefaultRequestHeaders.Authorization = null;
        }

        [Fact]
        public async Task EditVacancy_WhenCalled_ReturnsOkAndVacancyId()
        {
            Vacancy vacancy;
            using (var scope = _factory.Services.CreateScope())
            {
                var scopeProvider = scope.ServiceProvider;
                var dbContext = scopeProvider.GetRequiredService<MainDbContext>();
                vacancy = await dbContext.Vacancies.FirstAsync();
            }

            var description = _fixture.Create<string>();
            _fixture.Customize<VacancyWorkflowDto>(_ => new VacancyWorkflowDtoBuilder(2));
            var request = new EditVacancyRequest()
            {
                Description = description,
            };

            var token = await GetRootUserTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PutAsJsonAsync($"/api/vacancy/{vacancy.Id}", request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            using (var scope = _factory.Services.CreateScope())
            {
                var scopeProvider = scope.ServiceProvider;
                var dbContext = scopeProvider.GetRequiredService<MainDbContext>();
                var updatedVacancy = await dbContext.Vacancies.FindAsync(vacancy.Id);
                updatedVacancy.Should().NotBeNull();
                updatedVacancy!.Description.Should().Be(description);
            }
            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
}