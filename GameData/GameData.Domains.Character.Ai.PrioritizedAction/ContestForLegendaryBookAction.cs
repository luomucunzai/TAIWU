using System.Collections.Generic;
using GameData.Common;
using GameData.Domains.Character.Ai.GeneralAction.BehaviorAction;
using GameData.Domains.Character.Ai.GeneralAction.WealthDemand;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class ContestForLegendaryBookAction : BasePrioritizedAction
{
	[SerializableGameDataField]
	public sbyte LegendaryBookType;

	public override short ActionType => 9;

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
		if (element.GetKidnapperId() >= 0)
		{
			return false;
		}
		List<sbyte> charOwnedBookTypes = DomainManager.LegendaryBook.GetCharOwnedBookTypes(Target.TargetCharId);
		if (charOwnedBookTypes == null || !charOwnedBookTypes.Contains(LegendaryBookType))
		{
			return false;
		}
		return true;
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		lifeRecordCollection.AddDecideToContestForLegendaryBook(id, currDate, Target.TargetCharId, location, 12, (short)(211 + LegendaryBookType));
		selfChar.TryRetireTreasuryGuard(context);
		DomainManager.Extra.AddContestForLegendaryBookCharacter(context, id, LegendaryBookType);
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		lifeRecordCollection.AddFinishContestForLegendaryBook(id, currDate, location);
		DomainManager.Extra.RemoveContestForLegendaryBookCharacter(context, id, LegendaryBookType);
	}

	public override void OnCharacterDead(DataContext context, Character selfChar)
	{
		DomainManager.Extra.RemoveContestForLegendaryBookCharacter(context, selfChar.GetId(), LegendaryBookType);
	}

	public unsafe override bool Execute(DataContext context, Character selfChar)
	{
		Character element_Objects = DomainManager.Character.GetElement_Objects(Target.TargetCharId);
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		int id = selfChar.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		sbyte behaviorType = selfChar.GetBehaviorType();
		Personalities personalities = selfChar.GetPersonalities();
		ItemKey legendaryBookItem = DomainManager.LegendaryBook.GetLegendaryBookItem(LegendaryBookType);
		DomainManager.Extra.AddContestForLegendaryBookCharacter(context, id, LegendaryBookType);
		selfChar.TryRetireTreasuryGuard(context);
		if (context.Random.CheckPercentProb(AiHelper.LegendaryBookRelatedConstants.IdleDuringContestActionChance[behaviorType]))
		{
			return false;
		}
		int percentProb = ((element_Objects.GetCombatPower() > selfChar.GetCombatPower()) ? 25 : 75);
		if (context.Random.CheckPercentProb(percentProb))
		{
			sbyte[] array = AiHelper.LegendaryBookContestActionType.IndirectActionPriorities[behaviorType];
			sbyte[] array2 = array;
			foreach (sbyte b in array2)
			{
				sbyte b2 = AiHelper.LegendaryBookContestActionType.ToPersonalityType[b];
				int percentProb2 = 60 + personalities.Items[b2];
				if (!context.Random.CheckPercentProb(percentProb2))
				{
					continue;
				}
				switch (b)
				{
				case 0:
				{
					GainExpByCombatAction gainExpByCombatAction = new GainExpByCombatAction
					{
						CombatType = CombatType.Play
					};
					if (gainExpByCombatAction.CheckValid(selfChar, element_Objects))
					{
						if (Target.TargetCharId == taiwuCharId)
						{
							gainExpByCombatAction.ApplyInitialChangesForTaiwu(context, selfChar, element_Objects);
						}
						else
						{
							gainExpByCombatAction.ApplyChanges(context, selfChar, element_Objects);
						}
					}
					return false;
				}
				case 1:
				{
					ItemBase itemBase = selfChar.SelectSpareableItem(context, element_Objects.GetInteractionGrade(), allowUsed: false);
					if (itemBase == null)
					{
						return false;
					}
					ItemKey itemKey = itemBase.GetItemKey();
					GiveItemAction giveItemAction = new GiveItemAction
					{
						TargetItem = itemKey,
						Amount = 1,
						RefusePoisonousItem = element_Objects.TryDetectAttachedPoisons(itemKey)
					};
					if (giveItemAction.CheckValid(selfChar, element_Objects))
					{
						if (Target.TargetCharId == taiwuCharId)
						{
							giveItemAction.ApplyInitialChangesForTaiwu(context, selfChar, element_Objects);
						}
						else
						{
							giveItemAction.ApplyChanges(context, selfChar, element_Objects);
						}
					}
					return false;
				}
				case 2:
					DomainManager.Character.HandlePoisonAction(context, selfChar, element_Objects, ItemKey.Invalid, -1);
					return false;
				case 3:
					DomainManager.Character.HandlePlotHarmAction(context, selfChar, element_Objects, ItemKey.Invalid, -1);
					return false;
				}
			}
		}
		else
		{
			int itemAlertFactor = element_Objects.GetItemAlertFactor(legendaryBookItem, 1);
			if (Target.TargetCharId == taiwuCharId)
			{
				sbyte[] array3 = AiHelper.LegendaryBookContestActionType.DirectActionTaiwuTargetPriorities[behaviorType];
				sbyte[] array4 = array3;
				foreach (sbyte b3 in array4)
				{
					sbyte b4 = AiHelper.LegendaryBookContestActionType.ToPersonalityType[b3];
					int percentProb3 = 60 + personalities.Items[b4];
					if (!context.Random.CheckPercentProb(percentProb3))
					{
						continue;
					}
					CharacterDomain.AddLockMovementCharSet(selfChar.GetId());
					switch (b3)
					{
					case 4:
						if (!DomainManager.Extra.IsAiActionInCooldown(id, 3, 3))
						{
							monthlyEventCollection.AddChallengeForLegendaryBook(id, location, Target.TargetCharId, (ulong)legendaryBookItem);
							DomainManager.Extra.AddAiActionCooldown(context, id, 3, 3, 12);
						}
						break;
					case 5:
						monthlyEventCollection.AddRequestLegendaryBook(id, location, Target.TargetCharId, (ulong)legendaryBookItem);
						return false;
					case 6:
						AddTradeForBookMonthlyEvent(selfChar, legendaryBookItem);
						return false;
					case 7:
					{
						if (!DomainManager.Taiwu.CanTaiwuBeSneakyHarmfulActionTarget())
						{
							break;
						}
						StealItemAction stealItemAction2 = new StealItemAction
						{
							TargetItem = legendaryBookItem,
							Amount = 1,
							Phase = selfChar.GetStealActionPhase(context.Random, element_Objects, itemAlertFactor)
						};
						stealItemAction2.ApplyInitialChangesForTaiwu(context, selfChar, element_Objects);
						return false;
					}
					case 8:
					{
						if (!DomainManager.Taiwu.CanTaiwuBeSneakyHarmfulActionTarget())
						{
							break;
						}
						StealItemAction stealItemAction = new StealItemAction
						{
							TargetItem = legendaryBookItem,
							Amount = 1,
							Phase = selfChar.GetScamActionPhase(context.Random, element_Objects, itemAlertFactor)
						};
						stealItemAction.ApplyInitialChangesForTaiwu(context, selfChar, element_Objects);
						return false;
					}
					case 9:
					{
						if (DomainManager.Extra.IsAiActionInCooldown(id, 3, 3))
						{
							break;
						}
						RobItemAction robItemAction = new RobItemAction
						{
							TargetItem = legendaryBookItem,
							Amount = 1,
							Phase = selfChar.GetRobActionPhase(context.Random, element_Objects, itemAlertFactor)
						};
						robItemAction.ApplyInitialChangesForTaiwu(context, selfChar, element_Objects);
						DomainManager.Extra.AddAiActionCooldown(context, id, 3, 3, 12);
						return false;
					}
					}
				}
				return false;
			}
			sbyte[] array5 = AiHelper.LegendaryBookContestActionType.DirectActionNpcTargetPriorities[behaviorType];
			sbyte[] array6 = array5;
			foreach (sbyte b5 in array6)
			{
				sbyte b6 = AiHelper.LegendaryBookContestActionType.ToPersonalityType[b5];
				int percentProb4 = 60 + personalities.Items[b6];
				if (!context.Random.CheckPercentProb(percentProb4))
				{
					continue;
				}
				switch (b5)
				{
				case 4:
				{
					AiHelper.NpcCombatResultType npcCombatResultType = DomainManager.Character.SimulateCharacterCombat(context, selfChar, element_Objects, CombatType.Beat, isGroupCombat: false);
					if (npcCombatResultType == AiHelper.NpcCombatResultType.MajorVictory || npcCombatResultType == AiHelper.NpcCombatResultType.MinorVictory)
					{
						DomainManager.Character.TransferInventoryItem(context, element_Objects, selfChar, legendaryBookItem, 1);
						monthlyNotificationCollection.AddChallengeForLegendaryBook(id, location, Target.TargetCharId, legendaryBookItem.ItemType, legendaryBookItem.TemplateId);
						lifeRecordCollection.AddLegendaryBookChallengeWin(id, currDate, Target.TargetCharId, location, legendaryBookItem.ItemType, legendaryBookItem.TemplateId);
					}
					else
					{
						lifeRecordCollection.AddLegendaryBookChallengeLose(id, currDate, Target.TargetCharId, location, legendaryBookItem.ItemType, legendaryBookItem.TemplateId);
					}
					return false;
				}
				case 9:
				{
					RobItemAction robItemAction2 = new RobItemAction
					{
						TargetItem = legendaryBookItem,
						Amount = 1,
						Phase = selfChar.GetRobActionPhase(context.Random, element_Objects, itemAlertFactor)
					};
					if (robItemAction2.CheckValid(selfChar, element_Objects))
					{
						robItemAction2.ApplyChanges(context, selfChar, element_Objects);
					}
					if (selfChar.GetInventory().Items.ContainsKey(legendaryBookItem))
					{
						monthlyNotificationCollection.AddRobLegendaryBook(id, location, Target.TargetCharId, legendaryBookItem.ItemType, legendaryBookItem.TemplateId);
					}
					return false;
				}
				}
			}
		}
		return false;
	}

	private unsafe void AddTradeForBookMonthlyEvent(Character character, ItemKey bookItemKey)
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int id = character.GetId();
		Location location = character.GetLocation();
		ResourceInts resources = character.GetResources();
		int num = resources.Items[6] * GlobalConfig.ResourcesWorth[6];
		int num2 = resources.Items[7] * GlobalConfig.ResourcesWorth[7];
		int num3 = character.GetExp() * 5;
		if (num >= num2 && num >= num3)
		{
			monthlyEventCollection.AddExchangeLegendaryBookByMoney(id, location, Target.TargetCharId, (ulong)bookItemKey);
		}
		else if (num2 >= num && num2 >= num3)
		{
			monthlyEventCollection.AddExchangeLegendaryBookByAuthority(id, location, Target.TargetCharId, (ulong)bookItemKey);
		}
		else
		{
			monthlyEventCollection.AddExchangeLegendaryBookByExperience(id, location, Target.TargetCharId, (ulong)bookItemKey);
		}
	}

	public ContestForLegendaryBookAction()
	{
		LegendaryBookType = -1;
	}

	public ContestForLegendaryBookAction(ContestForLegendaryBookAction other)
	{
		LegendaryBookType = other.LegendaryBookType;
		Target = other.Target;
		HasArrived = other.HasArrived;
	}

	public void Assign(ContestForLegendaryBookAction other)
	{
		LegendaryBookType = other.LegendaryBookType;
		Target = other.Target;
		HasArrived = other.HasArrived;
	}

	public override bool IsSerializedSizeFixed()
	{
		return true;
	}

	public override int GetSerializedSize()
	{
		int num = 18;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)LegendaryBookType;
		ptr++;
		ptr += Target.Serialize(ptr);
		*ptr = (HasArrived ? ((byte)1) : ((byte)0));
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		LegendaryBookType = (sbyte)(*ptr);
		ptr++;
		ptr += Target.Deserialize(ptr);
		HasArrived = *ptr != 0;
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
