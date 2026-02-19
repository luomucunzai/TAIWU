using System;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Map;

public static class MapAreaEdge
{
	public const sbyte Left = 0;

	public const sbyte Right = 1;

	public const sbyte Top = 2;

	public const sbyte Bottom = 3;

	public const sbyte Count = 4;

	public static sbyte GetOppositeEdge(sbyte edgeType)
	{
		return edgeType switch
		{
			0 => 1, 
			1 => 0, 
			2 => 3, 
			3 => 2, 
			_ => -1, 
		};
	}

	public static sbyte GetEnterEdge(sbyte[] fromPos, sbyte[] toPos)
	{
		if (fromPos[0] == toPos[0])
		{
			if (fromPos[1] >= toPos[1])
			{
				return 2;
			}
			return 3;
		}
		return (fromPos[0] >= toPos[0]) ? ((sbyte)1) : ((sbyte)0);
	}

	public static sbyte[] GetNearestEdges(short blockId, byte areaSize)
	{
		ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(blockId, areaSize);
		bool flag = byteCoordinate.X < areaSize / 2;
		bool flag2 = byteCoordinate.Y < areaSize / 2;
		int num = (flag ? byteCoordinate.X : (areaSize - byteCoordinate.X));
		int num2 = (flag2 ? byteCoordinate.Y : (areaSize - byteCoordinate.Y));
		if (num != num2)
		{
			if (num >= num2)
			{
				return new sbyte[1] { (sbyte)(flag2 ? 3 : 2) };
			}
			return new sbyte[1] { (!flag) ? ((sbyte)1) : ((sbyte)0) };
		}
		return new sbyte[2]
		{
			(!flag) ? ((sbyte)1) : ((sbyte)0),
			(sbyte)(flag2 ? 3 : 2)
		};
	}

	public static ByteCoordinate GetRandomEdgeCoord(sbyte edgeType, byte areaSize, IRandomSource random)
	{
		return edgeType switch
		{
			0 => new ByteCoordinate(0, (byte)random.Next(1, areaSize - 1)), 
			1 => new ByteCoordinate((byte)(areaSize - 1), (byte)random.Next(1, areaSize - 1)), 
			2 => new ByteCoordinate((byte)random.Next(1, areaSize - 1), (byte)(areaSize - 1)), 
			3 => new ByteCoordinate((byte)random.Next(1, areaSize - 1), 0), 
			_ => throw new Exception($"Invalid edge type: {edgeType}"), 
		};
	}
}
