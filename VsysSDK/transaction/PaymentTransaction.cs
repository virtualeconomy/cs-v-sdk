using Newtonsoft.Json.Linq;
using System.Text;
using v.systems.type;
using v.systems.utils;

namespace v.systems.transaction
{
    public class PaymentTransaction : ProvenTransaction
    {
        public readonly string[] BYTE_SERIALIZED_FIELDS = new string[] { "Type", "Timestamp", "Amount", "Fee", "FeeScale", "Recipient", "Attachment" };

        protected internal string recipient;
        protected internal long amount;
        protected internal string attachment;

        public PaymentTransaction()
        {
            type = (byte)TransactionType.Payment;
        }

        public override JObject ToAPIRequestJson(string publicKey, string signature)
        {
            JObject json = base.ToAPIRequestJson(publicKey, signature);
            json["amount"] = this.amount;
            json["recipient"] = this.recipient;
            json["attachment"] = this.attachment;
            return json;
        }

        public override JObject ToColdSignJson(string publicKey)
        {
            int api = GetColdSignAPIVersion(this.amount);
            JObject json = base.ToColdSignJson(publicKey, api);
            json["amount"] = this.amount;
            json["recipient"] = this.recipient;
            json["attachment"] = this.attachment;
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

        [Base58Field(isFixedLength: false)]
        public string Attachment
        {
            get
            {
                return attachment;
            }
            set
            {
                this.attachment = value;
            }
        }


        public string AttachmentWithPlainText
        {
            set
            {
                this.attachment = Base58.Encode(Encoding.UTF8.GetBytes(value));
            }
        }
    }

}