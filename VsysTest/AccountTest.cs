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

        [TestMethod()]
        public void AddressTest()
        {
            string[] validT = { "AUBmETxBtCBqAJEDTchiFc6KViACXpimpry", "AUCux2tL8UTtaDau5ZpAsbJMz327vkY2YpX", "AU5QiZ6fFKEBXRtn5sNAPmBf1io2Q2NS5pk"};
            string[] validM = { "ARGH3jdgSibN94WZsdZvd4bjG2FAUpCFbW8", "AR6mndppyDaNtE2QygpTWNxWGknhuBs45qq", "ARCvdpy9ndq8VSRyxHH1Sa339Wmjc7g76hj" };
            string[] invalidT = { "123", "AU83FKKzTYCue5ZQPweCzJ68dQE4Htdmv5u", "AUCvVbLeLU5LiLxyYgEYcnccUDxJuk3jmea" };
            string[] invalidM = { "asd", "ARJBYYKTuVSGsh5YoxPmgPuRbCWXsjkscN2", "ARCug95rfqzbSRuYhREudLNVdhui21x97ch" };
            foreach (string address in validT)
            {
                Assert.IsTrue(Account.CheckAddress(NetworkType.Testnet, address), address + " should be valid address");
            }
            foreach (string address in validM)
            {
                Assert.IsTrue(Account.CheckAddress(NetworkType.Mainnet, address), address + " should be valid address");
            }
            foreach (string address in invalidT)
            {
                Assert.IsFalse(Account.CheckAddress(NetworkType.Testnet, address), address + " should be invalid address");
            }
            foreach (string address in invalidM)
            {
                Assert.IsFalse(Account.CheckAddress(NetworkType.Mainnet, address), address + " should be invalid address");
            }
        }
    }
}