using System;
using GameData.Serializer;

namespace GameData.Domains.Building;

[Obsolete]
public struct ShopEventData : ISerializableGameData
{
	public static readonly ShopEventData Invalid = new ShopEventData(0, -1, -1, -1, 0, -1, -1, -1);

	[SerializableGameDataField]
	public int EventDate;

	[SerializableGameDataField]
	public short ItemTemplateId;

	[SerializableGameDataField]
	public sbyte ItemType;

	[SerializableGameDataField]
	public sbyte ResourceType;

	[SerializableGameDataField]
	public int ResourceCount;

	[SerializableGameDataField]
	public sbyte RecruitPeopleLevel;

	[SerializableGameDataField]
	public short EventConfigId;

	[SerializableGameDataField]
	public sbyte EventDesType;

	public ShopEventData(int eventDate, short itemTemplateId, sbyte resourceType, int resourceCount, sbyte recruitPeopleLevel, short eventConfigId, sbyte eventDesType, sbyte itemType)
	{
		EventDate = eventDate;
		ItemTemplateId = itemTemplateId;
		ResourceType = resourceType;
		ResourceCount = resourceCount;
		EventConfigId = eventConfigId;
		RecruitPeopleLevel = recruitPeopleLevel;
		EventDesType = eventDesType;
		ItemType = itemType;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 16;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = EventDate;
		byte* num = pData + 4;
		*(short*)num = ItemTemplateId;
		byte* num2 = num + 2;
		*num2 = (byte)ItemType;
		byte* num3 = num2 + 1;
		*num3 = (byte)ResourceType;
		byte* num4 = num3 + 1;
		*(int*)num4 = ResourceCount;
		byte* num5 = num4 + 4;
		*num5 = (byte)RecruitPeopleLevel;
		byte* num6 = num5 + 1;
		*(short*)num6 = EventConfigId;
		byte* num7 = num6 + 2;
		*num7 = (byte)EventDesType;
		int num8 = (int)(num7 + 1 - pData);
		if (num8 > 4)
		{
			return (num8 + 3) / 4 * 4;
		}
		return num8;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		EventDate = *(int*)ptr;
		ptr += 4;
		ItemTemplateId = *(short*)ptr;
		ptr += 2;
		ItemType = (sbyte)(*ptr);
		ptr++;
		ResourceType = (sbyte)(*ptr);
		ptr++;
		ResourceCount = *(int*)ptr;
		ptr += 4;
		RecruitPeopleLevel = (sbyte)(*ptr);
		ptr++;
		EventConfigId = *(short*)ptr;
		ptr += 2;
		EventDesType = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
