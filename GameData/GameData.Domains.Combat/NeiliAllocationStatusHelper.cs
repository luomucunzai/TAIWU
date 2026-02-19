using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.Combat;

public static class NeiliAllocationStatusHelper
{
	public static ENeiliAllocationStatusType GetStatus(short currentValue, short originalValue)
	{
		if (originalValue == 0)
		{
			return ENeiliAllocationStatusType.None;
		}
		foreach (NeiliAllocationStatusItem item in (IEnumerable<NeiliAllocationStatusItem>)NeiliAllocationStatus.Instance)
		{
			int num = originalValue * item.MinThreshold / 100;
			int num2 = originalValue * item.MaxThreshold / 100;
			if ((currentValue > num || (currentValue == num && item.AllowEqualsMin)) && currentValue <= num2)
			{
				return item.Type;
			}
		}
		PredefinedLog.Show(8, $"GetStatus by {currentValue} {originalValue}");
		return ENeiliAllocationStatusType.None;
	}

	public static ENeiliAllocationStatusType GetStatus(this NeiliAllocation currentNeiliAllocation, NeiliAllocation originalNeiliAllocation, byte neiliAllocationType)
	{
		if (neiliAllocationType >= 4)
		{
			return ENeiliAllocationStatusType.None;
		}
		short currentValue = currentNeiliAllocation[neiliAllocationType];
		short originalValue = originalNeiliAllocation[neiliAllocationType];
		return GetStatus(currentValue, originalValue);
	}

	public static NeiliAllocationStatusItem GetConfig(this ENeiliAllocationStatusType statusType)
	{
		return NeiliAllocationStatus.Instance[(int)statusType];
	}

	public static byte GetRelatedNeiliAllocationType(this CombatSkillItem combatSkillConfig)
	{
		sbyte equipType = combatSkillConfig.EquipType;
		if (1 == 0)
		{
		}
		byte result = equipType switch
		{
			1 => 0, 
			2 => 1, 
			3 => 2, 
			4 => 3, 
			_ => 4, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static bool IsRelated(this CombatSkillItem combatSkillConfig, byte neiliAllocationType)
	{
		return combatSkillConfig.GetRelatedNeiliAllocationType() == neiliAllocationType;
	}

	public static ENeiliAllocationStatusType GetNeiliAllocationStatus(this CombatCharacter combatChar, byte neiliAllocationType)
	{
		NeiliAllocation neiliAllocation = combatChar.GetNeiliAllocation();
		NeiliAllocation originNeiliAllocation = combatChar.GetOriginNeiliAllocation();
		return neiliAllocation.GetStatus(originNeiliAllocation, neiliAllocationType);
	}

	public static ENeiliAllocationStatusType GetRelatedNeiliAllocationStatus(this CombatCharacter combatChar, CombatSkillItem combatSkillConfig)
	{
		byte relatedNeiliAllocationType = combatSkillConfig.GetRelatedNeiliAllocationType();
		return combatChar.GetNeiliAllocationStatus(relatedNeiliAllocationType);
	}

	public static int GetInjuredRate(this CombatCharacter combatChar, CombatSkillItem combatSkillConfig)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		sbyte injuredRate = DisorderLevelOfQi.GetDisorderLevelOfQiConfig(combatChar.GetCharacter().GetDisorderOfQi()).InjuredRate;
		ENeiliAllocationStatusType relatedNeiliAllocationStatus = combatChar.GetRelatedNeiliAllocationStatus(combatSkillConfig);
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(NeiliAllocationStatus.Instance[(int)relatedNeiliAllocationStatus].GoneMadInjuryRate);
		return (int)injuredRate * val;
	}

	public static int GetGoneMadInjuryTotalPercent(this CombatCharacter combatChar, CombatSkillItem combatSkillConfig)
	{
		ENeiliAllocationStatusType relatedNeiliAllocationStatus = combatChar.GetRelatedNeiliAllocationStatus(combatSkillConfig);
		return relatedNeiliAllocationStatus.GetConfig().GoneMadInjuryBonus;
	}

	public static int GetNeiliAllocationPowerAddPercent(this CombatSkillKey skillKey)
	{
		if (!DomainManager.Combat.IsInCombat() || skillKey.SkillTemplateId < 0)
		{
			return 0;
		}
		if (!DomainManager.Combat.TryGetElement_CombatCharacterDict(skillKey.CharId, out var element))
		{
			return 0;
		}
		CombatSkillItem combatSkillConfig = Config.CombatSkill.Instance[skillKey.SkillTemplateId];
		ENeiliAllocationStatusType relatedNeiliAllocationStatus = element.GetRelatedNeiliAllocationStatus(combatSkillConfig);
		return relatedNeiliAllocationStatus.GetConfig().PowerAddPercent;
	}

	public static int GetAddNeiliAllocationAddPercent(this CombatCharacter combatChar, byte neiliAllocationType)
	{
		return combatChar.GetNeiliAllocationStatus(neiliAllocationType).GetConfig().AddNeiliAllocation;
	}

	public static int GetCostNeiliAllocationAddPercent(this CombatCharacter combatChar, byte neiliAllocationType)
	{
		return combatChar.GetNeiliAllocationStatus(neiliAllocationType).GetConfig().CostNeiliAllocation;
	}
}
