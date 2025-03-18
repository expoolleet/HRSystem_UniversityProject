using AutoFixture;
using Domain.Candidates;
using Domain.Companies;
using DomainTest.Builders;
using FluentAssertions;

namespace DomainTest.Tests;

[TestFixture]
public class CandidateTests
{
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _fixture.Customize<CandidateDocument>(_ => new CandidateDocumentBuilder());
        _fixture.Customize<User>(_ => new UserBuilder());
    }

    [Test]
    public void CreateCandidate()
    {
        // Act
        var candidate = _fixture.Create<Candidate>();

        // Assert
        candidate.Should().NotBeNull();
        candidate.Id.Should().NotBe(Guid.Empty);
        candidate.Status.Should().Be(CandidateStatus.InProcessing);
    }

    [Test]
    public void ApproveCandidate()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _fixture.Customize<Candidate>(_ => new CandidateBuilder(user, 1));
        var candidate = _fixture.Create<Candidate>();
        var feedback = _fixture.Create<string>();

        // Act
        candidate.Approve(user, feedback);

        // Assert
        candidate.Status.Should().Be(CandidateStatus.Approved);
    }

    [Test]
    public void RejectCandidate()
    {
        // Arrange
        var candidate = _fixture.Create<Candidate>();
        var user = _fixture.Create<User>();
        var feedback = _fixture.Create<string>();

        // Act
        candidate.Reject(user, feedback);

        // Assert
        candidate.Status.Should().Be(CandidateStatus.Rejected);
    }

    [Test]
    public void RestartCandidate()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _fixture.Customize<Candidate>(_ => new CandidateBuilder(user, 1));
        var candidate = _fixture.Create<Candidate>();
        var feedback = _fixture.Create<string>();

        // Act - сначала изменяем статус, затем вызываем Restart.
        candidate.Approve(user, feedback);
        candidate.Restart();

        // Assert
        candidate.Status.Should().Be(CandidateStatus.InProcessing);
    }

    [Test]
    public void ApproveCandidate_WithWrongUser_ShouldThrowException()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _fixture.Customize<Candidate>(_ => new CandidateBuilder(user, 1));
        var candidate = _fixture.Create<Candidate>();

        var wrongUser = _fixture.Create<User>();
        var feedback = _fixture.Create<string>();

        // Act
        candidate.Approve(user, feedback);

        // Assert 
        Action act = () => candidate.Approve(wrongUser, feedback);
        act.Should().Throw<Exception>();
    }
    
    [Test]
    public void Candidate_ApproveOneStepWorkflow_ShouldApproveOnlyFirstStep()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _fixture.Customize<Candidate>(_ => new CandidateBuilder(user, 3));
        var candidate = _fixture.Create<Candidate>();
        var feedback = _fixture.Create<string>();

        // Act
        candidate.Approve(user, feedback);

        // Assert
        candidate.Workflow.Steps
            .OrderBy(s => s.Number)
            .First()
            .Status.Should().Be(CandidateStatus.Approved);
 
        candidate.Workflow.Steps
            .OrderBy(s => s.Number)
            .Skip(1)
            .All(s => s.Status == CandidateStatus.InProcessing).Should().BeTrue();
    }

    [Test]
    public void Candidate_RejectWorkflowStep_ShouldSetStatusToRejected()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _fixture.Customize<Candidate>(_ => new CandidateBuilder(user, 3));
        var candidate = _fixture.Create<Candidate>();
        var feedback = _fixture.Create<string>();

        // Act
        candidate.Reject(user, feedback);
        
        // Assert
        candidate.Workflow.Steps
            .OrderBy(s => s.Number)
            .First()
            .Status.Should().Be(CandidateStatus.Rejected);
        candidate.Workflow.Status
            .Should().Be(CandidateStatus.Rejected);
    }
}