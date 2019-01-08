﻿using System.IO;
using System.Text;

using GostCryptography.Base;
using GostCryptography.Gost_R3411;

using Xunit;


namespace GostCryptography.Tests.Gost_R3411
{
	/// <summary>
	/// Вычисление хэша в соответствии с ГОСТ Р 34.11-2012/256.
	/// </summary>
	/// <remarks>
	/// Тест создает поток байт, вычисляет хэш в соответствии с ГОСТ Р 34.11-2012/256 и проверяет его корректность.
	/// </remarks>
	public class Gost_R3411_2012_256_HashAlgorithmTest
	{
		[Theory]
		[MemberData(nameof(TestConfig.XProviders), MemberType = typeof(TestConfig))]
		public void ShouldComputeHash(ProviderType providerType)
		{
			// Given
			var dataStream = CreateDataStream();

			// When

			byte[] hashValue;

			using (var hash = new Gost_R3411_2012_256_HashAlgorithm(providerType))
			{
				hashValue = hash.ComputeHash(dataStream);
			}

			// Then
			Assert.NotNull(hashValue);
			Assert.Equal(256, 8 * hashValue.Length);
		}

		private static Stream CreateDataStream()
		{
			// Некоторый поток байт

			return new MemoryStream(Encoding.UTF8.GetBytes("Some data to hash..."));
		}
	}
}