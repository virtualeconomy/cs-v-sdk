using Newtonsoft.Json.Linq;

namespace v.systems.serialization
{
	public interface JsonSerializable
	{
        JObject ToAPIRequestJson(string publicKey, string signature);
        JObject ToColdSignJson(string publicKey);
	}
}