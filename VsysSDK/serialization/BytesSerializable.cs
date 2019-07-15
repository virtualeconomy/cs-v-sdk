using System.Collections.Generic;

namespace v.systems.serialization
{
	public interface BytesSerializable
	{
		byte[] ToBytes();
		IList<byte> ToByteList();
	}

}