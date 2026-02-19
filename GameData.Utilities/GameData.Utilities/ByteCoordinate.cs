using System;
using System.Collections.Generic;

namespace GameData.Utilities;

[Serializable]
public struct ByteCoordinate : IEquatable<ByteCoordinate>
{
	public byte X;

	public byte Y;

	public ByteCoordinate(byte x, byte y)
	{
		X = x;
		Y = y;
	}

	public static bool operator ==(ByteCoordinate byteCoordinateA, ByteCoordinate byteCoordinateB)
	{
		if (byteCoordinateA.X == byteCoordinateB.X)
		{
			return byteCoordinateA.Y == byteCoordinateB.Y;
		}
		return false;
	}

	public static bool operator !=(ByteCoordinate byteCoordinateA, ByteCoordinate byteCoordinateB)
	{
		return !(byteCoordinateA == byteCoordinateB);
	}

	public static ByteCoordinate operator -(ByteCoordinate byteCoordinateA, ByteCoordinate byteCoordinateB)
	{
		return new ByteCoordinate((byte)Math.Abs(byteCoordinateA.X - byteCoordinateB.X), (byte)Math.Abs(byteCoordinateA.Y - byteCoordinateB.Y));
	}

	public static ByteCoordinate operator +(ByteCoordinate byteCoordinateA, ByteCoordinate byteCoordinateB)
	{
		return new ByteCoordinate((byte)(byteCoordinateA.X + byteCoordinateB.X), (byte)(byteCoordinateA.Y + byteCoordinateB.Y));
	}

	public static ByteCoordinate operator *(ByteCoordinate byteCoordinate, int multi)
	{
		return new ByteCoordinate((byte)(multi * byteCoordinate.X), (byte)(multi * byteCoordinate.Y));
	}

	public bool Equals(ByteCoordinate other)
	{
		if (X == other.X)
		{
			return Y == other.Y;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is ByteCoordinate other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (X.GetHashCode() * 397) ^ Y.GetHashCode();
	}

	public static double Distance(ByteCoordinate byteCoordinateA, ByteCoordinate byteCoordinateB)
	{
		ByteCoordinate byteCoordinate = byteCoordinateA - byteCoordinateB;
		return Math.Sqrt(byteCoordinate.X * byteCoordinate.X + byteCoordinate.Y * byteCoordinate.Y);
	}

	public static short CoordinateToIndex(ByteCoordinate byteCoordinate, byte mapSize)
	{
		return (short)(byteCoordinate.X + byteCoordinate.Y * mapSize);
	}

	public static ByteCoordinate IndexToCoordinate(short blockIndex, byte mapSize)
	{
		return new ByteCoordinate((byte)(blockIndex % mapSize), (byte)(blockIndex / mapSize));
	}

	public int GetManhattanDistance(ByteCoordinate byteCoordinate)
	{
		return Math.Abs(byteCoordinate.X - X) + Math.Abs(byteCoordinate.Y - Y);
	}

	public int GetMinManhattanDistance(List<ByteCoordinate> byteCoordinateList)
	{
		int num = int.MaxValue;
		for (int i = 0; i < byteCoordinateList.Count; i++)
		{
			int manhattanDistance = GetManhattanDistance(byteCoordinateList[i]);
			if (manhattanDistance < num)
			{
				num = manhattanDistance;
			}
		}
		return num;
	}

	public override string ToString()
	{
		return $"({X},{Y})";
	}
}
