using Application.Companies.Models.Responses;
using Application.Companies.Models.Submodels;
using AutoFixture;
using AutoFixture.Kernel;
using Domain.Companies;

namespace ApplicationTest.Builders;

public class UserResponseBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(UserResponse).Equals(request))
        {
            return new NoSpecimen();
        }

        var role = Role.Create(context.Create<string>());

        return UserResponse.Create(
            context.Create<Guid>(),
            role,
            context.Create<Token>());
    }
}