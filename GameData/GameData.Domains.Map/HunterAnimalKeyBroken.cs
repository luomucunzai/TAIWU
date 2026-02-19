using System;
using GameData.Serializer;

namespace GameData.Domains.Map;

[Obsolete]
[SerializableGameData(NotForDisplayModule = true)]
public struct HunterAnimalKeyBroken : ISerializableGameData, IEquatable<HunterAnimalKeyBroken>
{
	public short AreaId;

	public short BlockId;

	public short AnimalId;

	public HunterAnimalKeyBroken(short areaId, short blockId, short animalId)
	{
		AreaId = areaId;
		BlockId = blockId;
		AnimalId = animalId;
	}

	public bool Equals(HunterAnimalKeyBroken other)
	{
		return AreaId == other.AreaId && BlockId == other.BlockId && AnimalId == other.AnimalId;
	}

	public override bool Equals(object obj)
	{
		return obj is HunterAnimalKeyBroken other && Equals(other);
	}

	public override int GetHashCode()
	{
		int hashCode = AreaId.GetHashCode();
		hashCode = (hashCode * 397) ^ BlockId.GetHashCode();
		return (hashCode * 397) ^ AnimalId.GetHashCode();
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		int num = (int)(pData - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		int num = (int)(pData - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
