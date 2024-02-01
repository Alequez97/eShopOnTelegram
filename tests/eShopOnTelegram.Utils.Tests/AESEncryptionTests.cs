using eShopOnTelegram.Utils.Encryption.Interfaces;
using eShopOnTelegram.Utils.Encryption.Services;

using FluentAssertions;

namespace eShopOnTelegram.Utils.Tests;

public class Tests
{
	private IEncryptionService _encryptionService;

	[SetUp]
	public void Setup()
	{
		var encryptionKey = "very-secret-encryption-key";
		_encryptionService = new AESEncryptionService(encryptionKey);
	}

	[Test]
	public void AESSymmetricEncryptionEncryptedAndDecryptedValuesAreSame()
	{
		// Arrange
		var textToEncrypt = "Very important text, that should not be stolen";

		// Act
		var encryptedText = _encryptionService.Encrypt(textToEncrypt);
		var decpryptedText = _encryptionService.Decrypt(encryptedText);

		// Assert
		textToEncrypt.Should().Be(decpryptedText);
	}
}
