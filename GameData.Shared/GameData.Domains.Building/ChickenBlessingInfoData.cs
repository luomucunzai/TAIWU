using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Building;

public struct ChickenBlessingInfoData : ISerializableGameData
{
	[SerializableGameDataField]
	public Dictionary<short, sbyte> RemainingMonths;

	public ChickenBlessingInfoData(ChickenBlessingInfoData other)
	{
		RemainingMonths = new Dictionary<short, sbyte>(other.RemainingMonths);
	}

	public void Assign(ChickenBlessingInfoData other)
	{
		RemainingMonths = new Dictionary<short, sbyte>(other.RemainingMonths);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((RemainingMonths == null) ? (num + 4) : (num + (4 + 3 * RemainingMonths.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (RemainingMonths != null)
		{
			int count = RemainingMonths.Count;
			*(int*)ptr = count;
			ptr += 4;
			foreach (KeyValuePair<short, sbyte> remainingMonth in RemainingMonths)
			{
				*(short*)ptr = remainingMonth.Key;
				ptr += 2;
				*ptr = (byte)remainingMonth.Value;
				ptr++;
			}
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
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
		uint num = *(uint*)ptr;
		ptr += 4;
		if (num != 0)
		{
			if (RemainingMonths == null)
			{
				RemainingMonths = new Dictionary<short, sbyte>();
			}
			else
			{
				RemainingMonths.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				short key = *(short*)ptr;
				ptr += 2;
				sbyte value = (sbyte)(*ptr);
				ptr++;
				RemainingMonths.Add(key, value);
			}
		}
		else
		{
			RemainingMonths?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
