using Newtonsoft.Json.Linq;
using v.systems.type;

namespace v.systems.transaction
{
    public class LeaseCancelTransaction : ProvenTransaction
    {
        public readonly string[] BYTE_SERIALIZED_FIELDS = new string[] { "Type", "Fee", "FeeScale", "Timestamp", "LeaseId" };

        protected internal string leaseId;
        protected internal LeaseTransaction lease;

        public LeaseCancelTransaction()
        {
            type = (byte)TransactionType.LeaseCancel;
        }

        public override JObject ToAPIRequestJson(string publicKey, string signature)
        {
            JObject json = base.ToAPIRequestJson(publicKey, signature);
            json["txId"] = this.leaseId;
            return json;
        }

        public override JObject ToColdSignJson(string publicKey)
        {
            JObject json = base.ToColdSignJson(publicKey);
            json["txId"] = this.leaseId;
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
        public string LeaseId
        {
            get
            {
                return leaseId;
            }
            set
            {
                this.leaseId = value;
            }
        }


        public LeaseTransaction Lease
        {
            get
            {
                return lease;
            }
            set
            {
                this.lease = value;
            }
        }

    }

}