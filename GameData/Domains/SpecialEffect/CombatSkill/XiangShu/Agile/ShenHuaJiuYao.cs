using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Agile
{
	// Token: 0x0200033D RID: 829
	public class ShenHuaJiuYao : AgileSkillBase
	{
		// Token: 0x060034BB RID: 13499 RVA: 0x00229A68 File Offset: 0x00227C68
		public ShenHuaJiuYao()
		{
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x00229A72 File Offset: 0x00227C72
		public ShenHuaJiuYao(CombatSkillKey skillKey) : base(skillKey, 16213)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x00229A8C File Offset: 0x00227C8C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x00229ACC File Offset: 0x00227CCC
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x00229B04 File Offset: 0x00227D04
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !this._affecting;
			if (!flag)
			{
				this._moveCounter++;
				bool flag2 = this._moveCounter < 3;
				if (!flag2)
				{
					this._moveCounter = 0;
					base.AppendAffectedData(context, base.CharacterId, 85, EDataModifyType.Custom, -1);
					base.AppendAffectedData(context, base.CharacterId, 86, EDataModifyType.Custom, -1);
					base.AppendAffectedData(context, base.CharacterId, 76, EDataModifyType.Custom, -1);
					base.CombatChar.NormalAttackFree();
					Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x00229BC0 File Offset: 0x00227DC0
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = attacker != base.CombatChar;
			if (!flag)
			{
				base.ClearAffectedData(context);
				Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			}
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x00229BFC File Offset: 0x00227DFC
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
					DomainManager.Combat.EnableJumpMove(base.CombatChar, base.SkillTemplateId);
				}
				else
				{
					DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
				}
			}
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x00229C60 File Offset: 0x00227E60
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId >= 0;
			return flag && dataValue;
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x00229C98 File Offset: 0x00227E98
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 76;
				if (flag2)
				{
					result = 100;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x04000F85 RID: 3973
		private const sbyte RequireMoveCount = 3;

		// Token: 0x04000F86 RID: 3974
		private bool _affecting;

		// Token: 0x04000F87 RID: 3975
		private int _moveCounter;
	}
}
