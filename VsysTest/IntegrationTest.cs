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
        private const long MIN_TEST_BALANCE = 3 * Blockchain.V_UNITY;
        private Blockchain chain;
        private Account account;
        private string recipient;
        private string leaseId;

        [TestInitialize]
        public void Init()
        {
            chain = new Blockchain(TestConfig.NETWORK, TestConfig.NODE_ADDRESS);
            account = new Account(TestConfig.NETWORK, TestConfig.SEED, TestConfig.ACCOUNT_INDEX);
            recipient = TestConfig.RECIPIENT;
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (leaseId != null)
            {
                CancelLease(leaseId);
            }
        }

        [TestMethod()]
        public void TestPayment()
        {
            CheckBalance();
            long amount = 1 * Blockchain.V_UNITY;
            PaymentTransaction tx = TransactionFactory.BuildPaymentTx(recipient, amount);
            ProvenTransaction result = account.SendTransaction(chain, tx);
            Assert.IsNotNull(result.Id);
            Assert.IsTrue(WaitPackageOnChain(result.Id));
        }

        private void CheckBalance()
        {
            long? balance = account.GetBalance(chain);
            Assert.IsTrue(balance.HasValue);
            Assert.IsTrue(balance.Value > MIN_TEST_BALANCE, "No enough balance for runing the whole test cases. You can get test coins by faucet in https://testexplorer.v.systems/");
        }

        [TestMethod()]
        public void TestLease()
        {
            CheckBalance();
            long amount = 1 * Blockchain.V_UNITY;
            LeaseTransaction tx = TransactionFactory.BuildLeaseTx(recipient, amount);
            ProvenTransaction result = account.SendTransaction(chain, tx);
            Assert.IsNotNull(result.Id);
            Assert.IsTrue(WaitPackageOnChain(result.Id));
            leaseId = result.Id;
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