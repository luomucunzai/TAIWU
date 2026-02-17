using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Sword
{
	// Token: 0x02000385 RID: 901
	public class GouLianJianFa : CombatSkillEffectBase
	{
		// Token: 0x0600360E RID: 13838 RVA: 0x0022F2D5 File Offset: 0x0022D4D5
		public GouLianJianFa()
		{
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x0022F2DF File Offset: 0x0022D4DF
		public GouLianJianFa(CombatSkillKey skillKey) : base(skillKey, 12301, -1)
		{
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x0022F2F0 File Offset: 0x0022D4F0
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x0022F317 File Offset: 0x0022D517
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x0022F340 File Offset: 0x0022D540
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = index != 3 || context.SkillKey != this.SkillKey || !base.CombatCharPowerMatchAffectRequire(0);
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				DomainManager.Combat.AddCombatState(context, base.CurrEnemyChar, 2, base.IsDirect ? 59 : 60, (int)(25 * base.CombatChar.GetAttackSkillPower() / 10));
				DomainManager.Combat.ChangeDistance(context, DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false), base.IsDirect ? 20 : -20, true);
				base.ShowSpecialEffectTips(0);
				DomainManager.Combat.SetDisplayPosition(context, !base.CombatChar.IsAlly, base.IsDirect ? int.MinValue : DomainManager.Combat.GetDisplayPosition(!base.CombatChar.IsAlly, DomainManager.Combat.GetCurrentDistance()));
				bool flag2 = !base.IsDirect;
				if (flag2)
				{
					base.CombatChar.SetCurrentPosition(base.CombatChar.GetDisplayPosition(), context);
					enemyChar.SetCurrentPosition(enemyChar.GetDisplayPosition(), context);
				}
			}
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x0022F4A0 File Offset: 0x0022D6A0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000FC3 RID: 4035
		private const sbyte StatePowerUnit = 25;

		// Token: 0x04000FC4 RID: 4036
		private const sbyte ForceMoveDist = 20;
	}
}
