using GameData.Utilities;

namespace GameData.Serializer;

public static class SerializerHolder<T>
{
	public delegate int SerializeFunction(T item, RawDataPool dataPool);

	public delegate int DeserializeFunction(RawDataPool dataPool, int offset, ref T item);

	public static SerializeFunction SerializeFunc;

	public static DeserializeFunction DeserializeFunc;

	public static int Serialize(T item, RawDataPool dataPool)
	{
		if (SerializeFunc != null)
		{
			return SerializeFunc(item, dataPool);
		}
		return Serializer.SerializeDefault(item, dataPool);
	}

	public static int Deserialize(RawDataPool dataPool, int offset, ref T item)
	{
		if (DeserializeFunc != null)
		{
			return DeserializeFunc(dataPool, offset, ref item);
		}
		return Serializer.DeserializeDefault(dataPool, offset, ref item);
	}
}
