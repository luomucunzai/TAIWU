using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.DefenseAndAssist
{
	// Token: 0x02000280 RID: 640
	public class BingQingYuJie : AssistSkillBase
	{
		// Token: 0x060030E7 RID: 12519 RVA: 0x0021907F File Offset: 0x0021727F
		public BingQingYuJie()
		{
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x00219089 File Offset: 0x00217289
		public BingQingYuJie(CombatSkillKey skillKey) : base(skillKey, 8600)
		{
			this.SetConstAffectingOnCombatBegin = true;
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x002190A0 File Offset: 0x002172A0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._addValue = Math.Min((int)(this.CharObj.GetAttraction() / 45), 20);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 35 : 41, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this._attractionUid = new DataUid(4, 0, (ulong)((long)base.CharacterId), 79U);
			GameDataBridge.AddPostDataModificationHandler(this._attractionUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnAttractionChanged));
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x0021913A File Offset: 0x0021733A
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			GameDataBridge.RemovePostDataModificationHandler(this._attractionUid, base.DataHandlerKey);
		}

		// Token: 0x060030EB RID: 12523 RVA: 0x00219157 File Offset: 0x00217357
		private void OnAttractionChanged(DataContext context, DataUid dataUid)
		{
			this._addValue = Math.Min((int)(this.CharObj.GetAttraction() / 45), 20);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 35);
		}

		// Token: 0x060030EC RID: 12524 RVA: 0x00219189 File Offset: 0x00217389
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			base.SetConstAffecting(context, base.CanAffect);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 35);
		}

		// Token: 0x060030ED RID: 12525 RVA: 0x002191B0 File Offset: 0x002173B0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this._addValue;
			}
			return result;
		}

		// Token: 0x04000E80 RID: 3712
		private const sbyte AttractionUnit = 45;

		// Token: 0x04000E81 RID: 3713
		private const sbyte MaxAddValue = 20;

		// Token: 0x04000E82 RID: 3714
		private DataUid _attractionUid;

		// Token: 0x04000E83 RID: 3715
		private int _addValue;
	}
}
