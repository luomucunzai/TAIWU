using System;
using System.Runtime.CompilerServices;

namespace GameData.Domains.Adventure.AdventureMap
{
	// Token: 0x020008D0 RID: 2256
	public struct AdvMapPos : IEquatable<AdvMapPos>
	{
		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06007FB1 RID: 32689 RVA: 0x004CC401 File Offset: 0x004CA601
		internal static AdvMapPos Error
		{
			get
			{
				return new AdvMapPos(short.MinValue, short.MinValue);
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06007FB2 RID: 32690 RVA: 0x004CC412 File Offset: 0x004CA612
		internal static AdvMapPos Left
		{
			get
			{
				return new AdvMapPos(-2, 0);
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06007FB3 RID: 32691 RVA: 0x004CC41C File Offset: 0x004CA61C
		internal static AdvMapPos Right
		{
			get
			{
				return new AdvMapPos(2, 0);
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06007FB4 RID: 32692 RVA: 0x004CC425 File Offset: 0x004CA625
		internal static AdvMapPos UpperLeft
		{
			get
			{
				return new AdvMapPos(-1, 1);
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06007FB5 RID: 32693 RVA: 0x004CC42E File Offset: 0x004CA62E
		internal static AdvMapPos UpperRight
		{
			get
			{
				return new AdvMapPos(1, 1);
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06007FB6 RID: 32694 RVA: 0x004CC437 File Offset: 0x004CA637
		internal static AdvMapPos LowerLeft
		{
			get
			{
				return new AdvMapPos(-1, -1);
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06007FB7 RID: 32695 RVA: 0x004CC440 File Offset: 0x004CA640
		internal static AdvMapPos LowerRight
		{
			get
			{
				return new AdvMapPos(1, -1);
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06007FB8 RID: 32696 RVA: 0x004CC449 File Offset: 0x004CA649
		internal static AdvMapPos Zero
		{
			get
			{
				return new AdvMapPos(0, 0);
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06007FB9 RID: 32697 RVA: 0x004CC452 File Offset: 0x004CA652
		public readonly short X { get; }

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06007FBA RID: 32698 RVA: 0x004CC45A File Offset: 0x004CA65A
		public readonly short Y { get; }

		// Token: 0x06007FBB RID: 32699 RVA: 0x004CC462 File Offset: 0x004CA662
		public AdvMapPos(int x, int y)
		{
			this = new AdvMapPos((short)x, (short)y);
		}

		// Token: 0x06007FBC RID: 32700 RVA: 0x004CC470 File Offset: 0x004CA670
		public AdvMapPos(short x, short y)
		{
			bool flag = x % 2 == 0 != (y % 2 == 0);
			if (flag)
			{
				throw new Exception("Error Map Pos");
			}
			this.X = x;
			this.Y = y;
		}

		// Token: 0x06007FBD RID: 32701 RVA: 0x004CC4AC File Offset: 0x004CA6AC
		public AdvMapPos(int pos)
		{
			this = default(AdvMapPos);
			this.X = (short)(pos >> 16);
			this.Y = (short)(pos & 65535);
		}

		// Token: 0x06007FBE RID: 32702 RVA: 0x004CC4D0 File Offset: 0x004CA6D0
		public override int GetHashCode()
		{
			return (int)this.X << 16 | ((int)this.Y & 65535);
		}

		// Token: 0x06007FBF RID: 32703 RVA: 0x004CC4F8 File Offset: 0x004CA6F8
		public AdvMapPos[] GetAroundPoints()
		{
			return new AdvMapPos[]
			{
				new AdvMapPos((int)(this.X + 2), (int)this.Y),
				new AdvMapPos((int)(this.X + 1), (int)(this.Y + 1)),
				new AdvMapPos((int)(this.X + 1), (int)(this.Y - 1)),
				new AdvMapPos((int)(this.X - 2), (int)this.Y),
				new AdvMapPos((int)(this.X - 1), (int)(this.Y + 1)),
				new AdvMapPos((int)(this.X - 1), (int)(this.Y - 1))
			};
		}

		// Token: 0x06007FC0 RID: 32704 RVA: 0x004CC5B4 File Offset: 0x004CA7B4
		public static AdvMapPos operator +(AdvMapPos a, AdvMapPos b)
		{
			return new AdvMapPos(a.X + b.X, a.Y + b.Y);
		}

		// Token: 0x06007FC1 RID: 32705 RVA: 0x004CC5EC File Offset: 0x004CA7EC
		public static AdvMapPos operator -(AdvMapPos a, AdvMapPos b)
		{
			return new AdvMapPos(a.X - b.X, a.Y - b.Y);
		}

		// Token: 0x06007FC2 RID: 32706 RVA: 0x004CC624 File Offset: 0x004CA824
		public static AdvMapPos operator *(AdvMapPos a, int b)
		{
			return new AdvMapPos((short)((int)a.X * b), (short)((int)a.Y * b));
		}

		// Token: 0x06007FC3 RID: 32707 RVA: 0x004CC650 File Offset: 0x004CA850
		public static AdvMapPos operator /(AdvMapPos a, int b)
		{
			return new AdvMapPos((short)((int)a.X / b), (short)((int)a.Y / b));
		}

		// Token: 0x06007FC4 RID: 32708 RVA: 0x004CC67C File Offset: 0x004CA87C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<short>(this.X);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted<short>(this.Y);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06007FC5 RID: 32709 RVA: 0x004CC6C8 File Offset: 0x004CA8C8
		public bool Equals(AdvMapPos other)
		{
			return this.X == other.X && this.Y == other.Y;
		}

		// Token: 0x06007FC6 RID: 32710 RVA: 0x004CC6FC File Offset: 0x004CA8FC
		public override bool Equals(object obj)
		{
			bool result;
			if (obj is AdvMapPos)
			{
				AdvMapPos other = (AdvMapPos)obj;
				result = this.Equals(other);
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
