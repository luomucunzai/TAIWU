using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

namespace GameData.DLC.FiveLoong
{
	// Token: 0x020008DD RID: 2269
	[SerializableGameData(IsExtensible = true, NoCopyConstructors = true, NotForDisplayModule = true)]
	public class FiveLoongDlcEntry : IDlcEntry, ISerializableGameData
	{
		// Token: 0x0600815C RID: 33116 RVA: 0x004D1744 File Offset: 0x004CF944
		private void PostAdvanceMonth_Main(DataContext context)
		{
			DomainManager.Extra.ClearTempData();
			this.UpdateMaxTaiwuVillageLevel();
			bool flag = FiveLoongDlcEntry.<PostAdvanceMonth_Main>g__IsMainStoryLineProgressMeetLoongDlc|10_0() && FiveLoongDlcEntry.<PostAdvanceMonth_Main>g__IsTaiwuVillageLevelMeetFiveLoongDlc|10_1();
			if (flag)
			{
				EventArgBox loongDlcArgBox = DomainManager.Extra.GetOrCreateDlcArgBox(2764950UL, context);
				bool fiveLoongDlcBeginMonthlyEventTriggered = false;
				bool flag2 = !loongDlcArgBox.Get("ConchShip_PresetKey_FiveLoongDlcBeginMonthlyEventTriggered", ref fiveLoongDlcBeginMonthlyEventTriggered) || !fiveLoongDlcBeginMonthlyEventTriggered;
				if (flag2)
				{
					MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
					monthlyEventCollection.AddFiveLoongLetterFromTaiwuVillage();
				}
			}
		}

		// Token: 0x0600815D RID: 33117 RVA: 0x004D17C0 File Offset: 0x004CF9C0
		public void PostAdvanceMonth_JiaoPool(DataContext context)
		{
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			List<JiaoPool> jiaoPools = DomainManager.Extra.GetJiaoPoolList();
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Location location = taiwu.GetLocation();
			bool flag = !location.IsValid();
			if (flag)
			{
				location = taiwu.GetValidLocation();
			}
			int i = 0;
			while (i < jiaoPools.Count)
			{
				JiaoPool jiaoPool = jiaoPools[i];
				bool flag2 = jiaoPool.Jiaos.Count == 1;
				if (flag2)
				{
					Jiao jiao;
					bool flag3 = !DomainManager.Extra.TryGetJiao(jiaoPool.Jiaos[0], out jiao);
					if (!flag3)
					{
						bool flag4 = jiao.GrowthStage == 0;
						if (flag4)
						{
							JiaoPool jiaoPool2 = jiaoPool;
							int num = jiaoPool2.NextPeriod - 1;
							jiaoPool2.NextPeriod = num;
							bool flag5 = num == 0;
							if (flag5)
							{
								DomainManager.Extra.JiaoHatch(context, i);
								monthlyNotificationCollection.AddJiaoBrokeThroughTheShell(location, jiao.Id);
							}
						}
						else
						{
							bool flag6 = jiao.GrowthStage == 1;
							if (flag6)
							{
								bool flag7 = DomainManager.Extra.JiaoFlee(context, i);
								if (flag7)
								{
									monthlyNotificationCollection.AddJiaoGoHome(location, jiao.Id);
								}
								else
								{
									bool isDisabled = jiaoPool.IsDisabled;
									if (isDisabled)
									{
										jiao.TamePoint = Math.Max(0, jiao.TamePoint - 5);
									}
									else
									{
										bool flag8 = !DomainManager.Extra.IsResourceEnoughForJiaoFoster(jiao.NurturanceTemplateId);
										if (flag8)
										{
											monthlyNotificationCollection.AddJiaoPoolAccident(location, jiao.Id);
										}
										else
										{
											bool flag9 = jiao.EvolveRemainingMonth != 0;
											if (flag9)
											{
												bool flag10 = jiao.NextPeriod >= 0;
												if (flag10)
												{
													DomainManager.Extra.ConsumeResourceForJiaoFoster(context, jiao.NurturanceTemplateId);
												}
												Jiao jiao2 = jiao;
												int num = jiao2.NextPeriod - 1;
												jiao2.NextPeriod = num;
												bool flag11 = num == 0 && jiao.NurturanceTemplateId != 0;
												if (flag11)
												{
													DomainManager.Extra.JiaoGrow(context, i);
												}
												else
												{
													bool flag12 = jiao.EvolveRemainingMonth > 1;
													if (flag12)
													{
														DomainManager.Extra.JiaoEvent(context, i);
													}
												}
												bool flag13 = jiao.TamePoint <= 50;
												if (flag13)
												{
													monthlyNotificationCollection.AddJiaoTamingPointsLow(location, jiao.Id);
												}
												DomainManager.Extra.UpdateJiaoEvolveRemainingMonth(context, jiao);
											}
										}
									}
									bool flag14 = jiao.EvolveRemainingMonth == 0;
									if (flag14)
									{
										DomainManager.Extra.JiaoEvolveToCarrier(context, i);
										monthlyNotificationCollection.AddJiaoHasReachedAnAdultAge(location, jiao.Id);
									}
								}
							}
						}
					}
				}
				else
				{
					bool flag15 = jiaoPool.Jiaos.Count == 2;
					if (flag15)
					{
						JiaoPool jiaoPool3 = jiaoPool;
						int num = jiaoPool3.NextPeriod - 1;
						jiaoPool3.NextPeriod = num;
						bool flag16 = num == 0;
						if (flag16)
						{
							bool flag17 = DomainManager.Extra.JiaoBreed(context, i);
							if (flag17)
							{
								monthlyNotificationCollection.AddJiaoLayEggs(location, jiaoPool.Jiaos[0], jiaoPool.Jiaos[1]);
							}
						}
					}
				}
				IL_2E9:
				i++;
				continue;
				goto IL_2E9;
			}
			DomainManager.Extra.SetJiaoPools(jiaoPools, context);
		}

		// Token: 0x0600815E RID: 33118 RVA: 0x004D1ADC File Offset: 0x004CFCDC
		public void PostAdvanceMonth_JiaoPoolLog(DataContext context)
		{
			List<JiaoPoolRecord> tempList = new List<JiaoPoolRecord>();
			int currDate = DomainManager.World.GetCurrDate();
			List<JiaoPoolRecordList> records = DomainManager.Extra.GetJiaoPoolRecords();
			foreach (JiaoPoolRecordList list in records)
			{
				tempList.Clear();
				foreach (JiaoPoolRecord record in list.Collection)
				{
					bool flag = currDate - record.Date <= 24;
					if (flag)
					{
						tempList.Add(record);
					}
				}
				list.Collection.Clear();
				list.Collection.AddRange(tempList);
			}
			DomainManager.Extra.SetJiaoPoolRecords(records, context);
		}

		// Token: 0x0600815F RID: 33119 RVA: 0x004D1BDC File Offset: 0x004CFDDC
		private void PostAdvanceMonth_ChildOfLoong(DataContext context)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			ItemKey equippedCarrier = taiwu.GetEquipment()[11];
			bool flag = !equippedCarrier.IsValid() || DomainManager.Item.GetBaseItem(equippedCarrier).IsDurabilityRunningOut();
			if (!flag)
			{
				short jiaoTemplateId;
				bool flag2 = !FiveLoongDlcEntry.CarrierToJiaoTemplate.TryGetValue(equippedCarrier.TemplateId, out jiaoTemplateId);
				if (!flag2)
				{
					JiaoItem jiaoCfg = Jiao.Instance[jiaoTemplateId];
					bool flag3 = jiaoCfg.MonthlyEventCost <= 0;
					if (!flag3)
					{
						bool flag4 = !context.Random.CheckPercentProb(this.ChildrenOfLoongMonthlyEventChance);
						if (flag4)
						{
							this.ChildrenOfLoongMonthlyEventChance += 10;
						}
						else
						{
							ChildrenOfLoong childOfLoong = DomainManager.Extra.GetChildrenOfLoongByItemKey(equippedCarrier);
							short templateId = equippedCarrier.TemplateId;
							if (!true)
							{
							}
							bool flag5;
							switch (templateId)
							{
							case 77:
								flag5 = this.PostAdvanceMonth_ChildOfLoong_Qiuniu(context, childOfLoong);
								break;
							case 78:
								flag5 = this.PostAdvanceMonth_ChildOfLoong_Yazi(context, childOfLoong);
								break;
							case 79:
								flag5 = this.PostAdvanceMonth_ChildOfLoong_Chaofeng(context, childOfLoong);
								break;
							case 80:
								flag5 = this.PostAdvanceMonth_ChildOfLoong_Pulao(context, childOfLoong);
								break;
							case 81:
								flag5 = this.PostAdvanceMonth_ChildOfLoong_Suanni(context, childOfLoong);
								break;
							case 82:
								flag5 = this.PostAdvanceMonth_ChildOfLoong_Baxia(context, childOfLoong);
								break;
							case 83:
								flag5 = this.PostAdvanceMonth_ChildOfLoong_Bian(context, childOfLoong);
								break;
							case 84:
								flag5 = this.PostAdvanceMonth_ChildOfLoong_Fuxi(context, childOfLoong);
								break;
							case 85:
								flag5 = this.PostAdvanceMonth_ChildOfLoong_Chiwen(context, childOfLoong);
								break;
							default:
								flag5 = false;
								break;
							}
							if (!true)
							{
							}
							bool succeed = flag5;
							bool flag6 = succeed;
							if (flag6)
							{
								this.ChildrenOfLoongMonthlyEventChance -= jiaoCfg.MonthlyEventCost;
							}
							else
							{
								this.ChildrenOfLoongMonthlyEventChance += 10;
							}
						}
					}
				}
			}
		}

		// Token: 0x06008160 RID: 33120 RVA: 0x004D1D94 File Offset: 0x004CFF94
		private bool PostAdvanceMonth_ChildOfLoong_Qiuniu(DataContext context, ChildrenOfLoong childOfLoong)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Location taiwuLocation = taiwuChar.GetLocation();
			bool flag = !taiwuLocation.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				List<MapBlockData> neighborBlocks = context.AdvanceMonthRelatedData.Blocks.Occupy();
				DomainManager.Map.GetNeighborBlocks(taiwuLocation.AreaId, taiwuLocation.BlockId, neighborBlocks, 3);
				List<int> potentialCharIds = context.AdvanceMonthRelatedData.CharIdList.Occupy();
				foreach (MapBlockData neighborBlock in neighborBlocks)
				{
					bool flag2 = neighborBlock.CharacterSet == null;
					if (!flag2)
					{
						foreach (int charId in neighborBlock.CharacterSet)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							bool flag3 = character.IsActiveExternalRelationState(60);
							if (!flag3)
							{
								potentialCharIds.Add(charId);
							}
						}
					}
				}
				context.AdvanceMonthRelatedData.Blocks.Release(ref neighborBlocks);
				bool flag4 = potentialCharIds.Count == 0;
				if (flag4)
				{
					context.AdvanceMonthRelatedData.CharIdList.Release(ref potentialCharIds);
					result = false;
				}
				else
				{
					int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
					int currDate = DomainManager.World.GetCurrDate();
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
					MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
					int affectedTargetCount = context.Random.Next(3) + 1;
					for (int i = 0; i < affectedTargetCount; i++)
					{
						bool flag5 = potentialCharIds.Count == 0;
						if (flag5)
						{
							break;
						}
						int index = context.Random.Next(potentialCharIds.Count);
						int affectedCharId = potentialCharIds[index];
						CollectionUtils.SwapAndRemove<int>(potentialCharIds, index);
						GameData.Domains.Character.Character affectedChar = DomainManager.Character.GetElement_Objects(affectedCharId);
						int favorChange = context.Random.Next(2000, 4001);
						DomainManager.Character.ChangeFavorabilityOptional(context, affectedChar, taiwuChar, favorChange, 5);
						Location location = affectedChar.GetLocation();
						lifeRecordCollection.AddDLCLoongRidingEffectQiuniuAudience(affectedCharId, currDate, taiwuCharId, location, childOfLoong.Id);
					}
					context.AdvanceMonthRelatedData.CharIdList.Release(ref potentialCharIds);
					lifeRecordCollection.AddDLCLoongRidingEffectQiuniu(taiwuCharId, currDate, taiwuLocation, childOfLoong.Id);
					monthlyEventCollection.AddDLCLoongRidingEffectQiuniu(taiwuCharId, childOfLoong.Id);
					monthlyNotifications.AddDLCLoongRidingEffectQiuniu(taiwuCharId, taiwuLocation, childOfLoong.Id);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06008161 RID: 33121 RVA: 0x004D2050 File Offset: 0x004D0250
		private bool PostAdvanceMonth_ChildOfLoong_Yazi(DataContext context, ChildrenOfLoong childOfLoong)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Location taiwuLocation = taiwuChar.GetLocation();
			bool flag = !taiwuLocation.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				List<MapBlockData> neighborBlocks = context.AdvanceMonthRelatedData.Blocks.Occupy();
				DomainManager.Map.GetNeighborBlocks(taiwuLocation.AreaId, taiwuLocation.BlockId, neighborBlocks, 3);
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				List<int> potentialCharIds = context.AdvanceMonthRelatedData.CharIdList.Occupy();
				foreach (MapBlockData neighborBlock in neighborBlocks)
				{
					bool flag2 = neighborBlock.CharacterSet == null;
					if (!flag2)
					{
						foreach (int charId in neighborBlock.CharacterSet)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							bool flag3 = character.GetAgeGroup() == 0;
							if (!flag3)
							{
								bool flag4 = character.IsActiveExternalRelationState(60);
								if (!flag4)
								{
									bool flag5 = DomainManager.Character.HasRelation(taiwuCharId, charId, 32768);
									if (flag5)
									{
										potentialCharIds.Add(charId);
									}
								}
							}
						}
					}
				}
				int selectedCharId = potentialCharIds.GetRandomOrDefault(context.Random, -1);
				context.AdvanceMonthRelatedData.CharIdList.Release(ref potentialCharIds);
				context.AdvanceMonthRelatedData.Blocks.Release(ref neighborBlocks);
				bool flag6 = selectedCharId < 0;
				if (flag6)
				{
					result = false;
				}
				else
				{
					GameData.Domains.Character.Character selectedChar = DomainManager.Character.GetElement_Objects(selectedCharId);
					DomainManager.Character.SimulateEnemyAttack(context, 728, selectedChar);
					int currDate = DomainManager.World.GetCurrDate();
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
					MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
					lifeRecordCollection.AddDLCLoongRidingEffectYazi(taiwuCharId, currDate, selectedCharId, taiwuLocation, childOfLoong.Id);
					lifeRecordCollection.AddDLCLoongRidingEffectYazi2(selectedCharId, currDate, taiwuLocation, childOfLoong.Id);
					monthlyEventCollection.AddDLCLoongRidingEffectYazi(taiwuCharId, childOfLoong.Id, selectedCharId);
					monthlyNotifications.AddDLCLoongRidingEffectYazi(selectedCharId, taiwuLocation, childOfLoong.Id);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06008162 RID: 33122 RVA: 0x004D22AC File Offset: 0x004D04AC
		private bool PostAdvanceMonth_ChildOfLoong_Chaofeng(DataContext context, ChildrenOfLoong childOfLoong)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			List<int> taiwuSecretInfoCollection = DomainManager.Information.GetSecretInformationOfCharacter(taiwuCharId, false);
			Location taiwuLocation = taiwuChar.GetLocation();
			bool flag = !taiwuLocation.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				List<MapBlockData> neighborBlocks = context.AdvanceMonthRelatedData.Blocks.Occupy();
				DomainManager.Map.GetNeighborBlocks(taiwuLocation.AreaId, taiwuLocation.BlockId, neighborBlocks, 3);
				List<int> potentialCharIds = context.AdvanceMonthRelatedData.CharIdList.Occupy();
				foreach (MapBlockData neighborBlock in neighborBlocks)
				{
					bool flag2 = neighborBlock.CharacterSet == null;
					if (!flag2)
					{
						foreach (int charId in neighborBlock.CharacterSet)
						{
							List<int> charSecrets = DomainManager.Information.GetSecretInformationOfCharacter(charId, false);
							bool flag3 = charSecrets == null || charSecrets.Count == 0;
							if (!flag3)
							{
								bool flag4 = taiwuSecretInfoCollection == null || taiwuSecretInfoCollection.Count == 0;
								if (flag4)
								{
									potentialCharIds.Add(charId);
								}
								else
								{
									foreach (int infoId in charSecrets)
									{
										bool flag5 = taiwuSecretInfoCollection.Contains(infoId);
										if (flag5)
										{
											potentialCharIds.Add(charId);
											break;
										}
									}
								}
							}
						}
					}
				}
				int selectedCharId = potentialCharIds.GetRandomOrDefault(context.Random, -1);
				context.AdvanceMonthRelatedData.CharIdList.Release(ref potentialCharIds);
				context.AdvanceMonthRelatedData.Blocks.Release(ref neighborBlocks);
				bool flag6 = selectedCharId < 0;
				if (flag6)
				{
					result = false;
				}
				else
				{
					List<int> targetInfoCollection = DomainManager.Information.GetSecretInformationOfCharacter(selectedCharId, false);
					List<int> metaIdList = context.AdvanceMonthRelatedData.IntList.Occupy();
					bool flag7 = taiwuSecretInfoCollection == null;
					if (flag7)
					{
						metaIdList.AddRange(targetInfoCollection);
					}
					else
					{
						foreach (int secretInformationId in targetInfoCollection)
						{
							bool flag8 = !taiwuSecretInfoCollection.Contains(secretInformationId);
							if (flag8)
							{
								metaIdList.Add(secretInformationId);
							}
						}
					}
					int selectedSecretInfoId = metaIdList.GetRandom(context.Random);
					DomainManager.Information.ReceiveSecretInformation(context, selectedSecretInfoId, taiwuCharId, selectedCharId);
					context.AdvanceMonthRelatedData.IntList.Release(ref metaIdList);
					int currDate = DomainManager.World.GetCurrDate();
					Location location = taiwuChar.GetLocation();
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
					MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
					monthlyEventCollection.AddDLCLoongRidingEffectChaofeng(taiwuCharId, childOfLoong.Id, selectedCharId);
					lifeRecordCollection.AddDLCLoongRidingEffectChaofeng(taiwuCharId, currDate, location, childOfLoong.Id);
					monthlyNotifications.AddDLCLoongRidingEffectChaofeng(location, childOfLoong.Id);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06008163 RID: 33123 RVA: 0x004D262C File Offset: 0x004D082C
		private bool PostAdvanceMonth_ChildOfLoong_Pulao(DataContext context, ChildrenOfLoong childOfLoong)
		{
			ItemKey cricketKey = DomainManager.Item.CreateCricketByLuckPoint(context, ref this.PulaoCricketLuckPoint, 1);
			DomainManager.Taiwu.GetTaiwu().AddInventoryItem(context, cricketKey, 1, false);
			GameData.Domains.Item.Cricket cricket = DomainManager.Item.GetElement_Crickets(cricketKey.Id);
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			int currDate = DomainManager.World.GetCurrDate();
			short colorId = cricket.GetColorId();
			short partId = cricket.GetPartId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
			lifeRecordCollection.AddDLCLoongRidingEffectPulao(taiwuCharId, currDate, childOfLoong.Id, colorId, partId);
			monthlyNotifications.AddDLCLoongRidingEffectPulao(childOfLoong.Id, colorId, partId);
			monthlyEventCollection.AddDLCLoongRidingEffectPulao(taiwuCharId, childOfLoong.Id, colorId, partId);
			return true;
		}

		// Token: 0x06008164 RID: 33124 RVA: 0x004D26FC File Offset: 0x004D08FC
		private bool PostAdvanceMonth_ChildOfLoong_Suanni(DataContext context, ChildrenOfLoong childOfLoong)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Inventory inventory = taiwu.GetInventory();
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> combatSkills = DomainManager.CombatSkill.GetCharCombatSkills(taiwu.GetId());
			List<ItemKey> potentialBooks = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventory.Items)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				bool flag = itemKey.ItemType != 10;
				if (!flag)
				{
					SkillBookItem bookCfg = Config.SkillBook.Instance[itemKey.TemplateId];
					bool flag2 = bookCfg.ItemSubType != 1001;
					if (!flag2)
					{
						GameData.Domains.CombatSkill.CombatSkill combatSkill;
						bool flag3 = !combatSkills.TryGetValue(bookCfg.CombatSkillTemplateId, out combatSkill);
						if (!flag3)
						{
							GameData.Domains.Item.SkillBook book = DomainManager.Item.GetElement_SkillBooks(itemKey.Id);
							byte pageTypes = book.GetPageTypes();
							ushort readingState = combatSkill.GetReadingState();
							sbyte outlinePageType = SkillBookStateHelper.GetOutlinePageType(pageTypes);
							byte outlinePageInternalIndex = CombatSkillStateHelper.GetOutlinePageInternalIndex(outlinePageType);
							bool hasReadPage = CombatSkillStateHelper.IsPageRead(readingState, outlinePageInternalIndex);
							bool hasUnreadPage = !hasReadPage;
							for (byte i = 1; i < 6; i += 1)
							{
								sbyte direction = SkillBookStateHelper.GetNormalPageType(pageTypes, i);
								byte internalIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(direction, i);
								bool flag4 = CombatSkillStateHelper.IsPageRead(readingState, internalIndex);
								if (flag4)
								{
									hasReadPage = true;
								}
								else
								{
									hasUnreadPage = true;
								}
							}
							bool flag5 = hasReadPage && hasUnreadPage;
							if (flag5)
							{
								potentialBooks.Add(itemKey);
							}
						}
					}
				}
			}
			bool hasPotentialBook = potentialBooks.Count > 0;
			bool flag6 = hasPotentialBook;
			if (flag6)
			{
				ItemKey selectedBookKey = potentialBooks.GetRandom(context.Random);
				GameData.Domains.Item.SkillBook selectedBook = DomainManager.Item.GetElement_SkillBooks(selectedBookKey.Id);
				byte pageTypes2 = selectedBook.GetPageTypes();
				GameData.Domains.CombatSkill.CombatSkill combatSkill2 = combatSkills[selectedBook.GetCombatSkillTemplateId()];
				ushort readingState2 = combatSkill2.GetReadingState();
				sbyte outlinePageType2 = SkillBookStateHelper.GetOutlinePageType(pageTypes2);
				for (byte pageId = 0; pageId < 6; pageId += 1)
				{
					sbyte direction2 = SkillBookStateHelper.GetNormalPageType(pageTypes2, pageId);
					byte internalIndex2 = CombatSkillStateHelper.GetPageInternalIndex(outlinePageType2, direction2, pageId);
					bool flag7 = CombatSkillStateHelper.IsPageRead(readingState2, internalIndex2);
					if (!flag7)
					{
						DomainManager.Taiwu.ReadSkillBookPageAndSetComplete(context, selectedBook, pageId);
						int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
						int currDate = DomainManager.World.GetCurrDate();
						Location location = taiwu.GetLocation();
						LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
						MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
						MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
						monthlyEventCollection.AddDLCLoongRidingEffectSuanni(taiwuCharId, childOfLoong.Id, selectedBookKey.ItemType, selectedBookKey.TemplateId, (int)(pageId + 1));
						lifeRecordCollection.AddDLCLoongRidingEffectSuanni(taiwuCharId, currDate, location, childOfLoong.Id, selectedBookKey.ItemType, selectedBookKey.TemplateId, (int)(pageId + 1));
						monthlyNotifications.AddDLCLoongRidingEffectSuanni(location, childOfLoong.Id, selectedBookKey.ItemType, selectedBookKey.TemplateId, (int)(pageId + 1));
						break;
					}
				}
			}
			context.AdvanceMonthRelatedData.ItemKeys.Release(ref potentialBooks);
			return hasPotentialBook;
		}

		// Token: 0x06008165 RID: 33125 RVA: 0x004D2A38 File Offset: 0x004D0C38
		private bool PostAdvanceMonth_ChildOfLoong_Baxia(DataContext context, ChildrenOfLoong childOfLoong)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Location taiwuLocation = taiwuChar.GetLocation();
			bool flag = !taiwuLocation.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				List<MapBlockData> neighborBlocks = context.AdvanceMonthRelatedData.Blocks.Occupy();
				DomainManager.Map.GetNeighborBlocks(taiwuLocation.AreaId, taiwuLocation.BlockId, neighborBlocks, 3);
				for (int i = neighborBlocks.Count - 1; i >= 0; i--)
				{
					MapBlockData block = neighborBlocks[i];
					bool flag2 = block.Items == null;
					if (flag2)
					{
						neighborBlocks.RemoveAt(i);
					}
				}
				int count = context.Random.Next(3) + 1;
				bool succeed = false;
				for (int j = 0; j < count; j++)
				{
					bool flag3 = neighborBlocks.Count == 0;
					if (flag3)
					{
						break;
					}
					int selectedBlockIndex = context.Random.Next(neighborBlocks.Count);
					MapBlockData selectedBlock = neighborBlocks[selectedBlockIndex];
					ItemKeyAndDate itemAndDates = selectedBlock.Items.Keys.GetRandom(context.Random);
					ItemKey itemKey = itemAndDates.ItemKey;
					int amount = selectedBlock.Items[itemAndDates];
					succeed = true;
					DomainManager.Map.RemoveBlockItem(context, selectedBlock, itemAndDates);
					taiwuChar.AddInventoryItem(context, itemKey, amount, false);
					bool flag4 = selectedBlock.Items == null;
					if (flag4)
					{
						neighborBlocks.RemoveAt(selectedBlockIndex);
					}
				}
				context.AdvanceMonthRelatedData.Blocks.Release(ref neighborBlocks);
				bool flag5 = succeed;
				if (flag5)
				{
					int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
					int currDate = DomainManager.World.GetCurrDate();
					Location location = taiwuChar.GetLocation();
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
					MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
					lifeRecordCollection.AddDLCLoongRidingEffectBaxia(taiwuCharId, currDate, location, childOfLoong.Id);
					monthlyEventCollection.AddDLCLoongRidingEffectBaxia(taiwuCharId, childOfLoong.Id);
					monthlyNotifications.AddDLCLoongRidingEffectBaxia(taiwuCharId, location, childOfLoong.Id);
				}
				result = succeed;
			}
			return result;
		}

		// Token: 0x06008166 RID: 33126 RVA: 0x004D2C4C File Offset: 0x004D0E4C
		private bool PostAdvanceMonth_ChildOfLoong_Bian(DataContext context, ChildrenOfLoong childOfLoong)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int currDate = DomainManager.World.GetCurrDate();
			List<FameActionRecord> fameActionRecords = taiwu.GetFameActionRecords();
			List<ValueTuple<short, short>> weightTable = context.AdvanceMonthRelatedData.WeightTable.Occupy();
			foreach (FameActionRecord record in fameActionRecords)
			{
				bool flag = record.EndDate <= currDate;
				if (!flag)
				{
					FameActionItem fameActionCfg = FameAction.Instance[record.Id];
					weightTable.Add(new ValueTuple<short, short>(record.Id, fameActionCfg.MaxStackCount));
				}
			}
			bool flag2 = weightTable.Count == 0;
			bool result;
			if (flag2)
			{
				context.AdvanceMonthRelatedData.WeightTable.Release(ref weightTable);
				result = false;
			}
			else
			{
				short fameActionTemplateId = RandomUtils.GetRandomResult<short>(weightTable, context.Random);
				context.AdvanceMonthRelatedData.WeightTable.Release(ref weightTable);
				taiwu.RecordFameAction(context, fameActionTemplateId, -1, 1, true);
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				Location location = taiwu.GetLocation();
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
				lifeRecordCollection.AddDLCLoongRidingEffectBian(taiwuCharId, currDate, location, childOfLoong.Id);
				monthlyEventCollection.AddDLCLoongRidingEffectBian(taiwuCharId, childOfLoong.Id);
				monthlyNotifications.AddDLCLoongRidingEffectBian(location, childOfLoong.Id);
				result = false;
			}
			return result;
		}

		// Token: 0x06008167 RID: 33127 RVA: 0x004D2DD8 File Offset: 0x004D0FD8
		private bool PostAdvanceMonth_ChildOfLoong_Fuxi(DataContext context, ChildrenOfLoong childOfLoong)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Inventory inventory = taiwu.GetInventory();
			List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = taiwu.GetLearnedLifeSkills();
			List<ItemKey> potentialBooks = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventory.Items)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				bool flag = itemKey.ItemType != 10;
				if (!flag)
				{
					SkillBookItem bookCfg = Config.SkillBook.Instance[itemKey.TemplateId];
					bool flag2 = bookCfg.ItemSubType != 1000;
					if (!flag2)
					{
						int learnedLifeSkillIndex = taiwu.FindLearnedLifeSkillIndex(bookCfg.LifeSkillTemplateId);
						bool flag3 = learnedLifeSkillIndex < 0;
						if (!flag3)
						{
							GameData.Domains.Character.LifeSkillItem learnedLifeSkill = learnedLifeSkills[learnedLifeSkillIndex];
							bool flag4 = learnedLifeSkill.IsAllPagesRead() || learnedLifeSkill.ReadingState == 0;
							if (!flag4)
							{
								potentialBooks.Add(itemKey);
							}
						}
					}
				}
			}
			bool flag5 = potentialBooks.Count == 0;
			bool result;
			if (flag5)
			{
				context.AdvanceMonthRelatedData.ItemKeys.Release(ref potentialBooks);
				result = false;
			}
			else
			{
				ItemKey selectedBookKey = potentialBooks.GetRandom(context.Random);
				context.AdvanceMonthRelatedData.ItemKeys.Release(ref potentialBooks);
				GameData.Domains.Item.SkillBook selectedBook = DomainManager.Item.GetElement_SkillBooks(selectedBookKey.Id);
				int lifeSkillIndex = taiwu.FindLearnedLifeSkillIndex(selectedBook.GetLifeSkillTemplateId());
				GameData.Domains.Character.LifeSkillItem lifeSkillItem = learnedLifeSkills[lifeSkillIndex];
				for (byte pageId = 0; pageId < 5; pageId += 1)
				{
					bool flag6 = lifeSkillItem.IsPageRead(pageId);
					if (!flag6)
					{
						DomainManager.Taiwu.ReadSkillBookPageAndSetComplete(context, selectedBook, pageId);
						int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
						int currDate = DomainManager.World.GetCurrDate();
						Location location = taiwu.GetLocation();
						LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
						MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
						MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
						monthlyEventCollection.AddDLCLoongRidingEffectFuxi(taiwuCharId, childOfLoong.Id, selectedBookKey.ItemType, selectedBookKey.TemplateId, (int)(pageId + 1));
						lifeRecordCollection.AddDLCLoongRidingEffectFuxi(taiwuCharId, currDate, location, childOfLoong.Id, selectedBookKey.ItemType, selectedBookKey.TemplateId, (int)(pageId + 1));
						monthlyNotifications.AddDLCLoongRidingEffectFuxi(location, childOfLoong.Id, selectedBookKey.ItemType, selectedBookKey.TemplateId, (int)(pageId + 1));
						break;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06008168 RID: 33128 RVA: 0x004D306C File Offset: 0x004D126C
		private bool PostAdvanceMonth_ChildOfLoong_Chiwen(DataContext context, ChildrenOfLoong childOfLoong)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int currNeili = taiwu.GetCurrNeili();
			int maxNeili = taiwu.GetMaxNeili();
			int neiliRecovery = taiwu.GetCurrNeiliRecovery(maxNeili);
			bool flag = maxNeili - currNeili < neiliRecovery * 3;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int recoverAmount = neiliRecovery * (context.Random.Next(3) + 1);
				taiwu.ChangeCurrNeili(context, recoverAmount);
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				int currDate = DomainManager.World.GetCurrDate();
				Location location = taiwu.GetLocation();
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
				lifeRecordCollection.AddDLCLoongRidingEffectChiwen(taiwuCharId, currDate, location, childOfLoong.Id);
				monthlyEventCollection.AddDLCLoongRidingEffectChiwen(taiwuCharId, childOfLoong.Id);
				monthlyNotifications.AddDLCLoongRidingEffectChiwen(location, childOfLoong.Id);
				result = false;
			}
			return result;
		}

		// Token: 0x06008169 RID: 33129 RVA: 0x004D314C File Offset: 0x004D134C
		private unsafe void PostAdvanceMonth_FiveLoongs(DataContext context)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			HashSet<int> handledCharSet = new HashSet<int>();
			HashSet<int> taiwuGroup = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			List<int> charIdList = context.AdvanceMonthRelatedData.CharIdList.Occupy();
			foreach (KeyValuePair<short, LoongInfo> keyValuePair in DomainManager.Extra.FiveLoongDict)
			{
				short num;
				LoongInfo loongInfo2;
				keyValuePair.Deconstruct(out num, out loongInfo2);
				short loongId = num;
				LoongInfo loongInfo = loongInfo2;
				Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(loongInfo.LoongCurrentLocation.AreaId);
				LoongItem loongCfg = loongInfo.ConfigData;
				handledCharSet.Clear();
				handledCharSet.UnionWith(taiwuGroup);
				bool flag = loongInfo.CoveredMapBlockTemplateId != null;
				if (flag)
				{
					foreach (short blockId in loongInfo.CoveredMapBlockTemplateId.Keys)
					{
						MapBlockData block = *areaBlocks[(int)blockId];
						bool flag2 = block.CharacterSet == null;
						if (!flag2)
						{
							foreach (int charId in block.CharacterSet)
							{
								GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
								bool flag3 = FiveLoongDlcEntry.OfflineTryApplyLoongBlockDebuff(loongInfo, character) != 0;
								if (flag3)
								{
									handledCharSet.Add(charId);
								}
							}
						}
					}
				}
				bool flag4 = loongInfo.CharacterDebuffCounts != null;
				if (flag4)
				{
					charIdList.Clear();
					charIdList.AddRange(loongInfo.CharacterDebuffCounts.Keys);
					foreach (int charId2 in charIdList)
					{
						bool flag5 = handledCharSet.Contains(charId2);
						if (!flag5)
						{
							GameData.Domains.Character.Character character2;
							bool flag6 = DomainManager.Character.TryGetElement_Objects(charId2, out character2);
							if (flag6)
							{
								FiveLoongDlcEntry.OfflineTryReduceLoongBlockDebuff(loongInfo, character2);
							}
							else
							{
								loongInfo.CharacterDebuffCounts.Remove(charId2);
							}
						}
					}
				}
				charIdList.Clear();
				Location loongLocation = loongInfo.LoongCurrentLocation;
				bool flag7 = !loongInfo.IsDisappear;
				if (flag7)
				{
					MapBlockData loongBlockData = *areaBlocks[(int)loongLocation.BlockId];
					bool flag8 = loongBlockData.CharacterSet != null;
					if (flag8)
					{
						charIdList.AddRange(loongBlockData.CharacterSet);
						int targetCharId = charIdList.GetRandom(context.Random);
						GameData.Domains.Character.Character targetChar = DomainManager.Character.GetElement_Objects(targetCharId);
						AiHelper.NpcCombatResultType resultType = DomainManager.Character.SimulateEnemyAttack(context, loongInfo.CharacterTemplateId, targetChar);
						bool flag9 = resultType == AiHelper.NpcCombatResultType.MajorVictory || resultType == AiHelper.NpcCombatResultType.MinorVictory;
						if (flag9)
						{
							lifeRecordCollection.AddDefeatedByLoong(targetCharId, currDate, loongInfo.CharacterTemplateId, loongLocation);
						}
						else
						{
							lifeRecordCollection.AddDefeatLoong(targetCharId, currDate, loongInfo.CharacterTemplateId, loongLocation);
						}
					}
					List<short> blockIds = context.AdvanceMonthRelatedData.BlockIds.Occupy();
					Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(loongLocation.AreaId);
					foreach (short blockId2 in loongInfo.CoveredMapBlockTemplateId.Keys)
					{
						bool flag10 = FiveLoongDlcEntry.IsBlockLoongBlock(*blocks[(int)blockId2]);
						if (flag10)
						{
							blockIds.Add(blockId2);
						}
					}
					short randomBlockId = blockIds.GetRandom(context.Random);
					context.AdvanceMonthRelatedData.BlockIds.Release(ref blockIds);
					DomainManager.Extra.RemoveAnimalByLocationAndTemplateId(context, loongInfo.LoongCurrentLocation, loongInfo.CharacterTemplateId);
					loongInfo.LoongCurrentLocation = new Location(loongInfo.LoongCurrentLocation.AreaId, randomBlockId);
					DomainManager.Extra.CreateAnimalByCharacterTemplateId(context, loongInfo.CharacterTemplateId, loongInfo.LoongCurrentLocation);
				}
				else
				{
					bool flag11 = loongInfo.DisappearDate + 108 <= DomainManager.World.GetCurrDate();
					if (flag11)
					{
						bool isDisappear = loongInfo.IsDisappear;
						if (isDisappear)
						{
							bool flag12 = !FiveLoongDlcEntry.TryCreateOrReAppearFiveLoong(context, true, loongInfo.CharacterTemplateId);
							if (flag12)
							{
								FiveLoongDlcEntry.TryCreateOrReAppearFiveLoong(context, false, loongInfo.CharacterTemplateId);
							}
						}
					}
				}
				Dictionary<short, List<int>> animalAreaData;
				bool flag13 = DomainManager.Extra.TryGetAnimalAreaDataByAreaId(loongLocation.AreaId, out animalAreaData);
				if (flag13)
				{
					List<GameData.Domains.Character.Animal> animalsToRemove = null;
					foreach (KeyValuePair<short, List<int>> keyValuePair2 in animalAreaData)
					{
						List<int> list;
						keyValuePair2.Deconstruct(out num, out list);
						short blockId3 = num;
						List<int> animalIds = list;
						GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
						Location taiwuLocation = taiwu.GetLocation();
						int taiwuCharId = taiwu.GetId();
						short minionTemplateId = loongCfg.MinionCharTemplateId;
						foreach (int animalId in animalIds)
						{
							GameData.Domains.Character.Animal animal;
							bool flag14 = !DomainManager.Extra.TryGetAnimal(animalId, out animal) || animal.CharacterTemplateId != minionTemplateId;
							if (!flag14)
							{
								charIdList.Clear();
								MapBlockData blockData = *areaBlocks[(int)blockId3];
								Location location = blockData.GetLocation();
								bool flag15 = blockData.CharacterSet != null;
								if (flag15)
								{
									charIdList.AddRange(blockData.CharacterSet);
								}
								bool flag16 = location == taiwuLocation;
								if (flag16)
								{
									charIdList.Add(taiwuCharId);
								}
								bool flag17 = charIdList.Count == 0;
								if (!flag17)
								{
									int selectedCharId = charIdList.GetRandom(context.Random);
									GameData.Domains.Character.Character selectedChar = DomainManager.Character.GetElement_Objects(selectedCharId);
									bool flag18 = selectedCharId == taiwuCharId;
									if (flag18)
									{
										MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
										monthlyEventCollection.AddMinionLoongAttack(taiwuCharId, loongLocation, minionTemplateId);
									}
									else
									{
										AiHelper.NpcCombatResultType resultType2 = DomainManager.Character.SimulateEnemyAttack(context, minionTemplateId, selectedChar);
										bool flag19 = resultType2 == AiHelper.NpcCombatResultType.MajorVictory || resultType2 == AiHelper.NpcCombatResultType.MinorVictory;
										if (flag19)
										{
											lifeRecordCollection.AddDefeatedByAnimal(selectedCharId, currDate, location, animal.CharacterTemplateId);
										}
										else
										{
											if (animalsToRemove == null)
											{
												animalsToRemove = new List<GameData.Domains.Character.Animal>();
											}
											animalsToRemove.Add(animal);
											lifeRecordCollection.AddKillAnimal(selectedCharId, currDate, location, animal.CharacterTemplateId);
										}
									}
								}
							}
						}
					}
					bool flag20 = animalsToRemove != null;
					if (flag20)
					{
						foreach (GameData.Domains.Character.Animal animal2 in animalsToRemove)
						{
							DomainManager.Extra.RemoveAnimal(context, animal2);
						}
					}
				}
				DomainManager.Extra.SetLoongInfo(context, loongId, loongInfo);
			}
			context.AdvanceMonthRelatedData.CharIdList.Release(ref charIdList);
		}

		// Token: 0x0600816A RID: 33130 RVA: 0x004D38D8 File Offset: 0x004D1AD8
		private static int OfflineTryApplyLoongBlockDebuff(LoongInfo loongInfo, GameData.Domains.Character.Character character)
		{
			int charId = character.GetId();
			LoongItem loongCfg = loongInfo.ConfigData;
			sbyte personality = character.GetPersonality(loongCfg.PersonalityType);
			bool hasClothing = false;
			ItemKey itemKey = character.GetEquipment()[4];
			bool flag = itemKey.IsValid() && !DomainManager.Item.GetBaseItem(itemKey).IsDurabilityRunningOut() && itemKey.TemplateId == loongCfg.ClothingTemplateId;
			if (flag)
			{
				hasClothing = true;
			}
			bool flag2 = personality < loongCfg.PersonalityRequirement && !hasClothing;
			int result;
			if (flag2)
			{
				loongInfo.ChangeCharacterDebuffCount(charId, 1);
				result = 1;
			}
			else
			{
				bool flag3 = loongInfo.GetCharacterDebuffCount(charId) > 0;
				if (flag3)
				{
					loongInfo.ChangeCharacterDebuffCount(charId, -1);
					result = -1;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0600816B RID: 33131 RVA: 0x004D3994 File Offset: 0x004D1B94
		private static int OfflineTryReduceLoongBlockDebuff(LoongInfo loongInfo, GameData.Domains.Character.Character character)
		{
			int charId = character.GetId();
			bool flag = loongInfo.CharacterDebuffCounts == null || !loongInfo.CharacterDebuffCounts.ContainsKey(charId);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				loongInfo.ChangeCharacterDebuffCount(charId, -1);
				result = -1;
			}
			return result;
		}

		// Token: 0x0600816C RID: 33132 RVA: 0x004D39DC File Offset: 0x004D1BDC
		private static void OnTaiwuMove(DataContext context, MapBlockData srcBlock, MapBlockData destBlock, int actionPointCost)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int taiwuCharId = taiwu.GetId();
			MapBlockItem blockConfig = destBlock.GetConfig();
			foreach (KeyValuePair<short, LoongInfo> keyValuePair in DomainManager.Extra.FiveLoongDict)
			{
				short num;
				LoongInfo loongInfo2;
				keyValuePair.Deconstruct(out num, out loongInfo2);
				short loongId = num;
				LoongInfo loongInfo = loongInfo2;
				LoongItem loongCfg = loongInfo.ConfigData;
				bool isLoongBlock = loongCfg.MapBlock == blockConfig.TemplateId;
				int delta = isLoongBlock ? FiveLoongDlcEntry.OfflineTryApplyLoongBlockDebuff(loongInfo, taiwu) : FiveLoongDlcEntry.OfflineTryReduceLoongBlockDebuff(loongInfo, taiwu);
				bool isModified = delta != 0;
				bool flag = isModified;
				if (flag)
				{
					ushort count;
					FiveLoongDlcEntry.AddLoongDebuffInstantNotification((delta > 0) ? loongCfg.DebuffCountIncNotification : loongCfg.DebuffCountDecNotification, (int)((loongInfo.CharacterDebuffCounts != null && loongInfo.CharacterDebuffCounts.TryGetValue(taiwuCharId, out count)) ? count : 0));
				}
				HashSet<int> taiwuGroup = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
				foreach (int groupCharId in taiwuGroup)
				{
					bool flag2 = groupCharId == taiwuCharId;
					if (!flag2)
					{
						GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(groupCharId);
						delta = (isLoongBlock ? FiveLoongDlcEntry.OfflineTryApplyLoongBlockDebuff(loongInfo, character) : FiveLoongDlcEntry.OfflineTryReduceLoongBlockDebuff(loongInfo, character));
						bool flag3 = delta != 0;
						if (flag3)
						{
							isModified = true;
						}
					}
				}
				bool flag4 = isModified;
				if (flag4)
				{
					DomainManager.Extra.SetLoongInfo(context, loongId, loongInfo);
				}
			}
		}

		// Token: 0x0600816D RID: 33133 RVA: 0x004D3BA4 File Offset: 0x004D1DA4
		public static void OnDefeatFiveLoongInCombat(DataContext context, sbyte combatStatus)
		{
			CombatConfigItem combatConfig = DomainManager.Combat.CombatConfig;
			bool flag = combatStatus != CombatStatusType.EnemyFail || combatConfig.TemplateId < 182 || combatConfig.TemplateId > 186;
			if (!flag)
			{
				short legacyId = DomainManager.Taiwu.GetRandomAvailableLegacy(context.Random, -1);
				bool flag2 = DomainManager.Taiwu.AddAvailableLegacy(context, legacyId);
				if (flag2)
				{
					DomainManager.Combat.AddCombatResultLegacy(legacyId);
				}
				short legacyId2 = DomainManager.Taiwu.GetRandomAvailableLegacy(context.Random, -1);
				bool flag3 = DomainManager.Taiwu.AddAvailableLegacy(context, legacyId2);
				if (flag3)
				{
					DomainManager.Combat.AddCombatResultLegacy(legacyId2);
				}
				short legacyId3 = DomainManager.Taiwu.GetRandomAvailableLegacy(context.Random, -1);
				bool flag4 = DomainManager.Taiwu.AddAvailableLegacy(context, legacyId3);
				if (flag4)
				{
					DomainManager.Combat.AddCombatResultLegacy(legacyId3);
				}
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x0600816E RID: 33134 RVA: 0x004D3C85 File Offset: 0x004D1E85
		public static IReadOnlyDictionary<short, short> CarrierToJiaoTemplate
		{
			get
			{
				return FiveLoongDlcEntry._carrierToJiaoTemplate;
			}
		}

		// Token: 0x0600816F RID: 33135 RVA: 0x004D3C8C File Offset: 0x004D1E8C
		public static bool IsCharacterMinionLoong(short charTemplateId)
		{
			return charTemplateId >= 691 && charTemplateId <= 695;
		}

		// Token: 0x06008170 RID: 33136 RVA: 0x004D3CB4 File Offset: 0x004D1EB4
		public static bool IsCharacterLoong(short charTemplateId)
		{
			return charTemplateId >= 686 && charTemplateId <= 690;
		}

		// Token: 0x06008171 RID: 33137 RVA: 0x004D3CDC File Offset: 0x004D1EDC
		public static short MinionLoongToLoong(short minionCharTemplateId)
		{
			return minionCharTemplateId - 691 + 686;
		}

		// Token: 0x06008172 RID: 33138 RVA: 0x004D3CFC File Offset: 0x004D1EFC
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
			case 122:
				instantNotificationCollection.AddBlazePowerGrow(debuffCount);
				break;
			case 123:
				instantNotificationCollection.AddStormPowerGrow(debuffCount);
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
			case 127:
				instantNotificationCollection.AddBlazePowerDecline(debuffCount);
				break;
			case 128:
				instantNotificationCollection.AddStormPowerDecline(debuffCount);
				break;
			case 129:
				instantNotificationCollection.AddSandPowerDecline(debuffCount);
				break;
			}
		}

		// Token: 0x06008173 RID: 33139 RVA: 0x004D3DB0 File Offset: 0x004D1FB0
		public static bool IsBlockLoongBlock(MapBlockData block)
		{
			return MapBlock.Instance[block.TemplateId].SubType == EMapBlockSubType.DLCLoong;
		}

		// Token: 0x06008174 RID: 33140 RVA: 0x004D3DDC File Offset: 0x004D1FDC
		private void UpdateMaxTaiwuVillageLevel()
		{
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData taiwuBuildingAreaData = DomainManager.Building.GetBuildingAreaData(taiwuVillageLocation);
			BuildingBlockKey buildingBlockKey = BuildingDomain.FindBuildingKey(taiwuVillageLocation, taiwuBuildingAreaData, 44, true);
			this.MaxTaiwuVillageLevel = Math.Max(this.MaxTaiwuVillageLevel, (int)DomainManager.Building.BuildingBlockLevel(buildingBlockKey));
		}

		// Token: 0x06008175 RID: 33141 RVA: 0x004D3E28 File Offset: 0x004D2028
		private void FixAbnormalJiaoNextPeriodData(DataContext context)
		{
			foreach (Jiao jiao in DomainManager.Extra.GetAllJiaos().Values)
			{
				bool flag = jiao.GrowthStage == 1 && jiao.NurturanceTemplateId != 0 && jiao.EvolveRemainingMonth != 0;
				if (flag)
				{
					short cost = JiaoNurturance.Instance[jiao.NurturanceTemplateId].StageCostMonth;
					int value = jiao.EvolveRemainingMonth % (int)cost;
					jiao.NextPeriod = ((value == 0) ? ((int)cost) : value);
					DomainManager.Extra.SetJiao(context, jiao);
					Logger logger = FiveLoongDlcEntry.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Fix Jiao NextPeriod: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(jiao.Id);
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x06008176 RID: 33142 RVA: 0x004D3F20 File Offset: 0x004D2120
		private void FixAbnormalJiaoPriceData(DataContext context)
		{
			foreach (Jiao jiao in DomainManager.Extra.GetAllJiaos().Values)
			{
				bool flag = (jiao.Properties.Value.Item1 + jiao.Properties.Value.Item2 / 100) * 2 < jiao.Properties.ExploreBonusRate.Item1 + jiao.Properties.ExploreBonusRate.Item2 / 100;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag)
				{
					Logger logger = FiveLoongDlcEntry.Logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Fix Jiao Value: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(jiao.Id);
					defaultInterpolatedStringHandler.AppendLiteral(",  ");
					defaultInterpolatedStringHandler.AppendFormatted<ValueTuple<int, int>>(jiao.Properties.Value);
					defaultInterpolatedStringHandler.AppendLiteral(" -> ");
					defaultInterpolatedStringHandler.AppendFormatted<ValueTuple<int, int>>(jiao.Properties.ExploreBonusRate);
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					jiao.Properties.Value = jiao.Properties.ExploreBonusRate;
				}
				Logger logger2 = FiveLoongDlcEntry.Logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Fix Jiao ExploreBonusRate: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(jiao.Id);
				defaultInterpolatedStringHandler.AppendLiteral(",  ");
				defaultInterpolatedStringHandler.AppendFormatted<ValueTuple<int, int>>(jiao.Properties.ExploreBonusRate);
				defaultInterpolatedStringHandler.AppendLiteral(" -> ");
				defaultInterpolatedStringHandler.AppendFormatted<ValueTuple<int, int>>(jiao.Properties.CaptureRateBonus);
				logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				jiao.Properties.ExploreBonusRate = jiao.Properties.CaptureRateBonus;
				bool flag2 = jiao.NurturanceTemplateId == 7;
				if (flag2)
				{
					jiao.NurturanceTemplateId = 10;
					Logger logger3 = FiveLoongDlcEntry.Logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Fix Jiao Nurturance: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(jiao.Id);
					defaultInterpolatedStringHandler.AppendLiteral(",  Shaving -> WrapTeach");
					logger3.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				DomainManager.Extra.SetJiaoEvolutionChoice(jiao.Id);
				DomainManager.Extra.SetJiao(context, jiao);
			}
			foreach (ChildrenOfLoong loong in DomainManager.Extra.GetAllLoongs().Values)
			{
				bool flag3 = (loong.Properties.Value.Item1 + loong.Properties.Value.Item2 / 100) * 2 < loong.Properties.ExploreBonusRate.Item1 + loong.Properties.ExploreBonusRate.Item2 / 100;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag3)
				{
					Logger logger4 = FiveLoongDlcEntry.Logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Fix Loong Value: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(loong.Id);
					defaultInterpolatedStringHandler.AppendLiteral(",");
					defaultInterpolatedStringHandler.AppendFormatted<ValueTuple<int, int>>(loong.Properties.Value);
					defaultInterpolatedStringHandler.AppendLiteral(" -> ");
					defaultInterpolatedStringHandler.AppendFormatted<ValueTuple<int, int>>(loong.Properties.ExploreBonusRate);
					logger4.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					loong.Properties.Value = loong.Properties.ExploreBonusRate;
				}
				Logger logger5 = FiveLoongDlcEntry.Logger;
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Fix Loong ExploreBonusRate: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(loong.Id);
				defaultInterpolatedStringHandler.AppendLiteral(",");
				defaultInterpolatedStringHandler.AppendFormatted<ValueTuple<int, int>>(loong.Properties.ExploreBonusRate);
				defaultInterpolatedStringHandler.AppendLiteral(" -> ");
				defaultInterpolatedStringHandler.AppendFormatted<ValueTuple<int, int>>(loong.Properties.CaptureRateBonus);
				logger5.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				loong.Properties.ExploreBonusRate = loong.Properties.CaptureRateBonus;
				DomainManager.Extra.SetChildrenOfLoong(context, loong);
			}
		}

		// Token: 0x06008177 RID: 33143 RVA: 0x004D435C File Offset: 0x004D255C
		private void FixAbnormalJiaoGenerationData(DataContext context)
		{
			foreach (Jiao jiao in DomainManager.Extra.GetAllJiaos().Values)
			{
				bool flag = jiao.Generation == 0 && (jiao.FatherId >= 0 || jiao.MotherId >= 0);
				if (flag)
				{
					jiao.Generation = 1 + Math.Max(this.FixAbnormalJiaoGenerationData(context, jiao.FatherId), this.FixAbnormalJiaoGenerationData(context, jiao.MotherId));
					DomainManager.Extra.SetJiao(context, jiao);
					Logger logger = FiveLoongDlcEntry.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Fix Jiao Generation: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(jiao.Id);
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x06008178 RID: 33144 RVA: 0x004D4450 File Offset: 0x004D2650
		private int FixAbnormalJiaoGenerationData(DataContext context, int jiaoId)
		{
			Jiao jiao;
			bool flag = DomainManager.Extra.TryGetJiao(jiaoId, out jiao);
			int result;
			if (flag)
			{
				bool flag2 = jiao.Generation == 0 && (jiao.FatherId >= 0 || jiao.MotherId >= 0);
				if (flag2)
				{
					jiao.Generation = 1 + Math.Max(this.FixAbnormalJiaoGenerationData(context, jiao.FatherId), this.FixAbnormalJiaoGenerationData(context, jiao.MotherId));
					DomainManager.Extra.SetJiao(context, jiao);
					Logger logger = FiveLoongDlcEntry.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Fix Jiao Generation: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(jiao.Id);
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = jiao.Generation;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06008179 RID: 33145 RVA: 0x004D4518 File Offset: 0x004D2718
		private void FixAbnormalJiaoEggAndTeenagerData(DataContext context)
		{
			foreach (Jiao jiao in DomainManager.Extra.GetAllJiaos().Values)
			{
				ItemKey oldKey = jiao.Key;
				bool flag = oldKey.ItemType == 12;
				if (flag)
				{
					ItemKey newKey = this.FixJiaoItemFromMiscToMaterial(context, jiao);
					Logger logger = FiveLoongDlcEntry.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Fixing jiao item from misc to material: ");
					defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(oldKey);
					defaultInterpolatedStringHandler.AppendLiteral(" => ");
					defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(newKey);
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x0600817A RID: 33146 RVA: 0x004D45DC File Offset: 0x004D27DC
		private void FixJiaoKeyInUnhandledCrossArchiveGameData(DataContext context)
		{
			CrossArchiveGameData crossArchiveGameData = DomainManager.Extra.UnhandledCrossArchiveGameData;
			bool flag = crossArchiveGameData == null;
			if (!flag)
			{
				bool flag2 = crossArchiveGameData.UnpackedItems == null;
				if (!flag2)
				{
					foreach (int itemId in crossArchiveGameData.UnpackedItems.Keys)
					{
						ItemKey oldKey = crossArchiveGameData.UnpackedItems[itemId];
						sbyte itemType = oldKey.ItemType;
						sbyte b = itemType;
						if (b == 12)
						{
							Jiao jiao;
							bool flag3 = DomainManager.Extra.TryGetJiaoByItemKey(oldKey, out jiao);
							if (flag3)
							{
								ItemKey newKey = this.FixJiaoItemFromMiscToMaterial(context, jiao);
								crossArchiveGameData.UnpackedItems[itemId] = newKey;
								Logger logger = FiveLoongDlcEntry.Logger;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(81, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Fixing jiao item from misc to material in unhandled cross archive game data: ");
								defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(oldKey);
								defaultInterpolatedStringHandler.AppendLiteral(" => ");
								defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(newKey);
								logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
							}
						}
					}
				}
			}
		}

		// Token: 0x0600817B RID: 33147 RVA: 0x004D4704 File Offset: 0x004D2904
		private void FixInvalidJiaoItems(DataContext context)
		{
			bool flag = !DomainManager.Extra.GetIsDreamBack();
			if (!flag)
			{
				bool itemUnlocked = DomainManager.Extra.IsDreamBackStateUnlocked(1);
				bool buildingUnlocked = DomainManager.Extra.IsDreamBackStateUnlocked(4);
				bool flag2 = !itemUnlocked || !buildingUnlocked;
				if (!flag2)
				{
					GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
					Inventory inventory = taiwu.GetInventory();
					List<ItemKey> toRemoveInventoryItems = new List<ItemKey>();
					foreach (KeyValuePair<ItemKey, int> keyValuePair in inventory.Items)
					{
						ItemKey itemKey5;
						int num;
						keyValuePair.Deconstruct(out itemKey5, out num);
						ItemKey itemKey = itemKey5;
						Jiao jiao;
						bool flag3 = itemKey.ItemType == 12 && itemKey.TemplateId >= 287 && itemKey.TemplateId <= 319 && !DomainManager.Extra.TryGetJiaoByItemKey(itemKey, out jiao);
						if (flag3)
						{
							toRemoveInventoryItems.Add(itemKey);
						}
					}
					List<ItemKey> toRemoveWarehouseItems = new List<ItemKey>();
					foreach (KeyValuePair<ItemKey, int> keyValuePair in DomainManager.Taiwu.WarehouseItems)
					{
						ItemKey itemKey5;
						int num;
						keyValuePair.Deconstruct(out itemKey5, out num);
						ItemKey itemKey2 = itemKey5;
						Jiao jiao2;
						bool flag4 = itemKey2.ItemType == 12 && itemKey2.TemplateId >= 287 && itemKey2.TemplateId <= 319 && !DomainManager.Extra.TryGetJiaoByItemKey(itemKey2, out jiao2);
						if (flag4)
						{
							toRemoveWarehouseItems.Add(itemKey2);
						}
					}
					bool flag5 = toRemoveInventoryItems.Count == 0 && toRemoveWarehouseItems.Count == 0;
					if (!flag5)
					{
						foreach (ItemKey itemKey3 in toRemoveInventoryItems)
						{
							bool flag6 = !DomainManager.Item.ItemExists(itemKey3);
							if (flag6)
							{
								inventory.OfflineRemove(itemKey3);
							}
							else
							{
								taiwu.RemoveInventoryItem(context, itemKey3, 1, true, false);
							}
							Logger logger = FiveLoongDlcEntry.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Fixing invalid jiao item in inventory by removing ");
							defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey3);
							defaultInterpolatedStringHandler.AppendLiteral(".");
							logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						taiwu.SetInventory(inventory, context);
						foreach (ItemKey itemKey4 in toRemoveWarehouseItems)
						{
							DomainManager.Taiwu.WarehouseRemove(context, itemKey4, 1, true);
							Logger logger2 = FiveLoongDlcEntry.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Fixing invalid jiao item in warehouse by removing ");
							defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey4);
							defaultInterpolatedStringHandler.AppendLiteral(".");
							logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						foreach (KeyValuePair<int, Jiao> keyValuePair2 in DomainManager.Extra.Jiaos)
						{
							int num;
							Jiao jiao4;
							keyValuePair2.Deconstruct(out num, out jiao4);
							Jiao jiao3 = jiao4;
							bool flag7 = jiao3.Key.ItemType != 5;
							if (!flag7)
							{
								GameData.Domains.Item.Material material;
								bool flag8 = !DomainManager.Item.TryGetElement_Materials(jiao3.Key.Id, out material);
								if (!flag8)
								{
									bool flag9 = material.Owner.OwnerType > ItemOwnerType.None;
									if (!flag9)
									{
										taiwu.AddInventoryItem(context, jiao3.Key, 1, false);
										Logger logger3 = FiveLoongDlcEntry.Logger;
										DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
										defaultInterpolatedStringHandler.AppendLiteral("Adding unowned jiao ");
										defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(jiao3.Key);
										defaultInterpolatedStringHandler.AppendLiteral(" to taiwu inventory.");
										logger3.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600817C RID: 33148 RVA: 0x004D4B30 File Offset: 0x004D2D30
		private ItemKey FixJiaoItemFromMiscToMaterial(DataContext context, Jiao jiao)
		{
			ItemKey oldKey = jiao.Key;
			short templateId = (jiao.GrowthStage == 0) ? DomainManager.Extra.GetJiaoEggTemplateIdByJiaoTemplateId(jiao.TemplateId) : DomainManager.Extra.GetJiaoTeenagerTemplateIdByJiaoTemplateId(jiao.TemplateId);
			ItemKey newKey = DomainManager.Item.CreateItem(context, 5, templateId);
			bool flag = DomainManager.Taiwu.GetItems(ItemSourceType.Inventory).ContainsKey(oldKey);
			if (flag)
			{
				DomainManager.Taiwu.RemoveItem(context, oldKey, 1, ItemSourceType.Inventory, false, false);
				DomainManager.Taiwu.AddItem(context, newKey, 1, ItemSourceType.Inventory, false);
			}
			else
			{
				bool flag2 = DomainManager.Taiwu.GetItems(ItemSourceType.Warehouse).ContainsKey(oldKey);
				if (flag2)
				{
					DomainManager.Taiwu.RemoveItem(context, oldKey, 1, ItemSourceType.Warehouse, false, false);
					DomainManager.Taiwu.AddItem(context, newKey, 1, ItemSourceType.Warehouse, false);
				}
				else
				{
					bool flag3 = DomainManager.Taiwu.GetItems(ItemSourceType.StockStorageGoodsShelf).ContainsKey(oldKey);
					if (flag3)
					{
						DomainManager.Taiwu.RemoveItem(context, oldKey, 1, ItemSourceType.StockStorageGoodsShelf, false, false);
						DomainManager.Taiwu.AddItem(context, newKey, 1, ItemSourceType.StockStorageGoodsShelf, false);
					}
					else
					{
						bool flag4 = DomainManager.Taiwu.GetItems(ItemSourceType.StockStorageWarehouse).ContainsKey(oldKey);
						if (flag4)
						{
							DomainManager.Taiwu.RemoveItem(context, oldKey, 1, ItemSourceType.StockStorageWarehouse, false, false);
							DomainManager.Taiwu.AddItem(context, newKey, 1, ItemSourceType.StockStorageWarehouse, false);
						}
						else
						{
							GameData.Domains.Character.Animal animal;
							bool flag5 = DomainManager.Extra.TryGetAnimalByItemKey(oldKey, out animal);
							if (flag5)
							{
								animal.ItemKey = newKey;
								DomainManager.Extra.SetAnimal(context, animal);
								DomainManager.Item.SetOwner(animal.ItemKey, ItemOwnerType.FleeCarrier, (int)animal.Location.AreaId);
							}
						}
					}
				}
			}
			DomainManager.Extra.SetJiaoItemKey(context, jiao.Id, jiao, newKey);
			DomainManager.Item.RemoveItem(context, oldKey);
			return newKey;
		}

		// Token: 0x0600817D RID: 33149 RVA: 0x004D4CE8 File Offset: 0x004D2EE8
		private void FixAbnormalJiaoLoongData(DataContext context, ItemKey itemKey)
		{
			int num;
			bool flag = itemKey.ItemType == 12 && !DomainManager.Extra.TryGetJiaoIdByItemKey(itemKey, out num);
			if (flag)
			{
				short templateId = itemKey.TemplateId;
				bool flag2 = templateId >= 287 && templateId <= 319;
				if (flag2)
				{
					DomainManager.Item.RemoveItem(context, itemKey);
				}
			}
			else
			{
				bool flag3 = itemKey.ItemType == 4;
				if (flag3)
				{
					short templateId = itemKey.TemplateId;
					bool flag4 = templateId >= 46 && templateId <= 76 && !DomainManager.Extra.TryGetJiaoIdByItemKey(itemKey, out num);
					if (flag4)
					{
						int id = DomainManager.Extra.AddJiao(context, itemKey, FiveLoongDlcEntry.CarrierToJiaoTemplate[itemKey.TemplateId], 0, 2, true, false, false);
						Jiao jiao = DomainManager.Extra.GetJiaoById(id);
						jiao.Properties = new JiaoProperty(50);
						DomainManager.Extra.SetJiao(context, jiao);
					}
					else
					{
						templateId = itemKey.TemplateId;
						bool flag5 = templateId >= 77 && templateId <= 85 && !DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(itemKey, out num);
						if (flag5)
						{
							int id2 = DomainManager.Extra.AddChildOfLoong(context, itemKey, FiveLoongDlcEntry.CarrierToJiaoTemplate[itemKey.TemplateId]);
							ChildrenOfLoong loong = DomainManager.Extra.GetChildrenOfLoongById(id2);
							ValueTuple<int, int> highProperty = new ValueTuple<int, int>(context.Random.Next(0, 12), context.Random.Next(12, 8));
							for (short i = 0; i <= 8; i += 1)
							{
								loong.Properties.Set(i, JiaoProperty.Instance[i].MaxValue * (((int)i == highProperty.Item1 || (int)i == highProperty.Item2) ? 75 : 50));
							}
							loong.Properties = new JiaoProperty(50);
							DomainManager.Extra.SetChildrenOfLoong(context, loong);
						}
					}
				}
			}
		}

		// Token: 0x0600817E RID: 33150 RVA: 0x004D4ED4 File Offset: 0x004D30D4
		private void FixAbnormalChildrenOfLoongData(DataContext context)
		{
			foreach (KeyValuePair<int, ChildrenOfLoong> keyValuePair in DomainManager.Extra.ChildrenOfLoong)
			{
				int num;
				ChildrenOfLoong childrenOfLoong;
				keyValuePair.Deconstruct(out num, out childrenOfLoong);
				ChildrenOfLoong loong = childrenOfLoong;
				bool flag = loong.JiaoTemplateId < 0;
				if (flag)
				{
					loong.JiaoTemplateId = 0;
					loong.LoongTemplateId = DomainManager.Extra.GetJiaoTemplateIdByCarrierTemplateId(loong.Key.TemplateId);
					loong.Gender = false;
					DomainManager.Extra.SetChildrenOfLoong(context, loong);
				}
			}
		}

		// Token: 0x0600817F RID: 33151 RVA: 0x004D4F7C File Offset: 0x004D317C
		private void FixAbnormalJiaoLoongData(DataContext context)
		{
			ItemKey equipment = DomainManager.Taiwu.GetTaiwu().GetEquipment()[11];
			bool flag = equipment.IsValid();
			if (flag)
			{
				this.FixAbnormalJiaoLoongData(context, equipment);
			}
			IReadOnlyDictionary<ItemKey, int> inventory = DomainManager.Taiwu.GetItems(ItemSourceType.Inventory);
			foreach (ItemKey itemKey in inventory.Keys)
			{
				this.FixAbnormalJiaoLoongData(context, itemKey);
			}
			IReadOnlyDictionary<ItemKey, int> warehouse = DomainManager.Taiwu.GetItems(ItemSourceType.Warehouse);
			foreach (ItemKey itemKey2 in warehouse.Keys)
			{
				this.FixAbnormalJiaoLoongData(context, itemKey2);
			}
			IReadOnlyDictionary<ItemKey, int> treasury = DomainManager.Taiwu.GetItems(ItemSourceType.StockStorageGoodsShelf);
			foreach (ItemKey itemKey3 in treasury.Keys)
			{
				this.FixAbnormalJiaoLoongData(context, itemKey3);
			}
		}

		// Token: 0x06008180 RID: 33152 RVA: 0x004D50B4 File Offset: 0x004D32B4
		private void FixLoongEventWrongState(DataContext context)
		{
			bool flag = DomainManager.Extra.GetFiveLoongDictCount(context) > 0;
			if (!flag)
			{
				EventArgBox loongDlcArgBox = DomainManager.Extra.GetOrCreateDlcArgBox(2764950UL, context);
				bool fiveLoongDlcBeginMonthlyEventTriggered = false;
				bool flag2 = loongDlcArgBox.Get("ConchShip_PresetKey_FiveLoongDlcBeginMonthlyEventTriggered", ref fiveLoongDlcBeginMonthlyEventTriggered) && fiveLoongDlcBeginMonthlyEventTriggered;
				if (flag2)
				{
					bool flag3 = !DomainManager.Extra.IsExtraTaskInProgress(264) && DomainManager.Extra.GetFiveLoongDictCount(context) == 0;
					if (flag3)
					{
						loongDlcArgBox.Set("ConchShip_PresetKey_FiveLoongDlcBeginMonthlyEventTriggered", false);
						DomainManager.Extra.SetDlcArgBox(2764950UL, loongDlcArgBox, context);
						FiveLoongDlcEntry.Logger.Warn("Fix Loong Event Wrong State");
					}
				}
			}
		}

		// Token: 0x06008181 RID: 33153 RVA: 0x004D515C File Offset: 0x004D335C
		private void InitializeJiaoData(DataContext context)
		{
			List<JiaoPool> jiaoPools = DomainManager.Extra.GetJiaoPoolList();
			List<JiaoPoolRecordList> jiaoPoolRecords = DomainManager.Extra.GetJiaoPoolRecordList();
			while (jiaoPools.Count < 9)
			{
				jiaoPools.Add(new JiaoPool());
				jiaoPoolRecords.Add(new JiaoPoolRecordList());
			}
			this.JiaoEggDropRate = GlobalConfig.Instance.InitJiaoEggDropRate;
			this.MaleJiaoEggDropRate = GlobalConfig.Instance.InitMaleJiaoEggDropRate;
			DomainManager.Extra.SetJiaoPools(jiaoPools, context);
			DomainManager.Extra.SetJiaoPoolRecords(jiaoPoolRecords, context);
		}

		// Token: 0x06008182 RID: 33154 RVA: 0x004D51E4 File Offset: 0x004D33E4
		private void InitializeMaxTaiwuVillageLevel()
		{
			this.UpdateMaxTaiwuVillageLevel();
		}

		// Token: 0x06008183 RID: 33155 RVA: 0x004D51F0 File Offset: 0x004D33F0
		private void InitializeJiaoCacheData()
		{
			foreach (KeyValuePair<int, Jiao> keyValuePair in DomainManager.Extra.Jiaos)
			{
				int num;
				Jiao jiao2;
				keyValuePair.Deconstruct(out num, out jiao2);
				int id = num;
				Jiao jiao = jiao2;
				DomainManager.Extra.SetJiaoEvolutionChoice(id);
				DomainManager.Extra.SetJiaoKeyToId(jiao.Key, id);
			}
			DomainManager.Extra.ResetJiaoPoolStatus(DataContextManager.GetCurrentThreadDataContext());
		}

		// Token: 0x06008184 RID: 33156 RVA: 0x004D5280 File Offset: 0x004D3480
		private void InitializeChildrenOfLoongCacheData()
		{
			foreach (KeyValuePair<int, ChildrenOfLoong> keyValuePair in DomainManager.Extra.ChildrenOfLoong)
			{
				int num;
				ChildrenOfLoong childrenOfLoong;
				keyValuePair.Deconstruct(out num, out childrenOfLoong);
				int id = num;
				ChildrenOfLoong loong = childrenOfLoong;
				DomainManager.Extra.SetChildOfLoongKeyToId(loong.Key, id);
			}
		}

		// Token: 0x06008185 RID: 33157 RVA: 0x004D52F4 File Offset: 0x004D34F4
		private void InitializeConfigCache()
		{
			FiveLoongDlcEntry._carrierToJiaoTemplate = new Dictionary<short, short>();
			foreach (JiaoItem jiaoCfg in ((IEnumerable<JiaoItem>)Jiao.Instance))
			{
				bool flag = jiaoCfg.IndexOfCarrierTemplate >= 0;
				if (flag)
				{
					FiveLoongDlcEntry._carrierToJiaoTemplate.Add(jiaoCfg.IndexOfCarrierTemplate, jiaoCfg.TemplateId);
				}
			}
		}

		// Token: 0x06008186 RID: 33158 RVA: 0x004D5370 File Offset: 0x004D3570
		private unsafe static bool TryCreateOrReAppearFiveLoong(DataContext context, bool isStrict, short characterTemplateId)
		{
			bool condition = FiveLoongDlcEntry.IsCharacterLoong(characterTemplateId);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Wrong FiveLoong characterTemplateId: ");
			defaultInterpolatedStringHandler.AppendFormatted<short>(characterTemplateId);
			Tester.Assert(condition, defaultInterpolatedStringHandler.ToStringAndClear());
			List<short> areaIds = new List<short>();
			List<short> stateIds = new List<short>();
			List<MapBlockData> neighborBlocks = new List<MapBlockData>();
			List<GameData.Domains.Character.Animal> animals = new List<GameData.Domains.Character.Animal>();
			areaIds.Clear();
			stateIds.Clear();
			neighborBlocks.Clear();
			stateIds.AddRange(DomainManager.Extra.GetFiveLoongStateIds());
			for (short areaId = 0; areaId < 135; areaId += 1)
			{
				areaIds.Add(areaId);
			}
			CollectionUtils.Shuffle<short>(context.Random, areaIds);
			short i = 0;
			while ((int)i < areaIds.Count)
			{
				short areaId2 = areaIds[(int)i];
				Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
				bool flag = areaId2 == taiwuVillageLocation.AreaId;
				if (!flag)
				{
					bool flag2 = DomainManager.Map.IsAreaBroken(areaId2);
					if (!flag2)
					{
						sbyte stateId = DomainManager.Map.GetStateIdByAreaId(areaId2);
						bool flag3 = stateIds.Contains((short)stateId);
						if (!flag3)
						{
							Span<MapBlockData> mapBlocks = DomainManager.Map.GetAreaBlocks(areaId2);
							Span<MapBlockData> span = mapBlocks;
							for (int n = 0; n < span.Length; n++)
							{
								MapBlockData centerBlock = *span[n];
								bool flag4 = !FiveLoongDlcEntry.<TryCreateOrReAppearFiveLoong>g__IsBlockMeet|55_0(centerBlock, isStrict);
								if (!flag4)
								{
									neighborBlocks.Clear();
									if (isStrict)
									{
										DomainManager.Map.GetNeighborBlocks(areaId2, centerBlock.BlockId, neighborBlocks, 4);
										bool hasDevelopedNeighbor = false;
										foreach (MapBlockData neighborBlock in neighborBlocks)
										{
											bool flag5 = !neighborBlock.IsNonDeveloped();
											if (flag5)
											{
												hasDevelopedNeighbor = true;
												break;
											}
										}
										bool flag6 = !hasDevelopedNeighbor;
										if (flag6)
										{
											goto IL_789;
										}
									}
									neighborBlocks.Clear();
									DomainManager.Map.GetNeighborBlocks(areaId2, centerBlock.BlockId, neighborBlocks, 3);
									bool isCenterBlockMeet = true;
									bool flag7 = neighborBlocks.Count < 24;
									if (!flag7)
									{
										foreach (MapBlockData neighborBlock2 in neighborBlocks)
										{
											bool flag8 = !FiveLoongDlcEntry.<TryCreateOrReAppearFiveLoong>g__IsBlockMeet|55_0(neighborBlock2, false);
											if (flag8)
											{
												isCenterBlockMeet = false;
												break;
											}
										}
										bool flag9 = isCenterBlockMeet;
										if (flag9)
										{
											Location centerLocation = centerBlock.GetLocation();
											stateIds.Add((short)stateId);
											Dictionary<short, short> blockConfigTemplateIds = new Dictionary<short, short>();
											foreach (MapBlockData neighborBlock3 in neighborBlocks)
											{
												blockConfigTemplateIds.Add(neighborBlock3.BlockId, neighborBlock3.TemplateId);
											}
											blockConfigTemplateIds.Add(centerBlock.BlockId, centerBlock.TemplateId);
											short loongTemplateId = LoongInfo.CharacterTemplateIdToLoongTemplateId(characterTemplateId);
											LoongItem loongCfg = Loong.Instance[loongTemplateId];
											LoongInfo loongInfo;
											bool flag10 = DomainManager.Extra.TryGetElement_FiveLoongDict(characterTemplateId, out loongInfo);
											if (flag10)
											{
												bool flag11 = !loongInfo.IsDisappear;
												if (flag11)
												{
													return false;
												}
												FiveLoongDlcEntry.RemoveMinionLoongsInArea(context, loongInfo.LoongTerrainCenterLocation.AreaId);
												MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
												monthlyNotifications.AddFiveLoongArise(centerLocation, characterTemplateId);
												loongInfo.LoongTerrainCenterLocation = centerLocation;
												loongInfo.LoongCurrentLocation = centerLocation;
												loongInfo.CoveredMapBlockTemplateId = blockConfigTemplateIds;
												loongInfo.IsDisappear = false;
												loongInfo.MapBlockExtraItems.Clear();
												DomainManager.Extra.SetLoongInfo(context, characterTemplateId, loongInfo);
											}
											else
											{
												DomainManager.Extra.SetLoongInfo(context, characterTemplateId, loongInfo = new LoongInfo(characterTemplateId, centerLocation, blockConfigTemplateIds));
											}
											FiveLoongDlcEntry.MapBlockToLoong(context, centerBlock, centerBlock, loongCfg.MapBlock);
											DomainManager.Extra.CreateAnimalByCharacterTemplateId(context, characterTemplateId, centerLocation);
											DomainManager.Extra.TriggerExtraTask(context, 46, 265);
											DomainManager.Extra.TriggerExtraTask(context, 46, (int)loongCfg.Task);
											int minionLoongCount = 0;
											int[] putLoongScaleBlockIndexes = new int[18];
											int[] putLoongEggBlockIndexes = new int[3];
											for (int j = 0; j < putLoongScaleBlockIndexes.Length; j++)
											{
												putLoongScaleBlockIndexes[j] = context.Random.Next(0, neighborBlocks.Count);
											}
											for (int k = 0; k < putLoongEggBlockIndexes.Length; k++)
											{
												putLoongEggBlockIndexes[k] = context.Random.Next(0, neighborBlocks.Count);
											}
											animals.Clear();
											CollectionUtils.Shuffle<MapBlockData>(context.Random, neighborBlocks);
											int neighborBlockIndex = 0;
											while (neighborBlockIndex < neighborBlocks.Count)
											{
												MapBlockData neighborBlock4 = neighborBlocks[neighborBlockIndex];
												Location neighborLocation = neighborBlock4.GetLocation();
												FiveLoongDlcEntry.MapBlockToLoong(context, neighborBlock4, centerBlock, loongCfg.MapBlock);
												DomainManager.Map.ClearBlockRandomEnemies(context, neighborBlock4);
												DomainManager.Map.SetBlockData(context, neighborBlock4);
												bool changedExtraItems = false;
												for (int l = 0; l < putLoongScaleBlockIndexes.Length; l++)
												{
													bool flag12 = putLoongScaleBlockIndexes[l] == neighborBlockIndex;
													if (flag12)
													{
														LoongInfo loongInfo2 = loongInfo;
														if (loongInfo2.MapBlockExtraItems == null)
														{
															loongInfo2.MapBlockExtraItems = new Dictionary<Location, Inventory>();
														}
														Inventory inventory;
														bool flag13 = !loongInfo.MapBlockExtraItems.TryGetValue(neighborLocation, out inventory);
														if (flag13)
														{
															loongInfo.MapBlockExtraItems.Add(neighborLocation, inventory = new Inventory());
														}
														inventory.OfflineAdd(new ItemKey(12, 0, 286, 0), 1);
														changedExtraItems = true;
													}
												}
												for (int m = 0; m < putLoongEggBlockIndexes.Length; m++)
												{
													bool flag14 = putLoongEggBlockIndexes[m] == neighborBlockIndex;
													if (flag14)
													{
														LoongInfo loongInfo2 = loongInfo;
														if (loongInfo2.MapBlockExtraItems == null)
														{
															loongInfo2.MapBlockExtraItems = new Dictionary<Location, Inventory>();
														}
														Inventory inventory2;
														bool flag15 = !loongInfo.MapBlockExtraItems.TryGetValue(neighborLocation, out inventory2);
														if (flag15)
														{
															loongInfo.MapBlockExtraItems.Add(neighborLocation, inventory2 = new Inventory());
														}
														inventory2.OfflineAdd(new ItemKey(5, 0, 280, 0), 1);
														changedExtraItems = true;
													}
												}
												bool flag16 = changedExtraItems;
												if (flag16)
												{
													DomainManager.Extra.SetLoongInfo(context, characterTemplateId, loongInfo);
												}
												List<int> animalIds;
												bool flag17 = DomainManager.Extra.TryGetAnimalIdsByLocation(new Location(areaId2, neighborLocation.BlockId), out animalIds);
												if (flag17)
												{
													foreach (int animalId in animalIds)
													{
														GameData.Domains.Character.Animal animal;
														bool flag18 = DomainManager.Extra.TryGetAnimal(animalId, out animal) && !FiveLoongDlcEntry.IsCharacterMinionLoong(animal.CharacterTemplateId) && !FiveLoongDlcEntry.IsCharacterLoong(animal.CharacterTemplateId);
														if (flag18)
														{
															animals.Add(animal);
														}
													}
												}
												bool flag19 = minionLoongCount < GlobalConfig.Instance.FiveLoongDlcMinionLoongMaxCount;
												if (flag19)
												{
													MapBlockData blockData = DomainManager.Map.GetBlockData(neighborBlock4.AreaId, neighborBlock4.BlockId);
													MapBlockItem config = MapBlock.Instance[blockData.TemplateId];
													bool flag20 = config.SubType != EMapBlockSubType.DLCLoong;
													if (!flag20)
													{
														minionLoongCount++;
														DomainManager.Extra.CreateAnimalByCharacterTemplateId(context, loongCfg.MinionCharTemplateId, neighborLocation);
													}
												}
												IL_6F6:
												neighborBlockIndex++;
												continue;
												goto IL_6F6;
											}
											foreach (GameData.Domains.Character.Animal animal2 in animals)
											{
												bool flag21 = DomainManager.Extra.IsAnimalAbleToAttack(animal2, false);
												if (flag21)
												{
													DomainManager.Extra.RemoveAnimal(context, animal2);
												}
												else
												{
													Location targetLocation;
													bool flag22 = FiveLoongDlcEntry.TryGetValidAnimalMoveLocation(context, areaId2, neighborBlocks, out targetLocation);
													if (flag22)
													{
														DomainManager.Extra.SetAnimalLocation(context, animal2, targetLocation);
													}
												}
											}
											return true;
										}
									}
								}
								IL_789:;
							}
						}
					}
				}
				i += 1;
			}
			return false;
		}

		// Token: 0x06008187 RID: 33159 RVA: 0x004D5B7C File Offset: 0x004D3D7C
		public static void MapBlockToLoong(DataContext context, MapBlockData block, MapBlockData center, short templateId)
		{
			block = block.GetRootBlock();
			List<MapBlockData> targetBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			bool flag = block.GetConfig().Size <= 1;
			if (flag)
			{
				targetBlocks.Add(block);
			}
			else
			{
				targetBlocks.AddRange(block.GroupBlockList);
				DomainManager.Map.SplitMultiBlock(context, block);
			}
			ByteCoordinate centerPos = center.GetBlockPos();
			foreach (MapBlockData targetBlock in targetBlocks)
			{
				Tester.Assert(targetBlock.GetConfig().Size == 1, "targetBlock.GetConfig().Size == 1");
				bool flag2 = targetBlock.GetManhattanDistanceToPos(centerPos.X, centerPos.Y) > 3;
				if (!flag2)
				{
					DomainManager.Map.ChangeBlockTemplate(context, targetBlock, templateId);
					DomainManager.Map.DestroyMapBlockItemsDirect(context, targetBlock);
					DomainManager.Map.SetBlockAndViewRangeVisible(context, targetBlock.AreaId, targetBlock.BlockId);
				}
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(targetBlocks);
		}

		// Token: 0x06008188 RID: 33160 RVA: 0x004D5C9C File Offset: 0x004D3E9C
		public unsafe static bool TryGetValidAnimalMoveLocation(DataContext context, short areaId, List<MapBlockData> invalidBlocks, out Location location)
		{
			location = Location.Invalid;
			List<MapBlockData> blocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
			for (int i = 0; i < areaBlocks.Length; i++)
			{
				MapBlockData block = *areaBlocks[i];
				bool flag = block.Visible && block.IsNonDeveloped() && block.IsPassable() && (invalidBlocks == null || !invalidBlocks.Contains(block));
				if (flag)
				{
					blocks.Add(block);
				}
			}
			bool flag2 = blocks.Count == 0;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				short blockId = blocks.GetRandom(context.Random).BlockId;
				ObjectPool<List<MapBlockData>>.Instance.Return(blocks);
				location = new Location(areaId, blockId);
				result = true;
			}
			return result;
		}

		// Token: 0x06008189 RID: 33161 RVA: 0x004D5D70 File Offset: 0x004D3F70
		public static void CreateAllFiveLoong(DataContext context)
		{
			bool flag = DomainManager.Extra.FiveLoongDict.Count >= Loong.Instance.Count;
			if (!flag)
			{
				foreach (LoongItem loongCfg in ((IEnumerable<LoongItem>)Loong.Instance))
				{
					bool flag2 = !FiveLoongDlcEntry.TryCreateOrReAppearFiveLoong(context, true, loongCfg.CharTemplateId);
					if (flag2)
					{
						FiveLoongDlcEntry.TryCreateOrReAppearFiveLoong(context, false, loongCfg.CharTemplateId);
					}
				}
			}
		}

		// Token: 0x0600818A RID: 33162 RVA: 0x004D5E00 File Offset: 0x004D4000
		public static int DefeatFiveLoong(DataContext context, short characterTemplateId)
		{
			bool condition = characterTemplateId >= 686 && characterTemplateId <= 690;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
			defaultInterpolatedStringHandler.AppendLiteral("FiveLoong Wrong characterTemplateId: ");
			defaultInterpolatedStringHandler.AppendFormatted<short>(characterTemplateId);
			Tester.Assert(condition, defaultInterpolatedStringHandler.ToStringAndClear());
			LoongInfo loongInfo;
			bool flag = DomainManager.Extra.TryGetElement_FiveLoongDict(characterTemplateId, out loongInfo);
			int result;
			if (flag)
			{
				Location centerLocation = loongInfo.LoongTerrainCenterLocation;
				bool flag2 = loongInfo.CoveredMapBlockTemplateId != null;
				if (flag2)
				{
					foreach (KeyValuePair<short, short> keyValuePair in loongInfo.CoveredMapBlockTemplateId)
					{
						short num;
						short num2;
						keyValuePair.Deconstruct(out num, out num2);
						short blockId = num;
						short templateId = num2;
						Location blockLocation = new Location(centerLocation.AreaId, blockId);
						DomainManager.Map.ChangeBlockTemplate(context, blockLocation, templateId, true);
						DomainManager.Map.SetBlockAndViewRangeVisible(context, blockLocation.AreaId, blockLocation.BlockId);
					}
					loongInfo.CoveredMapBlockTemplateId.Clear();
				}
				DomainManager.Extra.RemoveAnimalByLocationAndTemplateId(context, loongInfo.LoongCurrentLocation, characterTemplateId);
				short taskInfoTemplateId = characterTemplateId - 686 + 268;
				DomainManager.Extra.FinishTriggeredExtraTask(context, 46, (int)taskInfoTemplateId);
				ushort count;
				bool needInstantNotification = loongInfo.CharacterDebuffCounts != null && loongInfo.CharacterDebuffCounts.TryGetValue(DomainManager.Taiwu.GetTaiwuCharId(), out count) && count > 0;
				bool flag3 = loongInfo.CharacterDebuffCounts != null;
				if (flag3)
				{
					loongInfo.CharacterDebuffCounts.Clear();
				}
				bool flag4 = needInstantNotification;
				if (flag4)
				{
					FiveLoongDlcEntry.AddLoongDebuffInstantNotification(loongInfo.ConfigData.DebuffCountDecNotification, 0);
				}
				loongInfo.IsDisappear = true;
				loongInfo.DisappearDate = DomainManager.World.GetCurrDate();
				DomainManager.Extra.SetLoongInfo(context, characterTemplateId, loongInfo);
				result = FiveLoongDlcEntry.RemoveMinionLoongsInArea(context, loongInfo.LoongCurrentLocation.AreaId);
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x0600818B RID: 33163 RVA: 0x004D5FF4 File Offset: 0x004D41F4
		private static int RemoveMinionLoongsInArea(DataContext context, short areaId)
		{
			Dictionary<short, List<int>> animalAreaData;
			bool flag = DomainManager.Extra.TryGetAnimalAreaDataByAreaId(areaId, out animalAreaData);
			int result;
			if (flag)
			{
				List<GameData.Domains.Character.Animal> animals = new List<GameData.Domains.Character.Animal>();
				foreach (List<int> animalIds in animalAreaData.Values)
				{
					foreach (int animalId in animalIds)
					{
						GameData.Domains.Character.Animal animal;
						bool flag2 = DomainManager.Extra.TryGetAnimal(animalId, out animal);
						if (flag2)
						{
							bool flag3 = FiveLoongDlcEntry.IsCharacterMinionLoong(animal.CharacterTemplateId);
							if (flag3)
							{
								animals.Add(animal);
							}
						}
					}
				}
				foreach (GameData.Domains.Character.Animal animal2 in animals)
				{
					DomainManager.Extra.RemoveAnimal(context, animal2);
				}
				result = animals.Count;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x0600818C RID: 33164 RVA: 0x004D6124 File Offset: 0x004D4324
		void IDlcEntry.OnLoadedArchiveData()
		{
			this.InitializeConfigCache();
			this.InitializeJiaoCacheData();
			this.InitializeChildrenOfLoongCacheData();
			Events.RegisterHandler_TaiwuMove(new Events.OnTaiwuMove(FiveLoongDlcEntry.OnTaiwuMove));
			Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(FiveLoongDlcEntry.OnDefeatFiveLoongInCombat));
		}

		// Token: 0x0600818D RID: 33165 RVA: 0x004D6160 File Offset: 0x004D4360
		void IDlcEntry.OnEnterNewWorld()
		{
			this.InitializeConfigCache();
			this.InitializeJiaoData(DataContextManager.GetCurrentThreadDataContext());
			this.InitializeJiaoCacheData();
			this.InitializeChildrenOfLoongCacheData();
			Events.RegisterHandler_TaiwuMove(new Events.OnTaiwuMove(FiveLoongDlcEntry.OnTaiwuMove));
			Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(FiveLoongDlcEntry.OnDefeatFiveLoongInCombat));
		}

		// Token: 0x0600818E RID: 33166 RVA: 0x004D61B3 File Offset: 0x004D43B3
		void IDlcEntry.OnPostAdvanceMonth(DataContext context)
		{
			this.PostAdvanceMonth_Main(context);
			this.PostAdvanceMonth_ChildOfLoong(context);
			this.PostAdvanceMonth_JiaoPool(context);
			this.PostAdvanceMonth_JiaoPoolLog(context);
			this.PostAdvanceMonth_FiveLoongs(context);
		}

		// Token: 0x0600818F RID: 33167 RVA: 0x004D61E0 File Offset: 0x004D43E0
		void IDlcEntry.FixAbnormalArchiveData(DataContext context)
		{
			bool flag = DomainManager.Extra.GetJiaoPools().Count < 9;
			if (flag)
			{
				this.InitializeJiaoData(context);
			}
			bool flag2 = this.MaxTaiwuVillageLevel <= 0;
			if (flag2)
			{
				this.InitializeMaxTaiwuVillageLevel();
			}
			bool flag3 = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 69, 39);
			if (flag3)
			{
				this.FixAbnormalChildrenOfLoongData(context);
			}
			bool flag4 = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 69, 45);
			if (flag4)
			{
				this.FixAbnormalJiaoLoongData(context);
			}
			bool flag5 = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 70, 47);
			if (flag5)
			{
				bool flag6 = DomainManager.World.IsCurrWorldSavedWithVersion(0, 0, 70, 46);
				if (flag6)
				{
					this.FixInvalidJiaoItems(context);
				}
				this.FixJiaoKeyInUnhandledCrossArchiveGameData(context);
				this.FixAbnormalJiaoEggAndTeenagerData(context);
			}
			bool flag7 = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 70, 34);
			if (flag7)
			{
				this.FixAbnormalJiaoGenerationData(context);
			}
			bool flag8 = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 70, 54);
			if (flag8)
			{
				this.FixAbnormalJiaoNextPeriodData(context);
			}
			bool flag9 = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 79, 0);
			if (flag9)
			{
				this.FixAbnormalJiaoPriceData(context);
			}
			this.FixLoongEventWrongState(context);
		}

		// Token: 0x06008190 RID: 33168 RVA: 0x004D6304 File Offset: 0x004D4504
		public void OnCrossArchive(DataContext context, IDlcEntry entryBeforeCrossArchive)
		{
			FiveLoongDlcEntry entry = (FiveLoongDlcEntry)entryBeforeCrossArchive;
			this.MaleJiaoEggDropRate = entry.MaleJiaoEggDropRate;
			this.OwnedChildrenOfLoong = entry.OwnedChildrenOfLoong;
		}

		// Token: 0x06008191 RID: 33169 RVA: 0x004D6334 File Offset: 0x004D4534
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06008192 RID: 33170 RVA: 0x004D6348 File Offset: 0x004D4548
		public int GetSerializedSize()
		{
			int totalSize = 27;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06008193 RID: 33171 RVA: 0x004D6370 File Offset: 0x004D4570
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 7;
			byte* pCurrData = pData + 2;
			*(int*)pCurrData = this.ChildrenOfLoongMonthlyEventChance;
			pCurrData += 4;
			*(int*)pCurrData = this.PulaoCricketLuckPoint;
			pCurrData += 4;
			*(int*)pCurrData = this.JiaoEggDropRate;
			pCurrData += 4;
			*(int*)pCurrData = this.MaleJiaoEggDropRate;
			pCurrData += 4;
			*pCurrData = (this.IsJiaoPoolOpen ? 1 : 0);
			pCurrData++;
			*(int*)pCurrData = this.OwnedChildrenOfLoong;
			pCurrData += 4;
			*(int*)pCurrData = this.MaxTaiwuVillageLevel;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06008194 RID: 33172 RVA: 0x004D63F8 File Offset: 0x004D45F8
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				this.ChildrenOfLoongMonthlyEventChance = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag2 = fieldCount > 1;
			if (flag2)
			{
				this.PulaoCricketLuckPoint = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag3 = fieldCount > 2;
			if (flag3)
			{
				this.JiaoEggDropRate = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag4 = fieldCount > 3;
			if (flag4)
			{
				this.MaleJiaoEggDropRate = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag5 = fieldCount > 4;
			if (flag5)
			{
				this.IsJiaoPoolOpen = (*pCurrData != 0);
				pCurrData++;
			}
			bool flag6 = fieldCount > 5;
			if (flag6)
			{
				this.OwnedChildrenOfLoong = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag7 = fieldCount > 6;
			if (flag7)
			{
				this.MaxTaiwuVillageLevel = *(int*)pCurrData;
				pCurrData += 4;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06008197 RID: 33175 RVA: 0x004D64EC File Offset: 0x004D46EC
		[CompilerGenerated]
		internal static bool <PostAdvanceMonth_Main>g__IsMainStoryLineProgressMeetLoongDlc|10_0()
		{
			return DomainManager.World.GetMainStoryLineProgress() >= 21 && DomainManager.World.GetMainStoryLineProgress() < 27;
		}

		// Token: 0x06008198 RID: 33176 RVA: 0x004D6520 File Offset: 0x004D4720
		[CompilerGenerated]
		internal static bool <PostAdvanceMonth_Main>g__IsTaiwuVillageLevelMeetFiveLoongDlc|10_1()
		{
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData taiwuBuildingAreaData = DomainManager.Building.GetBuildingAreaData(taiwuVillageLocation);
			BuildingBlockKey buildingKey = BuildingDomain.FindBuildingKey(taiwuVillageLocation, taiwuBuildingAreaData, 44, true);
			BuildingBlockDataEx ex;
			return !buildingKey.IsInvalid && DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)buildingKey, out ex) && ex.CalcUnlockedLevelCount() >= 6;
		}

		// Token: 0x06008199 RID: 33177 RVA: 0x004D6590 File Offset: 0x004D4790
		[CompilerGenerated]
		internal static bool <TryCreateOrReAppearFiveLoong>g__IsBlockMeet|55_0(MapBlockData mapBlockData, bool avoidBigBlock = false)
		{
			MapBlockItem config = mapBlockData.GetConfig();
			bool flag = !mapBlockData.IsNonDeveloped() || mapBlockData.Destroyed || config.TemplateId == 126;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = avoidBigBlock && config.Size > 1;
				result = !flag2;
			}
			return result;
		}

		// Token: 0x040023AE RID: 9134
		public const int MaxJiaoPoolCount = 9;

		// Token: 0x040023AF RID: 9135
		[SerializableGameDataField]
		public int ChildrenOfLoongMonthlyEventChance;

		// Token: 0x040023B0 RID: 9136
		[SerializableGameDataField]
		public int PulaoCricketLuckPoint;

		// Token: 0x040023B1 RID: 9137
		[SerializableGameDataField]
		public int JiaoEggDropRate;

		// Token: 0x040023B2 RID: 9138
		[SerializableGameDataField]
		public int MaleJiaoEggDropRate;

		// Token: 0x040023B3 RID: 9139
		[SerializableGameDataField]
		public bool IsJiaoPoolOpen;

		// Token: 0x040023B4 RID: 9140
		[SerializableGameDataField]
		public int OwnedChildrenOfLoong;

		// Token: 0x040023B5 RID: 9141
		[SerializableGameDataField]
		public int MaxTaiwuVillageLevel;

		// Token: 0x040023B6 RID: 9142
		private const int DisablePoolTamePointLoss = 5;

		// Token: 0x040023B7 RID: 9143
		private const int JiaoTamingPointsLowLimit = 50;

		// Token: 0x040023B8 RID: 9144
		private static Dictionary<short, short> _carrierToJiaoTemplate;

		// Token: 0x040023B9 RID: 9145
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x02000D2E RID: 3374
		private static class FieldIds
		{
			// Token: 0x04003736 RID: 14134
			public const ushort ChildrenOfLoongMonthlyEventChance = 0;

			// Token: 0x04003737 RID: 14135
			public const ushort PulaoCricketLuckPoint = 1;

			// Token: 0x04003738 RID: 14136
			public const ushort JiaoEggDropRate = 2;

			// Token: 0x04003739 RID: 14137
			public const ushort MaleJiaoEggDropRate = 3;

			// Token: 0x0400373A RID: 14138
			public const ushort IsJiaoPoolOpen = 4;

			// Token: 0x0400373B RID: 14139
			public const ushort OwnedChildrenOfLoong = 5;

			// Token: 0x0400373C RID: 14140
			public const ushort MaxTaiwuVillageLevel = 6;

			// Token: 0x0400373D RID: 14141
			public const ushort Count = 7;

			// Token: 0x0400373E RID: 14142
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"ChildrenOfLoongMonthlyEventChance",
				"PulaoCricketLuckPoint",
				"JiaoEggDropRate",
				"MaleJiaoEggDropRate",
				"IsJiaoPoolOpen",
				"OwnedChildrenOfLoong",
				"MaxTaiwuVillageLevel"
			};
		}
	}
}
