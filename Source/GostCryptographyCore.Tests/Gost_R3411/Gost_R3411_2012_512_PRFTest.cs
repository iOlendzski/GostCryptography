﻿using System.Text;

using GostCryptography.Base;
using GostCryptography.Gost_28147_89;
using GostCryptography.Gost_R3411;

using Xunit;


namespace GostCryptography.Tests.Gost_R3411
{
	/// <summary>
	/// Использование PRF на базе алгоритма хэширования ГОСТ Р 34.11-2012/512.
	/// </summary>
	public class Gost_R3411_2012_512_PRFTest
	{
		private static readonly byte[] Label = { 1, 2, 3, 4, 5 };
		private static readonly byte[] Seed = { 6, 7, 8, 9, 0 };
		private static readonly byte[] TestData = Encoding.UTF8.GetBytes("Some data to encrypt...");


		[Theory]
		[MemberData(nameof(TestConfig.XProviders), MemberType = typeof(TestConfig))]
		public void ShouldDeriveBytes(ProviderType providerType)
		{
			// Given
			var initKey = new Gost_28147_89_SymmetricAlgorithm(providerType);

			// When
			byte[] randomBytes1;
			byte[] randomBytes2;
			byte[] randomBytes3;

			using (var prf = new Gost_R3411_2012_512_PRF(initKey, Label, Seed))
			{
				randomBytes1 = prf.DeriveBytes();
				randomBytes2 = prf.DeriveBytes();
				randomBytes3 = prf.DeriveBytes();
			}

			// Then
			Assert.NotNull(randomBytes1);
			Assert.NotNull(randomBytes2);
			Assert.NotNull(randomBytes3);
			Assert.Equal(512, 8 * randomBytes1.Length);
			Assert.Equal(512, 8 * randomBytes2.Length);
			Assert.Equal(512, 8 * randomBytes3.Length);
			Assert.NotEqual(randomBytes1, randomBytes2);
			Assert.NotEqual(randomBytes1, randomBytes3);
			Assert.NotEqual(randomBytes2, randomBytes3);
		}

		[Theory]
		[MemberData(nameof(TestConfig.XProviders), MemberType = typeof(TestConfig))]
		public void ShouldDeriveKey(ProviderType providerType)
		{
			// TODO: VipNet does not support this feature - https://infotecs.ru/forum/topic/10142-oshibka-pri-sozdanii-klyucha-shifrovaniya-na-osnove-dannyih-polzovatelya-cryptderivekey/
			Skip.If(providerType.IsVipNet(), "VipNet does not support this feature");

			// Given
			var initKey = new Gost_28147_89_SymmetricAlgorithm(providerType);

			// When
			Gost_28147_89_SymmetricAlgorithmBase randomKey1;
			Gost_28147_89_SymmetricAlgorithmBase randomKey2;
			Gost_28147_89_SymmetricAlgorithmBase randomKey3;

			using (var prf = new Gost_R3411_2012_512_PRF(initKey, Label, Seed))
			{
				randomKey1 = prf.DeriveKey();
				randomKey2 = prf.DeriveKey();
				randomKey3 = prf.DeriveKey();
			}

			// Then
			Assert.NotNull(randomKey1);
			Assert.NotNull(randomKey2);
			Assert.NotNull(randomKey3);
			AssertKeyIsValid(randomKey1);
			AssertKeyIsValid(randomKey2);
			AssertKeyIsValid(randomKey3);
			AssertKeysAreNotEqual(randomKey1, randomKey2);
			AssertKeysAreNotEqual(randomKey1, randomKey3);
			AssertKeysAreNotEqual(randomKey2, randomKey3);
		}
		
		public static void AssertKeyIsValid(Gost_28147_89_SymmetricAlgorithmBase key)
		{
			var encryptedData = EncryptData(key, TestData);
			var decryptedData = DecryptData(key, encryptedData);
			Assert.Equal(TestData, decryptedData);
		}

		public static void AssertKeysAreNotEqual(Gost_28147_89_SymmetricAlgorithmBase key1, Gost_28147_89_SymmetricAlgorithmBase key2)
		{
			var encryptedData1 = EncryptData(key1, TestData);
			var encryptedData2 = EncryptData(key2, TestData);
			Assert.NotEqual(encryptedData1, encryptedData2);
		}
		
		public static byte[] EncryptData(Gost_28147_89_SymmetricAlgorithmBase key, byte[] data)
		{
			var transform = key.CreateEncryptor();
			return transform.TransformFinalBlock(data, 0, data.Length);
		}

		public static byte[] DecryptData(Gost_28147_89_SymmetricAlgorithmBase key, byte[] data)
		{
			var transform = key.CreateDecryptor();
			return transform.TransformFinalBlock(data, 0, data.Length);
		}
	}
}