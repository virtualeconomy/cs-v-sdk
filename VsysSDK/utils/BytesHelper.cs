using System;
using System.Collections.Generic;
using System.Text;

namespace v.systems.utils
{
	public class BytesHelper
	{
        public const int LONG_BYTES = 8;
        public const int INT_BYTES = 4;
        public const int SHORT_BYTES = 2;

        private static byte[] ToBigEndian(byte[] arr)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(arr);
            }
            return arr;
        }

        public static byte[] ToBytes(long x)
        {
            byte[] arr = BitConverter.GetBytes(x);
            return ToBigEndian(arr);
        }

        public static byte[] ToBytes(int x)
		{
            byte[] arr = BitConverter.GetBytes(x);
            return ToBigEndian(arr);
        }

		public static byte[] ToBytes(short x)
		{
            byte[] arr = BitConverter.GetBytes(x);
            return ToBigEndian(arr);
        }

		public static byte[] ToBytes(byte x)
		{
			return new byte[]{x};
		}

		public static byte[] ToBytes(string x)
		{
			return Encoding.UTF8.GetBytes(x);
		}

		public static byte[] SerializeBase58(string base58Str)
		{
			return Base58.Decode(base58Str);
		}

		public static byte[] SerializeBase58WithSize(string base58Str, int byteLengthOfSize)
		{
			byte[] b58decode = Base58.Decode(base58Str);
			byte[] intSizeBytes = ToBytes(b58decode.Length);
			byte[] sizeBytes = new byte[byteLengthOfSize];
			for (int i = 1; i <= byteLengthOfSize; i++)
			{
				int destOffset = byteLengthOfSize - i;
				int srcOffset = INT_BYTES - i;
				sizeBytes[destOffset] = srcOffset >= 0 ? intSizeBytes[srcOffset] : (byte)0;
			}
			return Concat(sizeBytes, b58decode);
		}

		public static byte[] ToBytes(IList<byte> list)
		{
			byte[] bytes = new byte[list.Count];
			for (int i = 0 ; i < list.Count; i++)
			{
				bytes[i] = list[i];
			}
			return bytes;
		}

		public static string ToHex(byte[] bytes)
		{
			StringBuilder sb = new StringBuilder();
			foreach (byte b in bytes)
			{
				sb.Append(string.Format("{0:x2} ", b));
			}
			return sb.ToString();
		}

		public static string ToHex(IList<byte> bytes)
		{
			StringBuilder sb = new StringBuilder();
			foreach (byte? b in bytes)
			{
				sb.Append(string.Format("{0:x2} ", b));
			}
			return sb.ToString();
		}

		public static byte[] Concat(byte[] a, byte[] b)
		{
			byte[] result = new byte[a.Length + b.Length];
			Array.Copy(a, 0, result, 0, a.Length);
			Array.Copy(b, 0, result, a.Length, b.Length);
			return result;
		}
	}

}