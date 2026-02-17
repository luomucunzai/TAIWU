using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Agile
{
	// Token: 0x02000503 RID: 1283
	public class ChengWuChe : AgileSkillBase
	{
		// Token: 0x06003E90 RID: 16016 RVA: 0x00256557 File Offset: 0x00254757
		public ChengWuChe()
		{
		}

		// Token: 0x06003E91 RID: 16017 RVA: 0x00256561 File Offset: 0x00254761
		public ChengWuChe(CombatSkillKey skillKey) : base(skillKey, 13404)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06003E92 RID: 16018 RVA: 0x00256578 File Offset: 0x00254778
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(152, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x06003E93 RID: 16019 RVA: 0x002565C6 File Offset: 0x002547C6
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x002565FC File Offset: 0x002547FC
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0)) || !this._affecting || DomainManager.Combat.IsMovedByTeammate(base.CombatChar);
			if (!flag)
			{
				bool addingJumpSpeed = this._addingJumpSpeed;
				if (addingJumpSpeed)
				{
					this._addingJumpSpeed = false;
				}
				else
				{
					this._addingJumpSpeed = true;
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x0025667C File Offset: 0x0025487C
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

		// Token: 0x06003E96 RID: 16022 RVA: 0x002566E4 File Offset: 0x002548E4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || dataKey.FieldId != 152 || !base.CanAffect || !this._addingJumpSpeed;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = 80;
			}
			return result;
		}

		// Token: 0x04001275 RID: 4725
		private const int JumpSpeedAddPercent = 80;

		// Token: 0x04001276 RID: 4726
		private bool _affecting;

		// Token: 0x04001277 RID: 4727
		private bool _addingJumpSpeed;
	}
}
