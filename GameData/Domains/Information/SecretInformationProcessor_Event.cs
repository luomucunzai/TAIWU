using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using Config.EventConfig;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.TaiwuEvent.EventOption;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Information
{
	// Token: 0x02000684 RID: 1668
	public sealed class SecretInformationProcessor_Event
	{
		// Token: 0x0600546B RID: 21611 RVA: 0x002E26B4 File Offset: 0x002E08B4
		private SecretInformationProcessor_Event()
		{
		}

		// Token: 0x0600546C RID: 21612 RVA: 0x002E2780 File Offset: 0x002E0980
		public bool Initialize(GameData.Domains.Character.Character character, GameData.Domains.Character.Character taiwu, int metaDataId, EventArgBox argbox)
		{
			this.Reset();
			bool flag = taiwu == null || character == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this._metaDataId = metaDataId;
				bool flag2 = !this.Processor.Initialize(this._metaDataId);
				if (flag2)
				{
					result = false;
				}
				else
				{
					this._taiwu = taiwu;
					this._character = character;
					this._taiwuId = taiwu.GetId();
					this._characterId = character.GetId();
					this._argList = new List<int>(this.Processor.GetSecretInformationArgList());
					this._argList[3] = character.GetId();
					this._argList[5] = taiwu.GetId();
					this.ResultIndex = -1;
					this.LastResultIndex = -1;
					List<string> keys = new List<string>
					{
						"actorId",
						"reactorId",
						"secactorId"
					};
					for (int i = 0; i < 3; i++)
					{
						argbox.Set(keys[i], this._argList[i]);
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600546D RID: 21613 RVA: 0x002E28A4 File Offset: 0x002E0AA4
		public void Reset()
		{
			this._metaDataId = -1;
			this.ResultIndex = -1;
			this.LastResultIndex = -1;
			this._resultConfig = null;
			this._argList.Clear();
			this._taiwu = null;
			this._character = null;
			this._taiwuId = -1;
			this._characterId = -1;
			this._toKillCharIdTupleHashSet.Clear();
			this._toEscapeCharIdList.Clear();
			this._toDiscardCharIdList.Clear();
			this._toChangeHappinessCharIdList.Clear();
			this._toChangeInfectionCharIdList.Clear();
			this._toChangeCharacterFavorList.Clear();
			this._savedCombatData = null;
			this._savedActionData = null;
			this._savedRelationChangeData = null;
			this._savedContentSelections.Clear();
			this._secretInformationContentId = -1;
			this._secretInformationStructId = -1;
			this._secretInformationContentIndex = -1;
			this._eventAction = SecretInformationProcessor_Event.EventAction.ShowEvent;
			this.Processor.Reset();
		}

		// Token: 0x0600546E RID: 21614 RVA: 0x002E2988 File Offset: 0x002E0B88
		public bool SetEventGuid(string part1, string part2, EventArgBox argbox)
		{
			bool flag = DomainManager.TaiwuEvent.GetEvent(part1) == null || DomainManager.TaiwuEvent.GetEvent(part2) == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				argbox.Set("resultEventGuid", part1);
				argbox.Set("conditionEventGuid", part2);
				result = true;
			}
			return result;
		}

		// Token: 0x0600546F RID: 21615 RVA: 0x002E29DC File Offset: 0x002E0BDC
		public string GetEventGuid(EventArgBox argbox, bool isPart2 = false)
		{
			string key = isPart2 ? "conditionEventGuid" : "resultEventGuid";
			string guid = string.Empty;
			argbox.Get(key, ref guid);
			return guid;
		}

		// Token: 0x06005470 RID: 21616 RVA: 0x002E2A0F File Offset: 0x002E0C0F
		public void SetResultIndex(short resultId)
		{
			this.ResultIndex = resultId;
			this.LastResultIndex = -1;
		}

		// Token: 0x06005471 RID: 21617 RVA: 0x002E2A20 File Offset: 0x002E0C20
		public List<short> GetEventShowData_SelectionKey()
		{
			SecretInformationAppliedResultItem resultConfig = SecretInformationAppliedResult.Instance.GetItem(this.LastResultIndex);
			bool flag = resultConfig == null || resultConfig.SelectionIds == null;
			List<short> result;
			if (flag)
			{
				result = new List<short>();
			}
			else
			{
				result = this.Processor.GetVisibleSelection(resultConfig.SelectionIds, this._character, this._taiwu);
			}
			return result;
		}

		// Token: 0x06005472 RID: 21618 RVA: 0x002E2A7C File Offset: 0x002E0C7C
		public TaiwuEventOption[] GetEventShowData_Selection(EventArgBox argBox, TaiwuEvent eventData)
		{
			List<short> selectionKeys = this.GetEventShowData_SelectionKey();
			TaiwuEventOption[] result = this.MakeSecretInformationSelections(selectionKeys, argBox, eventData, "");
			bool addOtherOption = true;
			foreach (TaiwuEventOption item in result)
			{
				bool flag = this.CheckInformationSelectionAvailable(item);
				if (flag)
				{
					addOtherOption = false;
					break;
				}
			}
			bool flag2 = addOtherOption;
			if (flag2)
			{
				selectionKeys.Add(0);
				result = this.MakeSecretInformationSelections(selectionKeys, argBox, eventData, "");
			}
			return result;
		}

		// Token: 0x06005473 RID: 21619 RVA: 0x002E2AFC File Offset: 0x002E0CFC
		public string GetEventShowData_Content()
		{
			SecretInformationAppliedResultItem resultConfig = SecretInformationAppliedResult.Instance.GetItem(this.LastResultIndex);
			bool flag = resultConfig == null;
			string result;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
				defaultInterpolatedStringHandler.AppendLiteral("null Config：");
				defaultInterpolatedStringHandler.AppendFormatted<short>(this.LastResultIndex);
				result = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				bool flag2 = resultConfig.Texts == null;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
					defaultInterpolatedStringHandler.AppendLiteral("null Texts：");
					defaultInterpolatedStringHandler.AppendFormatted<short>(this.LastResultIndex);
					result = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				else
				{
					result = (string.IsNullOrEmpty(resultConfig.Texts[1]) ? resultConfig.Texts[0] : resultConfig.Texts[(int)this._character.GetBehaviorType()]);
				}
			}
			return result;
		}

		// Token: 0x06005474 RID: 21620 RVA: 0x002E2BC0 File Offset: 0x002E0DC0
		public bool GetEventShowData_RevealCharacters()
		{
			SecretInformationAppliedResultItem resultConfig = SecretInformationAppliedResult.Instance.GetItem(this.LastResultIndex);
			bool flag = resultConfig == null || resultConfig.SelectionIds == null;
			return !flag && (resultConfig.RevealCharacters && this._taiwu != null) && this._character != null;
		}

		// Token: 0x06005475 RID: 21621 RVA: 0x002E2C18 File Offset: 0x002E0E18
		public int GetEventAction(EventArgBox argBox, out string eventGuid)
		{
			eventGuid = string.Empty;
			bool flag = this.ResultIndex == -1;
			int result;
			if (flag)
			{
				this.ApplyEventEnd(argBox);
				result = 1;
			}
			else
			{
				while (this.ResultIndex != -1)
				{
					this.LastResultIndex = this.ResultIndex;
					bool flag2 = !this.RefreshResultIndex();
					if (flag2)
					{
						this.ApplyEventEnd(argBox);
						return 1;
					}
					bool flag3 = this.TryGetOutsideJumpEventGuid(out eventGuid);
					if (flag3)
					{
						bool endEventAfterJump = this._resultConfig.EndEventAfterJump;
						if (endEventAfterJump)
						{
							this.ApplyEventEnd(argBox);
						}
						return 6;
					}
					this.SaveResultCharacterStateChanges();
					this.SecretInformationMaker_Entrance(argBox);
					this.ResultIndex = this.ApplyEventCondition(argBox);
				}
				switch (this._eventAction)
				{
				case SecretInformationProcessor_Event.EventAction.ShowEvent:
				{
					bool flag4 = this.LastResultIndex == -1 || this.GetEventShowData_SelectionKey().Count == 0;
					if (flag4)
					{
						this.ApplyEventEnd(argBox);
						return 1;
					}
					break;
				}
				case SecretInformationProcessor_Event.EventAction.StartCombat:
				{
					bool flag5 = this._savedCombatData != null;
					if (!flag5)
					{
						this.ApplyEventEnd(argBox);
						return 1;
					}
					EventHelper.StartCombat(this._characterId, this._savedCombatData.CombatConfigId, this.GetEventGuid(argBox, true), argBox, this._savedCombatData.NoGuard);
					break;
				}
				case SecretInformationProcessor_Event.EventAction.StartLifeSkillCombat:
					EventHelper.StartLifeSkillCombat(this._characterId, 16, this.GetEventGuid(argBox, true), argBox);
					break;
				case SecretInformationProcessor_Event.EventAction.ChooseRope:
					eventGuid = this.GetEventGuid(argBox, true);
					return 6;
				}
				result = (int)this._eventAction;
			}
			return result;
		}

		// Token: 0x06005476 RID: 21622 RVA: 0x002E2DB4 File Offset: 0x002E0FB4
		public void ApplyEventEnd(EventArgBox argBox)
		{
			foreach (KeyValuePair<int, Dictionary<int, int>> item in this._toChangeCharacterFavorList)
			{
				foreach (KeyValuePair<int, int> item2 in item.Value)
				{
					SecretInformationProcessor_Event.ChangeFavorability(item.Key, item2.Key, item2.Value);
				}
			}
			foreach (KeyValuePair<int, int> item3 in this._toChangeHappinessCharIdList)
			{
				GameData.Domains.Character.Character character;
				bool flag = InformationDomain.CheckTuringTest(item3.Key, out character);
				if (flag)
				{
					EventHelper.ChangeRoleHappiness(character, item3.Value);
				}
			}
			foreach (KeyValuePair<int, int> item4 in this._toChangeInfectionCharIdList)
			{
				GameData.Domains.Character.Character character2;
				bool flag2 = InformationDomain.CheckTuringTest(item4.Key, out character2);
				if (flag2)
				{
					EventHelper.ChangeRoleInfectedValue(character2, item4.Value);
				}
			}
			EventHelper.DisseminateSecretInformationFromTaiwu(this._metaDataId, this._characterId);
			foreach (ValueTuple<int, int> item5 in this._toDiscardCharIdList)
			{
				DomainManager.Information.DiscardSecretInformation(DomainManager.TaiwuEvent.MainThreadDataContext, item5.Item1, item5.Item2);
			}
			bool taiwuDie = false;
			foreach (ValueTuple<int, int, bool> valueTuple in this._toKillCharIdTupleHashSet)
			{
				bool flag3 = valueTuple.Item2 == this._taiwuId;
				if (flag3)
				{
					taiwuDie = true;
				}
				else
				{
					GameData.Domains.Character.Character character3;
					bool flag4 = DomainManager.Character.TryGetElement_Objects(valueTuple.Item2, out character3);
					if (flag4)
					{
						short deathType = valueTuple.Item3 ? 4 : 3;
						DomainManager.Character.MakeCharacterDead(DomainManager.TaiwuEvent.MainThreadDataContext, character3, deathType);
					}
				}
			}
			HashSet<int> victimHashSet = new HashSet<int>(from tuple in this._toKillCharIdTupleHashSet
			select tuple.Item2);
			foreach (int item6 in this._toEscapeCharIdList)
			{
				GameData.Domains.Character.Character character4;
				bool flag5 = !victimHashSet.Contains(item6) && DomainManager.Character.TryGetElement_Objects(item6, out character4);
				if (flag5)
				{
					EventHelper.CharacterEscapeToNearbyBlock(argBox, character4, 1);
				}
			}
			bool flag6 = taiwuDie;
			if (flag6)
			{
				EventHelper.TriggerLegacyPassingEvent(true, "");
			}
			this.Reset();
		}

		// Token: 0x06005477 RID: 21623 RVA: 0x002E30F8 File Offset: 0x002E12F8
		private bool RefreshResultIndex()
		{
			this._eventAction = SecretInformationProcessor_Event.EventAction.ShowEvent;
			this._resultConfig = SecretInformationAppliedResult.Instance.GetItem(this.ResultIndex);
			bool flag = this._resultConfig == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				short curResult = this.ResultIndex;
				for (;;)
				{
					short curResult2 = SecretInformationAppliedResult.Instance.GetItem(curResult).InnerResultEvent;
					bool flag2 = curResult2 != -1;
					if (!flag2)
					{
						break;
					}
					curResult = curResult2;
				}
				bool flag3 = curResult != this.ResultIndex;
				if (flag3)
				{
					this.ResultIndex = curResult;
				}
				this._resultConfig = SecretInformationAppliedResult.Instance.GetItem(this.ResultIndex);
				bool flag4 = this._resultConfig == null;
				result = !flag4;
			}
			return result;
		}

		// Token: 0x06005478 RID: 21624 RVA: 0x002E31B4 File Offset: 0x002E13B4
		private bool TryGetOutsideJumpEventGuid(out string eventGuid)
		{
			eventGuid = this._resultConfig.ResultEventGuid;
			bool flag = string.IsNullOrEmpty(eventGuid);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = DomainManager.TaiwuEvent.GetEvent(eventGuid) == null;
				result = !flag2;
			}
			return result;
		}

		// Token: 0x06005479 RID: 21625 RVA: 0x002E31FC File Offset: 0x002E13FC
		private void SaveResultCharacterStateChanges()
		{
			bool flag = this._resultConfig.SelfHappinessDiff != 0;
			if (flag)
			{
				bool flag2 = !this._toChangeHappinessCharIdList.ContainsKey(this._taiwuId);
				if (flag2)
				{
					this._toChangeHappinessCharIdList.Add(this._taiwuId, 0);
				}
				Dictionary<int, int> dictionary = this._toChangeHappinessCharIdList;
				int key = this._taiwuId;
				dictionary[key] += (int)this._resultConfig.SelfHappinessDiff;
			}
			bool flag3 = this._resultConfig.SelfInfectionDiff != 0;
			if (flag3)
			{
				bool flag4 = !this._toChangeInfectionCharIdList.ContainsKey(this._taiwuId);
				if (flag4)
				{
					this._toChangeInfectionCharIdList.Add(this._taiwuId, 0);
				}
				Dictionary<int, int> dictionary = this._toChangeInfectionCharIdList;
				int key = this._taiwuId;
				dictionary[key] += (int)this._resultConfig.SelfInfectionDiff;
			}
			bool flag5 = this._resultConfig.OppositeHappinessDiff != 0;
			if (flag5)
			{
				bool flag6 = !this._toChangeHappinessCharIdList.ContainsKey(this._characterId);
				if (flag6)
				{
					this._toChangeHappinessCharIdList.Add(this._characterId, 0);
				}
				Dictionary<int, int> dictionary = this._toChangeHappinessCharIdList;
				int key = this._characterId;
				dictionary[key] += (int)this._resultConfig.OppositeHappinessDiff;
			}
			bool flag7 = this._resultConfig.OppositeInfectionDiff != 0;
			if (flag7)
			{
				bool flag8 = !this._toChangeInfectionCharIdList.ContainsKey(this._characterId);
				if (flag8)
				{
					this._toChangeInfectionCharIdList.Add(this._characterId, 0);
				}
				Dictionary<int, int> dictionary = this._toChangeInfectionCharIdList;
				int key = this._characterId;
				dictionary[key] += (int)this._resultConfig.OppositeInfectionDiff;
			}
			bool flag9 = this._resultConfig.SelfFavorabilityDiff != 0;
			if (flag9)
			{
				bool flag10 = !this._toChangeCharacterFavorList.ContainsKey(this._taiwuId);
				if (flag10)
				{
					this._toChangeCharacterFavorList.Add(this._taiwuId, new Dictionary<int, int>());
				}
				bool flag11 = !this._toChangeCharacterFavorList[this._taiwuId].ContainsKey(this._characterId);
				if (flag11)
				{
					this._toChangeCharacterFavorList[this._taiwuId].Add(this._characterId, 0);
				}
				Dictionary<int, int> dictionary = this._toChangeCharacterFavorList[this._taiwuId];
				int key = this._characterId;
				dictionary[key] += (int)this._resultConfig.SelfFavorabilityDiff;
			}
			bool flag12 = this._resultConfig.OppositeFavorabilityDiff != 0;
			if (flag12)
			{
				bool flag13 = !this._toChangeCharacterFavorList.ContainsKey(this._characterId);
				if (flag13)
				{
					this._toChangeCharacterFavorList.Add(this._characterId, new Dictionary<int, int>());
				}
				bool flag14 = !this._toChangeCharacterFavorList[this._characterId].ContainsKey(this._taiwuId);
				if (flag14)
				{
					this._toChangeCharacterFavorList[this._characterId].Add(this._taiwuId, 0);
				}
				Dictionary<int, int> dictionary = this._toChangeCharacterFavorList[this._characterId];
				int key = this._taiwuId;
				dictionary[key] += (int)this._resultConfig.OppositeFavorabilityDiff;
			}
		}

		// Token: 0x0600547A RID: 21626 RVA: 0x002E3540 File Offset: 0x002E1740
		public bool GetSecretInformationEventShowData(EventArgBox argBox, TaiwuEvent eventData, out TaiwuEventOption[] options)
		{
			options = null;
			IRandomSource radom = DomainManager.TaiwuEvent.MainThreadDataContext.Random;
			short structId = this.Processor.GetSecretInformationAppliedStructs(radom, this._character, this._taiwu);
			bool flag = structId == -1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this._secretInformationStructId = structId;
				List<short> selectionKeys;
				short contentIndex;
				short contentId = this.Processor.GetContentIdAndSelections(radom, structId, this._character, this._taiwu, out selectionKeys, out contentIndex);
				bool flag2 = contentId == -1;
				if (flag2)
				{
					result = false;
				}
				else
				{
					this._secretInformationContentId = contentId;
					this._secretInformationContentIndex = contentIndex;
					SecretInformationAppliedContentItem contenConfig = SecretInformationAppliedContent.Instance.GetItem(contentId);
					bool flag3 = contenConfig.LinkedResult != -1;
					if (flag3)
					{
						this.SetResultIndex(contenConfig.LinkedResult);
						result = true;
					}
					else
					{
						string content = contenConfig.Texts[(int)this._character.GetBehaviorType()];
						this._savedContentText = content;
						List<short> visibleSelectionKeys = this.Processor.GetVisibleSelection(selectionKeys, this._character, this._taiwu);
						options = this.MakeSecretInformationSelections(visibleSelectionKeys, argBox, eventData, "");
						bool addOtherOption = true;
						foreach (TaiwuEventOption item in options)
						{
							bool flag4 = this.CheckInformationSelectionAvailable(item);
							if (flag4)
							{
								addOtherOption = false;
								break;
							}
						}
						bool flag5 = addOtherOption;
						if (flag5)
						{
							visibleSelectionKeys.Add(0);
							options = this.MakeSecretInformationSelections(visibleSelectionKeys, argBox, eventData, "");
						}
						this._savedContentSelections = visibleSelectionKeys;
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x0600547B RID: 21627 RVA: 0x002E36BC File Offset: 0x002E18BC
		public TaiwuEventOption[] GetSavedContentSelection(EventArgBox argBox, TaiwuEvent eventData)
		{
			return this.MakeSecretInformationSelections(this._savedContentSelections, argBox, eventData, "");
		}

		// Token: 0x0600547C RID: 21628 RVA: 0x002E36E4 File Offset: 0x002E18E4
		public string GetSavedContentTexs()
		{
			return this._savedContentText;
		}

		// Token: 0x0600547D RID: 21629 RVA: 0x002E36FC File Offset: 0x002E18FC
		public short GetFristContentId()
		{
			return this._secretInformationContentId;
		}

		// Token: 0x0600547E RID: 21630 RVA: 0x002E3714 File Offset: 0x002E1914
		public short GetFristStructId()
		{
			return this._secretInformationStructId;
		}

		// Token: 0x0600547F RID: 21631 RVA: 0x002E372C File Offset: 0x002E192C
		public bool IsContentAskKeepContent()
		{
			return this._secretInformationContentIndex == 2;
		}

		// Token: 0x06005480 RID: 21632 RVA: 0x002E3748 File Offset: 0x002E1948
		private void SecretInformationMaker_Entrance(EventArgBox argBox)
		{
			foreach (Config.ShortList item in this._resultConfig.SecretInformation)
			{
				List<short> configItem = new List<short>(item.DataList);
				bool flag = configItem.Count < 2;
				if (!flag)
				{
					short infoKey = configItem[0];
					bool flag2 = infoKey < 0;
					if (!flag2)
					{
						this.SecretInformationMaker_Box(argBox, infoKey, this._argList[(int)configItem[1]], (configItem.Count > 2) ? this._argList[(int)configItem[2]] : -1, (configItem.Count > 3) ? this._argList[(int)configItem[3]] : -1);
					}
				}
			}
		}

		// Token: 0x06005481 RID: 21633 RVA: 0x002E382C File Offset: 0x002E1A2C
		private void SecretInformationMaker_Box(EventArgBox argBox, short infoKey, int actorId, int reactorId, int secactorId)
		{
			if (infoKey <= 38)
			{
				switch (infoKey)
				{
				case 1:
					this.MakeNewInfo_KillInPublic(actorId, reactorId);
					break;
				case 2:
					this.MakeNewInfo_KidnapInPublic(argBox, actorId, reactorId);
					break;
				case 3:
					this.MakeNewInfo_KillForPunishment(actorId, reactorId);
					break;
				case 4:
					this.MakeNewInfo_KidnapForPunishment(argBox, actorId, reactorId);
					break;
				default:
					switch (infoKey)
					{
					case 24:
						this.MakeNewInfo_ReleaseKidnappedCharacter(actorId, reactorId);
						break;
					case 25:
						this.MakeNewInfo_RescueKidnappedCharacter(actorId, reactorId, secactorId);
						break;
					case 30:
					{
						bool result = this.MakeNewInfo_SeverEnemy(actorId, reactorId);
						this._savedRelationChangeData = new SecretInformationProcessor_Event.RelationChangeData(actorId, reactorId, 32768, true, result);
						break;
					}
					case 31:
					{
						bool result2 = this.MakeNewInfo_BecomeEnemy(actorId, reactorId);
						this._savedRelationChangeData = new SecretInformationProcessor_Event.RelationChangeData(actorId, reactorId, 32768, false, result2);
						break;
					}
					case 32:
					{
						bool result3 = this.MakeNewInfo_BecomeFriend(actorId, reactorId);
						this._savedRelationChangeData = new SecretInformationProcessor_Event.RelationChangeData(actorId, reactorId, 8192, false, result3);
						break;
					}
					case 33:
					{
						bool result4 = this.MakeNewInfo_SeverFriend(actorId, reactorId);
						this._savedRelationChangeData = new SecretInformationProcessor_Event.RelationChangeData(actorId, reactorId, 8192, true, result4);
						break;
					}
					case 35:
					{
						bool result5 = this.MakeNewInfo_BreakupWithLover(actorId, reactorId);
						this._savedRelationChangeData = new SecretInformationProcessor_Event.RelationChangeData(actorId, reactorId, 16384, true, result5);
						break;
					}
					case 37:
					{
						bool result6 = this.MakeNewInfo_BecomeSwornBrothersAndSisters(actorId, reactorId);
						this._savedRelationChangeData = new SecretInformationProcessor_Event.RelationChangeData(actorId, reactorId, 512, false, result6);
						break;
					}
					case 38:
					{
						bool result7 = this.MakeNewInfo_SeverSwornBrothersAndSisters(actorId, reactorId);
						this._savedRelationChangeData = new SecretInformationProcessor_Event.RelationChangeData(actorId, reactorId, 512, true, result7);
						break;
					}
					}
					break;
				}
			}
			else if (infoKey != 95)
			{
				if (infoKey == 96)
				{
					this.MakeNewInfo_KidnapInPrivate(argBox, actorId, reactorId);
				}
			}
			else
			{
				this.MakeNewInfo_KillInPrivate(actorId, reactorId);
			}
		}

		// Token: 0x06005482 RID: 21634 RVA: 0x002E3A40 File Offset: 0x002E1C40
		private bool MakeNewInfo_KillInPublic(int killerId, int victimId)
		{
			bool flag = this.MakeNewInfo_ApplyKill(killerId, victimId, true);
			bool result;
			if (flag)
			{
				DomainManager.Information.AddSecretInformationMetaData(DomainManager.TaiwuEvent.MainThreadDataContext, DomainManager.Information.GetSecretInformationCollection().AddKillInPublic(killerId, victimId), true);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005483 RID: 21635 RVA: 0x002E3A8C File Offset: 0x002E1C8C
		private bool MakeNewInfo_KillInPrivate(int killerId, int victimId)
		{
			bool flag = this.MakeNewInfo_ApplyKill(killerId, victimId, false);
			bool result;
			if (flag)
			{
				DomainManager.Information.AddSecretInformationMetaData(DomainManager.TaiwuEvent.MainThreadDataContext, DomainManager.Information.GetSecretInformationCollection().AddKillInPrivate(killerId, victimId), true);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005484 RID: 21636 RVA: 0x002E3AD8 File Offset: 0x002E1CD8
		private bool MakeNewInfo_KillForPunishment(int killerId, int victimId)
		{
			bool flag = this.MakeNewInfo_ApplyKill(killerId, victimId, true);
			bool result;
			if (flag)
			{
				DomainManager.Information.AddSecretInformationMetaData(DomainManager.TaiwuEvent.MainThreadDataContext, DomainManager.Information.GetSecretInformationCollection().AddKillForPunishment(killerId, victimId), true);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005485 RID: 21637 RVA: 0x002E3B24 File Offset: 0x002E1D24
		private bool MakeNewInfo_ApplyKill(int killerId, int victimId, bool inPublic = true)
		{
			GameData.Domains.Character.Character killerChar;
			GameData.Domains.Character.Character victimChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(killerId, out killerChar) || !DomainManager.Character.TryGetElement_Objects(victimId, out victimChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Location location = killerChar.GetLocation();
				int currDate = DomainManager.World.GetCurrDate();
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				if (inPublic)
				{
					lifeRecordCollection.AddKillInPublic(killerId, currDate, victimId, location);
				}
				else
				{
					lifeRecordCollection.AddKillInPrivate(killerId, currDate, victimId, location);
				}
				this._toKillCharIdTupleHashSet.Add(new ValueTuple<int, int, bool>(killerId, victimId, inPublic));
				result = true;
			}
			return result;
		}

		// Token: 0x06005486 RID: 21638 RVA: 0x002E3BBC File Offset: 0x002E1DBC
		private bool MakeNewInfo_KidnapInPublic(EventArgBox argBox, int kidnapperId, int prisonerId)
		{
			bool flag = this.MakeNewInfo_ApplyKidnap(argBox, kidnapperId, prisonerId, true);
			bool result;
			if (flag)
			{
				DomainManager.Information.AddSecretInformationMetaData(DomainManager.TaiwuEvent.MainThreadDataContext, DomainManager.Information.GetSecretInformationCollection().AddKidnapInPublic(kidnapperId, prisonerId), true);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005487 RID: 21639 RVA: 0x002E3C0C File Offset: 0x002E1E0C
		private bool MakeNewInfo_KidnapInPrivate(EventArgBox argBox, int kidnapperId, int prisonerId)
		{
			bool flag = this.MakeNewInfo_ApplyKidnap(argBox, kidnapperId, prisonerId, false);
			bool result;
			if (flag)
			{
				DomainManager.Information.AddSecretInformationMetaData(DomainManager.TaiwuEvent.MainThreadDataContext, DomainManager.Information.GetSecretInformationCollection().AddKidnapInPrivate(kidnapperId, prisonerId), true);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005488 RID: 21640 RVA: 0x002E3C5C File Offset: 0x002E1E5C
		private bool MakeNewInfo_KidnapForPunishment(EventArgBox argBox, int kidnapperId, int prisonerId)
		{
			bool flag = this.MakeNewInfo_ApplyKidnap(argBox, kidnapperId, prisonerId, true);
			bool result;
			if (flag)
			{
				DomainManager.Information.AddSecretInformationMetaData(DomainManager.TaiwuEvent.MainThreadDataContext, DomainManager.Information.GetSecretInformationCollection().AddKidnapForPunishment(kidnapperId, prisonerId), true);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005489 RID: 21641 RVA: 0x002E3CAC File Offset: 0x002E1EAC
		private bool MakeNewInfo_ApplyKidnap(EventArgBox argBox, int kidnaperId, int prisonerId, bool inPublic = true)
		{
			bool flag = prisonerId == this._argList[5] || prisonerId == kidnaperId;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				GameData.Domains.Character.Character kidnaperChar;
				GameData.Domains.Character.Character prisonerChar;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(kidnaperId, out kidnaperChar) || !DomainManager.Character.TryGetElement_Objects(prisonerId, out prisonerChar);
				if (flag2)
				{
					result = false;
				}
				else
				{
					int oriKidnaperId = prisonerChar.GetKidnapperId();
					bool flag3 = oriKidnaperId == prisonerId;
					if (flag3)
					{
						result = false;
					}
					else
					{
						short ropeId = 73;
						ItemKey oriRope = default(ItemKey);
						bool isTanselate = false;
						bool flag4 = oriKidnaperId > 0;
						if (flag4)
						{
							KidnappedCharacterList kidnapList = DomainManager.Character.GetSomeoneKidnapCharacters(oriKidnaperId);
							bool flag5 = kidnapList != null;
							if (flag5)
							{
								KidnappedCharacter kidnapCharData = kidnapList.GetCollection().Find((KidnappedCharacter x) => x.CharId == prisonerId);
								bool flag6 = kidnapCharData != null;
								if (flag6)
								{
									oriRope = kidnapCharData.RopeItemKey;
									ropeId = (short)oriRope.ItemType;
									isTanselate = true;
								}
							}
							DomainManager.Character.RemoveKidnappedCharacter(DomainManager.TaiwuEvent.MainThreadDataContext, prisonerId, oriKidnaperId, false);
						}
						ItemKey curRope;
						bool flag7 = argBox.Get<ItemKey>("ItemKeySeizeCharacterInCombat", out curRope) && kidnaperId == this._argList[5];
						if (flag7)
						{
							argBox.Remove<ItemKey>("ItemKeySeizeCharacterInCombat");
							ropeId = curRope.TemplateId;
							oriRope = curRope;
						}
						else
						{
							bool flag8 = kidnaperId != this._taiwuId;
							if (flag8)
							{
								DomainManager.Character.CombatResultHandle_KidnapEnemy(DomainManager.TaiwuEvent.MainThreadDataContext, kidnaperChar, prisonerChar, inPublic);
								return false;
							}
							bool flag9 = !isTanselate;
							if (flag9)
							{
								oriRope = DomainManager.Item.CreateItem(DomainManager.TaiwuEvent.MainThreadDataContext, 12, ropeId);
								kidnaperChar.AddInventoryItem(DomainManager.TaiwuEvent.MainThreadDataContext, oriRope, 1, false);
							}
						}
						int useRopeCharId = -1;
						bool flag10 = argBox.Get("UseItemKeySeizeCharacterId", ref useRopeCharId) && useRopeCharId >= 0 && useRopeCharId != kidnaperId;
						if (flag10)
						{
							GameData.Domains.Character.Character useRopeCharacter = DomainManager.Character.GetElement_Objects(useRopeCharId);
							GameData.Domains.Character.Character kidnapper = DomainManager.Character.GetElement_Objects(kidnaperId);
							useRopeCharacter.RemoveInventoryItem(DomainManager.TaiwuEvent.MainThreadDataContext, oriRope, 1, false, false);
							kidnapper.AddInventoryItem(DomainManager.TaiwuEvent.MainThreadDataContext, oriRope, 1, false);
						}
						DomainManager.Character.AddKidnappedCharacter(DomainManager.TaiwuEvent.MainThreadDataContext, kidnaperId, prisonerId, oriRope);
						Location location = kidnaperChar.GetLocation();
						int currDate = DomainManager.World.GetCurrDate();
						LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
						if (inPublic)
						{
							lifeRecordCollection.AddKidnapInPublic(kidnaperId, currDate, prisonerId, location, 12, ropeId);
						}
						else
						{
							lifeRecordCollection.AddKidnapInPrivate(kidnaperId, currDate, prisonerId, location, 12, ropeId);
						}
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x0600548A RID: 21642 RVA: 0x002E3F80 File Offset: 0x002E2180
		private bool MakeNewInfo_RescueKidnappedCharacter(int saviorId, int prisonerId, int kidnaperId)
		{
			bool flag = this.MakeNewInfo_ApplyRescue(saviorId, prisonerId, kidnaperId);
			bool result;
			if (flag)
			{
				DomainManager.Information.AddSecretInformationMetaData(DomainManager.TaiwuEvent.MainThreadDataContext, DomainManager.Information.GetSecretInformationCollection().AddRescueKidnappedCharacter(saviorId, prisonerId, kidnaperId), true);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600548B RID: 21643 RVA: 0x002E3FD0 File Offset: 0x002E21D0
		private bool MakeNewInfo_ApplyRescue(int saviorId, int prisonerId, int kidnaperId)
		{
			GameData.Domains.Character.Character saviorChar;
			GameData.Domains.Character.Character kidnaperChar;
			GameData.Domains.Character.Character prisonerChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(saviorId, out saviorChar) || !DomainManager.Character.TryGetElement_Objects(kidnaperId, out kidnaperChar) || !DomainManager.Character.TryGetElement_Objects(prisonerId, out prisonerChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = prisonerChar.GetKidnapperId() != kidnaperId;
				if (flag2)
				{
					result = false;
				}
				else
				{
					DomainManager.Character.RemoveKidnappedCharacter(DomainManager.TaiwuEvent.MainThreadDataContext, prisonerId, kidnaperId, false);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600548C RID: 21644 RVA: 0x002E4050 File Offset: 0x002E2250
		private bool MakeNewInfo_ReleaseKidnappedCharacter(int kidnaperId, int prisonerId)
		{
			bool flag = this.MakeNewInfo_ApplyRelease(kidnaperId, prisonerId);
			bool result;
			if (flag)
			{
				DomainManager.Information.AddSecretInformationMetaData(DomainManager.TaiwuEvent.MainThreadDataContext, DomainManager.Information.GetSecretInformationCollection().AddReleaseKidnappedCharacter(kidnaperId, prisonerId), true);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600548D RID: 21645 RVA: 0x002E409C File Offset: 0x002E229C
		private bool MakeNewInfo_ApplyRelease(int kidnaperId, int prisonerId)
		{
			GameData.Domains.Character.Character kidnaperChar;
			GameData.Domains.Character.Character prisonerChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(kidnaperId, out kidnaperChar) || !DomainManager.Character.TryGetElement_Objects(prisonerId, out prisonerChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = prisonerChar.GetKidnapperId() != kidnaperId;
				if (flag2)
				{
					result = false;
				}
				else
				{
					DomainManager.Character.RemoveKidnappedCharacter(DomainManager.TaiwuEvent.MainThreadDataContext, prisonerId, kidnaperId, false);
					Location location = kidnaperChar.GetLocation();
					int currDate = DomainManager.World.GetCurrDate();
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					lifeRecordCollection.AddReleaseKidnappedCharacter(kidnaperId, currDate, prisonerId, location);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600548E RID: 21646 RVA: 0x002E4138 File Offset: 0x002E2338
		private bool MakeNewInfo_BecomeSwornBrothersAndSisters(int selfId, int targetId)
		{
			GameData.Domains.Character.Character selfChar;
			GameData.Domains.Character.Character targetChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(selfId, out selfChar) || !DomainManager.Character.TryGetElement_Objects(targetId, out targetChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !RelationTypeHelper.AllowAddingSwornBrotherOrSisterRelation(targetId, selfId);
				if (flag2)
				{
					result = false;
				}
				else
				{
					DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
					sbyte selfBehaviorType = selfChar.GetBehaviorType();
					bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
					bool targetIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(targetId);
					GameData.Domains.Character.Character.ApplyBecomeSwornBrotherOrSister(context, selfChar, targetChar, selfBehaviorType, selfIsTaiwuPeople, targetIsTaiwuPeople);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600548F RID: 21647 RVA: 0x002E41CC File Offset: 0x002E23CC
		private bool MakeNewInfo_SeverSwornBrothersAndSisters(int selfId, int targetId)
		{
			GameData.Domains.Character.Character selfChar;
			GameData.Domains.Character.Character targetChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(selfId, out selfChar) || !DomainManager.Character.TryGetElement_Objects(targetId, out targetChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !DomainManager.Character.HasRelation(targetId, selfId, 512);
				if (flag2)
				{
					result = false;
				}
				else
				{
					DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
					sbyte selfBehaviorType = selfChar.GetBehaviorType();
					bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
					bool targetIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(targetId);
					GameData.Domains.Character.Character.ApplySeverSwornBrotherOrSister(context, selfChar, targetChar, selfBehaviorType, selfIsTaiwuPeople, targetIsTaiwuPeople);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005490 RID: 21648 RVA: 0x002E4268 File Offset: 0x002E2468
		private bool MakeNewInfo_BreakupWithLover(int selfId, int targetId)
		{
			GameData.Domains.Character.Character selfChar;
			GameData.Domains.Character.Character targetChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(selfId, out selfChar) || !DomainManager.Character.TryGetElement_Objects(targetId, out targetChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !DomainManager.Character.HasRelation(targetId, selfId, 16384);
				if (flag2)
				{
					result = false;
				}
				else
				{
					DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
					sbyte selfBehaviorType = selfChar.GetBehaviorType();
					bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
					bool targetIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(targetId);
					GameData.Domains.Character.Character.ApplyBreakupWithBoyOrGirlFriend(context, selfChar, targetChar, selfBehaviorType, false, selfIsTaiwuPeople, targetIsTaiwuPeople);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005491 RID: 21649 RVA: 0x002E4304 File Offset: 0x002E2504
		private bool MakeNewInfo_BecomeFriend(int selfId, int targetId)
		{
			GameData.Domains.Character.Character selfChar;
			GameData.Domains.Character.Character targetChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(selfId, out selfChar) || !DomainManager.Character.TryGetElement_Objects(targetId, out targetChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !RelationTypeHelper.AllowAddingFriendRelation(targetId, selfId);
				if (flag2)
				{
					result = false;
				}
				else
				{
					DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
					sbyte selfBehaviorType = selfChar.GetBehaviorType();
					bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
					bool targetIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(targetId);
					GameData.Domains.Character.Character.ApplyBecomeFriend(context, selfChar, targetChar, selfBehaviorType, selfIsTaiwuPeople, targetIsTaiwuPeople);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005492 RID: 21650 RVA: 0x002E4398 File Offset: 0x002E2598
		private bool MakeNewInfo_SeverFriend(int selfId, int targetId)
		{
			GameData.Domains.Character.Character selfChar;
			GameData.Domains.Character.Character targetChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(selfId, out selfChar) || !DomainManager.Character.TryGetElement_Objects(targetId, out targetChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !DomainManager.Character.HasRelation(targetId, selfId, 8192);
				if (flag2)
				{
					result = false;
				}
				else
				{
					DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
					sbyte selfBehaviorType = selfChar.GetBehaviorType();
					bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
					bool targetIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(targetId);
					GameData.Domains.Character.Character.ApplySeverFriend(context, selfChar, targetChar, selfBehaviorType, selfIsTaiwuPeople, targetIsTaiwuPeople);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005493 RID: 21651 RVA: 0x002E4434 File Offset: 0x002E2634
		private bool MakeNewInfo_BecomeEnemy(int selfId, int targetId)
		{
			GameData.Domains.Character.Character selfChar;
			GameData.Domains.Character.Character targetChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(selfId, out selfChar) || !DomainManager.Character.TryGetElement_Objects(targetId, out targetChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !RelationTypeHelper.AllowAddingEnemyRelation(selfId, targetId);
				if (flag2)
				{
					result = false;
				}
				else
				{
					DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
					bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
					GameData.Domains.Character.Character.ApplyAddRelation_Enemy(context, selfChar, targetChar, selfIsTaiwuPeople, 5);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005494 RID: 21652 RVA: 0x002E44B0 File Offset: 0x002E26B0
		private bool MakeNewInfo_SeverEnemy(int selfId, int targetId)
		{
			GameData.Domains.Character.Character selfChar;
			GameData.Domains.Character.Character targetChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(selfId, out selfChar) || !DomainManager.Character.TryGetElement_Objects(targetId, out targetChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !DomainManager.Character.HasRelation(selfId, targetId, 32768);
				if (flag2)
				{
					result = false;
				}
				else
				{
					DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
					sbyte selfBehaviorType = selfChar.GetBehaviorType();
					bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
					GameData.Domains.Character.Character.ApplySeverEnemy(context, selfChar, targetChar, selfBehaviorType, selfIsTaiwuPeople);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005495 RID: 21653 RVA: 0x002E453C File Offset: 0x002E273C
		private void AddStealLifeRecord(int saviorId, int kidnaperId, int prisonerId, sbyte phase)
		{
			GameData.Domains.Character.Character saviorChar;
			GameData.Domains.Character.Character kidnaperChar;
			GameData.Domains.Character.Character prisonerChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(saviorId, out saviorChar) || !DomainManager.Character.TryGetElement_Objects(kidnaperId, out kidnaperChar) || !DomainManager.Character.TryGetElement_Objects(prisonerId, out prisonerChar);
			if (!flag)
			{
				Location location = saviorChar.GetLocation();
				int currDate = DomainManager.World.GetCurrDate();
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				switch (phase)
				{
				case 0:
					lifeRecordCollection.AddRescueKidnappedCharacterSecretlyFail1(saviorId, currDate, kidnaperId, location, prisonerId);
					break;
				case 1:
					lifeRecordCollection.AddRescueKidnappedCharacterSecretlyFail2(saviorId, currDate, kidnaperId, location, prisonerId);
					break;
				case 2:
					lifeRecordCollection.AddRescueKidnappedCharacterSecretlyFail3(saviorId, currDate, kidnaperId, location, prisonerId);
					break;
				case 3:
					lifeRecordCollection.AddRescueKidnappedCharacterSecretlyFail4(saviorId, currDate, kidnaperId, location, prisonerId);
					break;
				case 4:
					lifeRecordCollection.AddRescueKidnappedCharacterSecretlySucceed(saviorId, currDate, kidnaperId, location, prisonerId);
					break;
				case 5:
					lifeRecordCollection.AddRescueKidnappedCharacterSecretlySucceedAndEscaped(saviorId, currDate, kidnaperId, location, prisonerId);
					break;
				}
			}
		}

		// Token: 0x06005496 RID: 21654 RVA: 0x002E4630 File Offset: 0x002E2830
		private void AddScamLifeRecord(int saviorId, int kidnaperId, int prisonerId, sbyte phase)
		{
			GameData.Domains.Character.Character saviorChar;
			GameData.Domains.Character.Character kidnaperChar;
			GameData.Domains.Character.Character prisonerChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(saviorId, out saviorChar) || !DomainManager.Character.TryGetElement_Objects(kidnaperId, out kidnaperChar) || !DomainManager.Character.TryGetElement_Objects(prisonerId, out prisonerChar);
			if (!flag)
			{
				bool flag2 = phase < 3;
				if (flag2)
				{
					Location location = saviorChar.GetLocation();
					int currDate = DomainManager.World.GetCurrDate();
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					switch (phase)
					{
					case 0:
						lifeRecordCollection.AddRescueKidnappedCharacterWithWitFail1(saviorId, currDate, kidnaperId, location, prisonerId);
						break;
					case 1:
						lifeRecordCollection.AddRescueKidnappedCharacterWithWitFail2(saviorId, currDate, kidnaperId, location, prisonerId);
						break;
					case 2:
						lifeRecordCollection.AddRescueKidnappedCharacterWithWitFail3(saviorId, currDate, kidnaperId, location, prisonerId);
						break;
					}
				}
			}
		}

		// Token: 0x06005497 RID: 21655 RVA: 0x002E46F4 File Offset: 0x002E28F4
		private void AddRobLifeRecord(int saviorId, int kidnaperId, int prisonerId, sbyte phase)
		{
			GameData.Domains.Character.Character saviorChar;
			GameData.Domains.Character.Character kidnaperChar;
			GameData.Domains.Character.Character prisonerChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(saviorId, out saviorChar) || !DomainManager.Character.TryGetElement_Objects(kidnaperId, out kidnaperChar) || !DomainManager.Character.TryGetElement_Objects(prisonerId, out prisonerChar);
			if (!flag)
			{
				bool flag2 = phase < 4;
				if (flag2)
				{
					Location location = saviorChar.GetLocation();
					int currDate = DomainManager.World.GetCurrDate();
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					switch (phase)
					{
					case 0:
						lifeRecordCollection.AddRescueKidnappedCharacterWithForceFail1(saviorId, currDate, kidnaperId, location, prisonerId);
						break;
					case 1:
						lifeRecordCollection.AddRescueKidnappedCharacterWithForceFail2(saviorId, currDate, kidnaperId, location, prisonerId);
						break;
					case 2:
						lifeRecordCollection.AddRescueKidnappedCharacterWithForceFail3(saviorId, currDate, kidnaperId, location, prisonerId);
						break;
					case 3:
						lifeRecordCollection.AddRescueKidnappedCharacterWithForceFail4(saviorId, currDate, kidnaperId, location, prisonerId);
						break;
					}
				}
			}
		}

		// Token: 0x06005498 RID: 21656 RVA: 0x002E47D4 File Offset: 0x002E29D4
		private static short GetDataFromConditionResult(List<Config.ShortList> conditionResultIds, int listIndex, int dataIndex)
		{
			short result = -1;
			bool flag = listIndex < 0 || dataIndex < 0;
			short result2;
			if (flag)
			{
				result2 = result;
			}
			else
			{
				bool flag2 = conditionResultIds.Count<Config.ShortList>() <= listIndex;
				if (flag2)
				{
					result2 = result;
				}
				else
				{
					List<short> item = conditionResultIds[listIndex].DataList;
					bool flag3 = item.Count<short>() <= dataIndex;
					if (flag3)
					{
						result2 = result;
					}
					else
					{
						result2 = item[dataIndex];
					}
				}
			}
			return result2;
		}

		// Token: 0x06005499 RID: 21657 RVA: 0x002E483C File Offset: 0x002E2A3C
		private static List<short> GetDataListFromConditionResult(List<Config.ShortList> conditionResultIds, int listIndex)
		{
			List<short> result = new List<short>();
			bool flag = listIndex < 0;
			List<short> result2;
			if (flag)
			{
				result2 = result;
			}
			else
			{
				bool flag2 = conditionResultIds.Count<Config.ShortList>() <= listIndex;
				if (flag2)
				{
					result2 = result;
				}
				else
				{
					result2 = new List<short>(conditionResultIds[listIndex].DataList);
				}
			}
			return result2;
		}

		// Token: 0x0600549A RID: 21658 RVA: 0x002E4888 File Offset: 0x002E2A88
		private bool Chech_PercentProb(int prob)
		{
			return DomainManager.TaiwuEvent.MainThreadDataContext.Random.Next(0, 100) < prob;
		}

		// Token: 0x0600549B RID: 21659 RVA: 0x002E48B4 File Offset: 0x002E2AB4
		private bool Check_IfCanFight(GameData.Domains.Character.Character character, sbyte combatType)
		{
			return CombatDomain.GetDefeatMarksCountOutOfCombat(character) <= (int)GlobalConfig.NeedDefeatMarkCount[(int)combatType];
		}

		// Token: 0x0600549C RID: 21660 RVA: 0x002E48D8 File Offset: 0x002E2AD8
		private bool Check_IfCanPanish(GameData.Domains.Character.Character selfChar, GameData.Domains.Character.Character targetChar)
		{
			sbyte selfFame = selfChar.GetFameType();
			sbyte targetFame = targetChar.GetFameType();
			bool flag = selfFame == -2;
			if (flag)
			{
				selfFame = 3;
			}
			bool flag2 = targetFame == -2;
			if (flag2)
			{
				targetFame = 3;
			}
			return targetFame < 3 && selfFame > 3;
		}

		// Token: 0x0600549D RID: 21661 RVA: 0x002E491C File Offset: 0x002E2B1C
		private bool Check_CharKillTaiwuBecauseOfRefuseKeep(GameData.Domains.Character.Character selfChar, GameData.Domains.Character.Character targetChar, short[] probList)
		{
			sbyte favorLevel = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(selfChar.GetId(), targetChar.GetId()));
			short prob = probList[(int)selfChar.GetBehaviorType()];
			int prob_Final = (int)(prob * (short)(100 - (favorLevel - 2) * 20));
			return this.Chech_PercentProb(prob_Final);
		}

		// Token: 0x0600549E RID: 21662 RVA: 0x002E4968 File Offset: 0x002E2B68
		private bool Check_IsRescure(short lastResult)
		{
			bool flag = lastResult == -1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				SecretInformationAppliedResultItem config = SecretInformationAppliedResult.Instance.GetItem(lastResult);
				result = (SecretInformationProcessor_Event.GetDataFromConditionResult(config.SecretInformation, 0, 0) != -1);
			}
			return result;
		}

		// Token: 0x0600549F RID: 21663 RVA: 0x002E49A8 File Offset: 0x002E2BA8
		private short ApplyEventCondition(EventArgBox argBox)
		{
			SecretInformationSpecialConditionItem condition = SecretInformationSpecialCondition.Instance.GetItem(this._resultConfig.SpecialConditionId);
			bool flag = condition == null;
			short result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				ESecretInformationSpecialConditionCalculate calculate = condition.Calculate;
				if (!true)
				{
				}
				short num;
				switch (calculate)
				{
				case ESecretInformationSpecialConditionCalculate.TaiwuFight:
					num = this.ApplyCondition_TaiwuFight();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.TaiwuSteal:
					num = this.ApplyCondition_TaiwuSteal();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.TaiwuScam:
					num = this.ApplyCondition_TaiwuScam();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.TaiwuRob:
					num = this.ApplyCondition_TaiwuRob();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.TaiwuRescueEscape:
					num = this.ApplyCondition_TaiwuRescueEscape();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.CharRescueEscape:
					num = this.ApplyCondition_CharRescueEscape();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.CharFight:
					num = this.ApplyCondition_ChatFight();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.RefuseKeep:
					num = this.ApplyCondition_RefuseKeep();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.CharRescue:
					num = this.ApplyCondition_CharRescue();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.CharWin:
					num = this.ApplyCondition_CharWin(argBox);
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.CharJudge:
					num = this.ApplyCondition_CharJudge(argBox);
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.CharKidnap:
					num = this.ApplyCondition_CharKidnap(argBox);
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.TaiwuEscape:
					num = this.ApplyCondition_TaiwuEscape();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.CharEscape:
					num = this.ApplyCondition_CharEscape();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.StartFight:
					num = this.ApplyCondition_StartFight();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.StartLifeSkillCombat:
					num = this.ApplyCondition_StartLifeSkillCombat();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.ChooseRope:
					num = this.ApplyCondition_ChooseRope();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.KidnapWithRope:
					num = this.ApplyCondition_KidnapWithRope(argBox);
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.CharAdore:
					num = this.ApplyCondition_CharAdore();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.TaiwuAdore:
					num = this.ApplyCondition_TaiwuAdore();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.TaiwuDeleteInfomation:
					num = this.ApplyCondition_TaiwuDeleteInfomation();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.CharDeleteInfomation:
					num = this.ApplyCondition_CharDeleteInfomation();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.Forgive:
					num = this.ApplyCondition_Forgive(argBox);
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.NotForgiveRape:
					num = this.ApplyCondition_NotForgiveRape();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.AskCharKeep:
					num = this.ApplyCondition_AskCharKeep();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.AskCharRelease:
					num = this.ApplyCondition_AskCharRelease();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.AskCharBreakup:
					num = this.ApplyCondition_AskCharBreakup(argBox);
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.BreakupWithChar:
					num = this.ApplyCondition_BreakupWithChar();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.ForceBreakupWithChar:
					num = this.ApplyCondition_ForceBreakupWithChar();
					goto IL_219;
				case ESecretInformationSpecialConditionCalculate.ShowFristContent:
					num = this.ApplyCondition_ShowFristContent();
					goto IL_219;
				}
				num = -1;
				IL_219:
				if (!true)
				{
				}
				result = num;
			}
			return result;
		}

		// Token: 0x060054A0 RID: 21664 RVA: 0x002E4BD8 File Offset: 0x002E2DD8
		private short ApplyCondition_ShowFristContent()
		{
			this._eventAction = SecretInformationProcessor_Event.EventAction.ShowFristContent;
			return -1;
		}

		// Token: 0x060054A1 RID: 21665 RVA: 0x002E4BF4 File Offset: 0x002E2DF4
		private short ApplyCondition_AskCharKeep()
		{
			int favorLimit = SecretInformationProcessor_Event.AskCharKeepFavorLimits[(int)this._character.GetBehaviorType()];
			sbyte favorLevel = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(this._characterId, this._taiwuId));
			int index = 0;
			bool flag = (int)favorLevel >= favorLimit;
			if (flag)
			{
				index = 1;
				short change = this.Processor.GetSecretInformationEffectConfig().OppositeFavorabilityDiffsWhenResult[(int)this._character.GetBehaviorType()];
				EventHelper.ChangeFavorabilityOptional(this._character, this._taiwu, change, 3);
			}
			return SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 0, index);
		}

		// Token: 0x060054A2 RID: 21666 RVA: 0x002E4C90 File Offset: 0x002E2E90
		private short ApplyCondition_AskCharRelease()
		{
			int favorLimit = SecretInformationProcessor_Event.AskCharReleaseFavorLimits[(int)this._character.GetBehaviorType()];
			sbyte favorLevel = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(this._characterId, this._taiwuId));
			int index = ((int)favorLevel >= favorLimit) ? 1 : 0;
			return SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 0, index);
		}

		// Token: 0x060054A3 RID: 21667 RVA: 0x002E4CF0 File Offset: 0x002E2EF0
		private short ApplyCondition_TaiwuFight()
		{
			bool flag = this._savedCombatData == null;
			if (flag)
			{
				bool flag2 = this._resultConfig.CombatConfigId == -1;
				if (flag2)
				{
					return -1;
				}
				this._savedCombatData = new SecretInformationProcessor_Event.CombatData(this._resultConfig, DomainManager.Character.HasGuard(this._characterId, this._character));
			}
			bool flag3 = !this.Check_IfCanFight(this._character, this._savedCombatData.CombatType) && this._savedCombatData.NoGuard;
			short result;
			if (flag3)
			{
				result = this._savedCombatData.CantFightResult;
			}
			else
			{
				this._eventAction = SecretInformationProcessor_Event.EventAction.StartCombat;
				result = -1;
			}
			return result;
		}

		// Token: 0x060054A4 RID: 21668 RVA: 0x002E4D90 File Offset: 0x002E2F90
		private short ApplyCondition_ChatFight()
		{
			SecretInformationEffectItem effectConfig = this.Processor.GetSecretInformationEffectConfig();
			bool flag = effectConfig.CombatType != -1;
			short result;
			if (flag)
			{
				sbyte combatType = CombatConfig.Instance.GetItem(effectConfig.CombatType).CombatType;
				int index = 0;
				bool flag2 = combatType == 2;
				if (flag2)
				{
					bool flag3 = this.Check_IfCanPanish(this._character, this._taiwu);
					if (flag3)
					{
						index = 1;
					}
					else
					{
						index = 2;
					}
				}
				SecretInformationAppliedResultItem combatConfig = SecretInformationAppliedResult.Instance.GetItem(SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 0, index));
				this._savedCombatData = new SecretInformationProcessor_Event.CombatData(combatConfig, DomainManager.Character.HasGuard(this._characterId, this._character));
				result = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 1, 0);
			}
			else
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x060054A5 RID: 21669 RVA: 0x002E4E60 File Offset: 0x002E3060
		private short ApplyCondition_RefuseKeep()
		{
			SecretInformationEffectItem effectConfig = this.Processor.GetSecretInformationEffectConfig();
			bool flag = this.Check_CharKillTaiwuBecauseOfRefuseKeep(this._character, this._taiwu, effectConfig.KillingProbOfRefuseKeepSecret) && effectConfig.CombatType != -1;
			short result;
			if (flag)
			{
				sbyte combatType = CombatConfig.Instance.GetItem(effectConfig.CombatType).CombatType;
				int index = 0;
				bool flag2 = combatType == 2;
				if (flag2)
				{
					index = 1;
				}
				SecretInformationAppliedResultItem combatConfig = SecretInformationAppliedResult.Instance.GetItem(SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 0, index));
				this._savedCombatData = new SecretInformationProcessor_Event.CombatData(combatConfig, DomainManager.Character.HasGuard(this._characterId, this._character));
				result = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 1, 0);
			}
			else
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x060054A6 RID: 21670 RVA: 0x002E4F2C File Offset: 0x002E312C
		private short ApplyCondition_RefuseKeep_Ori()
		{
			SecretInformationEffectItem effectConfig = this.Processor.GetSecretInformationEffectConfig();
			bool flag = this.Check_CharKillTaiwuBecauseOfRefuseKeep(this._character, this._taiwu, effectConfig.KillingProbOfRefuseKeepSecret) && this._resultConfig.CombatConfigId != -1;
			short result;
			if (flag)
			{
				this._savedCombatData = new SecretInformationProcessor_Event.CombatData(this._resultConfig, DomainManager.Character.HasGuard(this._characterId, this._character));
				result = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 1, 0);
			}
			else
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x060054A7 RID: 21671 RVA: 0x002E4FBC File Offset: 0x002E31BC
		private short ApplyCondition_TaiwuEscape()
		{
			this._toEscapeCharIdList.Add(this._taiwuId);
			return -1;
		}

		// Token: 0x060054A8 RID: 21672 RVA: 0x002E4FE4 File Offset: 0x002E31E4
		private short ApplyCondition_CharEscape()
		{
			this._toEscapeCharIdList.Add(this._characterId);
			return -1;
		}

		// Token: 0x060054A9 RID: 21673 RVA: 0x002E500C File Offset: 0x002E320C
		private short ApplyCondition_TaiwuSteal()
		{
			sbyte phase = EventHelper.GetStealActionPhase(this._taiwu, this._character, 100, false);
			short nextResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 0, (int)((phase > 4) ? 4 : phase));
			short lastResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 0, 4);
			bool flag = this.Check_IsRescure(lastResult);
			if (flag)
			{
				this.AddStealLifeRecord(this._taiwuId, this._characterId, this._argList[1], phase);
			}
			this._savedActionData = new SecretInformationProcessor_Event.ActionData(1, phase, new List<short>(), this._taiwuId, this._characterId, this._argList[1]);
			return nextResult;
		}

		// Token: 0x060054AA RID: 21674 RVA: 0x002E50BC File Offset: 0x002E32BC
		private short ApplyCondition_TaiwuScam()
		{
			sbyte phase = EventHelper.GetScamActionPhase(this._taiwu, this._character, 100, false);
			short nextResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 0, (int)((phase > 3) ? 3 : phase));
			short lastResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 1, 0);
			bool flag = this.Check_IsRescure(lastResult);
			if (flag)
			{
				this.AddScamLifeRecord(this._taiwuId, this._characterId, this._argList[1], phase);
			}
			this._savedActionData = new SecretInformationProcessor_Event.ActionData(2, phase, SecretInformationProcessor_Event.GetDataListFromConditionResult(this._resultConfig.SpecialConditionResultIds, 1), this._taiwuId, this._characterId, this._argList[1]);
			return nextResult;
		}

		// Token: 0x060054AB RID: 21675 RVA: 0x002E5178 File Offset: 0x002E3378
		private short ApplyCondition_TaiwuRob()
		{
			sbyte phase = EventHelper.GetRobActionPhase(this._taiwu, this._character, 100, false);
			short nextResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 0, (int)((phase > 4) ? 4 : phase));
			short lastResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 0, 0);
			bool flag = this.Check_IsRescure(lastResult);
			if (flag)
			{
				this.AddRobLifeRecord(this._taiwuId, this._characterId, this._argList[1], phase);
			}
			bool flag2 = this._resultConfig.CombatConfigId != -1;
			if (flag2)
			{
				this._savedCombatData = new SecretInformationProcessor_Event.CombatData(this._resultConfig, DomainManager.Character.HasGuard(this._characterId, this._character));
			}
			this._savedActionData = new SecretInformationProcessor_Event.ActionData(3, phase, new List<short>(), this._taiwuId, this._characterId, this._argList[1]);
			return nextResult;
		}

		// Token: 0x060054AC RID: 21676 RVA: 0x002E5268 File Offset: 0x002E3468
		private short ApplyCondition_TaiwuRescueEscape()
		{
			short nextResult = -1;
			bool flag = this._savedActionData != null;
			if (flag)
			{
				bool flag2 = this._savedActionData.Phase == 5;
				if (flag2)
				{
					nextResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, (int)this._savedActionData.ActionKey, 1);
				}
				else
				{
					nextResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, (int)this._savedActionData.ActionKey, 0);
					bool flag3 = this._resultConfig.CombatConfigId != -1;
					if (flag3)
					{
						this._savedCombatData = new SecretInformationProcessor_Event.CombatData(this._resultConfig, DomainManager.Character.HasGuard(this._characterId, this._character));
					}
				}
			}
			this._savedActionData = null;
			return nextResult;
		}

		// Token: 0x060054AD RID: 21677 RVA: 0x002E5328 File Offset: 0x002E3528
		private short ApplyCondition_CharRescue()
		{
			short nextResult;
			switch (this.GetCharRescueAction(this._characterId, this._taiwuId, this._argList[1]))
			{
			case 1:
			{
				sbyte phase = EventHelper.GetStealActionPhase(this._character, this._taiwu, 100, false);
				short lastResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 1, 1);
				bool flag = this.Check_IsRescure(lastResult);
				if (flag)
				{
					this.AddStealLifeRecord(this._characterId, this._taiwuId, this._argList[1], phase);
				}
				bool flag2 = phase == 5;
				if (flag2)
				{
					nextResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 1, 1);
					this._savedActionData = new SecretInformationProcessor_Event.ActionData(1, phase, new List<short>(), this._characterId, this._taiwuId, this._argList[1]);
				}
				else
				{
					bool flag3 = phase == 4;
					if (!flag3)
					{
						return -1;
					}
					nextResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 1, 0);
					SecretInformationAppliedResultItem combatConfig = SecretInformationAppliedResult.Instance.GetItem(SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 0, 0));
					bool flag4 = combatConfig != null && combatConfig.CombatConfigId != -1;
					if (flag4)
					{
						this._savedCombatData = new SecretInformationProcessor_Event.CombatData(combatConfig, false);
					}
					this._savedActionData = new SecretInformationProcessor_Event.ActionData(1, phase, new List<short>(), this._characterId, this._taiwuId, this._argList[1]);
				}
				break;
			}
			case 2:
			{
				sbyte phase2 = EventHelper.GetStealActionPhase(this._character, this._taiwu, 100, false);
				SecretInformationAppliedResultItem nextResultTin = SecretInformationAppliedResult.Instance.GetItem(SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 0, 1));
				short lastResult2 = SecretInformationProcessor_Event.GetDataFromConditionResult(nextResultTin.SpecialConditionResultIds, 0, 1);
				bool flag5 = this.Check_IsRescure(lastResult2);
				if (flag5)
				{
					this.AddScamLifeRecord(this._characterId, this._taiwuId, this._argList[1], phase2);
				}
				bool flag6 = phase2 >= 3;
				if (!flag6)
				{
					return -1;
				}
				nextResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 2, 0);
				this._savedActionData = new SecretInformationProcessor_Event.ActionData(2, phase2, SecretInformationProcessor_Event.GetDataListFromConditionResult(nextResultTin.SpecialConditionResultIds, 0), this._characterId, this._taiwuId, this._argList[1]);
				break;
			}
			case 3:
			{
				sbyte phase3 = EventHelper.GetRobActionPhase(this._character, this._taiwu, 100, false);
				SecretInformationAppliedResultItem nextResultTin2 = SecretInformationAppliedResult.Instance.GetItem(SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 0, 2));
				short lastResult3 = SecretInformationProcessor_Event.GetDataFromConditionResult(nextResultTin2.SpecialConditionResultIds, 0, 1);
				bool flag7 = this.Check_IsRescure(lastResult3);
				if (flag7)
				{
					this.AddRobLifeRecord(this._characterId, this._taiwuId, this._argList[1], phase3);
				}
				bool flag8 = phase3 >= 4;
				if (!flag8)
				{
					return -1;
				}
				this._savedCombatData = new SecretInformationProcessor_Event.CombatData(nextResultTin2, false);
				nextResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 3, 0);
				this._savedActionData = new SecretInformationProcessor_Event.ActionData(3, phase3, new List<short>(), this._characterId, this._taiwuId, this._argList[1]);
				break;
			}
			default:
				return -1;
			}
			return nextResult;
		}

		// Token: 0x060054AE RID: 21678 RVA: 0x002E568C File Offset: 0x002E388C
		private sbyte GetCharRescueAction(int saviorId, int kidnaperId, int prisonerId)
		{
			sbyte selectedDemandActionType = -1;
			GameData.Domains.Character.Character saviorChar;
			GameData.Domains.Character.Character kidnaperChar;
			GameData.Domains.Character.Character prisonerChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(saviorId, out saviorChar) || !DomainManager.Character.TryGetElement_Objects(kidnaperId, out kidnaperChar) || !DomainManager.Character.TryGetElement_Objects(prisonerId, out prisonerChar);
			sbyte result;
			if (flag)
			{
				result = selectedDemandActionType;
			}
			else
			{
				sbyte favorType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(saviorId, prisonerId));
				bool flag2 = favorType <= 1;
				if (flag2)
				{
					result = selectedDemandActionType;
				}
				else
				{
					bool flag3 = !this.Chech_PercentProb((int)(favorType * 20 - 40));
					if (flag3)
					{
						result = selectedDemandActionType;
					}
					else
					{
						sbyte behaviorType = saviorChar.GetBehaviorType();
						sbyte[] priorityList = AiHelper.PrioritizedActionConstants.RescueFriendOrFamilyActionPriorities[(int)behaviorType];
						foreach (sbyte actionType in priorityList)
						{
							int chance = (int)(60 + AiHelper.DemandActionType.ToPersonalityType[(int)actionType]);
							bool flag4 = !this.Chech_PercentProb(chance);
							if (!flag4)
							{
								selectedDemandActionType = actionType;
								break;
							}
						}
						bool flag5 = selectedDemandActionType == 3 && !this.Check_IfCanFight(saviorChar, 1);
						if (flag5)
						{
							selectedDemandActionType = 0;
						}
						result = selectedDemandActionType;
					}
				}
			}
			return result;
		}

		// Token: 0x060054AF RID: 21679 RVA: 0x002E5798 File Offset: 0x002E3998
		private short ApplyCondition_CharRescueEscape()
		{
			bool flag = this._savedActionData != null;
			short result;
			if (flag)
			{
				bool flag2 = this._savedActionData.Phase == 5;
				short nextResult;
				if (flag2)
				{
					nextResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, (int)this._savedActionData.ActionKey, 1);
				}
				else
				{
					nextResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, (int)this._savedActionData.ActionKey, 0);
					bool flag3 = this._resultConfig.CombatConfigId != -1;
					if (flag3)
					{
						this._savedCombatData = new SecretInformationProcessor_Event.CombatData(this._resultConfig, false);
					}
				}
				this._savedActionData = null;
				result = nextResult;
			}
			else
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x060054B0 RID: 21680 RVA: 0x002E5848 File Offset: 0x002E3A48
		private short ApplyCondition_StartFight()
		{
			this._eventAction = SecretInformationProcessor_Event.EventAction.StartCombat;
			return -1;
		}

		// Token: 0x060054B1 RID: 21681 RVA: 0x002E5864 File Offset: 0x002E3A64
		private short ApplyCondition_StartLifeSkillCombat()
		{
			this._eventAction = SecretInformationProcessor_Event.EventAction.StartLifeSkillCombat;
			return -1;
		}

		// Token: 0x060054B2 RID: 21682 RVA: 0x002E5880 File Offset: 0x002E3A80
		private short ApplyCondition_ChooseRope()
		{
			this._eventAction = SecretInformationProcessor_Event.EventAction.ChooseRope;
			return -1;
		}

		// Token: 0x060054B3 RID: 21683 RVA: 0x002E589C File Offset: 0x002E3A9C
		private short ApplyCondition_CharWin(EventArgBox argBox)
		{
			sbyte combatType = this._savedCombatData.CombatType;
			this._savedCombatData = null;
			bool flag = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 2, 0) == -1;
			short result;
			if (flag)
			{
				short infoKey = this.GetCharAction_CharWin_InPublic(this._character) ? 1 : 95;
				this.SecretInformationMaker_Box(argBox, infoKey, this._characterId, this._taiwuId, -1);
				result = -1;
			}
			else
			{
				int endindex = this.GetCharAction_CharWin(this._character, this._taiwu, combatType, false);
				int endindex2 = this.GetCharAction_CharWin_InPublic(this._character) ? 0 : 1;
				short nextResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, endindex, endindex2);
				bool flag2 = nextResult == -1;
				if (flag2)
				{
					nextResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, endindex, 0);
				}
				result = nextResult;
			}
			return result;
		}

		// Token: 0x060054B4 RID: 21684 RVA: 0x002E596C File Offset: 0x002E3B6C
		private short ApplyCondition_CharJudge(EventArgBox argBox)
		{
			sbyte combatType = this._savedCombatData.CombatType;
			this._savedCombatData = null;
			bool flag = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 2, 0) == -1;
			short result;
			if (flag)
			{
				this.SecretInformationMaker_Box(argBox, 1, this._characterId, this._taiwuId, -1);
				result = -1;
			}
			else
			{
				int endindex = this.GetCharAction_CharWin(this._character, this._taiwu, combatType, false);
				short nextResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, endindex, 0);
				result = nextResult;
			}
			return result;
		}

		// Token: 0x060054B5 RID: 21685 RVA: 0x002E59F4 File Offset: 0x002E3BF4
		private short ApplyCondition_CharKidnap(EventArgBox argBox)
		{
			short infoKey = this.GetCharAction_CharWin_InPublic(this._character) ? 2 : 96;
			this.SecretInformationMaker_Box(argBox, infoKey, this._characterId, this._argList[1], -1);
			return -1;
		}

		// Token: 0x060054B6 RID: 21686 RVA: 0x002E5A38 File Offset: 0x002E3C38
		private int GetCharAction_CharWin(GameData.Domains.Character.Character selfChar, GameData.Domains.Character.Character targetChar, sbyte combatType, bool enableKidnap = false)
		{
			sbyte killBaseChance = 60;
			sbyte kidnapBaseChance = 60;
			sbyte releaseBaseChance = 60;
			bool flag = combatType == 1;
			if (flag)
			{
				killBaseChance = -100;
				kidnapBaseChance = -100;
				releaseBaseChance = 100;
			}
			sbyte behaviorType = selfChar.GetBehaviorType();
			AiHelper.CombatResultHandleType[] priorities = AiHelper.NpcCombat.ResultHandleTypePriorities[(int)behaviorType];
			int action = 0;
			for (int i = 0; i < 3; i++)
			{
				switch (priorities[i])
				{
				case AiHelper.CombatResultHandleType.Kill:
				{
					bool flag2 = this.Chech_PercentProb((int)(killBaseChance + EventHelper.GetRolePersonality(selfChar, 3)));
					if (flag2)
					{
						action = 1;
					}
					break;
				}
				case AiHelper.CombatResultHandleType.Kidnap:
				{
					bool flag3 = this.Chech_PercentProb((int)(kidnapBaseChance + EventHelper.GetRolePersonality(selfChar, 1)));
					if (flag3)
					{
						action = (enableKidnap ? 2 : 1);
					}
					break;
				}
				case AiHelper.CombatResultHandleType.Release:
				{
					bool flag4 = this.Chech_PercentProb((int)(releaseBaseChance + EventHelper.GetRolePersonality(selfChar, 0)));
					if (flag4)
					{
						action = 0;
					}
					break;
				}
				}
			}
			return action;
		}

		// Token: 0x060054B7 RID: 21687 RVA: 0x002E5B10 File Offset: 0x002E3D10
		private bool GetCharAction_CharWin_InPublic(GameData.Domains.Character.Character character)
		{
			sbyte behaviorType = character.GetBehaviorType();
			int prob = (int)AiHelper.NpcCombat.HandleEnemyInPublicChance[(int)behaviorType];
			return this.Chech_PercentProb(prob);
		}

		// Token: 0x060054B8 RID: 21688 RVA: 0x002E5B38 File Offset: 0x002E3D38
		private short ApplyCondition_KidnapWithRope(EventArgBox argBox)
		{
			sbyte combatType = 1;
			bool flag = this._savedActionData != null;
			if (flag)
			{
				combatType = this._savedCombatData.CombatType;
				this._savedCombatData = null;
			}
			ItemKey ropeKey;
			bool flag2 = argBox.Get<ItemKey>("ItemKeySeizeCharacterInCombat", out ropeKey) && CombatDomain.CheckRopeHitOutOfCombat(DomainManager.TaiwuEvent.MainThreadDataContext.Random, this._taiwu, this._character, combatType, true, (int)ItemTemplateHelper.GetGrade(ropeKey.ItemType, ropeKey.TemplateId));
			short nextResult;
			if (flag2)
			{
				nextResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 0, 1);
			}
			else
			{
				nextResult = SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 0, 0);
			}
			return nextResult;
		}

		// Token: 0x060054B9 RID: 21689 RVA: 0x002E5BEC File Offset: 0x002E3DEC
		private short ApplyCondition_TaiwuDeleteInfomation()
		{
			this._toDiscardCharIdList.Add(new ValueTuple<int, int>(this._taiwuId, this._metaDataId));
			return -1;
		}

		// Token: 0x060054BA RID: 21690 RVA: 0x002E5C1C File Offset: 0x002E3E1C
		private short ApplyCondition_CharDeleteInfomation()
		{
			this._toDiscardCharIdList.Add(new ValueTuple<int, int>(this._characterId, this._metaDataId));
			return -1;
		}

		// Token: 0x060054BB RID: 21691 RVA: 0x002E5C4C File Offset: 0x002E3E4C
		private short ApplyCondition_TaiwuAdore()
		{
			bool result = this.ApplyRelation_Adore(this._taiwuId, this._characterId);
			this._savedRelationChangeData = new SecretInformationProcessor_Event.RelationChangeData(this._characterId, this._taiwuId, 16384, false, result);
			return -1;
		}

		// Token: 0x060054BC RID: 21692 RVA: 0x002E5C90 File Offset: 0x002E3E90
		private short ApplyCondition_CharAdore()
		{
			bool result = this.ApplyRelation_Adore(this._characterId, this._taiwuId);
			this._savedRelationChangeData = new SecretInformationProcessor_Event.RelationChangeData(this._characterId, this._taiwuId, 16384, false, result);
			return -1;
		}

		// Token: 0x060054BD RID: 21693 RVA: 0x002E5CD4 File Offset: 0x002E3ED4
		private bool ApplyRelation_Adore(int selfId, int targetId)
		{
			GameData.Domains.Character.Character selfChar;
			GameData.Domains.Character.Character targetChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(selfId, out selfChar) || !DomainManager.Character.TryGetElement_Objects(targetId, out targetChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !RelationTypeHelper.AllowAddingAdoredRelation(selfId, targetId);
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
					bool targetIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(targetId);
					GameData.Domains.Character.Character.ApplyAddRelation_Adore(DomainManager.TaiwuEvent.MainThreadDataContext, selfChar, selfChar, selfChar.GetBehaviorType(), false, selfIsTaiwuPeople, targetIsTaiwuPeople);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060054BE RID: 21694 RVA: 0x002E5D60 File Offset: 0x002E3F60
		public bool ApplyRelation_SeverAdore(int selfId, int targetId)
		{
			GameData.Domains.Character.Character selfChar;
			GameData.Domains.Character.Character targetChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(selfId, out selfChar) || !DomainManager.Character.TryGetElement_Objects(targetId, out targetChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !DomainManager.Character.HasRelation(selfId, targetId, 16384);
				if (flag2)
				{
					result = false;
				}
				else
				{
					DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
					sbyte charBehaviorType = selfChar.GetBehaviorType();
					bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
					GameData.Domains.Character.Character.ApplySeverAdore(context, selfChar, targetChar, charBehaviorType, selfIsTaiwuPeople);
					bool flag3 = DomainManager.Taiwu.GetTaiwuCharId() != selfId && this.Chech_PercentProb(SecretInformationProcessor_Event.LoveRelationValue.MakeEnemyWhenNotAdore[(int)charBehaviorType]);
					if (flag3)
					{
						GameData.Domains.Character.Character.ApplyAddRelation_Enemy(context, targetChar, selfChar, selfIsTaiwuPeople, 3);
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060054BF RID: 21695 RVA: 0x002E5E1C File Offset: 0x002E401C
		public bool ApplyRelation_SeverSpouse(int selfId, int targetId)
		{
			GameData.Domains.Character.Character selfChar;
			GameData.Domains.Character.Character targetChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(selfId, out selfChar) || !DomainManager.Character.TryGetElement_Objects(targetId, out targetChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !DomainManager.Character.HasRelation(selfId, targetId, 1024);
				if (flag2)
				{
					result = false;
				}
				else
				{
					DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
					sbyte charBehaviorType = selfChar.GetBehaviorType();
					bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfId);
					bool targetIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(targetId);
					GameData.Domains.Character.Character.ApplySeverHusbandOrWife(context, selfChar, targetChar, charBehaviorType, selfIsTaiwuPeople, targetIsTaiwuPeople);
					bool flag3 = DomainManager.Taiwu.GetTaiwuCharId() != selfId && this.Chech_PercentProb(SecretInformationProcessor_Event.LoveRelationValue.MakeEnemyWhenDivorce[(int)charBehaviorType]);
					if (flag3)
					{
						GameData.Domains.Character.Character.ApplyAddRelation_Enemy(context, targetChar, selfChar, selfIsTaiwuPeople, 3);
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060054C0 RID: 21696 RVA: 0x002E5EEC File Offset: 0x002E40EC
		private short ApplyCondition_Forgive(EventArgBox argBox)
		{
			List<int> relation = this.Processor.GetAllSecretInformationRelationsOfCharacter(this._characterId, true, false);
			List<int> breakList = new List<int>();
			List<int> forgiveList = new List<int>();
			List<int> endAdoreList = new List<int>();
			List<int> stillAdoreList = new List<int>();
			Dictionary<int, GameData.Domains.Character.Character> actorList = this.Processor.GetActiveActorList_WithActorIndex();
			List<int> loveRelation = new List<int>
			{
				10,
				11,
				15
			};
			List<int> AdoredRelation = new List<int>
			{
				13,
				14,
				15
			};
			for (int i = 0; i < 3; i++)
			{
				GameData.Domains.Character.Character actor;
				bool flag = actorList.TryGetValue(i, out actor);
				if (flag)
				{
					bool flag2 = relation.Contains(loveRelation[i]);
					if (flag2)
					{
						bool flag3 = DomainManager.Character.HasRelation(actor.GetId(), this._characterId, 1024);
						if (flag3)
						{
							bool secsess = !this.Check_IfForgive(this._characterId, actor.GetId(), 2) && this.ApplyRelation_SeverSpouse(this._characterId, actor.GetId());
							bool flag4 = secsess;
							if (flag4)
							{
								breakList.Add(actor.GetId());
							}
							else
							{
								forgiveList.Add(actor.GetId());
								this.ApplyEffect_Forgive(this._character, actor, 2);
							}
						}
						else
						{
							bool flag5 = DomainManager.Character.HasRelation(this._characterId, actor.GetId(), 16384);
							if (flag5)
							{
								bool flag6 = !this.Check_IfForgive(this._characterId, actor.GetId(), 1) && this.MakeNewInfo_BreakupWithLover(this._characterId, actor.GetId());
								if (flag6)
								{
									breakList.Add(actor.GetId());
								}
								else
								{
									forgiveList.Add(actor.GetId());
									this.ApplyEffect_Forgive(this._character, actor, 1);
								}
							}
						}
					}
					else
					{
						bool flag7 = relation.Contains(AdoredRelation[i]);
						if (flag7)
						{
							bool flag8 = DomainManager.Character.HasRelation(this._characterId, actor.GetId(), 16384);
							if (flag8)
							{
								bool flag9 = !this.Check_IfForgive(this._characterId, actor.GetId(), 0) && this.ApplyRelation_SeverAdore(this._characterId, actor.GetId());
								if (flag9)
								{
									endAdoreList.Add(actor.GetId());
								}
								else
								{
									stillAdoreList.Add(actor.GetId());
									this.ApplyEffect_Forgive(this._character, actor, 0);
								}
							}
						}
					}
				}
			}
			int indexLove = 0;
			int index0 = 2;
			int index = 2;
			bool flag10 = breakList.Contains(this._taiwuId);
			if (flag10)
			{
				index0 = 0;
				index = 0;
			}
			else
			{
				bool flag11 = forgiveList.Contains(this._taiwuId);
				if (flag11)
				{
					index0 = 0;
					index = 1;
				}
				else
				{
					bool flag12 = endAdoreList.Contains(this._taiwuId);
					if (flag12)
					{
						indexLove = 1;
						index0 = 0;
						index = 0;
					}
					else
					{
						bool flag13 = stillAdoreList.Contains(this._taiwuId);
						if (flag13)
						{
							indexLove = 1;
							index0 = 0;
							index = 1;
						}
						else
						{
							bool flag14 = breakList.Count != 0;
							if (flag14)
							{
								index = 0;
								argBox.Set("breakTargetCharacterId", breakList[0]);
							}
							else
							{
								bool flag15 = forgiveList.Count != 0;
								if (flag15)
								{
									index = 1;
								}
								else
								{
									bool flag16 = endAdoreList.Count != 0;
									if (flag16)
									{
										indexLove = 1;
										index = 0;
										argBox.Set("breakTargetCharacterId", endAdoreList[0]);
									}
									else
									{
										bool flag17 = stillAdoreList.Count != 0;
										if (flag17)
										{
											indexLove = 1;
											index = 1;
										}
										else
										{
											bool flag18 = this.Processor.IsTaiwuSecretInformationActor();
											if (flag18)
											{
												index0 = 0;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, index0 + indexLove, index);
		}

		// Token: 0x060054C1 RID: 21697 RVA: 0x002E62CC File Offset: 0x002E44CC
		private short ApplyCondition_NotForgiveRape()
		{
			int index0 = (this._taiwuId == this._argList[0]) ? 0 : 1;
			int index = 2;
			bool flag = DomainManager.Character.HasRelation(this._characterId, this._argList[0], 1024);
			if (flag)
			{
				bool flag2 = this.ApplyRelation_SeverSpouse(this._characterId, this._argList[0]);
				if (flag2)
				{
					index = 0;
				}
			}
			else
			{
				bool flag3 = DomainManager.Character.HasRelation(this._characterId, this._argList[0], 16384);
				if (flag3)
				{
					bool flag4 = DomainManager.Character.HasRelation(this._argList[0], this._characterId, 16384);
					if (flag4)
					{
						bool flag5 = this.MakeNewInfo_BreakupWithLover(this._characterId, this._argList[0]);
						if (flag5)
						{
							index = 0;
						}
					}
					else
					{
						bool flag6 = this.ApplyRelation_SeverAdore(this._characterId, this._argList[0]);
						if (flag6)
						{
							index = 1;
						}
					}
				}
			}
			bool flag7 = index == 2 && this._taiwuId == this._argList[0];
			if (flag7)
			{
				bool flag8 = this.Processor.ConditionBox(23, this._characterId, this._taiwuId, -1, -1);
				if (flag8)
				{
					int combatIndex = 0;
					bool flag9 = this.Check_IfCanPanish(this._character, this._taiwu);
					if (flag9)
					{
						combatIndex = 1;
					}
					SecretInformationAppliedResultItem combatConfig = SecretInformationAppliedResult.Instance.GetItem(SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 2, combatIndex));
					bool flag10 = combatConfig != null;
					if (flag10)
					{
						this._savedCombatData = new SecretInformationProcessor_Event.CombatData(combatConfig, false);
						index = 3;
					}
				}
			}
			return SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, index0, index);
		}

		// Token: 0x060054C2 RID: 21698 RVA: 0x002E6490 File Offset: 0x002E4690
		private void ApplyEffect_Forgive(GameData.Domains.Character.Character selfChar, GameData.Domains.Character.Character targetChar, int loveRelationType)
		{
			EventHelper.ChangeRoleHappiness(selfChar, -5);
			short change = (short)(-3000 * (loveRelationType + 1));
			EventHelper.ChangeFavorabilityOptional(selfChar, targetChar, change, 3);
		}

		// Token: 0x060054C3 RID: 21699 RVA: 0x002E64BC File Offset: 0x002E46BC
		private short ApplyCondition_AskCharBreakup(EventArgBox argBox)
		{
			int index = 0;
			List<int> actors = this.Processor.GetActiveActorIdList(new List<int>
			{
				this._taiwuId,
				this._characterId
			});
			bool flag = actors.Count == 0;
			if (flag)
			{
				index = 2;
			}
			foreach (int actorId in actors)
			{
				GameData.Domains.Character.Character actor = DomainManager.Character.GetElement_Objects(actorId);
				bool flag2 = DomainManager.Character.HasRelation(actorId, this._characterId, 1024);
				if (flag2)
				{
					argBox.Set("breakTargetCharacterId", actorId);
					bool secsess = this.Check_IfBreak(this._taiwuId, this._characterId, actorId, 2) && this.ApplyRelation_SeverSpouse(this._characterId, actorId);
					bool flag3 = secsess;
					if (flag3)
					{
						index = 1;
					}
					this.ApplyFavorChange_Break(this._taiwuId, this._characterId, actorId, secsess);
					break;
				}
				bool flag4 = DomainManager.Character.HasRelation(actorId, this._characterId, 16384) && DomainManager.Character.HasRelation(this._characterId, actorId, 16384);
				if (flag4)
				{
					argBox.Set("breakTargetCharacterId", actorId);
					bool secsess2 = this.Check_IfBreak(this._taiwuId, this._characterId, actorId, 1) && this.MakeNewInfo_BreakupWithLover(this._characterId, actorId);
					bool flag5 = secsess2;
					if (flag5)
					{
						index = 1;
					}
					this.ApplyFavorChange_Break(this._taiwuId, this._characterId, actorId, secsess2);
					break;
				}
			}
			return SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, 0, index);
		}

		// Token: 0x060054C4 RID: 21700 RVA: 0x002E6690 File Offset: 0x002E4890
		private short ApplyCondition_BreakupWithChar()
		{
			int index0 = 3;
			int index = 0;
			bool flag = DomainManager.Character.HasRelation(this._taiwuId, this._characterId, 1024);
			if (flag)
			{
				bool flag2 = this.Check_IfForgive(this._characterId, this._taiwuId, 2);
				if (flag2)
				{
					index0 = 0;
				}
				else
				{
					index0 = 1;
					this.ApplyRelation_SeverSpouse(this._taiwuId, this._characterId);
				}
			}
			else
			{
				bool flag3 = DomainManager.Character.HasRelation(this._taiwuId, this._characterId, 16384);
				if (flag3)
				{
					bool flag4 = DomainManager.Character.HasRelation(this._characterId, this._taiwuId, 16384);
					if (flag4)
					{
						bool flag5 = this.Check_IfForgive(this._characterId, this._taiwuId, 1);
						if (flag5)
						{
							index0 = 0;
						}
						else
						{
							index0 = 1;
							this.MakeNewInfo_BreakupWithLover(this._taiwuId, this._characterId);
						}
					}
					else
					{
						index0 = 2;
						this.ApplyRelation_SeverAdore(this._taiwuId, this._characterId);
					}
				}
			}
			return SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, index0, index);
		}

		// Token: 0x060054C5 RID: 21701 RVA: 0x002E67AC File Offset: 0x002E49AC
		private short ApplyCondition_ForceBreakupWithChar()
		{
			int index0 = 2;
			int index = 0;
			bool flag = DomainManager.Character.HasRelation(this._taiwuId, this._characterId, 1024);
			if (flag)
			{
				EventHelper.ChangeFavorabilityOptional(this._character, this._taiwu, -6000, 3);
				EventHelper.ChangeFavorabilityOptional(this._taiwu, this._character, -6000, 3);
				bool flag2 = this.Check_IfForgive(this._characterId, this._taiwuId, 2);
				if (flag2)
				{
					index0 = 0;
					this.ApplyRelation_SeverAdore(this._taiwuId, this._characterId);
					EventHelper.ChangeFavorabilityOptional(this._character, this._taiwu, -6000, 3);
					EventHelper.ChangeFavorabilityOptional(this._taiwu, this._character, -6000, 3);
				}
				else
				{
					index0 = 1;
					this.ApplyRelation_SeverSpouse(this._taiwuId, this._characterId);
				}
			}
			else
			{
				bool flag3 = DomainManager.Character.HasRelation(this._taiwuId, this._characterId, 16384) && DomainManager.Character.HasRelation(this._characterId, this._taiwuId, 16384);
				if (flag3)
				{
					EventHelper.ChangeFavorabilityOptional(this._character, this._taiwu, -6000, 3);
					EventHelper.ChangeFavorabilityOptional(this._taiwu, this._character, -6000, 3);
					bool flag4 = this.Check_IfForgive(this._characterId, this._taiwuId, 2);
					if (flag4)
					{
						index0 = 0;
						this.ApplyRelation_SeverAdore(this._taiwuId, this._characterId);
						EventHelper.ChangeFavorabilityOptional(this._character, this._taiwu, -6000, 3);
						EventHelper.ChangeFavorabilityOptional(this._taiwu, this._character, -6000, 3);
					}
					else
					{
						index0 = 1;
						this.MakeNewInfo_BreakupWithLover(this._taiwuId, this._characterId);
					}
				}
			}
			return SecretInformationProcessor_Event.GetDataFromConditionResult(this._resultConfig.SpecialConditionResultIds, index0, index);
		}

		// Token: 0x060054C6 RID: 21702 RVA: 0x002E6998 File Offset: 0x002E4B98
		private bool Check_IfForgive(int selfId, int targetId, int relationIndex)
		{
			GameData.Domains.Character.Character selfChar;
			GameData.Domains.Character.Character targetChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(selfId, out selfChar) || !DomainManager.Character.TryGetElement_Objects(targetId, out targetChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte selfBehaviorType = selfChar.GetBehaviorType();
				int minSelfFavorabilityReq = 0;
				switch (relationIndex)
				{
				case 0:
					minSelfFavorabilityReq = SecretInformationProcessor_Event.LoveRelationValue.FavorOfNotAdore[(int)selfBehaviorType];
					break;
				case 1:
					minSelfFavorabilityReq = SecretInformationProcessor_Event.LoveRelationValue.FavorOfBreakUp[(int)selfBehaviorType];
					break;
				case 2:
					minSelfFavorabilityReq = SecretInformationProcessor_Event.LoveRelationValue.FavorOfDivorce[(int)selfBehaviorType];
					break;
				}
				sbyte favorLevel = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(selfId, targetId));
				result = ((int)favorLevel > minSelfFavorabilityReq);
			}
			return result;
		}

		// Token: 0x060054C7 RID: 21703 RVA: 0x002E6A34 File Offset: 0x002E4C34
		private bool Check_IfBreak(int selfId, int targetId, int secTargetId, int loveRelationType)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(targetId);
			sbyte favorLevel = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(targetId, selfId));
			bool flag = favorLevel > 0;
			bool result;
			if (flag)
			{
				result = !this.Check_IfForgive(targetId, secTargetId, loveRelationType);
			}
			else
			{
				sbyte calmValue = EventHelper.GetRolePersonality(character, 0);
				result = this.Chech_PercentProb((int)(30 + calmValue * 3));
			}
			return result;
		}

		// Token: 0x060054C8 RID: 21704 RVA: 0x002E6A94 File Offset: 0x002E4C94
		private void ApplyFavorChange_Break(int selfId, int targetId, int secTargetId, bool IfBreak)
		{
			GameData.Domains.Character.Character selfChar;
			GameData.Domains.Character.Character targetChar;
			GameData.Domains.Character.Character secTargetCahr;
			bool flag = !DomainManager.Character.TryGetElement_Objects(selfId, out selfChar) || !DomainManager.Character.TryGetElement_Objects(targetId, out targetChar) || !DomainManager.Character.TryGetElement_Objects(secTargetId, out secTargetCahr);
			if (!flag)
			{
				sbyte favorLevel = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(targetId, secTargetId));
				sbyte level = Math.Max(favorLevel, 0);
				int favor = (-3000 + (int)level * -1500) / 2;
				EventHelper.ChangeFavorabilityOptional(targetChar, selfChar, (short)favor, 3);
				if (IfBreak)
				{
					EventHelper.ChangeRoleHappiness(targetChar, (int)(-(int)(level + 3)));
				}
			}
		}

		// Token: 0x060054C9 RID: 21705 RVA: 0x002E6B2C File Offset: 0x002E4D2C
		public static void ChangeFavorability(int charAId, int charBId, int deltaFavor)
		{
			bool flag = charAId == charBId;
			if (!flag)
			{
				GameData.Domains.Character.Character characterA;
				bool flag2 = !InformationDomain.CheckTuringTest(charAId, out characterA);
				if (!flag2)
				{
					GameData.Domains.Character.Character characterB;
					bool flag3 = !InformationDomain.CheckTuringTest(charBId, out characterB);
					if (!flag3)
					{
						int curDelta = Math.Clamp(deltaFavor, -30000, 30000);
						EventHelper.ChangeFavorabilityOptional(characterA, characterB, (short)curDelta, 3);
					}
				}
			}
		}

		// Token: 0x060054CA RID: 21706 RVA: 0x002E6B88 File Offset: 0x002E4D88
		public bool GetEventAction_After(EventArgBox argBox)
		{
			short nextResultId = -1;
			bool result = false;
			switch (this._eventAction)
			{
			case SecretInformationProcessor_Event.EventAction.StartCombat:
				nextResultId = this.DoAfterAction_Combat(argBox);
				break;
			case SecretInformationProcessor_Event.EventAction.StartLifeSkillCombat:
				nextResultId = this.DoAfterAction_LifeSkillCombat(argBox);
				break;
			case SecretInformationProcessor_Event.EventAction.ChooseRope:
				result = true;
				break;
			}
			this.ResultIndex = nextResultId;
			return result;
		}

		// Token: 0x060054CB RID: 21707 RVA: 0x002E6BE4 File Offset: 0x002E4DE4
		private short DoAfterAction_Combat(EventArgBox argBox)
		{
			bool flag = this._savedCombatData == null;
			short result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				sbyte combatResult = -1;
				bool flag2 = !argBox.Get("CombatResult", ref combatResult);
				if (flag2)
				{
					result = -1;
				}
				else
				{
					this.AddLifeRecord_After(CombatResultType.IsPlayerWin(combatResult), false);
					bool flag3 = !this._savedCombatData.NoGuard && (combatResult == 0 || combatResult == 5);
					if (flag3)
					{
						combatResult = 3;
					}
					int avoidDeathId = DomainManager.Character.GetAvoidDeathCharId();
					bool flag4 = this._taiwuId == avoidDeathId && (combatResult == 4 || combatResult == 1);
					if (flag4)
					{
						combatResult = 7;
					}
					bool flag5 = this._characterId == avoidDeathId && (combatResult == 5 || combatResult == 0);
					if (flag5)
					{
						combatResult = 8;
					}
					int prisonerId = -1;
					ItemKey ropeKey;
					bool flag6 = this._savedCombatData.NoGuard && argBox.Get<ItemKey>("ItemKeySeizeCharacterInCombat", out ropeKey) && argBox.Get("CharIdSeizedInCombat", ref prisonerId) && prisonerId == this._characterId;
					if (flag6)
					{
						combatResult = 6;
					}
					argBox.Remove<int>("CharIdSeizedInCombat");
					argBox.Remove<int>("CombatResult");
					result = this._savedCombatData.CombatResult[(int)combatResult];
				}
			}
			return result;
		}

		// Token: 0x060054CC RID: 21708 RVA: 0x002E6D14 File Offset: 0x002E4F14
		private short DoAfterAction_LifeSkillCombat(EventArgBox argBox)
		{
			bool flag = this._savedActionData == null;
			short result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool winLifeSkill = false;
				bool flag2 = !argBox.Get("WinState", ref winLifeSkill);
				if (flag2)
				{
					result = -1;
				}
				else
				{
					this.AddLifeRecord_After(winLifeSkill, false);
					argBox.Remove<int>("WinState");
					int index = winLifeSkill ? 0 : 1;
					result = this._savedActionData.CombatResult[index];
				}
			}
			return result;
		}

		// Token: 0x060054CD RID: 21709 RVA: 0x002E6D84 File Offset: 0x002E4F84
		private void AddLifeRecord_After(bool taiwuWin, bool isCombat = false)
		{
			bool flag = this._savedActionData == null;
			if (!flag)
			{
				bool flag2 = this._savedActionData.ActionKey == 2 && !isCombat;
				if (flag2)
				{
					bool flag3 = this._savedActionData.Phase < 3;
					if (!flag3)
					{
						Location location = this._taiwu.GetLocation();
						int currDate = DomainManager.World.GetCurrDate();
						LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
						bool saviorWin = (this._taiwuId == this._savedActionData.SaviorId) ? taiwuWin : (!taiwuWin);
						bool flag4 = saviorWin;
						if (flag4)
						{
							bool flag5 = this._savedActionData.Phase == 5;
							if (flag5)
							{
								lifeRecordCollection.AddRescueKidnappedCharacterWithWitSucceedAndEscaped(this._savedActionData.SaviorId, currDate, this._savedActionData.KidnaperId, location, this._savedActionData.PrisonerId);
							}
							else
							{
								lifeRecordCollection.AddRescueKidnappedCharacterWithWitSucceed(this._savedActionData.SaviorId, currDate, this._savedActionData.KidnaperId, location, this._savedActionData.PrisonerId);
							}
						}
						else
						{
							lifeRecordCollection.AddRescueKidnappedCharacterWithWitFail4(this._savedActionData.SaviorId, currDate, this._savedActionData.KidnaperId, location, this._savedActionData.PrisonerId);
						}
					}
				}
				else
				{
					bool flag6 = this._savedActionData.ActionKey == 3 && isCombat;
					if (flag6)
					{
						bool flag7 = this._savedActionData.Phase < 4;
						if (!flag7)
						{
							Location location2 = this._taiwu.GetLocation();
							int currDate2 = DomainManager.World.GetCurrDate();
							LifeRecordCollection lifeRecordCollection2 = DomainManager.LifeRecord.GetLifeRecordCollection();
							bool saviorWin2 = (this._taiwuId == this._savedActionData.SaviorId) ? taiwuWin : (!taiwuWin);
							bool flag8 = saviorWin2;
							if (flag8)
							{
								lifeRecordCollection2.AddRescueKidnappedCharacterWithForceSucceed(this._savedActionData.SaviorId, currDate2, this._savedActionData.KidnaperId, location2, this._savedActionData.PrisonerId);
							}
							else
							{
								lifeRecordCollection2.AddRescueKidnappedCharacterWithForceFail4(this._savedActionData.SaviorId, currDate2, this._savedActionData.KidnaperId, location2, this._savedActionData.PrisonerId);
							}
						}
					}
				}
			}
		}

		// Token: 0x060054CE RID: 21710 RVA: 0x002E6FA4 File Offset: 0x002E51A4
		public unsafe TaiwuEventOption[] MakeSecretInformationSelections(List<short> TemplateIdList, EventArgBox argBox, TaiwuEvent eventData, string extraKey = "")
		{
			SecretInformationProcessor_Event.<>c__DisplayClass133_0 CS$<>8__locals1 = new SecretInformationProcessor_Event.<>c__DisplayClass133_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.eventData = eventData;
			List<TaiwuEventOption> result = new List<TaiwuEventOption>();
			CS$<>8__locals1.eventData.EventConfig.EscOptionKey = string.Empty;
			foreach (short id in TemplateIdList)
			{
				SecretInformationProcessor_Event.<>c__DisplayClass133_1 CS$<>8__locals2 = new SecretInformationProcessor_Event.<>c__DisplayClass133_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				SecretInformationAppliedSelectionItem selectionConfig = SecretInformationAppliedSelection.Instance.GetItem(id);
				bool flag = selectionConfig == null;
				if (!flag)
				{
					CS$<>8__locals2.selection = new TaiwuEventOption();
					CS$<>8__locals2.selection.GetExtraFormatLanguageKeys = (() => null);
					CS$<>8__locals2.selection.OptionAvailableConditions = new List<TaiwuEventOptionConditionBase>();
					CS$<>8__locals2.selection.OptionConsumeInfos = new List<OptionConsumeInfo>();
					CS$<>8__locals2.selection.SetContent("BlankSlection");
					CS$<>8__locals2.content = ((selectionConfig.Text != null) ? selectionConfig.Text : string.Empty);
					string extraContent = string.Empty;
					CS$<>8__locals2.selection.ArgBox = argBox;
					short[] behaviorRequire = selectionConfig.PlayerBehaviorTypeIds;
					bool flag2 = DomainManager.World.GetRestrictOptionsBehaviorType() && behaviorRequire != null && behaviorRequire.Length != 0;
					if (flag2)
					{
						CS$<>8__locals2.selection.OptionAvailableConditions.Add(new OptionConditionBehaviorTypes(15, (sbyte)behaviorRequire[0], (behaviorRequire.Length > 1) ? ((sbyte)behaviorRequire[1]) : -1, (behaviorRequire.Length > 2) ? ((sbyte)behaviorRequire[2]) : -1, (behaviorRequire.Length > 3) ? ((sbyte)behaviorRequire[3]) : -1, (behaviorRequire.Length > 4) ? ((sbyte)behaviorRequire[4]) : -1, new Func<sbyte, sbyte, sbyte, sbyte, sbyte, bool>(OptionConditionMatcher.TaiwuIsBehaviorType)));
					}
					bool flag3 = selectionConfig.TimeCost > 0;
					if (flag3)
					{
						CS$<>8__locals2.selection.OptionAvailableConditions.Add(new OptionConditionSbyte(2, selectionConfig.TimeCost, new Func<sbyte, bool>(OptionConditionMatcher.MovePointMore)));
						CS$<>8__locals2.selection.OptionConsumeInfos.Add(new OptionConsumeInfo(8, (int)selectionConfig.TimeCost, true));
					}
					bool flag4 = selectionConfig.FavorabilityCondition > -6;
					if (flag4)
					{
						CS$<>8__locals2.selection.OptionAvailableConditions.Add(new OptionConditionFavor(5, selectionConfig.FavorabilityCondition, new Func<int, int, sbyte, bool>(OptionConditionMatcher.FavorAtLeast)));
					}
					bool isMainAttributeCost = false;
					CS$<>8__locals2.propertyId = selectionConfig.MainAttributeCost.PropertyId;
					CS$<>8__locals2.value = selectionConfig.MainAttributeCost.Value;
					bool flag5 = CS$<>8__locals2.value > 0 && CS$<>8__locals2.propertyId >= 0 && CS$<>8__locals2.propertyId < 6;
					if (flag5)
					{
						isMainAttributeCost = true;
						List<string> attributeKeyList = new List<string>
						{
							"LK_Main_Attribute_Strength",
							"LK_Main_Attribute_Dexterity",
							"LK_Main_Attribute_Concentration",
							"LK_Main_Attribute_Vitality",
							"LK_Main_Attribute_Energy",
							"LK_Main_Attribute_Intelligence"
						};
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(107, 2);
						defaultInterpolatedStringHandler.AppendLiteral("<color=#darkgrey>[<Language Key=Event_ForSecretInformationAppliedSelection_Cost/><Language Key=");
						defaultInterpolatedStringHandler.AppendFormatted(attributeKeyList[(int)CS$<>8__locals2.propertyId]);
						defaultInterpolatedStringHandler.AppendLiteral("/>：");
						defaultInterpolatedStringHandler.AppendFormatted<short>(CS$<>8__locals2.value);
						defaultInterpolatedStringHandler.AppendLiteral("]</color>");
						extraContent = defaultInterpolatedStringHandler.ToStringAndClear();
					}
					bool isTaiwuFight = false;
					CS$<>8__locals2.resultId = selectionConfig.ResultId1;
					bool flag6 = CS$<>8__locals2.resultId != -1;
					if (flag6)
					{
						bool flag7 = SecretInformationAppliedResult.Instance[CS$<>8__locals2.resultId].SpecialConditionId == 17;
						if (flag7)
						{
							isTaiwuFight = true;
						}
					}
					bool flag8 = isTaiwuFight;
					if (flag8)
					{
						short combatId = SecretInformationAppliedResult.Instance[CS$<>8__locals2.resultId].CombatConfigId;
						bool flag9 = combatId == -1;
						if (flag9)
						{
							combatId = 1;
						}
						int markLimit = (int)GlobalConfig.NeedDefeatMarkCount[(int)CombatConfig.Instance[combatId].CombatType];
						bool flag10 = isMainAttributeCost;
						if (flag10)
						{
							CS$<>8__locals2.selection.OnOptionAvailableCheck = delegate()
							{
								GameData.Domains.Character.Character taiwu = CS$<>8__locals2.selection.ArgBox.GetCharacter("RoleTaiwu");
								bool result2 = *(ref taiwu.GetCurrMainAttributes().Items.FixedElementField + (IntPtr)CS$<>8__locals2.propertyId * 2) >= CS$<>8__locals2.value;
								bool result3 = CombatDomain.GetDefeatMarksCountOutOfCombat(taiwu) < markLimit;
								return result2 && result3;
							};
						}
						else
						{
							CS$<>8__locals2.selection.OnOptionAvailableCheck = delegate()
							{
								GameData.Domains.Character.Character taiwu = CS$<>8__locals2.selection.ArgBox.GetCharacter("RoleTaiwu");
								return CombatDomain.GetDefeatMarksCountOutOfCombat(taiwu) < markLimit;
							};
						}
					}
					else
					{
						bool flag11 = isMainAttributeCost;
						if (flag11)
						{
							CS$<>8__locals2.selection.OnOptionAvailableCheck = delegate()
							{
								GameData.Domains.Character.Character taiwu = CS$<>8__locals2.selection.ArgBox.GetCharacter("RoleTaiwu");
								return *(ref taiwu.GetCurrMainAttributes().Items.FixedElementField + (IntPtr)CS$<>8__locals2.propertyId * 2) >= CS$<>8__locals2.value;
							};
						}
					}
					bool flag12 = isMainAttributeCost;
					if (flag12)
					{
						CS$<>8__locals2.selection.OnOptionSelect = delegate()
						{
							GameData.Domains.Character.Character taiwu = CS$<>8__locals2.selection.ArgBox.GetCharacter("RoleTaiwu");
							EventHelper.ChangeRoleMainAttribute(taiwu, (sbyte)CS$<>8__locals2.propertyId, (int)(-(int)CS$<>8__locals2.value));
							bool flag20 = CS$<>8__locals2.resultId < 0;
							string result2;
							if (flag20)
							{
								CS$<>8__locals2.CS$<>8__locals1.<>4__this.ApplyEventEnd(CS$<>8__locals2.selection.ArgBox);
								result2 = string.Empty;
							}
							else
							{
								CS$<>8__locals2.CS$<>8__locals1.<>4__this.SetResultIndex(CS$<>8__locals2.resultId);
								result2 = CS$<>8__locals2.CS$<>8__locals1.<>4__this.GetEventGuid(CS$<>8__locals2.selection.ArgBox, false);
							}
							return result2;
						};
					}
					else
					{
						CS$<>8__locals2.selection.OnOptionSelect = delegate()
						{
							bool flag20 = CS$<>8__locals2.resultId < 0;
							string result2;
							if (flag20)
							{
								CS$<>8__locals2.CS$<>8__locals1.<>4__this.ApplyEventEnd(CS$<>8__locals2.selection.ArgBox);
								result2 = string.Empty;
							}
							else
							{
								CS$<>8__locals2.CS$<>8__locals1.<>4__this.SetResultIndex(CS$<>8__locals2.resultId);
								result2 = CS$<>8__locals2.CS$<>8__locals1.<>4__this.GetEventGuid(CS$<>8__locals2.selection.ArgBox, false);
							}
							return result2;
						};
					}
					bool isFiveBehavior = string.IsNullOrEmpty(CS$<>8__locals2.content) && selectionConfig.SelectionTexts != null && selectionConfig.SelectionTexts.Length >= 5;
					bool flag13 = !isFiveBehavior;
					if (flag13)
					{
						bool flag14 = !string.IsNullOrEmpty(extraContent);
						if (flag14)
						{
							CS$<>8__locals2.content += extraContent;
						}
						bool flag15 = isTaiwuFight;
						if (flag15)
						{
							short combatId2 = SecretInformationAppliedResult.Instance[CS$<>8__locals2.resultId].CombatConfigId;
							bool flag16 = combatId2 == -1;
							if (flag16)
							{
								combatId2 = 1;
							}
							int markLimit = (int)GlobalConfig.NeedDefeatMarkCount[(int)CombatConfig.Instance[combatId2].CombatType];
							CS$<>8__locals2.selection.GetReplacedContent = delegate()
							{
								string curContent = CS$<>8__locals2.content;
								GameData.Domains.Character.Character taiwu = CS$<>8__locals2.selection.ArgBox.GetCharacter("RoleTaiwu");
								bool flag20 = CombatDomain.GetDefeatMarksCountOutOfCombat(taiwu) >= markLimit;
								if (flag20)
								{
									curContent += "<Language Key=Event_ForSecretInformationAppliedSelection_CanNotFight/>";
								}
								return EventHelper.HandleStringTag(curContent, CS$<>8__locals2.selection.ArgBox, CS$<>8__locals2.CS$<>8__locals1.eventData);
							};
						}
						else
						{
							CS$<>8__locals2.selection.GetReplacedContent = (() => EventHelper.HandleStringTag(CS$<>8__locals2.content, CS$<>8__locals2.selection.ArgBox, CS$<>8__locals2.CS$<>8__locals1.eventData));
						}
						TaiwuEventOption selection = CS$<>8__locals2.selection;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 3);
						defaultInterpolatedStringHandler.AppendFormatted(extraKey);
						defaultInterpolatedStringHandler.AppendLiteral("SecretInformationStandaSelection");
						defaultInterpolatedStringHandler.AppendFormatted<short>(id);
						defaultInterpolatedStringHandler.AppendFormatted<int>(result.Count);
						selection.OptionKey = defaultInterpolatedStringHandler.ToStringAndClear();
						result.Add(CS$<>8__locals2.selection);
					}
					else
					{
						int curkey = result.Count;
						TaiwuEventOption selection2 = CS$<>8__locals2.selection;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 3);
						defaultInterpolatedStringHandler.AppendFormatted(extraKey);
						defaultInterpolatedStringHandler.AppendLiteral("SecretInformationStandaSelection");
						defaultInterpolatedStringHandler.AppendFormatted<short>(id);
						defaultInterpolatedStringHandler.AppendFormatted<int>(result.Count);
						selection2.OptionKey = defaultInterpolatedStringHandler.ToStringAndClear();
						for (int i = 0; i < 5; i++)
						{
							SecretInformationProcessor_Event.<>c__DisplayClass133_4 CS$<>8__locals5 = new SecretInformationProcessor_Event.<>c__DisplayClass133_4();
							CS$<>8__locals5.CS$<>8__locals4 = CS$<>8__locals2;
							SecretInformationProcessor_Event.<>c__DisplayClass133_4 CS$<>8__locals6 = CS$<>8__locals5;
							TaiwuEventOption selection3 = CS$<>8__locals5.CS$<>8__locals4.selection;
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
							defaultInterpolatedStringHandler.AppendFormatted<int>(i);
							CS$<>8__locals6.clone = this.CloneSecretInformationSelection(selection3, defaultInterpolatedStringHandler.ToStringAndClear());
							CS$<>8__locals5.clone.Behavior = (sbyte)(i + 1);
							CS$<>8__locals5.content0 = selectionConfig.SelectionTexts[i];
							bool flag17 = !string.IsNullOrEmpty(extraContent);
							if (flag17)
							{
								CS$<>8__locals5.content0 += extraContent;
							}
							CS$<>8__locals5.clone.GetReplacedContent = (() => EventHelper.HandleStringTag(CS$<>8__locals5.content0, CS$<>8__locals5.clone.ArgBox, CS$<>8__locals5.CS$<>8__locals4.CS$<>8__locals1.eventData));
							result.Add(CS$<>8__locals5.clone);
						}
					}
					bool flag18 = selectionConfig.HotKey == ESecretInformationAppliedSelectionHotKey.Esc;
					if (flag18)
					{
						bool flag19 = string.IsNullOrEmpty(CS$<>8__locals2.CS$<>8__locals1.eventData.EventConfig.EscOptionKey);
						if (flag19)
						{
							CS$<>8__locals2.CS$<>8__locals1.eventData.EventConfig.EscOptionKey = CS$<>8__locals2.selection.OptionKey;
						}
						else
						{
							AdaptableLog.Warning("multiple esc selection in secret", false);
						}
					}
				}
			}
			return result.ToArray();
		}

		// Token: 0x060054CF RID: 21711 RVA: 0x002E777C File Offset: 0x002E597C
		public TaiwuEventOption CloneSecretInformationSelection(TaiwuEventOption origin, string extraKey)
		{
			TaiwuEventOption selection = new TaiwuEventOption();
			selection.ArgBox = origin.ArgBox;
			selection.Behavior = origin.Behavior;
			selection.DefaultState = origin.DefaultState;
			selection.GetExtraFormatLanguageKeys = origin.GetExtraFormatLanguageKeys;
			selection.GetReplacedContent = origin.GetReplacedContent;
			selection.OnOptionAvailableCheck = origin.OnOptionAvailableCheck;
			selection.OnOptionSelect = origin.OnOptionSelect;
			selection.OnOptionVisibleCheck = origin.OnOptionVisibleCheck;
			selection.OptionAvailableConditions = origin.OptionAvailableConditions;
			selection.OptionConsumeInfos = origin.OptionConsumeInfos;
			selection.SetContent(origin.OptionContent);
			selection.OptionKey = origin.OptionKey + extraKey;
			return selection;
		}

		// Token: 0x060054D0 RID: 21712 RVA: 0x002E782C File Offset: 0x002E5A2C
		public bool CheckInformationSelectionAvailable(TaiwuEventOption selection)
		{
			bool flag = !selection.IsVisible;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !selection.IsAvailable;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = selection.OptionAvailableConditions != null;
					if (flag3)
					{
						foreach (TaiwuEventOptionConditionBase item in selection.OptionAvailableConditions)
						{
							bool flag4 = !item.CheckCondition(selection.ArgBox);
							if (flag4)
							{
								return false;
							}
						}
					}
					bool flag5 = selection.OptionConsumeInfos != null;
					if (flag5)
					{
						int charAId = -1;
						int charBId = -1;
						selection.ArgBox.Get("RoleTaiwu", ref charAId);
						selection.ArgBox.Get("CharacterId", ref charBId);
						foreach (OptionConsumeInfo item2 in selection.OptionConsumeInfos)
						{
							bool flag6 = !item2.HasConsumeResource(charAId, charBId);
							if (flag6)
							{
								return false;
							}
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x04001689 RID: 5769
		public static readonly SecretInformationProcessor_Event Instance = new SecretInformationProcessor_Event();

		// Token: 0x0400168A RID: 5770
		private int _metaDataId = -1;

		// Token: 0x0400168B RID: 5771
		private SecretInformationProcessor Processor = new SecretInformationProcessor();

		// Token: 0x0400168C RID: 5772
		private short ResultIndex = -1;

		// Token: 0x0400168D RID: 5773
		private short LastResultIndex = -1;

		// Token: 0x0400168E RID: 5774
		private SecretInformationAppliedResultItem _resultConfig;

		// Token: 0x0400168F RID: 5775
		private GameData.Domains.Character.Character _taiwu;

		// Token: 0x04001690 RID: 5776
		private int _taiwuId;

		// Token: 0x04001691 RID: 5777
		private GameData.Domains.Character.Character _character;

		// Token: 0x04001692 RID: 5778
		private int _characterId;

		// Token: 0x04001693 RID: 5779
		private List<int> _argList = new List<int>();

		// Token: 0x04001694 RID: 5780
		[TupleElementNames(new string[]
		{
			"killerId",
			"victimId",
			"isPublic"
		})]
		private HashSet<ValueTuple<int, int, bool>> _toKillCharIdTupleHashSet = new HashSet<ValueTuple<int, int, bool>>();

		// Token: 0x04001695 RID: 5781
		private HashSet<int> _toEscapeCharIdList = new HashSet<int>();

		// Token: 0x04001696 RID: 5782
		[TupleElementNames(new string[]
		{
			"charId",
			"metaDataId"
		})]
		private HashSet<ValueTuple<int, int>> _toDiscardCharIdList = new HashSet<ValueTuple<int, int>>();

		// Token: 0x04001697 RID: 5783
		private Dictionary<int, int> _toChangeInfectionCharIdList = new Dictionary<int, int>();

		// Token: 0x04001698 RID: 5784
		private Dictionary<int, int> _toChangeHappinessCharIdList = new Dictionary<int, int>();

		// Token: 0x04001699 RID: 5785
		private Dictionary<int, Dictionary<int, int>> _toChangeCharacterFavorList = new Dictionary<int, Dictionary<int, int>>();

		// Token: 0x0400169A RID: 5786
		private SecretInformationProcessor_Event.CombatData _savedCombatData = null;

		// Token: 0x0400169B RID: 5787
		private SecretInformationProcessor_Event.ActionData _savedActionData = null;

		// Token: 0x0400169C RID: 5788
		private SecretInformationProcessor_Event.RelationChangeData _savedRelationChangeData = null;

		// Token: 0x0400169D RID: 5789
		private short _secretInformationContentId = -1;

		// Token: 0x0400169E RID: 5790
		private short _secretInformationStructId = -1;

		// Token: 0x0400169F RID: 5791
		private static readonly List<int> AskCharReleaseFavorLimits = new List<int>
		{
			5,
			4,
			4,
			5,
			6
		};

		// Token: 0x040016A0 RID: 5792
		private static readonly List<int> AskCharKeepFavorLimits = new List<int>
		{
			6,
			5,
			4,
			5,
			6
		};

		// Token: 0x040016A1 RID: 5793
		private short _secretInformationContentIndex = -1;

		// Token: 0x040016A2 RID: 5794
		private List<short> _savedContentSelections = new List<short>();

		// Token: 0x040016A3 RID: 5795
		private string _savedContentText = string.Empty;

		// Token: 0x040016A4 RID: 5796
		private SecretInformationProcessor_Event.EventAction _eventAction = SecretInformationProcessor_Event.EventAction.ShowEvent;

		// Token: 0x02000AF0 RID: 2800
		private class CombatData
		{
			// Token: 0x170005BF RID: 1471
			// (get) Token: 0x0600898A RID: 35210 RVA: 0x004EEF59 File Offset: 0x004ED159
			public short CombatConfigId
			{
				get
				{
					return this.GetCombatConfigId();
				}
			}

			// Token: 0x0600898B RID: 35211 RVA: 0x004EEF61 File Offset: 0x004ED161
			public CombatData(sbyte combatType, bool noGuard, List<short> combatResult, short cantFightResult = -1)
			{
				this.CombatType = combatType;
				this.NoGuard = noGuard;
				this.CombatResult = combatResult;
				this.CantFightResult = cantFightResult;
			}

			// Token: 0x0600898C RID: 35212 RVA: 0x004EEF88 File Offset: 0x004ED188
			public CombatData(SecretInformationAppliedResultItem combatConfig, bool hasGuard)
			{
				this.CombatType = CombatConfig.Instance.GetItem(combatConfig.CombatConfigId).CombatType;
				this.NoGuard = (combatConfig.NoGuard || !this.NoGuard);
				this.CombatResult = SecretInformationProcessor_Event.GetDataListFromConditionResult(combatConfig.SpecialConditionResultIds, 0);
				this.CantFightResult = SecretInformationProcessor_Event.GetDataFromConditionResult(combatConfig.SpecialConditionResultIds, 1, 0);
			}

			// Token: 0x0600898D RID: 35213 RVA: 0x004EEFF8 File Offset: 0x004ED1F8
			private short GetCombatConfigId()
			{
				sbyte combatType = this.CombatType;
				sbyte b = combatType;
				short result;
				if (b != 1)
				{
					if (b != 2)
					{
						result = -1;
					}
					else
					{
						result = (this.NoGuard ? 2 : 122);
					}
				}
				else
				{
					result = (this.NoGuard ? 1 : 121);
				}
				return result;
			}

			// Token: 0x04002D42 RID: 11586
			public sbyte CombatType;

			// Token: 0x04002D43 RID: 11587
			public bool NoGuard;

			// Token: 0x04002D44 RID: 11588
			public List<short> CombatResult;

			// Token: 0x04002D45 RID: 11589
			public short CantFightResult;
		}

		// Token: 0x02000AF1 RID: 2801
		private class ActionData
		{
			// Token: 0x0600898E RID: 35214 RVA: 0x004EF040 File Offset: 0x004ED240
			public ActionData(sbyte actionKey, sbyte phase, List<short> combatResult, int savorId, int kidnaperId, int prisonerId)
			{
				this.ActionKey = actionKey;
				this.Phase = phase;
				this.CombatResult = combatResult;
				this.SaviorId = savorId;
				this.KidnaperId = kidnaperId;
				this.PrisonerId = prisonerId;
			}

			// Token: 0x04002D46 RID: 11590
			public sbyte ActionKey;

			// Token: 0x04002D47 RID: 11591
			public sbyte Phase;

			// Token: 0x04002D48 RID: 11592
			public List<short> CombatResult;

			// Token: 0x04002D49 RID: 11593
			public int SaviorId;

			// Token: 0x04002D4A RID: 11594
			public int KidnaperId;

			// Token: 0x04002D4B RID: 11595
			public int PrisonerId;
		}

		// Token: 0x02000AF2 RID: 2802
		private class RelationChangeData
		{
			// Token: 0x0600898F RID: 35215 RVA: 0x004EF077 File Offset: 0x004ED277
			public RelationChangeData(int selfCharId, int targetCharId, ushort relationType, bool isServe, bool isSuccess)
			{
				this.SelfCharId = selfCharId;
				this.TargetCharId = targetCharId;
				this.RelationType = relationType;
				this.IsServe = isServe;
				this.IsSuccess = isSuccess;
			}

			// Token: 0x04002D4C RID: 11596
			public int SelfCharId;

			// Token: 0x04002D4D RID: 11597
			public int TargetCharId;

			// Token: 0x04002D4E RID: 11598
			public ushort RelationType;

			// Token: 0x04002D4F RID: 11599
			public bool IsServe;

			// Token: 0x04002D50 RID: 11600
			public bool IsSuccess;
		}

		// Token: 0x02000AF3 RID: 2803
		public enum EventAction
		{
			// Token: 0x04002D52 RID: 11602
			ShowEvent,
			// Token: 0x04002D53 RID: 11603
			EndEvent,
			// Token: 0x04002D54 RID: 11604
			StartCombat,
			// Token: 0x04002D55 RID: 11605
			StartLifeSkillCombat,
			// Token: 0x04002D56 RID: 11606
			ChooseRope,
			// Token: 0x04002D57 RID: 11607
			ShowFristContent,
			// Token: 0x04002D58 RID: 11608
			JumpToOtherEvent
		}

		// Token: 0x02000AF4 RID: 2804
		public static class EventArgKeys_Infomation
		{
			// Token: 0x04002D59 RID: 11609
			public const string CombatResult = "CombatResult";

			// Token: 0x04002D5A RID: 11610
			public const string LifeSikllCombatResult = "WinState";

			// Token: 0x04002D5B RID: 11611
			public const string ResultEventGuid = "resultEventGuid";

			// Token: 0x04002D5C RID: 11612
			public const string ResultEventGuid_Part2 = "conditionEventGuid";

			// Token: 0x04002D5D RID: 11613
			public const string BreakCharacterId = "breakTargetCharacterId";

			// Token: 0x04002D5E RID: 11614
			public const string ActorId = "actorId";

			// Token: 0x04002D5F RID: 11615
			public const string ReactorId = "reactorId";

			// Token: 0x04002D60 RID: 11616
			public const string SecactorId = "secactorId";
		}

		// Token: 0x02000AF5 RID: 2805
		private static class LoveRelationValue
		{
			// Token: 0x04002D61 RID: 11617
			public static int[] MakeEnemyWhenDivorce = new int[]
			{
				20,
				10,
				30,
				50,
				40
			};

			// Token: 0x04002D62 RID: 11618
			public static int[] MakeEnemyWhenNotAdore = new int[]
			{
				0,
				0,
				10,
				30,
				20
			};

			// Token: 0x04002D63 RID: 11619
			public static int[] FavorOfDivorce = new int[]
			{
				-3,
				-5,
				-4,
				-5,
				-3
			};

			// Token: 0x04002D64 RID: 11620
			public static int[] FavorOfBreakUp = new int[]
			{
				0,
				-2,
				-1,
				-2,
				0
			};

			// Token: 0x04002D65 RID: 11621
			public static int[] FavorOfNotAdore = new int[]
			{
				1,
				0,
				-1,
				0,
				1
			};

			// Token: 0x04002D66 RID: 11622
			public const int Adore = 0;

			// Token: 0x04002D67 RID: 11623
			public const int Lover = 1;

			// Token: 0x04002D68 RID: 11624
			public const int Spouse = 2;

			// Token: 0x04002D69 RID: 11625
			public const int BaseFavorChangeOfForgive = -3000;

			// Token: 0x04002D6A RID: 11626
			public const int BaseHappinessChangeOfForgive = -5;

			// Token: 0x04002D6B RID: 11627
			public const int BasseFavorChangeOfAskBreak = -3000;

			// Token: 0x04002D6C RID: 11628
			public const int FavorMultipleChangeOfAskBreak = -1500;

			// Token: 0x04002D6D RID: 11629
			public const int FavorChangeOfRefuseBreak = -6000;
		}
	}
}
