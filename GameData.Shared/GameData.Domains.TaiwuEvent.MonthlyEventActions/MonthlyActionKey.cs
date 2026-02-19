using System;
using GameData.Serializer;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions;

[Serializable]
public struct MonthlyActionKey : ISerializableGameData, IEquatable<MonthlyActionKey>
{
	[SerializableGameDataField]
	public sbyte ActionType;

	[SerializableGameDataField]
	public short Index;

	public static readonly MonthlyActionKey Invalid = new MonthlyActionKey(-1, -1);

	public MonthlyActionKey(sbyte actionType, short index = 0)
	{
		ActionType = actionType;
		Index = index;
	}

	public bool IsValid()
	{
		return Index >= 0;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 3;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (byte)ActionType;
		byte* num = pData + 1;
		*(short*)num = Index;
		int num2 = (int)(num + 2 - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ActionType = (sbyte)(*ptr);
		ptr++;
		Index = *(short*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public override string ToString()
	{
		return $"({ActionType},{Index})";
	}

	public bool Equals(MonthlyActionKey other)
	{
		if (ActionType == other.ActionType)
		{
			return Index == other.Index;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is MonthlyActionKey other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (ActionType.GetHashCode() * 397) ^ Index.GetHashCode();
	}
}
