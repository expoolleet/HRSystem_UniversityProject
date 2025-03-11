using AutoFixture;
using Domain.Candidates;
using Domain.Companies;
using Domain.Vacancies;
using DomainTest.Builders;
using FluentAssertions;

namespace DomainTest.Tests;

[TestFixture]
public class VacancyWorkflowTests
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
        _fixture.Customize<User>(_ => new UserBuilder());
        var user = _fixture.Create<User>();
        _fixture.Customize<VacancyWorkflowStep>(_ => new VacancyWorkflowStepBuilder(user));
        var steps = _fixture.CreateMany<VacancyWorkflowStep>(1).ToArray();
        
        // Act
        var workflow = VacancyWorkflow.Create(steps);
        
        // Assert
        workflow.Should().NotBeNull();
        workflow.Steps.Should().BeEquivalentTo(steps);
    }
    
    [Test]
    public void CreateWorkflowWithNullStep()
    {
        // Arrange
        _fixture.Customize<User>(_ => new UserBuilder());
        var user = _fixture.Create<User>();
        _fixture.Customize<VacancyWorkflowStep>(_ => new VacancyWorkflowStepBuilder(user));
        
        // Act && Assert
        Assert.Throws<ArgumentNullException>(
            () => VacancyWorkflow.Create(null!), 
            message: null,
            "steps");
    }

    [Test]
    public void ToCandidate()
    {
        // Arrange
        _fixture.Customize<User>(_ => new UserBuilder());
        var user = _fixture.Create<User>();
        _fixture.Customize<VacancyWorkflowStep>(_ => new VacancyWorkflowStepBuilder(user));
        var steps = _fixture.CreateMany<VacancyWorkflowStep>(1).ToArray();
        var workflow = VacancyWorkflow.Create(steps);
        
        // Act
        var candidate = workflow.ToCandidate();
        
        // Assert
        Assert.That(candidate, Is.TypeOf<CandidateWorkflow>());
    }
}