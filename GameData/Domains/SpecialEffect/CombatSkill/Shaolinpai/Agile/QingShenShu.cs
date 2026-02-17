using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Agile
{
	// Token: 0x0200043E RID: 1086
	public class QingShenShu : AgileSkillBase
	{
		// Token: 0x06003A04 RID: 14852 RVA: 0x002419A8 File Offset: 0x0023FBA8
		public QingShenShu()
		{
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x002419B2 File Offset: 0x0023FBB2
		public QingShenShu(CombatSkillKey skillKey) : base(skillKey, 1402)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x002419CC File Offset: 0x0023FBCC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x00241A0C File Offset: 0x0023FC0C
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x06003A08 RID: 14856 RVA: 0x00241A44 File Offset: 0x0023FC44
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0)) || !this._affecting || DomainManager.Combat.IsMovedByTeammate(base.CombatChar);
			if (!flag)
			{
				DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, 10);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x00241ABC File Offset: 0x0023FCBC
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

		// Token: 0x040010FD RID: 4349
		private bool _affecting;
	}
}
