using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x02000329 RID: 809
	public class LiYouJinMeng : AssistSkillBase
	{
		// Token: 0x06003458 RID: 13400 RVA: 0x002286A9 File Offset: 0x002268A9
		public LiYouJinMeng()
		{
		}

		// Token: 0x06003459 RID: 13401 RVA: 0x002286B3 File Offset: 0x002268B3
		public LiYouJinMeng(CombatSkillKey skillKey) : base(skillKey, 16417)
		{
			this.SetConstAffectingOnCombatBegin = true;
		}

		// Token: 0x0600345A RID: 13402 RVA: 0x002286CC File Offset: 0x002268CC
		public override void OnEnable(DataContext context)
		{
			this._addPower = 0;
			this._defeatMarkUid = base.ParseCombatCharacterDataUid(50);
			GameDataBridge.AddPostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnMarkChanged));
			base.CreateAffectedData(126, EDataModifyType.Custom, -1);
			base.CreateAffectedData(199, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(102, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x0600345B RID: 13403 RVA: 0x00228731 File Offset: 0x00226931
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey);
		}

		// Token: 0x0600345C RID: 13404 RVA: 0x00228746 File Offset: 0x00226946
		private void OnMarkChanged(DataContext context, DataUid dataUid)
		{
			this._addPower = 10 * base.CombatChar.GetDefeatMarkCollection().InnerInjuryMarkList.Sum();
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
		}

		// Token: 0x0600345D RID: 13405 RVA: 0x0022877E File Offset: 0x0022697E
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			base.SetConstAffecting(context, base.CanAffect);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
		}

		// Token: 0x0600345E RID: 13406 RVA: 0x002287A8 File Offset: 0x002269A8
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 126;
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x0600345F RID: 13407 RVA: 0x002287F0 File Offset: 0x002269F0
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
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = this._addPower;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 102;
					if (flag3)
					{
						result = ((dataKey.CustomParam0 == 0) ? -50 : 50);
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000F6F RID: 3951
		private const sbyte ChangeDamage = 50;

		// Token: 0x04000F70 RID: 3952
		private const sbyte AddPowerUnit = 10;

		// Token: 0x04000F71 RID: 3953
		private DataUid _defeatMarkUid;

		// Token: 0x04000F72 RID: 3954
		private int _addPower;
	}
}
