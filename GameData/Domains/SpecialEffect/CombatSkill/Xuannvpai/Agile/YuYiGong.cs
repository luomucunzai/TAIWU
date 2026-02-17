using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Agile
{
	// Token: 0x0200028F RID: 655
	public class YuYiGong : AgileSkillBase
	{
		// Token: 0x06003135 RID: 12597 RVA: 0x0021A2AF File Offset: 0x002184AF
		public YuYiGong()
		{
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x0021A2B9 File Offset: 0x002184B9
		public YuYiGong(CombatSkillKey skillKey) : base(skillKey, 8408)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x0021A2D0 File Offset: 0x002184D0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_MoveBegin(new Events.OnMoveBegin(this.OnMoveBegin));
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x06003138 RID: 12600 RVA: 0x0021A324 File Offset: 0x00218524
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_MoveBegin(new Events.OnMoveBegin(this.OnMoveBegin));
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			bool flag = base.CombatChar.GetJumpPreparedDistance() > 0;
			if (flag)
			{
				this.AddFlawAndAcupoint(context, (int)base.CombatChar.GetJumpPreparedDistance());
			}
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x06003139 RID: 12601 RVA: 0x0021A39C File Offset: 0x0021859C
		private void OnMoveBegin(DataContext context, CombatCharacter mover, int distance, bool isJump)
		{
			bool flag = mover != base.CombatChar;
			if (!flag)
			{
				this._moveInAttackRange = DomainManager.Combat.InAttackRange(base.CurrEnemyChar);
			}
		}

		// Token: 0x0600313A RID: 12602 RVA: 0x0021A3D4 File Offset: 0x002185D4
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0)) || !this._affecting || !this._moveInAttackRange || DomainManager.Combat.IsMovedByTeammate(base.CombatChar);
			if (!flag)
			{
				this.AddFlawAndAcupoint(context, (int)distance);
			}
		}

		// Token: 0x0600313B RID: 12603 RVA: 0x0021A440 File Offset: 0x00218640
		private void AddFlawAndAcupoint(DataContext context, int distance)
		{
			int addCount = Math.Abs(distance) / 10;
			bool flag = addCount <= 0;
			if (!flag)
			{
				DomainManager.Combat.AddFlaw(context, base.CurrEnemyChar, 2, this.SkillKey, -1, addCount, true);
				DomainManager.Combat.AddAcupoint(context, base.CurrEnemyChar, 2, this.SkillKey, -1, addCount, true);
				DomainManager.Combat.AddToCheckFallenSet(base.CurrEnemyChar.GetId());
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x0021A4BC File Offset: 0x002186BC
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

		// Token: 0x04000E96 RID: 3734
		private bool _affecting;

		// Token: 0x04000E97 RID: 3735
		private bool _moveInAttackRange;
	}
}
