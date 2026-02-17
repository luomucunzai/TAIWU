using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Config;
using Config.ConfigCells;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Common.Binary;
using GameData.Common.SingleValueCollection;
using GameData.Common.WorkerThread;
using GameData.Dependencies;
using GameData.DLC;
using GameData.DLC.FiveLoong;
using GameData.DomainEvents;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.Character.Filters;
using GameData.Domains.Character.Relation;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Global;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.LifeRecord.GeneralRecord;
using GameData.Domains.Map;
using GameData.Domains.Mod;
using GameData.Domains.Organization;
using GameData.Domains.Organization.Display;
using GameData.Domains.Taiwu;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.Domains.World.SectMainStory;
using GameData.Domains.World.Task;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using Redzen.Random;

namespace GameData.Domains.World
{
	// Token: 0x0200002F RID: 47
	[GameDataDomain(1)]
	public class WorldDomain : BaseGameDataDomain
	{
		// Token: 0x06000D27 RID: 3367 RVA: 0x000DE748 File Offset: 0x000DC948
		private void OnInitializedDomainData()
		{
			this._monthlyEventCollection = new MonthlyEventCollection();
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x000DE756 File Offset: 0x000DC956
		private void InitializeOnInitializeGameDataModule()
		{
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x000DE75C File Offset: 0x000DC95C
		private void InitializeOnEnterNewWorld()
		{
			DataContext context = DataContextManager.GetCurrentThreadDataContext();
			this._worldId = context.Random.NextUInt();
			this._xiangshuProgress = 0;
			this._mainStoryLineProgress = 0;
			this._worldFunctionsStatuses = 0UL;
			this._nextCustomTextId = 0;
			this._currDate = 8;
			this._advancingMonthState = 0;
			this._instantNotificationsCommittedOffset = 0;
			sbyte i = 0;
			while ((int)i < this._xiangshuAvatarTasksInOrder.Length)
			{
				this._xiangshuAvatarTasksInOrder[(int)i] = i;
				i += 1;
			}
			CollectionUtils.Shuffle<sbyte>(context.Random, this._xiangshuAvatarTasksInOrder);
			this.SetWorldFunctionsStatus(context, 19);
			this.SetWorldFunctionsStatus(context, 20);
			this.SetWorldFunctionsStatus(context, 17);
			Logger logger = WorldDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
			defaultInterpolatedStringHandler.AppendLiteral("EnterNewWorld: ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this._worldId);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x000DE83C File Offset: 0x000DCA3C
		private void OnLoadedArchiveData()
		{
			this._instantNotificationsCommittedOffset = this._instantNotifications.Size;
			Logger logger = WorldDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
			defaultInterpolatedStringHandler.AppendLiteral("LoadWorld: ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this._worldId);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x000DE892 File Offset: 0x000DCA92
		public override void OnCurrWorldArchiveDataReady(DataContext context, bool isNewWorld)
		{
			this.InitializeBaihuaLinkedCharacters(context);
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x000DE8A0 File Offset: 0x000DCAA0
		public override void FixAbnormalDomainArchiveData(DataContext context)
		{
			bool flag = DomainManager.World.GetMainStoryLineProgress() >= 7 && !this.IsCurrWorldBeforeVersion(0, 0, 63, 0);
			if (flag)
			{
				this.SetWorldFunctionsStatus(context, 5);
			}
			bool worldFunctionsStatus = this.GetWorldFunctionsStatus(4);
			if (worldFunctionsStatus)
			{
				this.SetWorldFunctionsStatus(context, 3);
			}
			GameData.Domains.Character.Character character;
			bool flag2 = DomainManager.Character.TryGetFixedCharacterByTemplateId(441, out character);
			if (flag2)
			{
				this.SetWorldFunctionsStatus(context, 21);
			}
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x000DE90C File Offset: 0x000DCB0C
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			28
		}, Condition = InfluenceCondition.CharIsTaiwu)]
		private sbyte CalcXiangshuProgress()
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			bool flag = taiwuChar == null;
			sbyte result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int consummateLevel = (int)taiwuChar.GetConsummateLevel();
				result = (sbyte)Math.Clamp(consummateLevel, 0, 18);
			}
			return result;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x000DE948 File Offset: 0x000DCB48
		[SingleValueDependency(1, new ushort[]
		{
			1,
			5,
			27
		})]
		[ElementListDependency(1, 4, 15)]
		[SingleValueDependency(5, new ushort[]
		{
			25,
			0,
			8,
			9,
			58,
			70,
			34,
			74,
			75
		})]
		[SingleValueDependency(2, new ushort[]
		{
			57
		})]
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			21,
			26,
			44,
			65
		}, Condition = InfluenceCondition.CharIsTaiwu)]
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			105,
			104
		}, Condition = InfluenceCondition.CharIsInTaiwuGroup)]
		[ElementListDependency(10, 17, 45)]
		[SingleValueDependency(19, new ushort[]
		{
			56
		})]
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			26
		}, Condition = InfluenceCondition.CharIsInTaiwuGroup)]
		[SingleValueDependency(19, new ushort[]
		{
			157
		})]
		[SingleValueDependency(19, new ushort[]
		{
			265
		})]
		[SingleValueCollectionDependency(19, new ushort[]
		{
			163,
			245,
			223
		})]
		[SingleValueDependency(2, new ushort[]
		{
			66
		})]
		[SingleValueCollectionDependency(19, new ushort[]
		{
			247
		}, Scope = InfluenceScope.TaiwuChar)]
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			17
		}, Condition = InfluenceCondition.CharIsInTaiwuGroup)]
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			95
		}, Condition = InfluenceCondition.CharIsInTaiwuGroup)]
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			19
		}, Condition = InfluenceCondition.CharIsInTaiwuGroup)]
		[SingleValueDependency(9, new ushort[]
		{
			15
		})]
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			112
		}, Condition = InfluenceCondition.CharIsInTaiwuGroup)]
		private WorldStateData CalcWorldStateData()
		{
			WorldStateData value = default(WorldStateData);
			ref value.DetectWarehouseOverload();
			ref value.DetectResourceOverload();
			ref value.DetectInventoryOverload();
			ref value.DetectInjuries();
			ref value.DetectPoisons();
			ref value.DetectDisorderOfQi();
			ref value.DetectTeammateInjuries();
			ref value.DetectXiangshuInvasionProgress();
			ref value.DetectXiangshuInfection();
			ref value.DetectMainStory();
			ref value.DetectXiangshuAvatars();
			ref value.DetectMartialArtTournament();
			ref value.DetectChangeWorldCreation();
			ref value.DetectLoongDebuff();
			ref value.DetectInFulongFlameArea();
			ref value.DetectTribulation();
			ref value.DetectSectMainStory();
			ref value.DetectTaiwuWanted();
			ref value.DetectTeammateDying();
			ref value.DetectHomelessVillager();
			ref value.DetectNeiliConflicting();
			return value;
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x000DEA0C File Offset: 0x000DCC0C
		[ElementListDependency(1, 2, 9)]
		[SingleValueDependency(1, new ushort[]
		{
			5,
			7
		})]
		[SingleValueDependency(12, new ushort[]
		{
			0
		})]
		[SingleValueDependency(5, new ushort[]
		{
			34
		})]
		[SingleValueDependency(19, new ushort[]
		{
			56
		})]
		[SingleValueDependency(19, new ushort[]
		{
			246
		})]
		[ElementListDependency(10, 17, 139)]
		[SingleValueCollectionDependency(19, new ushort[]
		{
			245
		})]
		[SingleValueCollectionDependency(9, new ushort[]
		{
			0
		})]
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			58,
			56
		}, Condition = InfluenceCondition.CharIsTaskRelated)]
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			3,
			5,
			1,
			10
		}, Scope = InfluenceScope.CombatSkillOwner)]
		private void CalcCurrTaskList(List<TaskData> value)
		{
			foreach (TaskChainItem chainCfg in ((IEnumerable<TaskChainItem>)TaskChain.Instance))
			{
				bool flag = !this.CheckCurrMainStoryLineProgressInRange(chainCfg.MainStoryLineMin, chainCfg.MainStoryLineMax);
				if (!flag)
				{
					bool flag2 = chainCfg.TaskList.Count <= 0;
					if (!flag2)
					{
						bool flag3 = chainCfg.StartConditions.Count > 0 && !chainCfg.StartConditions.TrueForAll(new Predicate<int>(TaskConditionChecker.CheckCondition));
						if (!flag3)
						{
							bool flag4 = chainCfg.RemoveCondtions.Count > 0 && chainCfg.RemoveCondtions.TrueForAll(new Predicate<int>(TaskConditionChecker.CheckCondition));
							if (!flag4)
							{
								foreach (int taskInfoId in chainCfg.TaskList)
								{
									TaskInfoItem taskCfg = TaskInfo.Instance[taskInfoId];
									bool isTriggeredTask = taskCfg.IsTriggeredTask;
									if (!isTriggeredTask)
									{
										bool flag5 = !this.CheckCurrMainStoryLineProgressInRange(taskCfg.MainStoryLineMin, taskCfg.MainStoryLineMax);
										if (!flag5)
										{
											bool flag6 = taskCfg.RunCondition.Count > 0 && !taskCfg.RunCondition.TrueForAll(new Predicate<int>(TaskConditionChecker.CheckCondition));
											if (!flag6)
											{
												bool flag7 = taskCfg.FinishCondition.Count > 0 && taskCfg.FinishCondition.TrueForAll(new Predicate<int>(TaskConditionChecker.CheckCondition));
												if (!flag7)
												{
													bool isBlocked = taskCfg.BlockCondition.Count > 0 && taskCfg.BlockCondition.TrueForAll(new Predicate<int>(TaskConditionChecker.CheckCondition));
													TaskData taskData = new TaskData
													{
														TaskChainId = chainCfg.TemplateId,
														TaskInfoId = taskInfoId,
														TaskStatus = (isBlocked ? 1 : 0)
													};
													value.Add(taskData);
													bool flag8 = chainCfg.Type == ETaskChainType.Line;
													if (flag8)
													{
														break;
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
			List<TaskData> extraTriggeredTasks = DomainManager.Extra.GetExtraTriggeredTasks();
			foreach (TaskData extraTask in extraTriggeredTasks)
			{
				TaskInfoItem taskCfg2 = TaskInfo.Instance[extraTask.TaskInfoId];
				bool flag9 = !this.CheckCurrMainStoryLineProgressInRange(taskCfg2.MainStoryLineMin, taskCfg2.MainStoryLineMax);
				if (!flag9)
				{
					bool isBlocked2 = taskCfg2.BlockCondition.Count > 0 && taskCfg2.BlockCondition.TrueForAll(new Predicate<int>(TaskConditionChecker.CheckCondition));
					TaskData task = extraTask;
					task.TaskStatus = (isBlocked2 ? 1 : 0);
					value.Add(task);
				}
			}
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x000DED54 File Offset: 0x000DCF54
		[SingleValueDependency(1, new ushort[]
		{
			30
		})]
		[SingleValueDependency(19, new ushort[]
		{
			57
		})]
		[ObjectCollectionDependency(7, 0, new ushort[]
		{
			1
		}, Condition = InfluenceCondition.CombatSkillIsLearnedByTaiwu)]
		private void CalcSortedTaskList(List<TaskDisplayData> value)
		{
			List<TaskData> taskList = this.GetCurrTaskList();
			List<int> sortingOrder = DomainManager.Extra.GetTaskSortingOrder();
			foreach (TaskData task in taskList)
			{
				TaskInfoItem taskCfg = TaskInfo.Instance[task.TaskInfoId];
				bool flag = taskCfg == null;
				if (flag)
				{
					PredefinedLog.Show(10, task.TaskInfoId);
				}
				else
				{
					TaskDisplayData displayData = default(TaskDisplayData);
					displayData.DisplayType = 0;
					displayData.TargetLocation = Location.Invalid;
					displayData.SkillIdList = GameData.Utilities.ShortList.Create();
					displayData.CountDown = -1;
					displayData.SettlementNameData = new SettlementNameRelatedData
					{
						RandomNameId = -1,
						MapBlockTemplateId = -1
					};
					displayData.StringArray = null;
					displayData.InnerTaskData = task;
					foreach (short charId in taskCfg.CharacterTemplateId)
					{
						GameData.Domains.Character.Character character;
						bool flag2 = DomainManager.Character.TryGetFixedCharacterByTemplateId(charId, out character);
						if (flag2)
						{
							bool flag3 = character.GetLocation().IsValid();
							if (flag3)
							{
								displayData.TargetLocation = character.GetLocation();
							}
						}
					}
					TaskChainItem taskChainCfg = TaskChain.Instance[task.TaskChainId];
					ETaskChainGroup group = taskChainCfg.Group;
					if (!true)
					{
					}
					EventArgBox eventArgBox;
					if (group != ETaskChainGroup.MainStory)
					{
						if (group != ETaskChainGroup.SectMainStory)
						{
							eventArgBox = null;
						}
						else
						{
							eventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(taskChainCfg.Sect);
						}
					}
					else
					{
						eventArgBox = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
					}
					if (!true)
					{
					}
					EventArgBox argBox = eventArgBox;
					bool flag4 = argBox != null;
					int templateId;
					if (flag4)
					{
						bool flag5 = !string.IsNullOrEmpty(taskCfg.EventArgBoxKey);
						if (flag5)
						{
							bool flag6 = taskCfg.EventArgBoxKey == "ConchShip_PresetKey_StudyForBodhidharmaChallenge";
							if (flag6)
							{
								displayData.DisplayType |= 64;
								displayData.CountDown = argBox.GetInt(taskCfg.EventArgBoxKey);
							}
							else
							{
								bool flag7 = taskCfg.EventArgBoxKey == "ConchShip_PresetKey_SanZongBiWuCountDown" || taskCfg.EventArgBoxKey == "ConchShip_PresetKey_FulongAdventureOneCountDown" || taskCfg.EventArgBoxKey == "ConchShip_PresetKey_FulongAdventureThreeCountDown";
								if (flag7)
								{
									displayData.DisplayType |= 8;
									displayData.CountDown = argBox.GetInt(taskCfg.EventArgBoxKey);
								}
								else
								{
									bool flag8 = taskCfg.TemplateId == 309;
									if (flag8)
									{
										displayData.DisplayType |= 128;
										short settlementId = -1;
										argBox.Get("ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementId", ref settlementId);
										sbyte fiveElementType = -1;
										argBox.Get("ConchShip_PresetKey_BaihuaLeukoKillsFiveElementsType", ref fiveElementType);
										Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
										List<SettlementNameRelatedData> settlementNameDataList = DomainManager.Organization.GetSettlementNameRelatedData(new List<short>
										{
											settlement.GetId()
										});
										displayData.TargetLocation = DomainManager.Organization.GetSettlement(settlementId).GetLocation();
										displayData.SettlementNameData = settlementNameDataList[0];
										displayData.StringArray = new string[2];
										displayData.StringArray[0] = fiveElementType.ToString();
										displayData.StringArray[1] = MathF.Max(0f, (float)(1 - DomainManager.World.BaihuaGroupMeetCount(true, out templateId))).ToString();
										value.Add(displayData);
										continue;
									}
									bool flag9 = taskCfg.TemplateId == 311;
									if (flag9)
									{
										displayData.DisplayType |= 128;
										short settlementId2 = -1;
										argBox.Get("ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementId", ref settlementId2);
										sbyte fiveElementType2 = -1;
										argBox.Get("ConchShip_PresetKey_BaihuaMelanoKillsFiveElementsType", ref fiveElementType2);
										Settlement settlement2 = DomainManager.Organization.GetSettlement(settlementId2);
										List<SettlementNameRelatedData> settlementNameDataList2 = DomainManager.Organization.GetSettlementNameRelatedData(new List<short>
										{
											settlement2.GetId()
										});
										displayData.TargetLocation = DomainManager.Organization.GetSettlement(settlementId2).GetLocation();
										displayData.SettlementNameData = settlementNameDataList2[0];
										displayData.StringArray = new string[2];
										displayData.StringArray[0] = fiveElementType2.ToString();
										displayData.StringArray[1] = MathF.Max(0f, (float)(1 - DomainManager.World.BaihuaGroupMeetCount(false, out templateId))).ToString();
										value.Add(displayData);
										continue;
									}
									bool flag10 = !displayData.TargetLocation.IsValid();
									if (flag10)
									{
										bool flag11 = argBox.Get<Location>(taskCfg.EventArgBoxKey, out displayData.TargetLocation);
										if (flag11)
										{
											MapBlockData blockData = DomainManager.Map.GetBlock(displayData.TargetLocation);
											bool flag12 = blockData.IsCityTown();
											if (flag12)
											{
												Settlement settlement3 = DomainManager.Organization.GetSettlementByLocation(displayData.TargetLocation);
												List<SettlementNameRelatedData> settlementNameDataList3 = DomainManager.Organization.GetSettlementNameRelatedData(new List<short>
												{
													settlement3.GetId()
												});
												displayData.SettlementNameData = settlementNameDataList3[0];
											}
										}
										else
										{
											short settlementId3 = -1;
											argBox.Get(taskCfg.EventArgBoxKey, ref settlementId3);
											displayData.TargetLocation = DomainManager.Organization.GetSettlement(settlementId3).GetLocation();
											List<SettlementNameRelatedData> settlementNameDataList4 = DomainManager.Organization.GetSettlementNameRelatedData(new List<short>
											{
												settlementId3
											});
											displayData.SettlementNameData = settlementNameDataList4[0];
										}
									}
								}
							}
						}
						string[] array = taskCfg.CombatSkillIdsEventArgBoxKey;
						bool flag13 = array != null && array.Length > 0;
						if (flag13)
						{
							displayData.DisplayType |= 1;
							foreach (string key in taskCfg.CombatSkillIdsEventArgBoxKey)
							{
								short skillId = -1;
								bool flag14 = !argBox.Get(key, ref skillId);
								if (!flag14)
								{
									GameData.Domains.CombatSkill.CombatSkill skill;
									bool flag15 = skillId > -1 && DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(DomainManager.Taiwu.GetTaiwuCharId(), skillId), out skill);
									if (flag15)
									{
										bool flag16 = !skill.GetRevoked() && !CombatSkillStateHelper.IsBrokenOut(skill.GetActivationState());
										if (flag16)
										{
											displayData.SkillIdList.Items.Add(skillId);
										}
									}
								}
							}
						}
						array = taskCfg.SkillIdsEventArgBoxKey;
						bool flag17 = array != null && array.Length > 0;
						if (flag17)
						{
							displayData.DisplayType |= 4;
							foreach (string key2 in taskCfg.SkillIdsEventArgBoxKey)
							{
								short skillId2 = argBox.GetShort(key2);
								bool flag18 = skillId2 > -1;
								if (flag18)
								{
									displayData.SkillIdList.Items.Add(skillId2);
								}
							}
						}
						array = taskCfg.StringArrayEventArgBoxKey;
						bool flag19 = array != null && array.Length > 0;
						if (flag19)
						{
							displayData.DisplayType |= 32;
							displayData.StringArray = new string[taskCfg.StringArrayEventArgBoxKey.Length];
							int i = 0;
							int max = taskCfg.StringArrayEventArgBoxKey.Length;
							while (i < max)
							{
								displayData.StringArray[i] = argBox.GetString(taskCfg.StringArrayEventArgBoxKey[i]);
								i++;
							}
						}
					}
					bool flag20 = displayData.TargetLocation.IsValid();
					if (flag20)
					{
						displayData.DisplayType |= 2;
					}
					bool flag21 = taskCfg.FrontEndKey != null;
					if (flag21)
					{
						displayData.DisplayType |= 16;
					}
					templateId = taskCfg.TemplateId;
					bool flag22 = templateId >= 268 && templateId <= 272;
					if (flag22)
					{
						short loongCharacterTemplateId = (short)(taskCfg.TemplateId - 268 + 686);
						LoongInfo loongInfo;
						bool flag23 = DomainManager.Extra.TryGetElement_FiveLoongDict(loongCharacterTemplateId, out loongInfo);
						if (flag23)
						{
							displayData.DisplayType |= 2;
							displayData.TargetLocation = loongInfo.LoongCurrentLocation;
						}
					}
					bool flag24 = taskCfg.TemplateId == 342;
					if (flag24)
					{
						displayData.DisplayType |= 256;
						displayData.TargetLocations = new List<Location>();
						bool flag25 = DomainManager.Building.ChickenMapInfo == null;
						if (flag25)
						{
							DomainManager.Building.ClickChickenMap(DataContextManager.GetCurrentThreadDataContext());
						}
						for (int j = 0; j < DomainManager.Building.ChickenMapInfo.Count; j++)
						{
							short settlementId4 = DomainManager.Building.ChickenMapInfo[j];
							Settlement settlement4 = DomainManager.Organization.GetSettlement(settlementId4);
							displayData.TargetLocations.Add(settlement4.GetLocation());
						}
						List<SettlementNameRelatedData> settlementNameDataList5 = DomainManager.Organization.GetSettlementNameRelatedData(DomainManager.Building.ChickenMapInfo);
						displayData.SettlementNameDatas = settlementNameDataList5;
					}
					value.Add(displayData);
				}
			}
			value.Sort(delegate(TaskDisplayData a, TaskDisplayData b)
			{
				bool flag26 = a.InnerTaskData.IsBlocked != b.InnerTaskData.IsBlocked;
				int result;
				if (flag26)
				{
					result = a.InnerTaskData.IsBlocked.CompareTo(b.InnerTaskData.IsBlocked);
				}
				else
				{
					bool flag27 = sortingOrder.IndexOf(a.InnerTaskData.TaskInfoId) != -1;
					if (flag27)
					{
						result = -1;
					}
					else
					{
						bool flag28 = sortingOrder.IndexOf(b.InnerTaskData.TaskInfoId) != -1;
						if (flag28)
						{
							result = 1;
						}
						else
						{
							TaskInfoItem task2 = TaskInfo.Instance[a.InnerTaskData.TaskInfoId];
							TaskInfoItem task3 = TaskInfo.Instance[b.InnerTaskData.TaskInfoId];
							bool flag29 = task2.TaskOrder == task3.TaskOrder;
							if (flag29)
							{
								result = task2.TemplateId.CompareTo(task3.TemplateId);
							}
							else
							{
								result = task2.TaskOrder.CompareTo(task3.TaskOrder);
							}
						}
					}
				}
				return result;
			});
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x000DF668 File Offset: 0x000DD868
		public sbyte GetXiangshuLevel()
		{
			return GameData.Domains.World.SharedMethods.GetXiangshuLevel(this.GetXiangshuProgress());
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x000DF688 File Offset: 0x000DD888
		public sbyte GetMaxGradeOfXiangshuInfection()
		{
			return GameData.Domains.World.SharedMethods.GetMaxGradeOfXiangshuInfection(this.GetXiangshuProgress());
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x000DF6A8 File Offset: 0x000DD8A8
		public void SetSwordTombStatus(DataContext context, sbyte xiangshuAvatarId, sbyte swordTombStatus)
		{
			bool isPuppetCombat = DomainManager.Combat.GetIsPuppetCombat();
			if (!isPuppetCombat)
			{
				XiangshuAvatarTaskStatus taskStatus = this._xiangshuAvatarTaskStatuses[(int)xiangshuAvatarId];
				bool flag = swordTombStatus <= taskStatus.SwordTombStatus;
				if (!flag)
				{
					int delta = (int)(swordTombStatus - taskStatus.SwordTombStatus);
					taskStatus.SwordTombStatus = swordTombStatus;
					this.SetElement_XiangshuAvatarTaskStatuses((int)xiangshuAvatarId, taskStatus, context);
					GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
					taiwuChar.ChangeConsummateLevel(context, delta);
					DomainManager.Taiwu.UpdateConsummateLevelBrokenFeature(context);
					bool flag2 = swordTombStatus == 2;
					if (flag2)
					{
						sbyte level = TaiwuDomain.GetWorldCreationGroupLevel(0);
						short legacy = SwordTomb.Instance[xiangshuAvatarId].Legacies[(int)level];
						bool flag3 = DomainManager.Taiwu.AddAvailableLegacy(context, legacy);
						if (flag3)
						{
							DomainManager.Combat.AddCombatResultLegacy(legacy);
						}
						DomainManager.Extra.BigEventGainSwordTombRemoved(context, xiangshuAvatarId);
					}
				}
			}
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x000DF780 File Offset: 0x000DD980
		public unsafe void ChangeMainStoryLineProgress(DataContext context, short progress)
		{
			bool flag = !MainStoryLineProgress.CheckTransition(this._mainStoryLineProgress, progress);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (flag)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid transition: ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(this._mainStoryLineProgress);
				defaultInterpolatedStringHandler.AppendLiteral(" -> ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(progress);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Logger logger = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Main storyline progress is being changed to ");
			defaultInterpolatedStringHandler.AppendFormatted<short>(progress);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			bool flag2 = progress == 27;
			if (flag2)
			{
				DomainManager.Organization.TryRemoveTaiwuGroupBountyAndPunishment(context);
			}
			bool flag3 = progress == 8;
			if (flag3)
			{
				DomainManager.Building.SetAllResidenceAutoCheckIn(context);
			}
			bool flag4 = progress == 3;
			if (flag4)
			{
				GameData.Domains.Character.Character victim = EventHelper.GetOrCreateFixedCharacterByTemplateId(467);
				victim.AddDarkAsh(context, null, 0);
				victim.DirectlyChangeDarkAshDuration(context, DomainManager.World.GetCurrDate(), 0, 0, 6);
			}
			bool flag5 = progress == 6;
			if (flag5)
			{
				Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(138);
				for (int i = 0; i < areaBlocks.Length; i++)
				{
					MapBlockData block = *areaBlocks[i];
					bool flag6 = block.TemplateId == 36;
					if (flag6)
					{
						Settlement settlement = DomainManager.Organization.GetSettlementByLocation(new Location(138, block.BlockId));
						int max = context.Random.Next(GlobalConfig.Instance.BrokenPerformDarkAshInfectorRangeMax) + GlobalConfig.Instance.BrokenPerformDarkAshInfectorBase;
						foreach (int member in settlement.GetMembers())
						{
							GameData.Domains.Character.Character character;
							bool flag7 = DomainManager.Character.TryGetElement_Objects(member, out character);
							if (flag7)
							{
								bool flag8 = character.GetDarkAshProtector() == 0U && character.GetActualAge() >= 16 && max-- > 0;
								if (flag8)
								{
									character.AddDarkAsh(context, null, int.MinValue);
								}
							}
						}
						break;
					}
				}
			}
			bool flag9 = progress == 16 && !DomainManager.Extra.GetIsDreamBack();
			if (flag9)
			{
				ExtraDomain extra = DomainManager.Extra;
				sbyte[] fuyuFaithCountBySaveInfected = GlobalConfig.FuyuFaithCountBySaveInfected;
				extra.AddFuyuFaith(context, (int)fuyuFaithCountBySaveInfected[fuyuFaithCountBySaveInfected.Length - 1]);
				InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
				sbyte[] fuyuFaithCountBySaveInfected2 = GlobalConfig.FuyuFaithCountBySaveInfected;
				instantNotificationCollection.AddGainFuyuFaith3((int)fuyuFaithCountBySaveInfected2[fuyuFaithCountBySaveInfected2.Length - 1]);
			}
			this.SetMainStoryLineProgress(progress, context);
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x000DFA10 File Offset: 0x000DDC10
		public bool CheckCurrMainStoryLineProgressInRange(short min, short max)
		{
			return this._mainStoryLineProgress >= min && this._mainStoryLineProgress < max;
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x000DFA38 File Offset: 0x000DDC38
		public bool GetWorldFunctionsStatus(byte worldFunctionType)
		{
			return WorldFunctionType.Get(this._worldFunctionsStatuses, worldFunctionType);
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x000DFA58 File Offset: 0x000DDC58
		public void SetWorldFunctionsStatus(DataContext context, byte worldFunctionType)
		{
			ulong worldFunctionStatuses = WorldFunctionType.Set(this._worldFunctionsStatuses, worldFunctionType);
			bool flag = worldFunctionStatuses != this._worldFunctionsStatuses;
			if (flag)
			{
				this.SetWorldFunctionsStatuses(worldFunctionStatuses, context);
				Logger logger = WorldDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unlocking world function: ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(worldFunctionType);
				logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x000DFAC0 File Offset: 0x000DDCC0
		public void ResetWorldFunctionsStatus(DataContext context, byte worldFunctionType)
		{
			ulong worldFunctionStatuses = WorldFunctionType.Reset(this._worldFunctionsStatuses, worldFunctionType);
			bool flag = worldFunctionStatuses != this._worldFunctionsStatuses;
			if (flag)
			{
				this.SetWorldFunctionsStatuses(worldFunctionStatuses, context);
				Logger logger = WorldDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Reseting world function status: ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(worldFunctionType);
				logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x000DFB28 File Offset: 0x000DDD28
		[DomainMethod]
		public void CreateWorld(DataContext context, WorldCreationInfo info)
		{
			Stopwatch sw = GlobalDomain.StartTimer();
			this.SetWorldCreationInfo(context, info, false);
			context.SwitchRandomSource((ulong)this._worldId);
			DomainManager.Extra.InitializeWorldVersionInfo(context);
			DomainManager.Map.CreateAllAreas(context);
			DomainManager.Character.CreatePregeneratedCityTownGuards(context);
			DomainManager.Character.CreatePregeneratedRandomEnemies(context);
			DomainManager.Extra.CreatePregeneratedFixedEnemies(context);
			Stopwatch sw2 = GlobalDomain.StartTimer();
			DomainManager.Extra.CreatePickups(context);
			GlobalDomain.StopTimer(sw2, "CreatePickups");
			GlobalDomain.StopTimer(sw, "CreateWorld");
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x000DFBBC File Offset: 0x000DDDBC
		[DomainMethod]
		public void SetWorldCreationInfo(DataContext context, WorldCreationInfo info, bool inherit)
		{
			this.SetWorldPopulationType(info.WorldPopulationType, context);
			this.SetCharacterLifespanType(info.CharacterLifespanType, context);
			this.SetCombatDifficulty(info.CombatDifficulty, context);
			this.SetHereticsAmountType(info.HereticsAmountType, context);
			byte prevBossInvasionType = this.GetBossInvasionSpeedType();
			this.SetBossInvasionSpeedType(info.BossInvasionSpeedType, context);
			this.SetWorldResourceAmountType(info.WorldResourceAmountType, context);
			this.SetAllowRandomTaiwuHeir(info.AllowRandomTaiwuHeir, context);
			this.SetRestrictOptionsBehaviorType(info.RestrictOptionsBehaviorType, context);
			this.SetTaiwuVillageStateTemplateId(info.TaiwuVillageStateTemplateId, context);
			this.SetTaiwuVillageLandFormType(info.TaiwuVillageLandFormType, context);
			DomainManager.Extra.SetReadingDifficulty(info.ReadingDifficulty, context);
			DomainManager.Extra.SetBreakoutDifficulty(info.BreakoutDifficulty, context);
			DomainManager.Extra.SetLoopingDifficulty(info.LoopingDifficulty, context);
			DomainManager.Extra.SetEnemyPracticeLevel(info.EnemyPracticeLevel, context);
			DomainManager.Extra.SetFavorabilityChange(info.FavorabilityChange, context);
			DomainManager.Extra.SetProfessionUpgrade(info.ProfessionUpgrade, context);
			DomainManager.Extra.SetLootYield(info.LootYield, context);
			bool flag = !inherit;
			if (flag)
			{
				DomainManager.Extra.SetCanResetWorldSettings(false, context);
			}
			bool flag2 = this.GetMainStoryLineProgress() >= 16 && prevBossInvasionType != info.BossInvasionSpeedType;
			if (flag2)
			{
				Events.RaiseBossInvasionSpeedTypeChanged(context, prevBossInvasionType);
			}
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x000DFD18 File Offset: 0x000DDF18
		[DomainMethod]
		public WorldCreationInfo GetWorldCreationInfo()
		{
			return new WorldCreationInfo
			{
				WorldPopulationType = DomainManager.World.GetWorldPopulationType(),
				CharacterLifespanType = DomainManager.World.GetCharacterLifespanType(),
				CombatDifficulty = DomainManager.World.GetCombatDifficulty(),
				ReadingDifficulty = DomainManager.Extra.GetReadingDifficulty(),
				BreakoutDifficulty = DomainManager.Extra.GetBreakoutDifficulty(),
				LoopingDifficulty = DomainManager.Extra.GetLoopingDifficulty(),
				EnemyPracticeLevel = DomainManager.Extra.GetEnemyPracticeLevel(),
				FavorabilityChange = DomainManager.Extra.GetFavorabilityChange(),
				ProfessionUpgrade = DomainManager.Extra.GetProfessionUpgrade(),
				LootYield = DomainManager.Extra.GetLootYield(),
				HereticsAmountType = DomainManager.World.GetHereticsAmountType(),
				BossInvasionSpeedType = DomainManager.World.GetBossInvasionSpeedType(),
				WorldResourceAmountType = DomainManager.World.GetWorldResourceAmountType(),
				AllowRandomTaiwuHeir = DomainManager.World.GetAllowRandomTaiwuHeir(),
				RestrictOptionsBehaviorType = DomainManager.World.GetRestrictOptionsBehaviorType(),
				TaiwuVillageStateTemplateId = DomainManager.World._taiwuVillageStateTemplateId,
				TaiwuVillageLandFormType = DomainManager.World._taiwuVillageLandFormType
			};
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x000DFE54 File Offset: 0x000DE054
		public static WorldInfo GetWorldInfo()
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			ValueTuple<string, string> realName = CharacterDomain.GetRealName(taiwu);
			string surname = realName.Item1;
			string givenName = realName.Item2;
			Location location = taiwu.GetLocation();
			bool flag = !location.IsValid();
			if (flag)
			{
				location = taiwu.GetValidLocation();
			}
			ValueTuple<string, string> stateAndAreaNameByAreaId = DomainManager.Map.GetStateAndAreaNameByAreaId(location.AreaId);
			string stateName = stateAndAreaNameByAreaId.Item1;
			string areaName = stateAndAreaNameByAreaId.Item2;
			return new WorldInfo
			{
				CurrDate = DomainManager.World.GetCurrDate(),
				TaiwuGenerationsCount = DomainManager.Taiwu.GetTaiwuGenerationsCount(),
				SavingTimestamp = DateTime.UtcNow.Ticks,
				TaiwuSurname = surname,
				TaiwuGivenName = givenName,
				Gender = taiwu.GetGender(),
				AvatarRelatedData = taiwu.GenerateAvatarRelatedData(),
				AvatarExtraData = DomainManager.Extra.GetCharacterAvatarExtraData(DataContextManager.GetCurrentThreadDataContext(), taiwu.GetId()),
				MapStateName = stateName,
				MapAreaName = areaName,
				CharacterLifespanType = DomainManager.World.GetCharacterLifespanType(),
				CombatDifficulty = DomainManager.World.GetCombatDifficulty(),
				BreakoutDifficulty = DomainManager.Extra.GetBreakoutDifficulty(),
				ReadingDifficulty = DomainManager.Extra.GetReadingDifficulty(),
				LoopingDifficulty = DomainManager.Extra.GetLoopingDifficulty(),
				EnemyPracticeLevel = DomainManager.Extra.GetEnemyPracticeLevel(),
				FavorabilityChange = DomainManager.Extra.GetFavorabilityChange(),
				ProfessionUpgrade = DomainManager.Extra.GetProfessionUpgrade(),
				LootYield = DomainManager.Extra.GetLootYield(),
				HereticsAmountType = DomainManager.World.GetHereticsAmountType(),
				BossInvasionSpeedType = DomainManager.World.GetBossInvasionSpeedType(),
				WorldResourceAmountType = DomainManager.World.GetWorldResourceAmountType(),
				WorldPopulationType = DomainManager.World.GetWorldPopulationType(),
				AllowRandomTaiwuHeir = DomainManager.World.GetAllowRandomTaiwuHeir(),
				RestrictOptionsBehaviorType = DomainManager.World.GetRestrictOptionsBehaviorType(),
				StateTaskStatuses = DomainManager.World._stateTaskStatuses.ToArray<sbyte>(),
				XiangshuAvatarTaskStatuses = DomainManager.World._xiangshuAvatarTaskStatuses.ToArray<XiangshuAvatarTaskStatus>(),
				MainStoryLineProgress = DomainManager.World._mainStoryLineProgress,
				BeatRanChenZi = DomainManager.World._beatRanChenZi,
				ModIds = ModDomain.GetLoadedModIds(),
				DlcIds = DlcManager.GetAllInstalledDlcIds(),
				GameVersionInfo = DomainManager.Extra.GetWorldVersionInfo()
			};
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x000E00B0 File Offset: 0x000DE2B0
		public void UpdateCurrWorldGameVersion()
		{
			GameVersionInfo gameVersionInfo = DomainManager.Extra.GetWorldVersionInfo();
			this._currWorldGameVersion = ((gameVersionInfo != null && !string.IsNullOrEmpty(gameVersionInfo.GameVersionLastSaving)) ? GameVersionInfo.ParseGameVersion(gameVersionInfo.GameVersionLastSaving) : new Version(0, 0, 61, 23));
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x000E00F8 File Offset: 0x000DE2F8
		public Version GetCurrWorldGameVersion()
		{
			return this._currWorldGameVersion;
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x000E0110 File Offset: 0x000DE310
		public bool IsCurrWorldBeforeVersion(int major, int minor = 0, int build = 0, int revision = 0)
		{
			bool flag = this._currWorldGameVersion == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._currWorldGameVersion.Major != major;
				if (flag2)
				{
					result = (this._currWorldGameVersion.Major < major);
				}
				else
				{
					bool flag3 = this._currWorldGameVersion.Minor != minor;
					if (flag3)
					{
						result = (this._currWorldGameVersion.Minor < minor);
					}
					else
					{
						bool flag4 = this._currWorldGameVersion.Build != build;
						if (flag4)
						{
							result = (this._currWorldGameVersion.Build < build);
						}
						else
						{
							bool flag5 = this._currWorldGameVersion.Revision != revision;
							result = (flag5 && this._currWorldGameVersion.Revision < revision);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x000E01D8 File Offset: 0x000DE3D8
		public bool IsCurrWorldAfterVersion(int major, int minor = 0, int build = 0, int revision = 0)
		{
			bool flag = this._currWorldGameVersion == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._currWorldGameVersion.Major != major;
				if (flag2)
				{
					result = (this._currWorldGameVersion.Major > major);
				}
				else
				{
					bool flag3 = this._currWorldGameVersion.Minor != minor;
					if (flag3)
					{
						result = (this._currWorldGameVersion.Minor > minor);
					}
					else
					{
						bool flag4 = this._currWorldGameVersion.Build != build;
						if (flag4)
						{
							result = (this._currWorldGameVersion.Build > build);
						}
						else
						{
							bool flag5 = this._currWorldGameVersion.Revision != revision;
							result = (flag5 && this._currWorldGameVersion.Revision > revision);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x000E02A0 File Offset: 0x000DE4A0
		public bool IsCurrWorldSavedWithVersion(int major, int minor, int build, int revision)
		{
			bool flag = this._currWorldGameVersion == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._currWorldGameVersion.Major != major;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = this._currWorldGameVersion.Minor != minor;
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = this._currWorldGameVersion.Build != build;
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool flag5 = this._currWorldGameVersion.Revision != revision;
							result = !flag5;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x000E0330 File Offset: 0x000DE530
		public int RegisterCustomText(DataContext context, string text)
		{
			bool flag = text == null;
			if (flag)
			{
				throw new Exception("Text can not be null");
			}
			int id = this.GenerateNextCustomTextId(context);
			this.AddElement_CustomTexts(id, text, context);
			return id;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x000E0368 File Offset: 0x000DE568
		public void UnregisterCustomText(DataContext context, int id)
		{
			this.RemoveElement_CustomTexts(id, context);
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x000E0374 File Offset: 0x000DE574
		public IReadOnlyDictionary<int, string> GetCustomTexts()
		{
			return this._customTexts;
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x000E038C File Offset: 0x000DE58C
		[DomainMethod]
		public List<Location> GetJuniorXiangshuLocations()
		{
			List<Location> locations = new List<Location>();
			foreach (XiangshuAvatarTaskStatus xiangshuTaskStatus in this._xiangshuAvatarTaskStatuses)
			{
				bool flag = xiangshuTaskStatus.JuniorXiangshuCharId < 0;
				if (flag)
				{
					locations.Add(Location.Invalid);
				}
				else
				{
					Location location = DomainManager.Character.GetElement_Objects(xiangshuTaskStatus.JuniorXiangshuCharId).GetLocation();
					locations.Add(location);
				}
			}
			return locations;
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x000E0408 File Offset: 0x000DE608
		public void ChangeXiangshuAvatarFavorability(DataContext context, sbyte xiangshuAvatarId, int delta)
		{
			XiangshuAvatarTaskStatus avatarStatus = this._xiangshuAvatarTaskStatuses[(int)xiangshuAvatarId];
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			GameData.Domains.Character.Character xiangshuAvatarChar = DomainManager.Character.GetElement_Objects(avatarStatus.JuniorXiangshuCharId);
			DomainManager.Character.DirectlyChangeFavorabilityOptional(context, xiangshuAvatarChar, taiwuChar, delta, 4);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x000E0450 File Offset: 0x000DE650
		public short GetXiangshuAvatarFavorability(sbyte xiangshuAvatarId)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			XiangshuAvatarTaskStatus avatarStatus = this._xiangshuAvatarTaskStatuses[(int)xiangshuAvatarId];
			return DomainManager.Character.GetFavorability(avatarStatus.JuniorXiangshuCharId, taiwuCharId);
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x000E048C File Offset: 0x000DE68C
		public sbyte GetXiangshuAvatarFavorabilityType(sbyte xiangshuAvatarId)
		{
			return FavorabilityType.GetFavorabilityType(DomainManager.World.GetXiangshuAvatarFavorability(xiangshuAvatarId));
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x000E04B0 File Offset: 0x000DE6B0
		public void TransferXiangshuAvatarRelations(DataContext context, int oldTaiwuCharId, int newTaiwuCharId)
		{
			for (int avatarId = 0; avatarId < 9; avatarId++)
			{
				XiangshuAvatarTaskStatus xiangshuAvatarTaskStatus = this._xiangshuAvatarTaskStatuses[avatarId];
				int avatarCharId = xiangshuAvatarTaskStatus.JuniorXiangshuCharId;
				GameData.Domains.Character.Character character;
				bool flag = !DomainManager.Character.TryGetElement_Objects(avatarCharId, out character);
				if (!flag)
				{
					RelatedCharacter taiwuToTarget = DomainManager.Character.GetRelation(oldTaiwuCharId, avatarCharId);
					RelatedCharacter targetToTaiwu = DomainManager.Character.GetRelation(avatarCharId, oldTaiwuCharId);
					DomainManager.Character.DirectlySetFavorabilities(context, newTaiwuCharId, avatarCharId, taiwuToTarget.Favorability, targetToTaiwu.Favorability);
					this.SetElement_XiangshuAvatarTaskStatuses(avatarId, xiangshuAvatarTaskStatus, context);
				}
			}
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x000E0540 File Offset: 0x000DE740
		public string GetWorldDateKey()
		{
			int currYear = (int)(this.GetCurrYear() + 1);
			int currMonth = (int)(this.GetCurrMonthInYear() + 1);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this._worldId);
			defaultInterpolatedStringHandler.AppendLiteral("_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(currYear);
			defaultInterpolatedStringHandler.AppendLiteral("_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(currMonth);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x000E05B0 File Offset: 0x000DE7B0
		private int GenerateNextCustomTextId(DataContext context)
		{
			int customTextId = this._nextCustomTextId;
			this._nextCustomTextId++;
			bool flag = this._nextCustomTextId > int.MaxValue;
			if (flag)
			{
				this._nextCustomTextId = 0;
			}
			this.SetNextCustomTextId(this._nextCustomTextId, context);
			return customTextId;
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x000E05FE File Offset: 0x000DE7FE
		public override void PackCrossArchiveGameData(CrossArchiveGameData crossArchiveGameData)
		{
			crossArchiveGameData.CustomTexts = this._customTexts;
			crossArchiveGameData.NextCustomTextId = this._nextCustomTextId;
			crossArchiveGameData.FinalDateBeforeDreamBack = this._currDate;
			crossArchiveGameData.WorldCreationInfo = this.GetWorldCreationInfo();
			crossArchiveGameData.WorldId = this._worldId;
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x000E0640 File Offset: 0x000DE840
		public override void UnpackCrossArchiveGameData(DataContext context, CrossArchiveGameData crossArchiveGameData)
		{
			foreach (KeyValuePair<int, string> pair in crossArchiveGameData.CustomTexts)
			{
				bool flag = this._customTexts.ContainsKey(pair.Key);
				if (flag)
				{
					this.SetElement_CustomTexts(pair.Key, pair.Value, context);
				}
				else
				{
					this.AddElement_CustomTexts(pair.Key, pair.Value, context);
				}
			}
			this.SetNextCustomTextId(Math.Max(crossArchiveGameData.NextCustomTextId, this._nextCustomTextId), context);
			this.SetWorldCreationInfo(context, crossArchiveGameData.WorldCreationInfo, false);
			bool flag2 = crossArchiveGameData.WorldId != this._worldId || !DomainManager.TaiwuEvent.GetGlobalEventArgumentBox().GetBool("IsDreamBackArchive");
			if (flag2)
			{
				PredefinedLog.Show(16);
			}
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x000E0734 File Offset: 0x000DE934
		[DomainMethod]
		public bool GmCmd_SectEmeiAddSkillBreakBonus(DataContext context, short combatSkillId, short bonusTypeTemplateId)
		{
			return DomainManager.Extra.AddEmeiSkillBreakBonusWithoutCost(context, combatSkillId, bonusTypeTemplateId);
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x000E0754 File Offset: 0x000DE954
		[DomainMethod]
		public bool GmCmd_SectEmeiClearSkillBreakBonus(DataContext context, short combatSkillId)
		{
			return DomainManager.Extra.ClearEmeiSkillBreakBonus(context, combatSkillId);
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x000E0774 File Offset: 0x000DE974
		public InstantNotificationCollection GetInstantNotificationCollection()
		{
			return this._instantNotifications;
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x000E078C File Offset: 0x000DE98C
		public void CommitInstantNotifications(DataContext context)
		{
			int deltaSize = this._instantNotifications.Size - this._instantNotificationsCommittedOffset;
			bool flag = deltaSize <= 0;
			if (!flag)
			{
				this.CommitInsert_InstantNotifications(context, this._instantNotificationsCommittedOffset, deltaSize);
				this.CommitSetMetadata_InstantNotifications(context);
				this._instantNotificationsCommittedOffset = this._instantNotifications.Size;
			}
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x000E07E4 File Offset: 0x000DE9E4
		private void RemoveObsoletedInstantNotifications(DataContext context)
		{
			int deletedSize = this.RemoveObsoletedInstantNotificationsInternal(context);
			this._instantNotificationsCommittedOffset -= deletedSize;
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x000E0808 File Offset: 0x000DEA08
		private unsafe int RemoveObsoletedInstantNotificationsInternal(DataContext context)
		{
			int thresholdDate = this.GetCurrDate() - 12;
			int index = -1;
			int offset = -1;
			bool foundDemarcationPoint = false;
			byte[] array;
			byte* pRawData;
			if ((array = this._instantNotifications.RawData) == null || array.Length == 0)
			{
				pRawData = null;
			}
			else
			{
				pRawData = &array[0];
			}
			while (this._instantNotifications.Next(ref index, ref offset))
			{
				byte* pCurrData = pRawData + offset;
				int currDate = *(int*)(pCurrData + 1);
				bool flag = currDate >= thresholdDate;
				if (flag)
				{
					foundDemarcationPoint = true;
					break;
				}
			}
			array = null;
			bool flag2 = foundDemarcationPoint;
			int result;
			if (flag2)
			{
				bool flag3 = index <= 0;
				if (flag3)
				{
					result = 0;
				}
				else
				{
					this._instantNotifications.Remove(0, offset);
					this._instantNotifications.Count -= index;
					this.CommitRemove_InstantNotifications(context, 0, offset);
					this.CommitSetMetadata_InstantNotifications(context);
					result = offset;
				}
			}
			else
			{
				bool flag4 = this._instantNotifications.Count <= 0;
				if (flag4)
				{
					result = 0;
				}
				else
				{
					int size = this._instantNotifications.Size;
					this._instantNotifications.Remove(0, size);
					this._instantNotifications.Count = 0;
					this.CommitRemove_InstantNotifications(context, 0, size);
					this.CommitSetMetadata_InstantNotifications(context);
					result = size;
				}
			}
			return result;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x000E0940 File Offset: 0x000DEB40
		public void PrepareTestInstantNotificationRelatedData()
		{
			bool flag = this._instantNotificationTemplateIds.Count <= 0;
			if (flag)
			{
				this.InitializeTestInstantNotificationRelatedData();
			}
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x000E096C File Offset: 0x000DEB6C
		public void AddRandomInstantNotification(DataContext context, InstantNotificationCollection notifications)
		{
			int selectedIndex = context.Random.Next(this._instantNotificationTemplateIds.Count);
			short templateId = this._instantNotificationTemplateIds[selectedIndex];
			InstantNotificationItem config = InstantNotification.Instance[templateId];
			string name = this._instantNotificationTemplateId2Name[config.TemplateId];
			MethodInfo methodInfo = this._instantNotificationCollectionType.GetMethod("Add" + name);
			Tester.Assert(methodInfo != null, "");
			List<object> arguments = new List<object>();
			GameData.Domains.Character.Character character = null;
			int i = 0;
			int count = config.Parameters.Length;
			while (i < count)
			{
				string paramName = config.Parameters[i];
				bool flag = string.IsNullOrEmpty(paramName);
				if (flag)
				{
					break;
				}
				sbyte paramType = ParameterType.Parse(paramName);
				this.AddNotificationArguments(context.Random, arguments, paramType, ref character);
				i++;
			}
			methodInfo.Invoke(notifications, arguments.ToArray());
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x000E0A5C File Offset: 0x000DEC5C
		private void InitializeTestInstantNotificationRelatedData()
		{
			this._instantNotificationTemplateIds.Clear();
			this._instantNotificationTemplateId2Name.Clear();
			Type defKeysType = Type.GetType("Config.InstantNotification+DefKey");
			Tester.Assert(defKeysType != null, "");
			FieldInfo[] defKeysFieldInfos = defKeysType.GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (FieldInfo info in defKeysFieldInfos)
			{
				string name = info.Name;
				short templateId = (short)info.GetValue(null);
				this._instantNotificationTemplateIds.Add(templateId);
				this._instantNotificationTemplateId2Name.Add(templateId, name);
			}
			this._instantNotificationCollectionType = Type.GetType("GameData.Domains.World.Notification.InstantNotificationCollection");
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x000E0B08 File Offset: 0x000DED08
		public void CheckMonthlyEvents(DataContext context)
		{
			this.AddTestMonthlyEvent(context);
			bool isTaiwuVillageDestroyed = this._isTaiwuVillageDestroyed;
			if (isTaiwuVillageDestroyed)
			{
				this._monthlyEventCollection.Clear();
				this._monthlyEventCollection.AddTaiwuVillageBeDestoryed();
				this.<CheckMonthlyEvents>g__ResetFlag|75_0();
				WorldDomain.Logger.Info("CheckMonthlyEvents: TaiwuVillageBeDestroyed.");
			}
			else
			{
				bool flag = this._mainStoryLineProgress == 6 && EventHelper.GlobalArgBoxContainsKey<Location>("WangliuLocation");
				if (flag)
				{
					Location wangliuLocation = EventHelper.GetObjectFromGlobalArgBox<Location>("WangliuLocation");
					List<Location> outLocations = EventHelper.GetMapBlocksNearLocation(wangliuLocation, 3);
					List<Location> innerLocations = EventHelper.GetMapBlocksNearLocation(wangliuLocation, 2);
					outLocations.RemoveAll((Location e) => innerLocations.Contains(e));
					bool isAllRuin = true;
					foreach (Location eLocation in outLocations)
					{
						MapBlockData block = DomainManager.Map.GetBlock(eLocation);
						bool flag2 = block.BlockSubType != EMapBlockSubType.Ruin;
						if (flag2)
						{
							isAllRuin = false;
							break;
						}
					}
					bool flag3 = isAllRuin;
					if (flag3)
					{
						this._monthlyEventCollection.Clear();
						this._monthlyEventCollection.AddAreaTotallyDestoryed(DomainManager.Taiwu.GetTaiwuCharId());
						WorldDomain.Logger.Info("CheckMonthlyEvents: AreaTotallyDestoryed.");
						return;
					}
				}
				bool flag4 = this._isTaiwuDying || this._isTaiwuGettingCompletelyInfected;
				if (flag4)
				{
					this._monthlyEventCollection.Clear();
					GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
					bool isTaiwuDying = this._isTaiwuDying;
					if (isTaiwuDying)
					{
						this._monthlyEventCollection.AddTaiwuDeath(taiwuChar.GetId(), taiwuChar.GetLocation());
					}
					else
					{
						this._monthlyEventCollection.AddTaiwuInfected(taiwuChar.GetId(), taiwuChar.GetLocation());
					}
					this.<CheckMonthlyEvents>g__ResetFlag|75_0();
					Events.RaisePassingLegacyWhileAdvancingMonth(context);
					WorldDomain.Logger.Info("CheckMonthlyEvents: LegacyPassing.");
				}
				else
				{
					KidnappedTravelData kidnappedData = DomainManager.Extra.GetKidnappedTravelData();
					bool valid = kidnappedData.Valid;
					if (valid)
					{
						this._monthlyEventCollection.Clear();
						bool isTaiwuHunterDie = this.IsTaiwuHunterDie;
						if (isTaiwuHunterDie)
						{
							this._monthlyEventCollection.AddTaiwuBeHuntedHunterDie(kidnappedData.HunterCharId, DomainManager.Taiwu.GetTaiwuCharId());
							this.<CheckMonthlyEvents>g__ResetFlag|75_0();
						}
					}
					else
					{
						DomainManager.Adventure.CheckRandomEnemyAttackTaiwuOnAdvanceMonth();
						DomainManager.Extra.CheckAnimalAttackTaiwuOnAdvanceMonth(context.Random);
						this.UpdateMonthlyEventToRepayKindness(context);
						this.CheckWorldStateMonthlyEvents();
						this.CheckTaskMonthlyEvents();
						this.RemoveAllInvalidMonthlyEvents(context);
						this.TrimSpecialMonthlyEvents(context);
						Logger logger = WorldDomain.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 2);
						defaultInterpolatedStringHandler.AppendFormatted("CheckMonthlyEvents");
						defaultInterpolatedStringHandler.AppendLiteral(": ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(this._monthlyEventCollection.Count);
						defaultInterpolatedStringHandler.AppendLiteral(" events detected.");
						logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
			}
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x000E0DF0 File Offset: 0x000DEFF0
		[DomainMethod]
		public void HandleMonthlyEvent(DataContext context, int offset)
		{
			short recordType = this._monthlyEventCollection.GetRecordType(offset);
			MonthlyEventItem configData = MonthlyEvent.Instance[recordType];
			int size = this._monthlyEventCollection.GetRecordSize(offset);
			bool flag = recordType == 1;
			if (flag)
			{
				EventHelper.TriggerLegacyPassingEvent(true, string.Empty);
				DomainManager.TaiwuEvent.SetEventInProcessing(configData.Event);
			}
			else
			{
				bool flag2 = recordType == 3;
				if (flag2)
				{
					EventHelper.TriggerLegacyPassingEvent(false, string.Empty);
					DomainManager.TaiwuEvent.SetEventInProcessing(configData.Event);
				}
				else
				{
					bool flag3 = !string.IsNullOrEmpty(configData.Event);
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					if (flag3)
					{
						TaiwuEvent taiwuEvent = DomainManager.TaiwuEvent.GetEvent(configData.Event);
						bool flag4 = taiwuEvent != null;
						if (flag4)
						{
							bool flag5 = taiwuEvent.ArgBox == null;
							if (flag5)
							{
								taiwuEvent.ArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
							}
							this._monthlyEventCollection.FillEventArgBox(offset, taiwuEvent.ArgBox);
							bool flag6 = !taiwuEvent.EventConfig.CheckCondition();
							if (flag6)
							{
								defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 2);
								defaultInterpolatedStringHandler.AppendLiteral("monthly event ");
								defaultInterpolatedStringHandler.AppendFormatted(configData.Name);
								defaultInterpolatedStringHandler.AppendLiteral(" is triggering ");
								defaultInterpolatedStringHandler.AppendFormatted(taiwuEvent.EventGuid);
								defaultInterpolatedStringHandler.AppendLiteral(" when OnCheckEventCondition return false.");
								throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
							DomainManager.TaiwuEvent.SetEventInProcessing(configData.Event);
						}
						else
						{
							Logger logger = WorldDomain.Logger;
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Monthly Event ");
							defaultInterpolatedStringHandler.AppendFormatted(configData.Name);
							defaultInterpolatedStringHandler.AppendLiteral(" (");
							defaultInterpolatedStringHandler.AppendFormatted(configData.Event);
							defaultInterpolatedStringHandler.AppendLiteral(") not found.");
							logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
					Logger logger2 = WorldDomain.Logger;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(74, 5);
					defaultInterpolatedStringHandler.AppendLiteral("Removing monthly event ");
					defaultInterpolatedStringHandler.AppendFormatted<short>(recordType);
					defaultInterpolatedStringHandler.AppendLiteral(" at ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(offset);
					defaultInterpolatedStringHandler.AppendLiteral(" of size ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(size);
					defaultInterpolatedStringHandler.AppendLiteral(" from a collection of ");
					defaultInterpolatedStringHandler.AppendLiteral("size ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this._monthlyEventCollection.Size);
					defaultInterpolatedStringHandler.AppendLiteral(" and count ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this._monthlyEventCollection.Count);
					logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear());
					bool flag7 = this._monthlyEventCollection.Size == 0;
					if (flag7)
					{
						WorldDomain.Logger.AppendWarning("MonthlyEventCollection is empty.");
					}
					else
					{
						this._monthlyEventCollection.Remove(offset, size);
						this._monthlyEventCollection.Count--;
					}
				}
			}
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x000E10D4 File Offset: 0x000DF2D4
		[DomainMethod]
		public MonthlyEventCollection GetMonthlyEventCollection()
		{
			return this._monthlyEventCollection;
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x000E10EC File Offset: 0x000DF2EC
		[DomainMethod]
		public void RemoveAllInvalidMonthlyEvents(DataContext context)
		{
			int index = -1;
			int offset = -1;
			List<int> toRemoveOffsets = ObjectPool<List<int>>.Instance.Get();
			toRemoveOffsets.Clear();
			EventArgBox argBox = DomainManager.TaiwuEvent.GetEventArgBox();
			while (this._monthlyEventCollection.Next(ref index, ref offset))
			{
				short recordType = this._monthlyEventCollection.GetRecordType(offset);
				MonthlyEventItem monthlyEventCfg = MonthlyEvent.Instance[recordType];
				bool flag = string.IsNullOrEmpty(monthlyEventCfg.Event);
				if (flag)
				{
					bool flag2 = monthlyEventCfg.TemplateId == 0;
					if (flag2)
					{
						DomainManager.Taiwu.CheckNotInInventoryBooks(context);
						bool flag3 = !DomainManager.Taiwu.GetCurReadingBook().IsValid();
						if (flag3)
						{
							toRemoveOffsets.Add(offset);
						}
					}
				}
				else
				{
					TaiwuEvent taiwuEvent = DomainManager.TaiwuEvent.GetEvent(monthlyEventCfg.Event);
					bool flag4 = taiwuEvent == null;
					if (!flag4)
					{
						this._monthlyEventCollection.FillEventArgBox(offset, argBox);
						taiwuEvent.ArgBox = argBox;
						bool flag5 = !taiwuEvent.EventConfig.CheckCondition();
						if (flag5)
						{
							toRemoveOffsets.Add(offset);
						}
						taiwuEvent.ArgBox = null;
					}
				}
			}
			for (int i = toRemoveOffsets.Count - 1; i >= 0; i--)
			{
				int toRemoveOffset = toRemoveOffsets[i];
				int recordSize = this._monthlyEventCollection.GetRecordSize(toRemoveOffset);
				Logger logger = WorldDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(74, 5);
				defaultInterpolatedStringHandler.AppendLiteral("Removing monthly event ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(recordSize);
				defaultInterpolatedStringHandler.AppendLiteral(" at ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(toRemoveOffset);
				defaultInterpolatedStringHandler.AppendLiteral(" of size ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(recordSize);
				defaultInterpolatedStringHandler.AppendLiteral(" from a collection of ");
				defaultInterpolatedStringHandler.AppendLiteral("size ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._monthlyEventCollection.Size);
				defaultInterpolatedStringHandler.AppendLiteral(" and count ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._monthlyEventCollection.Count);
				logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				this._monthlyEventCollection.Remove(toRemoveOffset, recordSize);
				this._monthlyEventCollection.Count--;
			}
			DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			ObjectPool<List<int>>.Instance.Return(toRemoveOffsets);
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x000E1334 File Offset: 0x000DF534
		private void TrimSpecialMonthlyEvents(DataContext context)
		{
			int availableScore = 15;
			int index = -1;
			int offset = -1;
			WorldDomain.SpecialEvents.Clear();
			while (this._monthlyEventCollection.Next(ref index, ref offset))
			{
				short recordType = this._monthlyEventCollection.GetRecordType(offset);
				MonthlyEventItem cfg = MonthlyEvent.Instance[recordType];
				bool flag = cfg.Type != EMonthlyEventType.SpecialEvent;
				if (!flag)
				{
					WorldDomain.SpecialEvents.Push(new ValueTuple<int, int>(offset, cfg.Score));
				}
			}
			List<int> toRemoveOffsets = context.AdvanceMonthRelatedData.IntList.Occupy();
			while (WorldDomain.SpecialEvents.Count > 0)
			{
				ValueTuple<int, int> item = WorldDomain.SpecialEvents.Pop();
				bool flag2 = availableScore < item.Item2;
				if (flag2)
				{
					toRemoveOffsets.Add(item.Item1);
				}
				else
				{
					availableScore -= item.Item2;
				}
			}
			bool flag3 = toRemoveOffsets.Count == 0;
			if (flag3)
			{
				context.AdvanceMonthRelatedData.IntList.Release(ref toRemoveOffsets);
			}
			else
			{
				toRemoveOffsets.Sort();
				for (int i = toRemoveOffsets.Count - 1; i >= 0; i--)
				{
					int toRemoveOffset = toRemoveOffsets[i];
					int recordSize = this._monthlyEventCollection.GetRecordSize(toRemoveOffset);
					Logger logger = WorldDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(76, 6);
					defaultInterpolatedStringHandler.AppendFormatted("TrimSpecialMonthlyEvents");
					defaultInterpolatedStringHandler.AppendLiteral(": Removing monthly event ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(recordSize);
					defaultInterpolatedStringHandler.AppendLiteral(" at ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(toRemoveOffset);
					defaultInterpolatedStringHandler.AppendLiteral(" of size ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(recordSize);
					defaultInterpolatedStringHandler.AppendLiteral(" from a collection of size ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this._monthlyEventCollection.Size);
					defaultInterpolatedStringHandler.AppendLiteral(" and count ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this._monthlyEventCollection.Count);
					logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
					this._monthlyEventCollection.Remove(toRemoveOffset, recordSize);
					this._monthlyEventCollection.Count--;
				}
				context.AdvanceMonthRelatedData.IntList.Release(ref toRemoveOffsets);
			}
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x000E1568 File Offset: 0x000DF768
		public void ClearTrivialMonthlyEvents(DataContext context)
		{
			int index = -1;
			int offset = -1;
			EventArgBox argBox = DomainManager.TaiwuEvent.GetEventArgBox();
			sbyte behaviorType = DomainManager.Taiwu.GetTaiwu().GetBehaviorType();
			while (this._monthlyEventCollection.Next(ref index, ref offset))
			{
				short recordType = this._monthlyEventCollection.GetRecordType(offset);
				MonthlyEventItem cfg = MonthlyEvent.Instance[recordType];
				bool flag = cfg.Type == EMonthlyEventType.SpecialEvent;
				if (!flag)
				{
					bool flag2 = cfg.Type == EMonthlyEventType.LockedEvent;
					if (flag2)
					{
						WorldDomain.Logger.AppendWarning("Monthly Event " + cfg.Name + " is a locked event which cannot be cleared or handled with default option.");
					}
					else
					{
						bool flag3 = string.IsNullOrEmpty(cfg.Event);
						if (!flag3)
						{
							argBox.Clear();
							argBox.Set("DefaultHandleFlag", true);
							this._monthlyEventCollection.FillEventArgBox(offset, argBox);
							DomainManager.TaiwuEvent.ProcessEventWithDefaultOption(cfg.Event, argBox, behaviorType);
						}
					}
				}
			}
			DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			this._monthlyEventCollection.Clear();
			this.SetOnHandingMonthlyEventBlock(false, context);
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x000E1684 File Offset: 0x000DF884
		[DomainMethod]
		public void ProcessAllMonthlyEventsWithDefaultOption(DataContext context)
		{
			int index = -1;
			int offset = -1;
			EventArgBox argBox = DomainManager.TaiwuEvent.GetEventArgBox();
			sbyte behaviorType = DomainManager.Taiwu.GetTaiwu().GetBehaviorType();
			while (this._monthlyEventCollection.Next(ref index, ref offset))
			{
				short recordType = this._monthlyEventCollection.GetRecordType(offset);
				MonthlyEventItem cfg = MonthlyEvent.Instance[recordType];
				bool flag = cfg.Type == EMonthlyEventType.SpecialEvent || cfg.Type == EMonthlyEventType.LockedEvent;
				if (flag)
				{
					WorldDomain.Logger.AppendWarning(cfg.Name + " is a special event that cannot be handled with default option.");
				}
				else
				{
					bool flag2 = string.IsNullOrEmpty(cfg.Event);
					if (!flag2)
					{
						argBox.Clear();
						argBox.Set("DefaultHandleFlag", true);
						this._monthlyEventCollection.FillEventArgBox(offset, argBox);
						DomainManager.TaiwuEvent.ProcessEventWithDefaultOption(cfg.Event, argBox, behaviorType);
					}
				}
			}
			DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			this._monthlyEventCollection.Clear();
			this.SetOnHandingMonthlyEventBlock(false, context);
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x000E1794 File Offset: 0x000DF994
		public void EscapeDuringMonthlyEvent(DataContext context)
		{
			bool needToEscape = DomainManager.Taiwu.GetNeedToEscape();
			if (!needToEscape)
			{
				Tester.Assert(!this._isTaiwuDying && !this._isTaiwuGettingCompletelyInfected, "");
				Tester.Assert(this._advancingMonthState == 20, "");
				DomainManager.Taiwu.SetNeedToEscape(true, context);
				this.ClearTrivialMonthlyEvents(context);
				this._monthlyEventCollection.Clear();
			}
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x000E1808 File Offset: 0x000DFA08
		public void SetTaiwuDying(bool isTaiwuDyingOfDystocia = false)
		{
			bool isTaiwuDying = this._isTaiwuDying;
			if (!isTaiwuDying)
			{
				this._isTaiwuDying = true;
				this._isTaiwuDyingOfDystocia = isTaiwuDyingOfDystocia;
			}
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x000E1830 File Offset: 0x000DFA30
		public void SetTaiwuGettingCompletelyInfected()
		{
			this._isTaiwuGettingCompletelyInfected = true;
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x000E183A File Offset: 0x000DFA3A
		public void SetTaiwuVillageDestroyed()
		{
			this._isTaiwuVillageDestroyed = true;
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x000E1844 File Offset: 0x000DFA44
		public void SetToRepayKindnessCharId(int charId)
		{
			this._toRepayKindnessCharId = charId;
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x000E1850 File Offset: 0x000DFA50
		public void UpdateMonthlyEventToRepayKindness(DataContext context)
		{
			GameData.Domains.Character.Character character;
			bool flag = !GameData.Domains.Character.Character.IsCharacterIdValid(this._toRepayKindnessCharId) || !DomainManager.Character.TryGetElement_Objects(this._toRepayKindnessCharId, out character);
			if (!flag)
			{
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection.AddToRepayKindness(DomainManager.Taiwu.GetTaiwu().GetValidLocation());
			}
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x000E18AC File Offset: 0x000DFAAC
		public int GetToRepayKindnessCharId()
		{
			return this._toRepayKindnessCharId;
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x000E18C4 File Offset: 0x000DFAC4
		public bool ClearMonthlyEventCollectionNotEndGame()
		{
			return this._isTaiwuDying || this._isTaiwuGettingCompletelyInfected || DomainManager.Extra.GetKidnappedTravelData().Valid;
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x000E18F8 File Offset: 0x000DFAF8
		private void CheckTaskMonthlyEvents()
		{
			MonthlyEventCollection monthlyEventCollection = this.GetMonthlyEventCollection();
			List<TaskData> taskList = this.GetCurrTaskList();
			foreach (TaskData task in taskList)
			{
				TaskInfoItem taskInfo = TaskInfo.Instance[task.TaskInfoId];
				AutoTriggerMonthlyEvent[] monthlyEvents = taskInfo.MonthlyEvents;
				bool flag = monthlyEvents == null || monthlyEvents.Length <= 0;
				if (!flag)
				{
					sbyte orgTemplateId = TaskChain.Instance[task.TaskChainId].Sect;
					EventArgBox argBox = (orgTemplateId >= 0 && OrganizationDomain.IsSect(orgTemplateId)) ? DomainManager.Extra.GetSectMainStoryEventArgBox(orgTemplateId) : DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
					foreach (AutoTriggerMonthlyEvent autoMonthlyEvent in taskInfo.MonthlyEvents)
					{
						monthlyEventCollection.AddTaskMonthlyEvent(argBox, taskInfo, autoMonthlyEvent);
					}
				}
			}
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x000E19FC File Offset: 0x000DFBFC
		private unsafe void CheckWorldStateMonthlyEvents()
		{
			MonthlyEventCollection monthlyEventCollection = this.GetMonthlyEventCollection();
			WorldStateData worldStates = *this.GetWorldStateData();
			foreach (WorldStateItem worldStateCfg in ((IEnumerable<WorldStateItem>)WorldState.Instance))
			{
				short[] monthlyEvents = worldStateCfg.MonthlyEvents;
				bool flag = monthlyEvents == null || monthlyEvents.Length <= 0;
				if (!flag)
				{
					bool flag2 = !worldStates.GetWorldState((short)worldStateCfg.TemplateId);
					if (!flag2)
					{
						foreach (short monthlyEventId in worldStateCfg.MonthlyEvents)
						{
							monthlyEventCollection.AddWorldStateMonthlyEvent(worldStateCfg, MonthlyEvent.Instance[monthlyEventId]);
						}
					}
				}
			}
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x000E1ACC File Offset: 0x000DFCCC
		public MonthlyNotificationCollection GetMonthlyNotificationCollection()
		{
			return this._currMonthlyNotifications;
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x000E1AE4 File Offset: 0x000DFCE4
		private void CheckMonthlyNotifications()
		{
			MonthlyNotificationCollection monthlyNotifications = this.GetMonthlyNotificationCollection();
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			bool flag = this._mainStoryLineProgress == 12;
			if (flag)
			{
				int yirenId = EventHelper.GetIntFromGlobalArgBox("Yiren");
				GameData.Domains.Character.Character yiren;
				bool flag2 = yirenId >= 0 && DomainManager.Character.TryGetElement_Objects(yirenId, out yiren);
				if (flag2)
				{
					Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
					bool flag3 = !taiwuChar.GetLocation().Equals(taiwuVillageLocation) && yiren.GetLocation().IsValid();
					if (flag3)
					{
						monthlyNotifications.AddYirenAppearInTaiwuArea(taiwuChar.GetId());
					}
				}
			}
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x000E1B82 File Offset: 0x000DFD82
		private void TransferMonthlyNotifications(DataContext context)
		{
			DomainManager.Extra.UpdatePreviousMonthlyNotifications(context, ref this._currMonthlyNotifications);
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x000E1B98 File Offset: 0x000DFD98
		public void InitializeSortedMonthlyNotificationSortingGroups(Dictionary<int, NotificationSortingGroup> groups)
		{
			this._sortedMonthlyNotificationSortingGroups = new List<int>();
			foreach (int id in groups.Keys)
			{
				this._sortedMonthlyNotificationSortingGroups.Add(id);
			}
			this._sortedMonthlyNotificationSortingGroups.Sort(new Comparison<int>(this.CompareGroups));
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x000E1C1C File Offset: 0x000DFE1C
		public void SortMonthlyNotificationSortingGroups(DataContext context)
		{
			this._sortedMonthlyNotificationSortingGroups.Sort(new Comparison<int>(this.CompareGroups));
			this.SetSortedMonthlyNotificationSortingGroups(this._sortedMonthlyNotificationSortingGroups, context);
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x000E1C48 File Offset: 0x000DFE48
		private int CompareGroups(int groupId1, int groupId2)
		{
			NotificationSortingGroup group = DomainManager.Extra.GetElement_MonthlyNotificationSortingGroups(groupId1);
			NotificationSortingGroup group2 = DomainManager.Extra.GetElement_MonthlyNotificationSortingGroups(groupId2);
			bool flag = group.IsOnTop && !group2.IsOnTop;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = !group.IsOnTop && group2.IsOnTop;
				if (flag2)
				{
					result = 1;
				}
				else
				{
					result = ((group.Priority == group2.Priority) ? group.Id.CompareTo(group2.Id) : group.Priority.CompareTo(group2.Priority));
				}
			}
			return result;
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x000E1CDC File Offset: 0x000DFEDC
		public void PrepareTestMonthlyNotificationRelatedData()
		{
			bool flag = this._monthlyNotificationTemplateIds.Count <= 0;
			if (flag)
			{
				this.InitializeTestMonthlyNotificationRelatedData();
			}
			this._candidatesCharacters.Clear();
			DomainManager.Character.FindIntelligentCharacters((GameData.Domains.Character.Character _) => true, this._candidatesCharacters);
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x000E1D44 File Offset: 0x000DFF44
		public void AddRandomMonthlyNotification(DataContext context, MonthlyNotificationCollection notifications)
		{
			int selectedIndex = context.Random.Next(this._monthlyNotificationTemplateIds.Count);
			short templateId = this._monthlyNotificationTemplateIds[selectedIndex];
			MonthlyNotificationItem config = MonthlyNotification.Instance[templateId];
			string name = this._monthlyNotificationTemplateId2Name[config.TemplateId];
			MethodInfo methodInfo = this._monthlyNotificationCollectionType.GetMethod("Add" + name);
			Tester.Assert(methodInfo != null, "");
			List<object> arguments = new List<object>();
			GameData.Domains.Character.Character character = null;
			int i = 0;
			int count = config.Parameters.Length;
			while (i < count)
			{
				string paramName = config.Parameters[i];
				bool flag = string.IsNullOrEmpty(paramName);
				if (flag)
				{
					break;
				}
				sbyte paramType = ParameterType.Parse(paramName);
				this.AddNotificationArguments(context.Random, arguments, paramType, ref character);
				i++;
			}
			methodInfo.Invoke(notifications, arguments.ToArray());
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x000E1E34 File Offset: 0x000E0034
		private void InitializeTestMonthlyNotificationRelatedData()
		{
			this._monthlyNotificationTemplateIds.Clear();
			this._monthlyNotificationTemplateId2Name.Clear();
			Type defKeysType = Type.GetType("Config.MonthlyNotification+DefKey");
			Tester.Assert(defKeysType != null, "");
			FieldInfo[] defKeysFieldInfos = defKeysType.GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (FieldInfo info in defKeysFieldInfos)
			{
				string name = info.Name;
				short templateId = (short)info.GetValue(null);
				this._monthlyNotificationTemplateIds.Add(templateId);
				this._monthlyNotificationTemplateId2Name.Add(templateId, name);
			}
			this._monthlyNotificationCollectionType = Type.GetType("GameData.Domains.World.Notification.MonthlyNotificationCollection");
			this._candidateItems.Clear();
			WorldDomain.InitializeCandidateItems(this._candidateItems);
			this._candidateCombatSkills.Clear();
			foreach (CombatSkillItem item in ((IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance))
			{
				this._candidateCombatSkills.Add(item.TemplateId);
			}
			this._candidateSettlements.Clear();
			WorldDomain.InitializeCandidateSettlements(this._candidateSettlements);
			this._candidateBuildings.Clear();
			foreach (BuildingBlockItem item2 in ((IEnumerable<BuildingBlockItem>)BuildingBlock.Instance))
			{
				this._candidateBuildings.Add(item2.TemplateId);
			}
			this._candidateAdventures.Clear();
			foreach (AdventureItem item3 in ((IEnumerable<AdventureItem>)Adventure.Instance))
			{
				this._candidateAdventures.Add(item3.TemplateId);
			}
			this._candidateCrickets.Clear();
			WorldDomain.InitializeCandidateCrickets(this._candidateCrickets);
			this._candidateChickens.Clear();
			foreach (ChickenItem item4 in ((IEnumerable<ChickenItem>)Chicken.Instance))
			{
				this._candidateChickens.Add(item4.TemplateId);
			}
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x000E2098 File Offset: 0x000E0298
		private static void InitializeCandidateItems(List<TemplateKey> items)
		{
			foreach (AccessoryItem item in ((IEnumerable<AccessoryItem>)Config.Accessory.Instance))
			{
				items.Add(new TemplateKey(item.ItemType, item.TemplateId));
			}
			foreach (ArmorItem item2 in ((IEnumerable<ArmorItem>)Config.Armor.Instance))
			{
				items.Add(new TemplateKey(item2.ItemType, item2.TemplateId));
			}
			foreach (CarrierItem item3 in ((IEnumerable<CarrierItem>)Config.Carrier.Instance))
			{
				items.Add(new TemplateKey(item3.ItemType, item3.TemplateId));
			}
			foreach (ClothingItem item4 in ((IEnumerable<ClothingItem>)Config.Clothing.Instance))
			{
				items.Add(new TemplateKey(item4.ItemType, item4.TemplateId));
			}
			foreach (CraftToolItem item5 in ((IEnumerable<CraftToolItem>)Config.CraftTool.Instance))
			{
				items.Add(new TemplateKey(item5.ItemType, item5.TemplateId));
			}
			foreach (CricketItem item6 in ((IEnumerable<CricketItem>)Config.Cricket.Instance))
			{
				items.Add(new TemplateKey(item6.ItemType, item6.TemplateId));
			}
			foreach (FoodItem item7 in ((IEnumerable<FoodItem>)Config.Food.Instance))
			{
				items.Add(new TemplateKey(item7.ItemType, item7.TemplateId));
			}
			foreach (MaterialItem item8 in ((IEnumerable<MaterialItem>)Config.Material.Instance))
			{
				items.Add(new TemplateKey(item8.ItemType, item8.TemplateId));
			}
			foreach (MedicineItem item9 in ((IEnumerable<MedicineItem>)Config.Medicine.Instance))
			{
				items.Add(new TemplateKey(item9.ItemType, item9.TemplateId));
			}
			foreach (MiscItem item10 in ((IEnumerable<MiscItem>)Config.Misc.Instance))
			{
				items.Add(new TemplateKey(item10.ItemType, item10.TemplateId));
			}
			foreach (SkillBookItem item11 in ((IEnumerable<SkillBookItem>)Config.SkillBook.Instance))
			{
				items.Add(new TemplateKey(item11.ItemType, item11.TemplateId));
			}
			foreach (TeaWineItem item12 in ((IEnumerable<TeaWineItem>)Config.TeaWine.Instance))
			{
				items.Add(new TemplateKey(item12.ItemType, item12.TemplateId));
			}
			foreach (WeaponItem item13 in ((IEnumerable<WeaponItem>)Config.Weapon.Instance))
			{
				items.Add(new TemplateKey(item13.ItemType, item13.TemplateId));
			}
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x000E24F8 File Offset: 0x000E06F8
		private static void InitializeCandidateSettlements(List<short> settlementIds)
		{
			Type classType = Type.GetType("GameData.Domains.Organization.OrganizationDomain");
			Tester.Assert(classType != null, "");
			FieldInfo fieldInfo = classType.GetField("_settlements", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			Tester.Assert(fieldInfo != null, "");
			Dictionary<short, Settlement> settlements = (Dictionary<short, Settlement>)fieldInfo.GetValue(DomainManager.Organization);
			Tester.Assert(settlements != null, "");
			foreach (KeyValuePair<short, Settlement> keyValuePair in settlements)
			{
				short num;
				Settlement settlement;
				keyValuePair.Deconstruct(out num, out settlement);
				short settlementId = num;
				settlementIds.Add(settlementId);
			}
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x000E25BC File Offset: 0x000E07BC
		private static void InitializeCandidateCrickets([TupleElementNames(new string[]
		{
			"colorId",
			"partId"
		})] List<ValueTuple<short, short>> crickets)
		{
			List<short> partIds = new List<short>();
			List<short> colorIds = new List<short>();
			foreach (CricketPartsItem item in ((IEnumerable<CricketPartsItem>)CricketParts.Instance))
			{
				ECricketPartsType type = item.Type;
				ECricketPartsType ecricketPartsType = type;
				ECricketPartsType ecricketPartsType2 = ecricketPartsType;
				if (ecricketPartsType2 > ECricketPartsType.RealColor)
				{
					if (ecricketPartsType2 != ECricketPartsType.Parts)
					{
						colorIds.Add(item.TemplateId);
					}
					else
					{
						partIds.Add(item.TemplateId);
					}
				}
				else
				{
					crickets.Add(new ValueTuple<short, short>(item.TemplateId, 0));
				}
			}
			foreach (short partId in partIds)
			{
				foreach (short colorId in colorIds)
				{
					crickets.Add(new ValueTuple<short, short>(colorId, partId));
				}
			}
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x000E26E8 File Offset: 0x000E08E8
		private void AddNotificationArguments(IRandomSource random, List<object> arguments, sbyte paramType, ref GameData.Domains.Character.Character character)
		{
			switch (paramType)
			{
			case 0:
			{
				bool flag = character == null;
				GameData.Domains.Character.Character selectedCharacter;
				if (flag)
				{
					selectedCharacter = this.SelectRandomCharacter(random, -1);
					character = selectedCharacter;
				}
				else
				{
					selectedCharacter = this.SelectRandomCharacter(random, character.GetId());
				}
				arguments.Add(selectedCharacter.GetId());
				return;
			}
			case 1:
			{
				bool flag2 = character == null;
				Location location;
				if (flag2)
				{
					short areaId = (short)random.Next(139);
					location = new Location(areaId, -1);
				}
				else
				{
					location = character.GetLocation();
					bool flag3 = !location.IsValid();
					if (flag3)
					{
						location = character.GetValidLocation();
					}
				}
				arguments.Add(location);
				return;
			}
			case 2:
			{
				int selectedIndex = random.Next(this._candidateItems.Count);
				TemplateKey selectedItem = this._candidateItems[selectedIndex];
				arguments.Add(selectedItem.ItemType);
				arguments.Add(selectedItem.TemplateId);
				return;
			}
			case 3:
			{
				int selectedIndex2 = random.Next(this._candidateCombatSkills.Count);
				short selectedCombatSkillId = this._candidateCombatSkills[selectedIndex2];
				arguments.Add(selectedCombatSkillId);
				return;
			}
			case 4:
			{
				sbyte resourceType = (sbyte)random.Next(0, 8);
				arguments.Add(resourceType);
				return;
			}
			case 5:
			{
				int selectedIndex3 = random.Next(this._candidateSettlements.Count);
				short selectedSettlementId = this._candidateSettlements[selectedIndex3];
				arguments.Add(selectedSettlementId);
				return;
			}
			case 6:
			{
				Tester.Assert(character != null, "");
				OrganizationInfo orgInfo = character.GetOrganizationInfo();
				arguments.Add(orgInfo.OrgTemplateId);
				arguments.Add(orgInfo.Grade);
				arguments.Add(orgInfo.Principal);
				arguments.Add(character.GetGender());
				return;
			}
			case 7:
			{
				int selectedIndex4 = random.Next(this._candidateBuildings.Count);
				short selectedBuildingTemplateId = this._candidateBuildings[selectedIndex4];
				arguments.Add(selectedBuildingTemplateId);
				return;
			}
			case 8:
			{
				sbyte xiangshuAvatarId = (sbyte)random.Next(0, 9);
				arguments.Add(xiangshuAvatarId);
				return;
			}
			case 9:
			{
				sbyte xiangshuAvatarId2 = (sbyte)random.Next(0, 9);
				arguments.Add(xiangshuAvatarId2);
				return;
			}
			case 10:
			{
				int selectedIndex5 = random.Next(this._candidateAdventures.Count);
				short adventureTemplateId = this._candidateAdventures[selectedIndex5];
				arguments.Add(adventureTemplateId);
				return;
			}
			case 11:
			{
				sbyte behaviorType = (sbyte)random.Next(0, 5);
				arguments.Add(behaviorType);
				return;
			}
			case 12:
			{
				short favorability = (short)random.Next(-30000, 30001);
				sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
				arguments.Add(favorabilityType);
				return;
			}
			case 13:
			{
				int selectedIndex6 = random.Next(this._candidateCrickets.Count);
				ValueTuple<short, short> valueTuple = this._candidateCrickets[selectedIndex6];
				short colorId = valueTuple.Item1;
				short partId = valueTuple.Item2;
				arguments.Add(colorId);
				arguments.Add(partId);
				return;
			}
			case 14:
			{
				short itemSubType = ItemSubType.GetRandom(random);
				bool flag4 = !ItemSubType.IsHobbyType(itemSubType);
				if (flag4)
				{
					itemSubType = -1;
				}
				arguments.Add(itemSubType);
				return;
			}
			case 15:
			{
				int selectedIndex7 = random.Next(this._candidateChickens.Count);
				short chickenTemplateId = this._candidateChickens[selectedIndex7];
				arguments.Add(chickenTemplateId);
				return;
			}
			case 16:
			{
				short characterPropertyReferencedType = (short)random.Next(0, 112);
				arguments.Add(characterPropertyReferencedType);
				return;
			}
			case 17:
			{
				sbyte bodyPartType = (sbyte)random.Next(0, 7);
				arguments.Add(bodyPartType);
				return;
			}
			case 18:
			{
				sbyte injuryType = (sbyte)random.Next(0, 2);
				arguments.Add(injuryType);
				return;
			}
			case 19:
			{
				sbyte poisonType = (sbyte)random.Next(0, 6);
				arguments.Add(poisonType);
				return;
			}
			case 22:
			{
				int value = random.Next(1000);
				arguments.Add(value);
				return;
			}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Unsupported ParameterType: ");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(paramType);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x000E2BC0 File Offset: 0x000E0DC0
		private GameData.Domains.Character.Character SelectRandomCharacter(IRandomSource random, int exceptedCharId = -1)
		{
			for (int i = 0; i < 100; i++)
			{
				int selectedIndex = random.Next(this._candidatesCharacters.Count);
				GameData.Domains.Character.Character selectedCharacter = this._candidatesCharacters[selectedIndex];
				bool flag = exceptedCharId < 0 || selectedCharacter.GetId() != exceptedCharId;
				if (flag)
				{
					return selectedCharacter;
				}
			}
			throw new Exception("Exceeded max retry count");
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x000E2C2C File Offset: 0x000E0E2C
		[DomainMethod]
		public bool GmCmd_AddMonthlyEvent(short startTemplateId, short endTemplateId, int selfCharId, int targetCharId)
		{
			bool success = false;
			bool flag = endTemplateId == -1;
			if (flag)
			{
				endTemplateId = startTemplateId;
			}
			for (short templateId = startTemplateId; templateId <= endTemplateId; templateId += 1)
			{
				bool flag2 = !this._canTestMonthlyEventTemplateIdList.Contains(startTemplateId);
				if (!flag2)
				{
					ValueTuple<short, int, int> tuple = new ValueTuple<short, int, int>(templateId, selfCharId, targetCharId);
					bool flag3 = !this._testMonthlyEventList.Contains(tuple);
					if (flag3)
					{
						this._testMonthlyEventList.Add(tuple);
					}
					CharacterDomain.AddLockMovementCharSet(selfCharId);
					success = true;
				}
			}
			return success;
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x000E2CB4 File Offset: 0x000E0EB4
		public void AddTestMonthlyEvent(DataContext context)
		{
			WorldDomain.<>c__DisplayClass123_0 CS$<>8__locals1 = new WorldDomain.<>c__DisplayClass123_0();
			CS$<>8__locals1.taiwuChar = DomainManager.Taiwu.GetTaiwu();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int taiwuCharId = CS$<>8__locals1.taiwuChar.GetId();
			Location location = CS$<>8__locals1.taiwuChar.GetValidLocation();
			byte pageTypes = CombatSkillStateHelper.GeneratePageTypesFromReadingState(context.Random, 0);
			byte internalIndex = 0;
			foreach (ValueTuple<short, int, int> valueTuple in this._testMonthlyEventList)
			{
				short eventTemplateId = valueTuple.Item1;
				int selfCharId = valueTuple.Item2;
				int targetCharId = valueTuple.Item3;
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(selfCharId);
				short num = eventTemplateId;
				short num2 = num;
				if (num2 <= 118)
				{
					switch (num2)
					{
					case 61:
						monthlyEventCollection.AddAskProtectByRevengeAttack(selfCharId, location, targetCharId, taiwuCharId);
						break;
					case 62:
					case 63:
					case 64:
					case 65:
					case 67:
					case 69:
					case 71:
						goto IL_BFD;
					case 66:
					{
						short itemTemplateId = Config.Medicine.Instance.First((MedicineItem m) => m.EffectType == EMedicineEffectType.RecoverOuterInjury).TemplateId;
						ItemKey itemKey = this.AddItem(context, itemTemplateId, 8, taiwuCharId, false);
						character.ChangeInjury(context, 0, false, 1);
						monthlyEventCollection.AddRequestHealOuterInjuryByItem(selfCharId, location, taiwuCharId, (ulong)itemKey, 0);
						break;
					}
					case 68:
					{
						short itemTemplateId = Config.Medicine.Instance.First((MedicineItem m) => m.EffectType == EMedicineEffectType.RecoverInnerInjury).TemplateId;
						ItemKey itemKey = this.AddItem(context, itemTemplateId, 8, taiwuCharId, false);
						character.ChangeInjury(context, 0, true, 1);
						monthlyEventCollection.AddRequestHealInnerInjuryByItem(selfCharId, location, taiwuCharId, (ulong)itemKey, 0);
						break;
					}
					case 70:
					{
						WorldDomain.<>c__DisplayClass123_0 CS$<>8__locals3 = CS$<>8__locals1;
						IEnumerable<PoisonItem> instance = Poison.Instance;
						Func<PoisonItem, bool> predicate;
						if ((predicate = CS$<>8__locals1.<>9__2) == null)
						{
							predicate = (CS$<>8__locals1.<>9__2 = ((PoisonItem p) => !CS$<>8__locals1.taiwuChar.HasPoisonImmunity(p.TemplateId)));
						}
						CS$<>8__locals3.poisonConfig = instance.FirstOrDefault(predicate);
						IEnumerable<MedicineItem> instance2 = Config.Medicine.Instance;
						Func<MedicineItem, bool> predicate2;
						if ((predicate2 = CS$<>8__locals1.<>9__3) == null)
						{
							predicate2 = (CS$<>8__locals1.<>9__3 = ((MedicineItem m) => m.EffectType == EMedicineEffectType.DetoxPoison && m.DetoxPoisonType == CS$<>8__locals1.poisonConfig.TemplateId));
						}
						short itemTemplateId = instance2.First(predicate2).TemplateId;
						ItemKey itemKey = this.AddItem(context, itemTemplateId, 8, taiwuCharId, false);
						character.ChangePoisoned(context, CS$<>8__locals1.poisonConfig.TemplateId, 0, 100);
						monthlyEventCollection.AddRequestHealPoisonByItem(selfCharId, location, taiwuCharId, (ulong)itemKey, 0);
						break;
					}
					case 72:
					{
						short itemTemplateId = Config.Medicine.Instance.First((MedicineItem m) => m.EffectType == EMedicineEffectType.RecoverHealth).TemplateId;
						ItemKey itemKey = this.AddItem(context, itemTemplateId, 8, taiwuCharId, false);
						character.ChangeHealth(context, -10);
						monthlyEventCollection.AddRequestHealth(selfCharId, location, taiwuCharId, (ulong)itemKey);
						break;
					}
					case 73:
					{
						short itemTemplateId = Config.Medicine.Instance.First((MedicineItem m) => m.EffectType == EMedicineEffectType.ChangeDisorderOfQi).TemplateId;
						ItemKey itemKey = this.AddItem(context, itemTemplateId, 8, taiwuCharId, false);
						character.ChangeDisorderOfQi(context, 1000);
						monthlyEventCollection.AddRequestHealDisorderOfQi(selfCharId, location, taiwuCharId, (ulong)itemKey);
						break;
					}
					case 74:
					{
						short itemTemplateId = Config.Misc.Instance.First((MiscItem m) => m.Neili > 0).TemplateId;
						ItemKey itemKey = this.AddItem(context, itemTemplateId, 12, taiwuCharId, false);
						character.ChangeCurrNeili(context, -Math.Min(character.GetMaxNeili(), 100));
						monthlyEventCollection.AddRequestNeili(selfCharId, location, taiwuCharId, (ulong)itemKey);
						break;
					}
					case 75:
					{
						short itemTemplateId = Config.Medicine.Instance.First((MedicineItem m) => m.WugType == 0).TemplateId;
						ItemKey itemKey = this.AddItem(context, itemTemplateId, 8, taiwuCharId, false);
						ItemKey wugItemKey = this.AddWug(context, selfCharId);
						monthlyEventCollection.AddRequestKillWug(selfCharId, location, taiwuCharId, (ulong)itemKey, (ulong)wugItemKey);
						break;
					}
					case 76:
					{
						short itemTemplateId = Config.Food.Instance.First<FoodItem>().TemplateId;
						ItemKey itemKey = this.AddItem(context, itemTemplateId, 7, taiwuCharId, false);
						monthlyEventCollection.AddRequestFood(selfCharId, location, taiwuCharId, (ulong)itemKey, 1);
						break;
					}
					case 77:
					{
						short itemTemplateId = Config.TeaWine.Instance[1].TemplateId;
						ItemKey itemKey = this.AddItem(context, itemTemplateId, 9, taiwuCharId, false);
						monthlyEventCollection.AddRequestTeaWine(selfCharId, location, taiwuCharId, (ulong)itemKey);
						break;
					}
					case 78:
						monthlyEventCollection.AddRequestResource(selfCharId, location, taiwuCharId, 1, 0);
						break;
					case 79:
					{
						short itemTemplateId = 91;
						ItemKey itemKey = this.AddItem(context, itemTemplateId, 12, taiwuCharId, false);
						monthlyEventCollection.AddRequestItem(selfCharId, location, taiwuCharId, (ulong)itemKey, 1);
						break;
					}
					case 80:
					{
						short itemTemplateId = Config.Weapon.Instance.First((WeaponItem w) => w.Grade == 2 && w.ResourceType == 1).TemplateId;
						ItemKey itemKey = this.AddItem(context, itemTemplateId, 0, selfCharId, false);
						short itemTemplateId2 = Config.CraftTool.Instance.First((CraftToolItem w) => w.Grade == 2 && w.RequiredLifeSkillTypes.Contains(7)).TemplateId;
						ItemKey itemKey2 = this.AddItem(context, itemTemplateId2, 6, taiwuCharId, false);
						monthlyEventCollection.AddRequestRepairItem(selfCharId, location, taiwuCharId, (ulong)itemKey, (ulong)itemKey2, 1, 1);
						break;
					}
					case 81:
					{
						short itemTemplateId = Config.Weapon.Instance.First((WeaponItem w) => w.Grade == 2 && w.ResourceType == 2).TemplateId;
						ItemKey itemKey = this.AddItem(context, itemTemplateId, 0, selfCharId, true);
						short itemTemplateId2 = Config.Medicine.Instance.First((MedicineItem m) => m.EffectType == EMedicineEffectType.ApplyPoison).TemplateId;
						ItemKey itemKey2 = this.AddItem(context, itemTemplateId2, 8, taiwuCharId, false);
						monthlyEventCollection.AddRequestAddPoisonToItem(selfCharId, location, taiwuCharId, (ulong)itemKey, (ulong)itemKey2);
						break;
					}
					case 82:
					{
						SkillBookItem skillBookConfig = Config.SkillBook.Instance.FirstOrDefault((SkillBookItem s) => s.LifeSkillType > 0 && character.FindLearnedLifeSkillIndex(s.LifeSkillTemplateId) < 0);
						bool flag = skillBookConfig == null;
						if (flag)
						{
							Logger logger = WorldDomain.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
							defaultInterpolatedStringHandler.AppendLiteral("未找到");
							defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
							defaultInterpolatedStringHandler.AppendLiteral("未学过的技艺书籍");
							logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						else
						{
							ItemKey itemKey = this.AddItem(context, skillBookConfig.TemplateId, 10, selfCharId, false);
							monthlyEventCollection.AddRequestInstructionOnLifeSkill(selfCharId, location, taiwuCharId, itemKey.ItemType, itemKey.TemplateId, 1);
						}
						break;
					}
					case 83:
					{
						SkillBookItem skillBookConfig = Config.SkillBook.Instance.FirstOrDefault((SkillBookItem s) => s.CombatSkillType >= 0 && !character.GetLearnedCombatSkills().Contains(s.CombatSkillTemplateId));
						bool flag2 = skillBookConfig == null;
						if (flag2)
						{
							Logger logger2 = WorldDomain.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
							defaultInterpolatedStringHandler.AppendLiteral("未找到");
							defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
							defaultInterpolatedStringHandler.AppendLiteral("未学过的功法书籍");
							logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						else
						{
							ItemKey itemKey = this.AddItem(context, skillBookConfig.TemplateId, 10, selfCharId, false);
							monthlyEventCollection.AddRequestInstructionOnCombatSkill(selfCharId, location, taiwuCharId, itemKey.ItemType, itemKey.TemplateId, (int)(CombatSkillStateHelper.GetPageId(internalIndex) + 1), (int)internalIndex, (int)pageTypes);
						}
						break;
					}
					case 84:
					{
						SkillBookItem skillBookConfig = Config.SkillBook.Instance.FirstOrDefault((SkillBookItem s) => s.LifeSkillType >= 0 && character.FindLearnedLifeSkillIndex(s.LifeSkillTemplateId) < 0);
						bool flag3 = skillBookConfig == null;
						if (flag3)
						{
							Logger logger3 = WorldDomain.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
							defaultInterpolatedStringHandler.AppendLiteral("未找到");
							defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
							defaultInterpolatedStringHandler.AppendLiteral("未学过的技艺书籍");
							logger3.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						else
						{
							ItemKey itemKey = this.AddItem(context, skillBookConfig.TemplateId, 10, selfCharId, false);
							monthlyEventCollection.AddRequestInstructionOnReadingLifeSkill(selfCharId, location, taiwuCharId, (ulong)itemKey, 1);
						}
						break;
					}
					case 85:
					{
						SkillBookItem skillBookConfig = Config.SkillBook.Instance.FirstOrDefault((SkillBookItem s) => s.CombatSkillType >= 0 && !character.GetLearnedCombatSkills().Contains(s.CombatSkillTemplateId));
						bool flag4 = skillBookConfig == null;
						if (flag4)
						{
							Logger logger4 = WorldDomain.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
							defaultInterpolatedStringHandler.AppendLiteral("未找到");
							defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
							defaultInterpolatedStringHandler.AppendLiteral("未学过的功法书籍");
							logger4.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						else
						{
							ItemKey itemKey = this.AddItem(context, skillBookConfig.TemplateId, 10, selfCharId, false);
							monthlyEventCollection.AddRequestInstructionOnReadingCombatSkill(selfCharId, location, taiwuCharId, (ulong)itemKey, (int)(CombatSkillStateHelper.GetPageId(internalIndex) + 1), (int)internalIndex);
						}
						break;
					}
					case 86:
					{
						SkillBookItem skillBookConfig = Config.SkillBook.Instance.FirstOrDefault((SkillBookItem s) => s.CombatSkillType >= 0 && !character.GetLearnedCombatSkills().Contains(s.CombatSkillTemplateId));
						bool flag5 = skillBookConfig == null;
						if (flag5)
						{
							Logger logger5 = WorldDomain.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
							defaultInterpolatedStringHandler.AppendLiteral("未找到");
							defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
							defaultInterpolatedStringHandler.AppendLiteral("未学过的功法书籍");
							logger5.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						else
						{
							character.LearnNewCombatSkill(context, skillBookConfig.CombatSkillTemplateId, 32767);
							monthlyEventCollection.AddRequestInstructionOnBreakout(selfCharId, location, taiwuCharId, skillBookConfig.CombatSkillTemplateId);
						}
						break;
					}
					case 87:
						monthlyEventCollection.AddRequestPlayCombat(selfCharId, location, taiwuCharId);
						break;
					case 88:
						monthlyEventCollection.AddRequestNormalCombat(selfCharId, location, taiwuCharId);
						break;
					case 89:
						monthlyEventCollection.AddRequestLifeSkillBattle(selfCharId, location, taiwuCharId);
						break;
					case 90:
						monthlyEventCollection.AddRequestCricketBattle(selfCharId, location, taiwuCharId);
						break;
					default:
						switch (num2)
						{
						case 114:
							CS$<>8__locals1.taiwuChar.ChangeInjury(context, 0, true, 1);
							monthlyEventCollection.AddAdviseHealInjury(selfCharId, location, taiwuCharId, 1);
							break;
						case 115:
						{
							WorldDomain.<>c__DisplayClass123_0 CS$<>8__locals4 = CS$<>8__locals1;
							IEnumerable<PoisonItem> instance3 = Poison.Instance;
							Func<PoisonItem, bool> predicate3;
							if ((predicate3 = CS$<>8__locals1.<>9__17) == null)
							{
								predicate3 = (CS$<>8__locals1.<>9__17 = ((PoisonItem p) => !CS$<>8__locals1.taiwuChar.HasPoisonImmunity(p.TemplateId)));
							}
							CS$<>8__locals4.poisonConfig = instance3.FirstOrDefault(predicate3);
							CS$<>8__locals1.taiwuChar.ChangePoisoned(context, CS$<>8__locals1.poisonConfig.TemplateId, 0, 100);
							monthlyEventCollection.AddAdviseHealPoison(selfCharId, location, taiwuCharId, 1);
							break;
						}
						case 116:
						{
							short itemTemplateId = Config.Weapon.Instance.First((WeaponItem w) => w.Grade == 2 && w.ResourceType == 1).TemplateId;
							ItemKey itemKey = this.AddItem(context, itemTemplateId, 0, taiwuCharId, false);
							short itemTemplateId2 = Config.CraftTool.Instance.First((CraftToolItem w) => w.Grade == 2 && w.RequiredLifeSkillTypes.Contains(7)).TemplateId;
							ItemKey itemKey2 = this.AddItem(context, itemTemplateId2, 6, selfCharId, false);
							monthlyEventCollection.AddAdviseRepairItem(selfCharId, location, taiwuCharId, (ulong)itemKey, (ulong)itemKey2, 1, 1);
							break;
						}
						case 117:
							monthlyEventCollection.AddAdviseBarb(selfCharId, location, taiwuCharId);
							break;
						case 118:
							monthlyEventCollection.AddAskForMoney(selfCharId, location, taiwuCharId);
							break;
						default:
							goto IL_BFD;
						}
						break;
					}
				}
				else if (num2 != 290)
				{
					if (num2 != 353)
					{
						if (num2 != 354)
						{
							goto IL_BFD;
						}
						CS$<>8__locals1.taiwuChar.ChangeHealth(context, -10);
						monthlyEventCollection.AddAdviseHealHealth(selfCharId, location, taiwuCharId, 1);
					}
					else
					{
						CS$<>8__locals1.taiwuChar.ChangeDisorderOfQi(context, 1000);
						monthlyEventCollection.AddAdviseHealDisorderOfQi(selfCharId, location, taiwuCharId, 1);
					}
				}
				else
				{
					Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> combatSkills = DomainManager.CombatSkill.GetCharCombatSkills(selfCharId);
					IEnumerable<short> keys = combatSkills.Keys;
					Func<short, bool> predicate4;
					if ((predicate4 = CS$<>8__locals1.<>9__20) == null)
					{
						predicate4 = (CS$<>8__locals1.<>9__20 = ((short combatSkillTemplateId) => !CS$<>8__locals1.taiwuChar.GetLearnedCombatSkills().Contains(combatSkillTemplateId)));
					}
					short templateId = keys.FirstOrDefault(predicate4);
					bool flag6 = templateId < 0 || combatSkills.Keys.Count == 0;
					if (flag6)
					{
						WorldDomain.Logger.Warn("未找到可以指点的功法书籍");
					}
					else
					{
						monthlyEventCollection.AddTeachCombatSkill(selfCharId, location, taiwuCharId, templateId);
					}
				}
				continue;
				IL_BFD:
				MonthlyEventItem eventConfig = MonthlyEvent.Instance.GetItem(eventTemplateId);
				bool flag7 = eventConfig == null;
				if (flag7)
				{
					Logger logger6 = WorldDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
					defaultInterpolatedStringHandler.AppendLiteral("未找到ID为");
					defaultInterpolatedStringHandler.AppendFormatted<short>(eventTemplateId);
					defaultInterpolatedStringHandler.AppendLiteral("的过月事件");
					logger6.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else
				{
					WorldDomain.Logger.Warn("未添加过月事件 " + eventConfig.Name + " 的测试代码");
				}
			}
			this._testMonthlyEventList.Clear();
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x000E3984 File Offset: 0x000E1B84
		private ItemKey AddItem(DataContext context, short itemTemplateId, sbyte itemType, int charId, bool equip = false)
		{
			ItemKey itemKey = DomainManager.Item.CreateItem(context, itemType, itemTemplateId);
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			character.AddInventoryItem(context, itemKey, 1, false);
			bool flag = ItemType.IsEquipmentItemType(itemType);
			if (flag)
			{
				ItemBase itemBase = DomainManager.Item.GetBaseItem(itemKey);
				itemBase.SetCurrDurability(Convert.ToInt16((float)itemBase.GetMaxDurability() * 0.5f), context);
				if (equip)
				{
					ItemKey[] equipment = character.GetEquipment();
					bool flag2 = equipment[0].IsValid();
					if (flag2)
					{
						character.ChangeEquipment(context, 0, -1, equipment[0]);
					}
					character.ChangeEquipment(context, -1, 0, itemKey);
				}
			}
			return itemKey;
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x000E3A34 File Offset: 0x000E1C34
		private ItemKey AddWug(DataContext context, int charId)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			short medicineTemplateId = 347;
			ItemKey wugItemKey = new ItemKey(8, 0, medicineTemplateId, -1);
			character.AddWug(context, wugItemKey);
			return wugItemKey;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x000E3A70 File Offset: 0x000E1C70
		public float GetProbAdjustOfCreatingCharacter()
		{
			return this._probAdjustOfCreatingCharacter;
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x000E3A88 File Offset: 0x000E1C88
		public void RecordWorldStandardPopulation(DataContext context)
		{
			int worldPopulation = DomainManager.Character.GetWorldPopulation();
			this.SetWorldStandardPopulation(worldPopulation, context);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x000E3AAC File Offset: 0x000E1CAC
		public void ChangeWorldPopulation(DataContext context, byte oriWorldPopulationType)
		{
			int basicPopulation = this._worldStandardPopulation * 100 / WorldDomain.GetWorldPopulationFactor(oriWorldPopulationType);
			int currFactor = this.GetWorldPopulationFactor();
			int currStandardPopulation = basicPopulation * currFactor / 100;
			this.SetWorldStandardPopulation(currStandardPopulation, context);
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x000E3AE4 File Offset: 0x000E1CE4
		public void UpdatePopulationRelatedData()
		{
			int standardPopulation = this.GetWorldStandardPopulation();
			int currPopulation = DomainManager.Character.GetWorldPopulation();
			this._probAdjustOfCreatingCharacter = 1f;
			Logger logger = WorldDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(99, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Currrent Population / World Standard Population: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(currPopulation);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(standardPopulation);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendLiteral("Probability Adjustment of Creating Character: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>((int)Math.Round((double)(this._probAdjustOfCreatingCharacter * 100f)));
			defaultInterpolatedStringHandler.AppendLiteral("%");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x000E3B99 File Offset: 0x000E1D99
		private void AdvanceMonth_SectClearData(DataContext context)
		{
			this._sectMainStoryCombatTimesShaolin = 0;
			this._sectMainStoryDefeatingXuannv = false;
			this._sectMainStoryLifeLinkUpdated = false;
			Events.UnRegisterHandler_AdvanceMonthFinish(new Events.OnAdvanceMonthFinish(this.AdvanceMonth_SectClearData));
			this._storyStatus.Clear();
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x000E3BCF File Offset: 0x000E1DCF
		public int GetSectMainStoryCombatTimesShaolin()
		{
			return this._sectMainStoryCombatTimesShaolin;
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x000E3BD7 File Offset: 0x000E1DD7
		public bool GetSectMainStoryDefeatingXuannv()
		{
			return this._sectMainStoryDefeatingXuannv;
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x000E3BDF File Offset: 0x000E1DDF
		public void SetSectMainStoryDefeatingXuannv()
		{
			this._sectMainStoryDefeatingXuannv = true;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x000E3BE8 File Offset: 0x000E1DE8
		public bool CheckSectMainStoryAvailable(sbyte orgTemplateId)
		{
			bool flag = DomainManager.World.GetSectMainStoryTaskStatus(orgTemplateId) != 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				OrganizationItem organizationCfg = Organization.Instance[orgTemplateId];
				int[] taskChains = organizationCfg.TaskChains;
				bool flag2 = taskChains == null || taskChains.Length <= 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					for (int i = 0; i < organizationCfg.TaskChains.Length; i++)
					{
						bool flag3 = DomainManager.Extra.IsExtraTaskChainInProgress(organizationCfg.TaskChains[i]);
						if (flag3)
						{
							return false;
						}
					}
					EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(orgTemplateId);
					sbyte index = OrganizationDomain.GetLargeSectIndex(orgTemplateId);
					int date = int.MaxValue;
					bool flag4 = argBox.Get(SectMainStoryEventArgKeys.GoodEndDateKeys[(int)index], ref date) || argBox.Get(SectMainStoryEventArgKeys.BadEndDateKeys[(int)index], ref date);
					result = !flag4;
				}
			}
			return result;
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x000E3CD0 File Offset: 0x000E1ED0
		public void TriggerSectMainStoryEndingCountDown(DataContext context, sbyte orgTemplateId, bool isGoodEnding)
		{
			bool flag = this.GetSectMainStoryTaskStatus(orgTemplateId) != 0;
			if (flag)
			{
				WorldDomain.Logger.AppendWarning("Sect main story for " + Organization.Instance[orgTemplateId].Name + " is already finished.");
			}
			else
			{
				sbyte index = OrganizationDomain.GetLargeSectIndex(orgTemplateId);
				string argKey = isGoodEnding ? SectMainStoryEventArgKeys.GoodEndDateKeys[(int)index] : SectMainStoryEventArgKeys.BadEndDateKeys[(int)index];
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, orgTemplateId, argKey, this._currDate);
			}
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x000E3D48 File Offset: 0x000E1F48
		public sbyte GetSectMainStoryTaskStatus(sbyte orgTemplateId)
		{
			return this._stateTaskStatuses[(int)(orgTemplateId - 1)];
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x000E3D64 File Offset: 0x000E1F64
		public void SetSectMainStoryTaskStatus(DataContext context, sbyte orgTemplateId, sbyte status)
		{
			this.SetElement_StateTaskStatuses((int)(orgTemplateId - 1), status, context);
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x000E3D74 File Offset: 0x000E1F74
		public void StatSectMainStoryCombatTimes(short combatConfigTemplateId)
		{
			CombatConfigItem config = CombatConfig.Instance[combatConfigTemplateId];
			bool flag = DomainManager.Extra.IsExtraTaskChainInProgress(30);
			bool flag2 = flag;
			if (flag2)
			{
				sbyte combatType = config.CombatType;
				bool flag3 = combatType - 1 <= 1;
				flag2 = flag3;
			}
			bool flag4 = flag2;
			if (flag4)
			{
				this._sectMainStoryCombatTimesShaolin++;
			}
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x000E3DD0 File Offset: 0x000E1FD0
		private void AdvanceMonth_SectMainStory(DataContext context)
		{
			this.AdvanceMonth_SectMainStory_Kongsang(context);
			this.AdvanceMonth_SectMainStory_Xuehou(context);
			this.AdvanceMonth_SectMainStory_Shaolin(context);
			this.AdvanceMonth_SectMainStory_Xuannv(context);
			this.AdvanceMonth_SectMainStory_Wudang(context);
			this.AdvanceMonth_SectMainStory_Shixiang(context);
			this.AdvanceMonth_SectMainStory_Emei(context);
			this.AdvanceMonth_SectMainStory_Jingang(context);
			this.AdvanceMonth_SectMainStory_Wuxian(context);
			this.AdvanceMonth_SectMainStory_Ranshan(context);
			this.AdvanceMonth_SectMainStory_Baihua(context);
			this.AdvanceMonth_SectMainStory_Zhujian(context);
			this.AdvanceMonth_SectMainStoryFulong(context);
			this.UpdateBaihuaLifeLinkNeiliType(context);
			this.UpdateAreaMerchantType(context);
			DomainManager.Extra.UpdateThreeVitalsInfection(context);
			Events.RegisterHandler_AdvanceMonthFinish(new Events.OnAdvanceMonthFinish(this.AdvanceMonth_SectClearData));
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x000E3E74 File Offset: 0x000E2074
		[DomainMethod]
		public int GetSectMainStoryActiveStatus(sbyte orgTemplateId)
		{
			bool flag = !Common.IsInWorld();
			bool flag2 = flag;
			if (!flag2)
			{
				bool flag3 = orgTemplateId < 1 || orgTemplateId > 15;
				flag2 = flag3;
			}
			bool flag4 = flag2;
			int result;
			if (flag4)
			{
				result = int.MinValue;
			}
			else
			{
				int[] taskChains = Organization.Instance[orgTemplateId].TaskChains;
				bool flag5 = ((taskChains != null) ? taskChains.Length : 0) == 0;
				if (flag5)
				{
					result = 2;
				}
				else
				{
					bool avail = this.CheckSectMainStoryAvailable(orgTemplateId) && !this._storyStatus.Contains(orgTemplateId);
					bool flag6 = !avail;
					if (flag6)
					{
						result = 1;
					}
					else
					{
						EventArgBox box = EventHelper.GetSectMainStoryEventArgBox(orgTemplateId);
						result = (box.Contains<int>(SectMainStoryEventArgKeys.TriggeringStatus) ? -1 : 0);
					}
				}
			}
			return result;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x000E3F26 File Offset: 0x000E2126
		public bool SectMainStoryTriggeredThisMonth(sbyte orgTemplateId)
		{
			return this._storyStatus.Contains(orgTemplateId);
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x000E3F34 File Offset: 0x000E2134
		[DomainMethod]
		public void SetSectMainStoryActiveStatus(sbyte orgTemplateId, bool pause)
		{
			if (pause)
			{
				EventHelper.SaveArgToSectMainStory<int>(orgTemplateId, SectMainStoryEventArgKeys.TriggeringStatus, -1);
				bool flag = orgTemplateId == 2 && EventHelper.GetSectMainStoryEventArgBox(orgTemplateId).Contains<short>("ConchShip_PresetKey_WhiteApeBlockId");
				if (flag)
				{
					EventHelper.ClearWhiteApeBlockId();
				}
			}
			else
			{
				EventHelper.RemoveArgFromSectMainStory<int>(orgTemplateId, SectMainStoryEventArgKeys.TriggeringStatus);
				bool flag2 = orgTemplateId == 2 && DomainManager.Organization.GetSettlementByOrgTemplateId(2).CalcApprovingRate() >= 500;
				if (flag2)
				{
					EventHelper.SaveWhiteApeBlockId();
				}
			}
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x000E3FB2 File Offset: 0x000E21B2
		[DomainMethod]
		public void NotifySectStoryActivated(DataContext context, sbyte orgTemplateId)
		{
			this._storyStatus.Add(orgTemplateId);
			EventHelper.RemoveArgFromSectMainStory<int>(orgTemplateId, SectMainStoryEventArgKeys.TriggeringStatus);
			this.SetSectMainStoryTaskStatus(context, orgTemplateId, this.GetSectMainStoryTaskStatus(orgTemplateId));
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x000E3FE0 File Offset: 0x000E21E0
		private void AdvanceMonth_SectMainStory_Kongsang(DataContext context)
		{
			EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(10);
			int date = int.MaxValue;
			bool flag = argBox.Get("ConchShip_PresetKey_SectMainStoryKongsangProsperousEndDate", ref date) && this._currDate >= date + 1;
			if (flag)
			{
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection.AddSectMainStoryKongsangProsperous();
			}
			else
			{
				date = int.MaxValue;
				bool flag2 = argBox.Get("ConchShip_PresetKey_SectMainStoryKongsangFailingEndDate", ref date) && this._currDate >= date + 1;
				if (flag2)
				{
					MonthlyEventCollection monthlyEventCollection2 = DomainManager.World.GetMonthlyEventCollection();
					monthlyEventCollection2.AddSectMainStoryKongsangFailing();
				}
				else
				{
					GameData.Domains.Character.Character character;
					bool flag3 = !DomainManager.Character.TryGetFixedCharacterByTemplateId(668, out character);
					if (!flag3)
					{
						int taskInProgress = DomainManager.Extra.GetExtraTaskChainCurrentTask(27);
						int startDate = int.MaxValue;
						int num = taskInProgress;
						int num2 = num;
						switch (num2)
						{
						case 105:
							DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<bool>(context, 10, "ConchShip_PresetKey_MissionUnacceptedEventTriggeredSameMonth", true);
							return;
						case 106:
						case 109:
							break;
						case 107:
						case 108:
							return;
						default:
							if (num2 != 112)
							{
								if (num2 != 117)
								{
									return;
								}
								bool flag4 = !character.GetLocation().IsValid() && argBox.Get("ConchShip_PresetKey_KongsangAdventureCountDown", ref startDate) && startDate <= this._currDate;
								if (flag4)
								{
									MonthlyEventCollection monthlyEventCollection3 = DomainManager.World.GetMonthlyEventCollection();
									monthlyEventCollection3.AddSectMainStoryKongsangAdventure();
								}
								return;
							}
							break;
						}
						bool flag5 = !character.GetLocation().IsValid() && argBox.Get("ConchShip_PresetKey_LiaoWumingQuestStartDate", ref startDate) && startDate <= this._currDate;
						if (flag5)
						{
							MonthlyEventCollection monthlyEventCollection4 = DomainManager.World.GetMonthlyEventCollection();
							monthlyEventCollection4.AddSectMainStoryKongsangTargetFound();
						}
					}
				}
			}
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x000E41AC File Offset: 0x000E23AC
		private void AdvanceMonth_SectMainStory_Xuehou(DataContext context)
		{
			WorldDomain.<>c__DisplayClass150_0 CS$<>8__locals1 = new WorldDomain.<>c__DisplayClass150_0();
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.<>4__this = this;
			int xuehouTaskInProgress = DomainManager.Extra.GetExtraTaskChainCurrentTask(28);
			bool flag = xuehouTaskInProgress < 0;
			if (flag)
			{
				xuehouTaskInProgress = DomainManager.Extra.GetExtraTaskChainCurrentTask(29);
			}
			CS$<>8__locals1.argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(15);
			CS$<>8__locals1.monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			bool flag2 = this.IsJixiFree();
			if (flag2)
			{
				bool needTrigger = false;
				bool flag3 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_NeedTriggerPassLegacyMonthlyNotification", ref needTrigger);
				if (flag3)
				{
					CS$<>8__locals1.argBox.Set("ConchShip_PresetKey_NeedTriggerPassLegacyMonthlyNotification", false);
					CS$<>8__locals1.<AdvanceMonth_SectMainStory_Xuehou>g__TryTriggerXuehouJixiGone|0();
				}
			}
			bool flag4 = xuehouTaskInProgress < 0;
			if (flag4)
			{
				int date = int.MaxValue;
				bool flag5 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_SectMainStoryXuehouProsperousEndDate", ref date) && this._currDate >= date + 1;
				if (flag5)
				{
					CS$<>8__locals1.monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
					CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouProsperous();
				}
				else
				{
					date = int.MaxValue;
					bool flag6 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_SectMainStoryXuehouFailingEndDate", ref date) && this._currDate >= date + 1;
					if (flag6)
					{
						CS$<>8__locals1.monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
						CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouFailing();
					}
				}
			}
			else
			{
				CS$<>8__locals1.taiwu = DomainManager.Taiwu.GetTaiwu();
				CS$<>8__locals1.taiwuLocation = CS$<>8__locals1.taiwu.GetLocation();
				CS$<>8__locals1.taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
				CS$<>8__locals1.taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
				Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(15);
				Location settlementLocation = settlement.GetLocation();
				switch (xuehouTaskInProgress)
				{
				case 119:
				{
					bool areaHasGrave = this.AreaHasAdultGraveOfTargetOrganization(settlementLocation.AreaId, 15);
					bool flag7 = areaHasGrave;
					if (flag7)
					{
						int date2 = int.MaxValue;
						bool flag8 = !CS$<>8__locals1.argBox.Contains<bool>("ConchShip_PresetKey_XuehouGraveDiggingEventTriggered");
						if (flag8)
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouGraveDigging();
						}
						else
						{
							bool flag9 = !CS$<>8__locals1.argBox.Contains<int>("ConchShip_PresetKey_XuehouGraveDiggingNormalTriggerTime") || (CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_XuehouGraveDiggingNormalTriggerTime", ref date2) && date2 + 6 <= this._currDate);
							if (flag9)
							{
								CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouGraveDiggingNormal();
							}
						}
					}
					else
					{
						CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouStrangeDeath();
					}
					break;
				}
				case 120:
				{
					int startDate = int.MaxValue;
					CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_FirstGotBellTime", ref startDate);
					bool flag10 = CS$<>8__locals1.taiwuLocation.AreaId == settlementLocation.AreaId && startDate + 3 <= this._currDate;
					if (flag10)
					{
						CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouOldManAppears();
					}
					break;
				}
				case 121:
				{
					int defeatXuehouOldManTime = int.MaxValue;
					bool flag11 = !CS$<>8__locals1.argBox.GetBool("ConchShip_PresetKey_XuehouOldManGraveDisappearTriggered") && CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_DefeatXuehouOldManTime", ref defeatXuehouOldManTime) && defeatXuehouOldManTime + 3 <= this._currDate;
					if (flag11)
					{
						CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouOldManReturns();
					}
					else
					{
						int startDate2 = int.MaxValue;
						bool flag12 = CS$<>8__locals1.argBox.GetBool("ConchShip_PresetKey_XuehouOldManGraveDisappearTriggered") || (CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_GiveBellToXuehouOldManTime", ref startDate2) && startDate2 + 3 <= this._currDate);
						if (flag12)
						{
							GameData.Domains.Character.Character zombieOldMan = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(CS$<>8__locals1.context, 522);
							int zombieOldManId = zombieOldMan.GetId();
							Location zombieOldManLocation = zombieOldMan.GetLocation();
							CS$<>8__locals1.<AdvanceMonth_SectMainStory_Xuehou>g__DealXuehouOldManBell|2(zombieOldMan);
							DomainManager.Extra.TriggerExtraTask(CS$<>8__locals1.context, 28, 122);
							List<MapBlockData> blocks = CS$<>8__locals1.context.AdvanceMonthRelatedData.Blocks.Occupy();
							DomainManager.Map.GetSettlementAffiliatedBlocks(settlementLocation.AreaId, settlementLocation.BlockId, blocks);
							Location destLocation = blocks.GetRandom(CS$<>8__locals1.context.Random).GetLocation();
							CS$<>8__locals1.context.AdvanceMonthRelatedData.Blocks.Release(ref blocks);
							Events.RaiseFixedCharacterLocationChanged(CS$<>8__locals1.context, zombieOldManId, zombieOldManLocation, destLocation);
							zombieOldMan.SetLocation(destLocation, CS$<>8__locals1.context);
						}
					}
					break;
				}
				case 123:
				{
					List<Location> bloodLightLocationList = DomainManager.Extra.GetSectXuehouBloodLightLocations();
					bool flag13 = bloodLightLocationList.Count > 0 && DomainManager.Map.IsLocationInSettlementInfluenceRange(CS$<>8__locals1.taiwuLocation, settlement.GetId());
					if (flag13)
					{
						GameData.Domains.Character.Character zombieOldMan2 = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(CS$<>8__locals1.context, 522);
						CS$<>8__locals1.<AdvanceMonth_SectMainStory_Xuehou>g__DealXuehouOldManBell|2(zombieOldMan2);
						Location zombieOldManLocation2 = zombieOldMan2.GetLocation();
						bool sameBlockAsOldManInRed = CS$<>8__locals1.taiwuLocation.Equals(zombieOldManLocation2);
						bool isOnBloodBlock = bloodLightLocationList.Contains(CS$<>8__locals1.taiwuLocation);
						bool flag14 = sameBlockAsOldManInRed && isOnBloodBlock;
						if (flag14)
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouOnBloodBlock();
						}
						else
						{
							bool flag15 = sameBlockAsOldManInRed;
							if (flag15)
							{
								CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouOldManAttacks();
							}
						}
					}
					break;
				}
				case 125:
					CS$<>8__locals1.<AdvanceMonth_SectMainStory_Xuehou>g__DealMonthlyEvent|3();
					break;
				case 126:
				{
					CS$<>8__locals1.<AdvanceMonth_SectMainStory_Xuehou>g__DealMonthlyEvent|3();
					bool result = DomainManager.Map.IsLocationInSettlementInfluenceRange(CS$<>8__locals1.taiwuLocation, CS$<>8__locals1.taiwuVillageSettlementId);
					short villagerCount = DomainManager.Taiwu.GetTotalAdultVillagerCount();
					bool flag16 = result && villagerCount >= 3;
					if (flag16)
					{
						switch (CS$<>8__locals1.argBox.GetInt("ConchShip_PresetKey_JixiArrivedTaiwuMonthlyEventTriggeredCount"))
						{
						case 0:
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouHarmoniousTaiwu();
							break;
						case 1:
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouFeedJixi();
							break;
						case 2:
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouMythInVillage();
							break;
						}
					}
					break;
				}
				case 127:
				{
					int coolTime = -1;
					bool flag17 = !CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_XuehouComingTime", ref coolTime);
					if (flag17)
					{
						CS$<>8__locals1.argBox.Set("ConchShip_PresetKey_XuehouComingTime", 3);
					}
					else
					{
						coolTime++;
					}
					CS$<>8__locals1.argBox.Set("ConchShip_PresetKey_XuehouComingTime", coolTime);
					int triggeredCount = -1;
					CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_XuehouComingTriggeredCount", ref triggeredCount);
					bool flag18 = coolTime >= 3 && triggeredCount < (int)(this.GetJixiFavorabilityType() - 1);
					if (flag18)
					{
						CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouComing();
						CS$<>8__locals1.argBox.Set("ConchShip_PresetKey_XuehouComingTime", 0);
						bool flag19 = !CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_XuehouComingTriggeredCount", ref triggeredCount);
						if (flag19)
						{
							CS$<>8__locals1.argBox.Set("ConchShip_PresetKey_XuehouComingTriggeredCount", 1);
						}
						else
						{
							CS$<>8__locals1.argBox.Set("ConchShip_PresetKey_XuehouComingTriggeredCount", triggeredCount + 1);
						}
					}
					bool flag20 = CS$<>8__locals1.<AdvanceMonth_SectMainStory_Xuehou>g__TaiwuNotInVillageArea|4();
					if (!flag20)
					{
						bool flag21 = this.JixiAdventureDisappear(3, 9);
						if (flag21)
						{
							this.JixiGrowUp(CS$<>8__locals1.context, 538, 539);
							DomainManager.Extra.TriggerExtraTask(CS$<>8__locals1.context, 28, 134);
							DomainManager.Extra.FinishAllTaskInChain(CS$<>8__locals1.context, 29);
						}
						else
						{
							bool flag22 = this.JixiAdventurePass(2, 0) || this.JixiAdventureDisappear(2, 9);
							if (flag22)
							{
								bool flag23 = this.JixiAdventureDisappear(2, 9);
								if (flag23)
								{
									CS$<>8__locals1.argBox.Set("ConchShip_PresetKey_JixiAdventureTwoStartDate", int.MaxValue);
									this.JixiGrowUp(CS$<>8__locals1.context, 537, 538);
								}
								bool flag24 = !CS$<>8__locals1.argBox.Contains<int>("ConchShip_PresetKey_JixiAdventureThreeStartDate") && (this.JixiAdventurePass(2, 3) || this.JixiAdventureDisappear(2, 9));
								if (flag24)
								{
									List<MapBlockData> validBlocks = CS$<>8__locals1.context.AdvanceMonthRelatedData.Blocks.Occupy();
									DomainManager.Map.GetMapBlocksInAreaByFilters(CS$<>8__locals1.taiwuVillageLocation.AreaId, new Predicate<MapBlockData>(CS$<>8__locals1.<AdvanceMonth_SectMainStory_Xuehou>g__IsInTaiwuVillageRange|1), validBlocks);
									bool flag25 = validBlocks.Count > 0;
									if (flag25)
									{
										MapBlockData block = validBlocks.GetRandom(CS$<>8__locals1.context.Random);
										DomainManager.Adventure.TryCreateAdventureSite(CS$<>8__locals1.context, block.AreaId, block.BlockId, 162, MonthlyActionKey.Invalid);
										CS$<>8__locals1.argBox.Set("ConchShip_PresetKey_JixiAdventureThreeStartDate", this._currDate);
									}
									CS$<>8__locals1.context.AdvanceMonthRelatedData.Blocks.Release(ref validBlocks);
								}
								else
								{
									bool flag26 = !CS$<>8__locals1.argBox.GetBool("ConchShip_PresetKey_JixiFeedChickenEventTriggered");
									if (flag26)
									{
										CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouJixiFeedChicken();
									}
									bool flag27 = !CS$<>8__locals1.argBox.GetBool("ConchShip_PresetKey_JixiHarmvillagerEventTriggered");
									if (flag27)
									{
										CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouJixiKills();
									}
									else
									{
										CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouVillageWork();
									}
								}
							}
							else
							{
								bool flag28 = this.JixiAdventurePass(1, 0) || this.JixiAdventureDisappear(1, 9);
								if (flag28)
								{
									bool flag29 = !CS$<>8__locals1.argBox.Contains<int>("ConchShip_PresetKey_JixiAdventureTwoStartDate") && (this.JixiAdventurePass(1, 3) || this.JixiAdventureDisappear(1, 9));
									if (flag29)
									{
										List<MapBlockData> validBlocks2 = CS$<>8__locals1.context.AdvanceMonthRelatedData.Blocks.Occupy();
										DomainManager.Map.GetMapBlocksInAreaByFilters(CS$<>8__locals1.taiwuVillageLocation.AreaId, new Predicate<MapBlockData>(CS$<>8__locals1.<AdvanceMonth_SectMainStory_Xuehou>g__IsInTaiwuVillageRange|1), validBlocks2);
										bool flag30 = validBlocks2.Count > 0;
										if (flag30)
										{
											MapBlockData block2 = validBlocks2.GetRandom(CS$<>8__locals1.context.Random);
											DomainManager.Adventure.TryCreateAdventureSite(CS$<>8__locals1.context, block2.AreaId, block2.BlockId, 161, MonthlyActionKey.Invalid);
											CS$<>8__locals1.argBox.Set("ConchShip_PresetKey_JixiAdventureTwoStartDate", this._currDate);
										}
										CS$<>8__locals1.context.AdvanceMonthRelatedData.Blocks.Release(ref validBlocks2);
									}
									else
									{
										bool flag31 = !CS$<>8__locals1.argBox.GetBool("ConchShip_PresetKey_ProtectedJixiTriggered");
										if (flag31)
										{
											CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouProtectJixi();
										}
										else
										{
											CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouJixiAskForFood();
										}
									}
								}
							}
						}
					}
					break;
				}
				case 134:
				{
					bool flag32 = CS$<>8__locals1.<AdvanceMonth_SectMainStory_Xuehou>g__TaiwuNotInVillageArea|4();
					if (!flag32)
					{
						int passDate = int.MaxValue;
						int startDate3 = int.MaxValue;
						bool flag33 = !CS$<>8__locals1.argBox.Contains<bool>("ConchShip_PresetKey_CombatWithUltimateZombieTriggered") && ((CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_JixiAdventureThreePassDate", ref passDate) && this._currDate >= passDate + 2) || (CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_JixiAdventureThreeStartDate", ref startDate3) && this._currDate >= startDate3 + 9 + 2));
						if (flag33)
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryXuehouFinale();
						}
						startDate3 = int.MaxValue;
						bool flag34 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_JixiAdventureFourStartDate", ref startDate3) && this._currDate >= startDate3 + 6;
						if (flag34)
						{
							DomainManager.Extra.FinishTriggeredExtraTask(CS$<>8__locals1.context, 28, 134);
							DomainManager.World.SetSectMainStoryTaskStatus(CS$<>8__locals1.context, 15, 1);
							DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(CS$<>8__locals1.context, 15, "ConchShip_PresetKey_SectMainStoryXuehouProsperousEndDate", this._currDate + 1);
							this.JixiGrowUp(CS$<>8__locals1.context, 539, 537);
							GameData.Domains.Character.Character jixiBaby = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(CS$<>8__locals1.context, 537);
							Events.RaiseFixedCharacterLocationChanged(CS$<>8__locals1.context, jixiBaby.GetId(), jixiBaby.GetLocation(), CS$<>8__locals1.taiwuVillageLocation);
							jixiBaby.SetLocation(CS$<>8__locals1.taiwuVillageLocation, CS$<>8__locals1.context);
							MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
							monthlyNotifications.AddSectMainStoryXuehouJixiGoneFinal(CS$<>8__locals1.taiwu.GetId());
						}
					}
					break;
				}
				case 135:
					CS$<>8__locals1.<AdvanceMonth_SectMainStory_Xuehou>g__TryTriggerXuehouJixiGone|0();
					break;
				}
				DomainManager.Extra.SaveSectMainStoryEventArgumentBox(CS$<>8__locals1.context, 15);
			}
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x000E4D9C File Offset: 0x000E2F9C
		private void AdvanceMonth_SectMainStory_Shaolin(DataContext context)
		{
			WorldDomain.<>c__DisplayClass151_0 CS$<>8__locals1;
			CS$<>8__locals1.monthlyEventCollection = this.GetMonthlyEventCollection();
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int taiwuCharId = taiwu.GetId();
			Location taiwuLocation = taiwu.GetLocation();
			CS$<>8__locals1.argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(1);
			int shaolinTaskInProgress = DomainManager.Extra.GetExtraTaskChainCurrentTask(30);
			int prevTaiwuCharId = -1;
			bool isTaiwuChanged = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_DamoDreamMeetTaiwuId", ref prevTaiwuCharId) && prevTaiwuCharId != taiwuCharId;
			int num = shaolinTaskInProgress;
			int num2 = num;
			if (num2 >= 0)
			{
				if (num2 != 137)
				{
					switch (num2)
					{
					case 220:
					{
						bool flag = taiwuLocation.IsValid() && DomainManager.Map.GetBlock(taiwuLocation).BlockSubType == EMapBlockSubType.ShaolinPai;
						if (flag)
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShaolinTwoMonksTalk();
						}
						break;
					}
					case 221:
					{
						bool flag2 = isTaiwuChanged;
						if (flag2)
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShaolinDreamOfNewTaiwu();
						}
						else
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShaolinLearning();
						}
						break;
					}
					case 224:
					{
						bool flag3 = isTaiwuChanged;
						if (flag3)
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShaolinDreamOfNewTaiwu();
						}
						else
						{
							int prevDate = 0;
							bool isFirstTime = !CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_ShaolinMonthlyEventNotEnoughDate", ref prevDate);
							int expectedGradeGroup = Math.Clamp(CS$<>8__locals1.argBox.GetInt("ConchShip_PresetKey_ShaolinDamoFightTimes"), 0, 2);
							int cooldown = (expectedGradeGroup == 2) ? 6 : 3;
							sbyte combatSkillType = -1;
							bool flag4 = !CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_ShaolinCombatSkillType", ref combatSkillType) || combatSkillType < 0;
							if (!flag4)
							{
								IReadOnlyList<CombatSkillItem> learnableCombatSkills = CombatSkillDomain.GetLearnableCombatSkills(1, combatSkillType);
								Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> learnedSkills = DomainManager.CombatSkill.GetCharCombatSkills(taiwu.GetId());
								foreach (CombatSkillItem combatSkillCfg in learnableCombatSkills)
								{
									sbyte group = CombatSkillDomain.GetCombatSkillGradeGroup(combatSkillCfg.Grade, 1, combatSkillType);
									bool flag5 = (int)group != expectedGradeGroup;
									if (!flag5)
									{
										GameData.Domains.CombatSkill.CombatSkill combatSkill;
										bool flag6 = learnedSkills.TryGetValue(combatSkillCfg.TemplateId, out combatSkill) && CombatSkillStateHelper.IsBrokenOut(combatSkill.GetActivationState());
										if (!flag6)
										{
											bool flag7 = isFirstTime;
											if (flag7)
											{
												CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShaolinNotEnough();
											}
											else
											{
												bool flag8 = prevDate + cooldown <= this._currDate;
												if (flag8)
												{
													CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShaolinNotEnoughCommon();
												}
											}
											return;
										}
									}
								}
								bool flag9 = expectedGradeGroup == 2;
								if (flag9)
								{
									WorldDomain.<AdvanceMonth_SectMainStory_Shaolin>g__AddEndChallengeMonthlyEvent|151_1(ref CS$<>8__locals1);
								}
								else
								{
									WorldDomain.<AdvanceMonth_SectMainStory_Shaolin>g__AddChallengeMonthlyEvent|151_0(ref CS$<>8__locals1);
								}
							}
						}
						break;
					}
					case 225:
					{
						bool flag10 = isTaiwuChanged;
						if (flag10)
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShaolinDreamOfNewTaiwu();
						}
						else
						{
							bool flag11 = CS$<>8__locals1.argBox.GetInt("ConchShip_PresetKey_ShaolinDamoFightTimes") >= 2;
							if (flag11)
							{
								WorldDomain.<AdvanceMonth_SectMainStory_Shaolin>g__AddEndChallengeMonthlyEvent|151_1(ref CS$<>8__locals1);
							}
							else
							{
								WorldDomain.<AdvanceMonth_SectMainStory_Shaolin>g__AddChallengeMonthlyEvent|151_0(ref CS$<>8__locals1);
							}
						}
						break;
					}
					case 227:
					{
						bool flag12 = isTaiwuChanged;
						if (flag12)
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShaolinDreamOfNewTaiwu();
						}
						else
						{
							int offCooldownDate = 0;
							bool flag13 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_StudyForBodhidharmaChallenge", ref offCooldownDate) && offCooldownDate > this._currDate;
							if (!flag13)
							{
								bool flag14 = CS$<>8__locals1.argBox.GetInt("ConchShip_PresetKey_ShaolinDamoFightTimes") >= 2;
								if (flag14)
								{
									WorldDomain.<AdvanceMonth_SectMainStory_Shaolin>g__AddEndChallengeMonthlyEvent|151_1(ref CS$<>8__locals1);
								}
								else
								{
									WorldDomain.<AdvanceMonth_SectMainStory_Shaolin>g__AddChallengeMonthlyEvent|151_0(ref CS$<>8__locals1);
								}
							}
						}
						break;
					}
					case 228:
					{
						bool flag15 = isTaiwuChanged;
						if (flag15)
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShaolinDreamOfNewTaiwu();
						}
						else
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShaolinDreamOfReadingSutra();
						}
						break;
					}
					case 229:
					{
						bool flag16 = isTaiwuChanged;
						if (flag16)
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShaolinDreamOfNewTaiwu();
						}
						else
						{
							short lifeSkillTemplateId = -1;
							bool flag17 = !CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_ShaolinReadingMaxGradeSutra", ref lifeSkillTemplateId) || lifeSkillTemplateId < 0;
							if (!flag17)
							{
								int lifeSkillIndex = taiwu.FindLearnedLifeSkillIndex(lifeSkillTemplateId);
								bool flag18 = lifeSkillIndex < 0;
								if (!flag18)
								{
									List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = taiwu.GetLearnedLifeSkills();
									bool flag19 = learnedLifeSkills[lifeSkillIndex].GetReadPagesCount() < 3;
									if (!flag19)
									{
										bool flag20 = LifeSkill.Instance[lifeSkillTemplateId].Grade == 8;
										if (flag20)
										{
											CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShaolinEnlightenment();
										}
										else
										{
											CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShaolinLearning();
										}
									}
								}
							}
						}
						break;
					}
					}
				}
				else
				{
					bool flag21 = taiwu.GetInventory().GetInventoryItemCount(12, 247) > 0;
					if (flag21)
					{
						CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShaolinDreamFirst();
					}
				}
			}
			else
			{
				sbyte taskStatus = this.GetSectMainStoryTaskStatus(1);
				bool flag22 = taskStatus == 0;
				if (flag22)
				{
					int date = int.MaxValue;
					bool flag23 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_SectMainStoryShaolinProsperousEndDate", ref date);
					if (flag23)
					{
						bool flag24 = this._currDate >= date + 1;
						if (flag24)
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShaolinProsperous();
						}
					}
					else
					{
						bool flag25 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_SectMainStoryShaolinFailingEndDate", ref date);
						if (flag25)
						{
							bool flag26 = this._currDate >= date + 1;
							if (flag26)
							{
								CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShaolinFailing();
							}
						}
						else
						{
							Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(1);
							bool flag27 = sect.GetTaiwuExploreStatus() != 0 && taiwuLocation.IsValid() && DomainManager.Map.GetBlock(taiwuLocation).BlockSubType == EMapBlockSubType.ShaolinPai && !CS$<>8__locals1.argBox.Contains<int>(SectMainStoryEventArgKeys.TriggeringStatus);
							if (flag27)
							{
								CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShaolinTowerFalling();
							}
						}
					}
				}
			}
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x000E5330 File Offset: 0x000E3530
		private void AdvanceMonth_SectMainStory_Xuannv(DataContext context)
		{
			MonthlyEventCollection monthlyEventCollection = this.GetMonthlyEventCollection();
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int taiwuCharId = taiwu.GetId();
			Location taiwuLocation = taiwu.GetLocation();
			EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(8);
			int xuannvTaskInProgress = DomainManager.Extra.GetExtraTaskChainCurrentTask(31);
			bool flag = xuannvTaskInProgress < 0;
			if (flag)
			{
				sbyte taskStatus = this.GetSectMainStoryTaskStatus(8);
				bool flag2 = taskStatus == 0;
				if (flag2)
				{
					int date = int.MaxValue;
					bool flag3 = argBox.Get("ConchShip_PresetKey_SectMainStoryXuannvProsperousEndDate", ref date);
					if (flag3)
					{
						bool flag4 = this._currDate >= date + 1;
						if (flag4)
						{
							monthlyEventCollection.AddSectMainStoryXuannvProsperous();
						}
					}
					else
					{
						bool flag5 = argBox.Get("ConchShip_PresetKey_SectMainStoryXuannvFailingEndDate", ref date);
						if (flag5)
						{
							bool flag6 = this._currDate >= date + 1;
							if (flag6)
							{
								monthlyEventCollection.AddSectMainStoryXuannvFailing();
							}
						}
						else
						{
							Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(8);
							bool flag7 = sect.GetTaiwuExploreStatus() != 0 && taiwuLocation.IsValid() && DomainManager.Map.GetBlock(taiwuLocation).BlockSubType == EMapBlockSubType.XuannvPai && !argBox.Contains<int>(SectMainStoryEventArgKeys.TriggeringStatus);
							if (flag7)
							{
								monthlyEventCollection.AddSectMainStoryXuannvPrologue();
							}
						}
					}
				}
			}
			else
			{
				bool @bool = argBox.GetBool("ConchShip_PresetKey_XuannvStoryTriggerFirstTrack");
				if (@bool)
				{
					monthlyEventCollection.AddSectMainStoryXuannvFirstTrack();
				}
				int num = xuannvTaskInProgress;
				int num2 = num;
				if (num2 <= 146)
				{
					if (num2 != 139)
					{
						if (num2 != 140)
						{
							if (num2 == 146)
							{
								monthlyEventCollection.AddSectMainStoryXuannvMirrorDream();
							}
						}
						else
						{
							bool bool2 = argBox.GetBool("ConchShip_PresetKey_XuannStoryPartOneReceivingLetter");
							if (bool2)
							{
								monthlyEventCollection.AddSectMainStoryXuannvLetter(taiwuCharId);
							}
							bool bool3 = argBox.GetBool("ConchShip_PresetKey_XuannStoryPartOne_WaitSecretGuestEvent");
							if (bool3)
							{
								monthlyEventCollection.AddSectMainStoryXuannvDeadMessage(taiwuCharId);
							}
							bool flag8 = taiwu.GetInjuries().HasAnyInjury() || taiwu.GetPoisoned().IsNonZero() || taiwu.GetDisorderOfQi() > 0;
							if (flag8)
							{
								monthlyEventCollection.AddSectMainStoryXuannvHealing(taiwuCharId);
							}
						}
					}
					else
					{
						bool bool4 = argBox.GetBool("ConchShip_PresetKey_XuannStoryPartOneReceivingLetter");
						if (bool4)
						{
							monthlyEventCollection.AddSectMainStoryXuannvLetter(taiwuCharId);
						}
					}
				}
				else if (num2 != 148)
				{
					if (num2 != 248)
					{
						if (num2 == 249)
						{
							monthlyEventCollection.AddSectMainStoryXuannvStrangeMoan();
						}
					}
					else
					{
						int date2 = int.MaxValue;
						bool flag9 = taiwuLocation.IsValid() && DomainManager.Map.GetBlock(taiwuLocation).BlockSubType == EMapBlockSubType.XuannvPai;
						if (flag9)
						{
							monthlyEventCollection.AddSectMainStoryXuannvMeetJuner(taiwuCharId);
						}
						else
						{
							bool flag10 = argBox.Get("ConchShip_PresetKey_Xuannv_PartThree_TakeLoverDate", ref date2);
							if (flag10)
							{
								bool flag11 = this._currDate >= date2 + 6;
								if (flag11)
								{
									monthlyEventCollection.AddSectMainStoryXuannvReincarnationDeath2(taiwuCharId);
								}
								else
								{
									bool flag12 = this._currDate > date2 && !argBox.GetBool("ConchShip_PresetKey_Xuannv_MonthlyEventTrigger_WithSister");
									if (flag12)
									{
										monthlyEventCollection.AddSectMainStoryXuannvWithSister();
									}
								}
							}
						}
					}
				}
				else
				{
					monthlyEventCollection.AddSectMainStoryXuannvReincarnationDeath(taiwuCharId);
				}
			}
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x000E5640 File Offset: 0x000E3840
		private void AdvanceMonth_SectMainStory_Wudang(DataContext context)
		{
			WorldDomain.<>c__DisplayClass153_0 CS$<>8__locals1;
			CS$<>8__locals1.monthlyEventCollection = this.GetMonthlyEventCollection();
			GameData.Domains.Character.Character character3;
			bool notInProgress = !DomainManager.Character.TryGetFixedCharacterByTemplateId(543, out character3) || this.GetSectMainStoryTaskStatus(4) != 0;
			List<SectStoryHeavenlyTreeExtendable> trees = DomainManager.Extra.GetAllHeavenlyTrees();
			int newEnemyCount = 0;
			foreach (SectStoryHeavenlyTreeExtendable tree in trees)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(tree.Id);
				short templateId = character.GetTemplateId();
				bool flag = templateId == 677;
				if (flag)
				{
					bool flag2 = ItemTemplateHelper.CheckIsHeavenlyNormalTreeSeeds(12, tree.TemplateId) || notInProgress;
					if (flag2)
					{
						CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryWudangHeavenlyTreeDestroyed2(tree.Location);
						continue;
					}
				}
				bool flag3 = !tree.Location.IsValid() || templateId == 677;
				if (!flag3)
				{
					bool flag4 = ItemTemplateHelper.CheckIsHeavenlyNormalTreeSeeds(12, tree.TemplateId);
					if (flag4)
					{
						newEnemyCount += this.XiangshuMinionsProtectWudangHeavenlyTree(context, tree, true);
					}
				}
			}
			bool flag5 = newEnemyCount > 0;
			if (flag5)
			{
				CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryWudangProtectHeavenlyTree2();
			}
			bool flag6 = this.GetSectMainStoryTaskStatus(4) != 0;
			if (!flag6)
			{
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				CS$<>8__locals1.taiwuCharId = taiwu.GetId();
				CS$<>8__locals1.taiwuLocation = taiwu.GetLocation();
				CS$<>8__locals1.argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(4);
				CS$<>8__locals1.triggerTreeMonthlyNotification = false;
				bool isWudangHeavenlyTreeInProgress = DomainManager.Extra.IsExtraTaskChainInProgress(41);
				bool flag7 = isWudangHeavenlyTreeInProgress;
				if (flag7)
				{
					bool flag8 = DomainManager.Extra.IsExtraTaskInProgress(238);
					if (flag8)
					{
						List<SectStoryHeavenlyTreeExtendable> trees2 = DomainManager.Extra.GetAllHeavenlyTrees();
						int newEnemyCount2 = 0;
						foreach (SectStoryHeavenlyTreeExtendable tree2 in trees2)
						{
							bool flag9 = ItemTemplateHelper.CheckIsHeavenlyNormalTreeSeeds(12, tree2.TemplateId);
							if (!flag9)
							{
								GameData.Domains.Character.Character character2 = DomainManager.Character.GetElement_Objects(tree2.Id);
								short templateId2 = character2.GetTemplateId();
								bool flag10 = !tree2.Location.IsValid() || templateId2 == 677;
								if (!flag10)
								{
									newEnemyCount2 += this.XiangshuMinionsProtectWudangHeavenlyTree(context, tree2, false);
								}
							}
						}
						bool flag11 = newEnemyCount2 > 0;
						if (flag11)
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryWudangProtectHeavenlyTree();
						}
					}
					bool flag12 = DomainManager.Extra.IsExtraTaskInProgress(236);
					if (flag12)
					{
						List<SectStoryHeavenlyTreeExtendable> trees3 = DomainManager.Extra.GetAllHeavenlyTrees();
						foreach (SectStoryHeavenlyTreeExtendable tree3 in trees3)
						{
							bool flag13 = !tree3.Location.IsValid();
							if (!flag13)
							{
								bool flag14 = ItemTemplateHelper.CheckIsHeavenlyNormalTreeSeeds(12, tree3.TemplateId);
								if (!flag14)
								{
									bool flag15 = !tree3.MetInDream && tree3.GrowPoint >= 900;
									if (flag15)
									{
										CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryWudangMeetingImmortal(CS$<>8__locals1.taiwuCharId, taiwu.GetValidLocation(), tree3.Id, tree3.Location);
									}
								}
							}
						}
						GameData.Domains.Character.Character sloppyTaoist;
						bool flag16 = DomainManager.Character.TryGetFixedCharacterByTemplateId(543, out sloppyTaoist);
						if (flag16)
						{
							WorldDomain.<AdvanceMonth_SectMainStory_Wudang>g__AddSectMainStoryWudangGiftsReceived|153_0(sloppyTaoist, ref CS$<>8__locals1);
						}
						bool flag17 = CS$<>8__locals1.taiwuLocation.IsValid() && CS$<>8__locals1.argBox.GetInt("ConchShip_PresetKey_GiveTaoistTreasureCount") < 3 && CS$<>8__locals1.argBox.GetInt("ConchShip_PresetKey_GiveTaoistTreasureCount") > CS$<>8__locals1.argBox.GetInt("ConchShip_PresetKey_WudangChatEventTriggeredCount");
						if (flag17)
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryWudangChat();
						}
					}
					WorldDomain.<AdvanceMonth_SectMainStory_Wudang>g__TriggerTreeMonthlyNotification|153_1(ref CS$<>8__locals1);
				}
				int wudangTaskInProgress = DomainManager.Extra.GetExtraTaskChainCurrentTask(32);
				bool flag18 = wudangTaskInProgress < 0;
				if (flag18)
				{
					int date = int.MaxValue;
					bool flag19 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_SectMainStoryWudangProsperousEndDate", ref date) && this._currDate >= date + 1;
					if (flag19)
					{
						CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryWudangProsperous();
					}
					else
					{
						date = int.MaxValue;
						bool flag20 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_SectMainStoryWudangFailingEndDate", ref date) && this._currDate >= date + 1;
						if (flag20)
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryWudangFailing();
						}
					}
				}
				else
				{
					WorldDomain.<AdvanceMonth_SectMainStory_Wudang>g__TriggerTreeMonthlyNotification|153_1(ref CS$<>8__locals1);
					int num = wudangTaskInProgress;
					int num2 = num;
					if (num2 != 150)
					{
						if (num2 != 241)
						{
							if (num2 == 242)
							{
								GameData.Domains.Character.Character sloppyTaoist2;
								bool flag21 = DomainManager.Character.TryGetFixedCharacterByTemplateId(543, out sloppyTaoist2);
								if (flag21)
								{
									bool triggeredEvent = WorldDomain.<AdvanceMonth_SectMainStory_Wudang>g__AddSectMainStoryWudangGiftsReceived|153_0(sloppyTaoist2, ref CS$<>8__locals1);
									bool flag22 = !triggeredEvent;
									if (flag22)
									{
										CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryWudangGiftsReceived2(CS$<>8__locals1.taiwuCharId, CS$<>8__locals1.taiwuLocation);
									}
								}
							}
						}
						else
						{
							bool flag23 = CS$<>8__locals1.taiwuLocation.IsValid() && CS$<>8__locals1.argBox.GetInt("ConchShip_PresetKey_GiveTaoistTreasureCount") < 3 && CS$<>8__locals1.argBox.GetInt("ConchShip_PresetKey_GiveTaoistTreasureCount") > CS$<>8__locals1.argBox.GetInt("ConchShip_PresetKey_WudangChatEventTriggeredCount");
							if (flag23)
							{
								CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryWudangChat();
							}
						}
					}
					else
					{
						int date2 = int.MaxValue;
						bool flag24 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_CombatWithTaoistMonkDate", ref date2) && this._currDate >= date2 + 3;
						if (flag24)
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryWudangRequest(taiwu.GetId(), taiwu.GetLocation());
						}
					}
				}
			}
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x000E5C18 File Offset: 0x000E3E18
		private unsafe int XiangshuMinionsProtectWudangHeavenlyTree(DataContext context, SectStoryHeavenlyTreeExtendable tree, bool normalTree)
		{
			MonthlyEventCollection monthlyEventCollection = this.GetMonthlyEventCollection();
			Location treeLocation = tree.Location;
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(treeLocation.AreaId);
			List<MapTemplateEnemyInfo> movingEnemyList = new List<MapTemplateEnemyInfo>();
			List<GameData.Domains.Character.Character> villagerList = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
			List<Location> path = ObjectPool<List<Location>>.Instance.Get();
			List<MapTemplateEnemyInfo> blockEnemyList = new List<MapTemplateEnemyInfo>();
			bool isTreeDestroyed = false;
			movingEnemyList.Clear();
			Span<MapBlockData> span = areaBlocks;
			for (int i = 0; i < span.Length; i++)
			{
				MapBlockData block = *span[i];
				bool flag = block.TemplateEnemyList == null;
				if (!flag)
				{
					Location location = block.GetLocation();
					blockEnemyList.Clear();
					foreach (MapTemplateEnemyInfo mapTemplateEnemy in block.TemplateEnemyList)
					{
						bool flag2 = mapTemplateEnemy.SourceAdventureBlockId != treeLocation.BlockId;
						if (!flag2)
						{
							blockEnemyList.Add(mapTemplateEnemy);
						}
					}
					bool flag3 = blockEnemyList.Count == 0;
					if (!flag3)
					{
						villagerList.Clear();
						bool flag4 = block.CharacterSet != null;
						if (flag4)
						{
							foreach (int charId in block.CharacterSet)
							{
								GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
								OrganizationInfo orgInfo = character.GetOrganizationInfo();
								bool flag5 = orgInfo.OrgTemplateId != 16;
								if (!flag5)
								{
									bool flag6 = orgInfo.Grade == 8;
									if (!flag6)
									{
										VillagerWorkData work;
										bool flag7 = !DomainManager.Taiwu.TryGetElement_VillagerWork(charId, out work);
										if (!flag7)
										{
											bool flag8 = work.AreaId != location.AreaId || work.BlockId != location.BlockId;
											if (!flag8)
											{
												villagerList.Add(character);
											}
										}
									}
								}
							}
						}
						bool flag9 = villagerList.Count > 0;
						if (flag9)
						{
							int injuredCount = 0;
							foreach (GameData.Domains.Character.Character villager in villagerList)
							{
								bool flag10 = blockEnemyList.Count <= 0;
								if (flag10)
								{
									break;
								}
								int blockedEnemyIndex = context.Random.Next(blockEnemyList.Count);
								blockEnemyList.RemoveAt(blockedEnemyIndex);
								villager.ChangeHealth(context, -60);
								injuredCount++;
								bool flag11 = villager.GetHealth() <= 0;
								if (flag11)
								{
									LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
									bool flag12 = DomainManager.Extra.IsCharacterDying(villager.GetId());
									if (flag12)
									{
										DomainManager.Character.MakeCharacterDead(context, villager, 15);
										lifeRecordCollection.AddSectMainStoryWudangVillagerKilled(villager.GetId(), DomainManager.World.GetCurrDate(), villager.GetLocation());
										MonthlyNotificationCollection monthlyNotifications = this.GetMonthlyNotificationCollection();
										bool flag13 = !normalTree;
										if (flag13)
										{
											monthlyNotifications.AddSectMainStoryWudangVillagerCasualty(villager.GetId(), villager.GetLocation());
										}
										else
										{
											monthlyNotifications.AddNormalVillagerCasualty(villager.GetId(), villager.GetLocation());
										}
									}
									else
									{
										DomainManager.Extra.AddDyingCharacters(context, villager.GetId(), 15);
										lifeRecordCollection.AddSectMainStoryWudangInjured(villager.GetId(), DomainManager.World.GetCurrDate(), villager.GetLocation());
									}
								}
							}
							bool flag14 = injuredCount > 0;
							if (flag14)
							{
								MonthlyNotificationCollection monthlyNotifications2 = this.GetMonthlyNotificationCollection();
								bool flag15 = !normalTree;
								if (flag15)
								{
									monthlyNotifications2.AddSectMainStoryWudangVillagersInjured(injuredCount);
								}
								else
								{
									monthlyNotifications2.AddNormalVillagersInjured(injuredCount);
								}
							}
						}
						movingEnemyList.AddRange(blockEnemyList);
					}
				}
			}
			foreach (MapTemplateEnemyInfo enemy in movingEnemyList)
			{
				bool flag16 = enemy.SourceAdventureBlockId != treeLocation.BlockId || Config.Character.Instance[enemy.TemplateId].OrganizationInfo.OrgTemplateId != 19;
				if (!flag16)
				{
					Location location2 = new Location(treeLocation.AreaId, enemy.BlockId);
					MapDomain.GetPathInAreaWithoutCost(location2, treeLocation, path);
					Location nextLocation = (path.Count > 2) ? path[1] : treeLocation;
					Events.RaiseTemplateEnemyLocationChanged(context, enemy, location2, nextLocation);
					bool flag17 = nextLocation != treeLocation;
					if (!flag17)
					{
						GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
						Location taiwuLocation = taiwu.GetLocation();
						bool flag18 = taiwuLocation == nextLocation;
						if (!flag18)
						{
							bool flag19 = !normalTree;
							if (flag19)
							{
								monthlyEventCollection.AddSectMainStoryWudangHeavenlyTreeDestroyed(treeLocation);
							}
							else
							{
								monthlyEventCollection.AddNormalHeavenlyTreeDestroyed(treeLocation);
							}
							isTreeDestroyed = true;
							break;
						}
						MapBlockData block2 = DomainManager.Map.GetBlock(taiwuLocation);
						bool flag20 = block2.TemplateEnemyList == null || block2.TemplateEnemyList.Count == 0;
						if (!flag20)
						{
							bool triggered = false;
							foreach (MapTemplateEnemyInfo templateEnemy in block2.TemplateEnemyList)
							{
								CharacterItem template = Config.Character.Instance[templateEnemy.TemplateId];
								bool flag21 = template.OrganizationInfo.OrgTemplateId != 19;
								if (!flag21)
								{
									bool flag22 = templateEnemy.SourceAdventureBlockId != treeLocation.BlockId;
									if (!flag22)
									{
										bool flag23 = !normalTree;
										if (flag23)
										{
											monthlyEventCollection.AddSectMainStoryWudangGuardHeavenlyTree(taiwu.GetId(), taiwuLocation);
										}
										else
										{
											monthlyEventCollection.AddNormalGuardHeavenlyTree(taiwu.GetId(), taiwuLocation);
										}
										triggered = true;
										break;
									}
								}
							}
							bool flag24 = triggered;
							if (flag24)
							{
								break;
							}
						}
					}
				}
			}
			ObjectPool<List<Location>>.Instance.Return(path);
			ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(villagerList);
			bool flag25 = !isTreeDestroyed && !normalTree;
			int result;
			if (flag25)
			{
				short xiangshuMinionTemplateId = (short)(298 + (int)this.GetXiangshuLevel());
				bool flag26 = xiangshuMinionTemplateId > 306;
				if (flag26)
				{
					xiangshuMinionTemplateId = 306;
				}
				List<MapBlockData> blocks = context.AdvanceMonthRelatedData.Blocks.Occupy();
				DomainManager.Map.GetLocationByDistance(treeLocation, 3, 3, ref blocks);
				MapBlockData targetBlock = blocks.GetRandom(context.Random);
				context.AdvanceMonthRelatedData.Blocks.Release(ref blocks);
				targetBlock.AddTemplateEnemy(new MapTemplateEnemyInfo(xiangshuMinionTemplateId, targetBlock.BlockId, treeLocation.BlockId));
				result = 1;
			}
			else
			{
				bool flag27 = !isTreeDestroyed && normalTree;
				if (flag27)
				{
					List<MapBlockData> blockDataList = ObjectPool<List<MapBlockData>>.Instance.Get();
					DomainManager.Map.GetRealNeighborBlocks(treeLocation.AreaId, treeLocation.BlockId, blockDataList, 3, false);
					MapBlockData centerBlockData = DomainManager.Map.GetBlockData(treeLocation.AreaId, treeLocation.BlockId);
					blockDataList.Add(centerBlockData);
					foreach (MapBlockData blockData in blockDataList)
					{
						bool flag28 = blockData.TemplateEnemyList != null && blockData.TemplateEnemyList.Count > 0;
						if (flag28)
						{
							foreach (MapTemplateEnemyInfo mapTemplateEnemy2 in blockData.TemplateEnemyList)
							{
								bool flag29 = mapTemplateEnemy2.SourceAdventureBlockId == treeLocation.BlockId;
								if (flag29)
								{
									ObjectPool<List<MapBlockData>>.Instance.Return(blockDataList);
									return 1;
								}
							}
						}
					}
					ObjectPool<List<MapBlockData>>.Instance.Return(blockDataList);
				}
				result = 0;
			}
			return result;
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x000E647C File Offset: 0x000E467C
		private void AdvanceMonth_SectMainStory_Shixiang(DataContext context)
		{
			WorldDomain.<>c__DisplayClass155_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.monthlyEventCollection = this.GetMonthlyEventCollection();
			CS$<>8__locals1.taiwu = DomainManager.Taiwu.GetTaiwu();
			CS$<>8__locals1.taiwuLocation = CS$<>8__locals1.taiwu.GetLocation();
			EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(6);
			int shixiangTaskInProgress = DomainManager.Extra.GetExtraTaskChainCurrentTask(34);
			Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(6);
			Location settlementLocation = settlement.GetLocation();
			sbyte taskStatus = this.GetSectMainStoryTaskStatus(6);
			bool flag = taskStatus == 0;
			if (flag)
			{
				int date = int.MaxValue;
				bool flag2 = argBox.Get("ConchShip_PresetKey_SectMainStoryShixiangProsperousEndDate", ref date) && this._currDate >= date + 1;
				if (flag2)
				{
					CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShixiangProsperous(CS$<>8__locals1.taiwuLocation);
				}
				else
				{
					bool flag3 = argBox.Get("ConchShip_PresetKey_SectMainStoryShixiangFailingEndDate", ref date) && this._currDate >= date + 1;
					if (flag3)
					{
						CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShixiangFailing(CS$<>8__locals1.taiwuLocation);
					}
				}
			}
			bool flag4 = shixiangTaskInProgress - 171 <= 5;
			bool flag5 = flag4;
			if (flag5)
			{
				this.<AdvanceMonth_SectMainStory_Shixiang>g__AddInteractWithShixiangMemberEvent|155_1(ref CS$<>8__locals1);
			}
			int num = shixiangTaskInProgress;
			int num2 = num;
			switch (num2)
			{
			case 169:
			case 170:
			{
				int nextDate = 0;
				bool flag6 = argBox.Get("ConchShip_PresetKey_ShixiangAdventureAppearDate", ref nextDate) && this._currDate < nextDate;
				if (!flag6)
				{
					AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(settlementLocation.AreaId);
					foreach (KeyValuePair<short, AdventureSiteData> keyValuePair in adventuresInArea.AdventureSites)
					{
						short num3;
						AdventureSiteData adventureSiteData;
						keyValuePair.Deconstruct(out num3, out adventureSiteData);
						AdventureSiteData site = adventureSiteData;
						bool flag7 = site.TemplateId == 176;
						if (flag7)
						{
							return;
						}
					}
					List<short> blockIds = CS$<>8__locals1.context.AdvanceMonthRelatedData.BlockIds.Occupy();
					DomainManager.Map.GetSettlementBlocks(settlementLocation.AreaId, settlementLocation.BlockId, blockIds);
					short adventureBlockId = blockIds.GetRandom(CS$<>8__locals1.context.Random);
					CS$<>8__locals1.context.AdvanceMonthRelatedData.BlockIds.Release(ref blockIds);
					ShixiangStoryAdventureTriggerAction action = new ShixiangStoryAdventureTriggerAction
					{
						Location = new Location(settlementLocation.AreaId, adventureBlockId)
					};
					DomainManager.TaiwuEvent.AddTempDynamicAction<ShixiangStoryAdventureTriggerAction>(CS$<>8__locals1.context, action);
				}
				break;
			}
			case 171:
			{
				int date2 = int.MaxValue;
				bool flag8 = argBox.Get("ConchShip_PresetKey_ShixiangFirstLetterDate", ref date2) && this._currDate >= date2 + 4;
				if (flag8)
				{
					CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShixiangNotLetter(CS$<>8__locals1.taiwuLocation);
				}
				else
				{
					CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShixiangLetterFrom(CS$<>8__locals1.taiwuLocation);
				}
				break;
			}
			case 172:
			case 173:
				break;
			case 174:
				CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShixiangEnemyAttack3();
				break;
			case 175:
				CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShixiangEnemyAttack();
				break;
			case 176:
			{
				bool flag9 = !this.ShixiangSettlementAffiliatedBlocksHasEnemy(CS$<>8__locals1.context, 608);
				if (!flag9)
				{
					int startDate = 0;
					bool flag10 = argBox.Get("ConchShip_PresetKey_StartFightShixiangTraitorsDate", ref startDate) && this._currDate >= startDate + 36;
					if (flag10)
					{
						CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShixiangEnemyAttack2();
					}
				}
				break;
			}
			case 177:
			{
				sbyte shixiangMemberKillCount = argBox.GetSbyte("ConchShip_PresetKey_ShixiangKillBarbarianMasterCount2");
				int count = (int)(shixiangMemberKillCount / 10);
				bool flag11 = count > 0;
				if (flag11)
				{
					CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShixiangGoodNews();
					shixiangMemberKillCount -= (sbyte)(count * 10);
					argBox.Set("ConchShip_PresetKey_ShixiangKillBarbarianMasterCount2", shixiangMemberKillCount);
				}
				sbyte taiwuKillCount = argBox.GetSbyte("ConchShip_PresetKey_TaiwuKillBarbarianMasterCount2");
				count = (int)(taiwuKillCount / 10);
				bool flag12 = count > 0;
				if (flag12)
				{
					CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryShixiangGoodNews2();
					taiwuKillCount -= (sbyte)(count * 10);
					argBox.Set("ConchShip_PresetKey_TaiwuKillBarbarianMasterCount2", taiwuKillCount);
				}
				this.<AdvanceMonth_SectMainStory_Shixiang>g__EnemyClear|155_0(ref CS$<>8__locals1);
				break;
			}
			default:
				if (num2 == 234)
				{
					this.<AdvanceMonth_SectMainStory_Shixiang>g__EnemyClear|155_0(ref CS$<>8__locals1);
				}
				break;
			}
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x000E68BC File Offset: 0x000E4ABC
		private void AdvanceMonth_SectMainStory_Emei(DataContext context)
		{
			WorldDomain.<>c__DisplayClass156_0 CS$<>8__locals1;
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.<>4__this = this;
			sbyte taskStatus = this.GetSectMainStoryTaskStatus(2);
			bool flag = taskStatus != 0;
			if (!flag)
			{
				CS$<>8__locals1.argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(2);
				int emeiTaskInProgress = DomainManager.Extra.GetExtraTaskChainCurrentTask(37);
				MonthlyEventCollection monthlyEventCollection = this.GetMonthlyEventCollection();
				MonthlyNotificationCollection monthlyNotification = this.GetMonthlyNotificationCollection();
				CS$<>8__locals1.taiwu = DomainManager.Taiwu.GetTaiwu();
				int date = int.MaxValue;
				bool flag2 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_SectMainStoryEmeiProsperousEndDate", ref date) && this._currDate >= date + 1;
				if (flag2)
				{
					monthlyEventCollection.AddSectMainStoryEmeiProsperous();
				}
				else
				{
					bool flag3 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_SectMainStoryEmeiFailingEndDate", ref date) && this._currDate >= date + 1;
					if (flag3)
					{
						monthlyEventCollection.AddSectMainStoryEmeiFailing();
					}
				}
				int num = emeiTaskInProgress;
				int num2 = num;
				switch (num2)
				{
				case 200:
				{
					int date2 = int.MaxValue;
					Location emeiLocation = DomainManager.Organization.GetSettlementByOrgTemplateId(2).GetLocation();
					short apeBlockId = -1;
					CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_WhiteApeBlockId", ref apeBlockId);
					bool flag4 = (CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_HomocideCase0Time", ref date2) && date2 + 1 <= this._currDate) || (CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_HomocideCase1Time", ref date2) && date2 + 1 <= this._currDate);
					if (flag4)
					{
						DomainManager.Extra.GenerateEmeiBlood(CS$<>8__locals1.context, new Location(emeiLocation.AreaId, apeBlockId));
					}
					break;
				}
				case 201:
				case 202:
					break;
				case 203:
				{
					int date3 = int.MaxValue;
					bool flag5 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_ThirdClickWhiteGibbonDate", ref date3) && !CS$<>8__locals1.argBox.Contains<int>("ConchShip_PresetKey_FourthClickWhiteGibbonDate") && date3 + 3 <= this._currDate;
					if (flag5)
					{
						this.<AdvanceMonth_SectMainStory_Emei>g__EmeiWhiteGibbonAppear|156_0(false, ref CS$<>8__locals1);
						monthlyNotification.AddSectMainStoryWhiteGibbonReturns();
					}
					break;
				}
				case 204:
				{
					int date4 = int.MaxValue;
					bool flag6 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_EmeiAdventureTwoAppearDate", ref date4) && this._currDate >= date4 + 6;
					if (flag6)
					{
						bool flag7 = !CS$<>8__locals1.argBox.Contains<bool>("ConchShip_PresetKey_EmeiEnterAdventureTwo");
						if (flag7)
						{
							this.<AdvanceMonth_SectMainStory_Emei>g__HandleMissEmeiAdventureTwo|156_3(ref CS$<>8__locals1);
						}
					}
					break;
				}
				default:
					if (num2 != 258)
					{
						if (num2 == 259)
						{
							int date5 = int.MaxValue;
							bool flag8 = (CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_FourthClickWhiteGibbonDate", ref date5) && !CS$<>8__locals1.argBox.Contains<int>("ConchShip_PresetKey_FifthClickWhiteGibbonDate") && date5 + 1 <= this._currDate) || (CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_FifthClickWhiteGibbonDate", ref date5) && !CS$<>8__locals1.argBox.Contains<int>("ConchShip_PresetKey_SixthClickWhiteGibbonDate") && date5 + 2 <= this._currDate);
							if (flag8)
							{
								this.<AdvanceMonth_SectMainStory_Emei>g__ShiHoujiuAppear|156_1(ref CS$<>8__locals1);
								monthlyNotification.AddSectMainStoryEmeiShiReturns();
							}
						}
					}
					else
					{
						int date6 = int.MaxValue;
						bool flag9 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_FirstClickWhiteGibbonDate", ref date6) && !CS$<>8__locals1.argBox.Contains<int>("ConchShip_PresetKey_SecondClickWhiteGibbonDate") && date6 + 1 <= this._currDate;
						if (flag9)
						{
							this.<AdvanceMonth_SectMainStory_Emei>g__EmeiWhiteGibbonAppear|156_0(false, ref CS$<>8__locals1);
							monthlyNotification.AddSectMainStoryWhiteGibbonReturns();
						}
						bool flag10 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_SecondClickWhiteGibbonDate", ref date6) && !CS$<>8__locals1.argBox.Contains<int>("ConchShip_PresetKey_ThirdClickWhiteGibbonDate") && date6 + 2 <= this._currDate;
						if (flag10)
						{
							this.<AdvanceMonth_SectMainStory_Emei>g__EmeiWhiteGibbonAppear|156_0(true, ref CS$<>8__locals1);
							monthlyNotification.AddSectMainStoryWhiteGibbonReturns();
						}
					}
					break;
				}
			}
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x000E6C90 File Offset: 0x000E4E90
		private void AdvanceMonth_SectMainStory_Jingang(DataContext context)
		{
			sbyte taskStatus = this.GetSectMainStoryTaskStatus(11);
			bool flag = taskStatus != 0;
			if (!flag)
			{
				int jingangTaskInProgress = DomainManager.Extra.GetExtraTaskChainCurrentTask(35);
				EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				Location taiwuLocation = taiwu.GetValidLocation();
				Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(11);
				int date = int.MaxValue;
				bool flag2 = argBox.Get("ConchShip_PresetKey_JingangMonkMurderedTriggeredDate", ref date) && this._currDate >= date + 2;
				if (flag2)
				{
					monthlyNotificationCollection.AddSectMainStoryJingangWrongdoing();
					DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, 11, "ConchShip_PresetKey_JingangMonkMurderedTriggeredDate", this._currDate);
				}
				int num = jingangTaskInProgress;
				int num2 = num;
				if (num2 >= 0)
				{
					switch (num2)
					{
					case 179:
						break;
					case 180:
					case 181:
						break;
					case 182:
					{
						bool flag3 = taiwu.GetFeatureIds().Contains(483);
						if (flag3)
						{
							monthlyNotificationCollection.AddSectMainStoryJingangHaunted(taiwu.GetId());
						}
						break;
					}
					case 183:
						monthlyNotificationCollection.AddSectMainStoryJingangFollowedByGhost(taiwu.GetId());
						break;
					default:
						switch (num2)
						{
						case 187:
						{
							int stage = this.JingangSpreadSecInfoStage();
							int spreadSecInfoCountNeed = SectMainStoryRelatedConstants.JingangTaiwuSpreadSecInfoCount[stage];
							int secInfoSpreadSpeed = SectMainStoryRelatedConstants.JingangSecInfoSpreadSpeed[stage];
							int secInfoOpenCountNeed = SectMainStoryRelatedConstants.JingangSecInfoOpenCount[stage];
							int metaDataId = 0;
							argBox.Get("ConchShip_PresetKey_JingangSecInfoMetaDataId", ref metaDataId);
							bool flag4 = this.JingangKnowSecInfoCount() >= spreadSecInfoCountNeed;
							if (flag4)
							{
								IntList charIds;
								argBox.Get<IntList>("ConchShip_PresetKey_JingangKnowSecInfoIdList", out charIds);
								charIds.Items.Add(taiwu.GetId());
								CollectionUtils.Shuffle<int>(context.Random, charIds.Items);
								GameData.Domains.Character.Character character = null;
								for (int i = 0; i < charIds.Items.Count; i++)
								{
									bool flag5 = DomainManager.Character.TryGetElement_Objects(charIds.Items[i], out character);
									if (flag5)
									{
										break;
									}
								}
								MapBlockData[] mapBlockArray = DomainManager.Map.GetAreaBlocks(character.GetValidLocation().AreaId).ToArray();
								CollectionUtils.Shuffle<MapBlockData>(context.Random, mapBlockArray);
								int count = 0;
								for (int j = 0; j < mapBlockArray.Length; j++)
								{
									HashSet<int> characterSet = mapBlockArray[j].CharacterSet;
									bool flag6 = characterSet != null && characterSet.Count > 0;
									if (flag6)
									{
										foreach (int targetCharId in mapBlockArray[j].CharacterSet)
										{
											bool flag7 = charIds.Items.Contains(targetCharId);
											if (!flag7)
											{
												this.JingangDistributeSecInfo(context, metaDataId, targetCharId);
												this.JingangAddKnowSecInfoCharId(context, targetCharId);
												DomainManager.World.GetInstantNotifications().AddKnowMonkSecret(targetCharId);
												count++;
												bool flag8 = count >= secInfoSpreadSpeed;
												if (flag8)
												{
													break;
												}
											}
										}
									}
									bool flag9 = count >= secInfoSpreadSpeed;
									if (flag9)
									{
										break;
									}
								}
							}
							bool flag10 = this.JingangKnowSecInfoCount() >= secInfoOpenCountNeed - 1 || DomainManager.Information.IsSecretInformationInBroadcast(metaDataId);
							if (flag10)
							{
								monthlyEventCollection.AddSectMainStoryJingangVisitorsArrive();
								this.JingangBroadCastSecInfo(context);
							}
							break;
						}
						case 188:
						{
							bool flag11 = argBox.Contains<bool>("ConchShip_PresetKey_JingangTriggerMonthlyEventVillagerSuffer");
							if (flag11)
							{
								monthlyEventCollection.AddSectMainStoryJingangResidentsSufferingContinues();
							}
							break;
						}
						case 189:
						case 191:
							break;
						case 190:
						case 192:
						{
							bool jingangDefeatShmashanaAdhipati = false;
							argBox.Get("ConchShip_PresetKey_JingangDefeatShmashanaAdhipati", ref jingangDefeatShmashanaAdhipati);
							bool flag12 = jingangDefeatShmashanaAdhipati;
							if (flag12)
							{
								bool jingangMonkReincarnationTriggered = false;
								argBox.Get("ConchShip_PresetKey_JingangSoulTransformOpen", ref jingangMonkReincarnationTriggered);
								bool flag13 = !jingangMonkReincarnationTriggered;
								if (flag13)
								{
									monthlyEventCollection.AddSectMainStoryJingangReincarnation(taiwu.GetId());
									monthlyNotificationCollection.AddSectMainStoryJingangRockFleshed();
									GameData.Domains.Character.Character monk = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 747);
									Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
									Events.RaiseFixedCharacterLocationChanged(context, monk.GetId(), monk.GetLocation(), taiwuVillageLocation);
									monk.SetLocation(taiwuVillageLocation, context);
									DomainManager.Character.DirectlySetFavorabilities(context, monk.GetId(), taiwu.GetId(), 30000, 30000);
									DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, 11, "ConchShip_PresetKey_JingangWesternBuddhistMonkPassLegacyTaiwuId", taiwu.GetId());
								}
							}
							else
							{
								bool jingangMonkGhostVanishesTriggered = false;
								argBox.Get("ConchShip_PresetKey_JingangMonkGhostVanishesTriggered", ref jingangMonkGhostVanishesTriggered);
								bool flag14 = !jingangMonkGhostVanishesTriggered;
								if (flag14)
								{
									monthlyEventCollection.AddSectMainStoryJingangGhostVanishes();
								}
							}
							break;
						}
						default:
							switch (num2)
							{
							case 286:
								monthlyEventCollection.AddSectMainStoryJingangHearsay();
								break;
							case 287:
								monthlyEventCollection.AddSectMainStoryJingangSutraDisappears(taiwu.GetId());
								break;
							case 290:
								monthlyEventCollection.AddSectMainStoryJingangSutraSecrets();
								break;
							}
							break;
						}
						break;
					}
					bool flag15 = this.JingangIsInSpreadSutraTask();
					if (flag15)
					{
						bool flag16 = this.JingangCanTriggerMonkSoulEnterDream(context);
						if (flag16)
						{
							monthlyEventCollection.AddSectMainStoryJingangRitualsInDream();
						}
						int date2 = int.MaxValue;
						bool flag17 = argBox.Get("ConchShip_PresetKey_JingangAttackDate", ref date2) && this._currDate >= date2 + SectMainStoryRelatedConstants.JingangEventFrequency1;
						if (flag17)
						{
							monthlyEventCollection.AddSectMainStoryJingangAttack();
							argBox.Set("ConchShip_PresetKey_JingangAttackDate", this._currDate);
						}
						bool flag18 = argBox.Get("ConchShip_PresetKey_JingangFamousFakeMonkDate", ref date2) && this._currDate >= date2 + SectMainStoryRelatedConstants.JingangEventFrequency1;
						if (flag18)
						{
							monthlyNotificationCollection.AddSectMainStoryJingangFamousFakeMonk();
							argBox.Set("ConchShip_PresetKey_JingangFamousFakeMonkDate", this._currDate);
						}
						bool flag19 = argBox.Get("ConchShip_PresetKey_JingangPrayDate", ref date2) && this._currDate >= date2 + SectMainStoryRelatedConstants.JingangEventFrequency1;
						if (flag19)
						{
							monthlyNotificationCollection.AddSectMainStoryJingangPray();
							argBox.Set("ConchShip_PresetKey_JingangPrayDate", this._currDate);
						}
						bool jingangSelectHelpWestMonk = false;
						bool flag20 = !argBox.Get("ConchShip_PresetKey_JingangSelectHelpWestMonk", ref jingangSelectHelpWestMonk);
						if (flag20)
						{
							bool flag21 = argBox.Get("ConchShip_PresetKey_JingangLettersFromJingangDate", ref date2) && this._currDate >= date2 + SectMainStoryRelatedConstants.JingangEventFrequency2;
							if (flag21)
							{
								GameData.Domains.Character.Character leader = (settlement != null) ? settlement.GetAvailableHighMember(8, 0, false) : null;
								bool flag22 = leader != null;
								if (flag22)
								{
									monthlyEventCollection.AddSectMainStoryJingangLettersFromJingang(leader.GetId());
									argBox.Set("ConchShip_PresetKey_JingangLettersFromJingangDate", this._currDate);
								}
							}
						}
						bool jingangSecInfoSpreadingSelectBetray = false;
						bool flag23 = !argBox.Get("ConchShip_PresetKey_JingangSecInfoSpreadingSelectBetray", ref jingangSecInfoSpreadingSelectBetray);
						if (flag23)
						{
							bool flag24 = argBox.Get("ConchShip_PresetKey_JingangFameDistributionDate", ref date2) && this._currDate >= date2 + SectMainStoryRelatedConstants.JingangEventFrequency1;
							if (flag24)
							{
								short areaId = DomainManager.Map.GetSpiritualDebtLowestAreaIdByAreaId(taiwuLocation.AreaId);
								DomainManager.Extra.ChangeAreaSpiritualDebt(context, areaId, 200, true, true);
								taiwu.ChangeResource(context, 7, 4000);
								InstantNotificationCollection collection = DomainManager.World.GetInstantNotificationCollection();
								collection.AddResourceIncreased(taiwu.GetId(), 7, 4000);
								monthlyNotificationCollection.AddSectMainStoryJingangFameDistribution(taiwu.GetId(), taiwuLocation);
								argBox.Set("ConchShip_PresetKey_JingangFameDistributionDate", this._currDate);
							}
						}
						bool flag25 = this.JingangCanTriggerPietyEvent();
						if (flag25)
						{
							monthlyEventCollection.AddSectMainStoryJingangPiety(taiwu.GetId());
						}
					}
					int num3 = jingangTaskInProgress;
					int num4 = num3;
					if (num4 != 181)
					{
						if (num4 == 186)
						{
							int count2 = 0;
							argBox.Get("ConchShip_PresetKey_JingangPersuadeVillagerCount", ref count2);
							bool flag26 = count2 >= 1 && !argBox.Contains<bool>("ConchShip_PresetKey_JingangMonthlyEventVillagerEscapeTriggered");
							if (flag26)
							{
								monthlyNotificationCollection.AddSectMainStoryJingangVillagerFlee(taiwu.GetId());
							}
							goto IL_828;
						}
						if (num4 != 188)
						{
							goto IL_828;
						}
					}
					bool flag27 = !argBox.Contains<bool>("ConchShip_PresetKey_JingangMonthlyEventVillagerEscapeTriggered") && !argBox.Contains<bool>("ConchShip_PresetKey_JingangTriggerMonthlyEventVillagerSuffer");
					if (flag27)
					{
						monthlyNotificationCollection.AddSectMainStoryJingangVillagerFlee(taiwu.GetId());
					}
					IL_828:
					int num5 = jingangTaskInProgress;
					int num6 = num5;
					if (num6 == 188 || num6 == 192)
					{
						int date3 = int.MaxValue;
						bool flag28 = argBox.Get("ConchShip_PresetKey_SectMainStoryJingangProsperousEndDate", ref date3) && this._currDate >= date3 + 1;
						if (flag28)
						{
							monthlyEventCollection.AddSectMainStoryJingangProsperous();
						}
						else
						{
							bool flag29 = argBox.Get("ConchShip_PresetKey_SectMainStoryJingangFailingEndDate", ref date3) && this._currDate >= date3 + 1;
							if (flag29)
							{
								monthlyEventCollection.AddSectMainStoryJingangFailing();
							}
						}
					}
				}
				else
				{
					bool flag30 = this.JingangMonkWasRobbedCanTrigger() && !argBox.Contains<int>(SectMainStoryEventArgKeys.TriggeringStatus);
					if (flag30)
					{
						monthlyEventCollection.AddSectMainStoryJingangMonkMurdered();
					}
				}
			}
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x000E7560 File Offset: 0x000E5760
		private void AdvanceMonth_SectMainStory_Wuxian(DataContext context)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
			Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
			int progress = DomainManager.Extra.GetExtraTaskChainCurrentTask(36);
			int wugAdded;
			bool flag = this.TryGetPrologueAddedWug(out wugAdded) && wugAdded > -1;
			if (flag)
			{
				bool flag2 = this.IsWuxianTaiwuChanged() && this.IsWuxianPrologueWugAttackedOnce() && progress == 276;
				if (flag2)
				{
					monthlyEventCollection.AddSectMainStoryWuxianMiaoWoman(location);
				}
				else
				{
					monthlyEventCollection.AddSectMainStoryWuxianPoisonousWug(taiwuId);
				}
			}
			else
			{
				bool flag3 = this.GetWuxianChapterOneWishCount() > this.GetWuxianChapterOneWishComeTrueCount() && this.GetWuxianChapterOneWishComeTrueCount() < 2;
				if (flag3)
				{
					monthlyEventCollection.AddSectMainStoryWuxianStrangeThings();
				}
				else
				{
					bool flag4 = WorldDomain.IsAbleToTriggerWuxianChapterThreeMail();
					if (flag4)
					{
						monthlyEventCollection.AddSectMainStoryWuxianGiftsReceived(taiwuId, location);
					}
					else
					{
						EventArgBox permanentArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(12);
						int date = -1;
						int currDate = this.GetCurrDate();
						bool flag5 = progress >= 0 && progress != 284 && permanentArgBox.Get("ConchShip_PresetKey_Wuxian_Chapter4_HappyEndingEventDate", ref date) && currDate >= date;
						if (flag5)
						{
							monthlyEventCollection.AddSectMainStoryWuxianStrangeThings();
						}
						int num = progress;
						int num2 = num;
						if (num2 <= 199)
						{
							if (num2 == 198)
							{
								bool flag6 = permanentArgBox.Get("ConchShip_PresetKey_SectMainStoryWuxianProsperousEndDate", ref date) && currDate >= date && !this.IsWuxianEndingEventTriggered();
								if (flag6)
								{
									monthlyEventCollection.AddSectMainStoryWuxianProsperous();
								}
								goto IL_1FC;
							}
							if (num2 != 199)
							{
								goto IL_1FC;
							}
						}
						else
						{
							if (num2 == 279)
							{
								bool flag7 = permanentArgBox.Get("ConchShip_PresetKey_SectMainStoryWuxianFailingEndDate", ref date) && currDate >= date && !this.IsWuxianEndingEventTriggered();
								if (flag7)
								{
									monthlyEventCollection.AddSectMainStoryWuxianFailing0();
								}
								goto IL_1FC;
							}
							if (num2 != 285)
							{
								goto IL_1FC;
							}
						}
						bool flag8 = permanentArgBox.Get("ConchShip_PresetKey_SectMainStoryWuxianFailingEndDate", ref date) && currDate >= date && !this.IsWuxianEndingEventTriggered();
						if (flag8)
						{
							monthlyEventCollection.AddSectMainStoryWuxianFailing1();
						}
						IL_1FC:;
					}
				}
			}
			this.UpdateWuxianParanoiaCharacters(context);
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x000E7774 File Offset: 0x000E5974
		private void AdvanceMonth_SectMainStory_Ranshan(DataContext context)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
			Location location = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
			EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(7);
			bool flag = DomainManager.Extra.IsExtraTaskChainInProgress(49);
			if (flag)
			{
				int currDate = this.GetCurrDate();
				int startDate = currDate;
				argBox.Get("ConchShip_PresetKey_Ranshan_Chapter2_TeachStartDate", ref startDate);
				argBox.Set("ConchShip_PresetKey_SanZongBiWuCountDown", 24 - currDate + startDate);
				DomainManager.Extra.SaveSectMainStoryEventArgumentBox(context, 7);
			}
			bool flag2 = this.IsRanshanSectMainStoryAbleToTrigger();
			if (flag2)
			{
				bool flag3 = !argBox.Contains<int>(SectMainStoryEventArgKeys.TriggeringStatus);
				if (flag3)
				{
					monthlyEventCollection.AddSectMainStoryRanshanDragonGate();
				}
			}
			else
			{
				bool flag4 = this.IsRanshanChapter1MonthlyEvent2AbleToTrigger();
				if (flag4)
				{
					monthlyEventCollection.AddSectMainStoryRanshanMessage(taiwuId);
				}
				else
				{
					bool flag5 = this.IsRanshanChapter1MonthlyEvent3AbleToTrigger();
					if (flag5)
					{
						monthlyEventCollection.AddSectMainStoryRanshanAfterQinglang(taiwuId, location);
					}
					else
					{
						bool flag6 = this.IsRanshanChapter2HuajuAbleToTrigger();
						if (flag6)
						{
							monthlyEventCollection.AddSectMainStoryRanshanPaperCraneFromYufuFaction(taiwuId, location);
						}
						else
						{
							bool flag7 = this.IsRanshanChapter2XuanzhiAbleToTrigger();
							if (flag7)
							{
								monthlyEventCollection.AddSectMainStoryRanshanPaperCraneFromShenjianFaction(taiwuId, location);
							}
							else
							{
								bool flag8 = this.IsRanshanChapter2YingjiaoAbleToTrigger();
								if (flag8)
								{
									monthlyEventCollection.AddSectMainStoryRanshanPaperCraneFromYinyangFaction(taiwuId, location);
								}
								else
								{
									bool flag9 = this.IsRanshanChapter2MonthlyEventAbleToTrigger();
									if (flag9)
									{
										monthlyEventCollection.AddSectMainStoryRanshanSanshiLeave(taiwuId);
									}
									else
									{
										bool flag10 = DomainManager.Extra.IsExtraTaskInProgress(323);
										if (flag10)
										{
											DomainManager.Extra.TriggerExtraTask(context, 40, 297);
											GameData.Domains.Character.Character character = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 625);
											Location ranshanLocation = DomainManager.Organization.GetSettlementByOrgTemplateId(7).GetLocation();
											Events.RaiseFixedCharacterLocationChanged(context, character.GetId(), character.GetLocation(), ranshanLocation);
											character.SetLocation(ranshanLocation, context);
										}
										else
										{
											int progress = DomainManager.Extra.GetExtraTaskChainCurrentTask(40);
											int num = progress;
											int num2 = num;
											if (num2 != 299)
											{
												if (num2 == 300)
												{
													monthlyEventCollection.AddSectMainStoryRanshanFailing();
													this.ConvertRanshanFootman(context, false);
												}
											}
											else
											{
												bool flag11 = DomainManager.Extra.IsRanshanMenteeGoodStoryEnding();
												if (flag11)
												{
													monthlyEventCollection.AddSectMainStoryRanshanProsperous();
													this.ConvertRanshanFootman(context, true);
												}
												else
												{
													monthlyEventCollection.AddSectMainStoryRanshanFailing();
													this.ConvertRanshanFootman(context, false);
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
			this.UpdateRanshanThreeCorpsesAction(context);
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x000E79CC File Offset: 0x000E5BCC
		private void AdvanceMonth_SectMainStory_Baihua(DataContext context)
		{
			WorldDomain.<>c__DisplayClass160_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.context = context;
			bool flag = DomainManager.World.GetMainStoryLineProgress() < 22;
			if (!flag)
			{
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				CS$<>8__locals1.taiwuLocation = taiwu.GetLocation();
				CS$<>8__locals1.taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
				short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
				short whiteDeerLakeAreaId = DomainManager.Map.GetAreaIdByAreaTemplateId(18);
				CS$<>8__locals1.monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				CS$<>8__locals1.monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
				CS$<>8__locals1.lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				CS$<>8__locals1.argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
				bool baihuaEndenmicTriggered = CS$<>8__locals1.argBox.Contains<bool>("ConchShip_PresetKey_BaihuaEndenmicTriggered");
				bool flag2 = CS$<>8__locals1.taiwuLocation.AreaId == whiteDeerLakeAreaId && !baihuaEndenmicTriggered && !CS$<>8__locals1.argBox.Contains<int>(SectMainStoryEventArgKeys.TriggeringStatus);
				if (flag2)
				{
					CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryBaihuaEndenmic();
				}
				sbyte taskStatus = this.GetSectMainStoryTaskStatus(3);
				bool flag3 = taskStatus == 1;
				if (flag3)
				{
					bool flag4 = CS$<>8__locals1.argBox.Contains<int>("ConchShip_PresetKey_BaihuaLMTransferAnimalDate");
					if (flag4)
					{
						int date = int.MaxValue;
						bool flag5 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_BaihuaLMTransferAnimalDate", ref date) && date + (int)GlobalConfig.Instance.BaihuaLifeLinkRemoveCharacterCooldown <= this._currDate;
						if (flag5)
						{
							GameData.Domains.Character.Character leukorpus = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(CS$<>8__locals1.context, 781);
							GameData.Domains.Character.Character melanpsyche = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(CS$<>8__locals1.context, 786);
							GameData.Domains.Character.Character leukoDeer = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(CS$<>8__locals1.context, 808);
							GameData.Domains.Character.Character melanoOwl = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(CS$<>8__locals1.context, 809);
							Settlement baihuaSettlement = DomainManager.Organization.GetSettlementByOrgTemplateId(3);
							Location baihuaLocation = baihuaSettlement.GetLocation();
							Events.RaiseFixedCharacterLocationChanged(CS$<>8__locals1.context, leukorpus.GetId(), leukorpus.GetLocation(), baihuaLocation);
							leukorpus.SetLocation(baihuaLocation, CS$<>8__locals1.context);
							Events.RaiseFixedCharacterLocationChanged(CS$<>8__locals1.context, melanpsyche.GetId(), melanpsyche.GetLocation(), baihuaLocation);
							melanpsyche.SetLocation(baihuaLocation, CS$<>8__locals1.context);
							Events.RaiseFixedCharacterLocationChanged(CS$<>8__locals1.context, leukoDeer.GetId(), leukoDeer.GetLocation(), Location.Invalid);
							leukoDeer.SetLocation(Location.Invalid, CS$<>8__locals1.context);
							Events.RaiseFixedCharacterLocationChanged(CS$<>8__locals1.context, melanoOwl.GetId(), melanoOwl.GetLocation(), Location.Invalid);
							melanoOwl.SetLocation(Location.Invalid, CS$<>8__locals1.context);
							DomainManager.Extra.RemoveArgToSectMainStoryEventArgBox<int>(CS$<>8__locals1.context, 3, "ConchShip_PresetKey_BaihuaLMTransferAnimalDate");
							InstantNotificationCollection instantCollection = DomainManager.World.GetInstantNotificationCollection();
							instantCollection.AddSectStoryBaihuaToHuman();
						}
					}
				}
				bool flag6 = taskStatus != 0;
				if (!flag6)
				{
					int date2 = int.MaxValue;
					bool flag7 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_SectMainStoryBaihuaFailingEndDate", ref date2) && this._currDate >= date2 + 1;
					if (flag7)
					{
						CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryBaihuaFailing();
					}
					else
					{
						bool flag8 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_SectMainStoryBaihuaProsperousEndDate", ref date2) && this._currDate >= date2 + 1;
						if (flag8)
						{
							CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryBaihuaProsperous();
						}
						else
						{
							int baihuaTaskInProgress = DomainManager.Extra.GetExtraTaskChainCurrentTask(48);
							bool baihuaCombatTaskInProgress = DomainManager.Extra.IsExtraTaskChainInProgress(50);
							bool baihuaRelationshipTaskInProgress = DomainManager.Extra.IsExtraTaskChainInProgress(51);
							CS$<>8__locals1.tryTriggerBaihuaCombatTaskChain = false;
							CS$<>8__locals1.tryTriggerBaihuaManicLow = false;
							CS$<>8__locals1.tryTriggerBaihuaManicHigh = false;
							CS$<>8__locals1.tryTriggerLeukoMelanoPlay = false;
							CS$<>8__locals1.tryTriggerLMPlay = false;
							int num = baihuaTaskInProgress;
							int num2 = num;
							switch (num2)
							{
							case 302:
							{
								int nextDate = 0;
								bool flag9 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_BaihuaAdventureFourAppearDate", ref nextDate) && this._currDate >= nextDate;
								if (flag9)
								{
									bool flag10 = DomainManager.Adventure.QueryAdventureLocation(185).IsValid();
									if (flag10)
									{
										return;
									}
									DomainManager.TaiwuEvent.AddTempDynamicAction<BaihuaStoryAdventureFourTriggerAction>(CS$<>8__locals1.context, new BaihuaStoryAdventureFourTriggerAction());
								}
								break;
							}
							case 303:
								break;
							case 304:
							{
								bool flag11 = !CS$<>8__locals1.argBox.Contains<bool>("ConchShip_PresetKey_BaihuaDreamAboutPastFirstTriggered");
								if (flag11)
								{
									CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryBaihuaDreamAboutPastFirst(taiwu.GetId());
								}
								break;
							}
							case 305:
							{
								bool flag12 = !CS$<>8__locals1.argBox.Contains<bool>("ConchShip_PresetKey_BaihuaMelanpsycheArrivedEventTriggered");
								if (flag12)
								{
									CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryBaihuaMelanoArrived(taiwu.GetId());
								}
								break;
							}
							default:
								switch (num2)
								{
								case 325:
									this.<AdvanceMonth_SectMainStory_Baihua>g__TryTriggerBaihuaManicLow|160_3(ref CS$<>8__locals1);
									this.<AdvanceMonth_SectMainStory_Baihua>g__TryTriggerBaihuaManicHigh|160_4(true, ref CS$<>8__locals1);
									this.<AdvanceMonth_SectMainStory_Baihua>g__TryTriggedLeukoMelanoPlay|160_5(ref CS$<>8__locals1);
									this.<AdvanceMonth_SectMainStory_Baihua>g__TryTriggedLMPlay|160_6(ref CS$<>8__locals1);
									break;
								case 326:
								{
									this.<AdvanceMonth_SectMainStory_Baihua>g__TryTriggerBaihuaManicHigh|160_4(false, ref CS$<>8__locals1);
									int date3 = int.MaxValue;
									bool flag13 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_BaihuaManicHighDate", ref date3) && this._currDate >= date3 + 3;
									if (flag13)
									{
										CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryBaihuaAnonymReturns();
									}
									break;
								}
								case 327:
									this.<AdvanceMonth_SectMainStory_Baihua>g__TryTriggerBaihuaManicHigh|160_4(false, ref CS$<>8__locals1);
									break;
								case 328:
								{
									int date4 = int.MaxValue;
									bool flag14 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_BaihuaTriggerFinaleTaskDate", ref date4) && this._currDate >= date4 + 1;
									if (flag14)
									{
										CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryBaihuaGifts(taiwu.GetLocation());
									}
									break;
								}
								}
								break;
							}
							bool flag15 = baihuaCombatTaskInProgress;
							if (flag15)
							{
								bool flag16 = DomainManager.Extra.IsExtraTaskInProgress(307);
								if (flag16)
								{
									this.<AdvanceMonth_SectMainStory_Baihua>g__TryTriggerBaihuaCombatTaskChain|160_0(ref CS$<>8__locals1);
									int date5 = 0;
									bool flag17 = !CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_BaihuaDreamAboutPastLastDate", ref date5);
									if (flag17)
									{
										return;
									}
									bool flag18 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_BaihuaDreamAboutPastLastDate", ref date5) && this._currDate < date5 + 3;
									if (flag18)
									{
										return;
									}
									bool flag19 = !CS$<>8__locals1.argBox.Contains<bool>("ConchShip_PresetKey_BaihuaDreamAboutPastLastTriggered");
									if (flag19)
									{
										CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryBaihuaDreamAboutPastLast(taiwu.GetId());
									}
								}
								bool flag20 = DomainManager.Extra.IsExtraTaskInProgress(308);
								if (flag20)
								{
									this.<AdvanceMonth_SectMainStory_Baihua>g__TryTriggerBaihuaCombatTaskChain|160_0(ref CS$<>8__locals1);
									bool flag21 = CS$<>8__locals1.context.Random.CheckPercentProb(50);
									if (flag21)
									{
										bool isLock = false;
										CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementIdLock", ref isLock);
										short settlementId = -1;
										CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementId", ref settlementId);
										Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
										bool flag22 = !isLock;
										if (flag22)
										{
											List<short> settlementIds = ObjectPool<List<short>>.Instance.Get();
											DomainManager.Map.GetAreaSettlementIds(settlement.GetLocation().AreaId, settlementIds, true, true);
											short settlementIdNew = settlementIds.GetRandom(CS$<>8__locals1.context.Random);
											ObjectPool<List<short>>.Instance.Return(settlementIds);
											bool flag23 = settlementIdNew != settlementId;
											if (flag23)
											{
												DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<short>(CS$<>8__locals1.context, 3, "ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementId", settlementIdNew);
												settlement = DomainManager.Organization.GetSettlement(settlementIdNew);
												this.CallBaihuaMember(CS$<>8__locals1.context, true);
											}
										}
										else
										{
											this.CallBaihuaMember(CS$<>8__locals1.context, true);
										}
										CS$<>8__locals1.monthlyNotificationCollection.AddSectMainStoryBaihuaLeukoKills(settlement.GetLocation());
									}
								}
								bool flag24 = DomainManager.Extra.IsExtraTaskInProgress(310);
								if (flag24)
								{
									this.<AdvanceMonth_SectMainStory_Baihua>g__TryTriggerBaihuaCombatTaskChain|160_0(ref CS$<>8__locals1);
									bool flag25 = CS$<>8__locals1.context.Random.CheckPercentProb(50);
									if (flag25)
									{
										bool isLock2 = false;
										CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementIdLock", ref isLock2);
										short settlementId2 = -1;
										CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementId", ref settlementId2);
										Settlement settlement2 = DomainManager.Organization.GetSettlement(settlementId2);
										bool flag26 = !isLock2;
										if (flag26)
										{
											List<short> settlementIds2 = ObjectPool<List<short>>.Instance.Get();
											DomainManager.Map.GetAreaSettlementIds(settlement2.GetLocation().AreaId, settlementIds2, true, true);
											short settlementIdNew2 = settlementIds2.GetRandom(CS$<>8__locals1.context.Random);
											ObjectPool<List<short>>.Instance.Return(settlementIds2);
											bool flag27 = settlementIdNew2 != settlementId2;
											if (flag27)
											{
												DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<short>(CS$<>8__locals1.context, 3, "ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementId", settlementIdNew2);
												settlement2 = DomainManager.Organization.GetSettlement(settlementIdNew2);
												this.CallBaihuaMember(CS$<>8__locals1.context, false);
											}
										}
										else
										{
											this.CallBaihuaMember(CS$<>8__locals1.context, false);
										}
										CS$<>8__locals1.monthlyNotificationCollection.AddSectMainStoryBaihuaMelanoKills(settlement2.GetLocation());
									}
								}
								bool flag28 = DomainManager.Extra.IsExtraTaskInProgress(309);
								if (flag28)
								{
									short baihuaLeukoKillsMonthEventSettlementId = -1;
									CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementId", ref baihuaLeukoKillsMonthEventSettlementId);
									bool flag29 = DomainManager.Map.IsLocationInSettlementInfluenceRange(taiwu.GetLocation(), baihuaLeukoKillsMonthEventSettlementId);
									if (flag29)
									{
										int num3;
										bool flag30 = this.BaihuaGroupMeetCount(true, out num3) >= 1 && CS$<>8__locals1.context.Random.CheckPercentProb(this.<AdvanceMonth_SectMainStory_Baihua>g__GetAmbushProb|160_1(true, ref CS$<>8__locals1));
										if (flag30)
										{
											CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryBaihuaAmbushLeuko();
										}
									}
								}
								bool flag31 = DomainManager.Extra.IsExtraTaskInProgress(311);
								if (flag31)
								{
									short baihuaMelanoKillsMonthEventSettlementId = -1;
									CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementId", ref baihuaMelanoKillsMonthEventSettlementId);
									bool flag32 = DomainManager.Map.IsLocationInSettlementInfluenceRange(taiwu.GetLocation(), baihuaMelanoKillsMonthEventSettlementId);
									if (flag32)
									{
										int num3;
										bool flag33 = this.BaihuaGroupMeetCount(false, out num3) >= 1 && CS$<>8__locals1.context.Random.CheckPercentProb(this.<AdvanceMonth_SectMainStory_Baihua>g__GetAmbushProb|160_1(false, ref CS$<>8__locals1));
										if (flag33)
										{
											CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryBaihuaAmbushMelano();
										}
									}
								}
							}
							bool flag34 = baihuaRelationshipTaskInProgress;
							if (flag34)
							{
								bool flag35 = DomainManager.Extra.IsExtraTaskInProgress(313);
								if (flag35)
								{
									int date6 = int.MaxValue;
									bool flag36 = CS$<>8__locals1.argBox.Get("ConchShip_PresetKey_BaihuaAnimalsBackDate", ref date6) && this._currDate < date6 + 6;
									if (flag36)
									{
										this.<AdvanceMonth_SectMainStory_Baihua>g__TryTriggerPandemicStartTask|160_2(ref CS$<>8__locals1);
									}
									this.<AdvanceMonth_SectMainStory_Baihua>g__TryTriggerBaihuaManicLow|160_3(ref CS$<>8__locals1);
									this.<AdvanceMonth_SectMainStory_Baihua>g__TryTriggerBaihuaManicHigh|160_4(true, ref CS$<>8__locals1);
									this.<AdvanceMonth_SectMainStory_Baihua>g__TryTriggedLeukoMelanoPlay|160_5(ref CS$<>8__locals1);
									this.<AdvanceMonth_SectMainStory_Baihua>g__TryTriggedLMPlay|160_6(ref CS$<>8__locals1);
									bool flag37 = !CS$<>8__locals1.argBox.Contains<bool>("ConchShip_PresetKey_BaihuaLeukoAssistedMelano");
									if (flag37)
									{
										GameData.Domains.Character.Character leukoDeer2 = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(CS$<>8__locals1.context, 808);
										bool flag38 = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(leukoDeer2.GetId(), taiwu.GetId())) >= 5;
										if (flag38)
										{
											CS$<>8__locals1.monthlyNotificationCollection.AddSectMainStoryBaihuaLeukoHelps();
											DomainManager.Extra.TriggerExtraTask(CS$<>8__locals1.context, 51, 316);
											DomainManager.Extra.FinishTriggeredExtraTask(CS$<>8__locals1.context, 51, 315);
										}
									}
									bool flag39 = !CS$<>8__locals1.argBox.Contains<bool>("ConchShip_PresetKey_BaihuaMelanoAssistedLeuko");
									if (flag39)
									{
										GameData.Domains.Character.Character melanoOwl2 = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(CS$<>8__locals1.context, 809);
										bool flag40 = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(melanoOwl2.GetId(), taiwu.GetId())) >= 5;
										if (flag40)
										{
											CS$<>8__locals1.monthlyNotificationCollection.AddSectMainStoryBaihuaMelanoHelps();
											DomainManager.Extra.TriggerExtraTask(CS$<>8__locals1.context, 51, 324);
											DomainManager.Extra.FinishTriggeredExtraTask(CS$<>8__locals1.context, 51, 318);
										}
									}
								}
								bool flag41 = DomainManager.Extra.IsExtraTaskInProgress(316);
								if (flag41)
								{
									bool flag42 = this._currDate % 2 == 1 && DomainManager.Map.IsLocationInSettlementInfluenceRange(taiwu.GetLocation(), taiwuVillageSettlementId);
									if (flag42)
									{
										CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryBaihuaLeukoAssistsMelano();
									}
								}
								bool flag43 = DomainManager.Extra.IsExtraTaskInProgress(324);
								if (flag43)
								{
									bool flag44 = this._currDate % 2 == 0 && DomainManager.Map.IsLocationInSettlementInfluenceRange(taiwu.GetLocation(), taiwuVillageSettlementId);
									if (flag44)
									{
										CS$<>8__locals1.monthlyEventCollection.AddSectMainStoryBaihuaMelanoAssistsLeuko();
									}
								}
							}
							this.UpdateBaihuaManicCharacters(CS$<>8__locals1.context);
						}
					}
				}
			}
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x000E8604 File Offset: 0x000E6804
		private void AdvanceMonth_SectMainStory_Zhujian(DataContext context)
		{
			bool flag = this.GetSectMainStoryTaskStatus(9) != 0;
			if (!flag)
			{
				EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(9);
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				WorldDomain.<>c__DisplayClass161_0 CS$<>8__locals1;
				CS$<>8__locals1.taiwu = DomainManager.Taiwu.GetTaiwu();
				bool flag2 = this.CheckSectMainStoryAvailable(9) && DomainManager.Organization.GetSettlementByOrgTemplateId(9).CalcApprovingRate() >= 500 && !argBox.Contains<int>(SectMainStoryEventArgKeys.TriggeringStatus);
				if (flag2)
				{
					bool flag3 = WorldDomain.ZhujianMainStoryTrigger1();
					if (flag3)
					{
						monthlyEventCollection.AddSectMainStoryZhujianHeir(CS$<>8__locals1.taiwu.GetId(), CS$<>8__locals1.taiwu.GetLocation());
						return;
					}
				}
				ExtraDomain extraDomain = DomainManager.Extra;
				bool flag4 = extraDomain.IsExtraTaskInProgress(350);
				if (flag4)
				{
					Location taiwuLocation = CS$<>8__locals1.taiwu.GetLocation();
					bool flag5 = taiwuLocation.IsValid();
					if (flag5)
					{
						MapBlockData taiwuBlock = DomainManager.Map.GetBlock(taiwuLocation);
						bool flag6 = taiwuBlock.GetConfig().TemplateId == 27;
						if (flag6)
						{
							monthlyEventCollection.AddSectMainStoryZhujianHazyRain(CS$<>8__locals1.taiwu.GetId());
							return;
						}
					}
				}
				bool flag7 = extraDomain.IsExtraTaskInProgress(351);
				if (flag7)
				{
					Location location;
					bool flag8 = WorldDomain.<AdvanceMonth_SectMainStory_Zhujian>g__InTriggerLocation|161_0(out location, ref CS$<>8__locals1);
					if (flag8)
					{
						monthlyEventCollection.AddSectMainStoryZhujianTongshengSpeaks(CS$<>8__locals1.taiwu.GetId(), location);
						return;
					}
				}
				bool flag9 = extraDomain.IsExtraTaskInProgress(358);
				if (flag9)
				{
					Location location2;
					bool flag10 = WorldDomain.<AdvanceMonth_SectMainStory_Zhujian>g__InTriggerLocation|161_0(out location2, ref CS$<>8__locals1);
					if (flag10)
					{
						monthlyEventCollection.AddSectMainStoryZhujianHuichuntang(CS$<>8__locals1.taiwu.GetId(), location2);
						return;
					}
				}
				int date = int.MaxValue;
				bool flag11 = argBox.Get("ConchShip_PresetKey_SectMainStoryZhujianProsperousEndDate", ref date) && this._currDate >= date + 1;
				if (flag11)
				{
					Location location3;
					bool flag12 = WorldDomain.<AdvanceMonth_SectMainStory_Zhujian>g__InTriggerLocation|161_0(out location3, ref CS$<>8__locals1);
					if (flag12)
					{
						monthlyEventCollection.AddSectMainStoryZhujianProsperous();
						return;
					}
				}
				date = int.MaxValue;
				bool flag13 = argBox.Get("ConchShip_PresetKey_SectMainStoryZhujianFailingEndDate", ref date) && this._currDate >= date + 1;
				if (flag13)
				{
					Location location4;
					bool flag14 = WorldDomain.<AdvanceMonth_SectMainStory_Zhujian>g__InTriggerLocation|161_0(out location4, ref CS$<>8__locals1);
					if (flag14)
					{
						monthlyEventCollection.AddSectMainStoryZhujianFailing();
					}
				}
			}
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x000E883C File Offset: 0x000E6A3C
		private void AdvanceMonth_SectMainStoryFulong(DataContext context)
		{
			EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(14);
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Location taiwuLocation = taiwu.GetLocation();
			short fulongSettlementId = DomainManager.Organization.GetSettlementByOrgTemplateId(14).GetId();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			int date = int.MaxValue;
			bool flag = argBox.Get("ConchShip_PresetKey_SectMainStoryFulongProsperousEndDate", ref date) && this._currDate >= date + 1;
			if (flag)
			{
				monthlyEventCollection.AddSectMainStoryFulongProsperous();
			}
			else
			{
				date = int.MaxValue;
				bool flag2 = argBox.Get("ConchShip_PresetKey_SectMainStoryFulongFailingEndDate", ref date) && this._currDate >= date + 1;
				if (flag2)
				{
					monthlyEventCollection.AddSectMainStoryFulongFailing();
				}
				else
				{
					bool fulongDisasterStart = false;
					argBox.Get("ConchShip_PresetKey_FulongDisasterStart", ref fulongDisasterStart);
					bool flag3 = (!argBox.Contains<bool>("ConchShip_PresetKey_FulongDisasterStart") && !argBox.Contains<int>(SectMainStoryEventArgKeys.TriggeringStatus) && this.FulongDisasterStart()) || fulongDisasterStart;
					if (flag3)
					{
						int prob = 30;
						bool flag4 = argBox.Contains<int>("ConchShip_PresetKey_FulongDisasterStartProb");
						if (flag4)
						{
							argBox.Get("ConchShip_PresetKey_FulongDisasterStartProb", ref prob);
						}
						bool taiwuAtChimingdao = taiwuLocation.AreaId == DomainManager.Map.GetAreaIdByAreaTemplateId(29);
						bool flag5 = taiwuAtChimingdao;
						if (flag5)
						{
							prob += 30;
						}
						else
						{
							prob += 15;
						}
						bool flag6 = context.Random.CheckPercentProb(prob);
						if (flag6)
						{
							prob -= 45;
							this.FulongTriggerDisaster(context);
							monthlyNotificationCollection.AddSectMainStoryFulongSacrifice();
							bool flag7 = taiwuAtChimingdao;
							if (flag7)
							{
								monthlyEventCollection.AddSectMainStoryFulongDiasterAppear();
							}
						}
						DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, 14, "ConchShip_PresetKey_FulongDisasterStartProb", prob);
					}
					int fulongTaskInProgress = DomainManager.Extra.GetExtraTaskChainCurrentTask(52);
					int num = fulongTaskInProgress;
					int num2 = num;
					if (num2 - 333 <= 4)
					{
						int date2 = int.MaxValue;
						bool flag8 = argBox.Get("ConchShip_PresetKey_FulongMessengerAppearTime", ref date2) && this._currDate >= date2 + SectMainStoryRelatedConstants.FulongZealotStartRobTime;
						if (flag8)
						{
							DomainManager.Extra.ApplyFulongOutLawAdvanceMonth(context, date2);
						}
					}
					switch (fulongTaskInProgress)
					{
					case 330:
					{
						int fulongAdventureOneCountDown = 0;
						argBox.Get("ConchShip_PresetKey_FulongAdventureOneCountDown", ref fulongAdventureOneCountDown);
						DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, 14, "ConchShip_PresetKey_FulongAdventureOneCountDown", --fulongAdventureOneCountDown);
						break;
					}
					case 331:
					{
						int date3 = int.MaxValue;
						argBox.Get("ConchShip_PresetKey_FulongFireStartTime", ref date3);
						bool flag9 = this._currDate > date3 && (this._currDate - date3) % 3 == 0;
						if (flag9)
						{
							monthlyEventCollection.AddSectMainStoryFulongAftermath();
						}
						break;
					}
					case 333:
					{
						Location location = taiwu.GetValidLocation();
						bool atFulong = DomainManager.Map.IsLocationInSettlementInfluenceRange(location, fulongSettlementId);
						bool flag10 = atFulong;
						if (flag10)
						{
							monthlyEventCollection.AddSectMainStoryFulongShadow();
						}
						break;
					}
					case 338:
					{
						int fulongAdventureTwoTaiwuId = -1;
						argBox.Get("ConchShip_PresetKey_FulongAdventureTwoTaiwuId", ref fulongAdventureTwoTaiwuId);
						bool flag11 = fulongAdventureTwoTaiwuId != taiwu.GetId();
						if (flag11)
						{
							monthlyEventCollection.AddSectMainStoryFulongLazuliLetter();
						}
						GameData.Utilities.ShortList list = argBox.Get<GameData.Utilities.ShortList>("ConchShip_PresetKey_FulongChickenFeatherDropList");
						List<short> items = list.Items;
						bool flag12 = items != null && items.Count > 0;
						if (flag12)
						{
							foreach (short chickenTemplateId in list.Items)
							{
								monthlyNotificationCollection.AddSectMainStoryFulongFeatherDrop(chickenTemplateId);
							}
							list.Items.Clear();
							DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<GameData.Utilities.ShortList>(context, 14, "ConchShip_PresetKey_FulongChickenFeatherDropList", list);
						}
						break;
					}
					case 340:
					{
						int fulongAdventureThreeCountDown = 0;
						argBox.Get("ConchShip_PresetKey_FulongAdventureThreeCountDown", ref fulongAdventureThreeCountDown);
						DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, 14, "ConchShip_PresetKey_FulongAdventureThreeCountDown", --fulongAdventureThreeCountDown);
						break;
					}
					case 343:
					case 346:
					{
						DomainManager.Extra.ApplyFulongInFlameAreaAdvanceMonth(context);
						int date4 = int.MaxValue;
						bool flag13 = argBox.Get("ConchShip_PresetKey_FulongFireStartTime", ref date4) && this._currDate >= date4 + GlobalConfig.Instance.FulongFlameExtinguishTime;
						if (flag13)
						{
							monthlyNotificationCollection.AddSectMainStoryFulongFireVanishes();
							List<FulongInFlameArea> fireAreas = DomainManager.Extra.GetAllFulongInFlameAreas();
							int count = fireAreas.Count;
							for (int i = 0; i < count; i++)
							{
								DomainManager.Extra.ApplyFulongInFlameAreaFullyExtinguished(context, 0, false);
							}
						}
						else
						{
							bool putOutFire = false;
							argBox.Get("ConchShip_PresetKey_FulongPutOutFire", ref putOutFire);
							bool flag14 = putOutFire && DomainManager.Extra.GetAllFulongInFlameAreas().Count > 0;
							if (flag14)
							{
								monthlyEventCollection.AddSectMainStoryFulongFireFighting(taiwu.GetId());
							}
						}
						break;
					}
					case 344:
					{
						int level = 3;
						argBox.Get("ConchShip_PresetKey_FulongLazuliFindFlowerDialogLevel", ref level);
						bool flag15 = level >= 6;
						if (flag15)
						{
							int date5 = int.MaxValue;
							bool flag16 = argBox.Get("ConchShip_PresetKey_FulongStayWithLazuliTaskTriggerDate", ref date5) && this._currDate >= date5 + 3;
							if (flag16)
							{
								Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
								MapBlockData villageBlockData = DomainManager.Map.GetBlockData(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId);
								MapBlockData taiwuLocationBlockData = DomainManager.Map.GetBlockData(taiwu.GetValidLocation().AreaId, taiwu.GetValidLocation().BlockId);
								ByteCoordinate taiwuBlockPos = taiwuLocationBlockData.GetBlockPos();
								bool flag17 = villageBlockData.GetManhattanDistanceToPos(taiwuBlockPos.X, taiwuBlockPos.Y) <= 3;
								if (flag17)
								{
									DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<bool>(context, 14, "ConchShip_PresetKey_FulongTravelWithLazuliFinished", true);
								}
							}
						}
						break;
					}
					}
				}
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x000E8E1C File Offset: 0x000E701C
		private void UpdateWuxianParanoiaCharacters(DataContext context)
		{
			List<GameData.Domains.Character.Character> paranoiaCharacters = new List<GameData.Domains.Character.Character>();
			List<int> potentialTargetIds = new List<int>();
			MapCharacterFilter.ParallelFind(new Predicate<GameData.Domains.Character.Character>(WorldDomain.<UpdateWuxianParanoiaCharacters>g__ShouldAttackRandomTarget|163_0), paranoiaCharacters, 0, 135, false);
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Location taiwuLocation = taiwuChar.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
			bool flag = paranoiaCharacters.Count < 7 && this.GetSectMainStoryTaskStatus(12) == 2 && context.Random.CheckPercentProb(25);
			if (flag)
			{
				Settlement sect = DomainManager.Organization.GetSettlementByOrgTemplateId(12);
				List<int> charIdList = context.AdvanceMonthRelatedData.CharIdList.Occupy();
				sect.GetMembers().GetAllMembers(charIdList);
				int maxAttainment = int.MinValue;
				foreach (int charId in charIdList)
				{
					GameData.Domains.Character.Character character;
					bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out character) || character.GetHappinessType() == 6 || character.GetFeatureIds().Contains(486);
					if (!flag2)
					{
						short attainment = character.GetLifeSkillAttainment(9);
						bool flag3 = (int)attainment > maxAttainment;
						if (flag3)
						{
							maxAttainment = (int)attainment;
							potentialTargetIds.Clear();
							potentialTargetIds.Add(charId);
						}
						else
						{
							bool flag4 = (int)attainment == maxAttainment;
							if (flag4)
							{
								potentialTargetIds.Add(charId);
							}
						}
					}
				}
				context.AdvanceMonthRelatedData.CharIdList.Release(ref charIdList);
				bool flag5 = potentialTargetIds.Count > 0;
				if (flag5)
				{
					int selectedCharId = potentialTargetIds.GetRandom(context.Random);
					GameData.Domains.Character.Character selectedChar = DomainManager.Character.GetElement_Objects(selectedCharId);
					selectedChar.AddFeature(context, 486, false);
					lifeRecordCollection.AddWuxianParanoiaAdded(selectedCharId, currDate, selectedChar.GetLocation());
					monthlyNotifications.AddSectMainStoryWuxianParanoiaAppeared(selectedCharId);
				}
			}
			foreach (GameData.Domains.Character.Character character2 in paranoiaCharacters)
			{
				int charId2 = character2.GetId();
				Location location = character2.GetLocation();
				bool flag6 = !DomainManager.Character.IsCharacterAlive(charId2);
				if (!flag6)
				{
					bool flag7 = character2.GetHappinessType() == 6;
					if (flag7)
					{
						character2.RemoveFeature(context, 486);
						lifeRecordCollection.AddWuxianParanoiaErased(charId2, currDate, location);
					}
					else
					{
						bool flag8 = !location.IsValid();
						if (!flag8)
						{
							bool flag9 = !character2.IsInteractableAsIntelligentCharacter();
							if (!flag9)
							{
								bool flag10 = context.Random.NextBool();
								if (!flag10)
								{
									bool flag11 = location == taiwuLocation;
									if (flag11)
									{
										lifeRecordCollection.AddWuxianParanoiaAttack(charId2, currDate, taiwuChar.GetId(), location);
										monthlyEventCollection.AddSectMainStoryWuxianAssault(charId2, location);
										DomainManager.Character.HandleAttackAction(context, character2, taiwuChar);
									}
									else
									{
										character2.GetPotentialHarmfulActionTargets(potentialTargetIds);
										bool flag12 = potentialTargetIds.Count == 0;
										if (!flag12)
										{
											int selectedCharId2 = potentialTargetIds.GetRandom(context.Random);
											GameData.Domains.Character.Character selectedChar2 = DomainManager.Character.GetElement_Objects(selectedCharId2);
											lifeRecordCollection.AddWuxianParanoiaAttack(charId2, currDate, selectedCharId2, location);
											DomainManager.Character.HandleAttackAction(context, character2, selectedChar2);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x000E91B4 File Offset: 0x000E73B4
		private void UpdateBaihuaManicCharacters(DataContext context)
		{
			List<int> manicCharList = DomainManager.World.BaihuaGetSpecialDebuffIntList().Items;
			bool flag = manicCharList == null || manicCharList.Count <= 0;
			if (!flag)
			{
				List<int> potentialTargetIds = ObjectPool<List<int>>.Instance.Get();
				GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
				Location taiwuLocation = taiwuChar.GetLocation();
				int currDate = DomainManager.World.GetCurrDate();
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				foreach (int charId in manicCharList)
				{
					GameData.Domains.Character.Character character;
					bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
					if (!flag2)
					{
						Location location = character.GetLocation();
						bool flag3 = !location.IsValid() || !character.IsInteractableAsIntelligentCharacter();
						if (!flag3)
						{
							bool flag4 = DomainManager.Character.IsNotManicCharacter(character);
							if (!flag4)
							{
								bool flag5 = context.Random.NextBool();
								if (!flag5)
								{
									bool flag6 = location == taiwuLocation;
									if (flag6)
									{
										lifeRecordCollection.AddSectMainStoryBaihuaManiaAttack(charId, currDate, taiwuChar.GetId(), location);
										monthlyEventCollection.AddSectMainStoryBaihuaManicAttack(charId, location);
										DomainManager.Character.HandleAttackAction(context, character, taiwuChar);
									}
									else
									{
										character.GetPotentialHarmfulActionTargets(potentialTargetIds);
										bool flag7 = potentialTargetIds.Count == 0;
										if (!flag7)
										{
											int selectedCharId = potentialTargetIds.GetRandom(context.Random);
											GameData.Domains.Character.Character selectedChar = DomainManager.Character.GetElement_Objects(selectedCharId);
											lifeRecordCollection.AddSectMainStoryBaihuaManiaAttack(charId, currDate, selectedCharId, location);
											DomainManager.Character.HandleAttackAction(context, character, selectedChar);
										}
									}
								}
							}
						}
					}
				}
				ObjectPool<List<int>>.Instance.Return(potentialTargetIds);
			}
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x000E9398 File Offset: 0x000E7598
		private unsafe bool AreaHasAdultGraveOfTargetOrganization(short areaId, sbyte orgTemplateId)
		{
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
			Span<MapBlockData> span = areaBlocks;
			for (int i = 0; i < span.Length; i++)
			{
				MapBlockData mapBlockData = *span[i];
				bool flag = mapBlockData.GraveSet == null || mapBlockData.GraveSet.Count <= 0;
				if (!flag)
				{
					foreach (int graveId in mapBlockData.GraveSet)
					{
						Grave grave;
						DomainManager.Character.TryGetElement_Graves(graveId, out grave);
						DeadCharacter deadCharacter = DomainManager.Character.GetDeadCharacter(grave.GetId());
						bool flag2 = deadCharacter.OrganizationInfo.OrgTemplateId == orgTemplateId && deadCharacter.GetActualAge() >= 16;
						if (flag2)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x000E949C File Offset: 0x000E769C
		internal void ShixiangQueryEnemyLocations(DataContext context, out short areaId, out List<short> blockIds)
		{
			Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(6);
			Location settlementLocation = settlement.GetLocation();
			areaId = settlementLocation.AreaId;
			List<MapBlockData> blocks = context.AdvanceMonthRelatedData.Blocks.Occupy();
			DomainManager.Map.QueryRegularBelongBlocks(blocks, settlementLocation, true, Array.Empty<MapDomain.MapBlockDataFilter>());
			blockIds = new List<short>();
			foreach (MapBlockData blockData in blocks)
			{
				HashSet<int> enemyCharacterSet = blockData.EnemyCharacterSet;
				bool flag = enemyCharacterSet == null || enemyCharacterSet.Count <= 0;
				if (!flag)
				{
					foreach (int enemyCharId in blockData.EnemyCharacterSet)
					{
						GameData.Domains.Character.Character enemyChar;
						bool flag2 = !DomainManager.Character.TryGetElement_Objects(enemyCharId, out enemyChar);
						if (!flag2)
						{
							short templateId = enemyChar.GetTemplateId();
							bool flag3 = templateId < 608 || templateId > 617;
							bool flag4 = flag3;
							if (!flag4)
							{
								blockIds.Add(blockData.BlockId);
								break;
							}
						}
					}
				}
			}
			context.AdvanceMonthRelatedData.Blocks.Release(ref blocks);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x000E960C File Offset: 0x000E780C
		private void FindShixiangLeader()
		{
			Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(6);
			GameData.Domains.Character.Character leader = settlement.GetLeader();
			bool flag = leader == null;
			if (flag)
			{
			}
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x000E9638 File Offset: 0x000E7838
		public void JixiGrowUp(DataContext context, short oldCharTemplateId, short newCharTemplateId)
		{
			GameData.Domains.Character.Character newChar = DomainManager.Character.ReplaceFixedCharacter(context, oldCharTemplateId, newCharTemplateId);
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(15);
			bool hasFox = false;
			bool flag = sectArgBox.Get("ConchShip_PresetKey_JixiHasAntiqueJadeFox", ref hasFox) && hasFox;
			if (flag)
			{
				ItemKey fox = DomainManager.Item.CreateItem(context, 12, 244);
				newChar.AddInventoryItem(context, fox, 1, false);
			}
			bool hasBat = false;
			bool flag2 = sectArgBox.Get("ConchShip_PresetKey_JixiHasAntiqueJadeBat", ref hasBat) && hasBat;
			if (flag2)
			{
				ItemKey bat = DomainManager.Item.CreateItem(context, 12, 243);
				newChar.AddInventoryItem(context, bat, 1, false);
			}
			bool hasButterfly = false;
			bool flag3 = sectArgBox.Get("ConchShip_PresetKey_JixiHasAntiqueJadeButterfly", ref hasButterfly) && hasButterfly;
			if (flag3)
			{
				ItemKey butterfly = DomainManager.Item.CreateItem(context, 12, 245);
				newChar.AddInventoryItem(context, butterfly, 1, false);
			}
			bool flag4 = newCharTemplateId == 539;
			if (flag4)
			{
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, 15, "ConchShip_PresetKey_SectStory_Xuehou_Jixi_KilledCount", 14);
			}
			bool flag5 = newCharTemplateId == 538;
			if (flag5)
			{
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, 15, "ConchShip_PresetKey_SectStory_Xuehou_Jixi_KilledCount", 7);
			}
			bool flag6 = newCharTemplateId == 537;
			if (flag6)
			{
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, 15, "ConchShip_PresetKey_SectStory_Xuehou_Jixi_KilledCount", 0);
			}
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x000E9778 File Offset: 0x000E7978
		public bool JixiAdventurePass(sbyte index, int overTime)
		{
			string passKey = string.Empty;
			bool flag = index == 1;
			if (flag)
			{
				passKey = "ConchShip_PresetKey_JixiAdventureOnePassDate";
			}
			else
			{
				bool flag2 = index == 2;
				if (flag2)
				{
					passKey = "ConchShip_PresetKey_JixiAdventureTwoPassDate";
				}
				else
				{
					bool flag3 = index == 3;
					if (flag3)
					{
						passKey = "ConchShip_PresetKey_JixiAdventureThreePassDate";
					}
				}
			}
			Tester.Assert(passKey != string.Empty, "");
			EventArgBox eventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(15);
			int passDate = int.MaxValue;
			bool flag4 = eventArgBox.Get(passKey, ref passDate);
			bool result;
			if (flag4)
			{
				int currDate = DomainManager.World.GetCurrDate();
				result = (currDate >= passDate + overTime);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x000E9820 File Offset: 0x000E7A20
		public bool JixiAdventureDisappear(sbyte index, int overTime = 9)
		{
			string startKey = string.Empty;
			bool flag = index == 1;
			if (flag)
			{
				startKey = "ConchShip_PresetKey_JixiAdventureOneStartDate";
			}
			else
			{
				bool flag2 = index == 2;
				if (flag2)
				{
					startKey = "ConchShip_PresetKey_JixiAdventureTwoStartDate";
				}
				else
				{
					bool flag3 = index == 3;
					if (flag3)
					{
						startKey = "ConchShip_PresetKey_JixiAdventureThreeStartDate";
					}
				}
			}
			Tester.Assert(startKey != string.Empty, "");
			EventArgBox eventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(15);
			int startDate = int.MaxValue;
			bool flag4 = eventArgBox.Get(startKey, ref startDate);
			bool result;
			if (flag4)
			{
				int currDate = DomainManager.World.GetCurrDate();
				result = (currDate >= startDate + overTime);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x000E98C8 File Offset: 0x000E7AC8
		public sbyte GetJixiFavorabilityType()
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			GameData.Domains.Character.Character jixi = this.TryGetJixi();
			bool flag = jixi == null;
			sbyte result;
			if (flag)
			{
				result = sbyte.MinValue;
			}
			else
			{
				result = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(jixi.GetId(), taiwu.GetId()));
			}
			return result;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x000E9914 File Offset: 0x000E7B14
		public GameData.Domains.Character.Character TryGetJixi()
		{
			GameData.Domains.Character.Character jixiAdult;
			bool flag = DomainManager.Character.TryGetFixedCharacterByTemplateId(539, out jixiAdult);
			GameData.Domains.Character.Character result;
			if (flag)
			{
				result = jixiAdult;
			}
			else
			{
				GameData.Domains.Character.Character jixiBaby;
				bool flag2 = DomainManager.Character.TryGetFixedCharacterByTemplateId(537, out jixiBaby);
				if (flag2)
				{
					result = jixiBaby;
				}
				else
				{
					GameData.Domains.Character.Character jixiYoung;
					bool flag3 = DomainManager.Character.TryGetFixedCharacterByTemplateId(538, out jixiYoung);
					if (flag3)
					{
						result = jixiYoung;
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x000E9980 File Offset: 0x000E7B80
		public void DealSectMainStoryEnd(DataContext context, sbyte orgTemplateId, sbyte endState, int time)
		{
			Tester.Assert(endState != 0, "");
			this.SetSectMainStoryTaskStatus(context, orgTemplateId, endState);
			if (orgTemplateId == 15)
			{
				bool flag = endState == 1;
				if (flag)
				{
					DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, orgTemplateId, "ConchShip_PresetKey_SectMainStoryXuehouProsperousEndDate", time);
				}
				else
				{
					bool flag2 = endState == 2;
					if (flag2)
					{
						DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, orgTemplateId, "ConchShip_PresetKey_SectMainStoryXuehouFailingEndDate", time);
					}
				}
				DomainManager.Extra.FinishAllTaskInChain(context, 28);
				DomainManager.Extra.FinishAllTaskInChain(context, 29);
			}
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x000E9A10 File Offset: 0x000E7C10
		public bool IsJixiFree()
		{
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(15);
			return DomainManager.World.GetSectMainStoryTaskStatus(15) == 1 || sectArgBox.Contains<int>("ConchShip_PresetKey_SectMainStoryXuehouProsperousEndDate");
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x000E9A4C File Offset: 0x000E7C4C
		public void ShaolinGetSutraBooks(DataContext context, sbyte beginGrade, sbyte endGrade, Action<ItemKey> onGeneratedBook)
		{
			short[] buddhismLifeSkillTemplateIds = Config.LifeSkillType.Instance[13].SkillList;
			for (sbyte i = beginGrade; i <= endGrade; i += 1)
			{
				short lifeSkillTemplateId = buddhismLifeSkillTemplateIds[(int)i];
				short bookId = LifeSkill.Instance[lifeSkillTemplateId].SkillBookId;
				ItemKey itemKey = DomainManager.Item.CreateSkillBook(context, bookId, 5, -1, -1, 50, true);
				SkillBookItem bookConfig = Config.SkillBook.Instance[bookId];
				GameData.Domains.Item.SkillBook skillBook;
				bool flag = bookConfig.Grade == 8 && DomainManager.Item.TryGetElement_SkillBooks(itemKey.Id, out skillBook);
				if (flag)
				{
					skillBook.SetMaxDurability(15, context);
					skillBook.SetCurrDurability(15, context);
				}
				onGeneratedBook(itemKey);
			}
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<short>(context, 1, "ConchShip_PresetKey_ShaolinReadingMaxGradeSutra", buddhismLifeSkillTemplateIds[(int)endGrade]);
			DomainManager.Extra.TriggerExtraTask(context, 30, 228);
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x000E9B30 File Offset: 0x000E7D30
		[DomainMethod]
		public bool EmeiTransferBonusProgress(DataContext context, short bonusTemplateId, List<ItemKey> itemKeys)
		{
			SectEmeiBreakBonusData data;
			bool flag = !DomainManager.Extra.TryGetElement_SectEmeiBreakBonusData(bonusTemplateId, out data);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int progress = SectMainStorySharedMethods.CalcEmeiBonusItemProgress(bonusTemplateId, itemKeys);
				DomainManager.Taiwu.RemoveItemList(context, itemKeys, ItemSourceType.Inventory, true);
				data.OfflineAddProgress(progress);
				DomainManager.Extra.SectEmeiSetBonus(context, bonusTemplateId, data);
				result = true;
			}
			return result;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x000E9B8C File Offset: 0x000E7D8C
		public void GetEmeiPotentialVictims(GameData.Domains.Character.Character selfChar, out List<int> charIds)
		{
			Location location = selfChar.GetLocation();
			List<MapBlockData> blocks = new List<MapBlockData>();
			int selfId = selfChar.GetId();
			sbyte grade = selfChar.GetOrganizationInfo().Grade;
			CharacterMatcherItem matcher = CharacterMatcher.DefValue.EmeiPotentialVictims;
			charIds = new List<int>();
			DomainManager.Map.GetNeighborBlocks(location.AreaId, location.BlockId, blocks, 3);
			foreach (MapBlockData blockData in blocks)
			{
				HashSet<int> characterSet = blockData.CharacterSet;
				bool flag = characterSet == null || characterSet.Count <= 0;
				if (!flag)
				{
					foreach (int id in blockData.CharacterSet)
					{
						bool flag2 = id == selfId;
						if (!flag2)
						{
							GameData.Domains.Character.Character victim;
							bool flag3 = !DomainManager.Character.TryGetElement_Objects(id, out victim);
							if (!flag3)
							{
								bool flag4 = !matcher.Match(victim);
								if (!flag4)
								{
									OrganizationInfo victimOrgInfo = victim.GetOrganizationInfo();
									bool flag5 = victimOrgInfo.Grade > grade;
									if (!flag5)
									{
										charIds.Add(id);
										break;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x000E9CF4 File Offset: 0x000E7EF4
		public void AddEmeiExp(DataContext context, int expFromCombat)
		{
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(2);
			bool flag = !sectArgBox.GetBool("ConchShip_PresetKey_EmeiShiHoujiuFollowOpen") && !sectArgBox.GetBool("ConchShip_PresetKey_EmeiWhiteGibbonFollowOpen");
			if (!flag)
			{
				bool oldMeet = this.GetEmeiCostExpEnough();
				long exp = DomainManager.Extra.GetSectEmeiExp();
				exp += (long)((int)Math.Ceiling((double)expFromCombat / 4.0));
				DomainManager.Extra.SetSectEmeiExp(exp, context);
				bool newMeet = this.GetEmeiCostExpEnough();
				bool flag2 = !oldMeet && newMeet;
				if (flag2)
				{
					int charId = this.GetEmeiExpCharacterId();
					InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
					instantNotificationCollection.AddDuChuangYiGeReady(charId);
				}
			}
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x000E9D9C File Offset: 0x000E7F9C
		public bool GetEmeiCostExpEnough()
		{
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(2);
			bool @bool = sectArgBox.GetBool("ConchShip_PresetKey_EmeiBreakBonusSaved");
			return !@bool || true;
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x000E9DD0 File Offset: 0x000E7FD0
		public int GetEmeiExpCharacterId()
		{
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(2);
			bool emeiShiHoujiuFollowOpen = sectArgBox.GetBool("ConchShip_PresetKey_EmeiShiHoujiuFollowOpen");
			bool emeiWhiteGibbonFollowOpen = sectArgBox.GetBool("ConchShip_PresetKey_EmeiWhiteGibbonFollowOpen");
			bool flag = !emeiShiHoujiuFollowOpen && !emeiWhiteGibbonFollowOpen;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				short charTemplateId = emeiShiHoujiuFollowOpen ? 637 : 679;
				result = DomainManager.Character.GetFixedCharacterIdByTemplateId(charTemplateId);
			}
			return result;
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x000E9E38 File Offset: 0x000E8038
		[DomainMethod]
		public ItemKey RefiningWugKing(DataContext context)
		{
			List<int> costPoisons = ObjectPool<List<int>>.Instance.Get();
			SectWuxianWugJugData jugData = DomainManager.Extra.GetSectWuxianWugJugPoisons();
			sbyte wugKingType = SectMainStorySharedMethods.CalcWugKingType(costPoisons, jugData);
			ItemKey wugKingItemKey = ItemKey.Invalid;
			bool flag = wugKingType < 0;
			if (flag)
			{
				List<short> weights = ObjectPool<List<short>>.Instance.Get();
				foreach (WugKingItem wugKing in ((IEnumerable<WugKingItem>)WugKing.Instance))
				{
					weights.Add(wugKing.RefiningWeight);
				}
				wugKingType = (sbyte)RandomUtils.GetRandomIndex(weights, context.Random);
				ObjectPool<List<short>>.Instance.Return(weights);
			}
			bool flag2 = costPoisons.Sum() > 0;
			if (flag2)
			{
				for (sbyte i = 0; i < 6; i += 1)
				{
					jugData.ReducePoison(i, costPoisons[(int)i]);
				}
				jugData.UpdateRefiningDate();
				WugKingItem wugKingConfig = WugKing.Instance[wugKingType];
				wugKingItemKey = DomainManager.Item.CreateMedicine(context, wugKingConfig.WugMedicine);
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				taiwu.GetInventory().OfflineAdd(wugKingItemKey, 1);
				taiwu.SetInventory(taiwu.GetInventory(), context);
			}
			DomainManager.Extra.SetSectWuxianWugJugPoisons(jugData, context);
			ObjectPool<List<int>>.Instance.Return(costPoisons);
			return wugKingItemKey;
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x000E9F98 File Offset: 0x000E8198
		[DomainMethod]
		public unsafe bool DropPoisonsToWugJug(DataContext context, List<ItemKey> poisonMaterials)
		{
			bool flag = poisonMaterials == null || poisonMaterials.Count <= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				Inventory inventory = taiwu.GetInventory();
				foreach (ItemKey material in poisonMaterials)
				{
					bool flag2 = !inventory.Items.ContainsKey(material);
					if (flag2)
					{
						return false;
					}
					bool flag3 = !SectMainStorySharedMethods.CalcDropPoisonValue(material).IsNonZero();
					if (flag3)
					{
						return false;
					}
				}
				SectWuxianWugJugData jugData = DomainManager.Extra.GetSectWuxianWugJugPoisons();
				PoisonInts addPoisons = SectMainStorySharedMethods.CalcDropPoisonValue(jugData, poisonMaterials);
				taiwu.RemoveInventoryItemList(context, poisonMaterials, true);
				for (sbyte i = 0; i < 6; i += 1)
				{
					jugData.AddPoison(i, *addPoisons[(int)i]);
				}
				DomainManager.Extra.SetSectWuxianWugJugPoisons(jugData, context);
				result = true;
			}
			return result;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x000EA0B0 File Offset: 0x000E82B0
		public bool TryGetPrologueAddedWug(out int wugAdded)
		{
			wugAdded = -1;
			return DomainManager.Extra.GetSectMainStoryEventArgBox(12).Get("ConchShip_PresetKey_Wuxian_Prologue_AddedWug", ref wugAdded);
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x000EA0DC File Offset: 0x000E82DC
		public int GetWuxianChapterOneWishComeTrueCount()
		{
			int count = 0;
			return DomainManager.Extra.GetSectMainStoryEventArgBox(12).Get("ConchShip_PresetKey_Wuxian_Chapter1_WishComeTrueCount", ref count) ? count : 0;
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x000EA110 File Offset: 0x000E8310
		public int GetWuxianChapterOneWishCount()
		{
			int count = 0;
			return DomainManager.Extra.GetSectMainStoryEventArgBox(12).Get("ConchShip_PresetKey_Wuxian_Chapter1_WuxianChapter1WishCount", ref count) ? count : 0;
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x000EA144 File Offset: 0x000E8344
		public int GetWuxianHappyEndingEventDate()
		{
			int date = 0;
			return DomainManager.Extra.GetSectMainStoryEventArgBox(12).Get("ConchShip_PresetKey_Wuxian_Chapter4_HappyEndingEventDate", ref date) ? date : 0;
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x000EA178 File Offset: 0x000E8378
		public static bool IsAbleToTriggerWuxianChapterThreeMail()
		{
			EventArgBox permanentArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(12);
			bool canStart = false;
			int count = 0;
			return permanentArgBox.Get("ConchShip_PresetKey_Wuxian_Chapter3_AbleToStart", ref canStart) && canStart && (permanentArgBox.Get("ConchShip_PresetKey_Wuxian_Chapter3_MailReceivedCount", ref count) ? count : 0) < 3;
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x000EA1C8 File Offset: 0x000E83C8
		public bool IsWuxianFinalBossBeaten()
		{
			EventArgBox permanentArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(12);
			bool isBeaten = false;
			return permanentArgBox.Get("ConchShip_PresetKey_Wuxian_Chapter4_FinalBossBeaten", ref isBeaten) && isBeaten;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x000EA1F8 File Offset: 0x000E83F8
		public bool IsWuxianTaiwuChanged()
		{
			int taiwuId = -1;
			return DomainManager.Extra.GetSectMainStoryEventArgBox(12).Get("ConchShip_PresetKey_Wuxian_Prologue_TaiwuId", ref taiwuId) && taiwuId != DomainManager.Taiwu.GetTaiwuCharId();
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x000EA23C File Offset: 0x000E843C
		public bool IsWuxianPrologueWugAttackedOnce()
		{
			bool attacked = false;
			return DomainManager.Extra.GetSectMainStoryEventArgBox(12).Get("ConchShip_PresetKey_Wuxian_Prologue_WugAttacked", ref attacked) && attacked;
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x000EA26C File Offset: 0x000E846C
		public void WuxianEndingProsperous(DataContext context)
		{
			EventArgBox permanentArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(12);
			permanentArgBox.Set("ConchShip_PresetKey_SectMainStoryWuxianProsperousEndDate", DomainManager.World.GetCurrDate() + 1);
			permanentArgBox.Set("ConchShip_PresetKey_Wuxian_Chapter4_HappyEndingEventDate", DomainManager.World.GetCurrDate() + 3);
			permanentArgBox.Set("ConchShip_PresetKey_Wuxian_Chapter4_FinalBossBeaten", true);
			permanentArgBox.Set("ConchShip_PresetKey_Wuxian_Chapter4_AdventureComplete", false);
			DomainManager.Extra.SaveSectMainStoryEventArgumentBox(context, 12);
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x000EA2E0 File Offset: 0x000E84E0
		public void WuxianEndingFailing0(DataContext context)
		{
			EventArgBox permanentArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(12);
			permanentArgBox.Set("ConchShip_PresetKey_SectMainStoryWuxianFailingEndDate", DomainManager.World.GetCurrDate() + 3);
			DomainManager.Extra.SaveSectMainStoryEventArgumentBox(context, 12);
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x000EA324 File Offset: 0x000E8524
		public void WuxianEndingFailing1(DataContext context, bool isComplete)
		{
			EventArgBox permanentArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(12);
			permanentArgBox.Set("ConchShip_PresetKey_SectMainStoryWuxianFailingEndDate", DomainManager.World.GetCurrDate() + 1);
			permanentArgBox.Set("ConchShip_PresetKey_Wuxian_Chapter4_FinalBossBeaten", false);
			permanentArgBox.Set("ConchShip_PresetKey_Wuxian_Chapter4_AdventureComplete", isComplete);
			if (isComplete)
			{
				permanentArgBox.Set("ConchShip_PresetKey_Wuxian_Chapter4_HappyEndingEventDate", DomainManager.World.GetCurrDate() + 3);
			}
			DomainManager.Extra.SaveSectMainStoryEventArgumentBox(context, 12);
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x000EA39C File Offset: 0x000E859C
		public bool IsWuxianEndingEventTriggered()
		{
			bool triggered = false;
			return DomainManager.Extra.GetSectMainStoryEventArgBox(12).Get("ConchShip_PresetKey_Wuxian_Chapter4_EndingEventTriggered", ref triggered) && triggered;
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x000EA3CC File Offset: 0x000E85CC
		public bool JingangWorldStateCheck()
		{
			return DomainManager.Building.IsTaiwuVillageHaveSpecifyBuilding(50, true);
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x000EA3EC File Offset: 0x000E85EC
		public bool JingangMonkWasRobbedCanTrigger()
		{
			bool flag = !this.JingangWorldStateCheck();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				Location location = taiwu.GetValidLocation();
				bool flag2 = DomainManager.Map.IsAreaBroken(location.AreaId);
				if (flag2)
				{
					result = false;
				}
				else
				{
					sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
					result = (stateTemplateIdByAreaId == 11);
				}
			}
			return result;
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x000EA458 File Offset: 0x000E8658
		public int JingangSpreadSecInfoStage()
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			bool flag = DomainManager.Information.CharacterHasSecretInformationByTemplateId(taiwu.GetId(), 115);
			int result;
			if (flag)
			{
				result = 3;
			}
			else
			{
				bool flag2 = DomainManager.Information.CharacterHasSecretInformationByTemplateId(taiwu.GetId(), 114);
				if (flag2)
				{
					result = 2;
				}
				else
				{
					bool flag3 = DomainManager.Information.CharacterHasSecretInformationByTemplateId(taiwu.GetId(), 113);
					if (flag3)
					{
						result = 1;
					}
					else
					{
						bool flag4 = DomainManager.Information.CharacterHasSecretInformationByTemplateId(taiwu.GetId(), 112);
						if (flag4)
						{
							result = 0;
						}
						else
						{
							result = -1;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x000EA4E4 File Offset: 0x000E86E4
		[DomainMethod]
		public bool JingangMonkSoulBtnShow()
		{
			bool flag = this.JingangSpreadSecInfoStage() == -1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !this.JingangIsInSpreadSutraTask();
				if (flag2)
				{
					result = false;
				}
				else
				{
					EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
					bool jingangMonkSoulBtnDisappear = false;
					argBox.Get("ConchShip_PresetKey_JingangMonkSoulBtnDisappear", ref jingangMonkSoulBtnDisappear);
					result = !jingangMonkSoulBtnDisappear;
				}
			}
			return result;
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x000EA53C File Offset: 0x000E873C
		[DomainMethod]
		public bool JingangSoulTransformOpen()
		{
			EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
			bool jingangSoulTransformOpen = false;
			argBox.Get("ConchShip_PresetKey_JingangSoulTransformOpen", ref jingangSoulTransformOpen);
			return jingangSoulTransformOpen;
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x000EA56C File Offset: 0x000E876C
		public int JingangKnowSecInfoCount()
		{
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
			IntList charIds;
			sectArgBox.Get<IntList>("ConchShip_PresetKey_JingangKnowSecInfoIdList", out charIds);
			bool flag = charIds.Items == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = charIds.Items.Count;
			}
			return result;
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x000EA5B8 File Offset: 0x000E87B8
		public bool JingangCanTriggerMonkSoulEnterDream(DataContext context)
		{
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
			int totalCount = 0;
			sectArgBox.Get("ConchShip_PresetKey_JingangSpreadSecInfoTotalCount", ref totalCount);
			int enterDreamCount = 0;
			sectArgBox.Get("ConchShip_PresetKey_JingangMonkSoulEnterDreamCount", ref enterDreamCount);
			bool flag = totalCount / 5 > enterDreamCount && enterDreamCount < 5;
			bool result;
			if (flag)
			{
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, 11, "ConchShip_PresetKey_JingangMonkSoulEnterDreamCount", ++enterDreamCount);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x000EA62C File Offset: 0x000E882C
		public void JingangClearKnowSecInfo(DataContext context)
		{
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
			sectArgBox.Remove<IntList>("ConchShip_PresetKey_JingangKnowSecInfoIdList");
			DomainManager.Extra.SaveSectMainStoryEventArgumentBox(context, 11);
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x000EA664 File Offset: 0x000E8864
		public void JingangAddKnowSecInfoCharId(DataContext context, int charId)
		{
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
			IntList charIds;
			sectArgBox.Get<IntList>("ConchShip_PresetKey_JingangKnowSecInfoIdList", out charIds);
			bool flag = charIds.Items == null;
			if (flag)
			{
				charIds = IntList.Create();
			}
			charIds.Items.Add(charId);
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<IntList>(context, 11, "ConchShip_PresetKey_JingangKnowSecInfoIdList", charIds);
			int totalCount = 0;
			sectArgBox.Get("ConchShip_PresetKey_JingangSpreadSecInfoTotalCount", ref totalCount);
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, 11, "ConchShip_PresetKey_JingangSpreadSecInfoTotalCount", ++totalCount);
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x000EA6EC File Offset: 0x000E88EC
		public bool JingangCanTriggerPietyEvent()
		{
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
			int jingangPietyCount = 0;
			sectArgBox.Get("ConchShip_PresetKey_JingangPietyCount", ref jingangPietyCount);
			return jingangPietyCount < this.JingangCanTriggerPietyEventCount();
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x000EA724 File Offset: 0x000E8924
		public int JingangCanTriggerPietyEventCount()
		{
			int totalCount = 3;
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
			bool jingangGiveVillagerFood = false;
			sectArgBox.Get("ConchShip_PresetKey_JingangGiveVillagerFood", ref jingangGiveVillagerFood);
			bool flag = jingangGiveVillagerFood;
			if (flag)
			{
				totalCount++;
			}
			bool jingangGiveVillagerMoney = false;
			sectArgBox.Get("ConchShip_PresetKey_JingangGiveVillagerMoney", ref jingangGiveVillagerMoney);
			bool flag2 = jingangGiveVillagerMoney;
			if (flag2)
			{
				totalCount++;
			}
			bool jingangGiveVillagerPromise = false;
			sectArgBox.Get("ConchShip_PresetKey_JingangGiveVillagerPromise", ref jingangGiveVillagerPromise);
			bool flag3 = jingangGiveVillagerPromise;
			if (flag3)
			{
				totalCount++;
			}
			bool jingangGiveVillagerHelp = false;
			sectArgBox.Get("ConchShip_PresetKey_JingangGiveVillagerHelp", ref jingangGiveVillagerHelp);
			bool flag4 = jingangGiveVillagerHelp;
			if (flag4)
			{
				totalCount++;
			}
			return totalCount;
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x000EA7B8 File Offset: 0x000E89B8
		public void JingangDistributeSecInfo(DataContext context, int metaDataId, int targetCharId)
		{
			GameData.Domains.Character.Character monk = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 623);
			DomainManager.Information.DistributeSecretInformationToCharacter(context, metaDataId, targetCharId, monk.GetId());
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x000EA7EC File Offset: 0x000E89EC
		public bool JingangIsInSpreadSutraTask()
		{
			return DomainManager.Extra.IsExtraTaskInProgress(187) || DomainManager.Extra.IsExtraTaskInProgress(288) || DomainManager.Extra.IsExtraTaskInProgress(289) || DomainManager.Extra.IsExtraTaskInProgress(290);
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x000EA844 File Offset: 0x000E8A44
		public void JingangBroadCastSecInfo(DataContext context)
		{
			EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
			int metaDataId = 0;
			bool flag = argBox.Get("ConchShip_PresetKey_JingangSecInfoMetaDataId", ref metaDataId);
			if (flag)
			{
				DomainManager.Information.MakeSecretInformationBroadcastEffect(context, metaDataId, -1);
				bool flag2 = DomainManager.Information.GetSecretInformationConfig(metaDataId).TemplateId == 115;
				if (flag2)
				{
					DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<bool>(context, 11, "ConchShip_PresetKey_JingangMonkSoulBtnDisappear", true);
				}
			}
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x000EA8B0 File Offset: 0x000E8AB0
		public bool IsTaiwuAtRanshanSettlement(bool settlementBlockOnly = false)
		{
			short settlementId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(7);
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			bool flag = !taiwuLocation.IsValid();
			return !flag && (settlementBlockOnly ? DomainManager.Map.IsLocationOnSettlementBlock(taiwuLocation, settlementId) : DomainManager.Map.IsLocationInSettlementInfluenceRange(taiwuLocation, settlementId));
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x000EA910 File Offset: 0x000E8B10
		public bool IsRanshanSectMainStoryAbleToTrigger()
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
			int legendaryBookCount = 0;
			bool locationValid = false;
			bool flag = taiwuLocation.IsValid();
			if (flag)
			{
				sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(taiwuLocation.AreaId);
				locationValid = (stateTemplateId == 7);
			}
			for (sbyte combatSkillType = 0; combatSkillType < 14; combatSkillType += 1)
			{
				bool flag2 = DomainManager.Item.HasTrackedSpecialItems(12, (short)(211 + (int)combatSkillType));
				if (flag2)
				{
					legendaryBookCount++;
				}
			}
			return legendaryBookCount >= 3 && this.CheckSectMainStoryAvailable(7) && locationValid && !MapAreaData.IsBrokenArea(taiwuLocation.AreaId) && DomainManager.Organization.GetSettlementByOrgTemplateId(7).CalcApprovingRate() >= 500;
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x000EA9D4 File Offset: 0x000E8BD4
		public bool IsRanshanChapter1MonthlyEvent2AbleToTrigger()
		{
			bool flag = !DomainManager.Extra.IsExtraTaskInProgress(293);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int count = this.GetRanshanChapter1MonthlyEventTriggeredCount();
				bool flag2 = count >= 3;
				if (flag2)
				{
					result = false;
				}
				else
				{
					Location location = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
					MapAreaData areaData = DomainManager.Map.GetAreaByAreaId(location.AreaId);
					bool isAreaMeet = areaData.GetConfig().StateID == 7;
					bool flag3 = !isAreaMeet;
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool isTimeMeet = this.GetCurrDate() - this.GetRanshanChapter1MonthlyEventTriggeredDate() >= 3;
						result = isTimeMeet;
					}
				}
			}
			return result;
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x000EAA74 File Offset: 0x000E8C74
		public bool IsRanshanChapter1MonthlyEvent3AbleToTrigger()
		{
			return DomainManager.Extra.IsExtraTaskInProgress(295) && this.IsTaiwuAtRanshanSettlement(true);
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x000EAAA4 File Offset: 0x000E8CA4
		public bool IsRanshanChapter2HuajuAbleToTrigger()
		{
			return DomainManager.Extra.IsExtraTaskInProgress(296) && DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(660) == null;
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x000EAADC File Offset: 0x000E8CDC
		public bool IsRanshanChapter2XuanzhiAbleToTrigger()
		{
			return DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(660) != null && DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(661) == null;
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x000EAB14 File Offset: 0x000E8D14
		public bool IsRanshanChapter2YingjiaoAbleToTrigger()
		{
			return DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(661) != null && DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(662) == null;
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x000EAB4C File Offset: 0x000E8D4C
		public bool IsRanshanChapter2MonthlyEventAbleToTrigger()
		{
			int startDate = 0;
			DomainManager.Extra.GetSectMainStoryEventArgBox(7).Get("ConchShip_PresetKey_Ranshan_Chapter2_TeachStartDate", ref startDate);
			return DomainManager.Extra.IsExtraTaskChainInProgress(49) && (this.IsAllRanshanMenteeFinishedTeaching() || (DomainManager.Extra.GetSectMainStoryEventArgBox(7).Get("ConchShip_PresetKey_Ranshan_Chapter2_TeachStartDate", ref startDate) && this.GetCurrDate() - startDate >= 24));
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x000EABC0 File Offset: 0x000E8DC0
		public bool IsAllRanshanMenteeFinishedTeaching()
		{
			foreach (short templateId in SectMainStoryRelatedConstants.RanshanThreeCorpsesCharacterTemplateIdList)
			{
				bool flag = DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(templateId).Progress != 3;
				if (flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x000EAC34 File Offset: 0x000E8E34
		public int GetRanshanChapter1MonthlyEventTriggeredCount()
		{
			int count = 0;
			return DomainManager.Extra.GetSectMainStoryEventArgBox(7).Get("ConchShip_PresetKey_Ranshan_Chapter1_MonthlyEventTriggeredCount", ref count) ? count : 0;
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x000EAC68 File Offset: 0x000E8E68
		public int GetRanshanChapter1MonthlyEventTriggeredDate()
		{
			int date = 0;
			return DomainManager.Extra.GetSectMainStoryEventArgBox(7).Get("ConchShip_PresetKey_Ranshan_Chapter1_MonthlyEventTriggeredDate", ref date) ? date : 0;
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x000EAC9C File Offset: 0x000E8E9C
		public void UpdateRanshanThreeCorpsesAction(DataContext context)
		{
			bool flag = this.GetSectMainStoryTaskStatus(7) == 1;
			if (flag)
			{
				foreach (short templateId in SectMainStoryRelatedConstants.RanshanThreeCorpsesCharacterTemplateIdList)
				{
					SectStoryThreeCorpsesCharacter corpse = DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(templateId);
					bool flag2 = corpse == null || !corpse.IsGoodEnd;
					if (!flag2)
					{
						int currDate = this.GetCurrDate();
						bool flag3 = corpse.Target >= 0;
						if (flag3)
						{
							bool flag4 = currDate >= corpse.NextDate;
							if (flag4)
							{
								bool flag5 = DomainManager.LegendaryBook.GetOwner(corpse.Target) != corpse.TargetOwner;
								if (flag5)
								{
									DomainManager.Extra.ApplyRanshanThreeCorpsesLegendaryBookActionResult(context, templateId, 320, false);
								}
								else
								{
									bool ranshanThreeCorpsesActionSucceed = DomainManager.Extra.GetRanshanThreeCorpsesActionSucceed(context, templateId);
									if (ranshanThreeCorpsesActionSucceed)
									{
										DomainManager.Extra.ApplyRanshanThreeCorpsesLegendaryBookActionResult(context, templateId, 314, true);
									}
									else
									{
										DomainManager.Extra.SetRanshanThreeCorpsesCharacterNextDate(context, templateId);
									}
								}
							}
							bool flag6 = currDate >= corpse.EndDate && corpse.EndDate >= 0;
							if (flag6)
							{
								DomainManager.Extra.ApplyRanshanThreeCorpsesLegendaryBookActionResult(context, templateId, 317, false);
							}
							DomainManager.Extra.SetRanshanThreeCorpsesCharacterLocation(context, templateId);
						}
					}
				}
			}
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x000EAE1C File Offset: 0x000E901C
		public void ConvertRanshanFootman(DataContext context, bool isGoodEnd)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 625);
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			short value = FavorabilityType.GetRandomFavorability(context.Random, isGoodEnd ? 6 : 1);
			DomainManager.Character.ConvertFixedCharacter(context, character, taiwu.GetLocation(), false);
			DomainManager.Character.DirectlySetFavorabilities(context, character.GetId(), taiwu.GetId(), value, value);
			if (isGoodEnd)
			{
				DomainManager.Organization.ChangeGrade(context, character, 5, true);
			}
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x000EAE9C File Offset: 0x000E909C
		public IntList BaihuaGetSpecialDebuffIntList()
		{
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
			IntList list;
			sectArgBox.Get<IntList>("ConchShip_PresetKey_BaihuaSpecialDebuffIntList", out list);
			return list;
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x000EAECC File Offset: 0x000E90CC
		public void BaihuaAddCharIdToSpecialDebuffIntList(DataContext context, int charId)
		{
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
			IntList list;
			sectArgBox.Get<IntList>("ConchShip_PresetKey_BaihuaSpecialDebuffIntList", out list);
			ref List<int> ptr = ref list.Items;
			if (ptr == null)
			{
				ptr = new List<int>();
			}
			list.Items.Add(charId);
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<IntList>(context, 3, "ConchShip_PresetKey_BaihuaSpecialDebuffIntList", list);
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x000EAF28 File Offset: 0x000E9128
		public IntList BaihuaGetCureSpecialDebuffIntList()
		{
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
			IntList list;
			sectArgBox.Get<IntList>("ConchShip_PresetKey_BaihuaCureSpecialDebuffIntList", out list);
			return list;
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x000EAF58 File Offset: 0x000E9158
		public void BaihuaAddCharIdToCureSpecialDebuffIntList(DataContext context, int charId)
		{
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
			IntList list;
			sectArgBox.Get<IntList>("ConchShip_PresetKey_BaihuaCureSpecialDebuffIntList", out list);
			ref List<int> ptr = ref list.Items;
			if (ptr == null)
			{
				ptr = new List<int>();
			}
			list.Items.Add(charId);
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<IntList>(context, 3, "ConchShip_PresetKey_BaihuaCureSpecialDebuffIntList", list);
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x000EAFB4 File Offset: 0x000E91B4
		public void BaihuaRemoveCharIdToCureSpecialDebuffIntList(DataContext context, int charId)
		{
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
			IntList list;
			sectArgBox.Get<IntList>("ConchShip_PresetKey_BaihuaCureSpecialDebuffIntList", out list);
			bool flag = list.Items == null;
			if (!flag)
			{
				list.Items.Remove(charId);
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<IntList>(context, 3, "ConchShip_PresetKey_BaihuaCureSpecialDebuffIntList", list);
			}
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x000EB00C File Offset: 0x000E920C
		public short BaihuaSelectSettlementId(DataContext context, bool avoidGuangnan, bool avoidTaiwuVillageArea)
		{
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			List<short> areasCanSelect = ObjectPool<List<short>>.Instance.Get();
			List<short> areas = ObjectPool<List<short>>.Instance.Get();
			List<short> settlementIds = ObjectPool<List<short>>.Instance.Get();
			for (short stateTemplateId = 1; stateTemplateId <= 15; stateTemplateId += 1)
			{
				areas.Clear();
				bool flag = avoidGuangnan && stateTemplateId == 3;
				if (!flag)
				{
					sbyte stateId = DomainManager.Map.GetStateIdByStateTemplateId(stateTemplateId);
					DomainManager.Map.GetAllRegularAreaInState(stateId, areas);
					for (int i = 0; i < areas.Count; i++)
					{
						bool flag2 = !DomainManager.Map.IsAreaBroken(areas[i]);
						if (flag2)
						{
							areasCanSelect.Add(areas[i]);
						}
					}
				}
			}
			if (avoidTaiwuVillageArea)
			{
				areasCanSelect.Remove(taiwuVillageLocation.AreaId);
			}
			short areaSelected = areasCanSelect.GetRandom(context.Random);
			DomainManager.Map.GetAreaSettlementIds(areaSelected, settlementIds, true, true);
			short settlementId = settlementIds.GetRandom(context.Random);
			ObjectPool<List<short>>.Instance.Return(areasCanSelect);
			ObjectPool<List<short>>.Instance.Return(areas);
			ObjectPool<List<short>>.Instance.Return(settlementIds);
			return settlementId;
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x000EB154 File Offset: 0x000E9354
		public List<short> BaihuaSelectSettlementIds(DataContext context, bool avoidGuangnan, bool avoidTaiwuVillageArea, int needCount)
		{
			int count = 0;
			List<short> settlementIds = new List<short>();
			while (settlementIds.Count < needCount && count < 10000)
			{
				short settlementId = this.BaihuaSelectSettlementId(context, avoidGuangnan, avoidTaiwuVillageArea);
				bool flag = !settlementIds.Contains(settlementId);
				if (flag)
				{
					settlementIds.Add(settlementId);
				}
				count++;
			}
			return settlementIds;
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x000EB1B4 File Offset: 0x000E93B4
		public short BaihuaSelectSettlementIdNeighborTaiwuVillage(DataContext context)
		{
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			sbyte taiwuVillageStateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(taiwuVillageLocation.AreaId);
			sbyte taiwuVillageStateId = DomainManager.Map.GetStateIdByAreaId(taiwuVillageLocation.AreaId);
			MapStateItem config = MapState.Instance[taiwuVillageStateTemplateId];
			List<short> areasCanSelect = ObjectPool<List<short>>.Instance.Get();
			List<short> areas = ObjectPool<List<short>>.Instance.Get();
			List<short> settlementIds = ObjectPool<List<short>>.Instance.Get();
			EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
			short leukoSettlementId = -1;
			sbyte leukoStateId = -1;
			argBox.Get("ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementId", ref leukoSettlementId);
			short melanoSettlementId = -1;
			sbyte melanoStateId = -1;
			argBox.Get("ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementId", ref melanoSettlementId);
			bool flag = leukoSettlementId != -1;
			if (flag)
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(leukoSettlementId);
				leukoStateId = DomainManager.Map.GetStateIdByAreaId(settlement.GetLocation().AreaId);
			}
			bool flag2 = melanoSettlementId != -1;
			if (flag2)
			{
				Settlement settlement2 = DomainManager.Organization.GetSettlement(melanoSettlementId);
				melanoStateId = DomainManager.Map.GetStateIdByAreaId(settlement2.GetLocation().AreaId);
			}
			for (int i = 0; i < config.NeighborStates.Length; i++)
			{
				areas.Clear();
				bool flag3 = config.NeighborStates[i] == 3;
				if (!flag3)
				{
					sbyte stateId = DomainManager.Map.GetStateIdByStateTemplateId((short)config.NeighborStates[i]);
					bool flag4 = stateId == leukoStateId || stateId == melanoStateId;
					if (!flag4)
					{
						DomainManager.Map.GetAllRegularAreaInState(stateId, areas);
						for (int j = 0; j < areas.Count; j++)
						{
							bool flag5 = !DomainManager.Map.IsAreaBroken(areas[j]);
							if (flag5)
							{
								areasCanSelect.Add(areas[j]);
							}
						}
					}
				}
			}
			bool flag6 = areasCanSelect.Count == 0;
			if (flag6)
			{
				short usedSettlementId = (leukoSettlementId > 0) ? leukoSettlementId : melanoSettlementId;
				Settlement settlement3 = DomainManager.Organization.GetSettlement(usedSettlementId);
				sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(settlement3.GetLocation().AreaId);
				MapStateItem stateConfig = MapState.Instance[stateTemplateId];
				for (int k = 0; k < stateConfig.NeighborStates.Length; k++)
				{
					areas.Clear();
					bool flag7 = stateConfig.NeighborStates[k] == 3;
					if (!flag7)
					{
						sbyte stateId2 = DomainManager.Map.GetStateIdByStateTemplateId((short)stateConfig.NeighborStates[k]);
						bool flag8 = stateId2 == taiwuVillageStateId;
						if (!flag8)
						{
							DomainManager.Map.GetAllRegularAreaInState(stateId2, areas);
							for (int l = 0; l < areas.Count; l++)
							{
								bool flag9 = !DomainManager.Map.IsAreaBroken(areas[l]);
								if (flag9)
								{
									areasCanSelect.Add(areas[l]);
								}
							}
						}
					}
				}
			}
			short areaSelected = areasCanSelect.GetRandom(context.Random);
			DomainManager.Map.GetAreaSettlementIds(areaSelected, settlementIds, true, true);
			short settlementId = settlementIds.GetRandom(context.Random);
			ObjectPool<List<short>>.Instance.Return(areasCanSelect);
			ObjectPool<List<short>>.Instance.Return(areas);
			ObjectPool<List<short>>.Instance.Return(settlementIds);
			return settlementId;
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x000EB508 File Offset: 0x000E9708
		public void CallBaihuaMember(DataContext context, bool isLeuko)
		{
			EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
			short settlementId = -1;
			IntList charIdIntList;
			if (isLeuko)
			{
				argBox.Get<IntList>("ConchShip_PresetKey_BaihuaLeukoKillsCalledCharIds", out charIdIntList);
				argBox.Get("ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementId", ref settlementId);
			}
			else
			{
				argBox.Get<IntList>("ConchShip_PresetKey_BaihuaMelanoKillsCalledCharIds", out charIdIntList);
				argBox.Get("ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementId", ref settlementId);
			}
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			bool flag = charIdIntList.Items == null;
			if (flag)
			{
				charIdIntList = IntList.Create();
			}
			bool flag2 = !WorldDomain.<CallBaihuaMember>g__HaveAliveMember|230_0(charIdIntList.Items);
			if (flag2)
			{
				Settlement baihuaSettlement = DomainManager.Organization.GetSettlementByOrgTemplateId(3);
				List<int> members = ObjectPool<List<int>>.Instance.Get();
				for (sbyte grade = 0; grade <= 5; grade += 1)
				{
					members.AddRange(baihuaSettlement.GetMembers().GetMembers(grade));
				}
				for (int i = members.Count - 1; i >= 0; i--)
				{
					GameData.Domains.Character.Character character;
					bool flag3 = DomainManager.Character.TryGetElement_Objects(members[i], out character);
					if (flag3)
					{
						bool flag4 = !character.IsInteractableAsIntelligentCharacter() || character.GetAgeGroup() != 2;
						if (flag4)
						{
							members.RemoveAt(i);
						}
					}
					else
					{
						members.RemoveAt(i);
					}
				}
				int count = context.Random.Next(1, 4);
				CollectionUtils.Shuffle<int>(context.Random, members);
				for (int j = 0; j < Math.Min(count, members.Count); j++)
				{
					int selectCharId = members[j];
					charIdIntList.Items.Add(selectCharId);
					GameData.Domains.Character.Character character2;
					bool flag5 = DomainManager.Character.TryGetElement_Objects(selectCharId, out character2);
					if (flag5)
					{
						DomainManager.Character.GroupMove(context, character2, settlement.GetLocation());
						character2.ActiveExternalRelationState(context, 4);
					}
				}
				ObjectPool<List<int>>.Instance.Return(members);
				if (isLeuko)
				{
					DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<IntList>(context, 3, "ConchShip_PresetKey_BaihuaLeukoKillsCalledCharIds", charIdIntList);
				}
				else
				{
					DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<IntList>(context, 3, "ConchShip_PresetKey_BaihuaMelanoKillsCalledCharIds", charIdIntList);
				}
			}
			else
			{
				for (int k = 0; k < charIdIntList.Items.Count; k++)
				{
					GameData.Domains.Character.Character character3;
					bool flag6 = DomainManager.Character.TryGetElement_Objects(charIdIntList.Items[k], out character3);
					if (flag6)
					{
						DomainManager.Character.GroupMove(context, character3, settlement.GetLocation());
						character3.ActiveExternalRelationState(context, 4);
					}
				}
			}
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x000EB79C File Offset: 0x000E999C
		public void BaihuaClearCalledCharacters(DataContext context, bool isLeuko)
		{
			EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
			IntList charIdIntList;
			if (isLeuko)
			{
				argBox.Get<IntList>("ConchShip_PresetKey_BaihuaLeukoKillsCalledCharIds", out charIdIntList);
			}
			else
			{
				argBox.Get<IntList>("ConchShip_PresetKey_BaihuaMelanoKillsCalledCharIds", out charIdIntList);
			}
			bool flag = charIdIntList.Items == null;
			if (!flag)
			{
				for (int i = 0; i < charIdIntList.Items.Count; i++)
				{
					GameData.Domains.Character.Character character;
					bool flag2 = DomainManager.Character.TryGetElement_Objects(charIdIntList.Items[i], out character);
					if (flag2)
					{
						character.DeactivateExternalRelationState(context, 4);
					}
				}
			}
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x000EB834 File Offset: 0x000E9A34
		public int BaihuaGroupMeetCount(bool isLeuko, out int groupId)
		{
			groupId = -1;
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
			sbyte neiliType = -1;
			if (isLeuko)
			{
				sectArgBox.Get("ConchShip_PresetKey_BaihuaLeukoKillsFiveElementsType", ref neiliType);
			}
			else
			{
				sectArgBox.Get("ConchShip_PresetKey_BaihuaMelanoKillsFiveElementsType", ref neiliType);
			}
			HashSet<int> groupCharIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			int sum = 0;
			foreach (int charId in groupCharIds)
			{
				bool flag = charId == taiwu.GetId();
				if (!flag)
				{
					GameData.Domains.Character.Character character;
					bool flag2 = DomainManager.Character.TryGetElement_Objects(charId, out character);
					if (flag2)
					{
						bool flag3 = character.GetAgeGroup() < 2;
						if (!flag3)
						{
							NeiliTypeItem config = NeiliType.Instance[character.GetNeiliType()];
							bool flag4 = config.FiveElements == (byte)neiliType;
							if (flag4)
							{
								groupId = character.GetId();
								sum++;
							}
						}
					}
				}
			}
			return sum;
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x000EB954 File Offset: 0x000E9B54
		public bool FulongDisasterStart()
		{
			GameData.Domains.Character.Character character;
			bool flag = DomainManager.Character.TryGetFixedCharacterByTemplateId(446, out character);
			if (flag)
			{
				bool flag2 = !DomainManager.TaiwuEvent.GetGlobalEventArgumentBox().Contains<bool>("YuFuTellRanchenziStory");
				if (flag2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x000EB9A0 File Offset: 0x000E9BA0
		private unsafe void FulongTriggerDisaster(DataContext context)
		{
			int distance = 6;
			Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(14);
			List<MapBlockData> mapBlockList = ObjectPool<List<MapBlockData>>.Instance.Get();
			List<MapBlockData> mapBlockListFit = ObjectPool<List<MapBlockData>>.Instance.Get();
			DomainManager.Map.GetLocationByDistance(settlement.GetLocation(), distance, distance, ref mapBlockList);
			for (int i = 0; i < mapBlockList.Count; i++)
			{
				MapBlockData mapBlockData = mapBlockList[i];
				bool flag = mapBlockData.GetConfig().SubType == EMapBlockSubType.DLCLoong;
				if (!flag)
				{
					bool flag2 = mapBlockData.GetConfig().Size > 1;
					if (!flag2)
					{
						bool flag3 = mapBlockData.IsNonDeveloped();
						if (flag3)
						{
							mapBlockListFit.Add(mapBlockData);
						}
					}
				}
			}
			bool flag4 = mapBlockListFit.Count < 1;
			if (!flag4)
			{
				MapBlockData disasterCenterMapBlockData = mapBlockListFit.GetRandom(context.Random);
				mapBlockList.Clear();
				DomainManager.Map.GetLocationByDistance(disasterCenterMapBlockData.GetLocation(), 1, 1, ref mapBlockList);
				MapBlockData disasterMapBlockData = mapBlockList.GetRandom(context.Random);
				int maxResourceType = 0;
				int maxResourceCount = 0;
				for (sbyte type = 0; type < 6; type += 1)
				{
					bool flag5 = (int)(*(ref disasterMapBlockData.CurrResources.Items.FixedElementField + (IntPtr)type * 2)) > maxResourceCount;
					if (flag5)
					{
						maxResourceCount = (int)(*(ref disasterMapBlockData.CurrResources.Items.FixedElementField + (IntPtr)type * 2));
						maxResourceType = (int)type;
					}
				}
				List<short> advType = DomainManager.Adventure.GetDisasterAdventureTypesByResourceType(maxResourceType);
				short adventureId = advType.GetRandom(context.Random);
				Location disasterLocation = disasterMapBlockData.GetLocation();
				AreaAdventureData adventureData = DomainManager.Adventure.GetAdventuresInArea(settlement.GetLocation().AreaId);
				bool flag6 = !adventureData.AdventureSites.Keys.Contains(disasterLocation.BlockId);
				if (flag6)
				{
					DomainManager.Adventure.RemoveAdventureSite(context, disasterLocation.AreaId, disasterLocation.BlockId, false, false);
					DomainManager.Adventure.TryCreateAdventureSite(context, disasterLocation.AreaId, disasterLocation.BlockId, adventureId, MonthlyActionKey.Invalid);
				}
				mapBlockList.Add(disasterCenterMapBlockData);
				for (int j = 0; j < mapBlockList.Count; j++)
				{
					MapBlockData mapBlockData2 = mapBlockList[j];
					bool flag7 = mapBlockData2.GetConfig().Size > 1;
					if (!flag7)
					{
						mapBlockData2.Destroyed = true;
						mapBlockData2.DestroyItemsDirect(context.AdvanceMonthRelatedData.WorldItemsToBeRemoved);
						mapBlockData2.Malice = 0;
						for (sbyte type2 = 0; type2 < 6; type2 += 1)
						{
							*(ref mapBlockData2.CurrResources.Items.FixedElementField + (IntPtr)type2 * 2) = 0;
						}
						DomainManager.Map.SetBlockData(context, mapBlockData2);
					}
				}
				ObjectPool<List<MapBlockData>>.Instance.Return(mapBlockList);
				ObjectPool<List<MapBlockData>>.Instance.Return(mapBlockListFit);
			}
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x000EBC68 File Offset: 0x000E9E68
		public bool ShixiangSettlementAffiliatedBlocksHasEnemy(DataContext context, short startTemplateId)
		{
			Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(6);
			Location settlementLocation = settlement.GetLocation();
			List<MapBlockData> blockList = context.AdvanceMonthRelatedData.Blocks.Occupy();
			DomainManager.Map.GetSettlementAffiliatedBlocks(settlementLocation.AreaId, settlementLocation.BlockId, blockList);
			bool hasEnemy = false;
			foreach (MapBlockData block in blockList)
			{
				bool flag = block.EnemyCharacterSet == null;
				if (!flag)
				{
					foreach (int charId in block.EnemyCharacterSet)
					{
						GameData.Domains.Character.Character enemy;
						DomainManager.Character.TryGetElement_Objects(charId, out enemy);
						short templateId = enemy.GetTemplateId();
						bool flag2 = templateId < startTemplateId || templateId > startTemplateId + 4;
						if (!flag2)
						{
							hasEnemy = true;
							break;
						}
					}
				}
			}
			context.AdvanceMonthRelatedData.Blocks.Release(ref blockList);
			return hasEnemy;
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x000EBD9C File Offset: 0x000E9F9C
		private void InitializeBaihuaLinkedCharacters(DataContext context)
		{
			this._baihuaLinkedCharacters = new Dictionary<int, ValueTuple<int, bool>>();
			SectBaihuaLifeLinkData lifeLinkData = DomainManager.Extra.GetSectBaihuaLifeLinkData();
			bool flag = !lifeLinkData.IsInitialized();
			if (flag)
			{
				this._baihuaLifeLinkNeiliType = -1;
				lifeLinkData.Initialize();
				DomainManager.Extra.SetSectBaihuaLifeLinkData(lifeLinkData, context);
			}
			else
			{
				this._baihuaLifeLinkNeiliType = this.CalcBaihuaLifeLinkNeiliType();
				for (int i = 0; i < lifeLinkData.LifeGateCharIds.Length; i++)
				{
					int charId = lifeLinkData.LifeGateCharIds[i];
					bool flag2 = charId < 0;
					if (!flag2)
					{
						this._baihuaLinkedCharacters.Add(charId, new ValueTuple<int, bool>(i, true));
					}
				}
				for (int j = 0; j < lifeLinkData.DeathGateCharIds.Length; j++)
				{
					int charId2 = lifeLinkData.DeathGateCharIds[j];
					bool flag3 = charId2 < 0;
					if (!flag3)
					{
						this._baihuaLinkedCharacters.Add(charId2, new ValueTuple<int, bool>(j, false));
					}
				}
			}
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x000EBE8C File Offset: 0x000EA08C
		[DomainMethod]
		public sbyte GetBaihuaLifeLinkNeiliType()
		{
			return this._baihuaLifeLinkNeiliType;
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x000EBEA4 File Offset: 0x000EA0A4
		[DomainMethod]
		public void SetLifeLinkCharacter(DataContext context, int charId, int index, bool isLifeGate)
		{
			Logger logger = WorldDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Setting life link character ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
			defaultInterpolatedStringHandler.AppendLiteral(": isLifeGate = ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(isLifeGate);
			defaultInterpolatedStringHandler.AppendLiteral(", index = ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(index);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			SectBaihuaLifeLinkData lifeLinkData = DomainManager.Extra.GetSectBaihuaLifeLinkData();
			int[] charList = isLifeGate ? lifeLinkData.LifeGateCharIds : lifeLinkData.DeathGateCharIds;
			int prevCharId = charList[index];
			bool flag = prevCharId >= 0;
			if (flag)
			{
				this._baihuaLinkedCharacters.Remove(prevCharId);
				bool flag2 = this._baihuaLifeLinkNeiliType >= 0;
				if (flag2)
				{
					GameData.Domains.Character.Character prevChar = DomainManager.Character.GetElement_Objects(prevCharId);
					NeiliTypeItem neiliTypeCfg = NeiliType.Instance[this._baihuaLifeLinkNeiliType];
					short[] features = isLifeGate ? neiliTypeCfg.LifeGateFeatures : neiliTypeCfg.DeathGateFeatures;
					bool flag3 = features != null;
					if (flag3)
					{
						for (int i = 0; i < features.Length; i++)
						{
							prevChar.RemoveFeature(context, features[i]);
						}
					}
				}
			}
			charList[index] = charId;
			DomainManager.Extra.SetSectBaihuaLifeLinkData(lifeLinkData, context);
			bool neiliTypeChanged = this.UpdateBaihuaLifeLinkNeiliType(context);
			bool flag4 = charId >= 0;
			if (flag4)
			{
				this._baihuaLinkedCharacters.Add(charId, new ValueTuple<int, bool>(index, isLifeGate));
				bool flag5 = !neiliTypeChanged && this._baihuaLifeLinkNeiliType >= 0;
				if (flag5)
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
					NeiliTypeItem neiliTypeCfg2 = NeiliType.Instance[this._baihuaLifeLinkNeiliType];
					short[] features2 = isLifeGate ? neiliTypeCfg2.LifeGateFeatures : neiliTypeCfg2.DeathGateFeatures;
					bool flag6 = features2 != null;
					if (flag6)
					{
						for (int j = 0; j < features2.Length; j++)
						{
							character.AddFeature(context, features2[j], false);
						}
					}
				}
			}
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x000EC090 File Offset: 0x000EA290
		public void TryRemoveLifeLinkCharacter(DataContext context, GameData.Domains.Character.Character character)
		{
			int charId = character.GetId();
			ValueTuple<int, bool> info;
			bool flag = !this._baihuaLinkedCharacters.TryGetValue(charId, out info);
			if (!flag)
			{
				SectBaihuaLifeLinkData lifeLinkData = DomainManager.Extra.GetSectBaihuaLifeLinkData();
				int[] charList = info.Item2 ? lifeLinkData.LifeGateCharIds : lifeLinkData.DeathGateCharIds;
				charList[info.Item1] = -1;
				this._baihuaLinkedCharacters.Remove(charId);
				bool flag2 = this._baihuaLifeLinkNeiliType >= 0 && DomainManager.Character.IsCharacterAlive(charId);
				if (flag2)
				{
					NeiliTypeItem neiliTypeCfg = NeiliType.Instance[this._baihuaLifeLinkNeiliType];
					foreach (short featureId in neiliTypeCfg.LifeGateFeatures)
					{
						character.RemoveFeature(context, featureId);
					}
				}
				DomainManager.Extra.SetSectBaihuaLifeLinkData(lifeLinkData, context);
				this.UpdateBaihuaLifeLinkNeiliType(context);
			}
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x000EC170 File Offset: 0x000EA370
		private void UpdateBaihuaFixedCharacterLocations(DataContext context, int charId)
		{
			int currDate = DomainManager.World.GetCurrDate();
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
			int date = int.MaxValue;
			bool flag = sectArgBox.Get("ConchShip_PresetKey_BaihuaLMTransferAnimalDate", ref date) && date == currDate;
			if (!flag)
			{
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, 3, "ConchShip_PresetKey_BaihuaLMTransferAnimalDate", currDate);
				GameData.Domains.Character.Character leukorpus = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 781);
				GameData.Domains.Character.Character melanpsyche = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 786);
				GameData.Domains.Character.Character leukoDeer = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 808);
				GameData.Domains.Character.Character melanoOwl = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 809);
				bool flag2 = !leukorpus.GetLocation().IsValid();
				if (!flag2)
				{
					Events.RaiseFixedCharacterLocationChanged(context, leukoDeer.GetId(), leukoDeer.GetLocation(), leukorpus.GetLocation());
					leukoDeer.SetLocation(leukorpus.GetLocation(), context);
					Events.RaiseFixedCharacterLocationChanged(context, melanoOwl.GetId(), melanoOwl.GetLocation(), melanpsyche.GetLocation());
					melanoOwl.SetLocation(melanpsyche.GetLocation(), context);
					Events.RaiseFixedCharacterLocationChanged(context, leukorpus.GetId(), leukorpus.GetLocation(), Location.Invalid);
					leukorpus.SetLocation(Location.Invalid, context);
					Events.RaiseFixedCharacterLocationChanged(context, melanpsyche.GetId(), melanpsyche.GetLocation(), Location.Invalid);
					melanpsyche.SetLocation(Location.Invalid, context);
					InstantNotificationCollection instantCollection = DomainManager.World.GetInstantNotificationCollection();
					instantCollection.AddSectStoryBaihuaToAnimal(charId);
				}
			}
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x000EC2EC File Offset: 0x000EA4EC
		public bool UpdateBaihuaLifeLinkNeiliType(DataContext context)
		{
			SectBaihuaLifeLinkData lifeLinkData = DomainManager.Extra.GetSectBaihuaLifeLinkData();
			bool flag = !lifeLinkData.IsInitialized();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte newLifeLinkNeiliType = this.CalcBaihuaLifeLinkNeiliType();
				bool flag2 = newLifeLinkNeiliType == this._baihuaLifeLinkNeiliType;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = this._baihuaLifeLinkNeiliType >= 0;
					if (flag3)
					{
						NeiliTypeItem neiliTypeCfg = NeiliType.Instance[this._baihuaLifeLinkNeiliType];
						foreach (int lifeGateCharId in lifeLinkData.LifeGateCharIds)
						{
							bool flag4 = lifeGateCharId < 0;
							if (!flag4)
							{
								GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(lifeGateCharId);
								foreach (short featureId in neiliTypeCfg.LifeGateFeatures)
								{
									character.RemoveFeature(context, featureId);
								}
							}
						}
						foreach (int deathGateCharId in lifeLinkData.DeathGateCharIds)
						{
							bool flag5 = deathGateCharId < 0;
							if (!flag5)
							{
								GameData.Domains.Character.Character character2 = DomainManager.Character.GetElement_Objects(deathGateCharId);
								foreach (short featureId2 in neiliTypeCfg.DeathGateFeatures)
								{
									character2.RemoveFeature(context, featureId2);
								}
							}
						}
					}
					bool flag6 = newLifeLinkNeiliType >= 0;
					if (flag6)
					{
						NeiliTypeItem neiliTypeCfg2 = NeiliType.Instance[newLifeLinkNeiliType];
						foreach (int lifeGateCharId2 in lifeLinkData.LifeGateCharIds)
						{
							bool flag7 = lifeGateCharId2 < 0;
							if (!flag7)
							{
								GameData.Domains.Character.Character character3 = DomainManager.Character.GetElement_Objects(lifeGateCharId2);
								foreach (short featureId3 in neiliTypeCfg2.LifeGateFeatures)
								{
									character3.AddFeature(context, featureId3, false);
								}
							}
						}
						foreach (int deathGateCharId2 in lifeLinkData.DeathGateCharIds)
						{
							bool flag8 = deathGateCharId2 < 0;
							if (!flag8)
							{
								GameData.Domains.Character.Character character4 = DomainManager.Character.GetElement_Objects(deathGateCharId2);
								foreach (short featureId4 in neiliTypeCfg2.DeathGateFeatures)
								{
									character4.AddFeature(context, featureId4, false);
								}
							}
						}
					}
					this._baihuaLifeLinkNeiliType = newLifeLinkNeiliType;
					bool flag9 = this._advancingMonthState != 0 && !this._sectMainStoryLifeLinkUpdated;
					if (flag9)
					{
						this._sectMainStoryLifeLinkUpdated = true;
						this.GetMonthlyNotificationCollection().AddFiveElementsChange();
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x000EC578 File Offset: 0x000EA778
		private unsafe sbyte CalcBaihuaLifeLinkNeiliType()
		{
			SectBaihuaLifeLinkData lifeLinkData = DomainManager.Extra.GetSectBaihuaLifeLinkData();
			int length = lifeLinkData.LifeGateCharIds.Length + lifeLinkData.DeathGateCharIds.Length;
			int num = length;
			Span<NeiliProportionOfFiveElements> span = new Span<NeiliProportionOfFiveElements>(stackalloc byte[checked(unchecked((UIntPtr)num) * (UIntPtr)sizeof(NeiliProportionOfFiveElements))], num);
			SpanList<NeiliProportionOfFiveElements> spanList = span;
			int lifeGateCount = 0;
			foreach (int charId in lifeLinkData.LifeGateCharIds)
			{
				GameData.Domains.Character.Character character;
				bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (!flag)
				{
					NeiliProportionOfFiveElements fiveElements = character.GetNeiliProportionOfFiveElements();
					spanList.Add(fiveElements);
					lifeGateCount++;
				}
			}
			bool flag2 = lifeGateCount == 0;
			sbyte result;
			if (flag2)
			{
				result = -1;
			}
			else
			{
				int deathGateCount = 0;
				foreach (int charId2 in lifeLinkData.DeathGateCharIds)
				{
					GameData.Domains.Character.Character character2;
					bool flag3 = !DomainManager.Character.TryGetElement_Objects(charId2, out character2);
					if (!flag3)
					{
						NeiliProportionOfFiveElements fiveElements2 = character2.GetNeiliProportionOfFiveElements();
						spanList.Add(fiveElements2);
						deathGateCount++;
					}
				}
				bool flag4 = deathGateCount == 0;
				if (flag4)
				{
					result = -1;
				}
				else
				{
					NeiliProportionOfFiveElements proportionOfFiveElements = NeiliProportionOfFiveElements.GetTotal(spanList);
					result = proportionOfFiveElements.GetNeiliType(DomainManager.World.GetCurrMonthInYear());
				}
			}
			return result;
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x000EC6BC File Offset: 0x000EA8BC
		private unsafe void UpdateLifeDeathGateCharacters(DataContext context)
		{
			SectBaihuaLifeLinkData lifeDeathGate = DomainManager.Extra.GetSectBaihuaLifeLinkData();
			bool flag = lifeDeathGate == null || !lifeDeathGate.IsInitialized();
			if (!flag)
			{
				bool flag2 = lifeDeathGate.Cooldown > 0;
				if (flag2)
				{
					SectBaihuaLifeLinkData sectBaihuaLifeLinkData = lifeDeathGate;
					sectBaihuaLifeLinkData.Cooldown -= 1;
					DomainManager.Extra.SetSectBaihuaLifeLinkData(lifeDeathGate, context);
					bool flag3 = lifeDeathGate.Cooldown >= 0;
					if (flag3)
					{
						return;
					}
				}
				this._tmpLifeGateChars.Clear();
				foreach (int lifeGateCharId in lifeDeathGate.LifeGateCharIds)
				{
					bool flag4 = lifeGateCharId < 0;
					if (!flag4)
					{
						GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(lifeGateCharId);
						short health = character.GetHealth();
						short maxHealth = character.GetLeftMaxHealth(false);
						bool flag5 = health < maxHealth;
						if (flag5)
						{
							this._tmpLifeGateChars.Add(new ValueTuple<GameData.Domains.Character.Character, int, int>(character, (int)health, (int)maxHealth));
						}
					}
				}
				int distributableHealth = 0;
				this._tmpDeathGateChars.Clear();
				foreach (int deathGateCharId in lifeDeathGate.DeathGateCharIds)
				{
					bool flag6 = deathGateCharId < 0;
					if (!flag6)
					{
						GameData.Domains.Character.Character character2 = DomainManager.Character.GetElement_Objects(deathGateCharId);
						short health2 = character2.GetHealth();
						bool flag7 = health2 > 0;
						if (flag7)
						{
							distributableHealth += (int)health2;
							this._tmpDeathGateChars.Add(new ValueTuple<GameData.Domains.Character.Character, int>(character2, (int)health2));
						}
					}
				}
				bool flag8 = this._tmpLifeGateChars.Count == 0 || this._tmpDeathGateChars.Count == 0;
				if (!flag8)
				{
					IRandomSource random = context.Random;
					Span<int> span = new Span<int>(stackalloc byte[(UIntPtr)32], 8);
					SpanList<int> modifiedLifeGateCharIds = span;
					span = new Span<int>(stackalloc byte[(UIntPtr)32], 8);
					SpanList<int> modifiedDeathGateCharIds = span;
					while (this._tmpLifeGateChars.Count > 0 && this._tmpDeathGateChars.Count > 0)
					{
						int lifeGateCharIndex = random.Next(this._tmpLifeGateChars.Count);
						ValueTuple<GameData.Domains.Character.Character, int, int> lifeGateChar = this._tmpLifeGateChars[lifeGateCharIndex];
						int lifeGateNeedHealth = Math.Min(lifeGateChar.Item3 - lifeGateChar.Item2, distributableHealth);
						int distributedHealth = 0;
						for (int i = this._tmpDeathGateChars.Count - 1; i >= 0; i--)
						{
							ValueTuple<GameData.Domains.Character.Character, int> deathGateChar = this._tmpDeathGateChars[i];
							int transferHealth = lifeGateNeedHealth * deathGateChar.Item2 / distributableHealth;
							bool flag9 = transferHealth <= 0;
							if (!flag9)
							{
								deathGateChar.Item2 -= transferHealth;
								lifeGateChar.Item2 += transferHealth;
								distributedHealth += transferHealth;
								bool flag10 = deathGateChar.Item2 <= 0;
								if (flag10)
								{
									CollectionUtils.SwapAndRemove<ValueTuple<GameData.Domains.Character.Character, int>>(this._tmpDeathGateChars, i);
									deathGateChar.Item1.SetHealth(0, context);
									int deathGateCharId2 = deathGateChar.Item1.GetId();
									modifiedDeathGateCharIds.Add(deathGateCharId2);
									this.UpdateBaihuaFixedCharacterLocations(context, deathGateCharId2);
									lifeDeathGate.Cooldown = GlobalConfig.Instance.BaihuaLifeLinkRemoveCharacterCooldown;
								}
								else
								{
									this._tmpDeathGateChars[i] = deathGateChar;
								}
							}
						}
						bool flag11 = lifeGateChar.Item2 < lifeGateChar.Item3;
						if (flag11)
						{
							lifeGateNeedHealth = lifeGateChar.Item3 - lifeGateChar.Item2;
							while (lifeGateNeedHealth > 0 && this._tmpDeathGateChars.Count > 0)
							{
								int deathGateCharIndex = random.Next(this._tmpDeathGateChars.Count);
								ValueTuple<GameData.Domains.Character.Character, int> deathGateChar2 = this._tmpDeathGateChars[deathGateCharIndex];
								int transferredHealth = Math.Min(lifeGateNeedHealth, deathGateChar2.Item2);
								lifeGateChar.Item2 += transferredHealth;
								deathGateChar2.Item2 -= transferredHealth;
								lifeGateNeedHealth -= transferredHealth;
								distributedHealth += transferredHealth;
								bool flag12 = deathGateChar2.Item2 <= 0;
								if (flag12)
								{
									CollectionUtils.SwapAndRemove<ValueTuple<GameData.Domains.Character.Character, int>>(this._tmpDeathGateChars, deathGateCharIndex);
									deathGateChar2.Item1.SetHealth(0, context);
									int deathGateCharId3 = deathGateChar2.Item1.GetId();
									modifiedDeathGateCharIds.Add(deathGateCharId3);
									this.UpdateBaihuaFixedCharacterLocations(context, deathGateCharId3);
									lifeDeathGate.Cooldown = GlobalConfig.Instance.BaihuaLifeLinkRemoveCharacterCooldown;
								}
								else
								{
									this._tmpDeathGateChars[deathGateCharIndex] = deathGateChar2;
								}
							}
						}
						lifeGateChar.Item1.SetHealth((short)lifeGateChar.Item2, context);
						CollectionUtils.SwapAndRemove<ValueTuple<GameData.Domains.Character.Character, int, int>>(this._tmpLifeGateChars, lifeGateCharIndex);
						modifiedLifeGateCharIds.Add(lifeGateChar.Item1.GetId());
						Tester.Assert(distributedHealth <= distributableHealth, "");
						distributableHealth -= distributedHealth;
					}
					foreach (ValueTuple<GameData.Domains.Character.Character, int> deathGateChar3 in this._tmpDeathGateChars)
					{
						short prevHealth = deathGateChar3.Item1.GetHealth();
						int currHealth = deathGateChar3.Item2;
						bool flag13 = (int)prevHealth != currHealth;
						if (flag13)
						{
							deathGateChar3.Item1.SetHealth((short)currHealth, context);
							modifiedDeathGateCharIds.Add(deathGateChar3.Item1.GetId());
						}
					}
					MonthlyNotificationCollection monthlyNotifications = this.GetMonthlyNotificationCollection();
					for (int j = 0; j < modifiedLifeGateCharIds.Count; j++)
					{
						monthlyNotifications.AddLifeLinkHealing(*modifiedLifeGateCharIds[j]);
					}
					for (int k = 0; k < modifiedDeathGateCharIds.Count; k++)
					{
						monthlyNotifications.AddLifeLinkDamage(*modifiedDeathGateCharIds[k]);
					}
					DomainManager.Extra.SetSectBaihuaLifeLinkData(lifeDeathGate, context);
				}
			}
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x000ECC64 File Offset: 0x000EAE64
		[DomainMethod]
		public bool ShaolinInterruptDemonSlayerTrial(DataContext context)
		{
			SectShaolinDemonSlayerData data = DomainManager.Extra.GetSectShaolinDemonSlayerData();
			DomainManager.TaiwuEvent.CloseUI("ShaolinInterruptDemonSlayerTrial", false, data.TrialingLevel.TemplateId);
			bool success = data.ClearDemons();
			bool flag = success;
			if (flag)
			{
				DomainManager.Extra.SetSectShaolinDemonSlayerData(context, data);
			}
			return success;
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x000ECCB8 File Offset: 0x000EAEB8
		[DomainMethod]
		public bool ShaolinRegenerateRestricts(DataContext context)
		{
			SectShaolinDemonSlayerData data = DomainManager.Extra.GetSectShaolinDemonSlayerData();
			bool success = data.ReGenerateRestricts(context.Random);
			bool flag = success;
			if (flag)
			{
				DomainManager.Extra.SetSectShaolinDemonSlayerData(context, data);
			}
			return success;
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x000ECCF8 File Offset: 0x000EAEF8
		[DomainMethod]
		public byte ShaolinQueryRestrictsAreSatisfied(int index)
		{
			BoolArray8 result = 0;
			SectShaolinDemonSlayerData data = DomainManager.Extra.GetSectShaolinDemonSlayerData();
			int restrictIndex = 0;
			foreach (DemonSlayerTrialRestrictItem restrict in data.GetTrialingRestricts(index))
			{
				result[restrictIndex++] = restrict.Check();
			}
			return result;
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x000ECD78 File Offset: 0x000EAF78
		[DomainMethod]
		public void ShaolinStartDemonSlayerTrial(DataContext context, int index)
		{
			DomainManager.TaiwuEvent.OnEvent_StartSectShaolinDemonSlayer(index);
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x000ECD88 File Offset: 0x000EAF88
		[DomainMethod]
		public List<int> ShaolinGenerateTemporaryDemon(DataContext context)
		{
			List<int> demonCharIds = new List<int>();
			SectShaolinDemonSlayerData data = DomainManager.Extra.GetSectShaolinDemonSlayerData();
			for (int i = 0; i < 2; i++)
			{
				DemonSlayerTrialItem demon = data.GetTrialingDemon(i);
				bool flag = demon == null;
				if (!flag)
				{
					GameData.Domains.Character.Character character = this.CreateFixedDemon(context, demon.CharacterId);
					demonCharIds.Add(character.GetId());
				}
			}
			return demonCharIds;
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x000ECDF4 File Offset: 0x000EAFF4
		[DomainMethod]
		public void ShaolinClearTemporaryDemon(DataContext context, List<int> demonCharIds)
		{
			SectShaolinDemonSlayerData data = DomainManager.Extra.GetSectShaolinDemonSlayerData();
			for (int i = 0; i < 2; i++)
			{
				DemonSlayerTrialItem demon = data.GetTrialingDemon(i);
				bool flag = demon == null;
				if (!flag)
				{
					int charId = demonCharIds.GetOrDefault(i, -1);
					GameData.Domains.Character.Character character;
					bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
					if (!flag2)
					{
						bool flag3 = character.GetTemplateId() != demon.CharacterId;
						if (flag3)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Failed to clear demon template by mismatch ");
							defaultInterpolatedStringHandler.AppendFormatted<short>(character.GetTemplateId());
							AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
						}
						else
						{
							DomainManager.Character.RemoveNonIntelligentCharacter(context, character);
						}
					}
				}
			}
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x000ECEB8 File Offset: 0x000EB0B8
		public bool ShaolinGenerateDemonSlayerTrial(DataContext context)
		{
			SectShaolinDemonSlayerData data = DomainManager.Extra.GetSectShaolinDemonSlayerData();
			bool success = data.GenerateDemons(context.Random);
			bool flag = success;
			if (flag)
			{
				DomainManager.Extra.SetSectShaolinDemonSlayerData(context, data);
			}
			return success;
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x000ECEF8 File Offset: 0x000EB0F8
		public GameData.Domains.Character.Character CreateFixedDemon(DataContext context, short demonTemplateId)
		{
			ulong seed = (ulong)DomainManager.World.GetWorldId() + (ulong)((long)demonTemplateId);
			context.SwitchRandomSource(seed);
			GameData.Domains.Character.Character character = DomainManager.Character.CreateFixedEnemy(context, demonTemplateId);
			DomainManager.Character.CompleteCreatingCharacter(character.GetId());
			context.RestoreRandomSource();
			return character;
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x000ECF48 File Offset: 0x000EB148
		[DomainMethod]
		public int GetSectMainStoryTriggerConditions(short templateId)
		{
			int res = 0;
			List<Func<bool>> funcs = this._sectMainStoryTriggerConditions[templateId];
			for (int index = 0; index < funcs.Count; index++)
			{
				bool flag = funcs[index]();
				if (flag)
				{
					res |= 1 << index;
				}
			}
			return res;
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x000ECF9C File Offset: 0x000EB19C
		private static bool ShaolinMainStoryTrigger0()
		{
			Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(1);
			return DomainManager.Organization.GetElement_Sects(settlement.GetId()).GetTaiwuExploreStatus() == 2;
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x000ECFD4 File Offset: 0x000EB1D4
		private static bool ShaolinMainStoryTrigger1()
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
			short settlementId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(1);
			return taiwuLocation.IsValid() && DomainManager.Map.IsLocationOnSettlementBlock(taiwuLocation, settlementId);
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x000ED01C File Offset: 0x000EB21C
		private static bool EMeiMainStoryTrigger0()
		{
			return DomainManager.Organization.GetSettlementByOrgTemplateId(2).CalcApprovingRate() >= 500;
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x000ED048 File Offset: 0x000EB248
		private static bool EMeiMainStoryTrigger1()
		{
			short apeBlockId = -1;
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(2);
			bool whiteApeBlockIdExist = sectArgBox.Get("ConchShip_PresetKey_WhiteApeBlockId", ref apeBlockId);
			bool flag = !whiteApeBlockIdExist;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
				Location emeiLocation = DomainManager.Organization.GetSettlementByOrgTemplateId(2).GetLocation();
				bool flag2 = emeiLocation.AreaId != taiwuLocation.AreaId;
				if (flag2)
				{
					result = false;
				}
				else
				{
					MapBlockData taiwuBlockData = DomainManager.Map.GetBlockData(taiwuLocation.AreaId, taiwuLocation.BlockId);
					ByteCoordinate apeBlockPos = DomainManager.Map.GetBlockData(emeiLocation.AreaId, apeBlockId).GetBlockPos();
					result = (taiwuBlockData.GetManhattanDistanceToPos(apeBlockPos.X, apeBlockPos.Y) <= 3);
				}
			}
			return result;
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x000ED118 File Offset: 0x000EB318
		private static bool BaihuaMainStoryTrigger0()
		{
			return DomainManager.World.GetMainStoryLineProgress() >= 22;
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x000ED13C File Offset: 0x000EB33C
		private static bool BaihuaMainStoryTrigger1()
		{
			return DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId == DomainManager.Map.GetAreaIdByAreaTemplateId(18);
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x000ED170 File Offset: 0x000EB370
		private static bool WudangMainStoryTrigger0()
		{
			short settlementId = DomainManager.Organization.GetSettlementByOrgTemplateId(4).GetId();
			return DomainManager.Organization.GetElement_Sects(settlementId).GetTaiwuExploreStatus() == 2;
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x000ED1A8 File Offset: 0x000EB3A8
		private static bool WudangMainStoryTrigger1()
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			Location wudangLocation = DomainManager.Organization.GetSettlementByOrgTemplateId(4).GetLocation();
			return taiwuLocation.AreaId == wudangLocation.AreaId;
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x000ED1EC File Offset: 0x000EB3EC
		private static bool YuanshanMainStoryTrigger0()
		{
			return DomainManager.World.GetMainStoryLineProgress() >= 21;
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x000ED210 File Offset: 0x000EB410
		private static bool YuanshanMainStoryTrigger1()
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			Location yuanshanLocation = DomainManager.Organization.GetSettlementByOrgTemplateId(5).GetLocation();
			return taiwuLocation.AreaId == yuanshanLocation.AreaId;
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x000ED254 File Offset: 0x000EB454
		private static bool YuanshanMainStoryTrigger2()
		{
			return DomainManager.Organization.GetSettlementByOrgTemplateId(5).CalcApprovingRate() >= 500;
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x000ED280 File Offset: 0x000EB480
		private static bool ShixiangMainStoryTrigger0()
		{
			return DomainManager.Organization.GetSettlementByOrgTemplateId(6).CalcApprovingRate() >= 500;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x000ED2AC File Offset: 0x000EB4AC
		private static bool ShixiangMainStoryTrigger1()
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			bool flag = !taiwuLocation.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte stateId = DomainManager.Map.GetStateIdByAreaId(taiwuLocation.AreaId);
				sbyte guangdongStateId = DomainManager.Map.GetStateIdByStateTemplateId(6);
				MapBlockData taiwuBlockData = DomainManager.Map.GetBlockData(taiwuLocation.AreaId, taiwuLocation.BlockId);
				bool flag2 = stateId == guangdongStateId;
				bool flag3 = flag2;
				if (flag3)
				{
					EMapBlockType blockType = taiwuBlockData.BlockType;
					bool flag4 = blockType == EMapBlockType.City || blockType == EMapBlockType.Town;
					flag3 = flag4;
				}
				result = flag3;
			}
			return result;
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x000ED34C File Offset: 0x000EB54C
		private static bool RanshanMainStoryTrigger0()
		{
			return DomainManager.Organization.GetSettlementByOrgTemplateId(7).CalcApprovingRate() >= 500;
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x000ED378 File Offset: 0x000EB578
		private static bool RanshanMainStoryTrigger1()
		{
			int legendaryBookCount = 0;
			for (sbyte combatSkillType = 0; combatSkillType < 14; combatSkillType += 1)
			{
				bool flag = DomainManager.Item.HasTrackedSpecialItems(12, (short)(211 + (int)combatSkillType));
				if (flag)
				{
					legendaryBookCount++;
				}
			}
			return legendaryBookCount >= 3;
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x000ED3C4 File Offset: 0x000EB5C4
		private static bool RanshanMainStoryTrigger2()
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
			return taiwuLocation.IsValid() && !MapAreaData.IsBrokenArea(taiwuLocation.AreaId);
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x000ED400 File Offset: 0x000EB600
		private static bool XuannvMainStoryTrigger0()
		{
			Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(8);
			return DomainManager.Organization.GetElement_Sects(settlement.GetId()).GetTaiwuExploreStatus() == 2;
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x000ED438 File Offset: 0x000EB638
		private static bool XuannvMainStoryTrigger1()
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
			short settlementId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(8);
			return taiwuLocation.IsValid() && DomainManager.Map.IsLocationOnSettlementBlock(taiwuLocation, settlementId);
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x000ED480 File Offset: 0x000EB680
		private static bool ZhujianMainStoryTrigger0()
		{
			return DomainManager.Organization.GetSettlementByOrgTemplateId(9).CalcApprovingRate() >= 500;
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x000ED4B0 File Offset: 0x000EB6B0
		private static bool ZhujianMainStoryTrigger1()
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			bool flag = taiwuLocation.IsValid() && !MapAreaData.IsBrokenArea(taiwuLocation.AreaId);
			bool result;
			if (flag)
			{
				sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(taiwuLocation.AreaId);
				result = (stateTemplateId == 9);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x000ED50C File Offset: 0x000EB70C
		private static bool KongsangMainStoryTrigger0()
		{
			return DomainManager.Organization.GetSettlementByOrgTemplateId(10).CalcApprovingRate() >= 500;
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x000ED53C File Offset: 0x000EB73C
		private static bool KongsangMainStoryTrigger1()
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
			Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(10);
			bool flag = !DomainManager.Map.IsLocationOnSettlementBlock(taiwuLocation, settlement.GetId());
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				MapBlockData taiwuBlockData = DomainManager.Map.GetBlockData(taiwuLocation.AreaId, taiwuLocation.BlockId);
				bool flag2 = taiwuBlockData.CharacterSet == null || taiwuBlockData.CharacterSet.Count == 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					GameData.Domains.Character.Character leader = settlement.GetLeader();
					bool flag3 = leader == null;
					if (flag3)
					{
						result = false;
					}
					else
					{
						int leaderId = leader.GetId();
						result = taiwuBlockData.CharacterSet.Contains(leaderId);
					}
				}
			}
			return result;
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x000ED5F4 File Offset: 0x000EB7F4
		private static bool JingangMainStoryTrigger0()
		{
			return DomainManager.Building.IsTaiwuVillageHaveSpecifyBuilding(50, true);
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x000ED614 File Offset: 0x000EB814
		private static bool JingangMainStoryTrigger1()
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
			bool flag = DomainManager.Map.IsAreaBroken(taiwuLocation.AreaId);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(taiwuLocation.AreaId);
				result = (stateTemplateIdByAreaId == 11);
			}
			return result;
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x000ED664 File Offset: 0x000EB864
		private static bool WuxianMainStoryTrigger0()
		{
			short settlementId = DomainManager.Organization.GetSettlementByOrgTemplateId(12).GetId();
			return DomainManager.Organization.GetElement_Sects(settlementId).GetSpiritualDebtInteractionOccurred();
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x000ED698 File Offset: 0x000EB898
		private static bool WuxianMainStoryTrigger1()
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
			short settlementId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(12);
			return taiwuLocation.IsValid() && DomainManager.Map.IsLocationInSettlementInfluenceRange(taiwuLocation, settlementId);
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x000ED6E0 File Offset: 0x000EB8E0
		private static bool FulongMainStoryTrigger0()
		{
			GameData.Domains.Character.Character character;
			return DomainManager.Character.TryGetFixedCharacterByTemplateId(446, out character) && !DomainManager.TaiwuEvent.GetGlobalEventArgumentBox().Contains<bool>("YuFuTellRanchenziStory");
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x000ED720 File Offset: 0x000EB920
		private static bool FulongMainStoryTrigger1()
		{
			return DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId == DomainManager.Map.GetAreaIdByAreaTemplateId(29);
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x000ED754 File Offset: 0x000EB954
		private static bool XuehouMainStoryTrigger0()
		{
			short settlementId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(35);
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			return taiwuLocation.IsValid() && DomainManager.Map.IsLocationOnSettlementBlock(taiwuLocation, settlementId);
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x000ED79C File Offset: 0x000EB99C
		[DomainMethod]
		public int TryTriggerThiefCatch(DataContext context)
		{
			Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
			SectStoryThiefData thiefData;
			int thiefIndex;
			bool flag = !this.TryGetThief(location, out thiefData, out thiefIndex);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = thiefData.ThiefTriggered[thiefIndex];
				if (flag2)
				{
					result = -1;
				}
				else
				{
					List<SectStoryThiefData> thiefList = DomainManager.Extra.GetSectZhujianThiefList();
					thiefData.ThiefTriggered[thiefIndex] = true;
					bool isRealThief = thiefIndex == thiefData.RealThiefIndex;
					bool flag3 = isRealThief || thiefData.AllIsTriggered();
					if (flag3)
					{
						thiefList.Remove(thiefData);
					}
					else
					{
						thiefData.UpdatePlace(context.Random);
					}
					DomainManager.Extra.SetSectZhujianThiefList(thiefList, context);
					result = (isRealThief ? thiefData.CatchThiefTimes : -1);
				}
			}
			return result;
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x000ED85B File Offset: 0x000EBA5B
		[DomainMethod]
		public void CatchThief(sbyte thiefLevel, bool timeOut)
		{
			DomainManager.TaiwuEvent.OnEvent_CatchThief(thiefLevel, timeOut);
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x000ED86C File Offset: 0x000EBA6C
		public void CreateNewThief(DataContext context, short areaId)
		{
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(9);
			int catchTimes = sectArgBox.GetInt("ConchShip_PresetKey_ZhujianCatchThiefTimes");
			SectStoryThiefData thiefData = this.CreateThiefData(context.Random, catchTimes, areaId);
			foreach (short blockId in thiefData.ThiefBlockIds)
			{
				MapBlockData blockData = DomainManager.Map.GetBlock(areaId, blockId);
				bool flag = !blockData.Visible;
				if (flag)
				{
					blockData.SetVisible(true, context);
				}
			}
			List<SectStoryThiefData> thiefList = DomainManager.Extra.GetSectZhujianThiefList();
			thiefList.Add(thiefData);
			DomainManager.Extra.SetSectZhujianThiefList(thiefList, context);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x000ED930 File Offset: 0x000EBB30
		private unsafe SectStoryThiefData CreateThiefData(IRandomSource random, int catchTimes, short areaId)
		{
			SectStoryThiefData data = new SectStoryThiefData
			{
				CatchThiefTimes = catchTimes,
				AreaId = areaId,
				ThiefBlockIds = new List<short>(),
				ThiefTriggered = new List<bool>(),
				RealThiefIndex = random.Next(3)
			};
			List<MapBlockData> availableBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			availableBlocks.Clear();
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
			for (int j = 0; j < areaBlocks.Length; j++)
			{
				MapBlockData block = *areaBlocks[j];
				bool flag = SectStoryThiefDataHelper.IsBlockAvailable(block);
				if (flag)
				{
					availableBlocks.Add(block);
				}
			}
			CollectionUtils.Shuffle<MapBlockData>(random, availableBlocks);
			List<MapBlockData> neighborList = ObjectPool<List<MapBlockData>>.Instance.Get();
			foreach (MapBlockData block2 in availableBlocks)
			{
				DomainManager.Map.GetRealNeighborBlocks(block2.AreaId, block2.BlockId, neighborList, 1, false);
				neighborList.RemoveAll(new Predicate<MapBlockData>(SectStoryThiefDataHelper.IsBlockUnAvailable));
				bool flag2 = neighborList.Count < 2;
				if (!flag2)
				{
					CollectionUtils.Shuffle<MapBlockData>(random, neighborList);
					data.ThiefBlockIds.Add(block2.BlockId);
					data.ThiefBlockIds.Add(neighborList[0].BlockId);
					data.ThiefBlockIds.Add(neighborList[1].BlockId);
					for (int i = 0; i < 3; i++)
					{
						data.ThiefTriggered.Add(false);
					}
					break;
				}
			}
			bool flag3 = data.ThiefBlockIds.Count == 0;
			if (flag3)
			{
				short predefinedLogId = 11;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Create thief failed in ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(areaId);
				PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(availableBlocks);
			ObjectPool<List<MapBlockData>>.Instance.Return(neighborList);
			return data;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x000EDB38 File Offset: 0x000EBD38
		public bool TryGetThief(Location location, out SectStoryThiefData thiefData, out int thiefIndex)
		{
			List<SectStoryThiefData> thiefList = DomainManager.Extra.GetSectZhujianThiefList();
			foreach (SectStoryThiefData thief in thiefList)
			{
				bool flag = thief.AreaId != location.AreaId;
				if (!flag)
				{
					for (int i = 0; i < thief.ThiefBlockIds.Count; i++)
					{
						bool flag2 = thief.ThiefBlockIds[i] != location.BlockId;
						if (!flag2)
						{
							thiefData = thief;
							thiefIndex = i;
							return true;
						}
					}
				}
			}
			thiefData = null;
			thiefIndex = -1;
			return false;
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x000EDC00 File Offset: 0x000EBE00
		public void UpdateAreaMerchantType(DataContext context)
		{
			foreach (KeyValuePair<short, sbyte> keyValuePair in DomainManager.Extra.SectZhujianAreaMerchantTypeDict)
			{
				short num;
				sbyte b;
				keyValuePair.Deconstruct(out num, out b);
				short areaTemplateId = num;
				sbyte merchantType = b;
				short areaId = DomainManager.Map.GetAreaIdByAreaTemplateId(areaTemplateId);
				MapAreaData areaData = DomainManager.Map.GetAreaByAreaId(areaId);
				List<SettlementInfo> settlementInfoList = areaData.SettlementInfos.ToList<SettlementInfo>();
				foreach (SettlementInfo settlementInfo in areaData.SettlementInfos)
				{
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
					OrganizationItem orgConfig = Organization.Instance[settlement.GetOrgTemplateId()];
					bool isSect = orgConfig.IsSect;
					if (isSect)
					{
						settlementInfoList.Remove(settlementInfo);
					}
				}
				CollectionUtils.Shuffle<SettlementInfo>(context.Random, settlementInfoList);
				Dictionary<int, sbyte> charToType = new Dictionary<int, sbyte>();
				foreach (SettlementInfo settlementInfo2 in settlementInfoList)
				{
					Settlement settlement2 = DomainManager.Organization.GetSettlement(settlementInfo2.SettlementId);
					List<int> charList = new List<int>();
					settlement2.GetMembers().GetAllMembers(charList);
					foreach (int charId in charList)
					{
						GameData.Domains.Character.Character character;
						bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
						if (!flag)
						{
							bool flag2 = !character.IsInteractableAsIntelligentCharacter();
							if (!flag2)
							{
								bool flag3 = !OrganizationDomain.CanInteractWithType(character, 4);
								if (!flag3)
								{
									sbyte type;
									bool flag4 = DomainManager.Extra.TryGetMerchantCharToType(charId, out type);
									if (flag4)
									{
										charToType[charId] = type;
									}
								}
							}
						}
					}
				}
				bool hasTargetChar = false;
				foreach (KeyValuePair<int, sbyte> keyValuePair2 in charToType)
				{
					int num2;
					keyValuePair2.Deconstruct(out num2, out b);
					sbyte type2 = b;
					bool flag5 = type2 == merchantType;
					if (flag5)
					{
						hasTargetChar = true;
						break;
					}
				}
				bool flag6 = hasTargetChar;
				if (!flag6)
				{
					bool flag7 = charToType.Count > 0;
					if (flag7)
					{
						int charId2 = charToType.Keys.ToList<int>().GetRandom(context.Random);
						GameData.Domains.Character.Character character2 = DomainManager.Character.GetElement_Objects(charId2);
						character2.AddOrSetMerchantType(merchantType, context);
						DomainManager.Map.SetBlockData(context, DomainManager.Map.GetBlock(character2.GetLocation()));
					}
					else
					{
						SettlementInfo randomSettlementInfo = settlementInfoList.GetRandom(context.Random);
						Settlement randomSettlement = DomainManager.Organization.GetSettlement(randomSettlementInfo.SettlementId);
						Location location = randomSettlement.GetLocation();
						sbyte gender = Gender.GetRandom(context.Random);
						short age = 16;
						GameData.Domains.Character.Character newChar = EventHelper.CreateIntelligentCharacter(location, gender, age, -1, randomSettlementInfo.SettlementId, 4);
						newChar.AddOrSetMerchantType(merchantType, context);
						DomainManager.Map.SetBlockData(context, DomainManager.Map.GetBlock(location));
					}
				}
			}
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x000EDF8C File Offset: 0x000EC18C
		[DomainMethod]
		public void SpecifyWorldPopulationType(DataContext context, byte worldPopulationType)
		{
			byte oriType = this._worldPopulationType;
			this.SetWorldPopulationType(worldPopulationType, context);
			this.ChangeWorldPopulation(context, oriType);
			DomainManager.Organization.ChangeSettlementStandardPopulations(context, oriType);
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x000EDFC0 File Offset: 0x000EC1C0
		public int GetWorldCreationSetting(byte worldCreationType)
		{
			if (!true)
			{
			}
			int num;
			switch (worldCreationType)
			{
			case 1:
				num = (int)this.GetCombatDifficulty();
				goto IL_CA;
			case 2:
				num = (int)this.GetHereticsAmountType();
				goto IL_CA;
			case 3:
				num = (int)this.GetBossInvasionSpeedType();
				goto IL_CA;
			case 4:
				num = (int)this.GetWorldResourceAmountType();
				goto IL_CA;
			case 8:
				num = (int)DomainManager.Extra.GetReadingDifficulty();
				goto IL_CA;
			case 9:
				num = (int)DomainManager.Extra.GetBreakoutDifficulty();
				goto IL_CA;
			case 10:
				num = (int)DomainManager.Extra.GetLoopingDifficulty();
				goto IL_CA;
			case 11:
				num = (int)DomainManager.Extra.GetEnemyPracticeLevel();
				goto IL_CA;
			case 12:
				num = (int)DomainManager.Extra.GetFavorabilityChange();
				goto IL_CA;
			case 13:
				num = (int)DomainManager.Extra.GetProfessionUpgrade();
				goto IL_CA;
			case 14:
				num = (int)DomainManager.Extra.GetLootYield();
				goto IL_CA;
			}
			num = -1;
			IL_CA:
			if (!true)
			{
			}
			int setting = num;
			bool flag = setting < 0;
			if (flag)
			{
				short predefinedLogId = 19;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
				defaultInterpolatedStringHandler.AppendLiteral("GetSettingWorldCreationType by ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(worldCreationType);
				PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return setting;
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x000EE0DC File Offset: 0x000EC2DC
		public int GetWorldPopulationFactor()
		{
			return (int)WorldCreation.Instance[5].InfluenceFactors[(int)this._worldPopulationType];
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x000EE108 File Offset: 0x000EC308
		public static int GetWorldPopulationFactor(byte worldPopulationType)
		{
			return (int)WorldCreation.Instance[5].InfluenceFactors[(int)worldPopulationType];
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x000EE12C File Offset: 0x000EC32C
		public int GetCharacterLifeSpanFactor()
		{
			return (int)WorldCreation.Instance[0].InfluenceFactors[(int)this._characterLifespanType];
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x000EE158 File Offset: 0x000EC358
		public int GetHereticsAmountFactor()
		{
			return (int)WorldCreation.Instance[2].InfluenceFactors[(int)this._hereticsAmountType];
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x000EE184 File Offset: 0x000EC384
		public int GetBossInvasionSpeed()
		{
			return (int)WorldCreation.Instance[3].InfluenceFactors[(int)this._bossInvasionSpeedType];
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x000EE1B0 File Offset: 0x000EC3B0
		public int GetGainResourcePercent(byte type)
		{
			return (int)WorldResource.Instance[type].InfluenceFactors[(int)this._worldResourceAmountType];
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x000EE1DC File Offset: 0x000EC3DC
		[return: TupleElementNames(new string[]
		{
			"Value",
			"Reciprocal"
		})]
		public ValueTuple<int, bool> GetFavorabilityChangePercent(short type, bool isFavorabilityGainFixed)
		{
			bool flag = type < 0;
			ValueTuple<int, bool> result;
			if (flag)
			{
				result = new ValueTuple<int, bool>(100, false);
			}
			else
			{
				byte favorabilityChange = DomainManager.Extra.GetFavorabilityChange();
				if (isFavorabilityGainFixed)
				{
					type = 6;
				}
				WorldFavorabilityItem worldFavorability = WorldFavorability.Instance[type];
				result = new ValueTuple<int, bool>((int)worldFavorability.InfluenceFactors[(int)favorabilityChange], worldFavorability.NegativeUsingReciprocal);
			}
			return result;
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x000EE238 File Offset: 0x000EC438
		public short GetCurrYear()
		{
			return (short)(this._currDate / 12);
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x000EE254 File Offset: 0x000EC454
		public sbyte GetCurrMonthInYear()
		{
			return (sbyte)(this._currDate % 12);
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x000EE270 File Offset: 0x000EC470
		public bool CheckDateInterval(int beginDate, int expectedInterval)
		{
			int monthCount = this._currDate - beginDate;
			return monthCount / expectedInterval > 0 && monthCount % expectedInterval == 0;
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x000EE29C File Offset: 0x000EC49C
		public int GetLeftDaysInCurrMonth()
		{
			return DomainManager.Extra.GetTotalActionPointsRemaining() / 10;
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x000EE2BB File Offset: 0x000EC4BB
		[DomainMethod]
		public void AdvanceDaysInMonth(DataContext context, int days)
		{
			DomainManager.Extra.ConsumeActionPoint(context, days * 10);
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x000EE2CE File Offset: 0x000EC4CE
		public void ConsumeActionPoint(DataContext context, int actionPoints)
		{
			DomainManager.Extra.ConsumeActionPoint(context, actionPoints);
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x000EE2E0 File Offset: 0x000EC4E0
		[DomainMethod]
		public void AdvanceMonth(DataContext context)
		{
			bool flag = this._advancingMonthState != 0;
			if (flag)
			{
				Logger logger = WorldDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 2);
				defaultInterpolatedStringHandler.AppendFormatted("AdvanceMonth");
				defaultInterpolatedStringHandler.AppendLiteral(": Wrong AdvancingMonthState: ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this._advancingMonthState);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			else
			{
				WorldDomain.Logger.Info("AdvanceMonth: begin ------------------------------------------------------------");
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				ValueTuple<DataMonitorManager, NotificationCollection> valueTuple = GameDataBridge.StartSemiBlockingTask();
				DataMonitorManager monitor = valueTuple.Item1;
				NotificationCollection oriPendingNotifications = valueTuple.Item2;
				monitor.MonitorData(new DataUid(1, 29, ulong.MaxValue, uint.MaxValue));
				monitor.MonitorData(new DataUid(1, 27, ulong.MaxValue, uint.MaxValue));
				this._monthlyEventCollection.Clear();
				this._currMonthlyNotifications.Clear();
				DomainManager.Merchant.UpdateCaravansMove(context);
				DomainManager.Information.ClearedTaiwuReceivedSecretInformationInMonth();
				CharacterDomain.ClearLockMovementCharSet();
				DomainManager.LegendaryBook.ClearActCrazyShockedCharacters();
				DomainManager.Adventure.RemoveAllTemporaryEnemies(context);
				DomainManager.Adventure.RemoveAllTemporaryIntelligentCharacters(context);
				DomainManager.Building.RemoveTemporaryPossessionCharacter(context);
				DomainManager.Map.ClearHunterAnim(context);
				DomainManager.Information.PrepareSecretInformationAdvanceMonth();
				DomainManager.Character.ResetAllAdvanceMonthStatus();
				this.AdvanceMonth_CheckPrerequisites(context);
				Events.RaiseAdvanceMonthBegin(context);
				Logger logger2 = WorldDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 3);
				defaultInterpolatedStringHandler.AppendLiteral("New month begin: Year ");
				defaultInterpolatedStringHandler.AppendFormatted<int>((int)(this.GetCurrYear() + 1));
				defaultInterpolatedStringHandler.AppendLiteral(", Month ");
				defaultInterpolatedStringHandler.AppendFormatted<int>((int)(this.GetCurrMonthInYear() + 1));
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._currDate);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				bool flag2 = this._mainStoryLineProgress >= 3;
				if (flag2)
				{
					this.PreAdvanceMonth(context, monitor);
					this.PeriAdvanceMonth(context, monitor);
					this.PostAdvanceMonth(context, monitor);
				}
				else
				{
					this.PreAdvanceMonth_BornArea(context, monitor);
					this.PeriAdvanceMonth_BornArea(context, monitor);
					this.PostAdvanceMonth_BornArea(context, monitor);
				}
				GameDataBridge.StopSemiBlockingTask(monitor, oriPendingNotifications);
				CharacterDomain.ClearLockMovementCharSet();
				this.CheckMonthlyEvents(context);
				this.CheckMonthlyNotifications();
				this.TransferMonthlyNotifications(context);
				stopwatch.Stop();
				bool flag3 = stopwatch.ElapsedMilliseconds < 2000L;
				if (flag3)
				{
					Thread.Sleep((int)(2000L - stopwatch.ElapsedMilliseconds));
				}
				this.SetAdvancingMonthState(20, context);
				WorldDomain.Logger.Info("AdvanceMonth: end --------------------------------------------------------------");
			}
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x000EE574 File Offset: 0x000EC774
		[DomainMethod]
		public void AdvanceMonth_DisplayedMonthlyNotifications(DataContext context, bool saveWorld)
		{
			WorldDomain.Logger.Info(saveWorld ? "Exit advancing month state and start saving world." : "Exit advancing month state without saving world.");
			bool flag = this._advancingMonthState != 20;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Wrong AdvancingMonthState: ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this._advancingMonthState);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.SetAdvancingMonthState(0, context);
			Events.RaiseAdvanceMonthFinish(context);
			Events.ClearPassingLegacyWhileAdvancingMonthHandlers(context);
			this.RemoveObsoletedInstantNotifications(context);
			if (saveWorld)
			{
				DomainManager.Global.SaveWorld(context);
			}
			this.UpdatePopulationRelatedData();
			this.ShowWorldStatistics();
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x000EE61D File Offset: 0x000EC81D
		private void AdvanceMonth_CheckPrerequisites(DataContext context)
		{
			WorldDomain.CheckSanity();
			DomainManager.Global.CheckDriveSpace(context);
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x000EE632 File Offset: 0x000EC832
		public static void CheckSanity()
		{
			DomainManager.Character.CheckCharacterCreationState();
			DomainManager.Character.CheckTemporaryIntelligentCharacters();
			DomainManager.Character.CheckCharacterTemporaryModificationState();
			DomainManager.Character.Test_SoftCheckGroups();
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x000EE661 File Offset: 0x000EC861
		public void ShowWorldStatistics()
		{
			DomainManager.Character.ShowCharactersStats();
			DomainManager.Character.ShowNonIntelligentCharactersStats();
			EventArgBox.ShowStatus();
			DomainManager.Item.CheckUnownedItems();
			GlobalDomain.ShowMemoryUsage();
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x000EE691 File Offset: 0x000EC891
		private void SetAndNotifyAdvancingMonthState(DataContext context, sbyte value, DataMonitorManager monitor)
		{
			this.SetAdvancingMonthState(value, context);
			monitor.CheckMonitoredData();
			GameDataBridge.TransferPendingNotifications();
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x000EE6AC File Offset: 0x000EC8AC
		private void AddSolarTermNotification(int month)
		{
			switch (month)
			{
			case 0:
				this._currMonthlyNotifications.AddSolarTerm0();
				break;
			case 1:
				this._currMonthlyNotifications.AddSolarTerm1();
				break;
			case 2:
				this._currMonthlyNotifications.AddSolarTerm2();
				break;
			case 3:
				this._currMonthlyNotifications.AddSolarTerm3();
				break;
			case 4:
				this._currMonthlyNotifications.AddSolarTerm4();
				break;
			case 5:
				this._currMonthlyNotifications.AddSolarTerm5();
				break;
			case 6:
				this._currMonthlyNotifications.AddSolarTerm6();
				break;
			case 7:
				this._currMonthlyNotifications.AddSolarTerm7();
				break;
			case 8:
				this._currMonthlyNotifications.AddSolarTerm8();
				break;
			case 9:
				this._currMonthlyNotifications.AddSolarTerm9();
				break;
			case 10:
				this._currMonthlyNotifications.AddSolarTerm10();
				break;
			case 11:
				this._currMonthlyNotifications.AddSolarTerm11();
				break;
			}
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x000EE7A8 File Offset: 0x000EC9A8
		private void PeriAdvanceMonth(DataContext context, DataMonitorManager monitor)
		{
			Stopwatch sw = Stopwatch.StartNew();
			Location location;
			bool disableAiActions = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox().Get<Location>("MainStoryLine_SpiritualWanderPlace_TaiwuVillagersCenter", out location);
			this.SetAndNotifyAdvancingMonthState(context, 2, monitor);
			DomainManager.Character.AssassinationByJieqing(context);
			WorkerThreadManager.Run(new Action<DataContext, int>(WorldDomain.PeriAdvanceMonth_CharacterMixedPoisonEffect), -1, 139, monitor, 100);
			WorkerThreadManager.Run(new Action<DataContext, int>(WorldDomain.PeriAdvanceMonth_UpdateCharacterStatus), -2, 139, monitor, 100);
			this.UpdateLifeDeathGateCharacters(context);
			DomainManager.Character.UpdateIntelligentCharacterAliveStates(context);
			DomainManager.Character.RecoverGuards(context);
			DomainManager.Character.UpdateGroupFavorabilities(context);
			DomainManager.Character.UpdateKidnappedCharacters(context);
			DomainManager.Character.UpdatePregnancyUnlockDates(context);
			DomainManager.Taiwu.UpdateChildrenEducation(context);
			DomainManager.LegendaryBook.UpdateLegendaryBookOwnersStatuses(context);
			DomainManager.Extra.RecoverHunterCarrierAttackCount(context);
			DomainManager.Extra.TryRestoreCharacterAvatars(context);
			DomainManager.Taiwu.AddChoosyRemainUpgradeData(context);
			sw.Stop();
			Logger logger = WorldDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PeriAdvanceMonth.UpdateCharacterStatus: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			DomainManager.Extra.MartialArtistSkill3Execute(context, false);
			this.SetAndNotifyAdvancingMonthState(context, 3, monitor);
			WorkerThreadManager.Run(new Action<DataContext, int>(DomainManager.Adventure.PreAdvanceMonth_UpdateRandomEnemies), 0, 139, monitor, 100);
			Thread.Sleep(20);
			sw.Stop();
			Logger logger2 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PeriAdvanceMonth.UpdateRandomEnemies: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			this.SetAndNotifyAdvancingMonthState(context, 4, monitor);
			WorkerThreadManager.Run(new Action<DataContext, int>(WorldDomain.PeriAdvanceMonth_CharacterSelfImprovement), -1, 139, monitor, 100);
			DomainManager.Taiwu.UpdateReadingProgressOnMonthChange(context);
			DomainManager.Extra.DeleteExpiredReadingStrategiesOnMonthChange(context);
			DomainManager.Taiwu.ClearActiveReadingProgressOnMonthChange(context);
			DomainManager.Taiwu.ClearActiveNeigongLoopingProgressOnMonthChange(context);
			DomainManager.Taiwu.ResetLoopInCombatCounts(context);
			DomainManager.Taiwu.ClearExpiredQiArtStrategies(context);
			DomainManager.Taiwu.TryAddLoopingEvent(context, (int)GlobalConfig.Instance.BaseLoopingEventProbability);
			DomainManager.Taiwu.GenerateAllFollowingMonthNotifications();
			DomainManager.Extra.UpdateSeniorityForCharacterProfessions(context);
			WorkerThreadManager.Run(new Action<DataContext, int>(WorldDomain.PeriAdvanceMonth_CharacterSelfImprovement_PracticeAndBreakout), -1, 139, monitor, 100);
			WorkerThreadManager.Run(new Action<DataContext, int>(WorldDomain.PeriAdvanceMonth_CharacterSelfImprovement_LearnNewSkills), -1, 139, monitor, 100);
			sw.Stop();
			Logger logger3 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PeriAdvanceMonth.CharacterSelfImprovement: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger3.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			this.SetAndNotifyAdvancingMonthState(context, 5, monitor);
			WorkerThreadManager.Run(new Action<DataContext, int>(WorldDomain.PeriAdvanceMonth_CharacterActivePreparation_GetSupply), -1, 135, monitor, 100);
			WorkerThreadManager.Run(new Action<DataContext, int>(WorldDomain.PeriAdvanceMonth_CharacterActivePreparation), -1, 139, monitor, 100);
			sw.Stop();
			Logger logger4 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PeriAdvanceMonth.CharacterActivePreparation: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger4.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			this.SetAndNotifyAdvancingMonthState(context, 6, monitor);
			DomainManager.Item.UpdateCrickets(context);
			WorkerThreadManager.Run(new Action<DataContext, int>(WorldDomain.PeriAdvanceMonth_CharacterPassivePreparation), -1, 139, monitor, 100);
			DomainManager.Taiwu.UpdateVillagerTreasuryNeed(context);
			DomainManager.Taiwu.LoseOverloadResources(context);
			sw.Stop();
			Logger logger5 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PeriAdvanceMonth.CharacterPassivePreparation: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger5.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			this.SetAndNotifyAdvancingMonthState(context, 7, monitor);
			WorkerThreadManager.Run(new Action<DataContext, int>(WorldDomain.PeriAdvanceMonth_CharacterRelationsUpdate), -1, 139, monitor, 100);
			sw.Stop();
			Logger logger6 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PeriAdvanceMonth.CharacterRelationsUpdate: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger6.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			DomainManager.Character.UpdateDistantMarriages(context);
			DomainManager.Character.UpdateAdoreRelationsInMarriage(context);
			sw.Stop();
			Logger logger7 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PeriAdvanceMonth.UpdateAdoreRelationsInMarriage: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger7.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			DomainManager.Organization.ExpandAllFactions(context);
			sw.Stop();
			Logger logger8 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PeriAdvanceMonth.ExpandAllFactions: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger8.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			this.SetAndNotifyAdvancingMonthState(context, 8, monitor);
			WorkerThreadManager.Run(new Action<DataContext, int>(WorldDomain.PeriAdvanceMonth_CharacterPersonalNeedsProcessing), -1, 139, monitor, 100);
			Thread.Sleep(20);
			sw.Stop();
			Logger logger9 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PeriAdvanceMonth.CharacterPersonalNeedsProcessing: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger9.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			DomainManager.Character.UpdateInfectedCharacterActions(context);
			DomainManager.LegendaryBook.UpdateLegendaryBookOwnersActions(context);
			sw.Stop();
			Logger logger10 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PeriAdvanceMonth.UpdateInfectedCharacterActions: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger10.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			bool flag = !disableAiActions;
			if (flag)
			{
				DomainManager.Character.PrepareForPrioritizedAction(context);
				this.SetAndNotifyAdvancingMonthState(context, 9, monitor);
				WorkerThreadManager.Run(new Action<DataContext, int>(WorldDomain.PeriAdvanceMonth_CharacterPrioritizedAction), 0, 139, monitor, 100);
			}
			sw.Stop();
			Logger logger11 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PeriAdvanceMonth.CharacterPrioritizedAction: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger11.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			bool flag2 = !disableAiActions;
			if (flag2)
			{
				this.SetAndNotifyAdvancingMonthState(context, 10, monitor);
				WorkerThreadManager.Run(new Action<DataContext, int>(WorldDomain.PeriAdvanceMonth_CharacterGeneralAction), -1, 139, monitor, 100);
			}
			sw.Stop();
			Logger logger12 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PeriAdvanceMonth.CharacterGeneralAction: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger12.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			bool flag3 = !disableAiActions;
			if (flag3)
			{
				this.SetAndNotifyAdvancingMonthState(context, 11, monitor);
				WorkerThreadManager.Run(new Action<DataContext, int>(WorldDomain.PeriAdvanceMonth_CharacterFixedAction), 0, 139, monitor, 100);
				DomainManager.Organization.UpdateFugitiveGroupsOnAdvanceMonth(context);
				DomainManager.Character.UpdateExceedingGroupChars(context);
				DomainManager.Taiwu.UpdateVillagerFixedActions(context);
			}
			sw.Stop();
			Logger logger13 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PeriAdvanceMonth.CharacterFixedAction: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger13.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			DomainManager.Taiwu.MoveVillagersToWorkLocation(context);
			DomainManager.Character.RecoverSkillBookLibraries(context);
			DomainManager.Extra.UpdateItemPriceFluctuations(context);
			sw.Stop();
			Logger logger14 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PeriAdvanceMonth.UpdateMisc: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger14.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x000EF03C File Offset: 0x000ED23C
		private void PeriAdvanceMonth_BornArea(DataContext context, DataMonitorManager monitor)
		{
			this.SetAndNotifyAdvancingMonthState(context, 2, monitor);
			WorldDomain.PeriAdvanceMonth_UpdateCharacterStatus_TaiwuGroup(context);
			WorldDomain.PeriAdvanceMonth_UpdateCharacterStatus(context, 135);
			context.ParallelModificationsRecorder.ApplyAll(context);
			DomainManager.Taiwu.UpdateTaiwuGroupFavorabilities(context);
			DomainManager.Taiwu.UpdateChildrenEducation(context);
			Thread.Sleep(50);
			this.SetAndNotifyAdvancingMonthState(context, 4, monitor);
			WorldDomain.PeriAdvanceMonth_CharacterSelfImprovement_TaiwuGroup(context);
			context.ParallelModificationsRecorder.ApplyAll(context);
			DomainManager.Taiwu.UpdateReadingProgressOnMonthChange(context);
			DomainManager.Extra.DeleteExpiredReadingStrategiesOnMonthChange(context);
			DomainManager.Taiwu.ClearActiveNeigongLoopingProgressOnMonthChange(context);
			DomainManager.Taiwu.ClearActiveReadingProgressOnMonthChange(context);
			DomainManager.Taiwu.ClearExpiredQiArtStrategies(context);
			DomainManager.Taiwu.TryAddLoopingEvent(context, (int)GlobalConfig.Instance.BaseLoopingEventProbability);
			WorldDomain.PeriAdvanceMonth_CharacterSelfImprovement_PracticeAndBreakout_TaiwuGroup(context);
			context.ParallelModificationsRecorder.ApplyAll(context);
			Thread.Sleep(50);
			this.SetAndNotifyAdvancingMonthState(context, 5, monitor);
			WorldDomain.PeriAdvanceMonth_CharacterActivePreparation_TaiwuGroup(context);
			context.ParallelModificationsRecorder.ApplyAll(context);
			Thread.Sleep(50);
			this.SetAndNotifyAdvancingMonthState(context, 6, monitor);
			WorldDomain.PeriAdvanceMonth_CharacterPassivePreparation_TaiwuGroup(context);
			WorldDomain.PeriAdvanceMonth_CharacterPassivePreparation(context, 135);
			context.ParallelModificationsRecorder.ApplyAll(context);
			Thread.Sleep(50);
			DomainManager.Item.UpdateCrickets(context);
			DomainManager.Taiwu.LoseOverloadResources(context);
			this.SetAndNotifyAdvancingMonthState(context, 7, monitor);
			WorldDomain.PeriAdvanceMonth_CharacterRelationsUpdate_TaiwuGroup(context);
			context.ParallelModificationsRecorder.ApplyAll(context);
			Thread.Sleep(50);
			this.SetAndNotifyAdvancingMonthState(context, 8, monitor);
			WorldDomain.PeriAdvanceMonth_CharacterPersonalNeedsProcessing_TaiwuGroup(context);
			context.ParallelModificationsRecorder.ApplyAll(context);
			Thread.Sleep(50);
			this.SetAndNotifyAdvancingMonthState(context, 10, monitor);
			WorldDomain.PeriAdvanceMonth_CharacterGeneralAction_TaiwuGroup(context);
			context.ParallelModificationsRecorder.ApplyAll(context);
			Thread.Sleep(50);
			this.SetAndNotifyAdvancingMonthState(context, 11, monitor);
			WorldDomain.PeriAdvanceMonth_CharacterFixedAction_TaiwuGroup(context);
			context.ParallelModificationsRecorder.ApplyAll(context);
			Thread.Sleep(50);
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x000EF22C File Offset: 0x000ED42C
		private unsafe static void PeriAdvanceMonth_CharacterMixedPoisonEffect(DataContext context, int areaId)
		{
			bool flag = areaId < 0;
			if (flag)
			{
				WorldDomain.PeriAdvanceMonth_CharacterMixedPoisonEffect_TaiwuGroup(context);
			}
			else
			{
				Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks((short)areaId);
				int i = 0;
				int blocksCount = blocks.Length;
				while (i < blocksCount)
				{
					HashSet<int> charIds = blocks[i]->CharacterSet;
					bool flag2 = charIds == null;
					if (!flag2)
					{
						foreach (int charId in charIds)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							character.PeriAdvanceMonth_MixedPoisonEffect(context);
							bool flag3 = character.IsActiveExternalRelationState(2);
							if (flag3)
							{
								WorldDomain.PeriAdvanceMonth_CharacterMixedPoisonEffect_KidnappedChars(context, charId);
							}
						}
					}
					i++;
				}
			}
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x000EF30C File Offset: 0x000ED50C
		private static void PeriAdvanceMonth_CharacterMixedPoisonEffect_TaiwuGroup(DataContext context)
		{
			HashSet<int> charIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int charId in charIds)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				character.PeriAdvanceMonth_MixedPoisonEffect(context);
				bool flag = character.IsActiveExternalRelationState(2);
				if (flag)
				{
					WorldDomain.PeriAdvanceMonth_CharacterMixedPoisonEffect_KidnappedChars(context, charId);
				}
			}
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x000EF398 File Offset: 0x000ED598
		private static void PeriAdvanceMonth_CharacterMixedPoisonEffect_KidnappedChars(DataContext context, int kidnapperCharId)
		{
			List<KidnappedCharacter> kidnappedChars = DomainManager.Character.GetKidnappedCharacters(kidnapperCharId).GetCollection();
			int i = 0;
			int count = kidnappedChars.Count;
			while (i < count)
			{
				int kidnappedCharId = kidnappedChars[i].CharId;
				GameData.Domains.Character.Character kidnappedChar = DomainManager.Character.GetElement_Objects(kidnappedCharId);
				kidnappedChar.PeriAdvanceMonth_MixedPoisonEffect(context);
				i++;
			}
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x000EF3F8 File Offset: 0x000ED5F8
		private unsafe static void PeriAdvanceMonth_UpdateCharacterStatus(DataContext context, int areaId)
		{
			if (areaId != -2)
			{
				if (areaId != -1)
				{
					Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks((short)areaId);
					int i = 0;
					int blocksCount = blocks.Length;
					while (i < blocksCount)
					{
						HashSet<int> charIds = blocks[i]->CharacterSet;
						bool flag = charIds != null;
						if (flag)
						{
							foreach (int charId in charIds)
							{
								GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
								character.PeriAdvanceMonth_UpdateStatus(context);
								bool flag2 = character.IsActiveExternalRelationState(2);
								if (flag2)
								{
									WorldDomain.PeriAdvanceMonth_UpdateCharacterStatus_KidnappedChars(context, charId);
								}
							}
						}
						charIds = blocks[i]->InfectedCharacterSet;
						bool flag3 = charIds == null;
						if (!flag3)
						{
							foreach (int charId2 in charIds)
							{
								GameData.Domains.Character.Character character2 = DomainManager.Character.GetElement_Objects(charId2);
								character2.PeriAdvanceMonth_UpdateStatus(context);
								bool flag4 = character2.IsActiveExternalRelationState(2);
								if (flag4)
								{
									WorldDomain.PeriAdvanceMonth_UpdateCharacterStatus_KidnappedChars(context, charId2);
								}
							}
						}
						i++;
					}
				}
				else
				{
					WorldDomain.PeriAdvanceMonth_UpdateCharacterStatus_TaiwuGroup(context);
				}
			}
			else
			{
				WorldDomain.PeriAdvanceMonth_UpdateCharacterStatus_HiddenCharacters(context);
			}
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x000EF574 File Offset: 0x000ED774
		private static void PeriAdvanceMonth_UpdateCharacterStatus_HiddenCharacters(DataContext context)
		{
			List<int> charIdList = new List<int>();
			DomainManager.Character.GetCrossAreaTravelingCharacterIds(charIdList);
			foreach (int charId in charIdList)
			{
				GameData.Domains.Character.Character character;
				bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (!flag)
				{
					bool flag2 = character.GetLeaderId() == charId;
					if (flag2)
					{
						WorldDomain.PeriAdvanceMonth_UpdateCharacterStatus_HiddenGroupChars(context, charId);
					}
					bool flag3 = character.IsActiveExternalRelationState(60);
					if (!flag3)
					{
						character.PeriAdvanceMonth_UpdateStatus(context);
						bool flag4 = character.IsActiveExternalRelationState(2);
						if (flag4)
						{
							WorldDomain.PeriAdvanceMonth_UpdateCharacterStatus_KidnappedChars(context, charId);
						}
					}
				}
			}
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			HashSet<int> charIdSet = new HashSet<int>();
			DomainManager.TaiwuEvent.CollectUnreleasedCalledCharacters(charIdSet);
			foreach (int charId2 in charIdSet)
			{
				GameData.Domains.Character.Character character2;
				bool flag5 = !DomainManager.Character.TryGetElement_Objects(charId2, out character2);
				if (!flag5)
				{
					bool flag6 = !character2.IsActiveExternalRelationState(60);
					if (!flag6)
					{
						bool flag7 = character2.GetKidnapperId() >= 0;
						if (!flag7)
						{
							bool flag8 = character2.GetLeaderId() == taiwuCharId;
							if (!flag8)
							{
								Location location = character2.GetLocation();
								bool flag9 = location.IsValid();
								if (flag9)
								{
									MapBlockData block = DomainManager.Map.GetBlock(location);
									bool flag10 = block.CharacterSet != null && block.CharacterSet.Contains(charId2);
									if (flag10)
									{
										continue;
									}
									bool flag11 = block.InfectedCharacterSet != null && block.InfectedCharacterSet.Contains(charId2);
									if (flag11)
									{
										continue;
									}
								}
								character2.PeriAdvanceMonth_UpdateStatus(context);
								bool flag12 = character2.IsActiveExternalRelationState(2);
								if (flag12)
								{
									WorldDomain.PeriAdvanceMonth_UpdateCharacterStatus_KidnappedChars(context, charId2);
								}
							}
						}
					}
				}
			}
			for (sbyte i = 0; i < 15; i += 1)
			{
				sbyte orgTemplateId = OrganizationDomain.GetLargeSectTemplateId(i);
				Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(orgTemplateId);
				for (int index = sect.Prison.Prisoners.Count - 1; index >= 0; index--)
				{
					SettlementPrisoner prisoner = sect.Prison.Prisoners[index];
					GameData.Domains.Character.Character character3;
					bool flag13 = !DomainManager.Character.TryGetElement_Objects(prisoner.CharId, out character3);
					if (!flag13)
					{
						character3.PeriAdvanceMonth_UpdateStatus(context);
					}
				}
			}
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x000EF840 File Offset: 0x000EDA40
		private static void PeriAdvanceMonth_UpdateCharacterStatus_TaiwuGroup(DataContext context)
		{
			HashSet<int> charIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int charId in charIds)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				character.PeriAdvanceMonth_UpdateStatus(context);
				bool flag = character.IsActiveExternalRelationState(2);
				if (flag)
				{
					WorldDomain.PeriAdvanceMonth_UpdateCharacterStatus_KidnappedChars(context, charId);
				}
			}
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x000EF8CC File Offset: 0x000EDACC
		private static void PeriAdvanceMonth_UpdateCharacterStatus_KidnappedChars(DataContext context, int kidnapperCharId)
		{
			List<KidnappedCharacter> kidnappedChars = DomainManager.Character.GetKidnappedCharacters(kidnapperCharId).GetCollection();
			int i = 0;
			int count = kidnappedChars.Count;
			while (i < count)
			{
				int kidnappedCharId = kidnappedChars[i].CharId;
				GameData.Domains.Character.Character kidnappedChar = DomainManager.Character.GetElement_Objects(kidnappedCharId);
				kidnappedChar.PeriAdvanceMonth_UpdateStatus(context);
				i++;
			}
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x000EF92C File Offset: 0x000EDB2C
		private static void PeriAdvanceMonth_UpdateCharacterStatus_HiddenGroupChars(DataContext context, int charId)
		{
			Tester.Assert(!DomainManager.Character.GetElement_Objects(charId).GetLocation().IsValid(), "");
			HashSet<int> groupCharSet = DomainManager.Character.GetGroup(charId).GetCollection();
			foreach (int groupCharId in groupCharSet)
			{
				bool flag = groupCharId == charId;
				if (!flag)
				{
					GameData.Domains.Character.Character groupChar = DomainManager.Character.GetElement_Objects(groupCharId);
					bool flag2 = groupChar.IsActiveExternalRelationState(60);
					if (!flag2)
					{
						groupChar.PeriAdvanceMonth_UpdateStatus(context);
						bool flag3 = groupChar.IsActiveExternalRelationState(2);
						if (flag3)
						{
							WorldDomain.PeriAdvanceMonth_UpdateCharacterStatus_KidnappedChars(context, groupCharId);
						}
					}
				}
			}
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x000EFA00 File Offset: 0x000EDC00
		private unsafe static void PeriAdvanceMonth_CharacterSelfImprovement(DataContext context, int areaId)
		{
			bool flag = areaId < 0;
			if (flag)
			{
				WorldDomain.PeriAdvanceMonth_CharacterSelfImprovement_TaiwuGroup(context);
			}
			else
			{
				Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks((short)areaId);
				int i = 0;
				int blocksCount = blocks.Length;
				while (i < blocksCount)
				{
					HashSet<int> charIds = blocks[i]->CharacterSet;
					bool flag2 = charIds == null;
					if (!flag2)
					{
						foreach (int charId in charIds)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							character.PeriAdvanceMonth_SelfImprovement(context);
						}
					}
					i++;
				}
			}
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x000EFAC4 File Offset: 0x000EDCC4
		private static void PeriAdvanceMonth_CharacterSelfImprovement_TaiwuGroup(DataContext context)
		{
			HashSet<int> charIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int charId in charIds)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				bool flag = charId != DomainManager.Taiwu.GetTaiwuCharId();
				if (flag)
				{
					character.PeriAdvanceMonth_SelfImprovement(context);
				}
				else
				{
					character.PeriAdvanceMonth_SelfImprovement_Taiwu(context);
				}
			}
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x000EFB5C File Offset: 0x000EDD5C
		private unsafe static void PeriAdvanceMonth_CharacterSelfImprovement_PracticeAndBreakout(DataContext context, int areaId)
		{
			bool flag = areaId < 0;
			if (flag)
			{
				WorldDomain.PeriAdvanceMonth_CharacterSelfImprovement_PracticeAndBreakout_TaiwuGroup(context);
			}
			else
			{
				Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks((short)areaId);
				int i = 0;
				int blocksCount = blocks.Length;
				while (i < blocksCount)
				{
					HashSet<int> charIds = blocks[i]->CharacterSet;
					bool flag2 = charIds == null;
					if (!flag2)
					{
						foreach (int charId in charIds)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							context.Equipping.ParallelPracticeAndBreakoutCombatSkills(context, character);
							context.Equipping.ParallelUpdateBreakPlateBonuses(context, character);
						}
					}
					i++;
				}
			}
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x000EFC38 File Offset: 0x000EDE38
		private static void PeriAdvanceMonth_CharacterSelfImprovement_PracticeAndBreakout_TaiwuGroup(DataContext context)
		{
			HashSet<int> charIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int charId in charIds)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				bool flag = charId != DomainManager.Taiwu.GetTaiwuCharId();
				if (flag)
				{
					context.Equipping.ParallelPracticeAndBreakoutCombatSkills(context, character);
					context.Equipping.ParallelUpdateBreakPlateBonuses(context, character);
				}
			}
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x000EFCDC File Offset: 0x000EDEDC
		private unsafe static void PeriAdvanceMonth_CharacterSelfImprovement_LearnNewSkills(DataContext context, int areaId)
		{
			bool flag = areaId < 0;
			if (flag)
			{
				WorldDomain.PeriAdvanceMonth_CharacterSelfImprovement_LearnNewSkills_TaiwuGroup(context);
			}
			else
			{
				Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks((short)areaId);
				int i = 0;
				int blocksCount = blocks.Length;
				while (i < blocksCount)
				{
					HashSet<int> charIds = blocks[i]->CharacterSet;
					bool flag2 = charIds == null;
					if (!flag2)
					{
						foreach (int charId in charIds)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							character.PeriAdvanceMonth_SelfImprovement_LearnNewSkills(context);
						}
					}
					i++;
				}
			}
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x000EFDA0 File Offset: 0x000EDFA0
		private static void PeriAdvanceMonth_CharacterSelfImprovement_LearnNewSkills_TaiwuGroup(DataContext context)
		{
			HashSet<int> charIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int charId in charIds)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				bool flag = charId != DomainManager.Taiwu.GetTaiwuCharId();
				if (flag)
				{
					character.PeriAdvanceMonth_SelfImprovement_LearnNewSkills(context);
				}
			}
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x000EFE2C File Offset: 0x000EE02C
		private unsafe static void PeriAdvanceMonth_CharacterActivePreparation_GetSupply(DataContext context, int areaId)
		{
			bool flag = areaId < 0;
			if (flag)
			{
				WorldDomain.PeriAdvanceMonth_CharacterActivePreparation_GetSupply_TaiwuGroup(context);
			}
			else
			{
				Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks((short)areaId);
				int i = 0;
				int blocksCount = blocks.Length;
				while (i < blocksCount)
				{
					HashSet<int> charIds = blocks[i]->CharacterSet;
					bool flag2 = charIds == null;
					if (!flag2)
					{
						foreach (int charId in charIds)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							character.PeriAdvanceMonth_ActivePreparation_GetSupply(context);
						}
					}
					i++;
				}
			}
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x000EFEF0 File Offset: 0x000EE0F0
		private static void PeriAdvanceMonth_CharacterActivePreparation_GetSupply_TaiwuGroup(DataContext context)
		{
			HashSet<int> charIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int charId in charIds)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				bool flag = charId != DomainManager.Taiwu.GetTaiwuCharId();
				if (flag)
				{
					character.PeriAdvanceMonth_ActivePreparation_GetSupply(context);
				}
			}
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x000EFF7C File Offset: 0x000EE17C
		private unsafe static void PeriAdvanceMonth_CharacterActivePreparation(DataContext context, int areaId)
		{
			bool flag = areaId < 0;
			if (flag)
			{
				WorldDomain.PeriAdvanceMonth_CharacterActivePreparation_TaiwuGroup(context);
			}
			else
			{
				Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks((short)areaId);
				int i = 0;
				int blocksCount = blocks.Length;
				while (i < blocksCount)
				{
					HashSet<int> charIds = blocks[i]->CharacterSet;
					bool flag2 = charIds == null;
					if (!flag2)
					{
						foreach (int charId in charIds)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							character.PeriAdvanceMonth_ActivePreparation(context);
						}
					}
					i++;
				}
			}
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x000F0040 File Offset: 0x000EE240
		private static void PeriAdvanceMonth_CharacterActivePreparation_TaiwuGroup(DataContext context)
		{
			HashSet<int> charIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int charId in charIds)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				bool flag = charId != DomainManager.Taiwu.GetTaiwuCharId();
				if (flag)
				{
					character.PeriAdvanceMonth_ActivePreparation(context);
				}
			}
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x000F00CC File Offset: 0x000EE2CC
		private unsafe static void PeriAdvanceMonth_CharacterPassivePreparation(DataContext context, int areaId)
		{
			bool flag = areaId < 0;
			if (flag)
			{
				WorldDomain.PeriAdvanceMonth_CharacterPassivePreparation_TaiwuGroup(context);
			}
			else
			{
				Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks((short)areaId);
				int i = 0;
				int blocksCount = blocks.Length;
				while (i < blocksCount)
				{
					HashSet<int> charIds = blocks[i]->CharacterSet;
					bool flag2 = charIds == null;
					if (!flag2)
					{
						foreach (int charId in charIds)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							character.PeriAdvanceMonth_PassivePreparation(context);
							bool flag3 = character.IsActiveExternalRelationState(2);
							if (flag3)
							{
								WorldDomain.PeriAdvanceMonth_CharacterPassivePreparation_KidnappedChars(context, charId);
							}
						}
					}
					i++;
				}
			}
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x000F01AC File Offset: 0x000EE3AC
		private static void PeriAdvanceMonth_CharacterPassivePreparation_TaiwuGroup(DataContext context)
		{
			HashSet<int> charIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int charId in charIds)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				character.PeriAdvanceMonth_PassivePreparation(context);
				bool flag = character.IsActiveExternalRelationState(2);
				if (flag)
				{
					WorldDomain.PeriAdvanceMonth_CharacterPassivePreparation_KidnappedChars(context, charId);
				}
			}
			Dictionary<int, GearMate>.KeyCollection gearMateIds = DomainManager.Extra.GetAllGearMateId();
			foreach (int charId2 in gearMateIds)
			{
				DomainManager.Character.GetElement_Objects(charId2).PeriAdvanceMonth_GearMateLoseOverLoadedItems(context);
			}
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x000F0294 File Offset: 0x000EE494
		private static void PeriAdvanceMonth_CharacterPassivePreparation_KidnappedChars(DataContext context, int kidnapperCharId)
		{
			List<KidnappedCharacter> kidnappedChars = DomainManager.Character.GetKidnappedCharacters(kidnapperCharId).GetCollection();
			int i = 0;
			int count = kidnappedChars.Count;
			while (i < count)
			{
				int kidnappedCharId = kidnappedChars[i].CharId;
				GameData.Domains.Character.Character kidnappedChar = DomainManager.Character.GetElement_Objects(kidnappedCharId);
				kidnappedChar.PeriAdvanceMonth_PassivePreparation(context);
				i++;
			}
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x000F02F4 File Offset: 0x000EE4F4
		private unsafe static void PeriAdvanceMonth_CharacterRelationsUpdate(DataContext context, int areaId)
		{
			bool flag = areaId < 0;
			if (flag)
			{
				WorldDomain.PeriAdvanceMonth_CharacterRelationsUpdate_TaiwuGroup(context);
			}
			else
			{
				Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
				HashSet<int> canInteractCharSet = ObjectPool<HashSet<int>>.Instance.Get();
				bool flag2 = (int)taiwuLocation.AreaId == areaId;
				if (flag2)
				{
					canInteractCharSet.Clear();
					canInteractCharSet.UnionWith(DomainManager.Taiwu.GetGroupCharIds().GetCollection());
				}
				Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks((short)areaId);
				int i = 0;
				int blocksCount = blocks.Length;
				while (i < blocksCount)
				{
					MapBlockData blockData = *blocks[i];
					HashSet<int> charIds = blockData.CharacterSet;
					bool flag3 = charIds == null;
					if (!flag3)
					{
						HashSet<int> charSet = charIds;
						bool isTaiwuBlock = taiwuLocation.AreaId == blockData.AreaId && taiwuLocation.BlockId == blockData.BlockId;
						bool flag4 = isTaiwuBlock;
						if (flag4)
						{
							charSet = canInteractCharSet;
							charSet.UnionWith(charIds);
						}
						foreach (int charId in charIds)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							character.PeriAdvanceMonth_RelationsUpdate(context, charSet);
							bool flag5 = character.IsActiveExternalRelationState(2);
							if (flag5)
							{
								WorldDomain.PeriAdvanceMonth_CharacterRelationsUpdate_KidnappedChars(context, charId);
							}
						}
					}
					i++;
				}
				ObjectPool<HashSet<int>>.Instance.Return(canInteractCharSet);
			}
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x000F047C File Offset: 0x000EE67C
		private static void PeriAdvanceMonth_CharacterRelationsUpdate_TaiwuGroup(DataContext context)
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			HashSet<int> charIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			HashSet<int> canInteractCharSet = ObjectPool<HashSet<int>>.Instance.Get();
			canInteractCharSet.Clear();
			canInteractCharSet.UnionWith(charIds);
			bool flag = taiwuLocation.IsValid();
			if (flag)
			{
				MapBlockData blockData = DomainManager.Map.GetBlock(taiwuLocation);
				bool flag2 = blockData.CharacterSet != null;
				if (flag2)
				{
					canInteractCharSet.UnionWith(blockData.CharacterSet);
				}
			}
			foreach (int charId in charIds)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				bool flag3 = charId != DomainManager.Taiwu.GetTaiwuCharId();
				if (flag3)
				{
					character.PeriAdvanceMonth_RelationsUpdate(context, canInteractCharSet);
				}
				bool flag4 = character.IsActiveExternalRelationState(2);
				if (flag4)
				{
					WorldDomain.PeriAdvanceMonth_CharacterRelationsUpdate_KidnappedChars(context, charId);
				}
			}
			ObjectPool<HashSet<int>>.Instance.Return(canInteractCharSet);
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x000F0594 File Offset: 0x000EE794
		private static void PeriAdvanceMonth_CharacterRelationsUpdate_KidnappedChars(DataContext context, int kidnapperCharId)
		{
			List<KidnappedCharacter> kidnappedChars = DomainManager.Character.GetKidnappedCharacters(kidnapperCharId).GetCollection();
			int i = 0;
			int count = kidnappedChars.Count;
			while (i < count)
			{
				int kidnappedCharId = kidnappedChars[i].CharId;
				GameData.Domains.Character.Character kidnappedChar = DomainManager.Character.GetElement_Objects(kidnappedCharId);
				i++;
			}
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x000F05E8 File Offset: 0x000EE7E8
		private unsafe static void PeriAdvanceMonth_CharacterPrioritizedAction(DataContext context, int areaId)
		{
			Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks((short)areaId);
			int i = 0;
			int blocksCount = blocks.Length;
			while (i < blocksCount)
			{
				HashSet<int> charIds = blocks[i]->CharacterSet;
				bool flag = charIds == null;
				if (!flag)
				{
					foreach (int charId in charIds)
					{
						GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
						character.PeriAdvanceMonth_ExecutePrioritizedAction(context);
					}
				}
				i++;
			}
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x000F0694 File Offset: 0x000EE894
		private unsafe static void PeriAdvanceMonth_CharacterPersonalNeedsProcessing(DataContext context, int areaId)
		{
			bool flag = areaId < 0;
			if (flag)
			{
				WorldDomain.PeriAdvanceMonth_CharacterPersonalNeedsProcessing_TaiwuGroup(context);
			}
			else
			{
				Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks((short)areaId);
				int i = 0;
				int blocksCount = blocks.Length;
				while (i < blocksCount)
				{
					HashSet<int> charIds = blocks[i]->CharacterSet;
					bool flag2 = charIds == null;
					if (!flag2)
					{
						foreach (int charId in charIds)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							character.PeriAdvanceMonth_PersonalNeedsProcessing(context);
							bool flag3 = character.IsActiveExternalRelationState(2);
							if (flag3)
							{
								WorldDomain.PeriAdvanceMonth_CharacterPersonalNeedsProcessing_KidnappedChars(context, charId);
							}
						}
					}
					i++;
				}
			}
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x000F0774 File Offset: 0x000EE974
		private static void PeriAdvanceMonth_CharacterPersonalNeedsProcessing_TaiwuGroup(DataContext context)
		{
			HashSet<int> charIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int charId in charIds)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				character.PeriAdvanceMonth_PersonalNeedsProcessing(context);
				bool flag = character.IsActiveExternalRelationState(2);
				if (flag)
				{
					WorldDomain.PeriAdvanceMonth_CharacterPersonalNeedsProcessing_KidnappedChars(context, charId);
				}
			}
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x000F0800 File Offset: 0x000EEA00
		private static void PeriAdvanceMonth_CharacterPersonalNeedsProcessing_KidnappedChars(DataContext context, int kidnapperCharId)
		{
			List<KidnappedCharacter> kidnappedChars = DomainManager.Character.GetKidnappedCharacters(kidnapperCharId).GetCollection();
			int i = 0;
			int count = kidnappedChars.Count;
			while (i < count)
			{
				int kidnappedCharId = kidnappedChars[i].CharId;
				GameData.Domains.Character.Character kidnappedChar = DomainManager.Character.GetElement_Objects(kidnappedCharId);
				kidnappedChar.PeriAdvanceMonth_PersonalNeedsProcessing(context);
				i++;
			}
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x000F0860 File Offset: 0x000EEA60
		private unsafe static void PeriAdvanceMonth_CharacterGeneralAction(DataContext context, int areaId)
		{
			bool flag = areaId < 0;
			if (flag)
			{
				WorldDomain.PeriAdvanceMonth_CharacterGeneralAction_TaiwuGroup(context);
			}
			else
			{
				Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
				HashSet<int> interactableBlockCharSet = context.AdvanceMonthRelatedData.BlockCharSet.Occupy();
				Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks((short)areaId);
				int i = 0;
				int blocksCount = blocks.Length;
				while (i < blocksCount)
				{
					MapBlockData blockData = *blocks[i];
					HashSet<int> charIds = blockData.CharacterSet;
					bool flag2 = charIds == null;
					if (!flag2)
					{
						interactableBlockCharSet.Clear();
						CharacterDomain.UnionWithInteractableCharacters(interactableBlockCharSet, charIds);
						bool flag3 = taiwuLocation.AreaId == blockData.AreaId && taiwuLocation.BlockId == blockData.BlockId;
						if (flag3)
						{
							CharacterDomain.UnionWithInteractableCharacters(interactableBlockCharSet, DomainManager.Taiwu.GetGroupCharIds().GetCollection());
						}
						foreach (int charId in charIds)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							character.PeriAdvanceMonth_ExecuteGeneralAction(context, interactableBlockCharSet, blockData.GraveSet);
						}
					}
					i++;
				}
				context.AdvanceMonthRelatedData.BlockCharSet.Release(ref interactableBlockCharSet);
			}
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x000F09C4 File Offset: 0x000EEBC4
		private static void PeriAdvanceMonth_CharacterGeneralAction_TaiwuGroup(DataContext context)
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			bool flag = !taiwuLocation.IsValid();
			if (!flag)
			{
				HashSet<int> charIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
				HashSet<int> interactableBlockCharSet = context.AdvanceMonthRelatedData.BlockCharSet.Occupy();
				interactableBlockCharSet.UnionWith(charIds);
				MapBlockData blockData = DomainManager.Map.GetBlock(taiwuLocation);
				bool flag2 = blockData.CharacterSet != null;
				if (flag2)
				{
					CharacterDomain.UnionWithInteractableCharacters(interactableBlockCharSet, blockData.CharacterSet);
				}
				HashSet<int> graveSet = blockData.GraveSet;
				foreach (int charId in charIds)
				{
					bool flag3 = charId == DomainManager.Taiwu.GetTaiwuCharId();
					if (!flag3)
					{
						GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
						character.PeriAdvanceMonth_ExecuteGeneralAction(context, interactableBlockCharSet, graveSet);
					}
				}
				context.AdvanceMonthRelatedData.BlockCharSet.Release(ref interactableBlockCharSet);
			}
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x000F0AD8 File Offset: 0x000EECD8
		private unsafe static void PeriAdvanceMonth_CharacterFixedAction(DataContext context, int areaId)
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			HashSet<int> interactableBlockCharSet = context.AdvanceMonthRelatedData.BlockCharSet.Occupy();
			Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks((short)areaId);
			int i = 0;
			int blocksCount = blocks.Length;
			while (i < blocksCount)
			{
				MapBlockData blockData = *blocks[i];
				HashSet<int> charIds = blockData.CharacterSet;
				interactableBlockCharSet.Clear();
				bool flag = charIds != null;
				if (flag)
				{
					CharacterDomain.UnionWithInteractableCharacters(interactableBlockCharSet, charIds);
				}
				bool flag2 = taiwuLocation.AreaId == blockData.AreaId && taiwuLocation.BlockId == blockData.BlockId;
				if (flag2)
				{
					HashSet<int> taiwuGroupCharIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
					CharacterDomain.UnionWithInteractableCharacters(interactableBlockCharSet, taiwuGroupCharIds);
					foreach (int charId in taiwuGroupCharIds)
					{
						GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
						bool flag3 = !character.IsTaiwu();
						if (flag3)
						{
							character.PeriAdvanceMonth_ExecuteFixedActions(context, interactableBlockCharSet);
						}
					}
				}
				bool flag4 = charIds != null;
				if (flag4)
				{
					foreach (int charId2 in charIds)
					{
						GameData.Domains.Character.Character character2 = DomainManager.Character.GetElement_Objects(charId2);
						character2.PeriAdvanceMonth_ExecuteFixedActions(context, interactableBlockCharSet);
					}
				}
				i++;
			}
			context.AdvanceMonthRelatedData.BlockCharSet.Release(ref interactableBlockCharSet);
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x000F0C8C File Offset: 0x000EEE8C
		private static void PeriAdvanceMonth_CharacterFixedAction_TaiwuGroup(DataContext context)
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			bool flag = !taiwuLocation.IsValid();
			if (!flag)
			{
				HashSet<int> charIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
				HashSet<int> interactableBlockCharSet = context.AdvanceMonthRelatedData.BlockCharSet.Occupy();
				interactableBlockCharSet.UnionWith(charIds);
				MapBlockData blockData = DomainManager.Map.GetBlock(taiwuLocation);
				bool flag2 = blockData.CharacterSet != null;
				if (flag2)
				{
					interactableBlockCharSet.UnionWith(blockData.CharacterSet);
				}
				foreach (int charId in charIds)
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
					bool flag3 = character.GetId() != DomainManager.Taiwu.GetTaiwuCharId();
					if (flag3)
					{
						character.PeriAdvanceMonth_ExecuteFixedActions(context, interactableBlockCharSet);
					}
				}
				context.AdvanceMonthRelatedData.BlockCharSet.Release(ref interactableBlockCharSet);
			}
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x000F0D9C File Offset: 0x000EEF9C
		private unsafe static void TestCreateChildren(DataContext context)
		{
			List<ValueTuple<GameData.Domains.Character.Character, GameData.Domains.Character.Character>> charsToChildbirth = new List<ValueTuple<GameData.Domains.Character.Character, GameData.Domains.Character.Character>>();
			float childBirthProb = 0.01f * DomainManager.World.GetProbAdjustOfCreatingCharacter();
			for (short areaId = 0; areaId < 135; areaId += 1)
			{
				Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(areaId);
				int i = 0;
				int blocksCount = blocks.Length;
				while (i < blocksCount)
				{
					HashSet<int> charIds = blocks[i]->CharacterSet;
					bool flag = charIds == null;
					if (!flag)
					{
						foreach (int charId in charIds)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							bool flag2 = character.GetGender() != 0 || character.GetAgeGroup() != 2;
							if (!flag2)
							{
								HashSet<int> spouseCharIds = DomainManager.Character.GetRelatedCharIds(charId, 1024);
								int aliveSpouseId = DomainManager.Character.GetAliveSpouse(charId);
								bool flag3 = spouseCharIds.Count > 0 && aliveSpouseId < 0;
								if (!flag3)
								{
									bool flag4 = context.Random.NextFloat() >= childBirthProb;
									if (!flag4)
									{
										GameData.Domains.Character.Character spouse = (aliveSpouseId >= 0) ? DomainManager.Character.GetElement_Objects(aliveSpouseId) : null;
										charsToChildbirth.Add(new ValueTuple<GameData.Domains.Character.Character, GameData.Domains.Character.Character>(character, spouse));
									}
								}
							}
						}
					}
					i++;
				}
			}
			foreach (ValueTuple<GameData.Domains.Character.Character, GameData.Domains.Character.Character> valueTuple in charsToChildbirth)
			{
				GameData.Domains.Character.Character mother = valueTuple.Item1;
				GameData.Domains.Character.Character father = valueTuple.Item2;
				DomainManager.Character.TestAddPregnantState(context, mother, father);
				DomainManager.Character.ParallelCreateNewbornChildren(context, mother, false, false);
			}
			context.ParallelModificationsRecorder.ApplyAll(context);
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x000F0F9C File Offset: 0x000EF19C
		private unsafe static void TestGenerateLifeRecords(DataContext context)
		{
			LifeRecordCollection lifeRecords = DomainManager.LifeRecord.GetLifeRecordCollection();
			DomainManager.LifeRecord.InitializeTestRelatedData();
			for (short areaId = 0; areaId < 135; areaId += 1)
			{
				Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(areaId);
				int i = 0;
				int blocksCount = blocks.Length;
				while (i < blocksCount)
				{
					HashSet<int> charIds = blocks[i]->CharacterSet;
					bool flag = charIds == null;
					if (!flag)
					{
						foreach (int charId in charIds)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							DomainManager.LifeRecord.AddRandomLifeRecord(context, lifeRecords, character, *blocks[i]);
						}
					}
					i++;
				}
			}
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x000F1090 File Offset: 0x000EF290
		private void TestGenerateMonthlyNotifications(DataContext context)
		{
			MonthlyNotificationCollection notifications = this.GetMonthlyNotificationCollection();
			this.PrepareTestMonthlyNotificationRelatedData();
			int notificationsCount = context.Random.Next(6, 25);
			for (int i = 0; i < notificationsCount; i++)
			{
				this.AddRandomMonthlyNotification(context, notifications);
			}
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x000F10D4 File Offset: 0x000EF2D4
		private void TestGenerateInstantNotifications(DataContext context)
		{
			InstantNotificationCollection notifications = this.GetInstantNotificationCollection();
			this.PrepareTestInstantNotificationRelatedData();
			int notificationsCount = context.Random.Next(15, 31);
			for (int i = 0; i < notificationsCount; i++)
			{
				this.AddRandomInstantNotification(context, notifications);
			}
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x000F111C File Offset: 0x000EF31C
		private void PostAdvanceMonth(DataContext context, DataMonitorManager monitor)
		{
			Stopwatch sw = Stopwatch.StartNew();
			Location location;
			bool disableAiActions = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox().Get<Location>("MainStoryLine_SpiritualWanderPlace_TaiwuVillagersCenter", out location);
			Events.RaisePostAdvanceMonthBegin(context);
			sw.Stop();
			Logger logger = WorldDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
			defaultInterpolatedStringHandler.AppendLiteral("DynamicEvents: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			this.SetAndNotifyAdvancingMonthState(context, 12, monitor);
			DomainManager.Information.ProcessAdvanceMonth(context);
			sw.Stop();
			Logger logger2 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Information: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			this.SetAndNotifyAdvancingMonthState(context, 13, monitor);
			WorkerThreadManager.Run(new Action<DataContext, int>(WorldDomain.PostAdvanceMonth_CharacterPersonalNeedsUpdate), -1, 139, monitor, 100);
			Thread.Sleep(20);
			sw.Stop();
			Logger logger3 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PostAdvanceMonth.CharacterPersonalNeedsUpdate: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger3.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			DomainManager.Character.UpdateLuckEvents(context);
			sw.Stop();
			Logger logger4 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PostAdvanceMonth.UpdateLuckEvents: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger4.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			DomainManager.Merchant.GenTradeCaravansOnAdvanceMonth(context);
			DomainManager.Merchant.CaravanMonthEvent(context);
			sw.Stop();
			Logger logger5 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PostAdvanceMonth.GenTradeCarvansOnAdvanceMonth: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger5.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			DomainManager.Adventure.UpdateAdventuresInAllAreas(context);
			DomainManager.Adventure.TryCreateElopeWithLove(context);
			WorkerThreadManager.Run(new Action<DataContext, int>(MapDomain.ParallelUpdateOnMonthChange), 0, 45, monitor, 100);
			WorkerThreadManager.Run(new Action<DataContext, int>(MapDomain.ParallelUpdateBrokenBlockOnMonthChange), 45, 135, monitor, 100);
			DomainManager.Map.UpdateCricketPlaceData(context);
			DomainManager.Extra.UpdateMapBlockRecoveryUnlockDates(context);
			sw.Stop();
			Logger logger6 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
			defaultInterpolatedStringHandler.AppendLiteral("UpdateMapAndAdventures: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger6.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			bool flag = this._mainStoryLineProgress >= 25;
			if (flag)
			{
				DomainManager.Character.GenerateSkeletons(context);
			}
			DomainManager.TaiwuEvent.XiangshuMinionSurroundTaiwuVillage();
			DomainManager.Character.UpdateInfectedCharacterMovements(context);
			DomainManager.Character.UpdateLegendaryBookInsaneCharacterMovements(context);
			DomainManager.Character.UpdateFixedCharacterMovements(context);
			DomainManager.Character.UpdateFixedCharacterEatingItems(context);
			bool flag2 = !disableAiActions;
			if (flag2)
			{
				DomainManager.Character.UpdateIntelligentCharacterMovements(context);
			}
			DomainManager.Extra.UpdateTaiwuTeammateTaming(context);
			WorkerThreadManager.Run(new Action<DataContext, int>(DomainManager.Extra.PostAdvanceMonth_UpdateNpcTaming), 0, 139, monitor, 100);
			Thread.Sleep(20);
			DomainManager.Taiwu.CheckAboutToDieVillagersAndTaiwuPeople(context);
			sw.Stop();
			Logger logger7 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
			defaultInterpolatedStringHandler.AppendLiteral("UpdateCharacterMovements: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger7.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			bool worldFunctionsStatus = DomainManager.World.GetWorldFunctionsStatus(10);
			if (worldFunctionsStatus)
			{
				DomainManager.Building.UpdateBrokenBuildings(context);
				DomainManager.Building.ParallelUpdate(context);
				context.ParallelModificationsRecorder.ApplyAll(context);
				DomainManager.Taiwu.MakeVillagerWorkSettlementsVisited(context);
				DomainManager.Taiwu.UpdateVillagerRoleNewClothing(context);
				DomainManager.Extra.UpdateBuildingAreaEffectProgresses(context);
				DomainManager.Building.FeastAdvanceMonth_Complement(context);
				short availableVillagerCount = DomainManager.Taiwu.GetAvailableVillagerCount();
				bool flag3 = availableVillagerCount > 0;
				if (flag3)
				{
					short settlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
					InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotificationCollection();
					instantNotifications.AddTaiwuVillageIdleCount(settlementId, (int)availableVillagerCount);
				}
			}
			sw.Stop();
			Logger logger8 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
			defaultInterpolatedStringHandler.AppendLiteral("UpdateBuildings: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger8.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			DomainManager.Organization.UpdateOrganizationMembers(context);
			sw.Stop();
			Logger logger9 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
			defaultInterpolatedStringHandler.AppendLiteral("UpdateOrganizationMembers: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger9.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			DomainManager.Extra.UpdateAnimalAreaData(context);
			DomainManager.SpecialEffect.ApplyBrokenEffectChangedDuringAdvance(context);
			DomainManager.Organization.Test_CheckFactions();
			DomainManager.Extra.UpdateWorldCharacterTitles(context);
			DomainManager.Extra.UpdateCharacterTemporaryFeatures(context);
			DomainManager.Extra.UpdateAiActionCooldowns(context);
			DomainManager.Extra.UpdateTaiwuTaming(context);
			DomainManager.Extra.GearMateUpdateStatus(context);
			DomainManager.LegendaryBook.CreateLegendaryBooksAccordingToXiangshuProgress(context);
			DomainManager.TaiwuEvent.HandleMonthlyActions();
			ProfessionSkillHandle.OnPostAdvanceMonth(context);
			this.AdvanceMonth_SectMainStory(context);
			DomainManager.Taiwu.AddJieqingPunishmentMonthlyEvent(context);
			DomainManager.Taiwu.UpdateFollowingNotifications(context);
			DomainManager.Extra.TriggerTaiwuVillageVowMonthlyEvent(context);
			DomainManager.Extra.RemoveOverdueCityPunishmentSeverityCustomizeData(context);
			DomainManager.Extra.UpdateSpecialCustomizedSeverity(context);
			DomainManager.Extra.UpdateNpcArtisanOrderProgress(context);
			DomainManager.Extra.UpdateBuildingArtisanOrderProgress(context);
			DomainManager.Extra.UpdateResourceBlockBuildingCoreProducing(context);
			DomainManager.Extra.MapPickupsPostAdvanceMonth(context);
			DlcManager.OnPostAdvanceMonth(context);
			sw.Stop();
			Logger logger10 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PostAdvanceMonth.AdjustMisc: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger10.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			DomainManager.Character.PostAdvanceMonthCalcDarkAsh(context);
			sw.Stop();
			Logger logger11 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PostAdvanceMonth.CalcDarkAsh: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger11.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			sw.Restart();
			WorldDomain.PostAdvanceMonth_ClearRedundantData(context);
			WorldDomain.PostAdvanceMonth_SetEventGlobalArgs(context);
			sw.Stop();
			Logger logger12 = WorldDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PostAdvanceMonth.Clean: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger12.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x000F1870 File Offset: 0x000EFA70
		private void PostAdvanceMonth_BornArea(DataContext context, DataMonitorManager monitor)
		{
			Events.RaisePostAdvanceMonthBegin(context);
			this.SetAndNotifyAdvancingMonthState(context, 13, monitor);
			WorldDomain.PostAdvanceMonth_CharacterPersonalNeedsUpdate_TaiwuGroup(context);
			WorldDomain.PostAdvanceMonth_CharacterPersonalNeedsUpdate(context, 135);
			context.ParallelModificationsRecorder.ApplyAll(context);
			MapDomain.ParallelUpdateOnMonthChange(context, 135);
			context.ParallelModificationsRecorder.ApplyAll(context);
			Thread.Sleep(50);
			DomainManager.Adventure.UpdateAdventuresInAllAreas(context);
			DomainManager.Information.ProcessAdvanceMonth(context);
			DomainManager.SpecialEffect.ApplyBrokenEffectChangedDuringAdvance(context);
			DomainManager.Extra.UpdateTaiwuTaming(context);
			WorldDomain.PostAdvanceMonth_ClearRedundantData(context);
			WorldDomain.PostAdvanceMonth_SetEventGlobalArgs(context);
			bool flag = DomainManager.TutorialChapter.InGuiding && DomainManager.TutorialChapter.GetTutorialChapter() == 2;
			if (flag)
			{
				DomainManager.Building.TutorialUpdate(context);
			}
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x000F193D File Offset: 0x000EFB3D
		private static void PostAdvanceMonth_SetEventGlobalArgs(DataContext context)
		{
			EventHelper.SaveArgToSectMainStory<bool>(6, "EnteredShixiangDrumEasterEggThisMonth", false);
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x000F1950 File Offset: 0x000EFB50
		private static void PostAdvanceMonth_ClearRedundantData(DataContext context)
		{
			DomainManager.Taiwu.ClearAdvanceMonthData();
			sbyte currMonthInYear = DomainManager.World.GetCurrMonthInYear();
			bool flag = currMonthInYear == 0;
			if (flag)
			{
				DomainManager.Character.TryRemoveRecentDeadCharacters(context);
				DomainManager.Character.TryRemoveDeadCharacters(context);
				short currYear = DomainManager.World.GetCurrYear();
				bool flag2 = currYear % 10 == 0;
				if (flag2)
				{
					DomainManager.Character.RemoveObsoleteActualBloodParents(context);
				}
			}
			DomainManager.Merchant.RemoveObsoleteMerchantData(context);
			DomainManager.Merchant.SetVillagerRoleMerchantType(context);
			WorkerThreadManager.RunPostAction(new Action<DataContext>(WorldDomain.RemoveWorldItemsToBeRemoved));
			WorldDomain.RemoveWorldItemsToBeRemoved(context);
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x000F19EC File Offset: 0x000EFBEC
		private static void RemoveWorldItemsToBeRemoved(DataContext context)
		{
			List<ItemKey> items = context.AdvanceMonthRelatedData.WorldItemsToBeRemoved;
			DomainManager.Item.RemoveItems(context, items);
			items.Clear();
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x000F1A1C File Offset: 0x000EFC1C
		private unsafe static void PostAdvanceMonth_CharacterPersonalNeedsUpdate(DataContext context, int areaId)
		{
			bool flag = areaId < 0;
			if (flag)
			{
				WorldDomain.PostAdvanceMonth_CharacterPersonalNeedsUpdate_TaiwuGroup(context);
			}
			else
			{
				Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks((short)areaId);
				int i = 0;
				int blocksCount = blocks.Length;
				while (i < blocksCount)
				{
					HashSet<int> charIds = blocks[i]->CharacterSet;
					bool flag2 = charIds == null;
					if (!flag2)
					{
						foreach (int charId in charIds)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							bool flag3 = character.GetPersonalNeeds().Count > 0;
							if (flag3)
							{
								character.OfflineUpdatePersonalNeedsDuration();
								ParallelModificationsRecorder recorder = context.ParallelModificationsRecorder;
								recorder.RecordType(ParallelModificationType.PostAdvanceMonthPersonalNeedsUpdate);
								recorder.RecordParameterClass<GameData.Domains.Character.Character>(character);
							}
							bool flag4 = character.IsActiveExternalRelationState(2);
							if (flag4)
							{
								WorldDomain.PostAdvanceMonth_CharacterPersonalNeedsUpdate_KidnappedChars(context, charId);
							}
						}
					}
					i++;
				}
			}
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x000F1B30 File Offset: 0x000EFD30
		private static void PostAdvanceMonth_CharacterPersonalNeedsUpdate_TaiwuGroup(DataContext context)
		{
			HashSet<int> charIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int charId in charIds)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				bool flag = character.GetPersonalNeeds().Count > 0;
				if (flag)
				{
					character.OfflineUpdatePersonalNeedsDuration();
					ParallelModificationsRecorder recorder = context.ParallelModificationsRecorder;
					recorder.RecordType(ParallelModificationType.PostAdvanceMonthPersonalNeedsUpdate);
					recorder.RecordParameterClass<GameData.Domains.Character.Character>(character);
				}
				bool flag2 = character.IsActiveExternalRelationState(2);
				if (flag2)
				{
					WorldDomain.PostAdvanceMonth_CharacterPersonalNeedsUpdate_KidnappedChars(context, charId);
				}
			}
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x000F1BF0 File Offset: 0x000EFDF0
		private static void PostAdvanceMonth_CharacterPersonalNeedsUpdate_KidnappedChars(DataContext context, int kidnapperCharId)
		{
			List<KidnappedCharacter> kidnappedChars = DomainManager.Character.GetKidnappedCharacters(kidnapperCharId).GetCollection();
			int i = 0;
			int count = kidnappedChars.Count;
			while (i < count)
			{
				int kidnappedCharId = kidnappedChars[i].CharId;
				GameData.Domains.Character.Character kidnappedChar = DomainManager.Character.GetElement_Objects(kidnappedCharId);
				kidnappedChar.OfflineUpdatePersonalNeedsDuration();
				ParallelModificationsRecorder recorder = context.ParallelModificationsRecorder;
				recorder.RecordType(ParallelModificationType.PostAdvanceMonthPersonalNeedsUpdate);
				recorder.RecordParameterClass<GameData.Domains.Character.Character>(kidnappedChar);
				i++;
			}
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x000F1C68 File Offset: 0x000EFE68
		private void PreAdvanceMonth(DataContext context, DataMonitorManager monitor)
		{
			Stopwatch sw = Stopwatch.StartNew();
			DomainManager.Map.TaiwuMoveRecord.Clear();
			DomainManager.Taiwu.UpdateVillagerRoleRecords(context);
			DomainManager.Taiwu.UpdateVillagerRoleFixedActionSuccessArray(context, true);
			DomainManager.Taiwu.JieqingPunishmentAssassinAlreadyAdd = false;
			DomainManager.Taiwu.JieqingHuntTaiwu = (DomainManager.Taiwu.TryGetHuntTaiwuPrioritizedAction().Item2 != null);
			this.SetAndNotifyAdvancingMonthState(context, 1, monitor);
			this.SetCurrDate(this._currDate + 1, context);
			DomainManager.Extra.UpdateActionPoint(context);
			sbyte currMonthInYear = this.GetCurrMonthInYear();
			this.AddSolarTermNotification((int)currMonthInYear);
			DomainManager.Taiwu.LoseOverloadWarehouseItems(context);
			DomainManager.Organization.UpdateSectPrisonersOnAdvanceMonth(context);
			DomainManager.Character.DecayGraves(context);
			bool worldFunctionsStatus = DomainManager.World.GetWorldFunctionsStatus(10);
			if (worldFunctionsStatus)
			{
				DomainManager.Taiwu.CalcVillagerWorkOnMap(context);
			}
			DomainManager.Organization.MakeNoneOrgCharactersBecomeBeggar(context);
			DomainManager.Extra.UpdateMaxApprovingRateTempBonus(context);
			DomainManager.Organization.UpdateApprovingRateEffectOnAdvanceMonth(context);
			bool worldFunctionsStatus2 = DomainManager.World.GetWorldFunctionsStatus(10);
			if (worldFunctionsStatus2)
			{
				DomainManager.Building.UpdateTaiwuBuildingAutoOperation(context);
				DomainManager.Building.SerialUpdate(context);
				DomainManager.Building.UpdateResourceBlockEffectsOnAdvanceMonth(context);
				DomainManager.Extra.FeastAdvanceMonth(context);
			}
			DomainManager.Building.UpdateMakingProgressOnMonthChange(context);
			ProfessionSkillHandle.OnPreAdvanceMonth(context);
			sw.Stop();
			Logger logger = WorldDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PreAdvanceMonth: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x000F1E08 File Offset: 0x000F0008
		private void PreAdvanceMonth_BornArea(DataContext context, DataMonitorManager monitor)
		{
			this.SetAndNotifyAdvancingMonthState(context, 1, monitor);
			DomainManager.Map.TaiwuMoveRecord.Clear();
			this.SetCurrDate(this._currDate + 1, context);
			DomainManager.Extra.UpdateActionPoint(context);
			sbyte currMonthInYear = this.GetCurrMonthInYear();
			this.AddSolarTermNotification((int)currMonthInYear);
			DomainManager.Taiwu.LoseOverloadWarehouseItems(context);
			DomainManager.Building.UpdateMakingProgressOnMonthChange(context);
			Thread.Sleep(50);
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x000F1E7C File Offset: 0x000F007C
		public WorldDomain() : base(35)
		{
			this._worldId = 0U;
			this._xiangshuProgress = 0;
			this._xiangshuAvatarTaskStatuses = new XiangshuAvatarTaskStatus[9];
			this._xiangshuAvatarTasksInOrder = new sbyte[9];
			this._stateTaskStatuses = new sbyte[15];
			this._mainStoryLineProgress = 0;
			this._beatRanChenZi = false;
			this._worldFunctionsStatuses = 0UL;
			this._customTexts = new Dictionary<int, string>(0);
			this._nextCustomTextId = 0;
			this._instantNotifications = new InstantNotificationCollection(1024);
			this._onHandingMonthlyEventBlock = false;
			this._lastMonthlyNotifications = new MonthlyNotificationCollection(0);
			this._worldPopulationType = 0;
			this._characterLifespanType = 0;
			this._combatDifficulty = 0;
			this._hereticsAmountType = 0;
			this._bossInvasionSpeedType = 0;
			this._worldResourceAmountType = 0;
			this._allowRandomTaiwuHeir = false;
			this._restrictOptionsBehaviorType = false;
			this._taiwuVillageStateTemplateId = 0;
			this._taiwuVillageLandFormType = 0;
			this._hideTaiwuOriginalSurname = false;
			this._allowExecute = false;
			this._archiveFilesBackupInterval = 0;
			this._worldStandardPopulation = 0;
			this._currDate = 0;
			this._daysInCurrMonth = 0;
			this._advancingMonthState = 0;
			this._currTaskList = new List<TaskData>();
			this._sortedTaskList = new List<TaskDisplayData>();
			this._worldStateData = default(WorldStateData);
			this._archiveFilesBackupCount = 0;
			this._sortedMonthlyNotificationSortingGroups = new List<int>();
			this.OnInitializedDomainData();
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x000F252C File Offset: 0x000F072C
		public uint GetWorldId()
		{
			return this._worldId;
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x000F2544 File Offset: 0x000F0744
		private unsafe void SetWorldId(uint value, DataContext context)
		{
			this._worldId = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 0, 4);
			*(int*)pData = (int)this._worldId;
			pData += 4;
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x000F2584 File Offset: 0x000F0784
		public sbyte GetXiangshuProgress()
		{
			Thread.MemoryBarrier();
			bool flag = BaseGameDataDomain.IsCached(this.DataStates, 1);
			sbyte xiangshuProgress;
			if (flag)
			{
				xiangshuProgress = this._xiangshuProgress;
			}
			else
			{
				sbyte value = this.CalcXiangshuProgress();
				bool lockTaken = false;
				try
				{
					this._spinLockXiangshuProgress.Enter(ref lockTaken);
					this._xiangshuProgress = value;
					BaseGameDataDomain.SetCached(this.DataStates, 1);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLockXiangshuProgress.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				xiangshuProgress = this._xiangshuProgress;
			}
			return xiangshuProgress;
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x000F2618 File Offset: 0x000F0818
		public XiangshuAvatarTaskStatus GetElement_XiangshuAvatarTaskStatuses(int index)
		{
			return this._xiangshuAvatarTaskStatuses[index];
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x000F2638 File Offset: 0x000F0838
		public unsafe void SetElement_XiangshuAvatarTaskStatuses(int index, XiangshuAvatarTaskStatus value, DataContext context)
		{
			this._xiangshuAvatarTaskStatuses[index] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, this._dataStatesXiangshuAvatarTaskStatuses, WorldDomain.CacheInfluencesXiangshuAvatarTaskStatuses, context);
			byte* pData = OperationAdder.FixedElementList_Set(1, 2, index, 8);
			pData += value.Serialize(pData);
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x000F267C File Offset: 0x000F087C
		public sbyte[] GetXiangshuAvatarTasksInOrder()
		{
			return this._xiangshuAvatarTasksInOrder;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x000F2694 File Offset: 0x000F0894
		public unsafe void SetXiangshuAvatarTasksInOrder(sbyte[] value, DataContext context)
		{
			this._xiangshuAvatarTasksInOrder = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 3, 9);
			for (int i = 0; i < 9; i++)
			{
				pData[i] = (byte)this._xiangshuAvatarTasksInOrder[i];
			}
			pData += 9;
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x000F26E8 File Offset: 0x000F08E8
		public sbyte GetElement_StateTaskStatuses(int index)
		{
			return this._stateTaskStatuses[index];
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x000F2704 File Offset: 0x000F0904
		public unsafe void SetElement_StateTaskStatuses(int index, sbyte value, DataContext context)
		{
			this._stateTaskStatuses[index] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, this._dataStatesStateTaskStatuses, WorldDomain.CacheInfluencesStateTaskStatuses, context);
			byte* pData = OperationAdder.FixedElementList_Set(1, 4, index, 1);
			*pData = (byte)value;
			pData++;
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x000F2740 File Offset: 0x000F0940
		public short GetMainStoryLineProgress()
		{
			return this._mainStoryLineProgress;
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x000F2758 File Offset: 0x000F0958
		public unsafe void SetMainStoryLineProgress(short value, DataContext context)
		{
			this._mainStoryLineProgress = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 5, 2);
			*(short*)pData = this._mainStoryLineProgress;
			pData += 2;
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x000F2798 File Offset: 0x000F0998
		public bool GetBeatRanChenZi()
		{
			return this._beatRanChenZi;
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x000F27B0 File Offset: 0x000F09B0
		public unsafe void SetBeatRanChenZi(bool value, DataContext context)
		{
			this._beatRanChenZi = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 6, 1);
			*pData = (this._beatRanChenZi ? 1 : 0);
			pData++;
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x000F27F0 File Offset: 0x000F09F0
		public ulong GetWorldFunctionsStatuses()
		{
			return this._worldFunctionsStatuses;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x000F2808 File Offset: 0x000F0A08
		public unsafe void SetWorldFunctionsStatuses(ulong value, DataContext context)
		{
			this._worldFunctionsStatuses = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 7, 8);
			*(long*)pData = (long)this._worldFunctionsStatuses;
			pData += 8;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x000F2848 File Offset: 0x000F0A48
		public string GetElement_CustomTexts(int elementId)
		{
			return this._customTexts[elementId];
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x000F2868 File Offset: 0x000F0A68
		public bool TryGetElement_CustomTexts(int elementId, out string value)
		{
			return this._customTexts.TryGetValue(elementId, out value);
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x000F2888 File Offset: 0x000F0A88
		private unsafe void AddElement_CustomTexts(int elementId, string value, DataContext context)
		{
			this._customTexts.Add(elementId, value);
			this._modificationsCustomTexts.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, WorldDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int elementsCount = value.Length;
				int contentSize = 2 * elementsCount;
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<int>(1, 8, elementId, contentSize);
				char* ptr;
				if (value == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = value.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<int>(1, 8, elementId, 0);
			}
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x000F2930 File Offset: 0x000F0B30
		private unsafe void SetElement_CustomTexts(int elementId, string value, DataContext context)
		{
			this._customTexts[elementId] = value;
			this._modificationsCustomTexts.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, WorldDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int elementsCount = value.Length;
				int contentSize = 2 * elementsCount;
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<int>(1, 8, elementId, contentSize);
				char* ptr;
				if (value == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = value.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<int>(1, 8, elementId, 0);
			}
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x000F29D6 File Offset: 0x000F0BD6
		private void RemoveElement_CustomTexts(int elementId, DataContext context)
		{
			this._customTexts.Remove(elementId);
			this._modificationsCustomTexts.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, WorldDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<int>(1, 8, elementId);
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x000F2A0F File Offset: 0x000F0C0F
		private void ClearCustomTexts(DataContext context)
		{
			this._customTexts.Clear();
			this._modificationsCustomTexts.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, WorldDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(1, 8);
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x000F2A48 File Offset: 0x000F0C48
		private int GetNextCustomTextId()
		{
			return this._nextCustomTextId;
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x000F2A60 File Offset: 0x000F0C60
		private unsafe void SetNextCustomTextId(int value, DataContext context)
		{
			this._nextCustomTextId = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 9, 4);
			*(int*)pData = this._nextCustomTextId;
			pData += 4;
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x000F2AA0 File Offset: 0x000F0CA0
		public InstantNotificationCollection GetInstantNotifications()
		{
			return this._instantNotifications;
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x000F2AB8 File Offset: 0x000F0CB8
		private unsafe void CommitInsert_InstantNotifications(DataContext context, int offset, int size)
		{
			this._modificationsInstantNotifications.RecordInserting(offset, size);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.Binary_Insert(1, 10, offset, size);
			this._instantNotifications.CopyTo(offset, size, pData);
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x000F2B04 File Offset: 0x000F0D04
		private unsafe void CommitWrite_InstantNotifications(DataContext context, int offset, int size)
		{
			this._modificationsInstantNotifications.RecordWriting(offset, size);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.Binary_Write(1, 10, offset, size);
			this._instantNotifications.CopyTo(offset, size, pData);
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x000F2B4E File Offset: 0x000F0D4E
		private void CommitRemove_InstantNotifications(DataContext context, int offset, int size)
		{
			this._modificationsInstantNotifications.RecordRemoving(offset, size);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, this.DataStates, WorldDomain.CacheInfluences, context);
			OperationAdder.Binary_Remove(1, 10, offset, size);
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x000F2B80 File Offset: 0x000F0D80
		private unsafe void CommitSetMetadata_InstantNotifications(DataContext context)
		{
			this._modificationsInstantNotifications.RecordSettingMetadata();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, this.DataStates, WorldDomain.CacheInfluences, context);
			ushort metadataSize = this._instantNotifications.GetSerializedFixedSizeOfMetadata();
			byte* pData = OperationAdder.Binary_SetMetadata(1, 10, metadataSize);
			this._instantNotifications.SerializeMetadata(pData);
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x000F2BD4 File Offset: 0x000F0DD4
		public bool GetOnHandingMonthlyEventBlock()
		{
			return this._onHandingMonthlyEventBlock;
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x000F2BEC File Offset: 0x000F0DEC
		public void SetOnHandingMonthlyEventBlock(bool value, DataContext context)
		{
			this._onHandingMonthlyEventBlock = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, this.DataStates, WorldDomain.CacheInfluences, context);
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x000F2C0C File Offset: 0x000F0E0C
		[Obsolete("DomainData _lastMonthlyNotifications is no longer in use.")]
		public MonthlyNotificationCollection GetLastMonthlyNotifications()
		{
			return this._lastMonthlyNotifications;
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x000F2C24 File Offset: 0x000F0E24
		[Obsolete("DomainData _lastMonthlyNotifications is no longer in use.")]
		private unsafe void SetLastMonthlyNotifications(MonthlyNotificationCollection value, DataContext context)
		{
			this._lastMonthlyNotifications = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, this.DataStates, WorldDomain.CacheInfluences, context);
			int dataSize = this._lastMonthlyNotifications.GetSerializedSize();
			byte* pData = OperationAdder.DynamicSingleValue_Set(1, 12, dataSize);
			pData += this._lastMonthlyNotifications.Serialize(pData);
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x000F2C74 File Offset: 0x000F0E74
		public byte GetWorldPopulationType()
		{
			return this._worldPopulationType;
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x000F2C8C File Offset: 0x000F0E8C
		private unsafe void SetWorldPopulationType(byte value, DataContext context)
		{
			this._worldPopulationType = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 13, 1);
			*pData = this._worldPopulationType;
			pData++;
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x000F2CCC File Offset: 0x000F0ECC
		public byte GetCharacterLifespanType()
		{
			return this._characterLifespanType;
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x000F2CE4 File Offset: 0x000F0EE4
		public unsafe void SetCharacterLifespanType(byte value, DataContext context)
		{
			this._characterLifespanType = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 14, 1);
			*pData = this._characterLifespanType;
			pData++;
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x000F2D24 File Offset: 0x000F0F24
		public byte GetCombatDifficulty()
		{
			return this._combatDifficulty;
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x000F2D3C File Offset: 0x000F0F3C
		public unsafe void SetCombatDifficulty(byte value, DataContext context)
		{
			this._combatDifficulty = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 15, 1);
			*pData = this._combatDifficulty;
			pData++;
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x000F2D7C File Offset: 0x000F0F7C
		public byte GetHereticsAmountType()
		{
			return this._hereticsAmountType;
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x000F2D94 File Offset: 0x000F0F94
		public unsafe void SetHereticsAmountType(byte value, DataContext context)
		{
			this._hereticsAmountType = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 16, 1);
			*pData = this._hereticsAmountType;
			pData++;
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x000F2DD4 File Offset: 0x000F0FD4
		public byte GetBossInvasionSpeedType()
		{
			return this._bossInvasionSpeedType;
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x000F2DEC File Offset: 0x000F0FEC
		public unsafe void SetBossInvasionSpeedType(byte value, DataContext context)
		{
			this._bossInvasionSpeedType = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 17, 1);
			*pData = this._bossInvasionSpeedType;
			pData++;
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x000F2E2C File Offset: 0x000F102C
		public byte GetWorldResourceAmountType()
		{
			return this._worldResourceAmountType;
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x000F2E44 File Offset: 0x000F1044
		public unsafe void SetWorldResourceAmountType(byte value, DataContext context)
		{
			this._worldResourceAmountType = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 18, 1);
			*pData = this._worldResourceAmountType;
			pData++;
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x000F2E84 File Offset: 0x000F1084
		public bool GetAllowRandomTaiwuHeir()
		{
			return this._allowRandomTaiwuHeir;
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x000F2E9C File Offset: 0x000F109C
		public unsafe void SetAllowRandomTaiwuHeir(bool value, DataContext context)
		{
			this._allowRandomTaiwuHeir = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 19, 1);
			*pData = (this._allowRandomTaiwuHeir ? 1 : 0);
			pData++;
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x000F2EDC File Offset: 0x000F10DC
		public bool GetRestrictOptionsBehaviorType()
		{
			return this._restrictOptionsBehaviorType;
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x000F2EF4 File Offset: 0x000F10F4
		public unsafe void SetRestrictOptionsBehaviorType(bool value, DataContext context)
		{
			this._restrictOptionsBehaviorType = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 20, 1);
			*pData = (this._restrictOptionsBehaviorType ? 1 : 0);
			pData++;
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x000F2F34 File Offset: 0x000F1134
		public sbyte GetTaiwuVillageStateTemplateId()
		{
			return this._taiwuVillageStateTemplateId;
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x000F2F4C File Offset: 0x000F114C
		public unsafe void SetTaiwuVillageStateTemplateId(sbyte value, DataContext context)
		{
			this._taiwuVillageStateTemplateId = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 21, 1);
			*pData = (byte)this._taiwuVillageStateTemplateId;
			pData++;
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x000F2F8C File Offset: 0x000F118C
		public sbyte GetTaiwuVillageLandFormType()
		{
			return this._taiwuVillageLandFormType;
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x000F2FA4 File Offset: 0x000F11A4
		public unsafe void SetTaiwuVillageLandFormType(sbyte value, DataContext context)
		{
			this._taiwuVillageLandFormType = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 22, 1);
			*pData = (byte)this._taiwuVillageLandFormType;
			pData++;
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x000F2FE4 File Offset: 0x000F11E4
		public bool GetHideTaiwuOriginalSurname()
		{
			return this._hideTaiwuOriginalSurname;
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x000F2FFC File Offset: 0x000F11FC
		public void SetHideTaiwuOriginalSurname(bool value, DataContext context)
		{
			this._hideTaiwuOriginalSurname = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, this.DataStates, WorldDomain.CacheInfluences, context);
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x000F301C File Offset: 0x000F121C
		public bool GetAllowExecute()
		{
			return this._allowExecute;
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x000F3034 File Offset: 0x000F1234
		public void SetAllowExecute(bool value, DataContext context)
		{
			this._allowExecute = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, this.DataStates, WorldDomain.CacheInfluences, context);
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x000F3054 File Offset: 0x000F1254
		public sbyte GetArchiveFilesBackupInterval()
		{
			return this._archiveFilesBackupInterval;
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x000F306C File Offset: 0x000F126C
		public void SetArchiveFilesBackupInterval(sbyte value, DataContext context)
		{
			this._archiveFilesBackupInterval = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(25, this.DataStates, WorldDomain.CacheInfluences, context);
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x000F308C File Offset: 0x000F128C
		public int GetWorldStandardPopulation()
		{
			return this._worldStandardPopulation;
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x000F30A4 File Offset: 0x000F12A4
		private unsafe void SetWorldStandardPopulation(int value, DataContext context)
		{
			this._worldStandardPopulation = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(26, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 26, 4);
			*(int*)pData = this._worldStandardPopulation;
			pData += 4;
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x000F30E4 File Offset: 0x000F12E4
		public int GetCurrDate()
		{
			return this._currDate;
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x000F30FC File Offset: 0x000F12FC
		private unsafe void SetCurrDate(int value, DataContext context)
		{
			this._currDate = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(27, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 27, 4);
			*(int*)pData = this._currDate;
			pData += 4;
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x000F313C File Offset: 0x000F133C
		[Obsolete("DomainData _daysInCurrMonth is no longer in use.")]
		public sbyte GetDaysInCurrMonth()
		{
			return this._daysInCurrMonth;
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x000F3154 File Offset: 0x000F1354
		[Obsolete("DomainData _daysInCurrMonth is no longer in use.")]
		private unsafe void SetDaysInCurrMonth(sbyte value, DataContext context)
		{
			this._daysInCurrMonth = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, this.DataStates, WorldDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 28, 1);
			*pData = (byte)this._daysInCurrMonth;
			pData++;
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x000F3194 File Offset: 0x000F1394
		public sbyte GetAdvancingMonthState()
		{
			return this._advancingMonthState;
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x000F31AC File Offset: 0x000F13AC
		private void SetAdvancingMonthState(sbyte value, DataContext context)
		{
			this._advancingMonthState = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(29, this.DataStates, WorldDomain.CacheInfluences, context);
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x000F31CC File Offset: 0x000F13CC
		public List<TaskData> GetCurrTaskList()
		{
			Thread.MemoryBarrier();
			bool flag = BaseGameDataDomain.IsCached(this.DataStates, 30);
			List<TaskData> currTaskList;
			if (flag)
			{
				currTaskList = this._currTaskList;
			}
			else
			{
				List<TaskData> value = new List<TaskData>();
				this.CalcCurrTaskList(value);
				bool lockTaken = false;
				try
				{
					this._spinLockCurrTaskList.Enter(ref lockTaken);
					this._currTaskList.Assign(value);
					BaseGameDataDomain.SetCached(this.DataStates, 30);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLockCurrTaskList.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				currTaskList = this._currTaskList;
			}
			return currTaskList;
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x000F3270 File Offset: 0x000F1470
		public List<TaskDisplayData> GetSortedTaskList()
		{
			Thread.MemoryBarrier();
			bool flag = BaseGameDataDomain.IsCached(this.DataStates, 31);
			List<TaskDisplayData> sortedTaskList;
			if (flag)
			{
				sortedTaskList = this._sortedTaskList;
			}
			else
			{
				List<TaskDisplayData> value = new List<TaskDisplayData>();
				this.CalcSortedTaskList(value);
				bool lockTaken = false;
				try
				{
					this._spinLockSortedTaskList.Enter(ref lockTaken);
					this._sortedTaskList.Assign(value);
					BaseGameDataDomain.SetCached(this.DataStates, 31);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLockSortedTaskList.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				sortedTaskList = this._sortedTaskList;
			}
			return sortedTaskList;
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x000F3314 File Offset: 0x000F1514
		public ref WorldStateData GetWorldStateData()
		{
			Thread.MemoryBarrier();
			bool flag = BaseGameDataDomain.IsCached(this.DataStates, 32);
			WorldStateData result;
			if (flag)
			{
				result = ref this._worldStateData;
			}
			else
			{
				WorldStateData value = this.CalcWorldStateData();
				bool lockTaken = false;
				try
				{
					this._spinLockWorldStateData.Enter(ref lockTaken);
					this._worldStateData = value;
					BaseGameDataDomain.SetCached(this.DataStates, 32);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLockWorldStateData.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				result = ref this._worldStateData;
			}
			return ref result;
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x000F33AC File Offset: 0x000F15AC
		public sbyte GetArchiveFilesBackupCount()
		{
			return this._archiveFilesBackupCount;
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x000F33C4 File Offset: 0x000F15C4
		public void SetArchiveFilesBackupCount(sbyte value, DataContext context)
		{
			this._archiveFilesBackupCount = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(33, this.DataStates, WorldDomain.CacheInfluences, context);
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x000F33E4 File Offset: 0x000F15E4
		public List<int> GetSortedMonthlyNotificationSortingGroups()
		{
			return this._sortedMonthlyNotificationSortingGroups;
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x000F33FC File Offset: 0x000F15FC
		public void SetSortedMonthlyNotificationSortingGroups(List<int> value, DataContext context)
		{
			this._sortedMonthlyNotificationSortingGroups = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(34, this.DataStates, WorldDomain.CacheInfluences, context);
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x000F341A File Offset: 0x000F161A
		public override void OnInitializeGameDataModule()
		{
			this.InitializeOnInitializeGameDataModule();
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x000F3424 File Offset: 0x000F1624
		public unsafe override void OnEnterNewWorld()
		{
			this.InitializeOnEnterNewWorld();
			this.InitializeInternalDataOfCollections();
			byte* pData = OperationAdder.FixedSingleValue_Set(1, 0, 4);
			*(int*)pData = (int)this._worldId;
			pData += 4;
			byte* pData2 = OperationAdder.FixedElementList_InsertRange(1, 2, 0, 9, 72);
			for (int i = 0; i < 9; i++)
			{
				pData2 += this._xiangshuAvatarTaskStatuses[i].Serialize(pData2);
			}
			byte* pData3 = OperationAdder.FixedSingleValue_Set(1, 3, 9);
			for (int j = 0; j < 9; j++)
			{
				pData3[j] = (byte)this._xiangshuAvatarTasksInOrder[j];
			}
			pData3 += 9;
			byte* pData4 = OperationAdder.FixedElementList_InsertRange(1, 4, 0, 15, 15);
			for (int k = 0; k < 15; k++)
			{
				pData4[k] = (byte)this._stateTaskStatuses[k];
			}
			pData4 += 15;
			byte* pData5 = OperationAdder.FixedSingleValue_Set(1, 5, 2);
			*(short*)pData5 = this._mainStoryLineProgress;
			pData5 += 2;
			byte* pData6 = OperationAdder.FixedSingleValue_Set(1, 6, 1);
			*pData6 = (this._beatRanChenZi ? 1 : 0);
			pData6++;
			byte* pData7 = OperationAdder.FixedSingleValue_Set(1, 7, 8);
			*(long*)pData7 = (long)this._worldFunctionsStatuses;
			pData7 += 8;
			foreach (KeyValuePair<int, string> entry in this._customTexts)
			{
				int elementId = entry.Key;
				string value = entry.Value;
				bool flag = value != null;
				if (flag)
				{
					int elementsCount = value.Length;
					int contentSize = 2 * elementsCount;
					byte* pData8 = OperationAdder.DynamicSingleValueCollection_Add<int>(1, 8, elementId, contentSize);
					try
					{
						char* ptr;
						if (value == null)
						{
							ptr = null;
						}
						else
						{
							fixed (char* ptr2 = value.GetPinnableReference())
							{
								ptr = ptr2;
							}
						}
						char* pChar = ptr;
						for (int l = 0; l < elementsCount; l++)
						{
							*(short*)(pData8 + (IntPtr)l * 2) = (short)pChar[l];
						}
					}
					finally
					{
						char* ptr2 = null;
					}
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<int>(1, 8, elementId, 0);
				}
			}
			byte* pData9 = OperationAdder.FixedSingleValue_Set(1, 9, 4);
			*(int*)pData9 = this._nextCustomTextId;
			pData9 += 4;
			int dataSize = this._instantNotifications.GetSize();
			byte* pData10 = OperationAdder.Binary_Write(1, 10, 0, dataSize);
			this._instantNotifications.CopyTo(0, dataSize, pData10);
			ushort metadataSize = this._instantNotifications.GetSerializedFixedSizeOfMetadata();
			byte* pMetadata = OperationAdder.Binary_SetMetadata(1, 10, metadataSize);
			this._instantNotifications.SerializeMetadata(pMetadata);
			int dataSize2 = this._lastMonthlyNotifications.GetSerializedSize();
			byte* pData11 = OperationAdder.DynamicSingleValue_Set(1, 12, dataSize2);
			pData11 += this._lastMonthlyNotifications.Serialize(pData11);
			byte* pData12 = OperationAdder.FixedSingleValue_Set(1, 13, 1);
			*pData12 = this._worldPopulationType;
			pData12++;
			byte* pData13 = OperationAdder.FixedSingleValue_Set(1, 14, 1);
			*pData13 = this._characterLifespanType;
			pData13++;
			byte* pData14 = OperationAdder.FixedSingleValue_Set(1, 15, 1);
			*pData14 = this._combatDifficulty;
			pData14++;
			byte* pData15 = OperationAdder.FixedSingleValue_Set(1, 16, 1);
			*pData15 = this._hereticsAmountType;
			pData15++;
			byte* pData16 = OperationAdder.FixedSingleValue_Set(1, 17, 1);
			*pData16 = this._bossInvasionSpeedType;
			pData16++;
			byte* pData17 = OperationAdder.FixedSingleValue_Set(1, 18, 1);
			*pData17 = this._worldResourceAmountType;
			pData17++;
			byte* pData18 = OperationAdder.FixedSingleValue_Set(1, 19, 1);
			*pData18 = (this._allowRandomTaiwuHeir ? 1 : 0);
			pData18++;
			byte* pData19 = OperationAdder.FixedSingleValue_Set(1, 20, 1);
			*pData19 = (this._restrictOptionsBehaviorType ? 1 : 0);
			pData19++;
			byte* pData20 = OperationAdder.FixedSingleValue_Set(1, 21, 1);
			*pData20 = (byte)this._taiwuVillageStateTemplateId;
			pData20++;
			byte* pData21 = OperationAdder.FixedSingleValue_Set(1, 22, 1);
			*pData21 = (byte)this._taiwuVillageLandFormType;
			pData21++;
			byte* pData22 = OperationAdder.FixedSingleValue_Set(1, 26, 4);
			*(int*)pData22 = this._worldStandardPopulation;
			pData22 += 4;
			byte* pData23 = OperationAdder.FixedSingleValue_Set(1, 27, 4);
			*(int*)pData23 = this._currDate;
			pData23 += 4;
			byte* pData24 = OperationAdder.FixedSingleValue_Set(1, 28, 1);
			*pData24 = (byte)this._daysInCurrMonth;
			pData24++;
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x000F3844 File Offset: 0x000F1A44
		public override void OnLoadWorld()
		{
			this._pendingLoadingOperationIds = new Queue<uint>();
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 0));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedElementList_GetAll(1, 2));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 3));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedElementList_GetAll(1, 4));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 5));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 6));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 7));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(1, 8));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 9));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.Binary_Get(1, 10));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(1, 12));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 13));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 14));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 15));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 16));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 17));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 18));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 19));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 20));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 21));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 22));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 26));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 27));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 28));
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x000F3A38 File Offset: 0x000F1C38
		public unsafe override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
		{
			int result;
			switch (dataId)
			{
			case 0:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
				}
				result = Serializer.Serialize(this._worldId, dataPool);
				break;
			case 1:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
				}
				result = Serializer.Serialize(this.GetXiangshuProgress(), dataPool);
				break;
			case 2:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesXiangshuAvatarTaskStatuses, (int)subId0);
				}
				result = Serializer.Serialize(this._xiangshuAvatarTaskStatuses[(int)subId0], dataPool);
				break;
			case 3:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
				}
				result = Serializer.Serialize(this._xiangshuAvatarTasksInOrder, dataPool);
				break;
			case 4:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesStateTaskStatuses, (int)subId0);
				}
				result = Serializer.Serialize(this._stateTaskStatuses[(int)subId0], dataPool);
				break;
			case 5:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
				}
				result = Serializer.Serialize(this._mainStoryLineProgress, dataPool);
				break;
			case 6:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
				}
				result = Serializer.Serialize(this._beatRanChenZi, dataPool);
				break;
			case 7:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
				}
				result = Serializer.Serialize(this._worldFunctionsStatuses, dataPool);
				break;
			case 8:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
					this._modificationsCustomTexts.Reset();
				}
				result = Serializer.SerializeModifications<int>(this._customTexts, dataPool);
				break;
			case 9:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 10:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 10);
					this._modificationsInstantNotifications.Reset(this._instantNotifications.GetSize());
				}
				result = Serializer.SerializeModifications(this._instantNotifications, dataPool);
				break;
			case 11:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
				}
				result = Serializer.Serialize(this._onHandingMonthlyEventBlock, dataPool);
				break;
			case 12:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 12);
				}
				result = Serializer.Serialize(this._lastMonthlyNotifications, dataPool);
				break;
			case 13:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 13);
				}
				result = Serializer.Serialize(this._worldPopulationType, dataPool);
				break;
			case 14:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 14);
				}
				result = Serializer.Serialize(this._characterLifespanType, dataPool);
				break;
			case 15:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
				}
				result = Serializer.Serialize(this._combatDifficulty, dataPool);
				break;
			case 16:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
				}
				result = Serializer.Serialize(this._hereticsAmountType, dataPool);
				break;
			case 17:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 17);
				}
				result = Serializer.Serialize(this._bossInvasionSpeedType, dataPool);
				break;
			case 18:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
				}
				result = Serializer.Serialize(this._worldResourceAmountType, dataPool);
				break;
			case 19:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 19);
				}
				result = Serializer.Serialize(this._allowRandomTaiwuHeir, dataPool);
				break;
			case 20:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
				}
				result = Serializer.Serialize(this._restrictOptionsBehaviorType, dataPool);
				break;
			case 21:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
				}
				result = Serializer.Serialize(this._taiwuVillageStateTemplateId, dataPool);
				break;
			case 22:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
				}
				result = Serializer.Serialize(this._taiwuVillageLandFormType, dataPool);
				break;
			case 23:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
				}
				result = Serializer.Serialize(this._hideTaiwuOriginalSurname, dataPool);
				break;
			case 24:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
				}
				result = Serializer.Serialize(this._allowExecute, dataPool);
				break;
			case 25:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 25);
				}
				result = Serializer.Serialize(this._archiveFilesBackupInterval, dataPool);
				break;
			case 26:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 26);
				}
				result = Serializer.Serialize(this._worldStandardPopulation, dataPool);
				break;
			case 27:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 27);
				}
				result = Serializer.Serialize(this._currDate, dataPool);
				break;
			case 28:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 28);
				}
				result = Serializer.Serialize(this._daysInCurrMonth, dataPool);
				break;
			case 29:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 29);
				}
				result = Serializer.Serialize(this._advancingMonthState, dataPool);
				break;
			case 30:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 30);
				}
				result = Serializer.Serialize(this.GetCurrTaskList(), dataPool);
				break;
			case 31:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 31);
				}
				result = Serializer.Serialize(this.GetSortedTaskList(), dataPool);
				break;
			case 32:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 32);
				}
				result = Serializer.Serialize(*this.GetWorldStateData(), dataPool);
				break;
			case 33:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 33);
				}
				result = Serializer.Serialize(this._archiveFilesBackupCount, dataPool);
				break;
			case 34:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 34);
				}
				result = Serializer.Serialize(this._sortedMonthlyNotificationSortingGroups, dataPool);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x000F40D4 File Offset: 0x000F22D4
		public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				XiangshuAvatarTaskStatus value = default(XiangshuAvatarTaskStatus);
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				this._xiangshuAvatarTaskStatuses[(int)subId0] = value;
				this.SetElement_XiangshuAvatarTaskStatuses((int)subId0, value, context);
				break;
			}
			case 3:
				Serializer.Deserialize(dataPool, valueOffset, ref this._xiangshuAvatarTasksInOrder);
				this.SetXiangshuAvatarTasksInOrder(this._xiangshuAvatarTasksInOrder, context);
				break;
			case 4:
			{
				sbyte value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				this._stateTaskStatuses[(int)subId0] = value2;
				this.SetElement_StateTaskStatuses((int)subId0, value2, context);
				break;
			}
			case 5:
				Serializer.Deserialize(dataPool, valueOffset, ref this._mainStoryLineProgress);
				this.SetMainStoryLineProgress(this._mainStoryLineProgress, context);
				break;
			case 6:
				Serializer.Deserialize(dataPool, valueOffset, ref this._beatRanChenZi);
				this.SetBeatRanChenZi(this._beatRanChenZi, context);
				break;
			case 7:
				Serializer.Deserialize(dataPool, valueOffset, ref this._worldFunctionsStatuses);
				this.SetWorldFunctionsStatuses(this._worldFunctionsStatuses, context);
				break;
			case 8:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 9:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 10:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 11:
				Serializer.Deserialize(dataPool, valueOffset, ref this._onHandingMonthlyEventBlock);
				this.SetOnHandingMonthlyEventBlock(this._onHandingMonthlyEventBlock, context);
				break;
			case 12:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 13:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 14:
				Serializer.Deserialize(dataPool, valueOffset, ref this._characterLifespanType);
				this.SetCharacterLifespanType(this._characterLifespanType, context);
				break;
			case 15:
				Serializer.Deserialize(dataPool, valueOffset, ref this._combatDifficulty);
				this.SetCombatDifficulty(this._combatDifficulty, context);
				break;
			case 16:
				Serializer.Deserialize(dataPool, valueOffset, ref this._hereticsAmountType);
				this.SetHereticsAmountType(this._hereticsAmountType, context);
				break;
			case 17:
				Serializer.Deserialize(dataPool, valueOffset, ref this._bossInvasionSpeedType);
				this.SetBossInvasionSpeedType(this._bossInvasionSpeedType, context);
				break;
			case 18:
				Serializer.Deserialize(dataPool, valueOffset, ref this._worldResourceAmountType);
				this.SetWorldResourceAmountType(this._worldResourceAmountType, context);
				break;
			case 19:
				Serializer.Deserialize(dataPool, valueOffset, ref this._allowRandomTaiwuHeir);
				this.SetAllowRandomTaiwuHeir(this._allowRandomTaiwuHeir, context);
				break;
			case 20:
				Serializer.Deserialize(dataPool, valueOffset, ref this._restrictOptionsBehaviorType);
				this.SetRestrictOptionsBehaviorType(this._restrictOptionsBehaviorType, context);
				break;
			case 21:
				Serializer.Deserialize(dataPool, valueOffset, ref this._taiwuVillageStateTemplateId);
				this.SetTaiwuVillageStateTemplateId(this._taiwuVillageStateTemplateId, context);
				break;
			case 22:
				Serializer.Deserialize(dataPool, valueOffset, ref this._taiwuVillageLandFormType);
				this.SetTaiwuVillageLandFormType(this._taiwuVillageLandFormType, context);
				break;
			case 23:
				Serializer.Deserialize(dataPool, valueOffset, ref this._hideTaiwuOriginalSurname);
				this.SetHideTaiwuOriginalSurname(this._hideTaiwuOriginalSurname, context);
				break;
			case 24:
				Serializer.Deserialize(dataPool, valueOffset, ref this._allowExecute);
				this.SetAllowExecute(this._allowExecute, context);
				break;
			case 25:
				Serializer.Deserialize(dataPool, valueOffset, ref this._archiveFilesBackupInterval);
				this.SetArchiveFilesBackupInterval(this._archiveFilesBackupInterval, context);
				break;
			case 26:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 27:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 28:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 29:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 30:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 31:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 32:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 33:
				Serializer.Deserialize(dataPool, valueOffset, ref this._archiveFilesBackupCount);
				this.SetArchiveFilesBackupCount(this._archiveFilesBackupCount, context);
				break;
			case 34:
				Serializer.Deserialize(dataPool, valueOffset, ref this._sortedMonthlyNotificationSortingGroups);
				this.SetSortedMonthlyNotificationSortingGroups(this._sortedMonthlyNotificationSortingGroups, context);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x000F473C File Offset: 0x000F293C
		public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
		{
			int argsOffset = operation.ArgsOffset;
			int result;
			switch (operation.MethodId)
			{
			case 0:
			{
				int argsCount = operation.ArgsCount;
				int num = argsCount;
				if (num != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				WorldCreationInfo info = default(WorldCreationInfo);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref info);
				this.CreateWorld(context, info);
				result = -1;
				break;
			}
			case 1:
			{
				int argsCount2 = operation.ArgsCount;
				int num2 = argsCount2;
				if (num2 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				WorldCreationInfo info2 = default(WorldCreationInfo);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref info2);
				bool inherit = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref inherit);
				this.SetWorldCreationInfo(context, info2, inherit);
				result = -1;
				break;
			}
			case 2:
			{
				int argsCount3 = operation.ArgsCount;
				int num3 = argsCount3;
				if (num3 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				WorldCreationInfo returnValue = this.GetWorldCreationInfo();
				result = Serializer.Serialize(returnValue, returnDataPool);
				break;
			}
			case 3:
			{
				int argsCount4 = operation.ArgsCount;
				int num4 = argsCount4;
				if (num4 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<Location> returnValue2 = this.GetJuniorXiangshuLocations();
				result = Serializer.Serialize(returnValue2, returnDataPool);
				break;
			}
			case 4:
			{
				int argsCount5 = operation.ArgsCount;
				int num5 = argsCount5;
				if (num5 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int offset = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref offset);
				this.HandleMonthlyEvent(context, offset);
				result = -1;
				break;
			}
			case 5:
			{
				int argsCount6 = operation.ArgsCount;
				int num6 = argsCount6;
				if (num6 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				MonthlyEventCollection returnValue3 = this.GetMonthlyEventCollection();
				result = Serializer.Serialize(returnValue3, returnDataPool);
				break;
			}
			case 6:
			{
				int argsCount7 = operation.ArgsCount;
				int num7 = argsCount7;
				if (num7 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.RemoveAllInvalidMonthlyEvents(context);
				result = -1;
				break;
			}
			case 7:
			{
				int argsCount8 = operation.ArgsCount;
				int num8 = argsCount8;
				if (num8 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.ProcessAllMonthlyEventsWithDefaultOption(context);
				result = -1;
				break;
			}
			case 8:
			{
				int argsCount9 = operation.ArgsCount;
				int num9 = argsCount9;
				if (num9 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				byte worldPopulationType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref worldPopulationType);
				this.SpecifyWorldPopulationType(context, worldPopulationType);
				result = -1;
				break;
			}
			case 9:
			{
				int argsCount10 = operation.ArgsCount;
				int num10 = argsCount10;
				if (num10 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int days = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref days);
				this.AdvanceDaysInMonth(context, days);
				result = -1;
				break;
			}
			case 10:
			{
				int argsCount11 = operation.ArgsCount;
				int num11 = argsCount11;
				if (num11 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.AdvanceMonth(context);
				result = -1;
				break;
			}
			case 11:
			{
				int argsCount12 = operation.ArgsCount;
				int num12 = argsCount12;
				if (num12 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool saveWorld = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref saveWorld);
				this.AdvanceMonth_DisplayedMonthlyNotifications(context, saveWorld);
				result = -1;
				break;
			}
			case 12:
			{
				int argsCount13 = operation.ArgsCount;
				int num13 = argsCount13;
				if (num13 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short combatSkillId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref combatSkillId);
				short bonusTypeTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref bonusTypeTemplateId);
				bool returnValue4 = this.GmCmd_SectEmeiAddSkillBreakBonus(context, combatSkillId, bonusTypeTemplateId);
				result = Serializer.Serialize(returnValue4, returnDataPool);
				break;
			}
			case 13:
			{
				int argsCount14 = operation.ArgsCount;
				int num14 = argsCount14;
				if (num14 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short combatSkillId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref combatSkillId2);
				bool returnValue5 = this.GmCmd_SectEmeiClearSkillBreakBonus(context, combatSkillId2);
				result = Serializer.Serialize(returnValue5, returnDataPool);
				break;
			}
			case 14:
			{
				int argsCount15 = operation.ArgsCount;
				int num15 = argsCount15;
				if (num15 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemKey returnValue6 = this.RefiningWugKing(context);
				result = Serializer.Serialize(returnValue6, returnDataPool);
				break;
			}
			case 15:
			{
				int argsCount16 = operation.ArgsCount;
				int num16 = argsCount16;
				if (num16 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<ItemKey> poisonMaterials = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref poisonMaterials);
				bool returnValue7 = this.DropPoisonsToWugJug(context, poisonMaterials);
				result = Serializer.Serialize(returnValue7, returnDataPool);
				break;
			}
			case 16:
			{
				int argsCount17 = operation.ArgsCount;
				int num17 = argsCount17;
				if (num17 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool returnValue8 = this.JingangMonkSoulBtnShow();
				result = Serializer.Serialize(returnValue8, returnDataPool);
				break;
			}
			case 17:
			{
				int argsCount18 = operation.ArgsCount;
				int num18 = argsCount18;
				if (num18 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool returnValue9 = this.JingangSoulTransformOpen();
				result = Serializer.Serialize(returnValue9, returnDataPool);
				break;
			}
			case 18:
			{
				int argsCount19 = operation.ArgsCount;
				int num19 = argsCount19;
				if (num19 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte returnValue10 = this.GetBaihuaLifeLinkNeiliType();
				result = Serializer.Serialize(returnValue10, returnDataPool);
				break;
			}
			case 19:
			{
				int argsCount20 = operation.ArgsCount;
				int num20 = argsCount20;
				if (num20 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId);
				int index = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index);
				bool isLifeGate = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isLifeGate);
				this.SetLifeLinkCharacter(context, charId, index, isLifeGate);
				result = -1;
				break;
			}
			case 20:
			{
				int argsCount21 = operation.ArgsCount;
				int num21 = argsCount21;
				if (num21 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short bonusTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref bonusTemplateId);
				List<ItemKey> itemKeys = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKeys);
				bool returnValue11 = this.EmeiTransferBonusProgress(context, bonusTemplateId, itemKeys);
				result = Serializer.Serialize(returnValue11, returnDataPool);
				break;
			}
			case 21:
			{
				int argsCount22 = operation.ArgsCount;
				int num22 = argsCount22;
				if (num22 != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short startTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref startTemplateId);
				short endTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref endTemplateId);
				int selfCharId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref selfCharId);
				int targetCharId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref targetCharId);
				bool returnValue12 = this.GmCmd_AddMonthlyEvent(startTemplateId, endTemplateId, selfCharId, targetCharId);
				result = Serializer.Serialize(returnValue12, returnDataPool);
				break;
			}
			case 22:
			{
				int argsCount23 = operation.ArgsCount;
				int num23 = argsCount23;
				if (num23 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte thiefLevel = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref thiefLevel);
				bool timeOut = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref timeOut);
				this.CatchThief(thiefLevel, timeOut);
				result = -1;
				break;
			}
			case 23:
			{
				int argsCount24 = operation.ArgsCount;
				int num24 = argsCount24;
				if (num24 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int returnValue13 = this.TryTriggerThiefCatch(context);
				result = Serializer.Serialize(returnValue13, returnDataPool);
				break;
			}
			case 24:
			{
				int argsCount25 = operation.ArgsCount;
				int num25 = argsCount25;
				if (num25 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int index2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index2);
				this.ShaolinStartDemonSlayerTrial(context, index2);
				result = -1;
				break;
			}
			case 25:
			{
				int argsCount26 = operation.ArgsCount;
				int num26 = argsCount26;
				if (num26 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool returnValue14 = this.ShaolinInterruptDemonSlayerTrial(context);
				result = Serializer.Serialize(returnValue14, returnDataPool);
				break;
			}
			case 26:
			{
				int argsCount27 = operation.ArgsCount;
				int num27 = argsCount27;
				if (num27 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool returnValue15 = this.ShaolinRegenerateRestricts(context);
				result = Serializer.Serialize(returnValue15, returnDataPool);
				break;
			}
			case 27:
			{
				int argsCount28 = operation.ArgsCount;
				int num28 = argsCount28;
				if (num28 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int index3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index3);
				byte returnValue16 = this.ShaolinQueryRestrictsAreSatisfied(index3);
				result = Serializer.Serialize(returnValue16, returnDataPool);
				break;
			}
			case 28:
			{
				int argsCount29 = operation.ArgsCount;
				int num29 = argsCount29;
				if (num29 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<int> demonCharIds = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref demonCharIds);
				this.ShaolinClearTemporaryDemon(context, demonCharIds);
				result = -1;
				break;
			}
			case 29:
			{
				int argsCount30 = operation.ArgsCount;
				int num30 = argsCount30;
				if (num30 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<int> returnValue17 = this.ShaolinGenerateTemporaryDemon(context);
				result = Serializer.Serialize(returnValue17, returnDataPool);
				break;
			}
			case 30:
			{
				int argsCount31 = operation.ArgsCount;
				int num31 = argsCount31;
				if (num31 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short templateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId);
				int returnValue18 = this.GetSectMainStoryTriggerConditions(templateId);
				result = Serializer.Serialize(returnValue18, returnDataPool);
				break;
			}
			case 31:
			{
				int argsCount32 = operation.ArgsCount;
				int num32 = argsCount32;
				if (num32 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte orgTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref orgTemplateId);
				int returnValue19 = this.GetSectMainStoryActiveStatus(orgTemplateId);
				result = Serializer.Serialize(returnValue19, returnDataPool);
				break;
			}
			case 32:
			{
				int argsCount33 = operation.ArgsCount;
				int num33 = argsCount33;
				if (num33 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte orgTemplateId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref orgTemplateId2);
				bool pause = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref pause);
				this.SetSectMainStoryActiveStatus(orgTemplateId2, pause);
				result = -1;
				break;
			}
			case 33:
			{
				int argsCount34 = operation.ArgsCount;
				int num34 = argsCount34;
				if (num34 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte orgTemplateId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref orgTemplateId3);
				this.NotifySectStoryActivated(context, orgTemplateId3);
				result = -1;
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x000F5648 File Offset: 0x000F3848
		public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
		{
			switch (dataId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
			case 8:
				this._modificationsCustomTexts.ChangeRecording(monitoring);
				break;
			case 9:
				break;
			case 10:
				this._modificationsInstantNotifications.ChangeRecording(monitoring, this._instantNotifications.GetSize());
				break;
			case 11:
				break;
			case 12:
				break;
			case 13:
				break;
			case 14:
				break;
			case 15:
				break;
			case 16:
				break;
			case 17:
				break;
			case 18:
				break;
			case 19:
				break;
			case 20:
				break;
			case 21:
				break;
			case 22:
				break;
			case 23:
				break;
			case 24:
				break;
			case 25:
				break;
			case 26:
				break;
			case 27:
				break;
			case 28:
				break;
			case 29:
				break;
			case 30:
				break;
			case 31:
				break;
			case 32:
				break;
			case 33:
				break;
			case 34:
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x000F57A4 File Offset: 0x000F39A4
		public unsafe override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
		{
			int result;
			switch (dataId)
			{
			case 0:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 0);
				if (flag)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
					result = Serializer.Serialize(this._worldId, dataPool);
				}
				break;
			}
			case 1:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (flag2)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
					result = Serializer.Serialize(this.GetXiangshuProgress(), dataPool);
				}
				break;
			}
			case 2:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this._dataStatesXiangshuAvatarTaskStatuses, (int)subId0);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this._dataStatesXiangshuAvatarTaskStatuses, (int)subId0);
					result = Serializer.Serialize(this._xiangshuAvatarTaskStatuses[(int)subId0], dataPool);
				}
				break;
			}
			case 3:
			{
				bool flag4 = !BaseGameDataDomain.IsModified(this.DataStates, 3);
				if (flag4)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
					result = Serializer.Serialize(this._xiangshuAvatarTasksInOrder, dataPool);
				}
				break;
			}
			case 4:
			{
				bool flag5 = !BaseGameDataDomain.IsModified(this._dataStatesStateTaskStatuses, (int)subId0);
				if (flag5)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this._dataStatesStateTaskStatuses, (int)subId0);
					result = Serializer.Serialize(this._stateTaskStatuses[(int)subId0], dataPool);
				}
				break;
			}
			case 5:
			{
				bool flag6 = !BaseGameDataDomain.IsModified(this.DataStates, 5);
				if (flag6)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
					result = Serializer.Serialize(this._mainStoryLineProgress, dataPool);
				}
				break;
			}
			case 6:
			{
				bool flag7 = !BaseGameDataDomain.IsModified(this.DataStates, 6);
				if (flag7)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
					result = Serializer.Serialize(this._beatRanChenZi, dataPool);
				}
				break;
			}
			case 7:
			{
				bool flag8 = !BaseGameDataDomain.IsModified(this.DataStates, 7);
				if (flag8)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
					result = Serializer.Serialize(this._worldFunctionsStatuses, dataPool);
				}
				break;
			}
			case 8:
			{
				bool flag9 = !BaseGameDataDomain.IsModified(this.DataStates, 8);
				if (flag9)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
					int offset = Serializer.SerializeModifications<int>(this._customTexts, dataPool, this._modificationsCustomTexts);
					this._modificationsCustomTexts.Reset();
					result = offset;
				}
				break;
			}
			case 9:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 10:
			{
				bool flag10 = !BaseGameDataDomain.IsModified(this.DataStates, 10);
				if (flag10)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 10);
					int offset2 = Serializer.SerializeModifications(this._instantNotifications, dataPool, this._modificationsInstantNotifications);
					this._modificationsInstantNotifications.Reset(this._instantNotifications.GetSize());
					result = offset2;
				}
				break;
			}
			case 11:
			{
				bool flag11 = !BaseGameDataDomain.IsModified(this.DataStates, 11);
				if (flag11)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
					result = Serializer.Serialize(this._onHandingMonthlyEventBlock, dataPool);
				}
				break;
			}
			case 12:
			{
				bool flag12 = !BaseGameDataDomain.IsModified(this.DataStates, 12);
				if (flag12)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 12);
					result = Serializer.Serialize(this._lastMonthlyNotifications, dataPool);
				}
				break;
			}
			case 13:
			{
				bool flag13 = !BaseGameDataDomain.IsModified(this.DataStates, 13);
				if (flag13)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 13);
					result = Serializer.Serialize(this._worldPopulationType, dataPool);
				}
				break;
			}
			case 14:
			{
				bool flag14 = !BaseGameDataDomain.IsModified(this.DataStates, 14);
				if (flag14)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 14);
					result = Serializer.Serialize(this._characterLifespanType, dataPool);
				}
				break;
			}
			case 15:
			{
				bool flag15 = !BaseGameDataDomain.IsModified(this.DataStates, 15);
				if (flag15)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
					result = Serializer.Serialize(this._combatDifficulty, dataPool);
				}
				break;
			}
			case 16:
			{
				bool flag16 = !BaseGameDataDomain.IsModified(this.DataStates, 16);
				if (flag16)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
					result = Serializer.Serialize(this._hereticsAmountType, dataPool);
				}
				break;
			}
			case 17:
			{
				bool flag17 = !BaseGameDataDomain.IsModified(this.DataStates, 17);
				if (flag17)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 17);
					result = Serializer.Serialize(this._bossInvasionSpeedType, dataPool);
				}
				break;
			}
			case 18:
			{
				bool flag18 = !BaseGameDataDomain.IsModified(this.DataStates, 18);
				if (flag18)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
					result = Serializer.Serialize(this._worldResourceAmountType, dataPool);
				}
				break;
			}
			case 19:
			{
				bool flag19 = !BaseGameDataDomain.IsModified(this.DataStates, 19);
				if (flag19)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 19);
					result = Serializer.Serialize(this._allowRandomTaiwuHeir, dataPool);
				}
				break;
			}
			case 20:
			{
				bool flag20 = !BaseGameDataDomain.IsModified(this.DataStates, 20);
				if (flag20)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
					result = Serializer.Serialize(this._restrictOptionsBehaviorType, dataPool);
				}
				break;
			}
			case 21:
			{
				bool flag21 = !BaseGameDataDomain.IsModified(this.DataStates, 21);
				if (flag21)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
					result = Serializer.Serialize(this._taiwuVillageStateTemplateId, dataPool);
				}
				break;
			}
			case 22:
			{
				bool flag22 = !BaseGameDataDomain.IsModified(this.DataStates, 22);
				if (flag22)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
					result = Serializer.Serialize(this._taiwuVillageLandFormType, dataPool);
				}
				break;
			}
			case 23:
			{
				bool flag23 = !BaseGameDataDomain.IsModified(this.DataStates, 23);
				if (flag23)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
					result = Serializer.Serialize(this._hideTaiwuOriginalSurname, dataPool);
				}
				break;
			}
			case 24:
			{
				bool flag24 = !BaseGameDataDomain.IsModified(this.DataStates, 24);
				if (flag24)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
					result = Serializer.Serialize(this._allowExecute, dataPool);
				}
				break;
			}
			case 25:
			{
				bool flag25 = !BaseGameDataDomain.IsModified(this.DataStates, 25);
				if (flag25)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 25);
					result = Serializer.Serialize(this._archiveFilesBackupInterval, dataPool);
				}
				break;
			}
			case 26:
			{
				bool flag26 = !BaseGameDataDomain.IsModified(this.DataStates, 26);
				if (flag26)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 26);
					result = Serializer.Serialize(this._worldStandardPopulation, dataPool);
				}
				break;
			}
			case 27:
			{
				bool flag27 = !BaseGameDataDomain.IsModified(this.DataStates, 27);
				if (flag27)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 27);
					result = Serializer.Serialize(this._currDate, dataPool);
				}
				break;
			}
			case 28:
			{
				bool flag28 = !BaseGameDataDomain.IsModified(this.DataStates, 28);
				if (flag28)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 28);
					result = Serializer.Serialize(this._daysInCurrMonth, dataPool);
				}
				break;
			}
			case 29:
			{
				bool flag29 = !BaseGameDataDomain.IsModified(this.DataStates, 29);
				if (flag29)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 29);
					result = Serializer.Serialize(this._advancingMonthState, dataPool);
				}
				break;
			}
			case 30:
			{
				bool flag30 = !BaseGameDataDomain.IsModified(this.DataStates, 30);
				if (flag30)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 30);
					result = Serializer.Serialize(this.GetCurrTaskList(), dataPool);
				}
				break;
			}
			case 31:
			{
				bool flag31 = !BaseGameDataDomain.IsModified(this.DataStates, 31);
				if (flag31)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 31);
					result = Serializer.Serialize(this.GetSortedTaskList(), dataPool);
				}
				break;
			}
			case 32:
			{
				bool flag32 = !BaseGameDataDomain.IsModified(this.DataStates, 32);
				if (flag32)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 32);
					result = Serializer.Serialize(*this.GetWorldStateData(), dataPool);
				}
				break;
			}
			case 33:
			{
				bool flag33 = !BaseGameDataDomain.IsModified(this.DataStates, 33);
				if (flag33)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 33);
					result = Serializer.Serialize(this._archiveFilesBackupCount, dataPool);
				}
				break;
			}
			case 34:
			{
				bool flag34 = !BaseGameDataDomain.IsModified(this.DataStates, 34);
				if (flag34)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 34);
					result = Serializer.Serialize(this._sortedMonthlyNotificationSortingGroups, dataPool);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x000F6134 File Offset: 0x000F4334
		public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			switch (dataId)
			{
			case 0:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 0);
				if (!flag)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
				}
				break;
			}
			case 1:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (!flag2)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
				}
				break;
			}
			case 2:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this._dataStatesXiangshuAvatarTaskStatuses, (int)subId0);
				if (!flag3)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesXiangshuAvatarTaskStatuses, (int)subId0);
				}
				break;
			}
			case 3:
			{
				bool flag4 = !BaseGameDataDomain.IsModified(this.DataStates, 3);
				if (!flag4)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
				}
				break;
			}
			case 4:
			{
				bool flag5 = !BaseGameDataDomain.IsModified(this._dataStatesStateTaskStatuses, (int)subId0);
				if (!flag5)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesStateTaskStatuses, (int)subId0);
				}
				break;
			}
			case 5:
			{
				bool flag6 = !BaseGameDataDomain.IsModified(this.DataStates, 5);
				if (!flag6)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
				}
				break;
			}
			case 6:
			{
				bool flag7 = !BaseGameDataDomain.IsModified(this.DataStates, 6);
				if (!flag7)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
				}
				break;
			}
			case 7:
			{
				bool flag8 = !BaseGameDataDomain.IsModified(this.DataStates, 7);
				if (!flag8)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
				}
				break;
			}
			case 8:
			{
				bool flag9 = !BaseGameDataDomain.IsModified(this.DataStates, 8);
				if (!flag9)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
					this._modificationsCustomTexts.Reset();
				}
				break;
			}
			case 9:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 10:
			{
				bool flag10 = !BaseGameDataDomain.IsModified(this.DataStates, 10);
				if (!flag10)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 10);
				}
				break;
			}
			case 11:
			{
				bool flag11 = !BaseGameDataDomain.IsModified(this.DataStates, 11);
				if (!flag11)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
				}
				break;
			}
			case 12:
			{
				bool flag12 = !BaseGameDataDomain.IsModified(this.DataStates, 12);
				if (!flag12)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 12);
				}
				break;
			}
			case 13:
			{
				bool flag13 = !BaseGameDataDomain.IsModified(this.DataStates, 13);
				if (!flag13)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 13);
				}
				break;
			}
			case 14:
			{
				bool flag14 = !BaseGameDataDomain.IsModified(this.DataStates, 14);
				if (!flag14)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 14);
				}
				break;
			}
			case 15:
			{
				bool flag15 = !BaseGameDataDomain.IsModified(this.DataStates, 15);
				if (!flag15)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
				}
				break;
			}
			case 16:
			{
				bool flag16 = !BaseGameDataDomain.IsModified(this.DataStates, 16);
				if (!flag16)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
				}
				break;
			}
			case 17:
			{
				bool flag17 = !BaseGameDataDomain.IsModified(this.DataStates, 17);
				if (!flag17)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 17);
				}
				break;
			}
			case 18:
			{
				bool flag18 = !BaseGameDataDomain.IsModified(this.DataStates, 18);
				if (!flag18)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
				}
				break;
			}
			case 19:
			{
				bool flag19 = !BaseGameDataDomain.IsModified(this.DataStates, 19);
				if (!flag19)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 19);
				}
				break;
			}
			case 20:
			{
				bool flag20 = !BaseGameDataDomain.IsModified(this.DataStates, 20);
				if (!flag20)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
				}
				break;
			}
			case 21:
			{
				bool flag21 = !BaseGameDataDomain.IsModified(this.DataStates, 21);
				if (!flag21)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
				}
				break;
			}
			case 22:
			{
				bool flag22 = !BaseGameDataDomain.IsModified(this.DataStates, 22);
				if (!flag22)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
				}
				break;
			}
			case 23:
			{
				bool flag23 = !BaseGameDataDomain.IsModified(this.DataStates, 23);
				if (!flag23)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
				}
				break;
			}
			case 24:
			{
				bool flag24 = !BaseGameDataDomain.IsModified(this.DataStates, 24);
				if (!flag24)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
				}
				break;
			}
			case 25:
			{
				bool flag25 = !BaseGameDataDomain.IsModified(this.DataStates, 25);
				if (!flag25)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 25);
				}
				break;
			}
			case 26:
			{
				bool flag26 = !BaseGameDataDomain.IsModified(this.DataStates, 26);
				if (!flag26)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 26);
				}
				break;
			}
			case 27:
			{
				bool flag27 = !BaseGameDataDomain.IsModified(this.DataStates, 27);
				if (!flag27)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 27);
				}
				break;
			}
			case 28:
			{
				bool flag28 = !BaseGameDataDomain.IsModified(this.DataStates, 28);
				if (!flag28)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 28);
				}
				break;
			}
			case 29:
			{
				bool flag29 = !BaseGameDataDomain.IsModified(this.DataStates, 29);
				if (!flag29)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 29);
				}
				break;
			}
			case 30:
			{
				bool flag30 = !BaseGameDataDomain.IsModified(this.DataStates, 30);
				if (!flag30)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 30);
				}
				break;
			}
			case 31:
			{
				bool flag31 = !BaseGameDataDomain.IsModified(this.DataStates, 31);
				if (!flag31)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 31);
				}
				break;
			}
			case 32:
			{
				bool flag32 = !BaseGameDataDomain.IsModified(this.DataStates, 32);
				if (!flag32)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 32);
				}
				break;
			}
			case 33:
			{
				bool flag33 = !BaseGameDataDomain.IsModified(this.DataStates, 33);
				if (!flag33)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 33);
				}
				break;
			}
			case 34:
			{
				bool flag34 = !BaseGameDataDomain.IsModified(this.DataStates, 34);
				if (!flag34)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 34);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x000F6860 File Offset: 0x000F4A60
		public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			bool result;
			switch (dataId)
			{
			case 0:
				result = BaseGameDataDomain.IsModified(this.DataStates, 0);
				break;
			case 1:
				result = BaseGameDataDomain.IsModified(this.DataStates, 1);
				break;
			case 2:
				result = BaseGameDataDomain.IsModified(this._dataStatesXiangshuAvatarTaskStatuses, (int)subId0);
				break;
			case 3:
				result = BaseGameDataDomain.IsModified(this.DataStates, 3);
				break;
			case 4:
				result = BaseGameDataDomain.IsModified(this._dataStatesStateTaskStatuses, (int)subId0);
				break;
			case 5:
				result = BaseGameDataDomain.IsModified(this.DataStates, 5);
				break;
			case 6:
				result = BaseGameDataDomain.IsModified(this.DataStates, 6);
				break;
			case 7:
				result = BaseGameDataDomain.IsModified(this.DataStates, 7);
				break;
			case 8:
				result = BaseGameDataDomain.IsModified(this.DataStates, 8);
				break;
			case 9:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 10:
				result = BaseGameDataDomain.IsModified(this.DataStates, 10);
				break;
			case 11:
				result = BaseGameDataDomain.IsModified(this.DataStates, 11);
				break;
			case 12:
				result = BaseGameDataDomain.IsModified(this.DataStates, 12);
				break;
			case 13:
				result = BaseGameDataDomain.IsModified(this.DataStates, 13);
				break;
			case 14:
				result = BaseGameDataDomain.IsModified(this.DataStates, 14);
				break;
			case 15:
				result = BaseGameDataDomain.IsModified(this.DataStates, 15);
				break;
			case 16:
				result = BaseGameDataDomain.IsModified(this.DataStates, 16);
				break;
			case 17:
				result = BaseGameDataDomain.IsModified(this.DataStates, 17);
				break;
			case 18:
				result = BaseGameDataDomain.IsModified(this.DataStates, 18);
				break;
			case 19:
				result = BaseGameDataDomain.IsModified(this.DataStates, 19);
				break;
			case 20:
				result = BaseGameDataDomain.IsModified(this.DataStates, 20);
				break;
			case 21:
				result = BaseGameDataDomain.IsModified(this.DataStates, 21);
				break;
			case 22:
				result = BaseGameDataDomain.IsModified(this.DataStates, 22);
				break;
			case 23:
				result = BaseGameDataDomain.IsModified(this.DataStates, 23);
				break;
			case 24:
				result = BaseGameDataDomain.IsModified(this.DataStates, 24);
				break;
			case 25:
				result = BaseGameDataDomain.IsModified(this.DataStates, 25);
				break;
			case 26:
				result = BaseGameDataDomain.IsModified(this.DataStates, 26);
				break;
			case 27:
				result = BaseGameDataDomain.IsModified(this.DataStates, 27);
				break;
			case 28:
				result = BaseGameDataDomain.IsModified(this.DataStates, 28);
				break;
			case 29:
				result = BaseGameDataDomain.IsModified(this.DataStates, 29);
				break;
			case 30:
				result = BaseGameDataDomain.IsModified(this.DataStates, 30);
				break;
			case 31:
				result = BaseGameDataDomain.IsModified(this.DataStates, 31);
				break;
			case 32:
				result = BaseGameDataDomain.IsModified(this.DataStates, 32);
				break;
			case 33:
				result = BaseGameDataDomain.IsModified(this.DataStates, 33);
				break;
			case 34:
				result = BaseGameDataDomain.IsModified(this.DataStates, 34);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x000F6BD4 File Offset: 0x000F4DD4
		public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (influence.TargetIndicator.DataId)
			{
			case 0:
				break;
			case 1:
				BaseGameDataDomain.InvalidateSelfAndInfluencedCache(1, this.DataStates, WorldDomain.CacheInfluences, context);
				return;
			case 2:
				break;
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
			case 8:
				break;
			case 9:
				break;
			case 10:
				break;
			case 11:
				break;
			case 12:
				break;
			case 13:
				break;
			case 14:
				break;
			case 15:
				break;
			case 16:
				break;
			case 17:
				break;
			case 18:
				break;
			case 19:
				break;
			case 20:
				break;
			case 21:
				break;
			case 22:
				break;
			case 23:
				break;
			case 24:
				break;
			case 25:
				break;
			case 26:
				break;
			case 27:
				break;
			case 28:
				break;
			case 29:
				break;
			case 30:
				BaseGameDataDomain.InvalidateSelfAndInfluencedCache(30, this.DataStates, WorldDomain.CacheInfluences, context);
				return;
			case 31:
				BaseGameDataDomain.InvalidateSelfAndInfluencedCache(31, this.DataStates, WorldDomain.CacheInfluences, context);
				return;
			case 32:
				BaseGameDataDomain.InvalidateSelfAndInfluencedCache(32, this.DataStates, WorldDomain.CacheInfluences, context);
				return;
			case 33:
				break;
			case 34:
				break;
			default:
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot invalidate cache state of non-cache data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x000F6DF0 File Offset: 0x000F4FF0
		public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (operation.DataId)
			{
			case 0:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<uint>(operation, pResult, ref this._worldId);
				break;
			case 1:
				goto IL_34A;
			case 2:
				ResponseProcessor.ProcessElementList_CustomType_Fixed_Value<XiangshuAvatarTaskStatus>(operation, pResult, this._xiangshuAvatarTaskStatuses, 9, 8);
				break;
			case 3:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Array<sbyte>(operation, pResult, this._xiangshuAvatarTasksInOrder, 9);
				break;
			case 4:
				ResponseProcessor.ProcessElementList_BasicType_Fixed_Value<sbyte>(operation, pResult, this._stateTaskStatuses, 15, 1);
				break;
			case 5:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<short>(operation, pResult, ref this._mainStoryLineProgress);
				break;
			case 6:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<bool>(operation, pResult, ref this._beatRanChenZi);
				break;
			case 7:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<ulong>(operation, pResult, ref this._worldFunctionsStatuses);
				break;
			case 8:
				ResponseProcessor.ProcessSingleValueCollection_BasicType_String<int>(operation, pResult, this._customTexts);
				break;
			case 9:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<int>(operation, pResult, ref this._nextCustomTextId);
				break;
			case 10:
				ResponseProcessor.ProcessBinary(operation, pResult, this._instantNotifications);
				break;
			case 11:
				goto IL_34A;
			case 12:
				ResponseProcessor.ProcessSingleValue_CustomType_Dynamic_Ref_Single<MonthlyNotificationCollection>(operation, pResult, this._lastMonthlyNotifications);
				break;
			case 13:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<byte>(operation, pResult, ref this._worldPopulationType);
				break;
			case 14:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<byte>(operation, pResult, ref this._characterLifespanType);
				break;
			case 15:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<byte>(operation, pResult, ref this._combatDifficulty);
				break;
			case 16:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<byte>(operation, pResult, ref this._hereticsAmountType);
				break;
			case 17:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<byte>(operation, pResult, ref this._bossInvasionSpeedType);
				break;
			case 18:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<byte>(operation, pResult, ref this._worldResourceAmountType);
				break;
			case 19:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<bool>(operation, pResult, ref this._allowRandomTaiwuHeir);
				break;
			case 20:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<bool>(operation, pResult, ref this._restrictOptionsBehaviorType);
				break;
			case 21:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<sbyte>(operation, pResult, ref this._taiwuVillageStateTemplateId);
				break;
			case 22:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<sbyte>(operation, pResult, ref this._taiwuVillageLandFormType);
				break;
			case 23:
				goto IL_34A;
			case 24:
				goto IL_34A;
			case 25:
				goto IL_34A;
			case 26:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<int>(operation, pResult, ref this._worldStandardPopulation);
				break;
			case 27:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<int>(operation, pResult, ref this._currDate);
				break;
			case 28:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<sbyte>(operation, pResult, ref this._daysInCurrMonth);
				break;
			case 29:
				goto IL_34A;
			case 30:
				goto IL_34A;
			case 31:
				goto IL_34A;
			case 32:
				goto IL_34A;
			case 33:
				goto IL_34A;
			case 34:
				goto IL_34A;
			default:
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bool flag = this._pendingLoadingOperationIds != null;
			if (flag)
			{
				uint currPendingOperationId = this._pendingLoadingOperationIds.Peek();
				bool flag2 = currPendingOperationId == operation.Id;
				if (flag2)
				{
					this._pendingLoadingOperationIds.Dequeue();
					bool flag3 = this._pendingLoadingOperationIds.Count <= 0;
					if (flag3)
					{
						this._pendingLoadingOperationIds = null;
						this.InitializeInternalDataOfCollections();
						this.OnLoadedArchiveData();
						DomainManager.Global.CompleteLoading(1);
					}
				}
			}
			return;
			IL_34A:
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot process archive response of non-archive data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x000F717B File Offset: 0x000F537B
		private void InitializeInternalDataOfCollections()
		{
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x000F71D6 File Offset: 0x000F53D6
		[CompilerGenerated]
		private void <CheckMonthlyEvents>g__ResetFlag|75_0()
		{
			this._isTaiwuDying = false;
			this._isTaiwuGettingCompletelyInfected = false;
			this._isTaiwuDyingOfDystocia = false;
			this._isTaiwuVillageDestroyed = false;
			this.IsTaiwuHunterDie = false;
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x000F71FC File Offset: 0x000F53FC
		[CompilerGenerated]
		internal static void <AdvanceMonth_SectMainStory_Shaolin>g__AddChallengeMonthlyEvent|151_0(ref WorldDomain.<>c__DisplayClass151_0 A_0)
		{
			bool @bool = A_0.argBox.GetBool("ConchShip_PresetKey_ShaolinDamoTrialTriggered");
			if (@bool)
			{
				A_0.monthlyEventCollection.AddSectMainStoryShaolinChallengeCommon();
			}
			else
			{
				A_0.monthlyEventCollection.AddSectMainStoryShaolinChallenge();
			}
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x000F7238 File Offset: 0x000F5438
		[CompilerGenerated]
		internal static void <AdvanceMonth_SectMainStory_Shaolin>g__AddEndChallengeMonthlyEvent|151_1(ref WorldDomain.<>c__DisplayClass151_0 A_0)
		{
			bool @bool = A_0.argBox.GetBool("ConchShip_PresetKey_ShaolinLearnedAny");
			if (@bool)
			{
				bool bool2 = A_0.argBox.GetBool("ConchShip_PresetKey_ShaolinDamoFightTriggered");
				if (bool2)
				{
					A_0.monthlyEventCollection.AddSectMainStoryShaolinEndChallengeCommon();
				}
				else
				{
					A_0.monthlyEventCollection.AddSectMainStoryShaolinEndChallenge();
				}
			}
			else
			{
				bool bool3 = A_0.argBox.GetBool("ConchShip_PresetKey_ShaolinDamoFightTriggered");
				if (bool3)
				{
					A_0.monthlyEventCollection.AddSectMainStoryShaolinNeverLearnChallengeCommon();
				}
				else
				{
					A_0.monthlyEventCollection.AddSectMainStoryShaolinNeverLearnChallenge();
				}
			}
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x000F72BC File Offset: 0x000F54BC
		[CompilerGenerated]
		internal static bool <AdvanceMonth_SectMainStory_Wudang>g__AddSectMainStoryWudangGiftsReceived|153_0(GameData.Domains.Character.Character character, ref WorldDomain.<>c__DisplayClass153_0 A_1)
		{
			short prevFavorType = A_1.argBox.GetShort("ConchShip_PresetKey_LastEventSloppyTaoistMonkFavor");
			short currFavor = DomainManager.Character.GetFavorability(character.GetId(), A_1.taiwuCharId);
			sbyte currFavorType = FavorabilityType.GetFavorabilityType(currFavor);
			bool flag = (short)currFavorType > prevFavorType;
			bool result;
			if (flag)
			{
				A_1.monthlyEventCollection.AddSectMainStoryWudangGiftsReceived(A_1.taiwuCharId, A_1.taiwuLocation);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x000F7328 File Offset: 0x000F5528
		[CompilerGenerated]
		internal static void <AdvanceMonth_SectMainStory_Wudang>g__TriggerTreeMonthlyNotification|153_1(ref WorldDomain.<>c__DisplayClass153_0 A_0)
		{
			bool triggerTreeMonthlyNotification = A_0.triggerTreeMonthlyNotification;
			if (!triggerTreeMonthlyNotification)
			{
				List<SectStoryHeavenlyTreeExtendable> trees = DomainManager.Extra.GetAllHeavenlyTrees();
				bool flag = trees != null;
				if (flag)
				{
					foreach (SectStoryHeavenlyTreeExtendable tree in trees)
					{
						short templateId = DomainManager.Extra.GetHeavenlyTreeTemplateIdByGrowValue(tree.GrowPoint);
						bool flag2 = templateId == 677;
						if (flag2)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(tree.Id);
							bool flag3 = ItemTemplateHelper.CheckIsHeavenlyNormalTreeSeeds(12, tree.TemplateId);
							if (flag3)
							{
								DomainManager.World.GetMonthlyNotificationCollection().AddNormalTreesGrow(character.GetLocation());
							}
							else
							{
								DomainManager.World.GetMonthlyNotificationCollection().AddSectMainStoryWudangTreesGrow(character.GetLocation());
							}
						}
					}
				}
				A_0.triggerTreeMonthlyNotification = true;
			}
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x000F7424 File Offset: 0x000F5624
		[CompilerGenerated]
		private void <AdvanceMonth_SectMainStory_Shixiang>g__EnemyClear|155_0(ref WorldDomain.<>c__DisplayClass155_0 A_1)
		{
			bool flag = !this.ShixiangSettlementAffiliatedBlocksHasEnemy(A_1.context, 613);
			if (flag)
			{
				DomainManager.Extra.TriggerExtraTask(A_1.context, 34, 178);
				A_1.monthlyEventCollection.AddSectMainStoryShixiangLetterFrom2(A_1.taiwu.GetId());
				DomainManager.Extra.RemoveArgToSectMainStoryEventArgBox<bool>(A_1.context, 6, "ConchShip_PresetKey_ShixiangToFightEnemy");
			}
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x000F7494 File Offset: 0x000F5694
		[CompilerGenerated]
		private unsafe void <AdvanceMonth_SectMainStory_Shixiang>g__AddInteractWithShixiangMemberEvent|155_1(ref WorldDomain.<>c__DisplayClass155_0 A_1)
		{
			bool flag = !A_1.taiwuLocation.IsValid();
			if (!flag)
			{
				IRandomSource random = A_1.context.Random;
				IntPtr intPtr = stackalloc byte[(UIntPtr)3];
				cpblk(intPtr, ref <PrivateImplementationDetails>.AE4B3280E56E2FAF83F414A6E3DABE9D5FBE18976544C05FED121ACCB85B53FC, 3);
				Span<sbyte> span = new Span<sbyte>(intPtr, 3);
				Span<sbyte> actionOrder = span;
				MapBlockData block = DomainManager.Map.GetBlock(A_1.taiwuLocation);
				bool flag2 = block.CharacterSet == null;
				if (!flag2)
				{
					CharacterMatcherItem matcher = CharacterMatcher.DefValue.InteractWithShixiangMemberEventTarget;
					foreach (int charId in block.CharacterSet)
					{
						GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
						bool flag3 = !matcher.Match(character);
						if (!flag3)
						{
							bool flag4 = A_1.context.Random.CheckPercentProb(30);
							if (!flag4)
							{
								CollectionUtils.Shuffle<sbyte>(random, actionOrder, actionOrder.Length);
								Span<sbyte> span2 = actionOrder;
								for (int i = 0; i < span2.Length; i++)
								{
									sbyte actionType = *span2[i];
									if (!true)
									{
									}
									bool flag5;
									switch (actionType)
									{
									case 0:
										flag5 = this.<AdvanceMonth_SectMainStory_Shixiang>g__TryAddChallengeEvent|155_2(character, ref A_1);
										break;
									case 1:
										flag5 = this.<AdvanceMonth_SectMainStory_Shixiang>g__TryAddRequestBookEvent|155_3(character, ref A_1);
										break;
									case 2:
										flag5 = this.<AdvanceMonth_SectMainStory_Shixiang>g__TryAddLearnSkillEvent|155_4(character, ref A_1);
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
										break;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x000F7628 File Offset: 0x000F5828
		[CompilerGenerated]
		private bool <AdvanceMonth_SectMainStory_Shixiang>g__TryAddChallengeEvent|155_2(GameData.Domains.Character.Character shixiangMember, ref WorldDomain.<>c__DisplayClass155_0 A_2)
		{
			A_2.monthlyEventCollection.AddSectMainStoryShixiangDuel(shixiangMember.GetId(), A_2.taiwuLocation, A_2.taiwu.GetId());
			return true;
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x000F7660 File Offset: 0x000F5860
		[CompilerGenerated]
		private bool <AdvanceMonth_SectMainStory_Shixiang>g__TryAddRequestBookEvent|155_3(GameData.Domains.Character.Character shixiangMember, ref WorldDomain.<>c__DisplayClass155_0 A_2)
		{
			Inventory taiwuInventory = A_2.taiwu.GetInventory();
			List<ItemKey> itemKeys = A_2.context.AdvanceMonthRelatedData.ItemKeys.Occupy();
			foreach (KeyValuePair<ItemKey, int> keyValuePair in taiwuInventory.Items)
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
						sbyte lifeSkillType = bookCfg.LifeSkillType;
						bool flag3 = lifeSkillType <= 3;
						bool flag4 = !flag3;
						if (!flag4)
						{
							int learnedLifeSkillIndex = shixiangMember.FindLearnedLifeSkillIndex(bookCfg.LifeSkillTemplateId);
							bool flag5 = learnedLifeSkillIndex < 0;
							if (flag5)
							{
								itemKeys.Add(itemKey);
							}
						}
					}
				}
			}
			ItemKey selectedBookKey = itemKeys.GetRandomOrDefault(A_2.context.Random, ItemKey.Invalid);
			A_2.context.AdvanceMonthRelatedData.ItemKeys.Release(ref itemKeys);
			bool flag6 = !selectedBookKey.IsValid();
			bool result;
			if (flag6)
			{
				result = false;
			}
			else
			{
				A_2.monthlyEventCollection.AddSectMainStoryShixiangRequestBook(shixiangMember.GetId(), A_2.taiwuLocation, (ulong)selectedBookKey, A_2.taiwu.GetId());
				result = true;
			}
			return result;
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x000F77EC File Offset: 0x000F59EC
		[CompilerGenerated]
		private bool <AdvanceMonth_SectMainStory_Shixiang>g__TryAddLearnSkillEvent|155_4(GameData.Domains.Character.Character shixiangMember, ref WorldDomain.<>c__DisplayClass155_0 A_2)
		{
			List<GameData.Domains.Character.LifeSkillItem> taiwuLifeSkills = A_2.taiwu.GetLearnedLifeSkills();
			List<GameData.Domains.Character.LifeSkillItem> charLifeSkills = shixiangMember.GetLearnedLifeSkills();
			Inventory inventory = shixiangMember.GetInventory();
			List<ItemKey> itemKeys = A_2.context.AdvanceMonthRelatedData.ItemKeys.Occupy();
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
						sbyte lifeSkillType = bookCfg.LifeSkillType;
						bool flag3 = lifeSkillType <= 3;
						bool flag4 = !flag3;
						if (!flag4)
						{
							int charSkillIndex = shixiangMember.FindLearnedLifeSkillIndex(bookCfg.LifeSkillTemplateId);
							bool flag5 = charSkillIndex >= 0 && charLifeSkills[charSkillIndex].IsAllPagesRead();
							if (!flag5)
							{
								int taiwuSkillIndex = A_2.taiwu.FindLearnedLifeSkillIndex(bookCfg.LifeSkillTemplateId);
								bool flag6 = taiwuSkillIndex < 0 || !taiwuLifeSkills[taiwuSkillIndex].IsAllPagesRead();
								if (!flag6)
								{
									itemKeys.Add(itemKey);
								}
							}
						}
					}
				}
			}
			ItemKey selectedBookKey = itemKeys.GetRandomOrDefault(A_2.context.Random, ItemKey.Invalid);
			A_2.context.AdvanceMonthRelatedData.ItemKeys.Release(ref itemKeys);
			bool flag7 = !selectedBookKey.IsValid();
			bool result;
			if (flag7)
			{
				result = false;
			}
			else
			{
				A_2.monthlyEventCollection.AddSectMainStoryShixiangRequestLifeSkill(shixiangMember.GetId(), A_2.taiwuLocation, (ulong)selectedBookKey, A_2.taiwu.GetId());
				result = true;
			}
			return result;
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x000F79EC File Offset: 0x000F5BEC
		[CompilerGenerated]
		private void <AdvanceMonth_SectMainStory_Emei>g__EmeiWhiteGibbonAppear|156_0(bool isQuestionMark = false, ref WorldDomain.<>c__DisplayClass156_0 A_2)
		{
			GameData.Domains.Character.Character emeiWhiteGibbon = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(A_2.context, isQuestionMark ? 636 : 679);
			this.<AdvanceMonth_SectMainStory_Emei>g__CharacterAppear|156_2(emeiWhiteGibbon, ref A_2);
			DomainManager.Extra.TriggerExtraTask(A_2.context, 37, 202);
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x000F7A3C File Offset: 0x000F5C3C
		[CompilerGenerated]
		private void <AdvanceMonth_SectMainStory_Emei>g__ShiHoujiuAppear|156_1(ref WorldDomain.<>c__DisplayClass156_0 A_1)
		{
			GameData.Domains.Character.Character shiHoujiu = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(A_1.context, 637);
			this.<AdvanceMonth_SectMainStory_Emei>g__CharacterAppear|156_2(shiHoujiu, ref A_1);
			DomainManager.Extra.TriggerExtraTask(A_1.context, 37, 255);
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x000F7A84 File Offset: 0x000F5C84
		[CompilerGenerated]
		private void <AdvanceMonth_SectMainStory_Emei>g__CharacterAppear|156_2(GameData.Domains.Character.Character character, ref WorldDomain.<>c__DisplayClass156_0 A_2)
		{
			short blockId = -1;
			A_2.argBox.Get("ConchShip_PresetKey_WhiteApeBlockId", ref blockId);
			Location emeiLocation = DomainManager.Organization.GetSettlementByOrgTemplateId(2).GetLocation();
			Location location = new Location(emeiLocation.AreaId, blockId);
			Events.RaiseFixedCharacterLocationChanged(A_2.context, character.GetId(), character.GetLocation(), location);
			character.SetLocation(location, A_2.context);
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x000F7AF0 File Offset: 0x000F5CF0
		[CompilerGenerated]
		private void <AdvanceMonth_SectMainStory_Emei>g__HandleMissEmeiAdventureTwo|156_3(ref WorldDomain.<>c__DisplayClass156_0 A_1)
		{
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(A_1.context, 2, "ConchShip_PresetKey_SectMainStoryEmeiFailingEndDate", this._currDate);
			DomainManager.Extra.FinishAllTaskInChain(A_1.context, 37);
			ItemKey itemKey = A_1.taiwu.GetInventory().GetInventoryItemKey(12, 267);
			bool flag = itemKey.IsValid();
			if (flag)
			{
				A_1.taiwu.RemoveInventoryItem(A_1.context, itemKey, 1, true, false);
			}
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<bool>(A_1.context, 2, "ConchShip_PresetKey_EmeiOptionWhoIsOrthodoxVisible", false);
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<bool>(A_1.context, 2, "ConchShip_PresetKey_EmeiOptionReclusiveElderVisible", false);
			DomainManager.Extra.RemoveArgToSectMainStoryEventArgBox<int>(A_1.context, 2, "ConchShip_PresetKey_EmeiKillEachOtherStage");
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(A_1.context, 2, "ConchShip_PresetKey_EmeiPassAdventureTwoTaiwuId", taiwuId);
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x000F7BD4 File Offset: 0x000F5DD4
		[CompilerGenerated]
		private void <AdvanceMonth_SectMainStory_Baihua>g__TryTriggerBaihuaCombatTaskChain|160_0(ref WorldDomain.<>c__DisplayClass160_0 A_1)
		{
			bool tryTriggerBaihuaCombatTaskChain = A_1.tryTriggerBaihuaCombatTaskChain;
			if (!tryTriggerBaihuaCombatTaskChain)
			{
				A_1.tryTriggerBaihuaCombatTaskChain = true;
				bool flag = A_1.argBox.Contains<bool>("ConchShip_PresetKey_BaihuaDreamAboutPastLastTriggered");
				if (flag)
				{
					bool flag2 = this._currDate % 2 == 0;
					if (flag2)
					{
						bool flag3 = !A_1.argBox.Contains<bool>("ConchShip_PresetKey_BaihuaLeukoKillsMonthEventTriggered");
						if (flag3)
						{
							short settlementId = this.BaihuaSelectSettlementIdNeighborTaiwuVillage(A_1.context);
							Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
							DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<short>(A_1.context, 3, "ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementId", settlementId);
							DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<bool>(A_1.context, 3, "ConchShip_PresetKey_BaihuaLeukoKillsInteractOpen", true);
							A_1.monthlyEventCollection.AddSectMainStoryBaihuaLeukoKills(settlement.GetLocation());
						}
					}
					else
					{
						bool flag4 = !A_1.argBox.Contains<bool>("ConchShip_PresetKey_BaihuaMelanoKillsMonthEventTriggered");
						if (flag4)
						{
							short settlementId2 = this.BaihuaSelectSettlementIdNeighborTaiwuVillage(A_1.context);
							Settlement settlement2 = DomainManager.Organization.GetSettlement(settlementId2);
							DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<short>(A_1.context, 3, "ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementId", settlementId2);
							DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<bool>(A_1.context, 3, "ConchShip_PresetKey_BaihuaMelanoKillsInteractOpen", true);
							A_1.monthlyEventCollection.AddSectMainStoryBaihuaMelanoKills(settlement2.GetLocation());
						}
					}
				}
			}
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x000F7D1C File Offset: 0x000F5F1C
		[CompilerGenerated]
		private int <AdvanceMonth_SectMainStory_Baihua>g__GetAmbushProb|160_1(bool isisLeuko, ref WorldDomain.<>c__DisplayClass160_0 A_2)
		{
			int date = int.MaxValue;
			if (isisLeuko)
			{
				A_2.argBox.Get("ConchShipEventArgBoxKey_PresetKey_BaihuaLeukoKillsOptionSelectDate", ref date);
			}
			else
			{
				A_2.argBox.Get("ConchShip_PresetKey_BaihuaMelanoKillsOptionSelectDate", ref date);
			}
			return 40 + (this._currDate - date) * 10;
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x000F7D70 File Offset: 0x000F5F70
		[CompilerGenerated]
		private void <AdvanceMonth_SectMainStory_Baihua>g__TryTriggerPandemicStartTask|160_2(ref WorldDomain.<>c__DisplayClass160_0 A_1)
		{
			bool flag = A_1.argBox.Contains<bool>("ConchShip_PresetKey_BaihuaLeukoAssistedMelano") && A_1.argBox.Contains<bool>("ConchShip_PresetKey_BaihuaMelanoAssistedLeuko");
			if (flag)
			{
				DomainManager.Extra.TriggerExtraTask(A_1.context, 48, 325);
			}
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x000F7DC4 File Offset: 0x000F5FC4
		[CompilerGenerated]
		private void <AdvanceMonth_SectMainStory_Baihua>g__TryTriggerBaihuaManicLow|160_3(ref WorldDomain.<>c__DisplayClass160_0 A_1)
		{
			bool tryTriggerBaihuaManicLow = A_1.tryTriggerBaihuaManicLow;
			if (!tryTriggerBaihuaManicLow)
			{
				A_1.tryTriggerBaihuaManicLow = true;
				int date = int.MaxValue;
				bool flag = A_1.argBox.Get("ConchShip_PresetKey_BaihuaAnimalsBackDate", ref date) && this._currDate >= date + 6;
				if (flag)
				{
					bool flag2 = !A_1.argBox.Contains<int>("ConchShip_PresetKey_BaihuaManicLowDate") || (A_1.argBox.Get("ConchShip_PresetKey_BaihuaManicLowDate", ref date) && this._currDate <= date + 6);
					if (flag2)
					{
						List<short> settlementIds = this.BaihuaSelectSettlementIds(A_1.context, false, true, 3);
						List<int> members = ObjectPool<List<int>>.Instance.Get();
						CharacterSet groupCharIds = DomainManager.Taiwu.GetGroupCharIds();
						for (int i = 0; i < settlementIds.Count; i++)
						{
							short settlementId = settlementIds[i];
							Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
							A_1.monthlyNotificationCollection.AddSectMainStoryBaihuaManicLow(settlement.GetLocation());
							members.Clear();
							settlement.GetMembers().GetAllMembers(members);
							CollectionUtils.Shuffle<int>(A_1.context.Random, members);
							for (int j = members.Count - 1; j >= 0; j--)
							{
								int charId = members[j];
								GameData.Domains.Character.Character character;
								bool flag3 = DomainManager.Character.TryGetElement_Objects(charId, out character);
								if (flag3)
								{
									bool flag4 = groupCharIds.Contains(charId) || character.GetAgeGroup() < 2;
									if (flag4)
									{
										members.RemoveAt(j);
									}
								}
							}
							int debuffCount = A_1.context.Random.Next(3, 7);
							for (int k = 0; k < Math.Min(debuffCount, members.Count); k++)
							{
								int charId2 = members[k];
								GameData.Domains.Character.Character character2;
								bool flag5 = DomainManager.Character.TryGetElement_Objects(charId2, out character2);
								if (flag5)
								{
									character2.AddFeature(A_1.context, 541, false);
									A_1.lifeRecordCollection.AddSectMainStoryBaihuaManiaLow(charId2, this._currDate, character2.GetLocation());
									this.BaihuaAddCharIdToSpecialDebuffIntList(A_1.context, charId2);
								}
							}
						}
						ObjectPool<List<int>>.Instance.Return(members);
						this.<AdvanceMonth_SectMainStory_Baihua>g__TryTriggerPandemicStartTask|160_2(ref A_1);
						bool flag6 = !A_1.argBox.Contains<int>("ConchShip_PresetKey_BaihuaManicLowDate");
						if (flag6)
						{
							DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(A_1.context, 3, "ConchShip_PresetKey_BaihuaManicLowDate", this._currDate);
						}
					}
				}
			}
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x000F8054 File Offset: 0x000F6254
		[CompilerGenerated]
		private void <AdvanceMonth_SectMainStory_Baihua>g__TryTriggerBaihuaManicHigh|160_4(bool triggerTask, ref WorldDomain.<>c__DisplayClass160_0 A_2)
		{
			bool tryTriggerBaihuaManicHigh = A_2.tryTriggerBaihuaManicHigh;
			if (!tryTriggerBaihuaManicHigh)
			{
				A_2.tryTriggerBaihuaManicHigh = true;
				int date = int.MaxValue;
				bool flag = A_2.argBox.Get("ConchShip_PresetKey_BaihuaManicLowDate", ref date) && this._currDate >= date + 3;
				if (flag)
				{
					short taiwuAreaId = A_2.taiwuVillageLocation.AreaId;
					List<short> settlementIds = ObjectPool<List<short>>.Instance.Get();
					DomainManager.Map.GetAreaSettlementIds(taiwuAreaId, settlementIds, true, true);
					short settlementId = settlementIds.GetRandom(A_2.context.Random);
					CharacterSet groupCharIds = DomainManager.Taiwu.GetGroupCharIds();
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
					A_2.monthlyNotificationCollection.AddSectMainStoryBaihuaManicHigh(settlement.GetLocation());
					List<int> members = ObjectPool<List<int>>.Instance.Get();
					settlement.GetMembers().GetAllMembers(members);
					CollectionUtils.Shuffle<int>(A_2.context.Random, members);
					for (int i = members.Count - 1; i >= 0; i--)
					{
						int charId = members[i];
						GameData.Domains.Character.Character character;
						bool flag2 = DomainManager.Character.TryGetElement_Objects(charId, out character);
						if (flag2)
						{
							bool flag3 = groupCharIds.Contains(charId) || character.GetAgeGroup() < 2;
							if (flag3)
							{
								members.RemoveAt(i);
							}
						}
					}
					int debuffCount = A_2.context.Random.Next(3, 7);
					for (int j = 0; j < Math.Min(debuffCount, members.Count); j++)
					{
						int charId2 = members[j];
						GameData.Domains.Character.Character character2;
						bool flag4 = DomainManager.Character.TryGetElement_Objects(charId2, out character2);
						if (flag4)
						{
							character2.AddFeature(A_2.context, 542, false);
							A_2.lifeRecordCollection.AddSectMainStoryBaihuaManiaHigh(charId2, this._currDate, character2.GetLocation());
							this.BaihuaAddCharIdToSpecialDebuffIntList(A_2.context, charId2);
						}
					}
					bool flag5 = !A_2.argBox.Contains<int>("ConchShip_PresetKey_BaihuaManicHighDate");
					if (flag5)
					{
						DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(A_2.context, 3, "ConchShip_PresetKey_BaihuaManicHighDate", this._currDate);
					}
					ObjectPool<List<int>>.Instance.Return(members);
					DomainManager.Extra.FinishAllTaskInChain(A_2.context, 51);
					if (triggerTask)
					{
						DomainManager.Extra.TriggerExtraTask(A_2.context, 48, 326);
					}
				}
			}
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x000F82BC File Offset: 0x000F64BC
		[CompilerGenerated]
		private void <AdvanceMonth_SectMainStory_Baihua>g__TryTriggedLeukoMelanoPlay|160_5(ref WorldDomain.<>c__DisplayClass160_0 A_1)
		{
			bool tryTriggerLeukoMelanoPlay = A_1.tryTriggerLeukoMelanoPlay;
			if (!tryTriggerLeukoMelanoPlay)
			{
				A_1.tryTriggerLeukoMelanoPlay = true;
				bool flag = A_1.taiwuLocation.AreaId != A_1.taiwuVillageLocation.AreaId;
				if (!flag)
				{
					bool leukoNeedTrigger = false;
					bool melanoNeedTrigger = false;
					int count = -1;
					A_1.argBox.Get("ConchShip_PresetKey_BaihuaLeukoPlayCount", ref count);
					DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(A_1.context, 3, "ConchShip_PresetKey_BaihuaLeukoPlayCount", ++count);
					bool flag2 = count > 2;
					if (flag2)
					{
						leukoNeedTrigger = true;
					}
					count = -1;
					A_1.argBox.Get("ConchShip_PresetKey_BaihuaMelanoPlayCount", ref count);
					DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(A_1.context, 3, "ConchShip_PresetKey_BaihuaMelanoPlayCount", ++count);
					bool flag3 = count > 2;
					if (flag3)
					{
						melanoNeedTrigger = true;
					}
					bool flag4 = leukoNeedTrigger && melanoNeedTrigger;
					if (flag4)
					{
						int random = A_1.context.Random.Next(0, 2);
						bool flag5 = random == 0;
						if (flag5)
						{
							A_1.monthlyEventCollection.AddSectMainStoryBaihuaLeukoPlay();
						}
						else
						{
							A_1.monthlyEventCollection.AddSectMainStoryBaihuaMelanoPlay();
						}
					}
					else
					{
						bool flag6 = leukoNeedTrigger;
						if (flag6)
						{
							A_1.monthlyEventCollection.AddSectMainStoryBaihuaLeukoPlay();
						}
						else
						{
							bool flag7 = melanoNeedTrigger;
							if (flag7)
							{
								A_1.monthlyEventCollection.AddSectMainStoryBaihuaMelanoPlay();
							}
						}
					}
				}
			}
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x000F83F4 File Offset: 0x000F65F4
		[CompilerGenerated]
		private void <AdvanceMonth_SectMainStory_Baihua>g__TryTriggedLMPlay|160_6(ref WorldDomain.<>c__DisplayClass160_0 A_1)
		{
			bool tryTriggerLMPlay = A_1.tryTriggerLMPlay;
			if (!tryTriggerLMPlay)
			{
				A_1.tryTriggerLMPlay = true;
				bool flag = A_1.taiwuLocation.AreaId != A_1.taiwuVillageLocation.AreaId;
				if (!flag)
				{
					bool flag2 = !A_1.argBox.Contains<bool>("ConchShip_PresetKey_BaihuaLeukoAssistedMelano") || !A_1.argBox.Contains<bool>("ConchShip_PresetKey_BaihuaMelanoAssistedLeuko");
					if (!flag2)
					{
						int count = -1;
						A_1.argBox.Get("ConchShip_PresetKey_BaihuaLMPlayCount", ref count);
						DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(A_1.context, 3, "ConchShip_PresetKey_BaihuaLMPlayCount", ++count);
						bool flag3 = count >= 2;
						if (flag3)
						{
							A_1.monthlyEventCollection.AddSectMainStoryBaihuaLeukoMelanoPlay();
						}
					}
				}
			}
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x000F84B4 File Offset: 0x000F66B4
		[CompilerGenerated]
		internal static bool <AdvanceMonth_SectMainStory_Zhujian>g__InTriggerLocation|161_0(out Location location, ref WorldDomain.<>c__DisplayClass161_0 A_1)
		{
			location = Location.Invalid;
			bool isTraveling = DomainManager.Map.IsTraveling;
			bool result;
			if (isTraveling)
			{
				location = A_1.taiwu.GetValidLocation();
				result = true;
			}
			else
			{
				Location taiwuLocation = A_1.taiwu.GetLocation();
				bool flag = taiwuLocation.IsValid() && !MapAreaData.IsBrokenArea(taiwuLocation.AreaId);
				if (flag)
				{
					location = taiwuLocation;
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x000F8530 File Offset: 0x000F6730
		[CompilerGenerated]
		internal static bool <UpdateWuxianParanoiaCharacters>g__ShouldAttackRandomTarget|163_0(GameData.Domains.Character.Character character)
		{
			return character.GetFeatureIds().Contains(486);
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x000F8554 File Offset: 0x000F6754
		[CompilerGenerated]
		internal static bool <CallBaihuaMember>g__HaveAliveMember|230_0(List<int> members)
		{
			for (int i = 0; i < members.Count; i++)
			{
				GameData.Domains.Character.Character character;
				bool flag = DomainManager.Character.TryGetElement_Objects(members[i], out character);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040000D7 RID: 215
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040000D8 RID: 216
		[DomainData(DomainDataType.SingleValue, true, false, true, false)]
		private uint _worldId;

		// Token: 0x040000D9 RID: 217
		[DomainData(DomainDataType.SingleValue, false, true, true, true)]
		private sbyte _xiangshuProgress;

		// Token: 0x040000DA RID: 218
		[DomainData(DomainDataType.ElementList, true, false, true, true, ArrayElementsCount = 9)]
		private readonly XiangshuAvatarTaskStatus[] _xiangshuAvatarTaskStatuses;

		// Token: 0x040000DB RID: 219
		[DomainData(DomainDataType.SingleValue, true, false, true, true, ArrayElementsCount = 9)]
		private sbyte[] _xiangshuAvatarTasksInOrder;

		// Token: 0x040000DC RID: 220
		[DomainData(DomainDataType.ElementList, true, false, true, true, ArrayElementsCount = 15)]
		private readonly sbyte[] _stateTaskStatuses;

		// Token: 0x040000DD RID: 221
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private short _mainStoryLineProgress;

		// Token: 0x040000DE RID: 222
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private bool _beatRanChenZi;

		// Token: 0x040000DF RID: 223
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private ulong _worldFunctionsStatuses;

		// Token: 0x040000E0 RID: 224
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private readonly Dictionary<int, string> _customTexts;

		// Token: 0x040000E1 RID: 225
		[DomainData(DomainDataType.SingleValue, true, false, false, false)]
		private int _nextCustomTextId;

		// Token: 0x040000E2 RID: 226
		[DomainData(DomainDataType.SingleValue, false, true, true, true)]
		private WorldStateData _worldStateData;

		// Token: 0x040000E3 RID: 227
		private Version _currWorldGameVersion;

		// Token: 0x040000E4 RID: 228
		[DomainData(DomainDataType.Binary, true, false, true, true, CollectionCapacity = 1024)]
		private readonly InstantNotificationCollection _instantNotifications;

		// Token: 0x040000E5 RID: 229
		private int _instantNotificationsCommittedOffset;

		// Token: 0x040000E6 RID: 230
		private readonly List<short> _instantNotificationTemplateIds = new List<short>();

		// Token: 0x040000E7 RID: 231
		private readonly Dictionary<short, string> _instantNotificationTemplateId2Name = new Dictionary<short, string>();

		// Token: 0x040000E8 RID: 232
		private Type _instantNotificationCollectionType;

		// Token: 0x040000E9 RID: 233
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _onHandingMonthlyEventBlock;

		// Token: 0x040000EA RID: 234
		private MonthlyEventCollection _monthlyEventCollection;

		// Token: 0x040000EB RID: 235
		private bool _isTaiwuDying;

		// Token: 0x040000EC RID: 236
		private bool _isTaiwuGettingCompletelyInfected;

		// Token: 0x040000ED RID: 237
		private bool _isTaiwuVillageDestroyed;

		// Token: 0x040000EE RID: 238
		public bool IsTaiwuHunterDie;

		// Token: 0x040000EF RID: 239
		private bool _isTaiwuDyingOfDystocia;

		// Token: 0x040000F0 RID: 240
		private int _toRepayKindnessCharId = -1;

		// Token: 0x040000F1 RID: 241
		[TupleElementNames(new string[]
		{
			"offset",
			"score"
		})]
		private static readonly BinaryHeap<ValueTuple<int, int>> SpecialEvents = new BinaryHeap<ValueTuple<int, int>>(([TupleElementNames(new string[]
		{
			"offset",
			"score"
		})] ValueTuple<int, int> a, [TupleElementNames(new string[]
		{
			"offset",
			"score"
		})] ValueTuple<int, int> b) => a.Item2.CompareTo(b.Item2), 4);

		// Token: 0x040000F2 RID: 242
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private List<int> _sortedMonthlyNotificationSortingGroups;

		// Token: 0x040000F3 RID: 243
		[Obsolete("Use ExtraDomain._previousMonthlyNotifications instead.")]
		[DomainData(DomainDataType.SingleValue, true, false, true, false)]
		private MonthlyNotificationCollection _lastMonthlyNotifications;

		// Token: 0x040000F4 RID: 244
		private MonthlyNotificationCollection _currMonthlyNotifications = new MonthlyNotificationCollection();

		// Token: 0x040000F5 RID: 245
		private readonly List<short> _monthlyNotificationTemplateIds = new List<short>();

		// Token: 0x040000F6 RID: 246
		private readonly Dictionary<short, string> _monthlyNotificationTemplateId2Name = new Dictionary<short, string>();

		// Token: 0x040000F7 RID: 247
		private Type _monthlyNotificationCollectionType;

		// Token: 0x040000F8 RID: 248
		private readonly List<GameData.Domains.Character.Character> _candidatesCharacters = new List<GameData.Domains.Character.Character>();

		// Token: 0x040000F9 RID: 249
		private readonly List<TemplateKey> _candidateItems = new List<TemplateKey>();

		// Token: 0x040000FA RID: 250
		private readonly List<short> _candidateCombatSkills = new List<short>();

		// Token: 0x040000FB RID: 251
		private readonly List<short> _candidateSettlements = new List<short>();

		// Token: 0x040000FC RID: 252
		private readonly List<short> _candidateBuildings = new List<short>();

		// Token: 0x040000FD RID: 253
		private readonly List<short> _candidateAdventures = new List<short>();

		// Token: 0x040000FE RID: 254
		[TupleElementNames(new string[]
		{
			"colorId",
			"partId"
		})]
		private readonly List<ValueTuple<short, short>> _candidateCrickets = new List<ValueTuple<short, short>>();

		// Token: 0x040000FF RID: 255
		private readonly List<short> _candidateChickens = new List<short>();

		// Token: 0x04000100 RID: 256
		private readonly List<short> _canTestMonthlyEventTemplateIdList = new List<short>
		{
			87,
			88,
			89,
			90,
			66,
			68,
			70,
			72,
			73,
			74,
			75,
			76,
			77,
			78,
			79,
			80,
			81,
			82,
			83,
			84,
			85,
			86,
			114,
			115,
			116,
			117,
			118,
			353,
			354,
			290,
			61
		};

		// Token: 0x04000101 RID: 257
		[TupleElementNames(new string[]
		{
			"monthlyEventTemplateId",
			"selfCharId",
			"targetCharId"
		})]
		private readonly List<ValueTuple<short, int, int>> _testMonthlyEventList = new List<ValueTuple<short, int, int>>();

		// Token: 0x04000102 RID: 258
		private float _probAdjustOfCreatingCharacter;

		// Token: 0x04000103 RID: 259
		private int _sectMainStoryCombatTimesShaolin;

		// Token: 0x04000104 RID: 260
		private bool _sectMainStoryDefeatingXuannv;

		// Token: 0x04000105 RID: 261
		private bool _sectMainStoryLifeLinkUpdated;

		// Token: 0x04000106 RID: 262
		private readonly HashSet<sbyte> _storyStatus = new HashSet<sbyte>();

		// Token: 0x04000107 RID: 263
		[TupleElementNames(new string[]
		{
			"character",
			"health",
			"leftMaxHealth"
		})]
		private readonly List<ValueTuple<GameData.Domains.Character.Character, int, int>> _tmpLifeGateChars = new List<ValueTuple<GameData.Domains.Character.Character, int, int>>();

		// Token: 0x04000108 RID: 264
		[TupleElementNames(new string[]
		{
			"character",
			"distributableHealth"
		})]
		private readonly List<ValueTuple<GameData.Domains.Character.Character, int>> _tmpDeathGateChars = new List<ValueTuple<GameData.Domains.Character.Character, int>>();

		// Token: 0x04000109 RID: 265
		[TupleElementNames(new string[]
		{
			"index",
			"isLifeGate"
		})]
		private Dictionary<int, ValueTuple<int, bool>> _baihuaLinkedCharacters;

		// Token: 0x0400010A RID: 266
		private sbyte _baihuaLifeLinkNeiliType;

		// Token: 0x0400010B RID: 267
		private Dictionary<short, List<Func<bool>>> _sectMainStoryTriggerConditions = new Dictionary<short, List<Func<bool>>>
		{
			{
				1,
				new List<Func<bool>>
				{
					new Func<bool>(WorldDomain.ShaolinMainStoryTrigger0),
					new Func<bool>(WorldDomain.ShaolinMainStoryTrigger1)
				}
			},
			{
				2,
				new List<Func<bool>>
				{
					new Func<bool>(WorldDomain.EMeiMainStoryTrigger0),
					new Func<bool>(WorldDomain.EMeiMainStoryTrigger1)
				}
			},
			{
				3,
				new List<Func<bool>>
				{
					new Func<bool>(WorldDomain.BaihuaMainStoryTrigger0),
					new Func<bool>(WorldDomain.BaihuaMainStoryTrigger1)
				}
			},
			{
				4,
				new List<Func<bool>>
				{
					new Func<bool>(WorldDomain.WudangMainStoryTrigger0),
					new Func<bool>(WorldDomain.WudangMainStoryTrigger1)
				}
			},
			{
				5,
				new List<Func<bool>>
				{
					new Func<bool>(WorldDomain.YuanshanMainStoryTrigger0),
					new Func<bool>(WorldDomain.YuanshanMainStoryTrigger1),
					new Func<bool>(WorldDomain.YuanshanMainStoryTrigger2)
				}
			},
			{
				6,
				new List<Func<bool>>
				{
					new Func<bool>(WorldDomain.ShixiangMainStoryTrigger0),
					new Func<bool>(WorldDomain.ShixiangMainStoryTrigger1)
				}
			},
			{
				7,
				new List<Func<bool>>
				{
					new Func<bool>(WorldDomain.RanshanMainStoryTrigger0),
					new Func<bool>(WorldDomain.RanshanMainStoryTrigger1),
					new Func<bool>(WorldDomain.RanshanMainStoryTrigger2)
				}
			},
			{
				8,
				new List<Func<bool>>
				{
					new Func<bool>(WorldDomain.XuannvMainStoryTrigger0),
					new Func<bool>(WorldDomain.XuannvMainStoryTrigger1)
				}
			},
			{
				9,
				new List<Func<bool>>
				{
					new Func<bool>(WorldDomain.ZhujianMainStoryTrigger0),
					new Func<bool>(WorldDomain.ZhujianMainStoryTrigger1)
				}
			},
			{
				10,
				new List<Func<bool>>
				{
					new Func<bool>(WorldDomain.KongsangMainStoryTrigger0),
					new Func<bool>(WorldDomain.KongsangMainStoryTrigger1)
				}
			},
			{
				11,
				new List<Func<bool>>
				{
					new Func<bool>(WorldDomain.JingangMainStoryTrigger0),
					new Func<bool>(WorldDomain.JingangMainStoryTrigger1)
				}
			},
			{
				12,
				new List<Func<bool>>
				{
					new Func<bool>(WorldDomain.WuxianMainStoryTrigger0),
					new Func<bool>(WorldDomain.WuxianMainStoryTrigger1)
				}
			},
			{
				13,
				new List<Func<bool>>()
			},
			{
				14,
				new List<Func<bool>>
				{
					new Func<bool>(WorldDomain.FulongMainStoryTrigger0),
					new Func<bool>(WorldDomain.FulongMainStoryTrigger1)
				}
			},
			{
				15,
				new List<Func<bool>>
				{
					new Func<bool>(WorldDomain.XuehouMainStoryTrigger0)
				}
			}
		};

		// Token: 0x0400010C RID: 268
		public const sbyte ArchiveFilesBackupsCount = 3;

		// Token: 0x0400010D RID: 269
		[DomainData(DomainDataType.SingleValue, true, false, true, false)]
		private byte _worldPopulationType;

		// Token: 0x0400010E RID: 270
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private byte _characterLifespanType;

		// Token: 0x0400010F RID: 271
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private byte _combatDifficulty;

		// Token: 0x04000110 RID: 272
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private byte _hereticsAmountType;

		// Token: 0x04000111 RID: 273
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private byte _bossInvasionSpeedType;

		// Token: 0x04000112 RID: 274
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private byte _worldResourceAmountType;

		// Token: 0x04000113 RID: 275
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private bool _allowRandomTaiwuHeir;

		// Token: 0x04000114 RID: 276
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private bool _restrictOptionsBehaviorType;

		// Token: 0x04000115 RID: 277
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private sbyte _taiwuVillageStateTemplateId;

		// Token: 0x04000116 RID: 278
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private sbyte _taiwuVillageLandFormType;

		// Token: 0x04000117 RID: 279
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _hideTaiwuOriginalSurname;

		// Token: 0x04000118 RID: 280
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _allowExecute;

		// Token: 0x04000119 RID: 281
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private sbyte _archiveFilesBackupInterval;

		// Token: 0x0400011A RID: 282
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private sbyte _archiveFilesBackupCount;

		// Token: 0x0400011B RID: 283
		[DomainData(DomainDataType.SingleValue, true, false, true, false)]
		private int _worldStandardPopulation;

		// Token: 0x0400011C RID: 284
		[DomainData(DomainDataType.SingleValue, false, true, true, true)]
		private List<TaskData> _currTaskList;

		// Token: 0x0400011D RID: 285
		[DomainData(DomainDataType.SingleValue, false, true, true, true)]
		private List<TaskDisplayData> _sortedTaskList;

		// Token: 0x0400011E RID: 286
		private const int MonitorIntervalOfAdvancingMonth = 100;

		// Token: 0x0400011F RID: 287
		private const int MinAdvanceMonthTimeMilliseconds = 2000;

		// Token: 0x04000120 RID: 288
		[DomainData(DomainDataType.SingleValue, true, false, true, false)]
		private int _currDate;

		// Token: 0x04000121 RID: 289
		[Obsolete("Use ExtraDomain._actionPointCurrMonth instead.")]
		[DomainData(DomainDataType.SingleValue, true, false, true, false)]
		private sbyte _daysInCurrMonth;

		// Token: 0x04000122 RID: 290
		[DomainData(DomainDataType.SingleValue, false, false, true, false)]
		private sbyte _advancingMonthState;

		// Token: 0x04000123 RID: 291
		private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[35][];

		// Token: 0x04000124 RID: 292
		private SpinLock _spinLockXiangshuProgress = new SpinLock(false);

		// Token: 0x04000125 RID: 293
		private static readonly DataInfluence[][] CacheInfluencesXiangshuAvatarTaskStatuses = new DataInfluence[9][];

		// Token: 0x04000126 RID: 294
		private readonly byte[] _dataStatesXiangshuAvatarTaskStatuses = new byte[3];

		// Token: 0x04000127 RID: 295
		private static readonly DataInfluence[][] CacheInfluencesStateTaskStatuses = new DataInfluence[15][];

		// Token: 0x04000128 RID: 296
		private readonly byte[] _dataStatesStateTaskStatuses = new byte[4];

		// Token: 0x04000129 RID: 297
		private SingleValueCollectionModificationCollection<int> _modificationsCustomTexts = SingleValueCollectionModificationCollection<int>.Create();

		// Token: 0x0400012A RID: 298
		private BinaryModificationCollection _modificationsInstantNotifications = BinaryModificationCollection.Create();

		// Token: 0x0400012B RID: 299
		private SpinLock _spinLockCurrTaskList = new SpinLock(false);

		// Token: 0x0400012C RID: 300
		private SpinLock _spinLockSortedTaskList = new SpinLock(false);

		// Token: 0x0400012D RID: 301
		private SpinLock _spinLockWorldStateData = new SpinLock(false);

		// Token: 0x0400012E RID: 302
		private Queue<uint> _pendingLoadingOperationIds;
	}
}
