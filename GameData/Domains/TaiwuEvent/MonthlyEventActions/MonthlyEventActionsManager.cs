using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions
{
	// Token: 0x02000093 RID: 147
	[SerializableGameData(NotForDisplayModule = true)]
	public class MonthlyEventActionsManager : ISerializableGameData
	{
		// Token: 0x06001969 RID: 6505 RVA: 0x0016D81C File Offset: 0x0016BA1C
		public MonthlyActionBase GetMonthlyAction(MonthlyActionKey key)
		{
			MonthlyActionBase action;
			bool flag = !key.IsValid() || this._monthlyActions == null || !this._monthlyActions.TryGetValue(key, out action);
			MonthlyActionBase result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = action;
			}
			return result;
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x0016D85C File Offset: 0x0016BA5C
		public void RemoveTempDynamicAction(MonthlyActionKey key)
		{
			Tester.Assert(key.ActionType == 6, "");
			this._monthlyActions.Remove(key);
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x0016D880 File Offset: 0x0016BA80
		public MonthlyActionKey AddTempDynamicAction<T>(T action) where T : MonthlyActionBase, IDynamicAction
		{
			for (short i = 0; i < 32767; i += 1)
			{
				MonthlyActionKey key = new MonthlyActionKey(6, i);
				bool flag = this._monthlyActions.ContainsKey(key);
				if (!flag)
				{
					action.Key = key;
					this._monthlyActions.Add(key, action);
					return key;
				}
			}
			return MonthlyActionKey.Invalid;
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x0016D8F0 File Offset: 0x0016BAF0
		public MonthlyActionKey AddWrappedConfigAction(short templateId, short assignedAreaId = -1)
		{
			MonthlyActionKey key = new MonthlyActionKey(4, templateId);
			bool flag = MonthlyEventActionsManager.PredefinedKeys.ContainsValue(key);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unable to create monthly action because the key ");
				defaultInterpolatedStringHandler.AppendFormatted<MonthlyActionKey>(key);
				defaultInterpolatedStringHandler.AppendLiteral(" is pre-defined.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			MonthlyActionBase actionBase;
			ConfigWrapperAction wrapperAction = this._monthlyActions.TryGetValue(key, out actionBase) ? ((ConfigWrapperAction)actionBase) : new ConfigWrapperAction(key);
			wrapperAction.CreateWrappedAction(templateId, assignedAreaId);
			this._monthlyActions[key] = wrapperAction;
			return key;
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x0016D990 File Offset: 0x0016BB90
		public void ClearTaiwuBindingMonthlyActions()
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			List<MonthlyActionKey> actionKeys = new List<MonthlyActionKey>(this._monthlyActions.Keys);
			foreach (MonthlyActionKey key in actionKeys)
			{
				MonthlyActionBase action = this._monthlyActions[key];
				MarriageTriggerAction marriageTriggerAction = action as MarriageTriggerAction;
				bool flag = marriageTriggerAction != null;
				if (flag)
				{
					bool flag2 = marriageTriggerAction.Location.IsValid();
					if (flag2)
					{
						DomainManager.Adventure.RemoveAdventureSite(context, marriageTriggerAction.Location.AreaId, marriageTriggerAction.Location.BlockId, false, false);
					}
				}
			}
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x0016DA54 File Offset: 0x0016BC54
		public void CollectUnreleasedCalledCharacters(HashSet<int> calledCharacters)
		{
			bool flag = !this.IsInitialized;
			if (!flag)
			{
				foreach (KeyValuePair<MonthlyActionKey, MonthlyActionBase> keyValuePair in this._monthlyActions)
				{
					MonthlyActionKey monthlyActionKey;
					MonthlyActionBase monthlyActionBase;
					keyValuePair.Deconstruct(out monthlyActionKey, out monthlyActionBase);
					MonthlyActionBase action = monthlyActionBase;
					action.CollectCalledCharacters(calledCharacters);
				}
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600196F RID: 6511 RVA: 0x0016DAD0 File Offset: 0x0016BCD0
		public bool IsInitialized
		{
			get
			{
				return this._monthlyActions != null && this._monthlyActions.Count > 0;
			}
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x0016DAEC File Offset: 0x0016BCEC
		public void HandleMonthlyActions()
		{
			MonthlyEventActionsManager.NewlyActivated = 0;
			MonthlyEventActionsManager.NewlyTriggered = 0;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			bool flag = !this.IsInitialized;
			if (flag)
			{
				this.Init();
			}
			List<MonthlyActionBase> toExecute = this._monthlyActions.Values.ToList<MonthlyActionBase>();
			foreach (MonthlyActionBase actionItem in toExecute)
			{
				actionItem.MonthlyHandler();
			}
			stopwatch.Stop();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 3);
			defaultInterpolatedStringHandler.AppendLiteral("HandleMonthlyEventActions (");
			defaultInterpolatedStringHandler.AppendFormatted<int>(MonthlyEventActionsManager.NewlyTriggered);
			defaultInterpolatedStringHandler.AppendLiteral(" triggered, ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(MonthlyEventActionsManager.NewlyActivated);
			defaultInterpolatedStringHandler.AppendLiteral(" activated): ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(stopwatch.ElapsedMilliseconds);
			defaultInterpolatedStringHandler.AppendLiteral(" ms");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x0016DBF8 File Offset: 0x0016BDF8
		public void HandleInvalidActions()
		{
			bool flag = !this.IsInitialized;
			if (!flag)
			{
				foreach (MonthlyActionBase actionItem in this._monthlyActions.Values)
				{
					actionItem.ValidationHandler();
				}
			}
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0016DC64 File Offset: 0x0016BE64
		public void Init()
		{
			bool isInitialized = this.IsInitialized;
			if (!isInitialized)
			{
				this._monthlyActions = new Dictionary<MonthlyActionKey, MonthlyActionBase>();
				foreach (MonthlyActionsItem monthlyActionCfg in ((IEnumerable<MonthlyActionsItem>)MonthlyActions.Instance))
				{
					bool isEnemyNest = monthlyActionCfg.IsEnemyNest;
					if (!isEnemyNest)
					{
						bool flag = monthlyActionCfg.MinInterval <= 0;
						if (!flag)
						{
							ConfigMonthlyAction item = new ConfigMonthlyAction(monthlyActionCfg.TemplateId, -1);
							this._monthlyActions.Add(item.Key, item);
						}
					}
				}
				DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
				MonthlyActionKey enemyNestKey = MonthlyEventActionsManager.PredefinedKeys["EnemyNestDefault"];
				EnemyNestMonthlyAction enemyNestMonthlyAction = new EnemyNestMonthlyAction(enemyNestKey);
				enemyNestMonthlyAction.InitializeEnemyNests(context.Random);
				this._monthlyActions.Add(enemyNestKey, enemyNestMonthlyAction);
				MonthlyActionKey martialArtTournamentKey = MonthlyEventActionsManager.PredefinedKeys["MartialArtTournamentDefault"];
				this._monthlyActions.Add(martialArtTournamentKey, new MartialArtTournamentMonthlyAction
				{
					Key = martialArtTournamentKey
				});
				MonthlyActionKey brideOpenContestKey = MonthlyEventActionsManager.PredefinedKeys["BrideOpenContestDefault"];
				this._monthlyActions.Add(brideOpenContestKey, new ConfigWrapperAction(brideOpenContestKey));
				MonthlyActionKey seasonalActionKey = MonthlyEventActionsManager.PredefinedKeys["SeasonalActionDefault"];
				this._monthlyActions.Add(seasonalActionKey, new SeasonalMonthlyAction(seasonalActionKey));
				MonthlyActionKey emeiActionKey = MonthlyEventActionsManager.PredefinedKeys["EmeiStoryDefault"];
				this._monthlyActions.Add(emeiActionKey, new ConfigWrapperAction(emeiActionKey));
				MonthlyActionKey ranshanActionKey = MonthlyEventActionsManager.PredefinedKeys["RanshanStoryDefault"];
				this._monthlyActions.Add(ranshanActionKey, new ConfigWrapperAction(ranshanActionKey));
				short i = 0;
				while ((int)i < this.CustomMonthlyActionDefines.Count)
				{
					MonthlyActionKey key = new MonthlyActionKey(3, i);
					this.CustomMonthlyActionDefines[(int)i].Key = key;
					this._monthlyActions.Add(key, this.CustomMonthlyActionDefines[(int)i]);
					i += 1;
				}
			}
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x0016DE74 File Offset: 0x0016C074
		public void OnArchiveDataLoaded()
		{
			bool flag = !this.IsInitialized;
			if (!flag)
			{
				List<short> keys = MonthlyActions.Instance.GetAllKeys();
				foreach (MonthlyActionBase action in this._monthlyActions.Values)
				{
					LegendaryBookMonthlyAction legendaryBookAction = action as LegendaryBookMonthlyAction;
					bool flag2 = legendaryBookAction != null;
					if (flag2)
					{
						DomainManager.LegendaryBook.RegisterLegendaryBookMonthlyAction(legendaryBookAction);
					}
				}
				short i = 0;
				while ((int)i < keys.Count)
				{
					MonthlyActionKey key = new MonthlyActionKey(0, i);
					MonthlyActionsItem config = MonthlyActions.Instance[i];
					bool isEnemyNest = config.IsEnemyNest;
					if (!isEnemyNest)
					{
						bool flag3 = config.MinInterval <= 0;
						if (flag3)
						{
							bool flag4 = this._monthlyActions.Remove(key);
							if (flag4)
							{
								string tag = "MonthlyEventActionsManager";
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Removing invalid config action ");
								defaultInterpolatedStringHandler.AppendFormatted(config.Name);
								defaultInterpolatedStringHandler.AppendLiteral(" with key ");
								defaultInterpolatedStringHandler.AppendFormatted<MonthlyActionKey>(key);
								AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
							}
						}
						else
						{
							bool flag5 = !this._monthlyActions.ContainsKey(key);
							if (flag5)
							{
								ConfigMonthlyAction item = new ConfigMonthlyAction(keys[(int)i], -1);
								this._monthlyActions.Add(key, item);
								AdaptableLog.TagInfo("MonthlyEventActionsManager", "New Config Action: " + item.ConfigData.Name);
							}
						}
					}
					i += 1;
				}
				MonthlyActionKey brideOpenContestKey = MonthlyEventActionsManager.PredefinedKeys["BrideOpenContestDefault"];
				bool flag6 = !this._monthlyActions.ContainsKey(brideOpenContestKey);
				if (flag6)
				{
					this._monthlyActions.Add(brideOpenContestKey, new ConfigWrapperAction(brideOpenContestKey));
					string tag2 = "MonthlyEventActionsManager";
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("New Wrapper Action: ");
					defaultInterpolatedStringHandler.AppendFormatted<MonthlyActionKey>(brideOpenContestKey);
					AdaptableLog.TagInfo(tag2, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				MonthlyActionKey emeiStoryKey = MonthlyEventActionsManager.PredefinedKeys["EmeiStoryDefault"];
				bool flag7 = !this._monthlyActions.ContainsKey(emeiStoryKey);
				if (flag7)
				{
					this._monthlyActions.Add(emeiStoryKey, new ConfigWrapperAction(emeiStoryKey));
					string tag3 = "EmeiStoryDefault";
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("New Wrapper Action: ");
					defaultInterpolatedStringHandler.AppendFormatted<MonthlyActionKey>(emeiStoryKey);
					AdaptableLog.TagInfo(tag3, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				MonthlyActionKey ranshanStoryKey = MonthlyEventActionsManager.PredefinedKeys["RanshanStoryDefault"];
				bool flag8 = !this._monthlyActions.ContainsKey(ranshanStoryKey);
				if (flag8)
				{
					this._monthlyActions.Add(ranshanStoryKey, new ConfigWrapperAction(ranshanStoryKey));
					string tag4 = "RanshanStoryDefault";
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("New Wrapper Action: ");
					defaultInterpolatedStringHandler.AppendFormatted<MonthlyActionKey>(ranshanStoryKey);
					AdaptableLog.TagInfo(tag4, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				MonthlyActionKey seasonalActionKey = MonthlyEventActionsManager.PredefinedKeys["SeasonalActionDefault"];
				bool flag9 = !this._monthlyActions.ContainsKey(seasonalActionKey);
				if (flag9)
				{
					this._monthlyActions.Add(seasonalActionKey, new SeasonalMonthlyAction(seasonalActionKey));
					string tag5 = "MonthlyEventActionsManager";
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("New Seasonal Action: ");
					defaultInterpolatedStringHandler.AppendFormatted<MonthlyActionKey>(seasonalActionKey);
					AdaptableLog.TagInfo(tag5, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short j = 0;
				while ((int)j < this.CustomMonthlyActionDefines.Count)
				{
					bool flag10 = this.CustomMonthlyActionDefines[(int)j] == null;
					if (!flag10)
					{
						MonthlyActionKey key2 = new MonthlyActionKey(3, j);
						bool flag11 = this._monthlyActions.ContainsKey(key2);
						if (flag11)
						{
							this._monthlyActions[key2].InheritNonArchiveData(this.CustomMonthlyActionDefines[(int)j]);
						}
						else
						{
							MonthlyActionBase copy = this.CustomMonthlyActionDefines[(int)j].CreateCopy();
							this._monthlyActions.Add(key2, copy);
							string tag6 = "MonthlyEventActionsManager";
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
							defaultInterpolatedStringHandler.AppendLiteral("New Custom Monthly Action: ");
							defaultInterpolatedStringHandler.AppendFormatted<MonthlyActionKey>(this.CustomMonthlyActionDefines[(int)j].Key);
							AdaptableLog.TagInfo(tag6, defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
					j += 1;
				}
			}
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x0016E2D0 File Offset: 0x0016C4D0
		public static void OnConfigActionBecomeInvalid(short templateId, ConfigMonthlyAction actionItem)
		{
			Action<ConfigMonthlyAction> func;
			bool flag = MonthlyEventActionsManager.ConfigBecomeInvalidHandlerMap.TryGetValue(templateId, out func);
			if (flag)
			{
				func(actionItem);
			}
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x0016E2F8 File Offset: 0x0016C4F8
		public static bool ConfigItemTriggerCheck(short templateId, ConfigMonthlyAction actionItem)
		{
			Func<ConfigMonthlyAction, bool> func;
			bool flag = MonthlyEventActionsManager.ConfigTriggerCheckersExtensionMap.TryGetValue(templateId, out func);
			return !flag || func(actionItem);
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x0016E326 File Offset: 0x0016C526
		[Obsolete]
		public static void HandleConfigItemMonthlyNotification(short templateId, ConfigMonthlyAction actionItem)
		{
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x0016E32C File Offset: 0x0016C52C
		public static void ConfigItemOnActivate(short templateId, ConfigMonthlyAction monthlyAction)
		{
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection.AddConfigMonthlyActionNotification(monthlyAction.ConfigData, monthlyAction);
			Action<ConfigMonthlyAction> handler;
			bool flag = MonthlyEventActionsManager.ConfigMonthlyNotificationHandlerMap.TryGetValue(templateId, out handler);
			if (flag)
			{
				handler(monthlyAction);
			}
			if (templateId == 81)
			{
				DomainManager.Extra.TriggerExtraTask(DomainManager.TaiwuEvent.MainThreadDataContext, 37, 204);
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(DomainManager.TaiwuEvent.MainThreadDataContext, 2, "ConchShip_PresetKey_EmeiAdventureTwoAppearDate", DomainManager.World.GetCurrDate());
			}
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x0016E3BC File Offset: 0x0016C5BC
		public static void ConfigItemOnDeactivate(short templateId, ConfigMonthlyAction monthlyAction, bool isComplete)
		{
			bool flag = monthlyAction.Key.Equals(MonthlyEventActionsManager.PredefinedKeys["BrideOpenContestDefault"]);
			if (flag)
			{
				MonthlyEventActionsManager.RemoveVeil(monthlyAction, isComplete);
			}
			Action<ConfigMonthlyAction, bool> handler;
			bool flag2 = MonthlyEventActionsManager.ConfigDeactivateExtensionMap.TryGetValue(templateId, out handler);
			if (flag2)
			{
				handler(monthlyAction, isComplete);
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Deactivating Config Monthly Action: ");
			defaultInterpolatedStringHandler.AppendFormatted(monthlyAction.ConfigData.Name);
			defaultInterpolatedStringHandler.AppendLiteral(" at [");
			defaultInterpolatedStringHandler.AppendFormatted<Location>(monthlyAction.Location);
			defaultInterpolatedStringHandler.AppendLiteral("] where isComplete = ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(isComplete);
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x0016E474 File Offset: 0x0016C674
		public static bool CheckConfigActionBecomeInvalid(short templateId, ConfigMonthlyAction monthlyAction)
		{
			Predicate<ConfigMonthlyAction> predicate;
			return MonthlyEventActionsManager.ConfigIsValidPredicateMap.TryGetValue(templateId, out predicate) && predicate(monthlyAction);
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x0016E4A0 File Offset: 0x0016C6A0
		private static bool MajorCharacterIsNoLongerValid(ConfigMonthlyAction monthlyAction)
		{
			bool flag = monthlyAction.MajorCharacterSets == null || monthlyAction.MajorCharacterSets.Count == 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				sbyte curStateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(monthlyAction.Location.AreaId);
				result = !CallCharacterHelper.CheckCalledCharactersStillValid(monthlyAction.ConfigData.MajorTargetFilterList, monthlyAction.MajorCharacterSets, curStateTemplateId);
			}
			return result;
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x0016E504 File Offset: 0x0016C704
		private static void KillMajorCharacter(ConfigMonthlyAction monthlyAction, bool isComplete)
		{
			if (!isComplete)
			{
				bool flag = monthlyAction.MajorCharacterSets == null || monthlyAction.MajorCharacterSets.Count == 0;
				if (!flag)
				{
					short adventureId = monthlyAction.ConfigData.AdventureId;
					foreach (int charId in monthlyAction.MajorCharacterSets[0].GetCollection())
					{
						GameData.Domains.Character.Character character;
						bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
						if (!flag2)
						{
							DomainManager.Character.MakeCharacterDead(DomainManager.TaiwuEvent.MainThreadDataContext, character, 5, new CharacterDeathInfo(character.GetValidLocation())
							{
								AdventureId = adventureId
							});
						}
					}
				}
			}
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x0016E5E8 File Offset: 0x0016C7E8
		private static void RemoveAdventureOnActionBecomeInvalid(ConfigMonthlyAction monthlyAction)
		{
			bool isEnemyNest = monthlyAction.ConfigData.IsEnemyNest;
			if (isEnemyNest)
			{
				MonthlyEventActionsManager.KillMajorCharacter(monthlyAction, false);
			}
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			bool flag = monthlyAction.ConfigData.AdventureId < 0 || !monthlyAction.Location.IsValid();
			if (flag)
			{
				monthlyAction.Deactivate(false);
			}
			else
			{
				DomainManager.Adventure.RemoveAdventureSite(context, monthlyAction.Location.AreaId, monthlyAction.Location.BlockId, true, false);
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Removing invalid adventure ");
			defaultInterpolatedStringHandler.AppendFormatted<AdventureItem>(Adventure.Instance[monthlyAction.ConfigData.AdventureId]);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendLiteral("created by action ");
			defaultInterpolatedStringHandler.AppendFormatted<MonthlyActionKey>(monthlyAction.Key);
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted(monthlyAction.ConfigData.Name);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x0016E700 File Offset: 0x0016C900
		private static void RemoveVeil(ConfigMonthlyAction monthlyAction, bool isComplete)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			bool flag = !taiwuChar.GetAvatar().ShowVeil;
			if (!flag)
			{
				AvatarData avatarData = taiwuChar.GetAvatar();
				avatarData.ShowVeil = false;
				taiwuChar.SetAvatar(avatarData, DomainManager.TaiwuEvent.MainThreadDataContext);
			}
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x0016E750 File Offset: 0x0016C950
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x0016E764 File Offset: 0x0016C964
		public int GetSerializedSize()
		{
			int totalSize = 0;
			bool flag = this._monthlyActions != null;
			if (flag)
			{
				totalSize += 2;
				int elementsCount = this._monthlyActions.Count;
				totalSize += elementsCount * 3;
				foreach (MonthlyActionBase val in this._monthlyActions.Values)
				{
					totalSize += val.GetSerializedSize();
				}
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x0016E808 File Offset: 0x0016CA08
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData;
			bool flag = this._monthlyActions != null;
			if (flag)
			{
				int elementsCount = this._monthlyActions.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				foreach (KeyValuePair<MonthlyActionKey, MonthlyActionBase> pair in this._monthlyActions)
				{
					pCurrData += pair.Key.Serialize(pCurrData);
					pCurrData += pair.Value.Serialize(pCurrData);
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x0016E8EC File Offset: 0x0016CAEC
		public unsafe int Deserialize(byte* pData)
		{
			ushort elementsCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this._monthlyActions == null;
				if (flag2)
				{
					this._monthlyActions = new Dictionary<MonthlyActionKey, MonthlyActionBase>((int)elementsCount);
				}
				else
				{
					this._monthlyActions.Clear();
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					MonthlyActionKey key = default(MonthlyActionKey);
					pCurrData += key.Deserialize(pCurrData);
					switch (key.ActionType)
					{
					case 0:
					{
						ConfigMonthlyAction configAction = new ConfigMonthlyAction();
						pCurrData += configAction.Deserialize(pCurrData);
						this._monthlyActions.Add(key, configAction);
						break;
					}
					case 1:
					{
						EnemyNestMonthlyAction enemyNestAction = new EnemyNestMonthlyAction();
						pCurrData += enemyNestAction.Deserialize(pCurrData);
						this._monthlyActions.Add(key, enemyNestAction);
						break;
					}
					case 2:
					{
						MartialArtTournamentMonthlyAction martialArtTournamentMonthlyAction = new MartialArtTournamentMonthlyAction();
						pCurrData += martialArtTournamentMonthlyAction.Deserialize(pCurrData);
						this._monthlyActions.Add(key, martialArtTournamentMonthlyAction);
						break;
					}
					case 3:
					{
						MonthlyActionBase customAction = this.CustomMonthlyActionDefines[(int)key.Index].CreateCopy();
						pCurrData += customAction.Deserialize(pCurrData);
						this._monthlyActions.Add(key, customAction);
						break;
					}
					case 4:
					{
						ConfigWrapperAction wrapperAction = new ConfigWrapperAction();
						pCurrData += wrapperAction.Deserialize(pCurrData);
						this._monthlyActions.Add(key, wrapperAction);
						break;
					}
					case 5:
					{
						SeasonalMonthlyAction seasonalAction = new SeasonalMonthlyAction();
						pCurrData += seasonalAction.Deserialize(pCurrData);
						this._monthlyActions.Add(key, seasonalAction);
						break;
					}
					case 6:
					{
						short dynamicActionType = *(short*)pCurrData;
						MonthlyActionBase dynamicAction = DynamicActionType.CreateDynamicAction(dynamicActionType);
						pCurrData += dynamicAction.Deserialize(pCurrData);
						this._monthlyActions.Add(key, dynamicAction);
						break;
					}
					default:
						throw new Exception("Unrecognized type name.");
					}
				}
			}
			else
			{
				Dictionary<MonthlyActionKey, MonthlyActionBase> monthlyActions = this._monthlyActions;
				if (monthlyActions != null)
				{
					monthlyActions.Clear();
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x040005B2 RID: 1458
		[SerializableGameDataField]
		private Dictionary<MonthlyActionKey, MonthlyActionBase> _monthlyActions;

		// Token: 0x040005B3 RID: 1459
		public static readonly Dictionary<string, MonthlyActionKey> PredefinedKeys = new Dictionary<string, MonthlyActionKey>
		{
			{
				"EnemyNestDefault",
				new MonthlyActionKey(1, 0)
			},
			{
				"MartialArtTournamentDefault",
				new MonthlyActionKey(2, 0)
			},
			{
				"BrideOpenContestDefault",
				new MonthlyActionKey(4, 0)
			},
			{
				"SeasonalActionDefault",
				new MonthlyActionKey(5, 0)
			},
			{
				"EmeiStoryDefault",
				new MonthlyActionKey(4, 1)
			},
			{
				"RanshanStoryDefault",
				new MonthlyActionKey(4, 2)
			}
		};

		// Token: 0x040005B4 RID: 1460
		public readonly List<MonthlyActionBase> CustomMonthlyActionDefines = new List<MonthlyActionBase>();

		// Token: 0x040005B5 RID: 1461
		public static readonly Dictionary<short, Func<ConfigMonthlyAction, bool>> ConfigTriggerCheckersExtensionMap = new Dictionary<short, Func<ConfigMonthlyAction, bool>>
		{
			{
				30,
				(ConfigMonthlyAction action) => DomainManager.World.GetWorldFunctionsStatus(25)
			}
		};

		// Token: 0x040005B6 RID: 1462
		public static readonly Dictionary<short, Action<ConfigMonthlyAction>> ConfigMonthlyNotificationHandlerMap = new Dictionary<short, Action<ConfigMonthlyAction>>();

		// Token: 0x040005B7 RID: 1463
		public static readonly Dictionary<short, Action<ConfigMonthlyAction, bool>> ConfigDeactivateExtensionMap = new Dictionary<short, Action<ConfigMonthlyAction, bool>>
		{
			{
				23,
				new Action<ConfigMonthlyAction, bool>(MonthlyEventActionsManager.KillMajorCharacter)
			},
			{
				24,
				new Action<ConfigMonthlyAction, bool>(MonthlyEventActionsManager.KillMajorCharacter)
			},
			{
				25,
				new Action<ConfigMonthlyAction, bool>(MonthlyEventActionsManager.KillMajorCharacter)
			},
			{
				26,
				new Action<ConfigMonthlyAction, bool>(MonthlyEventActionsManager.KillMajorCharacter)
			}
		};

		// Token: 0x040005B8 RID: 1464
		public static readonly Dictionary<short, Predicate<ConfigMonthlyAction>> ConfigIsValidPredicateMap = new Dictionary<short, Predicate<ConfigMonthlyAction>>
		{
			{
				23,
				new Predicate<ConfigMonthlyAction>(MonthlyEventActionsManager.MajorCharacterIsNoLongerValid)
			},
			{
				24,
				new Predicate<ConfigMonthlyAction>(MonthlyEventActionsManager.MajorCharacterIsNoLongerValid)
			},
			{
				25,
				new Predicate<ConfigMonthlyAction>(MonthlyEventActionsManager.MajorCharacterIsNoLongerValid)
			},
			{
				26,
				new Predicate<ConfigMonthlyAction>(MonthlyEventActionsManager.MajorCharacterIsNoLongerValid)
			}
		};

		// Token: 0x040005B9 RID: 1465
		public static readonly Dictionary<short, Action<ConfigMonthlyAction>> ConfigBecomeInvalidHandlerMap = new Dictionary<short, Action<ConfigMonthlyAction>>
		{
			{
				23,
				new Action<ConfigMonthlyAction>(MonthlyEventActionsManager.RemoveAdventureOnActionBecomeInvalid)
			},
			{
				24,
				new Action<ConfigMonthlyAction>(MonthlyEventActionsManager.RemoveAdventureOnActionBecomeInvalid)
			},
			{
				25,
				new Action<ConfigMonthlyAction>(MonthlyEventActionsManager.RemoveAdventureOnActionBecomeInvalid)
			},
			{
				26,
				new Action<ConfigMonthlyAction>(MonthlyEventActionsManager.RemoveAdventureOnActionBecomeInvalid)
			}
		};

		// Token: 0x040005BA RID: 1466
		public static int NewlyActivated;

		// Token: 0x040005BB RID: 1467
		public static int NewlyTriggered;
	}
}
