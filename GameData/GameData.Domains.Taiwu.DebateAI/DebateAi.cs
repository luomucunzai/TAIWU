using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Domains.Taiwu.Debate;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.DebateAI;

public class DebateAi
{
	private bool _isTaiwu;

	private sbyte _behaviorType;

	private int _pawnDiff;

	private List<(int, int, int)> _lineWeight;

	private List<IntPair> _lineBases;

	private List<IntPair> _lineCanMakeMoveNodes;

	private List<short> _strategyCards;

	private int _state;

	public List<Action<List<StrategyTarget>>> PrepareStrategyTargetActions;

	public Dictionary<EDebateStrategyAiCheckType, Func<List<StrategyTarget>, int, bool>> StrategyCheckers;

	private static DebateGame Debate => DomainManager.Taiwu.Debate;

	public DebateAi(bool isTaiwu, sbyte behaviorType)
	{
		_isTaiwu = isTaiwu;
		_behaviorType = behaviorType;
		PrepareStrategyTargetActions = new List<Action<List<StrategyTarget>>>
		{
			SelectTargetMusic1, SelectTargetMusic2, SelectTargetMusic3, SelectTargetChess1, SelectTargetChess2, SelectTargetChess3, SelectTargetPoem1, SelectTargetPoem2, SelectTargetPoem3, SelectTargetPainting1,
			SelectTargetPainting2, SelectTargetPainting3, SelectTargetMath1, SelectTargetMath2, SelectTargetMath3, SelectTargetAppraisal1, SelectTargetAppraisal2, SelectTargetAppraisal3, SelectTargetForging1, SelectTargetForging2,
			SelectTargetForging3, SelectTargetWoodworking1, SelectTargetWoodworking2, SelectTargetWoodworking3, SelectTargetMedicine1, SelectTargetMedicine2, SelectTargetMedicine3, SelectTargetToxicology1, SelectTargetToxicology2, SelectTargetToxicology3,
			SelectTargetWeaving1, SelectTargetWeaving2, SelectTargetWeaving3, SelectTargetJade1, SelectTargetJade2, SelectTargetJade3, SelectTargetTaoism1, SelectTargetTaoism2, SelectTargetTaoism3, SelectTargetBuddhism1,
			SelectTargetBuddhism2, SelectTargetBuddhism3, SelectTargetCooking1, SelectTargetCooking2, SelectTargetCooking3, SelectTargetEclectic1, SelectTargetEclectic2, SelectTargetEclectic3
		};
		StrategyCheckers = new Dictionary<EDebateStrategyAiCheckType, Func<List<StrategyTarget>, int, bool>>
		{
			{
				EDebateStrategyAiCheckType.OpponentUsedCardCountGreater,
				CheckOpponentUsedCardCountGreater
			},
			{
				EDebateStrategyAiCheckType.OpponentOwnedCardCountGreater,
				CheckOpponentOwnedCardCountGreater
			},
			{
				EDebateStrategyAiCheckType.SelfCanUseCardCountSmaller,
				CheckSelfCanUseCardCountSmaller
			},
			{
				EDebateStrategyAiCheckType.SelfCanUseCardCountGreater,
				CheckSelfCanUseCardCountGreater
			},
			{
				EDebateStrategyAiCheckType.SelfPawnCountGreater,
				CheckSelfPawnCountGreater
			},
			{
				EDebateStrategyAiCheckType.SelfBasesGreater,
				CheckSelfBasesGreater
			},
			{
				EDebateStrategyAiCheckType.SelfStrategyPointSmaller,
				CheckSelfStrategyPointSmaller
			},
			{
				EDebateStrategyAiCheckType.TargetCountGreater,
				CheckTargetCountGreater
			},
			{
				EDebateStrategyAiCheckType.OpponentTargetCountGreater,
				CheckOpponentTargetCountGreater
			},
			{
				EDebateStrategyAiCheckType.SelfGamePointSmaller,
				CheckSelfGamePointSmaller
			},
			{
				EDebateStrategyAiCheckType.SelfGamePointGreater,
				CheckSelfGamePointGreater
			},
			{
				EDebateStrategyAiCheckType.OpponentGamePointGreater,
				CheckOpponentGamePointGreater
			},
			{
				EDebateStrategyAiCheckType.OpponentGamePointNotGreaterThanSelf,
				OpponentGamePointNotGreaterThanSelf
			},
			{
				EDebateStrategyAiCheckType.SelfNotCheckMate,
				CheckSelfNotCheckMate
			}
		};
	}

	public void Initialize(DataContext context)
	{
		_lineWeight = new List<(int, int, int)>();
		_lineBases = new List<IntPair>();
		_lineCanMakeMoveNodes = new List<IntPair>();
		_strategyCards = new List<short>();
		for (int i = 0; i < DebateConstants.DebateLineCount; i++)
		{
			int lineIndex = GetLineIndex(i);
			if (1 == 0)
			{
			}
			int num = lineIndex switch
			{
				0 => context.Random.Next(DebateAiConstants.AttackLineWeight[_behaviorType][0], DebateAiConstants.AttackLineWeight[_behaviorType][1]), 
				1 => context.Random.Next(DebateAiConstants.MidLineWeight[_behaviorType][0], DebateAiConstants.MidLineWeight[_behaviorType][1]), 
				2 => context.Random.Next(DebateAiConstants.DefenseLineWeight[_behaviorType][0], DebateAiConstants.DefenseLineWeight[_behaviorType][1]), 
				_ => throw new ArgumentOutOfRangeException(), 
			};
			if (1 == 0)
			{
			}
			int item = num;
			_lineWeight.Add((i, item, 100));
		}
	}

	public void Start(DataContext context)
	{
		int num = 0;
		while (GetCanMakeMove() && num++ < 10)
		{
			UpdateState();
			UpdateLineWeight();
			UpdateLineBases();
			ProcessStrategy(context, isBeforeMakeMove: true);
			ProcessMakeMove(context);
			ProcessStrategy(context, isBeforeMakeMove: false);
		}
	}

	public void UpdateLineWeightByDamage(bool isPawnOwner, int y)
	{
		int num = (isPawnOwner ? DebateAiConstants.DamageLineWeight[_behaviorType] : DebateAiConstants.DamagedLineWeight[_behaviorType]);
		for (int i = 0; i < _lineWeight.Count; i++)
		{
			if (_lineWeight[i].Item1 == y)
			{
				_lineWeight[i] = (y, _lineWeight[i].Item2 * num, _lineWeight[i].Item3);
				break;
			}
		}
	}

	public List<int> GetRemovingCards(DataContext context)
	{
		List<int> list = new List<int>();
		if (!Debate.TryGetPlayerCardRemovingCount(_isTaiwu, out var _))
		{
			return list;
		}
		List<short> canUseCards = Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu).CanUseCards;
		for (int i = 0; i < canUseCards.Count; i++)
		{
			list.Add(i);
		}
		CollectionUtils.Shuffle(context.Random, list);
		for (int j = 0; j < GlobalConfig.Instance.DebateMaxCanUseCards; j++)
		{
			list.RemoveAt(0);
		}
		return list;
	}

	private bool GetCanMakeMove()
	{
		if (Debate.GetPlayerCanMakeMove(_isTaiwu))
		{
			return true;
		}
		for (int i = 0; i < DebateConstants.DebateLineCount; i++)
		{
			for (int j = 0; j < DebateConstants.DebateLineNodeCount; j++)
			{
				IntPair coordinate = new IntPair(j, i);
				if (Debate.GetNodeCanMakeMove(coordinate, _isTaiwu) && Debate.GetNodeIsContainingEffect(coordinate, 41))
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool TryMakeMove(DataContext context, IntPair coordinate, sbyte grade)
	{
		if (!Debate.GetNodeCanMakeMove(coordinate, _isTaiwu))
		{
			return false;
		}
		DomainManager.Taiwu.DebateGameMakeMove(context, coordinate, _isTaiwu, grade);
		return true;
	}

	private bool TryMakeMove(DataContext context, int y, sbyte grade, sbyte type)
	{
		int num = 0;
		int num2 = 1;
		switch (type)
		{
		case 2:
		{
			_lineCanMakeMoveNodes.Clear();
			for (int i = 0; i < DebateConstants.DebateLineNodeCount; i++)
			{
				if (Debate.GetNodeCanMakeMove(new IntPair(i, y), _isTaiwu))
				{
					_lineCanMakeMoveNodes.Add(new IntPair(i, y));
				}
			}
			if (_lineCanMakeMoveNodes.Count <= 0)
			{
				return false;
			}
			CollectionUtils.Shuffle(context.Random, _lineCanMakeMoveNodes);
			DomainManager.Taiwu.DebateGameMakeMove(context, _lineCanMakeMoveNodes[0], _isTaiwu, grade);
			return true;
		}
		case 0:
			num = (_isTaiwu ? (DebateConstants.DebateLineNodeCount - 1) : 0);
			num2 = ((!_isTaiwu) ? 1 : (-1));
			break;
		case 1:
			num = ((!_isTaiwu) ? (DebateConstants.DebateLineNodeCount - 1) : 0);
			num2 = (_isTaiwu ? 1 : (-1));
			break;
		}
		foreach (var item in _lineWeight)
		{
			for (int j = num; Debate.GetCoordinateValid(j); j += num2)
			{
				IntPair coordinate = new IntPair(j, item.Item1);
				if (Debate.GetNodeCanMakeMove(coordinate, _isTaiwu))
				{
					DomainManager.Taiwu.DebateGameMakeMove(context, coordinate, _isTaiwu, grade);
					return true;
				}
			}
		}
		return false;
	}

	private void MakeMoveByWeight(DataContext context, sbyte grade, sbyte type)
	{
		using List<(int, int, int)>.Enumerator enumerator = _lineWeight.GetEnumerator();
		while (enumerator.MoveNext() && !TryMakeMove(context, enumerator.Current.Item1, grade, type))
		{
		}
	}

	private void MakeMoveByBases(DataContext context, sbyte grade, sbyte type, bool isReversed)
	{
		_lineBases.Sort(isReversed ? new Comparison<IntPair>(CompareLineBasesReversed) : new Comparison<IntPair>(CompareLineBases));
		using List<IntPair>.Enumerator enumerator = _lineBases.GetEnumerator();
		while (enumerator.MoveNext() && !TryMakeMove(context, enumerator.Current.First, grade, type))
		{
		}
	}

	private void MakeMoveByBehavior(DataContext context, sbyte grade)
	{
		switch (_behaviorType)
		{
		case 0:
		{
			if (!TryGetDisadvantageLine(out var y6) || !TryMakeMove(context, y6, grade, 2))
			{
				MakeMoveByWeight(context, grade, 2);
			}
			break;
		}
		case 1:
		{
			if (!TryGetDisadvantageLine(out var y3) || !TryMakeMove(context, y3, grade, 1))
			{
				MakeMoveByWeight(context, grade, 1);
			}
			break;
		}
		case 2:
		{
			DebatePlayer playerByPlayerIsTaiwu3 = Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu);
			DebatePlayer playerByPlayerIsTaiwu4 = Debate.GetPlayerByPlayerIsTaiwu(!_isTaiwu);
			int y5;
			if (playerByPlayerIsTaiwu3.MaxBases >= playerByPlayerIsTaiwu4.MaxBases)
			{
				if (!TryGetDisadvantageLine(out var y4) || !TryMakeMove(context, y4, grade, 2))
				{
					MakeMoveByWeight(context, grade, 2);
				}
			}
			else if (!TryGetEmptyOpponentPawnLine(out y5) || !TryMakeMove(context, y5, grade, 0))
			{
				MakeMoveByWeight(context, grade, 0);
			}
			break;
		}
		case 3:
		{
			DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu);
			DebatePlayer playerByPlayerIsTaiwu2 = Debate.GetPlayerByPlayerIsTaiwu(!_isTaiwu);
			int y2;
			if (playerByPlayerIsTaiwu.MaxBases >= playerByPlayerIsTaiwu2.MaxBases)
			{
				if (!TryGetDisadvantageLine(out var y) || !TryMakeMove(context, y, grade, 0))
				{
					MakeMoveByWeight(context, grade, 0);
				}
			}
			else if (!TryGetEmptyOpponentPawnLine(out y2) || !TryMakeMove(context, y2, grade, 0))
			{
				MakeMoveByWeight(context, grade, 0);
			}
			break;
		}
		case 4:
			MakeMoveByWeight(context, grade, 2);
			break;
		}
	}

	private void CastStrategy(DataContext context, short templateId)
	{
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu);
		DebateStrategyItem debateStrategyItem = DebateStrategy.Instance[templateId];
		int num = -1;
		for (int i = 0; i < playerByPlayerIsTaiwu.CanUseCards.Count; i++)
		{
			short num2 = playerByPlayerIsTaiwu.CanUseCards[i];
			if (num2 == templateId)
			{
				num = i;
				break;
			}
		}
		if (num < 0 || debateStrategyItem.UsedCost > playerByPlayerIsTaiwu.StrategyPoint || !Debate.TryGetStrategyTarget(templateId, _isTaiwu, out var res))
		{
			return;
		}
		if (res != null)
		{
			for (int j = 0; j < res.Count; j++)
			{
				StrategyTarget strategyTarget = res[j];
				Debate.AddStrategyRepeatedTargets(debateStrategyItem.TargetList[j][0], strategyTarget.List);
				CollectionUtils.Shuffle(context.Random, strategyTarget.List);
			}
		}
		PrepareStrategyTargetActions[templateId](res);
		if (TrySelectTargetGeneral(templateId, res) && CheckStrategyCanUse(templateId, res))
		{
			DomainManager.Taiwu.DebateGameCastStrategy(context, num, _isTaiwu, res);
		}
	}

	private void UpdateState()
	{
		if (Debate.Round > DebateAiConstants.StateRoundInfluence[_behaviorType])
		{
			_state = 2;
			return;
		}
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu);
		int num = playerByPlayerIsTaiwu.GamePoint * 100 / DebateConstants.MaxGamePoint - playerByPlayerIsTaiwu.Pressure * 100 / playerByPlayerIsTaiwu.MaxPressure;
		if (num <= DebateAiConstants.StateGamePointPressureInfluence[_behaviorType][1])
		{
			_state = 2;
			return;
		}
		_state = ((num <= DebateAiConstants.StateGamePointPressureInfluence[_behaviorType][0]) ? 1 : 0);
		_pawnDiff = GetPawnDiff();
		if (_pawnDiff >= DebateAiConstants.StatePawnCountInfluence[_behaviorType][1])
		{
			_state = 2;
		}
		else if (_pawnDiff >= DebateAiConstants.StatePawnCountInfluence[_behaviorType][0])
		{
			_state = 1;
		}
	}

	private void UpdateLineWeight()
	{
		for (int i = 0; i < _lineWeight.Count; i++)
		{
			int item = _lineWeight[i].Item1;
			int num = 100;
			if (Debate.DebateGrid[new IntPair(0, item)].EffectState.TemplateId == 4)
			{
				num += DebateAiConstants.EgoisticNodeEffectWeightPercent;
			}
			if (Debate.DebateGrid[new IntPair(DebateConstants.DebateLineNodeCount - 1, item)].EffectState.TemplateId == 4)
			{
				num += DebateAiConstants.EgoisticNodeEffectWeightPercent;
			}
			_lineWeight[i] = (item, _lineWeight[i].Item2, num);
		}
		_lineWeight.Sort(CompareLineWeight);
	}

	private void UpdateLineBases()
	{
		_lineBases.Clear();
		for (int i = 0; i < DebateConstants.DebateLineCount; i++)
		{
			int num = 0;
			for (int j = 0; j < DebateConstants.DebateLineNodeCount; j++)
			{
				int pawnId = Debate.DebateGrid[new IntPair(j, i)].PawnId;
				if (pawnId >= 0 && Debate.Pawns[pawnId].IsOwnedByTaiwu == _isTaiwu)
				{
					num += Debate.GetPawnBases(pawnId, -1, isReal: false, _isTaiwu);
				}
			}
			_lineBases.Add(new IntPair(i, num));
		}
	}

	private void ProcessStrategy(DataContext context, bool isBeforeMakeMove)
	{
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu);
		if (!isBeforeMakeMove)
		{
			_strategyCards.Clear();
			foreach (short canUseCard in playerByPlayerIsTaiwu.CanUseCards)
			{
				if (DebateStrategy.Instance[canUseCard].AvoidCheckMate)
				{
					_strategyCards.Add(canUseCard);
				}
			}
			_strategyCards.Sort(CompareStrategy);
			foreach (short strategyCard in _strategyCards)
			{
				if (!TryGetCheckMatePawn(_isTaiwu, checkBeatable: false, out var _, out var _))
				{
					break;
				}
				CastStrategy(context, strategyCard);
			}
		}
		int strategyPointKept = GetStrategyPointKept(isBeforeMakeMove);
		if (playerByPlayerIsTaiwu.StrategyPoint < strategyPointKept)
		{
			return;
		}
		_strategyCards.Clear();
		foreach (short canUseCard2 in playerByPlayerIsTaiwu.CanUseCards)
		{
			if (DebateStrategy.Instance[canUseCard2].UseBeforeMakeMove == isBeforeMakeMove)
			{
				_strategyCards.Add(canUseCard2);
			}
		}
		_strategyCards.Sort(CompareStrategy);
		foreach (short strategyCard2 in _strategyCards)
		{
			if (playerByPlayerIsTaiwu.StrategyPoint < strategyPointKept)
			{
				return;
			}
			CastStrategy(context, strategyCard2);
		}
		int item = Debate.GetGamePointAndPressureDelta(playerByPlayerIsTaiwu, GlobalConfig.Instance.DebateResetCardsPressureDelta).gamePoint;
		if (Debate.GetPlayerCanUseResetStrategy(_isTaiwu) && item + playerByPlayerIsTaiwu.GamePoint > 0 && playerByPlayerIsTaiwu.UsedCards.Count > GlobalConfig.Instance.ResetStrategyUsedCardLimit)
		{
			DomainManager.Taiwu.DebateGameResetCards(_isTaiwu);
		}
		DomainManager.Taiwu.DebateGameRemoveCards(_isTaiwu, GetRemovingCards(context));
	}

	private void ProcessMakeMove(DataContext context)
	{
		if (TryGetCheckMatePawn(_isTaiwu, checkBeatable: true, out var resGrade, out var resCoordinate) && TryGetMaxGradeCanMakeMove(8, resGrade, out var grade) && TryMakeMove(context, resCoordinate, (sbyte)context.Random.Next((int)resGrade, grade + 1)))
		{
			return;
		}
		TryGetMaxGradeCanMakeMove(8, 0, out grade);
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu);
		DebatePlayer playerByPlayerIsTaiwu2 = Debate.GetPlayerByPlayerIsTaiwu(!_isTaiwu);
		foreach (DebateNode value in Debate.DebateGrid.Values)
		{
			if (value.EffectState.TemplateId == 1 && TryMakeMove(context, GetBehindCoordinate(value.Coordinate), grade))
			{
				return;
			}
		}
		foreach (DebateNode value2 in Debate.DebateGrid.Values)
		{
			if (value2.EffectState.TemplateId == 2 && TryMakeMove(context, value2.Coordinate, (sbyte)(context.Random.CheckPercentProb(DebateAiConstants.EvenNodeEffectMaxGradeProb[_behaviorType]) ? grade : 0)))
			{
				return;
			}
		}
		if (_behaviorType == 3)
		{
			foreach (DebateNode value3 in Debate.DebateGrid.Values)
			{
				if (value3.EffectState.TemplateId == 3 && TryMakeMove(context, GetBehindCoordinate(value3.Coordinate), grade))
				{
					return;
				}
			}
		}
		if (Debate.Round <= DebateAiConstants.RoundBeforeEarly)
		{
			MakeMoveByBehavior(context, grade);
			return;
		}
		sbyte grade2 = (sbyte)(GetBasesEnough() ? context.Random.Next(DebateAiConstants.MinGradeIfEnoughBases, grade + 1) : ((!context.Random.CheckPercentProb(DebateAiConstants.ZeroGradePawnProb[_state])) ? grade : 0));
		if (_state == 2)
		{
			if (_pawnDiff + playerByPlayerIsTaiwu.GamePoint - playerByPlayerIsTaiwu2.GamePoint >= 0)
			{
				MakeMoveByBases(context, grade2, 2, isReversed: false);
			}
			else if (context.Random.CheckPercentProb(DebateAiConstants.MakeMoveOnOverwhelmingLineProb))
			{
				MakeMoveByBases(context, grade2, 2, isReversed: true);
			}
			else
			{
				MakeMoveByWeight(context, grade2, 2);
			}
		}
		else
		{
			MakeMoveByWeight(context, grade2, 2);
		}
	}

	private bool GetGamePointNotFull()
	{
		return Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu).GamePoint < DebateConstants.MaxGamePoint;
	}

	private int GetBasesPercent()
	{
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu);
		return (playerByPlayerIsTaiwu.MaxBases != 0) ? (playerByPlayerIsTaiwu.Bases * 100 / playerByPlayerIsTaiwu.MaxBases) : 0;
	}

	private bool GetBasesEnough()
	{
		int state = _state;
		if (1 == 0)
		{
		}
		int num = state switch
		{
			0 => DebateAiConstants.EarlyBases[_behaviorType], 
			1 => DebateAiConstants.MidBases[_behaviorType], 
			_ => DebateAiConstants.LateBases[_behaviorType], 
		};
		if (1 == 0)
		{
		}
		int num2 = num;
		return GetBasesPercent() >= num2;
	}

	private int GetStrategyPointKept(bool beforeMakeMove)
	{
		int num = ((!beforeMakeMove) ? 1 : 0);
		int state = _state;
		if (1 == 0)
		{
		}
		int result = state switch
		{
			0 => DebateAiConstants.EarlyStrategyPoint[_behaviorType][num], 
			1 => DebateAiConstants.MidStrategyPoint[_behaviorType][num], 
			_ => DebateAiConstants.LateStrategyPoint[_behaviorType][num], 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private int GetLineIndex(int y)
	{
		return _isTaiwu ? y : Math.Abs(y - 2);
	}

	private int GetCheckMatePosition(bool isTaiwu)
	{
		return (!isTaiwu) ? 1 : (DebateConstants.DebateLineNodeCount - 2);
	}

	private IntPair GetBehindCoordinate(IntPair coordinate)
	{
		return new IntPair(_isTaiwu ? (coordinate.First - 1) : (coordinate.First + 1), coordinate.Second);
	}

	private int GetPawnDiff()
	{
		int num = 0;
		int num2 = 0;
		foreach (Pawn value in Debate.Pawns.Values)
		{
			if (value.IsAlive)
			{
				if (value.IsOwnedByTaiwu == _isTaiwu)
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
		}
		return num2 - num;
	}

	private bool CheckStrategyCanUse(short templateId, List<StrategyTarget> targets)
	{
		DebateStrategyItem debateStrategyItem = DebateStrategy.Instance[templateId];
		int state = _state;
		if (1 == 0)
		{
		}
		List<EDebateStrategyAiCheckType> list = state switch
		{
			0 => debateStrategyItem.EarlyLimits, 
			1 => debateStrategyItem.MidLimits, 
			2 => debateStrategyItem.LateLimits, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
		if (1 == 0)
		{
		}
		List<EDebateStrategyAiCheckType> list2 = list;
		int state2 = _state;
		if (1 == 0)
		{
		}
		List<int> list3 = state2 switch
		{
			0 => debateStrategyItem.EarlyLimitParams, 
			1 => debateStrategyItem.MidLimitParams, 
			2 => debateStrategyItem.LateLimitParams, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
		if (1 == 0)
		{
		}
		List<int> list4 = list3;
		if (list2 == null || list2.Count == 0)
		{
			return true;
		}
		for (int i = 0; i < list2.Count; i++)
		{
			if (!StrategyCheckers[list2[i]](targets, list4[i]))
			{
				return false;
			}
		}
		return true;
	}

	private bool TrySelectTargetGeneral(short templateId, List<StrategyTarget> targets)
	{
		if (targets == null)
		{
			return true;
		}
		DebateStrategyItem debateStrategyItem = DebateStrategy.Instance[templateId];
		for (int i = 0; i < targets.Count; i++)
		{
			StrategyTarget strategyTarget = targets[i];
			short num = debateStrategyItem.TargetList[i][1];
			short num2 = debateStrategyItem.TargetList[i][2];
			if (strategyTarget == null)
			{
				return false;
			}
			if (i > 0 && strategyTarget.Type == targets[i - 1].Type)
			{
				Debate.CullStrategyTargets(targets[i - 1].List, strategyTarget.List);
			}
			if (strategyTarget.List.Count > num2)
			{
				while (strategyTarget.List.Count > num2)
				{
					strategyTarget.List.RemoveAt(strategyTarget.List.Count - 1);
				}
			}
			else if (strategyTarget.List.Count < num)
			{
				return false;
			}
		}
		sbyte usedCost = debateStrategyItem.UsedCost;
		int num3 = Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu).StrategyPoint - usedCost;
		for (int j = 0; j < targets.Count; j++)
		{
			StrategyTarget strategyTarget2 = targets[j];
			short num4 = debateStrategyItem.TargetList[j][1];
			if (strategyTarget2.Type != EDebateStrategyTargetObjectType.Pawn)
			{
				continue;
			}
			for (int k = 0; k < strategyTarget2.List.Count; k++)
			{
				if (!Debate.TryGetPawnStrategyEffectValue((int)strategyTarget2.List[k], 28, out var value))
				{
					continue;
				}
				num3 -= value;
				if (num3 < 0)
				{
					num3 += value;
					strategyTarget2.List.RemoveAt(k--);
					if (strategyTarget2.List.Count < num4)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	private bool TryGetCheckMatePawn(bool isTaiwu, bool checkBeatable, out sbyte resGrade, out IntPair resCoordinate)
	{
		int checkMatePosition = GetCheckMatePosition(!isTaiwu);
		bool result = false;
		resGrade = 8;
		resCoordinate = default(IntPair);
		for (int i = 0; i < DebateConstants.DebateLineCount; i++)
		{
			int pawnId = Debate.DebateGrid[new IntPair(checkMatePosition, i)].PawnId;
			IntPair startCoordinate = Debate.GetStartCoordinate(isTaiwu, i);
			sbyte grade = 0;
			if (pawnId >= 0 && Debate.Pawns[pawnId].IsOwnedByTaiwu != isTaiwu && Debate.DebateGrid[startCoordinate].PawnId < 0 && (!checkBeatable || TryGetRequiredGradeToBeatPawn(pawnId, out grade)) && grade <= resGrade)
			{
				result = true;
				resGrade = grade;
				resCoordinate = startCoordinate;
			}
		}
		return result;
	}

	private bool TryGetRequiredGradeToBeatPawn(int pawnId, out sbyte grade)
	{
		int num = Debate.GetPawnBases(pawnId, -1, isReal: false, _isTaiwu);
		grade = -1;
		if (Debate.TryGetPawnRevealedStrategyEffectValue(pawnId, 6, _isTaiwu, out var value))
		{
			num = num * 100 / value;
		}
		for (sbyte b = 0; b < 8; b++)
		{
			if (Debate.GetPawnInitialBases(_isTaiwu, b, 0) > num)
			{
				grade = b;
				return true;
			}
		}
		return false;
	}

	private bool TryGetMaxGradeCanMakeMove(sbyte maxGrade, sbyte minGrade, out sbyte grade)
	{
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu);
		for (grade = maxGrade; grade >= minGrade; grade--)
		{
			if (playerByPlayerIsTaiwu.Bases >= Debate.GetPawnGradeToBase(playerByPlayerIsTaiwu.MaxBases, grade))
			{
				return true;
			}
		}
		return false;
	}

	private bool TryGetDisadvantageLine(out int y)
	{
		y = -1;
		foreach (var item in _lineWeight)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < DebateConstants.DebateLineNodeCount; i++)
			{
				int pawnId = Debate.DebateGrid[new IntPair(i, item.Item1)].PawnId;
				if (pawnId >= 0)
				{
					if (Debate.Pawns[pawnId].IsOwnedByTaiwu == _isTaiwu)
					{
						num++;
					}
					else
					{
						num2++;
					}
				}
			}
			if (num == 0 && num2 != 0)
			{
				(y, _, _) = item;
				return true;
			}
		}
		return false;
	}

	private bool TryGetEmptyOpponentPawnLine(out int y)
	{
		y = -1;
		foreach (var item in _lineWeight)
		{
			int num = 0;
			for (int i = 0; i < DebateConstants.DebateLineNodeCount; i++)
			{
				int pawnId = Debate.DebateGrid[new IntPair(i, item.Item1)].PawnId;
				if (pawnId >= 0 && Debate.Pawns[pawnId].IsOwnedByTaiwu != _isTaiwu)
				{
					num++;
				}
			}
			if (num == 0)
			{
				(y, _, _) = item;
				return true;
			}
		}
		return false;
	}

	private bool TryGetConflictingPawnId(int pawnId, out int otherId)
	{
		otherId = Debate.DebateGrid[Debate.GetPawnTargetPosition(pawnId)].PawnId;
		return otherId >= 0 && Debate.Pawns[pawnId].IsOwnedByTaiwu != Debate.Pawns[otherId].IsOwnedByTaiwu;
	}

	private int CompareLineWeight((int, int, int) a, (int, int, int) b)
	{
		int num = a.Item2 * a.Item3 / 100;
		int value = b.Item2 * b.Item3 / 100;
		return -num.CompareTo(value);
	}

	private int CompareLineBases(IntPair a, IntPair b)
	{
		return a.Second.CompareTo(b.Second);
	}

	private int CompareLineBasesReversed(IntPair a, IntPair b)
	{
		return -a.Second.CompareTo(b.Second);
	}

	private int CompareStrategy(short a, short b)
	{
		return a.CompareTo(b);
	}

	private void RemoveInvertBasesPawnTarget(StrategyTarget target)
	{
		for (int num = target.List.Count - 1; num >= 0; num--)
		{
			int pawnId = (int)target.List[num];
			if (Debate.TryGetPawnRevealedStrategyEffectValue(pawnId, 8, _isTaiwu, out var _))
			{
				target.List.RemoveAt(num);
			}
		}
	}

	private void RemoveCheckMatePawnTarget(StrategyTarget target)
	{
		for (int num = target.List.Count - 1; num >= 0; num--)
		{
			int key = (int)target.List[num];
			Pawn pawn = Debate.Pawns[key];
			if (pawn.Coordinate.First == GetCheckMatePosition(pawn.IsOwnedByTaiwu))
			{
				target.List.RemoveAt(num);
			}
		}
	}

	private void RemoveSpawnPawnTarget(StrategyTarget target)
	{
		for (int num = target.List.Count - 1; num >= 0; num--)
		{
			int key = (int)target.List[num];
			Pawn pawn = Debate.Pawns[key];
			if (pawn.Coordinate.First == Debate.GetStartCoordinate(pawn.IsOwnedByTaiwu))
			{
				target.List.RemoveAt(num);
			}
		}
	}

	private void RemoveGreaterConflictingPawnTarget(StrategyTarget target)
	{
		for (int num = target.List.Count - 1; num >= 0; num--)
		{
			int num2 = (int)target.List[num];
			if (TryGetConflictingPawnId(num2, out var otherId) && Debate.GetPawnBases(num2, otherId, isReal: false, _isTaiwu) > Debate.GetPawnBases(otherId, num2, isReal: false, _isTaiwu))
			{
				target.List.RemoveAt(num);
			}
		}
	}

	private void RemoveSmallerConflictingPawnTarget(StrategyTarget target)
	{
		for (int num = target.List.Count - 1; num >= 0; num--)
		{
			int num2 = (int)target.List[num];
			if (TryGetConflictingPawnId(num2, out var otherId) && Debate.GetPawnBases(num2, otherId, isReal: false, _isTaiwu) <= Debate.GetPawnBases(otherId, num2, isReal: false, _isTaiwu))
			{
				target.List.RemoveAt(num);
			}
		}
	}

	private void RemoveSmallerPawnBasesPercentTarget(StrategyTarget target)
	{
		int maxBases = Debate.GetPlayerByPlayerIsTaiwu(isTaiwu: true).MaxBases;
		int maxBases2 = Debate.GetPlayerByPlayerIsTaiwu(isTaiwu: false).MaxBases;
		for (int num = target.List.Count - 1; num >= 0; num--)
		{
			int num2 = (int)target.List[num];
			int num3 = (Debate.Pawns[num2].IsOwnedByTaiwu ? maxBases : maxBases2);
			if (Debate.GetPawnBases(num2) < num3 * DebateAiConstants.RemoveStrategyTargetPawnBasesPercent / 100)
			{
				target.List.RemoveAt(num);
			}
		}
	}

	private void RemovePlayerConflictingPawnTarget(StrategyTarget target)
	{
		for (int num = target.List.Count - 1; num >= 0; num--)
		{
			int num2 = (int)target.List[num];
			if (TryGetConflictingPawnId(num2, out var otherId))
			{
				int pawnBases = Debate.GetPawnBases(num2, otherId, isReal: false, _isTaiwu);
				int pawnBases2 = Debate.GetPawnBases(otherId, num2, isReal: false, _isTaiwu);
				if ((Debate.Pawns[num2].IsOwnedByTaiwu == _isTaiwu && pawnBases > pawnBases2) || (Debate.Pawns[num2].IsOwnedByTaiwu != _isTaiwu && pawnBases <= pawnBases2))
				{
					target.List.RemoveAt(num);
				}
			}
		}
	}

	private void RemoveFactorPawnTarget(StrategyTarget target)
	{
		for (int num = target.List.Count - 1; num >= 0; num--)
		{
			int num2 = (int)target.List[num];
			Pawn pawn = Debate.Pawns[num2];
			if ((pawn.IsOwnedByTaiwu == _isTaiwu && Debate.GetPawnBasesFactor(num2, isReal: false, _isTaiwu) > 0) || (pawn.IsOwnedByTaiwu != _isTaiwu && Debate.GetPawnBasesFactor(num2, isReal: false, _isTaiwu) < 0))
			{
				target.List.RemoveAt(num);
			}
		}
	}

	private void RemoveSelfPawnTarget(StrategyTarget target)
	{
		for (int num = target.List.Count - 1; num >= 0; num--)
		{
			int key = (int)target.List[num];
			Pawn pawn = Debate.Pawns[key];
			if (pawn.IsOwnedByTaiwu == _isTaiwu)
			{
				target.List.RemoveAt(num);
			}
		}
	}

	private void RemoveOpponentPawnTarget(StrategyTarget target)
	{
		for (int num = target.List.Count - 1; num >= 0; num--)
		{
			int key = (int)target.List[num];
			Pawn pawn = Debate.Pawns[key];
			if (pawn.IsOwnedByTaiwu != _isTaiwu)
			{
				target.List.RemoveAt(num);
			}
		}
	}

	private void RemoveOpponentNonCheckMatePawnTarget(StrategyTarget target)
	{
		int checkMatePosition = GetCheckMatePosition(!_isTaiwu);
		for (int num = target.List.Count - 1; num >= 0; num--)
		{
			int key = (int)target.List[num];
			Pawn pawn = Debate.Pawns[key];
			if (pawn.IsOwnedByTaiwu != _isTaiwu && pawn.Coordinate.First != checkMatePosition)
			{
				target.List.RemoveAt(num);
			}
		}
	}

	private void RemoveNonOpponentStrategyPawnTarget(StrategyTarget target)
	{
		for (int num = target.List.Count - 1; num >= 0; num--)
		{
			int key = (int)target.List[num];
			Pawn pawn = Debate.Pawns[key];
			bool flag = false;
			int[] strategies = pawn.Strategies;
			foreach (int num2 in strategies)
			{
				if (num2 >= 0)
				{
					ActivatedStrategy activatedStrategy = Debate.ActivatedStrategies[num2];
					if (activatedStrategy.IsCastedByTaiwu != _isTaiwu)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				target.List.RemoveAt(num);
			}
		}
	}

	private void RemoveLessOpponentStrategyPawnTarget(StrategyTarget target)
	{
		for (int num = target.List.Count - 1; num >= 0; num--)
		{
			int key = (int)target.List[num];
			Pawn pawn = Debate.Pawns[key];
			int num2 = 0;
			int num3 = 0;
			int[] strategies = pawn.Strategies;
			foreach (int num4 in strategies)
			{
				if (num4 >= 0)
				{
					if (Debate.ActivatedStrategies[num4].IsCastedByTaiwu == _isTaiwu)
					{
						num2++;
					}
					else
					{
						num3++;
					}
				}
			}
			if (num3 == 0 || num2 >= num3)
			{
				target.List.RemoveAt(num);
			}
		}
	}

	private void RemoveFreeCard(StrategyTarget target)
	{
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu);
		for (int num = target.List.Count - 1; num >= 0; num--)
		{
			int index = (int)target.List[num];
			if (DebateStrategy.Instance[playerByPlayerIsTaiwu.CanUseCards[index]].UsedCost == 0)
			{
				target.List.RemoveAt(num);
			}
		}
	}

	private int ComparePawnByDistance(ulong a, ulong b)
	{
		Pawn pawn = Debate.Pawns[(int)a];
		Pawn pawn2 = Debate.Pawns[(int)b];
		int num = Math.Abs(pawn.Coordinate.First - Debate.GetStartCoordinate(pawn.IsOwnedByTaiwu));
		int value = Math.Abs(pawn2.Coordinate.First - Debate.GetStartCoordinate(pawn2.IsOwnedByTaiwu));
		return num.CompareTo(value);
	}

	private int ComparePawnByDistanceReversed(ulong a, ulong b)
	{
		Pawn pawn = Debate.Pawns[(int)a];
		Pawn pawn2 = Debate.Pawns[(int)b];
		int num = Math.Abs(pawn.Coordinate.First - Debate.GetStartCoordinate(pawn.IsOwnedByTaiwu));
		int value = Math.Abs(pawn2.Coordinate.First - Debate.GetStartCoordinate(pawn2.IsOwnedByTaiwu));
		return -num.CompareTo(value);
	}

	private int ComparePawnByBasesFactorAndPlayer(ulong a, ulong b)
	{
		Pawn pawn = Debate.Pawns[(int)a];
		Pawn pawn2 = Debate.Pawns[(int)b];
		int num = Debate.GetPawnBasesFactor(pawn.Id, isReal: false, _isTaiwu) * ((pawn.IsOwnedByTaiwu == _isTaiwu) ? 1 : (-1));
		int value = Debate.GetPawnBasesFactor(pawn2.Id, isReal: false, _isTaiwu) * ((pawn2.IsOwnedByTaiwu == _isTaiwu) ? 1 : (-1));
		return num.CompareTo(value);
	}

	private int ComparePawnByDistanceReversedAndConflict(ulong a, ulong b)
	{
		Pawn pawn = Debate.Pawns[(int)a];
		Pawn pawn2 = Debate.Pawns[(int)b];
		int num = Math.Abs(pawn.Coordinate.First - Debate.GetStartCoordinate(pawn.IsOwnedByTaiwu));
		int num2 = Math.Abs(pawn2.Coordinate.First - Debate.GetStartCoordinate(pawn2.IsOwnedByTaiwu));
		if (num == DebateConstants.DebateLineNodeCount - 2)
		{
			return -1;
		}
		if (num2 == DebateConstants.DebateLineNodeCount - 2)
		{
			return 1;
		}
		int otherId;
		bool flag = TryGetConflictingPawnId(pawn.Id, out otherId);
		bool flag2 = TryGetConflictingPawnId(pawn2.Id, out otherId);
		if (flag)
		{
			return -1;
		}
		if (flag2)
		{
			return 1;
		}
		return -num.CompareTo(num2);
	}

	private int ComparePawnByBases(ulong a, ulong b)
	{
		Pawn pawn = Debate.Pawns[(int)a];
		Pawn pawn2 = Debate.Pawns[(int)b];
		int pawnBases = Debate.GetPawnBases(pawn.Id, -1, isReal: false, _isTaiwu);
		int pawnBases2 = Debate.GetPawnBases(pawn2.Id, -1, isReal: false, _isTaiwu);
		return pawnBases.CompareTo(pawnBases2);
	}

	private int ComparePawnByBasesReversed(ulong a, ulong b)
	{
		Pawn pawn = Debate.Pawns[(int)a];
		Pawn pawn2 = Debate.Pawns[(int)b];
		int pawnBases = Debate.GetPawnBases(pawn.Id, -1, isReal: false, _isTaiwu);
		int pawnBases2 = Debate.GetPawnBases(pawn2.Id, -1, isReal: false, _isTaiwu);
		return -pawnBases.CompareTo(pawnBases2);
	}

	private int ComparePawnByBasesAndConflict(ulong a, ulong b)
	{
		Pawn pawn = Debate.Pawns[(int)a];
		Pawn pawn2 = Debate.Pawns[(int)b];
		int otherId;
		bool flag = TryGetConflictingPawnId(pawn.Id, out otherId);
		bool flag2 = TryGetConflictingPawnId(pawn2.Id, out otherId);
		if (flag && !flag2)
		{
			return -1;
		}
		if (!flag && flag2)
		{
			return 1;
		}
		int num = Debate.GetPawnBases(pawn.Id, -1, isReal: false, _isTaiwu) * ((pawn.IsOwnedByTaiwu == _isTaiwu) ? 1 : (-1));
		int value = Debate.GetPawnBases(pawn2.Id, -1, isReal: false, _isTaiwu) * ((pawn2.IsOwnedByTaiwu == _isTaiwu) ? 1 : (-1));
		return -num.CompareTo(value);
	}

	private int ComparePawnByOpponentStrategyCount(ulong a, ulong b)
	{
		Pawn pawn = Debate.Pawns[(int)a];
		Pawn pawn2 = Debate.Pawns[(int)b];
		int num = 0;
		int num2 = 0;
		int[] strategies = pawn.Strategies;
		foreach (int num3 in strategies)
		{
			if (num3 >= 0 && Debate.ActivatedStrategies[num3].IsCastedByTaiwu != _isTaiwu)
			{
				num++;
			}
		}
		int[] strategies2 = pawn2.Strategies;
		foreach (int num4 in strategies2)
		{
			if (num4 >= 0 && Debate.ActivatedStrategies[num4].IsCastedByTaiwu != _isTaiwu)
			{
				num2++;
			}
		}
		return num.CompareTo(num2);
	}

	private int ComparePawnByOpponentStrategyCountReversed(ulong a, ulong b)
	{
		Pawn pawn = Debate.Pawns[(int)a];
		Pawn pawn2 = Debate.Pawns[(int)b];
		int num = 0;
		int num2 = 0;
		int[] strategies = pawn.Strategies;
		foreach (int num3 in strategies)
		{
			if (num3 >= 0 && Debate.ActivatedStrategies[num3].IsCastedByTaiwu != _isTaiwu)
			{
				num++;
			}
		}
		int[] strategies2 = pawn2.Strategies;
		foreach (int num4 in strategies2)
		{
			if (num4 >= 0 && Debate.ActivatedStrategies[num4].IsCastedByTaiwu != _isTaiwu)
			{
				num2++;
			}
		}
		return -num.CompareTo(num2);
	}

	private int CompareNodeByDistance(ulong a, ulong b)
	{
		DebateNode debateNode = Debate.DebateGrid[(IntPair)a];
		DebateNode debateNode2 = Debate.DebateGrid[(IntPair)b];
		int num = Math.Abs(debateNode.Coordinate.First - Debate.GetStartCoordinate(!_isTaiwu));
		int value = Math.Abs(debateNode2.Coordinate.First - Debate.GetStartCoordinate(!_isTaiwu));
		return num.CompareTo(value);
	}

	private int CompareNodeByVantageDistanceReversed(ulong a, ulong b)
	{
		DebateNode debateNode = Debate.DebateGrid[(IntPair)a];
		DebateNode debateNode2 = Debate.DebateGrid[(IntPair)b];
		int num = Math.Abs(debateNode.Coordinate.First - Debate.GetStartCoordinate(debateNode.IsVantage));
		int value = Math.Abs(debateNode2.Coordinate.First - Debate.GetStartCoordinate(debateNode2.IsVantage));
		return -num.CompareTo(value);
	}

	private bool CheckOpponentUsedCardCountGreater(List<StrategyTarget> targets, int value)
	{
		return Debate.GetPlayerByPlayerIsTaiwu(!_isTaiwu).UsedCards.Count > value;
	}

	private bool CheckOpponentOwnedCardCountGreater(List<StrategyTarget> targets, int value)
	{
		return Debate.GetPlayerByPlayerIsTaiwu(!_isTaiwu).OwnedCards.Count > value;
	}

	private bool CheckSelfCanUseCardCountSmaller(List<StrategyTarget> targets, int value)
	{
		return Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu).CanUseCards.Count < value;
	}

	private bool CheckSelfCanUseCardCountGreater(List<StrategyTarget> targets, int value)
	{
		return Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu).CanUseCards.Count > value;
	}

	private bool CheckSelfPawnCountGreater(List<StrategyTarget> targets, int value)
	{
		return Debate.GetPawnCount(_isTaiwu) > value;
	}

	private bool CheckSelfBasesGreater(List<StrategyTarget> targets, int value)
	{
		return GetBasesPercent() > value;
	}

	private bool CheckSelfStrategyPointSmaller(List<StrategyTarget> targets, int value)
	{
		return Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu).StrategyPoint < value;
	}

	private bool CheckTargetCountGreater(List<StrategyTarget> targets, int value)
	{
		int num = 0;
		foreach (StrategyTarget target in targets)
		{
			num += target.List.Count;
		}
		return num > value;
	}

	private bool CheckOpponentTargetCountGreater(List<StrategyTarget> targets, int value)
	{
		int num = 0;
		foreach (StrategyTarget target in targets)
		{
			if (target.Type != EDebateStrategyTargetObjectType.Pawn)
			{
				continue;
			}
			foreach (ulong item in target.List)
			{
				if (Debate.Pawns[(int)item].IsOwnedByTaiwu != _isTaiwu)
				{
					num++;
				}
			}
		}
		return num > value;
	}

	private bool CheckSelfGamePointSmaller(List<StrategyTarget> targets, int value)
	{
		return Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu).GamePoint < value;
	}

	private bool CheckSelfGamePointGreater(List<StrategyTarget> targets, int value)
	{
		return Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu).GamePoint > value;
	}

	private bool CheckOpponentGamePointGreater(List<StrategyTarget> targets, int value)
	{
		return Debate.GetPlayerByPlayerIsTaiwu(!_isTaiwu).GamePoint > value;
	}

	private bool OpponentGamePointNotGreaterThanSelf(List<StrategyTarget> targets, int value)
	{
		return Debate.GetPlayerByPlayerIsTaiwu(!_isTaiwu).GamePoint <= Debate.GetPlayerByPlayerIsTaiwu(_isTaiwu).GamePoint;
	}

	private bool CheckSelfNotCheckMate(List<StrategyTarget> targets, int value)
	{
		sbyte resGrade;
		IntPair resCoordinate;
		return !TryGetCheckMatePawn(!_isTaiwu, checkBeatable: false, out resGrade, out resCoordinate);
	}

	private void SelectTargetMusic1(List<StrategyTarget> targets)
	{
		bool flag = !Debate.GetPlayerCanMakeMove(_isTaiwu);
		foreach (StrategyTarget target in targets)
		{
			RemoveInvertBasesPawnTarget(target);
			if (flag)
			{
				RemoveCheckMatePawnTarget(target);
			}
		}
	}

	private void SelectTargetMusic2(List<StrategyTarget> targets)
	{
		bool flag = !Debate.GetPlayerCanMakeMove(_isTaiwu);
		foreach (StrategyTarget target in targets)
		{
			RemoveInvertBasesPawnTarget(target);
			if (flag)
			{
				RemoveCheckMatePawnTarget(target);
			}
		}
	}

	private void SelectTargetMusic3(List<StrategyTarget> targets)
	{
		bool flag = !Debate.GetPlayerCanMakeMove(_isTaiwu);
		foreach (StrategyTarget target in targets)
		{
			RemoveInvertBasesPawnTarget(target);
			if (flag)
			{
				RemoveCheckMatePawnTarget(target);
			}
		}
	}

	private void SelectTargetChess1(List<StrategyTarget> targets)
	{
		List<ulong> list = targets[1].List;
		TryGetMaxGradeCanMakeMove(8, 0, out var grade);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (num > grade || num <= grade - 3)
			{
				list.RemoveAt(num);
			}
		}
	}

	private void SelectTargetChess2(List<StrategyTarget> targets)
	{
	}

	private void SelectTargetChess3(List<StrategyTarget> targets)
	{
		RemoveCheckMatePawnTarget(targets[0]);
		RemoveGreaterConflictingPawnTarget(targets[0]);
		targets[0].List.Sort(ComparePawnByDistance);
		targets[1].List.Sort(CompareNodeByVantageDistanceReversed);
	}

	private void SelectTargetPoem1(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveSmallerConflictingPawnTarget(target);
			target.List.Sort(ComparePawnByDistanceReversed);
		}
	}

	private void SelectTargetPoem2(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveCheckMatePawnTarget(target);
			RemoveSmallerConflictingPawnTarget(target);
		}
	}

	private void SelectTargetPoem3(List<StrategyTarget> targets)
	{
	}

	private void SelectTargetPainting1(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveInvertBasesPawnTarget(target);
		}
	}

	private void SelectTargetPainting2(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveInvertBasesPawnTarget(target);
		}
	}

	private void SelectTargetPainting3(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveInvertBasesPawnTarget(target);
		}
	}

	private void SelectTargetMath1(List<StrategyTarget> targets)
	{
	}

	private void SelectTargetMath2(List<StrategyTarget> targets)
	{
		RemoveInvertBasesPawnTarget(targets[0]);
		RemoveFactorPawnTarget(targets[0]);
		targets[0].List.Sort(ComparePawnByBasesFactorAndPlayer);
	}

	private void SelectTargetMath3(List<StrategyTarget> targets)
	{
		RemoveSmallerConflictingPawnTarget(targets[0]);
		RemoveSpawnPawnTarget(targets[0]);
		targets[1].List.Sort(CompareNodeByDistance);
	}

	private void SelectTargetAppraisal1(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveCheckMatePawnTarget(target);
			target.List.Sort(ComparePawnByDistance);
		}
	}

	private void SelectTargetAppraisal2(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveCheckMatePawnTarget(target);
			target.List.Sort(ComparePawnByDistance);
		}
	}

	private void SelectTargetAppraisal3(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveSmallerConflictingPawnTarget(target);
			target.List.Sort(ComparePawnByDistanceReversed);
		}
	}

	private void SelectTargetForging1(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveCheckMatePawnTarget(target);
			RemoveSmallerConflictingPawnTarget(target);
			RemoveSmallerPawnBasesPercentTarget(target);
		}
	}

	private void SelectTargetForging2(List<StrategyTarget> targets)
	{
	}

	private void SelectTargetForging3(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveInvertBasesPawnTarget(target);
			RemoveSmallerConflictingPawnTarget(target);
			target.List.Sort(ComparePawnByDistance);
		}
	}

	private void SelectTargetWoodworking1(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveSmallerConflictingPawnTarget(target);
			target.List.Sort(ComparePawnByDistance);
		}
	}

	private void SelectTargetWoodworking2(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveSmallerConflictingPawnTarget(target);
			target.List.Sort(ComparePawnByDistance);
		}
	}

	private void SelectTargetWoodworking3(List<StrategyTarget> targets)
	{
	}

	private void SelectTargetMedicine1(List<StrategyTarget> targets)
	{
	}

	private void SelectTargetMedicine2(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveSelfPawnTarget(target);
			target.List.Sort(ComparePawnByBasesReversed);
		}
	}

	private void SelectTargetMedicine3(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveSmallerConflictingPawnTarget(target);
			target.List.Sort(ComparePawnByDistanceReversed);
		}
	}

	private void SelectTargetToxicology1(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			target.List.Sort(ComparePawnByDistance);
		}
	}

	private void SelectTargetToxicology2(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveSelfPawnTarget(target);
			target.List.Sort(ComparePawnByBasesReversed);
		}
	}

	private void SelectTargetToxicology3(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveSmallerConflictingPawnTarget(target);
			target.List.Sort(ComparePawnByDistanceReversed);
		}
	}

	private void SelectTargetWeaving1(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveInvertBasesPawnTarget(target);
		}
	}

	private void SelectTargetWeaving2(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveSmallerConflictingPawnTarget(target);
			target.List.Sort(ComparePawnByBasesAndConflict);
		}
	}

	private void SelectTargetWeaving3(List<StrategyTarget> targets)
	{
		targets[0].List.Sort(ComparePawnByBases);
		List<ulong> list = targets[0].List;
		List<ulong> list2 = targets[0].List;
		int index = list2.Count - 1;
		List<ulong> list3 = targets[0].List;
		ulong value = list3[list3.Count - 1];
		ulong value2 = targets[0].List[0];
		list[0] = value;
		list2[index] = value2;
	}

	private void SelectTargetJade1(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveSelfPawnTarget(target);
			target.List.Sort(ComparePawnByDistanceReversedAndConflict);
		}
	}

	private void SelectTargetJade2(List<StrategyTarget> targets)
	{
	}

	private void SelectTargetJade3(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveSmallerConflictingPawnTarget(target);
			target.List.Sort(ComparePawnByDistanceReversed);
		}
	}

	private void SelectTargetTaoism1(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveSelfPawnTarget(target);
			RemoveSmallerConflictingPawnTarget(target);
			target.List.Sort(ComparePawnByDistanceReversed);
		}
	}

	private void SelectTargetTaoism2(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveFreeCard(target);
		}
	}

	private void SelectTargetTaoism3(List<StrategyTarget> targets)
	{
	}

	private void SelectTargetBuddhism1(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveNonOpponentStrategyPawnTarget(target);
		}
	}

	private void SelectTargetBuddhism2(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemovePlayerConflictingPawnTarget(target);
		}
	}

	private void SelectTargetBuddhism3(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveSmallerConflictingPawnTarget(target);
		}
	}

	private void SelectTargetCooking1(List<StrategyTarget> targets)
	{
	}

	private void SelectTargetCooking2(List<StrategyTarget> targets)
	{
		RemoveOpponentPawnTarget(targets[0]);
		RemoveSelfPawnTarget(targets[1]);
		targets[1].List.Sort(ComparePawnByBasesReversed);
	}

	private void SelectTargetCooking3(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveOpponentNonCheckMatePawnTarget(target);
		}
	}

	private void SelectTargetEclectic1(List<StrategyTarget> targets)
	{
		foreach (StrategyTarget target in targets)
		{
			RemoveLessOpponentStrategyPawnTarget(target);
		}
	}

	private void SelectTargetEclectic2(List<StrategyTarget> targets)
	{
		RemoveSelfPawnTarget(targets[0]);
		RemoveOpponentPawnTarget(targets[1]);
		targets[0].List.Sort(ComparePawnByOpponentStrategyCountReversed);
		targets[1].List.Sort(ComparePawnByOpponentStrategyCount);
	}

	private void SelectTargetEclectic3(List<StrategyTarget> targets)
	{
	}
}
