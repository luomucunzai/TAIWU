using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Neigong
{
	// Token: 0x02000620 RID: 1568
	public class WeiWoDuZun : AnimalEffectBase
	{
		// Token: 0x170002EF RID: 751
		// (get) Token: 0x060045C0 RID: 17856 RVA: 0x002735AC File Offset: 0x002717AC
		private CValuePercent CostPercent
		{
			get
			{
				return base.IsElite ? 66 : 33;
			}
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x002735C1 File Offset: 0x002717C1
		public WeiWoDuZun()
		{
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x002735CB File Offset: 0x002717CB
		public WeiWoDuZun(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x060045C3 RID: 17859 RVA: 0x002735D6 File Offset: 0x002717D6
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x060045C4 RID: 17860 RVA: 0x002735FD File Offset: 0x002717FD
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x060045C5 RID: 17861 RVA: 0x00273624 File Offset: 0x00271824
		private void OnCombatBegin(DataContext context)
		{
			this._inAttackRange = DomainManager.Combat.InAttackRange(base.CombatChar);
			this.DoAffect(context);
		}

		// Token: 0x060045C6 RID: 17862 RVA: 0x00273648 File Offset: 0x00271848
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool newInAttackRange = DomainManager.Combat.InAttackRange(base.CombatChar);
			bool flag = newInAttackRange == this._inAttackRange;
			if (!flag)
			{
				this._inAttackRange = newInAttackRange;
				this.DoAffect(context);
			}
		}

		// Token: 0x060045C7 RID: 17863 RVA: 0x00273688 File Offset: 0x00271888
		private void DoAffect(DataContext context)
		{
			bool flag = !base.IsCurrent;
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				base.ChangeStanceValue(context, enemyChar, -enemyChar.GetMaxStanceValue() * this.CostPercent);
				base.ChangeBreathValue(context, enemyChar, -enemyChar.GetMaxBreathValue() * this.CostPercent);
				base.ChangeMobilityValue(context, enemyChar, -MoveSpecialConstants.MaxMobility * this.CostPercent);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x04001496 RID: 5270
		private bool _inAttackRange;
	}
}
