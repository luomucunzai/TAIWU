using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Agile
{
	// Token: 0x02000508 RID: 1288
	public class TianYuanZong : AgileSkillBase
	{
		// Token: 0x06003EB8 RID: 16056 RVA: 0x00256ED2 File Offset: 0x002550D2
		public TianYuanZong()
		{
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x00256EDC File Offset: 0x002550DC
		public TianYuanZong(CombatSkillKey skillKey) : base(skillKey, 13407)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x00256EF4 File Offset: 0x002550F4
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x00256F34 File Offset: 0x00255134
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			bool flag = base.CombatChar.GetJumpPreparedDistance() > 0 && DomainManager.Combat.IsInCombat();
			if (flag)
			{
				this.AddCombatState(context, base.CombatChar.GetJumpPreparedDistance());
			}
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x00256FA8 File Offset: 0x002551A8
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0)) || !this._affecting || DomainManager.Combat.IsMovedByTeammate(base.CombatChar);
			if (!flag)
			{
				this.AddCombatState(context, distance);
			}
		}

		// Token: 0x06003EBD RID: 16061 RVA: 0x0025700A File Offset: 0x0025520A
		private void AddCombatState(DataContext context, short distance)
		{
			DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, 68, (int)(50 * Math.Abs(distance) / 10));
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x00257038 File Offset: 0x00255238
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect;
			bool flag = this._affecting == canAffect;
			if (!flag)
			{
				this._affecting = canAffect;
				bool flag2 = canAffect;
				if (flag2)
				{
					DomainManager.Combat.EnableJumpMove(base.CombatChar, base.SkillTemplateId, base.IsDirect);
				}
				else
				{
					DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
				}
			}
		}

		// Token: 0x04001284 RID: 4740
		private const sbyte StatePowerUnit = 50;

		// Token: 0x04001285 RID: 4741
		private bool _affecting;
	}
}
