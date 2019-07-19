namespace v.systems.type
{

    public class Base58Field : System.Attribute
    {
        internal bool isFixedLength;

        public Base58Field(bool isFixedLength = true)
        {
            this.isFixedLength = isFixedLength;
        }
    }

}