using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Throw
{
	// Token: 0x0200047B RID: 1147
	public class JingMengXiang : CombatSkillEffectBase
	{
		// Token: 0x06003B87 RID: 15239 RVA: 0x00248701 File Offset: 0x00246901
		public JingMengXiang()
		{
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x0024870B File Offset: 0x0024690B
		public JingMengXiang(CombatSkillKey skillKey) : base(skillKey, 10402, -1)
		{
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x0024871C File Offset: 0x0024691C
		public override void OnEnable(DataContext context)
		{
			OuterAndInnerShorts attackRange = base.CombatChar.GetAttackRange();
			short minDist = Math.Max(attackRange.Outer, 20);
			short maxDist = Math.Min(attackRange.Inner, 120);
			short currDist = DomainManager.Combat.GetCurrentDistance();
			int addProgress = base.CombatChar.SkillPrepareTotalProgress * (int)(base.IsDirect ? (maxDist - currDist) : (currDist - minDist)) / (int)(maxDist - minDist);
			bool flag = addProgress > 0;
			if (flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, addProgress);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x002487B9 File Offset: 0x002469B9
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x002487D0 File Offset: 0x002469D0
		private unsafe void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.SkillKey != this.SkillKey || index != 3;
			if (!flag)
			{
				bool flag2 = base.CombatCharPowerMatchAffectRequire(0);
				if (flag2)
				{
					HitOrAvoidInts selfHit = this.CharObj.GetHitValues();
					HitOrAvoidInts enemyAvoid = base.CurrEnemyChar.GetCharacter().GetAvoidValues();
					bool flag3 = *(ref selfHit.Items.FixedElementField + (IntPtr)3 * 4) > *(ref enemyAvoid.Items.FixedElementField + (IntPtr)3 * 4);
					if (flag3)
					{
						bool flag4 = base.CurrEnemyChar.GetPreparingSkillId() >= 0;
						if (flag4)
						{
							DomainManager.Combat.InterruptSkill(context, base.CurrEnemyChar, 100);
						}
						sbyte otherActionType = base.CurrEnemyChar.GetPreparingOtherAction();
						bool flag5 = otherActionType >= 0 && otherActionType != 3;
						if (flag5)
						{
							DomainManager.Combat.InterruptOtherAction(context, base.CurrEnemyChar);
						}
						base.ShowSpecialEffectTips(1);
					}
				}
				base.RemoveSelf(context);
			}
		}
	}
}
