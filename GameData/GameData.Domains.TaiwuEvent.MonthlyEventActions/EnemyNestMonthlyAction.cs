using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Config;
using Config.ConfigCells;
using GameData.Common;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Taiwu;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions;

[SerializableGameData(NotForDisplayModule = true)]
public class EnemyNestMonthlyAction : MonthlyActionBase, ISerializableGameData, IMonthlyActionGroup
{
	[SerializableGameDataField]
	private List<ConfigMonthlyAction>[] _enemyNestActionsInAreas;

	[Obsolete]
	public const sbyte LevelOneNestInitialAmount = 1;

	[Obsolete]
	public const sbyte LevelTwoNestInitialAmount = 1;

	[Obsolete]
	public const sbyte LevelThreeNestInitialAmount = 1;

	[Obsolete]
	public const sbyte MaxUpgradeCount = 9;

	[Obsolete]
	public const sbyte MaxHereticStrongholdCount = 5;

	[Obsolete]
	public const sbyte MaxRighteousStrongholdCount = 3;

	public EnemyNestMonthlyAction(List<short> enemyNestActionTempIds)
	{
		Key = new MonthlyActionKey(1, 0);
		_enemyNestActionsInAreas = new List<ConfigMonthlyAction>[45];
	}

	public EnemyNestMonthlyAction(MonthlyActionKey key)
	{
		Key = key;
		_enemyNestActionsInAreas = new List<ConfigMonthlyAction>[45];
	}

	public void InitializeEnemyNests(IRandomSource random)
	{
		for (short num = 0; num < 45; num++)
		{
			_enemyNestActionsInAreas[num] = new List<ConfigMonthlyAction>();
		}
	}

	public override void MonthlyHandler()
	{
		if (!DomainManager.World.GetWorldFunctionsStatus(7))
		{
			return;
		}
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		DomainManager.Extra.UpdateEnemyNestInitializationDates(mainThreadDataContext, currDate);
		for (short num = 0; num < 45; num++)
		{
			UpdateEnemiesInArea(mainThreadDataContext, num);
			List<ConfigMonthlyAction> list = _enemyNestActionsInAreas[num];
			MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(num);
			MapAreaItem config = element_Areas.GetConfig();
			for (int num2 = list.Count - 1; num2 >= 0; num2--)
			{
				ConfigMonthlyAction configMonthlyAction = list[num2];
				if (configMonthlyAction != null)
				{
					int intervalByAdventureId = GetIntervalByAdventureId(config, num2, configMonthlyAction.ConfigData.AdventureId);
					if (configMonthlyAction.State == 0 && configMonthlyAction.LastFinishDate + intervalByAdventureId <= currDate)
					{
						configMonthlyAction.SelectLocation();
						configMonthlyAction.TriggerAction();
					}
					else if (configMonthlyAction.State == 5 && DomainManager.Adventure.GetAdventureSiteState(num, configMonthlyAction.Location.BlockId) == 1)
					{
						List<CharacterSet> majorCharacterSets = configMonthlyAction.MajorCharacterSets;
						if (majorCharacterSets != null && majorCharacterSets.Count > 0)
						{
							bool flag = false;
							short adventureId = configMonthlyAction.ConfigData.AdventureId;
							foreach (CharacterSet majorCharacterSet in configMonthlyAction.MajorCharacterSets)
							{
								HashSet<int> collection = majorCharacterSet.GetCollection();
								foreach (int item in collection)
								{
									if (!DomainManager.Character.TryGetElement_Objects(item, out var element))
									{
										flag = true;
										continue;
									}
									element.ChangeHealth(mainThreadDataContext, GlobalConfig.Instance.EnemyNestKidnappedCharHealthChange);
									if (element.GetHealth() <= 0)
									{
										flag = true;
										DomainManager.Character.MakeCharacterDead(mainThreadDataContext, element, 5, new CharacterDeathInfo(element.GetValidLocation())
										{
											AdventureId = adventureId
										});
									}
									else if (CheckEscape(mainThreadDataContext.Random, element, adventureId))
									{
										flag = true;
										monthlyNotificationCollection.AddEscapeFromEnemyNest(item, configMonthlyAction.Location, adventureId);
										lifeRecordCollection.AddEscapeFromEnemyNestBySelf(item, currDate, configMonthlyAction.Location, adventureId);
										element.ActivateAdvanceMonthStatus(16);
									}
								}
							}
							if (flag)
							{
								configMonthlyAction.EnsurePrerequisites();
								if (configMonthlyAction.GetCharacterArg() >= 0)
								{
									monthlyNotificationCollection.AddConfigMonthlyActionNotification(configMonthlyAction.ConfigData, configMonthlyAction);
								}
							}
						}
					}
					configMonthlyAction.MonthlyHandler();
				}
			}
		}
	}

	private static int GetIntervalByAdventureId(MapAreaItem mapAreaCfg, int index, short adventureId)
	{
		if (mapAreaCfg.EnemyNests.Count <= index)
		{
			return 0;
		}
		EnemyNestCreationInfo[] array = mapAreaCfg.EnemyNests[index];
		EnemyNestCreationInfo[] array2 = array;
		foreach (EnemyNestCreationInfo enemyNestCreationInfo in array2)
		{
			if (EnemyNest.Instance[enemyNestCreationInfo.EnemyNest].AdventureId == adventureId)
			{
				return enemyNestCreationInfo.Interval;
			}
		}
		return 0;
	}

	private static bool CheckEscape(IRandomSource randomSource, GameData.Domains.Character.Character character, short adventureId)
	{
		sbyte enemyNestTemplateId = AdventureDomain.GetEnemyNestTemplateId(adventureId);
		EnemyNestItem enemyNestItem = EnemyNest.Instance[enemyNestTemplateId];
		CharacterItem characterItem = Config.Character.Instance[enemyNestItem.Leader];
		GameData.Domains.Character.Character pregeneratedRandomEnemy = DomainManager.Character.GetPregeneratedRandomEnemy(characterItem.RandomEnemyId);
		int combatPower = character.GetCombatPower();
		int combatPower2 = pregeneratedRandomEnemy.GetCombatPower();
		int percentProb = combatPower * 100 / combatPower2 - 100;
		return randomSource.CheckPercentProb(percentProb);
	}

	public override void ValidationHandler()
	{
		if (!DomainManager.World.GetWorldFunctionsStatus(7))
		{
			return;
		}
		for (short num = 0; num < 45; num++)
		{
			for (int num2 = _enemyNestActionsInAreas[num].Count - 1; num2 >= 0; num2--)
			{
				ConfigMonthlyAction configMonthlyAction = _enemyNestActionsInAreas[num][num2];
				configMonthlyAction.ValidationHandler();
			}
		}
	}

	public override void FillEventArgBox(EventArgBox eventArgBox)
	{
		eventArgBox.Get<Location>("AdventureLocation", out Location arg);
		if (arg.IsValid())
		{
			ConfigMonthlyAction configAction = GetConfigAction(arg.AreaId, arg.BlockId);
			configAction.EnsurePrerequisites();
			configAction.FillEventArgBox(eventArgBox);
		}
	}

	public override void CollectCalledCharacters(HashSet<int> calledCharacters)
	{
		List<ConfigMonthlyAction>[] enemyNestActionsInAreas = _enemyNestActionsInAreas;
		foreach (List<ConfigMonthlyAction> list in enemyNestActionsInAreas)
		{
			foreach (ConfigMonthlyAction item in list)
			{
				item?.CollectCalledCharacters(calledCharacters);
			}
		}
	}

	private void UpdateEnemiesInArea(DataContext context, short areaId)
	{
		if (_enemyNestActionsInAreas.Length <= areaId)
		{
			return;
		}
		List<ConfigMonthlyAction> list = _enemyNestActionsInAreas[areaId];
		MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(areaId);
		MapAreaItem config = element_Areas.GetConfig();
		if (config.EnemyNests == null)
		{
			return;
		}
		IRandomSource random = context.Random;
		int currDate = DomainManager.World.GetCurrDate();
		int i = 0;
		for (int count = config.EnemyNests.Count; i < count; i++)
		{
			if (list.Count > i)
			{
				ConfigMonthlyAction configMonthlyAction = list[i];
				if (configMonthlyAction != null)
				{
					AdventureSiteData adventureSite = DomainManager.Adventure.GetAdventureSite(configMonthlyAction.Location.AreaId, configMonthlyAction.Location.BlockId);
					if (adventureSite != null && adventureSite.SiteState == 1)
					{
						DomainManager.Adventure.ComplementEnemiesBySite(context, adventureSite, areaId, configMonthlyAction.Location.BlockId);
					}
					continue;
				}
			}
			else
			{
				list.Add(null);
			}
			EnemyNestCreationInfo random2 = config.EnemyNests[i].GetRandom(random);
			EnemyNestItem enemyNestItem = EnemyNest.Instance[random2.EnemyNest];
			int num = Math.Max(DomainManager.Extra.GetEnemyNestInitializationDate(areaId, i), currDate);
			list[i] = new ConfigMonthlyAction(Key, enemyNestItem.MonthlyActionId, areaId)
			{
				LastFinishDate = num - GetIntervalByAdventureId(config, i, enemyNestItem.AdventureId)
			};
		}
	}

	public ConfigMonthlyAction GetConfigAction(short areaId, short blockId)
	{
		foreach (ConfigMonthlyAction item in _enemyNestActionsInAreas[areaId])
		{
			if (item != null && item.Location.BlockId == blockId)
			{
				return item;
			}
		}
		return null;
	}

	public void DeactivateSubAction(short areaId, short blockId, bool isComplete)
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(areaId);
		MapAreaItem config = element_Areas.GetConfig();
		List<ConfigMonthlyAction> list = _enemyNestActionsInAreas[areaId];
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			ConfigMonthlyAction configMonthlyAction = list[i];
			if (configMonthlyAction != null && configMonthlyAction.Location.BlockId == blockId)
			{
				VillagerWorkData villagerMapWorkData = DomainManager.Taiwu.GetVillagerMapWorkData(areaId, blockId, 11);
				if (villagerMapWorkData != null)
				{
					DomainManager.Taiwu.RemoveVillagerWork(mainThreadDataContext, villagerMapWorkData.CharacterId);
					Location location = configMonthlyAction.Location;
					short adventureId = configMonthlyAction.ConfigData.AdventureId;
					InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
					instantNotificationCollection.AddTheNestOfRegulationDies(location, adventureId, villagerMapWorkData.CharacterId);
				}
				configMonthlyAction.Deactivate(isComplete);
				if (config.EnemyNests.CheckIndex(i))
				{
					EnemyNestCreationInfo random = config.EnemyNests[i].GetRandom(mainThreadDataContext.Random);
					EnemyNestItem enemyNestItem = EnemyNest.Instance[random.EnemyNest];
					list[i] = new ConfigMonthlyAction(Key, enemyNestItem.MonthlyActionId, areaId)
					{
						LastFinishDate = DomainManager.World.GetCurrDate()
					};
				}
				else
				{
					list.RemoveAt(i);
				}
				break;
			}
		}
	}

	internal void ResetIntervals()
	{
		List<ConfigMonthlyAction>[] enemyNestActionsInAreas = _enemyNestActionsInAreas;
		foreach (List<ConfigMonthlyAction> list in enemyNestActionsInAreas)
		{
			foreach (ConfigMonthlyAction item in list)
			{
				if (item != null && item.State == 0)
				{
					item.LastFinishDate = int.MinValue;
				}
			}
		}
	}

	public override MonthlyActionBase CreateCopy()
	{
		return GameData.Serializer.Serializer.CreateCopy(this);
	}

	public void Test_LogEnemyNestsLargeBase(IRandomSource random, int iterations)
	{
		string[] names = EnemyNest.Instance.Select((EnemyNestItem nest) => Config.Adventure.Instance[nest.AdventureId].Name).ToArray();
		short[] values = EnemyNest.Instance.GetAllKeys().ToArray();
		AccurateHistogram<short> accurateHistogram = new AccurateHistogram<short>(names, values);
		for (int num = 0; num < iterations; num++)
		{
			for (short num2 = 0; num2 < 45; num2++)
			{
				MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(num2);
				MapAreaItem config = element_Areas.GetConfig();
				_enemyNestActionsInAreas[num2] = new List<ConfigMonthlyAction>();
				if (config.EnemyNests != null)
				{
					foreach (EnemyNestCreationInfo[] enemyNest in config.EnemyNests)
					{
						EnemyNestCreationInfo random2 = enemyNest.GetRandom(random);
						accurateHistogram.Record(random2.EnemyNest);
					}
				}
			}
		}
		AdaptableLog.Info("\n" + accurateHistogram.GetTextGraph());
	}

	public void Test_LogEnemyNests()
	{
		StringBuilder stringBuilder = new StringBuilder();
		Dictionary<short, int> dictionary = new Dictionary<short, int>();
		for (short num = 0; num < 45; num++)
		{
			MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(num);
			MapAreaItem config = element_Areas.GetConfig();
			List<ConfigMonthlyAction> list = _enemyNestActionsInAreas[num];
			if (list != null)
			{
				stringBuilder.Append(config.Name);
				stringBuilder.Append(',');
				foreach (ConfigMonthlyAction item in list)
				{
					short adventureArg = item.GetAdventureArg();
					AdventureItem adventureItem = Config.Adventure.Instance[adventureArg];
					stringBuilder.Append(adventureItem.Name);
					stringBuilder.Append(',');
					if (!dictionary.TryAdd(adventureArg, 1))
					{
						dictionary[adventureArg]++;
					}
				}
				stringBuilder.AppendLine();
			}
		}
		AdaptableLog.Info(stringBuilder.ToString());
		stringBuilder.Clear();
		foreach (EnemyNestItem item2 in (IEnumerable<EnemyNestItem>)EnemyNest.Instance)
		{
			AdventureItem adventureItem2 = Config.Adventure.Instance[item2.AdventureId];
			int orDefault = dictionary.GetOrDefault(item2.AdventureId);
			stringBuilder.AppendFormat("{0}: {1}\n", adventureItem2.Name, orDefault.ToString());
		}
		AdaptableLog.Info(stringBuilder.ToString());
	}

	public EnemyNestMonthlyAction()
	{
	}

	public override bool IsSerializedSizeFixed()
	{
		return false;
	}

	public override int GetSerializedSize()
	{
		int num = 12;
		if (_enemyNestActionsInAreas != null)
		{
			num += 2;
			List<ConfigMonthlyAction>[] enemyNestActionsInAreas = _enemyNestActionsInAreas;
			foreach (List<ConfigMonthlyAction> list in enemyNestActionsInAreas)
			{
				num += 2;
				foreach (ConfigMonthlyAction item in list)
				{
					num += item.GetSerializedSize();
				}
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
		if (_enemyNestActionsInAreas != null)
		{
			int num = _enemyNestActionsInAreas.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			List<ConfigMonthlyAction>[] enemyNestActionsInAreas = _enemyNestActionsInAreas;
			foreach (List<ConfigMonthlyAction> list in enemyNestActionsInAreas)
			{
				*(ushort*)ptr = (ushort)list.Count;
				ptr += 2;
				foreach (ConfigMonthlyAction item in list)
				{
					ptr += item.Serialize(ptr);
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += Key.Serialize(ptr);
		*ptr = (byte)State;
		ptr++;
		*(int*)ptr = Month;
		ptr += 4;
		*(int*)ptr = LastFinishDate;
		ptr += 4;
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (_enemyNestActionsInAreas == null || _enemyNestActionsInAreas.Length != num)
		{
			_enemyNestActionsInAreas = new List<ConfigMonthlyAction>[num];
		}
		for (int i = 0; i < num; i++)
		{
			if (_enemyNestActionsInAreas[i] == null)
			{
				_enemyNestActionsInAreas[i] = new List<ConfigMonthlyAction>();
			}
			else
			{
				_enemyNestActionsInAreas[i].Clear();
			}
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			for (int j = 0; j < num2; j++)
			{
				ConfigMonthlyAction configMonthlyAction = new ConfigMonthlyAction();
				ptr += configMonthlyAction.Deserialize(ptr);
				_enemyNestActionsInAreas[i].Add(configMonthlyAction);
			}
		}
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

	[Obsolete]
	public ConfigMonthlyAction CreateNewConfigAction(short templateId, short areaId)
	{
		ConfigMonthlyAction configMonthlyAction = new ConfigMonthlyAction(templateId, areaId);
		configMonthlyAction.Key = Key;
		_enemyNestActionsInAreas[areaId].Add(configMonthlyAction);
		return configMonthlyAction;
	}

	[Obsolete("EnemyNest can no longer be upgraded")]
	public static bool UpgradeEnemyNest(DataContext context, ConfigMonthlyAction enemyNestAction, short curLevel)
	{
		if (curLevel >= EnemyNest.Instance.Count - 1)
		{
			return false;
		}
		EnemyNestItem enemyNestItem = EnemyNest.Instance[curLevel + 1];
		if (enemyNestItem.MonthlyActionId < 0)
		{
			return false;
		}
		enemyNestAction.State = 1;
		enemyNestAction.Month = 0;
		enemyNestAction.ClearCalledCharacters();
		enemyNestAction.ConfigTemplateId = enemyNestItem.MonthlyActionId;
		DomainManager.Adventure.UpgradeEnemyNest(context, enemyNestAction.Location.AreaId, enemyNestAction.Location.BlockId, enemyNestItem.AdventureId);
		return true;
	}

	[Obsolete("Use IMonthlyActionGroup.DeactivateSubAction instead.")]
	public void DeactivateEnemyNest(short areaId, short blockId, bool isComplete)
	{
		DeactivateSubAction(areaId, blockId, isComplete);
	}

	[Obsolete("EnemyNest can no longer be upgraded")]
	public void GetUpgradableEnemyNestsInArea(short areaId, List<short> result)
	{
		result.Clear();
		AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(areaId);
		foreach (ConfigMonthlyAction item in _enemyNestActionsInAreas[areaId])
		{
			AdventureItem adventureItem = Config.Adventure.Instance[item.ConfigData.AdventureId];
			if (DomainManager.Adventure.TryGetElement_EnemyNestSites(item.Location, out var value) && EnemyNestConstValues.HereticStrongholdNestIds.Exist(value.InitialNestTemplateId) && (!adventuresInArea.AdventureSites.TryGetValue(item.Location.BlockId, out var value2) || value2.SiteState == 1))
			{
				sbyte enemyNestTemplateId = AdventureDomain.GetEnemyNestTemplateId(adventureItem.TemplateId);
				if (enemyNestTemplateId != EnemyNestConstValues.HereticStrongholdNestIds[^1])
				{
					result.Add(item.Location.BlockId);
				}
			}
		}
	}
}
