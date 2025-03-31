using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Requests.Companies;

public class AddUserRequest
{
    [Required]
    public Guid RoleId { get; set; }
    
    [Required]
    public Guid CompanyId { get; set; } 
    
    [Required]
    [StringLength(50, MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password need to be at least 8 characters long " +
                       "and must contain at least one upper case letter, " +
                       "one lower case letter, " +
                       "one number " +
                       "and one special character.")]
    public required string Password { get; set; }
    
    [Required]
    [StringLength(30, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z]+$",
        ErrorMessage = "Name must contain only letters and white spaces.")]
    public required string Name { get; set; }
}