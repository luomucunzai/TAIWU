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
	// Token: 0x02000096 RID: 150
	[SerializableGameData(NotForDisplayModule = true)]
	public class BaihuaStoryAdventureFourTriggerAction : MonthlyActionBase, IDynamicAction, ISerializableGameData
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06001999 RID: 6553 RVA: 0x0016F914 File Offset: 0x0016DB14
		public short DynamicActionType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x0016F917 File Offset: 0x0016DB17
		public BaihuaStoryAdventureFourTriggerAction()
		{
			this._leaderId = -1;
			this.Location = Location.Invalid;
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x0016F933 File Offset: 0x0016DB33
		public override void MonthlyHandler()
		{
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x0016F938 File Offset: 0x0016DB38
		public override void TriggerAction()
		{
			bool flag = this.State != 0;
			if (!flag)
			{
				DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
				Settlement settlement = this.GetVillageSettlement();
				this.Location = settlement.GetLocation();
				bool flag2 = DomainManager.Adventure.TryCreateAdventureSite(context, this.Location.AreaId, this.Location.BlockId, 185, this.Key);
				if (flag2)
				{
					DomainManager.Adventure.ActivateAdventureSite(context, this.Location.AreaId, this.Location.BlockId);
					this.CallVillageLeader(context);
				}
			}
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x0016F9D0 File Offset: 0x0016DBD0
		public override void CollectCalledCharacters(HashSet<int> calledCharacters)
		{
			bool flag = this._leaderId >= 0;
			if (flag)
			{
				calledCharacters.Add(this._leaderId);
			}
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x0016F9FC File Offset: 0x0016DBFC
		public override void Deactivate(bool isComplete)
		{
			this.State = 0;
			this.Month = 0;
			this.LastFinishDate = DomainManager.World.GetCurrDate();
			this.Location = Location.Invalid;
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			BaihuaStoryAdventureFourTriggerAction.ReleaseCharacter(context, this._leaderId);
			this._leaderId = -1;
			bool flag = !isComplete;
			if (flag)
			{
				int nextAppearDate = this.LastFinishDate + 1;
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(context, 3, "ConchShip_PresetKey_BaihuaAdventureFourAppearDate", nextAppearDate);
			}
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x0016FA78 File Offset: 0x0016DC78
		public override MonthlyActionBase CreateCopy()
		{
			return Serializer.CreateCopy<BaihuaStoryAdventureFourTriggerAction>(this);
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x0016FA90 File Offset: 0x0016DC90
		public override void EnsurePrerequisites()
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			bool flag = this.IsLeaderOutdated();
			if (flag)
			{
				BaihuaStoryAdventureFourTriggerAction.ReleaseCharacter(context, this._leaderId);
				this._leaderId = -1;
				this.CallVillageLeader(context);
			}
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x0016FAD4 File Offset: 0x0016DCD4
		private Settlement GetVillageSettlement()
		{
			EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
			short settlementId = -1;
			argBox.Get("ConchShip_PresetKey_BaihuaVillageSettlementIdSelection", ref settlementId);
			return DomainManager.Organization.GetSettlement(settlementId);
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0016FB10 File Offset: 0x0016DD10
		public override void FillEventArgBox(EventArgBox eventArgBox)
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			GameData.Domains.Character.Character leader;
			bool flag = this._leaderId < 0 || !DomainManager.Character.TryGetElement_Objects(this._leaderId, out leader);
			if (flag)
			{
				Settlement settlement = this.GetVillageSettlement();
				bool flag2 = settlement.GetOrgTemplateId() == 37;
				OrganizationItem orgTemplate;
				if (flag2)
				{
					orgTemplate = Organization.Instance[37];
				}
				else
				{
					bool flag3 = settlement.GetOrgTemplateId() == 36;
					if (flag3)
					{
						orgTemplate = Organization.Instance[36];
					}
					else
					{
						orgTemplate = Organization.Instance[38];
					}
				}
				sbyte gender = orgTemplate.GenderRestriction;
				bool flag4 = gender < 0;
				if (flag4)
				{
					gender = Gender.GetRandom(context.Random);
				}
				sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(settlement.GetLocation().AreaId);
				short charTemplateId = OrganizationDomain.GetCharacterTemplateId(orgTemplate.TemplateId, stateTemplateId, gender);
				sbyte grade = (context.Random.Next(0, 2) == 0) ? 6 : 7;
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
				eventArgBox.Set("MajorCharacter_0_0", charId);
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Adding 1 major characters ");
				defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(leader);
				defaultInterpolatedStringHandler.AppendLiteral(" to adventure.");
				AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				eventArgBox.Set("MajorCharacter_0_0", this._leaderId);
			}
			eventArgBox.Set("MajorCharacter_0_Count", 1);
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x0016FDD8 File Offset: 0x0016DFD8
		private void CallVillageLeader(DataContext context)
		{
			Tester.Assert(this._leaderId < 0, "");
			Settlement settlement = this.GetVillageSettlement();
			GameData.Domains.Character.Character leader = settlement.GetAvailableHighMember(8, 6, true);
			bool flag = leader == null;
			if (!flag)
			{
				this._leaderId = leader.GetId();
				DomainManager.Character.LeaveGroup(context, leader, true);
				DomainManager.Character.GroupMove(context, leader, this.Location);
				Events.RaiseCharacterLocationChanged(context, this._leaderId, this.Location, Location.Invalid);
				leader.ActiveExternalRelationState(context, 4);
			}
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x0016FE64 File Offset: 0x0016E064
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
					bool flag3 = orgInfo.OrgTemplateId != 36 || orgInfo.OrgTemplateId != 37 || orgInfo.OrgTemplateId != 38;
					if (flag3)
					{
						result = true;
					}
					else
					{
						bool flag4 = orgInfo.Grade < 6;
						if (flag4)
						{
							result = true;
						}
						else
						{
							bool flag5 = leader.GetKidnapperId() >= 0;
							if (flag5)
							{
								result = true;
							}
							else
							{
								bool flag6 = leader.GetAgeGroup() < 2;
								if (flag6)
								{
									result = true;
								}
								else
								{
									bool flag7 = leader.GetLocation() != this.Location;
									result = flag7;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0016FF3C File Offset: 0x0016E13C
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

		// Token: 0x060019A6 RID: 6566 RVA: 0x0016FF9C File Offset: 0x0016E19C
		public override bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x0016FFB0 File Offset: 0x0016E1B0
		public override int GetSerializedSize()
		{
			int totalSize = 20;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x0016FFD8 File Offset: 0x0016E1D8
		public unsafe override int Serialize(byte* pData)
		{
			*(short*)pData = this.DynamicActionType;
			byte* pCurrData = pData + 2;
			pCurrData += this.Location.Serialize(pCurrData);
			*(int*)pCurrData = this._leaderId;
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

		// Token: 0x060019A9 RID: 6569 RVA: 0x00170060 File Offset: 0x0016E260
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + 2;
			pCurrData += this.Location.Deserialize(pCurrData);
			this._leaderId = *(int*)pCurrData;
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

		// Token: 0x040005C8 RID: 1480
		[SerializableGameDataField]
		public Location Location;

		// Token: 0x040005C9 RID: 1481
		[SerializableGameDataField]
		private int _leaderId;

		// Token: 0x040005CA RID: 1482
		public const string CharacterNameStr = "MajorCharacter_0_0";
	}
}
