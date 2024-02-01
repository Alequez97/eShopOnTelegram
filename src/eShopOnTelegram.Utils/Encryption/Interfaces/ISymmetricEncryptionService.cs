namespace eShopOnTelegram.Utils.Encryption.Interfaces;

public interface ISymmetricEncryptionService
{
	string Encrypt(string plainText);
	string Decrypt(string cipherText);
}
