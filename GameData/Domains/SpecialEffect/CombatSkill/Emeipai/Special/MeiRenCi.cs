using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Special
{
	// Token: 0x02000543 RID: 1347
	public class MeiRenCi : CombatSkillEffectBase
	{
		// Token: 0x06003FFD RID: 16381 RVA: 0x0025C74F File Offset: 0x0025A94F
		public MeiRenCi()
		{
		}

		// Token: 0x06003FFE RID: 16382 RVA: 0x0025C759 File Offset: 0x0025A959
		public MeiRenCi(CombatSkillKey skillKey) : base(skillKey, 2402, -1)
		{
		}

		// Token: 0x06003FFF RID: 16383 RVA: 0x0025C76A File Offset: 0x0025A96A
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x0025C791 File Offset: 0x0025A991
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004001 RID: 16385 RVA: 0x0025C7B8 File Offset: 0x0025A9B8
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = index != 3 || context.SkillKey != this.SkillKey || !base.CombatCharPowerMatchAffectRequire(0);
			if (!flag)
			{
				OuterAndInnerShorts enemyAttackRange = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false).GetAttackRange();
				short currDist = DomainManager.Combat.GetCurrentDistance();
				bool flag2 = base.IsDirect ? (currDist < enemyAttackRange.Inner) : (currDist > enemyAttackRange.Outer);
				if (flag2)
				{
					DomainManager.Combat.ChangeDistance(context, base.CombatChar, (int)(base.IsDirect ? (enemyAttackRange.Inner - currDist) : (enemyAttackRange.Outer - currDist)));
					base.ShowSpecialEffectTips(0);
					DomainManager.Combat.SetDisplayPosition(context, base.CombatChar.IsAlly, base.IsDirect ? int.MinValue : DomainManager.Combat.GetDisplayPosition(base.CombatChar.IsAlly, DomainManager.Combat.GetCurrentDistance()));
					bool flag3 = !base.IsDirect;
					if (flag3)
					{
						CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
						base.CombatChar.SetCurrentPosition(base.CombatChar.GetDisplayPosition(), context);
						enemyChar.SetCurrentPosition(enemyChar.GetDisplayPosition(), context);
					}
				}
			}
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x0025C928 File Offset: 0x0025AB28
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}
	}
}
