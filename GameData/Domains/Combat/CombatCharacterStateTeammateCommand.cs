using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x020006A7 RID: 1703
	public class CombatCharacterStateTeammateCommand : CombatCharacterStateBase
	{
		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06006243 RID: 25155 RVA: 0x0037F020 File Offset: 0x0037D220
		private CValuePercentBonus CmdEffectPercent
		{
			get
			{
				return DomainManager.SpecialEffect.GetModifyValue(this.CombatChar.GetId(), 184, EDataModifyType.Add, (int)this._commandImplement, -1, -1, EDataSumType.All);
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06006244 RID: 25156 RVA: 0x0037F04B File Offset: 0x0037D24B
		private bool Preparing
		{
			get
			{
				return this._teammateChar.TeammateCommandLeftPrepareFrame > 0;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06006245 RID: 25157 RVA: 0x0037F05B File Offset: 0x0037D25B
		private bool NeedPrepare
		{
			get
			{
				return this._commandConfig.PrepareFrame > 0;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06006246 RID: 25158 RVA: 0x0037F06B File Offset: 0x0037D26B
		private bool TeammateBeforeMainChar
		{
			get
			{
				return this._commandConfig.PosOffset > 0;
			}
		}

		// Token: 0x06006247 RID: 25159 RVA: 0x0037F07B File Offset: 0x0037D27B
		public CombatCharacterStateTeammateCommand(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.TeammateCommand)
		{
			this.IsUpdateOnPause = true;
		}

		// Token: 0x06006248 RID: 25160 RVA: 0x0037F090 File Offset: 0x0037D290
		public override void OnEnter()
		{
			this.OnEnterInitFields();
			DataContext context = this._teammateChar.GetDataContext();
			bool preparing = this.Preparing;
			if (preparing)
			{
				this.OnEnterPreparing(context);
			}
			else
			{
				bool needPrepare = this.NeedPrepare;
				if (needPrepare)
				{
					this.OnEnterPrepare(context);
				}
				else
				{
					this.OnEnterNoPrepare(context);
				}
			}
		}

		// Token: 0x06006249 RID: 25161 RVA: 0x0037F0E4 File Offset: 0x0037D2E4
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
				bool flag2 = this._foreCharAniStartFrame > 0;
				if (flag2)
				{
					this._foreCharAniStartFrame--;
					bool flag3 = this._foreCharAniStartFrame == 0;
					if (flag3)
					{
						this.OnForeCharAniStart();
					}
				}
				bool flag4 = this._applyLogicEffectFrame > 0;
				if (flag4)
				{
					this._applyLogicEffectFrame--;
					bool flag5 = this._applyLogicEffectFrame == 0;
					if (flag5)
					{
						this.ApplyLogicEffect();
					}
				}
				bool flag6 = this._teammateFallBackFrame > 0;
				if (flag6)
				{
					this._teammateFallBackFrame--;
					bool flag7 = this._teammateFallBackFrame == 0;
					if (flag7)
					{
						this.OnTeammateFallBack();
					}
				}
				bool flag8 = this._clearCmdFrame > 0;
				if (flag8)
				{
					this._clearCmdFrame--;
					bool flag9 = this._clearCmdFrame == 0;
					if (flag9)
					{
						this.OnClearCmd();
					}
				}
				bool flag10 = this._stateLeftFrame > 0;
				if (flag10)
				{
					this._stateLeftFrame--;
					bool flag11 = this._stateLeftFrame == 0;
					if (flag11)
					{
						this.OnStateLeft();
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600624A RID: 25162 RVA: 0x0037F210 File Offset: 0x0037D410
		private void OnEnterInitFields()
		{
			this._teammateChar = this.CombatChar.ActingTeammateCommandChar;
			this._commandType = this._teammateChar.GetExecutingTeammateCommand();
			this._commandConfig = TeammateCommand.Instance[this._commandType];
			this._commandImplement = this._commandConfig.Implement;
			this._backCharAni = (this.Preparing ? this._commandConfig.BackCharEnterAni : null);
			this._foreCharAni = this._commandConfig.ForeCharAni1;
			this._foreCharParticle = null;
			this._foreCharSound = null;
			this._foreCharAniStartFrame = 0;
			this._applyLogicEffectFrame = 0;
			this._teammateFallBackFrame = 0;
			this._clearCmdFrame = 0;
			this._stateLeftFrame = (this.Preparing ? AnimDataCollection.GetDurationFrame(this._commandConfig.BackCharEnterAni) : 48);
			bool flag = CombatCharacterStateTeammateCommand.NeedPostfixAnis.Contains(this._foreCharAni);
			if (flag)
			{
				CombatWeaponData weaponData = (this.TeammateBeforeMainChar ? this._teammateChar : this.CombatChar).GetWeaponData(-1);
				this._foreCharAni += weaponData.Template.TeammateCmdAniPostfix;
			}
			bool flag2 = (!this.NeedPrepare || this.Preparing) && !this.TeammateBeforeMainChar;
			if (flag2)
			{
				bool flag3 = this.Preparing || this._commandConfig.ForeCharAniUseHit;
				if (flag3)
				{
					this._foreCharAniStartFrame = AnimDataCollection.GetEventFrame(this._commandConfig.BackCharEnterAni, "hit", 0);
				}
				bool flag4 = !string.IsNullOrEmpty(this._foreCharAni) && !this.NeedPrepare;
				if (flag4)
				{
					this._teammateFallBackFrame = AnimDataCollection.GetEventFrame(this._commandConfig.BackCharEnterAni, "move", 1);
				}
			}
		}

		// Token: 0x0600624B RID: 25163 RVA: 0x0037F3C4 File Offset: 0x0037D5C4
		private void OnEnterPreparing(DataContext context)
		{
			this._teammateChar.SetAnimationToPlayOnce(this.TeammateBeforeMainChar ? this._foreCharAni : this._backCharAni, context);
			this._teammateChar.SetAnimationToLoop(this.TeammateBeforeMainChar ? this._commandConfig.ForeCharAni2 : this._commandConfig.BackCharPrepareAni, context);
			bool flag = !this.TeammateBeforeMainChar;
			if (!flag)
			{
				this._teammateChar.SpecialAnimationLoop = this._foreCharAni;
				this.CombatChar.SetAnimationToPlayOnce(this._backCharAni, context);
				this.CombatChar.SetAnimationToLoop(this._commandConfig.BackCharPrepareAni, context);
				this.CombatChar.SpecialAnimationLoop = this._commandConfig.BackCharPrepareAni;
			}
		}

		// Token: 0x0600624C RID: 25164 RVA: 0x0037F484 File Offset: 0x0037D684
		private void OnEnterPrepare(DataContext context)
		{
			bool teammateBeforeMainChar = this.TeammateBeforeMainChar;
			if (teammateBeforeMainChar)
			{
				this.CombatChar.SpecialAnimationLoop = null;
				this.CombatChar.SetAnimationToPlayOnce(this._commandConfig.BackCharExitAni, context);
				this.CombatChar.SetAnimationToLoop(this.CurrentCombatDomain.GetIdleAni(this.CombatChar), context);
				this._teammateChar.SpecialAnimationLoop = null;
				this._teammateChar.SetAnimationToPlayOnce(this._commandConfig.ForeCharAni3, context);
				this._teammateFallBackFrame = 1;
				this._stateLeftFrame = (this._clearCmdFrame = AnimDataCollection.GetDurationFrame(this._commandConfig.ForeCharAni3));
			}
			else
			{
				this._teammateChar.ClearTeammateCommand(context, false);
				this._stateLeftFrame = 48;
			}
			this.ApplyLogicEffect();
		}

		// Token: 0x0600624D RID: 25165 RVA: 0x0037F550 File Offset: 0x0037D750
		private void OnEnterNoPrepare(DataContext context)
		{
			bool flag = this._commandImplement.IsPushOrPull();
			if (flag)
			{
				this.OnEnterPushOrPull(context);
			}
			else
			{
				bool flag2 = this._commandConfig.Type == ETeammateCommandType.Negative;
				if (flag2)
				{
					this.OnEnterNegative(context);
				}
				else
				{
					bool flag3 = this._commandImplement.IsAttack();
					if (flag3)
					{
						this.OnEnterAttack(context);
					}
					else
					{
						bool flag4 = this._commandImplement.IsDefend();
						if (flag4)
						{
							this.OnEnterDefend(context);
						}
					}
				}
			}
		}

		// Token: 0x0600624E RID: 25166 RVA: 0x0037F5C4 File Offset: 0x0037D7C4
		private void OnEnterPushOrPull(DataContext context)
		{
			this._teammateChar.SetAnimationToPlayOnce(this._commandConfig.BackCharEnterAni, context);
			this._teammateChar.SetParticleToPlay(this._commandConfig.BackCharParticle, context);
			this._applyLogicEffectFrame = this._foreCharAniStartFrame + AnimDataCollection.GetEventFrame(this._foreCharAni, "act0", 0);
			this._stateLeftFrame = AnimDataCollection.GetDurationFrame(this._commandConfig.BackCharEnterAni);
			string sound = this._commandConfig.BackCharEnterSound;
			AnimalItem animalConfig = this._teammateChar.AnimalConfig;
			bool flag = ((animalConfig != null) ? animalConfig.TeammateCommandBackCharEnterSound : null) != null;
			if (flag)
			{
				sbyte cmdType = this._teammateChar.GetExecutingTeammateCommand();
				int index = this._teammateChar.GetCurrTeammateCommands().IndexOf(cmdType);
				bool flag2 = index >= 0 && index < this._teammateChar.AnimalConfig.TeammateCommandBackCharEnterSound.Count;
				if (flag2)
				{
					sound = this._teammateChar.AnimalConfig.TeammateCommandBackCharEnterSound[index];
				}
			}
			this._teammateChar.SetSkillSoundToPlay(sound, context);
		}

		// Token: 0x0600624F RID: 25167 RVA: 0x0037F6CC File Offset: 0x0037D8CC
		private void OnEnterNegative(DataContext context)
		{
			this._teammateChar.SetAnimationToPlayOnce(this._commandConfig.BackCharEnterAni, context);
			this._teammateChar.SetParticleToPlay(this._commandConfig.BackCharParticle, context);
			this._applyLogicEffectFrame = AnimDataCollection.GetEventFrame(this._commandConfig.BackCharEnterAni, "act0", 0);
			this._stateLeftFrame = AnimDataCollection.GetDurationFrame(this._commandConfig.BackCharEnterAni);
			this._teammateChar.SetSkillSoundToPlay(this._commandConfig.BackCharEnterSound, context);
		}

		// Token: 0x06006250 RID: 25168 RVA: 0x0037F754 File Offset: 0x0037D954
		private void OnEnterAttack(DataContext context)
		{
			sbyte trickType = this._teammateChar.GetAttackCommandTrickType();
			int displayPos = this.CurrentCombatDomain.GetDisplayPosition(this.CombatChar.IsAlly, (short)this._teammateChar.GetNormalAttackPosition(trickType));
			this._foreCharAni = this._teammateChar.GetNormalAttackAnimation(trickType);
			this._foreCharParticle = this._teammateChar.GetNormalAttackParticle(trickType);
			this._foreCharSound = this._teammateChar.GetNormalAttackSound(trickType);
			this._foreCharAniStartFrame = 34;
			string foreCharAniFull = this._teammateChar.GetNormalAttackAnimationFull(this._foreCharAni);
			this._applyLogicEffectFrame = this._foreCharAniStartFrame + AnimDataCollection.GetEventFrame(foreCharAniFull, "act0", 0);
			this._teammateFallBackFrame = 34 + AnimDataCollection.GetDurationFrame(foreCharAniFull);
			this._stateLeftFrame = (this._clearCmdFrame = this._teammateFallBackFrame + 48);
			this._teammateChar.SetDisplayPosition(displayPos, context);
			this._teammateChar.SetAnimationToPlayOnce("M_003", context);
		}

		// Token: 0x06006251 RID: 25169 RVA: 0x0037F844 File Offset: 0x0037DA44
		private void OnEnterDefend(DataContext context)
		{
			this._applyLogicEffectFrame = (this._stateLeftFrame = 34);
			this._teammateChar.SetAnimationToPlayOnce("M_003", context);
		}

		// Token: 0x06006252 RID: 25170 RVA: 0x0037F878 File Offset: 0x0037DA78
		private void ApplyLogicEffect()
		{
			DataContext context = this.CombatChar.GetDataContext();
			ETeammateCommandImplement implement = TeammateCommand.Instance[this._commandType].Implement;
			bool flag = this._commandConfig.PosOffset < 0;
			if (flag)
			{
				this._teammateChar.SetParticleToLoop(null, context);
			}
			else
			{
				this.CombatChar.SetParticleToLoop(null, context);
			}
			this._teammateChar.SetSoundToLoop(null, context);
			this.CombatChar.SetSoundToLoop(null, context);
			bool flag2 = implement == ETeammateCommandImplement.AccelerateCast;
			if (flag2)
			{
				this.ApplyAccelerateCast(context);
			}
			else
			{
				bool flag3 = implement - ETeammateCommandImplement.Push <= 1;
				bool flag4 = flag3;
				if (flag4)
				{
					this.ApplyPushOrPull(context);
				}
				else
				{
					bool flag5 = implement == ETeammateCommandImplement.PushOrPullIntoDanger;
					if (flag5)
					{
						this.ApplyPushOrPullIntoDanger(context);
					}
					else
					{
						bool flag6 = implement.IsAttack();
						if (flag6)
						{
							this.ApplyAttack();
						}
						else
						{
							bool flag7 = implement.IsDefend();
							if (flag7)
							{
								this.ApplyDefend(context);
							}
							else
							{
								bool flag8 = implement == ETeammateCommandImplement.HealInjury;
								if (flag8)
								{
									this.ApplyHealInjury(context);
								}
								else
								{
									bool flag9 = implement == ETeammateCommandImplement.HealPoison;
									if (flag9)
									{
										this.ApplyHealPoison(context);
									}
									else
									{
										bool flag10 = implement == ETeammateCommandImplement.HealFlaw;
										if (flag10)
										{
											this.ApplyHealFlaw(context);
										}
										else
										{
											bool flag11 = implement == ETeammateCommandImplement.HealAcupoint;
											if (flag11)
											{
												this.ApplyHealAcupoint(context);
											}
											else
											{
												bool flag12 = implement == ETeammateCommandImplement.TransferNeiliAllocation;
												if (flag12)
												{
													this.ApplyTransferNeiliAllocation(context);
												}
												else
												{
													bool flag13 = implement == ETeammateCommandImplement.TransferInjury;
													if (flag13)
													{
														this.ApplyTransferInjury(context);
													}
													else
													{
														bool flag14 = implement == ETeammateCommandImplement.InterruptSkill;
														if (flag14)
														{
															this.ApplyInterruptSkill(context);
														}
														else
														{
															bool flag15 = implement == ETeammateCommandImplement.AttackFlawAndAcupoint;
															if (flag15)
															{
																this.ApplyAttackFlawAndAcupoint(context);
															}
															else
															{
																bool flag16 = implement == ETeammateCommandImplement.ClearAgileAndDefense;
																if (flag16)
																{
																	this.ApplyClearAgileAndDefense(context);
																}
																else
																{
																	bool flag17 = implement == ETeammateCommandImplement.AddInjuryAndPoison;
																	if (flag17)
																	{
																		this.ApplyAddInjuryAndPoison(context);
																	}
																	else
																	{
																		bool flag18 = implement == ETeammateCommandImplement.InterruptOtherAction;
																		if (flag18)
																		{
																			this.ApplyInterruptOtherAction(context);
																		}
																		else
																		{
																			bool flag19 = implement == ETeammateCommandImplement.ReduceNeiliAllocation;
																			if (flag19)
																			{
																				this.ApplyReduceNeiliAllocation(context);
																			}
																			else
																			{
																				bool flag20 = implement == ETeammateCommandImplement.AddUnlockAttackValue;
																				if (flag20)
																				{
																					this.ApplyAddUnlockAttackValue(context);
																				}
																				else
																				{
																					bool flag21 = implement == ETeammateCommandImplement.TransferManyMark;
																					if (flag21)
																					{
																						this.ApplyTransferManyMark(context);
																					}
																					else
																					{
																						bool flag22 = implement == ETeammateCommandImplement.RepairItem;
																						if (flag22)
																						{
																							this.ApplyRepairItem(context);
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
						}
					}
				}
			}
		}

		// Token: 0x06006253 RID: 25171 RVA: 0x0037FAC8 File Offset: 0x0037DCC8
		private void ApplyAccelerateCast(DataContext context)
		{
			bool flag = this.CombatChar.GetPreparingSkillId() >= 0;
			if (flag)
			{
				int addValue = this.CombatChar.SkillPrepareTotalProgress * this._commandConfig.IntArg / 100;
				addValue *= this.CmdEffectPercent;
				this.CombatChar.SkillPrepareCurrProgress = Math.Min(this.CombatChar.SkillPrepareCurrProgress + addValue, this.CombatChar.SkillPrepareTotalProgress);
				this.CombatChar.SetSkillPreparePercent((byte)(this.CombatChar.SkillPrepareCurrProgress * 100 / this.CombatChar.SkillPrepareTotalProgress), context);
			}
		}

		// Token: 0x06006254 RID: 25172 RVA: 0x0037FB65 File Offset: 0x0037DD65
		private void ApplyPushOrPull(DataContext context)
		{
			this.CurrentCombatDomain.ChangeDistance(context, this.CombatChar, this._commandConfig.IntArg * this.CmdEffectPercent);
		}

		// Token: 0x06006255 RID: 25173 RVA: 0x0037FB94 File Offset: 0x0037DD94
		private void ApplyPushOrPullIntoDanger(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!this.CombatChar.IsAlly, false);
			short distance = DomainManager.Combat.GetCurrentDistance();
			OuterAndInnerShorts attackRange = enemyChar.GetAttackRange();
			int delta = (distance > attackRange.Inner) ? -10 : 10;
			this.CurrentCombatDomain.ChangeDistance(context, this.CombatChar, delta);
		}

		// Token: 0x06006256 RID: 25174 RVA: 0x0037FBF4 File Offset: 0x0037DDF4
		private void ApplyAttack()
		{
			CombatCharacter enemyChar = this.CurrentCombatDomain.GetCombatCharacter(!this.CombatChar.IsAlly, true);
			this.ApplyAttack(enemyChar);
		}

		// Token: 0x06006257 RID: 25175 RVA: 0x0037FC28 File Offset: 0x0037DE28
		private void ApplyAttack(CombatCharacter enemyChar)
		{
			CombatContext context = CombatContext.Create(this._teammateChar, enemyChar, -1, -1, -1, null);
			sbyte trickType = this._teammateChar.GetAttackCommandTrickType();
			this._teammateChar.NormalAttackHitType = this.CurrentCombatDomain.GetAttackHitType(this._teammateChar, trickType);
			this._teammateChar.NormalAttackBodyPart = this.CurrentCombatDomain.GetAttackBodyPart(this._teammateChar, enemyChar, context.Random, -1, trickType, -1);
			this.CurrentCombatDomain.UpdateDamageCompareData(context);
			this.CurrentCombatDomain.CalcNormalAttack(context, trickType);
			Events.RaiseNormalAttackAllEnd(context, this._teammateChar, enemyChar);
			this._teammateChar.FinishFreeAttack();
		}

		// Token: 0x06006258 RID: 25176 RVA: 0x0037FCDC File Offset: 0x0037DEDC
		private void ApplyDefend(DataContext context)
		{
			short defendSkillId = this._teammateChar.GetDefendCommandSkillId();
			GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new ValueTuple<int, short>(this._teammateChar.GetId(), defendSkillId));
			CombatSkillItem skillConfig = Config.CombatSkill.Instance[defendSkillId];
			this._teammateChar.SetAffectingDefendSkillId(defendSkillId, context);
			this._teammateChar.SetAnimationToLoop(skillConfig.DefendAnimation, context);
			short defendFrames = CombatSkillDomain.CalcContinuousFrames(skill);
			int defendFramesInt = (int)defendFrames * this._commandConfig.DefendSkillDurationPercent;
			defendFrames = (short)Math.Clamp(defendFramesInt, 1, 32767);
			this._teammateChar.TeammateCommandLeftFrame = (this._teammateChar.TeammateCommandTotalFrame = defendFrames);
			this.CurrentCombatDomain.UpdateMaxSkillGrade(this._teammateChar.IsAlly, defendSkillId);
			DomainManager.SpecialEffect.Add(context, this._teammateChar.GetId(), defendSkillId, 0, -1);
		}

		// Token: 0x06006259 RID: 25177 RVA: 0x0037FDC0 File Offset: 0x0037DFC0
		private void ApplyHealInjury(DataContext context)
		{
			this._teammateChar.SetHealInjuryCount((byte)Math.Max((int)(this._teammateChar.GetHealInjuryCount() - 1), 0), context);
			DomainManager.Character.UseCombatResources(context, this._teammateChar.GetId(), EHealActionType.Healing, 1);
			this.CurrentCombatDomain.HealInjuryInCombat(context, this.CombatChar, this._teammateChar, true);
		}

		// Token: 0x0600625A RID: 25178 RVA: 0x0037FE24 File Offset: 0x0037E024
		private void ApplyHealPoison(DataContext context)
		{
			this._teammateChar.SetHealPoisonCount((byte)Math.Max((int)(this._teammateChar.GetHealPoisonCount() - 1), 0), context);
			DomainManager.Character.UseCombatResources(context, this._teammateChar.GetId(), EHealActionType.Detox, 1);
			this.CurrentCombatDomain.HealPoisonInCombat(context, this.CombatChar, this._teammateChar, true);
		}

		// Token: 0x0600625B RID: 25179 RVA: 0x0037FE86 File Offset: 0x0037E086
		private void ApplyHealFlaw(DataContext context)
		{
			this.CombatChar.RemoveRandomFlawOrAcupoint(context, true, this._commandConfig.IntArg * this.CmdEffectPercent);
		}

		// Token: 0x0600625C RID: 25180 RVA: 0x0037FEAD File Offset: 0x0037E0AD
		private void ApplyHealAcupoint(DataContext context)
		{
			this.CombatChar.RemoveRandomFlawOrAcupoint(context, false, this._commandConfig.IntArg * this.CmdEffectPercent);
		}

		// Token: 0x0600625D RID: 25181 RVA: 0x0037FED4 File Offset: 0x0037E0D4
		private unsafe void ApplyTransferNeiliAllocation(DataContext context)
		{
			NeiliAllocation teammateNeiliAllocation = this._teammateChar.GetNeiliAllocation();
			CValuePercentBonus cmdEffectPercent = DomainManager.SpecialEffect.GetModifyValue(this.CombatChar.GetId(), 184, EDataModifyType.Add, (int)this._commandImplement, -1, -1, EDataSumType.All);
			for (byte type = 0; type < 4; type += 1)
			{
				bool flag = *(ref teammateNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2) > 0;
				if (flag)
				{
					int transferValue = Math.Min((int)(*(ref teammateNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2)), this._commandConfig.IntArg * cmdEffectPercent);
					this._teammateChar.ChangeNeiliAllocation(context, type, -transferValue, false, true);
					this.CombatChar.ChangeNeiliAllocation(context, type, transferValue, false, true);
				}
			}
		}

		// Token: 0x0600625E RID: 25182 RVA: 0x0037FF98 File Offset: 0x0037E198
		private void ApplyTransferInjury(DataContext context)
		{
			Injuries mainCharInjuries = this.CombatChar.GetInjuries();
			Injuries newInjuries = mainCharInjuries.Subtract(this.CombatChar.GetOldInjuries());
			Injuries teammateInjuries = this._teammateChar.GetInjuries();
			bool inner = this.CombatChar.TransferInjuryCommandIsInner;
			List<sbyte> bodyPartRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
			bodyPartRandomPool.Clear();
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				int canTransferValue = Math.Min((int)newInjuries.Get(bodyPart, inner), (int)(6 - teammateInjuries.Get(bodyPart, inner)));
				for (int i = 0; i < canTransferValue; i++)
				{
					bodyPartRandomPool.Add(bodyPart);
				}
			}
			int transferCount = Math.Min(this._commandConfig.IntArg, bodyPartRandomPool.Count);
			for (int j = 0; j < transferCount; j++)
			{
				sbyte bodyPart2 = bodyPartRandomPool[context.Random.Next(0, bodyPartRandomPool.Count)];
				bodyPartRandomPool.Remove(bodyPart2);
				mainCharInjuries.Change(bodyPart2, inner, -1);
				this.CurrentCombatDomain.AddInjury(context, this._teammateChar, bodyPart2, inner, 1, false, false);
			}
			ObjectPool<List<sbyte>>.Instance.Return(bodyPartRandomPool);
			this.CurrentCombatDomain.SetInjuries(context, this.CombatChar, mainCharInjuries, true, true);
			this.CurrentCombatDomain.UpdateBodyDefeatMark(context, this._teammateChar);
			this.CurrentCombatDomain.TransferFatalDamageMark(context, this.CombatChar, this._teammateChar, this._commandConfig.IntArg);
		}

		// Token: 0x0600625F RID: 25183 RVA: 0x0038011F File Offset: 0x0037E31F
		private void ApplyInterruptSkill(DataContext context)
		{
			DomainManager.Combat.InterruptSkill(context, this.CombatChar, 100);
			DomainManager.Combat.SetProperLoopAniAndParticle(context, this.CombatChar, false);
		}

		// Token: 0x06006260 RID: 25184 RVA: 0x0038014C File Offset: 0x0037E34C
		private void ApplyAttackFlawAndAcupoint(DataContext context)
		{
			this.ApplyAttack(this.CombatChar);
			bool flaw = context.Random.CheckPercentProb(50);
			for (sbyte i = 0; i <= 2; i += 1)
			{
				bool flag = flaw;
				if (flag)
				{
					DomainManager.Combat.AddFlaw(context, this.CombatChar, i, CombatSkillKey.Invalid, -1, 1, true);
				}
				else
				{
					DomainManager.Combat.AddAcupoint(context, this.CombatChar, i, CombatSkillKey.Invalid, -1, 1, true);
				}
			}
		}

		// Token: 0x06006261 RID: 25185 RVA: 0x003801C8 File Offset: 0x0037E3C8
		private void ApplyClearAgileAndDefense(DataContext context)
		{
			DomainManager.Combat.ClearAffectingAgileSkill(context, this.CombatChar);
			DomainManager.Combat.ClearAffectingDefenseSkill(context, this.CombatChar);
		}

		// Token: 0x06006262 RID: 25186 RVA: 0x003801F0 File Offset: 0x0037E3F0
		private void ApplyAddInjuryAndPoison(DataContext context)
		{
			GameData.Domains.Character.Character selfChar = this._teammateChar.GetCharacter();
			GameData.Domains.Character.Character targetChar = this.CombatChar.GetCharacter();
			sbyte poisonActionPhase = selfChar.GetPoisonActionPhase(context.Random, targetChar, 100, false);
			bool flag = poisonActionPhase > 3;
			if (flag)
			{
				DomainManager.Character.ApplyPoisonActionEffect(context, selfChar, targetChar, ItemKey.Invalid);
			}
			sbyte plotHarmActionPhase = selfChar.GetPlotHarmActionPhase(context.Random, targetChar, 100, false);
			bool flag2 = plotHarmActionPhase > 3;
			if (flag2)
			{
				DomainManager.Character.ApplyPlotHarmActionEffect(context, selfChar, targetChar, ItemKey.Invalid);
			}
		}

		// Token: 0x06006263 RID: 25187 RVA: 0x00380272 File Offset: 0x0037E472
		private void ApplyInterruptOtherAction(DataContext context)
		{
			DomainManager.Combat.InterruptOtherAction(context, this.CombatChar);
			this.CombatChar.SetPreparingItem(ItemKey.Invalid, context);
			DomainManager.Combat.SetProperLoopAniAndParticle(context, this.CombatChar, false);
		}

		// Token: 0x06006264 RID: 25188 RVA: 0x003802AC File Offset: 0x0037E4AC
		private void ApplyReduceNeiliAllocation(DataContext context)
		{
			int value = this._teammateChar.ExecutingTeammateCommandConfig.IntArg;
			for (byte i = 0; i < 4; i += 1)
			{
				this.CombatChar.ChangeNeiliAllocation(context, i, -value, true, true);
			}
		}

		// Token: 0x06006265 RID: 25189 RVA: 0x003802F0 File Offset: 0x0037E4F0
		private void ApplyAddUnlockAttackValue(DataContext context)
		{
			int addValue = GlobalConfig.Instance.UnlockAttackUnit * this._commandConfig.IntArg;
			this.CombatChar.ChangeUnlockAttackValue(context, this.CombatChar.GetUsingWeaponIndex(), addValue);
		}

		// Token: 0x06006266 RID: 25190 RVA: 0x00380337 File Offset: 0x0037E537
		private void ApplyTransferManyMark(DataContext context)
		{
			this.ApplyTransferManyMarkInjury(context);
			this.ApplyTransferManyMarkPoison(context);
			this.ApplyTransferManyMarkQiDisorder(context);
		}

		// Token: 0x06006267 RID: 25191 RVA: 0x00380354 File Offset: 0x0037E554
		private int ApplyTransferManyMarkDiv(int value, int canTransfer)
		{
			int divisor = Math.Max(this._commandConfig.IntArg, 1);
			int maxTransfer = value / divisor + ((value % divisor > 0) ? 1 : 0);
			return Math.Min(maxTransfer, canTransfer);
		}

		// Token: 0x06006268 RID: 25192 RVA: 0x00380390 File Offset: 0x0037E590
		private void ApplyTransferManyMarkInjury(DataContext context)
		{
			CombatCharacterStateTeammateCommand.<>c__DisplayClass55_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.mainCharInjuries = this.CombatChar.GetInjuries();
			CS$<>8__locals1.mainCharNewInjuries = CS$<>8__locals1.mainCharInjuries.Subtract(this.CombatChar.GetOldInjuries());
			CS$<>8__locals1.teammateInjuries = this._teammateChar.GetInjuries();
			bool anyInjuryChanged = false;
			CombatCharacterStateTeammateCommand.<>c__DisplayClass55_1 CS$<>8__locals2;
			CS$<>8__locals2.i = 0;
			while (CS$<>8__locals2.i < 7)
			{
				anyInjuryChanged = (this.<ApplyTransferManyMarkInjury>g__TryChangeInjuries|55_0(true, ref CS$<>8__locals1, ref CS$<>8__locals2) || anyInjuryChanged);
				anyInjuryChanged = (this.<ApplyTransferManyMarkInjury>g__TryChangeInjuries|55_0(false, ref CS$<>8__locals1, ref CS$<>8__locals2) || anyInjuryChanged);
				sbyte i = CS$<>8__locals2.i;
				CS$<>8__locals2.i = i + 1;
			}
			bool flag = anyInjuryChanged;
			if (flag)
			{
				this.CurrentCombatDomain.SetInjuries(CS$<>8__locals1.context, this.CombatChar, CS$<>8__locals1.mainCharInjuries, true, true);
				this.CurrentCombatDomain.UpdateBodyDefeatMark(CS$<>8__locals1.context, this._teammateChar);
			}
		}

		// Token: 0x06006269 RID: 25193 RVA: 0x00380480 File Offset: 0x0037E680
		private unsafe void ApplyTransferManyMarkPoison(DataContext context)
		{
			PoisonInts mainCharOldPoisons = *this.CombatChar.GetOldPoison();
			PoisonInts mainCharPoisons = *this.CombatChar.GetPoison();
			PoisonInts mainCharNewPoisons = mainCharPoisons.Subtract(ref mainCharOldPoisons);
			PoisonInts teammatePoisons = *this._teammateChar.GetPoison();
			bool anyPoisonChanged = false;
			for (int i = 0; i < 6; i++)
			{
				int newPoison = *mainCharPoisons[i];
				int canTransferPoison = 25000 - *teammatePoisons[i];
				int transferPoisonValue = this.ApplyTransferManyMarkDiv(newPoison, canTransferPoison);
				bool flag = transferPoisonValue <= 0;
				if (!flag)
				{
					*mainCharPoisons[i] -= transferPoisonValue;
					*mainCharNewPoisons[i] -= transferPoisonValue;
					*teammatePoisons[i] += transferPoisonValue;
					anyPoisonChanged = true;
				}
			}
			bool flag2 = anyPoisonChanged;
			if (flag2)
			{
				this.CurrentCombatDomain.SetPoisons(context, this.CombatChar, mainCharPoisons, true);
				this.CurrentCombatDomain.SetPoisons(context, this._teammateChar, teammatePoisons, true);
			}
		}

		// Token: 0x0600626A RID: 25194 RVA: 0x00380584 File Offset: 0x0037E784
		private void ApplyTransferManyMarkQiDisorder(DataContext context)
		{
			short mainCharQiDisorder = this.CombatChar.GetCharacter().GetDisorderOfQi();
			int mainCharNewQiDisorder = (int)(mainCharQiDisorder - this.CombatChar.GetOldDisorderOfQi());
			int canTransferQiDisorder = (int)(DisorderLevelOfQi.MaxValue - this._teammateChar.GetCharacter().GetDisorderOfQi());
			int transferQiDisorder = this.ApplyTransferManyMarkDiv(mainCharNewQiDisorder, canTransferQiDisorder);
			bool flag = transferQiDisorder > 0;
			if (flag)
			{
				this.CurrentCombatDomain.TransferDisorderOfQi(context, this.CombatChar, this._teammateChar, transferQiDisorder);
			}
		}

		// Token: 0x0600626B RID: 25195 RVA: 0x003805F8 File Offset: 0x0037E7F8
		private void ApplyRepairItem(DataContext context)
		{
			foreach (ItemKey equipKey in this.CombatChar.GetCharacter().GetEquipment())
			{
				int repairValue = this._teammateChar.CalcTeammateCommandRepairDurabilityValue(equipKey);
				bool flag = repairValue > 0;
				if (flag)
				{
					this.CurrentCombatDomain.ChangeDurability(context, this.CombatChar, equipKey, repairValue, EChangeDurabilitySourceType.Teammate);
				}
			}
		}

		// Token: 0x0600626C RID: 25196 RVA: 0x00380660 File Offset: 0x0037E860
		private void OnForeCharAniStart()
		{
			DataContext context = this.CombatChar.GetDataContext();
			bool flag = !string.IsNullOrEmpty(this._foreCharAni);
			if (flag)
			{
				CombatCharacter foreChar = (this._commandConfig.PosOffset > 0) ? this._teammateChar : this.CombatChar;
				foreChar.SetAnimationToPlayOnce(this._foreCharAni, context);
				bool flag2 = this._foreCharParticle != null;
				if (flag2)
				{
					foreChar.SetParticleToPlay(this._foreCharParticle, context);
				}
				bool flag3 = this._foreCharSound != null;
				if (flag3)
				{
					foreChar.SetAttackSoundToPlay(this._foreCharSound, context);
				}
			}
			bool flag4 = this._commandConfig.PosOffset < 0;
			if (flag4)
			{
				this._teammateChar.SetParticleToLoop(this._commandConfig.BackCharParticle, context);
				bool flag5 = !string.IsNullOrEmpty(this._commandConfig.BackCharPrepareSound);
				if (flag5)
				{
					this._teammateChar.SetSoundToLoop(this._commandConfig.BackCharPrepareSound, context);
				}
			}
		}

		// Token: 0x0600626D RID: 25197 RVA: 0x00380750 File Offset: 0x0037E950
		private void OnTeammateFallBack()
		{
			DataContext context = this.CombatChar.GetDataContext();
			bool teammateInFront = this.CombatChar.TeammateBeforeMainChar == this._teammateChar.GetId();
			this._teammateChar.SetDisplayPosition(int.MinValue, context);
			bool flag = teammateInFront;
			if (flag)
			{
				this.CombatChar.TeammateBeforeMainChar = -1;
			}
			else
			{
				this.CombatChar.TeammateAfterMainChar = -1;
			}
			this.CurrentCombatDomain.SetDisplayPosition(context, this.CombatChar.IsAlly, int.MinValue);
			bool flag2 = teammateInFront;
			if (flag2)
			{
				this.CombatChar.TeammateBeforeMainChar = this._teammateChar.GetId();
			}
			else
			{
				this.CombatChar.TeammateAfterMainChar = this._teammateChar.GetId();
			}
			bool flag3 = this._commandImplement.IsAttack();
			if (flag3)
			{
				this._teammateChar.SetAnimationToPlayOnce("M_004", context);
			}
		}

		// Token: 0x0600626E RID: 25198 RVA: 0x00380828 File Offset: 0x0037EA28
		private void OnClearCmd()
		{
			bool flag = this._commandImplement == ETeammateCommandImplement.GearMateA;
			if (flag)
			{
				this._teammateChar.PartlyClearTeammateCommand(this._teammateChar.GetDataContext(), false);
			}
			else
			{
				this._teammateChar.ClearTeammateCommand(this._teammateChar.GetDataContext(), false);
			}
		}

		// Token: 0x0600626F RID: 25199 RVA: 0x00380878 File Offset: 0x0037EA78
		private void OnStateLeft()
		{
			ETeammateCommandImplement commandImplement = this._commandImplement;
			bool flag = commandImplement - ETeammateCommandImplement.Push <= 1;
			bool flag2 = flag;
			if (flag2)
			{
				this._teammateChar.ClearTeammateCommand(this._teammateChar.GetDataContext(), false);
			}
			else
			{
				bool flag3 = this._commandImplement == ETeammateCommandImplement.TransferInjury && this._teammateChar.TeammateCommandLeftPrepareFrame > 0;
				if (flag3)
				{
					DataContext context = this.CombatChar.GetDataContext();
					this.CombatChar.SetParticleToLoop(this._commandConfig.BackCharParticle, this.CombatChar.GetDataContext());
					this.CombatChar.SetSoundToLoop(this._commandConfig.BackCharPrepareSound, context);
				}
			}
			this.CombatChar.StateMachine.TranslateState();
		}

		// Token: 0x06006271 RID: 25201 RVA: 0x00380968 File Offset: 0x0037EB68
		[CompilerGenerated]
		private bool <ApplyTransferManyMarkInjury>g__TryChangeInjuries|55_0(bool inner, ref CombatCharacterStateTeammateCommand.<>c__DisplayClass55_0 A_2, ref CombatCharacterStateTeammateCommand.<>c__DisplayClass55_1 A_3)
		{
			sbyte newInjury = A_2.mainCharNewInjuries.Get(A_3.i, inner);
			int canTransferInjury = (int)(6 - A_2.teammateInjuries.Get(A_3.i, inner));
			sbyte transferInjuryCount = (sbyte)this.ApplyTransferManyMarkDiv((int)newInjury, canTransferInjury);
			bool flag = transferInjuryCount <= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				A_2.mainCharInjuries.Change(A_3.i, inner, (int)(-transferInjuryCount));
				A_2.mainCharNewInjuries.Change(A_3.i, inner, (int)(-transferInjuryCount));
				this.CurrentCombatDomain.AddInjury(A_2.context, this._teammateChar, A_3.i, inner, transferInjuryCount, false, false);
				result = true;
			}
			return result;
		}

		// Token: 0x04001ABF RID: 6847
		private static readonly List<string> NeedPostfixAnis = new List<string>
		{
			"M_018",
			"M_019",
			"M_023"
		};

		// Token: 0x04001AC0 RID: 6848
		private CombatCharacter _teammateChar;

		// Token: 0x04001AC1 RID: 6849
		private sbyte _commandType;

		// Token: 0x04001AC2 RID: 6850
		private TeammateCommandItem _commandConfig;

		// Token: 0x04001AC3 RID: 6851
		private ETeammateCommandImplement _commandImplement;

		// Token: 0x04001AC4 RID: 6852
		private string _backCharAni;

		// Token: 0x04001AC5 RID: 6853
		private string _foreCharAni;

		// Token: 0x04001AC6 RID: 6854
		private string _foreCharParticle;

		// Token: 0x04001AC7 RID: 6855
		private string _foreCharSound;

		// Token: 0x04001AC8 RID: 6856
		private int _foreCharAniStartFrame;

		// Token: 0x04001AC9 RID: 6857
		private int _applyLogicEffectFrame;

		// Token: 0x04001ACA RID: 6858
		private int _teammateFallBackFrame;

		// Token: 0x04001ACB RID: 6859
		private int _clearCmdFrame;

		// Token: 0x04001ACC RID: 6860
		private int _stateLeftFrame;
	}
}
