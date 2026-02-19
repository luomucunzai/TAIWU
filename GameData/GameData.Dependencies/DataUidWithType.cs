using System;
using GameData.Common;

namespace GameData.Dependencies;

public readonly struct DataUidWithType : IEquatable<DataUidWithType>
{
	public readonly DomainDataType Type;

	public readonly DataUid DataUid;

	public DataUidWithType(DomainDataType type, DataUid dataUid)
	{
		Type = type;
		DataUid = dataUid;
	}

	public bool Equals(DataUidWithType other)
	{
		return Type == other.Type && DataUid.Equals(other.DataUid);
	}

	public override bool Equals(object obj)
	{
		return obj is DataUidWithType other && Equals(other);
	}

	public override int GetHashCode()
	{
		return ((int)Type * 397) ^ DataUid.GetHashCode();
	}
}
