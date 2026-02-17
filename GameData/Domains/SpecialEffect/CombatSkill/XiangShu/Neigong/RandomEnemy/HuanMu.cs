using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.RandomEnemy
{
	// Token: 0x02000292 RID: 658
	public class HuanMu : MinionBase
	{
		// Token: 0x06003148 RID: 12616 RVA: 0x0021A721 File Offset: 0x00218921
		public HuanMu()
		{
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x0021A72B File Offset: 0x0021892B
		public HuanMu(CombatSkillKey skillKey) : base(skillKey, 16002)
		{
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x0021A73B File Offset: 0x0021893B
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 84, -1, -1, -1, -1), EDataModifyType.Add);
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x0021A768 File Offset: 0x00218968
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !MinionBase.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 84;
				if (flag2)
				{
					result = (int)base.CurrEnemyChar.GetFlawCount()[dataKey.CustomParam0];
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}
	}
}
