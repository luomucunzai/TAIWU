using GameData.Serializer;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Building;

[AutoGenerateSerializableGameData]
public struct Production : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte ItemType;

	[SerializableGameDataField]
	public short TemplateId;

	public Production(sbyte itemType, short templateId)
	{
		ItemType = itemType;
		TemplateId = templateId;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 3;
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
		*(short*)num = TemplateId;
		int num2 = (int)(num + 2 - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ItemType = (sbyte)(*ptr);
		ptr++;
		TemplateId = *(short*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
