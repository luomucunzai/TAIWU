using Config;
using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.DLC.FiveLoong;

public struct JiaoLoongNameRelatedData : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte ItemType;

	[SerializableGameDataField]
	public short ItemTemplateId;

	[SerializableGameDataField]
	public short CharTemplateId;

	[SerializableGameDataField]
	public int NameId;

	public string GetName()
	{
		if (NameId >= 0 && ExternalDataBridge.Context.CustomTexts.TryGetValue(NameId, out var value))
		{
			return value;
		}
		if (CharTemplateId >= 0)
		{
			CharacterItem characterItem = Character.Instance[CharTemplateId];
			return characterItem.Surname + characterItem.GivenName;
		}
		if (ItemType < 0)
		{
			return ExtraNameText.Instance[5].Content;
		}
		return ItemTemplateHelper.GetName(ItemType, ItemTemplateId);
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 9;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (byte)ItemType;
		byte* num = pData + 1;
		*(short*)num = ItemTemplateId;
		byte* num2 = num + 2;
		*(short*)num2 = CharTemplateId;
		byte* num3 = num2 + 2;
		*(int*)num3 = NameId;
		int num4 = (int)(num3 + 4 - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ItemType = (sbyte)(*ptr);
		ptr++;
		ItemTemplateId = *(short*)ptr;
		ptr += 2;
		CharTemplateId = *(short*)ptr;
		ptr += 2;
		NameId = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
