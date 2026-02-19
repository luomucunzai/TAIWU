using System;
using GameData.Serializer;

namespace GameData.Domains.Extra;

[Serializable]
public struct TreasureStateInfo : ISerializableGameData, IEquatable<TreasureStateInfo>
{
	[SerializableGameDataField]
	public sbyte MapState;

	[SerializableGameDataField]
	public sbyte Amount;

	public TreasureStateInfo(sbyte mapState, sbyte amount)
	{
		MapState = mapState;
		Amount = amount;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (byte)MapState;
		byte* num = pData + 1;
		*num = (byte)Amount;
		int num2 = (int)(num + 1 - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		MapState = (sbyte)(*ptr);
		ptr++;
		Amount = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public bool Equals(TreasureStateInfo other)
	{
		if (MapState == other.MapState)
		{
			return Amount == other.Amount;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is TreasureStateInfo other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (MapState.GetHashCode() * 397) ^ Amount.GetHashCode();
	}
}
