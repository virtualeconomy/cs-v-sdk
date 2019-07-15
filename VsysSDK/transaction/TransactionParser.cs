using Newtonsoft.Json;
using v.systems.type;

namespace v.systems.transaction
{
    public class TransactionParser
    {
        public static ITransaction Parse(string json)
        {
            ITransaction tx = JsonConvert.DeserializeObject<UnknownTransaction>(json);
            TransactionType txType = (TransactionType)tx.Type;
            switch (txType)
            {
                case TransactionType.Payment:
                    tx = JsonConvert.DeserializeObject<PaymentTransaction>(json);
                    break;
                case TransactionType.Lease:
                    tx = JsonConvert.DeserializeObject<LeaseTransaction>(json);
                    break;
                case TransactionType.LeaseCancel:
                    tx = JsonConvert.DeserializeObject<LeaseCancelTransaction>(json);
                    break;
                case TransactionType.Minting:
                    tx = JsonConvert.DeserializeObject<MintingTransaction>(json);
                    break;
            }
            return tx;
        }
    }

}