using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x0200032A RID: 810
	public class PiXueGuaYan : AssistSkillBase
	{
		// Token: 0x06003460 RID: 13408 RVA: 0x0022885E File Offset: 0x00226A5E
		public PiXueGuaYan()
		{
		}

		// Token: 0x06003461 RID: 13409 RVA: 0x00228868 File Offset: 0x00226A68
		public PiXueGuaYan(CombatSkillKey skillKey) : base(skillKey, 16418)
		{
			this.SetConstAffectingOnCombatBegin = true;
		}

		// Token: 0x06003462 RID: 13410 RVA: 0x00228880 File Offset: 0x00226A80
		public override void OnEnable(DataContext context)
		{
			this._addPower = 0;
			this._defeatMarkUid = base.ParseCombatCharacterDataUid(50);
			GameDataBridge.AddPostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnMarkChanged));
			base.CreateAffectedData(131, EDataModifyType.Custom, -1);
			base.CreateAffectedData(199, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(102, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x06003463 RID: 13411 RVA: 0x002288E8 File Offset: 0x00226AE8
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey);
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x002288FD File Offset: 0x00226AFD
		private void OnMarkChanged(DataContext context, DataUid dataUid)
		{
			this._addPower = 10 * base.CombatChar.GetDefeatMarkCollection().OuterInjuryMarkList.Sum();
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
		}

		// Token: 0x06003465 RID: 13413 RVA: 0x00228935 File Offset: 0x00226B35
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			base.SetConstAffecting(context, base.CanAffect);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
		}

		// Token: 0x06003466 RID: 13414 RVA: 0x00228960 File Offset: 0x00226B60
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
				bool flag2 = dataKey.FieldId == 131;
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x06003467 RID: 13415 RVA: 0x002289AC File Offset: 0x00226BAC
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
						result = ((dataKey.CustomParam0 == 1) ? -50 : 50);
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000F73 RID: 3955
		private const sbyte ChangeDamage = 50;

		// Token: 0x04000F74 RID: 3956
		private const sbyte AddPowerUnit = 10;

		// Token: 0x04000F75 RID: 3957
		private DataUid _defeatMarkUid;

		// Token: 0x04000F76 RID: 3958
		private int _addPower;
	}
}
