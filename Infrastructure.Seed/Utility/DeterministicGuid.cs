using System.Text;
using System.Security.Cryptography;

namespace Infrastructure.Seed.Utility
{
    public static class DeterministicGuid
    {
        public static Guid Create(string input)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return new Guid(hash);
        }
    }
}
