using Domain.Companies;

namespace Application.Companies.Filters;

public interface IUserFilter
{
    public User?  FindUser(Guid id, Guid companyId, string title);
}