using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Config;
using Config.Common;
using Config.ConfigCells.Character;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Common.SingleValueCollection;
using GameData.Dependencies;
using GameData.Domains.Building.Display;
using GameData.Domains.Building.SamsaraPlatformRecord;
using GameData.Domains.Building.ShopEvent;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Creation;
using GameData.Domains.Character.Display;
using GameData.Domains.Character.Filters;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Global;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Organization.Display;
using GameData.Domains.Taiwu;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.VillagerRole;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using GameData.Utilities.RandomGenerator;
using NLog;
using Redzen.Random;

namespace GameData.Domains.Building;

[GameDataDomain(9)]
public class BuildingDomain : BaseGameDataDomain
{
	private struct ItemWithSource
	{
		public ItemKey ItemKey;

		public int Amount;

		public int Source;

		public int IndexInSource;
	}

	public enum MultiSelectOperateType
	{
		InventoryToSold = 1,
		SoldToInventory,
		WarehouseToSold,
		SoldToWarehouse,
		TreasuryToSold,
		SoldToTreasury,
		StockStorageGoodsShelfToSold,
		SoldToStockStorageGoodsShelf
	}

	public class TeaHorseCaravanState
	{
		public const sbyte None = 0;

		public const sbyte Ready = 1;

		public const sbyte Forward = 2;

		public const sbyte Return = 3;

		public const sbyte ReadyGetItem = 4;
	}

	public enum SaveInfectedType
	{
		Save = 1,
		Release,
		Expel,
		Kill
	}

	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private Dictionary<Location, BuildingAreaData> _buildingAreas;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private Dictionary<BuildingBlockKey, BuildingBlockData> _buildingBlocks;

	[DomainData(DomainDataType.SingleValue, true, false, true, false)]
	private List<Location> _taiwuBuildingAreas;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
	private Dictionary<BuildingBlockKey, sbyte> _CollectBuildingResourceType;

	[DomainData(DomainDataType.SingleValueCollection, false, false, true, false)]
	private Dictionary<BuildingBlockKey, CharacterList> _buildingOperatorDict;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private Dictionary<BuildingBlockKey, int> _customBuildingName;

	private bool _needUpdateEffects;

	public const short DependMaxDistance = 2;

	[DomainData(DomainDataType.SingleValue, true, false, false, false)]
	private List<BuildingBlockKey> _newCompleteOperationBuildings;

	private List<BuildingBlockKey> _newBrokenBuildings = new List<BuildingBlockKey>();

	private Dictionary<short, List<BuildingBlockData>> _alreadyUpdateShopProgressBlock = new Dictionary<short, List<BuildingBlockData>>();

	public readonly short BaseWorkContribution = 250;

	public readonly short AttainmentToProb = 20;

	private readonly BuildingExceptionData _buildingExceptionData = new BuildingExceptionData();

	private readonly HashSet<BuildingBlockKey> _buildingAutoExpandStoppedNotifiedSet = new HashSet<BuildingBlockKey>();

	private readonly BuildingFormulaContextBridge _formulaContextBridge = new BuildingFormulaContextBridge();

	private readonly Dictionary<Location, IBuildingEffectValue[]> _buildingBlockEffectsCache = new Dictionary<Location, IBuildingEffectValue[]>();

	private readonly BuildingFormulaContextBridge.CalcArgument _formulaArgHandler = CalcBuildingFormulaContextArg;

	[Obsolete]
	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private Dictionary<int, ChickenBlessingInfoData> _chickenBlessingInfoData;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private Dictionary<int, Chicken> _chicken;

	private int _nextChickenId;

	public List<short> ChickenMapInfo = null;

	private readonly Dictionary<int, List<int>> _settlementChickenIdLists = new Dictionary<int, List<int>>();

	[Obsolete("Use GameData.Domains.Extra.CricketCollectionData.CricketCollectionCapacity instead")]
	public const int CricketCollectionCapacity = 15;

	[Obsolete("Use GameData.Domains.Extra.CricketCollectionData instead")]
	[DomainData(DomainDataType.SingleValue, true, false, true, true, ArrayElementsCount = 15)]
	private ItemKey[] _collectionCrickets;

	[Obsolete("Use GameData.Domains.Extra.CricketCollectionData instead")]
	[DomainData(DomainDataType.SingleValue, true, false, true, true, ArrayElementsCount = 15)]
	private ItemKey[] _collectionCricketJars;

	[Obsolete("Use GameData.Domains.Extra.CricketCollectionData instead")]
	[DomainData(DomainDataType.SingleValue, true, false, true, true, ArrayElementsCount = 15)]
	private int[] _collectionCricketRegen;

	[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
	private Dictionary<BuildingBlockKey, MakeItemData> _makeItemDict;

	private bool _outsideMakeItem = false;

	[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
	private readonly Dictionary<BuildingBlockKey, CharacterList> _residences;

	[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
	private readonly Dictionary<BuildingBlockKey, CharacterList> _comfortableHouses;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private CharacterList _homeless;

	private Dictionary<int, BuildingBlockKey> _buildingResidents;

	private Dictionary<int, BuildingBlockKey> _buildingComfortableHouses;

	private Dictionary<int, (short, int, ItemKey)> _feastParticipants = new Dictionary<int, (short, int, ItemKey)>();

	private const int ResourceBlockBaseValueCount = 5;

	[DomainData(DomainDataType.SingleValue, true, false, true, false)]
	private MainAttributes _samsaraPlatformAddMainAttributes;

	[DomainData(DomainDataType.SingleValue, true, false, true, false)]
	private CombatSkillShorts _samsaraPlatformAddCombatSkillQualifications;

	[DomainData(DomainDataType.SingleValue, true, false, true, false)]
	private LifeSkillShorts _samsaraPlatformAddLifeSkillQualifications;

	[DomainData(DomainDataType.ElementList, true, false, true, true, ArrayElementsCount = 6)]
	private readonly IntPair[] _samsaraPlatformSlots;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
	private readonly Dictionary<int, IntPair> _samsaraPlatformBornDict;

	private int _temporaryPossessionCharId = -1;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private Dictionary<BuildingBlockKey, BuildingEarningsData> _collectBuildingEarningsData;

	[DomainData(DomainDataType.SingleValueCollection, false, false, true, false)]
	private Dictionary<BuildingBlockKey, CharacterList> _shopManagerDict;

	[DomainData(DomainDataType.SingleValue, true, false, true, false)]
	private TeaHorseCaravanData _teaHorseCaravanData;

	private Dictionary<BuildingBlockKey, ShopEventCollection> _shopEventCollections;

	public readonly short _caravanReplenishmentInitValue = 100;

	public readonly short _caravanAwarenessInitValue = 100;

	private readonly short _caravanReplenishmentCostPerMonth = 5;

	private List<short> _westTreasureTemplateId = new List<short> { 28, 29, 30, 31, 32 };

	private readonly short[][] WestItemArrayTwo = new short[9][]
	{
		new short[5] { 28, 29, 30, 31, 32 },
		new short[5] { 33, 34, 35, 36, 37 },
		new short[5] { 38, 39, 40, 41, 42 },
		new short[5] { 43, 44, 45, 46, 47 },
		new short[5] { 48, 49, 50, 51, 52 },
		new short[5] { 53, 54, 55, 56, 57 },
		new short[5] { 58, 59, 60, 61, 62 },
		new short[5] { 63, 64, 65, 66, 67 },
		new short[5] { 68, 69, 70, 71, 72 }
	};

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private ushort _shrineBuyTimes;

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[25][];

	private SingleValueCollectionModificationCollection<Location> _modificationsBuildingAreas = SingleValueCollectionModificationCollection<Location>.Create();

	private SingleValueCollectionModificationCollection<BuildingBlockKey> _modificationsBuildingBlocks = SingleValueCollectionModificationCollection<BuildingBlockKey>.Create();

	private SingleValueCollectionModificationCollection<BuildingBlockKey> _modificationsCollectBuildingResourceType = SingleValueCollectionModificationCollection<BuildingBlockKey>.Create();

	private SingleValueCollectionModificationCollection<BuildingBlockKey> _modificationsBuildingOperatorDict = SingleValueCollectionModificationCollection<BuildingBlockKey>.Create();

	private SingleValueCollectionModificationCollection<BuildingBlockKey> _modificationsCustomBuildingName = SingleValueCollectionModificationCollection<BuildingBlockKey>.Create();

	private SingleValueCollectionModificationCollection<int> _modificationsChickenBlessingInfoData = SingleValueCollectionModificationCollection<int>.Create();

	private SingleValueCollectionModificationCollection<int> _modificationsChicken = SingleValueCollectionModificationCollection<int>.Create();

	private static readonly DataInfluence[][] CacheInfluencesSamsaraPlatformSlots = new DataInfluence[6][];

	private readonly byte[] _dataStatesSamsaraPlatformSlots = new byte[2];

	private SingleValueCollectionModificationCollection<int> _modificationsSamsaraPlatformBornDict = SingleValueCollectionModificationCollection<int>.Create();

	private SingleValueCollectionModificationCollection<BuildingBlockKey> _modificationsCollectBuildingEarningsData = SingleValueCollectionModificationCollection<BuildingBlockKey>.Create();

	private SingleValueCollectionModificationCollection<BuildingBlockKey> _modificationsShopManagerDict = SingleValueCollectionModificationCollection<BuildingBlockKey>.Create();

	private Queue<uint> _pendingLoadingOperationIds;

	[Obsolete]
	private Dictionary<BuildingBlockKey, List<ShopEventData>> _shopEventDict;

	private void OnInitializedDomainData()
	{
		_buildingResidents = new Dictionary<int, BuildingBlockKey>();
		_buildingComfortableHouses = new Dictionary<int, BuildingBlockKey>();
	}

	private void InitializeOnInitializeGameDataModule()
	{
	}

	private void InitializeOnEnterNewWorld()
	{
		InitializeCricketCollection();
		InitializeSamsaraPlatform();
	}

	private void OnLoadedArchiveData()
	{
		Dictionary<int, VillagerWorkData> villagerWorkDict = DomainManager.Taiwu.GetVillagerWorkDict();
		DataContext currentThreadDataContext = DataContextManager.GetCurrentThreadDataContext();
		foreach (int key in villagerWorkDict.Keys)
		{
			VillagerWorkData villagerWorkData = villagerWorkDict[key];
			BuildingBlockKey blockKey = new BuildingBlockKey(villagerWorkData.AreaId, villagerWorkData.BlockId, villagerWorkData.BuildingBlockIndex);
			switch (villagerWorkData.WorkType)
			{
			case 0:
				SetBuildingOperator(currentThreadDataContext, blockKey, villagerWorkData.WorkerIndex, villagerWorkData.CharacterId);
				break;
			case 1:
				SetShopBuildingManager(currentThreadDataContext, blockKey, villagerWorkData.WorkerIndex, villagerWorkData.CharacterId, setArtisanOrder: false);
				break;
			}
		}
		FixResidentData(currentThreadDataContext);
		InitializeBuildingResidents(currentThreadDataContext);
		InitializeNextChickenId();
		RefreshSettlementChickenIdLists();
	}

	public override void OnCurrWorldArchiveDataReady(DataContext context, bool isNewWorld)
	{
		UpgradeTeaHorseCaravanByAwareness(context);
	}

	public override void OnUpdate(DataContext context)
	{
		base.OnUpdate(context);
		if (_needUpdateEffects)
		{
			UpdateTaiwuVillageBuildingEffect();
			_needUpdateEffects = false;
		}
	}

	[DomainMethod]
	public BuildingAreaData GetBuildingAreaData(Location location)
	{
		return GetElement_BuildingAreas(location);
	}

	[DomainMethod]
	public List<BuildingBlockData> GetBuildingBlockList(Location location)
	{
		List<BuildingBlockData> list = new List<BuildingBlockData>();
		foreach (KeyValuePair<BuildingBlockKey, BuildingBlockData> buildingBlock in _buildingBlocks)
		{
			if (buildingBlock.Key.AreaId == location.AreaId && buildingBlock.Key.BlockId == location.BlockId)
			{
				list.Add(buildingBlock.Value);
			}
		}
		list.Sort((BuildingBlockData block1, BuildingBlockData block2) => block1.BlockIndex.CompareTo(block2.BlockIndex));
		return list;
	}

	[DomainMethod]
	public BuildingBlockData GetBuildingBlockData(BuildingBlockKey blockKey)
	{
		return GetElement_BuildingBlocks(blockKey);
	}

	[DomainMethod]
	public void SetBuildingCustomName(DataContext context, BuildingBlockKey blockKey, string name)
	{
		int value;
		bool flag = _customBuildingName.TryGetValue(blockKey, out value);
		if (!(flag ? (DomainManager.World.GetElement_CustomTexts(value) == name) : string.IsNullOrEmpty(name)))
		{
			if (flag)
			{
				DomainManager.World.UnregisterCustomText(context, value);
				RemoveElement_CustomBuildingName(blockKey, context);
			}
			if (!string.IsNullOrEmpty(name))
			{
				value = DomainManager.World.RegisterCustomText(context, name);
				AddElement_CustomBuildingName(blockKey, value, context);
			}
		}
	}

	[DomainMethod]
	public int GetEmptyBlockCount(short areaId, short blockId)
	{
		int num = 0;
		BuildingAreaData buildingAreaData = _buildingAreas[new Location(areaId, blockId)];
		for (short num2 = 0; num2 < buildingAreaData.Width * buildingAreaData.Width; num2++)
		{
			if (IsBuildingBlocksEmpty(areaId, blockId, num2, buildingAreaData.Width, 1))
			{
				num++;
			}
		}
		return num;
	}

	[DomainMethod]
	public int GetSutraReadingRoomBuffValue()
	{
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		int buildingBlockEffect = GetBuildingBlockEffect(taiwuVillageSettlementId, EBuildingScaleEffect.ReadingStrategyCost);
		return Math.Clamp(100 - buildingBlockEffect, 0, 100);
	}

	[DomainMethod]
	public int PracticingCombatSkillInPracticeRoom(DataContext context, BuildingBlockKey blockKey, short skillTemplateId, int count, int cost)
	{
		int num = 0;
		CombatSkillKey combatSkillKey = new CombatSkillKey(DomainManager.Taiwu.GetTaiwuCharId(), skillTemplateId);
		int num2 = 1;
		int num3 = 0;
		int num4 = 0;
		int value;
		int num5 = (DomainManager.Extra.TryGetElement_CombatSkillProficiencies(combatSkillKey, out value) ? value : 0);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		BuildingBlockData buildingBlockData = GetBuildingBlockData(blockKey);
		short templateId = buildingBlockData.TemplateId;
		Location location = new Location(blockKey.AreaId, blockKey.BlockId);
		bool flag = DomainManager.Taiwu.GetTaiwuVillageLocation() == location;
		BuildingAreaData element_BuildingAreas = GetElement_BuildingAreas(location);
		sbyte width = BuildingBlock.Instance[_buildingBlocks[blockKey].TemplateId].Width;
		element_BuildingAreas.GetNeighborBlocks(blockKey.BuildingBlockIndex, width, list, null, 2);
		foreach (short item in list)
		{
			BuildingBlockData buildingBlockData2 = _buildingBlocks[new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, item)];
			if (buildingBlockData2.RootBlockIndex >= 0)
			{
				buildingBlockData2 = _buildingBlocks[new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, buildingBlockData2.RootBlockIndex)];
			}
			if (buildingBlockData2.TemplateId == 0 || !buildingBlockData2.CanUse())
			{
				continue;
			}
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[buildingBlockData2.TemplateId];
			if (buildingBlockItem.DependBuildings != null && buildingBlockItem.DependBuildings.Contains(templateId))
			{
				if (buildingBlockItem.ReduceCombatSkillCost >= 0)
				{
					num3 = 1;
				}
				if (buildingBlockItem.AddCombatSkillBreakout >= 0)
				{
					num4 = 1;
				}
			}
		}
		num2 += num3 + num4;
		if (!flag)
		{
			num2 *= 3;
		}
		ObjectPool<List<short>>.Instance.Return(list);
		for (int i = 0; i < count; i++)
		{
			num += context.Random.Next(GlobalConfig.Instance.BaseCombatSkillPracticeProficiencyDelta[0], GlobalConfig.Instance.BaseCombatSkillPracticeProficiencyDelta[1]) * num2;
		}
		DomainManager.Extra.ConsumeActionPoint(context, cost);
		DomainManager.Extra.ChangeCombatSkillProficiency(context, combatSkillKey, num);
		int baseDelta = ProfessionFormulaImpl.Calculate(113, DomainManager.Extra.GetElement_CombatSkillProficiencies(combatSkillKey) - num5);
		DomainManager.Extra.ChangeProfessionSeniority(context, 3, baseDelta);
		return num;
	}

	public List<short> GetAvailableBuildingBlocks(List<short> buildingBlocks, short areaId, short blockId, sbyte areaWidth, sbyte buildingWidth)
	{
		List<short> list = new List<short>();
		foreach (short buildingBlock in buildingBlocks)
		{
			if (IsBuildingBlocksEmpty(areaId, blockId, buildingBlock, areaWidth, buildingWidth))
			{
				list.Add(buildingBlock);
			}
		}
		return list;
	}

	public short GetRootBlockContainingIndex(short areaId, short blockId, short index, sbyte areaWidth, sbyte buildingWidth)
	{
		for (int i = 0; i < buildingWidth; i++)
		{
			for (int j = 0; j < buildingWidth; j++)
			{
				short num = (short)(index - i * areaWidth - j);
				if (IsBuildingBlocksEmpty(areaId, blockId, num, areaWidth, buildingWidth))
				{
					return num;
				}
			}
		}
		return -1;
	}

	public bool RootContainBlock(short areaId, short blockId, short rootIndex, short blockIndex, sbyte areaWidth, sbyte buildingWidth)
	{
		for (int i = 0; i < buildingWidth; i++)
		{
			for (int j = 0; j < buildingWidth; j++)
			{
				short num = (short)(rootIndex + i * areaWidth + j);
				if (num == blockIndex)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsBuildingBlocksEmpty(short areaId, short blockId, short rootIndex, sbyte areaWidth, sbyte buildingWidth)
	{
		int num = rootIndex % areaWidth;
		int num2 = rootIndex / areaWidth;
		if (num + buildingWidth > areaWidth || num2 + buildingWidth > areaWidth)
		{
			return false;
		}
		for (int i = 0; i < buildingWidth; i++)
		{
			for (int j = 0; j < buildingWidth; j++)
			{
				short buildingBlockIndex = (short)(rootIndex + i * areaWidth + j);
				BuildingBlockKey key = new BuildingBlockKey(areaId, blockId, buildingBlockIndex);
				if (_buildingBlocks.ContainsKey(key))
				{
					BuildingBlockData buildingBlockData = _buildingBlocks[key];
					if (buildingBlockData.TemplateId > 0 || buildingBlockData.RootBlockIndex >= 0)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public void ResetAllChildrenBlocks(DataContext context, BuildingBlockKey blockKey, short templateId, sbyte level = 1)
	{
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(blockKey);
		if (element_BuildingBlocks.TemplateId < 0)
		{
			return;
		}
		int num = Math.Max(BuildingBlock.Instance[element_BuildingBlocks.TemplateId].Width, BuildingBlock.Instance[templateId].Width);
		MapBlockItem mapBlockItem = MapBlock.Instance[DomainManager.Map.GetBlock(blockKey.AreaId, blockKey.BlockId).TemplateId];
		sbyte buildingAreaWidth = mapBlockItem.BuildingAreaWidth;
		int num2 = element_BuildingBlocks.BlockIndex % buildingAreaWidth;
		int num3 = element_BuildingBlocks.BlockIndex / buildingAreaWidth;
		for (int i = num2; i < Math.Min(num2 + num, buildingAreaWidth); i++)
		{
			for (int j = num3; j < Math.Min(num3 + num, buildingAreaWidth); j++)
			{
				short num4 = (short)(j * buildingAreaWidth + i);
				BuildingBlockKey buildingBlockKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, num4);
				if (templateId > 0 && templateId != 23 && num4 != blockKey.BuildingBlockIndex)
				{
					_buildingBlocks[buildingBlockKey].ResetData(-1, -1, element_BuildingBlocks.BlockIndex);
				}
				else
				{
					_buildingBlocks[buildingBlockKey].ResetData(templateId, level, -1);
				}
				SetElement_BuildingBlocks(buildingBlockKey, _buildingBlocks[buildingBlockKey], context);
			}
		}
	}

	public void AddBuilding(DataContext context, short areaId, short blockId, short centerIndex, short templateId, sbyte level, sbyte areaWidth)
	{
		ExtraDomain extra = DomainManager.Extra;
		sbyte width = BuildingBlock.Instance[templateId].Width;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < width; j++)
			{
				short num = (short)(centerIndex + i * areaWidth + j);
				BuildingBlockKey buildingBlockKey = new BuildingBlockKey(areaId, blockId, num);
				if (_buildingBlocks.ContainsKey(buildingBlockKey))
				{
					Logger.AppendWarning($"Building {BuildingBlock.Instance[templateId].Name} with key {buildingBlockKey} is being added to a occupied place at {MapBlock.Instance[DomainManager.Map.GetBlock(areaId, blockId).TemplateId].Name}");
				}
				BuildingBlockData value = ((num == centerIndex) ? new BuildingBlockData(num, templateId, level, -1) : new BuildingBlockData(num, -1, -1, centerIndex));
				AddElement_BuildingBlocks(buildingBlockKey, value, context);
				extra.ModifyBuildingExtraData(context, buildingBlockKey);
			}
		}
	}

	public void PlaceBuilding(DataContext context, short areaId, short blockId, short rootIndex, BuildingBlockData blockData, sbyte areaWidth)
	{
		sbyte width = BuildingBlock.Instance[blockData.TemplateId].Width;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < width; j++)
			{
				short num = (short)(rootIndex + i * areaWidth + j);
				BuildingBlockKey elementId = new BuildingBlockKey(areaId, blockId, num);
				BuildingBlockData value = ((num == rootIndex) ? blockData : new BuildingBlockData(num, -1, -1, rootIndex));
				SetElement_BuildingBlocks(elementId, value, context);
			}
		}
	}

	[Obsolete]
	public void PlaceBuildingAtRandomBlock(DataContext context, short areaId, short blockId, short templateId, bool forcePlace)
	{
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[templateId];
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		BuildingAreaData buildingArea = _buildingAreas[new Location(areaId, blockId)];
		sbyte areaWidth = buildingArea.Width;
		sbyte buildingWidth = buildingBlockItem.Width;
		for (short num = 0; num < buildingArea.Width * buildingArea.Width; num++)
		{
			if (IsBuildingBlocksEmpty(areaId, blockId, num, areaWidth, buildingBlockItem.Width))
			{
				list.Add(num);
			}
		}
		if (list.Count == 0)
		{
			if (!forcePlace)
			{
				ObjectPool<List<short>>.Instance.Return(list);
				return;
			}
			CalcAvailableBlockIndices(list, (short buildingTemplateId) => BuildingBlock.Instance[buildingTemplateId].Type != EBuildingBlockType.UselessResource && BuildingBlock.Instance[buildingTemplateId].Type != EBuildingBlockType.Empty);
			if (list.Count == 0)
			{
				CalcAvailableBlockIndices(list, (short buildingTemplateId) => !BuildingBlockData.IsResource(BuildingBlock.Instance[buildingTemplateId].Type) && BuildingBlock.Instance[buildingTemplateId].Type != EBuildingBlockType.Empty);
			}
		}
		list.Sort((short l, short r) => GetDisToCenter(l, buildingArea.Width) - GetDisToCenter(r, buildingArea.Width));
		short num2 = list[0];
		BuildingBlockKey elementId = new BuildingBlockKey(areaId, blockId, num2);
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(elementId);
		element_BuildingBlocks.ResetData(templateId, 1, -1);
		PlaceBuilding(context, areaId, blockId, num2, element_BuildingBlocks, buildingArea.Width);
		void CalcAvailableBlockIndices(List<short> blocks, Func<short, bool> func)
		{
			for (short num3 = 0; num3 < buildingArea.Width * buildingArea.Width; num3++)
			{
				int num4 = num3 % areaWidth;
				int num5 = num3 / areaWidth;
				if (num4 + buildingWidth <= areaWidth && num5 + buildingWidth <= areaWidth)
				{
					bool flag = true;
					for (int i = 0; i < buildingWidth; i++)
					{
						for (int j = 0; j < buildingWidth; j++)
						{
							short buildingBlockIndex = (short)(num3 + i * areaWidth + j);
							BuildingBlockKey key = new BuildingBlockKey(areaId, blockId, buildingBlockIndex);
							BuildingBlockData buildingBlockData = _buildingBlocks[key];
							if (buildingBlockData.RootBlockIndex >= 0)
							{
								flag = false;
								break;
							}
							if (buildingBlockData.TemplateId >= 0 && func(buildingBlockData.TemplateId))
							{
								flag = false;
								break;
							}
						}
						if (!flag)
						{
							break;
						}
					}
					if (flag)
					{
						blocks.Add(num3);
					}
				}
			}
		}
		static int GetDisToCenter(short blockIndex, sbyte width)
		{
			int num3 = blockIndex % width;
			int num4 = blockIndex / width;
			int num5 = width / 2 - 1;
			return Math.Abs(num3 - num5) + Math.Abs(num4 - num5);
		}
	}

	public void PlaceBuildingAtBlock(DataContext context, short areaId, short blockId, short templateId, bool forcePlace, bool isRandom)
	{
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[templateId];
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		BuildingAreaData buildingArea = _buildingAreas[new Location(areaId, blockId)];
		sbyte areaWidth = buildingArea.Width;
		sbyte buildingWidth = buildingBlockItem.Width;
		for (short num = 0; num < buildingArea.Width * buildingArea.Width; num++)
		{
			if (!IsEdge(num) && IsBuildingBlocksEmpty(areaId, blockId, num, areaWidth, buildingBlockItem.Width))
			{
				list.Add(num);
			}
		}
		if (list.Count == 0)
		{
			if (!forcePlace)
			{
				ObjectPool<List<short>>.Instance.Return(list);
				return;
			}
			CalcAvailableBlockIndices(list, (short buildingTemplateId) => BuildingBlock.Instance[buildingTemplateId].Type != EBuildingBlockType.UselessResource && BuildingBlock.Instance[buildingTemplateId].Type != EBuildingBlockType.Empty);
			if (list.Count == 0)
			{
				CalcAvailableBlockIndices(list, (short buildingTemplateId) => !BuildingBlockData.IsResource(BuildingBlock.Instance[buildingTemplateId].Type) && BuildingBlock.Instance[buildingTemplateId].Type != EBuildingBlockType.Empty);
			}
		}
		short num2 = -1;
		if (isRandom)
		{
			num2 = list.GetRandom(context.Random);
		}
		else
		{
			list.Sort((short l, short r) => GetDisToCenter(l, buildingArea.Width) - GetDisToCenter(r, buildingArea.Width));
			num2 = list[0];
		}
		BuildingBlockKey elementId = new BuildingBlockKey(areaId, blockId, num2);
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(elementId);
		element_BuildingBlocks.ResetData(templateId, 1, -1);
		PlaceBuilding(context, areaId, blockId, num2, element_BuildingBlocks, buildingArea.Width);
		void CalcAvailableBlockIndices(List<short> blocks, Func<short, bool> func)
		{
			for (short num3 = 0; num3 < buildingArea.Width * buildingArea.Width; num3++)
			{
				int num4 = num3 % areaWidth;
				int num5 = num3 / areaWidth;
				if (num4 + buildingWidth <= areaWidth && num5 + buildingWidth <= areaWidth)
				{
					bool flag = true;
					for (int i = 0; i < buildingWidth; i++)
					{
						for (int j = 0; j < buildingWidth; j++)
						{
							short buildingBlockIndex = (short)(num3 + i * areaWidth + j);
							BuildingBlockKey key = new BuildingBlockKey(areaId, blockId, buildingBlockIndex);
							BuildingBlockData buildingBlockData = _buildingBlocks[key];
							if (buildingBlockData.RootBlockIndex >= 0)
							{
								flag = false;
								break;
							}
							if (buildingBlockData.TemplateId >= 0 && func(buildingBlockData.TemplateId))
							{
								flag = false;
								break;
							}
							if (IsEdge(num3))
							{
								flag = false;
								break;
							}
						}
						if (!flag)
						{
							break;
						}
					}
					if (flag)
					{
						blocks.Add(num3);
					}
				}
			}
		}
		static int GetDisToCenter(short blockIndex, sbyte width)
		{
			int num3 = blockIndex % width;
			int num4 = blockIndex / width;
			int num5 = width / 2 - 1;
			return Math.Abs(num3 - num5) + Math.Abs(num4 - num5);
		}
		bool IsEdge(short blockIndex)
		{
			int num3 = blockIndex % areaWidth;
			int num4 = blockIndex / areaWidth;
			return num3 == 0 || num3 == areaWidth - buildingWidth || num4 == 0 || num4 == areaWidth - buildingWidth;
		}
	}

	public void XiangshuDestroyTaiwuVillageBuilding(DataContext context, Location location)
	{
		if (!DomainManager.Taiwu.GetTaiwuVillageLocation().Equals(location))
		{
			return;
		}
		List<BuildingBlockData> list = new List<BuildingBlockData>();
		DomainManager.Building.GetDestroyableBuildings(location, list);
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		int num = context.Random.Next(6, 19);
		for (int i = 0; i < num; i++)
		{
			if (list.Count == 0)
			{
				DomainManager.World.SetTaiwuVillageDestroyed();
				break;
			}
			int index = context.Random.Next(list.Count);
			BuildingBlockData buildingBlockData = list[index];
			BuildingBlockKey key = new BuildingBlockKey(location.AreaId, location.BlockId, buildingBlockData.BlockIndex);
			sbyte maxDurability = BuildingBlock.Instance[buildingBlockData.TemplateId].MaxDurability;
			int num2 = context.Random.Next(maxDurability / 2, maxDurability + maxDurability / 2 + 1);
			while (num2 > 0)
			{
				if (num2 >= buildingBlockData.Durability)
				{
					num2 -= buildingBlockData.Durability;
					continue;
				}
				buildingBlockData.Durability = (sbyte)(buildingBlockData.Durability - num2);
				num2 = 0;
			}
			if (buildingBlockData.TemplateId == 46)
			{
				RemoveExceedingResidents(context, key);
			}
			SetElement_BuildingBlocks(new BuildingBlockKey(location.AreaId, location.BlockId, buildingBlockData.BlockIndex), buildingBlockData, context);
		}
	}

	public void GetDestroyableBuildings(Location location, List<BuildingBlockData> destroyableBuildingList)
	{
		destroyableBuildingList.Clear();
		BuildingAreaData buildingAreaData = _buildingAreas[location];
		sbyte width = buildingAreaData.Width;
		for (short num = 0; num < buildingAreaData.Width * buildingAreaData.Width; num++)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(location.AreaId, location.BlockId, num);
			BuildingBlockData buildingBlockData = _buildingBlocks[buildingBlockKey];
			if (buildingBlockData.RootBlockIndex < 0)
			{
				BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[buildingBlockData.TemplateId];
				if (BuildingBlockData.IsBuilding(buildingBlockItem.Type) && buildingBlockItem.MaxDurability > 0)
				{
					if (BuildingBlockLevel(buildingBlockKey) <= 1 && buildingBlockData.Durability <= 0)
					{
						if (buildingBlockItem.Type == EBuildingBlockType.MainBuilding)
						{
							destroyableBuildingList.Clear();
							break;
						}
					}
					else
					{
						destroyableBuildingList.Add(buildingBlockData);
					}
				}
			}
		}
	}

	public static bool HasBuilt(Location location, BuildingAreaData buildingArea, short buildingTemplateId, bool checkUsable = true)
	{
		for (short num = 0; num < buildingArea.Width * buildingArea.Width; num++)
		{
			BuildingBlockKey elementId = new BuildingBlockKey(location.AreaId, location.BlockId, num);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(elementId);
			if (element_BuildingBlocks.TemplateId == buildingTemplateId && (!checkUsable || element_BuildingBlocks.CanUse()))
			{
				return true;
			}
		}
		return false;
	}

	public bool CanBuild(BuildingBlockKey blockKey, short buildingTemplateId = -1)
	{
		Location location = new Location(blockKey.AreaId, blockKey.BlockId);
		if (!DomainManager.TutorialChapter.InGuiding && !GetTaiwuBuildingAreas().Contains(location))
		{
			return false;
		}
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(blockKey);
		if (element_BuildingBlocks.TemplateId != 0)
		{
			return false;
		}
		BuildingAreaData element_BuildingAreas = GetElement_BuildingAreas(location);
		sbyte width = BuildingBlock.Instance[buildingTemplateId].Width;
		IsBuildingBlocksEmpty(blockKey.AreaId, blockKey.BlockId, blockKey.BuildingBlockIndex, element_BuildingAreas.Width, width);
		List<short> item = ObjectPool<List<short>>.Instance.Get();
		bool flag = false;
		flag = true;
		if (buildingTemplateId >= 0)
		{
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[buildingTemplateId];
			flag = buildingBlockItem.Type != EBuildingBlockType.MainBuilding;
			if (DomainManager.TutorialChapter.InGuiding)
			{
				flag = true;
			}
			flag = flag && (!buildingBlockItem.IsUnique || !HasBuilt(location, element_BuildingAreas, buildingTemplateId)) && AllDependBuildingAvailable(blockKey, buildingTemplateId, out var _);
			if (flag && buildingBlockItem.Class != EBuildingBlockClass.BornResource)
			{
				ushort[] baseBuildCost = buildingBlockItem.BaseBuildCost;
				ResourceInts allTaiwuResources = GetAllTaiwuResources();
				for (sbyte b = 0; b < 8; b++)
				{
					if (allTaiwuResources.Get(b) < baseBuildCost[b])
					{
						flag = false;
						break;
					}
				}
			}
		}
		ObjectPool<List<short>>.Instance.Return(item);
		return flag;
	}

	[Obsolete]
	public bool CanUpgrade(BuildingBlockKey blockKey, out bool dependencyIsNotMeet)
	{
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(blockKey);
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
		dependencyIsNotMeet = !UpgradeIsMeetDependency(blockKey);
		if ((((buildingBlockItem.Type != EBuildingBlockType.Building && buildingBlockItem.Type != EBuildingBlockType.MainBuilding && buildingBlockItem.Type != EBuildingBlockType.NormalResource) || BuildingBlockLevel(blockKey) >= buildingBlockItem.MaxLevel) | dependencyIsNotMeet) || element_BuildingBlocks.OperationType != -1)
		{
			return false;
		}
		return true;
	}

	[Obsolete]
	[DomainMethod]
	public bool CanAutoExpand(DataContext context, BuildingBlockKey blockKey)
	{
		if (!TryGetElement_BuildingBlocks(blockKey, out var _))
		{
			return false;
		}
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		List<short> autoExpandBlockIndexList = DomainManager.Extra.GetAutoExpandBlockIndexList();
		int[] array = new int[8];
		int[] array2 = new int[8];
		DomainManager.Taiwu.CalcResourceChangeBeforeExpand(array2);
		for (int i = 0; i < autoExpandBlockIndexList.Count; i++)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, autoExpandBlockIndexList[i]);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
			bool flag = buildingBlockKey.Equals(blockKey);
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
			if (buildingBlockItem.MaxLevel != BuildingBlockLevel(buildingBlockKey))
			{
				if (element_BuildingBlocks.OperationType == -1 && (!DomainManager.Building.HaveEnoughResourceToExpandBuilding(element_BuildingBlocks, array2, isChangeSourceData: true) || !DomainManager.Building.CanUpgrade(buildingBlockKey, out var _)))
				{
					return false;
				}
				if (flag)
				{
					break;
				}
			}
		}
		return true;
	}

	public IEnumerable<BuildingBlockData> GetBuildingBlocksAtLocation(Location location, Predicate<BuildingBlockData> condition = null)
	{
		BuildingAreaData areaData = GetElement_BuildingAreas(location);
		for (short blockIndex = 0; blockIndex < areaData.Width * areaData.Width; blockIndex++)
		{
			BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, blockIndex);
			BuildingBlockData buildingBlock = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
			if (condition?.Invoke(buildingBlock) ?? true)
			{
				yield return buildingBlock;
			}
		}
	}

	public static BuildingBlockData FindBuilding(Location location, BuildingAreaData buildingArea, short buildingTemplateId, bool checkUsable = true)
	{
		for (short num = 0; num < buildingArea.Width * buildingArea.Width; num++)
		{
			BuildingBlockKey elementId = new BuildingBlockKey(location.AreaId, location.BlockId, num);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(elementId);
			if (element_BuildingBlocks.TemplateId == buildingTemplateId && (!checkUsable || element_BuildingBlocks.CanUse()))
			{
				return element_BuildingBlocks;
			}
		}
		return null;
	}

	public static BuildingBlockKey FindBuildingKey(Location location, BuildingAreaData buildingArea, short buildingTemplateId, bool checkUsable = true)
	{
		for (short num = 0; num < buildingArea.Width * buildingArea.Width; num++)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(location.AreaId, location.BlockId, num);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
			if (element_BuildingBlocks.TemplateId == buildingTemplateId && (!checkUsable || element_BuildingBlocks.CanUse()))
			{
				return buildingBlockKey;
			}
		}
		return BuildingBlockKey.Invalid;
	}

	public List<BuildingBlockKey> FindAllBuildingsWithSameTemplate(Location location, BuildingAreaData buildingArea, short buildingTemplateId)
	{
		List<BuildingBlockKey> list = new List<BuildingBlockKey>();
		for (short num = 0; num < buildingArea.Width * buildingArea.Width; num++)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(location.AreaId, location.BlockId, num);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
			if (element_BuildingBlocks.TemplateId == buildingTemplateId && element_BuildingBlocks.CanUse())
			{
				list.Add(buildingBlockKey);
			}
		}
		return list;
	}

	public bool AllDependBuildingAvailable(BuildingBlockKey blockKey, short buildingTemplateId, out sbyte minLevel)
	{
		List<short> dependBuildings = BuildingBlock.Instance[buildingTemplateId].DependBuildings;
		bool result = true;
		minLevel = sbyte.MaxValue;
		if (dependBuildings.Count > 0)
		{
			Location elementId = new Location(blockKey.AreaId, blockKey.BlockId);
			BuildingAreaData element_BuildingAreas = GetElement_BuildingAreas(elementId);
			List<short> list = ObjectPool<List<short>>.Instance.Get();
			Span<bool> span = stackalloc bool[dependBuildings.Count];
			Span<sbyte> span2 = stackalloc sbyte[dependBuildings.Count];
			sbyte width = BuildingBlock.Instance[_buildingBlocks[blockKey].TemplateId].Width;
			element_BuildingAreas.GetNeighborBlocks(blockKey.BuildingBlockIndex, width, list, null, 2);
			for (int i = 0; i < list.Count; i++)
			{
				BuildingBlockKey buildingBlockKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, list[i]);
				BuildingBlockData buildingBlockData = _buildingBlocks[buildingBlockKey];
				if (buildingBlockData.RootBlockIndex >= 0)
				{
					buildingBlockKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, buildingBlockData.RootBlockIndex);
					buildingBlockData = _buildingBlocks[buildingBlockKey];
				}
				if (buildingBlockData.TemplateId != 0 && buildingBlockData.CanUse())
				{
					int num = dependBuildings.IndexOf(buildingBlockData.TemplateId);
					if (num >= 0)
					{
						span[num] = true;
						span2[num] = Math.Max(span2[num], BuildingBlockLevel(buildingBlockKey));
					}
				}
			}
			result = !span.Contains(value: false);
			minLevel = span2.Min();
			ObjectPool<List<short>>.Instance.Return(list);
		}
		return result;
	}

	[DomainMethod]
	public bool AllDependBuildingAvailable(BuildingBlockKey blockKey)
	{
		sbyte minLevel;
		if (TryGetElement_BuildingBlocks(blockKey, out var value) && value != null && value.ConfigData != null)
		{
			return AllDependBuildingAvailable(blockKey, value.TemplateId, out minLevel);
		}
		return false;
	}

	public void SetBuildingOperator(DataContext context, BuildingBlockKey blockKey, int index, int charId)
	{
		CharacterList value;
		if (!_buildingOperatorDict.ContainsKey(blockKey))
		{
			value = default(CharacterList);
			for (int i = 0; i < 3; i++)
			{
				value.Add(-1);
			}
			AddElement_BuildingOperatorDict(blockKey, value, context);
		}
		else
		{
			value = _buildingOperatorDict[blockKey];
		}
		value.GetCollection()[index] = charId;
		SetElement_BuildingOperatorDict(blockKey, value, context);
	}

	public void SetShopBuildingManager(DataContext context, BuildingBlockKey blockKey, int index, int charId, bool setArtisanOrder = true)
	{
		CharacterList value;
		if (!_shopManagerDict.ContainsKey(blockKey))
		{
			value = default(CharacterList);
			for (int i = 0; i < 7; i++)
			{
				value.Add(-1);
			}
			AddElement_ShopManagerDict(blockKey, value, context);
		}
		else
		{
			value = _shopManagerDict[blockKey];
		}
		value.GetCollection()[index] = charId;
		if (setArtisanOrder && TryGetElement_BuildingBlocks(blockKey, out var value2) && BuildingBlock.Instance[value2.TemplateId].ArtisanOrderAvailable)
		{
			DomainManager.Extra.SetBuildingOrderArtisan(context, blockKey, value.GetCollection()[0]);
		}
		SetElement_ShopManagerDict(blockKey, value, context);
	}

	public bool IsShopManager(int charId)
	{
		VillagerWorkData value;
		return DomainManager.Taiwu.TryGetElement_VillagerWork(charId, out value) && value.WorkType == 1;
	}

	public sbyte GetCollectBuildingResourceType(BuildingBlockKey blockKey)
	{
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(blockKey);
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
		if (!buildingBlockItem.IsCollectResourceBuilding)
		{
			throw new Exception($"Building {element_BuildingBlocks.TemplateId} is not a collect resource building");
		}
		if (_CollectBuildingResourceType.ContainsKey(blockKey))
		{
			return _CollectBuildingResourceType[blockKey];
		}
		BuildingBlockItem buildingBlockItem2 = BuildingBlock.Instance[buildingBlockItem.DependBuildings[0]];
		for (int i = 0; i < buildingBlockItem2.CollectResourcePercent.Length; i++)
		{
			if (buildingBlockItem2.CollectResourcePercent[i] > 0)
			{
				return (sbyte)i;
			}
		}
		throw new Exception($"Building {element_BuildingBlocks.TemplateId} has no collectable resource type");
	}

	public sbyte GetLifeSkillByResourceType(sbyte resourceType)
	{
		return (sbyte)((resourceType < 6) ? Config.ResourceType.Instance[resourceType].LifeSkillType : 9);
	}

	[DomainMethod]
	public BuildingFormulaContextBridge GetBuildingFormulaContextBridge(BuildingBlockKey blockKey)
	{
		BuildingBlockData buildingBlockData = GetBuildingBlockData(blockKey);
		_formulaContextBridge.Initialize(blockKey, buildingBlockData.ConfigData, _formulaArgHandler, cacheAllArgs: true);
		return _formulaContextBridge;
	}

	[Obsolete]
	[DomainMethod]
	public int GetAttainmentOfBuilding(BuildingBlockKey blockKey, bool isAverage = false)
	{
		bool hasManager;
		int num = (isAverage ? BuildingTotalAverageAttainment(blockKey, -1, out hasManager) : BuildingTotalAttainment(blockKey, -1, out hasManager));
		return hasManager ? num : 0;
	}

	[Obsolete]
	public int GetAttainmentOfBuildingWhetherCanWork(BuildingBlockKey blockKey, bool isAverage = false)
	{
		int num = 0;
		sbyte collectBuildingResourceType = GetCollectBuildingResourceType(blockKey);
		DomainManager.Building.TryGetElement_ShopManagerDict(blockKey, out var value);
		for (int i = 0; i < value.GetCount(); i++)
		{
			int num2 = value.GetCollection()[i];
			if (num2 >= 0)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num2);
				num += BaseWorkContribution;
				num += element_Objects.GetLifeSkillAttainment(GetLifeSkillByResourceType(collectBuildingResourceType));
			}
		}
		return isAverage ? (num / value.GetCount()) : num;
	}

	[Obsolete]
	public int GetAttainmentOfBuilding(BuildingBlockKey blockKey, sbyte resourceType, bool isAverage = false)
	{
		int num = 0;
		DomainManager.Building.TryGetElement_ShopManagerDict(blockKey, out var value);
		for (int i = 0; i < value.GetCount(); i++)
		{
			int num2 = value.GetCollection()[i];
			if (num2 >= 0 && DomainManager.Taiwu.CanWork(num2))
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num2);
				num += BaseWorkContribution;
				num += element_Objects.GetLifeSkillAttainment(GetLifeSkillByResourceType(resourceType));
			}
		}
		return isAverage ? (num / value.GetCount()) : num;
	}

	[Obsolete]
	public int GetShopBuildingMaxAttainment(BuildingBlockKey blockKey, bool isAverage = false)
	{
		int num = 0;
		DomainManager.Building.TryGetElement_ShopManagerDict(blockKey, out var value);
		for (int i = 0; i < value.GetCount(); i++)
		{
			int num2 = value.GetCollection()[i];
			if (num2 >= 0 && DomainManager.Taiwu.CanWork(num2))
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num2);
				num += BaseWorkContribution;
				num += element_Objects.GetMaxCombatSkillAttainment();
			}
		}
		return isAverage ? (num / value.GetCount()) : num;
	}

	public int GetShopBuildingMaxAttainmentWhetherCanWork(BuildingBlockKey blockKey, bool isAverage = false)
	{
		int num = 0;
		DomainManager.Building.TryGetElement_ShopManagerDict(blockKey, out var value);
		for (int i = 0; i < value.GetCount(); i++)
		{
			int num2 = value.GetCollection()[i];
			if (num2 >= 0)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num2);
				num += BaseWorkContribution;
				num += element_Objects.GetMaxCombatSkillAttainment();
			}
		}
		return isAverage ? (num / value.GetCount()) : num;
	}

	[Obsolete]
	public int GetShopBuildingAttainment(BuildingBlockData blockData, BuildingBlockKey blockKey, bool isAverage = false)
	{
		int num = 0;
		DomainManager.Building.TryGetElement_ShopManagerDict(blockKey, out var value);
		for (int i = 0; i < value.GetCount(); i++)
		{
			int num2 = value.GetCollection()[i];
			if (num2 >= 0 && DomainManager.Taiwu.CanWork(num2))
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num2);
				num += BaseWorkContribution;
				num += element_Objects.GetLifeSkillAttainment(BuildingBlock.Instance[blockData.TemplateId].RequireLifeSkillType);
			}
		}
		return isAverage ? (num / value.GetCount()) : num;
	}

	public int GetSpecialBuildingAttainment(BuildingBlockData blockData, BuildingBlockKey blockKey, bool isAverage = false)
	{
		int num = 0;
		DomainManager.Building.TryGetElement_ShopManagerDict(blockKey, out var value);
		for (int i = 0; i < value.GetCount(); i++)
		{
			int num2 = value.GetCollection()[i];
			if (num2 >= 0 && DomainManager.Taiwu.CanWork(num2))
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num2);
				num += BaseWorkContribution;
				num += element_Objects.GetLifeSkillAttainment(BuildingBlock.Instance[blockData.TemplateId].RequireLifeSkillType);
			}
		}
		return isAverage ? (num / value.GetCount()) : num;
	}

	public int GetShopBuildingAttainmentWhetherCanWork(BuildingBlockData blockData, BuildingBlockKey blockKey, bool isAverage = false)
	{
		int num = 0;
		DomainManager.Building.TryGetElement_ShopManagerDict(blockKey, out var value);
		for (int i = 0; i < value.GetCount(); i++)
		{
			int num2 = value.GetCollection()[i];
			if (num2 >= 0)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num2);
				num += BaseWorkContribution;
				num += element_Objects.GetLifeSkillAttainment(BuildingBlock.Instance[blockData.TemplateId].RequireLifeSkillType);
			}
		}
		return isAverage ? (num / value.GetCount()) : num;
	}

	[DomainMethod]
	public int GetBuildingAttainment(BuildingBlockData blockData, BuildingBlockKey blockKey, bool isAverage = false)
	{
		bool hasManager;
		int num = (isAverage ? BuildingTotalAverageAttainment(blockKey, -1, out hasManager) : (UseMaxAttainment(blockData) ? BuildingMaxAttainment(blockKey, -1, out hasManager) : BuildingTotalAttainment(blockKey, -1, out hasManager)));
		return hasManager ? num : 0;
	}

	private bool UseMaxAttainment(BuildingBlockData blockData)
	{
		BuildingBlockItem configData = blockData.ConfigData;
		if (configData != null)
		{
			List<short> expandInfos = configData.ExpandInfos;
			if (expandInfos != null && expandInfos.Count > 0)
			{
				foreach (short expandInfo in configData.ExpandInfos)
				{
					int formula = BuildingScale.Instance[expandInfo].Formula;
					if (formula < 0)
					{
						continue;
					}
					BuildingFormulaItem buildingFormulaItem = BuildingFormula.Instance[formula];
					EBuildingFormulaArgType[] arguments = buildingFormulaItem.Arguments;
					if (arguments == null || arguments.Length <= 0)
					{
						continue;
					}
					EBuildingFormulaArgType[] arguments2 = buildingFormulaItem.Arguments;
					for (int i = 0; i < arguments2.Length; i++)
					{
						if (arguments2[i] == EBuildingFormulaArgType.MaxAttainment)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	[Obsolete]
	public int GetBuildingAttainmentWhetherCanWork(BuildingBlockData blockData, BuildingBlockKey blockKey, bool isAverage = false)
	{
		BuildingBlockItem config = BuildingBlock.Instance[blockData.TemplateId];
		if (IsDependKungfuPracticeRoom(config))
		{
			return GetShopBuildingMaxAttainmentWhetherCanWork(blockKey, isAverage);
		}
		return GetShopBuildingAttainmentWhetherCanWork(blockData, blockKey, isAverage);
	}

	[DomainMethod]
	public int[] GetBuildingShopManagerAutoArrangeSorted(BuildingBlockKey blockKey, int[] managerCharacterIds)
	{
		if (managerCharacterIds == null)
		{
			return Array.Empty<int>();
		}
		Dictionary<int, (int, int, int)> values = new Dictionary<int, (int, int, int)>();
		if (TryGetElement_BuildingBlocks(blockKey, out var value))
		{
			BuildingBlockItem item = BuildingBlock.Instance.GetItem(value.TemplateId);
			if (item != null)
			{
				sbyte needLifeSkillType = GetNeedLifeSkillType(item, blockKey);
				foreach (int num in managerCharacterIds)
				{
					if (DomainManager.Character.TryGetElement_Objects(num, out var element))
					{
						short lifeSkillAttainment = element.GetLifeSkillAttainment(needLifeSkillType);
						int item2 = BuildingManageHarvestSuccessRate(blockKey, num) * 2;
						int item3 = BuildingManageHarvestSpecialSuccessRate(blockKey, num) / 2;
						(int, int, int) value2 = (lifeSkillAttainment, item2, item3);
						values[num] = value2;
					}
				}
			}
		}
		int[] array = managerCharacterIds.ToArray();
		Array.Sort(array, delegate(int charIdA, int charIdB)
		{
			values.TryGetValue(charIdA, out var value3);
			values.TryGetValue(charIdB, out var value4);
			int num2 = (value4.Item1 + value4.Item2 + value4.Item3).CompareTo(value3.Item1 + value3.Item2 + value3.Item3);
			if (num2 == 0)
			{
				num2 = charIdA.CompareTo(charIdB);
			}
			return num2;
		});
		return array;
	}

	public bool IsDependKungfuPracticeRoom(BuildingBlockItem config)
	{
		return config.DependBuildings.Count > 0 && config.DependBuildings[0] == 52 && config.IsShop;
	}

	public int GetBuildingAttainmentUniversalWhetherCanWork(BuildingBlockData blockData, BuildingBlockKey blockKey, bool isAverage = false)
	{
		bool hasManager;
		return BuildingTotalAttainment(blockKey, -1, out hasManager, ignoreCanWork: true);
	}

	[Obsolete]
	public short GetLevelByDistance(short level, int distance)
	{
		if (distance <= 0)
		{
			return level;
		}
		return (short)((double)level * (1.0 / (double)distance));
	}

	[Obsolete]
	public double GetNumByDistance(double num, int distance)
	{
		if (distance <= 0)
		{
			return num;
		}
		return num * (1.0 / (double)distance);
	}

	public int CalcResourceChangeFactor(BuildingBlockData blockData)
	{
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[BuildingBlock.Instance[blockData.TemplateId].DependBuildings[0]];
		int result = 1;
		for (int i = 0; i < buildingBlockItem.CollectResourcePercent.Length; i++)
		{
			if (buildingBlockItem.CollectResourcePercent[i] != 0)
			{
				result = buildingBlockItem.CollectResourcePercent[i];
				break;
			}
		}
		return result;
	}

	private void BuildingRemoveVillagerWork(DataContext context, BuildingBlockKey blockKey)
	{
		RemoveAllOperatorsInBuilding(context, blockKey);
		RemoveAllManagersInBuilding(context, blockKey);
	}

	private void BuildingRemoveResident(DataContext context, BuildingBlockData blockData, BuildingBlockKey blockKey)
	{
		if (blockData.TemplateId == 46)
		{
			RemoveResidence(context, blockKey);
		}
		else if (blockData.TemplateId == 47)
		{
			RemoveComfortableHouse(context, blockKey);
		}
	}

	public bool IsHaveSpecifyBuilding(short templateId, GameData.Domains.Character.Character character)
	{
		foreach (KeyValuePair<BuildingBlockKey, BuildingBlockData> buildingBlock in _buildingBlocks)
		{
			if (buildingBlock.Value.TemplateId == templateId && buildingBlock.Value.CanUse() && character.GetLocation().AreaId == buildingBlock.Key.AreaId && character.GetLocation().BlockId == buildingBlock.Key.BlockId)
			{
				return true;
			}
		}
		return false;
	}

	public (BuildingBlockKey, BuildingBlockData) GetSpecifyBuildingBlockData(short templateId)
	{
		foreach (KeyValuePair<BuildingBlockKey, BuildingBlockData> buildingBlock in _buildingBlocks)
		{
			if (buildingBlock.Value.TemplateId == templateId && buildingBlock.Value.CanUse())
			{
				return (buildingBlock.Key, buildingBlock.Value);
			}
		}
		return (BuildingBlockKey.Invalid, null);
	}

	public unsafe int CreateCharacterByRecruitCharacterData(DataContext context, RecruitCharacterData recruitCharacterData)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		recruitCharacterData.Recalculate();
		OrganizationInfo organizationInfo = taiwu.GetOrganizationInfo();
		organizationInfo.Grade = 0;
		IntelligentCharacterCreationInfo intelligentCharacterCreationInfo = new IntelligentCharacterCreationInfo(taiwuVillageLocation, organizationInfo, recruitCharacterData.TemplateId);
		intelligentCharacterCreationInfo.Age = recruitCharacterData.Age;
		intelligentCharacterCreationInfo.BirthMonth = recruitCharacterData.BirthMonth;
		intelligentCharacterCreationInfo.Gender = recruitCharacterData.Gender;
		intelligentCharacterCreationInfo.Transgender = recruitCharacterData.Transgender;
		intelligentCharacterCreationInfo.BaseAttraction = recruitCharacterData.BaseAttraction;
		intelligentCharacterCreationInfo.Avatar = recruitCharacterData.AvatarData;
		intelligentCharacterCreationInfo.CombatSkillQualificationGrowthType = recruitCharacterData.CombatSkillQualificationGrowthType;
		intelligentCharacterCreationInfo.LifeSkillQualificationGrowthType = recruitCharacterData.LifeSkillQualificationGrowthType;
		intelligentCharacterCreationInfo.InitializeSectSkills = false;
		intelligentCharacterCreationInfo.AllowRandomGrowingGradeAdjust = false;
		intelligentCharacterCreationInfo.GrowingSectGrade = recruitCharacterData.PeopleLevel;
		intelligentCharacterCreationInfo.DisableBeReincarnatedBySavedSoul = true;
		IntelligentCharacterCreationInfo info = intelligentCharacterCreationInfo;
		GameData.Domains.Character.Character character = DomainManager.Character.CreateIntelligentCharacter(context, ref info);
		int id = character.GetId();
		DomainManager.Character.CompleteCreatingCharacter(id);
		character.SetBaseMorality(recruitCharacterData.GetBaseMorality(), context);
		character.SetFullName(recruitCharacterData.FullName, context);
		character.SetFeatureIds(recruitCharacterData.FeatureIds.ToList(), context);
		short clothingTemplateId = recruitCharacterData.ClothingTemplateId;
		if (clothingTemplateId >= 0 && character.GetEquipment()[4].TemplateId != clothingTemplateId)
		{
			ItemKey itemKey = DomainManager.Item.CreateItem(context, 3, clothingTemplateId);
			character.AddInventoryItem(context, itemKey, 1);
			character.ChangeEquipment(context, -1, 4, itemKey);
			bool flag = false;
		}
		DomainManager.Extra.SetCharTeammateCommands(context, id, new SByteList(recruitCharacterData.TeammateCommands));
		MainAttributes mainAttributes = recruitCharacterData.MainAttributes;
		MainAttributes baseMainAttributes = character.GetBaseMainAttributes();
		MainAttributes maxMainAttributes = character.GetMaxMainAttributes();
		for (int i = 0; i < 6; i++)
		{
			int num = mainAttributes.Items[i] - maxMainAttributes.Items[i];
			ref short reference = ref baseMainAttributes.Items[i];
			reference += (short)num;
			baseMainAttributes.Items[i] = Math.Max(baseMainAttributes.Items[i], (short)0);
		}
		character.SetBaseMainAttributes(baseMainAttributes, context);
		character.SetCurrMainAttributes(character.GetMaxMainAttributes(), context);
		CombatSkillShorts combatSkillQualifications = recruitCharacterData.CombatSkillQualifications;
		CombatSkillShorts baseCombatSkillQualifications = character.GetBaseCombatSkillQualifications();
		CombatSkillShorts combatSkillQualifications2 = character.GetCombatSkillQualifications();
		for (int j = 0; j < 14; j++)
		{
			int num2 = combatSkillQualifications.Items[j] - combatSkillQualifications2.Items[j];
			ref short reference2 = ref baseCombatSkillQualifications.Items[j];
			reference2 += (short)num2;
			baseCombatSkillQualifications.Items[j] = Math.Max(baseCombatSkillQualifications.Items[j], (short)0);
		}
		character.SetBaseCombatSkillQualifications(ref baseCombatSkillQualifications, context);
		LifeSkillShorts lifeSkillQualifications = recruitCharacterData.LifeSkillQualifications;
		LifeSkillShorts baseLifeSkillQualifications = character.GetBaseLifeSkillQualifications();
		LifeSkillShorts lifeSkillQualifications2 = character.GetLifeSkillQualifications();
		for (int k = 0; k < 16; k++)
		{
			int num3 = lifeSkillQualifications.Items[k] - lifeSkillQualifications2.Items[k];
			ref short reference3 = ref baseLifeSkillQualifications.Items[k];
			reference3 += (short)num3;
			baseLifeSkillQualifications.Items[k] = Math.Max(baseLifeSkillQualifications.Items[k], (short)0);
		}
		character.SetBaseLifeSkillQualifications(ref baseLifeSkillQualifications, context);
		return id;
	}

	public void InitBuildingEffect()
	{
		InitAllAreaBuildingBlockEffectsCache();
	}

	private void UpdateTaiwuVillageBuildingEffect()
	{
		foreach (Location taiwuBuildingArea in _taiwuBuildingAreas)
		{
			UpdateLocationBuildingBlockEffectsCache(taiwuBuildingArea);
		}
		TriggerCacheUpdate();
		_needUpdateEffects = false;
	}

	private void TriggerCacheUpdate()
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		DomainManager.Map.GetSettlementBlocksAndAffiliatedBlocks(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, list);
		DataContext currentThreadDataContext = DataContextManager.GetCurrentThreadDataContext();
		for (int i = 0; i < list.Count; i++)
		{
			MapBlockData blockData = DomainManager.Map.GetBlockData(taiwuVillageLocation.AreaId, list[i]);
			if (blockData.CharacterSet != null)
			{
				foreach (int item in blockData.CharacterSet)
				{
					if (DomainManager.Character.TryGetElement_Objects(item, out var element))
					{
						element.SetLocation(element.GetLocation(), currentThreadDataContext);
					}
				}
			}
			if (blockData.InfectedCharacterSet == null)
			{
				continue;
			}
			foreach (int item2 in blockData.InfectedCharacterSet)
			{
				if (DomainManager.Character.TryGetElement_Objects(item2, out var element2))
				{
					element2.SetLocation(element2.GetLocation(), currentThreadDataContext);
				}
			}
		}
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item3 in collection)
		{
			if (DomainManager.Character.TryGetElement_Objects(item3, out var element3))
			{
				element3.SetLocation(element3.GetLocation(), currentThreadDataContext);
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
	}

	public override void FixAbnormalDomainArchiveData(DataContext context)
	{
		FixBuildingEarningsData(context);
		FixVillageResidentData(context);
		FixVillageStatusData(context);
		FixBuildingBlockData(context);
		FixBuildingCollectResourceTypeData(context);
		FixStoneRoomData(context);
		FixTeaHouseCaravan(context);
		FixBuildingLegacyData(context);
		FixChickenKing(context);
		FixSoldBuildingItemValueZero();
		FixObsoleteBuilding(context);
		FixBuildingOperateData(context);
		FixObsoleteChickenTask(context);
		BuildingVersionUpdateFix(context);
		FixBuildingBlockDataEx(context);
		FixFeastSetAutoRefill(context);
	}

	private void FixFeastSetAutoRefill(DataContext context)
	{
		if (!DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 79, 19))
		{
			return;
		}
		BuildingDomain building = DomainManager.Building;
		foreach (Location taiwuBuildingArea in building.GetTaiwuBuildingAreas())
		{
			BuildingAreaData buildingAreaData = building.GetBuildingAreaData(taiwuBuildingArea);
			short num = 0;
			short num2 = (short)(buildingAreaData.Width * buildingAreaData.Width);
			while (num < num2)
			{
				BuildingBlockKey buildingBlockKey = new BuildingBlockKey(taiwuBuildingArea.AreaId, taiwuBuildingArea.BlockId, num);
				if (building.TryGetElement_BuildingBlocks(buildingBlockKey, out var value) && value.TemplateId == 47)
				{
					DomainManager.Extra.FeastSetAutoRefill(context, buildingBlockKey, value: true);
					Logger.Warn($"FixFeastSetAutoRefill true ,blockKey.BuildingBlockIndex is : {buildingBlockKey.BuildingBlockIndex}");
				}
				num++;
			}
		}
	}

	private void FixBuildingBlockDataEx(DataContext context)
	{
		if (!DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 78, 22))
		{
			return;
		}
		BuildingDomain building = DomainManager.Building;
		foreach (Location taiwuBuildingArea in building.GetTaiwuBuildingAreas())
		{
			BuildingAreaData buildingAreaData = building.GetBuildingAreaData(taiwuBuildingArea);
			short num = 0;
			short num2 = (short)(buildingAreaData.Width * buildingAreaData.Width);
			while (num < num2)
			{
				BuildingBlockKey buildingBlockKey = new BuildingBlockKey(taiwuBuildingArea.AreaId, taiwuBuildingArea.BlockId, num);
				if (building.TryGetElement_BuildingBlocks(buildingBlockKey, out var value) && value.TemplateId <= 0)
				{
					DomainManager.Extra.ResetBuildingExtraData(context, buildingBlockKey);
					Logger.Warn($"FixBuildingBlockDataEx true ,blockKey.BuildingBlockIndex is : {buildingBlockKey.BuildingBlockIndex}");
				}
				num++;
			}
		}
	}

	private void BuildingVersionUpdateFix(DataContext context)
	{
		if (!DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 78))
		{
			return;
		}
		DomainManager.Extra.WipeAllVillagerRoleData(context);
		BuildingDomain building = DomainManager.Building;
		foreach (Location taiwuBuildingArea in building.GetTaiwuBuildingAreas())
		{
			BuildingAreaData buildingAreaData = building.GetBuildingAreaData(taiwuBuildingArea);
			short num = 0;
			short num2 = (short)(buildingAreaData.Width * buildingAreaData.Width);
			while (num < num2)
			{
				BuildingBlockKey buildingBlockKey = new BuildingBlockKey(taiwuBuildingArea.AreaId, taiwuBuildingArea.BlockId, num);
				if (building.TryGetElement_BuildingBlocks(buildingBlockKey, out var value))
				{
					BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[value.TemplateId];
					if (buildingBlockItem != null && buildingBlockItem.IsShop)
					{
						DomainManager.Building.SetBuildingAutoWork(context, buildingBlockKey.BuildingBlockIndex, isAutoWork: true);
						Logger.Warn($"SetBuildingAutoWork true ,blockKey.BuildingBlockIndex is : {buildingBlockKey.BuildingBlockIndex}");
					}
					bool flag = false;
					if (buildingBlockItem != null)
					{
						if (value.Durability > buildingBlockItem.MaxDurability)
						{
							value.Durability = buildingBlockItem.MaxDurability;
							flag = true;
						}
						if (value.OperationType == 2 || value.OperationType == 1 || value.OperationType == 3)
						{
							value.OperationType = -1;
							value.OperationProgress = 0;
							flag = true;
						}
						if (value.OperationType == 0)
						{
							CompleteBuilding(context, buildingBlockKey, value);
							value.OperationType = -1;
							value.OperationProgress = 0;
							flag = true;
						}
					}
					if (flag)
					{
						RemoveAllOperatorsInBuilding(context, buildingBlockKey);
						DomainManager.Building.SetElement_BuildingBlocks(buildingBlockKey, value, context);
					}
				}
				num++;
			}
		}
		CharacterList[] array = _shopManagerDict.Values.ToArray();
		foreach (CharacterList characterList in array)
		{
			int[] array2 = characterList.GetCollection().ToArray();
			foreach (int charId in array2)
			{
				DomainManager.Taiwu.RemoveVillagerWork(context, charId);
			}
		}
		ClearShopManagerDict(context);
		TaiwuDomain taiwu = DomainManager.Taiwu;
		int[] array3 = taiwu.GetVillagerWorkDict().Keys.ToArray();
		foreach (int num3 in array3)
		{
			if (taiwu.TryGetElement_VillagerWork(num3, out var value2) && value2.BuildingBlockIndex >= 0)
			{
				taiwu.RemoveVillagerWork(context, num3);
			}
		}
	}

	public void FixResidentAndComfortableHouseCount(DataContext context)
	{
		BuildingDomain building = DomainManager.Building;
		foreach (Location taiwuBuildingArea in building.GetTaiwuBuildingAreas())
		{
			BuildingAreaData buildingAreaData = building.GetBuildingAreaData(taiwuBuildingArea);
			short num = 0;
			short num2 = (short)(buildingAreaData.Width * buildingAreaData.Width);
			while (num < num2)
			{
				BuildingBlockKey buildingBlockKey = new BuildingBlockKey(taiwuBuildingArea.AreaId, taiwuBuildingArea.BlockId, num);
				if (building.TryGetElement_BuildingBlocks(buildingBlockKey, out var value))
				{
					BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[value.TemplateId];
					if (buildingBlockItem != null)
					{
						BuildingBlockDataEx element_BuildingBlockDataEx = DomainManager.Extra.GetElement_BuildingBlockDataEx((ulong)buildingBlockKey);
						switch (buildingBlockItem.TemplateId)
						{
						case 47:
						{
							if (_comfortableHouses.TryGetValue(buildingBlockKey, out var value3))
							{
								int levelEffect2 = BuildingScale.DefValue.ComfortableHouseCapacity.GetLevelEffect(element_BuildingBlockDataEx.CalcUnlockedLevelCount());
								int count2 = value3.GetCount();
								for (int num5 = count2 - 1; num5 >= levelEffect2; num5--)
								{
									int num6 = value3[num5];
									RemoveFromComfortableHouse(context, num6, _buildingComfortableHouses[num6]);
								}
							}
							break;
						}
						case 46:
						{
							if (_residences.TryGetValue(buildingBlockKey, out var value2))
							{
								int levelEffect = BuildingScale.DefValue.ResidenceCapacity.GetLevelEffect(element_BuildingBlockDataEx.CalcUnlockedLevelCount());
								int count = value2.GetCount();
								for (int num3 = count - 1; num3 >= levelEffect; num3--)
								{
									int num4 = value2[num3];
									RemoveFromResidence(context, num4, _buildingResidents[num4]);
								}
							}
							break;
						}
						}
					}
				}
				num++;
			}
		}
	}

	private void FixObsoleteChickenTask(DataContext context)
	{
		if (AllChickenInTaiwuVillage(context))
		{
			DomainManager.Extra.FinishTriggeredExtraTask(context, 53, 342);
		}
	}

	private void FixObsoleteBuilding(DataContext context)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData buildingAreaData = GetBuildingAreaData(taiwuVillageLocation);
		List<short> list = new List<short>();
		Span<short> span = stackalloc short[2];
		SpanList<short> spanList = span;
		spanList.Add(281);
		spanList.Add(282);
		SpanList<short>.Enumerator enumerator = spanList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			short current = enumerator.Current;
			BuildingBlockData buildingBlockData = FindBuilding(taiwuVillageLocation, buildingAreaData, current, checkUsable: false);
			if (buildingBlockData == null)
			{
				continue;
			}
			BuildingBlockKey blockKey = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, buildingBlockData.BlockIndex);
			BuildingRemoveVillagerWork(context, blockKey);
			ushort[] baseBuildCost = BuildingBlock.Instance[current].BaseBuildCost;
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			for (sbyte b = 0; b < 8; b++)
			{
				if (baseBuildCost[b] > 0)
				{
					taiwu.ChangeResource(context, b, baseBuildCost[b]);
				}
			}
			list.Add(current);
			ResetAllChildrenBlocks(context, blockKey, 0, -1);
			UpdateTaiwuVillageBuildingEffect();
		}
		if (list.Count != 0)
		{
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.ProfessionObsoleteBuildingRemoved, list);
		}
	}

	private void FixBuildingOperateData(DataContext context)
	{
		Dictionary<int, VillagerWorkData> villagerWorkDict = DomainManager.Taiwu.GetVillagerWorkDict();
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		foreach (int key in villagerWorkDict.Keys)
		{
			if (DomainManager.Character.TryGetElement_Objects(key, out var element))
			{
				if (element.GetOrganizationInfo().OrgTemplateId != 16)
				{
					list.Add(key);
					Logger.Warn($"FixBuildingOperateData ,remove charId: {key},because is not belong taiwu");
				}
			}
			else
			{
				list.Add(key);
				Logger.Warn($"FixBuildingOperateData ,remove charId: {key},because dead");
			}
		}
		foreach (int item in list)
		{
			DomainManager.Taiwu.RemoveVillagerWork(context, item);
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}

	private void FixSoldBuildingItemValueZero()
	{
		foreach (KeyValuePair<BuildingBlockKey, BuildingEarningsData> collectBuildingEarningsDatum in _collectBuildingEarningsData)
		{
			for (int num = collectBuildingEarningsDatum.Value.ShopSoldItemEarnList.Count - 1; num >= 0; num--)
			{
				if (collectBuildingEarningsDatum.Value.ShopSoldItemEarnList[num].Second == 0)
				{
					collectBuildingEarningsDatum.Value.ShopSoldItemEarnList[num] = new IntPair(-1, -1);
					Logger.Warn($"FixSoldBuildingItemValueZero ,index is {collectBuildingEarningsDatum.Key.BuildingBlockIndex}");
				}
			}
		}
	}

	private void FixBuildingLegacyData(DataContext context)
	{
		List<short> legaciesBuildingTemplateIdList = DomainManager.Extra.GetLegaciesBuildingTemplateIdList();
		for (int num = legaciesBuildingTemplateIdList.Count - 1; num >= 0; num--)
		{
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[legaciesBuildingTemplateIdList[num]];
			if (BuildingBlockData.IsUsefulResource(buildingBlockItem.Type))
			{
				legaciesBuildingTemplateIdList.RemoveAt(num);
				ItemKey itemKey = DomainManager.Item.CreateItem(context, 12, buildingBlockItem.BuildingCoreItem);
				DomainManager.Taiwu.WarehouseAdd(context, itemKey, 1);
				Logger.Warn($"Transfer legacy normal resource to buildingCore,buildingTemplateId is：{num}");
			}
		}
		DomainManager.Extra.SetLegaciesBuildingTemplateIdList(legaciesBuildingTemplateIdList, context);
	}

	private void FixTeaHouseCaravan(DataContext context)
	{
		List<ItemKey> list = new List<ItemKey>();
		for (int num = _teaHorseCaravanData.CarryGoodsList.Count - 1; num >= 0; num--)
		{
			ItemKey item = _teaHorseCaravanData.CarryGoodsList[num].Item1;
			if (!DomainManager.Item.ItemExists(item))
			{
				_teaHorseCaravanData.CarryGoodsList.RemoveAt(num);
				Logger.Warn($"Removing not exist tea horse item {item}.");
			}
			else if (!list.Contains(item))
			{
				list.Add(item);
			}
			else if (!ItemTemplateHelper.IsStackable(item.ItemType, item.TemplateId))
			{
				_teaHorseCaravanData.CarryGoodsList.RemoveAt(num);
				Logger.Warn($"Removing duplicate tea horse item {item}.");
			}
		}
		SetTeaHorseCaravanData(_teaHorseCaravanData, context);
		DomainManager.Building.UpgradeTeaHorseCaravanByAwareness(context);
	}

	private void FixStoneRoomData(DataContext context)
	{
		List<int> stoneRoomCharList = DomainManager.Extra.GetStoneRoomCharList();
		for (int num = stoneRoomCharList.Count - 1; num >= 0; num--)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(stoneRoomCharList[num]);
			if (!element_Objects.IsCompletelyInfected())
			{
				Logger.Warn($"Fixing not infected stone room character {element_Objects}.");
				stoneRoomCharList.Remove(stoneRoomCharList[num]);
				element_Objects.DeactivateExternalRelationState(context, 8);
			}
			else if (!element_Objects.IsActiveExternalRelationState(8))
			{
				Logger.Warn($"Fixing {element_Objects}'s not activated external relation state: CapturedInStoneRoom.");
				element_Objects.DeactivateExternalRelationState(context, 8);
			}
		}
		DomainManager.Extra.SetStoneRoomCharList(stoneRoomCharList, context);
	}

	private void FixBuildingCollectResourceTypeData(DataContext context)
	{
		List<BuildingBlockKey> list = new List<BuildingBlockKey>();
		foreach (KeyValuePair<BuildingBlockKey, sbyte> item in _CollectBuildingResourceType)
		{
			if (!TryGetElement_BuildingBlocks(item.Key, out var value))
			{
				continue;
			}
			if (value.TemplateId <= 0)
			{
				list.Add(item.Key);
				continue;
			}
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[value.TemplateId];
			if (buildingBlockItem.DependBuildings.Count == 0 || !buildingBlockItem.IsCollectResourceBuilding)
			{
				list.Add(item.Key);
				continue;
			}
			BuildingBlockItem buildingBlockItem2 = BuildingBlock.Instance[buildingBlockItem.DependBuildings[0]];
			if (buildingBlockItem2 == null || buildingBlockItem2.CollectResourcePercent == null)
			{
				list.Add(item.Key);
				continue;
			}
			int num = 0;
			for (int i = 0; i < buildingBlockItem2.CollectResourcePercent.Length; i++)
			{
				if (buildingBlockItem2.CollectResourcePercent[i] > 0)
				{
					num++;
				}
			}
			if (num == 1 && !list.Contains(item.Key))
			{
				list.Add(item.Key);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			Logger.Warn($"Fixing buildingCollectResourceTypeData,wrong blockIndex:{list[j].BuildingBlockIndex}");
			RemoveElement_CollectBuildingResourceType(list[j], context);
		}
	}

	private void FixBuildingBlockData(DataContext context)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(taiwuVillageLocation);
		bool flag = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 78);
		for (short num = 0; num < element_BuildingAreas.Width * element_BuildingAreas.Width; num++)
		{
			BuildingBlockKey elementId = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, num);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(elementId);
			if (element_BuildingBlocks.TemplateId > 0)
			{
				BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
				if (buildingBlockItem.Width == 2)
				{
					BuildingBlockKey elementId2 = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, (short)(num + 1));
					BuildingBlockData element_BuildingBlocks2 = DomainManager.Building.GetElement_BuildingBlocks(elementId2);
					if (element_BuildingBlocks2.RootBlockIndex != num)
					{
						element_BuildingBlocks2.ResetData(-1, -1, num);
						SetElement_BuildingBlocks(elementId2, element_BuildingBlocks2, context);
						Logger.Warn($"Fixing buildingBlockData,wrong blockIndex:{num + 1}");
					}
					elementId2 = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, (short)(num + element_BuildingAreas.Width));
					element_BuildingBlocks2 = DomainManager.Building.GetElement_BuildingBlocks(elementId2);
					if (element_BuildingBlocks2.RootBlockIndex != num)
					{
						element_BuildingBlocks2.ResetData(-1, -1, num);
						SetElement_BuildingBlocks(elementId2, element_BuildingBlocks2, context);
						Logger.Warn($"Fixing buildingBlockData,wrong blockIndex:{num + element_BuildingAreas.Width}");
					}
					elementId2 = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, (short)(num + element_BuildingAreas.Width + 1));
					element_BuildingBlocks2 = DomainManager.Building.GetElement_BuildingBlocks(elementId2);
					if (element_BuildingBlocks2.RootBlockIndex != num)
					{
						element_BuildingBlocks2.ResetData(-1, -1, num);
						SetElement_BuildingBlocks(elementId2, element_BuildingBlocks2, context);
						Logger.Warn($"Fixing buildingBlockData,wrong blockIndex:{num + element_BuildingAreas.Width + 1}");
					}
				}
			}
		}
	}

	private void FixVillageStatusData(DataContext context)
	{
		List<int> list = new List<int>();
		DomainManager.Organization.GetElement_CivilianSettlements(DomainManager.Taiwu.GetTaiwuVillageSettlementId()).GetMembers().GetAllMembers(list);
		list.Remove(DomainManager.Taiwu.GetTaiwuCharId());
		for (int i = 0; i < list.Count; i++)
		{
			if (!DomainManager.Taiwu.IsInGroup(list[i]) && !_buildingResidents.ContainsKey(list[i]) && !_homeless.Contains(list[i]))
			{
				_homeless.Add(list[i]);
				Logger.Warn($"Fixing homeless data,homeless add charId:{list[i]}");
			}
		}
		List<int> list2 = new List<int>();
		for (int num = _homeless.GetCount() - 1; num >= 0; num--)
		{
			if (list2.Contains(_homeless.GetCollection()[num]))
			{
				Logger.Warn($"Fixing homeless data,homeless repeat charId:{_homeless.GetCollection()[num]}");
				_homeless.Remove(_homeless.GetCollection()[num]);
			}
			else
			{
				list2.Add(_homeless.GetCollection()[num]);
			}
		}
		for (int num2 = _homeless.GetCount() - 1; num2 >= 0; num2--)
		{
			int num3 = _homeless.GetCollection()[num2];
			if (_buildingResidents.ContainsKey(num3) || DomainManager.Taiwu.IsInGroup(num3))
			{
				Logger.Warn($"Fixing homeless data,homeless with residents or group repeat charId:{num3}");
				_homeless.Remove(num3);
			}
		}
		for (int num4 = _homeless.GetCount() - 1; num4 >= 0; num4--)
		{
			if (!list.Contains(_homeless.GetCollection()[num4]))
			{
				Logger.Warn($"Fixing homeless data,homeless is not village,charId:{_homeless.GetCollection()[num4]}");
				_homeless.Remove(_homeless.GetCollection()[num4]);
			}
		}
		SetHomeless(_homeless, context);
		list2.Clear();
		foreach (KeyValuePair<BuildingBlockKey, CharacterList> residence in _residences)
		{
			for (int j = 0; j < residence.Value.GetCount(); j++)
			{
				int num5 = residence.Value.GetCollection()[j];
				if (!list.Contains(num5) || DomainManager.Taiwu.IsInGroup(num5))
				{
					list2.Add(num5);
				}
			}
		}
		for (int k = 0; k < list2.Count; k++)
		{
			Logger.Warn($"Fixing residents data,charId:{list2[k]} is not villager or in group");
			RemoveTaiwuResident(context, list2[k]);
		}
		list2.Clear();
		foreach (KeyValuePair<BuildingBlockKey, CharacterList> comfortableHouse in _comfortableHouses)
		{
			for (int l = 0; l < comfortableHouse.Value.GetCount(); l++)
			{
				int num6 = comfortableHouse.Value.GetCollection()[l];
				if (!IsCharacterAbleToJoinFeast(num6))
				{
					list2.Add(num6);
				}
			}
		}
		for (int m = 0; m < list2.Count; m++)
		{
			if (_buildingComfortableHouses.TryGetValue(list2[m], out var value))
			{
				Logger.Warn($"Fixing residents data,charId:{list2[m]} is not able to join feast");
				RemoveFromComfortableHouse(context, list2[m], value);
			}
		}
	}

	private void FixVillageResidentData(DataContext context)
	{
		List<BuildingBlockKey> list = new List<BuildingBlockKey>();
		foreach (KeyValuePair<BuildingBlockKey, CharacterList> residence in _residences)
		{
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(residence.Key);
			if (element_BuildingBlocks.TemplateId != 46)
			{
				list.Add(residence.Key);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			Logger.Warn($"Fixing village residences data,wrong buildingBlcokIndex:{list[i].BuildingBlockIndex}");
			RemoveResidence(context, list[i]);
		}
		list.Clear();
		foreach (KeyValuePair<BuildingBlockKey, CharacterList> comfortableHouse in _comfortableHouses)
		{
			BuildingBlockData element_BuildingBlocks2 = DomainManager.Building.GetElement_BuildingBlocks(comfortableHouse.Key);
			if (element_BuildingBlocks2.TemplateId != 47)
			{
				list.Add(comfortableHouse.Key);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			Logger.Warn($"Fixing village comfortableHouses data,wrong buildingBlockIndex:{list[j].BuildingBlockIndex}");
			RemoveComfortableHouse(context, list[j]);
		}
	}

	private void FixBuildingEarningsData(DataContext context)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(taiwuVillageLocation);
		for (short num = 0; num < element_BuildingAreas.Width * element_BuildingAreas.Width; num++)
		{
			BuildingBlockKey elementId = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, num);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(elementId);
			if (element_BuildingBlocks.TemplateId == 222)
			{
				BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
				if (buildingBlockItem.SuccesEvent.Count > 0)
				{
					short index = buildingBlockItem.SuccesEvent[0];
					ShopEventItem shopEventItem = Config.ShopEvent.Instance[index];
					if ((shopEventItem.ItemList.Count > 0 || shopEventItem.ItemGradeProbList.Count > 0) && TryGetElement_CollectBuildingEarningsData(elementId, out var value) && value.CollectionItemList != null)
					{
						bool flag = false;
						for (int i = 0; i < value.CollectionItemList.Count; i++)
						{
							ItemKey itemKey = value.CollectionItemList[i];
							if (itemKey.Id < 0)
							{
								Logger.Warn($"Fixing buildingEarnData item id,wrong item index:{i}, ItemType:{itemKey.ItemType},TemplateId:{itemKey.TemplateId}");
								ItemKey value2 = DomainManager.Item.CreateItem(context, itemKey.ItemType, itemKey.TemplateId);
								value.CollectionItemList[i] = value2;
								flag = true;
							}
						}
						if (flag)
						{
							SetElement_CollectBuildingEarningsData(elementId, value, context);
						}
					}
				}
			}
		}
	}

	public void TaiwuResourceGrow(DataContext context)
	{
	}

	[Obsolete]
	public bool CanAutoUpgrade(BuildingBlockKey blockKey)
	{
		return false;
	}

	public void TriggerBuildingCompleteEvents(DataContext context)
	{
		foreach (BuildingBlockKey newCompleteOperationBuilding in _newCompleteOperationBuildings)
		{
			BuildingBlockData buildingBlockData = GetBuildingBlockData(newCompleteOperationBuilding);
			DomainManager.TaiwuEvent.OnEvent_ConstructComplete(newCompleteOperationBuilding, buildingBlockData.TemplateId, BuildingBlockLevel(newCompleteOperationBuilding));
		}
		_newCompleteOperationBuildings.Clear();
		SetNewCompleteOperationBuildings(_newCompleteOperationBuildings, context);
	}

	public void UpdateResourceBlockEffectsOnAdvanceMonth(DataContext context)
	{
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		foreach (KeyValuePair<Location, BuildingAreaData> buildingArea in _buildingAreas)
		{
			buildingArea.Deconstruct(out var key, out var value);
			Location location = key;
			BuildingAreaData buildingAreaData = value;
			(ResourceInts resourcesChange, int expGain) tuple = CalcResourceBlockIncomeEffects(location);
			ResourceInts item = tuple.resourcesChange;
			int item2 = tuple.expGain;
			ResourceInts resourceChange = item;
			FinalizeResourceBlockIncomeEffectValues(ref resourceChange, isTaiwu: false);
			Settlement settlementByLocation = DomainManager.Organization.GetSettlementByLocation(location);
			OrgMemberCollection members = settlementByLocation.GetMembers();
			bool flag = item.IsNonZero();
			bool flag2 = item2 > 0;
			if (!flag && !flag2)
			{
				continue;
			}
			for (sbyte b = 0; b <= 8; b++)
			{
				HashSet<int> members2 = members.GetMembers(b);
				foreach (int item3 in members2)
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item3);
					if (!element_Objects.IsInteractableAsIntelligentCharacter())
					{
						continue;
					}
					if (flag)
					{
						if (item3 == taiwuCharId)
						{
							ResourceInts resourceChange2 = item;
							FinalizeResourceBlockIncomeEffectValues(ref resourceChange2, isTaiwu: true);
							element_Objects.ChangeResources(context, ref resourceChange2);
						}
						else
						{
							OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(element_Objects.GetOrganizationInfo());
							CValuePercent val = CValuePercent.op_Implicit(orgMemberConfig.ResourceIncomeRatio);
							ResourceInts delta = resourceChange;
							for (sbyte b2 = 0; b2 < 8; b2++)
							{
								ref int reference = ref delta[b2];
								reference *= val;
							}
							element_Objects.ChangeResources(context, ref delta);
						}
					}
					if (flag2)
					{
						element_Objects.ChangeExp(context, item2);
						if (taiwuCharId == item3)
						{
							InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
							instantNotificationCollection.AddBuildingExp(item2);
						}
					}
				}
			}
		}
	}

	public void SerialUpdate(DataContext context)
	{
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		ClearBuildingException();
		List<Location> taiwuBuildingAreas = GetTaiwuBuildingAreas();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		List<(BuildingBlockKey, BuildingBlockData)> list3 = new List<(BuildingBlockKey, BuildingBlockData)>();
		_newBrokenBuildings.Clear();
		_alreadyUpdateShopProgressBlock.Clear();
		for (int i = 0; i < taiwuBuildingAreas.Count; i++)
		{
			Location location = taiwuBuildingAreas[i];
			BuildingAreaData element_BuildingAreas = GetElement_BuildingAreas(location);
			MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(location.AreaId);
			int settlementIndex = DomainManager.Map.GetElement_Areas(location.AreaId).GetSettlementIndex(location.BlockId);
			short settlementId = element_Areas.SettlementInfos[settlementIndex].SettlementId;
			list2.Clear();
			list3.Clear();
			for (short num = 0; num < element_BuildingAreas.Width * element_BuildingAreas.Width; num++)
			{
				BuildingBlockKey buildingBlockKey = new BuildingBlockKey(location.AreaId, location.BlockId, num);
				BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(buildingBlockKey);
				if (element_BuildingBlocks.RootBlockIndex < 0)
				{
					BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
					if (buildingBlockItem.Type == EBuildingBlockType.Building || BuildingBlockData.IsResource(buildingBlockItem.Type))
					{
						if (buildingBlockItem.TemplateId == 45 && element_BuildingBlocks.CanUse())
						{
							int delta = CalculateGainAuthorityByShrinePerMonth(buildingBlockKey);
							taiwu.ChangeResource(context, 7, delta);
						}
						if (element_BuildingBlocks.Durability == 0 && BuildingBlockLevel(buildingBlockKey) == 1 && buildingBlockItem.DestoryType == 1)
						{
							int val = GetAllTaiwuResources().Get(7);
							ConsumeResource(context, 7, Math.Min(val, buildingBlockItem.MaxDurability));
						}
						if (buildingBlockItem.BaseMaintenanceCost.Count > 0 && element_BuildingBlocks.NeedMaintenanceCost())
						{
							list3.Add((buildingBlockKey, element_BuildingBlocks));
						}
						if (CanUpdateShopProgress(buildingBlockKey))
						{
							int buildingAttainment = GetBuildingAttainment(element_BuildingBlocks, buildingBlockKey);
							int shopManageProgressDelta = SharedMethods.GetShopManageProgressDelta(element_BuildingBlocks.TemplateId, buildingAttainment);
							int buildingBlockEffect = GetBuildingBlockEffect(settlementId, EBuildingScaleEffect.ShopProgressBonus);
							shopManageProgressDelta *= CValuePercentBonus.op_Implicit(buildingBlockEffect);
							element_BuildingBlocks.OfflineChangeShopProgress(shopManageProgressDelta);
							SetElement_BuildingBlocks(buildingBlockKey, element_BuildingBlocks, context);
						}
						if (BuildingBlockData.IsResource(buildingBlockItem.Type) && element_BuildingBlocks.OperationType == -1 && !list2.Contains(num))
						{
							sbyte width = buildingBlockItem.Width;
							List<int> neighborDistanceList = ObjectPool<List<int>>.Instance.Get();
							List<short> list4 = ObjectPool<List<short>>.Instance.Get();
							element_BuildingAreas.GetNeighborBlocks(num, width, list, neighborDistanceList, 2);
							element_BuildingAreas.GetNeighborBlocks(num, width, list4);
							UpdateResourceBlock(context, settlementId, buildingBlockKey, element_BuildingBlocks, list, list2, neighborDistanceList, list4);
						}
					}
				}
			}
			foreach (var (buildingBlockKey2, blockData) in list3)
			{
				if (!UpdateBlockMaintenance(context, buildingBlockKey2, blockData, taiwu))
				{
					_newBrokenBuildings.Add(buildingBlockKey2);
				}
			}
			BuildingBlockKey blockKey = FindBuildingKey(location, element_BuildingAreas, 50);
			if (!blockKey.IsInvalid)
			{
				AddSamsaraPlatformProgress(context, BuildingBlockLevel(blockKey));
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(list2);
		UpdateChickenInstances(context);
		UpdateCricketRegenProgress(context);
		ApplyCricketAuthorityGain(context);
		UpdateResidentsHappinessAndFavor(context);
		UpdateTeaHorseCaravan(context);
		UpdateStoneRoomData(context);
		UpdateKungfuPracticeRoom(context);
	}

	public int CalculateGainAuthorityByShrinePerMonth(BuildingBlockKey shrineBlockKey)
	{
		if (!HasShopManagerLeader(shrineBlockKey))
		{
			return 0;
		}
		sbyte leaderFameType = BuildLeaderFameType(shrineBlockKey);
		bool hasManager;
		int attainment = BuildingTotalAttainment(shrineBlockKey, -1, out hasManager);
		return SharedMethods.GetTaiwuShrineEffect(leaderFameType, attainment);
	}

	private void UpdateKungfuPracticeRoom(DataContext context)
	{
		if (!IsTaiwuVillageHaveSpecifyBuilding(52))
		{
			return;
		}
		List<sbyte> xiangshuIdInKungfuPracticeRoom = DomainManager.Extra.GetXiangshuIdInKungfuPracticeRoom();
		for (sbyte b = 0; b < 9; b++)
		{
			if (DomainManager.World.GetXiangshuAvatarFavorabilityType(b) >= 3 && !xiangshuIdInKungfuPracticeRoom.Contains(b))
			{
				xiangshuIdInKungfuPracticeRoom.Add(b);
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				switch (b)
				{
				case 0:
					monthlyEventCollection.AddVillageWoodenManByMonv();
					break;
				case 1:
					monthlyEventCollection.AddVillageWoodenManByDayueYaochang();
					break;
				case 2:
					monthlyEventCollection.AddVillageWoodenManByJiuhan();
					break;
				case 3:
					monthlyEventCollection.AddVillageWoodenManByJinHuanger();
					break;
				case 4:
					monthlyEventCollection.AddVillageWoodenManByYiYihou();
					break;
				case 5:
					monthlyEventCollection.AddVillageWoodenManByWeiQi();
					break;
				case 6:
					monthlyEventCollection.AddVillageWoodenManByYixiang();
					break;
				case 7:
					monthlyEventCollection.AddVillageWoodenManByXuefeng();
					break;
				case 8:
					monthlyEventCollection.AddVillageWoodenManByShuFang();
					break;
				}
			}
		}
		DomainManager.Extra.SetXiangshuIdInKungfuPracticeRoom(xiangshuIdInKungfuPracticeRoom, context);
	}

	private void UpdateStoneRoomData(DataContext context)
	{
		List<GameData.Domains.Character.Character> list = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
		List<GameData.Domains.Character.Character> list2 = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
		List<Predicate<GameData.Domains.Character.Character>> list3 = ObjectPool<List<Predicate<GameData.Domains.Character.Character>>>.Instance.Get();
		list3.Clear();
		list2.Clear();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		sbyte taiwuConsummateLevel = DomainManager.Taiwu.GetTaiwu().GetConsummateLevel();
		if (taiwuConsummateLevel < 6)
		{
			return;
		}
		list3.Add(CharacterMatchers.MatchNotCalledByAdventure);
		list3.Add((GameData.Domains.Character.Character character2) => CharacterMatchers.MatchConsummateLevel(character2, 0, (sbyte)(taiwuConsummateLevel - 6)));
		for (short num = 0; num < 135; num++)
		{
			list.Clear();
			MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(num);
			if (element_Areas.StationUnlocked)
			{
				MapCharacterFilter.FindInfected(list3, list, num);
				if (list.Count != 0)
				{
					list2.AddRange(list);
				}
			}
		}
		int val = context.Random.Next(1, 4);
		val = Math.Min(list2.Count, val);
		for (int num2 = 0; num2 < val; num2++)
		{
			if (DomainManager.Extra.IsStoneRoomFull())
			{
				break;
			}
			int index = context.Random.Next(0, list2.Count);
			GameData.Domains.Character.Character character = list2[index];
			int id = character.GetId();
			Location location = character.GetLocation();
			CollectionUtils.SwapAndRemove(list2, index);
			if (character.IsActiveExternalRelationState(32))
			{
				Logger.AppendWarning($"Trying to add {character} at {location} to stone room when the character is already in settlement prison.");
			}
			else if (character.IsActiveExternalRelationState(8))
			{
				Logger.AppendWarning($"Trying to add {character} at {location} to stone room when the character is already in stone room.");
			}
			else if (context.Random.CheckPercentProb(GlobalConfig.Instance.ImprisonInStoneHouseChance))
			{
				DomainManager.Extra.AddStoneRoomCharacter(context, character);
				short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
				monthlyNotificationCollection.AddStoneHouseInfectedKidnapped(taiwuVillageSettlementId, id);
				lifeRecordCollection.AddXiangshuInfectedPrisonTaiwuVillage(id, currDate, location);
			}
			else
			{
				sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
				sbyte sectID = MapState.Instance[stateTemplateIdByAreaId].SectID;
				Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(sectID);
				sect.AddPrisoner(context, character, 39);
				lifeRecordCollection.AddXiangshuInfectedPrisonSettlement(id, currDate, location, sect.GetId());
			}
		}
		ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list);
		ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list2);
		ObjectPool<List<Predicate<GameData.Domains.Character.Character>>>.Instance.Return(list3);
	}

	public sbyte GetResourceBlockGrowthChance(BuildingBlockKey blockKey)
	{
		BuildingBlockData buildingBlockData = GetBuildingBlockData(blockKey);
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[buildingBlockData.TemplateId];
		if (BuildingBlockLevel(blockKey) >= buildingBlockItem.MaxLevel)
		{
			return 0;
		}
		int num = 0;
		BuildingAreaData buildingAreaData = GetBuildingAreaData(blockKey.GetLocation());
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		buildingAreaData.GetNeighborBlocks(blockKey.BuildingBlockIndex, buildingBlockItem.Width, list, list2, 2);
		for (int i = 0; i < list.Count; i++)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, list[i]);
			BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(buildingBlockKey);
			sbyte b = BuildingBlockLevel(buildingBlockKey);
			if (element_BuildingBlocks.RootBlockIndex >= 0)
			{
				buildingBlockKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, element_BuildingBlocks.RootBlockIndex);
				element_BuildingBlocks = GetElement_BuildingBlocks(buildingBlockKey);
			}
			BuildingBlockItem buildingBlockItem2 = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
			if (!buildingBlockItem2.IsCollectResourceBuilding || buildingBlockItem2.DependBuildings[0] != buildingBlockData.TemplateId || !element_BuildingBlocks.CanUse() || !_shopManagerDict.ContainsKey(buildingBlockKey))
			{
				continue;
			}
			List<int> collection = _shopManagerDict[buildingBlockKey].GetCollection();
			int num2 = 0;
			bool flag = false;
			foreach (int item in collection)
			{
				if (item >= 0 && DomainManager.Taiwu.CanWork(item))
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
					num2 += element_Objects.GetLifeSkillAttainment(buildingBlockItem.RequireLifeSkillType);
					flag = true;
				}
			}
			if (flag)
			{
				num += b * (50 + num2) / 100;
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<int>>.Instance.Return(list2);
		int value = num * buildingBlockData.Durability / buildingBlockItem.MaxDurability;
		return (sbyte)Math.Clamp(value, 0, 100);
	}

	public sbyte GetResourceBlockExpandChance(BuildingBlockKey blockKey)
	{
		BuildingBlockData buildingBlockData = GetBuildingBlockData(blockKey);
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[buildingBlockData.TemplateId];
		if (BuildingBlockLevel(blockKey) >= buildingBlockItem.MaxLevel)
		{
			return 0;
		}
		int num = 0;
		BuildingAreaData buildingAreaData = GetBuildingAreaData(blockKey.GetLocation());
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		buildingAreaData.GetNeighborBlocks(blockKey.BuildingBlockIndex, buildingBlockItem.Width, list, list2, 2);
		for (int i = 0; i < list.Count; i++)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, list[i]);
			BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(buildingBlockKey);
			if (element_BuildingBlocks.RootBlockIndex >= 0)
			{
				buildingBlockKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, element_BuildingBlocks.RootBlockIndex);
				element_BuildingBlocks = GetElement_BuildingBlocks(buildingBlockKey);
			}
			BuildingBlockItem buildingBlockItem2 = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
			if (!buildingBlockItem2.IsCollectResourceBuilding || buildingBlockItem2.DependBuildings[0] != buildingBlockData.TemplateId || !element_BuildingBlocks.CanUse() || !_shopManagerDict.ContainsKey(buildingBlockKey))
			{
				continue;
			}
			List<int> collection = _shopManagerDict[buildingBlockKey].GetCollection();
			int num2 = 0;
			bool flag = false;
			foreach (int item in collection)
			{
				if (item >= 0 && DomainManager.Taiwu.CanWork(item))
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
					num2 += element_Objects.GetLifeSkillAttainment(buildingBlockItem.RequireLifeSkillType);
					flag = true;
				}
			}
			if (flag)
			{
				num += BuildingBlockLevel(blockKey) * (50 + num2) / 100;
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<int>>.Instance.Return(list2);
		int value = num * buildingBlockData.Durability / buildingBlockItem.MaxDurability;
		return (sbyte)Math.Clamp(value, 0, 100);
	}

	private void UpdateResourceBlock(DataContext context, short settlementId, BuildingBlockKey blockKey, BuildingBlockData blockData, List<short> neighborList, List<short> expandedResourceList, List<int> neighborDistanceList, List<short> neighborRangeOneList)
	{
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[blockData.TemplateId];
		IRandomSource random = context.Random;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < neighborList.Count; i++)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, neighborList[i]);
			BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(buildingBlockKey);
			sbyte b = BuildingBlockLevel(buildingBlockKey);
			if (element_BuildingBlocks.RootBlockIndex >= 0)
			{
				buildingBlockKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, element_BuildingBlocks.RootBlockIndex);
				element_BuildingBlocks = GetElement_BuildingBlocks(buildingBlockKey);
			}
			BuildingBlockItem buildingBlockItem2 = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
			if (!buildingBlockItem2.IsCollectResourceBuilding || buildingBlockItem2.DependBuildings[0] != blockData.TemplateId || !element_BuildingBlocks.CanUse() || !_shopManagerDict.ContainsKey(buildingBlockKey))
			{
				continue;
			}
			List<int> collection = _shopManagerDict[buildingBlockKey].GetCollection();
			sbyte collectBuildingResourceType = GetCollectBuildingResourceType(buildingBlockKey);
			sbyte b2 = (sbyte)((collectBuildingResourceType < 6) ? collectBuildingResourceType : 5);
			sbyte b3 = (sbyte)((collectBuildingResourceType < 6) ? Config.ResourceType.Instance[collectBuildingResourceType].LifeSkillType : 9);
			int num3 = 0;
			int num4 = 0;
			bool flag = false;
			foreach (int item in collection)
			{
				if (item >= 0 && DomainManager.Taiwu.CanWork(item))
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
					num3 += element_Objects.GetLifeSkillAttainment(buildingBlockItem.RequireLifeSkillType);
					flag = true;
				}
			}
			num4 = SharedMethods.GetShopManageProgressDelta(buildingBlockItem2.TemplateId, BuildingTotalAttainment(buildingBlockKey, collectBuildingResourceType, out var hasManager));
			int buildingBlockEffect = GetBuildingBlockEffect(settlementId, EBuildingScaleEffect.ShopProgressBonus);
			num4 *= CValuePercentBonus.op_Implicit(buildingBlockEffect);
			if (!flag || !hasManager)
			{
				continue;
			}
			num += b * (50 + num3) / 100;
			num2 += b * (50 + num3) / 100;
			bool flag2 = true;
			if (buildingBlockItem2.SuccesEvent.Count > 0 && Config.ShopEvent.Instance.GetItem(buildingBlockItem2.SuccesEvent[0]).ItemList.Count > 0)
			{
				TryGetElement_CollectBuildingEarningsData(buildingBlockKey, out var value);
				if (value == null || value.CollectionItemList == null)
				{
					flag2 = true;
				}
				else if (value.CollectionItemList.Count >= b)
				{
					flag2 = false;
				}
			}
			if (flag2)
			{
				if (!_alreadyUpdateShopProgressBlock.ContainsKey(settlementId) || !_alreadyUpdateShopProgressBlock[settlementId].Contains(element_BuildingBlocks))
				{
					element_BuildingBlocks.OfflineChangeShopProgress(num4);
				}
				UpdateShopProgressBlock(settlementId, element_BuildingBlocks);
			}
		}
		bool flag3 = false;
		if (blockData.Durability < buildingBlockItem.MaxDurability)
		{
			blockData.Durability++;
			flag3 = true;
		}
		if (flag3)
		{
			SetElement_BuildingBlocks(blockKey, blockData, context);
		}
		void UpdateShopProgressBlock(short key, BuildingBlockData item)
		{
			if (!_alreadyUpdateShopProgressBlock.ContainsKey(key))
			{
				_alreadyUpdateShopProgressBlock.Add(key, new List<BuildingBlockData> { item });
			}
			else if (!_alreadyUpdateShopProgressBlock[key].Contains(item))
			{
				_alreadyUpdateShopProgressBlock[key].Add(item);
			}
		}
	}

	private bool UpdateBlockMaintenance(DataContext context, BuildingBlockKey blockKey, BuildingBlockData blockData, GameData.Domains.Character.Character taiwuChar)
	{
		BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
		bool flag = true;
		ResourceInts allTaiwuResources = GetAllTaiwuResources();
		int[] finalMaintenanceCost = SharedMethods.GetFinalMaintenanceCost(configData);
		for (sbyte b = 0; b < finalMaintenanceCost.Length; b++)
		{
			if (allTaiwuResources.Get(b) < finalMaintenanceCost[b])
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			for (sbyte b2 = 0; b2 < finalMaintenanceCost.Length; b2++)
			{
				ConsumeResource(context, b2, finalMaintenanceCost[b2]);
			}
		}
		return flag;
	}

	public void UpdateBrokenBuildings(DataContext context)
	{
		foreach (BuildingBlockKey newBrokenBuilding in _newBrokenBuildings)
		{
			BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(newBrokenBuilding);
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
			MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(newBrokenBuilding.AreaId);
			int settlementIndex = element_Areas.GetSettlementIndex(newBrokenBuilding.BlockId);
			short settlementId = element_Areas.SettlementInfos[settlementIndex].SettlementId;
			element_BuildingBlocks.Durability--;
			if (element_BuildingBlocks.Durability > 0)
			{
				SetElement_BuildingBlocks(newBrokenBuilding, element_BuildingBlocks, context);
				return;
			}
			element_BuildingBlocks.Durability = 0;
			if (buildingBlockItem.DestoryType == 0)
			{
				MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
				monthlyNotificationCollection.AddBuildingRuined(settlementId, element_BuildingBlocks.TemplateId);
				BuildingRemoveResident(context, element_BuildingBlocks, newBrokenBuilding);
				BuildingRemoveVillagerWork(context, newBrokenBuilding);
				ClearBuildingBlockEarningsData(context, newBrokenBuilding, element_BuildingBlocks.TemplateId == 222);
				ResetAllChildrenBlocks(context, newBrokenBuilding, 23, 1);
				element_BuildingBlocks.ResetData(23, 1, -1);
			}
			else if (buildingBlockItem.DestoryType == 1)
			{
				InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
				instantNotificationCollection.AddBuildingLoseAuthority(settlementId, element_BuildingBlocks.TemplateId);
			}
			SetElement_BuildingBlocks(newBrokenBuilding, element_BuildingBlocks, context);
		}
		UpdateTaiwuVillageBuildingEffect();
	}

	public void ParallelUpdate(DataContext context)
	{
		foreach (KeyValuePair<BuildingBlockKey, BuildingBlockData> buildingBlock in _buildingBlocks)
		{
			BuildingBlockKey key = buildingBlock.Key;
			BuildingBlockData value = buildingBlock.Value;
			if (value.RootBlockIndex >= 0)
			{
				continue;
			}
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[value.TemplateId];
			if (buildingBlockItem.Type == EBuildingBlockType.Building || BuildingBlockData.IsResource(buildingBlockItem.Type) || buildingBlockItem.TemplateId == 44)
			{
				ParallelBuildingModification parallelBuildingModification = new ParallelBuildingModification
				{
					BlockKey = key,
					BlockData = value
				};
				MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(key.AreaId);
				int settlementIndex = DomainManager.Map.GetElement_Areas(key.AreaId).GetSettlementIndex(key.BlockId);
				short settlementId = element_Areas.SettlementInfos[settlementIndex].SettlementId;
				if (value.CanUse() && buildingBlockItem.IsShop)
				{
					OfflineUpdateShopManagement(parallelBuildingModification, settlementId, buildingBlockItem, key, value, context);
				}
				OfflineUpdateOperation(context, parallelBuildingModification, settlementId, buildingBlockItem, key, value);
				ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
				parallelModificationsRecorder.RecordType(ParallelModificationType.UpdateBuilding);
				parallelModificationsRecorder.RecordParameterClass(parallelBuildingModification);
			}
		}
	}

	public void TutorialUpdate(DataContext context)
	{
		foreach (KeyValuePair<BuildingBlockKey, BuildingBlockData> buildingBlock in _buildingBlocks)
		{
			BuildingBlockKey key = buildingBlock.Key;
			BuildingBlockData value = buildingBlock.Value;
			if (value.RootBlockIndex >= 0)
			{
				continue;
			}
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[value.TemplateId];
			if (buildingBlockItem.TemplateId == 258 && value.OperationType == 0 && DomainManager.Taiwu.VillagerHasWork(DomainManager.Taiwu.GetTaiwuCharId()))
			{
				DomainManager.Extra.ModifyBuildingExtraData(context, key, delegate(BuildingBlockDataEx blockDataEx)
				{
					blockDataEx.LevelUnlockedFlags = 1uL;
				});
				value.Durability = buildingBlockItem.MaxDurability;
				value.OperationType = -1;
				value.OperationProgress = 0;
				int settlementIndex = DomainManager.Map.GetElement_Areas(key.AreaId).GetSettlementIndex(key.BlockId);
				MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(key.AreaId);
				short settlementId = element_Areas.SettlementInfos[settlementIndex].SettlementId;
				MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
				monthlyNotificationCollection.AddBuildingConstructionCompleted(settlementId, value.TemplateId);
				DomainManager.Taiwu.RemoveVillagerWork(context, DomainManager.Taiwu.GetTaiwuCharId());
			}
		}
	}

	private void UpdateTeaHorseCaravan(DataContext context)
	{
		if (_teaHorseCaravanData == null || _teaHorseCaravanData.CaravanState < 1 || _teaHorseCaravanData.CaravanState == 4)
		{
			return;
		}
		_teaHorseCaravanData.IsShowExchangeReplenishment = false;
		if (_teaHorseCaravanData.CaravanState == 1)
		{
			_teaHorseCaravanData.CaravanState = 2;
		}
		if (_teaHorseCaravanData.StartMonth >= 360)
		{
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection.AddWesternMerchantBackAfterLong();
			_teaHorseCaravanData.CaravanState = 4;
			_teaHorseCaravanData.IsShowExchangeReplenishment = false;
			_teaHorseCaravanData.IsShowSeachReplenishment = false;
			_teaHorseCaravanData.DistanceToTaiwuVillage = 0;
			_teaHorseCaravanData.Terrain = 5;
		}
		_teaHorseCaravanData.StartMonth++;
		if (_teaHorseCaravanData.IsStartSearch)
		{
			_teaHorseCaravanData.IsStartSearch = false;
			return;
		}
		sbyte currMonthInYear = DomainManager.World.GetCurrMonthInYear();
		List<TeaHorseCaravanTerrainItem> list = new List<TeaHorseCaravanTerrainItem>();
		for (short num = 0; num < TeaHorseCaravanTerrain.Instance.Count; num++)
		{
			list.Add(TeaHorseCaravanTerrain.Instance.GetItem(num));
		}
		IEnumerable<(short, short)> weights = list.Select((TeaHorseCaravanTerrainItem a) => ((short TemplateId, short))(TemplateId: a.TemplateId, a.Weighted));
		short randomResult = RandomUtils.GetRandomResult(weights, context.Random);
		_teaHorseCaravanData.Terrain = randomResult;
		List<TeaHorseCaravanWeatherItem> list2 = new List<TeaHorseCaravanWeatherItem>();
		for (short num2 = 0; num2 < TeaHorseCaravanWeather.Instance.Count; num2++)
		{
			TeaHorseCaravanWeatherItem item = TeaHorseCaravanWeather.Instance.GetItem(num2);
			if (item.NeedSolarTerms.Contains(currMonthInYear) && item.NeedTerrain.Contains((sbyte)randomResult))
			{
				list2.Add(TeaHorseCaravanWeather.Instance.GetItem(num2));
			}
		}
		IEnumerable<(short, short)> weights2 = list2.Select((TeaHorseCaravanWeatherItem a) => ((short TemplateId, short))(TemplateId: a.TemplateId, a.Weighted));
		short randomResult2 = RandomUtils.GetRandomResult(weights2, context.Random);
		_teaHorseCaravanData.Weather = randomResult2;
		short num3 = 0;
		if (_teaHorseCaravanData.LackReplenishmentTurn > 0)
		{
			num3 = Math.Clamp((short)MathF.Pow(2f, _teaHorseCaravanData.LackReplenishmentTurn - 1), (short)0, (short)100);
		}
		if (num3 > 0 && context.Random.CheckProb(num3, 100))
		{
			if (context.Random.Next(2) == 0)
			{
				if (_teaHorseCaravanData.CarryGoodsList.Count > 0)
				{
					int index = context.Random.Next(_teaHorseCaravanData.CarryGoodsList.Count);
					ItemKey item2 = _teaHorseCaravanData.CarryGoodsList[index].Item1;
					_teaHorseCaravanData.CarryGoodsList.RemoveAt(index);
					DomainManager.Item.RemoveItem(context, item2);
				}
				else if (_teaHorseCaravanData.ExchangeGoodsList.Count > 0)
				{
					int index2 = context.Random.Next(_teaHorseCaravanData.ExchangeGoodsList.Count);
					_teaHorseCaravanData.ExchangeGoodsList.RemoveAt(index2);
				}
			}
			else if (_teaHorseCaravanData.ExchangeGoodsList.Count > 0)
			{
				int index3 = context.Random.Next(_teaHorseCaravanData.ExchangeGoodsList.Count);
				_teaHorseCaravanData.ExchangeGoodsList.RemoveAt(index3);
			}
			else if (_teaHorseCaravanData.CarryGoodsList.Count > 0)
			{
				int index4 = context.Random.Next(_teaHorseCaravanData.CarryGoodsList.Count);
				ItemKey item3 = _teaHorseCaravanData.CarryGoodsList[index4].Item1;
				_teaHorseCaravanData.CarryGoodsList.RemoveAt(index4);
				DomainManager.Item.RemoveItem(context, item3);
			}
		}
		_teaHorseCaravanData.CaravanReplenishment = (short)Math.Clamp(_teaHorseCaravanData.CaravanReplenishment - _caravanReplenishmentCostPerMonth - TeaHorseCaravanWeather.Instance.GetItem(randomResult2).ReplenishmentChange, 0, _caravanReplenishmentInitValue);
		if (_teaHorseCaravanData.CaravanReplenishment == 0)
		{
			_teaHorseCaravanData.LackReplenishmentTurn++;
			MonthlyNotificationCollection monthlyNotificationCollection2 = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection2.AddWesternMerchanLackReplenishment();
		}
		else
		{
			_teaHorseCaravanData.LackReplenishmentTurn = 0;
		}
		List<TeaHorseCaravanEventItem> list3 = new List<TeaHorseCaravanEventItem>();
		for (short num4 = 0; num4 < TeaHorseCaravanEvent.Instance.Count; num4++)
		{
			list3.Add(TeaHorseCaravanEvent.Instance.GetItem(num4));
		}
		IEnumerable<(short, short)> weights3 = list3.Select((TeaHorseCaravanEventItem a) => ((short TemplateId, short))(TemplateId: a.TemplateId, a.Weighted));
		short randomResult3 = RandomUtils.GetRandomResult(weights3, context.Random);
		TeaHorseCaravanEventItem item4 = TeaHorseCaravanEvent.Instance.GetItem(randomResult3);
		if (item4.EventType == 0 && (item4.ReplenishmentTrigger != 1 || _teaHorseCaravanData.CaravanReplenishment != 0))
		{
			_teaHorseCaravanData.IsShowExchangeReplenishment = true;
			_teaHorseCaravanData.ExchangeReplenishmentAmountMax = (short)context.Random.Next((int)item4.ExchangeMin, item4.ExchangeMax + 1);
			_teaHorseCaravanData.ExchangeReplenishmentRemainAmount = _teaHorseCaravanData.ExchangeReplenishmentAmountMax;
			_teaHorseCaravanData.DiaryList.Add(randomResult3);
		}
		else if (item4.EventType == 1 && (item4.ReplenishmentTrigger != 1 || _teaHorseCaravanData.CaravanReplenishment != 0))
		{
			_teaHorseCaravanData.IsShowSeachReplenishment = true;
			_teaHorseCaravanData.SearchReplenishmentMax = (short)context.Random.Next((int)item4.SearchMin, item4.SearchMax + 1);
			_teaHorseCaravanData.SearchReplenishmentAmount = (short)context.Random.Next((int)item4.SolarSearchMin, item4.SolarSearchMax + 1);
			_teaHorseCaravanData.DiaryList.Add(randomResult3);
		}
		else if ((item4.EventType == 2 || item4.EventType == 3) && (item4.ReplenishmentTrigger != 1 || _teaHorseCaravanData.CaravanReplenishment != 0))
		{
			_teaHorseCaravanData.CaravanAwareness = (short)Math.Max(_teaHorseCaravanData.CaravanAwareness + item4.AwarenessChange, 0);
			_teaHorseCaravanData.DiaryList.Add(randomResult3);
		}
		else if (item4.EventType == 4 && (item4.ReplenishmentTrigger != 1 || _teaHorseCaravanData.CaravanReplenishment != 0))
		{
			sbyte grade = (sbyte)context.Random.Next((int)item4.GetItemGradeMin, item4.GetItemGradeMax + 1);
			ItemKey westRandomItemByGarde = GetWestRandomItemByGarde(context, grade);
			_teaHorseCaravanData.ExchangeGoodsList.Add(westRandomItemByGarde);
			_teaHorseCaravanData.DiaryList.Add(randomResult3);
		}
		else if (item4.EventType == 5 && (item4.ReplenishmentTrigger != 1 || _teaHorseCaravanData.CaravanReplenishment != 0))
		{
			int num5 = context.Random.Next((int)item4.LoseGoodsNumMin, item4.LoseGoodsNumMax + 1);
			for (int num6 = 0; num6 < num5; num6++)
			{
				if (context.Random.Next(2) == 0)
				{
					if (_teaHorseCaravanData.CarryGoodsList.Count > 0)
					{
						int index5 = context.Random.Next(_teaHorseCaravanData.CarryGoodsList.Count);
						ItemKey item5 = _teaHorseCaravanData.CarryGoodsList[index5].Item1;
						_teaHorseCaravanData.CarryGoodsList.RemoveAt(index5);
						DomainManager.Item.RemoveItem(context, item5);
					}
					else if (_teaHorseCaravanData.ExchangeGoodsList.Count > 0)
					{
						int index6 = context.Random.Next(_teaHorseCaravanData.ExchangeGoodsList.Count);
						_teaHorseCaravanData.ExchangeGoodsList.RemoveAt(index6);
					}
				}
				else if (_teaHorseCaravanData.ExchangeGoodsList.Count > 0)
				{
					int index7 = context.Random.Next(_teaHorseCaravanData.ExchangeGoodsList.Count);
					_teaHorseCaravanData.ExchangeGoodsList.RemoveAt(index7);
				}
				else if (_teaHorseCaravanData.CarryGoodsList.Count > 0)
				{
					int index8 = context.Random.Next(_teaHorseCaravanData.CarryGoodsList.Count);
					ItemKey item6 = _teaHorseCaravanData.CarryGoodsList[index8].Item1;
					_teaHorseCaravanData.CarryGoodsList.RemoveAt(index8);
					DomainManager.Item.RemoveItem(context, item6);
				}
			}
			_teaHorseCaravanData.DiaryList.Add(randomResult3);
		}
		else if ((item4.EventType == 6 || item4.EventType == 7) && (item4.ReplenishmentTrigger != 1 || _teaHorseCaravanData.CaravanReplenishment != 0))
		{
			int num7 = context.Random.Next((int)item4.ReplenishmentChangeMin, item4.ReplenishmentChangeMax + 1);
			_teaHorseCaravanData.CaravanReplenishment = (short)Math.Clamp(_teaHorseCaravanData.CaravanReplenishment + num7, 0, _caravanReplenishmentInitValue);
			_teaHorseCaravanData.DiaryList.Add(randomResult3);
		}
		MonthlyNotificationCollection monthlyNotificationCollection3 = DomainManager.World.GetMonthlyNotificationCollection();
		switch (item4.TemplateId)
		{
		case 0:
			monthlyNotificationCollection3.AddWesternMerchanFindMirage();
			break;
		case 1:
			monthlyNotificationCollection3.AddWesternMerchanFindBigfoot();
			break;
		case 2:
			monthlyNotificationCollection3.AddWesternMerchanFindAnimal();
			break;
		case 3:
			monthlyNotificationCollection3.AddWesternMerchanFindPlant();
			break;
		case 4:
			DomainManager.Information.GiveOrUpgradeWesternRegionInformation(context, DomainManager.Taiwu.GetTaiwuCharId(), WesternRegion.Instance.GetAllKeys().GetRandom(context.Random));
			monthlyNotificationCollection3.AddWesternMerchanGetInformation();
			break;
		case 5:
			monthlyNotificationCollection3.AddWesternMerchanFindSettlement();
			break;
		case 6:
			monthlyNotificationCollection3.AddWesternMerchanFindWeather();
			break;
		case 7:
			monthlyNotificationCollection3.AddWesternMerchanLost();
			break;
		case 8:
			monthlyNotificationCollection3.AddWesternMerchanMeetTheif();
			break;
		case 9:
			monthlyNotificationCollection3.AddWesternMerchanGoodsDamage();
			break;
		case 10:
			monthlyNotificationCollection3.AddWesternMerchanFindWreckage();
			break;
		case 11:
			monthlyNotificationCollection3.AddWesternMerchanHelpPasserby();
			break;
		case 12:
			monthlyNotificationCollection3.AddWesternMerchanUnacclimatized();
			break;
		case 13:
			monthlyNotificationCollection3.AddWesternMerchanGetHelp();
			break;
		case 14:
			monthlyNotificationCollection3.AddWesternMerchanFindVenison();
			break;
		case 15:
		case 16:
		case 17:
			monthlyNotificationCollection3.AddWesternMerchanFindFruit();
			break;
		case 18:
		case 19:
		case 20:
			monthlyNotificationCollection3.AddWesternMerchanFindVillage();
			break;
		case 21:
		case 22:
		case 23:
			monthlyNotificationCollection3.AddWesternMerchanMeetMerchan();
			break;
		}
		if (_teaHorseCaravanData.CaravanState == 2 && _teaHorseCaravanData.CaravanReplenishment != 0 && _teaHorseCaravanData.CarryGoodsList.Count > 0)
		{
			int index9 = 0;
			(ItemKey, sbyte) tuple = _teaHorseCaravanData.CarryGoodsList[index9];
			ItemKey item7 = ItemKey.Invalid;
			int num8 = ItemTemplateHelper.GetBaseValue(tuple.Item1.ItemType, tuple.Item1.TemplateId) + _teaHorseCaravanData.CaravanAwareness;
			int num9 = 0;
			for (int num10 = 0; num10 < _westTreasureTemplateId.Count; num10++)
			{
				int baseValue = ItemTemplateHelper.GetBaseValue(12, _westTreasureTemplateId[num10]);
				if (num8 >= baseValue)
				{
					num9 = num10;
				}
			}
			num9 += 4;
			if (num9 > 4)
			{
				item7 = ((!context.Random.CheckProb(40, 100)) ? GetWestRandomItemByGarde(context, (sbyte)(num9 - 1)) : GetWestRandomItemByGarde(context, (sbyte)num9));
			}
			else
			{
				int baseValue2 = ItemTemplateHelper.GetBaseValue(12, _westTreasureTemplateId[0]);
				int value = (_teaHorseCaravanData.CaravanAwareness + num8) * 100 / baseValue2 - GlobalConfig.Instance.CaravanExchangeProbPenalize;
				int threshold = Math.Clamp(value, 0, 100);
				if (context.Random.CheckProb(threshold, 100))
				{
					item7 = GetWestRandomItemByGarde(context, (sbyte)num9);
				}
			}
			if (!item7.Equals(ItemKey.Invalid))
			{
				_teaHorseCaravanData.ExchangeGoodsList.Add(item7);
				_teaHorseCaravanData.CarryGoodsList.RemoveAt(index9);
			}
		}
		if (_teaHorseCaravanData.CaravanState == 2)
		{
			_teaHorseCaravanData.DistanceToTaiwuVillage++;
		}
		else if (_teaHorseCaravanData.CaravanState == 3)
		{
			_teaHorseCaravanData.DistanceToTaiwuVillage = (short)Math.Max(0, _teaHorseCaravanData.DistanceToTaiwuVillage - 1);
		}
		if (_teaHorseCaravanData.CaravanState == 2)
		{
			_teaHorseCaravanData.CaravanAwareness = (short)Math.Max(0, _teaHorseCaravanData.CaravanAwareness + 100);
		}
		if (_teaHorseCaravanData.CarryGoodsList.Count == 0)
		{
			_teaHorseCaravanData.CaravanState = 3;
		}
		if (_teaHorseCaravanData.CarryGoodsList.Count == 0 && _teaHorseCaravanData.ExchangeGoodsList.Count == 0)
		{
			ResetTeaHorseCaravanData(context);
			MonthlyNotificationCollection monthlyNotificationCollection4 = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection4.AddWesternMerchantLoseContact();
		}
		if (_teaHorseCaravanData.CaravanState == 3 && _teaHorseCaravanData.DistanceToTaiwuVillage == 0)
		{
			_teaHorseCaravanData.CaravanState = 4;
			_teaHorseCaravanData.IsShowExchangeReplenishment = false;
			_teaHorseCaravanData.IsShowSeachReplenishment = false;
			_teaHorseCaravanData.Terrain = 5;
			MonthlyNotificationCollection monthlyNotificationCollection5 = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection5.AddWesternMerchantBackSucceed();
		}
		SetTeaHorseCaravanData(_teaHorseCaravanData, context);
		DomainManager.Building.UpgradeTeaHorseCaravanByAwareness(context);
	}

	internal void CompleteBuilding(DataContext ctx, BuildingBlockKey blockKey, BuildingBlockData blockData)
	{
		blockData.Durability = blockData.ConfigData.MaxDurability;
		if (blockData.TemplateId == 46)
		{
			SetResidenceAutoCheckIn(ctx, blockKey.BuildingBlockIndex, isAutoCheckIn: true);
		}
		if (blockData.TemplateId == 47)
		{
			SetComfortableAutoCheckIn(ctx, blockKey.BuildingBlockIndex, isAutoCheckIn: true);
			DomainManager.Extra.FeastSetAutoRefill(ctx, blockKey, value: true);
		}
		EBuildingBlockType type = blockData.ConfigData.Type;
		if ((uint)type <= 1u)
		{
			DomainManager.Extra.SetResourceBlockExtraData(ctx, new ResourceBlockExtraData(blockKey));
		}
	}

	private void OnBuildingRemoved(DataContext context, BuildingBlockKey blockKey)
	{
		DomainManager.Extra.TryRemoveResourceBlockExtraData(context, blockKey);
		DomainManager.Extra.TryRemoveBuildingArtisanOrder(context, blockKey);
	}

	private void OfflineUpdateOperation(DataContext context, ParallelBuildingModification modification, short settlementId, BuildingBlockItem configData, BuildingBlockKey blockKey, BuildingBlockData blockData)
	{
		if (blockData.OperationType == -1 || !_buildingOperatorDict.ContainsKey(blockKey))
		{
			return;
		}
		BuildingBlockDataEx element_BuildingBlockDataEx = DomainManager.Extra.GetElement_BuildingBlockDataEx((ulong)blockKey);
		List<int> collection = _buildingOperatorDict[blockKey].GetCollection();
		int num = 0;
		short num2 = configData.OperationTotalProgress[blockData.OperationType];
		if (blockData.OperationStopping)
		{
			num = num2;
		}
		else
		{
			num += GetOperationSumValue(collection);
			if (collection.Count == 0 && blockData.OperationType == 0)
			{
				AddBuildingException(blockKey, blockData, BuildingExceptionType.BuildStoppedForWorkerShortage);
			}
		}
		if (num <= 0)
		{
			return;
		}
		if (num2 > 0)
		{
			blockData.OperationProgress = (short)Math.Clamp(blockData.OperationProgress + ((!blockData.OperationStopping) ? num : (-num)), 0, num2);
		}
		if (blockData.OperationProgress != ((!blockData.OperationStopping) ? num2 : 0))
		{
			return;
		}
		modification.FreeOperator = true;
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		switch (blockData.OperationType)
		{
		case 0:
			if (!blockData.OperationStopping)
			{
				modification.AddBuilding = true;
				monthlyNotificationCollection.AddBuildingConstructionCompleted(settlementId, blockData.TemplateId);
				_newCompleteOperationBuildings.Add(blockKey);
				modification.BuildingOperationComplete = true;
				CompleteBuilding(context, blockKey, blockData);
				break;
			}
			modification.ResetAllChildrenBlocks = true;
			if (SharedMethods.NeedCostResourceToBuild(configData))
			{
				for (sbyte b2 = 0; b2 < 8; b2++)
				{
					modification.SetResource(b2, configData.BaseBuildCost[b2] / 2);
				}
			}
			if (configData.BuildingCoreItem != -1)
			{
				DomainManager.Taiwu.ReturnBuildingCoreItem(DataContextManager.GetCurrentThreadDataContext(), configData);
			}
			break;
		case 1:
			if (!blockData.OperationStopping)
			{
				modification.RemoveMakeItemData = true;
				modification.RemoveEventBookData = true;
				modification.RemoveResidence = true;
				modification.FreeShopManager = true;
				sbyte level = DomainManager.Building.BuildingBlockLevel(blockKey);
				for (sbyte b = 0; b < 8; b++)
				{
					int resourceReturnOfRemoveBuilding = SharedMethods.GetResourceReturnOfRemoveBuilding(configData, level, b, blockData, element_BuildingBlockDataEx);
					modification.SetResource(b, resourceReturnOfRemoveBuilding);
				}
				if (_CollectBuildingResourceType.ContainsKey(blockKey))
				{
					modification.RemoveCollectResourceType = true;
				}
				modification.ResetAllChildrenBlocks = true;
				monthlyNotificationCollection.AddBuildingDemolitionCompleted(settlementId, blockData.TemplateId);
				modification.BuildingOperationComplete = true;
				DomainManager.Extra.ResetBuildingExtraData(context, blockKey);
				OnBuildingRemoved(context, blockKey);
			}
			break;
		}
		blockData.OperationType = -1;
		blockData.OperationProgress = 0;
	}

	private void OfflineUpdateShopManagement(ParallelBuildingModification modification, short settlementId, BuildingBlockItem buildingBlockCfg, BuildingBlockKey blockKey, BuildingBlockData blockData, DataContext context)
	{
		if (!HasShopManagerLeader(blockKey))
		{
			AddBuildingException(blockKey, blockData, BuildingExceptionType.ManageStoppedForNoLeader);
			return;
		}
		if (ShopBuildingCanTeach(blockKey) && HasShopManagerLeader(blockKey))
		{
			UpdateShopBuildingTeach(context, modification, blockKey, blockData);
		}
		IRandomSource random = context.Random;
		if (blockData.TemplateId == 105)
		{
			if (!_collectBuildingEarningsData.ContainsKey(blockKey) || (TryGetElement_CollectBuildingEarningsData(blockKey, out var value) && value != null && value.FixBookInfoList.Count <= 0))
			{
				return;
			}
			ItemKey item = value.FixBookInfoList[0];
			if (!item.IsValid())
			{
				return;
			}
			GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(item.Id);
			if (!element_SkillBooks.CanFix())
			{
				blockData.OfflineResetShopProgress();
				return;
			}
			short item2 = element_SkillBooks.GetFixProgress().needProgress;
			if (blockData.ShopProgress >= item2)
			{
				blockData.OfflineResetShopProgress();
				ParallelBuildingModification parallelBuildingModification = modification;
				if (parallelBuildingModification.FixBookList == null)
				{
					parallelBuildingModification.FixBookList = new List<ItemKey>();
				}
				modification.FixBookList.Add(item);
			}
			return;
		}
		sbyte buildingSlotCount = SharedMethods.GetBuildingSlotCount(blockData.TemplateId);
		if (TryGetElement_CollectBuildingEarningsData(blockKey, out var value2) && value2 != null && buildingSlotCount <= value2.RecruitLevelList.Count)
		{
			return;
		}
		if (buildingBlockCfg.IsShop && buildingBlockCfg.SuccesEvent.Count > 0)
		{
			ShopEventItem shopEventItem = Config.ShopEvent.Instance[buildingBlockCfg.SuccesEvent[0]];
			List<sbyte> resourceList = shopEventItem.ResourceList;
			if (resourceList != null && resourceList.Count > 0)
			{
				sbyte collectBuildingResourceType = GetCollectBuildingResourceType(blockKey);
				sbyte resourceType = (sbyte)((collectBuildingResourceType < 6) ? collectBuildingResourceType : 5);
				int num = CalcResourceOutputCount(blockKey, resourceType);
				DomainManager.Taiwu.GetTaiwu().ChangeResource(context, resourceType, num);
				ApplyBuildingResourceOutputSetting(context, blockKey, resourceType, num);
			}
		}
		if ((value2 != null && buildingSlotCount <= value2.CollectionItemList.Count + value2.CollectionResourceList.Count) || blockData.ShopProgress < buildingBlockCfg.MaxProduceValue)
		{
			return;
		}
		blockData.OfflineResetShopProgress();
		int date = DomainManager.World.GetCurrDate();
		ShopEventCollection shopEventCollection = GetOrCreateShopEventCollection(blockKey);
		MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
		if (!buildingBlockCfg.IsShop || buildingBlockCfg.SuccesEvent.Count <= 0)
		{
			return;
		}
		ShopEventItem shopEventItem2 = Config.ShopEvent.Instance[buildingBlockCfg.SuccesEvent[0]];
		ShopEventItem shopEventCfg = Config.ShopEvent.Instance[buildingBlockCfg.FailEvent[0]];
		if (shopEventItem2.ItemList.Count > 0)
		{
			List<TemplateKey> list = ObjectPool<List<TemplateKey>>.Instance.Get();
			list.Clear();
			int num2 = 0;
			int num3 = shopEventItem2.ItemList.Count;
			sbyte b = -1;
			if (shopEventItem2.ResourceList.Count > 1)
			{
				b = GetCollectBuildingResourceType(blockKey);
				if (b == shopEventItem2.ResourceList[0])
				{
					num3 = shopEventItem2.ItemList.Count / 2;
				}
				else
				{
					num2 = shopEventItem2.ItemList.Count / 2;
				}
			}
			for (int i = num2; i < num3; i++)
			{
				bool hasManager;
				int percentProb = (shopEventItem2.ItemList[i].Amount + BuildingTotalAttainment(blockKey, b, out hasManager) / 30) * BuildingProductivityByMaxDependencies(blockKey) / 100;
				if (hasManager && random.CheckPercentProb(percentProb))
				{
					PresetInventoryItem presetInventoryItem = shopEventItem2.ItemList[i];
					list.Add(new TemplateKey(presetInventoryItem.Type, presetInventoryItem.TemplateId));
				}
			}
			if (list.Count > 0)
			{
				TemplateKey templateKey = list[random.Next(0, list.Count)];
				modification.AddCollectableEarning(templateKey);
				shopEventCollection.AddCollectItemSuccessRecord(shopEventItem2, date, templateKey.ItemType, templateKey.TemplateId);
				monthlyNotifications.AddBuildingIncome(settlementId, buildingBlockCfg.TemplateId);
			}
			else
			{
				shopEventCollection.AddFailureRecord(shopEventCfg, date);
			}
			ObjectPool<List<TemplateKey>>.Instance.Return(list);
		}
		if (shopEventItem2.ItemGradeProbList.Count > 0)
		{
			List<sbyte> list2 = ObjectPool<List<sbyte>>.Instance.Get();
			list2.Clear();
			for (sbyte b2 = 0; b2 < shopEventItem2.ItemGradeProbList.Count; b2++)
			{
				int percentProb2 = shopEventItem2.ItemGradeProbList[b2] + GetBuildingAttainment(blockData, blockKey, isAverage: true) / AttainmentToProb;
				if (random.CheckPercentProb(percentProb2))
				{
					list2.Add(b2);
				}
			}
			if (list2.Count > 0)
			{
				sbyte grade = list2[random.Next(0, list2.Count)];
				ItemKey randomItemByGrade = GetRandomItemByGrade(random, grade, -1);
				TemplateKey templateKey2 = new TemplateKey(randomItemByGrade.ItemType, randomItemByGrade.TemplateId);
				modification.AddCollectableEarning(templateKey2);
				shopEventCollection.AddManageEclecticBuildingSuccess6(date, templateKey2.ItemType, templateKey2.TemplateId, 6, ItemTemplateHelper.GetBaseValue(templateKey2.ItemType, templateKey2.TemplateId));
			}
			else
			{
				shopEventCollection.AddFailureRecord(shopEventCfg, date);
			}
		}
		if (shopEventItem2.ResourceGoods != -1 && TryCalcShopManagementYieldAmount(random, blockKey, out var resourceType2, out var amount, out var _))
		{
			ParallelBuildingModification parallelBuildingModification = modification;
			if (parallelBuildingModification.BuildingMoneyPrestigeSuccessRateCompensationChanged == null)
			{
				parallelBuildingModification.BuildingMoneyPrestigeSuccessRateCompensationChanged = new Dictionary<BuildingBlockKey, int>();
			}
			modification.BuildingMoneyPrestigeSuccessRateCompensationChanged[blockKey] = 0;
			if (resourceType2 >= 0 && amount > 0)
			{
				modification.AddCollectableResources(resourceType2, amount);
				shopEventCollection.AddCollectResourceSuccessRecord(shopEventItem2, date, resourceType2, amount);
				monthlyNotifications.AddBuildingIncome(settlementId, buildingBlockCfg.TemplateId);
				List<int> collection = DomainManager.Building.GetElement_ShopManagerDict(blockKey).GetCollection();
				for (int j = 0; j < collection.Count; j++)
				{
					int num4 = collection[j];
					if (DomainManager.Character.TryGetElement_Objects(num4, out var element) && element.GetAgeGroup() == 2)
					{
						int amount2 = amount * GlobalConfig.Instance.ShopBuildingSharePencent[(j != 0) ? 1 : 0] / 100;
						modification.AddShopSalaryResources(num4, resourceType2, amount2);
					}
				}
			}
		}
		if (shopEventItem2.RecruitPeopleProb.Count > 0)
		{
			BuildingBlockDataEx element_BuildingBlockDataEx = DomainManager.Extra.GetElement_BuildingBlockDataEx((ulong)blockKey);
			int first = CalcRecruitGrade(random, blockKey, ref element_BuildingBlockDataEx.CumulatedScore);
			ParallelBuildingModification parallelBuildingModification = modification;
			if (parallelBuildingModification.RecruitLevelList == null)
			{
				parallelBuildingModification.RecruitLevelList = new List<IntPair>();
			}
			modification.RecruitLevelList.Add(new IntPair(first, 0));
			InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
			instantNotificationCollection.AddCandidateArrived(settlementId, blockData.TemplateId);
			if (blockData.TemplateId == 223)
			{
				shopEventCollection.AddRecruitWithCostSuccessRecord(shopEventItem2, date, 7, GlobalConfig.Instance.RecruitPeopleCost);
			}
			else
			{
				shopEventCollection.AddRecruitSuccessRecord(shopEventItem2, date);
			}
		}
		if (shopEventItem2.ExchangeResourceGoods == -1 || !TryGetElement_CollectBuildingEarningsData(blockKey, out var value3))
		{
			return;
		}
		int num5 = 0;
		for (int k = 0; k < value3.ShopSoldItemList.Count; k++)
		{
			if (value3.ShopSoldItemList[k].TemplateId != -1 && random.CheckProb(50, 100))
			{
				SellItemSuccess(shopEventItem2, value2, k);
				num5++;
			}
		}
		if (num5 != 0)
		{
			return;
		}
		for (int l = 0; l < value3.ShopSoldItemList.Count; l++)
		{
			if (value3.ShopSoldItemList[l].TemplateId != -1)
			{
				SellItemSuccess(shopEventItem2, value2, l);
				break;
			}
		}
		void SellItemSuccess(ShopEventItem successShopEventConfig, BuildingEarningsData data, int index)
		{
			sbyte exchangeResourceGoods = successShopEventConfig.ExchangeResourceGoods;
			int basePrice;
			BuildingProduceDependencyData dependencyData2;
			int num6 = CalcSoldItemValue(random, blockKey, blockData, data.ShopSoldItemList[index], out basePrice, out dependencyData2);
			int num7 = num6 / GlobalConfig.ResourcesWorth[exchangeResourceGoods];
			ParallelBuildingModification parallelBuildingModification2 = modification;
			if (parallelBuildingModification2.ShopSoldItems == null)
			{
				parallelBuildingModification2.ShopSoldItems = new List<(sbyte, IntPair)>();
			}
			modification.ShopSoldItems.Add(((sbyte)index, new IntPair(exchangeResourceGoods, num7)));
			ItemKey itemKey = data.ShopSoldItemList[index];
			shopEventCollection.AddSellItemSuccessRecord(successShopEventConfig, date, itemKey.ItemType, itemKey.TemplateId, exchangeResourceGoods, num7);
			monthlyNotifications.AddBuildingIncome(settlementId, buildingBlockCfg.TemplateId);
		}
	}

	private void UpdateShopBuildingTeach(DataContext context, ParallelBuildingModification modification, BuildingBlockKey blockKey, BuildingBlockData blockData)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		CharacterList element_ShopManagerDict = GetElement_ShopManagerDict(blockKey);
		if (element_ShopManagerDict.GetRealCount() <= 1)
		{
			return;
		}
		ShopEventCollection orCreateShopEventCollection = GetOrCreateShopEventCollection(blockKey);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[blockData.TemplateId];
		CValuePercentBonus val = CValuePercentBonus.op_Implicit((buildingBlockItem.RequireLifeSkillType >= 0) ? GetBuildingBlockEffect(taiwuVillageSettlementId, EBuildingScaleEffect.ShopManagerQualificationImproveRate, buildingBlockItem.RequireLifeSkillType) : 0);
		int num = element_ShopManagerDict[0];
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num);
		for (int i = 1; i < 7; i++)
		{
			int num2 = element_ShopManagerDict[i];
			if (num2 < 0)
			{
				continue;
			}
			GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(num2);
			DomainManager.Extra.TryGetElement_TaiwuVillagerPotentialData(num2, out var value);
			sbyte b = element_Objects2.GetPersonalities()[buildingBlockItem.RequirePersonalityType];
			sbyte b2 = ((buildingBlockItem.RequireLifeSkillType >= 0) ? buildingBlockItem.RequireLifeSkillType : buildingBlockItem.RequireCombatSkillType);
			if (value < GlobalConfig.Instance.TaiwuVillagerMaxPotential)
			{
				if (value != 0 && value % 4 == 0)
				{
					DomainManager.Extra.TrySetShopVillagerQualificationImprove(element_Objects2, b2, buildingBlockItem.RequireLifeSkillType >= 0);
					if (buildingBlockItem.RequireLifeSkillType >= 0)
					{
						orCreateShopEventCollection.AddBaseDevelopLifeSkill(currDate, num2, b2);
						lifeRecordCollection.AddShopBuildingBaseDevelopLifeSkill(num2, currDate, buildingBlockItem.TemplateId, b2);
					}
					else
					{
						orCreateShopEventCollection.AddBaseDevelopCombatSkill(currDate, num2, b2);
						lifeRecordCollection.AddShopBuildingBaseDevelopCombatSkill(num2, currDate, buildingBlockItem.TemplateId, b2);
					}
				}
				int percentProb = b / 3 * val;
				if (context.Random.CheckPercentProb(percentProb))
				{
					DomainManager.Extra.TrySetShopVillagerQualificationImprove(element_Objects2, b2, buildingBlockItem.RequireLifeSkillType >= 0);
					if (buildingBlockItem.RequireLifeSkillType >= 0)
					{
						orCreateShopEventCollection.AddPersonalityDevelopLifeSkill(currDate, num2, b2);
						lifeRecordCollection.AddShopBuildingPersonalityDevelopLifeSkill(num2, currDate, buildingBlockItem.TemplateId, b2);
					}
					else
					{
						orCreateShopEventCollection.AddPersonalityDevelopCombatSkill(currDate, num2, b2);
						lifeRecordCollection.AddShopBuildingPersonalityDevelopCombatSkill(num2, currDate, buildingBlockItem.TemplateId, b2);
					}
				}
				short num3 = ((buildingBlockItem.RequireLifeSkillType >= 0) ? element_Objects.GetLifeSkillQualification(buildingBlockItem.RequireLifeSkillType) : element_Objects.GetLifeSkillQualification(buildingBlockItem.RequireCombatSkillType));
				short num4 = ((buildingBlockItem.RequireLifeSkillType >= 0) ? element_Objects2.GetLifeSkillQualification(buildingBlockItem.RequireLifeSkillType) : element_Objects2.GetLifeSkillQualification(buildingBlockItem.RequireCombatSkillType));
				int percentProb2 = (num3 - num4) * 3 * val;
				if (context.Random.CheckPercentProb(percentProb2))
				{
					DomainManager.Extra.TrySetShopVillagerQualificationImprove(element_Objects2, b2, buildingBlockItem.RequireLifeSkillType >= 0);
					if (buildingBlockItem.RequireLifeSkillType >= 0)
					{
						orCreateShopEventCollection.AddLeaderDevelopLifeSkill(currDate, num2, num, b2);
						lifeRecordCollection.AddShopBuildingLeaderDevelopLifeSkill(num2, currDate, num, buildingBlockItem.TemplateId, b2);
					}
					else
					{
						orCreateShopEventCollection.AddLeaderDevelopCombatSkill(currDate, num2, num, b2);
						lifeRecordCollection.AddShopBuildingLeaderDevelopCombatSkill(num2, currDate, num, buildingBlockItem.TemplateId, b2);
					}
				}
				DomainManager.Extra.UpdateTaiwuVillagerPotentialData(context, num2, ++value);
			}
			short num5 = ((buildingBlockItem.RequireLifeSkillType >= 0) ? element_Objects2.GetLifeSkillAttainment(buildingBlockItem.RequireLifeSkillType) : element_Objects2.GetCombatSkillAttainment(buildingBlockItem.RequireCombatSkillType));
			int point = num5 * (100 + b) / 100;
			bool hasMemberUnread;
			if (buildingBlockItem.RequireLifeSkillType >= 0)
			{
				List<(short, byte)> list = ShopBuildingTeachLifeSkillBook(element_Objects, element_Objects2, point, blockData.TemplateId, out hasMemberUnread);
				foreach (var (num6, b3) in list)
				{
					modification.AddLearnLifeSkill(num2, num6, b3);
					Config.LifeSkillItem lifeSkillItem = LifeSkill.Instance[num6];
					SkillBookItem skillBookItem = Config.SkillBook.Instance[lifeSkillItem.SkillBookId];
					lifeRecordCollection.AddShopBuildingLearnLifeSkill(num2, currDate, buildingBlockItem.TemplateId, skillBookItem.ItemType, skillBookItem.TemplateId, b3 + 1);
					orCreateShopEventCollection.AddLearnLifeSkill(currDate, num2, skillBookItem.ItemType, skillBookItem.TemplateId, b3 + 1);
				}
				continue;
			}
			List<(short, byte, sbyte)> list2 = ShopBuildingTeachCombatSkillBook(element_Objects, element_Objects2, point, blockData.TemplateId, out hasMemberUnread);
			foreach (var (num7, pageInternalIndex, _) in list2)
			{
				modification.AddLearnCombatSkill(num2, num7, pageInternalIndex);
				CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[num7];
				SkillBookItem skillBookItem2 = Config.SkillBook.Instance[combatSkillItem.BookId];
				byte pageId = CombatSkillStateHelper.GetPageId(pageInternalIndex);
				lifeRecordCollection.AddShopBuildingLearnCombatSkill(num2, currDate, buildingBlockItem.TemplateId, skillBookItem2.ItemType, skillBookItem2.TemplateId, pageId + 1);
				orCreateShopEventCollection.AddLearnCombatSkill(currDate, num2, skillBookItem2.ItemType, skillBookItem2.TemplateId, pageId + 1);
			}
		}
	}

	[Obsolete]
	private (short skillTemplateId, byte pageInternalIndex) ShopManagerSelectCombatSkillToLearn(IRandomSource random, GameData.Domains.Character.Character character, sbyte combatSkillType, sbyte maxGrade)
	{
		int id = character.GetId();
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(id);
		sbyte b = sbyte.MaxValue;
		sbyte b2 = sbyte.MinValue;
		GameData.Domains.CombatSkill.CombatSkill combatSkill = null;
		sbyte b3 = sbyte.MaxValue;
		sbyte b4 = sbyte.MinValue;
		CombatSkillItem combatSkillItem = null;
		bool flag = random.CheckPercentProb(GlobalConfig.Instance.ShopManagerLearnRandomGradeChance);
		sbyte idealSectId = character.GetIdealSect();
		sbyte stateSectId = DomainManager.Taiwu.GetTaiwuVillageStateSect();
		sbyte behaviorType = character.GetBehaviorType();
		foreach (CombatSkillItem item3 in (IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance)
		{
			if (item3.Type != combatSkillType || item3.BookId < 0 || item3.Grade > maxGrade || item3.SectId < 0)
			{
				continue;
			}
			sbyte b5 = GetSectPriority(item3.SectId);
			if (charCombatSkills.TryGetValue(item3.TemplateId, out var value))
			{
				if (value.GetActivationState() != 0)
				{
					continue;
				}
				ushort readingState = value.GetReadingState();
				if (CombatSkillStateHelper.HasReadOutlinePages(readingState) && CombatSkillStateHelper.IsReadNormalPagesMeetConditionOfBreakout(readingState))
				{
					if (b > item3.Grade)
					{
						b2 = GetSectPriority(item3.SectId);
						b = item3.Grade;
						combatSkill = value;
					}
					else if (b == item3.Grade && b5 > b2)
					{
						b2 = b5;
						combatSkill = value;
					}
				}
			}
			else if (b3 > item3.Grade)
			{
				b3 = item3.Grade;
				b4 = b5;
				combatSkillItem = item3;
			}
			else if (b3 == item3.Grade && b5 > b4)
			{
				b4 = b5;
				combatSkillItem = item3;
			}
		}
		if (combatSkill != null)
		{
			ushort readingState2 = combatSkill.GetReadingState();
			if (!CombatSkillStateHelper.HasReadOutlinePages(readingState2))
			{
				byte item = (byte)random.Next(5);
				return (skillTemplateId: combatSkill.GetId().SkillTemplateId, pageInternalIndex: item);
			}
			Span<byte> span = stackalloc byte[15];
			SpanList<byte> spanList = span;
			for (int i = 0; i < 5; i++)
			{
				byte b6 = (byte)(5 + i);
				if ((readingState2 & (1 << (int)b6)) == 0)
				{
					spanList.Add(b6);
				}
				byte b7 = (byte)(b6 + 5);
				if ((readingState2 & (1 << (int)b7)) == 0)
				{
					spanList.Add(b7);
				}
				spanList.Add((byte)(i + 1));
			}
			byte random2 = spanList.GetRandom(random);
			return (skillTemplateId: combatSkill.GetId().SkillTemplateId, pageInternalIndex: random2);
		}
		if (combatSkillItem != null)
		{
			short templateId = combatSkillItem.TemplateId;
			byte item2 = (byte)random.Next(5);
			return (skillTemplateId: templateId, pageInternalIndex: item2);
		}
		return (skillTemplateId: -1, pageInternalIndex: 0);
		sbyte GetSectPriority(sbyte sectId)
		{
			if (sectId == stateSectId)
			{
				return 3;
			}
			if (sectId == idealSectId)
			{
				return 2;
			}
			short mainMorality = Config.Organization.Instance[sectId].MainMorality;
			if (mainMorality >= -500 && mainMorality <= 500 && GameData.Domains.Character.BehaviorType.GetBehaviorType(mainMorality) == behaviorType)
			{
				return 1;
			}
			return 0;
		}
	}

	[Obsolete]
	private (short skillTemplateId, byte pageId) ShopManagerSelectLifeSkillToLearn(IRandomSource random, GameData.Domains.Character.Character character, sbyte lifeSkillType, sbyte maxGrade)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = character.GetLearnedLifeSkills();
		sbyte b = sbyte.MaxValue;
		int num = -1;
		bool flag = random.CheckPercentProb(GlobalConfig.Instance.ShopManagerLearnRandomGradeChance);
		List<int> list = null;
		if (flag)
		{
			list = ObjectPool<List<int>>.Instance.Get();
			list.Clear();
		}
		BoolArray16 val = default(BoolArray16);
		int i = 0;
		for (int count = learnedLifeSkills.Count; i < count; i++)
		{
			GameData.Domains.Character.LifeSkillItem lifeSkillItem = learnedLifeSkills[i];
			Config.LifeSkillItem lifeSkillItem2 = LifeSkill.Instance[lifeSkillItem.SkillTemplateId];
			if (lifeSkillItem2.Type != lifeSkillType || lifeSkillItem2.Grade > maxGrade)
			{
				continue;
			}
			((BoolArray16)(ref val)).Set((int)lifeSkillItem2.Grade, true);
			if (!lifeSkillItem.IsAllPagesRead())
			{
				if (list != null)
				{
					list.Add(i);
				}
				else if (b > lifeSkillItem2.Grade)
				{
					b = lifeSkillItem2.Grade;
					num = i;
				}
			}
		}
		if (list != null)
		{
			if (list.Count > 0)
			{
				num = list.GetRandom(random);
			}
			ObjectPool<List<int>>.Instance.Return(list);
		}
		if (num >= 0)
		{
			GameData.Domains.Character.LifeSkillItem lifeSkillItem3 = learnedLifeSkills[num];
			Span<byte> span = stackalloc byte[5];
			SpanList<byte> spanList = span;
			for (byte b2 = 0; b2 < 5; b2++)
			{
				if (!lifeSkillItem3.IsPageRead(b2))
				{
					spanList.Add(b2);
				}
			}
			if (spanList.Count > 0)
			{
				return (skillTemplateId: lifeSkillItem3.SkillTemplateId, pageId: spanList.GetRandom(random));
			}
		}
		else if (flag)
		{
			Span<sbyte> span2 = stackalloc sbyte[(int)maxGrade];
			SpanList<sbyte> spanList2 = span2;
			for (sbyte b3 = 0; b3 < maxGrade; b3++)
			{
				if (!((BoolArray16)(ref val)).Get((int)b3))
				{
					spanList2.Add(b3);
				}
			}
			if (spanList2.Count > 0)
			{
				sbyte random2 = spanList2.GetRandom(random);
				short item = Config.LifeSkillType.Instance[lifeSkillType].SkillList[random2];
				return (skillTemplateId: item, pageId: (byte)random.Next(5));
			}
		}
		else
		{
			for (sbyte b4 = 0; b4 < maxGrade; b4++)
			{
				if (!((BoolArray16)(ref val)).Get((int)b4))
				{
					short item2 = Config.LifeSkillType.Instance[lifeSkillType].SkillList[b4];
					return (skillTemplateId: item2, pageId: (byte)random.Next(5));
				}
			}
		}
		return (skillTemplateId: -1, pageId: 0);
	}

	private List<(short skillTemplateId, byte pageId)> ShopBuildingTeachLifeSkillBook(GameData.Domains.Character.Character leader, GameData.Domains.Character.Character member, int point, short buildingTemplateId, out bool hasMemberUnread)
	{
		List<(short, byte)> list = new List<(short, byte)>();
		BuildingBlockItem buildingConfig = BuildingBlock.Instance[buildingTemplateId];
		List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = leader.GetLearnedLifeSkills();
		List<GameData.Domains.Character.LifeSkillItem> list2 = learnedLifeSkills.Where(delegate(GameData.Domains.Character.LifeSkillItem a)
		{
			Config.LifeSkillItem lifeSkillItem3 = LifeSkill.Instance[a.SkillTemplateId];
			return lifeSkillItem3.Type == buildingConfig.RequireLifeSkillType;
		}).ToList();
		list2.Sort(delegate(GameData.Domains.Character.LifeSkillItem a, GameData.Domains.Character.LifeSkillItem lifeSkillItem3)
		{
			sbyte grade = LifeSkill.Instance[a.SkillTemplateId].Grade;
			sbyte grade2 = LifeSkill.Instance[lifeSkillItem3.SkillTemplateId].Grade;
			return grade.CompareTo(grade2);
		});
		List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills2 = member.GetLearnedLifeSkills();
		List<short> list3 = learnedLifeSkills2.Select((GameData.Domains.Character.LifeSkillItem a) => a.SkillTemplateId).ToList();
		List<byte> list4 = new List<byte>();
		hasMemberUnread = false;
		for (int num = 0; num < list2.Count; num++)
		{
			GameData.Domains.Character.LifeSkillItem skillItem = list2[num];
			Config.LifeSkillItem lifeSkillItem = LifeSkill.Instance[skillItem.SkillTemplateId];
			int readingAttainmentRequirement = SkillGradeData.Instance[lifeSkillItem.Grade].ReadingAttainmentRequirement;
			list4.Clear();
			if (!list3.Contains(skillItem.SkillTemplateId))
			{
				for (byte b = 0; b < 5; b++)
				{
					if (skillItem.IsPageRead(b))
					{
						list4.Add(b);
					}
				}
			}
			else
			{
				GameData.Domains.Character.LifeSkillItem lifeSkillItem2 = learnedLifeSkills2.Find((GameData.Domains.Character.LifeSkillItem a) => a.SkillTemplateId == skillItem.SkillTemplateId);
				for (byte b2 = 0; b2 < 5; b2++)
				{
					if (skillItem.IsPageRead(b2) && !lifeSkillItem2.IsPageRead(b2))
					{
						list4.Add(b2);
					}
				}
			}
			hasMemberUnread |= list4.Count > 0;
			for (int num2 = 0; num2 < list4.Count; num2++)
			{
				point -= readingAttainmentRequirement;
				if (point >= 0)
				{
					list.Add((skillItem.SkillTemplateId, list4[num2]));
					continue;
				}
				break;
			}
		}
		return list;
	}

	private List<(short skillTemplateId, byte pageInternalIndex, sbyte direct)> ShopBuildingTeachCombatSkillBook(GameData.Domains.Character.Character leader, GameData.Domains.Character.Character member, int point, short buildingTemplateId, out bool hasMemberUnread)
	{
		List<(short, byte, sbyte)> list = new List<(short, byte, sbyte)>();
		BuildingBlockItem buildingConfig = BuildingBlock.Instance[buildingTemplateId];
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(leader.GetId());
		List<short> source = charCombatSkills.Keys.ToList();
		List<short> list2 = source.Where(delegate(short combatSkillTemplateId)
		{
			CombatSkillItem combatSkillItem2 = Config.CombatSkill.Instance[combatSkillTemplateId];
			return combatSkillItem2.Type == buildingConfig.RequireCombatSkillType;
		}).ToList();
		list2.Sort(delegate(short a, short index)
		{
			sbyte grade = Config.CombatSkill.Instance[a].Grade;
			sbyte grade2 = Config.CombatSkill.Instance[index].Grade;
			return grade.CompareTo(grade2);
		});
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills2 = DomainManager.CombatSkill.GetCharCombatSkills(member.GetId());
		List<short> list3 = charCombatSkills2.Keys.ToList();
		List<(byte, sbyte)> list4 = new List<(byte, sbyte)>();
		hasMemberUnread = false;
		for (int num = 0; num < list2.Count; num++)
		{
			short num2 = list2[num];
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[num2];
			int readingAttainmentRequirement = SkillGradeData.Instance[combatSkillItem.Grade].ReadingAttainmentRequirement;
			list4.Clear();
			GameData.Domains.CombatSkill.CombatSkill combatSkill = charCombatSkills[num2];
			ushort readingState = combatSkill.GetReadingState();
			if (!list3.Contains(num2))
			{
				for (int num3 = 0; num3 < 5; num3++)
				{
					byte b = (byte)num3;
					if ((readingState & (1 << (int)b)) != 0)
					{
						list4.Add((b, -1));
					}
					byte b2 = (byte)(5 + num3);
					if ((readingState & (1 << (int)b2)) != 0)
					{
						list4.Add((b2, 0));
					}
					byte b3 = (byte)(b2 + 5);
					if ((readingState & (1 << (int)b3)) != 0)
					{
						list4.Add((b3, 1));
					}
				}
			}
			else
			{
				GameData.Domains.CombatSkill.CombatSkill combatSkill2 = charCombatSkills2[num2];
				ushort readingState2 = combatSkill2.GetReadingState();
				for (int num4 = 0; num4 < 5; num4++)
				{
					byte b4 = (byte)num4;
					if ((readingState & (1 << (int)b4)) != 0 && (readingState2 & (1 << (int)b4)) == 0)
					{
						list4.Add((b4, -1));
					}
					byte b5 = (byte)(5 + num4);
					if ((readingState & (1 << (int)b5)) != 0 && (readingState2 & (1 << (int)b5)) == 0)
					{
						list4.Add((b5, 0));
					}
					byte b6 = (byte)(b5 + 5);
					if ((readingState & (1 << (int)b6)) != 0 && (readingState2 & (1 << (int)b6)) == 0)
					{
						list4.Add((b6, 1));
					}
				}
			}
			hasMemberUnread |= list4.Count > 0;
			for (int num5 = 0; num5 < list4.Count; num5++)
			{
				point -= readingAttainmentRequirement;
				if (point >= 0)
				{
					list.Add((num2, list4[num5].Item1, list4[num5].Item2));
					continue;
				}
				break;
			}
		}
		return list;
	}

	private BuildingBlockKey GetBuildingInRange(BuildingBlockKey blockKey, sbyte width, int range, short targetBuildingTemplateId)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		BuildingAreaData element_BuildingAreas = GetElement_BuildingAreas(new Location(blockKey.AreaId, blockKey.BlockId));
		element_BuildingAreas.GetNeighborBlocks(blockKey.BuildingBlockIndex, width, list, null, range);
		BuildingBlockKey result = BuildingBlockKey.Invalid;
		foreach (short item in list)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, item);
			if (TryGetElement_BuildingBlocks(buildingBlockKey, out var value))
			{
				if (value.RootBlockIndex >= 0)
				{
					buildingBlockKey.BuildingBlockIndex = value.RootBlockIndex;
					value = GetBuildingBlockData(buildingBlockKey);
				}
				if (value.TemplateId == targetBuildingTemplateId)
				{
					result = buildingBlockKey;
					break;
				}
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
		return result;
	}

	private void GetInfluencedBuildingBlocks(BuildingBlockKey blockKey, List<BuildingBlockData> influencedBlocks)
	{
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(blockKey);
		BuildingBlockItem configData = element_BuildingBlocks.ConfigData;
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		BuildingAreaData element_BuildingAreas = GetElement_BuildingAreas(new Location(blockKey.AreaId, blockKey.BlockId));
		element_BuildingAreas.GetNeighborBlocks(blockKey.BuildingBlockIndex, configData.Width, list, null, 2);
		foreach (short item in list)
		{
			BuildingBlockKey elementId = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, item);
			if (TryGetElement_BuildingBlocks(elementId, out var value) && element_BuildingBlocks.CanInfluenceBuildingBlock(value))
			{
				influencedBlocks.Add(element_BuildingBlocks);
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
	}

	private void RemoveAllOperatorsInBuilding(DataContext context, BuildingBlockKey blockKey)
	{
		if (!_buildingOperatorDict.ContainsKey(blockKey))
		{
			return;
		}
		List<int> collection = _buildingOperatorDict[blockKey].GetCollection();
		for (int i = 0; i < 3; i++)
		{
			if (collection[i] >= 0)
			{
				DomainManager.Taiwu.RemoveVillagerWork(context, collection[i]);
			}
		}
	}

	private void RemoveAllManagersInBuilding(DataContext context, BuildingBlockKey blockKey)
	{
		if (!_shopManagerDict.ContainsKey(blockKey))
		{
			return;
		}
		List<int> collection = _shopManagerDict[blockKey].GetCollection();
		for (int i = 0; i < 7; i++)
		{
			if (collection[i] >= 0)
			{
				DomainManager.Taiwu.RemoveVillagerWork(context, collection[i]);
			}
		}
	}

	public void UpdateTaiwuBuildingAutoOperation(DataContext context)
	{
		foreach (KeyValuePair<BuildingBlockKey, BuildingBlockData> buildingBlock in _buildingBlocks)
		{
			BuildingBlockKey key = buildingBlock.Key;
			BuildingBlockData value = buildingBlock.Value;
			if (value.RootBlockIndex >= 0)
			{
				continue;
			}
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[value.TemplateId];
			if (buildingBlockItem.Type != EBuildingBlockType.Building && !BuildingBlockData.IsResource(buildingBlockItem.Type) && buildingBlockItem.TemplateId != 44)
			{
				continue;
			}
			ParallelBuildingModification parallelBuildingModification = new ParallelBuildingModification
			{
				BlockKey = key,
				BlockData = value
			};
			if (value.CanUse() && buildingBlockItem.IsShop)
			{
				Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
				if (taiwuVillageLocation.AreaId == parallelBuildingModification.BlockKey.AreaId && taiwuVillageLocation.BlockId == parallelBuildingModification.BlockKey.BlockId)
				{
					TaiwuVillageAutoArrangeShopManager(context, parallelBuildingModification);
					TaiwuVillageAutoAddShopSoldItem(context, parallelBuildingModification);
				}
			}
		}
	}

	private void TaiwuVillageAutoArrangeShopManager(DataContext context, ParallelBuildingModification modification)
	{
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[modification.BlockData.TemplateId];
		if (!buildingBlockItem.IsShop)
		{
			return;
		}
		List<short> autoWorkBlockIndexList = DomainManager.Extra.GetAutoWorkBlockIndexList();
		if (!modification.FreeShopManager && autoWorkBlockIndexList.Contains(modification.BlockKey.BuildingBlockIndex))
		{
			TryGetElement_ShopManagerDict(modification.BlockKey, out var value);
			if (value.GetCount() != 0)
			{
				QuickArrangeShopManager(context, modification.BlockKey);
			}
		}
	}

	private void TaiwuVillageAutoAddShopSoldItem(DataContext context, ParallelBuildingModification modification)
	{
		List<short> autoSoldBlockIndexList = DomainManager.Extra.GetAutoSoldBlockIndexList();
		if (!modification.FreeShopManager && autoSoldBlockIndexList.Contains(modification.BlockKey.BuildingBlockIndex))
		{
			AutoAddShopSoldItem(context, modification.BlockKey);
		}
	}

	private void AutoCheckInResidence(DataContext context, BuildingBlockKey blockKey)
	{
		List<short> autoCheckInResidenceList = DomainManager.Extra.GetAutoCheckInResidenceList();
		if (autoCheckInResidenceList.Contains(blockKey.BuildingBlockIndex))
		{
			QuickFillResidence(context, blockKey);
		}
	}

	private void AutoCheckInComfortable(DataContext context, BuildingBlockKey blockKey)
	{
		List<short> autoCheckInComfortableList = DomainManager.Extra.GetAutoCheckInComfortableList();
		if (autoCheckInComfortableList.Contains(blockKey.BuildingBlockIndex) && !DomainManager.Extra.GetFeast(blockKey).CheckAvoidAutoCheckIn())
		{
			QuickFillComfortableHouse(context, blockKey);
		}
	}

	public void ComplementUpdateBuilding(DataContext context, ParallelBuildingModification modification)
	{
		InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		bool flag = modification.BlockKey.AreaId == taiwuVillageLocation.AreaId && modification.BlockKey.BlockId == taiwuVillageLocation.BlockId;
		if (modification.RemoveFromAutoExpand)
		{
			List<short> autoExpandBlockIndexList = DomainManager.Extra.GetAutoExpandBlockIndexList();
			autoExpandBlockIndexList.Remove(modification.BlockData.BlockIndex);
			DomainManager.Extra.SetAutoExpandBlockIndexList(autoExpandBlockIndexList, context);
		}
		if (modification.ResetAllChildrenBlocks)
		{
			ResetAllChildrenBlocks(context, modification.BlockKey, 0, -1);
			SetBuildingCustomName(context, modification.BlockKey, null);
		}
		SetNewCompleteOperationBuildings(_newCompleteOperationBuildings, context);
		SetElement_BuildingBlocks(modification.BlockKey, modification.BlockData, context);
		if (modification.FreeOperator)
		{
			RemoveAllOperatorsInBuilding(context, modification.BlockKey);
		}
		if (modification.AddBuilding)
		{
			if (modification.BlockData.TemplateId == 46)
			{
				AddResidence(context, modification.BlockKey);
			}
			else if (modification.BlockData.TemplateId == 47)
			{
				AddComfortableHouse(context, modification.BlockKey);
			}
		}
		if (modification.RemoveResidence)
		{
			if (_residences.ContainsKey(modification.BlockKey))
			{
				RemoveResidence(context, modification.BlockKey);
			}
			else if (_comfortableHouses.ContainsKey(modification.BlockKey))
			{
				RemoveComfortableHouse(context, modification.BlockKey);
			}
		}
		if (flag)
		{
			AutoCheckInComfortable(context, modification.BlockKey);
			AutoCheckInResidence(context, modification.BlockKey);
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		taiwu.ChangeResources(context, ref modification.DeltaResources);
		if (TryGetElement_CollectBuildingEarningsData(modification.BlockKey, out var value))
		{
			if (modification.BlockData.TemplateId == 222 && value.CollectionItemList != null)
			{
				short num = SharedMethods.CalcPawnshopKeepItemTimeIndex();
				for (int i = 0; i < value.CollectionItemList.Count; i++)
				{
					if (value.CollectionItemList[i].ModificationState >= GlobalConfig.BuildingPawnshopKeepItemTime[num])
					{
						value.CollectionItemList.RemoveAt(i);
					}
					else
					{
						value.CollectionItemList[i] = new ItemKey(value.CollectionItemList[i].ItemType, (byte)(value.CollectionItemList[i].ModificationState + 1), value.CollectionItemList[i].TemplateId, value.CollectionItemList[i].Id);
					}
				}
			}
			if (value.RecruitLevelList != null)
			{
				for (int j = 0; j < value.RecruitLevelList.Count; j++)
				{
					if (value.RecruitLevelList[j].Second >= 3)
					{
						OfflineHandleRecruitPeopleLeave(context, modification.BlockKey, j);
						instantNotificationCollection.AddCandidateLeaved(taiwuVillageSettlementId, modification.BlockData.TemplateId);
					}
					else
					{
						value.RecruitLevelList[j] = new IntPair(value.RecruitLevelList[j].First, value.RecruitLevelList[j].Second + 1);
					}
				}
			}
			SetElement_CollectBuildingEarningsData(modification.BlockKey, value, context);
		}
		if (modification.CollectableEarnings != null || modification.CollectableResources != null || modification.ShopSoldItems != null || modification.RecruitLevelList != null)
		{
			if (!TryGetElement_CollectBuildingEarningsData(modification.BlockKey, out var value2))
			{
				value2 = new BuildingEarningsData();
				AddElement_CollectBuildingEarningsData(modification.BlockKey, value2, context);
			}
			if (modification.CollectableEarnings != null)
			{
				foreach (TemplateKey collectableEarning in modification.CollectableEarnings)
				{
					ItemKey itemKey = DomainManager.Item.CreateItem(context, collectableEarning.ItemType, collectableEarning.TemplateId);
					DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, modification.BlockData.TemplateId);
					value2.CollectionItemList.Add(itemKey);
				}
			}
			if (modification.CollectableResources != null)
			{
				foreach (IntPair collectableResource in modification.CollectableResources)
				{
					value2.CollectionResourceList.Add(new IntPair(collectableResource.First, collectableResource.Second));
				}
			}
			if (modification.ShopBuildingSalaryList != null)
			{
				ShopEventCollection orCreateShopEventCollection = GetOrCreateShopEventCollection(modification.BlockKey);
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				int currDate = DomainManager.World.GetCurrDate();
				foreach (var (num2, resourceType, num3) in modification.ShopBuildingSalaryList)
				{
					if (DomainManager.Character.TryGetElement_Objects(num2, out var element))
					{
						element.ChangeResource(context, resourceType, num3);
						lifeRecordCollection.AddTaiwuVillagerSalaryReceived(num2, currDate, modification.BlockData.TemplateId, num3, resourceType);
						orCreateShopEventCollection.AddSalaryReceived(currDate, num2, num3, resourceType);
					}
				}
			}
			if (modification.RecruitLevelList != null)
			{
				value2.RecruitLevelList.AddRange(modification.RecruitLevelList);
			}
			if (modification.ShopSoldItems != null)
			{
				for (int k = 0; k < modification.ShopSoldItems.Count; k++)
				{
					if (modification.ShopSoldItems[k].index == -2)
					{
						int num4 = SharedMethods.GetBuildingSlotCount(modification.BlockData.TemplateId) - value2.ShopSoldItemList.Count;
						for (int l = 0; l < num4; l++)
						{
							value2.ShopSoldItemList.Add(ItemKey.Invalid);
							value2.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
						}
						continue;
					}
					sbyte item = modification.ShopSoldItems[k].index;
					if (item < value2.ShopSoldItemList.Count && item >= 0)
					{
						DomainManager.Item.RemoveItem(context, value2.ShopSoldItemList[item]);
						value2.ShopSoldItemList[item] = ItemKey.Invalid;
						value2.ShopSoldItemEarnList[item] = new IntPair(modification.ShopSoldItems[k].exchangeResource.First, modification.ShopSoldItems[k].exchangeResource.Second);
					}
				}
			}
			SetElement_CollectBuildingEarningsData(modification.BlockKey, value2, context);
		}
		if (modification.LearnCombatSkills != null)
		{
			foreach (var learnCombatSkill in modification.LearnCombatSkills)
			{
				if (DomainManager.Character.TryGetElement_Objects(learnCombatSkill.charId, out var element2))
				{
					CombatSkillKey objectId = new CombatSkillKey(learnCombatSkill.charId, learnCombatSkill.skillTemplateId);
					if (DomainManager.CombatSkill.TryGetElement_CombatSkills(objectId, out var element3))
					{
						ushort readingState = element3.GetReadingState();
						readingState = CombatSkillStateHelper.SetPageRead(readingState, learnCombatSkill.pageInternalIndex);
						element3.SetReadingState(readingState, context);
					}
					else
					{
						ushort readingState2 = CombatSkillStateHelper.SetPageRead(0, learnCombatSkill.pageInternalIndex);
						element2.LearnNewCombatSkill(context, learnCombatSkill.skillTemplateId, readingState2);
					}
					DomainManager.CombatSkill.TryActivateCombatSkillBookPageWhenSetReadingState(context, element2.GetId(), learnCombatSkill.skillTemplateId, learnCombatSkill.pageInternalIndex);
				}
			}
		}
		if (modification.LearnLifeSkills != null)
		{
			foreach (var learnLifeSkill in modification.LearnLifeSkills)
			{
				if (DomainManager.Character.TryGetElement_Objects(learnLifeSkill.charId, out var element4))
				{
					int num5 = element4.FindLearnedLifeSkillIndex(learnLifeSkill.skillTemplateId);
					if (num5 >= 0)
					{
						element4.ReadLifeSkillPage(context, num5, learnLifeSkill.pageId);
					}
					else
					{
						element4.LearnNewLifeSkill(context, learnLifeSkill.skillTemplateId, (byte)(1 << (int)learnLifeSkill.pageId));
					}
				}
			}
		}
		if (modification.FixBookList != null && modification.FixBookList.Count > 0)
		{
			GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(modification.FixBookList[0].Id);
			if (!element_SkillBooks.CanFix())
			{
				return;
			}
			sbyte item2 = element_SkillBooks.GetFixProgress().pageNum;
			ushort pageIncompleteState = element_SkillBooks.GetPageIncompleteState();
			pageIncompleteState = SkillBookStateHelper.SetPageIncompleteState(pageIncompleteState, (byte)item2, 0);
			element_SkillBooks.SetPageIncompleteState(pageIncompleteState, DataContextManager.GetCurrentThreadDataContext());
			if (!element_SkillBooks.CanFix())
			{
				DomainManager.World.GetInstantNotificationCollection().AddBookRepairSuccess(value.FixBookInfoList[0].ItemType, value.FixBookInfoList[0].TemplateId);
			}
		}
		if (modification.FreeShopManager)
		{
			RemoveAllManagersInBuilding(context, modification.BlockKey);
			ClearBuildingBlockEarningsData(context, modification.BlockKey, modification.BlockData.TemplateId == 222);
		}
		if (modification.RemoveMakeItemData && TryGetElement_MakeItemDict(modification.BlockKey, out var _))
		{
			RemoveElement_MakeItemDict(modification.BlockKey, context);
		}
		if (modification.RemoveEventBookData && _shopEventCollections != null && _shopEventCollections.ContainsKey(modification.BlockKey))
		{
			_shopEventCollections.Remove(modification.BlockKey);
		}
		if (modification.RemoveCollectResourceType)
		{
			RemoveElement_CollectBuildingResourceType(modification.BlockKey, context);
		}
		if (modification.AddBuilding)
		{
			DomainManager.Taiwu.AddLegacyPoint(context, 26);
			if (DomainManager.Extra.IsExtraTaskInProgress(22))
			{
				DomainManager.Extra.FinishTriggeredExtraTask(context, 14, 22);
			}
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[modification.BlockData.TemplateId];
			if (BuildingBlockData.IsUsefulResource(buildingBlockItem.Type))
			{
				List<short> legaciesBuildingTemplateIdList = DomainManager.Extra.GetLegaciesBuildingTemplateIdList();
				legaciesBuildingTemplateIdList.Remove(modification.BlockData.TemplateId);
				DomainManager.Extra.SetLegaciesBuildingTemplateIdList(legaciesBuildingTemplateIdList, context);
			}
		}
		if (modification.BuildingMoneyPrestigeSuccessRateCompensationChanged != null)
		{
			DomainManager.Extra.UpdateBuildingMoneyPrestigeSuccessRateCompensation(context, modification.BuildingMoneyPrestigeSuccessRateCompensationChanged);
			modification.BuildingMoneyPrestigeSuccessRateCompensationChanged.Clear();
		}
		if (modification.BuildingOperationComplete)
		{
			UpdateTaiwuVillageBuildingEffect();
		}
	}

	public void AddBuildingException(BuildingBlockKey buildingBlockKey, BuildingBlockData buildingBlockData, BuildingExceptionType buildingExceptionType)
	{
		if (!_buildingExceptionData.BuildingExceptionDict.TryGetValue(buildingBlockKey, out var value))
		{
			value = new BuildingExceptionItem();
			_buildingExceptionData.BuildingExceptionDict[buildingBlockKey] = value;
		}
		if (!value.ExceptionTypeList.Contains((sbyte)buildingExceptionType))
		{
			value.ExceptionTypeList.Add((sbyte)buildingExceptionType);
		}
	}

	private void ClearBuildingException()
	{
		_buildingExceptionData.BuildingExceptionDict.Clear();
	}

	[DomainMethod]
	public BuildingExceptionData GetBuildingExceptionData()
	{
		return _buildingExceptionData;
	}

	public int GetBuildingBlockEffect(Location location, EBuildingScaleEffect effectType, int subType = -1)
	{
		IBuildingEffectValue buildingBlockEffectObject = GetBuildingBlockEffectObject(location, effectType);
		if (buildingBlockEffectObject == null)
		{
			return 0;
		}
		return (subType < 0) ? buildingBlockEffectObject.Get() : buildingBlockEffectObject.Get(subType);
	}

	public int GetBuildingBlockEffect(short settlementId, EBuildingScaleEffect effectType, int subType = -1)
	{
		IBuildingEffectValue buildingBlockEffectObject = GetBuildingBlockEffectObject(settlementId, effectType);
		if (buildingBlockEffectObject == null)
		{
			return 0;
		}
		return (subType < 0) ? buildingBlockEffectObject.Get() : buildingBlockEffectObject.Get(subType);
	}

	public IBuildingEffectValue GetBuildingBlockEffectObject(Location location, EBuildingScaleEffect effectType)
	{
		if (DomainManager.World.GetMainStoryLineProgress() < 1)
		{
			return null;
		}
		if (!location.IsValid())
		{
			return null;
		}
		MapBlockData belongSettlementBlock = DomainManager.Map.GetBelongSettlementBlock(location);
		if (belongSettlementBlock == null)
		{
			return null;
		}
		Location key = new Location(belongSettlementBlock.AreaId, belongSettlementBlock.BlockId);
		IBuildingEffectValue[] value;
		return _buildingBlockEffectsCache.TryGetValue(key, out value) ? value[(int)effectType] : null;
	}

	public IBuildingEffectValue GetBuildingBlockEffectObject(short settlementId, EBuildingScaleEffect effectType)
	{
		if (DomainManager.World.GetMainStoryLineProgress() < 1)
		{
			return null;
		}
		if (settlementId < 0)
		{
			return null;
		}
		Location location = DomainManager.Organization.GetSettlement(settlementId).GetLocation();
		IBuildingEffectValue[] value;
		return _buildingBlockEffectsCache.TryGetValue(location, out value) ? value[(int)effectType] : null;
	}

	private void InitAllAreaBuildingBlockEffectsCache()
	{
		_buildingBlockEffectsCache.Clear();
		foreach (var (location2, buildingAreaData2) in _buildingAreas)
		{
			if (MapAreaData.IsRegularArea(location2.AreaId))
			{
				UpdateLocationBuildingBlockEffectsCache(location2);
			}
		}
	}

	private void UpdateLocationBuildingBlockEffectsCache(Location location)
	{
		Span<int> values = stackalloc int[5];
		IBuildingEffectValue[] array = _buildingBlockEffectsCache.GetOrDefault(location);
		if (array == null)
		{
			array = new IBuildingEffectValue[33];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitBonus((EBuildingScaleEffect)i);
			}
			_buildingBlockEffectsCache.Add(location, array);
		}
		else
		{
			for (int j = 0; j < array.Length; j++)
			{
				array[j].Clear();
			}
		}
		for (short num = 1; num < 21; num++)
		{
			CalcResourceBlockEffectBaseValues(location, num, ref values);
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[num];
			List<short> expandInfos = buildingBlockItem.ExpandInfos;
			if (expandInfos != null && expandInfos.Count > 0)
			{
				foreach (short expandInfo in buildingBlockItem.ExpandInfos)
				{
					BuildingScaleItem buildingScaleItem = BuildingScale.Instance[expandInfo];
					if (buildingScaleItem.Effect != EBuildingScaleEffect.Invalid && buildingScaleItem.Formula >= 0)
					{
						int delta = CalcResourceBlockTotalEffectValue(buildingScaleItem.Formula, values);
						array[(int)buildingScaleItem.Effect].Change(delta);
					}
				}
			}
		}
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(location);
		bool flag = DomainManager.Taiwu.GetTaiwuVillageLocation() == location;
		for (short num2 = 0; num2 < element_BuildingAreas.Width * element_BuildingAreas.Width; num2++)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(location.AreaId, location.BlockId, num2);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
			if (element_BuildingBlocks.TemplateId >= 0)
			{
				BuildingBlockItem configData = element_BuildingBlocks.ConfigData;
				if (configData.Class != EBuildingBlockClass.BornResource)
				{
					List<short> expandInfos = configData.ExpandInfos;
					if (expandInfos != null && expandInfos.Count > 0 && !(!HasShopManagerLeader(buildingBlockKey) && flag) && element_BuildingBlocks.CanUse() && AllDependBuildingAvailable(buildingBlockKey, element_BuildingBlocks.TemplateId, out var _))
					{
						_formulaContextBridge.Initialize(buildingBlockKey, configData, _formulaArgHandler);
						foreach (short expandInfo2 in configData.ExpandInfos)
						{
							BuildingScaleItem buildingScaleItem2 = BuildingScale.Instance[expandInfo2];
							if (buildingScaleItem2.Effect != EBuildingScaleEffect.Invalid)
							{
								ApplyScaleEffect(buildingBlockKey, buildingScaleItem2, array[(int)buildingScaleItem2.Effect]);
							}
						}
					}
				}
			}
		}
	}

	private static int CalcBuildingFormulaContextArg(BuildingBlockKey blockKey, EBuildingFormulaArgType argType)
	{
		if (1 == 0)
		{
		}
		bool hasManager;
		int result = argType switch
		{
			EBuildingFormulaArgType.MaxAttainment => DomainManager.Building.BuildingMaxAttainment(blockKey, -1, out hasManager), 
			EBuildingFormulaArgType.TotalAttainment => DomainManager.Building.BuildingTotalAttainment(blockKey, -1, out hasManager), 
			EBuildingFormulaArgType.LeaderFameType => DomainManager.Building.BuildLeaderFameType(blockKey), 
			_ => throw new ArgumentOutOfRangeException("argType", argType, null), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private IBuildingEffectValue InitBonus(EBuildingScaleEffect effect)
	{
		if (1 == 0)
		{
		}
		BuildingEffectValue result;
		switch (effect)
		{
		case EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor:
		case EBuildingScaleEffect.BreakOutSuccessRate:
		case EBuildingScaleEffect.CombatSkillAttainment:
			result = new BuildingEffectGroupValue(14);
			break;
		case EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor:
		case EBuildingScaleEffect.ShopManagerQualificationImproveRate:
		case EBuildingScaleEffect.MakeItemAttainmentRequirementReduction:
		case EBuildingScaleEffect.LifeSkillAttainment:
			result = new BuildingEffectGroupValue(16);
			break;
		default:
			result = new BuildingEffectValue();
			break;
		}
		if (1 == 0)
		{
		}
		return result;
	}

	private void ApplyScaleEffect(BuildingBlockKey blockKey, BuildingScaleItem scaleCfg, IBuildingEffectValue value)
	{
		int delta = 0;
		List<int> levelEffect = scaleCfg.LevelEffect;
		if (levelEffect != null && levelEffect.Count > 0)
		{
			sbyte level = BuildingBlockLevel(blockKey);
			delta = scaleCfg.GetLevelEffect(level);
		}
		else if (scaleCfg.Formula >= 0)
		{
			BuildingFormulaItem formula = BuildingFormula.Instance[scaleCfg.Formula];
			delta = formula.Calculate(_formulaContextBridge);
		}
		switch (scaleCfg.Effect)
		{
		case EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor:
		case EBuildingScaleEffect.BreakOutSuccessRate:
		case EBuildingScaleEffect.CombatSkillAttainment:
			value.Change(scaleCfg.CombatSkillType, delta);
			break;
		case EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor:
		case EBuildingScaleEffect.ShopManagerQualificationImproveRate:
		case EBuildingScaleEffect.MakeItemAttainmentRequirementReduction:
		case EBuildingScaleEffect.LifeSkillAttainment:
			value.Change(scaleCfg.LifeSkillType, delta);
			break;
		default:
			value.Change(delta);
			break;
		}
	}

	public IReadOnlyList<int> GetSettlementChickenIdList(int settlementId)
	{
		IReadOnlyList<int> result;
		if (!_settlementChickenIdLists.TryGetValue(settlementId, out var value))
		{
			IReadOnlyList<int> readOnlyList = Array.Empty<int>();
			result = readOnlyList;
		}
		else
		{
			IReadOnlyList<int> readOnlyList = value;
			result = readOnlyList;
		}
		return result;
	}

	private void AddChickenInSettlementChickenIdLists(int chickenId, int settlementId)
	{
		if (!_settlementChickenIdLists.TryGetValue(settlementId, out var value))
		{
			value = new List<int>();
			_settlementChickenIdLists.Add(settlementId, value);
		}
		value.Add(chickenId);
	}

	private void RemoveChickenInSettlementChickenIdLists(int chickenId, int settlementId)
	{
		if (!_settlementChickenIdLists.TryGetValue(settlementId, out var value))
		{
			PredefinedLog.Instance[(short)30].Log(string.Join(", ", _settlementChickenIdLists.Select((KeyValuePair<int, List<int>> x) => $"{x.Key}: [{string.Join(", ", x.Value.Select((int num) => num.ToString()))}]")), string.Join(", ", _chicken.Select((KeyValuePair<int, Chicken> x) => $"{x.Value.CurrentSettlementId}: {x.Key}({x.Value.TemplateId})")), new StackTrace().ToString(), settlementId);
		}
		else if (!value.Remove(chickenId))
		{
			PredefinedLog.Instance[(short)31].Log(string.Join(", ", _settlementChickenIdLists.Select((KeyValuePair<int, List<int>> x) => $"{x.Key}: [{string.Join(", ", x.Value.Select((int num) => num.ToString()))}]")), string.Join(", ", _chicken.Select((KeyValuePair<int, Chicken> x) => $"{x.Value.CurrentSettlementId}: {x.Key}({x.Value.TemplateId})")), new StackTrace().ToString(), settlementId, chickenId);
		}
		else if (value.Count == 0)
		{
			_settlementChickenIdLists.Remove(settlementId);
		}
	}

	private void ClearSettlementChickenIdLists()
	{
		_settlementChickenIdLists.Clear();
	}

	private void TransferChickenInSettlementChickenIdLists(int chickenId, int fromSettlementId, int toSettlementId)
	{
		RemoveChickenInSettlementChickenIdLists(chickenId, fromSettlementId);
		AddChickenInSettlementChickenIdLists(chickenId, toSettlementId);
	}

	public void RefreshSettlementChickenIdLists()
	{
		ClearSettlementChickenIdLists();
		foreach (KeyValuePair<int, Chicken> item in _chicken)
		{
			AddChickenInSettlementChickenIdLists(item.Key, item.Value.CurrentSettlementId);
		}
	}

	[DomainMethod]
	public int AddChicken(DataContext context, int settlementId, short templateId)
	{
		int num = GenerateNextChickenId();
		Chicken value = new Chicken
		{
			Id = num,
			TemplateId = templateId,
			CurrentSettlementId = settlementId
		};
		AddElement_Chicken(num, value, context);
		AddChickenInSettlementChickenIdLists(num, settlementId);
		return num;
	}

	[DomainMethod]
	public void RemoveChicken(DataContext context, int id)
	{
		if (_chicken.TryGetValue(id, out var value))
		{
			RemoveChickenInSettlementChickenIdLists(id, value.CurrentSettlementId);
			RemoveElement_Chicken(id, context);
			return;
		}
		PredefinedLog.Instance[(short)32].Log(string.Join(", ", _settlementChickenIdLists.Select((KeyValuePair<int, List<int>> x) => $"{x.Key}: [{string.Join(", ", x.Value.Select((int num) => num.ToString()))}]")), string.Join(", ", _chicken.Select((KeyValuePair<int, Chicken> x) => $"{x.Value.CurrentSettlementId}: {x.Key}({x.Value.TemplateId})")), new StackTrace().ToString(), id);
	}

	[DomainMethod]
	public void RemoveAllChicken(DataContext context)
	{
		ClearSettlementChickenIdLists();
		ClearChicken(context);
	}

	[DomainMethod]
	public void MoveChicken(DataContext context, int id, int targetSettlementId)
	{
		if (!TryGetElement_Chicken(id, out var value))
		{
			return;
		}
		TransferChickenInSettlementChickenIdLists(id, value.CurrentSettlementId, targetSettlementId);
		value.CurrentSettlementId = targetSettlementId;
		SetElement_Chicken(id, value, context);
		if (GetSettlementChickenIdList(id).Count != 0)
		{
			return;
		}
		List<short> chickenMapInfo = ChickenMapInfo;
		if (chickenMapInfo != null && chickenMapInfo.Remove((short)value.CurrentSettlementId))
		{
			DomainManager.Extra.FinishTriggeredExtraTask(context, 53, 342);
			if (ChickenMapInfo.Count > 0)
			{
				DomainManager.Extra.TriggerExtraTask(context, 53, 342);
			}
		}
	}

	[DomainMethod]
	public void TransferChicken(DataContext context, int id, int targetSettlementId)
	{
		if (TryGetElement_Chicken(id, out var value))
		{
			value.Happiness = 100;
			SetElement_Chicken(id, value, context);
			MoveChicken(context, id, targetSettlementId);
		}
	}

	[DomainMethod]
	public List<int> GetLocationChicken(Location location)
	{
		int id = DomainManager.Organization.GetSettlementByLocation(location).GetId();
		return GetSettlementChickenList(id, ignoreFulong: false);
	}

	[DomainMethod]
	public bool AllChickenInTaiwuVillage(DataContext context)
	{
		return _settlementChickenIdLists.Count == 1;
	}

	[DomainMethod]
	public bool ClickChickenMap(DataContext context)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location validLocation = taiwu.GetValidLocation();
		short num = short.MaxValue;
		bool flag = DomainManager.Extra.IsExtraTaskInProgress(338);
		List<int> sectFulongLoseFeatherChickens = DomainManager.Extra.GetSectFulongLoseFeatherChickens();
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		if (ChickenMapInfo == null)
		{
			ChickenMapInfo = new List<short>();
		}
		Dictionary<int, List<short>> dictionary = new Dictionary<int, List<short>>();
		foreach (KeyValuePair<int, Chicken> item in _chicken)
		{
			if (flag && sectFulongLoseFeatherChickens != null && sectFulongLoseFeatherChickens.Contains(item.Key))
			{
				continue;
			}
			Settlement settlement = DomainManager.Organization.GetSettlement((short)item.Value.CurrentSettlementId);
			Location location = settlement.GetLocation();
			if (validLocation.AreaId == location.AreaId && location != taiwuVillageLocation)
			{
				if (dictionary.ContainsKey(location.AreaId))
				{
					dictionary[location.AreaId].Add((short)item.Value.CurrentSettlementId);
					continue;
				}
				dictionary.Add(location.AreaId, new List<short> { (short)item.Value.CurrentSettlementId });
			}
		}
		if (dictionary.Count > 0)
		{
			DomainManager.Extra.FinishTriggeredExtraTask(context, 53, 342);
			DomainManager.Extra.TriggerExtraTask(context, 53, 342);
			ChickenMapInfo = dictionary.Values.ElementAt(0).Distinct().ToList();
			return true;
		}
		foreach (KeyValuePair<int, Chicken> item2 in _chicken)
		{
			if (flag && sectFulongLoseFeatherChickens != null && sectFulongLoseFeatherChickens.Contains(item2.Key))
			{
				continue;
			}
			Settlement settlement2 = DomainManager.Organization.GetSettlement((short)item2.Value.CurrentSettlementId);
			Location location2 = settlement2.GetLocation();
			if (location2 == taiwuVillageLocation)
			{
				continue;
			}
			CrossAreaMoveInfo crossAreaMoveInfo = DomainManager.Map.CalcAreaTravelRoute(taiwu, validLocation.AreaId, validLocation.BlockId, location2.AreaId);
			short totalTimeCost = crossAreaMoveInfo.Route.GetTotalTimeCost();
			if (totalTimeCost < num)
			{
				num = totalTimeCost;
				dictionary.Clear();
				dictionary.Add(location2.AreaId, new List<short> { (short)item2.Value.CurrentSettlementId });
			}
			else if (totalTimeCost == num)
			{
				if (dictionary.ContainsKey(location2.AreaId))
				{
					dictionary[location2.AreaId].Add((short)item2.Value.CurrentSettlementId);
					continue;
				}
				dictionary.Add(location2.AreaId, new List<short> { (short)item2.Value.CurrentSettlementId });
			}
		}
		DomainManager.Extra.FinishTriggeredExtraTask(context, 53, 342);
		if (dictionary.Count > 0)
		{
			ChickenMapInfo = dictionary.Values.ElementAt(context.Random.Next(0, dictionary.Count)).Distinct().ToList();
			DomainManager.Extra.TriggerExtraTask(context, 53, 342);
			return true;
		}
		ChickenMapInfo.Clear();
		return false;
	}

	[DomainMethod]
	public void ClickChickenSign(DataContext context, int chickenId)
	{
		List<int> sectFulongLoseFeatherChickens = DomainManager.Extra.GetSectFulongLoseFeatherChickens();
		if (!DomainManager.Extra.IsExtraTaskInProgress(338))
		{
			return;
		}
		foreach (KeyValuePair<int, Chicken> item in _chicken)
		{
			if (item.Key == chickenId && !sectFulongLoseFeatherChickens.Contains(chickenId))
			{
				DomainManager.TaiwuEvent.OnEvent_ClickChicken(chickenId, item.Value.TemplateId);
				break;
			}
		}
	}

	[DomainMethod]
	public bool IsInFulongSeekFeatherTask(DataContext context)
	{
		return DomainManager.Extra.IsExtraTaskInProgress(338);
	}

	[DomainMethod]
	public List<int> GetSettlementChickenList(int sourceSettlementId, bool ignoreFulong = true)
	{
		List<int> list = GetSettlementChickenIdList(sourceSettlementId).ToList();
		if (ignoreFulong)
		{
			return list;
		}
		if (DomainManager.World.GetSectMainStoryTaskStatus(14) == 0)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement((short)sourceSettlementId);
			if (DomainManager.Taiwu.GetTaiwuVillageLocation().Equals(settlement.GetLocation()))
			{
				EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(14);
				bool arg = false;
				sectMainStoryEventArgBox.Get("ConchShip_PresetKey_FulongChickenKingLeaveHome", ref arg);
				if (arg)
				{
					for (int num = list.Count - 1; num >= 0; num--)
					{
						if (_chicken[list[num]].TemplateId == 63)
						{
							list.RemoveAt(num);
							break;
						}
					}
				}
			}
		}
		return list;
	}

	public int GetChickenKingId()
	{
		foreach (KeyValuePair<int, Chicken> item in _chicken)
		{
			if (item.Value.TemplateId == 63)
			{
				return item.Key;
			}
		}
		return -1;
	}

	[DomainMethod]
	public List<Chicken> GetSettlementChickenDataList(Location location)
	{
		int id = DomainManager.Organization.GetSettlementByLocation(location).GetId();
		List<int> chickenKeyList = GetSettlementChickenList(id);
		MakeSureChickenKingFirst(ref chickenKeyList, location);
		List<Chicken> list = new List<Chicken>();
		for (int i = 0; i < chickenKeyList.Count; i++)
		{
			Chicken chickenData = GetChickenData(chickenKeyList[i]);
			list.Add(chickenData);
		}
		return list;
	}

	private void MakeSureChickenKingFirst(ref List<int> chickenKeyList, Location location)
	{
		if (DomainManager.Taiwu.GetTaiwuVillageLocation().Equals(location) && chickenKeyList.Count > 0 && _chicken[chickenKeyList[0]].TemplateId != 63)
		{
			int chickenKingId = GetChickenKingId();
			if (chickenKingId >= 0)
			{
				chickenKeyList.Remove(chickenKingId);
				chickenKeyList.Insert(0, chickenKingId);
			}
		}
	}

	private void FixChickenKing(DataContext context)
	{
		short mainStoryLineProgress = DomainManager.World.GetMainStoryLineProgress();
		if (mainStoryLineProgress < 16)
		{
			return;
		}
		if (_chicken.Count == 1 && DomainManager.Extra.IsDreamBack())
		{
			ForceInitMapBlockChicken(context);
			return;
		}
		foreach (int key in _chicken.Keys)
		{
			if (_chicken[key].TemplateId == 63)
			{
				return;
			}
		}
		short templateId = 63;
		int chickenId = AddChicken(context, DomainManager.Taiwu.GetTaiwuVillageSettlementId(), templateId);
		SetChickenHappiness(context, chickenId, 100);
		Logger.Warn("Fix chicken king missing.");
	}

	[DomainMethod]
	public List<int> GetSettlementChickenIdList(Location location)
	{
		int id = DomainManager.Organization.GetSettlementByLocation(location).GetId();
		List<int> chickenKeyList = GetSettlementChickenList(id);
		MakeSureChickenKingFirst(ref chickenKeyList, location);
		return chickenKeyList;
	}

	[DomainMethod]
	public Chicken GetChickenData(int id)
	{
		Chicken value;
		return (!TryGetElement_Chicken(id, out value)) ? default(Chicken) : value;
	}

	[DomainMethod]
	public List<Chicken> GetChickenDataList(List<int> idList)
	{
		List<Chicken> list = new List<Chicken>();
		if (idList == null)
		{
			return list;
		}
		foreach (int id in idList)
		{
			if (TryGetElement_Chicken(id, out var value))
			{
				list.Add(value);
			}
			else
			{
				list.Add(default(Chicken));
			}
		}
		return list;
	}

	[DomainMethod]
	public void SetNickNameByChickenId(DataContext context, int id, string nickname)
	{
		if (nickname == null)
		{
			nickname = string.Empty;
		}
		int element_NicknameDict;
		if (DomainManager.Extra.CheckChickenHasNickname(id))
		{
			element_NicknameDict = DomainManager.Extra.GetElement_NicknameDict(id);
			if (DomainManager.World.TryGetElement_CustomTexts(element_NicknameDict, out var value) && !string.IsNullOrEmpty(value) && value.Equals(nickname))
			{
				return;
			}
			DomainManager.World.UnregisterCustomText(context, element_NicknameDict);
		}
		element_NicknameDict = DomainManager.World.RegisterCustomText(context, nickname);
		DomainManager.Extra.SetNicknameByChickenId(id, element_NicknameDict, context);
	}

	[Obsolete]
	[DomainMethod]
	public List<string> GetChickensNicknameByIdList(Location location)
	{
		return GetChickensNicknameByLocation(location);
	}

	[DomainMethod]
	public List<string> GetChickensNicknameByLocation(Location location)
	{
		int id = DomainManager.Organization.GetSettlementByLocation(location).GetId();
		List<int> settlementChickenList = GetSettlementChickenList(id);
		return GetChickenNicknameList(settlementChickenList);
	}

	[DomainMethod]
	public List<string> GetChickenNicknameList(List<int> chickenIdList)
	{
		List<string> list = new List<string>();
		if (chickenIdList == null)
		{
			return list;
		}
		foreach (int chickenId in chickenIdList)
		{
			if (DomainManager.Extra.CheckChickenHasNickname(chickenId))
			{
				int element_NicknameDict = DomainManager.Extra.GetElement_NicknameDict(chickenId);
				list.Add(DomainManager.World.GetElement_CustomTexts(element_NicknameDict));
			}
			else
			{
				list.Add("");
			}
		}
		return list;
	}

	[DomainMethod]
	[Obsolete("可以使用FeedChickenWithArgs替代")]
	public sbyte FeedChicken(DataContext context, int id, ItemKey itemKey)
	{
		return FeedChickenWithArgs(context, id, itemKey, ItemSourceType.Inventory);
	}

	[DomainMethod]
	public bool SetFulongChicken(DataContext context, short orgMemberTemplateId, int chickenId)
	{
		DomainManager.Extra.TryGetElement_SectFulongOrgMemberChickens(orgMemberTemplateId, out var value);
		List<int> items = value.Items;
		if (items != null && items.Contains(chickenId))
		{
			return false;
		}
		ref List<int> items2 = ref value.Items;
		if (items2 == null)
		{
			items2 = new List<int>();
		}
		value.Items.Add(chickenId);
		DomainManager.Extra.AddOrSetFulongChickens(context, orgMemberTemplateId, value);
		return true;
	}

	[DomainMethod]
	public bool UnsetFulongChicken(DataContext context, short orgMemberTemplateId, int chickenId)
	{
		DomainManager.Extra.TryGetElement_SectFulongOrgMemberChickens(orgMemberTemplateId, out var value);
		List<int> items = value.Items;
		if (items == null || !items.Contains(chickenId))
		{
			return false;
		}
		value.Items.Remove(chickenId);
		DomainManager.Extra.AddOrSetFulongChickens(context, orgMemberTemplateId, value);
		return true;
	}

	public IEnumerable<Chicken> GetFulongChickens(short orgMemberTemplateId)
	{
		if (!DomainManager.Extra.TryGetElement_SectFulongOrgMemberChickens(orgMemberTemplateId, out var chickens))
		{
			yield break;
		}
		List<int> items = chickens.Items;
		if (items == null || items.Count <= 0)
		{
			yield break;
		}
		short taiwuSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		foreach (int chickenId in chickens.Items)
		{
			if (_chicken.TryGetValue(chickenId, out var chicken) && chicken.CurrentSettlementId == taiwuSettlementId)
			{
				yield return chicken;
			}
			chicken = default(Chicken);
		}
	}

	public bool HasFulongChicken(VillagerRoleItem role)
	{
		if (DomainManager.Extra.TryGetElement_SectFulongOrgMemberChickens(role.OrganizationMember, out var value))
		{
			List<int> items = value.Items;
			if (items != null && items.Count > 0)
			{
				return IsVillagerRoleExtraEffectUnlockState(role);
			}
		}
		return false;
	}

	[DomainMethod]
	public List<bool> GetVillagerRoleExtraEffectUnlockState()
	{
		List<bool> list = new List<bool>();
		foreach (VillagerRoleItem item in (IEnumerable<VillagerRoleItem>)VillagerRole.Instance)
		{
			list.Add(IsVillagerRoleExtraEffectUnlockState(item));
		}
		return list;
	}

	private static bool IsVillagerRoleExtraEffectUnlockState(VillagerRoleItem roleConfig)
	{
		bool flag = false;
		int[] array = new int[roleConfig.NeedPersonalityList.Length];
		foreach (Chicken fulongChicken in DomainManager.Building.GetFulongChickens(roleConfig.OrganizationMember))
		{
			flag = true;
			ChickenItem chickenItem = Config.Chicken.Instance[fulongChicken.TemplateId];
			for (int i = 0; i < roleConfig.NeedPersonalityList.Length; i++)
			{
				if (chickenItem.PersonalityType == roleConfig.NeedPersonalityList[i].PersonalityType)
				{
					array[i]++;
				}
			}
		}
		if (!flag)
		{
			return false;
		}
		bool result = true;
		for (int j = 0; j < roleConfig.NeedPersonalityList.Length; j++)
		{
			if (array[j] < roleConfig.NeedPersonalityList[j].NeedCount)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	private sbyte FeedChickenWithArgs(DataContext context, int templateId, ItemKey itemKey, ItemSourceType itemSourceType)
	{
		sbyte b = 0;
		if (!TryGetElement_ChickenByTemplateId(templateId, out var value))
		{
			return 0;
		}
		if (ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == 1204)
		{
			b = GlobalConfig.Instance.ChickenMiscTaste;
			value.Happiness += GlobalConfig.Instance.ChickenMiscTaste;
		}
		else if (itemKey.ItemType == 11)
		{
			ItemDisplayData itemDisplayData = DomainManager.Item.GetItemDisplayData(itemKey);
			CricketPartsItem cricketPartsItem = null;
			cricketPartsItem = ((ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId) < 7) ? CricketParts.Instance[itemDisplayData.CricketPartId] : CricketParts.Instance[itemDisplayData.CricketPartId + itemDisplayData.CricketColorId]);
			sbyte b2 = cricketPartsItem.Taste;
			if (cricketPartsItem.Type != ECricketPartsType.Trash && cricketPartsItem.Type != ECricketPartsType.RealColor && cricketPartsItem.Type != ECricketPartsType.King)
			{
				b2 += CricketParts.Instance[itemDisplayData.CricketColorId].Taste;
			}
			b = b2;
			value.Happiness += b2;
		}
		else if (itemKey.ItemType == 5)
		{
			b = DomainManager.Item.GetElement_Materials(itemKey.Id).GetHappinessChange();
			value.Happiness += b;
		}
		if (value.Happiness < 0)
		{
			value.Happiness = 100;
		}
		value.Happiness = Math.Max(0, Math.Min(value.Happiness, 100));
		SetElement_Chicken(value.Id, value, context);
		switch (itemSourceType)
		{
		case ItemSourceType.Inventory:
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			taiwu.RemoveInventoryItem(context, itemKey, 1, deleteItem: true);
			break;
		}
		case ItemSourceType.Trough:
			DomainManager.Extra.TroughRemove(context, itemKey, 1, deleteItem: true);
			break;
		}
		return b;
	}

	public bool TryGetFirstChickenInSettlement(Settlement settlement, out Chicken chicken)
	{
		chicken = default(Chicken);
		short id = settlement.GetId();
		foreach (KeyValuePair<int, Chicken> item in _chicken)
		{
			if (item.Value.CurrentSettlementId == id)
			{
				chicken = item.Value;
				return true;
			}
		}
		return false;
	}

	public bool TryGetElement_ChickenByTemplateId(int templateId, out Chicken value)
	{
		foreach (int key in _chicken.Keys)
		{
			if (_chicken[key].TemplateId == templateId)
			{
				value = _chicken[key];
				return true;
			}
		}
		value = default(Chicken);
		return false;
	}

	internal bool CanSettlementHaveChicken(Location settlementLocation)
	{
		short blockId = settlementLocation.BlockId;
		if (blockId >= 0)
		{
			MapBlockData block = DomainManager.Map.GetBlock(settlementLocation.AreaId, blockId);
			if (block.TemplateId == 34 || block.TemplateId == 35 || block.TemplateId == 36 || block.BlockType == EMapBlockType.City)
			{
				return true;
			}
		}
		return false;
	}

	internal List<int> GetChickenSettlements()
	{
		List<int> list = new List<int>();
		for (short num = 0; num < 135; num++)
		{
			MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(num);
			SettlementInfo[] settlementInfos = element_Areas.SettlementInfos;
			for (int i = 0; i < settlementInfos.Length; i++)
			{
				SettlementInfo settlementInfo = settlementInfos[i];
				if (CanSettlementHaveChicken(new Location(num, settlementInfo.BlockId)))
				{
					list.Add(settlementInfo.SettlementId);
				}
			}
		}
		return list;
	}

	protected List<int> _GetChickenVillageSettlements()
	{
		List<int> list = new List<int>();
		for (short num = 0; num < 135; num++)
		{
			MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(num);
			SettlementInfo[] settlementInfos = element_Areas.SettlementInfos;
			for (int i = 0; i < settlementInfos.Length; i++)
			{
				SettlementInfo settlementInfo = settlementInfos[i];
				short blockId = settlementInfo.BlockId;
				if (blockId >= 0)
				{
					MapBlockData block = DomainManager.Map.GetBlock(num, blockId);
					if (block.TemplateId == 34)
					{
						list.Add(settlementInfo.SettlementId);
					}
				}
			}
		}
		return list;
	}

	[DomainMethod]
	public void InitMapBlockChicken(DataContext context)
	{
		if (!DomainManager.Extra.IsDreamBack() || _chicken.Count == 1)
		{
			ForceInitMapBlockChicken(context);
		}
	}

	internal void ForceInitMapBlockChicken(DataContext context)
	{
		List<short> list = new List<short>();
		List<int> chickenSettlements = GetChickenSettlements();
		for (int i = 0; i < Config.Chicken.Instance.Count; i++)
		{
			if (Config.Chicken.Instance[i].TemplateId != 63)
			{
				list.Add(Config.Chicken.Instance[i].TemplateId);
			}
		}
		CollectionUtils.Shuffle(context.Random, chickenSettlements);
		bool flag = false;
		Chicken value = default(Chicken);
		foreach (int key in _chicken.Keys)
		{
			if (_chicken[key].TemplateId == 63)
			{
				flag = true;
				value = _chicken[key];
			}
		}
		ClearChicken(context);
		if (flag)
		{
			AddElement_Chicken(_chicken.Count, value, context);
		}
		while (list.Count > 0 && chickenSettlements.Count > 0)
		{
			short templateId = list[0];
			int settlementId = chickenSettlements[0];
			AddChicken(context, settlementId, templateId);
			list.RemoveAt(0);
			chickenSettlements.RemoveAt(0);
		}
	}

	[DomainMethod]
	public bool IsHaveChickenKing(DataContext context)
	{
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		List<int> settlementChickenList = GetSettlementChickenList(taiwuVillageSettlementId);
		if (settlementChickenList.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < settlementChickenList.Count; i++)
		{
			if (_chicken[settlementChickenList[i]].TemplateId == 63)
			{
				return true;
			}
		}
		return false;
	}

	public void UpdateChickenInstances(DataContext context)
	{
		Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
		foreach (int chickenSettlement in GetChickenSettlements())
		{
			dictionary.Add(chickenSettlement, new List<int>());
		}
		int[] array = _chicken.Keys.ToArray();
		foreach (int num in array)
		{
			Chicken value = _chicken[num];
			if (!dictionary.ContainsKey(value.CurrentSettlementId))
			{
				dictionary.Add(value.CurrentSettlementId, new List<int>());
			}
			dictionary[value.CurrentSettlementId].Add(value.Id);
			if (num != value.Id)
			{
				value.Id = num;
				SetElement_Chicken(num, value, context);
			}
		}
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		Chicken[] array2 = _chicken.Values.ToArray();
		for (int j = 0; j < array2.Length; j++)
		{
			Chicken chicken = array2[j];
			Chicken value2 = chicken;
			if (value2.TemplateId == 63)
			{
				if (value2.Happiness != 100)
				{
					SetChickenHappiness(context, value2.Id, 100);
				}
				continue;
			}
			if (DomainManager.Taiwu.GetTaiwuVillageSettlementId() == value2.CurrentSettlementId)
			{
				int num2 = 1;
				Location location = DomainManager.Organization.GetSettlement(DomainManager.Taiwu.GetTaiwuVillageSettlementId()).GetLocation();
				int num3 = 0;
				foreach (BuildingBlockData buildingBlock in DomainManager.Building.GetBuildingBlockList(location))
				{
					if (buildingBlock.TemplateId == 49)
					{
						num3++;
					}
				}
				if (num3 == 0)
				{
					num2 = 3;
				}
				value2.Happiness = (sbyte)Math.Max(0, value2.Happiness - num2 * context.Random.Next((int)GlobalConfig.Instance.ChickenDecayMin, (int)GlobalConfig.Instance.ChickenDecayMax));
				SetChickenHappiness(context, value2.Id, value2.Happiness);
				while (value2.Happiness < 50 && DomainManager.Extra.TroughItems.Count > 0)
				{
					ItemKey key = DomainManager.Extra.TroughItems.OrderBy((KeyValuePair<ItemKey, int> pair) => ItemTemplateHelper.GetGrade(pair.Key.ItemType, pair.Key.TemplateId)).First().Key;
					sbyte b = FeedChickenWithArgs(context, value2.TemplateId, key, ItemSourceType.Trough);
					value2.Happiness += b;
				}
				if (value2.Happiness <= 0)
				{
					int? num4 = null;
					foreach (var (value3, list2) in dictionary)
					{
						if (list2.Count == 0)
						{
							num4 = value3;
							list2.Add(value2.Id);
							break;
						}
					}
					dictionary[value2.CurrentSettlementId].Remove(value2.Id);
					if (!num4.HasValue)
					{
						throw new Exception($"chicken: #{value2.Id} cannot find a next settlement for escape");
					}
					value2.CurrentSettlementId = num4.Value;
					monthlyNotificationCollection.AddChickenEscaped(value2.TemplateId);
				}
			}
			short featureId = Config.Chicken.Instance[value2.TemplateId].FeatureId;
			if (featureId >= 0)
			{
				int[] array3 = (from data in DomainManager.Organization.GetSettlementMembers((short)value2.CurrentSettlementId)
					select data.CharacterId).Where(delegate(int characterId)
				{
					GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(characterId);
					List<short> chickenFeatures = element_Objects2.GetChickenFeatures();
					return !chickenFeatures.Contains(featureId);
				}).ToArray();
				if (array3.Any())
				{
					CollectionUtils.Shuffle(context.Random, array3);
					int objectId = array3.First();
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(objectId);
					element_Objects.AddFeature(context, featureId);
				}
			}
			SetElement_Chicken(chicken.Id, value2, context);
		}
	}

	public void SetChickenHappiness(DataContext context, int chickenId, sbyte happiness)
	{
		Chicken chickenData = GetChickenData(chickenId);
		chickenData.Happiness = Math.Clamp(happiness, 0, 100);
		SetElement_Chicken(chickenId, chickenData, context);
	}

	private int GenerateNextChickenId()
	{
		int nextChickenId = _nextChickenId;
		_nextChickenId++;
		return nextChickenId;
	}

	private void InitializeNextChickenId()
	{
		foreach (int key in _chicken.Keys)
		{
			if (key >= _nextChickenId)
			{
				_nextChickenId = key + 1;
			}
		}
	}

	public bool isChickenBlessingInfoEmpty()
	{
		return _chickenBlessingInfoData == null || _chickenBlessingInfoData.Count == 0;
	}

	public void ClearChickenBlessingInfo(DataContext context)
	{
		ClearChickenBlessingInfoData(context);
	}

	internal sbyte BuildingBlockLevel(BuildingBlockKey blockKey)
	{
		if (blockKey.IsInvalid)
		{
			return 0;
		}
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		Location other = new Location(blockKey.AreaId, blockKey.BlockId);
		BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
		if (!taiwuVillageLocation.Equals(other))
		{
			return Math.Min(element_BuildingBlocks.Level, buildingBlockItem.MaxLevel);
		}
		if (buildingBlockItem == null)
		{
			return 1;
		}
		if (buildingBlockItem.MaxLevel > 1)
		{
			if (buildingBlockItem.Type == EBuildingBlockType.UselessResource)
			{
				return element_BuildingBlocks.Level;
			}
			if (DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)blockKey, out var value))
			{
				return Math.Min(buildingBlockItem.MaxLevel, value.CalcUnlockedLevelCount());
			}
			return buildingBlockItem.MaxLevel;
		}
		return buildingBlockItem.MaxLevel;
	}

	internal void BuildingBlockDependencies(BuildingBlockKey blockKey, Action<BuildingBlockData, int, BuildingBlockKey> onDependencyFound)
	{
		if (!TryGetElement_BuildingAreas(blockKey.GetLocation(), out var value) || !TryGetElement_BuildingBlocks(blockKey, out var value2))
		{
			return;
		}
		BuildingBlockItem item = BuildingBlock.Instance.GetItem(value2.TemplateId);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		Dictionary<short, int> dictionary = ObjectPool<Dictionary<short, int>>.Instance.Get();
		dictionary.Clear();
		value.GetNeighborBlocks(blockKey.BuildingBlockIndex, item.Width, list, list2, 2);
		foreach (short dependBuilding in item.DependBuildings)
		{
			for (int i = 0; i < list.Count; i++)
			{
				BuildingBlockKey elementId = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, list[i]);
				if (!TryGetElement_BuildingBlocks(elementId, out var value3))
				{
					continue;
				}
				if (value3.RootBlockIndex >= 0)
				{
					elementId = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, value3.RootBlockIndex);
					value3 = GetElement_BuildingBlocks(elementId);
				}
				if (value3.TemplateId == dependBuilding)
				{
					int num = list2[i];
					if (!dictionary.TryGetValue(elementId.BuildingBlockIndex, out var value4) || value4 >= num)
					{
						dictionary[elementId.BuildingBlockIndex] = num;
					}
				}
			}
		}
		foreach (KeyValuePair<short, int> item2 in dictionary)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, item2.Key);
			onDependencyFound(GetElement_BuildingBlocks(buildingBlockKey), item2.Value, buildingBlockKey);
		}
		ObjectPool<Dictionary<short, int>>.Instance.Return(dictionary);
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<int>>.Instance.Return(list2);
	}

	internal void BuildingBlockInfluences(BuildingBlockKey blockKey, Action<BuildingBlockData, int> onInfluenceFound)
	{
		if (TryGetElement_BuildingAreas(blockKey.GetLocation(), out var value) && TryGetElement_BuildingBlocks(blockKey, out var value2))
		{
			BuildingBlockItem item = BuildingBlock.Instance.GetItem(value2.TemplateId);
			List<short> list = ObjectPool<List<short>>.Instance.Get();
			List<int> list2 = ObjectPool<List<int>>.Instance.Get();
			value.GetNeighborBlocks(blockKey.BuildingBlockIndex, item.Width, list, list2, 2);
			value2.CalcInfluences(list.Select(delegate(short index)
			{
				BuildingBlockKey elementId = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, index);
				BuildingBlockData value3;
				return TryGetElement_BuildingBlocks(elementId, out value3) ? value3 : null;
			}), list2, onInfluenceFound);
			ObjectPool<List<short>>.Instance.Return(list);
			ObjectPool<List<int>>.Instance.Return(list2);
		}
	}

	internal int BuildingTotalAttainment(BuildingBlockKey blockKey, sbyte resourceType, out bool hasManager, bool ignoreCanWork = false)
	{
		if (TryGetElement_BuildingBlocks(blockKey, out var value) && TryGetElement_ShopManagerDict(blockKey, out var value2))
		{
			hasManager = false;
			int num = 0;
			int num2 = 150;
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[value.TemplateId];
			if (buildingBlockItem.IsCollectResourceBuilding)
			{
				for (int i = 0; i < value2.GetCount(); i++)
				{
					int num3 = value2.GetCollection()[i];
					if (GameData.Domains.Character.Character.IsCharacterIdValid(num3) && (ignoreCanWork || DomainManager.Taiwu.CanWork(num3)))
					{
						GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num3);
						if (element_Objects.GetAgeGroup() == 2)
						{
							GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(num3);
							if (i == 0)
							{
								num2 += element_Objects2.GetLifeSkillAttainment(buildingBlockItem.RequireLifeSkillType);
							}
							else
							{
								num += 50 + element_Objects2.GetLifeSkillAttainment(buildingBlockItem.RequireLifeSkillType);
							}
							hasManager = true;
						}
					}
					else if (i == 0 && buildingBlockItem.NeedLeader)
					{
						return 0;
					}
				}
			}
			else
			{
				for (int j = 0; j < value2.GetCount(); j++)
				{
					int num4 = value2.GetCollection()[j];
					if (GameData.Domains.Character.Character.IsCharacterIdValid(num4) && (ignoreCanWork || DomainManager.Taiwu.CanWork(num4)))
					{
						GameData.Domains.Character.Character element_Objects3 = DomainManager.Character.GetElement_Objects(num4);
						if (element_Objects3.GetAgeGroup() == 2)
						{
							GameData.Domains.Character.Character element_Objects4 = DomainManager.Character.GetElement_Objects(num4);
							short lifeSkillAttainment = element_Objects4.GetLifeSkillAttainment(buildingBlockItem.RequireLifeSkillType);
							if (j == 0)
							{
								num2 += lifeSkillAttainment;
							}
							else
							{
								num += 50 + lifeSkillAttainment;
							}
							hasManager = true;
						}
					}
					else if (j == 0 && buildingBlockItem.NeedLeader)
					{
						return 0;
					}
				}
			}
			return num2 + num / GlobalConfig.Instance.BuildingTotalAttainmentFinalDivisor;
		}
		hasManager = false;
		return 0;
	}

	internal int BuildingMaxAttainment(BuildingBlockKey blockKey, sbyte resourceType, out bool hasManager)
	{
		hasManager = false;
		if (!TryGetElement_BuildingBlocks(blockKey, out var value) || !TryGetElement_ShopManagerDict(blockKey, out var value2))
		{
			return 0;
		}
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[value.TemplateId];
		int num = value2.GetCollection()[0];
		if (!GameData.Domains.Character.Character.IsCharacterIdValid(num) || !DomainManager.Taiwu.CanWork(num))
		{
			return 0;
		}
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num);
		hasManager = true;
		return 150 + ((buildingBlockItem.RequireLifeSkillType >= 0) ? element_Objects.GetLifeSkillAttainment(buildingBlockItem.RequireLifeSkillType) : element_Objects.GetCombatSkillAttainment(buildingBlockItem.RequireCombatSkillType));
	}

	internal int BuildingTotalAverageAttainment(BuildingBlockKey blockKey, sbyte resourceType, out bool hasManager, bool ignoreCanWork = false)
	{
		if (TryGetElement_BuildingBlocks(blockKey, out var value) && TryGetElement_ShopManagerDict(blockKey, out var value2))
		{
			hasManager = false;
			int num = 0;
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[value.TemplateId];
			if (buildingBlockItem.IsCollectResourceBuilding)
			{
				for (int i = 0; i < value2.GetCount(); i++)
				{
					int num2 = value2.GetCollection()[i];
					if (GameData.Domains.Character.Character.IsCharacterIdValid(num2) && (ignoreCanWork || DomainManager.Taiwu.CanWork(num2)))
					{
						GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num2);
						num += element_Objects.GetLifeSkillAttainment(buildingBlockItem.RequireLifeSkillType);
						hasManager = true;
					}
				}
			}
			else
			{
				for (int j = 0; j < value2.GetCount(); j++)
				{
					int num3 = value2.GetCollection()[j];
					if (GameData.Domains.Character.Character.IsCharacterIdValid(num3) && (ignoreCanWork || DomainManager.Taiwu.CanWork(num3)))
					{
						GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(num3);
						short lifeSkillAttainment = element_Objects2.GetLifeSkillAttainment(buildingBlockItem.RequireLifeSkillType);
						num += lifeSkillAttainment;
						hasManager = true;
					}
				}
			}
			return num / value2.GetCount();
		}
		hasManager = false;
		return 0;
	}

	internal sbyte BuildLeaderFameType(BuildingBlockKey blockKey)
	{
		GameData.Domains.Character.Character shopManagerLeader = GetShopManagerLeader(blockKey);
		if (shopManagerLeader == null)
		{
			return 0;
		}
		sbyte fame = shopManagerLeader.GetFame();
		return FameType.GetFameType(fame);
	}

	internal int BuildingProductivityByMaxDependencies(BuildingBlockKey blockKey)
	{
		if (!TryGetElement_BuildingAreas(blockKey.GetLocation(), out var _) || !TryGetElement_BuildingBlocks(blockKey, out var value2))
		{
			return 0;
		}
		int num = 0;
		BuildingBlockItem item = BuildingBlock.Instance.GetItem(value2.TemplateId);
		List<(BuildingBlockData, int)> set = new List<(BuildingBlockData, int)>();
		BuildingBlockDependencies(blockKey, delegate(BuildingBlockData data, int distance, BuildingBlockKey _)
		{
			set.Add((data, distance));
		});
		foreach (short dependTemplateId in item.DependBuildings)
		{
			int num2 = 0;
			foreach (var item2 in set.Where(((BuildingBlockData, int) b) => b.Item1.TemplateId == dependTemplateId))
			{
				int num3 = BuildingProductivityBySingleDependency(new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, item2.Item1.BlockIndex), item2.Item2);
				if (num3 > num2)
				{
					num2 = num3;
				}
			}
			num += num2;
		}
		return num;
	}

	internal int BuildingProductivityBySingleDependency(BuildingBlockKey buildingBlockKey, int dependencyDistance)
	{
		return (dependencyDistance > 1) ? 20 : 100;
	}

	internal ResourceInts BuildingBaseYield(BuildingBlockData resourceBlockData)
	{
		ResourceInts result = default(ResourceInts);
		BuildingBlockItem item = BuildingBlock.Instance.GetItem(resourceBlockData.TemplateId);
		for (int i = 0; i < item.CollectResourcePercent.Length; i++)
		{
			if (item.CollectResourcePercent[i] > 0)
			{
				int value = 50 + 50 * item.CollectResourcePercent[i];
				result.Add((sbyte)Math.Min(i, 5), value);
			}
		}
		return result;
	}

	internal int BuildingResourceYieldLevel(BuildingBlockKey blockKey, short templateId)
	{
		sbyte level = BuildingBlockLevel(blockKey);
		return SharedMethods.GetBuildingLevelEffect(templateId, level);
	}

	internal int BuildingRandomCorrection(int value, IRandomSource randomSource)
	{
		return value * randomSource.Next(GlobalConfig.Instance.BuildingOutputRandomFactorLowerLimit, GlobalConfig.Instance.BuildingOutputRandomFactorUpperLimit) / 100;
	}

	internal int BuildingManagerSpecificMaxCharacterId(BuildingBlockKey blockKey, Func<int, int> spec)
	{
		int num = 0;
		int result = -1;
		DomainManager.Building.TryGetElement_ShopManagerDict(blockKey, out var value);
		for (int i = 0; i < value.GetCount(); i++)
		{
			int num2 = value.GetCollection()[i];
			if (num2 >= 0 && DomainManager.Taiwu.CanWork(num2))
			{
				int num3 = spec(num2);
				if (num3 >= num)
				{
					num = num3;
					result = num2;
				}
			}
		}
		return result;
	}

	internal static int BuildingManagerAttraction(int charId)
	{
		if (DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			return element.GetAttraction();
		}
		return 0;
	}

	internal unsafe static int BuildingManagerPersonalitiesSum(int charId)
	{
		if (DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			Personalities personalities = element.GetPersonalities();
			int num = 0;
			for (int i = 0; i < 7; i++)
			{
				num += personalities.Items[i];
			}
			return num;
		}
		return 0;
	}

	internal int BuildingManageHarvestSuccessRate(BuildingBlockKey blockKey, int charId)
	{
		if (TryGetElement_BuildingBlocks(blockKey, out var value))
		{
			short templateId = value.TemplateId;
			short num = templateId;
			int buildingAttainment = GetBuildingAttainment(value, blockKey);
			return buildingAttainment / 5;
		}
		return 0;
	}

	internal int BuildingManageHarvestSpecialSuccessRate(BuildingBlockKey blockKey, int charId)
	{
		GameData.Domains.Character.Character character = (GameData.Domains.Character.Character.IsCharacterIdValid(charId) ? DomainManager.Character.GetElement_Objects(charId) : GetShopManagerLeader(blockKey));
		if (TryGetElement_BuildingBlocks(blockKey, out var value))
		{
			switch (value.TemplateId)
			{
			case 215:
			{
				int num2 = character?.GetPersonalities().GetSum() ?? 0;
				return num2 / 10;
			}
			case 216:
			{
				int num = character?.GetAttraction() ?? 0;
				return num / 30;
			}
			}
		}
		return -1;
	}

	internal int BuildingManageHarvestSuccessRate(BuildingBlockKey blockKey)
	{
		return BuildingManageHarvestSuccessRate(blockKey, -1);
	}

	public void GetTopLevelBlocks(short templateId, Location location, List<(BuildingBlockKey, int)> blocks, int count)
	{
		blocks.Clear();
		IEnumerable<BuildingBlockData> buildingBlocksAtLocation = GetBuildingBlocksAtLocation(location);
		foreach (BuildingBlockData item2 in buildingBlocksAtLocation)
		{
			if (item2.TemplateId == templateId)
			{
				BuildingBlockKey buildingBlockKey = new BuildingBlockKey(location.AreaId, location.BlockId, item2.BlockIndex);
				if (item2.CanUse() && AllDependBuildingAvailable(buildingBlockKey, item2.TemplateId, out var _))
				{
					sbyte item = BuildingBlockLevel(buildingBlockKey);
					blocks.Add((buildingBlockKey, item));
				}
			}
		}
		blocks.Sort(((BuildingBlockKey, int) a, (BuildingBlockKey, int) b) => b.Item2.CompareTo(a.Item2));
		if (blocks.Count > count)
		{
			blocks.RemoveRange(count, blocks.Count - count);
		}
	}

	public void ConsumeResource(DataContext context, sbyte resourceType, int delta)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int resource = taiwu.GetResource(resourceType);
		int num = Math.Min(resource, delta);
		taiwu.ChangeResource(context, resourceType, -num);
		delta -= num;
		if (delta > 0)
		{
			SettlementTreasury taiwuTreasury = DomainManager.Taiwu.GetTaiwuTreasury();
			resource = taiwuTreasury.Resources.Get(resourceType);
			num = Math.Min(resource, delta);
			taiwuTreasury.Resources.Subtract(resourceType, num);
			DomainManager.Taiwu.SetTaiwuTreasury(context, taiwuTreasury);
			delta -= num;
			if (delta > 0)
			{
				ConsumeStorageResource(context, resourceType, delta);
			}
		}
	}

	private void ConsumeStorageResource(DataContext context, sbyte resourceType, int delta)
	{
		TaiwuVillageStorage stockStorage = DomainManager.Extra.GetStockStorage();
		int val = stockStorage.Resources.Get(resourceType);
		int value = Math.Min(val, delta);
		stockStorage.Resources.Subtract(resourceType, value);
		DomainManager.Extra.CommitTaiwuVillageStorages(context);
	}

	private ResourceInts GetAllTaiwuResources()
	{
		ResourceInts result = default(ResourceInts);
		ResourceInts delta = DomainManager.Taiwu.GetAllResources(ItemSourceType.Resources).resource;
		result.Add(ref delta);
		delta = DomainManager.Taiwu.GetAllResources(ItemSourceType.Treasury).resource;
		result.Add(ref delta);
		delta = DomainManager.Taiwu.GetAllResources(ItemSourceType.StockStorageWarehouse).resource;
		result.Add(ref delta);
		return result;
	}

	[DomainMethod]
	public List<BuildingBlockData> GetTaiwuVillageResourceBlockEffectInfo(DataContext context, short templateId)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		List<(BuildingBlockKey, int)> list = new List<(BuildingBlockKey, int)>();
		List<BuildingBlockData> list2 = new List<BuildingBlockData>();
		GetTopLevelBlocks(templateId, taiwuVillageLocation, list, int.MaxValue);
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(GetBuildingBlockData(list[i].Item1));
		}
		return list2;
	}

	[DomainMethod]
	public int CalculateBuildingManageHarvestSuccessRate(BuildingBlockKey blockKey)
	{
		return BuildingManageHarvestSuccessRate(blockKey);
	}

	[DomainMethod]
	public int[] CalculateBuildingManageHarvestSuccessRates(BuildingBlockKey blockKey)
	{
		return new int[2]
		{
			BuildingManageHarvestSuccessRate(blockKey),
			BuildingManageHarvestSpecialSuccessRate(blockKey, -1)
		};
	}

	[DomainMethod]
	public int CalcExtraTaiwuGroupMaxCountByStrategyRoom()
	{
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		return GetBuildingBlockEffect(taiwuVillageSettlementId, EBuildingScaleEffect.TaiwuGroupMaxCount);
	}

	[DomainMethod]
	public int GetStoreLocation(int type)
	{
		return DomainManager.Extra.GetBuildingDefaultStoreLocation().GetMakeData(type);
	}

	[DomainMethod]
	public void SetStoreLocation(DataContext context, int type, int value)
	{
		DomainManager.Extra.GetBuildingDefaultStoreLocation().SetMakeData(type, value);
		DomainManager.Extra.SetBuildingDefaultStoreLocation(DomainManager.Extra.GetBuildingDefaultStoreLocation(), context);
	}

	public void CreateBuildingArea(DataContext context, short mapAreaId, short mapBlockId, short mapBlockTemplateId)
	{
		IRandomSource random = context.Random;
		MapBlockItem mapBlockItem = MapBlock.Instance[mapBlockTemplateId];
		sbyte buildingAreaWidth = mapBlockItem.BuildingAreaWidth;
		int num = buildingAreaWidth * buildingAreaWidth;
		sbyte centerBuildingMaxLevel = mapBlockItem.CenterBuildingMaxLevel;
		MapBlockItem mapBlockItem2 = MapBlock.Instance[mapBlockTemplateId];
		sbyte b = 0;
		if (IsLandFormTypeRandom(mapBlockItem2))
		{
			sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(mapAreaId);
			MapStateItem mapStateItem = MapState.Instance[stateTemplateIdByAreaId];
			b = (sbyte)mapStateItem.BornMapType.GetRandom(context.Random);
		}
		else
		{
			b = ((mapBlockTemplateId == 0) ? DomainManager.World.GetTaiwuVillageLandFormType() : mapBlockItem.LandFormType);
		}
		LandFormTypeItem landFormTypeItem = LandFormType.Instance[b];
		Location elementId = new Location(mapAreaId, mapBlockId);
		BuildingAreaData buildingAreaData = new BuildingAreaData(buildingAreaWidth, b);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		list2.Clear();
		for (short num2 = 0; num2 < num; num2++)
		{
			list.Add(num2);
			if (num2 >= buildingAreaWidth && num2 < num - buildingAreaWidth && num2 % buildingAreaWidth != 0 && num2 % buildingAreaWidth != buildingAreaWidth - 1)
			{
				list2.Add(num2);
			}
		}
		short centerBlockIndex = buildingAreaData.GetCenterBlockIndex();
		sbyte width = BuildingBlock.Instance[mapBlockItem.CenterBuilding].Width;
		sbyte level = (sbyte)Math.Max(random.Next(centerBuildingMaxLevel / 2, centerBuildingMaxLevel + 1), 1);
		short centerBuilding = mapBlockItem.CenterBuilding;
		AddBuilding(context, mapAreaId, mapBlockId, centerBlockIndex, centerBuilding, level, buildingAreaWidth);
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < width; j++)
			{
				short item = (short)(centerBlockIndex + i * buildingAreaWidth + j);
				list.Remove(item);
				list2.Remove(item);
			}
		}
		List<short> list3 = ObjectPool<List<short>>.Instance.Get();
		buildingAreaData.GetNeighborBlocks(centerBlockIndex, width, list3);
		List<short> list4 = ObjectPool<List<short>>.Instance.Get();
		GetAvailableRootBlocks(list3, list4, mapAreaId, mapBlockId, buildingAreaWidth, 2);
		List<short> list5 = ObjectPool<List<short>>.Instance.Get();
		List<short> list6 = ObjectPool<List<short>>.Instance.Get();
		List<short> list7 = ObjectPool<List<short>>.Instance.Get();
		if (!string.IsNullOrEmpty(mapBlockItem.FixedBuildingImage))
		{
			int[] array = CustomBuildingBlockConfig.Data[mapBlockItem.FixedBuildingImage];
			for (short num3 = 0; num3 < array.Length; num3++)
			{
				if (array[num3] != -1)
				{
					BuildingBlockKey buildingBlockKey = new BuildingBlockKey(mapAreaId, mapBlockId, num3);
					BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[array[num3]];
					if (!_buildingBlocks.ContainsKey(buildingBlockKey))
					{
						if (buildingBlockItem.Width == 1)
						{
							AddElement_BuildingBlocks(buildingBlockKey, new BuildingBlockData(num3, buildingBlockItem.TemplateId, (sbyte)((buildingBlockItem.MaxLevel <= 1) ? 1 : random.Next(1, (int)buildingBlockItem.MaxLevel)), -1), context);
							list.Remove(num3);
							list2.Remove(num3);
						}
						else if (buildingBlockItem.Width == 2)
						{
							AddBuilding(context, mapAreaId, mapBlockId, num3, buildingBlockItem.TemplateId, (sbyte)((buildingBlockItem.MaxLevel <= 1) ? 1 : random.Next(1, (int)buildingBlockItem.MaxLevel)), buildingAreaWidth);
							list.Remove(num3);
							list.Remove((short)(num3 + 1));
							list.Remove((short)(num3 + buildingAreaWidth));
							list.Remove((short)(num3 + buildingAreaWidth + 1));
							list2.Remove(num3);
							list2.Remove((short)(num3 + 1));
							list2.Remove((short)(num3 + buildingAreaWidth));
							list2.Remove((short)(num3 + buildingAreaWidth + 1));
						}
					}
				}
			}
		}
		List<short> list8 = ObjectPool<List<short>>.Instance.Get();
		list8.Clear();
		list8.AddRange(mapBlockItem.PresetBuildingList);
		list8.Sort(ComparePresetBuildings);
		for (int k = 0; k < list8.Count; k++)
		{
			short num4 = list8[k];
			if (num4 == 287 && mapAreaId == 138)
			{
				continue;
			}
			BuildingBlockItem buildingBlockItem2 = BuildingBlock.Instance[num4];
			width = buildingBlockItem2.Width;
			short num5;
			if (BuildingBlockData.IsResource(buildingBlockItem2.Type))
			{
				num5 = list2[random.Next(0, list2.Count)];
			}
			else
			{
				if (1 == 0)
				{
				}
				short num6 = ((width != 2) ? GetRandomAvailableBuildingBlock(context, list2, list3, list5, mapAreaId, mapBlockId, buildingAreaWidth, width) : GetRandomAvailableBuildingBlock(context, list2, list4, list6, mapAreaId, mapBlockId, buildingAreaWidth, width));
				if (1 == 0)
				{
				}
				num5 = num6;
				if (num5 < 0)
				{
					Logger.AppendWarning($"Failed to create building {buildingBlockItem2.Name} at {BuildingBlock.Instance[centerBuilding].Name}({centerBuilding}): no available space.");
					continue;
				}
			}
			list7.Clear();
			buildingAreaData.GetNeighborBlocks(num5, width, list7);
			list5.AddRange(list7);
			GetAvailableRootBlocks(list7, list6, mapAreaId, mapBlockId, buildingAreaWidth, 2, clearFirst: false);
			level = (sbyte)Math.Clamp(random.Next(centerBuildingMaxLevel / 2, centerBuildingMaxLevel + 1), 1, buildingBlockItem2.MaxLevel);
			AddBuilding(context, mapAreaId, mapBlockId, num5, num4, level, buildingAreaWidth);
			for (int l = 0; l < width; l++)
			{
				for (int m = 0; m < width; m++)
				{
					short item2 = (short)(num5 + l * buildingAreaWidth + m);
					if (list3.Contains(item2))
					{
						list3.Remove(item2);
					}
					list.Remove(item2);
					list2.Remove(item2);
				}
			}
		}
		ObjectPool<List<short>>.Instance.Return(list8);
		if (elementId.AreaId == 138)
		{
			short num7 = (short)(274 + context.Random.Next(7));
			width = BuildingBlock.Instance[num7].Width;
			if (1 == 0)
			{
			}
			short num6 = ((width != 2) ? GetRandomAvailableBuildingBlock(context, list2, list3, list5, mapAreaId, mapBlockId, buildingAreaWidth, width) : GetRandomAvailableBuildingBlock(context, list2, list4, list6, mapAreaId, mapBlockId, buildingAreaWidth, width));
			if (1 == 0)
			{
			}
			short num8 = num6;
			if (num8 < 0)
			{
				Logger.AppendWarning($"Failed to create building {BuildingBlock.Instance[num7].Name} at {BuildingBlock.Instance[centerBuilding].Name}({centerBuilding}): no available space.");
			}
			list7.Clear();
			buildingAreaData.GetNeighborBlocks(num8, width, list7);
			list5.AddRange(list7);
			GetAvailableRootBlocks(list7, list6, mapAreaId, mapBlockId, buildingAreaWidth, 2, clearFirst: false);
			level = (sbyte)Math.Max(random.Next(centerBuildingMaxLevel / 2, centerBuildingMaxLevel + 1), 1);
			AddBuilding(context, mapAreaId, mapBlockId, num8, num7, level, buildingAreaWidth);
			for (int n = 0; n < width; n++)
			{
				for (int num9 = 0; num9 < width; num9++)
				{
					short item3 = (short)(num8 + n * buildingAreaWidth + num9);
					if (list3.Contains(item3))
					{
						list3.Remove(item3);
					}
					list.Remove(item3);
					list2.Remove(item3);
				}
			}
		}
		List<short> list9 = ObjectPool<List<short>>.Instance.Get();
		list9.Clear();
		list9.AddRange(mapBlockItem.RandomBuildingList);
		list9.Sort(ComparePresetBuildings);
		for (short num10 = 0; num10 < list9.Count; num10++)
		{
			int num11 = random.Next(0, 2);
			if (num11 != 1)
			{
				continue;
			}
			short num12 = list9[num10];
			width = BuildingBlock.Instance[num12].Width;
			short num13;
			if (BuildingBlockData.IsResource(BuildingBlock.Instance[num12].Type))
			{
				num13 = list2[random.Next(0, list2.Count)];
			}
			else
			{
				if (1 == 0)
				{
				}
				short num6 = ((width != 2) ? GetRandomAvailableBuildingBlock(context, list2, list3, list5, mapAreaId, mapBlockId, buildingAreaWidth, width) : GetRandomAvailableBuildingBlock(context, list2, list4, list6, mapAreaId, mapBlockId, buildingAreaWidth, width));
				if (1 == 0)
				{
				}
				num13 = num6;
				if (num13 < 0)
				{
					Logger.AppendWarning($"Failed to create building {BuildingBlock.Instance[num12].Name} at {BuildingBlock.Instance[centerBuilding].Name}({centerBuilding}): no available space.");
					continue;
				}
			}
			list7.Clear();
			buildingAreaData.GetNeighborBlocks(num13, width, list7);
			list5.AddRange(list7);
			GetAvailableRootBlocks(list7, list6, mapAreaId, mapBlockId, buildingAreaWidth, 2, clearFirst: false);
			level = (sbyte)Math.Max(random.Next(centerBuildingMaxLevel / 2, centerBuildingMaxLevel + 1), 1);
			BuildingBlockKey buildingBlockKey2 = new BuildingBlockKey(mapAreaId, mapBlockId, num13);
			if (_buildingBlocks.ContainsKey(buildingBlockKey2))
			{
				BuildingBlockData buildingBlockData = GetBuildingBlockData(buildingBlockKey2);
				AddBuilding(context, mapAreaId, mapBlockId, num13, num12, level, buildingAreaWidth);
			}
			AddBuilding(context, mapAreaId, mapBlockId, num13, num12, level, buildingAreaWidth);
			for (int num14 = 0; num14 < width; num14++)
			{
				for (int num15 = 0; num15 < width; num15++)
				{
					short item4 = (short)(num13 + num14 * buildingAreaWidth + num15);
					if (list3.Contains(item4))
					{
						list3.Remove(item4);
					}
					list.Remove(item4);
					list2.Remove(item4);
				}
			}
		}
		ObjectPool<List<short>>.Instance.Return(list9);
		if (mapBlockTemplateId == 0)
		{
			sbyte width2 = BuildingBlock.Instance[mapBlockItem.CenterBuilding].Width;
			for (int num16 = -1; num16 < width2 + 1; num16++)
			{
				for (int num17 = -1; num17 < width2 + 1; num17++)
				{
					short item5 = (short)(centerBlockIndex + num17 * buildingAreaWidth + num16);
					list2.Remove(item5);
				}
			}
		}
		BuildingFormulaItem formula = ((mapBlockItem2.TemplateId == 0) ? BuildingFormula.DefValue.TaiwuVillageResourceInitLevel : BuildingFormula.DefValue.NonTaiwuVillageResourceInitLevel);
		for (int num18 = 0; num18 < landFormTypeItem.NormalResource.Length; num18++)
		{
			int num19 = landFormTypeItem.NormalResource[num18] * (int)((float)num / 1.6f) / 100;
			if (num19 <= 0)
			{
				continue;
			}
			short templateId = (short)(1 + num18);
			for (int num20 = 1; num20 <= num19; num20++)
			{
				if (num20 >= 5 || random.CheckPercentProb(num20 * 20))
				{
					short num21 = list2[random.Next(0, list2.Count)];
					sbyte level2 = (sbyte)formula.Calculate();
					AddElement_BuildingBlocks(new BuildingBlockKey(mapAreaId, mapBlockId, num21), new BuildingBlockData(num21, templateId, level2, -1), context);
					list.Remove(num21);
					list2.Remove(num21);
				}
			}
		}
		for (int num22 = 0; num22 < landFormTypeItem.SpecialResource.Length; num22++)
		{
			int num23 = landFormTypeItem.SpecialResource[num22] * (int)((float)num / 1.6f) / 100;
			if (num23 <= 0)
			{
				continue;
			}
			short templateId2 = (short)(11 + num22);
			for (int num24 = 1; num24 <= num23; num24++)
			{
				if (num24 >= 5 || random.CheckPercentProb(num24 * 20))
				{
					short num25 = list2[random.Next(0, list2.Count)];
					sbyte level3 = (sbyte)formula.Calculate();
					AddElement_BuildingBlocks(new BuildingBlockKey(mapAreaId, mapBlockId, num25), new BuildingBlockData(num25, templateId2, level3, -1), context);
					list.Remove(num25);
					list2.Remove(num25);
				}
			}
		}
		BuildingFormulaItem uselessResourceInitLevel = BuildingFormula.DefValue.UselessResourceInitLevel;
		if (mapBlockItem2.TemplateId == 0)
		{
			for (int num26 = 0; num26 < 3; num26++)
			{
				short templateId3 = (short)(21 + num26);
				for (int num27 = 1; num27 <= 10; num27++)
				{
					if (list2.Count < 1)
					{
						break;
					}
					short num28 = list2[random.Next(0, list2.Count)];
					sbyte level4 = (sbyte)uselessResourceInitLevel.Calculate();
					AddElement_BuildingBlocks(new BuildingBlockKey(mapAreaId, mapBlockId, num28), new BuildingBlockData(num28, templateId3, level4, -1), context);
					list.Remove(num28);
					list2.Remove(num28);
				}
			}
		}
		for (short num29 = 0; num29 < list.Count; num29++)
		{
			BuildingBlockKey elementId2 = new BuildingBlockKey(mapAreaId, mapBlockId, list[num29]);
			AddElement_BuildingBlocks(elementId2, new BuildingBlockData(list[num29], 0, -1, -1), context);
		}
		AddElement_BuildingAreas(elementId, buildingAreaData, context);
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(list2);
		ObjectPool<List<short>>.Instance.Return(list3);
		ObjectPool<List<short>>.Instance.Return(list4);
		ObjectPool<List<short>>.Instance.Return(list5);
		ObjectPool<List<short>>.Instance.Return(list6);
		ObjectPool<List<short>>.Instance.Return(list7);
	}

	private int ComparePresetBuildings(short templateIdA, short templateIdB)
	{
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[templateIdA];
		BuildingBlockItem buildingBlockItem2 = BuildingBlock.Instance[templateIdB];
		return buildingBlockItem.DependBuildings.Count.CompareTo(buildingBlockItem2.DependBuildings.Count);
	}

	public void AddTaiwuBuildingArea(DataContext context, Location location)
	{
		_taiwuBuildingAreas.Add(location);
		SetTaiwuBuildingAreas(_taiwuBuildingAreas, context);
		InitializeResidences(context, location);
		InitializeComfortableHouses(context, location);
		InitializeBuildingExtraData(context, location);
		DomainManager.Extra.InitializeResourceBlockExtraData(context, location);
	}

	private void InitializeBuildingExtraData(DataContext context, Location location)
	{
		IEnumerable<BuildingBlockData> buildingBlocksAtLocation = GetBuildingBlocksAtLocation(location);
		foreach (BuildingBlockData item in buildingBlocksAtLocation)
		{
			BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, item.BlockIndex);
			if (item.TemplateId == 44)
			{
				DomainManager.Extra.ModifyBuildingExtraData(context, blockKey, HandleTaiwuVillageInitUnlockSlot);
			}
			else if (item.TemplateId >= 0 && item.ConfigData.Class == EBuildingBlockClass.BornResource)
			{
				DomainManager.Extra.ModifyBuildingExtraData(context, blockKey, HandleResourceLevel);
			}
			else
			{
				DomainManager.Extra.ModifyBuildingExtraData(context, blockKey);
			}
		}
		void HandleResourceLevel(BuildingBlockDataEx extraData)
		{
			BuildingBlockData buildingBlockData = GetBuildingBlockData(extraData.Key);
			for (int i = 0; i < buildingBlockData.Level; i++)
			{
				extraData.UnlockLevelSlot(i);
			}
		}
		void HandleTaiwuVillageInitUnlockSlot(BuildingBlockDataEx extraData)
		{
			sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
			sbyte sectID = MapState.Instance[stateTemplateIdByAreaId].SectID;
			sbyte largeSectIndex = OrganizationDomain.GetLargeSectIndex(sectID);
			extraData.ResetInitialUnlockedSlot(largeSectIndex);
		}
	}

	private short GetRandomAvailableBuildingBlock(DataContext context, List<short> allValidBlocks, List<short> indexes, List<short> backupIndexes, short areaId, short blockId, sbyte areaWidth, sbyte buildingWidth)
	{
		RemoveUnavailableBuildingBlocks(allValidBlocks, indexes, areaId, blockId, areaWidth, buildingWidth);
		short result;
		if (indexes.Count == 0)
		{
			RemoveUnavailableBuildingBlocks(allValidBlocks, backupIndexes, areaId, blockId, areaWidth, buildingWidth);
			if (backupIndexes.Count == 0)
			{
				return -1;
			}
			result = backupIndexes[context.Random.Next(0, backupIndexes.Count)];
		}
		else
		{
			result = indexes[context.Random.Next(0, indexes.Count)];
		}
		return result;
	}

	public void RemoveUnavailableBuildingBlocks(List<short> validBlockList, List<short> buildingBlocks, short areaId, short blockId, sbyte areaWidth, sbyte buildingWidth)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		foreach (short buildingBlock in buildingBlocks)
		{
			if (IsAllBlocksInValidBlockList(validBlockList, buildingBlock, areaWidth, buildingWidth))
			{
				list.Add(buildingBlock);
			}
		}
		buildingBlocks.Clear();
		buildingBlocks.AddRange(list);
		ObjectPool<List<short>>.Instance.Return(list);
	}

	private void GetAvailableRootBlocks(List<short> buildingBlocks, List<short> availableIndexes, short areaId, short blockId, sbyte areaWidth, sbyte buildingWidth, bool clearFirst = true)
	{
		if (clearFirst)
		{
			availableIndexes.Clear();
		}
		foreach (short buildingBlock in buildingBlocks)
		{
			for (int i = 0; i < buildingWidth; i++)
			{
				for (int j = 0; j < buildingWidth; j++)
				{
					short num = (short)(buildingBlock - i * areaWidth - j);
					if (IsBuildingBlocksEmpty(areaId, blockId, num, areaWidth, buildingWidth) && !availableIndexes.Contains(num))
					{
						availableIndexes.Add(num);
					}
				}
			}
		}
	}

	private bool IsAllBlocksInValidBlockList(List<short> allValidBlock, short rootIndex, sbyte areaWidth, sbyte buildingWidth)
	{
		int num = rootIndex % areaWidth;
		int num2 = rootIndex / areaWidth;
		int num3 = 0;
		for (int i = 0; i < buildingWidth && num + i < areaWidth; i++)
		{
			for (int j = 0; j < buildingWidth && num2 + j < areaWidth; j++)
			{
				short item = (short)(rootIndex + i * areaWidth + j);
				if (!allValidBlock.Contains(item))
				{
					return false;
				}
				num3++;
			}
		}
		if (num3 == buildingWidth * buildingWidth)
		{
			return true;
		}
		return false;
	}

	private bool IsLandFormTypeRandom(MapBlockItem config)
	{
		if ((config.SubType == EMapBlockSubType.Village || config.SubType == EMapBlockSubType.Town || config.SubType == EMapBlockSubType.WalledTown) && config.LandFormType == -1 && config.BuildingAreaWidth != -1)
		{
			return true;
		}
		return false;
	}

	private void InitializeCricketCollection()
	{
		for (int i = 0; i < 15; i++)
		{
			_collectionCrickets[i] = ItemKey.Invalid;
			_collectionCricketJars[i] = ItemKey.Invalid;
			_collectionCricketRegen[i] = 0;
		}
	}

	[DomainMethod]
	public void CricketCollectionAdd(DataContext context, int index, bool isCricket, ItemKey itemKey)
	{
		Tester.Assert(itemKey.Id >= 0);
		Tester.Assert(index >= 0 && index < 17);
		List<CricketCollectionData> cricketCollectionDataList = DomainManager.Extra.GetCricketCollectionDataList();
		if (DomainManager.Taiwu.GetWarehouseAllItemKey().Contains(itemKey))
		{
			DomainManager.Taiwu.RemoveItem(context, itemKey, 1, 2, deleteItem: false);
		}
		else
		{
			DomainManager.Taiwu.RemoveItem(context, itemKey, 1, 1, deleteItem: false);
		}
		if (isCricket)
		{
			cricketCollectionDataList[index].Cricket = itemKey;
			DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, 44);
		}
		else
		{
			cricketCollectionDataList[index].CricketJar = itemKey;
			DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, 44);
		}
		cricketCollectionDataList[index].CricketRegen = 0;
		DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionDataList, context);
	}

	[DomainMethod]
	public void CricketCollectionRemove(DataContext context, int index, bool isCricket)
	{
		Tester.Assert(index >= 0 && index < 17);
		List<CricketCollectionData> cricketCollectionDataList = DomainManager.Extra.GetCricketCollectionDataList();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (isCricket)
		{
			ItemKey cricket = cricketCollectionDataList[index].Cricket;
			OfflineRemoveACricket(context, cricket, cricketCollectionDataList, index, ItemSourceType.Inventory);
		}
		else
		{
			ItemKey cricketJar = cricketCollectionDataList[index].CricketJar;
			OfflineRemoveAJar(context, cricketJar, cricketCollectionDataList, index, ItemSourceType.Inventory);
		}
		cricketCollectionDataList[index].CricketRegen = 0;
		DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionDataList, context);
	}

	[DomainMethod]
	public void CricketCollectionBatchRemoveCricket(DataContext context, ItemSourceType sourceType)
	{
		List<CricketCollectionData> cricketCollectionDataList = DomainManager.Extra.GetCricketCollectionDataList();
		for (int i = 0; i < cricketCollectionDataList.Count; i++)
		{
			ItemKey cricket = cricketCollectionDataList[i].Cricket;
			if (cricket.IsValid())
			{
				OfflineRemoveACricket(context, cricket, cricketCollectionDataList, i, sourceType);
				cricketCollectionDataList[i].CricketRegen = 0;
			}
		}
		DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionDataList, context);
	}

	[DomainMethod]
	public void CricketCollectionBatchRemoveJar(DataContext context, ItemSourceType sourceType)
	{
		CricketCollectionBatchRemoveCricket(context, sourceType);
		List<CricketCollectionData> cricketCollectionDataList = DomainManager.Extra.GetCricketCollectionDataList();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		for (int i = 0; i < cricketCollectionDataList.Count; i++)
		{
			ItemKey cricketJar = cricketCollectionDataList[i].CricketJar;
			if (cricketJar.IsValid())
			{
				OfflineRemoveAJar(context, cricketJar, cricketCollectionDataList, i, sourceType);
			}
		}
		DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionDataList, context);
	}

	private void OfflineRemoveAJar(DataContext context, ItemKey itemKey, List<CricketCollectionData> cricketCollectionData, int i, ItemSourceType sourceType)
	{
		DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Building, 44);
		if (!DomainManager.Taiwu.CanTransferItemToWarehouse(context) && sourceType == ItemSourceType.Inventory)
		{
			sourceType = ItemSourceType.Warehouse;
		}
		DomainManager.Taiwu.AddItem(context, itemKey, 1, (sbyte)sourceType);
		cricketCollectionData[i].CricketJar = ItemKey.Invalid;
	}

	private void OfflineRemoveACricket(DataContext context, ItemKey itemKey, List<CricketCollectionData> cricketCollectionData, int i, ItemSourceType sourceType)
	{
		DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Building, 44);
		if (!DomainManager.Taiwu.CanTransferItemToWarehouse(context) && sourceType == ItemSourceType.Inventory)
		{
			sourceType = ItemSourceType.Warehouse;
		}
		DomainManager.Taiwu.AddItem(context, itemKey, 1, (sbyte)sourceType);
		cricketCollectionData[i].Cricket = ItemKey.Invalid;
	}

	private bool IsCricket(KeyValuePair<ItemKey, int> pair)
	{
		short itemSubType = ItemTemplateHelper.GetItemSubType(pair.Key.ItemType, pair.Key.TemplateId);
		return itemSubType == 1100;
	}

	private bool IsCricketJar(KeyValuePair<ItemKey, int> pair)
	{
		short itemSubType = ItemTemplateHelper.GetItemSubType(pair.Key.ItemType, pair.Key.TemplateId);
		return itemSubType == 1201;
	}

	[DomainMethod]
	public void CricketCollectionBatchAddCricket(DataContext context)
	{
		bool canTransfer = DomainManager.Taiwu.CanTransferItemToWarehouse(context);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		List<ItemWithSource> list = new List<ItemWithSource>();
		List<KeyValuePair<ItemKey, int>> inventoryCrickets = GetInventoryCrickets(canTransfer, taiwu);
		List<KeyValuePair<ItemKey, int>> warehouseCrickets = GetWarehouseCrickets();
		List<(List<KeyValuePair<ItemKey, int>>, ItemSourceType)> list2 = new List<(List<KeyValuePair<ItemKey, int>>, ItemSourceType)>
		{
			(inventoryCrickets, ItemSourceType.Inventory),
			(warehouseCrickets, ItemSourceType.Warehouse)
		};
		for (int i = 0; i < list2.Count; i++)
		{
			for (int j = 0; j < list2[i].Item1.Count; j++)
			{
				list.Add(new ItemWithSource
				{
					ItemKey = list2[i].Item1[j].Key,
					Amount = list2[i].Item1[j].Value,
					Source = i,
					IndexInSource = j
				});
			}
		}
		list.Sort(Compare);
		List<CricketCollectionData> cricketCollectionDataList = DomainManager.Extra.GetCricketCollectionDataList();
		int num = 0;
		for (int k = 0; k < cricketCollectionDataList.Count; k++)
		{
			ItemKey cricket = cricketCollectionDataList[k].Cricket;
			ItemKey cricketJar = cricketCollectionDataList[k].CricketJar;
			if (cricketJar.IsValid() && !cricket.IsValid() && num <= list.Count - 1)
			{
				ItemWithSource value = list[num];
				ItemKey key = list2[value.Source].Item1[value.IndexInSource].Key;
				DomainManager.Taiwu.RemoveItem(context, key, 1, (sbyte)list2[value.Source].Item2, deleteItem: false);
				value.Amount--;
				list[num] = value;
				cricketCollectionDataList[k].Cricket = key;
				DomainManager.Item.SetOwner(key, ItemOwnerType.Building, 44);
				cricketCollectionDataList[k].CricketRegen = 0;
				if (value.Amount == 0)
				{
					num++;
				}
			}
		}
		DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionDataList, context);
		static int Compare(ItemWithSource a, ItemWithSource b)
		{
			GameData.Domains.Item.Cricket element_Crickets = DomainManager.Item.GetElement_Crickets(a.ItemKey.Id);
			GameData.Domains.Item.Cricket element_Crickets2 = DomainManager.Item.GetElement_Crickets(b.ItemKey.Id);
			if (element_Crickets == null || element_Crickets2 == null)
			{
				if (element_Crickets == null && element_Crickets2 == null)
				{
					return 0;
				}
				if (element_Crickets == null)
				{
					return -1;
				}
				if (element_Crickets2 == null)
				{
					return 1;
				}
			}
			short currDurability = element_Crickets.GetCurrDurability();
			short currDurability2 = element_Crickets2.GetCurrDurability();
			float num2 = (float)currDurability / (float)element_Crickets.GetMaxDurability();
			float value2 = (float)currDurability2 / (float)element_Crickets2.GetMaxDurability();
			if (currDurability == 0 && currDurability2 > 0)
			{
				return 1;
			}
			if (currDurability2 == 0 && currDurability > 0)
			{
				return -1;
			}
			int num3 = num2.CompareTo(value2);
			if (num3 != 0)
			{
				return num3;
			}
			sbyte grade = element_Crickets.GetGrade();
			int num4 = element_Crickets2.GetGrade().CompareTo(grade);
			if (num4 != 0)
			{
				return num4;
			}
			return a.ItemKey.Id.CompareTo(b.ItemKey.Id);
		}
	}

	private List<KeyValuePair<ItemKey, int>> GetTroughCrickets()
	{
		return DomainManager.Extra.TroughItems.Where(IsCricket).ToList();
	}

	private List<KeyValuePair<ItemKey, int>> GetWarehouseCrickets()
	{
		return DomainManager.Taiwu.WarehouseItems.Where(IsCricket).ToList();
	}

	private static List<KeyValuePair<ItemKey, int>> GetInventoryCrickets(bool canTransfer, GameData.Domains.Character.Character taiwuChar)
	{
		List<KeyValuePair<ItemKey, int>> list = new List<KeyValuePair<ItemKey, int>>();
		if (canTransfer)
		{
			List<ItemDisplayData> inventoryItems = DomainManager.Character.GetInventoryItems(taiwuChar.GetId(), 1100);
			foreach (ItemDisplayData item in inventoryItems)
			{
				list.Add(new KeyValuePair<ItemKey, int>(item.Key, item.Amount));
			}
		}
		return list;
	}

	[DomainMethod]
	public void CricketCollectionBatchAddCricketJar(DataContext context)
	{
		bool canTransfer = DomainManager.Taiwu.CanTransferItemToWarehouse(context);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		List<ItemWithSource> list = new List<ItemWithSource>();
		List<KeyValuePair<ItemKey, int>> inventoryCricketJars = GetInventoryCricketJars(canTransfer, taiwu);
		List<KeyValuePair<ItemKey, int>> warehouseCricketJars = GetWarehouseCricketJars();
		List<(List<KeyValuePair<ItemKey, int>>, ItemSourceType)> list2 = new List<(List<KeyValuePair<ItemKey, int>>, ItemSourceType)>
		{
			(inventoryCricketJars, ItemSourceType.Inventory),
			(warehouseCricketJars, ItemSourceType.Warehouse)
		};
		for (int i = 0; i < list2.Count; i++)
		{
			for (int j = 0; j < list2[i].Item1.Count; j++)
			{
				list.Add(new ItemWithSource
				{
					ItemKey = list2[i].Item1[j].Key,
					Amount = list2[i].Item1[j].Value,
					Source = i,
					IndexInSource = j
				});
			}
		}
		list.Sort(Compare);
		List<CricketCollectionData> cricketCollectionDataList = DomainManager.Extra.GetCricketCollectionDataList();
		int num = 0;
		for (int k = 0; k < cricketCollectionDataList.Count; k++)
		{
			ItemKey cricketJar = cricketCollectionDataList[k].CricketJar;
			if (!cricketJar.IsValid() && num <= list.Count - 1)
			{
				ItemWithSource value = list[num];
				ItemKey key = list2[value.Source].Item1[value.IndexInSource].Key;
				DomainManager.Taiwu.RemoveItem(context, key, 1, (sbyte)list2[value.Source].Item2, deleteItem: false);
				value.Amount--;
				list[num] = value;
				cricketCollectionDataList[k].CricketJar = key;
				DomainManager.Item.SetOwner(key, ItemOwnerType.Building, 44);
				cricketCollectionDataList[k].CricketRegen = 0;
				if (value.Amount == 0)
				{
					num++;
				}
			}
		}
		DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionDataList, context);
		static int Compare(ItemWithSource a, ItemWithSource b)
		{
			GameData.Domains.Item.Misc element_Misc = DomainManager.Item.GetElement_Misc(a.ItemKey.Id);
			GameData.Domains.Item.Misc element_Misc2 = DomainManager.Item.GetElement_Misc(b.ItemKey.Id);
			sbyte grade = element_Misc.GetGrade();
			int num2 = element_Misc2.GetGrade().CompareTo(grade);
			if (num2 != 0)
			{
				return num2;
			}
			return a.ItemKey.Id.CompareTo(b.ItemKey.Id);
		}
	}

	private List<KeyValuePair<ItemKey, int>> GetTroughCricketJars()
	{
		return DomainManager.Extra.TroughItems.Where(IsCricketJar).ToList();
	}

	private List<KeyValuePair<ItemKey, int>> GetWarehouseCricketJars()
	{
		return DomainManager.Taiwu.WarehouseItems.Where(IsCricketJar).ToList();
	}

	private static List<KeyValuePair<ItemKey, int>> GetInventoryCricketJars(bool canTransfer, GameData.Domains.Character.Character taiwuChar)
	{
		List<KeyValuePair<ItemKey, int>> list = new List<KeyValuePair<ItemKey, int>>();
		if (canTransfer)
		{
			List<ItemDisplayData> inventoryItems = DomainManager.Character.GetInventoryItems(taiwuChar.GetId(), 1201);
			foreach (ItemDisplayData item in inventoryItems)
			{
				list.Add(new KeyValuePair<ItemKey, int>(item.Key, item.Amount));
			}
		}
		return list;
	}

	[DomainMethod]
	public List<ItemDisplayData> GetCricketOrJarFromSourceStorage(DataContext context, short itemSubType, ItemSourceType sourceType)
	{
		if (itemSubType != 1100 && itemSubType != 1201)
		{
			throw new ArgumentException("itemSubType must be Cricket or CricketJar");
		}
		if (sourceType != ItemSourceType.Inventory && sourceType != ItemSourceType.Warehouse && sourceType != ItemSourceType.Trough)
		{
			throw new ArgumentException("sourceType must be Inventory or Warehouse or Trough");
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		List<ItemDisplayData> result = null;
		bool flag = DomainManager.Taiwu.CanTransferItemToWarehouse(context);
		switch (sourceType)
		{
		case ItemSourceType.Inventory:
			if (!flag)
			{
				throw new InvalidOperationException("Cannot transfer item");
			}
			result = DomainManager.Character.GetInventoryItems(taiwu.GetId(), itemSubType);
			break;
		case ItemSourceType.Warehouse:
			result = DomainManager.Taiwu.GetWarehouseItemsBySubType(context, itemSubType);
			break;
		case ItemSourceType.Trough:
		{
			Dictionary<ItemKey, int> items = DomainManager.Extra.TroughItems.Where((KeyValuePair<ItemKey, int> pair) => ItemTemplateHelper.GetItemSubType(pair.Key.ItemType, pair.Key.TemplateId) == itemSubType).ToDictionary((KeyValuePair<ItemKey, int> pair) => pair.Key, (KeyValuePair<ItemKey, int> pair) => pair.Value);
			result = CharacterDomain.GetItemDisplayData(taiwu.GetId(), items, ItemSourceType.Trough);
			break;
		}
		}
		return result;
	}

	[DomainMethod]
	public void SmartOperateCricketOrJarCollection(DataContext context, int collectionIndex, short itemSubType, ItemSourceType sourceType, ItemKey itemKey)
	{
		List<CricketCollectionData> cricketCollectionDataList = DomainManager.Extra.GetCricketCollectionDataList();
		if (collectionIndex < 0 || collectionIndex >= cricketCollectionDataList.Count)
		{
			throw new ArgumentOutOfRangeException("collectionIndex", collectionIndex, null);
		}
		ItemKey cricket = cricketCollectionDataList[collectionIndex].Cricket;
		ItemKey cricketJar = cricketCollectionDataList[collectionIndex].CricketJar;
		switch (itemSubType)
		{
		case 1100:
			if (!cricketJar.IsValid())
			{
				throw new InvalidOperationException("No jar in the collection");
			}
			if (cricket.IsValid())
			{
				OfflineRemoveACricket(context, cricket, cricketCollectionDataList, collectionIndex, sourceType);
			}
			if (itemKey.IsValid())
			{
				cricketCollectionDataList[collectionIndex].Cricket = itemKey;
				DomainManager.Taiwu.RemoveItem(context, itemKey, 1, (sbyte)sourceType, deleteItem: false);
				DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, 44);
				cricketCollectionDataList[collectionIndex].CricketRegen = 0;
			}
			break;
		case 1201:
			if (cricketJar.IsValid())
			{
				OfflineRemoveAJar(context, cricketJar, cricketCollectionDataList, collectionIndex, sourceType);
			}
			if (itemKey.IsValid())
			{
				DomainManager.Taiwu.RemoveItem(context, itemKey, 1, (sbyte)sourceType, deleteItem: false);
				DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, 44);
				cricketCollectionDataList[collectionIndex].CricketJar = itemKey;
			}
			else if (cricket.IsValid())
			{
				OfflineRemoveACricket(context, cricket, cricketCollectionDataList, collectionIndex, sourceType);
			}
			break;
		default:
			throw new ArgumentException("itemSubType must be Cricket or CricketJar");
		}
		DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionDataList, context);
	}

	[DomainMethod]
	public CricketCollectionBatchButtonStateDisplayData GetBatchButtonEnableState(DataContext context)
	{
		bool hasCricketInCollection = false;
		bool hasEmptyJarInCollection = false;
		bool hasJarInCollection = false;
		bool hasEmptyPositionInCollection = false;
		List<CricketCollectionData> cricketCollectionDataList = DomainManager.Extra.GetCricketCollectionDataList();
		foreach (CricketCollectionData item in cricketCollectionDataList)
		{
			if (item.CricketJar.IsValid())
			{
				hasJarInCollection = true;
				if (item.Cricket.IsValid())
				{
					hasCricketInCollection = true;
				}
				else
				{
					hasEmptyJarInCollection = true;
				}
			}
			else
			{
				hasEmptyPositionInCollection = true;
			}
		}
		bool canTransfer = DomainManager.Taiwu.CanTransferItemToWarehouse(context);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		bool hasCricketInSources = GetInventoryCrickets(canTransfer, taiwu).Count > 0 || GetWarehouseCrickets().Count > 0;
		bool hasJarInSources = GetInventoryCricketJars(canTransfer, taiwu).Count > 0 || GetWarehouseCricketJars().Count > 0;
		return new CricketCollectionBatchButtonStateDisplayData
		{
			HasCricketInCollection = hasCricketInCollection,
			HasJarInCollection = hasJarInCollection,
			HasCricketInSources = hasCricketInSources,
			HasEmptyJarInCollection = hasEmptyJarInCollection,
			HasJarInSources = hasJarInSources,
			HasEmptyPositionInCollection = hasEmptyPositionInCollection
		};
	}

	[DomainMethod]
	public ItemDisplayData[] GetCollectionCrickets(DataContext context)
	{
		List<CricketCollectionData> cricketCollectionDataList = DomainManager.Extra.GetCricketCollectionDataList();
		ItemDisplayData[] array = new ItemDisplayData[cricketCollectionDataList.Count];
		for (int i = 0; i < array.Length; i++)
		{
			if (!cricketCollectionDataList[i].Cricket.Equals(ItemKey.Invalid))
			{
				array[i] = DomainManager.Item.GetItemDisplayData(cricketCollectionDataList[i].Cricket);
			}
			else
			{
				array[i] = new ItemDisplayData();
			}
		}
		return array;
	}

	[DomainMethod]
	public ItemDisplayData[] GetCollectionJars(DataContext context)
	{
		List<CricketCollectionData> cricketCollectionDataList = DomainManager.Extra.GetCricketCollectionDataList();
		ItemDisplayData[] array = new ItemDisplayData[cricketCollectionDataList.Count];
		for (int i = 0; i < array.Length; i++)
		{
			if (!cricketCollectionDataList[i].CricketJar.Equals(ItemKey.Invalid))
			{
				array[i] = DomainManager.Item.GetItemDisplayData(cricketCollectionDataList[i].CricketJar);
			}
			else
			{
				array[i] = new ItemDisplayData();
			}
		}
		return array;
	}

	[DomainMethod]
	public int[] GetCollectionCricketRegen(DataContext context)
	{
		List<CricketCollectionData> cricketCollectionDataList = DomainManager.Extra.GetCricketCollectionDataList();
		int[] array = new int[cricketCollectionDataList.Count];
		for (int i = 0; i < cricketCollectionDataList.Count; i++)
		{
			array[i] = cricketCollectionDataList[i].CricketRegen;
		}
		return array;
	}

	[DomainMethod]
	public int GetAuthorityGain(DataContext context)
	{
		return GetCricketAuthorityGain();
	}

	public static int GetCricketAuthorityGain()
	{
		List<CricketCollectionData> cricketCollectionDataList = DomainManager.Extra.GetCricketCollectionDataList();
		int num = 0;
		for (int i = 0; i < cricketCollectionDataList.Count; i++)
		{
			if (!cricketCollectionDataList[i].Cricket.Equals(ItemKey.Invalid))
			{
				if (!DomainManager.Item.TryGetElement_Crickets(cricketCollectionDataList[i].Cricket.Id, out var element))
				{
					Logger.AppendWarning($"Cannot get cricket with id {cricketCollectionDataList[i].Cricket} at the position {i} of cricket collection while getting authority gain.");
				}
				else
				{
					int num2 = ((element.GetColorData().Level >= element.GetPartsData().Level) ? element.GetColorData().Level : element.GetPartsData().Level);
					int winsCount = element.GetWinsCount();
					int lossesCount = element.GetLossesCount();
					num += Math.Max(winsCount - lossesCount, 0) * num2 * ((lossesCount <= 0) ? 100 : 50) / 50;
				}
			}
		}
		return num;
	}

	public void ApplyCricketAuthorityGain(DataContext context)
	{
		if (DomainManager.World.GetCurrMonthInYear() == GlobalConfig.Instance.CricketActiveStartMonth)
		{
			int authorityGain = GetAuthorityGain(context);
			if (authorityGain > 0)
			{
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				taiwu.ChangeResource(context, 7, authorityGain);
			}
		}
	}

	public void UpdateCricketRegenProgress(DataContext context)
	{
		List<CricketCollectionData> cricketCollectionDataList = DomainManager.Extra.GetCricketCollectionDataList();
		for (int i = 0; i < 17; i++)
		{
			if (cricketCollectionDataList[i].Cricket.Equals(ItemKey.Invalid))
			{
				continue;
			}
			GameData.Domains.Item.Cricket element_Crickets = DomainManager.Item.GetElement_Crickets(cricketCollectionDataList[i].Cricket.Id);
			short[] injuries = element_Crickets.GetInjuries();
			bool flag = injuries.Exist((short value) => value > 0);
			if (element_Crickets.GetCurrDurability() == 0 || (element_Crickets.GetCurrDurability() >= element_Crickets.GetMaxDurability() && !flag))
			{
				continue;
			}
			MiscItem miscItem = Config.Misc.Instance[cricketCollectionDataList[i].CricketJar.TemplateId];
			cricketCollectionDataList[i].CricketRegen++;
			if (cricketCollectionDataList[i].CricketRegen < SharedMethods.CalcCricketRegenTime(miscItem.Grade))
			{
				continue;
			}
			cricketCollectionDataList[i].CricketRegen = 0;
			element_Crickets.SetCurrDurability(Math.Min(element_Crickets.GetMaxDurability(), (short)(element_Crickets.GetCurrDurability() + 1)), context);
			if (!flag || !context.Random.CheckPercentProb(miscItem.CricketHealInjuryOdds))
			{
				continue;
			}
			List<int> list = ObjectPool<List<int>>.Instance.Get();
			list.Clear();
			for (int num = 0; num < injuries.Length; num++)
			{
				if (injuries[num] > 0)
				{
					list.Add(num);
				}
			}
			int num2 = list[context.Random.Next(list.Count)];
			injuries[num2] = (short)Math.Max(injuries[num2] - ((num2 >= 2) ? 1 : 5), 0);
			element_Crickets.SetInjuries(injuries, context);
			ObjectPool<List<int>>.Instance.Return(list);
		}
		DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionDataList, context);
	}

	public override void PackCrossArchiveGameData(CrossArchiveGameData crossArchiveGameData)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(taiwuVillageLocation);
		for (short num = 0; num < element_BuildingAreas.Width * element_BuildingAreas.Width; num++)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, num);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
			if (buildingBlockItem != null && buildingBlockItem.TemplateId >= 0)
			{
				bool isPawnShop = buildingBlockItem.TemplateId == 222;
				DataContext currentThreadDataContext = DataContextManager.GetCurrentThreadDataContext();
				if (TryGetElement_CollectBuildingEarningsData(buildingBlockKey, out var value))
				{
					List<ItemKey> fixBookInfoList = value.FixBookInfoList;
					if (fixBookInfoList != null && fixBookInfoList.Count > 0)
					{
						if (crossArchiveGameData.WarehouseItems == null)
						{
							crossArchiveGameData.WarehouseItems = new Dictionary<ItemKey, int>();
						}
						foreach (ItemKey fixBookInfo in value.FixBookInfoList)
						{
							if (fixBookInfo.IsValid())
							{
								crossArchiveGameData.PackWarehouseItem(fixBookInfo, 1);
							}
						}
						element_BuildingBlocks.OfflineResetShopProgress();
						value.FixBookInfoList.Clear();
					}
				}
				ClearBuildingBlockEarningsData(currentThreadDataContext, buildingBlockKey, isPawnShop);
				if (element_BuildingBlocks.OperationType != -1)
				{
					SetStopOperation(currentThreadDataContext, buildingBlockKey, stop: true);
				}
			}
		}
		foreach (CricketCollectionData item in crossArchiveGameData.CricketCollectionDatas = DomainManager.Extra.GetCricketCollectionDataList())
		{
			if (item.Cricket.IsValid())
			{
				DomainManager.Item.PackCrossArchiveItem(crossArchiveGameData, item.Cricket);
			}
			if (item.CricketJar.IsValid())
			{
				DomainManager.Item.PackCrossArchiveItem(crossArchiveGameData, item.CricketJar);
			}
		}
		crossArchiveGameData.TaiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		crossArchiveGameData.TaiwuVillageAreaData = GetBuildingAreaData(crossArchiveGameData.TaiwuVillageLocation);
		crossArchiveGameData.TaiwuVillageBlocks = GetBuildingBlockList(crossArchiveGameData.TaiwuVillageLocation);
		crossArchiveGameData.TaiwuVillageBlocksEx = DomainManager.Extra.GetBuildingExtraDataListForCrossArchive();
		crossArchiveGameData.Chicken = new Dictionary<int, Chicken>();
		if (_chicken != null)
		{
			foreach (KeyValuePair<int, Chicken> item2 in _chicken)
			{
				crossArchiveGameData.Chicken[item2.Key] = item2.Value;
			}
		}
		crossArchiveGameData.XiangshuIdInKungfuPracticeRoom = new List<sbyte>();
		crossArchiveGameData.XiangshuIdInKungfuPracticeRoom.AddRange(DomainManager.Extra.GetXiangshuIdInKungfuPracticeRoom());
		crossArchiveGameData.SamsaraPlatformAddCombatSkillQualifications = _samsaraPlatformAddCombatSkillQualifications;
		crossArchiveGameData.SamsaraPlatformAddLifeSkillQualifications = _samsaraPlatformAddLifeSkillQualifications;
		crossArchiveGameData.SamsaraPlatformAddMainAttributes = _samsaraPlatformAddMainAttributes;
	}

	public override void UnpackCrossArchiveGameData(DataContext context, CrossArchiveGameData crossArchiveGameData)
	{
		SetSamsaraPlatformAddCombatSkillQualifications(ref crossArchiveGameData.SamsaraPlatformAddCombatSkillQualifications, context);
		SetSamsaraPlatformAddLifeSkillQualifications(ref crossArchiveGameData.SamsaraPlatformAddLifeSkillQualifications, context);
		SetSamsaraPlatformAddMainAttributes(crossArchiveGameData.SamsaraPlatformAddMainAttributes, context);
	}

	public void UnpackCrossArchiveGameData_Building(DataContext context, CrossArchiveGameData crossArchiveGameData)
	{
		SetAllVillagerHomeless(context);
		Location taiwuVillageLocation = crossArchiveGameData.TaiwuVillageLocation;
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		if (taiwuVillageLocation != DomainManager.Taiwu.GetTaiwuVillageLocation())
		{
			return;
		}
		List<BuildingBlockKey> list = new List<BuildingBlockKey>();
		SetElement_BuildingAreas(taiwuVillageLocation, crossArchiveGameData.TaiwuVillageAreaData, context);
		Dictionary<BuildingBlockKey, BuildingBlockDataEx> dictionary = ((crossArchiveGameData.TaiwuVillageBlocksEx != null) ? crossArchiveGameData.TaiwuVillageBlocksEx.ToDictionary((BuildingBlockDataEx data) => data.Key) : new Dictionary<BuildingBlockKey, BuildingBlockDataEx>());
		foreach (BuildingBlockData taiwuVillageBlock in crossArchiveGameData.TaiwuVillageBlocks)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, taiwuVillageBlock.BlockIndex);
			SetElement_BuildingBlocks(buildingBlockKey, taiwuVillageBlock, context);
			if (dictionary.TryGetValue(buildingBlockKey, out var value))
			{
				DomainManager.Extra.ModifyBuildingExtraData(context, buildingBlockKey, value);
				if (taiwuVillageBlock.TemplateId == 44)
				{
					DomainManager.Extra.TriggerTaiwuVillageVowMonthlyEventFromCrossArchive(context, value);
				}
			}
			if (taiwuVillageBlock.TemplateId == 46)
			{
				QuickFillResidence(context, buildingBlockKey);
			}
			else if (taiwuVillageBlock.TemplateId == 47)
			{
				QuickFillComfortableHouse(context, buildingBlockKey);
			}
		}
		DomainManager.Extra.InitFeasts(context);
		DomainManager.Extra.InitializeResourceBlockExtraData(context, DomainManager.Taiwu.GetTaiwuVillageLocation());
		foreach (BuildingBlockKey item in list)
		{
			ResetAllChildrenBlocks(context, item, 0, 1);
		}
		crossArchiveGameData.TaiwuVillageBlocks = null;
		crossArchiveGameData.TaiwuVillageBlocksEx = null;
		List<CricketCollectionData> cricketCollectionDataList = DomainManager.Extra.GetCricketCollectionDataList();
		if (crossArchiveGameData.CricketCollectionDatas != null && crossArchiveGameData.CricketCollectionDatas.Count != 0)
		{
			for (int num = 0; num < crossArchiveGameData.CricketCollectionDatas.Count; num++)
			{
				CricketCollectionData cricketCollectionData = crossArchiveGameData.CricketCollectionDatas[num];
				if (cricketCollectionData.Cricket.IsValid())
				{
					cricketCollectionDataList[num].Cricket = DomainManager.Item.UnpackCrossArchiveItem(context, crossArchiveGameData, cricketCollectionData.Cricket);
					DomainManager.Item.SetOwner(cricketCollectionDataList[num].Cricket, ItemOwnerType.Building, 44);
				}
				if (cricketCollectionData.CricketJar.IsValid())
				{
					cricketCollectionDataList[num].CricketJar = DomainManager.Item.UnpackCrossArchiveItem(context, crossArchiveGameData, cricketCollectionData.CricketJar);
					DomainManager.Item.SetOwner(cricketCollectionDataList[num].CricketJar, ItemOwnerType.Building, 44);
				}
				cricketCollectionDataList[num].CricketRegen = cricketCollectionData.CricketRegen;
			}
			crossArchiveGameData.CricketCollectionDatas = null;
			DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionDataList, context);
		}
		InitBuildingEffect();
		DomainManager.World.SetWorldFunctionsStatus(context, 11);
		ClearChicken(context);
		if (crossArchiveGameData.Chicken != null)
		{
			int num2 = 0;
			foreach (KeyValuePair<int, Chicken> item2 in crossArchiveGameData.Chicken)
			{
				if (item2.Value.TemplateId != 63)
				{
					Chicken value2 = item2.Value;
					value2.Id = num2;
					AddElement_Chicken(num2, value2, context);
					num2++;
				}
			}
		}
		InitializeNextChickenId();
		crossArchiveGameData.Chicken = null;
		if (crossArchiveGameData.XiangshuIdInKungfuPracticeRoom != null)
		{
			DomainManager.Extra.SetXiangshuIdInKungfuPracticeRoom(crossArchiveGameData.XiangshuIdInKungfuPracticeRoom, context);
			crossArchiveGameData.XiangshuIdInKungfuPracticeRoom = null;
		}
	}

	[DomainMethod]
	public (short, BuildingBlockData) GmCmd_BuildImmediately(DataContext context, short buildingTemplateId, BuildingBlockKey blockKey, sbyte level)
	{
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(blockKey);
		BuildingRemoveVillagerWork(context, blockKey);
		BuildingRemoveResident(context, element_BuildingBlocks, blockKey);
		level = Math.Clamp(level, 1, BuildingBlock.Instance[buildingTemplateId].MaxLevel);
		ResetAllChildrenBlocks(context, blockKey, buildingTemplateId, level);
		DomainManager.Extra.ModifyBuildingExtraData(context, blockKey, delegate(BuildingBlockDataEx dataEx)
		{
			for (int i = 0; i < level; i++)
			{
				dataEx.UnlockLevelSlot(i);
			}
		});
		UpdateTaiwuVillageBuildingEffect();
		return (blockKey.BuildingBlockIndex, element_BuildingBlocks);
	}

	[DomainMethod]
	public (short, BuildingBlockData) GmCmd_RemoveBuildingImmediately(DataContext context, BuildingBlockKey blockKey)
	{
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(blockKey);
		BuildingRemoveVillagerWork(context, blockKey);
		BuildingRemoveResident(context, element_BuildingBlocks, blockKey);
		ResetAllChildrenBlocks(context, blockKey, 0, -1);
		UpdateTaiwuVillageBuildingEffect();
		DomainManager.Extra.ResetBuildingExtraData(context, blockKey);
		OnBuildingRemoved(context, blockKey);
		return (blockKey.BuildingBlockIndex, GetElement_BuildingBlocks(blockKey));
	}

	[DomainMethod]
	public void GmCmd_AddLegacyBuilding(DataContext context, short buildingTemplateId)
	{
		List<short> legaciesBuildingTemplateIdList = DomainManager.Extra.GetLegaciesBuildingTemplateIdList();
		legaciesBuildingTemplateIdList.Add(buildingTemplateId);
		DomainManager.Extra.SetLegaciesBuildingTemplateIdList(legaciesBuildingTemplateIdList, context);
	}

	[DomainMethod]
	public List<Chicken> GmCmd_GetChickenData()
	{
		List<Chicken> list = new List<Chicken>();
		list.AddRange(_chicken.Values);
		return list;
	}

	[DomainMethod]
	public bool GmCmd_BeatMinionPerform(DataContext context, sbyte grade, int repeat)
	{
		CombatResultDisplayData combatResultDisplayData = new CombatResultDisplayData
		{
			CombatStatus = CombatStatusType.EnemyFail
		};
		short templateId = (short)(298 + grade);
		int[] array = new int[1] { -1 };
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		List<ItemDisplayData> list = new List<ItemDisplayData>();
		for (int i = 0; i < repeat; i++)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.CreateRandomEnemy(context, templateId);
			int id = character.GetId();
			DomainManager.Character.CompleteCreatingCharacter(id);
			DomainManager.Adventure.AddTemporaryEnemyId(id);
			array[0] = id;
			CombatConfigItem combatConfigItem = CombatConfig.Instance[(short)1];
			CombatDomain.ResultCalcExp(combatConfigItem, isPlaygroundCombat: false, taiwu, array, combatResultDisplayData);
			CombatDomain.ResultCalcResource(combatConfigItem, isPlaygroundCombat: false, taiwu, array, combatResultDisplayData);
			CombatDomain.ResultCalcAreaSpiritualDebt(isWin: true, taiwu, array, combatResultDisplayData);
			CombatDomain.ResultCalcLootItem(context.Random, 50, combatConfigItem.CombatType, combatResultDisplayData.CombatStatus, isPuppetCombat: false, combatConfigItem, taiwu, array, Array.Empty<int>(), combatResultDisplayData);
			GameData.Domains.Character.Character character2 = character;
			ItemKey[] equipment = character2.GetEquipment();
			foreach (ItemDisplayData item in combatResultDisplayData.ItemList)
			{
				ItemKey key = item.Key;
				sbyte b = (sbyte)equipment.IndexOf(key);
				if (b >= 0)
				{
					character2.ChangeEquipment(context, b, -1, ItemKey.Invalid);
				}
				character2.RemoveInventoryItem(context, key, 1, deleteItem: false);
				list.Add(item);
			}
			combatResultDisplayData.ItemList.Clear();
		}
		ExtraDomain.MergeKeyForItemDisplayDataList(list);
		taiwu.AddInventoryItemList(context, list.Select((ItemDisplayData d) => d.Key).ToList());
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenGetItem_Item, list, arg2: false, arg3: false, arg4: true);
		return true;
	}

	[DomainMethod]
	public bool GmCmd_BuildingCollectPerform(DataContext context, int totalAttainment, short buildingTemplateId, int repeat)
	{
		BuildingBlockItem item = BuildingBlock.Instance.GetItem(buildingTemplateId);
		if (item == null || !item.DependBuildings.CheckIndex(0))
		{
			return false;
		}
		BuildingBlockData buildingBlockData = new BuildingBlockData
		{
			TemplateId = item.DependBuildings[0]
		};
		BuildingBlockItem item2 = BuildingBlock.Instance.GetItem(buildingBlockData.TemplateId);
		int num = 0;
		sbyte b = -1;
		for (sbyte b2 = 0; b2 < item2.CollectResourcePercent.Length; b2++)
		{
			if (item2.CollectResourcePercent[b2] > 0)
			{
				b = b2;
				break;
			}
		}
		List<ItemDisplayData> list = new List<ItemDisplayData>();
		for (int i = 0; i < repeat; i++)
		{
			BuildingProduceDependencyData invalid = BuildingProduceDependencyData.Invalid;
			invalid.Level = item.MaxLevel;
			invalid.TemplateId = buildingBlockData.TemplateId;
			invalid.BlockBaseYieldFactor = BuildingBaseYield(buildingBlockData).Get(Math.Min(b, 5));
			invalid.ResourceYieldLevelFactor = SharedMethods.GetBuildingLevelEffect(buildingBlockData.TemplateId, item2.MaxLevel);
			invalid.ProductivityFactor = GlobalConfig.Instance.CollectResourceBuildingProductivityDistanceOne[0];
			invalid.TotalAttainmentFactor = totalAttainment;
			invalid.GainResourcePercentFactor = DomainManager.World.GetGainResourcePercent(1);
			invalid.ResourceSingleOutputValuation = invalid.ResourceBuildingOutput;
			num += invalid.ResourceSingleOutputValuation;
			if (!item.IsShop || item.SuccesEvent.Count <= 0)
			{
				continue;
			}
			ShopEventItem shopEventItem = Config.ShopEvent.Instance[item.SuccesEvent[0]];
			if (shopEventItem.ItemList.Count <= 0)
			{
				continue;
			}
			List<TemplateKey> list2 = ObjectPool<List<TemplateKey>>.Instance.Get();
			list2.Clear();
			int num2 = 0;
			int num3 = shopEventItem.ItemList.Count;
			if (shopEventItem.ResourceList.Count > 1)
			{
				num3 = shopEventItem.ItemList.Count / 2;
			}
			for (int j = num2; j < num3; j++)
			{
				int percentProb = shopEventItem.ItemList[j].Amount + totalAttainment / 30;
				if (context.Random.CheckPercentProb(percentProb))
				{
					PresetInventoryItem presetInventoryItem = shopEventItem.ItemList[j];
					list2.Add(new TemplateKey(presetInventoryItem.Type, presetInventoryItem.TemplateId));
				}
			}
			if (list2.Count > 0)
			{
				TemplateKey templateKey = list2[context.Random.Next(0, list2.Count)];
				ItemKey itemKey = DomainManager.Item.CreateItem(context, templateKey.ItemType, templateKey.TemplateId);
				DomainManager.Taiwu.GetTaiwu().AddInventoryItem(context, itemKey, 1);
				int num4 = list.FindIndex((ItemDisplayData data) => data.Key.Equals(itemKey));
				if (num4 >= 0)
				{
					list[num4].Amount++;
				}
				else
				{
					ItemDisplayData itemDisplayData = DomainManager.Item.GetItemDisplayData(itemKey);
					list.Add(itemDisplayData);
				}
			}
			ObjectPool<List<TemplateKey>>.Instance.Return(list2);
		}
		ItemDisplayData item3 = new ItemDisplayData
		{
			Key = new ItemKey(12, 0, (short)(321 + b), -1),
			Amount = num
		};
		list.Add(item3);
		DomainManager.Taiwu.GetTaiwu().ChangeResource(context, b, num);
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenGetItem_Item, list, arg2: false, arg3: false, arg4: true);
		return true;
	}

	public void InitializeOwnedItems()
	{
		for (int i = 0; i < 15; i++)
		{
			DomainManager.Item.SetOwner(_collectionCrickets[i], ItemOwnerType.Building, 44);
			DomainManager.Item.SetOwner(_collectionCricketJars[i], ItemOwnerType.Building, 44);
		}
		foreach (KeyValuePair<BuildingBlockKey, BuildingEarningsData> collectBuildingEarningsDatum in _collectBuildingEarningsData)
		{
			if (!DomainManager.Building.TryGetElement_BuildingBlocks(collectBuildingEarningsDatum.Key, out var value))
			{
				continue;
			}
			foreach (ItemKey collectionItem in collectBuildingEarningsDatum.Value.CollectionItemList)
			{
				DomainManager.Item.SetOwner(collectionItem, ItemOwnerType.Building, value.TemplateId);
			}
			foreach (ItemKey shopSoldItem in collectBuildingEarningsDatum.Value.ShopSoldItemList)
			{
				DomainManager.Item.SetOwner(shopSoldItem, ItemOwnerType.Building, value.TemplateId);
			}
			foreach (ItemKey fixBookInfo in collectBuildingEarningsDatum.Value.FixBookInfoList)
			{
				DomainManager.Item.SetOwner(fixBookInfo, ItemOwnerType.Building, value.TemplateId);
			}
		}
		for (int j = 0; j < _teaHorseCaravanData.CarryGoodsList.Count; j++)
		{
			ItemKey item = _teaHorseCaravanData.CarryGoodsList[j].Item1;
			DomainManager.Item.SetOwner(item, ItemOwnerType.Building, 51);
		}
		for (int k = 0; k < _teaHorseCaravanData.ExchangeGoodsList.Count; k++)
		{
			ItemKey itemKey = _teaHorseCaravanData.ExchangeGoodsList[k];
			DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, 51);
		}
		List<CricketCollectionData> cricketCollectionDataList = DomainManager.Extra.GetCricketCollectionDataList();
		for (int l = 0; l < cricketCollectionDataList.Count; l++)
		{
			DomainManager.Item.SetOwner(cricketCollectionDataList[l].Cricket, ItemOwnerType.Building, 44);
			DomainManager.Item.SetOwner(cricketCollectionDataList[l].CricketJar, ItemOwnerType.Building, 44);
		}
		IReadOnlyDictionary<ulong, Feast> allFeasts = DomainManager.Extra.GetAllFeasts();
		foreach (Feast value2 in allFeasts.Values)
		{
			for (int m = 0; m < GlobalConfig.Instance.FeastCount; m++)
			{
				ItemKey dish = value2.GetDish(m);
				if (dish.IsValid())
				{
					DomainManager.Item.SetOwner(dish, ItemOwnerType.Building, 47);
				}
			}
			for (int n = 0; n < GlobalConfig.Instance.FeastGiftCount; n++)
			{
				ItemKey gift = value2.GetGift(n);
				if (gift.Id >= 0)
				{
					DomainManager.Item.SetOwner(gift, ItemOwnerType.Building, 47);
				}
			}
		}
	}

	[DomainMethod]
	public unsafe MakeItemData StartMakeItem(DataContext context, StartMakeArguments startMakeArguments)
	{
		int charId = startMakeArguments.CharId;
		BuildingBlockKey buildingBlockKey = startMakeArguments.BuildingBlockKey;
		ItemDisplayData tool = startMakeArguments.Tool;
		ItemDisplayData material = startMakeArguments.Material;
		sbyte itemType = startMakeArguments.ItemType;
		List<short> itemList = startMakeArguments.ItemList;
		short makeItemSubTypeId = startMakeArguments.MakeItemSubTypeId;
		ResourceInts resourceCount = startMakeArguments.ResourceCount;
		ResourceInts needResource = startMakeArguments.NeedResource;
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		int count = itemList.Count;
		if (tool.Key.IsValid())
		{
			sbyte grade = ItemTemplateHelper.GetGrade(material.Key.ItemType, material.Key.TemplateId);
			CraftToolItem craftToolItem = Config.CraftTool.Instance[tool.Key.TemplateId];
			int reduceValue = craftToolItem.DurabilityCost[grade] * count;
			DomainManager.Item.ReduceToolDurability(context, charId, tool.Key, reduceValue, tool.ItemSourceType);
		}
		if (charId == DomainManager.Taiwu.GetTaiwuCharId())
		{
			for (sbyte b = 0; b < 8; b++)
			{
				int num = needResource.Get(b);
				if (num > 0)
				{
					ConsumeResource(context, b, num);
				}
			}
		}
		else
		{
			needResource = needResource.GetReversed();
			element_Objects.ChangeResources(context, ref needResource);
		}
		DomainManager.Taiwu.RemoveItem(context, material.Key, count, material.ItemSourceType, deleteItem: true);
		MaterialResources materialResources = default(MaterialResources);
		for (int i = 0; i < 6; i++)
		{
			materialResources.Items[i] = (short)resourceCount.Items[i];
		}
		short time = MakeItemSubType.Instance[makeItemSubTypeId].Time;
		short leftTime = (short)(time * (count / 3 + 1));
		MakeItemData makeItemData = new MakeItemData
		{
			ProductItemIdList = itemList,
			ProductItemType = itemType,
			LeftTime = leftTime,
			ToolKey = tool.Key,
			MaterialKey = material.Key,
			MaterialResources = materialResources,
			IsPerfect = startMakeArguments.IsPerfect
		};
		AddElement_MakeItemDict(buildingBlockKey, makeItemData, context);
		return makeItemData;
	}

	[DomainMethod]
	public unsafe bool CheckMakeCondition(MakeConditionArguments makeConditionArguments)
	{
		int charId = makeConditionArguments.CharId;
		BuildingBlockKey buildingBlockKey = makeConditionArguments.BuildingBlockKey;
		ItemKey toolKey = makeConditionArguments.ToolKey;
		ItemKey materialKey = makeConditionArguments.MaterialKey;
		short makeCount = makeConditionArguments.MakeCount;
		ResourceInts resourceCount = makeConditionArguments.ResourceCount;
		short makeItemSubTypeId = makeConditionArguments.MakeItemSubTypeId;
		bool isManual = makeConditionArguments.IsManual;
		bool isPerfect = makeConditionArguments.IsPerfect;
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		ResourceInts needResources = default(ResourceInts);
		for (int i = 0; i < 8; i++)
		{
			short craftMaterialRequiredResourceAmount = ItemTemplateHelper.GetCraftMaterialRequiredResourceAmount(materialKey.TemplateId);
			needResources.Items[i] = resourceCount.Items[i] * makeCount * craftMaterialRequiredResourceAmount;
		}
		if (!((charId == DomainManager.Taiwu.GetTaiwuCharId()) ? GetAllTaiwuResources() : element_Objects.GetResources()).CheckIsMeet(ref needResources))
		{
			return false;
		}
		sbyte grade = ItemTemplateHelper.GetGrade(materialKey.ItemType, materialKey.TemplateId);
		CraftToolItem craftToolItem = Config.CraftTool.Instance[toolKey.TemplateId];
		short num = craftToolItem.DurabilityCost[grade];
		int totalCost = num * makeCount;
		if (!CheckTool(toolKey, totalCost, num, -1))
		{
			return false;
		}
		MaterialItem materialItem = Config.Material.Instance[materialKey.TemplateId];
		int effectValue = 0;
		if (!buildingBlockKey.IsInvalid)
		{
			effectValue = GetBuildingEffectForMake(buildingBlockKey, materialItem.RequiredLifeSkillType).attainmentEffect;
		}
		short makeRequiredLifeSkillAttainment = SharedMethods.GetMakeRequiredLifeSkillAttainment(materialKey.TemplateId, makeItemSubTypeId, isManual, isPerfect, effectValue);
		if (!CheckLifeSkill(materialItem.RequiredLifeSkillType, makeRequiredLifeSkillAttainment, toolKey))
		{
			return false;
		}
		return true;
	}

	[DomainMethod]
	public void TryShowNotifications()
	{
		if (_outsideMakeItem)
		{
			DomainManager.World.GetInstantNotifications().AddMakeItemOutsideSettlement();
			_outsideMakeItem = false;
		}
	}

	[DomainMethod]
	public List<ItemDisplayData> GetMakeItems(DataContext context, BuildingBlockKey buildingBlockKey)
	{
		MakeItemData element_MakeItemDict = GetElement_MakeItemDict(buildingBlockKey);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		List<ItemDisplayData> list = new List<ItemDisplayData>();
		RemoveElement_MakeItemDict(buildingBlockKey, context);
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		Location location = taiwu.GetLocation();
		bool flag = false;
		if (location == taiwuVillageLocation)
		{
			flag = true;
		}
		else
		{
			MapBlockData block = DomainManager.Map.GetBlock(location);
			if (block.IsCityTown())
			{
				flag = true;
			}
		}
		sbyte spawnSpecialEffectMultiplier = 2;
		Dictionary<short, int> dictionary = new Dictionary<short, int>();
		foreach (short productItemId in element_MakeItemDict.ProductItemIdList)
		{
			ItemKey productItemKey;
			switch (element_MakeItemDict.ProductItemType)
			{
			case 0:
				productItemKey = DomainManager.Item.CreateWeapon(context, productItemId, spawnSpecialEffectMultiplier);
				break;
			case 1:
				productItemKey = DomainManager.Item.CreateArmor(context, productItemId, spawnSpecialEffectMultiplier);
				break;
			case 2:
				productItemKey = DomainManager.Item.CreateAccessory(context, productItemId, spawnSpecialEffectMultiplier);
				break;
			default:
				productItemKey = DomainManager.Item.CreateItem(context, element_MakeItemDict.ProductItemType, productItemId);
				break;
			}
			ItemBase baseItem = DomainManager.Item.GetBaseItem(productItemKey);
			if (ItemType.IsEquipmentItemType(productItemKey.ItemType))
			{
				EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(productItemKey);
				baseEquipment.SetMaterialResources(element_MakeItemDict.MaterialResources, context);
				if (ItemType.IsEquipmentEffectType(productItemKey.ItemType) && element_MakeItemDict.IsPerfect)
				{
					List<short> list2 = EquipmentEffect.Instance.GetAllKeys().Where(delegate(short k)
					{
						EquipmentEffectItem equipmentEffectItem = EquipmentEffect.Instance[k];
						if (equipmentEffectItem.Special)
						{
							return false;
						}
						sbyte type = equipmentEffectItem.Type;
						if (1 == 0)
						{
						}
						bool result;
						switch (type)
						{
						case 0:
						{
							sbyte itemType = productItemKey.ItemType;
							bool flag2 = (uint)itemType <= 2u;
							result = flag2;
							break;
						}
						case 1:
							result = productItemKey.ItemType == 0;
							break;
						case 2:
							result = productItemKey.ItemType == 1;
							break;
						default:
							result = false;
							break;
						}
						if (1 == 0)
						{
						}
						return result;
					}).ToList();
					short num = -1;
					if (list2.Count > 0)
					{
						num = list2.GetRandom(context.Random);
					}
					baseEquipment.ApplyDurabilityEquipmentEffectChange(context, baseEquipment.GetEquipmentEffectId(), num);
					baseEquipment.SetEquipmentEffectId(num, context);
					baseEquipment.SetCurrDurability(baseEquipment.GetMaxDurability(), context);
				}
			}
			if (baseItem.GetStackable())
			{
				if (!dictionary.TryGetValue(productItemId, out var value))
				{
					dictionary.Add(productItemId, 1);
					list.Add(DomainManager.Item.GetItemDisplayData(baseItem, 1, DomainManager.Taiwu.GetTaiwuCharId(), -1));
				}
				else
				{
					ItemDisplayData itemDisplayData = list.Find((ItemDisplayData d) => d.Key.TemplateId == productItemId);
					if (itemDisplayData != null)
					{
						itemDisplayData.Amount = value + 1;
						dictionary[productItemId] = itemDisplayData.Amount;
					}
				}
			}
			else
			{
				list.Add(DomainManager.Item.GetItemDisplayData(baseItem, 1, DomainManager.Taiwu.GetTaiwuCharId(), -1));
			}
			ItemSourceType itemSourceType = DomainManager.Extra.GetBuildingDefaultStoreLocation().GetMakeType(-1);
			if (itemSourceType == ItemSourceType.Inventory && !DomainManager.Taiwu.CanTransferItemToWarehouse(context))
			{
				itemSourceType = ItemSourceType.Warehouse;
				_outsideMakeItem = true;
			}
			DomainManager.Taiwu.AddItem(context, productItemKey, 1, itemSourceType);
			if (baseItem.GetGrade() >= DomainManager.World.GetXiangshuLevel())
			{
				DomainManager.Taiwu.AddLegacyPoint(context, 30);
			}
			sbyte craftRequiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(productItemKey.ItemType, productItemKey.TemplateId);
			if (((uint)(craftRequiredLifeSkillType - 6) <= 1u || (uint)(craftRequiredLifeSkillType - 10) <= 1u) ? true : false)
			{
				int baseDelta = ProfessionFormulaImpl.Calculate(16, baseItem.GetGrade());
				DomainManager.Extra.ChangeProfessionSeniority(context, 2, baseDelta);
			}
			if (ItemTemplateHelper.GetItemSubType(productItemKey.ItemType, productItemKey.TemplateId) == 800)
			{
				sbyte grade = ItemTemplateHelper.GetGrade(productItemKey.ItemType, productItemKey.TemplateId);
				int baseDelta2 = ProfessionFormulaImpl.Calculate(82, grade);
				DomainManager.Extra.ChangeProfessionSeniority(context, 13, baseDelta2);
			}
		}
		return list;
	}

	[DomainMethod]
	public MakeItemData GetMakingItemData(BuildingBlockKey buildingBlockKey)
	{
		_makeItemDict.TryGetValue(buildingBlockKey, out var value);
		return value;
	}

	public void UpdateMakingProgressOnMonthChange(DataContext context)
	{
		foreach (KeyValuePair<BuildingBlockKey, MakeItemData> item in _makeItemDict)
		{
			if (item.Value.LeftTime != 0)
			{
				BuildingBlockData buildingBlockData = _buildingBlocks[item.Key];
				if (buildingBlockData.CanUse() && AllDependBuildingAvailable(item.Key, buildingBlockData.TemplateId, out var _))
				{
					item.Value.LeftTime--;
					SetElement_MakeItemDict(item.Key, item.Value, context);
				}
			}
		}
	}

	[DomainMethod]
	public MakeResult GetMakeResult(short materialTemplateId, ItemKey toolKey, BuildingBlockKey buildingBlockKey, sbyte lifeSkillType, List<short> makeItemSubtypeIdList, short makeItemSubTypeId = -1)
	{
		MakeItemSubTypeItem makeItemSubTypeItem = MakeItemSubType.Instance[makeItemSubtypeIdList.First()];
		sbyte itemType = makeItemSubTypeItem.Result.ItemType;
		bool makeIsManual = makeItemSubTypeId != -1;
		(int attainmentEffect, bool upgradeMakeItem) buildingEffectForMake = GetBuildingEffectForMake(buildingBlockKey, lifeSkillType);
		int item = buildingEffectForMake.attainmentEffect;
		bool item2 = buildingEffectForMake.upgradeMakeItem;
		short lifeSkillTotalAttainment = GetLifeSkillTotalAttainment(lifeSkillType, toolKey);
		List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = DomainManager.Taiwu.GetTaiwu().GetLearnedLifeSkills();
		GetMaterialGradeAndAttainment(materialTemplateId, itemType, lifeSkillType, lifeSkillTotalAttainment, makeItemSubtypeIdList, out var grade, out var requiredAttainment, learnedLifeSkills, makeItemSubTypeId, item);
		GetMakeResultStageArray(materialTemplateId, grade, requiredAttainment, lifeSkillTotalAttainment, makeItemSubtypeIdList, item, lifeSkillType, out var resultIndex, out var makeResultStageArray, makeItemSubTypeId, makeIsManual, item2);
		string upgradeBuildingName = string.Empty;
		BuildingBlock.Instance.Iterate(delegate(BuildingBlockItem buildingBlockItem)
		{
			if (buildingBlockItem.UpgradeMakeItem && buildingBlockItem.RequireLifeSkillType == lifeSkillType)
			{
				upgradeBuildingName = buildingBlockItem.Name;
				return false;
			}
			return true;
		});
		return new MakeResult(resultIndex, makeResultStageArray, upgradeBuildingName, item2);
	}

	private void GetMakeResultStageArray(short materialTemplateId, sbyte grade, int requiredAttainment, int totalAttainment, List<short> makeItemSubtypeIdList, int attainmentEffect, sbyte lifeSkillType, out int resultIndex, out MakeResultStage[] makeResultStageArray, short makeItemSubTypeId = -1, bool makeIsManual = false, bool upgradeMakeItem = false)
	{
		bool flag = makeIsManual || makeItemSubtypeIdList.Count == 1;
		if (makeItemSubTypeId == -1)
		{
			makeItemSubTypeId = makeItemSubtypeIdList.First();
		}
		MakeItemSubTypeItem makeItemSubTypeItem = MakeItemSubType.Instance[makeItemSubTypeId];
		sbyte itemType = makeItemSubTypeItem.Result.ItemType;
		short subTypeExtraLifeSkill = 0;
		if (makeIsManual)
		{
			subTypeExtraLifeSkill = (short)(makeItemSubTypeItem.ExtraLifeSkill * (grade + 1));
		}
		makeResultStageArray = new MakeResultStage[3];
		resultIndex = 0;
		for (int i = 0; i < 3; i++)
		{
			sbyte startGrade = grade;
			GetStageRequirementAndGrade(i, startGrade, itemType, requiredAttainment, subTypeExtraLifeSkill, out var targetGrade, out var targetRequirement);
			targetRequirement = Math.Max(0, targetRequirement);
			bool flag2 = totalAttainment >= targetRequirement;
			bool flag4;
			if (flag)
			{
				sbyte grade2 = ItemTemplateHelper.GetGrade(itemType, makeItemSubTypeItem.Result.TemplateId);
				(bool, sbyte, short) finalGradeAndId = GetFinalGradeAndId(grade2, targetGrade, makeItemSubTypeItem.Result.ItemType, makeItemSubTypeItem.Result.TemplateId);
				bool item = finalGradeAndId.Item1;
				sbyte item2 = finalGradeAndId.Item2;
				short finialTemplateId = finalGradeAndId.Item3;
				bool flag3 = makeResultStageArray.Exist((MakeResultStage s) => s.IsInit && s.TemplateId == finialTemplateId);
				flag4 = item && !flag3;
				if (flag4)
				{
					makeResultStageArray[i] = new MakeResultStage(targetRequirement, flag2, itemType, finialTemplateId, makeItemSubTypeId);
				}
			}
			else
			{
				List<short> list = new List<short>();
				List<short> list2 = new List<short>();
				foreach (short makeItemSubtypeId in makeItemSubtypeIdList)
				{
					MakeItemSubTypeItem makeItemSubTypeItem2 = MakeItemSubType.Instance[makeItemSubtypeId];
					int requiredAttainment2;
					if (itemType == 8 && lifeSkillType == 8)
					{
						List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = DomainManager.Taiwu.GetTaiwu().GetLearnedLifeSkills();
						GetMaterialGradeAndAttainment(materialTemplateId, itemType, lifeSkillType, totalAttainment, makeItemSubtypeIdList, out grade, out requiredAttainment2, learnedLifeSkills, makeItemSubtypeId, attainmentEffect);
					}
					startGrade = grade;
					GetStageRequirementAndGrade(i, startGrade, itemType, requiredAttainment, subTypeExtraLifeSkill, out var targetGrade2, out requiredAttainment2);
					sbyte grade3 = ItemTemplateHelper.GetGrade(itemType, makeItemSubTypeItem2.Result.TemplateId);
					(bool, sbyte, short) finalGradeAndId = GetFinalGradeAndId(grade3, targetGrade2, makeItemSubTypeItem2.Result.ItemType, makeItemSubTypeItem2.Result.TemplateId);
					bool item3 = finalGradeAndId.Item1;
					sbyte item4 = finalGradeAndId.Item2;
					short finialTemplateId2 = finalGradeAndId.Item3;
					bool flag5 = makeResultStageArray.Exist((MakeResultStage s) => s.IsInit && s.TemplateIdList != null && s.TemplateIdList.Contains(finialTemplateId2));
					if (item3 && !flag5)
					{
						list.Add(finialTemplateId2);
						list2.Add(makeItemSubtypeId);
					}
				}
				flag4 = list.Count > 0;
				if (flag4)
				{
					makeResultStageArray[i] = new MakeResultStage(targetRequirement, flag2, itemType, list, list2);
				}
			}
			if (flag4 && flag2 && (i < 2 || (upgradeMakeItem && i == 2)))
			{
				resultIndex = i;
			}
		}
	}

	private (bool success, sbyte finalGrade, short finialTemplateId) GetFinalGradeAndId(sbyte baseGrade, sbyte targetGrade, sbyte itemType, short baseTemplateId)
	{
		short groupId = ItemTemplateHelper.GetGroupId(itemType, baseTemplateId);
		if (groupId < 0)
		{
			return (success: true, finalGrade: Convert.ToSByte(baseGrade), finialTemplateId: baseTemplateId);
		}
		targetGrade = Math.Clamp(targetGrade, 0, 8);
		int num = Math.Max(targetGrade - baseGrade, -2);
		short num2 = Convert.ToInt16(baseTemplateId + num);
		int num3 = (ItemTemplateHelper.CheckTemplateValid(itemType, num2) ? ItemTemplateHelper.GetGroupId(itemType, num2) : (-1));
		bool item = num3 == groupId;
		return (success: item, finalGrade: Convert.ToSByte(targetGrade), finialTemplateId: num2);
	}

	private void GetMaterialGradeAndAttainment(short materialTemplateId, sbyte itemType, sbyte lifeSkillType, int totalAttainment, List<short> makeItemSubtypeIdList, out sbyte grade, out int requiredAttainment, List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills, short makeItemSubTypeId = -1, int attainmentEffect = 0)
	{
		MaterialItem materialItem = Config.Material.Instance[materialTemplateId];
		grade = materialItem.Grade;
		requiredAttainment = SharedMethods.GetRequiredLifeSkillAttainmentByBuildingEffect(materialItem.RequiredAttainment, attainmentEffect);
		if (itemType == 8 && lifeSkillType == 8)
		{
			grade = SharedMethods.GetHerbMaterialTempGrade(grade, makeItemSubtypeIdList, makeItemSubTypeId);
			requiredAttainment = GlobalConfig.Instance.MakeMadicineAttainments[grade];
			requiredAttainment = SharedMethods.GetRequiredLifeSkillAttainmentByBuildingEffect(requiredAttainment, attainmentEffect);
		}
		else
		{
			if (itemType != 7)
			{
				return;
			}
			int finishedCookingCount = 0;
			learnedLifeSkills?.ForEach(delegate(GameData.Domains.Character.LifeSkillItem lifeSkillItem)
			{
				if (lifeSkillItem.IsAllPagesRead())
				{
					Config.LifeSkillItem lifeSkillItem2 = LifeSkill.Instance[lifeSkillItem.SkillTemplateId];
					if (lifeSkillItem2.Type == 14)
					{
						finishedCookingCount++;
					}
				}
			});
			int num = finishedCookingCount;
			sbyte b = grade;
			for (int num2 = 0; num2 < num; num2++)
			{
				sbyte targetGrade = (sbyte)Math.Min(6, b + num2);
				(bool success, sbyte finalGrade, short finialTemplateId) finalGradeAndId = GetFinalGradeAndId(b, targetGrade, itemType, materialTemplateId);
				sbyte item = finalGradeAndId.finalGrade;
				short item2 = finalGradeAndId.finialTemplateId;
				MaterialItem materialItem2 = Config.Material.Instance[item2];
				short requiredLifeSkillAttainmentByBuildingEffect = SharedMethods.GetRequiredLifeSkillAttainmentByBuildingEffect(materialItem2.RequiredAttainment, attainmentEffect);
				if (totalAttainment >= requiredLifeSkillAttainmentByBuildingEffect)
				{
					requiredAttainment = requiredLifeSkillAttainmentByBuildingEffect;
					grade = item;
					continue;
				}
				break;
			}
		}
	}

	private void GetStageRequirementAndGrade(int i, sbyte startGrade, sbyte itemType, int requirement, short subTypeExtraLifeSkill, out sbyte targetGrade, out int targetRequirement)
	{
		switch (i)
		{
		case 0:
			targetRequirement = GetStageRequiredAttainment(i, requirement, subTypeExtraLifeSkill);
			targetGrade = ((itemType == 7) ? startGrade : Convert.ToSByte(startGrade - 1));
			break;
		case 1:
			targetRequirement = GetStageRequiredAttainment(i, requirement, subTypeExtraLifeSkill);
			targetGrade = ((itemType == 7) ? Convert.ToSByte(startGrade + 1) : startGrade);
			break;
		default:
			targetRequirement = GetStageRequiredAttainment(i, requirement, subTypeExtraLifeSkill);
			targetGrade = ((itemType == 7) ? Convert.ToSByte(startGrade + 2) : Convert.ToSByte(startGrade + 1));
			break;
		}
	}

	private int GetStageRequiredAttainment(int i, int attainment, short subTypeExtraLifeSkill)
	{
		return attainment * GlobalConfig.Instance.MakeItemStageAttainmentFactor[i] / 100 + subTypeExtraLifeSkill;
	}

	public (sbyte grade, short templateId) GetMakeResultTargetItemGradeAndTemplateId(short materialTemplateId, short totalAttainment, sbyte lifeSkillType, List<short> makeItemSubtypeIdList, short makeItemSubTypeId, List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills, IRandomSource randomSource, bool upgradeMakeItem, int attainmentEffect)
	{
		MakeItemSubTypeItem makeItemSubTypeItem = MakeItemSubType.Instance[makeItemSubTypeId];
		sbyte itemType = makeItemSubTypeItem.Result.ItemType;
		GetMaterialGradeAndAttainment(materialTemplateId, itemType, lifeSkillType, totalAttainment, makeItemSubtypeIdList, out var grade, out var requiredAttainment, learnedLifeSkills, makeItemSubTypeId, attainmentEffect);
		GetMakeResultStageArray(materialTemplateId, grade, requiredAttainment, totalAttainment, makeItemSubtypeIdList, attainmentEffect, lifeSkillType, out var resultIndex, out var makeResultStageArray, makeItemSubTypeId, makeIsManual: false, upgradeMakeItem);
		MakeResultStage makeResultStage = makeResultStageArray[resultIndex];
		return ((sbyte grade, short templateId))(makeResultStage.LifeSkillIsMeet ? (((int grade, int templateId))makeResultStage.GetGradeAndId(randomSource)) : (grade: 0, templateId: -1));
	}

	[DomainMethod]
	[Obsolete("可以用RepairItemOptional替代")]
	public ItemDisplayData RepairItem(DataContext context, int charId, ItemKey toolKey, ItemKey itemKey)
	{
		return RepairItemOptional(context, charId, toolKey, itemKey, 1);
	}

	[DomainMethod]
	public ItemDisplayData RepairItemOptional(DataContext context, int charId, ItemKey toolKey, ItemKey itemKey, sbyte toolSourceType)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
		short currDurability = baseItem.GetCurrDurability();
		sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
		if (toolKey.IsValid() && toolKey.TemplateId != 54)
		{
			CraftToolItem craftToolItem = Config.CraftTool.Instance[toolKey.TemplateId];
			short reduceValue = craftToolItem.DurabilityCost[grade];
			DomainManager.Item.ReduceToolDurability(context, charId, toolKey, reduceValue, toolSourceType);
		}
		EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
		ResourceInts repairNeedResources = ItemTemplateHelper.GetRepairNeedResources(baseEquipment.GetMaterialResources(), itemKey, currDurability);
		if (charId == DomainManager.Taiwu.GetTaiwuCharId())
		{
			for (sbyte b = 0; b < 8; b++)
			{
				int num = repairNeedResources.Get(b);
				if (num > 0)
				{
					ConsumeResource(context, b, num);
				}
			}
		}
		else
		{
			repairNeedResources = repairNeedResources.GetReversed();
			element_Objects.ChangeResources(context, ref repairNeedResources);
		}
		sbyte craftRequiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(itemKey.ItemType, itemKey.TemplateId);
		if (((uint)(craftRequiredLifeSkillType - 6) <= 1u || (uint)(craftRequiredLifeSkillType - 10) <= 1u) ? true : false)
		{
			int baseDelta = ((baseItem.GetCurrDurability() > 0) ? ProfessionFormulaImpl.Calculate(18, grade, baseItem.GetCurrDurability(), baseItem.GetMaxDurability()) : ProfessionFormulaImpl.Calculate(19, grade));
			DomainManager.Extra.ChangeProfessionSeniority(context, 2, baseDelta);
		}
		baseItem.SetCurrDurability(baseItem.GetMaxDurability(), context);
		return DomainManager.Item.GetItemDisplayData(baseItem, charId, -1, -1);
	}

	[DomainMethod]
	public List<ItemDisplayData> RepairItemList(DataContext context, int charId, List<MultiplyOperation> operationList)
	{
		List<ItemDisplayData> list = new List<ItemDisplayData>();
		foreach (MultiplyOperation operation in operationList)
		{
			for (int i = 0; i < operation.Count; i++)
			{
				ItemDisplayData item = RepairItemOptional(context, charId, operation.Tool, operation.Target, operation.ToolItemSourceType);
				list.Add(item);
			}
		}
		return list;
	}

	[DomainMethod]
	public bool CheckRepairConditionIsMeet(int charId, ItemKey toolKey, ItemKey itemKey, BuildingBlockKey buildingBlockKey)
	{
		if (!DomainManager.Item.CheckItemNeedRepair(itemKey))
		{
			return false;
		}
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
		sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
		CraftToolItem craftToolItem = Config.CraftTool.Instance[toolKey.TemplateId];
		short num = craftToolItem.DurabilityCost[grade];
		if (!CheckTool(toolKey, num, num, -1))
		{
			return false;
		}
		EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
		ResourceInts needResources = ItemTemplateHelper.GetRepairNeedResources(baseEquipment.GetMaterialResources(), itemKey, baseItem.GetCurrDurability());
		if (!((charId == DomainManager.Taiwu.GetTaiwuCharId()) ? GetAllTaiwuResources() : element_Objects.GetResources()).CheckIsMeet(ref needResources))
		{
			return false;
		}
		sbyte craftRequiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(itemKey.ItemType, itemKey.TemplateId);
		int num2 = ItemTemplateHelper.GetRepairRequiredAttainment(itemKey.ItemType, itemKey.TemplateId, baseItem.GetCurrDurability());
		if (!buildingBlockKey.IsInvalid)
		{
			int item = GetBuildingEffectForMake(buildingBlockKey, craftRequiredLifeSkillType).attainmentEffect;
			num2 = SharedMethods.GetRequiredLifeSkillAttainmentByBuildingEffect(num2, item);
		}
		num2 = Math.Max(0, num2);
		return CheckLifeSkill(craftRequiredLifeSkillType, num2, toolKey);
	}

	[DomainMethod]
	public (bool, ItemDisplayData) AddItemPoison(DataContext context, int charId, ItemDisplayData tool, ItemDisplayData target, ItemDisplayData[] poisons, List<ItemDisplayData> condensePoisonItemList)
	{
		ItemKey itemKey = target.Key;
		ItemBase itemBase = DomainManager.Item.GetBaseItem(itemKey);
		CraftToolItem craftToolItem = Config.CraftTool.Instance[tool.Key.TemplateId];
		FullPoisonEffects poisonEffects = DomainManager.Item.GetPoisonEffects(itemKey);
		if (ModificationStateHelper.IsActive(itemKey.ModificationState, 1) && poisonEffects.IsValid && !poisonEffects.IsIdentified && !ItemType.IsEquipmentItemType(itemKey.ItemType))
		{
			return (false, new ItemDisplayData());
		}
		short num = 0;
		ItemBase itemBase2 = itemBase;
		List<short> list = new List<short>();
		for (int i = 0; i < poisons.Length; i++)
		{
			ItemDisplayData itemDisplayData = poisons[i];
			if (itemDisplayData == null || !itemDisplayData.Key.HasTemplate)
			{
				continue;
			}
			MedicineItem medicineItem = Config.Medicine.Instance[itemDisplayData.Key.TemplateId];
			sbyte b = medicineItem.Grade;
			PoisonSlot poisonSlot = poisonEffects.PoisonSlotList?.GetOrDefault(i);
			sbyte b2 = poisonSlot?.MedicineConfig?.Grade ?? (-1);
			bool flag = medicineItem.Grade <= b2 && (poisonSlot?.IsCondensed ?? false);
			list.Clear();
			if (condensePoisonItemList != null && condensePoisonItemList.Count > 0)
			{
				foreach (ItemDisplayData condensePoisonItem in condensePoisonItemList)
				{
					MedicineItem medicineItem2 = Config.Medicine.Instance[condensePoisonItem.Key.TemplateId];
					if (medicineItem2.PoisonType == medicineItem.PoisonType)
					{
						for (int j = 0; j < condensePoisonItem.Amount; j++)
						{
							list.Add(medicineItem2.TemplateId);
						}
					}
				}
			}
			if (list.Count > 0)
			{
				sbyte val = list.Max((short m) => ItemTemplateHelper.GetGrade(8, m));
				b = Math.Max(b, val);
			}
			else if (flag)
			{
				list.AddRange(poisonSlot.CondensedMedicineTemplateIdList);
			}
			if (list.Count > 0 || itemDisplayData.Key.IsValid())
			{
				short val2 = craftToolItem.DurabilityCost[b];
				num = Math.Max(num, val2);
			}
			if (!itemDisplayData.Key.IsValid())
			{
				poisonSlot?.SetPoison(itemDisplayData.Key.TemplateId, list);
				continue;
			}
			(ItemBase item, bool keyChanged) tuple = DomainManager.Item.SetAttachedPoisons(context, itemBase, itemDisplayData.Key.TemplateId, add: true, list);
			ItemBase item = tuple.item;
			bool item2 = tuple.keyChanged;
			itemBase2 = item;
			if (item2)
			{
				ItemKey oldKey = itemKey;
				ItemKey itemKey2 = item.GetItemKey();
				ChangeItem(context, oldKey, target.ItemSourceType, itemKey2, charId);
				itemKey = item.GetItemKey();
				itemBase = item;
			}
			DomainManager.Taiwu.RemoveItem(context, itemDisplayData.Key, 1, itemDisplayData.ItemSourceType, deleteItem: true);
		}
		DomainManager.Item.SetPoisonsIdentified(context, itemBase2.GetItemKey(), isIdentified: true);
		if (tool.Key.IsValid())
		{
			DomainManager.Item.ReduceToolDurability(context, charId, tool.Key, num, tool.ItemSourceType);
		}
		if (condensePoisonItemList != null && condensePoisonItemList.Count > 0)
		{
			foreach (ItemDisplayData condensePoisonItem2 in condensePoisonItemList)
			{
				DomainManager.Taiwu.RemoveItem(context, condensePoisonItem2.Key, condensePoisonItem2.Amount, condensePoisonItem2.ItemSourceType, deleteItem: true);
			}
		}
		return (true, DomainManager.Item.GetItemDisplayData(itemBase2, 1, charId, target.ItemSourceType));
	}

	[DomainMethod]
	public bool CheckAddPoisonCondition(int charId, ItemKey toolKey, ItemKey targetKey, ItemKey[] poisonKeys, BuildingBlockKey buildingBlockKey, FullPoisonEffects tempPoisonEffects)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		if (!DomainManager.Item.GetBaseItem(targetKey).GetPoisonable())
		{
			return false;
		}
		FullPoisonEffects poisonEffects = DomainManager.Item.GetPoisonEffects(targetKey);
		if (poisonEffects.SameOf(tempPoisonEffects) && (!poisonEffects.IsValid || poisonEffects.IsIdentified))
		{
			return false;
		}
		CraftToolItem craftToolItem = Config.CraftTool.Instance[toolKey.TemplateId];
		int num = 0;
		short num2 = 0;
		for (int i = 0; i < poisonKeys.Length; i++)
		{
			ItemKey itemKey = poisonKeys[i];
			if (!itemKey.HasTemplate)
			{
				continue;
			}
			short val = ItemTemplateHelper.GetPoisonRequiredAttainment(8, itemKey.TemplateId);
			bool flag = tempPoisonEffects.PoisonSlotList.CheckIndex(i) && tempPoisonEffects.PoisonSlotList[i].IsCondensed;
			if (flag)
			{
				short num3 = tempPoisonEffects.PoisonSlotList[i].CondensedMedicineTemplateIdList.Max((short m) => ItemTemplateHelper.GetPoisonRequiredAttainment(8, m));
				int condensePoisonRequiredAttainmentBonus = GlobalConfig.Instance.CondensePoisonRequiredAttainmentBonus;
				num3 = Convert.ToInt16(num3 * (100 + condensePoisonRequiredAttainmentBonus) / 100);
				val = Math.Max(val, num3);
			}
			sbyte b = ItemTemplateHelper.GetGrade(8, itemKey.TemplateId);
			if (flag)
			{
				sbyte val2 = tempPoisonEffects.PoisonSlotList[i].CondensedMedicineTemplateIdList.Max((short m) => ItemTemplateHelper.GetGrade(8, m));
				b = Math.Max(b, val2);
			}
			if (flag || itemKey.IsValid())
			{
				num = Math.Max(val, num);
				short val3 = craftToolItem.DurabilityCost[b];
				num2 = Math.Max(num2, val3);
			}
		}
		if (!CheckTool(toolKey, num2, num2, 9))
		{
			return false;
		}
		if (!buildingBlockKey.IsInvalid)
		{
			int item = GetBuildingEffectForMake(buildingBlockKey, 9).attainmentEffect;
			num = SharedMethods.GetRequiredLifeSkillAttainmentByBuildingEffect(num, item);
		}
		if (!CheckLifeSkill(9, num, toolKey))
		{
			return false;
		}
		return true;
	}

	[DomainMethod]
	public (bool, List<ItemDisplayData>) RemoveItemPoison(DataContext context, int charId, ItemDisplayData tool, ItemDisplayData target, ItemDisplayData[] medicines, bool isExtract)
	{
		List<ItemDisplayData> list = new List<ItemDisplayData>();
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		ItemKey[] equipment = element_Objects.GetEquipment();
		ItemKey itemKey = target.Key;
		ItemBase item = DomainManager.Item.GetBaseItem(itemKey);
		List<short> allMedicineTemplateIds = target.PoisonEffects.GetAllMedicineTemplateIds();
		CraftToolItem craftToolItem = Config.CraftTool.Instance[tool.Key.TemplateId];
		List<ItemDisplayData> list2 = new List<ItemDisplayData>();
		foreach (ItemDisplayData itemDisplayData in medicines)
		{
			if (itemDisplayData == null || !itemDisplayData.Key.IsValid() || allMedicineTemplateIds.Contains(itemDisplayData.Key.TemplateId))
			{
				continue;
			}
			MedicineItem config = Config.Medicine.Instance[itemDisplayData.Key.TemplateId];
			if (allMedicineTemplateIds.Exist((short id) => id > -1 && Config.Medicine.Instance[id].PoisonType == config.PoisonType))
			{
				if (itemDisplayData.HasAnyPoison && !itemDisplayData.PoisonIsIdentified)
				{
					list.Add(new ItemDisplayData());
					return (false, list);
				}
				list2.Add(itemDisplayData);
			}
		}
		List<short> list3 = null;
		if (isExtract)
		{
			list3 = new List<short>();
		}
		ItemBase item2 = null;
		short num = 0;
		foreach (ItemDisplayData item5 in list2)
		{
			MedicineItem config2 = Config.Medicine.Instance[item5.Key.TemplateId];
			short targetPoisonId = allMedicineTemplateIds.Find((short id) => id > -1 && Config.Medicine.Instance[id].PoisonType == config2.PoisonType);
			(ItemBase item, bool keyChanged) tuple = DomainManager.Item.SetAttachedPoisons(context, item, targetPoisonId, add: false);
			ItemBase item3 = tuple.item;
			bool item4 = tuple.keyChanged;
			item2 = item3;
			if (item4)
			{
				ItemKey oldKey = itemKey;
				ItemKey itemKey2 = item3.GetItemKey();
				ChangeItem(context, oldKey, target.ItemSourceType, itemKey2, charId);
				itemKey = item3.GetItemKey();
				item = item3;
			}
			DomainManager.Taiwu.RemoveItem(context, item5.Key, 1, item5.ItemSourceType, deleteItem: true);
			sbyte grade = ItemTemplateHelper.GetGrade(8, targetPoisonId);
			short val = craftToolItem.DurabilityCost[grade];
			num = Math.Max(num, val);
			if (isExtract)
			{
				list3.Add(targetPoisonId);
				PoisonSlot poisonSlot = target.PoisonEffects.PoisonSlotList.Find((PoisonSlot s) => s.MedicineTemplateId == targetPoisonId);
				if (poisonSlot.IsCondensed)
				{
					list3.AddRange(poisonSlot.CondensedMedicineTemplateIdList);
				}
			}
			int baseDelta = ProfessionFormulaImpl.Calculate(84, grade);
			DomainManager.Extra.ChangeProfessionSeniority(context, 13, baseDelta);
		}
		if (tool.Key.IsValid())
		{
			DomainManager.Item.ReduceToolDurability(context, charId, tool.Key, num, tool.ItemSourceType);
		}
		ItemDisplayData itemDisplayData2 = DomainManager.Item.GetItemDisplayData(item2, 1, charId, target.ItemSourceType);
		list.Add(itemDisplayData2);
		if (isExtract)
		{
			foreach (short item6 in list3)
			{
				ItemKey itemKey3 = DomainManager.Item.CreateItem(context, 8, item6);
				DomainManager.Taiwu.AddItem(context, itemKey3, 1, target.ItemSourceTypeEnum);
				ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey3);
				int num2 = list.FindIndex((ItemDisplayData d) => d.Key.Equals(itemKey3));
				if (num2 >= 0)
				{
					list[num2].Amount++;
					continue;
				}
				ItemDisplayData itemDisplayData3 = DomainManager.Item.GetItemDisplayData(baseItem, 1, charId, target.ItemSourceType);
				list.Add(itemDisplayData3);
			}
		}
		return (true, list);
	}

	[DomainMethod]
	public bool CheckRemovePoisonCondition(int charId, ItemKey toolKey, ItemKey targetKey, ItemKey[] medicineKeys, BuildingBlockKey buildingBlockKey, bool isExtract)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		CraftToolItem craftToolItem = Config.CraftTool.Instance[toolKey.TemplateId];
		short num = 0;
		FullPoisonEffects poisonEffects = DomainManager.Item.GetPoisonEffects(targetKey);
		for (int i = 0; i < medicineKeys.Length; i++)
		{
			ItemKey itemKey = medicineKeys[i];
			if (itemKey.IsValid())
			{
				short medicineTemplateIdAt = poisonEffects.GetMedicineTemplateIdAt(i);
				sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
				sbyte grade2 = ItemTemplateHelper.GetGrade(8, medicineTemplateIdAt);
				short val = craftToolItem.DurabilityCost[grade2];
				num = Math.Max(num, val);
				if (!poisonEffects.IsValid || grade < grade2)
				{
					return false;
				}
				int num2 = ItemTemplateHelper.GetPoisonRequiredAttainment(8, medicineTemplateIdAt);
				if (isExtract)
				{
					int condensePoisonRequiredAttainmentBonus = GlobalConfig.Instance.CondensePoisonRequiredAttainmentBonus;
					num2 = Convert.ToInt16(num2 * (100 + condensePoisonRequiredAttainmentBonus) / 100);
				}
				if (!buildingBlockKey.IsInvalid)
				{
					int item = GetBuildingEffectForMake(buildingBlockKey, 9).attainmentEffect;
					num2 = SharedMethods.GetRequiredLifeSkillAttainmentByBuildingEffect(num2, item);
				}
				if (!CheckLifeSkill(9, num2, toolKey))
				{
					return false;
				}
			}
		}
		if (!CheckTool(toolKey, num, num, 9))
		{
			return false;
		}
		return true;
	}

	[DomainMethod]
	public ItemDisplayData RefineItem(DataContext context, int charId, ItemDisplayData tool, ItemDisplayData target, ItemDisplayData[] materialItemArray, List<ItemSourceChange> changeList)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		ItemBase itemBase = DomainManager.Item.GetBaseItem(target.Key);
		CraftToolItem craftToolItem = Config.CraftTool.Instance[tool.Key.TemplateId];
		RefiningEffects refinedEffects = default(RefiningEffects);
		if (ModificationStateHelper.IsActive(itemBase.GetModificationState(), 2))
		{
			refinedEffects = DomainManager.Item.GetRefinedEffects(target.Key);
		}
		else
		{
			refinedEffects.Initialize();
		}
		short[] materialTemplateIds = materialItemArray.Select((ItemDisplayData d) => d.Key.TemplateId).ToArray();
		ResourceInts refineRequiredResources = ItemTemplateHelper.GetRefineRequiredResources(refinedEffects.GetAllMaterialTemplateIds(), materialTemplateIds);
		if (charId == DomainManager.Taiwu.GetTaiwuCharId())
		{
			for (sbyte b = 0; b < 8; b++)
			{
				int num = refineRequiredResources.Get(b);
				if (num > 0)
				{
					ConsumeResource(context, b, num);
					sbyte craftRequiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(target.Key.ItemType, target.Key.TemplateId);
					if (((uint)(craftRequiredLifeSkillType - 6) <= 1u || (uint)(craftRequiredLifeSkillType - 10) <= 1u) ? true : false)
					{
						int baseDelta = ProfessionFormulaImpl.Calculate(20, num);
						DomainManager.Extra.ChangeProfessionSeniority(context, 2, baseDelta);
					}
				}
			}
		}
		else
		{
			refineRequiredResources = refineRequiredResources.GetReversed();
			element_Objects.ChangeResources(context, ref refineRequiredResources);
		}
		short num2 = 0;
		for (int num3 = 0; num3 < materialItemArray.Length; num3++)
		{
			ItemDisplayData itemDisplayData = materialItemArray[num3];
			short templateId = itemDisplayData.Key.TemplateId;
			short materialTemplateIdAt = refinedEffects.GetMaterialTemplateIdAt(num3);
			if (templateId != materialTemplateIdAt)
			{
				(ItemBase item, bool keyChanged) tuple = DomainManager.Item.SetRefinedEffects(context, itemBase, num3, templateId);
				var (itemBase2, _) = tuple;
				if (tuple.keyChanged)
				{
					ItemKey key = target.Key;
					ItemKey itemKey = itemBase2.GetItemKey();
					itemBase = itemBase2;
					ChangeItem(context, key, target.ItemSourceType, itemKey, charId);
				}
				else
				{
					ItemKey[] equipment = element_Objects.GetEquipment();
					element_Objects.SetEquipment(equipment, context);
				}
				short num4 = 0;
				if (templateId >= 0)
				{
					sbyte grade = ItemTemplateHelper.GetGrade(5, templateId);
					short val = craftToolItem.DurabilityCost[grade];
					num4 = Math.Max(num4, val);
				}
				if (materialTemplateIdAt >= 0)
				{
					sbyte grade2 = ItemTemplateHelper.GetGrade(5, materialTemplateIdAt);
					short val2 = craftToolItem.DurabilityCost[grade2];
					num4 = Math.Max(num4, val2);
				}
				num2 = Math.Max(num2, num4);
			}
		}
		ItemKey itemKey2 = tool.Key;
		if (itemKey2.IsValid())
		{
			DomainManager.Item.ReduceToolDurability(context, charId, tool.Key, num2, tool.ItemSourceType);
		}
		foreach (ItemSourceChange change in changeList)
		{
			foreach (ItemKeyAndCount item in change.Items)
			{
				item.Deconstruct(out itemKey2, out var count);
				ItemKey itemKey3 = itemKey2;
				int num5 = count;
				if (num5 > 0)
				{
					ItemKey itemKey4 = DomainManager.Item.CreateMaterial(context, itemKey3.TemplateId);
					DomainManager.Taiwu.AddItem(context, itemKey4, num5, change.ItemSourceType);
				}
				else if (num5 < 0)
				{
					DomainManager.Taiwu.RemoveItem(context, itemKey3, -num5, change.ItemSourceType, deleteItem: true);
				}
			}
		}
		return DomainManager.Item.GetItemDisplayData(itemBase, 1, charId, target.ItemSourceType);
	}

	[DomainMethod]
	public unsafe bool CheckRefineCondition(int charId, ItemKey toolKey, ItemKey equipItemKey, ItemDisplayData[] materialItemData, BuildingBlockKey buildingBlockKey)
	{
		ItemBase baseItem = DomainManager.Item.GetBaseItem(equipItemKey);
		if (!baseItem.GetRefinable())
		{
			return false;
		}
		RefiningEffects refinedEffects = default(RefiningEffects);
		if (ModificationStateHelper.IsActive(baseItem.GetModificationState(), 2))
		{
			refinedEffects = DomainManager.Item.GetRefinedEffects(equipItemKey);
		}
		else
		{
			refinedEffects.Initialize();
		}
		short[] materialTemplateIds = materialItemData.Select((ItemDisplayData d) => d.Key.TemplateId).ToArray();
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		ResourceInts needResources = ItemTemplateHelper.GetRefineRequiredResources(refinedEffects.GetAllMaterialTemplateIds(), materialTemplateIds);
		if (!((charId == DomainManager.Taiwu.GetTaiwuCharId()) ? GetAllTaiwuResources() : element_Objects.GetResources()).CheckIsMeet(ref needResources))
		{
			return false;
		}
		sbyte craftRequiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(equipItemKey.ItemType, equipItemKey.TemplateId);
		LifeSkillShorts need = default(LifeSkillShorts);
		bool flag = true;
		int num = 0;
		if (!buildingBlockKey.IsInvalid)
		{
			num = GetBuildingEffectForMake(buildingBlockKey, craftRequiredLifeSkillType).attainmentEffect;
		}
		CraftToolItem craftToolItem = Config.CraftTool.Instance[toolKey.TemplateId];
		short num2 = 0;
		for (int num3 = 0; num3 < materialItemData.Length; num3++)
		{
			short materialTemplateIdAt = refinedEffects.GetMaterialTemplateIdAt(num3);
			short templateId = materialItemData[num3].Key.TemplateId;
			bool flag2 = materialTemplateIdAt == templateId;
			if (!flag2)
			{
				flag = false;
			}
			if (templateId < 0 && materialTemplateIdAt < 0)
			{
				continue;
			}
			short index = templateId;
			if (templateId < 0)
			{
				index = materialTemplateIdAt;
			}
			MaterialItem materialItem = Config.Material.Instance[index];
			short val = need.Items[materialItem.RequiredLifeSkillType];
			int num4 = ((materialItem.RequiredAttainment == craftRequiredLifeSkillType) ? num : 0);
			need.Items[materialItem.RequiredLifeSkillType] = (short)Math.Max(val, materialItem.RequiredAttainment * (100 - num4) / 100);
			if (!flag2)
			{
				short num5 = craftToolItem.DurabilityCost[materialItem.Grade];
				if (materialTemplateIdAt >= 0)
				{
					MaterialItem materialItem2 = Config.Material.Instance[materialTemplateIdAt];
					short val2 = craftToolItem.DurabilityCost[materialItem2.Grade];
					num5 = Math.Max(num5, val2);
				}
				num2 = Math.Max(num2, num5);
			}
		}
		if (flag)
		{
			return false;
		}
		if (!CheckTool(toolKey, num2, num2, -1))
		{
			return false;
		}
		if (!CheckLifeSkill(ref need, toolKey))
		{
			return false;
		}
		return true;
	}

	[DomainMethod]
	public ItemDisplayData WeaveClothingItem(DataContext context, ItemDisplayData tool, ItemDisplayData target, short weaveClothingTemplateId)
	{
		if (tool.Key.IsValid())
		{
			sbyte grade = ItemTemplateHelper.GetGrade(target.Key.ItemType, weaveClothingTemplateId);
			CraftToolItem craftToolItem = Config.CraftTool.Instance[tool.Key.TemplateId];
			short num = craftToolItem.DurabilityCost[grade];
			if (num > 0)
			{
				DomainManager.Item.ReduceToolDurability(context, DomainManager.Taiwu.GetTaiwuCharId(), tool.Key, num, tool.ItemSourceType);
			}
		}
		DomainManager.Extra.SetClothingDisplayModification(context, target.Key, weaveClothingTemplateId);
		ItemDisplayData itemDisplayData = target.Clone();
		itemDisplayData.WeavedClothingTemplateId = weaveClothingTemplateId;
		return itemDisplayData;
	}

	public ItemDisplayData ProfessionDoctorMakeMedicine(DataContext context, ItemDisplayData medicine, ItemDisplayData tool, int makeCount)
	{
		int amount = makeCount * 3;
		Tester.Assert(medicine.Amount >= makeCount);
		short targetTemplateId;
		bool condition = ItemTemplateHelper.CanMedicineUpgrade(medicine.Key.ItemType, medicine.Key.TemplateId, out targetTemplateId);
		Tester.Assert(condition);
		List<ItemKey> operationKeyListFromPool = medicine.GetOperationKeyListFromPool(amount);
		foreach (ItemKey item in operationKeyListFromPool)
		{
			DomainManager.Taiwu.RemoveItem(context, item, 1, medicine.ItemSourceTypeEnum, deleteItem: true);
		}
		ItemDisplayData.ReturnItemKeyListToPool(operationKeyListFromPool);
		if (tool.Key.IsValid())
		{
			sbyte grade = ItemTemplateHelper.GetGrade(medicine.Key.ItemType, medicine.Key.TemplateId);
			CraftToolItem craftToolItem = Config.CraftTool.Instance[tool.Key.TemplateId];
			int reduceValue = craftToolItem.DurabilityCost[grade] * makeCount;
			DomainManager.Item.ReduceToolDurability(context, DomainManager.Taiwu.GetTaiwuCharId(), tool.Key, reduceValue, tool.ItemSourceType);
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		Location location = taiwu.GetLocation();
		bool flag = false;
		if (location == taiwuVillageLocation)
		{
			flag = true;
		}
		else
		{
			MapBlockData block = DomainManager.Map.GetBlock(location);
			if (block.IsCityTown())
			{
				flag = true;
			}
		}
		ItemKey itemKey = DomainManager.Item.CreateMedicine(context, targetTemplateId);
		if (flag)
		{
			taiwu.AddInventoryItem(context, itemKey, makeCount);
		}
		else
		{
			DomainManager.Taiwu.WarehouseAdd(context, itemKey, makeCount);
		}
		if (ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == 800)
		{
			sbyte grade2 = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
			int num = ProfessionFormulaImpl.Calculate(83, grade2);
			DomainManager.Extra.ChangeProfessionSeniority(context, 13, num * makeCount);
		}
		ItemDisplayData itemDisplayData = DomainManager.Item.GetItemDisplayData(itemKey);
		itemDisplayData.Amount = makeCount;
		return itemDisplayData;
	}

	[DomainMethod]
	public (int attainmentEffect, bool upgradeMakeItem) GetBuildingEffectForMake(BuildingBlockKey buildingBlockKey, sbyte skillType)
	{
		bool item = false;
		BuildingAreaData element_BuildingAreas = GetElement_BuildingAreas(buildingBlockKey.GetLocation());
		int num = 0;
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		sbyte width = BuildingBlock.Instance[_buildingBlocks[buildingBlockKey].TemplateId].Width;
		element_BuildingAreas.GetNeighborBlocks(buildingBlockKey.BuildingBlockIndex, width, list, list2, 2);
		foreach (short item2 in list)
		{
			BuildingBlockKey buildingBlockKey2 = new BuildingBlockKey(buildingBlockKey.AreaId, buildingBlockKey.BlockId, item2);
			BuildingBlockData buildingBlockData = _buildingBlocks[buildingBlockKey2];
			if (buildingBlockData.RootBlockIndex >= 0)
			{
				buildingBlockKey2 = new BuildingBlockKey(buildingBlockKey.AreaId, buildingBlockKey.BlockId, (byte)buildingBlockData.RootBlockIndex);
				buildingBlockData = _buildingBlocks[buildingBlockKey2];
			}
			if (buildingBlockData.TemplateId == 0 || !buildingBlockData.CanUse())
			{
				continue;
			}
			sbyte level = BuildingBlockLevel(buildingBlockKey2);
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[buildingBlockData.TemplateId];
			if (buildingBlockItem.ReduceMakeRequirementLifeSkillType == skillType)
			{
				foreach (short expandInfo in buildingBlockItem.ExpandInfos)
				{
					BuildingScaleItem buildingScaleItem = BuildingScale.Instance[expandInfo];
					if (buildingScaleItem != null && buildingScaleItem.LifeSkillType == skillType && buildingScaleItem.Effect == EBuildingScaleEffect.MakeItemAttainmentRequirementReduction)
					{
						num += buildingScaleItem.GetLevelEffect(level);
					}
				}
			}
			if (buildingBlockItem.RequireLifeSkillType == skillType && buildingBlockItem.UpgradeMakeItem)
			{
				item = true;
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<int>>.Instance.Return(list2);
		return (attainmentEffect: num, upgradeMakeItem: item);
	}

	private void ChangeItem(DataContext context, ItemKey oldKey, sbyte itemSourceType, ItemKey newKey, int charId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		ItemKey[] equipment = element_Objects.GetEquipment();
		sbyte b = (sbyte)equipment.IndexOf(oldKey);
		int planIndex;
		int slotIndex;
		bool flag = DomainManager.Taiwu.FindItemInEquipmentPlan(oldKey, out planIndex, out slotIndex);
		sbyte legendaryBookWeaponSlotByItemKey = DomainManager.Extra.GetLegendaryBookWeaponSlotByItemKey(oldKey);
		bool flag2 = legendaryBookWeaponSlotByItemKey >= 0;
		bool flag3 = newKey.Id != oldKey.Id;
		DomainManager.Taiwu.RemoveItem(context, oldKey, 1, itemSourceType, flag3);
		DomainManager.Taiwu.AddItem(context, newKey, 1, itemSourceType);
		if (!flag3)
		{
			if (b > -1)
			{
				element_Objects.ChangeEquipment(context, -1, b, newKey);
			}
			if (flag)
			{
				DomainManager.Taiwu.SetEquipmentPlan(context, newKey, planIndex, slotIndex);
			}
			if (flag2)
			{
				DomainManager.Extra.SetLegendaryBookWeaponSlot(context, legendaryBookWeaponSlotByItemKey, newKey);
			}
		}
	}

	private short GetLifeSkillTotalAttainment(sbyte type, ItemKey toolKey)
	{
		int num = DomainManager.Taiwu.GetTaiwu().GetLifeSkillAttainment(type);
		GameData.Domains.Item.CraftTool element;
		if (ItemTemplateHelper.IsEmptyTool(toolKey.ItemType, toolKey.TemplateId))
		{
			bool flag = (((uint)(type - 6) <= 1u || (uint)(type - 10) <= 1u) ? true : false);
			int num2;
			if (flag && DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(6))
			{
				ProfessionData professionData = DomainManager.Extra.GetProfessionData(2);
				num2 = professionData.GetSeniorityEmptyToolAttainmentBonus();
			}
			else
			{
				num2 = ProfessionData.SeniorityToEmptyToolAttainmentBonus(0);
			}
			int num3 = num * num2 / 100;
			num += num3;
		}
		else if (toolKey.IsValid() && DomainManager.Item.TryGetElement_CraftTools(toolKey.Id, out element))
		{
			CraftToolItem craftToolItem = Config.CraftTool.Instance[toolKey.TemplateId];
			if (craftToolItem != null && craftToolItem.RequiredLifeSkillTypes.Contains(type))
			{
				num += element.GetRealAttainmentBonus();
			}
		}
		num = Math.Max(0, num);
		return (short)num;
	}

	private bool CheckTool(ItemKey toolKey, int totalCost, int oneCost, sbyte skillType = -1)
	{
		if (!toolKey.IsValid() || ItemTemplateHelper.IsEmptyTool(toolKey.ItemType, toolKey.TemplateId))
		{
			return true;
		}
		GameData.Domains.Item.CraftTool element_CraftTools = DomainManager.Item.GetElement_CraftTools(toolKey.Id);
		short currDurability = element_CraftTools.GetCurrDurability();
		bool flag = currDurability >= totalCost || currDurability + oneCost > totalCost;
		if (skillType > -1)
		{
			flag = flag && element_CraftTools.GetRequiredLifeSkillTypes().Contains(skillType);
		}
		return flag;
	}

	public unsafe bool CheckLifeSkill(ref LifeSkillShorts need, ItemKey toolKey)
	{
		for (sbyte b = 0; b < 8; b++)
		{
			short lifeSkillTotalAttainment = GetLifeSkillTotalAttainment(b, toolKey);
			if (lifeSkillTotalAttainment < need.Items[b])
			{
				return false;
			}
		}
		return true;
	}

	public bool CheckLifeSkill(sbyte lifeSkillType, int requireAttainment, ItemKey toolKey)
	{
		return GetLifeSkillTotalAttainment(lifeSkillType, toolKey) >= requireAttainment;
	}

	public short GetLifeSkillTotalAttainment(int charId, sbyte type, ItemKey toolKey)
	{
		if (charId == DomainManager.Taiwu.GetTaiwuCharId())
		{
			return GetLifeSkillTotalAttainment(type, toolKey);
		}
		short num = DomainManager.Character.GetElement_Objects(charId).GetLifeSkillAttainment(type);
		if (toolKey.IsValid() && DomainManager.Item.TryGetElement_CraftTools(toolKey.Id, out var element))
		{
			CraftToolItem craftToolItem = Config.CraftTool.Instance[toolKey.TemplateId];
			if (craftToolItem != null && craftToolItem.RequiredLifeSkillTypes.Contains(type))
			{
				num += element.GetAttainmentBonus();
			}
		}
		return Math.Max((short)0, num);
	}

	[DomainMethod]
	public (short, BuildingBlockData) Build(DataContext context, BuildingBlockKey blockKey, short buildingTemplateId, int[] workers)
	{
		if (!CanBuild(blockKey, buildingTemplateId))
		{
			throw new Exception($"Can not build building {buildingTemplateId} on block {blockKey}");
		}
		SetVillageBuildWork(context, blockKey, workers);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[buildingTemplateId];
		if (BuildingBlockData.CanUpgrade(buildingBlockItem.Type) && SharedMethods.NeedCostResourceToBuild(buildingBlockItem))
		{
			for (sbyte b = 0; b < 8; b++)
			{
				if (buildingBlockItem.BaseBuildCost[b] > 0)
				{
					ConsumeResource(context, b, buildingBlockItem.BaseBuildCost[b]);
				}
			}
		}
		CostBuildingCore(context, buildingBlockItem);
		MapBlockItem mapBlockItem = MapBlock.Instance[DomainManager.Map.GetBlock(blockKey.AreaId, blockKey.BlockId).TemplateId];
		sbyte buildingAreaWidth = mapBlockItem.BuildingAreaWidth;
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(blockKey);
		element_BuildingBlocks.TemplateId = buildingTemplateId;
		element_BuildingBlocks.OfflineResetShopProgress();
		element_BuildingBlocks.OperationType = 0;
		element_BuildingBlocks.OperationProgress = 0;
		element_BuildingBlocks.OperationStopping = false;
		PlaceBuilding(context, blockKey.AreaId, blockKey.BlockId, blockKey.BuildingBlockIndex, element_BuildingBlocks, buildingAreaWidth);
		return (blockKey.BuildingBlockIndex, element_BuildingBlocks);
	}

	private void CostBuildingCore(DataContext context, BuildingBlockItem configData)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (configData.BuildingCoreItem == -1)
		{
			return;
		}
		ItemKey itemKey = ItemKey.Invalid;
		foreach (ItemKey key in DomainManager.Taiwu.WarehouseItems.Keys)
		{
			if (key.ItemType == 12 && key.TemplateId == configData.BuildingCoreItem)
			{
				itemKey = key;
				break;
			}
		}
		if (!itemKey.Equals(ItemKey.Invalid))
		{
			DomainManager.Taiwu.WarehouseRemove(context, itemKey, 1);
			return;
		}
		Inventory inventory = taiwu.GetInventory();
		foreach (ItemKey key2 in inventory.Items.Keys)
		{
			if (key2.ItemType == 12 && key2.TemplateId == configData.BuildingCoreItem)
			{
				taiwu.RemoveInventoryItem(context, key2, 1, deleteItem: true);
				break;
			}
		}
	}

	private int ClacBuildingTimeCost(int[] workers, BuildingBlockItem configData, sbyte buildingOperationType)
	{
		int num = 0;
		foreach (int num2 in workers)
		{
			if (num2 >= 0)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num2);
				num += element_Objects.GetLifeSkillAttainment(configData.RequireLifeSkillType) + DomainManager.Building.BaseWorkContribution;
			}
		}
		int num3 = configData.OperationTotalProgress[buildingOperationType];
		return Convert.ToInt32(Math.Ceiling((float)num3 / (float)num));
	}

	[Obsolete]
	public bool UpgradeIsMeetDependency(BuildingBlockKey blockKey)
	{
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(blockKey);
		sbyte minLevel;
		bool flag = !AllDependBuildingAvailable(blockKey, element_BuildingBlocks.TemplateId, out minLevel) || BuildingBlockLevel(blockKey) >= minLevel;
		return !flag;
	}

	[Obsolete]
	public bool UpgradeIsHaveEnoughResource(BuildingBlockData blockData, bool checkResourceChanges = false)
	{
		return false;
	}

	[Obsolete]
	public bool HaveEnoughResourceToExpandBuilding(BuildingBlockData blockData, int[] resourceChanges, bool isChangeSourceData = false)
	{
		return false;
	}

	[Obsolete]
	[DomainMethod]
	public (short, BuildingBlockData) Upgrade(DataContext context, BuildingBlockKey blockKey, int[] workers)
	{
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(blockKey);
		if (!CanUpgrade(blockKey, out var _) || !UpgradeIsHaveEnoughResource(element_BuildingBlocks))
		{
			throw new Exception($"Can not upgrade building on block {blockKey}");
		}
		if (_buildingOperatorDict.ContainsKey(blockKey))
		{
			List<int> collection = _buildingOperatorDict[blockKey].GetCollection();
			for (int i = 0; i < collection.Count; i++)
			{
				if (collection[i] >= 0)
				{
					DomainManager.Taiwu.RemoveVillagerWork(context, collection[i]);
				}
			}
		}
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
		if (buildingBlockItem.Type == EBuildingBlockType.NormalResource)
		{
			CostBuildingCore(context, buildingBlockItem);
		}
		SetVillageBuildWorkAndBlockData(context, blockKey, element_BuildingBlocks, workers, 2);
		_buildingAutoExpandStoppedNotifiedSet.Remove(blockKey);
		return (blockKey.BuildingBlockIndex, element_BuildingBlocks);
	}

	[DomainMethod]
	public (short, BuildingBlockData) Remove(DataContext context, BuildingBlockKey blockKey, int[] workers)
	{
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(blockKey);
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
		if ((buildingBlockItem.Type != EBuildingBlockType.Building && !BuildingBlockData.IsResource(buildingBlockItem.Type)) || element_BuildingBlocks.OperationType != -1 || buildingBlockItem.Class == EBuildingBlockClass.Static || buildingBlockItem.OperationTotalProgress[1] < 0)
		{
			return (blockKey.BuildingBlockIndex, element_BuildingBlocks);
		}
		SetVillageBuildWorkAndBlockData(context, blockKey, element_BuildingBlocks, workers, 1);
		return (blockKey.BuildingBlockIndex, element_BuildingBlocks);
	}

	[Obsolete]
	[DomainMethod]
	public (short, BuildingBlockData) Collect(DataContext context, BuildingBlockKey blockKey, int[] workers)
	{
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(blockKey);
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
		if (!BuildingBlockData.IsResource(buildingBlockItem.Type) || element_BuildingBlocks.OperationType != -1 || buildingBlockItem.OperationTotalProgress[1] < 0)
		{
			return (blockKey.BuildingBlockIndex, element_BuildingBlocks);
		}
		SetVillageBuildWorkAndBlockData(context, blockKey, element_BuildingBlocks, workers, 3);
		return (blockKey.BuildingBlockIndex, element_BuildingBlocks);
	}

	private void SetVillageBuildWorkAndBlockData(DataContext context, BuildingBlockKey blockKey, BuildingBlockData blockData, int[] workers, sbyte buildingOperationType)
	{
		SetVillageBuildWork(context, blockKey, workers);
		blockData.OperationType = buildingOperationType;
		blockData.OperationProgress = 0;
		blockData.OperationStopping = false;
		SetElement_BuildingBlocks(blockKey, blockData, context);
	}

	private void SetVillageBuildWork(DataContext context, BuildingBlockKey blockKey, int[] workers)
	{
		for (int i = 0; i < workers.Length; i++)
		{
			int num = workers[i];
			if (num >= 0)
			{
				VillagerWorkData villagerWorkData = new VillagerWorkData(num, 0, blockKey.AreaId, blockKey.BlockId);
				villagerWorkData.BuildingBlockIndex = blockKey.BuildingBlockIndex;
				villagerWorkData.WorkerIndex = (sbyte)i;
				DomainManager.Taiwu.SetVillagerWork(context, num, villagerWorkData);
			}
		}
	}

	[DomainMethod]
	public (short, BuildingBlockData) SetStopOperation(DataContext context, BuildingBlockKey blockKey, bool stop)
	{
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(blockKey);
		if (stop && (element_BuildingBlocks.OperationProgress == 0 || element_BuildingBlocks.OperationType == 0))
		{
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			if (element_BuildingBlocks.OperationType == 0)
			{
				ResetAllChildrenBlocks(context, blockKey, 0, -1);
				if (SharedMethods.NeedCostResourceToBuild(buildingBlockItem))
				{
					for (sbyte b = 0; b < 8; b++)
					{
						taiwu.ChangeResource(context, b, buildingBlockItem.BaseBuildCost[b]);
					}
				}
				if (buildingBlockItem.BuildingCoreItem != -1)
				{
					DomainManager.Taiwu.ReturnBuildingCoreItem(DataContextManager.GetCurrentThreadDataContext(), buildingBlockItem);
				}
				SetBuildingCustomName(context, blockKey, null);
			}
			element_BuildingBlocks.OperationType = -1;
			SetElement_BuildingBlocks(blockKey, element_BuildingBlocks, context);
			if (_buildingOperatorDict.ContainsKey(blockKey))
			{
				List<int> collection = _buildingOperatorDict[blockKey].GetCollection();
				for (int i = 0; i < collection.Count; i++)
				{
					if (collection[i] >= 0)
					{
						DomainManager.Taiwu.RemoveVillagerWork(context, collection[i]);
					}
				}
			}
		}
		else
		{
			element_BuildingBlocks.OperationStopping = stop;
		}
		SetElement_BuildingBlocks(blockKey, element_BuildingBlocks, context);
		return (blockKey.BuildingBlockIndex, element_BuildingBlocks);
	}

	[DomainMethod]
	public void SetOperator(DataContext context, BuildingBlockKey blockKey, sbyte index, int charId)
	{
		if (_buildingOperatorDict.ContainsKey(blockKey))
		{
			int num = _buildingOperatorDict[blockKey].GetCollection()[index];
			if (num == charId)
			{
				throw new Exception($"Same operator already exist, id: {charId}");
			}
			if (num >= 0)
			{
				DomainManager.Taiwu.RemoveVillagerWork(context, num);
			}
		}
		if (charId >= 0)
		{
			VillagerWorkData villagerWorkData = new VillagerWorkData(charId, 0, blockKey.AreaId, blockKey.BlockId);
			villagerWorkData.BuildingBlockIndex = blockKey.BuildingBlockIndex;
			villagerWorkData.WorkerIndex = index;
			DomainManager.Taiwu.SetVillagerWork(context, charId, villagerWorkData);
		}
	}

	[DomainMethod]
	[Obsolete]
	public (short, BuildingBlockData) SetBuildingMaintenance(DataContext context, BuildingBlockKey blockKey, bool maintenance)
	{
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(blockKey);
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
		if (!maintenance && buildingBlockItem.MustMaintenance)
		{
			throw new Exception($"Building {element_BuildingBlocks.TemplateId} {buildingBlockItem.Name} must maintenance");
		}
		if (buildingBlockItem.BaseMaintenanceCost.Count > 0)
		{
			element_BuildingBlocks.Maintenance = maintenance;
			SetElement_BuildingBlocks(blockKey, element_BuildingBlocks, context);
		}
		return (blockKey.BuildingBlockIndex, element_BuildingBlocks);
	}

	[DomainMethod]
	public (short, BuildingBlockData) Repair(DataContext context, BuildingBlockKey blockKey)
	{
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(blockKey);
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
		if (element_BuildingBlocks.Durability == buildingBlockItem.MaxDurability)
		{
			return (blockKey.BuildingBlockIndex, element_BuildingBlocks);
		}
		ResourceInts allTaiwuResources = GetAllTaiwuResources();
		int num = SharedMethods.CalcRepairBuildingCost(element_BuildingBlocks, buildingBlockItem);
		if (allTaiwuResources.Get(6) >= num)
		{
			ConsumeResource(context, 6, num);
			element_BuildingBlocks.Durability = buildingBlockItem.MaxDurability;
			SetElement_BuildingBlocks(blockKey, element_BuildingBlocks, context);
		}
		return (blockKey.BuildingBlockIndex, element_BuildingBlocks);
	}

	[DomainMethod]
	public void ConfirmPlanBuilding(DataContext context, List<IntPair> operateRecord, Location location)
	{
		if (operateRecord == null)
		{
			return;
		}
		HashSet<BuildingBlockKey> downgradedSet = new HashSet<BuildingBlockKey>();
		foreach (IntPair item in operateRecord)
		{
			PlanResetBuilding(context, new BuildingBlockKey(location.AreaId, location.BlockId, (short)item.First), new BuildingBlockKey(location.AreaId, location.BlockId, (short)item.Second), downgradedSet);
		}
	}

	public void PlanResetBuilding(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey, HashSet<BuildingBlockKey> downgradedSet)
	{
		ExchangeBlockData(context, originalBlockKey, nowBlockKey, downgradedSet);
		ExchangeBlockDataEx(context, originalBlockKey, nowBlockKey);
		ExchangeCollectBuildingResourceType(context, originalBlockKey, nowBlockKey);
		ExchangeBuildingOperator(context, originalBlockKey, nowBlockKey);
		ExchangeCustomBuildingName(context, originalBlockKey, nowBlockKey);
		ExchangeCollectBuildingEarningsData(context, originalBlockKey, nowBlockKey);
		ExchangeShopManager(context, originalBlockKey, nowBlockKey);
		ExchangeShopEvent(context, originalBlockKey, nowBlockKey);
		ExchangeMakeItem(context, originalBlockKey, nowBlockKey);
		ExchangeResident(context, originalBlockKey, nowBlockKey);
		ExchangeComfortableHouses(context, originalBlockKey, nowBlockKey);
		ExchangeBuildingResident(context, originalBlockKey, nowBlockKey);
		ExchangeBuildingComfortableHouse(context, originalBlockKey, nowBlockKey);
		ExchangeAutoWorkList(context, originalBlockKey, nowBlockKey);
		ExchangeAutoSoldList(context, originalBlockKey, nowBlockKey);
		ExchangeAutoExpandList(context, originalBlockKey, nowBlockKey);
		ExchangeShopArrangeResultFirstList(context, originalBlockKey, nowBlockKey);
		ExchangeAutoCheckComfortableInList(context, originalBlockKey, nowBlockKey);
		ExchangeAutoCheckResidenceInList(context, originalBlockKey, nowBlockKey);
		ExchangeExtraBlockData(context, originalBlockKey, nowBlockKey);
		UpdateTaiwuVillageBuildingEffect();
	}

	private void ExchangeExtraBlockData(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		DomainManager.Extra.ExchangeBuildingArtisanOrder(context, originalBlockKey, nowBlockKey);
		DomainManager.Extra.ExchangeResourceBlockExtraData(context, originalBlockKey, nowBlockKey);
		DomainManager.Extra.ExchangeFeast(context, originalBlockKey, nowBlockKey);
	}

	private void ExchangeAutoCheckComfortableInList(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		List<short> autoCheckInComfortableList = DomainManager.Extra.GetAutoCheckInComfortableList();
		if (autoCheckInComfortableList.Contains(originalBlockKey.BuildingBlockIndex))
		{
			autoCheckInComfortableList.Remove(originalBlockKey.BuildingBlockIndex);
			if (!autoCheckInComfortableList.Contains(nowBlockKey.BuildingBlockIndex))
			{
				autoCheckInComfortableList.Add(nowBlockKey.BuildingBlockIndex);
			}
			DomainManager.Extra.SetAutoCheckInComfortableList(autoCheckInComfortableList, context);
		}
	}

	private void ExchangeAutoCheckResidenceInList(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		List<short> autoCheckInResidenceList = DomainManager.Extra.GetAutoCheckInResidenceList();
		if (autoCheckInResidenceList.Contains(originalBlockKey.BuildingBlockIndex))
		{
			autoCheckInResidenceList.Remove(originalBlockKey.BuildingBlockIndex);
			if (!autoCheckInResidenceList.Contains(nowBlockKey.BuildingBlockIndex))
			{
				autoCheckInResidenceList.Add(nowBlockKey.BuildingBlockIndex);
			}
			DomainManager.Extra.SetAutoCheckInResidenceList(autoCheckInResidenceList, context);
		}
	}

	private void ExchangeShopArrangeResultFirstList(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		List<short> shopArrangeResultFirstList = DomainManager.Extra.GetShopArrangeResultFirstList();
		if (shopArrangeResultFirstList.Contains(originalBlockKey.BuildingBlockIndex))
		{
			shopArrangeResultFirstList.Remove(originalBlockKey.BuildingBlockIndex);
			if (!shopArrangeResultFirstList.Contains(nowBlockKey.BuildingBlockIndex))
			{
				shopArrangeResultFirstList.Add(nowBlockKey.BuildingBlockIndex);
			}
			DomainManager.Extra.SetShopArrangeResultFirstList(shopArrangeResultFirstList, context);
		}
	}

	private void ExchangeAutoExpandList(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		List<short> autoExpandBlockIndexList = DomainManager.Extra.GetAutoExpandBlockIndexList();
		if (autoExpandBlockIndexList.Contains(originalBlockKey.BuildingBlockIndex))
		{
			int index = autoExpandBlockIndexList.IndexOf(originalBlockKey.BuildingBlockIndex);
			autoExpandBlockIndexList.Remove(originalBlockKey.BuildingBlockIndex);
			if (!autoExpandBlockIndexList.Contains(nowBlockKey.BuildingBlockIndex))
			{
				autoExpandBlockIndexList.Insert(index, nowBlockKey.BuildingBlockIndex);
			}
			DomainManager.Extra.SetAutoExpandBlockIndexList(autoExpandBlockIndexList, context);
		}
	}

	private void ExchangeAutoWorkList(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		List<short> autoWorkBlockIndexList = DomainManager.Extra.GetAutoWorkBlockIndexList();
		if (autoWorkBlockIndexList.Contains(originalBlockKey.BuildingBlockIndex))
		{
			autoWorkBlockIndexList.Remove(originalBlockKey.BuildingBlockIndex);
			if (!autoWorkBlockIndexList.Contains(nowBlockKey.BuildingBlockIndex))
			{
				autoWorkBlockIndexList.Add(nowBlockKey.BuildingBlockIndex);
			}
			DomainManager.Extra.SetAutoWorkBlockIndexList(autoWorkBlockIndexList, context);
		}
	}

	private void ExchangeAutoSoldList(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		List<short> autoSoldBlockIndexList = DomainManager.Extra.GetAutoSoldBlockIndexList();
		if (autoSoldBlockIndexList.Contains(originalBlockKey.BuildingBlockIndex))
		{
			autoSoldBlockIndexList.Remove(originalBlockKey.BuildingBlockIndex);
			if (!autoSoldBlockIndexList.Contains(nowBlockKey.BuildingBlockIndex))
			{
				autoSoldBlockIndexList.Add(nowBlockKey.BuildingBlockIndex);
			}
			DomainManager.Extra.SetAutoSoldBlockIndexList(autoSoldBlockIndexList, context);
		}
	}

	private void ExchangeBuildingResident(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		if (_buildingResidents == null)
		{
			return;
		}
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, BuildingBlockKey> buildingResident in _buildingResidents)
		{
			if (buildingResident.Value.Equals(originalBlockKey))
			{
				list.Add(buildingResident.Key);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			_buildingResidents[list[i]] = nowBlockKey;
		}
	}

	private void ExchangeBuildingComfortableHouse(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		if (_buildingComfortableHouses == null)
		{
			return;
		}
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, BuildingBlockKey> buildingComfortableHouse in _buildingComfortableHouses)
		{
			if (buildingComfortableHouse.Value.Equals(originalBlockKey))
			{
				list.Add(buildingComfortableHouse.Key);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			_buildingComfortableHouses[list[i]] = nowBlockKey;
		}
	}

	private void ExchangeComfortableHouses(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		if (TryGetElement_ComfortableHouses(originalBlockKey, out var value))
		{
			if (!TryGetElement_ComfortableHouses(nowBlockKey, out var _))
			{
				AddElement_ComfortableHouses(nowBlockKey, value, context);
			}
			else
			{
				SetElement_ComfortableHouses(nowBlockKey, value, context);
			}
			RemoveElement_ComfortableHouses(originalBlockKey, context);
		}
	}

	private void ExchangeResident(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		if (TryGetElement_Residences(originalBlockKey, out var value))
		{
			if (!TryGetElement_Residences(nowBlockKey, out var _))
			{
				AddElement_Residences(nowBlockKey, value, context);
			}
			else
			{
				SetElement_Residences(nowBlockKey, value, context);
			}
			RemoveElement_Residences(originalBlockKey, context);
		}
	}

	private void ExchangeMakeItem(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		if (TryGetElement_MakeItemDict(originalBlockKey, out var value))
		{
			if (!TryGetElement_MakeItemDict(nowBlockKey, out var _))
			{
				AddElement_MakeItemDict(nowBlockKey, value, context);
			}
			else
			{
				SetElement_MakeItemDict(nowBlockKey, value, context);
			}
			RemoveElement_MakeItemDict(originalBlockKey, context);
		}
	}

	private void ExchangeShopEvent(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		if (_shopEventCollections != null && _shopEventCollections.Remove(originalBlockKey, out var value))
		{
			_shopEventCollections[nowBlockKey] = value;
		}
	}

	private void ExchangeShopManager(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		if (!TryGetElement_ShopManagerDict(originalBlockKey, out var value))
		{
			return;
		}
		if (!TryGetElement_ShopManagerDict(nowBlockKey, out var _))
		{
			AddElement_ShopManagerDict(nowBlockKey, value, context);
		}
		else
		{
			SetElement_ShopManagerDict(nowBlockKey, value, context);
		}
		for (int i = 0; i < value.GetCount(); i++)
		{
			if (value.GetCollection()[i] != -1)
			{
				DomainManager.Taiwu.ChangeVillagerWorkBuildingBlockIndex(value.GetCollection()[i], nowBlockKey.BuildingBlockIndex, context);
			}
		}
		RemoveElement_ShopManagerDict(originalBlockKey, context);
	}

	private void ExchangeCollectBuildingEarningsData(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		if (TryGetElement_CollectBuildingEarningsData(originalBlockKey, out var value))
		{
			if (!TryGetElement_CollectBuildingEarningsData(nowBlockKey, out var _))
			{
				AddElement_CollectBuildingEarningsData(nowBlockKey, value, context);
			}
			else
			{
				SetElement_CollectBuildingEarningsData(nowBlockKey, value, context);
			}
			RemoveElement_CollectBuildingEarningsData(originalBlockKey, context);
		}
	}

	private void ExchangeCustomBuildingName(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		if (TryGetElement_CustomBuildingName(originalBlockKey, out var value))
		{
			if (!TryGetElement_CustomBuildingName(nowBlockKey, out var _))
			{
				AddElement_CustomBuildingName(nowBlockKey, value, context);
			}
			else
			{
				SetElement_CustomBuildingName(nowBlockKey, value, context);
			}
			RemoveElement_CustomBuildingName(originalBlockKey, context);
		}
	}

	private void ExchangeBuildingOperator(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		if (!TryGetElement_BuildingOperatorDict(originalBlockKey, out var value))
		{
			return;
		}
		if (!TryGetElement_BuildingOperatorDict(nowBlockKey, out var _))
		{
			AddElement_BuildingOperatorDict(nowBlockKey, value, context);
		}
		else
		{
			SetElement_BuildingOperatorDict(nowBlockKey, value, context);
		}
		RemoveElement_BuildingOperatorDict(originalBlockKey, context);
		for (int i = 0; i < value.GetCount(); i++)
		{
			if (value.GetCollection()[i] != -1)
			{
				DomainManager.Taiwu.ChangeVillagerWorkBuildingBlockIndex(value.GetCollection()[i], nowBlockKey.BuildingBlockIndex, context);
			}
		}
	}

	private void ExchangeCollectBuildingResourceType(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		if (TryGetElement_CollectBuildingResourceType(originalBlockKey, out var value))
		{
			if (!TryGetElement_CollectBuildingResourceType(nowBlockKey, out var _))
			{
				AddElement_CollectBuildingResourceType(nowBlockKey, value, context);
			}
			else
			{
				SetElement_CollectBuildingResourceType(nowBlockKey, value, context);
			}
			RemoveElement_CollectBuildingResourceType(originalBlockKey, context);
		}
	}

	private void ExchangeBlockData(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey, HashSet<BuildingBlockKey> downgradedSet)
	{
		MapBlockItem mapBlockItem = MapBlock.Instance[DomainManager.Map.GetBlock(originalBlockKey.AreaId, originalBlockKey.BlockId).TemplateId];
		sbyte buildingAreaWidth = mapBlockItem.BuildingAreaWidth;
		BuildingBlockData buildingBlockData = GetElement_BuildingBlocks(originalBlockKey).Clone();
		BuildingBlockItem item = BuildingBlock.Instance.GetItem(buildingBlockData.TemplateId);
		bool flag = downgradedSet.Contains(originalBlockKey);
		downgradedSet.Remove(originalBlockKey);
		downgradedSet.Add(nowBlockKey);
		if (item.Class == EBuildingBlockClass.BornResource)
		{
			if (!flag)
			{
				if (item.Type == EBuildingBlockType.UselessResource)
				{
					Tester.Assert(buildingBlockData.Level > 1);
					buildingBlockData.Level--;
				}
				else
				{
					BuildingBlockDataEx element_BuildingBlockDataEx = DomainManager.Extra.GetElement_BuildingBlockDataEx((ulong)originalBlockKey);
					sbyte b = element_BuildingBlockDataEx.CalcUnlockedLevelCount();
					Tester.Assert(b > 1);
					element_BuildingBlockDataEx.LevelUnlockedFlags >>= 1;
					DomainManager.Extra.SetBuildingBlockDataEx(context, originalBlockKey, element_BuildingBlockDataEx);
				}
			}
		}
		else
		{
			buildingBlockData.Durability = (sbyte)(item.MaxDurability / 2);
		}
		for (int i = 0; i < item.Width; i++)
		{
			for (int j = 0; j < item.Width; j++)
			{
				short num = (short)(originalBlockKey.BuildingBlockIndex + i * buildingAreaWidth + j);
				BuildingBlockKey elementId = new BuildingBlockKey(originalBlockKey.AreaId, originalBlockKey.BlockId, num);
				BuildingBlockData value = new BuildingBlockData(num, 0, -1, -1);
				SetElement_BuildingBlocks(elementId, value, context);
			}
		}
		for (int k = 0; k < item.Width; k++)
		{
			for (int l = 0; l < item.Width; l++)
			{
				short num2 = (short)(nowBlockKey.BuildingBlockIndex + k * buildingAreaWidth + l);
				BuildingBlockKey elementId2 = new BuildingBlockKey(nowBlockKey.AreaId, nowBlockKey.BlockId, num2);
				BuildingBlockData buildingBlockData2 = ((num2 == nowBlockKey.BuildingBlockIndex) ? buildingBlockData : new BuildingBlockData(num2, -1, -1, nowBlockKey.BuildingBlockIndex));
				buildingBlockData2.BlockIndex = num2;
				SetElement_BuildingBlocks(elementId2, buildingBlockData2, context);
			}
		}
	}

	private void ExchangeBlockDataEx(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
	{
		BuildingBlockDataEx element_BuildingBlockDataEx = DomainManager.Extra.GetElement_BuildingBlockDataEx((ulong)originalBlockKey);
		BuildingBlockDataEx element_BuildingBlockDataEx2 = DomainManager.Extra.GetElement_BuildingBlockDataEx((ulong)nowBlockKey);
		BuildingBlockDataEx buildingBlockDataEx = element_BuildingBlockDataEx;
		BuildingBlockKey key = element_BuildingBlockDataEx.Key;
		BuildingBlockKey key2 = element_BuildingBlockDataEx2.Key;
		element_BuildingBlockDataEx2.Key = key;
		buildingBlockDataEx.Key = key2;
		DomainManager.Extra.SetBuildingBlockDataEx(context, nowBlockKey, element_BuildingBlockDataEx);
		DomainManager.Extra.SetBuildingBlockDataEx(context, originalBlockKey, element_BuildingBlockDataEx2);
	}

	private void InitializeComfortableHouses(DataContext context, Location location)
	{
		List<BuildingBlockKey> list = FindAllBuildingsWithSameTemplate(location, _buildingAreas[location], 47);
		foreach (BuildingBlockKey item in list)
		{
			AddElement_ComfortableHouses(item, default(CharacterList), context);
		}
	}

	private void InitializeResidences(DataContext context, Location location)
	{
		List<BuildingBlockKey> list = FindAllBuildingsWithSameTemplate(location, _buildingAreas[location], 46);
		ExtraDomain extra = DomainManager.Extra;
		foreach (BuildingBlockKey item in list)
		{
			BuildingBlockData value = _buildingBlocks[item];
			extra.ModifyBuildingExtraData(context, item, delegate(BuildingBlockDataEx blockDataEx)
			{
				for (int i = 0; i < 7; i++)
				{
					blockDataEx.LevelUnlockedFlags = BitOperation.SetBit(blockDataEx.LevelUnlockedFlags, i, true);
				}
			});
			SetElement_BuildingBlocks(item, value, context);
			AddElement_Residences(item, default(CharacterList), context);
		}
		UpdateHomeless(context, updateAll: true);
	}

	private void InitializeBuildingResidents(DataContext context)
	{
		BuildingBlockKey key;
		CharacterList value;
		foreach (KeyValuePair<BuildingBlockKey, CharacterList> residence in _residences)
		{
			residence.Deconstruct(out key, out value);
			BuildingBlockKey value2 = key;
			CharacterList characterList = value;
			foreach (int item in characterList.GetCollection())
			{
				_buildingResidents.Add(item, value2);
			}
		}
		foreach (KeyValuePair<BuildingBlockKey, CharacterList> comfortableHouse in _comfortableHouses)
		{
			comfortableHouse.Deconstruct(out key, out value);
			BuildingBlockKey value3 = key;
			CharacterList characterList2 = value;
			foreach (int item2 in characterList2.GetCollection())
			{
				_buildingComfortableHouses.Add(item2, value3);
			}
		}
	}

	private void FixResidentData(DataContext context)
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<BuildingBlockKey, CharacterList> residence in _residences)
		{
			for (int num = residence.Value.GetCount() - 1; num >= 0; num--)
			{
				int item = residence.Value.GetCollection()[num];
				if (!list.Contains(item))
				{
					list.Add(item);
				}
				else
				{
					residence.Value.GetCollection().Remove(item);
				}
			}
		}
		list.Clear();
		foreach (KeyValuePair<BuildingBlockKey, CharacterList> comfortableHouse in _comfortableHouses)
		{
			for (int num2 = comfortableHouse.Value.GetCount() - 1; num2 >= 0; num2--)
			{
				int item2 = comfortableHouse.Value.GetCollection()[num2];
				if (!list.Contains(item2))
				{
					list.Add(item2);
				}
				else
				{
					comfortableHouse.Value.GetCollection().Remove(item2);
				}
			}
		}
	}

	public void AddResidence(DataContext context, BuildingBlockKey key)
	{
		if (_residences.ContainsKey(key))
		{
			SetElement_Residences(key, default(CharacterList), context);
		}
		else
		{
			AddElement_Residences(key, default(CharacterList), context);
		}
	}

	public void RemoveResidence(DataContext context, BuildingBlockKey key)
	{
		if (!_residences.TryGetValue(key, out var value))
		{
			return;
		}
		RemoveElement_Residences(key, context);
		List<int> collection = value.GetCollection();
		foreach (int item in collection)
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var _))
			{
				AddTaiwuResident(context, item);
			}
			_buildingResidents.Remove(item);
		}
		value.Clear();
		List<short> autoCheckInResidenceList = DomainManager.Extra.GetAutoCheckInResidenceList();
		if (autoCheckInResidenceList.Contains(key.BuildingBlockIndex))
		{
			autoCheckInResidenceList.Remove(key.BuildingBlockIndex);
			DomainManager.Extra.SetAutoCheckInResidenceList(autoCheckInResidenceList, context);
		}
	}

	public void AddComfortableHouse(DataContext context, BuildingBlockKey key)
	{
		DomainManager.Extra.SetFeast(context, key, DomainManager.Extra.GetFeast(key));
		if (_comfortableHouses.ContainsKey(key))
		{
			SetElement_ComfortableHouses(key, default(CharacterList), context);
		}
		else
		{
			AddElement_ComfortableHouses(key, default(CharacterList), context);
		}
	}

	public void RemoveComfortableHouse(DataContext context, BuildingBlockKey key)
	{
		DomainManager.Extra.RemoveFeast(context, key);
		if (!_comfortableHouses.TryGetValue(key, out var value))
		{
			return;
		}
		RemoveElement_ComfortableHouses(key, context);
		List<int> collection = value.GetCollection();
		foreach (int item in collection)
		{
			_buildingComfortableHouses.Remove(item);
		}
		List<short> autoCheckInComfortableList = DomainManager.Extra.GetAutoCheckInComfortableList();
		if (autoCheckInComfortableList.Contains(key.BuildingBlockIndex))
		{
			autoCheckInComfortableList.Remove(key.BuildingBlockIndex);
			DomainManager.Extra.SetAutoCheckInComfortableList(autoCheckInComfortableList, context);
		}
	}

	private void RemoveExceedingResidents(DataContext context, BuildingBlockKey key)
	{
		if (_residences.TryGetValue(key, out var value))
		{
			CharacterList value2 = default(CharacterList);
			int levelEffect = BuildingScale.DefValue.ResidenceCapacity.GetLevelEffect(BuildingBlockLevel(key));
			List<int> collection = value.GetCollection();
			for (int i = 0; i < Math.Min(levelEffect, collection.Count); i++)
			{
				int charId = collection[i];
				value2.Add(charId);
			}
			for (int j = levelEffect; j < collection.Count; j++)
			{
				int num = collection[j];
				_buildingResidents.Remove(num);
				_homeless.Add(num);
			}
			value.Clear();
			SetElement_Residences(key, value2, context);
			SetHomeless(_homeless, context);
		}
	}

	public void SetAllResidenceAutoCheckIn(DataContext context)
	{
		List<Location> taiwuBuildingAreas = GetTaiwuBuildingAreas();
		for (int i = 0; i < taiwuBuildingAreas.Count; i++)
		{
			Location elementId = taiwuBuildingAreas[i];
			BuildingAreaData element_BuildingAreas = GetElement_BuildingAreas(elementId);
			for (short num = 0; num < element_BuildingAreas.Width * element_BuildingAreas.Width; num++)
			{
				BuildingBlockKey elementId2 = new BuildingBlockKey(elementId.AreaId, elementId.BlockId, num);
				BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(elementId2);
				if (element_BuildingBlocks.TemplateId == 46)
				{
					SetResidenceAutoCheckIn(context, elementId2.BuildingBlockIndex, isAutoCheckIn: true);
				}
			}
		}
	}

	[DomainMethod]
	public void AddToResidence(DataContext context, int charId, BuildingBlockKey buildingBlockKey)
	{
		RemoveTaiwuResident(context, charId);
		if (!_residences.ContainsKey(buildingBlockKey))
		{
			AddElement_Residences(buildingBlockKey, default(CharacterList), context);
		}
		AddCharToResidence(context, charId, buildingBlockKey);
	}

	[DomainMethod]
	public void RemoveFromResidence(DataContext context, int charId, BuildingBlockKey buildingBlockKey)
	{
		RemoveCharFromResidence(context, charId, buildingBlockKey);
		AddTaiwuResident(context, charId);
	}

	[DomainMethod]
	public void RemoveAllFormResidence(DataContext context, BuildingBlockKey buildingBlockKey)
	{
		CharacterList characterList = _residences[buildingBlockKey];
		for (int num = characterList.GetCount() - 1; num >= 0; num--)
		{
			int charId = characterList.GetCollection()[num];
			RemoveCharFromResidence(context, charId, buildingBlockKey);
			AddTaiwuResident(context, charId);
		}
	}

	[DomainMethod]
	public void RemoveAllFromComfortableHouse(DataContext context, BuildingBlockKey buildingBlockKey)
	{
		if (_comfortableHouses.ContainsKey(buildingBlockKey))
		{
			CharacterList characterList = _comfortableHouses[buildingBlockKey];
			for (int num = characterList.GetCount() - 1; num >= 0; num--)
			{
				int charId = characterList.GetCollection()[num];
				RemoveCharFromComfortableHouse(context, charId, buildingBlockKey);
			}
		}
	}

	[DomainMethod]
	public List<int> SortedComfortableHousePeople(DataContext context, List<int> charIdList)
	{
		for (int num = charIdList.Count - 1; num >= 0; num--)
		{
			int objectId = charIdList[num];
			if (!DomainManager.Character.TryGetElement_Objects(objectId, out var element) || element.GetAgeGroup() == 0)
			{
				charIdList.RemoveAt(num);
			}
		}
		charIdList.Sort((int l, int r) => DomainManager.Character.GetElement_Objects(l).GetHappiness().CompareTo(DomainManager.Character.GetElement_Objects(r).GetHappiness()));
		return charIdList;
	}

	[DomainMethod]
	public void ReplaceCharacterInResidence(DataContext context, int charId, BuildingBlockKey buildingBlockKey, sbyte index)
	{
		CharacterList value = _residences[buildingBlockKey];
		Tester.Assert(index >= 0 && value.GetCount() > index);
		List<int> collection = value.GetCollection();
		int num = collection[index];
		if (charId < 0)
		{
			RemoveFromResidence(context, num, buildingBlockKey);
			return;
		}
		if (_homeless.Contains(charId))
		{
			_homeless.Remove(charId);
			collection[index] = charId;
			_homeless.Add(num);
			SetHomeless(_homeless, context);
			_buildingResidents.Add(charId, buildingBlockKey);
			_buildingResidents.Remove(num);
		}
		else
		{
			BuildingBlockKey buildingBlockKey2 = _buildingResidents[charId];
			if (!_residences.TryGetValue(buildingBlockKey2, out var value2))
			{
				throw new Exception($"{charId}'s previous living status is unknown.");
			}
			List<int> collection2 = value2.GetCollection();
			int num2 = collection2.IndexOf(charId);
			Tester.Assert(num2 >= 0);
			collection2[num2] = num;
			collection[index] = charId;
			SetElement_Residences(buildingBlockKey2, value2, context);
			_buildingResidents[num] = buildingBlockKey2;
			_buildingResidents[charId] = buildingBlockKey;
		}
		SetElement_Residences(buildingBlockKey, value, context);
	}

	[DomainMethod]
	public void ReplaceCharacterInComfortableHouse(DataContext context, int charIdB, BuildingBlockKey buildingBlockKey, sbyte index)
	{
		CharacterList value = _comfortableHouses[buildingBlockKey];
		Tester.Assert(index >= 0 && value.GetCount() > index);
		List<int> collection = value.GetCollection();
		int num = collection[index];
		if (charIdB < 0)
		{
			RemoveFromComfortableHouse(context, num, buildingBlockKey);
			return;
		}
		if (_buildingComfortableHouses.TryGetValue(charIdB, out var value2))
		{
			CharacterList valueOrDefault = _comfortableHouses.GetValueOrDefault(value2);
			List<int> collection2 = valueOrDefault.GetCollection();
			int num2 = collection2.IndexOf(charIdB);
			Tester.Assert(num2 >= 0);
			collection2[num2] = num;
			collection[index] = charIdB;
			SetElement_ComfortableHouses(value2, valueOrDefault, context);
			_buildingComfortableHouses[num] = value2;
			_buildingComfortableHouses[charIdB] = buildingBlockKey;
		}
		else
		{
			collection[index] = charIdB;
			_buildingComfortableHouses.Add(charIdB, buildingBlockKey);
			_buildingComfortableHouses.Remove(num);
		}
		SetElement_ComfortableHouses(buildingBlockKey, value, context);
	}

	[DomainMethod]
	public void AddToComfortableHouse(DataContext context, int charId, BuildingBlockKey buildingBlockKey)
	{
		if (_buildingComfortableHouses.TryGetValue(charId, out var value) && _comfortableHouses.ContainsKey(value))
		{
			RemoveCharFromComfortableHouse(context, charId, value);
		}
		if (!_comfortableHouses.ContainsKey(buildingBlockKey))
		{
			AddElement_ComfortableHouses(buildingBlockKey, default(CharacterList), context);
		}
		AddCharToComfortableHouse(context, charId, buildingBlockKey);
	}

	[DomainMethod]
	public void RemoveFromComfortableHouse(DataContext context, int charId, BuildingBlockKey buildingBlockKey)
	{
		RemoveCharFromComfortableHouse(context, charId, buildingBlockKey);
	}

	[DomainMethod]
	public CharacterList QuickFillResidence(DataContext context, BuildingBlockKey buildingBlockKey)
	{
		if (!_residences.ContainsKey(buildingBlockKey))
		{
			AddElement_Residences(buildingBlockKey, default(CharacterList), context);
		}
		sbyte level = BuildingBlockLevel(buildingBlockKey);
		int[] array = _homeless.GetCollection().ToArray();
		int[] array2 = array;
		foreach (int num in array2)
		{
			if (_residences[buildingBlockKey].GetCount() >= BuildingScale.DefValue.ResidenceCapacity.GetLevelEffect(level))
			{
				break;
			}
			if (!DomainManager.Character.GetElement_Objects(num).IsCompletelyInfected())
			{
				AddCharToResidence(context, num, buildingBlockKey);
				CharacterList homeless = _homeless;
				homeless.Remove(num);
				SetHomeless(homeless, context);
			}
		}
		return _residences[buildingBlockKey];
	}

	[DomainMethod]
	public CharacterList QuickFillComfortableHouse(DataContext context, BuildingBlockKey buildingBlockKey)
	{
		sbyte level = BuildingBlockLevel(buildingBlockKey);
		int levelEffect = BuildingScale.DefValue.ComfortableHouseCapacity.GetLevelEffect(level);
		int num = levelEffect;
		if (_comfortableHouses.ContainsKey(buildingBlockKey))
		{
			num = levelEffect - _comfortableHouses[buildingBlockKey].GetCount();
			if (num <= 0)
			{
				return _comfortableHouses[buildingBlockKey];
			}
		}
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		foreach (KeyValuePair<BuildingBlockKey, CharacterList> residence in _residences)
		{
			list.AddRange(residence.Value.GetCollection());
		}
		list.AddRange(_homeless.GetCollection());
		for (int num2 = list.Count - 1; num2 >= 0; num2--)
		{
			int num3 = list[num2];
			if (_buildingComfortableHouses.ContainsKey(num3) || !DomainManager.Character.TryGetElement_Objects(num3, out var element) || element.GetAgeGroup() == 0)
			{
				list.RemoveAt(num2);
			}
		}
		list.Sort((int l, int r) => DomainManager.Character.GetElement_Objects(l).GetHappiness().CompareTo(DomainManager.Character.GetElement_Objects(r).GetHappiness()));
		List<int> range = list.GetRange(0, Math.Min(num, list.Count));
		for (int num4 = 0; num4 < range.Count; num4++)
		{
			int charId = range[num4];
			AddToComfortableHouse(context, charId, buildingBlockKey);
		}
		ObjectPool<List<int>>.Instance.Return(list);
		_comfortableHouses.TryGetValue(buildingBlockKey, out var value);
		return value;
	}

	[DomainMethod]
	public CharacterList GetCharsInResidence(DataContext context, BuildingBlockKey key)
	{
		return _residences.ContainsKey(key) ? _residences[key] : default(CharacterList);
	}

	[DomainMethod]
	public List<CharacterList> GetAllResidents(DataContext context, BuildingBlockKey blockKey, bool homelessFirst)
	{
		List<CharacterList> list = new List<CharacterList>(4);
		CharacterList characterList = default(CharacterList);
		CharacterList characterList2 = default(CharacterList);
		List<int> collection = _homeless.GetCollection();
		foreach (int item in collection)
		{
			if (DomainManager.Character.GetElement_Objects(item).IsCompletelyInfected())
			{
				CharacterList characterList3 = characterList;
				characterList3.Add(item);
				characterList = characterList3;
			}
			else
			{
				CharacterList characterList4 = characterList2;
				characterList4.Add(item);
				characterList2 = characterList4;
			}
		}
		if (homelessFirst)
		{
			list.Add(characterList2);
			list.Add(default(CharacterList));
			foreach (KeyValuePair<BuildingBlockKey, CharacterList> residence in _residences)
			{
				if (!residence.Key.Equals(blockKey))
				{
					CharacterList value = list[1];
					value.AddRange(residence.Value.GetCollection());
					list[1] = value;
				}
			}
		}
		else
		{
			list.Add(default(CharacterList));
			foreach (KeyValuePair<BuildingBlockKey, CharacterList> residence2 in _residences)
			{
				if (!residence2.Key.Equals(blockKey))
				{
					CharacterList value2 = list[0];
					value2.AddRange(residence2.Value.GetCollection());
					list[0] = value2;
				}
			}
			list.Add(characterList2);
		}
		list.Add(characterList);
		return list;
	}

	[DomainMethod]
	public CharacterList GetCharsInComfortableHouse(DataContext context, BuildingBlockKey key)
	{
		if (!_comfortableHouses.ContainsKey(key))
		{
			return default(CharacterList);
		}
		return _comfortableHouses[key];
	}

	[DomainMethod]
	public CharacterList GetHomeless(DataContext context)
	{
		return _homeless;
	}

	[DomainMethod]
	public void SetResidenceAutoCheckIn(DataContext context, short blockIndex, bool isAutoCheckIn)
	{
		List<short> autoCheckInResidenceList = DomainManager.Extra.GetAutoCheckInResidenceList();
		if (isAutoCheckIn && !autoCheckInResidenceList.Contains(blockIndex))
		{
			autoCheckInResidenceList.Add(blockIndex);
			DomainManager.Extra.SetAutoCheckInResidenceList(autoCheckInResidenceList, context);
		}
		if (!isAutoCheckIn && autoCheckInResidenceList.Contains(blockIndex))
		{
			autoCheckInResidenceList.Remove(blockIndex);
			DomainManager.Extra.SetAutoCheckInResidenceList(autoCheckInResidenceList, context);
		}
	}

	[DomainMethod]
	public void SetComfortableAutoCheckIn(DataContext context, short blockIndex, bool isAutoCheckIn)
	{
		List<short> autoCheckInComfortableList = DomainManager.Extra.GetAutoCheckInComfortableList();
		if (isAutoCheckIn && !autoCheckInComfortableList.Contains(blockIndex))
		{
			autoCheckInComfortableList.Add(blockIndex);
			DomainManager.Extra.SetAutoCheckInComfortableList(autoCheckInComfortableList, context);
		}
		if (!isAutoCheckIn && autoCheckInComfortableList.Contains(blockIndex))
		{
			autoCheckInComfortableList.Remove(blockIndex);
			DomainManager.Extra.SetAutoCheckInComfortableList(autoCheckInComfortableList, context);
		}
	}

	[DomainMethod]
	public (int sumCapacity, int residentsCount, int villagerSumCount) GetResidenceInfo()
	{
		int num = 0;
		foreach (KeyValuePair<BuildingBlockKey, CharacterList> residence in _residences)
		{
			sbyte b = BuildingBlockLevel(residence.Key);
			if (b > 0)
			{
				num += BuildingScale.DefValue.ResidenceCapacity.GetLevelEffect(b);
			}
		}
		return (sumCapacity: num, residentsCount: _buildingResidents.Count, villagerSumCount: _buildingResidents.Count + _homeless.GetCount());
	}

	public byte GetLivingStatus(int charId)
	{
		if (DomainManager.Taiwu.IsInGroup(charId))
		{
			return 1;
		}
		if (_buildingResidents.TryGetValue(charId, out var _))
		{
			return 2;
		}
		if (_homeless.Contains(charId))
		{
			return 0;
		}
		Logger.Warn($"Given character {charId} is not a taiwu resident.");
		return 0;
	}

	public void AddTaiwuResident(DataContext context, int charId, bool autoHousing = false)
	{
		if (autoHousing)
		{
			foreach (KeyValuePair<BuildingBlockKey, CharacterList> residence in _residences)
			{
				BuildingBlockData buildingBlockData = _buildingBlocks[residence.Key];
				sbyte b = BuildingBlockLevel(residence.Key);
				if (b <= 0 || residence.Value.GetCount() == BuildingScale.DefValue.ResidenceCapacity.GetLevelEffect(b))
				{
					continue;
				}
				AddCharToResidence(context, charId, residence.Key);
				return;
			}
		}
		_homeless.Add(charId);
		SetHomeless(_homeless, context);
	}

	public void MakeTargetHomeless(DataContext context, int charId)
	{
		if (!_homeless.Contains(charId) && _buildingResidents.ContainsKey(charId))
		{
			RemoveTaiwuResident(context, charId, removeFromHomeless: false);
			_homeless.Add(charId);
			SetHomeless(_homeless, context);
		}
	}

	public void RemoveTaiwuResident(DataContext context, int charId, bool removeFromHomeless = true)
	{
		if (_buildingResidents.TryGetValue(charId, out var value))
		{
			if (_residences.ContainsKey(value))
			{
				RemoveCharFromResidence(context, charId, value);
			}
		}
		else if (removeFromHomeless && _homeless.Contains(charId))
		{
			_homeless.Remove(charId);
			SetHomeless(_homeless, context);
		}
	}

	private void UpdateHomeless(DataContext context, bool updateAll)
	{
		if (_homeless.GetCount() <= 0)
		{
			return;
		}
		List<int> collection = _homeless.GetCollection();
		if (updateAll)
		{
			_homeless = default(CharacterList);
			SetHomeless(_homeless, context);
			{
				foreach (int item in collection)
				{
					AddTaiwuResident(context, item, !DomainManager.Character.GetElement_Objects(item).IsCompletelyInfected());
				}
				return;
			}
		}
		foreach (int item2 in collection)
		{
			if (DomainManager.Character.GetElement_Objects(item2).IsCompletelyInfected())
			{
				continue;
			}
			CharacterList homeless = _homeless;
			homeless.Remove(item2);
			SetHomeless(homeless, context);
			AddTaiwuResident(context, item2, autoHousing: true);
			break;
		}
	}

	public void SetAllVillagerHomeless(DataContext context)
	{
		foreach (int key in _buildingResidents.Keys)
		{
			MakeTargetHomeless(context, key);
		}
	}

	private void UpdateResidentsHappinessAndFavor(DataContext context)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		List<int> collection = _homeless.GetCollection();
		foreach (int item in collection)
		{
			UpdateHomelessHappinessAndFavor(item);
		}
		foreach (KeyValuePair<BuildingBlockKey, CharacterList> residence in _residences)
		{
			BuildingBlockKey key = residence.Key;
			collection = residence.Value.GetCollection();
			BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(key);
			if (element_BuildingBlocks.CanUse())
			{
				continue;
			}
			foreach (int item2 in collection)
			{
				UpdateHomelessHappinessAndFavor(item2);
			}
		}
		static int ClampDelta(int curValue, int delta)
		{
			if (curValue <= 0)
			{
				return 0;
			}
			return (curValue + delta < 0) ? (-curValue) : delta;
		}
		void UpdateHomelessHappinessAndFavor(int charId)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
			short favorability = DomainManager.Character.GetFavorability(charId, taiwuCharId);
			if (favorability > 0)
			{
				int num = ClampDelta(favorability, GlobalConfig.Instance.HomelessFavorabilityChangePerMonth);
				if (favorability + num < 0)
				{
					num = -favorability;
				}
				if (favorability > 0)
				{
					DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, element_Objects, taiwu, num);
				}
			}
			sbyte happiness = element_Objects.GetHappiness();
			if (happiness > 0)
			{
				int delta = ClampDelta(happiness, GlobalConfig.Instance.HomelessHappinessChangePerMonth);
				element_Objects.ChangeHappiness(context, delta);
			}
		}
	}

	private void AddCharToResidence(DataContext context, int charId, BuildingBlockKey buildingBlockKey)
	{
		_buildingResidents.Add(charId, buildingBlockKey);
		CharacterList value = _residences[buildingBlockKey];
		value.Add(charId);
		SetElement_Residences(buildingBlockKey, value, context);
	}

	private void RemoveCharFromResidence(DataContext context, int charId, BuildingBlockKey buildingBlockKey)
	{
		_buildingResidents.Remove(charId);
		CharacterList value = _residences[buildingBlockKey];
		value.Remove(charId);
		SetElement_Residences(buildingBlockKey, value, context);
	}

	private void AddCharToComfortableHouse(DataContext context, int charId, BuildingBlockKey buildingBlockKey)
	{
		_buildingComfortableHouses.Add(charId, buildingBlockKey);
		CharacterList value = _comfortableHouses[buildingBlockKey];
		value.Add(charId);
		SetElement_ComfortableHouses(buildingBlockKey, value, context);
	}

	private void RemoveCharFromComfortableHouse(DataContext context, int charId, BuildingBlockKey buildingBlockKey)
	{
		_buildingComfortableHouses.Remove(charId);
		CharacterList value = _comfortableHouses[buildingBlockKey];
		value.Remove(charId);
		SetElement_ComfortableHouses(buildingBlockKey, value, context);
	}

	public void SetCharacterParticipantFeast(int charId, short feastType, int happiness, ItemKey dishCopy)
	{
		_feastParticipants[charId] = (feastType, happiness, dishCopy);
	}

	public void ClearFeastParticipants()
	{
		_feastParticipants.Clear();
	}

	public Dictionary<BuildingBlockKey, CharacterList> GetAllComfortableHouses()
	{
		return _comfortableHouses;
	}

	public bool IsCharacterParticipantFeast(int charId, out (short, int, ItemKey) value)
	{
		return _feastParticipants.TryGetValue(charId, out value);
	}

	public bool IsCharacterAbleToJoinFeast(int charId)
	{
		if (DomainManager.Taiwu.IsInGroup(charId))
		{
			return false;
		}
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			return false;
		}
		if (element.GetCreatingType() != 1)
		{
			return false;
		}
		return (element.GetOrganizationInfo().OrgTemplateId == 16) ? (element.GetAgeGroup() != 0) : (element.GetLocation().AreaId == DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId && DomainManager.Character.GetFavorabilityType(charId, DomainManager.Taiwu.GetTaiwuCharId()) >= 2 && CharacterMatcher.DefValue.CanJoinFeast.Match(element));
	}

	public void FeastAdvanceMonth_Complement(DataContext context)
	{
		foreach (var (objectId, tuple2) in _feastParticipants)
		{
			if (DomainManager.Character.TryGetElement_Objects(objectId, out var element) && element.GetEatingItems().GetAvailableEatingSlot(element.GetCurrMaxEatingSlotsCount()) >= 0)
			{
				element.AddEatingItem(context, tuple2.Item3);
			}
		}
	}

	public void TryRemoveFeastCustomer(DataContext context, int charId)
	{
		if (_buildingComfortableHouses.TryGetValue(charId, out var value) && _comfortableHouses.ContainsKey(value) && !IsCharacterAbleToJoinFeast(charId))
		{
			RemoveCharFromComfortableHouse(context, charId, value);
		}
	}

	[DomainMethod]
	public List<CharacterDisplayData> GetFeastTargetCharList(DataContext context, BuildingBlockKey buildingBlockKey)
	{
		CharacterList charsInComfortableHouse = GetCharsInComfortableHouse(context, buildingBlockKey);
		List<CharacterDisplayData> list = new List<CharacterDisplayData>();
		List<CharacterList> allResidents = GetAllResidents(context, buildingBlockKey, homelessFirst: false);
		foreach (CharacterList item in allResidents)
		{
			foreach (int item2 in item.GetCollection())
			{
				if (!charsInComfortableHouse.Contains(item2) && IsCharacterAbleToJoinFeast(item2))
				{
					CharacterDisplayData characterDisplayData = DomainManager.Character.GetCharacterDisplayData(item2);
					list.Add(characterDisplayData);
				}
			}
		}
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(buildingBlockKey.AreaId);
		Span<MapBlockData> span = areaBlocks;
		for (int i = 0; i < span.Length; i++)
		{
			MapBlockData mapBlockData = span[i];
			if (mapBlockData.CharacterSet == null)
			{
				continue;
			}
			foreach (int charId in mapBlockData.CharacterSet)
			{
				if (!charsInComfortableHouse.Contains(charId) && !list.Any((CharacterDisplayData d) => d.CharacterId == charId) && IsCharacterAbleToJoinFeast(charId))
				{
					CharacterDisplayData characterDisplayData2 = DomainManager.Character.GetCharacterDisplayData(charId);
					list.Add(characterDisplayData2);
				}
			}
		}
		return list;
	}

	[DomainMethod]
	public int GetTaiwuVillageResourceBlockEffect(DataContext context, EBuildingScaleEffect effectType)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		return GetBuildingBlockEffect(taiwuVillageLocation, effectType);
	}

	[DomainMethod]
	public int GetTaiwuLocationResourceBlockEffect(DataContext context, EBuildingScaleEffect effectType)
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		return GetBuildingBlockEffect(location, effectType);
	}

	private int CalcResourceBlockTotalEffectValue(int formulaTemplateId, Span<int> baseValues)
	{
		BuildingFormulaItem formula = BuildingFormula.Instance[formulaTemplateId];
		int num = 0;
		for (int i = 0; i < baseValues.Length; i++)
		{
			num += formula.Calculate(baseValues[i]);
		}
		return num;
	}

	public (ResourceInts resourcesChange, int expGain) CalcResourceBlockIncomeEffects(Location location)
	{
		ResourceInts item = default(ResourceInts);
		int num = 0;
		Span<int> values = stackalloc int[5];
		for (short num2 = 1; num2 < 11; num2++)
		{
			CalcResourceBlockEffectBaseValues(location, num2, ref values);
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[num2];
			List<short> expandInfos = buildingBlockItem.ExpandInfos;
			if (expandInfos != null && expandInfos.Count > 0)
			{
				foreach (short expandInfo in buildingBlockItem.ExpandInfos)
				{
					BuildingScaleItem buildingScaleItem = BuildingScale.Instance[expandInfo];
					switch (buildingScaleItem.Class)
					{
					case EBuildingScaleClass.MemberResourceIncome:
					{
						int value = CalcResourceBlockTotalEffectValue(buildingScaleItem.Formula, values);
						item.Add(buildingScaleItem.ResourceType, value);
						break;
					}
					case EBuildingScaleClass.MemberExpIncome:
					{
						int num3 = CalcResourceBlockTotalEffectValue(buildingScaleItem.Formula, values);
						num += num3;
						break;
					}
					}
				}
			}
		}
		return (resourcesChange: item, expGain: num);
	}

	public void CalcResourceBlockEffectBaseValues(Location location, short templateId, ref Span<int> values)
	{
		List<(BuildingBlockKey, int)> list = new List<(BuildingBlockKey, int)>();
		GetTopLevelBlocks(templateId, location, list, 5);
		for (int i = 0; i < values.Length; i++)
		{
			int num = GetPercentage(i);
			values[i] = ((list.Count > i) ? (list[i].Item2 * num / 100) : 0);
		}
		static int GetPercentage(int index)
		{
			return 100 * (5 - index) / 5;
		}
	}

	public void FinalizeResourceBlockIncomeEffectValues(ref ResourceInts resourceChange, bool isTaiwu)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		CValuePercent val = CValuePercent.op_Implicit(DomainManager.World.GetGainResourcePercent((byte)(isTaiwu ? 1 : 8)));
		CValuePercent val2 = CValuePercent.op_Implicit(DomainManager.World.GetGainResourcePercent((byte)(isTaiwu ? 9 : 8)));
		for (sbyte b = 0; b < 6; b++)
		{
			ref int reference = ref resourceChange[b];
			reference *= val;
		}
		for (sbyte b2 = 6; b2 <= 7; b2++)
		{
			ref int reference2 = ref resourceChange[b2];
			reference2 *= val2;
		}
	}

	private void InitializeSamsaraPlatform()
	{
		_samsaraPlatformAddMainAttributes.Initialize();
		_samsaraPlatformAddCombatSkillQualifications.Initialize();
		_samsaraPlatformAddLifeSkillQualifications.Initialize();
		for (int i = 0; i < _samsaraPlatformSlots.Length; i++)
		{
			_samsaraPlatformSlots[i] = new IntPair(-1, 0);
		}
	}

	private List<SamsaraPlatformCharDisplayData> GetSamsaraPlatformCharList(DataContext context, bool excludeCharactersInSlot)
	{
		List<SamsaraPlatformCharDisplayData> list = new List<SamsaraPlatformCharDisplayData>();
		HashSet<int> hashSet = ObjectPool<HashSet<int>>.Instance.Get();
		HashSet<int> hashSet2 = ObjectPool<HashSet<int>>.Instance.Get();
		DomainManager.Character.GetAllRelatedDeadCharIds(DomainManager.Taiwu.GetTaiwuCharId(), hashSet, includeGeneral: false);
		DomainManager.Character.GetTaiwuVillageDeadCharacter(hashSet);
		DomainManager.Character.GetAllWaitingReincarnationCharIds(hashSet2);
		foreach (int item3 in hashSet)
		{
			if (!hashSet2.Contains(item3) || IsCharOnSamsaraPlatform(item3, checkSlots: false) || ProfessionSkillHandle.BuddhistMonkSkill_IsDirectedSamsaraCharacter(item3))
			{
				continue;
			}
			if (excludeCharactersInSlot)
			{
				bool flag = false;
				IntPair[] samsaraPlatformSlots = _samsaraPlatformSlots;
				for (int i = 0; i < samsaraPlatformSlots.Length; i++)
				{
					IntPair intPair = samsaraPlatformSlots[i];
					if (intPair.First == item3)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					continue;
				}
			}
			DeadCharacter deadCharacter = DomainManager.Character.GetDeadCharacter(item3);
			SamsaraPlatformCharDisplayData item = new SamsaraPlatformCharDisplayData
			{
				Id = item3,
				TemplateId = deadCharacter.TemplateId,
				NameRelatedData = DomainManager.Character.GetNameRelatedData(item3),
				AvatarRelatedData = deadCharacter.GenerateAvatarRelatedData(),
				MainAttributes = deadCharacter.BaseMainAttributes,
				CombatSkillQualifications = deadCharacter.BaseCombatSkillQualifications,
				LifeSkillQualifications = deadCharacter.BaseLifeSkillQualifications
			};
			list.Add(item);
		}
		if (DomainManager.Extra.IsExtraTaskInProgress(183) || DomainManager.Extra.IsExtraTaskInProgress(184))
		{
			DeadCharacter deadCharacter2 = DomainManager.Character.TryGetDeadCharacterByTemplateId(748);
			if (deadCharacter2 == null)
			{
				GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 748);
				deadCharacter2 = DomainManager.Character.SectMainStoryJingangCreateDeadMonk(context, orCreateFixedCharacterByTemplateId);
			}
			int num = DomainManager.Character.TryGetDeadCharacterIdByTemplateId(748);
			Tester.Assert(num != -1);
			SamsaraPlatformCharDisplayData item2 = new SamsaraPlatformCharDisplayData
			{
				Id = num,
				TemplateId = deadCharacter2.TemplateId,
				NameRelatedData = DomainManager.Character.GetNameRelatedData(num),
				AvatarRelatedData = deadCharacter2.GenerateAvatarRelatedData(),
				MainAttributes = deadCharacter2.BaseMainAttributes,
				CombatSkillQualifications = deadCharacter2.BaseCombatSkillQualifications,
				LifeSkillQualifications = deadCharacter2.BaseLifeSkillQualifications
			};
			list.Add(item2);
		}
		ObjectPool<HashSet<int>>.Instance.Return(hashSet);
		ObjectPool<HashSet<int>>.Instance.Return(hashSet2);
		return list;
	}

	[DomainMethod]
	public List<SamsaraPlatformCharDisplayData> GetSamsaraPlatformCharList(DataContext context)
	{
		return GetSamsaraPlatformCharList(context, excludeCharactersInSlot: false);
	}

	[DomainMethod]
	public void SetSamsaraPlatformChar(DataContext context, sbyte destinyType, int charId)
	{
		SetElement_SamsaraPlatformSlots(destinyType, new IntPair(charId, 0), context);
		if (!DomainManager.Extra.IsExtraTaskChainInProgress(35))
		{
			return;
		}
		int num = DomainManager.Character.TryGetDeadCharacterIdByTemplateId(748);
		bool flag = false;
		IntPair[] samsaraPlatformSlots = _samsaraPlatformSlots;
		for (int i = 0; i < samsaraPlatformSlots.Length; i++)
		{
			IntPair intPair = samsaraPlatformSlots[i];
			if (intPair.First == num)
			{
				flag = true;
			}
		}
		if (DomainManager.Extra.IsExtraTaskInProgress(183) && flag)
		{
			DomainManager.Extra.TriggerExtraTask(context, 35, 184);
		}
		if (DomainManager.Extra.IsExtraTaskInProgress(184) && !flag)
		{
			DomainManager.Extra.TriggerExtraTask(context, 35, 183);
		}
	}

	[DomainMethod]
	public unsafe CharacterDisplayData SamsaraPlatformReborn(DataContext context, sbyte destinyType)
	{
		IntPair intPair = _samsaraPlatformSlots[destinyType];
		if (DomainManager.Extra.IsExtraTaskInProgress(184) && DomainManager.Character.GetDeadCharacter(intPair.First).TemplateId == 748)
		{
			DomainManager.TaiwuEvent.OnEvent_JingangSectMainStoryReborn();
			SamsaraPlatformRecordCollection samsaraPlatformRecordCollection = DomainManager.Extra.GetSamsaraPlatformRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			int first = intPair.First;
			samsaraPlatformRecordCollection.AddSamsaraFailed(currDate, first, destinyType);
			DomainManager.Extra.CommitSamsaraPlatformRecord(context);
			return null;
		}
		if (intPair.First < 0 || intPair.Second < GlobalConfig.Instance.SamsaraPlatformMaxProgress)
		{
			return null;
		}
		DestinyTypeItem destinyConfig = DestinyType.Instance[destinyType];
		bool flag = context.Random.CheckPercentProb(GlobalConfig.Instance.SamsaraPlatformBornInSectOdds);
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		if (flag)
		{
			AddSamsaraPlatformSectRandomPool(destinyConfig, list);
		}
		if (list.Count == 0)
		{
			AddSamsaraPlatformCityRandomPool(destinyConfig, list);
		}
		if (list.Count == 0 && !flag)
		{
			AddSamsaraPlatformSectRandomPool(destinyConfig, list);
		}
		int num = -1;
		if (list.Count > 0)
		{
			num = list[context.Random.Next(list.Count)];
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num);
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			AddElement_SamsaraPlatformBornDict(num, new IntPair(intPair.First, destinyType), context);
			if (!DomainManager.Character.TryGetPregnantState(num, out var _))
			{
				DomainManager.Character.RemovePregnantLock(context, num);
				DomainManager.Character.MakePregnantWithoutMale(context, element_Objects);
			}
			switch (destinyType)
			{
			case 0:
				lifeRecordCollection.AddPregnantWithSamsara0(num, DomainManager.World.GetCurrDate(), element_Objects.GetLocation());
				break;
			case 1:
				lifeRecordCollection.AddPregnantWithSamsara1(num, DomainManager.World.GetCurrDate(), element_Objects.GetLocation());
				break;
			case 2:
				lifeRecordCollection.AddPregnantWithSamsara2(num, DomainManager.World.GetCurrDate(), element_Objects.GetLocation());
				break;
			case 3:
				lifeRecordCollection.AddPregnantWithSamsara3(num, DomainManager.World.GetCurrDate(), element_Objects.GetLocation());
				break;
			case 4:
				lifeRecordCollection.AddPregnantWithSamsara4(num, DomainManager.World.GetCurrDate(), element_Objects.GetLocation());
				break;
			case 5:
				lifeRecordCollection.AddPregnantWithSamsara5(num, DomainManager.World.GetCurrDate(), element_Objects.GetLocation());
				break;
			}
		}
		ObjectPool<List<int>>.Instance.Return(list);
		DeadCharacter deadCharacter = DomainManager.Character.GetDeadCharacter(intPair.First);
		MainAttributes baseMainAttributes = deadCharacter.BaseMainAttributes;
		CombatSkillShorts baseCombatSkillQualifications = deadCharacter.BaseCombatSkillQualifications;
		LifeSkillShorts baseLifeSkillQualifications = deadCharacter.BaseLifeSkillQualifications;
		List<Location> taiwuBuildingAreas = GetTaiwuBuildingAreas();
		sbyte b = 0;
		for (int i = 0; i < taiwuBuildingAreas.Count; i++)
		{
			Location location = taiwuBuildingAreas[i];
			BuildingAreaData element_BuildingAreas = GetElement_BuildingAreas(location);
			BuildingBlockKey buildingBlockKey = FindBuildingKey(location, element_BuildingAreas, 50);
			if (!buildingBlockKey.IsInvalid && DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)buildingBlockKey, out var value))
			{
				b = value.CalcUnlockedLevelCount();
				break;
			}
		}
		int num2 = GlobalConfig.Instance.SamsaraPlatformAddBasePercent + GlobalConfig.Instance.SamsaraPlatformAddPercentPerLevel * b;
		for (int j = 0; j < 6; j++)
		{
			_samsaraPlatformAddMainAttributes.Items[j] = (short)Math.Max(_samsaraPlatformAddMainAttributes.Items[j], baseMainAttributes.Items[j] * num2 / 100);
		}
		SetSamsaraPlatformAddMainAttributes(_samsaraPlatformAddMainAttributes, context);
		for (int k = 0; k < 14; k++)
		{
			_samsaraPlatformAddCombatSkillQualifications.Items[k] = (short)Math.Max(_samsaraPlatformAddCombatSkillQualifications.Items[k], baseCombatSkillQualifications.Items[k] * num2 / 100);
		}
		SetSamsaraPlatformAddCombatSkillQualifications(ref _samsaraPlatformAddCombatSkillQualifications, context);
		for (int l = 0; l < 16; l++)
		{
			_samsaraPlatformAddLifeSkillQualifications.Items[l] = (short)Math.Max(_samsaraPlatformAddLifeSkillQualifications.Items[l], baseLifeSkillQualifications.Items[l] * num2 / 100);
		}
		SetSamsaraPlatformAddLifeSkillQualifications(ref _samsaraPlatformAddLifeSkillQualifications, context);
		SetElement_SamsaraPlatformSlots(destinyType, new IntPair(-1, 0), context);
		CharacterDisplayData characterDisplayData = null;
		if (num >= 0)
		{
			OrganizationInfo organizationInfo = DomainManager.Character.GetElement_Objects(num).GetOrganizationInfo();
			short settlementId = organizationInfo.SettlementId;
			characterDisplayData = DomainManager.Character.GetCharacterDisplayData(num);
			characterDisplayData.Location = DomainManager.Organization.GetSettlement(settlementId).GetLocation();
			SamsaraPlatformRecordCollection samsaraPlatformRecordCollection2 = DomainManager.Extra.GetSamsaraPlatformRecordCollection();
			int currDate2 = DomainManager.World.GetCurrDate();
			int first2 = intPair.First;
			samsaraPlatformRecordCollection2.AddSamsaraSuccess(currDate2, first2, destinyType, settlementId, organizationInfo.OrgTemplateId, organizationInfo.Grade, organizationInfo.Principal, characterDisplayData.Gender, num);
			DomainManager.Extra.CommitSamsaraPlatformRecord(context);
		}
		int arg = baseMainAttributes.GetSum() + baseLifeSkillQualifications.GetSum() + baseCombatSkillQualifications.GetSum();
		int baseDelta = ProfessionFormulaImpl.Calculate(46, arg);
		DomainManager.Extra.ChangeProfessionSeniority(context, 6, baseDelta);
		return characterDisplayData;
	}

	[DomainMethod]
	public void SectMainStoryJingangClickMonkSoulBtn(DataContext context)
	{
		DomainManager.TaiwuEvent.OnEvent_JingangSectMainStoryMonkSoul();
	}

	private void AddSamsaraPlatformSectRandomPool(DestinyTypeItem destinyConfig, List<int> motherRandomPool)
	{
		foreach (sbyte sect in destinyConfig.SectList)
		{
			OrgMemberCollection members = DomainManager.Organization.GetSettlementByOrgTemplateId(sect).GetMembers();
			sbyte[] organizationGradeRange = destinyConfig.OrganizationGradeRange;
			foreach (sbyte grade in organizationGradeRange)
			{
				HashSet<int> members2 = members.GetMembers(grade);
				foreach (int item in members2)
				{
					if (DomainManager.Character.TryGetElement_Objects(item, out var element) && DomainManager.Character.CanSelectForForcePregnancy(element))
					{
						motherRandomPool.Add(item);
					}
				}
			}
		}
	}

	private void AddSamsaraPlatformCityRandomPool(DestinyTypeItem destinyConfig, List<int> motherRandomPool)
	{
		List<Settlement> list = new List<Settlement>();
		DomainManager.Organization.GetAllCivilianSettlements(list);
		for (int i = 0; i < list.Count; i++)
		{
			OrgMemberCollection members = list[i].GetMembers();
			sbyte[] organizationGradeRange = destinyConfig.OrganizationGradeRange;
			foreach (sbyte grade in organizationGradeRange)
			{
				HashSet<int> members2 = members.GetMembers(grade);
				foreach (int item in members2)
				{
					if (DomainManager.Character.TryGetElement_Objects(item, out var element) && DomainManager.Character.CanSelectForForcePregnancy(element))
					{
						motherRandomPool.Add(item);
					}
				}
			}
		}
	}

	[Obsolete]
	private bool CanCharPregnantBySamsaraPlatform(int charId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		List<short> featureIds = element_Objects.GetFeatureIds();
		if (element_Objects.GetGender() != 0 || element_Objects.GetAgeGroup() != 2 || element_Objects.GetKidnapperId() >= 0 || featureIds.Contains(218))
		{
			return false;
		}
		if ((DomainManager.Character.TryGetPregnantState(charId, out var pregnantState) && (!pregnantState.IsHuman || _samsaraPlatformBornDict.ContainsKey(charId))) || ProfessionSkillHandle.BuddhistMonkSkill_IsDirectedSamsaraMother(charId))
		{
			return false;
		}
		HashSet<int> relatedCharIds = DomainManager.Character.GetRelatedCharIds(charId, 1024);
		bool result = false;
		foreach (int item in relatedCharIds)
		{
			if (DomainManager.Character.IsCharacterAlive(item))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private void AddSamsaraPlatformProgress(DataContext context, sbyte level)
	{
		HashSet<int> hashSet = ObjectPool<HashSet<int>>.Instance.Get();
		DomainManager.Character.GetAllRelatedDeadCharIds(DomainManager.Taiwu.GetTaiwuCharId(), hashSet, includeGeneral: false);
		for (int i = 0; i < _samsaraPlatformSlots.Length; i++)
		{
			IntPair value = _samsaraPlatformSlots[i];
			if (value.First >= 0)
			{
				value.Second = Math.Min(value.Second + level, GlobalConfig.Instance.SamsaraPlatformMaxProgress);
				if (DomainManager.Extra.IsExtraTaskInProgress(184) && DomainManager.Character.GetDeadCharacter(value.First).TemplateId == 748)
				{
					value.Second = 1;
				}
				if (hashSet.Contains(value.First) && value.Second >= GlobalConfig.Instance.SamsaraPlatformMaxProgress)
				{
					DomainManager.World.GetInstantNotificationCollection().AddReincarnationArchitectureReincarnationEnd(value.First);
				}
				SetElement_SamsaraPlatformSlots(i, value, context);
			}
		}
	}

	public bool IsCharOnSamsaraPlatform(int charId, bool checkSlots = true)
	{
		if (checkSlots)
		{
			for (int i = 0; i < _samsaraPlatformSlots.Length; i++)
			{
				if (_samsaraPlatformSlots[i].First == charId)
				{
					return true;
				}
			}
		}
		foreach (IntPair value in _samsaraPlatformBornDict.Values)
		{
			if (value.First == charId)
			{
				return true;
			}
		}
		return false;
	}

	public void TryRemoveSamsaraPlatformBornData(DataContext context, int motherId)
	{
		if (_samsaraPlatformBornDict.ContainsKey(motherId))
		{
			RemoveElement_SamsaraPlatformBornDict(motherId, context);
		}
	}

	[DomainMethod]
	public List<SamsaraPlatformCharDisplayData> GetSwapSoulCeremonySoulCharIdList(DataContext context)
	{
		return GetSamsaraPlatformCharList(context, excludeCharactersInSlot: true);
	}

	[DomainMethod]
	public List<int> GetSwapSoulCeremonyBodyCharIdList()
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		HashSet<int> hashSet = new HashSet<int>();
		OrgMemberCollection members = DomainManager.Organization.GetSettlementByOrgTemplateId(16).GetMembers();
		List<int> list = new List<int>();
		members.GetAllMembers(list);
		hashSet.UnionWith(list);
		CharacterSet groupCharIds = DomainManager.Taiwu.GetGroupCharIds();
		if (groupCharIds.GetCount() > 0)
		{
			hashSet.UnionWith(groupCharIds.GetCollection());
		}
		if (DomainManager.Character.TryGetKidnappedCharacters(taiwuCharId, out var list2))
		{
			hashSet.UnionWith(list2.GetCollection().ConvertAll((KidnappedCharacter e) => e.CharId));
		}
		hashSet.RemoveWhere(delegate(int e)
		{
			if (e == taiwuCharId)
			{
				return true;
			}
			GameData.Domains.Character.Character element;
			return !DomainManager.Character.TryGetElement_Objects(e, out element) || element.GetAgeGroup() != 2;
		});
		List<int> list3 = new List<int>();
		list3.AddRange(hashSet);
		return list3;
	}

	[DomainMethod]
	public byte TrySwapSoulCeremony(DataContext context, int soulCharId, int bodyCharId, AvatarExtraData avatarExtraData)
	{
		DeadCharacter deadCharacter;
		GameData.Domains.Character.Character bodyCharacter;
		byte possessionResult = GetPossessionResult(soulCharId, bodyCharId, out deadCharacter, out bodyCharacter);
		if (possessionResult != PossessionResult.Success)
		{
			return possessionResult;
		}
		DomainManager.Extra.TryAddCharacterAvatarExtraData(context, bodyCharId, avatarExtraData);
		if (!DomainManager.Character.TryGetElement_Objects(_temporaryPossessionCharId, out var element))
		{
			return PossessionResult.InvalidBodyCharId;
		}
		DomainManager.Character.Possession(context, bodyCharId, deadCharacter, bodyCharacter, new AvatarData(element.GetAvatar()));
		DomainManager.Character.PossessionRemoveWaitingReincarnationChar(context, soulCharId);
		DomainManager.Extra.RecordPossessionData(context, bodyCharId, new PossessionData(soulCharId));
		RemoveTemporaryPossessionCharacter(context);
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenGetItem_Character, new List<int> { bodyCharacter.GetId() }, arg2: true);
		DomainManager.Taiwu.SetVillagerRole(context, bodyCharId, -1);
		return possessionResult;
	}

	[DomainMethod]
	public PossessionPreview GetPossessionPreview(DataContext context, int soulCharId, int bodyCharId)
	{
		DeadCharacter deadCharacter;
		GameData.Domains.Character.Character bodyCharacter;
		PossessionPreview possessionPreview = new PossessionPreview
		{
			Result = GetPossessionResult(soulCharId, bodyCharId, out deadCharacter, out bodyCharacter)
		};
		if (possessionPreview.Result != PossessionResult.Success)
		{
			return possessionPreview;
		}
		RemoveTemporaryPossessionCharacter(context);
		GameData.Domains.Character.Character character = DomainManager.Character.CreateTemporaryCopyOfCharacter(context, bodyCharacter);
		_temporaryPossessionCharId = character.GetId();
		DomainManager.Character.Possession(context, _temporaryPossessionCharId, deadCharacter, character, null);
		possessionPreview.Id = _temporaryPossessionCharId;
		possessionPreview.Age = character.GetActualAge();
		possessionPreview.BirthDate = character.GetBirthDate();
		possessionPreview.Health = character.GetHealth();
		possessionPreview.Gender = character.GetGender();
		possessionPreview.BaseMorality = character.GetBaseMorality();
		possessionPreview.Happiness = character.GetHappiness();
		possessionPreview.Fame = character.GetFame();
		possessionPreview.Personalities = character.GetPersonalities();
		possessionPreview.BirthFeatureId = character.GetGroupFeature(183);
		possessionPreview.FeatureIds = new List<short>();
		possessionPreview.BaseMainAttributes = deadCharacter.BaseMainAttributes;
		possessionPreview.BaseLifeSkillQualifications = deadCharacter.BaseLifeSkillQualifications;
		possessionPreview.BaseCombatSkillQualifications = deadCharacter.BaseCombatSkillQualifications;
		possessionPreview.ConsummateLevel = character.GetConsummateLevel();
		possessionPreview.NeiliAllocation = character.GetNeiliAllocation();
		possessionPreview.CurrNeili = character.GetCurrNeili();
		possessionPreview.BaseNeiliProportionOfFiveElements = character.GetBaseNeiliProportionOfFiveElements();
		possessionPreview.CharacterSamsaraData = DomainManager.Character.GetCharacterSamsaraData(_temporaryPossessionCharId);
		foreach (short featureId in character.GetFeatureIds())
		{
			if (CharacterFeature.Instance[featureId].MutexGroupId != 183)
			{
				possessionPreview.FeatureIds.Add(featureId);
			}
		}
		return possessionPreview;
	}

	public byte GetPossessionResult(int soulCharId, int bodyCharId, out DeadCharacter deadCharacter, out GameData.Domains.Character.Character bodyCharacter, bool mustGetKidnapped = false)
	{
		deadCharacter = null;
		bodyCharacter = null;
		if (!DomainManager.Character.TryGetDeadCharacter(soulCharId, out deadCharacter))
		{
			return PossessionResult.InvalidSoulCharId;
		}
		if (!DomainManager.Character.TryGetElement_Objects(bodyCharId, out bodyCharacter))
		{
			return PossessionResult.InvalidBodyCharId;
		}
		if (mustGetKidnapped)
		{
			if (!DomainManager.Character.TryGetKidnappedCharacters(DomainManager.Taiwu.GetTaiwuCharId(), out var list))
			{
				return PossessionResult.BodyCharNotKidnappedByTaiwu;
			}
			bool flag = false;
			foreach (KidnappedCharacter item in list.GetCollection())
			{
				if (item.CharId == bodyCharId)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return PossessionResult.BodyCharNotKidnappedByTaiwu;
			}
		}
		return PossessionResult.Success;
	}

	[DomainMethod]
	public void SetTemporaryPossessionCharacterAvatar(DataContext context, AvatarData avatar, AvatarExtraData avatarExtraData)
	{
		if (_temporaryPossessionCharId >= 0 && DomainManager.Character.TryGetElement_Objects(_temporaryPossessionCharId, out var element))
		{
			DomainManager.Extra.TryAddCharacterAvatarExtraData(context, _temporaryPossessionCharId, avatarExtraData);
			element.SetAvatar(avatar, context);
		}
	}

	public void RemoveTemporaryPossessionCharacter(DataContext context)
	{
		if (_temporaryPossessionCharId >= 0 && DomainManager.Character.TryGetElement_Objects(_temporaryPossessionCharId, out var element))
		{
			DomainManager.Extra.TryRemoveCharacterAvatarExtraData(context, _temporaryPossessionCharId);
			DomainManager.Character.RemoveTemporaryIntelligentCharacter(context, element);
			_temporaryPossessionCharId = -1;
		}
	}

	[DomainMethod]
	public SamsaraPlatformRecordCollection GetSamsaraPlatformRecord()
	{
		return DomainManager.Extra.GetSamsaraPlatformRecordCollection();
	}

	[DomainMethod]
	public ShopEventCollection GetOrCreateShopEventCollection(BuildingBlockKey buildingBlockKey)
	{
		if (_shopEventCollections == null)
		{
			_shopEventCollections = new Dictionary<BuildingBlockKey, ShopEventCollection>();
		}
		if (_shopEventCollections.TryGetValue(buildingBlockKey, out var value))
		{
			return value;
		}
		value = new ShopEventCollection();
		_shopEventCollections.Add(buildingBlockKey, value);
		return value;
	}

	[DomainMethod]
	public bool HasShopManagerLeader(BuildingBlockKey blockKey)
	{
		if (!_buildingBlocks.TryGetValue(blockKey, out var value) || value.TemplateId < 0)
		{
			return false;
		}
		return GetShopManagerLeader(blockKey) != null || !value.ConfigData.NeedLeader;
	}

	public GameData.Domains.Character.Character GetShopManagerLeader(BuildingBlockKey blockKey)
	{
		if (!_shopManagerDict.TryGetValue(blockKey, out var value) || value.GetCount() == 0)
		{
			return null;
		}
		List<int> collection = value.GetCollection();
		int objectId = collection[0];
		GameData.Domains.Character.Character element;
		return DomainManager.Character.TryGetElement_Objects(objectId, out element) ? element : null;
	}

	[DomainMethod]
	public void SetShopManager(DataContext context, BuildingBlockKey blockKey, sbyte index, int charId)
	{
		bool allowChild = index != 0;
		if (_shopManagerDict.ContainsKey(blockKey))
		{
			int num = _shopManagerDict[blockKey].GetCollection()[index];
			if (num == -1 && charId == -1)
			{
				return;
			}
			if (num == charId)
			{
				throw new Exception($"Same shop manager already exist, id: {charId}");
			}
			if (num >= 0)
			{
				DomainManager.Taiwu.RemoveVillagerWork(context, num);
			}
		}
		if (charId >= 0)
		{
			VillagerWorkData villagerWorkData = new VillagerWorkData(charId, 1, blockKey.AreaId, blockKey.BlockId);
			villagerWorkData.BuildingBlockIndex = blockKey.BuildingBlockIndex;
			villagerWorkData.WorkerIndex = index;
			DomainManager.Taiwu.SetVillagerWork(context, charId, villagerWorkData, allowChild);
		}
		if (DomainManager.Building.TryGetElement_BuildingBlocks(blockKey, out var value))
		{
			BuildingBlockItem config = BuildingBlock.Instance[value.TemplateId];
			if (SharedMethods.HasEffect(config))
			{
				_needUpdateEffects = true;
			}
		}
	}

	[DomainMethod]
	public void SetCollectBuildingResourceType(DataContext context, BuildingBlockKey blockKey, sbyte resourceType)
	{
		if (_CollectBuildingResourceType.ContainsKey(blockKey))
		{
			SetElement_CollectBuildingResourceType(blockKey, resourceType, context);
		}
		else
		{
			AddElement_CollectBuildingResourceType(blockKey, resourceType, context);
		}
	}

	[DomainMethod]
	public void ClearBuildingBlockEarningsData(DataContext context, BuildingBlockKey key, bool isPawnShop)
	{
		if (TryGetElement_CollectBuildingEarningsData(key, out var value))
		{
			foreach (ItemKey shopSoldItem in value.ShopSoldItemList)
			{
				if (shopSoldItem.IsValid())
				{
					DomainManager.Item.GetBaseItem(shopSoldItem).ResetOwner();
					DomainManager.Taiwu.WarehouseAdd(context, shopSoldItem, 1);
				}
			}
			foreach (ItemKey fixBookInfo in value.FixBookInfoList)
			{
				if (fixBookInfo.IsValid())
				{
					DomainManager.Item.GetBaseItem(fixBookInfo).ResetOwner();
					DomainManager.Taiwu.WarehouseAdd(context, fixBookInfo, 1);
				}
			}
			if (!isPawnShop)
			{
				foreach (ItemKey collectionItem in value.CollectionItemList)
				{
					if (collectionItem.IsValid())
					{
						DomainManager.Item.GetBaseItem(collectionItem).ResetOwner();
						ItemSourceType itemSourceType = ApplyBuildingItemOutputSetting(key, collectionItem);
						DomainManager.Taiwu.AddItem(context, collectionItem, 1, itemSourceType);
					}
				}
			}
			foreach (IntPair shopSoldItemEarn in value.ShopSoldItemEarnList)
			{
				if (shopSoldItemEarn.First == 6)
				{
					DomainManager.Taiwu.GetTaiwu().ChangeResource(context, 6, shopSoldItemEarn.Second);
					ApplyBuildingResourceOutputSetting(context, key, (sbyte)shopSoldItemEarn.First, shopSoldItemEarn.Second);
				}
				if (shopSoldItemEarn.First == 7)
				{
					DomainManager.Taiwu.GetTaiwu().ChangeResource(context, 7, shopSoldItemEarn.Second);
				}
			}
			foreach (IntPair collectionResource in value.CollectionResourceList)
			{
				if (collectionResource.First == 6)
				{
					DomainManager.Taiwu.GetTaiwu().ChangeResource(context, 6, collectionResource.Second);
					ApplyBuildingResourceOutputSetting(context, key, (sbyte)collectionResource.First, collectionResource.Second);
				}
				if (collectionResource.First == 7)
				{
					DomainManager.Taiwu.GetTaiwu().ChangeResource(context, 7, collectionResource.Second);
				}
			}
		}
		if (_shopManagerDict.ContainsKey(key))
		{
			RemoveElement_ShopManagerDict(key, context);
		}
		if (_collectBuildingEarningsData.ContainsKey(key))
		{
			RemoveElement_CollectBuildingEarningsData(key, context);
		}
		RemoveBuildingResourceOutputSetting(context, key.BuildingBlockIndex);
	}

	private BuildingOptionAutoGiveMemberPreset GetArrangementSetting(BuildingBlockKey blockKey)
	{
		DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)blockKey, out var value);
		return value?.ArrangementSetting ?? new BuildingOptionAutoGiveMemberPreset();
	}

	private void GetBaseArrangementVillagerList(BuildingBlockKey blockKey, out List<int> roleVillagersForWork, out List<int> noRoleVillagersForWork)
	{
		BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
		List<int> villagersForWork = DomainManager.Taiwu.GetVillagersForWork(includeUnlockedWorkingVillagers: true);
		List<int> charIds = new List<int>();
		if (element_BuildingBlocks.ConfigData.VillagerRoleTemplateIds != null)
		{
			short[] villagerRoleTemplateIds = element_BuildingBlocks.ConfigData.VillagerRoleTemplateIds;
			foreach (short villagerRoleTemplateId in villagerRoleTemplateIds)
			{
				DomainManager.Extra.GetVillagerRoleCharactersByTemplateId(villagerRoleTemplateId, ref charIds);
			}
		}
		roleVillagersForWork = new List<int>();
		noRoleVillagersForWork = new List<int>();
		foreach (int item in villagersForWork)
		{
			if (charIds.Contains(item))
			{
				roleVillagersForWork.Add(item);
			}
			else
			{
				noRoleVillagersForWork.Add(item);
			}
		}
	}

	private void SortByAttainment(BuildingBlockItem buildingConfig, ref List<int> ids)
	{
		ids.Sort(delegate(int idLeft, int idRight)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(idLeft);
			GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(idRight);
			if (buildingConfig.RequireLifeSkillType >= 0)
			{
				return element_Objects2.GetLifeSkillAttainment(buildingConfig.RequireLifeSkillType) - element_Objects.GetLifeSkillAttainment(buildingConfig.RequireLifeSkillType);
			}
			return (buildingConfig.RequireCombatSkillType >= 0) ? (element_Objects2.GetCombatSkillAttainment(buildingConfig.RequireCombatSkillType) - element_Objects.GetCombatSkillAttainment(buildingConfig.RequireCombatSkillType)) : 0;
		});
	}

	private void SortByQualifications(BuildingBlockItem buildingConfig, ref List<int> ids, bool descending = true)
	{
		ids.Sort(delegate(int idLeft, int idRight)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(idLeft);
			GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(idRight);
			int num = 0;
			if (buildingConfig.RequireLifeSkillType >= 0)
			{
				num = element_Objects2.GetLifeSkillQualification(buildingConfig.RequireLifeSkillType) - element_Objects.GetLifeSkillQualification(buildingConfig.RequireLifeSkillType);
			}
			else if (buildingConfig.RequireCombatSkillType >= 0)
			{
				num = element_Objects2.GetCombatSkillQualification(buildingConfig.RequireCombatSkillType) - element_Objects.GetCombatSkillQualification(buildingConfig.RequireCombatSkillType);
			}
			return descending ? num : (-num);
		});
	}

	private void SortByReadBookTotalValue(BuildingBlockItem buildingConfig, ref List<int> ids, bool descending = true)
	{
		ids.Sort(delegate(int idLeft, int idRight)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(idLeft);
			GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(idRight);
			int num = 0;
			if (buildingConfig.RequireLifeSkillType >= 0)
			{
				num = element_Objects2.GetLearnedLifeSkillTotalValue(buildingConfig.RequireLifeSkillType) - element_Objects.GetLearnedLifeSkillTotalValue(buildingConfig.RequireLifeSkillType);
			}
			else if (buildingConfig.RequireCombatSkillType >= 0)
			{
				num = element_Objects2.GetLearnedCombatSkillTotalValue(buildingConfig.RequireCombatSkillType) - element_Objects.GetLearnedCombatSkillTotalValue(buildingConfig.RequireCombatSkillType);
			}
			return descending ? num : (-num);
		});
	}

	private void GetLeaderArrangementVillagerList(BuildingBlockKey blockKey, out List<int> roleVillagersForWork, out List<int> noRoleVillagersForWork)
	{
		BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
		BuildingOptionAutoGiveMemberPreset arrangementSetting = GetArrangementSetting(blockKey);
		if (!arrangementSetting.GetIsInfluenceLeader())
		{
			roleVillagersForWork = null;
			noRoleVillagersForWork = null;
			return;
		}
		GetBaseArrangementVillagerList(blockKey, out roleVillagersForWork, out noRoleVillagersForWork);
		switch ((BuildingOptionAutoGiveMemberPreset.PickRule)arrangementSetting.PickRuleForLeader)
		{
		case BuildingOptionAutoGiveMemberPreset.PickRule.ManageFirst:
			SortByAttainment(element_BuildingBlocks.ConfigData, ref roleVillagersForWork);
			SortByAttainment(element_BuildingBlocks.ConfigData, ref noRoleVillagersForWork);
			break;
		case BuildingOptionAutoGiveMemberPreset.PickRule.QualificationFirst:
			SortByQualifications(element_BuildingBlocks.ConfigData, ref roleVillagersForWork);
			SortByQualifications(element_BuildingBlocks.ConfigData, ref noRoleVillagersForWork);
			break;
		case BuildingOptionAutoGiveMemberPreset.PickRule.ReadingFirst:
			SortByReadBookTotalValue(element_BuildingBlocks.ConfigData, ref roleVillagersForWork);
			SortByReadBookTotalValue(element_BuildingBlocks.ConfigData, ref noRoleVillagersForWork);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case BuildingOptionAutoGiveMemberPreset.PickRule.QualificationHighest:
			break;
		}
		BuildingOptionAutoGiveMemberPreset.RoleRule roleRuleForLeader = (BuildingOptionAutoGiveMemberPreset.RoleRule)arrangementSetting.RoleRuleForLeader;
		if (roleRuleForLeader.HasFlag(BuildingOptionAutoGiveMemberPreset.RoleRule.OnlyRole))
		{
			noRoleVillagersForWork.Clear();
		}
	}

	private void GetMemberArrangementVillagerList(BuildingBlockKey blockKey, out List<int> roleVillagersForWork, out List<int> noRoleVillagersForWork)
	{
		BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
		BuildingOptionAutoGiveMemberPreset arrangementSetting = GetArrangementSetting(blockKey);
		if (!arrangementSetting.GetIsInfluenceMember())
		{
			roleVillagersForWork = null;
			noRoleVillagersForWork = null;
			return;
		}
		GetBaseArrangementVillagerList(blockKey, out roleVillagersForWork, out noRoleVillagersForWork);
		BuildingOptionAutoGiveMemberPreset.RoleRule roleRuleForMember = (BuildingOptionAutoGiveMemberPreset.RoleRule)arrangementSetting.RoleRuleForMember;
		if (roleRuleForMember.HasFlag(BuildingOptionAutoGiveMemberPreset.RoleRule.AllowChild))
		{
			List<int> allChildAvailableForWork = DomainManager.Taiwu.GetAllChildAvailableForWork(actuallyNotOccupiedOnly: true);
			noRoleVillagersForWork.AddRange(allChildAvailableForWork);
		}
		if (roleRuleForMember.HasFlag(BuildingOptionAutoGiveMemberPreset.RoleRule.NotAllowRole))
		{
			roleVillagersForWork.Clear();
			noRoleVillagersForWork.RemoveAll((int id) => DomainManager.Extra.GetVillagerRole(id) != null);
		}
		if (!roleRuleForMember.HasFlag(BuildingOptionAutoGiveMemberPreset.RoleRule.AllowNoPotential))
		{
			roleVillagersForWork.RemoveAll((int id) => DomainManager.Taiwu.GetTaiwuVillagerLeftPotentialCount(id) == 0);
			noRoleVillagersForWork.RemoveAll((int id) => DomainManager.Taiwu.GetTaiwuVillagerLeftPotentialCount(id) == 0);
		}
		if (!roleRuleForMember.HasFlag(BuildingOptionAutoGiveMemberPreset.RoleRule.AllowNoReadableBook))
		{
			roleVillagersForWork.RemoveAll(Match);
			noRoleVillagersForWork.RemoveAll(Match);
		}
		switch ((BuildingOptionAutoGiveMemberPreset.PickRule)arrangementSetting.PickRuleForMember)
		{
		case BuildingOptionAutoGiveMemberPreset.PickRule.ManageFirst:
			SortByAttainment(element_BuildingBlocks.ConfigData, ref roleVillagersForWork);
			SortByAttainment(element_BuildingBlocks.ConfigData, ref noRoleVillagersForWork);
			break;
		case BuildingOptionAutoGiveMemberPreset.PickRule.QualificationFirst:
			SortByQualifications(element_BuildingBlocks.ConfigData, ref roleVillagersForWork, descending: false);
			SortByQualifications(element_BuildingBlocks.ConfigData, ref noRoleVillagersForWork, descending: false);
			break;
		case BuildingOptionAutoGiveMemberPreset.PickRule.ReadingFirst:
			SortByReadBookTotalValue(element_BuildingBlocks.ConfigData, ref roleVillagersForWork, descending: false);
			SortByReadBookTotalValue(element_BuildingBlocks.ConfigData, ref noRoleVillagersForWork, descending: false);
			break;
		case BuildingOptionAutoGiveMemberPreset.PickRule.QualificationHighest:
			SortByQualifications(element_BuildingBlocks.ConfigData, ref roleVillagersForWork);
			SortByQualifications(element_BuildingBlocks.ConfigData, ref noRoleVillagersForWork);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		bool Match(int id)
		{
			ShopBuildingTeachBookData shopBuildingTeachBookData = GetShopBuildingTeachBookData(blockKey, id);
			sbyte teachBookResult = shopBuildingTeachBookData.TeachBookResult;
			if ((uint)(teachBookResult - 3) <= 1u)
			{
				return true;
			}
			return false;
		}
	}

	[DomainMethod]
	public List<int> QuickArrangeShopManager(DataContext context, BuildingBlockKey blockKey, bool onlyCheck = false)
	{
		BuildingOptionAutoGiveMemberPreset presetData = GetArrangementSetting(blockKey);
		List<int> charIds = new List<int>();
		for (sbyte b = 0; b < 7; b++)
		{
			charIds.Add(-1);
			if ((presetData.GetIsInfluenceLeader() || b != 0) && (presetData.GetIsInfluenceMember() || b == 0) && !onlyCheck)
			{
				SetShopManager(context, blockKey, b, -1);
			}
		}
		if (presetData.GetIsInfluenceLeader())
		{
			GetLeaderArrangementVillagerList(blockKey, out var roleVillagersForWork, out var noRoleVillagersForWork);
			bool unlock = !presetData.LockCharForLeader;
			FillLeaders(ref roleVillagersForWork, ref charIds, unlock);
			FillLeaders(ref noRoleVillagersForWork, ref charIds, unlock);
		}
		if (presetData.GetIsInfluenceMember())
		{
			GetMemberArrangementVillagerList(blockKey, out var roleVillagersForWork2, out var noRoleVillagersForWork2);
			int leaderIndex = 0;
			roleVillagersForWork2.RemoveAll((int id) => charIds[leaderIndex] == id);
			noRoleVillagersForWork2.RemoveAll((int id) => charIds[leaderIndex] == id);
			bool unlock2 = !presetData.LockCharForMember;
			FillMembers(ref roleVillagersForWork2, ref charIds, unlock2);
			FillMembers(ref noRoleVillagersForWork2, ref charIds, unlock2);
		}
		return charIds;
		void FillLeaders(ref List<int> availableList, ref List<int> targetList, bool add)
		{
			if (availableList.Count > 0 && targetList[0] < 0)
			{
				int index = 0;
				int num = availableList[index];
				availableList.RemoveAt(index);
				targetList[0] = num;
				if (!onlyCheck)
				{
					SetShopManager(context, blockKey, 0, num);
					SetUnlockedWorkingVillagers(context, num, add);
				}
			}
		}
		void FillMembers(ref List<int> availableList, ref List<int> targetList, bool add)
		{
			if (availableList.Count > 0)
			{
				int num = Math.Min(availableList.Count, presetData.Amount);
				int index = 0;
				for (sbyte b2 = 0; b2 < num; b2++)
				{
					sbyte index2 = (sbyte)(b2 + 1);
					if (targetList[index2] < 0)
					{
						int num2 = availableList[index];
						availableList.RemoveAt(index);
						targetList[index2] = num2;
						if (!onlyCheck)
						{
							SetShopManager(context, blockKey, index2, num2);
							SetUnlockedWorkingVillagers(context, num2, add);
						}
					}
				}
			}
		}
	}

	[DomainMethod]
	public bool CanQuickArrangeShopManager(BuildingBlockKey blockKey)
	{
		_shopManagerDict.TryGetValue(blockKey, out var value);
		List<int> collection = value.GetCollection();
		List<int> list = QuickArrangeShopManager(null, blockKey, onlyCheck: true);
		if (list == null)
		{
			return false;
		}
		foreach (int item in list)
		{
			if (item >= 0 && item != collection.GetOrDefault(-1))
			{
				return true;
			}
		}
		return false;
	}

	[DomainMethod]
	public List<int> QuickArrangeBuildOperator(DataContext context, short buildingTemplateId, BuildingBlockKey blockKey, sbyte operationType)
	{
		int num = 0;
		BuildingBlockItem buildingBlockItem = null;
		if (operationType == 0)
		{
			buildingBlockItem = BuildingBlock.Instance[buildingTemplateId];
			num = buildingBlockItem.OperationTotalProgress[operationType];
		}
		else
		{
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
			buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
			num = buildingBlockItem.OperationTotalProgress[operationType] - element_BuildingBlocks.OperationProgress;
		}
		List<int> result = new List<int>();
		for (sbyte b = 0; b < 3; b++)
		{
			result.Add(-1);
		}
		List<int> villagersForWork = DomainManager.Taiwu.GetVillagersForWork(includeUnlockedWorkingVillagers: true);
		villagersForWork.Sort(delegate(int leftId, int rightId)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(leftId);
			GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(rightId);
			return element_Objects.GetPersonalities().GetSum() - element_Objects2.GetPersonalities().GetSum();
		});
		FindMinSumCombination(villagersForWork, num, ref result);
		return result;
		List<int> FindMinSumCombination(List<int> list, int threshold, ref List<int> reference)
		{
			if (list == null || list.Count == 0)
			{
				return reference;
			}
			int count = list.Count;
			int num2 = int.MaxValue;
			for (int i = 0; i < count; i++)
			{
				int num3 = list[i];
				reference[0] = num3;
				int num4 = GetCharacterWorkValueSum(num3);
				if (num4 > threshold && num4 < num2)
				{
					num2 = num4;
					reference[0] = num3;
				}
			}
			for (int j = 0; j < count - 1; j++)
			{
				if (GetCharacterWorkValueSum(list[j]) + GetCharacterWorkValueSum(list[j + 1]) <= num2)
				{
					reference[0] = list[j];
					reference[1] = list[j + 1];
					for (int k = j + 1; k < count; k++)
					{
						int num5 = GetCharacterWorkValueSum(list[j]) + GetCharacterWorkValueSum(list[k]);
						if (num5 > num2)
						{
							break;
						}
						if (num5 > threshold)
						{
							num2 = num5;
							reference[0] = list[j];
							reference[1] = list[k];
						}
					}
				}
			}
			for (int l = 0; l < count - 2 && GetCharacterWorkValueSum(list[l]) + GetCharacterWorkValueSum(list[l + 1]) + GetCharacterWorkValueSum(list[l + 2]) <= num2; l++)
			{
				reference[0] = list[l];
				reference[1] = list[l + 1];
				reference[2] = list[l + 2];
				int num6 = l + 1;
				int num7 = count - 1;
				while (num6 < num7)
				{
					int num8 = GetCharacterWorkValueSum(list[l]) + GetCharacterWorkValueSum(list[num6]) + GetCharacterWorkValueSum(list[num7]);
					if (num8 <= threshold)
					{
						num6++;
					}
					else
					{
						if (num8 < num2)
						{
							num2 = num8;
							reference[0] = list[l];
							reference[1] = list[num6];
							reference[2] = list[num7];
						}
						num7--;
					}
				}
			}
			return reference;
		}
		int GetCharacterWorkValueSum(int charId)
		{
			if (charId < 0)
			{
				return 0;
			}
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
			return element_Objects.GetPersonalities().GetSum() + BaseWorkContribution;
		}
	}

	[DomainMethod]
	public int GetOperationLeftTime(DataContext context, short buildingTemplateId, BuildingBlockKey blockKey, sbyte operationType, List<int> operatorList)
	{
		int num = 0;
		BuildingBlockData value;
		int num2 = (DomainManager.Building.TryGetElement_BuildingBlocks(blockKey, out value) ? value.OperationProgress : 0);
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[buildingTemplateId];
		num = buildingBlockItem.OperationTotalProgress[operationType] - num2;
		int num3 = 0;
		foreach (int @operator in operatorList)
		{
			if (@operator >= 0)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(@operator);
				int sum = element_Objects.GetPersonalities().GetSum();
				num3 += BaseWorkContribution + sum;
			}
		}
		return (int)Math.Ceiling((float)num / (float)num3);
	}

	[DomainMethod]
	public int GetBuildingOperationLeftTime(DataContext context, BuildingBlockKey blockKey, sbyte operationType)
	{
		if (!DomainManager.Building.TryGetElement_BuildingBlocks(blockKey, out var value))
		{
			return -1;
		}
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[value.TemplateId];
		int num = buildingBlockItem.OperationTotalProgress[operationType] - value.OperationProgress;
		if (!DomainManager.Building.TryGetElement_BuildingOperatorDict(blockKey, out var value2))
		{
			return -1;
		}
		int num2 = 0;
		foreach (int item in value2.GetCollection())
		{
			if (item >= 0)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				int sum = element_Objects.GetPersonalities().GetSum();
				num2 += BaseWorkContribution + sum;
			}
		}
		if (num2 == 0)
		{
			return -1;
		}
		return (int)Math.Ceiling((float)num / (float)num2);
	}

	public int GetOperationSumValue(List<int> operatorList)
	{
		int num = 0;
		foreach (int @operator in operatorList)
		{
			if (@operator >= 0 && DomainManager.Taiwu.CanWork(@operator))
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(@operator);
				int sum = element_Objects.GetPersonalities().GetSum();
				num += BaseWorkContribution + sum;
			}
		}
		return num;
	}

	[DomainMethod]
	public bool ShopBuildingCanTeach(BuildingBlockKey blockKey)
	{
		if (TryGetElement_ShopManagerDict(blockKey, out var value))
		{
			BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(blockKey);
			int num = value[0];
			if (num == -1)
			{
				return false;
			}
			VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(num);
			if (villagerRole == null)
			{
				return false;
			}
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
			return buildingBlockItem.VillagerRoleTemplateIds.Contains(villagerRole.RoleTemplateId);
		}
		return false;
	}

	[DomainMethod]
	public ShopBuildingTeachBookData GetShopBuildingTeachBookData(BuildingBlockKey blockKey, int memberId)
	{
		ShopBuildingTeachBookData shopBuildingTeachBookData = ShopBuildingTeachBookData.CreateDefault();
		if (!HasShopManagerLeader(blockKey))
		{
			shopBuildingTeachBookData.TeachBookResult = 1;
			return shopBuildingTeachBookData;
		}
		if (!ShopBuildingCanTeach(blockKey))
		{
			shopBuildingTeachBookData.TeachBookResult = 2;
			return shopBuildingTeachBookData;
		}
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[GetElement_BuildingBlocks(blockKey).TemplateId];
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(memberId);
		sbyte b = element_Objects.GetPersonalities()[buildingBlockItem.RequirePersonalityType];
		short num = ((buildingBlockItem.RequireLifeSkillType >= 0) ? element_Objects.GetLifeSkillAttainment(buildingBlockItem.RequireLifeSkillType) : element_Objects.GetCombatSkillAttainment(buildingBlockItem.RequireCombatSkillType));
		int point = num * (100 + b) / 100;
		GameData.Domains.Character.Character shopManagerLeader = GetShopManagerLeader(blockKey);
		if (buildingBlockItem.RequireLifeSkillType >= 0)
		{
			bool hasMemberUnread;
			List<(short, byte)> list = ShopBuildingTeachLifeSkillBook(shopManagerLeader, element_Objects, point, buildingBlockItem.TemplateId, out hasMemberUnread);
			shopBuildingTeachBookData.TeachBookResult = (sbyte)((list.Count <= 0) ? (hasMemberUnread ? 4 : 3) : 0);
			if (list.Count > 0)
			{
				foreach (var item6 in list)
				{
					short item = item6.Item1;
					byte item2 = item6.Item2;
					Config.LifeSkillItem lifeSkillItem = LifeSkill.Instance[item];
					shopBuildingTeachBookData.TeachBookInfo.Add((lifeSkillItem.SkillBookId, item2, -1));
				}
			}
		}
		else
		{
			bool hasMemberUnread2;
			List<(short, byte, sbyte)> list2 = ShopBuildingTeachCombatSkillBook(shopManagerLeader, element_Objects, point, buildingBlockItem.TemplateId, out hasMemberUnread2);
			shopBuildingTeachBookData.TeachBookResult = (sbyte)((list2.Count <= 0) ? (hasMemberUnread2 ? 4 : 3) : 0);
			if (list2.Count > 0)
			{
				foreach (var item7 in list2)
				{
					short item3 = item7.Item1;
					byte item4 = item7.Item2;
					sbyte item5 = item7.Item3;
					CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[item3];
					byte pageId = CombatSkillStateHelper.GetPageId(item4);
					shopBuildingTeachBookData.TeachBookInfo.Add((combatSkillItem.BookId, pageId, item5));
				}
			}
		}
		return shopBuildingTeachBookData;
	}

	[DomainMethod]
	[Obsolete("Use CalcResourceOutputCount() replace", false)]
	public int CalcResourceOutput(DataContext context, BuildingBlockKey key)
	{
		return 0;
	}

	public void CalcResourceBlockProductivityList(BuildingBlockKey key, List<(BuildingBlockKey, int productivity)> productivityList)
	{
		productivityList.Clear();
		List<(BuildingBlockKey key, int level)> distanceOneBlocks = new List<(BuildingBlockKey, int)>();
		BuildingBlockDependencies(key, delegate(BuildingBlockData dependency, int distance, BuildingBlockKey blockKey)
		{
			switch (distance)
			{
			case 1:
				distanceOneBlocks.Add((blockKey, BuildingBlockLevel(blockKey)));
				break;
			case 2:
				productivityList.Add((blockKey, GlobalConfig.Instance.CollectResourceBuildingProductivityDistanceMore));
				break;
			}
		});
		distanceOneBlocks.Sort(((BuildingBlockKey key, int level) a, (BuildingBlockKey key, int level) b) => b.level.CompareTo(a.level));
		int[] collectResourceBuildingProductivityDistanceOne = GlobalConfig.Instance.CollectResourceBuildingProductivityDistanceOne;
		for (int num = distanceOneBlocks.Count - 1; num >= 0; num--)
		{
			(BuildingBlockKey, int) tuple = distanceOneBlocks[num];
			int item = collectResourceBuildingProductivityDistanceOne[num];
			productivityList.Add((tuple.Item1, item));
		}
		productivityList.Reverse();
	}

	public int CalcResourceOutputCountBySingleSpecifiedDependency(BuildingBlockKey key, sbyte resourceType, BuildingBlockData dependency, int productivity, out BuildingProduceDependencyData dependencyData)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		dependencyData = BuildingProduceDependencyData.Invalid;
		if (TryGetElement_BuildingBlocks(key, out var _))
		{
			BuildingBlockKey blockKey = new BuildingBlockKey(key.AreaId, key.BlockId, dependency.BlockIndex);
			dependencyData.Level = BuildingBlockLevel(blockKey);
			dependencyData.TemplateId = dependency.TemplateId;
			dependencyData.BlockBaseYieldFactor = BuildingBaseYield(dependency).Get(Math.Min(resourceType, 5));
			dependencyData.ResourceYieldLevelFactor = BuildingResourceYieldLevel(blockKey, dependency.TemplateId);
			dependencyData.ProductivityFactor = productivity;
			dependencyData.TotalAttainmentFactor = BuildingTotalAttainment(key, resourceType, out var hasManager);
			dependencyData.GainResourcePercentFactor = DomainManager.World.GetGainResourcePercent(1);
			dependencyData.ResourceSingleOutputValuation = dependencyData.ResourceBuildingOutput;
			CValuePercentBonus val = CValuePercentBonus.op_Implicit(DomainManager.Building.GetBuildingBlockEffect(key.GetLocation(), EBuildingScaleEffect.BuildingMaterialResourceIncomeBonus));
			ref int resourceSingleOutputValuation = ref dependencyData.ResourceSingleOutputValuation;
			resourceSingleOutputValuation *= val;
			if (!hasManager)
			{
				return 0;
			}
			return dependencyData.ResourceSingleOutputValuation;
		}
		return 0;
	}

	[DomainMethod]
	public int CalcResourceOutputCount(BuildingBlockKey key, sbyte resourceType)
	{
		if (TryGetElement_BuildingBlocks(key, out var _))
		{
			int num = 0;
			List<(BuildingBlockKey, int)> list = new List<(BuildingBlockKey, int)>();
			CalcResourceBlockProductivityList(key, list);
			foreach (var item in list)
			{
				num += CalcResourceOutputCountBySingleSpecifiedDependency(key, resourceType, GetBuildingBlockData(item.Item1), item.Item2, out var _);
			}
			return num;
		}
		return 0;
	}

	private int CalcResourceOutputCount(BuildingBlockKey key, sbyte resourceType, Dictionary<BuildingBlockKey, BuildingProduceDependencyData> dependencyDataDict)
	{
		if (TryGetElement_BuildingBlocks(key, out var _))
		{
			int num = 0;
			List<(BuildingBlockKey, int)> list = new List<(BuildingBlockKey, int)>();
			CalcResourceBlockProductivityList(key, list);
			foreach (var item in list)
			{
				num += CalcResourceOutputCountBySingleSpecifiedDependency(key, resourceType, GetBuildingBlockData(item.Item1), item.Item2, out var dependencyData);
				dependencyDataDict.Add(item.Item1, dependencyData);
			}
			return num;
		}
		return 0;
	}

	[DomainMethod]
	public BuildingEarningsData GetBuildingEarningData(BuildingBlockKey blockKey)
	{
		if (TryGetElement_CollectBuildingEarningsData(blockKey, out var value))
		{
			return value;
		}
		return null;
	}

	[DomainMethod]
	public List<int> GetBuildingOperatesData(BuildingBlockKey blockKey)
	{
		List<int> list = new List<int>();
		Dictionary<int, VillagerWorkData> villagerWorkDict = DomainManager.Taiwu.GetVillagerWorkDict();
		foreach (var (item, villagerWorkData2) in villagerWorkDict)
		{
			if (villagerWorkData2.BuildingBlockIndex == blockKey.BuildingBlockIndex && villagerWorkData2.WorkType == 0)
			{
				list.Add(item);
			}
		}
		return list;
	}

	[DomainMethod]
	public int GetBuildingBuildPeopleAttainments(BuildingBlockKey blockKey)
	{
		List<int> buildingOperatesData = GetBuildingOperatesData(blockKey);
		int num = 0;
		for (int i = 0; i < buildingOperatesData.Count; i++)
		{
			if (TryGetElement_BuildingBlocks(blockKey, out var value))
			{
				num += BaseWorkContribution;
				num += DomainManager.Character.GetElement_Objects(buildingOperatesData[i]).GetLifeSkillAttainment(BuildingBlock.Instance[value.TemplateId].RequireLifeSkillType);
			}
		}
		return num;
	}

	[DomainMethod]
	public void SetBuildingResourceOutputSetting(DataContext context, int blockIndex, BuildingResourceOutputSetting setting)
	{
		DomainManager.Extra.SetBuildingResourceOutputSetting(context, blockIndex, setting);
	}

	[DomainMethod]
	public BuildingResourceOutputSetting GetBuildingResourceOutputSetting(int blockIndex)
	{
		return DomainManager.Extra.GetBuildingResourceOutputSetting(blockIndex);
	}

	public void RemoveBuildingResourceOutputSetting(DataContext context, int blockIndex)
	{
		DomainManager.Extra.RemoveBuildingResourceOutputSetting(context, blockIndex);
	}

	public void SetCollectBuildingEarningsData(DataContext context, BuildingBlockKey blockKey, BuildingEarningsData data)
	{
		if (TryGetElement_CollectBuildingEarningsData(blockKey, out var _))
		{
			SetElement_CollectBuildingEarningsData(blockKey, data, context);
		}
		else
		{
			AddElement_CollectBuildingEarningsData(blockKey, data, context);
		}
	}

	[DomainMethod]
	public BuildingBlockKey AcceptBuildingBlockCollectEarning(DataContext context, BuildingBlockKey key, int earningDataIndex, bool isPutInInventory, bool isSetData = true, bool isCostMoney = false)
	{
		if (TryGetElement_CollectBuildingEarningsData(key, out var value) && earningDataIndex >= 0)
		{
			if (earningDataIndex < value.CollectionItemList.Count)
			{
				ItemKey itemKey = value.CollectionItemList[earningDataIndex];
				itemKey.ModificationState = 0;
				value.CollectionItemList.RemoveAt(earningDataIndex);
				BuildingBlockData buildingBlockData = GetBuildingBlockData(key);
				DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Building, buildingBlockData.TemplateId);
				if (isCostMoney)
				{
					ConsumeResource(context, 6, ItemTemplateHelper.GetBaseValue(itemKey.ItemType, itemKey.TemplateId));
				}
				ItemSourceType itemSourceType = ApplyBuildingItemOutputSetting(key, itemKey);
				DomainManager.Taiwu.AddItem(context, itemKey, 1, itemSourceType);
				DomainManager.Building.AddBuildingGainLegacy(context, key);
				BuildingChangeProfessionSeniority(context, itemKey);
			}
			if (earningDataIndex < value.CollectionResourceList.Count)
			{
				IntPair intPair = value.CollectionResourceList[earningDataIndex];
				value.CollectionResourceList.RemoveAt(earningDataIndex - value.CollectionItemList.Count);
				DomainManager.Character.GetElement_Objects(DomainManager.Taiwu.GetTaiwuCharId()).ChangeResource(context, (sbyte)intPair.First, intPair.Second);
				ApplyBuildingResourceOutputSetting(context, key, (sbyte)intPair.First, intPair.Second);
				DomainManager.Building.AddBuildingGainLegacy(context, key);
			}
			if (isSetData)
			{
				SetElement_CollectBuildingEarningsData(key, value, context);
			}
		}
		return key;
	}

	private void ApplyBuildingResourceOutputSetting(DataContext context, BuildingBlockKey key, sbyte resourceType, int amount)
	{
		if (!key.IsInvalid)
		{
			BuildingResourceOutputSetting buildingResourceOutputSetting = GetBuildingResourceOutputSetting(key.BuildingBlockIndex);
			if (buildingResourceOutputSetting.ResourceStorage.TryGetValue(resourceType, out var value) && value != -1)
			{
				bool isMoney = resourceType == 6;
				ItemSourceType itemSourceTypeByStorageType = BuildingResourceOutputSetting.GetItemSourceTypeByStorageType((TaiwuVillageStorageType)value, isMaterialItem: false, isMoney);
				DomainManager.Taiwu.TransferResource(context, ItemSourceType.Inventory, itemSourceTypeByStorageType, resourceType, amount);
			}
		}
	}

	public ItemSourceType ApplyBuildingItemOutputSetting(BuildingBlockKey key, ItemKey itemKey)
	{
		if (key.IsInvalid)
		{
			return ItemSourceType.Warehouse;
		}
		BuildingResourceOutputSetting buildingResourceOutputSetting = GetBuildingResourceOutputSetting(key.BuildingBlockIndex);
		sbyte resourceType = ItemTemplateHelper.GetResourceType(itemKey.ItemType, itemKey.TemplateId);
		if (!buildingResourceOutputSetting.ItemStorage.TryGetValue(resourceType, out var value))
		{
			return ItemSourceType.Warehouse;
		}
		bool isMaterialItem = itemKey.ItemType == 5;
		bool isMoney = resourceType == 6;
		ItemSourceType itemSourceTypeByStorageType = BuildingResourceOutputSetting.GetItemSourceTypeByStorageType((TaiwuVillageStorageType)value, isMaterialItem, isMoney);
		if (itemSourceTypeByStorageType == ItemSourceType.Inventory)
		{
			return ItemSourceType.Warehouse;
		}
		return itemSourceTypeByStorageType;
	}

	[DomainMethod]
	public void AcceptBuildingBlockCollectEarningQuick(DataContext context, BuildingBlockKey key, bool isPutInInventory)
	{
		if (!TryGetElement_CollectBuildingEarningsData(key, out var value))
		{
			return;
		}
		if (value.CollectionItemList != null)
		{
			int count = value.CollectionItemList.Count;
			for (int i = 0; i < count; i++)
			{
				AcceptBuildingBlockCollectEarning(context, key, 0, isPutInInventory, isSetData: false);
			}
		}
		if (value.CollectionResourceList != null)
		{
			int count2 = value.CollectionResourceList.Count;
			for (int j = 0; j < count2; j++)
			{
				AcceptBuildingBlockCollectEarning(context, key, 0, isPutInInventory: false);
			}
		}
		SetElement_CollectBuildingEarningsData(key, value, context);
	}

	private void OfflineHandleRecruitPeopleLeave(DataContext context, BuildingBlockKey buildingBlockKey, int earningDataIndex)
	{
		if (TryGetElement_CollectBuildingEarningsData(buildingBlockKey, out var value) && value != null && value.RecruitLevelList.Count > 0 && earningDataIndex >= 0)
		{
			DomainManager.Extra.RequestRecruitCharacterData(context, buildingBlockKey, earningDataIndex, autoRemove: true);
			value.RecruitLevelList.RemoveAt(earningDataIndex);
		}
	}

	[DomainMethod]
	public int AcceptBuildingBlockRecruitPeople(DataContext context, BuildingBlockKey key, int earningDataIndex, bool isSetData = true)
	{
		int num = -1;
		if (TryGetElement_CollectBuildingEarningsData(key, out var value) && value != null && value.RecruitLevelList.Count > 0 && earningDataIndex >= 0)
		{
			if (earningDataIndex >= value.RecruitLevelList.Count)
			{
				earningDataIndex = 0;
			}
			RecruitCharacterData recruitCharacterData = DomainManager.Extra.RequestRecruitCharacterData(context, key, earningDataIndex);
			num = CreateCharacterByRecruitCharacterData(context, recruitCharacterData);
			OfflineHandleRecruitPeopleLeave(context, key, earningDataIndex);
			InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
			instantNotificationCollection.AddJoinTaiwuVillage(num);
			BuildingBlockData buildingBlockData = DomainManager.Building.GetBuildingBlockData(key);
			if (buildingBlockData.TemplateId == 223)
			{
				ConsumeResource(context, 7, GlobalConfig.Instance.RecruitPeopleCost);
			}
			DomainManager.Taiwu.AddLegacyPoint(context, 31);
			if (isSetData)
			{
				SetElement_CollectBuildingEarningsData(key, value, context);
			}
		}
		return num;
	}

	[DomainMethod]
	public List<int> AcceptBuildingBlockRecruitPeopleQuick(DataContext context, BuildingBlockKey key)
	{
		List<int> list = new List<int>();
		if (!TryGetElement_CollectBuildingEarningsData(key, out var value) || value.RecruitLevelList == null)
		{
			return null;
		}
		int count = value.RecruitLevelList.Count;
		for (int i = 0; i < count; i++)
		{
			list.Add(AcceptBuildingBlockRecruitPeople(context, key, 0, isSetData: false));
		}
		SetElement_CollectBuildingEarningsData(key, value, context);
		return list;
	}

	[DomainMethod]
	public void RejectBuildingBlockRecruitPeople(DataContext context, BuildingBlockKey key, int earningDataIndex, bool isSetData = true)
	{
		if (TryGetElement_CollectBuildingEarningsData(key, out var value) && value != null && value.RecruitLevelList.Count > 0 && earningDataIndex >= 0)
		{
			if (earningDataIndex >= value.RecruitLevelList.Count)
			{
				earningDataIndex = 0;
			}
			OfflineHandleRecruitPeopleLeave(context, key, earningDataIndex);
			if (TryGetElement_BuildingBlocks(key, out var value2))
			{
				InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
				short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
				instantNotificationCollection.AddCandidateLeaved(taiwuVillageSettlementId, value2.TemplateId);
			}
			if (isSetData)
			{
				SetElement_CollectBuildingEarningsData(key, value, context);
			}
		}
	}

	[DomainMethod]
	public void RejectBuildingBlockRecruitPeopleQuick(DataContext context, BuildingBlockKey key)
	{
		if (TryGetElement_CollectBuildingEarningsData(key, out var value) && value.RecruitLevelList != null)
		{
			int count = value.RecruitLevelList.Count;
			for (int i = 0; i < count; i++)
			{
				RejectBuildingBlockRecruitPeople(context, key, 0, isSetData: false);
			}
			SetElement_CollectBuildingEarningsData(key, value, context);
		}
	}

	[Obsolete]
	[DomainMethod]
	public void ShopBuildingSoldItemAdd(DataContext context, BuildingBlockKey key, int earningDataIndex, ItemKey itemKey, bool isFromInventory)
	{
		if (!TryGetElement_BuildingBlocks(key, out var value) || earningDataIndex < 0)
		{
			return;
		}
		sbyte buildingSlotCount = SharedMethods.GetBuildingSlotCount(value.TemplateId);
		if (!TryGetElement_CollectBuildingEarningsData(key, out var value2))
		{
			value2 = new BuildingEarningsData();
			AddElement_CollectBuildingEarningsData(key, value2, context);
			for (int i = 0; i < buildingSlotCount; i++)
			{
				value2.ShopSoldItemList.Add(ItemKey.Invalid);
				value2.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
			}
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (DomainManager.Taiwu.GetWarehouseAllItemKey().Contains(itemKey))
		{
			DomainManager.Taiwu.WarehouseRemove(context, itemKey, 1);
		}
		else
		{
			taiwu.RemoveInventoryItem(context, itemKey, 1, deleteItem: false);
		}
		if (earningDataIndex >= value2.ShopSoldItemList.Count)
		{
			for (int j = 0; j < earningDataIndex - value2.ShopSoldItemList.Count + 1; j++)
			{
				value2.ShopSoldItemList.Add(ItemKey.Invalid);
				value2.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
			}
		}
		value2.ShopSoldItemList[earningDataIndex] = itemKey;
		value2.ShopSoldItemEarnList[earningDataIndex] = new IntPair(-1, -1);
		SetElement_CollectBuildingEarningsData(key, value2, context);
	}

	[Obsolete]
	[DomainMethod]
	public void ShopBuildingSoldItemChange(DataContext context, BuildingBlockKey key, int earningDataIndex, ItemKey itemKey, bool isFromInventory)
	{
		if (!TryGetElement_BuildingBlocks(key, out var value) || earningDataIndex < 0)
		{
			return;
		}
		sbyte buildingSlotCount = SharedMethods.GetBuildingSlotCount(value.TemplateId);
		if (!TryGetElement_CollectBuildingEarningsData(key, out var value2))
		{
			value2 = new BuildingEarningsData();
			AddElement_CollectBuildingEarningsData(key, value2, context);
			for (int i = 0; i < buildingSlotCount; i++)
			{
				value2.ShopSoldItemList.Add(ItemKey.Invalid);
				value2.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
			}
		}
		if (value2.ShopSoldItemList[earningDataIndex].Id == itemKey.Id)
		{
			return;
		}
		if (isFromInventory)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			if (itemKey.IsValid())
			{
				taiwu.RemoveInventoryItem(context, itemKey, 1, deleteItem: false);
			}
			taiwu.AddInventoryItem(context, value2.ShopSoldItemList[earningDataIndex], 1);
		}
		else
		{
			if (itemKey.IsValid())
			{
				DomainManager.Taiwu.WarehouseRemove(context, itemKey, 1);
			}
			DomainManager.Taiwu.WarehouseAdd(context, value2.ShopSoldItemList[earningDataIndex], 1);
		}
		value2.ShopSoldItemList[earningDataIndex] = itemKey;
		value2.ShopSoldItemEarnList[earningDataIndex] = new IntPair(-1, -1);
		SetElement_CollectBuildingEarningsData(key, value2, context);
	}

	[DomainMethod]
	public void ShopBuildingMultiChangeSoldItem(DataContext context, BuildingBlockKey key, List<ItemKey> itemList, List<int> operateTypeList)
	{
		if (!TryGetElement_BuildingBlocks(key, out var value) || itemList.Count != operateTypeList.Count)
		{
			return;
		}
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[value.TemplateId];
		sbyte buildingSlotCount = SharedMethods.GetBuildingSlotCount(buildingBlockItem.TemplateId);
		if (buildingBlockItem.TemplateId == 105)
		{
			for (int i = 0; i < itemList.Count; i++)
			{
				ItemKey itemKey = itemList[i];
				if (itemKey.IsValid())
				{
					switch (operateTypeList[i])
					{
					case 1:
						AddFixBook(context, key, itemKey, ItemSourceType.Inventory);
						break;
					case 2:
						ChangeFixBook(context, key, ItemKey.Invalid, ItemSourceType.Inventory);
						break;
					case 3:
						AddFixBook(context, key, itemKey, ItemSourceType.Warehouse);
						break;
					case 4:
						ChangeFixBook(context, key, ItemKey.Invalid, ItemSourceType.Warehouse);
						break;
					case 5:
						AddFixBook(context, key, itemKey, ItemSourceType.Treasury);
						break;
					case 6:
						ChangeFixBook(context, key, ItemKey.Invalid, ItemSourceType.Treasury);
						break;
					}
				}
			}
			return;
		}
		if (!TryGetElement_CollectBuildingEarningsData(key, out var value2))
		{
			value2 = new BuildingEarningsData();
			for (int j = 0; j < buildingSlotCount; j++)
			{
				value2.ShopSoldItemList.Add(ItemKey.Invalid);
				value2.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
			}
			AddElement_CollectBuildingEarningsData(key, value2, context);
		}
		if (value2.ShopSoldItemList.Count < buildingSlotCount)
		{
			int num = buildingSlotCount - value2.ShopSoldItemList.Count;
			for (int k = 0; k < num; k++)
			{
				value2.ShopSoldItemList.Add(ItemKey.Invalid);
				value2.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
			}
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		for (int l = 0; l < itemList.Count; l++)
		{
			ItemKey itemKey2 = itemList[l];
			if (!itemKey2.IsValid())
			{
				continue;
			}
			switch (operateTypeList[l])
			{
			case 1:
			{
				taiwu.RemoveInventoryItem(context, itemKey2, 1, deleteItem: false);
				sbyte index = FindFirstCanUseIndex(value2);
				if (index >= 0 && index < value2.ShopSoldItemList.Count)
				{
					AddBuildingEarningsDataShopSoldItem(index, itemKey2, value2, value.TemplateId);
				}
				break;
			}
			case 2:
			{
				sbyte index = FindItemKeyIndex(value2, itemKey2);
				RemoveBuildingEarningsDataShopSoldItem(index, itemKey2, value2, value.TemplateId);
				taiwu.AddInventoryItem(context, itemKey2, 1);
				break;
			}
			case 3:
			{
				DomainManager.Taiwu.WarehouseRemove(context, itemKey2, 1);
				sbyte index = FindFirstCanUseIndex(value2);
				if (index >= 0 && index < value2.ShopSoldItemList.Count)
				{
					AddBuildingEarningsDataShopSoldItem(index, itemKey2, value2, value.TemplateId);
				}
				break;
			}
			case 4:
			{
				sbyte index = FindItemKeyIndex(value2, itemKey2);
				RemoveBuildingEarningsDataShopSoldItem(index, itemKey2, value2, value.TemplateId);
				DomainManager.Taiwu.WarehouseAdd(context, itemKey2, 1);
				break;
			}
			case 5:
			{
				DomainManager.Taiwu.RemoveItem(context, itemKey2, 1, ItemSourceType.Treasury, deleteItem: false);
				sbyte index = FindFirstCanUseIndex(value2);
				if (index >= 0 && index < value2.ShopSoldItemList.Count)
				{
					AddBuildingEarningsDataShopSoldItem(index, itemKey2, value2, value.TemplateId);
				}
				break;
			}
			case 6:
			{
				sbyte index = FindItemKeyIndex(value2, itemKey2);
				RemoveBuildingEarningsDataShopSoldItem(index, itemKey2, value2, value.TemplateId);
				DomainManager.Taiwu.AddItem(context, itemKey2, 1, ItemSourceType.Treasury);
				break;
			}
			case 7:
			{
				DomainManager.Extra.RemoveStockStorageItem(context, 1, itemKey2, 1);
				sbyte index = FindFirstCanUseIndex(value2);
				if (index >= 0 && index < value2.ShopSoldItemList.Count)
				{
					AddBuildingEarningsDataShopSoldItem(index, itemKey2, value2, value.TemplateId);
				}
				break;
			}
			case 8:
			{
				sbyte index = FindItemKeyIndex(value2, itemKey2);
				RemoveBuildingEarningsDataShopSoldItem(index, itemKey2, value2, value.TemplateId);
				DomainManager.Extra.AddStockStorageItem(context, 1, itemKey2, 1);
				break;
			}
			}
		}
		SetElement_CollectBuildingEarningsData(key, value2, context);
	}

	private void AddBuildingEarningsDataShopSoldItem(int index, ItemKey itemKey, BuildingEarningsData earningsData, short templateId)
	{
		earningsData.ShopSoldItemList[index] = itemKey;
		DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, templateId);
	}

	private void RemoveBuildingEarningsDataShopSoldItem(int index, ItemKey itemKey, BuildingEarningsData earningsData, short templateId)
	{
		earningsData.ShopSoldItemList[index] = ItemKey.Invalid;
		DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Building, templateId);
	}

	private BuildingOptionAutoAddSoldItemPreset GetAutoAddSoldItemSetting(BuildingBlockKey blockKey)
	{
		DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)blockKey, out var value);
		return value?.SoldItemSetting ?? new BuildingOptionAutoAddSoldItemPreset();
	}

	private void AutoAddShopSoldItem(DataContext context, BuildingBlockKey blockKey)
	{
		BuildingBlockData buildingBlockData = GetBuildingBlockData(blockKey);
		BuildingBlockItem config = BuildingBlock.Instance[buildingBlockData.TemplateId];
		IReadOnlyDictionary<ItemKey, int> items = DomainManager.Taiwu.GetItems(ItemSourceType.StockStorageGoodsShelf);
		if (items.Count == 0)
		{
			return;
		}
		BuildingBlockKey moreBetterSoldBuilding = GetMoreBetterSoldBuilding(blockKey, buildingBlockData);
		BuildingBlockData value = null;
		BuildingEarningsData value2 = null;
		bool flag = false;
		if (!moreBetterSoldBuilding.Equals(BuildingBlockKey.Invalid))
		{
			TryGetElement_BuildingBlocks(moreBetterSoldBuilding, out value);
			flag = TryGetElement_CollectBuildingEarningsData(moreBetterSoldBuilding, out value2);
			if (value2 == null)
			{
				InitBuildingEarningsData(SharedMethods.GetBuildingSlotCount(value.TemplateId), ref value2);
			}
		}
		BuildingEarningsData value3;
		bool flag2 = TryGetElement_CollectBuildingEarningsData(blockKey, out value3);
		if (value3 == null)
		{
			InitBuildingEarningsData(SharedMethods.GetBuildingSlotCount(buildingBlockData.TemplateId), ref value3);
		}
		BuildingOptionAutoAddSoldItemPreset autoAddSoldItemSetting = GetAutoAddSoldItemSetting(blockKey);
		List<ItemKeyAndCount> list = new List<ItemKeyAndCount>();
		List<ItemKey> list2 = new List<ItemKey>();
		ItemKey itemKey;
		int count;
		foreach (KeyValuePair<ItemKey, int> item2 in items)
		{
			item2.Deconstruct(out itemKey, out count);
			ItemKey itemKey2 = itemKey;
			int count2 = count;
			if (!SharedMethods.IsBuildingCanSoldItem(config, itemKey2))
			{
				continue;
			}
			List<sbyte> itemTypeList = autoAddSoldItemSetting.ItemTypeList;
			if (itemTypeList == null || itemTypeList.Count <= 0 || autoAddSoldItemSetting.ItemTypeList.Contains(itemKey2.ItemType))
			{
				sbyte grade = ItemTemplateHelper.GetGrade(itemKey2.ItemType, itemKey2.TemplateId);
				if (grade >= autoAddSoldItemSetting.MinGrade && grade <= autoAddSoldItemSetting.MaxGrade)
				{
					list.Add(new ItemKeyAndCount(itemKey2, count2));
				}
			}
		}
		bool isGradeOrderHigh = autoAddSoldItemSetting.GradeOrder == 1;
		bool isPropertyOrderMaxValue = autoAddSoldItemSetting.PropertyOrderEnum.HasFlag(BuildingOptionAutoAddSoldItemPreset.EPropertyOrder.MaxValue);
		bool isPropertyOrderMaxAmount = autoAddSoldItemSetting.PropertyOrderEnum.HasFlag(BuildingOptionAutoAddSoldItemPreset.EPropertyOrder.MaxAmount);
		list.Sort(delegate(ItemKeyAndCount a, ItemKeyAndCount b)
		{
			sbyte grade2 = ItemTemplateHelper.GetGrade(a.ItemKey.ItemType, a.ItemKey.TemplateId);
			sbyte grade3 = ItemTemplateHelper.GetGrade(b.ItemKey.ItemType, b.ItemKey.TemplateId);
			if (grade2 != grade3)
			{
				return isGradeOrderHigh ? grade3.CompareTo(grade2) : grade2.CompareTo(grade3);
			}
			int value4 = DomainManager.Item.GetValue(a.ItemKey);
			int value5 = DomainManager.Item.GetValue(b.ItemKey);
			if (value4 != value5)
			{
				return isPropertyOrderMaxValue ? value5.CompareTo(value4) : value4.CompareTo(value5);
			}
			return (a.Count != b.Count) ? (isPropertyOrderMaxAmount ? b.Count.CompareTo(a.Count) : a.Count.CompareTo(b.Count)) : 0;
		});
		foreach (ItemKeyAndCount item3 in list)
		{
			item3.Deconstruct(out itemKey, out count);
			ItemKey item = itemKey;
			int num = count;
			for (int num2 = 0; num2 < num; num2++)
			{
				list2.Add(item);
			}
		}
		if (list2.Count <= 0)
		{
			return;
		}
		if (!moreBetterSoldBuilding.Equals(BuildingBlockKey.Invalid) && value != null)
		{
			int val = SharedMethods.GetBuildingSlotCount(value.TemplateId) - GetShopSoldItemCount(value2) - GetShopSoldEarnCount(value2);
			for (int num3 = 0; num3 < Math.Min(list2.Count, val); num3++)
			{
				int num4 = FindFirstCanUseIndex(value2);
				if (num4 < 0 && value2.ShopSoldItemList.Count < SharedMethods.GetBuildingSlotCount(value.TemplateId))
				{
					value2.ShopSoldItemList.Add(ItemKey.Invalid);
					value2.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
					num4 = value2.ShopSoldItemList.Count - 1;
				}
				if (num4 >= 0 && num4 < value2.ShopSoldItemList.Count)
				{
					DomainManager.Extra.RemoveStockStorageItem(context, 1, list2[num3], 1);
					AddBuildingEarningsDataShopSoldItem(num4, list2[num3], value2, buildingBlockData.TemplateId);
					list2.RemoveAt(num3);
				}
			}
			if (flag)
			{
				SetElement_CollectBuildingEarningsData(moreBetterSoldBuilding, value2, context);
			}
			else
			{
				AddElement_CollectBuildingEarningsData(moreBetterSoldBuilding, value2, context);
			}
		}
		int val2 = SharedMethods.GetBuildingSlotCount(buildingBlockData.TemplateId) - GetShopSoldItemCount(value3) - GetShopSoldEarnCount(value3);
		for (int num5 = 0; num5 < Math.Min(list2.Count, val2); num5++)
		{
			int num6 = FindFirstCanUseIndex(value3);
			if (num6 < 0 && value3.ShopSoldItemList.Count < SharedMethods.GetBuildingSlotCount(buildingBlockData.TemplateId))
			{
				value3.ShopSoldItemList.Add(ItemKey.Invalid);
				value3.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
				num6 = value3.ShopSoldItemList.Count - 1;
			}
			if (num6 >= 0 && num6 < value3.ShopSoldItemList.Count)
			{
				DomainManager.Extra.RemoveStockStorageItem(context, 1, list2[num5], 1);
				AddBuildingEarningsDataShopSoldItem(num6, list2[num5], value3, buildingBlockData.TemplateId);
			}
		}
		if (flag2)
		{
			SetElement_CollectBuildingEarningsData(blockKey, value3, context);
		}
		else
		{
			AddElement_CollectBuildingEarningsData(blockKey, value3, context);
		}
		static void InitBuildingEarningsData(sbyte level, ref BuildingEarningsData earningsData)
		{
			earningsData = new BuildingEarningsData();
			for (int i = 0; i < level; i++)
			{
				earningsData.ShopSoldItemList.Add(ItemKey.Invalid);
				earningsData.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
			}
		}
	}

	[DomainMethod]
	public void QuickAddShopSoldItem(DataContext context, BuildingBlockKey blockKey)
	{
		QuickRemoveShopSoldItem(context, blockKey);
		AutoAddShopSoldItem(context, blockKey);
		if (TryGetElement_CollectBuildingEarningsData(blockKey, out var value))
		{
			SetElement_CollectBuildingEarningsData(blockKey, value, context);
		}
	}

	[DomainMethod]
	public void QuickRemoveShopSoldItem(DataContext context, BuildingBlockKey blockKey)
	{
		if (!TryGetElement_CollectBuildingEarningsData(blockKey, out var value))
		{
			return;
		}
		for (sbyte b = 0; b < value.ShopSoldItemList.Count; b++)
		{
			ItemKey itemKey = value.ShopSoldItemList[b];
			IntPair intPair = value.ShopSoldItemEarnList[b];
			if (!itemKey.Equals(ItemKey.Invalid) && intPair.First == -1)
			{
				BuildingBlockData buildingBlockData = GetBuildingBlockData(blockKey);
				RemoveBuildingEarningsDataShopSoldItem(b, itemKey, value, buildingBlockData.TemplateId);
				DomainManager.Extra.AddStockStorageItem(context, 1, itemKey, 1);
			}
		}
		SetElement_CollectBuildingEarningsData(blockKey, value, context);
	}

	public sbyte FindFirstCanUseIndex(BuildingEarningsData earningsData)
	{
		sbyte result = -1;
		for (sbyte b = 0; b < earningsData.ShopSoldItemList.Count; b++)
		{
			if (earningsData.ShopSoldItemList[b].Equals(ItemKey.Invalid) && earningsData.ShopSoldItemEarnList[b].First == -1)
			{
				result = b;
				break;
			}
		}
		return result;
	}

	public sbyte FindItemKeyIndex(BuildingEarningsData earningsData, ItemKey itemKey)
	{
		sbyte result = 0;
		for (sbyte b = 0; b < earningsData.ShopSoldItemList.Count; b++)
		{
			if (earningsData.ShopSoldItemList[b].Equals(itemKey))
			{
				result = b;
				break;
			}
		}
		return result;
	}

	public int GetShopSoldItemCount(BuildingEarningsData earningsData)
	{
		if (earningsData == null || earningsData.ShopSoldItemList == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < earningsData.ShopSoldItemList.Count; i++)
		{
			if (!earningsData.ShopSoldItemList[i].Equals(ItemKey.Invalid))
			{
				num++;
			}
		}
		return num;
	}

	public int GetShopSoldEarnCount(BuildingEarningsData earningsData)
	{
		if (earningsData == null || earningsData.ShopSoldItemEarnList == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < earningsData.ShopSoldItemEarnList.Count; i++)
		{
			if (earningsData.ShopSoldItemEarnList[i].First != -1)
			{
				num++;
			}
		}
		return num;
	}

	[DomainMethod]
	public void ShopBuildingSoldItemReceive(DataContext context, BuildingBlockKey key, int earningDataIndex, bool isSetData = true)
	{
		if (TryGetElement_CollectBuildingEarningsData(key, out var value) && earningDataIndex >= 0)
		{
			IntPair intPair = value.ShopSoldItemEarnList[earningDataIndex];
			DomainManager.Character.GetElement_Objects(DomainManager.Taiwu.GetTaiwuCharId()).ChangeResource(context, (sbyte)intPair.First, intPair.Second);
			ApplyBuildingResourceOutputSetting(context, key, (sbyte)intPair.First, intPair.Second);
			value.ShopSoldItemList[earningDataIndex] = ItemKey.Invalid;
			value.ShopSoldItemEarnList[earningDataIndex] = new IntPair(-1, -1);
			DomainManager.Building.AddBuildingGainLegacy(context, key);
		}
		if (isSetData)
		{
			SetElement_CollectBuildingEarningsData(key, value, context);
		}
	}

	[DomainMethod]
	public void ShopBuildingSoldItemReceiveQuick(DataContext context, BuildingBlockKey key)
	{
		if (!TryGetElement_CollectBuildingEarningsData(key, out var value) || value == null)
		{
			return;
		}
		for (int i = 0; i < value.ShopSoldItemEarnList.Count; i++)
		{
			if (value.ShopSoldItemEarnList[i].First != -1)
			{
				ShopBuildingSoldItemReceive(context, key, i, isSetData: false);
			}
		}
		SetElement_CollectBuildingEarningsData(key, value, context);
	}

	[DomainMethod]
	public List<ItemDisplayData> QuickCollectShopItem(DataContext context)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(taiwuVillageLocation);
		List<ItemKey> list = new List<ItemKey>();
		for (short num = 0; num < element_BuildingAreas.Width * element_BuildingAreas.Width; num++)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, num);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
			if (element_BuildingBlocks.TemplateId > 0)
			{
				BuildingBlockItem item = BuildingBlock.Instance.GetItem(element_BuildingBlocks.TemplateId);
				if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).ItemList.Count > 0 && element_BuildingBlocks.TemplateId != 222)
				{
					CollectItemQuick(context, buildingBlockKey, list);
				}
			}
		}
		foreach (Feast value in DomainManager.Extra.GetAllFeasts().Values)
		{
			for (int i = 0; i < GlobalConfig.Instance.FeastGiftCount; i++)
			{
				ItemKey gift = value.GetGift(i);
				if (gift != ItemKey.Invalid && !ItemTemplateHelper.IsMiscResource(gift.ItemType, gift.TemplateId))
				{
					list.Add(gift);
				}
			}
			DomainManager.Extra.FeastReceiveGift(context, value.BuildingBlockKey, -1);
		}
		return DomainManager.Item.GetItemDisplayDataListOptional(list, DomainManager.Taiwu.GetTaiwuCharId(), -1);
	}

	[DomainMethod]
	public List<ItemDisplayData> QuickCollectSingleShopItem(DataContext context, BuildingBlockKey blockKey)
	{
		List<ItemKey> itemKeyList = new List<ItemKey>();
		BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
		BuildingBlockItem item = BuildingBlock.Instance.GetItem(element_BuildingBlocks.TemplateId);
		if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).ItemList.Count > 0)
		{
			CollectItemQuick(context, blockKey, itemKeyList);
		}
		return DomainManager.Item.GetItemDisplayDataList(itemKeyList, DomainManager.Taiwu.GetTaiwuCharId());
	}

	public void CollectItemQuick(DataContext context, BuildingBlockKey key, List<ItemKey> itemKeyList)
	{
		if (!TryGetElement_CollectBuildingEarningsData(key, out var value))
		{
			return;
		}
		if (value.CollectionItemList != null)
		{
			int count = value.CollectionItemList.Count;
			for (int i = 0; i < count; i++)
			{
				CollectShopItem(context, key, 0, itemKeyList);
			}
		}
		SetElement_CollectBuildingEarningsData(key, value, context);
	}

	public BuildingBlockKey CollectShopItem(DataContext context, BuildingBlockKey key, int earningDataIndex, List<ItemKey> itemKeyList)
	{
		if (TryGetElement_CollectBuildingEarningsData(key, out var value) && earningDataIndex >= 0 && earningDataIndex < value.CollectionItemList.Count)
		{
			ItemKey itemKey = value.CollectionItemList[earningDataIndex];
			itemKey.ModificationState = 0;
			value.CollectionItemList.RemoveAt(earningDataIndex);
			ItemSourceType itemSourceType = ApplyBuildingItemOutputSetting(key, itemKey);
			DomainManager.Taiwu.AddItem(context, itemKey, 1, itemSourceType);
			itemKeyList.Add(itemKey);
			AddBuildingGainLegacy(context, key);
			BuildingChangeProfessionSeniority(context, itemKey);
		}
		return key;
	}

	public void AddBuildingGainLegacy(DataContext context, BuildingBlockKey key)
	{
		if (DomainManager.Building.TryGetElement_BuildingBlocks(key, out var value))
		{
			BuildingBlockItem item = BuildingBlock.Instance.GetItem(value.TemplateId);
			DomainManager.Taiwu.AddLegacyPoint(context, (short)(item.IsCollectResourceBuilding ? 28 : 29));
		}
	}

	private void BuildingChangeProfessionSeniority(DataContext context, ItemKey itemKey)
	{
		short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
		int value = DomainManager.Item.GetBaseItem(itemKey).GetValue();
		switch (itemSubType)
		{
		case 901:
		{
			ProfessionFormulaItem formulaCfg2 = ProfessionFormula.Instance[54];
			DomainManager.Extra.ChangeProfessionSeniority(context, 7, formulaCfg2.Calculate(value));
			break;
		}
		case 900:
		{
			ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[105];
			DomainManager.Extra.ChangeProfessionSeniority(context, 16, formulaCfg.Calculate(value));
			break;
		}
		}
	}

	[DomainMethod]
	public int QuickCollectShopItemCount(DataContext context)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(taiwuVillageLocation);
		int num = 0;
		for (short num2 = 0; num2 < element_BuildingAreas.Width * element_BuildingAreas.Width; num2++)
		{
			BuildingBlockKey elementId = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, num2);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(elementId);
			if (element_BuildingBlocks.TemplateId > 0)
			{
				BuildingBlockItem item = BuildingBlock.Instance.GetItem(element_BuildingBlocks.TemplateId);
				if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).ItemList.Count > 0 && element_BuildingBlocks.TemplateId != 222 && TryGetElement_CollectBuildingEarningsData(elementId, out var value) && value.CollectionItemList != null)
				{
					num += value.CollectionItemList.Count;
				}
			}
		}
		return num;
	}

	[DomainMethod]
	public void QuickCollectShopSoldItem(DataContext context)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(taiwuVillageLocation);
		for (short num = 0; num < element_BuildingAreas.Width * element_BuildingAreas.Width; num++)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, num);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
			if (element_BuildingBlocks.TemplateId > 0)
			{
				BuildingBlockItem item = BuildingBlock.Instance.GetItem(element_BuildingBlocks.TemplateId);
				if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).ExchangeResourceGoods != -1)
				{
					ShopBuildingSoldItemReceiveQuick(context, buildingBlockKey);
				}
				if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).ResourceGoods != -1)
				{
					AcceptBuildingBlockCollectEarningQuick(context, buildingBlockKey, isPutInInventory: false);
				}
			}
		}
	}

	[DomainMethod]
	public void QuickCollectSingleShopSoldItem(DataContext context, BuildingBlockKey blockKey)
	{
		BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
		BuildingBlockItem item = BuildingBlock.Instance.GetItem(element_BuildingBlocks.TemplateId);
		if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).ExchangeResourceGoods != -1)
		{
			ShopBuildingSoldItemReceiveQuick(context, blockKey);
		}
		if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).ResourceGoods != -1)
		{
			AcceptBuildingBlockCollectEarningQuick(context, blockKey, isPutInInventory: false);
		}
	}

	[DomainMethod]
	public int QuickCollectShopSoldItemCount(DataContext context)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(taiwuVillageLocation);
		int num = 0;
		for (short num2 = 0; num2 < element_BuildingAreas.Width * element_BuildingAreas.Width; num2++)
		{
			BuildingBlockKey elementId = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, num2);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(elementId);
			if (element_BuildingBlocks.TemplateId <= 0)
			{
				continue;
			}
			BuildingBlockItem item = BuildingBlock.Instance.GetItem(element_BuildingBlocks.TemplateId);
			if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).ExchangeResourceGoods != -1 && TryGetElement_CollectBuildingEarningsData(elementId, out var value))
			{
				if (value == null)
				{
					continue;
				}
				for (int i = 0; i < value.ShopSoldItemEarnList.Count; i++)
				{
					if (value.ShopSoldItemEarnList[i].First != -1)
					{
						num++;
					}
				}
			}
			if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).ResourceGoods != -1 && TryGetElement_CollectBuildingEarningsData(elementId, out var value2) && value2.CollectionResourceList != null)
			{
				num += value2.CollectionResourceList.Count;
			}
		}
		return num;
	}

	[DomainMethod]
	public List<int> QuickRecruitPeople(DataContext context)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(taiwuVillageLocation);
		List<int> list = new List<int>();
		for (short num = 0; num < element_BuildingAreas.Width * element_BuildingAreas.Width; num++)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, num);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
			if (element_BuildingBlocks.TemplateId > 0)
			{
				BuildingBlockItem item = BuildingBlock.Instance.GetItem(element_BuildingBlocks.TemplateId);
				if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).RecruitPeopleProb.Count > 0 && element_BuildingBlocks.TemplateId != 223)
				{
					RecruitPeopleQuick(context, buildingBlockKey, list);
				}
			}
		}
		return list;
	}

	[DomainMethod]
	public List<int> QuickRecruitSingleBuildingPeople(DataContext context, BuildingBlockKey blockKey)
	{
		List<int> list = new List<int>();
		BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
		BuildingBlockItem item = BuildingBlock.Instance.GetItem(element_BuildingBlocks.TemplateId);
		if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).RecruitPeopleProb.Count > 0)
		{
			RecruitPeopleQuick(context, blockKey, list);
		}
		return list;
	}

	public void RecruitPeopleQuick(DataContext context, BuildingBlockKey key, List<int> charIdList)
	{
		if (TryGetElement_CollectBuildingEarningsData(key, out var value) && value.RecruitLevelList != null)
		{
			int count = value.RecruitLevelList.Count;
			for (int i = 0; i < count; i++)
			{
				RecruitPeople(context, key, 0, charIdList);
			}
			SetElement_CollectBuildingEarningsData(key, value, context);
		}
	}

	public void RecruitPeople(DataContext context, BuildingBlockKey key, int earningDataIndex, List<int> charIdList)
	{
		charIdList.Add(AcceptBuildingBlockRecruitPeople(context, key, earningDataIndex, isSetData: false));
	}

	[DomainMethod]
	public int QuickRecruitPeopleCount(DataContext context)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(taiwuVillageLocation);
		int num = 0;
		for (short num2 = 0; num2 < element_BuildingAreas.Width * element_BuildingAreas.Width; num2++)
		{
			BuildingBlockKey elementId = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, num2);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(elementId);
			if (element_BuildingBlocks.TemplateId > 0)
			{
				BuildingBlockItem item = BuildingBlock.Instance.GetItem(element_BuildingBlocks.TemplateId);
				if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).RecruitPeopleProb.Count > 0 && element_BuildingBlocks.TemplateId != 223 && TryGetElement_CollectBuildingEarningsData(elementId, out var value) && value.RecruitLevelList != null)
				{
					num += value.RecruitLevelList.Count;
				}
			}
		}
		return num;
	}

	[DomainMethod]
	public void QuickCollectBuildingEarn(DataContext context)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(taiwuVillageLocation);
		for (short num = 0; num < element_BuildingAreas.Width * element_BuildingAreas.Width; num++)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, num);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
			if (element_BuildingBlocks.TemplateId <= 0)
			{
				continue;
			}
			BuildingBlockItem item = BuildingBlock.Instance.GetItem(element_BuildingBlocks.TemplateId);
			if (item.SuccesEvent.Count != 0 && (Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).ItemList.Count > 0 || Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).ResourceGoods != -1))
			{
				AcceptBuildingBlockCollectEarningQuick(context, buildingBlockKey, isPutInInventory: false);
			}
			if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).ExchangeResourceGoods != -1)
			{
				if (element_BuildingBlocks.TemplateId == 222)
				{
					continue;
				}
				ShopBuildingSoldItemReceiveQuick(context, buildingBlockKey);
			}
			if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).RecruitPeopleProb.Count > 0 && element_BuildingBlocks.TemplateId != 223)
			{
				AcceptBuildingBlockRecruitPeopleQuick(context, buildingBlockKey);
			}
		}
	}

	[DomainMethod]
	public int QuickCollectBuildingEarnCount(DataContext context)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(taiwuVillageLocation);
		int num = 0;
		for (short num2 = 0; num2 < element_BuildingAreas.Width * element_BuildingAreas.Width; num2++)
		{
			BuildingBlockKey elementId = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, num2);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(elementId);
			if (element_BuildingBlocks.TemplateId <= 0)
			{
				continue;
			}
			BuildingBlockItem item = BuildingBlock.Instance.GetItem(element_BuildingBlocks.TemplateId);
			if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).RecruitPeopleProb.Count > 0)
			{
				if (element_BuildingBlocks.TemplateId == 223 || !TryGetElement_CollectBuildingEarningsData(elementId, out var value) || value.RecruitLevelList == null)
				{
					continue;
				}
				num += value.RecruitLevelList.Count;
			}
			if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).ExchangeResourceGoods != -1 && TryGetElement_CollectBuildingEarningsData(elementId, out var value2))
			{
				if (value2 == null)
				{
					continue;
				}
				for (int i = 0; i < value2.ShopSoldItemEarnList.Count; i++)
				{
					if (value2.ShopSoldItemEarnList[i].First != -1)
					{
						num++;
					}
				}
			}
			if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).ResourceGoods != -1)
			{
				if (!TryGetElement_CollectBuildingEarningsData(elementId, out var value3))
				{
					continue;
				}
				if (value3.CollectionResourceList != null)
				{
					num += value3.CollectionResourceList.Count;
				}
			}
			if (item.SuccesEvent.Count != 0 && Config.ShopEvent.Instance.GetItem(item.SuccesEvent[0]).ItemList.Count > 0 && element_BuildingBlocks.TemplateId != 222 && TryGetElement_CollectBuildingEarningsData(elementId, out var value4) && value4.CollectionItemList != null)
			{
				num += value4.CollectionItemList.Count;
			}
		}
		return num;
	}

	[DomainMethod]
	public void SetBuildingAutoWork(DataContext context, short blockIndex, bool isAutoWork)
	{
		List<short> autoWorkBlockIndexList = DomainManager.Extra.GetAutoWorkBlockIndexList();
		if (isAutoWork && !autoWorkBlockIndexList.Contains(blockIndex))
		{
			autoWorkBlockIndexList.Add(blockIndex);
			DomainManager.Extra.SetAutoWorkBlockIndexList(autoWorkBlockIndexList, context);
		}
		if (!isAutoWork && autoWorkBlockIndexList.Contains(blockIndex))
		{
			autoWorkBlockIndexList.Remove(blockIndex);
			DomainManager.Extra.SetAutoWorkBlockIndexList(autoWorkBlockIndexList, context);
		}
	}

	[DomainMethod]
	public bool GetBuildingIsAutoWork(short blockIndex)
	{
		List<short> autoWorkBlockIndexList = DomainManager.Extra.GetAutoWorkBlockIndexList();
		return autoWorkBlockIndexList.Contains(blockIndex);
	}

	[DomainMethod]
	public void SetBuildingAutoSold(DataContext context, short blockIndex, bool isAutoSold)
	{
		List<short> autoSoldBlockIndexList = DomainManager.Extra.GetAutoSoldBlockIndexList();
		if (isAutoSold && !autoSoldBlockIndexList.Contains(blockIndex))
		{
			autoSoldBlockIndexList.Add(blockIndex);
			DomainManager.Extra.SetAutoSoldBlockIndexList(autoSoldBlockIndexList, context);
		}
		if (!isAutoSold && autoSoldBlockIndexList.Contains(blockIndex))
		{
			autoSoldBlockIndexList.Remove(blockIndex);
			DomainManager.Extra.SetAutoSoldBlockIndexList(autoSoldBlockIndexList, context);
		}
	}

	[DomainMethod]
	public bool GetBuildingIsAutoSold(short blockIndex)
	{
		List<short> autoSoldBlockIndexList = DomainManager.Extra.GetAutoSoldBlockIndexList();
		return autoSoldBlockIndexList.Contains(blockIndex);
	}

	[DomainMethod]
	public bool GetResidenceIsAutoCheckIn(short blockIndex)
	{
		List<short> autoCheckInResidenceList = DomainManager.Extra.GetAutoCheckInResidenceList();
		return autoCheckInResidenceList.Contains(blockIndex);
	}

	[DomainMethod]
	public bool GetComfortableIsAutoCheckIn(short blockIndex)
	{
		List<short> autoCheckInComfortableList = DomainManager.Extra.GetAutoCheckInComfortableList();
		return autoCheckInComfortableList.Contains(blockIndex);
	}

	[DomainMethod]
	public void SetUnlockedWorkingVillagers(DataContext context, int charId, bool add)
	{
		if (charId < 0)
		{
			throw new Exception($"CharId is illegal, id: {charId}");
		}
		List<int> unlockedWorkingVillagers = DomainManager.Extra.GetUnlockedWorkingVillagers();
		if (add && !unlockedWorkingVillagers.Contains(charId))
		{
			unlockedWorkingVillagers.Add(charId);
		}
		if (!add && unlockedWorkingVillagers.Contains(charId))
		{
			unlockedWorkingVillagers.Remove(charId);
		}
		DomainManager.Extra.SetUnlockedWorkingVillagers(unlockedWorkingVillagers, context);
	}

	public void TryRemoveUnlockedWorkingVillager(DataContext context, int charId)
	{
		if (charId < 0)
		{
			throw new Exception($"CharId is illegal, id: {charId}");
		}
		List<int> unlockedWorkingVillagers = DomainManager.Extra.GetUnlockedWorkingVillagers();
		if (unlockedWorkingVillagers.Contains(charId))
		{
			unlockedWorkingVillagers.Remove(charId);
			DomainManager.Extra.SetUnlockedWorkingVillagers(unlockedWorkingVillagers, context);
		}
	}

	[DomainMethod]
	public List<sbyte> GetXiangshuIdInKungfuRoom()
	{
		return DomainManager.Extra.GetXiangshuIdInKungfuPracticeRoom();
	}

	[DomainMethod]
	[Obsolete]
	public void SetShopIsResultFirst(DataContext context, short blockIndex, bool resultFirst)
	{
		List<short> shopArrangeResultFirstList = DomainManager.Extra.GetShopArrangeResultFirstList();
		if (resultFirst && !shopArrangeResultFirstList.Contains(blockIndex))
		{
			shopArrangeResultFirstList.Add(blockIndex);
			DomainManager.Extra.SetShopArrangeResultFirstList(shopArrangeResultFirstList, context);
		}
		if (!resultFirst && shopArrangeResultFirstList.Contains(blockIndex))
		{
			shopArrangeResultFirstList.Remove(blockIndex);
			DomainManager.Extra.SetShopArrangeResultFirstList(shopArrangeResultFirstList, context);
		}
	}

	[DomainMethod]
	[Obsolete]
	public bool GetShopIsResultFirst(short blockIndex)
	{
		List<short> shopArrangeResultFirstList = DomainManager.Extra.GetShopArrangeResultFirstList();
		return shopArrangeResultFirstList.Contains(blockIndex);
	}

	[DomainMethod]
	[Obsolete]
	public void SetBuildingAutoExpand(DataContext context, short blockIndex, bool isAutoExpand)
	{
		List<short> autoExpandBlockIndexList = DomainManager.Extra.GetAutoExpandBlockIndexList();
		if (isAutoExpand && !autoExpandBlockIndexList.Contains(blockIndex))
		{
			autoExpandBlockIndexList.Insert(0, blockIndex);
			DomainManager.Extra.SetAutoExpandBlockIndexList(autoExpandBlockIndexList, context);
		}
		if (!isAutoExpand && autoExpandBlockIndexList.Contains(blockIndex))
		{
			autoExpandBlockIndexList.Remove(blockIndex);
			DomainManager.Extra.SetAutoExpandBlockIndexList(autoExpandBlockIndexList, context);
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, blockIndex);
			if (TryGetElement_BuildingBlocks(buildingBlockKey, out var value) && value.OperationType == -1)
			{
				RemoveAllOperatorsInBuilding(context, buildingBlockKey);
			}
		}
	}

	[DomainMethod]
	[Obsolete]
	public bool GetBuildingIsAutoExpand(short blockIndex)
	{
		List<short> autoExpandBlockIndexList = DomainManager.Extra.GetAutoExpandBlockIndexList();
		return autoExpandBlockIndexList.Contains(blockIndex);
	}

	[DomainMethod]
	[Obsolete]
	public void SetBuildingAutoExpandUp(DataContext context, short blockIndex)
	{
		List<short> autoExpandBlockIndexList = DomainManager.Extra.GetAutoExpandBlockIndexList();
		int num = autoExpandBlockIndexList.IndexOf(blockIndex);
		if (num > 0)
		{
			List<short> list = autoExpandBlockIndexList;
			int index = num;
			int index2 = num - 1;
			short value = autoExpandBlockIndexList[num - 1];
			short value2 = autoExpandBlockIndexList[num];
			list[index] = value;
			autoExpandBlockIndexList[index2] = value2;
			DomainManager.Extra.SetAutoExpandBlockIndexList(autoExpandBlockIndexList, context);
		}
	}

	[DomainMethod]
	public void SetBuildingAutoExpandDown(DataContext context, short blockIndex)
	{
		List<short> autoExpandBlockIndexList = DomainManager.Extra.GetAutoExpandBlockIndexList();
		int num = autoExpandBlockIndexList.IndexOf(blockIndex);
		if (num >= 0 && num < autoExpandBlockIndexList.Count - 1)
		{
			List<short> list = autoExpandBlockIndexList;
			int index = num;
			int index2 = num + 1;
			short value = autoExpandBlockIndexList[num + 1];
			short value2 = autoExpandBlockIndexList[num];
			list[index] = value;
			autoExpandBlockIndexList[index2] = value2;
			DomainManager.Extra.SetAutoExpandBlockIndexList(autoExpandBlockIndexList, context);
		}
	}

	[DomainMethod]
	[Obsolete]
	public void SetBuildingAutoExpandUpTop(DataContext context, short blockIndex)
	{
		List<short> autoExpandBlockIndexList = DomainManager.Extra.GetAutoExpandBlockIndexList();
		int num = autoExpandBlockIndexList.IndexOf(blockIndex);
		if (num > 0)
		{
			short value = autoExpandBlockIndexList[num];
			for (int num2 = num - 1; num2 >= 0; num2--)
			{
				autoExpandBlockIndexList[num2 + 1] = autoExpandBlockIndexList[num2];
			}
			autoExpandBlockIndexList[0] = value;
			DomainManager.Extra.SetAutoExpandBlockIndexList(autoExpandBlockIndexList, context);
		}
	}

	[DomainMethod]
	[Obsolete]
	public void SetBuildingAutoExpandDownBottom(DataContext context, short blockIndex)
	{
		List<short> autoExpandBlockIndexList = DomainManager.Extra.GetAutoExpandBlockIndexList();
		int num = autoExpandBlockIndexList.IndexOf(blockIndex);
		if (num >= 0 && num < autoExpandBlockIndexList.Count - 1)
		{
			short value = autoExpandBlockIndexList[num];
			for (int i = num; i < autoExpandBlockIndexList.Count - 1; i++)
			{
				autoExpandBlockIndexList[i] = autoExpandBlockIndexList[i + 1];
			}
			autoExpandBlockIndexList[autoExpandBlockIndexList.Count - 1] = value;
			DomainManager.Extra.SetAutoExpandBlockIndexList(autoExpandBlockIndexList, context);
		}
	}

	private bool CanUpdateShopProgress(BuildingBlockKey blockKey)
	{
		if (!TryGetElement_BuildingBlocks(blockKey, out var value))
		{
			return false;
		}
		if (!HasShopManagerLeader(blockKey))
		{
			return false;
		}
		BuildingBlockItem configData = value.ConfigData;
		if (!value.CanUse() || !configData.NeedShopProgress)
		{
			return false;
		}
		if (!AllDependBuildingAvailable(blockKey, value.TemplateId, out var _))
		{
			return false;
		}
		if (configData.TemplateId == 105)
		{
			return true;
		}
		ShopEventItem shopEventItem = Config.ShopEvent.Instance[configData.SuccesEvent[0]];
		sbyte buildingSlotCount = SharedMethods.GetBuildingSlotCount(configData.TemplateId);
		BuildingEarningsData value2;
		if (shopEventItem.ExchangeResourceGoods >= 0)
		{
			return TryGetElement_CollectBuildingEarningsData(blockKey, out value2) && value2 != null && value2.ShopSoldItemList != null && !value2.ShopSoldItemList.All(ItemKey.Invalid);
		}
		BuildingEarningsData value3;
		if (shopEventItem.ResourceGoods >= 0)
		{
			return !TryGetElement_CollectBuildingEarningsData(blockKey, out value3) || value3.CollectionResourceList == null || value3.CollectionResourceList.Count < buildingSlotCount;
		}
		BuildingEarningsData value4;
		if (shopEventItem.ItemList.Count > 0 || shopEventItem.ItemGradeProbList.Count > 0)
		{
			return !TryGetElement_CollectBuildingEarningsData(blockKey, out value4) || value4.CollectionItemList == null || value4.CollectionItemList.Count < buildingSlotCount;
		}
		BuildingEarningsData value5;
		if (shopEventItem.RecruitPeopleProb.Count > 0)
		{
			return !TryGetElement_CollectBuildingEarningsData(blockKey, out value5) || value5.RecruitLevelList == null || value5.RecruitLevelList.Count < buildingSlotCount;
		}
		return false;
	}

	public int CalcRecruitGrade(IRandomSource random, BuildingBlockKey blockKey, ref int score)
	{
		bool hasManager;
		int num = BuildingTotalAttainment(blockKey, -1, out hasManager);
		score += num * (random.NextBool() ? 80 : 120) / 100;
		int[] recruitCharacterGradeScoreThresholds = GlobalConfig.Instance.RecruitCharacterGradeScoreThresholds;
		sbyte b = 8;
		while (b >= 0 && score <= recruitCharacterGradeScoreThresholds[b])
		{
			b--;
		}
		score = Math.Max(score - recruitCharacterGradeScoreThresholds[b], 0);
		return b;
	}

	public int CalcSoldItemValue(IRandomSource random, BuildingBlockKey blockKey, BuildingBlockData blockData, ItemKey itemKey, out int basePrice, out BuildingProduceDependencyData dependencyData)
	{
		ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
		int value = baseItem.GetValue();
		basePrice = Math.Max(value, 1);
		int saleMiddleValue;
		return CalcSoldItemValueBySpecificBaseLine(random, blockKey, blockData, out dependencyData, basePrice, out saleMiddleValue);
	}

	public int CalcSoldItemValueBySpecificBaseLine(IRandomSource random, BuildingBlockKey blockKey, BuildingBlockData blockData, out BuildingProduceDependencyData dependencyData, int baseLine, out int saleMiddleValue)
	{
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		dependencyData = BuildingProduceDependencyData.Invalid;
		dependencyData.RandomFactorLowerLimit = GlobalConfig.Instance.BuildingOutputRandomFactorLowerLimit;
		dependencyData.RandomFactorUpperLimit = GlobalConfig.Instance.BuildingOutputRandomFactorUpperLimit;
		dependencyData.TemplateId = blockData.TemplateId;
		dependencyData.Level = BuildingBlockLevel(blockKey);
		BuildingBlockItem item = BuildingBlock.Instance.GetItem(blockData.TemplateId);
		dependencyData.ProductivityFactor = BuildingProductivityByMaxDependencies(blockKey);
		dependencyData.SafetyCultureFactor = CalcSafetyOrCultureFactor(item);
		dependencyData.TotalAttainmentFactor = dependencyData.BuildSaleItemAttainmentFactor(BuildingTotalAttainment(blockKey, -1, out var _));
		dependencyData.GainResourcePercentFactor = DomainManager.World.GetGainResourcePercent(2);
		saleMiddleValue = dependencyData.CalcSaleItemPrice(baseLine);
		int num = BuildingRandomCorrection(saleMiddleValue, random);
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[blockData.TemplateId];
		ShopEventItem shopEventItem = Config.ShopEvent.Instance[buildingBlockItem.SuccesEvent[0]];
		sbyte buildingResourceGoodsOrExchangeResourceGoodsType = SharedMethods.GetBuildingResourceGoodsOrExchangeResourceGoodsType(buildingBlockItem, shopEventItem);
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(DomainManager.Building.GetBuildingBlockEffect(blockKey.GetLocation(), (buildingResourceGoodsOrExchangeResourceGoodsType == 7) ? EBuildingScaleEffect.BuildingAuthorityIncomeBonus : EBuildingScaleEffect.BuildingMoneyIncomeBonus));
		return num * val;
	}

	private int CalcSafetyOrCultureFactor(BuildingBlockItem config)
	{
		SharedMethods.PickSafetyOrCultureFactorSettlements(config, DomainManager.Taiwu.GetAllVisitedSettlements(), out var addition);
		return 100 + addition;
	}

	public ItemKey GetRandomItemByGrade(IRandomSource randomSource, sbyte grade, short itemSubType = -1)
	{
		if (itemSubType == -1)
		{
			itemSubType = ItemSubType.GetRandom(randomSource);
			while (!ItemSubType.IsHobbyType(itemSubType))
			{
				itemSubType = ItemSubType.GetRandom(randomSource);
			}
		}
		short randomItemIdInSubType = ItemDomain.GetRandomItemIdInSubType(randomSource, itemSubType, grade);
		sbyte type = ItemSubType.GetType(itemSubType);
		return new ItemKey(type, 0, randomItemIdInSubType, -1);
	}

	public BuildingBlockData GetBuildingBlockData(int templateId)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(taiwuVillageLocation);
		for (short num = 0; num < element_BuildingAreas.Width * element_BuildingAreas.Width; num++)
		{
			BuildingBlockKey elementId = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, num);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(elementId);
			if (element_BuildingBlocks.TemplateId == templateId)
			{
				return element_BuildingBlocks;
			}
		}
		return null;
	}

	public bool IsShopNeedManager(BuildingBlockKey blockKey, BuildingBlockData blockData)
	{
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[blockData.TemplateId];
		return GetBuildingAttainmentUniversalWhetherCanWork(blockData, blockKey) < buildingBlockItem.MaxProduceValue;
	}

	public sbyte GetNeedLifeSkillType(BuildingBlockItem config, BuildingBlockKey blockKey)
	{
		return config.RequireLifeSkillType;
	}

	public bool IsTaiwuVillageHaveSpecifyBuilding(short templateId, bool notBuild = false)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(taiwuVillageLocation);
		for (short num = 0; num < element_BuildingAreas.Width * element_BuildingAreas.Width; num++)
		{
			BuildingBlockKey elementId = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, num);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(elementId);
			if (element_BuildingBlocks.TemplateId == templateId)
			{
				if (notBuild)
				{
					return element_BuildingBlocks.OperationType != 0;
				}
				return true;
			}
		}
		return false;
	}

	public BuildingBlockKey GetMoreBetterSoldBuilding(BuildingBlockKey orgiBlockKey, BuildingBlockData orgiBlockData)
	{
		BuildingBlockKey result = BuildingBlockKey.Invalid;
		int num = (40 + SharedMethods.GetBuildingSlotCount(orgiBlockData.TemplateId) * 2) * (100 + GetBuildingAttainmentUniversalWhetherCanWork(orgiBlockData, orgiBlockKey) / 4) / 100;
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(taiwuVillageLocation);
		List<short> autoSoldBlockIndexList = DomainManager.Extra.GetAutoSoldBlockIndexList();
		for (short num2 = 0; num2 < element_BuildingAreas.Width * element_BuildingAreas.Width; num2++)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, num2);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
			if (autoSoldBlockIndexList.Contains(buildingBlockKey.BuildingBlockIndex))
			{
				BuildingEarningsData buildingEarningData = GetBuildingEarningData(buildingBlockKey);
				if (buildingBlockKey.BuildingBlockIndex != orgiBlockKey.BuildingBlockIndex && element_BuildingBlocks.TemplateId == orgiBlockData.TemplateId && GetShopSoldItemCount(buildingEarningData) + GetShopSoldEarnCount(buildingEarningData) < SharedMethods.GetBuildingSlotCount(element_BuildingBlocks.TemplateId))
				{
					int num3 = GetBuildingAttainmentUniversalWhetherCanWork(element_BuildingBlocks, buildingBlockKey) * SharedMethods.GetBuildingSlotCount(orgiBlockData.TemplateId);
					if (num3 > num)
					{
						num = num3;
						result = buildingBlockKey;
					}
				}
			}
		}
		return result;
	}

	[DomainMethod]
	public bool NearDependBuildings(BuildingAreaData areaData, Location location, short rootIndex, BuildingBlockItem configData)
	{
		List<short> dependBuildings = configData.DependBuildings;
		if (dependBuildings.Count == 0)
		{
			return true;
		}
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		areaData.GetNeighborBlocks(rootIndex, configData.Width, list, null, 2);
		bool[] array = new bool[dependBuildings.Count];
		for (int i = 0; i < list.Count; i++)
		{
			BuildingBlockKey elementId = new BuildingBlockKey(location.AreaId, location.BlockId, list[i]);
			if (!TryGetElement_BuildingBlocks(elementId, out var value))
			{
				continue;
			}
			int num = dependBuildings.IndexOf(value.TemplateId);
			int num2 = -1;
			if (value.RootBlockIndex >= 0)
			{
				BuildingBlockKey elementId2 = new BuildingBlockKey(location.AreaId, location.BlockId, value.RootBlockIndex);
				if (TryGetElement_BuildingBlocks(elementId2, out var value2))
				{
					num2 = dependBuildings.IndexOf(value2.TemplateId);
				}
			}
			if (num >= 0)
			{
				array[num] = true;
			}
			if (num2 >= 0)
			{
				array[num2] = true;
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
		return !array.Exist(value: false);
	}

	[DomainMethod]
	public void AddFixBook(DataContext context, BuildingBlockKey key, ItemKey itemKey, ItemSourceType itemSourceType)
	{
		if (!TryGetElement_CollectBuildingEarningsData(key, out var value))
		{
			value = new BuildingEarningsData();
			AddElement_CollectBuildingEarningsData(key, value, context);
		}
		DomainManager.Taiwu.RemoveItem(context, itemKey, 1, itemSourceType, deleteItem: false);
		AddBuildingEarningsDataFixBook(context, key, value, itemKey);
	}

	[DomainMethod]
	public (short, BuildingBlockData) ChangeFixBook(DataContext context, BuildingBlockKey key, ItemKey itemKey, ItemSourceType itemSourceType)
	{
		BuildingBlockData value = null;
		if (TryGetElement_BuildingBlocks(key, out value))
		{
			value.OfflineResetShopProgress();
			SetElement_BuildingBlocks(key, value, context);
		}
		if (TryGetElement_CollectBuildingEarningsData(key, out var value2))
		{
			if (value2.FixBookInfoList.Count <= 0)
			{
				return (key.BuildingBlockIndex, value);
			}
			ItemKey itemKey2 = value2.FixBookInfoList[0];
			RemoveBuildingEarningsDataFixBook(context, key, value2, itemKey2);
			DomainManager.Taiwu.AddItem(context, itemKey2, 1, itemSourceType);
			if (itemKey.IsValid())
			{
				DomainManager.Taiwu.RemoveItem(context, itemKey, 1, itemSourceType, deleteItem: false);
				AddBuildingEarningsDataFixBook(context, key, value2, itemKey);
			}
		}
		return (key.BuildingBlockIndex, value);
	}

	[DomainMethod]
	public void ReceiveFixBook(DataContext context, BuildingBlockKey key, bool isPutInInventory)
	{
		if (TryGetElement_CollectBuildingEarningsData(key, out var value) && value.FixBookInfoList.Count > 0)
		{
			ItemKey itemKey = value.FixBookInfoList[0];
			value.FixBookInfoList.Clear();
			RemoveBuildingEarningsDataFixBook(context, key, value, itemKey);
			if (isPutInInventory)
			{
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				taiwu.AddInventoryItem(context, itemKey, 1);
			}
			else
			{
				DomainManager.Taiwu.WarehouseAdd(context, itemKey, 1);
			}
		}
	}

	[DomainMethod]
	public int GetFixBookProgress(DataContext context, BuildingBlockKey key)
	{
		if (TryGetElement_BuildingBlocks(key, out var value) && TryGetElement_CollectBuildingEarningsData(key, out var value2) && value2.FixBookInfoList.Count > 0 && value2.FixBookInfoList[0].IsValid())
		{
			GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(value2.FixBookInfoList[0].Id);
			return value.ShopProgress * 100 / element_SkillBooks.GetFixProgress().needProgress;
		}
		return 0;
	}

	private void AddBuildingEarningsDataFixBook(DataContext context, BuildingBlockKey key, BuildingEarningsData earningsData, ItemKey itemKey)
	{
		earningsData.FixBookInfoList.Clear();
		earningsData.FixBookInfoList.Add(itemKey);
		SetElement_CollectBuildingEarningsData(key, earningsData, context);
		DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, 105);
	}

	private void RemoveBuildingEarningsDataFixBook(DataContext context, BuildingBlockKey key, BuildingEarningsData earningsData, ItemKey itemKey)
	{
		earningsData.FixBookInfoList.Clear();
		SetElement_CollectBuildingEarningsData(key, earningsData, context);
		DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Building, 105);
	}

	[DomainMethod]
	public List<ItemDisplayData> GetTaiwuCanFixBookItemDataList(ItemSourceType itemSourceType)
	{
		List<ItemDisplayData> item = DomainManager.Taiwu.GetAllItems(itemSourceType).list;
		item.RemoveAll(delegate(ItemDisplayData d)
		{
			short itemSubType = ItemTemplateHelper.GetItemSubType(d.Key.ItemType, d.Key.TemplateId);
			if ((uint)(itemSubType - 1000) <= 1u)
			{
				SkillBookPageDisplayData skillBookPagesInfo = DomainManager.Item.GetSkillBookPagesInfo(d.Key);
				return !skillBookPagesInfo.CanFix();
			}
			return true;
		});
		return item;
	}

	[DomainMethod]
	public void SetTeaHorseCaravanState(DataContext context, sbyte state)
	{
		if (_teaHorseCaravanData != null)
		{
			_teaHorseCaravanData.CaravanState = state;
			SetTeaHorseCaravanData(_teaHorseCaravanData, context);
		}
	}

	[DomainMethod]
	public void SetTeaHorseCaravanWeather(DataContext context, short weatherId)
	{
		if (_teaHorseCaravanData != null)
		{
			_teaHorseCaravanData.Weather = weatherId;
			SetTeaHorseCaravanData(_teaHorseCaravanData, context);
		}
	}

	[DomainMethod]
	public void GetBackTeaHorseCarryItem(DataContext context, ItemKey itemKey, sbyte itemSource)
	{
		TeaHorseCarryRemove(context, itemKey);
		switch (itemSource)
		{
		case 2:
			DomainManager.Taiwu.WarehouseAdd(context, itemKey, 1);
			break;
		case 3:
			DomainManager.Taiwu.StoreItemInTreasury(context, itemKey, 1);
			break;
		case 4:
			DomainManager.Extra.AddStockStorageItem(context, 1, itemKey, 1);
			break;
		default:
			DomainManager.Taiwu.GetTaiwu().AddInventoryItem(context, itemKey, 1);
			break;
		}
	}

	[DomainMethod]
	public void AddItemToTeaHorseCarryItem(DataContext context, ItemKey itemKey, sbyte itemSource)
	{
		if (_teaHorseCaravanData == null)
		{
			_teaHorseCaravanData = new TeaHorseCaravanData();
		}
		switch (itemSource)
		{
		case 2:
		{
			int warehouseItemCount = DomainManager.Taiwu.GetWarehouseItemCount(itemKey);
			if (warehouseItemCount > 0)
			{
				DomainManager.Taiwu.WarehouseRemove(context, itemKey, 1);
				TeaHorseCarryAdd(context, itemKey, 2);
			}
			return;
		}
		case 3:
		{
			int treasuryItemCount = DomainManager.Taiwu.GetTreasuryItemCount(itemKey);
			if (treasuryItemCount > 0)
			{
				DomainManager.Taiwu.RemoveTaiwuItemFromTaiwuStorage(context, itemKey, 1, deleteItem: false);
				TeaHorseCarryAdd(context, itemKey, 3);
			}
			return;
		}
		case 4:
		{
			int stockStorageGoosShelfItemCount = DomainManager.Extra.GetStockStorageGoosShelfItemCount(itemKey);
			if (stockStorageGoosShelfItemCount > 0)
			{
				DomainManager.Extra.RemoveStockStorageItem(context, 1, itemKey, 1);
				TeaHorseCarryAdd(context, itemKey, 4);
			}
			return;
		}
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		ItemKey[] equipment = taiwu.GetEquipment();
		int num = equipment.IndexOf(itemKey);
		if (num >= 0)
		{
			taiwu.ChangeEquipment(context, (sbyte)num, -1, ItemKey.Invalid);
		}
		int inventoryItemCount = taiwu.GetInventory().GetInventoryItemCount(itemKey);
		if (inventoryItemCount > 0)
		{
			taiwu.RemoveInventoryItem(context, itemKey, 1, deleteItem: false);
			TeaHorseCarryAdd(context, itemKey, 1);
		}
	}

	private void TeaHorseCarryAdd(DataContext context, ItemKey itemKey, sbyte from)
	{
		_teaHorseCaravanData.CarryGoodsList.Add((itemKey, from));
		SetTeaHorseCaravanData(_teaHorseCaravanData, context);
		DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, 51);
	}

	private void TeaHorseCarryRemove(DataContext context, ItemKey itemKey)
	{
		for (int i = 0; i < _teaHorseCaravanData.CarryGoodsList.Count; i++)
		{
			if (_teaHorseCaravanData.CarryGoodsList[i].Item1.Equals(itemKey))
			{
				_teaHorseCaravanData.CarryGoodsList.RemoveAt(i);
				SetTeaHorseCaravanData(_teaHorseCaravanData, context);
				DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Building, 51);
				break;
			}
		}
	}

	[DomainMethod]
	public void ExchangeItemToReplenishment(DataContext context, List<ItemKey> carryItems, List<ItemKey> gainItems)
	{
		short num = 0;
		if (carryItems != null)
		{
			foreach (ItemKey carryItem in carryItems)
			{
				foreach (var carryGoods in _teaHorseCaravanData.CarryGoodsList)
				{
					var (itemKey, _) = carryGoods;
					if (itemKey.Equals(carryItem))
					{
						_teaHorseCaravanData.CarryGoodsList.Remove(carryGoods);
						DomainManager.Item.RemoveItem(context, carryItem);
						num += GradeToReplenishment(ItemTemplateHelper.GetGrade(carryItem.ItemType, carryItem.TemplateId));
						break;
					}
				}
			}
		}
		if (gainItems != null)
		{
			foreach (ItemKey gainItem in gainItems)
			{
				if (_teaHorseCaravanData.ExchangeGoodsList.Contains(gainItem))
				{
					_teaHorseCaravanData.ExchangeGoodsList.Remove(gainItem);
					num += GradeToReplenishment(ItemTemplateHelper.GetGrade(gainItem.ItemType, gainItem.TemplateId));
				}
			}
		}
		num = (short)Math.Min(num, _caravanReplenishmentInitValue - _teaHorseCaravanData.CaravanReplenishment);
		_teaHorseCaravanData.CaravanReplenishment += Math.Min(num, _teaHorseCaravanData.ExchangeReplenishmentRemainAmount);
		_teaHorseCaravanData.CaravanReplenishment = Math.Min(_teaHorseCaravanData.CaravanReplenishment, _caravanReplenishmentInitValue);
		_teaHorseCaravanData.ExchangeReplenishmentRemainAmount = (short)Math.Max(0, _teaHorseCaravanData.ExchangeReplenishmentRemainAmount - num);
		SetTeaHorseCaravanData(_teaHorseCaravanData, context);
	}

	private short GradeToReplenishment(sbyte grade)
	{
		return (short)((grade + 1) * 5 + 5);
	}

	private void ResetTeaHorseCaravanData(DataContext context)
	{
		_teaHorseCaravanData.DiaryList = new List<short>();
		_teaHorseCaravanData.CaravanState = 0;
		_teaHorseCaravanData.IsStartSearch = false;
		_teaHorseCaravanData.CaravanAwareness = _caravanAwarenessInitValue;
		_teaHorseCaravanData.CaravanReplenishment = _caravanReplenishmentInitValue;
		_teaHorseCaravanData.LackReplenishmentTurn = 0;
		_teaHorseCaravanData.IsShowExchangeReplenishment = false;
		_teaHorseCaravanData.IsShowSeachReplenishment = false;
		_teaHorseCaravanData.DistanceToTaiwuVillage = 0;
		_teaHorseCaravanData.StartMonth = 0;
		_teaHorseCaravanData.ExchangeReplenishmentAmountMax = 0;
		_teaHorseCaravanData.SearchReplenishmentMax = 0;
		_teaHorseCaravanData.SearchReplenishmentAmount = 0;
		_teaHorseCaravanData.Terrain = 5;
		SetTeaHorseCaravanData(_teaHorseCaravanData, context);
	}

	public ItemKey GetWestRandomItemByGarde(DataContext context, sbyte grade)
	{
		short templateId = WestItemArrayTwo[context.Random.Next(0, WestItemArrayTwo.Length)][grade - 4];
		sbyte type = ItemSubType.GetType(1203);
		return new ItemKey(type, 0, templateId, -1);
	}

	[DomainMethod]
	public void StartSearchReplenishment(DataContext context)
	{
		_teaHorseCaravanData.IsStartSearch = true;
		_teaHorseCaravanData.CaravanReplenishment = (short)Math.Min(_teaHorseCaravanData.CaravanReplenishment + _teaHorseCaravanData.SearchReplenishmentAmount, _caravanReplenishmentInitValue);
		_teaHorseCaravanData.SearchReplenishmentMax = (short)Math.Max(0, _teaHorseCaravanData.SearchReplenishmentMax - _teaHorseCaravanData.SearchReplenishmentAmount);
		if (_teaHorseCaravanData.CaravanReplenishment >= _caravanReplenishmentInitValue || _teaHorseCaravanData.SearchReplenishmentMax <= 0)
		{
			_teaHorseCaravanData.IsStartSearch = false;
			_teaHorseCaravanData.IsShowSeachReplenishment = false;
		}
		SetTeaHorseCaravanData(_teaHorseCaravanData, context);
	}

	[DomainMethod]
	public void QuickGetExchangeItem(DataContext context)
	{
		if (_teaHorseCaravanData != null)
		{
			for (int i = 0; i < _teaHorseCaravanData.ExchangeGoodsList.Count; i++)
			{
				ItemKey itemKey = _teaHorseCaravanData.ExchangeGoodsList[i];
				DomainManager.Taiwu.CreateWarehouseItem(context, itemKey.ItemType, itemKey.TemplateId, 1);
			}
			_teaHorseCaravanData.ExchangeGoodsList.Clear();
			SetTeaHorseCaravanData(_teaHorseCaravanData, context);
			ResetTeaHorseCaravanData(context);
		}
	}

	[DomainMethod]
	public void DealInfectedPeople(DataContext context, List<int> charList, byte dealType)
	{
		if (charList == null)
		{
			return;
		}
		OrganizationInfo organizationInfo = DomainManager.Taiwu.GetTaiwu().GetOrganizationInfo();
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		OrganizationInfo destOrgInfo = new OrganizationInfo(organizationInfo.OrgTemplateId, 0, principal: true, organizationInfo.SettlementId);
		int num = 0;
		int num2 = 0;
		ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[38];
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < charList.Count; i++)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charList[i]);
			Location location = element_Objects.GetLocation();
			int spiritualDebtChangeByInfected = DomainManager.Character.GetSpiritualDebtChangeByInfected(element_Objects, 50);
			int num5 = (HappinessType.Ranges[3].min + HappinessType.Ranges[3].max) / 2;
			switch (dealType)
			{
			case 1:
				DomainManager.Extra.ChangeAreaSpiritualDebt(context, location.AreaId, spiritualDebtChangeByInfected);
				DomainManager.Character.GroupMove(context, element_Objects, taiwuVillageLocation);
				DomainManager.Character.RemoveXiangshuInfection(context, element_Objects, 0);
				DomainManager.Organization.ChangeOrganization(context, element_Objects, destOrgInfo);
				element_Objects.SetHappiness((sbyte)num5, context);
				num += ProfessionRelatedConstants.TaoistMonkSkill2GetSecretSignCountBySave[element_Objects.GetConsummateLevel()];
				num2 += formulaCfg.Calculate(element_Objects.GetOrganizationInfo().Grade);
				element_Objects.SavedFromInfected(context, batchMode: true);
				num3 += element_Objects.SaveFromInfectedGainFaith;
				num4++;
				break;
			case 2:
				DomainManager.Extra.ChangeAreaSpiritualDebt(context, location.AreaId, spiritualDebtChangeByInfected);
				DomainManager.Character.RemoveXiangshuInfection(context, element_Objects, 0);
				element_Objects.SetHappiness((sbyte)num5, context);
				num += ProfessionRelatedConstants.TaoistMonkSkill2GetSecretSignCountBySave[element_Objects.GetConsummateLevel()];
				num2 += formulaCfg.Calculate(element_Objects.GetOrganizationInfo().Grade);
				element_Objects.SavedFromInfected(context, batchMode: true);
				num3 += element_Objects.SaveFromInfectedGainFaith;
				num4++;
				break;
			case 3:
			{
				DomainManager.Extra.ChangeAreaSpiritualDebt(context, location.AreaId, -spiritualDebtChangeByInfected);
				List<Location> list = ObjectPool<List<Location>>.Instance.Get();
				DomainManager.Map.GetAreaNotSettlementLocations(list, location.AreaId);
				Location location2 = list[context.Random.Next(0, list.Count)];
				DomainManager.Map.OnInfectedCharacterLocationChanged(context, element_Objects.GetId(), element_Objects.GetLocation(), location2);
				DomainManager.Character.GroupMove(context, element_Objects, location2);
				ObjectPool<List<Location>>.Instance.Return(list);
				DomainManager.Extra.TryRemoveStoneRoomCharacter(context, element_Objects);
				break;
			}
			case 4:
				DomainManager.Extra.ChangeAreaSpiritualDebt(context, location.AreaId, -spiritualDebtChangeByInfected);
				DomainManager.Character.GroupMove(context, element_Objects, taiwuVillageLocation);
				DomainManager.Character.MakeCharacterDead(context, element_Objects, 9, new CharacterDeathInfo(element_Objects.GetValidLocation())
				{
					KillerId = DomainManager.Taiwu.GetTaiwuCharId()
				});
				num += ProfessionRelatedConstants.TaoistMonkSkill2GetSecretSignCount[element_Objects.GetConsummateLevel()];
				num2 += formulaCfg.Calculate(element_Objects.GetOrganizationInfo().Grade) / 2;
				break;
			}
		}
		if (num2 > 0)
		{
			DomainManager.Extra.ChangeProfessionSeniority(context, 5, num2);
		}
		if (num > 0 && DomainManager.Extra.IsProfessionalSkillUnlocked(5, 2))
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			ItemKey itemKey = DomainManager.Item.CreateItem(context, 12, 234);
			taiwu.AddInventoryItem(context, itemKey, num);
			DomainManager.TaiwuEvent.OnEvent_TaiwuGotTianjieFulu(-1, itemKey, num);
			DomainManager.World.GetInstantNotificationCollection().AddGetItem(taiwu.GetId(), 12, 234);
		}
		if (num3 > 0)
		{
			DomainManager.World.GetInstantNotificationCollection().AddGainFuyuFaith2(num4, num3);
		}
	}

	internal bool TryCalcShopManagementYieldAmount(IRandomSource random, BuildingBlockKey blockKey, out sbyte resourceType, out int amount, out BuildingProduceDependencyData dependencyData, int valuationInclination = 0)
	{
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		resourceType = -1;
		amount = 0;
		dependencyData = BuildingProduceDependencyData.Invalid;
		if (!TryGetElement_BuildingBlocks(blockKey, out var value) || !DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)blockKey, out var value2))
		{
			return false;
		}
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[value.TemplateId];
		if (buildingBlockItem.IsShop && buildingBlockItem.SuccesEvent.Count > 0)
		{
			ShopEventItem shopEventItem = Config.ShopEvent.Instance[buildingBlockItem.SuccesEvent[0]];
			switch (valuationInclination)
			{
			case 1:
				random = (IRandomSource)(object)new MaxRandomGenerator();
				break;
			case -1:
				random = (IRandomSource)(object)new MinRandomGenerator();
				break;
			}
			sbyte level = value2.CalcUnlockedLevelCount();
			if (shopEventItem.ResourceGoods != -1)
			{
				if (!HasShopManagerLeader(blockKey))
				{
					AddBuildingException(blockKey, value, BuildingExceptionType.ManageStoppedForNoLeader);
				}
				dependencyData.TemplateId = value.TemplateId;
				dependencyData.Level = level;
				dependencyData.ProductivityFactor = BuildingProductivityByMaxDependencies(blockKey);
				dependencyData.SafetyCultureFactor = CalcSafetyOrCultureFactor(buildingBlockItem);
				dependencyData.TotalAttainmentFactor = BuildingTotalAttainment(blockKey, -1, out var hasManager);
				dependencyData.GainResourcePercentFactor = DomainManager.World.GetGainResourcePercent(9);
				if (value.TemplateId == 215)
				{
					dependencyData.RandomFactorUpperLimit = GlobalConfig.Instance.BuildingOutputRandomFactorUpperLimit * 3;
					dependencyData.RandomFactorLowerLimit = GlobalConfig.Instance.BuildingOutputRandomFactorLowerLimit / 2;
					resourceType = shopEventItem.ResourceGoods;
					amount = BuildingRandomCorrection(dependencyData.GamblingHouseOutput, random);
					CValuePercentBonus val = CValuePercentBonus.op_Implicit(DomainManager.Building.GetBuildingBlockEffect(blockKey.GetLocation(), EBuildingScaleEffect.BuildingMoneyIncomeBonus));
					amount *= val;
					if (hasManager)
					{
						if (!DomainManager.Extra.TryGetElement_BuildingMoneyPrestigeSuccessRateCompensation((ulong)blockKey, out var value3))
						{
							value3 = 0;
						}
						if (1 == 0)
						{
						}
						bool flag = valuationInclination switch
						{
							1 => true, 
							-1 => false, 
							_ => random.CheckPercentProb(BuildingManageHarvestSpecialSuccessRate(blockKey, -1) + value3), 
						};
						if (1 == 0)
						{
						}
						if (flag)
						{
							amount *= 3;
						}
						else
						{
							amount /= 2;
						}
						return true;
					}
				}
				else if (value.TemplateId == 216)
				{
					dependencyData.RandomFactorUpperLimit = GlobalConfig.Instance.BuildingOutputRandomFactorUpperLimit * 3;
					dependencyData.RandomFactorLowerLimit = GlobalConfig.Instance.BuildingOutputRandomFactorLowerLimit / 2;
					resourceType = shopEventItem.ResourceGoods;
					amount = BuildingRandomCorrection(dependencyData.BrothelOutput, random);
					CValuePercentBonus val2 = CValuePercentBonus.op_Implicit(DomainManager.Building.GetBuildingBlockEffect(blockKey.GetLocation(), EBuildingScaleEffect.BuildingMoneyIncomeBonus));
					amount *= val2;
					if (hasManager)
					{
						if (!DomainManager.Extra.TryGetElement_BuildingMoneyPrestigeSuccessRateCompensation((ulong)blockKey, out var value4))
						{
							value4 = 0;
						}
						if (1 == 0)
						{
						}
						bool flag = valuationInclination switch
						{
							1 => true, 
							-1 => false, 
							_ => random.CheckPercentProb(BuildingManageHarvestSpecialSuccessRate(blockKey, -1) + value4), 
						};
						if (1 == 0)
						{
						}
						if (flag)
						{
							amount *= 3;
						}
						else
						{
							amount /= 2;
						}
						return true;
					}
				}
				else
				{
					resourceType = shopEventItem.ResourceGoods;
					dependencyData.RandomFactorUpperLimit = GlobalConfig.Instance.BuildingOutputRandomFactorUpperLimit;
					dependencyData.RandomFactorLowerLimit = GlobalConfig.Instance.BuildingOutputRandomFactorLowerLimit;
					amount = ((resourceType == 7) ? dependencyData.AuthorityBuildingOutput : dependencyData.MoneyBuildingOutput);
					CValuePercentBonus val3 = CValuePercentBonus.op_Implicit(DomainManager.Building.GetBuildingBlockEffect(blockKey.GetLocation(), (resourceType == 7) ? EBuildingScaleEffect.BuildingAuthorityIncomeBonus : EBuildingScaleEffect.BuildingMoneyIncomeBonus));
					amount *= val3;
					amount = BuildingRandomCorrection(amount, random);
					if (dependencyData.ProductivityFactor == 0)
					{
						AddBuildingException(blockKey, value, BuildingExceptionType.ManageStoppedForDependency);
					}
					if (hasManager)
					{
						return true;
					}
				}
			}
			return false;
		}
		return false;
	}

	[DomainMethod]
	public BuildingManageYieldTipsData GetShopManagementYieldTipsData(DataContext context, BuildingBlockKey blockKey)
	{
		BuildingManageYieldTipsData result = new BuildingManageYieldTipsData(0);
		if (TryGetElement_BuildingBlocks(blockKey, out var value))
		{
			BuildingBlockItem item = BuildingBlock.Instance.GetItem(value.TemplateId);
			sbyte needLifeSkillType = GetNeedLifeSkillType(item, blockKey);
			ShopEventItem shopEventItem = Config.ShopEvent.Instance[item.SuccesEvent[0]];
			int addition;
			List<SettlementDisplayData> list = SharedMethods.PickSafetyOrCultureFactorSettlements(item, DomainManager.Taiwu.GetAllVisitedSettlements(), out addition);
			if (list.Count > 0 && addition > 0)
			{
				bool flag = item.RequireCulture != 0;
				result.SafetyOrCultureFactorSettlementsAndPickValue = new Dictionary<int, SettlementDisplayData>();
				foreach (SettlementDisplayData item2 in list)
				{
					int num = SharedMethods.CalcSafetyOrCultureFactorSettlementPickValue(flag ? item.RequireCulture : item.RequireSafety, flag ? item2.Culture : item2.Safety);
					if (num > 0)
					{
						result.SafetyOrCultureFactorSettlementsAndPickValue.Add(item2.SettlementId, item2);
					}
				}
			}
			if (item.IsCollectResourceBuilding)
			{
				sbyte b = GetCollectBuildingResourceType(blockKey);
				if (b > 5)
				{
					b = 5;
				}
				result.ResourceOutputValuation = CalcResourceOutputCount(blockKey, b, result.ProduceDependencies);
				result.ProduceResourceType = b;
				result.ManagerAttainment = 0;
				if (TryGetElement_ShopManagerDict(blockKey, out var value2))
				{
					for (int i = 0; i < value2.GetCount(); i++)
					{
						int num2 = value2.GetCollection()[i];
						if (GameData.Domains.Character.Character.IsCharacterIdValid(num2) && DomainManager.Character.TryGetElement_Objects(num2, out var element))
						{
							result.ManagerAttainment += element.GetLifeSkillAttainment(needLifeSkillType);
						}
					}
				}
			}
			else if (SharedMethods.IsBuildingProduceMoneyAuthority(item, shopEventItem))
			{
				TryCalcShopManagementYieldAmount(context.Random, blockKey, out var resourceType, out result.ManageProduceValuationMin, out result.BuildingProduceDependencyData, -1);
				TryCalcShopManagementYieldAmount(context.Random, blockKey, out resourceType, out result.ManageProduceValuationMax, out result.BuildingProduceDependencyData, 1);
				result.ProduceResourceType = shopEventItem.ResourceGoods;
				if (result.ManageProduceValuationMin > result.ManageProduceValuationMax)
				{
					ref int manageProduceValuationMin = ref result.ManageProduceValuationMin;
					ref int manageProduceValuationMax = ref result.ManageProduceValuationMax;
					int manageProduceValuationMax2 = result.ManageProduceValuationMax;
					int manageProduceValuationMin2 = result.ManageProduceValuationMin;
					manageProduceValuationMin = manageProduceValuationMax2;
					manageProduceValuationMax = manageProduceValuationMin2;
				}
				result.ManagerAttainment = 0;
				if (TryGetElement_ShopManagerDict(blockKey, out var value3))
				{
					for (int j = 0; j < value3.GetCount(); j++)
					{
						int num3 = value3.GetCollection()[j];
						if (GameData.Domains.Character.Character.IsCharacterIdValid(num3) && DomainManager.Character.TryGetElement_Objects(num3, out var element2))
						{
							result.ManagerAttainment += element2.GetLifeSkillAttainment(needLifeSkillType);
						}
					}
				}
			}
			else if (SharedMethods.IsBuildingSoldItem(item, shopEventItem))
			{
				result.ManageProduceValuationMin = CalcSoldItemValueBySpecificBaseLine((IRandomSource)(object)new MinRandomGenerator(), blockKey, value, out result.BuildingProduceDependencyData, 100, out var manageProduceValuationMin2);
				result.ManageProduceValuationMax = CalcSoldItemValueBySpecificBaseLine((IRandomSource)(object)new MaxRandomGenerator(), blockKey, value, out result.BuildingProduceDependencyData, 100, out manageProduceValuationMin2);
			}
		}
		return result;
	}

	public void UpgradeTeaHorseCaravanByAwareness(DataContext context)
	{
		if (_teaHorseCaravanData == null)
		{
			return;
		}
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData buildingAreaData = DomainManager.Building.GetBuildingAreaData(taiwuVillageLocation);
		BuildingBlockKey buildingBlockKey = FindBuildingKey(taiwuVillageLocation, buildingAreaData, 51, checkUsable: false);
		if (!DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)buildingBlockKey, out var value))
		{
			return;
		}
		sbyte b = value.CalcUnlockedLevelCount();
		short[] teaHorseCaravanLevelToAwareness = GlobalConfig.Instance.TeaHorseCaravanLevelToAwareness;
		int num = -1;
		for (int num2 = teaHorseCaravanLevelToAwareness.Length - 1; num2 >= 0; num2--)
		{
			if (_teaHorseCaravanData.CaravanAwareness >= teaHorseCaravanLevelToAwareness[num2])
			{
				num = num2 + 1;
				break;
			}
		}
		if (num != -1 && num > b)
		{
			DomainManager.Extra.UpgradeToLevelUnchecked(context, buildingBlockKey, value, num);
		}
	}

	[DomainMethod]
	public TaiwuShrineDisplayData GetShrineDisplayData(DataContext context, short areaId, short blockId, short buildingBlockIndex)
	{
		BuildingBlockKey shrineBlockKey = new BuildingBlockKey(areaId, blockId, buildingBlockIndex);
		TaiwuShrineDisplayData taiwuShrineDisplayData = new TaiwuShrineDisplayData
		{
			Authority = CalculateGainAuthorityByShrinePerMonth(shrineBlockKey),
			CharIdList = GetTaiwuShrineStudentList()
		};
		taiwuShrineDisplayData.CharIdList.Insert(0, DomainManager.Taiwu.GetTaiwuCharId());
		return taiwuShrineDisplayData;
	}

	[DomainMethod]
	public void TeachSkill(DataContext context, int characterId, SkillQualificationBonus bonus)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(characterId);
		if (element_Objects == null)
		{
			throw new KeyNotFoundException($"Not found Character with charId:{characterId}");
		}
		List<SkillQualificationBonus> skillQualificationBonuses = element_Objects.GetSkillQualificationBonuses();
		if (skillQualificationBonuses.Count < 11)
		{
			skillQualificationBonuses.Add(bonus);
			sbyte item = bonus.GetSkillGroupAndType().skillGroup;
			if (item == 1 && !element_Objects.GetLearnedCombatSkills().Contains(bonus.SkillId))
			{
				DomainManager.Character.LearnCombatSkill(context, characterId, bonus.SkillId, 0);
				int baseDelta = ProfessionFormulaImpl.Calculate(52, Config.CombatSkill.Instance[bonus.SkillId].Grade);
				DomainManager.Extra.ChangeProfessionSeniority(context, 7, baseDelta);
			}
			else if (item == 0 && element_Objects.FindLearnedLifeSkillIndex(bonus.SkillId) < 0)
			{
				DomainManager.Character.LearnLifeSkill(context, characterId, bonus.SkillId, 0);
				int baseDelta2 = ProfessionFormulaImpl.Calculate(103, LifeSkill.Instance[bonus.SkillId].Grade);
				DomainManager.Extra.ChangeProfessionSeniority(context, 16, baseDelta2);
			}
			ConsumeResource(context, 7, _shrineBuyTimes * GlobalConfig.Instance.ShrineAuthorityPerTime);
			SetShrineBuyTimes((ushort)(_shrineBuyTimes + 1), context);
			element_Objects.SetSkillQualificationBonuses(skillQualificationBonuses, context);
		}
	}

	private List<int> GetTaiwuShrineStudentList()
	{
		List<int> list = new List<int>();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		Settlement settlement = DomainManager.Organization.GetSettlement(taiwuVillageSettlementId);
		OrgMemberCollection members = settlement.GetMembers();
		List<int> list2 = new List<int>();
		members.GetAllMembers(list2);
		foreach (int item in list2)
		{
			if (item != taiwuCharId && DomainManager.Character.TryGetElement_Objects(item, out var element) && element.GetAgeGroup() != 0 && !element.IsCompletelyInfected())
			{
				list.Add(item);
			}
		}
		HashSet<int> groupHashSet = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		list.Sort((int l, int r) => (!groupHashSet.Contains(l) || groupHashSet.Contains(r)) ? 1 : (-1));
		return list;
	}

	[Obsolete]
	private int GetAutoAddAuthorityValue(BuildingBlockKey key)
	{
		BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(key);
		return GetAutoAddAuthorityValue(element_BuildingBlocks);
	}

	[Obsolete]
	private int GetAutoAddAuthorityValue(BuildingBlockData buildingBlock)
	{
		float[] array = new float[5] { 0f, 0.25f, 0.5f, 0.75f, 1f };
		byte[] array2 = new byte[9] { 0, 1, 2, 4, 8, 16, 32, 64, 128 };
		sbyte[] array3 = new sbyte[7] { -15, -10, -5, 0, 5, 10, 15 };
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		sbyte fameType = FameType.GetFameType(taiwu.GetFameType());
		sbyte b = DomainManager.World.GetXiangshuLevel();
		if (!array2.CheckIndex(b))
		{
			b = (sbyte)(array2.Length - 1);
		}
		float num = (float)(int)array2[b] * array[fameType];
		int num2 = 100;
		HashSet<int> hashSet = new HashSet<int>();
		DomainManager.Character.GetAllRelatedCharIds(taiwu.GetId(), hashSet);
		foreach (int item in hashSet)
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element))
			{
				sbyte fameType2 = FameType.GetFameType(element.GetFameType());
				RelatedCharacter relation = DomainManager.Character.GetRelation(item, taiwu.GetId());
				ushort relationType = relation.RelationType;
				short favorability = relation.Favorability;
				if (favorability > 0 && RelationType.ContainPositiveRelations(relationType))
				{
					num2 += array3[fameType2];
				}
				else if (favorability < 0 && RelationType.ContainNegativeRelations(relationType))
				{
					num2 -= array3[fameType2];
				}
			}
		}
		return Math.Max(0, (int)(num * (float)num2 / 100f));
	}

	[Obsolete("Instead by CalculateGainAuthorityByShrinePerMonth")]
	private void AddAuthorityOnSerialUpdate(DataContext context)
	{
		sbyte currMonthInYear = DomainManager.World.GetCurrMonthInYear();
		if (currMonthInYear == GlobalConfig.Instance.ShrineAuthorityAddMonth)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Location location = _taiwuBuildingAreas[0];
			BuildingAreaData buildingAreaData = GetBuildingAreaData(location);
			short buildingTemplateId = 45;
			BuildingBlockData buildingBlockData = FindBuilding(location, buildingAreaData, buildingTemplateId);
			if (buildingBlockData != null)
			{
				int autoAddAuthorityValue = GetAutoAddAuthorityValue(buildingBlockData);
				taiwu.ChangeResource(context, 7, autoAddAuthorityValue);
			}
		}
	}

	[Obsolete]
	public void SpiritualDebtInteractionEmei(DataContext context)
	{
		List<Location> taiwuBuildingAreas = GetTaiwuBuildingAreas();
		for (int i = 0; i < taiwuBuildingAreas.Count; i++)
		{
			Location elementId = taiwuBuildingAreas[i];
			BuildingAreaData element_BuildingAreas = GetElement_BuildingAreas(elementId);
			short num = 0;
			short num2 = (short)(element_BuildingAreas.Width * element_BuildingAreas.Width);
			while (num < num2)
			{
				BuildingBlockKey buildingBlockKey = new BuildingBlockKey(elementId.AreaId, elementId.BlockId, num);
				BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(buildingBlockKey);
				if (element_BuildingBlocks.RootBlockIndex < 0)
				{
					BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
					if (BuildingBlockData.IsResource(buildingBlockItem.Type))
					{
						sbyte resourceBlockGrowthChance = GetResourceBlockGrowthChance(buildingBlockKey);
						if (resourceBlockGrowthChance > 0 && DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)buildingBlockKey, out var value))
						{
							sbyte b = value.CalcUnlockedLevelCount();
							sbyte b2 = b;
							sbyte b3 = (sbyte)Math.Min(b + context.Random.Next(1, 4), buildingBlockItem.MaxLevel);
							if (b2 != b3)
							{
								SetElement_BuildingBlocks(buildingBlockKey, element_BuildingBlocks, context);
							}
						}
					}
				}
				num++;
			}
		}
	}

	public int GetVillagerRoleCapacity(short roleTemplateId)
	{
		return VillagerRole.Instance[roleTemplateId].MaxCount;
	}

	[Obsolete]
	private void UpdateVillagerRoleCapacities()
	{
	}

	public void GetVillagerRoleCapacitiesDetail(short roleTemplateId, ref List<IntPair> detailList)
	{
		List<Location> taiwuBuildingAreas = GetTaiwuBuildingAreas();
		for (int i = 0; i < taiwuBuildingAreas.Count; i++)
		{
			Location elementId = taiwuBuildingAreas[i];
			BuildingAreaData element_BuildingAreas = GetElement_BuildingAreas(elementId);
			short num = 0;
			short num2 = (short)(element_BuildingAreas.Width * element_BuildingAreas.Width);
			while (num < num2)
			{
				BuildingBlockKey buildingBlockKey = new BuildingBlockKey(elementId.AreaId, elementId.BlockId, num);
				BuildingBlockData element_BuildingBlocks = GetElement_BuildingBlocks(buildingBlockKey);
				if (DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)buildingBlockKey, out var value))
				{
					sbyte b = value.CalcUnlockedLevelCount();
					if (element_BuildingBlocks.RootBlockIndex < 0 && b > 0)
					{
						BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
						if (buildingBlockItem.ExpandInfos != null && buildingBlockItem.VillagerRoleTemplateIds != null && buildingBlockItem.VillagerRoleTemplateIds.Exist(roleTemplateId))
						{
							foreach (short expandInfo in buildingBlockItem.ExpandInfos)
							{
								BuildingScaleItem buildingScaleItem = BuildingScale.Instance[expandInfo];
								detailList.Add(new IntPair(element_BuildingBlocks.TemplateId, buildingScaleItem.LevelEffect.GetOrLast(b - 1)));
							}
						}
					}
				}
				num++;
			}
		}
	}

	public BuildingDomain()
		: base(25)
	{
		_buildingAreas = new Dictionary<Location, BuildingAreaData>(0);
		_buildingBlocks = new Dictionary<BuildingBlockKey, BuildingBlockData>(0);
		_taiwuBuildingAreas = new List<Location>();
		_CollectBuildingResourceType = new Dictionary<BuildingBlockKey, sbyte>(0);
		_buildingOperatorDict = new Dictionary<BuildingBlockKey, CharacterList>(0);
		_customBuildingName = new Dictionary<BuildingBlockKey, int>(0);
		_newCompleteOperationBuildings = new List<BuildingBlockKey>();
		_chickenBlessingInfoData = new Dictionary<int, ChickenBlessingInfoData>(0);
		_chicken = new Dictionary<int, Chicken>(0);
		_collectionCrickets = new ItemKey[15];
		_collectionCricketJars = new ItemKey[15];
		_collectionCricketRegen = new int[15];
		_makeItemDict = new Dictionary<BuildingBlockKey, MakeItemData>(0);
		_residences = new Dictionary<BuildingBlockKey, CharacterList>(0);
		_comfortableHouses = new Dictionary<BuildingBlockKey, CharacterList>(0);
		_homeless = default(CharacterList);
		_samsaraPlatformAddMainAttributes = default(MainAttributes);
		_samsaraPlatformAddCombatSkillQualifications = default(CombatSkillShorts);
		_samsaraPlatformAddLifeSkillQualifications = default(LifeSkillShorts);
		_samsaraPlatformSlots = new IntPair[6];
		_samsaraPlatformBornDict = new Dictionary<int, IntPair>(0);
		_collectBuildingEarningsData = new Dictionary<BuildingBlockKey, BuildingEarningsData>(0);
		_shopManagerDict = new Dictionary<BuildingBlockKey, CharacterList>(0);
		_teaHorseCaravanData = new TeaHorseCaravanData();
		_shrineBuyTimes = 0;
		OnInitializedDomainData();
	}

	public BuildingAreaData GetElement_BuildingAreas(Location elementId)
	{
		return _buildingAreas[elementId];
	}

	public bool TryGetElement_BuildingAreas(Location elementId, out BuildingAreaData value)
	{
		return _buildingAreas.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_BuildingAreas(Location elementId, BuildingAreaData value, DataContext context)
	{
		_buildingAreas.Add(elementId, value);
		_modificationsBuildingAreas.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(9, 0, elementId, 2);
		ptr += value.Serialize(ptr);
	}

	private unsafe void SetElement_BuildingAreas(Location elementId, BuildingAreaData value, DataContext context)
	{
		_buildingAreas[elementId] = value;
		_modificationsBuildingAreas.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(9, 0, elementId, 2);
		ptr += value.Serialize(ptr);
	}

	private void RemoveElement_BuildingAreas(Location elementId, DataContext context)
	{
		_buildingAreas.Remove(elementId);
		_modificationsBuildingAreas.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(9, 0, elementId);
	}

	private void ClearBuildingAreas(DataContext context)
	{
		_buildingAreas.Clear();
		_modificationsBuildingAreas.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(9, 0);
	}

	public BuildingBlockData GetElement_BuildingBlocks(BuildingBlockKey elementId)
	{
		return _buildingBlocks[elementId];
	}

	public bool TryGetElement_BuildingBlocks(BuildingBlockKey elementId, out BuildingBlockData value)
	{
		return _buildingBlocks.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_BuildingBlocks(BuildingBlockKey elementId, BuildingBlockData value, DataContext context)
	{
		_buildingBlocks.Add(elementId, value);
		_modificationsBuildingBlocks.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(9, 1, elementId, 16);
		ptr += value.Serialize(ptr);
	}

	private unsafe void SetElement_BuildingBlocks(BuildingBlockKey elementId, BuildingBlockData value, DataContext context)
	{
		_buildingBlocks[elementId] = value;
		_modificationsBuildingBlocks.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(9, 1, elementId, 16);
		ptr += value.Serialize(ptr);
	}

	private void RemoveElement_BuildingBlocks(BuildingBlockKey elementId, DataContext context)
	{
		_buildingBlocks.Remove(elementId);
		_modificationsBuildingBlocks.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(9, 1, elementId);
	}

	private void ClearBuildingBlocks(DataContext context)
	{
		_buildingBlocks.Clear();
		_modificationsBuildingBlocks.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(9, 1);
	}

	public List<Location> GetTaiwuBuildingAreas()
	{
		return _taiwuBuildingAreas;
	}

	private unsafe void SetTaiwuBuildingAreas(List<Location> value, DataContext context)
	{
		_taiwuBuildingAreas = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, DataStates, CacheInfluences, context);
		int count = _taiwuBuildingAreas.Count;
		int num = 4 * count;
		int valueSize = 2 + num;
		byte* ptr = OperationAdder.DynamicSingleValue_Set(9, 2, valueSize);
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int i = 0; i < count; i++)
		{
			ptr += _taiwuBuildingAreas[i].Serialize(ptr);
		}
	}

	public sbyte GetElement_CollectBuildingResourceType(BuildingBlockKey elementId)
	{
		return _CollectBuildingResourceType[elementId];
	}

	public bool TryGetElement_CollectBuildingResourceType(BuildingBlockKey elementId, out sbyte value)
	{
		return _CollectBuildingResourceType.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_CollectBuildingResourceType(BuildingBlockKey elementId, sbyte value, DataContext context)
	{
		_CollectBuildingResourceType.Add(elementId, value);
		_modificationsCollectBuildingResourceType.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(9, 3, elementId, 1);
		*ptr = (byte)value;
		ptr++;
	}

	private unsafe void SetElement_CollectBuildingResourceType(BuildingBlockKey elementId, sbyte value, DataContext context)
	{
		_CollectBuildingResourceType[elementId] = value;
		_modificationsCollectBuildingResourceType.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(9, 3, elementId, 1);
		*ptr = (byte)value;
		ptr++;
	}

	private void RemoveElement_CollectBuildingResourceType(BuildingBlockKey elementId, DataContext context)
	{
		_CollectBuildingResourceType.Remove(elementId);
		_modificationsCollectBuildingResourceType.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(9, 3, elementId);
	}

	private void ClearCollectBuildingResourceType(DataContext context)
	{
		_CollectBuildingResourceType.Clear();
		_modificationsCollectBuildingResourceType.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(9, 3);
	}

	public CharacterList GetElement_BuildingOperatorDict(BuildingBlockKey elementId)
	{
		return _buildingOperatorDict[elementId];
	}

	public bool TryGetElement_BuildingOperatorDict(BuildingBlockKey elementId, out CharacterList value)
	{
		return _buildingOperatorDict.TryGetValue(elementId, out value);
	}

	private void AddElement_BuildingOperatorDict(BuildingBlockKey elementId, CharacterList value, DataContext context)
	{
		_buildingOperatorDict.Add(elementId, value);
		_modificationsBuildingOperatorDict.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
	}

	private void SetElement_BuildingOperatorDict(BuildingBlockKey elementId, CharacterList value, DataContext context)
	{
		_buildingOperatorDict[elementId] = value;
		_modificationsBuildingOperatorDict.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
	}

	private void RemoveElement_BuildingOperatorDict(BuildingBlockKey elementId, DataContext context)
	{
		_buildingOperatorDict.Remove(elementId);
		_modificationsBuildingOperatorDict.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
	}

	private void ClearBuildingOperatorDict(DataContext context)
	{
		_buildingOperatorDict.Clear();
		_modificationsBuildingOperatorDict.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
	}

	public int GetElement_CustomBuildingName(BuildingBlockKey elementId)
	{
		return _customBuildingName[elementId];
	}

	public bool TryGetElement_CustomBuildingName(BuildingBlockKey elementId, out int value)
	{
		return _customBuildingName.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_CustomBuildingName(BuildingBlockKey elementId, int value, DataContext context)
	{
		_customBuildingName.Add(elementId, value);
		_modificationsCustomBuildingName.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(9, 5, elementId, 4);
		*(int*)ptr = value;
		ptr += 4;
	}

	private unsafe void SetElement_CustomBuildingName(BuildingBlockKey elementId, int value, DataContext context)
	{
		_customBuildingName[elementId] = value;
		_modificationsCustomBuildingName.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(9, 5, elementId, 4);
		*(int*)ptr = value;
		ptr += 4;
	}

	private void RemoveElement_CustomBuildingName(BuildingBlockKey elementId, DataContext context)
	{
		_customBuildingName.Remove(elementId);
		_modificationsCustomBuildingName.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(9, 5, elementId);
	}

	private void ClearCustomBuildingName(DataContext context)
	{
		_customBuildingName.Clear();
		_modificationsCustomBuildingName.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(9, 5);
	}

	private List<BuildingBlockKey> GetNewCompleteOperationBuildings()
	{
		return _newCompleteOperationBuildings;
	}

	private unsafe void SetNewCompleteOperationBuildings(List<BuildingBlockKey> value, DataContext context)
	{
		_newCompleteOperationBuildings = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
		int count = _newCompleteOperationBuildings.Count;
		int num = 8 * count;
		int valueSize = 2 + num;
		byte* ptr = OperationAdder.DynamicSingleValue_Set(9, 6, valueSize);
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int i = 0; i < count; i++)
		{
			ptr += _newCompleteOperationBuildings[i].Serialize(ptr);
		}
	}

	[Obsolete("DomainData _chickenBlessingInfoData is no longer in use.")]
	public ChickenBlessingInfoData GetElement_ChickenBlessingInfoData(int elementId)
	{
		return _chickenBlessingInfoData[elementId];
	}

	[Obsolete("DomainData _chickenBlessingInfoData is no longer in use.")]
	public bool TryGetElement_ChickenBlessingInfoData(int elementId, out ChickenBlessingInfoData value)
	{
		return _chickenBlessingInfoData.TryGetValue(elementId, out value);
	}

	[Obsolete("DomainData _chickenBlessingInfoData is no longer in use.")]
	private unsafe void AddElement_ChickenBlessingInfoData(int elementId, ChickenBlessingInfoData value, DataContext context)
	{
		_chickenBlessingInfoData.Add(elementId, value);
		_modificationsChickenBlessingInfoData.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
		int serializedSize = value.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(9, 7, elementId, serializedSize);
		ptr += value.Serialize(ptr);
	}

	[Obsolete("DomainData _chickenBlessingInfoData is no longer in use.")]
	private unsafe void SetElement_ChickenBlessingInfoData(int elementId, ChickenBlessingInfoData value, DataContext context)
	{
		_chickenBlessingInfoData[elementId] = value;
		_modificationsChickenBlessingInfoData.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
		int serializedSize = value.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(9, 7, elementId, serializedSize);
		ptr += value.Serialize(ptr);
	}

	[Obsolete("DomainData _chickenBlessingInfoData is no longer in use.")]
	private void RemoveElement_ChickenBlessingInfoData(int elementId, DataContext context)
	{
		_chickenBlessingInfoData.Remove(elementId);
		_modificationsChickenBlessingInfoData.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(9, 7, elementId);
	}

	[Obsolete("DomainData _chickenBlessingInfoData is no longer in use.")]
	private void ClearChickenBlessingInfoData(DataContext context)
	{
		_chickenBlessingInfoData.Clear();
		_modificationsChickenBlessingInfoData.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(9, 7);
	}

	public Chicken GetElement_Chicken(int elementId)
	{
		return _chicken[elementId];
	}

	public bool TryGetElement_Chicken(int elementId, out Chicken value)
	{
		return _chicken.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_Chicken(int elementId, Chicken value, DataContext context)
	{
		_chicken.Add(elementId, value);
		_modificationsChicken.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(9, 8, elementId, 12);
		ptr += value.Serialize(ptr);
	}

	private unsafe void SetElement_Chicken(int elementId, Chicken value, DataContext context)
	{
		_chicken[elementId] = value;
		_modificationsChicken.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(9, 8, elementId, 12);
		ptr += value.Serialize(ptr);
	}

	private void RemoveElement_Chicken(int elementId, DataContext context)
	{
		_chicken.Remove(elementId);
		_modificationsChicken.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(9, 8, elementId);
	}

	private void ClearChicken(DataContext context)
	{
		_chicken.Clear();
		_modificationsChicken.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(9, 8);
	}

	[Obsolete("DomainData _collectionCrickets is no longer in use.")]
	public ItemKey[] GetCollectionCrickets()
	{
		return _collectionCrickets;
	}

	[Obsolete("DomainData _collectionCrickets is no longer in use.")]
	public unsafe void SetCollectionCrickets(ItemKey[] value, DataContext context)
	{
		_collectionCrickets = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(9, 9, 120);
		for (int i = 0; i < 15; i++)
		{
			ptr += _collectionCrickets[i].Serialize(ptr);
		}
	}

	[Obsolete("DomainData _collectionCricketJars is no longer in use.")]
	public ItemKey[] GetCollectionCricketJars()
	{
		return _collectionCricketJars;
	}

	[Obsolete("DomainData _collectionCricketJars is no longer in use.")]
	public unsafe void SetCollectionCricketJars(ItemKey[] value, DataContext context)
	{
		_collectionCricketJars = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(9, 10, 120);
		for (int i = 0; i < 15; i++)
		{
			ptr += _collectionCricketJars[i].Serialize(ptr);
		}
	}

	[Obsolete("DomainData _collectionCricketRegen is no longer in use.")]
	public int[] GetCollectionCricketRegen()
	{
		return _collectionCricketRegen;
	}

	[Obsolete("DomainData _collectionCricketRegen is no longer in use.")]
	public unsafe void SetCollectionCricketRegen(int[] value, DataContext context)
	{
		_collectionCricketRegen = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(9, 11, 60);
		for (int i = 0; i < 15; i++)
		{
			((int*)ptr)[i] = _collectionCricketRegen[i];
		}
		ptr += 60;
	}

	private MakeItemData GetElement_MakeItemDict(BuildingBlockKey elementId)
	{
		return _makeItemDict[elementId];
	}

	private bool TryGetElement_MakeItemDict(BuildingBlockKey elementId, out MakeItemData value)
	{
		return _makeItemDict.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_MakeItemDict(BuildingBlockKey elementId, MakeItemData value, DataContext context)
	{
		_makeItemDict.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(9, 12, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(9, 12, elementId, 0);
		}
	}

	private unsafe void SetElement_MakeItemDict(BuildingBlockKey elementId, MakeItemData value, DataContext context)
	{
		_makeItemDict[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(9, 12, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(9, 12, elementId, 0);
		}
	}

	private void RemoveElement_MakeItemDict(BuildingBlockKey elementId, DataContext context)
	{
		_makeItemDict.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(9, 12, elementId);
	}

	private void ClearMakeItemDict(DataContext context)
	{
		_makeItemDict.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(9, 12);
	}

	private CharacterList GetElement_Residences(BuildingBlockKey elementId)
	{
		return _residences[elementId];
	}

	private bool TryGetElement_Residences(BuildingBlockKey elementId, out CharacterList value)
	{
		return _residences.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_Residences(BuildingBlockKey elementId, CharacterList value, DataContext context)
	{
		_residences.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, DataStates, CacheInfluences, context);
		int serializedSize = value.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(9, 13, elementId, serializedSize);
		ptr += value.Serialize(ptr);
	}

	private unsafe void SetElement_Residences(BuildingBlockKey elementId, CharacterList value, DataContext context)
	{
		_residences[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, DataStates, CacheInfluences, context);
		int serializedSize = value.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(9, 13, elementId, serializedSize);
		ptr += value.Serialize(ptr);
	}

	private void RemoveElement_Residences(BuildingBlockKey elementId, DataContext context)
	{
		_residences.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(9, 13, elementId);
	}

	private void ClearResidences(DataContext context)
	{
		_residences.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(9, 13);
	}

	private CharacterList GetElement_ComfortableHouses(BuildingBlockKey elementId)
	{
		return _comfortableHouses[elementId];
	}

	private bool TryGetElement_ComfortableHouses(BuildingBlockKey elementId, out CharacterList value)
	{
		return _comfortableHouses.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_ComfortableHouses(BuildingBlockKey elementId, CharacterList value, DataContext context)
	{
		_comfortableHouses.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
		int serializedSize = value.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(9, 14, elementId, serializedSize);
		ptr += value.Serialize(ptr);
	}

	private unsafe void SetElement_ComfortableHouses(BuildingBlockKey elementId, CharacterList value, DataContext context)
	{
		_comfortableHouses[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
		int serializedSize = value.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(9, 14, elementId, serializedSize);
		ptr += value.Serialize(ptr);
	}

	private void RemoveElement_ComfortableHouses(BuildingBlockKey elementId, DataContext context)
	{
		_comfortableHouses.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(9, 14, elementId);
	}

	private void ClearComfortableHouses(DataContext context)
	{
		_comfortableHouses.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(9, 14);
	}

	public CharacterList GetHomeless()
	{
		return _homeless;
	}

	public unsafe void SetHomeless(CharacterList value, DataContext context)
	{
		_homeless = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, DataStates, CacheInfluences, context);
		int serializedSize = _homeless.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValue_Set(9, 15, serializedSize);
		ptr += _homeless.Serialize(ptr);
	}

	public MainAttributes GetSamsaraPlatformAddMainAttributes()
	{
		return _samsaraPlatformAddMainAttributes;
	}

	private unsafe void SetSamsaraPlatformAddMainAttributes(MainAttributes value, DataContext context)
	{
		_samsaraPlatformAddMainAttributes = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(9, 16, 12);
		ptr += _samsaraPlatformAddMainAttributes.Serialize(ptr);
	}

	public ref CombatSkillShorts GetSamsaraPlatformAddCombatSkillQualifications()
	{
		return ref _samsaraPlatformAddCombatSkillQualifications;
	}

	private unsafe void SetSamsaraPlatformAddCombatSkillQualifications(ref CombatSkillShorts value, DataContext context)
	{
		_samsaraPlatformAddCombatSkillQualifications = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(9, 17, 28);
		ptr += _samsaraPlatformAddCombatSkillQualifications.Serialize(ptr);
	}

	public ref LifeSkillShorts GetSamsaraPlatformAddLifeSkillQualifications()
	{
		return ref _samsaraPlatformAddLifeSkillQualifications;
	}

	private unsafe void SetSamsaraPlatformAddLifeSkillQualifications(ref LifeSkillShorts value, DataContext context)
	{
		_samsaraPlatformAddLifeSkillQualifications = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(9, 18, 32);
		ptr += _samsaraPlatformAddLifeSkillQualifications.Serialize(ptr);
	}

	public IntPair GetElement_SamsaraPlatformSlots(int index)
	{
		return _samsaraPlatformSlots[index];
	}

	public unsafe void SetElement_SamsaraPlatformSlots(int index, IntPair value, DataContext context)
	{
		_samsaraPlatformSlots[index] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, _dataStatesSamsaraPlatformSlots, CacheInfluencesSamsaraPlatformSlots, context);
		byte* ptr = OperationAdder.FixedElementList_Set(9, 19, index, 8);
		ptr += value.Serialize(ptr);
	}

	public IntPair GetElement_SamsaraPlatformBornDict(int elementId)
	{
		return _samsaraPlatformBornDict[elementId];
	}

	public bool TryGetElement_SamsaraPlatformBornDict(int elementId, out IntPair value)
	{
		return _samsaraPlatformBornDict.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_SamsaraPlatformBornDict(int elementId, IntPair value, DataContext context)
	{
		_samsaraPlatformBornDict.Add(elementId, value);
		_modificationsSamsaraPlatformBornDict.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(9, 20, elementId, 8);
		ptr += value.Serialize(ptr);
	}

	private unsafe void SetElement_SamsaraPlatformBornDict(int elementId, IntPair value, DataContext context)
	{
		_samsaraPlatformBornDict[elementId] = value;
		_modificationsSamsaraPlatformBornDict.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(9, 20, elementId, 8);
		ptr += value.Serialize(ptr);
	}

	private void RemoveElement_SamsaraPlatformBornDict(int elementId, DataContext context)
	{
		_samsaraPlatformBornDict.Remove(elementId);
		_modificationsSamsaraPlatformBornDict.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(9, 20, elementId);
	}

	private void ClearSamsaraPlatformBornDict(DataContext context)
	{
		_samsaraPlatformBornDict.Clear();
		_modificationsSamsaraPlatformBornDict.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(9, 20);
	}

	public BuildingEarningsData GetElement_CollectBuildingEarningsData(BuildingBlockKey elementId)
	{
		return _collectBuildingEarningsData[elementId];
	}

	public bool TryGetElement_CollectBuildingEarningsData(BuildingBlockKey elementId, out BuildingEarningsData value)
	{
		return _collectBuildingEarningsData.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_CollectBuildingEarningsData(BuildingBlockKey elementId, BuildingEarningsData value, DataContext context)
	{
		_collectBuildingEarningsData.Add(elementId, value);
		_modificationsCollectBuildingEarningsData.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(9, 21, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(9, 21, elementId, 0);
		}
	}

	private unsafe void SetElement_CollectBuildingEarningsData(BuildingBlockKey elementId, BuildingEarningsData value, DataContext context)
	{
		_collectBuildingEarningsData[elementId] = value;
		_modificationsCollectBuildingEarningsData.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(9, 21, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(9, 21, elementId, 0);
		}
	}

	private void RemoveElement_CollectBuildingEarningsData(BuildingBlockKey elementId, DataContext context)
	{
		_collectBuildingEarningsData.Remove(elementId);
		_modificationsCollectBuildingEarningsData.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(9, 21, elementId);
	}

	private void ClearCollectBuildingEarningsData(DataContext context)
	{
		_collectBuildingEarningsData.Clear();
		_modificationsCollectBuildingEarningsData.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(9, 21);
	}

	public CharacterList GetElement_ShopManagerDict(BuildingBlockKey elementId)
	{
		return _shopManagerDict[elementId];
	}

	public bool TryGetElement_ShopManagerDict(BuildingBlockKey elementId, out CharacterList value)
	{
		return _shopManagerDict.TryGetValue(elementId, out value);
	}

	private void AddElement_ShopManagerDict(BuildingBlockKey elementId, CharacterList value, DataContext context)
	{
		_shopManagerDict.Add(elementId, value);
		_modificationsShopManagerDict.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, DataStates, CacheInfluences, context);
	}

	private void SetElement_ShopManagerDict(BuildingBlockKey elementId, CharacterList value, DataContext context)
	{
		_shopManagerDict[elementId] = value;
		_modificationsShopManagerDict.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, DataStates, CacheInfluences, context);
	}

	private void RemoveElement_ShopManagerDict(BuildingBlockKey elementId, DataContext context)
	{
		_shopManagerDict.Remove(elementId);
		_modificationsShopManagerDict.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, DataStates, CacheInfluences, context);
	}

	private void ClearShopManagerDict(DataContext context)
	{
		_shopManagerDict.Clear();
		_modificationsShopManagerDict.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, DataStates, CacheInfluences, context);
	}

	public TeaHorseCaravanData GetTeaHorseCaravanData()
	{
		return _teaHorseCaravanData;
	}

	private unsafe void SetTeaHorseCaravanData(TeaHorseCaravanData value, DataContext context)
	{
		_teaHorseCaravanData = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, DataStates, CacheInfluences, context);
		int serializedSize = _teaHorseCaravanData.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValue_Set(9, 23, serializedSize);
		ptr += _teaHorseCaravanData.Serialize(ptr);
	}

	public ushort GetShrineBuyTimes()
	{
		return _shrineBuyTimes;
	}

	public unsafe void SetShrineBuyTimes(ushort value, DataContext context)
	{
		_shrineBuyTimes = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(9, 24, 2);
		*(ushort*)ptr = _shrineBuyTimes;
		ptr += 2;
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public unsafe override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
		foreach (KeyValuePair<Location, BuildingAreaData> buildingArea in _buildingAreas)
		{
			Location key = buildingArea.Key;
			BuildingAreaData value = buildingArea.Value;
			byte* ptr = OperationAdder.FixedSingleValueCollection_Add(9, 0, key, 2);
			ptr += value.Serialize(ptr);
		}
		foreach (KeyValuePair<BuildingBlockKey, BuildingBlockData> buildingBlock in _buildingBlocks)
		{
			BuildingBlockKey key2 = buildingBlock.Key;
			BuildingBlockData value2 = buildingBlock.Value;
			byte* ptr2 = OperationAdder.FixedSingleValueCollection_Add(9, 1, key2, 16);
			ptr2 += value2.Serialize(ptr2);
		}
		int count = _taiwuBuildingAreas.Count;
		int num = 4 * count;
		int valueSize = 2 + num;
		byte* ptr3 = OperationAdder.DynamicSingleValue_Set(9, 2, valueSize);
		*(ushort*)ptr3 = (ushort)count;
		ptr3 += 2;
		for (int i = 0; i < count; i++)
		{
			ptr3 += _taiwuBuildingAreas[i].Serialize(ptr3);
		}
		foreach (KeyValuePair<BuildingBlockKey, sbyte> item in _CollectBuildingResourceType)
		{
			BuildingBlockKey key3 = item.Key;
			sbyte value3 = item.Value;
			byte* ptr4 = OperationAdder.FixedSingleValueCollection_Add(9, 3, key3, 1);
			*ptr4 = (byte)value3;
			ptr4++;
		}
		foreach (KeyValuePair<BuildingBlockKey, int> item2 in _customBuildingName)
		{
			BuildingBlockKey key4 = item2.Key;
			int value4 = item2.Value;
			byte* ptr5 = OperationAdder.FixedSingleValueCollection_Add(9, 5, key4, 4);
			*(int*)ptr5 = value4;
			ptr5 += 4;
		}
		int count2 = _newCompleteOperationBuildings.Count;
		int num2 = 8 * count2;
		int valueSize2 = 2 + num2;
		byte* ptr6 = OperationAdder.DynamicSingleValue_Set(9, 6, valueSize2);
		*(ushort*)ptr6 = (ushort)count2;
		ptr6 += 2;
		for (int j = 0; j < count2; j++)
		{
			ptr6 += _newCompleteOperationBuildings[j].Serialize(ptr6);
		}
		foreach (KeyValuePair<int, ChickenBlessingInfoData> chickenBlessingInfoDatum in _chickenBlessingInfoData)
		{
			int key5 = chickenBlessingInfoDatum.Key;
			ChickenBlessingInfoData value5 = chickenBlessingInfoDatum.Value;
			int serializedSize = value5.GetSerializedSize();
			byte* ptr7 = OperationAdder.DynamicSingleValueCollection_Add(9, 7, key5, serializedSize);
			ptr7 += value5.Serialize(ptr7);
		}
		foreach (KeyValuePair<int, Chicken> item3 in _chicken)
		{
			int key6 = item3.Key;
			Chicken value6 = item3.Value;
			byte* ptr8 = OperationAdder.FixedSingleValueCollection_Add(9, 8, key6, 12);
			ptr8 += value6.Serialize(ptr8);
		}
		byte* ptr9 = OperationAdder.FixedSingleValue_Set(9, 9, 120);
		for (int k = 0; k < 15; k++)
		{
			ptr9 += _collectionCrickets[k].Serialize(ptr9);
		}
		byte* ptr10 = OperationAdder.FixedSingleValue_Set(9, 10, 120);
		for (int l = 0; l < 15; l++)
		{
			ptr10 += _collectionCricketJars[l].Serialize(ptr10);
		}
		byte* ptr11 = OperationAdder.FixedSingleValue_Set(9, 11, 60);
		for (int m = 0; m < 15; m++)
		{
			((int*)ptr11)[m] = _collectionCricketRegen[m];
		}
		ptr11 += 60;
		foreach (KeyValuePair<BuildingBlockKey, MakeItemData> item4 in _makeItemDict)
		{
			BuildingBlockKey key7 = item4.Key;
			MakeItemData value7 = item4.Value;
			if (value7 != null)
			{
				int serializedSize2 = value7.GetSerializedSize();
				byte* ptr12 = OperationAdder.DynamicSingleValueCollection_Add(9, 12, key7, serializedSize2);
				ptr12 += value7.Serialize(ptr12);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(9, 12, key7, 0);
			}
		}
		foreach (KeyValuePair<BuildingBlockKey, CharacterList> residence in _residences)
		{
			BuildingBlockKey key8 = residence.Key;
			CharacterList value8 = residence.Value;
			int serializedSize3 = value8.GetSerializedSize();
			byte* ptr13 = OperationAdder.DynamicSingleValueCollection_Add(9, 13, key8, serializedSize3);
			ptr13 += value8.Serialize(ptr13);
		}
		foreach (KeyValuePair<BuildingBlockKey, CharacterList> comfortableHouse in _comfortableHouses)
		{
			BuildingBlockKey key9 = comfortableHouse.Key;
			CharacterList value9 = comfortableHouse.Value;
			int serializedSize4 = value9.GetSerializedSize();
			byte* ptr14 = OperationAdder.DynamicSingleValueCollection_Add(9, 14, key9, serializedSize4);
			ptr14 += value9.Serialize(ptr14);
		}
		int serializedSize5 = _homeless.GetSerializedSize();
		byte* ptr15 = OperationAdder.DynamicSingleValue_Set(9, 15, serializedSize5);
		ptr15 += _homeless.Serialize(ptr15);
		byte* ptr16 = OperationAdder.FixedSingleValue_Set(9, 16, 12);
		ptr16 += _samsaraPlatformAddMainAttributes.Serialize(ptr16);
		byte* ptr17 = OperationAdder.FixedSingleValue_Set(9, 17, 28);
		ptr17 += _samsaraPlatformAddCombatSkillQualifications.Serialize(ptr17);
		byte* ptr18 = OperationAdder.FixedSingleValue_Set(9, 18, 32);
		ptr18 += _samsaraPlatformAddLifeSkillQualifications.Serialize(ptr18);
		byte* ptr19 = OperationAdder.FixedElementList_InsertRange(9, 19, 0, 6, 48);
		for (int n = 0; n < 6; n++)
		{
			ptr19 += _samsaraPlatformSlots[n].Serialize(ptr19);
		}
		foreach (KeyValuePair<int, IntPair> item5 in _samsaraPlatformBornDict)
		{
			int key10 = item5.Key;
			IntPair value10 = item5.Value;
			byte* ptr20 = OperationAdder.FixedSingleValueCollection_Add(9, 20, key10, 8);
			ptr20 += value10.Serialize(ptr20);
		}
		foreach (KeyValuePair<BuildingBlockKey, BuildingEarningsData> collectBuildingEarningsDatum in _collectBuildingEarningsData)
		{
			BuildingBlockKey key11 = collectBuildingEarningsDatum.Key;
			BuildingEarningsData value11 = collectBuildingEarningsDatum.Value;
			if (value11 != null)
			{
				int serializedSize6 = value11.GetSerializedSize();
				byte* ptr21 = OperationAdder.DynamicSingleValueCollection_Add(9, 21, key11, serializedSize6);
				ptr21 += value11.Serialize(ptr21);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(9, 21, key11, 0);
			}
		}
		int serializedSize7 = _teaHorseCaravanData.GetSerializedSize();
		byte* ptr22 = OperationAdder.DynamicSingleValue_Set(9, 23, serializedSize7);
		ptr22 += _teaHorseCaravanData.Serialize(ptr22);
		byte* ptr23 = OperationAdder.FixedSingleValue_Set(9, 24, 2);
		*(ushort*)ptr23 = _shrineBuyTimes;
		ptr23 += 2;
	}

	public override void OnLoadWorld()
	{
		_pendingLoadingOperationIds = new Queue<uint>();
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(9, 0));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(9, 1));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(9, 2));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(9, 3));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(9, 5));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(9, 6));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(9, 7));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(9, 8));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(9, 9));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(9, 10));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(9, 11));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(9, 12));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(9, 13));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(9, 14));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(9, 15));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(9, 16));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(9, 17));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(9, 18));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedElementList_GetAll(9, 19));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(9, 20));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(9, 21));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(9, 23));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(9, 24));
	}

	public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
	{
		switch (dataId)
		{
		case 0:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 0);
				_modificationsBuildingAreas.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<Location, BuildingAreaData>)_buildingAreas, dataPool);
		case 1:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
				_modificationsBuildingBlocks.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<BuildingBlockKey, BuildingBlockData>)_buildingBlocks, dataPool);
		case 2:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 2);
			}
			return GameData.Serializer.Serializer.Serialize(_taiwuBuildingAreas, dataPool);
		case 3:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 3);
				_modificationsCollectBuildingResourceType.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<BuildingBlockKey, sbyte>)_CollectBuildingResourceType, dataPool);
		case 4:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 4);
				_modificationsBuildingOperatorDict.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<BuildingBlockKey, CharacterList>)_buildingOperatorDict, dataPool);
		case 5:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 5);
				_modificationsCustomBuildingName.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<BuildingBlockKey, int>)_customBuildingName, dataPool);
		case 6:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 7:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 7);
				_modificationsChickenBlessingInfoData.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<int, ChickenBlessingInfoData>)_chickenBlessingInfoData, dataPool);
		case 8:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
				_modificationsChicken.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<int, Chicken>)_chicken, dataPool);
		case 9:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 9);
			}
			return GameData.Serializer.Serializer.Serialize(_collectionCrickets, dataPool);
		case 10:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 10);
			}
			return GameData.Serializer.Serializer.Serialize(_collectionCricketJars, dataPool);
		case 11:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 11);
			}
			return GameData.Serializer.Serializer.Serialize(_collectionCricketRegen, dataPool);
		case 12:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 13:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 14:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 15:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 15);
			}
			return GameData.Serializer.Serializer.Serialize(_homeless, dataPool);
		case 16:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 16);
			}
			return GameData.Serializer.Serializer.Serialize(_samsaraPlatformAddMainAttributes, dataPool);
		case 17:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 17);
			}
			return GameData.Serializer.Serializer.Serialize(_samsaraPlatformAddCombatSkillQualifications, dataPool);
		case 18:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 18);
			}
			return GameData.Serializer.Serializer.Serialize(_samsaraPlatformAddLifeSkillQualifications, dataPool);
		case 19:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(_dataStatesSamsaraPlatformSlots, (int)subId0);
			}
			return GameData.Serializer.Serializer.Serialize(_samsaraPlatformSlots[(uint)subId0], dataPool);
		case 20:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 20);
				_modificationsSamsaraPlatformBornDict.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<int, IntPair>)_samsaraPlatformBornDict, dataPool);
		case 21:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 21);
				_modificationsCollectBuildingEarningsData.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<BuildingBlockKey, BuildingEarningsData>)_collectBuildingEarningsData, dataPool);
		case 22:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 22);
				_modificationsShopManagerDict.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<BuildingBlockKey, CharacterList>)_shopManagerDict, dataPool);
		case 23:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 23);
			}
			return GameData.Serializer.Serializer.Serialize(_teaHorseCaravanData, dataPool);
		case 24:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 24);
			}
			return GameData.Serializer.Serializer.Serialize(_shrineBuyTimes, dataPool);
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 1:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 2:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 3:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 4:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 5:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 6:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 7:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 8:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 9:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _collectionCrickets);
			SetCollectionCrickets(_collectionCrickets, context);
			break;
		case 10:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _collectionCricketJars);
			SetCollectionCricketJars(_collectionCricketJars, context);
			break;
		case 11:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _collectionCricketRegen);
			SetCollectionCricketRegen(_collectionCricketRegen, context);
			break;
		case 12:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 13:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 14:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 15:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _homeless);
			SetHomeless(_homeless, context);
			break;
		case 16:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 17:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 18:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 19:
		{
			IntPair item = default(IntPair);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			_samsaraPlatformSlots[(uint)subId0] = item;
			SetElement_SamsaraPlatformSlots((int)subId0, item, context);
			break;
		}
		case 20:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 21:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 22:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 23:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 24:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _shrineBuyTimes);
			SetShrineBuyTimes(_shrineBuyTimes, context);
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
		{
			int argsCount134 = operation.ArgsCount;
			int num134 = argsCount134;
			if (num134 == 1)
			{
				BuildingBlockKey item338 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item338);
				List<ShopEventData> shopEventDataList = GetShopEventDataList(item338);
				return GameData.Serializer.Serializer.Serialize(shopEventDataList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 1:
		{
			int argsCount91 = operation.ArgsCount;
			int num91 = argsCount91;
			if (num91 == 3)
			{
				BuildingBlockKey item227 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item227);
				sbyte item228 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item228);
				int item229 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item229);
				SetShopManager(context, item227, item228, item229);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 2:
		{
			int argsCount145 = operation.ArgsCount;
			int num145 = argsCount145;
			if (num145 == 2)
			{
				BuildingBlockKey item363 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item363);
				sbyte item364 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item364);
				SetCollectBuildingResourceType(context, item363, item364);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 3:
		{
			int argsCount46 = operation.ArgsCount;
			int num46 = argsCount46;
			if (num46 == 2)
			{
				BuildingBlockKey item92 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item92);
				bool item93 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item93);
				ClearBuildingBlockEarningsData(context, item92, item93);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 4:
		{
			int argsCount110 = operation.ArgsCount;
			int num110 = argsCount110;
			if (num110 == 1)
			{
				BuildingBlockKey item280 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item280);
				int item281 = CalcResourceOutput(context, item280);
				return GameData.Serializer.Serializer.Serialize(item281, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 5:
		{
			int argsCount22 = operation.ArgsCount;
			int num22 = argsCount22;
			if (num22 == 1)
			{
				BuildingBlockKey item41 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item41);
				BuildingEarningsData buildingEarningData = GetBuildingEarningData(item41);
				return GameData.Serializer.Serializer.Serialize(buildingEarningData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 6:
		{
			int argsCount117 = operation.ArgsCount;
			int num117 = argsCount117;
			if (num117 == 1)
			{
				BuildingBlockKey item293 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item293);
				List<int> buildingOperatesData = GetBuildingOperatesData(item293);
				return GameData.Serializer.Serializer.Serialize(buildingOperatesData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 7:
		{
			int argsCount78 = operation.ArgsCount;
			int num78 = argsCount78;
			if (num78 == 1)
			{
				BuildingBlockKey item204 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item204);
				int buildingBuildPeopleAttainments = GetBuildingBuildPeopleAttainments(item204);
				return GameData.Serializer.Serializer.Serialize(buildingBuildPeopleAttainments, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 8:
			switch (operation.ArgsCount)
			{
			case 3:
			{
				BuildingBlockKey item137 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item137);
				int item138 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item138);
				bool item139 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item139);
				BuildingBlockKey item140 = AcceptBuildingBlockCollectEarning(context, item137, item138, item139);
				return GameData.Serializer.Serializer.Serialize(item140, returnDataPool);
			}
			case 4:
			{
				BuildingBlockKey item132 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item132);
				int item133 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item133);
				bool item134 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item134);
				bool item135 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item135);
				BuildingBlockKey item136 = AcceptBuildingBlockCollectEarning(context, item132, item133, item134, item135);
				return GameData.Serializer.Serializer.Serialize(item136, returnDataPool);
			}
			case 5:
			{
				BuildingBlockKey item126 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item126);
				int item127 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item127);
				bool item128 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item128);
				bool item129 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item129);
				bool item130 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item130);
				BuildingBlockKey item131 = AcceptBuildingBlockCollectEarning(context, item126, item127, item128, item129, item130);
				return GameData.Serializer.Serializer.Serialize(item131, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 9:
		{
			int argsCount19 = operation.ArgsCount;
			int num19 = argsCount19;
			if (num19 == 2)
			{
				BuildingBlockKey item36 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item36);
				bool item37 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item37);
				AcceptBuildingBlockCollectEarningQuick(context, item36, item37);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 10:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				BuildingBlockKey item387 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item387);
				int item388 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item388);
				int item389 = AcceptBuildingBlockRecruitPeople(context, item387, item388);
				return GameData.Serializer.Serializer.Serialize(item389, returnDataPool);
			}
			case 3:
			{
				BuildingBlockKey item383 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item383);
				int item384 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item384);
				bool item385 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item385);
				int item386 = AcceptBuildingBlockRecruitPeople(context, item383, item384, item385);
				return GameData.Serializer.Serializer.Serialize(item386, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 11:
		{
			int argsCount129 = operation.ArgsCount;
			int num129 = argsCount129;
			if (num129 == 1)
			{
				BuildingBlockKey item328 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item328);
				List<int> item329 = AcceptBuildingBlockRecruitPeopleQuick(context, item328);
				return GameData.Serializer.Serializer.Serialize(item329, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 12:
		{
			int argsCount95 = operation.ArgsCount;
			int num95 = argsCount95;
			if (num95 == 4)
			{
				BuildingBlockKey item237 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item237);
				int item238 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item238);
				ItemKey item239 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item239);
				bool item240 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item240);
				ShopBuildingSoldItemAdd(context, item237, item238, item239, item240);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 13:
		{
			int argsCount70 = operation.ArgsCount;
			int num70 = argsCount70;
			if (num70 == 4)
			{
				BuildingBlockKey item183 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item183);
				int item184 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item184);
				ItemKey item185 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item185);
				bool item186 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item186);
				ShopBuildingSoldItemChange(context, item183, item184, item185, item186);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 14:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				BuildingBlockKey item161 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item161);
				int item162 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item162);
				ShopBuildingSoldItemReceive(context, item161, item162);
				return -1;
			}
			case 3:
			{
				BuildingBlockKey item158 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item158);
				int item159 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item159);
				bool item160 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item160);
				ShopBuildingSoldItemReceive(context, item158, item159, item160);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 15:
		{
			int argsCount38 = operation.ArgsCount;
			int num38 = argsCount38;
			if (num38 == 1)
			{
				BuildingBlockKey item73 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item73);
				ShopBuildingSoldItemReceiveQuick(context, item73);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 16:
			if (operation.ArgsCount == 0)
			{
				List<ItemDisplayData> item6 = QuickCollectShopItem(context);
				return GameData.Serializer.Serializer.Serialize(item6, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 17:
			if (operation.ArgsCount == 0)
			{
				int item352 = QuickCollectShopItemCount(context);
				return GameData.Serializer.Serializer.Serialize(item352, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 18:
			if (operation.ArgsCount == 0)
			{
				QuickCollectShopSoldItem(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 19:
			if (operation.ArgsCount == 0)
			{
				int item266 = QuickCollectShopSoldItemCount(context);
				return GameData.Serializer.Serializer.Serialize(item266, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 20:
			if (operation.ArgsCount == 0)
			{
				List<int> item215 = QuickRecruitPeople(context);
				return GameData.Serializer.Serializer.Serialize(item215, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 21:
			if (operation.ArgsCount == 0)
			{
				int item182 = QuickRecruitPeopleCount(context);
				return GameData.Serializer.Serializer.Serialize(item182, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 22:
			if (operation.ArgsCount == 0)
			{
				QuickCollectBuildingEarn(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 23:
			if (operation.ArgsCount == 0)
			{
				int item61 = QuickCollectBuildingEarnCount(context);
				return GameData.Serializer.Serializer.Serialize(item61, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 24:
		{
			int argsCount10 = operation.ArgsCount;
			int num10 = argsCount10;
			if (num10 == 3)
			{
				BuildingBlockKey item16 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item16);
				ItemKey item17 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item17);
				ItemSourceType item18 = ItemSourceType.Equipment;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item18);
				AddFixBook(context, item16, item17, item18);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 25:
		{
			int argsCount149 = operation.ArgsCount;
			int num149 = argsCount149;
			if (num149 == 3)
			{
				BuildingBlockKey item376 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item376);
				ItemKey item377 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item377);
				ItemSourceType item378 = ItemSourceType.Equipment;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item378);
				(short, BuildingBlockData) buildingData9 = ChangeFixBook(context, item376, item377, item378);
				return GameData.Serializer.Serializer.Serialize(buildingData9, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 26:
		{
			int argsCount136 = operation.ArgsCount;
			int num136 = argsCount136;
			if (num136 == 2)
			{
				BuildingBlockKey item341 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item341);
				bool item342 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item342);
				ReceiveFixBook(context, item341, item342);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 27:
		{
			int argsCount124 = operation.ArgsCount;
			int num124 = argsCount124;
			if (num124 == 1)
			{
				BuildingBlockKey item305 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item305);
				int fixBookProgress = GetFixBookProgress(context, item305);
				return GameData.Serializer.Serializer.Serialize(fixBookProgress, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 28:
		{
			int argsCount113 = operation.ArgsCount;
			int num113 = argsCount113;
			if (num113 == 1)
			{
				sbyte item286 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item286);
				SetTeaHorseCaravanState(context, item286);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 29:
		{
			int argsCount102 = operation.ArgsCount;
			int num102 = argsCount102;
			if (num102 == 2)
			{
				List<ItemKey> item264 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item264);
				List<ItemKey> item265 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item265);
				ExchangeItemToReplenishment(context, item264, item265);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 30:
			if (operation.ArgsCount == 0)
			{
				StartSearchReplenishment(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 31:
			if (operation.ArgsCount == 0)
			{
				QuickGetExchangeItem(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 32:
		{
			int argsCount65 = operation.ArgsCount;
			int num65 = argsCount65;
			if (num65 == 3)
			{
				short item165 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item165);
				short item166 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item166);
				short item167 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item167);
				TaiwuShrineDisplayData shrineDisplayData = GetShrineDisplayData(context, item165, item166, item167);
				return GameData.Serializer.Serializer.Serialize(shrineDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 33:
		{
			int argsCount56 = operation.ArgsCount;
			int num56 = argsCount56;
			if (num56 == 2)
			{
				int item116 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item116);
				SkillQualificationBonus item117 = default(SkillQualificationBonus);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item117);
				TeachSkill(context, item116, item117);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 34:
		{
			int argsCount40 = operation.ArgsCount;
			int num40 = argsCount40;
			if (num40 == 3)
			{
				int item75 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item75);
				bool item76 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item76);
				ItemKey item77 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item77);
				CricketCollectionAdd(context, item75, item76, item77);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 35:
		{
			int argsCount28 = operation.ArgsCount;
			int num28 = argsCount28;
			if (num28 == 2)
			{
				int item54 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item54);
				bool item55 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item55);
				CricketCollectionRemove(context, item54, item55);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 36:
			if (operation.ArgsCount == 0)
			{
				ItemDisplayData[] collectionCrickets = GetCollectionCrickets(context);
				return GameData.Serializer.Serializer.Serialize(collectionCrickets, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 37:
			if (operation.ArgsCount == 0)
			{
				ItemDisplayData[] collectionJars = GetCollectionJars(context);
				return GameData.Serializer.Serializer.Serialize(collectionJars, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 38:
			if (operation.ArgsCount == 0)
			{
				int[] collectionCricketRegen = GetCollectionCricketRegen(context);
				return GameData.Serializer.Serializer.Serialize(collectionCricketRegen, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 39:
			if (operation.ArgsCount == 0)
			{
				int authorityGain = GetAuthorityGain(context);
				return GameData.Serializer.Serializer.Serialize(authorityGain, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 40:
		{
			int argsCount132 = operation.ArgsCount;
			int num132 = argsCount132;
			if (num132 == 3)
			{
				short item333 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item333);
				BuildingBlockKey item334 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item334);
				sbyte item335 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item335);
				(short, BuildingBlockData) buildingData5 = GmCmd_BuildImmediately(context, item333, item334, item335);
				return GameData.Serializer.Serializer.Serialize(buildingData5, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 41:
		{
			int argsCount123 = operation.ArgsCount;
			int num123 = argsCount123;
			if (num123 == 1)
			{
				BuildingBlockKey item304 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item304);
				(short, BuildingBlockData) buildingData4 = GmCmd_RemoveBuildingImmediately(context, item304);
				return GameData.Serializer.Serializer.Serialize(buildingData4, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 42:
		{
			int argsCount114 = operation.ArgsCount;
			int num114 = argsCount114;
			if (num114 == 1)
			{
				StartMakeArguments item287 = default(StartMakeArguments);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item287);
				MakeItemData item288 = StartMakeItem(context, item287);
				return GameData.Serializer.Serializer.Serialize(item288, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 43:
		{
			int argsCount107 = operation.ArgsCount;
			int num107 = argsCount107;
			if (num107 == 1)
			{
				MakeConditionArguments item275 = default(MakeConditionArguments);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item275);
				bool item276 = CheckMakeCondition(item275);
				return GameData.Serializer.Serializer.Serialize(item276, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 44:
		{
			int argsCount98 = operation.ArgsCount;
			int num98 = argsCount98;
			if (num98 == 1)
			{
				BuildingBlockKey item256 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item256);
				List<ItemDisplayData> makeItems = GetMakeItems(context, item256);
				return GameData.Serializer.Serializer.Serialize(makeItems, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 45:
		{
			int argsCount90 = operation.ArgsCount;
			int num90 = argsCount90;
			if (num90 == 1)
			{
				BuildingBlockKey item226 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item226);
				MakeItemData makingItemData = GetMakingItemData(item226);
				return GameData.Serializer.Serializer.Serialize(makingItemData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 46:
		{
			int argsCount80 = operation.ArgsCount;
			int num80 = argsCount80;
			if (num80 == 3)
			{
				int item206 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item206);
				ItemKey item207 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item207);
				ItemKey item208 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item208);
				ItemDisplayData item209 = RepairItem(context, item206, item207, item208);
				return GameData.Serializer.Serializer.Serialize(item209, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 47:
		{
			int argsCount74 = operation.ArgsCount;
			int num74 = argsCount74;
			if (num74 == 4)
			{
				int item191 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item191);
				ItemKey item192 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item192);
				ItemKey item193 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item193);
				BuildingBlockKey item194 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item194);
				bool item195 = CheckRepairConditionIsMeet(item191, item192, item193, item194);
				return GameData.Serializer.Serializer.Serialize(item195, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 48:
		{
			int argsCount68 = operation.ArgsCount;
			int num68 = argsCount68;
			if (num68 == 5)
			{
				int item174 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item174);
				ItemDisplayData item175 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item175);
				ItemDisplayData item176 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item176);
				ItemDisplayData[] item177 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item177);
				List<ItemDisplayData> item178 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item178);
				(bool, ItemDisplayData) addPoisonResult = AddItemPoison(context, item174, item175, item176, item177, item178);
				return GameData.Serializer.Serializer.Serialize(addPoisonResult, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 49:
		{
			int argsCount62 = operation.ArgsCount;
			int num62 = argsCount62;
			if (num62 == 6)
			{
				int item149 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item149);
				ItemKey item150 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item150);
				ItemKey item151 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item151);
				ItemKey[] item152 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item152);
				BuildingBlockKey item153 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item153);
				FullPoisonEffects item154 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item154);
				bool item155 = CheckAddPoisonCondition(item149, item150, item151, item152, item153, item154);
				return GameData.Serializer.Serializer.Serialize(item155, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 50:
		{
			int argsCount51 = operation.ArgsCount;
			int num51 = argsCount51;
			if (num51 == 5)
			{
				int item102 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item102);
				ItemDisplayData item103 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item103);
				ItemDisplayData item104 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item104);
				ItemDisplayData[] item105 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item105);
				bool item106 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item106);
				(bool, List<ItemDisplayData>) result = RemoveItemPoison(context, item102, item103, item104, item105, item106);
				return GameData.Serializer.Serializer.Serialize(result, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 51:
		{
			int argsCount43 = operation.ArgsCount;
			int num43 = argsCount43;
			if (num43 == 6)
			{
				int item81 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item81);
				ItemKey item82 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item82);
				ItemKey item83 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item83);
				ItemKey[] item84 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item84);
				BuildingBlockKey item85 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item85);
				bool item86 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item86);
				bool item87 = CheckRemovePoisonCondition(item81, item82, item83, item84, item85, item86);
				return GameData.Serializer.Serializer.Serialize(item87, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 52:
		{
			int argsCount35 = operation.ArgsCount;
			int num35 = argsCount35;
			if (num35 == 5)
			{
				int item65 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item65);
				ItemDisplayData item66 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item66);
				ItemDisplayData item67 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item67);
				ItemDisplayData[] item68 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item68);
				List<ItemSourceChange> item69 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item69);
				ItemDisplayData item70 = RefineItem(context, item65, item66, item67, item68, item69);
				return GameData.Serializer.Serializer.Serialize(item70, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 53:
		{
			int argsCount27 = operation.ArgsCount;
			int num27 = argsCount27;
			if (num27 == 5)
			{
				int item48 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item48);
				ItemKey item49 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item49);
				ItemKey item50 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item50);
				ItemDisplayData[] item51 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item51);
				BuildingBlockKey item52 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item52);
				bool item53 = CheckRefineCondition(item48, item49, item50, item51, item52);
				return GameData.Serializer.Serializer.Serialize(item53, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 54:
		{
			int argsCount15 = operation.ArgsCount;
			int num15 = argsCount15;
			if (num15 == 3)
			{
				BuildingBlockKey item25 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item25);
				short item26 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item26);
				int[] item27 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item27);
				(short, BuildingBlockData) buildingData2 = Build(context, item25, item26, item27);
				return GameData.Serializer.Serializer.Serialize(buildingData2, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 55:
		{
			int argsCount7 = operation.ArgsCount;
			int num7 = argsCount7;
			if (num7 == 2)
			{
				BuildingBlockKey item10 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item10);
				int[] item11 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item11);
				(short, BuildingBlockData) buildingData = Upgrade(context, item10, item11);
				return GameData.Serializer.Serializer.Serialize(buildingData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 56:
		{
			int argsCount153 = operation.ArgsCount;
			int num153 = argsCount153;
			if (num153 == 2)
			{
				BuildingBlockKey item392 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item392);
				int[] item393 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item393);
				(short, BuildingBlockData) buildingData10 = Remove(context, item392, item393);
				return GameData.Serializer.Serializer.Serialize(buildingData10, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 57:
		{
			int argsCount148 = operation.ArgsCount;
			int num148 = argsCount148;
			if (num148 == 2)
			{
				BuildingBlockKey item374 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item374);
				bool item375 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item375);
				(short, BuildingBlockData) buildingData8 = SetStopOperation(context, item374, item375);
				return GameData.Serializer.Serializer.Serialize(buildingData8, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 58:
		{
			int argsCount142 = operation.ArgsCount;
			int num142 = argsCount142;
			if (num142 == 3)
			{
				BuildingBlockKey item357 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item357);
				sbyte item358 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item358);
				int item359 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item359);
				SetOperator(context, item357, item358, item359);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 59:
		{
			int argsCount138 = operation.ArgsCount;
			int num138 = argsCount138;
			if (num138 == 2)
			{
				BuildingBlockKey item345 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item345);
				bool item346 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item346);
				(short, BuildingBlockData) buildingData7 = SetBuildingMaintenance(context, item345, item346);
				return GameData.Serializer.Serializer.Serialize(buildingData7, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 60:
		{
			int argsCount133 = operation.ArgsCount;
			int num133 = argsCount133;
			if (num133 == 1)
			{
				BuildingBlockKey item336 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item336);
				(short, BuildingBlockData) buildingData6 = Repair(context, item336);
				return GameData.Serializer.Serializer.Serialize(buildingData6, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 61:
		{
			int argsCount128 = operation.ArgsCount;
			int num128 = argsCount128;
			if (num128 == 2)
			{
				List<IntPair> item321 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item321);
				Location item322 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item322);
				ConfirmPlanBuilding(context, item321, item322);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 62:
		{
			int argsCount120 = operation.ArgsCount;
			int num120 = argsCount120;
			if (num120 == 2)
			{
				int item297 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item297);
				BuildingBlockKey item298 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item298);
				AddToResidence(context, item297, item298);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 63:
		{
			int argsCount116 = operation.ArgsCount;
			int num116 = argsCount116;
			if (num116 == 2)
			{
				int item291 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item291);
				BuildingBlockKey item292 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item292);
				RemoveFromResidence(context, item291, item292);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 64:
		{
			int argsCount111 = operation.ArgsCount;
			int num111 = argsCount111;
			if (num111 == 3)
			{
				int item282 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item282);
				BuildingBlockKey item283 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item283);
				sbyte item284 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item284);
				ReplaceCharacterInResidence(context, item282, item283, item284);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 65:
		{
			int argsCount105 = operation.ArgsCount;
			int num105 = argsCount105;
			if (num105 == 3)
			{
				int item271 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item271);
				BuildingBlockKey item272 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item272);
				sbyte item273 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item273);
				ReplaceCharacterInComfortableHouse(context, item271, item272, item273);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 66:
		{
			int argsCount99 = operation.ArgsCount;
			int num99 = argsCount99;
			if (num99 == 2)
			{
				int item257 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item257);
				BuildingBlockKey item258 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item258);
				AddToComfortableHouse(context, item257, item258);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 67:
		{
			int argsCount93 = operation.ArgsCount;
			int num93 = argsCount93;
			if (num93 == 2)
			{
				int item233 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item233);
				BuildingBlockKey item234 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item234);
				RemoveFromComfortableHouse(context, item233, item234);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 68:
		{
			int argsCount87 = operation.ArgsCount;
			int num87 = argsCount87;
			if (num87 == 1)
			{
				BuildingBlockKey item219 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item219);
				CharacterList item220 = QuickFillResidence(context, item219);
				return GameData.Serializer.Serializer.Serialize(item220, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 69:
		{
			int argsCount83 = operation.ArgsCount;
			int num83 = argsCount83;
			if (num83 == 1)
			{
				BuildingBlockKey item214 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item214);
				CharacterList charsInResidence = GetCharsInResidence(context, item214);
				return GameData.Serializer.Serializer.Serialize(charsInResidence, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 70:
		{
			int argsCount76 = operation.ArgsCount;
			int num76 = argsCount76;
			if (num76 == 2)
			{
				BuildingBlockKey item198 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item198);
				bool item199 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item199);
				List<CharacterList> allResidents = GetAllResidents(context, item198, item199);
				return GameData.Serializer.Serializer.Serialize(allResidents, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 71:
		{
			int argsCount73 = operation.ArgsCount;
			int num73 = argsCount73;
			if (num73 == 1)
			{
				BuildingBlockKey item189 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item189);
				CharacterList charsInComfortableHouse = GetCharsInComfortableHouse(context, item189);
				return GameData.Serializer.Serializer.Serialize(charsInComfortableHouse, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 72:
			if (operation.ArgsCount == 0)
			{
				CharacterList homeless = GetHomeless(context);
				return GameData.Serializer.Serializer.Serialize(homeless, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 73:
			if (operation.ArgsCount == 0)
			{
				List<SamsaraPlatformCharDisplayData> samsaraPlatformCharList = GetSamsaraPlatformCharList(context);
				return GameData.Serializer.Serializer.Serialize(samsaraPlatformCharList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 74:
		{
			int argsCount59 = operation.ArgsCount;
			int num59 = argsCount59;
			if (num59 == 2)
			{
				sbyte item141 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item141);
				int item142 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item142);
				SetSamsaraPlatformChar(context, item141, item142);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 75:
		{
			int argsCount54 = operation.ArgsCount;
			int num54 = argsCount54;
			if (num54 == 1)
			{
				sbyte item110 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item110);
				CharacterDisplayData item111 = SamsaraPlatformReborn(context, item110);
				return GameData.Serializer.Serializer.Serialize(item111, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 76:
		{
			int argsCount48 = operation.ArgsCount;
			int num48 = argsCount48;
			if (num48 == 1)
			{
				Location item96 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item96);
				BuildingAreaData buildingAreaData = GetBuildingAreaData(item96);
				return GameData.Serializer.Serializer.Serialize(buildingAreaData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 77:
		{
			int argsCount42 = operation.ArgsCount;
			int num42 = argsCount42;
			if (num42 == 1)
			{
				Location item80 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item80);
				List<BuildingBlockData> buildingBlockList = GetBuildingBlockList(item80);
				return GameData.Serializer.Serializer.Serialize(buildingBlockList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 78:
		{
			int argsCount36 = operation.ArgsCount;
			int num36 = argsCount36;
			if (num36 == 1)
			{
				BuildingBlockKey item71 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item71);
				BuildingBlockData buildingBlockData = GetBuildingBlockData(item71);
				return GameData.Serializer.Serializer.Serialize(buildingBlockData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 79:
		{
			int argsCount31 = operation.ArgsCount;
			int num31 = argsCount31;
			if (num31 == 2)
			{
				BuildingBlockKey item58 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item58);
				string item59 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item59);
				SetBuildingCustomName(context, item58, item59);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 80:
		{
			int argsCount24 = operation.ArgsCount;
			int num24 = argsCount24;
			if (num24 == 2)
			{
				short item43 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item43);
				short item44 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item44);
				int emptyBlockCount = GetEmptyBlockCount(item43, item44);
				return GameData.Serializer.Serializer.Serialize(emptyBlockCount, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 81:
		{
			int argsCount18 = operation.ArgsCount;
			int num18 = argsCount18;
			if (num18 == 2)
			{
				int item33 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item33);
				short item34 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item34);
				int item35 = AddChicken(context, item33, item34);
				return GameData.Serializer.Serializer.Serialize(item35, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 82:
		{
			int argsCount11 = operation.ArgsCount;
			int num11 = argsCount11;
			if (num11 == 1)
			{
				int item19 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item19);
				RemoveChicken(context, item19);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 83:
			if (operation.ArgsCount == 0)
			{
				RemoveAllChicken(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 84:
		{
			int argsCount2 = operation.ArgsCount;
			int num2 = argsCount2;
			if (num2 == 2)
			{
				int item2 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item2);
				int item3 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item3);
				MoveChicken(context, item2, item3);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 85:
		{
			int argsCount151 = operation.ArgsCount;
			int num151 = argsCount151;
			if (num151 == 2)
			{
				int item381 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item381);
				int item382 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item382);
				TransferChicken(context, item381, item382);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 86:
		{
			int argsCount146 = operation.ArgsCount;
			int num146 = argsCount146;
			if (num146 == 1)
			{
				Location item369 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item369);
				List<int> locationChicken = GetLocationChicken(item369);
				return GameData.Serializer.Serializer.Serialize(locationChicken, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 87:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				int item368 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item368);
				List<int> settlementChickenList2 = GetSettlementChickenList(item368);
				return GameData.Serializer.Serializer.Serialize(settlementChickenList2, returnDataPool);
			}
			case 2:
			{
				int item366 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item366);
				bool item367 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item367);
				List<int> settlementChickenList = GetSettlementChickenList(item366, item367);
				return GameData.Serializer.Serializer.Serialize(settlementChickenList, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 88:
		{
			int argsCount143 = operation.ArgsCount;
			int num143 = argsCount143;
			if (num143 == 1)
			{
				int item360 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item360);
				Chicken chickenData = GetChickenData(item360);
				return GameData.Serializer.Serializer.Serialize(chickenData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 89:
		{
			int argsCount140 = operation.ArgsCount;
			int num140 = argsCount140;
			if (num140 == 2)
			{
				int item349 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item349);
				ItemKey item350 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item350);
				sbyte item351 = FeedChicken(context, item349, item350);
				return GameData.Serializer.Serializer.Serialize(item351, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 90:
			if (operation.ArgsCount == 0)
			{
				InitMapBlockChicken(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 91:
			if (operation.ArgsCount == 0)
			{
				bool item337 = IsHaveChickenKing(context);
				return GameData.Serializer.Serializer.Serialize(item337, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 92:
		{
			int argsCount130 = operation.ArgsCount;
			int num130 = argsCount130;
			if (num130 == 1)
			{
				BuildingBlockKey item330 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item330);
				RemoveAllFormResidence(context, item330);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 93:
		{
			int argsCount127 = operation.ArgsCount;
			int num127 = argsCount127;
			if (num127 == 4)
			{
				BuildingAreaData item316 = new BuildingAreaData();
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item316);
				Location item317 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item317);
				short item318 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item318);
				BuildingBlockItem item319 = null;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item319);
				bool item320 = NearDependBuildings(item316, item317, item318, item319);
				return GameData.Serializer.Serializer.Serialize(item320, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 94:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				BuildingBlockData item313 = new BuildingBlockData();
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item313);
				BuildingBlockKey item314 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item314);
				int buildingAttainment2 = GetBuildingAttainment(item313, item314);
				return GameData.Serializer.Serializer.Serialize(buildingAttainment2, returnDataPool);
			}
			case 3:
			{
				BuildingBlockData item310 = new BuildingBlockData();
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item310);
				BuildingBlockKey item311 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item311);
				bool item312 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item312);
				int buildingAttainment = GetBuildingAttainment(item310, item311, item312);
				return GameData.Serializer.Serializer.Serialize(buildingAttainment, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 95:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				BuildingBlockKey item309 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item309);
				int attainmentOfBuilding2 = GetAttainmentOfBuilding(item309);
				return GameData.Serializer.Serializer.Serialize(attainmentOfBuilding2, returnDataPool);
			}
			case 2:
			{
				BuildingBlockKey item307 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item307);
				bool item308 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item308);
				int attainmentOfBuilding = GetAttainmentOfBuilding(item307, item308);
				return GameData.Serializer.Serializer.Serialize(attainmentOfBuilding, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 96:
		{
			int argsCount121 = operation.ArgsCount;
			int num121 = argsCount121;
			if (num121 == 2)
			{
				BuildingBlockKey item299 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item299);
				sbyte item300 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item300);
				int item301 = CalcResourceOutputCount(item299, item300);
				return GameData.Serializer.Serializer.Serialize(item301, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 97:
		{
			int argsCount118 = operation.ArgsCount;
			int num118 = argsCount118;
			if (num118 == 2)
			{
				List<int> item294 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item294);
				byte item295 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item295);
				DealInfectedPeople(context, item294, item295);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 98:
		{
			int argsCount115 = operation.ArgsCount;
			int num115 = argsCount115;
			if (num115 == 1)
			{
				BuildingBlockKey item289 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item289);
				List<ItemDisplayData> item290 = QuickCollectSingleShopItem(context, item289);
				return GameData.Serializer.Serializer.Serialize(item290, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 99:
		{
			int argsCount112 = operation.ArgsCount;
			int num112 = argsCount112;
			if (num112 == 1)
			{
				BuildingBlockKey item285 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item285);
				QuickCollectSingleShopSoldItem(context, item285);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 100:
		{
			int argsCount108 = operation.ArgsCount;
			int num108 = argsCount108;
			if (num108 == 1)
			{
				BuildingBlockKey item277 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item277);
				List<int> item278 = QuickRecruitSingleBuildingPeople(context, item277);
				return GameData.Serializer.Serializer.Serialize(item278, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 101:
		{
			int argsCount104 = operation.ArgsCount;
			int num104 = argsCount104;
			if (num104 == 1)
			{
				BuildingBlockKey item269 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item269);
				CharacterList item270 = QuickFillComfortableHouse(context, item269);
				return GameData.Serializer.Serializer.Serialize(item270, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 102:
		{
			int argsCount100 = operation.ArgsCount;
			int num100 = argsCount100;
			if (num100 == 1)
			{
				BuildingBlockKey item259 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item259);
				RemoveAllFromComfortableHouse(context, item259);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 103:
		{
			int argsCount97 = operation.ArgsCount;
			int num97 = argsCount97;
			if (num97 == 1)
			{
				List<int> item254 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item254);
				List<int> item255 = SortedComfortableHousePeople(context, item254);
				return GameData.Serializer.Serializer.Serialize(item255, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 104:
			switch (operation.ArgsCount)
			{
			case 5:
			{
				short item247 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item247);
				ItemKey item248 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item248);
				BuildingBlockKey item249 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item249);
				sbyte item250 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item250);
				List<short> item251 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item251);
				MakeResult makeResult2 = GetMakeResult(item247, item248, item249, item250, item251, -1);
				return GameData.Serializer.Serializer.Serialize(makeResult2, returnDataPool);
			}
			case 6:
			{
				short item241 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item241);
				ItemKey item242 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item242);
				BuildingBlockKey item243 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item243);
				sbyte item244 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item244);
				List<short> item245 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item245);
				short item246 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item246);
				MakeResult makeResult = GetMakeResult(item241, item242, item243, item244, item245, item246);
				return GameData.Serializer.Serializer.Serialize(makeResult, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 105:
			if (operation.ArgsCount == 0)
			{
				int sutraReadingRoomBuffValue = GetSutraReadingRoomBuffValue();
				return GameData.Serializer.Serializer.Serialize(sutraReadingRoomBuffValue, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 106:
		{
			int argsCount88 = operation.ArgsCount;
			int num88 = argsCount88;
			if (num88 == 2)
			{
				short item221 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item221);
				bool item222 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item222);
				SetBuildingAutoWork(context, item221, item222);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 107:
		{
			int argsCount85 = operation.ArgsCount;
			int num85 = argsCount85;
			if (num85 == 1)
			{
				short item217 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item217);
				bool buildingIsAutoWork = GetBuildingIsAutoWork(item217);
				return GameData.Serializer.Serializer.Serialize(buildingIsAutoWork, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 108:
		{
			int argsCount81 = operation.ArgsCount;
			int num81 = argsCount81;
			if (num81 == 3)
			{
				BuildingBlockKey item210 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item210);
				List<ItemKey> item211 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item211);
				List<int> item212 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item212);
				ShopBuildingMultiChangeSoldItem(context, item210, item211, item212);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 109:
		{
			int argsCount77 = operation.ArgsCount;
			int num77 = argsCount77;
			if (num77 == 2)
			{
				int item201 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item201);
				List<MultiplyOperation> item202 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item202);
				List<ItemDisplayData> item203 = RepairItemList(context, item201, item202);
				return GameData.Serializer.Serializer.Serialize(item203, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 110:
		{
			int argsCount75 = operation.ArgsCount;
			int num75 = argsCount75;
			if (num75 == 2)
			{
				short item196 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item196);
				bool item197 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item197);
				SetBuildingAutoSold(context, item196, item197);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 111:
		{
			int argsCount72 = operation.ArgsCount;
			int num72 = argsCount72;
			if (num72 == 1)
			{
				short item188 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item188);
				bool buildingIsAutoSold = GetBuildingIsAutoSold(item188);
				return GameData.Serializer.Serializer.Serialize(buildingIsAutoSold, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 112:
			if (operation.ArgsCount == 0)
			{
				List<sbyte> xiangshuIdInKungfuRoom = GetXiangshuIdInKungfuRoom();
				return GameData.Serializer.Serializer.Serialize(xiangshuIdInKungfuRoom, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 113:
		{
			int argsCount67 = operation.ArgsCount;
			int num67 = argsCount67;
			if (num67 == 4)
			{
				int item169 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item169);
				ItemKey item170 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item170);
				ItemKey item171 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item171);
				sbyte item172 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item172);
				ItemDisplayData item173 = RepairItemOptional(context, item169, item170, item171, item172);
				return GameData.Serializer.Serializer.Serialize(item173, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 114:
		{
			int argsCount64 = operation.ArgsCount;
			int num64 = argsCount64;
			if (num64 == 2)
			{
				short item163 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item163);
				bool item164 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item164);
				SetShopIsResultFirst(context, item163, item164);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 115:
		{
			int argsCount61 = operation.ArgsCount;
			int num61 = argsCount61;
			if (num61 == 1)
			{
				short item148 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item148);
				bool shopIsResultFirst = GetShopIsResultFirst(item148);
				return GameData.Serializer.Serializer.Serialize(shopIsResultFirst, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 116:
		{
			int argsCount57 = operation.ArgsCount;
			int num57 = argsCount57;
			if (num57 == 1)
			{
				short item118 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item118);
				SetBuildingAutoExpandUpTop(context, item118);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 117:
		{
			int argsCount53 = operation.ArgsCount;
			int num53 = argsCount53;
			if (num53 == 1)
			{
				short item109 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item109);
				SetBuildingAutoExpandDown(context, item109);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 118:
		{
			int argsCount49 = operation.ArgsCount;
			int num49 = argsCount49;
			if (num49 == 1)
			{
				short item97 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item97);
				bool buildingIsAutoExpand = GetBuildingIsAutoExpand(item97);
				return GameData.Serializer.Serializer.Serialize(buildingIsAutoExpand, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 119:
		{
			int argsCount45 = operation.ArgsCount;
			int num45 = argsCount45;
			if (num45 == 2)
			{
				short item90 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item90);
				bool item91 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item91);
				SetBuildingAutoExpand(context, item90, item91);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 120:
		{
			int argsCount41 = operation.ArgsCount;
			int num41 = argsCount41;
			if (num41 == 1)
			{
				short item78 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item78);
				SetBuildingAutoExpandDownBottom(context, item78);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 121:
		{
			int argsCount37 = operation.ArgsCount;
			int num37 = argsCount37;
			if (num37 == 1)
			{
				short item72 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item72);
				SetBuildingAutoExpandUp(context, item72);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 122:
		{
			int argsCount33 = operation.ArgsCount;
			int num33 = argsCount33;
			if (num33 == 2)
			{
				int item62 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item62);
				string item63 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item63);
				SetNickNameByChickenId(context, item62, item63);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 123:
		{
			int argsCount30 = operation.ArgsCount;
			int num30 = argsCount30;
			if (num30 == 1)
			{
				Location item57 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item57);
				List<string> chickensNicknameByIdList = GetChickensNicknameByIdList(item57);
				return GameData.Serializer.Serializer.Serialize(chickensNicknameByIdList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 124:
		{
			int argsCount25 = operation.ArgsCount;
			int num25 = argsCount25;
			if (num25 == 1)
			{
				Location item45 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item45);
				List<Chicken> settlementChickenDataList = GetSettlementChickenDataList(item45);
				return GameData.Serializer.Serializer.Serialize(settlementChickenDataList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 125:
		{
			int argsCount21 = operation.ArgsCount;
			int num21 = argsCount21;
			if (num21 == 1)
			{
				short item40 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item40);
				SetTeaHorseCaravanWeather(context, item40);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 126:
		{
			int argsCount16 = operation.ArgsCount;
			int num16 = argsCount16;
			if (num16 == 1)
			{
				short item28 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item28);
				bool comfortableIsAutoCheckIn = GetComfortableIsAutoCheckIn(item28);
				return GameData.Serializer.Serializer.Serialize(comfortableIsAutoCheckIn, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 127:
		{
			int argsCount13 = operation.ArgsCount;
			int num13 = argsCount13;
			if (num13 == 1)
			{
				short item21 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item21);
				bool residenceIsAutoCheckIn = GetResidenceIsAutoCheckIn(item21);
				return GameData.Serializer.Serializer.Serialize(residenceIsAutoCheckIn, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 128:
		{
			int argsCount8 = operation.ArgsCount;
			int num8 = argsCount8;
			if (num8 == 2)
			{
				short item12 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item12);
				bool item13 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item13);
				SetComfortableAutoCheckIn(context, item12, item13);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 129:
		{
			int argsCount5 = operation.ArgsCount;
			int num5 = argsCount5;
			if (num5 == 2)
			{
				short item7 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item7);
				bool item8 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item8);
				SetResidenceAutoCheckIn(context, item7, item8);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 130:
		{
			int argsCount3 = operation.ArgsCount;
			int num3 = argsCount3;
			if (num3 == 1)
			{
				short item4 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item4);
				GmCmd_AddLegacyBuilding(context, item4);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 131:
		{
			int argsCount152 = operation.ArgsCount;
			int num152 = argsCount152;
			if (num152 == 2)
			{
				int item390 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item390);
				bool item391 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item391);
				SetUnlockedWorkingVillagers(context, item390, item391);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 132:
		{
			int argsCount150 = operation.ArgsCount;
			int num150 = argsCount150;
			if (num150 == 1)
			{
				BuildingBlockKey item379 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item379);
				bool item380 = CanAutoExpand(context, item379);
				return GameData.Serializer.Serializer.Serialize(item380, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 133:
		{
			int argsCount147 = operation.ArgsCount;
			int num147 = argsCount147;
			if (num147 == 3)
			{
				ItemDisplayData item370 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item370);
				ItemDisplayData item371 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item371);
				short item372 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item372);
				ItemDisplayData item373 = WeaveClothingItem(context, item370, item371, item372);
				return GameData.Serializer.Serializer.Serialize(item373, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 134:
			if (operation.ArgsCount == 0)
			{
				List<Chicken> item365 = GmCmd_GetChickenData();
				return GameData.Serializer.Serializer.Serialize(item365, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 135:
		{
			int argsCount144 = operation.ArgsCount;
			int num144 = argsCount144;
			if (num144 == 2)
			{
				int item361 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item361);
				int item362 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item362);
				PossessionPreview possessionPreview = GetPossessionPreview(context, item361, item362);
				return GameData.Serializer.Serializer.Serialize(possessionPreview, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 136:
		{
			int argsCount141 = operation.ArgsCount;
			int num141 = argsCount141;
			if (num141 == 3)
			{
				int item353 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item353);
				int item354 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item354);
				AvatarExtraData item355 = new AvatarExtraData();
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item355);
				byte item356 = TrySwapSoulCeremony(context, item353, item354, item355);
				return GameData.Serializer.Serializer.Serialize(item356, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 137:
		{
			int argsCount139 = operation.ArgsCount;
			int num139 = argsCount139;
			if (num139 == 2)
			{
				ItemKey item347 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item347);
				sbyte item348 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item348);
				GetBackTeaHorseCarryItem(context, item347, item348);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 138:
		{
			int argsCount137 = operation.ArgsCount;
			int num137 = argsCount137;
			if (num137 == 2)
			{
				ItemKey item343 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item343);
				sbyte item344 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item344);
				AddItemToTeaHorseCarryItem(context, item343, item344);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 139:
		{
			int argsCount135 = operation.ArgsCount;
			int num135 = argsCount135;
			if (num135 == 2)
			{
				AvatarData item339 = new AvatarData();
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item339);
				AvatarExtraData item340 = new AvatarExtraData();
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item340);
				SetTemporaryPossessionCharacterAvatar(context, item339, item340);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 140:
			if (operation.ArgsCount == 0)
			{
				List<int> swapSoulCeremonyBodyCharIdList = GetSwapSoulCeremonyBodyCharIdList();
				return GameData.Serializer.Serializer.Serialize(swapSoulCeremonyBodyCharIdList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 141:
		{
			int argsCount131 = operation.ArgsCount;
			int num131 = argsCount131;
			if (num131 == 2)
			{
				BuildingBlockKey item331 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item331);
				int[] item332 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item332);
				int[] buildingShopManagerAutoArrangeSorted = GetBuildingShopManagerAutoArrangeSorted(item331, item332);
				return GameData.Serializer.Serializer.Serialize(buildingShopManagerAutoArrangeSorted, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 142:
			if (operation.ArgsCount == 0)
			{
				SectMainStoryJingangClickMonkSoulBtn(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 143:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				BuildingBlockKey item326 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item326);
				int item327 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item327);
				RejectBuildingBlockRecruitPeople(context, item326, item327);
				return -1;
			}
			case 3:
			{
				BuildingBlockKey item323 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item323);
				int item324 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item324);
				bool item325 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item325);
				RejectBuildingBlockRecruitPeople(context, item323, item324, item325);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 144:
		{
			int argsCount126 = operation.ArgsCount;
			int num126 = argsCount126;
			if (num126 == 1)
			{
				BuildingBlockKey item315 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item315);
				RejectBuildingBlockRecruitPeopleQuick(context, item315);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 145:
		{
			int argsCount125 = operation.ArgsCount;
			int num125 = argsCount125;
			if (num125 == 1)
			{
				BuildingBlockKey item306 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item306);
				BuildingManageYieldTipsData shopManagementYieldTipsData = GetShopManagementYieldTipsData(context, item306);
				return GameData.Serializer.Serializer.Serialize(shopManagementYieldTipsData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 146:
		{
			int argsCount122 = operation.ArgsCount;
			int num122 = argsCount122;
			if (num122 == 1)
			{
				BuildingBlockKey item302 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item302);
				int item303 = CalculateBuildingManageHarvestSuccessRate(item302);
				return GameData.Serializer.Serializer.Serialize(item303, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 147:
		{
			int argsCount119 = operation.ArgsCount;
			int num119 = argsCount119;
			if (num119 == 1)
			{
				BuildingBlockKey item296 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item296);
				ShopEventCollection orCreateShopEventCollection = GetOrCreateShopEventCollection(item296);
				return GameData.Serializer.Serializer.Serialize(orCreateShopEventCollection, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 148:
			if (operation.ArgsCount == 0)
			{
				SamsaraPlatformRecordCollection samsaraPlatformRecord = GetSamsaraPlatformRecord();
				return GameData.Serializer.Serializer.Serialize(samsaraPlatformRecord, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 149:
			if (operation.ArgsCount == 0)
			{
				List<SamsaraPlatformCharDisplayData> swapSoulCeremonySoulCharIdList = GetSwapSoulCeremonySoulCharIdList(context);
				return GameData.Serializer.Serializer.Serialize(swapSoulCeremonySoulCharIdList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 150:
			if (operation.ArgsCount == 0)
			{
				CricketCollectionBatchAddCricketJar(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 151:
			if (operation.ArgsCount == 0)
			{
				CricketCollectionBatchAddCricket(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 152:
		{
			int argsCount109 = operation.ArgsCount;
			int num109 = argsCount109;
			if (num109 == 1)
			{
				ItemSourceType item279 = ItemSourceType.Equipment;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item279);
				CricketCollectionBatchRemoveJar(context, item279);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 153:
		{
			int argsCount106 = operation.ArgsCount;
			int num106 = argsCount106;
			if (num106 == 1)
			{
				ItemSourceType item274 = ItemSourceType.Equipment;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item274);
				CricketCollectionBatchRemoveCricket(context, item274);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 154:
		{
			int argsCount103 = operation.ArgsCount;
			int num103 = argsCount103;
			if (num103 == 2)
			{
				short item267 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item267);
				ItemSourceType item268 = ItemSourceType.Equipment;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item268);
				List<ItemDisplayData> cricketOrJarFromSourceStorage = GetCricketOrJarFromSourceStorage(context, item267, item268);
				return GameData.Serializer.Serializer.Serialize(cricketOrJarFromSourceStorage, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 155:
		{
			int argsCount101 = operation.ArgsCount;
			int num101 = argsCount101;
			if (num101 == 4)
			{
				int item260 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item260);
				short item261 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item261);
				ItemSourceType item262 = ItemSourceType.Equipment;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item262);
				ItemKey item263 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item263);
				SmartOperateCricketOrJarCollection(context, item260, item261, item262, item263);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 156:
			if (operation.ArgsCount == 0)
			{
				CricketCollectionBatchButtonStateDisplayData batchButtonEnableState = GetBatchButtonEnableState(context);
				return GameData.Serializer.Serializer.Serialize(batchButtonEnableState, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 157:
		{
			int argsCount96 = operation.ArgsCount;
			int num96 = argsCount96;
			if (num96 == 1)
			{
				BuildingBlockKey item252 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item252);
				int[] item253 = CalculateBuildingManageHarvestSuccessRates(item252);
				return GameData.Serializer.Serializer.Serialize(item253, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 158:
		{
			int argsCount94 = operation.ArgsCount;
			int num94 = argsCount94;
			if (num94 == 2)
			{
				BuildingBlockKey item235 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item235);
				int[] item236 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item236);
				(short, BuildingBlockData) buildingData3 = Collect(context, item235, item236);
				return GameData.Serializer.Serializer.Serialize(buildingData3, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 159:
		{
			int argsCount92 = operation.ArgsCount;
			int num92 = argsCount92;
			if (num92 == 2)
			{
				short item230 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item230);
				int item231 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item231);
				bool item232 = UnsetFulongChicken(context, item230, item231);
				return GameData.Serializer.Serializer.Serialize(item232, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 160:
		{
			int argsCount89 = operation.ArgsCount;
			int num89 = argsCount89;
			if (num89 == 2)
			{
				short item223 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item223);
				int item224 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item224);
				bool item225 = SetFulongChicken(context, item223, item224);
				return GameData.Serializer.Serializer.Serialize(item225, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 161:
		{
			int argsCount86 = operation.ArgsCount;
			int num86 = argsCount86;
			if (num86 == 1)
			{
				List<int> item218 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item218);
				List<Chicken> chickenDataList = GetChickenDataList(item218);
				return GameData.Serializer.Serializer.Serialize(chickenDataList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 162:
		{
			int argsCount84 = operation.ArgsCount;
			int num84 = argsCount84;
			if (num84 == 1)
			{
				List<int> item216 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item216);
				List<string> chickenNicknameList = GetChickenNicknameList(item216);
				return GameData.Serializer.Serializer.Serialize(chickenNicknameList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 163:
		{
			int argsCount82 = operation.ArgsCount;
			int num82 = argsCount82;
			if (num82 == 1)
			{
				Location item213 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item213);
				List<int> settlementChickenIdList = GetSettlementChickenIdList(item213);
				return GameData.Serializer.Serializer.Serialize(settlementChickenIdList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 164:
		{
			int argsCount79 = operation.ArgsCount;
			int num79 = argsCount79;
			if (num79 == 1)
			{
				Location item205 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item205);
				List<string> chickensNicknameByLocation = GetChickensNicknameByLocation(item205);
				return GameData.Serializer.Serializer.Serialize(chickensNicknameByLocation, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 165:
			if (operation.ArgsCount == 0)
			{
				bool item200 = AllChickenInTaiwuVillage(context);
				return GameData.Serializer.Serializer.Serialize(item200, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 166:
			if (operation.ArgsCount == 0)
			{
				List<bool> villagerRoleExtraEffectUnlockState = GetVillagerRoleExtraEffectUnlockState();
				return GameData.Serializer.Serializer.Serialize(villagerRoleExtraEffectUnlockState, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 167:
			if (operation.ArgsCount == 0)
			{
				bool item190 = ClickChickenMap(context);
				return GameData.Serializer.Serializer.Serialize(item190, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 168:
		{
			int argsCount71 = operation.ArgsCount;
			int num71 = argsCount71;
			if (num71 == 1)
			{
				int item187 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item187);
				ClickChickenSign(context, item187);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 169:
			if (operation.ArgsCount == 0)
			{
				bool item181 = IsInFulongSeekFeatherTask(context);
				return GameData.Serializer.Serializer.Serialize(item181, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 170:
		{
			int argsCount69 = operation.ArgsCount;
			int num69 = argsCount69;
			if (num69 == 2)
			{
				int item179 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item179);
				BuildingResourceOutputSetting item180 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item180);
				SetBuildingResourceOutputSetting(context, item179, item180);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 171:
		{
			int argsCount66 = operation.ArgsCount;
			int num66 = argsCount66;
			if (num66 == 1)
			{
				int item168 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item168);
				BuildingResourceOutputSetting buildingResourceOutputSetting = GetBuildingResourceOutputSetting(item168);
				return GameData.Serializer.Serializer.Serialize(buildingResourceOutputSetting, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 172:
			if (operation.ArgsCount == 0)
			{
				BuildingExceptionData buildingExceptionData = GetBuildingExceptionData();
				return GameData.Serializer.Serializer.Serialize(buildingExceptionData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 173:
		{
			int argsCount63 = operation.ArgsCount;
			int num63 = argsCount63;
			if (num63 == 1)
			{
				BuildingBlockKey item156 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item156);
				bool item157 = AllDependBuildingAvailable(item156);
				return GameData.Serializer.Serializer.Serialize(item157, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 174:
		{
			int argsCount60 = operation.ArgsCount;
			int num60 = argsCount60;
			if (num60 == 4)
			{
				BuildingBlockKey item143 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item143);
				short item144 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item144);
				int item145 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item145);
				int item146 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item146);
				int item147 = PracticingCombatSkillInPracticeRoom(context, item143, item144, item145, item146);
				return GameData.Serializer.Serializer.Serialize(item147, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 175:
		{
			int argsCount58 = operation.ArgsCount;
			int num58 = argsCount58;
			if (num58 == 1)
			{
				BuildingBlockKey item124 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item124);
				bool item125 = HasShopManagerLeader(item124);
				return GameData.Serializer.Serializer.Serialize(item125, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 176:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				BuildingBlockKey item122 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item122);
				List<int> item123 = QuickArrangeShopManager(context, item122);
				return GameData.Serializer.Serializer.Serialize(item123, returnDataPool);
			}
			case 2:
			{
				BuildingBlockKey item119 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item119);
				bool item120 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item120);
				List<int> item121 = QuickArrangeShopManager(context, item119, item120);
				return GameData.Serializer.Serializer.Serialize(item121, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 177:
		{
			int argsCount55 = operation.ArgsCount;
			int num55 = argsCount55;
			if (num55 == 3)
			{
				short item112 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item112);
				BuildingBlockKey item113 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item113);
				sbyte item114 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item114);
				List<int> item115 = QuickArrangeBuildOperator(context, item112, item113, item114);
				return GameData.Serializer.Serializer.Serialize(item115, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 178:
		{
			int argsCount52 = operation.ArgsCount;
			int num52 = argsCount52;
			if (num52 == 1)
			{
				BuildingBlockKey item107 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item107);
				bool item108 = ShopBuildingCanTeach(item107);
				return GameData.Serializer.Serializer.Serialize(item108, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 179:
		{
			int argsCount50 = operation.ArgsCount;
			int num50 = argsCount50;
			if (num50 == 4)
			{
				short item98 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item98);
				BuildingBlockKey item99 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item99);
				sbyte item100 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item100);
				List<int> item101 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item101);
				int operationLeftTime = GetOperationLeftTime(context, item98, item99, item100, item101);
				return GameData.Serializer.Serializer.Serialize(operationLeftTime, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 180:
		{
			int argsCount47 = operation.ArgsCount;
			int num47 = argsCount47;
			if (num47 == 2)
			{
				BuildingBlockKey item94 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item94);
				sbyte item95 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item95);
				int buildingOperationLeftTime = GetBuildingOperationLeftTime(context, item94, item95);
				return GameData.Serializer.Serializer.Serialize(buildingOperationLeftTime, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 181:
		{
			int argsCount44 = operation.ArgsCount;
			int num44 = argsCount44;
			if (num44 == 2)
			{
				BuildingBlockKey item88 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item88);
				int item89 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item89);
				ShopBuildingTeachBookData shopBuildingTeachBookData = GetShopBuildingTeachBookData(item88, item89);
				return GameData.Serializer.Serializer.Serialize(shopBuildingTeachBookData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 182:
			if (operation.ArgsCount == 0)
			{
				int item79 = CalcExtraTaiwuGroupMaxCountByStrategyRoom();
				return GameData.Serializer.Serializer.Serialize(item79, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 183:
		{
			int argsCount39 = operation.ArgsCount;
			int num39 = argsCount39;
			if (num39 == 1)
			{
				ItemSourceType item74 = ItemSourceType.Equipment;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item74);
				List<ItemDisplayData> taiwuCanFixBookItemDataList = GetTaiwuCanFixBookItemDataList(item74);
				return GameData.Serializer.Serializer.Serialize(taiwuCanFixBookItemDataList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 184:
			if (operation.ArgsCount == 0)
			{
				(int, int, int) residenceInfo = GetResidenceInfo();
				return GameData.Serializer.Serializer.Serialize(residenceInfo, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 185:
		{
			int argsCount34 = operation.ArgsCount;
			int num34 = argsCount34;
			if (num34 == 1)
			{
				EBuildingScaleEffect item64 = EBuildingScaleEffect.MigrateSpeedBonusFactor;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item64);
				int taiwuVillageResourceBlockEffect = GetTaiwuVillageResourceBlockEffect(context, item64);
				return GameData.Serializer.Serializer.Serialize(taiwuVillageResourceBlockEffect, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 186:
		{
			int argsCount32 = operation.ArgsCount;
			int num32 = argsCount32;
			if (num32 == 1)
			{
				EBuildingScaleEffect item60 = EBuildingScaleEffect.MigrateSpeedBonusFactor;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item60);
				int taiwuLocationResourceBlockEffect = GetTaiwuLocationResourceBlockEffect(context, item60);
				return GameData.Serializer.Serializer.Serialize(taiwuLocationResourceBlockEffect, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 187:
		{
			int argsCount29 = operation.ArgsCount;
			int num29 = argsCount29;
			if (num29 == 1)
			{
				short item56 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item56);
				List<BuildingBlockData> taiwuVillageResourceBlockEffectInfo = GetTaiwuVillageResourceBlockEffectInfo(context, item56);
				return GameData.Serializer.Serializer.Serialize(taiwuVillageResourceBlockEffectInfo, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 188:
		{
			int argsCount26 = operation.ArgsCount;
			int num26 = argsCount26;
			if (num26 == 1)
			{
				BuildingBlockKey item46 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item46);
				bool item47 = CanQuickArrangeShopManager(item46);
				return GameData.Serializer.Serializer.Serialize(item47, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 189:
		{
			int argsCount23 = operation.ArgsCount;
			int num23 = argsCount23;
			if (num23 == 1)
			{
				BuildingBlockKey item42 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item42);
				BuildingFormulaContextBridge buildingFormulaContextBridge = GetBuildingFormulaContextBridge(item42);
				return GameData.Serializer.Serializer.Serialize(buildingFormulaContextBridge, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 190:
		{
			int argsCount20 = operation.ArgsCount;
			int num20 = argsCount20;
			if (num20 == 2)
			{
				BuildingBlockKey item38 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item38);
				sbyte item39 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item39);
				(int, bool) buildingEffectForMake = GetBuildingEffectForMake(item38, item39);
				return GameData.Serializer.Serializer.Serialize(buildingEffectForMake, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 191:
		{
			int argsCount17 = operation.ArgsCount;
			int num17 = argsCount17;
			if (num17 == 3)
			{
				int item29 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item29);
				short item30 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item30);
				int item31 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item31);
				bool item32 = GmCmd_BuildingCollectPerform(context, item29, item30, item31);
				return GameData.Serializer.Serializer.Serialize(item32, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 192:
		{
			int argsCount14 = operation.ArgsCount;
			int num14 = argsCount14;
			if (num14 == 2)
			{
				sbyte item22 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item22);
				int item23 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item23);
				bool item24 = GmCmd_BeatMinionPerform(context, item22, item23);
				return GameData.Serializer.Serializer.Serialize(item24, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 193:
		{
			int argsCount12 = operation.ArgsCount;
			int num12 = argsCount12;
			if (num12 == 1)
			{
				int item20 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item20);
				int storeLocation = GetStoreLocation(item20);
				return GameData.Serializer.Serializer.Serialize(storeLocation, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 194:
		{
			int argsCount9 = operation.ArgsCount;
			int num9 = argsCount9;
			if (num9 == 2)
			{
				int item14 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item14);
				int item15 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item15);
				SetStoreLocation(context, item14, item15);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 195:
		{
			int argsCount6 = operation.ArgsCount;
			int num6 = argsCount6;
			if (num6 == 1)
			{
				BuildingBlockKey item9 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item9);
				List<CharacterDisplayData> feastTargetCharList = GetFeastTargetCharList(context, item9);
				return GameData.Serializer.Serializer.Serialize(feastTargetCharList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 196:
			if (operation.ArgsCount == 0)
			{
				TryShowNotifications();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 197:
		{
			int argsCount4 = operation.ArgsCount;
			int num4 = argsCount4;
			if (num4 == 1)
			{
				BuildingBlockKey item5 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item5);
				QuickRemoveShopSoldItem(context, item5);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 198:
		{
			int argsCount = operation.ArgsCount;
			int num = argsCount;
			if (num == 1)
			{
				BuildingBlockKey item = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item);
				QuickAddShopSoldItem(context, item);
				return -1;
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
			_modificationsBuildingAreas.ChangeRecording(monitoring);
			break;
		case 1:
			_modificationsBuildingBlocks.ChangeRecording(monitoring);
			break;
		case 2:
			break;
		case 3:
			_modificationsCollectBuildingResourceType.ChangeRecording(monitoring);
			break;
		case 4:
			_modificationsBuildingOperatorDict.ChangeRecording(monitoring);
			break;
		case 5:
			_modificationsCustomBuildingName.ChangeRecording(monitoring);
			break;
		case 6:
			break;
		case 7:
			_modificationsChickenBlessingInfoData.ChangeRecording(monitoring);
			break;
		case 8:
			_modificationsChicken.ChangeRecording(monitoring);
			break;
		case 9:
			break;
		case 10:
			break;
		case 11:
			break;
		case 12:
			break;
		case 13:
			break;
		case 14:
			break;
		case 15:
			break;
		case 16:
			break;
		case 17:
			break;
		case 18:
			break;
		case 19:
			break;
		case 20:
			_modificationsSamsaraPlatformBornDict.ChangeRecording(monitoring);
			break;
		case 21:
			_modificationsCollectBuildingEarningsData.ChangeRecording(monitoring);
			break;
		case 22:
			_modificationsShopManagerDict.ChangeRecording(monitoring);
			break;
		case 23:
			break;
		case 24:
			break;
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
	{
		switch (dataId)
		{
		case 0:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 0);
			int result9 = GameData.Serializer.Serializer.SerializeModifications(_buildingAreas, dataPool, _modificationsBuildingAreas);
			_modificationsBuildingAreas.Reset();
			return result9;
		}
		case 1:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 1))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 1);
			int result3 = GameData.Serializer.Serializer.SerializeModifications(_buildingBlocks, dataPool, _modificationsBuildingBlocks);
			_modificationsBuildingBlocks.Reset();
			return result3;
		}
		case 2:
			if (!BaseGameDataDomain.IsModified(DataStates, 2))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 2);
			return GameData.Serializer.Serializer.Serialize(_taiwuBuildingAreas, dataPool);
		case 3:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 3))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 3);
			int result = GameData.Serializer.Serializer.SerializeModifications(_CollectBuildingResourceType, dataPool, _modificationsCollectBuildingResourceType);
			_modificationsCollectBuildingResourceType.Reset();
			return result;
		}
		case 4:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 4))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 4);
			int result5 = GameData.Serializer.Serializer.SerializeModifications(_buildingOperatorDict, dataPool, _modificationsBuildingOperatorDict);
			_modificationsBuildingOperatorDict.Reset();
			return result5;
		}
		case 5:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 5))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 5);
			int result10 = GameData.Serializer.Serializer.SerializeModifications(_customBuildingName, dataPool, _modificationsCustomBuildingName);
			_modificationsCustomBuildingName.Reset();
			return result10;
		}
		case 6:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 7:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 7))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 7);
			int result8 = GameData.Serializer.Serializer.SerializeModifications(_chickenBlessingInfoData, dataPool, _modificationsChickenBlessingInfoData);
			_modificationsChickenBlessingInfoData.Reset();
			return result8;
		}
		case 8:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 8))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 8);
			int result2 = GameData.Serializer.Serializer.SerializeModifications(_chicken, dataPool, _modificationsChicken);
			_modificationsChicken.Reset();
			return result2;
		}
		case 9:
			if (!BaseGameDataDomain.IsModified(DataStates, 9))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 9);
			return GameData.Serializer.Serializer.Serialize(_collectionCrickets, dataPool);
		case 10:
			if (!BaseGameDataDomain.IsModified(DataStates, 10))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 10);
			return GameData.Serializer.Serializer.Serialize(_collectionCricketJars, dataPool);
		case 11:
			if (!BaseGameDataDomain.IsModified(DataStates, 11))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 11);
			return GameData.Serializer.Serializer.Serialize(_collectionCricketRegen, dataPool);
		case 12:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 13:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 14:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 15:
			if (!BaseGameDataDomain.IsModified(DataStates, 15))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 15);
			return GameData.Serializer.Serializer.Serialize(_homeless, dataPool);
		case 16:
			if (!BaseGameDataDomain.IsModified(DataStates, 16))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 16);
			return GameData.Serializer.Serializer.Serialize(_samsaraPlatformAddMainAttributes, dataPool);
		case 17:
			if (!BaseGameDataDomain.IsModified(DataStates, 17))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 17);
			return GameData.Serializer.Serializer.Serialize(_samsaraPlatformAddCombatSkillQualifications, dataPool);
		case 18:
			if (!BaseGameDataDomain.IsModified(DataStates, 18))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 18);
			return GameData.Serializer.Serializer.Serialize(_samsaraPlatformAddLifeSkillQualifications, dataPool);
		case 19:
			if (!BaseGameDataDomain.IsModified(_dataStatesSamsaraPlatformSlots, (int)subId0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(_dataStatesSamsaraPlatformSlots, (int)subId0);
			return GameData.Serializer.Serializer.Serialize(_samsaraPlatformSlots[(uint)subId0], dataPool);
		case 20:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 20))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 20);
			int result7 = GameData.Serializer.Serializer.SerializeModifications(_samsaraPlatformBornDict, dataPool, _modificationsSamsaraPlatformBornDict);
			_modificationsSamsaraPlatformBornDict.Reset();
			return result7;
		}
		case 21:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 21))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 21);
			int result6 = GameData.Serializer.Serializer.SerializeModifications(_collectBuildingEarningsData, dataPool, _modificationsCollectBuildingEarningsData);
			_modificationsCollectBuildingEarningsData.Reset();
			return result6;
		}
		case 22:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 22))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 22);
			int result4 = GameData.Serializer.Serializer.SerializeModifications(_shopManagerDict, dataPool, _modificationsShopManagerDict);
			_modificationsShopManagerDict.Reset();
			return result4;
		}
		case 23:
			if (!BaseGameDataDomain.IsModified(DataStates, 23))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 23);
			return GameData.Serializer.Serializer.Serialize(_teaHorseCaravanData, dataPool);
		case 24:
			if (!BaseGameDataDomain.IsModified(DataStates, 24))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 24);
			return GameData.Serializer.Serializer.Serialize(_shrineBuyTimes, dataPool);
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		switch (dataId)
		{
		case 0:
			if (BaseGameDataDomain.IsModified(DataStates, 0))
			{
				BaseGameDataDomain.ResetModified(DataStates, 0);
				_modificationsBuildingAreas.Reset();
			}
			break;
		case 1:
			if (BaseGameDataDomain.IsModified(DataStates, 1))
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
				_modificationsBuildingBlocks.Reset();
			}
			break;
		case 2:
			if (BaseGameDataDomain.IsModified(DataStates, 2))
			{
				BaseGameDataDomain.ResetModified(DataStates, 2);
			}
			break;
		case 3:
			if (BaseGameDataDomain.IsModified(DataStates, 3))
			{
				BaseGameDataDomain.ResetModified(DataStates, 3);
				_modificationsCollectBuildingResourceType.Reset();
			}
			break;
		case 4:
			if (BaseGameDataDomain.IsModified(DataStates, 4))
			{
				BaseGameDataDomain.ResetModified(DataStates, 4);
				_modificationsBuildingOperatorDict.Reset();
			}
			break;
		case 5:
			if (BaseGameDataDomain.IsModified(DataStates, 5))
			{
				BaseGameDataDomain.ResetModified(DataStates, 5);
				_modificationsCustomBuildingName.Reset();
			}
			break;
		case 6:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 7:
			if (BaseGameDataDomain.IsModified(DataStates, 7))
			{
				BaseGameDataDomain.ResetModified(DataStates, 7);
				_modificationsChickenBlessingInfoData.Reset();
			}
			break;
		case 8:
			if (BaseGameDataDomain.IsModified(DataStates, 8))
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
				_modificationsChicken.Reset();
			}
			break;
		case 9:
			if (BaseGameDataDomain.IsModified(DataStates, 9))
			{
				BaseGameDataDomain.ResetModified(DataStates, 9);
			}
			break;
		case 10:
			if (BaseGameDataDomain.IsModified(DataStates, 10))
			{
				BaseGameDataDomain.ResetModified(DataStates, 10);
			}
			break;
		case 11:
			if (BaseGameDataDomain.IsModified(DataStates, 11))
			{
				BaseGameDataDomain.ResetModified(DataStates, 11);
			}
			break;
		case 12:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 13:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 14:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 15:
			if (BaseGameDataDomain.IsModified(DataStates, 15))
			{
				BaseGameDataDomain.ResetModified(DataStates, 15);
			}
			break;
		case 16:
			if (BaseGameDataDomain.IsModified(DataStates, 16))
			{
				BaseGameDataDomain.ResetModified(DataStates, 16);
			}
			break;
		case 17:
			if (BaseGameDataDomain.IsModified(DataStates, 17))
			{
				BaseGameDataDomain.ResetModified(DataStates, 17);
			}
			break;
		case 18:
			if (BaseGameDataDomain.IsModified(DataStates, 18))
			{
				BaseGameDataDomain.ResetModified(DataStates, 18);
			}
			break;
		case 19:
			if (BaseGameDataDomain.IsModified(_dataStatesSamsaraPlatformSlots, (int)subId0))
			{
				BaseGameDataDomain.ResetModified(_dataStatesSamsaraPlatformSlots, (int)subId0);
			}
			break;
		case 20:
			if (BaseGameDataDomain.IsModified(DataStates, 20))
			{
				BaseGameDataDomain.ResetModified(DataStates, 20);
				_modificationsSamsaraPlatformBornDict.Reset();
			}
			break;
		case 21:
			if (BaseGameDataDomain.IsModified(DataStates, 21))
			{
				BaseGameDataDomain.ResetModified(DataStates, 21);
				_modificationsCollectBuildingEarningsData.Reset();
			}
			break;
		case 22:
			if (BaseGameDataDomain.IsModified(DataStates, 22))
			{
				BaseGameDataDomain.ResetModified(DataStates, 22);
				_modificationsShopManagerDict.Reset();
			}
			break;
		case 23:
			if (BaseGameDataDomain.IsModified(DataStates, 23))
			{
				BaseGameDataDomain.ResetModified(DataStates, 23);
			}
			break;
		case 24:
			if (BaseGameDataDomain.IsModified(DataStates, 24))
			{
				BaseGameDataDomain.ResetModified(DataStates, 24);
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
			0 => BaseGameDataDomain.IsModified(DataStates, 0), 
			1 => BaseGameDataDomain.IsModified(DataStates, 1), 
			2 => BaseGameDataDomain.IsModified(DataStates, 2), 
			3 => BaseGameDataDomain.IsModified(DataStates, 3), 
			4 => BaseGameDataDomain.IsModified(DataStates, 4), 
			5 => BaseGameDataDomain.IsModified(DataStates, 5), 
			6 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			7 => BaseGameDataDomain.IsModified(DataStates, 7), 
			8 => BaseGameDataDomain.IsModified(DataStates, 8), 
			9 => BaseGameDataDomain.IsModified(DataStates, 9), 
			10 => BaseGameDataDomain.IsModified(DataStates, 10), 
			11 => BaseGameDataDomain.IsModified(DataStates, 11), 
			12 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			13 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			14 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			15 => BaseGameDataDomain.IsModified(DataStates, 15), 
			16 => BaseGameDataDomain.IsModified(DataStates, 16), 
			17 => BaseGameDataDomain.IsModified(DataStates, 17), 
			18 => BaseGameDataDomain.IsModified(DataStates, 18), 
			19 => BaseGameDataDomain.IsModified(_dataStatesSamsaraPlatformSlots, (int)subId0), 
			20 => BaseGameDataDomain.IsModified(DataStates, 20), 
			21 => BaseGameDataDomain.IsModified(DataStates, 21), 
			22 => BaseGameDataDomain.IsModified(DataStates, 22), 
			23 => BaseGameDataDomain.IsModified(DataStates, 23), 
			24 => BaseGameDataDomain.IsModified(DataStates, 24), 
			_ => throw new Exception($"Unsupported dataId {dataId}"), 
		};
	}

	public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
	{
		switch (influence.TargetIndicator.DataId)
		{
		default:
			throw new Exception($"Unsupported dataId {influence.TargetIndicator.DataId}");
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
		case 20:
		case 21:
		case 22:
		case 23:
		case 24:
			throw new Exception($"Cannot invalidate cache state of non-cache data {influence.TargetIndicator.DataId}");
		}
	}

	public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		uint num;
		switch (operation.DataId)
		{
		case 0:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Ref(operation, pResult, _buildingAreas, 2);
			goto IL_026f;
		case 1:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Ref(operation, pResult, _buildingBlocks, 16);
			goto IL_026f;
		case 2:
			ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_List<Location>(operation, pResult, _taiwuBuildingAreas, 4);
			goto IL_026f;
		case 3:
			ResponseProcessor.ProcessSingleValueCollection_BasicType_Fixed_Value(operation, pResult, _CollectBuildingResourceType);
			goto IL_026f;
		case 5:
			ResponseProcessor.ProcessSingleValueCollection_BasicType_Fixed_Value(operation, pResult, _customBuildingName);
			goto IL_026f;
		case 6:
			ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_List<BuildingBlockKey>(operation, pResult, _newCompleteOperationBuildings, 8);
			goto IL_026f;
		case 7:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Value<int, ChickenBlessingInfoData>(operation, pResult, (IDictionary<int, ChickenBlessingInfoData>)_chickenBlessingInfoData);
			goto IL_026f;
		case 8:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Value<int, Chicken>(operation, pResult, (IDictionary<int, Chicken>)_chicken, 12);
			goto IL_026f;
		case 9:
			ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Array<ItemKey>(operation, pResult, _collectionCrickets, 15, 8);
			goto IL_026f;
		case 10:
			ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Array<ItemKey>(operation, pResult, _collectionCricketJars, 15, 8);
			goto IL_026f;
		case 11:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Array(operation, pResult, _collectionCricketRegen, 15);
			goto IL_026f;
		case 12:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _makeItemDict);
			goto IL_026f;
		case 13:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Value<BuildingBlockKey, CharacterList>(operation, pResult, (IDictionary<BuildingBlockKey, CharacterList>)_residences);
			goto IL_026f;
		case 14:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Value<BuildingBlockKey, CharacterList>(operation, pResult, (IDictionary<BuildingBlockKey, CharacterList>)_comfortableHouses);
			goto IL_026f;
		case 15:
			ResponseProcessor.ProcessSingleValue_CustomType_Dynamic_Value_Single<CharacterList>(operation, pResult, ref _homeless);
			goto IL_026f;
		case 16:
			ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Single<MainAttributes>(operation, pResult, ref _samsaraPlatformAddMainAttributes);
			goto IL_026f;
		case 17:
			ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Single<CombatSkillShorts>(operation, pResult, ref _samsaraPlatformAddCombatSkillQualifications);
			goto IL_026f;
		case 18:
			ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Single<LifeSkillShorts>(operation, pResult, ref _samsaraPlatformAddLifeSkillQualifications);
			goto IL_026f;
		case 19:
			ResponseProcessor.ProcessElementList_CustomType_Fixed_Value<IntPair>(operation, pResult, _samsaraPlatformSlots, 6, 8);
			goto IL_026f;
		case 20:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Value<int, IntPair>(operation, pResult, (IDictionary<int, IntPair>)_samsaraPlatformBornDict, 8);
			goto IL_026f;
		case 21:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _collectBuildingEarningsData);
			goto IL_026f;
		case 23:
			ResponseProcessor.ProcessSingleValue_CustomType_Dynamic_Ref_Single(operation, pResult, _teaHorseCaravanData);
			goto IL_026f;
		case 24:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _shrineBuyTimes);
			goto IL_026f;
		default:
			throw new Exception($"Unsupported dataId {operation.DataId}");
		case 4:
		case 22:
			{
				throw new Exception($"Cannot process archive response of non-archive data {operation.DataId}");
			}
			IL_026f:
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
					DomainManager.Global.CompleteLoading(9);
				}
			}
			break;
		}
	}

	private void InitializeInternalDataOfCollections()
	{
	}

	[DomainMethod]
	[Obsolete]
	public List<ShopEventData> GetShopEventDataList(BuildingBlockKey blockKey)
	{
		if (_shopEventDict == null)
		{
			_shopEventDict = new Dictionary<BuildingBlockKey, List<ShopEventData>>();
		}
		if (_shopEventDict.TryGetValue(blockKey, out var value))
		{
			return value;
		}
		List<ShopEventData> list = new List<ShopEventData>();
		_shopEventDict.Add(blockKey, list);
		return list;
	}
}
