using System;
using System.Security.Cryptography;

namespace Keysharp.Core
{
	public static class Security
	{
		/// <summary>
		/// Calculates the CRC32 polynomial of an object.
		/// </summary>
		/// <param name="value">The object to check.</param>
		/// <returns>A checksum of <paramref name="value"/> as an integer.</returns>
		public static long CRC32(object value)
		{
			var raw = Crypt.ToByteArray(value.ParseObject());
			var alg = new Common.CRC32();
			_ = alg.ComputeHash(raw);
			return alg.Value;
		}

		/// <summary>
		/// Encrypt or decrypt data with the Data Encryption Standard (DES) algorithm.
		/// </summary>
		/// <param name="value">The data to encrypt or decrypt.</param>
		/// <param name="key">The secret key.</param>
		/// <param name="decrypt"><code>true</code> to decrypt the given <paramref name="value"/>, otherwise encrypt.</param>
		/// <returns>The corresponding encrypted or decrypted data.</returns>
		/// <remarks>A key length of 64 bits is supported.</remarks>
		public static byte[] DES(object value, object key, bool decrypt = false)
		{
			return Crypt.Encrypt(value.ParseObject(), key.ParseObject(), decrypt, new DESCryptoServiceProvider());
		}

		/// <summary>
		/// Calculates the MD5 hash of an object.
		/// </summary>
		/// <param name="value">The object to hash.</param>
		/// <returns>A 32-character hexadecimal number.</returns>
		public static string MD5(object value)
		{
			return Crypt.Hash(value.ParseObject(), new MD5CryptoServiceProvider());
		}

		/// <summary>
		/// Encrypt or decrypt data with the RC2 algorithm.
		/// </summary>
		/// <param name="value">The data to encrypt or decrypt.</param>
		/// <param name="key">The secret key.</param>
		/// <param name="decrypt"><code>true</code> to decrypt the given <paramref name="value"/>, otherwise encrypt.</param>
		/// <returns>The corresponding encrypted or decrypted data.</returns>
		/// <remarks>Key lengths from 40 bits to 128 bits in increments of 8 bits are supported.</remarks>
		public static byte[] RC2(object value, object key, bool decrypt = false)
		{
			return Crypt.Encrypt(value.ParseObject(), key.ParseObject(), decrypt, new RC2CryptoServiceProvider());
		}

		/// <summary>
		/// Encrypt or decrypt data with the Rijndael algorithm.
		/// </summary>
		/// <param name="value">The data to encrypt or decrypt.</param>
		/// <param name="key">The secret key.</param>
		/// <param name="decrypt"><code>true</code> to decrypt the given <paramref name="value"/>, otherwise encrypt.</param>
		/// <returns>The corresponding encrypted or decrypted data.</returns>
		/// <remarks>Key lengths of 128, 192, or 256 bits are supported.</remarks>
		public static byte[] Rijndael(object value, object key, bool decrypt = false)
		{
			return Crypt.Encrypt(value.ParseObject(), key.ParseObject(), decrypt, new RijndaelManaged());
		}

		/// <summary>
		/// Calculates the RIPEMD160 hash of an object.
		/// </summary>
		/// <param name="value">The object to hash.</param>
		/// <returns>A 40-character hexadecimal number.</returns>
		//public static string RIPEMD160(object value)
		//{
		//  return Crypt.Hash(value, new System.Security.Cryptography.RIPEMD160Managed());//Deprecated, no longer supported.//MATT
		//}

		/// <summary>
		/// Generates a secure (cryptographic) random number.
		/// </summary>
		/// <param name="min">The lower bound.</param>
		/// <param name="max">The upper bound.</param>
		/// <returns>A random number between the specified range. Leave both parameters blank to give any 128-bit numeric result.
		/// If <paramref name="min"/> and <paramref name="max"/> are both pure integers, the result would also be an integer without a remainder.</returns>
		/// <remarks>A cryptographic random number generator produces an output that is computationally infeasible to predict with a probability that is better than one half.
		/// <see cref="Random"/> uses a simpler algorithm which is much faster but less secure.</remarks>
		public static decimal SecureRandom(decimal min = decimal.MinValue, decimal max = decimal.MaxValue)
		{
			var diff = Math.Abs(min - max);

			if (diff == 0 && !(min == 0 && max == 0))
				return min;

			var csp = new RNGCryptoServiceProvider();
			var rnd = new byte[4 * 3 + 1];
			csp.GetBytes(rnd);
			var s = new int[3];

			for (var i = 0; i < s.Length; i++)
				s[i] = BitConverter.ToInt32(rnd, i * 4);

			var rem = decimal.Remainder(min, 1) != 0 || decimal.Remainder(max, 1) != 0;
			byte scale = 0;

			if (rem)
				scale = (byte)(rnd[12] % 28);

			var val = new decimal(s[0], s[1], s[2], false, scale);
			return diff == 0 ? val : min + val % diff;
		}

		/// <summary>
		/// Calculates the SHA1 hash of an object.
		/// </summary>
		/// <param name="value">The object to hash.</param>
		/// <returns>A 40-character hexadecimal number.</returns>
		public static string SHA1(object value)
		{
			return Crypt.Hash(value.ParseObject(), new SHA1CryptoServiceProvider());
		}

		/// <summary>
		/// Calculates the SHA256 hash of an object.
		/// </summary>
		/// <param name="value">The object to hash.</param>
		/// <returns>A 64-character hexadecimal number.</returns>
		public static string SHA256(object value)
		{
			return Crypt.Hash(value.ParseObject(), new SHA256Managed());
		}

		/// <summary>
		/// Calculates the SHA384 hash of an object.
		/// </summary>
		/// <param name="value">The object to hash.</param>
		/// <returns>A 96-character hexadecimal number.</returns>
		public static string SHA384(object value)
		{
			return Crypt.Hash(value.ParseObject(), new SHA384Managed());
		}

		/// <summary>
		/// Calculates the SHA512 hash of an object.
		/// </summary>
		/// <param name="value">The object to hash.</param>
		/// <returns>A 128-character hexadecimal number.</returns>
		public static string SHA512(object value)
		{
			return Crypt.Hash(value.ParseObject(), new SHA512Managed());
		}

		/// <summary>
		/// Encrypt or decrypt data with the Triple Data Encryption Standard (TripleDES) algorithm.
		/// </summary>
		/// <param name="value">The data to encrypt or decrypt.</param>
		/// <param name="key">The secret key.</param>
		/// <param name="decrypt"><code>true</code> to decrypt the given <paramref name="value"/>, otherwise encrypt.</param>
		/// <returns>The corresponding encrypted or decrypted data.</returns>
		/// <remarks>
		/// <para>Three successive iterations of the <see cref="DES"/> algorithm are used, with either two or three 56-bit keys.</para>
		/// <para>Key lengths from 128 bits to 192 bits in increments of 64 bits are supported.</para>
		/// </remarks>
		public static byte[] TripleDES(object value, object key, bool decrypt = false)
		{
			return Crypt.Encrypt(value.ParseObject(), key.ParseObject(), decrypt, new TripleDESCryptoServiceProvider());
		}
	}
}