using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Combat.Math;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.Combat
{
	// Token: 0x020006CD RID: 1741
	public static class NeiliAllocationStatusHelper
	{
		// Token: 0x06006714 RID: 26388 RVA: 0x003B022C File Offset: 0x003AE42C
		public static ENeiliAllocationStatusType GetStatus(short currentValue, short originalValue)
		{
			bool flag = originalValue == 0;
			ENeiliAllocationStatusType result;
			if (flag)
			{
				result = ENeiliAllocationStatusType.None;
			}
			else
			{
				foreach (NeiliAllocationStatusItem status in ((IEnumerable<NeiliAllocationStatusItem>)NeiliAllocationStatus.Instance))
				{
					int min = (int)originalValue * status.MinThreshold / 100;
					int max = (int)originalValue * status.MaxThreshold / 100;
					bool flag2 = ((int)currentValue > min || ((int)currentValue == min && status.AllowEqualsMin)) && (int)currentValue <= max;
					if (flag2)
					{
						return status.Type;
					}
				}
				short predefinedLogId = 8;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 2);
				defaultInterpolatedStringHandler.AppendLiteral("GetStatus by ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(currentValue);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(originalValue);
				PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
				result = ENeiliAllocationStatusType.None;
			}
			return result;
		}

		// Token: 0x06006715 RID: 26389 RVA: 0x003B0314 File Offset: 0x003AE514
		public unsafe static ENeiliAllocationStatusType GetStatus(this NeiliAllocation currentNeiliAllocation, NeiliAllocation originalNeiliAllocation, byte neiliAllocationType)
		{
			bool flag = neiliAllocationType >= 4;
			ENeiliAllocationStatusType result;
			if (flag)
			{
				result = ENeiliAllocationStatusType.None;
			}
			else
			{
				short currentValue = *currentNeiliAllocation[(int)neiliAllocationType];
				short originalValue = *originalNeiliAllocation[(int)neiliAllocationType];
				result = NeiliAllocationStatusHelper.GetStatus(currentValue, originalValue);
			}
			return result;
		}

		// Token: 0x06006716 RID: 26390 RVA: 0x003B0350 File Offset: 0x003AE550
		public static NeiliAllocationStatusItem GetConfig(this ENeiliAllocationStatusType statusType)
		{
			return NeiliAllocationStatus.Instance[(int)statusType];
		}

		// Token: 0x06006717 RID: 26391 RVA: 0x003B0370 File Offset: 0x003AE570
		public static byte GetRelatedNeiliAllocationType(this CombatSkillItem combatSkillConfig)
		{
			sbyte equipType = combatSkillConfig.EquipType;
			if (!true)
			{
			}
			byte result;
			switch (equipType)
			{
			case 1:
				result = 0;
				break;
			case 2:
				result = 1;
				break;
			case 3:
				result = 2;
				break;
			case 4:
				result = 3;
				break;
			default:
				result = 4;
				break;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06006718 RID: 26392 RVA: 0x003B03C0 File Offset: 0x003AE5C0
		public static bool IsRelated(this CombatSkillItem combatSkillConfig, byte neiliAllocationType)
		{
			return combatSkillConfig.GetRelatedNeiliAllocationType() == neiliAllocationType;
		}

		// Token: 0x06006719 RID: 26393 RVA: 0x003B03DC File Offset: 0x003AE5DC
		public static ENeiliAllocationStatusType GetNeiliAllocationStatus(this CombatCharacter combatChar, byte neiliAllocationType)
		{
			NeiliAllocation currentNeiliAllocation = combatChar.GetNeiliAllocation();
			NeiliAllocation originalNeiliAllocation = combatChar.GetOriginNeiliAllocation();
			return currentNeiliAllocation.GetStatus(originalNeiliAllocation, neiliAllocationType);
		}

		// Token: 0x0600671A RID: 26394 RVA: 0x003B0404 File Offset: 0x003AE604
		public static ENeiliAllocationStatusType GetRelatedNeiliAllocationStatus(this CombatCharacter combatChar, CombatSkillItem combatSkillConfig)
		{
			byte relatedNeiliAllocationType = combatSkillConfig.GetRelatedNeiliAllocationType();
			return combatChar.GetNeiliAllocationStatus(relatedNeiliAllocationType);
		}

		// Token: 0x0600671B RID: 26395 RVA: 0x003B0424 File Offset: 0x003AE624
		public static int GetInjuredRate(this CombatCharacter combatChar, CombatSkillItem combatSkillConfig)
		{
			sbyte baseValue = DisorderLevelOfQi.GetDisorderLevelOfQiConfig(combatChar.GetCharacter().GetDisorderOfQi()).InjuredRate;
			ENeiliAllocationStatusType status = combatChar.GetRelatedNeiliAllocationStatus(combatSkillConfig);
			CValuePercentBonus percent = NeiliAllocationStatus.Instance[(int)status].GoneMadInjuryRate;
			return (int)baseValue * percent;
		}

		// Token: 0x0600671C RID: 26396 RVA: 0x003B0474 File Offset: 0x003AE674
		public static int GetGoneMadInjuryTotalPercent(this CombatCharacter combatChar, CombatSkillItem combatSkillConfig)
		{
			ENeiliAllocationStatusType status = combatChar.GetRelatedNeiliAllocationStatus(combatSkillConfig);
			return status.GetConfig().GoneMadInjuryBonus;
		}

		// Token: 0x0600671D RID: 26397 RVA: 0x003B049C File Offset: 0x003AE69C
		public static int GetNeiliAllocationPowerAddPercent(this CombatSkillKey skillKey)
		{
			bool flag = !DomainManager.Combat.IsInCombat() || skillKey.SkillTemplateId < 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				CombatCharacter combatChar;
				bool flag2 = !DomainManager.Combat.TryGetElement_CombatCharacterDict(skillKey.CharId, out combatChar);
				if (flag2)
				{
					result = 0;
				}
				else
				{
					CombatSkillItem configData = Config.CombatSkill.Instance[skillKey.SkillTemplateId];
					ENeiliAllocationStatusType status = combatChar.GetRelatedNeiliAllocationStatus(configData);
					result = status.GetConfig().PowerAddPercent;
				}
			}
			return result;
		}

		// Token: 0x0600671E RID: 26398 RVA: 0x003B0518 File Offset: 0x003AE718
		public static int GetAddNeiliAllocationAddPercent(this CombatCharacter combatChar, byte neiliAllocationType)
		{
			return combatChar.GetNeiliAllocationStatus(neiliAllocationType).GetConfig().AddNeiliAllocation;
		}

		// Token: 0x0600671F RID: 26399 RVA: 0x003B053C File Offset: 0x003AE73C
		public static int GetCostNeiliAllocationAddPercent(this CombatCharacter combatChar, byte neiliAllocationType)
		{
			return combatChar.GetNeiliAllocationStatus(neiliAllocationType).GetConfig().CostNeiliAllocation;
		}
	}
}
