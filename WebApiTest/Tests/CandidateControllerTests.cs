using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using AutoFixture;
using Domain.Candidates;
using Domain.Companies;
using WebApi.Contracts.Dto.Candidates;
using WebApi.Contracts.Requests.Candidates;
using WebApi.Services;

namespace WebApiTest.Tests
{
    public class CandidateControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
        private readonly TokenService _tokenService;
        private readonly IFixture _fixture;

        public CandidateControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _fixture = new Fixture();
            _client = factory.CreateClient(); 
            _tokenService = new TokenService();
        }

        private async Task<(User, Role)> GetUserAndHisRoleFromDbContext(string name)
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
            var user = await dbContext.Users.FirstAsync(u => u.Name == name);
            var role = await dbContext.Roles.FirstAsync(r => r.Id == user.RoleId);
            return (user, role);
        }
        
        private async Task<string> GetRootUserTokenAsync()
        {
            var (user, role) = await GetUserAndHisRoleFromDbContext("root");
            var tokenResult = _tokenService.GenerateToken(user, role);
            return tokenResult.Data!;
        }
        
        private async Task<string> GetManagerTokenAsync()
        {
            var (user, role) = await GetUserAndHisRoleFromDbContext("manager");
            var tokenResult = _tokenService.GenerateToken(user, role);
            return tokenResult.Data!;
        }
        
        private async Task<string> GetRecruiterTokenAsync()
        {
            var (user, role) = await GetUserAndHisRoleFromDbContext("recruiter");
            var tokenResult = _tokenService.GenerateToken(user, role);
            return tokenResult.Data!;
        }
        
        private async Task<Guid> GetCandidateIdAsync(string name)
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
            var candidate = await dbContext.Candidates.FirstOrDefaultAsync(c => c.Document.Name == name);
            if (candidate == null) throw new InvalidOperationException("No seeded candidates found in the database.");
            return candidate.Id;
        }

        [Fact]
        public async Task GetCandidateById_WhenUnauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var candidateId = await GetCandidateIdAsync("Candidate 1");
             _client.DefaultRequestHeaders.Authorization = null;

            // Act
            var response = await _client.GetAsync($"/api/candidate/{candidateId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetCandidateById_WhenAuthorizedAndExists_ReturnsOkAndCandidateDto()
        {
            // Arrange
            var token = await GetRootUserTokenAsync();
            var candidateId = await GetCandidateIdAsync("Candidate 1");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync($"/api/candidate/{candidateId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var candidateDto = await response.Content.ReadFromJsonAsync<CandidateDto>();

            candidateDto.Should().NotBeNull();
            candidateDto!.Id.Should().Be(candidateId);
            candidateDto.Document.Name.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetCandidates_WhenUnauthorized_ReturnsUnauthorized()
        {
            // Arrange
             _client.DefaultRequestHeaders.Authorization = null;

            // Act
            var response = await _client.GetAsync("/api/candidate?page=1&pageSize=10");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetCandidates_WhenAuthorized_ReturnsOkAndPagedList()
        {
            // Arrange
            var token = await GetRootUserTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            int page = 1;
            int pageSize = 10;

            // Act
            var response = await _client.GetAsync($"/api/candidate?page={page}&pageSize={pageSize}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var candidates = await response.Content.ReadFromJsonAsync<List<CandidateDto>>();

            candidates.Should().NotBeNull();
            candidates!.Count.Should().BeGreaterThan(0);
        }


        [Fact]
        public async Task ApproveCandidate_WhenUnauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var candidateId = await GetCandidateIdAsync("Candidate 1");
            var request = _fixture.Build<ApproveCandidateRequest>().With(r => r.Feedback, _fixture.Create<string>()).Create();
             _client.DefaultRequestHeaders.Authorization = null;

            // Act
            var response = await _client.PostAsJsonAsync($"/api/candidate/{candidateId}/approve", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

         [Fact]
        public async Task ApproveCandidate_WhenAuthorizedAndExistsWithRightRole_ReturnOK()
        {
            // Arrange
            var token = await GetRootUserTokenAsync();
            var candidateId = await GetCandidateIdAsync("Candidate 2");
            var request = _fixture.Build<ApproveCandidateRequest>().With(r => r.Feedback, _fixture.Create<string>()).Create();
             _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.PostAsJsonAsync($"/api/candidate/{candidateId}/approve", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            using (var scope = _factory.Services.CreateScope())
            {
             var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
             var candidate = await dbContext.Candidates.Include(c => c.Workflow).FirstOrDefaultAsync(c => c.Id == candidateId);
             candidate.Should().NotBeNull();
             candidate.Status.Should().Be(CandidateStatus.Approved);
            }
        }

        [Fact]
        public async Task ApproveCandidate_WhenAuthorizedAndExistsWithWrongRole_ReturnsForbidden()
        {
            // Arrange
            var token = await GetRecruiterTokenAsync();
            var candidateId = await GetCandidateIdAsync("Candidate 1");
            var request = _fixture.Build<ApproveCandidateRequest>().With(r => r.Feedback, _fixture.Create<string>()).Create();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.PostAsJsonAsync($"/api/candidate/{candidateId}/approve", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task RejectCandidate_WhenUnauthorized_ReturnsUnauthorized()
        {
             // Arrange
            var candidateId = await GetCandidateIdAsync("Candidate 1");
            var request = _fixture.Build<RejectCandidateRequest>().With(r => r.Feedback, _fixture.Create<string>()).Create();
             _client.DefaultRequestHeaders.Authorization = null;

            // Act
            var response = await _client.PostAsJsonAsync($"/api/candidate/{candidateId}/reject", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task RejectCandidate_WhenAuthorizedAndExists_ReturnsOk()
        {
             // Arrange
            var token = await GetManagerTokenAsync();
            var candidateId = await GetCandidateIdAsync("Candidate 1");
            var request = _fixture.Build<RejectCandidateRequest>().With(r => r.Feedback, _fixture.Create<string>()).Create();
             _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.PostAsJsonAsync($"/api/candidate/{candidateId}/reject", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
                var candidate = await dbContext.Candidates.Include(c => c.Workflow).FirstOrDefaultAsync(c => c.Id == candidateId);
                candidate.Should().NotBeNull();
                candidate.Status.Should().Be(CandidateStatus.Rejected);
            }
        }
    }
}