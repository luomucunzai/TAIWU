using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Agile
{
	// Token: 0x0200033F RID: 831
	public class XueLiuPiaoLu : AgileSkillBase
	{
		// Token: 0x060034CA RID: 13514 RVA: 0x00229F35 File Offset: 0x00228135
		public XueLiuPiaoLu()
		{
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x00229F3F File Offset: 0x0022813F
		public XueLiuPiaoLu(CombatSkillKey skillKey) : base(skillKey, 16207)
		{
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x00229F50 File Offset: 0x00228150
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AutoRemove = false;
			this._addPower = 0;
			base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
			this._defeatMarkUid = base.ParseCombatCharacterDataUid(50);
			GameDataBridge.AddPostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateAddPower));
			this.UpdateAddPower(context, default(DataUid));
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x00229FC4 File Offset: 0x002281C4
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			GameDataBridge.RemovePostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey);
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x00229FE4 File Offset: 0x002281E4
		private void UpdateAddPower(DataContext context, DataUid dataUid)
		{
			DefeatMarkCollection marks = base.CombatChar.GetDefeatMarkCollection();
			int addPower = 10 * (marks.OuterInjuryMarkList.Sum() + marks.InnerInjuryMarkList.Sum());
			bool flag = this._addPower == addPower;
			if (!flag)
			{
				bool flag2 = addPower > this._addPower;
				if (flag2)
				{
					base.ShowSpecialEffectTips(0);
				}
				this._addPower = addPower;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x0022A05C File Offset: 0x0022825C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = this._addPower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000F8A RID: 3978
		private const sbyte AddPowerUnit = 10;

		// Token: 0x04000F8B RID: 3979
		private DataUid _defeatMarkUid;

		// Token: 0x04000F8C RID: 3980
		private int _addPower;
	}
}
