using System.Security.Cryptography;
using System.Text;

namespace Median.Intranet.Utils
{    
    public static class NanoIdGenerator
    {
        private const string DefaultAlphabet =
            "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public static string Generate(
            int size = 10,
            string? prefix = null,
            string? alphabet = null)
        {
            alphabet ??= DefaultAlphabet;

            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            var buffer = new byte[size];
            RandomNumberGenerator.Fill(buffer);

            var chars = new char[size];

            for (int i = 0; i < size; i++)
            {
                chars[i] = alphabet[buffer[i] % alphabet.Length];
            }

            var id = new string(chars);
            return prefix is null ? id : $"{prefix}_{id}";
        }
    }

}
