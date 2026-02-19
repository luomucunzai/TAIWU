using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DLC;
using GameData.DLC.Shared;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.InteractOfLoveEventHelper;

public class InteractOfLoveEventHelper
{
	public static readonly int[] TownBlockArray = new int[18]
	{
		62, 0, 1, 2, 3, 4, 5, 6, 7, 8,
		9, 10, 11, 12, 13, 14, 15, 16
	};

	private static TaiwuEventDomain Domain => DomainManager.TaiwuEvent;

	public static bool CheckDateNpcOptionIsVisible(int charId)
	{
		if (!GameData.Domains.TaiwuEvent.EventHelper.EventHelper.CheckMainStoryLineProgress(6))
		{
			return false;
		}
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			throw new Exception($"can not get character, Id = {charId}");
		}
		if (!DomainManager.Character.TryGetElement_Objects(DomainManager.Taiwu.GetTaiwuCharId(), out var element2))
		{
			throw new Exception($"can not get character, Id = {DomainManager.Taiwu.GetTaiwuCharId()}");
		}
		if (GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GetRoleAge(element2) >= 16 && GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GetRoleAge(element) >= 16 && GameData.Domains.TaiwuEvent.EventHelper.EventHelper.CheckHasRelationship(element2, element, 8192))
		{
			if (GameData.Domains.TaiwuEvent.EventHelper.EventHelper.CheckHasRelationship(element2, element, 2048) || GameData.Domains.TaiwuEvent.EventHelper.EventHelper.CheckHasRelationship(element, element2, 2048))
			{
				return false;
			}
			if (GameData.Domains.TaiwuEvent.EventHelper.EventHelper.HasNominalBloodRelation(element2.GetId(), element.GetId()))
			{
				return false;
			}
			if (GameData.Domains.TaiwuEvent.EventHelper.EventHelper.HasBloodExclusionRelation(element2.GetId(), element.GetId()) && !GameData.Domains.TaiwuEvent.EventHelper.EventHelper.CheckHasRelationship(element2, element, 1024))
			{
				return false;
			}
			if (GameData.Domains.TaiwuEvent.EventHelper.EventHelper.CheckHasRelationship(element2, element, 16384) && GameData.Domains.TaiwuEvent.EventHelper.EventHelper.CheckHasRelationship(element, element2, 16384))
			{
				return true;
			}
			if (GameData.Domains.TaiwuEvent.EventHelper.EventHelper.CheckHasRelationship(element2, element, 1024))
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public static bool IsSaveInteractTime(int charId)
	{
		LoveDataItem loveDataItem;
		return InteractOfLove.IsInstalled() && InteractOfLove.TryGetLoveDataItem(charId, out loveDataItem);
	}

	public static bool IsLover(int charId)
	{
		return InteractOfLove.IsInstalled() && DomainManager.Extra.IsTaiwuLover(charId);
	}

	public static bool IsPreviousLover(int charId)
	{
		return InteractOfLove.IsInstalled() && DomainManager.Extra.IsPreviousTaiwuLover(charId);
	}

	public static bool IsLoveToken(ItemKey itemKey)
	{
		if (!InteractOfLove.IsInstalled())
		{
			return false;
		}
		return DomainManager.Extra.IsLoveToken(itemKey);
	}

	public static bool IsCurrentLoveToken(ItemKey itemKey, int charId)
	{
		if (!InteractOfLove.IsInstalled())
		{
			return false;
		}
		LoveTokenDataItem loveTokenDataItem;
		return DomainManager.Extra.TryGetLoveTokenData(itemKey, out loveTokenDataItem) && loveTokenDataItem.LoverCharId == charId;
	}

	public unsafe static bool IsPreviousLoveToken(ItemKey itemKey, int charId)
	{
		if (!InteractOfLove.IsInstalled())
		{
			return false;
		}
		if (DomainManager.Extra.TryGetLoveTokenData(itemKey, out var loveTokenDataItem))
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
			PreexistenceCharIds preexistenceCharIds = element_Objects.GetPreexistenceCharIds();
			int i = 0;
			for (int count = preexistenceCharIds.Count; i < count; i++)
			{
				int num = preexistenceCharIds.CharIds[i];
				if (num == loveTokenDataItem.LoverCharId)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static sbyte GetConfessionLoveResult(int charId)
	{
		if (DomainManager.Extra.TryGetInteractOfLoveData(charId, out var _))
		{
			return 2;
		}
		if (DomainManager.Extra.IsConfessLoveFailedCharacter(charId))
		{
			return 1;
		}
		return 0;
	}

	public static void SetConfessionLoveResult(int charId, sbyte result)
	{
		DataContext mainThreadDataContext = Domain.MainThreadDataContext;
		if (result == 1)
		{
			DomainManager.Extra.AddConfessLoveFailedCharacter(mainThreadDataContext, charId);
			if (InteractOfLove.TryGetLoveDataItem(charId, out var _))
			{
				InteractOfLove.LeaveLover(mainThreadDataContext, charId);
			}
		}
		if (result == 2)
		{
			DomainManager.Extra.AddConfessLoveFailedCharacter(mainThreadDataContext, charId);
			if (!InteractOfLove.TryGetLoveDataItem(charId, out var _))
			{
				InteractOfLove.BecomeLover(mainThreadDataContext, charId);
			}
		}
	}

	public static int GetDateNpcCount(int charId)
	{
		if (!InteractOfLove.TryGetLoveDataItem(charId, out var loveDataItem))
		{
			return 0;
		}
		return loveDataItem.DateCount;
	}

	public static void SetDateNpcCount(int charId, int count)
	{
		if (InteractOfLove.TryGetLoveDataItem(charId, out var loveDataItem))
		{
			loveDataItem.DateCount = count;
			DomainManager.Extra.SetInteractOfLoveData(Domain.MainThreadDataContext, charId, loveDataItem);
		}
	}

	public static string GetTaiwuNickname(int charId)
	{
		if (!InteractOfLove.TryGetLoveDataItem(charId, out var loveDataItem))
		{
			return "";
		}
		return loveDataItem.TaiwuNicknameId.ToString();
	}

	public static void SetTaiwuNickname(int charId, int nickName)
	{
		if (!InteractOfLove.TryGetLoveDataItem(charId, out var loveDataItem))
		{
			loveDataItem.TaiwuNicknameId = nickName;
			DomainManager.Extra.SetInteractOfLoveData(Domain.MainThreadDataContext, charId, loveDataItem);
		}
	}

	public static void SetTaiwuNickname(int charId, string nickName)
	{
		int nickName2 = DomainManager.World.RegisterCustomText(Domain.MainThreadDataContext, nickName);
		SetTaiwuNickname(charId, nickName2);
	}

	public static int GetInteractTime(int charId)
	{
		if (InteractOfLove.TryGetLoveDataItem(charId, out var loveDataItem))
		{
			return loveDataItem.InteractTime;
		}
		return -1;
	}

	public static void SetInteractTime(int charId, int time)
	{
		DataContext mainThreadDataContext = Domain.MainThreadDataContext;
		if (!InteractOfLove.TryGetLoveDataItem(charId, out var loveDataItem))
		{
			InteractOfLove.BecomeLover(mainThreadDataContext, charId);
			SetInteractTime(charId, time);
		}
		else
		{
			loveDataItem.InteractTime = time;
			DomainManager.Extra.SetInteractOfLoveData(mainThreadDataContext, charId, loveDataItem);
		}
	}

	public static void SetEventTimeDict(int charId, Dictionary<sbyte, int> dict)
	{
		DataContext mainThreadDataContext = Domain.MainThreadDataContext;
		if (!InteractOfLove.TryGetLoveDataItem(charId, out var loveDataItem))
		{
			InteractOfLove.BecomeLover(mainThreadDataContext, charId);
			SetEventTimeDict(charId, dict);
		}
		else
		{
			loveDataItem.EventTimeDict = dict;
			DomainManager.Extra.SetInteractOfLoveData(mainThreadDataContext, charId, loveDataItem);
		}
	}

	public static Dictionary<sbyte, int> GetEventTimeDict(int charId)
	{
		if (InteractOfLove.TryGetLoveDataItem(charId, out var loveDataItem))
		{
			return loveDataItem.EventTimeDict;
		}
		return null;
	}

	public static void SaveEventTimeDict(int charId, sbyte eventId, int time)
	{
		Dictionary<sbyte, int> dictionary = GetEventTimeDict(charId);
		if (dictionary == null)
		{
			dictionary = new Dictionary<sbyte, int>();
		}
		if (!dictionary.ContainsKey(eventId))
		{
			dictionary.Add(eventId, time);
		}
		else
		{
			dictionary[eventId] = time;
		}
		SetEventTimeDict(charId, dictionary);
	}

	public static void SetLoveToken(int charId, ItemKey taiwuOwnedToken, ItemKey loverOwnedToken)
	{
		DataContext mainThreadDataContext = Domain.MainThreadDataContext;
		if (!InteractOfLove.TryGetLoveDataItem(charId, out var loveDataItem))
		{
			InteractOfLove.BecomeLover(mainThreadDataContext, charId);
			SetLoveToken(charId, taiwuOwnedToken, loverOwnedToken);
		}
		else
		{
			loveDataItem.TaiwuOwnedToken = taiwuOwnedToken;
			loveDataItem.LoverOwnedToken = loverOwnedToken;
			DomainManager.Extra.SetInteractOfLoveData(mainThreadDataContext, charId, loveDataItem);
		}
	}

	public static ItemKey ExchangeLoveToken(int charId, ItemKey selectItemKey)
	{
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			throw new Exception($"can not get character,Id = {charId}");
		}
		if (!DomainManager.Character.TryGetElement_Objects(DomainManager.Taiwu.GetTaiwuCharId(), out var element2))
		{
			throw new Exception($"can not get character,Id = {DomainManager.Taiwu.GetTaiwuCharId()}");
		}
		sbyte consummateLevel = element.GetConsummateLevel();
		sbyte grade = (sbyte)Math.Clamp(consummateLevel / 2, 0, 8);
		ItemKey randomItemByGrade = DomainManager.Building.GetRandomItemByGrade(Domain.MainThreadDataContext.Random, grade, -1);
		randomItemByGrade = DomainManager.Item.CreateItem(Domain.MainThreadDataContext, randomItemByGrade.ItemType, randomItemByGrade.TemplateId);
		element2.AddInventoryItem(Domain.MainThreadDataContext, randomItemByGrade, 1);
		element2.RemoveInventoryItem(Domain.MainThreadDataContext, selectItemKey, 1, deleteItem: false);
		element.AddInventoryItem(Domain.MainThreadDataContext, selectItemKey, 1);
		InteractOfLove.SetLoveToken(Domain.MainThreadDataContext, charId, randomItemByGrade, selectItemKey);
		return randomItemByGrade;
	}

	public static ItemKey SendBirthdayGift(int charId, int targetId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			throw new Exception($"can not get character,Id = {charId}");
		}
		if (!DomainManager.Character.TryGetElement_Objects(DomainManager.Taiwu.GetTaiwuCharId(), out var element2))
		{
			throw new Exception($"can not get character,Id = {DomainManager.Taiwu.GetTaiwuCharId()}");
		}
		ItemKey itemKey = GameData.Domains.TaiwuEvent.EventHelper.EventHelper.SelectCharacterItemByGrade(charId, 6, 8, includeTransferable: false, -1, -1);
		if (itemKey.Equals(ItemKey.Invalid))
		{
			itemKey = GameData.Domains.TaiwuEvent.EventHelper.EventHelper.SelectCharacterTopGradeItem(charId);
		}
		if (itemKey.Equals(ItemKey.Invalid))
		{
			throw new Exception("item can not invalid");
		}
		element.RemoveInventoryItem(Domain.MainThreadDataContext, itemKey, 1, deleteItem: false);
		element2.AddInventoryItem(Domain.MainThreadDataContext, itemKey, 1);
		GameData.Domains.TaiwuEvent.EventHelper.EventHelper.ShowGetItemPageForItems(new List<(ItemKey, int)> { (itemKey, 1) });
		return itemKey;
	}

	public static MapBlockData GetCharacterNearBlockByType(GameData.Domains.Character.Character character, List<int> mapBlockSubTypeList)
	{
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		Location location = character.GetLocation();
		MapBlockData mapBlockData = GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GetMapBlockData(location.AreaId, location.BlockId);
		if (mapBlockSubTypeList.Contains((int)mapBlockData.BlockSubType))
		{
			return mapBlockData;
		}
		MapBlockData mapBlockData2 = null;
		for (int i = 3; i < 30; i += 2)
		{
			DomainManager.Map.GetNeighborBlocks(location.AreaId, location.BlockId, list, i);
			for (int j = 0; j < list.Count; j++)
			{
				if (mapBlockSubTypeList.Contains((int)list[j].GetConfig().SubType))
				{
					mapBlockData2 = list[j];
					break;
				}
			}
			if (mapBlockData2 != null)
			{
				break;
			}
		}
		if (mapBlockData2 == null)
		{
			ObjectPool<List<MapBlockData>>.Instance.Return(list);
			return null;
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		return mapBlockData2;
	}

	public static MapBlockData CharacterMoveToNearBlockByType(int charId, int mapBlockSubType)
	{
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			throw new Exception($"can not get character,Id = {charId}");
		}
		if (element == null)
		{
			return null;
		}
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		if (mapBlockSubType == 62)
		{
			list.AddRange(TownBlockArray);
		}
		else
		{
			list.Add(mapBlockSubType);
		}
		MapBlockData characterNearBlockByType = GetCharacterNearBlockByType(element, list);
		ObjectPool<List<int>>.Instance.Return(list);
		if (characterNearBlockByType == null)
		{
			return null;
		}
		if (charId == DomainManager.Taiwu.GetTaiwuCharId())
		{
			GameData.Domains.TaiwuEvent.EventHelper.EventHelper.TeleportMoveTaiwuToBlock(characterNearBlockByType.GetLocation().BlockId);
		}
		else
		{
			DomainManager.Character.GroupMove(Domain.MainThreadDataContext, element, characterNearBlockByType.GetLocation());
		}
		return characterNearBlockByType;
	}

	public static void AddStartRelationSuccessRate(int charId, int targetId, sbyte actionType, sbyte actionSubType, short rateAdjust, int duration)
	{
		DomainManager.Extra.AddAiActionSuccessRateAdjust(Domain.MainThreadDataContext, charId, targetId, actionType, actionSubType, rateAdjust, duration);
	}

	public static short GetAiActionSuccessRateAdjust(int charId, int targetId, sbyte actionType, sbyte actionSubType)
	{
		return DomainManager.Extra.GetAiActionSuccessRateAdjust(charId, targetId, actionType, actionSubType);
	}

	public static void AddStartRelationSuccessRate_BoyOrGirlFriend(int charId, int targetId, short rateAdjust, int duration)
	{
		short num = GetAiActionSuccessRateAdjust(charId, targetId, 10, RelationType.GetTypeId(16384));
		if (num < 100)
		{
			num += rateAdjust;
		}
		AddStartRelationSuccessRate(charId, targetId, 10, RelationType.GetTypeId(16384), num, duration);
	}

	public static void SetBindSamsara(int charId, bool isBindSamsara)
	{
		if (InteractOfLove.TryGetLoveDataItem(charId, out var loveDataItem))
		{
			loveDataItem.IsBindSamsara = isBindSamsara;
			DomainManager.Extra.SetInteractOfLoveData(Domain.MainThreadDataContext, charId, loveDataItem);
		}
	}

	public static sbyte GetCanHappenDateEvent(int charId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			throw new Exception($"can not get character,Id = {charId}");
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Dictionary<sbyte, int> eventTimeDict = GetEventTimeDict(charId);
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		sbyte favorabilityType = GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GetFavorabilityType(element, taiwu);
		if (!GameData.Domains.TaiwuEvent.EventHelper.EventHelper.CheckHasRelationship(taiwu, element, 1024))
		{
			if (favorabilityType >= 4)
			{
				list.AddRange(LoveEventId.UnmarriedFavorite4DateEvents);
			}
			if (favorabilityType >= 5)
			{
				list.AddRange(LoveEventId.UnmarriedFavorite5DateEvents);
			}
			if (favorabilityType >= 6)
			{
				list.AddRange(LoveEventId.UnmarriedFavorite6DateEvents);
			}
		}
		else
		{
			if (favorabilityType >= 4)
			{
				list.AddRange(LoveEventId.MarriedFavorite4DateEvents);
			}
			if (favorabilityType >= 5)
			{
				list.AddRange(LoveEventId.MarriedFavorite5DateEvents);
			}
			if (favorabilityType >= 6)
			{
				list.AddRange(LoveEventId.MarriedFavorite6DateEvents);
			}
			for (int i = 0; i < LoveEventId.MarriedNeedConditionDateEvents.Length; i++)
			{
				if (LoveEventId.MarriedNeedConditionDateEvents[i].Item1 <= favorabilityType && eventTimeDict.ContainsKey(LoveEventId.MarriedNeedConditionDateEvents[i].Item2))
				{
					list.Add(LoveEventId.MarriedNeedConditionDateEvents[i].Item3);
				}
			}
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
		return list[Domain.MainThreadDataContext.Random.Next(list.Count)];
	}

	public static string GetChatJumpEvent(GameData.Domains.Character.Character taiwu, GameData.Domains.Character.Character character)
	{
		int id = character.GetId();
		if (GameData.Domains.TaiwuEvent.EventHelper.EventHelper.CheckRoleInjured(taiwu))
		{
			if (character.GetBehaviorType() == 0)
			{
				return "33d4f6ec-bb8f-4a38-9bb8-59d11fd41ad6";
			}
			if (character.GetBehaviorType() == 1)
			{
				return "42e4338d-d95c-46f6-a6bd-050fdba73607";
			}
			if (character.GetBehaviorType() == 2)
			{
				return "8610d2a3-6fbd-4c6a-9bef-25e45f33c0c0";
			}
			if (character.GetBehaviorType() == 3)
			{
				return "7bd86493-caa8-46ad-88ab-dba7923a9661";
			}
			return "65616bcc-568a-4384-b33f-4e8faf7f8490";
		}
		if (GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GetRoleDisorderOfQi(taiwu) > 0)
		{
			if (character.GetBehaviorType() == 0)
			{
				return "8c0f68b8-761e-4898-9625-ca3208a5eeeb";
			}
			if (character.GetBehaviorType() == 1)
			{
				return "cf77ec99-828d-4189-a969-c4fd85b4e0bd";
			}
			if (character.GetBehaviorType() == 2)
			{
				return "8ce39363-030d-4703-ad4b-8b61aad772e6";
			}
			if (character.GetBehaviorType() == 3)
			{
				return "24c4e0ce-4df5-47f9-aae1-ca42b8bdae4a";
			}
			return "33ca2048-31ba-4171-b0c3-672214f44187";
		}
		if (GameData.Domains.TaiwuEvent.EventHelper.EventHelper.CheckCharacterIsPoisoning(taiwu))
		{
			if (character.GetBehaviorType() == 0)
			{
				return "46a1b716-f7a5-4d58-a164-c64e32b53a13";
			}
			if (character.GetBehaviorType() == 1)
			{
				return "e42b212f-9d2f-42c3-9493-ec0f000d2215";
			}
			if (character.GetBehaviorType() == 2)
			{
				return "b6831032-fda5-4ac8-b119-d4efc13914f1";
			}
			if (character.GetBehaviorType() == 3)
			{
				return "f177703e-634f-4675-b53f-6a8a862200e2";
			}
			if (character.GetBehaviorType() == 4)
			{
				return "5209ed50-0677-4974-85ce-1dc0ade1ed5b";
			}
			return string.Empty;
		}
		int currDate = DomainManager.World.GetCurrDate();
		Dictionary<sbyte, int> eventTimeDict = GetEventTimeDict(id);
		if (eventTimeDict == null || !eventTimeDict.ContainsKey(0) || eventTimeDict[0] == currDate)
		{
			return "cf53fcbf-f322-43fe-9003-8b4cced7ee5c";
		}
		SaveEventTimeDict(id, 0, currDate);
		if (!GameData.Domains.TaiwuEvent.EventHelper.EventHelper.CheckMainStoryLineProgress(8))
		{
			return "cf53fcbf-f322-43fe-9003-8b4cced7ee5c";
		}
		if (!GameData.Domains.TaiwuEvent.EventHelper.EventHelper.TryGetFixedCharacterByTemplateId(446, out var _))
		{
			if (character.GetBehaviorType() == 0)
			{
				return "3d05e502-0b26-4ba4-8efb-9d0384207638";
			}
			if (character.GetBehaviorType() == 1)
			{
				return "231b4406-a564-4d74-ba09-52d9fe2401de";
			}
			if (character.GetBehaviorType() == 2)
			{
				return "3edd5756-9c21-4cfe-afd4-62b4a5fc412f";
			}
			if (character.GetBehaviorType() == 3)
			{
				return "b0956f71-1303-42a9-90db-3955a25e5cf3";
			}
			return "f234b08f-7ad4-4a62-92e7-9e8b1795eae6";
		}
		if (!GameData.Domains.TaiwuEvent.EventHelper.EventHelper.CheckMainStoryLineProgress(28))
		{
			if (character.GetBehaviorType() == 0)
			{
				return "b427c3b4-d1c4-48c1-8649-36490f4faee8";
			}
			if (character.GetBehaviorType() == 1)
			{
				return "b50a0bba-c478-4d54-b308-26e7dbdb3d32";
			}
			if (character.GetBehaviorType() == 2)
			{
				return "89ab1413-1e8f-469e-8e83-dcdcdebf931c";
			}
			if (character.GetBehaviorType() == 3)
			{
				return "e2987e91-bd5b-4fc0-9565-2ded89b5805b";
			}
			return "6902a6f2-84c7-451c-98bb-35595a2d253e";
		}
		if (character.GetBehaviorType() == 0)
		{
			return "e0dc16b3-f81c-4d53-bf27-c8ad02f42aa4";
		}
		if (character.GetBehaviorType() == 1)
		{
			return "3e40c1b3-6acd-4f68-946f-d7051eacfb39";
		}
		if (character.GetBehaviorType() == 2)
		{
			return "132cd56f-9635-47d4-8aa6-7338794b09c5";
		}
		if (character.GetBehaviorType() == 3)
		{
			return "27cf3537-5104-4073-9fe4-56c50ab680ad";
		}
		return "a2a2650c-04bb-49fc-abda-f7d1df5195d1";
	}
}
