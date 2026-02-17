using System;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.TaiwuEvent.DisplayEvent;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000B0 RID: 176
	public static class OptionConditionModifier
	{
		// Token: 0x06001B9C RID: 7068 RVA: 0x0017CBD0 File Offset: 0x0017ADD0
		public static void ModifyCondition(ref OptionAvailableInfoMinimumElement element, TaiwuEventOptionConditionBase condition, EventArgBox argBox)
		{
			short id = condition.Id;
			short num = id;
			string[] ptr;
			ValueTuple<short, string[]> displayData;
			if (num <= 16)
			{
				if (num == 6)
				{
					OptionConditionAreaSpiritualDebt areaSpiritualDebtCondition = condition as OptionConditionAreaSpiritualDebt;
					bool flag = areaSpiritualDebtCondition != null;
					if (flag)
					{
						Character character = argBox.GetCharacter("CharacterId");
						bool flag2 = character == null;
						if (flag2)
						{
							throw new Exception("error:failed to get character of key CharacterId from argBox when modify AreaSpiritualDebtMore");
						}
						short settlementId = character.GetOrganizationInfo().SettlementId;
						bool flag3 = settlementId < 0;
						if (flag3)
						{
							element.Hide = true;
							element.Pass = true;
						}
						else
						{
							short modifiedValue = DomainManager.Taiwu.GetSpiritualDebtFinalCost(settlementId, areaSpiritualDebtCondition.SpiritualDebtValue);
							OptionConditionAreaSpiritualDebt modifiedCondition = new OptionConditionAreaSpiritualDebt(condition.Id, modifiedValue, areaSpiritualDebtCondition.ConditionChecker);
							element.Pass = modifiedCondition.CheckCondition(argBox);
							ptr = ref element.FormatArgs;
							displayData = modifiedCondition.GetDisplayData(argBox);
							element.ConditionId = displayData.Item1;
							ptr = displayData.Item2;
						}
					}
					return;
				}
				if (num == 16)
				{
					OptionConditionAreaSpiritualDebtKey areaSpiritualDebtKeyCondition = condition as OptionConditionAreaSpiritualDebtKey;
					bool flag4 = areaSpiritualDebtKeyCondition != null;
					if (flag4)
					{
						Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
						location = DomainManager.Map.GetBlock(location).GetRootBlock().GetLocation();
						short settlementId2 = DomainManager.Organization.GetSettlementByLocation(location).GetId();
						short modifiedValue2 = DomainManager.Taiwu.GetSpiritualDebtFinalCost(settlementId2, areaSpiritualDebtKeyCondition.SpiritualDebtValue);
						OptionConditionAreaSpiritualDebt modifiedCondition2 = new OptionConditionAreaSpiritualDebt(condition.Id, modifiedValue2, areaSpiritualDebtKeyCondition.ConditionChecker);
						element.Pass = modifiedCondition2.CheckCondition(settlementId2);
						ptr = ref element.FormatArgs;
						displayData = modifiedCondition2.GetDisplayData(settlementId2);
						element.ConditionId = displayData.Item1;
						ptr = displayData.Item2;
					}
					return;
				}
			}
			else
			{
				if (num == 19)
				{
					OptionConditionInt intCondition = condition as OptionConditionInt;
					bool flag5 = intCondition != null;
					if (flag5)
					{
						OptionConditionInt modifiedCondition3 = new OptionConditionInt(condition.Id, OptionConditionModifier.GetRealTeammateMax(intCondition.Arg), intCondition.ConditionChecker);
						element.Pass = modifiedCondition3.CheckCondition(argBox);
						ptr = ref element.FormatArgs;
						displayData = modifiedCondition3.GetDisplayData(argBox);
						element.ConditionId = displayData.Item1;
						ptr = displayData.Item2;
					}
					return;
				}
				if (num == 37)
				{
					OptionConditionInt intOptionCondition = condition as OptionConditionInt;
					bool flag6 = intOptionCondition != null;
					if (flag6)
					{
						Func<int, bool> conditionChecker = intOptionCondition.ConditionChecker;
						Character character2 = argBox.GetCharacter("CharacterId");
						int value = ProfessionSkillHandle.TravelingTaoistMonkSkill1_ExpCost(character2);
						OptionConditionInt modifiedCondition4 = new OptionConditionInt(condition.Id, value, conditionChecker);
						element.Pass = modifiedCondition4.CheckCondition(argBox);
						ptr = ref element.FormatArgs;
						displayData = modifiedCondition4.GetDisplayData(argBox);
						element.ConditionId = displayData.Item1;
						ptr = displayData.Item2;
					}
					return;
				}
			}
			element.Pass = condition.CheckCondition(argBox);
			ptr = ref element.FormatArgs;
			displayData = condition.GetDisplayData(argBox);
			element.ConditionId = displayData.Item1;
			ptr = displayData.Item2;
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x0017CED4 File Offset: 0x0017B0D4
		private static int GetRealTeammateMax(int srcTeammateMax)
		{
			return DomainManager.Taiwu.GetTaiwuGroupMaxCount();
		}
	}
}
