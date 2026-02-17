using System;
using System.Runtime.CompilerServices;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x020006C1 RID: 1729
	public readonly struct DefeatMarkKey : IEquatable<DefeatMarkKey>, IComparable<DefeatMarkKey>
	{
		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x060066AD RID: 26285 RVA: 0x003AE036 File Offset: 0x003AC236
		public static DefeatMarkKey Invalid
		{
			get
			{
				return new DefeatMarkKey(EMarkType.Invalid, 0, 0);
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x060066AE RID: 26286 RVA: 0x003AE040 File Offset: 0x003AC240
		public EMarkType Type { get; }

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x060066AF RID: 26287 RVA: 0x003AE048 File Offset: 0x003AC248
		public int SubType { get; }

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x060066B0 RID: 26288 RVA: 0x003AE050 File Offset: 0x003AC250
		public int SubType2 { get; }

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x060066B1 RID: 26289 RVA: 0x003AE058 File Offset: 0x003AC258
		public bool Valid
		{
			get
			{
				return this.Type >= EMarkType.Outer;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x060066B2 RID: 26290 RVA: 0x003AE068 File Offset: 0x003AC268
		public sbyte BodyPart
		{
			get
			{
				EMarkType type = this.Type;
				bool flag = type <= EMarkType.Acupoint;
				return (sbyte)(flag ? this.SubType : -1);
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x060066B3 RID: 26291 RVA: 0x003AE095 File Offset: 0x003AC295
		public sbyte PoisonType
		{
			get
			{
				return (sbyte)((this.Type == EMarkType.Poison) ? this.SubType : -1);
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x060066B4 RID: 26292 RVA: 0x003AE0AC File Offset: 0x003AC2AC
		public bool HasLevel
		{
			get
			{
				EMarkType type = this.Type;
				return type - EMarkType.Flaw <= 1;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x060066B5 RID: 26293 RVA: 0x003AE0CF File Offset: 0x003AC2CF
		public int Level
		{
			get
			{
				return this.HasLevel ? this.SubType2 : -1;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060066B6 RID: 26294 RVA: 0x003AE0E4 File Offset: 0x003AC2E4
		public bool HasOld
		{
			get
			{
				EMarkType type = this.Type;
				return type <= EMarkType.Inner || type - EMarkType.Poison <= 1 || type == EMarkType.QiDisorder;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060066B7 RID: 26295 RVA: 0x003AE114 File Offset: 0x003AC314
		public bool Old
		{
			get
			{
				return this.HasOld && this.SubType2 == 1;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060066B8 RID: 26296 RVA: 0x003AE12A File Offset: 0x003AC32A
		public bool Scatter
		{
			get
			{
				return this.Type == EMarkType.NeiliAllocation && this.SubType == 0;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060066B9 RID: 26297 RVA: 0x003AE142 File Offset: 0x003AC342
		public bool Bulge
		{
			get
			{
				return this.Type == EMarkType.NeiliAllocation && this.SubType == 1;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060066BA RID: 26298 RVA: 0x003AE15A File Offset: 0x003AC35A
		public int UiIncorrectBodyPart
		{
			get
			{
				return DefeatMarkKey.ParseUiIncorrectBodyPart((int)this.BodyPart);
			}
		}

		// Token: 0x060066BB RID: 26299 RVA: 0x003AE168 File Offset: 0x003AC368
		public static int ParseUiIncorrectBodyPart(int bodyPart)
		{
			if (!true)
			{
			}
			int result;
			switch (bodyPart)
			{
			case 0:
				result = 1;
				break;
			case 1:
				result = 2;
				break;
			case 2:
				result = 0;
				break;
			default:
				result = bodyPart;
				break;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x060066BC RID: 26300 RVA: 0x003AE1A4 File Offset: 0x003AC3A4
		internal static DefeatMarkKey Assert(DefeatMarkKey markKey)
		{
			EMarkType type = markKey.Type;
			Tester.Assert(type >= EMarkType.Invalid && type <= EMarkType.Health, "");
			int num = markKey.SubType;
			Tester.Assert(num >= 0 && num < 100, "");
			num = markKey.SubType2;
			Tester.Assert(num >= 0 && num < 100, "");
			return markKey;
		}

		// Token: 0x060066BD RID: 26301 RVA: 0x003AE218 File Offset: 0x003AC418
		public static implicit operator int(DefeatMarkKey markKey)
		{
			return (int)(markKey.Type * (EMarkType)10000 + markKey.SubType * 100 + markKey.SubType2);
		}

		// Token: 0x060066BE RID: 26302 RVA: 0x003AE24C File Offset: 0x003AC44C
		public static explicit operator DefeatMarkKey(int key)
		{
			return DefeatMarkKey.Assert(new DefeatMarkKey((EMarkType)(key / 10000), key % 10000 / 100, key % 100));
		}

		// Token: 0x060066BF RID: 26303 RVA: 0x003AE27D File Offset: 0x003AC47D
		public static implicit operator DefeatMarkKey(EMarkType markType)
		{
			return new DefeatMarkKey(markType, 0, 0);
		}

		// Token: 0x060066C0 RID: 26304 RVA: 0x003AE287 File Offset: 0x003AC487
		public static implicit operator DefeatMarkKey([TupleElementNames(new string[]
		{
			"markType",
			"subType"
		})] ValueTuple<EMarkType, int> tup)
		{
			return new DefeatMarkKey(tup.Item1, tup.Item2, 0);
		}

		// Token: 0x060066C1 RID: 26305 RVA: 0x003AE29B File Offset: 0x003AC49B
		public static implicit operator DefeatMarkKey([TupleElementNames(new string[]
		{
			"markType",
			"subType",
			"subType2"
		})] ValueTuple<EMarkType, int, int> tup)
		{
			return new DefeatMarkKey(tup.Item1, tup.Item2, tup.Item3);
		}

		// Token: 0x060066C2 RID: 26306 RVA: 0x003AE2B4 File Offset: 0x003AC4B4
		public DefeatMarkKey(EMarkType type, int subType = 0, int subType2 = 0)
		{
			this.Type = type;
			this.SubType = subType;
			this.SubType2 = subType2;
			DefeatMarkKey.Assert(this);
		}

		// Token: 0x060066C3 RID: 26307 RVA: 0x003AE2D8 File Offset: 0x003AC4D8
		public bool GroupEquals(DefeatMarkKey markKey)
		{
			bool flag = this.HasLevel || this.HasOld;
			bool result;
			if (flag)
			{
				result = (this.Type == markKey.Type && this.SubType == markKey.SubType);
			}
			else
			{
				result = this.Equals(markKey);
			}
			return result;
		}

		// Token: 0x060066C4 RID: 26308 RVA: 0x003AE32C File Offset: 0x003AC52C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 3);
			defaultInterpolatedStringHandler.AppendLiteral("DefeatMarkKey(");
			defaultInterpolatedStringHandler.AppendFormatted<EMarkType>(this.Type);
			defaultInterpolatedStringHandler.AppendLiteral(",");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.SubType);
			defaultInterpolatedStringHandler.AppendLiteral(",");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.SubType2);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060066C5 RID: 26309 RVA: 0x003AE3B0 File Offset: 0x003AC5B0
		public override bool Equals(object obj)
		{
			bool result;
			if (obj is DefeatMarkKey)
			{
				DefeatMarkKey key = (DefeatMarkKey)obj;
				result = this.Equals(key);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060066C6 RID: 26310 RVA: 0x003AE3DC File Offset: 0x003AC5DC
		public bool Equals(DefeatMarkKey other)
		{
			return this.SubType == other.SubType && this.SubType2 == other.SubType2 && this.Type == other.Type;
		}

		// Token: 0x060066C7 RID: 26311 RVA: 0x003AE420 File Offset: 0x003AC620
		public override int GetHashCode()
		{
			int hashCode = this.SubType;
			hashCode = (hashCode * 397 ^ this.SubType2);
			return hashCode * 397 ^ (int)this.Type;
		}

		// Token: 0x060066C8 RID: 26312 RVA: 0x003AE45C File Offset: 0x003AC65C
		public int CompareTo(DefeatMarkKey other)
		{
			return this.CompareTo(other);
		}

		// Token: 0x060066C9 RID: 26313 RVA: 0x003AE488 File Offset: 0x003AC688
		public static bool operator ==(DefeatMarkKey left, DefeatMarkKey right)
		{
			return left.Equals(right);
		}

		// Token: 0x060066CA RID: 26314 RVA: 0x003AE4A4 File Offset: 0x003AC6A4
		public static bool operator !=(DefeatMarkKey left, DefeatMarkKey right)
		{
			return !(left == right);
		}
	}
}
