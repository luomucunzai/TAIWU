using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using GameData.Domains.Item;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Map
{
	// Token: 0x020008B9 RID: 2233
	public class MapPickupGenerator
	{
		// Token: 0x06007BA8 RID: 31656 RVA: 0x0048E8E4 File Offset: 0x0048CAE4
		private static int BlockTypeToIndex(EMapBlockType blockType)
		{
			if (!true)
			{
			}
			int result;
			switch (blockType)
			{
			case EMapBlockType.City:
				result = 0;
				break;
			case EMapBlockType.Sect:
				result = 1;
				break;
			case EMapBlockType.Town:
				result = 2;
				break;
			default:
				result = 3;
				break;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06007BA9 RID: 31657 RVA: 0x0048E924 File Offset: 0x0048CB24
		public void Generate(IRandomSource random, Dictionary<Location, MapPickupCollection> outDict)
		{
			this.PreGenerate();
			outDict.Clear();
			Dictionary<int, Dictionary<sbyte, List<MapPickupGenerator.GenerateRequest>>> requestsByState = MapPickupGenerator.CollectRequestsByState();
			foreach (KeyValuePair<int, Dictionary<sbyte, List<MapPickupGenerator.GenerateRequest>>> keyValuePair in requestsByState)
			{
				int num;
				Dictionary<sbyte, List<MapPickupGenerator.GenerateRequest>> dictionary;
				keyValuePair.Deconstruct(out num, out dictionary);
				int stateId = num;
				Dictionary<sbyte, List<MapPickupGenerator.GenerateRequest>> xiangshuLevelRequestDict = dictionary;
				this.GeneratePerState(random, outDict, stateId, xiangshuLevelRequestDict);
			}
			Dictionary<short, List<MapPickupGenerator.GenerateRequest>> specialAreaRequestDict = MapPickupGenerator.CollectSpecialRequests();
			foreach (KeyValuePair<short, List<MapPickupGenerator.GenerateRequest>> keyValuePair2 in specialAreaRequestDict)
			{
				short num2;
				List<MapPickupGenerator.GenerateRequest> list;
				keyValuePair2.Deconstruct(out num2, out list);
				short areaId = num2;
				List<MapPickupGenerator.GenerateRequest> reqList = list;
				this.GenerateForSpecialArea(random, areaId, reqList, outDict);
			}
			this.PostGenerate();
		}

		// Token: 0x06007BAA RID: 31658 RVA: 0x0048EA10 File Offset: 0x0048CC10
		private void PreGenerate()
		{
			this.ClearCaches();
		}

		// Token: 0x06007BAB RID: 31659 RVA: 0x0048EA1A File Offset: 0x0048CC1A
		private void PostGenerate()
		{
			this.ClearCaches();
		}

		// Token: 0x06007BAC RID: 31660 RVA: 0x0048EA24 File Offset: 0x0048CC24
		private void ClearCaches()
		{
			MapPickupGenerator._cachedConfigBlockSet.Clear();
			MapPickupGenerator._cacheItemTypeToAllIdDict.Clear();
		}

		// Token: 0x06007BAD RID: 31661 RVA: 0x0048EA40 File Offset: 0x0048CC40
		private static Dictionary<int, Dictionary<sbyte, List<MapPickupGenerator.GenerateRequest>>> CollectRequestsByState()
		{
			Dictionary<int, Dictionary<sbyte, List<MapPickupGenerator.GenerateRequest>>> requestsByState = new Dictionary<int, Dictionary<sbyte, List<MapPickupGenerator.GenerateRequest>>>();
			foreach (MapPickupsItem pickupConfig in ((IEnumerable<MapPickupsItem>)MapPickups.Instance))
			{
				bool flag = pickupConfig.Type == EMapPickupsType.Event;
				if (!flag)
				{
					for (int stateIndex = 0; stateIndex < pickupConfig.StateTimes.Length; stateIndex++)
					{
						byte repeat = pickupConfig.StateTimes[stateIndex];
						bool flag2 = repeat <= 0;
						if (!flag2)
						{
							for (int levelIndex = 0; levelIndex < pickupConfig.XiangshuLevel.Length; levelIndex++)
							{
								sbyte level = pickupConfig.XiangshuLevel[levelIndex];
								Dictionary<sbyte, List<MapPickupGenerator.GenerateRequest>> levelDict;
								bool flag3 = !requestsByState.TryGetValue(stateIndex, out levelDict);
								if (flag3)
								{
									levelDict = new Dictionary<sbyte, List<MapPickupGenerator.GenerateRequest>>();
									requestsByState[stateIndex] = levelDict;
								}
								List<MapPickupGenerator.GenerateRequest> requestList;
								bool flag4 = !levelDict.TryGetValue(level, out requestList);
								if (flag4)
								{
									requestList = new List<MapPickupGenerator.GenerateRequest>();
									levelDict[level] = requestList;
								}
								for (int i = 0; i < (int)repeat; i++)
								{
									requestList.Add(new MapPickupGenerator.GenerateRequest
									{
										PickupConfig = pickupConfig,
										XiangshuLevelIndex = levelIndex
									});
								}
							}
						}
					}
				}
			}
			return requestsByState;
		}

		// Token: 0x06007BAE RID: 31662 RVA: 0x0048EBB4 File Offset: 0x0048CDB4
		private static Dictionary<short, List<MapPickupGenerator.GenerateRequest>> CollectSpecialRequests()
		{
			Dictionary<short, List<MapPickupGenerator.GenerateRequest>> result = new Dictionary<short, List<MapPickupGenerator.GenerateRequest>>();
			short[] targetAreas = new short[]
			{
				135,
				137,
				138
			};
			foreach (short areaId in targetAreas)
			{
				List<MapPickupGenerator.GenerateRequest> requestList = new List<MapPickupGenerator.GenerateRequest>();
				result[areaId] = requestList;
				foreach (MapPickupsItem pickupConfig in ((IEnumerable<MapPickupsItem>)MapPickups.Instance))
				{
					bool flag = pickupConfig.Type == EMapPickupsType.Event;
					if (!flag)
					{
						byte repeat = pickupConfig.SpecialAreaTimes;
						bool flag2 = repeat > 0 && !pickupConfig.XiangshuLevel.Contains(0);
						if (!flag2)
						{
							for (int i = 0; i < (int)repeat; i++)
							{
								requestList.Add(new MapPickupGenerator.GenerateRequest
								{
									PickupConfig = pickupConfig,
									XiangshuLevelIndex = 0
								});
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06007BAF RID: 31663 RVA: 0x0048ECC8 File Offset: 0x0048CEC8
		private void GeneratePerState(IRandomSource random, Dictionary<Location, MapPickupCollection> outDict, int stateId, Dictionary<sbyte, List<MapPickupGenerator.GenerateRequest>> xiangshuLevelRequestDict)
		{
			List<MapBlockData>[] areaBlocks = MapPickupGenerator.GetShuffledGroupedBlocksInState(random, stateId);
			Dictionary<EMapBlockType, int> areaTypeLoad = new Dictionary<EMapBlockType, int>();
			Dictionary<Location, int> locationLoad = new Dictionary<Location, int>();
			foreach (KeyValuePair<sbyte, List<MapPickupGenerator.GenerateRequest>> levelEntry in xiangshuLevelRequestDict)
			{
				List<MapPickupGenerator.GenerateRequest> reqList = levelEntry.Value;
				CollectionUtils.Shuffle<MapPickupGenerator.GenerateRequest>(random, reqList);
				foreach (MapPickupGenerator.GenerateRequest req in reqList)
				{
					this.GeneratePerRequest(random, outDict, req, areaBlocks, areaTypeLoad, locationLoad);
				}
			}
		}

		// Token: 0x06007BB0 RID: 31664 RVA: 0x0048ED88 File Offset: 0x0048CF88
		private void GenerateForSpecialArea(IRandomSource random, short areaId, List<MapPickupGenerator.GenerateRequest> reqList, Dictionary<Location, MapPickupCollection> outDict)
		{
			List<MapBlockData> blockList = MapPickupGenerator.GetShuffledBlocksInArea(random, areaId);
			Dictionary<Location, int> locationLoad = new Dictionary<Location, int>();
			foreach (MapPickupGenerator.GenerateRequest req in reqList)
			{
				this.GeneratePerSpecialRequest(random, outDict, req, blockList, locationLoad);
			}
		}

		// Token: 0x06007BB1 RID: 31665 RVA: 0x0048EDF0 File Offset: 0x0048CFF0
		private void GeneratePerRequest(IRandomSource random, Dictionary<Location, MapPickupCollection> outDict, MapPickupGenerator.GenerateRequest req, List<MapBlockData>[] areaBlocks, Dictionary<EMapBlockType, int> areaLoad, Dictionary<Location, int> locationLoad)
		{
			MapPickupsItem config = req.PickupConfig;
			Dictionary<EMapBlockType, List<MapBlockData>> validAreaBlocks = this.GetValidAreaBlocks(areaBlocks, config);
			bool flag = !validAreaBlocks.Any<KeyValuePair<EMapBlockType, List<MapBlockData>>>();
			if (!flag)
			{
				EMapBlockType chosenAreaType = (from aid in validAreaBlocks.Keys
				orderby areaLoad.GetValueOrDefault(aid, 0)
				select aid).First<EMapBlockType>();
				List<MapBlockData> candidateBlocks = validAreaBlocks[chosenAreaType];
				MapBlockData chosenBlock = MapPickupGenerator.PickBlockByLoad(candidateBlocks, locationLoad, random);
				Location pickupLocation = chosenBlock.GetLocation();
				locationLoad[pickupLocation] = locationLoad.GetValueOrDefault(pickupLocation, 0) + 1;
				MapPickup pickup = MapPickupGenerator.GenerateOnePickup(config, req.XiangshuLevelIndex, random, true);
				pickup.Location = pickupLocation;
				MapPickupGenerator.AddPickupToResult(outDict, pickup);
				areaLoad.TryAdd(chosenAreaType, 0);
				Dictionary<EMapBlockType, int> areaLoad2 = areaLoad;
				EMapBlockType key = chosenAreaType;
				int num = areaLoad2[key];
				areaLoad2[key] = num + 1;
			}
		}

		// Token: 0x06007BB2 RID: 31666 RVA: 0x0048EED4 File Offset: 0x0048D0D4
		private void GeneratePerSpecialRequest(IRandomSource random, Dictionary<Location, MapPickupCollection> outDict, MapPickupGenerator.GenerateRequest req, List<MapBlockData> blockList, Dictionary<Location, int> locationLoad)
		{
			MapPickupsItem config = req.PickupConfig;
			HashSet<short> blockSet;
			bool flag = !MapPickupGenerator._cachedConfigBlockSet.TryGetValue(config.TemplateId, out blockSet);
			if (flag)
			{
				blockSet = config.BlockList.ToHashSet<short>();
			}
			MapBlockData chosenBlock = MapPickupGenerator.PickBlockByLoad((from b in blockList
			where blockSet.Contains(b.TemplateId)
			select b).ToList<MapBlockData>(), locationLoad, random);
			bool flag2 = chosenBlock == null;
			if (!flag2)
			{
				Location pickupLocation = chosenBlock.GetLocation();
				locationLoad[pickupLocation] = locationLoad.GetValueOrDefault(pickupLocation, 0) + 1;
				MapPickup pickup = MapPickupGenerator.GenerateOnePickup(config, req.XiangshuLevelIndex, random, false);
				pickup.Location = pickupLocation;
				MapPickupGenerator.AddPickupToResult(outDict, pickup);
			}
		}

		// Token: 0x06007BB3 RID: 31667 RVA: 0x0048EF8C File Offset: 0x0048D18C
		private Dictionary<EMapBlockType, List<MapBlockData>> GetValidAreaBlocks(List<MapBlockData>[] areaBlocks, MapPickupsItem config)
		{
			HashSet<short> blockSet;
			bool flag = !MapPickupGenerator._cachedConfigBlockSet.TryGetValue(config.TemplateId, out blockSet);
			if (flag)
			{
				blockSet = config.BlockList.ToHashSet<short>();
			}
			Dictionary<EMapBlockType, List<MapBlockData>> validAreaBlocks = new Dictionary<EMapBlockType, List<MapBlockData>>();
			for (int blockTypeIndex = 0; blockTypeIndex < 4; blockTypeIndex++)
			{
				EMapBlockType areaType = this._mapBlockTypes[blockTypeIndex];
				List<MapBlockData> blocks = areaBlocks[blockTypeIndex];
				List<MapBlockData> validBlocks = new List<MapBlockData>();
				foreach (MapBlockData block in blocks)
				{
					bool flag2 = blockSet.Contains(block.TemplateId);
					if (flag2)
					{
						validBlocks.Add(block);
					}
				}
				bool flag3 = validBlocks.Any<MapBlockData>();
				if (flag3)
				{
					validAreaBlocks[areaType] = validBlocks;
				}
			}
			return validAreaBlocks;
		}

		// Token: 0x06007BB4 RID: 31668 RVA: 0x0048F078 File Offset: 0x0048D278
		private unsafe static List<MapBlockData>[] GetShuffledGroupedBlocksInState(IRandomSource random, int stateId)
		{
			List<MapBlockData>[] result = new List<MapBlockData>[4];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = new List<MapBlockData>();
			}
			List<short> areaIdList = ObjectPool<List<short>>.Instance.Get();
			DomainManager.Map.GetAllAreaInState((sbyte)stateId, areaIdList);
			foreach (short areaId in areaIdList)
			{
				Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
				MapAreaItem areaConfig = DomainManager.Map.GetAreaByAreaId(areaId).GetConfig();
				EMapBlockType areaType = MapPickupGenerator.GetAreaType(areaConfig);
				List<MapBlockData> blockList = result[MapPickupGenerator.BlockTypeToIndex(areaType)];
				Span<MapBlockData> span = areaBlocks;
				for (int j = 0; j < span.Length; j++)
				{
					MapBlockData block = *span[j];
					blockList.Add(block);
				}
			}
			foreach (List<MapBlockData> blockList2 in result)
			{
				CollectionUtils.Shuffle<MapBlockData>(random, blockList2);
			}
			ObjectPool<List<short>>.Instance.Return(areaIdList);
			return result;
		}

		// Token: 0x06007BB5 RID: 31669 RVA: 0x0048F1A8 File Offset: 0x0048D3A8
		private unsafe static List<MapBlockData> GetShuffledBlocksInArea(IRandomSource random, short areaId)
		{
			List<MapBlockData> blockList = new List<MapBlockData>();
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
			Span<MapBlockData> span = areaBlocks;
			for (int i = 0; i < span.Length; i++)
			{
				MapBlockData block = *span[i];
				blockList.Add(block);
			}
			CollectionUtils.Shuffle<MapBlockData>(random, blockList);
			return blockList;
		}

		// Token: 0x06007BB6 RID: 31670 RVA: 0x0048F204 File Offset: 0x0048D404
		private static void AddPickupToResult(Dictionary<Location, MapPickupCollection> outDict, MapPickup pickup)
		{
			MapPickupCollection pickupCollection;
			bool flag = !outDict.TryGetValue(pickup.Location, out pickupCollection);
			if (flag)
			{
				pickupCollection = new MapPickupCollection();
				outDict.Add(pickup.Location, pickupCollection);
			}
			pickupCollection.AddPickup(pickup);
		}

		// Token: 0x06007BB7 RID: 31671 RVA: 0x0048F248 File Offset: 0x0048D448
		public static EMapBlockType GetAreaType(MapAreaItem areaConfig)
		{
			short[] settlementBlockCore = areaConfig.SettlementBlockCore;
			short[] array = settlementBlockCore;
			int i = 0;
			while (i < array.Length)
			{
				short blockId = array[i];
				MapBlockItem blockConfig = MapBlock.Instance[blockId];
				EMapBlockType result;
				switch (blockConfig.Type)
				{
				case EMapBlockType.City:
					result = EMapBlockType.City;
					break;
				case EMapBlockType.Sect:
					result = EMapBlockType.Sect;
					break;
				case EMapBlockType.Town:
					result = EMapBlockType.Town;
					break;
				default:
					i++;
					continue;
				}
				return result;
			}
			return EMapBlockType.Invalid;
		}

		// Token: 0x06007BB8 RID: 31672 RVA: 0x0048F2BC File Offset: 0x0048D4BC
		private static MapPickup GenerateOnePickup(MapPickupsItem pickupConfig, int xiangshuLevelIndex, IRandomSource random, bool needXiangshuMinion)
		{
			Location location = Location.Invalid;
			sbyte xiangshuLevel = pickupConfig.XiangshuLevel[xiangshuLevelIndex];
			bool flag = pickupConfig.Type == EMapPickupsType.Event;
			if (flag)
			{
				throw new Exception("Event pickup type is invalid for disabled.");
			}
			bool hasXiangshuMinion = needXiangshuMinion && random.CheckPercentProb((int)GlobalConfig.Instance.MapPickupHasXiangshuMinionProbability);
			bool flag2 = pickupConfig.BonusCount.Length != 0;
			MapPickup result;
			if (flag2)
			{
				int baseBonusCount = pickupConfig.BonusCount[xiangshuLevelIndex];
				short randomFactor = GlobalConfig.Instance.MapPickupResourceCountRandomFactor;
				int min = baseBonusCount * (int)(100 - randomFactor) / 100;
				int max = baseBonusCount * (int)(100 + randomFactor) / 100;
				int bonusCount = random.Next(min, max);
				bool isExpBonus = pickupConfig.IsExpBonus;
				if (isExpBonus)
				{
					result = MapPickup.CreateExpBonus(location, pickupConfig.TemplateId, bonusCount, xiangshuLevel, hasXiangshuMinion);
				}
				else
				{
					bool isDebtBonus = pickupConfig.IsDebtBonus;
					if (isDebtBonus)
					{
						result = MapPickup.CreateDebtBonus(location, pickupConfig.TemplateId, bonusCount, xiangshuLevel, hasXiangshuMinion);
					}
					else
					{
						result = MapPickup.CreateResource(location, pickupConfig.TemplateId, bonusCount, xiangshuLevel, hasXiangshuMinion);
					}
				}
			}
			else
			{
				bool flag3 = pickupConfig.ItemGrade.Length != 0;
				if (flag3)
				{
					sbyte baseGrade = pickupConfig.ItemGrade[xiangshuLevelIndex];
					short itemId = MapPickupGenerator.RandomPickItemWithBaseGrade(random, pickupConfig, baseGrade);
					result = MapPickup.CreateItem(location, pickupConfig.TemplateId, pickupConfig.ItemGroup.ItemType, itemId, xiangshuLevel, hasXiangshuMinion);
				}
				else
				{
					bool loopEffect = pickupConfig.LoopEffect;
					if (loopEffect)
					{
						result = MapPickup.CreateLoopEffect(location, pickupConfig.TemplateId, xiangshuLevel, hasXiangshuMinion);
					}
					else
					{
						bool readEffect = pickupConfig.ReadEffect;
						if (!readEffect)
						{
							throw new ArgumentException("Invalid pickup config");
						}
						result = MapPickup.CreateReadEffect(location, pickupConfig.TemplateId, xiangshuLevel, hasXiangshuMinion);
					}
				}
			}
			return result;
		}

		// Token: 0x06007BB9 RID: 31673 RVA: 0x0048F448 File Offset: 0x0048D648
		private static short RandomPickItemWithBaseGrade(IRandomSource random, MapPickupsItem pickupConfig, sbyte baseGrade)
		{
			List<short> possibleIds = ObjectPool<List<short>>.Instance.Get();
			possibleIds.Clear();
			byte randomFactor = GlobalConfig.Instance.MapPickupItemGradeRandomFactor;
			sbyte itemType = pickupConfig.ItemGroup.ItemType;
			short groupId = pickupConfig.ItemGroup.TemplateId;
			IList<int> templateIds;
			bool flag = !MapPickupGenerator._cacheItemTypeToAllIdDict.TryGetValue(itemType, out templateIds);
			if (flag)
			{
				templateIds = ItemTemplateHelper.GetTemplateDataAllKeys(itemType);
				MapPickupGenerator._cacheItemTypeToAllIdDict[itemType] = templateIds;
			}
			sbyte exceptedMinGrade = baseGrade - (sbyte)randomFactor;
			sbyte exceptedMaxGrade = baseGrade;
			possibleIds.AddRange(from tid in templateIds
			where ItemTemplateHelper.GetGroupId(itemType, (short)tid) == groupId && ItemTemplateHelper.GetGrade(itemType, (short)tid) >= exceptedMinGrade && ItemTemplateHelper.GetGrade(itemType, (short)tid) <= exceptedMaxGrade
			select (short)tid);
			bool flag2 = possibleIds.Count == 0;
			if (flag2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(86, 6);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid pickup config ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(pickupConfig.TemplateId);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted(pickupConfig.Name);
				defaultInterpolatedStringHandler.AppendLiteral(". No item found in group ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(groupId);
				defaultInterpolatedStringHandler.AppendLiteral(" with grade between ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(exceptedMinGrade);
				defaultInterpolatedStringHandler.AppendLiteral(" and ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(exceptedMaxGrade);
				defaultInterpolatedStringHandler.AppendLiteral(". itemType: ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(itemType);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int randomIndex = random.Next(0, possibleIds.Count);
			short itemId = possibleIds[randomIndex];
			ObjectPool<List<short>>.Instance.Return(possibleIds);
			return itemId;
		}

		// Token: 0x06007BBA RID: 31674 RVA: 0x0048F62C File Offset: 0x0048D82C
		private static MapBlockData PickBlockByLoad(IList<MapBlockData> sourceList, Dictionary<Location, int> locationLoad, IRandomSource random)
		{
			bool flag = sourceList == null || sourceList.Count == 0;
			MapBlockData result;
			if (flag)
			{
				result = null;
			}
			else
			{
				int[] cumulativeWeights = new int[sourceList.Count];
				int totalWeight = 0;
				for (int i = 0; i < sourceList.Count; i++)
				{
					Location location = sourceList[i].GetLocation();
					int load = locationLoad.GetValueOrDefault(location, 0);
					int weight = 1 << 7 - Math.Min(load, 7);
					totalWeight += weight;
					cumulativeWeights[i] = totalWeight;
				}
				int randomValue = random.Next(0, totalWeight);
				int index = Array.BinarySearch<int>(cumulativeWeights, randomValue);
				bool flag2 = index < 0;
				if (flag2)
				{
					index = ~index;
				}
				result = sourceList[index];
			}
			return result;
		}

		// Token: 0x06007BBB RID: 31675 RVA: 0x0048F6E4 File Offset: 0x0048D8E4
		public MapPickupGenerator()
		{
			EMapBlockType[] array = new EMapBlockType[4];
			RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.8EB98E5DC91BE0043765DAB820052016D078F57BEAE8DE3657271BB0F1317914).FieldHandle);
			this._mapBlockTypes = array;
			base..ctor();
		}

		// Token: 0x04002244 RID: 8772
		private const int BlockTypeCount = 4;

		// Token: 0x04002245 RID: 8773
		private readonly EMapBlockType[] _mapBlockTypes;

		// Token: 0x04002246 RID: 8774
		private static readonly Dictionary<short, HashSet<short>> _cachedConfigBlockSet = new Dictionary<short, HashSet<short>>();

		// Token: 0x04002247 RID: 8775
		private static readonly Dictionary<sbyte, IList<int>> _cacheItemTypeToAllIdDict = new Dictionary<sbyte, IList<int>>();

		// Token: 0x02000C6D RID: 3181
		private class GenerateRequest
		{
			// Token: 0x04003618 RID: 13848
			public MapPickupsItem PickupConfig;

			// Token: 0x04003619 RID: 13849
			public int XiangshuLevelIndex;
		}
	}
}
