using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Agile
{
	// Token: 0x02000568 RID: 1384
	public class BaiYuanFeiZongShu : AgileSkillBase
	{
		// Token: 0x060040E0 RID: 16608 RVA: 0x002608A3 File Offset: 0x0025EAA3
		public BaiYuanFeiZongShu()
		{
		}

		// Token: 0x060040E1 RID: 16609 RVA: 0x002608AD File Offset: 0x0025EAAD
		public BaiYuanFeiZongShu(CombatSkillKey skillKey) : base(skillKey, 2501)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x060040E2 RID: 16610 RVA: 0x002608C4 File Offset: 0x0025EAC4
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x060040E3 RID: 16611 RVA: 0x00260904 File Offset: 0x0025EB04
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x060040E4 RID: 16612 RVA: 0x0026093C File Offset: 0x0025EB3C
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0)) || !this._affecting || !DomainManager.Combat.InAttackRange(base.CombatChar) || DomainManager.Combat.IsMovedByTeammate(base.CombatChar);
			if (!flag)
			{
				base.CombatChar.NeedNormalAttackSkipPrepare++;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060040E5 RID: 16613 RVA: 0x002609C4 File Offset: 0x0025EBC4
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

		// Token: 0x04001317 RID: 4887
		private bool _affecting;
	}
}
