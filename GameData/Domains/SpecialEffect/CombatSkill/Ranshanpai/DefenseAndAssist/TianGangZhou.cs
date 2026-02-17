using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.DefenseAndAssist
{
	// Token: 0x02000465 RID: 1125
	public class TianGangZhou : DefenseSkillBase
	{
		// Token: 0x06003B04 RID: 15108 RVA: 0x00246395 File Offset: 0x00244595
		public TianGangZhou()
		{
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x0024639F File Offset: 0x0024459F
		public TianGangZhou(CombatSkillKey skillKey) : base(skillKey, 7505)
		{
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x002463AF File Offset: 0x002445AF
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.TotalPercent);
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x002463E4 File Offset: 0x002445E4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId < 0 || !base.CanAffect || DomainManager.CombatSkill.GetSkillDirection(base.CurrEnemyChar.GetId(), dataKey.CombatSkillId) != (base.IsDirect ? 1 : 0);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 102;
				if (flag2)
				{
					base.ShowSpecialEffectTips(0);
					result = -40;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0400114C RID: 4428
		private const sbyte ReduceDamage = -40;
	}
}
