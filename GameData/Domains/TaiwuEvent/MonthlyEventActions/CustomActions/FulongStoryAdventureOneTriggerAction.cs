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
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions
{
	// Token: 0x02000099 RID: 153
	[SerializableGameData(NotForDisplayModule = true)]
	public class FulongStoryAdventureOneTriggerAction : MonthlyActionBase, IDynamicAction, ISerializableGameData
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060019BE RID: 6590 RVA: 0x00170E21 File Offset: 0x0016F021
		public short DynamicActionType
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x00170E24 File Offset: 0x0016F024
		public FulongStoryAdventureOneTriggerAction()
		{
			this.InitCharacterId();
			this.Location = Location.Invalid;
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x00170E40 File Offset: 0x0016F040
		public override void MonthlyHandler()
		{
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x00170E44 File Offset: 0x0016F044
		public override void TriggerAction()
		{
			bool flag = this.State != 0;
			if (!flag)
			{
				DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
				Settlement settlement = this.GetSettlement();
				this.Location = settlement.GetLocation();
				List<short> blocks = new List<short>();
				DomainManager.Map.GetSettlementBlocksWithoutAdventure(this.Location.AreaId, this.Location.BlockId, blocks);
				bool flag2 = blocks.Count <= 0;
				if (!flag2)
				{
					short blockId = blocks.GetRandom(context.Random);
					bool flag3 = DomainManager.Adventure.TryCreateAdventureSite(context, this.Location.AreaId, blockId, 191, this.Key);
					if (flag3)
					{
						DomainManager.Adventure.ActivateAdventureSite(context, this.Location.AreaId, blockId);
						DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, 14, "ConchShip_PresetKey_FulongAdventureOneCountDown", 6);
						DomainManager.Extra.TriggerExtraTask(context, 52, 330);
						DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<bool>(context, 14, "ConchShip_PresetKey_FulongDisasterStart", false);
						this.CallLeader(context);
						this.CallParticipant(context, ref this._participant1, 0, 2);
						this.CallParticipant(context, ref this._participant2, 0, 2);
						this.CallParticipant(context, ref this._participant3, 0, 2);
						this.CallParticipant(context, ref this._participant4, 3, 5);
						this.CallParticipant(context, ref this._participant5, 3, 5);
						this.CallParticipant(context, ref this._participant6, 3, 5);
						this.CallParticipant(context, ref this._participant7, 6, 7);
						this.CallParticipant(context, ref this._participant8, 6, 7);
						this.CallParticipant(context, ref this._participant9, 6, 7);
					}
				}
			}
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x00170FEC File Offset: 0x0016F1EC
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
			bool flag9 = this._participant8 >= 0;
			if (flag9)
			{
				calledCharacters.Add(this._participant8);
			}
			bool flag10 = this._participant9 >= 0;
			if (flag10)
			{
				calledCharacters.Add(this._participant9);
			}
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x0017112C File Offset: 0x0016F32C
		public override void Deactivate(bool isComplete)
		{
			this.State = 0;
			this.Month = 0;
			this.LastFinishDate = DomainManager.World.GetCurrDate();
			this.Location = Location.Invalid;
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			FulongStoryAdventureOneTriggerAction.ReleaseCharacter(context, this._leaderId);
			FulongStoryAdventureOneTriggerAction.ReleaseCharacter(context, this._participant1);
			FulongStoryAdventureOneTriggerAction.ReleaseCharacter(context, this._participant2);
			FulongStoryAdventureOneTriggerAction.ReleaseCharacter(context, this._participant3);
			FulongStoryAdventureOneTriggerAction.ReleaseCharacter(context, this._participant4);
			FulongStoryAdventureOneTriggerAction.ReleaseCharacter(context, this._participant5);
			FulongStoryAdventureOneTriggerAction.ReleaseCharacter(context, this._participant6);
			FulongStoryAdventureOneTriggerAction.ReleaseCharacter(context, this._participant7);
			FulongStoryAdventureOneTriggerAction.ReleaseCharacter(context, this._participant8);
			FulongStoryAdventureOneTriggerAction.ReleaseCharacter(context, this._participant9);
			this.InitCharacterId();
			bool flag = !isComplete;
			if (flag)
			{
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<bool>(context, 14, "ConchShip_PresetKey_FulongDisasterStart", true);
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, 14, "ConchShip_PresetKey_FulongDisasterStartProb", 100);
				DomainManager.Extra.TriggerExtraTask(context, 52, 329);
			}
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x00171244 File Offset: 0x0016F444
		public override MonthlyActionBase CreateCopy()
		{
			return Serializer.CreateCopy<FulongStoryAdventureOneTriggerAction>(this);
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x0017125C File Offset: 0x0016F45C
		public override void EnsurePrerequisites()
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			bool flag = this.IsLeaderOutdated();
			if (flag)
			{
				FulongStoryAdventureOneTriggerAction.ReleaseCharacter(context, this._leaderId);
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
			this.HandleParticipantOutdated(context, ref this._participant8);
			this.HandleParticipantOutdated(context, ref this._participant9);
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x00171320 File Offset: 0x0016F520
		private Settlement GetSettlement()
		{
			return DomainManager.Organization.GetSettlementByOrgTemplateId(14);
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x00171340 File Offset: 0x0016F540
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
			this.FillParticipantEventArgBox(context, eventArgBox, this._participant1, "MajorCharacter_0_1_0", 0, 2);
			this.FillParticipantEventArgBox(context, eventArgBox, this._participant2, "MajorCharacter_0_1_1", 0, 2);
			this.FillParticipantEventArgBox(context, eventArgBox, this._participant3, "MajorCharacter_0_1_2", 0, 2);
			this.FillParticipantEventArgBox(context, eventArgBox, this._participant4, "MajorCharacter_0_2_0", 3, 5);
			this.FillParticipantEventArgBox(context, eventArgBox, this._participant5, "MajorCharacter_0_2_1", 3, 5);
			this.FillParticipantEventArgBox(context, eventArgBox, this._participant6, "MajorCharacter_0_2_2", 3, 5);
			this.FillParticipantEventArgBox(context, eventArgBox, this._participant7, "MajorCharacter_0_3_0", 6, 8);
			this.FillParticipantEventArgBox(context, eventArgBox, this._participant8, "MajorCharacter_0_3_1", 6, 8);
			this.FillParticipantEventArgBox(context, eventArgBox, this._participant9, "MajorCharacter_0_3_2", 6, 8);
			eventArgBox.Set("MajorCharacter_0_Count", 10);
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x001714A0 File Offset: 0x0016F6A0
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

		// Token: 0x060019C9 RID: 6601 RVA: 0x00171674 File Offset: 0x0016F874
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

		// Token: 0x060019CA RID: 6602 RVA: 0x00171710 File Offset: 0x0016F910
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

		// Token: 0x060019CB RID: 6603 RVA: 0x001717B8 File Offset: 0x0016F9B8
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

		// Token: 0x060019CC RID: 6604 RVA: 0x00171818 File Offset: 0x0016FA18
		private void CallParticipant(DataContext context, ref int participantId, sbyte minGrade, sbyte maxGrade)
		{
			Tester.Assert(participantId < 0, "");
			Settlement settlement = this.GetSettlement();
			GameData.Domains.Character.Character participant = settlement.GetAvailableHighMember(maxGrade, minGrade, true);
			bool flag = participant == null;
			if (!flag)
			{
				participantId = participant.GetId();
				DomainManager.Character.LeaveGroup(context, participant, true);
				DomainManager.Character.GroupMove(context, participant, this.Location);
				Events.RaiseCharacterLocationChanged(context, this._leaderId, this.Location, Location.Invalid);
				participant.ActiveExternalRelationState(context, 4);
			}
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x0017189C File Offset: 0x0016FA9C
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

		// Token: 0x060019CE RID: 6606 RVA: 0x00171904 File Offset: 0x0016FB04
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
			this._participant8 = -1;
			this._participant9 = -1;
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x00171958 File Offset: 0x0016FB58
		private void HandleParticipantOutdated(DataContext context, ref int characterId)
		{
			bool flag = this.IsParticipantOutdated(characterId);
			if (flag)
			{
				FulongStoryAdventureOneTriggerAction.ReleaseCharacter(context, characterId);
				characterId = -1;
				this.CallParticipant(context, ref characterId, 0, 2);
			}
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x0017198C File Offset: 0x0016FB8C
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

		// Token: 0x060019D1 RID: 6609 RVA: 0x00171A1C File Offset: 0x0016FC1C
		public override bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x00171A30 File Offset: 0x0016FC30
		public override int GetSerializedSize()
		{
			int totalSize = 58;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x00171A58 File Offset: 0x0016FC58
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
			*(int*)pCurrData = this._participant8;
			pCurrData += 4;
			*(int*)pCurrData = this._participant9;
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

		// Token: 0x060019D4 RID: 6612 RVA: 0x00171B4C File Offset: 0x0016FD4C
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
			this._participant8 = *(int*)pCurrData;
			pCurrData += 4;
			this._participant9 = *(int*)pCurrData;
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

		// Token: 0x040005D4 RID: 1492
		[SerializableGameDataField]
		public Location Location;

		// Token: 0x040005D5 RID: 1493
		[SerializableGameDataField]
		private int _leaderId;

		// Token: 0x040005D6 RID: 1494
		[SerializableGameDataField]
		private int _participant1;

		// Token: 0x040005D7 RID: 1495
		[SerializableGameDataField]
		private int _participant2;

		// Token: 0x040005D8 RID: 1496
		[SerializableGameDataField]
		private int _participant3;

		// Token: 0x040005D9 RID: 1497
		[SerializableGameDataField]
		private int _participant4;

		// Token: 0x040005DA RID: 1498
		[SerializableGameDataField]
		private int _participant5;

		// Token: 0x040005DB RID: 1499
		[SerializableGameDataField]
		private int _participant6;

		// Token: 0x040005DC RID: 1500
		[SerializableGameDataField]
		private int _participant7;

		// Token: 0x040005DD RID: 1501
		[SerializableGameDataField]
		private int _participant8;

		// Token: 0x040005DE RID: 1502
		[SerializableGameDataField]
		private int _participant9;

		// Token: 0x040005DF RID: 1503
		public const string LeaderNameStr = "MajorCharacter_0_0";

		// Token: 0x040005E0 RID: 1504
		public const string Participant1NameStr = "MajorCharacter_0_1_0";

		// Token: 0x040005E1 RID: 1505
		public const string Participant2NameStr = "MajorCharacter_0_1_1";

		// Token: 0x040005E2 RID: 1506
		public const string Participant3NameStr = "MajorCharacter_0_1_2";

		// Token: 0x040005E3 RID: 1507
		public const string Participant4NameStr = "MajorCharacter_0_2_0";

		// Token: 0x040005E4 RID: 1508
		public const string Participant5NameStr = "MajorCharacter_0_2_1";

		// Token: 0x040005E5 RID: 1509
		public const string Participant6NameStr = "MajorCharacter_0_2_2";

		// Token: 0x040005E6 RID: 1510
		public const string Participant7NameStr = "MajorCharacter_0_3_0";

		// Token: 0x040005E7 RID: 1511
		public const string Participant8NameStr = "MajorCharacter_0_3_1";

		// Token: 0x040005E8 RID: 1512
		public const string Participant9NameStr = "MajorCharacter_0_3_2";
	}
}
