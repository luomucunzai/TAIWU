using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.DefenseAndAssist
{
	// Token: 0x02000403 RID: 1027
	public class SunGangYiRou : DefenseSkillBase
	{
		// Token: 0x060038C6 RID: 14534 RVA: 0x0023BD71 File Offset: 0x00239F71
		public SunGangYiRou()
		{
		}

		// Token: 0x060038C7 RID: 14535 RVA: 0x0023BD7B File Offset: 0x00239F7B
		public SunGangYiRou(CombatSkillKey skillKey) : base(skillKey, 6502)
		{
		}

		// Token: 0x060038C8 RID: 14536 RVA: 0x0023BD8B File Offset: 0x00239F8B
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x060038C9 RID: 14537 RVA: 0x0023BDC8 File Offset: 0x00239FC8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 102 && base.CanAffect;
				if (flag2)
				{
					sbyte innerRatio = (dataKey.CombatSkillId >= 0) ? DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(base.CurrEnemyChar.GetId(), dataKey.CombatSkillId)).GetCurrInnerRatio() : base.CurrEnemyChar.GetWeaponData(-1).GetInnerRatio();
					bool flag3 = innerRatio > 0 && innerRatio < 100 && (base.IsDirect ? (innerRatio > 50) : (innerRatio < 50));
					if (flag3)
					{
						return (dataKey.CustomParam0 == (base.IsDirect ? 1 : 0)) ? -40 : 20;
					}
				}
				result = 0;
			}
			return result;
		}

		// Token: 0x040010A0 RID: 4256
		private const sbyte ReduceDamagePercent = -40;

		// Token: 0x040010A1 RID: 4257
		private const sbyte AddDamagePercent = 20;
	}
}
