using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Whip
{
	// Token: 0x0200037D RID: 893
	public class ChiYouTieBian : AddWeaponEquipAttackOnAttack
	{
		// Token: 0x060035DA RID: 13786 RVA: 0x0022E230 File Offset: 0x0022C430
		public ChiYouTieBian()
		{
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x0022E23A File Offset: 0x0022C43A
		public ChiYouTieBian(CombatSkillKey skillKey) : base(skillKey, 12404)
		{
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x0022E24A File Offset: 0x0022C44A
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x0022E267 File Offset: 0x0022C467
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x0022E284 File Offset: 0x0022C484
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = index != 3 || context.SkillKey != this.SkillKey || !base.CombatCharPowerMatchAffectRequire(0);
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				base.ClearAffectingAgileSkill(context, enemyChar);
				base.ChangeMobilityValue(context, enemyChar, -enemyChar.GetMobilityValue());
				DomainManager.Combat.ChangeDistance(context, enemyChar, base.IsDirect ? 60 : -60, true);
				base.ShowSpecialEffectTips(1);
				DomainManager.Combat.SetDisplayPosition(context, !base.CombatChar.IsAlly, base.IsDirect ? int.MinValue : DomainManager.Combat.GetDisplayPosition(!base.CombatChar.IsAlly, DomainManager.Combat.GetCurrentDistance()));
				bool flag2 = !base.IsDirect;
				if (flag2)
				{
					base.CombatChar.SetCurrentPosition(base.CombatChar.GetDisplayPosition(), context);
					enemyChar.SetCurrentPosition(enemyChar.GetDisplayPosition(), context);
				}
			}
		}

		// Token: 0x04000FB3 RID: 4019
		private const sbyte MoveDistance = 60;
	}
}
