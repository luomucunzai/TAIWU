using System;

namespace GameData.Domains.Item;

public struct ItemOwnerKey : IEquatable<ItemOwnerKey>
{
	public static readonly ItemOwnerKey None = default(ItemOwnerKey);

	public ItemOwnerType OwnerType;

	public int OwnerId;

	public ItemOwnerKey(ItemOwnerType ownerType, int ownerId)
	{
		OwnerType = ownerType;
		OwnerId = ownerId;
	}

	public override string ToString()
	{
		return $"{{{OwnerType}, {OwnerId}}}";
	}

	public bool Equals(ItemOwnerKey other)
	{
		return OwnerType == other.OwnerType && OwnerId == other.OwnerId;
	}

	public override bool Equals(object obj)
	{
		return obj is ItemOwnerKey other && Equals(other);
	}

	public override int GetHashCode()
	{
		return ((int)OwnerType * 397) ^ OwnerId;
	}
}
