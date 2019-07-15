using Microsoft.VisualStudio.TestTools.UnitTesting;
using v.systems.transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using v.systems.utils;

namespace v.systems.Tests
{
    [TestClass()]
    public class BytesSerializableTransactionTest
    {
        [TestMethod()]
        public void ToBytesTest()
        {
            PaymentTransaction tx = TransactionFactory.BuildPaymentTx("AU6GsBinGPqW8zUuvmjgwpBNLfyyTU3p83Q", 1000000000L, "HXRC", 1547722056762119200L);
            byte[] expect = { 0x02, 0x15, 0x7a, 0x9d, 0x02, 0xac, 0x57, 0xd4, 0x20, 0x00, 0x00, 0x00, 0x00, 0x3b, 0x9a, 0xca, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x98, 0x96, 0x80, 0x00, 0x64, 0x05, 0x54, 0x9c, 0x6d, 0xf7, 0xb3, 0x76, 0x77, 0x1b, 0x19, 0xff, 0x3b, 0xdb, 0x58, 0xd0, 0x4b, 0x49, 0x99, 0x91, 0x66, 0x3c, 0x47, 0x44, 0x4e, 0x42, 0x5f, 0x00, 0x03, 0x31, 0x32, 0x33 };
            string expectHex = BytesHelper.ToHex(expect);
            byte[] actual = tx.ToBytes();
            IList<byte> actual2 = tx.ToByteList();
            Assert.AreEqual(expect.Length, actual.Length, "Bytes length must the same.");
            Assert.AreEqual(expect.Length, actual2.Count, "Bytes length must the same.");
            Assert.AreEqual(expectHex, BytesHelper.ToHex(actual), "Hex Bytes must the same.");
            Assert.AreEqual(expectHex, BytesHelper.ToHex(actual2), "Hex Bytes must the same.");
        }
    }
}