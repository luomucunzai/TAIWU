using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Creation;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions
{
	// Token: 0x0200009A RID: 154
	[SerializableGameData(NotForDisplayModule = true)]
	public class FulongStoryAdventureThreeTriggerAction : MonthlyActionBase, IDynamicAction, ISerializableGameData
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060019D5 RID: 6613 RVA: 0x00171C36 File Offset: 0x0016FE36
		public short DynamicActionType
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x00171C39 File Offset: 0x0016FE39
		public FulongStoryAdventureThreeTriggerAction()
		{
			this.InitCharacterId();
			this.Location = Location.Invalid;
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x00171C55 File Offset: 0x0016FE55
		public override void MonthlyHandler()
		{
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x00171C58 File Offset: 0x0016FE58
		public override void TriggerAction()
		{
			bool flag = this.State != 0;
			if (!flag)
			{
				DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
				Settlement settlement = this.GetSettlement();
				this.Location = settlement.GetLocation();
				bool flag2 = DomainManager.Adventure.TryCreateAdventureSite(context, this.Location.AreaId, this.Location.BlockId, 190, this.Key);
				if (flag2)
				{
					DomainManager.Adventure.ActivateAdventureSite(context, this.Location.AreaId, this.Location.BlockId);
					DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, 14, "ConchShip_PresetKey_FulongAdventureThreeCountDown", 3);
					DomainManager.Extra.TriggerExtraTask(context, 52, 340);
					this.CallLeader(context);
					this.CallParticipant(context, ref this._participant1);
					this.CallParticipant(context, ref this._participant2);
					this.CallParticipant(context, ref this._participant3);
					this.CallParticipant(context, ref this._participant4);
					this.CallParticipant(context, ref this._participant5);
					this.CallParticipant(context, ref this._participant6);
					this.CallParticipant(context, ref this._participant7);
				}
			}
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x00171D80 File Offset: 0x0016FF80
		public override void CollectCalledCharacters(HashSet<int> calledCharacters)
		{
			bool flag = this._leaderId >= 0;
			if (flag)
			{
				calledCharacters.Add(this._leaderId);
			}
			bool flag2 = this._participant1 >= 0;
			if (flag2)
			{
				calledCharacters.Add(this._participant1);
			}
			bool flag3 = this._participant2 >= 0;
			if (flag3)
			{
				calledCharacters.Add(this._participant2);
			}
			bool flag4 = this._participant3 >= 0;
			if (flag4)
			{
				calledCharacters.Add(this._participant3);
			}
			bool flag5 = this._participant4 >= 0;
			if (flag5)
			{
				calledCharacters.Add(this._participant4);
			}
			bool flag6 = this._participant5 >= 0;
			if (flag6)
			{
				calledCharacters.Add(this._participant5);
			}
			bool flag7 = this._participant6 >= 0;
			if (flag7)
			{
				calledCharacters.Add(this._participant6);
			}
			bool flag8 = this._participant7 >= 0;
			if (flag8)
			{
				calledCharacters.Add(this._participant7);
			}
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x00171E80 File Offset: 0x00170080
		public override void Deactivate(bool isComplete)
		{
			this.State = 0;
			this.Month = 0;
			this.LastFinishDate = DomainManager.World.GetCurrDate();
			this.Location = Location.Invalid;
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			FulongStoryAdventureThreeTriggerAction.ReleaseCharacter(context, this._leaderId);
			FulongStoryAdventureThreeTriggerAction.ReleaseCharacter(context, this._participant1);
			FulongStoryAdventureThreeTriggerAction.ReleaseCharacter(context, this._participant2);
			FulongStoryAdventureThreeTriggerAction.ReleaseCharacter(context, this._participant3);
			FulongStoryAdventureThreeTriggerAction.ReleaseCharacter(context, this._participant4);
			FulongStoryAdventureThreeTriggerAction.ReleaseCharacter(context, this._participant5);
			FulongStoryAdventureThreeTriggerAction.ReleaseCharacter(context, this._participant6);
			FulongStoryAdventureThreeTriggerAction.ReleaseCharacter(context, this._participant7);
			this.InitCharacterId();
			bool flag = !isComplete;
			if (flag)
			{
				EventHelper.FulongEndEffect(false, false);
			}
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x00171F48 File Offset: 0x00170148
		public override MonthlyActionBase CreateCopy()
		{
			return Serializer.CreateCopy<FulongStoryAdventureThreeTriggerAction>(this);
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x00171F60 File Offset: 0x00170160
		public override void EnsurePrerequisites()
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			bool flag = this.IsLeaderOutdated();
			if (flag)
			{
				FulongStoryAdventureThreeTriggerAction.ReleaseCharacter(context, this._leaderId);
				this._leaderId = -1;
				this.CallLeader(context);
			}
			this.HandleParticipantOutdated(context, ref this._participant1);
			this.HandleParticipantOutdated(context, ref this._participant2);
			this.HandleParticipantOutdated(context, ref this._participant3);
			this.HandleParticipantOutdated(context, ref this._participant4);
			this.HandleParticipantOutdated(context, ref this._participant5);
			this.HandleParticipantOutdated(context, ref this._participant6);
			this.HandleParticipantOutdated(context, ref this._participant7);
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x00172004 File Offset: 0x00170204
		private Settlement GetSettlement()
		{
			return DomainManager.Organization.GetSettlementByOrgTemplateId(14);
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x00172024 File Offset: 0x00170224
		public override void FillEventArgBox(EventArgBox eventArgBox)
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			GameData.Domains.Character.Character leader;
			bool flag = this._leaderId > 0 && DomainManager.Character.TryGetElement_Objects(this._leaderId, out leader);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Adding 1 major characters ");
				defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(leader);
				defaultInterpolatedStringHandler.AppendLiteral(" to adventure.");
				AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				eventArgBox.Set("MajorCharacter_0_0", this._leaderId);
			}
			this.FillParticipantEventArgBox(context, eventArgBox, this._participant1, "MajorCharacter_0_1", 0, 2);
			this.FillParticipantEventArgBox(context, eventArgBox, this._participant2, "MajorCharacter_0_2", 3, 5);
			this.FillParticipantEventArgBox(context, eventArgBox, this._participant3, "MajorCharacter_0_3", 3, 5);
			this.FillParticipantEventArgBox(context, eventArgBox, this._participant4, "MajorCharacter_0_4", 0, 2);
			this.FillParticipantEventArgBox(context, eventArgBox, this._participant5, "MajorCharacter_0_5", 3, 5);
			this.FillParticipantEventArgBox(context, eventArgBox, this._participant6, "MajorCharacter_0_6", 3, 5);
			this.FillParticipantEventArgBox(context, eventArgBox, this._participant7, "MajorCharacter_0_7", 3, 5);
			eventArgBox.Set("MajorCharacter_0_Count", 8);
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x00172154 File Offset: 0x00170354
		private GameData.Domains.Character.Character CreateTemporaryIntelligentParticipant(DataContext context, EventArgBox eventArgBox, string participant1NameStr, sbyte grade)
		{
			Settlement settlement = this.GetSettlement();
			OrganizationItem orgTemplate = Organization.Instance[14];
			sbyte gender = orgTemplate.GenderRestriction;
			bool flag = gender < 0;
			if (flag)
			{
				gender = Gender.GetRandom(context.Random);
			}
			sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(settlement.GetLocation().AreaId);
			short charTemplateId = OrganizationDomain.GetCharacterTemplateId(orgTemplate.TemplateId, stateTemplateId, gender);
			TemporaryIntelligentCharacterCreationInfo tempCreationInfo = new TemporaryIntelligentCharacterCreationInfo
			{
				Location = this.Location,
				CharTemplateId = charTemplateId,
				OrgInfo = new OrganizationInfo(orgTemplate.TemplateId, grade, true, settlement.GetId())
			};
			GameData.Domains.Character.Character character = DomainManager.Character.CreateTemporaryIntelligentCharacter(context, ref tempCreationInfo);
			OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(tempCreationInfo.OrgInfo);
			ValueTuple<string, string> realName = CharacterDomain.GetRealName(character);
			string surname = realName.Item1;
			string givenName = realName.Item2;
			string tag = "MartialArtTournamentMonthlyAction";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 5);
			defaultInterpolatedStringHandler.AppendLiteral("Creating temporary character ");
			defaultInterpolatedStringHandler.AppendFormatted(orgTemplate.Name);
			defaultInterpolatedStringHandler.AppendLiteral("-");
			defaultInterpolatedStringHandler.AppendFormatted(orgMemberCfg.GradeName);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(surname);
			defaultInterpolatedStringHandler.AppendFormatted(givenName);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted<int>(character.GetId());
			defaultInterpolatedStringHandler.AppendLiteral(")");
			AdaptableLog.TagInfo(tag, defaultInterpolatedStringHandler.ToStringAndClear());
			int charId = character.GetId();
			DomainManager.Adventure.AddTemporaryIntelligentCharacter(charId);
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Adding 1 temporary major characters ");
			defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
			defaultInterpolatedStringHandler.AppendLiteral(" to adventure.");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			eventArgBox.Set(participant1NameStr, charId);
			return character;
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x00172328 File Offset: 0x00170528
		private void CallLeader(DataContext context)
		{
			Tester.Assert(this._leaderId < 0, "");
			Settlement settlement = this.GetSettlement();
			GameData.Domains.Character.Character leader = settlement.GetLeader();
			bool flag = leader == null;
			if (!flag)
			{
				bool flag2 = leader.GetAgeGroup() != 2;
				if (!flag2)
				{
					this._leaderId = leader.GetId();
					DomainManager.Character.LeaveGroup(context, leader, true);
					DomainManager.Character.GroupMove(context, leader, this.Location);
					Events.RaiseCharacterLocationChanged(context, this._leaderId, this.Location, Location.Invalid);
					leader.ActiveExternalRelationState(context, 4);
				}
			}
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x001723C4 File Offset: 0x001705C4
		private bool IsLeaderOutdated()
		{
			bool flag = this._leaderId < 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				GameData.Domains.Character.Character leader;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(this._leaderId, out leader);
				if (flag2)
				{
					result = true;
				}
				else
				{
					OrganizationInfo orgInfo = leader.GetOrganizationInfo();
					bool flag3 = orgInfo.Grade != 8;
					if (flag3)
					{
						result = true;
					}
					else
					{
						bool flag4 = leader.GetKidnapperId() >= 0;
						if (flag4)
						{
							result = true;
						}
						else
						{
							bool flag5 = leader.GetAgeGroup() < 2;
							if (flag5)
							{
								result = true;
							}
							else
							{
								bool flag6 = leader.GetLocation() != this.Location;
								result = flag6;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x0017246C File Offset: 0x0017066C
		private static void ReleaseCharacter(DataContext context, int charId)
		{
			GameData.Domains.Character.Character character;
			bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
			if (!flag)
			{
				character.DeactivateExternalRelationState(context, 4);
				bool flag2 = character.IsCompletelyInfected();
				if (flag2)
				{
					Events.RaiseInfectedCharacterLocationChanged(context, charId, Location.Invalid, character.GetLocation());
				}
				else
				{
					Events.RaiseCharacterLocationChanged(context, charId, Location.Invalid, character.GetLocation());
				}
			}
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x001724CC File Offset: 0x001706CC
		private void CallParticipant(DataContext context, ref int participantId)
		{
			Tester.Assert(participantId < 0, "");
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Settlement settlement = this.GetSettlement();
			List<int> charIds = new List<int>();
			settlement.GetMembers().GetAllMembers(charIds);
			short maxFavorType = 1;
			int i = 0;
			while (i < charIds.Count)
			{
				int charId = charIds[i];
				GameData.Domains.Character.Character character;
				bool flag = DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (flag)
				{
					OrganizationInfo orgInfo = character.GetOrganizationInfo();
					bool flag2 = orgInfo.Grade == 8 && orgInfo.Principal;
					if (!flag2)
					{
						bool flag3 = character.IsActiveExternalRelationState(4);
						if (!flag3)
						{
							sbyte favorType = DomainManager.Character.GetFavorabilityType(charId, taiwu.GetId());
							bool flag4 = favorType < 2;
							if (!flag4)
							{
								bool flag5 = DomainManager.Character.HasRelation(taiwu.GetId(), charId, 1) || DomainManager.Character.HasRelation(taiwu.GetId(), charId, 8) || DomainManager.Character.HasRelation(taiwu.GetId(), charId, 2) || DomainManager.Character.HasRelation(taiwu.GetId(), charId, 16) || DomainManager.Character.HasRelation(taiwu.GetId(), charId, 1024) || DomainManager.Character.HasRelation(taiwu.GetId(), charId, 4) || DomainManager.Character.HasRelation(taiwu.GetId(), charId, 32);
								if (flag5)
								{
									participantId = charId;
									break;
								}
								bool flag6 = DomainManager.Character.HasRelation(taiwu.GetId(), charId, 512) || (DomainManager.Character.HasRelation(taiwu.GetId(), charId, 16384) && DomainManager.Character.HasRelation(charId, taiwu.GetId(), 16384));
								if (flag6)
								{
									participantId = charId;
									break;
								}
								bool flag7 = DomainManager.Character.HasRelation(taiwu.GetId(), charId, 8192) || DomainManager.Character.HasRelation(taiwu.GetId(), charId, 2048) || DomainManager.Character.HasRelation(taiwu.GetId(), charId, 4096);
								if (flag7)
								{
									participantId = charId;
									break;
								}
								bool flag8 = (short)favorType > maxFavorType;
								if (flag8)
								{
									maxFavorType = (short)favorType;
									participantId = charId;
								}
							}
						}
					}
				}
				IL_23D:
				i++;
				continue;
				goto IL_23D;
			}
			bool flag9 = participantId < 0;
			if (!flag9)
			{
				GameData.Domains.Character.Character participant = DomainManager.Character.GetElement_Objects(participantId);
				DomainManager.Character.LeaveGroup(context, participant, true);
				DomainManager.Character.GroupMove(context, participant, this.Location);
				Events.RaiseCharacterLocationChanged(context, this._leaderId, this.Location, Location.Invalid);
				participant.ActiveExternalRelationState(context, 4);
			}
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x00172790 File Offset: 0x00170990
		private bool IsParticipantOutdated(int participantId)
		{
			bool flag = participantId < 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				GameData.Domains.Character.Character participant;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(participantId, out participant);
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = participant.GetKidnapperId() >= 0;
					if (flag3)
					{
						result = true;
					}
					else
					{
						bool flag4 = participant.GetLocation() != this.Location;
						result = flag4;
					}
				}
			}
			return result;
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x001727F8 File Offset: 0x001709F8
		private void InitCharacterId()
		{
			this._leaderId = -1;
			this._participant1 = -1;
			this._participant2 = -1;
			this._participant3 = -1;
			this._participant4 = -1;
			this._participant5 = -1;
			this._participant6 = -1;
			this._participant7 = -1;
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x00172834 File Offset: 0x00170A34
		private void HandleParticipantOutdated(DataContext context, ref int characterId)
		{
			bool flag = this.IsParticipantOutdated(characterId);
			if (flag)
			{
				FulongStoryAdventureThreeTriggerAction.ReleaseCharacter(context, characterId);
				characterId = -1;
				this.CallParticipant(context, ref characterId);
			}
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x00172868 File Offset: 0x00170A68
		private void FillParticipantEventArgBox(DataContext context, EventArgBox eventArgBox, int characterId, string nameStr, sbyte gradeLow, sbyte gradeHigh)
		{
			GameData.Domains.Character.Character character;
			bool flag = characterId < 0 || !DomainManager.Character.TryGetElement_Objects(characterId, out character);
			if (flag)
			{
				this.CreateTemporaryIntelligentParticipant(context, eventArgBox, nameStr, (sbyte)context.Random.Next((int)gradeLow, (int)(gradeHigh + 1)));
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Adding 1 major characters ");
				defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
				defaultInterpolatedStringHandler.AppendLiteral(" to adventure.");
				AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				eventArgBox.Set(nameStr, characterId);
			}
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x001728F8 File Offset: 0x00170AF8
		public override bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x0017290C File Offset: 0x00170B0C
		public override int GetSerializedSize()
		{
			int totalSize = 50;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x00172934 File Offset: 0x00170B34
		public unsafe override int Serialize(byte* pData)
		{
			*(short*)pData = this.DynamicActionType;
			byte* pCurrData = pData + 2;
			pCurrData += this.Location.Serialize(pCurrData);
			*(int*)pCurrData = this._leaderId;
			pCurrData += 4;
			*(int*)pCurrData = this._participant1;
			pCurrData += 4;
			*(int*)pCurrData = this._participant2;
			pCurrData += 4;
			*(int*)pCurrData = this._participant3;
			pCurrData += 4;
			*(int*)pCurrData = this._participant4;
			pCurrData += 4;
			*(int*)pCurrData = this._participant5;
			pCurrData += 4;
			*(int*)pCurrData = this._participant6;
			pCurrData += 4;
			*(int*)pCurrData = this._participant7;
			pCurrData += 4;
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

		// Token: 0x060019EB RID: 6635 RVA: 0x00172A10 File Offset: 0x00170C10
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + 2;
			pCurrData += this.Location.Deserialize(pCurrData);
			this._leaderId = *(int*)pCurrData;
			pCurrData += 4;
			this._participant1 = *(int*)pCurrData;
			pCurrData += 4;
			this._participant2 = *(int*)pCurrData;
			pCurrData += 4;
			this._participant3 = *(int*)pCurrData;
			pCurrData += 4;
			this._participant4 = *(int*)pCurrData;
			pCurrData += 4;
			this._participant5 = *(int*)pCurrData;
			pCurrData += 4;
			this._participant6 = *(int*)pCurrData;
			pCurrData += 4;
			this._participant7 = *(int*)pCurrData;
			pCurrData += 4;
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

		// Token: 0x040005E9 RID: 1513
		[SerializableGameDataField]
		public Location Location;

		// Token: 0x040005EA RID: 1514
		[SerializableGameDataField]
		private int _leaderId;

		// Token: 0x040005EB RID: 1515
		[SerializableGameDataField]
		private int _participant1;

		// Token: 0x040005EC RID: 1516
		[SerializableGameDataField]
		private int _participant2;

		// Token: 0x040005ED RID: 1517
		[SerializableGameDataField]
		private int _participant3;

		// Token: 0x040005EE RID: 1518
		[SerializableGameDataField]
		private int _participant4;

		// Token: 0x040005EF RID: 1519
		[SerializableGameDataField]
		private int _participant5;

		// Token: 0x040005F0 RID: 1520
		[SerializableGameDataField]
		private int _participant6;

		// Token: 0x040005F1 RID: 1521
		[SerializableGameDataField]
		private int _participant7;

		// Token: 0x040005F2 RID: 1522
		public const string LeaderNameStr = "MajorCharacter_0_0";

		// Token: 0x040005F3 RID: 1523
		public const string Participant1NameStr = "MajorCharacter_0_1";

		// Token: 0x040005F4 RID: 1524
		public const string Participant2NameStr = "MajorCharacter_0_2";

		// Token: 0x040005F5 RID: 1525
		public const string Participant3NameStr = "MajorCharacter_0_3";

		// Token: 0x040005F6 RID: 1526
		public const string Participant4NameStr = "MajorCharacter_0_4";

		// Token: 0x040005F7 RID: 1527
		public const string Participant5NameStr = "MajorCharacter_0_5";

		// Token: 0x040005F8 RID: 1528
		public const string Participant6NameStr = "MajorCharacter_0_6";

		// Token: 0x040005F9 RID: 1529
		public const string Participant7NameStr = "MajorCharacter_0_7";
	}
}
