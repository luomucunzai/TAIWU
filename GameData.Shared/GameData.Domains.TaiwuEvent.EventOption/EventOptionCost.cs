using System;
using GameData.Serializer;

namespace GameData.Domains.TaiwuEvent.EventOption;

[Serializable]
[SerializableGameData(NotForDisplayModule = true, NotForArchive = true)]
public class EventOptionCost : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte ConsumeType;

	[SerializableGameDataField]
	public int CostAmount;

	[SerializableGameDataField]
	public bool AutoConsume;

	public EventOptionCost()
	{
		ConsumeType = -1;
		CostAmount = 0;
		AutoConsume = false;
	}

	public EventOptionCost(EventOptionCost other)
	{
		ConsumeType = other.ConsumeType;
		CostAmount = other.CostAmount;
		AutoConsume = other.AutoConsume;
	}

	public void Assign(EventOptionCost other)
	{
		ConsumeType = other.ConsumeType;
		CostAmount = other.CostAmount;
		AutoConsume = other.AutoConsume;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 6;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (byte)ConsumeType;
		byte* num = pData + 1;
		*(int*)num = CostAmount;
		byte* num2 = num + 4;
		*num2 = (AutoConsume ? ((byte)1) : ((byte)0));
		int num3 = (int)(num2 + 1 - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ConsumeType = (sbyte)(*ptr);
		ptr++;
		CostAmount = *(int*)ptr;
		ptr += 4;
		AutoConsume = *ptr != 0;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
