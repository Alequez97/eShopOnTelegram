using System.Security.Cryptography;
using System.Text;

using eShopOnTelegram.Utils.Encryption.Interfaces;

namespace eShopOnTelegram.Utils.Encryption.Services;

public class AESEncryptionService : ISymmetricEncryptionService
{
	private readonly byte[] _encryptionKeyBytes;

	public AESEncryptionService(string encryptionKey)
	{
		_encryptionKeyBytes = new byte[32]; // AES-256 key size
		Array.Copy(Encoding.UTF8.GetBytes(encryptionKey), _encryptionKeyBytes, Math.Min(encryptionKey.Length, _encryptionKeyBytes.Length));
	}

	public string Encrypt(string plainText)
	{
		using (Aes aesEncryptionAlghoritm = Aes.Create())
		{
			aesEncryptionAlghoritm.Key = _encryptionKeyBytes;
			aesEncryptionAlghoritm.IV = new byte[16]; // Initialization vector should be unique and unpredictable, but for simplicity, use a zero-filled array.

			ICryptoTransform encryptor = aesEncryptionAlghoritm.CreateEncryptor(aesEncryptionAlghoritm.Key, aesEncryptionAlghoritm.IV);
			using (MemoryStream msEncrypt = new MemoryStream())
			{
				using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
				{
					using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
					{
						swEncrypt.Write(plainText);
					}
				}

				return Convert.ToBase64String(msEncrypt.ToArray());
			}
		}
	}

	public string Decrypt(string cipherText)
	{
		byte[] cipherBytes = Convert.FromBase64String(cipherText);

		using (Aes aesEncryptionAlghoritm = Aes.Create())
		{
			aesEncryptionAlghoritm.Key = _encryptionKeyBytes;
			aesEncryptionAlghoritm.IV = new byte[16]; // Initialization vector should be same as what was used during encryption.

			ICryptoTransform decryptor = aesEncryptionAlghoritm.CreateDecryptor(aesEncryptionAlghoritm.Key, aesEncryptionAlghoritm.IV);

			using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
			{
				using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				{
					using (StreamReader srDecrypt = new StreamReader(csDecrypt))
					{
						return srDecrypt.ReadToEnd();
					}
				}
			}
		}
	}
}
