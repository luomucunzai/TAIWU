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

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions
{
	// Token: 0x0200008D RID: 141
	[SerializableGameData(NotForDisplayModule = true)]
	public class EnemyNestMonthlyAction : MonthlyActionBase, ISerializableGameData, IMonthlyActionGroup
	{
		// Token: 0x06001924 RID: 6436 RVA: 0x0016A7A6 File Offset: 0x001689A6
		public EnemyNestMonthlyAction(List<short> enemyNestActionTempIds)
		{
			this.Key = new MonthlyActionKey(1, 0);
			this._enemyNestActionsInAreas = new List<ConfigMonthlyAction>[45];
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x0016A7CA File Offset: 0x001689CA
		public EnemyNestMonthlyAction(MonthlyActionKey key)
		{
			this.Key = key;
			this._enemyNestActionsInAreas = new List<ConfigMonthlyAction>[45];
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x0016A7E8 File Offset: 0x001689E8
		public void InitializeEnemyNests(IRandomSource random)
		{
			for (short areaId = 0; areaId < 45; areaId += 1)
			{
				this._enemyNestActionsInAreas[(int)areaId] = new List<ConfigMonthlyAction>();
			}
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0016A818 File Offset: 0x00168A18
		public override void MonthlyHandler()
		{
			bool flag = !DomainManager.World.GetWorldFunctionsStatus(7);
			if (!flag)
			{
				DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
				MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				int currDate = DomainManager.World.GetCurrDate();
				DomainManager.Extra.UpdateEnemyNestInitializationDates(context, currDate);
				for (short areaId = 0; areaId < 45; areaId += 1)
				{
					this.UpdateEnemiesInArea(context, areaId);
					List<ConfigMonthlyAction> nestsInArea = this._enemyNestActionsInAreas[(int)areaId];
					MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)areaId);
					MapAreaItem areaCfg = areaData.GetConfig();
					for (int index = nestsInArea.Count - 1; index >= 0; index--)
					{
						ConfigMonthlyAction configAction = nestsInArea[index];
						bool flag2 = configAction == null;
						if (!flag2)
						{
							int interval = EnemyNestMonthlyAction.GetIntervalByAdventureId(areaCfg, index, configAction.ConfigData.AdventureId);
							bool flag3 = configAction.State == 0 && configAction.LastFinishDate + interval <= currDate;
							if (flag3)
							{
								configAction.SelectLocation();
								configAction.TriggerAction();
							}
							else
							{
								bool flag4 = configAction.State == 5 && DomainManager.Adventure.GetAdventureSiteState(areaId, configAction.Location.BlockId) == 1;
								if (flag4)
								{
									List<CharacterSet> majorCharacterSets = configAction.MajorCharacterSets;
									bool flag5 = majorCharacterSets != null && majorCharacterSets.Count > 0;
									if (flag5)
									{
										bool noLongerValid = false;
										short adventureId = configAction.ConfigData.AdventureId;
										foreach (CharacterSet charSet in configAction.MajorCharacterSets)
										{
											HashSet<int> collection = charSet.GetCollection();
											foreach (int charId in collection)
											{
												GameData.Domains.Character.Character character;
												bool flag6 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
												if (flag6)
												{
													noLongerValid = true;
												}
												else
												{
													character.ChangeHealth(context, (int)GlobalConfig.Instance.EnemyNestKidnappedCharHealthChange);
													bool flag7 = character.GetHealth() <= 0;
													if (flag7)
													{
														noLongerValid = true;
														DomainManager.Character.MakeCharacterDead(context, character, 5, new CharacterDeathInfo(character.GetValidLocation())
														{
															AdventureId = adventureId
														});
													}
													else
													{
														bool flag8 = EnemyNestMonthlyAction.CheckEscape(context.Random, character, adventureId);
														if (flag8)
														{
															noLongerValid = true;
															monthlyNotifications.AddEscapeFromEnemyNest(charId, configAction.Location, adventureId);
															lifeRecordCollection.AddEscapeFromEnemyNestBySelf(charId, currDate, configAction.Location, adventureId);
															character.ActivateAdvanceMonthStatus(16);
														}
													}
												}
											}
										}
										bool flag9 = noLongerValid;
										if (flag9)
										{
											configAction.EnsurePrerequisites();
											bool flag10 = configAction.GetCharacterArg() >= 0;
											if (flag10)
											{
												monthlyNotifications.AddConfigMonthlyActionNotification(configAction.ConfigData, configAction);
											}
										}
									}
								}
							}
							configAction.MonthlyHandler();
						}
					}
				}
			}
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x0016AB6C File Offset: 0x00168D6C
		private static int GetIntervalByAdventureId(MapAreaItem mapAreaCfg, int index, short adventureId)
		{
			bool flag = mapAreaCfg.EnemyNests.Count <= index;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				EnemyNestCreationInfo[] creationInfoGroup = mapAreaCfg.EnemyNests[index];
				foreach (EnemyNestCreationInfo creationInfo in creationInfoGroup)
				{
					bool flag2 = EnemyNest.Instance[creationInfo.EnemyNest].AdventureId == adventureId;
					if (flag2)
					{
						return creationInfo.Interval;
					}
				}
				result = 0;
			}
			return result;
		}

		// Token: 0x06001929 RID: 6441 RVA: 0x0016ABEC File Offset: 0x00168DEC
		private static bool CheckEscape(IRandomSource randomSource, GameData.Domains.Character.Character character, short adventureId)
		{
			sbyte enemyNestTemplateId = AdventureDomain.GetEnemyNestTemplateId(adventureId);
			EnemyNestItem enemyNestCfg = EnemyNest.Instance[(short)enemyNestTemplateId];
			CharacterItem nestLeaderCfg = Config.Character.Instance[enemyNestCfg.Leader];
			GameData.Domains.Character.Character nestLeader = DomainManager.Character.GetPregeneratedRandomEnemy(nestLeaderCfg.RandomEnemyId);
			int characterCombatPower = character.GetCombatPower();
			int nestLeaderCombatPower = nestLeader.GetCombatPower();
			int rate = characterCombatPower * 100 / nestLeaderCombatPower - 100;
			return randomSource.CheckPercentProb(rate);
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x0016AC5C File Offset: 0x00168E5C
		public override void ValidationHandler()
		{
			bool flag = !DomainManager.World.GetWorldFunctionsStatus(7);
			if (!flag)
			{
				for (short areaId = 0; areaId < 45; areaId += 1)
				{
					for (int index = this._enemyNestActionsInAreas[(int)areaId].Count - 1; index >= 0; index--)
					{
						ConfigMonthlyAction configAction = this._enemyNestActionsInAreas[(int)areaId][index];
						configAction.ValidationHandler();
					}
				}
			}
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x0016ACD0 File Offset: 0x00168ED0
		public override void FillEventArgBox(EventArgBox eventArgBox)
		{
			Location location;
			eventArgBox.Get<Location>("AdventureLocation", out location);
			bool flag = !location.IsValid();
			if (!flag)
			{
				ConfigMonthlyAction configAction = this.GetConfigAction(location.AreaId, location.BlockId);
				configAction.EnsurePrerequisites();
				configAction.FillEventArgBox(eventArgBox);
			}
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x0016AD20 File Offset: 0x00168F20
		public override void CollectCalledCharacters(HashSet<int> calledCharacters)
		{
			foreach (List<ConfigMonthlyAction> areaActions in this._enemyNestActionsInAreas)
			{
				foreach (ConfigMonthlyAction action in areaActions)
				{
					if (action != null)
					{
						action.CollectCalledCharacters(calledCharacters);
					}
				}
			}
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x0016AD94 File Offset: 0x00168F94
		private void UpdateEnemiesInArea(DataContext context, short areaId)
		{
			bool flag = this._enemyNestActionsInAreas.Length <= (int)areaId;
			if (!flag)
			{
				List<ConfigMonthlyAction> nestsInArea = this._enemyNestActionsInAreas[(int)areaId];
				MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)areaId);
				MapAreaItem areaCfg = areaData.GetConfig();
				bool flag2 = areaCfg.EnemyNests == null;
				if (!flag2)
				{
					IRandomSource random = context.Random;
					int currDate = DomainManager.World.GetCurrDate();
					int i = 0;
					int count = areaCfg.EnemyNests.Count;
					while (i < count)
					{
						bool flag3 = nestsInArea.Count > i;
						if (!flag3)
						{
							nestsInArea.Add(null);
							goto IL_FE;
						}
						ConfigMonthlyAction action = nestsInArea[i];
						bool flag4 = action != null;
						if (!flag4)
						{
							goto IL_FE;
						}
						AdventureSiteData siteData = DomainManager.Adventure.GetAdventureSite(action.Location.AreaId, action.Location.BlockId);
						bool flag5 = siteData != null && siteData.SiteState == 1;
						if (flag5)
						{
							DomainManager.Adventure.ComplementEnemiesBySite(context, siteData, areaId, action.Location.BlockId, false);
						}
						IL_171:
						i++;
						continue;
						IL_FE:
						EnemyNestCreationInfo creationInfo = areaCfg.EnemyNests[i].GetRandom(random);
						EnemyNestItem nestCfg = EnemyNest.Instance[creationInfo.EnemyNest];
						int beginDate = Math.Max(DomainManager.Extra.GetEnemyNestInitializationDate(areaId, i), currDate);
						nestsInArea[i] = new ConfigMonthlyAction(this.Key, nestCfg.MonthlyActionId, areaId)
						{
							LastFinishDate = beginDate - EnemyNestMonthlyAction.GetIntervalByAdventureId(areaCfg, i, nestCfg.AdventureId)
						};
						goto IL_171;
					}
				}
			}
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x0016AF28 File Offset: 0x00169128
		public ConfigMonthlyAction GetConfigAction(short areaId, short blockId)
		{
			foreach (ConfigMonthlyAction configAction in this._enemyNestActionsInAreas[(int)areaId])
			{
				bool flag = configAction != null && configAction.Location.BlockId == blockId;
				if (flag)
				{
					return configAction;
				}
			}
			return null;
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x0016AF9C File Offset: 0x0016919C
		public void DeactivateSubAction(short areaId, short blockId, bool isComplete)
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)areaId);
			MapAreaItem areaConfig = areaData.GetConfig();
			List<ConfigMonthlyAction> nestsInArea = this._enemyNestActionsInAreas[(int)areaId];
			int index = 0;
			int count = nestsInArea.Count;
			while (index < count)
			{
				ConfigMonthlyAction configAction = nestsInArea[index];
				bool flag = configAction == null || configAction.Location.BlockId != blockId;
				if (!flag)
				{
					VillagerWorkData workData = DomainManager.Taiwu.GetVillagerMapWorkData(areaId, blockId, 11);
					bool flag2 = workData != null;
					if (flag2)
					{
						DomainManager.Taiwu.RemoveVillagerWork(context, workData.CharacterId, true);
						Location location = configAction.Location;
						short adventureId = configAction.ConfigData.AdventureId;
						InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotificationCollection();
						instantNotifications.AddTheNestOfRegulationDies(location, adventureId, workData.CharacterId);
					}
					configAction.Deactivate(isComplete);
					bool flag3 = areaConfig.EnemyNests.CheckIndex(index);
					if (flag3)
					{
						EnemyNestCreationInfo creationInfo = areaConfig.EnemyNests[index].GetRandom(context.Random);
						EnemyNestItem nestCfg = EnemyNest.Instance[creationInfo.EnemyNest];
						nestsInArea[index] = new ConfigMonthlyAction(this.Key, nestCfg.MonthlyActionId, areaId)
						{
							LastFinishDate = DomainManager.World.GetCurrDate()
						};
					}
					else
					{
						nestsInArea.RemoveAt(index);
					}
					break;
				}
				index++;
			}
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x0016B110 File Offset: 0x00169310
		internal void ResetIntervals()
		{
			foreach (List<ConfigMonthlyAction> areaActions in this._enemyNestActionsInAreas)
			{
				foreach (ConfigMonthlyAction action in areaActions)
				{
					bool flag = action != null && action.State == 0;
					if (flag)
					{
						action.LastFinishDate = int.MinValue;
					}
				}
			}
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x0016B198 File Offset: 0x00169398
		public override MonthlyActionBase CreateCopy()
		{
			return Serializer.CreateCopy<EnemyNestMonthlyAction>(this);
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x0016B1B0 File Offset: 0x001693B0
		public void Test_LogEnemyNestsLargeBase(IRandomSource random, int iterations)
		{
			string[] names = (from nest in EnemyNest.Instance
			select Adventure.Instance[nest.AdventureId].Name).ToArray<string>();
			short[] values = EnemyNest.Instance.GetAllKeys().ToArray();
			AccurateHistogram<short> histogram = new AccurateHistogram<short>(names, values);
			for (int i = 0; i < iterations; i++)
			{
				for (short areaId = 0; areaId < 45; areaId += 1)
				{
					MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)areaId);
					MapAreaItem areaCfg = areaData.GetConfig();
					this._enemyNestActionsInAreas[(int)areaId] = new List<ConfigMonthlyAction>();
					bool flag = areaCfg.EnemyNests == null;
					if (!flag)
					{
						foreach (EnemyNestCreationInfo[] enemyNestArr in areaCfg.EnemyNests)
						{
							EnemyNestCreationInfo enemyNestCreationInfo = enemyNestArr.GetRandom(random);
							histogram.Record(enemyNestCreationInfo.EnemyNest);
						}
					}
				}
			}
			AdaptableLog.Info("\n" + histogram.GetTextGraph(100));
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x0016B2EC File Offset: 0x001694EC
		public void Test_LogEnemyNests()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Dictionary<short, int> adventureCounts = new Dictionary<short, int>();
			for (short areaId = 0; areaId < 45; areaId += 1)
			{
				MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)areaId);
				MapAreaItem areaCfg = areaData.GetConfig();
				List<ConfigMonthlyAction> enemyNests = this._enemyNestActionsInAreas[(int)areaId];
				bool flag = enemyNests == null;
				if (!flag)
				{
					stringBuilder.Append(areaCfg.Name);
					stringBuilder.Append(',');
					foreach (ConfigMonthlyAction action in enemyNests)
					{
						short adventureId = action.GetAdventureArg();
						AdventureItem adventureCfg = Adventure.Instance[adventureId];
						stringBuilder.Append(adventureCfg.Name);
						stringBuilder.Append(',');
						bool flag2 = !adventureCounts.TryAdd(adventureId, 1);
						if (flag2)
						{
							Dictionary<short, int> dictionary = adventureCounts;
							short key = adventureId;
							int num = dictionary[key];
							dictionary[key] = num + 1;
						}
					}
					stringBuilder.AppendLine();
				}
			}
			AdaptableLog.Info(stringBuilder.ToString());
			stringBuilder.Clear();
			foreach (EnemyNestItem enemyNestCfg in ((IEnumerable<EnemyNestItem>)EnemyNest.Instance))
			{
				AdventureItem adventureCfg2 = Adventure.Instance[enemyNestCfg.AdventureId];
				int count = adventureCounts.GetOrDefault(enemyNestCfg.AdventureId);
				stringBuilder.AppendFormat("{0}: {1}\n", adventureCfg2.Name, count.ToString());
			}
			AdaptableLog.Info(stringBuilder.ToString());
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x0016B4A4 File Offset: 0x001696A4
		public EnemyNestMonthlyAction()
		{
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x0016B4B0 File Offset: 0x001696B0
		public override bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x0016B4C4 File Offset: 0x001696C4
		public override int GetSerializedSize()
		{
			int totalSize = 12;
			bool flag = this._enemyNestActionsInAreas != null;
			if (flag)
			{
				totalSize += 2;
				foreach (List<ConfigMonthlyAction> actionsInArea in this._enemyNestActionsInAreas)
				{
					totalSize += 2;
					foreach (ConfigMonthlyAction action in actionsInArea)
					{
						totalSize += action.GetSerializedSize();
					}
				}
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x0016B574 File Offset: 0x00169774
		public unsafe override int Serialize(byte* pData)
		{
			byte* pCurrData = pData;
			bool flag = this._enemyNestActionsInAreas != null;
			if (flag)
			{
				int elementsCount = this._enemyNestActionsInAreas.Length;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				foreach (List<ConfigMonthlyAction> actionsInArea in this._enemyNestActionsInAreas)
				{
					*(short*)pCurrData = (short)((ushort)actionsInArea.Count);
					pCurrData += 2;
					foreach (ConfigMonthlyAction action in actionsInArea)
					{
						pCurrData += action.Serialize(pCurrData);
					}
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			pCurrData += this.Key.Serialize(pCurrData);
			*pCurrData = (byte)this.State;
			pCurrData++;
			*(int*)pCurrData = this.Month;
			pCurrData += 4;
			*(int*)pCurrData = this.LastFinishDate;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x0016B69C File Offset: 0x0016989C
		public unsafe override int Deserialize(byte* pData)
		{
			ushort elementsCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = this._enemyNestActionsInAreas == null || this._enemyNestActionsInAreas.Length != (int)elementsCount;
			if (flag)
			{
				this._enemyNestActionsInAreas = new List<ConfigMonthlyAction>[(int)elementsCount];
			}
			for (int i = 0; i < (int)elementsCount; i++)
			{
				bool flag2 = this._enemyNestActionsInAreas[i] == null;
				if (flag2)
				{
					this._enemyNestActionsInAreas[i] = new List<ConfigMonthlyAction>();
				}
				else
				{
					this._enemyNestActionsInAreas[i].Clear();
				}
				ushort subElementCount = *(ushort*)pCurrData;
				pCurrData += 2;
				for (int j = 0; j < (int)subElementCount; j++)
				{
					ConfigMonthlyAction action = new ConfigMonthlyAction();
					pCurrData += action.Deserialize(pCurrData);
					this._enemyNestActionsInAreas[i].Add(action);
				}
			}
			pCurrData += this.Key.Deserialize(pCurrData);
			this.State = *(sbyte*)pCurrData;
			pCurrData++;
			this.Month = *(int*)pCurrData;
			pCurrData += 4;
			this.LastFinishDate = *(int*)pCurrData;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x0016B7C4 File Offset: 0x001699C4
		[Obsolete]
		public ConfigMonthlyAction CreateNewConfigAction(short templateId, short areaId)
		{
			ConfigMonthlyAction action = new ConfigMonthlyAction(templateId, areaId);
			action.Key = this.Key;
			this._enemyNestActionsInAreas[(int)areaId].Add(action);
			return action;
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x0016B7FC File Offset: 0x001699FC
		[Obsolete("EnemyNest can no longer be upgraded")]
		public static bool UpgradeEnemyNest(DataContext context, ConfigMonthlyAction enemyNestAction, short curLevel)
		{
			bool flag = (int)curLevel >= EnemyNest.Instance.Count - 1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				EnemyNestItem enemyNestCfg = EnemyNest.Instance[(int)(curLevel + 1)];
				bool flag2 = enemyNestCfg.MonthlyActionId < 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					enemyNestAction.State = 1;
					enemyNestAction.Month = 0;
					enemyNestAction.ClearCalledCharacters();
					enemyNestAction.ConfigTemplateId = enemyNestCfg.MonthlyActionId;
					DomainManager.Adventure.UpgradeEnemyNest(context, enemyNestAction.Location.AreaId, enemyNestAction.Location.BlockId, enemyNestCfg.AdventureId);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600193B RID: 6459 RVA: 0x0016B891 File Offset: 0x00169A91
		[Obsolete("Use IMonthlyActionGroup.DeactivateSubAction instead.")]
		public void DeactivateEnemyNest(short areaId, short blockId, bool isComplete)
		{
			this.DeactivateSubAction(areaId, blockId, isComplete);
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x0016B8A0 File Offset: 0x00169AA0
		[Obsolete("EnemyNest can no longer be upgraded")]
		public void GetUpgradableEnemyNestsInArea(short areaId, List<short> result)
		{
			result.Clear();
			AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(areaId);
			foreach (ConfigMonthlyAction enemyNestAction in this._enemyNestActionsInAreas[(int)areaId])
			{
				AdventureItem curAdvCfg = Adventure.Instance[enemyNestAction.ConfigData.AdventureId];
				EnemyNestSiteExtraData nestExtraData;
				bool flag = !DomainManager.Adventure.TryGetElement_EnemyNestSites(enemyNestAction.Location, out nestExtraData);
				if (!flag)
				{
					bool flag2 = !EnemyNestConstValues.HereticStrongholdNestIds.Exist((short)nestExtraData.InitialNestTemplateId);
					if (!flag2)
					{
						AdventureSiteData siteData;
						bool flag3 = adventuresInArea.AdventureSites.TryGetValue(enemyNestAction.Location.BlockId, out siteData) && siteData.SiteState != 1;
						if (!flag3)
						{
							sbyte curLevel = AdventureDomain.GetEnemyNestTemplateId(curAdvCfg.TemplateId);
							short num = (short)curLevel;
							short[] hereticStrongholdNestIds = EnemyNestConstValues.HereticStrongholdNestIds;
							bool flag4 = num == hereticStrongholdNestIds[hereticStrongholdNestIds.Length - 1];
							if (!flag4)
							{
								result.Add(enemyNestAction.Location.BlockId);
							}
						}
					}
				}
			}
		}

		// Token: 0x04000599 RID: 1433
		[SerializableGameDataField]
		private List<ConfigMonthlyAction>[] _enemyNestActionsInAreas;

		// Token: 0x0400059A RID: 1434
		[Obsolete]
		public const sbyte LevelOneNestInitialAmount = 1;

		// Token: 0x0400059B RID: 1435
		[Obsolete]
		public const sbyte LevelTwoNestInitialAmount = 1;

		// Token: 0x0400059C RID: 1436
		[Obsolete]
		public const sbyte LevelThreeNestInitialAmount = 1;

		// Token: 0x0400059D RID: 1437
		[Obsolete]
		public const sbyte MaxUpgradeCount = 9;

		// Token: 0x0400059E RID: 1438
		[Obsolete]
		public const sbyte MaxHereticStrongholdCount = 5;

		// Token: 0x0400059F RID: 1439
		[Obsolete]
		public const sbyte MaxRighteousStrongholdCount = 3;
	}
}
