using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Agile
{
	// Token: 0x020003B2 RID: 946
	public class TianSheFan : AgileSkillBase
	{
		// Token: 0x060036F2 RID: 14066 RVA: 0x00232FED File Offset: 0x002311ED
		public TianSheFan()
		{
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x00232FF7 File Offset: 0x002311F7
		public TianSheFan(CombatSkillKey skillKey) : base(skillKey, 12606)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x00233010 File Offset: 0x00231210
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x00233050 File Offset: 0x00231250
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x00233088 File Offset: 0x00231288
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0)) || !this._affecting || DomainManager.Combat.IsMovedByTeammate(base.CombatChar);
			if (!flag)
			{
				int statePower = 50 * Math.Abs((int)(distance / 10));
				bool flag2 = statePower > 0;
				if (flag2)
				{
					DomainManager.Combat.AddCombatState(context, DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false), 2, 63, statePower);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x0023312C File Offset: 0x0023132C
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

		// Token: 0x04001007 RID: 4103
		private const sbyte StatePowerUnit = 50;

		// Token: 0x04001008 RID: 4104
		private bool _affecting;
	}
}
