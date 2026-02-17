using System;
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

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x02000855 RID: 2133
	[SerializableGameData(NotForDisplayModule = true)]
	public class ContestForLegendaryBookAction : BasePrioritizedAction
	{
		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x060076BE RID: 30398 RVA: 0x0045829B File Offset: 0x0045649B
		public override short ActionType
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x060076BF RID: 30399 RVA: 0x004582A0 File Offset: 0x004564A0
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
					bool flag3 = targetChar.GetKidnapperId() >= 0;
					if (flag3)
					{
						result = false;
					}
					else
					{
						List<sbyte> ownedBookTypes = DomainManager.LegendaryBook.GetCharOwnedBookTypes(this.Target.TargetCharId);
						bool flag4 = ownedBookTypes == null || !ownedBookTypes.Contains(this.LegendaryBookType);
						result = !flag4;
					}
				}
			}
			return result;
		}

		// Token: 0x060076C0 RID: 30400 RVA: 0x00458334 File Offset: 0x00456534
		public override void OnStart(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location currLocation = selfChar.GetLocation();
			lifeRecordCollection.AddDecideToContestForLegendaryBook(selfCharId, currDate, this.Target.TargetCharId, currLocation, 12, (short)(211 + (int)this.LegendaryBookType));
			selfChar.TryRetireTreasuryGuard(context);
			DomainManager.Extra.AddContestForLegendaryBookCharacter(context, selfCharId, this.LegendaryBookType);
		}

		// Token: 0x060076C1 RID: 30401 RVA: 0x004583A8 File Offset: 0x004565A8
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location currLocation = selfChar.GetLocation();
			lifeRecordCollection.AddFinishContestForLegendaryBook(selfCharId, currDate, currLocation);
			DomainManager.Extra.RemoveContestForLegendaryBookCharacter(context, selfCharId, this.LegendaryBookType);
		}

		// Token: 0x060076C2 RID: 30402 RVA: 0x004583F7 File Offset: 0x004565F7
		public override void OnCharacterDead(DataContext context, Character selfChar)
		{
			DomainManager.Extra.RemoveContestForLegendaryBookCharacter(context, selfChar.GetId(), this.LegendaryBookType);
		}

		// Token: 0x060076C3 RID: 30403 RVA: 0x00458414 File Offset: 0x00456614
		public unsafe override bool Execute(DataContext context, Character selfChar)
		{
			Character targetChar = DomainManager.Character.GetElement_Objects(this.Target.TargetCharId);
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
			int selfCharId = selfChar.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			sbyte behaviorType = selfChar.GetBehaviorType();
			Personalities personalities = selfChar.GetPersonalities();
			ItemKey itemKey = DomainManager.LegendaryBook.GetLegendaryBookItem(this.LegendaryBookType);
			DomainManager.Extra.AddContestForLegendaryBookCharacter(context, selfCharId, this.LegendaryBookType);
			selfChar.TryRetireTreasuryGuard(context);
			bool flag = context.Random.CheckPercentProb((int)AiHelper.LegendaryBookRelatedConstants.IdleDuringContestActionChance[(int)behaviorType]);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int indirectContestChance = (targetChar.GetCombatPower() > selfChar.GetCombatPower()) ? 25 : 75;
				bool flag2 = context.Random.CheckPercentProb(indirectContestChance);
				if (flag2)
				{
					sbyte[] priorities = AiHelper.LegendaryBookContestActionType.IndirectActionPriorities[(int)behaviorType];
					foreach (sbyte actionType in priorities)
					{
						sbyte personalityType = AiHelper.LegendaryBookContestActionType.ToPersonalityType[(int)actionType];
						int chance = (int)(60 + *(ref personalities.Items.FixedElementField + personalityType));
						bool flag3 = !context.Random.CheckPercentProb(chance);
						if (!flag3)
						{
							switch (actionType)
							{
							case 0:
							{
								GainExpByCombatAction action = new GainExpByCombatAction
								{
									CombatType = CombatType.Play
								};
								bool flag4 = action.CheckValid(selfChar, targetChar);
								if (flag4)
								{
									bool flag5 = this.Target.TargetCharId == taiwuCharId;
									if (flag5)
									{
										action.ApplyInitialChangesForTaiwu(context, selfChar, targetChar);
									}
									else
									{
										action.ApplyChanges(context, selfChar, targetChar);
									}
								}
								return false;
							}
							case 1:
							{
								ItemBase selectedItem = selfChar.SelectSpareableItem(context, targetChar.GetInteractionGrade(), false);
								bool flag6 = selectedItem == null;
								if (flag6)
								{
									return false;
								}
								ItemKey selectedItemKey = selectedItem.GetItemKey();
								GiveItemAction action2 = new GiveItemAction
								{
									TargetItem = selectedItemKey,
									Amount = 1,
									RefusePoisonousItem = targetChar.TryDetectAttachedPoisons(selectedItemKey)
								};
								bool flag7 = action2.CheckValid(selfChar, targetChar);
								if (flag7)
								{
									bool flag8 = this.Target.TargetCharId == taiwuCharId;
									if (flag8)
									{
										action2.ApplyInitialChangesForTaiwu(context, selfChar, targetChar);
									}
									else
									{
										action2.ApplyChanges(context, selfChar, targetChar);
									}
								}
								return false;
							}
							case 2:
								DomainManager.Character.HandlePoisonAction(context, selfChar, targetChar, ItemKey.Invalid, -1);
								return false;
							case 3:
								DomainManager.Character.HandlePlotHarmAction(context, selfChar, targetChar, ItemKey.Invalid, -1);
								return false;
							}
						}
					}
				}
				else
				{
					int alertFactor = targetChar.GetItemAlertFactor(itemKey, 1);
					bool flag9 = this.Target.TargetCharId == taiwuCharId;
					if (flag9)
					{
						sbyte[] priorities2 = AiHelper.LegendaryBookContestActionType.DirectActionTaiwuTargetPriorities[(int)behaviorType];
						foreach (sbyte actionType2 in priorities2)
						{
							sbyte personalityType2 = AiHelper.LegendaryBookContestActionType.ToPersonalityType[(int)actionType2];
							int chance2 = (int)(60 + *(ref personalities.Items.FixedElementField + personalityType2));
							bool flag10 = !context.Random.CheckPercentProb(chance2);
							if (!flag10)
							{
								CharacterDomain.AddLockMovementCharSet(selfChar.GetId());
								switch (actionType2)
								{
								case 4:
								{
									bool flag11 = DomainManager.Extra.IsAiActionInCooldown(selfCharId, 3, 3);
									if (!flag11)
									{
										monthlyEventCollection.AddChallengeForLegendaryBook(selfCharId, location, this.Target.TargetCharId, (ulong)itemKey);
										DomainManager.Extra.AddAiActionCooldown(context, selfCharId, 3, 3, 12);
									}
									break;
								}
								case 5:
									monthlyEventCollection.AddRequestLegendaryBook(selfCharId, location, this.Target.TargetCharId, (ulong)itemKey);
									return false;
								case 6:
									this.AddTradeForBookMonthlyEvent(selfChar, itemKey);
									return false;
								case 7:
								{
									bool flag12 = !DomainManager.Taiwu.CanTaiwuBeSneakyHarmfulActionTarget();
									if (!flag12)
									{
										StealItemAction action3 = new StealItemAction
										{
											TargetItem = itemKey,
											Amount = 1,
											Phase = selfChar.GetStealActionPhase(context.Random, targetChar, alertFactor, false)
										};
										action3.ApplyInitialChangesForTaiwu(context, selfChar, targetChar);
										return false;
									}
									break;
								}
								case 8:
								{
									bool flag13 = !DomainManager.Taiwu.CanTaiwuBeSneakyHarmfulActionTarget();
									if (!flag13)
									{
										StealItemAction action4 = new StealItemAction
										{
											TargetItem = itemKey,
											Amount = 1,
											Phase = selfChar.GetScamActionPhase(context.Random, targetChar, alertFactor, false)
										};
										action4.ApplyInitialChangesForTaiwu(context, selfChar, targetChar);
										return false;
									}
									break;
								}
								case 9:
								{
									bool flag14 = DomainManager.Extra.IsAiActionInCooldown(selfCharId, 3, 3);
									if (!flag14)
									{
										RobItemAction action5 = new RobItemAction
										{
											TargetItem = itemKey,
											Amount = 1,
											Phase = selfChar.GetRobActionPhase(context.Random, targetChar, alertFactor, false)
										};
										action5.ApplyInitialChangesForTaiwu(context, selfChar, targetChar);
										DomainManager.Extra.AddAiActionCooldown(context, selfCharId, 3, 3, 12);
										return false;
									}
									break;
								}
								}
							}
						}
						return false;
					}
					sbyte[] priorities3 = AiHelper.LegendaryBookContestActionType.DirectActionNpcTargetPriorities[(int)behaviorType];
					foreach (sbyte actionType3 in priorities3)
					{
						sbyte personalityType3 = AiHelper.LegendaryBookContestActionType.ToPersonalityType[(int)actionType3];
						int chance3 = (int)(60 + *(ref personalities.Items.FixedElementField + personalityType3));
						bool flag15 = !context.Random.CheckPercentProb(chance3);
						if (!flag15)
						{
							sbyte b = actionType3;
							sbyte b2 = b;
							if (b2 == 4)
							{
								AiHelper.NpcCombatResultType resultType = DomainManager.Character.SimulateCharacterCombat(context, selfChar, targetChar, CombatType.Beat, false, 1);
								bool flag16 = resultType == AiHelper.NpcCombatResultType.MajorVictory || resultType == AiHelper.NpcCombatResultType.MinorVictory;
								if (flag16)
								{
									DomainManager.Character.TransferInventoryItem(context, targetChar, selfChar, itemKey, 1);
									monthlyNotifications.AddChallengeForLegendaryBook(selfCharId, location, this.Target.TargetCharId, itemKey.ItemType, itemKey.TemplateId);
									lifeRecordCollection.AddLegendaryBookChallengeWin(selfCharId, currDate, this.Target.TargetCharId, location, itemKey.ItemType, itemKey.TemplateId);
								}
								else
								{
									lifeRecordCollection.AddLegendaryBookChallengeLose(selfCharId, currDate, this.Target.TargetCharId, location, itemKey.ItemType, itemKey.TemplateId);
								}
								return false;
							}
							if (b2 == 9)
							{
								RobItemAction action6 = new RobItemAction
								{
									TargetItem = itemKey,
									Amount = 1,
									Phase = selfChar.GetRobActionPhase(context.Random, targetChar, alertFactor, false)
								};
								bool flag17 = action6.CheckValid(selfChar, targetChar);
								if (flag17)
								{
									action6.ApplyChanges(context, selfChar, targetChar);
								}
								bool flag18 = selfChar.GetInventory().Items.ContainsKey(itemKey);
								if (flag18)
								{
									monthlyNotifications.AddRobLegendaryBook(selfCharId, location, this.Target.TargetCharId, itemKey.ItemType, itemKey.TemplateId);
								}
								return false;
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060076C4 RID: 30404 RVA: 0x00458B00 File Offset: 0x00456D00
		private unsafe void AddTradeForBookMonthlyEvent(Character character, ItemKey bookItemKey)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int charId = character.GetId();
			Location location = character.GetLocation();
			ResourceInts resources = *character.GetResources();
			int moneyWorth = *(ref resources.Items.FixedElementField + (IntPtr)6 * 4) * (int)GlobalConfig.ResourcesWorth[6];
			int authorityWorth = *(ref resources.Items.FixedElementField + (IntPtr)7 * 4) * (int)GlobalConfig.ResourcesWorth[7];
			int expWorth = character.GetExp() * 5;
			bool flag = moneyWorth >= authorityWorth && moneyWorth >= expWorth;
			if (flag)
			{
				monthlyEventCollection.AddExchangeLegendaryBookByMoney(charId, location, this.Target.TargetCharId, (ulong)bookItemKey);
			}
			else
			{
				bool flag2 = authorityWorth >= moneyWorth && authorityWorth >= expWorth;
				if (flag2)
				{
					monthlyEventCollection.AddExchangeLegendaryBookByAuthority(charId, location, this.Target.TargetCharId, (ulong)bookItemKey);
				}
				else
				{
					monthlyEventCollection.AddExchangeLegendaryBookByExperience(charId, location, this.Target.TargetCharId, (ulong)bookItemKey);
				}
			}
		}

		// Token: 0x060076C5 RID: 30405 RVA: 0x00458BF7 File Offset: 0x00456DF7
		public ContestForLegendaryBookAction()
		{
			this.LegendaryBookType = -1;
		}

		// Token: 0x060076C6 RID: 30406 RVA: 0x00458C08 File Offset: 0x00456E08
		public ContestForLegendaryBookAction(ContestForLegendaryBookAction other)
		{
			this.LegendaryBookType = other.LegendaryBookType;
			this.Target = other.Target;
			this.HasArrived = other.HasArrived;
		}

		// Token: 0x060076C7 RID: 30407 RVA: 0x00458C36 File Offset: 0x00456E36
		public void Assign(ContestForLegendaryBookAction other)
		{
			this.LegendaryBookType = other.LegendaryBookType;
			this.Target = other.Target;
			this.HasArrived = other.HasArrived;
		}

		// Token: 0x060076C8 RID: 30408 RVA: 0x00458C60 File Offset: 0x00456E60
		public override bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x060076C9 RID: 30409 RVA: 0x00458C74 File Offset: 0x00456E74
		public override int GetSerializedSize()
		{
			int totalSize = 18;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060076CA RID: 30410 RVA: 0x00458C9C File Offset: 0x00456E9C
		public unsafe override int Serialize(byte* pData)
		{
			*pData = (byte)this.LegendaryBookType;
			byte* pCurrData = pData + 1;
			pCurrData += this.Target.Serialize(pCurrData);
			*pCurrData = (this.HasArrived ? 1 : 0);
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060076CB RID: 30411 RVA: 0x00458CF0 File Offset: 0x00456EF0
		public unsafe override int Deserialize(byte* pData)
		{
			this.LegendaryBookType = *(sbyte*)pData;
			byte* pCurrData = pData + 1;
			pCurrData += this.Target.Deserialize(pCurrData);
			this.HasArrived = (*pCurrData != 0);
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x040020CA RID: 8394
		[SerializableGameDataField]
		public sbyte LegendaryBookType;
	}
}
