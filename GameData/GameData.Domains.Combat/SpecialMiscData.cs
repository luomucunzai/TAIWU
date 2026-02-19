using System;
using GameData.Serializer;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForArchive = true)]
public struct SpecialMiscData : ISerializableGameData, IEquatable<SpecialMiscData>
{
	[SerializableGameDataField]
	public int Chance;

	public bool CanUse => Chance > 0;

	public static implicit operator SpecialMiscData(int chance)
	{
		return new SpecialMiscData
		{
			Chance = chance
		};
	}

	public static bool operator ==(SpecialMiscData lhs, SpecialMiscData rhs)
	{
		return lhs.Chance == rhs.Chance;
	}

	public static bool operator !=(SpecialMiscData lhs, SpecialMiscData rhs)
	{
		return lhs.Chance != rhs.Chance;
	}

	public bool Equals(SpecialMiscData other)
	{
		return Chance == other.Chance;
	}

	public override bool Equals(object obj)
	{
		return obj is SpecialMiscData other && Equals(other);
	}

	public override int GetHashCode()
	{
		return Chance;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = Chance;
		ptr += 4;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Chance = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
