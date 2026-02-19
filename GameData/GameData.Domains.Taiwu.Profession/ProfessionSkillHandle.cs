using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Creation;
using GameData.Domains.Character.Filters;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Information;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Organization.SettlementPrisonRecord;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession;

public static class ProfessionSkillHandle
{
	public static readonly IReadOnlyList<short> AnimalCharacterTemplateIds = new short[18]
	{
		210, 211, 212, 213, 214, 215, 216, 217, 218, 219,
		220, 221, 222, 223, 224, 225, 226, 227
	};

	public static bool CanExecuteSkill(ProfessionData professionData, int skillIndex)
	{
		ProfessionSkillItem skillConfig = professionData.GetSkillConfig(skillIndex);
		if (skillConfig.IgnoreCanExecuteSkill)
		{
			return true;
		}
		if (skillConfig.TriggerType != EProfessionSkillTriggerType.Active)
		{
			return false;
		}
		if (DomainManager.Taiwu.GetTaiwu().GetExp() < skillConfig.ExpCost)
		{
			return false;
		}
		if (DomainManager.World.GetLeftDaysInCurrMonth() < skillConfig.TimeCost)
		{
			return false;
		}
		if (professionData.IsSkillCooldown(DomainManager.World.GetCurrDate(), skillIndex))
		{
			return false;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		ResourceInts resources = taiwu.GetResources();
		foreach (ResourceInfo item in skillConfig.ResourcesCost)
		{
			if (!resources.CheckIsMeet(item.ResourceType, item.ResourceCount))
			{
				return false;
			}
		}
		if (!CheckSpecialCondition(professionData, skillIndex))
		{
			return false;
		}
		return true;
	}

	public static int GetSkillIndex(ProfessionSkillItem skillCfg)
	{
		ProfessionItem professionItem = Config.Profession.Instance[skillCfg.Profession];
		return (professionItem.ExtraProfessionSkill == skillCfg.TemplateId) ? professionItem.ProfessionSkills.Length : professionItem.ProfessionSkills.IndexOf(skillCfg.TemplateId);
	}

	public static bool CheckSpecialCondition(int professionId, int skillIndex)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(professionId);
		return CheckSpecialCondition(professionData, skillIndex);
	}

	public static bool CheckSpecialCondition(ProfessionData professionData, int skillIndex)
	{
		return professionData.TemplateId switch
		{
			0 => CheckSpecialCondition_SavageSkill(professionData, skillIndex), 
			1 => CheckSpecialCondition_HunterSkill(professionData, skillIndex), 
			3 => CheckSpecialCondition_MartialArtistSkill(professionData, skillIndex), 
			4 => CheckSpecialCondition_LiteratiSkill(professionData, skillIndex), 
			5 => CheckSpecialCondition_TaoistMonkSkill(professionData, skillIndex), 
			6 => CheckSpecialCondition_BuddhistMonkSkill(professionData, skillIndex), 
			7 => CheckSpecialCondition_WineTasterSkill(professionData, skillIndex), 
			8 => CheckSpecialCondition_AristocratSkill(professionData, skillIndex), 
			9 => CheckSpecialCondition_BeggarSkill(professionData, skillIndex), 
			10 => CheckSpecialCondition_CivilianSkill(professionData, skillIndex), 
			12 => CheckSpecialCondition_TravelingBuddhistMonkSkill(professionData, skillIndex), 
			13 => CheckSpecialCondition_DoctorSkill(professionData, skillIndex), 
			14 => CheckSpecialCondition_TravelingTaoistMonkSkill(professionData, skillIndex), 
			15 => CheckSpecialCondition_CapitalistSkill(professionData, skillIndex), 
			16 => CheckSpecialCondition_TeaTasterSkill(professionData, skillIndex), 
			17 => CheckSpecialCondition_DukeSkill(professionData, skillIndex), 
			_ => true, 
		};
	}

	public static void OnSkillExecuted(DataContext context, ref ProfessionSkillArg arg)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(arg.ProfessionId);
		int skillIndex = professionData.GetSkillIndex(arg.SkillId);
		ProfessionSkillItem skillConfig = professionData.GetSkillConfig(skillIndex);
		if (!DomainManager.Extra.NoProfessionSkillCost)
		{
			if (!skillConfig.CostTimeWhenFinished)
			{
				DomainManager.World.AdvanceDaysInMonth(context, skillConfig.TimeCost);
			}
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			foreach (ResourceInfo item in skillConfig.ResourcesCost)
			{
				taiwu.ChangeResource(context, item.ResourceType, -item.ResourceCount);
			}
			DomainManager.Taiwu.GetTaiwu().ChangeExp(context, -skillConfig.ExpCost);
		}
		professionData.OfflineSkillCooldown(skillIndex);
		DomainManager.Extra.SetProfessionData(context, professionData);
		if (skillConfig.Type == EProfessionSkillType.Interactive)
		{
			DomainManager.TaiwuEvent.SetIsSequential(value: true);
		}
	}

	public static void OnActiveSkillExecuted(DataContext context, ref ProfessionSkillArg arg)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(arg.ProfessionId);
		int skillIndex = professionData.GetSkillIndex(arg.SkillId);
		ProfessionSkillItem skillConfig = professionData.GetSkillConfig(skillIndex);
		if (skillConfig.Type == EProfessionSkillType.Active)
		{
			switch (professionData.TemplateId)
			{
			case 0:
				ExecuteOnClick_SavageSkill(context, professionData, skillIndex, ref arg);
				break;
			case 1:
				ExecuteOnClick_HunterSkill(context, professionData, skillIndex, ref arg);
				break;
			case 2:
				ExecuteOnClick_CraftSkill(context, professionData, skillIndex, ref arg);
				break;
			case 3:
				ExecuteOnClick_MartialArtist(context, professionData, skillIndex, ref arg);
				break;
			case 4:
				break;
			case 5:
				ExecuteOnClick_TaoistMonkSkill(context, professionData, skillIndex, ref arg);
				break;
			case 6:
				ExecuteOnClick_BuddhistMonkSkill(context, professionData, skillIndex, ref arg);
				break;
			case 7:
				ExecuteOnClick_WineTasterSkill(context, professionData, skillIndex, ref arg);
				break;
			case 8:
				ExecuteOnClick_AristocratSkill(context, professionData, skillIndex, ref arg);
				break;
			case 9:
				ExecuteOnClick_BeggarSkill(context, professionData, skillIndex, ref arg);
				break;
			case 10:
				ExecuteOnClick_CivilianSkill(context, professionData, skillIndex, ref arg);
				break;
			case 11:
				ExecuteOnClick_TravelerSkill(context, professionData, skillIndex, ref arg);
				break;
			case 12:
				ExecuteOnClick_TravelingBuddhistMonkSkill(context, professionData, skillIndex, ref arg);
				break;
			case 13:
				ExecuteOnClick_DoctorSkill(context, professionData, skillIndex, ref arg);
				break;
			case 14:
				ExecuteOnClick_TravelingTaoistMonkSkill(context, professionData, skillIndex, ref arg);
				break;
			case 15:
				break;
			case 16:
				ExecuteOnClick_TeaTasterSkill(context, professionData, skillIndex, ref arg);
				break;
			case 17:
				ExecuteOnClick_DukeSkill(context, professionData, skillIndex, ref arg);
				break;
			}
		}
	}

	public static void ConfirmSkillExecute(ref ProfessionSkillArg professionSkillArg)
	{
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.ConfirmProfessionSkillExecute, professionSkillArg);
	}

	public static void ConfirmSkillExecuteWithEvent(ProfessionSkillArg professionSkillArg, string afterEvent, EventArgBox argBox)
	{
		if (!string.IsNullOrEmpty(afterEvent))
		{
			EventHelper.AddEventInListenWithActionName(afterEvent, argBox, "ConfirmProfessionSkillExecuteAndAnimComplete");
		}
		int arg = 0;
		if (argBox.Contains<int>("BeggarMoneyCount"))
		{
			arg = argBox.GetInt("BeggarMoneyCount");
		}
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.ConfirmProfessionSkillExecute, professionSkillArg, arg);
	}

	public static void ExecuteActiveProfessionSkill(int professionId, int skillIndex)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(professionId);
		ProfessionSkillItem skillConfig = professionData.GetSkillConfig(skillIndex);
		DomainManager.TaiwuEvent.OnEvent_ProfessionSkillClicked(skillConfig.TemplateId);
		if (skillConfig.Instant)
		{
			ProfessionSkillArg professionSkillArg = new ProfessionSkillArg
			{
				ProfessionId = professionId,
				SkillId = skillConfig.TemplateId,
				IsSuccess = true,
				SkipAnimation = (skillConfig.TemplateId == 18 || skillConfig.TemplateId == 63)
			};
			ConfirmSkillExecute(ref professionSkillArg);
			return;
		}
		switch (skillConfig.TemplateId)
		{
		case 60:
			OpenItemSelectFromBlock();
			break;
		case 8:
			OpenSetEquipmentEffect();
			break;
		case 37:
		case 41:
		case 58:
		case 65:
		case 67:
		case 68:
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenProfessionSkillSpecial, skillConfig.TemplateId);
			break;
		case 14:
		{
			SecretInformationDisplayPackage secretInformationDisplayPackageFromCharacter = DomainManager.Information.GetSecretInformationDisplayPackageFromCharacter(DomainManager.Taiwu.GetTaiwuCharId());
			if (secretInformationDisplayPackageFromCharacter.SecretInformationDisplayDataList != null)
			{
				foreach (SecretInformationDisplayData secretInformationDisplayData in secretInformationDisplayPackageFromCharacter.SecretInformationDisplayDataList)
				{
					SecretInformationItem item = SecretInformation.Instance.GetItem(secretInformationDisplayData.SecretInformationTemplateId);
					secretInformationDisplayData.AuthorityCostWhenDisseminatingForBroadcast = CalcLiteratiSkill2AuthorityCost(item, secretInformationDisplayData.HolderCount);
				}
			}
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenSelectSecretInformationLiteratiSkill2, secretInformationDisplayPackageFromCharacter);
			break;
		}
		case 64:
		{
			NormalInformationCollection characterNormalInformation = DomainManager.Information.GetCharacterNormalInformation(DomainManager.Taiwu.GetTaiwuCharId());
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenSelectNormalInformationLiteratiSkill3, characterNormalInformation.GetList().Where(CheckLiteratiSkillNormalInformationUsable).ToList());
			break;
		}
		case 57:
		{
			List<ItemDisplayData> list = new List<ItemDisplayData>();
			foreach (KeyValuePair<ItemKey, int> item2 in DomainManager.Taiwu.GetTaiwu().GetInventory().Items)
			{
				if (item2.Key.ItemType == 11 && DomainManager.Item.TryGetElement_Crickets(item2.Key.Id, out var element) && !element.IsAlive)
				{
					list.Add(DomainManager.Item.GetItemDisplayData(item2.Key));
				}
			}
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenSelectCricketDukeSkill2, list);
			break;
		}
		case 51:
			OpenInvestCaravan();
			break;
		}
	}

	public static void OnPreAdvanceMonth(DataContext context)
	{
		TravelingTaoistMonkSkill_OnPreAdvanceMonth(context);
	}

	public static void OnPostAdvanceMonth(DataContext context)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		TeaTasterSkill_UpdateVillagerLearnSkillInterval(context);
		TeaTasterSkill_SetActionPointGained(context, 0);
		WineTasterSkill_UpdateVillagerLearnSkillInterval(context);
		BeggarSkill_AdvanceMonth(context);
		TaoistMonkSkill_OnPostAdvanceMonth(context);
		UpdateSeniorityOnPostAdvanceMonth(context, taiwu);
		UpdateDukeMonthlyEvent(context);
		int currDate = DomainManager.World.GetCurrDate();
		TaiwuProfessionSkillSlots taiwuProfessionSkillSlots = DomainManager.Extra.GetTaiwuProfessionSkillSlots();
		IntList[] slots = taiwuProfessionSkillSlots.Slots;
		for (int i = 0; i < slots.Length; i++)
		{
			IntList intList = slots[i];
			foreach (int item in intList.Items)
			{
				if (item < 0)
				{
					continue;
				}
				ProfessionSkillItem professionSkillItem = ProfessionSkill.Instance[item];
				if (professionSkillItem.Type == EProfessionSkillType.Passive)
				{
					continue;
				}
				ProfessionData professionData = DomainManager.Extra.GetProfessionData(professionSkillItem.Profession);
				int num = professionSkillItem.Level - 1;
				if (professionData.IsSkillUnlocked(num))
				{
					int num2 = professionData.SkillOffCooldownDates[num];
					if (num2 != 0 && currDate == num2)
					{
						DomainManager.World.GetInstantNotificationCollection().AddProfessionSkillHasCoolDown(professionSkillItem.TemplateId);
					}
				}
			}
		}
	}

	private static bool IsSkillUnlocked(int skillTemplateId)
	{
		return DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(skillTemplateId);
	}

	private static void UpdateSeniorityOnPostAdvanceMonth(DataContext context, GameData.Domains.Character.Character character)
	{
		ItemKey itemKey = character.GetEquipment()[4];
		if (!itemKey.IsValid() || DomainManager.Item.GetBaseItem(itemKey).IsDurabilityRunningOut())
		{
			return;
		}
		foreach (ProfessionItem item in (IEnumerable<ProfessionItem>)Config.Profession.Instance)
		{
			if (item.BonusClothing != itemKey.TemplateId)
			{
				continue;
			}
			DomainManager.Extra.ChangeProfessionSeniority(context, item.TemplateId, GlobalConfig.Instance.ProfessionSeniorityPerMonth);
			break;
		}
	}

	private static void UpdateDukeMonthlyEvent(DataContext context)
	{
		SeasonItem seasonItem = Season.Instance[(sbyte)2];
		sbyte currMonthInYear = DomainManager.World.GetCurrMonthInYear();
		if (seasonItem.Months.Contains(currMonthInYear))
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(17);
			DukeSkillsData dukeSkillsData = (DukeSkillsData)professionData.SkillsData;
			if (dukeSkillsData.GetNotGivenCricketTitles(DomainManager.Character.IsCharacterAlive).Any())
			{
				DomainManager.World.GetMonthlyEventCollection().AddProfessionDukeReceiveCricket(DomainManager.Taiwu.GetTaiwuCharId());
			}
		}
		else if (currMonthInYear == 10)
		{
			ProfessionData professionData2 = DomainManager.Extra.GetProfessionData(17);
			DukeSkillsData dukeSkillsData2 = (DukeSkillsData)professionData2.SkillsData;
			dukeSkillsData2.ResetAllCricketGivenData();
			DomainManager.Extra.SetProfessionData(context, professionData2);
		}
	}

	public static void UnpackCrossArchiveProfession(DataContext context, int professionId)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(professionId);
		ProfessionItem professionItem = Config.Profession.Instance[professionId];
		if (professionData.SkillsData != null && professionItem.ReinitOnCrossArchive)
		{
			professionData.SkillsData.Initialize();
			DomainManager.Extra.SetProfessionData(context, professionData);
		}
		switch (professionId)
		{
		case 5:
			UnpackCrossArchiveProfession_TaoistMonk(context);
			break;
		case 6:
			UnpackCrossArchiveProfession_BuddhistMonk(context);
			break;
		}
	}

	public static void OnTaiwuDeath(DataContext context)
	{
		TaoistMonkSkill_ResetSurvivedTribulationCount(context);
		BuddhistMonkSkill_ClearSavedSoulCount(context);
		TravelingTaoistMonkSkill_ClearHealthBonus(context);
	}

	public static void AristocratSkill_ChangeInfluencePower(DataContext context, int charId, bool isAdd)
	{
		SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(charId);
		short influencePower = settlementCharacter.GetInfluencePower();
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(8);
		int num = 50 + 50 * professionData.GetSeniorityPercent() / 100;
		AristocratSkillsData skillsData = professionData.GetSkillsData<AristocratSkillsData>();
		int num2 = influencePower * num / 100;
		if (isAdd)
		{
			skillsData.OfflineAddRecommendedCharId(charId);
		}
		else
		{
			num2 = -num2;
		}
		skillsData.OfflineSetInfluencePowerBonus(charId, (short)num2);
		DomainManager.Extra.SetProfessionData(context, professionData);
		short num3 = (short)Math.Clamp(influencePower + num2, 0, 32767);
		settlementCharacter.SetInfluencePower(num3, context);
		int value = Math.Abs(num3 - influencePower);
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (DomainManager.Taiwu.GetTaiwu().GetLocation().IsValid())
		{
			MapBlockData block = DomainManager.Map.GetBlock(location);
			DomainManager.Map.SetBlockData(context, block);
			InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
			if (isAdd)
			{
				instantNotificationCollection.AddRecommendFellowUp(charId, value, num3);
			}
			else
			{
				instantNotificationCollection.AddRecommendFellowDown(charId, value, num3);
			}
		}
	}

	public static void AristocratSkill_BoostTaiwuAsTargetInCollection(List<int> collection)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		for (int i = 0; i < 4; i++)
		{
			collection.Add(taiwuCharId);
		}
	}

	private static void ExecuteOnClick_AristocratSkill4(DataContext context)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int id = taiwu.GetId();
		Location location = taiwu.GetLocation();
		MapBlockData block = DomainManager.Map.GetBlock(location);
		MapBlockData rootBlock = block.GetRootBlock();
		Settlement settlementByLocation = DomainManager.Organization.GetSettlementByLocation(rootBlock.GetLocation());
		short id2 = settlementByLocation.GetId();
		sbyte orgTemplateId = settlementByLocation.GetOrgTemplateId();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		SettlementPrisonRecordCollection settlementPrisonRecordCollection = DomainManager.Extra.GetSettlementPrisonRecordCollection(context, id2);
		if (!(settlementByLocation is Sect { Prison: var prison } sect))
		{
			return;
		}
		int count = prison.Prisoners.Count;
		int num = 0;
		for (int num2 = count - 1; num2 >= 0; num2--)
		{
			SettlementPrisoner settlementPrisoner = prison.Prisoners[num2];
			if (DomainManager.Character.TryGetElement_Objects(settlementPrisoner.CharId, out var element) && !element.IsCompletelyInfected())
			{
				sect.RemovePrisoner(context, settlementPrisoner.CharId);
				num++;
				OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(orgTemplateId, element.GetOrganizationInfo().Grade);
				sbyte rejoinGrade = orgMemberConfig.GetRejoinGrade();
				OrganizationInfo destOrgInfo = new OrganizationInfo(orgTemplateId, rejoinGrade, principal: true, id2);
				int num3 = settlementPrisoner.Duration - (currDate - settlementPrisoner.KidnapBeginDate);
				DomainManager.Character.ChangeFavorabilityOptional(context, element, taiwu, 2500 * settlementPrisoner.PunishmentSeverity * num3 / settlementPrisoner.Duration, 3);
				lifeRecordCollection.AddAristocratReleasePrisoner(id, currDate, id2);
				lifeRecordCollection.AddPrisonerBeReleaseByAristocrat(settlementPrisoner.CharId, currDate, id, id2);
				DomainManager.Organization.ChangeOrganization(context, element, destOrgInfo);
				SectCharacter element_SectCharacters = DomainManager.Organization.GetElement_SectCharacters(settlementPrisoner.CharId);
				element_SectCharacters.SetApprovedTaiwu(context, approve: true);
				settlementPrisonRecordCollection.AddPrisonerBeReleaseByAristocrat(currDate, id2, settlementPrisoner.CharId, id);
				DomainManager.Extra.SetSettlementPrisonRecordCollection(context, id2, settlementPrisonRecordCollection);
			}
		}
		if (num > 0)
		{
			DomainManager.World.GetInstantNotificationCollection().AddReleasePrisoners(id2, num);
		}
	}

	private static void ExecuteOnClick_AristocratSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
	{
		if (index == 3)
		{
			ExecuteOnClick_AristocratSkill4(context);
			return;
		}
		throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
	}

	private static bool CheckSpecialCondition_AristocratSkill(ProfessionData professionData, int index)
	{
		if (index == 3)
		{
			return DomainManager.Extra.CheckAristocratUltimateSpecialCondition() == 0;
		}
		return true;
	}

	private static bool CheckSpecialCondition_BeggarSkill(ProfessionData professionData, int index)
	{
		return index switch
		{
			0 => CheckSpecialCondition_BeggarSkill_1(professionData), 
			1 => true, 
			3 => CheckSpecialCondition_BeggarSkill_4(professionData), 
			_ => true, 
		};
	}

	private static bool CheckSpecialCondition_BeggarSkill_1(ProfessionData professionData)
	{
		sbyte seniorityBeggarMaxSettlementType = professionData.GetSeniorityBeggarMaxSettlementType();
		return CheckSettlementBlockTypeValid(seniorityBeggarMaxSettlementType);
	}

	private static bool CheckSpecialCondition_BeggarSkill_4(ProfessionData professionData)
	{
		return DomainManager.Extra.CheckBeggarUltimateSpecialCondition() == 0;
	}

	private static void ExecuteOnClick_BeggarSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
	{
		switch (index)
		{
		case 1:
			ExecuteOnClick_BeggarSkill2(context);
			break;
		case 3:
			ExecuteOnClick_BeggarSkill4(context, arg.CharId, arg.ItemKey);
			break;
		default:
			throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
		}
	}

	public static int BeggarSkill_GetBeggingMoney(DataContext context, GameData.Domains.Character.Character character)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
		int num = 10 + 90 * professionData.GetSeniorityPercent() / 100;
		int num2 = -taiwu.GetHappiness();
		int num3 = taiwu.GetInjuries().GetSum() * 3;
		int num4 = taiwu.GetPoisonMarkCount() * 9;
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(num2 + num3 + num4);
		CValuePercent val2 = CValuePercent.op_Implicit((int)ProfessionRelatedConstants.BeggarMoneyBehaviorTypeFactors[character.GetBehaviorType()]);
		int val3 = num * val * val2;
		return Math.Min(val3, character.GetResource(6) * ProfessionRelatedConstants.BeggarMoneyMaxPercent);
	}

	public static int BeggarSkill_GetLocationBeggingMoney(DataContext context, Location location, bool isTransferMoney = false)
	{
		MapBlockData blockData = DomainManager.Map.GetBlockData(location.AreaId, location.BlockId);
		HashSet<int> characterSet = blockData.CharacterSet;
		int num = 0;
		foreach (int item in characterSet)
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element))
			{
				int num2 = BeggarSkill_GetBeggingMoney(context, element);
				if (isTransferMoney)
				{
					element.ChangeResource(context, 6, -num2);
				}
				num += num2;
			}
		}
		return num;
	}

	public static void BeggarSkill_AddLookingForCharacter(DataContext context, string name)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
		BeggarSkillsData beggarSkillsData = (BeggarSkillsData)professionData.SkillsData;
		beggarSkillsData.LookingForCharName = name;
		beggarSkillsData.AlreadyFoundCharacters.Clear();
		DomainManager.Extra.SetProfessionData(context, professionData);
	}

	public static void BeggarSkill_ClearLookingForCharacter(DataContext context)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
		BeggarSkillsData beggarSkillsData = (BeggarSkillsData)professionData.SkillsData;
		beggarSkillsData.LookingForCharName = null;
		beggarSkillsData.AlreadyFoundCharacters.Clear();
		DomainManager.Extra.SetProfessionData(context, professionData);
	}

	public static string BeggarSkill_LookingForCharacter()
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
		BeggarSkillsData beggarSkillsData = (BeggarSkillsData)professionData.SkillsData;
		return beggarSkillsData.LookingForCharName;
	}

	public static bool BeggarSkill_FoundMoreAlive()
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
		BeggarSkillsData beggarSkillsData = (BeggarSkillsData)professionData.SkillsData;
		return beggarSkillsData.FoundMoreAlive;
	}

	public static bool BeggarSkill_FoundMoreDead()
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
		BeggarSkillsData beggarSkillsData = (BeggarSkillsData)professionData.SkillsData;
		return beggarSkillsData.FoundMoreDead;
	}

	public static void ExecuteOnClick_BeggarSkill2(DataContext context)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		int num = 3;
		List<MapBlockData> neighborMapBlockList = ObjectPool<List<MapBlockData>>.Instance.Get();
		DomainManager.Map.GetLocationByDistance(location, num, num, ref neighborMapBlockList);
		MapBlockData mapBlockData = DomainManager.Map.GetAreaBlocks(location.AreaId)[location.BlockId];
		List<int> list = new List<int>();
		if (mapBlockData.CharacterSet != null)
		{
			list.AddRange(mapBlockData.CharacterSet);
		}
		if (list.Count > 0)
		{
			list.ForEach(delegate(int charId)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
				if (element_Objects.GetId() != taiwu.GetId())
				{
					DomainManager.Character.GroupMove(context, element_Objects, SelectRandomValidTargetLocation(ref neighborMapBlockList));
				}
			});
		}
		BeggarSkill_AddCurrentLocationForbiddenMapBlock(context, location);
		DomainManager.World.GetInstantNotificationCollection().AddDriveAwayPeople(location);
		ObjectPool<List<MapBlockData>>.Instance.Return(neighborMapBlockList);
		Location SelectRandomValidTargetLocation(ref List<MapBlockData> reference)
		{
			CollectionUtils.Shuffle(context.Random, reference);
			Location result = Location.Invalid;
			foreach (MapBlockData item in reference)
			{
				Location location2 = item.GetLocation();
				if (location2.IsValid() && !IsLocationForbiddenByBeggarSkill(location2))
				{
					result = location2;
					break;
				}
			}
			return result;
		}
	}

	public static void ExecuteOnClick_BeggarSkill4(DataContext context, int charId, ItemKey itemKey)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
		int favorabilityChange = baseItem.GetFavorabilityChange();
		sbyte happinessChange = baseItem.GetHappinessChange();
		ItemKey itemKey2 = itemKey;
		int amount = 1;
		if (ItemTemplateHelper.IsTianJieFuLu(itemKey.ItemType, itemKey.TemplateId))
		{
			amount = ItemTemplateHelper.GetTianJieFuLuCountUnit();
			itemKey2 = DomainManager.Item.CreateItem(context, 8, 432);
			taiwu.AddInventoryItem(context, itemKey2, 1);
		}
		element_Objects.RemoveInventoryItem(context, itemKey, amount, deleteItem: false);
		taiwu.AddEatingItem(context, itemKey2);
		DomainManager.Item.RemoveItem(context, itemKey);
		DomainManager.Character.ChangeFavorabilityOptional(context, element_Objects, taiwu, favorabilityChange, 0);
		DomainManager.Character.AddFavorabilityChangeInstantNotification(element_Objects, taiwu, favorabilityChange > 0);
		element_Objects.ChangeHappiness(context, happinessChange);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		lifeRecordCollection.AddBeggarEatSomeoneFood(taiwu.GetId(), currDate, element_Objects.GetId(), taiwu.GetLocation(), itemKey.ItemType, itemKey.TemplateId);
	}

	public static bool IsLocationForbiddenByBeggarSkill(Location location)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
		BeggarSkillsData beggarSkillsData = (BeggarSkillsData)professionData.SkillsData;
		return beggarSkillsData.ForbiddenLocations != null && beggarSkillsData.ForbiddenLocations.Contains(location);
	}

	private static void BeggarSkill_AddCurrentLocationForbiddenMapBlock(DataContext context, Location targetLocation)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
		BeggarSkillsData beggarSkillsData = (BeggarSkillsData)professionData.SkillsData;
		if (beggarSkillsData.ForbiddenLocations == null)
		{
			beggarSkillsData.ForbiddenLocations = new List<Location>();
		}
		beggarSkillsData.ForbiddenLocations.Add(targetLocation);
		DomainManager.Extra.SetProfessionData(context, professionData);
	}

	private static void BeggarSkill_AdvanceMonth(DataContext context)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
		BeggarSkillsData skillsData = professionData.GetSkillsData<BeggarSkillsData>();
		skillsData?.ForbiddenLocations?.Clear();
		DomainManager.Extra.SetProfessionData(context, professionData);
		if (!IsSkillUnlocked(31))
		{
			return;
		}
		skillsData.FoundMoreAlive = false;
		skillsData.FoundMoreDead = false;
		if (string.IsNullOrEmpty(skillsData.LookingForCharName))
		{
			return;
		}
		HashSet<int> exceptions = skillsData.AlreadyFoundCharacters.GetCollection();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		List<GameData.Domains.Character.Character> list = new List<GameData.Domains.Character.Character>();
		MapCharacterFilter.ParallelFind(CharacterPredicate, list, 0, 135);
		MapCharacterFilter.FindTraveling(CharacterPredicate, list);
		Predicate<GameData.Domains.Character.Character> match = (GameData.Domains.Character.Character character) => character.IsActiveExternalRelationState(60);
		if (list.Count > 0)
		{
			if (list.TrueForAll(match))
			{
				GameData.Domains.Character.Character random = list.GetRandom(context.Random);
				int id = random.GetId();
				monthlyEventCollection.AddBeggerSkill2TargetUnavailable(id);
				skillsData.AlreadyFoundCharacters.Add(id);
				DomainManager.Extra.SetProfessionData(context, professionData);
				return;
			}
			list.RemoveAll(match);
			GameData.Domains.Character.Character random2 = list.GetRandom(context.Random);
			monthlyEventCollection.AddBeggarSkill2TargetBrought(random2.GetId(), random2.GetLocation());
			skillsData.FoundMoreAlive = list.Count > 1;
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Location targetLocation = taiwu.GetLocation();
			if (!targetLocation.IsValid())
			{
				targetLocation = taiwu.GetValidLocation();
			}
			DomainManager.Character.GroupMove(context, random2, targetLocation);
			skillsData.AlreadyFoundCharacters.Add(random2.GetId());
			DomainManager.Extra.SetProfessionData(context, professionData);
			return;
		}
		MapCharacterFilter.FindHiddenCharacters(CharacterPredicate, list);
		MapCharacterFilter.FindKidnappedCharacters(CharacterPredicate, list);
		if (list.Count > 0)
		{
			GameData.Domains.Character.Character random3 = list.GetRandom(context.Random);
			int id2 = random3.GetId();
			monthlyEventCollection.AddBeggerSkill2TargetUnavailable(id2);
			skillsData.AlreadyFoundCharacters.Add(id2);
			DomainManager.Extra.SetProfessionData(context, professionData);
			return;
		}
		List<Grave> list2 = new List<Grave>();
		DomainManager.Character.FindGrave(GravePredicate, list2);
		if (list2.Count > 0)
		{
			Grave random4 = list2.GetRandom(context.Random);
			int id3 = random4.GetId();
			monthlyEventCollection.AddBeggarSkill2TargetDead(id3, random4.GetLocation());
			skillsData.FoundMoreDead = list2.Count > 0;
			skillsData.AlreadyFoundCharacters.Add(id3);
			DomainManager.Extra.SetProfessionData(context, professionData);
		}
		else
		{
			monthlyEventCollection.AddBeggarSkill2TargetNoneExistent(skillsData.LookingForCharName);
		}
		bool CharacterPredicate(GameData.Domains.Character.Character character)
		{
			if (character.GetAgeGroup() == 0)
			{
				return false;
			}
			if (character.GetLegendaryBookOwnerState() >= 2)
			{
				return false;
			}
			if (!CharacterMatchers.MatchMonasticTitleOrDisplayName(character, skillsData.LookingForCharName))
			{
				return false;
			}
			if (exceptions != null && exceptions.Contains(character.GetId()))
			{
				return false;
			}
			return true;
		}
		bool GravePredicate(Grave grave)
		{
			int id4 = grave.GetId();
			if (exceptions != null && exceptions.Contains(id4))
			{
				return false;
			}
			var (text, text2) = DomainManager.Character.GetNameRelatedData(id4).GetMonasticTitleOrDisplayName(isTaiwu: false);
			return skillsData.LookingForCharName == text + text2;
		}
	}

	private static bool CheckSettlementBlockTypeValid(sbyte settlementType)
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (!location.IsValid())
		{
			return false;
		}
		MapBlockData block = DomainManager.Map.GetBlock(location);
		MapBlockData rootBlock = block.GetRootBlock();
		if (!rootBlock.IsCityTown())
		{
			return false;
		}
		return settlementType switch
		{
			0 => rootBlock.BlockSubType == EMapBlockSubType.Village || rootBlock.BlockSubType == EMapBlockSubType.TaiwuCun, 
			1 => rootBlock.BlockType != EMapBlockType.Sect && rootBlock.BlockType != EMapBlockType.City && rootBlock.BlockSubType != EMapBlockSubType.Town, 
			2 => rootBlock.BlockType == EMapBlockType.Town, 
			3 => true, 
			_ => false, 
		};
	}

	private static void ExecuteOnClick_BuddhistMonkSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
	{
		if (index == 3)
		{
			BuddhistMonkSkill_SetSamsaraFeature(context, DomainManager.Taiwu.GetTaiwu().GetId(), arg.EffectId);
			return;
		}
		throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
	}

	public static void BuddhistMonkSkill_SelectDirectedSamsara(DataContext context, int motherId, int reincarnatedCharId)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
		BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
		skillsData.OfflineAddDirectedSamsara(motherId, reincarnatedCharId);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		DomainManager.Extra.SetProfessionData(context, professionData);
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(motherId);
		if (!DomainManager.Character.TryGetPregnantState(motherId, out var _))
		{
			DomainManager.Character.RemovePregnantLock(context, motherId);
			DomainManager.Character.MakePregnantWithoutMale(context, element_Objects);
		}
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		lifeRecordCollection.AddTaiwuReincarnationPregnancy(motherId, currDate, element_Objects.GetLocation());
		lifeRecordCollection.AddTaiwuReincarnation(reincarnatedCharId, currDate, motherId, taiwu.GetLocation(), element_Objects.GetOrganizationInfo().SettlementId);
	}

	public static void BuddhistMonkSkill_TryRemoveDirectedSamsara(DataContext context, int motherId, bool addMonthlyEvent = true)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
		BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
		int charId = skillsData.GetDirectedSamsara(motherId);
		if (!skillsData.OfflineRemoveDirectedSamsara(motherId))
		{
			return;
		}
		DomainManager.Extra.SetProfessionData(context, professionData);
		if (addMonthlyEvent)
		{
			if (DomainManager.World.GetAdvancingMonthState() != 0)
			{
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				monthlyEventCollection.AddTaiwuComingDefeated(taiwuCharId, charId);
			}
			else
			{
				Events.RegisterHandler_PostAdvanceMonthBegin(AddTaiwuComingEvent);
			}
		}
		void AddTaiwuComingEvent(DataContext dataContext)
		{
			MonthlyEventCollection monthlyEventCollection2 = DomainManager.World.GetMonthlyEventCollection();
			int taiwuCharId2 = DomainManager.Taiwu.GetTaiwuCharId();
			monthlyEventCollection2.AddTaiwuComingDefeated(taiwuCharId2, charId);
			Events.UnRegisterHandler_PostAdvanceMonthBegin(AddTaiwuComingEvent);
		}
	}

	public static bool BuddhistMonkSkill_IsDirectedSamsaraMother(int motherId)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
		BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
		return skillsData.GetDirectedSamsara(motherId) >= 0;
	}

	public static bool BuddhistMonkSkill_IsDirectedSamsaraCharacter(int reincarnatedCharId)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
		BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
		return skillsData.IsDirectedSamsaraCharacter(reincarnatedCharId);
	}

	public static void BuddhistMonkSkill_SetSamsaraFeature(DataContext context, int reincarnatedCharId, short featureID)
	{
		if (reincarnatedCharId != -1)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
			BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
			skillsData.OfflineAddSamsaraFeature(reincarnatedCharId, featureID);
			DomainManager.Extra.SetProfessionData(context, professionData);
		}
	}

	public static bool BuddhistMonkSkill_TryGetSamsaraFeature(int reincarnatedCharId, out short featureID)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
		BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
		return skillsData.TryGetSamaraFeature(reincarnatedCharId, out featureID);
	}

	public static bool BuddhistMonkSkill_RemoveSamsaraFeature(DataContext context, int reincarnatedCharId)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
		BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
		if (skillsData.OfflineRemoveSamsaraFeature(reincarnatedCharId))
		{
			DomainManager.Extra.SetProfessionData(context, professionData);
			return true;
		}
		return false;
	}

	public static int BuddhistMonkSkill_GetDirectedSamsaraMother(int reincarnatedCharId)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
		BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
		return skillsData.GetDirectedSamsaraMother(reincarnatedCharId);
	}

	private static void BuddhistMonkSkill_ClearSavedSoulCount(DataContext context)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
		BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
		skillsData.OfflineClearSavedSoulsCount();
		DomainManager.Extra.SetProfessionData(context, professionData);
	}

	private static void UnpackCrossArchiveProfession_BuddhistMonk(DataContext context)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
		BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
		skillsData.OfflineClearDirectedSamsara();
		DomainManager.Extra.SetProfessionData(context, professionData);
	}

	private static bool CheckSpecialCondition_BuddhistMonkSkill(ProfessionData professionData, int index)
	{
		if (index == 3)
		{
			return CheckSpecialCondition_BuddhistMonkSkill_3(professionData);
		}
		return true;
	}

	private static bool CheckSpecialCondition_BuddhistMonkSkill_3(ProfessionData professionData)
	{
		BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
		return skillsData.GetSavedSoulsCount() >= 100;
	}

	private static bool CheckSpecialCondition_CapitalistSkill(ProfessionData professionData, int index)
	{
		return true;
	}

	public static void OpenInvestCaravan()
	{
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenInvestCaravan);
	}

	private static void ExecuteOnClick_CivilianSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
	{
		if (index == 1)
		{
			ExecuteOnClick_CivilianSkill_1(context, professionData);
			return;
		}
		throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
	}

	private static void ExecuteOnClick_CivilianSkill_1(DataContext context, ProfessionData professionData)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		Location location = taiwu.GetLocation();
		if (!location.IsValid())
		{
			return;
		}
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		int num = 0;
		int seniorityCivilianSeverHatredLimit = ProfessionData.GetSeniorityCivilianSeverHatredLimit(professionData.Seniority);
		List<int> list = new List<int>();
		List<(int, int)> list2 = new List<(int, int)>();
		MapBlockData block = DomainManager.Map.GetBlock(location);
		if (block.CharacterSet != null && block.CharacterSet.Count != 0)
		{
			foreach (int item3 in block.CharacterSet)
			{
				list.Add(item3);
			}
		}
		foreach (int item4 in DomainManager.Taiwu.GetGroupCharIds().GetCollection())
		{
			if (item4 != taiwuCharId)
			{
				list.Add(item4);
			}
		}
		foreach (int item5 in list)
		{
			if (DomainManager.Character.GetRelatedCharIds(item5, 32768).Contains(taiwuCharId))
			{
				list2.Add((item5, taiwuCharId));
				num++;
				if (num >= seniorityCivilianSeverHatredLimit)
				{
					break;
				}
			}
		}
		if (num < seniorityCivilianSeverHatredLimit)
		{
			foreach (int item6 in list)
			{
				HashSet<int> relatedCharIds = DomainManager.Character.GetRelatedCharIds(item6, 32768);
				foreach (int item7 in list)
				{
					HashSet<int> relatedCharIds2 = DomainManager.Character.GetRelatedCharIds(item7, 32768);
					if (relatedCharIds.Contains(item7) && !list2.Contains((item6, item7)) && relatedCharIds2.Contains(item6) && !list2.Contains((item7, item6)))
					{
						list2.Add((item6, item7));
						list2.Add((item7, item6));
						num++;
						if (num >= seniorityCivilianSeverHatredLimit)
						{
							break;
						}
					}
				}
				if (num >= seniorityCivilianSeverHatredLimit)
				{
					break;
				}
			}
		}
		if (num < seniorityCivilianSeverHatredLimit)
		{
			foreach (int item8 in list)
			{
				HashSet<int> relatedCharIds3 = DomainManager.Character.GetRelatedCharIds(item8, 32768);
				foreach (int item9 in list)
				{
					if (relatedCharIds3.Contains(item9) && !list2.Contains((item8, item9)))
					{
						list2.Add((item8, item9));
						num++;
						if (num >= seniorityCivilianSeverHatredLimit)
						{
							break;
						}
					}
				}
				if (num >= seniorityCivilianSeverHatredLimit)
				{
					break;
				}
			}
		}
		foreach (var item10 in list2)
		{
			int item = item10.Item1;
			int item2 = item10.Item2;
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(item2);
			ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[67];
			DomainManager.Extra.ChangeProfessionSeniority(context, 10, formulaCfg.Calculate(element_Objects.GetInteractionGrade() + element_Objects2.GetInteractionGrade()));
			lifeRecordCollection.AddForgiveForCivilianSkill(item, currDate, taiwuCharId, location, item2);
			DomainManager.Character.ChangeRelationType(context, item, item2, 32768, 0);
			element_Objects.ChangeHappiness(context, 20);
			element_Objects.RecordFameAction(context, 4, item2, 1);
			if (DomainManager.Character.TryGetCharacterPrioritizedAction(item, out var action) && action.ActionType == 8)
			{
				DomainManager.Character.RemoveCharacterPrioritizedAction(context, item);
			}
		}
		if (num > 0)
		{
			taiwu.RecordFameAction(context, 12, taiwuCharId, (short)num);
			lifeRecordCollection.AddCivilianSkillDissolveResentment(taiwuCharId, currDate, location);
			DomainManager.World.GetInstantNotificationCollection().AddQuenchHatred(list2.Count);
			DomainManager.Map.SetBlockData(context, block);
		}
	}

	public static void CivilianSkill_MakeCharacterLeaveSect(DataContext context, GameData.Domains.Character.Character character)
	{
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		Tester.Assert(OrganizationDomain.IsSect(organizationInfo.OrgTemplateId), "$OrganizationDomain.IsSect({oriOrgInfo.OrgTemplateId})");
		Settlement settlement = DomainManager.Organization.GetSettlement(organizationInfo.SettlementId);
		short areaId = settlement.GetLocation().AreaId;
		sbyte stateIdByAreaId = DomainManager.Map.GetStateIdByAreaId(areaId);
		sbyte retireGrade = Config.Organization.Instance[organizationInfo.OrgTemplateId].RetireGrade;
		List<short> list = new List<short>();
		DomainManager.Map.GetStateSettlementIds(stateIdByAreaId, list, containsMainCity: true);
		list.Remove(DomainManager.Taiwu.GetTaiwuVillageSettlementId());
		if (retireGrade == 1)
		{
			list.RemoveAll((short settlementId) => DomainManager.Organization.GetSettlement(settlementId).GetOrgTemplateId() == 36);
		}
		short random = list.GetRandom(context.Random);
		Settlement settlement2 = DomainManager.Organization.GetSettlement(random);
		OrganizationInfo destOrgInfo = new OrganizationInfo(settlement2.GetOrgTemplateId(), retireGrade, principal: true, random);
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		lifeRecordCollection.AddPersuadeWithdrawlFromOrganization(taiwuCharId, currDate, character.GetId(), location);
		DomainManager.Organization.ChangeOrganization(context, character, destOrgInfo);
		int taiwuCharId2 = DomainManager.Taiwu.GetTaiwuCharId();
		DomainManager.World.GetInstantNotificationCollection().AddProfessionCivilianSkill2(character.GetId(), taiwuCharId2, destOrgInfo.OrgTemplateId, destOrgInfo.Grade, orgPrincipal: true, character.GetGender());
		if (location.IsValid())
		{
			MapBlockData block = DomainManager.Map.GetBlock(location);
			DomainManager.Map.SetBlockData(context, block);
		}
		int seniorityCivilianAddHatredLimit = ProfessionData.GetSeniorityCivilianAddHatredLimit(DomainManager.Extra.GetProfessionData(10).Seniority);
		int num = 0;
		OrgMemberCollection members = settlement.GetMembers();
		for (sbyte b = 8; b >= 0; b--)
		{
			foreach (int member in members.GetMembers(b))
			{
				DomainManager.Character.TryAddAndApplyOneWayRelation(context, member, character.GetId(), 32768);
				num++;
				if (num >= seniorityCivilianAddHatredLimit)
				{
					break;
				}
			}
			if (num >= seniorityCivilianAddHatredLimit)
			{
				break;
			}
		}
	}

	private static bool CheckSpecialCondition_CivilianSkill(ProfessionData professionData, int index)
	{
		if (index == 1)
		{
			return CheckSpecialCondition_CivilianSkill_1(professionData);
		}
		return true;
	}

	private static bool CheckSpecialCondition_CivilianSkill_1(ProfessionData professionData)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		Location location = taiwu.GetLocation();
		if (!location.IsValid())
		{
			return false;
		}
		MapBlockData block = DomainManager.Map.GetBlock(location);
		List<int> list = new List<int>();
		if (block.CharacterSet != null && block.CharacterSet.Count != 0)
		{
			foreach (int item in block.CharacterSet)
			{
				list.Add(item);
			}
		}
		foreach (int item2 in DomainManager.Taiwu.GetGroupCharIds().GetCollection())
		{
			if (item2 != taiwuCharId)
			{
				list.Add(item2);
			}
		}
		foreach (int item3 in list)
		{
			HashSet<int> relatedCharIds = DomainManager.Character.GetRelatedCharIds(item3, 32768);
			if (relatedCharIds.Contains(taiwuCharId))
			{
				return true;
			}
			foreach (int item4 in list)
			{
				if (relatedCharIds.Contains(item4))
				{
					return true;
				}
			}
		}
		return false;
	}

	private static void ExecuteOnClick_CraftSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
	{
		if (index == 2)
		{
			ExecuteOnClick_CraftSkill_2(context, professionData, ref arg);
			return;
		}
		throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
	}

	public static void ExecuteOnClick_CraftSkill_2(DataContext context, ProfessionData professionData, ref ProfessionSkillArg arg)
	{
		EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(arg.ItemKey);
		baseEquipment.ApplyDurabilityEquipmentEffectChange(context, baseEquipment.GetEquipmentEffectId(), arg.EffectId);
		baseEquipment.SetEquipmentEffectId(arg.EffectId, context);
		ItemDisplayData itemDisplayData = DomainManager.Item.GetItemDisplayData(baseEquipment.GetItemKey(), DomainManager.Taiwu.GetTaiwuCharId());
		List<ItemDisplayData> arg2 = new List<ItemDisplayData> { itemDisplayData };
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenGetItem_Item, arg2, arg2: false, arg3: false);
	}

	public static void OpenSetEquipmentEffect()
	{
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenSetEquipmentEffect);
	}

	public static int GetAddSeniority_CraftSkill_0(sbyte itemType, short templateId)
	{
		int baseValue = ItemTemplateHelper.GetBaseValue(itemType, templateId);
		return baseValue / 100;
	}

	public static int GetRefineBonus_CraftSkill_2(int refineBonus, int equippedCharId)
	{
		if ((!DomainManager.Character.TryGetElement_Objects(equippedCharId, out var _) || equippedCharId == DomainManager.Taiwu.GetTaiwuCharId()) && DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(62))
		{
			refineBonus = refineBonus * 150 / 100;
		}
		return refineBonus;
	}

	private static void ExecuteOnClick_DoctorSkill(DataContext context, ProfessionData professionData, int skillIndex, ref ProfessionSkillArg arg)
	{
		if (skillIndex == 1)
		{
			ExecuteOnClick_DoctorSkill_1(context, professionData);
			return;
		}
		throw new Exception(professionData.GetSkillConfig(skillIndex).Name + " is not an executable skill.");
	}

	private static void ExecuteOnClick_DoctorSkill_1(DataContext context, ProfessionData professionData)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short lifeSkillAttainment = taiwu.GetLifeSkillAttainment(8);
		short lifeSkillAttainment2 = taiwu.GetLifeSkillAttainment(9);
		short disorderOfQiDelta = (short)(-Math.Clamp((lifeSkillAttainment + lifeSkillAttainment2) * 5, DisorderLevelOfQi.MinValue, DisorderLevelOfQi.MaxValue));
		Location location = taiwu.GetLocation();
		MapBlockData belongSettlementBlock = DomainManager.Map.GetBelongSettlementBlock(location);
		int num = 0;
		int seniorityOrgGrade = professionData.GetSeniorityOrgGrade();
		List<short> list = new List<short>();
		DomainManager.Map.GetSettlementBlocks(belongSettlementBlock.AreaId, belongSettlementBlock.BlockId, list);
		foreach (short item in list)
		{
			MapBlockData block = DomainManager.Map.GetBlock(belongSettlementBlock.AreaId, item);
			if (block.CharacterSet == null)
			{
				continue;
			}
			foreach (int item2 in block.CharacterSet)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item2);
				if (element_Objects.GetOrganizationInfo().Grade <= seniorityOrgGrade && element_Objects.GetLegendaryBookOwnerState() < 2 && TryTreatCharacter(element_Objects))
				{
					num++;
				}
			}
		}
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item3 in collection)
		{
			if (item3 != taiwu.GetId())
			{
				GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(item3);
				if (TryTreatCharacter(element_Objects2))
				{
					num++;
				}
			}
		}
		taiwu.RecordFameAction(context, 24, -1, 1);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		lifeRecordCollection.AddFreeMedicalConsultation(taiwu.GetId(), currDate, location);
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		DomainManager.World.GetInstantNotificationCollection().AddProfessionDoctorSkill1(taiwuCharId, location, num);
		bool TryTreatCharacter(GameData.Domains.Character.Character character)
		{
			Injuries injuries = character.GetInjuries();
			PoisonInts poisoned = character.GetPoisoned();
			short disorderOfQi = character.GetDisorderOfQi();
			if (disorderOfQi <= 0 && !poisoned.IsNonZero() && injuries.GetSum() <= 0)
			{
				return false;
			}
			Injuries injuries2 = DomainManager.Combat.HealInjury(character.GetId(), taiwu);
			character.SetInjuries(injuries2, context);
			PoisonInts poisoned2 = DomainManager.Combat.HealPoison(character.GetId(), taiwu);
			character.SetPoisoned(ref poisoned2, context);
			character.ChangeDisorderOfQi(context, disorderOfQiDelta);
			int delta = (character.GetOrganizationInfo().Grade + 1) * (10 + 10 * professionData.Seniority / 3000000);
			DomainManager.Extra.ChangeAreaSpiritualDebt(context, location.AreaId, delta);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, character, taiwu, 6000);
			return true;
		}
	}

	private static bool CheckSpecialCondition_DoctorSkill(ProfessionData professionData, int index)
	{
		if (index == 1)
		{
			return CheckSpecialCondition_DoctorSkill_1(professionData);
		}
		return true;
	}

	private static bool CheckSpecialCondition_DoctorSkill_1(ProfessionData professionData)
	{
		sbyte seniorityDoctorMaxSettlementType = professionData.GetSeniorityDoctorMaxSettlementType();
		return CheckSettlementBlockTypeValid(seniorityDoctorMaxSettlementType);
	}

	private static void ExecuteOnClick_DukeSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
	{
		switch (index)
		{
		case 2:
			break;
		case 3:
			ExecuteOnClick_DukeSkill_3(context, professionData, ref arg);
			break;
		default:
			throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
		}
	}

	private static void ExecuteOnClick_DukeSkill_3(DataContext context, ProfessionData professionData, ref ProfessionSkillArg arg)
	{
		GetCricketBlocks(context, professionData);
	}

	private static void GetCricketBlocks(DataContext context, ProfessionData professionData)
	{
		TaiwuDomain taiwu = DomainManager.Taiwu;
		if (!DomainManager.Character.TryGetElement_Objects(taiwu.GetTaiwuCharId(), out var element))
		{
			return;
		}
		List<MapBlockData> list = new List<MapBlockData>();
		Location location = element.GetLocation();
		MapDomain map = DomainManager.Map;
		map.GetNeighborBlocks(location.AreaId, location.BlockId, list, 3);
		List<MapBlockData> list2 = new List<MapBlockData>();
		if (list.Count > 0)
		{
			CollectionUtils.Shuffle(context.Random, list);
			list.RemoveRange(0, list.Count / 2);
			foreach (MapBlockData item in list)
			{
				Location location2 = item.GetLocation();
				if (!map.LocationHasCricket(context, location2))
				{
					list2.Add(item);
				}
			}
			taiwu.SetCricketLuckPoint(taiwu.GetCricketLuckPoint() + 300, context);
		}
		ProfessionSkillArg arg = new ProfessionSkillArg
		{
			ProfessionId = 17,
			SkillId = 70,
			IsSuccess = true,
			EffectBlocks = list2.Select((MapBlockData block) => block.BlockId).ToList()
		};
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.ConfirmSkillExecuteAndPlayAnim, arg, arg2: false);
	}

	private static bool CheckSpecialCondition_DukeSkill(ProfessionData professionData, int skillIndex)
	{
		if (skillIndex == 3)
		{
			return CheckSpecialCondition_DukeSkill_3(professionData);
		}
		return true;
	}

	private static bool CheckSpecialCondition_DukeSkill_3(ProfessionData professionData)
	{
		TaiwuDomain taiwu = DomainManager.Taiwu;
		if (DomainManager.Character.TryGetElement_Objects(taiwu.GetTaiwuCharId(), out var element))
		{
			return element.GetLocation().IsValid();
		}
		return false;
	}

	public static bool DukeSkill_CheckCharacterHasTitle(int charId, ProfessionData professionData)
	{
		Tester.Assert(professionData.SkillsData is DukeSkillsData);
		DukeSkillsData dukeSkillsData = (DukeSkillsData)professionData.SkillsData;
		return dukeSkillsData.CharacterHasTitle(charId);
	}

	public static short DukeSkill_GetTitleFromOwner(int charId, ProfessionData professionData)
	{
		Tester.Assert(professionData.SkillsData is DukeSkillsData);
		DukeSkillsData dukeSkillsData = (DukeSkillsData)professionData.SkillsData;
		return dukeSkillsData.GetTitleFromOwner(charId);
	}

	public static int DukeSkill_GetOwnerOfTitle(short templateId, ProfessionData professionData)
	{
		Tester.Assert(professionData.SkillsData is DukeSkillsData);
		DukeSkillsData dukeSkillsData = (DukeSkillsData)professionData.SkillsData;
		return dukeSkillsData.GetOwnerOfTitle(templateId);
	}

	public static void DukeSkill_AddCharacterTitle(DataContext context, ProfessionData professionData, int charId, short templateId)
	{
		Tester.Assert(professionData.SkillsData is DukeSkillsData);
		DukeSkillsData dukeSkillsData = (DukeSkillsData)professionData.SkillsData;
		dukeSkillsData.OfflineAssignTitleToCharacter(context.Random, templateId, charId);
		DomainManager.Extra.SetProfessionData(context, professionData);
		DomainManager.Extra.AddCharacterProfessionExtraTitle(context, charId, templateId);
		DomainManager.Organization.TryRemoveBounty(context, charId);
	}

	public static bool DukeSkill_RemoveCharacterTitle(DataContext context, ProfessionData professionData, int charId)
	{
		Tester.Assert(professionData.SkillsData is DukeSkillsData);
		DukeSkillsData dukeSkillsData = (DukeSkillsData)professionData.SkillsData;
		short num = dukeSkillsData.OfflineRemoveTitleFromCharacter(charId);
		if (num == -1)
		{
			return false;
		}
		DomainManager.World.GetInstantNotifications().AddResignationPosition(charId);
		DomainManager.Extra.SetProfessionData(context, professionData);
		DomainManager.Extra.RemoveCharacterProfessionExtraTitle(context, charId, num);
		return true;
	}

	public static void DukeSkill_ClearAllTitle(DataContext context, ProfessionData professionData)
	{
		Tester.Assert(professionData.SkillsData is DukeSkillsData);
		DukeSkillsData dukeSkillsData = (DukeSkillsData)professionData.SkillsData;
		foreach (var allOwner in dukeSkillsData.GetAllOwners())
		{
			DomainManager.Extra.RemoveCharacterProfessionExtraTitle(context, allOwner.CharacterId, allOwner.TemplateId);
		}
		dukeSkillsData.OfflineClearAllTitles();
		DomainManager.Extra.SetProfessionData(context, professionData);
	}

	public static ItemKey DukeSkill_GetNewCricket(DataContext context, int charId)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(17);
		DukeSkillsData skillsData = professionData.GetSkillsData<DukeSkillsData>();
		ItemKey itemKey = OfflineCreateCricketAndModifyLuckPoint(context, skillsData, charId);
		DomainManager.Extra.SetProfessionData(context, professionData);
		DomainManager.Taiwu.AddItem(context, itemKey, 1, ItemSourceType.Inventory);
		GameData.Domains.Item.Cricket element_Crickets = DomainManager.Item.GetElement_Crickets(itemKey.Id);
		DomainManager.Item.AddCatchCricketProfessionSeniority(context, element_Crickets);
		return itemKey;
	}

	private static ItemKey OfflineCreateCricketAndModifyLuckPoint(DataContext context, DukeSkillsData data, int charId)
	{
		short titleFromOwner = data.GetTitleFromOwner(charId);
		if (titleFromOwner < 0)
		{
			return ItemKey.Invalid;
		}
		int index = 112;
		int simulateCount = ProfessionFormula.Instance[index].Calculate();
		int luckPoint = data.GetDukeLuckPointByTitle(titleFromOwner);
		ItemKey result = DomainManager.Item.CreateCricketByLuckPoint(context, ref luckPoint, simulateCount);
		data.OfflineSetDukeLuckPointByTitle(titleFromOwner, luckPoint);
		return result;
	}

	internal static bool DukeSkill_CheckCharacterHasTitle(int charId)
	{
		if (!DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(56))
		{
			return false;
		}
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(17);
		return DukeSkill_CheckCharacterHasTitle(charId, professionData);
	}

	internal static void DukeSkill_RemoveCharacterTitle(DataContext context, int charId)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(17);
		DukeSkill_RemoveCharacterTitle(context, professionData, charId);
	}

	internal static void DukeSkill_ClearAllTitle(DataContext context)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(17);
		DukeSkill_ClearAllTitle(context, professionData);
	}

	private static void ExecuteOnClick_HunterSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
	{
		switch (index)
		{
		case 0:
			ExecuteOnClick_HunterSkill_0(context, professionData);
			break;
		case 2:
			ExecuteOnClick_HunterSkill_2(context, professionData);
			break;
		default:
			throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
		}
	}

	public static CarrierItem GetCarrierByAnimal(short animalCharTemplateId)
	{
		sbyte index = GameData.Domains.Combat.SharedConstValue.CharId2AnimalId[animalCharTemplateId];
		AnimalItem animalItem = Config.Animal.Instance[index];
		return Config.Carrier.Instance[animalItem.CarrierId];
	}

	private static void ExecuteOnClick_HunterSkill_2(DataContext context, ProfessionData professionData)
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		Location animalGenerateCenterLocationByHunterSkill = DomainManager.Extra.GetAnimalGenerateCenterLocationByHunterSkill(context, location.AreaId);
		if (!animalGenerateCenterLocationByHunterSkill.IsValid())
		{
			DomainManager.World.GetInstantNotificationCollection().AddProfessionHunterSkill0None();
			return;
		}
		sbyte seniorityAnimalCount = professionData.GetSeniorityAnimalCount();
		int percentProb = GlobalConfig.Instance.HunterSkill2_OddFormulaFactorA + GlobalConfig.Instance.HunterSkill2_OddFormulaFactorB * professionData.GetSeniorityPercent() / 100;
		List<CharacterItem> list = new List<CharacterItem>(seniorityAnimalCount);
		for (int i = 0; i < seniorityAnimalCount; i++)
		{
			byte[] array = GlobalConfig.Instance.HunterSkill2_AnimalCountIndexToAnimalConsummateLevelList[i];
			byte animalConsummateLevel = array[0];
			for (int num = array.Length - 1; num >= 1; num--)
			{
				if (context.Random.CheckPercentProb(percentProb))
				{
					animalConsummateLevel = array[num];
				}
			}
			CharacterItem[] array2 = (from templateId in AnimalCharacterTemplateIds
				select Config.Character.Instance.GetItem(templateId) into template
				where template.ConsummateLevel <= animalConsummateLevel
				orderby template.ConsummateLevel descending
				select template).ToArray();
			CharacterItem characterItem = array2[0];
			if (array2.Length != 0)
			{
				for (int num2 = 1; num2 < array2.Length; num2++)
				{
					CharacterItem characterItem2 = array2[num2];
					if (characterItem2.ConsummateLevel == characterItem.ConsummateLevel && characterItem2.TemplateId >= 219 && characterItem2.TemplateId > characterItem.TemplateId && context.Random.CheckPercentProb(33))
					{
						characterItem = characterItem2;
						break;
					}
				}
			}
			list.Add(characterItem);
		}
		List<Location> locations = ObjectPool<List<Location>>.Instance.Get();
		DomainManager.Extra.GetAnimalGenerateLocationListByCenterLocation(context, animalGenerateCenterLocationByHunterSkill, seniorityAnimalCount, ref locations);
		for (int num3 = 0; num3 < list.Count; num3++)
		{
			if (num3 == 0)
			{
				DomainManager.Extra.AnimalGenerateInAreaByHunterSkill(context, animalGenerateCenterLocationByHunterSkill, list[num3].TemplateId);
			}
			else if (num3 < locations.Count)
			{
				DomainManager.Extra.AnimalGenerateInAreaByHunterSkill(context, locations[num3], list[num3].TemplateId);
			}
			else
			{
				ExtraDomain extra = DomainManager.Extra;
				List<Location> list2 = locations;
				extra.AnimalGenerateInAreaByHunterSkill(context, list2[list2.Count - 1], list[num3].TemplateId);
			}
			CharacterItem item = Config.Character.Instance.GetItem(list[num3].TemplateId);
			DomainManager.World.GetInstantNotificationCollection().AddProfessionHunterSkill0(item.OrganizationInfo.OrgTemplateId, item.OrganizationInfo.Grade, orgPrincipal: true, item.Gender);
		}
	}

	[Obsolete]
	private static void ExecuteOnClick_HunterSkill_0(DataContext context, ProfessionData professionData)
	{
	}

	public static GameData.Domains.Character.Character HunterSkill_ItemToAnimalCharacter(DataContext context, ItemKey animalItemKey, string displayName)
	{
		short characterIdInCombat = Config.Carrier.Instance[animalItemKey.TemplateId].CharacterIdInCombat;
		if (characterIdInCombat < 0)
		{
			throw new Exception($"Invalid carrier to become animal character {animalItemKey}.");
		}
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(1);
		HunterSkillsData skillsData = professionData.GetSkillsData<HunterSkillsData>();
		HunterSkillsData hunterSkillsData = skillsData;
		if (hunterSkillsData.AnimalItemKeyToGender == null)
		{
			hunterSkillsData.AnimalItemKeyToGender = new Dictionary<ItemKey, sbyte>();
		}
		hunterSkillsData = skillsData;
		if (hunterSkillsData.AnimalCharIdToItemKey == null)
		{
			hunterSkillsData.AnimalCharIdToItemKey = new Dictionary<int, ItemKey>();
		}
		hunterSkillsData = skillsData;
		if (hunterSkillsData.AnimalCharIdToAttraction == null)
		{
			hunterSkillsData.AnimalCharIdToAttraction = new Dictionary<ItemKey, short>();
		}
		sbyte b = Config.Character.Instance[characterIdInCombat].Gender;
		bool flag = false;
		if (b == -1)
		{
			if (skillsData.AnimalItemKeyToGender.TryGetValue(animalItemKey, out var value))
			{
				b = value;
			}
			else
			{
				b = (sbyte)context.Random.Next(2);
				flag = true;
			}
		}
		FixedEnemyCreationInfo fixedEnemyCreationInfo = new FixedEnemyCreationInfo();
		fixedEnemyCreationInfo.Gender = b;
		FixedEnemyCreationInfo creationInfo = fixedEnemyCreationInfo;
		GameData.Domains.Character.Character character = DomainManager.Character.CreateFixedEnemyWithCreationInfo(context, characterIdInCombat, ref creationInfo);
		FullName fullName = character.GetFullName();
		fullName.Type = 8;
		character.SetFullName(fullName, context);
		int id = character.GetId();
		if (!skillsData.AnimalCharIdToAttraction.TryGetValue(animalItemKey, out var _))
		{
			short value3 = (short)context.Random.Next(0, 901);
			skillsData.AnimalCharIdToAttraction.Add(animalItemKey, value3);
		}
		DomainManager.Character.CompleteCreatingCharacter(id);
		DomainManager.Extra.AssignCharacterCustomDisplayName(context, id, displayName);
		DomainManager.Taiwu.JoinGroup(context, id);
		skillsData.AnimalCharIdToItemKey.Add(id, animalItemKey);
		if (flag)
		{
			skillsData.AnimalItemKeyToGender.Add(animalItemKey, b);
		}
		DomainManager.Extra.SetProfessionData(context, professionData);
		DomainManager.Taiwu.RemoveItem(context, animalItemKey, 1, 1, deleteItem: false);
		DomainManager.Item.SetOwner(animalItemKey, ItemOwnerType.SpecialGroupMember, DomainManager.Taiwu.GetTaiwuCharId());
		EventHelper.ShowGetItemPageForCharacters(new List<int> { id }, isVillager: false);
		DomainManager.World.GetInstantNotificationCollection().AddBeastUpgrade(animalItemKey.ItemType, animalItemKey.TemplateId);
		return character;
	}

	public static ItemKey HunterSkill_AnimalCharacterToItem(DataContext context, GameData.Domains.Character.Character character)
	{
		int id = character.GetId();
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(1);
		HunterSkillsData skillsData = professionData.GetSkillsData<HunterSkillsData>();
		ItemKey itemKey = skillsData.AnimalCharIdToItemKey[id];
		Dictionary<ItemKey, int> items = character.GetInventory().Items;
		List<(ItemKey, int)> list = new List<(ItemKey, int)>();
		foreach (KeyValuePair<ItemKey, int> item in items)
		{
			list.Add((item.Key, item.Value));
		}
		int leaderId = character.GetLeaderId();
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(leaderId);
		Location location = element_Objects.GetLocation();
		MapBlockData blockData = DomainManager.Map.GetBlockData(location.AreaId, location.BlockId);
		foreach (var (itemKey2, amount) in list)
		{
			character.RemoveInventoryItem(context, itemKey2, amount, deleteItem: false);
			DomainManager.Map.AddBlockItem(context, blockData, itemKey2, amount);
		}
		DomainManager.Character.LeaveGroup(context, character);
		DomainManager.World.GetInstantNotificationCollection().AddBeastDowngrade(itemKey.ItemType, itemKey.TemplateId);
		DomainManager.Character.RemoveNonIntelligentCharacter(context, character);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		taiwu.AddInventoryItem(context, itemKey, 1);
		return itemKey;
	}

	private static bool CheckSpecialCondition_HunterSkill(ProfessionData professionData, int skillIndex)
	{
		HashSet<(Location, int)> result;
		return skillIndex switch
		{
			1 => DomainManager.Extra.TryGetAnimalAttackInRange(DomainManager.Taiwu.GetTaiwu().GetLocation(), 2, isTaiwuVictim: true, out result), 
			2 => DomainManager.Extra.CheckSpecialCondition_HunterSkill2(DataContextManager.GetCurrentThreadDataContext()), 
			3 => CheckGroupCountForConvertToAnimalCharacter() && CheckItem(), 
			_ => true, 
		};
		static bool CheckItem()
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			List<ItemDisplayData> allInventoryItems = DomainManager.Character.GetAllInventoryItems(taiwu.GetId());
			return allInventoryItems.Select((ItemDisplayData d) => d.Key).Any(IsItemCanConvertToAnimalCharacter);
		}
	}

	public static bool CheckGroupCountForConvertToAnimalCharacter()
	{
		return true;
	}

	public static bool IsItemCanConvertToAnimalCharacter(ItemKey itemKey)
	{
		short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
		return itemSubType == 402;
	}

	internal static int CalcLiteratiSkill2AuthorityCost(SecretInformationItem config, int holderCount)
	{
		return 5000 * config.SortValue * (100 - holderCount * 50 / config.MaxPersonAmount) / 100;
	}

	public static void LiteratiSkill_BroadcastModifiedSecretInformation(DataContext context, int secretInformationId)
	{
		InformationDomain information = DomainManager.Information;
		SecretInformationItem secretInformationConfig = information.GetSecretInformationConfig(secretInformationId);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		DomainManager.Information.TryGetElement_CharacterSecretInformation(taiwu.GetId(), out var _);
		taiwu.ChangeResource(context, 7, -CalcLiteratiSkill2AuthorityCost(secretInformationConfig, information.GetSecretInformationHolderCount(secretInformationId)));
		DomainManager.Information.MakeSecretInformationBroadcastEffect(context, secretInformationId, DomainManager.Taiwu.GetTaiwuCharId());
		DomainManager.World.GetInstantNotificationCollection().AddDisseminateSecretInformation(secretInformationConfig.TemplateId, secretInformationId);
	}

	public static void LiteratiSkill_AreaBroadcastNormalInformation(DataContext context, NormalInformation normalInformation)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		if (!location.IsValid())
		{
			return;
		}
		int num = 0;
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(location.AreaId);
		for (int i = 0; i < areaBlocks.Length; i++)
		{
			MapBlockData mapBlockData = areaBlocks[i];
			HashSet<int> characterSet = mapBlockData.CharacterSet;
			if (characterSet == null)
			{
				continue;
			}
			EventArgBox eventArgBox = new EventArgBox();
			InformationItem informationItem = Config.Information.Instance[normalInformation.TemplateId];
			foreach (int item in characterSet)
			{
				eventArgBox.Clear();
				if (DomainManager.Character.TryGetElement_Objects(item, out var element))
				{
					sbyte normalInformationUseResultType = EventHelper.GetNormalInformationUseResultType(item, normalInformation);
					int num2 = informationItem.EffectRate[normalInformationUseResultType];
					EventHelper.ChangeFavorabilityOptionalShareInformation(element, taiwu, EventHelper.ClampFavorabilityChangeValue(EventHelper.GetTaiwuFavorabilityHotChangeValue() * num2 / 100 * 150 / 100));
				}
				EventHelper.ApplyNormalInformationWithEffectRate(item, eventArgBox, normalInformation, string.Empty, string.Empty, string.Empty, string.Empty, 150);
				num++;
			}
		}
		DomainManager.World.GetInstantNotificationCollection().AddDisseminateInformation(location, num);
	}

	private static bool CheckLiteratiSkillNormalInformationUsable(NormalInformation normalInformation)
	{
		InformationItem item = Config.Information.Instance.GetItem(normalInformation.TemplateId);
		InformationInfoItem item2 = InformationInfo.Instance.GetItem(item.InfoIds[normalInformation.Level]);
		sbyte type = item.Type;
		bool flag = (uint)type <= 3u;
		bool flag2 = flag && item.IsGeneral;
		bool flag3 = flag2;
		if (flag3)
		{
			sbyte lifeSkillType = item2.LifeSkillType;
			bool flag4 = (uint)(lifeSkillType - 12) <= 1u;
			flag3 = !flag4;
		}
		return flag3;
	}

	private static bool CheckSpecialCondition_LiteratiSkill(ProfessionData professionData, int index)
	{
		return index switch
		{
			2 => CheckSpecialCondition_LiteratiSkill_2(professionData), 
			3 => CheckSpecialCondition_LiteratiSkill_3(professionData), 
			_ => true, 
		};
	}

	private static bool CheckSpecialCondition_LiteratiSkill_2(ProfessionData professionData)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		SecretInformationDisplayPackage secretInformationDisplayPackageFromCharacter = DomainManager.Information.GetSecretInformationDisplayPackageFromCharacter(taiwuCharId);
		List<SecretInformationDisplayData> secretInformationDisplayDataList = secretInformationDisplayPackageFromCharacter.SecretInformationDisplayDataList;
		return secretInformationDisplayDataList != null && secretInformationDisplayDataList.Count > 0;
	}

	private static bool CheckSpecialCondition_LiteratiSkill_3(ProfessionData professionData)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		IList<NormalInformation> source = DomainManager.Information.GetCharacterNormalInformation(taiwuCharId).GetList() ?? new List<NormalInformation>();
		return source.Any(CheckLiteratiSkillNormalInformationUsable);
	}

	public static void MartialArtistSkill_MakeAreaLearnCombatSkill(DataContext context, ProfessionData professionData, sbyte combatSkillType)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		if (!location.IsValid())
		{
			return;
		}
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(location.AreaId);
		List<short> selectableSkillIds = new List<short>();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = taiwu.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		lifeRecordCollection.AddCombatSkillModel(id, currDate, location, combatSkillType);
		int num = 0;
		Span<MapBlockData> span = areaBlocks;
		for (int i = 0; i < span.Length; i++)
		{
			MapBlockData mapBlockData = span[i];
			if (mapBlockData.CharacterSet == null)
			{
				continue;
			}
			foreach (int item in mapBlockData.CharacterSet)
			{
				if (TryLearnCombatSkill(item))
				{
					num++;
				}
			}
		}
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item2 in collection)
		{
			if (item2 != id && TryLearnCombatSkill(item2))
			{
				num++;
			}
		}
		int num2 = 1000 + num * 100;
		taiwu.ChangeResource(context, 7, num2);
		InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
		if (num2 > 0)
		{
			instantNotificationCollection.AddResourceIncreased(id, 7, num2);
		}
		unsafe bool TryLearnCombatSkill(int charId)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
			if (element_Objects.GetAgeGroup() != 2)
			{
				return false;
			}
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(charId);
			sbyte b = 8;
			selectableSkillIds.Clear();
			foreach (CombatSkillItem item3 in (IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance)
			{
				if (combatSkillType == item3.Type && item3.SectId != 0 && item3.Grade <= b && item3.BookId >= 0 && !charCombatSkills.ContainsKey(item3.TemplateId))
				{
					if (item3.Grade < b)
					{
						b = item3.Grade;
						selectableSkillIds.Clear();
					}
					selectableSkillIds.Add(item3.TemplateId);
				}
			}
			if (selectableSkillIds.Count == 0)
			{
				return false;
			}
			short random = selectableSkillIds.GetRandom(context.Random);
			short bookId = Config.CombatSkill.Instance[random].BookId;
			SkillBookItem skillBookItem = Config.SkillBook.Instance[bookId];
			CombatSkillShorts combatSkillAttainments = element_Objects.GetCombatSkillAttainments();
			CombatSkillShorts combatSkillQualifications = element_Objects.GetCombatSkillQualifications();
			Personalities personalities = element_Objects.GetPersonalities();
			int taughtNewSkillSuccessRate = GameData.Domains.Character.Character.GetTaughtNewSkillSuccessRate(skillBookItem.Grade, combatSkillQualifications.Items[skillBookItem.CombatSkillType], combatSkillAttainments.Items[skillBookItem.CombatSkillType], personalities.Items[1]);
			if (!context.Random.CheckPercentProb(taughtNewSkillSuccessRate))
			{
				return false;
			}
			ItemKey itemKey = DomainManager.Item.CreateSkillBook(context, bookId, -1, -1, -1, 50);
			element_Objects.AddInventoryItem(context, itemKey, 1);
			element_Objects.LearnNewCombatSkill(context, random, 0);
			int baseDelta = (b + 1) * 1000;
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, element_Objects, taiwu, baseDelta);
			lifeRecordCollection.AddLearnCombatSkill(charId, currDate, element_Objects.GetLocation(), random);
			return true;
		}
	}

	private static bool CheckSpecialCondition_MartialArtistSkill(ProfessionData professionData, int index)
	{
		return index switch
		{
			1 => CheckSpecialCondition_MartialArtistSkill_1(professionData), 
			2 => CheckSpecialCondition_MartialArtistSkill_2(professionData), 
			_ => true, 
		};
	}

	private static bool CheckSpecialCondition_MartialArtistSkill_1(ProfessionData professionData)
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (!location.IsValid())
		{
			return false;
		}
		MapBlockData block = DomainManager.Map.GetBlock(location);
		if (!block.IsCityTown())
		{
			return false;
		}
		MapBlockData belongSettlementBlock = DomainManager.Map.GetBelongSettlementBlock(location);
		Settlement settlementByLocation = DomainManager.Organization.GetSettlementByLocation(belongSettlementBlock.GetLocation());
		return settlementByLocation.GetSafety() < settlementByLocation.GetMaxSafety();
	}

	private static bool CheckSpecialCondition_MartialArtistSkill_2(ProfessionData professionData)
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (!location.IsValid())
		{
			return false;
		}
		MapBlockData block = DomainManager.Map.GetBlock(location);
		if (!block.IsCityTown())
		{
			return false;
		}
		MapBlockData belongSettlementBlock = DomainManager.Map.GetBelongSettlementBlock(location);
		Settlement settlementByLocation = DomainManager.Organization.GetSettlementByLocation(belongSettlementBlock.GetLocation());
		return settlementByLocation.GetSafety() >= 25;
	}

	private static void ExecuteOnClick_MartialArtist(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
	{
		if (index == 3)
		{
			ExecuteOnClick_MartialArtist_3(context, professionData, ref arg);
			return;
		}
		throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
	}

	private static void ExecuteOnClick_MartialArtist_3(DataContext context, ProfessionData professionData, ref ProfessionSkillArg professionSkillArg)
	{
		if (professionSkillArg.EffectBlocks == null)
		{
			DomainManager.Extra.MartialArtistSkill3Execute(context, updateData: true);
		}
	}

	private static void ExecuteOnClick_SavageSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
	{
		switch (index)
		{
		case 1:
			ExecuteOnClick_SavageSkill_1(context, professionData);
			break;
		case 3:
			ExecuteOnClick_SavageSkill_3(context, professionData, ref arg);
			break;
		default:
			throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
		}
	}

	internal static IEnumerable<Location> GetSavageSkill_1_EffectRange(Location location)
	{
		if (location.IsValid())
		{
			yield return location;
			MapDomain mapDomain = DomainManager.Map;
			byte areaSize = mapDomain.GetAreaSize(location.AreaId);
			ByteCoordinate origin = ByteCoordinate.IndexToCoordinate(location.BlockId, areaSize);
			if (origin.X > 0)
			{
				yield return new Location(location.AreaId, ByteCoordinate.CoordinateToIndex(new ByteCoordinate((byte)(origin.X - 1), origin.Y), areaSize));
			}
			if (origin.Y > 0)
			{
				yield return new Location(location.AreaId, ByteCoordinate.CoordinateToIndex(new ByteCoordinate(origin.X, (byte)(origin.Y - 1)), areaSize));
			}
			if (origin.X < areaSize - 1)
			{
				yield return new Location(location.AreaId, ByteCoordinate.CoordinateToIndex(new ByteCoordinate((byte)(origin.X + 1), origin.Y), areaSize));
			}
			if (origin.Y < areaSize - 1)
			{
				yield return new Location(location.AreaId, ByteCoordinate.CoordinateToIndex(new ByteCoordinate(origin.X, (byte)(origin.Y + 1)), areaSize));
			}
		}
	}

	private unsafe static void ExecuteOnClick_SavageSkill_1(DataContext context, ProfessionData professionData)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (!taiwu.GetLocation().IsValid())
		{
			return;
		}
		MapDomain map = DomainManager.Map;
		int num = 0;
		int seniorityResourceRecoveryFactor = professionData.GetSeniorityResourceRecoveryFactor();
		InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
		instantNotificationCollection.AddBlockResourceRecovery(seniorityResourceRecoveryFactor);
		foreach (Location item in GetSavageSkill_1_EffectRange(taiwu.GetLocation()))
		{
			MapBlockData block = map.GetBlock(item);
			if (126 == block.TemplateId)
			{
				continue;
			}
			if (DomainManager.Extra.TryGetHeavenlyTreeByLocation(block.GetLocation(), out var tree))
			{
				int heavenlyTreeRequiredGrowPointById = DomainManager.Extra.GetHeavenlyTreeRequiredGrowPointById(tree.Id);
				if (heavenlyTreeRequiredGrowPointById >= 0)
				{
					DomainManager.Extra.HeavenlyTreeGrewUp(context, tree.Id, heavenlyTreeRequiredGrowPointById, showUI: false);
					instantNotificationCollection.AddShenTreeGrow();
				}
			}
			for (sbyte b = 0; b < 6; b++)
			{
				short num2 = block.MaxResources.Items[b];
				short num3 = block.CurrResources.Items[b];
				short num4 = (short)Math.Min(num2, num3 + num2 * seniorityResourceRecoveryFactor / 100);
				num += num4 - block.CurrResources.Items[b];
				block.CurrResources.Items[b] = num4;
			}
			int sum = block.CurrResources.GetSum();
			int sum2 = block.MaxResources.GetSum();
			if (sum >= sum2 / 2)
			{
				block.Destroyed = false;
			}
			DomainManager.Map.SetBlockData(context, block);
		}
		ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[7];
		int baseDelta = formulaCfg.Calculate(num);
		DomainManager.Extra.ChangeProfessionSeniority(context, 0, baseDelta);
	}

	private static void ExecuteOnClick_SavageSkill_3(DataContext context, ProfessionData professionData, ref ProfessionSkillArg arg)
	{
		ItemKey selectedItem = arg.ItemKey;
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		MapBlockData block = DomainManager.Map.GetBlock(location);
		using (IEnumerator<KeyValuePair<ItemKeyAndDate, int>> enumerator = block.Items.Where((KeyValuePair<ItemKeyAndDate, int> item) => item.Key.ItemKey == selectedItem).GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				ItemKeyAndDate key = enumerator.Current.Key;
				block.RemoveItemByCount(key, 1);
				DomainManager.Item.RemoveOwner(key.ItemKey, ItemOwnerType.MapBlock, location.GetHashCode());
			}
		}
		DomainManager.Map.SetBlockData(context, block);
		taiwu.AddInventoryItem(context, selectedItem, 1);
		List<ItemDisplayData> arg2 = new List<ItemDisplayData> { DomainManager.Item.GetItemDisplayData(selectedItem, DomainManager.Taiwu.GetTaiwu().GetId()) };
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenGetItem_Item, arg2, arg2: false, arg3: true);
	}

	private static void OpenItemSelectFromBlock()
	{
		DomainManager.World.AdvanceDaysInMonth(DomainManager.TaiwuEvent.MainThreadDataContext, GlobalConfig.Instance.SavageSkill3_OpenItemSelectTimeCost);
		List<ItemDisplayData> list = new List<ItemDisplayData>();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		if (!location.IsValid())
		{
			return;
		}
		MapBlockData block = DomainManager.Map.GetBlock(location);
		Dictionary<ItemKey, int> dictionary = new Dictionary<ItemKey, int>();
		foreach (KeyValuePair<ItemKeyAndDate, int> item in block.Items)
		{
			if (dictionary.ContainsKey(item.Key.ItemKey))
			{
				dictionary[item.Key.ItemKey] += item.Value;
			}
			else
			{
				dictionary.Add(item.Key.ItemKey, item.Value);
			}
		}
		list = DomainManager.Item.GetItemDisplayDataListOptional(dictionary.Select((KeyValuePair<ItemKey, int> v) => v.Key).ToList(), taiwu.GetId(), -1);
		foreach (ItemDisplayData item2 in list)
		{
			item2.Amount = dictionary[item2.Key];
		}
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenProfessionSkillSpecial, 60, list);
	}

	private static bool CheckSpecialCondition_SavageSkill(ProfessionData professionData, int skillIndex)
	{
		return skillIndex switch
		{
			1 => DomainManager.Extra.CheckSpecialCondition_SavageSkill_1(professionData), 
			3 => CheckSpecialCondition_SavageSkill_3(professionData), 
			_ => true, 
		};
	}

	private static bool CheckSpecialCondition_SavageSkill_3(ProfessionData professionData)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		if (!location.IsValid())
		{
			return false;
		}
		MapBlockData block = DomainManager.Map.GetBlock(location);
		SortedList<ItemKeyAndDate, int> items = block.Items;
		return items != null && items.Count > 0;
	}

	private static void ExecuteOnClick_TaoistMonkSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
	{
		if (index == 3)
		{
			ExecuteOnClick_TaoistMonkSkill_3(context, professionData);
			return;
		}
		throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
	}

	private static void ExecuteOnClick_TaoistMonkSkill_3(DataContext context, ProfessionData professionData)
	{
		TaoistMonkSkillsData skillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
		skillsData.IsTriggeringTribulation = true;
	}

	public static bool TaoistMonkSkill_CheckTribulationSucceed()
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Dictionary<ItemKey, int> items = taiwu.GetInventory().Items;
		int num = 0;
		foreach (var (itemKey2, num3) in items)
		{
			if (itemKey2.ItemType == 12 && itemKey2.TemplateId == 234)
			{
				num += num3;
			}
		}
		return num >= 99;
	}

	public static void TaoistMonkSkill_ConfirmTribulationSucceed(DataContext context, ProfessionData professionData)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		List<short> featureIds = taiwu.GetFeatureIds();
		TaoistMonkSkillsData skillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
		Dictionary<ItemKey, int> items = taiwu.GetInventory().Items;
		foreach (var (itemKey2, num2) in items)
		{
			if (itemKey2.ItemType == 12 && itemKey2.TemplateId == 234)
			{
				taiwu.RemoveInventoryItem(context, itemKey2, 99, num2 == 99);
				break;
			}
		}
		if (featureIds.Contains(629))
		{
			taiwu.RemoveFeature(context, 629);
			taiwu.RemoveFeature(context, 628);
			taiwu.RemoveFeature(context, 627);
			skillsData.SurvivedTribulationCount = 4;
			skillsData.LastAgeIncreaseDate = DomainManager.World.GetCurrDate() - 12 - DomainManager.World.GetCurrMonthInYear() + taiwu.GetBirthMonth();
			DomainManager.Extra.SetProfessionData(context, professionData);
			if (taiwu.GetCurrAge() > 16)
			{
				short leftMaxHealth = taiwu.GetLeftMaxHealth();
				taiwu.SetCurrAge(16, context);
				short leftMaxHealth2 = taiwu.GetLeftMaxHealth();
				taiwu.ChangeHealth(context, (leftMaxHealth2 - leftMaxHealth) * 12);
				AvatarData avatar = taiwu.GetAvatar();
				if (avatar.UpdateGrowableElementsShowingAbilities(taiwu))
				{
					taiwu.SetAvatar(avatar, context);
				}
			}
			taiwu.AddFeature(context, 195, removeMutexFeature: true);
		}
		else
		{
			if (taiwu.AddFeature(context, 627))
			{
				skillsData.SurvivedTribulationCount = 1;
			}
			else if (taiwu.AddFeature(context, 628))
			{
				skillsData.SurvivedTribulationCount = 2;
			}
			else if (taiwu.AddFeature(context, 629))
			{
				skillsData.SurvivedTribulationCount = 3;
			}
			DomainManager.Extra.SetProfessionData(context, professionData);
		}
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		lifeRecordCollection.AddTribulationSucceeded(taiwu.GetId(), currDate, taiwu.GetLocation());
	}

	[Obsolete]
	public static bool TaoistMonkSkill_CanTriggerTribulation()
	{
		if (!DomainManager.Extra.IsProfessionalSkillUnlocked(5, 3))
		{
			return false;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (taiwu.GetLeftMaxHealth() > 0)
		{
			return false;
		}
		return DomainManager.Extra.IsOneShotEventHandled(39);
	}

	public static bool TaoistMonkSkill_HasSurvivedAllTribulation()
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(5);
		TaoistMonkSkillsData skillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
		return skillsData.HasSurvivedAllTribulation();
	}

	public static bool TaoistMonkSkill_ShouldIncreaseAge()
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(5);
		TaoistMonkSkillsData skillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
		return skillsData.ShouldIncreaseAge();
	}

	public static void TaoistMonkSkill_UpdateAgeIncreaseDate(DataContext context)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(5);
		TaoistMonkSkillsData skillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
		skillsData.LastAgeIncreaseDate = DomainManager.World.GetCurrDate();
		DomainManager.Extra.SetProfessionData(context, professionData);
	}

	private static void UnpackCrossArchiveProfession_TaoistMonk(DataContext context)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(5);
		TaoistMonkSkillsData skillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
		if (skillsData.SurvivedTribulationCount <= 0)
		{
			return;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (skillsData.HasSurvivedAllTribulation())
		{
			if (taiwu.GetCurrAge() > 16)
			{
				taiwu.SetCurrAge(16, context);
			}
			return;
		}
		for (int i = 0; i < skillsData.SurvivedTribulationCount; i++)
		{
			short featureId = (short)(627 + i);
			taiwu.AddFeature(context, featureId);
		}
	}

	private static void TaoistMonkSkill_ResetSurvivedTribulationCount(DataContext context)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(5);
		TaoistMonkSkillsData skillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
		skillsData.SurvivedTribulationCount = 0;
		DomainManager.Extra.SetProfessionData(context, professionData);
	}

	private static void TaoistMonkSkill_OnPostAdvanceMonth(DataContext context)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(5);
		TaoistMonkSkillsData skillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (skillsData.SurvivedTribulationCount > 0 && !skillsData.HasSurvivedAllTribulation())
		{
			taiwu.CreateInventoryItem(context, 12, 234, skillsData.SurvivedTribulationCount * 3);
			DomainManager.World.GetInstantNotificationCollection().AddGetItem(taiwu.GetId(), 12, 234);
		}
		if (skillsData.IsTriggeringTribulation)
		{
			skillsData.IsTriggeringTribulation = false;
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddTaiwuTribulation(taiwu.GetId(), taiwu.GetLocation());
		}
		if (skillsData.ShouldIncreaseAge())
		{
			TaoistMonkSkill_UpdateAgeIncreaseDate(context);
		}
	}

	private static bool CheckSpecialCondition_TaoistMonkSkill(ProfessionData professionData, int index)
	{
		if (index == 3)
		{
			return CheckSpecialCondition_TaoistMonkSkill_3(professionData);
		}
		return true;
	}

	private static bool CheckSpecialCondition_TaoistMonkSkill_3(ProfessionData professionData)
	{
		return true;
	}

	private static void TeaTasterSkill_UpdateVillagerLearnSkillInterval(DataContext context)
	{
		if (DomainManager.Taiwu.GetVillagerLearnLifeSkillsFromSect())
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(16);
			int currDate = DomainManager.World.GetCurrDate();
			if (professionData.SkillsData is TeaTasterSkillsData teaTasterSkillsData && teaTasterSkillsData.VillagersLastLearnSkillDate + 3 < currDate)
			{
				teaTasterSkillsData.VillagersLastLearnSkillDate = currDate;
				DomainManager.Extra.SetProfessionData(context, professionData);
			}
		}
	}

	public static void TeaTasterSkill_SetActionPointGained(DataContext context, int value)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(16);
		if (professionData.SkillsData is TeaTasterSkillsData teaTasterSkillsData)
		{
			teaTasterSkillsData.ActionPointGained = value;
			DomainManager.Extra.SetProfessionData(context, professionData);
		}
	}

	public static int TeaTasterSkill_GetActionPointGained()
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(16);
		int result = 0;
		if (professionData.SkillsData is TeaTasterSkillsData teaTasterSkillsData)
		{
			result = teaTasterSkillsData.ActionPointGained;
		}
		return result;
	}

	private static void ExecuteOnClick_TeaTasterSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
	{
		if (index == 3)
		{
			TasterUltimateResult arg2 = DomainManager.Extra.CastTasterUltimateSkill(context, arg.CharIds, arg.BookIds, isCombatSkill: false);
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenTasterUltimateResult, arg1: false, arg2);
			return;
		}
		throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
	}

	private static bool CheckSpecialCondition_TeaTasterSkill(ProfessionData professionData, int index)
	{
		if (index == 3)
		{
			return DomainManager.Extra.CheckTasterUltimateSpecialCondition(isCombatSkill: false) == 0;
		}
		return true;
	}

	private static void ExecuteOnClick_TravelerSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
	{
		switch (index)
		{
		case 2:
			ExecuteOnClick_TravelerSkill_2(context, professionData, arg.ProfessionTravelerTargetLocation);
			break;
		case 3:
			ExecuteOnClick_TravelerSkill_3(context, professionData);
			break;
		default:
			throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
		}
	}

	private static void ExecuteOnClick_TravelerSkill_2(DataContext context, ProfessionData professionData, Location targetLocation)
	{
		DomainManager.Map.TeleportByTraveler(context, targetLocation.BlockId);
	}

	private static void ExecuteOnClick_TravelerSkill_3(DataContext context, ProfessionData professionData)
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		DomainManager.Map.BuildTravelerPalace(context, location);
	}

	private static void ExecuteOnClick_TravelingBuddhistMonkSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
	{
		if (index == 3)
		{
			TravelingBuddhistMonkSkill3_SetFeature(context, arg.EffectId);
			return;
		}
		throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
	}

	public static void TravelingBuddhistMonkSkill_SetTempleVisited(DataContext context, Location location)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(12);
		TravelingBuddhistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingBuddhistMonkSkillsData>();
		sbyte stateIdByAreaId = DomainManager.Map.GetStateIdByAreaId(location.AreaId);
		skillsData.OfflineSetStateTempleVisited(stateIdByAreaId);
		DomainManager.Extra.SetProfessionData(context, professionData);
		DomainManager.World.GetInstantNotificationCollection().AddVisitTemple(location, 5 - skillsData.GetVisitedTempleCount());
		int baseDelta = ProfessionFormulaImpl.Calculate(80);
		DomainManager.Extra.ChangeProfessionSeniority(context, 12, baseDelta);
	}

	public static bool TravelingBuddhistMonkSkill_CanVisitTemple(Location location)
	{
		if (!IsSkillUnlocked(40))
		{
			return false;
		}
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(12);
		TravelingBuddhistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingBuddhistMonkSkillsData>();
		sbyte stateIdByAreaId = DomainManager.Map.GetStateIdByAreaId(location.AreaId);
		if (skillsData.IsStateTempleVisited(stateIdByAreaId))
		{
			return false;
		}
		Location stateTempleLocation = skillsData.GetStateTempleLocation(stateIdByAreaId);
		return stateTempleLocation.IsValid() && stateTempleLocation.Equals(location);
	}

	public static void TravelingBuddhistMonkSkill_CreateTemples(DataContext context)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(12);
		TravelingBuddhistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingBuddhistMonkSkillsData>();
		skillsData.OfflineClearAllTampleState();
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		Span<short> span = stackalloc short[3];
		Span<short> span2 = stackalloc short[3];
		List<sbyte> list = new List<sbyte>(15);
		for (sbyte b = 0; b < 15; b++)
		{
			list.Add(b);
		}
		CollectionUtils.Shuffle(context.Random, list);
		for (int i = 0; i < 5; i++)
		{
			sbyte b2 = list[i];
			if (skillsData.StateHasTemple(b2))
			{
				continue;
			}
			int num = 0;
			for (int j = 0; j < 3; j++)
			{
				short num2 = (short)(b2 * 3 + j);
				MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(num2);
				if (!string.IsNullOrEmpty(element_Areas.GetConfig().TempleName))
				{
					span[num] = num2;
					num++;
				}
			}
			int index = context.Random.Next(num);
			short num3 = span[index];
			MapAreaData element_Areas2 = DomainManager.Map.GetElement_Areas(num3);
			int num4 = 0;
			SettlementInfo[] settlementInfos = element_Areas2.SettlementInfos;
			for (int k = 0; k < settlementInfos.Length; k++)
			{
				SettlementInfo settlementInfo = settlementInfos[k];
				if (settlementInfo.SettlementId >= 0 && settlementInfo.SettlementId != taiwuVillageSettlementId)
				{
					span2[num4] = settlementInfo.BlockId;
					num4++;
				}
			}
			index = context.Random.Next(num4);
			short blockId = span2[index];
			Location location = new Location(num3, blockId);
			skillsData.OfflineCreateTemple(b2, location);
			AdaptableLog.TagInfo("Profession", "Creating temple " + element_Areas2.GetConfig().TempleName + " at " + location.ToString());
		}
		DomainManager.Extra.SetProfessionData(context, professionData);
	}

	public static List<string> TravelingBuddhistMonkSkill_GetTempleNames()
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(12);
		TravelingBuddhistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingBuddhistMonkSkillsData>();
		List<string> list = new List<string>();
		for (sbyte b = 0; b < 15; b++)
		{
			if (skillsData.StateHasTemple(b))
			{
				Location stateTempleLocation = skillsData.GetStateTempleLocation(b);
				MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(stateTempleLocation.AreaId);
				list.Add(element_Areas.GetConfig().TempleName);
			}
		}
		return list;
	}

	public static void TravelingBuddhistMonkSkill3_SetFeature(DataContext context, short featureId)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		taiwu.AddFeature(context, featureId, removeMutexFeature: true);
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(12);
		TravelingBuddhistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingBuddhistMonkSkillsData>();
		skillsData.OfflineSetSelectedSkill3FeatureId(featureId);
		DomainManager.Extra.SetProfessionData(context, professionData);
	}

	private static bool CheckSpecialCondition_TravelingBuddhistMonkSkill(ProfessionData professionData, int index)
	{
		if (index == 3)
		{
			return CheckSpecialCondition_TravelingBuddhistMonkSkill_3(professionData);
		}
		return true;
	}

	private static bool CheckSpecialCondition_TravelingBuddhistMonkSkill_3(ProfessionData professionData)
	{
		return true;
	}

	private static bool CheckSpecialCondition_TravelingTaoistMonkSkill(ProfessionData professionData, int index)
	{
		if (index == 3)
		{
			return CheckSpecialCondition_TravelingTaoistMonkSkill_3(professionData);
		}
		return true;
	}

	private static bool CheckSpecialCondition_TravelingTaoistMonkSkill_3(ProfessionData professionData)
	{
		return true;
	}

	private static void ExecuteOnClick_TravelingTaoistMonkSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
	{
		if (index == 2)
		{
			TravelingTaoistMonkSkill2(context, professionData, arg);
			return;
		}
		throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
	}

	private static void TravelingTaoistMonkSkill2(DataContext context, ProfessionData professionData, ProfessionSkillArg arg)
	{
		int objectId = arg.CharIds.FirstOrDefault();
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(objectId);
		GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(arg.CharId);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		List<int> bookIds = arg.BookIds;
		List<short> effectBlocks = arg.EffectBlocks;
		int badFeatureCount = EventHelper.GetBadFeatureCount(element_Objects);
		foreach (int item in bookIds)
		{
			element_Objects2.RemoveFeature(context, (short)item);
		}
		foreach (short item2 in effectBlocks)
		{
			element_Objects.RemoveFeature(context, item2);
		}
		foreach (short item3 in effectBlocks)
		{
			element_Objects2.AddFeature(context, item3);
		}
		foreach (int item4 in bookIds)
		{
			element_Objects.AddFeature(context, (short)item4);
		}
		int badFeatureCount2 = EventHelper.GetBadFeatureCount(element_Objects);
		int num = badFeatureCount2 - badFeatureCount;
		EventHelper.ChangeFavorabilityOptionalRepeatedEvent(element_Objects, taiwu, (short)(-num * ProfessionRelatedConstants.TravelingTaoistMonkSkill2FavorValue));
		int num2 = 0;
		foreach (short item5 in effectBlocks)
		{
			num2 += Math.Abs(CharacterFeature.Instance[item5].Level);
		}
		int seniority = professionData.Seniority;
		int num3 = 3000000;
		int num4 = num2 * (9 - 6 * seniority / num3);
		short baseMaxHealth = taiwu.GetBaseMaxHealth();
		baseMaxHealth = (short)Math.Max(baseMaxHealth - num4, 0);
		int arg2 = taiwu.GetBaseMaxHealth() - baseMaxHealth;
		taiwu.SetBaseMaxHealth(baseMaxHealth, context);
		ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[93];
		int baseDelta = formulaCfg.Calculate(arg2);
		DomainManager.Extra.ChangeProfessionSeniority(context, 14, baseDelta);
		DomainManager.TaiwuEvent.SetListenerEventActionBoolArg("TravelingTaoistMonkSkill2Executed", "ConchShip_PresetKey_FinishSkillExecute", value: true);
		DomainManager.TaiwuEvent.TriggerListener("TravelingTaoistMonkSkill2Executed", value: true);
	}

	public static int TravelingTaoistMonkSkill1_ExpCost(GameData.Domains.Character.Character targetChar)
	{
		sbyte grade = targetChar.GetOrganizationInfo().Grade;
		return (grade + 1) * (grade + 1) * 200;
	}

	private static void TravelingTaoistMonkSkill_ClearHealthBonus(DataContext context)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(14);
		TravelingTaoistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingTaoistMonkSkillsData>();
		skillsData.BonusMaxHealth = 0;
		DomainManager.Extra.SetProfessionData(context, professionData);
	}

	public static short TravelingTaoistMonkSkill_GetMaxHealthBonus()
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(14);
		if (professionData == null)
		{
			return 0;
		}
		TravelingTaoistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingTaoistMonkSkillsData>();
		return skillsData.BonusMaxHealth;
	}

	private static void TravelingTaoistMonkSkill_OnPreAdvanceMonth(DataContext context)
	{
		if (!IsSkillUnlocked(48))
		{
			return;
		}
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(14);
		if (CheckSpecialCondition(professionData, 3))
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			if (taiwu.GetBirthMonth() == DomainManager.World.GetCurrMonthInYear() && !taiwu.IsAgeIncreaseStopped())
			{
				short currAge = taiwu.GetCurrAge();
				currAge += 2;
				taiwu.SetCurrAge(currAge, context);
				TravelingTaoistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingTaoistMonkSkillsData>();
				skillsData.BonusMaxHealth += 36;
				DomainManager.Extra.SetProfessionData(context, professionData);
				short baseMaxHealth = taiwu.GetBaseMaxHealth();
				taiwu.SetBaseMaxHealth(baseMaxHealth, context);
			}
		}
	}

	private static void WineTasterSkill_UpdateVillagerLearnSkillInterval(DataContext context)
	{
		if (DomainManager.Taiwu.GetVillagerLearnCombatSkillsFromSect())
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(7);
			int currDate = DomainManager.World.GetCurrDate();
			if (professionData.SkillsData is WineTasterSkillsData wineTasterSkillsData && wineTasterSkillsData.VillagersLastLearnSkillDate + 3 < currDate)
			{
				wineTasterSkillsData.VillagersLastLearnSkillDate = currDate;
				DomainManager.Extra.SetProfessionData(context, professionData);
			}
		}
	}

	private static void ExecuteOnClick_WineTasterSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
	{
		if (index == 3)
		{
			TasterUltimateResult arg2 = DomainManager.Extra.CastTasterUltimateSkill(context, arg.CharIds, arg.BookIds, isCombatSkill: true);
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenTasterUltimateResult, arg1: true, arg2);
			return;
		}
		throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
	}

	private static bool CheckSpecialCondition_WineTasterSkill(ProfessionData professionData, int index)
	{
		if (index == 3)
		{
			return DomainManager.Extra.CheckTasterUltimateSpecialCondition(isCombatSkill: true) == 0;
		}
		return true;
	}
}
