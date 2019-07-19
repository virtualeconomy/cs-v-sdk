using HashLib;
using Blake2Sharp;
using System.Security.Cryptography;

namespace v.systems.utils
{
    public class Hash
    {
        private static readonly SHA256Managed sha256 = new SHA256Managed();
        private static readonly IHash keccak256 = HashFactory.Crypto.SHA3.CreateKeccak256();

        public static byte[] SHA256(byte[] context)
        {
            return sha256.ComputeHash(context, 0, context.Length);
        }

        public static byte[] SHA256(byte[] context, int offset, int count)
        {
            return sha256.ComputeHash(context, offset, count);
        }

        public static byte[] FastHash(byte[] message, int offset, int length)
        {
            var blakeConfig = new Blake2BConfig { OutputSizeInBits = 256 };
            return Blake2B.ComputeHash(message, offset, length, blakeConfig);
        }

        public static byte[] SecureHash(byte[] context)
        {
            return SecureHash(context, 0, context.Length);
        }

        public static byte[] SecureHash(byte[] message, int offset, int length)
        {
            var blake2B = FastHash(message, offset, length);
            keccak256.Initialize();
            keccak256.TransformBytes(blake2B, 0, blake2B.Length);
            return keccak256.TransformFinal().GetBytes();
        }
    }

}