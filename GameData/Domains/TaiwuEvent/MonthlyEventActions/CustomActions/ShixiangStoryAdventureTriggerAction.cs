using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Creation;
using GameData.Domains.Character.Filters;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions
{
	// Token: 0x0200009D RID: 157
	[SerializableGameData(NotForDisplayModule = true)]
	public class ShixiangStoryAdventureTriggerAction : MonthlyActionBase, IDynamicAction, ISerializableGameData
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06001A14 RID: 6676 RVA: 0x0017489B File Offset: 0x00172A9B
		public short DynamicActionType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x0017489E File Offset: 0x00172A9E
		public ShixiangStoryAdventureTriggerAction()
		{
			this._sectLeaderId = -1;
			this._literatiId = -1;
			this.Location = Location.Invalid;
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x001748C1 File Offset: 0x00172AC1
		public override void MonthlyHandler()
		{
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x001748C4 File Offset: 0x00172AC4
		public override void TriggerAction()
		{
			bool flag = this.State != 0;
			if (!flag)
			{
				DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
				bool flag2 = DomainManager.Adventure.TryCreateAdventureSite(context, this.Location.AreaId, this.Location.BlockId, 176, this.Key);
				if (flag2)
				{
					DomainManager.Adventure.ActivateAdventureSite(context, this.Location.AreaId, this.Location.BlockId);
					this.CallShixiangLeader(context);
					this.CallLiterati(context);
					DomainManager.World.GetMonthlyNotificationCollection().AddSectMainStoryShixiangAdventure();
					DomainManager.Extra.TriggerExtraTask(context, 34, 170);
				}
			}
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x00174978 File Offset: 0x00172B78
		public override void CollectCalledCharacters(HashSet<int> calledCharacters)
		{
			bool flag = this._sectLeaderId >= 0;
			if (flag)
			{
				calledCharacters.Add(this._sectLeaderId);
			}
			bool flag2 = this._literatiId >= 0;
			if (flag2)
			{
				calledCharacters.Add(this._literatiId);
			}
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x001749C0 File Offset: 0x00172BC0
		public override void Deactivate(bool isComplete)
		{
			this.State = 0;
			this.Month = 0;
			this.LastFinishDate = DomainManager.World.GetCurrDate();
			this.Location = Location.Invalid;
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			ShixiangStoryAdventureTriggerAction.ReleaseCharacter(context, this._sectLeaderId);
			ShixiangStoryAdventureTriggerAction.ReleaseCharacter(context, this._literatiId);
			this._sectLeaderId = -1;
			this._literatiId = -1;
			bool flag = !isComplete;
			if (flag)
			{
				int nextAppearDate = this.LastFinishDate + 3;
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, 6, "ConchShip_PresetKey_ShixiangAdventureAppearDate", nextAppearDate);
			}
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x00174A50 File Offset: 0x00172C50
		public override MonthlyActionBase CreateCopy()
		{
			return Serializer.CreateCopy<ShixiangStoryAdventureTriggerAction>(this);
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x00174A68 File Offset: 0x00172C68
		public override void EnsurePrerequisites()
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			bool flag = this.IsLeaderOutdated();
			if (flag)
			{
				ShixiangStoryAdventureTriggerAction.ReleaseCharacter(context, this._sectLeaderId);
				this._sectLeaderId = -1;
				this.CallShixiangLeader(context);
			}
			bool flag2 = this.IsLiteratiOutdated();
			if (flag2)
			{
				ShixiangStoryAdventureTriggerAction.ReleaseCharacter(context, this._literatiId);
				this._literatiId = -1;
				this.CallLiterati(context);
			}
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x00174AD4 File Offset: 0x00172CD4
		public override void FillEventArgBox(EventArgBox eventArgBox)
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			GameData.Domains.Character.Character leader;
			bool flag = this._sectLeaderId < 0 || !DomainManager.Character.TryGetElement_Objects(this._sectLeaderId, out leader);
			if (flag)
			{
				Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(6);
				OrganizationItem orgTemplate = Organization.Instance[6];
				sbyte gender = orgTemplate.GenderRestriction;
				bool flag2 = gender < 0;
				if (flag2)
				{
					gender = Gender.GetRandom(context.Random);
				}
				sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(settlement.GetLocation().AreaId);
				short charTemplateId = OrganizationDomain.GetCharacterTemplateId(orgTemplate.TemplateId, stateTemplateId, gender);
				TemporaryIntelligentCharacterCreationInfo tempCreationInfo = new TemporaryIntelligentCharacterCreationInfo
				{
					Location = this.Location,
					CharTemplateId = charTemplateId,
					OrgInfo = new OrganizationInfo(orgTemplate.TemplateId, 8, true, settlement.GetId())
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
				eventArgBox.Set("MajorCharacter_0_0", charId);
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Adding 1 major characters ");
				defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(leader);
				defaultInterpolatedStringHandler.AppendLiteral(" to adventure.");
				AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				eventArgBox.Set("MajorCharacter_0_0", this._sectLeaderId);
			}
			GameData.Domains.Character.Character literati;
			bool flag3 = this._literatiId < 0 || !DomainManager.Character.TryGetElement_Objects(this._literatiId, out literati);
			if (flag3)
			{
				Settlement settlement2 = DomainManager.Organization.GetSettlementByOrgTemplateId(26);
				OrganizationItem orgTemplate2 = Organization.Instance[26];
				sbyte gender2 = 1;
				sbyte stateTemplateId2 = DomainManager.Map.GetStateTemplateIdByAreaId(settlement2.GetLocation().AreaId);
				short charTemplateId2 = OrganizationDomain.GetCharacterTemplateId(orgTemplate2.TemplateId, stateTemplateId2, gender2);
				TemporaryIntelligentCharacterCreationInfo tempCreationInfo2 = new TemporaryIntelligentCharacterCreationInfo
				{
					Location = this.Location,
					CharTemplateId = charTemplateId2,
					OrgInfo = new OrganizationInfo(orgTemplate2.TemplateId, 5, true, settlement2.GetId())
				};
				GameData.Domains.Character.Character character2 = DomainManager.Character.CreateTemporaryIntelligentCharacter(context, ref tempCreationInfo2);
				OrganizationMemberItem orgMemberCfg2 = OrganizationDomain.GetOrgMemberConfig(tempCreationInfo2.OrgInfo);
				ValueTuple<string, string> realName2 = CharacterDomain.GetRealName(character2);
				string surname2 = realName2.Item1;
				string givenName2 = realName2.Item2;
				string tag2 = "MartialArtTournamentMonthlyAction";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 5);
				defaultInterpolatedStringHandler.AppendLiteral("Creating temporary character ");
				defaultInterpolatedStringHandler.AppendFormatted(orgTemplate2.Name);
				defaultInterpolatedStringHandler.AppendLiteral("-");
				defaultInterpolatedStringHandler.AppendFormatted(orgMemberCfg2.GradeName);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted(surname2);
				defaultInterpolatedStringHandler.AppendFormatted(givenName2);
				defaultInterpolatedStringHandler.AppendLiteral("(");
				defaultInterpolatedStringHandler.AppendFormatted<int>(character2.GetId());
				defaultInterpolatedStringHandler.AppendLiteral(")");
				AdaptableLog.TagInfo(tag2, defaultInterpolatedStringHandler.ToStringAndClear());
				int charId2 = character2.GetId();
				DomainManager.Adventure.AddTemporaryIntelligentCharacter(charId2);
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Adding 1 temporary major characters ");
				defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character2);
				defaultInterpolatedStringHandler.AppendLiteral(" to adventure.");
				AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				eventArgBox.Set("MajorCharacter_0_1", charId2);
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Adding 1 major characters ");
				defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(literati);
				defaultInterpolatedStringHandler.AppendLiteral(" to adventure.");
				AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				eventArgBox.Set("MajorCharacter_0_1", this._literatiId);
			}
			eventArgBox.Set("MajorCharacter_0_Count", 2);
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x00174F7C File Offset: 0x0017317C
		private void CallShixiangLeader(DataContext context)
		{
			Tester.Assert(this._sectLeaderId < 0, "");
			Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(6);
			GameData.Domains.Character.Character leader = settlement.GetLeader();
			bool flag = leader == null;
			if (!flag)
			{
				bool flag2 = leader.IsActiveExternalRelationState(60);
				if (!flag2)
				{
					bool flag3 = leader.GetKidnapperId() >= 0;
					if (!flag3)
					{
						bool flag4 = leader.GetAgeGroup() == 0;
						if (!flag4)
						{
							this._sectLeaderId = leader.GetId();
							DomainManager.Character.LeaveGroup(context, leader, true);
							DomainManager.Character.GroupMove(context, leader, this.Location);
							Events.RaiseCharacterLocationChanged(context, this._sectLeaderId, this.Location, Location.Invalid);
							leader.ActiveExternalRelationState(context, 4);
						}
					}
				}
			}
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x00175040 File Offset: 0x00173240
		private void CallLiterati(DataContext context)
		{
			List<GameData.Domains.Character.Character> targetList = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
			List<short> areaList = ObjectPool<List<short>>.Instance.Get();
			targetList.Clear();
			areaList.Clear();
			sbyte stateId = DomainManager.Map.GetStateIdByAreaId(this.Location.AreaId);
			DomainManager.Map.GetAllAreaInState(stateId, areaList);
			MapCharacterFilter.ParallelFind(new Predicate<GameData.Domains.Character.Character>(ShixiangStoryAdventureTriggerAction.IsLiteratiValid), targetList, areaList, false);
			bool flag = targetList.Count > 0;
			if (flag)
			{
				GameData.Domains.Character.Character selectedChar = targetList.GetRandom(context.Random);
				this._literatiId = selectedChar.GetId();
				DomainManager.Character.LeaveGroup(context, selectedChar, true);
				DomainManager.Character.GroupMove(context, selectedChar, this.Location);
				Events.RaiseCharacterLocationChanged(context, this._sectLeaderId, this.Location, Location.Invalid);
				selectedChar.ActiveExternalRelationState(context, 4);
			}
			ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(targetList);
			ObjectPool<List<short>>.Instance.Return(areaList);
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x00175134 File Offset: 0x00173334
		private static bool IsLiteratiValid(GameData.Domains.Character.Character character)
		{
			bool flag = character.GetAgeGroup() != 2;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = character.GetGender() != 1;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = character.IsActiveExternalRelationState(60);
					if (flag3)
					{
						result = false;
					}
					else
					{
						OrganizationInfo orgInfo = character.GetOrganizationInfo();
						bool flag4 = !Organization.Instance[orgInfo.OrgTemplateId].IsCivilian;
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool flag5 = orgInfo.Grade != 5;
							if (flag5)
							{
								result = false;
							}
							else
							{
								bool flag6 = character.GetKidnapperId() >= 0;
								result = !flag6;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x001751D8 File Offset: 0x001733D8
		private bool IsLeaderOutdated()
		{
			bool flag = this._sectLeaderId < 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				GameData.Domains.Character.Character leader;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(this._sectLeaderId, out leader);
				if (flag2)
				{
					result = true;
				}
				else
				{
					OrganizationInfo orgInfo = leader.GetOrganizationInfo();
					bool flag3 = orgInfo.OrgTemplateId != 6;
					if (flag3)
					{
						result = true;
					}
					else
					{
						bool flag4 = orgInfo.Grade != 8;
						if (flag4)
						{
							result = true;
						}
						else
						{
							bool flag5 = !orgInfo.Principal;
							if (flag5)
							{
								result = true;
							}
							else
							{
								bool flag6 = leader.GetKidnapperId() >= 0;
								if (flag6)
								{
									result = true;
								}
								else
								{
									bool flag7 = leader.GetAgeGroup() == 0;
									if (flag7)
									{
										result = true;
									}
									else
									{
										bool flag8 = leader.GetLocation() != this.Location;
										result = flag8;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x001752AC File Offset: 0x001734AC
		private bool IsLiteratiOutdated()
		{
			bool flag = this._literatiId < 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				GameData.Domains.Character.Character character;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(this._literatiId, out character);
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = character.GetAgeGroup() != 2;
					if (flag3)
					{
						result = true;
					}
					else
					{
						bool flag4 = character.GetGender() != 1;
						if (flag4)
						{
							result = true;
						}
						else
						{
							OrganizationInfo orgInfo = character.GetOrganizationInfo();
							bool flag5 = !Organization.Instance[orgInfo.OrgTemplateId].IsCivilian;
							if (flag5)
							{
								result = true;
							}
							else
							{
								bool flag6 = orgInfo.Grade != 5;
								if (flag6)
								{
									result = true;
								}
								else
								{
									bool flag7 = character.GetLocation() != this.Location;
									if (flag7)
									{
										result = true;
									}
									else
									{
										bool flag8 = character.GetKidnapperId() >= 0;
										result = flag8;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x00175394 File Offset: 0x00173594
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

		// Token: 0x06001A23 RID: 6691 RVA: 0x001753F4 File Offset: 0x001735F4
		public override bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x00175408 File Offset: 0x00173608
		public override int GetSerializedSize()
		{
			int totalSize = 26;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x00175430 File Offset: 0x00173630
		public unsafe override int Serialize(byte* pData)
		{
			*(short*)pData = this.DynamicActionType;
			byte* pCurrData = pData + 2;
			pCurrData += this.Location.Serialize(pCurrData);
			*(int*)pCurrData = this._sectLeaderId;
			pCurrData += 4;
			*(int*)pCurrData = this._literatiId;
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

		// Token: 0x06001A26 RID: 6694 RVA: 0x001754C4 File Offset: 0x001736C4
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + 2;
			pCurrData += this.Location.Deserialize(pCurrData);
			this._sectLeaderId = *(int*)pCurrData;
			pCurrData += 4;
			this._literatiId = *(int*)pCurrData;
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

		// Token: 0x04000607 RID: 1543
		[SerializableGameDataField]
		public Location Location;

		// Token: 0x04000608 RID: 1544
		[SerializableGameDataField]
		private int _sectLeaderId;

		// Token: 0x04000609 RID: 1545
		[SerializableGameDataField]
		private int _literatiId;
	}
}
