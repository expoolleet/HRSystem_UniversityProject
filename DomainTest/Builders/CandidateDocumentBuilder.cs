using AutoFixture;
using AutoFixture.Kernel;
using Domain.Candidates;
using Domain.Companies;

namespace DomainTest.Builders;

public class CandidateDocumentBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(CandidateDocument).Equals(request))
        {
            return new NoSpecimen();
        }

        return CandidateDocument.Create(
            User.Create(
                context.Create<Guid>(), 
                context.Create<Guid>(),
                context.Create<string>(),
                context.Create<string>()),
            Math.Abs(context.Create<int>()),
            context.Create<string>());
    }
}