using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Requests.Companies;

public class CreateCompanyRequest
{
    [Required]
    public required string Name { get; init; }
}