using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Microsoft.Deployment.Common.Model.Scribe;

namespace Microsoft.Deployment.Common.Helpers
{
    public class ScribeUtility
    {
        private const string URL_ENDPOINT = "https://api.scribesoft.com";

        public static RestClient Initialize(string username, string password)
        {
            return new RestClient(URL_ENDPOINT, new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Concat(username, ":", password)))));
        }

        public string AesEncrypt(string apiToken, string message)
        {
            const string salt = "ac103458-fcb6-41d3-94r0-43d25b4f4ff4";
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            string result = null;

            // Setup
            using (var aes = new AesManaged())
            {
                aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                aes.IV = new byte[aes.BlockSize / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(aes.IV);
                }

                aes.Padding = PaddingMode.PKCS7;

                // PBKDF2 standard with HMACSHA1 for password-based key generation
                using (var rfcDerivative = new Rfc2898DeriveBytes(apiToken, saltBytes))
                {
                    aes.Key = rfcDerivative.GetBytes(aes.KeySize / 8);
                }


                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            // Convert the passed string to UTF8 as a byte array
                            var messageAsBytes = Encoding.UTF8.GetBytes(message);

                            // Write the iv + cipherText array to the crypto stream and flush it
                            cryptoStream.Write(messageAsBytes, 0, messageAsBytes.Length);
                            cryptoStream.FlushFinalBlock();

                            // Get an array of bytes from the MemoryStream that holds the encrypted data
                            var encryptedBytes = memoryStream.ToArray();
                            result = Convert.ToBase64String(aes.IV) + Convert.ToBase64String(encryptedBytes);
                        }
                    }
                }
            }

            return result;
        }
    }
}