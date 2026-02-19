using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Domains.Taiwu.LifeSkillCombat.Operation;
using GameData.Domains.Taiwu.LifeSkillCombat.Snapshot;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Status;

public class Grid : ISerializableGameData
{
	private readonly List<OperationGridBase> _operations = new List<OperationGridBase>();

	public static readonly Coordinate2D<sbyte>[] OffsetStraight = new Coordinate2D<sbyte>[4]
	{
		new Coordinate2D<sbyte>(1, 0),
		new Coordinate2D<sbyte>(-1, 0),
		new Coordinate2D<sbyte>(0, 1),
		new Coordinate2D<sbyte>(0, -1)
	};

	public static readonly Coordinate2D<sbyte>[] OffsetCross = new Coordinate2D<sbyte>[4]
	{
		new Coordinate2D<sbyte>(1, 1),
		new Coordinate2D<sbyte>(-1, -1),
		new Coordinate2D<sbyte>(1, -1),
		new Coordinate2D<sbyte>(-1, 1)
	};

	public int Index { get; private set; }

	public OperationGridBase LastHistoryOperation
	{
		get
		{
			int num = _operations.Count - 1;
			if (num >= 0)
			{
				OperationGridBase operationGridBase = _operations[num];
				if (!operationGridBase.IsActive)
				{
					return operationGridBase;
				}
				if (num != _operations.Count - 1)
				{
					throw new Exception("Active operation must be last one.");
				}
			}
			return null;
		}
	}

	public OperationGridBase ActiveOperation
	{
		get
		{
			if (_operations.Count < 1)
			{
				return null;
			}
			OperationGridBase operationGridBase = _operations[_operations.Count - 1];
			return operationGridBase.IsActive ? operationGridBase : null;
		}
	}

	public Coordinate2D<sbyte> Coordinate => ToCenterAnchoredCoordinate(Index);

	public Grid()
	{
	}

	public IEnumerable<OperationGridBase> HistoryOperations()
	{
		int i = 0;
		for (int len = _operations.Count; i < len; i++)
		{
			OperationGridBase operation = _operations[i];
			if (operation.IsActive)
			{
				if (i != _operations.Count - 1)
				{
					throw new Exception("Active operation must be last one.");
				}
				break;
			}
			yield return operation;
		}
	}

	public void GetAllOperations(List<OperationGridBase> receiver, bool withClear)
	{
		if (withClear)
		{
			receiver.Clear();
		}
		receiver.AddRange(_operations);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += 4;
		num += 2;
		int i = 0;
		for (int count = _operations.Count; i < count; i++)
		{
			num += OperationBase.GetSerializeSizeWithPolymorphism(_operations[i]);
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = Index;
		ptr += 4;
		Tester.Assert(_operations.Count < 65535);
		*(ushort*)ptr = (ushort)_operations.Count;
		ptr += 2;
		int i = 0;
		for (int count = _operations.Count; i < count; i++)
		{
			ptr += OperationBase.SerializeWithPolymorphism(_operations[i], ptr);
		}
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Index = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		_operations.Clear();
		for (int i = 0; i < num; i++)
		{
			OperationBase operation = null;
			ptr += OperationBase.DeserializeWithPolymorphism(ref operation, ptr);
			if (operation is OperationGridBase item)
			{
				_operations.Add(item);
				continue;
			}
			throw new Exception("operation storage in Grid must be OperationGridBase");
		}
		return (int)(ptr - pData);
	}

	public Grid(int index)
	{
		Index = index;
	}

	public IReadOnlyList<OperationGridBase> GetAllOperations()
	{
		return _operations;
	}

	public static int ToGridIndex(Coordinate2D<sbyte> coordinate)
	{
		return (coordinate.Y + 3) * 7 + (coordinate.X + 3);
	}

	public static Coordinate2D<byte> ToCornerAnchoredCoordinate(int index)
	{
		return new Coordinate2D<byte>((byte)(index % 7), (byte)(index / 7));
	}

	public static Coordinate2D<sbyte> ToCenterAnchoredCoordinate(int index)
	{
		Coordinate2D<byte> coordinate2D = ToCornerAnchoredCoordinate(index);
		return new Coordinate2D<sbyte>((sbyte)(coordinate2D.X - 3), (sbyte)(coordinate2D.Y - 3));
	}

	public static IEnumerable<Coordinate2D<sbyte>> OffsetIterate(Coordinate2D<sbyte> origin, sbyte straight, sbyte cross, Func<Coordinate2D<sbyte>, bool> breakCondition = null)
	{
		Coordinate2D<sbyte>[] offsetStraight = OffsetStraight;
		for (int i = 0; i < offsetStraight.Length; i++)
		{
			Coordinate2D<sbyte> offset = offsetStraight[i];
			for (int j = 0; j < straight; j++)
			{
				Coordinate2D<sbyte> coord = new Coordinate2D<sbyte>((sbyte)Math.Clamp(origin.X + offset.X * (j + 1), -128, 127), (sbyte)Math.Clamp(origin.Y + offset.Y * (j + 1), -128, 127));
				if (coord.X >= -3 && coord.X <= 3 && coord.Y >= -3 && coord.Y <= 3)
				{
					if (breakCondition?.Invoke(coord) ?? false)
					{
						break;
					}
					yield return coord;
				}
			}
		}
		Coordinate2D<sbyte>[] offsetCross = OffsetCross;
		for (int k = 0; k < offsetCross.Length; k++)
		{
			Coordinate2D<sbyte> offset2 = offsetCross[k];
			for (int l = 0; l < cross; l++)
			{
				Coordinate2D<sbyte> coord2 = new Coordinate2D<sbyte>((sbyte)Math.Clamp(origin.X + offset2.X * (l + 1), -128, 127), (sbyte)Math.Clamp(origin.Y + offset2.Y * (l + 1), -128, 127));
				if (coord2.X >= -3 && coord2.X <= 3 && coord2.Y >= -3 && coord2.Y <= 3)
				{
					if (breakCondition?.Invoke(coord2) ?? false)
					{
						break;
					}
					yield return coord2;
				}
			}
		}
	}

	public IEnumerable<OperationGridBase> TakeHistoryOperations()
	{
		for (int i = _operations.Count - 1; i >= 0; i--)
		{
			OperationGridBase operation = _operations[i];
			if (operation.IsActive)
			{
				break;
			}
			_operations.RemoveAt(i);
			yield return operation;
		}
	}

	public OperationGridBase TakeActiveOperation()
	{
		if (_operations.Count < 1)
		{
			return null;
		}
		int index = _operations.Count - 1;
		OperationGridBase operationGridBase = _operations[index];
		if (operationGridBase.IsActive)
		{
			_operations.RemoveAt(index);
			return operationGridBase;
		}
		return null;
	}

	public void DropHistoryOperations()
	{
		foreach (OperationGridBase item in TakeHistoryOperations())
		{
		}
	}

	public void SetActiveOperation(OperationGridBase operation, Match lifeSkillCombat, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs)
	{
		if (operation != null)
		{
			Tester.Assert(operation.GridIndex == Index);
		}
		ActiveOperation?.MakeNonActive();
		TakeActiveOperation();
		if (operation != null)
		{
			_operations.Add(operation);
			operation.MakeActive();
		}
		if (operation == null && _operations.Count > 0)
		{
			List<OperationGridBase> operations = _operations;
			if (operations[operations.Count - 1] is OperationPointBase pOp)
			{
				lifeSkillCombat.OnGridActiveFixed(pOp, gridTrapStateExtraDiffs);
			}
		}
	}

	public OperationPointBase GetThesis()
	{
		for (int num = _operations.Count - 1; num >= 0; num--)
		{
			OperationGridBase operationGridBase = _operations[num];
			if (operationGridBase.IsActive)
			{
				return null;
			}
			if (operationGridBase is OperationPointBase result)
			{
				return result;
			}
		}
		return null;
	}

	public int GetThesisScore()
	{
		Coordinate2D<sbyte> coordinate = Coordinate;
		if (Math.Abs(coordinate.X) == 3 || Math.Abs(coordinate.Y) == 3)
		{
			return 0;
		}
		if (Math.Abs(coordinate.X) == 2 || Math.Abs(coordinate.Y) == 2)
		{
			return 1;
		}
		if (Math.Abs(coordinate.X) == 1 || Math.Abs(coordinate.Y) == 1)
		{
			return 2;
		}
		return 3;
	}

	public List<sbyte> ProcessOnGridActiveFixedRecycle()
	{
		Tester.Assert(ActiveOperation == null);
		List<sbyte> list = new List<sbyte>();
		if (_operations.Count >= 2 && _operations[_operations.Count - 2] is OperationPointBase operationPointBase)
		{
			list.AddRange(operationPointBase.EffectiveEffectCardTemplateIds.Where((sbyte c) => LifeSkillCombatEffect.Instance[c].IsSaveCard));
		}
		list.RemoveAll((sbyte c) => LifeSkillCombatEffect.Instance[c].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndRecycleCardAndExchangeOperation);
		return list;
	}
}
