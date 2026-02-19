using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai;

[SerializableGameData(NotForDisplayModule = true)]
public class AiActionAdjusts : ISerializableGameData
{
	public readonly Dictionary<AiActionKey, (short RateAdjust, int ExpireDate)> Collection;

	public AiActionAdjusts()
	{
		Collection = new Dictionary<AiActionKey, (short, int)>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2 + Collection.Count * 8;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(ushort*)ptr = (ushort)Collection.Count;
		ptr += 2;
		foreach (KeyValuePair<AiActionKey, (short, int)> item in Collection)
		{
			ptr += item.Key.Serialize(ptr);
			*(short*)ptr = item.Value.Item1;
			ptr += 2;
			*(int*)ptr = item.Value.Item2;
			ptr += 4;
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		Collection.Clear();
		for (int i = 0; i < num; i++)
		{
			AiActionKey key = default(AiActionKey);
			ptr += key.Deserialize(ptr);
			short item = *(short*)ptr;
			ptr += 2;
			int item2 = *(int*)ptr;
			ptr += 4;
			Collection.Add(key, (item, item2));
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
