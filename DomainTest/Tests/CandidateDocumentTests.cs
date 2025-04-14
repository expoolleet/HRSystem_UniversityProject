using AutoFixture;
using Domain.Candidates;
using Domain.Companies;
using DomainTest.Builders;
using FluentAssertions;

namespace DomainTest.Tests;

[TestFixture]
public class CandidateDocumentTests
{
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    [Test]
    public void CreateDocument()
    {
        // Arrange
        _fixture.Customize<CandidateDocument>(
            _ => new CandidateDocumentBuilder());

        // Act
        var candidateDocument = _fixture.Create<CandidateDocument>();

        // Assert
        candidateDocument.Should().NotBeNull();
        candidateDocument.Name.Should().NotBeNullOrEmpty();
        candidateDocument.WorkExperience.Should().BeGreaterThanOrEqualTo(0);
    }
    
    [Test]
    public void CreateDocumentWithNullUser()
    {
        // Act && Assert
        Assert.Throws<ArgumentException>(() =>
        {
            CandidateDocument.Create(
                "", 
                _fixture.Create<int>(), 
                _fixture.Create<string>(),
                    _fixture.Create<string>());
        });
    }
    
    [Test]
    public void CreateDocumentWithNegativeWorkExperience()
    {
        // Arrange
        _fixture.Customize<User>(_ => new UserBuilder());
        var user = _fixture.Create<User>();

        // Act
        Exception exception = Assert.Throws<Exception>(() =>
        {
            CandidateDocument.Create(
                _fixture.Create<string>(), 
                -1, 
                _fixture.Create<string>(),
                _fixture.Create<string>());
        });

        // Assert
        exception.Message.Should().Be("Wrong work experience (negative value)");
    }
}