using System.Collections.Generic;
using GameData.Utilities;

namespace GameData.Domains.Map;

public class MapBlockEightDirectionType
{
	public const sbyte LeftUpDown = 0;

	public const sbyte LeftUp = 1;

	public const sbyte LeftUpRight = 2;

	public const sbyte LeftDown = 3;

	public const sbyte UpRight = 4;

	public const sbyte LeftDownRight = 5;

	public const sbyte DownRight = 6;

	public const sbyte UpDownRight = 7;

	public const sbyte Left = 1;

	public const sbyte Up = 2;

	public const sbyte Down = 4;

	public const sbyte Right = 8;

	public const sbyte AllDirection = 15;

	public static bool IsBlockInArea(Dictionary<short, int> blocks, byte areaSize, int coordinateX, int coordinateY)
	{
		if (coordinateX >= 0 && coordinateY >= 0)
		{
			return blocks.ContainsKey(ByteCoordinate.CoordinateToIndex(new ByteCoordinate((byte)coordinateX, (byte)coordinateY), areaSize));
		}
		return false;
	}

	public static bool IsBlockInArea(Dictionary<short, short> blocks, byte areaSize, int coordinateX, int coordinateY, short settlementBlockId)
	{
		if (coordinateX >= 0 && coordinateY >= 0 && blocks.TryGetValue(ByteCoordinate.CoordinateToIndex(new ByteCoordinate((byte)coordinateX, (byte)coordinateY), areaSize), out var value))
		{
			return value == settlementBlockId;
		}
		return false;
	}

	public static bool IsBlockInArea(HashSet<short> blocks, byte areaSize, int coordinateX, int coordinateY)
	{
		if (coordinateX >= 0 && coordinateY >= 0)
		{
			return blocks.Contains(ByteCoordinate.CoordinateToIndex(new ByteCoordinate((byte)coordinateX, (byte)coordinateY), areaSize));
		}
		return false;
	}

	public static sbyte GetDirectionType(Dictionary<short, int> blocks, byte areaSize, short blockId)
	{
		ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(blockId, areaSize);
		sbyte b = 0;
		if (IsBlockInArea(blocks, areaSize, byteCoordinate.X - 1, byteCoordinate.Y))
		{
			b |= 1;
		}
		if (IsBlockInArea(blocks, areaSize, byteCoordinate.X + 1, byteCoordinate.Y))
		{
			b |= 8;
		}
		if (IsBlockInArea(blocks, areaSize, byteCoordinate.X, byteCoordinate.Y - 1))
		{
			b |= 4;
		}
		if (IsBlockInArea(blocks, areaSize, byteCoordinate.X, byteCoordinate.Y + 1))
		{
			b |= 2;
		}
		return b;
	}

	public static sbyte GetDirectionType(HashSet<short> blocks, byte areaSize, short blockId)
	{
		ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(blockId, areaSize);
		sbyte b = 0;
		if (IsBlockInArea(blocks, areaSize, byteCoordinate.X - 1, byteCoordinate.Y))
		{
			b |= 1;
		}
		if (IsBlockInArea(blocks, areaSize, byteCoordinate.X + 1, byteCoordinate.Y))
		{
			b |= 8;
		}
		if (IsBlockInArea(blocks, areaSize, byteCoordinate.X, byteCoordinate.Y - 1))
		{
			b |= 4;
		}
		if (IsBlockInArea(blocks, areaSize, byteCoordinate.X, byteCoordinate.Y + 1))
		{
			b |= 2;
		}
		return b;
	}

	public static sbyte GetDirectionType(Dictionary<short, short> blocks, byte areaSize, short blockId)
	{
		ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(blockId, areaSize);
		sbyte b = 0;
		if (!blocks.TryGetValue(blockId, out var value))
		{
			return b;
		}
		if (IsBlockInArea(blocks, areaSize, byteCoordinate.X - 1, byteCoordinate.Y, value))
		{
			b |= 1;
		}
		if (IsBlockInArea(blocks, areaSize, byteCoordinate.X + 1, byteCoordinate.Y, value))
		{
			b |= 8;
		}
		if (IsBlockInArea(blocks, areaSize, byteCoordinate.X, byteCoordinate.Y - 1, value))
		{
			b |= 4;
		}
		if (IsBlockInArea(blocks, areaSize, byteCoordinate.X, byteCoordinate.Y + 1, value))
		{
			b |= 2;
		}
		return b;
	}

	public static int GetSingleIndex(int type)
	{
		return type switch
		{
			1 => 0, 
			2 => 1, 
			4 => 2, 
			8 => 3, 
			_ => -1, 
		};
	}
}
