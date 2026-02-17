using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Neigong
{
	// Token: 0x02000623 RID: 1571
	public class ZhuTuXiYong : AnimalEffectBase
	{
		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x060045D5 RID: 17877 RVA: 0x0027397D File Offset: 0x00271B7D
		private int AddDamagePercent
		{
			get
			{
				return base.IsElite ? 60 : 30;
			}
		}

		// Token: 0x060045D6 RID: 17878 RVA: 0x0027398D File Offset: 0x00271B8D
		public ZhuTuXiYong()
		{
		}

		// Token: 0x060045D7 RID: 17879 RVA: 0x00273997 File Offset: 0x00271B97
		public ZhuTuXiYong(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x060045D8 RID: 17880 RVA: 0x002739A4 File Offset: 0x00271BA4
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.RegisterHandler_MoveEnd(new Events.OnMoveEnd(this.OnMoveEnd));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x060045D9 RID: 17881 RVA: 0x00273A24 File Offset: 0x00271C24
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.UnRegisterHandler_MoveEnd(new Events.OnMoveEnd(this.OnMoveEnd));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x060045DA RID: 17882 RVA: 0x00273A7C File Offset: 0x00271C7C
		private void OnCombatBegin(DataContext context)
		{
			this._inAttackRange = DomainManager.Combat.InAttackRange(base.CombatChar);
			bool flag = !this._inAttackRange;
			if (flag)
			{
				this.EnableJumpMove();
			}
		}

		// Token: 0x060045DB RID: 17883 RVA: 0x00273AB4 File Offset: 0x00271CB4
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool newInAttackRange = DomainManager.Combat.InAttackRange(base.CombatChar);
			bool flag = this._inAttackRange == newInAttackRange;
			if (!flag)
			{
				this._inAttackRange = newInAttackRange;
				bool inAttackRange = this._inAttackRange;
				if (inAttackRange)
				{
					this.DisableJumpMove(context);
				}
				else
				{
					this.EnableJumpMove();
				}
			}
		}

		// Token: 0x060045DC RID: 17884 RVA: 0x00273B04 File Offset: 0x00271D04
		private void OnMoveEnd(DataContext context, CombatCharacter mover, int distance, bool isJump)
		{
			bool flag = mover.GetId() != base.CharacterId || !isJump;
			if (!flag)
			{
				DomainManager.Combat.ChangeMobilityValue(context, mover, -MoveSpecialConstants.MaxMobility * ZhuTuXiYong.CostMobilityPercent, false, null, false);
				bool flag2 = !DomainManager.Combat.InAttackRange(base.CombatChar) || !base.IsCurrent;
				if (!flag2)
				{
					this._isAffecting = true;
					base.CombatChar.NormalAttackFree();
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x060045DD RID: 17885 RVA: 0x00273B90 File Offset: 0x00271D90
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = attacker.GetId() != base.CharacterId;
			if (!flag)
			{
				this._isAffecting = false;
			}
		}

		// Token: 0x060045DE RID: 17886 RVA: 0x00273BBC File Offset: 0x00271DBC
		private void EnableJumpMove()
		{
			bool isCurrent = base.IsCurrent;
			if (isCurrent)
			{
				base.ShowSpecialEffectTips(0);
			}
			DomainManager.Combat.EnableJumpMove(base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x060045DF RID: 17887 RVA: 0x00273BF3 File Offset: 0x00271DF3
		private void DisableJumpMove(DataContext context)
		{
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x060045E0 RID: 17888 RVA: 0x00273C10 File Offset: 0x00271E10
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 69 || !this._isAffecting;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this.AddDamagePercent;
			}
			return result;
		}

		// Token: 0x0400149A RID: 5274
		private static readonly CValuePercent CostMobilityPercent = 10;

		// Token: 0x0400149B RID: 5275
		private bool _inAttackRange;

		// Token: 0x0400149C RID: 5276
		private bool _isAffecting;
	}
}
