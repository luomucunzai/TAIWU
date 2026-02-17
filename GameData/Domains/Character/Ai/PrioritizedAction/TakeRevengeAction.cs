using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x0200086A RID: 2154
	[SerializableGameData(NotForDisplayModule = true)]
	public class TakeRevengeAction : BasePrioritizedAction
	{
		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x0600774C RID: 30540 RVA: 0x0045C50C File Offset: 0x0045A70C
		public override short ActionType
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x0600774D RID: 30541 RVA: 0x0045C510 File Offset: 0x0045A710
		public override bool CheckValid(Character selfChar)
		{
			bool flag = !base.CheckValid(selfChar);
			return !flag && DomainManager.Character.IsCharacterAlive(this.Target.TargetCharId);
		}

		// Token: 0x0600774E RID: 30542 RVA: 0x0045C54C File Offset: 0x0045A74C
		public override void OnStart(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location currLocation = selfChar.GetLocation();
			lifeRecordCollection.AddDecideToRevenge(selfCharId, currDate, this.Target.TargetCharId, currLocation);
			bool flag = DomainManager.Character.IsTaiwuPeople(selfCharId) || DomainManager.Character.IsTaiwuPeople(this.Target.TargetCharId);
			if (flag)
			{
				MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
				monthlyNotifications.AddGoToRevenge(selfCharId, this.Target.TargetCharId);
			}
			DomainManager.Character.AddOngoingVengeance(context, selfCharId, this.Target.TargetCharId);
		}

		// Token: 0x0600774F RID: 30543 RVA: 0x0045C5F8 File Offset: 0x0045A7F8
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			int selfCharId = selfChar.GetId();
			Location currLocation = selfChar.GetLocation();
			lifeRecordCollection.AddFinishTakingRevenge(selfCharId, currDate, this.Target.TargetCharId, currLocation);
			DomainManager.Character.FinishOngoingVengeance(context, selfCharId, this.Target.TargetCharId);
		}

		// Token: 0x06007750 RID: 30544 RVA: 0x0045C657 File Offset: 0x0045A857
		public override void OnCharacterDead(DataContext context, Character selfChar)
		{
			DomainManager.Character.FinishOngoingVengeance(context, selfChar.GetId(), this.Target.TargetCharId);
		}

		// Token: 0x06007751 RID: 30545 RVA: 0x0045C678 File Offset: 0x0045A878
		public override bool Execute(DataContext context, Character selfChar)
		{
			Character targetChar = DomainManager.Character.GetElement_Objects(this.Target.TargetCharId);
			bool flag = targetChar.GetKidnapperId() >= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte behaviorType = selfChar.GetBehaviorType();
				sbyte[] priorityList = AiHelper.PrioritizedActionConstants.TakeRevengeActionPriorities[(int)behaviorType];
				sbyte selectedHarmType = -1;
				MainAttributes selfMainAttributes = selfChar.GetCurrMainAttributes();
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				bool canTaiwuBeTarget = DomainManager.Taiwu.CanTaiwuBeSneakyHarmfulActionTarget();
				bool needToAvoidCombat = selfChar.NeedToAvoidCombat(CombatType.Die);
				sbyte[] array = priorityList;
				int i = 0;
				while (i < array.Length)
				{
					sbyte actionType = array[i];
					bool flag2 = actionType == 0;
					if (flag2)
					{
						bool flag3 = needToAvoidCombat;
						if (!flag3)
						{
							goto IL_B5;
						}
					}
					else
					{
						bool flag4 = this.Target.TargetCharId == taiwuCharId && canTaiwuBeTarget;
						if (!flag4)
						{
							goto IL_B5;
						}
					}
					IL_124:
					i++;
					continue;
					IL_B5:
					sbyte mainAttributeType = AiHelper.HarmActionType.ToMainAttributeType(actionType);
					bool flag5 = mainAttributeType >= 0 && selfChar.GetCurrMainAttribute(mainAttributeType) < (short)GlobalConfig.Instance.HarmfulActionCost;
					if (flag5)
					{
						goto IL_124;
					}
					short rateAdjust = DomainManager.Extra.GetAiActionRateAdjust(selfChar.GetId(), 5, actionType);
					int chance = (int)((short)(60 + AiHelper.HarmActionType.ToPersonalityType[(int)actionType]) + rateAdjust);
					bool flag6 = !context.Random.CheckPercentProb(chance);
					if (flag6)
					{
						goto IL_124;
					}
					selectedHarmType = actionType;
					break;
				}
				switch (selectedHarmType)
				{
				case 0:
					DomainManager.Character.HandleAttackAction(context, selfChar, targetChar);
					break;
				case 1:
					DomainManager.Character.HandlePoisonAction(context, selfChar, targetChar, ItemKey.Invalid, -1);
					break;
				case 2:
					DomainManager.Character.HandlePlotHarmAction(context, selfChar, targetChar, ItemKey.Invalid, -1);
					break;
				default:
					return false;
				}
				result = (!DomainManager.Character.IsCharacterAlive(this.Target.TargetCharId) || targetChar.GetKidnapperId() == selfChar.GetId());
			}
			return result;
		}

		// Token: 0x06007752 RID: 30546 RVA: 0x0045C845 File Offset: 0x0045AA45
		[Obsolete]
		public static void HandleAttack(DataContext context, Character selfChar, Character targetChar)
		{
			DomainManager.Character.HandleAttackAction(context, selfChar, targetChar);
		}

		// Token: 0x06007753 RID: 30547 RVA: 0x0045C858 File Offset: 0x0045AA58
		[Obsolete]
		public static void HandlePoison(DataContext context, Character selfChar, Character targetChar, ItemKey poison)
		{
			DomainManager.Character.HandlePoisonAction(context, selfChar, targetChar, poison, selfChar.GetPoisonActionPhase(context.Random, targetChar, 100, false));
		}

		// Token: 0x06007754 RID: 30548 RVA: 0x0045C884 File Offset: 0x0045AA84
		[Obsolete]
		public static void HandlePlotHarm(DataContext context, Character selfChar, Character targetChar)
		{
			DomainManager.Character.HandlePlotHarmAction(context, selfChar, targetChar, ItemKey.Invalid, selfChar.GetPlotHarmActionPhase(context.Random, targetChar, 100, false));
		}

		// Token: 0x06007755 RID: 30549 RVA: 0x0045C8B4 File Offset: 0x0045AAB4
		[Obsolete]
		public static ItemKey SelectPoisonToUse(Character selfChar, Character targetChar)
		{
			return DomainManager.Character.SelectMostEffectivePoisonToUse(selfChar, targetChar);
		}
	}
}
