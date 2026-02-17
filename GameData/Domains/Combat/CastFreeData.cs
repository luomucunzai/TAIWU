using System;
using System.Runtime.CompilerServices;

namespace GameData.Domains.Combat
{
	// Token: 0x0200068F RID: 1679
	public struct CastFreeData : IEquatable<CastFreeData>, IComparable<CastFreeData>
	{
		// Token: 0x06005F70 RID: 24432 RVA: 0x0036501C File Offset: 0x0036321C
		public static implicit operator CastFreeData([TupleElementNames(new string[]
		{
			"skillId",
			"priority"
		})] ValueTuple<short, ECombatCastFreePriority> tuple)
		{
			return new CastFreeData
			{
				SkillId = tuple.Item1,
				Priority = tuple.Item2
			};
		}

		// Token: 0x06005F71 RID: 24433 RVA: 0x00365051 File Offset: 0x00363251
		public void Deconstruct(out short skillId, out ECombatCastFreePriority priority)
		{
			skillId = this.SkillId;
			priority = this.Priority;
		}

		// Token: 0x06005F72 RID: 24434 RVA: 0x00365064 File Offset: 0x00363264
		public bool Equals(CastFreeData other)
		{
			return this.SkillId == other.SkillId && this.Priority == other.Priority;
		}

		// Token: 0x06005F73 RID: 24435 RVA: 0x00365098 File Offset: 0x00363298
		public override bool Equals(object obj)
		{
			bool result;
			if (obj is CastFreeData)
			{
				CastFreeData other = (CastFreeData)obj;
				result = this.Equals(other);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005F74 RID: 24436 RVA: 0x003650C4 File Offset: 0x003632C4
		public override int GetHashCode()
		{
			return HashCode.Combine<short, int>(this.SkillId, (int)this.Priority);
		}

		// Token: 0x06005F75 RID: 24437 RVA: 0x003650E8 File Offset: 0x003632E8
		public int CompareTo(CastFreeData other)
		{
			int priority = (int)this.Priority;
			int priorityComparison = priority.CompareTo((int)other.Priority);
			return (priorityComparison != 0) ? priorityComparison : this.SkillId.CompareTo(other.SkillId);
		}

		// Token: 0x06005F76 RID: 24438 RVA: 0x00365128 File Offset: 0x00363328
		public static bool operator ==(CastFreeData left, CastFreeData right)
		{
			return left.Equals(right);
		}

		// Token: 0x06005F77 RID: 24439 RVA: 0x00365144 File Offset: 0x00363344
		public static bool operator !=(CastFreeData left, CastFreeData right)
		{
			return !(left == right);
		}

		// Token: 0x04001948 RID: 6472
		public short SkillId;

		// Token: 0x04001949 RID: 6473
		public ECombatCastFreePriority Priority;
	}
}
