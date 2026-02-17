using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Agile
{
	// Token: 0x0200025D RID: 605
	public class LiCuanShu : AgileSkillBase
	{
		// Token: 0x06003046 RID: 12358 RVA: 0x00216956 File Offset: 0x00214B56
		public LiCuanShu()
		{
		}

		// Token: 0x06003047 RID: 12359 RVA: 0x00216960 File Offset: 0x00214B60
		public LiCuanShu(CombatSkillKey skillKey) : base(skillKey, 15600)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x00216978 File Offset: 0x00214B78
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(152, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_MoveEnd(new Events.OnMoveEnd(this.OnMoveEnd));
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x06003049 RID: 12361 RVA: 0x002169D8 File Offset: 0x00214BD8
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_MoveEnd(new Events.OnMoveEnd(this.OnMoveEnd));
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x0600304A RID: 12362 RVA: 0x00216A2C File Offset: 0x00214C2C
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = defender != base.CombatChar || pursueIndex > 0 || this._attackedCount >= 4;
			if (!flag)
			{
				this._attackedCount++;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x00216A74 File Offset: 0x00214C74
		private void OnMoveEnd(DataContext context, CombatCharacter mover, int distance, bool isJump)
		{
			bool flag = mover.GetId() == base.CharacterId && isJump && this._attackedCount > 0;
			if (flag)
			{
				this._attackedCount = 0;
			}
		}

		// Token: 0x0600304C RID: 12364 RVA: 0x00216AAC File Offset: 0x00214CAC
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

		// Token: 0x0600304D RID: 12365 RVA: 0x00216B14 File Offset: 0x00214D14
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || this._attackedCount <= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 152;
				if (flag2)
				{
					result = (base.CanAffect ? (20 * this._attackedCount) : 0);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E55 RID: 3669
		private const sbyte AddPrepareUnit = 20;

		// Token: 0x04000E56 RID: 3670
		private const sbyte MaxAttackedCount = 4;

		// Token: 0x04000E57 RID: 3671
		private bool _affecting;

		// Token: 0x04000E58 RID: 3672
		private int _attackedCount;
	}
}
