using Newtonsoft.Json;
using System;

namespace v.systems.error
{
	public class ApiError : VException
	{
        public ApiError(string message, int errorCode) : base(message)
		{
			this.Error = errorCode;
		}

		public ApiError(string message) : base(message)
		{
			this.Error = 0;
		}

        public ApiError() : base()
        {
            this.Error = 0;
        }

        public int? Error { get; set; }

        public static ApiError FromJson(string json)
		{
            ApiError result;
            try
            {
                result = JsonConvert.DeserializeObject<ApiError>(json);
            }
            catch (Exception)
            {
                result = new ApiError(json);
            }
            return result;
        }
	}

}