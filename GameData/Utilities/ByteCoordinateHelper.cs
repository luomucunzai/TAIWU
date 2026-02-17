using System;

namespace GameData.Utilities
{
	// Token: 0x02000016 RID: 22
	public static class ByteCoordinateHelper
	{
		// Token: 0x0600007D RID: 125 RVA: 0x00050878 File Offset: 0x0004EA78
		public static sbyte GetDirectionRelatedTo(this ByteCoordinate self, ByteCoordinate other)
		{
			int deltaX = (int)(self.X - other.X);
			int deltaY = (int)(self.Y - other.Y);
			bool flag = deltaX == 0 && deltaY == 0;
			sbyte result2;
			if (flag)
			{
				result2 = 8;
			}
			else
			{
				float result = MathF.Atan2((float)deltaY, (float)deltaX);
				bool flag2 = result < 0f;
				if (flag2)
				{
					result += 6.2831855f;
				}
				result += 0.3926991f;
				bool flag3 = result >= 6.2831855f;
				if (flag3)
				{
					result -= 6.2831855f;
				}
				sbyte direction = (sbyte)MathF.Floor(result / 0.7853982f);
				Tester.Assert(direction >= 0 && direction < 8, "");
				direction -= 1;
				bool flag4 = direction < 0;
				if (flag4)
				{
					direction += 8;
				}
				result2 = direction;
			}
			return result2;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00050938 File Offset: 0x0004EB38
		public static sbyte GetDirectionIn(this ByteCoordinate self, byte mapSize)
		{
			bool flag = mapSize <= 5;
			sbyte result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				int centerUnit = (int)(mapSize / 3);
				bool flag2 = (int)self.X >= centerUnit && (int)self.X < (int)mapSize - centerUnit && (int)self.Y >= centerUnit && (int)self.Y < (int)mapSize - centerUnit;
				if (flag2)
				{
					result = 8;
				}
				else
				{
					byte center = mapSize / 2;
					result = self.GetDirectionRelatedTo(new ByteCoordinate(center, center));
				}
			}
			return result;
		}
	}
}
