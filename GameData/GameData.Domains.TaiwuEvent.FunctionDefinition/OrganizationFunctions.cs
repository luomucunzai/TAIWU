using System;
using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition;

public class OrganizationFunctions
{
	[EventFunction(58)]
	private static void SetSectAllowLearning(EventScriptRuntime runtime, Settlement settlement)
	{
		if (settlement is Sect sect)
		{
			sect.SetTaiwuExploreStatus(2, runtime.Context);
		}
	}

	[EventFunction(59)]
	private static void JoinOrganization(EventScriptRuntime runtime, GameData.Domains.Character.Character character, Settlement settlement, sbyte grade)
	{
		OrganizationInfo destOrgInfo = new OrganizationInfo
		{
			OrgTemplateId = settlement.GetOrgTemplateId(),
			Grade = grade,
			SettlementId = settlement.GetId(),
			Principal = true
		};
		DomainManager.Organization.ChangeOrganization(runtime.Context, character, destOrgInfo);
	}

	[EventFunction(60)]
	private static void SetSectCharApprovedTaiwu(EventScriptRuntime runtime, GameData.Domains.Character.Character character, bool approved)
	{
		int id = character.GetId();
		DomainManager.Character.TryCreateRelation(runtime.Context, id, DomainManager.Taiwu.GetTaiwuCharId());
		SectCharacter element_SectCharacters = DomainManager.Organization.GetElement_SectCharacters(id);
		element_SectCharacters.SetApprovedTaiwu(runtime.Context, approved);
	}

	[EventFunction(55)]
	private static void ChangeSafetyForSettlement(EventScriptRuntime runtime, Settlement settlement, int delta)
	{
		settlement.ChangeSafety(runtime.Context, delta);
	}

	[EventFunction(56)]
	private static void ChangeCultureForSettlement(EventScriptRuntime runtime, Settlement settlement, int delta)
	{
		settlement.ChangeCulture(runtime.Context, delta);
	}

	[EventFunction(227)]
	private static void RegisterSettlementMemberFeature(EventScriptRuntime runtime, Settlement settlement, short featureId, sbyte minGrade = 0, sbyte maxGrade = 8)
	{
		OrgMemberCollection members = settlement.GetMembers();
		for (sbyte b = 0; b <= 8; b++)
		{
			HashSet<int> members2 = members.GetMembers(b);
			if (b >= minGrade && b <= maxGrade)
			{
				foreach (int item in members2)
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
					element_Objects.AddFeature(runtime.Context, featureId);
				}
			}
		}
		DomainManager.Extra.RegisterSettlementMemberFeature(runtime.Context, settlement.GetId(), new SettlementMemberFeature
		{
			FeatureId = featureId,
			MinGrade = minGrade,
			MaxGrade = maxGrade
		});
	}

	[EventFunction(226)]
	private static void SetSectSpiritualDebtInteractionOccurred(EventScriptRuntime runtime, sbyte orgTemplateId)
	{
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(orgTemplateId);
		if (!(settlementByOrgTemplateId is Sect sect))
		{
			throw new InvalidCastException($"Unable to cast {settlementByOrgTemplateId} to Sect.");
		}
		sect.SetSpiritualDebtInteractionOccurred(spiritualDebtInteractionOccurred: true, runtime.Context);
	}

	[EventFunction(243)]
	private static short GetMapBlockSettlement(EventScriptRuntime runtime, MapBlockData mapBlock)
	{
		Location location = mapBlock.GetRootBlock().GetLocation();
		Settlement settlementByLocation = DomainManager.Organization.GetSettlementByLocation(location);
		return settlementByLocation.GetId();
	}

	[EventFunction(192)]
	private static short GetRandomSettlementInState(EventScriptRuntime runtime, sbyte stateTemplateId, EOrganizationSettlementType settlementType)
	{
		sbyte stateIdByStateTemplateId = DomainManager.Map.GetStateIdByStateTemplateId(stateTemplateId);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		DomainManager.Map.GetStateSettlementIds(stateIdByStateTemplateId, list, containsMainCity: true, containsSect: true);
		if (settlementType != EOrganizationSettlementType.Invalid)
		{
			for (int num = list.Count - 1; num >= 0; num--)
			{
				short settlementId = list[num];
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
				if (settlement.OrganizationConfig.SettlementType != settlementType)
				{
					CollectionUtils.SwapAndRemove(list, num);
				}
			}
		}
		if (list.Count == 0)
		{
			return -1;
		}
		short random = list.GetRandom(runtime.Context.Random);
		ObjectPool<List<short>>.Instance.Return(list);
		return random;
	}

	[EventFunction(217)]
	private static ShortList GetSettlementListInState(EventScriptRuntime runtime, sbyte stateTemplateId, EOrganizationSettlementType settlementType)
	{
		sbyte stateIdByStateTemplateId = DomainManager.Map.GetStateIdByStateTemplateId(stateTemplateId);
		ShortList result = ShortList.Create();
		List<short> items = result.Items;
		DomainManager.Map.GetStateSettlementIds(stateIdByStateTemplateId, items, containsMainCity: true, containsSect: true);
		if (settlementType != EOrganizationSettlementType.Invalid)
		{
			for (int num = items.Count - 1; num >= 0; num--)
			{
				short settlementId = items[num];
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
				if (settlement.OrganizationConfig.SettlementType != settlementType)
				{
					CollectionUtils.SwapAndRemove(items, num);
				}
			}
		}
		return result;
	}

	[EventFunction(263)]
	private static short GetCharacterSettlement(EventScriptRuntime runtime, GameData.Domains.Character.Character character)
	{
		return character.GetOrganizationInfo().SettlementId;
	}
}
