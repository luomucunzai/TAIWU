using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.DLC.FiveLoong;

[SerializableGameData(NotForArchive = true)]
public struct LoongLocationData : ISerializableGameData
{
	[SerializableGameDataField]
	public int TemplateId;

	[SerializableGameDataField]
	public Location Location;

	public LoongLocationData(LoongInfo loongInfo)
	{
		TemplateId = loongInfo.CharacterTemplateId;
		Location = loongInfo.LoongCurrentLocation;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 8;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = TemplateId;
		ptr += 4;
		ptr += Location.Serialize(ptr);
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		TemplateId = *(int*)ptr;
		ptr += 4;
		ptr += Location.Deserialize(ptr);
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
