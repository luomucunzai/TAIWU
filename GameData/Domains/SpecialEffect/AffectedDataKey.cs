using System;
using Config;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect
{
	// Token: 0x020000D9 RID: 217
	public readonly struct AffectedDataKey : IEquatable<AffectedDataKey>
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600286A RID: 10346 RVA: 0x001EF9C3 File Offset: 0x001EDBC3
		public CombatSkillKey SkillKey
		{
			get
			{
				return new CombatSkillKey(this.CharId, this.CombatSkillId);
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600286B RID: 10347 RVA: 0x001EF9D6 File Offset: 0x001EDBD6
		public CombatSkillItem SkillTemplate
		{
			get
			{
				return Config.CombatSkill.Instance[this.CombatSkillId];
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600286C RID: 10348 RVA: 0x001EF9E8 File Offset: 0x001EDBE8
		public bool IsNormalAttack
		{
			get
			{
				return this.CombatSkillId < 0;
			}
		}

		// Token: 0x0600286D RID: 10349 RVA: 0x001EF9F4 File Offset: 0x001EDBF4
		public bool IsMatch(CombatSkillKey skillKey)
		{
			return this.CharId == skillKey.CharId && this.CombatSkillId == skillKey.SkillTemplateId;
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x001EFA25 File Offset: 0x001EDC25
		public AffectedDataKey(int charId, ushort fieldId, short combatSkillId = -1, int customParam0 = -1, int customParam1 = -1, int customParam2 = -1)
		{
			this.CharId = charId;
			this.CombatSkillId = combatSkillId;
			this.FieldId = fieldId;
			this.CustomParam0 = customParam0;
			this.CustomParam1 = customParam1;
			this.CustomParam2 = customParam2;
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x001EFA58 File Offset: 0x001EDC58
		public bool Equals(AffectedDataKey other)
		{
			return this.CharId == other.CharId && this.FieldId == other.FieldId;
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x001EFA8C File Offset: 0x001EDC8C
		public override bool Equals(object obj)
		{
			bool result;
			if (obj is AffectedDataKey)
			{
				AffectedDataKey other = (AffectedDataKey)obj;
				result = this.Equals(other);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x001EFAB8 File Offset: 0x001EDCB8
		public override int GetHashCode()
		{
			int hashCode = this.CharId;
			return hashCode * 397 ^ this.FieldId.GetHashCode();
		}

		// Token: 0x04000814 RID: 2068
		public readonly int CharId;

		// Token: 0x04000815 RID: 2069
		public readonly short CombatSkillId;

		// Token: 0x04000816 RID: 2070
		public readonly ushort FieldId;

		// Token: 0x04000817 RID: 2071
		public readonly int CustomParam0;

		// Token: 0x04000818 RID: 2072
		public readonly int CustomParam1;

		// Token: 0x04000819 RID: 2073
		public readonly int CustomParam2;
	}
}
