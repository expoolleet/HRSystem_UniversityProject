using AutoFixture;
using AutoFixture.Kernel;
using Domain.Companies;

namespace DomainTest.Builders;

public class UserBuilder : ISpecimenBuilder
{
    private readonly string? _password;
    public UserBuilder() { }
    public UserBuilder(string? password)
    {
        _password = password;
    }
    
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(User).Equals(request))
        {
            return new NoSpecimen();
        }

        return User.Create(
            context.Create<Guid>(),
            context.Create<Guid>(),
            context.Create<string>(),
            _password ?? context.Create<string>());
    }
}