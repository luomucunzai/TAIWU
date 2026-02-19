using System;
using GameData.Serializer;

namespace GameData.Utilities;

[Serializable]
public struct IntPair : ISerializableGameData, IComparable<IntPair>, IEquatable<IntPair>
{
	public int First;

	public int Second;

	public IntPair(int first, int second)
	{
		First = first;
		Second = second;
	}

	public static explicit operator ulong(IntPair value)
	{
		return (ulong)(((long)value.Second << 32) + value.First);
	}

	public static explicit operator IntPair(ulong value)
	{
		return new IntPair((int)value, (int)(value >> 32));
	}

	public void Deconstruct(out int first, out int second)
	{
		first = First;
		second = Second;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 8;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = First;
		((int*)pData)[1] = Second;
		return 8;
	}

	public unsafe int Deserialize(byte* pData)
	{
		First = *(int*)pData;
		Second = ((int*)pData)[1];
		return 8;
	}

	public int CompareTo(IntPair other)
	{
		int num = First - other.First;
		if (num == 0)
		{
			return Second - other.Second;
		}
		return num;
	}

	public bool Equals(IntPair other)
	{
		if (First == other.First)
		{
			return Second == other.Second;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is IntPair other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (First * 397) ^ Second;
	}
}
