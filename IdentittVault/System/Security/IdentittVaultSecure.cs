using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IdentittVault.System.Security
{
    public class IdentittVaultSecure
    {
        private const string NUMBER_OF_CHARS = "At least 8 characters in length, but no more than 32";
        private const string AT_LEAST_ONE_DIGIT = "At least one digit [0-9]";
        private const string AT_LEAST_ONE_LOW = "At least one lowercase character [a-z]";
        private const string AT_LEAST_ONE_UP = "At least one uppercase character [A-Z]";
        private const string AT_LEAST_ONE_SPEC = @"At least one special character [*.!@#$%^&(){}[]:;<>,.?/~_+-=|\];";

        private readonly SHA256 sha256Hash;

        private byte[] Key { get; }
        private byte[] IV { get; }

        private readonly List<PasswordRule> rules;

        public IdentittVaultSecure(string key, string iv)
        {
            sha256Hash = SHA256.Create();
            Key = Convert.FromBase64String(key);
            IV = Convert.FromBase64String(iv);
            rules = new();
            PopulateRules();
        }

        public string ComputeSha256Hash(byte[] plainData)
        {
            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(plainData);

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }

        public string DecryptionWithRSAPrivateKey(string cipherData, ReadOnlySpan<byte> RSAKey, bool DoOAEPadding)
        {
            byte[] plainData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportRSAPrivateKey(RSAKey, out int bytesRead);
                plainData = RSA.Decrypt(Convert.FromBase64String(cipherData), DoOAEPadding);
            }
            return Encoding.UTF8.GetString(plainData);
        }

        public string ComputeSha256Hash(string plainData)
        {
            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plainData));

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }

        public byte[] EncryptWithAES(byte[] plainData)
        {
            // Check arguments.
            if (plainData == null || plainData.Length <= 0)
                throw new ArgumentNullException("plainData");

            byte[] encrypted;

            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(Convert.ToBase64String(plainData));
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        public byte[] DecryptWithAES(byte[] cipherData)
        {
            // Check arguments.
            if (cipherData == null || cipherData.Length <= 0)
                throw new ArgumentNullException("cipherData");

            // Declare the string used to hold
            // the decrypted text.
            byte[] plainData = null;

            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plainData = Convert.FromBase64String(srDecrypt.ReadToEnd());
                        }
                    }
                }
            }

            return plainData;
        }

        public Dictionary<string, byte[]> GenerateRSAKeys()
        {
            Dictionary<string, byte[]> keys = new();

            using(RSA rsa = RSA.Create())
            {
                keys.Add("public", rsa.ExportRSAPublicKey());
                keys.Add("private", rsa.ExportRSAPrivateKey());
            }

            return keys;
        }

        public string EncryptionWithRSAPublicKey(string plainData, ReadOnlySpan<byte> RSAKey, bool DoOAEPPadding)
        {
            byte[] encryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportRSAPublicKey(RSAKey, out int bytesRead);
                encryptedData = RSA.Encrypt(Encoding.UTF8.GetBytes(plainData), DoOAEPPadding);
            }
            return Convert.ToBase64String(encryptedData);
        }

        public PasswordResult ValidatePasswordStrength(string password)
        {
            PasswordResult passwordResult = new();

            rules.ForEach(rule =>
            {
                if (!rule.Match(password))
                    passwordResult.AddError(rule.Name, rule.Message);
            });

            return passwordResult;
        }

        private void PopulateRules()
        {
            rules.Add(new PasswordRule("PasswordLength", NUMBER_OF_CHARS, password => password.Length >= 8 && password.Length <= 32));
            rules.Add(new PasswordRule("PasswordAtLeastOneDigit", AT_LEAST_ONE_DIGIT, pattern: ".*\\d.*"));
            rules.Add(new PasswordRule("PasswordAtLeastOneLowCase", AT_LEAST_ONE_LOW, pattern: ".*[a-z].*"));
            rules.Add(new PasswordRule("PasswordAtLeastOneUpCase", AT_LEAST_ONE_UP, pattern: ".*[A-Z].*"));
            rules.Add(new PasswordRule("PasswordAtLeastOneSpecialChar", AT_LEAST_ONE_SPEC, pattern: ".*[[*.!@#$%^&(){}:\";'<>,.?/~`_+=|\\]].*"));
        }
    }
}
