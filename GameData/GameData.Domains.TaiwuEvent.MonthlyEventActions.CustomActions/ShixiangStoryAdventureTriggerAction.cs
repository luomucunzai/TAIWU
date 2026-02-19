using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Creation;
using GameData.Domains.Character.Filters;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions;

[SerializableGameData(NotForDisplayModule = true)]
public class ShixiangStoryAdventureTriggerAction : MonthlyActionBase, IDynamicAction, ISerializableGameData
{
	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	private int _sectLeaderId;

	[SerializableGameDataField]
	private int _literatiId;

	public short DynamicActionType => 2;

	public ShixiangStoryAdventureTriggerAction()
	{
		_sectLeaderId = -1;
		_literatiId = -1;
		Location = Location.Invalid;
	}

	public override void MonthlyHandler()
	{
	}

	public override void TriggerAction()
	{
		if (State == 0)
		{
			DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
			if (DomainManager.Adventure.TryCreateAdventureSite(mainThreadDataContext, Location.AreaId, Location.BlockId, 176, Key))
			{
				DomainManager.Adventure.ActivateAdventureSite(mainThreadDataContext, Location.AreaId, Location.BlockId);
				CallShixiangLeader(mainThreadDataContext);
				CallLiterati(mainThreadDataContext);
				DomainManager.World.GetMonthlyNotificationCollection().AddSectMainStoryShixiangAdventure();
				DomainManager.Extra.TriggerExtraTask(mainThreadDataContext, 34, 170);
			}
		}
	}

	public override void CollectCalledCharacters(HashSet<int> calledCharacters)
	{
		if (_sectLeaderId >= 0)
		{
			calledCharacters.Add(_sectLeaderId);
		}
		if (_literatiId >= 0)
		{
			calledCharacters.Add(_literatiId);
		}
	}

	public override void Deactivate(bool isComplete)
	{
		State = 0;
		Month = 0;
		LastFinishDate = DomainManager.World.GetCurrDate();
		Location = Location.Invalid;
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		ReleaseCharacter(mainThreadDataContext, _sectLeaderId);
		ReleaseCharacter(mainThreadDataContext, _literatiId);
		_sectLeaderId = -1;
		_literatiId = -1;
		if (!isComplete)
		{
			int value = LastFinishDate + 3;
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(mainThreadDataContext, 6, "ConchShip_PresetKey_ShixiangAdventureAppearDate", value);
		}
	}

	public override MonthlyActionBase CreateCopy()
	{
		return GameData.Serializer.Serializer.CreateCopy(this);
	}

	public override void EnsurePrerequisites()
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		if (IsLeaderOutdated())
		{
			ReleaseCharacter(mainThreadDataContext, _sectLeaderId);
			_sectLeaderId = -1;
			CallShixiangLeader(mainThreadDataContext);
		}
		if (IsLiteratiOutdated())
		{
			ReleaseCharacter(mainThreadDataContext, _literatiId);
			_literatiId = -1;
			CallLiterati(mainThreadDataContext);
		}
	}

	public override void FillEventArgBox(EventArgBox eventArgBox)
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		if (_sectLeaderId < 0 || !DomainManager.Character.TryGetElement_Objects(_sectLeaderId, out var element))
		{
			Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(6);
			OrganizationItem organizationItem = Config.Organization.Instance[(sbyte)6];
			sbyte b = organizationItem.GenderRestriction;
			if (b < 0)
			{
				b = Gender.GetRandom(mainThreadDataContext.Random);
			}
			sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(settlementByOrgTemplateId.GetLocation().AreaId);
			short characterTemplateId = OrganizationDomain.GetCharacterTemplateId(organizationItem.TemplateId, stateTemplateIdByAreaId, b);
			TemporaryIntelligentCharacterCreationInfo tmpInfo = new TemporaryIntelligentCharacterCreationInfo
			{
				Location = Location,
				CharTemplateId = characterTemplateId,
				OrgInfo = new OrganizationInfo(organizationItem.TemplateId, 8, principal: true, settlementByOrgTemplateId.GetId())
			};
			GameData.Domains.Character.Character character = DomainManager.Character.CreateTemporaryIntelligentCharacter(mainThreadDataContext, ref tmpInfo);
			OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(tmpInfo.OrgInfo);
			var (value, value2) = CharacterDomain.GetRealName(character);
			AdaptableLog.TagInfo("MartialArtTournamentMonthlyAction", $"Creating temporary character {organizationItem.Name}-{orgMemberConfig.GradeName} {value}{value2}({character.GetId()})");
			int id = character.GetId();
			DomainManager.Adventure.AddTemporaryIntelligentCharacter(id);
			AdaptableLog.Info($"Adding 1 temporary major characters {character} to adventure.");
			eventArgBox.Set("MajorCharacter_0_0", id);
		}
		else
		{
			AdaptableLog.Info($"Adding 1 major characters {element} to adventure.");
			eventArgBox.Set("MajorCharacter_0_0", _sectLeaderId);
		}
		if (_literatiId < 0 || !DomainManager.Character.TryGetElement_Objects(_literatiId, out var element2))
		{
			Settlement settlementByOrgTemplateId2 = DomainManager.Organization.GetSettlementByOrgTemplateId(26);
			OrganizationItem organizationItem2 = Config.Organization.Instance[(sbyte)26];
			sbyte gender = 1;
			sbyte stateTemplateIdByAreaId2 = DomainManager.Map.GetStateTemplateIdByAreaId(settlementByOrgTemplateId2.GetLocation().AreaId);
			short characterTemplateId2 = OrganizationDomain.GetCharacterTemplateId(organizationItem2.TemplateId, stateTemplateIdByAreaId2, gender);
			TemporaryIntelligentCharacterCreationInfo tmpInfo2 = new TemporaryIntelligentCharacterCreationInfo
			{
				Location = Location,
				CharTemplateId = characterTemplateId2,
				OrgInfo = new OrganizationInfo(organizationItem2.TemplateId, 5, principal: true, settlementByOrgTemplateId2.GetId())
			};
			GameData.Domains.Character.Character character2 = DomainManager.Character.CreateTemporaryIntelligentCharacter(mainThreadDataContext, ref tmpInfo2);
			OrganizationMemberItem orgMemberConfig2 = OrganizationDomain.GetOrgMemberConfig(tmpInfo2.OrgInfo);
			var (value3, value4) = CharacterDomain.GetRealName(character2);
			AdaptableLog.TagInfo("MartialArtTournamentMonthlyAction", $"Creating temporary character {organizationItem2.Name}-{orgMemberConfig2.GradeName} {value3}{value4}({character2.GetId()})");
			int id2 = character2.GetId();
			DomainManager.Adventure.AddTemporaryIntelligentCharacter(id2);
			AdaptableLog.Info($"Adding 1 temporary major characters {character2} to adventure.");
			eventArgBox.Set("MajorCharacter_0_1", id2);
		}
		else
		{
			AdaptableLog.Info($"Adding 1 major characters {element2} to adventure.");
			eventArgBox.Set("MajorCharacter_0_1", _literatiId);
		}
		eventArgBox.Set("MajorCharacter_0_Count", 2);
	}

	private void CallShixiangLeader(DataContext context)
	{
		Tester.Assert(_sectLeaderId < 0);
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(6);
		GameData.Domains.Character.Character leader = settlementByOrgTemplateId.GetLeader();
		if (leader != null && !leader.IsActiveExternalRelationState(60) && leader.GetKidnapperId() < 0 && leader.GetAgeGroup() != 0)
		{
			_sectLeaderId = leader.GetId();
			DomainManager.Character.LeaveGroup(context, leader);
			DomainManager.Character.GroupMove(context, leader, Location);
			Events.RaiseCharacterLocationChanged(context, _sectLeaderId, Location, Location.Invalid);
			leader.ActiveExternalRelationState(context, 4);
		}
	}

	private void CallLiterati(DataContext context)
	{
		List<GameData.Domains.Character.Character> list = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		list2.Clear();
		sbyte stateIdByAreaId = DomainManager.Map.GetStateIdByAreaId(Location.AreaId);
		DomainManager.Map.GetAllAreaInState(stateIdByAreaId, list2);
		MapCharacterFilter.ParallelFind(IsLiteratiValid, list, list2);
		if (list.Count > 0)
		{
			GameData.Domains.Character.Character random = list.GetRandom(context.Random);
			_literatiId = random.GetId();
			DomainManager.Character.LeaveGroup(context, random);
			DomainManager.Character.GroupMove(context, random, Location);
			Events.RaiseCharacterLocationChanged(context, _sectLeaderId, Location, Location.Invalid);
			random.ActiveExternalRelationState(context, 4);
		}
		ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(list2);
	}

	private static bool IsLiteratiValid(GameData.Domains.Character.Character character)
	{
		if (character.GetAgeGroup() != 2)
		{
			return false;
		}
		if (character.GetGender() != 1)
		{
			return false;
		}
		if (character.IsActiveExternalRelationState(60))
		{
			return false;
		}
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		if (!Config.Organization.Instance[organizationInfo.OrgTemplateId].IsCivilian)
		{
			return false;
		}
		if (organizationInfo.Grade != 5)
		{
			return false;
		}
		if (character.GetKidnapperId() >= 0)
		{
			return false;
		}
		return true;
	}

	private bool IsLeaderOutdated()
	{
		if (_sectLeaderId < 0)
		{
			return true;
		}
		if (!DomainManager.Character.TryGetElement_Objects(_sectLeaderId, out var element))
		{
			return true;
		}
		OrganizationInfo organizationInfo = element.GetOrganizationInfo();
		if (organizationInfo.OrgTemplateId != 6)
		{
			return true;
		}
		if (organizationInfo.Grade != 8)
		{
			return true;
		}
		if (!organizationInfo.Principal)
		{
			return true;
		}
		if (element.GetKidnapperId() >= 0)
		{
			return true;
		}
		if (element.GetAgeGroup() == 0)
		{
			return true;
		}
		if (element.GetLocation() != Location)
		{
			return true;
		}
		return false;
	}

	private bool IsLiteratiOutdated()
	{
		if (_literatiId < 0)
		{
			return true;
		}
		if (!DomainManager.Character.TryGetElement_Objects(_literatiId, out var element))
		{
			return true;
		}
		if (element.GetAgeGroup() != 2)
		{
			return true;
		}
		if (element.GetGender() != 1)
		{
			return true;
		}
		OrganizationInfo organizationInfo = element.GetOrganizationInfo();
		if (!Config.Organization.Instance[organizationInfo.OrgTemplateId].IsCivilian)
		{
			return true;
		}
		if (organizationInfo.Grade != 5)
		{
			return true;
		}
		if (element.GetLocation() != Location)
		{
			return true;
		}
		if (element.GetKidnapperId() >= 0)
		{
			return true;
		}
		return false;
	}

	private static void ReleaseCharacter(DataContext context, int charId)
	{
		if (DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			element.DeactivateExternalRelationState(context, 4);
			if (element.IsCompletelyInfected())
			{
				Events.RaiseInfectedCharacterLocationChanged(context, charId, Location.Invalid, element.GetLocation());
			}
			else
			{
				Events.RaiseCharacterLocationChanged(context, charId, Location.Invalid, element.GetLocation());
			}
		}
	}

	public override bool IsSerializedSizeFixed()
	{
		return true;
	}

	public override int GetSerializedSize()
	{
		int num = 26;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = DynamicActionType;
		ptr += 2;
		ptr += Location.Serialize(ptr);
		*(int*)ptr = _sectLeaderId;
		ptr += 4;
		*(int*)ptr = _literatiId;
		ptr += 4;
		ptr += Key.Serialize(ptr);
		*ptr = (byte)State;
		ptr++;
		*(int*)ptr = Month;
		ptr += 4;
		*(int*)ptr = LastFinishDate;
		ptr += 4;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += 2;
		ptr += Location.Deserialize(ptr);
		_sectLeaderId = *(int*)ptr;
		ptr += 4;
		_literatiId = *(int*)ptr;
		ptr += 4;
		ptr += Key.Deserialize(ptr);
		State = (sbyte)(*ptr);
		ptr++;
		Month = *(int*)ptr;
		ptr += 4;
		LastFinishDate = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
