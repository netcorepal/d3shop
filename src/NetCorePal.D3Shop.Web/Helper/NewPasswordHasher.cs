using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace NetCorePal.D3Shop.Web.Helper;

public static class NewPasswordHasher
{
    public static string HashPassword(string value, string salt)
    {
        var valueBytes = KeyDerivation.Pbkdf2(
            password: value,
            salt: Encoding.UTF8.GetBytes(salt),
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 100000,
            numBytesRequested: 256 / 8);

        return Convert.ToBase64String(valueBytes);
    }

    public static (string Hash, string Salt) HashPassword(string password)
    {
        var salt = GenerateSalt();
        var hash = HashPassword(password, salt);
        return (hash, salt);
    }

    private static string GenerateSalt()
    {
        var randomBytes = new byte[128 / 8];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}