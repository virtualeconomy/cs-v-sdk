namespace v.systems.transaction
{
    public interface ITransaction
    {

        string Id { get; set; }


        byte Type { get; set; }


        long Timestamp { get; set; }


        int? Height { get; set; }


        string Status { get; set; }


    }

}