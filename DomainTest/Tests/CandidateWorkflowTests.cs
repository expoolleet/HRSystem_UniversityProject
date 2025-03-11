using NUnit.Framework;
using FluentAssertions;
using AutoFixture;
using AutoFixture.Kernel;
using Domain.Candidates;
using Domain.Companies;
using DomainTest.Builders;

namespace DomainTest.Tests;

[TestFixture]
public class CandidateWorkflowTests
{
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    [Test]
    public void CreateWorkflow()
    {
        // Arrange
        _fixture.Customize<User>(
            _ => new UserBuilder());
        var user = _fixture.Create<User>();
        _fixture.Customize<CandidateWorkflowStep>(
            _ => new CandidateWorkflowStepBuilder(user));
        var steps = _fixture.CreateMany<CandidateWorkflowStep>(1).ToArray();
        
        // Act
        var workflow = CandidateWorkflow.Create(steps);
        
        // Assert
        Assert.That(workflow, Is.Not.Null);
        workflow.Should().NotBeNull();
        workflow.Steps.Should().BeEquivalentTo(steps);
    }
    
    [Test]
    public void CreateWorkflowWithNullStep()
    {
        // Arrange
        _fixture.Customize<User>(
            _ => new UserBuilder());
        var user = _fixture.Create<User>();
        _fixture.Customize<CandidateWorkflowStep>(
            _ => new CandidateWorkflowStepBuilder(user));
        
        // Act && Assert
        Assert.Throws<ArgumentNullException>(
            () => CandidateWorkflow.Create(null!),
            message: null,
            "steps");
    }

    [Test]
    public void ApproveWorkflowStep()
    {
        // Arrange
        _fixture.Customize<User>(
            _ => new UserBuilder());
        var user = _fixture.Create<User>();
        _fixture.Customize<CandidateWorkflowStep>(
            _ => new CandidateWorkflowStepBuilder(user));
        var steps = _fixture.CreateMany<CandidateWorkflowStep>(1).ToArray();
        
        // Act
        var workflow = CandidateWorkflow.Create(steps);
        var firstStep = workflow.Steps.First();
        workflow.Approve(user, _fixture.Create<string>());
        
        // Assert
        firstStep.Status.Should().Be(CandidateStatus.Approved);
    }

    [Test]
    public void ApproveWorkflowSteps()
    {
        // Arrange
        _fixture.Customize<User>(
            _ => new UserBuilder());
        var user = _fixture.Create<User>();
        _fixture.Customize<CandidateWorkflowStep>(
            _ => new CandidateWorkflowStepBuilder(user));
        var steps = _fixture.CreateMany<CandidateWorkflowStep>(3).ToArray();
        
        // Act
        var workflow = CandidateWorkflow.Create(steps);
        do
        {
            workflow.Approve(user, _fixture.Create<string>());
        } while (workflow.Steps
                 .Any(s => s.Status == CandidateStatus.InProcessing));

        // Assert
        workflow.Steps.All(s => s.Status == CandidateStatus.Approved).Should().BeTrue();
    }
    
    [Test]
    public void RejectWorkflowStep()
    {
        // Arrange
        _fixture.Customize<User>(
            _ => new UserBuilder());
        var user = _fixture.Create<User>();
        _fixture.Customize<CandidateWorkflowStep>(
            _ => new CandidateWorkflowStepBuilder(user));
        var steps = _fixture.CreateMany<CandidateWorkflowStep>(1).ToArray();
        
        // Act
        var workflow = CandidateWorkflow.Create(steps);
        var firstStep = workflow.Steps.First();
        workflow.Reject(user, _fixture.Create<string>());
        
        // Assert
        firstStep.Status.Should().Be(CandidateStatus.Rejected);
    }
    
    [Test]
    public void GetRejectedWorkflowStatus()
    {
        // Arrange
        _fixture.Customize<User>(
            _ => new UserBuilder());
        var user = _fixture.Create<User>();
        _fixture.Customize<CandidateWorkflowStep>(
            _ => new CandidateWorkflowStepBuilder(user));
        var steps = _fixture.CreateMany<CandidateWorkflowStep>(2).ToArray();
        
        // Act
        var workflow = CandidateWorkflow.Create(steps);
        workflow.Reject(user, _fixture.Create<string>());
        
        // Assert
        workflow.Status.Should().Be(CandidateStatus.Rejected);
    }

    [Test]
    public void RestartWorkflowSteps()
    {
        // Arrange
        _fixture.Customize<User>(
            _ => new UserBuilder());
        var user = _fixture.Create<User>();
        _fixture.Customize<CandidateWorkflowStep>(
            _ => new CandidateWorkflowStepBuilder(user));
        var steps = _fixture.CreateMany<CandidateWorkflowStep>(3).ToArray();
        
        // Act
        var workflow = CandidateWorkflow.Create(steps);
        workflow.Approve(user, _fixture.Create<string>());
        workflow.Restart();
        
        // Assert
        workflow.Steps.All(
            s => s.Status == CandidateStatus.InProcessing)
            .Should()
            .BeTrue();
    }
    
    [Test]
    public void ShouldThrowExceptionInApproveStep_WrongId()
    {
        // Arrange
        _fixture.Customize<User>(
            _ => new UserBuilder());
        var user = _fixture.Create<User>();
        _fixture.Customize<CandidateWorkflowStep>(
            _ => new CandidateWorkflowStepBuilder(user));
        var steps = _fixture.CreateMany<CandidateWorkflowStep>(1).ToArray();
        
        // Act
        var workflow = CandidateWorkflow.Create(steps);
        
        var user2 = _fixture.Create<User>();

        // Assert
        workflow.Invoking(w => w.Approve(user2, _fixture.Create<string>()))
            .Should().Throw<Exception>();
    }

    [Test]
    public void ShouldThrowExceptionInApproveStep_ApprovedStatus()
    {
        // Arrange
        _fixture.Customize<User>(
            _ => new UserBuilder());
        var user = _fixture.Create<User>();
        _fixture.Customize<CandidateWorkflowStep>(
            _ => new CandidateWorkflowStepBuilder(user));
        var steps = _fixture.CreateMany<CandidateWorkflowStep>(1).ToArray();
        
        var workflow = CandidateWorkflow.Create(steps);
        var feedback = _fixture.Create<string>();
        // Act
        workflow.Approve(user, feedback);
        
        // Assert
        workflow.Invoking(w => w.Approve(user, feedback)).Should().Throw<Exception>();
    }

    [Test]
    public void ShouldThrowExceptionInRejectStep_RejectedStatus()
    {
        // Arrange
        _fixture.Customize<User>(
            _ => new UserBuilder());
        var user = _fixture.Create<User>();
        _fixture.Customize<CandidateWorkflowStep>(
            _ => new CandidateWorkflowStepBuilder(user));
        var steps = _fixture.CreateMany<CandidateWorkflowStep>(3).ToArray();
        var workflow = CandidateWorkflow.Create(steps);
        var feedback = _fixture.Create<string>();
        
        // Act
        workflow.Reject(user, feedback);
        
        // Assert
        workflow.Invoking(w => w.Reject(user, feedback)).Should().Throw<Exception>();
    }
}