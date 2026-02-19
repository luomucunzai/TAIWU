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

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class RescueFriendOrFamilyAction : BasePrioritizedAction
{
	public override short ActionType => 3;

	public override bool CheckValid(Character selfChar)
	{
		if (!base.CheckValid(selfChar))
		{
			return false;
		}
		if (!DomainManager.Character.TryGetElement_Objects(Target.TargetCharId, out var element))
		{
			return false;
		}
		Location location = element.GetLocation();
		Location location2 = selfChar.GetLocation();
		int kidnapperId = element.GetKidnapperId();
		if (kidnapperId == selfChar.GetId())
		{
			return false;
		}
		if (kidnapperId < 0 && location2.IsValid() && location2.AreaId == location.AreaId)
		{
			byte areaSize = DomainManager.Map.GetAreaSize(location2.AreaId);
			ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(location2.BlockId, areaSize);
			ByteCoordinate byteCoordinate2 = ByteCoordinate.IndexToCoordinate(location.BlockId, areaSize);
			if (byteCoordinate.GetManhattanDistance(byteCoordinate2) <= 3)
			{
				return false;
			}
		}
		short favorability = DomainManager.Character.GetFavorability(selfChar.GetId(), Target.TargetCharId);
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
		return favorabilityType >= AiHelper.PrioritizedActionConstants.PrioritizedActionMinFavorType[ActionType];
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		int leaderId = selfChar.GetLeaderId();
		if (leaderId >= 0 && leaderId != id)
		{
			DomainManager.Character.LeaveGroup(context, selfChar);
		}
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		lifeRecordCollection.AddDecideToRescue(id, currDate, Target.TargetCharId, location);
		if (DomainManager.Character.IsTaiwuPeople(id) || DomainManager.Character.IsTaiwuPeople(Target.TargetCharId))
		{
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			Character element_Objects = DomainManager.Character.GetElement_Objects(Target.TargetCharId);
			monthlyNotificationCollection.AddGoToRescue(id, Target.TargetCharId, element_Objects.GetKidnapperId());
		}
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		int id = selfChar.GetId();
		Location location = selfChar.GetLocation();
		lifeRecordCollection.AddFinishRescue(id, currDate, Target.TargetCharId, location);
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		if (CharacterDomain.IsLockMovementChar(Target.TargetCharId))
		{
			return false;
		}
		sbyte behaviorType = selfChar.GetBehaviorType();
		sbyte[] array = AiHelper.PrioritizedActionConstants.RescueFriendOrFamilyActionPriorities[behaviorType];
		sbyte b = -1;
		sbyte[] array2 = array;
		foreach (sbyte b2 in array2)
		{
			int percentProb = 60 + AiHelper.DemandActionType.ToPersonalityType[b2];
			if (context.Random.CheckPercentProb(percentProb))
			{
				b = b2;
				break;
			}
		}
		if (1 == 0)
		{
		}
		bool result = b switch
		{
			1 => HandleSteal(context, selfChar), 
			2 => HandleScam(context, selfChar), 
			3 => HandleRob(context, selfChar), 
			_ => false, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private bool HandleSteal(DataContext context, Character selfChar)
	{
		Character element_Objects = DomainManager.Character.GetElement_Objects(Target.TargetCharId);
		Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int id = selfChar.GetId();
		int id2 = element_Objects.GetId();
		int kidnapperId = element_Objects.GetKidnapperId();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		int gradeAlertFactor = element_Objects.GetGradeAlertFactor(element_Objects.GetOrganizationInfo().Grade, 1);
		switch (selfChar.GetStealActionPhase(context.Random, element_Objects, gradeAlertFactor))
		{
		case 0:
			lifeRecordCollection.AddRescueKidnappedCharacterSecretlyFail1(id, currDate, kidnapperId, location, id2);
			break;
		case 1:
			lifeRecordCollection.AddRescueKidnappedCharacterSecretlyFail2(id, currDate, kidnapperId, location, id2);
			break;
		case 2:
			lifeRecordCollection.AddRescueKidnappedCharacterSecretlyFail3(id, currDate, kidnapperId, location, id2);
			break;
		case 3:
			lifeRecordCollection.AddRescueKidnappedCharacterSecretlyFail4(id, currDate, kidnapperId, location, id2);
			break;
		case 4:
		{
			lifeRecordCollection.AddRescueKidnappedCharacterSecretlySucceed(id, currDate, kidnapperId, location, id2);
			RescueSucceed(context, id, id2, kidnapperId);
			if (kidnapperId == taiwuCharId)
			{
				MonthlyEventCollection monthlyEventCollection2 = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection2.AddRescueKidnappedCharacterSecretlyButBeCaught(id, location, kidnapperId, id2);
				return false;
			}
			AiHelper.NpcCombatResultType npcCombatResultType = DomainManager.Character.SimulateCharacterCombat(context, selfChar, element_Objects, CombatType.Beat);
			if ((uint)npcCombatResultType <= 1u)
			{
				DomainManager.Character.SimulateCharacterCombatResult(context, selfChar, element_Objects, 20, 40, 60);
			}
			else
			{
				DomainManager.Character.SimulateCharacterCombatResult(context, element_Objects, selfChar, 20, 40, 60);
			}
			break;
		}
		default:
			lifeRecordCollection.AddRescueKidnappedCharacterSecretlySucceedAndEscaped(id, currDate, kidnapperId, location, id2);
			RescueSucceed(context, id, id2, kidnapperId);
			if (kidnapperId == taiwuCharId)
			{
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection.AddRescueKidnappedCharacterSecretlyAndEscape(kidnapperId, location, id2);
				return false;
			}
			break;
		}
		return element_Objects.GetKidnapperId() < 0;
	}

	private bool HandleScam(DataContext context, Character selfChar)
	{
		Character element_Objects = DomainManager.Character.GetElement_Objects(Target.TargetCharId);
		Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int id = selfChar.GetId();
		int id2 = element_Objects.GetId();
		int kidnapperId = element_Objects.GetKidnapperId();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		int gradeAlertFactor = element_Objects.GetGradeAlertFactor(element_Objects.GetOrganizationInfo().Grade, 1);
		sbyte scamActionPhase = selfChar.GetScamActionPhase(context.Random, element_Objects, gradeAlertFactor);
		if (kidnapperId == taiwuCharId && scamActionPhase >= 3)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddRescueKidnappedCharacterWithWit(id, location, kidnapperId, id2);
			return false;
		}
		switch (scamActionPhase)
		{
		case 0:
			lifeRecordCollection.AddRescueKidnappedCharacterWithWitFail1(id, currDate, kidnapperId, location, id2);
			break;
		case 1:
			lifeRecordCollection.AddRescueKidnappedCharacterWithWitFail2(id, currDate, kidnapperId, location, id2);
			break;
		case 2:
			lifeRecordCollection.AddRescueKidnappedCharacterWithWitFail3(id, currDate, kidnapperId, location, id2);
			break;
		case 3:
			lifeRecordCollection.AddRescueKidnappedCharacterWithWitFail4(id, currDate, kidnapperId, location, id2);
			break;
		case 4:
		{
			lifeRecordCollection.AddRescueKidnappedCharacterWithWitSucceed(id, currDate, kidnapperId, location, id2);
			RescueSucceed(context, id, id2, kidnapperId);
			AiHelper.NpcCombatResultType npcCombatResultType = DomainManager.Character.SimulateCharacterCombat(context, selfChar, element_Objects, CombatType.Beat);
			if ((uint)npcCombatResultType <= 1u)
			{
				DomainManager.Character.SimulateCharacterCombatResult(context, selfChar, element_Objects, 20, 40, 60);
			}
			else
			{
				DomainManager.Character.SimulateCharacterCombatResult(context, element_Objects, selfChar, 20, 40, 60);
			}
			break;
		}
		default:
			lifeRecordCollection.AddRescueKidnappedCharacterWithWitSucceedAndEscaped(id, currDate, kidnapperId, location, id2);
			RescueSucceed(context, id, id2, kidnapperId);
			break;
		}
		return element_Objects.GetKidnapperId() < 0;
	}

	private bool HandleRob(DataContext context, Character selfChar)
	{
		Character element_Objects = DomainManager.Character.GetElement_Objects(Target.TargetCharId);
		Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int id = selfChar.GetId();
		int id2 = element_Objects.GetId();
		int kidnapperId = element_Objects.GetKidnapperId();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		int gradeAlertFactor = element_Objects.GetGradeAlertFactor(element_Objects.GetOrganizationInfo().Grade, 1);
		sbyte robActionPhase = selfChar.GetRobActionPhase(context.Random, element_Objects, gradeAlertFactor);
		if (kidnapperId == taiwuCharId && robActionPhase >= 3)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddRescueKidnappedCharacterWithForce(id, location, kidnapperId, id2);
			return false;
		}
		switch (robActionPhase)
		{
		case 0:
			lifeRecordCollection.AddRescueKidnappedCharacterWithForceFail1(id, currDate, kidnapperId, location, id2);
			break;
		case 1:
			lifeRecordCollection.AddRescueKidnappedCharacterWithForceFail2(id, currDate, kidnapperId, location, id2);
			break;
		case 2:
			lifeRecordCollection.AddRescueKidnappedCharacterWithForceFail3(id, currDate, kidnapperId, location, id2);
			break;
		case 3:
			lifeRecordCollection.AddRescueKidnappedCharacterWithForceFail4(id, currDate, kidnapperId, location, id2);
			break;
		case 4:
		{
			lifeRecordCollection.AddRescueKidnappedCharacterWithForceSucceed(id, currDate, kidnapperId, location, id2);
			RescueSucceed(context, id, id2, kidnapperId);
			AiHelper.NpcCombatResultType npcCombatResultType = DomainManager.Character.SimulateCharacterCombat(context, selfChar, element_Objects, CombatType.Beat);
			if ((uint)npcCombatResultType <= 1u)
			{
				DomainManager.Character.SimulateCharacterCombatResult(context, selfChar, element_Objects, 20, 40, 60);
			}
			else
			{
				DomainManager.Character.SimulateCharacterCombatResult(context, element_Objects, selfChar, 20, 40, 60);
			}
			break;
		}
		default:
			lifeRecordCollection.AddRescueKidnappedCharacterWithForceSucceedAndEscaped(id, currDate, kidnapperId, location, id2);
			RescueSucceed(context, id, id2, kidnapperId);
			break;
		}
		return element_Objects.GetKidnapperId() < 0;
	}

	private void RescueSucceed(DataContext context, int selfCharId, int targetCharId, int kidnapperId)
	{
		DomainManager.Character.RemoveKidnappedCharacter(context, targetCharId, kidnapperId, isEscaped: true);
		SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
		int dataOffset = secretInformationCollection.AddRescueKidnappedCharacter(selfCharId, targetCharId, kidnapperId);
		int metaDataId = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		bool flag = DomainManager.Character.IsTaiwuPeople(selfCharId);
		bool flag2 = DomainManager.Character.IsTaiwuPeople(targetCharId);
		if (flag || flag2)
		{
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection.AddRescuePrisoner(selfCharId, targetCharId, kidnapperId);
			DomainManager.Information.ReceiveSecretInformation(context, metaDataId, DomainManager.Taiwu.GetTaiwuCharId(), flag ? selfCharId : targetCharId);
		}
	}
}
