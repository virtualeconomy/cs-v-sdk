using Newtonsoft.Json.Linq;
using v.systems.type;

namespace v.systems.transaction
{
    public class LeaseTransaction : ProvenTransaction
    {
        public readonly string[] BYTE_SERIALIZED_FIELDS = new string[] { "Type", "Recipient", "Amount", "Fee", "FeeScale", "Timestamp" };

        protected internal string recipient;
        protected internal long amount;

        public LeaseTransaction()
        {
            type = (byte)TransactionType.Lease;
        }

        public override JObject ToAPIRequestJson(string publicKey, string signature)
        {
            JObject json = base.ToAPIRequestJson(publicKey, signature);
            json["amount"] = this.amount;
            json["recipient"] = this.recipient;
            return json;
        }

        public override JObject ToColdSignJson(string publicKey)
        {
            int api = GetColdSignAPIVersion(this.amount);
            JObject json = base.ToColdSignJson(publicKey, api);
            json["amount"] = this.amount;
            json["recipient"] = this.recipient;
            return json;
        }

        protected internal override string[] ByteSerializedFields
        {
            get
            {
                return BYTE_SERIALIZED_FIELDS;
            }
        }

        [Base58Field]
        public string Recipient
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


        public long Amount
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