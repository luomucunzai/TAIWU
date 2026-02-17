using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Neigong
{
	// Token: 0x020003F4 RID: 1012
	public class TieDingJinShenGong : CombatSkillEffectBase
	{
		// Token: 0x06003870 RID: 14448 RVA: 0x0023A5D4 File Offset: 0x002387D4
		public TieDingJinShenGong()
		{
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x0023A5FC File Offset: 0x002387FC
		public TieDingJinShenGong(CombatSkillKey skillKey) : base(skillKey, 6002, -1)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 125, -1, -1, -1, -1), EDataModifyType.Add);
			bool flag = !base.IsDirect;
			if (flag)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			}
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x0023A68C File Offset: 0x0023888C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 125;
			int result;
			if (flag)
			{
				result = (int)(base.IsDirect ? this.DirectReduceMaxFlaw : this.ReverseReduceMaxFlaw);
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69 && dataKey.CustomParam0 == 0 && base.CombatChar.GetDefeatMarkCollection().GetTotalFlawCount() >= (int)this.ReverseReduceDamageFlaw;
				if (flag2)
				{
					result = (int)this.ReverseReduceDamagePercent;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04001083 RID: 4227
		private sbyte DirectReduceMaxFlaw = -1;

		// Token: 0x04001084 RID: 4228
		private sbyte ReverseReduceMaxFlaw = -2;

		// Token: 0x04001085 RID: 4229
		private sbyte ReverseReduceDamageFlaw = 3;

		// Token: 0x04001086 RID: 4230
		private sbyte ReverseReduceDamagePercent = -30;
	}
}
