using System;

namespace v.systems.error
{
	public class VException : Exception
	{
        public VException(string message) : base(message)
		{
			this.Message = message;
		}

		public VException() : base()
		{
			this.Message = "Unexpected error occurred.";
		}

        public new string Message { get; set; }

    }

}