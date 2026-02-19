using System;
using GameData.Serializer;

namespace GameData.Domains.TaiwuEvent.EventOption;

[Serializable]
[SerializableGameData(NotForDisplayModule = true)]
public struct OptionConsumeInfo : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte ConsumeType;

	[SerializableGameDataField]
	public int ConsumeCount;

	[SerializableGameDataField]
	public int HoldCount;

	[SerializableGameDataField]
	public bool HasEnough;

	public bool AutoConsume;

	public OptionConsumeInfo(sbyte type, int count, bool auto)
	{
		ConsumeType = type;
		ConsumeCount = count;
		AutoConsume = auto;
		HoldCount = 0;
		HasEnough = false;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 10;
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
		*(int*)num = ConsumeCount;
		byte* num2 = num + 4;
		*(int*)num2 = HoldCount;
		byte* num3 = num2 + 4;
		*num3 = (HasEnough ? ((byte)1) : ((byte)0));
		int num4 = (int)(num3 + 1 - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ConsumeType = (sbyte)(*ptr);
		ptr++;
		ConsumeCount = *(int*)ptr;
		ptr += 4;
		HoldCount = *(int*)ptr;
		ptr += 4;
		HasEnough = *ptr != 0;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
