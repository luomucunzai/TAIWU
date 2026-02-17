using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Agile
{
	// Token: 0x02000288 RID: 648
	public class ChangEBenYue : AgileSkillBase
	{
		// Token: 0x06003113 RID: 12563 RVA: 0x00219990 File Offset: 0x00217B90
		public ChangEBenYue()
		{
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x0021999A File Offset: 0x00217B9A
		public ChangEBenYue(CombatSkillKey skillKey) : base(skillKey, 8404)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06003115 RID: 12565 RVA: 0x002199B4 File Offset: 0x00217BB4
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x06003116 RID: 12566 RVA: 0x002199F4 File Offset: 0x00217BF4
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x00219A2C File Offset: 0x00217C2C
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0)) || !this._affecting || DomainManager.Combat.IsMovedByTeammate(base.CombatChar);
			if (!flag)
			{
				base.CombatChar.ChangeNeiliAllocation(context, 1, 3, true, true);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x00219AA0 File Offset: 0x00217CA0
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

		// Token: 0x04000E8A RID: 3722
		private const sbyte AddNeiliAllocation = 3;

		// Token: 0x04000E8B RID: 3723
		private bool _affecting;
	}
}
