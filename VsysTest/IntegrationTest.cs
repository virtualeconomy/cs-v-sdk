using Microsoft.VisualStudio.TestTools.UnitTesting;
using v.systems.transaction;
using System;
using v.systems.error;
using System.Threading;

namespace v.systems.Tests
{
    [TestClass()]
    public class IntegrationTest
    {
        private Blockchain chain;
        private Account account;
        private string recipient;

        [TestInitialize]
        public void Init()
        {
            chain = new Blockchain(TestConfig.NETWORK, TestConfig.NODE_ADDRESS);
            account = new Account(TestConfig.NETWORK, TestConfig.SEED, TestConfig.ACCOUNT_INDEX);
            recipient = TestConfig.RECIPIENT;
        }

        [TestMethod()]
        public void TestPayment()
        {
            long amount = 1 * Blockchain.V_UNITY;
            PaymentTransaction tx = TransactionFactory.BuildPaymentTx(recipient, amount);
            ProvenTransaction result = account.SendTransaction(chain, tx);
            Assert.IsNotNull(result.Id);
            Assert.IsTrue(WaitPackageOnChain(result.Id));
        }

        [TestMethod()]
        public void TestLease()
        {
            long amount = 1 * Blockchain.V_UNITY;
            LeaseTransaction tx = TransactionFactory.BuildLeaseTx(recipient, amount);
            ProvenTransaction result = account.SendTransaction(chain, tx);
            Assert.IsNotNull(result.Id);
            Assert.IsTrue(WaitPackageOnChain(result.Id));
        }

        [TestMethod()]
        public void CancelLease()
        {
            CancelLease("<Please input lease id>");
        }

        public void CancelLease(string txId)
        {
            LeaseCancelTransaction tx = TransactionFactory.BuildCancelLeasingTx(txId);
            ProvenTransaction result = account.SendTransaction(chain, tx);
            Assert.IsNotNull(result.Id);
            Assert.IsTrue(WaitPackageOnChain(result.Id));
        }

        private bool WaitPackageOnChain(string txId)
        {
            int time = 0;
            ITransaction result = null;
            while (result == null && time < TestConfig.WAIT_BLOCK_TIMEOUT)
            {
                try
                {
                    result = chain.GetTransactionById(txId);
                } catch (TransactionError)
                {
                    Console.Out.WriteLine("Wait for supernode packaging tx ...");
                    Thread.Sleep(3000);
                    time += 3;
                }
            }
            return result != null && result.Id != null;
        }
    }
}