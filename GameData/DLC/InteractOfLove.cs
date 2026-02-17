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

namespace GameData.DLC
{
	// Token: 0x020008DA RID: 2266
	public static class InteractOfLove
	{
		// Token: 0x0600814E RID: 33102 RVA: 0x004D0C18 File Offset: 0x004CEE18
		public static bool IsInstalled()
		{
			return DlcManager.IsDlcInstalled(2305890UL);
		}

		// Token: 0x0600814F RID: 33103 RVA: 0x004D0C35 File Offset: 0x004CEE35
		public static bool TryGetLoveDataItem(int charId, out LoveDataItem loveDataItem)
		{
			return DomainManager.Extra.TryGetInteractOfLoveData(charId, out loveDataItem);
		}

		// Token: 0x06008150 RID: 33104 RVA: 0x004D0C44 File Offset: 0x004CEE44
		public static void OnLoverReincarnate(DataContext context, Character character, int loverCharId)
		{
			bool flag = !InteractOfLove.IsInstalled();
			if (!flag)
			{
				int newCharId = character.GetId();
				LoveDataItem loveDataItem;
				bool flag2 = DomainManager.Extra.TryGetInteractOfLoveData(loverCharId, out loveDataItem);
				if (flag2)
				{
					DomainManager.Extra.RemoveInteractOfLoveData(context, loverCharId);
					loveDataItem.ReincarnationLoveList.Add(false);
					loveDataItem.DateCount = 0;
					loveDataItem.InteractTime = 0;
					loveDataItem.EventTimeDict.Clear();
					loveDataItem.LoverCharId = newCharId;
					DomainManager.Extra.AddInteractOfLoveData(context, newCharId, loveDataItem);
					bool isBindSamsara = loveDataItem.IsBindSamsara;
					if (isBindSamsara)
					{
						character.AddFeature(context, 385, false);
					}
				}
				DomainManager.Extra.RemovePreviousTaiwuLover(context, loverCharId);
				DomainManager.Extra.RemoveConfessLoveFailedCharacter(context, loverCharId);
			}
		}

		// Token: 0x06008151 RID: 33105 RVA: 0x004D0CFC File Offset: 0x004CEEFC
		public static LoveDataItem BecomeLover(DataContext context, int loverCharId)
		{
			LoveDataItem loveDataItem;
			bool flag = DomainManager.Extra.TryGetInteractOfLoveData(loverCharId, out loveDataItem);
			if (flag)
			{
				List<bool> reincarnationLoveList = loveDataItem.ReincarnationLoveList;
				reincarnationLoveList[reincarnationLoveList.Count - 1] = true;
				loveDataItem.BecomeLoverTime = DomainManager.World.GetCurrDate();
				DomainManager.Extra.SetInteractOfLoveData(context, loverCharId, loveDataItem);
				bool flag2 = loveDataItem.CheckReincarnationLoveContinueThreeTimes();
				if (flag2)
				{
					Character character = DomainManager.Character.GetElement_Objects(loverCharId);
					Character taiwu = DomainManager.Taiwu.GetTaiwu();
					character.AddFeature(context, 386, false);
					taiwu.AddFeature(context, 386, false);
				}
			}
			else
			{
				loveDataItem = new LoveDataItem();
				loveDataItem.ReincarnationLoveList.Add(true);
				loveDataItem.TaiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				loveDataItem.LoverCharId = loverCharId;
				loveDataItem.BecomeLoverTime = DomainManager.World.GetCurrDate();
				DomainManager.Extra.AddInteractOfLoveData(context, loverCharId, loveDataItem);
			}
			DomainManager.Extra.RemoveConfessLoveFailedCharacter(context, loverCharId);
			return loveDataItem;
		}

		// Token: 0x06008152 RID: 33106 RVA: 0x004D0DF4 File Offset: 0x004CEFF4
		public static void LeaveLover(DataContext context, int loverCharId)
		{
			LoveDataItem loveDataItem;
			bool flag = !InteractOfLove.TryGetLoveDataItem(loverCharId, out loveDataItem);
			if (!flag)
			{
				Character loverChar = DomainManager.Character.GetElement_Objects(loverCharId);
				loverChar.RemoveFeature(context, 384);
				loverChar.RemoveFeature(context, 385);
				loverChar.RemoveFeature(context, 386);
				bool keepThreeLifeLover = false;
				foreach (KeyValuePair<int, LoveDataItem> pair in DomainManager.Extra.LoveDataDict)
				{
					loverChar = DomainManager.Character.GetElement_Objects(pair.Key);
					List<short> featureIds = loverChar.GetFeatureIds();
					bool flag2 = featureIds != null && featureIds.Contains(386);
					if (flag2)
					{
						keepThreeLifeLover = true;
					}
				}
				Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
				bool flag3 = !keepThreeLifeLover;
				if (flag3)
				{
					taiwuChar.RemoveFeature(context, 386);
				}
				DomainManager.Extra.RemoveInteractOfLoveData(context, loverCharId);
				DomainManager.Extra.RemoveLoveTokenData(context, loveDataItem.LoverOwnedToken, false);
				DomainManager.Extra.RemoveLoveTokenData(context, loveDataItem.TaiwuOwnedToken, false);
			}
		}

		// Token: 0x06008153 RID: 33107 RVA: 0x004D0F20 File Offset: 0x004CF120
		public static bool CheckItemIsLoveTokenAndReplace(DataContext context, ItemKey oldItemKey, ItemKey newItemKey)
		{
			bool flag = !InteractOfLove.IsInstalled();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				LoveTokenDataItem loveTokenData;
				bool flag2 = DomainManager.Extra.TryGetLoveTokenData(oldItemKey, out loveTokenData);
				if (flag2)
				{
					DomainManager.Extra.RemoveLoveTokenData(context, oldItemKey, true);
					bool flag3 = newItemKey.IsValid();
					if (flag3)
					{
						newItemKey = DomainManager.Extra.AddLoveTokenData(context, newItemKey, loveTokenData);
					}
					LoveDataItem loveDataItem;
					bool flag4 = InteractOfLove.TryGetLoveDataItem(loveTokenData.LoverCharId, out loveDataItem);
					if (flag4)
					{
						bool flag5 = oldItemKey.Equals(loveDataItem.TaiwuOwnedToken);
						if (flag5)
						{
							loveDataItem.TaiwuOwnedToken = newItemKey;
						}
						else
						{
							loveDataItem.LoverOwnedToken = newItemKey;
						}
						DomainManager.Extra.SetInteractOfLoveData(context, loveTokenData.LoverCharId, loveDataItem);
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06008154 RID: 33108 RVA: 0x004D0FD8 File Offset: 0x004CF1D8
		public static void SetLoveToken(DataContext context, int charId, ItemKey taiwuOwnedToken, ItemKey loverOwnedToken)
		{
			bool flag = !InteractOfLove.IsInstalled();
			if (!flag)
			{
				LoveDataItem loveDataItem;
				bool flag2 = !InteractOfLove.TryGetLoveDataItem(charId, out loveDataItem);
				if (flag2)
				{
					InteractOfLove.BecomeLover(context, charId);
					InteractOfLove.SetLoveToken(context, charId, taiwuOwnedToken, loverOwnedToken);
				}
				else
				{
					int date = DomainManager.World.GetCurrDate();
					int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
					LoveTokenDataItem taiwuOwnedTokenData = new LoveTokenDataItem(date, charId, taiwuCharId, taiwuCharId, false);
					LoveTokenDataItem loverOwnedTokenData = new LoveTokenDataItem(date, charId, taiwuCharId, charId, true);
					taiwuOwnedToken = DomainManager.Extra.AddLoveTokenData(context, taiwuOwnedToken, taiwuOwnedTokenData);
					loverOwnedToken = DomainManager.Extra.AddLoveTokenData(context, loverOwnedToken, loverOwnedTokenData);
					loveDataItem.TaiwuOwnedToken = taiwuOwnedToken;
					loveDataItem.LoverOwnedToken = loverOwnedToken;
					DomainManager.Extra.SetInteractOfLoveData(context, charId, loveDataItem);
				}
			}
		}

		// Token: 0x06008155 RID: 33109 RVA: 0x004D108C File Offset: 0x004CF28C
		public static void SetLoveTokenHolder(DataContext context, ItemKey loveToken, int charId)
		{
			bool flag = !InteractOfLove.IsInstalled();
			if (!flag)
			{
				LoveTokenDataItem loveTokenDataItem;
				bool flag2 = DomainManager.Extra.TryGetLoveTokenData(loveToken, out loveTokenDataItem);
				if (flag2)
				{
					loveTokenDataItem.CurHolderCharId = charId;
					DomainManager.Extra.SetLoveTokenData(context, loveToken, loveTokenDataItem);
				}
			}
		}

		// Token: 0x06008156 RID: 33110 RVA: 0x004D10D0 File Offset: 0x004CF2D0
		public unsafe static void TryTriggerInteractOfLoveMonthlyEvents(DataContext context)
		{
			Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			int taiwuCharId = taiwuChar.GetId();
			sbyte taiwuBirthMonth = taiwuChar.GetBirthMonth();
			HashSet<int> taiwuGroup = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			sbyte currMonthInYear = DomainManager.World.GetCurrMonthInYear();
			List<int> canTriggerJealousyEventCharIds = new List<int>();
			List<ValueTuple<int, int, int>> canTriggerViolentJealousyEventCharIds = new List<ValueTuple<int, int, int>>();
			List<int> taiwuLoverCharIds = new List<int>();
			List<int> sameGroupLoverIds = new List<int>();
			Span<int> span = new Span<int>(stackalloc byte[(UIntPtr)8], 2);
			Span<int> otherLovers = span;
			InteractOfLove.GetAllAliveLovers(taiwuCharId, taiwuLoverCharIds);
			InteractOfLove.GetAllInHashSet(taiwuLoverCharIds, taiwuGroup, sameGroupLoverIds);
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			foreach (int charId in taiwuGroup)
			{
				bool flag = charId == taiwuCharId;
				if (!flag)
				{
					RelatedCharacter selfToTarget = DomainManager.Character.GetRelation(taiwuCharId, charId);
					RelatedCharacter targetToSelf = DomainManager.Character.GetRelation(charId, taiwuCharId);
					bool flag2 = !InteractOfLove.IsLover(selfToTarget, targetToSelf);
					if (!flag2)
					{
						sbyte favorabilityType = FavorabilityType.GetFavorabilityType(targetToSelf.Favorability);
						bool flag3 = taiwuBirthMonth == currMonthInYear;
						if (flag3)
						{
						}
						bool flag4 = favorabilityType >= 5;
						if (flag4)
						{
						}
						bool flag5 = taiwuLoverCharIds.Count >= 3 && favorabilityType >= 4 && !DomainManager.Extra.IsAiActionInCooldown(charId, 1, 4);
						if (flag5)
						{
							canTriggerJealousyEventCharIds.Add(charId);
						}
						bool flag6 = sameGroupLoverIds.Count >= 3 && !DomainManager.Extra.IsAiActionInCooldown(charId, 1, 5);
						if (flag6)
						{
							CollectionUtils.Shuffle<int>(context.Random, sameGroupLoverIds);
							int index = 0;
							foreach (int sameGroupLoverId in sameGroupLoverIds)
							{
								bool flag7 = sameGroupLoverId == charId;
								if (!flag7)
								{
									*otherLovers[index] = sameGroupLoverId;
									index++;
									bool flag8 = index >= otherLovers.Length;
									if (flag8)
									{
										break;
									}
								}
							}
							canTriggerViolentJealousyEventCharIds.Add(new ValueTuple<int, int, int>(charId, *otherLovers[0], *otherLovers[1]));
						}
					}
				}
			}
			Location taiwuLocation = taiwuChar.GetLocation();
			bool flag9 = !taiwuLocation.IsValid();
			if (!flag9)
			{
				MapBlockData block = DomainManager.Map.GetBlock(taiwuLocation);
				bool flag10 = block.CharacterSet == null;
				if (!flag10)
				{
					foreach (int charId2 in block.CharacterSet)
					{
						RelatedCharacter selfToTarget2;
						RelatedCharacter targetToSelf2;
						bool flag11 = !DomainManager.Character.TryGetRelation(taiwuCharId, charId2, out selfToTarget2) || !DomainManager.Character.TryGetRelation(charId2, taiwuCharId, out targetToSelf2);
						if (!flag11)
						{
							bool flag12 = !InteractOfLove.IsLover(selfToTarget2, targetToSelf2);
							if (!flag12)
							{
								sbyte favorabilityType2 = FavorabilityType.GetFavorabilityType(targetToSelf2.Favorability);
								bool flag13 = taiwuLoverCharIds.Count >= 3 && favorabilityType2 >= 4 && !DomainManager.Extra.IsAiActionInCooldown(charId2, 1, 4);
								if (flag13)
								{
									canTriggerJealousyEventCharIds.Add(charId2);
								}
							}
						}
					}
					int spouseCharId = DomainManager.Character.GetAliveSpouse(taiwuCharId);
					bool flag14 = spouseCharId >= 0 && taiwuGroup.Contains(spouseCharId);
					if (flag14)
					{
						PregnantState pregnantState;
						bool flag15 = DomainManager.Character.TryGetPregnantState(taiwuCharId, out pregnantState);
						if (flag15)
						{
						}
						bool flag16 = DomainManager.Character.TryGetPregnantState(spouseCharId, out pregnantState);
						if (flag16)
						{
						}
					}
					bool flag17 = canTriggerJealousyEventCharIds.Count > 0;
					if (flag17)
					{
						int jealousCharId = canTriggerJealousyEventCharIds.GetRandom(context.Random);
						DomainManager.Extra.AddAiActionCooldown(context, jealousCharId, 1, 5, 32767);
					}
					bool flag18 = canTriggerViolentJealousyEventCharIds.Count > 0;
					if (flag18)
					{
						ValueTuple<int, int, int> random = canTriggerViolentJealousyEventCharIds.GetRandom(context.Random);
						int charIdA = random.Item1;
						int charIdB = random.Item2;
						int charIdC = random.Item3;
						DomainManager.Extra.AddAiActionCooldown(context, charIdA, 1, 5, 32767);
					}
				}
			}
		}

		// Token: 0x06008157 RID: 33111 RVA: 0x004D1528 File Offset: 0x004CF728
		private static bool IsLover(RelatedCharacter selfToTarget, RelatedCharacter targetToSelf)
		{
			return RelationType.HasRelation(selfToTarget.RelationType, 1024) || (RelationType.HasRelation(selfToTarget.RelationType, 16384) && RelationType.HasRelation(targetToSelf.RelationType, 16384));
		}

		// Token: 0x06008158 RID: 33112 RVA: 0x004D1574 File Offset: 0x004CF774
		private static void GetAllAliveLovers(int charId, List<int> loverIds)
		{
			loverIds.Clear();
			RelatedCharacters relatedCharIds = DomainManager.Character.GetRelatedCharacters(charId);
			foreach (int relatedCharId in relatedCharIds.Adored.GetCollection())
			{
				bool flag = !DomainManager.Character.IsCharacterAlive(charId);
				if (!flag)
				{
					bool flag2 = DomainManager.Character.HasRelation(relatedCharId, charId, 16384);
					if (!flag2)
					{
						loverIds.Add(relatedCharId);
					}
				}
			}
		}

		// Token: 0x06008159 RID: 33113 RVA: 0x004D1614 File Offset: 0x004CF814
		private static void GetAllInHashSet(IEnumerable<int> collection, HashSet<int> hashSet, List<int> result)
		{
			foreach (int element in collection)
			{
				bool flag = hashSet.Contains(element);
				if (flag)
				{
					result.Add(element);
				}
			}
		}

		// Token: 0x04002389 RID: 9097
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
	}
}
