using System;
using System.Linq;

namespace DnsCrypt.Tools
{
	/// <summary>
	///     Helper class for working with arrays.
	/// </summary>
	public static class ArrayHelper
	{
		/// <summary>
		///     Concatenate the given byte arrays.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arrays">The byte arrays.</param>
		/// <returns>The concatenated byte arrays.</returns>
		/// <remarks>code courtesy of @CodesInChaos, public domain</remarks>
		/// <see cref="https://gist.github.com/CodesInChaos/3175971" />
		/// <exception cref="OverflowException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static T[] ConcatArrays<T>(params T[][] arrays)
		{
			checked
			{
				var result = new T[arrays.Sum(arr => arr.Length)];
				var offset = 0;

				foreach (var arr in arrays)
				{
					Buffer.BlockCopy(arr, 0, result, offset, arr.Length);
					offset += arr.Length;
				}

				return result;
			}
		}

		/// <summary>
		///     Concatenate two byte arrays.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arr1">The first byte array.</param>
		/// <param name="arr2">The second byte array.</param>
		/// <returns>The concatenated byte arrays.</returns>
		/// <remarks>code courtesy of @CodesInChaos, public domain</remarks>
		/// <see cref="https://gist.github.com/CodesInChaos/3175971" />
		/// <exception cref="OverflowException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static T[] ConcatArrays<T>(T[] arr1, T[] arr2)
		{
			checked
			{
				var result = new T[arr1.Length + arr2.Length];
				Buffer.BlockCopy(arr1, 0, result, 0, arr1.Length);
				Buffer.BlockCopy(arr2, 0, result, arr1.Length, arr2.Length);

				return result;
			}
		}

		/// <summary>
		///     Extract a part of a byte array from another byte array.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arr">A byte array.</param>
		/// <param name="start">Position to start extraction.</param>
		/// <param name="length">The length of the extraction started at start.</param>
		/// <returns>A part with the given length of the byte array.</returns>
		/// <remarks>code courtesy of @CodesInChaos, public domain</remarks>
		/// <see cref="https://gist.github.com/CodesInChaos/3175971" />
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static T[] SubArray<T>(T[] arr, int start, int length)
		{
			var result = new T[length];
			Buffer.BlockCopy(arr, start, result, 0, length);

			return result;
		}

		/// <summary>
		///     Extract a part of a byte array from another byte array.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arr">A byte array.</param>
		/// <param name="start">Position to start extraction.</param>
		/// <returns>A part of the given byte array.</returns>
		/// <remarks>code courtesy of @CodesInChaos, public domain</remarks>
		/// <see cref="https://gist.github.com/CodesInChaos/3175971" />
		/// <exception cref="OverflowException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public static T[] SubArray<T>(T[] arr, int start)
		{
			return SubArray(arr, start, arr.Length - start);
		}

		/// <summary>
		///     Constant-time comparison of two byte arrays.
		/// </summary>
		/// <param name="a">The first byte array.</param>
		/// <param name="b">The second byte array.</param>
		/// <returns><c>true</c> if valid; otherwise, <c>false</c>.</returns>
		/// <exception cref="OverflowException"></exception>
		public static bool ConstantTimeEquals(byte[] a, byte[] b)
		{
			var diff = (uint)a.Length ^ (uint)b.Length;
			for (var i = 0; i < a.Length && i < b.Length; i++)
				diff |= (uint)(a[i] ^ b[i]);
			return diff == 0;
		}
	}
}
