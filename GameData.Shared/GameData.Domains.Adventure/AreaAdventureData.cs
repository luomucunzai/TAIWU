using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Adventure;

public class AreaAdventureData : ISerializableGameData
{
	[SerializableGameDataField]
	public readonly Dictionary<short, AdventureSiteData> AdventureSites;

	public AreaAdventureData()
	{
		AdventureSites = new Dictionary<short, AdventureSiteData>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2 + AdventureSites.Count * 14;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = (short)AdventureSites.Count;
		ptr += 2;
		foreach (KeyValuePair<short, AdventureSiteData> adventureSite in AdventureSites)
		{
			*(short*)ptr = adventureSite.Key;
			ptr += 2;
			ptr += adventureSite.Value.Serialize(ptr);
		}
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		AdventureSites.Clear();
		byte* ptr = pData;
		short num = *(short*)ptr;
		ptr += 2;
		for (int i = 0; i < num; i++)
		{
			short key = *(short*)ptr;
			ptr += 2;
			AdventureSiteData adventureSiteData = new AdventureSiteData();
			ptr += adventureSiteData.Deserialize(ptr);
			AdventureSites.Add(key, adventureSiteData);
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
