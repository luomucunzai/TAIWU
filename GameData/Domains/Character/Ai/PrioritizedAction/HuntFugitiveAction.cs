using System;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.World.MonthlyEvent;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x0200085D RID: 2141
	[SerializableGameData(NotForDisplayModule = true, IsExtensible = true, NoCopyConstructors = true)]
	public class HuntFugitiveAction : ExtensiblePrioritizedAction
	{
		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x060076F5 RID: 30453 RVA: 0x00459A94 File Offset: 0x00457C94
		public override short ActionType
		{
			get
			{
				return 17;
			}
		}

		// Token: 0x060076F6 RID: 30454 RVA: 0x00459A98 File Offset: 0x00457C98
		public override bool CheckValid(Character selfChar)
		{
			OrganizationInfo orgInfo = selfChar.GetOrganizationInfo();
			Sect sect;
			bool flag = !DomainManager.Organization.TryGetElement_Sects(orgInfo.SettlementId, out sect);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				SettlementBounty bounty = sect.Prison.GetBounty(this.Target.TargetCharId);
				bool flag2 = bounty == null;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = bounty.CurrentHunterId >= 0 && bounty.CurrentHunterId != selfChar.GetId();
					if (flag3)
					{
						result = false;
					}
					else
					{
						Character targetChar;
						bool flag4 = !DomainManager.Character.TryGetElement_Objects(bounty.CharId, out targetChar);
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool flag5 = OrganizationDomain.IsLargeSect((short)targetChar.GetOrganizationInfo().OrgTemplateId);
							result = (!flag5 && base.CheckValid(selfChar));
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060076F7 RID: 30455 RVA: 0x00459B68 File Offset: 0x00457D68
		public override void OnStart(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			OrganizationInfo orgInfo = selfChar.GetOrganizationInfo();
			Sect sect = DomainManager.Organization.GetElement_Sects(orgInfo.SettlementId);
			SettlementBounty bounty = sect.Prison.GetBounty(this.Target.TargetCharId);
			bool flag = bounty.CurrentHunterId >= 0;
			if (!flag)
			{
				bounty.CurrentHunterId = selfCharId;
				DomainManager.Extra.SetSettlementPrison(context, orgInfo.SettlementId, sect.Prison);
				int leaderId = selfChar.GetLeaderId();
				bool flag2 = leaderId >= 0 && leaderId != selfCharId;
				if (flag2)
				{
					DomainManager.Character.LeaveGroup(context, selfChar, true);
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 2);
				defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
				defaultInterpolatedStringHandler.AppendLiteral(" 开始追捕逃犯 ");
				defaultInterpolatedStringHandler.AppendFormatted<Character>(DomainManager.Character.GetElement_Objects(bounty.CharId));
				defaultInterpolatedStringHandler.AppendLiteral(".");
				AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				Location location = selfChar.GetLocation();
				int currDate = DomainManager.World.GetCurrDate();
				LifeRecordCollection lifeRecords = DomainManager.LifeRecord.GetLifeRecordCollection();
				lifeRecords.AddDecideToHuntFugitive(selfCharId, currDate, this.Target.TargetCharId, location);
			}
		}

		// Token: 0x060076F8 RID: 30456 RVA: 0x00459C98 File Offset: 0x00457E98
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			OrganizationInfo orgInfo = selfChar.GetOrganizationInfo();
			sbyte targetBountySect = DomainManager.Organization.GetFugitiveBountySect(this.Target.TargetCharId);
			bool flag = targetBountySect >= 0;
			if (flag)
			{
				Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(targetBountySect);
				SettlementBounty bounty = sect.Prison.GetBounty(this.Target.TargetCharId);
				bool flag2 = bounty.CurrentHunterId == selfChar.GetId();
				if (flag2)
				{
					bounty.CurrentHunterId = -1;
					DomainManager.Extra.SetSettlementPrison(context, orgInfo.SettlementId, sect.Prison);
				}
			}
			int selfCharId = selfChar.GetId();
			Location location = selfChar.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecords = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecords.AddFinishHuntFugitive(selfCharId, currDate, this.Target.TargetCharId, location);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
			defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
			defaultInterpolatedStringHandler.AppendLiteral(" 终止追捕逃犯.");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060076F9 RID: 30457 RVA: 0x00459DA0 File Offset: 0x00457FA0
		public override bool Execute(DataContext context, Character selfChar)
		{
			Character targetChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(this.Target.TargetCharId, out targetChar);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				int selfCharId = selfChar.GetId();
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				bool flag2 = targetChar.GetId() == taiwuCharId;
				if (flag2)
				{
					bool worldFunctionsStatus = DomainManager.World.GetWorldFunctionsStatus(4);
					if (worldFunctionsStatus)
					{
						MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
						monthlyEventCollection.AddHuntCriminalTaiwu(selfCharId, this.Target.TargetCharId, selfChar.GetLocation());
						CharacterDomain.AddLockMovementCharSet(selfCharId);
					}
					result = false;
				}
				else
				{
					bool flag3 = targetChar.GetLeaderId() == taiwuCharId;
					if (flag3)
					{
						MonthlyEventCollection monthlyEventCollection2 = DomainManager.World.GetMonthlyEventCollection();
						monthlyEventCollection2.AddHuntCriminal(taiwuCharId, selfCharId, this.Target.TargetCharId);
						CharacterDomain.AddLockMovementCharSet(selfCharId);
						result = false;
					}
					else
					{
						bool flag4 = !targetChar.GetLocation().IsValid() || !targetChar.IsInteractableAsIntelligentCharacter();
						if (flag4)
						{
							result = false;
						}
						else
						{
							OrganizationInfo orgInfo = selfChar.GetOrganizationInfo();
							Sect sect = DomainManager.Organization.GetElement_Sects(orgInfo.SettlementId);
							SettlementPrison prison = sect.Prison;
							SettlementBounty bounty = prison.GetBounty(this.Target.TargetCharId);
							PunishmentSeverityItem punishSeverityCfg = PunishmentSeverity.Instance[bounty.PunishmentSeverity];
							Location location = selfChar.GetLocation();
							int currDate = DomainManager.World.GetCurrDate();
							AiHelper.NpcCombatResultType resultType = DomainManager.Character.SimulateCharacterCombat(context, selfChar, targetChar, CombatType.Beat, true, 1);
							bool flag5 = resultType <= AiHelper.NpcCombatResultType.MinorVictory;
							bool flag6 = flag5;
							if (flag6)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 2);
								defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
								defaultInterpolatedStringHandler.AppendLiteral(" 成功捕获逃犯 ");
								defaultInterpolatedStringHandler.AppendFormatted<Character>(targetChar);
								defaultInterpolatedStringHandler.AppendLiteral(".");
								AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
								LifeRecordCollection lifeRecords = DomainManager.LifeRecord.GetLifeRecordCollection();
								lifeRecords.AddArrestedSuccessfullyCaptor(selfCharId, currDate, this.Target.TargetCharId, location, orgInfo.SettlementId, 889);
								BasePrioritizedAction prevAction;
								bool flag7 = DomainManager.Character.TryGetCharacterPrioritizedAction(this.Target.TargetCharId, out prevAction);
								if (flag7)
								{
									DomainManager.Character.RemoveCharacterPrioritizedAction(context, this.Target.TargetCharId);
								}
								bool flag8 = punishSeverityCfg.PrisonTime > 0 || punishSeverityCfg.Expel;
								if (flag8)
								{
									DomainManager.Character.CombatResultHandle_KidnapEnemy(context, selfChar, targetChar, true);
								}
								else
								{
									DomainManager.Organization.PunishSectMember(context, sect, targetChar, bounty.PunishmentSeverity, bounty.PunishmentType, true);
									sect.RemoveBounty(context, targetChar.GetId());
									selfChar.RecordFameAction(context, 83, targetChar.GetId(), bounty.CaptorFameActionMultiplier, true);
								}
							}
							else
							{
								LifeRecordCollection lifeRecords2 = DomainManager.LifeRecord.GetLifeRecordCollection();
								lifeRecords2.AddArrestFailedCaptor(selfCharId, currDate, this.Target.TargetCharId, location, orgInfo.SettlementId, 886);
								bounty.CurrentHunterId = -1;
								bool flag9 = bounty.RequiredConsummateLevel < 0;
								if (flag9)
								{
									bounty.RequiredConsummateLevel = targetChar.GetConsummateLevel();
								}
								SettlementBounty settlementBounty = bounty;
								settlementBounty.RequiredConsummateLevel += 1;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 2);
								defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
								defaultInterpolatedStringHandler.AppendLiteral(" 未能战胜逃犯 ");
								defaultInterpolatedStringHandler.AppendFormatted<Character>(targetChar);
								defaultInterpolatedStringHandler.AppendLiteral(".");
								AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
								DomainManager.Extra.SetSettlementPrison(context, orgInfo.SettlementId, prison);
							}
							result = true;
						}
					}
				}
			}
			return result;
		}
	}
}
