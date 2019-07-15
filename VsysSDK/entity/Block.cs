using System.Collections.Generic;
using v.systems.transaction;

namespace v.systems.entity
{

    public class Block
    {
        public int? Version { get; set; }


        public long? Timestamp { get; set; }


        public string Reference { get; set; }


        public SPOSConsensus SPOSConsensusObject { get; set; }


        public string TransactionMerkleRoot { get; set; }


        public IList<ITransaction> Transactions { get; set; } = new List<ITransaction>();


        public string Generator { get; set; }


        public string Signature { get; set; }


        public long? Fee { get; set; }


        public int? Blocksize { get; set; }


        public int? Height { get; set; }

    }

}