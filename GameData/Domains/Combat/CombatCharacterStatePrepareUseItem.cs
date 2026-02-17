using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x020006A2 RID: 1698
	public class CombatCharacterStatePrepareUseItem : CombatCharacterStateBase
	{
		// Token: 0x06006220 RID: 25120 RVA: 0x0037D699 File Offset: 0x0037B899
		public CombatCharacterStatePrepareUseItem(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.PrepareUseItem)
		{
		}

		// Token: 0x06006221 RID: 25121 RVA: 0x0037D6A8 File Offset: 0x0037B8A8
		public override void OnEnter()
		{
			bool flag = this.CombatChar.GetPreparingItem().IsValid();
			if (!flag)
			{
				base.OnEnter();
				this.AutoUpdateDelayCall = false;
				DataContext context = this.CombatChar.GetDataContext();
				ItemKey itemKey = this.CombatChar.NeedUseItem;
				ItemKey repairItem = this.CombatChar.NeedRepairItem;
				this._itemUseType = this.CombatChar.ItemUseType;
				this._itemTargetBodyParts = this.CombatChar.ItemTargetBodyParts;
				this.CombatChar.SetNeedUseItem(context, ItemKey.Invalid);
				this.CombatChar.NeedRepairItem = ItemKey.Invalid;
				this.CombatChar.ItemUseType = -1;
				this.CombatChar.ItemTargetBodyParts = null;
				bool flag2 = !this.CheckItemKeyIsValid(itemKey);
				if (flag2)
				{
					this.ClearStateAndTranslateState();
				}
				else
				{
					bool flag3 = itemKey.ItemType == 12 && SharedConstValue.SwordFragment2BossId.ContainsKey(itemKey.TemplateId);
					if (flag3)
					{
						short skillId = DomainManager.Item.GetSwordFragmentCurrSkill(itemKey);
						List<short> learnedSkills = this.CombatChar.GetCharacter().GetLearnedCombatSkills();
						bool flag4 = !learnedSkills.Contains(skillId);
						if (flag4)
						{
							sbyte outlineType = this.CombatChar.GetCharacter().GetBehaviorType();
							byte outlineIndex = CombatSkillStateHelper.GetPageInternalIndex(outlineType, 0, 0);
							ushort readingState = CombatSkillStateHelper.SetPageRead(0, outlineIndex);
							ushort activeState = CombatSkillStateHelper.SetPageActive(0, outlineIndex);
							for (byte page = 1; page <= 5; page += 1)
							{
								byte index = CombatSkillStateHelper.GetPageInternalIndex(outlineType, 0, page);
								readingState = CombatSkillStateHelper.SetPageRead(readingState, index);
								activeState = CombatSkillStateHelper.SetPageActive(activeState, index);
							}
							GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.CreateCombatSkill(this.CombatChar.GetId(), skillId, readingState);
							skill.SetActivationState(activeState, context);
							learnedSkills.Add(skillId);
							this.CombatChar.GetCharacter().SetLearnedCombatSkills(learnedSkills, context);
							this.CombatChar.ForgetAfterCombatSkills.Add(skillId);
							DomainManager.SpecialEffect.Add(context, this.CombatChar.GetId(), skillId, 1, -1);
						}
						this.CombatChar.GetCharacter().ChangeXiangshuInfection(context, (int)GlobalConfig.Instance.UseSwordFragmentAddXiangshuInfection);
						this.CurrentCombatDomain.CastSkillFree(context, this.CombatChar, skillId, ECombatCastFreePriority.SwordFragment);
						this.CombatChar.StateMachine.TranslateState(CombatCharacterStateType.PrepareSkill);
					}
					else
					{
						int costWisdom = itemKey.GetConsumedFeatureMedals();
						bool costEnough = this.CombatChar.IsAlly ? ((int)this.CurrentCombatDomain.GetSelfTeamWisdomCount() >= costWisdom) : ((int)this.CurrentCombatDomain.GetEnemyTeamWisdomCount() >= costWisdom);
						bool flag5 = !costEnough;
						if (flag5)
						{
							this.ClearStateAndTranslateState();
						}
						else
						{
							bool flag6 = costWisdom > 0;
							if (flag6)
							{
								this.CurrentCombatDomain.CostWisdom(context, this.CombatChar.IsAlly, costWisdom);
							}
							ICombatItemConfig configAs = itemKey.GetConfigAs<ICombatItemConfig>();
							int prepareFrames = (configAs != null) ? configAs.UseFrame : 0;
							this.CombatChar.SetPreparingItem(itemKey, context);
							bool flag7 = this.CombatChar.GetUseItemPreparePercent() > 0;
							if (flag7)
							{
								this.CombatChar.SetUseItemPreparePercent(0, context);
							}
							sbyte itemType = itemKey.ItemType;
							bool flag8 = itemType == 7 || itemType == 9;
							bool flag9;
							if (!flag8 && (itemKey.ItemType != 8 || this._itemUseType != 0))
							{
								if (itemKey.ItemType == 12)
								{
									short templateId = itemKey.TemplateId;
									if (templateId >= 0)
									{
										flag9 = (templateId <= 8);
										goto IL_363;
									}
								}
								flag9 = false;
								IL_363:;
							}
							else
							{
								flag9 = true;
							}
							bool flag10 = flag9;
							if (flag10)
							{
								CombatItemUseItem eatConfig = CombatItemUse.DefValue.EatItem;
								this.CombatChar.SetAnimationToPlayOnce(eatConfig.Animation, context);
								this.CombatChar.SetParticleToPlay((this.CombatChar.BossConfig == null) ? eatConfig.Particle : this.CombatChar.BossConfig.EatParticles[(int)this.CombatChar.GetBossPhase()], context);
							}
							else
							{
								bool flag11 = itemKey.ItemType == 8;
								if (flag11)
								{
									CombatItemUseItem throwConfig = CombatItemUse.DefValue.PrepareThrowPoison;
									this.CombatChar.SetAnimationToPlayOnce(throwConfig.Animation, context);
									this.CombatChar.SetParticleToPlay(throwConfig.Particle, context);
									this.CombatChar.SetSkillSoundToPlay(throwConfig.Sound, context);
								}
								else
								{
									bool flag12 = itemKey.ItemType == 6;
									if (flag12)
									{
										this.CombatChar.RepairingItem = repairItem;
										ItemBase targetItem = DomainManager.Item.GetBaseItem(repairItem);
										bool flag13 = targetItem.GetMaxDurability() == targetItem.GetCurrDurability();
										if (flag13)
										{
											this.ClearStateAndTranslateState();
											return;
										}
										prepareFrames *= (int)(targetItem.GetMaxDurability() - targetItem.GetCurrDurability());
										prepareFrames *= ((targetItem.GetCurrDurability() > 0) ? 1 : 2);
										CombatItemUseItem repairConfig = CombatItemUse.Instance[3];
										this.CombatChar.SpecialAnimationLoop = repairConfig.Animation;
										this.CombatChar.SetAnimationToLoop(repairConfig.Animation, context);
										this.CombatChar.SetParticleToLoop(repairConfig.Particle, context);
									}
									else
									{
										bool flag14;
										if (itemKey.ItemType == 12)
										{
											short templateId = itemKey.TemplateId;
											if (templateId >= 73)
											{
												flag14 = (templateId <= 81);
												goto IL_511;
											}
										}
										flag14 = false;
										IL_511:
										bool flag15 = flag14;
										if (flag15)
										{
											CombatItemUseItem ropeConfig = CombatItemUse.Instance[4];
											this.CombatChar.SpecialAnimationLoop = ropeConfig.Animation;
											this.CombatChar.SetAnimationToLoop(ropeConfig.Animation, context);
											this.CombatChar.SetParticleToLoop(ropeConfig.Particle, context);
											this.CombatChar.SetSoundToLoop(ropeConfig.Sound, context);
										}
										else
										{
											bool flag16;
											if (itemKey.ItemType == 12)
											{
												short templateId = itemKey.TemplateId;
												if (templateId >= 375)
												{
													flag16 = (templateId <= 384);
													goto IL_5A3;
												}
											}
											flag16 = false;
											IL_5A3:
											bool flag17 = flag16;
											if (flag17)
											{
												CombatItemUseItem useItemConfig = CombatItemUse.Instance[Config.Misc.Instance[itemKey.TemplateId].CombatPrepareUseEffect];
												this.CombatChar.SetAnimationToPlayOnce(useItemConfig.Animation, context);
												this.CombatChar.SetParticleToPlay(useItemConfig.Particle, context);
											}
											else
											{
												CombatItemUseItem useXiangshuSwordConfig = CombatItemUse.Instance[2];
												this.CombatChar.SetAnimationToPlayOnce(useXiangshuSwordConfig.Animation, context);
											}
										}
									}
								}
							}
							DomainManager.Combat.UpdateAllTeammateCommandUsable(context, this.CombatChar.IsAlly, ETeammateCommandImplement.InterruptOtherAction);
							base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.OnPrepared), new CombatCharacterStateBase.CombatCharacterStateDelayCallTickPercent(this.OnPrepareTickPercent), prepareFrames);
						}
					}
				}
			}
		}

		// Token: 0x06006222 RID: 25122 RVA: 0x0037DD08 File Offset: 0x0037BF08
		public override void OnExit()
		{
		}

		// Token: 0x06006223 RID: 25123 RVA: 0x0037DD0C File Offset: 0x0037BF0C
		public override bool OnUpdate()
		{
			bool flag = !base.OnUpdate();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this.CheckInterruptPrepare();
				if (flag2)
				{
					result = false;
				}
				else
				{
					base.UpdateDelayCall();
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06006224 RID: 25124 RVA: 0x0037DD48 File Offset: 0x0037BF48
		private bool CheckInterruptPrepare()
		{
			bool preparingItemInvalid = !this.CombatChar.GetValidItems().Contains(this.CombatChar.GetPreparingItem());
			bool repairingItemInvalid = this.CombatChar.GetPreparingItem().ItemType == 6 && DomainManager.Item.TryGetBaseItem(this.CombatChar.RepairingItem) == null;
			bool flag = !preparingItemInvalid && !repairingItemInvalid;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.ClearStateAndTranslateState();
				result = true;
			}
			return result;
		}

		// Token: 0x06006225 RID: 25125 RVA: 0x0037DDC4 File Offset: 0x0037BFC4
		private void OnPrepareTickPercent(int preparePercent)
		{
			bool flag = preparePercent != (int)this.CombatChar.GetUseItemPreparePercent();
			if (flag)
			{
				this.CombatChar.SetUseItemPreparePercent((byte)preparePercent, this.CombatChar.GetDataContext());
			}
		}

		// Token: 0x06006226 RID: 25126 RVA: 0x0037DE00 File Offset: 0x0037C000
		private void OnPrepared()
		{
			DataContext context = this.CombatChar.GetDataContext();
			ItemKey itemKey = this.CombatChar.GetPreparingItem();
			bool enterUseItemState = false;
			bool flag;
			if (itemKey.ItemType != 7 && itemKey.ItemType != 9 && (itemKey.ItemType != 8 || this._itemUseType != 0))
			{
				if (itemKey.ItemType == 12)
				{
					short templateId = itemKey.TemplateId;
					if (templateId >= 0 && templateId <= 8)
					{
						goto IL_88;
					}
				}
				if (!ItemTemplateHelper.IsTianJieFuLu(itemKey.ItemType, itemKey.TemplateId))
				{
					flag = (itemKey.ItemType == 5 && DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(44));
					goto IL_89;
				}
			}
			IL_88:
			flag = true;
			IL_89:
			bool flag2 = flag;
			if (flag2)
			{
				bool flag3 = ItemTemplateHelper.IsTianJieFuLu(itemKey.ItemType, itemKey.TemplateId);
				if (flag3)
				{
					DomainManager.Extra.EatTianJieFuLu(context, this.CombatChar.GetId(), itemKey, ItemTemplateHelper.GetTianJieFuLuCountUnit());
				}
				else
				{
					DomainManager.Character.AddEatingItem(context, this.CombatChar.GetId(), itemKey, this._itemTargetBodyParts);
				}
				this.SyncInjuryAndPoison();
				bool flag4 = !EatingItems.IsWug(itemKey);
				if (flag4)
				{
					Events.RaiseUsedMedicine(context, this.CombatChar.GetId(), itemKey);
				}
				else
				{
					this.CurrentCombatDomain.ShowWugKingEffectTips(context, this.CombatChar.GetId(), this.CombatChar.GetId());
				}
				bool flag5 = this.CombatChar.GetOldDisorderOfQi() > this.CombatChar.GetCharacter().GetDisorderOfQi();
				if (flag5)
				{
					this.CombatChar.SetOldDisorderOfQi(this.CombatChar.GetCharacter().GetDisorderOfQi(), context);
				}
			}
			else
			{
				bool flag6 = itemKey.ItemType == 6;
				if (flag6)
				{
					short newDurability = DomainManager.Building.RepairItemOptional(context, this.CombatChar.GetId(), itemKey, this.CombatChar.RepairingItem, 1).Durability;
					CombatWeaponData weaponData;
					bool flag7 = DomainManager.Combat.TryGetElement_WeaponDataDict(this.CombatChar.RepairingItem.Id, out weaponData);
					if (flag7)
					{
						weaponData.SetDurability(newDurability, context);
					}
					DomainManager.Combat.EnsureOldDurability(this.CombatChar.RepairingItem);
					this.CombatChar.SetParticleToLoop(null, context);
				}
				else
				{
					bool flag8;
					if (itemKey.ItemType == 12)
					{
						short templateId = itemKey.TemplateId;
						if (templateId >= 200)
						{
							flag8 = (templateId <= 209);
							goto IL_231;
						}
					}
					flag8 = false;
					IL_231:
					bool flag9 = flag8;
					if (!flag9)
					{
						enterUseItemState = true;
					}
				}
			}
			this.CombatChar.SpecialAnimationLoop = null;
			bool flag10 = enterUseItemState;
			if (flag10)
			{
				this.CombatChar.UsingItem = this.CombatChar.GetPreparingItem();
				this.CombatChar.StateMachine.TranslateState(CombatCharacterStateType.UseItem);
			}
			else
			{
				bool flag11 = this.CombatChar.NeedUseItem.IsValid();
				if (flag11)
				{
					this.ClearState();
					this.OnEnter();
				}
				else
				{
					this.ClearStateAndTranslateState();
				}
			}
		}

		// Token: 0x06006227 RID: 25127 RVA: 0x0037E0C4 File Offset: 0x0037C2C4
		private void ClearState()
		{
			DataContext context = this.CombatChar.GetDataContext();
			this.CombatChar.SetPreparingItem(ItemKey.Invalid, context);
			this.CombatChar.RepairingItem = ItemKey.Invalid;
			this.CombatChar.SetParticleToLoop(null, context);
			this.CombatChar.SpecialAnimationLoop = null;
		}

		// Token: 0x06006228 RID: 25128 RVA: 0x0037E11A File Offset: 0x0037C31A
		private void ClearStateAndTranslateState()
		{
			this.ClearState();
			this.CombatChar.StateMachine.TranslateState();
		}

		// Token: 0x06006229 RID: 25129 RVA: 0x0037E138 File Offset: 0x0037C338
		private unsafe void SyncInjuryAndPoison()
		{
			DataContext context = this.CombatChar.GetDataContext();
			PoisonInts poisons = *this.CombatChar.GetCharacter().GetPoisoned();
			PoisonInts lastPoisons = *this.CombatChar.GetPoison();
			DomainManager.Combat.SetPoisons(context, this.CombatChar, poisons, true);
			for (sbyte type = 0; type < 6; type += 1)
			{
				int addedPoison = *(ref poisons.Items.FixedElementField + (IntPtr)type * 4) - *(ref lastPoisons.Items.FixedElementField + (IntPtr)type * 4);
				bool flag = addedPoison > 0;
				if (flag)
				{
					Events.RaiseAddPoison(context, -1, this.CombatChar.GetId(), type, 0, addedPoison, -1, false);
				}
			}
			DomainManager.Combat.SetInjuries(context, this.CombatChar, this.CombatChar.GetCharacter().GetInjuries(), true, true);
		}

		// Token: 0x0600622A RID: 25130 RVA: 0x0037E214 File Offset: 0x0037C414
		private bool CheckItemKeyIsValid(ItemKey itemKey)
		{
			bool flag = !itemKey.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte itemType = itemKey.ItemType;
				if (!true)
				{
				}
				bool flag2;
				if (itemType != 6)
				{
					flag2 = this.CombatChar.GetValidItems().Contains(itemKey);
				}
				else
				{
					GameData.Domains.Item.CraftTool craftTool;
					flag2 = (DomainManager.Item.TryGetElement_CraftTools(itemKey.Id, out craftTool) && craftTool.GetCurrDurability() >= 0);
				}
				if (!true)
				{
				}
				result = flag2;
			}
			return result;
		}

		// Token: 0x04001AB6 RID: 6838
		private sbyte _itemUseType;

		// Token: 0x04001AB7 RID: 6839
		private List<sbyte> _itemTargetBodyParts;
	}
}
