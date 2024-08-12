using System.Security.Cryptography;
using System.Text;

namespace PaparaStore.Data.Service;
public class Md5HashingService : IHashingService
{
    public string HashPassword(string password)
    {
        using (var md5 = MD5.Create())
        {
            var inputBytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = md5.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
