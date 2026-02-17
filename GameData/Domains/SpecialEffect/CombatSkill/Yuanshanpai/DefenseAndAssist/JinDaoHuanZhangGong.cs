using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.DefenseAndAssist
{
	// Token: 0x02000205 RID: 517
	public class JinDaoHuanZhangGong : DefenseSkillBase
	{
		// Token: 0x06002EB5 RID: 11957 RVA: 0x002106FF File Offset: 0x0020E8FF
		public JinDaoHuanZhangGong()
		{
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x00210709 File Offset: 0x0020E909
		public JinDaoHuanZhangGong(CombatSkillKey skillKey) : base(skillKey, 5503)
		{
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x00210719 File Offset: 0x0020E919
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(291, EDataModifyType.Custom, -1);
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x00210734 File Offset: 0x0020E934
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 291 || dataValue;
			bool result;
			if (flag)
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			else
			{
				int attackerId = dataKey.CustomParam2;
				CombatCharacter attacker = DomainManager.Combat.GetElement_CombatCharacterDict(attackerId);
				bool flag2 = base.IsDirect ? (!attacker.GetChangeTrickAttack()) : (attacker.PursueAttackCount == 0);
				if (flag2)
				{
					result = false;
				}
				else
				{
					DataContext context = DomainManager.Combat.Context;
					bool affected = context.Random.CheckPercentProb(75);
					bool flag3 = affected;
					if (flag3)
					{
						base.ShowSpecialEffectTipsOnceInFrame(0);
					}
					result = affected;
				}
			}
			return result;
		}

		// Token: 0x04000DEA RID: 3562
		private const int InevitableAvoidOdds = 75;
	}
}
