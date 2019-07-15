using Newtonsoft.Json;
using System;

namespace v.systems.error
{

	public class TransactionError : VException
    {
        public TransactionError(string details) : this("error", details)
		{
		}

		public TransactionError(string status, string details) : base(details)
		{
			this.Details = details;
			this.Status = status;
		}

        public string Status { get; set; }


        public string Details { get; set; }

        public static TransactionError FromJson(string json)
        {
            TransactionError result;
            try
            {
                result = JsonConvert.DeserializeObject<TransactionError>(json);
            }
            catch (Exception)
            {
                result = new TransactionError(json);
            }
            return result;
        }
    }

}