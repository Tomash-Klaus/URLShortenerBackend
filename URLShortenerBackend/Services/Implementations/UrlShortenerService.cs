using System.Security.Cryptography;
using System.Text;

namespace URLShortenerBackend.Services.Implementations
{
    public class UrlShortenerService : IUrlShortenerService
    {

        private const string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public string GenerateShortUrl(string longUrl)
        {
            // Compute MD5 hash
            using MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(longUrl));

            // Extract the first 6 bytes and convert to a decimal number
            long decimalValue = 0;
            for (int i = 0; i < 6; i++)
            {
                decimalValue = (decimalValue << 8) | hash[i];
            }

            // Encode the decimal number in Base62
            return Base62Encode(decimalValue);
        }

        private string Base62Encode(long value)
        {
            var result = new StringBuilder();
            do
            {
                result.Insert(0, Base62Chars[(int)(value % 62)]);
                value /= 62;
            } while (value > 0);

            return result.ToString();
        }
    }
}
