using System.Security.Cryptography;
using System.Text;

namespace DeltaBrainsJSCAppBE.Handle
{
    public static class CompareSHA256
    {
        public static string ToSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
