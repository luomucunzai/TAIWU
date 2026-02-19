using System;
using System.Collections.Generic;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.LegendaryBook;

[GameDataDomain(11)]
public class LegendaryBookDomain : BaseGameDataDomain
{
	[DomainData(DomainDataType.ElementList, true, false, true, true, ArrayElementsCount = 14)]
	private readonly int[] _bookOwners;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private LegendaryBookOwnerData _legendaryBookOwnerData;

	private Dictionary<int, List<sbyte>> _charBookTypes;

	private readonly HashSet<int> _actCrazyShockedCharIds = new HashSet<int>();

	private readonly LegendaryBookMonthlyAction[] _legendaryBookMonthlyActions = new LegendaryBookMonthlyAction[14];

	private readonly ItemKey[] _legendaryBookItems = new ItemKey[14];

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[2][];

	private static readonly DataInfluence[][] CacheInfluencesBookOwners = new DataInfluence[14][];

	private readonly byte[] _dataStatesBookOwners = new byte[4];

	private Queue<uint> _pendingLoadingOperationIds;

	public void InitializeOwnedItems()
	{
		for (int i = 0; i < _legendaryBookMonthlyActions.Length; i++)
		{
			LegendaryBookMonthlyAction legendaryBookMonthlyAction = _legendaryBookMonthlyActions[i];
			if (legendaryBookMonthlyAction != null)
			{
				ItemKey itemKey = _legendaryBookItems[legendaryBookMonthlyAction.BookType];
				DomainManager.Item.SetOwner(itemKey, ItemOwnerType.System, 11);
			}
		}
	}

	private void OnInitializedDomainData()
	{
		InitializeLegendaryBookItems();
	}

	private void InitializeOnInitializeGameDataModule()
	{
	}

	private void InitializeOnEnterNewWorld()
	{
		for (int i = 0; i < 14; i++)
		{
			_bookOwners[i] = -1;
		}
		InitializeCharBookTypes();
	}

	private void OnLoadedArchiveData()
	{
		InitializeCharBookTypes();
	}

	public override void FixAbnormalDomainArchiveData(DataContext context)
	{
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 71, 58))
		{
			FixAbnormalLostLegendaryBooksByThreeCorpsesGiveUpAction(context);
		}
	}

	private void FixAbnormalLostLegendaryBooksByThreeCorpsesGiveUpAction(DataContext context)
	{
		if (DomainManager.World.GetSectMainStoryTaskStatus(7) != 1)
		{
			return;
		}
		SectStoryThreeCorpsesCharacter ranshanThreeCorpsesCharacterByTemplateId = DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(660);
		if (ranshanThreeCorpsesCharacterByTemplateId == null || !ranshanThreeCorpsesCharacterByTemplateId.IsGoodEnd)
		{
			ranshanThreeCorpsesCharacterByTemplateId = DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(661);
			if (ranshanThreeCorpsesCharacterByTemplateId == null || !ranshanThreeCorpsesCharacterByTemplateId.IsGoodEnd)
			{
				ranshanThreeCorpsesCharacterByTemplateId = DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(662);
				if (ranshanThreeCorpsesCharacterByTemplateId == null || !ranshanThreeCorpsesCharacterByTemplateId.IsGoodEnd)
				{
					return;
				}
			}
		}
		for (sbyte b = 0; b < 14; b++)
		{
			if (_legendaryBookItems[b] != ItemKey.Invalid && DomainManager.Item.GetBaseItem(_legendaryBookItems[b]).Owner.OwnerType == ItemOwnerType.None)
			{
				short areaId = (short)context.Random.Next(45);
				MapBlockData randomMapBlockDataInAreaByFilters = DomainManager.Map.GetRandomMapBlockDataInAreaByFilters(context.Random, areaId, null, includeBlocksWithAdventure: false);
				LegendaryBookMonthlyAction action = new LegendaryBookMonthlyAction
				{
					Location = randomMapBlockDataInAreaByFilters.GetLocation(),
					BookType = b,
					BookAppearType = 0
				};
				DomainManager.TaiwuEvent.AddTempDynamicAction(context, action);
				AdaptableLog.Warning($"Fix abnormal lost legendary book by generating its adventure: {b}");
			}
		}
	}

	internal void RegisterLegendaryBookMonthlyAction(LegendaryBookMonthlyAction action)
	{
		if (_legendaryBookMonthlyActions[action.BookType] != null)
		{
			AdaptableLog.Warning($"Legendary book monthly action already registered {Config.CombatSkillType.Instance[action.BookType]}");
		}
		_legendaryBookMonthlyActions[action.BookType] = action;
	}

	internal void UnregisterLegendaryBookMonthlyAction(sbyte bookType)
	{
		_legendaryBookMonthlyActions[bookType] = null;
	}

	public LegendaryBookMonthlyAction GetOnGoingMonthlyAction(sbyte bookType)
	{
		return _legendaryBookMonthlyActions[bookType];
	}

	public void ClearActCrazyShockedCharacters()
	{
		_actCrazyShockedCharIds.Clear();
	}

	public void AddActCrazyShockedCharacters(int charId)
	{
		_actCrazyShockedCharIds.Add(charId);
	}

	public bool IsCharacterActingCrazy(GameData.Domains.Character.Character character)
	{
		sbyte legendaryBookOwnerState = character.GetLegendaryBookOwnerState();
		if (1 == 0)
		{
		}
		bool result = legendaryBookOwnerState >= 1 && (legendaryBookOwnerState != 1 || _actCrazyShockedCharIds.Contains(character.GetId()));
		if (1 == 0)
		{
		}
		return result;
	}

	public void CreateLegendaryBooksAccordingToXiangshuProgress(DataContext context)
	{
		if (!DomainManager.World.GetWorldFunctionsStatus(21))
		{
			DomainManager.Extra.SetFirstLegendaryBookDelay(context, (sbyte)context.Random.Next(3, 9));
			return;
		}
		sbyte firstLegendaryBookDelay = DomainManager.Extra.GetFirstLegendaryBookDelay();
		if (firstLegendaryBookDelay > 0)
		{
			DomainManager.Extra.SetFirstLegendaryBookDelay(context, (sbyte)(firstLegendaryBookDelay - 1));
			return;
		}
		List<TemplateKey> obj = context.AdvanceMonthRelatedData.ItemTemplateKeys.Occupy();
		for (sbyte b = 0; b < 14; b++)
		{
			short num = (short)(211 + b);
			if (!DomainManager.Item.HasTrackedSpecialItems(12, num))
			{
				obj.Add(new TemplateKey(12, num));
			}
		}
		CollectionUtils.Shuffle(context.Random, obj);
		int num2 = Math.Clamp(DomainManager.World.GetXiangshuLevel() - 1, 0, GlobalConfig.Instance.LegendaryBookAppearAmounts.Length - 1);
		sbyte b2 = GlobalConfig.Instance.LegendaryBookAppearAmounts[num2];
		int num3 = 14 - obj.Count;
		if (num3 < b2 && context.Random.CheckPercentProb(GlobalConfig.Instance.LegendaryBookAppearChance))
		{
			TemplateKey templateKey;
			short areaId;
			if (num3 == 0)
			{
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				templateKey = obj[0];
				foreach (TemplateKey item in obj)
				{
					if (item.TemplateId > 213 && taiwu.GetCombatSkillAttainment((sbyte)(templateKey.TemplateId - 211)) < taiwu.GetCombatSkillAttainment((sbyte)(item.TemplateId - 211)))
					{
						templateKey = item;
					}
				}
				areaId = DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId;
			}
			else
			{
				templateKey = obj.GetRandom(context.Random);
				areaId = (short)context.Random.Next(45);
			}
			sbyte bookType = (sbyte)(templateKey.TemplateId - 211);
			ItemKey itemKey = DomainManager.Item.CreateItem(context, templateKey.ItemType, templateKey.TemplateId);
			MapBlockData randomMapBlockDataInAreaByFilters = DomainManager.Map.GetRandomMapBlockDataInAreaByFilters(context.Random, areaId, null, includeBlocksWithAdventure: false);
			LegendaryBookMonthlyAction action = new LegendaryBookMonthlyAction
			{
				Location = randomMapBlockDataInAreaByFilters.GetLocation(),
				BookType = bookType,
				BookAppearType = 0
			};
			DomainManager.TaiwuEvent.AddTempDynamicAction(context, action);
		}
		context.AdvanceMonthRelatedData.ItemTemplateKeys.Release(ref obj);
	}

	public void Test_GiveUnownedLegendaryBookToTaiwu(DataContext context)
	{
		List<ItemKey> warehouseAllItemKey = DomainManager.Taiwu.GetWarehouseAllItemKey();
		for (sbyte b = 0; b < _legendaryBookItems.Length; b++)
		{
			ItemKey itemKey = _legendaryBookItems[b];
			if (itemKey.IsValid() && GetOwner(b) < 0 && !warehouseAllItemKey.Contains(itemKey))
			{
				DomainManager.Taiwu.GetTaiwu().AddInventoryItem(context, itemKey, 1);
			}
		}
	}

	public void UpdateLegendaryBookOwnersStatuses(DataContext context)
	{
		List<int> obj = context.AdvanceMonthRelatedData.CharIdList.Occupy();
		obj.AddRange(_charBookTypes.Keys);
		foreach (int item in obj)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (element_Objects.GetId() == DomainManager.Taiwu.GetTaiwuCharId())
			{
				continue;
			}
			if (element_Objects.GetAgeGroup() != 2)
			{
				LoseAllLegendaryBooks(context, element_Objects, createAdventures: true);
				continue;
			}
			if (element_Objects.GetKidnapperId() >= 0)
			{
				DomainManager.Character.RemoveKidnappedCharacter(context, item, element_Objects.GetKidnapperId(), isEscaped: true);
			}
			UpdateOwnerStatus(context, element_Objects);
		}
		HashSet<int> hashSet = ObjectPool<HashSet<int>>.Instance.Get();
		DomainManager.Extra.GetLegendaryBookConsumedCharacters(hashSet);
		foreach (int item2 in hashSet)
		{
			if (DomainManager.Character.TryGetElement_Objects(item2, out var element))
			{
				element.ActivateAdvanceMonthStatus(7);
			}
		}
		ObjectPool<HashSet<int>>.Instance.Return(hashSet);
		context.AdvanceMonthRelatedData.CharIdList.Release(ref obj);
	}

	private void UpdateOwnerStatus(DataContext context, GameData.Domains.Character.Character character)
	{
		int id = character.GetId();
		byte bossInvasionSpeedType = DomainManager.World.GetBossInvasionSpeedType();
		DomainManager.Extra.TryGetElement_LegendaryBookShockedMonths(id, out var value);
		switch (character.GetLegendaryBookOwnerState())
		{
		case 0:
			UpdateLegendaryBookOwnerGrowth(context, character);
			break;
		case 1:
			if (value >= GlobalConfig.SwordTombAdventureCountDownCoolDown[bossInvasionSpeedType] * 2)
			{
				character.AddFeature(context, 205, removeMutexFeature: true);
				DomainManager.Character.LeaveGroup(context, character, bringWards: false);
				Events.RaiseLegendaryBookOwnerStateChanged(context, character, 2);
			}
			else if (context.Random.CheckPercentProb(50))
			{
				AddActCrazyShockedCharacters(id);
				AdaptableLog.TagInfo("LegendaryBook", $"{character} => 入邪发狂判定通过");
			}
			DomainManager.Extra.SetLegendaryBookShockedMonths(context, id, value + 1);
			if (!DomainManager.Extra.IsCharacterHiddenByLegendaryBook(id))
			{
				break;
			}
			if (!character.GetLocation().IsValid())
			{
				short num = character.GetOrganizationInfo().SettlementId;
				if (num < 0)
				{
					num = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
				}
				Settlement settlement = DomainManager.Organization.GetSettlement(num);
				Location location = settlement.GetLocation();
				character.SetLocation(location, context);
			}
			DomainManager.Extra.RemoveLegendaryBookHiddenChar(context, id);
			DomainManager.Character.UnhideCharacterOnMap(context, character, 16);
			break;
		case 2:
			if (value >= GlobalConfig.SwordTombAdventureCountDownCoolDown[bossInvasionSpeedType] * 3)
			{
				List<sbyte> charOwnedBookTypes = DomainManager.LegendaryBook.GetCharOwnedBookTypes(id);
				foreach (sbyte item in charOwnedBookTypes)
				{
					short legendaryBookConsumedFeature = Config.CombatSkillType.Instance[item].LegendaryBookConsumedFeature;
					character.AddFeature(context, legendaryBookConsumedFeature, removeMutexFeature: true);
				}
				DomainManager.Extra.AddLegendaryBookConsumed(context, id);
				Events.RaiseLegendaryBookOwnerStateChanged(context, character, 3);
				LoseAllLegendaryBooks(context, character, createAdventures: true);
			}
			else
			{
				DomainManager.Extra.SetLegendaryBookShockedMonths(context, id, value + 1);
			}
			break;
		case 3:
			LoseAllLegendaryBooks(context, character, createAdventures: true);
			break;
		}
		if (IsCharacterActingCrazy(character))
		{
			character.ActivateAdvanceMonthStatus(7);
		}
	}

	private void UpdateLegendaryBookOwnerGrowth(DataContext context, GameData.Domains.Character.Character character)
	{
		int id = character.GetId();
		sbyte b = character.GetConsummateLevel();
		sbyte behaviorType = character.GetBehaviorType();
		if (character.GetOrganizationInfo().OrgTemplateId == 16)
		{
			DomainManager.Character.LeaveGroup(context, character, bringWards: false);
			DomainManager.Organization.JoinNearbyVillageTownAsBeggar(context, character, -1);
			if (character.IsCrossAreaTraveling())
			{
				if (!character.GetLocation().IsValid())
				{
					Location validLocation = character.GetValidLocation();
					character.SetLocation(validLocation, context);
				}
				DomainManager.Character.RemoveCrossAreaTravelInfo(context, id);
			}
			DomainManager.Character.HideCharacterOnMap(context, character, 16);
			DomainManager.World.GetMonthlyNotificationCollection().AddVillagerLeftForLegendaryBook(id);
			DomainManager.Extra.AddLegendaryBookHiddenChar(context, id);
		}
		if (b < 18)
		{
			b = (sbyte)Math.Clamp(b + 2, 0, 18);
			character.SetConsummateLevel(b, context);
		}
		if (b >= 18)
		{
			character.AddFeature(context, 204);
			DomainManager.Extra.SetLegendaryBookShockedMonths(context, id, 1);
			Events.RaiseLegendaryBookOwnerStateChanged(context, character, 1);
		}
		int num = b / 2;
		int index = 298 + num;
		CharacterItem characterItem = Config.Character.Instance[index];
		if (character.GetExtraNeili() < characterItem.ExtraNeili)
		{
			character.SetExtraNeili(characterItem.ExtraNeili, context);
		}
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(id);
		List<sbyte> charOwnedBookTypes = DomainManager.LegendaryBook.GetCharOwnedBookTypes(id);
		Span<bool> span = stackalloc bool[14];
		span.Fill(value: false);
		foreach (CombatSkillItem item in (IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance)
		{
			if (item.Grade > num || !charOwnedBookTypes.Contains(item.Type) || item.BookId < 0)
			{
				continue;
			}
			if (!charCombatSkills.TryGetValue(item.TemplateId, out var value))
			{
				byte pageTypes = GameData.Domains.Item.SkillBook.GenerateCombatPageTypes(context.Random, -1, 50);
				value = character.LearnNewCombatSkill(context, item.TemplateId, CombatSkillStateHelper.GenerateReadingStateFromSkillBook(pageTypes));
				span[item.Type] = true;
			}
			if (!CombatSkillStateHelper.IsBrokenOut(value.GetActivationState()))
			{
				byte pageTypes2 = GameData.Domains.Item.SkillBook.GenerateCombatPageTypes(context.Random, -1, 50);
				ushort readingState = (ushort)(value.GetReadingState() | CombatSkillStateHelper.GenerateReadingStateFromSkillBook(pageTypes2));
				value.SetReadingState(readingState, context);
				if (value.CanBreakout())
				{
					ushort activationState = CombatSkillStateHelper.GenerateRandomActivatedNormalPages(context.Random, readingState, 0);
					activationState = CombatSkillStateHelper.GenerateRandomActivatedOutlinePage(context.Random, readingState, activationState, behaviorType);
					sbyte skillBreakoutAvailableStepsCount = character.GetSkillBreakoutAvailableStepsCount(item.TemplateId);
					value.SetActivationState(activationState, context);
					value.SetBreakoutStepsCount(skillBreakoutAvailableStepsCount, context);
					span[item.Type] = true;
				}
			}
		}
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = character.GetLocation();
		foreach (sbyte item2 in charOwnedBookTypes)
		{
			if (span[item2])
			{
				ItemKey legendaryBookItem = GetLegendaryBookItem(item2);
				lifeRecordCollection.AddBoostedByLegendaryBooks(id, currDate, location, legendaryBookItem.ItemType, legendaryBookItem.TemplateId);
			}
		}
	}

	public void UpdateLegendaryBookOwnersActions(DataContext context)
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.AddRange(_charBookTypes.Keys);
		foreach (int item in list)
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element) && item != DomainManager.Taiwu.GetTaiwuCharId() && IsCharacterActingCrazy(element))
			{
				UpdateOwnerAction(context, element);
			}
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}

	private void UpdateOwnerAction(DataContext context, GameData.Domains.Character.Character character)
	{
		Location location = character.GetLocation();
		if (location.IsValid() && !character.IsActiveExternalRelationState(60))
		{
			GameData.Domains.Character.Character character2 = SelectHarmActionTarget(context, character, location);
			if (character2 != null)
			{
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				int currDate = DomainManager.World.GetCurrDate();
				lifeRecordCollection.AddActCrazy(character.GetId(), currDate, location);
				DomainManager.Character.HandleAttackAction(context, character, character2);
			}
		}
	}

	private GameData.Domains.Character.Character SelectHarmActionTarget(DataContext context, GameData.Domains.Character.Character character, Location location)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int selfCharId = character.GetId();
		if (location.Equals(taiwu.GetLocation()))
		{
			return taiwu;
		}
		MapBlockData block = DomainManager.Map.GetBlock(location);
		if (block.CharacterSet == null || block.CharacterSet.Count == 1)
		{
			return null;
		}
		List<int> obj = context.AdvanceMonthRelatedData.TargetCharIdList.Occupy();
		obj.AddRange(block.CharacterSet);
		obj.RemoveAll((int charId) => DomainManager.Character.GetElement_Objects(charId).GetAgeGroup() == 0 || charId == selfCharId);
		int randomOrDefault = obj.GetRandomOrDefault(context.Random, -1);
		context.AdvanceMonthRelatedData.TargetCharIdList.Release(ref obj);
		if (randomOrDefault < 0)
		{
			return null;
		}
		return DomainManager.Character.GetElement_Objects(randomOrDefault);
	}

	public void UpgradeEnemyNestsByLegendaryBookOwner(DataContext context, short areaId, int upgradeCount)
	{
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		monthlyNotificationCollection.AddRandomEnemyGrow(new Location(areaId, -1));
	}

	public void RegisterOwner(DataContext context, GameData.Domains.Character.Character character, sbyte bookType)
	{
		AdaptableLog.TagInfo("LegendaryBook", $"{character} => 得到奇书 {Config.Misc.Instance[211 + bookType].Name}");
		int num = _bookOwners[bookType];
		if (num >= 0)
		{
			throw new Exception($"Book {bookType} already has owner: {num}");
		}
		int id = character.GetId();
		SetElement_BookOwners(bookType, id, context);
		RegisterCharBookType(id, bookType);
		character.TryRetireTreasuryGuard(context);
		if (id == DomainManager.Taiwu.GetTaiwuCharId())
		{
			DomainManager.World.SetWorldFunctionsStatus(context, 21);
		}
		else if (DomainManager.Taiwu.GetLegacyPassingState() != 4)
		{
			short legendaryBookFeature = Config.CombatSkillType.Instance[bookType].LegendaryBookFeature;
			character.AddFeature(context, legendaryBookFeature);
		}
	}

	public void UnregisterOwner(DataContext context, GameData.Domains.Character.Character character, sbyte bookType)
	{
		AdaptableLog.TagInfo("LegendaryBook", $"{character} => 失去奇书 {Config.Misc.Instance[211 + bookType].Name}");
		int num = _bookOwners[bookType];
		int id = character.GetId();
		if (num < 0)
		{
			throw new Exception($"Book {bookType} does not have owner");
		}
		if (num != id)
		{
			throw new Exception($"Wrong owner of book {bookType}: {num} - {id}");
		}
		List<short> featureIds = character.GetFeatureIds();
		short legendaryBookFeature = Config.CombatSkillType.Instance[bookType].LegendaryBookFeature;
		bool flag = DomainManager.Extra.IsLegendaryBookConsumed(id);
		if (flag || featureIds.Contains(205))
		{
			DomainManager.Character.UnregisterFeatureForAllXiangshuAvatars(context, legendaryBookFeature);
			DomainManager.Extra.RegisterPrevLegendaryBookOwner(context, character, bookType);
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			sbyte advancingMonthState = DomainManager.World.GetAdvancingMonthState();
			if (advancingMonthState > 0 && advancingMonthState < 20)
			{
				ItemKey legendaryBookItem = DomainManager.LegendaryBook.GetLegendaryBookItem(bookType);
				monthlyEventCollection.AddSwordTombBackToNormal((ulong)legendaryBookItem);
			}
		}
		SetElement_BookOwners(bookType, -1, context);
		UnregisterCharBookType(id, bookType);
		DomainManager.Extra.RemoveContestForLegendaryBookCharacters(context, bookType);
		DomainManager.Extra.AddPreviousLegendaryBookOwner(context, id, bookType);
		if (DomainManager.Character.IsCharacterAlive(id))
		{
			character.RemoveFeature(context, legendaryBookFeature);
			if (!(_charBookTypes.ContainsKey(id) || flag))
			{
				DomainManager.Extra.RemoveLegendaryBookShockedMonths(context, id);
				character.RemoveFeature(context, 204);
				character.RemoveFeature(context, 205);
			}
		}
	}

	public int GetOwner(sbyte bookType)
	{
		return _bookOwners[bookType];
	}

	public sbyte GetConsumedCharacterLegendaryBookType(GameData.Domains.Character.Character character)
	{
		short legendaryBookConsumedFeature = Config.CombatSkillType.Instance[(sbyte)0].LegendaryBookConsumedFeature;
		short legendaryBookConsumedFeature2 = Config.CombatSkillType.Instance[(sbyte)13].LegendaryBookConsumedFeature;
		foreach (short featureId in character.GetFeatureIds())
		{
			if (featureId >= legendaryBookConsumedFeature && featureId <= legendaryBookConsumedFeature2)
			{
				return (sbyte)(featureId - legendaryBookConsumedFeature);
			}
		}
		return -1;
	}

	public void UpdateBossCharacterLegendaryBookFeatures(DataContext context, GameData.Domains.Character.Character character)
	{
		sbyte xiangshuType = character.GetXiangshuType();
		switch (xiangshuType)
		{
		case 1:
		{
			foreach (CombatSkillTypeItem item in (IEnumerable<CombatSkillTypeItem>)Config.CombatSkillType.Instance)
			{
				character.RemoveFeatureGroup(context, item.LegendaryBookFeature);
			}
			for (sbyte b = 0; b < 14; b++)
			{
				int num = _bookOwners[b];
				if (num >= 0)
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num);
					if (element_Objects.GetLegendaryBookOwnerState() == 2)
					{
						short legendaryBookFeature2 = Config.CombatSkillType.Instance[b].LegendaryBookFeature;
						character.AddFeature(context, legendaryBookFeature2);
					}
				}
			}
			break;
		}
		default:
			if (character.GetTemplateId() != 443)
			{
				if (xiangshuType != 4)
				{
					break;
				}
				foreach (CombatSkillTypeItem item2 in (IEnumerable<CombatSkillTypeItem>)Config.CombatSkillType.Instance)
				{
					character.RemoveFeatureGroup(context, item2.LegendaryBookFeature);
				}
				List<short> woodenXiangshuAvatarSelectedFeatures = DomainManager.Extra.GetWoodenXiangshuAvatarSelectedFeatures();
				{
					foreach (short item3 in woodenXiangshuAvatarSelectedFeatures)
					{
						character.AddFeature(context, item3);
					}
					break;
				}
			}
			goto case 2;
		case 2:
		{
			foreach (CombatSkillTypeItem item4 in (IEnumerable<CombatSkillTypeItem>)Config.CombatSkillType.Instance)
			{
				character.RemoveFeatureGroup(context, item4.LegendaryBookFeature);
			}
			HashSet<int> hashSet = ObjectPool<HashSet<int>>.Instance.Get();
			DomainManager.Extra.GetLegendaryBookConsumedCharacters(hashSet);
			foreach (int item5 in hashSet)
			{
				if (!DomainManager.Character.TryGetElement_Objects(item5, out var element))
				{
					continue;
				}
				short legendaryBookConsumedFeature = Config.CombatSkillType.Instance[(sbyte)0].LegendaryBookConsumedFeature;
				short legendaryBookConsumedFeature2 = Config.CombatSkillType.Instance[(sbyte)13].LegendaryBookConsumedFeature;
				foreach (short featureId in element.GetFeatureIds())
				{
					if (featureId >= legendaryBookConsumedFeature && featureId <= legendaryBookConsumedFeature2)
					{
						int index = featureId - legendaryBookConsumedFeature;
						short legendaryBookFeature = Config.CombatSkillType.Instance[index].LegendaryBookFeature;
						character.AddFeature(context, legendaryBookFeature);
					}
				}
			}
			ObjectPool<HashSet<int>>.Instance.Return(hashSet);
			break;
		}
		}
	}

	public sbyte GetCharacterLegendaryBookOwnerState(int charId)
	{
		if (DomainManager.Extra.IsLegendaryBookConsumed(charId))
		{
			return 3;
		}
		if (DomainManager.LegendaryBook.GetCharOwnedBookTypes(charId) == null || !DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			return -1;
		}
		List<short> featureIds = element.GetFeatureIds();
		if (featureIds.Contains(204))
		{
			return 1;
		}
		if (featureIds.Contains(205))
		{
			return 2;
		}
		return 0;
	}

	public sbyte GetLegendaryBookAppearType(int prevOwnerId)
	{
		if (prevOwnerId < 0)
		{
			return 0;
		}
		if (!DomainManager.Character.IsCharacterAlive(prevOwnerId))
		{
			return 2;
		}
		if (DomainManager.Extra.IsLegendaryBookConsumed(prevOwnerId))
		{
			return 1;
		}
		return 3;
	}

	public List<sbyte> GetCharOwnedBookTypes(int charId)
	{
		List<sbyte> value;
		return _charBookTypes.TryGetValue(charId, out value) ? value : null;
	}

	public void OnCharacterDead(DataContext context, GameData.Domains.Character.Character character)
	{
		int id = character.GetId();
		DomainManager.Extra.RemoveLegendaryBookConsumed(context, id);
		DomainManager.Extra.RemoveLegendaryBookShockedMonths(context, id);
		DomainManager.Extra.ApplyRanshanThreeCorpsesLegendaryBookActionTargetDeadResult(context, id);
		LoseAllLegendaryBooks(context, character, createAdventures: true);
	}

	public bool LoseAllLegendaryBooks(DataContext context, GameData.Domains.Character.Character character, bool createAdventures)
	{
		int id = character.GetId();
		List<sbyte> charOwnedBookTypes = GetCharOwnedBookTypes(id);
		if (charOwnedBookTypes == null)
		{
			return false;
		}
		Dictionary<ItemKey, int> items = character.GetInventory().Items;
		List<ItemKey> obj = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
		foreach (ItemKey key in items.Keys)
		{
			if (ItemTemplateHelper.GetItemSubType(key.ItemType, key.TemplateId) == 1202)
			{
				obj.Add(key);
			}
		}
		sbyte legendaryBookAppearType = GetLegendaryBookAppearType(id);
		Inventory inventory = character.GetInventory();
		Location location = character.GetLocation();
		short num = location.AreaId;
		if (num < 0)
		{
			num = (short)context.Random.Next(45);
		}
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		monthlyNotificationCollection.AddRandomEnemyDecay(location);
		foreach (ItemKey item in obj)
		{
			sbyte bookType = (sbyte)(item.TemplateId - 211);
			if (legendaryBookAppearType != 2)
			{
				character.RemoveInventoryItem(context, item, 1, deleteItem: false);
			}
			else
			{
				inventory.OfflineRemove(item, 1);
				UnregisterOwner(context, character, bookType);
				DomainManager.Item.GetBaseItem(item).ResetOwner();
			}
			monthlyNotificationCollection.AddLegendaryBookLost(id, location, item.ItemType, item.TemplateId);
			DomainManager.Item.SetOwner(item, ItemOwnerType.System, 11);
			if (createAdventures)
			{
				MapBlockData randomMapBlockDataInAreaByFilters = DomainManager.Map.GetRandomMapBlockDataInAreaByFilters(context.Random, num, null, includeBlocksWithAdventure: false);
				LegendaryBookMonthlyAction action = new LegendaryBookMonthlyAction
				{
					Location = (randomMapBlockDataInAreaByFilters?.GetLocation() ?? Location.Invalid),
					BookType = bookType,
					BookAppearType = legendaryBookAppearType,
					PrevOwnerId = id
				};
				DomainManager.TaiwuEvent.AddTempDynamicAction(context, action);
			}
		}
		context.AdvanceMonthRelatedData.ItemKeys.Release(ref obj);
		return true;
	}

	public void LoseTargetLegendaryBook(DataContext context, GameData.Domains.Character.Character character, bool createAdventures, ItemKey itemKey)
	{
		int id = character.GetId();
		sbyte legendaryBookAppearType = GetLegendaryBookAppearType(id);
		Inventory inventory = character.GetInventory();
		Location location = character.GetLocation();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		sbyte bookType = (sbyte)(itemKey.TemplateId - 211);
		short num = location.AreaId;
		if (num < 0)
		{
			num = (short)context.Random.Next(45);
		}
		if (legendaryBookAppearType != 2)
		{
			character.RemoveInventoryItem(context, itemKey, 1, deleteItem: false);
		}
		else
		{
			inventory.OfflineRemove(itemKey, 1);
			UnregisterOwner(context, character, bookType);
			DomainManager.Item.GetBaseItem(itemKey).ResetOwner();
		}
		monthlyNotificationCollection.AddLegendaryBookLost(id, location, itemKey.ItemType, itemKey.TemplateId);
		DomainManager.Item.SetOwner(itemKey, ItemOwnerType.System, 11);
		if (createAdventures)
		{
			MapBlockData randomMapBlockDataInAreaByFilters = DomainManager.Map.GetRandomMapBlockDataInAreaByFilters(context.Random, num, null, includeBlocksWithAdventure: false);
			LegendaryBookMonthlyAction action = new LegendaryBookMonthlyAction
			{
				Location = (randomMapBlockDataInAreaByFilters?.GetLocation() ?? Location.Invalid),
				BookType = bookType,
				BookAppearType = legendaryBookAppearType,
				PrevOwnerId = id
			};
			DomainManager.TaiwuEvent.AddTempDynamicAction(context, action);
		}
	}

	public LegendaryBookCharacterRelatedData GetLegendaryBookCharacterRelatedData(int charId, sbyte bookType = -1)
	{
		if (!GameData.Domains.Character.Character.IsCharacterIdValid(charId) || !DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			return null;
		}
		Location location = element.GetLocation();
		FullBlockName blockFullName = DomainManager.Map.GetBlockFullName(location);
		if (location != Location.Invalid)
		{
			MapBlockData mapBlockData = DomainManager.Map.GetBlock(element.GetValidLocation());
			if (mapBlockData.RootBlockId > -1)
			{
				mapBlockData = DomainManager.Map.GetBlockData(mapBlockData.AreaId, mapBlockData.RootBlockId);
			}
			blockFullName = DomainManager.Map.GetBlockFullName(mapBlockData.GetLocation());
		}
		LegendaryBookCharacterRelatedData legendaryBookCharacterRelatedData = new LegendaryBookCharacterRelatedData
		{
			Id = charId,
			CurrAge = element.GetCurrAge(),
			Gender = element.GetGender(),
			FeatureId = -1,
			ConsummateLevel = element.GetConsummateLevel(),
			Charm = element.GetAttraction(),
			BehaviorType = element.GetBehaviorType(),
			HappinessType = element.GetHappinessType(),
			Favorability = DomainManager.Character.GetFavorability(charId, DomainManager.Taiwu.GetTaiwuCharId()),
			FameType = element.GetFameType(),
			HealthType = DomainManager.Character.GetHealthType(charId),
			BookType = bookType,
			Location = location,
			AvatarRelatedData = DomainManager.Character.GetAvatarRelatedData(charId),
			NameRelatedData = DomainManager.Character.GetNameRelatedData(charId),
			OrganizationInfo = element.GetOrganizationInfo(),
			FullBlockName = blockFullName
		};
		List<short> featureIds = element.GetFeatureIds();
		if (featureIds.Contains(204))
		{
			legendaryBookCharacterRelatedData.FeatureId = 204;
		}
		else if (featureIds.Contains(205))
		{
			legendaryBookCharacterRelatedData.FeatureId = 205;
		}
		return legendaryBookCharacterRelatedData;
	}

	[DomainMethod]
	public LegendaryBookIncrementData GetLegendaryBookIncrementData()
	{
		LegendaryBookIncrementData legendaryBookIncrementData = new LegendaryBookIncrementData();
		for (sbyte b = 0; b < 14; b++)
		{
			LegendaryBookMonthlyAction legendaryBookMonthlyAction = _legendaryBookMonthlyActions[b];
			if (legendaryBookMonthlyAction != null)
			{
				if (legendaryBookMonthlyAction.State == 5)
				{
					MapBlockData mapBlockData = DomainManager.Map.GetBlock(legendaryBookMonthlyAction.Location);
					FullBlockName blockFullName = DomainManager.Map.GetBlockFullName(mapBlockData.GetLocation());
					if (mapBlockData.RootBlockId > -1)
					{
						mapBlockData = DomainManager.Map.GetBlockData(mapBlockData.AreaId, mapBlockData.RootBlockId);
					}
					legendaryBookIncrementData.BookLocationMap.Add(b, legendaryBookMonthlyAction.Location);
					legendaryBookIncrementData.BookDurationMap.Add(b, DomainManager.Adventure.GetAdventureSite(legendaryBookMonthlyAction.Location.AreaId, legendaryBookMonthlyAction.Location.BlockId).RemainingMonths);
					legendaryBookIncrementData.BlockDataMap.TryAdd(b, mapBlockData);
					legendaryBookIncrementData.BlockNameDataMap.TryAdd(b, blockFullName);
				}
				else
				{
					legendaryBookIncrementData.BookDurationMap.Add(b, legendaryBookMonthlyAction.ActivateDelay - legendaryBookMonthlyAction.Month + 1);
				}
			}
			else
			{
				int owner = GetOwner(b);
				LegendaryBookCharacterRelatedData legendaryBookCharacterRelatedData = GetLegendaryBookCharacterRelatedData(owner, b);
				if (legendaryBookCharacterRelatedData != null)
				{
					Location location = DomainManager.Character.GetElement_Objects(owner).GetLocation();
					if (location.IsValid())
					{
						MapBlockData mapBlockData2 = DomainManager.Map.GetBlock(location);
						FullBlockName blockFullName2 = DomainManager.Map.GetBlockFullName(mapBlockData2.GetLocation());
						if (mapBlockData2.RootBlockId > -1)
						{
							mapBlockData2 = DomainManager.Map.GetBlockData(mapBlockData2.AreaId, mapBlockData2.RootBlockId);
						}
						legendaryBookIncrementData.BlockDataMap.TryAdd(b, mapBlockData2);
						legendaryBookIncrementData.BlockNameDataMap.TryAdd(b, blockFullName2);
					}
					legendaryBookIncrementData.OwnerMap.Add(b, owner);
					legendaryBookIncrementData.CharacterMap.TryAdd(owner, legendaryBookCharacterRelatedData);
				}
				foreach (int item in DomainManager.Extra.GetContestForLegendaryBookCharacterSet(b).GetCollection())
				{
					legendaryBookCharacterRelatedData = GetLegendaryBookCharacterRelatedData(item, b);
					if (legendaryBookCharacterRelatedData != null)
					{
						legendaryBookIncrementData.ContestList.Add(item);
						legendaryBookIncrementData.CharacterMap.TryAdd(item, legendaryBookCharacterRelatedData);
					}
				}
			}
		}
		foreach (LegendaryBookCharacterRelatedData value in legendaryBookIncrementData.CharacterMap.Values)
		{
			if (value.FeatureId == 204)
			{
				legendaryBookIncrementData.ShockedList.Add(value.Id);
			}
			else if (value.FeatureId == 205)
			{
				legendaryBookIncrementData.InsaneList.Add(value.Id);
			}
		}
		HashSet<int> hashSet = new HashSet<int>();
		DomainManager.Extra.GetLegendaryBookConsumedCharacters(hashSet);
		foreach (int item2 in hashSet)
		{
			LegendaryBookCharacterRelatedData legendaryBookCharacterRelatedData2 = GetLegendaryBookCharacterRelatedData(item2, -1);
			if (legendaryBookCharacterRelatedData2 != null)
			{
				legendaryBookIncrementData.ConsumedList.Add(item2);
				legendaryBookIncrementData.CharacterMap.TryAdd(item2, legendaryBookCharacterRelatedData2);
			}
		}
		foreach (LegendaryBookCharacterRelatedData value2 in legendaryBookIncrementData.CharacterMap.Values)
		{
			if (value2.FeatureId == 204)
			{
				continue;
			}
			List<short> featureIds = DomainManager.Character.GetElement_Objects(value2.Id).GetFeatureIds();
			for (sbyte b2 = 0; b2 < 14; b2++)
			{
				CombatSkillTypeItem combatSkillTypeItem = Config.CombatSkillType.Instance[b2];
				if (featureIds.Contains(combatSkillTypeItem.LegendaryBookFeature))
				{
					value2.FeatureId = combatSkillTypeItem.LegendaryBookFeature;
					break;
				}
				if (featureIds.Contains(combatSkillTypeItem.LegendaryBookConsumedFeature))
				{
					value2.FeatureId = combatSkillTypeItem.LegendaryBookConsumedFeature;
					break;
				}
			}
		}
		return legendaryBookIncrementData;
	}

	[DomainMethod]
	public List<IntPair> GmCmd_GetAllLegendaryBookStates()
	{
		List<IntPair> list = new List<IntPair>();
		for (sbyte b = 0; b < 14; b++)
		{
			list.Add(new IntPair(_bookOwners[b], -1));
		}
		for (short num = 0; num < 135; num++)
		{
			foreach (KeyValuePair<short, AdventureSiteData> adventureSite in DomainManager.Adventure.GetElement_AdventureAreas(num).AdventureSites)
			{
				if (Config.Adventure.Instance[adventureSite.Value.TemplateId].Type == 17)
				{
					list[GetBookTypeByAdventureTemplateId(adventureSite.Value.TemplateId)] = new IntPair(-1, num);
				}
			}
		}
		return list;
	}

	[DomainMethod]
	public int GetAllLegendaryBooksOwningState()
	{
		int num = 0;
		for (sbyte b = 0; b < 14; b++)
		{
			if (GetOwner(b) >= 0 || DomainManager.Extra.TryGetElement_PrevLegendaryBookOwnerCopies(b, out var _) || DomainManager.Extra.IsBookOwnedByTaiwu(b) || DomainManager.Extra.IsLegendaryBookOwned(b))
			{
				num |= 1 << (int)b;
			}
		}
		return num;
	}

	[DomainMethod]
	public void GmCmd_GiveAllTaiwuLegendaryBookToRandomNpc(DataContext context)
	{
		List<GameData.Domains.Character.Character> list = new List<GameData.Domains.Character.Character>();
		DomainManager.Character.FindIntelligentCharacters((GameData.Domains.Character.Character _) => true, list);
		for (sbyte b = 0; b < 14; b++)
		{
			int index = context.Random.Next(0, list.Count);
			GameData.Domains.Character.Character destChar = list[index];
			int owner = GetOwner(b);
			if (owner > 0)
			{
				DomainManager.Character.TransferInventoryItem(context, DomainManager.Character.GetElement_Objects(owner), destChar, GetLegendaryBookItem(b), 1);
			}
		}
	}

	public bool IsCharacterLegendaryBookOwnerOrContest(int charId)
	{
		if (_charBookTypes.ContainsKey(charId))
		{
			return true;
		}
		for (sbyte b = 0; b < 14; b++)
		{
			if (DomainManager.Extra.GetContestForLegendaryBookCharacterSet(b).Contains(charId))
			{
				return true;
			}
		}
		return false;
	}

	private void InitializeCharBookTypes()
	{
		_charBookTypes = new Dictionary<int, List<sbyte>>();
		for (sbyte b = 0; b < 14; b++)
		{
			int num = _bookOwners[b];
			if (num >= 0)
			{
				RegisterCharBookType(num, b);
			}
		}
	}

	private void RegisterCharBookType(int charId, sbyte bookType)
	{
		if (!_charBookTypes.TryGetValue(charId, out var value))
		{
			value = new List<sbyte>();
			_charBookTypes.Add(charId, value);
		}
		value.Add(bookType);
	}

	private void UnregisterCharBookType(int charId, sbyte bookType)
	{
		if (_charBookTypes.TryGetValue(charId, out var value))
		{
			value.Remove(bookType);
			if (value.Count <= 0)
			{
				_charBookTypes.Remove(charId);
			}
		}
	}

	private sbyte GetBookTypeByAdventureTemplateId(short templateId)
	{
		if (1 == 0)
		{
		}
		sbyte result = templateId switch
		{
			145 => 5, 
			146 => 1, 
			147 => 2, 
			148 => 6, 
			149 => 0, 
			150 => 11, 
			151 => 13, 
			152 => 8, 
			153 => 12, 
			154 => 7, 
			155 => 10, 
			156 => 3, 
			157 => 4, 
			158 => 9, 
			_ => -1, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private void InitializeLegendaryBookItems()
	{
		for (sbyte b = 0; b < 14; b++)
		{
			_legendaryBookItems[b] = ItemKey.Invalid;
		}
	}

	public ItemKey GetLegendaryBookItem(sbyte combatSkillType)
	{
		return _legendaryBookItems[combatSkillType];
	}

	internal void RegisterLegendaryBookItem(ItemKey itemKey)
	{
		Tester.Assert(ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == 1202, $"Target item {itemKey} is not a valid legendary book.");
		int num = itemKey.TemplateId - 211;
		Tester.Assert(!_legendaryBookItems[num].IsValid(), $"Legendary book {itemKey} of the same type already exist {_legendaryBookItems[num]}");
		_legendaryBookItems[num] = itemKey;
	}

	internal void UnregisterLegendaryBookItem(ItemKey itemKey)
	{
		Tester.Assert(ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == 1202, $"Target item {itemKey} is not a valid legendary book.");
		int num = itemKey.TemplateId - 211;
		_legendaryBookItems[num] = ItemKey.Invalid;
	}

	public bool IsAnyLegendaryBookOwned()
	{
		return _charBookTypes.Count > 0;
	}

	public LegendaryBookDomain()
		: base(2)
	{
		_bookOwners = new int[14];
		_legendaryBookOwnerData = new LegendaryBookOwnerData();
		OnInitializedDomainData();
	}

	public int GetElement_BookOwners(int index)
	{
		return _bookOwners[index];
	}

	public unsafe void SetElement_BookOwners(int index, int value, DataContext context)
	{
		_bookOwners[index] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, _dataStatesBookOwners, CacheInfluencesBookOwners, context);
		byte* ptr = OperationAdder.FixedElementList_Set(11, 0, index, 4);
		*(int*)ptr = value;
		ptr += 4;
	}

	public LegendaryBookOwnerData GetLegendaryBookOwnerData()
	{
		return _legendaryBookOwnerData;
	}

	public void SetLegendaryBookOwnerData(LegendaryBookOwnerData value, DataContext context)
	{
		_legendaryBookOwnerData = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public unsafe override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
		byte* ptr = OperationAdder.FixedElementList_InsertRange(11, 0, 0, 14, 56);
		for (int i = 0; i < 14; i++)
		{
			((int*)ptr)[i] = _bookOwners[i];
		}
		ptr += 56;
	}

	public override void OnLoadWorld()
	{
		_pendingLoadingOperationIds = new Queue<uint>();
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedElementList_GetAll(11, 0));
	}

	public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
	{
		switch (dataId)
		{
		case 0:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(_dataStatesBookOwners, (int)subId0);
			}
			return GameData.Serializer.Serializer.Serialize(_bookOwners[(uint)subId0], dataPool);
		case 1:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
			}
			return GameData.Serializer.Serializer.Serialize(_legendaryBookOwnerData, dataPool);
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		switch (dataId)
		{
		case 0:
		{
			int item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			_bookOwners[(uint)subId0] = item;
			SetElement_BookOwners((int)subId0, item, context);
			break;
		}
		case 1:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _legendaryBookOwnerData);
			SetLegendaryBookOwnerData(_legendaryBookOwnerData, context);
			break;
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
	{
		int argsOffset = operation.ArgsOffset;
		switch (operation.MethodId)
		{
		case 0:
			if (operation.ArgsCount == 0)
			{
				List<IntPair> item = GmCmd_GetAllLegendaryBookStates();
				return GameData.Serializer.Serializer.Serialize(item, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 1:
			if (operation.ArgsCount == 0)
			{
				GmCmd_GiveAllTaiwuLegendaryBookToRandomNpc(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 2:
			if (operation.ArgsCount == 0)
			{
				LegendaryBookIncrementData legendaryBookIncrementData = GetLegendaryBookIncrementData();
				return GameData.Serializer.Serializer.Serialize(legendaryBookIncrementData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 3:
			if (operation.ArgsCount == 0)
			{
				int allLegendaryBooksOwningState = GetAllLegendaryBooksOwningState();
				return GameData.Serializer.Serializer.Serialize(allLegendaryBooksOwningState, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
	{
		ushort num = dataId;
		ushort num2 = num;
		if (num2 == 0 || num2 == 1)
		{
			return;
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
	{
		switch (dataId)
		{
		case 0:
			if (!BaseGameDataDomain.IsModified(_dataStatesBookOwners, (int)subId0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(_dataStatesBookOwners, (int)subId0);
			return GameData.Serializer.Serializer.Serialize(_bookOwners[(uint)subId0], dataPool);
		case 1:
			if (!BaseGameDataDomain.IsModified(DataStates, 1))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 1);
			return GameData.Serializer.Serializer.Serialize(_legendaryBookOwnerData, dataPool);
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		switch (dataId)
		{
		case 0:
			if (BaseGameDataDomain.IsModified(_dataStatesBookOwners, (int)subId0))
			{
				BaseGameDataDomain.ResetModified(_dataStatesBookOwners, (int)subId0);
			}
			break;
		case 1:
			if (BaseGameDataDomain.IsModified(DataStates, 1))
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
			}
			break;
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		return dataId switch
		{
			0 => BaseGameDataDomain.IsModified(_dataStatesBookOwners, (int)subId0), 
			1 => BaseGameDataDomain.IsModified(DataStates, 1), 
			_ => throw new Exception($"Unsupported dataId {dataId}"), 
		};
	}

	public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
	{
		ushort dataId = influence.TargetIndicator.DataId;
		ushort num = dataId;
		if (num != 0 && num != 1)
		{
			throw new Exception($"Unsupported dataId {influence.TargetIndicator.DataId}");
		}
		throw new Exception($"Cannot invalidate cache state of non-cache data {influence.TargetIndicator.DataId}");
	}

	public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		switch (operation.DataId)
		{
		case 0:
		{
			ResponseProcessor.ProcessElementList_BasicType_Fixed_Value(operation, pResult, _bookOwners, 14, 4);
			if (_pendingLoadingOperationIds == null)
			{
				break;
			}
			uint num = _pendingLoadingOperationIds.Peek();
			if (num == operation.Id)
			{
				_pendingLoadingOperationIds.Dequeue();
				if (_pendingLoadingOperationIds.Count <= 0)
				{
					_pendingLoadingOperationIds = null;
					InitializeInternalDataOfCollections();
					OnLoadedArchiveData();
					DomainManager.Global.CompleteLoading(11);
				}
			}
			break;
		}
		default:
			throw new Exception($"Unsupported dataId {operation.DataId}");
		case 1:
			throw new Exception($"Cannot process archive response of non-archive data {operation.DataId}");
		}
	}

	private void InitializeInternalDataOfCollections()
	{
	}
}
