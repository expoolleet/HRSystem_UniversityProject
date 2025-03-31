using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Requests;

public class RejectCandidateRequest
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public required string Feedback {get; set;}
}