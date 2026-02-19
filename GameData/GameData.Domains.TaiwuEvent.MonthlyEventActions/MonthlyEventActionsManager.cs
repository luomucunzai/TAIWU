using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions;

[SerializableGameData(NotForDisplayModule = true)]
public class MonthlyEventActionsManager : ISerializableGameData
{
	[SerializableGameDataField]
	private Dictionary<MonthlyActionKey, MonthlyActionBase> _monthlyActions;

	public static readonly Dictionary<string, MonthlyActionKey> PredefinedKeys = new Dictionary<string, MonthlyActionKey>
	{
		{
			"EnemyNestDefault",
			new MonthlyActionKey(1, 0)
		},
		{
			"MartialArtTournamentDefault",
			new MonthlyActionKey(2, 0)
		},
		{
			"BrideOpenContestDefault",
			new MonthlyActionKey(4, 0)
		},
		{
			"SeasonalActionDefault",
			new MonthlyActionKey(5, 0)
		},
		{
			"EmeiStoryDefault",
			new MonthlyActionKey(4, 1)
		},
		{
			"RanshanStoryDefault",
			new MonthlyActionKey(4, 2)
		}
	};

	public readonly List<MonthlyActionBase> CustomMonthlyActionDefines = new List<MonthlyActionBase>();

	public static readonly Dictionary<short, Func<ConfigMonthlyAction, bool>> ConfigTriggerCheckersExtensionMap = new Dictionary<short, Func<ConfigMonthlyAction, bool>> { 
	{
		30,
		(ConfigMonthlyAction action) => DomainManager.World.GetWorldFunctionsStatus(25)
	} };

	public static readonly Dictionary<short, Action<ConfigMonthlyAction>> ConfigMonthlyNotificationHandlerMap = new Dictionary<short, Action<ConfigMonthlyAction>>();

	public static readonly Dictionary<short, Action<ConfigMonthlyAction, bool>> ConfigDeactivateExtensionMap = new Dictionary<short, Action<ConfigMonthlyAction, bool>>
	{
		{ 23, KillMajorCharacter },
		{ 24, KillMajorCharacter },
		{ 25, KillMajorCharacter },
		{ 26, KillMajorCharacter }
	};

	public static readonly Dictionary<short, Predicate<ConfigMonthlyAction>> ConfigIsValidPredicateMap = new Dictionary<short, Predicate<ConfigMonthlyAction>>
	{
		{ 23, MajorCharacterIsNoLongerValid },
		{ 24, MajorCharacterIsNoLongerValid },
		{ 25, MajorCharacterIsNoLongerValid },
		{ 26, MajorCharacterIsNoLongerValid }
	};

	public static readonly Dictionary<short, Action<ConfigMonthlyAction>> ConfigBecomeInvalidHandlerMap = new Dictionary<short, Action<ConfigMonthlyAction>>
	{
		{ 23, RemoveAdventureOnActionBecomeInvalid },
		{ 24, RemoveAdventureOnActionBecomeInvalid },
		{ 25, RemoveAdventureOnActionBecomeInvalid },
		{ 26, RemoveAdventureOnActionBecomeInvalid }
	};

	public static int NewlyActivated;

	public static int NewlyTriggered;

	public bool IsInitialized => _monthlyActions != null && _monthlyActions.Count > 0;

	public MonthlyActionBase GetMonthlyAction(MonthlyActionKey key)
	{
		if (!key.IsValid() || _monthlyActions == null || !_monthlyActions.TryGetValue(key, out var value))
		{
			return null;
		}
		return value;
	}

	public void RemoveTempDynamicAction(MonthlyActionKey key)
	{
		Tester.Assert(key.ActionType == 6);
		_monthlyActions.Remove(key);
	}

	public MonthlyActionKey AddTempDynamicAction<T>(T action) where T : MonthlyActionBase, IDynamicAction
	{
		for (short num = 0; num < short.MaxValue; num++)
		{
			MonthlyActionKey monthlyActionKey = new MonthlyActionKey(6, num);
			if (!_monthlyActions.ContainsKey(monthlyActionKey))
			{
				action.Key = monthlyActionKey;
				_monthlyActions.Add(monthlyActionKey, action);
				return monthlyActionKey;
			}
		}
		return MonthlyActionKey.Invalid;
	}

	public MonthlyActionKey AddWrappedConfigAction(short templateId, short assignedAreaId = -1)
	{
		MonthlyActionKey monthlyActionKey = new MonthlyActionKey(4, templateId);
		if (PredefinedKeys.ContainsValue(monthlyActionKey))
		{
			throw new InvalidOperationException($"Unable to create monthly action because the key {monthlyActionKey} is pre-defined.");
		}
		MonthlyActionBase value;
		ConfigWrapperAction configWrapperAction = (_monthlyActions.TryGetValue(monthlyActionKey, out value) ? ((ConfigWrapperAction)value) : new ConfigWrapperAction(monthlyActionKey));
		configWrapperAction.CreateWrappedAction(templateId, assignedAreaId);
		_monthlyActions[monthlyActionKey] = configWrapperAction;
		return monthlyActionKey;
	}

	public void ClearTaiwuBindingMonthlyActions()
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		List<MonthlyActionKey> list = new List<MonthlyActionKey>(_monthlyActions.Keys);
		foreach (MonthlyActionKey item in list)
		{
			MonthlyActionBase monthlyActionBase = _monthlyActions[item];
			if (monthlyActionBase is MarriageTriggerAction marriageTriggerAction && marriageTriggerAction.Location.IsValid())
			{
				DomainManager.Adventure.RemoveAdventureSite(mainThreadDataContext, marriageTriggerAction.Location.AreaId, marriageTriggerAction.Location.BlockId, isTimeout: false, isComplete: false);
			}
		}
	}

	public void CollectUnreleasedCalledCharacters(HashSet<int> calledCharacters)
	{
		if (!IsInitialized)
		{
			return;
		}
		foreach (var (monthlyActionKey2, monthlyActionBase2) in _monthlyActions)
		{
			monthlyActionBase2.CollectCalledCharacters(calledCharacters);
		}
	}

	public void HandleMonthlyActions()
	{
		NewlyActivated = 0;
		NewlyTriggered = 0;
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		if (!IsInitialized)
		{
			Init();
		}
		List<MonthlyActionBase> list = _monthlyActions.Values.ToList();
		foreach (MonthlyActionBase item in list)
		{
			item.MonthlyHandler();
		}
		stopwatch.Stop();
		AdaptableLog.Info($"HandleMonthlyEventActions ({NewlyTriggered} triggered, {NewlyActivated} activated): {stopwatch.ElapsedMilliseconds} ms");
	}

	public void HandleInvalidActions()
	{
		if (!IsInitialized)
		{
			return;
		}
		foreach (MonthlyActionBase value in _monthlyActions.Values)
		{
			value.ValidationHandler();
		}
	}

	public void Init()
	{
		if (IsInitialized)
		{
			return;
		}
		_monthlyActions = new Dictionary<MonthlyActionKey, MonthlyActionBase>();
		foreach (MonthlyActionsItem item in (IEnumerable<MonthlyActionsItem>)MonthlyActions.Instance)
		{
			if (!item.IsEnemyNest && item.MinInterval > 0)
			{
				ConfigMonthlyAction configMonthlyAction = new ConfigMonthlyAction(item.TemplateId, -1);
				_monthlyActions.Add(configMonthlyAction.Key, configMonthlyAction);
			}
		}
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		MonthlyActionKey key = PredefinedKeys["EnemyNestDefault"];
		EnemyNestMonthlyAction enemyNestMonthlyAction = new EnemyNestMonthlyAction(key);
		enemyNestMonthlyAction.InitializeEnemyNests(mainThreadDataContext.Random);
		_monthlyActions.Add(key, enemyNestMonthlyAction);
		MonthlyActionKey key2 = PredefinedKeys["MartialArtTournamentDefault"];
		_monthlyActions.Add(key2, new MartialArtTournamentMonthlyAction
		{
			Key = key2
		});
		MonthlyActionKey key3 = PredefinedKeys["BrideOpenContestDefault"];
		_monthlyActions.Add(key3, new ConfigWrapperAction(key3));
		MonthlyActionKey key4 = PredefinedKeys["SeasonalActionDefault"];
		_monthlyActions.Add(key4, new SeasonalMonthlyAction(key4));
		MonthlyActionKey key5 = PredefinedKeys["EmeiStoryDefault"];
		_monthlyActions.Add(key5, new ConfigWrapperAction(key5));
		MonthlyActionKey key6 = PredefinedKeys["RanshanStoryDefault"];
		_monthlyActions.Add(key6, new ConfigWrapperAction(key6));
		for (short num = 0; num < CustomMonthlyActionDefines.Count; num++)
		{
			MonthlyActionKey key7 = new MonthlyActionKey(3, num);
			CustomMonthlyActionDefines[num].Key = key7;
			_monthlyActions.Add(key7, CustomMonthlyActionDefines[num]);
		}
	}

	public void OnArchiveDataLoaded()
	{
		if (!IsInitialized)
		{
			return;
		}
		List<short> allKeys = MonthlyActions.Instance.GetAllKeys();
		foreach (MonthlyActionBase value2 in _monthlyActions.Values)
		{
			if (value2 is LegendaryBookMonthlyAction action)
			{
				DomainManager.LegendaryBook.RegisterLegendaryBookMonthlyAction(action);
			}
		}
		for (short num = 0; num < allKeys.Count; num++)
		{
			MonthlyActionKey monthlyActionKey = new MonthlyActionKey(0, num);
			MonthlyActionsItem monthlyActionsItem = MonthlyActions.Instance[num];
			if (!monthlyActionsItem.IsEnemyNest)
			{
				if (monthlyActionsItem.MinInterval <= 0)
				{
					if (_monthlyActions.Remove(monthlyActionKey))
					{
						AdaptableLog.TagWarning("MonthlyEventActionsManager", $"Removing invalid config action {monthlyActionsItem.Name} with key {monthlyActionKey}");
					}
				}
				else if (!_monthlyActions.ContainsKey(monthlyActionKey))
				{
					ConfigMonthlyAction configMonthlyAction = new ConfigMonthlyAction(allKeys[num], -1);
					_monthlyActions.Add(monthlyActionKey, configMonthlyAction);
					AdaptableLog.TagInfo("MonthlyEventActionsManager", "New Config Action: " + configMonthlyAction.ConfigData.Name);
				}
			}
		}
		MonthlyActionKey monthlyActionKey2 = PredefinedKeys["BrideOpenContestDefault"];
		if (!_monthlyActions.ContainsKey(monthlyActionKey2))
		{
			_monthlyActions.Add(monthlyActionKey2, new ConfigWrapperAction(monthlyActionKey2));
			AdaptableLog.TagInfo("MonthlyEventActionsManager", $"New Wrapper Action: {monthlyActionKey2}");
		}
		MonthlyActionKey monthlyActionKey3 = PredefinedKeys["EmeiStoryDefault"];
		if (!_monthlyActions.ContainsKey(monthlyActionKey3))
		{
			_monthlyActions.Add(monthlyActionKey3, new ConfigWrapperAction(monthlyActionKey3));
			AdaptableLog.TagInfo("EmeiStoryDefault", $"New Wrapper Action: {monthlyActionKey3}");
		}
		MonthlyActionKey monthlyActionKey4 = PredefinedKeys["RanshanStoryDefault"];
		if (!_monthlyActions.ContainsKey(monthlyActionKey4))
		{
			_monthlyActions.Add(monthlyActionKey4, new ConfigWrapperAction(monthlyActionKey4));
			AdaptableLog.TagInfo("RanshanStoryDefault", $"New Wrapper Action: {monthlyActionKey4}");
		}
		MonthlyActionKey monthlyActionKey5 = PredefinedKeys["SeasonalActionDefault"];
		if (!_monthlyActions.ContainsKey(monthlyActionKey5))
		{
			_monthlyActions.Add(monthlyActionKey5, new SeasonalMonthlyAction(monthlyActionKey5));
			AdaptableLog.TagInfo("MonthlyEventActionsManager", $"New Seasonal Action: {monthlyActionKey5}");
		}
		for (short num2 = 0; num2 < CustomMonthlyActionDefines.Count; num2++)
		{
			if (CustomMonthlyActionDefines[num2] != null)
			{
				MonthlyActionKey key = new MonthlyActionKey(3, num2);
				if (_monthlyActions.ContainsKey(key))
				{
					_monthlyActions[key].InheritNonArchiveData(CustomMonthlyActionDefines[num2]);
				}
				else
				{
					MonthlyActionBase value = CustomMonthlyActionDefines[num2].CreateCopy();
					_monthlyActions.Add(key, value);
					AdaptableLog.TagInfo("MonthlyEventActionsManager", $"New Custom Monthly Action: {CustomMonthlyActionDefines[num2].Key}");
				}
			}
		}
	}

	public static void OnConfigActionBecomeInvalid(short templateId, ConfigMonthlyAction actionItem)
	{
		if (ConfigBecomeInvalidHandlerMap.TryGetValue(templateId, out var value))
		{
			value(actionItem);
		}
	}

	public static bool ConfigItemTriggerCheck(short templateId, ConfigMonthlyAction actionItem)
	{
		if (ConfigTriggerCheckersExtensionMap.TryGetValue(templateId, out var value))
		{
			return value(actionItem);
		}
		return true;
	}

	[Obsolete]
	public static void HandleConfigItemMonthlyNotification(short templateId, ConfigMonthlyAction actionItem)
	{
	}

	public static void ConfigItemOnActivate(short templateId, ConfigMonthlyAction monthlyAction)
	{
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		monthlyNotificationCollection.AddConfigMonthlyActionNotification(monthlyAction.ConfigData, monthlyAction);
		if (ConfigMonthlyNotificationHandlerMap.TryGetValue(templateId, out var value))
		{
			value(monthlyAction);
		}
		short num = templateId;
		short num2 = num;
		if (num2 == 81)
		{
			DomainManager.Extra.TriggerExtraTask(DomainManager.TaiwuEvent.MainThreadDataContext, 37, 204);
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(DomainManager.TaiwuEvent.MainThreadDataContext, 2, "ConchShip_PresetKey_EmeiAdventureTwoAppearDate", DomainManager.World.GetCurrDate());
		}
	}

	public static void ConfigItemOnDeactivate(short templateId, ConfigMonthlyAction monthlyAction, bool isComplete)
	{
		if (monthlyAction.Key.Equals(PredefinedKeys["BrideOpenContestDefault"]))
		{
			RemoveVeil(monthlyAction, isComplete);
		}
		if (ConfigDeactivateExtensionMap.TryGetValue(templateId, out var value))
		{
			value(monthlyAction, isComplete);
		}
		AdaptableLog.Info($"Deactivating Config Monthly Action: {monthlyAction.ConfigData.Name} at [{monthlyAction.Location}] where isComplete = {isComplete}");
	}

	public static bool CheckConfigActionBecomeInvalid(short templateId, ConfigMonthlyAction monthlyAction)
	{
		Predicate<ConfigMonthlyAction> value;
		return ConfigIsValidPredicateMap.TryGetValue(templateId, out value) && value(monthlyAction);
	}

	private static bool MajorCharacterIsNoLongerValid(ConfigMonthlyAction monthlyAction)
	{
		if (monthlyAction.MajorCharacterSets == null || monthlyAction.MajorCharacterSets.Count == 0)
		{
			return true;
		}
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(monthlyAction.Location.AreaId);
		return !CallCharacterHelper.CheckCalledCharactersStillValid(monthlyAction.ConfigData.MajorTargetFilterList, monthlyAction.MajorCharacterSets, stateTemplateIdByAreaId);
	}

	private static void KillMajorCharacter(ConfigMonthlyAction monthlyAction, bool isComplete)
	{
		if (isComplete || monthlyAction.MajorCharacterSets == null || monthlyAction.MajorCharacterSets.Count == 0)
		{
			return;
		}
		short adventureId = monthlyAction.ConfigData.AdventureId;
		foreach (int item in monthlyAction.MajorCharacterSets[0].GetCollection())
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element))
			{
				DomainManager.Character.MakeCharacterDead(DomainManager.TaiwuEvent.MainThreadDataContext, element, 5, new CharacterDeathInfo(element.GetValidLocation())
				{
					AdventureId = adventureId
				});
			}
		}
	}

	private static void RemoveAdventureOnActionBecomeInvalid(ConfigMonthlyAction monthlyAction)
	{
		if (monthlyAction.ConfigData.IsEnemyNest)
		{
			KillMajorCharacter(monthlyAction, isComplete: false);
		}
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		if (monthlyAction.ConfigData.AdventureId < 0 || !monthlyAction.Location.IsValid())
		{
			monthlyAction.Deactivate(isComplete: false);
		}
		else
		{
			DomainManager.Adventure.RemoveAdventureSite(mainThreadDataContext, monthlyAction.Location.AreaId, monthlyAction.Location.BlockId, isTimeout: true, isComplete: false);
		}
		AdaptableLog.Info($"Removing invalid adventure {Config.Adventure.Instance[monthlyAction.ConfigData.AdventureId]} created by action {monthlyAction.Key} ({monthlyAction.ConfigData.Name})");
	}

	private static void RemoveVeil(ConfigMonthlyAction monthlyAction, bool isComplete)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (taiwu.GetAvatar().ShowVeil)
		{
			AvatarData avatar = taiwu.GetAvatar();
			avatar.ShowVeil = false;
			taiwu.SetAvatar(avatar, DomainManager.TaiwuEvent.MainThreadDataContext);
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		if (_monthlyActions != null)
		{
			num += 2;
			int count = _monthlyActions.Count;
			num += count * 3;
			foreach (MonthlyActionBase value in _monthlyActions.Values)
			{
				num += value.GetSerializedSize();
			}
		}
		else
		{
			num += 2;
		}
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (_monthlyActions != null)
		{
			int count = _monthlyActions.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			foreach (KeyValuePair<MonthlyActionKey, MonthlyActionBase> monthlyAction in _monthlyActions)
			{
				ptr += monthlyAction.Key.Serialize(ptr);
				ptr += monthlyAction.Value.Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (_monthlyActions == null)
			{
				_monthlyActions = new Dictionary<MonthlyActionKey, MonthlyActionBase>(num);
			}
			else
			{
				_monthlyActions.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				MonthlyActionKey key = default(MonthlyActionKey);
				ptr += key.Deserialize(ptr);
				switch (key.ActionType)
				{
				case 0:
				{
					ConfigMonthlyAction configMonthlyAction = new ConfigMonthlyAction();
					ptr += configMonthlyAction.Deserialize(ptr);
					_monthlyActions.Add(key, configMonthlyAction);
					break;
				}
				case 1:
				{
					EnemyNestMonthlyAction enemyNestMonthlyAction = new EnemyNestMonthlyAction();
					ptr += enemyNestMonthlyAction.Deserialize(ptr);
					_monthlyActions.Add(key, enemyNestMonthlyAction);
					break;
				}
				case 2:
				{
					MartialArtTournamentMonthlyAction martialArtTournamentMonthlyAction = new MartialArtTournamentMonthlyAction();
					ptr += martialArtTournamentMonthlyAction.Deserialize(ptr);
					_monthlyActions.Add(key, martialArtTournamentMonthlyAction);
					break;
				}
				case 3:
				{
					MonthlyActionBase monthlyActionBase2 = CustomMonthlyActionDefines[key.Index].CreateCopy();
					ptr += monthlyActionBase2.Deserialize(ptr);
					_monthlyActions.Add(key, monthlyActionBase2);
					break;
				}
				case 4:
				{
					ConfigWrapperAction configWrapperAction = new ConfigWrapperAction();
					ptr += configWrapperAction.Deserialize(ptr);
					_monthlyActions.Add(key, configWrapperAction);
					break;
				}
				case 5:
				{
					SeasonalMonthlyAction seasonalMonthlyAction = new SeasonalMonthlyAction();
					ptr += seasonalMonthlyAction.Deserialize(ptr);
					_monthlyActions.Add(key, seasonalMonthlyAction);
					break;
				}
				case 6:
				{
					short dynamicActionType = *(short*)ptr;
					MonthlyActionBase monthlyActionBase = DynamicActionType.CreateDynamicAction(dynamicActionType);
					ptr += monthlyActionBase.Deserialize(ptr);
					_monthlyActions.Add(key, monthlyActionBase);
					break;
				}
				default:
					throw new Exception("Unrecognized type name.");
				}
			}
		}
		else
		{
			_monthlyActions?.Clear();
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
