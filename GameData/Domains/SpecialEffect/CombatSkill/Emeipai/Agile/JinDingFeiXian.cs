using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Agile
{
	// Token: 0x02000569 RID: 1385
	public class JinDingFeiXian : AgileSkillBase
	{
		// Token: 0x060040E6 RID: 16614 RVA: 0x00260A2B File Offset: 0x0025EC2B
		public JinDingFeiXian()
		{
		}

		// Token: 0x060040E7 RID: 16615 RVA: 0x00260A35 File Offset: 0x0025EC35
		public JinDingFeiXian(CombatSkillKey skillKey) : base(skillKey, 2506)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x060040E8 RID: 16616 RVA: 0x00260A4C File Offset: 0x0025EC4C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x060040E9 RID: 16617 RVA: 0x00260A8C File Offset: 0x0025EC8C
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			bool flag = base.CombatChar.GetJumpPreparedDistance() > 0 && DomainManager.Combat.IsInCombat();
			if (flag)
			{
				DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, 19, (int)(50 * Math.Abs(base.CombatChar.GetJumpPreparedDistance()) / 10));
				base.ShowSpecialEffectTips(0);
			}
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x060040EA RID: 16618 RVA: 0x00260B20 File Offset: 0x0025ED20
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0)) || !this._affecting || DomainManager.Combat.IsMovedByTeammate(base.CombatChar);
			if (!flag)
			{
				DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, 19, (int)(50 * Math.Abs(distance) / 10));
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x00260BA4 File Offset: 0x0025EDA4
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

		// Token: 0x04001318 RID: 4888
		private const sbyte StatePowerUnit = 50;

		// Token: 0x04001319 RID: 4889
		private bool _affecting;
	}
}
