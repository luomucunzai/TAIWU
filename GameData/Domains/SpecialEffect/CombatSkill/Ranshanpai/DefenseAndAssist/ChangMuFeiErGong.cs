using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.DefenseAndAssist
{
	// Token: 0x02000462 RID: 1122
	public class ChangMuFeiErGong : AssistSkillBase
	{
		// Token: 0x06003AF1 RID: 15089 RVA: 0x00245DFE File Offset: 0x00243FFE
		public ChangMuFeiErGong()
		{
		}

		// Token: 0x06003AF2 RID: 15090 RVA: 0x00245E08 File Offset: 0x00244008
		public ChangMuFeiErGong(CombatSkillKey skillKey) : base(skillKey, 7601)
		{
		}

		// Token: 0x06003AF3 RID: 15091 RVA: 0x00245E18 File Offset: 0x00244018
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._currAddValue = -1;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 32, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 33, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 34, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 35, -1, -1, -1, -1), EDataModifyType.AddPercent);
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 38, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 39, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 40, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 41, -1, -1, -1, -1), EDataModifyType.AddPercent);
			}
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06003AF4 RID: 15092 RVA: 0x00245F64 File Offset: 0x00244164
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x00245F81 File Offset: 0x00244181
		private void OnCombatBegin(DataContext context)
		{
			this.UpdateAddValue(context);
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x00245F9E File Offset: 0x0024419E
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			this.UpdateAddValue(context);
		}

		// Token: 0x06003AF7 RID: 15095 RVA: 0x00245FA9 File Offset: 0x002441A9
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			this.InvalidateCache(context);
		}

		// Token: 0x06003AF8 RID: 15096 RVA: 0x00245FB4 File Offset: 0x002441B4
		private void UpdateAddValue(DataContext context)
		{
			short currDistance = DomainManager.Combat.GetCurrentDistance();
			int addValue = (int)((currDistance <= 30) ? 0 : (5 + (currDistance - 30) * 5 / 10));
			bool flag = this._currAddValue != addValue;
			if (flag)
			{
				this._currAddValue = addValue;
				this.InvalidateCache(context);
			}
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x00246004 File Offset: 0x00244204
		private void InvalidateCache(DataContext context)
		{
			base.SetConstAffecting(context, base.CanAffect && this._currAddValue > 0);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 32);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 33);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 34);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 35);
			}
			else
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 38);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 39);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 40);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 41);
			}
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x002460E0 File Offset: 0x002442E0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			return (!base.CanAffect) ? 0 : this._currAddValue;
		}

		// Token: 0x04001146 RID: 4422
		private const sbyte MinAffectDistance = 30;

		// Token: 0x04001147 RID: 4423
		private int _currAddValue;
	}
}
