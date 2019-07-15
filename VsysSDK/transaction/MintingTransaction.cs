using v.systems.type;

namespace v.systems.transaction
{
	public class MintingTransaction : Transaction
	{
		protected internal string recipient;
		protected internal long? amount;

        public MintingTransaction()
        {
            type = (byte)TransactionType.Minting;
        }

        public virtual string Recipient
		{
			get
			{
				return recipient;
			}
			set
			{
				this.recipient = value;
			}
		}


		public virtual long? Amount
		{
			get
			{
				return amount;
			}
			set
			{
				this.amount = value;
			}
		}

	}

}