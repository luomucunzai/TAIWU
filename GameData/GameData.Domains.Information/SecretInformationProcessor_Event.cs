using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using Config.EventConfig;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.TaiwuEvent.EventOption;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Information;

public sealed class SecretInformationProcessor_Event
{
	private class CombatData
	{
		public sbyte CombatType;

		public bool NoGuard;

		public List<short> CombatResult;

		public short CantFightResult;

		public short CombatConfigId => GetCombatConfigId();

		public CombatData(sbyte combatType, bool noGuard, List<short> combatResult, short cantFightResult = -1)
		{
			CombatType = combatType;
			NoGuard = noGuard;
			CombatResult = combatResult;
			CantFightResult = cantFightResult;
		}

		public CombatData(SecretInformationAppliedResultItem combatConfig, bool hasGuard)
		{
			CombatType = CombatConfig.Instance.GetItem(combatConfig.CombatConfigId).CombatType;
			NoGuard = combatConfig.NoGuard || !NoGuard;
			CombatResult = GetDataListFromConditionResult(combatConfig.SpecialConditionResultIds, 0);
			CantFightResult = GetDataFromConditionResult(combatConfig.SpecialConditionResultIds, 1, 0);
		}

		private short GetCombatConfigId()
		{
			return CombatType switch
			{
				2 => (short)(NoGuard ? 2 : 122), 
				1 => (short)(NoGuard ? 1 : 121), 
				_ => -1, 
			};
		}
	}

	private class ActionData
	{
		public sbyte ActionKey;

		public sbyte Phase;

		public List<short> CombatResult;

		public int SaviorId;

		public int KidnaperId;

		public int PrisonerId;

		public ActionData(sbyte actionKey, sbyte phase, List<short> combatResult, int savorId, int kidnaperId, int prisonerId)
		{
			ActionKey = actionKey;
			Phase = phase;
			CombatResult = combatResult;
			SaviorId = savorId;
			KidnaperId = kidnaperId;
			PrisonerId = prisonerId;
		}
	}

	private class RelationChangeData
	{
		public int SelfCharId;

		public int TargetCharId;

		public ushort RelationType;

		public bool IsServe;

		public bool IsSuccess;

		public RelationChangeData(int selfCharId, int targetCharId, ushort relationType, bool isServe, bool isSuccess)
		{
			SelfCharId = selfCharId;
			TargetCharId = targetCharId;
			RelationType = relationType;
			IsServe = isServe;
			IsSuccess = isSuccess;
		}
	}

	public enum EventAction
	{
		ShowEvent,
		EndEvent,
		StartCombat,
		StartLifeSkillCombat,
		ChooseRope,
		ShowFristContent,
		JumpToOtherEvent
	}

	public static class EventArgKeys_Infomation
	{
		public const string CombatResult = "CombatResult";

		public const string LifeSikllCombatResult = "WinState";

		public const string ResultEventGuid = "resultEventGuid";

		public const string ResultEventGuid_Part2 = "conditionEventGuid";

		public const string BreakCharacterId = "breakTargetCharacterId";

		public const string ActorId = "actorId";

		public const string ReactorId = "reactorId";

		public const string SecactorId = "secactorId";
	}

	private static class LoveRelationValue
	{
		public static int[] MakeEnemyWhenDivorce = new int[5] { 20, 10, 30, 50, 40 };

		public static int[] MakeEnemyWhenNotAdore = new int[5] { 0, 0, 10, 30, 20 };

		public static int[] FavorOfDivorce = new int[5] { -3, -5, -4, -5, -3 };

		public static int[] FavorOfBreakUp = new int[5] { 0, -2, -1, -2, 0 };

		public static int[] FavorOfNotAdore = new int[5] { 1, 0, -1, 0, 1 };

		public const int Adore = 0;

		public const int Lover = 1;

		public const int Spouse = 2;

		public const int BaseFavorChangeOfForgive = -3000;

		public const int BaseHappinessChangeOfForgive = -5;

		public const int BasseFavorChangeOfAskBreak = -3000;

		public const int FavorMultipleChangeOfAskBreak = -1500;

		public const int FavorChangeOfRefuseBreak = -6000;
	}

	public static readonly SecretInformationProcessor_Event Instance = new SecretInformationProcessor_Event();

	private int _metaDataId = -1;

	private SecretInformationProcessor Processor = new SecretInformationProcessor();

	private short ResultIndex = -1;

	private short LastResultIndex = -1;

	private SecretInformationAppliedResultItem _resultConfig;

	private GameData.Domains.Character.Character _taiwu;

	private int _taiwuId;

	private GameData.Domains.Character.Character _character;

	private int _characterId;

	private List<int> _argList = new List<int>();

	private HashSet<(int killerId, int victimId, bool isPublic)> _toKillCharIdTupleHashSet = new HashSet<(int, int, bool)>();

	private HashSet<int> _toEscapeCharIdList = new HashSet<int>();

	private HashSet<(int charId, int metaDataId)> _toDiscardCharIdList = new HashSet<(int, int)>();

	private Dictionary<int, int> _toChangeInfectionCharIdList = new Dictionary<int, int>();

	private Dictionary<int, int> _toChangeHappinessCharIdList = new Dictionary<int, int>();

	private Dictionary<int, Dictionary<int, int>> _toChangeCharacterFavorList = new Dictionary<int, Dictionary<int, int>>();

	private CombatData _savedCombatData = null;

	private ActionData _savedActionData = null;

	private RelationChangeData _savedRelationChangeData = null;

	private short _secretInformationContentId = -1;

	private short _secretInformationStructId = -1;

	private static readonly List<int> AskCharReleaseFavorLimits = new List<int> { 5, 4, 4, 5, 6 };

	private static readonly List<int> AskCharKeepFavorLimits = new List<int> { 6, 5, 4, 5, 6 };

	private short _secretInformationContentIndex = -1;

	private List<short> _savedContentSelections = new List<short>();

	private string _savedContentText = string.Empty;

	private EventAction _eventAction = EventAction.ShowEvent;

	private SecretInformationProcessor_Event()
	{
	}

	public bool Initialize(GameData.Domains.Character.Character character, GameData.Domains.Character.Character taiwu, int metaDataId, EventArgBox argbox)
	{
		Reset();
		if (taiwu == null || character == null)
		{
			return false;
		}
		_metaDataId = metaDataId;
		if (!Processor.Initialize(_metaDataId))
		{
			return false;
		}
		_taiwu = taiwu;
		_character = character;
		_taiwuId = taiwu.GetId();
		_characterId = character.GetId();
		_argList = new List<int>(Processor.GetSecretInformationArgList());
		_argList[3] = character.GetId();
		_argList[5] = taiwu.GetId();
		ResultIndex = -1;
		LastResultIndex = -1;
		List<string> list = new List<string> { "actorId", "reactorId", "secactorId" };
		for (int i = 0; i < 3; i++)
		{
			argbox.Set(list[i], _argList[i]);
		}
		return true;
	}

	public void Reset()
	{
		_metaDataId = -1;
		ResultIndex = -1;
		LastResultIndex = -1;
		_resultConfig = null;
		_argList.Clear();
		_taiwu = null;
		_character = null;
		_taiwuId = -1;
		_characterId = -1;
		_toKillCharIdTupleHashSet.Clear();
		_toEscapeCharIdList.Clear();
		_toDiscardCharIdList.Clear();
		_toChangeHappinessCharIdList.Clear();
		_toChangeInfectionCharIdList.Clear();
		_toChangeCharacterFavorList.Clear();
		_savedCombatData = null;
		_savedActionData = null;
		_savedRelationChangeData = null;
		_savedContentSelections.Clear();
		_secretInformationContentId = -1;
		_secretInformationStructId = -1;
		_secretInformationContentIndex = -1;
		_eventAction = EventAction.ShowEvent;
		Processor.Reset();
	}

	public bool SetEventGuid(string part1, string part2, EventArgBox argbox)
	{
		if (DomainManager.TaiwuEvent.GetEvent(part1) == null || DomainManager.TaiwuEvent.GetEvent(part2) == null)
		{
			return false;
		}
		argbox.Set("resultEventGuid", part1);
		argbox.Set("conditionEventGuid", part2);
		return true;
	}

	public string GetEventGuid(EventArgBox argbox, bool isPart2 = false)
	{
		string key = (isPart2 ? "conditionEventGuid" : "resultEventGuid");
		string arg = string.Empty;
		argbox.Get(key, ref arg);
		return arg;
	}

	public void SetResultIndex(short resultId)
	{
		ResultIndex = resultId;
		LastResultIndex = -1;
	}

	public List<short> GetEventShowData_SelectionKey()
	{
		SecretInformationAppliedResultItem item = SecretInformationAppliedResult.Instance.GetItem(LastResultIndex);
		if (item == null || item.SelectionIds == null)
		{
			return new List<short>();
		}
		return Processor.GetVisibleSelection(item.SelectionIds, _character, _taiwu);
	}

	public TaiwuEventOption[] GetEventShowData_Selection(EventArgBox argBox, GameData.Domains.TaiwuEvent.TaiwuEvent eventData)
	{
		List<short> eventShowData_SelectionKey = GetEventShowData_SelectionKey();
		TaiwuEventOption[] array = MakeSecretInformationSelections(eventShowData_SelectionKey, argBox, eventData);
		bool flag = true;
		TaiwuEventOption[] array2 = array;
		foreach (TaiwuEventOption selection in array2)
		{
			if (CheckInformationSelectionAvailable(selection))
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			eventShowData_SelectionKey.Add(0);
			array = MakeSecretInformationSelections(eventShowData_SelectionKey, argBox, eventData);
		}
		return array;
	}

	public string GetEventShowData_Content()
	{
		SecretInformationAppliedResultItem item = SecretInformationAppliedResult.Instance.GetItem(LastResultIndex);
		if (item == null)
		{
			return $"null Config：{LastResultIndex}";
		}
		if (item.Texts == null)
		{
			return $"null Texts：{LastResultIndex}";
		}
		return string.IsNullOrEmpty(item.Texts[1]) ? item.Texts[0] : item.Texts[_character.GetBehaviorType()];
	}

	public bool GetEventShowData_RevealCharacters()
	{
		SecretInformationAppliedResultItem item = SecretInformationAppliedResult.Instance.GetItem(LastResultIndex);
		if (item == null || item.SelectionIds == null)
		{
			return false;
		}
		return item.RevealCharacters && _taiwu != null && _character != null;
	}

	public int GetEventAction(EventArgBox argBox, out string eventGuid)
	{
		eventGuid = string.Empty;
		if (ResultIndex == -1)
		{
			ApplyEventEnd(argBox);
			return 1;
		}
		while (ResultIndex != -1)
		{
			LastResultIndex = ResultIndex;
			if (!RefreshResultIndex())
			{
				ApplyEventEnd(argBox);
				return 1;
			}
			if (TryGetOutsideJumpEventGuid(out eventGuid))
			{
				if (_resultConfig.EndEventAfterJump)
				{
					ApplyEventEnd(argBox);
				}
				return 6;
			}
			SaveResultCharacterStateChanges();
			SecretInformationMaker_Entrance(argBox);
			ResultIndex = ApplyEventCondition(argBox);
		}
		switch (_eventAction)
		{
		case EventAction.ChooseRope:
			eventGuid = GetEventGuid(argBox, isPart2: true);
			return 6;
		case EventAction.StartCombat:
			if (_savedCombatData != null)
			{
				EventHelper.StartCombat(_characterId, _savedCombatData.CombatConfigId, GetEventGuid(argBox, isPart2: true), argBox, _savedCombatData.NoGuard);
				break;
			}
			ApplyEventEnd(argBox);
			return 1;
		case EventAction.StartLifeSkillCombat:
			EventHelper.StartLifeSkillCombat(_characterId, 16, GetEventGuid(argBox, isPart2: true), argBox);
			break;
		case EventAction.ShowEvent:
			if (LastResultIndex == -1 || GetEventShowData_SelectionKey().Count == 0)
			{
				ApplyEventEnd(argBox);
				return 1;
			}
			break;
		}
		return (int)_eventAction;
	}

	public void ApplyEventEnd(EventArgBox argBox)
	{
		foreach (KeyValuePair<int, Dictionary<int, int>> toChangeCharacterFavor in _toChangeCharacterFavorList)
		{
			foreach (KeyValuePair<int, int> item in toChangeCharacterFavor.Value)
			{
				ChangeFavorability(toChangeCharacterFavor.Key, item.Key, item.Value);
			}
		}
		foreach (KeyValuePair<int, int> toChangeHappinessCharId in _toChangeHappinessCharIdList)
		{
			if (InformationDomain.CheckTuringTest(toChangeHappinessCharId.Key, out var character))
			{
				EventHelper.ChangeRoleHappiness(character, toChangeHappinessCharId.Value);
			}
		}
		foreach (KeyValuePair<int, int> toChangeInfectionCharId in _toChangeInfectionCharIdList)
		{
			if (InformationDomain.CheckTuringTest(toChangeInfectionCharId.Key, out var character2))
			{
				EventHelper.ChangeRoleInfectedValue(character2, toChangeInfectionCharId.Value);
			}
		}
		EventHelper.DisseminateSecretInformationFromTaiwu(_metaDataId, _characterId);
		foreach (var toDiscardCharId in _toDiscardCharIdList)
		{
			DomainManager.Information.DiscardSecretInformation(DomainManager.TaiwuEvent.MainThreadDataContext, toDiscardCharId.charId, toDiscardCharId.metaDataId);
		}
		bool flag = false;
		foreach (var item2 in _toKillCharIdTupleHashSet)
		{
			GameData.Domains.Character.Character element;
			if (item2.victimId == _taiwuId)
			{
				flag = true;
			}
			else if (DomainManager.Character.TryGetElement_Objects(item2.victimId, out element))
			{
				short deathType = (short)(item2.isPublic ? 4 : 3);
				DomainManager.Character.MakeCharacterDead(DomainManager.TaiwuEvent.MainThreadDataContext, element, deathType);
			}
		}
		HashSet<int> hashSet = new HashSet<int>(_toKillCharIdTupleHashSet.Select(((int killerId, int victimId, bool isPublic) tuple) => tuple.victimId));
		foreach (int toEscapeCharId in _toEscapeCharIdList)
		{
			if (!hashSet.Contains(toEscapeCharId) && DomainManager.Character.TryGetElement_Objects(toEscapeCharId, out var element2))
			{
				EventHelper.CharacterEscapeToNearbyBlock(argBox, element2, 1);
			}
		}
		if (flag)
		{
			EventHelper.TriggerLegacyPassingEvent(isTaiwuDying: true);
		}
		Reset();
	}

	private bool RefreshResultIndex()
	{
		_eventAction = EventAction.ShowEvent;
		_resultConfig = SecretInformationAppliedResult.Instance.GetItem(ResultIndex);
		if (_resultConfig == null)
		{
			return false;
		}
		short num = ResultIndex;
		while (true)
		{
			short innerResultEvent = SecretInformationAppliedResult.Instance.GetItem(num).InnerResultEvent;
			if (innerResultEvent != -1)
			{
				num = innerResultEvent;
				continue;
			}
			break;
		}
		if (num != ResultIndex)
		{
			ResultIndex = num;
		}
		_resultConfig = SecretInformationAppliedResult.Instance.GetItem(ResultIndex);
		if (_resultConfig == null)
		{
			return false;
		}
		return true;
	}

	private bool TryGetOutsideJumpEventGuid(out string eventGuid)
	{
		eventGuid = _resultConfig.ResultEventGuid;
		if (string.IsNullOrEmpty(eventGuid))
		{
			return false;
		}
		if (DomainManager.TaiwuEvent.GetEvent(eventGuid) == null)
		{
			return false;
		}
		return true;
	}

	private void SaveResultCharacterStateChanges()
	{
		if (_resultConfig.SelfHappinessDiff != 0)
		{
			if (!_toChangeHappinessCharIdList.ContainsKey(_taiwuId))
			{
				_toChangeHappinessCharIdList.Add(_taiwuId, 0);
			}
			_toChangeHappinessCharIdList[_taiwuId] += _resultConfig.SelfHappinessDiff;
		}
		if (_resultConfig.SelfInfectionDiff != 0)
		{
			if (!_toChangeInfectionCharIdList.ContainsKey(_taiwuId))
			{
				_toChangeInfectionCharIdList.Add(_taiwuId, 0);
			}
			_toChangeInfectionCharIdList[_taiwuId] += _resultConfig.SelfInfectionDiff;
		}
		if (_resultConfig.OppositeHappinessDiff != 0)
		{
			if (!_toChangeHappinessCharIdList.ContainsKey(_characterId))
			{
				_toChangeHappinessCharIdList.Add(_characterId, 0);
			}
			_toChangeHappinessCharIdList[_characterId] += _resultConfig.OppositeHappinessDiff;
		}
		if (_resultConfig.OppositeInfectionDiff != 0)
		{
			if (!_toChangeInfectionCharIdList.ContainsKey(_characterId))
			{
				_toChangeInfectionCharIdList.Add(_characterId, 0);
			}
			_toChangeInfectionCharIdList[_characterId] += _resultConfig.OppositeInfectionDiff;
		}
		if (_resultConfig.SelfFavorabilityDiff != 0)
		{
			if (!_toChangeCharacterFavorList.ContainsKey(_taiwuId))
			{
				_toChangeCharacterFavorList.Add(_taiwuId, new Dictionary<int, int>());
			}
			if (!_toChangeCharacterFavorList[_taiwuId].ContainsKey(_characterId))
			{
				_toChangeCharacterFavorList[_taiwuId].Add(_characterId, 0);
			}
			_toChangeCharacterFavorList[_taiwuId][_characterId] += _resultConfig.SelfFavorabilityDiff;
		}
		if (_resultConfig.OppositeFavorabilityDiff != 0)
		{
			if (!_toChangeCharacterFavorList.ContainsKey(_characterId))
			{
				_toChangeCharacterFavorList.Add(_characterId, new Dictionary<int, int>());
			}
			if (!_toChangeCharacterFavorList[_characterId].ContainsKey(_taiwuId))
			{
				_toChangeCharacterFavorList[_characterId].Add(_taiwuId, 0);
			}
			_toChangeCharacterFavorList[_characterId][_taiwuId] += _resultConfig.OppositeFavorabilityDiff;
		}
	}

	public bool GetSecretInformationEventShowData(EventArgBox argBox, GameData.Domains.TaiwuEvent.TaiwuEvent eventData, out TaiwuEventOption[] options)
	{
		options = null;
		IRandomSource random = DomainManager.TaiwuEvent.MainThreadDataContext.Random;
		short secretInformationAppliedStructs = Processor.GetSecretInformationAppliedStructs(random, _character, _taiwu);
		if (secretInformationAppliedStructs == -1)
		{
			return false;
		}
		_secretInformationStructId = secretInformationAppliedStructs;
		List<short> selectionList;
		short contentIndex;
		short contentIdAndSelections = Processor.GetContentIdAndSelections(random, secretInformationAppliedStructs, _character, _taiwu, out selectionList, out contentIndex);
		if (contentIdAndSelections == -1)
		{
			return false;
		}
		_secretInformationContentId = contentIdAndSelections;
		_secretInformationContentIndex = contentIndex;
		SecretInformationAppliedContentItem item = SecretInformationAppliedContent.Instance.GetItem(contentIdAndSelections);
		if (item.LinkedResult != -1)
		{
			SetResultIndex(item.LinkedResult);
			return true;
		}
		string savedContentText = item.Texts[_character.GetBehaviorType()];
		_savedContentText = savedContentText;
		List<short> visibleSelection = Processor.GetVisibleSelection(selectionList, _character, _taiwu);
		options = MakeSecretInformationSelections(visibleSelection, argBox, eventData);
		bool flag = true;
		TaiwuEventOption[] array = options;
		foreach (TaiwuEventOption selection in array)
		{
			if (CheckInformationSelectionAvailable(selection))
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			visibleSelection.Add(0);
			options = MakeSecretInformationSelections(visibleSelection, argBox, eventData);
		}
		_savedContentSelections = visibleSelection;
		return true;
	}

	public TaiwuEventOption[] GetSavedContentSelection(EventArgBox argBox, GameData.Domains.TaiwuEvent.TaiwuEvent eventData)
	{
		return MakeSecretInformationSelections(_savedContentSelections, argBox, eventData);
	}

	public string GetSavedContentTexs()
	{
		return _savedContentText;
	}

	public short GetFristContentId()
	{
		return _secretInformationContentId;
	}

	public short GetFristStructId()
	{
		return _secretInformationStructId;
	}

	public bool IsContentAskKeepContent()
	{
		return _secretInformationContentIndex == 2;
	}

	private void SecretInformationMaker_Entrance(EventArgBox argBox)
	{
		foreach (Config.ShortList item in _resultConfig.SecretInformation)
		{
			List<short> list = new List<short>(item.DataList);
			if (list.Count >= 2)
			{
				short num = list[0];
				if (num >= 0)
				{
					SecretInformationMaker_Box(argBox, num, _argList[list[1]], (list.Count > 2) ? _argList[list[2]] : (-1), (list.Count > 3) ? _argList[list[3]] : (-1));
				}
			}
		}
	}

	private void SecretInformationMaker_Box(EventArgBox argBox, short infoKey, int actorId, int reactorId, int secactorId)
	{
		switch (infoKey)
		{
		case 1:
			MakeNewInfo_KillInPublic(actorId, reactorId);
			break;
		case 95:
			MakeNewInfo_KillInPrivate(actorId, reactorId);
			break;
		case 3:
			MakeNewInfo_KillForPunishment(actorId, reactorId);
			break;
		case 2:
			MakeNewInfo_KidnapInPublic(argBox, actorId, reactorId);
			break;
		case 96:
			MakeNewInfo_KidnapInPrivate(argBox, actorId, reactorId);
			break;
		case 4:
			MakeNewInfo_KidnapForPunishment(argBox, actorId, reactorId);
			break;
		case 25:
			MakeNewInfo_RescueKidnappedCharacter(actorId, reactorId, secactorId);
			break;
		case 24:
			MakeNewInfo_ReleaseKidnappedCharacter(actorId, reactorId);
			break;
		case 37:
		{
			bool isSuccess7 = MakeNewInfo_BecomeSwornBrothersAndSisters(actorId, reactorId);
			_savedRelationChangeData = new RelationChangeData(actorId, reactorId, 512, isServe: false, isSuccess7);
			break;
		}
		case 38:
		{
			bool isSuccess6 = MakeNewInfo_SeverSwornBrothersAndSisters(actorId, reactorId);
			_savedRelationChangeData = new RelationChangeData(actorId, reactorId, 512, isServe: true, isSuccess6);
			break;
		}
		case 35:
		{
			bool isSuccess5 = MakeNewInfo_BreakupWithLover(actorId, reactorId);
			_savedRelationChangeData = new RelationChangeData(actorId, reactorId, 16384, isServe: true, isSuccess5);
			break;
		}
		case 32:
		{
			bool isSuccess4 = MakeNewInfo_BecomeFriend(actorId, reactorId);
			_savedRelationChangeData = new RelationChangeData(actorId, reactorId, 8192, isServe: false, isSuccess4);
			break;
		}
		case 33:
		{
			bool isSuccess3 = MakeNewInfo_SeverFriend(actorId, reactorId);
			_savedRelationChangeData = new RelationChangeData(actorId, reactorId, 8192, isServe: true, isSuccess3);
			break;
		}
		case 31:
		{
			bool isSuccess2 = MakeNewInfo_BecomeEnemy(actorId, reactorId);
			_savedRelationChangeData = new RelationChangeData(actorId, reactorId, 32768, isServe: false, isSuccess2);
			break;
		}
		case 30:
		{
			bool isSuccess = MakeNewInfo_SeverEnemy(actorId, reactorId);
			_savedRelationChangeData = new RelationChangeData(actorId, reactorId, 32768, isServe: true, isSuccess);
			break;
		}
		}
	}

	private bool MakeNewInfo_KillInPublic(int killerId, int victimId)
	{
		if (MakeNewInfo_ApplyKill(killerId, victimId))
		{
			DomainManager.Information.AddSecretInformationMetaData(DomainManager.TaiwuEvent.MainThreadDataContext, DomainManager.Information.GetSecretInformationCollection().AddKillInPublic(killerId, victimId));
			return true;
		}
		return false;
	}

	private bool MakeNewInfo_KillInPrivate(int killerId, int victimId)
	{
		if (MakeNewInfo_ApplyKill(killerId, victimId, inPublic: false))
		{
			DomainManager.Information.AddSecretInformationMetaData(DomainManager.TaiwuEvent.MainThreadDataContext, DomainManager.Information.GetSecretInformationCollection().AddKillInPrivate(killerId, victimId));
			return true;
		}
		return false;
	}

	private bool MakeNewInfo_KillForPunishment(int killerId, int victimId)
	{
		if (MakeNewInfo_ApplyKill(killerId, victimId))
		{
			DomainManager.Information.AddSecretInformationMetaData(DomainManager.TaiwuEvent.MainThreadDataContext, DomainManager.Information.GetSecretInformationCollection().AddKillForPunishment(killerId, victimId));
			return true;
		}
		return false;
	}

	private bool MakeNewInfo_ApplyKill(int killerId, int victimId, bool inPublic = true)
	{
		if (!DomainManager.Character.TryGetElement_Objects(killerId, out var element) || !DomainManager.Character.TryGetElement_Objects(victimId, out var _))
		{
			return false;
		}
		Location location = element.GetLocation();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		if (inPublic)
		{
			lifeRecordCollection.AddKillInPublic(killerId, currDate, victimId, location);
		}
		else
		{
			lifeRecordCollection.AddKillInPrivate(killerId, currDate, victimId, location);
		}
		_toKillCharIdTupleHashSet.Add((killerId, victimId, inPublic));
		return true;
	}

	private bool MakeNewInfo_KidnapInPublic(EventArgBox argBox, int kidnapperId, int prisonerId)
	{
		if (MakeNewInfo_ApplyKidnap(argBox, kidnapperId, prisonerId))
		{
			DomainManager.Information.AddSecretInformationMetaData(DomainManager.TaiwuEvent.MainThreadDataContext, DomainManager.Information.GetSecretInformationCollection().AddKidnapInPublic(kidnapperId, prisonerId));
			return true;
		}
		return false;
	}

	private bool MakeNewInfo_KidnapInPrivate(EventArgBox argBox, int kidnapperId, int prisonerId)
	{
		if (MakeNewInfo_ApplyKidnap(argBox, kidnapperId, prisonerId, inPublic: false))
		{
			DomainManager.Information.AddSecretInformationMetaData(DomainManager.TaiwuEvent.MainThreadDataContext, DomainManager.Information.GetSecretInformationCollection().AddKidnapInPrivate(kidnapperId, prisonerId));
			return true;
		}
		return false;
	}

	private bool MakeNewInfo_KidnapForPunishment(EventArgBox argBox, int kidnapperId, int prisonerId)
	{
		if (MakeNewInfo_ApplyKidnap(argBox, kidnapperId, prisonerId))
		{
			DomainManager.Information.AddSecretInformationMetaData(DomainManager.TaiwuEvent.MainThreadDataContext, DomainManager.Information.GetSecretInformationCollection().AddKidnapForPunishment(kidnapperId, prisonerId));
			return true;
		}
		return false;
	}

	private bool MakeNewInfo_ApplyKidnap(EventArgBox argBox, int kidnaperId, int prisonerId, bool inPublic = true)
	{
		if (prisonerId == _argList[5] || prisonerId == kidnaperId)
		{
			return false;
		}
		if (!DomainManager.Character.TryGetElement_Objects(kidnaperId, out var element) || !DomainManager.Character.TryGetElement_Objects(prisonerId, out var element2))
		{
			return false;
		}
		int kidnapperId = element2.GetKidnapperId();
		if (kidnapperId == prisonerId)
		{
			return false;
		}
		short num = 73;
		ItemKey itemKey = default(ItemKey);
		bool flag = false;
		if (kidnapperId > 0)
		{
			KidnappedCharacterList someoneKidnapCharacters = DomainManager.Character.GetSomeoneKidnapCharacters(kidnapperId);
			if (someoneKidnapCharacters != null)
			{
				KidnappedCharacter kidnappedCharacter = someoneKidnapCharacters.GetCollection().Find((KidnappedCharacter x) => x.CharId == prisonerId);
				if (kidnappedCharacter != null)
				{
					itemKey = kidnappedCharacter.RopeItemKey;
					num = itemKey.ItemType;
					flag = true;
				}
			}
			DomainManager.Character.RemoveKidnappedCharacter(DomainManager.TaiwuEvent.MainThreadDataContext, prisonerId, kidnapperId, isEscaped: false);
		}
		if (argBox.Get<ItemKey>("ItemKeySeizeCharacterInCombat", out ItemKey arg) && kidnaperId == _argList[5])
		{
			argBox.Remove<ItemKey>("ItemKeySeizeCharacterInCombat");
			num = arg.TemplateId;
			itemKey = arg;
		}
		else
		{
			if (kidnaperId != _taiwuId)
			{
				DomainManager.Character.CombatResultHandle_KidnapEnemy(DomainManager.TaiwuEvent.MainThreadDataContext, element, element2, inPublic);
				return false;
			}
			if (!flag)
			{
				itemKey = DomainManager.Item.CreateItem(DomainManager.TaiwuEvent.MainThreadDataContext, 12, num);
				element.AddInventoryItem(DomainManager.TaiwuEvent.MainThreadDataContext, itemKey, 1);
			}
		}
		int arg2 = -1;
		if (argBox.Get("UseItemKeySeizeCharacterId", ref arg2) && arg2 >= 0 && arg2 != kidnaperId)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(arg2);
			GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(kidnaperId);
			element_Objects.RemoveInventoryItem(DomainManager.TaiwuEvent.MainThreadDataContext, itemKey, 1, deleteItem: false);
			element_Objects2.AddInventoryItem(DomainManager.TaiwuEvent.MainThreadDataContext, itemKey, 1);
		}
		DomainManager.Character.AddKidnappedCharacter(DomainManager.TaiwuEvent.MainThreadDataContext, kidnaperId, prisonerId, itemKey);
		Location location = element.GetLocation();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		if (inPublic)
		{
			lifeRecordCollection.AddKidnapInPublic(kidnaperId, currDate, prisonerId, location, 12, num);
		}
		else
		{
			lifeRecordCollection.AddKidnapInPrivate(kidnaperId, currDate, prisonerId, location, 12, num);
		}
		return true;
	}

	private bool MakeNewInfo_RescueKidnappedCharacter(int saviorId, int prisonerId, int kidnaperId)
	{
		if (MakeNewInfo_ApplyRescue(saviorId, prisonerId, kidnaperId))
		{
			DomainManager.Information.AddSecretInformationMetaData(DomainManager.TaiwuEvent.MainThreadDataContext, DomainManager.Information.GetSecretInformationCollection().AddRescueKidnappedCharacter(saviorId, prisonerId, kidnaperId));
			return true;
		}
		return false;
	}

	private bool MakeNewInfo_ApplyRescue(int saviorId, int prisonerId, int kidnaperId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(saviorId, out var _) || !DomainManager.Character.TryGetElement_Objects(kidnaperId, out var _) || !DomainManager.Character.TryGetElement_Objects(prisonerId, out var element3))
		{
			return false;
		}
		if (element3.GetKidnapperId() != kidnaperId)
		{
			return false;
		}
		DomainManager.Character.RemoveKidnappedCharacter(DomainManager.TaiwuEvent.MainThreadDataContext, prisonerId, kidnaperId, isEscaped: false);
		return true;
	}

	private bool MakeNewInfo_ReleaseKidnappedCharacter(int kidnaperId, int prisonerId)
	{
		if (MakeNewInfo_ApplyRelease(kidnaperId, prisonerId))
		{
			DomainManager.Information.AddSecretInformationMetaData(DomainManager.TaiwuEvent.MainThreadDataContext, DomainManager.Information.GetSecretInformationCollection().AddReleaseKidnappedCharacter(kidnaperId, prisonerId));
			return true;
		}
		return false;
	}

	private bool MakeNewInfo_ApplyRelease(int kidnaperId, int prisonerId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(kidnaperId, out var element) || !DomainManager.Character.TryGetElement_Objects(prisonerId, out var element2))
		{
			return false;
		}
		if (element2.GetKidnapperId() != kidnaperId)
		{
			return false;
		}
		DomainManager.Character.RemoveKidnappedCharacter(DomainManager.TaiwuEvent.MainThreadDataContext, prisonerId, kidnaperId, isEscaped: false);
		Location location = element.GetLocation();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		lifeRecordCollection.AddReleaseKidnappedCharacter(kidnaperId, currDate, prisonerId, location);
		return true;
	}

	private bool MakeNewInfo_BecomeSwornBrothersAndSisters(int selfId, int targetId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(selfId, out var element) || !DomainManager.Character.TryGetElement_Objects(targetId, out var element2))
		{
			return false;
		}
		if (!RelationTypeHelper.AllowAddingSwornBrotherOrSisterRelation(targetId, selfId))
		{
			return false;
		}
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		sbyte behaviorType = element.GetBehaviorType();
		bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
		bool targetIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(targetId);
		GameData.Domains.Character.Character.ApplyBecomeSwornBrotherOrSister(mainThreadDataContext, element, element2, behaviorType, selfIsTaiwuPeople, targetIsTaiwuPeople);
		return true;
	}

	private bool MakeNewInfo_SeverSwornBrothersAndSisters(int selfId, int targetId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(selfId, out var element) || !DomainManager.Character.TryGetElement_Objects(targetId, out var element2))
		{
			return false;
		}
		if (!DomainManager.Character.HasRelation(targetId, selfId, 512))
		{
			return false;
		}
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		sbyte behaviorType = element.GetBehaviorType();
		bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
		bool targetIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(targetId);
		GameData.Domains.Character.Character.ApplySeverSwornBrotherOrSister(mainThreadDataContext, element, element2, behaviorType, selfIsTaiwuPeople, targetIsTaiwuPeople);
		return true;
	}

	private bool MakeNewInfo_BreakupWithLover(int selfId, int targetId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(selfId, out var element) || !DomainManager.Character.TryGetElement_Objects(targetId, out var element2))
		{
			return false;
		}
		if (!DomainManager.Character.HasRelation(targetId, selfId, 16384))
		{
			return false;
		}
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		sbyte behaviorType = element.GetBehaviorType();
		bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
		bool targetIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(targetId);
		GameData.Domains.Character.Character.ApplyBreakupWithBoyOrGirlFriend(mainThreadDataContext, element, element2, behaviorType, targetStillLoveSelf: false, selfIsTaiwuPeople, targetIsTaiwuPeople);
		return true;
	}

	private bool MakeNewInfo_BecomeFriend(int selfId, int targetId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(selfId, out var element) || !DomainManager.Character.TryGetElement_Objects(targetId, out var element2))
		{
			return false;
		}
		if (!RelationTypeHelper.AllowAddingFriendRelation(targetId, selfId))
		{
			return false;
		}
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		sbyte behaviorType = element.GetBehaviorType();
		bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
		bool targetIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(targetId);
		GameData.Domains.Character.Character.ApplyBecomeFriend(mainThreadDataContext, element, element2, behaviorType, selfIsTaiwuPeople, targetIsTaiwuPeople);
		return true;
	}

	private bool MakeNewInfo_SeverFriend(int selfId, int targetId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(selfId, out var element) || !DomainManager.Character.TryGetElement_Objects(targetId, out var element2))
		{
			return false;
		}
		if (!DomainManager.Character.HasRelation(targetId, selfId, 8192))
		{
			return false;
		}
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		sbyte behaviorType = element.GetBehaviorType();
		bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
		bool targetIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(targetId);
		GameData.Domains.Character.Character.ApplySeverFriend(mainThreadDataContext, element, element2, behaviorType, selfIsTaiwuPeople, targetIsTaiwuPeople);
		return true;
	}

	private bool MakeNewInfo_BecomeEnemy(int selfId, int targetId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(selfId, out var element) || !DomainManager.Character.TryGetElement_Objects(targetId, out var element2))
		{
			return false;
		}
		if (!RelationTypeHelper.AllowAddingEnemyRelation(selfId, targetId))
		{
			return false;
		}
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
		GameData.Domains.Character.Character.ApplyAddRelation_Enemy(mainThreadDataContext, element, element2, selfIsTaiwuPeople, 5);
		return true;
	}

	private bool MakeNewInfo_SeverEnemy(int selfId, int targetId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(selfId, out var element) || !DomainManager.Character.TryGetElement_Objects(targetId, out var element2))
		{
			return false;
		}
		if (!DomainManager.Character.HasRelation(selfId, targetId, 32768))
		{
			return false;
		}
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		sbyte behaviorType = element.GetBehaviorType();
		bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
		GameData.Domains.Character.Character.ApplySeverEnemy(mainThreadDataContext, element, element2, behaviorType, selfIsTaiwuPeople);
		return true;
	}

	private void AddStealLifeRecord(int saviorId, int kidnaperId, int prisonerId, sbyte phase)
	{
		if (DomainManager.Character.TryGetElement_Objects(saviorId, out var element) && DomainManager.Character.TryGetElement_Objects(kidnaperId, out var _) && DomainManager.Character.TryGetElement_Objects(prisonerId, out var _))
		{
			Location location = element.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			switch (phase)
			{
			case 0:
				lifeRecordCollection.AddRescueKidnappedCharacterSecretlyFail1(saviorId, currDate, kidnaperId, location, prisonerId);
				break;
			case 1:
				lifeRecordCollection.AddRescueKidnappedCharacterSecretlyFail2(saviorId, currDate, kidnaperId, location, prisonerId);
				break;
			case 2:
				lifeRecordCollection.AddRescueKidnappedCharacterSecretlyFail3(saviorId, currDate, kidnaperId, location, prisonerId);
				break;
			case 3:
				lifeRecordCollection.AddRescueKidnappedCharacterSecretlyFail4(saviorId, currDate, kidnaperId, location, prisonerId);
				break;
			case 4:
				lifeRecordCollection.AddRescueKidnappedCharacterSecretlySucceed(saviorId, currDate, kidnaperId, location, prisonerId);
				break;
			case 5:
				lifeRecordCollection.AddRescueKidnappedCharacterSecretlySucceedAndEscaped(saviorId, currDate, kidnaperId, location, prisonerId);
				break;
			}
		}
	}

	private void AddScamLifeRecord(int saviorId, int kidnaperId, int prisonerId, sbyte phase)
	{
		if (DomainManager.Character.TryGetElement_Objects(saviorId, out var element) && DomainManager.Character.TryGetElement_Objects(kidnaperId, out var _) && DomainManager.Character.TryGetElement_Objects(prisonerId, out var _) && phase < 3)
		{
			Location location = element.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			switch (phase)
			{
			case 0:
				lifeRecordCollection.AddRescueKidnappedCharacterWithWitFail1(saviorId, currDate, kidnaperId, location, prisonerId);
				break;
			case 1:
				lifeRecordCollection.AddRescueKidnappedCharacterWithWitFail2(saviorId, currDate, kidnaperId, location, prisonerId);
				break;
			case 2:
				lifeRecordCollection.AddRescueKidnappedCharacterWithWitFail3(saviorId, currDate, kidnaperId, location, prisonerId);
				break;
			}
		}
	}

	private void AddRobLifeRecord(int saviorId, int kidnaperId, int prisonerId, sbyte phase)
	{
		if (DomainManager.Character.TryGetElement_Objects(saviorId, out var element) && DomainManager.Character.TryGetElement_Objects(kidnaperId, out var _) && DomainManager.Character.TryGetElement_Objects(prisonerId, out var _) && phase < 4)
		{
			Location location = element.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			switch (phase)
			{
			case 0:
				lifeRecordCollection.AddRescueKidnappedCharacterWithForceFail1(saviorId, currDate, kidnaperId, location, prisonerId);
				break;
			case 1:
				lifeRecordCollection.AddRescueKidnappedCharacterWithForceFail2(saviorId, currDate, kidnaperId, location, prisonerId);
				break;
			case 2:
				lifeRecordCollection.AddRescueKidnappedCharacterWithForceFail3(saviorId, currDate, kidnaperId, location, prisonerId);
				break;
			case 3:
				lifeRecordCollection.AddRescueKidnappedCharacterWithForceFail4(saviorId, currDate, kidnaperId, location, prisonerId);
				break;
			}
		}
	}

	private static short GetDataFromConditionResult(List<Config.ShortList> conditionResultIds, int listIndex, int dataIndex)
	{
		short result = -1;
		if (listIndex < 0 || dataIndex < 0)
		{
			return result;
		}
		if (conditionResultIds.Count() <= listIndex)
		{
			return result;
		}
		List<short> dataList = conditionResultIds[listIndex].DataList;
		if (dataList.Count() <= dataIndex)
		{
			return result;
		}
		return dataList[dataIndex];
	}

	private static List<short> GetDataListFromConditionResult(List<Config.ShortList> conditionResultIds, int listIndex)
	{
		List<short> result = new List<short>();
		if (listIndex < 0)
		{
			return result;
		}
		if (conditionResultIds.Count() <= listIndex)
		{
			return result;
		}
		return new List<short>(conditionResultIds[listIndex].DataList);
	}

	private bool Chech_PercentProb(int prob)
	{
		return DomainManager.TaiwuEvent.MainThreadDataContext.Random.Next(0, 100) < prob;
	}

	private bool Check_IfCanFight(GameData.Domains.Character.Character character, sbyte combatType)
	{
		return CombatDomain.GetDefeatMarksCountOutOfCombat(character) <= GlobalConfig.NeedDefeatMarkCount[combatType];
	}

	private bool Check_IfCanPanish(GameData.Domains.Character.Character selfChar, GameData.Domains.Character.Character targetChar)
	{
		sbyte b = selfChar.GetFameType();
		sbyte b2 = targetChar.GetFameType();
		if (b == -2)
		{
			b = 3;
		}
		if (b2 == -2)
		{
			b2 = 3;
		}
		return b2 < 3 && b > 3;
	}

	private bool Check_CharKillTaiwuBecauseOfRefuseKeep(GameData.Domains.Character.Character selfChar, GameData.Domains.Character.Character targetChar, short[] probList)
	{
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(selfChar.GetId(), targetChar.GetId()));
		short num = probList[selfChar.GetBehaviorType()];
		int prob = num * (100 - (favorabilityType - 2) * 20);
		return Chech_PercentProb(prob);
	}

	private bool Check_IsRescure(short lastResult)
	{
		if (lastResult == -1)
		{
			return false;
		}
		SecretInformationAppliedResultItem item = SecretInformationAppliedResult.Instance.GetItem(lastResult);
		return GetDataFromConditionResult(item.SecretInformation, 0, 0) != -1;
	}

	private short ApplyEventCondition(EventArgBox argBox)
	{
		SecretInformationSpecialConditionItem item = SecretInformationSpecialCondition.Instance.GetItem(_resultConfig.SpecialConditionId);
		if (item == null)
		{
			return -1;
		}
		ESecretInformationSpecialConditionCalculate calculate = item.Calculate;
		if (1 == 0)
		{
		}
		short result = calculate switch
		{
			ESecretInformationSpecialConditionCalculate.AskCharKeep => ApplyCondition_AskCharKeep(), 
			ESecretInformationSpecialConditionCalculate.AskCharRelease => ApplyCondition_AskCharRelease(), 
			ESecretInformationSpecialConditionCalculate.TaiwuFight => ApplyCondition_TaiwuFight(), 
			ESecretInformationSpecialConditionCalculate.CharFight => ApplyCondition_ChatFight(), 
			ESecretInformationSpecialConditionCalculate.RefuseKeep => ApplyCondition_RefuseKeep(), 
			ESecretInformationSpecialConditionCalculate.TaiwuEscape => ApplyCondition_TaiwuEscape(), 
			ESecretInformationSpecialConditionCalculate.CharEscape => ApplyCondition_CharEscape(), 
			ESecretInformationSpecialConditionCalculate.TaiwuSteal => ApplyCondition_TaiwuSteal(), 
			ESecretInformationSpecialConditionCalculate.TaiwuScam => ApplyCondition_TaiwuScam(), 
			ESecretInformationSpecialConditionCalculate.TaiwuRob => ApplyCondition_TaiwuRob(), 
			ESecretInformationSpecialConditionCalculate.TaiwuRescueEscape => ApplyCondition_TaiwuRescueEscape(), 
			ESecretInformationSpecialConditionCalculate.CharRescue => ApplyCondition_CharRescue(), 
			ESecretInformationSpecialConditionCalculate.CharRescueEscape => ApplyCondition_CharRescueEscape(), 
			ESecretInformationSpecialConditionCalculate.StartFight => ApplyCondition_StartFight(), 
			ESecretInformationSpecialConditionCalculate.StartLifeSkillCombat => ApplyCondition_StartLifeSkillCombat(), 
			ESecretInformationSpecialConditionCalculate.ChooseRope => ApplyCondition_ChooseRope(), 
			ESecretInformationSpecialConditionCalculate.KidnapWithRope => ApplyCondition_KidnapWithRope(argBox), 
			ESecretInformationSpecialConditionCalculate.CharWin => ApplyCondition_CharWin(argBox), 
			ESecretInformationSpecialConditionCalculate.CharJudge => ApplyCondition_CharJudge(argBox), 
			ESecretInformationSpecialConditionCalculate.CharKidnap => ApplyCondition_CharKidnap(argBox), 
			ESecretInformationSpecialConditionCalculate.TaiwuDeleteInfomation => ApplyCondition_TaiwuDeleteInfomation(), 
			ESecretInformationSpecialConditionCalculate.CharDeleteInfomation => ApplyCondition_CharDeleteInfomation(), 
			ESecretInformationSpecialConditionCalculate.TaiwuAdore => ApplyCondition_TaiwuAdore(), 
			ESecretInformationSpecialConditionCalculate.CharAdore => ApplyCondition_CharAdore(), 
			ESecretInformationSpecialConditionCalculate.Forgive => ApplyCondition_Forgive(argBox), 
			ESecretInformationSpecialConditionCalculate.NotForgiveRape => ApplyCondition_NotForgiveRape(), 
			ESecretInformationSpecialConditionCalculate.AskCharBreakup => ApplyCondition_AskCharBreakup(argBox), 
			ESecretInformationSpecialConditionCalculate.ShowFristContent => ApplyCondition_ShowFristContent(), 
			ESecretInformationSpecialConditionCalculate.BreakupWithChar => ApplyCondition_BreakupWithChar(), 
			ESecretInformationSpecialConditionCalculate.ForceBreakupWithChar => ApplyCondition_ForceBreakupWithChar(), 
			_ => -1, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private short ApplyCondition_ShowFristContent()
	{
		_eventAction = EventAction.ShowFristContent;
		return -1;
	}

	private short ApplyCondition_AskCharKeep()
	{
		int num = AskCharKeepFavorLimits[_character.GetBehaviorType()];
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(_characterId, _taiwuId));
		int dataIndex = 0;
		if (favorabilityType >= num)
		{
			dataIndex = 1;
			short changeValue = Processor.GetSecretInformationEffectConfig().OppositeFavorabilityDiffsWhenResult[_character.GetBehaviorType()];
			EventHelper.ChangeFavorabilityOptional(_character, _taiwu, changeValue, 3);
		}
		return GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 0, dataIndex);
	}

	private short ApplyCondition_AskCharRelease()
	{
		int num = AskCharReleaseFavorLimits[_character.GetBehaviorType()];
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(_characterId, _taiwuId));
		int dataIndex = ((favorabilityType >= num) ? 1 : 0);
		return GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 0, dataIndex);
	}

	private short ApplyCondition_TaiwuFight()
	{
		if (_savedCombatData == null)
		{
			if (_resultConfig.CombatConfigId == -1)
			{
				return -1;
			}
			_savedCombatData = new CombatData(_resultConfig, DomainManager.Character.HasGuard(_characterId, _character));
		}
		if (!Check_IfCanFight(_character, _savedCombatData.CombatType) && _savedCombatData.NoGuard)
		{
			return _savedCombatData.CantFightResult;
		}
		_eventAction = EventAction.StartCombat;
		return -1;
	}

	private short ApplyCondition_ChatFight()
	{
		SecretInformationEffectItem secretInformationEffectConfig = Processor.GetSecretInformationEffectConfig();
		if (secretInformationEffectConfig.CombatType != -1)
		{
			sbyte combatType = CombatConfig.Instance.GetItem(secretInformationEffectConfig.CombatType).CombatType;
			int dataIndex = 0;
			if (combatType == 2)
			{
				dataIndex = (Check_IfCanPanish(_character, _taiwu) ? 1 : 2);
			}
			SecretInformationAppliedResultItem item = SecretInformationAppliedResult.Instance.GetItem(GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 0, dataIndex));
			_savedCombatData = new CombatData(item, DomainManager.Character.HasGuard(_characterId, _character));
			return GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 1, 0);
		}
		return -1;
	}

	private short ApplyCondition_RefuseKeep()
	{
		SecretInformationEffectItem secretInformationEffectConfig = Processor.GetSecretInformationEffectConfig();
		if (Check_CharKillTaiwuBecauseOfRefuseKeep(_character, _taiwu, secretInformationEffectConfig.KillingProbOfRefuseKeepSecret) && secretInformationEffectConfig.CombatType != -1)
		{
			sbyte combatType = CombatConfig.Instance.GetItem(secretInformationEffectConfig.CombatType).CombatType;
			int dataIndex = 0;
			if (combatType == 2)
			{
				dataIndex = 1;
			}
			SecretInformationAppliedResultItem item = SecretInformationAppliedResult.Instance.GetItem(GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 0, dataIndex));
			_savedCombatData = new CombatData(item, DomainManager.Character.HasGuard(_characterId, _character));
			return GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 1, 0);
		}
		return -1;
	}

	private short ApplyCondition_RefuseKeep_Ori()
	{
		SecretInformationEffectItem secretInformationEffectConfig = Processor.GetSecretInformationEffectConfig();
		if (Check_CharKillTaiwuBecauseOfRefuseKeep(_character, _taiwu, secretInformationEffectConfig.KillingProbOfRefuseKeepSecret) && _resultConfig.CombatConfigId != -1)
		{
			_savedCombatData = new CombatData(_resultConfig, DomainManager.Character.HasGuard(_characterId, _character));
			return GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 1, 0);
		}
		return -1;
	}

	private short ApplyCondition_TaiwuEscape()
	{
		_toEscapeCharIdList.Add(_taiwuId);
		return -1;
	}

	private short ApplyCondition_CharEscape()
	{
		_toEscapeCharIdList.Add(_characterId);
		return -1;
	}

	private short ApplyCondition_TaiwuSteal()
	{
		sbyte stealActionPhase = EventHelper.GetStealActionPhase(_taiwu, _character);
		short dataFromConditionResult = GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 0, (stealActionPhase > 4) ? 4 : stealActionPhase);
		short dataFromConditionResult2 = GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 0, 4);
		if (Check_IsRescure(dataFromConditionResult2))
		{
			AddStealLifeRecord(_taiwuId, _characterId, _argList[1], stealActionPhase);
		}
		_savedActionData = new ActionData(1, stealActionPhase, new List<short>(), _taiwuId, _characterId, _argList[1]);
		return dataFromConditionResult;
	}

	private short ApplyCondition_TaiwuScam()
	{
		sbyte scamActionPhase = EventHelper.GetScamActionPhase(_taiwu, _character);
		short dataFromConditionResult = GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 0, (scamActionPhase > 3) ? 3 : scamActionPhase);
		short dataFromConditionResult2 = GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 1, 0);
		if (Check_IsRescure(dataFromConditionResult2))
		{
			AddScamLifeRecord(_taiwuId, _characterId, _argList[1], scamActionPhase);
		}
		_savedActionData = new ActionData(2, scamActionPhase, GetDataListFromConditionResult(_resultConfig.SpecialConditionResultIds, 1), _taiwuId, _characterId, _argList[1]);
		return dataFromConditionResult;
	}

	private short ApplyCondition_TaiwuRob()
	{
		sbyte robActionPhase = EventHelper.GetRobActionPhase(_taiwu, _character);
		short dataFromConditionResult = GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 0, (robActionPhase > 4) ? 4 : robActionPhase);
		short dataFromConditionResult2 = GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 0, 0);
		if (Check_IsRescure(dataFromConditionResult2))
		{
			AddRobLifeRecord(_taiwuId, _characterId, _argList[1], robActionPhase);
		}
		if (_resultConfig.CombatConfigId != -1)
		{
			_savedCombatData = new CombatData(_resultConfig, DomainManager.Character.HasGuard(_characterId, _character));
		}
		_savedActionData = new ActionData(3, robActionPhase, new List<short>(), _taiwuId, _characterId, _argList[1]);
		return dataFromConditionResult;
	}

	private short ApplyCondition_TaiwuRescueEscape()
	{
		short result = -1;
		if (_savedActionData != null)
		{
			if (_savedActionData.Phase == 5)
			{
				result = GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, _savedActionData.ActionKey, 1);
			}
			else
			{
				result = GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, _savedActionData.ActionKey, 0);
				if (_resultConfig.CombatConfigId != -1)
				{
					_savedCombatData = new CombatData(_resultConfig, DomainManager.Character.HasGuard(_characterId, _character));
				}
			}
		}
		_savedActionData = null;
		return result;
	}

	private short ApplyCondition_CharRescue()
	{
		sbyte charRescueAction = GetCharRescueAction(_characterId, _taiwuId, _argList[1]);
		short num = -1;
		switch (charRescueAction)
		{
		case 1:
		{
			sbyte stealActionPhase = EventHelper.GetStealActionPhase(_character, _taiwu);
			short dataFromConditionResult2 = GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 1, 1);
			if (Check_IsRescure(dataFromConditionResult2))
			{
				AddStealLifeRecord(_characterId, _taiwuId, _argList[1], stealActionPhase);
			}
			switch (stealActionPhase)
			{
			case 5:
				num = GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 1, 1);
				_savedActionData = new ActionData(1, stealActionPhase, new List<short>(), _characterId, _taiwuId, _argList[1]);
				break;
			case 4:
			{
				num = GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 1, 0);
				SecretInformationAppliedResultItem item2 = SecretInformationAppliedResult.Instance.GetItem(GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 0, 0));
				if (item2 != null && item2.CombatConfigId != -1)
				{
					_savedCombatData = new CombatData(item2, hasGuard: false);
				}
				_savedActionData = new ActionData(1, stealActionPhase, new List<short>(), _characterId, _taiwuId, _argList[1]);
				break;
			}
			default:
				return -1;
			}
			break;
		}
		case 2:
		{
			sbyte stealActionPhase2 = EventHelper.GetStealActionPhase(_character, _taiwu);
			SecretInformationAppliedResultItem item3 = SecretInformationAppliedResult.Instance.GetItem(GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 0, 1));
			short dataFromConditionResult3 = GetDataFromConditionResult(item3.SpecialConditionResultIds, 0, 1);
			if (Check_IsRescure(dataFromConditionResult3))
			{
				AddScamLifeRecord(_characterId, _taiwuId, _argList[1], stealActionPhase2);
			}
			if (stealActionPhase2 >= 3)
			{
				num = GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 2, 0);
				_savedActionData = new ActionData(2, stealActionPhase2, GetDataListFromConditionResult(item3.SpecialConditionResultIds, 0), _characterId, _taiwuId, _argList[1]);
				break;
			}
			return -1;
		}
		case 3:
		{
			sbyte robActionPhase = EventHelper.GetRobActionPhase(_character, _taiwu);
			SecretInformationAppliedResultItem item = SecretInformationAppliedResult.Instance.GetItem(GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 0, 2));
			short dataFromConditionResult = GetDataFromConditionResult(item.SpecialConditionResultIds, 0, 1);
			if (Check_IsRescure(dataFromConditionResult))
			{
				AddRobLifeRecord(_characterId, _taiwuId, _argList[1], robActionPhase);
			}
			if (robActionPhase >= 4)
			{
				_savedCombatData = new CombatData(item, hasGuard: false);
				num = GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 3, 0);
				_savedActionData = new ActionData(3, robActionPhase, new List<short>(), _characterId, _taiwuId, _argList[1]);
				break;
			}
			return -1;
		}
		default:
			return -1;
		}
		return num;
	}

	private sbyte GetCharRescueAction(int saviorId, int kidnaperId, int prisonerId)
	{
		sbyte b = -1;
		if (!DomainManager.Character.TryGetElement_Objects(saviorId, out var element) || !DomainManager.Character.TryGetElement_Objects(kidnaperId, out var _) || !DomainManager.Character.TryGetElement_Objects(prisonerId, out var _))
		{
			return b;
		}
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(saviorId, prisonerId));
		if (favorabilityType <= 1)
		{
			return b;
		}
		if (!Chech_PercentProb(favorabilityType * 20 - 40))
		{
			return b;
		}
		sbyte behaviorType = element.GetBehaviorType();
		sbyte[] array = AiHelper.PrioritizedActionConstants.RescueFriendOrFamilyActionPriorities[behaviorType];
		sbyte[] array2 = array;
		foreach (sbyte b2 in array2)
		{
			int prob = 60 + AiHelper.DemandActionType.ToPersonalityType[b2];
			if (Chech_PercentProb(prob))
			{
				b = b2;
				break;
			}
		}
		if (b == 3 && !Check_IfCanFight(element, 1))
		{
			b = 0;
		}
		return b;
	}

	private short ApplyCondition_CharRescueEscape()
	{
		short num = -1;
		if (_savedActionData != null)
		{
			if (_savedActionData.Phase == 5)
			{
				num = GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, _savedActionData.ActionKey, 1);
			}
			else
			{
				num = GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, _savedActionData.ActionKey, 0);
				if (_resultConfig.CombatConfigId != -1)
				{
					_savedCombatData = new CombatData(_resultConfig, hasGuard: false);
				}
			}
			_savedActionData = null;
			return num;
		}
		return -1;
	}

	private short ApplyCondition_StartFight()
	{
		_eventAction = EventAction.StartCombat;
		return -1;
	}

	private short ApplyCondition_StartLifeSkillCombat()
	{
		_eventAction = EventAction.StartLifeSkillCombat;
		return -1;
	}

	private short ApplyCondition_ChooseRope()
	{
		_eventAction = EventAction.ChooseRope;
		return -1;
	}

	private short ApplyCondition_CharWin(EventArgBox argBox)
	{
		sbyte combatType = _savedCombatData.CombatType;
		_savedCombatData = null;
		if (GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 2, 0) == -1)
		{
			short infoKey = (short)(GetCharAction_CharWin_InPublic(_character) ? 1 : 95);
			SecretInformationMaker_Box(argBox, infoKey, _characterId, _taiwuId, -1);
			return -1;
		}
		int charAction_CharWin = GetCharAction_CharWin(_character, _taiwu, combatType);
		int dataIndex = ((!GetCharAction_CharWin_InPublic(_character)) ? 1 : 0);
		short dataFromConditionResult = GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, charAction_CharWin, dataIndex);
		if (dataFromConditionResult == -1)
		{
			dataFromConditionResult = GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, charAction_CharWin, 0);
		}
		return dataFromConditionResult;
	}

	private short ApplyCondition_CharJudge(EventArgBox argBox)
	{
		sbyte combatType = _savedCombatData.CombatType;
		_savedCombatData = null;
		if (GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 2, 0) == -1)
		{
			SecretInformationMaker_Box(argBox, 1, _characterId, _taiwuId, -1);
			return -1;
		}
		int charAction_CharWin = GetCharAction_CharWin(_character, _taiwu, combatType);
		return GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, charAction_CharWin, 0);
	}

	private short ApplyCondition_CharKidnap(EventArgBox argBox)
	{
		short infoKey = (short)(GetCharAction_CharWin_InPublic(_character) ? 2 : 96);
		SecretInformationMaker_Box(argBox, infoKey, _characterId, _argList[1], -1);
		return -1;
	}

	private int GetCharAction_CharWin(GameData.Domains.Character.Character selfChar, GameData.Domains.Character.Character targetChar, sbyte combatType, bool enableKidnap = false)
	{
		sbyte b = 60;
		sbyte b2 = 60;
		sbyte b3 = 60;
		if (combatType == 1)
		{
			b = -100;
			b2 = -100;
			b3 = 100;
		}
		sbyte behaviorType = selfChar.GetBehaviorType();
		AiHelper.CombatResultHandleType[] array = AiHelper.NpcCombat.ResultHandleTypePriorities[behaviorType];
		int result = 0;
		for (int i = 0; i < 3; i++)
		{
			switch (array[i])
			{
			case AiHelper.CombatResultHandleType.Kill:
				if (Chech_PercentProb(b + EventHelper.GetRolePersonality(selfChar, 3)))
				{
					result = 1;
				}
				break;
			case AiHelper.CombatResultHandleType.Kidnap:
				if (Chech_PercentProb(b2 + EventHelper.GetRolePersonality(selfChar, 1)))
				{
					result = ((!enableKidnap) ? 1 : 2);
				}
				break;
			case AiHelper.CombatResultHandleType.Release:
				if (Chech_PercentProb(b3 + EventHelper.GetRolePersonality(selfChar, 0)))
				{
					result = 0;
				}
				break;
			}
		}
		return result;
	}

	private bool GetCharAction_CharWin_InPublic(GameData.Domains.Character.Character character)
	{
		sbyte behaviorType = character.GetBehaviorType();
		int prob = AiHelper.NpcCombat.HandleEnemyInPublicChance[behaviorType];
		return Chech_PercentProb(prob);
	}

	private short ApplyCondition_KidnapWithRope(EventArgBox argBox)
	{
		short num = -1;
		sbyte combatType = 1;
		if (_savedActionData != null)
		{
			combatType = _savedCombatData.CombatType;
			_savedCombatData = null;
		}
		if (argBox.Get<ItemKey>("ItemKeySeizeCharacterInCombat", out ItemKey arg) && CombatDomain.CheckRopeHitOutOfCombat(DomainManager.TaiwuEvent.MainThreadDataContext.Random, _taiwu, _character, combatType, useMaxMarkCount: true, ItemTemplateHelper.GetGrade(arg.ItemType, arg.TemplateId)))
		{
			return GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 0, 1);
		}
		return GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 0, 0);
	}

	private short ApplyCondition_TaiwuDeleteInfomation()
	{
		_toDiscardCharIdList.Add((_taiwuId, _metaDataId));
		return -1;
	}

	private short ApplyCondition_CharDeleteInfomation()
	{
		_toDiscardCharIdList.Add((_characterId, _metaDataId));
		return -1;
	}

	private short ApplyCondition_TaiwuAdore()
	{
		bool isSuccess = ApplyRelation_Adore(_taiwuId, _characterId);
		_savedRelationChangeData = new RelationChangeData(_characterId, _taiwuId, 16384, isServe: false, isSuccess);
		return -1;
	}

	private short ApplyCondition_CharAdore()
	{
		bool isSuccess = ApplyRelation_Adore(_characterId, _taiwuId);
		_savedRelationChangeData = new RelationChangeData(_characterId, _taiwuId, 16384, isServe: false, isSuccess);
		return -1;
	}

	private bool ApplyRelation_Adore(int selfId, int targetId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(selfId, out var element) || !DomainManager.Character.TryGetElement_Objects(targetId, out var _))
		{
			return false;
		}
		if (!RelationTypeHelper.AllowAddingAdoredRelation(selfId, targetId))
		{
			return false;
		}
		bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
		bool targetIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(targetId);
		GameData.Domains.Character.Character.ApplyAddRelation_Adore(DomainManager.TaiwuEvent.MainThreadDataContext, element, element, element.GetBehaviorType(), targetLovesBack: false, selfIsTaiwuPeople, targetIsTaiwuPeople);
		return true;
	}

	public bool ApplyRelation_SeverAdore(int selfId, int targetId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(selfId, out var element) || !DomainManager.Character.TryGetElement_Objects(targetId, out var element2))
		{
			return false;
		}
		if (!DomainManager.Character.HasRelation(selfId, targetId, 16384))
		{
			return false;
		}
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		sbyte behaviorType = element.GetBehaviorType();
		bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
		GameData.Domains.Character.Character.ApplySeverAdore(mainThreadDataContext, element, element2, behaviorType, selfIsTaiwuPeople);
		if (DomainManager.Taiwu.GetTaiwuCharId() != selfId && Chech_PercentProb(LoveRelationValue.MakeEnemyWhenNotAdore[behaviorType]))
		{
			GameData.Domains.Character.Character.ApplyAddRelation_Enemy(mainThreadDataContext, element2, element, selfIsTaiwuPeople, 3);
		}
		return true;
	}

	public bool ApplyRelation_SeverSpouse(int selfId, int targetId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(selfId, out var element) || !DomainManager.Character.TryGetElement_Objects(targetId, out var element2))
		{
			return false;
		}
		if (!DomainManager.Character.HasRelation(selfId, targetId, 1024))
		{
			return false;
		}
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		sbyte behaviorType = element.GetBehaviorType();
		bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
		bool targetIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(targetId);
		GameData.Domains.Character.Character.ApplySeverHusbandOrWife(mainThreadDataContext, element, element2, behaviorType, selfIsTaiwuPeople, targetIsTaiwuPeople);
		if (DomainManager.Taiwu.GetTaiwuCharId() != selfId && Chech_PercentProb(LoveRelationValue.MakeEnemyWhenDivorce[behaviorType]))
		{
			GameData.Domains.Character.Character.ApplyAddRelation_Enemy(mainThreadDataContext, element2, element, selfIsTaiwuPeople, 3);
		}
		return true;
	}

	private short ApplyCondition_Forgive(EventArgBox argBox)
	{
		List<int> allSecretInformationRelationsOfCharacter = Processor.GetAllSecretInformationRelationsOfCharacter(_characterId, includeLoveRelation: true, includeLeadership: false);
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		List<int> list4 = new List<int>();
		Dictionary<int, GameData.Domains.Character.Character> activeActorList_WithActorIndex = Processor.GetActiveActorList_WithActorIndex();
		List<int> list5 = new List<int> { 10, 11, 15 };
		List<int> list6 = new List<int> { 13, 14, 15 };
		for (int i = 0; i < 3; i++)
		{
			if (!activeActorList_WithActorIndex.TryGetValue(i, out var value))
			{
				continue;
			}
			if (allSecretInformationRelationsOfCharacter.Contains(list5[i]))
			{
				if (DomainManager.Character.HasRelation(value.GetId(), _characterId, 1024))
				{
					if (!Check_IfForgive(_characterId, value.GetId(), 2) && ApplyRelation_SeverSpouse(_characterId, value.GetId()))
					{
						list.Add(value.GetId());
						continue;
					}
					list2.Add(value.GetId());
					ApplyEffect_Forgive(_character, value, 2);
				}
				else if (DomainManager.Character.HasRelation(_characterId, value.GetId(), 16384))
				{
					if (!Check_IfForgive(_characterId, value.GetId(), 1) && MakeNewInfo_BreakupWithLover(_characterId, value.GetId()))
					{
						list.Add(value.GetId());
						continue;
					}
					list2.Add(value.GetId());
					ApplyEffect_Forgive(_character, value, 1);
				}
			}
			else if (allSecretInformationRelationsOfCharacter.Contains(list6[i]) && DomainManager.Character.HasRelation(_characterId, value.GetId(), 16384))
			{
				if (!Check_IfForgive(_characterId, value.GetId(), 0) && ApplyRelation_SeverAdore(_characterId, value.GetId()))
				{
					list3.Add(value.GetId());
					continue;
				}
				list4.Add(value.GetId());
				ApplyEffect_Forgive(_character, value, 0);
			}
		}
		int num = 0;
		int num2 = 2;
		int dataIndex = 2;
		if (list.Contains(_taiwuId))
		{
			num2 = 0;
			dataIndex = 0;
		}
		else if (list2.Contains(_taiwuId))
		{
			num2 = 0;
			dataIndex = 1;
		}
		else if (list3.Contains(_taiwuId))
		{
			num = 1;
			num2 = 0;
			dataIndex = 0;
		}
		else if (list4.Contains(_taiwuId))
		{
			num = 1;
			num2 = 0;
			dataIndex = 1;
		}
		else if (list.Count != 0)
		{
			dataIndex = 0;
			argBox.Set("breakTargetCharacterId", list[0]);
		}
		else if (list2.Count != 0)
		{
			dataIndex = 1;
		}
		else if (list3.Count != 0)
		{
			num = 1;
			dataIndex = 0;
			argBox.Set("breakTargetCharacterId", list3[0]);
		}
		else if (list4.Count != 0)
		{
			num = 1;
			dataIndex = 1;
		}
		else if (Processor.IsTaiwuSecretInformationActor())
		{
			num2 = 0;
		}
		return GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, num2 + num, dataIndex);
	}

	private short ApplyCondition_NotForgiveRape()
	{
		int listIndex = ((_taiwuId != _argList[0]) ? 1 : 0);
		int num = 2;
		if (DomainManager.Character.HasRelation(_characterId, _argList[0], 1024))
		{
			if (ApplyRelation_SeverSpouse(_characterId, _argList[0]))
			{
				num = 0;
			}
		}
		else if (DomainManager.Character.HasRelation(_characterId, _argList[0], 16384))
		{
			if (DomainManager.Character.HasRelation(_argList[0], _characterId, 16384))
			{
				if (MakeNewInfo_BreakupWithLover(_characterId, _argList[0]))
				{
					num = 0;
				}
			}
			else if (ApplyRelation_SeverAdore(_characterId, _argList[0]))
			{
				num = 1;
			}
		}
		if (num == 2 && _taiwuId == _argList[0] && Processor.ConditionBox(23, _characterId, _taiwuId))
		{
			int dataIndex = 0;
			if (Check_IfCanPanish(_character, _taiwu))
			{
				dataIndex = 1;
			}
			SecretInformationAppliedResultItem item = SecretInformationAppliedResult.Instance.GetItem(GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 2, dataIndex));
			if (item != null)
			{
				_savedCombatData = new CombatData(item, hasGuard: false);
				num = 3;
			}
		}
		return GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, listIndex, num);
	}

	private void ApplyEffect_Forgive(GameData.Domains.Character.Character selfChar, GameData.Domains.Character.Character targetChar, int loveRelationType)
	{
		EventHelper.ChangeRoleHappiness(selfChar, -5);
		short changeValue = (short)(-3000 * (loveRelationType + 1));
		EventHelper.ChangeFavorabilityOptional(selfChar, targetChar, changeValue, 3);
	}

	private short ApplyCondition_AskCharBreakup(EventArgBox argBox)
	{
		int dataIndex = 0;
		List<int> activeActorIdList = Processor.GetActiveActorIdList(new List<int> { _taiwuId, _characterId });
		if (activeActorIdList.Count == 0)
		{
			dataIndex = 2;
		}
		foreach (int item in activeActorIdList)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (DomainManager.Character.HasRelation(item, _characterId, 1024))
			{
				argBox.Set("breakTargetCharacterId", item);
				bool flag = Check_IfBreak(_taiwuId, _characterId, item, 2) && ApplyRelation_SeverSpouse(_characterId, item);
				if (flag)
				{
					dataIndex = 1;
				}
				ApplyFavorChange_Break(_taiwuId, _characterId, item, flag);
				break;
			}
			if (DomainManager.Character.HasRelation(item, _characterId, 16384) && DomainManager.Character.HasRelation(_characterId, item, 16384))
			{
				argBox.Set("breakTargetCharacterId", item);
				bool flag2 = Check_IfBreak(_taiwuId, _characterId, item, 1) && MakeNewInfo_BreakupWithLover(_characterId, item);
				if (flag2)
				{
					dataIndex = 1;
				}
				ApplyFavorChange_Break(_taiwuId, _characterId, item, flag2);
				break;
			}
		}
		return GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, 0, dataIndex);
	}

	private short ApplyCondition_BreakupWithChar()
	{
		int listIndex = 3;
		int dataIndex = 0;
		if (DomainManager.Character.HasRelation(_taiwuId, _characterId, 1024))
		{
			if (Check_IfForgive(_characterId, _taiwuId, 2))
			{
				listIndex = 0;
			}
			else
			{
				listIndex = 1;
				ApplyRelation_SeverSpouse(_taiwuId, _characterId);
			}
		}
		else if (DomainManager.Character.HasRelation(_taiwuId, _characterId, 16384))
		{
			if (DomainManager.Character.HasRelation(_characterId, _taiwuId, 16384))
			{
				if (Check_IfForgive(_characterId, _taiwuId, 1))
				{
					listIndex = 0;
				}
				else
				{
					listIndex = 1;
					MakeNewInfo_BreakupWithLover(_taiwuId, _characterId);
				}
			}
			else
			{
				listIndex = 2;
				ApplyRelation_SeverAdore(_taiwuId, _characterId);
			}
		}
		return GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, listIndex, dataIndex);
	}

	private short ApplyCondition_ForceBreakupWithChar()
	{
		int listIndex = 2;
		int dataIndex = 0;
		if (DomainManager.Character.HasRelation(_taiwuId, _characterId, 1024))
		{
			EventHelper.ChangeFavorabilityOptional(_character, _taiwu, -6000, 3);
			EventHelper.ChangeFavorabilityOptional(_taiwu, _character, -6000, 3);
			if (Check_IfForgive(_characterId, _taiwuId, 2))
			{
				listIndex = 0;
				ApplyRelation_SeverAdore(_taiwuId, _characterId);
				EventHelper.ChangeFavorabilityOptional(_character, _taiwu, -6000, 3);
				EventHelper.ChangeFavorabilityOptional(_taiwu, _character, -6000, 3);
			}
			else
			{
				listIndex = 1;
				ApplyRelation_SeverSpouse(_taiwuId, _characterId);
			}
		}
		else if (DomainManager.Character.HasRelation(_taiwuId, _characterId, 16384) && DomainManager.Character.HasRelation(_characterId, _taiwuId, 16384))
		{
			EventHelper.ChangeFavorabilityOptional(_character, _taiwu, -6000, 3);
			EventHelper.ChangeFavorabilityOptional(_taiwu, _character, -6000, 3);
			if (Check_IfForgive(_characterId, _taiwuId, 2))
			{
				listIndex = 0;
				ApplyRelation_SeverAdore(_taiwuId, _characterId);
				EventHelper.ChangeFavorabilityOptional(_character, _taiwu, -6000, 3);
				EventHelper.ChangeFavorabilityOptional(_taiwu, _character, -6000, 3);
			}
			else
			{
				listIndex = 1;
				MakeNewInfo_BreakupWithLover(_taiwuId, _characterId);
			}
		}
		return GetDataFromConditionResult(_resultConfig.SpecialConditionResultIds, listIndex, dataIndex);
	}

	private bool Check_IfForgive(int selfId, int targetId, int relationIndex)
	{
		if (!DomainManager.Character.TryGetElement_Objects(selfId, out var element) || !DomainManager.Character.TryGetElement_Objects(targetId, out var _))
		{
			return false;
		}
		sbyte behaviorType = element.GetBehaviorType();
		int num = 0;
		switch (relationIndex)
		{
		case 0:
			num = LoveRelationValue.FavorOfNotAdore[behaviorType];
			break;
		case 1:
			num = LoveRelationValue.FavorOfBreakUp[behaviorType];
			break;
		case 2:
			num = LoveRelationValue.FavorOfDivorce[behaviorType];
			break;
		}
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(selfId, targetId));
		return favorabilityType > num;
	}

	private bool Check_IfBreak(int selfId, int targetId, int secTargetId, int loveRelationType)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(targetId);
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(targetId, selfId));
		if (favorabilityType > 0)
		{
			return !Check_IfForgive(targetId, secTargetId, loveRelationType);
		}
		sbyte rolePersonality = EventHelper.GetRolePersonality(element_Objects, 0);
		return Chech_PercentProb(30 + rolePersonality * 3);
	}

	private void ApplyFavorChange_Break(int selfId, int targetId, int secTargetId, bool IfBreak)
	{
		if (DomainManager.Character.TryGetElement_Objects(selfId, out var element) && DomainManager.Character.TryGetElement_Objects(targetId, out var element2) && DomainManager.Character.TryGetElement_Objects(secTargetId, out var _))
		{
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(targetId, secTargetId));
			sbyte b = Math.Max(favorabilityType, 0);
			int num = (-3000 + b * -1500) / 2;
			EventHelper.ChangeFavorabilityOptional(element2, element, (short)num, 3);
			if (IfBreak)
			{
				EventHelper.ChangeRoleHappiness(element2, -(b + 3));
			}
		}
	}

	public static void ChangeFavorability(int charAId, int charBId, int deltaFavor)
	{
		if (charAId != charBId && InformationDomain.CheckTuringTest(charAId, out var character) && InformationDomain.CheckTuringTest(charBId, out var character2))
		{
			int num = Math.Clamp(deltaFavor, -30000, 30000);
			EventHelper.ChangeFavorabilityOptional(character, character2, (short)num, 3);
		}
	}

	public bool GetEventAction_After(EventArgBox argBox)
	{
		short resultIndex = -1;
		bool result = false;
		switch (_eventAction)
		{
		case EventAction.StartCombat:
			resultIndex = DoAfterAction_Combat(argBox);
			break;
		case EventAction.StartLifeSkillCombat:
			resultIndex = DoAfterAction_LifeSkillCombat(argBox);
			break;
		case EventAction.ChooseRope:
			result = true;
			break;
		}
		ResultIndex = resultIndex;
		return result;
	}

	private short DoAfterAction_Combat(EventArgBox argBox)
	{
		if (_savedCombatData == null)
		{
			return -1;
		}
		sbyte arg = -1;
		if (!argBox.Get("CombatResult", ref arg))
		{
			return -1;
		}
		AddLifeRecord_After(CombatResultType.IsPlayerWin(arg));
		if (!_savedCombatData.NoGuard && (arg == 0 || arg == 5))
		{
			arg = 3;
		}
		int avoidDeathCharId = DomainManager.Character.GetAvoidDeathCharId();
		if (_taiwuId == avoidDeathCharId && (arg == 4 || arg == 1))
		{
			arg = 7;
		}
		if (_characterId == avoidDeathCharId && (arg == 5 || arg == 0))
		{
			arg = 8;
		}
		int arg2 = -1;
		if (_savedCombatData.NoGuard && argBox.Get<ItemKey>("ItemKeySeizeCharacterInCombat", out ItemKey _) && argBox.Get("CharIdSeizedInCombat", ref arg2) && arg2 == _characterId)
		{
			arg = 6;
		}
		argBox.Remove<int>("CharIdSeizedInCombat");
		argBox.Remove<int>("CombatResult");
		return _savedCombatData.CombatResult[arg];
	}

	private short DoAfterAction_LifeSkillCombat(EventArgBox argBox)
	{
		if (_savedActionData == null)
		{
			return -1;
		}
		bool arg = false;
		if (!argBox.Get("WinState", ref arg))
		{
			return -1;
		}
		AddLifeRecord_After(arg);
		argBox.Remove<int>("WinState");
		int index = ((!arg) ? 1 : 0);
		return _savedActionData.CombatResult[index];
	}

	private void AddLifeRecord_After(bool taiwuWin, bool isCombat = false)
	{
		if (_savedActionData == null)
		{
			return;
		}
		if (_savedActionData.ActionKey == 2 && !isCombat)
		{
			if (_savedActionData.Phase < 3)
			{
				return;
			}
			Location location = _taiwu.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			if ((_taiwuId == _savedActionData.SaviorId) ? taiwuWin : (!taiwuWin))
			{
				if (_savedActionData.Phase == 5)
				{
					lifeRecordCollection.AddRescueKidnappedCharacterWithWitSucceedAndEscaped(_savedActionData.SaviorId, currDate, _savedActionData.KidnaperId, location, _savedActionData.PrisonerId);
				}
				else
				{
					lifeRecordCollection.AddRescueKidnappedCharacterWithWitSucceed(_savedActionData.SaviorId, currDate, _savedActionData.KidnaperId, location, _savedActionData.PrisonerId);
				}
			}
			else
			{
				lifeRecordCollection.AddRescueKidnappedCharacterWithWitFail4(_savedActionData.SaviorId, currDate, _savedActionData.KidnaperId, location, _savedActionData.PrisonerId);
			}
		}
		else if (_savedActionData.ActionKey == 3 && isCombat && _savedActionData.Phase >= 4)
		{
			Location location2 = _taiwu.GetLocation();
			int currDate2 = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection2 = DomainManager.LifeRecord.GetLifeRecordCollection();
			if ((_taiwuId == _savedActionData.SaviorId) ? taiwuWin : (!taiwuWin))
			{
				lifeRecordCollection2.AddRescueKidnappedCharacterWithForceSucceed(_savedActionData.SaviorId, currDate2, _savedActionData.KidnaperId, location2, _savedActionData.PrisonerId);
			}
			else
			{
				lifeRecordCollection2.AddRescueKidnappedCharacterWithForceFail4(_savedActionData.SaviorId, currDate2, _savedActionData.KidnaperId, location2, _savedActionData.PrisonerId);
			}
		}
	}

	public unsafe TaiwuEventOption[] MakeSecretInformationSelections(List<short> TemplateIdList, EventArgBox argBox, GameData.Domains.TaiwuEvent.TaiwuEvent eventData, string extraKey = "")
	{
		List<TaiwuEventOption> list = new List<TaiwuEventOption>();
		eventData.EventConfig.EscOptionKey = string.Empty;
		foreach (short TemplateId in TemplateIdList)
		{
			SecretInformationAppliedSelectionItem item = SecretInformationAppliedSelection.Instance.GetItem(TemplateId);
			if (item == null)
			{
				continue;
			}
			TaiwuEventOption selection = new TaiwuEventOption();
			selection.GetExtraFormatLanguageKeys = () => (List<string>)null;
			selection.OptionAvailableConditions = new List<TaiwuEventOptionConditionBase>();
			selection.OptionConsumeInfos = new List<OptionConsumeInfo>();
			selection.SetContent("BlankSlection");
			string content = ((item.Text != null) ? item.Text : string.Empty);
			string text = string.Empty;
			selection.ArgBox = argBox;
			short[] playerBehaviorTypeIds = item.PlayerBehaviorTypeIds;
			if (DomainManager.World.GetRestrictOptionsBehaviorType() && playerBehaviorTypeIds != null && playerBehaviorTypeIds.Length != 0)
			{
				selection.OptionAvailableConditions.Add(new OptionConditionBehaviorTypes(15, (sbyte)playerBehaviorTypeIds[0], (sbyte)((playerBehaviorTypeIds.Length > 1) ? ((sbyte)playerBehaviorTypeIds[1]) : (-1)), (sbyte)((playerBehaviorTypeIds.Length > 2) ? ((sbyte)playerBehaviorTypeIds[2]) : (-1)), (sbyte)((playerBehaviorTypeIds.Length > 3) ? ((sbyte)playerBehaviorTypeIds[3]) : (-1)), (sbyte)((playerBehaviorTypeIds.Length > 4) ? ((sbyte)playerBehaviorTypeIds[4]) : (-1)), OptionConditionMatcher.TaiwuIsBehaviorType));
			}
			if (item.TimeCost > 0)
			{
				selection.OptionAvailableConditions.Add(new OptionConditionSbyte(2, item.TimeCost, OptionConditionMatcher.MovePointMore));
				selection.OptionConsumeInfos.Add(new OptionConsumeInfo(8, item.TimeCost, auto: true));
			}
			if (item.FavorabilityCondition > -6)
			{
				selection.OptionAvailableConditions.Add(new OptionConditionFavor(5, item.FavorabilityCondition, OptionConditionMatcher.FavorAtLeast));
			}
			bool flag = false;
			short propertyId = item.MainAttributeCost.PropertyId;
			short value = item.MainAttributeCost.Value;
			if (value > 0 && propertyId >= 0 && propertyId < 6)
			{
				flag = true;
				List<string> list2 = new List<string> { "LK_Main_Attribute_Strength", "LK_Main_Attribute_Dexterity", "LK_Main_Attribute_Concentration", "LK_Main_Attribute_Vitality", "LK_Main_Attribute_Energy", "LK_Main_Attribute_Intelligence" };
				text = $"<color=#darkgrey>[<Language Key=Event_ForSecretInformationAppliedSelection_Cost/><Language Key={list2[propertyId]}/>：{value}]</color>";
			}
			bool flag2 = false;
			short resultId = item.ResultId1;
			if (resultId != -1 && SecretInformationAppliedResult.Instance[resultId].SpecialConditionId == 17)
			{
				flag2 = true;
			}
			if (flag2)
			{
				short num = SecretInformationAppliedResult.Instance[resultId].CombatConfigId;
				if (num == -1)
				{
					num = 1;
				}
				int markLimit = GlobalConfig.NeedDefeatMarkCount[CombatConfig.Instance[num].CombatType];
				if (flag)
				{
					selection.OnOptionAvailableCheck = delegate
					{
						GameData.Domains.Character.Character character = selection.ArgBox.GetCharacter("RoleTaiwu");
						MainAttributes currMainAttributes = character.GetCurrMainAttributes();
						bool flag3 = currMainAttributes.Items[propertyId] >= value;
						bool flag4 = CombatDomain.GetDefeatMarksCountOutOfCombat(character) < markLimit;
						return flag3 && flag4;
					};
				}
				else
				{
					selection.OnOptionAvailableCheck = delegate
					{
						GameData.Domains.Character.Character character = selection.ArgBox.GetCharacter("RoleTaiwu");
						return CombatDomain.GetDefeatMarksCountOutOfCombat(character) < markLimit;
					};
				}
			}
			else if (flag)
			{
				selection.OnOptionAvailableCheck = delegate
				{
					GameData.Domains.Character.Character character = selection.ArgBox.GetCharacter("RoleTaiwu");
					MainAttributes currMainAttributes = character.GetCurrMainAttributes();
					return currMainAttributes.Items[propertyId] >= value;
				};
			}
			if (flag)
			{
				selection.OnOptionSelect = delegate
				{
					GameData.Domains.Character.Character character = selection.ArgBox.GetCharacter("RoleTaiwu");
					EventHelper.ChangeRoleMainAttribute(character, (sbyte)propertyId, -value);
					if (resultId < 0)
					{
						ApplyEventEnd(selection.ArgBox);
						return string.Empty;
					}
					SetResultIndex(resultId);
					return GetEventGuid(selection.ArgBox);
				};
			}
			else
			{
				selection.OnOptionSelect = delegate
				{
					if (resultId < 0)
					{
						ApplyEventEnd(selection.ArgBox);
						return string.Empty;
					}
					SetResultIndex(resultId);
					return GetEventGuid(selection.ArgBox);
				};
			}
			if (!string.IsNullOrEmpty(content) || item.SelectionTexts == null || item.SelectionTexts.Length < 5)
			{
				if (!string.IsNullOrEmpty(text))
				{
					content += text;
				}
				if (flag2)
				{
					short num2 = SecretInformationAppliedResult.Instance[resultId].CombatConfigId;
					if (num2 == -1)
					{
						num2 = 1;
					}
					int markLimit2 = GlobalConfig.NeedDefeatMarkCount[CombatConfig.Instance[num2].CombatType];
					selection.GetReplacedContent = delegate
					{
						string text2 = content;
						GameData.Domains.Character.Character character = selection.ArgBox.GetCharacter("RoleTaiwu");
						if (CombatDomain.GetDefeatMarksCountOutOfCombat(character) >= markLimit2)
						{
							text2 += "<Language Key=Event_ForSecretInformationAppliedSelection_CanNotFight/>";
						}
						return EventHelper.HandleStringTag(text2, selection.ArgBox, eventData);
					};
				}
				else
				{
					selection.GetReplacedContent = () => EventHelper.HandleStringTag(content, selection.ArgBox, eventData);
				}
				selection.OptionKey = $"{extraKey}SecretInformationStandaSelection{TemplateId}{list.Count}";
				list.Add(selection);
			}
			else
			{
				int count = list.Count;
				selection.OptionKey = $"{extraKey}SecretInformationStandaSelection{TemplateId}{list.Count}";
				for (int num3 = 0; num3 < 5; num3++)
				{
					TaiwuEventOption clone = CloneSecretInformationSelection(selection, $"{num3}");
					clone.Behavior = (sbyte)(num3 + 1);
					string content2 = item.SelectionTexts[num3];
					if (!string.IsNullOrEmpty(text))
					{
						content2 += text;
					}
					clone.GetReplacedContent = () => EventHelper.HandleStringTag(content2, clone.ArgBox, eventData);
					list.Add(clone);
				}
			}
			if (item.HotKey == ESecretInformationAppliedSelectionHotKey.Esc)
			{
				if (string.IsNullOrEmpty(eventData.EventConfig.EscOptionKey))
				{
					eventData.EventConfig.EscOptionKey = selection.OptionKey;
				}
				else
				{
					AdaptableLog.Warning("multiple esc selection in secret");
				}
			}
		}
		return list.ToArray();
	}

	public TaiwuEventOption CloneSecretInformationSelection(TaiwuEventOption origin, string extraKey)
	{
		TaiwuEventOption taiwuEventOption = new TaiwuEventOption();
		taiwuEventOption.ArgBox = origin.ArgBox;
		taiwuEventOption.Behavior = origin.Behavior;
		taiwuEventOption.DefaultState = origin.DefaultState;
		taiwuEventOption.GetExtraFormatLanguageKeys = origin.GetExtraFormatLanguageKeys;
		taiwuEventOption.GetReplacedContent = origin.GetReplacedContent;
		taiwuEventOption.OnOptionAvailableCheck = origin.OnOptionAvailableCheck;
		taiwuEventOption.OnOptionSelect = origin.OnOptionSelect;
		taiwuEventOption.OnOptionVisibleCheck = origin.OnOptionVisibleCheck;
		taiwuEventOption.OptionAvailableConditions = origin.OptionAvailableConditions;
		taiwuEventOption.OptionConsumeInfos = origin.OptionConsumeInfos;
		taiwuEventOption.SetContent(origin.OptionContent);
		taiwuEventOption.OptionKey = origin.OptionKey + extraKey;
		return taiwuEventOption;
	}

	public bool CheckInformationSelectionAvailable(TaiwuEventOption selection)
	{
		if (!selection.IsVisible)
		{
			return false;
		}
		if (!selection.IsAvailable)
		{
			return false;
		}
		if (selection.OptionAvailableConditions != null)
		{
			foreach (TaiwuEventOptionConditionBase optionAvailableCondition in selection.OptionAvailableConditions)
			{
				if (!optionAvailableCondition.CheckCondition(selection.ArgBox))
				{
					return false;
				}
			}
		}
		if (selection.OptionConsumeInfos != null)
		{
			int arg = -1;
			int arg2 = -1;
			selection.ArgBox.Get("RoleTaiwu", ref arg);
			selection.ArgBox.Get("CharacterId", ref arg2);
			foreach (OptionConsumeInfo optionConsumeInfo in selection.OptionConsumeInfos)
			{
				if (!optionConsumeInfo.HasConsumeResource(arg, arg2))
				{
					return false;
				}
			}
		}
		return true;
	}
}
