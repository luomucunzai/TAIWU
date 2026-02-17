using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Neigong
{
	// Token: 0x020003C9 RID: 969
	public class WuDangHunYuanGong : CombatSkillEffectBase
	{
		// Token: 0x06003775 RID: 14197 RVA: 0x002359A7 File Offset: 0x00233BA7
		public WuDangHunYuanGong()
		{
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x002359B1 File Offset: 0x00233BB1
		public WuDangHunYuanGong(CombatSkillKey skillKey) : base(skillKey, 4005, -1)
		{
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x002359C2 File Offset: 0x00233BC2
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06003778 RID: 14200 RVA: 0x002359F0 File Offset: 0x00233BF0
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
				bool flag2 = dataKey.FieldId == 102 && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1);
				if (flag2)
				{
					sbyte innerRatio = (dataKey.CombatSkillId >= 0) ? DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(base.CurrEnemyChar.GetId(), dataKey.CombatSkillId)).GetCurrInnerRatio() : base.CurrEnemyChar.GetWeaponData(-1).GetInnerRatio();
					bool flag3 = innerRatio > 0 && innerRatio < 100;
					if (flag3)
					{
						base.ShowSpecialEffectTips(0);
						return -60;
					}
				}
				result = 0;
			}
			return result;
		}

		// Token: 0x04001032 RID: 4146
		private const sbyte ReduceDamagePercent = -60;
	}
}
