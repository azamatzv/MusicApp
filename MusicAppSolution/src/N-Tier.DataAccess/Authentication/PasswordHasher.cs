using System.Security.Cryptography;
using System.Text;

namespace N_Tier.DataAccess.Authentication;

public class PasswordHasher : IPasswordHasher
{
    private const int KeySize = 32;
    private const int IterationsCount = 10_000;

    public string Encrypt(string password, string salt)
    {
        using (var algorithm = new Rfc2898DeriveBytes(
            password: password,
            salt: Encoding.UTF8.GetBytes(salt),
            iterations: IterationsCount,
            hashAlgorithm: HashAlgorithmName.SHA256))
        {
            return Convert.ToBase64String(algorithm.GetBytes(KeySize));
        }
    }

    public bool Verify(string hash, string password, string salt)
    {
        return hash == Encrypt(password, salt);
    }

    public static string GenerateSalt(int size = 16)
    {
        var saltBytes = new byte[size];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }
}