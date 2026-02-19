using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Relation;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.EventManager;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions;

[SerializableGameData(NotForDisplayModule = true)]
public class MarriageTriggerAction : MonthlyActionBase, IDynamicAction, ISerializableGameData
{
	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	public int SpouseCharId;

	[SerializableGameDataField]
	public List<CharacterSet> ParticipatingCharacterSets;

	public short DynamicActionType => 0;

	public MarriageTriggerAction()
	{
		Key = MonthlyActionKey.Invalid;
		ParticipatingCharacterSets = new List<CharacterSet>();
		Location = Location.Invalid;
		SpouseCharId = -1;
	}

	public override void TriggerAction()
	{
		if (Location.IsValid() && SpouseCharId >= 0 && DomainManager.Character.TryGetElement_Objects(SpouseCharId, out var element) && !element.IsCompletelyInfected())
		{
			if (DomainManager.Adventure.TryCreateAdventureSite(DomainManager.TaiwuEvent.MainThreadDataContext, Location.AreaId, Location.BlockId, 144, Key))
			{
				MonthlyEventActionsManager.NewlyTriggered++;
				State = 1;
				CallSpouse();
			}
			else
			{
				Location = Location.Invalid;
			}
		}
	}

	public override void MonthlyHandler()
	{
		if (State != 0)
		{
			DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			if (DomainManager.Character.GetAliveSpouse(taiwuCharId) >= 0)
			{
				DomainManager.Adventure.RemoveAdventureSite(mainThreadDataContext, Location.AreaId, Location.BlockId, isTimeout: false, isComplete: false);
				return;
			}
		}
		if (State == 1)
		{
			CallParticipateCharacters();
			Activate();
			State = 5;
		}
	}

	public override void Activate()
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		MonthlyEventActionsManager.NewlyActivated++;
		DomainManager.Adventure.ActivateAdventureSite(mainThreadDataContext, Location.AreaId, Location.BlockId);
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		monthlyNotificationCollection.AddMarryNotice(taiwuCharId, SpouseCharId, Location);
	}

	public override void Deactivate(bool isComplete)
	{
		if (!isComplete)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			if (DomainManager.Character.GetAliveSpouse(taiwuCharId) >= 0)
			{
				monthlyEventCollection.AddTaiwuAlreadyMarried(taiwuCharId, SpouseCharId);
			}
			else
			{
				monthlyEventCollection.AddTaiwuNotAttendingWedding(taiwuCharId, SpouseCharId);
			}
		}
		State = 0;
		Month = 0;
		LastFinishDate = DomainManager.World.GetCurrDate();
		Location = Location.Invalid;
		ClearCalledCharacters();
	}

	public override MonthlyActionBase CreateCopy()
	{
		return GameData.Serializer.Serializer.CreateCopy(this);
	}

	public override void CollectCalledCharacters(HashSet<int> calledCharacters)
	{
		calledCharacters.Add(SpouseCharId);
		foreach (CharacterSet participatingCharacterSet in ParticipatingCharacterSets)
		{
			calledCharacters.UnionWith(participatingCharacterSet.GetCollection());
		}
	}

	public override void FillEventArgBox(EventArgBox eventArgBox)
	{
		AdaptableLog.Info("Adding major characters to adventure.");
		eventArgBox.Set("MajorCharacter_0_0", SpouseCharId);
		eventArgBox.Set("MajorCharacter_0_Count", 1);
		AdaptableLog.Info("Adding participating characters to adventure.");
		for (int i = 0; i < ParticipatingCharacterSets.Count; i++)
		{
			CharacterSet charSet = ((i < ParticipatingCharacterSets.Count) ? ParticipatingCharacterSets[i] : default(CharacterSet));
			FillIntelligentCharactersToArgBox(eventArgBox, $"ParticipateCharacter_{i}", charSet);
			AdventureCharacterSortUtils.Sort(eventArgBox, isMajorChar: false, i, CharacterSortType.CombatPower, ascendingOrder: false);
		}
	}

	private void FillIntelligentCharactersToArgBox(EventArgBox eventArgBox, string keyPrefix, CharacterSet charSet)
	{
		int num = 0;
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		foreach (int item in charSet.GetCollection())
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element))
			{
				if (element.IsCrossAreaTraveling())
				{
					DomainManager.Character.GroupMove(mainThreadDataContext, element, Location);
				}
				if (element.GetLocation().Equals(Location))
				{
					eventArgBox.Set($"{keyPrefix}_{num}", item);
					num++;
				}
			}
		}
		AdaptableLog.Info($"Adding {num} real characters to adventure.");
		eventArgBox.Set(keyPrefix + "_Count", num);
	}

	private void ClearCalledCharacters()
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		if (DomainManager.Character.TryGetElement_Objects(SpouseCharId, out var element))
		{
			element.DeactivateExternalRelationState(mainThreadDataContext, 4);
			if (element.GetLeaderId() != DomainManager.Taiwu.GetTaiwuCharId())
			{
				if (element.IsCompletelyInfected())
				{
					Events.RaiseInfectedCharacterLocationChanged(mainThreadDataContext, SpouseCharId, Location.Invalid, element.GetLocation());
				}
				else
				{
					Events.RaiseCharacterLocationChanged(mainThreadDataContext, SpouseCharId, Location.Invalid, element.GetLocation());
				}
			}
			short settlementId = element.GetOrganizationInfo().SettlementId;
			if (settlementId >= 0)
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
				element.AddTravelTarget(mainThreadDataContext, new NpcTravelTarget(settlement.GetLocation(), 12));
			}
			SpouseCharId = -1;
		}
		CallCharacterHelper.ClearCalledCharacters(ParticipatingCharacterSets, unHideCharacters: true, removeExternalState: true);
	}

	private void CallSpouse()
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(SpouseCharId);
		DomainManager.Character.LeaveGroup(mainThreadDataContext, element_Objects);
		DomainManager.Character.GroupMove(mainThreadDataContext, element_Objects, Location);
		Events.RaiseCharacterLocationChanged(mainThreadDataContext, SpouseCharId, Location, Location.Invalid);
		element_Objects.ActiveExternalRelationState(mainThreadDataContext, 4);
	}

	private void CallParticipateCharacters()
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (ParticipatingCharacterSets.Count <= 0)
		{
			ParticipatingCharacterSets.Add(default(CharacterSet));
		}
		CharacterSet characterSet = ParticipatingCharacterSets[0];
		CallFriendsAndFamilyMembers(mainThreadDataContext, taiwuCharId, ref characterSet, 10);
		ParticipatingCharacterSets[0] = characterSet;
		if (ParticipatingCharacterSets.Count <= 1)
		{
			ParticipatingCharacterSets.Add(default(CharacterSet));
		}
		CharacterSet characterSet2 = ParticipatingCharacterSets[1];
		CallFriendsAndFamilyMembers(mainThreadDataContext, SpouseCharId, ref characterSet2, 10);
		ParticipatingCharacterSets[1] = characterSet2;
		if (ParticipatingCharacterSets.Count <= 2)
		{
			ParticipatingCharacterSets.Add(default(CharacterSet));
		}
		CharacterSet characterSet3 = ParticipatingCharacterSets[2];
		CallTwoWayAdoredCharacters(mainThreadDataContext, taiwuCharId, ref characterSet3, 5);
		ParticipatingCharacterSets[2] = characterSet3;
		if (ParticipatingCharacterSets.Count <= 3)
		{
			ParticipatingCharacterSets.Add(default(CharacterSet));
		}
		CharacterSet characterSet4 = ParticipatingCharacterSets[3];
		CallTwoWayAdoredCharacters(mainThreadDataContext, SpouseCharId, ref characterSet4, 5);
		ParticipatingCharacterSets[3] = characterSet4;
	}

	private void CallFriendsAndFamilyMembers(DataContext context, int charId, ref CharacterSet characterSet, int estimateAmount)
	{
		HashSet<int> obj = context.AdvanceMonthRelatedData.RelatedCharIds.Occupy();
		RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(charId);
		obj.UnionWith(relatedCharacters.BloodParents.GetCollection());
		obj.UnionWith(relatedCharacters.AdoptiveParents.GetCollection());
		obj.UnionWith(relatedCharacters.StepParents.GetCollection());
		obj.UnionWith(relatedCharacters.BloodBrothersAndSisters.GetCollection());
		obj.UnionWith(relatedCharacters.AdoptiveBrothersAndSisters.GetCollection());
		obj.UnionWith(relatedCharacters.StepBrothersAndSisters.GetCollection());
		obj.UnionWith(relatedCharacters.Mentors.GetCollection());
		obj.UnionWith(relatedCharacters.Mentees.GetCollection());
		obj.UnionWith(relatedCharacters.SwornBrothersAndSisters.GetCollection());
		obj.UnionWith(relatedCharacters.Friends.GetCollection());
		obj.UnionWith(relatedCharacters.BloodParents.GetCollection());
		obj.UnionWith(relatedCharacters.AdoptiveParents.GetCollection());
		obj.UnionWith(relatedCharacters.StepParents.GetCollection());
		int num = 0;
		foreach (int item in obj)
		{
			if (num > estimateAmount)
			{
				break;
			}
			if (DomainManager.Character.TryGetElement_Objects(item, out var element) && CheckCharacterAvailable(element) && !DomainManager.Character.HasRelation(item, charId, 16384))
			{
				if (element.GetLeaderId() >= 0)
				{
					DomainManager.Character.LeaveGroup(context, element);
				}
				DomainManager.Character.GroupMove(context, element, Location);
				element.ActiveExternalRelationState(context, 4);
				characterSet.Add(item);
				num++;
			}
		}
		context.AdvanceMonthRelatedData.RelatedCharIds.Release(ref obj);
	}

	private void CallTwoWayAdoredCharacters(DataContext context, int charId, ref CharacterSet characterSet, int estimateAmount)
	{
		RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(charId);
		HashSet<int> collection = relatedCharacters.Adored.GetCollection();
		int num = 0;
		foreach (int item in collection)
		{
			if (num > estimateAmount)
			{
				break;
			}
			if (DomainManager.Character.TryGetElement_Objects(item, out var element) && CheckCharacterAvailable(element) && DomainManager.Character.HasRelation(item, charId, 16384))
			{
				if (element.GetLeaderId() >= 0)
				{
					DomainManager.Character.LeaveGroup(context, element);
				}
				DomainManager.Character.GroupMove(context, element, Location);
				element.ActiveExternalRelationState(context, 4);
				characterSet.Add(item);
				num++;
			}
		}
	}

	private bool CheckCharacterAvailable(GameData.Domains.Character.Character character)
	{
		if (character.IsCompletelyInfected())
		{
			return false;
		}
		if (character.GetLegendaryBookOwnerState() >= 1)
		{
			return false;
		}
		if (character.GetAgeGroup() == 0)
		{
			return false;
		}
		if (!character.GetLocation().IsValid())
		{
			return false;
		}
		if (character.GetCreatingType() != 1)
		{
			return false;
		}
		if (character.GetKidnapperId() >= 0)
		{
			return false;
		}
		if (character.IsActiveExternalRelationState(60))
		{
			return false;
		}
		if (DomainManager.Taiwu.IsInGroup(character.GetId()))
		{
			return false;
		}
		return true;
	}

	public override bool IsSerializedSizeFixed()
	{
		return false;
	}

	public override int GetSerializedSize()
	{
		int num = 22;
		if (ParticipatingCharacterSets != null)
		{
			num += 2;
			int count = ParticipatingCharacterSets.Count;
			for (int i = 0; i < count; i++)
			{
				num += ParticipatingCharacterSets[i].GetSerializedSize();
			}
		}
		else
		{
			num += 2;
		}
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = DynamicActionType;
		ptr += 2;
		ptr += Location.Serialize(ptr);
		*(int*)ptr = SpouseCharId;
		ptr += 4;
		if (ParticipatingCharacterSets != null)
		{
			int count = ParticipatingCharacterSets.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				int num = ParticipatingCharacterSets[i].Serialize(ptr);
				ptr += num;
				Tester.Assert(num <= 65535);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += Key.Serialize(ptr);
		*ptr = (byte)State;
		ptr++;
		*(int*)ptr = Month;
		ptr += 4;
		*(int*)ptr = LastFinishDate;
		ptr += 4;
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += 2;
		ptr += Location.Deserialize(ptr);
		SpouseCharId = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (ParticipatingCharacterSets == null)
			{
				ParticipatingCharacterSets = new List<CharacterSet>(num);
			}
			else
			{
				ParticipatingCharacterSets.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				CharacterSet item = default(CharacterSet);
				ptr += item.Deserialize(ptr);
				ParticipatingCharacterSets.Add(item);
			}
		}
		else
		{
			ParticipatingCharacterSets?.Clear();
		}
		ptr += Key.Deserialize(ptr);
		State = (sbyte)(*ptr);
		ptr++;
		Month = *(int*)ptr;
		ptr += 4;
		LastFinishDate = *(int*)ptr;
		ptr += 4;
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
