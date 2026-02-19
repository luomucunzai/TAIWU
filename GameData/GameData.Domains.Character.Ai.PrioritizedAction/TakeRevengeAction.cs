using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class TakeRevengeAction : BasePrioritizedAction
{
	public override short ActionType => 8;

	public override bool CheckValid(Character selfChar)
	{
		if (!base.CheckValid(selfChar))
		{
			return false;
		}
		return DomainManager.Character.IsCharacterAlive(Target.TargetCharId);
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		lifeRecordCollection.AddDecideToRevenge(id, currDate, Target.TargetCharId, location);
		if (DomainManager.Character.IsTaiwuPeople(id) || DomainManager.Character.IsTaiwuPeople(Target.TargetCharId))
		{
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection.AddGoToRevenge(id, Target.TargetCharId);
		}
		DomainManager.Character.AddOngoingVengeance(context, id, Target.TargetCharId);
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		int id = selfChar.GetId();
		Location location = selfChar.GetLocation();
		lifeRecordCollection.AddFinishTakingRevenge(id, currDate, Target.TargetCharId, location);
		DomainManager.Character.FinishOngoingVengeance(context, id, Target.TargetCharId);
	}

	public override void OnCharacterDead(DataContext context, Character selfChar)
	{
		DomainManager.Character.FinishOngoingVengeance(context, selfChar.GetId(), Target.TargetCharId);
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		Character element_Objects = DomainManager.Character.GetElement_Objects(Target.TargetCharId);
		if (element_Objects.GetKidnapperId() >= 0)
		{
			return false;
		}
		sbyte behaviorType = selfChar.GetBehaviorType();
		sbyte[] array = AiHelper.PrioritizedActionConstants.TakeRevengeActionPriorities[behaviorType];
		sbyte b = -1;
		MainAttributes currMainAttributes = selfChar.GetCurrMainAttributes();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		bool flag = DomainManager.Taiwu.CanTaiwuBeSneakyHarmfulActionTarget();
		bool flag2 = selfChar.NeedToAvoidCombat(CombatType.Die);
		sbyte[] array2 = array;
		foreach (sbyte b2 in array2)
		{
			if (b2 == 0)
			{
				if (flag2)
				{
					continue;
				}
			}
			else if (Target.TargetCharId == taiwuCharId && flag)
			{
				continue;
			}
			sbyte b3 = AiHelper.HarmActionType.ToMainAttributeType(b2);
			if (b3 < 0 || selfChar.GetCurrMainAttribute(b3) >= GlobalConfig.Instance.HarmfulActionCost)
			{
				short aiActionRateAdjust = DomainManager.Extra.GetAiActionRateAdjust(selfChar.GetId(), 5, b2);
				int percentProb = 60 + AiHelper.HarmActionType.ToPersonalityType[b2] + aiActionRateAdjust;
				if (context.Random.CheckPercentProb(percentProb))
				{
					b = b2;
					break;
				}
			}
		}
		switch (b)
		{
		case 0:
			DomainManager.Character.HandleAttackAction(context, selfChar, element_Objects);
			break;
		case 1:
			DomainManager.Character.HandlePoisonAction(context, selfChar, element_Objects, ItemKey.Invalid, -1);
			break;
		case 2:
			DomainManager.Character.HandlePlotHarmAction(context, selfChar, element_Objects, ItemKey.Invalid, -1);
			break;
		default:
			return false;
		}
		return !DomainManager.Character.IsCharacterAlive(Target.TargetCharId) || element_Objects.GetKidnapperId() == selfChar.GetId();
	}

	[Obsolete]
	public static void HandleAttack(DataContext context, Character selfChar, Character targetChar)
	{
		DomainManager.Character.HandleAttackAction(context, selfChar, targetChar);
	}

	[Obsolete]
	public static void HandlePoison(DataContext context, Character selfChar, Character targetChar, ItemKey poison)
	{
		DomainManager.Character.HandlePoisonAction(context, selfChar, targetChar, poison, selfChar.GetPoisonActionPhase(context.Random, targetChar));
	}

	[Obsolete]
	public static void HandlePlotHarm(DataContext context, Character selfChar, Character targetChar)
	{
		DomainManager.Character.HandlePlotHarmAction(context, selfChar, targetChar, ItemKey.Invalid, selfChar.GetPlotHarmActionPhase(context.Random, targetChar));
	}

	[Obsolete]
	public static ItemKey SelectPoisonToUse(Character selfChar, Character targetChar)
	{
		return DomainManager.Character.SelectMostEffectivePoisonToUse(selfChar, targetChar);
	}
}
