using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Agile
{
	// Token: 0x020003B0 RID: 944
	public class HaMaZongTiaoGong : AgileSkillBase
	{
		// Token: 0x060036E7 RID: 14055 RVA: 0x00232D08 File Offset: 0x00230F08
		public HaMaZongTiaoGong()
		{
		}

		// Token: 0x060036E8 RID: 14056 RVA: 0x00232D12 File Offset: 0x00230F12
		public HaMaZongTiaoGong(CombatSkillKey skillKey) : base(skillKey, 12602)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x060036E9 RID: 14057 RVA: 0x00232D2C File Offset: 0x00230F2C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 175, -1, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			this._affecting = false;
			this._reduceCost = 0;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x060036EA RID: 14058 RVA: 0x00232DC4 File Offset: 0x00230FC4
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x060036EB RID: 14059 RVA: 0x00232E28 File Offset: 0x00231028
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0)) || !this._affecting || DomainManager.Combat.IsMovedByTeammate(base.CombatChar);
			if (!flag)
			{
				bool flag2 = this._reduceCost > 0;
				if (flag2)
				{
					base.ShowSpecialEffectTips(0);
				}
				this._reduceCost = Math.Min(this._reduceCost + 40, 80);
			}
		}

		// Token: 0x060036EC RID: 14060 RVA: 0x00232EAC File Offset: 0x002310AC
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = attacker != base.CombatChar;
			if (!flag)
			{
				this._reduceCost = 0;
			}
		}

		// Token: 0x060036ED RID: 14061 RVA: 0x00232ED4 File Offset: 0x002310D4
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker != base.CombatChar;
			if (!flag)
			{
				this._reduceCost = 0;
			}
		}

		// Token: 0x060036EE RID: 14062 RVA: 0x00232EFC File Offset: 0x002310FC
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

		// Token: 0x060036EF RID: 14063 RVA: 0x00232F64 File Offset: 0x00231164
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId < 0 || !this._affecting || this._reduceCost == 0;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 175;
				if (flag2)
				{
					result = dataValue * (100 - this._reduceCost) / 100;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x04001003 RID: 4099
		private const sbyte CostReduceUnit = 40;

		// Token: 0x04001004 RID: 4100
		private const sbyte MaxCostReduceUnit = 80;

		// Token: 0x04001005 RID: 4101
		private bool _affecting;

		// Token: 0x04001006 RID: 4102
		private int _reduceCost;
	}
}
