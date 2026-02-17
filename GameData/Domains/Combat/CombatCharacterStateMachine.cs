using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;

namespace GameData.Domains.Combat
{
	// Token: 0x0200069D RID: 1693
	public class CombatCharacterStateMachine
	{
		// Token: 0x06006205 RID: 25093 RVA: 0x0037BE6C File Offset: 0x0037A06C
		public void Init(CombatDomain combatDomain, CombatCharacter combatChar)
		{
			this._currentState = null;
			this._lastUpdateCombatFrame = ulong.MaxValue;
			this._currentCombatDomain = combatDomain;
			this._combatChar = combatChar;
			this._allStates.Clear();
			this.RegisterState(new CombatCharacterStateIdle(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStateSelectChangeTrick(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStatePrepareAttack(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStateBreakAttack(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStateUnlockAttack(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStateRawCreate(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStatePrepareUnlockAttack(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStateAttack(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStatePrepareSkill(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStateCastSkill(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStatePrepareOtherAction(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStatePrepareUseItem(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStateUseItem(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStateSelectMercy(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStateDelaySettlement(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStateChangeCharacter(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStateTeammateCommand(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStateChangeBossPhase(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStateAnimalAttack(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStateJumpMove(combatDomain, combatChar));
			this.RegisterState(new CombatCharacterStateSpecialShow(combatDomain, combatChar));
		}

		// Token: 0x06006206 RID: 25094 RVA: 0x0037BFCC File Offset: 0x0037A1CC
		public void OnUpdate()
		{
			bool flag = this._currentState != null;
			if (flag)
			{
				bool isUpdateOnPause = this._currentState.IsUpdateOnPause;
				if (isUpdateOnPause)
				{
					this._currentState.OnUpdate();
				}
				else
				{
					bool flag2 = this._lastUpdateCombatFrame != this._currentCombatDomain.GetCombatFrame();
					if (flag2)
					{
						this._lastUpdateCombatFrame = this._currentCombatDomain.GetCombatFrame();
						this._currentState.OnUpdate();
					}
				}
				Events.RaiseCombatStateMachineUpdateEnd(this._currentCombatDomain.Context, this._combatChar);
			}
		}

		// Token: 0x06006207 RID: 25095 RVA: 0x0037C058 File Offset: 0x0037A258
		public CombatCharacterStateBase GetCurrentState()
		{
			return this._currentState;
		}

		// Token: 0x06006208 RID: 25096 RVA: 0x0037C060 File Offset: 0x0037A260
		public CombatCharacterStateType GetCurrentStateType()
		{
			return this.GetCurrentState().StateType;
		}

		// Token: 0x06006209 RID: 25097 RVA: 0x0037C06D File Offset: 0x0037A26D
		public void TranslateState()
		{
			this.TranslateState(this.GetProperState());
		}

		// Token: 0x0600620A RID: 25098 RVA: 0x0037C07C File Offset: 0x0037A27C
		public void TranslateState(CombatCharacterStateType stateType)
		{
			bool flag = this._allStates == null;
			if (flag)
			{
				throw new Exception("State machine not inited");
			}
			bool flag2;
			if (this._allStates.ContainsKey(stateType))
			{
				CombatCharacterStateBase currentState = this._currentState;
				flag2 = (currentState != null && currentState.StateType == stateType);
			}
			else
			{
				flag2 = true;
			}
			bool flag3 = flag2;
			if (!flag3)
			{
				CombatCharacterStateBase lastState = this._currentState;
				this._combatChar.MoveData.Reset();
				CombatCharacterStateBase currentState2 = this._currentState;
				if (currentState2 != null)
				{
					currentState2.OnExit();
				}
				this._currentState = this._allStates[stateType];
				bool flag4 = this._currentCombatDomain.Pause != this._currentState.IsUpdateOnPause;
				if (flag4)
				{
					DataContext context = this._combatChar.GetDataContext();
					this._currentCombatDomain.EnsurePauseState();
					bool flag5 = this._currentCombatDomain.IsInCombat();
					if (flag5)
					{
						this._currentCombatDomain.UpdateAllTeammateCommandUsable(context, true, ETeammateCommandImplement.Fight);
						this._currentCombatDomain.UpdateAllTeammateCommandUsable(context, false, ETeammateCommandImplement.Fight);
					}
				}
				this._currentState.OnEnter();
				bool flag6 = this._currentCombatDomain.IsInCombat();
				if (flag6)
				{
					DataContext context2 = this._combatChar.GetDataContext();
					bool flag7 = lastState != null && lastState.StateType != CombatCharacterStateType.Idle && stateType == CombatCharacterStateType.Idle;
					if (flag7)
					{
						this._currentCombatDomain.UpdateAllCommandAvailability(context2, this._combatChar);
					}
					else
					{
						bool flag8 = stateType == CombatCharacterStateType.PrepareSkill;
						if (flag8)
						{
							this._currentCombatDomain.UpdateAllTeammateCommandUsable(context2, this._combatChar.IsAlly, -1);
						}
					}
				}
			}
		}

		// Token: 0x0600620B RID: 25099 RVA: 0x0037C1F8 File Offset: 0x0037A3F8
		public void RegisterState(CombatCharacterStateBase state)
		{
			bool flag = state != null && !this._allStates.ContainsKey(state.StateType);
			if (flag)
			{
				this._allStates.Add(state.StateType, state);
			}
		}

		// Token: 0x0600620C RID: 25100 RVA: 0x0037C238 File Offset: 0x0037A438
		public CombatCharacterStateType GetProperState()
		{
			bool flag = !this._currentCombatDomain.IsInCombat();
			CombatCharacterStateType result;
			if (flag)
			{
				result = CombatCharacterStateType.Invalid;
			}
			else
			{
				bool flag2 = this._combatChar.NeedChangeBossPhase && this._currentCombatDomain.GetCombatCharacter(!this._combatChar.IsAlly, false).ChangeCharId < 0;
				if (flag2)
				{
					result = CombatCharacterStateType.ChangeBossPhase;
				}
				else
				{
					bool needSelectMercyOption = this._combatChar.NeedSelectMercyOption;
					if (needSelectMercyOption)
					{
						result = CombatCharacterStateType.SelectMercy;
					}
					else
					{
						bool needDelaySettlement = this._combatChar.NeedDelaySettlement;
						if (needDelaySettlement)
						{
							result = CombatCharacterStateType.DelaySettlement;
						}
						else
						{
							bool anyRawCreate = this._combatChar.AnyRawCreate;
							if (anyRawCreate)
							{
								result = CombatCharacterStateType.RawCreate;
							}
							else
							{
								bool needUnlockAttack = this._combatChar.NeedUnlockAttack;
								if (needUnlockAttack)
								{
									result = CombatCharacterStateType.UnlockAttack;
								}
								else
								{
									bool flag3 = this._combatChar.GetCombatReserveData().NeedUnlockWeaponIndex >= 0;
									if (flag3)
									{
										result = CombatCharacterStateType.PrepareUnlockAttack;
									}
									else
									{
										bool needBreakAttack = this._combatChar.NeedBreakAttack;
										if (needBreakAttack)
										{
											result = CombatCharacterStateType.BreakAttack;
										}
										else
										{
											bool needNormalAttack = this._combatChar.NeedNormalAttack;
											if (needNormalAttack)
											{
												result = CombatCharacterStateType.PrepareAttack;
											}
											else
											{
												bool flag4 = this._combatChar.GetPreparingSkillId() >= 0 || this._combatChar.NeedUseSkillFreeId >= 0 || (this._combatChar.NeedUseSkillId >= 0 && (this._combatChar.GetAffectingDefendSkillId() < 0 || DomainManager.SpecialEffect.ModifyData(this._combatChar.GetId(), this._combatChar.NeedUseSkillId, 223, false, -1, -1, -1)));
												if (flag4)
												{
													result = CombatCharacterStateType.PrepareSkill;
												}
												else
												{
													bool flag5 = this._combatChar.NeedShowChangeTrick && !this._combatChar.PreparingOrDoingTeammateCommand();
													if (flag5)
													{
														result = CombatCharacterStateType.SelectChangeTrick;
													}
													else
													{
														bool needAnimalAttack = this._combatChar.NeedAnimalAttack;
														if (needAnimalAttack)
														{
															result = CombatCharacterStateType.AnimalAttack;
														}
														else
														{
															bool flag6 = this._combatChar.GetPreparingOtherAction() >= 0 || this._combatChar.NeedUseOtherAction != -1;
															if (flag6)
															{
																result = CombatCharacterStateType.PrepareOtherAction;
															}
															else
															{
																bool flag7 = this._combatChar.NeedUseItem.IsValid() || this._combatChar.GetPreparingItem().IsValid();
																if (flag7)
																{
																	result = CombatCharacterStateType.PrepareUseItem;
																}
																else
																{
																	bool needPauseJumpMove = this._combatChar.NeedPauseJumpMove;
																	if (needPauseJumpMove)
																	{
																		result = CombatCharacterStateType.JumpMove;
																	}
																	else
																	{
																		bool needEnterSpecialShow = this._combatChar.NeedEnterSpecialShow;
																		if (needEnterSpecialShow)
																		{
																			result = CombatCharacterStateType.SpecialShow;
																		}
																		else
																		{
																			result = CombatCharacterStateType.Idle;
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04001AAE RID: 6830
		private Dictionary<CombatCharacterStateType, CombatCharacterStateBase> _allStates = new Dictionary<CombatCharacterStateType, CombatCharacterStateBase>();

		// Token: 0x04001AAF RID: 6831
		private CombatCharacterStateBase _currentState;

		// Token: 0x04001AB0 RID: 6832
		private CombatDomain _currentCombatDomain;

		// Token: 0x04001AB1 RID: 6833
		private CombatCharacter _combatChar;

		// Token: 0x04001AB2 RID: 6834
		private ulong _lastUpdateCombatFrame;
	}
}
