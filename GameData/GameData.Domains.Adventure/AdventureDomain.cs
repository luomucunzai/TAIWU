using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Common.SingleValueCollection;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Adventure.AdventureMap;
using GameData.Domains.Adventure.Modifications;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Creation;
using GameData.Domains.Character.Display;
using GameData.Domains.Character.Filters;
using GameData.Domains.Character.Relation;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.Map;
using GameData.Domains.Map.Filters;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.Domains.World;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using Redzen.Random;

namespace GameData.Domains.Adventure;

[GameDataDomain(10)]
public class AdventureDomain : BaseGameDataDomain
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private int _playerPos;

	[DomainData(DomainDataType.SingleValue, false, false, true, true, ArrayElementsCount = 7)]
	private int[] _personalities;

	[DomainData(DomainDataType.SingleValue, false, true, true, true, ArrayElementsCount = 7)]
	private int[] _personalitiesCost;

	[DomainData(DomainDataType.SingleValue, false, true, true, true, ArrayElementsCount = 7)]
	private int[] _personalitiesGain;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private List<int> _indicatePath;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private List<int> _arrangeableNodes;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private List<int> _arrangedNodes;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private List<int> _teamDetectedNodes;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private List<int> _perceivedNodes;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private string _errorInfo;

	[DomainData(DomainDataType.SingleValueCollection, false, false, false, false)]
	private readonly Dictionary<int, int> _adBranchOpenRecord;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private short _curAdventureId = -1;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _operationBlock = false;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private sbyte _adventureState;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private AdventureMapTrunk _curMapTrunk;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private List<int> _advParameters;

	[DomainData(DomainDataType.SingleValue, false, false, true, false)]
	private List<ItemKey> _enterItems;

	private Dictionary<string, int> _parameterMap;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _allowExitAdventure;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private int _actionPointWithhold = 0;

	private readonly HashSet<AdvMapPos> _triggeredPosSet = new HashSet<AdvMapPos>();

	private readonly HashSet<AdvMapPos> _finishedPosSet = new HashSet<AdvMapPos>();

	private List<(string, sbyte, short)> _extraEvents;

	private bool _eventBlock = false;

	private readonly List<AdvMapNode> _path = new List<AdvMapNode>();

	private Location _curAdvSiteLocation;

	private string _chosenBranchKey;

	private Stack<AdventureMapTrunk> _mapTrunks;

	private Action _onEventFinishCallback;

	private readonly AdventureResultDisplayData _resultDisplayData = new AdventureResultDisplayData();

	[DomainData(DomainDataType.ElementList, true, false, true, false, ArrayElementsCount = 139)]
	private readonly AreaAdventureData[] _adventureAreas;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
	private readonly Dictionary<Location, EnemyNestSiteExtraData> _enemyNestSites;

	[DomainData(DomainDataType.ElementList, true, false, true, false, ArrayElementsCount = 90)]
	private readonly BrokenAreaData[] _brokenAreaEnemies;

	private readonly sbyte[] _stateBrokenAreaLevels = new sbyte[6] { 1, 2, 3, 4, 5, 6 };

	private static int _brokenAreaEnemyBaseCount;

	private static List<short>[] AdventureTypes;

	[Obsolete]
	public static readonly Dictionary<short, sbyte> AdventureTemplateIdToXiangshuAvatarId = new Dictionary<short, sbyte>
	{
		{ 114, 0 },
		{ 111, 1 },
		{ 108, 2 },
		{ 110, 3 },
		{ 109, 4 },
		{ 113, 5 },
		{ 116, 6 },
		{ 115, 7 },
		{ 112, 8 }
	};

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private List<int> _spentCharList;

	private readonly Dictionary<int, int> _spentCharInCombatGroupDict = new Dictionary<int, int>();

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private int _curBranchChosenChar;

	private HashSet<int> _temporaryIntelligentCharacters;

	private HashSet<int> _temporaryEnemies;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private List<short> _escapingRandomEnemies;

	private int[] _enemyNestCounts;

	private readonly List<AdvMapBranch> _branches = new List<AdvMapBranch>();

	private readonly List<AdvMapNodeVertex> _vertices = new List<AdvMapNodeVertex>();

	public readonly Dictionary<AdvMapPos, AdvMapNode> NodesDict = new Dictionary<AdvMapPos, AdvMapNode>();

	private int _enterTerrain;

	public List<(short, short)> EnterTerrainWeights;

	private AdvMapBranch _curBranch;

	private readonly List<(int, int)> _contentList = new List<(int, int)>();

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[25][];

	private SpinLock _spinLockPersonalitiesCost = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockPersonalitiesGain = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockArrangeableNodes = new SpinLock(enableThreadOwnerTracking: false);

	private static readonly DataInfluence[][] CacheInfluencesAdventureAreas = new DataInfluence[139][];

	private readonly byte[] _dataStatesAdventureAreas = new byte[35];

	private SingleValueCollectionModificationCollection<Location> _modificationsEnemyNestSites = SingleValueCollectionModificationCollection<Location>.Create();

	private static readonly DataInfluence[][] CacheInfluencesBrokenAreaEnemies = new DataInfluence[90][];

	private readonly byte[] _dataStatesBrokenAreaEnemies = new byte[23];

	private Queue<uint> _pendingLoadingOperationIds;

	private AdvMapPos PlayerPos
	{
		get
		{
			return new AdvMapPos(_playerPos);
		}
		set
		{
			_playerPos = value.GetHashCode();
		}
	}

	public AdventureItem AdventureCfg { get; private set; }

	private AdvMapNode CurNode => _curBranch.GetNode(PlayerPos) ?? _curBranch.AdvancedBranch?.GetNode(PlayerPos);

	public sbyte CurAdventureLifeSkillDifficulty => SharedMethods.GetAdventureLifeSkillDifficulty(AdventureCfg.TemplateId, DomainManager.World.GetXiangshuLevel());

	[DomainMethod]
	public unsafe bool EnterAdventure(DataContext context, short adventureId, List<ItemKey> itemKeys)
	{
		context.SwitchRandomSource("EnterAdventure");
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		_curAdvSiteLocation = taiwu.GetLocation();
		AdventureSiteData siteData = _adventureAreas[_curAdvSiteLocation.AreaId].AdventureSites[_curAdvSiteLocation.BlockId];
		MapBlockData block = DomainManager.Map.GetBlock(_curAdvSiteLocation);
		_enterTerrain = block.TemplateId;
		EnterTerrainWeights = MapBlock.Instance[_enterTerrain].AdventureTerrainWeights;
		Personalities personalities = taiwu.GetPersonalities();
		for (sbyte b = 0; b < 7; b++)
		{
			_personalities[b] = personalities.Items[b];
		}
		SetPersonalities(_personalities, context);
		ClearAdvParameters();
		AdventureCfg = Config.Adventure.Instance[adventureId];
		InitializeMap(context);
		if (DomainManager.World.GetLeftDaysInCurrMonth() < AdventureCfg.TimeCost)
		{
			return false;
		}
		WithholdingActionPoint(context, AdventureCfg.TimeCost * 10);
		_enterItems.Clear();
		if (itemKeys != null && itemKeys.Count > 0)
		{
			_enterItems.AddRange(itemKeys);
		}
		SetEnterItems(_enterItems, context);
		for (int i = 0; i < itemKeys?.Count; i++)
		{
			if (itemKeys[i].ItemType == 12)
			{
				if (itemKeys[i].TemplateId == 210)
				{
					continue;
				}
				short templateId = itemKeys[i].TemplateId;
				if (templateId >= 200 && templateId <= 209)
				{
					continue;
				}
			}
			taiwu.RemoveInventoryItem(context, itemKeys[i], 1, deleteItem: false);
		}
		ResourceInts delta = new ResourceInts(AdventureCfg.ResCost.Select((int res) => res * -1).ToArray());
		taiwu.ChangeResources(context, ref delta);
		EventArgBox argBox = CreateAdventureEventArgBox(context, _curAdvSiteLocation, siteData);
		DomainManager.TaiwuEvent.SetNewAdventure(adventureId, argBox);
		SetCurAdventureId(adventureId, context);
		_resultDisplayData.Clear();
		return true;
	}

	private EventArgBox CreateAdventureEventArgBox(DataContext context, Location location, AdventureSiteData siteData)
	{
		EventArgBox eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
		eventArgBox.Set("AdventureSite", (ISerializableGameData)(object)siteData);
		eventArgBox.Set("AdventureLocation", (ISerializableGameData)(object)location);
		if (siteData.MonthlyActionKey.IsValid())
		{
			MonthlyActionBase monthlyAction = DomainManager.TaiwuEvent.GetMonthlyAction(siteData.MonthlyActionKey);
			monthlyAction.EnsurePrerequisites();
			monthlyAction.FillEventArgBox(eventArgBox);
		}
		return eventArgBox;
	}

	public void GenerateAdventureMap(DataContext context, string nodeKey)
	{
		Logger.Info("Generating initial branch starting at node \"" + nodeKey + "\"");
		AdvMapNodeVertex advMapNodeVertex = null;
		foreach (AdvMapNodeVertex vertex in _vertices)
		{
			if (nodeKey.Equals(vertex.NodeKey))
			{
				if (vertex.NodeType != ENodeType.Start)
				{
					throw new Exception("The selected node " + nodeKey + " is not a start node.");
				}
				advMapNodeVertex = vertex;
			}
		}
		if (advMapNodeVertex == null)
		{
			throw new Exception("Cannot find the starting node with key " + nodeKey + ".");
		}
		AdvMapNodeVertex key = advMapNodeVertex.ConnectedBranchDict.First().Key;
		SetCurMapTrunk(GenerateAdvMapTrunk(context, advMapNodeVertex, key), context);
		SetPlayerPos(_curBranch.EnterNode.AdjustedPos.GetHashCode(), context);
		RefreshIndicatePath(context);
		SetAdventureState(0, context);
	}

	[DomainMethod]
	public void ArrangeNode(DataContext context, int pos)
	{
		if (_adventureState != 1)
		{
			return;
		}
		AdvMapNodeNormal normalNodeInCurBranch = GetNormalNodeInCurBranch(pos);
		if (normalNodeInCurBranch == null)
		{
			return;
		}
		if (_arrangedNodes.Contains(pos))
		{
			_arrangedNodes.Remove(pos);
		}
		else
		{
			int[] personalitiesCost = GetPersonalitiesCost();
			if (_personalities[normalNodeInCurBranch.SevenElementType] + personalitiesCost[normalNodeInCurBranch.SevenElementType] < normalNodeInCurBranch.SevenElementCost)
			{
				return;
			}
			_arrangedNodes.Add(pos);
		}
		_arrangedNodes.Sort(delegate(int a, int b)
		{
			AdvMapNodeNormal normalNodeInCurBranch2 = GetNormalNodeInCurBranch(a);
			AdvMapNodeNormal normalNodeInCurBranch3 = GetNormalNodeInCurBranch(b);
			return normalNodeInCurBranch2.Offset.X.CompareTo(normalNodeInCurBranch3.Offset.X);
		});
		RefreshIndicatePath(context);
		SetArrangedNodes(_arrangedNodes, context);
		List<int> list = new List<int>();
		foreach (int perceivedNode in _perceivedNodes)
		{
			if (!_indicatePath.Contains(perceivedNode))
			{
				list.Add(perceivedNode);
			}
		}
		foreach (int item in list)
		{
			PerceiveNode(context, item);
		}
		SetPerceivedNodes(_perceivedNodes, context);
	}

	private AdvMapNodeNormal GetNormalNodeInCurBranch(int pos)
	{
		AdvMapPos advMapPos = new AdvMapPos(pos);
		return GetNormalNodeInCurBranch(advMapPos);
	}

	private AdvMapNodeNormal GetNormalNodeInCurBranch(AdvMapPos advMapPos)
	{
		AdvMapNode node = _curBranch.GetNode(advMapPos);
		if (node == null && _curBranchChosenChar > -1)
		{
			node = _curBranch.AdvancedBranch.GetNode(advMapPos);
		}
		return node as AdvMapNodeNormal;
	}

	[DomainMethod]
	public void PerceiveNode(DataContext context, int pos)
	{
		if (_adventureState != 1)
		{
			return;
		}
		if (_perceivedNodes.Contains(pos))
		{
			_perceivedNodes.Remove(pos);
		}
		else
		{
			AdvMapNodeNormal normalNodeInCurBranch = GetNormalNodeInCurBranch(pos);
			sbyte sevenElementCost = normalNodeInCurBranch.SevenElementCost;
			int[] personalitiesCost = GetPersonalitiesCost();
			if (_personalities[6] + personalitiesCost[6] < sevenElementCost)
			{
				return;
			}
			_perceivedNodes.Add(pos);
		}
		SetPerceivedNodes(_perceivedNodes, context);
	}

	[DomainMethod]
	public void ConfirmPath(DataContext context)
	{
		if (_adventureState == 1)
		{
			SetAdventureState(2, context);
			_eventBlock = false;
			InitPathContent(context);
			MoveForward(context);
		}
	}

	[DomainMethod]
	public (int, int) ConfirmArrived(DataContext context)
	{
		if (_eventBlock)
		{
			return (-1, -1);
		}
		switch (CurNode.NodeType)
		{
		case ENodeType.Start:
			SetAdventureState(0, context);
			DomainManager.TaiwuEvent.OnEvent_AdventureReachStartNode((short)CurNode.Index);
			break;
		case ENodeType.Transfer:
			SetAdventureState(0, context);
			DomainManager.TaiwuEvent.OnEvent_AdventureReachTransferNode((short)CurNode.Index);
			break;
		case ENodeType.End:
			SetAdventureState(0, context);
			DomainManager.TaiwuEvent.OnEvent_AdventureReachEndNode((short)CurNode.Index);
			break;
		case ENodeType.Normal:
		{
			AdvBaseMapBranch advBaseMapBranch = ((_curBranch.AdvancedBranch != null && _curBranch.AdvancedBranch.Nodes.Contains(CurNode)) ? ((AdvBaseMapBranch)_curBranch.AdvancedBranch) : ((AdvBaseMapBranch)_curBranch));
			int branchIndex = advBaseMapBranch.BranchIndex;
			AdventureBranch branchCfg = ((branchIndex < AdventureCfg.BaseBranches.Count) ? ((AdventureBranch)AdventureCfg.BaseBranches[branchIndex]) : ((AdventureBranch)AdventureCfg.AdvancedBranches[branchIndex - AdventureCfg.BaseBranches.Count]));
			AdventureMapPoint pointData = CurNode.ToAdventureMapPoint();
			switch (CurNode.NodeContent.Item1)
			{
			case -1:
				break;
			case 0:
				return HandleNodeContent_Event(context, branchCfg, pointData);
			case 1:
				return HandleNodeContent_Resource(context, branchCfg, pointData);
			case 2:
				return HandleNodeContent_Item(context, branchCfg, pointData);
			case 3:
				return HandleNodeContent_Bonus(context, branchCfg, pointData);
			case 10:
				return HandleNodeContent_ExtraEvent(context, branchCfg, pointData);
			default:
				throw new Exception("Invalid node content type.");
			}
			TriggerGlobalEvent(branchCfg, pointData, delegate
			{
				OnEventHandleFinished(context);
			});
			break;
		}
		}
		return (-1, -1);
	}

	[DomainMethod]
	public void ExitAdventure(DataContext context, bool isAdventureCompleted)
	{
		Logger.Info("Leaving adventure " + (isAdventureCompleted ? "with a complete state" : "with a incomplete state"));
		DomainManager.TaiwuEvent.CheckTaiwuStatusImmediately();
		RemoveWithheldActionPoint(context, AdventureCfg.TimeCost * 10);
		if (isAdventureCompleted)
		{
			DomainManager.Taiwu.AddLegacyPoint(context, 39);
		}
		AdventureSiteData adventureSiteData = _adventureAreas[_curAdvSiteLocation.AreaId].AdventureSites[_curAdvSiteLocation.BlockId];
		MapBlockData block = DomainManager.Map.GetBlock(_curAdvSiteLocation);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int originExp = taiwu.GetExp() - _resultDisplayData.ChangedExp;
		int areaSpiritualDebt = DomainManager.Extra.GetAreaSpiritualDebt(block.AreaId);
		ResourceInts originResources = taiwu.GetResources().Subtract(ref _resultDisplayData.ChangedResources);
		_resultDisplayData.SetOrigin(_curAdventureId, originExp, areaSpiritualDebt, originResources);
		_resultDisplayData.IsFinished = isAdventureCompleted;
		if ((isAdventureCompleted && adventureSiteData.SiteState != 2) || DomainManager.Adventure.AdventureCfg.Interruptible == 1)
		{
			RemoveAdventureSite(context, _curAdvSiteLocation.AreaId, _curAdvSiteLocation.BlockId, isTimeout: false, isAdventureCompleted);
			Logger.Info($"Removing adventure block at {_curAdvSiteLocation}");
		}
		_curAdvSiteLocation = Location.Invalid;
		for (int i = 0; i < _enterItems.Count; i++)
		{
			if (_enterItems[i].ItemType == 12)
			{
				if (_enterItems[i].TemplateId == 210)
				{
					continue;
				}
				short templateId = _enterItems[i].TemplateId;
				if (templateId >= 200 && templateId <= 209)
				{
					continue;
				}
			}
			DomainManager.Item.RemoveItem(context, _enterItems[i]);
		}
		_enterItems.Clear();
		DomainManager.Taiwu.ClearAllTemporaryRestrictions(context);
		DomainManager.TaiwuEvent.ClearAllMarriageLook();
		DomainManager.TaiwuEvent.SeriesEventTexture = string.Empty;
		DomainManager.Character.RevertAllTemporaryModificationsOfAllCharacters(context);
		RestoreSpentCharacters(context);
		RemoveAllTemporaryEnemies(context);
		RemoveAllTemporaryIntelligentCharacters(context);
		SetCurAdventureId(-1, context);
		context.RestoreRandomSource();
	}

	[DomainMethod]
	public void SwitchBranch(DataContext context)
	{
		if (_mapTrunks.Count <= 1)
		{
			throw new Exception("Current branch must not be the first branch.");
		}
		if (!_curBranch.EnterNode.ConnectedBranchDict.Any((KeyValuePair<AdvMapNodeVertex, AdvMapBranch> pair) => pair.Value.EnterNode == _curBranch.EnterNode && _chosenBranchKey == pair.Value.BranchKey))
		{
			throw new Exception("The enter node is not connected with the target branch.");
		}
		_eventBlock = true;
		ConfirmPersonalitiesChange(context);
		_curBranch.ExitNode.PrevVertex = null;
		_curBranch.Nodes.Clear();
		_curBranch.Nodes.Add(_curBranch.FirstNode);
		_mapTrunks.Pop();
		_curMapTrunk = _mapTrunks.Peek();
		_curBranch = _branches[_curMapTrunk.BranchIndex];
		ClearPathArrangement(context);
		SetCurMapTrunk(GenerateNextAdvMapTrunk(context), context);
		SetPlayerPos(_curBranch.EnterNode.AdjustedPos.GetHashCode(), context);
		RefreshIndicatePath(context);
		SetAdventureState(1, context);
	}

	[DomainMethod]
	public AdventureResultDisplayData GetAdventureResultDisplayData()
	{
		return _resultDisplayData;
	}

	[DomainMethod]
	public void SelectGetItem(DataContext context, List<ItemKey> acceptItems, List<int> acceptCounts)
	{
		if (acceptItems == null || acceptCounts == null)
		{
			return;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		for (int i = 0; i < acceptItems.Count; i++)
		{
			ItemKey key = acceptItems[i];
			int amount = acceptCounts[i];
			ItemDisplayData itemDisplayData = _resultDisplayData.ItemList.Find((ItemDisplayData d) => d.Key.Equals(key));
			if (itemDisplayData != null)
			{
				taiwu.AddInventoryItem(context, key, amount);
			}
		}
	}

	private void ConfirmPersonalitiesChange(DataContext context)
	{
		int[] personalitiesCost = GetPersonalitiesCost();
		int[] personalitiesGain = GetPersonalitiesGain();
		for (int i = 0; i < 7; i++)
		{
			_personalities[i] += (sbyte)(personalitiesCost[i] + personalitiesGain[i]);
		}
		SetPersonalities(_personalities, context);
	}

	private void MoveForward(DataContext context)
	{
		AdvMapNode nextNode = GetNextNode(context, PlayerPos);
		if (nextNode != null)
		{
			SetPlayerPos(nextNode.AdjustedPos.GetHashCode(), context);
			RefreshIndicatePath(context);
			_contentList.RemoveAt(0);
		}
	}

	private void RefreshIndicatePath(DataContext context)
	{
		_path.Clear();
		_path.Add(CurNode);
		while (true)
		{
			List<AdvMapNode> path = _path;
			if (path[path.Count - 1] == _curBranch.ExitNode)
			{
				break;
			}
			List<AdvMapNode> path2 = _path;
			AdvMapNode nextNode = GetNextNode(context, path2[path2.Count - 1]);
			if (nextNode == null)
			{
				Logger.AppendWarning($"node at index {_path.Count - 1} is null");
				break;
			}
			_path.Add(nextNode);
		}
		SetIndicatePath(_path.Select((AdvMapNode a) => a.AdjustedPos.GetHashCode()).ToList(), context);
	}

	private AdvMapNode GetNextNode(DataContext context, AdvMapPos pos)
	{
		return GetNextNode(context, _curBranch.GetNode(pos) ?? _curBranch.AdvancedBranch.GetNode(pos));
	}

	private AdvMapNode GetNextNode(DataContext context, AdvMapNode curNode)
	{
		if (curNode == _curBranch.EnterNode)
		{
			return _curBranch.FirstNode;
		}
		if (curNode == _curBranch.ExitNode)
		{
			return null;
		}
		if (curNode == _curBranch.LastNode)
		{
			return _curBranch.ExitNode;
		}
		if (_curBranchChosenChar >= 0)
		{
			bool flag = false;
			foreach (AdvMapNodeNormal node in _curBranch.AdvancedBranch.Nodes)
			{
				int hashCode = node.AdjustedPos.GetHashCode();
				if (_arrangedNodes.Contains(hashCode))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				if (curNode == _curBranch.AdvancedBranch.EnterNode)
				{
					return _curBranch.AdvancedBranch.FirstNode;
				}
				if (curNode == _curBranch.AdvancedBranch.LastNode)
				{
					return _curBranch.AdvancedBranch.ExitNode;
				}
			}
		}
		AdvMapNodeNormal curNormalNode = curNode as AdvMapNodeNormal;
		if (curNormalNode == null)
		{
			throw new Exception("Given node is not a normal node when it should be.");
		}
		List<AdvMapPos> list = (from a in _arrangedNodes.Select(delegate(int a)
			{
				AdvMapPos advMapPos = new AdvMapPos(a);
				if (_curBranch.AdvancedBranch != null)
				{
					AdvMapNodeNormal advMapNodeNormal = _curBranch.AdvancedBranch.Nodes.Find((AdvMapNodeNormal n) => n.AdjustedPos.Equals(advMapPos));
					if (advMapNodeNormal != null)
					{
						advMapPos = _curBranch.AdvancedBranch.EnterNode.AdjustedPos;
					}
				}
				AdvMapNodeNormal normalNodeInCurBranch = GetNormalNodeInCurBranch(advMapPos);
				return normalNodeInCurBranch.Offset;
			})
			where a.X > curNormalNode.Offset.X
			select a).ToList();
		list.Sort((AdvMapPos a, AdvMapPos b) => a.X.CompareTo(b.X));
		AdvMapPos targetOffset = ((list.Count > 0) ? list[0] : _curBranch.LastNode.Offset);
		List<AdvMapNodeNormal> nexts = (_curBranch.Nodes.Contains(curNode) ? _curBranch.FindNextNodes(curNormalNode) : _curBranch.AdvancedBranch.FindNextNodes(curNormalNode));
		nexts.RemoveAll(delegate(AdvMapNodeNormal a)
		{
			if (a.Offset.X > targetOffset.X)
			{
				return true;
			}
			AdvMapPos advMapPos = targetOffset - a.Offset;
			return Math.Abs(advMapPos.Y) > Math.Abs(advMapPos.X);
		});
		if (nexts.Count == 0)
		{
			throw new Exception("Error Path Calc");
		}
		AdvMapNodeNormal result;
		if (nexts.Any((AdvMapNodeNormal a) => a.Offset.Equals(targetOffset)))
		{
			result = nexts.First((AdvMapNodeNormal a) => a.Offset.Equals(targetOffset));
		}
		else
		{
			CollectionUtils.Sort(nexts, delegate(AdvMapNodeNormal a, AdvMapNodeNormal b)
			{
				int num = _personalities[a.SevenElementType];
				int num2 = _personalities[b.SevenElementType];
				if (num != num2)
				{
					return num2.CompareTo(num);
				}
				int randomValue = curNormalNode.GetRandomValue(context.Random);
				return ((nexts.IndexOf(a) + randomValue) % nexts.Count).CompareTo((nexts.IndexOf(b) + randomValue) % nexts.Count);
			});
			result = nexts[0];
		}
		return result;
	}

	private void WithholdingActionPoint(DataContext context, int value)
	{
		DomainManager.Extra.ConsumeActionPoint(context, value);
		SetActionPointWithhold(_actionPointWithhold + value, context);
	}

	public void RemoveWithheldActionPoint(DataContext context, int value)
	{
		if (value > _actionPointWithhold)
		{
			throw new Exception($"Error occurred. Not enough withhold action point: need {value}, but only {_actionPointWithhold} left");
		}
		SetActionPointWithhold(_actionPointWithhold - value, context);
	}

	public int GetAdvParameter(string key)
	{
		if (!_parameterMap.ContainsKey(key))
		{
			return -1;
		}
		if (!AdventureCfg.AdventureParams.Exists(((string, string, string, string) param) => key.Equals(param.Item1)))
		{
			return _parameterMap[key];
		}
		return _advParameters[_parameterMap[key]];
	}

	public (string, string, string, string) GetAdvParameterConfig(string key)
	{
		return AdventureCfg.AdventureParams.First(((string, string, string, string) param) => key.Equals(param.Item1));
	}

	public void SetAdvParameter(string key, int val, DataContext context)
	{
		if (!AdventureCfg.AdventureParams.Exists(((string, string, string, string) param) => key.Equals(param.Item1)))
		{
			_parameterMap[key] = val;
			return;
		}
		if (!_parameterMap.ContainsKey(key))
		{
			_parameterMap.Add(key, _advParameters.Count);
			_advParameters.Add(val);
		}
		else
		{
			_advParameters[_parameterMap[key]] = val;
		}
		SetAdvParameters(_advParameters, context);
	}

	public void ClearAdvParameters()
	{
		_advParameters.Clear();
		_parameterMap.Clear();
	}

	public void SelectBranch(DataContext context, string branchKey)
	{
		Logger.Info("Selecting branch \"" + branchKey + "\"");
		_chosenBranchKey = branchKey;
	}

	public void AddNextBranchExtraEvent(string eventGuid, sbyte personality, short weight)
	{
		_extraEvents.Add((eventGuid, personality, weight));
	}

	public int GetXDistanceToNextTransferNode()
	{
		if (_curBranch == null)
		{
			return 0;
		}
		return _curBranch.ExitNode.AdjustedPos.X - new AdvMapPos(_playerPos).X;
	}

	public void OnEventHandleFinished(DataContext context)
	{
		if (_onEventFinishCallback != null)
		{
			Action onEventFinishCallback = _onEventFinishCallback;
			_onEventFinishCallback = null;
			onEventFinishCallback();
		}
		else if (_adventureState != 4)
		{
			if (CurNode.NodeType == ENodeType.End)
			{
				SetAdventureState(3, context);
			}
			else if (CurNode.NodeType == ENodeType.Start)
			{
				SetAdventureState(1, context);
			}
			else if (CurNode == _curBranch.ExitNode)
			{
				ConfirmPersonalitiesChange(context);
				SetCurMapTrunk(GenerateNextAdvMapTrunk(context), context);
				SetAdventureState(1, context);
				_arrangedNodes.Clear();
				SetArrangedNodes(_arrangedNodes, context);
				_perceivedNodes.Clear();
				SetPerceivedNodes(_perceivedNodes, context);
				RefreshIndicatePath(context);
			}
			else if (CurNode.NodeType == ENodeType.Normal)
			{
				MoveForward(context);
			}
		}
	}

	public void AddCharacterToResultDisplay(int charId)
	{
		if (_curAdventureId > -1)
		{
			CharacterDisplayData characterDisplayData = DomainManager.Character.GetCharacterDisplayData(charId);
			_resultDisplayData.CharList.Add(characterDisplayData);
		}
	}

	[DomainMethod]
	public void ClearPathArrangement(DataContext context)
	{
		_arrangedNodes.Clear();
		_perceivedNodes.Clear();
		SetArrangedNodes(_arrangedNodes, context);
		SetPerceivedNodes(_perceivedNodes, context);
	}

	private void OnInitializedDomainData()
	{
		_curAdventureId = -2;
		_parameterMap = new Dictionary<string, int>();
		_extraEvents = new List<(string, sbyte, short)>();
		_temporaryIntelligentCharacters = new HashSet<int>();
		_temporaryEnemies = new HashSet<int>();
		_mapTrunks = new Stack<AdventureMapTrunk>();
	}

	private void InitializeOnInitializeGameDataModule()
	{
		InitializeAdventureTypes();
	}

	private void InitializeOnEnterNewWorld()
	{
		InitializeAdventureAreas();
		InitializeBrokenAreaEnemies();
		_enemyNestCounts = new int[EnemyNest.Instance.Count];
	}

	private void OnLoadedArchiveData()
	{
		InitializeEnemyNestCounts();
	}

	public override void FixAbnormalDomainArchiveData(DataContext context)
	{
		FixConqueredEnemyNests(context);
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 70, 58))
		{
			FixOutdatedEnemyNests(context);
		}
	}

	[SingleValueDependency(10, new ushort[] { 12, 6, 7, 13 })]
	private void CalcPersonalitiesCost(int[] value)
	{
		Array.Fill(value, 0);
		if (_curMapTrunk.BranchIndex < 0)
		{
			return;
		}
		AdvMapBranch advMapBranch = _branches[_curMapTrunk.BranchIndex];
		foreach (int arrangedNode in _arrangedNodes)
		{
			AdvMapPos pos = new AdvMapPos(arrangedNode);
			AdvMapNode advMapNode = advMapBranch.GetNode(pos) ?? advMapBranch.AdvancedBranch.GetNode(pos);
			if (advMapNode.SevenElementType >= 0 && advMapNode.SevenElementCost > 0)
			{
				value[advMapNode.SevenElementType] -= advMapNode.SevenElementCost;
			}
		}
		foreach (int perceivedNode in _perceivedNodes)
		{
			AdvMapPos pos2 = new AdvMapPos(perceivedNode);
			AdvMapNode advMapNode2 = advMapBranch.GetNode(pos2) ?? advMapBranch.AdvancedBranch.GetNode(pos2);
			value[6] -= advMapNode2.SevenElementCost;
		}
	}

	[SingleValueDependency(10, new ushort[] { 12, 6, 13 })]
	private void CalcPersonalitiesGain(int[] value)
	{
		Array.Fill(value, 0);
		if (_curMapTrunk.BranchIndex < 0)
		{
			return;
		}
		foreach (int arrangedNode in _arrangedNodes)
		{
			AdvMapPos pos = new AdvMapPos(arrangedNode);
			AdvMapNode node = _curBranch.GetNode(pos);
			if (node == null && _curBranchChosenChar > -1)
			{
				node = _curBranch.AdvancedBranch.GetNode(pos);
			}
			if (node.SevenElementCost > 0)
			{
				value[PersonalityType.Producing[node.SevenElementType]] += node.SevenElementCost;
			}
		}
	}

	[SingleValueDependency(10, new ushort[] { 1, 2, 13, 21, 6 })]
	private void CalcArrangeableNodes(List<int> value)
	{
		if (_curMapTrunk.BranchIndex < 0)
		{
			value.Clear();
			return;
		}
		AdvMapBranch curBranch = _branches[_curMapTrunk.BranchIndex];
		bool hasArrangedOnAdvancedBranch = false;
		List<AdvMapPos> list = _arrangedNodes.Select(delegate(int a)
		{
			AdvMapPos src = new AdvMapPos(a);
			if (_curBranch.GetNode(new AdvMapPos(a)) != null)
			{
				return _curBranch.GetOffset(src);
			}
			hasArrangedOnAdvancedBranch = true;
			return _curBranch.AdvancedBranch.GetOffset(src);
		}).ToList();
		if (hasArrangedOnAdvancedBranch)
		{
			list.Add(_curBranch.AdvancedBranch.EnterNode.Offset);
		}
		AdvMapPos curAdvanceBranchEnterPos = AdvMapPos.Error;
		AdvMapPos advMapPos = AdvMapPos.Error;
		float num = 0f;
		float num2 = 0f;
		if (curBranch.AdvancedBranch != null && _curBranchChosenChar >= 0)
		{
			curAdvanceBranchEnterPos = curBranch.AdvancedBranch.EnterNode.Offset;
			advMapPos = curBranch.AdvancedBranch.ExitNode.Offset;
			num = AdvMapNode.CalcSlope(curBranch.AdvancedBranch.EnterNode.AdjustedPos, curBranch.AdvancedBranch.FirstNode.AdjustedPos);
			num2 = AdvMapNode.CalcSlope(curBranch.AdvancedBranch.ExitNode.AdjustedPos, curBranch.AdvancedBranch.LastNode.AdjustedPos);
		}
		int[] personalitiesCost = GetPersonalitiesCost();
		value.Clear();
		List<AdvMapNodeNormal> list2 = new List<AdvMapNodeNormal>(curBranch.Nodes);
		bool flag = _curBranchChosenChar >= 0 && curBranch.AdvancedBranch != null;
		if (flag)
		{
			list2.AddRange(curBranch.AdvancedBranch.Nodes);
		}
		bool flag2 = flag && _arrangedNodes.Any(delegate(int a)
		{
			AdvMapNodeNormal normalNodeInCurBranch = GetNormalNodeInCurBranch(a);
			return normalNodeInCurBranch.Offset.X > curAdvanceBranchEnterPos.X;
		});
		foreach (AdvMapNodeNormal node in list2)
		{
			if (list.Any(delegate(AdvMapPos pos)
			{
				AdvMapPos advMapPos2 = node.Offset - pos;
				return Math.Abs(advMapPos2.Y) > Math.Abs(advMapPos2.X);
			}) || list.Contains(node.Offset))
			{
				continue;
			}
			if (flag)
			{
				bool flag3 = curBranch.AdvancedBranch.Nodes.Contains(node);
				if (flag3 && flag2)
				{
					continue;
				}
				if (!flag3 && hasArrangedOnAdvancedBranch)
				{
					if (node.Offset.X >= curAdvanceBranchEnterPos.X && node.Offset.X <= advMapPos.X)
					{
						continue;
					}
					float num3 = AdvMapNode.CalcSlope(curBranch.AdvancedBranch.FirstNode.AdjustedPos, node.AdjustedPos);
					if ((num > 0f && num3 > 0f && num < num3) || (num < 0f && num3 < 0f && num > num3))
					{
						continue;
					}
					float num4 = AdvMapNode.CalcSlope(curBranch.AdvancedBranch.LastNode.AdjustedPos, node.AdjustedPos);
					if ((num2 > 0f && num3 > 0f && num2 < num4) || (num2 < 0f && num3 < 0f && num2 > num4))
					{
						continue;
					}
				}
			}
			if (node.SevenElementType >= 0 && node.SevenElementCost <= _personalities[node.SevenElementType] + personalitiesCost[node.SevenElementType])
			{
				value.Add(node.AdjustedPos.GetHashCode());
			}
		}
		if (!flag)
		{
			return;
		}
		int hashCode = curBranch.AdvancedBranch.FirstNode.AdjustedPos.GetHashCode();
		bool flag4 = !value.Contains(hashCode) && !_arrangedNodes.Contains(hashCode);
		int hashCode2 = curBranch.AdvancedBranch.EnterNode.AdjustedPos.GetHashCode();
		bool flag5 = !value.Contains(hashCode2) && !_arrangedNodes.Contains(hashCode2);
		if (!(flag4 || flag5))
		{
			return;
		}
		value.RemoveAll((int pos) => curBranch.AdvancedBranch.Nodes.Exists((AdvMapNodeNormal n) => n.AdjustedPos.GetHashCode() == pos.GetHashCode()));
	}

	private void InitializeAdventureTypes()
	{
		AdventureTypes = new List<short>[Config.AdventureType.Instance.Count];
		for (int i = 0; i < AdventureTypes.Length; i++)
		{
			if (AdventureTypes[i] == null)
			{
				AdventureTypes[i] = new List<short>();
			}
			else
			{
				AdventureTypes[i].Clear();
			}
		}
		foreach (AdventureItem item in (IEnumerable<AdventureItem>)Config.Adventure.Instance)
		{
			AdventureTypes[item.Type].Add(item.TemplateId);
		}
	}

	public void InitializeBrokenAreaEnemies()
	{
		for (int i = 0; i < _brokenAreaEnemies.Length; i++)
		{
			_brokenAreaEnemies[i] = new BrokenAreaData();
		}
		_brokenAreaEnemyBaseCount = GlobalConfig.Instance.BrokenAreaEnemyCountLevelDist.Sum();
	}

	public void GenerateBrokenAreaInitialEnemies(DataContext context)
	{
		int hereticsAmountFactor = DomainManager.World.GetHereticsAmountFactor();
		List<MapBlockData> validBlocks = new List<MapBlockData>();
		for (int i = 0; i < _brokenAreaEnemies.Length; i += 6)
		{
			CollectionUtils.Shuffle(context.Random, _stateBrokenAreaLevels);
			for (int j = 0; j < 6; j++)
			{
				int num = i + j;
				short areaId = (short)(45 + num);
				_brokenAreaEnemies[num].Level = _stateBrokenAreaLevels[j];
				DomainManager.Adventure.GetValidBlocksForRandomEnemy(areaId, -1, 3, onAdventureSite: false, onSettlement: false, nearTaiwu: false, validBlocks);
				for (int k = 0; k < 3; k++)
				{
					short enemyTemplateId = (short)(298 + k + _brokenAreaEnemies[num].Level - 1);
					int enemyAmount = GlobalConfig.Instance.BrokenAreaEnemyCountLevelDist[k] * hereticsAmountFactor / 100;
					CreateRandomEnemiesOnValidBlocks(context, Location.Invalid, enemyTemplateId, enemyAmount, validBlocks);
				}
				SetElement_BrokenAreaEnemies(num, _brokenAreaEnemies[num], context);
			}
		}
		DomainManager.Extra.InitializeTreasureMaterials(context);
	}

	public short GetSwordTombAdventureMaxMonthCount()
	{
		byte bossInvasionSpeedType = DomainManager.World.GetBossInvasionSpeedType();
		return GlobalConfig.Instance.SwordTombAdventureLastMonthCount[bossInvasionSpeedType];
	}

	public void StartNextSwordTombCountDown()
	{
		sbyte[] xiangshuAvatarTasksInOrder = DomainManager.World.GetXiangshuAvatarTasksInOrder();
		AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(EventArgBox.TaiwuVillageAreaId);
		short swordTombAdventureMaxMonthCount = GetSwordTombAdventureMaxMonthCount();
		Dictionary<sbyte, KeyValuePair<short, AdventureSiteData>> dictionary = new Dictionary<sbyte, KeyValuePair<short, AdventureSiteData>>();
		foreach (KeyValuePair<short, AdventureSiteData> adventureSite in adventuresInArea.AdventureSites)
		{
			sbyte xiangshuAvatarIdBySwordTomb = XiangshuAvatarIds.GetXiangshuAvatarIdBySwordTomb(adventureSite.Value.TemplateId);
			if (xiangshuAvatarIdBySwordTomb < 0)
			{
				continue;
			}
			if (adventureSite.Value.RemainingMonths > 0)
			{
				if (swordTombAdventureMaxMonthCount < 0)
				{
					DomainManager.Adventure.SetAdventureRemainingMonths(DomainManager.TaiwuEvent.MainThreadDataContext, EventArgBox.TaiwuVillageAreaId, adventureSite.Key, swordTombAdventureMaxMonthCount);
				}
				return;
			}
			dictionary.Add(xiangshuAvatarIdBySwordTomb, adventureSite);
		}
		foreach (sbyte key in xiangshuAvatarTasksInOrder)
		{
			if (dictionary.TryGetValue(key, out var value))
			{
				DomainManager.Adventure.SetAdventureRemainingMonths(DomainManager.TaiwuEvent.MainThreadDataContext, EventArgBox.TaiwuVillageAreaId, value.Key, swordTombAdventureMaxMonthCount);
				break;
			}
		}
	}

	public void StopAllSwordTombAdventureCountDown(DataContext context)
	{
		AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(EventArgBox.TaiwuVillageAreaId);
		foreach (KeyValuePair<short, AdventureSiteData> adventureSite in adventuresInArea.AdventureSites)
		{
			if (XiangshuAvatarIds.GetXiangshuAvatarIdBySwordTomb(adventureSite.Value.TemplateId) >= 0)
			{
				adventureSite.Value.RemainingMonths = -1;
				SetElement_AdventureAreas(EventArgBox.TaiwuVillageAreaId, _adventureAreas[EventArgBox.TaiwuVillageAreaId], context);
			}
		}
	}

	public bool IsAdventureVisible(short adventureTemplateId)
	{
		int i = 0;
		for (int num = _adventureAreas.Length; i < num; i++)
		{
			AreaAdventureData areaAdventureData = _adventureAreas[i];
			foreach (KeyValuePair<short, AdventureSiteData> adventureSite in areaAdventureData.AdventureSites)
			{
				if (adventureSite.Value.TemplateId == adventureTemplateId)
				{
					return adventureSite.Value.SiteState == 1;
				}
			}
		}
		return false;
	}

	public void GetCharactersInAdventure(Location location, HashSet<int> charIds)
	{
		charIds.Clear();
		AreaAdventureData areaAdventureData = _adventureAreas[location.AreaId];
		if (!areaAdventureData.AdventureSites.ContainsKey(location.BlockId))
		{
			return;
		}
		AdventureSiteData adventureSiteData = areaAdventureData.AdventureSites[location.BlockId];
		MonthlyActionBase monthlyAction = DomainManager.TaiwuEvent.GetMonthlyAction(adventureSiteData.MonthlyActionKey);
		if (monthlyAction == null)
		{
			return;
		}
		if (monthlyAction is IMonthlyActionGroup monthlyActionGroup)
		{
			ConfigMonthlyAction configAction = monthlyActionGroup.GetConfigAction(location.AreaId, location.BlockId);
			if (configAction == null)
			{
				return;
			}
			configAction.CollectCalledCharacters(charIds);
		}
		else
		{
			monthlyAction.CollectCalledCharacters(charIds);
		}
		charIds.RemoveWhere(delegate(int charId)
		{
			if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
			{
				return true;
			}
			return !element.GetLocation().Equals(location);
		});
	}

	[DomainMethod]
	public List<CharacterDisplayData> GetCharacterDisplayDataListInAdventure(Location location)
	{
		HashSet<int> hashSet = new HashSet<int>();
		GetCharactersInAdventure(location, hashSet);
		return DomainManager.Character.GetCharacterDisplayDataList(hashSet.ToList());
	}

	[DomainMethod]
	public AreaAdventureData GetAdventuresInArea(short areaId)
	{
		return _adventureAreas[areaId];
	}

	[DomainMethod]
	public Location QueryAdventureLocation(short templateId)
	{
		for (short num = 0; num < _adventureAreas.Length; num++)
		{
			if (TryQueryAdventureLocation(templateId, num, out var location))
			{
				return location;
			}
		}
		return Location.Invalid;
	}

	[DomainMethod]
	public Location QueryAdventureLocationFirstInCurrent(short templateId)
	{
		short areaId = DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId;
		if (areaId >= 0 && areaId < _adventureAreas.Length && TryQueryAdventureLocation(templateId, areaId, out var location))
		{
			return location;
		}
		return QueryAdventureLocation(templateId);
	}

	private bool TryQueryAdventureLocation(short templateId, short areaId, out Location location)
	{
		AreaAdventureData areaAdventureData = _adventureAreas[areaId];
		foreach (KeyValuePair<short, AdventureSiteData> adventureSite in areaAdventureData.AdventureSites)
		{
			if (adventureSite.Value.TemplateId != templateId)
			{
				continue;
			}
			location = new Location(areaId, adventureSite.Key);
			return true;
		}
		location = Location.Invalid;
		return false;
	}

	[DomainMethod]
	public Dictionary<Location, AdventureSiteData> GetAdventureSiteDataDict(List<Location> locationList)
	{
		Dictionary<Location, AdventureSiteData> dictionary = new Dictionary<Location, AdventureSiteData>();
		if (locationList != null)
		{
			foreach (Location location in locationList)
			{
				if (_adventureAreas[location.AreaId].AdventureSites.TryGetValue(location.BlockId, out var value))
				{
					dictionary.Add(location, value);
				}
			}
		}
		return dictionary;
	}

	[DomainMethod]
	public List<short> GetAwakeSwordTombs()
	{
		List<short> list = new List<short>();
		short areaId = DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId;
		AreaAdventureData areaAdventureData = _adventureAreas[areaId];
		foreach (var (num2, adventureSiteData2) in areaAdventureData.AdventureSites)
		{
			if (XiangshuAvatarIds.GetXiangshuAvatarIdBySwordTomb(adventureSiteData2.TemplateId) >= 0 && adventureSiteData2.RemainingMonths > 0)
			{
				list.Add(adventureSiteData2.TemplateId);
			}
		}
		return list;
	}

	[DomainMethod]
	public List<short> GetAttackingSwordTombs()
	{
		List<short> list = new List<short>();
		short areaId = DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId;
		AreaAdventureData areaAdventureData = _adventureAreas[areaId];
		foreach (KeyValuePair<short, AdventureSiteData> adventureSite in areaAdventureData.AdventureSites)
		{
			adventureSite.Deconstruct(out var key, out var value);
			short num = key;
			AdventureSiteData adventureSiteData = value;
			sbyte xiangshuAvatarIdBySwordTomb = XiangshuAvatarIds.GetXiangshuAvatarIdBySwordTomb(adventureSiteData.TemplateId);
			if (xiangshuAvatarIdBySwordTomb >= 0 && adventureSiteData.RemainingMonths <= 0)
			{
				sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
				short currentLevelXiangshuTemplateId = XiangshuAvatarIds.GetCurrentLevelXiangshuTemplateId(xiangshuAvatarIdBySwordTomb, xiangshuLevel, isWeakened: true);
				if (DomainManager.Character.TryGetFixedCharacterByTemplateId(currentLevelXiangshuTemplateId, out var character) && character.GetLocation().IsValid())
				{
					list.Add(adventureSiteData.TemplateId);
				}
			}
		}
		return list;
	}

	public bool TryCreateAdventureSite(DataContext context, short areaId, short blockId, short adventureId, MonthlyActionKey monthlyActionKey)
	{
		AdventureItem adventureItem = Config.Adventure.Instance[adventureId];
		if (adventureItem.RestrictedByWorldPopulation && !CheckPassPopulationRestriction(context.Random))
		{
			return false;
		}
		AdventureSiteData adventureSiteData = new AdventureSiteData(adventureId, adventureItem.KeepTime, monthlyActionKey);
		if (_adventureAreas[areaId].AdventureSites.TryGetValue(blockId, out var value))
		{
			AdventureItem adventureItem2 = Config.Adventure.Instance[value.TemplateId];
			if (value.TemplateId == adventureId)
			{
				Logger.Warn($"Skipping Recreation of duplicate adventure {adventureItem.Name} at ({areaId}, {blockId}).");
				return true;
			}
			Tester.Assert(Config.AdventureType.Instance[adventureItem2.Type].IsTrivial, $"({areaId},{blockId}) already contains an adventure {adventureItem2.Name} which cannot be overwritten by {adventureItem.Name}.");
			Tester.Assert(!Config.AdventureType.Instance[adventureItem.Type].IsTrivial, $"Trying to overwrite adventure {adventureItem2.Name} at ({areaId},{blockId}) with a trivial adventure {adventureItem.Name}.");
			RemoveAdventureSite(context, areaId, blockId, isTimeout: false, isComplete: false);
		}
		_adventureAreas[areaId].AdventureSites.Add(blockId, adventureSiteData);
		if (adventureSiteData.IsEnemyNest())
		{
			sbyte enemyNestTemplateId = GetEnemyNestTemplateId(adventureId);
			RegisterEnemyNest(enemyNestTemplateId);
			AddElement_EnemyNestSites(new Location(areaId, blockId), new EnemyNestSiteExtraData(enemyNestTemplateId), context);
		}
		SetElement_AdventureAreas(areaId, _adventureAreas[areaId], context);
		Events.RaiseAdventureSiteStateChanged(context, areaId, blockId, adventureSiteData);
		return true;
	}

	public void ActivateAdventureSite(DataContext context, short areaId, short blockId)
	{
		AdventureSiteData adventureSiteData = _adventureAreas[areaId].AdventureSites[blockId];
		if (adventureSiteData.SiteState != 0)
		{
			throw new Exception($"Adventure Site at area {areaId} and block {blockId} is not waiting to be activated.");
		}
		adventureSiteData.SiteState = 1;
		SetElement_AdventureAreas(areaId, _adventureAreas[areaId], context);
		if (adventureSiteData.IsEnemyNest())
		{
			GenerateRandomEnemiesBySite(context, adventureSiteData, areaId, blockId);
		}
		Events.RaiseAdventureSiteStateChanged(context, areaId, blockId, adventureSiteData);
	}

	public void DeactivateAdventureSite(DataContext context, short areaId, short blockId)
	{
		AdventureSiteData adventureSiteData = _adventureAreas[areaId].AdventureSites[blockId];
		adventureSiteData.SiteState = 0;
		SetElement_AdventureAreas(areaId, _adventureAreas[areaId], context);
	}

	public void SetAdventureSiteRemainingMonths(DataContext context, short areaId, short blockId, sbyte remainingMonths)
	{
		AdventureSiteData adventureSiteData = _adventureAreas[areaId].AdventureSites[blockId];
		adventureSiteData.RemainingMonths = remainingMonths;
		SetElement_AdventureAreas(areaId, _adventureAreas[areaId], context);
	}

	private void ClearRandomEnemiesBySite(DataContext context, Location nestLocation)
	{
		EnemyNestSiteExtraData element_EnemyNestSites = GetElement_EnemyNestSites(nestLocation);
		foreach (MapTemplateEnemyInfo randomEnemy in element_EnemyNestSites.RandomEnemies)
		{
			DomainManager.Map.OnTemplateEnemyLocationChanged(context, randomEnemy, new Location(nestLocation.AreaId, randomEnemy.BlockId), Location.Invalid);
		}
		element_EnemyNestSites.RandomEnemies.Clear();
		SetElement_EnemyNestSites(nestLocation, element_EnemyNestSites, context);
	}

	private void GenerateRandomEnemiesBySite(DataContext context, AdventureSiteData advSite, short areaId, short blockId)
	{
		Location sourceNestLocation = new Location(areaId, blockId);
		short templateId = EnemyNest.Instance.First((EnemyNestItem nest) => nest.AdventureId == advSite.TemplateId).TemplateId;
		List<short> members = EnemyNest.Instance[templateId].Members;
		List<short> spawnAmountFactors = EnemyNest.Instance[templateId].SpawnAmountFactors;
		int hereticsAmountFactor = DomainManager.World.GetHereticsAmountFactor();
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		DomainManager.Adventure.GetValidBlocksForRandomEnemy(areaId, blockId, 3, onAdventureSite: false, onSettlement: false, nearTaiwu: false, list);
		if (templateId == 3)
		{
			sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(areaId);
			sbyte sectID = MapState.Instance[stateTemplateIdByAreaId].SectID;
			int num = sectID - 1;
			DomainManager.Adventure.SetAdventureSiteInitData(context, areaId, blockId, num);
			if (list.Count > 0)
			{
				DomainManager.Adventure.CreateRandomEnemiesOnValidBlocks(context, sourceNestLocation, members[num], spawnAmountFactors[num] * hereticsAmountFactor / 100, list);
			}
		}
		else if (list.Count > 0)
		{
			for (int num2 = 0; num2 < members.Count; num2++)
			{
				DomainManager.Adventure.CreateRandomEnemiesOnValidBlocks(context, sourceNestLocation, members[num2], spawnAmountFactors[num2] * hereticsAmountFactor / 100, list);
			}
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
	}

	[Obsolete("Use GameData.Domains.Map.MapDomain.GetRandomMapBlockDataByFilters instead.")]
	public MapBlockData GetRandomMapBlockDataByFilters(DataContext context, sbyte stateTemplateId, sbyte areaFilterType, List<short> mapBlockSubTypes)
	{
		return DomainManager.Map.GetRandomMapBlockDataByFilters(context.Random, stateTemplateId, areaFilterType, mapBlockSubTypes, includeBlockWithAdventure: false);
	}

	[Obsolete("use GameData.Domains.Map.MapDomain.GetMapBlocksInAreaByFilters instead.")]
	public void GetMapBlocksInAreaByFilters(short areaId, Predicate<MapBlockData> predicate, List<MapBlockData> result)
	{
		DomainManager.Map.GetMapBlocksInAreaByFilters(areaId, predicate, result);
	}

	[Obsolete("use GameData.Domains.Map.MapDomain.GetRandomMapBlockDataInAreaByFilters")]
	public MapBlockData GetRandomMapBlockDataInAreaByFilters(DataContext context, short areaId, List<short> mapBlockSubTypes)
	{
		return DomainManager.Map.GetRandomMapBlockDataInAreaByFilters(context.Random, areaId, mapBlockSubTypes, includeBlocksWithAdventure: false);
	}

	public void GetValidBlocksForRandomEnemy(short areaId, short centerBlockId, short maxSteps, bool onAdventureSite, bool onSettlement, bool nearTaiwu, List<MapBlockData> validBlocks)
	{
		validBlocks.Clear();
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		Location location = DomainManager.Taiwu.GetTaiwu()?.GetLocation() ?? Location.Invalid;
		if (!nearTaiwu && location.IsValid())
		{
			DomainManager.Map.GetRealNeighborBlocks(location.AreaId, location.BlockId, list);
		}
		Dictionary<short, AdventureSiteData> adventureSites = _adventureAreas[areaId].AdventureSites;
		if (centerBlockId != -1)
		{
			List<MapBlockData> list2 = ObjectPool<List<MapBlockData>>.Instance.Get();
			DomainManager.Map.GetRealNeighborBlocks(areaId, centerBlockId, list2, maxSteps);
			foreach (MapBlockData item in list2)
			{
				if ((nearTaiwu || location.AreaId != areaId || (location.BlockId != item.BlockId && !list.Contains(item))) && (onAdventureSite || !adventureSites.ContainsKey(item.BlockId)) && (onSettlement || (!item.IsCityTown() && item.BlockType != EMapBlockType.Station)) && MapBlockDataMatchers.IsValidForRandomEnemy(item))
				{
					validBlocks.Add(item);
				}
			}
			list2.Clear();
			ObjectPool<List<MapBlockData>>.Instance.Return(list2);
		}
		else
		{
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
			Span<MapBlockData> span = areaBlocks;
			for (int i = 0; i < span.Length; i++)
			{
				MapBlockData mapBlockData = span[i];
				if ((nearTaiwu || location.AreaId != areaId || (location.BlockId != mapBlockData.BlockId && !list.Contains(mapBlockData))) && (onAdventureSite || !adventureSites.ContainsKey(mapBlockData.BlockId)) && (onSettlement || (!mapBlockData.IsCityTown() && mapBlockData.BlockType != EMapBlockType.Station)) && MapBlockDataMatchers.IsValidForRandomEnemy(mapBlockData))
				{
					validBlocks.Add(mapBlockData);
				}
			}
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
	}

	public void CreateRandomEnemiesOnValidBlocks(DataContext context, Location sourceNestLocation, short enemyTemplateId, int enemyAmount, IList<MapBlockData> validBlocks)
	{
		CreateTemporaryEnemiesOnValidBlocks(context, sourceNestLocation, enemyTemplateId, enemyAmount, validBlocks, -1);
	}

	public void CreateTemporaryEnemiesOnValidBlocks(DataContext context, Location sourceNestLocation, short enemyTemplateId, int enemyAmount, IList<MapBlockData> validBlocks, sbyte duration = sbyte.MinValue)
	{
		if (enemyTemplateId <= 0)
		{
			return;
		}
		byte creatingType = Config.Character.Instance[enemyTemplateId].CreatingType;
		bool condition = (uint)(creatingType - 2) <= 1u;
		Tester.Assert(condition);
		if (sourceNestLocation.IsValid() && duration != -1)
		{
			AdaptableLog.TagWarning("CreateTemporaryEnemiesOnValidBlocks", $"Creating Enemy with sourceNestLocation and duration = {duration} is invalid, changing duration to -1:\n{new StackTrace()}");
			duration = -1;
		}
		else if (duration == sbyte.MinValue)
		{
			duration = MapTemplateEnemyInfo.DefaultDuration(DomainManager.Taiwu.GetTaiwu().GetConsummateLevel());
		}
		for (int i = 0; i < enemyAmount; i++)
		{
			if (validBlocks.Count == 0)
			{
				break;
			}
			int index = context.Random.Next(0, validBlocks.Count);
			Location destLocation = new Location(validBlocks[index].AreaId, validBlocks[index].BlockId);
			MapTemplateEnemyInfo templateEnemyInfo = new MapTemplateEnemyInfo(enemyTemplateId, destLocation.BlockId, sourceNestLocation.BlockId, duration);
			Events.RaiseTemplateEnemyLocationChanged(context, templateEnemyInfo, Location.Invalid, destLocation);
		}
	}

	private void InitializeAdventureAreas()
	{
		for (int i = 0; i < _adventureAreas.Length; i++)
		{
			_adventureAreas[i] = new AreaAdventureData();
		}
	}

	public void ClearAllAdventureSites(DataContext context)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		for (short num = 0; num < _adventureAreas.Length; num++)
		{
			AreaAdventureData areaAdventureData = _adventureAreas[num];
			list.AddRange(areaAdventureData.AdventureSites.Keys);
			foreach (short item in list)
			{
				RemoveAdventureSite(context, num, item, isTimeout: false, isComplete: false);
			}
		}
	}

	public void RemoveAdventureSite(DataContext context, short areaId, short blockId, bool isTimeout, bool isComplete)
	{
		if (!_adventureAreas[areaId].AdventureSites.TryGetValue(blockId, out var value))
		{
			return;
		}
		value.OnDestroy();
		if (value.IsEnemyNest())
		{
			Location location = new Location(areaId, blockId);
			ClearRandomEnemiesBySite(context, location);
			RemoveElement_EnemyNestSites(location, context);
			if (value.SiteState <= 1)
			{
				sbyte enemyNestTemplateId = GetEnemyNestTemplateId(value.TemplateId);
				UnregisterEnemyNest(enemyNestTemplateId);
			}
		}
		_adventureAreas[areaId].AdventureSites.Remove(blockId);
		SetElement_AdventureAreas(areaId, _adventureAreas[areaId], context);
		Events.RaiseAdventureRemoved(context, areaId, blockId, isTimeout, isComplete, value);
	}

	[Obsolete]
	public void TransferAdventure(DataContext context, Location from, Location to)
	{
	}

	public void RemoveAllJuniorXiangshuAdventures(DataContext context)
	{
		List<short> list = new List<short>();
		for (short num = 0; num < _adventureAreas.Length; num++)
		{
			AreaAdventureData areaAdventureData = _adventureAreas[num];
			if (areaAdventureData != null && areaAdventureData.AdventureSites.Count > 0)
			{
				list.Clear();
				foreach (KeyValuePair<short, AdventureSiteData> adventureSite in areaAdventureData.AdventureSites)
				{
					if (adventureSite.Value.TemplateId >= 117 && adventureSite.Value.TemplateId <= 134)
					{
						list.Add(adventureSite.Key);
					}
				}
				if (list.Count > 0)
				{
					int i = 0;
					for (int count = list.Count; i < count; i++)
					{
						RemoveAdventureSite(context, num, list[i], isTimeout: false, isComplete: false);
					}
				}
			}
		}
	}

	public void SetAdventureRemainingMonths(DataContext context, short areaId, short blockId, short remainingMonths)
	{
		_adventureAreas[areaId].AdventureSites[blockId].RemainingMonths = remainingMonths;
		SetElement_AdventureAreas(areaId, _adventureAreas[areaId], context);
	}

	public void SetAdventureSiteInitData(DataContext context, short areaId, short blockId, int initData)
	{
		_adventureAreas[areaId].AdventureSites[blockId].SiteInitData = initData;
		SetElement_AdventureAreas(areaId, _adventureAreas[areaId], context);
	}

	public int GetAdventureSiteInitData(short areaId, short blockId)
	{
		return _adventureAreas[areaId].AdventureSites[blockId].SiteInitData;
	}

	public AdventureSiteData GetAdventureSite(short areaId, short blockId)
	{
		if (areaId < 0 || areaId >= _adventureAreas.Length)
		{
			return null;
		}
		AreaAdventureData areaAdventureData = _adventureAreas[areaId];
		return areaAdventureData.AdventureSites.GetValueOrDefault(blockId);
	}

	public Location GetAdventureLocationByTemplateId(short templateId)
	{
		for (short num = 0; num < _adventureAreas.Count(); num++)
		{
			foreach (KeyValuePair<short, AdventureSiteData> adventureSite in _adventureAreas[num].AdventureSites)
			{
				if (adventureSite.Value.TemplateId == templateId)
				{
					return new Location(num, adventureSite.Key);
				}
			}
		}
		return Location.Invalid;
	}

	public sbyte GetAdventureSiteState(short areaId, short blockId)
	{
		AdventureSiteData value;
		return (sbyte)(_adventureAreas[areaId].AdventureSites.TryGetValue(blockId, out value) ? value.SiteState : (-1));
	}

	public void UpdateAdventuresInArea(DataContext context, short areaId)
	{
		short[] array = _adventureAreas[areaId].AdventureSites.Keys.ToArray();
		short[] array2 = array;
		foreach (short num in array2)
		{
			AdventureSiteData adventureSiteData = _adventureAreas[areaId].AdventureSites[num];
			Location location = new Location(areaId, num);
			if (adventureSiteData.SiteState == 1)
			{
				if (adventureSiteData.RemainingMonths > 0)
				{
					adventureSiteData.RemainingMonths--;
				}
				if (adventureSiteData.RemainingMonths != 0)
				{
					continue;
				}
				sbyte xiangshuAvatarIdBySwordTomb = XiangshuAvatarIds.GetXiangshuAvatarIdBySwordTomb(adventureSiteData.TemplateId);
				if (xiangshuAvatarIdBySwordTomb >= 0)
				{
					SetAdventureRemainingMonths(context, areaId, num, -1);
					sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
					short currentLevelXiangshuTemplateId = XiangshuAvatarIds.GetCurrentLevelXiangshuTemplateId(xiangshuAvatarIdBySwordTomb, xiangshuLevel, isWeakened: true);
					GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId = EventHelper.GetOrCreateFixedCharacterByTemplateId(currentLevelXiangshuTemplateId);
					List<Location> blockLocationGroup = EventHelper.GetBlockLocationGroup(location);
					EventHelper.MoveFixedCharacter(orCreateFixedCharacterByTemplateId, blockLocationGroup.Sample());
					if (!DomainManager.Extra.IsOneShotEventHandled(44) && DomainManager.Taiwu.GetTaiwu().GetConsummateLevel() < 4 && !DomainManager.Extra.IsDreamBack())
					{
						DomainManager.World.GetMonthlyEventCollection().AddWardOffXiangshuProtection(DomainManager.Taiwu.GetTaiwuCharId(), adventureSiteData.TemplateId);
					}
				}
				else
				{
					RemoveAdventureSite(context, areaId, num, isTimeout: true, isComplete: false);
				}
			}
			else if (adventureSiteData.SiteState >= 2)
			{
				if (adventureSiteData.RemainingMonths > 0)
				{
					adventureSiteData.RemainingMonths--;
				}
				if (adventureSiteData.RemainingMonths == 0)
				{
					RemoveAdventureSite(context, areaId, num, isTimeout: true, isComplete: false);
					continue;
				}
				EnemyNestSiteExtraData enemyNestSiteExtraData = _enemyNestSites[location];
				OfflineAddConqueredEnemyNestIncome(context, adventureSiteData, enemyNestSiteExtraData);
				SetElement_EnemyNestSites(location, enemyNestSiteExtraData, context);
			}
		}
		SetElement_AdventureAreas(areaId, _adventureAreas[areaId], context);
	}

	public void ComplementEnemiesBySite(DataContext context, AdventureSiteData advSite, short areaId, short blockId, bool ignoreCountLimit = false)
	{
		Location location = new Location(areaId, blockId);
		if (!_enemyNestSites.TryGetValue(location, out var value))
		{
			return;
		}
		sbyte enemyNestTemplateId = GetEnemyNestTemplateId(advSite.TemplateId);
		List<short> members = EnemyNest.Instance[enemyNestTemplateId].Members;
		List<short> spawnAmountFactors = EnemyNest.Instance[enemyNestTemplateId].SpawnAmountFactors;
		int hereticsAmountFactor = DomainManager.World.GetHereticsAmountFactor();
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		DomainManager.Adventure.GetValidBlocksForRandomEnemy(areaId, blockId, 3, onAdventureSite: false, onSettlement: false, nearTaiwu: false, list);
		if (enemyNestTemplateId == 3)
		{
			sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(areaId);
			sbyte sectID = MapState.Instance[stateTemplateIdByAreaId].SectID;
			int num = sectID - 1;
			DomainManager.Adventure.SetAdventureSiteInitData(context, areaId, blockId, num);
			int num2 = spawnAmountFactors[num] * hereticsAmountFactor / 100;
			int count = value.RandomEnemies.Count;
			if (list.Count > 0 && num2 > count)
			{
				DomainManager.Adventure.CreateRandomEnemiesOnValidBlocks(context, location, members[num], 1, list);
			}
		}
		else if (list.Count > 0)
		{
			Span<int> span = stackalloc int[members.Count];
			Span<int> pArray = stackalloc int[members.Count];
			for (int i = 0; i < members.Count; i++)
			{
				span[i] = 0;
				pArray[i] = i;
			}
			CollectionUtils.Shuffle(context.Random, pArray, members.Count);
			foreach (MapTemplateEnemyInfo randomEnemy in value.RandomEnemies)
			{
				int num3 = members.IndexOf(randomEnemy.TemplateId);
				if (num3 >= 0)
				{
					span[num3]++;
				}
			}
			for (int j = 0; j < members.Count; j++)
			{
				int index = pArray[j];
				int num4 = spawnAmountFactors[index] * hereticsAmountFactor / 100;
				int num5 = span[index];
				if (num4 > num5)
				{
					DomainManager.Adventure.CreateRandomEnemiesOnValidBlocks(context, location, members[index], 1, list);
					ObjectPool<List<MapBlockData>>.Instance.Return(list);
					return;
				}
			}
			if (ignoreCountLimit)
			{
				short random = members.GetRandom(context.Random);
				DomainManager.Adventure.CreateRandomEnemiesOnValidBlocks(context, location, random, 1, list);
			}
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
	}

	public void ComplementEnemiesInBrokenArea(DataContext context, short areaId)
	{
		bool flag = false;
		int num = areaId - 45;
		int hereticsAmountFactor = DomainManager.World.GetHereticsAmountFactor();
		Span<int> span = stackalloc int[GlobalConfig.Instance.BrokenAreaEnemyCountLevelDist.Length];
		span.Fill(0);
		BrokenAreaData brokenAreaData = _brokenAreaEnemies[num];
		if (brokenAreaData.RandomEnemies.Count >= _brokenAreaEnemyBaseCount * hereticsAmountFactor / 100)
		{
			return;
		}
		foreach (MapTemplateEnemyInfo randomEnemy in brokenAreaData.RandomEnemies)
		{
			int num2 = randomEnemy.TemplateId - brokenAreaData.BaseXiangshuMinionTemplateId;
			if (num2 >= 0 && num2 < span.Length)
			{
				span[num2]++;
			}
		}
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		list.Clear();
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		for (int i = 0; i < areaBlocks.Length; i++)
		{
			MapBlockData mapBlockData = areaBlocks[i];
			if (location != mapBlockData.GetLocation())
			{
				list.Add(mapBlockData);
			}
		}
		for (int j = 0; j < 3; j++)
		{
			int num3 = GlobalConfig.Instance.BrokenAreaEnemyCountLevelDist[j] * hereticsAmountFactor / 100 - span[j];
			if (num3 > 0)
			{
				short enemyTemplateId = (short)Math.Clamp(j + brokenAreaData.BaseXiangshuMinionTemplateId, 298, 306);
				CreateRandomEnemiesOnValidBlocks(context, Location.Invalid, enemyTemplateId, 1, list);
				flag = true;
				break;
			}
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		if (flag)
		{
			SetElement_BrokenAreaEnemies(num, brokenAreaData, context);
		}
	}

	public void PreAdvanceMonth_UpdateRandomEnemies(DataContext context, int areaId)
	{
		PreAdvanceMonthRandomEnemiesModification preAdvanceMonthRandomEnemiesModification = new PreAdvanceMonthRandomEnemiesModification
		{
			AreaId = (short)areaId
		};
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(preAdvanceMonthRandomEnemiesModification.AreaId);
		CharacterMatcherItem canBeAttackedByRandomEnemy = CharacterMatcher.DefValue.CanBeAttackedByRandomEnemy;
		Span<MapBlockData> span = areaBlocks;
		short key;
		for (int i = 0; i < span.Length; i++)
		{
			MapBlockData mapBlockData = span[i];
			if (mapBlockData.TemplateEnemyList == null || mapBlockData.CharacterSet == null)
			{
				continue;
			}
			list.Clear();
			foreach (int item2 in mapBlockData.CharacterSet)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item2);
				if (canBeAttackedByRandomEnemy.Match(element_Objects))
				{
					list.Add(item2);
				}
			}
			if (list.Count == 0)
			{
				continue;
			}
			foreach (MapTemplateEnemyInfo templateEnemy in mapBlockData.TemplateEnemyList)
			{
				key = templateEnemy.TemplateId;
				if (key >= 298 && key <= 306 && templateEnemy.SourceAdventureBlockId >= 0)
				{
					Location location = new Location((short)areaId, templateEnemy.SourceAdventureBlockId);
					if (DomainManager.Extra.TryGetHeavenlyTreeByLocation(location, out var _))
					{
						continue;
					}
				}
				int num = list[context.Random.Next(list.Count)];
				if (Config.Character.Instance[templateEnemy.TemplateId].OrganizationInfo.OrgTemplateId == 18)
				{
					GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(num);
					sbyte fameType = element_Objects2.GetFameType();
					if (!FameType.IsNonNegative(fameType))
					{
						preAdvanceMonthRandomEnemiesModification.RandomEnemyAttackRecords.Add((num, templateEnemy));
					}
				}
				else
				{
					preAdvanceMonthRandomEnemiesModification.RandomEnemyAttackRecords.Add((num, templateEnemy));
				}
			}
		}
		if (DomainManager.Extra.TryGetAnimalAreaDataByAreaId((short)areaId, out var animalAreaData))
		{
			foreach (KeyValuePair<short, List<int>> item3 in animalAreaData)
			{
				item3.Deconstruct(out key, out var value);
				short index = key;
				List<int> list2 = value;
				MapBlockData mapBlockData2 = areaBlocks[index];
				if (mapBlockData2.CharacterSet == null)
				{
					continue;
				}
				list.Clear();
				foreach (int item4 in mapBlockData2.CharacterSet)
				{
					GameData.Domains.Character.Character element_Objects3 = DomainManager.Character.GetElement_Objects(item4);
					if (element_Objects3.GetAgeGroup() != 0)
					{
						list.Add(item4);
					}
				}
				if (list.Count == 0)
				{
					continue;
				}
				foreach (int item5 in list2)
				{
					if (DomainManager.Extra.TryGetAnimal(item5, out var animal) && DomainManager.Extra.IsAnimalAbleToAttack(animal, isTaiwuVictim: false))
					{
						int item = list[context.Random.Next(list.Count)];
						preAdvanceMonthRandomEnemiesModification.AnimalAttackRecords.Add((item, item5));
					}
				}
			}
		}
		ObjectPool<List<int>>.Instance.Return(list);
		ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
		parallelModificationsRecorder.RecordType(ParallelModificationType.PreAdvanceMonthUpdateRandomEnemies);
		parallelModificationsRecorder.RecordParameterClass(preAdvanceMonthRandomEnemiesModification);
	}

	public void ComplementPreAdvanceMonth_UpdateRandomEnemies(DataContext context, PreAdvanceMonthRandomEnemiesModification mod)
	{
		foreach (var randomEnemyAttackRecord in mod.RandomEnemyAttackRecords)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(randomEnemyAttackRecord.charId);
			Location srcLocation = new Location(mod.AreaId, randomEnemyAttackRecord.enemyInfo.BlockId);
			AiHelper.NpcCombatResultType npcCombatResultType = DomainManager.Character.SimulateEnemyAttack(context, randomEnemyAttackRecord.enemyInfo.TemplateId, element_Objects);
			if (npcCombatResultType == AiHelper.NpcCombatResultType.MajorDefeat || npcCombatResultType == AiHelper.NpcCombatResultType.MinorDefeat)
			{
				Events.RaiseTemplateEnemyLocationChanged(context, randomEnemyAttackRecord.enemyInfo, srcLocation, Location.Invalid);
			}
			else
			{
				DomainManager.Character.EscapeToRandomNearbyBlock(context, element_Objects);
			}
			element_Objects.SetInjuries(element_Objects.GetInjuries(), context);
		}
		foreach (var animalAttackRecord in mod.AnimalAttackRecords)
		{
			if (!DomainManager.Character.TryGetElement_Objects(animalAttackRecord.charId, out var element))
			{
				continue;
			}
			Location location = element.GetLocation();
			if (DomainManager.Extra.TryGetAnimalIdsByLocation(location, out var animals) && animals.Contains(animalAttackRecord.animal) && DomainManager.Extra.TryGetAnimal(animalAttackRecord.animal, out var animal))
			{
				AiHelper.NpcCombatResultType npcCombatResultType2 = DomainManager.Character.SimulateEnemyAttack(context, animal.CharacterTemplateId, element);
				if ((uint)(npcCombatResultType2 - 2) <= 1u)
				{
					DomainManager.Extra.ApplyAnimalDeadByAccident(context, animal.Id);
				}
				else
				{
					DomainManager.Character.EscapeToRandomNearbyBlock(context, element);
				}
			}
		}
	}

	private void UpdateNormalAreaRandomEnemiesMovement(DataContext context)
	{
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		List<MapTemplateEnemyInfo> list2 = ObjectPool<List<MapTemplateEnemyInfo>>.Instance.Get();
		foreach (KeyValuePair<Location, EnemyNestSiteExtraData> enemyNestSite in _enemyNestSites)
		{
			List<MapTemplateEnemyInfo> randomEnemies = enemyNestSite.Value.RandomEnemies;
			if (randomEnemies == null || randomEnemies.Count <= 0)
			{
				continue;
			}
			int num = (EnemyNestConstValues.RighteousStrongholdNestIds.Exist(enemyNestSite.Value.InitialNestTemplateId) ? 5 : 3);
			GetValidBlocksForRandomEnemy(enemyNestSite.Key.AreaId, enemyNestSite.Key.BlockId, (short)num, onAdventureSite: false, onSettlement: false, nearTaiwu: true, list);
			if (list.Count == 0)
			{
				continue;
			}
			list2.Clear();
			list2.AddRange(enemyNestSite.Value.RandomEnemies);
			foreach (MapTemplateEnemyInfo item in list2)
			{
				MapBlockData random = list.GetRandom(context.Random);
				if (item.BlockId != random.BlockId)
				{
					Events.RaiseTemplateEnemyLocationChanged(srcLocation: new Location(enemyNestSite.Key.AreaId, item.BlockId), destLocation: new Location(enemyNestSite.Key.AreaId, random.BlockId), context: context, templateEnemyInfo: item);
				}
			}
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		ObjectPool<List<MapTemplateEnemyInfo>>.Instance.Return(list2);
	}

	private void UpdateBrokenAreaRandomEnemiesMovement(DataContext context)
	{
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		List<MapTemplateEnemyInfo> list2 = ObjectPool<List<MapTemplateEnemyInfo>>.Instance.Get();
		for (int i = 0; i < _brokenAreaEnemies.Length; i++)
		{
			if (_brokenAreaEnemies[i].RandomEnemies == null)
			{
				return;
			}
			list2.Clear();
			list2.AddRange(_brokenAreaEnemies[i].RandomEnemies);
			short areaId = (short)(i + 45);
			foreach (MapTemplateEnemyInfo item in list2)
			{
				GetValidBlocksForRandomEnemy(areaId, item.BlockId, 3, onAdventureSite: false, onSettlement: false, nearTaiwu: true, list);
				MapBlockData random = list.GetRandom(context.Random);
				if (item.BlockId != random.BlockId)
				{
					Events.RaiseTemplateEnemyLocationChanged(srcLocation: new Location(areaId, item.BlockId), destLocation: new Location(areaId, random.BlockId), context: context, templateEnemyInfo: item);
				}
			}
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		ObjectPool<List<MapTemplateEnemyInfo>>.Instance.Return(list2);
	}

	public void CheckRandomEnemyAttackTaiwuOnAdvanceMonth()
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		if (!location.IsValid())
		{
			return;
		}
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		sbyte fame = taiwu.GetFame();
		sbyte fameType = FameType.GetFameType(fame);
		bool flag = DomainManager.Taiwu.CanTaiwuBeSneakyHarmfulActionTarget();
		MapBlockData block = DomainManager.Map.GetBlock(location);
		if (block.TemplateEnemyList != null && block.TemplateEnemyList.Count > 0)
		{
			MapTemplateEnemyInfo mapTemplateEnemyInfo = new MapTemplateEnemyInfo(-1, -1, -1);
			MapTemplateEnemyInfo mapTemplateEnemyInfo2 = new MapTemplateEnemyInfo(-1, -1, -1);
			sbyte b = -1;
			sbyte b2 = -1;
			foreach (MapTemplateEnemyInfo templateEnemy in block.TemplateEnemyList)
			{
				CharacterItem characterItem = Config.Character.Instance[templateEnemy.TemplateId];
				if (characterItem.OrganizationInfo.OrgTemplateId == 18)
				{
					if (flag && characterItem.OrganizationInfo.Grade > b2)
					{
						mapTemplateEnemyInfo2 = templateEnemy;
						b2 = characterItem.OrganizationInfo.Grade;
					}
				}
				else if (characterItem.OrganizationInfo.OrgTemplateId == 17)
				{
					if (flag && characterItem.OrganizationInfo.Grade > b)
					{
						mapTemplateEnemyInfo = templateEnemy;
						b = characterItem.OrganizationInfo.Grade;
					}
				}
				else if (characterItem.OrganizationInfo.OrgTemplateId == 19)
				{
					if ((templateEnemy.SourceAdventureBlockId < 0 || !DomainManager.Extra.TryGetHeavenlyTreeByLocation(location, out var _)) && characterItem.OrganizationInfo.Grade > b)
					{
						mapTemplateEnemyInfo = templateEnemy;
						b = characterItem.OrganizationInfo.Grade;
					}
				}
				else if (characterItem.OrganizationInfo.Grade > b)
				{
					mapTemplateEnemyInfo = templateEnemy;
					b = characterItem.OrganizationInfo.Grade;
				}
			}
			if (FameType.AttackByRighteous(fameType) && b2 >= 0 && !_escapingRandomEnemies.Contains(b))
			{
				monthlyEventCollection.AddRandomRighteousAttack(location, mapTemplateEnemyInfo2.TemplateId);
			}
			if (b >= 0 && !_escapingRandomEnemies.Contains(b))
			{
				monthlyEventCollection.AddRandomEnemyAttack(location, mapTemplateEnemyInfo.TemplateId);
			}
		}
		if (block.EnemyCharacterSet == null || block.EnemyCharacterSet.Count <= 0)
		{
			return;
		}
		int num = (3 + block.EnemyCharacterSet.Count) / 4;
		List<GameData.Domains.Character.Character> list = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
		list.Clear();
		foreach (int item in block.EnemyCharacterSet)
		{
			list.Add(DomainManager.Character.GetElement_Objects(item));
		}
		list.Sort((GameData.Domains.Character.Character charA, GameData.Domains.Character.Character charB) => -charA.GetCombatPower().CompareTo(charB.GetCombatPower()));
		for (int num2 = 0; num2 < num; num2++)
		{
			GameData.Domains.Character.Character character = list[num2];
		}
	}

	public void UpdateAdventuresInAllAreas(DataContext context)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		for (short num = 0; num < 45; num++)
		{
			UpdateAdventuresInArea(context, num);
		}
		for (short num2 = 45; num2 < 135; num2++)
		{
			UpdateAdventuresInArea(context, num2);
			ComplementEnemiesInBrokenArea(context, num2);
		}
		UpdateAdventuresInArea(context, 135);
		UpdateAdventuresInArea(context, 136);
		UpdateAdventuresInArea(context, 137);
		UpdateAdventuresInArea(context, 138);
		UpdateNormalAreaRandomEnemiesMovement(context);
		UpdateBrokenAreaRandomEnemiesMovement(context);
		stopwatch.Stop();
		Logger.Info($"PostAdvanceMonth.UpdateAdventuresInAllAreas: {stopwatch.Elapsed.TotalMilliseconds:N1}");
	}

	public void OnRandomEnemyLocationChange(DataContext context, MapTemplateEnemyInfo mapTemplateEnemyInfo, Location srcLocation, Location destLocation)
	{
		if (srcLocation.IsValid())
		{
			Location location = new Location(srcLocation.AreaId, mapTemplateEnemyInfo.SourceAdventureBlockId);
			if (location.IsValid())
			{
				if (_enemyNestSites.TryGetValue(location, out var value) && value.RandomEnemies.Remove(mapTemplateEnemyInfo))
				{
					SetElement_EnemyNestSites(location, value, context);
				}
			}
			else if (srcLocation.AreaId >= 45 && srcLocation.AreaId < 135)
			{
				int num = srcLocation.AreaId - 45;
				BrokenAreaData brokenAreaData = _brokenAreaEnemies[num];
				if (brokenAreaData.RandomEnemies.Remove(mapTemplateEnemyInfo))
				{
					SetElement_BrokenAreaEnemies(num, brokenAreaData, context);
				}
			}
		}
		if (!destLocation.IsValid())
		{
			return;
		}
		mapTemplateEnemyInfo.BlockId = destLocation.BlockId;
		Location location2 = new Location(destLocation.AreaId, mapTemplateEnemyInfo.SourceAdventureBlockId);
		if (location2.IsValid())
		{
			if (_enemyNestSites.TryGetValue(location2, out var value2))
			{
				value2.RandomEnemies.Add(mapTemplateEnemyInfo);
				SetElement_EnemyNestSites(location2, value2, context);
			}
		}
		else if (destLocation.AreaId >= 45 && destLocation.AreaId < 135)
		{
			int num2 = destLocation.AreaId - 45;
			BrokenAreaData brokenAreaData2 = _brokenAreaEnemies[num2];
			brokenAreaData2.RandomEnemies.Add(mapTemplateEnemyInfo);
			SetElement_BrokenAreaEnemies(num2, brokenAreaData2, context);
		}
	}

	public unsafe short GenerateDisasterAdventureId(IRandomSource random, MapBlockData mapBlockData)
	{
		if (!_adventureAreas[mapBlockData.AreaId].AdventureSites.ContainsKey(mapBlockData.BlockId) && random.Next(0, 100) < GlobalConfig.Instance.DisasterAdventureSpawnChance)
		{
			List<short> list = new List<short>();
			for (int i = 0; i < 6; i++)
			{
				List<short> disasterAdventureTypesByResourceType = GetDisasterAdventureTypesByResourceType(i);
				if (disasterAdventureTypesByResourceType.Count > 0 && mapBlockData.CurrResources.Items[i] >= 100)
				{
					short random2 = disasterAdventureTypesByResourceType.GetRandom(random);
					list.Add(random2);
				}
			}
			if (list.Count > 0)
			{
				return list.GetRandom(random);
			}
		}
		return -1;
	}

	public List<short> GetDisasterAdventureTypesByResourceType(int resourceType)
	{
		return AdventureTypes[9 + resourceType];
	}

	public static sbyte GetEnemyNestTemplateId(short adventureId)
	{
		foreach (EnemyNestItem item in (IEnumerable<EnemyNestItem>)EnemyNest.Instance)
		{
			if (item.AdventureId == adventureId)
			{
				return (sbyte)item.TemplateId;
			}
		}
		Logger.Warn($"adventureId {Config.Adventure.Instance[adventureId].Name}{adventureId} is not an heretic stronghold.");
		return 0;
	}

	public static bool CheckPassPopulationRestriction(IRandomSource randomSource)
	{
		float probAdjustOfCreatingCharacter = DomainManager.World.GetProbAdjustOfCreatingCharacter();
		return randomSource.NextFloat() < probAdjustOfCreatingCharacter;
	}

	public void TryCreateElopeWithLove(DataContext context)
	{
		EventArgBox globalEventArgumentBox = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
		bool flag = false;
		if (globalEventArgumentBox.Contains<int>("AwayForeverTime"))
		{
			int currDate = DomainManager.World.GetCurrDate();
			AdaptableLog.Info("远走高飞还剩" + (currDate - globalEventArgumentBox.GetInt("AwayForeverTime")));
			if (currDate >= globalEventArgumentBox.GetInt("AwayForeverTime"))
			{
				AdaptableLog.Info("远走高飞开始检测" + currDate);
				if (globalEventArgumentBox.Contains<int>("ForeverLoverId"))
				{
					AdaptableLog.Info("远走高飞有人" + currDate);
					int objectId = globalEventArgumentBox.GetInt("ForeverLoverId");
					AdaptableLog.Info("远走高飞有数据" + globalEventArgumentBox.GetInt("AwayForeverTime"));
					DomainManager.Character.TryGetElement_Objects(objectId, out var element);
					if (element == null || element.IsActiveExternalRelationState(60))
					{
						return;
					}
					sbyte orgTemplateId = element.GetOrganizationInfo().OrgTemplateId;
					if ((orgTemplateId == 1 || orgTemplateId == 8 || (uint)(orgTemplateId - 11) <= 1u) ? true : false)
					{
						AdaptableLog.Info("远走高飞可生成");
						if (!globalEventArgumentBox.Contains<int>("StoryForeverLoverId"))
						{
							flag = true;
						}
					}
				}
			}
		}
		if (!flag)
		{
			return;
		}
		int num = globalEventArgumentBox.GetInt("ForeverLoverId");
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num);
		if (element_Objects == null)
		{
			return;
		}
		short settlementId = element_Objects.GetOrganizationInfo().SettlementId;
		Location location = DomainManager.Organization.GetSettlement(settlementId).GetLocation();
		MapBlockData block = DomainManager.Map.GetBlock(location);
		short blockId = block.BlockId;
		if (block.RootBlockId >= 0)
		{
			blockId = block.RootBlockId;
		}
		List<short> list = new List<short>();
		DomainManager.Map.GetSettlementBlocks(location.AreaId, blockId, list);
		for (int i = 0; i < list.Count; i++)
		{
			MapBlockData block2 = DomainManager.Map.GetBlock(location.AreaId, list[i]);
			if (!DomainManager.Adventure.GetAdventuresInArea(location.AreaId).AdventureSites.ContainsKey(location.BlockId))
			{
				blockId = block2.BlockId;
				break;
			}
		}
		MapBlockData block3 = DomainManager.Map.GetBlock(location.AreaId, blockId);
		if (block3 == null)
		{
			return;
		}
		Location location2 = block3.GetLocation();
		if (!DomainManager.Adventure.GetAdventuresInArea(location2.AreaId).AdventureSites.ContainsKey(location2.BlockId) && element_Objects.GetKidnapperId() < 0)
		{
			if (DomainManager.Adventure.TryCreateAdventureSite(context, location2.AreaId, location2.BlockId, 28, MonthlyActionKey.Invalid))
			{
				block3.SetVisible(visible: true, context);
			}
			Location targetLocation = new Location(location.AreaId, blockId);
			Tester.Assert(element_Objects.GetCreatingType() == 1, $"character-{element_Objects.GetId()}.GetCreatingType() == CreatingType.IntelligentCharacter");
			DomainManager.Character.GroupMove(context, element_Objects, targetLocation);
			DomainManager.Character.HideCharacterOnMap(context, element_Objects, 16);
			AdaptableLog.Info("远走高飞已生成");
			if (globalEventArgumentBox.Contains<int>("AwayForeverTime"))
			{
				globalEventArgumentBox.Remove<int>("AwayForeverTime");
			}
			if (globalEventArgumentBox.Contains<int>("ForeverLoverId"))
			{
				globalEventArgumentBox.Remove<int>("ForeverLoverId");
			}
			DomainManager.TaiwuEvent.SaveArgToGlobalArgBox("StoryForeverLoverId", num);
		}
	}

	[DomainMethod]
	public void SetCharacterToAdvanceBranch(DataContext context, int charId)
	{
		SetCurBranchChosenChar(charId, context);
		if (_curBranchChosenChar >= 0 && !_spentCharList.Contains(_curBranchChosenChar))
		{
			int charCombatGroupIndex = DomainManager.Taiwu.GetCharCombatGroupIndex(charId);
			if (charCombatGroupIndex > -1)
			{
				_spentCharInCombatGroupDict.Add(charCombatGroupIndex, charId);
			}
			_spentCharList.Add(_curBranchChosenChar);
			SetSpentCharList(_spentCharList, context);
			DomainManager.Taiwu.LeaveGroup(context, _curBranchChosenChar, bringWards: false);
			foreach (AdvMapNodeNormal node in _curBranch.AdvancedBranch.Nodes)
			{
				_teamDetectedNodes.Add(node.AdjustedPos.GetHashCode());
			}
			SetTeamDetectedNodes(_teamDetectedNodes, context);
		}
		RefreshIndicatePath(context);
	}

	[DomainMethod]
	public bool CanSetCharacterToAdvanceBranch()
	{
		if (_spentCharList.Contains(_curBranchChosenChar) || _curBranch.AdvancedBranch == null)
		{
			return false;
		}
		List<AdvMapPos> offsets = _arrangedNodes.Select(delegate(int a)
		{
			AdvMapPos src = new AdvMapPos(a);
			return (_curBranch.GetNode(new AdvMapPos(a)) != null) ? _curBranch.GetOffset(src) : _curBranch.AdvancedBranch.GetOffset(src);
		}).ToList();
		AdvMapPos offset = _curBranch.AdvancedBranch.EnterNode.Offset;
		AdvMapPos offset2 = _curBranch.AdvancedBranch.FirstNode.Offset;
		AdvMapPos offset3 = _curBranch.AdvancedBranch.ExitNode.Offset;
		AdvMapPos offset4 = _curBranch.AdvancedBranch.LastNode.Offset;
		AdvMapPos curAdvanceBranchEnterPos = offset + (offset2 - offset) / 2;
		AdvMapPos curAdvanceBranchExitPos = offset4 + (offset3 - offset4) / 2;
		if (offsets.Exists((AdvMapPos advMapPos) => advMapPos.X >= curAdvanceBranchEnterPos.X && advMapPos.X <= curAdvanceBranchExitPos.X))
		{
			return false;
		}
		offsets.Add(curAdvanceBranchEnterPos);
		offsets.Add(curAdvanceBranchExitPos);
		return IsReachable(_curBranch.AdvancedBranch.EnterNode) && IsReachable(_curBranch.AdvancedBranch.ExitNode);
		bool IsReachable(AdvMapNodeNormal node)
		{
			if (offsets.Any(delegate(AdvMapPos pos)
			{
				AdvMapPos advMapPos = node.Offset - pos;
				return Math.Abs(advMapPos.Y) > Math.Abs(advMapPos.X);
			}))
			{
				return false;
			}
			return true;
		}
	}

	[DomainMethod]
	public List<int> GetAdventureSpentCharList()
	{
		return GetSpentCharList();
	}

	public void RestoreSpentCharacters(DataContext context)
	{
		Logger.Info($"Restoring spent teammates with a total count of {_spentCharList.Count}");
		foreach (int spentChar in _spentCharList)
		{
			DomainManager.Taiwu.JoinGroup(context, spentChar);
		}
		_spentCharList.Clear();
		SetSpentCharList(_spentCharList, context);
		foreach (var (index, value) in _spentCharInCombatGroupDict)
		{
			DomainManager.Taiwu.SetElement_CombatGroupCharIds(index, value, context);
		}
		_spentCharInCombatGroupDict.Clear();
	}

	public void AddTemporaryIntelligentCharacter(int charId)
	{
		_temporaryIntelligentCharacters.Add(charId);
	}

	public void KeepTemporaryCharacterAfterAdventure(int charId)
	{
		_temporaryIntelligentCharacters.Remove(charId);
	}

	public void RemoveAllTemporaryIntelligentCharacters(DataContext context)
	{
		Logger.Info($"Removing all temporary intelligent characters with a total count of {_temporaryIntelligentCharacters.Count}");
		foreach (int temporaryIntelligentCharacter in _temporaryIntelligentCharacters)
		{
			if (DomainManager.Character.TryGetElement_Objects(temporaryIntelligentCharacter, out var element))
			{
				DomainManager.Character.RevertAllTemporaryModifications(context, element);
				DomainManager.Character.RemoveTemporaryIntelligentCharacter(context, element);
			}
		}
		_temporaryIntelligentCharacters.Clear();
	}

	public void KillIntelligentCharacterInAdventure(DataContext context, GameData.Domains.Character.Character character)
	{
		int id = character.GetId();
		short curAdventureId = DomainManager.Adventure.GetCurAdventureId();
		DomainManager.Character.RevertAllTemporaryModifications(context, character);
		if (!_temporaryIntelligentCharacters.Contains(id))
		{
			DomainManager.Character.MakeCharacterDead(context, character, (short)((Config.Adventure.Instance[curAdventureId].Type == 4) ? 5 : 0), new CharacterDeathInfo(character.GetValidLocation())
			{
				KillerId = -1,
				AdventureId = curAdventureId
			});
		}
		else
		{
			DomainManager.Character.RemoveTemporaryIntelligentCharacter(context, character);
		}
		_temporaryIntelligentCharacters.Remove(id);
	}

	public void AddTemporaryEnemyId(int charId)
	{
		_temporaryEnemies.Add(charId);
	}

	public void RemoveTemporaryEnemyId(int charId)
	{
		_temporaryEnemies.Remove(charId);
	}

	public void RemoveAllTemporaryEnemies(DataContext context)
	{
		Logger.Info($"Removing all temporary enemies with a total count of {_temporaryEnemies.Count}");
		foreach (int temporaryEnemy in _temporaryEnemies)
		{
			if (DomainManager.Character.TryGetElement_Objects(temporaryEnemy, out var element))
			{
				DomainManager.Character.RevertAllTemporaryModifications(context, element);
				if (element.GetCreatingType() != 1)
				{
					DomainManager.Character.RemoveNonIntelligentCharacter(context, element);
				}
			}
		}
		_temporaryEnemies.Clear();
	}

	public int GetEnemyNestCount(short enemyNestTemplateId)
	{
		return _enemyNestCounts[enemyNestTemplateId];
	}

	private void InitializeEnemyNestCounts()
	{
		_enemyNestCounts = new int[EnemyNest.Instance.Count];
		AreaAdventureData[] adventureAreas = _adventureAreas;
		foreach (AreaAdventureData areaAdventureData in adventureAreas)
		{
			foreach (AdventureSiteData value in areaAdventureData.AdventureSites.Values)
			{
				if (value.IsEnemyNest())
				{
					sbyte enemyNestTemplateId = GetEnemyNestTemplateId(value.TemplateId);
					_enemyNestCounts[enemyNestTemplateId]++;
				}
			}
		}
	}

	private void RegisterEnemyNest(short enemyNestTemplateId)
	{
		_enemyNestCounts[enemyNestTemplateId]++;
	}

	private void UnregisterEnemyNest(short enemyNestTemplateId)
	{
		_enemyNestCounts[enemyNestTemplateId]--;
	}

	public unsafe void DestroyEnemyNest(DataContext context, short areaId, short enemyNestId, sbyte behaviorType)
	{
		InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int id = taiwu.GetId();
		EnemyNestItem enemyNestItem = EnemyNest.Instance[enemyNestId];
		Config.Character instance = Config.Character.Instance;
		List<short> members = enemyNestItem.Members;
		CharacterItem characterItem = instance[members[members.Count - 1]];
		AdventureItem adventureItem = Config.Adventure.Instance[enemyNestItem.AdventureId];
		ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[26];
		int baseDelta = formulaCfg.Calculate(adventureItem.CombatDifficulty);
		DomainManager.Extra.ChangeProfessionSeniority(context, 3, baseDelta);
		DomainManager.Taiwu.AddLegacyPoint(context, 13);
		DomainManager.Extra.ChangeAreaSpiritualDebt(context, areaId, enemyNestItem.SpiritualDebtChange);
		taiwu.ChangeResource(context, 6, enemyNestItem.MoneyReward);
		taiwu.ChangeResource(context, 7, enemyNestItem.AuthorityReward);
		taiwu.ChangeExp(context, enemyNestItem.ExpReward);
		_resultDisplayData.ChangedSpiritualDebt = enemyNestItem.SpiritualDebtChange;
		_resultDisplayData.ChangedResources.Add(6, enemyNestItem.MoneyReward);
		_resultDisplayData.ChangedResources.Add(7, enemyNestItem.AuthorityReward);
		_resultDisplayData.ChangedExp += enemyNestItem.ExpReward;
		foreach (short member in enemyNestItem.Members)
		{
			if (!_escapingRandomEnemies.Contains(member))
			{
				_escapingRandomEnemies.Add(member);
			}
		}
		SetEscapingRandomEnemies(_escapingRandomEnemies, context);
		switch (behaviorType)
		{
		case 0:
			taiwu.RecordFameAction(context, 41, -1, 5, jumpAccordingToTargetFame: false);
			DomainManager.Map.ChangeSettlementSafetyInArea(context, areaId, 1);
			instantNotificationCollection.AddFameIncreased(id);
			break;
		case 1:
			taiwu.RecordFameAction(context, 38, -1, 5, jumpAccordingToTargetFame: false);
			DomainManager.Map.ChangeSettlementCultureInArea(context, areaId, 1);
			instantNotificationCollection.AddFameIncreased(id);
			break;
		case 2:
			DomainManager.Map.ChangeSettlementSafetyInArea(context, areaId, 1);
			DomainManager.Map.ChangeSettlementCultureInArea(context, areaId, 1);
			break;
		case 3:
		{
			taiwu.RecordFameAction(context, 42, -1, 5, jumpAccordingToTargetFame: false);
			taiwu.ChangeHappiness(context, 3);
			int num = characterItem.Resources.Items[6];
			num = context.Random.Next(num * 5, num * 10 + 1);
			if (num > 0)
			{
				taiwu.ChangeResource(context, 6, num);
				instantNotificationCollection.AddResourceIncreased(id, 6, num);
			}
			instantNotificationCollection.AddFameDecreased(id);
			instantNotificationCollection.AddHappinessIncreased(id);
			break;
		}
		case 4:
		{
			taiwu.RecordFameAction(context, 44, -1, 5, jumpAccordingToTargetFame: false);
			instantNotificationCollection.AddFameDecreased(id);
			sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(areaId);
			sbyte sectID = MapState.Instance[stateTemplateIdByAreaId].SectID;
			sbyte random = Gender.GetRandom(context.Random);
			short characterTemplateId = OrganizationDomain.GetCharacterTemplateId(sectID, stateTemplateIdByAreaId, random);
			OrganizationInfo organizationInfo = taiwu.GetOrganizationInfo();
			organizationInfo.Grade = 0;
			IntelligentCharacterCreationInfo info = new IntelligentCharacterCreationInfo(taiwu.GetLocation(), organizationInfo, characterTemplateId);
			info.BaseAttraction = (short)context.Random.Next(characterItem.BaseAttraction / 2, characterItem.BaseAttraction + 1);
			info.Age = (short)context.Random.Next(16, 25);
			GameData.Domains.Character.Character character = DomainManager.Character.CreateIntelligentCharacter(context, ref info);
			int id2 = character.GetId();
			DomainManager.Character.CompleteCreatingCharacter(id2);
			character.AddFeature(context, 198);
			DomainManager.Character.ChangeFavorabilityOptional(context, character, taiwu, -10000, 0);
			DomainManager.Taiwu.JoinGroup(context, id2);
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenGetItem_Character, new List<int> { id2 }, arg2: false);
			DomainManager.Adventure.AddCharacterToResultDisplay(character.GetId());
			break;
		}
		}
	}

	[Obsolete]
	private void IncreaseSafety(DataContext context, MapAreaData areaData)
	{
		SettlementInfo[] settlementInfos = areaData.SettlementInfos;
		for (int i = 0; i < settlementInfos.Length; i++)
		{
			SettlementInfo settlementInfo = settlementInfos[i];
			if (settlementInfo.SettlementId >= 0)
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
				short safety = (short)Math.Min(settlement.GetSafety() + 1, settlement.GetMaxSafety());
				settlement.SetSafety(safety, context);
			}
		}
	}

	[Obsolete]
	private void IncreaseCulture(DataContext context, MapAreaData areaData)
	{
		SettlementInfo[] settlementInfos = areaData.SettlementInfos;
		for (int i = 0; i < settlementInfos.Length; i++)
		{
			SettlementInfo settlementInfo = settlementInfos[i];
			if (settlementInfo.SettlementId >= 0)
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
				short culture = (short)Math.Min(settlement.GetCulture() + 1, settlement.GetMaxCulture());
				settlement.SetCulture(culture, context);
			}
		}
	}

	public void ConquerEnemyNest(DataContext context, short areaId, short blockId)
	{
		AdventureSiteData adventureSiteData = _adventureAreas[areaId].AdventureSites[blockId];
		sbyte enemyNestTemplateId = GetEnemyNestTemplateId(adventureSiteData.TemplateId);
		EnemyNestItem enemyNestItem = EnemyNest.Instance[enemyNestTemplateId];
		adventureSiteData.SiteState = 2;
		adventureSiteData.RemainingMonths = enemyNestItem.ConqueredDuration;
		SetElement_AdventureAreas(areaId, _adventureAreas[areaId], context);
		AdventureItem adventureItem = Config.Adventure.Instance[enemyNestItem.AdventureId];
		ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[26];
		int baseDelta = formulaCfg.Calculate(adventureItem.CombatDifficulty);
		DomainManager.Extra.ChangeProfessionSeniority(context, 3, baseDelta);
		MonthlyActionKey key = new MonthlyActionKey(1, 0);
		EnemyNestMonthlyAction enemyNestMonthlyAction = (EnemyNestMonthlyAction)DomainManager.TaiwuEvent.GetMonthlyAction(key);
		enemyNestMonthlyAction.GetConfigAction(areaId, blockId)?.ClearCalledCharacters();
		DomainManager.Taiwu.AddLegacyPoint(context, 13);
		if (enemyNestItem.SpiritualDebtChange < 0)
		{
			DomainManager.Extra.ChangeAreaSpiritualDebt(context, areaId, -enemyNestItem.SpiritualDebtChange);
			_resultDisplayData.ChangedSpiritualDebt = -enemyNestItem.SpiritualDebtChange;
		}
		UnregisterEnemyNest(enemyNestTemplateId);
	}

	private unsafe void OfflineAddConqueredEnemyNestIncome(DataContext context, AdventureSiteData siteData, EnemyNestSiteExtraData enemyNestData)
	{
		IRandomSource random = context.Random;
		switch (siteData.TemplateId)
		{
		case 29:
			if (!random.CheckPercentProb(50))
			{
				siteData.SiteState = 3;
				sbyte* pWeightedElements2 = stackalloc sbyte[9] { 40, 40, 40, 30, 20, 10, 0, 0, 0 };
				sbyte item2 = (sbyte)CollectionUtils.GetRandomWeightedElement(context.Random, pWeightedElements2, 9, 100);
				enemyNestData.Tribute = (Type: item2, TemplateId: -1, Amount: -1);
			}
			break;
		case 30:
			siteData.SiteState = 3;
			enemyNestData.Tribute = (Type: 6, TemplateId: -1, Amount: random.Next(500, 1001));
			break;
		case 31:
			if (!random.CheckPercentProb(50))
			{
				siteData.SiteState = 3;
				sbyte b = (sbyte)random.Next(0, 2);
				sbyte* pWeightedElements = stackalloc sbyte[9] { 0, 40, 30, 20, 10, 0, 0, 0, 0 };
				int grade = CollectionUtils.GetRandomWeightedElement(random, pWeightedElements, 9, 100);
				if (1 == 0)
				{
				}
				short num = b switch
				{
					0 => Config.Weapon.Instance.Where((WeaponItem weaponItem) => weaponItem.Grade == grade && weaponItem.DropRate > 0).ToList().GetRandom(random)
						.TemplateId, 
					1 => Config.Armor.Instance.Where((ArmorItem armorItem) => armorItem.Grade == grade && armorItem.DropRate > 0).ToList().GetRandom(random)
						.TemplateId, 
					_ => throw new ArgumentOutOfRangeException(), 
				};
				if (1 == 0)
				{
				}
				short item = num;
				enemyNestData.Tribute = (Type: b, TemplateId: item, Amount: 1);
			}
			break;
		case 34:
			siteData.SiteState = 3;
			enemyNestData.Tribute = (Type: 7, TemplateId: -1, Amount: random.Next(250, 501));
			break;
		case 36:
			break;
		case 41:
			break;
		case 32:
			siteData.SiteState = 3;
			enemyNestData.Tribute = (Type: 8, TemplateId: -1, Amount: random.Next(500, 1001));
			break;
		case 37:
			siteData.SiteState = 3;
			enemyNestData.Tribute = (Type: (sbyte)random.Next(6), TemplateId: -1, Amount: random.Next(500, 1001));
			break;
		case 33:
		case 35:
		case 38:
		case 39:
		case 40:
			break;
		}
	}

	public void CollectTribute(DataContext context, int charId, short areaId, short blockId)
	{
		AreaAdventureData areaAdventureData = _adventureAreas[areaId];
		AdventureSiteData adventureSiteData = areaAdventureData.AdventureSites[blockId];
		if (adventureSiteData.SiteState != 3)
		{
			return;
		}
		Location location = new Location(areaId, blockId);
		EnemyNestSiteExtraData enemyNestSiteExtraData = _enemyNestSites[location];
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		if (enemyNestSiteExtraData.Tribute.Type < 0)
		{
			OfflineAddConqueredEnemyNestIncome(context, adventureSiteData, enemyNestSiteExtraData);
		}
		InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		(sbyte, short, int) tribute = enemyNestSiteExtraData.Tribute;
		switch (adventureSiteData.TemplateId)
		{
		case 29:
		{
			sbyte stateIdByAreaId = DomainManager.Map.GetStateIdByAreaId(areaId);
			if (!DomainManager.Information.GainRandomSettlementInformationByStateIdToCharacter(context, tribute.Item1, taiwu.GetId(), stateIdByAreaId))
			{
				adventureSiteData.SiteState = 2;
				enemyNestSiteExtraData.Tribute = (Type: -1, TemplateId: -1, Amount: -1);
				SetElement_EnemyNestSites(location, enemyNestSiteExtraData, context);
				SetElement_AdventureAreas(areaId, areaAdventureData, context);
				return;
			}
			break;
		}
		case 30:
			taiwu.ChangeResource(context, tribute.Item1, tribute.Item3);
			instantNotificationCollection.AddResourceIncreased(taiwu.GetId(), tribute.Item1, tribute.Item3);
			break;
		case 31:
			if (charId == taiwu.GetId())
			{
				ItemKey itemKey = DomainManager.Item.CreateItem(context, tribute.Item1, tribute.Item2);
				taiwu.AddInventoryItem(context, itemKey, tribute.Item3);
				List<ItemDisplayData> itemDisplayDataListOptional = DomainManager.Item.GetItemDisplayDataListOptional(new List<ItemKey> { itemKey }, DomainManager.Taiwu.GetTaiwuCharId(), -1);
				itemDisplayDataListOptional[0].Amount = tribute.Item3;
				GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenGetItem_Item, itemDisplayDataListOptional, arg2: false, arg3: true);
			}
			else
			{
				DomainManager.Taiwu.CreateWarehouseItem(context, tribute.Item1, tribute.Item2, tribute.Item3);
			}
			instantNotificationCollection.AddGetItem(taiwu.GetId(), tribute.Item1, tribute.Item2);
			break;
		case 34:
			taiwu.ChangeResource(context, tribute.Item1, tribute.Item3);
			instantNotificationCollection.AddResourceIncreased(taiwu.GetId(), tribute.Item1, tribute.Item3);
			break;
		case 36:
			adventureSiteData.SiteState = 2;
			return;
		case 41:
		{
			List<GameData.Domains.Character.Character> list = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
			list.Clear();
			MapCharacterFilter.Find(CharacterMatchers.MatchCompletelyInfected, list, areaId, includeInfected: true);
			if (list.Count > 0)
			{
				CollectionUtils.Shuffle(context.Random, list);
				foreach (GameData.Domains.Character.Character item in list)
				{
					if (item.GetLocation().Equals(location))
					{
						continue;
					}
					DomainManager.Character.GroupMove(context, item, location);
					CharacterDomain.AddLockMovementCharSet(item.GetId());
					break;
				}
			}
			ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list);
			adventureSiteData.SiteState = 2;
			return;
		}
		case 32:
			taiwu.ChangeExp(context, tribute.Item3);
			instantNotificationCollection.AddExpIncreased(taiwu.GetId(), tribute.Item3);
			break;
		case 37:
			taiwu.ChangeResource(context, tribute.Item1, tribute.Item3);
			instantNotificationCollection.AddResourceIncreased(taiwu.GetId(), tribute.Item1, tribute.Item3);
			break;
		}
		adventureSiteData.SiteState = 2;
		enemyNestSiteExtraData.Tribute = (Type: -1, TemplateId: -1, Amount: -1);
		SetElement_EnemyNestSites(location, enemyNestSiteExtraData, context);
		if (charId != taiwu.GetId())
		{
			Location location2 = new Location(areaId, blockId);
			short templateId = adventureSiteData.TemplateId;
			monthlyNotificationCollection.AddIncomeFromNest(charId, location2, taiwu.GetId(), templateId);
		}
		SetElement_AdventureAreas(areaId, areaAdventureData, context);
	}

	private void FixOutdatedEnemyNests(DataContext context)
	{
		MonthlyActionKey key = MonthlyEventActionsManager.PredefinedKeys["EnemyNestDefault"];
		EnemyNestMonthlyAction enemyNestMonthlyAction = (EnemyNestMonthlyAction)DomainManager.TaiwuEvent.GetMonthlyAction(key);
		List<Location> list = new List<Location>();
		for (short num = 0; num < _adventureAreas.Length; num++)
		{
			AreaAdventureData areaAdventureData = _adventureAreas[num];
			foreach (var (num3, adventureSiteData2) in areaAdventureData.AdventureSites)
			{
				if (adventureSiteData2.SiteState < 2 && adventureSiteData2.MonthlyActionKey.ActionType == 1)
				{
					ConfigMonthlyAction configAction = enemyNestMonthlyAction.GetConfigAction(num, num3);
					if (configAction != null && configAction.MajorCharacterSets.Count > 0)
					{
						configAction.ClearCalledCharacters();
						Logger.Warn($"Clearing characters called by outdated enemy nest {Config.Adventure.Instance[adventureSiteData2.TemplateId].Name} at ({num},{num3})");
					}
					list.Add(new Location(num, num3));
				}
			}
		}
		foreach (Location item in list)
		{
			RemoveAdventureSite(context, item.AreaId, item.BlockId, isTimeout: true, isComplete: false);
		}
		enemyNestMonthlyAction.ResetIntervals();
	}

	private void FixConqueredEnemyNests(DataContext context)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		for (short num2 = 0; num2 < _adventureAreas.Length; num2++)
		{
			AreaAdventureData areaAdventureData = _adventureAreas[num2];
			bool flag = false;
			foreach (var (num4, adventureSiteData2) in areaAdventureData.AdventureSites)
			{
				if (adventureSiteData2.SiteState < 2)
				{
					continue;
				}
				if (adventureSiteData2.MonthlyActionKey.IsValid())
				{
					MonthlyActionBase monthlyActionBase = DomainManager.TaiwuEvent.GetMonthlyAction(adventureSiteData2.MonthlyActionKey);
					if (monthlyActionBase is IMonthlyActionGroup monthlyActionGroup)
					{
						monthlyActionBase = monthlyActionGroup.GetConfigAction(num2, num4);
					}
					if (monthlyActionBase is ConfigMonthlyAction configMonthlyAction && configMonthlyAction.MajorCharacterSets.Count > 0)
					{
						configMonthlyAction.ClearCalledCharacters();
						Logger.Warn($"Clearing characters called by conquered adventure site {Config.Adventure.Instance[adventureSiteData2.TemplateId].Name} at ({num2},{num4})");
					}
				}
				if (adventureSiteData2.RemainingMonths < 0)
				{
					sbyte enemyNestTemplateId = GetEnemyNestTemplateId(adventureSiteData2.TemplateId);
					EnemyNestItem enemyNestItem = EnemyNest.Instance[enemyNestTemplateId];
					if (enemyNestItem.ConqueredDuration >= 0)
					{
						adventureSiteData2.RemainingMonths = enemyNestItem.ConqueredDuration;
						flag = true;
						stringBuilder.AppendFormat("Conquered Nest {0}'s Duration: -1 => {1}", Config.Adventure.Instance[adventureSiteData2.TemplateId].Name, adventureSiteData2.RemainingMonths.ToString());
						stringBuilder.AppendLine();
						num++;
					}
				}
			}
			if (flag)
			{
				SetElement_AdventureAreas(num2, areaAdventureData, context);
			}
		}
		if (num > 0)
		{
			Logger.Warn($"Fixing conquered enemy nests duration ({num}):\n{stringBuilder}");
		}
	}

	[DomainMethod]
	public Location GmCmd_GenerateAdventure(DataContext context, short adventureId)
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		short num = -1;
		foreach (MonthlyActionsItem item in (IEnumerable<MonthlyActionsItem>)MonthlyActions.Instance)
		{
			if (item.AdventureId != adventureId)
			{
				continue;
			}
			num = item.TemplateId;
			break;
		}
		if (num < 0)
		{
			if (adventureId == 144)
			{
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				int num2 = -1;
				HashSet<int> relatedCharIds = DomainManager.Character.GetRelatedCharIds(taiwu.GetId(), 0);
				foreach (int item2 in relatedCharIds)
				{
					if (!DomainManager.Character.TryGetElement_Objects(item2, out var element) || element.IsCompletelyInfected() || element.GetAgeGroup() != 2 || DomainManager.Character.GetAliveSpouse(item2) >= 0 || element.IsActiveExternalRelationState(60) || !element.GetLocation().IsValid() || RelationType.ContainBloodExclusionRelations(DomainManager.Character.GetRelation(taiwu.GetId(), item2).RelationType))
					{
						continue;
					}
					num2 = item2;
					break;
				}
				if (num2 < 0)
				{
					return Location.Invalid;
				}
				Location location2 = taiwu.GetLocation();
				MonthlyActionKey monthlyActionKey = EventHelper.TriggerTaiwuWeddingAdventure(num2, location2);
				return location2;
			}
			if (_adventureAreas[location.AreaId].AdventureSites.ContainsKey(location.BlockId))
			{
				return location;
			}
			if (TryCreateAdventureSite(context, location.AreaId, location.BlockId, adventureId, MonthlyActionKey.Invalid))
			{
				return location;
			}
			return Location.Invalid;
		}
		MonthlyActionsItem monthlyActionsItem = MonthlyActions.Instance[num];
		if (monthlyActionsItem.IsEnemyNest)
		{
			if (location.AreaId >= 45)
			{
				return Location.Invalid;
			}
			RemoveAdventureSite(context, location.AreaId, location.BlockId, isTimeout: false, isComplete: false);
			EnemyNestMonthlyAction enemyNestMonthlyAction = (EnemyNestMonthlyAction)DomainManager.TaiwuEvent.GetMonthlyAction(new MonthlyActionKey(1, 0));
			ConfigMonthlyAction configMonthlyAction = enemyNestMonthlyAction.CreateNewConfigAction(num, location.AreaId);
			configMonthlyAction.Location = location;
			configMonthlyAction.TriggerAction();
			if (configMonthlyAction.Location.IsValid())
			{
				configMonthlyAction.MonthlyHandler();
				DomainManager.TaiwuEvent.GmCmd_SaveMonthlyActionManager(context);
			}
			return configMonthlyAction.Location;
		}
		if (monthlyActionsItem.MinInterval > 0)
		{
			ConfigMonthlyAction configMonthlyAction2 = (ConfigMonthlyAction)DomainManager.TaiwuEvent.GetMonthlyAction(new MonthlyActionKey(0, num));
			if (configMonthlyAction2.State == 0)
			{
				configMonthlyAction2.SelectLocation();
				configMonthlyAction2.TriggerAction();
				if (configMonthlyAction2.Location.IsValid())
				{
					AdventureSiteData adventureSiteData = _adventureAreas[configMonthlyAction2.Location.AreaId].AdventureSites[configMonthlyAction2.Location.BlockId];
					adventureSiteData.TemplateId = adventureId;
					configMonthlyAction2.MonthlyHandler();
					DomainManager.TaiwuEvent.GmCmd_SaveMonthlyActionManager(context);
				}
			}
			return configMonthlyAction2.Location;
		}
		if (ConfigMonthlyActionDefines.OrgTemplateIdToContestForTaiwuBride.ContainsValue(num))
		{
			if (location.AreaId >= 45)
			{
				return Location.Invalid;
			}
			MonthlyActionKey key = MonthlyEventActionsManager.PredefinedKeys["BrideOpenContestDefault"];
			ConfigWrapperAction configWrapperAction = (ConfigWrapperAction)DomainManager.TaiwuEvent.GetMonthlyAction(key);
			if (configWrapperAction.CurrConfigMonthlyAction == null)
			{
				configWrapperAction.CreateWrappedAction(num, -1);
				ConfigMonthlyAction currConfigMonthlyAction = configWrapperAction.CurrConfigMonthlyAction;
				if (currConfigMonthlyAction != null)
				{
					AdventureSiteData adventureSiteData2 = _adventureAreas[currConfigMonthlyAction.Location.AreaId].AdventureSites[currConfigMonthlyAction.Location.BlockId];
					adventureSiteData2.TemplateId = adventureId;
					currConfigMonthlyAction.MonthlyHandler();
					DomainManager.TaiwuEvent.GmCmd_SaveMonthlyActionManager(context);
					return configWrapperAction.CurrConfigMonthlyAction.Location;
				}
				return Location.Invalid;
			}
			if (configWrapperAction.CurrConfigMonthlyAction.ConfigTemplateId == num)
			{
				return configWrapperAction.CurrConfigMonthlyAction.Location;
			}
			return Location.Invalid;
		}
		return Location.Invalid;
	}

	[DomainMethod]
	public int GmCmd_GetAdventureParameter(string key)
	{
		return EventHelper.GetAdventureParameter(key);
	}

	[DomainMethod]
	public void GmCmd_SetAdventureParameter(string key, int value)
	{
		EventHelper.SetAdventureParameter(key, value);
	}

	[DomainMethod]
	public bool TryInvokeConfirmEnterEvent()
	{
		DomainManager.TaiwuEvent.OnEvent_ConfirmEnterSwordTomb();
		return DomainManager.TaiwuEvent.IsShowingEvent || DomainManager.TaiwuEvent.GetHasListeningEvent();
	}

	private void InitializeMap(DataContext context)
	{
		_triggeredPosSet.Clear();
		_finishedPosSet.Clear();
		_vertices.Clear();
		_branches.Clear();
		_spentCharList.Clear();
		_teamDetectedNodes.Clear();
		_curBranchChosenChar = -1;
		_mapTrunks.Clear();
		_onEventFinishCallback = null;
		_eventBlock = false;
		_curMapTrunk = new AdventureMapTrunk();
		_indicatePath.Clear();
		SetCurMapTrunk(_curMapTrunk, context);
		ClearPathArrangement(context);
		SetPlayerPos(int.MinValue, context);
		SetAllowExitAdventure(AdventureCfg.Interruptible != 0, context);
		foreach (AdventureStartNode startNode in AdventureCfg.StartNodes)
		{
			_vertices.Add(new AdvMapNodeVertex(ENodeType.Start, _vertices.Count, (startNode.TerrainId >= 0) ? startNode.TerrainId : _enterTerrain, startNode.NodeKey));
		}
		foreach (AdventureTransferNode transferNode in AdventureCfg.TransferNodes)
		{
			_vertices.Add(new AdvMapNodeVertex(ENodeType.Transfer, _vertices.Count, transferNode.TerrainId, transferNode.NodeKey));
		}
		foreach (AdventureEndNode endNode in AdventureCfg.EndNodes)
		{
			_vertices.Add(new AdvMapNodeVertex(ENodeType.End, _vertices.Count, (endNode.TerrainId >= 0) ? endNode.TerrainId : _enterTerrain, endNode.NodeKey));
		}
		foreach (AdventureBaseBranch baseBranch in AdventureCfg.BaseBranches)
		{
			AdvMapBranch advMapBranch = new AdvMapBranch(_branches.Count, _vertices[baseBranch.PortA], _vertices[baseBranch.PortB], baseBranch, context);
			_branches.Add(advMapBranch);
			_vertices[baseBranch.PortA].ConnectedBranchDict.Add(_vertices[baseBranch.PortB], advMapBranch);
			_vertices[baseBranch.PortB].ConnectedBranchDict.Add(_vertices[baseBranch.PortA], advMapBranch);
		}
	}

	public AdventureMapTrunk GenerateNextAdvMapTrunk(DataContext context)
	{
		AdvMapNodeVertex startingVertex = _curBranch.ExitNode;
		Logger.Info($"Generating branch with \"{startingVertex.NodeKey}\" as entrance node key, \"{_chosenBranchKey}\" as branch key.");
		AdvMapNodeVertex exitNode = _curBranch.ExitNode.ConnectedBranchDict.Values.First((AdvMapBranch a) => a.EnterNode == startingVertex && a.BranchKey == _chosenBranchKey).ExitNode;
		SetCurBranchChosenChar(-1, context);
		return GenerateAdvMapTrunk(context, startingVertex, exitNode);
	}

	public AdventureMapTrunk GenerateAdvMapTrunk(DataContext context, AdvMapNodeVertex startingVertex, AdvMapNodeVertex endingVertex)
	{
		_curBranch = startingVertex.ConnectedBranchDict[endingVertex];
		_curBranch.GenBranch(context, _extraEvents);
		_extraEvents.Clear();
		AdventureMapTrunk adventureMapTrunk = new AdventureMapTrunk();
		adventureMapTrunk.BranchIndex = _curBranch.BranchIndex;
		if (startingVertex.NodeType == ENodeType.Start)
		{
			adventureMapTrunk.Points.Add(startingVertex.ToAdventureMapPoint());
		}
		adventureMapTrunk.Points.Add(endingVertex.ToAdventureMapPoint());
		foreach (AdvMapNodeNormal node in _curBranch.Nodes)
		{
			adventureMapTrunk.Points.Add(node.ToAdventureMapPoint());
		}
		if (_curBranch.AdvancedBranch != null)
		{
			foreach (AdvMapNodeNormal node2 in _curBranch.AdvancedBranch.Nodes)
			{
				adventureMapTrunk.Points.Add(node2.ToAdventureMapPoint());
			}
		}
		_curBranch.FillConnect(adventureMapTrunk.Connects, includeEnterConnect: false);
		FillFollowNodes(context, adventureMapTrunk, endingVertex);
		_mapTrunks.Push(adventureMapTrunk);
		return adventureMapTrunk;
	}

	private void FillFollowNodes(DataContext context, AdventureMapTrunk ret, AdvMapNodeVertex nextNode)
	{
		if (nextNode.NodeType == ENodeType.Transfer)
		{
			List<AdvMapNodeVertex> list = (from a in nextNode.ConnectedBranchDict.Values
				where a.EnterNode == nextNode
				select a.ExitNode).ToList();
			Span<ELinkDir> pArray = stackalloc ELinkDir[3]
			{
				ELinkDir.Right,
				ELinkDir.DownRight,
				ELinkDir.UpRight
			};
			CollectionUtils.Shuffle(context.Random, pArray, pArray.Length);
			for (int num = 0; num < list.Count; num++)
			{
				AdvMapBranch advMapBranch = nextNode.ConnectedBranchDict[list[num]];
				advMapBranch.EnterDir = pArray[num];
				ret.Points.Add(advMapBranch.FirstNode.ToAdventureMapPoint());
				AdvMapPos adjustedPos = nextNode.AdjustedPos;
				AdvMapPos advMapPos = adjustedPos + advMapBranch.EnterDir.Rotate(new AdvMapPos(4, 0));
				ret.Connects.Add(new AdventureMapConnect(adjustedPos.GetHashCode(), advMapPos.GetHashCode()));
			}
		}
	}

	private (int, int) HandleNodeContent_Event(DataContext context, AdventureBranch branchCfg, AdventureMapPoint pointData)
	{
		TriggerGlobalEvent(branchCfg, pointData, delegate
		{
			DomainManager.TaiwuEvent.OnEvent_AdventureEnterNode(pointData);
		});
		return (0, 0);
	}

	private (int, int) HandleNodeContent_Resource(DataContext context, AdventureBranch branchCfg, AdventureMapPoint pointData)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		pointData.JudgeSuccess = _perceivedNodes.Contains(CurNode.AdjustedPos.GetHashCode()) || taiwu.GetLifeSkillAttainment((sbyte)CurNode.LifeSkillType) >= CurNode.LifeSkillRequiredVal;
		byte b = 0;
		short num = 0;
		if (pointData.JudgeSuccess)
		{
			if (!branchCfg.PersonalityContentWeights.CheckIndex(pointData.SevenElementType))
			{
				Logger.Warn($"Invalid seven element type {pointData.SevenElementType} for branch with weights count {branchCfg.PersonalityContentWeights.Length}");
				TriggerGlobalEvent(branchCfg, pointData, delegate
				{
					OnEventHandleFinished(context);
				});
				pointData.JudgeSuccess = false;
				return (-1, -1);
			}
			if (!branchCfg.PersonalityContentWeights[pointData.SevenElementType].NormalResWeights.CheckIndex(CurNode.NodeContent.Item2))
			{
				Logger.Warn($"Invalid node content ({CurNode.NodeContent.Item1},{CurNode.NodeContent.Item2} for normal resource weights count{branchCfg.PersonalityContentWeights[pointData.SevenElementType].NormalResWeights.Length})");
				TriggerGlobalEvent(branchCfg, pointData, delegate
				{
					OnEventHandleFinished(context);
				});
				pointData.JudgeSuccess = false;
				return (-1, -1);
			}
			int num2 = _indicatePath.IndexOf(CurNode.AdjustedPos.GetHashCode());
			if (num2 > -1 && _contentList.CheckIndex(num2))
			{
				b = (byte)_contentList[num2].Item1;
				num = (short)_contentList[num2].Item2;
			}
			if (b == Config.ResourceType.Instance.Count)
			{
				taiwu.ChangeExp(context, num);
				_resultDisplayData.ChangedExp += num;
			}
			else
			{
				taiwu.ChangeResource(context, (sbyte)b, num);
				_resultDisplayData.ChangedResources.Add((sbyte)b, num);
			}
		}
		TriggerGlobalEvent(branchCfg, pointData, delegate
		{
			OnEventHandleFinished(context);
		});
		return pointData.JudgeSuccess ? (b, num) : (-1, -1);
	}

	[DomainMethod]
	public ItemDisplayData GetAdventureGainsItemList(ItemKey itemKey)
	{
		return _resultDisplayData.ItemList.Find((ItemDisplayData d) => d.Key.TemplateEquals(itemKey));
	}

	private (int, int) HandleNodeContent_Item(DataContext context, AdventureBranch branchCfg, AdventureMapPoint pointData)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		pointData.JudgeSuccess = _perceivedNodes.Contains(CurNode.AdjustedPos.GetHashCode()) || taiwu.GetLifeSkillAttainment((sbyte)CurNode.LifeSkillType) >= CurNode.LifeSkillRequiredVal;
		byte b = 0;
		short num = 0;
		if (pointData.JudgeSuccess)
		{
			if (!branchCfg.PersonalityContentWeights.CheckIndex(pointData.SevenElementType))
			{
				Logger.Warn($"Invalid seven element type {pointData.SevenElementType} for branch with weights count {branchCfg.PersonalityContentWeights.Length}");
				pointData.JudgeSuccess = false;
				TriggerGlobalEvent(branchCfg, pointData, delegate
				{
					OnEventHandleFinished(context);
				});
				return (-1, -1);
			}
			if (!branchCfg.PersonalityContentWeights[pointData.SevenElementType].SpecialResWeights.CheckIndex(CurNode.NodeContent.Item2))
			{
				Logger.Warn($"Invalid node content ({CurNode.NodeContent.Item1},{CurNode.NodeContent.Item2} for special resource weights {branchCfg.PersonalityContentWeights[pointData.SevenElementType].SpecialResWeights.Length})");
				pointData.JudgeSuccess = false;
				TriggerGlobalEvent(branchCfg, pointData, delegate
				{
					OnEventHandleFinished(context);
				});
				return (-1, -1);
			}
			short num2;
			(b, num, num2, _) = branchCfg.PersonalityContentWeights[pointData.SevenElementType].SpecialResWeights[CurNode.NodeContent.Item2];
			if (num < 0)
			{
				int num3 = _indicatePath.IndexOf(CurNode.AdjustedPos.GetHashCode());
				if (num3 > -1 && _contentList.CheckIndex(num3))
				{
					num = (short)_contentList[num3].Item2;
				}
			}
			ItemKey itemKey = new ItemKey((sbyte)b, 0, num, -1);
			ItemDisplayData itemDisplayData = _resultDisplayData.ItemList.Find((ItemDisplayData d) => d.Key.TemplateEquals(itemKey) && ItemTemplateHelper.IsStackable(itemKey.ItemType, itemKey.TemplateId));
			if (itemDisplayData == null)
			{
				ItemKey itemKey2 = DomainManager.Item.CreateItem(context, (sbyte)b, num);
				ItemDisplayData itemDisplayData2 = DomainManager.Item.GetItemDisplayData(itemKey2);
				itemDisplayData2.Amount = num2;
				_resultDisplayData.ItemList.Add(itemDisplayData2);
			}
			else
			{
				itemDisplayData.Amount += num2;
			}
		}
		TriggerGlobalEvent(branchCfg, pointData, delegate
		{
			OnEventHandleFinished(context);
		});
		return pointData.JudgeSuccess ? (b, num) : (-1, -1);
	}

	private (int, int) HandleNodeContent_Bonus(DataContext context, AdventureBranch branchCfg, AdventureMapPoint pointData)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		pointData.JudgeSuccess = (pointData.JudgeSuccess = _perceivedNodes.Contains(CurNode.AdjustedPos.GetHashCode()) || taiwu.GetLifeSkillAttainment((sbyte)CurNode.LifeSkillType) >= CurNode.LifeSkillRequiredVal);
		if (pointData.JudgeSuccess)
		{
			TriggerGlobalEvent(branchCfg, pointData, delegate
			{
				DomainManager.TaiwuEvent.OnEvent_AdventureEnterNode(pointData);
			});
		}
		else
		{
			TriggerGlobalEvent(branchCfg, pointData, delegate
			{
				OnEventHandleFinished(context);
			});
		}
		return pointData.JudgeSuccess ? (0, 0) : (-1, -1);
	}

	private (int, int) HandleNodeContent_ExtraEvent(DataContext context, AdventureBranch branchCfg, AdventureMapPoint pointData)
	{
		TriggerGlobalEvent(branchCfg, pointData, delegate
		{
			DomainManager.TaiwuEvent.TriggerAdventureExtraEvent(_curBranch.GetExtraEvent(pointData.NodeContentIndex), pointData);
		});
		return (0, 0);
	}

	private void TriggerGlobalEvent(AdventureBranch branchCfg, AdventureMapPoint pointData, Action onFinishCallback)
	{
		if (!string.IsNullOrEmpty(branchCfg.GlobalEvent))
		{
			_onEventFinishCallback = onFinishCallback;
			DomainManager.TaiwuEvent.TriggerAdventureGlobalEvent(pointData);
		}
		else
		{
			onFinishCallback();
		}
	}

	[DomainMethod]
	public List<(int, int)> GetPathContentList()
	{
		return _contentList;
	}

	private void InitPathContent(DataContext context)
	{
		_contentList.Clear();
		foreach (int pos in _indicatePath)
		{
			AdvMapNode advMapNode = _path.Find((AdvMapNode n) => n.AdjustedPos.GetHashCode() == pos);
			if (advMapNode.NodeType != ENodeType.Normal)
			{
				_contentList.Add((-1, -1));
				continue;
			}
			AdvBaseMapBranch advBaseMapBranch = ((_curBranch.AdvancedBranch != null && _curBranch.AdvancedBranch.Nodes.Contains((AdvMapNodeNormal)advMapNode)) ? ((AdvBaseMapBranch)_curBranch.AdvancedBranch) : ((AdvBaseMapBranch)_curBranch));
			int branchIndex = advBaseMapBranch.BranchIndex;
			AdventureBranch adventureBranch = ((branchIndex < AdventureCfg.BaseBranches.Count) ? ((AdventureBranch)AdventureCfg.BaseBranches[branchIndex]) : ((AdventureBranch)AdventureCfg.AdvancedBranches[branchIndex - AdventureCfg.BaseBranches.Count]));
			if (!adventureBranch.PersonalityContentWeights.CheckIndex(advMapNode.SevenElementType))
			{
				_contentList.Add((-1, -1));
				continue;
			}
			switch (advMapNode.NodeContent.Item1)
			{
			case 0:
				_contentList.Add((0, 0));
				break;
			case 1:
				if (adventureBranch.PersonalityContentWeights[advMapNode.SevenElementType].NormalResWeights.CheckIndex(advMapNode.NodeContent.Item2))
				{
					(byte resId, short amount, short weight) tuple2 = adventureBranch.PersonalityContentWeights[advMapNode.SevenElementType].NormalResWeights[advMapNode.NodeContent.Item2];
					byte item2 = tuple2.resId;
					short item3 = tuple2.amount;
					int num2 = Math.Max(1, item3 * 90 / 100);
					int num3 = item3 * 110 / 100;
					item3 = (short)(context.Random.Next(num2, num3) * DomainManager.World.GetGainResourcePercent(4) / 100);
					_contentList.Add((item2, item3));
				}
				else
				{
					_contentList.Add((-1, -1));
				}
				break;
			case 2:
				if (adventureBranch.PersonalityContentWeights[advMapNode.SevenElementType].SpecialResWeights.CheckIndex(advMapNode.NodeContent.Item2))
				{
					var (item, num, _, _) = adventureBranch.PersonalityContentWeights[advMapNode.SevenElementType].SpecialResWeights[advMapNode.NodeContent.Item2];
					if (num < 0)
					{
						sbyte grade = (sbyte)RandomUtils.GetRandomIndex(AdventureItemDropRate.Instance[CurAdventureLifeSkillDifficulty - 1].ItemGradeDropRate, context.Random);
						num = ItemDomain.GetRandomItemIdInSubType(context.Random, (short)(-num - 1), grade);
					}
					_contentList.Add((item, num));
				}
				else
				{
					_contentList.Add((-1, -1));
				}
				break;
			case 3:
				_contentList.Add((0, 0));
				break;
			case 10:
				_contentList.Add((0, 0));
				break;
			default:
				_contentList.Add((-1, -1));
				break;
			}
		}
	}

	[Obsolete("EnemyNest can no longer be upgraded")]
	public void UpgradeEnemyNest(DataContext context, short areaId, short blockId, short newAdventureId)
	{
		Location location = new Location(areaId, blockId);
		AdventureSiteData adventureSiteData = _adventureAreas[areaId].AdventureSites[blockId];
		EnemyNestSiteExtraData enemyNestSiteExtraData = _enemyNestSites[location];
		enemyNestSiteExtraData.UpgradeChance = 0;
		adventureSiteData.TemplateId = newAdventureId;
		adventureSiteData.SiteState = 0;
		ClearRandomEnemiesBySite(context, location);
		GenerateRandomEnemiesBySite(context, adventureSiteData, areaId, blockId);
		SetElement_AdventureAreas(areaId, _adventureAreas[areaId], context);
	}

	[Obsolete("EnemyNest can no longer be upgraded")]
	public bool TryUpgradeEnemyNest(DataContext context, short areaId, short blockId)
	{
		Location location = new Location(areaId, blockId);
		AdventureSiteData adventureSiteData = _adventureAreas[areaId].AdventureSites[blockId];
		EnemyNestSiteExtraData enemyNestSiteExtraData = _enemyNestSites[location];
		sbyte enemyNestTemplateId = GetEnemyNestTemplateId(adventureSiteData.TemplateId);
		int num = EnemyNestConstValues.HereticStrongholdNestIds.IndexOf(enemyNestTemplateId);
		if (num < 0 || num >= EnemyNestConstValues.HereticStrongholdNestIds.Length - 1)
		{
			return false;
		}
		short num2 = EnemyNestConstValues.HereticStrongholdNestIds[num + 1];
		EnemyNestItem enemyNestItem = EnemyNest.Instance[num2];
		if (enemyNestItem.MonthlyActionId < 0 || enemyNestItem.AdventureId < 0)
		{
			return false;
		}
		if (enemyNestItem.WorldTotalCountLimit > 0 && GetEnemyNestCount(num2) >= enemyNestItem.WorldTotalCountLimit)
		{
			return false;
		}
		MonthlyActionKey key = new MonthlyActionKey(1, 0);
		EnemyNestMonthlyAction enemyNestMonthlyAction = (EnemyNestMonthlyAction)DomainManager.TaiwuEvent.GetMonthlyAction(key);
		ConfigMonthlyAction configAction = enemyNestMonthlyAction.GetConfigAction(areaId, blockId);
		configAction.State = 1;
		configAction.Month = 0;
		configAction.ClearCalledCharacters();
		configAction.ConfigTemplateId = enemyNestItem.MonthlyActionId;
		enemyNestSiteExtraData.UpgradeChance = 0;
		adventureSiteData.TemplateId = enemyNestItem.AdventureId;
		adventureSiteData.SiteState = 0;
		UnregisterEnemyNest(enemyNestTemplateId);
		RegisterEnemyNest(num2);
		ClearRandomEnemiesBySite(context, location);
		GenerateRandomEnemiesBySite(context, adventureSiteData, areaId, blockId);
		SetElement_AdventureAreas(areaId, _adventureAreas[areaId], context);
		return true;
	}

	[Obsolete("EnemyNest can no longer be upgraded")]
	public void SetEnemyNestUpgradeChance(DataContext context, short areaId, short blockId, short chance)
	{
		Location location = new Location(areaId, blockId);
		EnemyNestSiteExtraData enemyNestSiteExtraData = _enemyNestSites[location];
		enemyNestSiteExtraData.UpgradeChance = chance;
		SetElement_EnemyNestSites(location, enemyNestSiteExtraData, context);
	}

	[DomainMethod]
	public bool EnterAdventureByConfigData(DataContext context, AdventureItem configData, int startNodeIndex, int enterTerrain = 1)
	{
		context.SwitchRandomSource("EnterAdventureByConfigData");
		_enterTerrain = enterTerrain;
		EnterTerrainWeights = MapBlock.Instance[_enterTerrain].AdventureTerrainWeights;
		for (sbyte b = 0; b < 7; b++)
		{
			_personalities[b] = 20;
		}
		SetPersonalities(_personalities, context);
		AdventureCfg = configData;
		ClearAdvParameters();
		InitializeMap(context);
		GenerateAdventureMap(context, _vertices[startNodeIndex].NodeKey);
		SetCurAdventureId(configData.TemplateId, context);
		return true;
	}

	[DomainMethod]
	public void SelectBranchForPreview(DataContext context, sbyte nodeBranchIndex)
	{
		AdvMapBranch advMapBranch = _curBranch.ExitNode.ConnectedBranchDict.Values.FirstOrDefault((AdvMapBranch node) => node.BranchIndex == nodeBranchIndex && node.EnterNode == _curBranch.ExitNode);
		if (advMapBranch != null)
		{
			SetCurMapTrunk(GenerateAdvMapTrunk(context, advMapBranch.EnterNode, advMapBranch.ExitNode), context);
			SetAdventureState(1, context);
			SetPlayerPos(_curBranch.EnterNode.AdjustedPos.GetHashCode(), context);
			_arrangedNodes.Clear();
			SetArrangedNodes(_arrangedNodes, context);
			RefreshIndicatePath(context);
		}
	}

	[DomainMethod]
	public void ClearBranchForPreview(DataContext context)
	{
		if (_mapTrunks.Count > 1)
		{
			_curBranch.ExitNode.PrevVertex = null;
			_curBranch.Nodes.Clear();
			_curBranch.Nodes.Add(_curBranch.FirstNode);
			_mapTrunks.Pop();
			_curMapTrunk = null;
			_curBranch = null;
			SetCurMapTrunk(_mapTrunks.Peek(), context);
			_curBranch = _branches[_curMapTrunk.BranchIndex];
			SetPlayerPos(_curBranch.EnterNode.AdjustedPos.GetHashCode(), context);
			ClearPathArrangement(context);
			SetCharacterToAdvanceBranch(context, -1);
		}
	}

	private void Test_GenerateAdventures(DataContext context)
	{
		AdaptableLog.TagInfo("Test_GenerateAdventures", "Start testing adventure generation.");
		foreach (AdventureItem item in (IEnumerable<AdventureItem>)Config.Adventure.Instance)
		{
			if (item.TemplateId == 98)
			{
				continue;
			}
			for (int i = 0; i < 100; i++)
			{
				if (!EnterAdventureByConfigData(context, item, 0))
				{
					throw new Exception("Unable to create adventure " + item.Name);
				}
				for (int j = 0; j < _branches.Count; j++)
				{
					AdvMapBranch advMapBranch = _branches[j];
					foreach (AdvMapNodeNormal node in advMapBranch.Nodes)
					{
						AdventureMapPoint adventureMapPoint = node.ToAdventureMapPoint();
						if (adventureMapPoint.NodeContentType == 0 || adventureMapPoint.NodeContentType == 3)
						{
							int nodeContentIndex = adventureMapPoint.NodeContentIndex;
							int affiliatedBranchIdx = adventureMapPoint.AffiliatedBranchIdx;
							AdventurePersonalityContentWeights[] array = ((affiliatedBranchIdx < item.BaseBranches.Count) ? item.BaseBranches[affiliatedBranchIdx].PersonalityContentWeights : item.AdvancedBranches[affiliatedBranchIdx - item.BaseBranches.Count].PersonalityContentWeights);
							if (!array.CheckIndex(adventureMapPoint.SevenElementType))
							{
								throw new Exception($"Invalid seven element type {adventureMapPoint.SevenElementType} of node {adventureMapPoint.GetDetailedInfo()}");
							}
							AdventurePersonalityContentWeights adventurePersonalityContentWeights = array[adventureMapPoint.SevenElementType];
							sbyte nodeContentType = adventureMapPoint.NodeContentType;
							if (1 == 0)
							{
							}
							(string, short)[] array2 = nodeContentType switch
							{
								0 => adventurePersonalityContentWeights.EventWeights, 
								3 => adventurePersonalityContentWeights.BonusWeights, 
								_ => throw new Exception($"Incorrect node content type for EventTrigger_AdventureEnterNode: {adventureMapPoint.NodeContentType} given."), 
							};
							if (1 == 0)
							{
							}
							(string, short)[] list = array2;
							if (!list.CheckIndex(nodeContentIndex))
							{
								throw new Exception($"Invalid eventIndex {nodeContentIndex} of node {adventureMapPoint.GetDetailedInfo()}");
							}
						}
					}
					if (advMapBranch.AdvancedBranch == null)
					{
						continue;
					}
					foreach (AdvMapNodeNormal node2 in advMapBranch.AdvancedBranch.Nodes)
					{
						AdventureMapPoint adventureMapPoint2 = node2.ToAdventureMapPoint();
						if (adventureMapPoint2.NodeContentType == 0 || adventureMapPoint2.NodeContentType == 3)
						{
							int nodeContentIndex2 = adventureMapPoint2.NodeContentIndex;
							int affiliatedBranchIdx2 = adventureMapPoint2.AffiliatedBranchIdx;
							AdventurePersonalityContentWeights[] array3 = ((affiliatedBranchIdx2 < item.BaseBranches.Count) ? item.BaseBranches[affiliatedBranchIdx2].PersonalityContentWeights : item.AdvancedBranches[affiliatedBranchIdx2 - item.BaseBranches.Count].PersonalityContentWeights);
							if (!array3.CheckIndex(adventureMapPoint2.SevenElementType))
							{
								throw new Exception($"Invalid seven element type {adventureMapPoint2.SevenElementType} of node {adventureMapPoint2.GetDetailedInfo()}");
							}
							AdventurePersonalityContentWeights adventurePersonalityContentWeights2 = array3[adventureMapPoint2.SevenElementType];
							sbyte nodeContentType2 = adventureMapPoint2.NodeContentType;
							if (1 == 0)
							{
							}
							(string, short)[] array2 = nodeContentType2 switch
							{
								0 => adventurePersonalityContentWeights2.EventWeights, 
								3 => adventurePersonalityContentWeights2.BonusWeights, 
								_ => throw new Exception($"Incorrect node content type for EventTrigger_AdventureEnterNode: {adventureMapPoint2.NodeContentType} given."), 
							};
							if (1 == 0)
							{
							}
							(string, short)[] list2 = array2;
							if (!list2.CheckIndex(nodeContentIndex2))
							{
								throw new Exception($"Invalid eventIndex {nodeContentIndex2} of node {adventureMapPoint2.GetDetailedInfo()}");
							}
						}
					}
				}
			}
		}
		AdaptableLog.TagInfo("Test_GenerateAdventures", $"Total of {100} iterations executed for each adventure.");
	}

	private static void Test_GetClosestNeighboringGradeWithValidItem()
	{
		Logger.Warn("Testing Sword: ");
		for (sbyte b = 0; b < 9; b++)
		{
			sbyte closestNeighboringGradeWithValidItem = ItemDomain.GetClosestNeighboringGradeWithValidItem(b, Config.Weapon.Instance.ToList(), ((WeaponItem, sbyte) pair) => pair.Item1.Grade == pair.Item2 && 8 == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
			Logger.Info($"Queried grade: {b} \t Closest grade: {closestNeighboringGradeWithValidItem}");
		}
		Logger.Warn("Testing Medicine: ");
		for (sbyte b2 = 0; b2 < 9; b2++)
		{
			sbyte closestNeighboringGradeWithValidItem2 = ItemDomain.GetClosestNeighboringGradeWithValidItem(b2, Config.Medicine.Instance.ToList(), ((MedicineItem, sbyte) pair) => pair.Item1.Grade == pair.Item2 && 800 == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
			Logger.Info($"Queried grade: {b2} \t Closest grade: {closestNeighboringGradeWithValidItem2}");
		}
		Logger.Warn("Testing WoodMaterial: ");
		for (sbyte b3 = 0; b3 < 9; b3++)
		{
			sbyte closestNeighboringGradeWithValidItem3 = ItemDomain.GetClosestNeighboringGradeWithValidItem(b3, Config.Material.Instance.ToList(), ((MaterialItem, sbyte) pair) => pair.Item1.Grade == pair.Item2 && 501 == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
			Logger.Info($"Queried grade: {b3} \t Closest grade: {closestNeighboringGradeWithValidItem3}");
		}
	}

	private static void TestLifeSkillRequirementDistribution(DataContext context)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < 9; i++)
		{
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder3 = stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(3, 1, stringBuilder2);
			handler.AppendLiteral("难度");
			handler.AppendFormatted(i + 1);
			handler.AppendLiteral("\t");
			stringBuilder3.Append(ref handler);
		}
		stringBuilder.AppendLine();
		for (int j = 0; j < 1000; j++)
		{
			for (int k = 0; k < 9; k++)
			{
				int baseSkillRequirement = AdvMapBranch.GetBaseSkillRequirement(context.Random);
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder4 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
				handler.AppendFormatted(baseSkillRequirement * (k + 1));
				handler.AppendLiteral("\t");
				stringBuilder4.Append(ref handler);
			}
			stringBuilder.AppendLine();
		}
		File.WriteAllText("LifeSkillRequirementDist.txt", stringBuilder.ToString());
	}

	private static void Test_AdventureGeneration(DataContext context)
	{
		Logger.Info("Test - generate adventures in settlements...");
		for (EMapBlockSubType eMapBlockSubType = EMapBlockSubType.TaiwuCun; eMapBlockSubType < EMapBlockSubType.Farmland; eMapBlockSubType++)
		{
			sbyte stateTemplateId = 0;
			sbyte areaFilterType = -1;
			MapBlockData randomMapBlockDataByFilters = DomainManager.Adventure.GetRandomMapBlockDataByFilters(context, stateTemplateId, areaFilterType, new List<short> { (short)eMapBlockSubType });
			short adventureId = (short)context.Random.Next(0, Config.Adventure.Instance.Count);
			DomainManager.Adventure.TryCreateAdventureSite(context, randomMapBlockDataByFilters.AreaId, randomMapBlockDataByFilters.BlockId, adventureId, MonthlyActionKey.Invalid);
		}
		for (sbyte b = 0; b < MapState.Instance.Count; b++)
		{
			for (int i = 0; i < 30; i++)
			{
				sbyte areaFilterType2 = (sbyte)context.Random.Next(-1, 1);
				MapBlockData randomMapBlockDataByFilters2 = DomainManager.Adventure.GetRandomMapBlockDataByFilters(context, b, areaFilterType2, null);
				short adventureId2 = (short)context.Random.Next(0, Config.Adventure.Instance.Count);
				if (randomMapBlockDataByFilters2 != null)
				{
					DomainManager.Adventure.TryCreateAdventureSite(context, randomMapBlockDataByFilters2.AreaId, randomMapBlockDataByFilters2.BlockId, adventureId2, MonthlyActionKey.Invalid);
				}
			}
		}
	}

	public AdventureDomain()
		: base(25)
	{
		_playerPos = 0;
		_personalities = new int[7];
		_personalitiesCost = new int[7];
		_personalitiesGain = new int[7];
		_indicatePath = new List<int>();
		_arrangeableNodes = new List<int>();
		_arrangedNodes = new List<int>();
		_perceivedNodes = new List<int>();
		_errorInfo = string.Empty;
		_adBranchOpenRecord = new Dictionary<int, int>(0);
		_curAdventureId = 0;
		_operationBlock = false;
		_adventureState = 0;
		_curMapTrunk = new AdventureMapTrunk();
		_advParameters = new List<int>();
		_enterItems = new List<ItemKey>();
		_allowExitAdventure = false;
		_adventureAreas = new AreaAdventureData[139];
		_enemyNestSites = new Dictionary<Location, EnemyNestSiteExtraData>(0);
		_brokenAreaEnemies = new BrokenAreaData[90];
		_spentCharList = new List<int>();
		_curBranchChosenChar = 0;
		_escapingRandomEnemies = new List<short>();
		_teamDetectedNodes = new List<int>();
		_actionPointWithhold = 0;
		OnInitializedDomainData();
	}

	public int GetPlayerPos()
	{
		return _playerPos;
	}

	public void SetPlayerPos(int value, DataContext context)
	{
		_playerPos = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
	}

	public int[] GetPersonalities()
	{
		return _personalities;
	}

	public void SetPersonalities(int[] value, DataContext context)
	{
		_personalities = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
	}

	public int[] GetPersonalitiesCost()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 2))
		{
			return _personalitiesCost;
		}
		int[] array = new int[7];
		CalcPersonalitiesCost(array);
		bool lockTaken = false;
		try
		{
			_spinLockPersonalitiesCost.Enter(ref lockTaken);
			for (int i = 0; i < 7; i++)
			{
				_personalitiesCost[i] = array[i];
			}
			BaseGameDataDomain.SetCached(DataStates, 2);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockPersonalitiesCost.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _personalitiesCost;
	}

	public int[] GetPersonalitiesGain()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 3))
		{
			return _personalitiesGain;
		}
		int[] array = new int[7];
		CalcPersonalitiesGain(array);
		bool lockTaken = false;
		try
		{
			_spinLockPersonalitiesGain.Enter(ref lockTaken);
			for (int i = 0; i < 7; i++)
			{
				_personalitiesGain[i] = array[i];
			}
			BaseGameDataDomain.SetCached(DataStates, 3);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockPersonalitiesGain.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _personalitiesGain;
	}

	public List<int> GetIndicatePath()
	{
		return _indicatePath;
	}

	public void SetIndicatePath(List<int> value, DataContext context)
	{
		_indicatePath = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
	}

	public List<int> GetArrangeableNodes()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 5))
		{
			return _arrangeableNodes;
		}
		List<int> list = new List<int>();
		CalcArrangeableNodes(list);
		bool lockTaken = false;
		try
		{
			_spinLockArrangeableNodes.Enter(ref lockTaken);
			_arrangeableNodes.Assign(list);
			BaseGameDataDomain.SetCached(DataStates, 5);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockArrangeableNodes.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _arrangeableNodes;
	}

	public List<int> GetArrangedNodes()
	{
		return _arrangedNodes;
	}

	public void SetArrangedNodes(List<int> value, DataContext context)
	{
		_arrangedNodes = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
	}

	public List<int> GetPerceivedNodes()
	{
		return _perceivedNodes;
	}

	public void SetPerceivedNodes(List<int> value, DataContext context)
	{
		_perceivedNodes = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
	}

	public string GetErrorInfo()
	{
		return _errorInfo;
	}

	public void SetErrorInfo(string value, DataContext context)
	{
		_errorInfo = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
	}

	private int GetElement_AdBranchOpenRecord(int elementId)
	{
		return _adBranchOpenRecord[elementId];
	}

	private bool TryGetElement_AdBranchOpenRecord(int elementId, out int value)
	{
		return _adBranchOpenRecord.TryGetValue(elementId, out value);
	}

	private void AddElement_AdBranchOpenRecord(int elementId, int value, DataContext context)
	{
		_adBranchOpenRecord.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, DataStates, CacheInfluences, context);
	}

	private void SetElement_AdBranchOpenRecord(int elementId, int value, DataContext context)
	{
		_adBranchOpenRecord[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, DataStates, CacheInfluences, context);
	}

	private void RemoveElement_AdBranchOpenRecord(int elementId, DataContext context)
	{
		_adBranchOpenRecord.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, DataStates, CacheInfluences, context);
	}

	private void ClearAdBranchOpenRecord(DataContext context)
	{
		_adBranchOpenRecord.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, DataStates, CacheInfluences, context);
	}

	public short GetCurAdventureId()
	{
		return _curAdventureId;
	}

	public void SetCurAdventureId(short value, DataContext context)
	{
		_curAdventureId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, DataStates, CacheInfluences, context);
	}

	public bool GetOperationBlock()
	{
		return _operationBlock;
	}

	public void SetOperationBlock(bool value, DataContext context)
	{
		_operationBlock = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, DataStates, CacheInfluences, context);
	}

	public sbyte GetAdventureState()
	{
		return _adventureState;
	}

	public void SetAdventureState(sbyte value, DataContext context)
	{
		_adventureState = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, DataStates, CacheInfluences, context);
	}

	public AdventureMapTrunk GetCurMapTrunk()
	{
		return _curMapTrunk;
	}

	public void SetCurMapTrunk(AdventureMapTrunk value, DataContext context)
	{
		_curMapTrunk = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, DataStates, CacheInfluences, context);
	}

	public List<int> GetAdvParameters()
	{
		return _advParameters;
	}

	public void SetAdvParameters(List<int> value, DataContext context)
	{
		_advParameters = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
	}

	public List<ItemKey> GetEnterItems()
	{
		return _enterItems;
	}

	private void SetEnterItems(List<ItemKey> value, DataContext context)
	{
		_enterItems = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, DataStates, CacheInfluences, context);
	}

	public bool GetAllowExitAdventure()
	{
		return _allowExitAdventure;
	}

	public void SetAllowExitAdventure(bool value, DataContext context)
	{
		_allowExitAdventure = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, DataStates, CacheInfluences, context);
	}

	public AreaAdventureData GetElement_AdventureAreas(int index)
	{
		return _adventureAreas[index];
	}

	private unsafe void SetElement_AdventureAreas(int index, AreaAdventureData value, DataContext context)
	{
		_adventureAreas[index] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, _dataStatesAdventureAreas, CacheInfluencesAdventureAreas, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicElementList_Set(10, 17, index, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicElementList_Set(10, 17, index, 0);
		}
	}

	public EnemyNestSiteExtraData GetElement_EnemyNestSites(Location elementId)
	{
		return _enemyNestSites[elementId];
	}

	public bool TryGetElement_EnemyNestSites(Location elementId, out EnemyNestSiteExtraData value)
	{
		return _enemyNestSites.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_EnemyNestSites(Location elementId, EnemyNestSiteExtraData value, DataContext context)
	{
		_enemyNestSites.Add(elementId, value);
		_modificationsEnemyNestSites.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(10, 18, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(10, 18, elementId, 0);
		}
	}

	private unsafe void SetElement_EnemyNestSites(Location elementId, EnemyNestSiteExtraData value, DataContext context)
	{
		_enemyNestSites[elementId] = value;
		_modificationsEnemyNestSites.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(10, 18, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(10, 18, elementId, 0);
		}
	}

	private void RemoveElement_EnemyNestSites(Location elementId, DataContext context)
	{
		_enemyNestSites.Remove(elementId);
		_modificationsEnemyNestSites.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(10, 18, elementId);
	}

	private void ClearEnemyNestSites(DataContext context)
	{
		_enemyNestSites.Clear();
		_modificationsEnemyNestSites.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(10, 18);
	}

	public BrokenAreaData GetElement_BrokenAreaEnemies(int index)
	{
		return _brokenAreaEnemies[index];
	}

	private unsafe void SetElement_BrokenAreaEnemies(int index, BrokenAreaData value, DataContext context)
	{
		_brokenAreaEnemies[index] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, _dataStatesBrokenAreaEnemies, CacheInfluencesBrokenAreaEnemies, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicElementList_Set(10, 19, index, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicElementList_Set(10, 19, index, 0);
		}
	}

	public List<int> GetSpentCharList()
	{
		return _spentCharList;
	}

	public void SetSpentCharList(List<int> value, DataContext context)
	{
		_spentCharList = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
	}

	public int GetCurBranchChosenChar()
	{
		return _curBranchChosenChar;
	}

	public void SetCurBranchChosenChar(int value, DataContext context)
	{
		_curBranchChosenChar = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, DataStates, CacheInfluences, context);
	}

	public List<short> GetEscapingRandomEnemies()
	{
		return _escapingRandomEnemies;
	}

	public unsafe void SetEscapingRandomEnemies(List<short> value, DataContext context)
	{
		_escapingRandomEnemies = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, DataStates, CacheInfluences, context);
		int count = _escapingRandomEnemies.Count;
		int num = 2 * count;
		int valueSize = 2 + num;
		byte* ptr = OperationAdder.DynamicSingleValue_Set(10, 22, valueSize);
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int i = 0; i < count; i++)
		{
			((short*)ptr)[i] = _escapingRandomEnemies[i];
		}
		ptr += num;
	}

	public List<int> GetTeamDetectedNodes()
	{
		return _teamDetectedNodes;
	}

	public void SetTeamDetectedNodes(List<int> value, DataContext context)
	{
		_teamDetectedNodes = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, DataStates, CacheInfluences, context);
	}

	public int GetActionPointWithhold()
	{
		return _actionPointWithhold;
	}

	public void SetActionPointWithhold(int value, DataContext context)
	{
		_actionPointWithhold = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, DataStates, CacheInfluences, context);
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public unsafe override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
		int num = 0;
		for (int i = 0; i < 139; i++)
		{
			AreaAdventureData areaAdventureData = _adventureAreas[i];
			num = ((areaAdventureData == null) ? (num + 4) : (num + (4 + areaAdventureData.GetSerializedSize())));
		}
		byte* ptr = OperationAdder.DynamicElementList_InsertRange(10, 17, 0, 139, num);
		for (int j = 0; j < 139; j++)
		{
			AreaAdventureData areaAdventureData2 = _adventureAreas[j];
			if (areaAdventureData2 != null)
			{
				byte* ptr2 = ptr;
				ptr += 4;
				int num2 = areaAdventureData2.Serialize(ptr);
				ptr += num2;
				*(int*)ptr2 = num2;
			}
			else
			{
				*(int*)ptr = 0;
				ptr += 4;
			}
		}
		foreach (KeyValuePair<Location, EnemyNestSiteExtraData> enemyNestSite in _enemyNestSites)
		{
			Location key = enemyNestSite.Key;
			EnemyNestSiteExtraData value = enemyNestSite.Value;
			if (value != null)
			{
				int serializedSize = value.GetSerializedSize();
				byte* ptr3 = OperationAdder.DynamicSingleValueCollection_Add(10, 18, key, serializedSize);
				ptr3 += value.Serialize(ptr3);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(10, 18, key, 0);
			}
		}
		int num3 = 0;
		for (int k = 0; k < 90; k++)
		{
			BrokenAreaData brokenAreaData = _brokenAreaEnemies[k];
			num3 = ((brokenAreaData == null) ? (num3 + 4) : (num3 + (4 + brokenAreaData.GetSerializedSize())));
		}
		byte* ptr4 = OperationAdder.DynamicElementList_InsertRange(10, 19, 0, 90, num3);
		for (int l = 0; l < 90; l++)
		{
			BrokenAreaData brokenAreaData2 = _brokenAreaEnemies[l];
			if (brokenAreaData2 != null)
			{
				byte* ptr5 = ptr4;
				ptr4 += 4;
				int num4 = brokenAreaData2.Serialize(ptr4);
				ptr4 += num4;
				*(int*)ptr5 = num4;
			}
			else
			{
				*(int*)ptr4 = 0;
				ptr4 += 4;
			}
		}
		int count = _escapingRandomEnemies.Count;
		int num5 = 2 * count;
		int valueSize = 2 + num5;
		byte* ptr6 = OperationAdder.DynamicSingleValue_Set(10, 22, valueSize);
		*(ushort*)ptr6 = (ushort)count;
		ptr6 += 2;
		for (int m = 0; m < count; m++)
		{
			((short*)ptr6)[m] = _escapingRandomEnemies[m];
		}
		ptr6 += num5;
	}

	public override void OnLoadWorld()
	{
		_pendingLoadingOperationIds = new Queue<uint>();
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicElementList_GetAll(10, 17));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(10, 18));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicElementList_GetAll(10, 19));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(10, 22));
	}

	public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
	{
		switch (dataId)
		{
		case 0:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 0);
			}
			return GameData.Serializer.Serializer.Serialize(_playerPos, dataPool);
		case 1:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
			}
			return GameData.Serializer.Serializer.Serialize(_personalities, dataPool);
		case 2:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 2);
			}
			return GameData.Serializer.Serializer.Serialize(GetPersonalitiesCost(), dataPool);
		case 3:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 3);
			}
			return GameData.Serializer.Serializer.Serialize(GetPersonalitiesGain(), dataPool);
		case 4:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 4);
			}
			return GameData.Serializer.Serializer.Serialize(_indicatePath, dataPool);
		case 5:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 5);
			}
			return GameData.Serializer.Serializer.Serialize(GetArrangeableNodes(), dataPool);
		case 6:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 6);
			}
			return GameData.Serializer.Serializer.Serialize(_arrangedNodes, dataPool);
		case 7:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 7);
			}
			return GameData.Serializer.Serializer.Serialize(_perceivedNodes, dataPool);
		case 8:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
			}
			return GameData.Serializer.Serializer.Serialize(_errorInfo, dataPool);
		case 9:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 10:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 10);
			}
			return GameData.Serializer.Serializer.Serialize(_curAdventureId, dataPool);
		case 11:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 11);
			}
			return GameData.Serializer.Serializer.Serialize(_operationBlock, dataPool);
		case 12:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 12);
			}
			return GameData.Serializer.Serializer.Serialize(_adventureState, dataPool);
		case 13:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 13);
			}
			return GameData.Serializer.Serializer.Serialize(_curMapTrunk, dataPool);
		case 14:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 14);
			}
			return GameData.Serializer.Serializer.Serialize(_advParameters, dataPool);
		case 15:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 15);
			}
			return GameData.Serializer.Serializer.Serialize(_enterItems, dataPool);
		case 16:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 16);
			}
			return GameData.Serializer.Serializer.Serialize(_allowExitAdventure, dataPool);
		case 17:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(_dataStatesAdventureAreas, (int)subId0);
			}
			return GameData.Serializer.Serializer.Serialize(_adventureAreas[(uint)subId0], dataPool);
		case 18:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 18);
				_modificationsEnemyNestSites.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<Location, EnemyNestSiteExtraData>)_enemyNestSites, dataPool);
		case 19:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(_dataStatesBrokenAreaEnemies, (int)subId0);
			}
			return GameData.Serializer.Serializer.Serialize(_brokenAreaEnemies[(uint)subId0], dataPool);
		case 20:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 20);
			}
			return GameData.Serializer.Serializer.Serialize(_spentCharList, dataPool);
		case 21:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 21);
			}
			return GameData.Serializer.Serializer.Serialize(_curBranchChosenChar, dataPool);
		case 22:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 22);
			}
			return GameData.Serializer.Serializer.Serialize(_escapingRandomEnemies, dataPool);
		case 23:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 23);
			}
			return GameData.Serializer.Serializer.Serialize(_teamDetectedNodes, dataPool);
		case 24:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 24);
			}
			return GameData.Serializer.Serializer.Serialize(_actionPointWithhold, dataPool);
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		switch (dataId)
		{
		case 0:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _playerPos);
			SetPlayerPos(_playerPos, context);
			break;
		case 1:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _personalities);
			SetPersonalities(_personalities, context);
			break;
		case 2:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 3:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 4:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _indicatePath);
			SetIndicatePath(_indicatePath, context);
			break;
		case 5:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 6:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _arrangedNodes);
			SetArrangedNodes(_arrangedNodes, context);
			break;
		case 7:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _perceivedNodes);
			SetPerceivedNodes(_perceivedNodes, context);
			break;
		case 8:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _errorInfo);
			SetErrorInfo(_errorInfo, context);
			break;
		case 9:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 10:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _curAdventureId);
			SetCurAdventureId(_curAdventureId, context);
			break;
		case 11:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _operationBlock);
			SetOperationBlock(_operationBlock, context);
			break;
		case 12:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _adventureState);
			SetAdventureState(_adventureState, context);
			break;
		case 13:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _curMapTrunk);
			SetCurMapTrunk(_curMapTrunk, context);
			break;
		case 14:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _advParameters);
			SetAdvParameters(_advParameters, context);
			break;
		case 15:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 16:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _allowExitAdventure);
			SetAllowExitAdventure(_allowExitAdventure, context);
			break;
		case 17:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 18:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 19:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 20:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _spentCharList);
			SetSpentCharList(_spentCharList, context);
			break;
		case 21:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _curBranchChosenChar);
			SetCurBranchChosenChar(_curBranchChosenChar, context);
			break;
		case 22:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _escapingRandomEnemies);
			SetEscapingRandomEnemies(_escapingRandomEnemies, context);
			break;
		case 23:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _teamDetectedNodes);
			SetTeamDetectedNodes(_teamDetectedNodes, context);
			break;
		case 24:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _actionPointWithhold);
			SetActionPointWithhold(_actionPointWithhold, context);
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
			int argsCount14 = operation.ArgsCount;
			int num14 = argsCount14;
			if (num14 == 1)
			{
				short item29 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item29);
				Location item30 = GmCmd_GenerateAdventure(context, item29);
				return GameData.Serializer.Serializer.Serialize(item30, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 1:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				AdventureItem item17 = null;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item17);
				int item18 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item18);
				bool item19 = EnterAdventureByConfigData(context, item17, item18);
				return GameData.Serializer.Serializer.Serialize(item19, returnDataPool);
			}
			case 3:
			{
				AdventureItem item13 = null;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item13);
				int item14 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item14);
				int item15 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item15);
				bool item16 = EnterAdventureByConfigData(context, item13, item14, item15);
				return GameData.Serializer.Serializer.Serialize(item16, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 2:
		{
			int argsCount15 = operation.ArgsCount;
			int num15 = argsCount15;
			if (num15 == 1)
			{
				sbyte item31 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item31);
				SelectBranchForPreview(context, item31);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 3:
			if (operation.ArgsCount == 0)
			{
				ClearBranchForPreview(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 4:
		{
			int argsCount10 = operation.ArgsCount;
			int num10 = argsCount10;
			if (num10 == 2)
			{
				short item23 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item23);
				List<ItemKey> item24 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item24);
				bool item25 = EnterAdventure(context, item23, item24);
				return GameData.Serializer.Serializer.Serialize(item25, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 5:
		{
			int argsCount4 = operation.ArgsCount;
			int num4 = argsCount4;
			if (num4 == 1)
			{
				int item6 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item6);
				ArrangeNode(context, item6);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 6:
		{
			int argsCount12 = operation.ArgsCount;
			int num12 = argsCount12;
			if (num12 == 1)
			{
				int item27 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item27);
				PerceiveNode(context, item27);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 7:
			if (operation.ArgsCount == 0)
			{
				ConfirmPath(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 8:
			if (operation.ArgsCount == 0)
			{
				(int, int) item34 = ConfirmArrived(context);
				return GameData.Serializer.Serializer.Serialize(item34, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 9:
		{
			int argsCount13 = operation.ArgsCount;
			int num13 = argsCount13;
			if (num13 == 1)
			{
				bool item28 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item28);
				ExitAdventure(context, item28);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 10:
			if (operation.ArgsCount == 0)
			{
				SwitchBranch(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 11:
			if (operation.ArgsCount == 0)
			{
				ClearPathArrangement(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 12:
		{
			int argsCount2 = operation.ArgsCount;
			int num2 = argsCount2;
			if (num2 == 1)
			{
				short item3 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item3);
				AreaAdventureData adventuresInArea = GetAdventuresInArea(item3);
				return GameData.Serializer.Serializer.Serialize(adventuresInArea, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 13:
			if (operation.ArgsCount == 0)
			{
				List<short> awakeSwordTombs = GetAwakeSwordTombs();
				return GameData.Serializer.Serializer.Serialize(awakeSwordTombs, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 14:
			if (operation.ArgsCount == 0)
			{
				List<short> attackingSwordTombs = GetAttackingSwordTombs();
				return GameData.Serializer.Serializer.Serialize(attackingSwordTombs, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 15:
		{
			int argsCount9 = operation.ArgsCount;
			int num9 = argsCount9;
			if (num9 == 1)
			{
				int item22 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item22);
				SetCharacterToAdvanceBranch(context, item22);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 16:
			if (operation.ArgsCount == 0)
			{
				bool item10 = CanSetCharacterToAdvanceBranch();
				return GameData.Serializer.Serializer.Serialize(item10, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 17:
		{
			int argsCount5 = operation.ArgsCount;
			int num5 = argsCount5;
			if (num5 == 1)
			{
				List<Location> item8 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item8);
				Dictionary<Location, AdventureSiteData> adventureSiteDataDict = GetAdventureSiteDataDict(item8);
				return GameData.Serializer.Serializer.Serialize(adventureSiteDataDict, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 18:
			if (operation.ArgsCount == 0)
			{
				AdventureResultDisplayData adventureResultDisplayData = GetAdventureResultDisplayData();
				return GameData.Serializer.Serializer.Serialize(adventureResultDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 19:
		{
			int argsCount16 = operation.ArgsCount;
			int num16 = argsCount16;
			if (num16 == 2)
			{
				List<ItemKey> item32 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item32);
				List<int> item33 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item33);
				SelectGetItem(context, item32, item33);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 20:
			if (operation.ArgsCount == 0)
			{
				List<(int, int)> pathContentList = GetPathContentList();
				return GameData.Serializer.Serializer.Serialize(pathContentList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 21:
			if (operation.ArgsCount == 0)
			{
				List<int> adventureSpentCharList = GetAdventureSpentCharList();
				return GameData.Serializer.Serializer.Serialize(adventureSpentCharList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 22:
		{
			int argsCount11 = operation.ArgsCount;
			int num11 = argsCount11;
			if (num11 == 1)
			{
				ItemKey item26 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item26);
				ItemDisplayData adventureGainsItemList = GetAdventureGainsItemList(item26);
				return GameData.Serializer.Serializer.Serialize(adventureGainsItemList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 23:
		{
			int argsCount8 = operation.ArgsCount;
			int num8 = argsCount8;
			if (num8 == 1)
			{
				short item20 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item20);
				Location item21 = QueryAdventureLocation(item20);
				return GameData.Serializer.Serializer.Serialize(item21, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 24:
		{
			int argsCount7 = operation.ArgsCount;
			int num7 = argsCount7;
			if (num7 == 1)
			{
				short item11 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item11);
				Location item12 = QueryAdventureLocationFirstInCurrent(item11);
				return GameData.Serializer.Serializer.Serialize(item12, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 25:
		{
			int argsCount6 = operation.ArgsCount;
			int num6 = argsCount6;
			if (num6 == 1)
			{
				Location item9 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item9);
				List<CharacterDisplayData> characterDisplayDataListInAdventure = GetCharacterDisplayDataListInAdventure(item9);
				return GameData.Serializer.Serializer.Serialize(characterDisplayDataListInAdventure, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 26:
			if (operation.ArgsCount == 0)
			{
				bool item7 = TryInvokeConfirmEnterEvent();
				return GameData.Serializer.Serializer.Serialize(item7, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 27:
		{
			int argsCount3 = operation.ArgsCount;
			int num3 = argsCount3;
			if (num3 == 2)
			{
				string item4 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item4);
				int item5 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item5);
				GmCmd_SetAdventureParameter(item4, item5);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 28:
		{
			int argsCount = operation.ArgsCount;
			int num = argsCount;
			if (num == 1)
			{
				string item = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item);
				int item2 = GmCmd_GetAdventureParameter(item);
				return GameData.Serializer.Serializer.Serialize(item2, returnDataPool);
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
			break;
		case 1:
			break;
		case 2:
			break;
		case 3:
			break;
		case 4:
			break;
		case 5:
			break;
		case 6:
			break;
		case 7:
			break;
		case 8:
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
			_modificationsEnemyNestSites.ChangeRecording(monitoring);
			break;
		case 19:
			break;
		case 20:
			break;
		case 21:
			break;
		case 22:
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
			if (!BaseGameDataDomain.IsModified(DataStates, 0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 0);
			return GameData.Serializer.Serializer.Serialize(_playerPos, dataPool);
		case 1:
			if (!BaseGameDataDomain.IsModified(DataStates, 1))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 1);
			return GameData.Serializer.Serializer.Serialize(_personalities, dataPool);
		case 2:
			if (!BaseGameDataDomain.IsModified(DataStates, 2))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 2);
			return GameData.Serializer.Serializer.Serialize(GetPersonalitiesCost(), dataPool);
		case 3:
			if (!BaseGameDataDomain.IsModified(DataStates, 3))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 3);
			return GameData.Serializer.Serializer.Serialize(GetPersonalitiesGain(), dataPool);
		case 4:
			if (!BaseGameDataDomain.IsModified(DataStates, 4))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 4);
			return GameData.Serializer.Serializer.Serialize(_indicatePath, dataPool);
		case 5:
			if (!BaseGameDataDomain.IsModified(DataStates, 5))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 5);
			return GameData.Serializer.Serializer.Serialize(GetArrangeableNodes(), dataPool);
		case 6:
			if (!BaseGameDataDomain.IsModified(DataStates, 6))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 6);
			return GameData.Serializer.Serializer.Serialize(_arrangedNodes, dataPool);
		case 7:
			if (!BaseGameDataDomain.IsModified(DataStates, 7))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 7);
			return GameData.Serializer.Serializer.Serialize(_perceivedNodes, dataPool);
		case 8:
			if (!BaseGameDataDomain.IsModified(DataStates, 8))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 8);
			return GameData.Serializer.Serializer.Serialize(_errorInfo, dataPool);
		case 9:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 10:
			if (!BaseGameDataDomain.IsModified(DataStates, 10))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 10);
			return GameData.Serializer.Serializer.Serialize(_curAdventureId, dataPool);
		case 11:
			if (!BaseGameDataDomain.IsModified(DataStates, 11))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 11);
			return GameData.Serializer.Serializer.Serialize(_operationBlock, dataPool);
		case 12:
			if (!BaseGameDataDomain.IsModified(DataStates, 12))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 12);
			return GameData.Serializer.Serializer.Serialize(_adventureState, dataPool);
		case 13:
			if (!BaseGameDataDomain.IsModified(DataStates, 13))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 13);
			return GameData.Serializer.Serializer.Serialize(_curMapTrunk, dataPool);
		case 14:
			if (!BaseGameDataDomain.IsModified(DataStates, 14))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 14);
			return GameData.Serializer.Serializer.Serialize(_advParameters, dataPool);
		case 15:
			if (!BaseGameDataDomain.IsModified(DataStates, 15))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 15);
			return GameData.Serializer.Serializer.Serialize(_enterItems, dataPool);
		case 16:
			if (!BaseGameDataDomain.IsModified(DataStates, 16))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 16);
			return GameData.Serializer.Serializer.Serialize(_allowExitAdventure, dataPool);
		case 17:
			if (!BaseGameDataDomain.IsModified(_dataStatesAdventureAreas, (int)subId0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(_dataStatesAdventureAreas, (int)subId0);
			return GameData.Serializer.Serializer.Serialize(_adventureAreas[(uint)subId0], dataPool);
		case 18:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 18))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 18);
			int result = GameData.Serializer.Serializer.SerializeModifications(_enemyNestSites, dataPool, _modificationsEnemyNestSites);
			_modificationsEnemyNestSites.Reset();
			return result;
		}
		case 19:
			if (!BaseGameDataDomain.IsModified(_dataStatesBrokenAreaEnemies, (int)subId0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(_dataStatesBrokenAreaEnemies, (int)subId0);
			return GameData.Serializer.Serializer.Serialize(_brokenAreaEnemies[(uint)subId0], dataPool);
		case 20:
			if (!BaseGameDataDomain.IsModified(DataStates, 20))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 20);
			return GameData.Serializer.Serializer.Serialize(_spentCharList, dataPool);
		case 21:
			if (!BaseGameDataDomain.IsModified(DataStates, 21))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 21);
			return GameData.Serializer.Serializer.Serialize(_curBranchChosenChar, dataPool);
		case 22:
			if (!BaseGameDataDomain.IsModified(DataStates, 22))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 22);
			return GameData.Serializer.Serializer.Serialize(_escapingRandomEnemies, dataPool);
		case 23:
			if (!BaseGameDataDomain.IsModified(DataStates, 23))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 23);
			return GameData.Serializer.Serializer.Serialize(_teamDetectedNodes, dataPool);
		case 24:
			if (!BaseGameDataDomain.IsModified(DataStates, 24))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 24);
			return GameData.Serializer.Serializer.Serialize(_actionPointWithhold, dataPool);
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
			}
			break;
		case 1:
			if (BaseGameDataDomain.IsModified(DataStates, 1))
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
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
			}
			break;
		case 4:
			if (BaseGameDataDomain.IsModified(DataStates, 4))
			{
				BaseGameDataDomain.ResetModified(DataStates, 4);
			}
			break;
		case 5:
			if (BaseGameDataDomain.IsModified(DataStates, 5))
			{
				BaseGameDataDomain.ResetModified(DataStates, 5);
			}
			break;
		case 6:
			if (BaseGameDataDomain.IsModified(DataStates, 6))
			{
				BaseGameDataDomain.ResetModified(DataStates, 6);
			}
			break;
		case 7:
			if (BaseGameDataDomain.IsModified(DataStates, 7))
			{
				BaseGameDataDomain.ResetModified(DataStates, 7);
			}
			break;
		case 8:
			if (BaseGameDataDomain.IsModified(DataStates, 8))
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
			}
			break;
		case 9:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
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
			if (BaseGameDataDomain.IsModified(DataStates, 12))
			{
				BaseGameDataDomain.ResetModified(DataStates, 12);
			}
			break;
		case 13:
			if (BaseGameDataDomain.IsModified(DataStates, 13))
			{
				BaseGameDataDomain.ResetModified(DataStates, 13);
			}
			break;
		case 14:
			if (BaseGameDataDomain.IsModified(DataStates, 14))
			{
				BaseGameDataDomain.ResetModified(DataStates, 14);
			}
			break;
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
			if (BaseGameDataDomain.IsModified(_dataStatesAdventureAreas, (int)subId0))
			{
				BaseGameDataDomain.ResetModified(_dataStatesAdventureAreas, (int)subId0);
			}
			break;
		case 18:
			if (BaseGameDataDomain.IsModified(DataStates, 18))
			{
				BaseGameDataDomain.ResetModified(DataStates, 18);
				_modificationsEnemyNestSites.Reset();
			}
			break;
		case 19:
			if (BaseGameDataDomain.IsModified(_dataStatesBrokenAreaEnemies, (int)subId0))
			{
				BaseGameDataDomain.ResetModified(_dataStatesBrokenAreaEnemies, (int)subId0);
			}
			break;
		case 20:
			if (BaseGameDataDomain.IsModified(DataStates, 20))
			{
				BaseGameDataDomain.ResetModified(DataStates, 20);
			}
			break;
		case 21:
			if (BaseGameDataDomain.IsModified(DataStates, 21))
			{
				BaseGameDataDomain.ResetModified(DataStates, 21);
			}
			break;
		case 22:
			if (BaseGameDataDomain.IsModified(DataStates, 22))
			{
				BaseGameDataDomain.ResetModified(DataStates, 22);
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
			6 => BaseGameDataDomain.IsModified(DataStates, 6), 
			7 => BaseGameDataDomain.IsModified(DataStates, 7), 
			8 => BaseGameDataDomain.IsModified(DataStates, 8), 
			9 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			10 => BaseGameDataDomain.IsModified(DataStates, 10), 
			11 => BaseGameDataDomain.IsModified(DataStates, 11), 
			12 => BaseGameDataDomain.IsModified(DataStates, 12), 
			13 => BaseGameDataDomain.IsModified(DataStates, 13), 
			14 => BaseGameDataDomain.IsModified(DataStates, 14), 
			15 => BaseGameDataDomain.IsModified(DataStates, 15), 
			16 => BaseGameDataDomain.IsModified(DataStates, 16), 
			17 => BaseGameDataDomain.IsModified(_dataStatesAdventureAreas, (int)subId0), 
			18 => BaseGameDataDomain.IsModified(DataStates, 18), 
			19 => BaseGameDataDomain.IsModified(_dataStatesBrokenAreaEnemies, (int)subId0), 
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
		case 2:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(2, DataStates, CacheInfluences, context);
			break;
		case 3:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(3, DataStates, CacheInfluences, context);
			break;
		case 5:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(5, DataStates, CacheInfluences, context);
			break;
		default:
			throw new Exception($"Unsupported dataId {influence.TargetIndicator.DataId}");
		case 0:
		case 1:
		case 4:
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
		case 17:
			ResponseProcessor.ProcessElementList_CustomType_Dynamic_Ref(operation, pResult, _adventureAreas, 139);
			goto IL_015d;
		case 18:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _enemyNestSites);
			goto IL_015d;
		case 19:
			ResponseProcessor.ProcessElementList_CustomType_Dynamic_Ref(operation, pResult, _brokenAreaEnemies, 90);
			goto IL_015d;
		case 22:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_List(operation, pResult, _escapingRandomEnemies);
			goto IL_015d;
		default:
			throw new Exception($"Unsupported dataId {operation.DataId}");
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
		case 20:
		case 21:
		case 23:
		case 24:
			{
				throw new Exception($"Cannot process archive response of non-archive data {operation.DataId}");
			}
			IL_015d:
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
					DomainManager.Global.CompleteLoading(10);
				}
			}
			break;
		}
	}

	private void InitializeInternalDataOfCollections()
	{
	}
}
