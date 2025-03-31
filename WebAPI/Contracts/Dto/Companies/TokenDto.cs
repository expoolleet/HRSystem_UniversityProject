namespace WebApi.Contracts.Dto.Companies;

public class TokenDto
{
    public required string Data { get; init; }
    public DateTime Expires { get; init; }
}