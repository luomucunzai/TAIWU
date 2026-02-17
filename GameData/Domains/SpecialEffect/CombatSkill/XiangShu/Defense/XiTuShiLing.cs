using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense
{
	// Token: 0x020002B3 RID: 691
	public class XiTuShiLing : DefenseSkillBase
	{
		// Token: 0x0600320D RID: 12813 RVA: 0x0021DA65 File Offset: 0x0021BC65
		public XiTuShiLing()
		{
		}

		// Token: 0x0600320E RID: 12814 RVA: 0x0021DA6F File Offset: 0x0021BC6F
		public XiTuShiLing(CombatSkillKey skillKey) : base(skillKey, 16306)
		{
		}

		// Token: 0x0600320F RID: 12815 RVA: 0x0021DA7F File Offset: 0x0021BC7F
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x0021DAAE File Offset: 0x0021BCAE
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003211 RID: 12817 RVA: 0x0021DAE0 File Offset: 0x0021BCE0
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !hit || pursueIndex != 0 || defender != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				this.StealEnemyNeiliAllocation(context);
			}
		}

		// Token: 0x06003212 RID: 12818 RVA: 0x0021DB1C File Offset: 0x0021BD1C
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !hit || index > 2 || context.Defender != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				this.StealEnemyNeiliAllocation(context);
			}
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x0021DB60 File Offset: 0x0021BD60
		private void StealEnemyNeiliAllocation(DataContext context)
		{
			bool flag = base.CombatChar.AbsorbNeiliAllocationRandom(context, base.CurrEnemyChar, 3);
			if (flag)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x04000ED2 RID: 3794
		private const sbyte NeiliAllocationValue = 3;
	}
}
