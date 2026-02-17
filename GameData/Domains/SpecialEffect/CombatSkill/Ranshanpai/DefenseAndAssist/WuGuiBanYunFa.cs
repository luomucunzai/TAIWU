using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.DefenseAndAssist
{
	// Token: 0x02000467 RID: 1127
	public class WuGuiBanYunFa : DefenseSkillBase
	{
		// Token: 0x06003B0F RID: 15119 RVA: 0x0024669B File Offset: 0x0024489B
		public WuGuiBanYunFa()
		{
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x002466A5 File Offset: 0x002448A5
		public WuGuiBanYunFa(CombatSkillKey skillKey) : base(skillKey, 7500)
		{
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x002466B5 File Offset: 0x002448B5
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CompareDataCalcFinished(new Events.OnCompareDataCalcFinished(this.OnCompareDataCalcFinished));
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x002466D2 File Offset: 0x002448D2
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_CompareDataCalcFinished(new Events.OnCompareDataCalcFinished(this.OnCompareDataCalcFinished));
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x002466F0 File Offset: 0x002448F0
		private void OnCompareDataCalcFinished(CombatContext context, DamageCompareData compareData)
		{
			bool flag = base.CombatChar != context.Defender || !base.CanAffect;
			if (!flag)
			{
				bool flag2 = base.IsDirect ? (compareData.OuterDefendValue < compareData.InnerAttackValue) : (compareData.InnerDefendValue < compareData.OuterAttackValue);
				if (flag2)
				{
					int save = base.IsDirect ? compareData.OuterDefendValue : compareData.InnerDefendValue;
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						compareData.OuterDefendValue = compareData.InnerAttackValue;
						compareData.InnerAttackValue = save;
					}
					else
					{
						compareData.InnerDefendValue = compareData.OuterAttackValue;
						compareData.OuterAttackValue = save;
					}
					base.ShowSpecialEffectTips(0);
				}
			}
		}
	}
}
