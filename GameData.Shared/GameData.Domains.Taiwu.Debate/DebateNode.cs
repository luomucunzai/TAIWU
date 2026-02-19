using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Debate;

public class DebateNode : ISerializableGameData
{
	[SerializableGameDataField]
	public IntPair Coordinate;

	[SerializableGameDataField]
	public bool IsVantage;

	[SerializableGameDataField]
	public bool IsVisible;

	[SerializableGameDataField]
	public bool TaiwuCanMakeMove;

	[SerializableGameDataField]
	public bool NpcCanMakeMove;

	[SerializableGameDataField]
	public int PawnId;

	[SerializableGameDataField]
	public DebateNodeEffectState EffectState;

	public DebateNode(int x, int y)
	{
		Coordinate = new IntPair(x, y);
		IsVantage = x < DebateConstants.TaiwuVantageNodeCount[y];
		IsVisible = IsVantage;
		TaiwuCanMakeMove = false;
		PawnId = -1;
		EffectState = DebateNodeEffectState.Invalid;
	}

	public DebateNode()
	{
	}

	public DebateNode(DebateNode other)
	{
		Coordinate = other.Coordinate;
		IsVantage = other.IsVantage;
		IsVisible = other.IsVisible;
		TaiwuCanMakeMove = other.TaiwuCanMakeMove;
		NpcCanMakeMove = other.NpcCanMakeMove;
		PawnId = other.PawnId;
		EffectState = new DebateNodeEffectState(other.EffectState);
	}

	public void Assign(DebateNode other)
	{
		Coordinate = other.Coordinate;
		IsVantage = other.IsVantage;
		IsVisible = other.IsVisible;
		TaiwuCanMakeMove = other.TaiwuCanMakeMove;
		NpcCanMakeMove = other.NpcCanMakeMove;
		PawnId = other.PawnId;
		EffectState = new DebateNodeEffectState(other.EffectState);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 16;
		num = ((EffectState == null) ? (num + 2) : (num + (2 + EffectState.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += Coordinate.Serialize(ptr);
		*ptr = (IsVantage ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (IsVisible ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (TaiwuCanMakeMove ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (NpcCanMakeMove ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = PawnId;
		ptr += 4;
		if (EffectState != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = EffectState.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += Coordinate.Deserialize(ptr);
		IsVantage = *ptr != 0;
		ptr++;
		IsVisible = *ptr != 0;
		ptr++;
		TaiwuCanMakeMove = *ptr != 0;
		ptr++;
		NpcCanMakeMove = *ptr != 0;
		ptr++;
		PawnId = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (EffectState == null)
			{
				EffectState = new DebateNodeEffectState();
			}
			ptr += EffectState.Deserialize(ptr);
		}
		else
		{
			EffectState = null;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
