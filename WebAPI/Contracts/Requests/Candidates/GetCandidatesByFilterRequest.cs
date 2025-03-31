using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Requests;

public class GetCandidatesByFilterRequest
{
    public Guid? CompanyId { get; set; }
    
    [StringLength(50)]
    public string? Title { get; set; }
    
    [Required]
    public int Page { get; set; }
    
    [Required]
    public int PageSize { get; set; }
}