using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Domains.Taiwu.Debate;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.DebateAI
{
	// Token: 0x0200006D RID: 109
	public class DebateAi
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060015EB RID: 5611 RVA: 0x0014CCE9 File Offset: 0x0014AEE9
		private static DebateGame Debate
		{
			get
			{
				return DomainManager.Taiwu.Debate;
			}
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x0014CCF8 File Offset: 0x0014AEF8
		public DebateAi(bool isTaiwu, sbyte behaviorType)
		{
			this._isTaiwu = isTaiwu;
			this._behaviorType = behaviorType;
			this.PrepareStrategyTargetActions = new List<Action<List<StrategyTarget>>>
			{
				new Action<List<StrategyTarget>>(this.SelectTargetMusic1),
				new Action<List<StrategyTarget>>(this.SelectTargetMusic2),
				new Action<List<StrategyTarget>>(this.SelectTargetMusic3),
				new Action<List<StrategyTarget>>(this.SelectTargetChess1),
				new Action<List<StrategyTarget>>(this.SelectTargetChess2),
				new Action<List<StrategyTarget>>(this.SelectTargetChess3),
				new Action<List<StrategyTarget>>(this.SelectTargetPoem1),
				new Action<List<StrategyTarget>>(this.SelectTargetPoem2),
				new Action<List<StrategyTarget>>(this.SelectTargetPoem3),
				new Action<List<StrategyTarget>>(this.SelectTargetPainting1),
				new Action<List<StrategyTarget>>(this.SelectTargetPainting2),
				new Action<List<StrategyTarget>>(this.SelectTargetPainting3),
				new Action<List<StrategyTarget>>(this.SelectTargetMath1),
				new Action<List<StrategyTarget>>(this.SelectTargetMath2),
				new Action<List<StrategyTarget>>(this.SelectTargetMath3),
				new Action<List<StrategyTarget>>(this.SelectTargetAppraisal1),
				new Action<List<StrategyTarget>>(this.SelectTargetAppraisal2),
				new Action<List<StrategyTarget>>(this.SelectTargetAppraisal3),
				new Action<List<StrategyTarget>>(this.SelectTargetForging1),
				new Action<List<StrategyTarget>>(this.SelectTargetForging2),
				new Action<List<StrategyTarget>>(this.SelectTargetForging3),
				new Action<List<StrategyTarget>>(this.SelectTargetWoodworking1),
				new Action<List<StrategyTarget>>(this.SelectTargetWoodworking2),
				new Action<List<StrategyTarget>>(this.SelectTargetWoodworking3),
				new Action<List<StrategyTarget>>(this.SelectTargetMedicine1),
				new Action<List<StrategyTarget>>(this.SelectTargetMedicine2),
				new Action<List<StrategyTarget>>(this.SelectTargetMedicine3),
				new Action<List<StrategyTarget>>(this.SelectTargetToxicology1),
				new Action<List<StrategyTarget>>(this.SelectTargetToxicology2),
				new Action<List<StrategyTarget>>(this.SelectTargetToxicology3),
				new Action<List<StrategyTarget>>(this.SelectTargetWeaving1),
				new Action<List<StrategyTarget>>(this.SelectTargetWeaving2),
				new Action<List<StrategyTarget>>(this.SelectTargetWeaving3),
				new Action<List<StrategyTarget>>(this.SelectTargetJade1),
				new Action<List<StrategyTarget>>(this.SelectTargetJade2),
				new Action<List<StrategyTarget>>(this.SelectTargetJade3),
				new Action<List<StrategyTarget>>(this.SelectTargetTaoism1),
				new Action<List<StrategyTarget>>(this.SelectTargetTaoism2),
				new Action<List<StrategyTarget>>(this.SelectTargetTaoism3),
				new Action<List<StrategyTarget>>(this.SelectTargetBuddhism1),
				new Action<List<StrategyTarget>>(this.SelectTargetBuddhism2),
				new Action<List<StrategyTarget>>(this.SelectTargetBuddhism3),
				new Action<List<StrategyTarget>>(this.SelectTargetCooking1),
				new Action<List<StrategyTarget>>(this.SelectTargetCooking2),
				new Action<List<StrategyTarget>>(this.SelectTargetCooking3),
				new Action<List<StrategyTarget>>(this.SelectTargetEclectic1),
				new Action<List<StrategyTarget>>(this.SelectTargetEclectic2),
				new Action<List<StrategyTarget>>(this.SelectTargetEclectic3)
			};
			this.StrategyCheckers = new Dictionary<EDebateStrategyAiCheckType, Func<List<StrategyTarget>, int, bool>>
			{
				{
					EDebateStrategyAiCheckType.OpponentUsedCardCountGreater,
					new Func<List<StrategyTarget>, int, bool>(this.CheckOpponentUsedCardCountGreater)
				},
				{
					EDebateStrategyAiCheckType.OpponentOwnedCardCountGreater,
					new Func<List<StrategyTarget>, int, bool>(this.CheckOpponentOwnedCardCountGreater)
				},
				{
					EDebateStrategyAiCheckType.SelfCanUseCardCountSmaller,
					new Func<List<StrategyTarget>, int, bool>(this.CheckSelfCanUseCardCountSmaller)
				},
				{
					EDebateStrategyAiCheckType.SelfCanUseCardCountGreater,
					new Func<List<StrategyTarget>, int, bool>(this.CheckSelfCanUseCardCountGreater)
				},
				{
					EDebateStrategyAiCheckType.SelfPawnCountGreater,
					new Func<List<StrategyTarget>, int, bool>(this.CheckSelfPawnCountGreater)
				},
				{
					EDebateStrategyAiCheckType.SelfBasesGreater,
					new Func<List<StrategyTarget>, int, bool>(this.CheckSelfBasesGreater)
				},
				{
					EDebateStrategyAiCheckType.SelfStrategyPointSmaller,
					new Func<List<StrategyTarget>, int, bool>(this.CheckSelfStrategyPointSmaller)
				},
				{
					EDebateStrategyAiCheckType.TargetCountGreater,
					new Func<List<StrategyTarget>, int, bool>(this.CheckTargetCountGreater)
				},
				{
					EDebateStrategyAiCheckType.OpponentTargetCountGreater,
					new Func<List<StrategyTarget>, int, bool>(this.CheckOpponentTargetCountGreater)
				},
				{
					EDebateStrategyAiCheckType.SelfGamePointSmaller,
					new Func<List<StrategyTarget>, int, bool>(this.CheckSelfGamePointSmaller)
				},
				{
					EDebateStrategyAiCheckType.SelfGamePointGreater,
					new Func<List<StrategyTarget>, int, bool>(this.CheckSelfGamePointGreater)
				},
				{
					EDebateStrategyAiCheckType.OpponentGamePointGreater,
					new Func<List<StrategyTarget>, int, bool>(this.CheckOpponentGamePointGreater)
				},
				{
					EDebateStrategyAiCheckType.OpponentGamePointNotGreaterThanSelf,
					new Func<List<StrategyTarget>, int, bool>(this.OpponentGamePointNotGreaterThanSelf)
				},
				{
					EDebateStrategyAiCheckType.SelfNotCheckMate,
					new Func<List<StrategyTarget>, int, bool>(this.CheckSelfNotCheckMate)
				}
			};
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x0014D1E0 File Offset: 0x0014B3E0
		public void Initialize(DataContext context)
		{
			this._lineWeight = new List<ValueTuple<int, int, int>>();
			this._lineBases = new List<IntPair>();
			this._lineCanMakeMoveNodes = new List<IntPair>();
			this._strategyCards = new List<short>();
			for (int y = 0; y < DebateConstants.DebateLineCount; y++)
			{
				int index = this.GetLineIndex(y);
				if (!true)
				{
				}
				int num;
				switch (index)
				{
				case 0:
					num = context.Random.Next(DebateAiConstants.AttackLineWeight[(int)this._behaviorType][0], DebateAiConstants.AttackLineWeight[(int)this._behaviorType][1]);
					break;
				case 1:
					num = context.Random.Next(DebateAiConstants.MidLineWeight[(int)this._behaviorType][0], DebateAiConstants.MidLineWeight[(int)this._behaviorType][1]);
					break;
				case 2:
					num = context.Random.Next(DebateAiConstants.DefenseLineWeight[(int)this._behaviorType][0], DebateAiConstants.DefenseLineWeight[(int)this._behaviorType][1]);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				if (!true)
				{
				}
				int value = num;
				this._lineWeight.Add(new ValueTuple<int, int, int>(y, value, 100));
			}
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x0014D314 File Offset: 0x0014B514
		public void Start(DataContext context)
		{
			int count = 0;
			while (this.GetCanMakeMove() && count++ < 10)
			{
				this.UpdateState();
				this.UpdateLineWeight();
				this.UpdateLineBases();
				this.ProcessStrategy(context, true);
				this.ProcessMakeMove(context);
				this.ProcessStrategy(context, false);
			}
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x0014D370 File Offset: 0x0014B570
		public void UpdateLineWeightByDamage(bool isPawnOwner, int y)
		{
			int factor = isPawnOwner ? DebateAiConstants.DamageLineWeight[(int)this._behaviorType] : DebateAiConstants.DamagedLineWeight[(int)this._behaviorType];
			for (int i = 0; i < this._lineWeight.Count; i++)
			{
				bool flag = this._lineWeight[i].Item1 == y;
				if (flag)
				{
					this._lineWeight[i] = new ValueTuple<int, int, int>(y, this._lineWeight[i].Item2 * factor, this._lineWeight[i].Item3);
					break;
				}
			}
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x0014D40C File Offset: 0x0014B60C
		public List<int> GetRemovingCards(DataContext context)
		{
			List<int> res = new List<int>();
			int num;
			bool flag = !DebateAi.Debate.TryGetPlayerCardRemovingCount(this._isTaiwu, out num);
			List<int> result;
			if (flag)
			{
				result = res;
			}
			else
			{
				List<short> cards = DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu).CanUseCards;
				for (int i = 0; i < cards.Count; i++)
				{
					res.Add(i);
				}
				CollectionUtils.Shuffle<int>(context.Random, res);
				for (int j = 0; j < GlobalConfig.Instance.DebateMaxCanUseCards; j++)
				{
					res.RemoveAt(0);
				}
				result = res;
			}
			return result;
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x0014D4B4 File Offset: 0x0014B6B4
		private bool GetCanMakeMove()
		{
			bool playerCanMakeMove = DebateAi.Debate.GetPlayerCanMakeMove(this._isTaiwu);
			bool result;
			if (playerCanMakeMove)
			{
				result = true;
			}
			else
			{
				for (int y = 0; y < DebateConstants.DebateLineCount; y++)
				{
					for (int x = 0; x < DebateConstants.DebateLineNodeCount; x++)
					{
						IntPair coordinate = new IntPair(x, y);
						bool flag = DebateAi.Debate.GetNodeCanMakeMove(coordinate, this._isTaiwu) && DebateAi.Debate.GetNodeIsContainingEffect(coordinate, 41);
						if (flag)
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x0014D548 File Offset: 0x0014B748
		private bool TryMakeMove(DataContext context, IntPair coordinate, sbyte grade)
		{
			bool flag = !DebateAi.Debate.GetNodeCanMakeMove(coordinate, this._isTaiwu);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				DomainManager.Taiwu.DebateGameMakeMove(context, coordinate, this._isTaiwu, grade, true);
				result = true;
			}
			return result;
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x0014D58C File Offset: 0x0014B78C
		private bool TryMakeMove(DataContext context, int y, sbyte grade, sbyte type)
		{
			int start = 0;
			int direction = 1;
			switch (type)
			{
			case 0:
				start = (this._isTaiwu ? (DebateConstants.DebateLineNodeCount - 1) : 0);
				direction = (this._isTaiwu ? -1 : 1);
				break;
			case 1:
				start = (this._isTaiwu ? 0 : (DebateConstants.DebateLineNodeCount - 1));
				direction = (this._isTaiwu ? 1 : -1);
				break;
			case 2:
			{
				this._lineCanMakeMoveNodes.Clear();
				for (int x = 0; x < DebateConstants.DebateLineNodeCount; x++)
				{
					bool nodeCanMakeMove = DebateAi.Debate.GetNodeCanMakeMove(new IntPair(x, y), this._isTaiwu);
					if (nodeCanMakeMove)
					{
						this._lineCanMakeMoveNodes.Add(new IntPair(x, y));
					}
				}
				bool flag = this._lineCanMakeMoveNodes.Count <= 0;
				if (flag)
				{
					return false;
				}
				CollectionUtils.Shuffle<IntPair>(context.Random, this._lineCanMakeMoveNodes);
				DomainManager.Taiwu.DebateGameMakeMove(context, this._lineCanMakeMoveNodes[0], this._isTaiwu, grade, true);
				return true;
			}
			}
			foreach (ValueTuple<int, int, int> data in this._lineWeight)
			{
				int x2 = start;
				while (DebateAi.Debate.GetCoordinateValid(x2))
				{
					IntPair coordinate = new IntPair(x2, data.Item1);
					bool nodeCanMakeMove2 = DebateAi.Debate.GetNodeCanMakeMove(coordinate, this._isTaiwu);
					if (nodeCanMakeMove2)
					{
						DomainManager.Taiwu.DebateGameMakeMove(context, coordinate, this._isTaiwu, grade, true);
						return true;
					}
					x2 += direction;
				}
			}
			return false;
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x0014D764 File Offset: 0x0014B964
		private void MakeMoveByWeight(DataContext context, sbyte grade, sbyte type)
		{
			foreach (ValueTuple<int, int, int> data in this._lineWeight)
			{
				bool flag = this.TryMakeMove(context, data.Item1, grade, type);
				if (flag)
				{
					break;
				}
			}
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x0014D7C8 File Offset: 0x0014B9C8
		private void MakeMoveByBases(DataContext context, sbyte grade, sbyte type, bool isReversed)
		{
			this._lineBases.Sort(isReversed ? new Comparison<IntPair>(this.CompareLineBasesReversed) : new Comparison<IntPair>(this.CompareLineBases));
			foreach (IntPair data in this._lineBases)
			{
				bool flag = this.TryMakeMove(context, data.First, grade, type);
				if (flag)
				{
					break;
				}
			}
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x0014D858 File Offset: 0x0014BA58
		private void MakeMoveByBehavior(DataContext context, sbyte grade)
		{
			switch (this._behaviorType)
			{
			case 0:
			{
				int y;
				bool flag = !this.TryGetDisadvantageLine(out y) || !this.TryMakeMove(context, y, grade, 2);
				if (flag)
				{
					this.MakeMoveByWeight(context, grade, 2);
				}
				break;
			}
			case 1:
			{
				int y2;
				bool flag2 = !this.TryGetDisadvantageLine(out y2) || !this.TryMakeMove(context, y2, grade, 1);
				if (flag2)
				{
					this.MakeMoveByWeight(context, grade, 1);
				}
				break;
			}
			case 2:
			{
				DebatePlayer selfPlayer = DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu);
				DebatePlayer opponentPlayer = DebateAi.Debate.GetPlayerByPlayerIsTaiwu(!this._isTaiwu);
				bool flag3 = selfPlayer.MaxBases >= opponentPlayer.MaxBases;
				if (flag3)
				{
					int y3;
					bool flag4 = !this.TryGetDisadvantageLine(out y3) || !this.TryMakeMove(context, y3, grade, 2);
					if (flag4)
					{
						this.MakeMoveByWeight(context, grade, 2);
					}
				}
				else
				{
					int y4;
					bool flag5 = !this.TryGetEmptyOpponentPawnLine(out y4) || !this.TryMakeMove(context, y4, grade, 0);
					if (flag5)
					{
						this.MakeMoveByWeight(context, grade, 0);
					}
				}
				break;
			}
			case 3:
			{
				DebatePlayer selfPlayer2 = DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu);
				DebatePlayer opponentPlayer2 = DebateAi.Debate.GetPlayerByPlayerIsTaiwu(!this._isTaiwu);
				bool flag6 = selfPlayer2.MaxBases >= opponentPlayer2.MaxBases;
				if (flag6)
				{
					int y5;
					bool flag7 = !this.TryGetDisadvantageLine(out y5) || !this.TryMakeMove(context, y5, grade, 0);
					if (flag7)
					{
						this.MakeMoveByWeight(context, grade, 0);
					}
				}
				else
				{
					int y6;
					bool flag8 = !this.TryGetEmptyOpponentPawnLine(out y6) || !this.TryMakeMove(context, y6, grade, 0);
					if (flag8)
					{
						this.MakeMoveByWeight(context, grade, 0);
					}
				}
				break;
			}
			case 4:
				this.MakeMoveByWeight(context, grade, 2);
				break;
			}
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x0014DA3C File Offset: 0x0014BC3C
		private void CastStrategy(DataContext context, short templateId)
		{
			DebatePlayer player = DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu);
			DebateStrategyItem config = DebateStrategy.Instance[templateId];
			int index = -1;
			for (int i = 0; i < player.CanUseCards.Count; i++)
			{
				short card = player.CanUseCards[i];
				bool flag = card == templateId;
				if (flag)
				{
					index = i;
					break;
				}
			}
			bool flag2 = index < 0;
			if (!flag2)
			{
				bool flag3 = (int)config.UsedCost > player.StrategyPoint;
				if (!flag3)
				{
					List<StrategyTarget> targets;
					bool flag4 = !DebateAi.Debate.TryGetStrategyTarget(templateId, this._isTaiwu, out targets);
					if (!flag4)
					{
						bool flag5 = targets != null;
						if (flag5)
						{
							for (int j = 0; j < targets.Count; j++)
							{
								StrategyTarget target = targets[j];
								DebateAi.Debate.AddStrategyRepeatedTargets(config.TargetList[j][0], target.List);
								CollectionUtils.Shuffle<ulong>(context.Random, target.List);
							}
						}
						this.PrepareStrategyTargetActions[(int)templateId](targets);
						bool flag6 = this.TrySelectTargetGeneral(templateId, targets) && this.CheckStrategyCanUse(templateId, targets);
						if (flag6)
						{
							DomainManager.Taiwu.DebateGameCastStrategy(context, index, this._isTaiwu, targets);
						}
					}
				}
			}
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x0014DBA0 File Offset: 0x0014BDA0
		private void UpdateState()
		{
			bool flag = DebateAi.Debate.Round > DebateAiConstants.StateRoundInfluence[(int)this._behaviorType];
			if (flag)
			{
				this._state = 2;
			}
			else
			{
				DebatePlayer player = DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu);
				int gamePointPressure = player.GamePoint * 100 / DebateConstants.MaxGamePoint - player.Pressure * 100 / player.MaxPressure;
				bool flag2 = gamePointPressure <= DebateAiConstants.StateGamePointPressureInfluence[(int)this._behaviorType][1];
				if (flag2)
				{
					this._state = 2;
				}
				else
				{
					this._state = ((gamePointPressure > DebateAiConstants.StateGamePointPressureInfluence[(int)this._behaviorType][0]) ? 0 : 1);
					this._pawnDiff = this.GetPawnDiff();
					bool flag3 = this._pawnDiff >= DebateAiConstants.StatePawnCountInfluence[(int)this._behaviorType][1];
					if (flag3)
					{
						this._state = 2;
					}
					else
					{
						bool flag4 = this._pawnDiff >= DebateAiConstants.StatePawnCountInfluence[(int)this._behaviorType][0];
						if (flag4)
						{
							this._state = 1;
						}
					}
				}
			}
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x0014DCB8 File Offset: 0x0014BEB8
		private void UpdateLineWeight()
		{
			for (int i = 0; i < this._lineWeight.Count; i++)
			{
				int y = this._lineWeight[i].Item1;
				int factor = 100;
				bool flag = DebateAi.Debate.DebateGrid[new IntPair(0, y)].EffectState.TemplateId == 4;
				if (flag)
				{
					factor += DebateAiConstants.EgoisticNodeEffectWeightPercent;
				}
				bool flag2 = DebateAi.Debate.DebateGrid[new IntPair(DebateConstants.DebateLineNodeCount - 1, y)].EffectState.TemplateId == 4;
				if (flag2)
				{
					factor += DebateAiConstants.EgoisticNodeEffectWeightPercent;
				}
				this._lineWeight[i] = new ValueTuple<int, int, int>(y, this._lineWeight[i].Item2, factor);
			}
			this._lineWeight.Sort(new Comparison<ValueTuple<int, int, int>>(this.CompareLineWeight));
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x0014DDA4 File Offset: 0x0014BFA4
		private void UpdateLineBases()
		{
			this._lineBases.Clear();
			for (int y = 0; y < DebateConstants.DebateLineCount; y++)
			{
				int bases = 0;
				for (int x = 0; x < DebateConstants.DebateLineNodeCount; x++)
				{
					int pawnId = DebateAi.Debate.DebateGrid[new IntPair(x, y)].PawnId;
					bool flag = pawnId >= 0 && DebateAi.Debate.Pawns[pawnId].IsOwnedByTaiwu == this._isTaiwu;
					if (flag)
					{
						bases += DebateAi.Debate.GetPawnBases(pawnId, -1, false, this._isTaiwu);
					}
				}
				this._lineBases.Add(new IntPair(y, bases));
			}
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x0014DE68 File Offset: 0x0014C068
		private void ProcessStrategy(DataContext context, bool isBeforeMakeMove)
		{
			DebatePlayer player = DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu);
			bool flag = !isBeforeMakeMove;
			if (flag)
			{
				this._strategyCards.Clear();
				foreach (short card in player.CanUseCards)
				{
					bool avoidCheckMate = DebateStrategy.Instance[card].AvoidCheckMate;
					if (avoidCheckMate)
					{
						this._strategyCards.Add(card);
					}
				}
				this._strategyCards.Sort(new Comparison<short>(this.CompareStrategy));
				foreach (short card2 in this._strategyCards)
				{
					sbyte b;
					IntPair intPair;
					bool flag2 = !this.TryGetCheckMatePawn(this._isTaiwu, false, out b, out intPair);
					if (flag2)
					{
						break;
					}
					this.CastStrategy(context, card2);
				}
			}
			int kept = this.GetStrategyPointKept(isBeforeMakeMove);
			bool flag3 = player.StrategyPoint < kept;
			if (!flag3)
			{
				this._strategyCards.Clear();
				foreach (short card3 in player.CanUseCards)
				{
					bool flag4 = DebateStrategy.Instance[card3].UseBeforeMakeMove == isBeforeMakeMove;
					if (flag4)
					{
						this._strategyCards.Add(card3);
					}
				}
				this._strategyCards.Sort(new Comparison<short>(this.CompareStrategy));
				foreach (short card4 in this._strategyCards)
				{
					bool flag5 = player.StrategyPoint < kept;
					if (flag5)
					{
						return;
					}
					this.CastStrategy(context, card4);
				}
				int gamePointDelta = DebateAi.Debate.GetGamePointAndPressureDelta(player, GlobalConfig.Instance.DebateResetCardsPressureDelta).Item2;
				bool flag6 = DebateAi.Debate.GetPlayerCanUseResetStrategy(this._isTaiwu) && gamePointDelta + player.GamePoint > 0 && player.UsedCards.Count > GlobalConfig.Instance.ResetStrategyUsedCardLimit;
				if (flag6)
				{
					DomainManager.Taiwu.DebateGameResetCards(this._isTaiwu);
				}
				DomainManager.Taiwu.DebateGameRemoveCards(this._isTaiwu, this.GetRemovingCards(context));
			}
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x0014E110 File Offset: 0x0014C310
		private void ProcessMakeMove(DataContext context)
		{
			sbyte minGrade;
			IntPair coordinate;
			sbyte maxGrade;
			bool flag = this.TryGetCheckMatePawn(this._isTaiwu, true, out minGrade, out coordinate) && this.TryGetMaxGradeCanMakeMove(8, minGrade, out maxGrade) && this.TryMakeMove(context, coordinate, (sbyte)context.Random.Next((int)minGrade, (int)(maxGrade + 1)));
			if (!flag)
			{
				this.TryGetMaxGradeCanMakeMove(8, 0, out maxGrade);
				DebatePlayer selfPlayer = DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu);
				DebatePlayer opponentPlayer = DebateAi.Debate.GetPlayerByPlayerIsTaiwu(!this._isTaiwu);
				foreach (DebateNode node in DebateAi.Debate.DebateGrid.Values)
				{
					bool flag2 = node.EffectState.TemplateId == 1 && this.TryMakeMove(context, this.GetBehindCoordinate(node.Coordinate), maxGrade);
					if (flag2)
					{
						return;
					}
				}
				foreach (DebateNode node2 in DebateAi.Debate.DebateGrid.Values)
				{
					bool flag3 = node2.EffectState.TemplateId == 2 && this.TryMakeMove(context, node2.Coordinate, context.Random.CheckPercentProb(DebateAiConstants.EvenNodeEffectMaxGradeProb[(int)this._behaviorType]) ? maxGrade : 0);
					if (flag3)
					{
						return;
					}
				}
				bool flag4 = this._behaviorType == 3;
				if (flag4)
				{
					foreach (DebateNode node3 in DebateAi.Debate.DebateGrid.Values)
					{
						bool flag5 = node3.EffectState.TemplateId == 3 && this.TryMakeMove(context, this.GetBehindCoordinate(node3.Coordinate), maxGrade);
						if (flag5)
						{
							return;
						}
					}
				}
				bool flag6 = DebateAi.Debate.Round <= DebateAiConstants.RoundBeforeEarly;
				if (flag6)
				{
					this.MakeMoveByBehavior(context, maxGrade);
				}
				else
				{
					sbyte grade = (sbyte)(this.GetBasesEnough() ? context.Random.Next(DebateAiConstants.MinGradeIfEnoughBases, (int)(maxGrade + 1)) : ((int)(context.Random.CheckPercentProb(DebateAiConstants.ZeroGradePawnProb[this._state]) ? 0 : maxGrade)));
					bool flag7 = this._state == 2;
					if (flag7)
					{
						bool flag8 = this._pawnDiff + selfPlayer.GamePoint - opponentPlayer.GamePoint >= 0;
						if (flag8)
						{
							this.MakeMoveByBases(context, grade, 2, false);
						}
						else
						{
							bool flag9 = context.Random.CheckPercentProb(DebateAiConstants.MakeMoveOnOverwhelmingLineProb);
							if (flag9)
							{
								this.MakeMoveByBases(context, grade, 2, true);
							}
							else
							{
								this.MakeMoveByWeight(context, grade, 2);
							}
						}
					}
					else
					{
						this.MakeMoveByWeight(context, grade, 2);
					}
				}
			}
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x0014E410 File Offset: 0x0014C610
		private bool GetGamePointNotFull()
		{
			return DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu).GamePoint < DebateConstants.MaxGamePoint;
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x0014E440 File Offset: 0x0014C640
		private int GetBasesPercent()
		{
			DebatePlayer player = DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu);
			return (player.MaxBases == 0) ? 0 : (player.Bases * 100 / player.MaxBases);
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x0014E480 File Offset: 0x0014C680
		private bool GetBasesEnough()
		{
			int state = this._state;
			if (!true)
			{
			}
			int num;
			if (state != 0)
			{
				if (state != 1)
				{
					num = DebateAiConstants.LateBases[(int)this._behaviorType];
				}
				else
				{
					num = DebateAiConstants.MidBases[(int)this._behaviorType];
				}
			}
			else
			{
				num = DebateAiConstants.EarlyBases[(int)this._behaviorType];
			}
			if (!true)
			{
			}
			int minPercent = num;
			return this.GetBasesPercent() >= minPercent;
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x0014E4E8 File Offset: 0x0014C6E8
		private int GetStrategyPointKept(bool beforeMakeMove)
		{
			int index = beforeMakeMove ? 0 : 1;
			int state = this._state;
			if (!true)
			{
			}
			int result;
			if (state != 0)
			{
				if (state != 1)
				{
					result = DebateAiConstants.LateStrategyPoint[(int)this._behaviorType][index];
				}
				else
				{
					result = DebateAiConstants.MidStrategyPoint[(int)this._behaviorType][index];
				}
			}
			else
			{
				result = DebateAiConstants.EarlyStrategyPoint[(int)this._behaviorType][index];
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x0014E55C File Offset: 0x0014C75C
		private int GetLineIndex(int y)
		{
			return this._isTaiwu ? y : Math.Abs(y - 2);
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x0014E584 File Offset: 0x0014C784
		private int GetCheckMatePosition(bool isTaiwu)
		{
			return isTaiwu ? (DebateConstants.DebateLineNodeCount - 2) : 1;
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x0014E5A4 File Offset: 0x0014C7A4
		private IntPair GetBehindCoordinate(IntPair coordinate)
		{
			return new IntPair(this._isTaiwu ? (coordinate.First - 1) : (coordinate.First + 1), coordinate.Second);
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x0014E5DC File Offset: 0x0014C7DC
		private int GetPawnDiff()
		{
			int selfCount = 0;
			int opponentCount = 0;
			foreach (Pawn pawn in DebateAi.Debate.Pawns.Values)
			{
				bool flag = !pawn.IsAlive;
				if (!flag)
				{
					bool flag2 = pawn.IsOwnedByTaiwu == this._isTaiwu;
					if (flag2)
					{
						selfCount++;
					}
					else
					{
						opponentCount++;
					}
				}
			}
			return opponentCount - selfCount;
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x0014E674 File Offset: 0x0014C874
		private bool CheckStrategyCanUse(short templateId, List<StrategyTarget> targets)
		{
			DebateStrategyItem config = DebateStrategy.Instance[templateId];
			int state = this._state;
			if (!true)
			{
			}
			List<EDebateStrategyAiCheckType> list;
			switch (state)
			{
			case 0:
				list = config.EarlyLimits;
				break;
			case 1:
				list = config.MidLimits;
				break;
			case 2:
				list = config.LateLimits;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			if (!true)
			{
			}
			List<EDebateStrategyAiCheckType> limits = list;
			int state2 = this._state;
			if (!true)
			{
			}
			List<int> list2;
			switch (state2)
			{
			case 0:
				list2 = config.EarlyLimitParams;
				break;
			case 1:
				list2 = config.MidLimitParams;
				break;
			case 2:
				list2 = config.LateLimitParams;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			if (!true)
			{
			}
			List<int> limitParams = list2;
			bool flag = limits == null || limits.Count == 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				for (int index = 0; index < limits.Count; index++)
				{
					bool flag2 = !this.StrategyCheckers[limits[index]](targets, limitParams[index]);
					if (flag2)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x0014E790 File Offset: 0x0014C990
		private bool TrySelectTargetGeneral(short templateId, List<StrategyTarget> targets)
		{
			bool flag = targets == null;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				DebateStrategyItem config = DebateStrategy.Instance[templateId];
				for (int i = 0; i < targets.Count; i++)
				{
					StrategyTarget target = targets[i];
					short min = config.TargetList[i][1];
					short max = config.TargetList[i][2];
					bool flag2 = target == null;
					if (flag2)
					{
						return false;
					}
					bool flag3 = i > 0 && target.Type == targets[i - 1].Type;
					if (flag3)
					{
						DebateAi.Debate.CullStrategyTargets(targets[i - 1].List, target.List);
					}
					bool flag4 = target.List.Count > (int)max;
					if (flag4)
					{
						while (target.List.Count > (int)max)
						{
							target.List.RemoveAt(target.List.Count - 1);
						}
					}
					else
					{
						bool flag5 = target.List.Count < (int)min;
						if (flag5)
						{
							return false;
						}
					}
				}
				sbyte cost = config.UsedCost;
				int remaining = DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu).StrategyPoint - (int)cost;
				for (int j = 0; j < targets.Count; j++)
				{
					StrategyTarget target2 = targets[j];
					short min2 = config.TargetList[j][1];
					bool flag6 = target2.Type > EDebateStrategyTargetObjectType.Pawn;
					if (!flag6)
					{
						for (int k = 0; k < target2.List.Count; k++)
						{
							int value;
							bool flag7 = !DebateAi.Debate.TryGetPawnStrategyEffectValue((int)target2.List[k], 28, out value, null);
							if (!flag7)
							{
								remaining -= value;
								bool flag8 = remaining < 0;
								if (flag8)
								{
									remaining += value;
									target2.List.RemoveAt(k--);
									bool flag9 = target2.List.Count < (int)min2;
									if (flag9)
									{
										return false;
									}
								}
							}
						}
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x0014E9DC File Offset: 0x0014CBDC
		private bool TryGetCheckMatePawn(bool isTaiwu, bool checkBeatable, out sbyte resGrade, out IntPair resCoordinate)
		{
			int x = this.GetCheckMatePosition(!isTaiwu);
			bool found = false;
			resGrade = 8;
			resCoordinate = default(IntPair);
			for (int y = 0; y < DebateConstants.DebateLineCount; y++)
			{
				int pawnId = DebateAi.Debate.DebateGrid[new IntPair(x, y)].PawnId;
				IntPair coordinate = DebateAi.Debate.GetStartCoordinate(isTaiwu, y);
				sbyte grade = 0;
				bool flag = pawnId >= 0 && DebateAi.Debate.Pawns[pawnId].IsOwnedByTaiwu != isTaiwu && DebateAi.Debate.DebateGrid[coordinate].PawnId < 0 && (!checkBeatable || this.TryGetRequiredGradeToBeatPawn(pawnId, out grade));
				if (flag)
				{
					bool flag2 = grade <= resGrade;
					if (flag2)
					{
						found = true;
						resGrade = grade;
						resCoordinate = coordinate;
					}
				}
			}
			return found;
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x0014EAC4 File Offset: 0x0014CCC4
		private bool TryGetRequiredGradeToBeatPawn(int pawnId, out sbyte grade)
		{
			int bases = DebateAi.Debate.GetPawnBases(pawnId, -1, false, this._isTaiwu);
			grade = -1;
			int percent;
			bool flag = DebateAi.Debate.TryGetPawnRevealedStrategyEffectValue(pawnId, 6, this._isTaiwu, out percent, null);
			if (flag)
			{
				bases = bases * 100 / percent;
			}
			for (sbyte i = 0; i < 8; i += 1)
			{
				bool flag2 = DebateAi.Debate.GetPawnInitialBases(this._isTaiwu, i, 0) > bases;
				if (flag2)
				{
					grade = i;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x0014EB4C File Offset: 0x0014CD4C
		private bool TryGetMaxGradeCanMakeMove(sbyte maxGrade, sbyte minGrade, out sbyte grade)
		{
			DebatePlayer player = DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu);
			for (grade = maxGrade; grade >= minGrade; grade -= 1)
			{
				bool flag = player.Bases >= DebateAi.Debate.GetPawnGradeToBase(player.MaxBases, (int)grade);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x0014EBB0 File Offset: 0x0014CDB0
		private bool TryGetDisadvantageLine(out int y)
		{
			y = -1;
			foreach (ValueTuple<int, int, int> data in this._lineWeight)
			{
				int selfCount = 0;
				int opponentCount = 0;
				for (int x = 0; x < DebateConstants.DebateLineNodeCount; x++)
				{
					int pawnId = DebateAi.Debate.DebateGrid[new IntPair(x, data.Item1)].PawnId;
					bool flag = pawnId < 0;
					if (!flag)
					{
						bool flag2 = DebateAi.Debate.Pawns[pawnId].IsOwnedByTaiwu == this._isTaiwu;
						if (flag2)
						{
							selfCount++;
						}
						else
						{
							opponentCount++;
						}
					}
				}
				bool flag3 = selfCount == 0 && opponentCount != 0;
				if (flag3)
				{
					y = data.Item1;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x0014ECB0 File Offset: 0x0014CEB0
		private bool TryGetEmptyOpponentPawnLine(out int y)
		{
			y = -1;
			foreach (ValueTuple<int, int, int> data in this._lineWeight)
			{
				int opponentCount = 0;
				for (int x = 0; x < DebateConstants.DebateLineNodeCount; x++)
				{
					int pawnId = DebateAi.Debate.DebateGrid[new IntPair(x, data.Item1)].PawnId;
					bool flag = pawnId < 0;
					if (!flag)
					{
						bool flag2 = DebateAi.Debate.Pawns[pawnId].IsOwnedByTaiwu != this._isTaiwu;
						if (flag2)
						{
							opponentCount++;
						}
					}
				}
				bool flag3 = opponentCount == 0;
				if (flag3)
				{
					y = data.Item1;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x0014EDA0 File Offset: 0x0014CFA0
		private bool TryGetConflictingPawnId(int pawnId, out int otherId)
		{
			otherId = DebateAi.Debate.DebateGrid[DebateAi.Debate.GetPawnTargetPosition(pawnId)].PawnId;
			return otherId >= 0 && DebateAi.Debate.Pawns[pawnId].IsOwnedByTaiwu != DebateAi.Debate.Pawns[otherId].IsOwnedByTaiwu;
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x0014EE0C File Offset: 0x0014D00C
		private int CompareLineWeight(ValueTuple<int, int, int> a, ValueTuple<int, int, int> b)
		{
			int aValue = a.Item2 * a.Item3 / 100;
			int bValue = b.Item2 * b.Item3 / 100;
			return -aValue.CompareTo(bValue);
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x0014EE4C File Offset: 0x0014D04C
		private int CompareLineBases(IntPair a, IntPair b)
		{
			return a.Second.CompareTo(b.Second);
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x0014EE70 File Offset: 0x0014D070
		private int CompareLineBasesReversed(IntPair a, IntPair b)
		{
			return -a.Second.CompareTo(b.Second);
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x0014EE98 File Offset: 0x0014D098
		private int CompareStrategy(short a, short b)
		{
			return a.CompareTo(b);
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x0014EEB4 File Offset: 0x0014D0B4
		private void RemoveInvertBasesPawnTarget(StrategyTarget target)
		{
			for (int i = target.List.Count - 1; i >= 0; i--)
			{
				int pawnId = (int)target.List[i];
				int num;
				bool flag = DebateAi.Debate.TryGetPawnRevealedStrategyEffectValue(pawnId, 8, this._isTaiwu, out num, null);
				if (flag)
				{
					target.List.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x0014EF1C File Offset: 0x0014D11C
		private void RemoveCheckMatePawnTarget(StrategyTarget target)
		{
			for (int i = target.List.Count - 1; i >= 0; i--)
			{
				int pawnId = (int)target.List[i];
				Pawn pawn = DebateAi.Debate.Pawns[pawnId];
				bool flag = pawn.Coordinate.First == this.GetCheckMatePosition(pawn.IsOwnedByTaiwu);
				if (flag)
				{
					target.List.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x0014EF98 File Offset: 0x0014D198
		private void RemoveSpawnPawnTarget(StrategyTarget target)
		{
			for (int i = target.List.Count - 1; i >= 0; i--)
			{
				int pawnId = (int)target.List[i];
				Pawn pawn = DebateAi.Debate.Pawns[pawnId];
				bool flag = pawn.Coordinate.First == DebateAi.Debate.GetStartCoordinate(pawn.IsOwnedByTaiwu);
				if (flag)
				{
					target.List.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x0014F018 File Offset: 0x0014D218
		private void RemoveGreaterConflictingPawnTarget(StrategyTarget target)
		{
			for (int i = target.List.Count - 1; i >= 0; i--)
			{
				int pawnId = (int)target.List[i];
				int otherId;
				bool flag = this.TryGetConflictingPawnId(pawnId, out otherId) && DebateAi.Debate.GetPawnBases(pawnId, otherId, false, this._isTaiwu) > DebateAi.Debate.GetPawnBases(otherId, pawnId, false, this._isTaiwu);
				if (flag)
				{
					target.List.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x0014F0A0 File Offset: 0x0014D2A0
		private void RemoveSmallerConflictingPawnTarget(StrategyTarget target)
		{
			for (int i = target.List.Count - 1; i >= 0; i--)
			{
				int pawnId = (int)target.List[i];
				int otherId;
				bool flag = this.TryGetConflictingPawnId(pawnId, out otherId) && DebateAi.Debate.GetPawnBases(pawnId, otherId, false, this._isTaiwu) <= DebateAi.Debate.GetPawnBases(otherId, pawnId, false, this._isTaiwu);
				if (flag)
				{
					target.List.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x0014F12C File Offset: 0x0014D32C
		private void RemoveSmallerPawnBasesPercentTarget(StrategyTarget target)
		{
			int taiwuMaxBases = DebateAi.Debate.GetPlayerByPlayerIsTaiwu(true).MaxBases;
			int npcMaxBases = DebateAi.Debate.GetPlayerByPlayerIsTaiwu(false).MaxBases;
			for (int i = target.List.Count - 1; i >= 0; i--)
			{
				int pawnId = (int)target.List[i];
				int maxBases = DebateAi.Debate.Pawns[pawnId].IsOwnedByTaiwu ? taiwuMaxBases : npcMaxBases;
				bool flag = DebateAi.Debate.GetPawnBases(pawnId, -1, true, true) < maxBases * DebateAiConstants.RemoveStrategyTargetPawnBasesPercent / 100;
				if (flag)
				{
					target.List.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x0014F1D8 File Offset: 0x0014D3D8
		private void RemovePlayerConflictingPawnTarget(StrategyTarget target)
		{
			for (int i = target.List.Count - 1; i >= 0; i--)
			{
				int pawnId = (int)target.List[i];
				int otherId;
				bool flag = !this.TryGetConflictingPawnId(pawnId, out otherId);
				if (!flag)
				{
					int selfPawnBases = DebateAi.Debate.GetPawnBases(pawnId, otherId, false, this._isTaiwu);
					int opponentPawnBases = DebateAi.Debate.GetPawnBases(otherId, pawnId, false, this._isTaiwu);
					bool flag2 = (DebateAi.Debate.Pawns[pawnId].IsOwnedByTaiwu == this._isTaiwu && selfPawnBases > opponentPawnBases) || (DebateAi.Debate.Pawns[pawnId].IsOwnedByTaiwu != this._isTaiwu && selfPawnBases <= opponentPawnBases);
					if (flag2)
					{
						target.List.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x0014F2C0 File Offset: 0x0014D4C0
		private void RemoveFactorPawnTarget(StrategyTarget target)
		{
			for (int i = target.List.Count - 1; i >= 0; i--)
			{
				int pawnId = (int)target.List[i];
				Pawn pawn = DebateAi.Debate.Pawns[pawnId];
				bool flag = (pawn.IsOwnedByTaiwu == this._isTaiwu && DebateAi.Debate.GetPawnBasesFactor(pawnId, false, this._isTaiwu) > 0) || (pawn.IsOwnedByTaiwu != this._isTaiwu && DebateAi.Debate.GetPawnBasesFactor(pawnId, false, this._isTaiwu) < 0);
				if (flag)
				{
					target.List.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x0014F374 File Offset: 0x0014D574
		private void RemoveSelfPawnTarget(StrategyTarget target)
		{
			for (int i = target.List.Count - 1; i >= 0; i--)
			{
				int pawnId = (int)target.List[i];
				Pawn pawn = DebateAi.Debate.Pawns[pawnId];
				bool flag = pawn.IsOwnedByTaiwu == this._isTaiwu;
				if (flag)
				{
					target.List.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x0014F3E4 File Offset: 0x0014D5E4
		private void RemoveOpponentPawnTarget(StrategyTarget target)
		{
			for (int i = target.List.Count - 1; i >= 0; i--)
			{
				int pawnId = (int)target.List[i];
				Pawn pawn = DebateAi.Debate.Pawns[pawnId];
				bool flag = pawn.IsOwnedByTaiwu != this._isTaiwu;
				if (flag)
				{
					target.List.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x0014F458 File Offset: 0x0014D658
		private void RemoveOpponentNonCheckMatePawnTarget(StrategyTarget target)
		{
			int x = this.GetCheckMatePosition(!this._isTaiwu);
			for (int i = target.List.Count - 1; i >= 0; i--)
			{
				int pawnId = (int)target.List[i];
				Pawn pawn = DebateAi.Debate.Pawns[pawnId];
				bool flag = pawn.IsOwnedByTaiwu != this._isTaiwu && pawn.Coordinate.First != x;
				if (flag)
				{
					target.List.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x0014F4F0 File Offset: 0x0014D6F0
		private void RemoveNonOpponentStrategyPawnTarget(StrategyTarget target)
		{
			for (int i = target.List.Count - 1; i >= 0; i--)
			{
				int pawnId = (int)target.List[i];
				Pawn pawn = DebateAi.Debate.Pawns[pawnId];
				bool found = false;
				foreach (int strategyId in pawn.Strategies)
				{
					bool flag = strategyId < 0;
					if (!flag)
					{
						ActivatedStrategy strategy = DebateAi.Debate.ActivatedStrategies[strategyId];
						bool flag2 = strategy.IsCastedByTaiwu == this._isTaiwu;
						if (!flag2)
						{
							found = true;
							break;
						}
					}
				}
				bool flag3 = !found;
				if (flag3)
				{
					target.List.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x0014F5C0 File Offset: 0x0014D7C0
		private void RemoveLessOpponentStrategyPawnTarget(StrategyTarget target)
		{
			for (int i = target.List.Count - 1; i >= 0; i--)
			{
				int pawnId = (int)target.List[i];
				Pawn pawn = DebateAi.Debate.Pawns[pawnId];
				int selfCount = 0;
				int opponentCount = 0;
				foreach (int strategyId in pawn.Strategies)
				{
					bool flag = strategyId < 0;
					if (!flag)
					{
						bool flag2 = DebateAi.Debate.ActivatedStrategies[strategyId].IsCastedByTaiwu == this._isTaiwu;
						if (flag2)
						{
							selfCount++;
						}
						else
						{
							opponentCount++;
						}
					}
				}
				bool flag3 = opponentCount == 0 || selfCount >= opponentCount;
				if (flag3)
				{
					target.List.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x0014F6A0 File Offset: 0x0014D8A0
		private void RemoveFreeCard(StrategyTarget target)
		{
			DebatePlayer player = DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu);
			for (int i = target.List.Count - 1; i >= 0; i--)
			{
				int index = (int)target.List[i];
				bool flag = DebateStrategy.Instance[player.CanUseCards[index]].UsedCost == 0;
				if (flag)
				{
					target.List.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x0014F720 File Offset: 0x0014D920
		private int ComparePawnByDistance(ulong a, ulong b)
		{
			Pawn pawnA = DebateAi.Debate.Pawns[(int)a];
			Pawn pawnB = DebateAi.Debate.Pawns[(int)b];
			int distanceA = Math.Abs(pawnA.Coordinate.First - DebateAi.Debate.GetStartCoordinate(pawnA.IsOwnedByTaiwu));
			int distanceB = Math.Abs(pawnB.Coordinate.First - DebateAi.Debate.GetStartCoordinate(pawnB.IsOwnedByTaiwu));
			return distanceA.CompareTo(distanceB);
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x0014F7A4 File Offset: 0x0014D9A4
		private int ComparePawnByDistanceReversed(ulong a, ulong b)
		{
			Pawn pawnA = DebateAi.Debate.Pawns[(int)a];
			Pawn pawnB = DebateAi.Debate.Pawns[(int)b];
			int distanceA = Math.Abs(pawnA.Coordinate.First - DebateAi.Debate.GetStartCoordinate(pawnA.IsOwnedByTaiwu));
			int distanceB = Math.Abs(pawnB.Coordinate.First - DebateAi.Debate.GetStartCoordinate(pawnB.IsOwnedByTaiwu));
			return -distanceA.CompareTo(distanceB);
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x0014F82C File Offset: 0x0014DA2C
		private int ComparePawnByBasesFactorAndPlayer(ulong a, ulong b)
		{
			Pawn pawnA = DebateAi.Debate.Pawns[(int)a];
			Pawn pawnB = DebateAi.Debate.Pawns[(int)b];
			int factorA = DebateAi.Debate.GetPawnBasesFactor(pawnA.Id, false, this._isTaiwu) * ((pawnA.IsOwnedByTaiwu == this._isTaiwu) ? 1 : -1);
			int factorB = DebateAi.Debate.GetPawnBasesFactor(pawnB.Id, false, this._isTaiwu) * ((pawnB.IsOwnedByTaiwu == this._isTaiwu) ? 1 : -1);
			return factorA.CompareTo(factorB);
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x0014F8C4 File Offset: 0x0014DAC4
		private int ComparePawnByDistanceReversedAndConflict(ulong a, ulong b)
		{
			Pawn pawnA = DebateAi.Debate.Pawns[(int)a];
			Pawn pawnB = DebateAi.Debate.Pawns[(int)b];
			int distanceA = Math.Abs(pawnA.Coordinate.First - DebateAi.Debate.GetStartCoordinate(pawnA.IsOwnedByTaiwu));
			int distanceB = Math.Abs(pawnB.Coordinate.First - DebateAi.Debate.GetStartCoordinate(pawnB.IsOwnedByTaiwu));
			bool flag = distanceA == DebateConstants.DebateLineNodeCount - 2;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = distanceB == DebateConstants.DebateLineNodeCount - 2;
				if (flag2)
				{
					result = 1;
				}
				else
				{
					int num;
					bool resultA = this.TryGetConflictingPawnId(pawnA.Id, out num);
					bool resultB = this.TryGetConflictingPawnId(pawnB.Id, out num);
					bool flag3 = resultA;
					if (flag3)
					{
						result = -1;
					}
					else
					{
						bool flag4 = resultB;
						if (flag4)
						{
							result = 1;
						}
						else
						{
							result = -distanceA.CompareTo(distanceB);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x0014F9B0 File Offset: 0x0014DBB0
		private int ComparePawnByBases(ulong a, ulong b)
		{
			Pawn pawnA = DebateAi.Debate.Pawns[(int)a];
			Pawn pawnB = DebateAi.Debate.Pawns[(int)b];
			int basesA = DebateAi.Debate.GetPawnBases(pawnA.Id, -1, false, this._isTaiwu);
			int basesB = DebateAi.Debate.GetPawnBases(pawnB.Id, -1, false, this._isTaiwu);
			return basesA.CompareTo(basesB);
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x0014FA24 File Offset: 0x0014DC24
		private int ComparePawnByBasesReversed(ulong a, ulong b)
		{
			Pawn pawnA = DebateAi.Debate.Pawns[(int)a];
			Pawn pawnB = DebateAi.Debate.Pawns[(int)b];
			int basesA = DebateAi.Debate.GetPawnBases(pawnA.Id, -1, false, this._isTaiwu);
			int basesB = DebateAi.Debate.GetPawnBases(pawnB.Id, -1, false, this._isTaiwu);
			return -basesA.CompareTo(basesB);
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x0014FA98 File Offset: 0x0014DC98
		private int ComparePawnByBasesAndConflict(ulong a, ulong b)
		{
			Pawn pawnA = DebateAi.Debate.Pawns[(int)a];
			Pawn pawnB = DebateAi.Debate.Pawns[(int)b];
			int num;
			bool resultA = this.TryGetConflictingPawnId(pawnA.Id, out num);
			bool resultB = this.TryGetConflictingPawnId(pawnB.Id, out num);
			bool flag = resultA && !resultB;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = !resultA && resultB;
				if (flag2)
				{
					result = 1;
				}
				else
				{
					int basesA = DebateAi.Debate.GetPawnBases(pawnA.Id, -1, false, this._isTaiwu) * ((pawnA.IsOwnedByTaiwu == this._isTaiwu) ? 1 : -1);
					int basesB = DebateAi.Debate.GetPawnBases(pawnB.Id, -1, false, this._isTaiwu) * ((pawnB.IsOwnedByTaiwu == this._isTaiwu) ? 1 : -1);
					result = -basesA.CompareTo(basesB);
				}
			}
			return result;
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x0014FB78 File Offset: 0x0014DD78
		private int ComparePawnByOpponentStrategyCount(ulong a, ulong b)
		{
			Pawn pawnA = DebateAi.Debate.Pawns[(int)a];
			Pawn pawnB = DebateAi.Debate.Pawns[(int)b];
			int countA = 0;
			int countB = 0;
			foreach (int strategyId in pawnA.Strategies)
			{
				bool flag = strategyId < 0;
				if (!flag)
				{
					bool flag2 = DebateAi.Debate.ActivatedStrategies[strategyId].IsCastedByTaiwu != this._isTaiwu;
					if (flag2)
					{
						countA++;
					}
				}
			}
			foreach (int strategyId2 in pawnB.Strategies)
			{
				bool flag3 = strategyId2 < 0;
				if (!flag3)
				{
					bool flag4 = DebateAi.Debate.ActivatedStrategies[strategyId2].IsCastedByTaiwu != this._isTaiwu;
					if (flag4)
					{
						countB++;
					}
				}
			}
			return countA.CompareTo(countB);
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x0014FC78 File Offset: 0x0014DE78
		private int ComparePawnByOpponentStrategyCountReversed(ulong a, ulong b)
		{
			Pawn pawnA = DebateAi.Debate.Pawns[(int)a];
			Pawn pawnB = DebateAi.Debate.Pawns[(int)b];
			int countA = 0;
			int countB = 0;
			foreach (int strategyId in pawnA.Strategies)
			{
				bool flag = strategyId < 0;
				if (!flag)
				{
					bool flag2 = DebateAi.Debate.ActivatedStrategies[strategyId].IsCastedByTaiwu != this._isTaiwu;
					if (flag2)
					{
						countA++;
					}
				}
			}
			foreach (int strategyId2 in pawnB.Strategies)
			{
				bool flag3 = strategyId2 < 0;
				if (!flag3)
				{
					bool flag4 = DebateAi.Debate.ActivatedStrategies[strategyId2].IsCastedByTaiwu != this._isTaiwu;
					if (flag4)
					{
						countB++;
					}
				}
			}
			return -countA.CompareTo(countB);
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x0014FD78 File Offset: 0x0014DF78
		private int CompareNodeByDistance(ulong a, ulong b)
		{
			DebateNode nodeA = DebateAi.Debate.DebateGrid[(IntPair)a];
			DebateNode nodeB = DebateAi.Debate.DebateGrid[(IntPair)b];
			int distanceA = Math.Abs(nodeA.Coordinate.First - DebateAi.Debate.GetStartCoordinate(!this._isTaiwu));
			int distanceB = Math.Abs(nodeB.Coordinate.First - DebateAi.Debate.GetStartCoordinate(!this._isTaiwu));
			return distanceA.CompareTo(distanceB);
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x0014FE0C File Offset: 0x0014E00C
		private int CompareNodeByVantageDistanceReversed(ulong a, ulong b)
		{
			DebateNode nodeA = DebateAi.Debate.DebateGrid[(IntPair)a];
			DebateNode nodeB = DebateAi.Debate.DebateGrid[(IntPair)b];
			int distanceA = Math.Abs(nodeA.Coordinate.First - DebateAi.Debate.GetStartCoordinate(nodeA.IsVantage));
			int distanceB = Math.Abs(nodeB.Coordinate.First - DebateAi.Debate.GetStartCoordinate(nodeB.IsVantage));
			return -distanceA.CompareTo(distanceB);
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x0014FE9C File Offset: 0x0014E09C
		private bool CheckOpponentUsedCardCountGreater(List<StrategyTarget> targets, int value)
		{
			return DebateAi.Debate.GetPlayerByPlayerIsTaiwu(!this._isTaiwu).UsedCards.Count > value;
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x0014FED0 File Offset: 0x0014E0D0
		private bool CheckOpponentOwnedCardCountGreater(List<StrategyTarget> targets, int value)
		{
			return DebateAi.Debate.GetPlayerByPlayerIsTaiwu(!this._isTaiwu).OwnedCards.Count > value;
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x0014FF04 File Offset: 0x0014E104
		private bool CheckSelfCanUseCardCountSmaller(List<StrategyTarget> targets, int value)
		{
			return DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu).CanUseCards.Count < value;
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x0014FF34 File Offset: 0x0014E134
		private bool CheckSelfCanUseCardCountGreater(List<StrategyTarget> targets, int value)
		{
			return DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu).CanUseCards.Count > value;
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x0014FF64 File Offset: 0x0014E164
		private bool CheckSelfPawnCountGreater(List<StrategyTarget> targets, int value)
		{
			return DebateAi.Debate.GetPawnCount(this._isTaiwu) > value;
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x0014FF8C File Offset: 0x0014E18C
		private bool CheckSelfBasesGreater(List<StrategyTarget> targets, int value)
		{
			return this.GetBasesPercent() > value;
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x0014FFA8 File Offset: 0x0014E1A8
		private bool CheckSelfStrategyPointSmaller(List<StrategyTarget> targets, int value)
		{
			return DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu).StrategyPoint < value;
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x0014FFD4 File Offset: 0x0014E1D4
		private bool CheckTargetCountGreater(List<StrategyTarget> targets, int value)
		{
			int count = 0;
			foreach (StrategyTarget target in targets)
			{
				count += target.List.Count;
			}
			return count > value;
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x00150038 File Offset: 0x0014E238
		private bool CheckOpponentTargetCountGreater(List<StrategyTarget> targets, int value)
		{
			int count = 0;
			foreach (StrategyTarget target in targets)
			{
				bool flag = target.Type > EDebateStrategyTargetObjectType.Pawn;
				if (!flag)
				{
					foreach (ulong data in target.List)
					{
						bool flag2 = DebateAi.Debate.Pawns[(int)data].IsOwnedByTaiwu != this._isTaiwu;
						if (flag2)
						{
							count++;
						}
					}
				}
			}
			return count > value;
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x00150110 File Offset: 0x0014E310
		private bool CheckSelfGamePointSmaller(List<StrategyTarget> targets, int value)
		{
			return DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu).GamePoint < value;
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x0015013C File Offset: 0x0014E33C
		private bool CheckSelfGamePointGreater(List<StrategyTarget> targets, int value)
		{
			return DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu).GamePoint > value;
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x00150168 File Offset: 0x0014E368
		private bool CheckOpponentGamePointGreater(List<StrategyTarget> targets, int value)
		{
			return DebateAi.Debate.GetPlayerByPlayerIsTaiwu(!this._isTaiwu).GamePoint > value;
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x00150198 File Offset: 0x0014E398
		private bool OpponentGamePointNotGreaterThanSelf(List<StrategyTarget> targets, int value)
		{
			return DebateAi.Debate.GetPlayerByPlayerIsTaiwu(!this._isTaiwu).GamePoint <= DebateAi.Debate.GetPlayerByPlayerIsTaiwu(this._isTaiwu).GamePoint;
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x001501DC File Offset: 0x0014E3DC
		private bool CheckSelfNotCheckMate(List<StrategyTarget> targets, int value)
		{
			sbyte b;
			IntPair intPair;
			return !this.TryGetCheckMatePawn(!this._isTaiwu, false, out b, out intPair);
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x00150208 File Offset: 0x0014E408
		private void SelectTargetMusic1(List<StrategyTarget> targets)
		{
			bool avoidCheckMate = !DebateAi.Debate.GetPlayerCanMakeMove(this._isTaiwu);
			foreach (StrategyTarget target in targets)
			{
				this.RemoveInvertBasesPawnTarget(target);
				bool flag = avoidCheckMate;
				if (flag)
				{
					this.RemoveCheckMatePawnTarget(target);
				}
			}
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x00150280 File Offset: 0x0014E480
		private void SelectTargetMusic2(List<StrategyTarget> targets)
		{
			bool avoidCheckMate = !DebateAi.Debate.GetPlayerCanMakeMove(this._isTaiwu);
			foreach (StrategyTarget target in targets)
			{
				this.RemoveInvertBasesPawnTarget(target);
				bool flag = avoidCheckMate;
				if (flag)
				{
					this.RemoveCheckMatePawnTarget(target);
				}
			}
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x001502F8 File Offset: 0x0014E4F8
		private void SelectTargetMusic3(List<StrategyTarget> targets)
		{
			bool avoidCheckMate = !DebateAi.Debate.GetPlayerCanMakeMove(this._isTaiwu);
			foreach (StrategyTarget target in targets)
			{
				this.RemoveInvertBasesPawnTarget(target);
				bool flag = avoidCheckMate;
				if (flag)
				{
					this.RemoveCheckMatePawnTarget(target);
				}
			}
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x00150370 File Offset: 0x0014E570
		private void SelectTargetChess1(List<StrategyTarget> targets)
		{
			List<ulong> list = targets[1].List;
			sbyte grade;
			this.TryGetMaxGradeCanMakeMove(8, 0, out grade);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				bool flag = i > (int)grade || i <= (int)(grade - 3);
				if (flag)
				{
					list.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x001503CE File Offset: 0x0014E5CE
		private void SelectTargetChess2(List<StrategyTarget> targets)
		{
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x001503D4 File Offset: 0x0014E5D4
		private void SelectTargetChess3(List<StrategyTarget> targets)
		{
			this.RemoveCheckMatePawnTarget(targets[0]);
			this.RemoveGreaterConflictingPawnTarget(targets[0]);
			targets[0].List.Sort(new Comparison<ulong>(this.ComparePawnByDistance));
			targets[1].List.Sort(new Comparison<ulong>(this.CompareNodeByVantageDistanceReversed));
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x0015043C File Offset: 0x0014E63C
		private void SelectTargetPoem1(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveSmallerConflictingPawnTarget(target);
				target.List.Sort(new Comparison<ulong>(this.ComparePawnByDistanceReversed));
			}
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x001504A8 File Offset: 0x0014E6A8
		private void SelectTargetPoem2(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveCheckMatePawnTarget(target);
				this.RemoveSmallerConflictingPawnTarget(target);
			}
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x00150504 File Offset: 0x0014E704
		private void SelectTargetPoem3(List<StrategyTarget> targets)
		{
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x00150508 File Offset: 0x0014E708
		private void SelectTargetPainting1(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveInvertBasesPawnTarget(target);
			}
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x0015055C File Offset: 0x0014E75C
		private void SelectTargetPainting2(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveInvertBasesPawnTarget(target);
			}
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x001505B0 File Offset: 0x0014E7B0
		private void SelectTargetPainting3(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveInvertBasesPawnTarget(target);
			}
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x00150604 File Offset: 0x0014E804
		private void SelectTargetMath1(List<StrategyTarget> targets)
		{
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x00150607 File Offset: 0x0014E807
		private void SelectTargetMath2(List<StrategyTarget> targets)
		{
			this.RemoveInvertBasesPawnTarget(targets[0]);
			this.RemoveFactorPawnTarget(targets[0]);
			targets[0].List.Sort(new Comparison<ulong>(this.ComparePawnByBasesFactorAndPlayer));
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x00150644 File Offset: 0x0014E844
		private void SelectTargetMath3(List<StrategyTarget> targets)
		{
			this.RemoveSmallerConflictingPawnTarget(targets[0]);
			this.RemoveSpawnPawnTarget(targets[0]);
			targets[1].List.Sort(new Comparison<ulong>(this.CompareNodeByDistance));
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x00150684 File Offset: 0x0014E884
		private void SelectTargetAppraisal1(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveCheckMatePawnTarget(target);
				target.List.Sort(new Comparison<ulong>(this.ComparePawnByDistance));
			}
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x001506F0 File Offset: 0x0014E8F0
		private void SelectTargetAppraisal2(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveCheckMatePawnTarget(target);
				target.List.Sort(new Comparison<ulong>(this.ComparePawnByDistance));
			}
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x0015075C File Offset: 0x0014E95C
		private void SelectTargetAppraisal3(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveSmallerConflictingPawnTarget(target);
				target.List.Sort(new Comparison<ulong>(this.ComparePawnByDistanceReversed));
			}
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x001507C8 File Offset: 0x0014E9C8
		private void SelectTargetForging1(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveCheckMatePawnTarget(target);
				this.RemoveSmallerConflictingPawnTarget(target);
				this.RemoveSmallerPawnBasesPercentTarget(target);
			}
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x0015082C File Offset: 0x0014EA2C
		private void SelectTargetForging2(List<StrategyTarget> targets)
		{
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x00150830 File Offset: 0x0014EA30
		private void SelectTargetForging3(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveInvertBasesPawnTarget(target);
				this.RemoveSmallerConflictingPawnTarget(target);
				target.List.Sort(new Comparison<ulong>(this.ComparePawnByDistance));
			}
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x001508A4 File Offset: 0x0014EAA4
		private void SelectTargetWoodworking1(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveSmallerConflictingPawnTarget(target);
				target.List.Sort(new Comparison<ulong>(this.ComparePawnByDistance));
			}
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x00150910 File Offset: 0x0014EB10
		private void SelectTargetWoodworking2(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveSmallerConflictingPawnTarget(target);
				target.List.Sort(new Comparison<ulong>(this.ComparePawnByDistance));
			}
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x0015097C File Offset: 0x0014EB7C
		private void SelectTargetWoodworking3(List<StrategyTarget> targets)
		{
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x0015097F File Offset: 0x0014EB7F
		private void SelectTargetMedicine1(List<StrategyTarget> targets)
		{
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x00150984 File Offset: 0x0014EB84
		private void SelectTargetMedicine2(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveSelfPawnTarget(target);
				target.List.Sort(new Comparison<ulong>(this.ComparePawnByBasesReversed));
			}
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x001509F0 File Offset: 0x0014EBF0
		private void SelectTargetMedicine3(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveSmallerConflictingPawnTarget(target);
				target.List.Sort(new Comparison<ulong>(this.ComparePawnByDistanceReversed));
			}
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x00150A5C File Offset: 0x0014EC5C
		private void SelectTargetToxicology1(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				target.List.Sort(new Comparison<ulong>(this.ComparePawnByDistance));
			}
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x00150AC0 File Offset: 0x0014ECC0
		private void SelectTargetToxicology2(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveSelfPawnTarget(target);
				target.List.Sort(new Comparison<ulong>(this.ComparePawnByBasesReversed));
			}
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x00150B2C File Offset: 0x0014ED2C
		private void SelectTargetToxicology3(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveSmallerConflictingPawnTarget(target);
				target.List.Sort(new Comparison<ulong>(this.ComparePawnByDistanceReversed));
			}
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x00150B98 File Offset: 0x0014ED98
		private void SelectTargetWeaving1(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveInvertBasesPawnTarget(target);
			}
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x00150BEC File Offset: 0x0014EDEC
		private void SelectTargetWeaving2(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveSmallerConflictingPawnTarget(target);
				target.List.Sort(new Comparison<ulong>(this.ComparePawnByBasesAndConflict));
			}
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x00150C58 File Offset: 0x0014EE58
		private void SelectTargetWeaving3(List<StrategyTarget> targets)
		{
			targets[0].List.Sort(new Comparison<ulong>(this.ComparePawnByBases));
			List<ulong> list = targets[0].List;
			List<ulong> list2 = targets[0].List;
			int index = list2.Count - 1;
			List<ulong> list3 = targets[0].List;
			ulong value = list3[list3.Count - 1];
			ulong value2 = targets[0].List[0];
			list[0] = value;
			list2[index] = value2;
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x00150CE4 File Offset: 0x0014EEE4
		private void SelectTargetJade1(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveSelfPawnTarget(target);
				target.List.Sort(new Comparison<ulong>(this.ComparePawnByDistanceReversedAndConflict));
			}
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00150D50 File Offset: 0x0014EF50
		private void SelectTargetJade2(List<StrategyTarget> targets)
		{
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x00150D54 File Offset: 0x0014EF54
		private void SelectTargetJade3(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveSmallerConflictingPawnTarget(target);
				target.List.Sort(new Comparison<ulong>(this.ComparePawnByDistanceReversed));
			}
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00150DC0 File Offset: 0x0014EFC0
		private void SelectTargetTaoism1(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveSelfPawnTarget(target);
				this.RemoveSmallerConflictingPawnTarget(target);
				target.List.Sort(new Comparison<ulong>(this.ComparePawnByDistanceReversed));
			}
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x00150E34 File Offset: 0x0014F034
		private void SelectTargetTaoism2(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveFreeCard(target);
			}
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x00150E88 File Offset: 0x0014F088
		private void SelectTargetTaoism3(List<StrategyTarget> targets)
		{
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x00150E8C File Offset: 0x0014F08C
		private void SelectTargetBuddhism1(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveNonOpponentStrategyPawnTarget(target);
			}
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00150EE0 File Offset: 0x0014F0E0
		private void SelectTargetBuddhism2(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemovePlayerConflictingPawnTarget(target);
			}
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x00150F34 File Offset: 0x0014F134
		private void SelectTargetBuddhism3(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveSmallerConflictingPawnTarget(target);
			}
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x00150F88 File Offset: 0x0014F188
		private void SelectTargetCooking1(List<StrategyTarget> targets)
		{
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x00150F8B File Offset: 0x0014F18B
		private void SelectTargetCooking2(List<StrategyTarget> targets)
		{
			this.RemoveOpponentPawnTarget(targets[0]);
			this.RemoveSelfPawnTarget(targets[1]);
			targets[1].List.Sort(new Comparison<ulong>(this.ComparePawnByBasesReversed));
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x00150FC8 File Offset: 0x0014F1C8
		private void SelectTargetCooking3(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveOpponentNonCheckMatePawnTarget(target);
			}
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x0015101C File Offset: 0x0014F21C
		private void SelectTargetEclectic1(List<StrategyTarget> targets)
		{
			foreach (StrategyTarget target in targets)
			{
				this.RemoveLessOpponentStrategyPawnTarget(target);
			}
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x00151070 File Offset: 0x0014F270
		private void SelectTargetEclectic2(List<StrategyTarget> targets)
		{
			this.RemoveSelfPawnTarget(targets[0]);
			this.RemoveOpponentPawnTarget(targets[1]);
			targets[0].List.Sort(new Comparison<ulong>(this.ComparePawnByOpponentStrategyCountReversed));
			targets[1].List.Sort(new Comparison<ulong>(this.ComparePawnByOpponentStrategyCount));
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x001510D6 File Offset: 0x0014F2D6
		private void SelectTargetEclectic3(List<StrategyTarget> targets)
		{
		}

		// Token: 0x04000396 RID: 918
		private bool _isTaiwu;

		// Token: 0x04000397 RID: 919
		private sbyte _behaviorType;

		// Token: 0x04000398 RID: 920
		private int _pawnDiff;

		// Token: 0x04000399 RID: 921
		private List<ValueTuple<int, int, int>> _lineWeight;

		// Token: 0x0400039A RID: 922
		private List<IntPair> _lineBases;

		// Token: 0x0400039B RID: 923
		private List<IntPair> _lineCanMakeMoveNodes;

		// Token: 0x0400039C RID: 924
		private List<short> _strategyCards;

		// Token: 0x0400039D RID: 925
		private int _state;

		// Token: 0x0400039E RID: 926
		public List<Action<List<StrategyTarget>>> PrepareStrategyTargetActions;

		// Token: 0x0400039F RID: 927
		public Dictionary<EDebateStrategyAiCheckType, Func<List<StrategyTarget>, int, bool>> StrategyCheckers;
	}
}
