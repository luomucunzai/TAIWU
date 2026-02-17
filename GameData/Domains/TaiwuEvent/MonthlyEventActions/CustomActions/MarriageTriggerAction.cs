using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Relation;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.EventManager;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions
{
	// Token: 0x0200009C RID: 156
	[SerializableGameData(NotForDisplayModule = true)]
	public class MarriageTriggerAction : MonthlyActionBase, IDynamicAction, ISerializableGameData
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06001A00 RID: 6656 RVA: 0x00173A02 File Offset: 0x00171C02
		public short DynamicActionType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x00173A05 File Offset: 0x00171C05
		public MarriageTriggerAction()
		{
			this.Key = MonthlyActionKey.Invalid;
			this.ParticipatingCharacterSets = new List<CharacterSet>();
			this.Location = Location.Invalid;
			this.SpouseCharId = -1;
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x00173A38 File Offset: 0x00171C38
		public override void TriggerAction()
		{
			bool flag = !this.Location.IsValid();
			if (!flag)
			{
				Character spouseChar;
				bool flag2 = this.SpouseCharId < 0 || !DomainManager.Character.TryGetElement_Objects(this.SpouseCharId, out spouseChar) || spouseChar.IsCompletelyInfected();
				if (!flag2)
				{
					bool flag3 = DomainManager.Adventure.TryCreateAdventureSite(DomainManager.TaiwuEvent.MainThreadDataContext, this.Location.AreaId, this.Location.BlockId, 144, this.Key);
					if (flag3)
					{
						MonthlyEventActionsManager.NewlyTriggered++;
						this.State = 1;
						this.CallSpouse();
					}
					else
					{
						this.Location = Location.Invalid;
					}
				}
			}
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x00173AEC File Offset: 0x00171CEC
		public override void MonthlyHandler()
		{
			bool flag = this.State != 0;
			if (flag)
			{
				DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				bool flag2 = DomainManager.Character.GetAliveSpouse(taiwuCharId) >= 0;
				if (flag2)
				{
					DomainManager.Adventure.RemoveAdventureSite(context, this.Location.AreaId, this.Location.BlockId, false, false);
					return;
				}
			}
			bool flag3 = this.State == 1;
			if (flag3)
			{
				this.CallParticipateCharacters();
				this.Activate();
				this.State = 5;
			}
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x00173B84 File Offset: 0x00171D84
		public override void Activate()
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			MonthlyEventActionsManager.NewlyActivated++;
			DomainManager.Adventure.ActivateAdventureSite(context, this.Location.AreaId, this.Location.BlockId);
			MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			monthlyNotifications.AddMarryNotice(taiwuCharId, this.SpouseCharId, this.Location);
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x00173BF8 File Offset: 0x00171DF8
		public override void Deactivate(bool isComplete)
		{
			bool flag = !isComplete;
			if (flag)
			{
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				bool flag2 = DomainManager.Character.GetAliveSpouse(taiwuCharId) >= 0;
				if (flag2)
				{
					monthlyEventCollection.AddTaiwuAlreadyMarried(taiwuCharId, this.SpouseCharId);
				}
				else
				{
					monthlyEventCollection.AddTaiwuNotAttendingWedding(taiwuCharId, this.SpouseCharId);
				}
			}
			this.State = 0;
			this.Month = 0;
			this.LastFinishDate = DomainManager.World.GetCurrDate();
			this.Location = Location.Invalid;
			this.ClearCalledCharacters();
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x00173C8C File Offset: 0x00171E8C
		public override MonthlyActionBase CreateCopy()
		{
			return Serializer.CreateCopy<MarriageTriggerAction>(this);
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x00173CA4 File Offset: 0x00171EA4
		public override void CollectCalledCharacters(HashSet<int> calledCharacters)
		{
			calledCharacters.Add(this.SpouseCharId);
			foreach (CharacterSet charSet in this.ParticipatingCharacterSets)
			{
				calledCharacters.UnionWith(charSet.GetCollection());
			}
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x00173D10 File Offset: 0x00171F10
		public override void FillEventArgBox(EventArgBox eventArgBox)
		{
			AdaptableLog.Info("Adding major characters to adventure.");
			eventArgBox.Set("MajorCharacter_0_0", this.SpouseCharId);
			eventArgBox.Set("MajorCharacter_0_Count", 1);
			AdaptableLog.Info("Adding participating characters to adventure.");
			for (int participateCharSetIndex = 0; participateCharSetIndex < this.ParticipatingCharacterSets.Count; participateCharSetIndex++)
			{
				CharacterSet participateCharSet = (participateCharSetIndex < this.ParticipatingCharacterSets.Count) ? this.ParticipatingCharacterSets[participateCharSetIndex] : default(CharacterSet);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("ParticipateCharacter_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(participateCharSetIndex);
				this.FillIntelligentCharactersToArgBox(eventArgBox, defaultInterpolatedStringHandler.ToStringAndClear(), participateCharSet);
				AdventureCharacterSortUtils.Sort(eventArgBox, false, participateCharSetIndex, CharacterSortType.CombatPower, false);
			}
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x00173DD4 File Offset: 0x00171FD4
		private void FillIntelligentCharactersToArgBox(EventArgBox eventArgBox, string keyPrefix, CharacterSet charSet)
		{
			int amountAdded = 0;
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			foreach (int charId in charSet.GetCollection())
			{
				Character character;
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
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
						defaultInterpolatedStringHandler.AppendFormatted(keyPrefix);
						defaultInterpolatedStringHandler.AppendLiteral("_");
						defaultInterpolatedStringHandler.AppendFormatted<int>(amountAdded);
						eventArgBox.Set(defaultInterpolatedStringHandler.ToStringAndClear(), charId);
						amountAdded++;
					}
				}
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Adding ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(amountAdded);
			defaultInterpolatedStringHandler.AppendLiteral(" real characters to adventure.");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			eventArgBox.Set(keyPrefix + "_Count", amountAdded);
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x00173F1C File Offset: 0x0017211C
		private void ClearCalledCharacters()
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			Character spouseChar;
			bool flag = DomainManager.Character.TryGetElement_Objects(this.SpouseCharId, out spouseChar);
			if (flag)
			{
				spouseChar.DeactivateExternalRelationState(context, 4);
				bool flag2 = spouseChar.GetLeaderId() != DomainManager.Taiwu.GetTaiwuCharId();
				if (flag2)
				{
					bool flag3 = spouseChar.IsCompletelyInfected();
					if (flag3)
					{
						Events.RaiseInfectedCharacterLocationChanged(context, this.SpouseCharId, Location.Invalid, spouseChar.GetLocation());
					}
					else
					{
						Events.RaiseCharacterLocationChanged(context, this.SpouseCharId, Location.Invalid, spouseChar.GetLocation());
					}
				}
				short settlementId = spouseChar.GetOrganizationInfo().SettlementId;
				bool flag4 = settlementId >= 0;
				if (flag4)
				{
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
					spouseChar.AddTravelTarget(context, new NpcTravelTarget(settlement.GetLocation(), 12));
				}
				this.SpouseCharId = -1;
			}
			CallCharacterHelper.ClearCalledCharacters(this.ParticipatingCharacterSets, true, true);
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x00174008 File Offset: 0x00172208
		private void CallSpouse()
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			Character spouseChar = DomainManager.Character.GetElement_Objects(this.SpouseCharId);
			DomainManager.Character.LeaveGroup(context, spouseChar, true);
			DomainManager.Character.GroupMove(context, spouseChar, this.Location);
			Events.RaiseCharacterLocationChanged(context, this.SpouseCharId, this.Location, Location.Invalid);
			spouseChar.ActiveExternalRelationState(context, 4);
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x00174074 File Offset: 0x00172274
		private void CallParticipateCharacters()
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			bool flag = this.ParticipatingCharacterSets.Count <= 0;
			if (flag)
			{
				this.ParticipatingCharacterSets.Add(default(CharacterSet));
			}
			CharacterSet taiwuFriendsAndFamilyMembers = this.ParticipatingCharacterSets[0];
			this.CallFriendsAndFamilyMembers(context, taiwuCharId, ref taiwuFriendsAndFamilyMembers, 10);
			this.ParticipatingCharacterSets[0] = taiwuFriendsAndFamilyMembers;
			bool flag2 = this.ParticipatingCharacterSets.Count <= 1;
			if (flag2)
			{
				this.ParticipatingCharacterSets.Add(default(CharacterSet));
			}
			CharacterSet spouseFriendsAndFamilyMembers = this.ParticipatingCharacterSets[1];
			this.CallFriendsAndFamilyMembers(context, this.SpouseCharId, ref spouseFriendsAndFamilyMembers, 10);
			this.ParticipatingCharacterSets[1] = spouseFriendsAndFamilyMembers;
			bool flag3 = this.ParticipatingCharacterSets.Count <= 2;
			if (flag3)
			{
				this.ParticipatingCharacterSets.Add(default(CharacterSet));
			}
			CharacterSet taiwuTwoWayAdoredCharacters = this.ParticipatingCharacterSets[2];
			this.CallTwoWayAdoredCharacters(context, taiwuCharId, ref taiwuTwoWayAdoredCharacters, 5);
			this.ParticipatingCharacterSets[2] = taiwuTwoWayAdoredCharacters;
			bool flag4 = this.ParticipatingCharacterSets.Count <= 3;
			if (flag4)
			{
				this.ParticipatingCharacterSets.Add(default(CharacterSet));
			}
			CharacterSet spouseTwoWayAdoredCharacters = this.ParticipatingCharacterSets[3];
			this.CallTwoWayAdoredCharacters(context, this.SpouseCharId, ref spouseTwoWayAdoredCharacters, 5);
			this.ParticipatingCharacterSets[3] = spouseTwoWayAdoredCharacters;
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x001741F8 File Offset: 0x001723F8
		private void CallFriendsAndFamilyMembers(DataContext context, int charId, ref CharacterSet characterSet, int estimateAmount)
		{
			HashSet<int> relatedCharIds = context.AdvanceMonthRelatedData.RelatedCharIds.Occupy();
			RelatedCharacters relatedChars = DomainManager.Character.GetRelatedCharacters(charId);
			relatedCharIds.UnionWith(relatedChars.BloodParents.GetCollection());
			relatedCharIds.UnionWith(relatedChars.AdoptiveParents.GetCollection());
			relatedCharIds.UnionWith(relatedChars.StepParents.GetCollection());
			relatedCharIds.UnionWith(relatedChars.BloodBrothersAndSisters.GetCollection());
			relatedCharIds.UnionWith(relatedChars.AdoptiveBrothersAndSisters.GetCollection());
			relatedCharIds.UnionWith(relatedChars.StepBrothersAndSisters.GetCollection());
			relatedCharIds.UnionWith(relatedChars.Mentors.GetCollection());
			relatedCharIds.UnionWith(relatedChars.Mentees.GetCollection());
			relatedCharIds.UnionWith(relatedChars.SwornBrothersAndSisters.GetCollection());
			relatedCharIds.UnionWith(relatedChars.Friends.GetCollection());
			relatedCharIds.UnionWith(relatedChars.BloodParents.GetCollection());
			relatedCharIds.UnionWith(relatedChars.AdoptiveParents.GetCollection());
			relatedCharIds.UnionWith(relatedChars.StepParents.GetCollection());
			int calledAmount = 0;
			foreach (int relatedCharId in relatedCharIds)
			{
				bool flag = calledAmount > estimateAmount;
				if (flag)
				{
					break;
				}
				Character character;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(relatedCharId, out character);
				if (!flag2)
				{
					bool flag3 = !this.CheckCharacterAvailable(character);
					if (!flag3)
					{
						bool flag4 = DomainManager.Character.HasRelation(relatedCharId, charId, 16384);
						if (!flag4)
						{
							bool flag5 = character.GetLeaderId() >= 0;
							if (flag5)
							{
								DomainManager.Character.LeaveGroup(context, character, true);
							}
							DomainManager.Character.GroupMove(context, character, this.Location);
							character.ActiveExternalRelationState(context, 4);
							characterSet.Add(relatedCharId);
							calledAmount++;
						}
					}
				}
			}
			context.AdvanceMonthRelatedData.RelatedCharIds.Release(ref relatedCharIds);
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x0017440C File Offset: 0x0017260C
		private void CallTwoWayAdoredCharacters(DataContext context, int charId, ref CharacterSet characterSet, int estimateAmount)
		{
			RelatedCharacters relatedChars = DomainManager.Character.GetRelatedCharacters(charId);
			HashSet<int> adoredCharIds = relatedChars.Adored.GetCollection();
			int calledAmount = 0;
			foreach (int relatedCharId in adoredCharIds)
			{
				bool flag = calledAmount > estimateAmount;
				if (flag)
				{
					break;
				}
				Character character;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(relatedCharId, out character);
				if (!flag2)
				{
					bool flag3 = !this.CheckCharacterAvailable(character);
					if (!flag3)
					{
						bool flag4 = !DomainManager.Character.HasRelation(relatedCharId, charId, 16384);
						if (!flag4)
						{
							bool flag5 = character.GetLeaderId() >= 0;
							if (flag5)
							{
								DomainManager.Character.LeaveGroup(context, character, true);
							}
							DomainManager.Character.GroupMove(context, character, this.Location);
							character.ActiveExternalRelationState(context, 4);
							characterSet.Add(relatedCharId);
							calledAmount++;
						}
					}
				}
			}
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x00174520 File Offset: 0x00172720
		private bool CheckCharacterAvailable(Character character)
		{
			bool flag = character.IsCompletelyInfected();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = character.GetLegendaryBookOwnerState() >= 1;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = character.GetAgeGroup() == 0;
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = !character.GetLocation().IsValid();
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool flag5 = character.GetCreatingType() != 1;
							if (flag5)
							{
								result = false;
							}
							else
							{
								bool flag6 = character.GetKidnapperId() >= 0;
								if (flag6)
								{
									result = false;
								}
								else
								{
									bool flag7 = character.IsActiveExternalRelationState(60);
									if (flag7)
									{
										result = false;
									}
									else
									{
										bool flag8 = DomainManager.Taiwu.IsInGroup(character.GetId());
										result = !flag8;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x001745E0 File Offset: 0x001727E0
		public override bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x001745F4 File Offset: 0x001727F4
		public override int GetSerializedSize()
		{
			int totalSize = 22;
			bool flag = this.ParticipatingCharacterSets != null;
			if (flag)
			{
				totalSize += 2;
				int elementsCount = this.ParticipatingCharacterSets.Count;
				for (int i = 0; i < elementsCount; i++)
				{
					totalSize += this.ParticipatingCharacterSets[i].GetSerializedSize();
				}
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x00174670 File Offset: 0x00172870
		public unsafe override int Serialize(byte* pData)
		{
			*(short*)pData = this.DynamicActionType;
			byte* pCurrData = pData + 2;
			pCurrData += this.Location.Serialize(pCurrData);
			*(int*)pCurrData = this.SpouseCharId;
			pCurrData += 4;
			bool flag = this.ParticipatingCharacterSets != null;
			if (flag)
			{
				int elementsCount = this.ParticipatingCharacterSets.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					int subDataSize = this.ParticipatingCharacterSets[i].Serialize(pCurrData);
					pCurrData += subDataSize;
					Tester.Assert(subDataSize <= 65535, "");
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

		// Token: 0x06001A13 RID: 6675 RVA: 0x00174790 File Offset: 0x00172990
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + 2;
			pCurrData += this.Location.Deserialize(pCurrData);
			this.SpouseCharId = *(int*)pCurrData;
			pCurrData += 4;
			ushort elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this.ParticipatingCharacterSets == null;
				if (flag2)
				{
					this.ParticipatingCharacterSets = new List<CharacterSet>((int)elementsCount);
				}
				else
				{
					this.ParticipatingCharacterSets.Clear();
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					CharacterSet element = default(CharacterSet);
					pCurrData += element.Deserialize(pCurrData);
					this.ParticipatingCharacterSets.Add(element);
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

		// Token: 0x04000604 RID: 1540
		[SerializableGameDataField]
		public Location Location;

		// Token: 0x04000605 RID: 1541
		[SerializableGameDataField]
		public int SpouseCharId;

		// Token: 0x04000606 RID: 1542
		[SerializableGameDataField]
		public List<CharacterSet> ParticipatingCharacterSets;
	}
}
