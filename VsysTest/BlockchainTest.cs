using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using v.systems.transaction;
using v.systems.error;

namespace v.systems.Tests
{
    [TestClass()]
    public class BlockchainTest
    {
        private Blockchain chain;

        [TestInitialize]
        public void Init()
        {
            chain = new Blockchain(TestConfig.NETWORK, TestConfig.NODE_ADDRESS);
        }

        [TestMethod()]
        public void GetTransactionHistoryFailedCaseTest()
        {
            string address = TestConfig.RECIPIENT;
            try
            {
                IList<ITransaction> txList = chain.GetTransactionHistory(address, 100000);
                Assert.Fail("There should have Exception because query tx num is greater than upper limitation.");
            } catch (ApiError ex)
            {
                Assert.AreEqual(10, ex.Error);
                Assert.IsNotNull(ex.Message, "There should have error detailed message feedback to user.");
            }
        }

        [TestMethod()]
        public void GetTransactionHistoryTest()
        {
            string address = TestConfig.RECIPIENT;
            IList<ITransaction> txList = chain.GetTransactionHistory(address, 10);
            Assert.AreEqual(txList.Count, 10);
            foreach (ITransaction tx in txList)
            {
                Assert.IsNotNull(tx.Id, "Failed to get Tx Id");
                Assert.IsNotNull(tx.Height, "Failed to get Tx Height (Id: " + tx.Id + ")");
                Assert.IsNotNull(tx.Status, "Failed to get Tx Status (Id: " + tx.Id + ")");
                if (tx is ProvenTransaction)
                {
                    ProvenTransaction pvtx = tx as ProvenTransaction;
                    Assert.IsNotNull(pvtx.Proofs, "Failed to get Tx Proofs (Id: " + tx.Id + ")");
                    Assert.IsTrue(pvtx.Proofs.Count > 0, "Tx Proofs should more than one (Id: " + tx.Id + ")");
                    Assert.IsNotNull(pvtx.Proofs[0].PublicKey, "Failed to get Tx PublicKey (Id: " + tx.Id + ")");
                    Assert.IsNotNull(pvtx.Proofs[0].Signature, "Failed to get Tx Signature (Id: " + tx.Id + ")");
                }
                if (tx is PaymentTransaction)
                {
                    PaymentTransaction ptx = tx as PaymentTransaction;
                    Assert.IsNotNull(ptx.Recipient, "Failed to get Recipient for Payment Tx (Id: " + tx.Id + ")");
                    Assert.IsNotNull(ptx.Amount, "Failed to get Amount for Payment Tx (Id: " + tx.Id + ")");
                    Assert.IsNotNull(ptx.Attachment, "Failed to get Attachment for Payment Tx (Id: " + tx.Id + ")");
                }
                if (tx is LeaseTransaction)
                {
                    LeaseTransaction ltx = tx as LeaseTransaction;
                    Assert.IsNotNull(ltx.Recipient, "Failed to get Recipient for Lease Tx (Id: " + tx.Id + ")");
                    Assert.IsNotNull(ltx.Amount, "Failed to get Amount for Lease Tx (Id: " + tx.Id + ")");
                }
                if (tx is LeaseCancelTransaction)
                {
                    LeaseCancelTransaction lctx = tx as LeaseCancelTransaction;
                    Assert.IsNotNull(lctx.LeaseId, "Failed to get LeaseId for LeaseCancel Tx (Id: " + tx.Id + ")");
                    Assert.IsNotNull(lctx.Lease, "Failed to get Lease Info for LeaseCancel Tx (Id: " + tx.Id + ")");
                    Assert.IsNotNull(lctx.Lease.Id, "Failed to get Id of Lease Info for LeaseCancel Tx (Id: " + tx.Id + ")");
                    Assert.IsNotNull(lctx.Lease.Recipient, "Failed to get Recipient of Lease Info for LeaseCancel Tx (Id: " + tx.Id + ")");
                    Assert.IsNotNull(lctx.Lease.Amount, "Failed to get Amount of Lease Info for LeaseCancel Tx (Id: " + tx.Id + ")");
                }
                if (tx is MintingTransaction)
                {
                    MintingTransaction mtx = tx as MintingTransaction;
                    Assert.IsNotNull(mtx.Recipient, "Failed to get Recipient for Minting Tx (Id: " + tx.Id + ")");
                    Assert.IsNotNull(mtx.Amount, "Failed to get Amount for Minting Tx (Id: " + tx.Id + ")");
                }
                if (!TestConfig.ACCEPT_UNKNOWN_TX)
                {
                    Assert.IsNotInstanceOfType(tx, typeof(UnknownTransaction), "Failed to parse Tx Type (Id: " + tx.Id + ")");
                }
            }
        }
    }
}