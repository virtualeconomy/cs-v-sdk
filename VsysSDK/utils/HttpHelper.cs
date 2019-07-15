using System;
using System.Text;
using System.Net.Http;

namespace v.systems.utils
{
	public class HttpHelper
	{
        public static string Get(string url)
		{
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(new Uri(url)).GetAwaiter().GetResult();
            string result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return result;
        }

		public static string Post(string url, string json)
		{
            HttpClient client = new HttpClient();
            HttpContent contentPost = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(new Uri(url), contentPost).GetAwaiter().GetResult();
            string result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return result;
		}
	}

}