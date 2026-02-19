using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Domains.Adventure;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions;

[SerializableGameData(NotForDisplayModule = true)]
public class SeasonalMonthlyAction : MonthlyActionBase, IMonthlyActionGroup, ISerializableGameData
{
	private const int CombatSkillCompetitionRate = 20;

	private const int MinCountPerState = 3;

	private const int MaxCountPerState = 5;

	[SerializableGameDataField]
	private int _prevSpringDate;

	[SerializableGameDataField]
	private int _prevSummerDate;

	[SerializableGameDataField]
	private int _prevAutumnDate;

	[SerializableGameDataField]
	private int _prevWinterDate;

	[SerializableGameDataField]
	private Dictionary<Location, ConfigMonthlyAction> _monthlyActions;

	public SeasonalMonthlyAction(MonthlyActionKey key)
	{
		Key = key;
		_prevSpringDate = 0;
		_prevSummerDate = 0;
		_prevAutumnDate = 0;
		_prevWinterDate = 0;
		_monthlyActions = new Dictionary<Location, ConfigMonthlyAction>();
	}

	public override void TriggerAction()
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		switch (DomainManager.World.GetCurrMonthInYear())
		{
		case 0:
			DomainManager.World.GetMonthlyNotificationCollection().AddMarketComing(1);
			break;
		case 1:
			CreateSpringMarkets(mainThreadDataContext);
			break;
		case 2:
			CreateSummerCombatSkillCompetitions(mainThreadDataContext);
			break;
		case 4:
			CreateAutumnCricketConference(mainThreadDataContext);
			break;
		case 8:
			CreateWinterLifeSkillCompetitions(mainThreadDataContext);
			break;
		case 11:
			DomainManager.World.GetMonthlyNotificationCollection().AddMarketComing(2);
			break;
		}
	}

	public override void MonthlyHandler()
	{
		if (DomainManager.World.GetMainStoryLineProgress() < 16)
		{
			return;
		}
		TriggerAction();
		foreach (var (location2, configMonthlyAction2) in _monthlyActions)
		{
			configMonthlyAction2.MonthlyHandler();
		}
	}

	public override MonthlyActionBase CreateCopy()
	{
		return GameData.Serializer.Serializer.CreateCopy(this);
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
		foreach (var (location2, configMonthlyAction2) in _monthlyActions)
		{
			configMonthlyAction2.CollectCalledCharacters(calledCharacters);
		}
	}

	public void DeactivateSubAction(short areaId, short blockId, bool isComplete)
	{
		Location key = new Location(areaId, blockId);
		ConfigMonthlyAction configMonthlyAction = _monthlyActions[key];
		configMonthlyAction.Deactivate(isComplete);
		_monthlyActions.Remove(key);
	}

	public ConfigMonthlyAction GetConfigAction(short areaId, short blockId)
	{
		return _monthlyActions[new Location(areaId, blockId)];
	}

	public ConfigMonthlyAction CreateNewConfigAction(short templateId, Location location)
	{
		ConfigMonthlyAction configMonthlyAction = new ConfigMonthlyAction(templateId, -1)
		{
			Key = Key,
			Location = location
		};
		if (!location.IsValid())
		{
			configMonthlyAction.SelectLocation();
		}
		_monthlyActions.Add(configMonthlyAction.Location, configMonthlyAction);
		return configMonthlyAction;
	}

	private void CreateSpringMarkets(DataContext context)
	{
		_prevSpringDate = DomainManager.World.GetCurrDate();
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		for (sbyte b = 0; b < 15; b++)
		{
			DomainManager.Map.GetStateSettlementIds(b, list, containsMainCity: true);
			list.RemoveAll((short id) => !IsValidSettlement(id));
			if (list.Count != 0)
			{
				CollectionUtils.Shuffle(context.Random, list);
				int num = context.Random.Next(3, 6);
				for (int num2 = 0; num2 < list.Count && num2 < num; num2++)
				{
					Settlement settlement = DomainManager.Organization.GetSettlement(list[num2]);
					Location location = settlement.GetLocation();
					AreaAdventureData areaAdventures = DomainManager.Adventure.GetAdventuresInArea(location.AreaId);
					list2.Clear();
					DomainManager.Map.GetSettlementBlocks(location.AreaId, location.BlockId, list2);
					list2.RemoveAll((short blockId) => areaAdventures.AdventureSites.ContainsKey(blockId));
					short random = list2.GetRandom(context.Random);
					DomainManager.Adventure.TryCreateAdventureSite(context, location.AreaId, random, 3, MonthlyActionKey.Invalid);
				}
			}
		}
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		monthlyNotificationCollection.AddMarketAppeared();
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(list2);
	}

	private void CreateSummerCombatSkillCompetitions(DataContext context)
	{
		_prevSummerDate = DomainManager.World.GetCurrDate();
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		HashSet<sbyte> allSelectableOrgTemplateIds = ObjectPool<HashSet<sbyte>>.Instance.Get();
		List<sbyte> list3 = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		list2.Clear();
		allSelectableOrgTemplateIds.Clear();
		list.AddRange(ConfigMonthlyActionDefines.CombatSkillTypeToMonthlyAction.Values);
		CollectionUtils.Shuffle(context.Random, list);
		for (sbyte b = 21; b <= 35; b++)
		{
			short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(b);
			if (IsValidSettlement(settlementIdByOrgTemplateId))
			{
				allSelectableOrgTemplateIds.Add(b);
			}
		}
		foreach (short item in list)
		{
			if (context.Random.CheckPercentProb(20))
			{
				if (allSelectableOrgTemplateIds.Count == 0)
				{
					break;
				}
				GetCurrSelectableOrgTemplateIds(MonthlyActions.Instance[item].MapBlockSubType, list3);
				if (list3.Count == 0)
				{
					break;
				}
				sbyte random = list3.GetRandom(context.Random);
				Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(random);
				Location location = settlementByOrgTemplateId.GetLocation();
				AreaAdventureData areaAdventures = DomainManager.Adventure.GetAdventuresInArea(location.AreaId);
				list2.Clear();
				DomainManager.Map.GetSettlementBlocks(location.AreaId, location.BlockId, list2);
				list2.RemoveAll((short blockId) => areaAdventures.AdventureSites.ContainsKey(blockId));
				if (list2.Count != 0)
				{
					short random2 = list2.GetRandom(context.Random);
					Location location2 = new Location(location.AreaId, random2);
					ConfigMonthlyAction configMonthlyAction = CreateNewConfigAction(item, location2);
					configMonthlyAction.TriggerAction();
					allSelectableOrgTemplateIds.Remove(random);
				}
			}
		}
		ObjectPool<List<short>>.Instance.Return(list2);
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<sbyte>>.Instance.Return(list3);
		ObjectPool<HashSet<sbyte>>.Instance.Return(allSelectableOrgTemplateIds);
		void GetCurrSelectableOrgTemplateIds(List<short> blockSubTypes, List<sbyte> orgTemplateIds)
		{
			orgTemplateIds.Clear();
			foreach (short blockSubType in blockSubTypes)
			{
				sbyte b2 = (sbyte)(21 + (blockSubType - 1));
				Tester.Assert(b2 >= 21 && b2 <= 35);
				if (allSelectableOrgTemplateIds.Contains(b2))
				{
					orgTemplateIds.Add(b2);
				}
			}
		}
	}

	private void CreateAutumnCricketConference(DataContext context)
	{
		int currDate = DomainManager.World.GetCurrDate();
		if (currDate - _prevAutumnDate >= 36)
		{
			_prevAutumnDate = currDate;
			short templateId = 73;
			ConfigMonthlyAction configMonthlyAction = CreateNewConfigAction(templateId, Location.Invalid);
			configMonthlyAction.TriggerAction();
		}
	}

	private void CreateWinterLifeSkillCompetitions(DataContext context)
	{
		_prevWinterDate = DomainManager.World.GetCurrDate();
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		list.AddRange(ConfigMonthlyActionDefines.MonthlyActionToLifeSkillType.Keys);
		short random = list.GetRandom(context.Random);
		ConfigMonthlyAction configMonthlyAction = CreateNewConfigAction(random, Location.Invalid);
		configMonthlyAction.TriggerAction();
	}

	private bool IsValidSettlement(short settlementId)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
		Location location = settlement.GetLocation();
		AreaAdventureData areaAdventures = DomainManager.Adventure.GetAdventuresInArea(location.AreaId);
		if (IsBlockIdValid(location.BlockId))
		{
			return true;
		}
		DomainManager.Map.GetSettlementBlocks(location.AreaId, location.BlockId, list);
		bool result = list.Exists(IsBlockIdValid);
		ObjectPool<List<short>>.Instance.Return(list);
		return result;
		bool IsBlockIdValid(short blockId)
		{
			return !areaAdventures.AdventureSites.ContainsKey(blockId);
		}
	}

	public SeasonalMonthlyAction()
	{
	}

	public override bool IsSerializedSizeFixed()
	{
		return false;
	}

	public override int GetSerializedSize()
	{
		int num = 28;
		if (_monthlyActions != null)
		{
			num += 2;
			foreach (KeyValuePair<Location, ConfigMonthlyAction> monthlyAction in _monthlyActions)
			{
				monthlyAction.Deconstruct(out var key, out var value);
				Location location = key;
				ConfigMonthlyAction configMonthlyAction = value;
				num += location.GetSerializedSize();
				num += configMonthlyAction.GetSerializedSize();
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
		ptr += Key.Serialize(ptr);
		*ptr = (byte)State;
		ptr++;
		*(int*)ptr = Month;
		ptr += 4;
		*(int*)ptr = LastFinishDate;
		ptr += 4;
		*(int*)ptr = _prevSpringDate;
		ptr += 4;
		*(int*)ptr = _prevSummerDate;
		ptr += 4;
		*(int*)ptr = _prevAutumnDate;
		ptr += 4;
		*(int*)ptr = _prevWinterDate;
		ptr += 4;
		if (_monthlyActions != null)
		{
			int count = _monthlyActions.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			foreach (KeyValuePair<Location, ConfigMonthlyAction> monthlyAction in _monthlyActions)
			{
				monthlyAction.Deconstruct(out var key, out var value);
				Location location = key;
				ConfigMonthlyAction configMonthlyAction = value;
				ptr += location.Serialize(ptr);
				ptr += configMonthlyAction.Serialize(ptr);
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

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += Key.Deserialize(ptr);
		State = (sbyte)(*ptr);
		ptr++;
		Month = *(int*)ptr;
		ptr += 4;
		LastFinishDate = *(int*)ptr;
		ptr += 4;
		_prevSpringDate = *(int*)ptr;
		ptr += 4;
		_prevSummerDate = *(int*)ptr;
		ptr += 4;
		_prevAutumnDate = *(int*)ptr;
		ptr += 4;
		_prevWinterDate = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (_monthlyActions == null)
		{
			_monthlyActions = new Dictionary<Location, ConfigMonthlyAction>();
		}
		else
		{
			_monthlyActions.Clear();
		}
		for (int i = 0; i < num; i++)
		{
			Location key = default(Location);
			ConfigMonthlyAction configMonthlyAction = new ConfigMonthlyAction();
			ptr += key.Deserialize(ptr);
			ptr += configMonthlyAction.Deserialize(ptr);
			_monthlyActions.Add(key, configMonthlyAction);
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
