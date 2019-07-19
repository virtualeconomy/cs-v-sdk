﻿using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using v.systems.entity;
using v.systems.serialization;

namespace v.systems.transaction
{

    public abstract class ProvenTransaction : BytesSerializableTransaction, JsonSerializable
    {
        protected internal List<Proof> proofs;
        protected internal long? feeCharged;
        protected internal short feeScale;
        protected internal long fee;

        public virtual JObject ToAPIRequestJson(string publicKey, string signature)
        {
            JObject json = new JObject
            {
                ["timestamp"] = this.timestamp,
                ["fee"] = this.fee,
                ["feeScale"] = this.feeScale,
                ["senderPublicKey"] = publicKey,
                ["signature"] = signature
            };
            return json;
        }

        public virtual JObject ToColdSignJson(string publicKey)
        {
            return ToColdSignJson(publicKey, 1);
        }

        public virtual JObject ToColdSignJson(string publicKey, int ApiVersion)
        {
            JObject json = new JObject
            {
                ["protocol"] = "v.systems",
                ["api"] = ApiVersion,
                ["opc"] = "transaction",
                ["transactionType"] = this.type,
                ["senderPublicKey"] = publicKey,
                ["fee"] = this.fee,
                ["feeScale"] = this.feeScale,
                ["timestamp"] = this.timestamp
            };
            return json;
        }

        // According to "Cold and Hot Wallet Interaction Specification 2.0"
        // https://github.com/virtualeconomy/rfcs/blob/master/text/0003-wallet-interaction-specification-2.md
        public static int GetColdSignAPIVersion(long? amount)
        {
            if (amount % 100 == 0)
            {
                return 1;
            }
            return amount > 9007199254740991L ? 2 : 1;
        }

        public virtual List<Proof> Proofs
        {
            get
            {
                return proofs;
            }
            set
            {
                this.proofs = value;
            }
        }


        public virtual long? FeeCharged
        {
            get
            {
                return feeCharged;
            }
            set
            {
                this.feeCharged = value;
            }
        }


        public virtual short FeeScale
        {
            get
            {
                return feeScale;
            }
            set
            {
                this.feeScale = value;
            }
        }


        public virtual long Fee
        {
            get
            {
                return fee;
            }
            set
            {
                this.fee = value;
            }
        }

    }

}