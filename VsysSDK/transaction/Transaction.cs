namespace v.systems.transaction
{
    public abstract class Transaction : ITransaction
    {

        protected internal string id;
        protected internal byte type;
        protected internal long timestamp;
        protected internal int? height;
        protected internal string status;

        public virtual string Id
        {
            get
            {
                return id;
            }
            set
            {
                this.id = value;
            }
        }


        public virtual byte Type
        {
            get
            {
                return type;
            }
            set
            {
                this.type = value;
            }
        }


        public virtual long Timestamp
        {
            get
            {
                return timestamp;
            }
            set
            {
                this.timestamp = value;
            }
        }


        public virtual int? Height
        {
            get
            {
                return height;
            }
            set
            {
                this.height = value;
            }
        }


        public virtual string Status
        {
            get
            {
                return status;
            }
            set
            {
                this.status = value;
            }
        }

    }

}