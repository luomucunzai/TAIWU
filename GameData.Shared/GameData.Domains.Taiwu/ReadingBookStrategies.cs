using System;
using Config;
using GameData.Serializer;

namespace GameData.Domains.Taiwu;

public struct ReadingBookStrategies : ISerializableGameData
{
	public const int StrategiesPerPage = 3;

	private const int MaxTotalStrategyCount = 18;

	public unsafe fixed sbyte StrategyIds[18];

	public unsafe fixed sbyte Bonus[18];

	public unsafe void Initialize()
	{
		fixed (sbyte* strategyIds = StrategyIds)
		{
			sbyte* num = strategyIds;
			*(long*)num = -1L;
			((long*)num)[1] = -1L;
			((short*)num)[8] = -1;
		}
		fixed (sbyte* strategyIds = Bonus)
		{
			sbyte* num2 = strategyIds;
			*(long*)num2 = 0L;
			((long*)num2)[1] = 0L;
			((short*)num2)[8] = 0;
		}
	}

	public unsafe sbyte GetPageStrategy(byte pageIndex, int strategyIndex)
	{
		return StrategyIds[pageIndex * 3 + strategyIndex];
	}

	public unsafe void SetPageStrategy(byte pageIndex, int strategyIndex, sbyte strategyId, sbyte efficiencyBonus = 0)
	{
		int num = pageIndex * 3 + strategyIndex;
		StrategyIds[num] = strategyId;
		Bonus[num] = efficiencyBonus;
	}

	public unsafe void ClearPageStrategies(byte pageIndex)
	{
		for (int i = 0; i < 3; i++)
		{
			int num = pageIndex * 3 + i;
			StrategyIds[num] = -1;
			Bonus[num] = 0;
		}
	}

	public unsafe bool IsStrategySlotsFullAtPage(byte pageIndex)
	{
		for (int i = 0; i < 3; i++)
		{
			if (StrategyIds[pageIndex * 3 + i] == -1)
			{
				return false;
			}
		}
		return true;
	}

	public unsafe short GetPageIntCostChange(byte pageIndex)
	{
		short num = 0;
		for (int i = 0; i < 3; i++)
		{
			sbyte b = StrategyIds[pageIndex * 3 + i];
			if (b >= 0)
			{
				num += ReadingStrategy.Instance[b].CurrPageIntCostChange;
			}
		}
		return num;
	}

	public unsafe bool PageContainsStrategy(byte pageIndex, sbyte strategyId)
	{
		for (int i = 0; i < 3; i++)
		{
			sbyte b = StrategyIds[pageIndex * 3 + i];
			if (strategyId == b)
			{
				return true;
			}
		}
		return false;
	}

	public unsafe bool GetSkipPage(byte pageIndex)
	{
		for (int i = 0; i < 3; i++)
		{
			sbyte b = StrategyIds[pageIndex * 3 + i];
			if (b >= 0 && ReadingStrategy.Instance[b].SkipPage)
			{
				return true;
			}
		}
		return false;
	}

	public unsafe int GetPageReadingEfficiencyBonus(byte pageIndex)
	{
		int num = 0;
		int num2 = pageIndex * 3;
		for (int i = 0; i < pageIndex; i++)
		{
			int num3 = 0;
			int num4 = 0;
			for (int j = 0; j < 3; j++)
			{
				sbyte b = StrategyIds[i * 3 + j];
				if (b >= 0)
				{
					if (b >= ReadingStrategy.Instance.Count)
					{
						throw new Exception($"strategy id {b} at index {i * 3 + j} out of range: [0, {ReadingStrategy.Instance.Count}).");
					}
					num3 += ReadingStrategy.Instance[b].FollowingPagesEfficiencyChange;
					if (b == 5)
					{
						num4++;
					}
				}
			}
			for (int k = 0; k < num4; k++)
			{
				num3 *= 2;
			}
			num += num3;
		}
		for (int l = num2; l < num2 + 3; l++)
		{
			sbyte b2 = StrategyIds[l];
			if (b2 >= ReadingStrategy.Instance.Count)
			{
				throw new Exception($"strategy id {b2} at index {l} out of range: [0, {ReadingStrategy.Instance.Count}).");
			}
			if (b2 >= 0)
			{
				num += Bonus[l];
			}
		}
		return num;
	}

	public unsafe override string ToString()
	{
		string text = "";
		for (byte b = 0; b < 6; b++)
		{
			for (int i = 0; i < 3; i++)
			{
				sbyte index = StrategyIds[b * 3 + i];
				text = text + ReadingStrategy.Instance[index].Name + " ";
			}
			text += "\n";
		}
		return text;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 36;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		fixed (sbyte* strategyIds = StrategyIds)
		{
			*(long*)pData = *(long*)strategyIds;
			((long*)pData)[1] = ((long*)strategyIds)[1];
			((short*)pData)[8] = ((short*)strategyIds)[8];
		}
		fixed (sbyte* bonus = Bonus)
		{
			*(long*)(pData + 18) = *(long*)bonus;
			*(long*)(pData + 26) = ((long*)bonus)[1];
			((short*)pData)[17] = ((short*)bonus)[8];
		}
		int num = 36;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		fixed (sbyte* strategyIds = StrategyIds)
		{
			sbyte* num = strategyIds;
			*(long*)num = *(long*)pData;
			((long*)num)[1] = ((long*)pData)[1];
			((short*)num)[8] = ((short*)pData)[8];
		}
		fixed (sbyte* strategyIds = Bonus)
		{
			sbyte* num2 = strategyIds;
			*(long*)num2 = *(long*)(pData + 18);
			((long*)num2)[1] = *(long*)(pData + 26);
			((short*)num2)[8] = ((short*)pData)[17];
		}
		int num3 = 36;
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
