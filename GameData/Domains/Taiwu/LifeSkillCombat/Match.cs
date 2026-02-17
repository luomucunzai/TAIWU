using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Information;
using GameData.Domains.Item.Display;
using GameData.Domains.Taiwu.LifeSkillCombat.Operation;
using GameData.Domains.Taiwu.LifeSkillCombat.Snapshot;
using GameData.Domains.Taiwu.LifeSkillCombat.Status;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Taiwu.LifeSkillCombat
{
	// Token: 0x02000058 RID: 88
	public class Match
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060014F5 RID: 5365 RVA: 0x00144B4C File Offset: 0x00142D4C
		// (set) Token: 0x060014F6 RID: 5366 RVA: 0x00144B54 File Offset: 0x00142D54
		public bool SuicideIsForced { get; private set; }

		// Token: 0x060014F7 RID: 5367 RVA: 0x00144B60 File Offset: 0x00142D60
		public Match(DataContext context, sbyte lifeSkillType, int characterIdA, int characterIdB, sbyte firstTurnPlayerId)
		{
			this.LifeSkillType = lifeSkillType;
			this._playerSelf = new Player(context, this, 0, characterIdA, lifeSkillType);
			this._playerAdversary = new Player(context, this, 1, characterIdB, lifeSkillType);
			bool flag = DomainManager.Character.GetElement_Objects(characterIdA).GetLeaderId() == DomainManager.Character.GetElement_Objects(characterIdB).GetLeaderId();
			if (flag)
			{
				this._playerSelf.DropAllUsableFriendlyCharacters();
				this._playerAdversary.DropAllUsableFriendlyCharacters();
			}
			this.Setup(firstTurnPlayerId);
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x00144C18 File Offset: 0x00142E18
		private void Setup(sbyte firstTurnPlayerId)
		{
			int i = 0;
			int len = 49;
			while (i < len)
			{
				this._gridStatus[i] = new Grid(i);
				i++;
			}
			this._suiciderPlayerId = null;
			this._currentPlayerId = firstTurnPlayerId;
			this._lastOperationIsSilent = false;
			this.PlayerSwitchCount = 0;
			this.SuicideIsForced = false;
			this.AcceptSilentPlayerId = -1;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
			defaultInterpolatedStringHandler.AppendLiteral("first turn started - House[");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this._currentPlayerId);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			this.RecordLine(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x00144CBC File Offset: 0x00142EBC
		public bool CheckResult(out sbyte winnerPlayerId)
		{
			bool flag = this._suiciderPlayerId != null;
			bool result;
			if (flag)
			{
				winnerPlayerId = Player.PredefinedId.GetTheOtherSide(this._suiciderPlayerId.Value);
				result = true;
			}
			else
			{
				winnerPlayerId = -1;
				for (sbyte playerId = 0; playerId < 2; playerId += 1)
				{
					bool flag2 = this.CalcPlayerScore(playerId) < 12;
					if (!flag2)
					{
						winnerPlayerId = playerId;
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x00144D24 File Offset: 0x00142F24
		public int GetPlayerCharacterId(sbyte playerId)
		{
			return this.GetPlayer(playerId).CharacterId;
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x00144D42 File Offset: 0x00142F42
		public Grid GetGrid(int index)
		{
			return this._gridStatus[index];
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x00144D4C File Offset: 0x00142F4C
		public Grid GetGrid(Coordinate2D<sbyte> coordinate)
		{
			return this.GetGrid(Grid.ToGridIndex(coordinate));
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x00144D5C File Offset: 0x00142F5C
		public Player GetPlayer(sbyte playerId)
		{
			Player result;
			if (playerId != 0)
			{
				if (playerId != 1)
				{
					throw new IndexOutOfRangeException();
				}
				result = this._playerAdversary;
			}
			else
			{
				result = this._playerSelf;
			}
			return result;
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x00144D94 File Offset: 0x00142F94
		public int CalcPlayerScore(sbyte playerId)
		{
			int score = 0;
			int i = 0;
			int len = this._gridStatus.Length;
			while (i < len)
			{
				Grid grid = this._gridStatus[i];
				OperationPointBase thesis = grid.GetThesis();
				bool flag = thesis != null && thesis.PlayerId == playerId;
				if (flag)
				{
					score += grid.GetThesisScore();
				}
				i++;
			}
			return score;
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x00144DFC File Offset: 0x00142FFC
		public OperationQuestion GetAnswerTarget(OperationAnswer answer)
		{
			Grid grid = this.GetGrid(answer.GridIndex);
			IReadOnlyList<OperationGridBase> ops = grid.GetAllOperations();
			for (int i = ops.Count - 1; i >= 0; i--)
			{
				OperationQuestion question = ops[i] as OperationQuestion;
				bool flag = question != null;
				if (flag)
				{
					return question;
				}
			}
			throw new IndexOutOfRangeException();
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x00144E60 File Offset: 0x00143060
		public static IEnumerable<Coordinate2D<sbyte>> OffsetIterate(Coordinate2D<sbyte> origin, sbyte straight, sbyte cross, Func<Coordinate2D<sbyte>, bool> breakCondition = null)
		{
			foreach (Coordinate2D<sbyte> offset in Match.OffsetStraight)
			{
				int num;
				for (int i = 0; i < (int)straight; i = num + 1)
				{
					Coordinate2D<sbyte> coord = new Coordinate2D<sbyte>((sbyte)Math.Clamp((int)origin.X + (int)offset.X * (i + 1), -128, 127), (sbyte)Math.Clamp((int)origin.Y + (int)offset.Y * (i + 1), -128, 127));
					bool flag = coord.X >= -3 && coord.X <= 3 && coord.Y >= -3 && coord.Y <= 3;
					if (flag)
					{
						bool flag2 = breakCondition != null && breakCondition(coord);
						if (flag2)
						{
							break;
						}
						yield return coord;
					}
					coord = default(Coordinate2D<sbyte>);
					num = i;
				}
				offset = default(Coordinate2D<sbyte>);
			}
			Coordinate2D<sbyte>[] array = null;
			foreach (Coordinate2D<sbyte> offset2 in Match.OffsetCross)
			{
				int num;
				for (int j = 0; j < (int)cross; j = num + 1)
				{
					Coordinate2D<sbyte> coord2 = new Coordinate2D<sbyte>((sbyte)Math.Clamp((int)origin.X + (int)offset2.X * (j + 1), -128, 127), (sbyte)Math.Clamp((int)origin.Y + (int)offset2.Y * (j + 1), -128, 127));
					bool flag3 = coord2.X >= -3 && coord2.X <= 3 && coord2.Y >= -3 && coord2.Y <= 3;
					if (flag3)
					{
						bool flag4 = breakCondition != null && breakCondition(coord2);
						if (flag4)
						{
							break;
						}
						yield return coord2;
					}
					coord2 = default(Coordinate2D<sbyte>);
					num = j;
				}
				offset2 = default(Coordinate2D<sbyte>);
			}
			Coordinate2D<sbyte>[] array2 = null;
			yield break;
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x00144E85 File Offset: 0x00143085
		public void OnGridActiveFixed(OperationPointBase pOp, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs)
		{
			pOp.ProcessOnGridActiveFixed(this, gridTrapStateExtraDiffs);
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06001502 RID: 5378 RVA: 0x00144E91 File Offset: 0x00143091
		// (set) Token: 0x06001503 RID: 5379 RVA: 0x00144E99 File Offset: 0x00143099
		public int PlayerSwitchCount { get; private set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06001504 RID: 5380 RVA: 0x00144EA2 File Offset: 0x001430A2
		public sbyte CurrentPlayerId
		{
			get
			{
				return this._currentPlayerId;
			}
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x00144EAC File Offset: 0x001430AC
		public void CommitOperation(DataContext context, OperationBase operation, IList<StatusSnapshotDiff.BookStateExtraDiff> bookStateExtraDiffs, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs)
		{
			this._beforeSimulated = null;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 2);
			defaultInterpolatedStringHandler.AppendLiteral("House[");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(operation.PlayerId);
			defaultInterpolatedStringHandler.AppendLiteral("] commit operation -> [");
			defaultInterpolatedStringHandler.AppendFormatted<OperationBase>(operation);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			this.RecordLine(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
			this.CommitOperationProcess(context, operation, bookStateExtraDiffs, gridTrapStateExtraDiffs, true);
			bool flag;
			if (this.AcceptSilentPlayerId == operation.PlayerId)
			{
				OperationSilent pOs = operation as OperationSilent;
				flag = (pOs != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.AcceptSilentPlayerId = -1;
			}
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x00144F50 File Offset: 0x00143150
		public sbyte CalcTargetPlayerId(sbyte configTargetPlayerId, sbyte selfAnchorPlayerId)
		{
			sbyte result;
			if (selfAnchorPlayerId != 0)
			{
				if (selfAnchorPlayerId != 1)
				{
					result = -1;
				}
				else
				{
					result = ((configTargetPlayerId > 0) ? 1 : 0);
				}
			}
			else
			{
				result = ((configTargetPlayerId > 0) ? 0 : 1);
			}
			return result;
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x00144F88 File Offset: 0x00143188
		internal void CommitOperationProcess(DataContext context, OperationBase operation, IList<StatusSnapshotDiff.BookStateExtraDiff> bookStateExtraDiffs, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs, bool withProcessActiveOperationCells = true)
		{
			bool flag = operation.GetStamp() >= 0 && operation.GetStamp() < this.PlayerSwitchCount;
			if (flag)
			{
				PredefinedLog.Show(15, operation.Inspect(), operation.GetStamp(), this.PlayerSwitchCount);
			}
			else
			{
				bool condition = operation.PlayerId == this._currentPlayerId;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 2);
				defaultInterpolatedStringHandler.AppendFormatted("_currentPlayerId");
				defaultInterpolatedStringHandler.AppendLiteral(" mismatch! (stamp: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(operation.GetStamp());
				defaultInterpolatedStringHandler.AppendLiteral(")");
				this.Assert(condition, defaultInterpolatedStringHandler.ToStringAndClear());
				this._lastOperationIsSilent = false;
				bool flag2 = operation is OperationSilent;
				if (flag2)
				{
					this.SwitchCurrentPlayerProcess(context, bookStateExtraDiffs, gridTrapStateExtraDiffs, withProcessActiveOperationCells);
					this._lastOperationIsSilent = true;
				}
				else
				{
					bool flag3 = operation is OperationGiveUp;
					if (flag3)
					{
						this._suiciderPlayerId = new sbyte?(operation.PlayerId);
					}
					else
					{
						OperationUseEffectCard pUec = operation as OperationUseEffectCard;
						bool flag4 = pUec != null;
						if (flag4)
						{
							sbyte playerId = pUec.PlayerId;
							sbyte effectCardId = pUec.Info.EffectCardTemplateId;
							LifeSkillCombatEffectItem effectCardConfig = LifeSkillCombatEffect.Instance[effectCardId];
							this.Assert(effectCardConfig.IsInstant, effectCardConfig.Name + " is not [Instant]");
							Player player = this.GetPlayer(playerId);
							player.DropEffectCard(effectCardId);
							ELifeSkillCombatEffectSubEffect subEffect = effectCardConfig.SubEffect;
							ELifeSkillCombatEffectSubEffect elifeSkillCombatEffectSubEffect = subEffect;
							if (elifeSkillCombatEffectSubEffect - ELifeSkillCombatEffectSubEffect.SelfGridCoverBookStatesWhenAllQuestion > 1 && elifeSkillCombatEffectSubEffect != ELifeSkillCombatEffectSubEffect.SelfDoPickByPoint)
							{
								switch (elifeSkillCombatEffectSubEffect)
								{
								case ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam:
								case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedEraseEffectsAroundHouseAllQuestionLowAndThesisLow:
								case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoCancelAroundHouseAllQuestionLowAndThesisLow:
								case ELifeSkillCombatEffectSubEffect.PointChange:
									break;
								case ELifeSkillCombatEffectSubEffect.SelfNotCostBookStates:
								case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoPickWithAroundHouseThesisCount:
									goto IL_1E0;
								default:
									goto IL_1E0;
								}
							}
							gridTrapStateExtraDiffs.Add(new StatusSnapshotDiff.GridTrapStateExtraDiff
							{
								GridIndex = pUec.Info.CellIndex,
								OwnerPlayerId = playerId,
								Type = StatusSnapshotDiff.GridTrapStateExtraDiff.TrapChangeType.Added
							});
							IL_1E0:
							switch (effectCardConfig.SubEffect)
							{
							case ELifeSkillCombatEffectSubEffect.SelfChangeBookCd:
							{
								sbyte targetPlayerId = this.CalcTargetPlayerId(effectCardConfig.SubEffectParameters[0], playerId);
								sbyte count = effectCardConfig.SubEffectParameters[3];
								sbyte value = effectCardConfig.SubEffectParameters[4];
								Player targetPlayer = this.GetPlayer(targetPlayerId);
								List<Book> states = new List<Book>();
								bool flag5 = pUec.Info.TargetBookStateIndex >= 0;
								if (flag5)
								{
									states.Add(this.GetPlayer(pUec.Info.TargetBookOwnerPlayerId).BookStates[(int)pUec.Info.TargetBookStateIndex]);
								}
								else
								{
									states.AddRange(targetPlayer.BookStates);
									CollectionUtils.Shuffle<Book>(context.Random, states);
								}
								List<sbyte> indices = new List<sbyte>();
								int i = 0;
								foreach (Book bookState in states)
								{
									bool flag6 = i >= (int)count;
									if (flag6)
									{
										break;
									}
									bool flag7 = value == sbyte.MaxValue;
									if (flag7)
									{
										bool flag8 = this.GetPlayer(pUec.Info.TargetBookOwnerPlayerId).RefBook(bookState).RemainingCd == 0;
										if (flag8)
										{
											continue;
										}
										this.GetPlayer(pUec.Info.TargetBookOwnerPlayerId).RefBook(bookState).RemainingCd = 0;
									}
									else
									{
										bool flag9 = targetPlayerId == playerId;
										if (flag9)
										{
											bool flag10 = this.GetPlayer(pUec.Info.TargetBookOwnerPlayerId).RefBook(bookState).RemainingCd <= 0;
											if (flag10)
											{
												continue;
											}
											this.GetPlayer(pUec.Info.TargetBookOwnerPlayerId).RefBook(bookState).RemainingCd -= (int)value;
										}
										else
										{
											bool flag11 = targetPlayerId != playerId;
											if (flag11)
											{
												this.GetPlayer(pUec.Info.TargetBookOwnerPlayerId).RefBook(bookState).RemainingCd += (int)value;
											}
										}
									}
									i++;
									this.GetPlayer(pUec.Info.TargetBookOwnerPlayerId).RefBook(bookState).RemainingCd = Math.Max(0, this.GetPlayer(pUec.Info.TargetBookOwnerPlayerId).RefBook(bookState).RemainingCd);
									bookStateExtraDiffs.Add(new StatusSnapshotDiff.BookStateExtraDiff
									{
										OwnerPlayerId = pUec.Info.TargetBookOwnerPlayerId,
										BookCdIndex = targetPlayer.RefBookIndex(bookState),
										NewCdValue = this.GetPlayer(pUec.Info.TargetBookOwnerPlayerId).RefBook(bookState).RemainingCd,
										NewDisplayCdValue = this.GetPlayer(pUec.Info.TargetBookOwnerPlayerId).RefBook(bookState).DisplayCd,
										ByPlayerId = playerId
									});
								}
								break;
							}
							case ELifeSkillCombatEffectSubEffect.SelfDoPickByPoint:
							{
								Grid grid = this.GetGrid(pUec.Info.CellIndex);
								bool flag12 = grid != null;
								if (flag12)
								{
									OperationPointBase thesis = grid.GetThesis();
									bool flag13 = thesis != null && thesis.PlayerId == playerId;
									if (flag13)
									{
										player.RecruitEffectCards(context.Random, this, grid.GetThesisScore());
									}
								}
								break;
							}
							case ELifeSkillCombatEffectSubEffect.SelfEraseAroundSelfThesisHouseQuestionThesis:
							{
								Grid grid2 = this.GetGrid(pUec.Info.CellIndex2);
								bool flag14 = grid2 != null;
								if (flag14)
								{
									OperationPointBase op = grid2.GetThesis();
									bool flag15 = op == null;
									if (flag15)
									{
										op = (grid2.ActiveOperation as OperationPointBase);
									}
									sbyte targetPlayerId2 = this.CalcTargetPlayerId(effectCardConfig.SubEffectParameters[0], playerId);
									bool flag16 = op != null && op.PlayerId == targetPlayerId2 && op.Point < this.GetGrid(pUec.Info.CellIndex).GetThesis().Point;
									if (flag16)
									{
										grid2.SetActiveOperation(null, this, gridTrapStateExtraDiffs);
										grid2.DropHistoryOperations();
									}
								}
								break;
							}
							case ELifeSkillCombatEffectSubEffect.SelfEraseAroundHouseQuestionEffects:
							{
								Grid cell = this.GetGrid(pUec.Info.CellIndex);
								bool flag17 = cell != null;
								if (flag17)
								{
									OperationPointBase op2 = cell.GetThesis();
									bool flag18 = op2 == null;
									if (flag18)
									{
										op2 = (cell.ActiveOperation as OperationPointBase);
									}
									sbyte targetPlayerId3 = this.CalcTargetPlayerId(effectCardConfig.SubEffectParameters[0], playerId);
									bool flag19 = op2 != null && op2.PlayerId == targetPlayerId3;
									if (flag19)
									{
										foreach (sbyte e in op2.EffectiveEffectCardTemplateIds)
										{
											ELifeSkillCombatEffectSubEffect subEffect2 = LifeSkillCombatEffect.Instance[e].SubEffect;
											ELifeSkillCombatEffectSubEffect elifeSkillCombatEffectSubEffect2 = subEffect2;
											if (elifeSkillCombatEffectSubEffect2 - ELifeSkillCombatEffectSubEffect.SelfGridCoverBookStatesWhenAllQuestion <= 1 || elifeSkillCombatEffectSubEffect2 == ELifeSkillCombatEffectSubEffect.SelfDoPickByPoint)
											{
												goto IL_39C;
											}
											switch (elifeSkillCombatEffectSubEffect2)
											{
											case ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam:
											case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedEraseEffectsAroundHouseAllQuestionLowAndThesisLow:
											case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoCancelAroundHouseAllQuestionLowAndThesisLow:
											case ELifeSkillCombatEffectSubEffect.PointChange:
												goto IL_39C;
											}
											continue;
											IL_39C:
											gridTrapStateExtraDiffs.Add(new StatusSnapshotDiff.GridTrapStateExtraDiff
											{
												GridIndex = cell.Index,
												OwnerPlayerId = targetPlayerId3,
												Type = StatusSnapshotDiff.GridTrapStateExtraDiff.TrapChangeType.Lost
											});
										}
										op2.DropEffectiveEffectCards();
									}
								}
								break;
							}
							default:
								throw new NotImplementedException();
							}
							this.RecalculateThesisPoints();
						}
						else
						{
							OperationPointBase pOp = operation as OperationPointBase;
							bool flag20 = pOp != null;
							if (flag20)
							{
								Player player2 = this.GetPlayer(pOp.PlayerId);
								Grid grid3 = this.GetGrid(pOp.Coordinate);
								OperationGridBase activeOperation = grid3.ActiveOperation;
								OperationAnswer answer = pOp as OperationAnswer;
								sbyte notCostBookEffect = -1;
								foreach (int friendlyCharacterId in pOp.UseFriendlyCharacterIds)
								{
									pOp.BasePoint += player2.UseFriendlyCharacterAndGivePoint(context.Random, friendlyCharacterId, this.LifeSkillType);
								}
								foreach (sbyte e2 in pOp.EffectiveEffectCardTemplateIds)
								{
									bool flag21 = LifeSkillCombatEffect.Instance[e2].SubEffect == ELifeSkillCombatEffectSubEffect.SelfNotCostBookStates;
									if (flag21)
									{
										notCostBookEffect = e2;
										break;
									}
								}
								foreach (sbyte effectCardId2 in pOp.EffectiveEffectCardTemplateIds)
								{
									ELifeSkillCombatEffectSubEffect subEffect3 = LifeSkillCombatEffect.Instance[effectCardId2].SubEffect;
									ELifeSkillCombatEffectSubEffect elifeSkillCombatEffectSubEffect3 = subEffect3;
									if (elifeSkillCombatEffectSubEffect3 - ELifeSkillCombatEffectSubEffect.SelfGridCoverBookStatesWhenAllQuestion <= 1 || elifeSkillCombatEffectSubEffect3 == ELifeSkillCombatEffectSubEffect.SelfDoPickByPoint)
									{
										goto IL_893;
									}
									switch (elifeSkillCombatEffectSubEffect3)
									{
									case ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam:
									case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedEraseEffectsAroundHouseAllQuestionLowAndThesisLow:
									case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoCancelAroundHouseAllQuestionLowAndThesisLow:
									case ELifeSkillCombatEffectSubEffect.PointChange:
										goto IL_893;
									}
									continue;
									IL_893:
									gridTrapStateExtraDiffs.Add(new StatusSnapshotDiff.GridTrapStateExtraDiff
									{
										GridIndex = pOp.GridIndex,
										OwnerPlayerId = pOp.PlayerId,
										Type = StatusSnapshotDiff.GridTrapStateExtraDiff.TrapChangeType.Added
									});
								}
								bool flag22 = notCostBookEffect >= 0;
								if (!flag22)
								{
									OperationQuestion question = pOp as OperationQuestion;
									bool flag23 = question != null;
									if (flag23)
									{
										foreach (Book bookState2 in question.EffectiveBookStates)
										{
											this.Assert(!bookState2.IsCd, "using bookState when [IsCd]");
											player2.RefBook(bookState2).RemainingCd = 2;
											bookStateExtraDiffs.Add(new StatusSnapshotDiff.BookStateExtraDiff
											{
												OwnerPlayerId = player2.Id,
												BookCdIndex = player2.RefBookIndex(bookState2),
												NewCdValue = player2.RefBook(bookState2).RemainingCd,
												NewDisplayCdValue = player2.RefBook(bookState2).DisplayCd,
												ByPlayerId = -1
											});
										}
									}
									bool flag24 = answer != null;
									if (flag24)
									{
										bool isCdEffect = false;
										foreach (sbyte e3 in this.GetAnswerTarget(answer).EffectiveEffectCardTemplateIds)
										{
											ELifeSkillCombatEffectSubEffect subEffect4 = LifeSkillCombatEffect.Instance[e3].SubEffect;
											ELifeSkillCombatEffectSubEffect elifeSkillCombatEffectSubEffect4 = subEffect4;
											switch (elifeSkillCombatEffectSubEffect4)
											{
											case ELifeSkillCombatEffectSubEffect.SelfGridCoverBookStatesWhenAllQuestion:
											case ELifeSkillCombatEffectSubEffect.SelfDoPickByPoint:
												goto IL_B01;
											case ELifeSkillCombatEffectSubEffect.SelfTrapedInCell:
											{
												bool flag25 = LifeSkillCombatEffect.Instance[e3].Type == ELifeSkillCombatEffectType.BUFF;
												if (flag25)
												{
													int delta = (int)(LifeSkillCombatEffect.Instance[e3].SubEffectParameters[3] * 10);
													answer.ChangeBasePoint(delta);
												}
												else
												{
													bool flag26 = LifeSkillCombatEffect.Instance[e3].Type == ELifeSkillCombatEffectType.Strategy;
													if (flag26)
													{
														isCdEffect = true;
													}
												}
												gridTrapStateExtraDiffs.Add(new StatusSnapshotDiff.GridTrapStateExtraDiff
												{
													GridIndex = answer.GridIndex,
													OwnerPlayerId = this.GetAnswerTarget(answer).PlayerId,
													Type = StatusSnapshotDiff.GridTrapStateExtraDiff.TrapChangeType.Triggered
												});
												break;
											}
											case ELifeSkillCombatEffectSubEffect.SelfChangeBookCd:
												break;
											default:
												switch (elifeSkillCombatEffectSubEffect4)
												{
												case ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam:
												case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedEraseEffectsAroundHouseAllQuestionLowAndThesisLow:
												case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoCancelAroundHouseAllQuestionLowAndThesisLow:
												case ELifeSkillCombatEffectSubEffect.PointChange:
													goto IL_B01;
												}
												break;
											}
											continue;
											IL_B01:
											gridTrapStateExtraDiffs.Add(new StatusSnapshotDiff.GridTrapStateExtraDiff
											{
												GridIndex = answer.GridIndex,
												OwnerPlayerId = this.GetAnswerTarget(answer).PlayerId,
												Type = StatusSnapshotDiff.GridTrapStateExtraDiff.TrapChangeType.Triggered
											});
										}
										foreach (Book bookState3 in answer.EffectiveBookStates)
										{
											this.Assert(!bookState3.IsCd, "using bookState when [IsCd]");
											player2.RefBook(bookState3).RemainingCd = (isCdEffect ? 3 : 2);
											bookStateExtraDiffs.Add(new StatusSnapshotDiff.BookStateExtraDiff
											{
												OwnerPlayerId = player2.Id,
												BookCdIndex = player2.RefBookIndex(bookState3),
												NewCdValue = player2.RefBook(bookState3).RemainingCd,
												NewDisplayCdValue = player2.RefBook(bookState3).DisplayCd,
												ByPlayerId = -1
											});
										}
									}
								}
								int card = -1;
								foreach (sbyte e4 in pOp.EffectiveEffectCardTemplateIds)
								{
									bool flag27 = LifeSkillCombatEffect.Instance[e4].SubEffect == ELifeSkillCombatEffectSubEffect.PointChange;
									if (flag27)
									{
										card = (int)e4;
										break;
									}
								}
								bool flag28 = card >= 0;
								if (flag28)
								{
									int delta2 = (int)(LifeSkillCombatEffect.Instance[card].SubEffectParameters[3] * 10);
									pOp.ChangeBasePoint(delta2);
								}
								bool directFixed = false;
								foreach (sbyte card2 in pOp.EffectiveEffectCardTemplateIds)
								{
									ELifeSkillCombatEffectSubEffect subEffect5 = LifeSkillCombatEffect.Instance[card2].SubEffect;
									ELifeSkillCombatEffectSubEffect elifeSkillCombatEffectSubEffect5 = subEffect5;
									if (elifeSkillCombatEffectSubEffect5 - ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndTransition <= 1)
									{
										directFixed = true;
									}
								}
								bool flag29 = directFixed;
								if (flag29)
								{
									grid3.SetActiveOperation(pOp, this, gridTrapStateExtraDiffs);
									grid3.SetActiveOperation(null, this, gridTrapStateExtraDiffs);
								}
								else
								{
									OperationPointBase activeOperationWithPoint = activeOperation as OperationPointBase;
									bool flag30 = activeOperationWithPoint == null || activeOperationWithPoint.Point < pOp.Point;
									if (flag30)
									{
										grid3.SetActiveOperation(pOp, this, gridTrapStateExtraDiffs);
										bool flag31 = answer != null;
										if (flag31)
										{
											grid3.SetActiveOperation(null, this, gridTrapStateExtraDiffs);
											OperationQuestion answerTarget = this.GetAnswerTarget(answer);
											Player targetPlayer2 = this.GetPlayer(answerTarget.PlayerId);
											foreach (Book bookState4 in answerTarget.EffectiveBookStates)
											{
												targetPlayer2.RefBook(bookState4).RemainingCd++;
												bookStateExtraDiffs.Add(new StatusSnapshotDiff.BookStateExtraDiff
												{
													OwnerPlayerId = targetPlayer2.Id,
													BookCdIndex = targetPlayer2.RefBookIndex(bookState4),
													NewCdValue = targetPlayer2.RefBook(bookState4).RemainingCd,
													NewDisplayCdValue = targetPlayer2.RefBook(bookState4).DisplayCd,
													ByPlayerId = -1
												});
											}
										}
									}
								}
								foreach (sbyte effectCard in pOp.EffectiveEffectCardTemplateIds)
								{
									player2.DropEffectCard(effectCard);
								}
								this.SwitchCurrentPlayerProcess(context, bookStateExtraDiffs, gridTrapStateExtraDiffs, withProcessActiveOperationCells);
							}
							else
							{
								OperationPrepareForceAdversary pOf = operation as OperationPrepareForceAdversary;
								bool flag32 = pOf != null;
								if (flag32)
								{
									Player player3 = this.GetPlayer(pOf.PlayerId);
									Player adversaryPlayer = this.GetPlayer(Player.PredefinedId.GetTheOtherSide(pOf.PlayerId));
									OperationPrepareForceAdversary.ForceAdversaryOperation type = pOf.Type;
									OperationPrepareForceAdversary.ForceAdversaryOperation forceAdversaryOperation = type;
									if (forceAdversaryOperation != OperationPrepareForceAdversary.ForceAdversaryOperation.Silent)
									{
										if (forceAdversaryOperation != OperationPrepareForceAdversary.ForceAdversaryOperation.GiveUp)
										{
											throw new ArgumentOutOfRangeException();
										}
										player3.SetForceGiveUpRemainingCount(player3.ForceGiveUpRemainingCount - 1);
										short taiwuAttainment = player3.Attainment;
										short enemyAttainment = adversaryPlayer.Attainment;
										bool flag33 = taiwuAttainment < 50 || enemyAttainment < 50;
										bool success;
										if (flag33)
										{
											success = context.Random.CheckPercentProb((int)Math.Pow((double)(taiwuAttainment - enemyAttainment - 50), 1.0499999523162842));
										}
										else
										{
											success = context.Random.CheckPercentProb((int)Math.Pow((double)(taiwuAttainment - enemyAttainment - 80), 1.0499999523162842));
										}
										bool flag34 = success;
										if (flag34)
										{
											this._suiciderPlayerId = new sbyte?(adversaryPlayer.Id);
										}
									}
									else
									{
										int useCount = 3 - player3.ForceSilentRemainingCount;
										DomainManager.TaiwuEvent.OnLifeSkillCombatForceSilent(adversaryPlayer.CharacterId, (sbyte)adversaryPlayer.ForcedSilentCount, (sbyte)(useCount - adversaryPlayer.ForcedSilentCount));
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x00146094 File Offset: 0x00144294
		public OperationList CalcUsableOperationList(IEnumerable<sbyte> additionalEffectCards, IEnumerable<sbyte> usedBookStateIndices, IEnumerable<int> usedFriendlyCharacterIds, out bool maybeHasAdditionalOperation, bool exceptIncompleteMatchingEffectCards = true)
		{
			Match.<>c__DisplayClass38_0 CS$<>8__locals1 = new Match.<>c__DisplayClass38_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.player = this.GetPlayer(this._currentPlayerId);
			CS$<>8__locals1.playerId = CS$<>8__locals1.player.Id;
			CS$<>8__locals1.operations = new OperationList();
			CS$<>8__locals1.operations.Add(new OperationSilent(CS$<>8__locals1.playerId, this.PlayerSwitchCount));
			CS$<>8__locals1.operations.Add(new OperationGiveUp(CS$<>8__locals1.playerId, this.PlayerSwitchCount));
			bool flag = this.AcceptSilentPlayerId == CS$<>8__locals1.playerId;
			OperationList operations;
			if (flag)
			{
				maybeHasAdditionalOperation = false;
				operations = CS$<>8__locals1.operations;
			}
			else
			{
				CS$<>8__locals1.wantUseEffectCards = new HashSet<sbyte>();
				bool flag2 = additionalEffectCards != null;
				if (flag2)
				{
					CS$<>8__locals1.wantUseEffectCards.UnionWith(additionalEffectCards);
				}
				CS$<>8__locals1.usedBookStates = (from index in usedBookStateIndices
				select CS$<>8__locals1.player.BookStates[(int)index]).ToArray<Book>();
				CS$<>8__locals1.usedFriendlyCharacterStates = (from s in usedFriendlyCharacterIds.Select(delegate(int id)
				{
					foreach (ValueTuple<int, bool, int> characterState in CS$<>8__locals1.player.FriendlyCharacterStates)
					{
						bool flag25 = characterState.Item1 == id;
						if (flag25)
						{
							return characterState;
						}
					}
					string tag = "LifeSkillCombat";
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 2);
					defaultInterpolatedStringHandler.AppendLiteral("request character ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(id);
					defaultInterpolatedStringHandler.AppendLiteral(" isn't exist in PlayerHouse[");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(CS$<>8__locals1.playerId);
					defaultInterpolatedStringHandler.AppendLiteral("]'s teammate");
					AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
					return new ValueTuple<int, bool, int>(-1, false, 0);
				})
				where s.Item1 >= 0
				select s).ToArray<ValueTuple<int, bool, int>>();
				CS$<>8__locals1.point = 0;
				foreach (Book bookState in CS$<>8__locals1.usedBookStates)
				{
					CS$<>8__locals1.point += bookState.BasePoint;
				}
				foreach (ValueTuple<int, bool, int> charState in CS$<>8__locals1.usedFriendlyCharacterStates)
				{
					CS$<>8__locals1.point += charState.Item3;
				}
				maybeHasAdditionalOperation = CS$<>8__locals1.wantUseEffectCards.Any((sbyte e) => LifeSkillCombatEffect.Instance[e].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionAroundHouseThesisLow || LifeSkillCombatEffect.Instance[e].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndTransition || LifeSkillCombatEffect.Instance[e].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionAroundHouseThesisBreakWhenAdversaryThesisHigh || LifeSkillCombatEffect.Instance[e].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndRecycleCardAndExchangeOperation);
				HashSet<Coordinate2D<sbyte>> canRhetoricalQuestionIndices = new HashSet<Coordinate2D<sbyte>>();
				HashSet<Coordinate2D<sbyte>> canQuestionIndices = new HashSet<Coordinate2D<sbyte>>();
				HashSet<Coordinate2D<sbyte>> canAnswerIndices = new HashSet<Coordinate2D<sbyte>>();
				CS$<>8__locals1.additionalIndices = new HashSet<Coordinate2D<sbyte>>();
				int index3 = 0;
				int len = this._gridStatus.Length;
				while (index3 < len)
				{
					Grid grid = this._gridStatus[index3];
					Coordinate2D<sbyte> coordinate = grid.Coordinate;
					OperationPointBase thesis = grid.GetThesis();
					OperationGridBase activeOperation = grid.ActiveOperation;
					bool flag3 = thesis != null && thesis.PlayerId == CS$<>8__locals1.player.Id;
					if (flag3)
					{
						foreach (ValueTuple<sbyte, sbyte> valueTuple in Match._Offset8)
						{
							sbyte x = valueTuple.Item1;
							sbyte y = valueTuple.Item2;
							canQuestionIndices.Add(new Coordinate2D<sbyte>(coordinate.X + x, coordinate.Y + y));
						}
					}
					else
					{
						bool flag4 = activeOperation != null && activeOperation.PlayerId != CS$<>8__locals1.player.Id;
						if (flag4)
						{
							OperationQuestion question = activeOperation as OperationQuestion;
							bool flag5 = question != null;
							if (flag5)
							{
								canAnswerIndices.Add(grid.Coordinate);
								bool flag6 = !(question is OperationQuestionRhetorical);
								if (flag6)
								{
									foreach (ValueTuple<sbyte, sbyte> valueTuple2 in Match._Offset8)
									{
										sbyte x2 = valueTuple2.Item1;
										sbyte y2 = valueTuple2.Item2;
										canRhetoricalQuestionIndices.Add(new Coordinate2D<sbyte>(coordinate.X + x2, coordinate.Y + y2));
									}
								}
							}
						}
					}
					index3++;
				}
				for (sbyte y3 = -3; y3 <= 3; y3 += 1)
				{
					for (sbyte x3 = -3; x3 <= 3; x3 += 1)
					{
						bool flag7 = (Math.Abs(x3) != 3 || Math.Abs(y3) != 3) && (Math.Abs(x3) != 3 || y3 != 0) && (x3 != 0 || Math.Abs(y3) != 3);
						if (!flag7)
						{
							canQuestionIndices.Add(new Coordinate2D<sbyte>(x3, y3));
						}
					}
				}
				foreach (sbyte e2 in CS$<>8__locals1.wantUseEffectCards)
				{
					Match.<>c__DisplayClass38_1 CS$<>8__locals2;
					CS$<>8__locals2.e = e2;
					Match.<>c__DisplayClass38_2 CS$<>8__locals3 = new Match.<>c__DisplayClass38_2();
					CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals1;
					sbyte house = this.CalcTargetPlayerId(LifeSkillCombatEffect.Instance[CS$<>8__locals2.e].SubEffectParameters[0], CS$<>8__locals3.CS$<>8__locals1.player.Id);
					sbyte b = LifeSkillCombatEffect.Instance[CS$<>8__locals2.e].SubEffectParameters[1];
					sbyte b2 = LifeSkillCombatEffect.Instance[CS$<>8__locals2.e].SubEffectParameters[2];
					sbyte value = LifeSkillCombatEffect.Instance[CS$<>8__locals2.e].SubEffectParameters[3];
					sbyte cross = b2;
					sbyte straight = b;
					CS$<>8__locals3.house = house;
					int index2 = 0;
					while (index2 < this._gridStatus.Length)
					{
						Match.<>c__DisplayClass38_3 CS$<>8__locals4 = new Match.<>c__DisplayClass38_3();
						CS$<>8__locals4.CS$<>8__locals2 = CS$<>8__locals3;
						CS$<>8__locals4.grid = this._gridStatus[index2];
						OperationPointBase thesis2 = CS$<>8__locals4.grid.GetThesis();
						CS$<>8__locals4.coord = CS$<>8__locals4.grid.Coordinate;
						ELifeSkillCombatEffectSubEffect subEffect = LifeSkillCombatEffect.Instance[CS$<>8__locals2.e].SubEffect;
						ELifeSkillCombatEffectSubEffect elifeSkillCombatEffectSubEffect = subEffect;
						if (elifeSkillCombatEffectSubEffect != ELifeSkillCombatEffectSubEffect.SelfExtraQuestionAroundHouseThesisLow)
						{
							switch (elifeSkillCombatEffectSubEffect)
							{
							case ELifeSkillCombatEffectSubEffect.SelfExtraQuestionAroundHouseThesisBreakWhenAdversaryThesisHigh:
							{
								bool flag8 = thesis2 != null && thesis2.PlayerId == CS$<>8__locals4.CS$<>8__locals2.house;
								if (flag8)
								{
									Coordinate2D<sbyte> coord = CS$<>8__locals4.coord;
									sbyte straight2 = straight;
									sbyte cross2 = cross;
									Func<Coordinate2D<sbyte>, bool> breakCondition;
									if ((breakCondition = CS$<>8__locals4.<>9__5) == null)
									{
										breakCondition = (CS$<>8__locals4.<>9__5 = delegate(Coordinate2D<sbyte> p)
										{
											bool flag25 = p == CS$<>8__locals4.coord;
											bool result;
											if (flag25)
											{
												result = false;
											}
											else
											{
												OperationPointBase pt = CS$<>8__locals4.CS$<>8__locals2.CS$<>8__locals1.<>4__this.GetGrid(p).GetThesis();
												result = (pt != null && pt.PlayerId != CS$<>8__locals4.CS$<>8__locals2.house && pt.Point > CS$<>8__locals4.CS$<>8__locals2.CS$<>8__locals1.point);
											}
											return result;
										});
									}
									foreach (Coordinate2D<sbyte> target in Match.OffsetIterate(coord, straight2, cross2, breakCondition))
									{
										bool flag9 = CS$<>8__locals4.<CalcUsableOperationList>g__AddExtraAdditionalQuestionOnGrid|4(target, true, ref CS$<>8__locals2);
										if (!flag9)
										{
											maybeHasAdditionalOperation = true;
										}
									}
								}
								break;
							}
							case ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndTransition:
							{
								bool flag10 = thesis2 != null && thesis2.PlayerId == CS$<>8__locals4.CS$<>8__locals2.house && thesis2.Point < CS$<>8__locals4.CS$<>8__locals2.CS$<>8__locals1.point;
								if (flag10)
								{
									bool flag11 = CS$<>8__locals4.<CalcUsableOperationList>g__AddExtraAdditionalQuestionOnGrid|4(CS$<>8__locals4.grid.Coordinate, false, ref CS$<>8__locals2);
									if (!flag11)
									{
										maybeHasAdditionalOperation = true;
									}
								}
								break;
							}
							case ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndRecycleCardAndExchangeOperation:
							{
								bool flag12 = thesis2 != null && thesis2.PlayerId == CS$<>8__locals4.CS$<>8__locals2.CS$<>8__locals1.playerId;
								if (flag12)
								{
									bool flag13 = CS$<>8__locals4.<CalcUsableOperationList>g__AddExtraAdditionalQuestionOnGrid|4(CS$<>8__locals4.grid.Coordinate, false, ref CS$<>8__locals2);
									if (!flag13)
									{
										maybeHasAdditionalOperation = true;
									}
								}
								break;
							}
							}
						}
						else
						{
							bool flag14 = thesis2 != null && thesis2.PlayerId == CS$<>8__locals4.CS$<>8__locals2.house && thesis2.Point < CS$<>8__locals4.CS$<>8__locals2.CS$<>8__locals1.point;
							if (flag14)
							{
								foreach (Coordinate2D<sbyte> target2 in Match.OffsetIterate(CS$<>8__locals4.coord, straight, cross, null))
								{
									bool flag15 = CS$<>8__locals4.<CalcUsableOperationList>g__AddExtraAdditionalQuestionOnGrid|4(target2, true, ref CS$<>8__locals2);
									if (!flag15)
									{
										maybeHasAdditionalOperation = true;
									}
								}
							}
						}
						IL_721:
						index2++;
						continue;
						goto IL_721;
					}
				}
				canAnswerIndices.ExceptWith(CS$<>8__locals1.additionalIndices);
				canQuestionIndices.ExceptWith(CS$<>8__locals1.additionalIndices);
				canRhetoricalQuestionIndices.ExceptWith(CS$<>8__locals1.additionalIndices);
				canQuestionIndices.ExceptWith(canAnswerIndices);
				canRhetoricalQuestionIndices.ExceptWith(canAnswerIndices);
				canRhetoricalQuestionIndices.ExceptWith(canQuestionIndices);
				foreach (Coordinate2D<sbyte> coordinate2 in canQuestionIndices)
				{
					bool flag16 = Math.Abs(coordinate2.X) > 3 || Math.Abs(coordinate2.Y) > 3;
					if (!flag16)
					{
						Grid grid2 = this.GetGrid(coordinate2);
						bool flag17 = grid2.ActiveOperation == null && grid2.GetThesis() == null;
						if (flag17)
						{
							CS$<>8__locals1.operations.Add(new OperationQuestion(CS$<>8__locals1.player.Id, this.PlayerSwitchCount, Grid.ToGridIndex(coordinate2), CS$<>8__locals1.point, CS$<>8__locals1.wantUseEffectCards).CommitUsedBookStates(CS$<>8__locals1.usedBookStates, CS$<>8__locals1.usedFriendlyCharacterStates, false));
						}
					}
				}
				foreach (Coordinate2D<sbyte> coordinate3 in canRhetoricalQuestionIndices)
				{
					bool flag18 = Math.Abs(coordinate3.X) > 3 || Math.Abs(coordinate3.Y) > 3;
					if (!flag18)
					{
						Grid grid3 = this.GetGrid(coordinate3);
						bool flag19 = grid3.ActiveOperation == null && grid3.GetThesis() == null;
						if (flag19)
						{
							CS$<>8__locals1.operations.Add(new OperationQuestionRhetorical(CS$<>8__locals1.player.Id, this.PlayerSwitchCount, grid3.Index, CS$<>8__locals1.point, CS$<>8__locals1.wantUseEffectCards).CommitUsedBookStates(CS$<>8__locals1.usedBookStates, CS$<>8__locals1.usedFriendlyCharacterStates, false));
						}
					}
				}
				foreach (Coordinate2D<sbyte> coordinate4 in canAnswerIndices)
				{
					bool flag20 = Math.Abs(coordinate4.X) > 3 || Math.Abs(coordinate4.Y) > 3;
					if (!flag20)
					{
						Grid grid4 = this.GetGrid(coordinate4);
						CS$<>8__locals1.operations.Add(new OperationAnswer(CS$<>8__locals1.player.Id, this.PlayerSwitchCount, grid4.Index, CS$<>8__locals1.point, CS$<>8__locals1.wantUseEffectCards).CommitUsedBookStates(CS$<>8__locals1.usedBookStates, CS$<>8__locals1.usedFriendlyCharacterStates, false));
					}
				}
				bool flag21 = CS$<>8__locals1.player.ForceSilentRemainingCount > 0;
				if (flag21)
				{
					CS$<>8__locals1.operations.Add(new OperationPrepareForceAdversary(CS$<>8__locals1.playerId, this.PlayerSwitchCount, OperationPrepareForceAdversary.ForceAdversaryOperation.Silent));
				}
				bool flag22 = CS$<>8__locals1.player.ForceGiveUpRemainingCount > 0;
				if (flag22)
				{
					CS$<>8__locals1.operations.Add(new OperationPrepareForceAdversary(CS$<>8__locals1.playerId, this.PlayerSwitchCount, OperationPrepareForceAdversary.ForceAdversaryOperation.GiveUp));
				}
				if (exceptIncompleteMatchingEffectCards)
				{
					HashSet<sbyte> effectiveCardSet = new HashSet<sbyte>();
					HashSet<sbyte> remainCardSet = new HashSet<sbyte>();
					for (int i = CS$<>8__locals1.operations.Count - 1; i >= 0; i--)
					{
						OperationPointBase pOp = CS$<>8__locals1.operations[i] as OperationPointBase;
						bool flag23 = pOp == null;
						if (!flag23)
						{
							effectiveCardSet.Clear();
							effectiveCardSet.UnionWith(pOp.PickEffectiveEffectCards(CS$<>8__locals1.wantUseEffectCards));
							foreach (sbyte effectCardId in effectiveCardSet)
							{
								pOp.RegisterEffectiveEffectCards(effectCardId);
							}
							remainCardSet.Clear();
							remainCardSet.UnionWith(CS$<>8__locals1.wantUseEffectCards);
							remainCardSet.ExceptWith(effectiveCardSet);
							bool flag24 = remainCardSet.Count > 0;
							if (flag24)
							{
								CS$<>8__locals1.operations.RemoveAt(i);
							}
						}
					}
				}
				operations = CS$<>8__locals1.operations;
			}
			return operations;
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x00146CAC File Offset: 0x00144EAC
		public OperationList CalcUsableSecondPhaseOperationList(OperationUseEffectCard firstPhaseOperation, IEnumerable<sbyte> additionalEffectCards, IEnumerable<sbyte> usedBookStateIndices, IEnumerable<int> usedFriendlyCharacterIds)
		{
			OperationList result = new OperationList();
			OperationUseEffectCard.UseEffectCardInfo info = firstPhaseOperation.Info;
			sbyte playerId = firstPhaseOperation.PlayerId;
			sbyte effectCardId = info.EffectCardTemplateId;
			LifeSkillCombatEffectItem effectCardConfig = LifeSkillCombatEffect.Instance[effectCardId];
			switch (effectCardConfig.SubEffect)
			{
			case ELifeSkillCombatEffectSubEffect.SelfChangeBookCd:
			{
				sbyte targetPlayerId3 = this.CalcTargetPlayerId(effectCardConfig.SubEffectParameters[0], playerId);
				sbyte value = effectCardConfig.SubEffectParameters[4];
				Player targetPlayer = this.GetPlayer(targetPlayerId3);
				bool flag = value == sbyte.MaxValue;
				if (flag)
				{
					bool flag2 = targetPlayer.BookStates.Any((Book b) => b.RemainingCd != 0);
					if (flag2)
					{
						info.TargetBookOwnerPlayerId = targetPlayerId3;
						result.Add(new OperationUseEffectCard(playerId, this.PlayerSwitchCount, info));
					}
				}
				else
				{
					bool flag3 = targetPlayerId3 == playerId;
					if (flag3)
					{
						bool flag4 = targetPlayer.BookStates.Any((Book b) => b.RemainingCd > 0);
						if (flag4)
						{
							info.TargetBookOwnerPlayerId = targetPlayerId3;
							result.Add(new OperationUseEffectCard(playerId, this.PlayerSwitchCount, info));
						}
					}
					else
					{
						bool flag5 = targetPlayerId3 != playerId;
						if (flag5)
						{
							result.Add(new OperationUseEffectCard(playerId, this.PlayerSwitchCount, info));
						}
					}
				}
				break;
			}
			case ELifeSkillCombatEffectSubEffect.SelfDoPickByPoint:
				result.AddRange(this._gridStatus.Where(delegate(Grid grid)
				{
					OperationPointBase op2 = grid.GetThesis();
					return op2 != null && op2.PlayerId == playerId;
				}).Select(delegate(Grid grid)
				{
					info.CellIndex2 = grid.Index;
					return new OperationUseEffectCard(playerId, this.PlayerSwitchCount, info);
				}));
				break;
			case ELifeSkillCombatEffectSubEffect.SelfEraseAroundSelfThesisHouseQuestionThesis:
			{
				sbyte targetPlayerId2 = this.CalcTargetPlayerId(effectCardConfig.SubEffectParameters[0], playerId);
				foreach (Coordinate2D<sbyte> target in Match.OffsetIterate(this._gridStatus[firstPhaseOperation.Info.CellIndex].Coordinate, 1, 1, null))
				{
					Grid grid2 = this.GetGrid(target);
					OperationPointBase op = grid2.GetThesis();
					bool flag6 = op != null && op.PlayerId == targetPlayerId2 && op.Point < this.GetGrid(info.CellIndex).GetThesis().Point;
					if (flag6)
					{
						info.CellIndex2 = grid2.Index;
						result.Add(new OperationUseEffectCard(playerId, this.PlayerSwitchCount, info));
					}
				}
				break;
			}
			case ELifeSkillCombatEffectSubEffect.SelfEraseAroundHouseQuestionEffects:
			{
				sbyte targetPlayerId = this.CalcTargetPlayerId(effectCardConfig.SubEffectParameters[0], playerId);
				result.AddRange(this._gridStatus.Where(delegate(Grid grid)
				{
					OperationPointBase op2 = grid.GetThesis();
					bool flag7 = op2 == null;
					if (flag7)
					{
						op2 = (grid.ActiveOperation as OperationPointBase);
					}
					return op2 != null && op2.PlayerId == targetPlayerId;
				}).Select(delegate(Grid grid)
				{
					info.CellIndex2 = grid.Index;
					return new OperationUseEffectCard(playerId, this.PlayerSwitchCount, info);
				}));
				break;
			}
			}
			return result;
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x00147000 File Offset: 0x00145200
		public void CalcUsableFirstPhaseEffectCardInfo(sbyte effectCardTemplateId, out List<int> gridIndices, out List<sbyte> bookIndices)
		{
			sbyte playerId = this._currentPlayerId;
			LifeSkillCombatEffectItem effectCardConfig = LifeSkillCombatEffect.Instance[effectCardTemplateId];
			gridIndices = new List<int>();
			bookIndices = new List<sbyte>();
			ELifeSkillCombatEffectSubEffect subEffect = effectCardConfig.SubEffect;
			ELifeSkillCombatEffectSubEffect elifeSkillCombatEffectSubEffect = subEffect;
			if (elifeSkillCombatEffectSubEffect - ELifeSkillCombatEffectSubEffect.SelfDoPickByPoint <= 2)
			{
				sbyte targetPlayerId = this.CalcTargetPlayerId(effectCardConfig.SubEffectParameters[0], playerId);
				bool flag = ELifeSkillCombatEffectSubEffect.SelfEraseAroundSelfThesisHouseQuestionThesis == effectCardConfig.SubEffect;
				if (flag)
				{
					targetPlayerId = playerId;
				}
				foreach (Grid grid in this._gridStatus)
				{
					bool flag2 = grid != null;
					if (flag2)
					{
						OperationPointBase thesis = grid.GetThesis();
						bool flag3 = thesis != null && thesis.PlayerId == targetPlayerId;
						if (flag3)
						{
							gridIndices.Add(grid.Index);
						}
					}
				}
			}
			ELifeSkillCombatEffectSubEffect subEffect2 = effectCardConfig.SubEffect;
			ELifeSkillCombatEffectSubEffect elifeSkillCombatEffectSubEffect2 = subEffect2;
			if (elifeSkillCombatEffectSubEffect2 == ELifeSkillCombatEffectSubEffect.SelfChangeBookCd)
			{
				sbyte targetPlayerId2 = this.CalcTargetPlayerId(effectCardConfig.SubEffectParameters[0], playerId);
				sbyte value = effectCardConfig.SubEffectParameters[4];
				Player targetPlayer = this.GetPlayer(targetPlayerId2);
				bool flag4 = value == sbyte.MaxValue;
				if (flag4)
				{
					for (sbyte index = 0; index < 9; index += 1)
					{
						Book b = targetPlayer.BookStates[(int)index];
						bool flag5 = b.RemainingCd != 0;
						if (flag5)
						{
							bookIndices.Add(index);
						}
					}
				}
				else
				{
					bool flag6 = targetPlayerId2 == playerId;
					if (flag6)
					{
						for (sbyte index2 = 0; index2 < 9; index2 += 1)
						{
							Book b2 = targetPlayer.BookStates[(int)index2];
							bool flag7 = b2.RemainingCd > 0;
							if (flag7)
							{
								bookIndices.Add(index2);
							}
						}
					}
					else
					{
						bool flag8 = targetPlayerId2 != playerId;
						if (flag8)
						{
							for (sbyte index3 = 0; index3 < 9; index3 += 1)
							{
								bookIndices.Add(index3);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x001471EC File Offset: 0x001453EC
		public List<sbyte> GetNotUsableEffectCardTemplateIds(DataContext context, IEnumerable<sbyte> usedBookStateIndices, IEnumerable<int> usedFriendlyCharacterIds, List<sbyte> selectedEffectCardTemplateIds)
		{
			HashSet<sbyte> result = new HashSet<sbyte>();
			bool usingFlexible = false;
			bool flag;
			OperationList usableOperations = this.CalcUsableOperationList(selectedEffectCardTemplateIds, usedBookStateIndices, usedFriendlyCharacterIds, out flag, true);
			foreach (sbyte cardId4 in selectedEffectCardTemplateIds)
			{
				LifeSkillCombatEffectItem card = LifeSkillCombatEffect.Instance[cardId4];
				result.UnionWith(card.BanCardList);
				result.Add(cardId4);
				bool flag2 = card.Group == ELifeSkillCombatEffectGroup.FlexibleFall;
				if (flag2)
				{
					usingFlexible = true;
				}
			}
			HashSet<sbyte> handleEffectCards = this.GetPlayer(this._currentPlayerId).EffectCards.ToHashSet<sbyte>();
			foreach (sbyte cardId2 in handleEffectCards)
			{
				bool flag3 = result.Contains(cardId2);
				if (!flag3)
				{
					bool flag4 = !Match.<GetNotUsableEffectCardTemplateIds>g__IsSpecialCard|41_0(cardId2);
					if (!flag4)
					{
						selectedEffectCardTemplateIds.Add(cardId2);
						OperationList specialUsableOperations = this.CalcUsableOperationList(selectedEffectCardTemplateIds, usedBookStateIndices, usedFriendlyCharacterIds, out flag, true);
						selectedEffectCardTemplateIds.RemoveAt(selectedEffectCardTemplateIds.Count - 1);
						bool flag5 = !Match.<GetNotUsableEffectCardTemplateIds>g__IsCardUsableInOperations|41_1(cardId2, specialUsableOperations);
						if (flag5)
						{
							result.Add(cardId2);
						}
					}
				}
			}
			foreach (sbyte cardId3 in handleEffectCards)
			{
				LifeSkillCombatEffectItem card2 = LifeSkillCombatEffect.Instance[cardId3];
				bool flag6 = result.Contains(cardId3);
				if (!flag6)
				{
					bool flag7 = card2.Group == ELifeSkillCombatEffectGroup.FlexibleFall;
					if (flag7)
					{
						bool flag8 = usingFlexible;
						if (flag8)
						{
							result.Add(cardId3);
							continue;
						}
					}
					bool flag9 = !card2.IsInstant && !Match.<GetNotUsableEffectCardTemplateIds>g__IsSpecialCard|41_0(cardId3) && !Match.<GetNotUsableEffectCardTemplateIds>g__IsCardUsableInOperations|41_1(cardId3, usableOperations);
					if (flag9)
					{
						result.Add(cardId3);
					}
					ELifeSkillCombatEffectSubEffect subEffect = card2.SubEffect;
					ELifeSkillCombatEffectSubEffect elifeSkillCombatEffectSubEffect = subEffect;
					if (elifeSkillCombatEffectSubEffect == ELifeSkillCombatEffectSubEffect.SelfChangeBookCd)
					{
						sbyte targetPlayerId = this.CalcTargetPlayerId(card2.SubEffectParameters[0], this._currentPlayerId);
						sbyte count = card2.SubEffectParameters[3];
						sbyte value = card2.SubEffectParameters[4];
						Player targetPlayer = this.GetPlayer(targetPlayerId);
						bool flag10 = targetPlayerId == this._currentPlayerId;
						if (flag10)
						{
							bool hasCd = false;
							for (int i = 0; i < 9; i++)
							{
								bool isCd = targetPlayer.BookStates[i].IsCd;
								if (isCd)
								{
									hasCd = true;
									break;
								}
							}
							bool flag11 = !hasCd;
							if (flag11)
							{
								result.Add(cardId3);
							}
						}
					}
				}
			}
			result.RemoveWhere(delegate(sbyte cardId)
			{
				LifeSkillCombatEffectItem card3 = LifeSkillCombatEffect.Instance[cardId];
				return card3.IsInstant && card3.SubEffect != ELifeSkillCombatEffectSubEffect.SelfChangeBookCd;
			});
			return result.ToList<sbyte>();
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x00147510 File Offset: 0x00145710
		public Match.Ai GetAiState(sbyte playerId)
		{
			Match.Ai aiState;
			bool flag = !this._aiStates.TryGetValue(playerId, out aiState) || aiState == null;
			if (flag)
			{
				aiState = (this._aiStates[playerId] = new Match.Ai(this));
			}
			return aiState;
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x00147554 File Offset: 0x00145754
		public OperationBase CalcAiOperation(DataContext context, OperationList usableOperationList)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Start ai process, reference operations count: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(usableOperationList.Count);
			this.RecordLineAi(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
			Match.Ai aiState = this.GetAiState(this._currentPlayerId);
			OperationBase op = aiState.GiveDecidedOperation(context, this);
			aiState.RecordInspect(this);
			return op;
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x001475BC File Offset: 0x001457BC
		public void SwitchCurrentPlayerProcess(DataContext context, IList<StatusSnapshotDiff.BookStateExtraDiff> bookStateExtraDiffs, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs, bool withProcessActiveOperationCells = true)
		{
			if (withProcessActiveOperationCells)
			{
				this.ProcessActiveOperationCells(Player.PredefinedId.GetTheOtherSide(this._currentPlayerId), gridTrapStateExtraDiffs);
			}
			this.RecalculateThesisPoints();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 1);
			defaultInterpolatedStringHandler.AppendLiteral("TURN ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.PlayerSwitchCount);
			defaultInterpolatedStringHandler.AppendLiteral(" IS OVER");
			this.RecordLine(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
			Match.Ai aiState;
			bool flag = this._aiStates.TryGetValue(this._currentPlayerId, out aiState);
			if (flag)
			{
				aiState.TurnClear();
			}
			this._currentPlayerId = Player.PredefinedId.GetTheOtherSide(this._currentPlayerId);
			Player player = this.GetPlayer(this._currentPlayerId);
			player.UpdateBooksCd(this, bookStateExtraDiffs);
			player.UpdateBooksDisplayCd(this, bookStateExtraDiffs);
			player.RecruitEffectCards(context.Random, this, 1);
			this.PlayerSwitchCount++;
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x001476A4 File Offset: 0x001458A4
		private void ProcessActiveOperationCells(sbyte playerId, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs)
		{
			foreach (Grid grid in this._gridStatus)
			{
				bool flag = grid.ActiveOperation == null || grid.ActiveOperation.PlayerId != playerId;
				if (!flag)
				{
					grid.SetActiveOperation(null, this, gridTrapStateExtraDiffs);
				}
			}
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x001476FC File Offset: 0x001458FC
		private void RecalculateThesisPoints()
		{
			foreach (Grid grid in this._gridStatus)
			{
				OperationGridBase w = grid.GetThesis();
				bool flag = w == null;
				if (flag)
				{
					w = grid.ActiveOperation;
				}
				OperationPointBase pOp = w as OperationPointBase;
				bool flag2 = pOp != null;
				if (flag2)
				{
					pOp.AdditionalPoint = 0;
				}
			}
			foreach (Grid grid2 in this._gridStatus)
			{
				OperationPointBase thesis = grid2.GetThesis();
				bool flag3 = thesis == null;
				if (!flag3)
				{
					foreach (sbyte e in thesis.EffectiveEffectCardTemplateIds)
					{
						sbyte b = this.CalcTargetPlayerId(LifeSkillCombatEffect.Instance[e].SubEffectParameters[0], thesis.PlayerId);
						sbyte b2 = LifeSkillCombatEffect.Instance[e].SubEffectParameters[1];
						sbyte b3 = LifeSkillCombatEffect.Instance[e].SubEffectParameters[2];
						sbyte value = LifeSkillCombatEffect.Instance[e].SubEffectParameters[3];
						sbyte cross = b3;
						sbyte straight = b2;
						sbyte house = b;
						ELifeSkillCombatEffectSubEffect subEffect = LifeSkillCombatEffect.Instance[e].SubEffect;
						ELifeSkillCombatEffectSubEffect elifeSkillCombatEffectSubEffect = subEffect;
						if (elifeSkillCombatEffectSubEffect == ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam)
						{
							foreach (Coordinate2D<sbyte> target in Match.OffsetIterate(grid2.Coordinate, straight, cross, null))
							{
								Grid c = this.GetGrid(target);
								OperationGridBase w2 = c.GetThesis();
								bool flag4 = w2 == null;
								if (flag4)
								{
									w2 = c.ActiveOperation;
								}
								if (w2 == null)
								{
									goto IL_18D;
								}
								OperationPointBase pOp2 = w2 as OperationPointBase;
								if (pOp2 == null)
								{
									goto IL_18D;
								}
								bool flag5 = w2.PlayerId == house;
								IL_18E:
								bool flag6 = flag5;
								if (flag6)
								{
									pOp2.AdditionalPoint += (int)(value * 10);
								}
								continue;
								IL_18D:
								flag5 = false;
								goto IL_18E;
							}
						}
					}
				}
			}
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x00147930 File Offset: 0x00145B30
		public StatusSnapshot Dump()
		{
			sbyte winnerPlayerId;
			bool flag = !this.CheckResult(out winnerPlayerId);
			if (flag)
			{
				winnerPlayerId = -1;
			}
			return new StatusSnapshot(this, this._gridStatus, this._playerSelf, this._playerAdversary, this._currentPlayerId, winnerPlayerId);
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x00147974 File Offset: 0x00145B74
		public unsafe void Restore(StatusSnapshot snapshot)
		{
			for (int i = 0; i < 49; i++)
			{
				this._gridStatus[i] = Serializer.CreateCopy<Grid>(snapshot.GridStatus[i]);
			}
			byte[] buffer = new byte[snapshot.Self.GetSerializedSize()];
			byte[] array;
			byte* pData;
			if ((array = buffer) == null || array.Length == 0)
			{
				pData = null;
			}
			else
			{
				pData = &array[0];
			}
			snapshot.Self.Serialize(pData);
			this._playerSelf.Deserialize(pData);
			array = null;
			byte[] buffer2 = new byte[snapshot.Adversary.GetSerializedSize()];
			byte* pData2;
			if ((array = buffer2) == null || array.Length == 0)
			{
				pData2 = null;
			}
			else
			{
				pData2 = &array[0];
			}
			snapshot.Adversary.Serialize(pData2);
			this._playerAdversary.Deserialize(pData2);
			array = null;
			this._currentPlayerId = snapshot.CurrentPlayerId;
			this.PlayerSwitchCount = snapshot.PlayerSwitchCount;
			Tester.Assert(this.CalcPlayerScore(0) == snapshot.ScoreSelf, "");
			Tester.Assert(this.CalcPlayerScore(1) == snapshot.ScoreAdversary, "");
			sbyte winnerPlayerId;
			bool flag = this.CheckResult(out winnerPlayerId);
			if (flag)
			{
				Tester.Assert(winnerPlayerId == snapshot.WinnerPlayerId, "");
			}
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x00147ABF File Offset: 0x00145CBF
		public void SimulateOperationCommit(DataContext context, OperationBase operation, IList<StatusSnapshotDiff.BookStateExtraDiff> bookStateExtraDiffs, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs)
		{
			Tester.Assert(this._beforeSimulated == null, "");
			this._beforeSimulated = this.Dump();
			this.CommitOperationProcess(context, operation, bookStateExtraDiffs, gridTrapStateExtraDiffs, true);
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x00147AEF File Offset: 0x00145CEF
		public void SimulateOperationCancel(DataContext context)
		{
			Tester.Assert(this._beforeSimulated != null, "");
			this.Restore(this._beforeSimulated);
			this._beforeSimulated = null;
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x00147B1C File Offset: 0x00145D1C
		public void RecordLine(string message, int indent = 0)
		{
			for (int i = 0; i < indent; i++)
			{
				this._recorder.Append("  ");
			}
			this._recorder.AppendLine(message);
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x00147B58 File Offset: 0x00145D58
		public void RecordDiff(StatusSnapshotDiff diff)
		{
			int indent = 0;
			this.RecordLine("-----------------------------------------------------", indent);
			this.RecordLine("State changed", indent++);
			int i = 0;
			int len = diff.GridStatusDiffPrevious.Count;
			while (i < len)
			{
				Grid previous = diff.GridStatusDiffPrevious[i];
				Grid current = diff.GridStatusDiffCurrent[i];
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Grid status ");
				defaultInterpolatedStringHandler.AppendFormatted<Coordinate2D<sbyte>>(current.Coordinate);
				this.RecordLine(defaultInterpolatedStringHandler.ToStringAndClear(), indent);
				indent++;
				this.RecordLine(Match.<RecordDiff>g__GridInspect|59_1(previous) ?? "", indent);
				this.RecordLine("-> changed to", indent);
				this.RecordLine(Match.<RecordDiff>g__GridInspect|59_1(current) ?? "", indent);
				indent--;
				i++;
			}
			bool flag = diff.ScoreSelfDiff != null;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 2);
				defaultInterpolatedStringHandler.AppendFormatted(Match.<RecordDiff>g__PlayerName|59_0(0));
				defaultInterpolatedStringHandler.AppendLiteral(" score -> ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(diff.ScoreSelfDiff.Value);
				this.RecordLine(defaultInterpolatedStringHandler.ToStringAndClear(), indent);
			}
			bool flag2 = diff.ScoreAdversaryDiff != null;
			if (flag2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 2);
				defaultInterpolatedStringHandler.AppendFormatted(Match.<RecordDiff>g__PlayerName|59_0(1));
				defaultInterpolatedStringHandler.AppendLiteral(" score -> ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(diff.ScoreAdversaryDiff.Value);
				this.RecordLine(defaultInterpolatedStringHandler.ToStringAndClear(), indent);
			}
			bool flag3 = diff.WinnerPlayerId != null;
			if (flag3)
			{
				this.RecordLine("Yield winner -> " + Match.<RecordDiff>g__PlayerName|59_0(diff.WinnerPlayerId.Value), indent);
			}
			this.RecordLine("-----------------------------------------------------", indent - 1);
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x00147D38 File Offset: 0x00145F38
		public void RecordLineAi(string message, int indent = 0)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 2);
			defaultInterpolatedStringHandler.AppendLiteral("[House[");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this._currentPlayerId);
			defaultInterpolatedStringHandler.AppendLiteral("] Ai]: ");
			defaultInterpolatedStringHandler.AppendFormatted(message);
			this.RecordLine(defaultInterpolatedStringHandler.ToStringAndClear(), indent);
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x00147D90 File Offset: 0x00145F90
		public void Assert(bool condition, string message = "")
		{
			bool flag = !condition;
			if (flag)
			{
				this.RecordLine("--- Assertion failure --- " + message, 0);
				this.GenerateRecordFile();
				throw new Exception("Assertion failure. " + message);
			}
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x00147DD4 File Offset: 0x00145FD4
		public void GenerateRecordFile()
		{
			try
			{
				List<string> logPaths = new List<string>();
				logPaths.AddRange(Directory.EnumerateFiles("../Logs", "LifeSkillCombatLog_*", SearchOption.TopDirectoryOnly));
				bool flag = logPaths.Count > 20;
				if (flag)
				{
					logPaths.Sort(delegate(string pathL, string pathR)
					{
						DateTime timeL = File.GetLastWriteTime(pathL);
						DateTime timeR = File.GetLastWriteTime(pathR);
						return timeL.CompareTo(timeR);
					});
					for (int index = 0; index < logPaths.Count - 20; index++)
					{
						File.Delete(logPaths[index]);
					}
				}
			}
			catch (Exception e)
			{
				AdaptableLog.TagWarning("LifeSkillCombat Log", e.Message, false);
			}
			StringBuilder filePathBuilder = new StringBuilder();
			filePathBuilder.Append("../Logs/LifeSkillCombatLog_");
			StringBuilder stringBuilder = filePathBuilder;
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder);
			appendInterpolatedStringHandler.AppendFormatted<DateTime>(DateTime.Now, "yyyy-MM-dd_HH_mm_ss");
			stringBuilder2.Append(ref appendInterpolatedStringHandler);
			stringBuilder = filePathBuilder;
			StringBuilder stringBuilder3 = stringBuilder;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder);
			appendInterpolatedStringHandler.AppendLiteral("[");
			appendInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(DomainManager.Character.GetElement_Objects(this._playerSelf.CharacterId));
			appendInterpolatedStringHandler.AppendLiteral("]");
			stringBuilder3.Append(ref appendInterpolatedStringHandler);
			stringBuilder = filePathBuilder;
			StringBuilder stringBuilder4 = stringBuilder;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder);
			appendInterpolatedStringHandler.AppendLiteral("[");
			appendInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(DomainManager.Character.GetElement_Objects(this._playerAdversary.CharacterId));
			appendInterpolatedStringHandler.AppendLiteral("]");
			stringBuilder4.Append(ref appendInterpolatedStringHandler);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
			defaultInterpolatedStringHandler.AppendFormatted<StringBuilder>(filePathBuilder);
			defaultInterpolatedStringHandler.AppendLiteral(".txt");
			File.WriteAllText(defaultInterpolatedStringHandler.ToStringAndClear(), this._recorder.ToString());
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x001480AC File Offset: 0x001462AC
		[CompilerGenerated]
		internal static bool <GetNotUsableEffectCardTemplateIds>g__IsSpecialCard|41_0(sbyte cardId)
		{
			return LifeSkillCombatEffect.Instance[cardId].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionAroundHouseThesisLow || LifeSkillCombatEffect.Instance[cardId].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndTransition || LifeSkillCombatEffect.Instance[cardId].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionAroundHouseThesisBreakWhenAdversaryThesisHigh || LifeSkillCombatEffect.Instance[cardId].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndRecycleCardAndExchangeOperation;
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x00148110 File Offset: 0x00146310
		[CompilerGenerated]
		internal static bool <GetNotUsableEffectCardTemplateIds>g__IsCardUsableInOperations|41_1(sbyte cardId, OperationList operations)
		{
			foreach (OperationBase op in operations)
			{
				OperationPointBase pOp = op as OperationPointBase;
				bool flag = pOp == null;
				if (!flag)
				{
					bool flag2 = pOp.PickEffectiveEffectCards(Enumerable.Repeat<sbyte>(cardId, 1)).Contains(cardId);
					if (flag2)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x00148198 File Offset: 0x00146398
		[CompilerGenerated]
		internal static string <RecordDiff>g__PlayerName|59_0(sbyte playerId)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Player[");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(playerId);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x001481D8 File Offset: 0x001463D8
		[CompilerGenerated]
		internal static string <RecordDiff>g__GridInspect|59_1(Grid grid)
		{
			OperationGridBase active = grid.ActiveOperation;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			string text;
			if (active != null)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[Current ");
				defaultInterpolatedStringHandler.AppendFormatted("ActiveOperation");
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<OperationGridBase>(active);
				defaultInterpolatedStringHandler.AppendLiteral("]");
				text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				text = string.Empty;
			}
			string op = text;
			OperationPointBase thesis = grid.GetThesis();
			string text2;
			if (thesis != null)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
				defaultInterpolatedStringHandler.AppendLiteral("[Current Thesis: ");
				defaultInterpolatedStringHandler.AppendFormatted<OperationPointBase>(thesis);
				defaultInterpolatedStringHandler.AppendLiteral("]");
				text2 = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				text2 = string.Empty;
			}
			string thesisOp = text2;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 3);
			defaultInterpolatedStringHandler.AppendLiteral("[Stacked operations count: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(grid.HistoryOperations().Count<OperationGridBase>());
			defaultInterpolatedStringHandler.AppendLiteral("]");
			defaultInterpolatedStringHandler.AppendFormatted(thesisOp);
			defaultInterpolatedStringHandler.AppendFormatted(op);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000337 RID: 823
		public readonly sbyte LifeSkillType;

		// Token: 0x04000338 RID: 824
		private readonly Grid[] _gridStatus = new Grid[49];

		// Token: 0x04000339 RID: 825
		private sbyte _currentPlayerId;

		// Token: 0x0400033A RID: 826
		private sbyte? _suiciderPlayerId;

		// Token: 0x0400033B RID: 827
		private bool _lastOperationIsSilent;

		// Token: 0x0400033D RID: 829
		public sbyte AcceptSilentPlayerId;

		// Token: 0x0400033E RID: 830
		private readonly Dictionary<sbyte, Match.Ai> _aiStates = new Dictionary<sbyte, Match.Ai>();

		// Token: 0x0400033F RID: 831
		private readonly Player _playerSelf;

		// Token: 0x04000340 RID: 832
		private readonly Player _playerAdversary;

		// Token: 0x04000341 RID: 833
		[TupleElementNames(new string[]
		{
			"X",
			"Y"
		})]
		private static readonly ValueTuple<sbyte, sbyte>[] _Offset8 = new ValueTuple<sbyte, sbyte>[]
		{
			new ValueTuple<sbyte, sbyte>(0, 1),
			new ValueTuple<sbyte, sbyte>(0, -1),
			new ValueTuple<sbyte, sbyte>(1, 0),
			new ValueTuple<sbyte, sbyte>(-1, 0),
			new ValueTuple<sbyte, sbyte>(1, 1),
			new ValueTuple<sbyte, sbyte>(-1, -1),
			new ValueTuple<sbyte, sbyte>(1, -1),
			new ValueTuple<sbyte, sbyte>(-1, 1)
		};

		// Token: 0x04000342 RID: 834
		private static readonly Coordinate2D<sbyte>[] OffsetStraight = new Coordinate2D<sbyte>[]
		{
			new Coordinate2D<sbyte>(1, 0),
			new Coordinate2D<sbyte>(-1, 0),
			new Coordinate2D<sbyte>(0, 1),
			new Coordinate2D<sbyte>(0, -1)
		};

		// Token: 0x04000343 RID: 835
		private static readonly Coordinate2D<sbyte>[] OffsetCross = new Coordinate2D<sbyte>[]
		{
			new Coordinate2D<sbyte>(1, 1),
			new Coordinate2D<sbyte>(-1, -1),
			new Coordinate2D<sbyte>(1, -1),
			new Coordinate2D<sbyte>(-1, 1)
		};

		// Token: 0x04000345 RID: 837
		public readonly Dictionary<sbyte, sbyte> PlayerForcedSecretInformation = new Dictionary<sbyte, sbyte>();

		// Token: 0x04000346 RID: 838
		private StatusSnapshot _beforeSimulated;

		// Token: 0x04000347 RID: 839
		public const byte BoardWidth = 7;

		// Token: 0x04000348 RID: 840
		public const byte BoardRadius = 3;

		// Token: 0x04000349 RID: 841
		public const byte BoardGridCount = 49;

		// Token: 0x0400034A RID: 842
		public const int ScoreForWinner = 12;

		// Token: 0x0400034B RID: 843
		private const int MaxStatisticsFile = 20;

		// Token: 0x0400034C RID: 844
		private readonly StringBuilder _recorder = new StringBuilder();

		// Token: 0x02000977 RID: 2423
		public class Ai
		{
			// Token: 0x06008470 RID: 33904 RVA: 0x004E24FC File Offset: 0x004E06FC
			public Ai(Match match)
			{
				Player player = match.GetPlayer(match._currentPlayerId);
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(player.CharacterId);
				this.ChangeBehaviourTypeStyle(character.GetBehaviorType(), match);
				this._playerId = player.Id;
				this.AlwaysUseForceAdversary = false;
			}

			// Token: 0x06008471 RID: 33905 RVA: 0x004E2583 File Offset: 0x004E0783
			public void TurnClear()
			{
				this._canUseForceAdversary = true;
			}

			// Token: 0x06008472 RID: 33906 RVA: 0x004E258D File Offset: 0x004E078D
			public void ChangeBehaviourTypeStyle(sbyte behaviourType, Match match)
			{
				this._behaviourTypeStyle = behaviourType;
				match.RecordLineAi("BehaviourType changed to " + Config.BehaviorType.Instance[(short)this._behaviourTypeStyle].Name, 0);
			}

			// Token: 0x06008473 RID: 33907 RVA: 0x004E25C0 File Offset: 0x004E07C0
			public void InferringForOpponentBookPoints(IRandomSource random, Match match)
			{
				sbyte playerId = match._currentPlayerId;
				sbyte opponentId = Player.PredefinedId.GetTheOtherSide(playerId);
				Player opponentPlayer = match.GetPlayer(opponentId);
				Dictionary<sbyte, SortedSet<int>> recordBefore = new Dictionary<sbyte, SortedSet<int>>();
				for (sbyte i = 0; i < 9; i += 1)
				{
					SortedSet<int> points;
					bool flag = this.TryGetOpponentBookPoint(match, i, out points);
					if (flag)
					{
						SortedSet<int> record = new SortedSet<int>();
						record.UnionWith(points);
						recordBefore.Add(i, record);
					}
				}
				HashSet<sbyte> bookIndexSet = new HashSet<sbyte>();
				foreach (Grid grid in match._gridStatus)
				{
					OperationGridBase op = grid.ActiveOperation;
					bool flag2 = op != null;
					if (!flag2)
					{
						OperationPointBase pOp = grid.LastHistoryOperation as OperationPointBase;
						bool flag3 = pOp == null || pOp.PlayerId == playerId;
						if (!flag3)
						{
							bool flag4 = pOp.EffectiveBookStates.Count == 1;
							if (flag4)
							{
								this._opponentPreciselyBookPointImpressions[opponentPlayer.RefBookIndex(pOp.EffectiveBookStates[0])] = pOp.Point;
							}
							int remainPoint = pOp.Point;
							bookIndexSet.Clear();
							foreach (Book bookState in pOp.EffectiveBookStates)
							{
								bookIndexSet.Add(opponentPlayer.RefBookIndex(bookState));
							}
							bool flag5 = bookIndexSet.Count == 0 || this._ignoredOperations.Contains(pOp.GridIndex);
							if (!flag5)
							{
								this._ignoredOperations.Add(pOp.GridIndex);
								foreach (KeyValuePair<sbyte, int> keyValuePair in this._opponentPreciselyBookPointImpressions)
								{
									sbyte b;
									int num;
									keyValuePair.Deconstruct(out b, out num);
									sbyte bookIndex = b;
									int bookPoint = num;
									bool flag6 = bookIndexSet.Contains(bookIndex);
									if (flag6)
									{
										bookIndexSet.Remove(bookIndex);
										remainPoint -= bookPoint;
									}
								}
								remainPoint = Math.Max(remainPoint, 0);
								bool flag7 = bookIndexSet.Count == 1;
								if (flag7)
								{
									foreach (sbyte bookIndex2 in bookIndexSet)
									{
										this._opponentPreciselyBookPointImpressions[bookIndex2] = remainPoint;
									}
								}
								else
								{
									foreach (sbyte bookIndex3 in bookIndexSet)
									{
										SortedSet<int> points2;
										bool flag8 = !this._opponentVolatileBookPointImpressions.TryGetValue(bookIndex3, out points2);
										if (flag8)
										{
											this._opponentVolatileBookPointImpressions.Add(bookIndex3, points2 = new SortedSet<int>());
										}
										points2.Add(remainPoint / bookIndexSet.Count);
									}
								}
							}
						}
					}
				}
				match.RecordLineAi("Recorded book point after this time inferring：", 0);
				for (sbyte j = 0; j < 9; j += 1)
				{
					SortedSet<int> oldPoints;
					bool hasBefore = recordBefore.TryGetValue(j, out oldPoints);
					SortedSet<int> currentPoints;
					bool hasCurrent = this.TryGetOpponentBookPoint(match, j, out currentPoints);
					string before = hasBefore ? string.Join<int>(", ", recordBefore[j]) : string.Empty;
					string current = hasCurrent ? string.Join<int>(", ", currentPoints) : string.Empty;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 3);
					defaultInterpolatedStringHandler.AppendFormatted(LifeSkill.Instance[opponentPlayer.BookStates[(int)j].LifeSkill.SkillTemplateId].Name);
					defaultInterpolatedStringHandler.AppendLiteral(" {");
					defaultInterpolatedStringHandler.AppendFormatted(before);
					defaultInterpolatedStringHandler.AppendLiteral("} -> {");
					defaultInterpolatedStringHandler.AppendFormatted(current);
					defaultInterpolatedStringHandler.AppendLiteral("}");
					match.RecordLineAi(defaultInterpolatedStringHandler.ToStringAndClear(), 1);
				}
			}

			// Token: 0x06008474 RID: 33908 RVA: 0x004E29E8 File Offset: 0x004E0BE8
			public bool TryGetOpponentBookPoint(Match match, sbyte bookIndex, out SortedSet<int> points)
			{
				int point;
				bool flag = this._opponentPreciselyBookPointImpressions.TryGetValue(bookIndex, out point);
				bool result;
				if (flag)
				{
					this._opponentVolatileBookPointImpressions.Remove(bookIndex);
					points = new SortedSet<int>();
					points.Add(point);
					result = true;
				}
				else
				{
					bool flag2 = this._opponentVolatileBookPointImpressions.TryGetValue(bookIndex, out points) && points != null && points.Count > 0;
					if (flag2)
					{
						result = true;
					}
					else
					{
						sbyte opponentPlayerId = Player.PredefinedId.GetTheOtherSide(match._currentPlayerId);
						GameData.Domains.Character.Character opponentCharacter = DomainManager.Character.GetElement_Objects(match.GetPlayerCharacterId(opponentPlayerId));
						switch (this._behaviourTypeStyle)
						{
						case 0:
							points = new SortedSet<int>
							{
								(int)(opponentCharacter.GetLifeSkillAttainment(match.LifeSkillType) * 4 + opponentCharacter.GetLifeSkillQualification(match.LifeSkillType))
							};
							result = false;
							break;
						case 1:
							points = new SortedSet<int>
							{
								(int)(opponentCharacter.GetLifeSkillAttainment(match.LifeSkillType) * 5 + opponentCharacter.GetLifeSkillQualification(match.LifeSkillType))
							};
							result = false;
							break;
						case 2:
							points = new SortedSet<int>
							{
								(int)(opponentCharacter.GetLifeSkillAttainment(match.LifeSkillType) * 4 + opponentCharacter.GetLifeSkillQualification(match.LifeSkillType))
							};
							result = false;
							break;
						case 3:
							points = new SortedSet<int>
							{
								(int)(opponentCharacter.GetLifeSkillAttainment(match.LifeSkillType) * 3 + opponentCharacter.GetLifeSkillQualification(match.LifeSkillType))
							};
							result = false;
							break;
						case 4:
							points = new SortedSet<int>
							{
								(int)(opponentCharacter.GetLifeSkillAttainment(match.LifeSkillType) * 3 + opponentCharacter.GetLifeSkillQualification(match.LifeSkillType))
							};
							result = false;
							break;
						default:
							throw new NotImplementedException();
						}
					}
				}
				return result;
			}

			// Token: 0x06008475 RID: 33909 RVA: 0x004E2B9C File Offset: 0x004E0D9C
			public int GetPlayerBookSetPoint(Match match, ushort bookSet, Player player)
			{
				int point = 0;
				IReadOnlyList<Book> bookStates = player.BookStates;
				for (sbyte i = 0; i < 9; i += 1)
				{
					bool flag = (1 << (int)i & (int)bookSet) == 0;
					if (!flag)
					{
						point += bookStates[(int)i].BasePoint;
					}
				}
				return point;
			}

			// Token: 0x06008476 RID: 33910 RVA: 0x004E2BF0 File Offset: 0x004E0DF0
			public int GetCurrentPlayerBookSetPoint(Match match, ushort bookSet)
			{
				return this.GetPlayerBookSetPoint(match, bookSet, match.GetPlayer(match._currentPlayerId));
			}

			// Token: 0x06008477 RID: 33911 RVA: 0x004E2C06 File Offset: 0x004E0E06
			public IEnumerable<Book> GetBookStatesByBookSet(ushort bookSet, Player player)
			{
				IReadOnlyList<Book> bookStates = player.BookStates;
				sbyte b;
				for (sbyte i = 0; i < 9; i = b + 1)
				{
					bool flag = (1 << (int)i & (int)bookSet) == 0;
					if (!flag)
					{
						yield return bookStates[(int)i];
					}
					b = i;
				}
				yield break;
			}

			// Token: 0x06008478 RID: 33912 RVA: 0x004E2C24 File Offset: 0x004E0E24
			public ushort GetOpponentUsableBookSetInNext(Match match)
			{
				Player opponentPlayer = match.GetPlayer(Player.PredefinedId.GetTheOtherSide(match._currentPlayerId));
				IReadOnlyList<Book> bookStates = opponentPlayer.BookStates;
				int origin = 0;
				int i = 0;
				int count = 9;
				while (i < count)
				{
					bool flag = !bookStates[i].IsDisplayCd || bookStates[i].DisplayCd <= 1;
					if (flag)
					{
						origin |= 1 << i;
					}
					i++;
				}
				return (ushort)origin;
			}

			// Token: 0x06008479 RID: 33913 RVA: 0x004E2CA8 File Offset: 0x004E0EA8
			public List<int> GetOpponentBookSetPossiblePoints(Match match, ushort bookSet)
			{
				List<int> result = new List<int>();
				for (sbyte i = 0; i < 9; i += 1)
				{
					bool flag = (1 << (int)i & (int)bookSet) == 0;
					if (!flag)
					{
						SortedSet<int> singleBookPossiblePoints;
						this.TryGetOpponentBookPoint(match, i, out singleBookPossiblePoints);
						foreach (int p in singleBookPossiblePoints)
						{
							result.Add(p);
						}
						int j = 0;
						int jCount = result.Count - singleBookPossiblePoints.Count;
						while (j < jCount)
						{
							foreach (int p2 in singleBookPossiblePoints)
							{
								result.Add(result[j] + p2);
							}
							j++;
						}
					}
				}
				result = result.ToHashSet<int>().ToList<int>();
				result.Sort();
				return result;
			}

			// Token: 0x0600847A RID: 33914 RVA: 0x004E2DCC File Offset: 0x004E0FCC
			public ushort GetOperationBookSets(OperationPointBase pOp, Match match)
			{
				ushort result = 0;
				IReadOnlyList<Book> effectiveBookStates = pOp.EffectiveBookStates;
				IReadOnlyList<Book> bookStates = match.GetPlayer(pOp.PlayerId).BookStates;
				for (int i = 0; i < effectiveBookStates.Count; i++)
				{
					Book effectiveBookState = effectiveBookStates[i];
					for (int j = 0; j < bookStates.Count; j++)
					{
						bool flag = bookStates[j].LifeSkill.SkillTemplateId == effectiveBookState.LifeSkill.SkillTemplateId;
						if (flag)
						{
							result |= (ushort)(1 << j);
							break;
						}
					}
				}
				return result;
			}

			// Token: 0x0600847B RID: 33915 RVA: 0x004E2E70 File Offset: 0x004E1070
			public ushort GetOriginalUsableBookSets(Player player)
			{
				IReadOnlyList<Book> bookStates = player.BookStates;
				int origin = 0;
				int i = 0;
				int count = 9;
				while (i < count)
				{
					bool flag = !bookStates[i].IsCd;
					if (flag)
					{
						origin |= 1 << i;
					}
					i++;
				}
				return (ushort)origin;
			}

			// Token: 0x0600847C RID: 33916 RVA: 0x004E2EC8 File Offset: 0x004E10C8
			public List<ushort> GetPossibleBookSets(ushort origin)
			{
				List<ushort> result = new List<ushort>();
				int i = 0;
				int count = 9;
				while (i < count)
				{
					ushort currentPosition = (ushort)(1 << i);
					bool flag = (currentPosition & origin) == 0;
					if (!flag)
					{
						result.Add(currentPosition);
						int j = 0;
						int jCount = result.Count;
						while (j < jCount)
						{
							result.Add(result[j] | currentPosition);
							j++;
						}
					}
					i++;
				}
				return result.ToHashSet<ushort>().ToList<ushort>();
			}

			// Token: 0x0600847D RID: 33917 RVA: 0x004E2F58 File Offset: 0x004E1158
			public List<ushort> GetUsableBookSets(Player player)
			{
				ushort origin = this.GetOriginalUsableBookSets(player);
				return this.GetPossibleBookSets(origin);
			}

			// Token: 0x0600847E RID: 33918 RVA: 0x004E2F7C File Offset: 0x004E117C
			public OperationBase GiveDecidedOperation(DataContext context, Match match)
			{
				Match.Ai.<>c__DisplayClass24_0 CS$<>8__locals1 = new Match.Ai.<>c__DisplayClass24_0();
				CS$<>8__locals1.match = match;
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.playerId = CS$<>8__locals1.match._currentPlayerId;
				sbyte opponentPlayerId = Player.PredefinedId.GetTheOtherSide(CS$<>8__locals1.playerId);
				CS$<>8__locals1.player = CS$<>8__locals1.match.GetPlayer(CS$<>8__locals1.playerId);
				CS$<>8__locals1.opponentPlayer = CS$<>8__locals1.match.GetPlayer(opponentPlayerId);
				CS$<>8__locals1.character = DomainManager.Character.GetElement_Objects(CS$<>8__locals1.match.GetPlayerCharacterId(CS$<>8__locals1.playerId));
				CS$<>8__locals1.random = context.Random;
				this.InferringForOpponentBookPoints(CS$<>8__locals1.random, CS$<>8__locals1.match);
				sbyte behaviorType = CS$<>8__locals1.character.GetBehaviorType();
				sbyte b = behaviorType;
				if (b != 0)
				{
					if (b == 4)
					{
						bool flag = this._behaviourTypeStyle != 0 && CS$<>8__locals1.match.CalcPlayerScore(CS$<>8__locals1.playerId) - CS$<>8__locals1.match.CalcPlayerScore(opponentPlayerId) >= 5;
						if (flag)
						{
							this.ChangeBehaviourTypeStyle(0, CS$<>8__locals1.match);
						}
						else
						{
							bool flag2 = this._behaviourTypeStyle == 0 && CS$<>8__locals1.match.CalcPlayerScore(opponentPlayerId) >= CS$<>8__locals1.match.CalcPlayerScore(CS$<>8__locals1.playerId);
							if (flag2)
							{
								this.ChangeBehaviourTypeStyle(4, CS$<>8__locals1.match);
							}
						}
					}
				}
				else
				{
					bool flag3 = this._behaviourTypeStyle != 4 && CS$<>8__locals1.match.CalcPlayerScore(opponentPlayerId) - CS$<>8__locals1.match.CalcPlayerScore(CS$<>8__locals1.playerId) >= 5;
					if (flag3)
					{
						this.ChangeBehaviourTypeStyle(4, CS$<>8__locals1.match);
					}
					else
					{
						bool flag4 = this._behaviourTypeStyle == 4 && CS$<>8__locals1.match.CalcPlayerScore(CS$<>8__locals1.playerId) >= CS$<>8__locals1.match.CalcPlayerScore(opponentPlayerId);
						if (flag4)
						{
							this.ChangeBehaviourTypeStyle(0, CS$<>8__locals1.match);
						}
					}
				}
				CS$<>8__locals1.additionalEffectCardTemplateIds = new List<sbyte>();
				Match.Ai.<>c__DisplayClass24_1 CS$<>8__locals2;
				CS$<>8__locals2.ratePerGroup = CS$<>8__locals1.<GiveDecidedOperation>g__GetCardRatePerGroup|3();
				this._banCardTemplateIds.Clear();
				List<sbyte> effectCardsList = new List<sbyte>();
				effectCardsList.AddRange(CS$<>8__locals1.player.EffectCards);
				CollectionUtils.Shuffle<sbyte>(CS$<>8__locals1.random, effectCardsList);
				bool flag5 = CS$<>8__locals1.random.CheckPercentProb(CS$<>8__locals2.ratePerGroup[ELifeSkillCombatEffectGroup.FlexibleFall]);
				if (flag5)
				{
					CS$<>8__locals2.ratePerGroup[ELifeSkillCombatEffectGroup.FlexibleFall] = 0;
					foreach (sbyte cardId in effectCardsList)
					{
						LifeSkillCombatEffectItem config = LifeSkillCombatEffect.Instance[cardId];
						bool flag6 = config.Group == ELifeSkillCombatEffectGroup.FlexibleFall && CS$<>8__locals1.<GiveDecidedOperation>g__TryUseCardDone|10(cardId, ref CS$<>8__locals2);
						if (flag6)
						{
							break;
						}
					}
				}
				List<ELifeSkillCombatEffectGroup> groupList = new List<ELifeSkillCombatEffectGroup>();
				groupList.AddRange(CS$<>8__locals2.ratePerGroup.Keys);
				CollectionUtils.Shuffle<ELifeSkillCombatEffectGroup>(CS$<>8__locals1.random, groupList);
				foreach (ELifeSkillCombatEffectGroup group in groupList)
				{
					bool flag7 = CS$<>8__locals1.random.CheckPercentProb(CS$<>8__locals2.ratePerGroup[group]);
					if (flag7)
					{
						foreach (sbyte cardId2 in effectCardsList)
						{
							LifeSkillCombatEffectItem config2 = LifeSkillCombatEffect.Instance[cardId2];
							bool flag8 = config2.Group == group;
							if (flag8)
							{
								bool flag9 = CS$<>8__locals1.<GiveDecidedOperation>g__TryUseCardDone|10(cardId2, ref CS$<>8__locals2);
								if (flag9)
								{
									break;
								}
							}
						}
					}
				}
				bool flag10;
				OperationList usableOperationList = CS$<>8__locals1.match.CalcUsableOperationList(CS$<>8__locals1.additionalEffectCardTemplateIds, Array.Empty<sbyte>(), Array.Empty<int>(), out flag10, false);
				CS$<>8__locals1.usablePopList = new List<OperationPointBase>();
				foreach (OperationBase op in usableOperationList)
				{
					OperationPointBase pOp = op as OperationPointBase;
					bool flag11 = pOp != null;
					if (flag11)
					{
						CS$<>8__locals1.usablePopList.Add(pOp);
					}
				}
				bool canUseForceAdversary = this._canUseForceAdversary;
				if (canUseForceAdversary)
				{
					this._canUseForceAdversary = false;
					OperationBase forceSilentPrepare = usableOperationList.FirstOrDefault(delegate(OperationBase p)
					{
						OperationPrepareForceAdversary force = p as OperationPrepareForceAdversary;
						return force != null && force.Type == OperationPrepareForceAdversary.ForceAdversaryOperation.Silent;
					});
					bool flag12 = forceSilentPrepare != null;
					if (flag12)
					{
						int prob = 0;
						switch (this._behaviourTypeStyle)
						{
						case 1:
							prob = 1;
							break;
						case 2:
							prob = 2;
							break;
						case 3:
							prob = 3;
							break;
						case 4:
							prob = 5;
							break;
						}
						bool flag13 = DomainManager.Extra.GetAiActionRateAdjust(CS$<>8__locals1.player.CharacterId, 11, -1) != 0;
						if (flag13)
						{
							prob = 0;
						}
						bool alwaysUseForceAdversary = this.AlwaysUseForceAdversary;
						if (alwaysUseForceAdversary)
						{
							prob = 100;
						}
						Match match2 = CS$<>8__locals1.match;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 1);
						defaultInterpolatedStringHandler.AppendLiteral("forceSilentPrepare prob ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(prob);
						defaultInterpolatedStringHandler.AppendLiteral("%");
						match2.RecordLineAi(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
						bool flag14 = CS$<>8__locals1.random.CheckPercentProb(prob);
						if (flag14)
						{
							bool useSecretInformation = false;
							switch (this._behaviourTypeStyle)
							{
							case 1:
								useSecretInformation = CS$<>8__locals1.random.CheckPercentProb(70);
								break;
							case 2:
								useSecretInformation = CS$<>8__locals1.random.CheckPercentProb(50);
								break;
							case 3:
								useSecretInformation = CS$<>8__locals1.random.CheckPercentProb(80);
								break;
							case 4:
								useSecretInformation = CS$<>8__locals1.random.CheckPercentProb(40);
								break;
							}
							bool flag15 = useSecretInformation;
							if (flag15)
							{
								SecretInformationDisplayPackage secretInformation = DomainManager.Taiwu.PickLifeSkillCombatCharacterUseSecretInformation(context, CS$<>8__locals1.player.CharacterId);
								bool flag16 = secretInformation != null && secretInformation.SecretInformationDisplayDataList.Count > 0;
								if (flag16)
								{
									CS$<>8__locals1.player.SetForceSilentRemainingCount(CS$<>8__locals1.player.ForceSilentRemainingCount - 1);
									CS$<>8__locals1.match.PlayerForcedSecretInformation[CS$<>8__locals1.player.Id] = 1;
									return new OperationForceAdversary(CS$<>8__locals1.playerId, CS$<>8__locals1.match.PlayerSwitchCount, secretInformation);
								}
							}
							else
							{
								ItemDisplayData item = DomainManager.Taiwu.PickLifeSkillCombatCharacterUseItem(context, CS$<>8__locals1.player.CharacterId);
								bool flag17 = item != null;
								if (flag17)
								{
									CS$<>8__locals1.player.SetForceSilentRemainingCount(CS$<>8__locals1.player.ForceSilentRemainingCount - 1);
									CS$<>8__locals1.match.PlayerForcedSecretInformation[CS$<>8__locals1.player.Id] = 2;
									return new OperationForceAdversary(CS$<>8__locals1.playerId, CS$<>8__locals1.match.PlayerSwitchCount, item);
								}
							}
						}
					}
				}
				OperationBase result = CS$<>8__locals1.<GiveDecidedOperation>g__DecideFirstPhase|5();
				OperationPointBase pOp2 = result as OperationPointBase;
				bool flag18 = pOp2 != null && pOp2.EffectiveEffectCardTemplateIds.Count == 0;
				if (flag18)
				{
					Dictionary<ELifeSkillCombatEffectGroup, int> ratePerGroup = CS$<>8__locals1.<GiveDecidedOperation>g__GetCardRatePerGroup|3();
					List<sbyte> effectCardsList2 = new List<sbyte>();
					effectCardsList2.AddRange(CS$<>8__locals1.player.EffectCards);
					CollectionUtils.Shuffle<sbyte>(CS$<>8__locals1.random, effectCardsList2);
					List<ELifeSkillCombatEffectGroup> groupList2 = new List<ELifeSkillCombatEffectGroup>();
					groupList2.AddRange(ratePerGroup.Keys);
					CollectionUtils.Shuffle<ELifeSkillCombatEffectGroup>(CS$<>8__locals1.random, groupList2);
					ratePerGroup[ELifeSkillCombatEffectGroup.FlexibleFall] = 0;
					this._banCardTemplateIds.Clear();
					foreach (ELifeSkillCombatEffectGroup group2 in groupList2)
					{
						bool flag19 = CS$<>8__locals1.random.CheckPercentProb(ratePerGroup[group2]);
						if (flag19)
						{
							foreach (sbyte cardId3 in effectCardsList2)
							{
								LifeSkillCombatEffectItem config3 = LifeSkillCombatEffect.Instance[cardId3];
								bool flag20 = config3.Group == group2;
								if (flag20)
								{
									bool flag21 = !this._banCardTemplateIds.Contains(cardId3) && pOp2.PickEffectiveEffectCards(Enumerable.Repeat<sbyte>(cardId3, 1)).Contains(cardId3);
									if (flag21)
									{
										pOp2.RegisterEffectiveEffectCards(cardId3);
										this._banCardTemplateIds.Add(cardId3);
										this._banCardTemplateIds.UnionWith(config3.BanCardList);
										break;
									}
								}
							}
						}
					}
					CS$<>8__locals1.match.RecordLineAi("try using card in second phase.", 0);
				}
				return result;
			}

			// Token: 0x0600847F RID: 33919 RVA: 0x004E3854 File Offset: 0x004E1A54
			public void RecordInspect(Match match)
			{
				Player opponentPlayer = match.GetPlayer(Player.PredefinedId.GetTheOtherSide(match._currentPlayerId));
				int indent = 0;
				match.RecordLineAi("---- Ai inspect begin ----", indent);
				match.RecordLineAi("Current behaviour_type: " + Config.BehaviorType.Instance[(short)this._behaviourTypeStyle].Name, 0);
				match.RecordLineAi("Current ban cards in turn: {" + string.Join(", ", from id in this._banCardTemplateIds
				select LifeSkillCombatEffect.Instance[id].Name) + "}", 0);
				indent++;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Recorded the precisely book_set point of adversary count: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._opponentPreciselyBookPointImpressions.Count);
				match.RecordLineAi(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
				for (sbyte i = 0; i < 9; i += 1)
				{
					SortedSet<int> points;
					bool flag = this.TryGetOpponentBookPoint(match, i, out points);
					if (flag)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 4);
						defaultInterpolatedStringHandler.AppendLiteral("adversary single book point ");
						defaultInterpolatedStringHandler.AppendFormatted(LifeSkill.Instance[opponentPlayer.BookStates[(int)i].LifeSkill.SkillTemplateId].Name);
						defaultInterpolatedStringHandler.AppendLiteral(" memory = {");
						defaultInterpolatedStringHandler.AppendFormatted(string.Join<int>(", ", points));
						defaultInterpolatedStringHandler.AppendLiteral("} (House[");
						defaultInterpolatedStringHandler.AppendFormatted<sbyte>(opponentPlayer.Id);
						defaultInterpolatedStringHandler.AppendLiteral("]'s real point: ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(opponentPlayer.BookStates[(int)i].BasePoint);
						match.RecordLineAi(defaultInterpolatedStringHandler.ToStringAndClear(), indent);
					}
				}
				indent--;
				match.RecordLineAi("---- Ai inspect end ----", indent);
			}

			// Token: 0x06008480 RID: 33920 RVA: 0x004E3A30 File Offset: 0x004E1C30
			public static string GetBookIndicesSetInspect(ushort bookSet, Player player, bool isName = true)
			{
				IReadOnlyList<Book> bookStates = player.BookStates;
				string result = string.Empty;
				bool first = true;
				for (int i = 0; i < 9; i++)
				{
					bool flag = (1 << i & (int)bookSet) == 0;
					if (!flag)
					{
						bool flag2 = !first;
						if (flag2)
						{
							result += ", ";
						}
						string str = result;
						string str2;
						if (!isName)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
							defaultInterpolatedStringHandler.AppendFormatted<int>(i);
							str2 = defaultInterpolatedStringHandler.ToStringAndClear();
						}
						else
						{
							str2 = LifeSkill.Instance[bookStates[i].LifeSkill.SkillTemplateId].Name;
						}
						result = str + str2;
						first = false;
					}
				}
				return "{" + result + "}";
			}

			// Token: 0x06008481 RID: 33921 RVA: 0x004E3AEC File Offset: 0x004E1CEC
			// Note: this type is marked as 'beforefieldinit'.
			static Ai()
			{
				Dictionary<ELifeSkillCombatEffectType, ELifeSkillCombatEffectGroup[]> dictionary = new Dictionary<ELifeSkillCombatEffectType, ELifeSkillCombatEffectGroup[]>();
				dictionary.Add(ELifeSkillCombatEffectType.Common, new ELifeSkillCombatEffectGroup[]
				{
					ELifeSkillCombatEffectGroup.ExtractCard,
					ELifeSkillCombatEffectGroup.FlexibleFall,
					ELifeSkillCombatEffectGroup.EliminateArgument
				});
				dictionary.Add(ELifeSkillCombatEffectType.BUFF, new ELifeSkillCombatEffectGroup[]
				{
					ELifeSkillCombatEffectGroup.ReinforceArgument,
					ELifeSkillCombatEffectGroup.WeakenArgument
				});
				dictionary.Add(ELifeSkillCombatEffectType.Strategy, new ELifeSkillCombatEffectGroup[]
				{
					ELifeSkillCombatEffectGroup.RemoveEffect,
					ELifeSkillCombatEffectGroup.DissembleArgument
				});
				Dictionary<ELifeSkillCombatEffectType, ELifeSkillCombatEffectGroup[]> dictionary2 = dictionary;
				ELifeSkillCombatEffectType key = ELifeSkillCombatEffectType.Cooling;
				ELifeSkillCombatEffectGroup[] array = new ELifeSkillCombatEffectGroup[3];
				RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.BCBC01A5036673E493422616677A83718EDFE475D3E938B1A879903FFB2A05A0).FieldHandle);
				dictionary2.Add(key, array);
				Dictionary<ELifeSkillCombatEffectType, ELifeSkillCombatEffectGroup[]> dictionary3 = dictionary;
				ELifeSkillCombatEffectType key2 = ELifeSkillCombatEffectType.Special;
				ELifeSkillCombatEffectGroup[] array2 = new ELifeSkillCombatEffectGroup[3];
				RuntimeHelpers.InitializeArray(array2, fieldof(<PrivateImplementationDetails>.A56D6BBBE254A23749343FB727E7F348B719BC6314763D6A792843E2F7C466EE).FieldHandle);
				dictionary3.Add(key2, array2);
				Match.Ai.LifeSkillCombatEffectTypeGroups = dictionary;
				Match.Ai.LifeSkillCombatEffectGroupPersonalityTypes = new Dictionary<ELifeSkillCombatEffectGroup, sbyte[]>
				{
					{
						ELifeSkillCombatEffectGroup.ExtractCard,
						new sbyte[1]
					},
					{
						ELifeSkillCombatEffectGroup.FlexibleFall,
						new sbyte[]
						{
							1
						}
					},
					{
						ELifeSkillCombatEffectGroup.EliminateArgument,
						new sbyte[]
						{
							3
						}
					},
					{
						ELifeSkillCombatEffectGroup.ReinforceArgument,
						new sbyte[]
						{
							2,
							1
						}
					},
					{
						ELifeSkillCombatEffectGroup.WeakenArgument,
						new sbyte[]
						{
							3,
							4
						}
					},
					{
						ELifeSkillCombatEffectGroup.RemoveEffect,
						new sbyte[1]
					},
					{
						ELifeSkillCombatEffectGroup.DissembleArgument,
						new sbyte[]
						{
							1
						}
					},
					{
						ELifeSkillCombatEffectGroup.ReduceCooling,
						new sbyte[]
						{
							2
						}
					},
					{
						ELifeSkillCombatEffectGroup.AddCooling,
						new sbyte[]
						{
							4
						}
					},
					{
						ELifeSkillCombatEffectGroup.CompleteCooling,
						new sbyte[]
						{
							3
						}
					},
					{
						ELifeSkillCombatEffectGroup.RecycleCard,
						new sbyte[]
						{
							0,
							2,
							1,
							4
						}
					},
					{
						ELifeSkillCombatEffectGroup.QuickArgument,
						new sbyte[]
						{
							0,
							2,
							1,
							4
						}
					},
					{
						ELifeSkillCombatEffectGroup.CaptureArgument,
						new sbyte[]
						{
							0,
							2,
							1,
							4
						}
					}
				};
			}

			// Token: 0x040027B2 RID: 10162
			private readonly sbyte _playerId;

			// Token: 0x040027B3 RID: 10163
			private readonly Dictionary<sbyte, int> _opponentPreciselyBookPointImpressions = new Dictionary<sbyte, int>();

			// Token: 0x040027B4 RID: 10164
			private readonly Dictionary<sbyte, SortedSet<int>> _opponentVolatileBookPointImpressions = new Dictionary<sbyte, SortedSet<int>>();

			// Token: 0x040027B5 RID: 10165
			private readonly HashSet<int> _ignoredOperations = new HashSet<int>();

			// Token: 0x040027B6 RID: 10166
			private readonly HashSet<sbyte> _banCardTemplateIds = new HashSet<sbyte>();

			// Token: 0x040027B7 RID: 10167
			private sbyte _behaviourTypeStyle = -1;

			// Token: 0x040027B8 RID: 10168
			private bool _canUseForceAdversary;

			// Token: 0x040027B9 RID: 10169
			public bool AlwaysUseForceAdversary;

			// Token: 0x040027BA RID: 10170
			internal static readonly Dictionary<ELifeSkillCombatEffectType, ELifeSkillCombatEffectGroup[]> LifeSkillCombatEffectTypeGroups;

			// Token: 0x040027BB RID: 10171
			internal static readonly Dictionary<ELifeSkillCombatEffectGroup, sbyte[]> LifeSkillCombatEffectGroupPersonalityTypes;
		}
	}
}
