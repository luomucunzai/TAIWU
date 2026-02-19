using System;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.TaiwuEvent.DisplayEvent;

namespace GameData.Domains.TaiwuEvent.EventOption;

public static class OptionConditionModifier
{
	public static void ModifyCondition(ref OptionAvailableInfoMinimumElement element, TaiwuEventOptionConditionBase condition, EventArgBox argBox)
	{
		switch (condition.Id)
		{
		case 19:
			if (condition is OptionConditionInt optionConditionInt3)
			{
				OptionConditionInt optionConditionInt4 = new OptionConditionInt(condition.Id, GetRealTeammateMax(optionConditionInt3.Arg), optionConditionInt3.ConditionChecker);
				element.Pass = optionConditionInt4.CheckCondition(argBox);
				ref short conditionId5 = ref element.ConditionId;
				ref string[] formatArgs = ref element.FormatArgs;
				(conditionId5, formatArgs) = optionConditionInt4.GetDisplayData(argBox);
			}
			break;
		case 6:
			if (condition is OptionConditionAreaSpiritualDebt optionConditionAreaSpiritualDebt)
			{
				GameData.Domains.Character.Character character2 = argBox.GetCharacter("CharacterId");
				if (character2 == null)
				{
					throw new Exception("error:failed to get character of key CharacterId from argBox when modify AreaSpiritualDebtMore");
				}
				short settlementId = character2.GetOrganizationInfo().SettlementId;
				if (settlementId < 0)
				{
					element.Hide = true;
					element.Pass = true;
					break;
				}
				short spiritualDebtFinalCost = DomainManager.Taiwu.GetSpiritualDebtFinalCost(settlementId, optionConditionAreaSpiritualDebt.SpiritualDebtValue);
				OptionConditionAreaSpiritualDebt optionConditionAreaSpiritualDebt2 = new OptionConditionAreaSpiritualDebt(condition.Id, spiritualDebtFinalCost, optionConditionAreaSpiritualDebt.ConditionChecker);
				element.Pass = optionConditionAreaSpiritualDebt2.CheckCondition(argBox);
				ref short conditionId3 = ref element.ConditionId;
				ref string[] formatArgs = ref element.FormatArgs;
				(conditionId3, formatArgs) = optionConditionAreaSpiritualDebt2.GetDisplayData(argBox);
			}
			break;
		case 16:
			if (condition is OptionConditionAreaSpiritualDebtKey optionConditionAreaSpiritualDebtKey)
			{
				Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
				location = DomainManager.Map.GetBlock(location).GetRootBlock().GetLocation();
				short id = DomainManager.Organization.GetSettlementByLocation(location).GetId();
				short spiritualDebtFinalCost2 = DomainManager.Taiwu.GetSpiritualDebtFinalCost(id, optionConditionAreaSpiritualDebtKey.SpiritualDebtValue);
				OptionConditionAreaSpiritualDebt optionConditionAreaSpiritualDebt3 = new OptionConditionAreaSpiritualDebt(condition.Id, spiritualDebtFinalCost2, optionConditionAreaSpiritualDebtKey.ConditionChecker);
				element.Pass = optionConditionAreaSpiritualDebt3.CheckCondition(id);
				ref short conditionId4 = ref element.ConditionId;
				ref string[] formatArgs = ref element.FormatArgs;
				(conditionId4, formatArgs) = optionConditionAreaSpiritualDebt3.GetDisplayData(id);
			}
			break;
		case 37:
			if (condition is OptionConditionInt { ConditionChecker: var conditionChecker })
			{
				GameData.Domains.Character.Character character = argBox.GetCharacter("CharacterId");
				int arg = ProfessionSkillHandle.TravelingTaoistMonkSkill1_ExpCost(character);
				OptionConditionInt optionConditionInt2 = new OptionConditionInt(condition.Id, arg, conditionChecker);
				element.Pass = optionConditionInt2.CheckCondition(argBox);
				ref short conditionId2 = ref element.ConditionId;
				ref string[] formatArgs = ref element.FormatArgs;
				(conditionId2, formatArgs) = optionConditionInt2.GetDisplayData(argBox);
			}
			break;
		default:
		{
			element.Pass = condition.CheckCondition(argBox);
			ref short conditionId = ref element.ConditionId;
			ref string[] formatArgs = ref element.FormatArgs;
			(conditionId, formatArgs) = condition.GetDisplayData(argBox);
			break;
		}
		}
	}

	private static int GetRealTeammateMax(int srcTeammateMax)
	{
		return DomainManager.Taiwu.GetTaiwuGroupMaxCount();
	}
}
