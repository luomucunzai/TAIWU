using System;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Extra;

public struct SectStoryFairyland : ISerializableGameData
{
	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	public bool Visited;

	[SerializableGameDataField]
	public bool Destroyed;

	[SerializableGameDataField]
	[Obsolete]
	public short MapAreaTemplateId;

	[SerializableGameDataField]
	[Obsolete]
	public sbyte MapAreaIndex;

	public SectStoryFairyland()
	{
		Visited = false;
		Destroyed = true;
		Location = Location.Invalid;
		MapAreaTemplateId = -1;
		MapAreaIndex = -1;
	}

	public SectStoryFairyland(Location location)
	{
		Visited = false;
		Destroyed = false;
		Location = location;
		MapAreaTemplateId = -1;
		MapAreaIndex = -1;
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
		byte* ptr = pData;
		ptr += Location.Serialize(ptr);
		*ptr = (Visited ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (Destroyed ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = MapAreaTemplateId;
		ptr += 2;
		*ptr = (byte)MapAreaIndex;
		ptr++;
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
		ptr += Location.Deserialize(ptr);
		Visited = *ptr != 0;
		ptr++;
		Destroyed = *ptr != 0;
		ptr++;
		MapAreaTemplateId = *(short*)ptr;
		ptr += 2;
		MapAreaIndex = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
