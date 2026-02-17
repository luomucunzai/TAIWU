using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.Combat
{
	// Token: 0x020006A0 RID: 1696
	public class CombatCharacterStatePrepareSkill : CombatCharacterStateBase
	{
		// Token: 0x0600621A RID: 25114 RVA: 0x0037CEF7 File Offset: 0x0037B0F7
		public CombatCharacterStatePrepareSkill(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.PrepareSkill)
		{
		}

		// Token: 0x0600621B RID: 25115 RVA: 0x0037CF04 File Offset: 0x0037B104
		public override void OnEnter()
		{
			DataContext context = this.CombatChar.GetDataContext();
			bool flag = this.CombatChar.NeedUseSkillFreeId >= 0;
			short skillId;
			if (flag)
			{
				skillId = this.CombatChar.NeedUseSkillFreeId;
				List<CastFreeData> castFreeDataList = this.CombatChar.CastFreeDataList;
				ECombatCastFreePriority priority = castFreeDataList[castFreeDataList.Count - 1].Priority;
				this.CombatChar.CastFreeDataList.RemoveAt(this.CombatChar.CastFreeDataList.Count - 1);
				this.CombatChar.SetAutoCastingSkill(false, context);
				bool flag2 = !this.CurrentCombatDomain.CanCastSkill(this.CombatChar, skillId, true, false) && priority != ECombatCastFreePriority.Gm;
				if (flag2)
				{
					this.CombatChar.StateMachine.TranslateState();
					return;
				}
				bool flag3 = priority != ECombatCastFreePriority.Gm;
				if (flag3)
				{
					this.CombatChar.SetAutoCastingSkill(true, context);
				}
				this.CombatChar.MoveData.ResetJumpState(context, false);
			}
			else
			{
				bool needChangeSkill = this.CombatChar.NeedChangeSkill;
				if (needChangeSkill)
				{
					skillId = this.CombatChar.NeedUseSkillId;
					this.CombatChar.SetNeedUseSkillId(context, -1);
					this.CombatChar.SetAutoCastingSkill(false, context);
					bool flag4 = skillId == this.CombatChar.NeedAddEffectAgileSkillId || skillId == this.CombatChar.GetAffectingMoveSkillId();
					if (flag4)
					{
						this.CombatChar.StateMachine.TranslateState();
						return;
					}
					bool flag5 = !this.CurrentCombatDomain.CanCastSkill(this.CombatChar, skillId, false, false);
					if (flag5)
					{
						this.CombatChar.StateMachine.TranslateState();
						return;
					}
					this.CurrentCombatDomain.DoCombatSkillCost(context, this.CombatChar, skillId);
				}
				else
				{
					skillId = -1;
				}
			}
			bool flag6 = skillId >= 0;
			if (flag6)
			{
				bool flag7 = this.CombatChar.GetAffectingMoveSkillId() >= 0 && DomainManager.CombatSkill.GetSkillType(this.CombatChar.GetId(), skillId) == 5;
				if (flag7)
				{
					Events.RaiseCastLegSkillWithAgile(context, this.CombatChar, skillId);
				}
				skillId = (short)DomainManager.SpecialEffect.ModifyData(this.CombatChar.GetId(), -1, 156, (int)skillId, -1, -1, -1);
				CombatSkillItem configData = Config.CombatSkill.Instance[skillId];
				GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(this.CombatChar.GetId(), skillId));
				int totalProgress = skill.GetPrepareTotalProgress();
				bool flag8 = this.CurrentCombatDomain.SkillBodyPartHasHeavyInjury(this.CombatChar, skillId);
				if (flag8)
				{
					totalProgress *= 2;
				}
				this.CombatChar.SkillPrepareTotalProgress = totalProgress;
				this.CombatChar.SkillPrepareCurrProgress = 0;
				this.CombatChar.SetPreparingSkillId(skillId, context);
				DomainManager.Combat.UpdateAllTeammateCommandUsable(context, this.CombatChar.IsAlly, -1);
				Events.RaisePrepareSkillEffectNotYetCreated(context, this.CombatChar, skillId);
				bool flag9 = configData.EquipType == 1;
				if (flag9)
				{
					DomainManager.SpecialEffect.Add(context, this.CombatChar.GetId(), skillId, 0, -1);
				}
				Events.RaisePrepareSkillBegin(context, this.CombatChar.GetId(), this.CombatChar.IsAlly, skillId);
			}
			this.CurrentCombatDomain.SetProperLoopAniAndParticle(context, this.CombatChar, false);
			CombatSkillData skillData;
			bool flag10 = DomainManager.Combat.TryGetCombatSkillData(this.CombatChar.GetId(), this.CombatChar.GetPreparingSkillId(), out skillData) && skillData.GetSilencing();
			if (flag10)
			{
				DomainManager.Combat.InterruptSkill(context, this.CombatChar, 100);
			}
		}

		// Token: 0x0600621C RID: 25116 RVA: 0x0037D280 File Offset: 0x0037B480
		public override void OnExit()
		{
		}

		// Token: 0x0600621D RID: 25117 RVA: 0x0037D284 File Offset: 0x0037B484
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
				bool needChangeBossPhase = DomainManager.Combat.GetCombatCharacter(!this.CombatChar.IsAlly, false).NeedChangeBossPhase;
				if (needChangeBossPhase)
				{
					result = false;
				}
				else
				{
					bool flag2 = this.CombatChar.GetPreparingSkillId() < 0;
					if (flag2)
					{
						CombatCharacterStateType properState = this.CombatChar.StateMachine.GetProperState();
						this.CombatChar.MoveData.ClearSkillPrepareMoveDist();
						bool flag3 = properState != CombatCharacterStateType.PrepareSkill;
						if (flag3)
						{
							this.CombatChar.StateMachine.TranslateState(properState);
						}
						else
						{
							this.OnEnter();
						}
						result = false;
					}
					else
					{
						DataContext context = this.CombatChar.GetDataContext();
						int newProgress = this.CombatChar.SkillPrepareCurrProgress + this.CurrentCombatDomain.GetSkillPrepareSpeed(this.CombatChar);
						this.CombatChar.SkillPrepareCurrProgress = Math.Min(newProgress, this.CombatChar.SkillPrepareTotalProgress);
						byte preparePercent = (byte)CValuePercent.ParseInt(this.CombatChar.SkillPrepareCurrProgress, this.CombatChar.SkillPrepareTotalProgress);
						bool flag4 = preparePercent != this.CombatChar.GetSkillPreparePercent();
						if (flag4)
						{
							this.CombatChar.SetSkillPreparePercent(preparePercent, this.CombatChar.GetDataContext());
							Events.RaisePrepareSkillProgressChange(this.CombatChar.GetDataContext(), this.CombatChar.GetId(), this.CombatChar.IsAlly, this.CombatChar.GetPreparingSkillId(), (sbyte)preparePercent);
						}
						bool flag5 = this.CombatChar.SkillPrepareCurrProgress == this.CombatChar.SkillPrepareTotalProgress;
						if (flag5)
						{
							short skillId = this.CombatChar.GetPreparingSkillId();
							CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillId];
							this.CurrentCombatDomain.CalcSkillQiDisorderAndInjury(this.CombatChar, skillConfig);
							bool flag6 = this.CurrentCombatDomain.IsMainCharacter(this.CombatChar);
							if (flag6)
							{
								this.CurrentCombatDomain.UpdateAllTeammateCommandUsable(context, this.CombatChar.IsAlly, -1);
							}
							bool flag7 = skillConfig.EquipType == 1 && this.CurrentCombatDomain.IsMainCharacter(this.CombatChar);
							if (flag7)
							{
								this.CurrentCombatDomain.ForceAllTeammateLeaveCombatField(context, this.CombatChar.IsAlly);
							}
							this.CombatChar.MoveData.ClearSkillPrepareMoveDist();
							this.CombatChar.StateMachine.TranslateState((!this.CurrentCombatDomain.IsCharacterFallen(this.CombatChar)) ? CombatCharacterStateType.CastSkill : CombatCharacterStateType.Idle);
							result = false;
						}
						else
						{
							bool needChangeSkill = this.CombatChar.NeedChangeSkill;
							if (needChangeSkill)
							{
								bool flag8 = Config.CombatSkill.Instance[this.CombatChar.NeedUseSkillId].EquipType == 1;
								if (flag8)
								{
									Events.RaiseChangePreparingSkillBegin(context, this.CombatChar.GetId(), this.CombatChar.GetPreparingSkillId(), this.CombatChar.NeedUseSkillId);
									this.OnEnter();
								}
								else
								{
									this.CurrentCombatDomain.CastAgileOrDefenseWithoutPrepare(this.CombatChar, this.CombatChar.NeedUseSkillId);
									this.CombatChar.SetNeedUseSkillId(this.CombatChar.GetDataContext(), -1);
								}
							}
							bool needNormalAttack = this.CombatChar.NeedNormalAttack;
							if (needNormalAttack)
							{
								this.CombatChar.StateMachine.TranslateState(CombatCharacterStateType.PrepareAttack);
							}
							bool needUnlockAttack = this.CombatChar.NeedUnlockAttack;
							if (needUnlockAttack)
							{
								this.CombatChar.StateMachine.TranslateState(CombatCharacterStateType.UnlockAttack);
							}
							bool needShowChangeTrick = this.CombatChar.NeedShowChangeTrick;
							if (needShowChangeTrick)
							{
								this.CombatChar.StateMachine.TranslateState(CombatCharacterStateType.SelectChangeTrick);
							}
							result = false;
						}
					}
				}
			}
			return result;
		}
	}
}
