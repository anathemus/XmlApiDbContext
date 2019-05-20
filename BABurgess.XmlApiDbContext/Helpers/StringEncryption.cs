using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace BABurgess.XmlApiDbContext.Helpers
{
    public static class StringEncryption
    {
        public static string EncryptString(string encryptString, string userPassHash)
        {
            string EncryptionKey = userPassHash;
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public static string DecryptString(string cipherText, string userPassHash)
        {
            string EncryptionKey = userPassHash;
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string EmbedUserString(string encryptedString, string passwordHash)
        {
            StringBuilder builder = new StringBuilder(encryptedString);
            builder.Insert((builder.Length / 3), "&" + passwordHash + "&");

            return builder.ToString();
        }

        public static string DecryptUserAccount(string encryptedString, string passwordHash)
        {
            List<string> stringParts = new List<string>();
            string xmlAgain = String.Empty;
            stringParts.AddRange(encryptedString.Split('&'));
            if (stringParts[1] == passwordHash)
            {
                xmlAgain = StringEncryption.DecryptString((stringParts[0] + stringParts[2]), passwordHash);
            }
            return xmlAgain;
        }
    }
}