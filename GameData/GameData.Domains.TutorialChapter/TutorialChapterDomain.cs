using System;
using System.Collections.Generic;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Dependencies;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Relation;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.World;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TutorialChapter;

[GameDataDomain(15)]
public class TutorialChapterDomain : BaseGameDataDomain
{
	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private int _curProgress;

	private Dictionary<string, int> _fixedCharacters;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _inGuiding = false;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private short _tutorialChapter = -1;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _canMove = true;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _canOpenCharacterMenu = true;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private string _guidVideoName = string.Empty;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _canAdvanceMonth = true;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private Location _nextForceLocation = Location.Invalid;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _neiliAllocateFitChapter7;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _huanxinDying;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _huanxinSurprised;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _canShowEnterIndustry = true;

	public sbyte Counter;

	public sbyte CounterTarget;

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[12][];

	private Queue<uint> _pendingLoadingOperationIds;

	public byte TutorialAreaSize { get; private set; } = 20;

	public bool InGuiding => _inGuiding;

	[DomainMethod]
	public unsafe int CreateFixedWorld(DataContext context, short templateId, WorldCreationInfo info)
	{
		SetInGuiding(value: true, context);
		TutorialChaptersItem item = TutorialChapters.Instance.GetItem(templateId);
		DomainManager.World.SetWorldCreationInfo(context, info, inherit: false);
		DomainManager.Map.CreateFixedTutorialArea(context);
		TutorialAreaSize = 20;
		if (templateId == 0)
		{
			Location tutorialLocationByCoordinate = EventHelper.GetTutorialLocationByCoordinate(9, 9);
			MapBlockData block = DomainManager.Map.GetBlock(tutorialLocationByCoordinate);
			block.TemplateId = 103;
			block.InitResources(context.Random);
			DomainManager.Map.GetBlock(tutorialLocationByCoordinate).InitResources(context.Random);
		}
		if (templateId >= 1)
		{
			MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(136);
			SettlementInfo settlementInfo = element_Areas.SettlementInfos[0];
			DomainManager.Taiwu.TryAddVisitedSettlement(settlementInfo.SettlementId, context);
			if (templateId == 2)
			{
				Location tutorialLocationByCoordinate2 = EventHelper.GetTutorialLocationByCoordinate(8, 11);
				MapBlockData block2 = DomainManager.Map.GetBlock(tutorialLocationByCoordinate2);
				block2.CurrResources.Initialize();
				block2.CurrResources.Items[1] = block2.MaxResources.Items[1];
				DomainManager.Map.SetBlockData(context, block2);
			}
		}
		GameData.Domains.Character.Character character = DomainManager.Character.CreateFixedCharacter(context, 444);
		int id = character.GetId();
		DomainManager.Character.CompleteCreatingCharacter(id);
		GameData.Domains.Character.Character character2 = character;
		int num = id;
		GameData.Domains.Character.Character character3 = DomainManager.Character.CreateFixedCharacter(context, 453);
		int id2 = character3.GetId();
		if (!RelationTypeHelper.AllowAddingAdoptiveParentRelation(id, id2))
		{
			throw new Exception($"Failed to add adoptive parent relation: {id} - {id2}");
		}
		DomainManager.Character.AddRelation(context, id, id2, 64);
		DomainManager.Character.DirectlySetFavorabilities(context, id, id2, 30000, 30000);
		DomainManager.Character.CompleteCreatingCharacter(id2);
		if (item.MainCharacter == character3.GetTemplateId())
		{
			character2 = character3;
			num = id2;
		}
		TutorialChaptersItem tutorialChaptersItem = TutorialChapters.Instance[templateId];
		Tuple<string, short>[] fixedCharacters = tutorialChaptersItem.FixedCharacters;
		foreach (Tuple<string, short> tuple in fixedCharacters)
		{
			if (tuple.Item2 == 444)
			{
				_fixedCharacters.Add(tuple.Item1, id);
				continue;
			}
			if (tuple.Item2 == 453)
			{
				_fixedCharacters.Add(tuple.Item1, id2);
				continue;
			}
			short item2 = tuple.Item2;
			byte creatingType = Config.Character.Instance[item2].CreatingType;
			if (1 == 0)
			{
			}
			GameData.Domains.Character.Character character4 = creatingType switch
			{
				0 => DomainManager.Character.CreateFixedCharacter(context, item2), 
				2 => DomainManager.Character.CreateRandomEnemy(context, item2), 
				3 => DomainManager.Character.CreateFixedEnemy(context, item2), 
				_ => throw new ArgumentOutOfRangeException("charTemplateId", $"character with template id {item2} is neither fixed character nor random enemy"), 
			};
			if (1 == 0)
			{
			}
			GameData.Domains.Character.Character character5 = character4;
			int id3 = character5.GetId();
			DomainManager.Character.CompleteCreatingCharacter(id3);
			if (tuple.Item2 == 436)
			{
				AvatarData avatar = character5.GetAvatar();
				avatar.ResetGrowableElementShowingAbility(0);
				character5.SetAvatar(avatar, context);
			}
			_fixedCharacters.Add(tuple.Item1, id3);
		}
		DomainManager.Taiwu.SetTaiwu(context, character2);
		DomainManager.Taiwu.JoinGroup(context, num, showNotification: false);
		short areaId = 136;
		short blockId = ByteCoordinate.CoordinateToIndex(new ByteCoordinate(item.StartBlockCoordinate[0], item.StartBlockCoordinate[1]), TutorialAreaSize);
		Location location = new Location(areaId, blockId);
		character2.SetLocation(location, context);
		DomainManager.Global.OnCurrWorldArchiveDataReady(context, isNewWorld: true);
		return num;
	}

	[DomainMethod]
	public void StartChapter(DataContext context, int chapter)
	{
		_curProgress = 0;
		SetTutorialChapter((short)chapter, context);
		DomainManager.TaiwuEvent.OnEvent_EnterTutorialChapter(_tutorialChapter);
	}

	[DomainMethod]
	public Location GetNextForceMoveToLocation()
	{
		return GetNextForceLocation();
	}

	public void AdvanceProgress(DataContext context)
	{
	}

	public void SpecialAdvanceMonthInTutorial()
	{
		if (_tutorialChapter != 2)
		{
			return;
		}
		Location tutorialLocationByCoordinate = EventHelper.GetTutorialLocationByCoordinate(9, 9);
		List<BuildingBlockData> buildingBlockList = DomainManager.Building.GetBuildingBlockList(tutorialLocationByCoordinate);
		foreach (BuildingBlockData item2 in buildingBlockList)
		{
			if (item2.OperationType == 0 && item2.TemplateId == 258)
			{
				BuildingBlockItem item = BuildingBlock.Instance.GetItem(item2.TemplateId);
				item2.Durability = item.MaxDurability;
			}
		}
	}

	public void SetTutorialEventArgBox(int chapter, EventArgBox argBox)
	{
		TutorialChaptersItem tutorialChaptersItem = TutorialChapters.Instance[chapter - 1];
		Tuple<string, short>[] fixedCharacters = tutorialChaptersItem.FixedCharacters;
		foreach (Tuple<string, short> tuple in fixedCharacters)
		{
			argBox.Set(tuple.Item1, _fixedCharacters[tuple.Item1]);
		}
	}

	public bool IsInTutorialChapter(int chapterIndex)
	{
		return _inGuiding && GetTutorialChapter() == chapterIndex;
	}

	private void OnInitializedDomainData()
	{
		_fixedCharacters = new Dictionary<string, int>();
	}

	private void InitializeOnInitializeGameDataModule()
	{
	}

	private void InitializeOnEnterNewWorld()
	{
		_nextForceLocation = Location.Invalid;
		_canMove = true;
		_canAdvanceMonth = true;
		_canOpenCharacterMenu = true;
		_tutorialChapter = -1;
		_guidVideoName = string.Empty;
	}

	private void OnLoadedArchiveData()
	{
	}

	public TutorialChapterDomain()
		: base(12)
	{
		_curProgress = 0;
		_inGuiding = false;
		_tutorialChapter = 0;
		_canMove = false;
		_canOpenCharacterMenu = false;
		_guidVideoName = string.Empty;
		_canAdvanceMonth = false;
		_nextForceLocation = default(Location);
		_neiliAllocateFitChapter7 = false;
		_huanxinDying = false;
		_huanxinSurprised = false;
		_canShowEnterIndustry = false;
		OnInitializedDomainData();
	}

	public int GetCurProgress()
	{
		return _curProgress;
	}

	public void SetCurProgress(int value, DataContext context)
	{
		_curProgress = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
	}

	public bool GetInGuiding()
	{
		return _inGuiding;
	}

	public void SetInGuiding(bool value, DataContext context)
	{
		_inGuiding = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
	}

	public short GetTutorialChapter()
	{
		return _tutorialChapter;
	}

	public void SetTutorialChapter(short value, DataContext context)
	{
		_tutorialChapter = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, DataStates, CacheInfluences, context);
	}

	public bool GetCanMove()
	{
		return _canMove;
	}

	public void SetCanMove(bool value, DataContext context)
	{
		_canMove = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
	}

	public bool GetCanOpenCharacterMenu()
	{
		return _canOpenCharacterMenu;
	}

	public void SetCanOpenCharacterMenu(bool value, DataContext context)
	{
		_canOpenCharacterMenu = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
	}

	public string GetGuidVideoName()
	{
		return _guidVideoName;
	}

	public void SetGuidVideoName(string value, DataContext context)
	{
		_guidVideoName = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
	}

	public bool GetCanAdvanceMonth()
	{
		return _canAdvanceMonth;
	}

	public void SetCanAdvanceMonth(bool value, DataContext context)
	{
		_canAdvanceMonth = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
	}

	public Location GetNextForceLocation()
	{
		return _nextForceLocation;
	}

	public void SetNextForceLocation(Location value, DataContext context)
	{
		_nextForceLocation = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
	}

	public bool GetNeiliAllocateFitChapter7()
	{
		return _neiliAllocateFitChapter7;
	}

	public void SetNeiliAllocateFitChapter7(bool value, DataContext context)
	{
		_neiliAllocateFitChapter7 = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
	}

	public bool GetHuanxinDying()
	{
		return _huanxinDying;
	}

	public void SetHuanxinDying(bool value, DataContext context)
	{
		_huanxinDying = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, DataStates, CacheInfluences, context);
	}

	public bool GetHuanxinSurprised()
	{
		return _huanxinSurprised;
	}

	public void SetHuanxinSurprised(bool value, DataContext context)
	{
		_huanxinSurprised = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, DataStates, CacheInfluences, context);
	}

	public bool GetCanShowEnterIndustry()
	{
		return _canShowEnterIndustry;
	}

	public void SetCanShowEnterIndustry(bool value, DataContext context)
	{
		_canShowEnterIndustry = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, DataStates, CacheInfluences, context);
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
	}

	public override void OnLoadWorld()
	{
		InitializeInternalDataOfCollections();
		OnLoadedArchiveData();
		DomainManager.Global.CompleteLoading(15);
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
			return GameData.Serializer.Serializer.Serialize(_curProgress, dataPool);
		case 1:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
			}
			return GameData.Serializer.Serializer.Serialize(_inGuiding, dataPool);
		case 2:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 2);
			}
			return GameData.Serializer.Serializer.Serialize(_tutorialChapter, dataPool);
		case 3:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 3);
			}
			return GameData.Serializer.Serializer.Serialize(_canMove, dataPool);
		case 4:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 4);
			}
			return GameData.Serializer.Serializer.Serialize(_canOpenCharacterMenu, dataPool);
		case 5:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 5);
			}
			return GameData.Serializer.Serializer.Serialize(_guidVideoName, dataPool);
		case 6:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 6);
			}
			return GameData.Serializer.Serializer.Serialize(_canAdvanceMonth, dataPool);
		case 7:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 7);
			}
			return GameData.Serializer.Serializer.Serialize(_nextForceLocation, dataPool);
		case 8:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
			}
			return GameData.Serializer.Serializer.Serialize(_neiliAllocateFitChapter7, dataPool);
		case 9:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 9);
			}
			return GameData.Serializer.Serializer.Serialize(_huanxinDying, dataPool);
		case 10:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 10);
			}
			return GameData.Serializer.Serializer.Serialize(_huanxinSurprised, dataPool);
		case 11:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 11);
			}
			return GameData.Serializer.Serializer.Serialize(_canShowEnterIndustry, dataPool);
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		switch (dataId)
		{
		case 0:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _curProgress);
			SetCurProgress(_curProgress, context);
			break;
		case 1:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _inGuiding);
			SetInGuiding(_inGuiding, context);
			break;
		case 2:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _tutorialChapter);
			SetTutorialChapter(_tutorialChapter, context);
			break;
		case 3:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _canMove);
			SetCanMove(_canMove, context);
			break;
		case 4:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _canOpenCharacterMenu);
			SetCanOpenCharacterMenu(_canOpenCharacterMenu, context);
			break;
		case 5:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _guidVideoName);
			SetGuidVideoName(_guidVideoName, context);
			break;
		case 6:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _canAdvanceMonth);
			SetCanAdvanceMonth(_canAdvanceMonth, context);
			break;
		case 7:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _nextForceLocation);
			SetNextForceLocation(_nextForceLocation, context);
			break;
		case 8:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _neiliAllocateFitChapter7);
			SetNeiliAllocateFitChapter7(_neiliAllocateFitChapter7, context);
			break;
		case 9:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _huanxinDying);
			SetHuanxinDying(_huanxinDying, context);
			break;
		case 10:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _huanxinSurprised);
			SetHuanxinSurprised(_huanxinSurprised, context);
			break;
		case 11:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _canShowEnterIndustry);
			SetCanShowEnterIndustry(_canShowEnterIndustry, context);
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
			int argsCount2 = operation.ArgsCount;
			int num2 = argsCount2;
			if (num2 == 2)
			{
				short item2 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item2);
				WorldCreationInfo item3 = default(WorldCreationInfo);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item3);
				int item4 = CreateFixedWorld(context, item2, item3);
				return GameData.Serializer.Serializer.Serialize(item4, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 1:
		{
			int argsCount = operation.ArgsCount;
			int num = argsCount;
			if (num == 1)
			{
				int item = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item);
				StartChapter(context, item);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 2:
			if (operation.ArgsCount == 0)
			{
				Location nextForceMoveToLocation = GetNextForceMoveToLocation();
				return GameData.Serializer.Serializer.Serialize(nextForceMoveToLocation, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
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
		}
		throw new Exception($"Unsupported dataId {dataId}");
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
			return GameData.Serializer.Serializer.Serialize(_curProgress, dataPool);
		case 1:
			if (!BaseGameDataDomain.IsModified(DataStates, 1))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 1);
			return GameData.Serializer.Serializer.Serialize(_inGuiding, dataPool);
		case 2:
			if (!BaseGameDataDomain.IsModified(DataStates, 2))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 2);
			return GameData.Serializer.Serializer.Serialize(_tutorialChapter, dataPool);
		case 3:
			if (!BaseGameDataDomain.IsModified(DataStates, 3))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 3);
			return GameData.Serializer.Serializer.Serialize(_canMove, dataPool);
		case 4:
			if (!BaseGameDataDomain.IsModified(DataStates, 4))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 4);
			return GameData.Serializer.Serializer.Serialize(_canOpenCharacterMenu, dataPool);
		case 5:
			if (!BaseGameDataDomain.IsModified(DataStates, 5))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 5);
			return GameData.Serializer.Serializer.Serialize(_guidVideoName, dataPool);
		case 6:
			if (!BaseGameDataDomain.IsModified(DataStates, 6))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 6);
			return GameData.Serializer.Serializer.Serialize(_canAdvanceMonth, dataPool);
		case 7:
			if (!BaseGameDataDomain.IsModified(DataStates, 7))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 7);
			return GameData.Serializer.Serializer.Serialize(_nextForceLocation, dataPool);
		case 8:
			if (!BaseGameDataDomain.IsModified(DataStates, 8))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 8);
			return GameData.Serializer.Serializer.Serialize(_neiliAllocateFitChapter7, dataPool);
		case 9:
			if (!BaseGameDataDomain.IsModified(DataStates, 9))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 9);
			return GameData.Serializer.Serializer.Serialize(_huanxinDying, dataPool);
		case 10:
			if (!BaseGameDataDomain.IsModified(DataStates, 10))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 10);
			return GameData.Serializer.Serializer.Serialize(_huanxinSurprised, dataPool);
		case 11:
			if (!BaseGameDataDomain.IsModified(DataStates, 11))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 11);
			return GameData.Serializer.Serializer.Serialize(_canShowEnterIndustry, dataPool);
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
			9 => BaseGameDataDomain.IsModified(DataStates, 9), 
			10 => BaseGameDataDomain.IsModified(DataStates, 10), 
			11 => BaseGameDataDomain.IsModified(DataStates, 11), 
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
			throw new Exception($"Cannot invalidate cache state of non-cache data {influence.TargetIndicator.DataId}");
		}
	}

	public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		throw new Exception($"Cannot process archive response of non-archive data {operation.DataId}");
	}

	private void InitializeInternalDataOfCollections()
	{
	}
}
