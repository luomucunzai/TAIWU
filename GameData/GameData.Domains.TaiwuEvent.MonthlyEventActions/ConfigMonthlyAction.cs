using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using Config.ConfigCells.Character;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.LifeRecord;
using GameData.Domains.LifeRecord.GeneralRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Domains.TaiwuEvent.EventManager;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions;

[SerializableGameData(NotForDisplayModule = true)]
public class ConfigMonthlyAction : MonthlyActionBase, ISerializableGameData, IRecordArgumentSource
{
	[SerializableGameDataField]
	public short ConfigTemplateId;

	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	public List<CharacterSet> MajorCharacterSets;

	[SerializableGameDataField]
	public List<CharacterSet> ParticipatingCharacterSets;

	[SerializableGameDataField]
	private sbyte _announceMonth;

	[SerializableGameDataField]
	private short _assignedAreaId;

	public MonthlyActionsItem ConfigData => MonthlyActions.Instance[ConfigTemplateId];

	public ConfigMonthlyAction(short templateId, short assignedAreaId = -1)
	{
		Key = new MonthlyActionKey(0, templateId);
		ConfigTemplateId = templateId;
		State = 0;
		MajorCharacterSets = new List<CharacterSet>();
		ParticipatingCharacterSets = new List<CharacterSet>();
		Location = Location.Invalid;
		_assignedAreaId = assignedAreaId;
	}

	public ConfigMonthlyAction(MonthlyActionKey key, short templateId, short assignedAreaId = -1)
	{
		Key = key;
		ConfigTemplateId = templateId;
		State = 0;
		MajorCharacterSets = new List<CharacterSet>();
		ParticipatingCharacterSets = new List<CharacterSet>();
		Location = Location.Invalid;
		_assignedAreaId = assignedAreaId;
	}

	public override bool IsMonthMatch()
	{
		return ConfigData.EnterMonthList.Count == 0 || ConfigData.EnterMonthList.Contains(DomainManager.World.GetCurrMonthInYear());
	}

	public override void TriggerAction()
	{
		Month = 0;
		ClearCalledCharacters();
		if (Location.IsValid())
		{
			if (DomainManager.Adventure.TryCreateAdventureSite(DomainManager.TaiwuEvent.MainThreadDataContext, Location.AreaId, Location.BlockId, ConfigData.AdventureId, Key))
			{
				MonthlyEventActionsManager.NewlyTriggered++;
				State = 1;
			}
			else
			{
				Location = Location.Invalid;
			}
		}
	}

	public override void MonthlyHandler()
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		if (Config.Adventure.Instance.GetItem(ConfigData.AdventureId) == null || DomainManager.World.GetMainStoryLineProgress() < 3 || (ConfigData.MinInterval <= 0 && State == 0))
		{
			return;
		}
		if (State == 0 && IsMonthMatch() && MonthlyEventActionsManager.ConfigItemTriggerCheck(ConfigTemplateId, this))
		{
			if (LastFinishDate + ConfigData.MinInterval >= DomainManager.World.GetCurrDate())
			{
				return;
			}
			SelectLocation();
			TriggerAction();
		}
		bool flag = ConfigData.PreparationDuration > 0 && Month >= ConfigData.PreparationDuration - ConfigData.PreannouncingTime;
		if (State == 1)
		{
			CallMajorCharacters();
			CallParticipateCharacters();
		}
		if (State == 2)
		{
			CallParticipateCharacters();
		}
		sbyte state = State;
		bool flag2 = (uint)(state - 2) <= 2u;
		if (flag2 && (flag || ConfigData.CanActionBeforehand) && (ConfigData.AllowTemporaryMajorCharacter || CallCharacterHelper.IsAllCharactersAtLocation(Location, ConfigData.MajorTargetFilterList, MajorCharacterSets)) && (ConfigData.AllowTemporaryParticipateCharacter || CallCharacterHelper.IsAllCharactersAtLocation(Location, ConfigData.ParticipateTargetFilterList, ParticipatingCharacterSets)))
		{
			if (State != 4)
			{
				if (ConfigData.PreannouncingTime <= 0)
				{
					State = 5;
					Activate();
				}
				else
				{
					State = 4;
					_announceMonth = 0;
					MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
					monthlyNotificationCollection.AddConfigMonthlyActionAnnouncementNotification(ConfigData, this, ConfigData.PreannouncingTime);
				}
			}
			else
			{
				_announceMonth++;
				if (_announceMonth >= ConfigData.PreannouncingTime)
				{
					State = 5;
					Activate();
				}
				else
				{
					MonthlyNotificationCollection monthlyNotificationCollection2 = DomainManager.World.GetMonthlyNotificationCollection();
					monthlyNotificationCollection2.AddConfigMonthlyActionAnnouncementNotification(ConfigData, this, ConfigData.PreannouncingTime - _announceMonth);
				}
			}
		}
		if (State > 0 && State <= 3 && flag)
		{
			if (ConfigData.AdventureId >= 0 && Location.IsValid())
			{
				DomainManager.Adventure.RemoveAdventureSite(mainThreadDataContext, Location.AreaId, Location.BlockId, isTimeout: true, isComplete: false);
			}
			else
			{
				Deactivate(isComplete: false);
			}
		}
		ValidationHandler();
		if (State != 0)
		{
			Month++;
		}
	}

	public override void Activate()
	{
		if (ConfigData.AdventureId >= 0)
		{
			DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
			MonthlyEventActionsManager.NewlyActivated++;
			DomainManager.Adventure.ActivateAdventureSite(mainThreadDataContext, Location.AreaId, Location.BlockId);
		}
		MonthlyEventActionsManager.ConfigItemOnActivate(ConfigTemplateId, this);
	}

	public override void Deactivate(bool isComplete)
	{
		MonthlyEventActionsManager.ConfigItemOnDeactivate(ConfigTemplateId, this, isComplete);
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

	public void ClearCalledCharacters()
	{
		CallCharacterHelper.ClearCalledCharacters(MajorCharacterSets, !ConfigData.MajorTargetMoveVisible, removeExternalState: true);
		CallCharacterHelper.ClearCalledCharacters(ParticipatingCharacterSets, unHideCharacters: false, removeExternalState: true);
	}

	public override void EnsurePrerequisites()
	{
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(Location.AreaId);
		int num = CallCharacterHelper.RemoveInvalidCharacters(ConfigData.MajorTargetFilterList, MajorCharacterSets, Location, stateTemplateIdByAreaId);
		if (num > 0)
		{
			AdaptableLog.TagWarning("ConfigMonthlyAction", $"{ConfigData.Name}{Key} at {Location} removed {num} major characters that are invalid");
		}
		int num2 = CallCharacterHelper.RemoveInvalidCharacters(ConfigData.ParticipateTargetFilterList, ParticipatingCharacterSets, Location, stateTemplateIdByAreaId);
		if (num2 > 0)
		{
			AdaptableLog.TagWarning("ConfigMonthlyAction", $"{ConfigData.Name}{Key} at {Location} removed {num2} participate characters that are invalid");
		}
		if (!ConfigData.AllowTemporaryMajorCharacter && !CallCharacterHelper.IsAllCharactersAtLocation(Location, ConfigData.MajorTargetFilterList, MajorCharacterSets))
		{
			CallMajorCharacters();
		}
		if (!ConfigData.AllowTemporaryParticipateCharacter && !CallCharacterHelper.IsAllCharactersAtLocation(Location, ConfigData.ParticipateTargetFilterList, ParticipatingCharacterSets))
		{
			CallParticipateCharacters();
		}
		State = 5;
	}

	public override void ValidationHandler()
	{
		if (State == 5 && (!Location.IsValid() || DomainManager.Adventure.GetAdventureSiteState(Location.AreaId, Location.BlockId) == 1) && (!CheckValid() || MonthlyEventActionsManager.CheckConfigActionBecomeInvalid(ConfigTemplateId, this)))
		{
			MonthlyEventActionsManager.OnConfigActionBecomeInvalid(ConfigTemplateId, this);
		}
	}

	private bool CheckValid()
	{
		if (!ConfigData.AllowTemporaryMajorCharacter && !CallCharacterHelper.IsAllCharactersAtLocation(Location, ConfigData.MajorTargetFilterList, MajorCharacterSets))
		{
			return false;
		}
		if (!ConfigData.AllowTemporaryParticipateCharacter && !CallCharacterHelper.IsAllCharactersAtLocation(Location, ConfigData.ParticipateTargetFilterList, ParticipatingCharacterSets))
		{
			return false;
		}
		return true;
	}

	public override void FillEventArgBox(EventArgBox eventArgBox)
	{
		AdaptableLog.Info("Adding major characters to adventure.");
		for (int i = 0; i < ConfigData.MajorTargetFilterList.Length; i++)
		{
			CharacterFilterRequirement filterReq = ConfigData.MajorTargetFilterList[i];
			CharacterSet charSet = ((i < MajorCharacterSets.Count) ? MajorCharacterSets[i] : default(CharacterSet));
			FillIntelligentCharactersToArgBox(eventArgBox, $"MajorCharacter_{i}", charSet, filterReq, ConfigData.AllowTemporaryMajorCharacter);
			AdventureCharacterSortUtils.Sort(eventArgBox, isMajorChar: true, i, CharacterSortType.CombatPower, ascendingOrder: false);
		}
		AdaptableLog.Info("Adding participating characters to adventure.");
		for (int j = 0; j < ConfigData.ParticipateTargetFilterList.Length; j++)
		{
			CharacterFilterRequirement filterReq2 = ConfigData.ParticipateTargetFilterList[j];
			CharacterSet charSet2 = ((j < ParticipatingCharacterSets.Count) ? ParticipatingCharacterSets[j] : default(CharacterSet));
			FillIntelligentCharactersToArgBox(eventArgBox, $"ParticipateCharacter_{j}", charSet2, filterReq2, ConfigData.AllowTemporaryParticipateCharacter);
			AdventureCharacterSortUtils.Sort(eventArgBox, isMajorChar: false, j, CharacterSortType.CombatPower, ascendingOrder: false);
		}
	}

	public override void CollectCalledCharacters(HashSet<int> calledCharacters)
	{
		if (ConfigData.MinInterval <= 0 && Key.ActionType == 0)
		{
			MajorCharacterSets.ForEach(delegate(CharacterSet charSet)
			{
				charSet.Clear();
			});
			ParticipatingCharacterSets.ForEach(delegate(CharacterSet charSet)
			{
				charSet.Clear();
			});
			MajorCharacterSets.Clear();
			ParticipatingCharacterSets.Clear();
			return;
		}
		foreach (CharacterSet majorCharacterSet in MajorCharacterSets)
		{
			calledCharacters.UnionWith(majorCharacterSet.GetCollection());
		}
		foreach (CharacterSet participatingCharacterSet in ParticipatingCharacterSets)
		{
			calledCharacters.UnionWith(participatingCharacterSet.GetCollection());
		}
	}

	private void FillIntelligentCharactersToArgBox(EventArgBox eventArgBox, string keyPrefix, CharacterSet charSet, CharacterFilterRequirement filterReq, bool allowTempChars)
	{
		int i = 0;
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		foreach (int item in charSet.GetCollection())
		{
			if (!DomainManager.Character.TryGetElement_Objects(item, out var element))
			{
				continue;
			}
			if (element.IsCrossAreaTraveling())
			{
				DomainManager.Character.GroupMove(mainThreadDataContext, element, Location);
			}
			if (element.GetLocation().Equals(Location))
			{
				if (filterReq.HasMaximum() && i >= filterReq.MaxCharactersRequired)
				{
					Events.RaiseCharacterLocationChanged(mainThreadDataContext, item, element.GetLocation(), Location);
					element.SetLocation(Location, mainThreadDataContext);
					continue;
				}
				eventArgBox.Set($"{keyPrefix}_{i}", item);
				i++;
			}
		}
		AdaptableLog.Info($"Adding {i} real characters to adventure.");
		if (allowTempChars)
		{
			AdaptableLog.Info($"creating {Math.Max(0, filterReq.MinCharactersRequired - i)} temporary characters.");
			for (; i < filterReq.MinCharactersRequired; i++)
			{
				short characterFilterRulesId = filterReq.CharacterFilterRuleIds[mainThreadDataContext.Random.Next(0, filterReq.CharacterFilterRuleIds.Length)];
				GameData.Domains.Character.Character character = DomainManager.Character.CreateTemporaryIntelligentCharacter(mainThreadDataContext, characterFilterRulesId, Location);
				int id = character.GetId();
				DomainManager.Adventure.AddTemporaryIntelligentCharacter(id);
				eventArgBox.Set($"{keyPrefix}_{i}", id);
			}
		}
		eventArgBox.Set(keyPrefix + "_Count", i);
	}

	private void CallMajorCharacters()
	{
		if (Location.IsValid() && CallCharacterHelper.CallCharacters(Location, ConfigData.CharacterSearchRange, ConfigData.MajorTargetFilterList, MajorCharacterSets, ConfigData.AllowTemporaryMajorCharacter, modifyExternalState: true, !ConfigData.MajorTargetMoveVisible, OnMajorCharacterCalled))
		{
			State = 2;
		}
	}

	private void CallParticipateCharacters()
	{
		if (Location.IsValid() && CallCharacterHelper.CallCharacters(Location, ConfigData.CharacterSearchRange, ConfigData.ParticipateTargetFilterList, ParticipatingCharacterSets, ConfigData.AllowTemporaryParticipateCharacter, modifyExternalState: true))
		{
			State = 3;
		}
	}

	private void OnMajorCharacterCalled(DataContext context, GameData.Domains.Character.Character character)
	{
		if (ConfigData.IsEnemyNest)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int id = character.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			lifeRecordCollection.AddEnterEnemyNest(id, currDate, Location, ConfigData.AdventureId);
		}
	}

	public void SelectLocation()
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		MapBlockData mapBlockData = ((_assignedAreaId < 0) ? DomainManager.Map.GetRandomMapBlockDataByFilters(mainThreadDataContext.Random, ConfigData.MapState, ConfigData.MapArea, ConfigData.MapBlockSubType, includeBlockWithAdventure: false) : DomainManager.Map.GetRandomMapBlockDataInAreaByFilters(mainThreadDataContext.Random, _assignedAreaId, ConfigData.MapBlockSubType, includeBlocksWithAdventure: false));
		if (mapBlockData != null)
		{
			Location.AreaId = mapBlockData.AreaId;
			Location.BlockId = mapBlockData.BlockId;
		}
		else
		{
			Location.AreaId = -1;
			Location.BlockId = -1;
		}
	}

	public short GetSettlementArg()
	{
		MapBlockData belongSettlementBlock = DomainManager.Map.GetBelongSettlementBlock(Location);
		Settlement settlementByLocation = DomainManager.Organization.GetSettlementByLocation(belongSettlementBlock.GetLocation());
		return settlementByLocation.GetId();
	}

	public Location GetLocationArg()
	{
		return Location;
	}

	public int GetCharacterArg()
	{
		if (MajorCharacterSets.Count < 1)
		{
			return -1;
		}
		HashSet<int> collection = MajorCharacterSets[0].GetCollection();
		if (collection.Count < 1)
		{
			return -1;
		}
		return collection.First();
	}

	public short GetAdventureArg()
	{
		return ConfigData.AdventureId;
	}

	public sbyte GetLifeSkillTypeArg()
	{
		return ConfigMonthlyActionDefines.MonthlyActionToLifeSkillType[ConfigTemplateId];
	}

	public ConfigMonthlyAction()
	{
		MajorCharacterSets = new List<CharacterSet>();
		ParticipatingCharacterSets = new List<CharacterSet>();
	}

	public override bool IsSerializedSizeFixed()
	{
		return false;
	}

	public override int GetSerializedSize()
	{
		int num = 21;
		if (MajorCharacterSets != null)
		{
			num += 2;
			int count = MajorCharacterSets.Count;
			for (int i = 0; i < count; i++)
			{
				num += MajorCharacterSets[i].GetSerializedSize();
			}
		}
		else
		{
			num += 2;
		}
		if (ParticipatingCharacterSets != null)
		{
			num += 2;
			int count2 = ParticipatingCharacterSets.Count;
			for (int j = 0; j < count2; j++)
			{
				num += ParticipatingCharacterSets[j].GetSerializedSize();
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
		*(short*)ptr = ConfigTemplateId;
		ptr += 2;
		ptr += Location.Serialize(ptr);
		if (MajorCharacterSets != null)
		{
			int count = MajorCharacterSets.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				int num = MajorCharacterSets[i].Serialize(ptr);
				ptr += num;
				Tester.Assert(num <= 65535);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (ParticipatingCharacterSets != null)
		{
			int count2 = ParticipatingCharacterSets.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				int num2 = ParticipatingCharacterSets[j].Serialize(ptr);
				ptr += num2;
				Tester.Assert(num2 <= 65535);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)_announceMonth;
		ptr++;
		*(short*)ptr = _assignedAreaId;
		ptr += 2;
		ptr += Key.Serialize(ptr);
		*ptr = (byte)State;
		ptr++;
		*(int*)ptr = Month;
		ptr += 4;
		*(int*)ptr = LastFinishDate;
		ptr += 4;
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ConfigTemplateId = *(short*)ptr;
		ptr += 2;
		ptr += Location.Deserialize(ptr);
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (MajorCharacterSets == null)
			{
				MajorCharacterSets = new List<CharacterSet>(num);
			}
			else
			{
				MajorCharacterSets.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				CharacterSet item = default(CharacterSet);
				ptr += item.Deserialize(ptr);
				MajorCharacterSets.Add(item);
			}
		}
		else
		{
			MajorCharacterSets?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (ParticipatingCharacterSets == null)
			{
				ParticipatingCharacterSets = new List<CharacterSet>(num2);
			}
			else
			{
				ParticipatingCharacterSets.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				CharacterSet item2 = default(CharacterSet);
				ptr += item2.Deserialize(ptr);
				ParticipatingCharacterSets.Add(item2);
			}
		}
		else
		{
			ParticipatingCharacterSets?.Clear();
		}
		_announceMonth = (sbyte)(*ptr);
		ptr++;
		_assignedAreaId = *(short*)ptr;
		ptr += 2;
		ptr += Key.Deserialize(ptr);
		State = (sbyte)(*ptr);
		ptr++;
		Month = *(int*)ptr;
		ptr += 4;
		LastFinishDate = *(int*)ptr;
		ptr += 4;
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}
}
