using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts.Requests.Companies;

public class AuthUserRequest
{
    [Required]
    [StringLength(30, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z]+$",
        ErrorMessage = "Name must contain only letters and white spaces.")]
    public required string Login { get; init; }
    
    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password need to be at least 8 characters long " +
                       "and must contain at least one upper case letter, " +
                       "one lower case letter, " +
                       "one number " +
                       "and one special character.")]
    public required string Password { get; init; }
}