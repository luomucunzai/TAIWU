using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Utilities;
using NLog;

namespace GameData.DLC;

public static class InteractOfLove
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public static bool IsInstalled()
	{
		return DlcManager.IsDlcInstalled(2305890uL);
	}

	public static bool TryGetLoveDataItem(int charId, out LoveDataItem loveDataItem)
	{
		return DomainManager.Extra.TryGetInteractOfLoveData(charId, out loveDataItem);
	}

	public static void OnLoverReincarnate(DataContext context, Character character, int loverCharId)
	{
		if (!IsInstalled())
		{
			return;
		}
		int id = character.GetId();
		if (DomainManager.Extra.TryGetInteractOfLoveData(loverCharId, out var loveDataItem))
		{
			DomainManager.Extra.RemoveInteractOfLoveData(context, loverCharId);
			loveDataItem.ReincarnationLoveList.Add(item: false);
			loveDataItem.DateCount = 0;
			loveDataItem.InteractTime = 0;
			loveDataItem.EventTimeDict.Clear();
			loveDataItem.LoverCharId = id;
			DomainManager.Extra.AddInteractOfLoveData(context, id, loveDataItem);
			if (loveDataItem.IsBindSamsara)
			{
				character.AddFeature(context, 385);
			}
		}
		DomainManager.Extra.RemovePreviousTaiwuLover(context, loverCharId);
		DomainManager.Extra.RemoveConfessLoveFailedCharacter(context, loverCharId);
	}

	public static LoveDataItem BecomeLover(DataContext context, int loverCharId)
	{
		if (DomainManager.Extra.TryGetInteractOfLoveData(loverCharId, out var loveDataItem))
		{
			List<bool> reincarnationLoveList = loveDataItem.ReincarnationLoveList;
			reincarnationLoveList[reincarnationLoveList.Count - 1] = true;
			loveDataItem.BecomeLoverTime = DomainManager.World.GetCurrDate();
			DomainManager.Extra.SetInteractOfLoveData(context, loverCharId, loveDataItem);
			if (loveDataItem.CheckReincarnationLoveContinueThreeTimes())
			{
				Character element_Objects = DomainManager.Character.GetElement_Objects(loverCharId);
				Character taiwu = DomainManager.Taiwu.GetTaiwu();
				element_Objects.AddFeature(context, 386);
				taiwu.AddFeature(context, 386);
			}
		}
		else
		{
			loveDataItem = new LoveDataItem();
			loveDataItem.ReincarnationLoveList.Add(item: true);
			loveDataItem.TaiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			loveDataItem.LoverCharId = loverCharId;
			loveDataItem.BecomeLoverTime = DomainManager.World.GetCurrDate();
			DomainManager.Extra.AddInteractOfLoveData(context, loverCharId, loveDataItem);
		}
		DomainManager.Extra.RemoveConfessLoveFailedCharacter(context, loverCharId);
		return loveDataItem;
	}

	public static void LeaveLover(DataContext context, int loverCharId)
	{
		if (!TryGetLoveDataItem(loverCharId, out var loveDataItem))
		{
			return;
		}
		Character element_Objects = DomainManager.Character.GetElement_Objects(loverCharId);
		element_Objects.RemoveFeature(context, 384);
		element_Objects.RemoveFeature(context, 385);
		element_Objects.RemoveFeature(context, 386);
		bool flag = false;
		foreach (KeyValuePair<int, LoveDataItem> item in DomainManager.Extra.LoveDataDict)
		{
			element_Objects = DomainManager.Character.GetElement_Objects(item.Key);
			List<short> featureIds = element_Objects.GetFeatureIds();
			if (featureIds != null && featureIds.Contains(386))
			{
				flag = true;
			}
		}
		Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (!flag)
		{
			taiwu.RemoveFeature(context, 386);
		}
		DomainManager.Extra.RemoveInteractOfLoveData(context, loverCharId);
		DomainManager.Extra.RemoveLoveTokenData(context, loveDataItem.LoverOwnedToken);
		DomainManager.Extra.RemoveLoveTokenData(context, loveDataItem.TaiwuOwnedToken);
	}

	public static bool CheckItemIsLoveTokenAndReplace(DataContext context, ItemKey oldItemKey, ItemKey newItemKey)
	{
		if (!IsInstalled())
		{
			return false;
		}
		if (DomainManager.Extra.TryGetLoveTokenData(oldItemKey, out var loveTokenDataItem))
		{
			DomainManager.Extra.RemoveLoveTokenData(context, oldItemKey, itemIsDeleted: true);
			if (newItemKey.IsValid())
			{
				newItemKey = DomainManager.Extra.AddLoveTokenData(context, newItemKey, loveTokenDataItem);
			}
			if (TryGetLoveDataItem(loveTokenDataItem.LoverCharId, out var loveDataItem))
			{
				if (oldItemKey.Equals(loveDataItem.TaiwuOwnedToken))
				{
					loveDataItem.TaiwuOwnedToken = newItemKey;
				}
				else
				{
					loveDataItem.LoverOwnedToken = newItemKey;
				}
				DomainManager.Extra.SetInteractOfLoveData(context, loveTokenDataItem.LoverCharId, loveDataItem);
				return true;
			}
		}
		return false;
	}

	public static void SetLoveToken(DataContext context, int charId, ItemKey taiwuOwnedToken, ItemKey loverOwnedToken)
	{
		if (IsInstalled())
		{
			if (!TryGetLoveDataItem(charId, out var loveDataItem))
			{
				BecomeLover(context, charId);
				SetLoveToken(context, charId, taiwuOwnedToken, loverOwnedToken);
				return;
			}
			int currDate = DomainManager.World.GetCurrDate();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			LoveTokenDataItem loveTokenDataItem = new LoveTokenDataItem(currDate, charId, taiwuCharId, taiwuCharId, isTaiwuPresent: false);
			LoveTokenDataItem loveTokenDataItem2 = new LoveTokenDataItem(currDate, charId, taiwuCharId, charId, isTaiwuPresent: true);
			taiwuOwnedToken = DomainManager.Extra.AddLoveTokenData(context, taiwuOwnedToken, loveTokenDataItem);
			loverOwnedToken = DomainManager.Extra.AddLoveTokenData(context, loverOwnedToken, loveTokenDataItem2);
			loveDataItem.TaiwuOwnedToken = taiwuOwnedToken;
			loveDataItem.LoverOwnedToken = loverOwnedToken;
			DomainManager.Extra.SetInteractOfLoveData(context, charId, loveDataItem);
		}
	}

	public static void SetLoveTokenHolder(DataContext context, ItemKey loveToken, int charId)
	{
		if (IsInstalled() && DomainManager.Extra.TryGetLoveTokenData(loveToken, out var loveTokenDataItem))
		{
			loveTokenDataItem.CurHolderCharId = charId;
			DomainManager.Extra.SetLoveTokenData(context, loveToken, loveTokenDataItem);
		}
	}

	public static void TryTriggerInteractOfLoveMonthlyEvents(DataContext context)
	{
		Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int id = taiwu.GetId();
		sbyte birthMonth = taiwu.GetBirthMonth();
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		sbyte currMonthInYear = DomainManager.World.GetCurrMonthInYear();
		List<int> list = new List<int>();
		List<(int, int, int)> list2 = new List<(int, int, int)>();
		List<int> list3 = new List<int>();
		List<int> list4 = new List<int>();
		Span<int> span = stackalloc int[2];
		GetAllAliveLovers(id, list3);
		GetAllInHashSet(list3, collection, list4);
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		foreach (int item in collection)
		{
			if (item == id)
			{
				continue;
			}
			RelatedCharacter relation = DomainManager.Character.GetRelation(id, item);
			RelatedCharacter relation2 = DomainManager.Character.GetRelation(item, id);
			if (!IsLover(relation, relation2))
			{
				continue;
			}
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(relation2.Favorability);
			if (birthMonth == currMonthInYear)
			{
			}
			if (favorabilityType < 5)
			{
				bool flag = true;
			}
			if (list3.Count >= 3 && favorabilityType >= 4 && !DomainManager.Extra.IsAiActionInCooldown(item, 1, 4))
			{
				list.Add(item);
			}
			if (list4.Count < 3 || DomainManager.Extra.IsAiActionInCooldown(item, 1, 5))
			{
				continue;
			}
			CollectionUtils.Shuffle(context.Random, list4);
			int num = 0;
			foreach (int item2 in list4)
			{
				if (item2 != item)
				{
					span[num] = item2;
					num++;
					if (num >= span.Length)
					{
						break;
					}
				}
			}
			list2.Add((item, span[0], span[1]));
		}
		Location location = taiwu.GetLocation();
		if (!location.IsValid())
		{
			return;
		}
		MapBlockData block = DomainManager.Map.GetBlock(location);
		if (block.CharacterSet == null)
		{
			return;
		}
		foreach (int item3 in block.CharacterSet)
		{
			if (DomainManager.Character.TryGetRelation(id, item3, out var relation3) && DomainManager.Character.TryGetRelation(item3, id, out var relation4) && IsLover(relation3, relation4))
			{
				sbyte favorabilityType2 = FavorabilityType.GetFavorabilityType(relation4.Favorability);
				if (list3.Count >= 3 && favorabilityType2 >= 4 && !DomainManager.Extra.IsAiActionInCooldown(item3, 1, 4))
				{
					list.Add(item3);
				}
			}
		}
		int aliveSpouse = DomainManager.Character.GetAliveSpouse(id);
		if (aliveSpouse >= 0 && collection.Contains(aliveSpouse))
		{
			if (DomainManager.Character.TryGetPregnantState(id, out var pregnantState))
			{
			}
			if (!DomainManager.Character.TryGetPregnantState(aliveSpouse, out pregnantState))
			{
			}
		}
		if (list.Count > 0)
		{
			int random = list.GetRandom(context.Random);
			DomainManager.Extra.AddAiActionCooldown(context, random, 1, 5, 32767);
		}
		if (list2.Count > 0)
		{
			var (charId, num2, num3) = list2.GetRandom(context.Random);
			DomainManager.Extra.AddAiActionCooldown(context, charId, 1, 5, 32767);
		}
	}

	private static bool IsLover(RelatedCharacter selfToTarget, RelatedCharacter targetToSelf)
	{
		return RelationType.HasRelation(selfToTarget.RelationType, 1024) || (RelationType.HasRelation(selfToTarget.RelationType, 16384) && RelationType.HasRelation(targetToSelf.RelationType, 16384));
	}

	private static void GetAllAliveLovers(int charId, List<int> loverIds)
	{
		loverIds.Clear();
		RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(charId);
		foreach (int item in relatedCharacters.Adored.GetCollection())
		{
			if (DomainManager.Character.IsCharacterAlive(charId) && !DomainManager.Character.HasRelation(item, charId, 16384))
			{
				loverIds.Add(item);
			}
		}
	}

	private static void GetAllInHashSet(IEnumerable<int> collection, HashSet<int> hashSet, List<int> result)
	{
		foreach (int item in collection)
		{
			if (hashSet.Contains(item))
			{
				result.Add(item);
			}
		}
	}
}
