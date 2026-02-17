using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.DefenseAndAssist
{
	// Token: 0x020003A9 RID: 937
	public class SangHunXi : DefenseSkillBase
	{
		// Token: 0x060036BB RID: 14011 RVA: 0x00231C52 File Offset: 0x0022FE52
		public SangHunXi()
		{
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x00231C5C File Offset: 0x0022FE5C
		public SangHunXi(CombatSkillKey skillKey) : base(skillKey, 12705)
		{
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x00231C6C File Offset: 0x0022FE6C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x00231C89 File Offset: 0x0022FE89
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x00231CA8 File Offset: 0x0022FEA8
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !isFightBack || !hit || attacker != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				int fightBackPower = base.CombatChar.GetFightBackPower(base.CombatChar.FightBackHitType);
				DomainManager.Combat.AddPoison(context, base.CombatChar, defender, base.IsDirect ? 4 : 5, 2, 180 * fightBackPower / 100, base.SkillTemplateId, true, true, default(ItemKey), false, false, false);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x04000FF3 RID: 4083
		private const int AddPoison = 180;
	}
}
