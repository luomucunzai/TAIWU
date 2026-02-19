using System;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Item;

[SerializableGameData(NotForArchive = true)]
public class CricketData : ISerializableGameData
{
	[SerializableGameDataField]
	public short[] Injuries;

	[SerializableGameDataField]
	public short WinsCount;

	[SerializableGameDataField]
	public short LossesCount;

	[SerializableGameDataField]
	public short BestEnemyColorId;

	[SerializableGameDataField]
	public short BestEnemyPartId;

	[SerializableGameDataField]
	public short Age;

	[SerializableGameDataField]
	public short MaxAge;

	[SerializableGameDataField]
	public bool IsSmart;

	[SerializableGameDataField]
	public bool IsIdentified;

	[SerializableGameDataField]
	public int CircketValue;

	public short InjuryHp => Injuries[0];

	public short InjurySp => Injuries[1];

	public short InjuryVigor => Injuries[2];

	public short InjuryStrength => Injuries[3];

	public short InjuryBite => Injuries[4];

	public short this[int index]
	{
		get
		{
			switch (index)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
				return Injuries[index];
			case 5:
				return WinsCount;
			case 6:
				return LossesCount;
			case 7:
				return BestEnemyColorId;
			case 8:
				return BestEnemyPartId;
			case 9:
				return Age;
			case 10:
				return IsSmart ? ((short)1) : ((short)0);
			case 11:
				return IsIdentified ? ((short)1) : ((short)0);
			case 12:
				return MaxAge;
			default:
				throw new IndexOutOfRangeException($"index is not valid for cricket display data {index}");
			}
		}
	}

	public CricketData()
	{
	}

	public CricketData(CricketData other)
	{
		short[] injuries = other.Injuries;
		int num = injuries.Length;
		Injuries = new short[num];
		for (int i = 0; i < num; i++)
		{
			Injuries[i] = injuries[i];
		}
		WinsCount = other.WinsCount;
		LossesCount = other.LossesCount;
		BestEnemyColorId = other.BestEnemyColorId;
		BestEnemyPartId = other.BestEnemyPartId;
		Age = other.Age;
		MaxAge = other.MaxAge;
		IsSmart = other.IsSmart;
		IsIdentified = other.IsIdentified;
		CircketValue = other.CircketValue;
	}

	public void Assign(CricketData other)
	{
		short[] injuries = other.Injuries;
		int num = injuries.Length;
		Injuries = new short[num];
		for (int i = 0; i < num; i++)
		{
			Injuries[i] = injuries[i];
		}
		WinsCount = other.WinsCount;
		LossesCount = other.LossesCount;
		BestEnemyColorId = other.BestEnemyColorId;
		BestEnemyPartId = other.BestEnemyPartId;
		Age = other.Age;
		MaxAge = other.MaxAge;
		IsSmart = other.IsSmart;
		IsIdentified = other.IsIdentified;
		CircketValue = other.CircketValue;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 18;
		num = ((Injuries == null) ? (num + 2) : (num + (2 + 2 * Injuries.Length)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (Injuries != null)
		{
			int num = Injuries.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				((short*)ptr)[i] = Injuries[i];
			}
			ptr += 2 * num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(short*)ptr = WinsCount;
		ptr += 2;
		*(short*)ptr = LossesCount;
		ptr += 2;
		*(short*)ptr = BestEnemyColorId;
		ptr += 2;
		*(short*)ptr = BestEnemyPartId;
		ptr += 2;
		*(short*)ptr = Age;
		ptr += 2;
		*(short*)ptr = MaxAge;
		ptr += 2;
		*ptr = (IsSmart ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (IsIdentified ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = CircketValue;
		ptr += 4;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Injuries == null || Injuries.Length != num)
			{
				Injuries = new short[num];
			}
			for (int i = 0; i < num; i++)
			{
				Injuries[i] = ((short*)ptr)[i];
			}
			ptr += 2 * num;
		}
		else
		{
			Injuries = null;
		}
		WinsCount = *(short*)ptr;
		ptr += 2;
		LossesCount = *(short*)ptr;
		ptr += 2;
		BestEnemyColorId = *(short*)ptr;
		ptr += 2;
		BestEnemyPartId = *(short*)ptr;
		ptr += 2;
		Age = *(short*)ptr;
		ptr += 2;
		MaxAge = *(short*)ptr;
		ptr += 2;
		IsSmart = *ptr != 0;
		ptr++;
		IsIdentified = *ptr != 0;
		ptr++;
		CircketValue = *(int*)ptr;
		ptr += 4;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
