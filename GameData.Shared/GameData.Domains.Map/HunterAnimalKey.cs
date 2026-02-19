using System;
using GameData.Serializer;

namespace GameData.Domains.Map;

public struct HunterAnimalKey : ISerializableGameData, IEquatable<HunterAnimalKey>
{
	public short AreaId;

	public short BlockId;

	public short AnimalId;

	public HunterAnimalKey(short areaId, short blockId, short animalId)
	{
		AreaId = areaId;
		BlockId = blockId;
		AnimalId = animalId;
	}

	public bool Equals(HunterAnimalKey other)
	{
		if (AreaId == other.AreaId && BlockId == other.BlockId)
		{
			return AnimalId == other.AnimalId;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is HunterAnimalKey other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((AreaId.GetHashCode() * 397) ^ BlockId.GetHashCode()) * 397) ^ AnimalId.GetHashCode();
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
		*(short*)pData = AreaId;
		byte* num = pData + 2;
		*(short*)num = BlockId;
		byte* num2 = num + 2;
		*(short*)num2 = AnimalId;
		int num3 = (int)(num2 + 2 - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		AreaId = *(short*)ptr;
		ptr += 2;
		BlockId = *(short*)ptr;
		ptr += 2;
		AnimalId = *(short*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
