using System;
using GameData.Common;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x02000864 RID: 2148
	[SerializableGameData(NotForDisplayModule = true)]
	public class RescueFriendOrFamilyAction : BasePrioritizedAction
	{
		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x0600771E RID: 30494 RVA: 0x0045AF78 File Offset: 0x00459178
		public override short ActionType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x0600771F RID: 30495 RVA: 0x0045AF7C File Offset: 0x0045917C
		public override bool CheckValid(Character selfChar)
		{
			bool flag = !base.CheckValid(selfChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Character targetChar;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(this.Target.TargetCharId, out targetChar);
				if (flag2)
				{
					result = false;
				}
				else
				{
					Location targetLocation = targetChar.GetLocation();
					Location selfLocation = selfChar.GetLocation();
					int kidnapperId = targetChar.GetKidnapperId();
					bool flag3 = kidnapperId == selfChar.GetId();
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = kidnapperId < 0 && selfLocation.IsValid() && selfLocation.AreaId == targetLocation.AreaId;
						if (flag4)
						{
							byte areaSize = DomainManager.Map.GetAreaSize(selfLocation.AreaId);
							ByteCoordinate selfCoordinate = ByteCoordinate.IndexToCoordinate(selfLocation.BlockId, areaSize);
							ByteCoordinate targetCoordinate = ByteCoordinate.IndexToCoordinate(targetLocation.BlockId, areaSize);
							bool flag5 = selfCoordinate.GetManhattanDistance(targetCoordinate) <= 3;
							if (flag5)
							{
								return false;
							}
						}
						short favorability = DomainManager.Character.GetFavorability(selfChar.GetId(), this.Target.TargetCharId);
						sbyte favorType = FavorabilityType.GetFavorabilityType(favorability);
						result = (favorType >= AiHelper.PrioritizedActionConstants.PrioritizedActionMinFavorType[(int)this.ActionType]);
					}
				}
			}
			return result;
		}

		// Token: 0x06007720 RID: 30496 RVA: 0x0045B0A8 File Offset: 0x004592A8
		public override void OnStart(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			int groupLeader = selfChar.GetLeaderId();
			bool flag = groupLeader >= 0 && groupLeader != selfCharId;
			if (flag)
			{
				DomainManager.Character.LeaveGroup(context, selfChar, true);
			}
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location currLocation = selfChar.GetLocation();
			lifeRecordCollection.AddDecideToRescue(selfCharId, currDate, this.Target.TargetCharId, currLocation);
			bool flag2 = DomainManager.Character.IsTaiwuPeople(selfCharId) || DomainManager.Character.IsTaiwuPeople(this.Target.TargetCharId);
			if (flag2)
			{
				MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
				Character targetChar = DomainManager.Character.GetElement_Objects(this.Target.TargetCharId);
				monthlyNotifications.AddGoToRescue(selfCharId, this.Target.TargetCharId, targetChar.GetKidnapperId());
			}
		}

		// Token: 0x06007721 RID: 30497 RVA: 0x0045B188 File Offset: 0x00459388
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			int selfCharId = selfChar.GetId();
			Location currLocation = selfChar.GetLocation();
			lifeRecordCollection.AddFinishRescue(selfCharId, currDate, this.Target.TargetCharId, currLocation);
		}

		// Token: 0x06007722 RID: 30498 RVA: 0x0045B1D0 File Offset: 0x004593D0
		public override bool Execute(DataContext context, Character selfChar)
		{
			bool flag = CharacterDomain.IsLockMovementChar(this.Target.TargetCharId);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte behaviorType = selfChar.GetBehaviorType();
				sbyte[] priorityList = AiHelper.PrioritizedActionConstants.RescueFriendOrFamilyActionPriorities[(int)behaviorType];
				sbyte selectedDemandActionType = -1;
				foreach (sbyte actionType in priorityList)
				{
					int chance = (int)(60 + AiHelper.DemandActionType.ToPersonalityType[(int)actionType]);
					bool flag2 = !context.Random.CheckPercentProb(chance);
					if (!flag2)
					{
						selectedDemandActionType = actionType;
						break;
					}
				}
				if (!true)
				{
				}
				bool flag3;
				switch (selectedDemandActionType)
				{
				case 1:
					flag3 = this.HandleSteal(context, selfChar);
					break;
				case 2:
					flag3 = this.HandleScam(context, selfChar);
					break;
				case 3:
					flag3 = this.HandleRob(context, selfChar);
					break;
				default:
					flag3 = false;
					break;
				}
				if (!true)
				{
				}
				result = flag3;
			}
			return result;
		}

		// Token: 0x06007723 RID: 30499 RVA: 0x0045B2A4 File Offset: 0x004594A4
		private bool HandleSteal(DataContext context, Character selfChar)
		{
			Character targetChar = DomainManager.Character.GetElement_Objects(this.Target.TargetCharId);
			Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			int kidnapperId = targetChar.GetKidnapperId();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			int alertFactor = targetChar.GetGradeAlertFactor(targetChar.GetOrganizationInfo().Grade, 1);
			switch (selfChar.GetStealActionPhase(context.Random, targetChar, alertFactor, false))
			{
			case 0:
				lifeRecordCollection.AddRescueKidnappedCharacterSecretlyFail1(selfCharId, currDate, kidnapperId, location, targetCharId);
				break;
			case 1:
				lifeRecordCollection.AddRescueKidnappedCharacterSecretlyFail2(selfCharId, currDate, kidnapperId, location, targetCharId);
				break;
			case 2:
				lifeRecordCollection.AddRescueKidnappedCharacterSecretlyFail3(selfCharId, currDate, kidnapperId, location, targetCharId);
				break;
			case 3:
				lifeRecordCollection.AddRescueKidnappedCharacterSecretlyFail4(selfCharId, currDate, kidnapperId, location, targetCharId);
				break;
			case 4:
			{
				lifeRecordCollection.AddRescueKidnappedCharacterSecretlySucceed(selfCharId, currDate, kidnapperId, location, targetCharId);
				this.RescueSucceed(context, selfCharId, targetCharId, kidnapperId);
				bool flag = kidnapperId == taiwuCharId;
				if (flag)
				{
					MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
					monthlyEventCollection.AddRescueKidnappedCharacterSecretlyButBeCaught(selfCharId, location, kidnapperId, targetCharId);
					return false;
				}
				AiHelper.NpcCombatResultType result = DomainManager.Character.SimulateCharacterCombat(context, selfChar, targetChar, CombatType.Beat, true, 1);
				bool flag2 = result <= AiHelper.NpcCombatResultType.MinorVictory;
				bool flag3 = flag2;
				if (flag3)
				{
					DomainManager.Character.SimulateCharacterCombatResult(context, selfChar, targetChar, 20, 40, 60);
				}
				else
				{
					DomainManager.Character.SimulateCharacterCombatResult(context, targetChar, selfChar, 20, 40, 60);
				}
				break;
			}
			default:
			{
				lifeRecordCollection.AddRescueKidnappedCharacterSecretlySucceedAndEscaped(selfCharId, currDate, kidnapperId, location, targetCharId);
				this.RescueSucceed(context, selfCharId, targetCharId, kidnapperId);
				bool flag4 = kidnapperId == taiwuCharId;
				if (flag4)
				{
					MonthlyEventCollection monthlyEventCollection2 = DomainManager.World.GetMonthlyEventCollection();
					monthlyEventCollection2.AddRescueKidnappedCharacterSecretlyAndEscape(kidnapperId, location, targetCharId);
					return false;
				}
				break;
			}
			}
			return targetChar.GetKidnapperId() < 0;
		}

		// Token: 0x06007724 RID: 30500 RVA: 0x0045B4B4 File Offset: 0x004596B4
		private bool HandleScam(DataContext context, Character selfChar)
		{
			Character targetChar = DomainManager.Character.GetElement_Objects(this.Target.TargetCharId);
			Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			int kidnapperId = targetChar.GetKidnapperId();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			int alertFactor = targetChar.GetGradeAlertFactor(targetChar.GetOrganizationInfo().Grade, 1);
			sbyte actionPhase = selfChar.GetScamActionPhase(context.Random, targetChar, alertFactor, false);
			bool flag = kidnapperId == taiwuCharId && actionPhase >= 3;
			bool result2;
			if (flag)
			{
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection.AddRescueKidnappedCharacterWithWit(selfCharId, location, kidnapperId, targetCharId);
				result2 = false;
			}
			else
			{
				switch (actionPhase)
				{
				case 0:
					lifeRecordCollection.AddRescueKidnappedCharacterWithWitFail1(selfCharId, currDate, kidnapperId, location, targetCharId);
					break;
				case 1:
					lifeRecordCollection.AddRescueKidnappedCharacterWithWitFail2(selfCharId, currDate, kidnapperId, location, targetCharId);
					break;
				case 2:
					lifeRecordCollection.AddRescueKidnappedCharacterWithWitFail3(selfCharId, currDate, kidnapperId, location, targetCharId);
					break;
				case 3:
					lifeRecordCollection.AddRescueKidnappedCharacterWithWitFail4(selfCharId, currDate, kidnapperId, location, targetCharId);
					break;
				case 4:
				{
					lifeRecordCollection.AddRescueKidnappedCharacterWithWitSucceed(selfCharId, currDate, kidnapperId, location, targetCharId);
					this.RescueSucceed(context, selfCharId, targetCharId, kidnapperId);
					AiHelper.NpcCombatResultType result = DomainManager.Character.SimulateCharacterCombat(context, selfChar, targetChar, CombatType.Beat, true, 1);
					bool flag2 = result <= AiHelper.NpcCombatResultType.MinorVictory;
					bool flag3 = flag2;
					if (flag3)
					{
						DomainManager.Character.SimulateCharacterCombatResult(context, selfChar, targetChar, 20, 40, 60);
					}
					else
					{
						DomainManager.Character.SimulateCharacterCombatResult(context, targetChar, selfChar, 20, 40, 60);
					}
					break;
				}
				default:
					lifeRecordCollection.AddRescueKidnappedCharacterWithWitSucceedAndEscaped(selfCharId, currDate, kidnapperId, location, targetCharId);
					this.RescueSucceed(context, selfCharId, targetCharId, kidnapperId);
					break;
				}
				result2 = (targetChar.GetKidnapperId() < 0);
			}
			return result2;
		}

		// Token: 0x06007725 RID: 30501 RVA: 0x0045B6A4 File Offset: 0x004598A4
		private bool HandleRob(DataContext context, Character selfChar)
		{
			Character targetChar = DomainManager.Character.GetElement_Objects(this.Target.TargetCharId);
			Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			int kidnapperId = targetChar.GetKidnapperId();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			int alertFactor = targetChar.GetGradeAlertFactor(targetChar.GetOrganizationInfo().Grade, 1);
			sbyte actionPhase = selfChar.GetRobActionPhase(context.Random, targetChar, alertFactor, false);
			bool flag = kidnapperId == taiwuCharId && actionPhase >= 3;
			bool result2;
			if (flag)
			{
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection.AddRescueKidnappedCharacterWithForce(selfCharId, location, kidnapperId, targetCharId);
				result2 = false;
			}
			else
			{
				switch (actionPhase)
				{
				case 0:
					lifeRecordCollection.AddRescueKidnappedCharacterWithForceFail1(selfCharId, currDate, kidnapperId, location, targetCharId);
					break;
				case 1:
					lifeRecordCollection.AddRescueKidnappedCharacterWithForceFail2(selfCharId, currDate, kidnapperId, location, targetCharId);
					break;
				case 2:
					lifeRecordCollection.AddRescueKidnappedCharacterWithForceFail3(selfCharId, currDate, kidnapperId, location, targetCharId);
					break;
				case 3:
					lifeRecordCollection.AddRescueKidnappedCharacterWithForceFail4(selfCharId, currDate, kidnapperId, location, targetCharId);
					break;
				case 4:
				{
					lifeRecordCollection.AddRescueKidnappedCharacterWithForceSucceed(selfCharId, currDate, kidnapperId, location, targetCharId);
					this.RescueSucceed(context, selfCharId, targetCharId, kidnapperId);
					AiHelper.NpcCombatResultType result = DomainManager.Character.SimulateCharacterCombat(context, selfChar, targetChar, CombatType.Beat, true, 1);
					bool flag2 = result <= AiHelper.NpcCombatResultType.MinorVictory;
					bool flag3 = flag2;
					if (flag3)
					{
						DomainManager.Character.SimulateCharacterCombatResult(context, selfChar, targetChar, 20, 40, 60);
					}
					else
					{
						DomainManager.Character.SimulateCharacterCombatResult(context, targetChar, selfChar, 20, 40, 60);
					}
					break;
				}
				default:
					lifeRecordCollection.AddRescueKidnappedCharacterWithForceSucceedAndEscaped(selfCharId, currDate, kidnapperId, location, targetCharId);
					this.RescueSucceed(context, selfCharId, targetCharId, kidnapperId);
					break;
				}
				result2 = (targetChar.GetKidnapperId() < 0);
			}
			return result2;
		}

		// Token: 0x06007726 RID: 30502 RVA: 0x0045B894 File Offset: 0x00459A94
		private void RescueSucceed(DataContext context, int selfCharId, int targetCharId, int kidnapperId)
		{
			DomainManager.Character.RemoveKidnappedCharacter(context, targetCharId, kidnapperId, true);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int secretInfoOffset = secretInformationCollection.AddRescueKidnappedCharacter(selfCharId, targetCharId, kidnapperId);
			int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(selfCharId);
			bool targetIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(targetCharId);
			bool flag = selfIsTaiwuPeople || targetIsTaiwuPeople;
			if (flag)
			{
				MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
				monthlyNotifications.AddRescuePrisoner(selfCharId, targetCharId, kidnapperId);
				DomainManager.Information.ReceiveSecretInformation(context, secretInfoId, DomainManager.Taiwu.GetTaiwuCharId(), selfIsTaiwuPeople ? selfCharId : targetCharId);
			}
		}
	}
}
