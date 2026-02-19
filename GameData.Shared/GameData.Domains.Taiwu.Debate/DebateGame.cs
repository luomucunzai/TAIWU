using System;
using System.Collections.Generic;
using Config;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Debate;

[SerializableGameData(NoCopyConstructors = true)]
public class DebateGame : ISerializableGameData
{
	public struct EffectItem
	{
		public int Value;

		public short EffectTemplateId;

		public short StrategyTemplateId;

		public EffectItem(int value, short effectTemplateId, short strategyTemplateId)
		{
			Value = value;
			EffectTemplateId = effectTemplateId;
			StrategyTemplateId = strategyTemplateId;
		}
	}

	[SerializableGameDataField]
	public sbyte LifeSkillType;

	[SerializableGameDataField]
	public bool IsTaiwuFirst;

	[SerializableGameDataField]
	public sbyte State;

	[SerializableGameDataField]
	public int Round;

	[SerializableGameDataField]
	public DebatePlayer PlayerLeft;

	[SerializableGameDataField]
	public DebatePlayer PlayerRight;

	[SerializableGameDataField]
	public List<DebateComment> Comments;

	[SerializableGameDataField]
	public List<int> Spectators;

	[SerializableGameDataField]
	public Dictionary<IntPair, DebateNode> DebateGrid;

	[SerializableGameDataField]
	public Dictionary<int, Pawn> Pawns;

	[SerializableGameDataField]
	public Dictionary<int, ActivatedStrategy> ActivatedStrategies;

	[SerializableGameDataField]
	public Dictionary<int, DebateNodeEffectState> NodeEffects;

	[SerializableGameDataField]
	public List<DebateOperation> DebateOperations;

	[SerializableGameDataField]
	public bool IsGameOver;

	[SerializableGameDataField]
	public bool IsTaiwuWin;

	[SerializableGameDataField]
	public bool IsTaiwuAi;

	[SerializableGameDataField]
	public bool IsTaiwuAiProcessedInRound;

	public DebateGame(sbyte type, bool isTaiwuFirst, DebatePlayer playerLeft, DebatePlayer playerRight, List<int> spectators, Dictionary<IntPair, DebateNode> debateGrid, bool isTaiwuAi)
	{
		LifeSkillType = type;
		IsTaiwuFirst = isTaiwuFirst;
		State = -1;
		Round = 0;
		PlayerLeft = playerLeft;
		PlayerRight = playerRight;
		Comments = new List<DebateComment>();
		Spectators = spectators;
		DebateGrid = debateGrid;
		Pawns = new Dictionary<int, Pawn>();
		NodeEffects = new Dictionary<int, DebateNodeEffectState>();
		ActivatedStrategies = new Dictionary<int, ActivatedStrategy>();
		DebateOperations = new List<DebateOperation>();
		IsGameOver = false;
		IsTaiwuWin = false;
		IsTaiwuAi = isTaiwuAi;
		IsTaiwuAiProcessedInRound = false;
	}

	public DebateGame()
	{
		PlayerLeft = new DebatePlayer();
		PlayerRight = new DebatePlayer();
		Comments = new List<DebateComment>();
		Spectators = new List<int>();
		DebateGrid = new Dictionary<IntPair, DebateNode>();
		Pawns = new Dictionary<int, Pawn>();
		NodeEffects = new Dictionary<int, DebateNodeEffectState>();
		ActivatedStrategies = new Dictionary<int, ActivatedStrategy>();
		DebateOperations = new List<DebateOperation>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 11;
		num = ((PlayerLeft == null) ? (num + 2) : (num + (2 + PlayerLeft.GetSerializedSize())));
		num = ((PlayerRight == null) ? (num + 2) : (num + (2 + PlayerRight.GetSerializedSize())));
		num = ((Comments == null) ? (num + 2) : (num + (2 + 12 * Comments.Count)));
		num = ((Spectators == null) ? (num + 2) : (num + (2 + 4 * Spectators.Count)));
		num += DictionaryOfCustomTypePair.GetSerializedSize<IntPair, DebateNode>(DebateGrid);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, Pawn>(Pawns);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, ActivatedStrategy>(ActivatedStrategies);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, DebateNodeEffectState>(NodeEffects);
		if (DebateOperations != null)
		{
			num += 2;
			int count = DebateOperations.Count;
			for (int i = 0; i < count; i++)
			{
				DebateOperation debateOperation = DebateOperations[i];
				num = ((debateOperation == null) ? (num + 2) : (num + (2 + debateOperation.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)LifeSkillType;
		ptr++;
		*ptr = (IsTaiwuFirst ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)State;
		ptr++;
		*(int*)ptr = Round;
		ptr += 4;
		if (PlayerLeft != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = PlayerLeft.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (PlayerRight != null)
		{
			byte* intPtr2 = ptr;
			ptr += 2;
			int num2 = PlayerRight.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr2 = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (Comments != null)
		{
			int count = Comments.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += Comments[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (Spectators != null)
		{
			int count2 = Spectators.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				((int*)ptr)[j] = Spectators[j];
			}
			ptr += 4 * count2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfCustomTypePair.Serialize<IntPair, DebateNode>(ptr, ref DebateGrid);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, Pawn>(ptr, ref Pawns);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, ActivatedStrategy>(ptr, ref ActivatedStrategies);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, DebateNodeEffectState>(ptr, ref NodeEffects);
		if (DebateOperations != null)
		{
			int count3 = DebateOperations.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				DebateOperation debateOperation = DebateOperations[k];
				if (debateOperation != null)
				{
					byte* intPtr3 = ptr;
					ptr += 2;
					int num3 = debateOperation.Serialize(ptr);
					ptr += num3;
					Tester.Assert(num3 <= 65535);
					*(ushort*)intPtr3 = (ushort)num3;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (IsGameOver ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (IsTaiwuWin ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (IsTaiwuAi ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (IsTaiwuAiProcessedInRound ? ((byte)1) : ((byte)0));
		ptr++;
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		LifeSkillType = (sbyte)(*ptr);
		ptr++;
		IsTaiwuFirst = *ptr != 0;
		ptr++;
		State = (sbyte)(*ptr);
		ptr++;
		Round = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (PlayerLeft == null)
			{
				PlayerLeft = new DebatePlayer();
			}
			ptr += PlayerLeft.Deserialize(ptr);
		}
		else
		{
			PlayerLeft = null;
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (PlayerRight == null)
			{
				PlayerRight = new DebatePlayer();
			}
			ptr += PlayerRight.Deserialize(ptr);
		}
		else
		{
			PlayerRight = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (Comments == null)
			{
				Comments = new List<DebateComment>(num3);
			}
			else
			{
				Comments.Clear();
			}
			for (int i = 0; i < num3; i++)
			{
				DebateComment debateComment = new DebateComment();
				ptr += debateComment.Deserialize(ptr);
				Comments.Add(debateComment);
			}
		}
		else
		{
			Comments?.Clear();
		}
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (Spectators == null)
			{
				Spectators = new List<int>(num4);
			}
			else
			{
				Spectators.Clear();
			}
			for (int j = 0; j < num4; j++)
			{
				Spectators.Add(((int*)ptr)[j]);
			}
			ptr += 4 * num4;
		}
		else
		{
			Spectators?.Clear();
		}
		ptr += DictionaryOfCustomTypePair.Deserialize<IntPair, DebateNode>(ptr, ref DebateGrid);
		ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, Pawn>(ptr, ref Pawns);
		ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, ActivatedStrategy>(ptr, ref ActivatedStrategies);
		ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, DebateNodeEffectState>(ptr, ref NodeEffects);
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (DebateOperations == null)
			{
				DebateOperations = new List<DebateOperation>(num5);
			}
			else
			{
				DebateOperations.Clear();
			}
			for (int k = 0; k < num5; k++)
			{
				ushort num6 = *(ushort*)ptr;
				ptr += 2;
				if (num6 > 0)
				{
					DebateOperation debateOperation = new DebateOperation();
					ptr += debateOperation.Deserialize(ptr);
					DebateOperations.Add(debateOperation);
				}
				else
				{
					DebateOperations.Add(null);
				}
			}
		}
		else
		{
			DebateOperations?.Clear();
		}
		IsGameOver = *ptr != 0;
		ptr++;
		IsTaiwuWin = *ptr != 0;
		ptr++;
		IsTaiwuAi = *ptr != 0;
		ptr++;
		IsTaiwuAiProcessedInRound = *ptr != 0;
		ptr++;
		int num7 = (int)(ptr - pData);
		if (num7 > 4)
		{
			return (num7 + 3) / 4 * 4;
		}
		return num7;
	}

	public sbyte GetPressureType(int pressure, int maxPressure)
	{
		int num = pressure * 100 / maxPressure;
		if (num >= DebateConstants.HighPressurePercent)
		{
			return 3;
		}
		if (num >= DebateConstants.MidPressurePercent)
		{
			return 2;
		}
		if (num >= DebateConstants.LowPressurePercent)
		{
			return 1;
		}
		return 0;
	}

	public DebatePlayer GetPlayerByPlayerIsTaiwu(bool isTaiwu)
	{
		if (!isTaiwu)
		{
			return PlayerRight;
		}
		return PlayerLeft;
	}

	public bool GetIsTaiwuTurn()
	{
		if (State != 0 || !IsTaiwuFirst)
		{
			if (State == 1)
			{
				return !IsTaiwuFirst;
			}
			return false;
		}
		return true;
	}

	public int GetValueDividedByMaxBases(int value, bool isTaiwu)
	{
		DebatePlayer playerByPlayerIsTaiwu = GetPlayerByPlayerIsTaiwu(isTaiwu);
		if (playerByPlayerIsTaiwu.MaxBases != 0)
		{
			return value / playerByPlayerIsTaiwu.MaxBases;
		}
		return 0;
	}

	public (int pressure, int gamePoint) GetGamePointAndPressureDelta(DebatePlayer player, int value)
	{
		int num = Math.Clamp(value, 0, player.MaxPressure);
		return (pressure: num, gamePoint: GetPressureType(player.HighestPressure, player.MaxPressure) - GetPressureType(num, player.MaxPressure));
	}

	public int GetPawnGradeToBase(int maxBases, int grade)
	{
		return maxBases * grade * DebateConstants.GradeToBasesPercent / 100;
	}

	public int GetPawnInitialBases(bool isOwnedByTaiwu, sbyte grade, int value)
	{
		return GetPawnGradeToBase(GetPlayerByPlayerIsTaiwu(isOwnedByTaiwu).MaxBases, grade) + value;
	}

	public int GetPawnBases(int pawnId, int otherId = -1, bool isReal = true, bool isTaiwu = true)
	{
		bool num = otherId >= 0;
		int num2 = 0;
		if (num && TryGetPawnStrategyEffectValue(pawnId, 6, isReal, isTaiwu, out var value))
		{
			num2 += GetPawnBases(otherId, isReal: true, isTaiwu) * value / 100 * ((!TryGetPawnStrategyEffectValue(pawnId, 8, isReal, isTaiwu, out var _)) ? 1 : (-1));
		}
		if (!TryGetMaxBasesOfLinkedPawns(pawnId, isReal, isTaiwu, includeSelf: true, out var value3))
		{
			return num2 + GetPawnBases(pawnId, isReal, isTaiwu);
		}
		return num2 + value3;
	}

	public int GetPawnBases(int id, bool isReal, bool isTaiwu)
	{
		return Pawns[id].Bases * (100 + GetPawnBasesFactor(id, isReal, isTaiwu)) / 100;
	}

	public int GetPawnBasesFactor(int id, bool isReal, bool isTaiwu)
	{
		int num = 0;
		if (TryGetPawnStrategyEffectValue(id, 0, isReal, isTaiwu, out var value))
		{
			num += value;
		}
		if (TryGetPawnStrategyEffectValue(id, 24, isReal, isTaiwu, out value))
		{
			num += value * GetPawnCount(Pawns[id].IsOwnedByTaiwu);
		}
		if (TryGetPawnStrategyEffectValue(id, 8, isReal, isTaiwu, out var _))
		{
			num = -num;
		}
		return num;
	}

	public List<EffectItem> GetPawnBasesFactorList(int id, bool isReal, bool isTaiwu)
	{
		List<EffectItem> list = new List<EffectItem>();
		int value;
		bool flag = TryGetPawnStrategyEffectValue(id, 8, isReal, isTaiwu, out value, list);
		if (TryGetPawnStrategyEffectValue(id, 0, isReal, isTaiwu, out value, list))
		{
			for (int i = 0; i < list.Count; i++)
			{
				EffectItem value2 = list[i];
				if (value2.EffectTemplateId == 0)
				{
					value2.Value = (flag ? (-value2.Value) : value2.Value);
					list[i] = value2;
				}
			}
		}
		if (TryGetPawnStrategyEffectValue(id, 24, isReal, isTaiwu, out var value3, list))
		{
			int num = value3 * GetPawnCount(Pawns[id].IsOwnedByTaiwu);
			if (flag)
			{
				num = -num;
			}
			for (int j = 0; j < list.Count; j++)
			{
				EffectItem value4 = list[j];
				if (value4.EffectTemplateId == 24)
				{
					value4.Value = num;
					list[j] = value4;
				}
			}
		}
		return list;
	}

	public bool TryGetMaxBasesOfLinkedPawns(int id, bool isReal, bool isTaiwu, bool includeSelf, out int value)
	{
		value = 0;
		if (!TryGetPawnStrategyEffectValue(id, 26, isReal, isTaiwu, out var value2))
		{
			return false;
		}
		bool isOwnedByTaiwu = Pawns[id].IsOwnedByTaiwu;
		foreach (KeyValuePair<int, Pawn> pawn2 in Pawns)
		{
			pawn2.Deconstruct(out value2, out var value3);
			int num = value2;
			Pawn pawn = value3;
			if (pawn.IsAlive && pawn.IsOwnedByTaiwu == isOwnedByTaiwu && (includeSelf || id != num) && TryGetPawnStrategyEffectValue(num, 26, isReal: true, isTaiwu, out value2))
			{
				value = Math.Max(value, GetPawnBases(num, isReal: true, isTaiwu));
			}
		}
		return true;
	}

	public int GetPawnCount(bool isTaiwu)
	{
		int num = 0;
		foreach (Pawn value in Pawns.Values)
		{
			if (value.IsOwnedByTaiwu == isTaiwu && value.IsAlive)
			{
				num++;
			}
		}
		return num;
	}

	public IntPair GetPawnTargetPosition(int pawnId)
	{
		return GetPawnTargetPosition(Pawns[pawnId]);
	}

	public IntPair GetPawnTargetPosition(Pawn pawn)
	{
		return new IntPair(pawn.IsOwnedByTaiwu ? (pawn.Coordinate.First + 1) : (pawn.Coordinate.First - 1), pawn.Coordinate.Second);
	}

	public IntPair GetPawnBehindPosition(int pawnId, int distance = 1)
	{
		return GetPawnBehindPosition(Pawns[pawnId], distance);
	}

	public IntPair GetPawnBehindPosition(Pawn pawn, int distance)
	{
		return new IntPair(pawn.IsOwnedByTaiwu ? (pawn.Coordinate.First - distance) : (pawn.Coordinate.First + distance), pawn.Coordinate.Second);
	}

	public bool GetPawnCanDamage(bool isOwnedByTaiwu, int x)
	{
		if (!isOwnedByTaiwu || x != DebateConstants.DebateLineNodeCount - 1)
		{
			if (!isOwnedByTaiwu)
			{
				return x == 0;
			}
			return false;
		}
		return true;
	}

	public bool GetPawnIsHalt(int pawnId)
	{
		if (Pawns[pawnId].IsHalt)
		{
			return Round % 2 == 0;
		}
		return false;
	}

	public DebateNode GetNode(int x, int y)
	{
		return DebateGrid[new IntPair(x, y)];
	}

	public bool GetCoordinateValid(IntPair coordinate)
	{
		if (coordinate.First >= 0 && coordinate.First < DebateConstants.DebateLineNodeCount && coordinate.Second >= 0)
		{
			return coordinate.Second < DebateConstants.DebateLineCount;
		}
		return false;
	}

	public bool GetCoordinateValid(int x)
	{
		if (x >= 0)
		{
			return x < DebateConstants.DebateLineNodeCount;
		}
		return false;
	}

	public bool GetNodeCanMakeMove(IntPair coordinate, bool isTaiwu)
	{
		if (!GetCoordinateValid(coordinate) || DebateGrid[coordinate].IsVantage != isTaiwu || DebateGrid[coordinate].PawnId >= 0 || GetMakeMoveBlockedByPawn(new IntPair(coordinate.First + 1, coordinate.Second), isTaiwu) || GetMakeMoveBlockedByPawn(new IntPair(coordinate.First - 1, coordinate.Second), isTaiwu) || GetMakeMoveBlockedByPawn(new IntPair(coordinate.First, coordinate.Second + 1), isTaiwu) || GetMakeMoveBlockedByPawn(new IntPair(coordinate.First, coordinate.Second - 1), isTaiwu) || GetNodeIsContainingEffect(coordinate, 23))
		{
			return false;
		}
		int startCoordinate = GetStartCoordinate(isTaiwu);
		int num = (isTaiwu ? 1 : (-1));
		for (int i = startCoordinate; i != coordinate.First; i += num)
		{
			int pawnId = DebateGrid[new IntPair(i, coordinate.Second)].PawnId;
			if (pawnId >= 0 && Pawns[pawnId].IsOwnedByTaiwu != isTaiwu)
			{
				return false;
			}
		}
		return true;
	}

	public bool GetNodeCanTeleportPawn(IntPair coordinate)
	{
		if (GetCoordinateValid(coordinate))
		{
			return DebateGrid[coordinate].PawnId < 0;
		}
		return false;
	}

	public bool GetMakeMoveBlockedByPawn(IntPair coordinate, bool isTaiwu)
	{
		int value;
		if (GetCoordinateValid(coordinate) && DebateGrid[coordinate].PawnId >= 0)
		{
			return TryGetPawnStrategyEffectId(DebateGrid[coordinate].PawnId, 23, !isTaiwu, out value);
		}
		return false;
	}

	public bool GetNodeContainsPawn(IntPair coordinate, bool isTaiwu)
	{
		if (GetCoordinateValid(coordinate) && DebateGrid[coordinate].PawnId >= 0)
		{
			return Pawns[DebateGrid[coordinate].PawnId].IsOwnedByTaiwu == isTaiwu;
		}
		return false;
	}

	public bool TryGetEmptyNode(bool isTaiwu, out List<IntPair> coordinates)
	{
		coordinates = null;
		foreach (var (item, debateNode2) in DebateGrid)
		{
			if (debateNode2.PawnId < 0 && debateNode2.IsVantage == isTaiwu)
			{
				if (coordinates == null)
				{
					coordinates = new List<IntPair>();
				}
				coordinates.Add(item);
			}
		}
		return coordinates != null;
	}

	public bool TryGetEmptyNode(bool isTaiwu, List<IntPair> coordinates)
	{
		coordinates.Clear();
		foreach (var (item, debateNode2) in DebateGrid)
		{
			if (debateNode2.PawnId < 0 && debateNode2.IsVantage == isTaiwu)
			{
				coordinates.Add(item);
			}
		}
		return coordinates.Count != 0;
	}

	public IntPair GetStartCoordinate(bool isTaiwu, int y)
	{
		return new IntPair(GetStartCoordinate(isTaiwu), y);
	}

	public int GetStartCoordinate(bool isTaiwu)
	{
		if (!isTaiwu)
		{
			return DebateConstants.DebateLineNodeCount - 1;
		}
		return 0;
	}

	public bool GetPawnNodeContainsEmptyNeighbor(int pawnId, int distance)
	{
		Pawn pawn = Pawns[pawnId];
		int num = ((distance > 0) ? 1 : (-1));
		IntPair coordinate = pawn.Coordinate;
		num *= (pawn.IsOwnedByTaiwu ? 1 : (-1));
		distance = Math.Abs(distance);
		for (int i = 1; i <= distance; i++)
		{
			if (GetNodeCanTeleportPawn(new IntPair(coordinate.First + i * num, coordinate.Second)))
			{
				return true;
			}
		}
		return false;
	}

	public bool GetNodeIsContainingEffect(IntPair coordinate, short effectId)
	{
		if (!GetCoordinateValid(coordinate))
		{
			return false;
		}
		DebateNode debateNode = DebateGrid[coordinate];
		if (debateNode.EffectState.TemplateId < 0)
		{
			return false;
		}
		foreach (IntPair specialEffect in DebateNodeEffect.Instance[debateNode.EffectState.TemplateId].SpecialEffectList)
		{
			if (specialEffect.First == effectId)
			{
				return true;
			}
		}
		return false;
	}

	public bool GetPlayerCanMakeMove(bool isTaiwu)
	{
		return GetPlayerByPlayerIsTaiwu(isTaiwu).MakeMoveCount < DebateConstants.MakeMoveLimit;
	}

	public bool TryGetStrategyTarget(short templateId, bool isTaiwu, out List<StrategyTarget> res)
	{
		res = null;
		DebateStrategyItem debateStrategyItem = DebateStrategy.Instance[templateId];
		if (debateStrategyItem.TargetList == null || debateStrategyItem.TargetList.Count == 0)
		{
			return IsStrategyTargetEnough(templateId, isTaiwu);
		}
		res = new List<StrategyTarget>();
		bool isInstant = debateStrategyItem.TriggerType == EDebateStrategyTriggerType.Instant;
		foreach (short[] target in debateStrategyItem.TargetList)
		{
			StrategyTarget strategyTarget = GetStrategyTarget(debateStrategyItem.UsedCost, target, isTaiwu, isInstant);
			CullStrategyTargets(templateId, strategyTarget);
			res.Add(strategyTarget);
			if (!IsStrategyTargetEnough(strategyTarget, target[0], target[1], isInstant))
			{
				return false;
			}
		}
		return true;
	}

	private bool IsStrategyTargetEnough(short templateId, bool isTaiwu)
	{
		List<int> collection;
		return DebateStrategy.Instance[templateId].TargetRestrict switch
		{
			15 => TryGetStrategyCardIndexCollection(ref isTaiwu, 6, out collection) != -1, 
			16 => TryGetStrategyCardIndexCollection(ref isTaiwu, 7, out collection) != -1, 
			17 => (isTaiwu ? PlayerRight.CanUseCards.Count : PlayerLeft.CanUseCards.Count) > 0, 
			_ => true, 
		};
	}

	private bool IsStrategyTargetEnough(StrategyTarget target, short type, int limit, bool isInstant)
	{
		if (target == null)
		{
			return false;
		}
		switch (type)
		{
		case 0:
		case 2:
		case 4:
		{
			if (isInstant)
			{
				return target.List.Count > 0;
			}
			int num = 0;
			foreach (ulong item in target.List)
			{
				num += DebateConstants.PawnStrategyLimit - GetPawnStrategyCount((int)item);
			}
			return num >= limit;
		}
		default:
			return target.List.Count >= limit;
		}
	}

	private StrategyTarget GetStrategyTarget(int cost, short[] config, bool isTaiwu, bool isInstant)
	{
		EDebateStrategyTargetObjectType strategyTargetType = GetStrategyTargetType(config[0]);
		List<ulong> list = config[0] switch
		{
			0 => GetSelfPawn(isTaiwu, !isInstant, cost), 
			1 => GetSelfPawn(isTaiwu, !isInstant, cost), 
			2 => GetOpponentPawn(isTaiwu, !isInstant, cost), 
			3 => GetOpponentPawn(isTaiwu, !isInstant, cost), 
			4 => GetBothPawn(isTaiwu, !isInstant, cost), 
			5 => GetBothPawn(isTaiwu, !isInstant, cost), 
			6 => GetAnyNode(!isTaiwu), 
			7 => GetAnyNode(isTaiwu), 
			8 => GetSelfNode(isTaiwu), 
			9 => GetPawnGrade(), 
			10 => GetStrategyCard(isTaiwu), 
			_ => null, 
		};
		if (list != null)
		{
			return new StrategyTarget(strategyTargetType, list);
		}
		return null;
	}

	private List<ulong> GetSelfPawn(bool isTaiwu, bool checkStrategySlot, int cost)
	{
		if (!TryGetStrategyPawnTarget(isBoth: false, isTaiwu, checkStrategySlot, cost, out var res))
		{
			return null;
		}
		return res;
	}

	private List<ulong> GetOpponentPawn(bool isTaiwu, bool checkStrategySlot, int cost)
	{
		if (!TryGetStrategyPawnTarget(isBoth: false, !isTaiwu, checkStrategySlot, cost, out var res))
		{
			return null;
		}
		return res;
	}

	private List<ulong> GetBothPawn(bool isTaiwu, bool checkStrategySlot, int cost)
	{
		if (!TryGetStrategyPawnTarget(isBoth: true, isTaiwu, checkStrategySlot, cost, out var res))
		{
			return null;
		}
		return res;
	}

	private List<ulong> GetAnyNode(bool isTaiwu)
	{
		List<ulong> list = null;
		int startCoordinate = GetStartCoordinate(isTaiwu);
		foreach (var (intPair2, debateNode2) in DebateGrid)
		{
			if (debateNode2.PawnId < 0 && intPair2.First != startCoordinate)
			{
				if (list == null)
				{
					list = new List<ulong>();
				}
				list.Add((ulong)intPair2);
			}
		}
		return list;
	}

	private List<ulong> GetSelfNode(bool isTaiwu)
	{
		List<ulong> list = null;
		foreach (var (intPair2, debateNode2) in DebateGrid)
		{
			if (debateNode2.PawnId < 0 && debateNode2.IsVantage == isTaiwu && !GetNodeIsContainingEffect(intPair2, 23) && (GetNodeContainsPawn(new IntPair(intPair2.First + 1, intPair2.Second), isTaiwu) || GetNodeContainsPawn(new IntPair(intPair2.First - 1, intPair2.Second), isTaiwu) || GetNodeContainsPawn(new IntPair(intPair2.First, intPair2.Second + 1), isTaiwu) || GetNodeContainsPawn(new IntPair(intPair2.First, intPair2.Second - 1), isTaiwu)) && !GetMakeMoveBlockedByPawn(new IntPair(intPair2.First + 1, intPair2.Second), isTaiwu) && !GetMakeMoveBlockedByPawn(new IntPair(intPair2.First - 1, intPair2.Second), isTaiwu) && !GetMakeMoveBlockedByPawn(new IntPair(intPair2.First, intPair2.Second + 1), isTaiwu) && !GetMakeMoveBlockedByPawn(new IntPair(intPair2.First, intPair2.Second - 1), isTaiwu))
			{
				if (list == null)
				{
					list = new List<ulong>();
				}
				list.Add((ulong)intPair2);
			}
		}
		return list;
	}

	private List<ulong> GetPawnGrade()
	{
		return new List<ulong> { 0uL, 1uL, 2uL, 3uL, 4uL, 5uL, 6uL, 7uL, 8uL };
	}

	private List<ulong> GetStrategyCard(bool isTaiwu)
	{
		DebatePlayer playerByPlayerIsTaiwu = GetPlayerByPlayerIsTaiwu(isTaiwu);
		if (playerByPlayerIsTaiwu.CanUseCards.Count == 0)
		{
			return null;
		}
		List<ulong> list = new List<ulong>();
		for (int i = 0; i < playerByPlayerIsTaiwu.CanUseCards.Count; i++)
		{
			list.Add((ulong)i);
		}
		return list;
	}

	private bool TryGetStrategyPawnTarget(bool isBoth, bool isTaiwu, bool emptySlot, int cost, out List<ulong> res)
	{
		res = null;
		int num = GetPlayerByPlayerIsTaiwu(isTaiwu).StrategyPoint - cost;
		foreach (KeyValuePair<int, Pawn> pawn2 in Pawns)
		{
			pawn2.Deconstruct(out var key, out var value);
			int num2 = key;
			Pawn pawn = value;
			if (pawn.IsAlive && (isBoth || isTaiwu == pawn.IsOwnedByTaiwu) && (!emptySlot || TryGetPawnEmptyStrategySlotIndex(num2, out key)) && (!TryGetPawnStrategyEffectValue(num2, 28, out var value2) || value2 <= num))
			{
				if (res == null)
				{
					res = new List<ulong>();
				}
				res.Add((ulong)num2);
			}
		}
		return res != null;
	}

	public void AddStrategyRepeatedTargets(short type, List<ulong> targets)
	{
		switch (type)
		{
		case 0:
		case 2:
		case 4:
		{
			for (int num = targets.Count - 1; num >= 0; num--)
			{
				ulong num2 = targets[num];
				int num3 = DebateConstants.PawnStrategyLimit - GetPawnStrategyCount((int)num2);
				for (int i = 0; i < num3 - 1; i++)
				{
					targets.Add(num2);
				}
			}
			break;
		}
		case 1:
		case 3:
			break;
		}
	}

	public void CullStrategyTargets(List<ulong> selectedTargets, List<ulong> canSelectTargets)
	{
		foreach (ulong selectedTarget in selectedTargets)
		{
			canSelectTargets.Remove(selectedTarget);
		}
	}

	private void CullStrategyTargets(short templateId, StrategyTarget target)
	{
		if (target == null)
		{
			return;
		}
		DebateStrategyItem debateStrategyItem = DebateStrategy.Instance[templateId];
		switch (debateStrategyItem.TargetRestrict)
		{
		case 11:
		{
			if (debateStrategyItem.TargetRestrictValue == 0)
			{
				List<IntPair> coordinates;
				bool flag3 = TryGetEmptyNode(isTaiwu: true, out coordinates);
				bool flag4 = TryGetEmptyNode(isTaiwu: false, out coordinates);
				for (int num6 = target.List.Count - 1; num6 >= 0; num6--)
				{
					int key4 = (int)target.List[num6];
					Pawn pawn3 = Pawns[key4];
					if ((pawn3.IsOwnedByTaiwu && !flag3) || (!pawn3.IsOwnedByTaiwu && !flag4))
					{
						target.List.RemoveAt(num6);
					}
				}
				break;
			}
			for (int num7 = target.List.Count - 1; num7 >= 0; num7--)
			{
				int pawnId = (int)target.List[num7];
				if (!GetPawnNodeContainsEmptyNeighbor(pawnId, debateStrategyItem.TargetRestrictValue))
				{
					target.List.RemoveAt(num7);
				}
			}
			break;
		}
		case 12:
		{
			for (int num3 = target.List.Count - 1; num3 >= 0; num3--)
			{
				int key2 = (int)target.List[num3];
				if (Pawns[key2].IsRevealed)
				{
					target.List.RemoveAt(num3);
				}
			}
			break;
		}
		case 13:
		{
			for (int num4 = target.List.Count - 1; num4 >= 0; num4--)
			{
				int key3 = (int)target.List[num4];
				Pawn pawn2 = Pawns[key3];
				bool flag2 = false;
				int[] strategies = pawn2.Strategies;
				foreach (int num5 in strategies)
				{
					if (num5 >= 0 && !ActivatedStrategies[num5].IsRevealed)
					{
						flag2 = true;
					}
				}
				if (!flag2)
				{
					target.List.RemoveAt(num4);
				}
			}
			break;
		}
		case 14:
		{
			for (int num = target.List.Count - 1; num >= 0; num--)
			{
				int key = (int)target.List[num];
				Pawn pawn = Pawns[key];
				bool flag = false;
				int[] strategies = pawn.Strategies;
				foreach (int num2 in strategies)
				{
					if (num2 >= 0 && !ActivatedStrategies[num2].GetIsInertia())
					{
						flag = true;
					}
				}
				if (!flag)
				{
					target.List.RemoveAt(num);
				}
			}
			break;
		}
		}
	}

	private EDebateStrategyTargetObjectType GetStrategyTargetType(short templateId)
	{
		return DebateStrategyTarget.Instance[templateId].ObjectType;
	}

	public int GetPawnStrategyCount(int pawnId)
	{
		int num = 0;
		int[] strategies = Pawns[pawnId].Strategies;
		for (int i = 0; i < strategies.Length; i++)
		{
			if (strategies[i] >= 0)
			{
				num++;
			}
		}
		return num;
	}

	public bool TryGetPawnEmptyStrategySlotIndex(int pawnId, out int index)
	{
		index = -1;
		for (int i = 0; i < Pawns[pawnId].Strategies.Length; i++)
		{
			if (Pawns[pawnId].Strategies[i] < 0)
			{
				index = i;
				return true;
			}
		}
		return false;
	}

	public bool TryGetPawnStrategyEffectId(int pawnId, short effectTemplateId, bool isCastedByTaiwu, out int value)
	{
		value = -1;
		int[] strategies = Pawns[pawnId].Strategies;
		foreach (int num in strategies)
		{
			if (num < 0 || !ActivatedStrategies.TryGetValue(num, out var value2) || value2.IsCastedByTaiwu != isCastedByTaiwu)
			{
				continue;
			}
			DebateStrategyItem config = value2.GetConfig();
			if (config.EffectList == null || config.EffectList.Count == 0)
			{
				continue;
			}
			foreach (IntPair effect in config.EffectList)
			{
				if (effect.First == effectTemplateId)
				{
					value = num;
					return true;
				}
			}
		}
		return false;
	}

	public bool TryGetPawnStrategyEffectValue(int pawnId, short effectTemplateId, out int value, List<EffectItem> effectItemList = null)
	{
		value = 0;
		int[] strategies = Pawns[pawnId].Strategies;
		foreach (int num in strategies)
		{
			if (num < 0 || !ActivatedStrategies.TryGetValue(num, out var value2))
			{
				continue;
			}
			DebateStrategyItem config = value2.GetConfig();
			if (config.EffectList == null || config.EffectList.Count == 0)
			{
				continue;
			}
			foreach (IntPair effect in config.EffectList)
			{
				if (effect.First == effectTemplateId)
				{
					value += effect.Second;
					if (effect.Second != 0)
					{
						effectItemList?.Add(new EffectItem(effect.Second, effectTemplateId, value2.TemplateId));
					}
				}
			}
		}
		return value != 0;
	}

	public bool TryGetPawnRevealedStrategyEffectValue(int pawnId, short effectTemplateId, bool isTaiwu, out int value, List<EffectItem> effectItemList = null)
	{
		value = 0;
		int[] strategies = Pawns[pawnId].Strategies;
		foreach (int num in strategies)
		{
			if (num < 0 || !ActivatedStrategies.TryGetValue(num, out var value2) || (value2.IsCastedByTaiwu != isTaiwu && !value2.IsRevealed))
			{
				continue;
			}
			DebateStrategyItem config = value2.GetConfig();
			if (config.EffectList == null || config.EffectList.Count == 0)
			{
				continue;
			}
			foreach (IntPair effect in config.EffectList)
			{
				if (effect.First == effectTemplateId)
				{
					value += effect.Second;
					if (effect.Second != 0)
					{
						effectItemList?.Add(new EffectItem(effect.Second, effectTemplateId, value2.TemplateId));
					}
				}
			}
		}
		return value != 0;
	}

	public bool TryGetPawnStrategyEffectValue(int pawnId, short templateId, bool isReal, bool isTaiwu, out int value, List<EffectItem> effectItemList = null)
	{
		if (!isReal)
		{
			return TryGetPawnRevealedStrategyEffectValue(pawnId, templateId, isTaiwu, out value, effectItemList);
		}
		return TryGetPawnStrategyEffectValue(pawnId, templateId, out value, effectItemList);
	}

	public int GetStrategyCardLocation(bool isTaiwu, int location)
	{
		return location switch
		{
			6 => (!isTaiwu) ? 3 : 0, 
			7 => isTaiwu ? 1 : 4, 
			8 => isTaiwu ? 2 : 8, 
			_ => location, 
		};
	}

	public List<short> GetStrategyCardCollection(bool isTaiwu, int location)
	{
		return location switch
		{
			6 => isTaiwu ? PlayerLeft.OwnedCards : PlayerRight.OwnedCards, 
			7 => isTaiwu ? PlayerLeft.UsedCards : PlayerRight.UsedCards, 
			8 => isTaiwu ? PlayerLeft.CanUseCards : PlayerRight.CanUseCards, 
			0 => PlayerLeft.OwnedCards, 
			1 => PlayerLeft.UsedCards, 
			2 => PlayerLeft.CanUseCards, 
			3 => PlayerRight.OwnedCards, 
			4 => PlayerRight.UsedCards, 
			5 => PlayerRight.CanUseCards, 
			_ => null, 
		};
	}

	public bool GetStrategyTemplateContainsEffect(short templateId, short effectId)
	{
		DebateStrategyItem debateStrategyItem = DebateStrategy.Instance[templateId];
		if (debateStrategyItem.EffectList == null || debateStrategyItem.EffectList.Count == 0)
		{
			return false;
		}
		foreach (IntPair effect in debateStrategyItem.EffectList)
		{
			if (effect.First == effectId)
			{
				return true;
			}
		}
		return false;
	}

	public int TryGetStrategyCardIndexCollection(ref bool isTaiwu, int location, out List<int> collection)
	{
		List<short> strategyCardCollection = GetStrategyCardCollection(isTaiwu: true, location);
		List<short> strategyCardCollection2 = GetStrategyCardCollection(isTaiwu: false, location);
		int num = strategyCardCollection.Count;
		int num2 = strategyCardCollection2.Count;
		if (location == 7)
		{
			foreach (short item in strategyCardCollection)
			{
				if (GetStrategyTemplateContainsEffect(item, 31))
				{
					num--;
				}
			}
			foreach (short item2 in strategyCardCollection2)
			{
				if (GetStrategyTemplateContainsEffect(item2, 31))
				{
					num2--;
				}
			}
		}
		if (num == 0 && num2 == 0)
		{
			collection = null;
			return -1;
		}
		if (num != 0 && num2 == 0)
		{
			isTaiwu = true;
		}
		else if (num == 0 && num2 != 0)
		{
			isTaiwu = false;
		}
		List<short> list = (isTaiwu ? strategyCardCollection : strategyCardCollection2);
		collection = new List<int>();
		for (int i = 0; i < list.Count; i++)
		{
			if (location != 7 || !GetStrategyTemplateContainsEffect(list[i], 31))
			{
				collection.Add(i);
			}
		}
		return GetStrategyCardLocation(isTaiwu, location);
	}

	public bool TryGetPlayerCardRemovingCount(bool isTaiwu, out int count)
	{
		count = GetPlayerByPlayerIsTaiwu(isTaiwu).CanUseCards.Count - GlobalConfig.Instance.DebateMaxCanUseCards;
		return count > 0;
	}

	public bool GetPlayerCanUseResetStrategy(bool isTaiwu)
	{
		DebatePlayer playerByPlayerIsTaiwu = GetPlayerByPlayerIsTaiwu(isTaiwu);
		if (playerByPlayerIsTaiwu.Pressure < playerByPlayerIsTaiwu.MaxPressure && playerByPlayerIsTaiwu.OwnedCards.Count == 0)
		{
			return playerByPlayerIsTaiwu.UsedCards.Count != 0;
		}
		return false;
	}
}
