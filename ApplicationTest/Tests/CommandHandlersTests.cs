using Application.Candidates.Handlers.CommandHandlers;
using Application.Candidates.Models.Commands;
using Application.Candidates.Repository;
using Application.Companies.Handlers.CommandHandlers;
using Application.Companies.Models.Commands;
using Application.Companies.Models.Response.Responses;
using Application.Companies.Repositories;
using Application.Vacancies.Handlers.CommandHandlers;
using Application.Vacancies.Models.Commands;
using Application.Vacancies.Repository;
using ApplicationTest.Builders;
using AutoFixture;
using Domain.Companies;
using Domain.Vacancies;
using DomainTest.Builders;
using FluentAssertions;
using Moq;

namespace ApplicationTest.Tests;

[TestFixture]
public class CommandHandlersTests
{
    private Fixture _fixture;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IVacancyRepository> _vacancyRepositoryMock;
    private Mock<ICandidateRepository> _candidateRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _userRepositoryMock = new Mock<IUserRepository>();
        _vacancyRepositoryMock = new Mock<IVacancyRepository>();
        _candidateRepositoryMock = new Mock<ICandidateRepository>();
    }

    [Test]
    public async Task AddUserCommandHandler()
    {
        // Arrange
        var command = _fixture.Create<AddUserCommand>();
        _fixture.Customize<User>(
            _ => new UserBuilder());
        _userRepositoryMock
            .Setup(
                repo => repo.AddUser(
                    command.User,
                    It.IsAny<CancellationToken>()));
        var handler = new AddUserCommandHandler(_userRepositoryMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _userRepositoryMock.Verify(
            repo => repo.AddUser(
                command.User,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task AuthorizeUserCommandHandler()
    {
        // Arrange
        var command = _fixture.Create<AuthorizeUserCommand>();
        _fixture.Customize<UserResponse>(_ => new UserResponseBuilder());
        var response = _fixture.Create<UserResponse>();
        _userRepositoryMock
            .Setup(
                repo => repo.AuthUser(
                    command.Login,
                    command.Password,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var handler = new AuthorizeUserCommandHandler(_userRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(response);
    }

    [Test]
    public async Task EditVacancyCommandHandler()
    {
        // Arrange
        _fixture.Customize<User>(_ => new UserBuilder());
        var user = _fixture.Create<User>();
        
        _fixture.Customize<Vacancy>(
            _ => new VacancyBuilder(user, 1));
        var command = _fixture.Create<EditVacancyCommand>();
        _vacancyRepositoryMock
            .Setup(
                repo => repo.Edit(
                    command.Vacancy,
                    command.Description,
                    It.IsAny<CancellationToken>()));
        var handler = new EditVacancyCommandHandler(_vacancyRepositoryMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _vacancyRepositoryMock.Verify(
            repo => repo.Edit(
                command.Vacancy,
                command.Description,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task AcceptCandidateCommandHandler()
    {
        // Arrange
        var command = _fixture.Create<AcceptCandidateCommand>();
        _candidateRepositoryMock
            .Setup(
                repo => repo.Accept(
                    command.Candidate,
                    It.IsAny<CancellationToken>()));
        var handler = new AcceptCandidateCommandHandler(_candidateRepositoryMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Asset
        _candidateRepositoryMock.Verify(service => service.Accept(
                command.Candidate,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task DeclineCandidateCommandHandler()
    {
        // Arrange
        var command = _fixture.Create<DeclineCandidateCommand>();
        _candidateRepositoryMock
            .Setup(
                repo => repo.Decline(
                    command.Candidate,
                    It.IsAny<CancellationToken>()));
        var handler = new DeclineCandidateCommandHandler(_candidateRepositoryMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Asset
        _candidateRepositoryMock.Verify(
            service => service.Decline(
                command.Candidate,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task ReplyVacancyCommandHandler()
    {
        // Arrange
        var command = _fixture.Create<ReplyVacancyCommand>();
        _vacancyRepositoryMock
            .Setup(
                repo => repo.Reply(
                    command.VacancyId,
                    It.IsAny<CancellationToken>()));
    var handler = new ReplyVacancyCommandHandler(_vacancyRepositoryMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Asset
        _vacancyRepositoryMock.Verify(
            service => service.Reply(
                command.VacancyId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task CreateVacancyCommandHandler()
    {
        // Arrange
        _fixture.Customize<User>(_ => new UserBuilder());
        var user = _fixture.Create<User>();
        
        _fixture.Customize<Vacancy>(
            _ => new VacancyBuilder(user, 1));
        var command = _fixture.Create<CreateVacancyCommand>();
        _vacancyRepositoryMock
            .Setup(
                repo => repo.Create(
                    command.Vacancy,
                    It.IsAny<CancellationToken>()));
        var handler = new CreateVacancyCommandHandler(_vacancyRepositoryMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _vacancyRepositoryMock.Verify(
            repo => repo.Create(
                command.Vacancy,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}