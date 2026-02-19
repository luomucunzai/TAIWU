using System;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Building;

public struct BuildingBlockKey : ISerializableGameData, IEquatable<BuildingBlockKey>
{
	[SerializableGameDataField]
	public short AreaId;

	[SerializableGameDataField]
	public short BlockId;

	[SerializableGameDataField]
	public short BuildingBlockIndex;

	public static readonly BuildingBlockKey Invalid = new BuildingBlockKey(-1, -1, 0);

	public bool IsInvalid => Equals(Invalid);

	public BuildingBlockKey(short areaId, short blockId, short buildingBlockIndex)
	{
		AreaId = areaId;
		BlockId = blockId;
		BuildingBlockIndex = buildingBlockIndex;
	}

	public Location GetLocation()
	{
		return new Location(AreaId, BlockId);
	}

	public bool Equals(BuildingBlockKey other)
	{
		if (AreaId == other.AreaId && BlockId == other.BlockId)
		{
			return BuildingBlockIndex == other.BuildingBlockIndex;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is BuildingBlockKey other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((AreaId.GetHashCode() * 397) ^ BlockId.GetHashCode()) * 397) ^ BuildingBlockIndex.GetHashCode();
	}

	public static explicit operator ulong(BuildingBlockKey value)
	{
		return (ulong)(((long)value.AreaId << 32) + ((long)value.BlockId << 16) + value.BuildingBlockIndex);
	}

	public static explicit operator BuildingBlockKey(ulong value)
	{
		return new BuildingBlockKey((short)(value >> 32), (short)(value >> 16), (short)value);
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
		*(short*)num2 = BuildingBlockIndex;
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
		BuildingBlockIndex = *(short*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
