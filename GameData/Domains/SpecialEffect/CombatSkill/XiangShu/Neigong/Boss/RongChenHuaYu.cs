using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss
{
	// Token: 0x020002A2 RID: 674
	public class RongChenHuaYu : BossNeigongBase
	{
		// Token: 0x060031B3 RID: 12723 RVA: 0x0021BDB0 File Offset: 0x00219FB0
		public RongChenHuaYu()
		{
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x0021BDBA File Offset: 0x00219FBA
		public RongChenHuaYu(CombatSkillKey skillKey) : base(skillKey, 16106)
		{
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x0021BDCA File Offset: 0x00219FCA
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			GameDataBridge.RemovePostDataModificationHandler(this._mobilityLevelUid, base.DataHandlerKey);
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x0021BDE8 File Offset: 0x00219FE8
		protected override void ActivePhase2Effect(DataContext context)
		{
			base.AppendAffectedData(context, base.CharacterId, 38, EDataModifyType.TotalPercent, -1);
			base.AppendAffectedData(context, base.CharacterId, 39, EDataModifyType.TotalPercent, -1);
			base.AppendAffectedData(context, base.CharacterId, 40, EDataModifyType.TotalPercent, -1);
			base.AppendAffectedData(context, base.CharacterId, 41, EDataModifyType.TotalPercent, -1);
			this._mobilityLevelUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 132U);
			GameDataBridge.AddPostDataModificationHandler(this._mobilityLevelUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateAddPercent));
			this.UpdateAddPercent(context, default(DataUid));
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x0021BE88 File Offset: 0x0021A088
		private void UpdateAddPercent(DataContext context, DataUid dataUid)
		{
			this._addPercent = (int)(base.CombatChar.GetMobilityLevel() * 100);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 38);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 39);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 40);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 41);
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x0021BEFC File Offset: 0x0021A0FC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this._addPercent;
			}
			return result;
		}

		// Token: 0x04000EBB RID: 3771
		private DataUid _mobilityLevelUid;

		// Token: 0x04000EBC RID: 3772
		private int _addPercent;
	}
}
