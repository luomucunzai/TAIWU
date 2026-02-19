using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Creation;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions;

[SerializableGameData(NotForDisplayModule = true)]
public class BaihuaStoryAdventureFourTriggerAction : MonthlyActionBase, IDynamicAction, ISerializableGameData
{
	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	private int _leaderId;

	public const string CharacterNameStr = "MajorCharacter_0_0";

	public short DynamicActionType => 4;

	public BaihuaStoryAdventureFourTriggerAction()
	{
		_leaderId = -1;
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
			Settlement villageSettlement = GetVillageSettlement();
			Location = villageSettlement.GetLocation();
			if (DomainManager.Adventure.TryCreateAdventureSite(mainThreadDataContext, Location.AreaId, Location.BlockId, 185, Key))
			{
				DomainManager.Adventure.ActivateAdventureSite(mainThreadDataContext, Location.AreaId, Location.BlockId);
				CallVillageLeader(mainThreadDataContext);
			}
		}
	}

	public override void CollectCalledCharacters(HashSet<int> calledCharacters)
	{
		if (_leaderId >= 0)
		{
			calledCharacters.Add(_leaderId);
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
		_leaderId = -1;
		if (!isComplete)
		{
			int value = LastFinishDate + 1;
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(mainThreadDataContext, 3, "ConchShip_PresetKey_BaihuaAdventureFourAppearDate", value);
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
			CallVillageLeader(mainThreadDataContext);
		}
	}

	private Settlement GetVillageSettlement()
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
		short arg = -1;
		sectMainStoryEventArgBox.Get("ConchShip_PresetKey_BaihuaVillageSettlementIdSelection", ref arg);
		return DomainManager.Organization.GetSettlement(arg);
	}

	public override void FillEventArgBox(EventArgBox eventArgBox)
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		if (_leaderId < 0 || !DomainManager.Character.TryGetElement_Objects(_leaderId, out var element))
		{
			Settlement villageSettlement = GetVillageSettlement();
			OrganizationItem organizationItem = null;
			organizationItem = ((villageSettlement.GetOrgTemplateId() == 37) ? Config.Organization.Instance[(sbyte)37] : ((villageSettlement.GetOrgTemplateId() != 36) ? Config.Organization.Instance[(sbyte)38] : Config.Organization.Instance[(sbyte)36]));
			sbyte b = organizationItem.GenderRestriction;
			if (b < 0)
			{
				b = Gender.GetRandom(mainThreadDataContext.Random);
			}
			sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(villageSettlement.GetLocation().AreaId);
			short characterTemplateId = OrganizationDomain.GetCharacterTemplateId(organizationItem.TemplateId, stateTemplateIdByAreaId, b);
			sbyte grade = (sbyte)((mainThreadDataContext.Random.Next(0, 2) == 0) ? 6 : 7);
			TemporaryIntelligentCharacterCreationInfo tmpInfo = new TemporaryIntelligentCharacterCreationInfo
			{
				Location = Location,
				CharTemplateId = characterTemplateId,
				OrgInfo = new OrganizationInfo(organizationItem.TemplateId, grade, principal: true, villageSettlement.GetId())
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
			eventArgBox.Set("MajorCharacter_0_0", _leaderId);
		}
		eventArgBox.Set("MajorCharacter_0_Count", 1);
	}

	private void CallVillageLeader(DataContext context)
	{
		Tester.Assert(_leaderId < 0);
		Settlement villageSettlement = GetVillageSettlement();
		GameData.Domains.Character.Character availableHighMember = villageSettlement.GetAvailableHighMember(8, 6);
		if (availableHighMember != null)
		{
			_leaderId = availableHighMember.GetId();
			DomainManager.Character.LeaveGroup(context, availableHighMember);
			DomainManager.Character.GroupMove(context, availableHighMember, Location);
			Events.RaiseCharacterLocationChanged(context, _leaderId, Location, Location.Invalid);
			availableHighMember.ActiveExternalRelationState(context, 4);
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
		OrganizationInfo organizationInfo = element.GetOrganizationInfo();
		if (organizationInfo.OrgTemplateId != 36 || organizationInfo.OrgTemplateId != 37 || organizationInfo.OrgTemplateId != 38)
		{
			return true;
		}
		if (organizationInfo.Grade < 6)
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

	public override bool IsSerializedSizeFixed()
	{
		return true;
	}

	public override int GetSerializedSize()
	{
		int num = 20;
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
