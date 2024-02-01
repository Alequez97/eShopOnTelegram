namespace eShopOnTelegram.Utils.Encryption.Interfaces;

public interface IEncryptionService
{
	string Encrypt(string plainText);
	string Decrypt(string cipherText);
}
