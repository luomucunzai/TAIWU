using System;
using GameData.Serializer;

namespace Config;

[Serializable]
public struct BreakGrid : ISerializableGameData
{
	public short BonusType;

	public sbyte GridCount;

	public BreakGrid(short bonusType, sbyte gridCount)
	{
		BonusType = bonusType;
		GridCount = gridCount;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 3;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = BonusType;
		byte* num = pData + 2;
		*num = (byte)GridCount;
		return (int)(num + 1 - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		BonusType = *(short*)ptr;
		ptr += 2;
		GridCount = (sbyte)(*ptr);
		ptr++;
		return (int)(ptr - pData);
	}
}
