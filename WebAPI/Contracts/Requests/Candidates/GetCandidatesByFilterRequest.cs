using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Requests.Candidates;

public class GetCandidatesByFilterRequest
{
    public Guid? CompanyId { get; init; }
    
    [StringLength(50, MinimumLength = 0)]
    public string? Title { get; init; }
    
    [Required]
    public int Page { get; init; }
    
    [Required]
    public int PageSize { get; init; }
}