using System;

namespace GameData.Utilities;

public static class ByteCoordinateHelper
{
	public static sbyte GetDirectionRelatedTo(this ByteCoordinate self, ByteCoordinate other)
	{
		int num = self.X - other.X;
		int num2 = self.Y - other.Y;
		if (num == 0 && num2 == 0)
		{
			return 8;
		}
		float num3 = MathF.Atan2(num2, num);
		if (num3 < 0f)
		{
			num3 += (float)Math.PI * 2f;
		}
		num3 += (float)Math.PI / 8f;
		if (num3 >= (float)Math.PI * 2f)
		{
			num3 -= (float)Math.PI * 2f;
		}
		sbyte b = (sbyte)MathF.Floor(num3 / ((float)Math.PI / 4f));
		Tester.Assert(b >= 0 && b < 8);
		b--;
		if (b < 0)
		{
			b += 8;
		}
		return b;
	}

	public static sbyte GetDirectionIn(this ByteCoordinate self, byte mapSize)
	{
		if (mapSize <= 5)
		{
			return -1;
		}
		int num = mapSize / 3;
		if (self.X >= num && self.X < mapSize - num && self.Y >= num && self.Y < mapSize - num)
		{
			return 8;
		}
		byte b = (byte)(mapSize / 2);
		return self.GetDirectionRelatedTo(new ByteCoordinate(b, b));
	}
}
