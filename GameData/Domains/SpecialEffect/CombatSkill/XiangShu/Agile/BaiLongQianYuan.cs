using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Agile
{
	// Token: 0x02000334 RID: 820
	public class BaiLongQianYuan : AgileSkillBase
	{
		// Token: 0x0600348B RID: 13451 RVA: 0x00228F9D File Offset: 0x0022719D
		public BaiLongQianYuan()
		{
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x00228FA7 File Offset: 0x002271A7
		public BaiLongQianYuan(CombatSkillKey skillKey) : base(skillKey, 16205)
		{
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x00228FB8 File Offset: 0x002271B8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._reduceCostPercent = 0;
			base.CreateAffectedData(175, EDataModifyType.TotalPercent, -1);
			base.CreateAffectedData(179, EDataModifyType.TotalPercent, -1);
			this._defeatMarkUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 50U);
			GameDataBridge.AddPostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefeatMarkChanged));
			this.OnDefeatMarkChanged(context, default(DataUid));
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x0022903F File Offset: 0x0022723F
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			GameDataBridge.RemovePostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey);
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x0022905C File Offset: 0x0022725C
		private void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
		{
			DefeatMarkCollection markCollection = base.CombatChar.GetDefeatMarkCollection();
			byte maxMarkCount = GlobalConfig.NeedDefeatMarkCount[(int)DomainManager.Combat.GetCombatType()];
			this._reduceCostPercent = Math.Max(-150 * markCollection.GetTotalCount() / (int)maxMarkCount, -100);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 175);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 179);
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x002290D0 File Offset: 0x002272D0
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
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId == 175 || fieldId == 179;
				bool flag3 = flag2;
				if (flag3)
				{
					result = this._reduceCostPercent;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000F7B RID: 3963
		private const int ReduceCostUnitPercent = -150;

		// Token: 0x04000F7C RID: 3964
		private DataUid _defeatMarkUid;

		// Token: 0x04000F7D RID: 3965
		private int _reduceCostPercent;
	}
}
