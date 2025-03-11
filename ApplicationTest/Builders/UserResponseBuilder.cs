using Application.Companies.Models.Response.Responses;
using AutoFixture;
using AutoFixture.Kernel;

namespace ApplicationTest.Builders;

public class UserResponseBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(UserResponse).Equals(request))
        {
            return new NoSpecimen();
        }

        return UserResponse.Create(
            context.Create<Guid>(),
            context.Create<string>(),
            context.Create<Guid>(),
            context.Create<string>());
    }
}