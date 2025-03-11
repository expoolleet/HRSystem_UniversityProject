using System.Security.Cryptography;
using System.Text;

namespace Domain.Companies;

public class Password 
{
    public IReadOnlyCollection<byte> Hash { get; private init; }
    public IReadOnlyCollection<byte> Salt { get; private init; }

    private Password(
        IReadOnlyCollection<byte> hash,
        IReadOnlyCollection<byte> salt)
    {
        ArgumentNullException.ThrowIfNull(hash);
        ArgumentNullException.ThrowIfNull(salt);
        Hash = hash;
        Salt = salt;
    }

    public static Password Create(string password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password);
        using var hmac = new HMACSHA512();

        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return new Password(passwordHash, salt: hmac.Key);
    }
    
    internal bool Verify(string passwordToVerify)
    {
        ArgumentNullException.ThrowIfNull(passwordToVerify);

        using var hmac = new HMACSHA512(Salt.ToArray());

        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordToVerify));
        var a = hmac.ComputeHash(passwordHash);

        var passwordHashArray = Hash.ToArray();
        var b = hmac.ComputeHash(passwordHashArray);
        return Xor(a, b) && Xor(passwordHash, passwordHashArray);
    }

    private static bool Xor(byte[] a, byte[] b)
    {
        var x = a.Length ^ b.Length;

        for (var i = 0; i < a.Length && i < b.Length; ++i)
        {
            x |= a[i] ^ b[i];
        }

        return x == 0;
    }
}