using System;

namespace v.systems.transaction
{

	public class TransactionFactory
	{

		private const long PAYMENT_FEE= 10000000L;
        private const long LEASE_FEE = 10000000L;
        private const long CANCEL_LEASE_FEE = 10000000L;
        private const short DEFAULT_FEE_SCALE = 100;

		public static PaymentTransaction BuildPaymentTx(string recipient, long amount)
		{
			return BuildPaymentTx(recipient, amount, "");
		}

		public static PaymentTransaction BuildPaymentTx(string recipient, long amount, string attachment)
		{
			return BuildPaymentTx(recipient, amount, attachment, CurrentTime);
		}

		public static PaymentTransaction BuildPaymentTx(string recipient, long amount, string attachment, long timestamp)
		{
			PaymentTransaction tx = new PaymentTransaction();
			tx.Recipient = recipient;
			tx.Amount = amount;
			tx.Attachment = attachment;
			tx.Timestamp = timestamp;
            tx.Fee = PAYMENT_FEE;
            tx.FeeScale = DEFAULT_FEE_SCALE;
            return tx;
		}

		public static LeaseTransaction BuildLeaseTx(string recipient, long amount)
		{
			return BuildLeaseTx(recipient, amount, CurrentTime);
		}

		public static LeaseTransaction BuildLeaseTx(string recipient, long amount, long timestamp)
		{
			LeaseTransaction tx = new LeaseTransaction();
			tx.Recipient = recipient;
			tx.Amount = amount;
			tx.Timestamp = timestamp;
            tx.Fee = LEASE_FEE;
            tx.FeeScale = DEFAULT_FEE_SCALE;
            return tx;
		}

		public static LeaseCancelTransaction BuildCancelLeasingTx(string leaseId)
		{
			return BuildCancelLeasingTx(leaseId, CurrentTime);
		}

		public static LeaseCancelTransaction BuildCancelLeasingTx(string leaseId, long timestamp)
		{
			LeaseCancelTransaction tx = new LeaseCancelTransaction();
			tx.LeaseId = leaseId;
			tx.Timestamp = timestamp;
            tx.Fee = CANCEL_LEASE_FEE;
            tx.FeeScale = DEFAULT_FEE_SCALE;
			return tx;
		}

		private static long CurrentTime
		{
			get
			{
                long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                return unixTimestamp * 1000000000L; // to nano seconds
			}
		}
	}

}