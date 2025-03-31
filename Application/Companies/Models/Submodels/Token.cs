namespace Application.Companies.Models.Submodels;

public class Token
{
    public string Data { get; private init; }
    public DateTime Expires { get; private init; }

    private Token(string data, DateTime expires)
    {
        ArgumentException.ThrowIfNullOrEmpty(data);

        Data = data;
        Expires = expires;
    }

    public static Token Create(string data, DateTime expires)
        => new Token(data, expires);
}