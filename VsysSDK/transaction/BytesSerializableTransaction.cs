using System;
using System.Collections.Generic;
using System.Reflection;
using v.systems.error;
using v.systems.serialization;
using v.systems.type;
using v.systems.utils;

namespace v.systems.transaction
{
	public abstract class BytesSerializableTransaction : Transaction, BytesSerializable
	{

		protected internal abstract string[] ByteSerializedFields { get; }

		public virtual byte[] ToBytes()
		{
			return BytesHelper.ToBytes(ToByteList());
		}

		public virtual IList<byte> ToByteList()
		{
			IList<byte> result = new List<byte>();
			foreach (string fieldName in ByteSerializedFields)
			{
                PropertyInfo field = null;
                object value = null;
				Type objClass = this.GetType();
				while (objClass != null && field == null)
				{
                    field = objClass.GetProperty(fieldName);
                    objClass = objClass.BaseType;
                }
				if (field == null)
				{
					throw new SerializationError(string.Format("Cannot find field '{0}'", fieldName));
				}
                value = field.GetValue(this);
                if (value == null)
				{
					throw new SerializationError(string.Format("The value of field '{0}' is null", fieldName));
				}
				byte[] bytesArray;
				if (field.PropertyType == typeof(string))
				{
					Base58Field b58field = field.GetCustomAttribute<Base58Field>();
					if (b58field != null)
					{
						if (!b58field.isFixedLength)
						{
							bytesArray = BytesHelper.SerializeBase58WithSize(value.ToString(), BytesHelper.SHORT_BYTES);
						}
						else
						{
							bytesArray = BytesHelper.SerializeBase58(value.ToString());
						}
					}
					else
					{
						bytesArray = BytesHelper.ToBytes(value.ToString());
					}
				}
				else if (field.PropertyType == typeof(long))
                {
					bytesArray = BytesHelper.ToBytes((long)value);
				}
				else if (field.PropertyType == typeof(int))
                {
					bytesArray = BytesHelper.ToBytes((int)value);
				}
				else if (field.PropertyType == typeof(short))
                {
					bytesArray = BytesHelper.ToBytes((short)value);
				}
				else if (field.PropertyType == typeof(byte))
				{
					bytesArray = BytesHelper.ToBytes((byte)value);
				}
				else
				{
					throw new SerializationError("Unable to Serialized Field: " + fieldName);
				}
				foreach (byte b in bytesArray)
				{
					result.Add(b);
				}
			}
			return result;
		}
	}

}