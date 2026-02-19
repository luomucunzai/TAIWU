using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Extra;

[SerializableGameData(NotForArchive = true)]
public struct TreasureExpectResult : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte MaxGrade;

	[SerializableGameDataField]
	public int Chance;

	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	public bool AnyMaterial;

	[SerializableGameDataField]
	public bool AnyNormalItem;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 11;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)MaxGrade;
		ptr++;
		*(int*)ptr = Chance;
		ptr += 4;
		ptr += Location.Serialize(ptr);
		*ptr = (AnyMaterial ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (AnyNormalItem ? ((byte)1) : ((byte)0));
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
		MaxGrade = (sbyte)(*ptr);
		ptr++;
		Chance = *(int*)ptr;
		ptr += 4;
		ptr += Location.Deserialize(ptr);
		AnyMaterial = *ptr != 0;
		ptr++;
		AnyNormalItem = *ptr != 0;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
