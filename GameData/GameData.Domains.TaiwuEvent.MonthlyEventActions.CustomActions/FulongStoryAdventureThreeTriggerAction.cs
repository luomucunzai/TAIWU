using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Creation;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions;

[SerializableGameData(NotForDisplayModule = true)]
public class FulongStoryAdventureThreeTriggerAction : MonthlyActionBase, IDynamicAction, ISerializableGameData
{
	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	private int _leaderId;

	[SerializableGameDataField]
	private int _participant1;

	[SerializableGameDataField]
	private int _participant2;

	[SerializableGameDataField]
	private int _participant3;

	[SerializableGameDataField]
	private int _participant4;

	[SerializableGameDataField]
	private int _participant5;

	[SerializableGameDataField]
	private int _participant6;

	[SerializableGameDataField]
	private int _participant7;

	public const string LeaderNameStr = "MajorCharacter_0_0";

	public const string Participant1NameStr = "MajorCharacter_0_1";

	public const string Participant2NameStr = "MajorCharacter_0_2";

	public const string Participant3NameStr = "MajorCharacter_0_3";

	public const string Participant4NameStr = "MajorCharacter_0_4";

	public const string Participant5NameStr = "MajorCharacter_0_5";

	public const string Participant6NameStr = "MajorCharacter_0_6";

	public const string Participant7NameStr = "MajorCharacter_0_7";

	public short DynamicActionType => 6;

	public FulongStoryAdventureThreeTriggerAction()
	{
		InitCharacterId();
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
			Settlement settlement = GetSettlement();
			Location = settlement.GetLocation();
			if (DomainManager.Adventure.TryCreateAdventureSite(mainThreadDataContext, Location.AreaId, Location.BlockId, 190, Key))
			{
				DomainManager.Adventure.ActivateAdventureSite(mainThreadDataContext, Location.AreaId, Location.BlockId);
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(mainThreadDataContext, 14, "ConchShip_PresetKey_FulongAdventureThreeCountDown", 3);
				DomainManager.Extra.TriggerExtraTask(mainThreadDataContext, 52, 340);
				CallLeader(mainThreadDataContext);
				CallParticipant(mainThreadDataContext, ref _participant1);
				CallParticipant(mainThreadDataContext, ref _participant2);
				CallParticipant(mainThreadDataContext, ref _participant3);
				CallParticipant(mainThreadDataContext, ref _participant4);
				CallParticipant(mainThreadDataContext, ref _participant5);
				CallParticipant(mainThreadDataContext, ref _participant6);
				CallParticipant(mainThreadDataContext, ref _participant7);
			}
		}
	}

	public override void CollectCalledCharacters(HashSet<int> calledCharacters)
	{
		if (_leaderId >= 0)
		{
			calledCharacters.Add(_leaderId);
		}
		if (_participant1 >= 0)
		{
			calledCharacters.Add(_participant1);
		}
		if (_participant2 >= 0)
		{
			calledCharacters.Add(_participant2);
		}
		if (_participant3 >= 0)
		{
			calledCharacters.Add(_participant3);
		}
		if (_participant4 >= 0)
		{
			calledCharacters.Add(_participant4);
		}
		if (_participant5 >= 0)
		{
			calledCharacters.Add(_participant5);
		}
		if (_participant6 >= 0)
		{
			calledCharacters.Add(_participant6);
		}
		if (_participant7 >= 0)
		{
			calledCharacters.Add(_participant7);
		}
	}

	public override void Deactivate(bool isComplete)
	{
		State = 0;
		Month = 0;
		LastFinishDate = DomainManager.World.GetCurrDate();
		Location = Location.Invalid;
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		ReleaseCharacter(mainThreadDataContext, _leaderId);
		ReleaseCharacter(mainThreadDataContext, _participant1);
		ReleaseCharacter(mainThreadDataContext, _participant2);
		ReleaseCharacter(mainThreadDataContext, _participant3);
		ReleaseCharacter(mainThreadDataContext, _participant4);
		ReleaseCharacter(mainThreadDataContext, _participant5);
		ReleaseCharacter(mainThreadDataContext, _participant6);
		ReleaseCharacter(mainThreadDataContext, _participant7);
		InitCharacterId();
		if (!isComplete)
		{
			GameData.Domains.TaiwuEvent.EventHelper.EventHelper.FulongEndEffect(goodEnd: false, adventureComplete: false);
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
			ReleaseCharacter(mainThreadDataContext, _leaderId);
			_leaderId = -1;
			CallLeader(mainThreadDataContext);
		}
		HandleParticipantOutdated(mainThreadDataContext, ref _participant1);
		HandleParticipantOutdated(mainThreadDataContext, ref _participant2);
		HandleParticipantOutdated(mainThreadDataContext, ref _participant3);
		HandleParticipantOutdated(mainThreadDataContext, ref _participant4);
		HandleParticipantOutdated(mainThreadDataContext, ref _participant5);
		HandleParticipantOutdated(mainThreadDataContext, ref _participant6);
		HandleParticipantOutdated(mainThreadDataContext, ref _participant7);
	}

	private Settlement GetSettlement()
	{
		return DomainManager.Organization.GetSettlementByOrgTemplateId(14);
	}

	public override void FillEventArgBox(EventArgBox eventArgBox)
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		if (_leaderId > 0 && DomainManager.Character.TryGetElement_Objects(_leaderId, out var element))
		{
			AdaptableLog.Info($"Adding 1 major characters {element} to adventure.");
			eventArgBox.Set("MajorCharacter_0_0", _leaderId);
		}
		FillParticipantEventArgBox(mainThreadDataContext, eventArgBox, _participant1, "MajorCharacter_0_1", 0, 2);
		FillParticipantEventArgBox(mainThreadDataContext, eventArgBox, _participant2, "MajorCharacter_0_2", 3, 5);
		FillParticipantEventArgBox(mainThreadDataContext, eventArgBox, _participant3, "MajorCharacter_0_3", 3, 5);
		FillParticipantEventArgBox(mainThreadDataContext, eventArgBox, _participant4, "MajorCharacter_0_4", 0, 2);
		FillParticipantEventArgBox(mainThreadDataContext, eventArgBox, _participant5, "MajorCharacter_0_5", 3, 5);
		FillParticipantEventArgBox(mainThreadDataContext, eventArgBox, _participant6, "MajorCharacter_0_6", 3, 5);
		FillParticipantEventArgBox(mainThreadDataContext, eventArgBox, _participant7, "MajorCharacter_0_7", 3, 5);
		eventArgBox.Set("MajorCharacter_0_Count", 8);
	}

	private GameData.Domains.Character.Character CreateTemporaryIntelligentParticipant(DataContext context, EventArgBox eventArgBox, string participant1NameStr, sbyte grade)
	{
		Settlement settlement = GetSettlement();
		OrganizationItem organizationItem = Config.Organization.Instance[(sbyte)14];
		sbyte b = organizationItem.GenderRestriction;
		if (b < 0)
		{
			b = Gender.GetRandom(context.Random);
		}
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(settlement.GetLocation().AreaId);
		short characterTemplateId = OrganizationDomain.GetCharacterTemplateId(organizationItem.TemplateId, stateTemplateIdByAreaId, b);
		TemporaryIntelligentCharacterCreationInfo tmpInfo = new TemporaryIntelligentCharacterCreationInfo
		{
			Location = Location,
			CharTemplateId = characterTemplateId,
			OrgInfo = new OrganizationInfo(organizationItem.TemplateId, grade, principal: true, settlement.GetId())
		};
		GameData.Domains.Character.Character character = DomainManager.Character.CreateTemporaryIntelligentCharacter(context, ref tmpInfo);
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(tmpInfo.OrgInfo);
		var (value, value2) = CharacterDomain.GetRealName(character);
		AdaptableLog.TagInfo("MartialArtTournamentMonthlyAction", $"Creating temporary character {organizationItem.Name}-{orgMemberConfig.GradeName} {value}{value2}({character.GetId()})");
		int id = character.GetId();
		DomainManager.Adventure.AddTemporaryIntelligentCharacter(id);
		AdaptableLog.Info($"Adding 1 temporary major characters {character} to adventure.");
		eventArgBox.Set(participant1NameStr, id);
		return character;
	}

	private void CallLeader(DataContext context)
	{
		Tester.Assert(_leaderId < 0);
		Settlement settlement = GetSettlement();
		GameData.Domains.Character.Character leader = settlement.GetLeader();
		if (leader != null && leader.GetAgeGroup() == 2)
		{
			_leaderId = leader.GetId();
			DomainManager.Character.LeaveGroup(context, leader);
			DomainManager.Character.GroupMove(context, leader, Location);
			Events.RaiseCharacterLocationChanged(context, _leaderId, Location, Location.Invalid);
			leader.ActiveExternalRelationState(context, 4);
		}
	}

	private bool IsLeaderOutdated()
	{
		if (_leaderId < 0)
		{
			return true;
		}
		if (!DomainManager.Character.TryGetElement_Objects(_leaderId, out var element))
		{
			return true;
		}
		if (element.GetOrganizationInfo().Grade != 8)
		{
			return true;
		}
		if (element.GetKidnapperId() >= 0)
		{
			return true;
		}
		if (element.GetAgeGroup() < 2)
		{
			return true;
		}
		if (element.GetLocation() != Location)
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

	private void CallParticipant(DataContext context, ref int participantId)
	{
		Tester.Assert(participantId < 0);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Settlement settlement = GetSettlement();
		List<int> list = new List<int>();
		settlement.GetMembers().GetAllMembers(list);
		short num = 1;
		for (int i = 0; i < list.Count; i++)
		{
			int num2 = list[i];
			if (!DomainManager.Character.TryGetElement_Objects(num2, out var element))
			{
				continue;
			}
			OrganizationInfo organizationInfo = element.GetOrganizationInfo();
			if ((organizationInfo.Grade == 8 && organizationInfo.Principal) || element.IsActiveExternalRelationState(4))
			{
				continue;
			}
			sbyte favorabilityType = DomainManager.Character.GetFavorabilityType(num2, taiwu.GetId());
			if (favorabilityType >= 2)
			{
				if (DomainManager.Character.HasRelation(taiwu.GetId(), num2, 1) || DomainManager.Character.HasRelation(taiwu.GetId(), num2, 8) || DomainManager.Character.HasRelation(taiwu.GetId(), num2, 2) || DomainManager.Character.HasRelation(taiwu.GetId(), num2, 16) || DomainManager.Character.HasRelation(taiwu.GetId(), num2, 1024) || DomainManager.Character.HasRelation(taiwu.GetId(), num2, 4) || DomainManager.Character.HasRelation(taiwu.GetId(), num2, 32))
				{
					participantId = num2;
					break;
				}
				if (DomainManager.Character.HasRelation(taiwu.GetId(), num2, 512) || (DomainManager.Character.HasRelation(taiwu.GetId(), num2, 16384) && DomainManager.Character.HasRelation(num2, taiwu.GetId(), 16384)))
				{
					participantId = num2;
					break;
				}
				if (DomainManager.Character.HasRelation(taiwu.GetId(), num2, 8192) || DomainManager.Character.HasRelation(taiwu.GetId(), num2, 2048) || DomainManager.Character.HasRelation(taiwu.GetId(), num2, 4096))
				{
					participantId = num2;
					break;
				}
				if (favorabilityType > num)
				{
					num = favorabilityType;
					participantId = num2;
				}
			}
		}
		if (participantId >= 0)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(participantId);
			DomainManager.Character.LeaveGroup(context, element_Objects);
			DomainManager.Character.GroupMove(context, element_Objects, Location);
			Events.RaiseCharacterLocationChanged(context, _leaderId, Location, Location.Invalid);
			element_Objects.ActiveExternalRelationState(context, 4);
		}
	}

	private bool IsParticipantOutdated(int participantId)
	{
		if (participantId < 0)
		{
			return true;
		}
		if (!DomainManager.Character.TryGetElement_Objects(participantId, out var element))
		{
			return true;
		}
		if (element.GetKidnapperId() >= 0)
		{
			return true;
		}
		if (element.GetLocation() != Location)
		{
			return true;
		}
		return false;
	}

	private void InitCharacterId()
	{
		_leaderId = -1;
		_participant1 = -1;
		_participant2 = -1;
		_participant3 = -1;
		_participant4 = -1;
		_participant5 = -1;
		_participant6 = -1;
		_participant7 = -1;
	}

	private void HandleParticipantOutdated(DataContext context, ref int characterId)
	{
		if (IsParticipantOutdated(characterId))
		{
			ReleaseCharacter(context, characterId);
			characterId = -1;
			CallParticipant(context, ref characterId);
		}
	}

	private void FillParticipantEventArgBox(DataContext context, EventArgBox eventArgBox, int characterId, string nameStr, sbyte gradeLow, sbyte gradeHigh)
	{
		if (characterId < 0 || !DomainManager.Character.TryGetElement_Objects(characterId, out var element))
		{
			CreateTemporaryIntelligentParticipant(context, eventArgBox, nameStr, (sbyte)context.Random.Next((int)gradeLow, gradeHigh + 1));
			return;
		}
		AdaptableLog.Info($"Adding 1 major characters {element} to adventure.");
		eventArgBox.Set(nameStr, characterId);
	}

	public override bool IsSerializedSizeFixed()
	{
		return true;
	}

	public override int GetSerializedSize()
	{
		int num = 50;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = DynamicActionType;
		ptr += 2;
		ptr += Location.Serialize(ptr);
		*(int*)ptr = _leaderId;
		ptr += 4;
		*(int*)ptr = _participant1;
		ptr += 4;
		*(int*)ptr = _participant2;
		ptr += 4;
		*(int*)ptr = _participant3;
		ptr += 4;
		*(int*)ptr = _participant4;
		ptr += 4;
		*(int*)ptr = _participant5;
		ptr += 4;
		*(int*)ptr = _participant6;
		ptr += 4;
		*(int*)ptr = _participant7;
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
		_leaderId = *(int*)ptr;
		ptr += 4;
		_participant1 = *(int*)ptr;
		ptr += 4;
		_participant2 = *(int*)ptr;
		ptr += 4;
		_participant3 = *(int*)ptr;
		ptr += 4;
		_participant4 = *(int*)ptr;
		ptr += 4;
		_participant5 = *(int*)ptr;
		ptr += 4;
		_participant6 = *(int*)ptr;
		ptr += 4;
		_participant7 = *(int*)ptr;
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
