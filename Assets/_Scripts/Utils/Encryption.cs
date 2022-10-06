using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Security.Cryptography;

namespace Game.Utils
{
    public static class Encryption
    {
		const string DEFAULT_SECRET_KEY = "elperrojugabaconlapelotaXD1234567890";
        public static string Encrypt(string content)
        {
			var b = Encoding.UTF8.GetBytes(content);
			var encrypted = GetAes().CreateEncryptor().TransformFinalBlock(b, 0, b.Length);
			return Convert.ToBase64String(encrypted);
		}

        public static string Decrypt(string encrypted)
        {
			var b = Convert.FromBase64String(encrypted);
			var decrypted = GetAes().CreateDecryptor().TransformFinalBlock(b, 0, b.Length);
			return Encoding.UTF8.GetString(decrypted);
		}

		static Aes GetAes()
		{
			var keyBytes = new byte[16];
			var skeyBytes = Encoding.UTF8.GetBytes(DEFAULT_SECRET_KEY);
			Array.Copy(skeyBytes, keyBytes, Math.Min(keyBytes.Length, skeyBytes.Length));

			Aes aes = Aes.Create();
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;
			aes.KeySize = 128;
			aes.Key = keyBytes;
			aes.IV = keyBytes;

			return aes;
		}
	}
}
