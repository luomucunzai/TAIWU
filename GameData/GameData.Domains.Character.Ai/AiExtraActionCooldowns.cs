using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai;

[SerializableGameData(NotForDisplayModule = true)]
public class AiExtraActionCooldowns : ISerializableGameData
{
	[SerializableGameDataField]
	public Dictionary<AiActionKey, int> OffCooldownDates;

	public AiExtraActionCooldowns()
	{
		OffCooldownDates = new Dictionary<AiActionKey, int>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2 + (OffCooldownDates?.Count ?? 0) * 6;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = (short)OffCooldownDates.Count;
		ptr += 2;
		foreach (KeyValuePair<AiActionKey, int> offCooldownDate in OffCooldownDates)
		{
			ptr += offCooldownDate.Key.Serialize(ptr);
			*(int*)ptr = offCooldownDate.Value;
			ptr += 4;
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		OffCooldownDates.Clear();
		byte* ptr = pData;
		short num = *(short*)ptr;
		ptr += 2;
		for (int i = 0; i < num; i++)
		{
			AiActionKey key = default(AiActionKey);
			ptr += key.Deserialize(ptr);
			int value = *(int*)ptr;
			ptr += 4;
			OffCooldownDates.Add(key, value);
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
