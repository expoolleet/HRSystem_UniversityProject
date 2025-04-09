using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Requests.Candidates;

public class RejectCandidateRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public required string Feedback {get; set;}
}