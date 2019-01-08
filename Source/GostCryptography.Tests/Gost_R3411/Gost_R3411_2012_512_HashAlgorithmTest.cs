using System.IO;
using System.Text;

using GostCryptography.Base;
using GostCryptography.Gost_R3411;



namespace GostCryptography.Tests.Gost_R3411
{
	/// <summary>
	/// Вычисление хэша в соответствии с ГОСТ Р 34.11-2012/512.
	/// </summary>
	/// <remarks>
	/// Тест создает поток байт, вычисляет хэш в соответствии с ГОСТ Р 34.11-2012/512 и проверяет его корректность.
	/// </remarks>
	public class Gost_R3411_2012_512_HashAlgorithmTest
	{
		[Theory]
		[MemberData(typeof(TestConfig), nameof(TestConfig.Providers))]
		public void ShouldComputeHash(ProviderType providerType)
		{
			// Given
			var dataStream = CreateDataStream();

			// When

			byte[] hashValue;

			using (var hash = new Gost_R3411_2012_512_HashAlgorithm(providerType))
			{
				hashValue = hash.ComputeHash(dataStream);
			}

			// Then
			Assert.IsNotNull(hashValue);
			Assert.AreEqual(512, 8 * hashValue.Length);
		}

		private static Stream CreateDataStream()
		{
			// Некоторый поток байт

			return new MemoryStream(Encoding.UTF8.GetBytes("Some data to hash..."));
		}
	}
}