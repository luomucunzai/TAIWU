using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Agile
{
	// Token: 0x020005E4 RID: 1508
	public class YuZhenXing : AgileSkillBase
	{
		// Token: 0x06004470 RID: 17520 RVA: 0x0026F821 File Offset: 0x0026DA21
		public YuZhenXing()
		{
		}

		// Token: 0x06004471 RID: 17521 RVA: 0x0026F82B File Offset: 0x0026DA2B
		public YuZhenXing(CombatSkillKey skillKey) : base(skillKey, 3403)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06004472 RID: 17522 RVA: 0x0026F844 File Offset: 0x0026DA44
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x06004473 RID: 17523 RVA: 0x0026F884 File Offset: 0x0026DA84
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x0026F8BC File Offset: 0x0026DABC
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0)) || !this._affecting || DomainManager.Combat.IsMovedByTeammate(base.CombatChar);
			if (!flag)
			{
				DomainManager.Combat.ChangeChangeTrickCount(context, base.CombatChar, 2, false);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x0026F934 File Offset: 0x0026DB34
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

		// Token: 0x04001446 RID: 5190
		private const int AddChangeTrickCount = 2;

		// Token: 0x04001447 RID: 5191
		private bool _affecting;
	}
}
