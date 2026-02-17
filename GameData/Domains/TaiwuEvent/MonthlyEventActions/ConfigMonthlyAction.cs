using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using Config.ConfigCells.Character;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.LifeRecord;
using GameData.Domains.LifeRecord.GeneralRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Domains.TaiwuEvent.EventManager;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions
{
	// Token: 0x0200008B RID: 139
	[SerializableGameData(NotForDisplayModule = true)]
	public class ConfigMonthlyAction : MonthlyActionBase, ISerializableGameData, IRecordArgumentSource
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060018F7 RID: 6391 RVA: 0x00168F3C File Offset: 0x0016713C
		public MonthlyActionsItem ConfigData
		{
			get
			{
				return MonthlyActions.Instance[this.ConfigTemplateId];
			}
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x00168F50 File Offset: 0x00167150
		public ConfigMonthlyAction(short templateId, short assignedAreaId = -1)
		{
			this.Key = new MonthlyActionKey(0, templateId);
			this.ConfigTemplateId = templateId;
			this.State = 0;
			this.MajorCharacterSets = new List<CharacterSet>();
			this.ParticipatingCharacterSets = new List<CharacterSet>();
			this.Location = Location.Invalid;
			this._assignedAreaId = assignedAreaId;
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x00168FA8 File Offset: 0x001671A8
		public ConfigMonthlyAction(MonthlyActionKey key, short templateId, short assignedAreaId = -1)
		{
			this.Key = key;
			this.ConfigTemplateId = templateId;
			this.State = 0;
			this.MajorCharacterSets = new List<CharacterSet>();
			this.ParticipatingCharacterSets = new List<CharacterSet>();
			this.Location = Location.Invalid;
			this._assignedAreaId = assignedAreaId;
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x00168FFC File Offset: 0x001671FC
		public override bool IsMonthMatch()
		{
			return this.ConfigData.EnterMonthList.Count == 0 || this.ConfigData.EnterMonthList.Contains(DomainManager.World.GetCurrMonthInYear());
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x00169040 File Offset: 0x00167240
		public override void TriggerAction()
		{
			this.Month = 0;
			this.ClearCalledCharacters();
			bool flag = this.Location.IsValid();
			if (flag)
			{
				bool flag2 = DomainManager.Adventure.TryCreateAdventureSite(DomainManager.TaiwuEvent.MainThreadDataContext, this.Location.AreaId, this.Location.BlockId, this.ConfigData.AdventureId, this.Key);
				if (flag2)
				{
					MonthlyEventActionsManager.NewlyTriggered++;
					this.State = 1;
				}
				else
				{
					this.Location = Location.Invalid;
				}
			}
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x001690D0 File Offset: 0x001672D0
		public override void MonthlyHandler()
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			bool flag = Adventure.Instance.GetItem(this.ConfigData.AdventureId) == null;
			if (!flag)
			{
				bool flag2 = DomainManager.World.GetMainStoryLineProgress() < 3;
				if (!flag2)
				{
					bool flag3 = this.ConfigData.MinInterval <= 0 && this.State == 0;
					if (!flag3)
					{
						bool flag4 = this.State == 0 && this.IsMonthMatch() && MonthlyEventActionsManager.ConfigItemTriggerCheck(this.ConfigTemplateId, this);
						if (flag4)
						{
							bool flag5 = this.LastFinishDate + (int)this.ConfigData.MinInterval >= DomainManager.World.GetCurrDate();
							if (flag5)
							{
								return;
							}
							this.SelectLocation();
							this.TriggerAction();
						}
						bool timeIsUp = this.ConfigData.PreparationDuration > 0 && this.Month >= (int)(this.ConfigData.PreparationDuration - this.ConfigData.PreannouncingTime);
						bool flag6 = this.State == 1;
						if (flag6)
						{
							this.CallMajorCharacters();
							this.CallParticipateCharacters();
						}
						bool flag7 = this.State == 2;
						if (flag7)
						{
							this.CallParticipateCharacters();
						}
						sbyte state = this.State;
						bool flag8 = state - 2 <= 2;
						bool flag9 = flag8;
						if (flag9)
						{
							bool flag10 = (timeIsUp || this.ConfigData.CanActionBeforehand) && (this.ConfigData.AllowTemporaryMajorCharacter || CallCharacterHelper.IsAllCharactersAtLocation(this.Location, this.ConfigData.MajorTargetFilterList, this.MajorCharacterSets)) && (this.ConfigData.AllowTemporaryParticipateCharacter || CallCharacterHelper.IsAllCharactersAtLocation(this.Location, this.ConfigData.ParticipateTargetFilterList, this.ParticipatingCharacterSets));
							if (flag10)
							{
								bool flag11 = this.State != 4;
								if (flag11)
								{
									bool flag12 = this.ConfigData.PreannouncingTime <= 0;
									if (flag12)
									{
										this.State = 5;
										this.Activate();
									}
									else
									{
										this.State = 4;
										this._announceMonth = 0;
										MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
										monthlyNotificationCollection.AddConfigMonthlyActionAnnouncementNotification(this.ConfigData, this, (int)this.ConfigData.PreannouncingTime);
									}
								}
								else
								{
									this._announceMonth += 1;
									bool flag13 = this._announceMonth >= this.ConfigData.PreannouncingTime;
									if (flag13)
									{
										this.State = 5;
										this.Activate();
									}
									else
									{
										MonthlyNotificationCollection monthlyNotificationCollection2 = DomainManager.World.GetMonthlyNotificationCollection();
										monthlyNotificationCollection2.AddConfigMonthlyActionAnnouncementNotification(this.ConfigData, this, (int)(this.ConfigData.PreannouncingTime - this._announceMonth));
									}
								}
							}
						}
						bool flag14 = this.State > 0 && this.State <= 3 && timeIsUp;
						if (flag14)
						{
							bool flag15 = this.ConfigData.AdventureId >= 0 && this.Location.IsValid();
							if (flag15)
							{
								DomainManager.Adventure.RemoveAdventureSite(context, this.Location.AreaId, this.Location.BlockId, true, false);
							}
							else
							{
								this.Deactivate(false);
							}
						}
						this.ValidationHandler();
						bool flag16 = this.State != 0;
						if (flag16)
						{
							this.Month++;
						}
					}
				}
			}
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x00169428 File Offset: 0x00167628
		public override void Activate()
		{
			bool flag = this.ConfigData.AdventureId >= 0;
			if (flag)
			{
				DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
				MonthlyEventActionsManager.NewlyActivated++;
				DomainManager.Adventure.ActivateAdventureSite(context, this.Location.AreaId, this.Location.BlockId);
			}
			MonthlyEventActionsManager.ConfigItemOnActivate(this.ConfigTemplateId, this);
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x00169494 File Offset: 0x00167694
		public override void Deactivate(bool isComplete)
		{
			MonthlyEventActionsManager.ConfigItemOnDeactivate(this.ConfigTemplateId, this, isComplete);
			this.State = 0;
			this.Month = 0;
			this.LastFinishDate = DomainManager.World.GetCurrDate();
			this.Location = Location.Invalid;
			this.ClearCalledCharacters();
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x001694E0 File Offset: 0x001676E0
		public override MonthlyActionBase CreateCopy()
		{
			return Serializer.CreateCopy<ConfigMonthlyAction>(this);
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x001694F8 File Offset: 0x001676F8
		public void ClearCalledCharacters()
		{
			CallCharacterHelper.ClearCalledCharacters(this.MajorCharacterSets, !this.ConfigData.MajorTargetMoveVisible, true);
			CallCharacterHelper.ClearCalledCharacters(this.ParticipatingCharacterSets, false, true);
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x00169524 File Offset: 0x00167724
		public override void EnsurePrerequisites()
		{
			sbyte curStateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(this.Location.AreaId);
			int removedMajorCharCount = CallCharacterHelper.RemoveInvalidCharacters(this.ConfigData.MajorTargetFilterList, this.MajorCharacterSets, this.Location, curStateTemplateId);
			bool flag = removedMajorCharCount > 0;
			if (flag)
			{
				string tag = "ConfigMonthlyAction";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 4);
				defaultInterpolatedStringHandler.AppendFormatted(this.ConfigData.Name);
				defaultInterpolatedStringHandler.AppendFormatted<MonthlyActionKey>(this.Key);
				defaultInterpolatedStringHandler.AppendLiteral(" at ");
				defaultInterpolatedStringHandler.AppendFormatted<Location>(this.Location);
				defaultInterpolatedStringHandler.AppendLiteral(" removed ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(removedMajorCharCount);
				defaultInterpolatedStringHandler.AppendLiteral(" major characters that are invalid");
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
			}
			int removedParticipateCharCount = CallCharacterHelper.RemoveInvalidCharacters(this.ConfigData.ParticipateTargetFilterList, this.ParticipatingCharacterSets, this.Location, curStateTemplateId);
			bool flag2 = removedParticipateCharCount > 0;
			if (flag2)
			{
				string tag2 = "ConfigMonthlyAction";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 4);
				defaultInterpolatedStringHandler.AppendFormatted(this.ConfigData.Name);
				defaultInterpolatedStringHandler.AppendFormatted<MonthlyActionKey>(this.Key);
				defaultInterpolatedStringHandler.AppendLiteral(" at ");
				defaultInterpolatedStringHandler.AppendFormatted<Location>(this.Location);
				defaultInterpolatedStringHandler.AppendLiteral(" removed ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(removedParticipateCharCount);
				defaultInterpolatedStringHandler.AppendLiteral(" participate characters that are invalid");
				AdaptableLog.TagWarning(tag2, defaultInterpolatedStringHandler.ToStringAndClear(), false);
			}
			bool flag3 = !this.ConfigData.AllowTemporaryMajorCharacter && !CallCharacterHelper.IsAllCharactersAtLocation(this.Location, this.ConfigData.MajorTargetFilterList, this.MajorCharacterSets);
			if (flag3)
			{
				this.CallMajorCharacters();
			}
			bool flag4 = !this.ConfigData.AllowTemporaryParticipateCharacter && !CallCharacterHelper.IsAllCharactersAtLocation(this.Location, this.ConfigData.ParticipateTargetFilterList, this.ParticipatingCharacterSets);
			if (flag4)
			{
				this.CallParticipateCharacters();
			}
			this.State = 5;
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x00169710 File Offset: 0x00167910
		public override void ValidationHandler()
		{
			bool flag = this.State != 5;
			if (!flag)
			{
				bool flag2 = this.Location.IsValid() && DomainManager.Adventure.GetAdventureSiteState(this.Location.AreaId, this.Location.BlockId) != 1;
				if (!flag2)
				{
					bool flag3 = !this.CheckValid() || MonthlyEventActionsManager.CheckConfigActionBecomeInvalid(this.ConfigTemplateId, this);
					if (flag3)
					{
						MonthlyEventActionsManager.OnConfigActionBecomeInvalid(this.ConfigTemplateId, this);
					}
				}
			}
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x00169794 File Offset: 0x00167994
		private bool CheckValid()
		{
			bool flag = !this.ConfigData.AllowTemporaryMajorCharacter && !CallCharacterHelper.IsAllCharactersAtLocation(this.Location, this.ConfigData.MajorTargetFilterList, this.MajorCharacterSets);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !this.ConfigData.AllowTemporaryParticipateCharacter && !CallCharacterHelper.IsAllCharactersAtLocation(this.Location, this.ConfigData.ParticipateTargetFilterList, this.ParticipatingCharacterSets);
				result = !flag2;
			}
			return result;
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x00169818 File Offset: 0x00167A18
		public override void FillEventArgBox(EventArgBox eventArgBox)
		{
			AdaptableLog.Info("Adding major characters to adventure.");
			for (int majorCharSetIndex = 0; majorCharSetIndex < this.ConfigData.MajorTargetFilterList.Length; majorCharSetIndex++)
			{
				CharacterFilterRequirement filterReq = this.ConfigData.MajorTargetFilterList[majorCharSetIndex];
				CharacterSet majorCharSet = (majorCharSetIndex < this.MajorCharacterSets.Count) ? this.MajorCharacterSets[majorCharSetIndex] : default(CharacterSet);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
				defaultInterpolatedStringHandler.AppendLiteral("MajorCharacter_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(majorCharSetIndex);
				this.FillIntelligentCharactersToArgBox(eventArgBox, defaultInterpolatedStringHandler.ToStringAndClear(), majorCharSet, filterReq, this.ConfigData.AllowTemporaryMajorCharacter);
				AdventureCharacterSortUtils.Sort(eventArgBox, true, majorCharSetIndex, CharacterSortType.CombatPower, false);
			}
			AdaptableLog.Info("Adding participating characters to adventure.");
			for (int participateCharSetIndex = 0; participateCharSetIndex < this.ConfigData.ParticipateTargetFilterList.Length; participateCharSetIndex++)
			{
				CharacterFilterRequirement filterReq2 = this.ConfigData.ParticipateTargetFilterList[participateCharSetIndex];
				CharacterSet participateCharSet = (participateCharSetIndex < this.ParticipatingCharacterSets.Count) ? this.ParticipatingCharacterSets[participateCharSetIndex] : default(CharacterSet);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("ParticipateCharacter_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(participateCharSetIndex);
				this.FillIntelligentCharactersToArgBox(eventArgBox, defaultInterpolatedStringHandler.ToStringAndClear(), participateCharSet, filterReq2, this.ConfigData.AllowTemporaryParticipateCharacter);
				AdventureCharacterSortUtils.Sort(eventArgBox, false, participateCharSetIndex, CharacterSortType.CombatPower, false);
			}
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x0016998C File Offset: 0x00167B8C
		public override void CollectCalledCharacters(HashSet<int> calledCharacters)
		{
			bool flag = this.ConfigData.MinInterval <= 0 && this.Key.ActionType == 0;
			if (flag)
			{
				this.MajorCharacterSets.ForEach(delegate(CharacterSet charSet)
				{
					charSet.Clear();
				});
				this.ParticipatingCharacterSets.ForEach(delegate(CharacterSet charSet)
				{
					charSet.Clear();
				});
				this.MajorCharacterSets.Clear();
				this.ParticipatingCharacterSets.Clear();
			}
			else
			{
				foreach (CharacterSet charSet3 in this.MajorCharacterSets)
				{
					calledCharacters.UnionWith(charSet3.GetCollection());
				}
				foreach (CharacterSet charSet2 in this.ParticipatingCharacterSets)
				{
					calledCharacters.UnionWith(charSet2.GetCollection());
				}
			}
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x00169ACC File Offset: 0x00167CCC
		private void FillIntelligentCharactersToArgBox(EventArgBox eventArgBox, string keyPrefix, CharacterSet charSet, CharacterFilterRequirement filterReq, bool allowTempChars)
		{
			int amountAdded = 0;
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			foreach (int charId in charSet.GetCollection())
			{
				GameData.Domains.Character.Character character;
				bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (!flag)
				{
					bool flag2 = character.IsCrossAreaTraveling();
					if (flag2)
					{
						DomainManager.Character.GroupMove(context, character, this.Location);
					}
					bool flag3 = !character.GetLocation().Equals(this.Location);
					if (!flag3)
					{
						bool flag4 = filterReq.HasMaximum() && amountAdded >= filterReq.MaxCharactersRequired;
						if (flag4)
						{
							Events.RaiseCharacterLocationChanged(context, charId, character.GetLocation(), this.Location);
							character.SetLocation(this.Location, context);
						}
						else
						{
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
							defaultInterpolatedStringHandler.AppendFormatted(keyPrefix);
							defaultInterpolatedStringHandler.AppendLiteral("_");
							defaultInterpolatedStringHandler.AppendFormatted<int>(amountAdded);
							eventArgBox.Set(defaultInterpolatedStringHandler.ToStringAndClear(), charId);
							amountAdded++;
						}
					}
				}
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Adding ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(amountAdded);
			defaultInterpolatedStringHandler.AppendLiteral(" real characters to adventure.");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			if (allowTempChars)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
				defaultInterpolatedStringHandler.AppendLiteral("creating ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(Math.Max(0, filterReq.MinCharactersRequired - amountAdded));
				defaultInterpolatedStringHandler.AppendLiteral(" temporary characters.");
				AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				while (amountAdded < filterReq.MinCharactersRequired)
				{
					short ruleId = filterReq.CharacterFilterRuleIds[context.Random.Next(0, filterReq.CharacterFilterRuleIds.Length)];
					GameData.Domains.Character.Character character2 = DomainManager.Character.CreateTemporaryIntelligentCharacter(context, ruleId, this.Location);
					int charId2 = character2.GetId();
					DomainManager.Adventure.AddTemporaryIntelligentCharacter(charId2);
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted(keyPrefix);
					defaultInterpolatedStringHandler.AppendLiteral("_");
					defaultInterpolatedStringHandler.AppendFormatted<int>(amountAdded);
					eventArgBox.Set(defaultInterpolatedStringHandler.ToStringAndClear(), charId2);
					amountAdded++;
				}
			}
			eventArgBox.Set(keyPrefix + "_Count", amountAdded);
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x00169D54 File Offset: 0x00167F54
		private void CallMajorCharacters()
		{
			bool flag = !this.Location.IsValid();
			if (!flag)
			{
				bool flag2 = !CallCharacterHelper.CallCharacters(this.Location, this.ConfigData.CharacterSearchRange, this.ConfigData.MajorTargetFilterList, this.MajorCharacterSets, this.ConfigData.AllowTemporaryMajorCharacter, true, !this.ConfigData.MajorTargetMoveVisible, new Action<DataContext, GameData.Domains.Character.Character>(this.OnMajorCharacterCalled));
				if (!flag2)
				{
					this.State = 2;
				}
			}
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x00169DD4 File Offset: 0x00167FD4
		private void CallParticipateCharacters()
		{
			bool flag = !this.Location.IsValid();
			if (!flag)
			{
				bool flag2 = !CallCharacterHelper.CallCharacters(this.Location, this.ConfigData.CharacterSearchRange, this.ConfigData.ParticipateTargetFilterList, this.ParticipatingCharacterSets, this.ConfigData.AllowTemporaryParticipateCharacter, true, false);
				if (!flag2)
				{
					this.State = 3;
				}
			}
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x00169E3C File Offset: 0x0016803C
		private void OnMajorCharacterCalled(DataContext context, GameData.Domains.Character.Character character)
		{
			bool isEnemyNest = this.ConfigData.IsEnemyNest;
			if (isEnemyNest)
			{
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				int charId = character.GetId();
				int currDate = DomainManager.World.GetCurrDate();
				lifeRecordCollection.AddEnterEnemyNest(charId, currDate, this.Location, this.ConfigData.AdventureId);
			}
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x00169E94 File Offset: 0x00168094
		public void SelectLocation()
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			MapBlockData blockData = (this._assignedAreaId < 0) ? DomainManager.Map.GetRandomMapBlockDataByFilters(context.Random, this.ConfigData.MapState, this.ConfigData.MapArea, this.ConfigData.MapBlockSubType, false) : DomainManager.Map.GetRandomMapBlockDataInAreaByFilters(context.Random, this._assignedAreaId, this.ConfigData.MapBlockSubType, false);
			bool flag = blockData != null;
			if (flag)
			{
				this.Location.AreaId = blockData.AreaId;
				this.Location.BlockId = blockData.BlockId;
			}
			else
			{
				this.Location.AreaId = -1;
				this.Location.BlockId = -1;
			}
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x00169F58 File Offset: 0x00168158
		public short GetSettlementArg()
		{
			MapBlockData belongSettlementBlock = DomainManager.Map.GetBelongSettlementBlock(this.Location);
			Settlement settlement = DomainManager.Organization.GetSettlementByLocation(belongSettlementBlock.GetLocation());
			return settlement.GetId();
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x00169F92 File Offset: 0x00168192
		public Location GetLocationArg()
		{
			return this.Location;
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x00169F9C File Offset: 0x0016819C
		public int GetCharacterArg()
		{
			bool flag = this.MajorCharacterSets.Count < 1;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				HashSet<int> collection = this.MajorCharacterSets[0].GetCollection();
				bool flag2 = collection.Count < 1;
				if (flag2)
				{
					result = -1;
				}
				else
				{
					result = collection.First<int>();
				}
			}
			return result;
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x00169FF2 File Offset: 0x001681F2
		public short GetAdventureArg()
		{
			return this.ConfigData.AdventureId;
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x00169FFF File Offset: 0x001681FF
		public sbyte GetLifeSkillTypeArg()
		{
			return ConfigMonthlyActionDefines.MonthlyActionToLifeSkillType[this.ConfigTemplateId];
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x0016A011 File Offset: 0x00168211
		public ConfigMonthlyAction()
		{
			this.MajorCharacterSets = new List<CharacterSet>();
			this.ParticipatingCharacterSets = new List<CharacterSet>();
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x0016A034 File Offset: 0x00168234
		public override bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x0016A048 File Offset: 0x00168248
		public override int GetSerializedSize()
		{
			int totalSize = 21;
			bool flag = this.MajorCharacterSets != null;
			if (flag)
			{
				totalSize += 2;
				int elementsCount = this.MajorCharacterSets.Count;
				for (int i = 0; i < elementsCount; i++)
				{
					totalSize += this.MajorCharacterSets[i].GetSerializedSize();
				}
			}
			else
			{
				totalSize += 2;
			}
			bool flag2 = this.ParticipatingCharacterSets != null;
			if (flag2)
			{
				totalSize += 2;
				int elementsCount2 = this.ParticipatingCharacterSets.Count;
				for (int j = 0; j < elementsCount2; j++)
				{
					totalSize += this.ParticipatingCharacterSets[j].GetSerializedSize();
				}
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x0016A11C File Offset: 0x0016831C
		public unsafe override int Serialize(byte* pData)
		{
			*(short*)pData = this.ConfigTemplateId;
			byte* pCurrData = pData + 2;
			pCurrData += this.Location.Serialize(pCurrData);
			bool flag = this.MajorCharacterSets != null;
			if (flag)
			{
				int elementsCount = this.MajorCharacterSets.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					int subDataSize = this.MajorCharacterSets[i].Serialize(pCurrData);
					pCurrData += subDataSize;
					Tester.Assert(subDataSize <= 65535, "");
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag2 = this.ParticipatingCharacterSets != null;
			if (flag2)
			{
				int elementsCount2 = this.ParticipatingCharacterSets.Count;
				Tester.Assert(elementsCount2 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount2);
				pCurrData += 2;
				for (int j = 0; j < elementsCount2; j++)
				{
					int subDataSize2 = this.ParticipatingCharacterSets[j].Serialize(pCurrData);
					pCurrData += subDataSize2;
					Tester.Assert(subDataSize2 <= 65535, "");
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			*pCurrData = (byte)this._announceMonth;
			pCurrData++;
			*(short*)pCurrData = this._assignedAreaId;
			pCurrData += 2;
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

		// Token: 0x06001914 RID: 6420 RVA: 0x0016A2DC File Offset: 0x001684DC
		public unsafe override int Deserialize(byte* pData)
		{
			this.ConfigTemplateId = *(short*)pData;
			byte* pCurrData = pData + 2;
			pCurrData += this.Location.Deserialize(pCurrData);
			ushort elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this.MajorCharacterSets == null;
				if (flag2)
				{
					this.MajorCharacterSets = new List<CharacterSet>((int)elementsCount);
				}
				else
				{
					this.MajorCharacterSets.Clear();
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					CharacterSet element = default(CharacterSet);
					pCurrData += element.Deserialize(pCurrData);
					this.MajorCharacterSets.Add(element);
				}
			}
			else
			{
				List<CharacterSet> majorCharacterSets = this.MajorCharacterSets;
				if (majorCharacterSets != null)
				{
					majorCharacterSets.Clear();
				}
			}
			ushort elementsCount2 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag3 = elementsCount2 > 0;
			if (flag3)
			{
				bool flag4 = this.ParticipatingCharacterSets == null;
				if (flag4)
				{
					this.ParticipatingCharacterSets = new List<CharacterSet>((int)elementsCount2);
				}
				else
				{
					this.ParticipatingCharacterSets.Clear();
				}
				for (int j = 0; j < (int)elementsCount2; j++)
				{
					CharacterSet element2 = default(CharacterSet);
					pCurrData += element2.Deserialize(pCurrData);
					this.ParticipatingCharacterSets.Add(element2);
				}
			}
			else
			{
				List<CharacterSet> participatingCharacterSets = this.ParticipatingCharacterSets;
				if (participatingCharacterSets != null)
				{
					participatingCharacterSets.Clear();
				}
			}
			this._announceMonth = *(sbyte*)pCurrData;
			pCurrData++;
			this._assignedAreaId = *(short*)pCurrData;
			pCurrData += 2;
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

		// Token: 0x04000592 RID: 1426
		[SerializableGameDataField]
		public short ConfigTemplateId;

		// Token: 0x04000593 RID: 1427
		[SerializableGameDataField]
		public Location Location;

		// Token: 0x04000594 RID: 1428
		[SerializableGameDataField]
		public List<CharacterSet> MajorCharacterSets;

		// Token: 0x04000595 RID: 1429
		[SerializableGameDataField]
		public List<CharacterSet> ParticipatingCharacterSets;

		// Token: 0x04000596 RID: 1430
		[SerializableGameDataField]
		private sbyte _announceMonth;

		// Token: 0x04000597 RID: 1431
		[SerializableGameDataField]
		private short _assignedAreaId;
	}
}
