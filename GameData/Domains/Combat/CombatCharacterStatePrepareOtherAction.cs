using System;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x0200069F RID: 1695
	public class CombatCharacterStatePrepareOtherAction : CombatCharacterStateBase
	{
		// Token: 0x06006213 RID: 25107 RVA: 0x0037C886 File Offset: 0x0037AA86
		public CombatCharacterStatePrepareOtherAction(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.PrepareOtherAction)
		{
		}

		// Token: 0x06006214 RID: 25108 RVA: 0x0037C894 File Offset: 0x0037AA94
		public override void OnEnter()
		{
			DataContext context = this.CombatChar.GetDataContext();
			bool flag = this.CombatChar.GetPreparingOtherAction() < 0;
			if (flag)
			{
				sbyte actionType = this.CombatChar.NeedUseOtherAction;
				this.CombatChar.SetNeedUseOtherAction(context, -1);
				bool flag2 = !this.CheckOtherActionUsable(actionType);
				if (flag2)
				{
					this.TranslateProperState();
					return;
				}
				this.DoOtherActionCost(context, actionType);
				short prepareFrame = this.CombatChar.GetOtherActionPrepareFrame(actionType);
				this._leftPrepareFrame = (this._totalPrepareFrame = prepareFrame);
				this.CombatChar.NeedInterruptSurrender = false;
				this.CombatChar.SetPreparingOtherAction(actionType, context);
				bool flag3 = this.CombatChar.GetOtherActionPreparePercent() > 0;
				if (flag3)
				{
					this.CombatChar.SetOtherActionPreparePercent(0, context);
				}
				this.CombatChar.CanFleeOutOfRange = false;
			}
			this.CurrentCombatDomain.SetProperLoopAniAndParticle(context, this.CombatChar, false);
			this.CurrentCombatDomain.UpdateAllTeammateCommandUsable(context, this.CombatChar.IsAlly, ETeammateCommandImplement.InterruptOtherAction);
		}

		// Token: 0x06006215 RID: 25109 RVA: 0x0037C99C File Offset: 0x0037AB9C
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
				bool flag2 = this.CombatChar.GetPreparingOtherAction() < 0;
				if (flag2)
				{
					this.TranslateProperState();
					result = false;
				}
				else
				{
					bool flag3 = this.CombatChar.GetPreparingOtherAction() == 4;
					if (flag3)
					{
						bool needInterruptSurrender = this.CombatChar.NeedInterruptSurrender;
						if (needInterruptSurrender)
						{
							this.TranslateProperState();
						}
						result = false;
					}
					else
					{
						bool flag4 = this._leftPrepareFrame <= 0;
						if (flag4)
						{
							AdaptableLog.TagWarning("CombatCharacterStatePrepareOtherAction", PredefinedLog.Instance[5].Info, true);
							this.TranslateProperState();
							result = false;
						}
						else
						{
							this._leftPrepareFrame -= 1;
							byte preparePercent = (byte)((this._totalPrepareFrame - this._leftPrepareFrame) * 100 / this._totalPrepareFrame);
							bool flag5 = preparePercent != this.CombatChar.GetOtherActionPreparePercent();
							if (flag5)
							{
								this.CombatChar.SetOtherActionPreparePercent(preparePercent, this.CombatChar.GetDataContext());
							}
							bool flag6 = this._leftPrepareFrame == 0;
							if (flag6)
							{
								DataContext context = this.CombatChar.GetDataContext();
								sbyte actionType = this.CombatChar.GetPreparingOtherAction();
								switch (actionType)
								{
								case 0:
									this.CurrentCombatDomain.HealInjuryInCombat(context, this.CombatChar, this.CombatChar, true);
									break;
								case 1:
									this.CurrentCombatDomain.HealPoisonInCombat(context, this.CombatChar, this.CombatChar, true);
									break;
								case 2:
									this.CurrentCombatDomain.Flee(context, this.CombatChar);
									break;
								case 3:
									this.CombatChar.NeedAnimalAttack = true;
									break;
								}
								bool flag7 = actionType != 3 && this.CombatChar.NeedUseOtherAction != -1;
								if (flag7)
								{
									this.ReEnter(context);
								}
								else
								{
									this.TranslateProperState();
								}
								bool flag8 = CombatDomain.OtherActionSpecialEffectId.Length > (int)actionType;
								if (flag8)
								{
									this.CombatChar.GetShowEffectList().ShowEffectList.Add(new ShowSpecialEffectDisplayData(this.CombatChar.GetId(), CombatDomain.OtherActionSpecialEffectId[(int)actionType], 0, ItemKey.Invalid));
								}
							}
							bool flag9 = this.CombatChar.NeedUseSkillFreeId >= 0;
							if (flag9)
							{
								this.CombatChar.StateMachine.TranslateState(CombatCharacterStateType.PrepareSkill);
							}
							bool needNormalAttack = this.CombatChar.NeedNormalAttack;
							if (needNormalAttack)
							{
								this.CombatChar.StateMachine.TranslateState(CombatCharacterStateType.PrepareAttack);
							}
							result = false;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06006216 RID: 25110 RVA: 0x0037CC1C File Offset: 0x0037AE1C
		private bool CheckOtherActionUsable(sbyte actionType)
		{
			bool flag = actionType < 0 || actionType >= 5;
			bool flag2 = flag;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				if (!true)
				{
				}
				switch (actionType)
				{
				case 0:
					flag = (this.CombatChar.GetHealInjuryCount() > 0);
					break;
				case 1:
					flag = (this.CombatChar.GetHealPoisonCount() > 0);
					break;
				case 2:
					flag = (this.CombatChar.CanFleeOutOfRange || this.CombatChar.GetOtherActionCanUse()[2]);
					break;
				case 3:
					flag = (this.CombatChar.GetAnimalAttackCount() > 0);
					break;
				default:
					flag = true;
					break;
				}
				if (!true)
				{
				}
				bool costEnough = flag;
				bool flag3 = !costEnough;
				result = (!flag3 && (!this.CombatChar.IsAlly || this.CombatChar.GetOtherActionCanUse()[(int)actionType]));
			}
			return result;
		}

		// Token: 0x06006217 RID: 25111 RVA: 0x0037CCF0 File Offset: 0x0037AEF0
		private void DoOtherActionCost(DataContext context, sbyte actionType)
		{
			switch (actionType)
			{
			case 0:
				this.CombatChar.SetHealInjuryCount((byte)Math.Max((int)(this.CombatChar.GetHealInjuryCount() - 1), 0), context);
				DomainManager.Character.UseCombatResources(context, this.CombatChar.GetId(), EHealActionType.Healing, 1);
				break;
			case 1:
				this.CombatChar.SetHealPoisonCount((byte)Math.Max((int)(this.CombatChar.GetHealPoisonCount() - 1), 0), context);
				DomainManager.Character.UseCombatResources(context, this.CombatChar.GetId(), EHealActionType.Detox, 1);
				break;
			case 3:
			{
				this.CombatChar.SetAnimalAttackCount((sbyte)Math.Max((int)(this.CombatChar.GetAnimalAttackCount() - 1), 0), context);
				ProfessionData hunterData = DomainManager.Extra.GetProfessionData(1);
				HunterSkillsData hunterSkillsData = (HunterSkillsData)hunterData.SkillsData;
				hunterSkillsData.UsedCarrierAnimalAttackCount += 1;
				DomainManager.Extra.SetProfessionData(context, hunterData);
				break;
			}
			}
		}

		// Token: 0x06006218 RID: 25112 RVA: 0x0037CDEE File Offset: 0x0037AFEE
		private void ReEnter(DataContext context)
		{
			this.CombatChar.SetPreparingOtherAction(-1, context);
			this.OnEnter();
		}

		// Token: 0x06006219 RID: 25113 RVA: 0x0037CE08 File Offset: 0x0037B008
		private void TranslateProperState()
		{
			DataContext context = this.CombatChar.GetDataContext();
			OtherActionTypeItem otherActionConfig = OtherActionType.Instance[this.CombatChar.GetPreparingOtherAction()];
			this.CombatChar.SetPreparingOtherAction(-1, context);
			bool flag = otherActionConfig != null;
			if (flag)
			{
				bool flag2 = this.CombatChar.IsActorSkeleton && !string.IsNullOrEmpty(otherActionConfig.PrepareEndAnim);
				if (flag2)
				{
					this.CombatChar.SetAnimationToPlayOnce(otherActionConfig.PrepareEndAnim, context);
				}
				bool flag3 = this.CombatChar.IsActorSkeleton && !string.IsNullOrEmpty(otherActionConfig.PrepareEndParticle);
				if (flag3)
				{
					this.CombatChar.SetParticleToPlay(otherActionConfig.PrepareEndParticle, context);
				}
			}
			CombatCharacterStateType properState = this.CombatChar.StateMachine.GetProperState();
			bool flag4 = properState != CombatCharacterStateType.PrepareOtherAction;
			if (flag4)
			{
				this.CombatChar.StateMachine.TranslateState();
			}
			else
			{
				this.OnEnter();
			}
		}

		// Token: 0x04001AB4 RID: 6836
		private short _totalPrepareFrame;

		// Token: 0x04001AB5 RID: 6837
		private short _leftPrepareFrame;
	}
}
