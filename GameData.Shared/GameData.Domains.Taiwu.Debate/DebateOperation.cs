using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Debate;

public class DebateOperation : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte OperationType;

	[SerializableGameDataField]
	public int PawnId;

	[SerializableGameDataField]
	public int NpcPawnId;

	[SerializableGameDataField]
	public int TaiwuPressure;

	[SerializableGameDataField]
	public int TaiwuGamePoint;

	[SerializableGameDataField]
	public int TaiwuBases;

	[SerializableGameDataField]
	public int NpcPressure;

	[SerializableGameDataField]
	public int NpcGamePoint;

	[SerializableGameDataField]
	public int NpcBases;

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public int Result;

	[SerializableGameDataField]
	public bool IsFailed;

	[SerializableGameDataField]
	public IntPair Target;

	[SerializableGameDataField]
	public int Index;

	[SerializableGameDataField]
	public int Source;

	[SerializableGameDataField]
	public int Destination;

	[SerializableGameDataField]
	public bool IsTaiwu;

	[SerializableGameDataField]
	public List<StrategyTarget> StrategyTargets;

	[SerializableGameDataField]
	public DebateNodeEffectState NodeEffectState;

	[SerializableGameDataField]
	public int[] RecordParams;

	public DebateOperation(sbyte type, int id, IntPair target, DebatePlayer taiwu, DebatePlayer npc, bool isImmuneRemove)
	{
		OperationType = type;
		PawnId = id;
		NpcPawnId = -1;
		TemplateId = -1;
		Target = target;
		TaiwuPressure = taiwu.Pressure;
		TaiwuBases = taiwu.Bases;
		TaiwuGamePoint = taiwu.GamePoint;
		NpcPressure = npc.Pressure;
		NpcBases = npc.Bases;
		NpcGamePoint = npc.GamePoint;
		Result = (isImmuneRemove ? 1 : 0);
	}

	public DebateOperation(sbyte type, bool isTaiwu, int id, IntPair target, DebatePlayer taiwu, DebatePlayer npc)
	{
		OperationType = type;
		IsTaiwu = isTaiwu;
		PawnId = id;
		NpcPawnId = -1;
		TemplateId = -1;
		Target = target;
		TaiwuPressure = taiwu.Pressure;
		TaiwuBases = taiwu.Bases;
		TaiwuGamePoint = taiwu.GamePoint;
		NpcPressure = npc.Pressure;
		NpcBases = npc.Bases;
		NpcGamePoint = npc.GamePoint;
	}

	public DebateOperation(sbyte type, bool isTaiwu, int id, IntPair target, DebatePlayer taiwu, DebatePlayer npc, int bases, bool isFailed)
	{
		OperationType = type;
		IsTaiwu = isTaiwu;
		PawnId = id;
		NpcPawnId = -1;
		TemplateId = -1;
		Target = target;
		TaiwuPressure = taiwu.Pressure;
		TaiwuBases = taiwu.Bases;
		TaiwuGamePoint = taiwu.GamePoint;
		NpcPressure = npc.Pressure;
		NpcBases = npc.Bases;
		NpcGamePoint = npc.GamePoint;
		Result = bases;
		IsFailed = isFailed;
	}

	public DebateOperation(sbyte type, int id, int result, DebatePlayer taiwu, DebatePlayer npc, bool isTaiwu = false)
	{
		OperationType = type;
		PawnId = id;
		NpcPawnId = -1;
		TemplateId = -1;
		Result = result;
		IsTaiwu = isTaiwu;
		TaiwuPressure = taiwu.Pressure;
		TaiwuBases = taiwu.Bases;
		TaiwuGamePoint = taiwu.GamePoint;
		NpcPressure = npc.Pressure;
		NpcBases = npc.Bases;
		NpcGamePoint = npc.GamePoint;
	}

	public DebateOperation(sbyte type, int value1, int value2, int result, short templateId, DebatePlayer taiwu, DebatePlayer npc)
	{
		OperationType = type;
		Source = value1;
		Destination = value2;
		Index = result;
		TemplateId = templateId;
		TaiwuPressure = taiwu.Pressure;
		TaiwuBases = taiwu.Bases;
		TaiwuGamePoint = taiwu.GamePoint;
		NpcPressure = npc.Pressure;
		NpcBases = npc.Bases;
		NpcGamePoint = npc.GamePoint;
	}

	public DebateOperation(sbyte type, int value1, int value2, int result, IntPair bases, DebatePlayer taiwu, DebatePlayer npc)
	{
		OperationType = type;
		PawnId = value1;
		NpcPawnId = value2;
		Result = result;
		Target = bases;
		TemplateId = -1;
		TaiwuPressure = taiwu.Pressure;
		TaiwuBases = taiwu.Bases;
		TaiwuGamePoint = taiwu.GamePoint;
		NpcPressure = npc.Pressure;
		NpcBases = npc.Bases;
		NpcGamePoint = npc.GamePoint;
	}

	public DebateOperation(sbyte type, bool isTaiwu, IntPair ids, short result, DebatePlayer taiwu, DebatePlayer npc)
	{
		OperationType = type;
		PawnId = -1;
		NpcPawnId = -1;
		IsTaiwu = isTaiwu;
		Target = ids;
		TemplateId = result;
		TaiwuPressure = taiwu.Pressure;
		TaiwuBases = taiwu.Bases;
		TaiwuGamePoint = taiwu.GamePoint;
		NpcPressure = npc.Pressure;
		NpcBases = npc.Bases;
		NpcGamePoint = npc.GamePoint;
	}

	public DebateOperation(sbyte type, bool isTaiwu, DebatePlayer taiwu, DebatePlayer npc, short nodeEffectTemplateId = -1)
	{
		OperationType = type;
		PawnId = -1;
		NpcPawnId = -1;
		TemplateId = nodeEffectTemplateId;
		IsTaiwu = isTaiwu;
		TaiwuPressure = taiwu.Pressure;
		TaiwuBases = taiwu.Bases;
		TaiwuGamePoint = taiwu.GamePoint;
		NpcPressure = npc.Pressure;
		NpcBases = npc.Bases;
		NpcGamePoint = npc.GamePoint;
	}

	public DebateOperation(sbyte type, bool isTaiwu, short templateId, DebatePlayer taiwu, DebatePlayer npc)
	{
		OperationType = type;
		PawnId = -1;
		NpcPawnId = -1;
		IsTaiwu = isTaiwu;
		TemplateId = templateId;
		TaiwuPressure = taiwu.Pressure;
		TaiwuBases = taiwu.Bases;
		TaiwuGamePoint = taiwu.GamePoint;
		NpcPressure = npc.Pressure;
		NpcBases = npc.Bases;
		NpcGamePoint = npc.GamePoint;
	}

	public DebateOperation(sbyte type, bool isTaiwu, short templateId, DebatePlayer taiwu, DebatePlayer npc, List<StrategyTarget> strategyTargets, bool isFailed)
	{
		OperationType = type;
		PawnId = -1;
		NpcPawnId = -1;
		IsTaiwu = isTaiwu;
		TemplateId = templateId;
		TaiwuPressure = taiwu.Pressure;
		TaiwuBases = taiwu.Bases;
		TaiwuGamePoint = taiwu.GamePoint;
		NpcPressure = npc.Pressure;
		NpcBases = npc.Bases;
		NpcGamePoint = npc.GamePoint;
		StrategyTargets = strategyTargets;
		IsFailed = isFailed;
	}

	public DebateOperation(sbyte type, bool isTaiwu, sbyte pressureType, DebatePlayer taiwu, DebatePlayer npc)
	{
		OperationType = type;
		PawnId = -1;
		NpcPawnId = -1;
		IsTaiwu = isTaiwu;
		Result = pressureType;
		TaiwuPressure = taiwu.Pressure;
		TaiwuBases = taiwu.Bases;
		TaiwuGamePoint = taiwu.GamePoint;
		NpcPressure = npc.Pressure;
		NpcBases = npc.Bases;
		NpcGamePoint = npc.GamePoint;
	}

	public DebateOperation(sbyte type, DebateNodeEffectState nodeEffectState, IntPair coordinate, bool isTaiwu = false)
	{
		OperationType = type;
		NodeEffectState = nodeEffectState;
		Target = coordinate;
		IsTaiwu = isTaiwu;
	}

	public DebateOperation(sbyte type, bool isTaiwu, short templateId, int[] recordParams)
	{
		OperationType = type;
		IsTaiwu = isTaiwu;
		TemplateId = templateId;
		RecordParams = recordParams;
	}

	public DebateOperation()
	{
	}

	public DebateOperation(DebateOperation other)
	{
		OperationType = other.OperationType;
		PawnId = other.PawnId;
		NpcPawnId = other.NpcPawnId;
		TaiwuPressure = other.TaiwuPressure;
		TaiwuGamePoint = other.TaiwuGamePoint;
		TaiwuBases = other.TaiwuBases;
		NpcPressure = other.NpcPressure;
		NpcGamePoint = other.NpcGamePoint;
		NpcBases = other.NpcBases;
		TemplateId = other.TemplateId;
		Result = other.Result;
		IsFailed = other.IsFailed;
		Target = other.Target;
		Index = other.Index;
		Source = other.Source;
		Destination = other.Destination;
		IsTaiwu = other.IsTaiwu;
		if (other.StrategyTargets != null)
		{
			List<StrategyTarget> strategyTargets = other.StrategyTargets;
			int count = strategyTargets.Count;
			StrategyTargets = new List<StrategyTarget>(count);
			for (int i = 0; i < count; i++)
			{
				StrategyTargets.Add(new StrategyTarget(strategyTargets[i]));
			}
		}
		else
		{
			StrategyTargets = null;
		}
		NodeEffectState = new DebateNodeEffectState(other.NodeEffectState);
		int[] recordParams = other.RecordParams;
		int num = recordParams.Length;
		RecordParams = new int[num];
		for (int j = 0; j < num; j++)
		{
			RecordParams[j] = recordParams[j];
		}
	}

	public void Assign(DebateOperation other)
	{
		OperationType = other.OperationType;
		PawnId = other.PawnId;
		NpcPawnId = other.NpcPawnId;
		TaiwuPressure = other.TaiwuPressure;
		TaiwuGamePoint = other.TaiwuGamePoint;
		TaiwuBases = other.TaiwuBases;
		NpcPressure = other.NpcPressure;
		NpcGamePoint = other.NpcGamePoint;
		NpcBases = other.NpcBases;
		TemplateId = other.TemplateId;
		Result = other.Result;
		IsFailed = other.IsFailed;
		Target = other.Target;
		Index = other.Index;
		Source = other.Source;
		Destination = other.Destination;
		IsTaiwu = other.IsTaiwu;
		if (other.StrategyTargets != null)
		{
			List<StrategyTarget> strategyTargets = other.StrategyTargets;
			int count = strategyTargets.Count;
			StrategyTargets = new List<StrategyTarget>(count);
			for (int i = 0; i < count; i++)
			{
				StrategyTargets.Add(new StrategyTarget(strategyTargets[i]));
			}
		}
		else
		{
			StrategyTargets = null;
		}
		NodeEffectState = new DebateNodeEffectState(other.NodeEffectState);
		int[] recordParams = other.RecordParams;
		int num = recordParams.Length;
		RecordParams = new int[num];
		for (int j = 0; j < num; j++)
		{
			RecordParams[j] = recordParams[j];
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 61;
		if (StrategyTargets != null)
		{
			num += 2;
			int count = StrategyTargets.Count;
			for (int i = 0; i < count; i++)
			{
				StrategyTarget strategyTarget = StrategyTargets[i];
				num = ((strategyTarget == null) ? (num + 2) : (num + (2 + strategyTarget.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		num = ((NodeEffectState == null) ? (num + 2) : (num + (2 + NodeEffectState.GetSerializedSize())));
		num = ((RecordParams == null) ? (num + 2) : (num + (2 + 4 * RecordParams.Length)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)OperationType;
		ptr++;
		*(int*)ptr = PawnId;
		ptr += 4;
		*(int*)ptr = NpcPawnId;
		ptr += 4;
		*(int*)ptr = TaiwuPressure;
		ptr += 4;
		*(int*)ptr = TaiwuGamePoint;
		ptr += 4;
		*(int*)ptr = TaiwuBases;
		ptr += 4;
		*(int*)ptr = NpcPressure;
		ptr += 4;
		*(int*)ptr = NpcGamePoint;
		ptr += 4;
		*(int*)ptr = NpcBases;
		ptr += 4;
		*(short*)ptr = TemplateId;
		ptr += 2;
		*(int*)ptr = Result;
		ptr += 4;
		*ptr = (IsFailed ? ((byte)1) : ((byte)0));
		ptr++;
		ptr += Target.Serialize(ptr);
		*(int*)ptr = Index;
		ptr += 4;
		*(int*)ptr = Source;
		ptr += 4;
		*(int*)ptr = Destination;
		ptr += 4;
		*ptr = (IsTaiwu ? ((byte)1) : ((byte)0));
		ptr++;
		if (StrategyTargets != null)
		{
			int count = StrategyTargets.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				StrategyTarget strategyTarget = StrategyTargets[i];
				if (strategyTarget != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = strategyTarget.Serialize(ptr);
					ptr += num;
					Tester.Assert(num <= 65535);
					*(ushort*)intPtr = (ushort)num;
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
		if (NodeEffectState != null)
		{
			byte* intPtr2 = ptr;
			ptr += 2;
			int num2 = NodeEffectState.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr2 = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (RecordParams != null)
		{
			int num3 = RecordParams.Length;
			Tester.Assert(num3 <= 65535);
			*(ushort*)ptr = (ushort)num3;
			ptr += 2;
			for (int j = 0; j < num3; j++)
			{
				((int*)ptr)[j] = RecordParams[j];
			}
			ptr += 4 * num3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
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
		OperationType = (sbyte)(*ptr);
		ptr++;
		PawnId = *(int*)ptr;
		ptr += 4;
		NpcPawnId = *(int*)ptr;
		ptr += 4;
		TaiwuPressure = *(int*)ptr;
		ptr += 4;
		TaiwuGamePoint = *(int*)ptr;
		ptr += 4;
		TaiwuBases = *(int*)ptr;
		ptr += 4;
		NpcPressure = *(int*)ptr;
		ptr += 4;
		NpcGamePoint = *(int*)ptr;
		ptr += 4;
		NpcBases = *(int*)ptr;
		ptr += 4;
		TemplateId = *(short*)ptr;
		ptr += 2;
		Result = *(int*)ptr;
		ptr += 4;
		IsFailed = *ptr != 0;
		ptr++;
		ptr += Target.Deserialize(ptr);
		Index = *(int*)ptr;
		ptr += 4;
		Source = *(int*)ptr;
		ptr += 4;
		Destination = *(int*)ptr;
		ptr += 4;
		IsTaiwu = *ptr != 0;
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (StrategyTargets == null)
			{
				StrategyTargets = new List<StrategyTarget>(num);
			}
			else
			{
				StrategyTargets.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				if (num2 > 0)
				{
					StrategyTarget strategyTarget = new StrategyTarget();
					ptr += strategyTarget.Deserialize(ptr);
					StrategyTargets.Add(strategyTarget);
				}
				else
				{
					StrategyTargets.Add(null);
				}
			}
		}
		else
		{
			StrategyTargets?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (NodeEffectState == null)
			{
				NodeEffectState = new DebateNodeEffectState();
			}
			ptr += NodeEffectState.Deserialize(ptr);
		}
		else
		{
			NodeEffectState = null;
		}
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (RecordParams == null || RecordParams.Length != num4)
			{
				RecordParams = new int[num4];
			}
			for (int j = 0; j < num4; j++)
			{
				RecordParams[j] = ((int*)ptr)[j];
			}
			ptr += 4 * num4;
		}
		else
		{
			RecordParams = null;
		}
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}
}
