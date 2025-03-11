using AutoFixture;
using AutoFixture.Kernel;
using Domain.Companies;

namespace DomainTest.Builders;

public class UserBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(User).Equals(request))
        {
            return new NoSpecimen();
        }

        return User.Create(
            context.Create<Guid>(),
            context.Create<string>(),
            context.Create<string>()
            );
    }
}