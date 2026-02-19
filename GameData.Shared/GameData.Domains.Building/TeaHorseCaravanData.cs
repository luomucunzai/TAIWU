using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Building;

public class TeaHorseCaravanData : ISerializableGameData
{
	public const sbyte FromInventory = 1;

	public const sbyte FromWarehouse = 2;

	public const sbyte FromTreasury = 3;

	public const sbyte FromStockStorage = 4;

	[SerializableGameDataField]
	public List<(ItemKey, sbyte)> CarryGoodsList;

	[SerializableGameDataField]
	public List<ItemKey> ExchangeGoodsList;

	[SerializableGameDataField]
	public List<short> DiaryList;

	[SerializableGameDataField]
	public sbyte CaravanState;

	[SerializableGameDataField]
	public bool IsStartSearch;

	[SerializableGameDataField]
	public short Weather;

	[SerializableGameDataField]
	public short Terrain;

	[SerializableGameDataField]
	public short CaravanAwareness;

	[SerializableGameDataField]
	public short CaravanReplenishment;

	[SerializableGameDataField]
	public short LackReplenishmentTurn;

	[SerializableGameDataField]
	public bool IsShowSeachReplenishment;

	[SerializableGameDataField]
	public bool IsShowExchangeReplenishment;

	[SerializableGameDataField]
	public short DistanceToTaiwuVillage;

	[SerializableGameDataField]
	public short StartMonth;

	[SerializableGameDataField]
	public short ExchangeReplenishmentAmountMax;

	[SerializableGameDataField]
	public short ExchangeReplenishmentRemainAmount;

	[SerializableGameDataField]
	public short SearchReplenishmentMax;

	[SerializableGameDataField]
	public short SearchReplenishmentAmount;

	public TeaHorseCaravanData()
	{
		CarryGoodsList = new List<(ItemKey, sbyte)>();
		ExchangeGoodsList = new List<ItemKey>();
		DiaryList = new List<short>();
		CaravanAwareness = 100;
		CaravanReplenishment = 100;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 26;
		num = ((CarryGoodsList == null) ? (num + 2) : (num + (2 + 9 * CarryGoodsList.Count)));
		num = ((ExchangeGoodsList == null) ? (num + 2) : (num + (2 + 8 * ExchangeGoodsList.Count)));
		num = ((DiaryList == null) ? (num + 2) : (num + (2 + 2 * DiaryList.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (CarryGoodsList != null)
		{
			int count = CarryGoodsList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += CarryGoodsList[i].Item1.Serialize(ptr);
				*ptr = (byte)CarryGoodsList[i].Item2;
				ptr++;
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (ExchangeGoodsList != null)
		{
			int count2 = ExchangeGoodsList.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				ptr += ExchangeGoodsList[j].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (DiaryList != null)
		{
			int count3 = DiaryList.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				((short*)ptr)[k] = DiaryList[k];
			}
			ptr += 2 * count3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)CaravanState;
		ptr++;
		*ptr = (IsStartSearch ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = Weather;
		ptr += 2;
		*(short*)ptr = Terrain;
		ptr += 2;
		*(short*)ptr = CaravanAwareness;
		ptr += 2;
		*(short*)ptr = CaravanReplenishment;
		ptr += 2;
		*(short*)ptr = LackReplenishmentTurn;
		ptr += 2;
		*ptr = (IsShowSeachReplenishment ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (IsShowExchangeReplenishment ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = DistanceToTaiwuVillage;
		ptr += 2;
		*(short*)ptr = StartMonth;
		ptr += 2;
		*(short*)ptr = ExchangeReplenishmentAmountMax;
		ptr += 2;
		*(short*)ptr = ExchangeReplenishmentRemainAmount;
		ptr += 2;
		*(short*)ptr = SearchReplenishmentMax;
		ptr += 2;
		*(short*)ptr = SearchReplenishmentAmount;
		ptr += 2;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (CarryGoodsList == null)
			{
				CarryGoodsList = new List<(ItemKey, sbyte)>(num);
			}
			else
			{
				CarryGoodsList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ItemKey item = default(ItemKey);
				ptr += item.Deserialize(ptr);
				sbyte item2 = (sbyte)(*ptr);
				ptr++;
				CarryGoodsList.Add((item, item2));
			}
		}
		else
		{
			CarryGoodsList?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (ExchangeGoodsList == null)
			{
				ExchangeGoodsList = new List<ItemKey>(num2);
			}
			else
			{
				ExchangeGoodsList.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				ItemKey item3 = default(ItemKey);
				ptr += item3.Deserialize(ptr);
				ExchangeGoodsList.Add(item3);
			}
		}
		else
		{
			ExchangeGoodsList?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (DiaryList == null)
			{
				DiaryList = new List<short>(num3);
			}
			else
			{
				DiaryList.Clear();
			}
			for (int k = 0; k < num3; k++)
			{
				DiaryList.Add(((short*)ptr)[k]);
			}
			ptr += 2 * num3;
		}
		else
		{
			DiaryList?.Clear();
		}
		CaravanState = (sbyte)(*ptr);
		ptr++;
		IsStartSearch = *ptr != 0;
		ptr++;
		Weather = *(short*)ptr;
		ptr += 2;
		Terrain = *(short*)ptr;
		ptr += 2;
		CaravanAwareness = *(short*)ptr;
		ptr += 2;
		CaravanReplenishment = *(short*)ptr;
		ptr += 2;
		LackReplenishmentTurn = *(short*)ptr;
		ptr += 2;
		IsShowSeachReplenishment = *ptr != 0;
		ptr++;
		IsShowExchangeReplenishment = *ptr != 0;
		ptr++;
		DistanceToTaiwuVillage = *(short*)ptr;
		ptr += 2;
		StartMonth = *(short*)ptr;
		ptr += 2;
		ExchangeReplenishmentAmountMax = *(short*)ptr;
		ptr += 2;
		ExchangeReplenishmentRemainAmount = *(short*)ptr;
		ptr += 2;
		SearchReplenishmentMax = *(short*)ptr;
		ptr += 2;
		SearchReplenishmentAmount = *(short*)ptr;
		ptr += 2;
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
