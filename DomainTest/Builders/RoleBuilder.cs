using AutoFixture;
using AutoFixture.Kernel;
using Domain.Companies;

namespace DomainTest.Builders;

public class RoleBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(Role).Equals(request))
        {
            return new NoSpecimen();
        }

        return Role.Create(context.Create<string>());
    }
}