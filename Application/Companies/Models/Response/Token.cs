namespace Application.Companies.Models.Response;

public class Token
{
    private const int ExpiresDays = 30;
    public string Data { get; private init; }
    public DateTime Expires { get; private init; }

    private Token(string data, DateTime expires)
    {
        ArgumentException.ThrowIfNullOrEmpty(data);

        Data = data;
        Expires = expires;
    }

    public static Token Create(string data)
        => new Token(data, DateTime.UtcNow.AddDays(ExpiresDays));
}