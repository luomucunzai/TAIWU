using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.DLC;
using GameData.DLC.Shared;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.InteractOfLoveEventHelper
{
	// Token: 0x0200009F RID: 159
	public class InteractOfLoveEventHelper
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06001A37 RID: 6711 RVA: 0x001762EF File Offset: 0x001744EF
		private static TaiwuEventDomain Domain
		{
			get
			{
				return DomainManager.TaiwuEvent;
			}
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x001762F8 File Offset: 0x001744F8
		public static bool CheckDateNpcOptionIsVisible(int charId)
		{
			bool flag = !EventHelper.CheckMainStoryLineProgress(6);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Character character;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
					defaultInterpolatedStringHandler.AppendLiteral("can not get character, Id = ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Character taiwu;
				bool flag3 = !DomainManager.Character.TryGetElement_Objects(DomainManager.Taiwu.GetTaiwuCharId(), out taiwu);
				if (flag3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
					defaultInterpolatedStringHandler.AppendLiteral("can not get character, Id = ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(DomainManager.Taiwu.GetTaiwuCharId());
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag4 = EventHelper.GetRoleAge(taiwu) >= 16 && EventHelper.GetRoleAge(character) >= 16 && EventHelper.CheckHasRelationship(taiwu, character, 8192);
				if (flag4)
				{
					bool flag5 = EventHelper.CheckHasRelationship(taiwu, character, 2048) || EventHelper.CheckHasRelationship(character, taiwu, 2048);
					if (flag5)
					{
						result = false;
					}
					else
					{
						bool flag6 = EventHelper.HasNominalBloodRelation(taiwu.GetId(), character.GetId());
						if (flag6)
						{
							result = false;
						}
						else
						{
							bool flag7 = EventHelper.HasBloodExclusionRelation(taiwu.GetId(), character.GetId()) && !EventHelper.CheckHasRelationship(taiwu, character, 1024);
							if (flag7)
							{
								result = false;
							}
							else
							{
								bool flag8 = EventHelper.CheckHasRelationship(taiwu, character, 16384) && EventHelper.CheckHasRelationship(character, taiwu, 16384);
								if (flag8)
								{
									result = true;
								}
								else
								{
									bool flag9 = EventHelper.CheckHasRelationship(taiwu, character, 1024);
									result = flag9;
								}
							}
						}
					}
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x001764A4 File Offset: 0x001746A4
		public static bool IsSaveInteractTime(int charId)
		{
			LoveDataItem loveDataItem;
			return InteractOfLove.IsInstalled() && InteractOfLove.TryGetLoveDataItem(charId, out loveDataItem);
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x001764C8 File Offset: 0x001746C8
		public static bool IsLover(int charId)
		{
			return InteractOfLove.IsInstalled() && DomainManager.Extra.IsTaiwuLover(charId);
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x001764F0 File Offset: 0x001746F0
		public static bool IsPreviousLover(int charId)
		{
			return InteractOfLove.IsInstalled() && DomainManager.Extra.IsPreviousTaiwuLover(charId);
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x00176518 File Offset: 0x00174718
		public static bool IsLoveToken(ItemKey itemKey)
		{
			bool flag = !InteractOfLove.IsInstalled();
			return !flag && DomainManager.Extra.IsLoveToken(itemKey);
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x00176548 File Offset: 0x00174748
		public static bool IsCurrentLoveToken(ItemKey itemKey, int charId)
		{
			bool flag = !InteractOfLove.IsInstalled();
			LoveTokenDataItem loveTokenDataItem;
			return !flag && DomainManager.Extra.TryGetLoveTokenData(itemKey, out loveTokenDataItem) && loveTokenDataItem.LoverCharId == charId;
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x00176588 File Offset: 0x00174788
		public unsafe static bool IsPreviousLoveToken(ItemKey itemKey, int charId)
		{
			bool flag = !InteractOfLove.IsInstalled();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				LoveTokenDataItem loveTokenDataItem;
				bool flag2 = DomainManager.Extra.TryGetLoveTokenData(itemKey, out loveTokenDataItem);
				if (flag2)
				{
					Character character = DomainManager.Character.GetElement_Objects(charId);
					PreexistenceCharIds preexistenceCharIds = *character.GetPreexistenceCharIds();
					int i = 0;
					int max = preexistenceCharIds.Count;
					while (i < max)
					{
						int preCharId = *(ref preexistenceCharIds.CharIds.FixedElementField + (IntPtr)i * 4);
						bool flag3 = preCharId == loveTokenDataItem.LoverCharId;
						if (flag3)
						{
							return true;
						}
						i++;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x00176624 File Offset: 0x00174824
		public static sbyte GetConfessionLoveResult(int charId)
		{
			LoveDataItem loveDataItem;
			bool flag = DomainManager.Extra.TryGetInteractOfLoveData(charId, out loveDataItem);
			sbyte result;
			if (flag)
			{
				result = 2;
			}
			else
			{
				bool flag2 = DomainManager.Extra.IsConfessLoveFailedCharacter(charId);
				if (flag2)
				{
					result = 1;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x00176664 File Offset: 0x00174864
		public static void SetConfessionLoveResult(int charId, sbyte result)
		{
			DataContext context = InteractOfLoveEventHelper.Domain.MainThreadDataContext;
			bool flag = result == 1;
			if (flag)
			{
				DomainManager.Extra.AddConfessLoveFailedCharacter(context, charId);
				LoveDataItem loveDataItem;
				bool flag2 = InteractOfLove.TryGetLoveDataItem(charId, out loveDataItem);
				if (flag2)
				{
					InteractOfLove.LeaveLover(context, charId);
				}
			}
			bool flag3 = result == 2;
			if (flag3)
			{
				DomainManager.Extra.AddConfessLoveFailedCharacter(context, charId);
				LoveDataItem loveDataItem2;
				bool flag4 = !InteractOfLove.TryGetLoveDataItem(charId, out loveDataItem2);
				if (flag4)
				{
					InteractOfLove.BecomeLover(context, charId);
				}
			}
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x001766E0 File Offset: 0x001748E0
		public static int GetDateNpcCount(int charId)
		{
			LoveDataItem loveDataItem;
			bool flag = !InteractOfLove.TryGetLoveDataItem(charId, out loveDataItem);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = loveDataItem.DateCount;
			}
			return result;
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x00176710 File Offset: 0x00174910
		public static void SetDateNpcCount(int charId, int count)
		{
			LoveDataItem loveDataItem;
			bool flag = InteractOfLove.TryGetLoveDataItem(charId, out loveDataItem);
			if (flag)
			{
				loveDataItem.DateCount = count;
				DomainManager.Extra.SetInteractOfLoveData(InteractOfLoveEventHelper.Domain.MainThreadDataContext, charId, loveDataItem);
			}
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x0017674C File Offset: 0x0017494C
		public static string GetTaiwuNickname(int charId)
		{
			LoveDataItem loveDataItem;
			bool flag = !InteractOfLove.TryGetLoveDataItem(charId, out loveDataItem);
			string result;
			if (flag)
			{
				result = "";
			}
			else
			{
				result = loveDataItem.TaiwuNicknameId.ToString();
			}
			return result;
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x00176784 File Offset: 0x00174984
		public static void SetTaiwuNickname(int charId, int nickName)
		{
			LoveDataItem loveDataItem;
			bool flag = !InteractOfLove.TryGetLoveDataItem(charId, out loveDataItem);
			if (flag)
			{
				loveDataItem.TaiwuNicknameId = nickName;
				DomainManager.Extra.SetInteractOfLoveData(InteractOfLoveEventHelper.Domain.MainThreadDataContext, charId, loveDataItem);
			}
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x001767C4 File Offset: 0x001749C4
		public static void SetTaiwuNickname(int charId, string nickName)
		{
			int id = DomainManager.World.RegisterCustomText(InteractOfLoveEventHelper.Domain.MainThreadDataContext, nickName);
			InteractOfLoveEventHelper.SetTaiwuNickname(charId, id);
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x001767F0 File Offset: 0x001749F0
		public static int GetInteractTime(int charId)
		{
			LoveDataItem loveDataItem;
			bool flag = InteractOfLove.TryGetLoveDataItem(charId, out loveDataItem);
			int result;
			if (flag)
			{
				result = loveDataItem.InteractTime;
			}
			else
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x0017681C File Offset: 0x00174A1C
		public static void SetInteractTime(int charId, int time)
		{
			DataContext context = InteractOfLoveEventHelper.Domain.MainThreadDataContext;
			LoveDataItem loveDataItem;
			bool flag = !InteractOfLove.TryGetLoveDataItem(charId, out loveDataItem);
			if (flag)
			{
				InteractOfLove.BecomeLover(context, charId);
				InteractOfLoveEventHelper.SetInteractTime(charId, time);
			}
			else
			{
				loveDataItem.InteractTime = time;
				DomainManager.Extra.SetInteractOfLoveData(context, charId, loveDataItem);
			}
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x00176870 File Offset: 0x00174A70
		public static void SetEventTimeDict(int charId, Dictionary<sbyte, int> dict)
		{
			DataContext context = InteractOfLoveEventHelper.Domain.MainThreadDataContext;
			LoveDataItem loveDataItem;
			bool flag = !InteractOfLove.TryGetLoveDataItem(charId, out loveDataItem);
			if (flag)
			{
				InteractOfLove.BecomeLover(context, charId);
				InteractOfLoveEventHelper.SetEventTimeDict(charId, dict);
			}
			else
			{
				loveDataItem.EventTimeDict = dict;
				DomainManager.Extra.SetInteractOfLoveData(context, charId, loveDataItem);
			}
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x001768C4 File Offset: 0x00174AC4
		public static Dictionary<sbyte, int> GetEventTimeDict(int charId)
		{
			LoveDataItem loveDataItem;
			bool flag = InteractOfLove.TryGetLoveDataItem(charId, out loveDataItem);
			Dictionary<sbyte, int> result;
			if (flag)
			{
				result = loveDataItem.EventTimeDict;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x001768F0 File Offset: 0x00174AF0
		public static void SaveEventTimeDict(int charId, sbyte eventId, int time)
		{
			Dictionary<sbyte, int> dict = InteractOfLoveEventHelper.GetEventTimeDict(charId);
			bool flag = dict == null;
			if (flag)
			{
				dict = new Dictionary<sbyte, int>();
			}
			bool flag2 = !dict.ContainsKey(eventId);
			if (flag2)
			{
				dict.Add(eventId, time);
			}
			else
			{
				dict[eventId] = time;
			}
			InteractOfLoveEventHelper.SetEventTimeDict(charId, dict);
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x00176944 File Offset: 0x00174B44
		public static void SetLoveToken(int charId, ItemKey taiwuOwnedToken, ItemKey loverOwnedToken)
		{
			DataContext context = InteractOfLoveEventHelper.Domain.MainThreadDataContext;
			LoveDataItem loveDataItem;
			bool flag = !InteractOfLove.TryGetLoveDataItem(charId, out loveDataItem);
			if (flag)
			{
				InteractOfLove.BecomeLover(context, charId);
				InteractOfLoveEventHelper.SetLoveToken(charId, taiwuOwnedToken, loverOwnedToken);
			}
			else
			{
				loveDataItem.TaiwuOwnedToken = taiwuOwnedToken;
				loveDataItem.LoverOwnedToken = loverOwnedToken;
				DomainManager.Extra.SetInteractOfLoveData(context, charId, loveDataItem);
			}
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x001769A0 File Offset: 0x00174BA0
		public static ItemKey ExchangeLoveToken(int charId, ItemKey selectItemKey)
		{
			Character character;
			bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
				defaultInterpolatedStringHandler.AppendLiteral("can not get character,Id = ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Character taiwu;
			bool flag2 = !DomainManager.Character.TryGetElement_Objects(DomainManager.Taiwu.GetTaiwuCharId(), out taiwu);
			if (flag2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
				defaultInterpolatedStringHandler.AppendLiteral("can not get character,Id = ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(DomainManager.Taiwu.GetTaiwuCharId());
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			sbyte consummateLevel = character.GetConsummateLevel();
			sbyte grade = (sbyte)Math.Clamp((int)(consummateLevel / 2), 0, 8);
			ItemKey npcItemKey = DomainManager.Building.GetRandomItemByGrade(InteractOfLoveEventHelper.Domain.MainThreadDataContext.Random, grade, -1);
			npcItemKey = DomainManager.Item.CreateItem(InteractOfLoveEventHelper.Domain.MainThreadDataContext, npcItemKey.ItemType, npcItemKey.TemplateId);
			taiwu.AddInventoryItem(InteractOfLoveEventHelper.Domain.MainThreadDataContext, npcItemKey, 1, false);
			taiwu.RemoveInventoryItem(InteractOfLoveEventHelper.Domain.MainThreadDataContext, selectItemKey, 1, false, false);
			character.AddInventoryItem(InteractOfLoveEventHelper.Domain.MainThreadDataContext, selectItemKey, 1, false);
			InteractOfLove.SetLoveToken(InteractOfLoveEventHelper.Domain.MainThreadDataContext, charId, npcItemKey, selectItemKey);
			return npcItemKey;
		}

		// Token: 0x06001A4D RID: 6733 RVA: 0x00176AF8 File Offset: 0x00174CF8
		public static ItemKey SendBirthdayGift(int charId, int targetId)
		{
			Character character;
			bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
				defaultInterpolatedStringHandler.AppendLiteral("can not get character,Id = ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Character taiwu;
			bool flag2 = !DomainManager.Character.TryGetElement_Objects(DomainManager.Taiwu.GetTaiwuCharId(), out taiwu);
			if (flag2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
				defaultInterpolatedStringHandler.AppendLiteral("can not get character,Id = ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(DomainManager.Taiwu.GetTaiwuCharId());
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			ItemKey item = EventHelper.SelectCharacterItemByGrade(charId, 6, 8, false, -1, -1, null);
			bool flag3 = item.Equals(ItemKey.Invalid);
			if (flag3)
			{
				item = EventHelper.SelectCharacterTopGradeItem(charId, false);
			}
			bool flag4 = item.Equals(ItemKey.Invalid);
			if (flag4)
			{
				throw new Exception("item can not invalid");
			}
			character.RemoveInventoryItem(InteractOfLoveEventHelper.Domain.MainThreadDataContext, item, 1, false, false);
			taiwu.AddInventoryItem(InteractOfLoveEventHelper.Domain.MainThreadDataContext, item, 1, false);
			EventHelper.ShowGetItemPageForItems(new List<ValueTuple<ItemKey, int>>
			{
				new ValueTuple<ItemKey, int>(item, 1)
			}, "", null, false);
			return item;
		}

		// Token: 0x06001A4E RID: 6734 RVA: 0x00176C38 File Offset: 0x00174E38
		public static MapBlockData GetCharacterNearBlockByType(Character character, List<int> mapBlockSubTypeList)
		{
			List<MapBlockData> mapBlockList = ObjectPool<List<MapBlockData>>.Instance.Get();
			Location location = character.GetLocation();
			MapBlockData mapBlockData = EventHelper.GetMapBlockData(location.AreaId, location.BlockId);
			bool flag = mapBlockSubTypeList.Contains((int)mapBlockData.BlockSubType);
			MapBlockData result;
			if (flag)
			{
				result = mapBlockData;
			}
			else
			{
				MapBlockData mapBlock = null;
				for (int step = 3; step < 30; step += 2)
				{
					DomainManager.Map.GetNeighborBlocks(location.AreaId, location.BlockId, mapBlockList, step);
					for (int i = 0; i < mapBlockList.Count; i++)
					{
						bool flag2 = mapBlockSubTypeList.Contains((int)mapBlockList[i].GetConfig().SubType);
						if (flag2)
						{
							mapBlock = mapBlockList[i];
							break;
						}
					}
					bool flag3 = mapBlock != null;
					if (flag3)
					{
						break;
					}
				}
				bool flag4 = mapBlock == null;
				if (flag4)
				{
					ObjectPool<List<MapBlockData>>.Instance.Return(mapBlockList);
					result = null;
				}
				else
				{
					ObjectPool<List<MapBlockData>>.Instance.Return(mapBlockList);
					result = mapBlock;
				}
			}
			return result;
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x00176D40 File Offset: 0x00174F40
		public static MapBlockData CharacterMoveToNearBlockByType(int charId, int mapBlockSubType)
		{
			Character character;
			bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
				defaultInterpolatedStringHandler.AppendLiteral("can not get character,Id = ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bool flag2 = character == null;
			MapBlockData result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				List<int> mapBlockSubTypeList = ObjectPool<List<int>>.Instance.Get();
				mapBlockSubTypeList.Clear();
				bool flag3 = mapBlockSubType == 62;
				if (flag3)
				{
					mapBlockSubTypeList.AddRange(InteractOfLoveEventHelper.TownBlockArray);
				}
				else
				{
					mapBlockSubTypeList.Add(mapBlockSubType);
				}
				MapBlockData mapBlock = InteractOfLoveEventHelper.GetCharacterNearBlockByType(character, mapBlockSubTypeList);
				ObjectPool<List<int>>.Instance.Return(mapBlockSubTypeList);
				bool flag4 = mapBlock == null;
				if (flag4)
				{
					result = null;
				}
				else
				{
					bool flag5 = charId == DomainManager.Taiwu.GetTaiwuCharId();
					if (flag5)
					{
						EventHelper.TeleportMoveTaiwuToBlock(mapBlock.GetLocation().BlockId);
					}
					else
					{
						DomainManager.Character.GroupMove(InteractOfLoveEventHelper.Domain.MainThreadDataContext, character, mapBlock.GetLocation());
					}
					result = mapBlock;
				}
			}
			return result;
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x00176E49 File Offset: 0x00175049
		public static void AddStartRelationSuccessRate(int charId, int targetId, sbyte actionType, sbyte actionSubType, short rateAdjust, int duration)
		{
			DomainManager.Extra.AddAiActionSuccessRateAdjust(InteractOfLoveEventHelper.Domain.MainThreadDataContext, charId, targetId, actionType, actionSubType, rateAdjust, duration);
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x00176E6C File Offset: 0x0017506C
		public static short GetAiActionSuccessRateAdjust(int charId, int targetId, sbyte actionType, sbyte actionSubType)
		{
			return DomainManager.Extra.GetAiActionSuccessRateAdjust(charId, targetId, actionType, actionSubType);
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x00176E8C File Offset: 0x0017508C
		public static void AddStartRelationSuccessRate_BoyOrGirlFriend(int charId, int targetId, short rateAdjust, int duration)
		{
			short rate = InteractOfLoveEventHelper.GetAiActionSuccessRateAdjust(charId, targetId, 10, RelationType.GetTypeId(16384));
			bool flag = rate < 100;
			if (flag)
			{
				rate += rateAdjust;
			}
			InteractOfLoveEventHelper.AddStartRelationSuccessRate(charId, targetId, 10, RelationType.GetTypeId(16384), rate, duration);
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x00176ED4 File Offset: 0x001750D4
		public static void SetBindSamsara(int charId, bool isBindSamsara)
		{
			LoveDataItem loveDataItem;
			bool flag = InteractOfLove.TryGetLoveDataItem(charId, out loveDataItem);
			if (flag)
			{
				loveDataItem.IsBindSamsara = isBindSamsara;
				DomainManager.Extra.SetInteractOfLoveData(InteractOfLoveEventHelper.Domain.MainThreadDataContext, charId, loveDataItem);
			}
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x00176F10 File Offset: 0x00175110
		public static sbyte GetCanHappenDateEvent(int charId)
		{
			Character character;
			bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
				defaultInterpolatedStringHandler.AppendLiteral("can not get character,Id = ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Dictionary<sbyte, int> dateDict = InteractOfLoveEventHelper.GetEventTimeDict(charId);
			List<sbyte> dateEvents = ObjectPool<List<sbyte>>.Instance.Get();
			sbyte favorType = EventHelper.GetFavorabilityType(character, taiwu);
			bool flag2 = !EventHelper.CheckHasRelationship(taiwu, character, 1024);
			if (flag2)
			{
				bool flag3 = favorType >= 4;
				if (flag3)
				{
					dateEvents.AddRange(LoveEventId.UnmarriedFavorite4DateEvents);
				}
				bool flag4 = favorType >= 5;
				if (flag4)
				{
					dateEvents.AddRange(LoveEventId.UnmarriedFavorite5DateEvents);
				}
				bool flag5 = favorType >= 6;
				if (flag5)
				{
					dateEvents.AddRange(LoveEventId.UnmarriedFavorite6DateEvents);
				}
			}
			else
			{
				bool flag6 = favorType >= 4;
				if (flag6)
				{
					dateEvents.AddRange(LoveEventId.MarriedFavorite4DateEvents);
				}
				bool flag7 = favorType >= 5;
				if (flag7)
				{
					dateEvents.AddRange(LoveEventId.MarriedFavorite5DateEvents);
				}
				bool flag8 = favorType >= 6;
				if (flag8)
				{
					dateEvents.AddRange(LoveEventId.MarriedFavorite6DateEvents);
				}
				for (int i = 0; i < LoveEventId.MarriedNeedConditionDateEvents.Length; i++)
				{
					bool flag9 = LoveEventId.MarriedNeedConditionDateEvents[i].Item1 > favorType;
					if (!flag9)
					{
						bool flag10 = dateDict.ContainsKey(LoveEventId.MarriedNeedConditionDateEvents[i].Item2);
						if (flag10)
						{
							dateEvents.Add(LoveEventId.MarriedNeedConditionDateEvents[i].Item3);
						}
					}
				}
			}
			ObjectPool<List<sbyte>>.Instance.Return(dateEvents);
			return dateEvents[InteractOfLoveEventHelper.Domain.MainThreadDataContext.Random.Next(dateEvents.Count)];
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x001770E8 File Offset: 0x001752E8
		public static string GetChatJumpEvent(Character taiwu, Character character)
		{
			int charId = character.GetId();
			bool isTaiWuHurt = EventHelper.CheckRoleInjured(taiwu);
			bool flag = isTaiWuHurt;
			string result;
			if (flag)
			{
				bool flag2 = character.GetBehaviorType() == 0;
				if (flag2)
				{
					result = "33d4f6ec-bb8f-4a38-9bb8-59d11fd41ad6";
				}
				else
				{
					bool flag3 = character.GetBehaviorType() == 1;
					if (flag3)
					{
						result = "42e4338d-d95c-46f6-a6bd-050fdba73607";
					}
					else
					{
						bool flag4 = character.GetBehaviorType() == 2;
						if (flag4)
						{
							result = "8610d2a3-6fbd-4c6a-9bef-25e45f33c0c0";
						}
						else
						{
							bool flag5 = character.GetBehaviorType() == 3;
							if (flag5)
							{
								result = "7bd86493-caa8-46ad-88ab-dba7923a9661";
							}
							else
							{
								result = "65616bcc-568a-4384-b33f-4e8faf7f8490";
							}
						}
					}
				}
			}
			else
			{
				bool flag6 = EventHelper.GetRoleDisorderOfQi(taiwu) > 0;
				if (flag6)
				{
					bool flag7 = character.GetBehaviorType() == 0;
					if (flag7)
					{
						result = "8c0f68b8-761e-4898-9625-ca3208a5eeeb";
					}
					else
					{
						bool flag8 = character.GetBehaviorType() == 1;
						if (flag8)
						{
							result = "cf77ec99-828d-4189-a969-c4fd85b4e0bd";
						}
						else
						{
							bool flag9 = character.GetBehaviorType() == 2;
							if (flag9)
							{
								result = "8ce39363-030d-4703-ad4b-8b61aad772e6";
							}
							else
							{
								bool flag10 = character.GetBehaviorType() == 3;
								if (flag10)
								{
									result = "24c4e0ce-4df5-47f9-aae1-ca42b8bdae4a";
								}
								else
								{
									result = "33ca2048-31ba-4171-b0c3-672214f44187";
								}
							}
						}
					}
				}
				else
				{
					bool flag11 = EventHelper.CheckCharacterIsPoisoning(taiwu);
					if (flag11)
					{
						bool flag12 = character.GetBehaviorType() == 0;
						if (flag12)
						{
							result = "46a1b716-f7a5-4d58-a164-c64e32b53a13";
						}
						else
						{
							bool flag13 = character.GetBehaviorType() == 1;
							if (flag13)
							{
								result = "e42b212f-9d2f-42c3-9493-ec0f000d2215";
							}
							else
							{
								bool flag14 = character.GetBehaviorType() == 2;
								if (flag14)
								{
									result = "b6831032-fda5-4ac8-b119-d4efc13914f1";
								}
								else
								{
									bool flag15 = character.GetBehaviorType() == 3;
									if (flag15)
									{
										result = "f177703e-634f-4675-b53f-6a8a862200e2";
									}
									else
									{
										bool flag16 = character.GetBehaviorType() == 4;
										if (flag16)
										{
											result = "5209ed50-0677-4974-85ce-1dc0ade1ed5b";
										}
										else
										{
											result = string.Empty;
										}
									}
								}
							}
						}
					}
					else
					{
						int currDate = DomainManager.World.GetCurrDate();
						Dictionary<sbyte, int> dict = InteractOfLoveEventHelper.GetEventTimeDict(charId);
						bool flag17 = dict == null || !dict.ContainsKey(0) || dict[0] == currDate;
						if (flag17)
						{
							result = "cf53fcbf-f322-43fe-9003-8b4cced7ee5c";
						}
						else
						{
							InteractOfLoveEventHelper.SaveEventTimeDict(charId, 0, currDate);
							bool flag18 = !EventHelper.CheckMainStoryLineProgress(8);
							if (flag18)
							{
								result = "cf53fcbf-f322-43fe-9003-8b4cced7ee5c";
							}
							else
							{
								Character character2;
								bool flag19 = !EventHelper.TryGetFixedCharacterByTemplateId(446, out character2);
								if (flag19)
								{
									bool flag20 = character.GetBehaviorType() == 0;
									if (flag20)
									{
										result = "3d05e502-0b26-4ba4-8efb-9d0384207638";
									}
									else
									{
										bool flag21 = character.GetBehaviorType() == 1;
										if (flag21)
										{
											result = "231b4406-a564-4d74-ba09-52d9fe2401de";
										}
										else
										{
											bool flag22 = character.GetBehaviorType() == 2;
											if (flag22)
											{
												result = "3edd5756-9c21-4cfe-afd4-62b4a5fc412f";
											}
											else
											{
												bool flag23 = character.GetBehaviorType() == 3;
												if (flag23)
												{
													result = "b0956f71-1303-42a9-90db-3955a25e5cf3";
												}
												else
												{
													result = "f234b08f-7ad4-4a62-92e7-9e8b1795eae6";
												}
											}
										}
									}
								}
								else
								{
									bool flag24 = !EventHelper.CheckMainStoryLineProgress(28);
									if (flag24)
									{
										bool flag25 = character.GetBehaviorType() == 0;
										if (flag25)
										{
											result = "b427c3b4-d1c4-48c1-8649-36490f4faee8";
										}
										else
										{
											bool flag26 = character.GetBehaviorType() == 1;
											if (flag26)
											{
												result = "b50a0bba-c478-4d54-b308-26e7dbdb3d32";
											}
											else
											{
												bool flag27 = character.GetBehaviorType() == 2;
												if (flag27)
												{
													result = "89ab1413-1e8f-469e-8e83-dcdcdebf931c";
												}
												else
												{
													bool flag28 = character.GetBehaviorType() == 3;
													if (flag28)
													{
														result = "e2987e91-bd5b-4fc0-9565-2ded89b5805b";
													}
													else
													{
														result = "6902a6f2-84c7-451c-98bb-35595a2d253e";
													}
												}
											}
										}
									}
									else
									{
										bool flag29 = character.GetBehaviorType() == 0;
										if (flag29)
										{
											result = "e0dc16b3-f81c-4d53-bf27-c8ad02f42aa4";
										}
										else
										{
											bool flag30 = character.GetBehaviorType() == 1;
											if (flag30)
											{
												result = "3e40c1b3-6acd-4f68-946f-d7051eacfb39";
											}
											else
											{
												bool flag31 = character.GetBehaviorType() == 2;
												if (flag31)
												{
													result = "132cd56f-9635-47d4-8aa6-7338794b09c5";
												}
												else
												{
													bool flag32 = character.GetBehaviorType() == 3;
													if (flag32)
													{
														result = "27cf3537-5104-4073-9fe4-56c50ab680ad";
													}
													else
													{
														result = "a2a2650c-04bb-49fc-abda-f7d1df5195d1";
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0400060F RID: 1551
		public static readonly int[] TownBlockArray = new int[]
		{
			62,
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			10,
			11,
			12,
			13,
			14,
			15,
			16
		};
	}
}
