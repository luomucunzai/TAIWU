using System;
using System.Collections.Generic;
using Config;
using Config.ConfigCells.Character;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.Character.Creation;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.EventManager;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions;

[SerializableGameData(NotForDisplayModule = true)]
public class MartialArtTournamentMonthlyAction : MonthlyActionBase, ISerializableGameData
{
	[SerializableGameDataField]
	public short CurrentHost;

	[SerializableGameDataField]
	public List<CharacterSet> MajorCharacterSets;

	[SerializableGameDataField]
	public List<CharacterSet> ParticipatingCharacterSets;

	[SerializableGameDataField]
	public Location Location;

	private static readonly sbyte[] WinnerLearnCombatSkillCounts = new sbyte[9] { 1, 1, 2, 3, 3, 4, 5, 5, 6 };

	private static readonly sbyte[] WinnerLearnLifeSkillCounts = new sbyte[9] { 1, 1, 1, 2, 2, 2, 3, 3, 3 };

	private bool _tmpSkipMonth;

	public MonthlyActionsItem ConfigData => MonthlyActions.Instance[(short)30];

	public MartialArtTournamentMonthlyAction()
	{
		CurrentHost = -1;
		LastFinishDate = int.MinValue;
		MajorCharacterSets = new List<CharacterSet>();
		ParticipatingCharacterSets = new List<CharacterSet>();
	}

	public override void MonthlyHandler()
	{
		if (_tmpSkipMonth)
		{
			_tmpSkipMonth = false;
			return;
		}
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		if (DomainManager.World.GetMainStoryLineProgress() >= 27)
		{
			if (State > 0)
			{
				State = 0;
				ClearCalledCharacters();
				CurrentHost = -1;
				Month = 0;
				if (Location.IsValid())
				{
					DomainManager.Adventure.RemoveAdventureSite(mainThreadDataContext, Location.AreaId, Location.BlockId, isTimeout: false, isComplete: false);
				}
				Location = Location.Invalid;
			}
			return;
		}
		if (State == 0)
		{
			if (!DomainManager.World.GetWorldFunctionsStatus(25) || LastFinishDate + 108 > DomainManager.World.GetCurrDate())
			{
				return;
			}
			State = 1;
			Month = 0;
			DomainManager.World.GetMonthlyNotificationCollection().AddWulinConferenceInPreparing();
		}
		if (State == 1)
		{
			if (Month >= 12)
			{
				if (Month == 12 || !Location.IsValid())
				{
					FinishPreparation();
				}
				else
				{
					CallMajorCharacters();
					CallParticipateCharacters();
				}
			}
			else
			{
				SectAskForHelp();
			}
		}
		if (State == 2)
		{
			CallParticipateCharacters();
		}
		if (State == 2 || State == 3)
		{
			bool flag = Month >= 18;
			if ((CallCharacterHelper.IsAllCharactersAtLocation(Location, ConfigData.MajorTargetFilterList, MajorCharacterSets) && CallCharacterHelper.IsAllCharactersAtLocation(Location, ConfigData.ParticipateTargetFilterList, ParticipatingCharacterSets)) || flag)
			{
				State = 5;
				Month = 0;
				Activate();
			}
		}
		if (State != 0)
		{
			Month++;
		}
	}

	private void FinishPreparation()
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		List<MartialArtTournamentPreparationInfo> martialArtTournamentPreparationInfoList = DomainManager.Organization.GetMartialArtTournamentPreparationInfoList();
		CurrentHost = martialArtTournamentPreparationInfoList.Max().SettlementId;
		Sect element_Sects = DomainManager.Organization.GetElement_Sects(CurrentHost);
		Location location = element_Sects.GetLocation();
		Location = Location.Invalid;
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		DomainManager.Map.GetSettlementBlocks(location.AreaId, location.BlockId, list);
		AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(location.AreaId);
		list.RemoveAll((short blockId) => adventuresInArea.AdventureSites.ContainsKey(blockId));
		if (list.Count == 0)
		{
			DomainManager.Map.GetSettlementBlocks(location.AreaId, location.BlockId, list);
		}
		CollectionUtils.Shuffle(mainThreadDataContext.Random, list);
		foreach (short item in list)
		{
			if (DomainManager.Adventure.TryCreateAdventureSite(mainThreadDataContext, location.AreaId, item, ConfigData.AdventureId, Key))
			{
				Location = new Location(location.AreaId, item);
				break;
			}
		}
		if (!Location.IsValid())
		{
			throw new Exception($"No valid location for adventure {Config.Adventure.Instance[ConfigData.AdventureId]}");
		}
		CallMajorCharacters();
		CallParticipateCharacters();
	}

	private void SectAskForHelp()
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		List<MartialArtTournamentPreparationInfo> martialArtTournamentPreparationInfoList = DomainManager.Organization.GetMartialArtTournamentPreparationInfoList();
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		foreach (MartialArtTournamentPreparationInfo item in martialArtTournamentPreparationInfoList)
		{
			short settlementId = item.SettlementId;
			Sect element_Sects = DomainManager.Organization.GetElement_Sects(settlementId);
			short num = element_Sects.CalcApprovingRate();
			if (num >= 500)
			{
				list.Add(settlementId);
			}
		}
		if (list.Count == 0)
		{
			ObjectPool<List<short>>.Instance.Return(list);
			return;
		}
		short random = list.GetRandom(mainThreadDataContext.Random);
		monthlyEventCollection.AddWulinConferenceAskForHelp(random, taiwuCharId);
		ObjectPool<List<short>>.Instance.Return(list);
	}

	public override void Activate()
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		Sect element_Sects = DomainManager.Organization.GetElement_Sects(CurrentHost);
		MonthlyEventActionsManager.NewlyActivated++;
		DomainManager.Adventure.ActivateAdventureSite(mainThreadDataContext, Location.AreaId, Location.BlockId);
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		monthlyNotificationCollection.AddWulinConferenceInProgress(element_Sects.GetLocation());
	}

	public override void Deactivate(bool isComplete)
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		ClearCalledCharacters();
		Location = Location.Invalid;
		State = 0;
		if (CurrentHost >= 0)
		{
			List<short> previousMartialArtTournamentHosts = DomainManager.Organization.GetPreviousMartialArtTournamentHosts();
			if (isComplete)
			{
				LastFinishDate = DomainManager.World.GetCurrDate();
				previousMartialArtTournamentHosts.Add(CurrentHost);
				DomainManager.Organization.SetPreviousMartialArtTournamentHosts(previousMartialArtTournamentHosts, mainThreadDataContext);
				CurrentHost = -1;
				return;
			}
			if (previousMartialArtTournamentHosts.Count == 0)
			{
				CurrentHost = -1;
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection.AddWulinConferenceTaiwuAbsent();
				_tmpSkipMonth = true;
				return;
			}
			sbyte b = SelectWinner();
			AddMartialArtTournamentWinnerPrize(mainThreadDataContext, b);
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(b);
			monthlyNotificationCollection.AddWulinConferenceWinner(settlementIdByOrgTemplateId);
			LastFinishDate = DomainManager.World.GetCurrDate();
			previousMartialArtTournamentHosts.Add(CurrentHost);
			DomainManager.Organization.SetPreviousMartialArtTournamentHosts(previousMartialArtTournamentHosts, mainThreadDataContext);
			CurrentHost = -1;
		}
	}

	private sbyte SelectWinner()
	{
		Span<int> pList = stackalloc int[15];
		Span<int> span = stackalloc int[15];
		Span<int> span2 = stackalloc int[15];
		pList.Fill(0);
		foreach (CharacterSet participatingCharacterSet in ParticipatingCharacterSets)
		{
			foreach (int item in participatingCharacterSet.GetCollection())
			{
				if (!DomainManager.Character.TryGetElement_Objects(item, out var element))
				{
					continue;
				}
				sbyte orgTemplateId = element.GetOrganizationInfo().OrgTemplateId;
				if (OrganizationDomain.IsSect(orgTemplateId))
				{
					int index = orgTemplateId - 1;
					int combatPower = element.GetCombatPower();
					if (combatPower > pList[index])
					{
						pList[index] = combatPower;
					}
					span[index] += combatPower;
					span2[index]++;
				}
			}
		}
		for (int i = 0; i < span.Length; i++)
		{
			if (span2[i] > 0)
			{
				pList[i] += span[i] / span2[i];
			}
		}
		int maxIndex = CollectionUtils.GetMaxIndex(pList);
		int num = 1 + maxIndex;
		return (sbyte)num;
	}

	public static void AddMartialArtTournamentWinnerPrize(DataContext context, sbyte sectTemplateId)
	{
		DomainManager.Extra.RegisterMartialArtTournamentWinner(context, sectTemplateId);
		int num = 3;
		List<TemplateKey> list = new List<TemplateKey>();
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(sectTemplateId);
		OrgMemberCollection members = settlementByOrgTemplateId.GetMembers();
		for (sbyte b = 0; b <= 8; b++)
		{
			HashSet<int> members2 = members.GetMembers(b);
			foreach (int item in members2)
			{
				if (!DomainManager.Character.TryGetElement_Objects(item, out var element))
				{
					continue;
				}
				OrganizationInfo organizationInfo = element.GetOrganizationInfo();
				OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(organizationInfo);
				ArraySegmentList<short> attack = element.GetCombatSkillEquipment().Attack;
				list.Clear();
				for (int i = 0; i < attack.Count; i++)
				{
					short num2 = attack[i];
					if (num2 >= 0)
					{
						CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[num2];
						if (combatSkillItem.MostFittingWeaponID >= 0)
						{
							list.Add(new TemplateKey(0, combatSkillItem.MostFittingWeaponID));
						}
					}
				}
				for (int j = 0; j < orgMemberConfig.Equipment.Length; j++)
				{
					PresetEquipmentItemWithProb presetEquipmentItemWithProb = orgMemberConfig.Equipment[j];
					if (presetEquipmentItemWithProb.Type >= 0 && presetEquipmentItemWithProb.TemplateId >= 0)
					{
						list.Add(new TemplateKey(presetEquipmentItemWithProb.Type, presetEquipmentItemWithProb.TemplateId));
					}
				}
				CollectionUtils.Shuffle(context.Random, list);
				if (num > list.Count)
				{
					num = list.Count;
				}
				for (int k = 0; k < num; k++)
				{
					TemplateKey templateKey = list[k];
					short templateId = (short)(templateKey.TemplateId + organizationInfo.Grade);
					element.CreateInventoryItem(context, templateKey.ItemType, templateId, 1);
				}
				WinnerLearnCombatSkills(context, element);
				WinnerLearnLifeSkills(context, element);
			}
		}
	}

	private static void WinnerLearnCombatSkills(DataContext context, GameData.Domains.Character.Character character)
	{
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		sbyte behaviorType = character.GetBehaviorType();
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(character.GetId());
		List<sbyte> combatSkillTypes = Config.Organization.Instance[organizationInfo.OrgTemplateId].CombatSkillTypes;
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		foreach (sbyte item in combatSkillTypes)
		{
			IReadOnlyList<CombatSkillItem> learnableCombatSkills = CombatSkillDomain.GetLearnableCombatSkills(organizationInfo.OrgTemplateId, item);
			for (int i = 0; i < learnableCombatSkills.Count; i++)
			{
				CombatSkillItem combatSkillItem = learnableCombatSkills[i];
				if (charCombatSkills.TryGetValue(combatSkillItem.TemplateId, out var value))
				{
					if (value.CanBreakout() && !CombatSkillStateHelper.IsBrokenOut(value.GetActivationState()))
					{
						list.Add(combatSkillItem.TemplateId);
					}
				}
				else
				{
					list.Add(combatSkillItem.TemplateId);
				}
			}
		}
		CollectionUtils.Shuffle(context.Random, list);
		int num = WinnerLearnCombatSkillCounts[organizationInfo.Grade];
		if (num > list.Count)
		{
			num = list.Count;
		}
		for (int j = 0; j < num; j++)
		{
			short num2 = list[j];
			if (charCombatSkills.TryGetValue(num2, out var value2))
			{
				value2.SetPracticeLevel(100, context);
				value2.SetReadingState(32767, context);
				ushort activationState = CombatSkillStateHelper.GenerateRandomActivatedNormalPages(context.Random, 32767, 0);
				activationState = CombatSkillStateHelper.GenerateRandomActivatedOutlinePage(context.Random, 32767, activationState, behaviorType);
				value2.SetActivationState(activationState, context);
				value2.SetBreakoutStepsCount(GlobalConfig.Instance.BreakoutSpecialNpcStepsCount, context);
				value2.SetForcedBreakoutStepsCount(0, context);
			}
			else
			{
				character.LearnNewCombatSkill(context, num2, 32767);
			}
		}
	}

	private static void WinnerLearnLifeSkills(DataContext context, GameData.Domains.Character.Character character)
	{
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(organizationInfo);
		List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = character.GetLearnedLifeSkills();
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		for (sbyte b = 0; b < orgMemberConfig.LifeSkillsAdjust.Length; b++)
		{
			short num = orgMemberConfig.LifeSkillsAdjust[b];
			if (num > 0)
			{
				for (int i = 0; i <= orgMemberConfig.LifeSkillGradeLimit; i++)
				{
					short num2 = Config.LifeSkillType.Instance[b].SkillList[i];
					int num3 = character.FindLearnedLifeSkillIndex(num2);
					if (num3 < 0)
					{
						list.Add(num2);
						continue;
					}
					GameData.Domains.Character.LifeSkillItem lifeSkillItem = learnedLifeSkills[num3];
					if (!lifeSkillItem.IsAllPagesRead())
					{
						list.Add(lifeSkillItem.SkillTemplateId);
					}
				}
			}
		}
		CollectionUtils.Shuffle(context.Random, list);
		int num4 = WinnerLearnLifeSkillCounts[organizationInfo.Grade];
		if (list.Count < num4)
		{
			num4 = list.Count;
		}
		for (int j = 0; j < num4; j++)
		{
			short num5 = list[j];
			int num6 = character.FindLearnedLifeSkillIndex(num5);
			if (num6 < 0)
			{
				character.LearnNewLifeSkill(context, num5, 31);
			}
			else
			{
				character.UpdateLifeSkillReadingState(context, num6, 31);
			}
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

	public override MonthlyActionBase CreateCopy()
	{
		return GameData.Serializer.Serializer.CreateCopy(this);
	}

	public void ClearCalledCharacters()
	{
		CallCharacterHelper.ClearCalledCharacters(MajorCharacterSets, !ConfigData.MajorTargetMoveVisible, removeExternalState: true);
		CallCharacterHelper.ClearCalledCharacters(ParticipatingCharacterSets, unHideCharacters: false, removeExternalState: true);
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
			FillIntelligentCharactersToArgBox(eventArgBox, $"ParticipateCharacter_{j}", charSet2, filterReq2, allowTempChars: true);
			AdventureCharacterSortUtils.Sort(eventArgBox, isMajorChar: false, j, CharacterSortType.CombatPower, ascendingOrder: false);
		}
	}

	private void FillIntelligentCharactersToArgBox(EventArgBox eventArgBox, string keyPrefix, CharacterSet charSet, CharacterFilterRequirement filterReq, bool allowTempChars)
	{
		int num = 0;
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		Span<int> span = stackalloc int[charSet.GetCount()];
		SpanList<int> spanList = span;
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
				if (filterReq.HasMaximum() && num >= filterReq.MaxCharactersRequired)
				{
					Events.RaiseCharacterLocationChanged(mainThreadDataContext, item, element.GetLocation(), Location);
					element.SetLocation(Location, mainThreadDataContext);
					continue;
				}
				eventArgBox.Set($"{keyPrefix}_{num}", item);
				num++;
				spanList.Add(item);
			}
		}
		AdaptableLog.Info($"Adding {num} real characters to adventure.");
		if (allowTempChars && filterReq.CharacterFilterRuleIds[0] == 16)
		{
			AdaptableLog.Info($"creating {Math.Max(0, filterReq.MinCharactersRequired - num)} temporary characters.");
			foreach (OrganizationItem item2 in (IEnumerable<OrganizationItem>)Config.Organization.Instance)
			{
				if (!item2.IsSect)
				{
					continue;
				}
				Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(item2.TemplateId);
				GameData.Domains.Character.Character leader = settlementByOrgTemplateId.GetLeader();
				if (leader == null || !spanList.Contains(leader.GetId()))
				{
					sbyte b = item2.GenderRestriction;
					if (b < 0)
					{
						b = Gender.GetRandom(mainThreadDataContext.Random);
					}
					sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(settlementByOrgTemplateId.GetLocation().AreaId);
					short characterTemplateId = OrganizationDomain.GetCharacterTemplateId(item2.TemplateId, stateTemplateIdByAreaId, b);
					TemporaryIntelligentCharacterCreationInfo tmpInfo = new TemporaryIntelligentCharacterCreationInfo
					{
						Location = Location,
						CharTemplateId = characterTemplateId,
						OrgInfo = new OrganizationInfo(item2.TemplateId, 8, principal: true, settlementByOrgTemplateId.GetId())
					};
					GameData.Domains.Character.Character character = DomainManager.Character.CreateTemporaryIntelligentCharacter(mainThreadDataContext, ref tmpInfo);
					OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(tmpInfo.OrgInfo);
					var (value, value2) = CharacterDomain.GetRealName(character);
					AdaptableLog.TagInfo("MartialArtTournamentMonthlyAction", $"Creating temporary character {item2.Name}-{orgMemberConfig.GradeName} {value}{value2}({character.GetId()})");
					int id = character.GetId();
					DomainManager.Adventure.AddTemporaryIntelligentCharacter(id);
					eventArgBox.Set($"{keyPrefix}_{num}", id);
					num++;
				}
			}
		}
		eventArgBox.Set(keyPrefix + "_Count", num);
	}

	public override void EnsurePrerequisites()
	{
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(Location.AreaId);
		int num = CallCharacterHelper.RemoveInvalidCharacters(ConfigData.MajorTargetFilterList, MajorCharacterSets, Location, stateTemplateIdByAreaId);
		if (num > 0)
		{
			AdaptableLog.TagWarning("ConfigMonthlyAction", $"{ConfigData.Name}{Key} removed {num} major characters that are invalid");
		}
		int num2 = CallCharacterHelper.RemoveInvalidCharacters(ConfigData.ParticipateTargetFilterList, ParticipatingCharacterSets, Location, stateTemplateIdByAreaId);
		if (num2 > 0)
		{
			AdaptableLog.TagWarning("ConfigMonthlyAction", $"{ConfigData.Name}{Key} removed {num2} participate characters that are invalid");
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

	private void CallMajorCharacters()
	{
		if (Location.IsValid() && CallCharacterHelper.CallCharacters(Location, ConfigData.CharacterSearchRange, ConfigData.MajorTargetFilterList, MajorCharacterSets, ConfigData.AllowTemporaryMajorCharacter, modifyExternalState: true, !ConfigData.MajorTargetMoveVisible))
		{
			State = 2;
		}
	}

	private void CallParticipateCharacters()
	{
		if (Location.IsValid())
		{
			ConfigData.ParticipateTargetFilterList[0].MinCharactersRequired = 0;
			if (CallCharacterHelper.CallCharacters(Location, ConfigData.CharacterSearchRange, ConfigData.ParticipateTargetFilterList, ParticipatingCharacterSets, ConfigData.AllowTemporaryParticipateCharacter, modifyExternalState: true))
			{
				State = 3;
			}
		}
	}

	public override bool IsSerializedSizeFixed()
	{
		return false;
	}

	public override int GetSerializedSize()
	{
		int num = 18;
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
		*(short*)ptr = CurrentHost;
		ptr += 2;
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
		ptr += Location.Serialize(ptr);
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
		CurrentHost = *(short*)ptr;
		ptr += 2;
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
		ptr += Location.Deserialize(ptr);
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
