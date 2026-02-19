using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Debate;

public class DebatePlayer : ISerializableGameData
{
	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public int GamePoint;

	[SerializableGameDataField]
	public int Pressure;

	[SerializableGameDataField]
	public int MaxPressure;

	[SerializableGameDataField]
	public int HighestPressure;

	[SerializableGameDataField]
	public int Bases;

	[SerializableGameDataField]
	public int MaxBases;

	[SerializableGameDataField]
	public int StrategyPoint;

	[SerializableGameDataField]
	public int MakeMoveCount;

	[SerializableGameDataField]
	public List<short> OwnedCards = new List<short>();

	[SerializableGameDataField]
	public List<short> UsedCards = new List<short>();

	[SerializableGameDataField]
	public List<short> CanUseCards = new List<short>();

	public DebatePlayer(int id, int maxPressure, int attainment, List<short> ownedCards)
	{
		Id = id;
		GamePoint = DebateConstants.MaxGamePoint;
		Pressure = 0;
		MaxPressure = maxPressure;
		HighestPressure = 0;
		Bases = attainment;
		MaxBases = attainment;
		StrategyPoint = DebateConstants.InitialStrategyPoint;
		MakeMoveCount = 0;
		OwnedCards = ownedCards;
	}

	public DebatePlayer()
	{
	}

	public DebatePlayer(DebatePlayer other)
	{
		Id = other.Id;
		GamePoint = other.GamePoint;
		Pressure = other.Pressure;
		MaxPressure = other.MaxPressure;
		HighestPressure = other.HighestPressure;
		Bases = other.Bases;
		MaxBases = other.MaxBases;
		StrategyPoint = other.StrategyPoint;
		MakeMoveCount = other.MakeMoveCount;
		OwnedCards = ((other.OwnedCards == null) ? null : new List<short>(other.OwnedCards));
		UsedCards = ((other.UsedCards == null) ? null : new List<short>(other.UsedCards));
		CanUseCards = ((other.CanUseCards == null) ? null : new List<short>(other.CanUseCards));
	}

	public void Assign(DebatePlayer other)
	{
		Id = other.Id;
		GamePoint = other.GamePoint;
		Pressure = other.Pressure;
		MaxPressure = other.MaxPressure;
		HighestPressure = other.HighestPressure;
		Bases = other.Bases;
		MaxBases = other.MaxBases;
		StrategyPoint = other.StrategyPoint;
		MakeMoveCount = other.MakeMoveCount;
		OwnedCards = ((other.OwnedCards == null) ? null : new List<short>(other.OwnedCards));
		UsedCards = ((other.UsedCards == null) ? null : new List<short>(other.UsedCards));
		CanUseCards = ((other.CanUseCards == null) ? null : new List<short>(other.CanUseCards));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 36;
		num = ((OwnedCards == null) ? (num + 2) : (num + (2 + 2 * OwnedCards.Count)));
		num = ((UsedCards == null) ? (num + 2) : (num + (2 + 2 * UsedCards.Count)));
		num = ((CanUseCards == null) ? (num + 2) : (num + (2 + 2 * CanUseCards.Count)));
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
		*(int*)ptr = GamePoint;
		ptr += 4;
		*(int*)ptr = Pressure;
		ptr += 4;
		*(int*)ptr = MaxPressure;
		ptr += 4;
		*(int*)ptr = HighestPressure;
		ptr += 4;
		*(int*)ptr = Bases;
		ptr += 4;
		*(int*)ptr = MaxBases;
		ptr += 4;
		*(int*)ptr = StrategyPoint;
		ptr += 4;
		*(int*)ptr = MakeMoveCount;
		ptr += 4;
		if (OwnedCards != null)
		{
			int count = OwnedCards.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = OwnedCards[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (UsedCards != null)
		{
			int count2 = UsedCards.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				((short*)ptr)[j] = UsedCards[j];
			}
			ptr += 2 * count2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (CanUseCards != null)
		{
			int count3 = CanUseCards.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				((short*)ptr)[k] = CanUseCards[k];
			}
			ptr += 2 * count3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
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
		GamePoint = *(int*)ptr;
		ptr += 4;
		Pressure = *(int*)ptr;
		ptr += 4;
		MaxPressure = *(int*)ptr;
		ptr += 4;
		HighestPressure = *(int*)ptr;
		ptr += 4;
		Bases = *(int*)ptr;
		ptr += 4;
		MaxBases = *(int*)ptr;
		ptr += 4;
		StrategyPoint = *(int*)ptr;
		ptr += 4;
		MakeMoveCount = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (OwnedCards == null)
			{
				OwnedCards = new List<short>(num);
			}
			else
			{
				OwnedCards.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				OwnedCards.Add(((short*)ptr)[i]);
			}
			ptr += 2 * num;
		}
		else
		{
			OwnedCards?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (UsedCards == null)
			{
				UsedCards = new List<short>(num2);
			}
			else
			{
				UsedCards.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				UsedCards.Add(((short*)ptr)[j]);
			}
			ptr += 2 * num2;
		}
		else
		{
			UsedCards?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (CanUseCards == null)
			{
				CanUseCards = new List<short>(num3);
			}
			else
			{
				CanUseCards.Clear();
			}
			for (int k = 0; k < num3; k++)
			{
				CanUseCards.Add(((short*)ptr)[k]);
			}
			ptr += 2 * num3;
		}
		else
		{
			CanUseCards?.Clear();
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
