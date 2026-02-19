using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Config;
using Config.ConfigCells.Character;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DLC;
using GameData.DLC.FiveLoong;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Global;
using GameData.Domains.Item.Display;
using GameData.Domains.Map;
using GameData.Domains.Taiwu;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.World;
using GameData.Domains.World.Notification;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using Redzen.Random;

namespace GameData.Domains.Item;

[GameDataDomain(6)]
public class ItemDomain : BaseGameDataDomain
{
	private static readonly int WagerValueUnit = GlobalConfig.UnitsOfResourceTransfer[0] * GlobalConfig.ResourcesWorth[0];

	private sbyte _minCricketGrade = 0;

	private sbyte _maxCricketGrade = 8;

	private bool _onlyNoInjuryCricket;

	private int _cricketBattleEnemyId;

	private readonly List<ItemKey> _cricketBattleEnemyCrickets = new List<ItemKey>();

	private Wager _cricketBattleSelfWager;

	private Wager _cricketBattleEnemyWager;

	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<int, Weapon> _weapons;

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<int, Armor> _armors;

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<int, Accessory> _accessories;

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<int, Clothing> _clothing;

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<int, Carrier> _carriers;

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<int, Material> _materials;

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<int, CraftTool> _craftTools;

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<int, Food> _foods;

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<int, Medicine> _medicines;

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<int, TeaWine> _teaWines;

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<int, SkillBook> _skillBooks;

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<int, Cricket> _crickets;

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
	private readonly Dictionary<int, Misc> _misc;

	[DomainData(DomainDataType.SingleValue, true, false, false, false)]
	private int _nextItemId;

	[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
	private readonly Dictionary<TemplateKey, int> _stackableItems;

	[Obsolete("Now only for data fix. Use ExtraDomain.PoisonEffects instead")]
	[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
	private readonly Dictionary<int, PoisonEffects> _poisonItems;

	[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
	private readonly Dictionary<int, RefiningEffects> _refinedItems;

	[DomainData(DomainDataType.SingleValueCollection, false, false, false, false)]
	private readonly Dictionary<int, GameData.Utilities.ShortList> _externEquipmentEffects;

	private readonly HashSet<ItemKey> _trackedSpecialItems = new HashSet<ItemKey>();

	[DomainData(DomainDataType.SingleValue, true, false, false, false)]
	private ItemKey _emptyHandKey;

	[DomainData(DomainDataType.SingleValue, true, false, false, false)]
	private ItemKey _branchKey;

	[DomainData(DomainDataType.SingleValue, true, false, false, false)]
	private ItemKey _stoneKey;

	private static List<short>[] _categorizedEquipmentEffects;

	private static Dictionary<short, List<short>>[] _categorizedItemTemplates;

	private static List<TemplateKey>[] _skillBreakPlateBonusEffects;

	public const int ItemGradeSatisfactionOffset = -2;

	public const int NormalPageBaseCostExp = 20;

	public const int OutlinePageBaseCostExp = 60;

	private readonly HashSet<ItemKey> _newDeadCrickets = new HashSet<ItemKey>();

	private static short[] _wugTemplateIds;

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[21][];

	private static readonly DataInfluence[][] CacheInfluencesWeapons = new DataInfluence[13][];

	private readonly ObjectCollectionDataStates _dataStatesWeapons = new ObjectCollectionDataStates(13, 0);

	public readonly ObjectCollectionHelperData HelperDataWeapons;

	private static readonly DataInfluence[][] CacheInfluencesArmors = new DataInfluence[13][];

	private readonly ObjectCollectionDataStates _dataStatesArmors = new ObjectCollectionDataStates(13, 0);

	public readonly ObjectCollectionHelperData HelperDataArmors;

	private static readonly DataInfluence[][] CacheInfluencesAccessories = new DataInfluence[8][];

	private readonly ObjectCollectionDataStates _dataStatesAccessories = new ObjectCollectionDataStates(8, 0);

	public readonly ObjectCollectionHelperData HelperDataAccessories;

	private static readonly DataInfluence[][] CacheInfluencesClothing = new DataInfluence[9][];

	private readonly ObjectCollectionDataStates _dataStatesClothing = new ObjectCollectionDataStates(9, 0);

	public readonly ObjectCollectionHelperData HelperDataClothing;

	private static readonly DataInfluence[][] CacheInfluencesCarriers = new DataInfluence[8][];

	private readonly ObjectCollectionDataStates _dataStatesCarriers = new ObjectCollectionDataStates(8, 0);

	public readonly ObjectCollectionHelperData HelperDataCarriers;

	private static readonly DataInfluence[][] CacheInfluencesMaterials = new DataInfluence[5][];

	private readonly ObjectCollectionDataStates _dataStatesMaterials = new ObjectCollectionDataStates(5, 0);

	public readonly ObjectCollectionHelperData HelperDataMaterials;

	private static readonly DataInfluence[][] CacheInfluencesCraftTools = new DataInfluence[5][];

	private readonly ObjectCollectionDataStates _dataStatesCraftTools = new ObjectCollectionDataStates(5, 0);

	public readonly ObjectCollectionHelperData HelperDataCraftTools;

	private static readonly DataInfluence[][] CacheInfluencesFoods = new DataInfluence[5][];

	private readonly ObjectCollectionDataStates _dataStatesFoods = new ObjectCollectionDataStates(5, 0);

	public readonly ObjectCollectionHelperData HelperDataFoods;

	private static readonly DataInfluence[][] CacheInfluencesMedicines = new DataInfluence[5][];

	private readonly ObjectCollectionDataStates _dataStatesMedicines = new ObjectCollectionDataStates(5, 0);

	public readonly ObjectCollectionHelperData HelperDataMedicines;

	private static readonly DataInfluence[][] CacheInfluencesTeaWines = new DataInfluence[5][];

	private readonly ObjectCollectionDataStates _dataStatesTeaWines = new ObjectCollectionDataStates(5, 0);

	public readonly ObjectCollectionHelperData HelperDataTeaWines;

	private static readonly DataInfluence[][] CacheInfluencesSkillBooks = new DataInfluence[7][];

	private readonly ObjectCollectionDataStates _dataStatesSkillBooks = new ObjectCollectionDataStates(7, 0);

	public readonly ObjectCollectionHelperData HelperDataSkillBooks;

	private static readonly DataInfluence[][] CacheInfluencesCrickets = new DataInfluence[13][];

	private readonly ObjectCollectionDataStates _dataStatesCrickets = new ObjectCollectionDataStates(13, 0);

	public readonly ObjectCollectionHelperData HelperDataCrickets;

	private static readonly DataInfluence[][] CacheInfluencesMisc = new DataInfluence[5][];

	private readonly ObjectCollectionDataStates _dataStatesMisc = new ObjectCollectionDataStates(5, 0);

	public readonly ObjectCollectionHelperData HelperDataMisc;

	private Queue<uint> _pendingLoadingOperationIds;

	[Obsolete("Instead by FullPoisonEffects. Now only for archive data fix. Do not delete this code.")]
	public Dictionary<int, PoisonEffects> PoisonItems => _poisonItems;

	public IReadOnlyDictionary<int, FullPoisonEffects> PoisonEffects => DomainManager.Extra.PoisonEffects;

	public bool VersionNeedRepairSectAccessory => (object)DomainManager.World.GetCurrWorldGameVersion() == null || DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 78, 31);

	[DomainMethod]
	public List<ItemDisplayData> CatchCricket(DataContext context, short colorId, short partId, short singLevel, sbyte cricketPlaceId)
	{
		List<ItemDisplayData> list = new List<ItemDisplayData>();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		sbyte b = Math.Max(CricketParts.Instance[colorId].Level, CricketParts.Instance[partId].Level);
		int num = 10 + singLevel - Math.Min(b * 5, 40);
		bool flag = CricketParts.Instance[colorId].MustSuccessLoud || CricketParts.Instance[partId].MustSuccessLoud;
		bool flag2 = singLevel >= (flag ? 80 : GlobalConfig.Instance.CatchCricketSuccessSingLevel);
		if (!flag2)
		{
			flag2 = context.Random.CheckPercentProb((singLevel >= 80) ? num : (num / 2));
		}
		if (flag2)
		{
			ItemKey itemKey = DomainManager.Item.CreateCricket(context, colorId, partId);
			Cricket element_Crickets = DomainManager.Item.GetElement_Crickets(itemKey.Id);
			DomainManager.Taiwu.SetCricketLuckPoint(DomainManager.Taiwu.GetCricketLuckPoint() + element_Crickets.CalcCatchLucky(), context);
			DomainManager.Taiwu.AddLegacyPoint(context, 32, 100 + Math.Abs(element_Crickets.CalcCatchLucky()) * 5);
			taiwu.AddInventoryItem(context, itemKey, 1);
			AddCatchCricketProfessionSeniority(context, element_Crickets);
			if (context.Random.CheckPercentProb(element_Crickets.GetGrade() * 2))
			{
				element_Crickets.SetCurrDurability((short)(element_Crickets.GetCurrDurability() - 1), context);
				ItemKey itemKey2 = DomainManager.Item.CreateMisc(context, 25);
				taiwu.AddInventoryItem(context, itemKey2, 1);
				list.Add(DomainManager.Item.GetItemDisplayData(DomainManager.Item.GetBaseItem(itemKey2), 1, -1, -1));
			}
			else if (element_Crickets.GetGrade() > 0 && context.Random.CheckPercentProb(10))
			{
				ItemKey itemKey3 = DomainManager.Item.CreateCricket(context, (short)(element_Crickets.GetGrade() - 1));
				Cricket element_Crickets2 = DomainManager.Item.GetElement_Crickets(itemKey3.Id);
				element_Crickets.SetCurrDurability((short)(element_Crickets.GetMaxDurability() / 2), context);
				element_Crickets2.SetCurrDurability((short)(element_Crickets2.GetMaxDurability() / 2), context);
				taiwu.AddInventoryItem(context, itemKey3, 1);
				list.Add(DomainManager.Item.GetItemDisplayData(DomainManager.Item.GetBaseItem(itemKey3), 1, -1, -1));
				AddCatchCricketProfessionSeniority(context, element_Crickets2);
			}
			list.Insert(0, DomainManager.Item.GetItemDisplayData(element_Crickets, 1, -1, -1));
		}
		else if (singLevel >= 80)
		{
			short[] uselessItemList = CricketPlace.Instance[cricketPlaceId].UselessItemList;
			short templateId = uselessItemList[context.Random.Next(uselessItemList.Length)];
			ItemKey itemKey4 = DomainManager.Item.CreateItem(context, 12, templateId);
			taiwu.AddInventoryItem(context, itemKey4, 1);
			list.Add(DomainManager.Item.GetItemDisplayData(DomainManager.Item.GetBaseItem(itemKey4), 1, -1, -1));
		}
		return list;
	}

	public void AddCatchCricketProfessionSeniority(DataContext context, Cricket cricket)
	{
		ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[106];
		int baseDelta = formulaCfg.Calculate(cricket.GetValue());
		DomainManager.Extra.ChangeProfessionSeniority(context, 17, baseDelta);
	}

	[DomainMethod]
	public CricketData GetCricketData(int itemId)
	{
		Cricket element_Crickets = DomainManager.Item.GetElement_Crickets(itemId);
		return new CricketData
		{
			Injuries = element_Crickets.GetInjuries(),
			WinsCount = element_Crickets.GetWinsCount(),
			LossesCount = element_Crickets.GetLossesCount(),
			BestEnemyColorId = element_Crickets.GetBestEnemyColorId(),
			BestEnemyPartId = element_Crickets.GetBestEnemyPartId(),
			Age = element_Crickets.GetAge(),
			MaxAge = (short)element_Crickets.CalcMaxAge(),
			IsSmart = DomainManager.Extra.IsCricketSmart(itemId),
			IsIdentified = DomainManager.Extra.IsCricketIdentified(itemId),
			CircketValue = element_Crickets.GetValue()
		};
	}

	[DomainMethod]
	public List<CricketData> GetCricketDataList(List<ItemKey> itemList)
	{
		List<CricketData> list = new List<CricketData>();
		if (itemList == null)
		{
			return list;
		}
		for (int i = 0; i < itemList.Count; i++)
		{
			CricketData cricketData = GetCricketData(itemList[i].Id);
			list.Add(cricketData);
		}
		return list;
	}

	[DomainMethod]
	public void SetCricketRecord(DataContext context, int itemId, bool win, int enemyItemId)
	{
		if (!DomainManager.Item.TryGetElement_Crickets(itemId, out var element))
		{
			return;
		}
		if (win)
		{
			if (DomainManager.Item.TryGetElement_Crickets(enemyItemId, out var element2) && element.GetGrade() - element2.GetGrade() <= 2)
			{
				int num = ((element.GetBestEnemyColorId() > 0) ? Math.Max(CricketParts.Instance[element.GetBestEnemyColorId()].Level, CricketParts.Instance[element.GetBestEnemyPartId()].Level) : 0);
				element.SetWinsCount((short)(element.GetWinsCount() + 1), context);
				if (element2.GetGrade() >= num)
				{
					element.SetBestEnemyColorId(element2.GetColorId(), context);
					element.SetBestEnemyPartId(element2.GetPartId(), context);
				}
				ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[107];
				int baseDelta = formulaCfg.Calculate(element2.GetGrade());
				DomainManager.Extra.ChangeProfessionSeniority(context, 17, baseDelta);
			}
		}
		else
		{
			element.SetLossesCount((short)(element.GetLossesCount() + 1), context);
		}
	}

	[DomainMethod]
	public void AddCricketInjury(DataContext context, int itemId, int index, short value)
	{
		Cricket element_Crickets = DomainManager.Item.GetElement_Crickets(itemId);
		short[] injuries = element_Crickets.GetInjuries();
		injuries[index] += value;
		element_Crickets.SetInjuries(injuries, context);
	}

	[DomainMethod]
	public void SetCricketBattleConfig(sbyte minGrade, sbyte maxGrade, bool onlyNoInjuryCricket)
	{
		_minCricketGrade = minGrade;
		_maxCricketGrade = maxGrade;
		_onlyNoInjuryCricket = onlyNoInjuryCricket;
	}

	public List<CricketWagerData> SelectCricketWagers(DataContext context, int enemyId)
	{
		_cricketBattleEnemyId = enemyId;
		_cricketBattleSelfWager.Type = -1;
		_cricketBattleEnemyWager.Type = -1;
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(enemyId);
		List<CricketWagerData> list = new List<CricketWagerData>();
		foreach (Wager item in CalcEnemyWagers(context.Random, element_Objects))
		{
			long wagerValue = GetWagerValue(item);
			CricketWagerData cricketWagerData = new CricketWagerData
			{
				Wager = item,
				Crickets = GetNpcCricketDisplayDataListForCricketBattle(context, enemyId, _cricketBattleEnemyCrickets, item.Grade),
				MinWagerValue = ((wagerValue == 0L) ? 0 : Math.Max(wagerValue, WagerValueUnit))
			};
			cricketWagerData.PreRandomizedShowCricketIndex = (byte)context.Random.Next(cricketWagerData.Crickets.Count);
			list.Add(cricketWagerData);
		}
		return list;
	}

	public List<ItemDisplayData> GetNpcCricketDisplayDataListForCricketBattle(DataContext context, int charId, List<ItemKey> tempCreateCricketKeyList, sbyte wagerGrade = -1)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		sbyte grade = element_Objects.GetOrganizationInfo().Grade;
		if (wagerGrade < 0)
		{
			wagerGrade = grade;
		}
		short lifeSkillAttainment = element_Objects.GetLifeSkillAttainment(15);
		List<ItemDisplayData> list = new List<ItemDisplayData>();
		List<sbyte> list2 = ObjectPool<List<sbyte>>.Instance.Get();
		list2.AddRange(CricketGenerator.Generate(grade, wagerGrade, lifeSkillAttainment));
		FillInventoryCricket(charId, list2, list);
		foreach (sbyte item in list2)
		{
			sbyte b = Math.Clamp(item, _minCricketGrade, _maxCricketGrade);
			short templateId = b;
			ItemKey itemKey = DomainManager.Item.CreateCricket(context, templateId, b == 8);
			if (_onlyNoInjuryCricket)
			{
				Cricket element_Crickets = DomainManager.Item.GetElement_Crickets(itemKey.Id);
				short[] injuries = element_Crickets.GetInjuries();
				Array.Clear(injuries, 0, injuries.Length);
				element_Crickets.SetInjuries(injuries, context);
			}
			tempCreateCricketKeyList.Add(itemKey);
			list.Add(DomainManager.Item.GetItemDisplayData(itemKey));
		}
		ObjectPool<List<sbyte>>.Instance.Return(list2);
		CollectionUtils.Shuffle(context.Random, list);
		return list;
	}

	private void FillInventoryCricket(int charId, IList<sbyte> grades, List<ItemDisplayData> crickets)
	{
		sbyte minGrade = grades.Min();
		List<ItemDisplayData> inventoryItems = DomainManager.Character.GetInventoryItems(charId, 1100);
		inventoryItems.RemoveAll(delegate(ItemDisplayData data)
		{
			if (data.Durability <= 0)
			{
				return true;
			}
			sbyte cricketGrade = ItemTemplateHelper.GetCricketGrade(data.CricketColorId, data.CricketPartId);
			if (cricketGrade < _minCricketGrade || cricketGrade > _maxCricketGrade || cricketGrade < minGrade)
			{
				return true;
			}
			return (_onlyNoInjuryCricket && GetElement_Crickets(data.Key.Id).GetInjuries().Sum() > 0) ? true : false;
		});
		inventoryItems.Sort((ItemDisplayData lhs, ItemDisplayData rhs) => GetGrade(lhs).CompareTo(GetGrade(rhs)));
		while (inventoryItems.Count > grades.Count)
		{
			inventoryItems.RemoveAt(0);
		}
		crickets.AddRange(inventoryItems);
		for (int num = 0; num < inventoryItems.Count; num++)
		{
			minGrade = grades.Min();
			grades.Remove(minGrade);
		}
		static sbyte GetGrade(ItemDisplayData data)
		{
			return ItemTemplateHelper.GetCricketGrade(data.CricketColorId, data.CricketPartId);
		}
	}

	public void SetWager(Wager selfWager, Wager enemyWager)
	{
		_cricketBattleSelfWager = selfWager;
		_cricketBattleEnemyWager = enemyWager;
	}

	[DomainMethod]
	public bool SettlementCricketWagerByGiveUp(DataContext context, bool win)
	{
		return SettlementCricketWager(context, win, null, null);
	}

	[DomainMethod]
	public bool SettlementCricketWager(DataContext context, bool win, ItemKey[] taiwuCricketKeys, short[] durabilityList)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(_cricketBattleEnemyId);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (taiwuCricketKeys != null && taiwuCricketKeys.Length > 0 && durabilityList != null && durabilityList.Length > 0)
		{
			for (int i = 0; i < taiwuCricketKeys.Length; i++)
			{
				DomainManager.TaiwuEvent.SetListenerEventActionISerializableArg("CricketCombatOver", $"SelfCricket{i}", (ISerializableGameData)(object)taiwuCricketKeys[i]);
				Cricket element_Crickets = DomainManager.Item.GetElement_Crickets(taiwuCricketKeys[i].Id);
				element_Crickets.SetCurrDurability(durabilityList[i], context);
			}
		}
		if (win)
		{
			TransferWager(context, element_Objects, taiwu, _cricketBattleEnemyWager);
		}
		else if (_cricketBattleSelfWager.Type != 2)
		{
			TransferWager(context, taiwu, element_Objects, _cricketBattleSelfWager);
		}
		else
		{
			TaiwuTransferCharacterWager(context);
		}
		int delta = (win ? GlobalConfig.Instance.OtherCombatWinHappiness[taiwu.GetBehaviorType()] : GlobalConfig.Instance.OtherCombatLoseHappiness[taiwu.GetBehaviorType()]);
		int baseDelta = (win ? GlobalConfig.Instance.OtherCombatWinFavorability[taiwu.GetBehaviorType()] : GlobalConfig.Instance.OtherCombatLoseFavorability[taiwu.GetBehaviorType()]);
		int delta2 = (win ? GlobalConfig.Instance.OtherCombatLoseHappiness[element_Objects.GetBehaviorType()] : GlobalConfig.Instance.OtherCombatWinHappiness[element_Objects.GetBehaviorType()]);
		int baseDelta2 = (win ? GlobalConfig.Instance.OtherCombatLoseFavorability[element_Objects.GetBehaviorType()] : GlobalConfig.Instance.OtherCombatWinFavorability[element_Objects.GetBehaviorType()]);
		taiwu.ChangeHappiness(context, delta);
		element_Objects.ChangeHappiness(context, delta2);
		DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, taiwu, element_Objects, baseDelta);
		DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, element_Objects, taiwu, baseDelta2);
		Events.RaiseCricketCombatFinished(context, win);
		if (win && taiwuCricketKeys != null)
		{
			int num = 0;
			for (int j = 0; j < taiwuCricketKeys.Length; j++)
			{
				ItemKey itemKey = taiwuCricketKeys[j];
				num += Config.Cricket.Instance[itemKey.TemplateId].Grade;
			}
			int num2 = 0;
			foreach (ItemKey cricketBattleEnemyCricket in _cricketBattleEnemyCrickets)
			{
				num2 += Config.Cricket.Instance[cricketBattleEnemyCricket.TemplateId].Grade;
			}
			if (num <= num2 + 3)
			{
				DomainManager.Taiwu.AddLegacyPoint(context, 33);
			}
		}
		foreach (ItemKey cricketBattleEnemyCricket2 in _cricketBattleEnemyCrickets)
		{
			DomainManager.Item.RemoveItem(context, cricketBattleEnemyCricket2);
		}
		_cricketBattleEnemyCrickets.Clear();
		_minCricketGrade = 0;
		_maxCricketGrade = 8;
		_onlyNoInjuryCricket = false;
		return win;
	}

	private void TaiwuTransferCharacterWager(DataContext context)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(_cricketBattleEnemyId);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int id = taiwu.GetId();
		if (ExternalRelationStateHelper.IsActive(taiwu.GetExternalRelationState(), 2))
		{
			KidnappedCharacterList kidnappedCharacters = DomainManager.Character.GetKidnappedCharacters(id);
			int num = kidnappedCharacters.IndexOf(_cricketBattleSelfWager.CharId);
			if (num >= 0)
			{
				DomainManager.Character.TransferKidnappedCharacter(context, _cricketBattleEnemyId, taiwu.GetId(), kidnappedCharacters.Get(num));
				return;
			}
		}
		if (_cricketBattleSelfWager.CharId == id)
		{
			EventHelper.TriggerLegacyPassingEvent(isTaiwuDying: true);
		}
		else if (DomainManager.Taiwu.IsInGroup(_cricketBattleSelfWager.CharId))
		{
			ItemKey inventoryRope = element_Objects.GetInventoryRope(context, element_Objects.GetOrganizationInfo().Grade);
			DomainManager.Character.AddKidnappedCharacter(context, _cricketBattleEnemyId, _cricketBattleSelfWager.CharId, inventoryRope);
		}
	}

	public void TransferWager(DataContext context, GameData.Domains.Character.Character srcChar, GameData.Domains.Character.Character destChar, Wager wager)
	{
		switch (wager.Type)
		{
		case 0:
			if (wager.Count > 0)
			{
				DomainManager.Character.TransferResource(context, srcChar, destChar, wager.WagerResourceType, wager.Count);
			}
			break;
		case 1:
			if (wager.Count > 0)
			{
				DomainManager.Character.TransferInventoryItem(context, srcChar, destChar, wager.ItemKey, wager.Count);
			}
			break;
		case 2:
		{
			int id = destChar.GetId();
			int id2 = srcChar.GetId();
			KidnappedCharacterList kidnappedCharacters = DomainManager.Character.GetKidnappedCharacters(srcChar.GetId());
			KidnappedCharacter kidnappedCharData = kidnappedCharacters.Get(kidnappedCharacters.IndexOf(wager.CharId));
			DomainManager.Character.TransferKidnappedCharacter(context, id, id2, kidnappedCharData);
			break;
		}
		case 3:
			if (wager.Count > 0)
			{
				destChar.ChangeExp(context, wager.Count);
			}
			break;
		}
	}

	public bool CheckCharacterHasWager(GameData.Domains.Character.Character character, Wager wager)
	{
		int value;
		GameData.Domains.Character.Character element;
		return wager.Type switch
		{
			0 => character.GetResource(wager.WagerResourceType) >= wager.Count, 
			1 => character.GetInventory().Items.TryGetValue(wager.ItemKey, out value) && value >= wager.Count, 
			2 => DomainManager.Character.TryGetElement_Objects(wager.CharId, out element) && element.GetKidnapperId() == character.GetId(), 
			3 => true, 
			_ => false, 
		};
	}

	public bool NpcHasAnyCricketWager(int charId)
	{
		IRandomSource random = DataContextManager.GetCurrentThreadDataContext().Random;
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		return CalcEnemyWagers(random, element_Objects).Any();
	}

	public Wager SelectCharacterValidWager(DataContext context, GameData.Domains.Character.Character character)
	{
		List<Wager> obj = context.AdvanceMonthRelatedData.Wagers.Occupy();
		GetCharacterValidWagers(context.Random, character, obj);
		Wager randomOrDefault = obj.GetRandomOrDefault(context.Random, Wager.CreateResource(6, 0));
		context.AdvanceMonthRelatedData.Wagers.Release(ref obj);
		Tester.Assert(CheckCharacterHasWager(character, randomOrDefault));
		return randomOrDefault;
	}

	public static void GetCharacterValidWagers(IRandomSource random, GameData.Domains.Character.Character character, List<Wager> validWagers)
	{
		validWagers.Clear();
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		Vector2 vector = Wager.BehaviorValueRange[character.GetBehaviorType()];
		OrganizationItem organizationItem = Config.Organization.Instance[organizationInfo.OrgTemplateId];
		sbyte b = organizationInfo.Grade;
		do
		{
			short index = organizationItem.Members[b];
			OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[index];
			int expectedWagerValue = organizationMemberItem.ExpectedWagerValue;
			Dictionary<ItemKey, int> items = character.GetInventory().Items;
			float num = Math.Max(vector.X * (float)expectedWagerValue, 1f);
			float num2 = Math.Max(vector.Y * (float)expectedWagerValue, num);
			foreach (KeyValuePair<ItemKey, int> item2 in items)
			{
				int value = DomainManager.Item.GetValue(item2.Key);
				if (num <= (float)value && (float)value <= num2)
				{
					for (int i = 0; i < 3; i++)
					{
						validWagers.Add(Wager.CreateItem(item2.Key, 1));
					}
				}
			}
			for (sbyte b2 = 0; b2 < 8; b2++)
			{
				int resource = character.GetResource(b2);
				sbyte b3 = GlobalConfig.ResourcesWorth[b2];
				short num3 = GlobalConfig.UnitsOfResourceTransfer[b2];
				num = Math.Max(vector.X * (float)expectedWagerValue, b3 * num3);
				num2 = Math.Max(vector.Y * (float)expectedWagerValue, num);
				int num4 = (int)(num / (float)b3 / (float)num3);
				int num5 = (int)(Math.Min(num2 / (float)b3, resource) / (float)num3);
				if (resource / num3 >= num4)
				{
					Wager item = Wager.CreateResource(b2, num3 * random.Next(num4, num5 + 1));
					for (int j = 0; j < Wager.ResourceRandomWeight[b2]; j++)
					{
						validWagers.Add(item);
					}
				}
			}
			b--;
		}
		while (validWagers.Count == 0 && b >= 0);
	}

	private IEnumerable<Wager> CalcEnemyWagers(IRandomSource random, GameData.Domains.Character.Character character)
	{
		sbyte charGrade = character.GetOrganizationInfo().Grade;
		sbyte taiwuFame = DomainManager.Taiwu.GetTaiwu().GetFame();
		(sbyte, sbyte) tuple = CricketSpecialConstants.CalcWagerGradeRange(charGrade, taiwuFame);
		sbyte minGrade = tuple.Item1;
		sbyte maxGrade = tuple.Item2;
		List<ItemKey> itemPool = ObjectPool<List<ItemKey>>.Instance.Get();
		Dictionary<ItemKey, int> items = character.GetInventory().Items;
		foreach (Func<ItemKey, bool> matcher in CricketSpecialConstants.WagerItemMatchers)
		{
			itemPool.Clear();
			itemPool.AddRange(items.Keys.Where(matcher).Where(GradeMatcher));
			itemPool.RemoveAll((ItemKey x) => GetBaseItem(x).GetValue() < 1);
			if (itemPool.Count != 0)
			{
				sbyte highestGrade = itemPool.Max((ItemKey x) => ItemTemplateHelper.GetGrade(x.ItemType, x.TemplateId));
				itemPool.RemoveAll((ItemKey x) => ItemTemplateHelper.GetGrade(x.ItemType, x.TemplateId) < highestGrade);
				ItemKey itemKey = itemPool.GetRandom(random);
				yield return Wager.CreateItem(itemKey, 1);
			}
		}
		ObjectPool<List<ItemKey>>.Instance.Return(itemPool);
		List<sbyte> resourcePool = ObjectPool<List<sbyte>>.Instance.Get();
		List<sbyte> resourceGrades = ObjectPool<List<sbyte>>.Instance.Get();
		for (sbyte resourceType = 0; resourceType < 8; resourceType++)
		{
			int resourceCount = character.GetResource(resourceType);
			for (sbyte resourceGrade = maxGrade; resourceGrade >= minGrade; resourceGrade--)
			{
				int gradeCount = CricketSpecialConstants.GradeToPriceResource(resourceType, resourceGrade);
				if (gradeCount <= resourceCount)
				{
					resourcePool.Add(resourceType);
					resourceGrades.Add(resourceGrade);
					break;
				}
			}
		}
		foreach (sbyte resourceType2 in RandomUtils.GetRandomUnrepeated(random, 3, resourcePool))
		{
			int index = resourcePool.IndexOf(resourceType2);
			sbyte grade = resourceGrades[index];
			int count = CricketSpecialConstants.GradeToPriceResource(resourceType2, grade);
			yield return Wager.CreateResource(resourceType2, count);
		}
		ObjectPool<List<sbyte>>.Instance.Return(resourcePool);
		ObjectPool<List<sbyte>>.Instance.Return(resourceGrades);
		int exp = CricketSpecialConstants.GradeToPriceExp((sbyte)(maxGrade / 2));
		yield return Wager.CreateExp(exp);
		bool GradeMatcher(ItemKey itemKey2)
		{
			sbyte grade2 = ItemTemplateHelper.GetGrade(itemKey2.ItemType, itemKey2.TemplateId);
			return minGrade <= grade2 && grade2 <= maxGrade;
		}
	}

	private long GetWagerValue(Wager wager)
	{
		sbyte type = wager.Type;
		if ((type == 0 || type == 3) ? true : false)
		{
			return wager.CalcWagerValue(0, 0, 0, 0, -1, 0);
		}
		if (wager.Type == 1)
		{
			return wager.CalcWagerValue(GetValue(wager.ItemKey), 0, 0, 0, -1, 0);
		}
		if (wager.Type == 2)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(wager.CharId);
			return wager.CalcWagerValue(0, element_Objects.GetFame(), element_Objects.GetAttraction(), element_Objects.GetPhysiologicalAge(), element_Objects.GetAvatar().Gender, element_Objects.GetOrganizationInfo().Grade);
		}
		if (wager.Type == 3)
		{
			return wager.CalcWagerValue(0, 0, 0, 0, -1, 0);
		}
		return 0L;
	}

	[DomainMethod]
	public void MakeCricketRebirth(DataContext ctx, ItemKey itemKey)
	{
		if (itemKey.ItemType == 11 && TryGetElement_Crickets(itemKey.Id, out var element))
		{
			element.Rebirth(ctx);
			ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[110];
			int baseDelta = formulaCfg.Calculate(element.GetValue());
			DomainManager.Extra.ChangeProfessionSeniority(ctx, 17, baseDelta);
		}
	}

	public List<int> GetAllCricketIdList()
	{
		return new List<int>(_crickets.Keys);
	}

	public ItemKey CreateCricketByLuckPoint(DataContext context, ref int luckPoint, int simulateCount = 1)
	{
		List<(short, short)> obj = context.AdvanceMonthRelatedData.WeightTable.Occupy();
		short colorId = 0;
		short partId = 0;
		sbyte b = 0;
		for (int i = 0; i < simulateCount; i++)
		{
			(short, short) tuple = SimulateCricketByLuckPoint(context.Random, luckPoint, obj);
			luckPoint += tuple.CalcCatchLucky();
			sbyte b2 = tuple.CalcLevel();
			if (b2 >= b && (b2 != b || !context.Random.CheckPercentProb(50)))
			{
				(colorId, partId) = tuple;
			}
		}
		context.AdvanceMonthRelatedData.WeightTable.Release(ref obj);
		return CreateCricket(context, colorId, partId);
	}

	private static (short colorId, short partId) SimulateCricketByLuckPoint(IRandomSource random, int luckPoint, List<(short, short)> weightTable)
	{
		weightTable.Clear();
		foreach (CricketPlaceItem item2 in (IEnumerable<CricketPlaceItem>)CricketPlace.Instance)
		{
			weightTable.Add((item2.TemplateId, item2.PlaceRate));
		}
		short randomResult = RandomUtils.GetRandomResult(weightTable, random);
		CricketPlaceItem cricketPlaceItem = CricketPlace.Instance[randomResult];
		weightTable.Clear();
		weightTable.Add((4, cricketPlaceItem.Cyan));
		weightTable.Add((5, cricketPlaceItem.Yellow));
		weightTable.Add((6, cricketPlaceItem.Purple));
		weightTable.Add((7, cricketPlaceItem.Red));
		weightTable.Add((8, cricketPlaceItem.Black));
		weightTable.Add((9, cricketPlaceItem.White));
		weightTable.Add((0, cricketPlaceItem.Trash));
		ECricketPartsType randomResult2 = (ECricketPartsType)RandomUtils.GetRandomResult(weightTable, random);
		if (randomResult2 == ECricketPartsType.Trash)
		{
			return (colorId: 0, partId: 0);
		}
		weightTable.Clear();
		foreach (CricketPartsItem item3 in (IEnumerable<CricketPartsItem>)CricketParts.Instance)
		{
			if (item3.Type == randomResult2)
			{
				weightTable.Add((item3.TemplateId, item3.Rate));
			}
		}
		short randomResult3 = RandomUtils.GetRandomResult(weightTable, random);
		if (random.Next(21) == 0 && random.CheckPercentProb(CricketParts.Instance[randomResult3].AdvanceRate * luckPoint / 100))
		{
			short item;
			if (random.CheckPercentProb(luckPoint / 10))
			{
				weightTable.Clear();
				foreach (CricketPartsItem item4 in (IEnumerable<CricketPartsItem>)CricketParts.Instance)
				{
					if (item4.Type == ECricketPartsType.King)
					{
						weightTable.Add((item4.TemplateId, item4.Rate));
					}
				}
				item = RandomUtils.GetRandomResult(weightTable, random);
			}
			else
			{
				if (1 == 0)
				{
				}
				short num = randomResult2 switch
				{
					ECricketPartsType.Cyan => 22, 
					ECricketPartsType.Yellow => 23, 
					ECricketPartsType.Purple => 24, 
					ECricketPartsType.Red => 25, 
					ECricketPartsType.Black => 26, 
					ECricketPartsType.White => 27, 
					_ => 0, 
				};
				if (1 == 0)
				{
				}
				item = num;
			}
			return (colorId: item, partId: 0);
		}
		weightTable.Clear();
		foreach (CricketPartsItem item5 in (IEnumerable<CricketPartsItem>)CricketParts.Instance)
		{
			if (item5.Type == ECricketPartsType.Parts)
			{
				weightTable.Add((item5.TemplateId, item5.Rate));
			}
		}
		short randomResult4 = RandomUtils.GetRandomResult(weightTable, random);
		return (colorId: randomResult3, partId: randomResult4);
	}

	[DomainMethod]
	public List<sbyte> GetWeaponTricks(ItemKey weaponKey)
	{
		if (TryGetElement_Weapons(weaponKey.Id, out var element))
		{
			return element.GetTricks();
		}
		return Config.Weapon.Instance[weaponKey.TemplateId].Tricks;
	}

	[DomainMethod]
	public (int minDist, int maxDist) GetWeaponAttackRange(int charId, ItemKey weaponKey)
	{
		if (!TryGetElement_Weapons(weaponKey.Id, out var element))
		{
			WeaponItem weaponItem = Config.Weapon.Instance[weaponKey.TemplateId];
			return (minDist: weaponItem.MinDistance, maxDist: weaponItem.MaxDistance);
		}
		(int, int) result = (element.GetMinDistance(), element.GetMaxDistance());
		int modifyValue = DomainManager.SpecialEffect.GetModifyValue(charId, 29, (EDataModifyType)0, weaponKey.ItemType, weaponKey.TemplateId, weaponKey.Id, (EDataSumType)0);
		result.Item1 = Math.Max(result.Item1 - modifyValue, 20);
		result.Item2 = Math.Min(result.Item2 + modifyValue, 120);
		return result;
	}

	[DomainMethod]
	public int GetWeaponPrepareFrame(int charId, ItemKey weaponKey)
	{
		if (!_weapons.TryGetValue(weaponKey.Id, out var value))
		{
			return 0;
		}
		if (DomainManager.Combat.IsCharInCombat(charId))
		{
			CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
			return element_CombatCharacterDict.CalcNormalAttackStartupFrames(value);
		}
		int baseStartupFrames = Config.Weapon.Instance[value.GetTemplateId()].BaseStartupFrames;
		if (DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			return value.CalcAttackStartupOrRecoveryFrame(element.GetAttackSpeed(), baseStartupFrames);
		}
		AdaptableLog.Warning($"Try to get {weaponKey} prepare frame with a invalid charId {charId}");
		return value.CalcAttackStartupOrRecoveryFrame(100, baseStartupFrames);
	}

	[DomainMethod]
	public short[] GetCricketCombatRecords(ItemKey cricketKey)
	{
		Cricket element_Crickets = DomainManager.Item.GetElement_Crickets(cricketKey.Id);
		return new short[2]
		{
			element_Crickets.GetWinsCount(),
			element_Crickets.GetLossesCount()
		};
	}

	[DomainMethod]
	public ItemDisplayData GetItemDisplayData(ItemKey itemKey, int charId = -1)
	{
		if (itemKey.Id == -1)
		{
			itemKey = CreateItem(DomainManager.TaiwuEvent.MainThreadDataContext, itemKey.ItemType, itemKey.TemplateId);
			PredefinedLog.Show(12, $"Cannot use GetItemDisplayData for create item, {itemKey} {charId}");
		}
		ItemBase itemBase = TryGetBaseItem(itemKey);
		if (itemBase != null)
		{
			return DomainManager.Item.GetItemDisplayData(itemBase, 1, charId, -1);
		}
		Logger.Warn($"{itemKey} try to get deleted item display data through pure template.");
		return new ItemDisplayData(itemKey.ItemType, itemKey.TemplateId);
	}

	[Obsolete("注意此方法不会合并有毒物品！！！可以用GetItemDisplayDataListOptional替代")]
	[DomainMethod]
	public List<ItemDisplayData> GetItemDisplayDataList(List<ItemKey> itemKeyList, int charId = -1)
	{
		return GetItemDisplayDataListOptional(itemKeyList, charId, -1);
	}

	[DomainMethod]
	public List<ItemDisplayData> GetItemDisplayDataListOptional(List<ItemKey> itemKeyList, int charId = -1, sbyte itemSourceType = -1, bool merge = false)
	{
		List<ItemDisplayData> list = null;
		if (itemKeyList != null && itemKeyList.Count > 0)
		{
			if (merge)
			{
				Dictionary<ItemKey, int> items = itemKeyList.GroupBy((ItemKey i) => i, (ItemKey key, IEnumerable<ItemKey> keys) => new
				{
					key = key,
					amount = keys.Count()
				}).ToDictionary(g => g.key, g => g.amount);
				list = CharacterDomain.GetItemDisplayData(charId, items, (ItemSourceType)itemSourceType);
			}
			else
			{
				list = new List<ItemDisplayData>();
				foreach (ItemKey itemKey in itemKeyList)
				{
					ItemBase itemBase = TryGetBaseItem(itemKey);
					ItemDisplayData item = ((itemBase != null) ? DomainManager.Item.GetItemDisplayData(itemBase, 1, charId, itemSourceType) : new ItemDisplayData(itemKey.ItemType, itemKey.TemplateId));
					if (itemBase == null && !ItemTemplateHelper.IsThanksLetter(itemKey.ItemType, itemKey.TemplateId))
					{
						AdaptableLog.Warning($"Try get not exist item display data by {itemKey}");
					}
					list.Add(item);
				}
			}
		}
		return list;
	}

	[DomainMethod]
	public List<ItemDisplayData> GetItemDisplayDataListOptionalFromInventory(Inventory inventory, int charId = -1, sbyte itemSourceType = -1, bool merge = false)
	{
		List<ItemDisplayData> list = null;
		if (inventory?.Items != null && inventory.Items.Count > 0)
		{
			if (merge)
			{
				list = CharacterDomain.GetItemDisplayData(charId, inventory.Items, (ItemSourceType)itemSourceType);
			}
			else
			{
				list = new List<ItemDisplayData>();
				foreach (KeyValuePair<ItemKey, int> item in inventory.Items)
				{
					item.Deconstruct(out var key, out var value);
					ItemKey itemKey = key;
					int amount = value;
					ItemDisplayData itemDisplayData = DomainManager.Item.GetItemDisplayData(GetBaseItem(itemKey), amount, charId, itemSourceType);
					list.Add(itemDisplayData);
				}
			}
		}
		return list;
	}

	[DomainMethod]
	public SkillBookPageDisplayData GetSkillBookPagesInfo(ItemKey itemKey)
	{
		if (!ItemExists(itemKey))
		{
			SkillBookItem skillBookItem = Config.SkillBook.Instance[itemKey.TemplateId];
			bool flag = skillBookItem.ItemSubType == 1001;
			SkillBookPageDisplayData skillBookPageDisplayData = new SkillBookPageDisplayData();
			skillBookPageDisplayData.ItemKey = itemKey;
			skillBookPageDisplayData.State = new sbyte[flag ? 6 : 5];
			skillBookPageDisplayData.ReadingProgress = new sbyte[flag ? 6 : 5];
			skillBookPageDisplayData.Type = new sbyte[flag ? 6 : 5];
			return skillBookPageDisplayData;
		}
		SkillBook element_SkillBooks = GetElement_SkillBooks(itemKey.Id);
		SkillBookPageDisplayData skillBookPageDisplayData2 = new SkillBookPageDisplayData();
		ushort pageIncompleteState = element_SkillBooks.GetPageIncompleteState();
		byte pageTypes = element_SkillBooks.GetPageTypes();
		skillBookPageDisplayData2.ItemKey = itemKey;
		if (SkillGroup.FromItemSubType(element_SkillBooks.GetItemSubType()) == 0)
		{
			short lifeSkillTemplateId = element_SkillBooks.GetLifeSkillTemplateId();
			TaiwuLifeSkill notLearnLifeSkill;
			if (DomainManager.Taiwu.TryGetTaiwuLifeSkill(lifeSkillTemplateId, out var lifeSkill))
			{
				skillBookPageDisplayData2.ReadingProgress = lifeSkill.GetAllBookPageReadingProgress();
			}
			else if (DomainManager.Taiwu.TryGetNotLearnLifeSkillReadingProgress(lifeSkillTemplateId, out notLearnLifeSkill))
			{
				skillBookPageDisplayData2.ReadingProgress = notLearnLifeSkill.GetAllBookPageReadingProgress();
			}
			else
			{
				skillBookPageDisplayData2.ReadingProgress = new sbyte[5];
			}
			skillBookPageDisplayData2.Type = new sbyte[5];
		}
		else
		{
			skillBookPageDisplayData2.Type = new sbyte[6];
			skillBookPageDisplayData2.Type[0] = SkillBookStateHelper.GetOutlinePageType(pageTypes);
			for (byte b = 1; b < 6; b++)
			{
				skillBookPageDisplayData2.Type[b] = SkillBookStateHelper.GetNormalPageType(pageTypes, b);
			}
			short combatSkillTemplateId = element_SkillBooks.GetCombatSkillTemplateId();
			sbyte[] array = null;
			TaiwuCombatSkill notLearnCombatSkill;
			if (DomainManager.Taiwu.TryGetElement_CombatSkills(combatSkillTemplateId, out var value))
			{
				array = value.GetAllBookPageReadingProgress();
			}
			else if (DomainManager.Taiwu.TryGetNotLearnCombatSkillReadingProgress(combatSkillTemplateId, out notLearnCombatSkill))
			{
				array = notLearnCombatSkill.GetAllBookPageReadingProgress();
			}
			skillBookPageDisplayData2.ReadingProgress = new sbyte[6];
			if (array != null)
			{
				for (byte b2 = 0; b2 < skillBookPageDisplayData2.ReadingProgress.Length; b2++)
				{
					skillBookPageDisplayData2.ReadingProgress[b2] = array[CombatSkillStateHelper.GetPageInternalIndex(skillBookPageDisplayData2.Type[0], skillBookPageDisplayData2.Type[b2], b2)];
				}
			}
		}
		skillBookPageDisplayData2.State = new sbyte[skillBookPageDisplayData2.ReadingProgress.Length];
		for (byte b3 = 0; b3 < skillBookPageDisplayData2.ReadingProgress.Length; b3++)
		{
			skillBookPageDisplayData2.State[b3] = SkillBookStateHelper.GetPageIncompleteState(pageIncompleteState, b3);
		}
		return skillBookPageDisplayData2;
	}

	[DomainMethod]
	public List<SkillBookPageDisplayData> GetSkillBookPageDisplayDataList(List<ItemKey> itemKeyList)
	{
		List<SkillBookPageDisplayData> list = new List<SkillBookPageDisplayData>();
		foreach (ItemKey itemKey in itemKeyList)
		{
			if (itemKey.IsValid())
			{
				list.Add(GetSkillBookPagesInfo(itemKey));
			}
		}
		return list;
	}

	public ItemDisplayData GetItemDisplayData(ItemBase item, int amount = 1, int charId = -1, sbyte itemSourceType = -1)
	{
		ItemKey itemKey = item.GetItemKey();
		LoveTokenDataItem loveTokenDataItem;
		ItemDisplayData itemDisplayData = new ItemDisplayData(itemKey, amount)
		{
			Durability = item.GetCurrDurability(),
			MaxDurability = item.GetMaxDurability(),
			Weight = item.GetWeight(),
			Value = DomainManager.Item.GetValue(itemKey),
			OwnerCharId = charId,
			ItemSourceType = itemSourceType,
			CarrierTamePoint = DomainManager.Extra.GetCarrierTamePoint(itemKey.Id),
			IsReadingFinished = (itemKey.ItemType == 10 && DomainManager.Taiwu.GetTotalReadingProgress(itemKey.Id) >= 100),
			IsThreeCorpseKeepingLegendaryBook = DomainManager.Extra.IsThreeCorpseKeepingLegendaryBook(itemKey),
			LoveTokenDataItem = (DomainManager.Extra.TryGetLoveTokenData(itemKey, out loveTokenDataItem) ? loveTokenDataItem : new LoveTokenDataItem())
		};
		if (ModificationStateHelper.IsActive(itemKey.ModificationState, 2))
		{
			itemDisplayData.RefiningEffects = DomainManager.Item.GetRefinedEffects(itemKey);
		}
		if (ModificationStateHelper.IsActive(itemKey.ModificationState, 1))
		{
			DomainManager.Extra.TryGetPoisonEffect(itemKey.Id, out itemDisplayData.PoisonEffects);
		}
		if (item is EquipmentBase equipmentBase)
		{
			itemDisplayData.EquipmentEffectIds = new List<short>(from x in DomainManager.Item.GetEquipmentEffects(equipmentBase)
				select x.TemplateId);
			itemDisplayData.MaterialResources = equipmentBase.GetMaterialResources();
		}
		if (item is Weapon weapon)
		{
			itemDisplayData.EquipmentAttack = weapon.GetEquipmentAttack();
			itemDisplayData.EquipmentDefense = weapon.GetEquipmentDefense();
			itemDisplayData.HitAvoidFactor = ((charId >= 0) ? weapon.GetHitFactors(charId) : weapon.GetHitFactors());
			itemDisplayData.PenetrationInfo.Item1 = weapon.GetPenetrationFactor();
		}
		else if (item is Armor armor)
		{
			itemDisplayData.EquipmentAttack = armor.GetEquipmentAttack();
			itemDisplayData.EquipmentDefense = armor.GetEquipmentDefense();
			itemDisplayData.HitAvoidFactor = ((charId >= 0) ? armor.GetAvoidFactors(charId) : armor.GetAvoidFactors());
			armor.GetPenetrationResistFactors().Deconstruct(out itemDisplayData.PenetrationInfo.Item1, out itemDisplayData.PenetrationInfo.Item2);
			itemDisplayData.InjuryFactors = armor.GetInjuryFactor();
		}
		else if (itemKey.ItemType == 3)
		{
			itemDisplayData.WeavedClothingTemplateId = DomainManager.Extra.GetModifiedClothingTemplateId(itemKey);
		}
		if (item is Cricket cricket)
		{
			itemDisplayData.SpecialArg = ((ushort)cricket.GetColorId() << 16) | (ushort)cricket.GetPartId();
		}
		else if (item is Weapon || item is Armor)
		{
			itemDisplayData.PowerInfo = DomainManager.Character.GetItemPowerInfo(charId, itemKey);
			itemDisplayData.Requirements = DomainManager.Character.GetItemRequirementsAndActualValues(charId, itemKey);
		}
		else if (item is Misc && GameData.Domains.Combat.SharedConstValue.SwordFragment2BossId.ContainsKey(itemKey.TemplateId))
		{
			itemDisplayData.SpecialArg = DomainManager.Item.GetSwordFragmentCurrSkill(itemKey);
		}
		if (itemDisplayData.ItemSourceTypeEnum == ItemSourceType.Inventory || itemDisplayData.ItemSourceTypeEnum == ItemSourceType.Equipment || itemDisplayData.ItemSourceTypeEnum == ItemSourceType.JiaoPool)
		{
			itemDisplayData.UsingType = DomainManager.Character.GetItemUsingType(itemKey, charId);
		}
		else
		{
			itemDisplayData.UsingType = ItemDisplayData.ItemUsingType.Invalid;
		}
		if (DomainManager.Extra.TryGetJiaoByItemKey(itemKey, out var jiao) && jiao.GrowthStage == 0)
		{
			JiaoItem jiaoItem = Config.Jiao.Instance[jiao.TemplateId];
			itemDisplayData.JiaoEggItemDisplayData = new JiaoEggItemDisplayData
			{
				TemplateId = jiao.TemplateId,
				ColorCount = Convert.ToSByte(jiaoItem.ColorList?.Count ?? 0),
				Behavior = jiao.Behavior,
				Gender = jiao.Gender,
				Generation = jiao.Generation
			};
		}
		return itemDisplayData;
	}

	private void OnInitializedDomainData()
	{
	}

	private void InitializeOnInitializeGameDataModule()
	{
		InitializeWugTemplateIds();
		InitializeCreationTemplateIds();
		InitializeCategorizedItemTemplates();
		InitializeCategorizedEquipmentEffects();
		InitializeSkillBreakPlateBonusEffects();
		MixedPoisonType.InitializeMaskDict();
	}

	private void InitializeOnEnterNewWorld()
	{
		_nextItemId = 0;
		_emptyHandKey = ItemKey.Invalid;
		_branchKey = ItemKey.Invalid;
		_stoneKey = ItemKey.Invalid;
		_trackedSpecialItems.Clear();
	}

	private void OnLoadedArchiveData()
	{
		InitializeTrackedSpecialItems();
	}

	public override void FixAbnormalDomainArchiveData(DataContext context)
	{
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 71, 23))
		{
			int num = 0;
			foreach (KeyValuePair<int, CraftTool> craftTool2 in _craftTools)
			{
				craftTool2.Deconstruct(out var _, out var value);
				CraftTool craftTool = value;
				short maxDurability = craftTool.GetMaxDurability();
				short currDurability = craftTool.GetCurrDurability();
				if (maxDurability > 0 && currDurability > 0)
				{
					CraftToolItem craftToolItem = Config.CraftTool.Instance[craftTool.GetTemplateId()];
					short num2 = ItemBase.GenerateMaxDurability(context.Random, craftToolItem.MaxDurability);
					if (num2 > maxDurability)
					{
						craftTool.SetMaxDurability(num2, context);
						short currDurability2 = (short)(currDurability + num2 - maxDurability);
						craftTool.SetCurrDurability(currDurability2, context);
						num++;
					}
				}
			}
			Logger.Info($"Regenerated max durability for {num} craft tools.");
		}
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 79, 4))
		{
			foreach (Carrier value2 in _carriers.Values)
			{
				short maxDurability2 = value2.GetMaxDurability();
				short currDurability3 = value2.GetCurrDurability();
				CarrierItem carrierItem = Config.Carrier.Instance[value2.GetTemplateId()];
				short num3 = ItemBase.GenerateMaxDurability(context.Random, carrierItem.MaxDurability);
				value2.SetMaxDurability(num3, context);
				value2.SetCurrDurability(num3, context);
				Logger.Warn($"Regenerate {value2.Owner}'s {carrierItem.Name} Durability: ({currDurability3}/{maxDurability2}) -> ({num3}/{num3})");
			}
		}
		TransferOldPoisonData(context);
		DomainManager.Character.RemoveDlcDuplicateClothing(context, fixArchiveData: true);
	}

	[Obsolete("This method is obsolete, and will be removed in future.")]
	public (int outer, int inner, int mind, int fatalDamage) GetWeaponBlockDamageValue(DataContext context, int charId, ItemKey weaponKey)
	{
		return (outer: 0, inner: 0, mind: 0, fatalDamage: 0);
	}

	private void InitializeCategorizedEquipmentEffects()
	{
		if (_categorizedEquipmentEffects == null)
		{
			_categorizedEquipmentEffects = new List<short>[3];
		}
		for (int i = 0; i < 3; i++)
		{
			List<short>[] categorizedEquipmentEffects = _categorizedEquipmentEffects;
			int num = i;
			if (categorizedEquipmentEffects[num] == null)
			{
				categorizedEquipmentEffects[num] = new List<short>();
			}
			_categorizedEquipmentEffects[i].Clear();
		}
		foreach (EquipmentEffectItem item in (IEnumerable<EquipmentEffectItem>)EquipmentEffect.Instance)
		{
			if (!item.Special)
			{
				switch (item.Type)
				{
				case 0:
					_categorizedEquipmentEffects[0].Add(item.TemplateId);
					_categorizedEquipmentEffects[1].Add(item.TemplateId);
					_categorizedEquipmentEffects[2].Add(item.TemplateId);
					break;
				case 1:
					_categorizedEquipmentEffects[0].Add(item.TemplateId);
					break;
				case 2:
					_categorizedEquipmentEffects[1].Add(item.TemplateId);
					break;
				}
			}
		}
	}

	private void InitializeSkillBreakPlateBonusEffects()
	{
		int count = SkillBreakBonusEffect.Instance.Count;
		_skillBreakPlateBonusEffects = new List<TemplateKey>[count];
		for (int i = 0; i < count; i++)
		{
			_skillBreakPlateBonusEffects[i] = new List<TemplateKey>();
		}
		for (sbyte b = 0; b < 13; b++)
		{
			IList<int> templateDataAllKeys = ItemTemplateHelper.GetTemplateDataAllKeys(b);
			foreach (int item in templateDataAllKeys)
			{
				short num = (short)item;
				sbyte breakBonusEffect = ItemTemplateHelper.GetBreakBonusEffect(b, num);
				if (breakBonusEffect >= 0)
				{
					short num2 = ItemTemplateHelper.GetGroupId(b, num);
					if (num2 < 0)
					{
						num2 = num;
					}
					else if (num2 != num)
					{
						continue;
					}
					_skillBreakPlateBonusEffects[breakBonusEffect].Add(new TemplateKey(b, num2));
				}
			}
		}
	}

	private void InitializeCategorizedItemTemplates()
	{
		if (_categorizedItemTemplates == null)
		{
			_categorizedItemTemplates = new Dictionary<short, List<short>>[9];
		}
		for (int i = 0; i < 9; i++)
		{
			Dictionary<short, List<short>>[] categorizedItemTemplates = _categorizedItemTemplates;
			int num = i;
			if (categorizedItemTemplates[num] == null)
			{
				categorizedItemTemplates[num] = new Dictionary<short, List<short>>();
			}
			Dictionary<short, List<short>> dictionary = _categorizedItemTemplates[i];
			for (int j = 0; j < 13; j++)
			{
				short[] array = ItemSubType.Type2SubTypes[j];
				short[] array2 = array;
				foreach (short key in array2)
				{
					if (dictionary.TryGetValue(key, out var value))
					{
						value.Clear();
					}
					else
					{
						_categorizedItemTemplates[i].Add(key, new List<short>());
					}
				}
			}
		}
		for (sbyte b = 0; b < 9; b++)
		{
			Dictionary<short, List<short>> dictionary2 = _categorizedItemTemplates[b];
			for (sbyte b2 = 0; b2 < 13; b2++)
			{
				short[] array3 = ItemSubType.Type2SubTypes[b2];
				foreach (short itemSubType in array3)
				{
					if (!ItemSubType.IsHobbyType(itemSubType))
					{
						continue;
					}
					sbyte grade;
					IEnumerable<short> collection;
					switch (b2)
					{
					case 0:
						grade = GetClosestNeighboringGradeWithValidItem(b, Config.Weapon.Instance.ToList(), ((WeaponItem, sbyte) pair) => pair.Item1.Grade == pair.Item2 && itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
						collection = from item in Config.Weapon.Instance
							where item.Grade == grade && itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
							select item.TemplateId;
						break;
					case 1:
						grade = GetClosestNeighboringGradeWithValidItem(b, Config.Armor.Instance.ToList(), ((ArmorItem, sbyte) pair) => pair.Item1.Grade == pair.Item2 && itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
						collection = from item in Config.Armor.Instance
							where item.Grade == grade && itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
							select item.TemplateId;
						break;
					case 2:
						grade = GetClosestNeighboringGradeWithValidItem(b, Config.Accessory.Instance.ToList(), ((AccessoryItem, sbyte) pair) => pair.Item1.Grade == pair.Item2 && itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
						collection = from item in Config.Accessory.Instance
							where item.Grade == grade && itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
							select item.TemplateId;
						break;
					case 3:
						grade = GetClosestNeighboringGradeWithValidItem(b, Config.Clothing.Instance.ToList(), ((ClothingItem, sbyte) pair) => pair.Item1.Grade == pair.Item2 && itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
						collection = from item in Config.Clothing.Instance
							where item.Grade == grade && itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
							select item.TemplateId;
						break;
					case 4:
						grade = GetClosestNeighboringGradeWithValidItem(b, Config.Carrier.Instance.ToList(), ((CarrierItem, sbyte) pair) => pair.Item1.Grade == pair.Item2 && itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
						collection = from item in Config.Carrier.Instance
							where item.Grade == grade && itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
							select item.TemplateId;
						break;
					case 5:
						grade = GetClosestNeighboringGradeWithValidItem(b, Config.Material.Instance.ToList(), ((MaterialItem, sbyte) pair) => pair.Item1.Grade == pair.Item2 && itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
						collection = from item in Config.Material.Instance
							where item.Grade == grade && itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
							select item.TemplateId;
						break;
					case 6:
						grade = GetClosestNeighboringGradeWithValidItem(b, Config.CraftTool.Instance.ToList(), ((CraftToolItem, sbyte) pair) => pair.Item1.Grade == pair.Item2 && itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
						collection = from item in Config.CraftTool.Instance
							where item.Grade == grade && itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
							select item.TemplateId;
						break;
					case 7:
						grade = GetClosestNeighboringGradeWithValidItem(b, Config.Food.Instance.ToList(), ((FoodItem, sbyte) pair) => pair.Item1.Grade == pair.Item2 && itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
						collection = from item in Config.Food.Instance
							where item.Grade == grade && itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
							select item.TemplateId;
						break;
					case 8:
						grade = GetClosestNeighboringGradeWithValidItem(b, Config.Medicine.Instance.ToList(), ((MedicineItem, sbyte) pair) => pair.Item1.Grade == pair.Item2 && itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
						collection = from item in Config.Medicine.Instance
							where item.Grade == grade && itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
							select item.TemplateId;
						break;
					case 9:
						grade = GetClosestNeighboringGradeWithValidItem(b, Config.TeaWine.Instance.ToList(), ((TeaWineItem, sbyte) pair) => pair.Item1.Grade == pair.Item2 && itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
						collection = from item in Config.TeaWine.Instance
							where item.Grade == grade && itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
							select item.TemplateId;
						break;
					case 10:
						grade = GetClosestNeighboringGradeWithValidItem(b, Config.SkillBook.Instance.ToList(), ((SkillBookItem, sbyte) pair) => pair.Item1.Grade == pair.Item2 && itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
						collection = from item in Config.SkillBook.Instance
							where item.Grade == grade && itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
							select item.TemplateId;
						break;
					case 11:
						grade = GetClosestNeighboringGradeWithValidItem(b, Config.Cricket.Instance.ToList(), ((CricketItem, sbyte) pair) => pair.Item1.Grade == pair.Item2 && itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
						collection = from item in Config.Cricket.Instance
							where item.Grade == grade && itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
							select item.TemplateId;
						break;
					case 12:
						grade = GetClosestNeighboringGradeWithValidItem(b, Config.Misc.Instance.ToList(), ((MiscItem, sbyte) pair) => pair.Item1.Grade == pair.Item2 && itemSubType == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
						collection = from item in Config.Misc.Instance
							where item != null && item.Grade == grade && itemSubType == item.ItemSubType && item.DropRate > 0 && item.BaseValue > 0 && item.Transferable && item.AllowRandomCreate
							select item.TemplateId;
						break;
					default:
						throw ItemTemplateHelper.CreateItemTypeException(b2);
					}
					dictionary2[itemSubType].AddRange(collection);
				}
			}
		}
	}

	private void InitializeTrackedSpecialItems()
	{
		foreach (Misc value in _misc.Values)
		{
			short templateId = value.GetTemplateId();
			if (templateId == 225)
			{
				_trackedSpecialItems.Add(value.GetItemKey());
			}
			else if (Config.Misc.Instance[templateId].ItemSubType == 1202)
			{
				ItemKey itemKey = value.GetItemKey();
				_trackedSpecialItems.Add(itemKey);
				DomainManager.LegendaryBook.RegisterLegendaryBookItem(itemKey);
			}
		}
	}

	public void CheckAndTrackSpecialItem(ItemKey itemKey)
	{
		if (itemKey.ItemType == 12)
		{
			if (itemKey.TemplateId == 225)
			{
				_trackedSpecialItems.Add(itemKey);
			}
			else if (Config.Misc.Instance[itemKey.TemplateId].ItemSubType == 1202)
			{
				_trackedSpecialItems.Add(itemKey);
				DomainManager.LegendaryBook.RegisterLegendaryBookItem(itemKey);
			}
		}
	}

	public bool HasTrackedSpecialItems(sbyte itemType, short itemTemplateId)
	{
		foreach (ItemKey trackedSpecialItem in _trackedSpecialItems)
		{
			if (trackedSpecialItem.ItemType == itemType && trackedSpecialItem.TemplateId == itemTemplateId)
			{
				return true;
			}
		}
		return false;
	}

	public ItemKey GetWuYingFromCharacterInventory(GameData.Domains.Character.Character character)
	{
		Inventory inventory = character.GetInventory();
		foreach (ItemKey trackedSpecialItem in _trackedSpecialItems)
		{
			if (12 != trackedSpecialItem.ItemType || 225 != trackedSpecialItem.TemplateId || !inventory.Items.ContainsKey(trackedSpecialItem))
			{
				continue;
			}
			return trackedSpecialItem;
		}
		return ItemKey.Invalid;
	}

	public static short GetRandomEquipmentEffect(IRandomSource random, sbyte itemType)
	{
		if (!_categorizedEquipmentEffects.CheckIndex(itemType) || _categorizedEquipmentEffects[itemType].Count == 0)
		{
			return -1;
		}
		return _categorizedEquipmentEffects[itemType].GetRandom(random);
	}

	public static TemplateKey GetRandomItemGroupIdByEffect(IRandomSource random, sbyte effectId)
	{
		List<TemplateKey> list = _skillBreakPlateBonusEffects[effectId];
		return (list.Count >= 0) ? list.GetRandom(random) : TemplateKey.Invalid;
	}

	public static bool IsValidEquipmentEffectForItemType(sbyte itemType, short equipmentEffect)
	{
		return _categorizedEquipmentEffects[itemType].Contains(equipmentEffect);
	}

	public static bool CanItemBeLost(ItemKey itemKey)
	{
		if (!ItemTemplateHelper.IsTransferable(itemKey.ItemType, itemKey.TemplateId))
		{
			return false;
		}
		if (ModificationStateHelper.IsActive(itemKey.ModificationState, 4))
		{
			return false;
		}
		return true;
	}

	public ItemKey GetDefaultWeaponItemKey(DataContext context, short templateId)
	{
		switch (templateId)
		{
		case 0:
			if (!_emptyHandKey.IsValid())
			{
				SetEmptyHandKey(CreateWeapon(context, templateId, 0), context);
			}
			return _emptyHandKey;
		case 1:
			if (!_branchKey.IsValid())
			{
				SetBranchKey(CreateWeapon(context, templateId, 0), context);
			}
			return _branchKey;
		case 2:
			if (!_stoneKey.IsValid())
			{
				SetStoneKey(CreateWeapon(context, templateId, 0), context);
			}
			return _stoneKey;
		default:
			throw new Exception($"{templateId} is not a valid default weapon template id.");
		}
	}

	public ItemBase GetBaseItem(ItemKey itemKey)
	{
		int id = itemKey.Id;
		sbyte itemType = itemKey.ItemType;
		if (1 == 0)
		{
		}
		ItemBase result = itemType switch
		{
			0 => _weapons[id], 
			1 => _armors[id], 
			2 => _accessories[id], 
			3 => _clothing[id], 
			4 => _carriers[id], 
			5 => _materials[id], 
			6 => _craftTools[id], 
			7 => _foods[id], 
			8 => _medicines[id], 
			9 => _teaWines[id], 
			10 => _skillBooks[id], 
			11 => _crickets[id], 
			12 => _misc[id], 
			_ => throw ItemTemplateHelper.CreateItemTypeException(itemKey.ItemType), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public ItemBase TryGetBaseItem(ItemKey itemKey)
	{
		int id = itemKey.Id;
		sbyte itemType = itemKey.ItemType;
		if (1 == 0)
		{
		}
		Weapon value;
		Armor value2;
		Accessory value3;
		Clothing value4;
		Carrier value5;
		Material value6;
		CraftTool value7;
		Food value8;
		Medicine value9;
		TeaWine value10;
		SkillBook value11;
		Cricket value12;
		Misc value13;
		ItemBase result = itemType switch
		{
			0 => _weapons.TryGetValue(id, out value) ? value : null, 
			1 => _armors.TryGetValue(id, out value2) ? value2 : null, 
			2 => _accessories.TryGetValue(id, out value3) ? value3 : null, 
			3 => _clothing.TryGetValue(id, out value4) ? value4 : null, 
			4 => _carriers.TryGetValue(id, out value5) ? value5 : null, 
			5 => _materials.TryGetValue(id, out value6) ? value6 : null, 
			6 => _craftTools.TryGetValue(id, out value7) ? value7 : null, 
			7 => _foods.TryGetValue(id, out value8) ? value8 : null, 
			8 => _medicines.TryGetValue(id, out value9) ? value9 : null, 
			9 => _teaWines.TryGetValue(id, out value10) ? value10 : null, 
			10 => _skillBooks.TryGetValue(id, out value11) ? value11 : null, 
			11 => _crickets.TryGetValue(id, out value12) ? value12 : null, 
			12 => _misc.TryGetValue(id, out value13) ? value13 : null, 
			_ => null, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public bool ItemExists(ItemKey itemKey)
	{
		int id = itemKey.Id;
		sbyte itemType = itemKey.ItemType;
		if (1 == 0)
		{
		}
		bool result = itemType switch
		{
			0 => _weapons.ContainsKey(id), 
			1 => _armors.ContainsKey(id), 
			2 => _accessories.ContainsKey(id), 
			3 => _clothing.ContainsKey(id), 
			4 => _carriers.ContainsKey(id), 
			5 => _materials.ContainsKey(id), 
			6 => _craftTools.ContainsKey(id), 
			7 => _foods.ContainsKey(id), 
			8 => _medicines.ContainsKey(id), 
			9 => _teaWines.ContainsKey(id), 
			10 => _skillBooks.ContainsKey(id), 
			11 => _crickets.ContainsKey(id), 
			12 => _misc.ContainsKey(id), 
			_ => throw ItemTemplateHelper.CreateItemTypeException(itemKey.ItemType), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public EquipmentBase GetBaseEquipment(ItemKey itemKey)
	{
		int id = itemKey.Id;
		sbyte itemType = itemKey.ItemType;
		if (1 == 0)
		{
		}
		EquipmentBase result = itemType switch
		{
			0 => _weapons[id], 
			1 => _armors[id], 
			2 => _accessories[id], 
			3 => _clothing[id], 
			4 => _carriers[id], 
			_ => throw ItemTemplateHelper.CreateItemTypeException(itemKey.ItemType), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public EquipmentBase TryGetBaseEquipment(ItemKey itemKey)
	{
		int id = itemKey.Id;
		sbyte itemType = itemKey.ItemType;
		if (1 == 0)
		{
		}
		Weapon value;
		Armor value2;
		Accessory value3;
		Clothing value4;
		Carrier value5;
		EquipmentBase result = itemType switch
		{
			0 => _weapons.TryGetValue(id, out value) ? value : null, 
			1 => _armors.TryGetValue(id, out value2) ? value2 : null, 
			2 => _accessories.TryGetValue(id, out value3) ? value3 : null, 
			3 => _clothing.TryGetValue(id, out value4) ? value4 : null, 
			4 => _carriers.TryGetValue(id, out value5) ? value5 : null, 
			_ => null, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static bool IsPureStackable(ItemBase item)
	{
		return item.GetStackable() && !ModificationStateHelper.IsAnyActive(item.GetModificationState());
	}

	public int GetCharacterPropertyBonus(ItemKey itemKey, ECharacterPropertyReferencedType propertyType)
	{
		ItemBase baseItem = GetBaseItem(itemKey);
		return baseItem.GetCharacterPropertyBonus(propertyType);
	}

	public static int GetDestroyedDate(ItemKey itemKey)
	{
		short preservationDuration = ItemTemplateHelper.GetPreservationDuration(itemKey.ItemType, itemKey.TemplateId);
		return (preservationDuration >= 0) ? (DomainManager.World.GetCurrDate() + preservationDuration) : int.MaxValue;
	}

	public static int GetDestroyedDate(ItemKey itemKey, int date)
	{
		short preservationDuration = ItemTemplateHelper.GetPreservationDuration(itemKey.ItemType, itemKey.TemplateId);
		return (preservationDuration >= 0) ? (date + preservationDuration) : int.MaxValue;
	}

	public int GetStackableItemIdByTemplateId(sbyte itemType, short templateId)
	{
		int value;
		return _stackableItems.TryGetValue(new TemplateKey(itemType, templateId), out value) ? value : (-1);
	}

	[DomainMethod]
	public int GetValue(ItemKey itemKey)
	{
		return GetBaseItem(itemKey).GetValue();
	}

	[DomainMethod]
	[Obsolete("Use GetValue Instead.")]
	public int GetPrice(ItemKey itemKey)
	{
		return GetBaseItem(itemKey).GetValue();
	}

	public static short GetRandomItemIdInSubType(IRandomSource random, short itemSubType, sbyte grade)
	{
		if (!ItemSubType.IsHobbyType(itemSubType))
		{
			throw new Exception($"Unsupported itemSubType {itemSubType}");
		}
		sbyte type = ItemSubType.GetType(itemSubType);
		Logger.Info($"Getting item of type {type}({ItemType.TypeId2TypeName[type]}) and sub type {itemSubType} with grade {grade}");
		return _categorizedItemTemplates[grade][itemSubType].GetRandom(random);
	}

	[Obsolete("This method will remove in future, use GetRandomItemIdInSubType instead.")]
	public static short GetRandomItemTemplateId(IRandomSource random, short itemSubType, sbyte grade)
	{
		return GetRandomItemIdInSubType(random, itemSubType, grade);
	}

	public static sbyte GetClosestNeighboringGradeWithValidItem<T>(sbyte grade, List<T> collection, Predicate<(T, sbyte)> matchingFunc)
	{
		int num = ((grade < 4) ? 1 : (-1));
		for (int i = grade; i >= 0 && i < 9; i += num)
		{
			sbyte b = (sbyte)i;
			foreach (T item in collection)
			{
				if (matchingFunc((item, b)))
				{
					return b;
				}
			}
		}
		int num2 = grade - num;
		while (num2 >= 0 && num2 < 9)
		{
			sbyte b2 = (sbyte)num2;
			foreach (T item2 in collection)
			{
				if (matchingFunc((item2, b2)))
				{
					return b2;
				}
			}
			num2 -= num;
		}
		return -1;
	}

	public short GetSwordFragmentCurrSkill(ItemKey itemKey)
	{
		if (itemKey.ItemType != 12 || !GameData.Domains.Combat.SharedConstValue.SwordFragment2BossId.TryGetValue(itemKey.TemplateId, out var value))
		{
			return -1;
		}
		List<short> playerCastSkills = Boss.Instance[value].PlayerCastSkills;
		XiangshuAvatarTaskStatus element_XiangshuAvatarTaskStatuses = DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(value);
		bool flag = FavorabilityType.GetFavorabilityType(DomainManager.World.GetXiangshuAvatarFavorability(value)) >= 4;
		if (element_XiangshuAvatarTaskStatuses.JuniorXiangshuTaskStatus == 6)
		{
			return flag ? playerCastSkills[2] : playerCastSkills[0];
		}
		if (element_XiangshuAvatarTaskStatuses.JuniorXiangshuTaskStatus == 5)
		{
			return flag ? playerCastSkills[3] : playerCastSkills[1];
		}
		return -1;
	}

	public static long GetItemWorth(ItemKey itemKey, int amount)
	{
		long result = 0L;
		short currDurability = DomainManager.Item.GetBaseItem(itemKey).GetCurrDurability();
		if (currDurability > 0)
		{
			int value = DomainManager.Item.GetValue(itemKey);
			result = value * amount;
		}
		return result;
	}

	public ClothingItem GetClothingItemByDisplayId(short displayId)
	{
		foreach (ClothingItem item in (IEnumerable<ClothingItem>)Config.Clothing.Instance)
		{
			if (item.DisplayId == displayId)
			{
				return item;
			}
		}
		return null;
	}

	public List<int> GetAllCarrierIdList()
	{
		return new List<int>(_carriers.Keys);
	}

	[DomainMethod]
	public ItemKey GetEmptyToolKey(DataContext context)
	{
		return DomainManager.Extra.GetEmptyToolKey(context);
	}

	[DomainMethod]
	public int GetRepairItemNeedResourceCount(ItemKey itemKey, short targetDurability = -1)
	{
		ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
		EquipmentBase equipmentBase = DomainManager.Item.TryGetBaseEquipment(itemKey);
		if (equipmentBase == null)
		{
			return 0;
		}
		return ItemTemplateHelper.GetRepairNeedResourceCount(equipmentBase.GetMaterialResources(), itemKey, baseItem.GetCurrDurability());
	}

	[DomainMethod]
	[Obsolete("可以用DisassembleItemOptional替代")]
	public List<ItemDisplayData> DisassembleItem(DataContext context, int charId, ItemKey itemKey, ItemKey toolKey)
	{
		return DisassembleItemOptional(context, charId, itemKey, toolKey, 1, 1);
	}

	[DomainMethod]
	public List<ItemDisplayData> DisassembleItemOptional(DataContext context, int charId, ItemKey itemKey, ItemKey toolKey, sbyte itemSourceType, sbyte toolSourceType)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		ItemBase baseItem = GetBaseItem(itemKey);
		sbyte resourceType = ItemTemplateHelper.GetResourceType(itemKey.ItemType, itemKey.TemplateId);
		if (resourceType == -1)
		{
			return null;
		}
		List<ItemKey> keyList = new List<ItemKey>();
		sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
		int disassembleSameGradeRate = ItemTemplateHelper.GetDisassembleSameGradeRate(grade);
		short disassemblyMaterial = ItemTemplateHelper.GetDisassemblyMaterial(itemKey.ItemType, itemKey.TemplateId, context.Random, disassembleSameGradeRate);
		if (disassemblyMaterial > -1)
		{
			ItemKey itemKey2 = CreateMaterial(context, disassemblyMaterial);
			DomainManager.Taiwu.AddItem(context, itemKey2, 1, itemSourceType);
			keyList.Add(itemKey2);
		}
		if (ModificationStateHelper.IsActive(baseItem.GetModificationState(), 2))
		{
			DomainManager.Item.GetRefinedEffects(itemKey).GetAllMaterialTemplateIds()?.ForEach(delegate(int i, short materialId)
			{
				if (materialId <= -1)
				{
					return false;
				}
				ItemKey itemKey4 = CreateMaterial(context, materialId);
				DomainManager.Taiwu.AddItem(context, itemKey4, 1, itemSourceType);
				keyList.Add(itemKey4);
				return false;
			});
		}
		if (ItemType.IsEquipmentItemType(itemKey.ItemType))
		{
			EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
			ResourceInts delta = ItemTemplateHelper.GetDisassembleResources(baseEquipment.GetMaterialResources(), itemKey.ItemType, itemKey.TemplateId, 1);
			element_Objects.ChangeResources(context, ref delta);
		}
		else if (itemKey.ItemType == 12)
		{
			MiscItem miscItem = Config.Misc.Instance[itemKey.TemplateId];
			MaterialResources materialResources = MakeItemSubType.Instance[miscItem.MakeItemSubType]?.MaxMaterialResources ?? default(MaterialResources);
			ResourceInts delta2 = ItemTemplateHelper.GetDisassembleResources(materialResources, itemKey.ItemType, itemKey.TemplateId, 1);
			element_Objects.ChangeResources(context, ref delta2);
		}
		else
		{
			ResourceInts delta3 = ItemTemplateHelper.GetDisassembleResources(default(MaterialResources), itemKey.ItemType, itemKey.TemplateId, 1);
			element_Objects.ChangeResources(context, ref delta3);
			if (itemKey.ItemType == 5)
			{
				MaterialItem materialItem = Config.Material.Instance[itemKey.TemplateId];
				List<PresetInventoryItem> disassembleResultItemList = materialItem.DisassembleResultItemList;
				if (disassembleResultItemList != null && disassembleResultItemList.Count > 0 && materialItem.DisassembleResultCount > 0)
				{
					int num = 0;
					List<PresetInventoryItem> list = new List<PresetInventoryItem>();
					list.AddRange(materialItem.DisassembleResultItemList);
					for (int num2 = 0; num2 < materialItem.DisassembleResultItemList.Count; num2++)
					{
						PresetInventoryItem presetInventoryItem = materialItem.DisassembleResultItemList[num2];
						int amount = presetInventoryItem.Amount;
						num += amount;
						list[num2] = new PresetInventoryItem(presetInventoryItem.Type, presetInventoryItem.TemplateId, num, 100);
					}
					for (int num3 = 0; num3 < materialItem.DisassembleResultCount; num3++)
					{
						int random = context.Random.Next(list.Last().Amount) + 1;
						int index = list.FindIndex((PresetInventoryItem r) => random <= r.Amount);
						PresetInventoryItem presetInventoryItem2 = list[index];
						ItemKey itemKey3 = DomainManager.Item.CreateItem(context, presetInventoryItem2.Type, presetInventoryItem2.TemplateId);
						keyList.Add(itemKey3);
						DomainManager.Taiwu.AddItem(context, itemKey3, 1, itemSourceType);
					}
				}
				int value = DomainManager.Item.GetValue(itemKey);
				int baseDelta = ProfessionFormulaImpl.Calculate(4, value);
				DomainManager.Extra.ChangeProfessionSeniority(context, 0, baseDelta);
			}
		}
		if (itemKey.ItemType != 5)
		{
			sbyte craftRequiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(itemKey.ItemType, itemKey.TemplateId);
			if (((uint)(craftRequiredLifeSkillType - 6) <= 1u || (uint)(craftRequiredLifeSkillType - 10) <= 1u) ? true : false)
			{
				int baseDelta2 = ProfessionFormulaImpl.Calculate(17, grade);
				DomainManager.Extra.ChangeProfessionSeniority(context, 2, baseDelta2);
			}
		}
		DomainManager.Taiwu.RemoveItem(context, itemKey, 1, itemSourceType, deleteItem: true);
		if (toolKey.IsValid())
		{
			CraftToolItem craftToolItem = Config.CraftTool.Instance[toolKey.TemplateId];
			short num4 = craftToolItem.DurabilityCost[grade];
			if (num4 > 0)
			{
				ReduceToolDurability(context, charId, toolKey, num4, toolSourceType);
			}
		}
		if (keyList.Count > 0)
		{
			return GetItemDisplayDataListOptional(keyList, charId, itemSourceType, merge: true);
		}
		return null;
	}

	[DomainMethod]
	public List<ItemDisplayData> DisassembleItemList(DataContext context, int charId, List<MultiplyOperation> operationList)
	{
		List<ItemDisplayData> list = new List<ItemDisplayData>();
		foreach (MultiplyOperation operation in operationList)
		{
			for (int i = 0; i < operation.Count; i++)
			{
				List<ItemDisplayData> list2 = DisassembleItemOptional(context, charId, operation.Target, operation.Tool, operation.TargetItemSourceType, operation.ToolItemSourceType);
				if (list2 == null || list2.Count <= 0)
				{
					continue;
				}
				foreach (ItemDisplayData newItem in list2)
				{
					ItemDisplayData itemDisplayData = list.Find((ItemDisplayData itemDisplayData2) => itemDisplayData2.Key.TemplateEquals(newItem.Key));
					if (itemDisplayData == null)
					{
						list.Add(newItem);
					}
					else
					{
						itemDisplayData.Amount += newItem.Amount;
					}
				}
			}
		}
		return list;
	}

	[DomainMethod]
	[Obsolete("可以使用DiscardItemOptional替代")]
	public void DiscardItem(DataContext context, int charId, ItemKey itemKey, int count = 1)
	{
		DiscardItemOptional(context, charId, itemKey, 1, count);
	}

	[DomainMethod]
	public void DiscardItemOptional(DataContext context, int charId, ItemKey itemKey, sbyte itemSourceType, int count = 1)
	{
		if (ItemTemplateHelper.IsMiscResource(itemKey.ItemType, itemKey.TemplateId))
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
			sbyte miscResourceType = ItemTemplateHelper.GetMiscResourceType(itemKey.ItemType, itemKey.TemplateId);
			element_Objects.ChangeResourceWithoutChecking(context, miscResourceType, -count);
		}
		else
		{
			DomainManager.Taiwu.RemoveItem(context, itemKey, count, itemSourceType, deleteItem: true);
		}
	}

	[DomainMethod]
	[Obsolete("可以使用DiscardItemListOptional替代")]
	public void DiscardItemList(DataContext context, int charId, List<ItemKey> keyList)
	{
		DiscardItemListOptional(context, charId, keyList, 1);
	}

	[DomainMethod]
	public void DiscardItemListOptional(DataContext context, int charId, List<ItemKey> keyList, sbyte itemSourceType)
	{
		Tester.Assert(keyList != null);
		Tester.Assert(keyList.Count > 0);
		foreach (ItemKey key in keyList)
		{
			DiscardItemOptional(context, charId, key, itemSourceType);
		}
	}

	[DomainMethod]
	public List<ItemKey> GetRepairableItems(DataContext context, int charId, ItemKey toolKey)
	{
		List<ItemKey> ret = new List<ItemKey>();
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		Dictionary<ItemKey, int> items = element_Objects.GetInventory().Items;
		if (!toolKey.TemplateEquals(DomainManager.Item.GetEmptyToolKey(context)))
		{
			if (!items.ContainsKey(toolKey))
			{
				return ret;
			}
			CraftTool element_CraftTools = GetElement_CraftTools(toolKey.Id);
			if (element_CraftTools.GetCurrDurability() <= 0)
			{
				return ret;
			}
		}
		List<sbyte> toolRequiredLifeSkillTypes = Config.CraftTool.Instance[toolKey.TemplateId].RequiredLifeSkillTypes;
		foreach (var (itemKey2, _) in items)
		{
			Do(itemKey2);
		}
		ItemKey[] equipment = element_Objects.GetEquipment();
		foreach (ItemKey itemKey3 in equipment)
		{
			Do(itemKey3);
		}
		return ret;
		void Do(ItemKey itemKey4)
		{
			if (itemKey4.IsValid() && DomainManager.Item.CheckItemNeedRepair(itemKey4))
			{
				sbyte craftRequiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(itemKey4.ItemType, itemKey4.TemplateId);
				if (toolRequiredLifeSkillTypes.Contains(craftRequiredLifeSkillType))
				{
					ret.Add(itemKey4);
				}
			}
		}
	}

	[DomainMethod]
	public List<ItemKey> GetDisassemblableItems(DataContext context, int charId, ItemKey toolKey)
	{
		List<ItemKey> ret = new List<ItemKey>();
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		Dictionary<ItemKey, int> items = element_Objects.GetInventory().Items;
		if (!toolKey.TemplateEquals(DomainManager.Item.GetEmptyToolKey(context)))
		{
			if (!items.ContainsKey(toolKey))
			{
				return ret;
			}
			CraftTool element_CraftTools = GetElement_CraftTools(toolKey.Id);
			if (element_CraftTools.GetCurrDurability() <= 0)
			{
				return ret;
			}
		}
		List<sbyte> toolRequiredLifeSkillTypes = Config.CraftTool.Instance[toolKey.TemplateId].RequiredLifeSkillTypes;
		foreach (var (itemKey2, _) in items)
		{
			Do(itemKey2);
		}
		ItemKey[] equipment = element_Objects.GetEquipment();
		foreach (ItemKey itemKey3 in equipment)
		{
			Do(itemKey3);
		}
		return ret;
		void Do(ItemKey itemKey4)
		{
			if (itemKey4.IsValid() && ItemTemplateHelper.GetCanDisassemble(itemKey4.ItemType, itemKey4.TemplateId))
			{
				sbyte craftRequiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(itemKey4.ItemType, itemKey4.TemplateId);
				if (toolRequiredLifeSkillTypes.Contains(craftRequiredLifeSkillType))
				{
					ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey4);
					ret.Add(baseItem.GetItemKey());
				}
			}
		}
	}

	public bool CheckItemNeedRepair(ItemKey itemKey)
	{
		if (!itemKey.IsValid())
		{
			return false;
		}
		ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
		return baseItem.GetRepairable() && baseItem.GetCurrDurability() < baseItem.GetMaxDurability();
	}

	public void ReduceToolDurability(DataContext context, int charId, ItemKey toolKey, int reduceValue, sbyte itemSourceType)
	{
		if (ItemTemplateHelper.IsEmptyTool(toolKey.ItemType, toolKey.TemplateId))
		{
			return;
		}
		CraftTool element_CraftTools = DomainManager.Item.GetElement_CraftTools(toolKey.Id);
		int num = Math.Max(0, element_CraftTools.GetCurrDurability() - reduceValue);
		element_CraftTools.SetCurrDurability((short)num, context);
		if (element_CraftTools.GetCurrDurability() <= 0)
		{
			if (charId == DomainManager.Taiwu.GetTaiwuCharId())
			{
				DomainManager.Taiwu.RemoveItem(context, toolKey, 1, itemSourceType, deleteItem: true);
				return;
			}
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
			element_Objects.RemoveInventoryItem(context, toolKey, 1, deleteItem: true);
		}
	}

	private void InitializeCreationTemplateIds()
	{
		Cricket.InitializeCricketWeights();
	}

	public ItemKey CreateItem(DataContext context, sbyte itemType, short templateId)
	{
		if (ItemTemplateHelper.IsStackable(itemType, templateId))
		{
			int stackableItem = GetStackableItem(context, itemType, templateId);
			return new ItemKey(itemType, 0, templateId, stackableItem);
		}
		ItemBase itemBase = CreateItemInternal(context, itemType, templateId);
		return itemBase.GetItemKey();
	}

	public ItemKey CreateCopyOfItem(DataContext context, ItemKey srcItemKey)
	{
		if (ItemTemplateHelper.IsPureStackable(srcItemKey))
		{
			return srcItemKey;
		}
		IRandomSource random = context.Random;
		int num = GenerateNextItemId(context);
		CopyModificationState(context, srcItemKey, num);
		switch (srcItemKey.ItemType)
		{
		case 0:
		{
			Weapon element_Weapons = GetElement_Weapons(srcItemKey.Id);
			Weapon weapon = GameData.Serializer.Serializer.CreateCopy(element_Weapons);
			weapon.OfflineSetId(num);
			weapon.OfflineSetEquippedCharId(-1);
			AddElement_Weapons(num, weapon);
			return weapon.GetItemKey();
		}
		case 1:
		{
			Armor element_Armors = GetElement_Armors(srcItemKey.Id);
			Armor armor = GameData.Serializer.Serializer.CreateCopy(element_Armors);
			armor.OfflineSetId(num);
			armor.OfflineSetEquippedCharId(-1);
			AddElement_Armors(num, armor);
			return armor.GetItemKey();
		}
		case 2:
		{
			Accessory element_Accessories = GetElement_Accessories(srcItemKey.Id);
			Accessory accessory = GameData.Serializer.Serializer.CreateCopy(element_Accessories);
			accessory.OfflineSetId(num);
			accessory.OfflineSetEquippedCharId(-1);
			AddElement_Accessories(num, accessory);
			return accessory.GetItemKey();
		}
		case 3:
		{
			Clothing element_Clothing = GetElement_Clothing(srcItemKey.Id);
			Clothing clothing = GameData.Serializer.Serializer.CreateCopy(element_Clothing);
			clothing.OfflineSetId(num);
			clothing.OfflineSetEquippedCharId(-1);
			AddElement_Clothing(num, clothing);
			return clothing.GetItemKey();
		}
		case 4:
		{
			Carrier element_Carriers = GetElement_Carriers(srcItemKey.Id);
			Carrier carrier = GameData.Serializer.Serializer.CreateCopy(element_Carriers);
			carrier.OfflineSetId(num);
			carrier.OfflineSetEquippedCharId(-1);
			AddElement_Carriers(num, carrier);
			ItemKey itemKey2 = carrier.GetItemKey();
			int id3;
			ChildrenOfLoong childOfLoong;
			if (DomainManager.Extra.TryGetJiaoIdByItemKey(srcItemKey, out var id2) && DomainManager.Extra.TryGetJiao(id2, out var jiao2))
			{
				DomainManager.Extra.AddCopyOfJiao(context, jiao2, itemKey2);
			}
			else if (DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(srcItemKey, out id3) && DomainManager.Extra.TryGetLoong(id3, out childOfLoong))
			{
				DomainManager.Extra.AddCopyOfChildOfLoong(context, childOfLoong, itemKey2);
			}
			return itemKey2;
		}
		case 5:
		{
			Material element_Materials = GetElement_Materials(srcItemKey.Id);
			Material material = GameData.Serializer.Serializer.CreateCopy(element_Materials);
			material.OfflineSetId(num);
			AddElement_Materials(num, material);
			return material.GetItemKey();
		}
		case 6:
		{
			CraftTool element_CraftTools = GetElement_CraftTools(srcItemKey.Id);
			CraftTool craftTool = GameData.Serializer.Serializer.CreateCopy(element_CraftTools);
			craftTool.OfflineSetId(num);
			AddElement_CraftTools(num, craftTool);
			return craftTool.GetItemKey();
		}
		case 7:
		{
			Food element_Foods = GetElement_Foods(srcItemKey.Id);
			Food food = GameData.Serializer.Serializer.CreateCopy(element_Foods);
			food.OfflineSetId(num);
			AddElement_Foods(num, food);
			return food.GetItemKey();
		}
		case 8:
		{
			Medicine element_Medicines = GetElement_Medicines(srcItemKey.Id);
			Medicine medicine = GameData.Serializer.Serializer.CreateCopy(element_Medicines);
			medicine.OfflineSetId(num);
			AddElement_Medicines(num, medicine);
			return medicine.GetItemKey();
		}
		case 9:
		{
			TeaWine element_TeaWines = GetElement_TeaWines(srcItemKey.Id);
			TeaWine teaWine = GameData.Serializer.Serializer.CreateCopy(element_TeaWines);
			teaWine.OfflineSetId(num);
			AddElement_TeaWines(num, teaWine);
			return teaWine.GetItemKey();
		}
		case 10:
		{
			SkillBook element_SkillBooks = GetElement_SkillBooks(srcItemKey.Id);
			SkillBook skillBook = GameData.Serializer.Serializer.CreateCopy(element_SkillBooks);
			skillBook.OfflineSetId(num);
			AddElement_SkillBooks(num, skillBook);
			return skillBook.GetItemKey();
		}
		case 11:
		{
			Cricket element_Crickets = GetElement_Crickets(srcItemKey.Id);
			Cricket cricket = GameData.Serializer.Serializer.CreateCopy(element_Crickets);
			cricket.OfflineSetId(num);
			AddElement_Crickets(num, cricket);
			Events.RaiseCricketCreated(context, cricket.GetItemKey());
			return cricket.GetItemKey();
		}
		case 12:
		{
			Misc element_Misc = GetElement_Misc(srcItemKey.Id);
			Misc misc = GameData.Serializer.Serializer.CreateCopy(element_Misc);
			misc.OfflineSetId(num);
			AddElement_Misc(num, misc);
			ItemKey itemKey = misc.GetItemKey();
			if (DomainManager.Extra.TryGetJiaoIdByItemKey(srcItemKey, out var id) && DomainManager.Extra.TryGetJiao(id, out var jiao))
			{
				DomainManager.Extra.AddCopyOfJiao(context, jiao, itemKey);
			}
			return itemKey;
		}
		default:
			throw ItemTemplateHelper.CreateItemTypeException(srcItemKey.ItemType);
		}
	}

	private void CopyModificationState(DataContext context, ItemKey srcItemKey, int destItemId)
	{
		if (ModificationStateHelper.IsActive(srcItemKey.ModificationState, 2))
		{
			RefiningEffects refinedEffects = GetRefinedEffects(srcItemKey);
			AddElement_RefinedItems(destItemId, refinedEffects, context);
		}
		if (ModificationStateHelper.IsActive(srcItemKey.ModificationState, 1))
		{
			FullPoisonEffects poisonEffects = GetPoisonEffects(srcItemKey);
			DomainManager.Extra.SetPoisonEffect(context, destItemId, new FullPoisonEffects(poisonEffects));
		}
	}

	public ItemKey CreateEquipment(DataContext context, sbyte itemType, short templateId, int spawnSpecialEffectChance)
	{
		if (1 == 0)
		{
		}
		ItemKey itemKey = itemType switch
		{
			0 => CreateWeapon(context, templateId, 0), 
			1 => CreateArmor(context, templateId, 0), 
			3 => CreateClothing(context, templateId, 0), 
			2 => CreateAccessory(context, templateId, 0), 
			4 => CreateCarrier(context, templateId), 
			_ => throw new Exception($"ItemType {itemType} is not an equipment"), 
		};
		if (1 == 0)
		{
		}
		ItemKey itemKey2 = itemKey;
		EquipmentBase baseEquipment = GetBaseEquipment(itemKey2);
		if (context.Random.CheckPercentProb(spawnSpecialEffectChance))
		{
			baseEquipment.OfflineGenerateEquipmentEffect(context.Random);
			baseEquipment.SetEquipmentEffectId(baseEquipment.GetEquipmentEffectId(), context);
			baseEquipment.SetCurrDurability(baseEquipment.GetCurrDurability(), context);
			baseEquipment.SetMaxDurability(baseEquipment.GetMaxDurability(), context);
		}
		return itemKey2;
	}

	public ItemKey CreateWeapon(DataContext context, short templateId, sbyte spawnSpecialEffectMultiplier = 1)
	{
		WeaponItem weaponItem = Config.Weapon.Instance[templateId];
		Tester.Assert(!weaponItem.Stackable);
		int num = GenerateNextItemId(context);
		Weapon weapon = new Weapon(context.Random, templateId, num);
		if (context.Random.CheckPercentProb(GlobalConfig.Instance.EquipmentWithEffectRate * spawnSpecialEffectMultiplier))
		{
			weapon.OfflineGenerateEquipmentEffect(context.Random);
		}
		weapon.OfflineGenerateMaterialResources(context.Random);
		AddElement_Weapons(num, weapon);
		return weapon.GetItemKey();
	}

	public ItemKey CreateArmor(DataContext context, short templateId, sbyte spawnSpecialEffectMultiplier = 1)
	{
		Tester.Assert(!Config.Armor.Instance[templateId].Stackable);
		int num = GenerateNextItemId(context);
		Armor armor = new Armor(context.Random, templateId, num);
		if (context.Random.CheckPercentProb(GlobalConfig.Instance.EquipmentWithEffectRate * spawnSpecialEffectMultiplier))
		{
			armor.OfflineGenerateEquipmentEffect(context.Random);
		}
		armor.OfflineGenerateMaterialResources(context.Random);
		AddElement_Armors(num, armor);
		return armor.GetItemKey();
	}

	public ItemKey CreateAccessory(DataContext context, short templateId, sbyte spawnSpecialEffectMultiplier = 1)
	{
		Tester.Assert(!Config.Accessory.Instance[templateId].Stackable);
		int num = GenerateNextItemId(context);
		Accessory accessory = new Accessory(context.Random, templateId, num);
		if (context.Random.CheckPercentProb(GlobalConfig.Instance.EquipmentWithEffectRate * spawnSpecialEffectMultiplier))
		{
			accessory.OfflineGenerateEquipmentEffect(context.Random);
		}
		accessory.OfflineGenerateMaterialResources(context.Random);
		AddElement_Accessories(num, accessory);
		return accessory.GetItemKey();
	}

	public ItemKey CreateClothing(DataContext context, short templateId, sbyte gender)
	{
		Tester.Assert(!Config.Clothing.Instance[templateId].Stackable);
		int num = GenerateNextItemId(context);
		Clothing clothing = new Clothing(context.Random, templateId, num, gender);
		clothing.OfflineGenerateMaterialResources(context.Random);
		AddElement_Clothing(num, clothing);
		return clothing.GetItemKey();
	}

	public ItemKey CreateCarrier(DataContext context, short templateId)
	{
		Tester.Assert(!Config.Carrier.Instance[templateId].Stackable);
		int num = GenerateNextItemId(context);
		Carrier carrier = new Carrier(context.Random, templateId, num);
		carrier.OfflineGenerateMaterialResources(context.Random);
		AddElement_Carriers(num, carrier);
		return carrier.GetItemKey();
	}

	public ItemKey CreateMaterial(DataContext context, short templateId)
	{
		Tester.Assert(Config.Material.Instance[templateId].Stackable);
		int stackableItem = GetStackableItem(context, 5, templateId);
		return new ItemKey(5, 0, templateId, stackableItem);
	}

	public ItemKey CreateCraftTool(DataContext context, short templateId)
	{
		Tester.Assert(!Config.CraftTool.Instance[templateId].Stackable);
		int num = GenerateNextItemId(context);
		CraftTool craftTool = new CraftTool(context.Random, templateId, num);
		AddElement_CraftTools(num, craftTool);
		return craftTool.GetItemKey();
	}

	public ItemKey CreateFood(DataContext context, short templateId)
	{
		Tester.Assert(Config.Food.Instance[templateId].Stackable);
		int stackableItem = GetStackableItem(context, 7, templateId);
		return new ItemKey(7, 0, templateId, stackableItem);
	}

	public ItemKey CreateMedicine(DataContext context, short templateId)
	{
		Tester.Assert(Config.Medicine.Instance[templateId].Stackable);
		int stackableItem = GetStackableItem(context, 8, templateId);
		return new ItemKey(8, 0, templateId, stackableItem);
	}

	public ItemKey CreateTeaWine(DataContext context, short templateId)
	{
		Tester.Assert(Config.TeaWine.Instance[templateId].Stackable);
		int stackableItem = GetStackableItem(context, 9, templateId);
		return new ItemKey(9, 0, templateId, stackableItem);
	}

	public ItemKey CreateDemandedSkillBook(DataContext context, short templateId, byte ensuredPageIndex, byte pageTypes = 0)
	{
		SkillBookItem skillBookItem = Config.SkillBook.Instance[templateId];
		ItemKey result;
		if (skillBookItem.CombatSkillType >= 0)
		{
			sbyte lostPagesCount = (sbyte)context.Random.Next(6);
			result = DomainManager.Item.CreateSkillBook(context, templateId, pageTypes, 0, lostPagesCount);
		}
		else
		{
			sbyte lostPagesCount2 = (sbyte)context.Random.Next(6);
			result = DomainManager.Item.CreateSkillBook(context, templateId, 0, lostPagesCount2, -1, 50);
		}
		SkillBook skillBook = _skillBooks[result.Id];
		sbyte skillGroup = SkillGroup.FromItemSubType(skillBookItem.ItemSubType);
		sbyte grade = (sbyte)(skillBookItem.Grade + 2);
		ushort pageIncompleteState = SkillBook.GeneratePageIncompleteState(context.Random, skillGroup, grade, -1, -1, outlineAlwaysComplete: true);
		pageIncompleteState = SkillBookStateHelper.SetPageIncompleteState(pageIncompleteState, ensuredPageIndex, 0);
		skillBook.SetPageIncompleteState(pageIncompleteState, context);
		return result;
	}

	public ItemKey CreateSkillBook(DataContext context, short templateId, sbyte completePagesCount = -1, sbyte lostPagesCount = -1, sbyte outlinePageType = -1, sbyte normalPagesDirectProb = 50, bool outlineAlwaysComplete = true)
	{
		Tester.Assert(!Config.SkillBook.Instance[templateId].Stackable);
		int num = GenerateNextItemId(context);
		SkillBook skillBook = new SkillBook(context.Random, templateId, num, completePagesCount, lostPagesCount, outlinePageType, normalPagesDirectProb, outlineAlwaysComplete);
		AddElement_SkillBooks(num, skillBook);
		return skillBook.GetItemKey();
	}

	public ItemKey CreateSkillBook(DataContext context, short templateId, byte pageTypes, sbyte completePagesCount = -1, sbyte lostPagesCount = -1, bool outlineAlwaysComplete = true)
	{
		Tester.Assert(!Config.SkillBook.Instance[templateId].Stackable);
		int num = GenerateNextItemId(context);
		SkillBook skillBook = new SkillBook(context.Random, templateId, num, pageTypes, completePagesCount, lostPagesCount, outlineAlwaysComplete);
		AddElement_SkillBooks(num, skillBook);
		return skillBook.GetItemKey();
	}

	public ItemKey CreateSkillBook(DataContext context, short templateId, ushort activationState)
	{
		Tester.Assert(!Config.SkillBook.Instance[templateId].Stackable);
		int num = GenerateNextItemId(context);
		SkillBook skillBook = new SkillBook(context.Random, templateId, num, activationState);
		AddElement_SkillBooks(num, skillBook);
		return skillBook.GetItemKey();
	}

	public ItemKey CreateCricket(DataContext context, short colorId, short partId)
	{
		int num = GenerateNextItemId(context);
		Cricket cricket = new Cricket(context.Random, colorId, partId, num);
		Tester.Assert(!Config.Cricket.Instance[cricket.GetTemplateId()].Stackable);
		AddElement_Crickets(num, cricket);
		Events.RaiseCricketCreated(context, cricket.GetItemKey());
		return cricket.GetItemKey();
	}

	public ItemKey CreateCricket(DataContext context, short templateId)
	{
		Tester.Assert(!Config.Cricket.Instance[templateId].Stackable);
		int num = GenerateNextItemId(context);
		Cricket cricket = new Cricket(context.Random, templateId, num);
		AddElement_Crickets(num, cricket);
		Events.RaiseCricketCreated(context, cricket.GetItemKey());
		return cricket.GetItemKey();
	}

	public ItemKey CreateCricket(DataContext context, short templateId, bool isSpecial)
	{
		Tester.Assert(!Config.Cricket.Instance[templateId].Stackable);
		int num = GenerateNextItemId(context);
		Cricket cricket = new Cricket(context.Random, templateId, num, isSpecial);
		AddElement_Crickets(num, cricket);
		Events.RaiseCricketCreated(context, cricket.GetItemKey());
		return cricket.GetItemKey();
	}

	public ItemKey CreateMisc(DataContext context, short templateId)
	{
		if (Config.Misc.Instance[templateId].Stackable)
		{
			int stackableItem = GetStackableItem(context, 12, templateId);
			return new ItemKey(12, 0, templateId, stackableItem);
		}
		int num = GenerateNextItemId(context);
		Misc misc = new Misc(context.Random, templateId, num);
		AddElement_Misc(num, misc);
		return misc.GetItemKey();
	}

	public void RemoveItem(DataContext context, ItemKey itemKey)
	{
		if (!ItemTemplateHelper.IsPureStackable(itemKey))
		{
			int id = itemKey.Id;
			_trackedSpecialItems.Remove(itemKey);
			RemoveItemInternal(itemKey.ItemType, id);
			byte modificationState = itemKey.ModificationState;
			if (ModificationStateHelper.IsActive(modificationState, 1))
			{
				RemoveElement_PoisonItems(id, context);
				DomainManager.Extra.RemovePoisonEffect(context, id);
			}
			if (ModificationStateHelper.IsActive(modificationState, 2))
			{
				RemoveElement_RefinedItems(id, context);
			}
			if (ModificationStateHelper.IsActive(modificationState, 4))
			{
				DomainManager.Extra.RemoveLoveTokenData(context, itemKey, itemIsDeleted: true);
			}
		}
	}

	public void ForceRemoveItem(DataContext context, ItemKey itemKey)
	{
		int id = itemKey.Id;
		_trackedSpecialItems.Remove(itemKey);
		RemoveItemInternal(itemKey.ItemType, id);
		if (_poisonItems.ContainsKey(id))
		{
			RemoveElement_PoisonItems(id, context);
		}
		if (DomainManager.Extra.PoisonEffects.ContainsKey(id))
		{
			DomainManager.Extra.RemovePoisonEffect(context, id);
		}
		if (_refinedItems.ContainsKey(id))
		{
			RemoveElement_RefinedItems(id, context);
		}
	}

	public void RemoveItems(DataContext context, List<ItemKey> itemKeys)
	{
		int i = 0;
		for (int count = itemKeys.Count; i < count; i++)
		{
			RemoveItem(context, itemKeys[i]);
		}
	}

	public void RemoveItems(DataContext context, Dictionary<ItemKey, int> items)
	{
		foreach (var (itemKey2, _) in items)
		{
			RemoveItem(context, itemKey2);
		}
	}

	public void RemoveItems(DataContext context, List<(ItemKey, int)> items)
	{
		foreach (var item2 in items)
		{
			ItemKey item = item2.Item1;
			RemoveItem(context, item);
		}
	}

	public static short GenerateRandomItemTemplateId(IRandomSource random, sbyte itemType, short groupBeginId, sbyte expectedGrade)
	{
		sbyte b = GenerateRandomItemGrade(random, expectedGrade);
		if (itemType == 8 && Config.Medicine.Instance[groupBeginId].ItemSubType == 800)
		{
			b = GetRandomMedicineGrade(b);
			return (short)(groupBeginId + b);
		}
		return ItemTemplateHelper.GetTemplateIdInGroup(itemType, groupBeginId, b);
	}

	public static sbyte GenerateRandomItemGrade(IRandomSource random, sbyte itemGrade)
	{
		int num = itemGrade + -2;
		return (sbyte)RedzenHelper.SkewDistribute(random, num, 0.333333f, 3f, 0, itemGrade);
	}

	public static sbyte GetRandomMedicineGrade(sbyte generatedGrade)
	{
		generatedGrade = (sbyte)(generatedGrade / 2 + 1);
		return (sbyte)((generatedGrade > 5) ? 5 : generatedGrade);
	}

	private int GenerateNextItemId(DataContext context)
	{
		int nextItemId = _nextItemId;
		_nextItemId++;
		if ((uint)_nextItemId > 2147483647u)
		{
			_nextItemId = 0;
		}
		SetNextItemId(_nextItemId, context);
		return nextItemId;
	}

	private int GetStackableItem(DataContext context, sbyte itemType, short templateId)
	{
		TemplateKey templateKey = new TemplateKey(itemType, templateId);
		if (_stackableItems.TryGetValue(templateKey, out var value))
		{
			return value;
		}
		ItemBase itemBase = CreateItemInternal(context, itemType, templateId);
		Tester.Assert(itemBase.GetModificationState() == 0);
		value = itemBase.GetId();
		AddElement_StackableItems(templateKey, value, context);
		return value;
	}

	public ItemBase CreateUniqueStackableItem(DataContext context, sbyte itemType, short templateId)
	{
		ItemBase itemBase = CreateItemInternal(context, itemType, templateId);
		Tester.Assert(itemBase.GetModificationState() == 0);
		return itemBase;
	}

	private ItemBase CreateItemInternal(DataContext context, sbyte itemType, short templateId)
	{
		IRandomSource random = context.Random;
		int num = GenerateNextItemId(context);
		switch (itemType)
		{
		case 0:
		{
			Weapon weapon = new Weapon(random, templateId, num);
			if (context.Random.CheckPercentProb(GlobalConfig.Instance.EquipmentWithEffectRate))
			{
				weapon.OfflineGenerateEquipmentEffect(context.Random);
			}
			weapon.OfflineGenerateMaterialResources(context.Random);
			AddElement_Weapons(num, weapon);
			return weapon;
		}
		case 1:
		{
			Armor armor = new Armor(random, templateId, num);
			if (context.Random.CheckPercentProb(GlobalConfig.Instance.EquipmentWithEffectRate))
			{
				armor.OfflineGenerateEquipmentEffect(context.Random);
			}
			armor.OfflineGenerateMaterialResources(context.Random);
			AddElement_Armors(num, armor);
			return armor;
		}
		case 2:
		{
			Accessory accessory = new Accessory(random, templateId, num);
			if (context.Random.CheckPercentProb(GlobalConfig.Instance.EquipmentWithEffectRate))
			{
				accessory.OfflineGenerateEquipmentEffect(context.Random);
			}
			accessory.OfflineGenerateMaterialResources(context.Random);
			AddElement_Accessories(num, accessory);
			return accessory;
		}
		case 3:
		{
			Clothing clothing = new Clothing(random, templateId, num, -1);
			clothing.OfflineGenerateMaterialResources(context.Random);
			AddElement_Clothing(num, clothing);
			return clothing;
		}
		case 4:
		{
			Carrier carrier = new Carrier(random, templateId, num);
			carrier.OfflineGenerateMaterialResources(context.Random);
			AddElement_Carriers(num, carrier);
			Events.RaiseCarrierCreated(context, carrier.GetItemKey());
			return carrier;
		}
		case 5:
		{
			Material material = new Material(random, templateId, num);
			AddElement_Materials(num, material);
			return material;
		}
		case 6:
		{
			CraftTool craftTool = new CraftTool(random, templateId, num);
			AddElement_CraftTools(num, craftTool);
			return craftTool;
		}
		case 7:
		{
			Food food = new Food(random, templateId, num);
			AddElement_Foods(num, food);
			return food;
		}
		case 8:
		{
			Medicine medicine = new Medicine(random, templateId, num);
			AddElement_Medicines(num, medicine);
			return medicine;
		}
		case 9:
		{
			TeaWine teaWine = new TeaWine(random, templateId, num);
			AddElement_TeaWines(num, teaWine);
			return teaWine;
		}
		case 10:
		{
			SkillBook skillBook = new SkillBook(random, templateId, num, -1, -1, -1, 50);
			AddElement_SkillBooks(num, skillBook);
			return skillBook;
		}
		case 11:
		{
			Cricket cricket = new Cricket(random, templateId, num);
			AddElement_Crickets(num, cricket);
			Events.RaiseCricketCreated(context, cricket.GetItemKey());
			return cricket;
		}
		case 12:
		{
			Misc misc = new Misc(random, templateId, num);
			AddElement_Misc(num, misc);
			CheckAndTrackSpecialItem(misc.GetItemKey());
			return misc;
		}
		default:
			throw ItemTemplateHelper.CreateItemTypeException(itemType);
		}
	}

	private void RemoveItemInternal(sbyte itemType, int itemId)
	{
		switch (itemType)
		{
		case 0:
			RemoveElement_Weapons(itemId);
			break;
		case 1:
			RemoveElement_Armors(itemId);
			break;
		case 2:
			RemoveElement_Accessories(itemId);
			break;
		case 3:
			RemoveElement_Clothing(itemId);
			break;
		case 4:
			Events.RaiseCarrierRemoved(DataContextManager.GetCurrentThreadDataContext(), GetElement_Carriers(itemId).GetItemKey());
			RemoveElement_Carriers(itemId);
			break;
		case 5:
			RemoveElement_Materials(itemId);
			break;
		case 6:
			RemoveElement_CraftTools(itemId);
			break;
		case 7:
			RemoveElement_Foods(itemId);
			break;
		case 8:
			RemoveElement_Medicines(itemId);
			break;
		case 9:
			RemoveElement_TeaWines(itemId);
			break;
		case 10:
			RemoveElement_SkillBooks(itemId);
			break;
		case 11:
			Events.RaiseCricketRemoved(DataContextManager.GetCurrentThreadDataContext(), GetElement_Crickets(itemId).GetItemKey());
			RemoveElement_Crickets(itemId);
			break;
		case 12:
			RemoveElement_Misc(itemId);
			break;
		default:
			throw ItemTemplateHelper.CreateItemTypeException(itemType);
		}
	}

	public bool IsInStackableItems(ItemKey itemKey)
	{
		TemplateKey key = new TemplateKey(itemKey.ItemType, itemKey.TemplateId);
		int value;
		return _stackableItems.TryGetValue(key, out value) && itemKey.Id == value;
	}

	public override void UnpackCrossArchiveGameData(DataContext context, CrossArchiveGameData crossArchiveGameData)
	{
		foreach (KeyValuePair<int, ItemBase> item in crossArchiveGameData.ItemGroupPackage.Items)
		{
			UnpackCrossArchiveItem(context, crossArchiveGameData, item.Key);
		}
		crossArchiveGameData.ItemGroupPackage = null;
	}

	internal void PackCrossArchiveItem(CrossArchiveGameData crossArchiveGameData, ItemKey itemKey)
	{
		if (ItemExists(itemKey))
		{
			ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
			baseItem.ResetOwner();
			if (crossArchiveGameData.ItemGroupPackage == null)
			{
				crossArchiveGameData.ItemGroupPackage = new ItemGroupPackage();
			}
			PackItem(crossArchiveGameData.ItemGroupPackage, baseItem);
		}
	}

	internal ItemKey UnpackCrossArchiveItem(DataContext context, CrossArchiveGameData crossArchiveGameData, ItemKey srcItemKey)
	{
		return UnpackCrossArchiveItem(context, crossArchiveGameData, srcItemKey.Id);
	}

	internal ItemKey UnpackCrossArchiveItem(DataContext context, CrossArchiveGameData crossArchiveGameData, int srcItemId)
	{
		if (crossArchiveGameData.UnpackedItems == null)
		{
			crossArchiveGameData.UnpackedItems = new Dictionary<int, ItemKey>();
		}
		if (crossArchiveGameData.UnpackedItems.TryGetValue(srcItemId, out var value))
		{
			return value;
		}
		if (crossArchiveGameData.ItemGroupPackage == null)
		{
			return ItemKey.Invalid;
		}
		ItemKey itemKey = UnpackItem(context, crossArchiveGameData.ItemGroupPackage, srcItemId);
		if (itemKey.IsValid())
		{
			crossArchiveGameData.UnpackedItems.Add(srcItemId, itemKey);
			if (itemKey.ItemType == 11)
			{
				crossArchiveGameData.CricketCombatPlans.ReplaceCricket(srcItemId, itemKey);
			}
		}
		return itemKey;
	}

	public void PackItem(ItemGroupPackage package, ItemBase item)
	{
		ItemKey itemKey = item.GetItemKey();
		if (!package.Items.TryAdd(itemKey.Id, item))
		{
			return;
		}
		if (ModificationStateHelper.IsActive(itemKey.ModificationState, 2))
		{
			RefiningEffects refinedEffects = DomainManager.Item.GetRefinedEffects(itemKey);
			ItemGroupPackage itemGroupPackage = package;
			if (itemGroupPackage.RefiningEffects == null)
			{
				itemGroupPackage.RefiningEffects = new Dictionary<int, RefiningEffects>();
			}
			package.RefiningEffects.Add(itemKey.Id, refinedEffects);
		}
		if (ModificationStateHelper.IsActive(itemKey.ModificationState, 1))
		{
			FullPoisonEffects poisonEffects = DomainManager.Item.GetPoisonEffects(itemKey);
			ItemGroupPackage itemGroupPackage = package;
			if (itemGroupPackage.FullPoisonEffects == null)
			{
				itemGroupPackage.FullPoisonEffects = new Dictionary<int, FullPoisonEffects>();
			}
			package.FullPoisonEffects.Add(itemKey.Id, new FullPoisonEffects(poisonEffects));
		}
		switch (itemKey.ItemType)
		{
		case 11:
		{
			bool flag = DomainManager.Extra.IsCricketSmart(itemKey.Id);
			bool flag2 = DomainManager.Extra.IsCricketIdentified(itemKey.Id);
			if (flag)
			{
				package.CricketIsSmart.Add(itemKey.Id, value: true);
			}
			if (flag2)
			{
				package.CricketIsIdentified.Add(itemKey.Id, value: true);
			}
			break;
		}
		case 4:
		{
			int carrierTamePoint = DomainManager.Extra.GetCarrierTamePoint(itemKey.Id);
			if (carrierTamePoint >= 0)
			{
				package.CarrierTamePoint.Add(itemKey.Id, carrierTamePoint);
			}
			int id3;
			ChildrenOfLoong childOfLoong;
			if (DomainManager.Extra.TryGetJiaoIdByItemKey(itemKey, out var id2) && DomainManager.Extra.TryGetJiao(id2, out var jiao2))
			{
				jiao2.PettingCoolDown = -1;
				package.Jiaos.Add(id2, jiao2);
				package.JiaoKeyToId.Add(itemKey, id2);
			}
			else if (DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(itemKey, out id3) && DomainManager.Extra.TryGetLoong(id3, out childOfLoong))
			{
				package.ChildrenOfLoong.Add(id3, childOfLoong);
				package.ChildrenOfLoongKeyToId.Add(itemKey, id3);
			}
			break;
		}
		case 5:
		{
			if (DomainManager.Extra.TryGetJiaoIdByItemKey(itemKey, out var id) && DomainManager.Extra.TryGetJiao(id, out var jiao))
			{
				jiao.PettingCoolDown = -1;
				package.Jiaos.Add(id, jiao);
				package.JiaoKeyToId.Add(itemKey, id);
			}
			break;
		}
		case 3:
		{
			short modifiedClothingTemplateId = DomainManager.Extra.GetModifiedClothingTemplateId(itemKey);
			if (modifiedClothingTemplateId != itemKey.TemplateId)
			{
				ItemGroupPackage itemGroupPackage = package;
				if (itemGroupPackage.ClothingDisplayModifications == null)
				{
					itemGroupPackage.ClothingDisplayModifications = new Dictionary<int, short>();
				}
				package.ClothingDisplayModifications.Add(itemKey.Id, modifiedClothingTemplateId);
			}
			break;
		}
		}
	}

	public ItemKey UnpackItem(DataContext context, ItemGroupPackage package, int srcItemId)
	{
		if (package.Items == null || !package.Items.TryGetValue(srcItemId, out var value))
		{
			return ItemKey.Invalid;
		}
		ItemKey itemKey = value.GetItemKey();
		if (ItemTemplateHelper.IsPureStackable(itemKey))
		{
			ItemKey result = itemKey;
			result.Id = GetStackableItem(context, itemKey.ItemType, itemKey.TemplateId);
			return result;
		}
		int num = GenerateNextItemId(context);
		if (ModificationStateHelper.IsActive(itemKey.ModificationState, 2))
		{
			RefiningEffects value2 = package.RefiningEffects[srcItemId];
			AddElement_RefinedItems(num, value2, context);
		}
		if (ModificationStateHelper.IsActive(itemKey.ModificationState, 1))
		{
			FullPoisonEffects other = package.FullPoisonEffects[srcItemId];
			DomainManager.Extra.SetPoisonEffect(context, num, new FullPoisonEffects(other));
		}
		switch (itemKey.ItemType)
		{
		case 0:
		{
			Weapon weapon = GameData.Serializer.Serializer.CreateCopy((Weapon)value);
			weapon.OfflineSetId(num);
			weapon.OfflineSetEquippedCharId(-1);
			AddElement_Weapons(num, weapon);
			return weapon.GetItemKey();
		}
		case 1:
		{
			Armor armor = GameData.Serializer.Serializer.CreateCopy((Armor)value);
			armor.OfflineSetId(num);
			armor.OfflineSetEquippedCharId(-1);
			AddElement_Armors(num, armor);
			return armor.GetItemKey();
		}
		case 2:
		{
			Accessory accessory = GameData.Serializer.Serializer.CreateCopy((Accessory)value);
			accessory.OfflineSetId(num);
			accessory.OfflineSetEquippedCharId(-1);
			AddElement_Accessories(num, accessory);
			return accessory.GetItemKey();
		}
		case 3:
		{
			Clothing clothing = GameData.Serializer.Serializer.CreateCopy((Clothing)value);
			clothing.OfflineSetId(num);
			clothing.OfflineSetEquippedCharId(-1);
			AddElement_Clothing(num, clothing);
			ItemKey itemKey5 = clothing.GetItemKey();
			if (package.ClothingDisplayModifications != null && package.ClothingDisplayModifications.TryGetValue(srcItemId, out var value7))
			{
				DomainManager.Extra.SetClothingDisplayModification(context, itemKey5, value7);
			}
			return itemKey5;
		}
		case 4:
		{
			Carrier carrier = GameData.Serializer.Serializer.CreateCopy((Carrier)value);
			carrier.OfflineSetId(num);
			carrier.OfflineSetEquippedCharId(-1);
			AddElement_Carriers(num, carrier);
			ItemKey itemKey7 = carrier.GetItemKey();
			if (package.CarrierTamePoint.TryGetValue(itemKey.Id, out var value8))
			{
				DomainManager.Extra.SetCarrierTamePoint(context, itemKey7.Id, value8);
			}
			ChildrenOfLoong value11;
			if (package.JiaoKeyToId.TryGetValue(itemKey, out var value9) && package.Jiaos.TryGetValue(value9, out var value10))
			{
				DomainManager.Extra.SetJiaoItemKey(context, value9, value10, itemKey7);
			}
			else if (package.ChildrenOfLoongKeyToId.TryGetValue(itemKey, out value9) && package.ChildrenOfLoong.TryGetValue(value9, out value11))
			{
				DomainManager.Extra.SetChildrenOfLoongItemKey(context, value9, value11, itemKey7);
			}
			return itemKey7;
		}
		case 5:
		{
			Material material2 = GameData.Serializer.Serializer.CreateCopy((Material)value);
			material2.OfflineSetId(num);
			AddElement_Materials(num, material2);
			ItemKey itemKey4 = material2.GetItemKey();
			if (package.JiaoKeyToId.TryGetValue(itemKey, out var value5) && package.Jiaos.TryGetValue(value5, out var value6))
			{
				DomainManager.Extra.SetJiaoItemKey(context, value5, value6, itemKey4);
			}
			return itemKey4;
		}
		case 6:
		{
			CraftTool craftTool = GameData.Serializer.Serializer.CreateCopy((CraftTool)value);
			craftTool.OfflineSetId(num);
			AddElement_CraftTools(num, craftTool);
			return craftTool.GetItemKey();
		}
		case 7:
		{
			Food food = GameData.Serializer.Serializer.CreateCopy((Food)value);
			food.OfflineSetId(num);
			AddElement_Foods(num, food);
			return food.GetItemKey();
		}
		case 8:
		{
			Medicine medicine = GameData.Serializer.Serializer.CreateCopy((Medicine)value);
			medicine.OfflineSetId(num);
			AddElement_Medicines(num, medicine);
			return medicine.GetItemKey();
		}
		case 9:
		{
			TeaWine teaWine = GameData.Serializer.Serializer.CreateCopy((TeaWine)value);
			teaWine.OfflineSetId(num);
			AddElement_TeaWines(num, teaWine);
			return teaWine.GetItemKey();
		}
		case 10:
		{
			SkillBook skillBook = GameData.Serializer.Serializer.CreateCopy((SkillBook)value);
			skillBook.OfflineSetId(num);
			AddElement_SkillBooks(num, skillBook);
			return skillBook.GetItemKey();
		}
		case 11:
		{
			Cricket cricket = GameData.Serializer.Serializer.CreateCopy((Cricket)value);
			cricket.OfflineSetId(num);
			AddElement_Crickets(num, cricket);
			Events.RaiseCricketCreated(context, cricket.GetItemKey());
			ItemKey itemKey6 = cricket.GetItemKey();
			if (package.CricketIsSmart.ContainsKey(itemKey.Id))
			{
				DomainManager.Extra.ForceCricketSmart(context, itemKey6);
			}
			if (package.CricketIsIdentified.ContainsKey(itemKey.Id))
			{
				DomainManager.Extra.SetCricketIdentified(context, itemKey6.Id);
			}
			DomainManager.Taiwu.ReplaceCricketPlan(context, itemKey, itemKey6);
			return itemKey6;
		}
		case 12:
		{
			if (package.JiaoKeyToId.TryGetValue(itemKey, out var value3) && package.Jiaos.TryGetValue(value3, out var value4))
			{
				Material material = new Material(context.Random, (value4.GrowthStage == 0) ? DomainManager.Extra.GetJiaoEggTemplateIdByJiaoTemplateId(value4.TemplateId) : DomainManager.Extra.GetJiaoTeenagerTemplateIdByJiaoTemplateId(value4.TemplateId), num);
				AddElement_Materials(num, material);
				ItemKey itemKey2 = material.GetItemKey();
				DomainManager.Extra.SetJiaoItemKey(context, value3, value4, itemKey2);
				return itemKey2;
			}
			Misc misc = GameData.Serializer.Serializer.CreateCopy((Misc)value);
			misc.OfflineSetId(num);
			AddElement_Misc(num, misc);
			ItemKey itemKey3 = misc.GetItemKey();
			CheckAndTrackSpecialItem(itemKey3);
			return itemKey3;
		}
		default:
			throw ItemTemplateHelper.CreateItemTypeException(itemKey.ItemType);
		}
	}

	public void AddExternEquipmentEffect(DataContext context, ItemKey itemKey, short effectId)
	{
		GameData.Utilities.ShortList value;
		bool flag = _externEquipmentEffects.TryGetValue(itemKey.Id, out value);
		ref List<short> items = ref value.Items;
		if (items == null)
		{
			items = new List<short>();
		}
		value.Items.Add(effectId);
		if (flag)
		{
			SetElement_ExternEquipmentEffects(itemKey.Id, value, context);
		}
		else
		{
			AddElement_ExternEquipmentEffects(itemKey.Id, value, context);
		}
		EquipmentBase baseEquipment = GetBaseEquipment(itemKey);
		baseEquipment.SetEquipmentEffectId(baseEquipment.GetEquipmentEffectId(), context);
	}

	public void RemoveExternEquipmentEffect(DataContext context, ItemKey itemKey, short effectId)
	{
		if (TryGetElement_ExternEquipmentEffects(itemKey.Id, out var value))
		{
			value.Items.Remove(effectId);
			if (value.Items.Count > 0)
			{
				SetElement_ExternEquipmentEffects(itemKey.Id, value, context);
			}
			else
			{
				RemoveElement_ExternEquipmentEffects(itemKey.Id, context);
			}
			EquipmentBase equipmentBase = TryGetBaseEquipment(itemKey);
			equipmentBase?.SetEquipmentEffectId(equipmentBase.GetEquipmentEffectId(), context);
		}
	}

	public IEnumerable<EquipmentEffectItem> GetEquipmentEffects(EquipmentBase equipment)
	{
		if (equipment.GetEquipmentEffectId() >= 0)
		{
			yield return EquipmentEffect.Instance[equipment.GetEquipmentEffectId()];
		}
		if (!TryGetElement_ExternEquipmentEffects(equipment.GetId(), out var effectIds))
		{
			yield break;
		}
		foreach (short effectId in effectIds.Items)
		{
			yield return EquipmentEffect.Instance[effectId];
		}
	}

	[DomainMethod]
	public void ChangeDurability(DataContext dataContext, int charId, short changeValue, sbyte itemType, short startId, short endId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		ItemKey[] equipment = element_Objects.GetEquipment();
		ItemKey[] array = equipment;
		for (int i = 0; i < array.Length; i++)
		{
			ItemKey itemKey = array[i];
			if (itemKey.IsValid())
			{
				ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
				if (itemKey.ItemType == itemType && itemKey.TemplateId >= startId && itemKey.TemplateId <= endId)
				{
					int value = baseItem.GetCurrDurability() + changeValue;
					value = Math.Clamp(value, 0, baseItem.GetMaxDurability());
					baseItem.SetCurrDurability((short)value, dataContext);
				}
			}
		}
		Inventory inventory = element_Objects.GetInventory();
		foreach (KeyValuePair<ItemKey, int> item in inventory.Items)
		{
			item.Deconstruct(out var key, out var value2);
			ItemKey itemKey2 = key;
			int num = value2;
			ItemBase baseItem2 = DomainManager.Item.GetBaseItem(itemKey2);
			if (itemKey2.ItemType == itemType && itemKey2.TemplateId >= startId && itemKey2.TemplateId <= endId)
			{
				int value3 = baseItem2.GetCurrDurability() + changeValue;
				value3 = Math.Clamp(value3, 0, baseItem2.GetMaxDurability());
				baseItem2.SetCurrDurability((short)value3, dataContext);
			}
		}
	}

	[DomainMethod]
	public void ChangePoisonIdentified(DataContext dataContext, int charId, bool isIdentified)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		Inventory inventory = element_Objects.GetInventory();
		foreach (ItemKey key in inventory.Items.Keys)
		{
			SetPoisonsIdentified(dataContext, key, isIdentified);
		}
	}

	[DomainMethod]
	public void GmCmd_StartCricketCombat(DataContext context, int enemyId)
	{
		Wager wager = Wager.CreateExp(0);
		List<CricketWagerData> list = SelectCricketWagers(context, enemyId);
		CricketWagerData cricketWagerData = list[list.Count - 1];
		SetWager(wager, cricketWagerData.Wager);
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenCricketBattle, enemyId, wager, cricketWagerData);
	}

	public static void RegisterItemOwners(DataContext context, DataUid uid)
	{
		DomainManager.Item.InitializeOwnedItems();
		DomainManager.Organization.InitializeOwnedItems();
		DomainManager.Character.InitializeOwnedItems();
		DomainManager.Taiwu.InitializeOwnedItems();
		DomainManager.Map.InitializeOwnedItems();
		DomainManager.Merchant.InitializeOwnedItems();
		DomainManager.Building.InitializeOwnedItems();
		DomainManager.LegendaryBook.InitializeOwnedItems();
		DomainManager.Extra.InitializeOwnedItems();
		DomainManager.Item.CheckUnownedItems();
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(uid, "RegisterItemOwners");
	}

	private void InitializeOwnedItems()
	{
		SetOwner(_emptyHandKey, ItemOwnerType.System, 6);
		SetOwner(_branchKey, ItemOwnerType.System, 6);
		SetOwner(_stoneKey, ItemOwnerType.System, 6);
	}

	public void CheckUnownedItems()
	{
		Span<int> span = stackalloc int[13];
		int key;
		foreach (KeyValuePair<int, Weapon> weapon2 in _weapons)
		{
			weapon2.Deconstruct(out key, out var value);
			Weapon weapon = value;
			if (!IsPureStackable(weapon) && weapon.Owner.OwnerType == ItemOwnerType.None)
			{
				span[0]++;
				LogUnownedItem(weapon);
			}
		}
		foreach (KeyValuePair<int, Armor> armor2 in _armors)
		{
			armor2.Deconstruct(out key, out var value2);
			Armor armor = value2;
			if (!IsPureStackable(armor) && armor.Owner.OwnerType == ItemOwnerType.None)
			{
				span[1]++;
				LogUnownedItem(armor);
			}
		}
		foreach (KeyValuePair<int, Accessory> accessory2 in _accessories)
		{
			accessory2.Deconstruct(out key, out var value3);
			Accessory accessory = value3;
			if (!IsPureStackable(accessory) && accessory.Owner.OwnerType == ItemOwnerType.None)
			{
				span[2]++;
				LogUnownedItem(accessory);
			}
		}
		foreach (KeyValuePair<int, Clothing> item in _clothing)
		{
			item.Deconstruct(out key, out var value4);
			Clothing clothing = value4;
			if (!IsPureStackable(clothing) && clothing.Owner.OwnerType == ItemOwnerType.None)
			{
				span[3]++;
				LogUnownedItem(clothing);
			}
		}
		foreach (KeyValuePair<int, Carrier> carrier2 in _carriers)
		{
			carrier2.Deconstruct(out key, out var value5);
			Carrier carrier = value5;
			if (!IsPureStackable(carrier) && carrier.Owner.OwnerType == ItemOwnerType.None)
			{
				span[4]++;
				LogUnownedItem(carrier);
			}
		}
		foreach (KeyValuePair<int, Material> material2 in _materials)
		{
			material2.Deconstruct(out key, out var value6);
			Material material = value6;
			if (!IsPureStackable(material) && material.Owner.OwnerType == ItemOwnerType.None)
			{
				span[5]++;
				LogUnownedItem(material);
			}
		}
		foreach (KeyValuePair<int, CraftTool> craftTool2 in _craftTools)
		{
			craftTool2.Deconstruct(out key, out var value7);
			CraftTool craftTool = value7;
			if (!IsPureStackable(craftTool) && craftTool.Owner.OwnerType == ItemOwnerType.None)
			{
				span[6]++;
				LogUnownedItem(craftTool);
			}
		}
		foreach (KeyValuePair<int, Food> food2 in _foods)
		{
			food2.Deconstruct(out key, out var value8);
			Food food = value8;
			if (!IsPureStackable(food) && food.Owner.OwnerType == ItemOwnerType.None)
			{
				span[7]++;
				LogUnownedItem(food);
			}
		}
		foreach (KeyValuePair<int, Medicine> medicine2 in _medicines)
		{
			medicine2.Deconstruct(out key, out var value9);
			Medicine medicine = value9;
			if (!IsPureStackable(medicine) && medicine.Owner.OwnerType == ItemOwnerType.None)
			{
				span[8]++;
				LogUnownedItem(medicine);
			}
		}
		foreach (KeyValuePair<int, TeaWine> teaWine2 in _teaWines)
		{
			teaWine2.Deconstruct(out key, out var value10);
			TeaWine teaWine = value10;
			if (!IsPureStackable(teaWine) && teaWine.Owner.OwnerType == ItemOwnerType.None)
			{
				span[9]++;
				LogUnownedItem(teaWine);
			}
		}
		foreach (KeyValuePair<int, SkillBook> skillBook2 in _skillBooks)
		{
			skillBook2.Deconstruct(out key, out var value11);
			SkillBook skillBook = value11;
			if (!IsPureStackable(skillBook) && skillBook.Owner.OwnerType == ItemOwnerType.None)
			{
				span[10]++;
				LogUnownedItem(skillBook);
			}
		}
		foreach (KeyValuePair<int, Cricket> cricket2 in _crickets)
		{
			cricket2.Deconstruct(out key, out var value12);
			Cricket cricket = value12;
			if (!IsPureStackable(cricket) && cricket.Owner.OwnerType == ItemOwnerType.None)
			{
				span[11]++;
				LogUnownedItem(cricket);
			}
		}
		foreach (KeyValuePair<int, Misc> item2 in _misc)
		{
			item2.Deconstruct(out key, out var value13);
			Misc misc = value13;
			if (!IsPureStackable(misc) && misc.Owner.OwnerType == ItemOwnerType.None)
			{
				span[12]++;
				LogUnownedItem(misc);
			}
		}
		int num = 0;
		for (sbyte b = 0; b < 13; b++)
		{
			if (span[b] > 0)
			{
				num += span[b];
				Logger.Warn($"{span[b]} unowned {ItemType.TypeId2TypeName[b]} detected.");
			}
		}
		Logger.Info($"Total unowned items: {num}");
	}

	private void LogUnownedItem(ItemBase itemBase)
	{
		if (itemBase.PrevOwner.OwnerType != ItemOwnerType.None)
		{
			Logger.Warn($"Item {itemBase.GetItemKey()} is no longer owned buy any container. Prev owner: {itemBase.PrevOwner}");
		}
	}

	public void SetOwner(ItemKey itemKey, ItemOwnerType ownerType, int ownerId)
	{
		if (itemKey.IsValid() && ItemExists(itemKey))
		{
			ItemBase baseItem = GetBaseItem(itemKey);
			baseItem.SetOwner(ownerType, ownerId);
		}
	}

	public void RemoveOwner(ItemKey itemKey, ItemOwnerType ownerType, int ownerId)
	{
		ItemBase baseItem = GetBaseItem(itemKey);
		baseItem.RemoveOwner(ownerType, ownerId);
	}

	[Obsolete("Instead by FullPoisonEffects. Now only for archive data fix. Do not delete this code.")]
	public bool HasOldPoisonEffects(ItemKey itemKey)
	{
		return _poisonItems.ContainsKey(itemKey.Id);
	}

	[Obsolete("Instead by FullPoisonEffects. Now only for archive data fix. Do not delete this code.")]
	public PoisonEffects GetOldPoisonEffects(ItemKey itemKey)
	{
		return _poisonItems[itemKey.Id];
	}

	public PoisonsAndLevels GetAttachedPoisons(ItemKey itemKey)
	{
		if (DomainManager.Extra.TryGetPoisonEffect(itemKey.Id, out var poisonEffects))
		{
			return poisonEffects.GetAllPoisonsAndLevels();
		}
		PoisonsAndLevels result = default(PoisonsAndLevels);
		result.Initialize();
		return result;
	}

	public void SetPoisonsIdentified(DataContext context, ItemKey itemKey, bool isIdentified)
	{
		if (DomainManager.Extra.TryGetPoisonEffect(itemKey.Id, out var poisonEffects))
		{
			poisonEffects.IsIdentified = isIdentified;
			DomainManager.Extra.SetPoisonEffect(context, itemKey.Id, poisonEffects);
		}
	}

	public FullPoisonEffects GetPoisonEffects(ItemKey itemKey)
	{
		PoisonEffects.TryGetValue(itemKey.Id, out var value);
		if (value == null)
		{
			return new FullPoisonEffects();
		}
		return value;
	}

	public (ItemBase item, bool keyChanged) SetAttachedPoisons(DataContext context, ItemBase item, short medicineTemplateId, bool add, IReadOnlyList<short> condensedMedicineTemplateIdList = null)
	{
		ItemBase itemBase = item;
		bool flag = false;
		int id = item.GetId();
		ItemKey itemKey = item.GetItemKey();
		byte modificationState = item.GetModificationState();
		if (ModificationStateHelper.IsActive(modificationState, 1))
		{
			FullPoisonEffects fullPoisonEffects = PoisonEffects[id];
			if (!add)
			{
				fullPoisonEffects.RemovePoison(medicineTemplateId);
			}
			else
			{
				fullPoisonEffects.AddPoison(medicineTemplateId, condensedMedicineTemplateIdList);
			}
			if (fullPoisonEffects.IsValid)
			{
				DomainManager.Extra.SetPoisonEffect(context, id, fullPoisonEffects);
			}
			else
			{
				DomainManager.Extra.RemovePoisonEffect(context, id);
				byte modificationState2 = ModificationStateHelper.Deactivate(modificationState, 1);
				item.SetModificationState(modificationState2, context);
				if (ItemTemplateHelper.IsStackable(itemKey.ItemType, itemKey.TemplateId))
				{
					ItemKey itemKey2 = DomainManager.Item.CreateItem(context, itemKey.ItemType, itemKey.TemplateId);
					itemBase = DomainManager.Item.GetBaseItem(itemKey2);
				}
				flag = true;
			}
		}
		else
		{
			FullPoisonEffects fullPoisonEffects2 = new FullPoisonEffects();
			fullPoisonEffects2.AddPoison(medicineTemplateId, condensedMedicineTemplateIdList);
			int id2;
			if (!IsPureStackable(item))
			{
				byte modificationState3 = ModificationStateHelper.Activate(modificationState, 1);
				item.SetModificationState(modificationState3, context);
				id2 = item.GetId();
			}
			else
			{
				ItemBase itemBase2 = CreateUniqueStackableItem(context, item.GetItemType(), item.GetTemplateId());
				byte modificationState4 = itemBase2.GetModificationState();
				modificationState4 = ModificationStateHelper.Activate(modificationState4, 1);
				itemBase2.SetModificationState(modificationState4, context);
				itemBase = itemBase2;
				id2 = itemBase2.GetId();
			}
			flag = true;
			DomainManager.Extra.SetPoisonEffect(context, id2, fullPoisonEffects2);
		}
		if (flag)
		{
			InteractOfLove.CheckItemIsLoveTokenAndReplace(context, item.GetItemKey(), itemBase.GetItemKey());
		}
		return (item: itemBase, keyChanged: flag);
	}

	public ItemBase SetAttachedPoisons(DataContext context, ItemBase item, FullPoisonEffects poisonEffects)
	{
		if (poisonEffects == null || item == null)
		{
			return item;
		}
		if (DomainManager.Extra.TryGetPoisonEffect(item.GetId(), out var _))
		{
			DomainManager.Extra.RemovePoisonEffect(context, item.GetId());
		}
		if (!poisonEffects.IsValid)
		{
			return item;
		}
		ItemBase itemBase = item;
		foreach (PoisonSlot poisonSlot in poisonEffects.PoisonSlotList)
		{
			itemBase = SetAttachedPoisons(context, itemBase, poisonSlot.MedicineTemplateId, add: true, poisonSlot.CondensedMedicineTemplateIdList).item;
		}
		SetPoisonsIdentified(context, itemBase.GetItemKey(), poisonEffects.IsIdentified);
		return itemBase;
	}

	[DomainMethod]
	public List<ItemDisplayData> IdentifyPoisons(DataContext context, int charId, ItemDisplayData itemDisplayData)
	{
		List<ItemKey> allItemKeysFromPool = itemDisplayData.GetAllItemKeysFromPool();
		GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
		List<ItemDisplayData> identifiedList = new List<ItemDisplayData>();
		allItemKeysFromPool.ForEach(delegate(ItemKey k)
		{
			if (PoisonEffects.TryGetValue(k.Id, out var value))
			{
				if (!value.IsIdentified)
				{
					List<short> allMedicineTemplateIds = value.GetAllMedicineTemplateIds();
					int num = allMedicineTemplateIds.Max((short id) => (id > -1) ? ItemTemplateHelper.GetGrade(8, id) : (-1));
					if (num > -1)
					{
						short num2 = GlobalConfig.Instance.PoisonAttainments[num];
						short lifeSkillAttainment = character.GetLifeSkillAttainment(9);
						if (lifeSkillAttainment >= num2)
						{
							IdentifySuccess(k);
						}
					}
					else
					{
						IdentifySuccess(k);
					}
				}
			}
			else
			{
				IdentifySuccess(k);
			}
		});
		ItemDisplayData.ReturnItemKeyListToPool(allItemKeysFromPool);
		DomainManager.World.AdvanceDaysInMonth(context, 1);
		bool flag = character.IsOnCityTown();
		Inventory inventory = character.GetInventory();
		bool flag2 = character.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
		List<ItemSourceType> list = new List<ItemSourceType>();
		switch ((ItemSourceType)itemDisplayData.ItemSourceType)
		{
		case ItemSourceType.Equipment:
		case ItemSourceType.Inventory:
		case ItemSourceType.EquipmentPlan:
			list.Add(ItemSourceType.Inventory);
			if (flag && flag2)
			{
				list.Add(ItemSourceType.Warehouse);
				list.Add(ItemSourceType.Treasury);
			}
			break;
		case ItemSourceType.Warehouse:
			if (flag2)
			{
				list.Add(ItemSourceType.Warehouse);
				if (flag)
				{
					list.Add(ItemSourceType.Inventory);
				}
				list.Add(ItemSourceType.Treasury);
			}
			break;
		case ItemSourceType.Treasury:
			if (flag2)
			{
				list.Add(ItemSourceType.Treasury);
				if (flag)
				{
					list.Add(ItemSourceType.Inventory);
				}
				list.Add(ItemSourceType.Warehouse);
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		if (!CheckItemInSourceList(list))
		{
			throw new Exception("IdentifyPoisons dot not have enough TestingNeedle");
		}
		if (itemDisplayData.HasAnyPoison && identifiedList.Count == 1)
		{
			identifiedList.RemoveAll((ItemDisplayData d) => !d.HasAnyPoison);
		}
		return identifiedList;
		static bool CheckItem(IReadOnlyDictionary<ItemKey, int> items, out ItemKey testingNeedleKey)
		{
			testingNeedleKey = items.FirstOrDefault((KeyValuePair<ItemKey, int> p) => p.Key.ItemType == 12 && p.Key.TemplateId == 91).Key;
			if (!testingNeedleKey.IsValid() || !items.TryGetValue(testingNeedleKey, out var value) || value <= 0)
			{
				return false;
			}
			return true;
		}
		bool CheckItemBySource(ItemSourceType itemSourceType)
		{
			ItemKey testingNeedleKey;
			switch (itemSourceType)
			{
			case ItemSourceType.Inventory:
				if (CheckItem(inventory.Items, out testingNeedleKey))
				{
					character.RemoveInventoryItem(context, testingNeedleKey, 1, deleteItem: true);
					return true;
				}
				return false;
			case ItemSourceType.Warehouse:
				if (CheckItem(DomainManager.Taiwu.WarehouseItems, out testingNeedleKey))
				{
					DomainManager.Taiwu.RemoveItem(context, testingNeedleKey, 1, 2, deleteItem: true);
					return true;
				}
				return false;
			case ItemSourceType.Treasury:
				if (CheckItem(DomainManager.Taiwu.GetItems(ItemSourceType.Treasury), out testingNeedleKey))
				{
					DomainManager.Taiwu.RemoveItem(context, testingNeedleKey, 1, 3, deleteItem: true);
					return true;
				}
				return false;
			default:
				throw new ArgumentOutOfRangeException("itemSourceType", itemSourceType, null);
			}
		}
		bool CheckItemInSourceList(List<ItemSourceType> list2)
		{
			foreach (ItemSourceType item in list2)
			{
				if (CheckItemBySource(item))
				{
					return true;
				}
			}
			return false;
		}
		void IdentifySuccess(ItemKey itemKey)
		{
			SetPoisonsIdentified(context, itemKey, isIdentified: true);
			ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
			ItemDisplayData newItemData = DomainManager.Item.GetItemDisplayData(baseItem, 1, charId, itemDisplayData.ItemSourceType);
			ItemDisplayData itemDisplayData2 = identifiedList.Find((ItemDisplayData d) => d.PoisonEquals(newItemData));
			if (itemDisplayData2 == null)
			{
				identifiedList.Add(newItemData);
			}
			else
			{
				itemDisplayData2.Amount++;
			}
		}
	}

	internal ItemKey RemoveOldPoisonEffect(DataContext dataContext, ItemKey itemKey)
	{
		ItemBase baseItem = GetBaseItem(itemKey);
		ItemKey result;
		if (ItemTemplateHelper.IsStackable(itemKey.ItemType, itemKey.TemplateId))
		{
			RemoveItem(dataContext, itemKey);
			result = CreateItem(dataContext, itemKey.ItemType, itemKey.TemplateId);
		}
		else
		{
			if (DomainManager.Item.HasOldPoisonEffects(itemKey))
			{
				RemoveElement_PoisonItems(itemKey.Id, dataContext);
			}
			byte modificationState = baseItem.GetModificationState();
			modificationState = ModificationStateHelper.Deactivate(modificationState, 1);
			baseItem.SetModificationState(modificationState, dataContext);
			result = baseItem.GetItemKey();
		}
		Logger.Warn($"Fixing wrong poison effects of item {itemKey}");
		return result;
	}

	internal ItemKey RemovePoisonEffect(DataContext dataContext, ItemKey itemKey)
	{
		ItemBase baseItem = GetBaseItem(itemKey);
		ItemKey result;
		if (ItemTemplateHelper.IsStackable(itemKey.ItemType, itemKey.TemplateId))
		{
			RemoveItem(dataContext, itemKey);
			result = CreateItem(dataContext, itemKey.ItemType, itemKey.TemplateId);
		}
		else
		{
			if (DomainManager.Extra.TryGetPoisonEffect(itemKey.Id, out var _))
			{
				DomainManager.Extra.RemovePoisonEffect(dataContext, itemKey.Id);
			}
			byte modificationState = baseItem.GetModificationState();
			modificationState = ModificationStateHelper.Deactivate(modificationState, 1);
			baseItem.SetModificationState(modificationState, dataContext);
			result = baseItem.GetItemKey();
		}
		Logger.Warn($"Fixing wrong poison effects of item {itemKey}");
		return result;
	}

	private void TransferOldPoisonData(DataContext context)
	{
		foreach (KeyValuePair<int, PoisonEffects> poisonItem in _poisonItems)
		{
			poisonItem.Deconstruct(out var key, out var value);
			int id = key;
			PoisonEffects poisonEffects = value;
			FullPoisonEffects fullPoisonEffects = new FullPoisonEffects
			{
				IsIdentified = poisonEffects.IsIdentified,
				PoisonSlotList = new List<PoisonSlot>()
			};
			short[] allMedicineTemplateIds = poisonEffects.GetAllMedicineTemplateIds();
			short[] array = allMedicineTemplateIds;
			foreach (short medicineTemplateId in array)
			{
				PoisonSlot item = new PoisonSlot
				{
					MedicineTemplateId = medicineTemplateId
				};
				fullPoisonEffects.PoisonSlotList.Add(item);
			}
			DomainManager.Extra.AddPoisonEffect(context, id, fullPoisonEffects);
		}
		ClearPoisonItems(context);
	}

	public RefiningEffects GetRefinedEffects(ItemKey itemKey)
	{
		return _refinedItems[itemKey.Id];
	}

	public (ItemBase item, bool keyChanged) SetRefinedEffects(DataContext context, ItemBase item, int index, short materialTemplateId)
	{
		ItemBase itemBase = item;
		bool flag = false;
		int id = item.GetId();
		byte modificationState = item.GetModificationState();
		if (ModificationStateHelper.IsActive(modificationState, 2))
		{
			RefiningEffects value = _refinedItems[id];
			value.Set(index, materialTemplateId);
			if (value.GetTotalRefiningCount() > 0)
			{
				SetElement_RefinedItems(id, value, context);
				item.SetModificationState(modificationState, context);
			}
			else
			{
				RemoveElement_RefinedItems(id, context);
				byte modificationState2 = ModificationStateHelper.Deactivate(modificationState, 2);
				item.SetModificationState(modificationState2, context);
				flag = true;
			}
		}
		else
		{
			RefiningEffects value2 = default(RefiningEffects);
			value2.Initialize();
			value2.Set(index, materialTemplateId);
			if (!IsPureStackable(item))
			{
				byte modificationState3 = ModificationStateHelper.Activate(modificationState, 2);
				item.SetModificationState(modificationState3, context);
				AddElement_RefinedItems(item.GetId(), value2, context);
				flag = true;
			}
			else
			{
				ItemBase itemBase2 = CreateUniqueStackableItem(context, item.GetItemType(), item.GetTemplateId());
				byte modificationState4 = itemBase2.GetModificationState();
				modificationState4 = ModificationStateHelper.Activate(modificationState4, 2);
				itemBase2.SetModificationState(modificationState4, context);
				AddElement_RefinedItems(itemBase2.GetId(), value2, context);
				flag = true;
				itemBase = itemBase2;
			}
		}
		if (flag)
		{
			InteractOfLove.CheckItemIsLoveTokenAndReplace(context, item.GetItemKey(), itemBase.GetItemKey());
		}
		return (item: itemBase, keyChanged: flag);
	}

	public ItemBase SetRefinedEffects(DataContext context, ItemBase item, RefiningEffects refiningEffects)
	{
		if (refiningEffects.GetTotalRefiningCount() == 0)
		{
			return item;
		}
		ItemBase itemBase = item;
		short[] allMaterialTemplateIds = refiningEffects.GetAllMaterialTemplateIds();
		for (int i = 0; i < allMaterialTemplateIds.Length; i++)
		{
			short materialTemplateId = allMaterialTemplateIds[i];
			itemBase = SetRefinedEffects(context, itemBase, i, materialTemplateId).item;
		}
		return itemBase;
	}

	public bool RemoveRefinedEffectsAndReturnMaterial(DataContext context, ItemKey baseKey, Inventory inventory, out ItemBase resultItem)
	{
		resultItem = null;
		if (!ModificationStateHelper.IsActive(baseKey.ModificationState, 2))
		{
			return false;
		}
		if (baseKey.ItemType != 2)
		{
			return false;
		}
		AccessoryItem accessoryItem = Config.Accessory.Instance[baseKey.TemplateId];
		if (accessoryItem.GroupId != 250)
		{
			return false;
		}
		ItemBase baseItem = GetBaseItem(baseKey);
		RefiningEffects refinedEffects = DomainManager.Item.GetRefinedEffects(baseKey);
		for (int i = 0; i < 5; i++)
		{
			short materialTemplateIdAt = refinedEffects.GetMaterialTemplateIdAt(i);
			if (materialTemplateIdAt >= 0)
			{
				ItemKey itemKey = CreateMaterial(context, materialTemplateIdAt);
				if (inventory == null)
				{
					DomainManager.Taiwu.AddItem(context, itemKey, 1, ItemSourceType.Warehouse);
				}
				else
				{
					inventory.OfflineAdd(itemKey, 1);
				}
				(ItemBase item, bool keyChanged) tuple = DomainManager.Item.SetRefinedEffects(context, baseItem, i, -1);
				var (itemBase, _) = tuple;
				if (tuple.keyChanged)
				{
					resultItem = itemBase;
				}
			}
		}
		return resultItem != null;
	}

	[DomainMethod]
	public List<SkillBookModifyDisplayData> GetTaiwuInventoryCombatSkillBooks()
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Inventory inventory = taiwu.GetInventory();
		List<SkillBookModifyDisplayData> list = new List<SkillBookModifyDisplayData>();
		foreach (ItemKey key in inventory.Items.Keys)
		{
			if (key.IsValid() && key.ItemType == 10)
			{
				SkillBookItem skillBookItem = Config.SkillBook.Instance[key.TemplateId];
				if (skillBookItem.ItemSubType == 1001 && _skillBooks.TryGetValue(key.Id, out var value))
				{
					short readingExpGainPerPage = SkillGradeData.Instance[value.GetGrade()].ReadingExpGainPerPage;
					int normalPageCostExp = readingExpGainPerPage * 20;
					int outlinePageCostExp = readingExpGainPerPage * 60;
					SkillBookModifyDisplayData item = new SkillBookModifyDisplayData
					{
						ItemDisplayData = GetItemDisplayData(key),
						PageTypes = value.GetPageTypes(),
						PageIncompleteState = value.GetPageIncompleteState(),
						NormalPageCostExp = normalPageCostExp,
						OutlinePageCostExp = outlinePageCostExp
					};
					list.Add(item);
				}
			}
		}
		return list;
	}

	[DomainMethod]
	[Obsolete("Use SetCombatSkillBookPage instead")]
	public bool ModifyCombatSkillBookPageOutline(DataContext context, ItemKey itemKey, sbyte behaviorType)
	{
		if (DomainManager.World.GetLeftDaysInCurrMonth() < 10)
		{
			return false;
		}
		if (!_skillBooks.TryGetValue(itemKey.Id, out var value))
		{
			return false;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int num = SkillGradeData.Instance[value.GetGrade()].ReadingExpGainPerPage * 60;
		if (taiwu.GetExp() < num)
		{
			return false;
		}
		DomainManager.World.AdvanceDaysInMonth(context, 10);
		taiwu.ChangeExp(context, -num);
		value.SetOutlinePageType(context, behaviorType);
		return true;
	}

	[DomainMethod]
	[Obsolete("Use SetCombatSkillBookPage instead")]
	public bool ModifyCombatSkillBookPageNormal(DataContext context, ItemKey itemKey, List<byte> pageIds, List<sbyte> directions)
	{
		if (DomainManager.World.GetLeftDaysInCurrMonth() < 10)
		{
			return false;
		}
		if (pageIds == null || pageIds.Count <= 0 || directions == null || directions.Count <= 0 || pageIds.Count != directions.Count)
		{
			return false;
		}
		if (!_skillBooks.TryGetValue(itemKey.Id, out var value))
		{
			return false;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int num = SkillGradeData.Instance[value.GetGrade()].ReadingExpGainPerPage * 20 * pageIds.Count;
		if (taiwu.GetExp() < num)
		{
			return false;
		}
		DomainManager.World.AdvanceDaysInMonth(context, 10);
		taiwu.ChangeExp(context, -num);
		for (int i = 0; i < pageIds.Count; i++)
		{
			value.SetNormalPageType(context, pageIds[i], directions[i]);
		}
		return true;
	}

	[DomainMethod]
	public bool SetCombatSkillBookPage(DataContext context, ItemKey itemKey, sbyte behaviorType, List<sbyte> directions)
	{
		if (DomainManager.World.GetLeftDaysInCurrMonth() < 10)
		{
			return false;
		}
		if (!_skillBooks.TryGetValue(itemKey.Id, out var value))
		{
			return false;
		}
		if (directions == null || directions.Count != 5)
		{
			return false;
		}
		byte pageTypes = value.GetPageTypes();
		sbyte outlinePageType = SkillBookStateHelper.GetOutlinePageType(pageTypes);
		bool flag = outlinePageType != behaviorType;
		int num = (flag ? (SkillGradeData.Instance[value.GetGrade()].ReadingExpGainPerPage * 60) : 0);
		int num2 = 0;
		Span<bool> span = stackalloc bool[5];
		for (int i = 0; i < 5; i++)
		{
			sbyte normalPageType = SkillBookStateHelper.GetNormalPageType(pageTypes, (byte)(i + 1));
			span[i] = normalPageType != directions[i];
			num2 += (span[i] ? (SkillGradeData.Instance[value.GetGrade()].ReadingExpGainPerPage * 20) : 0);
		}
		int num3 = num + num2;
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (taiwu.GetExp() < num3)
		{
			return false;
		}
		DomainManager.World.AdvanceDaysInMonth(context, 10);
		taiwu.ChangeExp(context, -num3);
		if (flag)
		{
			value.SetOutlinePageType(context, behaviorType);
		}
		for (int j = 0; j < 5; j++)
		{
			if (span[j])
			{
				value.SetNormalPageType(context, (byte)(j + 1), directions[j]);
			}
		}
		return true;
	}

	public int GetPageIncompleteState(ushort pageIncompleteState, byte pageId, ItemKey[] referenceBooks, ItemKey curReadingBook)
	{
		sbyte b = SkillBookStateHelper.GetPageIncompleteState(pageIncompleteState, pageId);
		if (b == 0 || !curReadingBook.IsValid())
		{
			return b;
		}
		SkillBook element_SkillBooks = GetElement_SkillBooks(curReadingBook.Id);
		for (int i = 0; i < referenceBooks.Length; i++)
		{
			ItemKey itemKey = referenceBooks[i];
			if (!itemKey.IsValid() || itemKey.TemplateId != element_SkillBooks.GetTemplateId())
			{
				continue;
			}
			SkillBook element_SkillBooks2 = GetElement_SkillBooks(itemKey.Id);
			bool flag = true;
			if (element_SkillBooks.GetCombatSkillTemplateId() > -1)
			{
				byte pageTypes = element_SkillBooks.GetPageTypes();
				byte pageTypes2 = element_SkillBooks2.GetPageTypes();
				sbyte normalPageType = SkillBookStateHelper.GetNormalPageType(pageTypes, pageId);
				sbyte normalPageType2 = SkillBookStateHelper.GetNormalPageType(pageTypes2, pageId);
				if (normalPageType != normalPageType2)
				{
					flag = false;
				}
			}
			if (flag)
			{
				sbyte pageIncompleteState2 = SkillBookStateHelper.GetPageIncompleteState(element_SkillBooks2.GetPageIncompleteState(), pageId);
				if (pageIncompleteState2 >= 0 && pageIncompleteState2 < b)
				{
					b = pageIncompleteState2;
				}
			}
		}
		return b;
	}

	public bool HasNewDeadCricket()
	{
		return _newDeadCrickets.Count > 0;
	}

	public bool IsNewDeadCricket(ItemKey itemKey)
	{
		return _newDeadCrickets.Contains(itemKey);
	}

	public void UpdateCrickets(DataContext context)
	{
		_newDeadCrickets.Clear();
		if (DomainManager.World.GetCurrMonthInYear() != GlobalConfig.Instance.CricketActiveStartMonth + 1)
		{
			return;
		}
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		foreach (Cricket value in _crickets.Values)
		{
			if (value.UpdateCricketAge(context))
			{
				ItemKey itemKey = value.GetItemKey();
				List<CricketCollectionData> cricketCollectionDataList = DomainManager.Extra.GetCricketCollectionDataList();
				if (DomainManager.Taiwu.GetWarehouseItemCount(itemKey) > 0 || cricketCollectionDataList.Select((CricketCollectionData item) => item.Cricket).Contains(itemKey) || DomainManager.Taiwu.GetTaiwu().GetInventory().Items.ContainsKey(itemKey))
				{
					short colorId = value.GetColorId();
					short partId = value.GetPartId();
					monthlyNotificationCollection.AddCricketEndLife(colorId, partId);
				}
				else
				{
					_newDeadCrickets.Add(itemKey);
				}
			}
		}
	}

	[DomainMethod]
	public bool[] GetCricketsAliveState(List<ItemKey> keyList)
	{
		bool[] array = new bool[keyList.Count];
		if (keyList != null)
		{
			for (int i = 0; i < keyList.Count; i++)
			{
				ItemKey itemKey = keyList[i];
				bool flag = false;
				if (DomainManager.Item.TryGetElement_Crickets(itemKey.Id, out var element))
				{
					flag = element.IsAlive;
				}
				array[i] = flag;
			}
		}
		return array;
	}

	public static short GetWugTemplateId(sbyte wugType, sbyte wugGrowthType)
	{
		int num = wugType * 6 + wugGrowthType;
		return _wugTemplateIds[num];
	}

	public static IEnumerable<short> GetWugTemplateIdGroup(sbyte wugType, bool isKing)
	{
		int group = wugType * 6;
		int begin = (isKing ? 5 : group);
		int end = (isKing ? 53 : (group + 5));
		int step = ((!isKing) ? 1 : 6);
		for (int i = begin; i < end; i += step)
		{
			yield return _wugTemplateIds[i];
		}
	}

	private static void InitializeWugTemplateIds()
	{
		_wugTemplateIds = new short[48];
		foreach (MedicineItem item in (IEnumerable<MedicineItem>)Config.Medicine.Instance)
		{
			if (item.WugType >= 0)
			{
				int num = item.WugType * 6 + item.WugGrowthType;
				_wugTemplateIds[num] = item.TemplateId;
			}
		}
	}

	public ItemDomain()
		: base(21)
	{
		_weapons = new Dictionary<int, Weapon>(0);
		_armors = new Dictionary<int, Armor>(0);
		_accessories = new Dictionary<int, Accessory>(0);
		_clothing = new Dictionary<int, Clothing>(0);
		_carriers = new Dictionary<int, Carrier>(0);
		_materials = new Dictionary<int, Material>(0);
		_craftTools = new Dictionary<int, CraftTool>(0);
		_foods = new Dictionary<int, Food>(0);
		_medicines = new Dictionary<int, Medicine>(0);
		_teaWines = new Dictionary<int, TeaWine>(0);
		_skillBooks = new Dictionary<int, SkillBook>(0);
		_crickets = new Dictionary<int, Cricket>(0);
		_misc = new Dictionary<int, Misc>(0);
		_nextItemId = 0;
		_stackableItems = new Dictionary<TemplateKey, int>(0);
		_poisonItems = new Dictionary<int, PoisonEffects>(0);
		_refinedItems = new Dictionary<int, RefiningEffects>(0);
		_emptyHandKey = default(ItemKey);
		_branchKey = default(ItemKey);
		_stoneKey = default(ItemKey);
		_externEquipmentEffects = new Dictionary<int, GameData.Utilities.ShortList>(0);
		HelperDataWeapons = new ObjectCollectionHelperData(6, 0, CacheInfluencesWeapons, _dataStatesWeapons, isArchive: true);
		HelperDataArmors = new ObjectCollectionHelperData(6, 1, CacheInfluencesArmors, _dataStatesArmors, isArchive: true);
		HelperDataAccessories = new ObjectCollectionHelperData(6, 2, CacheInfluencesAccessories, _dataStatesAccessories, isArchive: true);
		HelperDataClothing = new ObjectCollectionHelperData(6, 3, CacheInfluencesClothing, _dataStatesClothing, isArchive: true);
		HelperDataCarriers = new ObjectCollectionHelperData(6, 4, CacheInfluencesCarriers, _dataStatesCarriers, isArchive: true);
		HelperDataMaterials = new ObjectCollectionHelperData(6, 5, CacheInfluencesMaterials, _dataStatesMaterials, isArchive: true);
		HelperDataCraftTools = new ObjectCollectionHelperData(6, 6, CacheInfluencesCraftTools, _dataStatesCraftTools, isArchive: true);
		HelperDataFoods = new ObjectCollectionHelperData(6, 7, CacheInfluencesFoods, _dataStatesFoods, isArchive: true);
		HelperDataMedicines = new ObjectCollectionHelperData(6, 8, CacheInfluencesMedicines, _dataStatesMedicines, isArchive: true);
		HelperDataTeaWines = new ObjectCollectionHelperData(6, 9, CacheInfluencesTeaWines, _dataStatesTeaWines, isArchive: true);
		HelperDataSkillBooks = new ObjectCollectionHelperData(6, 10, CacheInfluencesSkillBooks, _dataStatesSkillBooks, isArchive: true);
		HelperDataCrickets = new ObjectCollectionHelperData(6, 11, CacheInfluencesCrickets, _dataStatesCrickets, isArchive: true);
		HelperDataMisc = new ObjectCollectionHelperData(6, 12, CacheInfluencesMisc, _dataStatesMisc, isArchive: true);
		OnInitializedDomainData();
	}

	public Weapon GetElement_Weapons(int objectId)
	{
		return _weapons[objectId];
	}

	public bool TryGetElement_Weapons(int objectId, out Weapon element)
	{
		return _weapons.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_Weapons(int objectId, Weapon instance)
	{
		instance.CollectionHelperData = HelperDataWeapons;
		instance.DataStatesOffset = _dataStatesWeapons.Create();
		_weapons.Add(objectId, instance);
		byte* pData = OperationAdder.DynamicObjectCollection_Add(6, 0, objectId, instance.GetSerializedSize());
		instance.Serialize(pData);
	}

	private void RemoveElement_Weapons(int objectId)
	{
		if (_weapons.TryGetValue(objectId, out var value))
		{
			_dataStatesWeapons.Remove(value.DataStatesOffset);
			_weapons.Remove(objectId);
			OperationAdder.DynamicObjectCollection_Remove(6, 0, objectId);
		}
	}

	private void ClearWeapons()
	{
		_dataStatesWeapons.Clear();
		_weapons.Clear();
		OperationAdder.DynamicObjectCollection_Clear(6, 0);
	}

	public int GetElementField_Weapons(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_weapons.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_Weapons", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesWeapons.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetEquipmentEffectId(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetTricks(), dataPool);
		case 5:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool);
		case 6:
			return GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool);
		case 7:
			return GameData.Serializer.Serializer.Serialize(value.GetEquippedCharId(), dataPool);
		case 8:
			return GameData.Serializer.Serializer.Serialize(value.GetMaterialResources(), dataPool);
		case 9:
			return GameData.Serializer.Serializer.Serialize(value.GetPenetrationFactor(), dataPool);
		case 10:
			return GameData.Serializer.Serializer.Serialize(value.GetEquipmentAttack(), dataPool);
		case 11:
			return GameData.Serializer.Serializer.Serialize(value.GetEquipmentDefense(), dataPool);
		case 12:
			return GameData.Serializer.Serializer.Serialize(value.GetWeight(), dataPool);
		default:
			if (fieldId >= 83)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_Weapons(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_weapons.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 2:
		{
			short item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetMaxDurability(item2, context);
			return;
		}
		case 3:
		{
			short item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetEquipmentEffectId(item, context);
			return;
		}
		case 4:
		{
			List<sbyte> item6 = value.GetTricks();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item6);
			value.SetTricks(item6, context);
			return;
		}
		case 5:
		{
			short item5 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item5);
			value.SetCurrDurability(item5, context);
			return;
		}
		case 6:
		{
			byte item4 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item4);
			value.SetModificationState(item4, context);
			return;
		}
		case 7:
		{
			int item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetEquippedCharId(item3, context);
			return;
		}
		case 8:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		if (fieldId >= 83)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 13)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_Weapons(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_weapons.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 13)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesWeapons.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesWeapons.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetEquipmentEffectId(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetTricks(), dataPool), 
			5 => GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool), 
			6 => GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool), 
			7 => GameData.Serializer.Serializer.Serialize(value.GetEquippedCharId(), dataPool), 
			8 => GameData.Serializer.Serializer.Serialize(value.GetMaterialResources(), dataPool), 
			9 => GameData.Serializer.Serializer.Serialize(value.GetPenetrationFactor(), dataPool), 
			10 => GameData.Serializer.Serializer.Serialize(value.GetEquipmentAttack(), dataPool), 
			11 => GameData.Serializer.Serializer.Serialize(value.GetEquipmentDefense(), dataPool), 
			12 => GameData.Serializer.Serializer.Serialize(value.GetWeight(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_Weapons(int objectId, ushort fieldId)
	{
		if (_weapons.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 13)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesWeapons.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesWeapons.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_Weapons(int objectId, ushort fieldId)
	{
		if (!_weapons.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 13)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesWeapons.IsModified(value.DataStatesOffset, fieldId);
	}

	public Armor GetElement_Armors(int objectId)
	{
		return _armors[objectId];
	}

	public bool TryGetElement_Armors(int objectId, out Armor element)
	{
		return _armors.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_Armors(int objectId, Armor instance)
	{
		instance.CollectionHelperData = HelperDataArmors;
		instance.DataStatesOffset = _dataStatesArmors.Create();
		_armors.Add(objectId, instance);
		byte* pData = OperationAdder.FixedObjectCollection_Add(6, 1, objectId, 29);
		instance.Serialize(pData);
	}

	private void RemoveElement_Armors(int objectId)
	{
		if (_armors.TryGetValue(objectId, out var value))
		{
			_dataStatesArmors.Remove(value.DataStatesOffset);
			_armors.Remove(objectId);
			OperationAdder.FixedObjectCollection_Remove(6, 1, objectId);
		}
	}

	private void ClearArmors()
	{
		_dataStatesArmors.Clear();
		_armors.Clear();
		OperationAdder.FixedObjectCollection_Clear(6, 1);
	}

	public int GetElementField_Armors(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_armors.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_Armors", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesArmors.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetEquipmentEffectId(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool);
		case 5:
			return GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool);
		case 6:
			return GameData.Serializer.Serializer.Serialize(value.GetEquippedCharId(), dataPool);
		case 7:
			return GameData.Serializer.Serializer.Serialize(value.GetMaterialResources(), dataPool);
		case 8:
			return GameData.Serializer.Serializer.Serialize(value.GetPenetrationResistFactors(), dataPool);
		case 9:
			return GameData.Serializer.Serializer.Serialize(value.GetEquipmentAttack(), dataPool);
		case 10:
			return GameData.Serializer.Serializer.Serialize(value.GetEquipmentDefense(), dataPool);
		case 11:
			return GameData.Serializer.Serializer.Serialize(value.GetWeight(), dataPool);
		case 12:
			return GameData.Serializer.Serializer.Serialize(value.GetInjuryFactor(), dataPool);
		default:
			if (fieldId >= 53)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_Armors(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_armors.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 2:
		{
			short item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetMaxDurability(item2, context);
			return;
		}
		case 3:
		{
			short item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetEquipmentEffectId(item, context);
			return;
		}
		case 4:
		{
			short item5 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item5);
			value.SetCurrDurability(item5, context);
			return;
		}
		case 5:
		{
			byte item4 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item4);
			value.SetModificationState(item4, context);
			return;
		}
		case 6:
		{
			int item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetEquippedCharId(item3, context);
			return;
		}
		case 7:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		if (fieldId >= 53)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 13)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_Armors(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_armors.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 13)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesArmors.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesArmors.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetEquipmentEffectId(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool), 
			5 => GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool), 
			6 => GameData.Serializer.Serializer.Serialize(value.GetEquippedCharId(), dataPool), 
			7 => GameData.Serializer.Serializer.Serialize(value.GetMaterialResources(), dataPool), 
			8 => GameData.Serializer.Serializer.Serialize(value.GetPenetrationResistFactors(), dataPool), 
			9 => GameData.Serializer.Serializer.Serialize(value.GetEquipmentAttack(), dataPool), 
			10 => GameData.Serializer.Serializer.Serialize(value.GetEquipmentDefense(), dataPool), 
			11 => GameData.Serializer.Serializer.Serialize(value.GetWeight(), dataPool), 
			12 => GameData.Serializer.Serializer.Serialize(value.GetInjuryFactor(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_Armors(int objectId, ushort fieldId)
	{
		if (_armors.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 13)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesArmors.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesArmors.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_Armors(int objectId, ushort fieldId)
	{
		if (!_armors.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 13)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesArmors.IsModified(value.DataStatesOffset, fieldId);
	}

	public Accessory GetElement_Accessories(int objectId)
	{
		return _accessories[objectId];
	}

	public bool TryGetElement_Accessories(int objectId, out Accessory element)
	{
		return _accessories.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_Accessories(int objectId, Accessory instance)
	{
		instance.CollectionHelperData = HelperDataAccessories;
		instance.DataStatesOffset = _dataStatesAccessories.Create();
		_accessories.Add(objectId, instance);
		byte* pData = OperationAdder.FixedObjectCollection_Add(6, 2, objectId, 29);
		instance.Serialize(pData);
	}

	private void RemoveElement_Accessories(int objectId)
	{
		if (_accessories.TryGetValue(objectId, out var value))
		{
			_dataStatesAccessories.Remove(value.DataStatesOffset);
			_accessories.Remove(objectId);
			OperationAdder.FixedObjectCollection_Remove(6, 2, objectId);
		}
	}

	private void ClearAccessories()
	{
		_dataStatesAccessories.Clear();
		_accessories.Clear();
		OperationAdder.FixedObjectCollection_Clear(6, 2);
	}

	public int GetElementField_Accessories(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_accessories.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_Accessories", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesAccessories.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetEquipmentEffectId(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool);
		case 5:
			return GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool);
		case 6:
			return GameData.Serializer.Serializer.Serialize(value.GetEquippedCharId(), dataPool);
		case 7:
			return GameData.Serializer.Serializer.Serialize(value.GetMaterialResources(), dataPool);
		default:
			if (fieldId >= 79)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_Accessories(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_accessories.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 2:
		{
			short item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetMaxDurability(item2, context);
			return;
		}
		case 3:
		{
			short item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetEquipmentEffectId(item, context);
			return;
		}
		case 4:
		{
			short item5 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item5);
			value.SetCurrDurability(item5, context);
			return;
		}
		case 5:
		{
			byte item4 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item4);
			value.SetModificationState(item4, context);
			return;
		}
		case 6:
		{
			int item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetEquippedCharId(item3, context);
			return;
		}
		case 7:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		if (fieldId >= 79)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 8)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_Accessories(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_accessories.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 8)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesAccessories.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesAccessories.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetEquipmentEffectId(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool), 
			5 => GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool), 
			6 => GameData.Serializer.Serializer.Serialize(value.GetEquippedCharId(), dataPool), 
			7 => GameData.Serializer.Serializer.Serialize(value.GetMaterialResources(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_Accessories(int objectId, ushort fieldId)
	{
		if (_accessories.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 8)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesAccessories.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesAccessories.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_Accessories(int objectId, ushort fieldId)
	{
		if (!_accessories.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 8)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesAccessories.IsModified(value.DataStatesOffset, fieldId);
	}

	public Clothing GetElement_Clothing(int objectId)
	{
		return _clothing[objectId];
	}

	public bool TryGetElement_Clothing(int objectId, out Clothing element)
	{
		return _clothing.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_Clothing(int objectId, Clothing instance)
	{
		instance.CollectionHelperData = HelperDataClothing;
		instance.DataStatesOffset = _dataStatesClothing.Create();
		_clothing.Add(objectId, instance);
		byte* pData = OperationAdder.FixedObjectCollection_Add(6, 3, objectId, 30);
		instance.Serialize(pData);
	}

	private void RemoveElement_Clothing(int objectId)
	{
		if (_clothing.TryGetValue(objectId, out var value))
		{
			_dataStatesClothing.Remove(value.DataStatesOffset);
			_clothing.Remove(objectId);
			OperationAdder.FixedObjectCollection_Remove(6, 3, objectId);
		}
	}

	private void ClearClothing()
	{
		_dataStatesClothing.Clear();
		_clothing.Clear();
		OperationAdder.FixedObjectCollection_Clear(6, 3);
	}

	public int GetElementField_Clothing(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_clothing.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_Clothing", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesClothing.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetEquipmentEffectId(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool);
		case 5:
			return GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool);
		case 6:
			return GameData.Serializer.Serializer.Serialize(value.GetEquippedCharId(), dataPool);
		case 7:
			return GameData.Serializer.Serializer.Serialize(value.GetGender(), dataPool);
		case 8:
			return GameData.Serializer.Serializer.Serialize(value.GetMaterialResources(), dataPool);
		default:
			if (fieldId >= 46)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_Clothing(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_clothing.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 2:
		{
			short item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetMaxDurability(item2, context);
			return;
		}
		case 3:
		{
			short item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetEquipmentEffectId(item, context);
			return;
		}
		case 4:
		{
			short item6 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item6);
			value.SetCurrDurability(item6, context);
			return;
		}
		case 5:
		{
			byte item5 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item5);
			value.SetModificationState(item5, context);
			return;
		}
		case 6:
		{
			int item4 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item4);
			value.SetEquippedCharId(item4, context);
			return;
		}
		case 7:
		{
			sbyte item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetGender(item3, context);
			return;
		}
		case 8:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		if (fieldId >= 46)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 9)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_Clothing(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_clothing.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 9)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesClothing.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesClothing.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetEquipmentEffectId(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool), 
			5 => GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool), 
			6 => GameData.Serializer.Serializer.Serialize(value.GetEquippedCharId(), dataPool), 
			7 => GameData.Serializer.Serializer.Serialize(value.GetGender(), dataPool), 
			8 => GameData.Serializer.Serializer.Serialize(value.GetMaterialResources(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_Clothing(int objectId, ushort fieldId)
	{
		if (_clothing.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 9)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesClothing.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesClothing.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_Clothing(int objectId, ushort fieldId)
	{
		if (!_clothing.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 9)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesClothing.IsModified(value.DataStatesOffset, fieldId);
	}

	public Carrier GetElement_Carriers(int objectId)
	{
		return _carriers[objectId];
	}

	public bool TryGetElement_Carriers(int objectId, out Carrier element)
	{
		return _carriers.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_Carriers(int objectId, Carrier instance)
	{
		instance.CollectionHelperData = HelperDataCarriers;
		instance.DataStatesOffset = _dataStatesCarriers.Create();
		_carriers.Add(objectId, instance);
		byte* pData = OperationAdder.FixedObjectCollection_Add(6, 4, objectId, 29);
		instance.Serialize(pData);
	}

	private void RemoveElement_Carriers(int objectId)
	{
		if (_carriers.TryGetValue(objectId, out var value))
		{
			_dataStatesCarriers.Remove(value.DataStatesOffset);
			_carriers.Remove(objectId);
			OperationAdder.FixedObjectCollection_Remove(6, 4, objectId);
		}
	}

	private void ClearCarriers()
	{
		_dataStatesCarriers.Clear();
		_carriers.Clear();
		OperationAdder.FixedObjectCollection_Clear(6, 4);
	}

	public int GetElementField_Carriers(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_carriers.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_Carriers", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesCarriers.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetEquipmentEffectId(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool);
		case 5:
			return GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool);
		case 6:
			return GameData.Serializer.Serializer.Serialize(value.GetEquippedCharId(), dataPool);
		case 7:
			return GameData.Serializer.Serializer.Serialize(value.GetMaterialResources(), dataPool);
		default:
			if (fieldId >= 54)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_Carriers(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_carriers.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 2:
		{
			short item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetMaxDurability(item2, context);
			return;
		}
		case 3:
		{
			short item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetEquipmentEffectId(item, context);
			return;
		}
		case 4:
		{
			short item5 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item5);
			value.SetCurrDurability(item5, context);
			return;
		}
		case 5:
		{
			byte item4 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item4);
			value.SetModificationState(item4, context);
			return;
		}
		case 6:
		{
			int item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetEquippedCharId(item3, context);
			return;
		}
		case 7:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		if (fieldId >= 54)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 8)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_Carriers(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_carriers.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 8)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesCarriers.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesCarriers.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetEquipmentEffectId(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool), 
			5 => GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool), 
			6 => GameData.Serializer.Serializer.Serialize(value.GetEquippedCharId(), dataPool), 
			7 => GameData.Serializer.Serializer.Serialize(value.GetMaterialResources(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_Carriers(int objectId, ushort fieldId)
	{
		if (_carriers.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 8)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesCarriers.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesCarriers.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_Carriers(int objectId, ushort fieldId)
	{
		if (!_carriers.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 8)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesCarriers.IsModified(value.DataStatesOffset, fieldId);
	}

	public Material GetElement_Materials(int objectId)
	{
		return _materials[objectId];
	}

	public bool TryGetElement_Materials(int objectId, out Material element)
	{
		return _materials.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_Materials(int objectId, Material instance)
	{
		instance.CollectionHelperData = HelperDataMaterials;
		instance.DataStatesOffset = _dataStatesMaterials.Create();
		_materials.Add(objectId, instance);
		byte* pData = OperationAdder.FixedObjectCollection_Add(6, 5, objectId, 11);
		instance.Serialize(pData);
	}

	private void RemoveElement_Materials(int objectId)
	{
		if (_materials.TryGetValue(objectId, out var value))
		{
			_dataStatesMaterials.Remove(value.DataStatesOffset);
			_materials.Remove(objectId);
			OperationAdder.FixedObjectCollection_Remove(6, 5, objectId);
		}
	}

	private void ClearMaterials()
	{
		_dataStatesMaterials.Clear();
		_materials.Clear();
		OperationAdder.FixedObjectCollection_Clear(6, 5);
	}

	public int GetElementField_Materials(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_materials.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_Materials", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesMaterials.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool);
		default:
			if (fieldId >= 87)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_Materials(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_materials.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 2:
		{
			short item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetMaxDurability(item2, context);
			return;
		}
		case 3:
		{
			short item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetCurrDurability(item, context);
			return;
		}
		case 4:
		{
			byte item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetModificationState(item3, context);
			return;
		}
		}
		if (fieldId >= 87)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_Materials(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_materials.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesMaterials.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesMaterials.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_Materials(int objectId, ushort fieldId)
	{
		if (_materials.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 5)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesMaterials.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesMaterials.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_Materials(int objectId, ushort fieldId)
	{
		if (!_materials.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesMaterials.IsModified(value.DataStatesOffset, fieldId);
	}

	public CraftTool GetElement_CraftTools(int objectId)
	{
		return _craftTools[objectId];
	}

	public bool TryGetElement_CraftTools(int objectId, out CraftTool element)
	{
		return _craftTools.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_CraftTools(int objectId, CraftTool instance)
	{
		instance.CollectionHelperData = HelperDataCraftTools;
		instance.DataStatesOffset = _dataStatesCraftTools.Create();
		_craftTools.Add(objectId, instance);
		byte* pData = OperationAdder.FixedObjectCollection_Add(6, 6, objectId, 11);
		instance.Serialize(pData);
	}

	private void RemoveElement_CraftTools(int objectId)
	{
		if (_craftTools.TryGetValue(objectId, out var value))
		{
			_dataStatesCraftTools.Remove(value.DataStatesOffset);
			_craftTools.Remove(objectId);
			OperationAdder.FixedObjectCollection_Remove(6, 6, objectId);
		}
	}

	private void ClearCraftTools()
	{
		_dataStatesCraftTools.Clear();
		_craftTools.Clear();
		OperationAdder.FixedObjectCollection_Clear(6, 6);
	}

	public int GetElementField_CraftTools(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_craftTools.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_CraftTools", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesCraftTools.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool);
		default:
			if (fieldId >= 35)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_CraftTools(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_craftTools.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 2:
		{
			short item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetMaxDurability(item2, context);
			return;
		}
		case 3:
		{
			short item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetCurrDurability(item, context);
			return;
		}
		case 4:
		{
			byte item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetModificationState(item3, context);
			return;
		}
		}
		if (fieldId >= 35)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_CraftTools(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_craftTools.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesCraftTools.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesCraftTools.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_CraftTools(int objectId, ushort fieldId)
	{
		if (_craftTools.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 5)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesCraftTools.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesCraftTools.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_CraftTools(int objectId, ushort fieldId)
	{
		if (!_craftTools.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesCraftTools.IsModified(value.DataStatesOffset, fieldId);
	}

	public Food GetElement_Foods(int objectId)
	{
		return _foods[objectId];
	}

	public bool TryGetElement_Foods(int objectId, out Food element)
	{
		return _foods.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_Foods(int objectId, Food instance)
	{
		instance.CollectionHelperData = HelperDataFoods;
		instance.DataStatesOffset = _dataStatesFoods.Create();
		_foods.Add(objectId, instance);
		byte* pData = OperationAdder.FixedObjectCollection_Add(6, 7, objectId, 11);
		instance.Serialize(pData);
	}

	private void RemoveElement_Foods(int objectId)
	{
		if (_foods.TryGetValue(objectId, out var value))
		{
			_dataStatesFoods.Remove(value.DataStatesOffset);
			_foods.Remove(objectId);
			OperationAdder.FixedObjectCollection_Remove(6, 7, objectId);
		}
	}

	private void ClearFoods()
	{
		_dataStatesFoods.Clear();
		_foods.Clear();
		OperationAdder.FixedObjectCollection_Clear(6, 7);
	}

	public int GetElementField_Foods(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_foods.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_Foods", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesFoods.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool);
		default:
			if (fieldId >= 71)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_Foods(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_foods.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 2:
		{
			short item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetMaxDurability(item2, context);
			return;
		}
		case 3:
		{
			short item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetCurrDurability(item, context);
			return;
		}
		case 4:
		{
			byte item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetModificationState(item3, context);
			return;
		}
		}
		if (fieldId >= 71)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_Foods(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_foods.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesFoods.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesFoods.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_Foods(int objectId, ushort fieldId)
	{
		if (_foods.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 5)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesFoods.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesFoods.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_Foods(int objectId, ushort fieldId)
	{
		if (!_foods.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesFoods.IsModified(value.DataStatesOffset, fieldId);
	}

	public Medicine GetElement_Medicines(int objectId)
	{
		return _medicines[objectId];
	}

	public bool TryGetElement_Medicines(int objectId, out Medicine element)
	{
		return _medicines.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_Medicines(int objectId, Medicine instance)
	{
		instance.CollectionHelperData = HelperDataMedicines;
		instance.DataStatesOffset = _dataStatesMedicines.Create();
		_medicines.Add(objectId, instance);
		byte* pData = OperationAdder.FixedObjectCollection_Add(6, 8, objectId, 11);
		instance.Serialize(pData);
	}

	private void RemoveElement_Medicines(int objectId)
	{
		if (_medicines.TryGetValue(objectId, out var value))
		{
			_dataStatesMedicines.Remove(value.DataStatesOffset);
			_medicines.Remove(objectId);
			OperationAdder.FixedObjectCollection_Remove(6, 8, objectId);
		}
	}

	private void ClearMedicines()
	{
		_dataStatesMedicines.Clear();
		_medicines.Clear();
		OperationAdder.FixedObjectCollection_Clear(6, 8);
	}

	public int GetElementField_Medicines(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_medicines.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_Medicines", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesMedicines.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool);
		default:
			if (fieldId >= 83)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_Medicines(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_medicines.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 2:
		{
			short item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetMaxDurability(item2, context);
			return;
		}
		case 3:
		{
			short item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetCurrDurability(item, context);
			return;
		}
		case 4:
		{
			byte item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetModificationState(item3, context);
			return;
		}
		}
		if (fieldId >= 83)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_Medicines(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_medicines.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesMedicines.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesMedicines.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_Medicines(int objectId, ushort fieldId)
	{
		if (_medicines.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 5)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesMedicines.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesMedicines.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_Medicines(int objectId, ushort fieldId)
	{
		if (!_medicines.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesMedicines.IsModified(value.DataStatesOffset, fieldId);
	}

	public TeaWine GetElement_TeaWines(int objectId)
	{
		return _teaWines[objectId];
	}

	public bool TryGetElement_TeaWines(int objectId, out TeaWine element)
	{
		return _teaWines.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_TeaWines(int objectId, TeaWine instance)
	{
		instance.CollectionHelperData = HelperDataTeaWines;
		instance.DataStatesOffset = _dataStatesTeaWines.Create();
		_teaWines.Add(objectId, instance);
		byte* pData = OperationAdder.FixedObjectCollection_Add(6, 9, objectId, 11);
		instance.Serialize(pData);
	}

	private void RemoveElement_TeaWines(int objectId)
	{
		if (_teaWines.TryGetValue(objectId, out var value))
		{
			_dataStatesTeaWines.Remove(value.DataStatesOffset);
			_teaWines.Remove(objectId);
			OperationAdder.FixedObjectCollection_Remove(6, 9, objectId);
		}
	}

	private void ClearTeaWines()
	{
		_dataStatesTeaWines.Clear();
		_teaWines.Clear();
		OperationAdder.FixedObjectCollection_Clear(6, 9);
	}

	public int GetElementField_TeaWines(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_teaWines.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_TeaWines", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesTeaWines.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool);
		default:
			if (fieldId >= 53)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_TeaWines(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_teaWines.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 2:
		{
			short item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetMaxDurability(item2, context);
			return;
		}
		case 3:
		{
			short item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetCurrDurability(item, context);
			return;
		}
		case 4:
		{
			byte item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetModificationState(item3, context);
			return;
		}
		}
		if (fieldId >= 53)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_TeaWines(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_teaWines.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesTeaWines.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesTeaWines.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_TeaWines(int objectId, ushort fieldId)
	{
		if (_teaWines.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 5)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesTeaWines.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesTeaWines.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_TeaWines(int objectId, ushort fieldId)
	{
		if (!_teaWines.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesTeaWines.IsModified(value.DataStatesOffset, fieldId);
	}

	public SkillBook GetElement_SkillBooks(int objectId)
	{
		return _skillBooks[objectId];
	}

	public bool TryGetElement_SkillBooks(int objectId, out SkillBook element)
	{
		return _skillBooks.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_SkillBooks(int objectId, SkillBook instance)
	{
		instance.CollectionHelperData = HelperDataSkillBooks;
		instance.DataStatesOffset = _dataStatesSkillBooks.Create();
		_skillBooks.Add(objectId, instance);
		byte* pData = OperationAdder.FixedObjectCollection_Add(6, 10, objectId, 14);
		instance.Serialize(pData);
	}

	private void RemoveElement_SkillBooks(int objectId)
	{
		if (_skillBooks.TryGetValue(objectId, out var value))
		{
			_dataStatesSkillBooks.Remove(value.DataStatesOffset);
			_skillBooks.Remove(objectId);
			OperationAdder.FixedObjectCollection_Remove(6, 10, objectId);
		}
	}

	private void ClearSkillBooks()
	{
		_dataStatesSkillBooks.Clear();
		_skillBooks.Clear();
		OperationAdder.FixedObjectCollection_Clear(6, 10);
	}

	public int GetElementField_SkillBooks(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_skillBooks.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_SkillBooks", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesSkillBooks.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool);
		case 5:
			return GameData.Serializer.Serializer.Serialize(value.GetPageTypes(), dataPool);
		case 6:
			return GameData.Serializer.Serializer.Serialize(value.GetPageIncompleteState(), dataPool);
		default:
			if (fieldId >= 40)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_SkillBooks(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_skillBooks.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 2:
		{
			short item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetMaxDurability(item2, context);
			return;
		}
		case 3:
		{
			short item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetCurrDurability(item, context);
			return;
		}
		case 4:
		{
			byte item5 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item5);
			value.SetModificationState(item5, context);
			return;
		}
		case 5:
		{
			byte item4 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item4);
			value.SetPageTypes(item4, context);
			return;
		}
		case 6:
		{
			ushort item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetPageIncompleteState(item3, context);
			return;
		}
		}
		if (fieldId >= 40)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 7)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_SkillBooks(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_skillBooks.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 7)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesSkillBooks.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesSkillBooks.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool), 
			5 => GameData.Serializer.Serializer.Serialize(value.GetPageTypes(), dataPool), 
			6 => GameData.Serializer.Serializer.Serialize(value.GetPageIncompleteState(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_SkillBooks(int objectId, ushort fieldId)
	{
		if (_skillBooks.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 7)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesSkillBooks.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesSkillBooks.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_SkillBooks(int objectId, ushort fieldId)
	{
		if (!_skillBooks.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 7)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesSkillBooks.IsModified(value.DataStatesOffset, fieldId);
	}

	public Cricket GetElement_Crickets(int objectId)
	{
		return _crickets[objectId];
	}

	public bool TryGetElement_Crickets(int objectId, out Cricket element)
	{
		return _crickets.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_Crickets(int objectId, Cricket instance)
	{
		instance.CollectionHelperData = HelperDataCrickets;
		instance.DataStatesOffset = _dataStatesCrickets.Create();
		_crickets.Add(objectId, instance);
		byte* pData = OperationAdder.FixedObjectCollection_Add(6, 11, objectId, 34);
		instance.Serialize(pData);
	}

	private void RemoveElement_Crickets(int objectId)
	{
		if (_crickets.TryGetValue(objectId, out var value))
		{
			_dataStatesCrickets.Remove(value.DataStatesOffset);
			_crickets.Remove(objectId);
			OperationAdder.FixedObjectCollection_Remove(6, 11, objectId);
		}
	}

	private void ClearCrickets()
	{
		_dataStatesCrickets.Clear();
		_crickets.Clear();
		OperationAdder.FixedObjectCollection_Clear(6, 11);
	}

	public int GetElementField_Crickets(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_crickets.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_Crickets", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesCrickets.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool);
		case 5:
			return GameData.Serializer.Serializer.Serialize(value.GetColorId(), dataPool);
		case 6:
			return GameData.Serializer.Serializer.Serialize(value.GetPartId(), dataPool);
		case 7:
			return GameData.Serializer.Serializer.Serialize(value.GetInjuries(), dataPool);
		case 8:
			return GameData.Serializer.Serializer.Serialize(value.GetWinsCount(), dataPool);
		case 9:
			return GameData.Serializer.Serializer.Serialize(value.GetLossesCount(), dataPool);
		case 10:
			return GameData.Serializer.Serializer.Serialize(value.GetBestEnemyColorId(), dataPool);
		case 11:
			return GameData.Serializer.Serializer.Serialize(value.GetBestEnemyPartId(), dataPool);
		case 12:
			return GameData.Serializer.Serializer.Serialize(value.GetAge(), dataPool);
		default:
			if (fieldId >= 39)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_Crickets(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_crickets.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 2:
		{
			short item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetMaxDurability(item2, context);
			return;
		}
		case 3:
		{
			short item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetCurrDurability(item, context);
			return;
		}
		case 4:
		{
			byte item9 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item9);
			value.SetModificationState(item9, context);
			return;
		}
		case 5:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 6:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 7:
		{
			short[] item8 = value.GetInjuries();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item8);
			value.SetInjuries(item8, context);
			return;
		}
		case 8:
		{
			short item7 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item7);
			value.SetWinsCount(item7, context);
			return;
		}
		case 9:
		{
			short item6 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item6);
			value.SetLossesCount(item6, context);
			return;
		}
		case 10:
		{
			short item5 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item5);
			value.SetBestEnemyColorId(item5, context);
			return;
		}
		case 11:
		{
			short item4 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item4);
			value.SetBestEnemyPartId(item4, context);
			return;
		}
		case 12:
		{
			sbyte item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetAge(item3, context);
			return;
		}
		}
		if (fieldId >= 39)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 13)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_Crickets(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_crickets.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 13)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesCrickets.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesCrickets.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool), 
			5 => GameData.Serializer.Serializer.Serialize(value.GetColorId(), dataPool), 
			6 => GameData.Serializer.Serializer.Serialize(value.GetPartId(), dataPool), 
			7 => GameData.Serializer.Serializer.Serialize(value.GetInjuries(), dataPool), 
			8 => GameData.Serializer.Serializer.Serialize(value.GetWinsCount(), dataPool), 
			9 => GameData.Serializer.Serializer.Serialize(value.GetLossesCount(), dataPool), 
			10 => GameData.Serializer.Serializer.Serialize(value.GetBestEnemyColorId(), dataPool), 
			11 => GameData.Serializer.Serializer.Serialize(value.GetBestEnemyPartId(), dataPool), 
			12 => GameData.Serializer.Serializer.Serialize(value.GetAge(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_Crickets(int objectId, ushort fieldId)
	{
		if (_crickets.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 13)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesCrickets.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesCrickets.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_Crickets(int objectId, ushort fieldId)
	{
		if (!_crickets.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 13)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesCrickets.IsModified(value.DataStatesOffset, fieldId);
	}

	public Misc GetElement_Misc(int objectId)
	{
		return _misc[objectId];
	}

	public bool TryGetElement_Misc(int objectId, out Misc element)
	{
		return _misc.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_Misc(int objectId, Misc instance)
	{
		instance.CollectionHelperData = HelperDataMisc;
		instance.DataStatesOffset = _dataStatesMisc.Create();
		_misc.Add(objectId, instance);
		byte* pData = OperationAdder.FixedObjectCollection_Add(6, 12, objectId, 11);
		instance.Serialize(pData);
	}

	private void RemoveElement_Misc(int objectId)
	{
		if (_misc.TryGetValue(objectId, out var value))
		{
			_dataStatesMisc.Remove(value.DataStatesOffset);
			_misc.Remove(objectId);
			OperationAdder.FixedObjectCollection_Remove(6, 12, objectId);
		}
	}

	private void ClearMisc()
	{
		_dataStatesMisc.Clear();
		_misc.Clear();
		OperationAdder.FixedObjectCollection_Clear(6, 12);
	}

	public int GetElementField_Misc(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_misc.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_Misc", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesMisc.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool);
		default:
			if (fieldId >= 47)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_Misc(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_misc.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 2:
		{
			short item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetMaxDurability(item2, context);
			return;
		}
		case 3:
		{
			short item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetCurrDurability(item, context);
			return;
		}
		case 4:
		{
			byte item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetModificationState(item3, context);
			return;
		}
		}
		if (fieldId >= 47)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_Misc(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_misc.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesMisc.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesMisc.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetTemplateId(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetMaxDurability(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetCurrDurability(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetModificationState(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_Misc(int objectId, ushort fieldId)
	{
		if (_misc.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 5)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesMisc.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesMisc.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_Misc(int objectId, ushort fieldId)
	{
		if (!_misc.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 5)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesMisc.IsModified(value.DataStatesOffset, fieldId);
	}

	private int GetNextItemId()
	{
		return _nextItemId;
	}

	private unsafe void SetNextItemId(int value, DataContext context)
	{
		_nextItemId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(6, 13, 4);
		*(int*)ptr = _nextItemId;
		ptr += 4;
	}

	private int GetElement_StackableItems(TemplateKey elementId)
	{
		return _stackableItems[elementId];
	}

	private bool TryGetElement_StackableItems(TemplateKey elementId, out int value)
	{
		return _stackableItems.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_StackableItems(TemplateKey elementId, int value, DataContext context)
	{
		_stackableItems.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(6, 14, elementId, 4);
		*(int*)ptr = value;
		ptr += 4;
	}

	private unsafe void SetElement_StackableItems(TemplateKey elementId, int value, DataContext context)
	{
		_stackableItems[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(6, 14, elementId, 4);
		*(int*)ptr = value;
		ptr += 4;
	}

	private void RemoveElement_StackableItems(TemplateKey elementId, DataContext context)
	{
		_stackableItems.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(6, 14, elementId);
	}

	private void ClearStackableItems(DataContext context)
	{
		_stackableItems.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(6, 14);
	}

	[Obsolete("DomainData _poisonItems is no longer in use.")]
	private PoisonEffects GetElement_PoisonItems(int elementId)
	{
		return _poisonItems[elementId];
	}

	[Obsolete("DomainData _poisonItems is no longer in use.")]
	private bool TryGetElement_PoisonItems(int elementId, out PoisonEffects value)
	{
		return _poisonItems.TryGetValue(elementId, out value);
	}

	[Obsolete("DomainData _poisonItems is no longer in use.")]
	private unsafe void AddElement_PoisonItems(int elementId, ref PoisonEffects value, DataContext context)
	{
		_poisonItems.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(6, 15, elementId, 21);
		ptr += value.Serialize(ptr);
	}

	[Obsolete("DomainData _poisonItems is no longer in use.")]
	private unsafe void SetElement_PoisonItems(int elementId, ref PoisonEffects value, DataContext context)
	{
		_poisonItems[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(6, 15, elementId, 21);
		ptr += value.Serialize(ptr);
	}

	[Obsolete("DomainData _poisonItems is no longer in use.")]
	private void RemoveElement_PoisonItems(int elementId, DataContext context)
	{
		_poisonItems.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(6, 15, elementId);
	}

	[Obsolete("DomainData _poisonItems is no longer in use.")]
	private void ClearPoisonItems(DataContext context)
	{
		_poisonItems.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(6, 15);
	}

	private RefiningEffects GetElement_RefinedItems(int elementId)
	{
		return _refinedItems[elementId];
	}

	private bool TryGetElement_RefinedItems(int elementId, out RefiningEffects value)
	{
		return _refinedItems.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_RefinedItems(int elementId, RefiningEffects value, DataContext context)
	{
		_refinedItems.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(6, 16, elementId, 10);
		ptr += value.Serialize(ptr);
	}

	private unsafe void SetElement_RefinedItems(int elementId, RefiningEffects value, DataContext context)
	{
		_refinedItems[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(6, 16, elementId, 10);
		ptr += value.Serialize(ptr);
	}

	private void RemoveElement_RefinedItems(int elementId, DataContext context)
	{
		_refinedItems.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(6, 16, elementId);
	}

	private void ClearRefinedItems(DataContext context)
	{
		_refinedItems.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(6, 16);
	}

	private ItemKey GetEmptyHandKey()
	{
		return _emptyHandKey;
	}

	private unsafe void SetEmptyHandKey(ItemKey value, DataContext context)
	{
		_emptyHandKey = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(6, 17, 8);
		ptr += _emptyHandKey.Serialize(ptr);
	}

	private ItemKey GetBranchKey()
	{
		return _branchKey;
	}

	private unsafe void SetBranchKey(ItemKey value, DataContext context)
	{
		_branchKey = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(6, 18, 8);
		ptr += _branchKey.Serialize(ptr);
	}

	private ItemKey GetStoneKey()
	{
		return _stoneKey;
	}

	private unsafe void SetStoneKey(ItemKey value, DataContext context)
	{
		_stoneKey = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(6, 19, 8);
		ptr += _stoneKey.Serialize(ptr);
	}

	private GameData.Utilities.ShortList GetElement_ExternEquipmentEffects(int elementId)
	{
		return _externEquipmentEffects[elementId];
	}

	private bool TryGetElement_ExternEquipmentEffects(int elementId, out GameData.Utilities.ShortList value)
	{
		return _externEquipmentEffects.TryGetValue(elementId, out value);
	}

	private void AddElement_ExternEquipmentEffects(int elementId, GameData.Utilities.ShortList value, DataContext context)
	{
		_externEquipmentEffects.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
	}

	private void SetElement_ExternEquipmentEffects(int elementId, GameData.Utilities.ShortList value, DataContext context)
	{
		_externEquipmentEffects[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
	}

	private void RemoveElement_ExternEquipmentEffects(int elementId, DataContext context)
	{
		_externEquipmentEffects.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
	}

	private void ClearExternEquipmentEffects(DataContext context)
	{
		_externEquipmentEffects.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public unsafe override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
		foreach (KeyValuePair<int, Weapon> weapon in _weapons)
		{
			int key = weapon.Key;
			Weapon value = weapon.Value;
			byte* pData = OperationAdder.DynamicObjectCollection_Add(6, 0, key, value.GetSerializedSize());
			value.Serialize(pData);
		}
		foreach (KeyValuePair<int, Armor> armor in _armors)
		{
			int key2 = armor.Key;
			Armor value2 = armor.Value;
			byte* pData2 = OperationAdder.FixedObjectCollection_Add(6, 1, key2, 29);
			value2.Serialize(pData2);
		}
		foreach (KeyValuePair<int, Accessory> accessory in _accessories)
		{
			int key3 = accessory.Key;
			Accessory value3 = accessory.Value;
			byte* pData3 = OperationAdder.FixedObjectCollection_Add(6, 2, key3, 29);
			value3.Serialize(pData3);
		}
		foreach (KeyValuePair<int, Clothing> item in _clothing)
		{
			int key4 = item.Key;
			Clothing value4 = item.Value;
			byte* pData4 = OperationAdder.FixedObjectCollection_Add(6, 3, key4, 30);
			value4.Serialize(pData4);
		}
		foreach (KeyValuePair<int, Carrier> carrier in _carriers)
		{
			int key5 = carrier.Key;
			Carrier value5 = carrier.Value;
			byte* pData5 = OperationAdder.FixedObjectCollection_Add(6, 4, key5, 29);
			value5.Serialize(pData5);
		}
		foreach (KeyValuePair<int, Material> material in _materials)
		{
			int key6 = material.Key;
			Material value6 = material.Value;
			byte* pData6 = OperationAdder.FixedObjectCollection_Add(6, 5, key6, 11);
			value6.Serialize(pData6);
		}
		foreach (KeyValuePair<int, CraftTool> craftTool in _craftTools)
		{
			int key7 = craftTool.Key;
			CraftTool value7 = craftTool.Value;
			byte* pData7 = OperationAdder.FixedObjectCollection_Add(6, 6, key7, 11);
			value7.Serialize(pData7);
		}
		foreach (KeyValuePair<int, Food> food in _foods)
		{
			int key8 = food.Key;
			Food value8 = food.Value;
			byte* pData8 = OperationAdder.FixedObjectCollection_Add(6, 7, key8, 11);
			value8.Serialize(pData8);
		}
		foreach (KeyValuePair<int, Medicine> medicine in _medicines)
		{
			int key9 = medicine.Key;
			Medicine value9 = medicine.Value;
			byte* pData9 = OperationAdder.FixedObjectCollection_Add(6, 8, key9, 11);
			value9.Serialize(pData9);
		}
		foreach (KeyValuePair<int, TeaWine> teaWine in _teaWines)
		{
			int key10 = teaWine.Key;
			TeaWine value10 = teaWine.Value;
			byte* pData10 = OperationAdder.FixedObjectCollection_Add(6, 9, key10, 11);
			value10.Serialize(pData10);
		}
		foreach (KeyValuePair<int, SkillBook> skillBook in _skillBooks)
		{
			int key11 = skillBook.Key;
			SkillBook value11 = skillBook.Value;
			byte* pData11 = OperationAdder.FixedObjectCollection_Add(6, 10, key11, 14);
			value11.Serialize(pData11);
		}
		foreach (KeyValuePair<int, Cricket> cricket in _crickets)
		{
			int key12 = cricket.Key;
			Cricket value12 = cricket.Value;
			byte* pData12 = OperationAdder.FixedObjectCollection_Add(6, 11, key12, 34);
			value12.Serialize(pData12);
		}
		foreach (KeyValuePair<int, Misc> item2 in _misc)
		{
			int key13 = item2.Key;
			Misc value13 = item2.Value;
			byte* pData13 = OperationAdder.FixedObjectCollection_Add(6, 12, key13, 11);
			value13.Serialize(pData13);
		}
		byte* ptr = OperationAdder.FixedSingleValue_Set(6, 13, 4);
		*(int*)ptr = _nextItemId;
		ptr += 4;
		foreach (KeyValuePair<TemplateKey, int> stackableItem in _stackableItems)
		{
			TemplateKey key14 = stackableItem.Key;
			int value14 = stackableItem.Value;
			byte* ptr2 = OperationAdder.FixedSingleValueCollection_Add(6, 14, key14, 4);
			*(int*)ptr2 = value14;
			ptr2 += 4;
		}
		foreach (KeyValuePair<int, PoisonEffects> poisonItem in _poisonItems)
		{
			int key15 = poisonItem.Key;
			PoisonEffects value15 = poisonItem.Value;
			byte* ptr3 = OperationAdder.FixedSingleValueCollection_Add(6, 15, key15, 21);
			ptr3 += value15.Serialize(ptr3);
		}
		foreach (KeyValuePair<int, RefiningEffects> refinedItem in _refinedItems)
		{
			int key16 = refinedItem.Key;
			RefiningEffects value16 = refinedItem.Value;
			byte* ptr4 = OperationAdder.FixedSingleValueCollection_Add(6, 16, key16, 10);
			ptr4 += value16.Serialize(ptr4);
		}
		byte* ptr5 = OperationAdder.FixedSingleValue_Set(6, 17, 8);
		ptr5 += _emptyHandKey.Serialize(ptr5);
		byte* ptr6 = OperationAdder.FixedSingleValue_Set(6, 18, 8);
		ptr6 += _branchKey.Serialize(ptr6);
		byte* ptr7 = OperationAdder.FixedSingleValue_Set(6, 19, 8);
		ptr7 += _stoneKey.Serialize(ptr7);
	}

	public override void OnLoadWorld()
	{
		_pendingLoadingOperationIds = new Queue<uint>();
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicObjectCollection_GetAllObjects(6, 0));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 1));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 2));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 3));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 4));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 5));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 6));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 7));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 8));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 9));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 10));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 11));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(6, 12));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(6, 13));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(6, 14));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(6, 15));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(6, 16));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(6, 17));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(6, 18));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(6, 19));
	}

	public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
	{
		return dataId switch
		{
			0 => GetElementField_Weapons((int)subId0, (ushort)subId1, dataPool, resetModified), 
			1 => GetElementField_Armors((int)subId0, (ushort)subId1, dataPool, resetModified), 
			2 => GetElementField_Accessories((int)subId0, (ushort)subId1, dataPool, resetModified), 
			3 => GetElementField_Clothing((int)subId0, (ushort)subId1, dataPool, resetModified), 
			4 => GetElementField_Carriers((int)subId0, (ushort)subId1, dataPool, resetModified), 
			5 => GetElementField_Materials((int)subId0, (ushort)subId1, dataPool, resetModified), 
			6 => GetElementField_CraftTools((int)subId0, (ushort)subId1, dataPool, resetModified), 
			7 => GetElementField_Foods((int)subId0, (ushort)subId1, dataPool, resetModified), 
			8 => GetElementField_Medicines((int)subId0, (ushort)subId1, dataPool, resetModified), 
			9 => GetElementField_TeaWines((int)subId0, (ushort)subId1, dataPool, resetModified), 
			10 => GetElementField_SkillBooks((int)subId0, (ushort)subId1, dataPool, resetModified), 
			11 => GetElementField_Crickets((int)subId0, (ushort)subId1, dataPool, resetModified), 
			12 => GetElementField_Misc((int)subId0, (ushort)subId1, dataPool, resetModified), 
			13 => throw new Exception($"Not allow to get value of dataId: {dataId}"), 
			14 => throw new Exception($"Not allow to get value of dataId: {dataId}"), 
			15 => throw new Exception($"Not allow to get value of dataId: {dataId}"), 
			16 => throw new Exception($"Not allow to get value of dataId: {dataId}"), 
			17 => throw new Exception($"Not allow to get value of dataId: {dataId}"), 
			18 => throw new Exception($"Not allow to get value of dataId: {dataId}"), 
			19 => throw new Exception($"Not allow to get value of dataId: {dataId}"), 
			20 => throw new Exception($"Not allow to get value of dataId: {dataId}"), 
			_ => throw new Exception($"Unsupported dataId {dataId}"), 
		};
	}

	public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		switch (dataId)
		{
		case 0:
			SetElementField_Weapons((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 1:
			SetElementField_Armors((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 2:
			SetElementField_Accessories((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 3:
			SetElementField_Clothing((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 4:
			SetElementField_Carriers((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 5:
			SetElementField_Materials((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 6:
			SetElementField_CraftTools((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 7:
			SetElementField_Foods((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 8:
			SetElementField_Medicines((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 9:
			SetElementField_TeaWines((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 10:
			SetElementField_SkillBooks((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 11:
			SetElementField_Crickets((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 12:
			SetElementField_Misc((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 13:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 14:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 15:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 16:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 17:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 18:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 19:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 20:
			throw new Exception($"Not allow to set value of dataId {dataId}");
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
		{
			int argsCount24 = operation.ArgsCount;
			int num24 = argsCount24;
			if (num24 == 2)
			{
				int item100 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item100);
				ItemDisplayData item101 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item101);
				List<ItemDisplayData> item102 = IdentifyPoisons(context, item100, item101);
				return GameData.Serializer.Serializer.Serialize(item102, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 1:
		{
			int argsCount9 = operation.ArgsCount;
			int num9 = argsCount9;
			if (num9 == 4)
			{
				short item21 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item21);
				short item22 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item22);
				short item23 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item23);
				sbyte item24 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item24);
				List<ItemDisplayData> item25 = CatchCricket(context, item21, item22, item23, item24);
				return GameData.Serializer.Serializer.Serialize(item25, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 2:
		{
			int argsCount27 = operation.ArgsCount;
			int num27 = argsCount27;
			if (num27 == 1)
			{
				int item109 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item109);
				CricketData cricketData = GetCricketData(item109);
				return GameData.Serializer.Serializer.Serialize(cricketData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 3:
		{
			int argsCount2 = operation.ArgsCount;
			int num2 = argsCount2;
			if (num2 == 3)
			{
				int item3 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item3);
				bool item4 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item4);
				int item5 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item5);
				SetCricketRecord(context, item3, item4, item5);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 4:
		{
			int argsCount14 = operation.ArgsCount;
			int num14 = argsCount14;
			if (num14 == 3)
			{
				int item37 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item37);
				int item38 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item38);
				short item39 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item39);
				AddCricketInjury(context, item37, item38, item39);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 5:
		{
			int argsCount32 = operation.ArgsCount;
			int num32 = argsCount32;
			if (num32 == 1)
			{
				ItemKey item119 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item119);
				List<sbyte> weaponTricks = GetWeaponTricks(item119);
				return GameData.Serializer.Serializer.Serialize(weaponTricks, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 6:
		{
			int argsCount20 = operation.ArgsCount;
			int num20 = argsCount20;
			if (num20 == 1)
			{
				ItemKey item93 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item93);
				short[] cricketCombatRecords = GetCricketCombatRecords(item93);
				return GameData.Serializer.Serializer.Serialize(cricketCombatRecords, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 7:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				ItemKey item82 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item82);
				ItemDisplayData itemDisplayData2 = GetItemDisplayData(item82);
				return GameData.Serializer.Serializer.Serialize(itemDisplayData2, returnDataPool);
			}
			case 2:
			{
				ItemKey item80 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item80);
				int item81 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item81);
				ItemDisplayData itemDisplayData = GetItemDisplayData(item80, item81);
				return GameData.Serializer.Serializer.Serialize(itemDisplayData, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 8:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				List<ItemKey> item42 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item42);
				List<ItemDisplayData> itemDisplayDataList2 = GetItemDisplayDataList(item42);
				return GameData.Serializer.Serializer.Serialize(itemDisplayDataList2, returnDataPool);
			}
			case 2:
			{
				List<ItemKey> item40 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item40);
				int item41 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item41);
				List<ItemDisplayData> itemDisplayDataList = GetItemDisplayDataList(item40, item41);
				return GameData.Serializer.Serializer.Serialize(itemDisplayDataList, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 9:
		{
			int argsCount6 = operation.ArgsCount;
			int num6 = argsCount6;
			if (num6 == 1)
			{
				ItemKey item13 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item13);
				SkillBookPageDisplayData skillBookPagesInfo = GetSkillBookPagesInfo(item13);
				return GameData.Serializer.Serializer.Serialize(skillBookPagesInfo, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 10:
		{
			int argsCount29 = operation.ArgsCount;
			int num29 = argsCount29;
			if (num29 == 1)
			{
				ItemKey item111 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item111);
				int value = GetValue(item111);
				return GameData.Serializer.Serializer.Serialize(value, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 11:
		{
			int argsCount23 = operation.ArgsCount;
			int num23 = argsCount23;
			if (num23 == 1)
			{
				ItemKey item99 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item99);
				int price = GetPrice(item99);
				return GameData.Serializer.Serializer.Serialize(price, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 12:
		{
			int argsCount17 = operation.ArgsCount;
			int num17 = argsCount17;
			if (num17 == 3)
			{
				int item65 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item65);
				ItemKey item66 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item66);
				ItemKey item67 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item67);
				List<ItemDisplayData> item68 = DisassembleItem(context, item65, item66, item67);
				return GameData.Serializer.Serializer.Serialize(item68, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 13:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				int item53 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item53);
				ItemKey item54 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item54);
				DiscardItem(context, item53, item54);
				return -1;
			}
			case 3:
			{
				int item50 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item50);
				ItemKey item51 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item51);
				int item52 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item52);
				DiscardItem(context, item50, item51, item52);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 14:
		{
			int argsCount11 = operation.ArgsCount;
			int num11 = argsCount11;
			if (num11 == 2)
			{
				int item28 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item28);
				ItemKey item29 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item29);
				List<ItemKey> repairableItems = GetRepairableItems(context, item28, item29);
				return GameData.Serializer.Serializer.Serialize(repairableItems, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 15:
		{
			int argsCount5 = operation.ArgsCount;
			int num5 = argsCount5;
			if (num5 == 2)
			{
				int item11 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item11);
				ItemKey item12 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item12);
				List<ItemKey> disassemblableItems = GetDisassemblableItems(context, item11, item12);
				return GameData.Serializer.Serializer.Serialize(disassemblableItems, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 16:
		{
			int argsCount30 = operation.ArgsCount;
			int num30 = argsCount30;
			if (num30 == 5)
			{
				int item112 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item112);
				short item113 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item113);
				sbyte item114 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item114);
				short item115 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item115);
				short item116 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item116);
				ChangeDurability(context, item112, item113, item114, item115, item116);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 17:
		{
			int argsCount26 = operation.ArgsCount;
			int num26 = argsCount26;
			if (num26 == 2)
			{
				int item107 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item107);
				bool item108 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item108);
				ChangePoisonIdentified(context, item107, item108);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 18:
		{
			int argsCount21 = operation.ArgsCount;
			int num21 = argsCount21;
			if (num21 == 2)
			{
				int item94 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item94);
				List<ItemKey> item95 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item95);
				DiscardItemList(context, item94, item95);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 19:
		{
			int argsCount19 = operation.ArgsCount;
			int num19 = argsCount19;
			if (num19 == 2)
			{
				int item77 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item77);
				List<MultiplyOperation> item78 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item78);
				List<ItemDisplayData> item79 = DisassembleItemList(context, item77, item78);
				return GameData.Serializer.Serializer.Serialize(item79, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 20:
			switch (operation.ArgsCount)
			{
			case 3:
			{
				int item73 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item73);
				ItemKey item74 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item74);
				sbyte item75 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item75);
				DiscardItemOptional(context, item73, item74, item75);
				return -1;
			}
			case 4:
			{
				int item69 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item69);
				ItemKey item70 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item70);
				sbyte item71 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item71);
				int item72 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item72);
				DiscardItemOptional(context, item69, item70, item71, item72);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 21:
		{
			int argsCount16 = operation.ArgsCount;
			int num16 = argsCount16;
			if (num16 == 3)
			{
				int item47 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item47);
				List<ItemKey> item48 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item48);
				sbyte item49 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item49);
				DiscardItemListOptional(context, item47, item48, item49);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 22:
		{
			int argsCount12 = operation.ArgsCount;
			int num12 = argsCount12;
			if (num12 == 5)
			{
				int item30 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item30);
				ItemKey item31 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item31);
				ItemKey item32 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item32);
				sbyte item33 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item33);
				sbyte item34 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item34);
				List<ItemDisplayData> item35 = DisassembleItemOptional(context, item30, item31, item32, item33, item34);
				return GameData.Serializer.Serializer.Serialize(item35, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 23:
		{
			int argsCount8 = operation.ArgsCount;
			int num8 = argsCount8;
			if (num8 == 3)
			{
				sbyte item18 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item18);
				sbyte item19 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item19);
				bool item20 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item20);
				SetCricketBattleConfig(item18, item19, item20);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 24:
		{
			int argsCount3 = operation.ArgsCount;
			int num3 = argsCount3;
			if (num3 == 1)
			{
				List<ItemKey> item6 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item6);
				List<CricketData> cricketDataList = GetCricketDataList(item6);
				return GameData.Serializer.Serializer.Serialize(cricketDataList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 25:
		{
			int argsCount31 = operation.ArgsCount;
			int num31 = argsCount31;
			if (num31 == 2)
			{
				int item117 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item117);
				ItemKey item118 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item118);
				(int, int) weaponAttackRange = GetWeaponAttackRange(item117, item118);
				return GameData.Serializer.Serializer.Serialize(weaponAttackRange, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 26:
		{
			int argsCount28 = operation.ArgsCount;
			int num28 = argsCount28;
			if (num28 == 1)
			{
				List<ItemKey> item110 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item110);
				bool[] cricketsAliveState = GetCricketsAliveState(item110);
				return GameData.Serializer.Serializer.Serialize(cricketsAliveState, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 27:
		{
			int argsCount25 = operation.ArgsCount;
			int num25 = argsCount25;
			if (num25 == 3)
			{
				ItemKey item103 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item103);
				List<byte> item104 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item104);
				List<sbyte> item105 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item105);
				bool item106 = ModifyCombatSkillBookPageNormal(context, item103, item104, item105);
				return GameData.Serializer.Serializer.Serialize(item106, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 28:
		{
			int argsCount22 = operation.ArgsCount;
			int num22 = argsCount22;
			if (num22 == 2)
			{
				ItemKey item96 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item96);
				sbyte item97 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item97);
				bool item98 = ModifyCombatSkillBookPageOutline(context, item96, item97);
				return GameData.Serializer.Serializer.Serialize(item98, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 29:
			if (operation.ArgsCount == 0)
			{
				List<SkillBookModifyDisplayData> taiwuInventoryCombatSkillBooks = GetTaiwuInventoryCombatSkillBooks();
				return GameData.Serializer.Serializer.Serialize(taiwuInventoryCombatSkillBooks, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 30:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				List<ItemKey> item92 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item92);
				List<ItemDisplayData> itemDisplayDataListOptional4 = GetItemDisplayDataListOptional(item92, -1, -1);
				return GameData.Serializer.Serializer.Serialize(itemDisplayDataListOptional4, returnDataPool);
			}
			case 2:
			{
				List<ItemKey> item90 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item90);
				int item91 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item91);
				List<ItemDisplayData> itemDisplayDataListOptional3 = GetItemDisplayDataListOptional(item90, item91, -1);
				return GameData.Serializer.Serializer.Serialize(itemDisplayDataListOptional3, returnDataPool);
			}
			case 3:
			{
				List<ItemKey> item87 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item87);
				int item88 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item88);
				sbyte item89 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item89);
				List<ItemDisplayData> itemDisplayDataListOptional2 = GetItemDisplayDataListOptional(item87, item88, item89);
				return GameData.Serializer.Serializer.Serialize(itemDisplayDataListOptional2, returnDataPool);
			}
			case 4:
			{
				List<ItemKey> item83 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item83);
				int item84 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item84);
				sbyte item85 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item85);
				bool item86 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item86);
				List<ItemDisplayData> itemDisplayDataListOptional = GetItemDisplayDataListOptional(item83, item84, item85, item86);
				return GameData.Serializer.Serializer.Serialize(itemDisplayDataListOptional, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 31:
		{
			int argsCount18 = operation.ArgsCount;
			int num18 = argsCount18;
			if (num18 == 1)
			{
				List<ItemKey> item76 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item76);
				List<SkillBookPageDisplayData> skillBookPageDisplayDataList = GetSkillBookPageDisplayDataList(item76);
				return GameData.Serializer.Serializer.Serialize(skillBookPageDisplayDataList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 32:
			if (operation.ArgsCount == 0)
			{
				ItemKey emptyToolKey = GetEmptyToolKey(context);
				return GameData.Serializer.Serializer.Serialize(emptyToolKey, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 33:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				Inventory item64 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item64);
				List<ItemDisplayData> itemDisplayDataListOptionalFromInventory4 = GetItemDisplayDataListOptionalFromInventory(item64, -1, -1);
				return GameData.Serializer.Serializer.Serialize(itemDisplayDataListOptionalFromInventory4, returnDataPool);
			}
			case 2:
			{
				Inventory item62 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item62);
				int item63 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item63);
				List<ItemDisplayData> itemDisplayDataListOptionalFromInventory3 = GetItemDisplayDataListOptionalFromInventory(item62, item63, -1);
				return GameData.Serializer.Serializer.Serialize(itemDisplayDataListOptionalFromInventory3, returnDataPool);
			}
			case 3:
			{
				Inventory item59 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item59);
				int item60 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item60);
				sbyte item61 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item61);
				List<ItemDisplayData> itemDisplayDataListOptionalFromInventory2 = GetItemDisplayDataListOptionalFromInventory(item59, item60, item61);
				return GameData.Serializer.Serializer.Serialize(itemDisplayDataListOptionalFromInventory2, returnDataPool);
			}
			case 4:
			{
				Inventory item55 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item55);
				int item56 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item56);
				sbyte item57 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item57);
				bool item58 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item58);
				List<ItemDisplayData> itemDisplayDataListOptionalFromInventory = GetItemDisplayDataListOptionalFromInventory(item55, item56, item57, item58);
				return GameData.Serializer.Serializer.Serialize(itemDisplayDataListOptionalFromInventory, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 34:
		{
			int argsCount15 = operation.ArgsCount;
			int num15 = argsCount15;
			if (num15 == 3)
			{
				bool item43 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item43);
				ItemKey[] item44 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item44);
				short[] item45 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item45);
				bool item46 = SettlementCricketWager(context, item43, item44, item45);
				return GameData.Serializer.Serializer.Serialize(item46, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 35:
		{
			int argsCount13 = operation.ArgsCount;
			int num13 = argsCount13;
			if (num13 == 1)
			{
				int item36 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item36);
				GmCmd_StartCricketCombat(context, item36);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 36:
		{
			int argsCount10 = operation.ArgsCount;
			int num10 = argsCount10;
			if (num10 == 1)
			{
				bool item26 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item26);
				bool item27 = SettlementCricketWagerByGiveUp(context, item26);
				return GameData.Serializer.Serializer.Serialize(item27, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 37:
		{
			int argsCount7 = operation.ArgsCount;
			int num7 = argsCount7;
			if (num7 == 1)
			{
				ItemKey item17 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item17);
				MakeCricketRebirth(context, item17);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 38:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				ItemKey item16 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item16);
				int repairItemNeedResourceCount2 = GetRepairItemNeedResourceCount(item16, -1);
				return GameData.Serializer.Serializer.Serialize(repairItemNeedResourceCount2, returnDataPool);
			}
			case 2:
			{
				ItemKey item14 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item14);
				short item15 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item15);
				int repairItemNeedResourceCount = GetRepairItemNeedResourceCount(item14, item15);
				return GameData.Serializer.Serializer.Serialize(repairItemNeedResourceCount, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 39:
		{
			int argsCount4 = operation.ArgsCount;
			int num4 = argsCount4;
			if (num4 == 3)
			{
				ItemKey item7 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item7);
				sbyte item8 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item8);
				List<sbyte> item9 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item9);
				bool item10 = SetCombatSkillBookPage(context, item7, item8, item9);
				return GameData.Serializer.Serializer.Serialize(item10, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 40:
		{
			int argsCount = operation.ArgsCount;
			int num = argsCount;
			if (num == 2)
			{
				int item = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item);
				ItemKey item2 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item2);
				int weaponPrepareFrame = GetWeaponPrepareFrame(item, item2);
				return GameData.Serializer.Serializer.Serialize(weaponPrepareFrame, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
	{
		switch (dataId)
		{
		case 0:
			return;
		case 1:
			return;
		case 2:
			return;
		case 3:
			return;
		case 4:
			return;
		case 5:
			return;
		case 6:
			return;
		case 7:
			return;
		case 8:
			return;
		case 9:
			return;
		case 10:
			return;
		case 11:
			return;
		case 12:
			return;
		case 13:
			return;
		case 14:
			return;
		case 15:
			return;
		case 16:
			return;
		case 17:
			return;
		case 18:
			return;
		case 19:
			return;
		case 20:
			return;
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
	{
		return dataId switch
		{
			0 => CheckModified_Weapons((int)subId0, (ushort)subId1, dataPool), 
			1 => CheckModified_Armors((int)subId0, (ushort)subId1, dataPool), 
			2 => CheckModified_Accessories((int)subId0, (ushort)subId1, dataPool), 
			3 => CheckModified_Clothing((int)subId0, (ushort)subId1, dataPool), 
			4 => CheckModified_Carriers((int)subId0, (ushort)subId1, dataPool), 
			5 => CheckModified_Materials((int)subId0, (ushort)subId1, dataPool), 
			6 => CheckModified_CraftTools((int)subId0, (ushort)subId1, dataPool), 
			7 => CheckModified_Foods((int)subId0, (ushort)subId1, dataPool), 
			8 => CheckModified_Medicines((int)subId0, (ushort)subId1, dataPool), 
			9 => CheckModified_TeaWines((int)subId0, (ushort)subId1, dataPool), 
			10 => CheckModified_SkillBooks((int)subId0, (ushort)subId1, dataPool), 
			11 => CheckModified_Crickets((int)subId0, (ushort)subId1, dataPool), 
			12 => CheckModified_Misc((int)subId0, (ushort)subId1, dataPool), 
			13 => throw new Exception($"Not allow to check modification of dataId {dataId}"), 
			14 => throw new Exception($"Not allow to check modification of dataId {dataId}"), 
			15 => throw new Exception($"Not allow to check modification of dataId {dataId}"), 
			16 => throw new Exception($"Not allow to check modification of dataId {dataId}"), 
			17 => throw new Exception($"Not allow to check modification of dataId {dataId}"), 
			18 => throw new Exception($"Not allow to check modification of dataId {dataId}"), 
			19 => throw new Exception($"Not allow to check modification of dataId {dataId}"), 
			20 => throw new Exception($"Not allow to check modification of dataId {dataId}"), 
			_ => throw new Exception($"Unsupported dataId {dataId}"), 
		};
	}

	public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		switch (dataId)
		{
		case 0:
			ResetModifiedWrapper_Weapons((int)subId0, (ushort)subId1);
			break;
		case 1:
			ResetModifiedWrapper_Armors((int)subId0, (ushort)subId1);
			break;
		case 2:
			ResetModifiedWrapper_Accessories((int)subId0, (ushort)subId1);
			break;
		case 3:
			ResetModifiedWrapper_Clothing((int)subId0, (ushort)subId1);
			break;
		case 4:
			ResetModifiedWrapper_Carriers((int)subId0, (ushort)subId1);
			break;
		case 5:
			ResetModifiedWrapper_Materials((int)subId0, (ushort)subId1);
			break;
		case 6:
			ResetModifiedWrapper_CraftTools((int)subId0, (ushort)subId1);
			break;
		case 7:
			ResetModifiedWrapper_Foods((int)subId0, (ushort)subId1);
			break;
		case 8:
			ResetModifiedWrapper_Medicines((int)subId0, (ushort)subId1);
			break;
		case 9:
			ResetModifiedWrapper_TeaWines((int)subId0, (ushort)subId1);
			break;
		case 10:
			ResetModifiedWrapper_SkillBooks((int)subId0, (ushort)subId1);
			break;
		case 11:
			ResetModifiedWrapper_Crickets((int)subId0, (ushort)subId1);
			break;
		case 12:
			ResetModifiedWrapper_Misc((int)subId0, (ushort)subId1);
			break;
		case 13:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 14:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 15:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 16:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 17:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 18:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 19:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 20:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		return dataId switch
		{
			0 => IsModifiedWrapper_Weapons((int)subId0, (ushort)subId1), 
			1 => IsModifiedWrapper_Armors((int)subId0, (ushort)subId1), 
			2 => IsModifiedWrapper_Accessories((int)subId0, (ushort)subId1), 
			3 => IsModifiedWrapper_Clothing((int)subId0, (ushort)subId1), 
			4 => IsModifiedWrapper_Carriers((int)subId0, (ushort)subId1), 
			5 => IsModifiedWrapper_Materials((int)subId0, (ushort)subId1), 
			6 => IsModifiedWrapper_CraftTools((int)subId0, (ushort)subId1), 
			7 => IsModifiedWrapper_Foods((int)subId0, (ushort)subId1), 
			8 => IsModifiedWrapper_Medicines((int)subId0, (ushort)subId1), 
			9 => IsModifiedWrapper_TeaWines((int)subId0, (ushort)subId1), 
			10 => IsModifiedWrapper_SkillBooks((int)subId0, (ushort)subId1), 
			11 => IsModifiedWrapper_Crickets((int)subId0, (ushort)subId1), 
			12 => IsModifiedWrapper_Misc((int)subId0, (ushort)subId1), 
			13 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			14 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			15 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			16 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			17 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			18 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			19 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			20 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			_ => throw new Exception($"Unsupported dataId {dataId}"), 
		};
	}

	public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
	{
		switch (influence.TargetIndicator.DataId)
		{
		case 0:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list2 = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _weapons, list2))
				{
					int count3 = list2.Count;
					for (int k = 0; k < count3; k++)
					{
						BaseGameDataObject baseGameDataObject2 = list2[k];
						List<DataUid> targetUids2 = influence.TargetUids;
						int count4 = targetUids2.Count;
						for (int l = 0; l < count4; l++)
						{
							baseGameDataObject2.InvalidateSelfAndInfluencedCache((ushort)targetUids2[l].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesWeapons, _dataStatesWeapons, influence, context);
				}
				list2.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list2);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesWeapons, _dataStatesWeapons, influence, context);
			}
			break;
		case 1:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list4 = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _armors, list4))
				{
					int count7 = list4.Count;
					for (int num = 0; num < count7; num++)
					{
						BaseGameDataObject baseGameDataObject4 = list4[num];
						List<DataUid> targetUids4 = influence.TargetUids;
						int count8 = targetUids4.Count;
						for (int num2 = 0; num2 < count8; num2++)
						{
							baseGameDataObject4.InvalidateSelfAndInfluencedCache((ushort)targetUids4[num2].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesArmors, _dataStatesArmors, influence, context);
				}
				list4.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list4);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesArmors, _dataStatesArmors, influence, context);
			}
			break;
		case 2:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list9 = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _accessories, list9))
				{
					int count17 = list9.Count;
					for (int num11 = 0; num11 < count17; num11++)
					{
						BaseGameDataObject baseGameDataObject9 = list9[num11];
						List<DataUid> targetUids9 = influence.TargetUids;
						int count18 = targetUids9.Count;
						for (int num12 = 0; num12 < count18; num12++)
						{
							baseGameDataObject9.InvalidateSelfAndInfluencedCache((ushort)targetUids9[num12].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesAccessories, _dataStatesAccessories, influence, context);
				}
				list9.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list9);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesAccessories, _dataStatesAccessories, influence, context);
			}
			break;
		case 3:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list5 = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _clothing, list5))
				{
					int count9 = list5.Count;
					for (int num3 = 0; num3 < count9; num3++)
					{
						BaseGameDataObject baseGameDataObject5 = list5[num3];
						List<DataUid> targetUids5 = influence.TargetUids;
						int count10 = targetUids5.Count;
						for (int num4 = 0; num4 < count10; num4++)
						{
							baseGameDataObject5.InvalidateSelfAndInfluencedCache((ushort)targetUids5[num4].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesClothing, _dataStatesClothing, influence, context);
				}
				list5.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list5);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesClothing, _dataStatesClothing, influence, context);
			}
			break;
		case 4:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list12 = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _carriers, list12))
				{
					int count23 = list12.Count;
					for (int num17 = 0; num17 < count23; num17++)
					{
						BaseGameDataObject baseGameDataObject12 = list12[num17];
						List<DataUid> targetUids12 = influence.TargetUids;
						int count24 = targetUids12.Count;
						for (int num18 = 0; num18 < count24; num18++)
						{
							baseGameDataObject12.InvalidateSelfAndInfluencedCache((ushort)targetUids12[num18].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesCarriers, _dataStatesCarriers, influence, context);
				}
				list12.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list12);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesCarriers, _dataStatesCarriers, influence, context);
			}
			break;
		case 5:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list11 = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _materials, list11))
				{
					int count21 = list11.Count;
					for (int num15 = 0; num15 < count21; num15++)
					{
						BaseGameDataObject baseGameDataObject11 = list11[num15];
						List<DataUid> targetUids11 = influence.TargetUids;
						int count22 = targetUids11.Count;
						for (int num16 = 0; num16 < count22; num16++)
						{
							baseGameDataObject11.InvalidateSelfAndInfluencedCache((ushort)targetUids11[num16].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesMaterials, _dataStatesMaterials, influence, context);
				}
				list11.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list11);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesMaterials, _dataStatesMaterials, influence, context);
			}
			break;
		case 6:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list10 = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _craftTools, list10))
				{
					int count19 = list10.Count;
					for (int num13 = 0; num13 < count19; num13++)
					{
						BaseGameDataObject baseGameDataObject10 = list10[num13];
						List<DataUid> targetUids10 = influence.TargetUids;
						int count20 = targetUids10.Count;
						for (int num14 = 0; num14 < count20; num14++)
						{
							baseGameDataObject10.InvalidateSelfAndInfluencedCache((ushort)targetUids10[num14].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesCraftTools, _dataStatesCraftTools, influence, context);
				}
				list10.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list10);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesCraftTools, _dataStatesCraftTools, influence, context);
			}
			break;
		case 7:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list6 = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _foods, list6))
				{
					int count11 = list6.Count;
					for (int num5 = 0; num5 < count11; num5++)
					{
						BaseGameDataObject baseGameDataObject6 = list6[num5];
						List<DataUid> targetUids6 = influence.TargetUids;
						int count12 = targetUids6.Count;
						for (int num6 = 0; num6 < count12; num6++)
						{
							baseGameDataObject6.InvalidateSelfAndInfluencedCache((ushort)targetUids6[num6].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesFoods, _dataStatesFoods, influence, context);
				}
				list6.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list6);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesFoods, _dataStatesFoods, influence, context);
			}
			break;
		case 8:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list3 = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _medicines, list3))
				{
					int count5 = list3.Count;
					for (int m = 0; m < count5; m++)
					{
						BaseGameDataObject baseGameDataObject3 = list3[m];
						List<DataUid> targetUids3 = influence.TargetUids;
						int count6 = targetUids3.Count;
						for (int n = 0; n < count6; n++)
						{
							baseGameDataObject3.InvalidateSelfAndInfluencedCache((ushort)targetUids3[n].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesMedicines, _dataStatesMedicines, influence, context);
				}
				list3.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list3);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesMedicines, _dataStatesMedicines, influence, context);
			}
			break;
		case 9:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list13 = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _teaWines, list13))
				{
					int count25 = list13.Count;
					for (int num19 = 0; num19 < count25; num19++)
					{
						BaseGameDataObject baseGameDataObject13 = list13[num19];
						List<DataUid> targetUids13 = influence.TargetUids;
						int count26 = targetUids13.Count;
						for (int num20 = 0; num20 < count26; num20++)
						{
							baseGameDataObject13.InvalidateSelfAndInfluencedCache((ushort)targetUids13[num20].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesTeaWines, _dataStatesTeaWines, influence, context);
				}
				list13.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list13);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesTeaWines, _dataStatesTeaWines, influence, context);
			}
			break;
		case 10:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list7 = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _skillBooks, list7))
				{
					int count13 = list7.Count;
					for (int num7 = 0; num7 < count13; num7++)
					{
						BaseGameDataObject baseGameDataObject7 = list7[num7];
						List<DataUid> targetUids7 = influence.TargetUids;
						int count14 = targetUids7.Count;
						for (int num8 = 0; num8 < count14; num8++)
						{
							baseGameDataObject7.InvalidateSelfAndInfluencedCache((ushort)targetUids7[num8].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesSkillBooks, _dataStatesSkillBooks, influence, context);
				}
				list7.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list7);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesSkillBooks, _dataStatesSkillBooks, influence, context);
			}
			break;
		case 11:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list8 = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _crickets, list8))
				{
					int count15 = list8.Count;
					for (int num9 = 0; num9 < count15; num9++)
					{
						BaseGameDataObject baseGameDataObject8 = list8[num9];
						List<DataUid> targetUids8 = influence.TargetUids;
						int count16 = targetUids8.Count;
						for (int num10 = 0; num10 < count16; num10++)
						{
							baseGameDataObject8.InvalidateSelfAndInfluencedCache((ushort)targetUids8[num10].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesCrickets, _dataStatesCrickets, influence, context);
				}
				list8.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list8);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesCrickets, _dataStatesCrickets, influence, context);
			}
			break;
		case 12:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _misc, list))
				{
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						BaseGameDataObject baseGameDataObject = list[i];
						List<DataUid> targetUids = influence.TargetUids;
						int count2 = targetUids.Count;
						for (int j = 0; j < count2; j++)
						{
							baseGameDataObject.InvalidateSelfAndInfluencedCache((ushort)targetUids[j].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesMisc, _dataStatesMisc, influence, context);
				}
				list.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesMisc, _dataStatesMisc, influence, context);
			}
			break;
		default:
			throw new Exception($"Unsupported dataId {influence.TargetIndicator.DataId}");
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
		case 20:
			throw new Exception($"Cannot invalidate cache state of non-cache data {influence.TargetIndicator.DataId}");
		}
	}

	public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		uint num;
		switch (operation.DataId)
		{
		case 0:
			ResponseProcessor.ProcessDynamicObjectCollection(operation, pResult, _weapons);
			goto IL_0213;
		case 1:
			ResponseProcessor.ProcessFixedObjectCollection(operation, pResult, _armors);
			goto IL_0213;
		case 2:
			ResponseProcessor.ProcessFixedObjectCollection(operation, pResult, _accessories);
			goto IL_0213;
		case 3:
			ResponseProcessor.ProcessFixedObjectCollection(operation, pResult, _clothing);
			goto IL_0213;
		case 4:
			ResponseProcessor.ProcessFixedObjectCollection(operation, pResult, _carriers);
			goto IL_0213;
		case 5:
			ResponseProcessor.ProcessFixedObjectCollection(operation, pResult, _materials);
			goto IL_0213;
		case 6:
			ResponseProcessor.ProcessFixedObjectCollection(operation, pResult, _craftTools);
			goto IL_0213;
		case 7:
			ResponseProcessor.ProcessFixedObjectCollection(operation, pResult, _foods);
			goto IL_0213;
		case 8:
			ResponseProcessor.ProcessFixedObjectCollection(operation, pResult, _medicines);
			goto IL_0213;
		case 9:
			ResponseProcessor.ProcessFixedObjectCollection(operation, pResult, _teaWines);
			goto IL_0213;
		case 10:
			ResponseProcessor.ProcessFixedObjectCollection(operation, pResult, _skillBooks);
			goto IL_0213;
		case 11:
			ResponseProcessor.ProcessFixedObjectCollection(operation, pResult, _crickets);
			goto IL_0213;
		case 12:
			ResponseProcessor.ProcessFixedObjectCollection(operation, pResult, _misc);
			goto IL_0213;
		case 13:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _nextItemId);
			goto IL_0213;
		case 14:
			ResponseProcessor.ProcessSingleValueCollection_BasicType_Fixed_Value(operation, pResult, _stackableItems);
			goto IL_0213;
		case 15:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Value<int, PoisonEffects>(operation, pResult, (IDictionary<int, PoisonEffects>)_poisonItems, 21);
			goto IL_0213;
		case 16:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Value<int, RefiningEffects>(operation, pResult, (IDictionary<int, RefiningEffects>)_refinedItems, 10);
			goto IL_0213;
		case 17:
			ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Single<ItemKey>(operation, pResult, ref _emptyHandKey);
			goto IL_0213;
		case 18:
			ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Single<ItemKey>(operation, pResult, ref _branchKey);
			goto IL_0213;
		case 19:
			ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Single<ItemKey>(operation, pResult, ref _stoneKey);
			goto IL_0213;
		default:
			throw new Exception($"Unsupported dataId {operation.DataId}");
		case 20:
			{
				throw new Exception($"Cannot process archive response of non-archive data {operation.DataId}");
			}
			IL_0213:
			if (_pendingLoadingOperationIds == null)
			{
				break;
			}
			num = _pendingLoadingOperationIds.Peek();
			if (num == operation.Id)
			{
				_pendingLoadingOperationIds.Dequeue();
				if (_pendingLoadingOperationIds.Count <= 0)
				{
					_pendingLoadingOperationIds = null;
					InitializeInternalDataOfCollections();
					OnLoadedArchiveData();
					DomainManager.Global.CompleteLoading(6);
				}
			}
			break;
		}
	}

	private void InitializeInternalDataOfCollections()
	{
		foreach (KeyValuePair<int, Weapon> weapon in _weapons)
		{
			Weapon value = weapon.Value;
			value.CollectionHelperData = HelperDataWeapons;
			value.DataStatesOffset = _dataStatesWeapons.Create();
		}
		foreach (KeyValuePair<int, Armor> armor in _armors)
		{
			Armor value2 = armor.Value;
			value2.CollectionHelperData = HelperDataArmors;
			value2.DataStatesOffset = _dataStatesArmors.Create();
		}
		foreach (KeyValuePair<int, Accessory> accessory in _accessories)
		{
			Accessory value3 = accessory.Value;
			value3.CollectionHelperData = HelperDataAccessories;
			value3.DataStatesOffset = _dataStatesAccessories.Create();
		}
		foreach (KeyValuePair<int, Clothing> item in _clothing)
		{
			Clothing value4 = item.Value;
			value4.CollectionHelperData = HelperDataClothing;
			value4.DataStatesOffset = _dataStatesClothing.Create();
		}
		foreach (KeyValuePair<int, Carrier> carrier in _carriers)
		{
			Carrier value5 = carrier.Value;
			value5.CollectionHelperData = HelperDataCarriers;
			value5.DataStatesOffset = _dataStatesCarriers.Create();
		}
		foreach (KeyValuePair<int, Material> material in _materials)
		{
			Material value6 = material.Value;
			value6.CollectionHelperData = HelperDataMaterials;
			value6.DataStatesOffset = _dataStatesMaterials.Create();
		}
		foreach (KeyValuePair<int, CraftTool> craftTool in _craftTools)
		{
			CraftTool value7 = craftTool.Value;
			value7.CollectionHelperData = HelperDataCraftTools;
			value7.DataStatesOffset = _dataStatesCraftTools.Create();
		}
		foreach (KeyValuePair<int, Food> food in _foods)
		{
			Food value8 = food.Value;
			value8.CollectionHelperData = HelperDataFoods;
			value8.DataStatesOffset = _dataStatesFoods.Create();
		}
		foreach (KeyValuePair<int, Medicine> medicine in _medicines)
		{
			Medicine value9 = medicine.Value;
			value9.CollectionHelperData = HelperDataMedicines;
			value9.DataStatesOffset = _dataStatesMedicines.Create();
		}
		foreach (KeyValuePair<int, TeaWine> teaWine in _teaWines)
		{
			TeaWine value10 = teaWine.Value;
			value10.CollectionHelperData = HelperDataTeaWines;
			value10.DataStatesOffset = _dataStatesTeaWines.Create();
		}
		foreach (KeyValuePair<int, SkillBook> skillBook in _skillBooks)
		{
			SkillBook value11 = skillBook.Value;
			value11.CollectionHelperData = HelperDataSkillBooks;
			value11.DataStatesOffset = _dataStatesSkillBooks.Create();
		}
		foreach (KeyValuePair<int, Cricket> cricket in _crickets)
		{
			Cricket value12 = cricket.Value;
			value12.CollectionHelperData = HelperDataCrickets;
			value12.DataStatesOffset = _dataStatesCrickets.Create();
		}
		foreach (KeyValuePair<int, Misc> item2 in _misc)
		{
			Misc value13 = item2.Value;
			value13.CollectionHelperData = HelperDataMisc;
			value13.DataStatesOffset = _dataStatesMisc.Create();
		}
	}
}
