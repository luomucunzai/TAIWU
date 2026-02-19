using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Global;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Taiwu;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;
using NLog;

namespace GameData.DLC.FiveLoong;

[SerializableGameData(IsExtensible = true, NoCopyConstructors = true, NotForDisplayModule = true)]
public class FiveLoongDlcEntry : IDlcEntry, ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort ChildrenOfLoongMonthlyEventChance = 0;

		public const ushort PulaoCricketLuckPoint = 1;

		public const ushort JiaoEggDropRate = 2;

		public const ushort MaleJiaoEggDropRate = 3;

		public const ushort IsJiaoPoolOpen = 4;

		public const ushort OwnedChildrenOfLoong = 5;

		public const ushort MaxTaiwuVillageLevel = 6;

		public const ushort Count = 7;

		public static readonly string[] FieldId2FieldName = new string[7] { "ChildrenOfLoongMonthlyEventChance", "PulaoCricketLuckPoint", "JiaoEggDropRate", "MaleJiaoEggDropRate", "IsJiaoPoolOpen", "OwnedChildrenOfLoong", "MaxTaiwuVillageLevel" };
	}

	public const int MaxJiaoPoolCount = 9;

	[SerializableGameDataField]
	public int ChildrenOfLoongMonthlyEventChance;

	[SerializableGameDataField]
	public int PulaoCricketLuckPoint;

	[SerializableGameDataField]
	public int JiaoEggDropRate;

	[SerializableGameDataField]
	public int MaleJiaoEggDropRate;

	[SerializableGameDataField]
	public bool IsJiaoPoolOpen;

	[SerializableGameDataField]
	public int OwnedChildrenOfLoong;

	[SerializableGameDataField]
	public int MaxTaiwuVillageLevel;

	private const int DisablePoolTamePointLoss = 5;

	private const int JiaoTamingPointsLowLimit = 50;

	private static Dictionary<short, short> _carrierToJiaoTemplate;

	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public static IReadOnlyDictionary<short, short> CarrierToJiaoTemplate => _carrierToJiaoTemplate;

	private void PostAdvanceMonth_Main(DataContext context)
	{
		DomainManager.Extra.ClearTempData();
		UpdateMaxTaiwuVillageLevel();
		if (IsMainStoryLineProgressMeetLoongDlc() && IsTaiwuVillageLevelMeetFiveLoongDlc())
		{
			EventArgBox orCreateDlcArgBox = DomainManager.Extra.GetOrCreateDlcArgBox(2764950uL, context);
			bool arg = false;
			if (!orCreateDlcArgBox.Get("ConchShip_PresetKey_FiveLoongDlcBeginMonthlyEventTriggered", ref arg) || !arg)
			{
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection.AddFiveLoongLetterFromTaiwuVillage();
			}
		}
		static bool IsMainStoryLineProgressMeetLoongDlc()
		{
			return DomainManager.World.GetMainStoryLineProgress() >= 21 && DomainManager.World.GetMainStoryLineProgress() < 27;
		}
		static bool IsTaiwuVillageLevelMeetFiveLoongDlc()
		{
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData buildingAreaData = DomainManager.Building.GetBuildingAreaData(taiwuVillageLocation);
			BuildingBlockKey buildingBlockKey = BuildingDomain.FindBuildingKey(taiwuVillageLocation, buildingAreaData, 44);
			if (!buildingBlockKey.IsInvalid && DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)buildingBlockKey, out var value) && value.CalcUnlockedLevelCount() >= 6)
			{
				return true;
			}
			return false;
		}
	}

	public void PostAdvanceMonth_JiaoPool(DataContext context)
	{
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		List<JiaoPool> jiaoPoolList = DomainManager.Extra.GetJiaoPoolList();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		if (!location.IsValid())
		{
			location = taiwu.GetValidLocation();
		}
		for (int i = 0; i < jiaoPoolList.Count; i++)
		{
			JiaoPool jiaoPool = jiaoPoolList[i];
			if (jiaoPool.Jiaos.Count == 1)
			{
				if (!DomainManager.Extra.TryGetJiao(jiaoPool.Jiaos[0], out var jiao))
				{
					continue;
				}
				if (jiao.GrowthStage == 0)
				{
					if (--jiaoPool.NextPeriod == 0)
					{
						DomainManager.Extra.JiaoHatch(context, i);
						monthlyNotificationCollection.AddJiaoBrokeThroughTheShell(location, jiao.Id);
					}
				}
				else
				{
					if (jiao.GrowthStage != 1)
					{
						continue;
					}
					if (DomainManager.Extra.JiaoFlee(context, i))
					{
						monthlyNotificationCollection.AddJiaoGoHome(location, jiao.Id);
						continue;
					}
					if (jiaoPool.IsDisabled)
					{
						jiao.TamePoint = Math.Max(0, jiao.TamePoint - 5);
					}
					else if (!DomainManager.Extra.IsResourceEnoughForJiaoFoster(jiao.NurturanceTemplateId))
					{
						monthlyNotificationCollection.AddJiaoPoolAccident(location, jiao.Id);
					}
					else if (jiao.EvolveRemainingMonth != 0)
					{
						if (jiao.NextPeriod >= 0)
						{
							DomainManager.Extra.ConsumeResourceForJiaoFoster(context, jiao.NurturanceTemplateId);
						}
						if (--jiao.NextPeriod == 0 && jiao.NurturanceTemplateId != 0)
						{
							DomainManager.Extra.JiaoGrow(context, i);
						}
						else if (jiao.EvolveRemainingMonth > 1)
						{
							DomainManager.Extra.JiaoEvent(context, i);
						}
						if (jiao.TamePoint <= 50)
						{
							monthlyNotificationCollection.AddJiaoTamingPointsLow(location, jiao.Id);
						}
						DomainManager.Extra.UpdateJiaoEvolveRemainingMonth(context, jiao);
					}
					if (jiao.EvolveRemainingMonth == 0)
					{
						DomainManager.Extra.JiaoEvolveToCarrier(context, i);
						monthlyNotificationCollection.AddJiaoHasReachedAnAdultAge(location, jiao.Id);
					}
				}
			}
			else if (jiaoPool.Jiaos.Count == 2 && --jiaoPool.NextPeriod == 0 && DomainManager.Extra.JiaoBreed(context, i))
			{
				monthlyNotificationCollection.AddJiaoLayEggs(location, jiaoPool.Jiaos[0], jiaoPool.Jiaos[1]);
			}
		}
		DomainManager.Extra.SetJiaoPools(jiaoPoolList, context);
	}

	public void PostAdvanceMonth_JiaoPoolLog(DataContext context)
	{
		List<JiaoPoolRecord> list = new List<JiaoPoolRecord>();
		int currDate = DomainManager.World.GetCurrDate();
		List<JiaoPoolRecordList> jiaoPoolRecords = DomainManager.Extra.GetJiaoPoolRecords();
		foreach (JiaoPoolRecordList item in jiaoPoolRecords)
		{
			list.Clear();
			foreach (JiaoPoolRecord item2 in item.Collection)
			{
				if (currDate - item2.Date <= 24)
				{
					list.Add(item2);
				}
			}
			item.Collection.Clear();
			item.Collection.AddRange(list);
		}
		DomainManager.Extra.SetJiaoPoolRecords(jiaoPoolRecords, context);
	}

	private void PostAdvanceMonth_ChildOfLoong(DataContext context)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		ItemKey itemKey = taiwu.GetEquipment()[11];
		if (!itemKey.IsValid() || DomainManager.Item.GetBaseItem(itemKey).IsDurabilityRunningOut() || !CarrierToJiaoTemplate.TryGetValue(itemKey.TemplateId, out var value))
		{
			return;
		}
		JiaoItem jiaoItem = Config.Jiao.Instance[value];
		if (jiaoItem.MonthlyEventCost <= 0)
		{
			return;
		}
		if (!context.Random.CheckPercentProb(ChildrenOfLoongMonthlyEventChance))
		{
			ChildrenOfLoongMonthlyEventChance += 10;
			return;
		}
		ChildrenOfLoong childrenOfLoongByItemKey = DomainManager.Extra.GetChildrenOfLoongByItemKey(itemKey);
		short templateId = itemKey.TemplateId;
		if (1 == 0)
		{
		}
		bool flag = templateId switch
		{
			77 => PostAdvanceMonth_ChildOfLoong_Qiuniu(context, childrenOfLoongByItemKey), 
			78 => PostAdvanceMonth_ChildOfLoong_Yazi(context, childrenOfLoongByItemKey), 
			79 => PostAdvanceMonth_ChildOfLoong_Chaofeng(context, childrenOfLoongByItemKey), 
			80 => PostAdvanceMonth_ChildOfLoong_Pulao(context, childrenOfLoongByItemKey), 
			81 => PostAdvanceMonth_ChildOfLoong_Suanni(context, childrenOfLoongByItemKey), 
			82 => PostAdvanceMonth_ChildOfLoong_Baxia(context, childrenOfLoongByItemKey), 
			83 => PostAdvanceMonth_ChildOfLoong_Bian(context, childrenOfLoongByItemKey), 
			84 => PostAdvanceMonth_ChildOfLoong_Fuxi(context, childrenOfLoongByItemKey), 
			85 => PostAdvanceMonth_ChildOfLoong_Chiwen(context, childrenOfLoongByItemKey), 
			_ => false, 
		};
		if (1 == 0)
		{
		}
		if (flag)
		{
			ChildrenOfLoongMonthlyEventChance -= jiaoItem.MonthlyEventCost;
		}
		else
		{
			ChildrenOfLoongMonthlyEventChance += 10;
		}
	}

	private bool PostAdvanceMonth_ChildOfLoong_Qiuniu(DataContext context, ChildrenOfLoong childOfLoong)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		if (!location.IsValid())
		{
			return false;
		}
		List<MapBlockData> obj = context.AdvanceMonthRelatedData.Blocks.Occupy();
		DomainManager.Map.GetNeighborBlocks(location.AreaId, location.BlockId, obj, 3);
		List<int> obj2 = context.AdvanceMonthRelatedData.CharIdList.Occupy();
		foreach (MapBlockData item in obj)
		{
			if (item.CharacterSet == null)
			{
				continue;
			}
			foreach (int item2 in item.CharacterSet)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item2);
				if (!element_Objects.IsActiveExternalRelationState(60))
				{
					obj2.Add(item2);
				}
			}
		}
		context.AdvanceMonthRelatedData.Blocks.Release(ref obj);
		if (obj2.Count == 0)
		{
			context.AdvanceMonthRelatedData.CharIdList.Release(ref obj2);
			return false;
		}
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		int num = context.Random.Next(3) + 1;
		for (int i = 0; i < num; i++)
		{
			if (obj2.Count == 0)
			{
				break;
			}
			int index = context.Random.Next(obj2.Count);
			int num2 = obj2[index];
			CollectionUtils.SwapAndRemove(obj2, index);
			GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(num2);
			int baseDelta = context.Random.Next(2000, 4001);
			DomainManager.Character.ChangeFavorabilityOptional(context, element_Objects2, taiwu, baseDelta, 5);
			Location location2 = element_Objects2.GetLocation();
			lifeRecordCollection.AddDLCLoongRidingEffectQiuniuAudience(num2, currDate, taiwuCharId, location2, childOfLoong.Id);
		}
		context.AdvanceMonthRelatedData.CharIdList.Release(ref obj2);
		lifeRecordCollection.AddDLCLoongRidingEffectQiuniu(taiwuCharId, currDate, location, childOfLoong.Id);
		monthlyEventCollection.AddDLCLoongRidingEffectQiuniu(taiwuCharId, childOfLoong.Id);
		monthlyNotificationCollection.AddDLCLoongRidingEffectQiuniu(taiwuCharId, location, childOfLoong.Id);
		return true;
	}

	private bool PostAdvanceMonth_ChildOfLoong_Yazi(DataContext context, ChildrenOfLoong childOfLoong)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		if (!location.IsValid())
		{
			return false;
		}
		List<MapBlockData> obj = context.AdvanceMonthRelatedData.Blocks.Occupy();
		DomainManager.Map.GetNeighborBlocks(location.AreaId, location.BlockId, obj, 3);
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		List<int> obj2 = context.AdvanceMonthRelatedData.CharIdList.Occupy();
		foreach (MapBlockData item in obj)
		{
			if (item.CharacterSet == null)
			{
				continue;
			}
			foreach (int item2 in item.CharacterSet)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item2);
				if (element_Objects.GetAgeGroup() != 0 && !element_Objects.IsActiveExternalRelationState(60) && DomainManager.Character.HasRelation(taiwuCharId, item2, 32768))
				{
					obj2.Add(item2);
				}
			}
		}
		int randomOrDefault = obj2.GetRandomOrDefault(context.Random, -1);
		context.AdvanceMonthRelatedData.CharIdList.Release(ref obj2);
		context.AdvanceMonthRelatedData.Blocks.Release(ref obj);
		if (randomOrDefault < 0)
		{
			return false;
		}
		GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(randomOrDefault);
		DomainManager.Character.SimulateEnemyAttack(context, 728, element_Objects2);
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		lifeRecordCollection.AddDLCLoongRidingEffectYazi(taiwuCharId, currDate, randomOrDefault, location, childOfLoong.Id);
		lifeRecordCollection.AddDLCLoongRidingEffectYazi2(randomOrDefault, currDate, location, childOfLoong.Id);
		monthlyEventCollection.AddDLCLoongRidingEffectYazi(taiwuCharId, childOfLoong.Id, randomOrDefault);
		monthlyNotificationCollection.AddDLCLoongRidingEffectYazi(randomOrDefault, location, childOfLoong.Id);
		return true;
	}

	private bool PostAdvanceMonth_ChildOfLoong_Chaofeng(DataContext context, ChildrenOfLoong childOfLoong)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		List<int> secretInformationOfCharacter = DomainManager.Information.GetSecretInformationOfCharacter(taiwuCharId, includeBroadcast: false);
		Location location = taiwu.GetLocation();
		if (!location.IsValid())
		{
			return false;
		}
		List<MapBlockData> obj = context.AdvanceMonthRelatedData.Blocks.Occupy();
		DomainManager.Map.GetNeighborBlocks(location.AreaId, location.BlockId, obj, 3);
		List<int> obj2 = context.AdvanceMonthRelatedData.CharIdList.Occupy();
		foreach (MapBlockData item in obj)
		{
			if (item.CharacterSet == null)
			{
				continue;
			}
			foreach (int item2 in item.CharacterSet)
			{
				List<int> secretInformationOfCharacter2 = DomainManager.Information.GetSecretInformationOfCharacter(item2, includeBroadcast: false);
				if (secretInformationOfCharacter2 == null || secretInformationOfCharacter2.Count == 0)
				{
					continue;
				}
				if (secretInformationOfCharacter == null || secretInformationOfCharacter.Count == 0)
				{
					obj2.Add(item2);
					continue;
				}
				foreach (int item3 in secretInformationOfCharacter2)
				{
					if (secretInformationOfCharacter.Contains(item3))
					{
						obj2.Add(item2);
						break;
					}
				}
			}
		}
		int randomOrDefault = obj2.GetRandomOrDefault(context.Random, -1);
		context.AdvanceMonthRelatedData.CharIdList.Release(ref obj2);
		context.AdvanceMonthRelatedData.Blocks.Release(ref obj);
		if (randomOrDefault < 0)
		{
			return false;
		}
		List<int> secretInformationOfCharacter3 = DomainManager.Information.GetSecretInformationOfCharacter(randomOrDefault, includeBroadcast: false);
		List<int> obj3 = context.AdvanceMonthRelatedData.IntList.Occupy();
		if (secretInformationOfCharacter == null)
		{
			obj3.AddRange(secretInformationOfCharacter3);
		}
		else
		{
			foreach (int item4 in secretInformationOfCharacter3)
			{
				if (!secretInformationOfCharacter.Contains(item4))
				{
					obj3.Add(item4);
				}
			}
		}
		int random = obj3.GetRandom(context.Random);
		DomainManager.Information.ReceiveSecretInformation(context, random, taiwuCharId, randomOrDefault);
		context.AdvanceMonthRelatedData.IntList.Release(ref obj3);
		int currDate = DomainManager.World.GetCurrDate();
		Location location2 = taiwu.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		monthlyEventCollection.AddDLCLoongRidingEffectChaofeng(taiwuCharId, childOfLoong.Id, randomOrDefault);
		lifeRecordCollection.AddDLCLoongRidingEffectChaofeng(taiwuCharId, currDate, location2, childOfLoong.Id);
		monthlyNotificationCollection.AddDLCLoongRidingEffectChaofeng(location2, childOfLoong.Id);
		return true;
	}

	private bool PostAdvanceMonth_ChildOfLoong_Pulao(DataContext context, ChildrenOfLoong childOfLoong)
	{
		ItemKey itemKey = DomainManager.Item.CreateCricketByLuckPoint(context, ref PulaoCricketLuckPoint);
		DomainManager.Taiwu.GetTaiwu().AddInventoryItem(context, itemKey, 1);
		GameData.Domains.Item.Cricket element_Crickets = DomainManager.Item.GetElement_Crickets(itemKey.Id);
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		int currDate = DomainManager.World.GetCurrDate();
		short colorId = element_Crickets.GetColorId();
		short partId = element_Crickets.GetPartId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		lifeRecordCollection.AddDLCLoongRidingEffectPulao(taiwuCharId, currDate, childOfLoong.Id, colorId, partId);
		monthlyNotificationCollection.AddDLCLoongRidingEffectPulao(childOfLoong.Id, colorId, partId);
		monthlyEventCollection.AddDLCLoongRidingEffectPulao(taiwuCharId, childOfLoong.Id, colorId, partId);
		return true;
	}

	private bool PostAdvanceMonth_ChildOfLoong_Suanni(DataContext context, ChildrenOfLoong childOfLoong)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Inventory inventory = taiwu.GetInventory();
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(taiwu.GetId());
		List<ItemKey> obj = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
		foreach (var (item, _) in inventory.Items)
		{
			if (item.ItemType != 10)
			{
				continue;
			}
			SkillBookItem skillBookItem = Config.SkillBook.Instance[item.TemplateId];
			if (skillBookItem.ItemSubType != 1001 || !charCombatSkills.TryGetValue(skillBookItem.CombatSkillTemplateId, out var value))
			{
				continue;
			}
			GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(item.Id);
			byte pageTypes = element_SkillBooks.GetPageTypes();
			ushort readingState = value.GetReadingState();
			sbyte outlinePageType = SkillBookStateHelper.GetOutlinePageType(pageTypes);
			byte outlinePageInternalIndex = CombatSkillStateHelper.GetOutlinePageInternalIndex(outlinePageType);
			bool flag = CombatSkillStateHelper.IsPageRead(readingState, outlinePageInternalIndex);
			bool flag2 = !flag;
			for (byte b = 1; b < 6; b++)
			{
				sbyte normalPageType = SkillBookStateHelper.GetNormalPageType(pageTypes, b);
				byte normalPageInternalIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(normalPageType, b);
				if (CombatSkillStateHelper.IsPageRead(readingState, normalPageInternalIndex))
				{
					flag = true;
				}
				else
				{
					flag2 = true;
				}
			}
			if (flag && flag2)
			{
				obj.Add(item);
			}
		}
		bool flag3 = obj.Count > 0;
		if (flag3)
		{
			ItemKey random = obj.GetRandom(context.Random);
			GameData.Domains.Item.SkillBook element_SkillBooks2 = DomainManager.Item.GetElement_SkillBooks(random.Id);
			byte pageTypes2 = element_SkillBooks2.GetPageTypes();
			GameData.Domains.CombatSkill.CombatSkill combatSkill = charCombatSkills[element_SkillBooks2.GetCombatSkillTemplateId()];
			ushort readingState2 = combatSkill.GetReadingState();
			sbyte outlinePageType2 = SkillBookStateHelper.GetOutlinePageType(pageTypes2);
			for (byte b2 = 0; b2 < 6; b2++)
			{
				sbyte normalPageType2 = SkillBookStateHelper.GetNormalPageType(pageTypes2, b2);
				byte pageInternalIndex = CombatSkillStateHelper.GetPageInternalIndex(outlinePageType2, normalPageType2, b2);
				if (!CombatSkillStateHelper.IsPageRead(readingState2, pageInternalIndex))
				{
					DomainManager.Taiwu.ReadSkillBookPageAndSetComplete(context, element_SkillBooks2, b2);
					int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
					int currDate = DomainManager.World.GetCurrDate();
					Location location = taiwu.GetLocation();
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
					MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
					monthlyEventCollection.AddDLCLoongRidingEffectSuanni(taiwuCharId, childOfLoong.Id, random.ItemType, random.TemplateId, b2 + 1);
					lifeRecordCollection.AddDLCLoongRidingEffectSuanni(taiwuCharId, currDate, location, childOfLoong.Id, random.ItemType, random.TemplateId, b2 + 1);
					monthlyNotificationCollection.AddDLCLoongRidingEffectSuanni(location, childOfLoong.Id, random.ItemType, random.TemplateId, b2 + 1);
					break;
				}
			}
		}
		context.AdvanceMonthRelatedData.ItemKeys.Release(ref obj);
		return flag3;
	}

	private bool PostAdvanceMonth_ChildOfLoong_Baxia(DataContext context, ChildrenOfLoong childOfLoong)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		if (!location.IsValid())
		{
			return false;
		}
		List<MapBlockData> obj = context.AdvanceMonthRelatedData.Blocks.Occupy();
		DomainManager.Map.GetNeighborBlocks(location.AreaId, location.BlockId, obj, 3);
		for (int num = obj.Count - 1; num >= 0; num--)
		{
			MapBlockData mapBlockData = obj[num];
			if (mapBlockData.Items == null)
			{
				obj.RemoveAt(num);
			}
		}
		int num2 = context.Random.Next(3) + 1;
		bool flag = false;
		for (int i = 0; i < num2; i++)
		{
			if (obj.Count == 0)
			{
				break;
			}
			int index = context.Random.Next(obj.Count);
			MapBlockData mapBlockData2 = obj[index];
			ItemKeyAndDate random = mapBlockData2.Items.Keys.GetRandom(context.Random);
			ItemKey itemKey = random.ItemKey;
			int amount = mapBlockData2.Items[random];
			flag = true;
			DomainManager.Map.RemoveBlockItem(context, mapBlockData2, random);
			taiwu.AddInventoryItem(context, itemKey, amount);
			if (mapBlockData2.Items == null)
			{
				obj.RemoveAt(index);
			}
		}
		context.AdvanceMonthRelatedData.Blocks.Release(ref obj);
		if (flag)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			int currDate = DomainManager.World.GetCurrDate();
			Location location2 = taiwu.GetLocation();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			lifeRecordCollection.AddDLCLoongRidingEffectBaxia(taiwuCharId, currDate, location2, childOfLoong.Id);
			monthlyEventCollection.AddDLCLoongRidingEffectBaxia(taiwuCharId, childOfLoong.Id);
			monthlyNotificationCollection.AddDLCLoongRidingEffectBaxia(taiwuCharId, location2, childOfLoong.Id);
		}
		return flag;
	}

	private bool PostAdvanceMonth_ChildOfLoong_Bian(DataContext context, ChildrenOfLoong childOfLoong)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int currDate = DomainManager.World.GetCurrDate();
		List<FameActionRecord> fameActionRecords = taiwu.GetFameActionRecords();
		List<(short, short)> obj = context.AdvanceMonthRelatedData.WeightTable.Occupy();
		foreach (FameActionRecord item in fameActionRecords)
		{
			if (item.EndDate > currDate)
			{
				FameActionItem fameActionItem = FameAction.Instance[item.Id];
				obj.Add((item.Id, fameActionItem.MaxStackCount));
			}
		}
		if (obj.Count == 0)
		{
			context.AdvanceMonthRelatedData.WeightTable.Release(ref obj);
			return false;
		}
		short randomResult = RandomUtils.GetRandomResult(obj, context.Random);
		context.AdvanceMonthRelatedData.WeightTable.Release(ref obj);
		taiwu.RecordFameAction(context, randomResult, -1, 1);
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		Location location = taiwu.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		lifeRecordCollection.AddDLCLoongRidingEffectBian(taiwuCharId, currDate, location, childOfLoong.Id);
		monthlyEventCollection.AddDLCLoongRidingEffectBian(taiwuCharId, childOfLoong.Id);
		monthlyNotificationCollection.AddDLCLoongRidingEffectBian(location, childOfLoong.Id);
		return false;
	}

	private bool PostAdvanceMonth_ChildOfLoong_Fuxi(DataContext context, ChildrenOfLoong childOfLoong)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Inventory inventory = taiwu.GetInventory();
		List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = taiwu.GetLearnedLifeSkills();
		List<ItemKey> obj = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
		foreach (var (item, _) in inventory.Items)
		{
			if (item.ItemType != 10)
			{
				continue;
			}
			SkillBookItem skillBookItem = Config.SkillBook.Instance[item.TemplateId];
			if (skillBookItem.ItemSubType != 1000)
			{
				continue;
			}
			int num2 = taiwu.FindLearnedLifeSkillIndex(skillBookItem.LifeSkillTemplateId);
			if (num2 >= 0)
			{
				GameData.Domains.Character.LifeSkillItem lifeSkillItem = learnedLifeSkills[num2];
				if (!lifeSkillItem.IsAllPagesRead() && lifeSkillItem.ReadingState != 0)
				{
					obj.Add(item);
				}
			}
		}
		if (obj.Count == 0)
		{
			context.AdvanceMonthRelatedData.ItemKeys.Release(ref obj);
			return false;
		}
		ItemKey random = obj.GetRandom(context.Random);
		context.AdvanceMonthRelatedData.ItemKeys.Release(ref obj);
		GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(random.Id);
		int index = taiwu.FindLearnedLifeSkillIndex(element_SkillBooks.GetLifeSkillTemplateId());
		GameData.Domains.Character.LifeSkillItem lifeSkillItem2 = learnedLifeSkills[index];
		for (byte b = 0; b < 5; b++)
		{
			if (!lifeSkillItem2.IsPageRead(b))
			{
				DomainManager.Taiwu.ReadSkillBookPageAndSetComplete(context, element_SkillBooks, b);
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				int currDate = DomainManager.World.GetCurrDate();
				Location location = taiwu.GetLocation();
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
				monthlyEventCollection.AddDLCLoongRidingEffectFuxi(taiwuCharId, childOfLoong.Id, random.ItemType, random.TemplateId, b + 1);
				lifeRecordCollection.AddDLCLoongRidingEffectFuxi(taiwuCharId, currDate, location, childOfLoong.Id, random.ItemType, random.TemplateId, b + 1);
				monthlyNotificationCollection.AddDLCLoongRidingEffectFuxi(location, childOfLoong.Id, random.ItemType, random.TemplateId, b + 1);
				break;
			}
		}
		return true;
	}

	private bool PostAdvanceMonth_ChildOfLoong_Chiwen(DataContext context, ChildrenOfLoong childOfLoong)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int currNeili = taiwu.GetCurrNeili();
		int maxNeili = taiwu.GetMaxNeili();
		int currNeiliRecovery = taiwu.GetCurrNeiliRecovery(maxNeili);
		if (maxNeili - currNeili < currNeiliRecovery * 3)
		{
			return false;
		}
		int delta = currNeiliRecovery * (context.Random.Next(3) + 1);
		taiwu.ChangeCurrNeili(context, delta);
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = taiwu.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		lifeRecordCollection.AddDLCLoongRidingEffectChiwen(taiwuCharId, currDate, location, childOfLoong.Id);
		monthlyEventCollection.AddDLCLoongRidingEffectChiwen(taiwuCharId, childOfLoong.Id);
		monthlyNotificationCollection.AddDLCLoongRidingEffectChiwen(location, childOfLoong.Id);
		return false;
	}

	private void PostAdvanceMonth_FiveLoongs(DataContext context)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		HashSet<int> hashSet = new HashSet<int>();
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		List<int> obj = context.AdvanceMonthRelatedData.CharIdList.Occupy();
		foreach (KeyValuePair<short, LoongInfo> item in DomainManager.Extra.FiveLoongDict)
		{
			item.Deconstruct(out var key, out var value);
			short loongCharTemplateId = key;
			LoongInfo loongInfo = value;
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(loongInfo.LoongCurrentLocation.AreaId);
			LoongItem configData = loongInfo.ConfigData;
			hashSet.Clear();
			hashSet.UnionWith(collection);
			if (loongInfo.CoveredMapBlockTemplateId != null)
			{
				foreach (short key2 in loongInfo.CoveredMapBlockTemplateId.Keys)
				{
					MapBlockData mapBlockData = areaBlocks[key2];
					if (mapBlockData.CharacterSet == null)
					{
						continue;
					}
					foreach (int item2 in mapBlockData.CharacterSet)
					{
						GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item2);
						if (OfflineTryApplyLoongBlockDebuff(loongInfo, element_Objects) != 0)
						{
							hashSet.Add(item2);
						}
					}
				}
			}
			if (loongInfo.CharacterDebuffCounts != null)
			{
				obj.Clear();
				obj.AddRange(loongInfo.CharacterDebuffCounts.Keys);
				foreach (int item3 in obj)
				{
					if (!hashSet.Contains(item3))
					{
						if (DomainManager.Character.TryGetElement_Objects(item3, out var element))
						{
							OfflineTryReduceLoongBlockDebuff(loongInfo, element);
						}
						else
						{
							loongInfo.CharacterDebuffCounts.Remove(item3);
						}
					}
				}
			}
			obj.Clear();
			Location loongCurrentLocation = loongInfo.LoongCurrentLocation;
			if (!loongInfo.IsDisappear)
			{
				MapBlockData mapBlockData2 = areaBlocks[loongCurrentLocation.BlockId];
				if (mapBlockData2.CharacterSet != null)
				{
					obj.AddRange(mapBlockData2.CharacterSet);
					int random = obj.GetRandom(context.Random);
					GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(random);
					AiHelper.NpcCombatResultType npcCombatResultType = DomainManager.Character.SimulateEnemyAttack(context, loongInfo.CharacterTemplateId, element_Objects2);
					if (npcCombatResultType == AiHelper.NpcCombatResultType.MajorVictory || npcCombatResultType == AiHelper.NpcCombatResultType.MinorVictory)
					{
						lifeRecordCollection.AddDefeatedByLoong(random, currDate, loongInfo.CharacterTemplateId, loongCurrentLocation);
					}
					else
					{
						lifeRecordCollection.AddDefeatLoong(random, currDate, loongInfo.CharacterTemplateId, loongCurrentLocation);
					}
				}
				List<short> obj2 = context.AdvanceMonthRelatedData.BlockIds.Occupy();
				Span<MapBlockData> areaBlocks2 = DomainManager.Map.GetAreaBlocks(loongCurrentLocation.AreaId);
				foreach (short key3 in loongInfo.CoveredMapBlockTemplateId.Keys)
				{
					if (IsBlockLoongBlock(areaBlocks2[key3]))
					{
						obj2.Add(key3);
					}
				}
				short random2 = obj2.GetRandom(context.Random);
				context.AdvanceMonthRelatedData.BlockIds.Release(ref obj2);
				DomainManager.Extra.RemoveAnimalByLocationAndTemplateId(context, loongInfo.LoongCurrentLocation, loongInfo.CharacterTemplateId);
				loongInfo.LoongCurrentLocation = new Location(loongInfo.LoongCurrentLocation.AreaId, random2);
				DomainManager.Extra.CreateAnimalByCharacterTemplateId(context, loongInfo.CharacterTemplateId, loongInfo.LoongCurrentLocation);
			}
			else if (loongInfo.DisappearDate + 108 <= DomainManager.World.GetCurrDate() && loongInfo.IsDisappear && !TryCreateOrReAppearFiveLoong(context, isStrict: true, loongInfo.CharacterTemplateId))
			{
				TryCreateOrReAppearFiveLoong(context, isStrict: false, loongInfo.CharacterTemplateId);
			}
			if (DomainManager.Extra.TryGetAnimalAreaDataByAreaId(loongCurrentLocation.AreaId, out var animalAreaData))
			{
				List<GameData.Domains.Character.Animal> list = null;
				foreach (KeyValuePair<short, List<int>> item4 in animalAreaData)
				{
					item4.Deconstruct(out key, out var value2);
					short index = key;
					List<int> list2 = value2;
					GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
					Location location = taiwu.GetLocation();
					int id = taiwu.GetId();
					short minionCharTemplateId = configData.MinionCharTemplateId;
					foreach (int item5 in list2)
					{
						if (!DomainManager.Extra.TryGetAnimal(item5, out var animal) || animal.CharacterTemplateId != minionCharTemplateId)
						{
							continue;
						}
						obj.Clear();
						MapBlockData mapBlockData3 = areaBlocks[index];
						Location location2 = mapBlockData3.GetLocation();
						if (mapBlockData3.CharacterSet != null)
						{
							obj.AddRange(mapBlockData3.CharacterSet);
						}
						if (location2 == location)
						{
							obj.Add(id);
						}
						if (obj.Count == 0)
						{
							continue;
						}
						int random3 = obj.GetRandom(context.Random);
						GameData.Domains.Character.Character element_Objects3 = DomainManager.Character.GetElement_Objects(random3);
						if (random3 == id)
						{
							MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
							monthlyEventCollection.AddMinionLoongAttack(id, loongCurrentLocation, minionCharTemplateId);
							continue;
						}
						AiHelper.NpcCombatResultType npcCombatResultType2 = DomainManager.Character.SimulateEnemyAttack(context, minionCharTemplateId, element_Objects3);
						if (npcCombatResultType2 == AiHelper.NpcCombatResultType.MajorVictory || npcCombatResultType2 == AiHelper.NpcCombatResultType.MinorVictory)
						{
							lifeRecordCollection.AddDefeatedByAnimal(random3, currDate, location2, animal.CharacterTemplateId);
							continue;
						}
						if (list == null)
						{
							list = new List<GameData.Domains.Character.Animal>();
						}
						list.Add(animal);
						lifeRecordCollection.AddKillAnimal(random3, currDate, location2, animal.CharacterTemplateId);
					}
				}
				if (list != null)
				{
					foreach (GameData.Domains.Character.Animal item6 in list)
					{
						DomainManager.Extra.RemoveAnimal(context, item6);
					}
				}
			}
			DomainManager.Extra.SetLoongInfo(context, loongCharTemplateId, loongInfo);
		}
		context.AdvanceMonthRelatedData.CharIdList.Release(ref obj);
	}

	private static int OfflineTryApplyLoongBlockDebuff(LoongInfo loongInfo, GameData.Domains.Character.Character character)
	{
		int id = character.GetId();
		LoongItem configData = loongInfo.ConfigData;
		sbyte personality = character.GetPersonality(configData.PersonalityType);
		bool flag = false;
		ItemKey itemKey = character.GetEquipment()[4];
		if (itemKey.IsValid() && !DomainManager.Item.GetBaseItem(itemKey).IsDurabilityRunningOut() && itemKey.TemplateId == configData.ClothingTemplateId)
		{
			flag = true;
		}
		if (personality < configData.PersonalityRequirement && !flag)
		{
			loongInfo.ChangeCharacterDebuffCount(id, 1);
			return 1;
		}
		if (loongInfo.GetCharacterDebuffCount(id) > 0)
		{
			loongInfo.ChangeCharacterDebuffCount(id, -1);
			return -1;
		}
		return 0;
	}

	private static int OfflineTryReduceLoongBlockDebuff(LoongInfo loongInfo, GameData.Domains.Character.Character character)
	{
		int id = character.GetId();
		if (loongInfo.CharacterDebuffCounts == null || !loongInfo.CharacterDebuffCounts.ContainsKey(id))
		{
			return 0;
		}
		loongInfo.ChangeCharacterDebuffCount(id, -1);
		return -1;
	}

	private static void OnTaiwuMove(DataContext context, MapBlockData srcBlock, MapBlockData destBlock, int actionPointCost)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int id = taiwu.GetId();
		MapBlockItem config = destBlock.GetConfig();
		foreach (KeyValuePair<short, LoongInfo> item in DomainManager.Extra.FiveLoongDict)
		{
			item.Deconstruct(out var key, out var value);
			short loongCharTemplateId = key;
			LoongInfo loongInfo = value;
			LoongItem configData = loongInfo.ConfigData;
			bool flag = configData.MapBlock == config.TemplateId;
			int num = (flag ? OfflineTryApplyLoongBlockDebuff(loongInfo, taiwu) : OfflineTryReduceLoongBlockDebuff(loongInfo, taiwu));
			bool flag2 = num != 0;
			if (flag2)
			{
				AddLoongDebuffInstantNotification((num > 0) ? configData.DebuffCountIncNotification : configData.DebuffCountDecNotification, (loongInfo.CharacterDebuffCounts != null && loongInfo.CharacterDebuffCounts.TryGetValue(id, out var value2)) ? value2 : 0);
			}
			HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int item2 in collection)
			{
				if (item2 != id)
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item2);
					if ((flag ? OfflineTryApplyLoongBlockDebuff(loongInfo, element_Objects) : OfflineTryReduceLoongBlockDebuff(loongInfo, element_Objects)) != 0)
					{
						flag2 = true;
					}
				}
			}
			if (flag2)
			{
				DomainManager.Extra.SetLoongInfo(context, loongCharTemplateId, loongInfo);
			}
		}
	}

	public static void OnDefeatFiveLoongInCombat(DataContext context, sbyte combatStatus)
	{
		CombatConfigItem combatConfig = DomainManager.Combat.CombatConfig;
		if (combatStatus == CombatStatusType.EnemyFail && combatConfig.TemplateId >= 182 && combatConfig.TemplateId <= 186)
		{
			short randomAvailableLegacy = DomainManager.Taiwu.GetRandomAvailableLegacy(context.Random, -1);
			if (DomainManager.Taiwu.AddAvailableLegacy(context, randomAvailableLegacy))
			{
				DomainManager.Combat.AddCombatResultLegacy(randomAvailableLegacy);
			}
			short randomAvailableLegacy2 = DomainManager.Taiwu.GetRandomAvailableLegacy(context.Random, -1);
			if (DomainManager.Taiwu.AddAvailableLegacy(context, randomAvailableLegacy2))
			{
				DomainManager.Combat.AddCombatResultLegacy(randomAvailableLegacy2);
			}
			short randomAvailableLegacy3 = DomainManager.Taiwu.GetRandomAvailableLegacy(context.Random, -1);
			if (DomainManager.Taiwu.AddAvailableLegacy(context, randomAvailableLegacy3))
			{
				DomainManager.Combat.AddCombatResultLegacy(randomAvailableLegacy3);
			}
		}
	}

	public static bool IsCharacterMinionLoong(short charTemplateId)
	{
		return charTemplateId >= 691 && charTemplateId <= 695;
	}

	public static bool IsCharacterLoong(short charTemplateId)
	{
		return charTemplateId >= 686 && charTemplateId <= 690;
	}

	public static short MinionLoongToLoong(short minionCharTemplateId)
	{
		return (short)(minionCharTemplateId - 691 + 686);
	}

	public static void AddLoongDebuffInstantNotification(short notificationTemplate, int debuffCount)
	{
		InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
		switch (notificationTemplate)
		{
		case 120:
			instantNotificationCollection.AddThunderPowerGrow(debuffCount);
			break;
		case 121:
			instantNotificationCollection.AddFloodPowerGrow(debuffCount);
			break;
		case 123:
			instantNotificationCollection.AddStormPowerGrow(debuffCount);
			break;
		case 122:
			instantNotificationCollection.AddBlazePowerGrow(debuffCount);
			break;
		case 124:
			instantNotificationCollection.AddSandPowerGrow(debuffCount);
			break;
		case 125:
			instantNotificationCollection.AddThunderPowerDecline(debuffCount);
			break;
		case 126:
			instantNotificationCollection.AddFloodPowerDecline(debuffCount);
			break;
		case 128:
			instantNotificationCollection.AddStormPowerDecline(debuffCount);
			break;
		case 127:
			instantNotificationCollection.AddBlazePowerDecline(debuffCount);
			break;
		case 129:
			instantNotificationCollection.AddSandPowerDecline(debuffCount);
			break;
		}
	}

	public static bool IsBlockLoongBlock(MapBlockData block)
	{
		return MapBlock.Instance[block.TemplateId].SubType == EMapBlockSubType.DLCLoong;
	}

	private void UpdateMaxTaiwuVillageLevel()
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData buildingAreaData = DomainManager.Building.GetBuildingAreaData(taiwuVillageLocation);
		BuildingBlockKey blockKey = BuildingDomain.FindBuildingKey(taiwuVillageLocation, buildingAreaData, 44);
		MaxTaiwuVillageLevel = Math.Max(MaxTaiwuVillageLevel, DomainManager.Building.BuildingBlockLevel(blockKey));
	}

	private void FixAbnormalJiaoNextPeriodData(DataContext context)
	{
		foreach (Jiao value in DomainManager.Extra.GetAllJiaos().Values)
		{
			if (value.GrowthStage == 1 && value.NurturanceTemplateId != 0 && value.EvolveRemainingMonth != 0)
			{
				short stageCostMonth = JiaoNurturance.Instance[value.NurturanceTemplateId].StageCostMonth;
				int num = value.EvolveRemainingMonth % stageCostMonth;
				value.NextPeriod = ((num == 0) ? stageCostMonth : num);
				DomainManager.Extra.SetJiao(context, value);
				Logger.Warn($"Fix Jiao NextPeriod: {value.Id}");
			}
		}
	}

	private void FixAbnormalJiaoPriceData(DataContext context)
	{
		foreach (Jiao value in DomainManager.Extra.GetAllJiaos().Values)
		{
			if ((value.Properties.Value.Inherited + value.Properties.Value.Fostered / 100) * 2 < value.Properties.ExploreBonusRate.Inherited + value.Properties.ExploreBonusRate.Fostered / 100)
			{
				Logger.Warn($"Fix Jiao Value: {value.Id},  {value.Properties.Value} -> {value.Properties.ExploreBonusRate}");
				value.Properties.Value = value.Properties.ExploreBonusRate;
			}
			Logger.Warn($"Fix Jiao ExploreBonusRate: {value.Id},  {value.Properties.ExploreBonusRate} -> {value.Properties.CaptureRateBonus}");
			value.Properties.ExploreBonusRate = value.Properties.CaptureRateBonus;
			if (value.NurturanceTemplateId == 7)
			{
				value.NurturanceTemplateId = 10;
				Logger.Warn($"Fix Jiao Nurturance: {value.Id},  Shaving -> WrapTeach");
			}
			DomainManager.Extra.SetJiaoEvolutionChoice(value.Id);
			DomainManager.Extra.SetJiao(context, value);
		}
		foreach (ChildrenOfLoong value2 in DomainManager.Extra.GetAllLoongs().Values)
		{
			if ((value2.Properties.Value.Inherited + value2.Properties.Value.Fostered / 100) * 2 < value2.Properties.ExploreBonusRate.Inherited + value2.Properties.ExploreBonusRate.Fostered / 100)
			{
				Logger.Warn($"Fix Loong Value: {value2.Id},{value2.Properties.Value} -> {value2.Properties.ExploreBonusRate}");
				value2.Properties.Value = value2.Properties.ExploreBonusRate;
			}
			Logger.Warn($"Fix Loong ExploreBonusRate: {value2.Id},{value2.Properties.ExploreBonusRate} -> {value2.Properties.CaptureRateBonus}");
			value2.Properties.ExploreBonusRate = value2.Properties.CaptureRateBonus;
			DomainManager.Extra.SetChildrenOfLoong(context, value2);
		}
	}

	private void FixAbnormalJiaoGenerationData(DataContext context)
	{
		foreach (Jiao value in DomainManager.Extra.GetAllJiaos().Values)
		{
			if (value.Generation == 0 && (value.FatherId >= 0 || value.MotherId >= 0))
			{
				value.Generation = 1 + Math.Max(FixAbnormalJiaoGenerationData(context, value.FatherId), FixAbnormalJiaoGenerationData(context, value.MotherId));
				DomainManager.Extra.SetJiao(context, value);
				Logger.Warn($"Fix Jiao Generation: {value.Id}");
			}
		}
	}

	private int FixAbnormalJiaoGenerationData(DataContext context, int jiaoId)
	{
		if (DomainManager.Extra.TryGetJiao(jiaoId, out var jiao))
		{
			if (jiao.Generation == 0 && (jiao.FatherId >= 0 || jiao.MotherId >= 0))
			{
				jiao.Generation = 1 + Math.Max(FixAbnormalJiaoGenerationData(context, jiao.FatherId), FixAbnormalJiaoGenerationData(context, jiao.MotherId));
				DomainManager.Extra.SetJiao(context, jiao);
				Logger.Warn($"Fix Jiao Generation: {jiao.Id}");
			}
			return jiao.Generation;
		}
		return 0;
	}

	private void FixAbnormalJiaoEggAndTeenagerData(DataContext context)
	{
		foreach (Jiao value2 in DomainManager.Extra.GetAllJiaos().Values)
		{
			ItemKey key = value2.Key;
			if (key.ItemType == 12)
			{
				ItemKey value = FixJiaoItemFromMiscToMaterial(context, value2);
				Logger.Warn($"Fixing jiao item from misc to material: {key} => {value}");
			}
		}
	}

	private void FixJiaoKeyInUnhandledCrossArchiveGameData(DataContext context)
	{
		CrossArchiveGameData unhandledCrossArchiveGameData = DomainManager.Extra.UnhandledCrossArchiveGameData;
		if (unhandledCrossArchiveGameData == null || unhandledCrossArchiveGameData.UnpackedItems == null)
		{
			return;
		}
		foreach (int key in unhandledCrossArchiveGameData.UnpackedItems.Keys)
		{
			ItemKey itemKey = unhandledCrossArchiveGameData.UnpackedItems[key];
			sbyte itemType = itemKey.ItemType;
			sbyte b = itemType;
			if (b == 12 && DomainManager.Extra.TryGetJiaoByItemKey(itemKey, out var jiao))
			{
				ItemKey value = FixJiaoItemFromMiscToMaterial(context, jiao);
				unhandledCrossArchiveGameData.UnpackedItems[key] = value;
				Logger.Warn($"Fixing jiao item from misc to material in unhandled cross archive game data: {itemKey} => {value}");
			}
		}
	}

	private void FixInvalidJiaoItems(DataContext context)
	{
		if (!DomainManager.Extra.GetIsDreamBack())
		{
			return;
		}
		bool flag = DomainManager.Extra.IsDreamBackStateUnlocked(1);
		bool flag2 = DomainManager.Extra.IsDreamBackStateUnlocked(4);
		if (!flag || !flag2)
		{
			return;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Inventory inventory = taiwu.GetInventory();
		List<ItemKey> list = new List<ItemKey>();
		ItemKey key;
		int value;
		foreach (KeyValuePair<ItemKey, int> item in inventory.Items)
		{
			item.Deconstruct(out key, out value);
			ItemKey itemKey = key;
			int num = value;
			if (itemKey.ItemType == 12 && itemKey.TemplateId >= 287 && itemKey.TemplateId <= 319 && !DomainManager.Extra.TryGetJiaoByItemKey(itemKey, out var _))
			{
				list.Add(itemKey);
			}
		}
		List<ItemKey> list2 = new List<ItemKey>();
		foreach (KeyValuePair<ItemKey, int> warehouseItem in DomainManager.Taiwu.WarehouseItems)
		{
			warehouseItem.Deconstruct(out key, out value);
			ItemKey itemKey2 = key;
			int num2 = value;
			if (itemKey2.ItemType == 12 && itemKey2.TemplateId >= 287 && itemKey2.TemplateId <= 319 && !DomainManager.Extra.TryGetJiaoByItemKey(itemKey2, out var _))
			{
				list2.Add(itemKey2);
			}
		}
		if (list.Count == 0 && list2.Count == 0)
		{
			return;
		}
		foreach (ItemKey item2 in list)
		{
			if (!DomainManager.Item.ItemExists(item2))
			{
				inventory.OfflineRemove(item2);
			}
			else
			{
				taiwu.RemoveInventoryItem(context, item2, 1, deleteItem: true);
			}
			Logger.Warn($"Fixing invalid jiao item in inventory by removing {item2}.");
		}
		taiwu.SetInventory(inventory, context);
		foreach (ItemKey item3 in list2)
		{
			DomainManager.Taiwu.WarehouseRemove(context, item3, 1, deleteItem: true);
			Logger.Warn($"Fixing invalid jiao item in warehouse by removing {item3}.");
		}
		foreach (KeyValuePair<int, Jiao> jiao4 in DomainManager.Extra.Jiaos)
		{
			jiao4.Deconstruct(out value, out var value2);
			int num3 = value;
			Jiao jiao3 = value2;
			if (jiao3.Key.ItemType == 5 && DomainManager.Item.TryGetElement_Materials(jiao3.Key.Id, out var element) && element.Owner.OwnerType == ItemOwnerType.None)
			{
				taiwu.AddInventoryItem(context, jiao3.Key, 1);
				Logger.Warn($"Adding unowned jiao {jiao3.Key} to taiwu inventory.");
			}
		}
	}

	private ItemKey FixJiaoItemFromMiscToMaterial(DataContext context, Jiao jiao)
	{
		ItemKey key = jiao.Key;
		short templateId = ((jiao.GrowthStage == 0) ? DomainManager.Extra.GetJiaoEggTemplateIdByJiaoTemplateId(jiao.TemplateId) : DomainManager.Extra.GetJiaoTeenagerTemplateIdByJiaoTemplateId(jiao.TemplateId));
		ItemKey itemKey = DomainManager.Item.CreateItem(context, 5, templateId);
		GameData.Domains.Character.Animal animal;
		if (DomainManager.Taiwu.GetItems(ItemSourceType.Inventory).ContainsKey(key))
		{
			DomainManager.Taiwu.RemoveItem(context, key, 1, ItemSourceType.Inventory, deleteItem: false);
			DomainManager.Taiwu.AddItem(context, itemKey, 1, ItemSourceType.Inventory);
		}
		else if (DomainManager.Taiwu.GetItems(ItemSourceType.Warehouse).ContainsKey(key))
		{
			DomainManager.Taiwu.RemoveItem(context, key, 1, ItemSourceType.Warehouse, deleteItem: false);
			DomainManager.Taiwu.AddItem(context, itemKey, 1, ItemSourceType.Warehouse);
		}
		else if (DomainManager.Taiwu.GetItems(ItemSourceType.StockStorageGoodsShelf).ContainsKey(key))
		{
			DomainManager.Taiwu.RemoveItem(context, key, 1, ItemSourceType.StockStorageGoodsShelf, deleteItem: false);
			DomainManager.Taiwu.AddItem(context, itemKey, 1, ItemSourceType.StockStorageGoodsShelf);
		}
		else if (DomainManager.Taiwu.GetItems(ItemSourceType.StockStorageWarehouse).ContainsKey(key))
		{
			DomainManager.Taiwu.RemoveItem(context, key, 1, ItemSourceType.StockStorageWarehouse, deleteItem: false);
			DomainManager.Taiwu.AddItem(context, itemKey, 1, ItemSourceType.StockStorageWarehouse);
		}
		else if (DomainManager.Extra.TryGetAnimalByItemKey(key, out animal))
		{
			animal.ItemKey = itemKey;
			DomainManager.Extra.SetAnimal(context, animal);
			DomainManager.Item.SetOwner(animal.ItemKey, ItemOwnerType.FleeCarrier, animal.Location.AreaId);
		}
		DomainManager.Extra.SetJiaoItemKey(context, jiao.Id, jiao, itemKey);
		DomainManager.Item.RemoveItem(context, key);
		return itemKey;
	}

	private void FixAbnormalJiaoLoongData(DataContext context, ItemKey itemKey)
	{
		if (itemKey.ItemType == 12 && !DomainManager.Extra.TryGetJiaoIdByItemKey(itemKey, out var id))
		{
			short templateId = itemKey.TemplateId;
			if (templateId >= 287 && templateId <= 319)
			{
				DomainManager.Item.RemoveItem(context, itemKey);
			}
		}
		else
		{
			if (itemKey.ItemType != 4)
			{
				return;
			}
			short templateId = itemKey.TemplateId;
			if (templateId >= 46 && templateId <= 76 && !DomainManager.Extra.TryGetJiaoIdByItemKey(itemKey, out id))
			{
				int id2 = DomainManager.Extra.AddJiao(context, itemKey, CarrierToJiaoTemplate[itemKey.TemplateId], 0, 2, isMale: true, isMaxTamePoint: false, isMaxProperty: false);
				Jiao jiaoById = DomainManager.Extra.GetJiaoById(id2);
				jiaoById.Properties = new JiaoProperty(50);
				DomainManager.Extra.SetJiao(context, jiaoById);
				return;
			}
			templateId = itemKey.TemplateId;
			if (templateId >= 77 && templateId <= 85 && !DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(itemKey, out id))
			{
				int id3 = DomainManager.Extra.AddChildOfLoong(context, itemKey, CarrierToJiaoTemplate[itemKey.TemplateId]);
				ChildrenOfLoong childrenOfLoongById = DomainManager.Extra.GetChildrenOfLoongById(id3);
				(int, int) tuple = (context.Random.Next(0, 12), context.Random.Next(12, 8));
				for (short num = 0; num <= 8; num++)
				{
					childrenOfLoongById.Properties.Set(num, Config.JiaoProperty.Instance[num].MaxValue * ((num == tuple.Item1 || num == tuple.Item2) ? 75 : 50));
				}
				childrenOfLoongById.Properties = new JiaoProperty(50);
				DomainManager.Extra.SetChildrenOfLoong(context, childrenOfLoongById);
			}
		}
	}

	private void FixAbnormalChildrenOfLoongData(DataContext context)
	{
		foreach (var (num2, childrenOfLoong2) in DomainManager.Extra.ChildrenOfLoong)
		{
			if (childrenOfLoong2.JiaoTemplateId < 0)
			{
				childrenOfLoong2.JiaoTemplateId = 0;
				childrenOfLoong2.LoongTemplateId = DomainManager.Extra.GetJiaoTemplateIdByCarrierTemplateId(childrenOfLoong2.Key.TemplateId);
				childrenOfLoong2.Gender = false;
				DomainManager.Extra.SetChildrenOfLoong(context, childrenOfLoong2);
			}
		}
	}

	private void FixAbnormalJiaoLoongData(DataContext context)
	{
		ItemKey itemKey = DomainManager.Taiwu.GetTaiwu().GetEquipment()[11];
		if (itemKey.IsValid())
		{
			FixAbnormalJiaoLoongData(context, itemKey);
		}
		IReadOnlyDictionary<ItemKey, int> items = DomainManager.Taiwu.GetItems(ItemSourceType.Inventory);
		foreach (ItemKey key in items.Keys)
		{
			FixAbnormalJiaoLoongData(context, key);
		}
		IReadOnlyDictionary<ItemKey, int> items2 = DomainManager.Taiwu.GetItems(ItemSourceType.Warehouse);
		foreach (ItemKey key2 in items2.Keys)
		{
			FixAbnormalJiaoLoongData(context, key2);
		}
		IReadOnlyDictionary<ItemKey, int> items3 = DomainManager.Taiwu.GetItems(ItemSourceType.StockStorageGoodsShelf);
		foreach (ItemKey key3 in items3.Keys)
		{
			FixAbnormalJiaoLoongData(context, key3);
		}
	}

	private void FixLoongEventWrongState(DataContext context)
	{
		if (DomainManager.Extra.GetFiveLoongDictCount(context) <= 0)
		{
			EventArgBox orCreateDlcArgBox = DomainManager.Extra.GetOrCreateDlcArgBox(2764950uL, context);
			bool arg = false;
			if (orCreateDlcArgBox.Get("ConchShip_PresetKey_FiveLoongDlcBeginMonthlyEventTriggered", ref arg) && arg && !DomainManager.Extra.IsExtraTaskInProgress(264) && DomainManager.Extra.GetFiveLoongDictCount(context) == 0)
			{
				orCreateDlcArgBox.Set("ConchShip_PresetKey_FiveLoongDlcBeginMonthlyEventTriggered", arg: false);
				DomainManager.Extra.SetDlcArgBox(2764950uL, orCreateDlcArgBox, context);
				Logger.Warn("Fix Loong Event Wrong State");
			}
		}
	}

	private void InitializeJiaoData(DataContext context)
	{
		List<JiaoPool> jiaoPoolList = DomainManager.Extra.GetJiaoPoolList();
		List<JiaoPoolRecordList> jiaoPoolRecordList = DomainManager.Extra.GetJiaoPoolRecordList();
		while (jiaoPoolList.Count < 9)
		{
			jiaoPoolList.Add(new JiaoPool());
			jiaoPoolRecordList.Add(new JiaoPoolRecordList());
		}
		JiaoEggDropRate = GlobalConfig.Instance.InitJiaoEggDropRate;
		MaleJiaoEggDropRate = GlobalConfig.Instance.InitMaleJiaoEggDropRate;
		DomainManager.Extra.SetJiaoPools(jiaoPoolList, context);
		DomainManager.Extra.SetJiaoPoolRecords(jiaoPoolRecordList, context);
	}

	private void InitializeMaxTaiwuVillageLevel()
	{
		UpdateMaxTaiwuVillageLevel();
	}

	private void InitializeJiaoCacheData()
	{
		foreach (var (num2, jiao2) in DomainManager.Extra.Jiaos)
		{
			DomainManager.Extra.SetJiaoEvolutionChoice(num2);
			DomainManager.Extra.SetJiaoKeyToId(jiao2.Key, num2);
		}
		DomainManager.Extra.ResetJiaoPoolStatus(DataContextManager.GetCurrentThreadDataContext());
	}

	private void InitializeChildrenOfLoongCacheData()
	{
		foreach (var (id, childrenOfLoong2) in DomainManager.Extra.ChildrenOfLoong)
		{
			DomainManager.Extra.SetChildOfLoongKeyToId(childrenOfLoong2.Key, id);
		}
	}

	private void InitializeConfigCache()
	{
		_carrierToJiaoTemplate = new Dictionary<short, short>();
		foreach (JiaoItem item in (IEnumerable<JiaoItem>)Config.Jiao.Instance)
		{
			if (item.IndexOfCarrierTemplate >= 0)
			{
				_carrierToJiaoTemplate.Add(item.IndexOfCarrierTemplate, item.TemplateId);
			}
		}
	}

	private static bool TryCreateOrReAppearFiveLoong(DataContext context, bool isStrict, short characterTemplateId)
	{
		Tester.Assert(IsCharacterLoong(characterTemplateId), $"Wrong FiveLoong characterTemplateId: {characterTemplateId}");
		List<short> list = new List<short>();
		List<short> list2 = new List<short>();
		List<MapBlockData> list3 = new List<MapBlockData>();
		List<GameData.Domains.Character.Animal> list4 = new List<GameData.Domains.Character.Animal>();
		list.Clear();
		list2.Clear();
		list3.Clear();
		list2.AddRange(DomainManager.Extra.GetFiveLoongStateIds());
		for (short num = 0; num < 135; num++)
		{
			list.Add(num);
		}
		CollectionUtils.Shuffle(context.Random, list);
		for (short num2 = 0; num2 < list.Count; num2++)
		{
			short num3 = list[num2];
			if (num3 != DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId && !DomainManager.Map.IsAreaBroken(num3))
			{
				sbyte stateIdByAreaId = DomainManager.Map.GetStateIdByAreaId(num3);
				if (!list2.Contains(stateIdByAreaId))
				{
					Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(num3);
					Span<MapBlockData> span = areaBlocks;
					for (int i = 0; i < span.Length; i++)
					{
						MapBlockData mapBlockData = span[i];
						if (!IsBlockMeet(mapBlockData, isStrict))
						{
							continue;
						}
						list3.Clear();
						if (isStrict)
						{
							DomainManager.Map.GetNeighborBlocks(num3, mapBlockData.BlockId, list3, 4);
							bool flag = false;
							foreach (MapBlockData item in list3)
							{
								if (!item.IsNonDeveloped())
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								continue;
							}
						}
						list3.Clear();
						DomainManager.Map.GetNeighborBlocks(num3, mapBlockData.BlockId, list3, 3);
						bool flag2 = true;
						if (list3.Count < 24)
						{
							continue;
						}
						foreach (MapBlockData item2 in list3)
						{
							if (!IsBlockMeet(item2))
							{
								flag2 = false;
								break;
							}
						}
						if (!flag2)
						{
							continue;
						}
						Location location = mapBlockData.GetLocation();
						list2.Add(stateIdByAreaId);
						Dictionary<short, short> dictionary = new Dictionary<short, short>();
						foreach (MapBlockData item3 in list3)
						{
							dictionary.Add(item3.BlockId, item3.TemplateId);
						}
						dictionary.Add(mapBlockData.BlockId, mapBlockData.TemplateId);
						short index = LoongInfo.CharacterTemplateIdToLoongTemplateId(characterTemplateId);
						LoongItem loongItem = Loong.Instance[index];
						if (DomainManager.Extra.TryGetElement_FiveLoongDict(characterTemplateId, out var value))
						{
							if (!value.IsDisappear)
							{
								return false;
							}
							RemoveMinionLoongsInArea(context, value.LoongTerrainCenterLocation.AreaId);
							MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
							monthlyNotificationCollection.AddFiveLoongArise(location, characterTemplateId);
							value.LoongTerrainCenterLocation = location;
							value.LoongCurrentLocation = location;
							value.CoveredMapBlockTemplateId = dictionary;
							value.IsDisappear = false;
							value.MapBlockExtraItems.Clear();
							DomainManager.Extra.SetLoongInfo(context, characterTemplateId, value);
						}
						else
						{
							DomainManager.Extra.SetLoongInfo(context, characterTemplateId, value = new LoongInfo(characterTemplateId, location, dictionary));
						}
						MapBlockToLoong(context, mapBlockData, mapBlockData, loongItem.MapBlock);
						DomainManager.Extra.CreateAnimalByCharacterTemplateId(context, characterTemplateId, location);
						DomainManager.Extra.TriggerExtraTask(context, 46, 265);
						DomainManager.Extra.TriggerExtraTask(context, 46, loongItem.Task);
						int num4 = 0;
						int[] array = new int[18];
						int[] array2 = new int[3];
						for (int j = 0; j < array.Length; j++)
						{
							array[j] = context.Random.Next(0, list3.Count);
						}
						for (int k = 0; k < array2.Length; k++)
						{
							array2[k] = context.Random.Next(0, list3.Count);
						}
						list4.Clear();
						CollectionUtils.Shuffle(context.Random, list3);
						for (int l = 0; l < list3.Count; l++)
						{
							MapBlockData mapBlockData2 = list3[l];
							Location location2 = mapBlockData2.GetLocation();
							MapBlockToLoong(context, mapBlockData2, mapBlockData, loongItem.MapBlock);
							DomainManager.Map.ClearBlockRandomEnemies(context, mapBlockData2);
							DomainManager.Map.SetBlockData(context, mapBlockData2);
							bool flag3 = false;
							for (int m = 0; m < array.Length; m++)
							{
								if (array[m] == l)
								{
									LoongInfo loongInfo = value;
									if (loongInfo.MapBlockExtraItems == null)
									{
										loongInfo.MapBlockExtraItems = new Dictionary<Location, Inventory>();
									}
									if (!value.MapBlockExtraItems.TryGetValue(location2, out var value2))
									{
										value.MapBlockExtraItems.Add(location2, value2 = new Inventory());
									}
									value2.OfflineAdd(new ItemKey(12, 0, 286, 0), 1);
									flag3 = true;
								}
							}
							for (int n = 0; n < array2.Length; n++)
							{
								if (array2[n] == l)
								{
									LoongInfo loongInfo = value;
									if (loongInfo.MapBlockExtraItems == null)
									{
										loongInfo.MapBlockExtraItems = new Dictionary<Location, Inventory>();
									}
									if (!value.MapBlockExtraItems.TryGetValue(location2, out var value3))
									{
										value.MapBlockExtraItems.Add(location2, value3 = new Inventory());
									}
									value3.OfflineAdd(new ItemKey(5, 0, 280, 0), 1);
									flag3 = true;
								}
							}
							if (flag3)
							{
								DomainManager.Extra.SetLoongInfo(context, characterTemplateId, value);
							}
							if (DomainManager.Extra.TryGetAnimalIdsByLocation(new Location(num3, location2.BlockId), out var animals))
							{
								foreach (int item4 in animals)
								{
									if (DomainManager.Extra.TryGetAnimal(item4, out var animal) && !IsCharacterMinionLoong(animal.CharacterTemplateId) && !IsCharacterLoong(animal.CharacterTemplateId))
									{
										list4.Add(animal);
									}
								}
							}
							if (num4 < GlobalConfig.Instance.FiveLoongDlcMinionLoongMaxCount)
							{
								MapBlockData blockData = DomainManager.Map.GetBlockData(mapBlockData2.AreaId, mapBlockData2.BlockId);
								MapBlockItem mapBlockItem = MapBlock.Instance[blockData.TemplateId];
								if (mapBlockItem.SubType == EMapBlockSubType.DLCLoong)
								{
									num4++;
									DomainManager.Extra.CreateAnimalByCharacterTemplateId(context, loongItem.MinionCharTemplateId, location2);
								}
							}
						}
						foreach (GameData.Domains.Character.Animal item5 in list4)
						{
							Location location3;
							if (DomainManager.Extra.IsAnimalAbleToAttack(item5, isTaiwuVictim: false))
							{
								DomainManager.Extra.RemoveAnimal(context, item5);
							}
							else if (TryGetValidAnimalMoveLocation(context, num3, list3, out location3))
							{
								DomainManager.Extra.SetAnimalLocation(context, item5, location3);
							}
						}
						return true;
					}
				}
			}
		}
		return false;
		static bool IsBlockMeet(MapBlockData mapBlockData3, bool avoidBigBlock = false)
		{
			MapBlockItem config = mapBlockData3.GetConfig();
			if (!mapBlockData3.IsNonDeveloped() || mapBlockData3.Destroyed || config.TemplateId == 126)
			{
				return false;
			}
			if (avoidBigBlock && config.Size > 1)
			{
				return false;
			}
			return true;
		}
	}

	public static void MapBlockToLoong(DataContext context, MapBlockData block, MapBlockData center, short templateId)
	{
		block = block.GetRootBlock();
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		if (block.GetConfig().Size <= 1)
		{
			list.Add(block);
		}
		else
		{
			list.AddRange(block.GroupBlockList);
			DomainManager.Map.SplitMultiBlock(context, block);
		}
		ByteCoordinate blockPos = center.GetBlockPos();
		foreach (MapBlockData item in list)
		{
			Tester.Assert(item.GetConfig().Size == 1, "targetBlock.GetConfig().Size == 1");
			if (item.GetManhattanDistanceToPos(blockPos.X, blockPos.Y) <= 3)
			{
				DomainManager.Map.ChangeBlockTemplate(context, item, templateId);
				DomainManager.Map.DestroyMapBlockItemsDirect(context, item);
				DomainManager.Map.SetBlockAndViewRangeVisible(context, item.AreaId, item.BlockId);
			}
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
	}

	public static bool TryGetValidAnimalMoveLocation(DataContext context, short areaId, List<MapBlockData> invalidBlocks, out Location location)
	{
		location = Location.Invalid;
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		for (int i = 0; i < areaBlocks.Length; i++)
		{
			MapBlockData mapBlockData = areaBlocks[i];
			if (mapBlockData.Visible && mapBlockData.IsNonDeveloped() && mapBlockData.IsPassable() && (invalidBlocks == null || !invalidBlocks.Contains(mapBlockData)))
			{
				list.Add(mapBlockData);
			}
		}
		if (list.Count == 0)
		{
			return false;
		}
		short blockId = list.GetRandom(context.Random).BlockId;
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		location = new Location(areaId, blockId);
		return true;
	}

	public static void CreateAllFiveLoong(DataContext context)
	{
		if (DomainManager.Extra.FiveLoongDict.Count >= Loong.Instance.Count)
		{
			return;
		}
		foreach (LoongItem item in (IEnumerable<LoongItem>)Loong.Instance)
		{
			if (!TryCreateOrReAppearFiveLoong(context, isStrict: true, item.CharTemplateId))
			{
				TryCreateOrReAppearFiveLoong(context, isStrict: false, item.CharTemplateId);
			}
		}
	}

	public static int DefeatFiveLoong(DataContext context, short characterTemplateId)
	{
		Tester.Assert(characterTemplateId >= 686 && characterTemplateId <= 690, $"FiveLoong Wrong characterTemplateId: {characterTemplateId}");
		if (DomainManager.Extra.TryGetElement_FiveLoongDict(characterTemplateId, out var value))
		{
			Location loongTerrainCenterLocation = value.LoongTerrainCenterLocation;
			if (value.CoveredMapBlockTemplateId != null)
			{
				foreach (KeyValuePair<short, short> item in value.CoveredMapBlockTemplateId)
				{
					item.Deconstruct(out var key, out var value2);
					short blockId = key;
					short blockTemplateId = value2;
					Location location = new Location(loongTerrainCenterLocation.AreaId, blockId);
					DomainManager.Map.ChangeBlockTemplate(context, location, blockTemplateId, isTurnVisible: true);
					DomainManager.Map.SetBlockAndViewRangeVisible(context, location.AreaId, location.BlockId);
				}
				value.CoveredMapBlockTemplateId.Clear();
			}
			DomainManager.Extra.RemoveAnimalByLocationAndTemplateId(context, value.LoongCurrentLocation, characterTemplateId);
			short taskInfoId = (short)(characterTemplateId - 686 + 268);
			DomainManager.Extra.FinishTriggeredExtraTask(context, 46, taskInfoId);
			ushort value3;
			bool flag = value.CharacterDebuffCounts != null && value.CharacterDebuffCounts.TryGetValue(DomainManager.Taiwu.GetTaiwuCharId(), out value3) && value3 > 0;
			if (value.CharacterDebuffCounts != null)
			{
				value.CharacterDebuffCounts.Clear();
			}
			if (flag)
			{
				AddLoongDebuffInstantNotification(value.ConfigData.DebuffCountDecNotification, 0);
			}
			value.IsDisappear = true;
			value.DisappearDate = DomainManager.World.GetCurrDate();
			DomainManager.Extra.SetLoongInfo(context, characterTemplateId, value);
			return RemoveMinionLoongsInArea(context, value.LoongCurrentLocation.AreaId);
		}
		return 0;
	}

	private static int RemoveMinionLoongsInArea(DataContext context, short areaId)
	{
		if (DomainManager.Extra.TryGetAnimalAreaDataByAreaId(areaId, out var animalAreaData))
		{
			List<GameData.Domains.Character.Animal> list = new List<GameData.Domains.Character.Animal>();
			foreach (List<int> value in animalAreaData.Values)
			{
				foreach (int item in value)
				{
					if (DomainManager.Extra.TryGetAnimal(item, out var animal) && IsCharacterMinionLoong(animal.CharacterTemplateId))
					{
						list.Add(animal);
					}
				}
			}
			foreach (GameData.Domains.Character.Animal item2 in list)
			{
				DomainManager.Extra.RemoveAnimal(context, item2);
			}
			return list.Count;
		}
		return 0;
	}

	void IDlcEntry.OnLoadedArchiveData()
	{
		InitializeConfigCache();
		InitializeJiaoCacheData();
		InitializeChildrenOfLoongCacheData();
		Events.RegisterHandler_TaiwuMove(OnTaiwuMove);
		Events.RegisterHandler_CombatSettlement(OnDefeatFiveLoongInCombat);
	}

	void IDlcEntry.OnEnterNewWorld()
	{
		InitializeConfigCache();
		InitializeJiaoData(DataContextManager.GetCurrentThreadDataContext());
		InitializeJiaoCacheData();
		InitializeChildrenOfLoongCacheData();
		Events.RegisterHandler_TaiwuMove(OnTaiwuMove);
		Events.RegisterHandler_CombatSettlement(OnDefeatFiveLoongInCombat);
	}

	void IDlcEntry.OnPostAdvanceMonth(DataContext context)
	{
		PostAdvanceMonth_Main(context);
		PostAdvanceMonth_ChildOfLoong(context);
		PostAdvanceMonth_JiaoPool(context);
		PostAdvanceMonth_JiaoPoolLog(context);
		PostAdvanceMonth_FiveLoongs(context);
	}

	void IDlcEntry.FixAbnormalArchiveData(DataContext context)
	{
		if (DomainManager.Extra.GetJiaoPools().Count < 9)
		{
			InitializeJiaoData(context);
		}
		if (MaxTaiwuVillageLevel <= 0)
		{
			InitializeMaxTaiwuVillageLevel();
		}
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 69, 39))
		{
			FixAbnormalChildrenOfLoongData(context);
		}
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 69, 45))
		{
			FixAbnormalJiaoLoongData(context);
		}
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 70, 47))
		{
			if (DomainManager.World.IsCurrWorldSavedWithVersion(0, 0, 70, 46))
			{
				FixInvalidJiaoItems(context);
			}
			FixJiaoKeyInUnhandledCrossArchiveGameData(context);
			FixAbnormalJiaoEggAndTeenagerData(context);
		}
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 70, 34))
		{
			FixAbnormalJiaoGenerationData(context);
		}
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 70, 54))
		{
			FixAbnormalJiaoNextPeriodData(context);
		}
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 79))
		{
			FixAbnormalJiaoPriceData(context);
		}
		FixLoongEventWrongState(context);
	}

	public void OnCrossArchive(DataContext context, IDlcEntry entryBeforeCrossArchive)
	{
		FiveLoongDlcEntry fiveLoongDlcEntry = (FiveLoongDlcEntry)entryBeforeCrossArchive;
		MaleJiaoEggDropRate = fiveLoongDlcEntry.MaleJiaoEggDropRate;
		OwnedChildrenOfLoong = fiveLoongDlcEntry.OwnedChildrenOfLoong;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 27;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 7;
		ptr += 2;
		*(int*)ptr = ChildrenOfLoongMonthlyEventChance;
		ptr += 4;
		*(int*)ptr = PulaoCricketLuckPoint;
		ptr += 4;
		*(int*)ptr = JiaoEggDropRate;
		ptr += 4;
		*(int*)ptr = MaleJiaoEggDropRate;
		ptr += 4;
		*ptr = (IsJiaoPoolOpen ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = OwnedChildrenOfLoong;
		ptr += 4;
		*(int*)ptr = MaxTaiwuVillageLevel;
		ptr += 4;
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
			ChildrenOfLoongMonthlyEventChance = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			PulaoCricketLuckPoint = *(int*)ptr;
			ptr += 4;
		}
		if (num > 2)
		{
			JiaoEggDropRate = *(int*)ptr;
			ptr += 4;
		}
		if (num > 3)
		{
			MaleJiaoEggDropRate = *(int*)ptr;
			ptr += 4;
		}
		if (num > 4)
		{
			IsJiaoPoolOpen = *ptr != 0;
			ptr++;
		}
		if (num > 5)
		{
			OwnedChildrenOfLoong = *(int*)ptr;
			ptr += 4;
		}
		if (num > 6)
		{
			MaxTaiwuVillageLevel = *(int*)ptr;
			ptr += 4;
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
