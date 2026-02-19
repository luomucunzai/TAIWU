using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Domains.Item;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Map;

public class MapPickupGenerator
{
	private class GenerateRequest
	{
		public MapPickupsItem PickupConfig;

		public int XiangshuLevelIndex;
	}

	private const int BlockTypeCount = 4;

	private readonly EMapBlockType[] _mapBlockTypes = new EMapBlockType[4]
	{
		EMapBlockType.City,
		EMapBlockType.Sect,
		EMapBlockType.Town,
		EMapBlockType.Invalid
	};

	private static readonly Dictionary<short, HashSet<short>> _cachedConfigBlockSet = new Dictionary<short, HashSet<short>>();

	private static readonly Dictionary<sbyte, IList<int>> _cacheItemTypeToAllIdDict = new Dictionary<sbyte, IList<int>>();

	private static int BlockTypeToIndex(EMapBlockType blockType)
	{
		if (1 == 0)
		{
		}
		int result = blockType switch
		{
			EMapBlockType.City => 0, 
			EMapBlockType.Sect => 1, 
			EMapBlockType.Town => 2, 
			_ => 3, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public void Generate(IRandomSource random, Dictionary<Location, MapPickupCollection> outDict)
	{
		PreGenerate();
		outDict.Clear();
		Dictionary<int, Dictionary<sbyte, List<GenerateRequest>>> dictionary = CollectRequestsByState();
		foreach (var (stateId, xiangshuLevelRequestDict) in dictionary)
		{
			GeneratePerState(random, outDict, stateId, xiangshuLevelRequestDict);
		}
		Dictionary<short, List<GenerateRequest>> dictionary3 = CollectSpecialRequests();
		foreach (var (areaId, reqList) in dictionary3)
		{
			GenerateForSpecialArea(random, areaId, reqList, outDict);
		}
		PostGenerate();
	}

	private void PreGenerate()
	{
		ClearCaches();
	}

	private void PostGenerate()
	{
		ClearCaches();
	}

	private void ClearCaches()
	{
		_cachedConfigBlockSet.Clear();
		_cacheItemTypeToAllIdDict.Clear();
	}

	private static Dictionary<int, Dictionary<sbyte, List<GenerateRequest>>> CollectRequestsByState()
	{
		Dictionary<int, Dictionary<sbyte, List<GenerateRequest>>> dictionary = new Dictionary<int, Dictionary<sbyte, List<GenerateRequest>>>();
		foreach (MapPickupsItem item in (IEnumerable<MapPickupsItem>)MapPickups.Instance)
		{
			if (item.Type == EMapPickupsType.Event)
			{
				continue;
			}
			for (int i = 0; i < item.StateTimes.Length; i++)
			{
				byte b = item.StateTimes[i];
				if (b <= 0)
				{
					continue;
				}
				for (int j = 0; j < item.XiangshuLevel.Length; j++)
				{
					sbyte key = item.XiangshuLevel[j];
					if (!dictionary.TryGetValue(i, out var value))
					{
						value = (dictionary[i] = new Dictionary<sbyte, List<GenerateRequest>>());
					}
					if (!value.TryGetValue(key, out var value2))
					{
						value2 = (value[key] = new List<GenerateRequest>());
					}
					for (int k = 0; k < b; k++)
					{
						value2.Add(new GenerateRequest
						{
							PickupConfig = item,
							XiangshuLevelIndex = j
						});
					}
				}
			}
		}
		return dictionary;
	}

	private static Dictionary<short, List<GenerateRequest>> CollectSpecialRequests()
	{
		Dictionary<short, List<GenerateRequest>> dictionary = new Dictionary<short, List<GenerateRequest>>();
		short[] array = new short[3] { 135, 137, 138 };
		short[] array2 = array;
		foreach (short key in array2)
		{
			List<GenerateRequest> list = (dictionary[key] = new List<GenerateRequest>());
			foreach (MapPickupsItem item in (IEnumerable<MapPickupsItem>)MapPickups.Instance)
			{
				if (item.Type == EMapPickupsType.Event)
				{
					continue;
				}
				byte specialAreaTimes = item.SpecialAreaTimes;
				if (specialAreaTimes <= 0 || ((IEnumerable<sbyte>)item.XiangshuLevel).Contains((sbyte)0))
				{
					for (int j = 0; j < specialAreaTimes; j++)
					{
						list.Add(new GenerateRequest
						{
							PickupConfig = item,
							XiangshuLevelIndex = 0
						});
					}
				}
			}
		}
		return dictionary;
	}

	private void GeneratePerState(IRandomSource random, Dictionary<Location, MapPickupCollection> outDict, int stateId, Dictionary<sbyte, List<GenerateRequest>> xiangshuLevelRequestDict)
	{
		List<MapBlockData>[] shuffledGroupedBlocksInState = GetShuffledGroupedBlocksInState(random, stateId);
		Dictionary<EMapBlockType, int> areaLoad = new Dictionary<EMapBlockType, int>();
		Dictionary<Location, int> locationLoad = new Dictionary<Location, int>();
		foreach (KeyValuePair<sbyte, List<GenerateRequest>> item in xiangshuLevelRequestDict)
		{
			List<GenerateRequest> value = item.Value;
			CollectionUtils.Shuffle(random, value);
			foreach (GenerateRequest item2 in value)
			{
				GeneratePerRequest(random, outDict, item2, shuffledGroupedBlocksInState, areaLoad, locationLoad);
			}
		}
	}

	private void GenerateForSpecialArea(IRandomSource random, short areaId, List<GenerateRequest> reqList, Dictionary<Location, MapPickupCollection> outDict)
	{
		List<MapBlockData> shuffledBlocksInArea = GetShuffledBlocksInArea(random, areaId);
		Dictionary<Location, int> locationLoad = new Dictionary<Location, int>();
		foreach (GenerateRequest req in reqList)
		{
			GeneratePerSpecialRequest(random, outDict, req, shuffledBlocksInArea, locationLoad);
		}
	}

	private void GeneratePerRequest(IRandomSource random, Dictionary<Location, MapPickupCollection> outDict, GenerateRequest req, List<MapBlockData>[] areaBlocks, Dictionary<EMapBlockType, int> areaLoad, Dictionary<Location, int> locationLoad)
	{
		MapPickupsItem pickupConfig = req.PickupConfig;
		Dictionary<EMapBlockType, List<MapBlockData>> validAreaBlocks = GetValidAreaBlocks(areaBlocks, pickupConfig);
		if (validAreaBlocks.Any())
		{
			EMapBlockType key = validAreaBlocks.Keys.OrderBy((EMapBlockType aid) => areaLoad.GetValueOrDefault(aid, 0)).First();
			List<MapBlockData> sourceList = validAreaBlocks[key];
			MapBlockData mapBlockData = PickBlockByLoad(sourceList, locationLoad, random);
			Location location = mapBlockData.GetLocation();
			locationLoad[location] = locationLoad.GetValueOrDefault(location, 0) + 1;
			MapPickup mapPickup = GenerateOnePickup(pickupConfig, req.XiangshuLevelIndex, random, needXiangshuMinion: true);
			mapPickup.Location = location;
			AddPickupToResult(outDict, mapPickup);
			areaLoad.TryAdd(key, 0);
			areaLoad[key]++;
		}
	}

	private void GeneratePerSpecialRequest(IRandomSource random, Dictionary<Location, MapPickupCollection> outDict, GenerateRequest req, List<MapBlockData> blockList, Dictionary<Location, int> locationLoad)
	{
		MapPickupsItem pickupConfig = req.PickupConfig;
		if (!_cachedConfigBlockSet.TryGetValue(pickupConfig.TemplateId, out var blockSet))
		{
			blockSet = pickupConfig.BlockList.ToHashSet();
		}
		MapBlockData mapBlockData = PickBlockByLoad(blockList.Where((MapBlockData b) => blockSet.Contains(b.TemplateId)).ToList(), locationLoad, random);
		if (mapBlockData != null)
		{
			Location location = mapBlockData.GetLocation();
			locationLoad[location] = locationLoad.GetValueOrDefault(location, 0) + 1;
			MapPickup mapPickup = GenerateOnePickup(pickupConfig, req.XiangshuLevelIndex, random, needXiangshuMinion: false);
			mapPickup.Location = location;
			AddPickupToResult(outDict, mapPickup);
		}
	}

	private Dictionary<EMapBlockType, List<MapBlockData>> GetValidAreaBlocks(List<MapBlockData>[] areaBlocks, MapPickupsItem config)
	{
		if (!_cachedConfigBlockSet.TryGetValue(config.TemplateId, out var value))
		{
			value = config.BlockList.ToHashSet();
		}
		Dictionary<EMapBlockType, List<MapBlockData>> dictionary = new Dictionary<EMapBlockType, List<MapBlockData>>();
		for (int i = 0; i < 4; i++)
		{
			EMapBlockType key = _mapBlockTypes[i];
			List<MapBlockData> list = areaBlocks[i];
			List<MapBlockData> list2 = new List<MapBlockData>();
			foreach (MapBlockData item in list)
			{
				if (value.Contains(item.TemplateId))
				{
					list2.Add(item);
				}
			}
			if (list2.Any())
			{
				dictionary[key] = list2;
			}
		}
		return dictionary;
	}

	private static List<MapBlockData>[] GetShuffledGroupedBlocksInState(IRandomSource random, int stateId)
	{
		List<MapBlockData>[] array = new List<MapBlockData>[4];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new List<MapBlockData>();
		}
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		DomainManager.Map.GetAllAreaInState((sbyte)stateId, list);
		foreach (short item2 in list)
		{
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(item2);
			MapAreaItem config = DomainManager.Map.GetAreaByAreaId(item2).GetConfig();
			EMapBlockType areaType = GetAreaType(config);
			List<MapBlockData> list2 = array[BlockTypeToIndex(areaType)];
			Span<MapBlockData> span = areaBlocks;
			for (int j = 0; j < span.Length; j++)
			{
				MapBlockData item = span[j];
				list2.Add(item);
			}
		}
		List<MapBlockData>[] array2 = array;
		foreach (List<MapBlockData> list3 in array2)
		{
			CollectionUtils.Shuffle(random, list3);
		}
		ObjectPool<List<short>>.Instance.Return(list);
		return array;
	}

	private static List<MapBlockData> GetShuffledBlocksInArea(IRandomSource random, short areaId)
	{
		List<MapBlockData> list = new List<MapBlockData>();
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		Span<MapBlockData> span = areaBlocks;
		for (int i = 0; i < span.Length; i++)
		{
			MapBlockData item = span[i];
			list.Add(item);
		}
		CollectionUtils.Shuffle(random, list);
		return list;
	}

	private static void AddPickupToResult(Dictionary<Location, MapPickupCollection> outDict, MapPickup pickup)
	{
		if (!outDict.TryGetValue(pickup.Location, out var value))
		{
			value = new MapPickupCollection();
			outDict.Add(pickup.Location, value);
		}
		value.AddPickup(pickup);
	}

	public static EMapBlockType GetAreaType(MapAreaItem areaConfig)
	{
		short[] settlementBlockCore = areaConfig.SettlementBlockCore;
		short[] array = settlementBlockCore;
		foreach (short index in array)
		{
			MapBlockItem mapBlockItem = MapBlock.Instance[index];
			switch (mapBlockItem.Type)
			{
			case EMapBlockType.City:
				return EMapBlockType.City;
			case EMapBlockType.Sect:
				return EMapBlockType.Sect;
			case EMapBlockType.Town:
				return EMapBlockType.Town;
			}
		}
		return EMapBlockType.Invalid;
	}

	private static MapPickup GenerateOnePickup(MapPickupsItem pickupConfig, int xiangshuLevelIndex, IRandomSource random, bool needXiangshuMinion)
	{
		Location invalid = Location.Invalid;
		sbyte xiangshuProgress = pickupConfig.XiangshuLevel[xiangshuLevelIndex];
		if (pickupConfig.Type == EMapPickupsType.Event)
		{
			throw new Exception("Event pickup type is invalid for disabled.");
		}
		bool hasXiangshuMinion = needXiangshuMinion && random.CheckPercentProb(GlobalConfig.Instance.MapPickupHasXiangshuMinionProbability);
		if (pickupConfig.BonusCount.Length != 0)
		{
			int num = pickupConfig.BonusCount[xiangshuLevelIndex];
			short mapPickupResourceCountRandomFactor = GlobalConfig.Instance.MapPickupResourceCountRandomFactor;
			int num2 = num * (100 - mapPickupResourceCountRandomFactor) / 100;
			int num3 = num * (100 + mapPickupResourceCountRandomFactor) / 100;
			int num4 = random.Next(num2, num3);
			if (pickupConfig.IsExpBonus)
			{
				return MapPickup.CreateExpBonus(invalid, pickupConfig.TemplateId, num4, xiangshuProgress, hasXiangshuMinion);
			}
			if (pickupConfig.IsDebtBonus)
			{
				return MapPickup.CreateDebtBonus(invalid, pickupConfig.TemplateId, num4, xiangshuProgress, hasXiangshuMinion);
			}
			return MapPickup.CreateResource(invalid, pickupConfig.TemplateId, num4, xiangshuProgress, hasXiangshuMinion);
		}
		if (pickupConfig.ItemGrade.Length != 0)
		{
			sbyte baseGrade = pickupConfig.ItemGrade[xiangshuLevelIndex];
			short itemTemplateId = RandomPickItemWithBaseGrade(random, pickupConfig, baseGrade);
			return MapPickup.CreateItem(invalid, pickupConfig.TemplateId, pickupConfig.ItemGroup.ItemType, itemTemplateId, xiangshuProgress, hasXiangshuMinion);
		}
		if (pickupConfig.LoopEffect)
		{
			return MapPickup.CreateLoopEffect(invalid, pickupConfig.TemplateId, xiangshuProgress, hasXiangshuMinion);
		}
		if (pickupConfig.ReadEffect)
		{
			return MapPickup.CreateReadEffect(invalid, pickupConfig.TemplateId, xiangshuProgress, hasXiangshuMinion);
		}
		throw new ArgumentException("Invalid pickup config");
	}

	private static short RandomPickItemWithBaseGrade(IRandomSource random, MapPickupsItem pickupConfig, sbyte baseGrade)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		byte mapPickupItemGradeRandomFactor = GlobalConfig.Instance.MapPickupItemGradeRandomFactor;
		sbyte itemType = pickupConfig.ItemGroup.ItemType;
		short groupId = pickupConfig.ItemGroup.TemplateId;
		if (!_cacheItemTypeToAllIdDict.TryGetValue(itemType, out var value))
		{
			value = ItemTemplateHelper.GetTemplateDataAllKeys(itemType);
			_cacheItemTypeToAllIdDict[itemType] = value;
		}
		sbyte exceptedMinGrade = (sbyte)(baseGrade - mapPickupItemGradeRandomFactor);
		sbyte exceptedMaxGrade = baseGrade;
		list.AddRange(from tid in value
			where ItemTemplateHelper.GetGroupId(itemType, (short)tid) == groupId && ItemTemplateHelper.GetGrade(itemType, (short)tid) >= exceptedMinGrade && ItemTemplateHelper.GetGrade(itemType, (short)tid) <= exceptedMaxGrade
			select (short)tid);
		if (list.Count == 0)
		{
			throw new ArgumentException($"Invalid pickup config {pickupConfig.TemplateId} {pickupConfig.Name}. No item found in group {groupId} with grade between {exceptedMinGrade} and {exceptedMaxGrade}. itemType: {itemType}.");
		}
		int index = random.Next(0, list.Count);
		short result = list[index];
		ObjectPool<List<short>>.Instance.Return(list);
		return result;
	}

	private static MapBlockData PickBlockByLoad(IList<MapBlockData> sourceList, Dictionary<Location, int> locationLoad, IRandomSource random)
	{
		if (sourceList == null || sourceList.Count == 0)
		{
			return null;
		}
		int[] array = new int[sourceList.Count];
		int num = 0;
		for (int i = 0; i < sourceList.Count; i++)
		{
			Location location = sourceList[i].GetLocation();
			int valueOrDefault = locationLoad.GetValueOrDefault(location, 0);
			int num2 = 1 << 7 - Math.Min(valueOrDefault, 7);
			num = (array[i] = num + num2);
		}
		int value = random.Next(0, num);
		int num3 = Array.BinarySearch(array, value);
		if (num3 < 0)
		{
			num3 = ~num3;
		}
		return sourceList[num3];
	}
}
