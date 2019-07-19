using Microsoft.VisualStudio.TestTools.UnitTesting;
using v.systems.type;

namespace v.systems.Tests
{
    [TestClass()]
    public class AccountTest
    {
        [TestMethod()]
        public void CreateAccountTest()
        {
            string seed = "captain slight hurdle bless engage gallery senior wisdom uniform young pretty first glad jar claw";
            Account acc = new Account(NetworkType.Testnet, seed, 0);
            Assert.AreEqual("5uS81kZNHFd1seHaoVoc7bqTRaAUEbtW2e84HrsHUHij", acc.PrivateKey, "Private Key is wrong");
            Assert.AreEqual("3NaHDL4hig5A9Dh3gLbycWnfxpPTBe4WgeAG5AgeXPbL", acc.PublicKey, "Public Key is wrong");
            Assert.AreEqual("AU9fRxUciwuG6JvdnPBPb8BJXvfDn8oxwtn", acc.Address, "Address is wrong");
        }
    }
}