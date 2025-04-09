using Application.Candidates.Handlers.CommandHandlers;
using Application.Candidates.Models.Commands;
using Application.Candidates.Repositories;
using Application.Companies.Handlers.CommandHandlers;
using Application.Companies.Models.Commands;
using Application.Companies.Models.Services;
using Application.Companies.Models.Submodels;
using Application.Companies.Repositories;
using Application.Vacancies.Handlers.CommandHandlers;
using Application.Vacancies.Models.Commands;
using Application.Vacancies.Repositories;
using ApplicationTest.Builders;
using AutoFixture;
using Domain.Candidates;
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
    private Mock<ITokenService> _tokenServiceMock;
    private Mock<IRoleRepository> _roleRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _userRepositoryMock = new Mock<IUserRepository>();
        _vacancyRepositoryMock = new Mock<IVacancyRepository>();
        _candidateRepositoryMock = new Mock<ICandidateRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _roleRepositoryMock = new Mock<IRoleRepository>();
    }

    [Test]
    public async Task CreateUserCommandHandler()
    {
        // Arrange
        var command = _fixture.Create<CreateUserCommand>();
        
        var user = User.Create(
            command.RoleId,
            command.CompanyId,
            command.Password,
            command.Name);
        
        _userRepositoryMock
            .Setup(
                repo => repo.Create(
                    user,
                    It.IsAny<CancellationToken>()));
        var handler = new CreateUserCommandHandler(_userRepositoryMock.Object);

        // Act
        var userId = await handler.Handle(command, CancellationToken.None);

        // Assert
        userId.Should().NotBe(Guid.Empty);
        _userRepositoryMock
            .Verify(
                repo => repo.Create(
                    It.Is<User>(u =>
                        u.Name == command.Name &&
                        u.RoleId == command.RoleId &&
                        u.CompanyId == command.CompanyId),
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task AuthorizeUserCommandHandler()
    {
        // Arrange
        var command = _fixture.Create<AuthorizeUserCommand>();
        _fixture.Customize<User>(_ => new UserBuilder(command.Password));
        var user = _fixture.Create<User>();
        _userRepositoryMock
            .Setup(
                repo => repo.Get(
                    command.Login,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        var token = _fixture.Create<Token>();
        
        _fixture.Customize<Role>(_ => new RoleBuilder());
        var role = _fixture.Create<Role>();
        _roleRepositoryMock
            .Setup(
                repository => repository.Get(
                    user.RoleId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        _tokenServiceMock
            .Setup(
                service => service.GenerateToken(
                    user, 
                    role))
            .Returns(token);
        
        var handler = new AuthorizeUserCommandHandler(
            _userRepositoryMock.Object, 
            _roleRepositoryMock.Object, 
            _tokenServiceMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.UserId.Should().Be(user.Id);
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
        _vacancyRepositoryMock
            .Verify(
            repo => repo.Edit(
                command.Vacancy,
                command.Description,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task ApproveCandidateCommandHandler()
    {
        // Arrange
        var command = _fixture.Create<ApproveCandidateCommand>();

        _fixture.Customize<User>(_ => new UserBuilder());
        var user = _fixture.Create<User>();
        _fixture.Customize<Candidate>(_ => new CandidateBuilder(user, 1));
        var candidate = _fixture.Create<Candidate>();

        _candidateRepositoryMock
            .Setup(
                repo => repo.Get(
                    command.CandidateId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(candidate);
        
        _userRepositoryMock
            .Setup(
                repo => repo.Get(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        // Act
        var handler = new ApproveCandidateCommandHandler(
            _candidateRepositoryMock.Object,
            _userRepositoryMock.Object);
        
        await handler.Handle(command, CancellationToken.None);

        // Asset
        candidate.Status.Should().Be(CandidateStatus.Approved);
    }

    [Test]
    public async Task RejectCandidateCommandHandler()
    {

        // Arrange
        var command = _fixture.Create<RejectCandidateCommand>();
        _fixture.Customize<User>(_ => new UserBuilder());
        var user = _fixture.Create<User>();
        _fixture.Customize<Candidate>(_ => new CandidateBuilder(user, 1));
        var candidate = _fixture.Create<Candidate>();

        _candidateRepositoryMock
            .Setup(
                repo => repo.Get(
                    command.CandidateId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(candidate);
        
        _userRepositoryMock
            .Setup(
                repo => repo.Get(
                    command.UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        // Act
        var handler = new RejectCandidateCommandHandler(
            _candidateRepositoryMock.Object,
            _userRepositoryMock.Object);
        
        await handler.Handle(command, CancellationToken.None);

        // Asset
        candidate.Status.Should().Be(CandidateStatus.Rejected);
    }

    [Test]
    public async Task ReplyVacancyCommandHandler()
    {
        // Arrange
        var command = _fixture.Create<ReplyVacancyCommand>();
        _fixture.Customize<User>(_ => new UserBuilder());
        var user = _fixture.Create<User>();
        _fixture.Customize<Vacancy>(_ => new VacancyBuilder(user, 1));
        var vacancy = _fixture.Create<Vacancy>();
        _vacancyRepositoryMock
            .Setup(
                repo => repo.Get(
                    command.VacancyId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(vacancy);

        _candidateRepositoryMock
            .Setup(
                repo => repo.Create(
                    vacancy.CreateCandidate(command.Document, command.ReferalId),
                    It.IsAny<CancellationToken>()));
                    
        var handler = new ReplyVacancyCommandHandler(_vacancyRepositoryMock.Object, _candidateRepositoryMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Asset
        _candidateRepositoryMock
            .Verify(
               repo => repo.Create(
                   It.Is<Candidate>(c =>
                       c.VacancyId == vacancy.Id &&
                       c.Document == command.Document &&
                       c.ReferralId == command.ReferalId),
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

        var vacancy = Vacancy.Create(
            command.CompanyId,
            command.Description,
            command.Workflow);
        
        _vacancyRepositoryMock
            .Setup(
                repo => repo.Create(
                    vacancy,
                    It.IsAny<CancellationToken>()));
        var handler = new CreateVacancyCommandHandler(_vacancyRepositoryMock.Object);

        // Act
        var vacancyId = await handler.Handle(command, CancellationToken.None);

        // Assert
        vacancyId.Should().NotBeEmpty();
        _vacancyRepositoryMock
            .Verify(
            repo => repo.Create(
                It.Is<Vacancy>(v =>
                    v.Description == command.Description &&
                    v.CompanyId == command.CompanyId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}