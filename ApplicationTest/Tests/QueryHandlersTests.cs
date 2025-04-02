using Application.Candidates.Handlers.QueryHandlers;
using Application.Candidates.Models.Queries;
using Application.Candidates.Repositories;
using Application.Context;
using Application.Vacancies.Handlers.QueryHandlers;
using Application.Vacancies.Models.Queries;
using Application.Vacancies.Repositories;
using AutoFixture;
using Domain.Candidates;
using Domain.Companies;
using Domain.Vacancies;
using DomainTest.Builders;
using FluentAssertions;
using Moq;

namespace ApplicationTest.Tests;

[TestFixture]
public class QueryHandlersTests
{
    private  Fixture _fixture;

    private Mock<ICandidateRepository> _candidateRepositoryMock;
    private Mock<IVacancyRepository> _vacancyRepositories;
    private Mock<IUserContext> _userContextMock;
    
    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _candidateRepositoryMock = new Mock<ICandidateRepository>();
        _vacancyRepositories = new Mock<IVacancyRepository>();
        _userContextMock = new Mock<IUserContext>();
    }

    [Test]
    public async Task GetCandidatesByFilterQueryHandler()
    {
        // Arrange
        var query = _fixture.Create<GetCandidatesByFilterQuery>();

        _fixture.Customize<User>(_ => new UserBuilder());
        var user = _fixture.Create<User>();
        
        _fixture.Customize<Candidate>(_ => new CandidateBuilder(user, 1));
        var candidates = _fixture.CreateMany<Candidate>(1).ToArray();
        
        _candidateRepositoryMock.Setup(
            repo => repo.GetCollectionByFilter(
                query.CompanyId,
                query.Title,
                query.Page,
                query.PageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(candidates);
        var handler = new GetCandidatesByFilterQueryHandler(_candidateRepositoryMock.Object);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(candidates);
        
        _candidateRepositoryMock.Verify(
            repo => repo.GetCollectionByFilter(
                query.CompanyId,
                query.Title,
                query.Page,
                query.PageSize,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetCandidateQueryHandler()
    {
        // Arrange
        var query = _fixture.Create<GetCandidateQuery>();

        _fixture.Customize<User>(_ => new UserBuilder());
        var user = _fixture.Create<User>();
        
        _fixture.Customize<Candidate>(_ => new CandidateBuilder(user, 1));
        var candidate = _fixture.Create<Candidate>();
        
        _candidateRepositoryMock.Setup(
                repo => repo.Get(
                    query.CandidateId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(candidate);
        var handler = new GetCandidateQueryHandler(_candidateRepositoryMock.Object);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(candidate);
        
        _candidateRepositoryMock.Verify(
            repo => repo.Get(
                query.CandidateId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Test]
    public async Task GetVacanciesByFilterQueryHandler()
    {
        // Arrange
        _fixture.Customize<User>(_ => new UserBuilder());
        var user = _fixture.Create<User>();
        
        _fixture.Customize<Vacancy>(
            _ => new VacancyBuilder(user, 1));
        
        var query = _fixture.Create<GetVacanciesByFilterQuery>();
        
        var vacancies = _fixture.CreateMany<Vacancy>(1).ToArray();

        _vacancyRepositories
            .Setup(
            repo => repo.GetCollectionByFilter(
                query.CompnayId,
                query.Title,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(vacancies);

        _userContextMock.Setup(repo => repo.GetUserId(It.IsAny<CancellationToken>()));

        var handler = new GetVacanciesByFilterQueryHandler(
            _vacancyRepositories.Object, 
            _userContextMock.Object);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(vacancies);
        _vacancyRepositories.Verify(
            repo => repo.GetCollectionByFilter(
                query.CompnayId,
                query.Title,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetVacancyQueryHandler()
    {
        // Arrange
        _fixture.Customize<User>(_ => new UserBuilder());
        var user = _fixture.Create<User>();
        
        _fixture.Customize<Vacancy>(
            _ => new VacancyBuilder(user, 1));
        var query = _fixture.Create<GetVacancyQuery>();
        
        var vacancy = _fixture.Create<Vacancy>();

        _vacancyRepositories
            .Setup(
                repo => repo.Get(
                    query.VacancyId,
                    It.IsAny<CancellationToken>()
                ))
            .ReturnsAsync(vacancy);

        _userContextMock.Setup(repo => repo.GetUserId(It.IsAny<CancellationToken>()));

        var handler = new GetVacancyQueryHandler(
            _vacancyRepositories.Object, 
            _userContextMock.Object);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(vacancy);
        _vacancyRepositories.Verify(
            repo => repo.Get(
                query.VacancyId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}