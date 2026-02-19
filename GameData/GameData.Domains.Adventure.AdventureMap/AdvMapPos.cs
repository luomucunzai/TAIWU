using System;

namespace GameData.Domains.Adventure.AdventureMap;

public struct AdvMapPos : IEquatable<AdvMapPos>
{
	internal static AdvMapPos Error => new AdvMapPos(short.MinValue, short.MinValue);

	internal static AdvMapPos Left => new AdvMapPos(-2, 0);

	internal static AdvMapPos Right => new AdvMapPos(2, 0);

	internal static AdvMapPos UpperLeft => new AdvMapPos(-1, 1);

	internal static AdvMapPos UpperRight => new AdvMapPos(1, 1);

	internal static AdvMapPos LowerLeft => new AdvMapPos(-1, -1);

	internal static AdvMapPos LowerRight => new AdvMapPos(1, -1);

	internal static AdvMapPos Zero => new AdvMapPos(0, 0);

	public short X { get; }

	public short Y { get; }

	public AdvMapPos(int x, int y)
		: this((short)x, (short)y)
	{
	}

	public AdvMapPos(short x, short y)
	{
		if (x % 2 == 0 != (y % 2 == 0))
		{
			throw new Exception("Error Map Pos");
		}
		X = x;
		Y = y;
	}

	public AdvMapPos(int pos)
	{
		this = default(AdvMapPos);
		X = (short)(pos >> 16);
		Y = (short)(pos & 0xFFFF);
	}

	public override int GetHashCode()
	{
		return (X << 16) | (Y & 0xFFFF);
	}

	public AdvMapPos[] GetAroundPoints()
	{
		return new AdvMapPos[6]
		{
			new AdvMapPos(X + 2, Y),
			new AdvMapPos(X + 1, Y + 1),
			new AdvMapPos(X + 1, Y - 1),
			new AdvMapPos(X - 2, Y),
			new AdvMapPos(X - 1, Y + 1),
			new AdvMapPos(X - 1, Y - 1)
		};
	}

	public static AdvMapPos operator +(AdvMapPos a, AdvMapPos b)
	{
		return new AdvMapPos((short)(a.X + b.X), (short)(a.Y + b.Y));
	}

	public static AdvMapPos operator -(AdvMapPos a, AdvMapPos b)
	{
		return new AdvMapPos((short)(a.X - b.X), (short)(a.Y - b.Y));
	}

	public static AdvMapPos operator *(AdvMapPos a, int b)
	{
		return new AdvMapPos((short)(a.X * b), (short)(a.Y * b));
	}

	public static AdvMapPos operator /(AdvMapPos a, int b)
	{
		return new AdvMapPos((short)(a.X / b), (short)(a.Y / b));
	}

	public override string ToString()
	{
		return $"{X}:{Y}";
	}

	public bool Equals(AdvMapPos other)
	{
		return X == other.X && Y == other.Y;
	}

	public override bool Equals(object obj)
	{
		return obj is AdvMapPos other && Equals(other);
	}
}
