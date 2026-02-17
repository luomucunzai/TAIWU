using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.DefenseAndAssist
{
	// Token: 0x02000206 RID: 518
	public class JiuGunShiBaDie : DefenseSkillBase
	{
		// Token: 0x06002EB9 RID: 11961 RVA: 0x002107E0 File Offset: 0x0020E9E0
		public JiuGunShiBaDie()
		{
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x002107EA File Offset: 0x0020E9EA
		public JiuGunShiBaDie(CombatSkillKey skillKey) : base(skillKey, 5500)
		{
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x002107FA File Offset: 0x0020E9FA
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x00210817 File Offset: 0x0020EA17
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x00210834 File Offset: 0x0020EA34
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !isFightBack || attacker != base.CombatChar || !hit || !base.CanAffect;
			if (!flag)
			{
				CombatCharacter affectChar = base.IsDirect ? base.CombatChar : base.CurrEnemyChar;
				int changeMobility = MoveSpecialConstants.MaxMobility * JiuGunShiBaDie.ChangeMobilityPercent;
				base.ChangeMobilityValue(context, affectChar, base.IsDirect ? changeMobility : (-changeMobility));
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x04000DEB RID: 3563
		private static readonly CValuePercent ChangeMobilityPercent = 12;
	}
}
