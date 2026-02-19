using System.Collections.Generic;
using System.Linq;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Debate;

public class Pawn : ISerializableGameData
{
	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public IntPair Coordinate;

	[SerializableGameDataField]
	public bool IsOwnedByTaiwu;

	[SerializableGameDataField]
	public int Bases;

	[SerializableGameDataField]
	public bool IsRevealed;

	[SerializableGameDataField]
	public bool IsAlive;

	[SerializableGameDataField]
	public bool IsZero;

	[SerializableGameDataField(ArrayElementsCount = 3)]
	public int[] Strategies = new int[3] { -1, -1, -1 };

	public int NodeDamage;

	public bool Damaged;

	public bool IsHalt;

	public bool IsSwitchLocation;

	public bool IsImmuneDebuff;

	public bool IsHalfImmuneRemove;

	public bool IsImmuneRemove;

	public int ChangeSelfGamePointByDamagePercent;

	public bool ChangeSelfGamePointByDamagePercentIsCastedByTaiwu;

	public List<PawnDamageInfo> DamageList = new List<PawnDamageInfo>();

	public int DamageToSelf => DamageList.Where((PawnDamageInfo d) => d.IsToSelf).Sum((PawnDamageInfo d) => d.Damage);

	public int DamageToOpponent => DamageList.Where((PawnDamageInfo d) => !d.IsToSelf && !d.IsStrategyDamage).Sum((PawnDamageInfo d) => d.Damage);

	public void ResetNodeValue()
	{
		NodeDamage = 0;
		Damaged = false;
	}

	public void ResetStrategyValue()
	{
		IsHalt = false;
		IsSwitchLocation = false;
		IsHalfImmuneRemove = false;
		IsImmuneRemove = false;
		IsImmuneDebuff = false;
		ChangeSelfGamePointByDamagePercent = 0;
		DamageList.Clear();
	}

	public Pawn(int id, IntPair coordinate, bool isOwnedByTaiwu, int bases)
	{
		Id = id;
		Coordinate = coordinate;
		IsOwnedByTaiwu = isOwnedByTaiwu;
		Bases = bases;
		IsRevealed = false;
		IsAlive = true;
		IsZero = bases == 0;
	}

	public Pawn()
	{
	}

	public Pawn(Pawn other)
	{
		Id = other.Id;
		Coordinate = other.Coordinate;
		IsOwnedByTaiwu = other.IsOwnedByTaiwu;
		Bases = other.Bases;
		IsRevealed = other.IsRevealed;
		IsAlive = other.IsAlive;
		IsZero = other.IsZero;
		int[] strategies = other.Strategies;
		int num = strategies.Length;
		Strategies = new int[num];
		for (int i = 0; i < num; i++)
		{
			Strategies[i] = strategies[i];
		}
	}

	public void Assign(Pawn other)
	{
		Id = other.Id;
		Coordinate = other.Coordinate;
		IsOwnedByTaiwu = other.IsOwnedByTaiwu;
		Bases = other.Bases;
		IsRevealed = other.IsRevealed;
		IsAlive = other.IsAlive;
		IsZero = other.IsZero;
		int[] strategies = other.Strategies;
		int num = strategies.Length;
		Strategies = new int[num];
		for (int i = 0; i < num; i++)
		{
			Strategies[i] = strategies[i];
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 32;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = Id;
		ptr += 4;
		ptr += Coordinate.Serialize(ptr);
		*ptr = (IsOwnedByTaiwu ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = Bases;
		ptr += 4;
		*ptr = (IsRevealed ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (IsAlive ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (IsZero ? ((byte)1) : ((byte)0));
		ptr++;
		Tester.Assert(Strategies.Length == 3);
		for (int i = 0; i < 3; i++)
		{
			((int*)ptr)[i] = Strategies[i];
		}
		ptr += 12;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Id = *(int*)ptr;
		ptr += 4;
		ptr += Coordinate.Deserialize(ptr);
		IsOwnedByTaiwu = *ptr != 0;
		ptr++;
		Bases = *(int*)ptr;
		ptr += 4;
		IsRevealed = *ptr != 0;
		ptr++;
		IsAlive = *ptr != 0;
		ptr++;
		IsZero = *ptr != 0;
		ptr++;
		if (Strategies == null || Strategies.Length != 3)
		{
			Strategies = new int[3];
		}
		for (int i = 0; i < 3; i++)
		{
			Strategies[i] = ((int*)ptr)[i];
		}
		ptr += 12;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
