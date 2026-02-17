using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Agile
{
	// Token: 0x02000287 RID: 647
	public class BieLiBu : AgileSkillBase
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600310B RID: 12555 RVA: 0x002197FA File Offset: 0x002179FA
		private int RequireHitCount
		{
			get
			{
				return base.IsDirect ? 3 : 6;
			}
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x00219808 File Offset: 0x00217A08
		public BieLiBu()
		{
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x00219812 File Offset: 0x00217A12
		public BieLiBu(CombatSkillKey skillKey) : base(skillKey, 8402)
		{
		}

		// Token: 0x0600310E RID: 12558 RVA: 0x00219822 File Offset: 0x00217A22
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._hitAccumulator = 0;
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x00219858 File Offset: 0x00217A58
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003110 RID: 12560 RVA: 0x00219888 File Offset: 0x00217A88
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = attacker != base.CombatChar || !hit;
			if (!flag)
			{
				this.AddHitCount(context);
			}
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x002198B8 File Offset: 0x00217AB8
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.Attacker != base.CombatChar || !hit;
			if (!flag)
			{
				this.AddHitCount(context);
			}
		}

		// Token: 0x06003112 RID: 12562 RVA: 0x002198F0 File Offset: 0x00217AF0
		private void AddHitCount(DataContext context)
		{
			this._hitAccumulator++;
			bool flag = this._hitAccumulator < this.RequireHitCount;
			if (!flag)
			{
				this._hitAccumulator -= this.RequireHitCount;
				bool flag2 = !base.CanAffect;
				if (!flag2)
				{
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						DomainManager.Combat.AddTrick(context, enemyChar, 20, false);
					}
					else
					{
						DomainManager.Combat.AppendMindDefeatMark(context, enemyChar, 1, -1, false);
					}
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x04000E89 RID: 3721
		private int _hitAccumulator;
	}
}
