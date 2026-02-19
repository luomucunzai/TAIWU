using System;
using System.Collections.Generic;
using Config;
using Config.ConfigCells;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.Domains.Merchant;

[SerializableGameData(NotRestrictCollectionSerializedSize = true)]
public class MerchantData : ISerializableGameData
{
	public const int BehaviorAddDiscount = 25;

	public const int BehaviorReduceDiscount = -25;

	[SerializableGameDataField]
	public int CharId;

	[SerializableGameDataField]
	public sbyte MerchantTemplateId;

	[SerializableGameDataField]
	public int Money;

	[Obsolete]
	[SerializableGameDataField]
	public sbyte GoodsKeepTime;

	[SerializableGameDataField]
	public Inventory GoodsList0;

	[SerializableGameDataField]
	public Inventory GoodsList1;

	[SerializableGameDataField]
	public Inventory GoodsList2;

	[SerializableGameDataField]
	public Inventory GoodsList3;

	[SerializableGameDataField]
	public Inventory GoodsList4;

	[SerializableGameDataField]
	public Inventory GoodsList5;

	[SerializableGameDataField]
	public Inventory GoodsList6;

	[Obsolete("已废弃，只能往出来转移到MerchantBuyBackData")]
	[SerializableGameDataField]
	public Inventory BuyInGoodsList;

	[Obsolete("已废弃，只能往出来转移到MerchantBuyBackData")]
	[SerializableGameDataField]
	public readonly Dictionary<ItemKey, int> BuyInPrice = new Dictionary<ItemKey, int>();

	[SerializableGameDataField]
	public readonly Dictionary<ItemKey, int> PriceChangeData = new Dictionary<ItemKey, int>();

	public const int MaxGoodsListCount = 7;

	public MerchantItem MerchantConfig => Config.Merchant.Instance[MerchantTemplateId];

	public sbyte MerchantType => MerchantConfig.MerchantType;

	public sbyte MerchantLevel => MerchantConfig.Level;

	public MerchantItem GroupConfig => Config.Merchant.Instance[MerchantConfig.GroupId];

	public MerchantData(int charId, sbyte merchantTemplateId)
	{
		CharId = charId;
		MerchantTemplateId = merchantTemplateId;
	}

	public MerchantData()
	{
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 10;
		num = ((GoodsList0 == null) ? (num + 2) : (num + (2 + GoodsList0.GetSerializedSize())));
		num = ((GoodsList1 == null) ? (num + 2) : (num + (2 + GoodsList1.GetSerializedSize())));
		num = ((GoodsList2 == null) ? (num + 2) : (num + (2 + GoodsList2.GetSerializedSize())));
		num = ((GoodsList3 == null) ? (num + 2) : (num + (2 + GoodsList3.GetSerializedSize())));
		num = ((GoodsList4 == null) ? (num + 2) : (num + (2 + GoodsList4.GetSerializedSize())));
		num = ((GoodsList5 == null) ? (num + 2) : (num + (2 + GoodsList5.GetSerializedSize())));
		num = ((GoodsList6 == null) ? (num + 2) : (num + (2 + GoodsList6.GetSerializedSize())));
		num = ((BuyInGoodsList == null) ? (num + 2) : (num + (2 + BuyInGoodsList.GetSerializedSize())));
		num = ((BuyInGoodsList == null) ? (num + 2) : (num + (2 + BuyInGoodsList.GetSerializedSize())));
		num = ((BuyInPrice == null) ? (num + 2) : (num + (2 + (ItemKey.Invalid.GetSerializedSize() + 4) * BuyInPrice.Count)));
		num = ((PriceChangeData == null) ? (num + 2) : (num + (2 + (ItemKey.Invalid.GetSerializedSize() + 4) * PriceChangeData.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = CharId;
		ptr += 4;
		*ptr = (byte)MerchantTemplateId;
		ptr++;
		*(int*)ptr = Money;
		ptr += 4;
		*ptr = (byte)GoodsKeepTime;
		ptr++;
		if (GoodsList0 != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = GoodsList0.Serialize(ptr);
			ptr += num;
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (GoodsList1 != null)
		{
			byte* intPtr2 = ptr;
			ptr += 2;
			int num2 = GoodsList1.Serialize(ptr);
			ptr += num2;
			*(ushort*)intPtr2 = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (GoodsList2 != null)
		{
			byte* intPtr3 = ptr;
			ptr += 2;
			int num3 = GoodsList2.Serialize(ptr);
			ptr += num3;
			*(ushort*)intPtr3 = (ushort)num3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (GoodsList3 != null)
		{
			byte* intPtr4 = ptr;
			ptr += 2;
			int num4 = GoodsList3.Serialize(ptr);
			ptr += num4;
			*(ushort*)intPtr4 = (ushort)num4;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (GoodsList4 != null)
		{
			byte* intPtr5 = ptr;
			ptr += 2;
			int num5 = GoodsList4.Serialize(ptr);
			ptr += num5;
			*(ushort*)intPtr5 = (ushort)num5;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (GoodsList5 != null)
		{
			byte* intPtr6 = ptr;
			ptr += 2;
			int num6 = GoodsList5.Serialize(ptr);
			ptr += num6;
			*(ushort*)intPtr6 = (ushort)num6;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (GoodsList6 != null)
		{
			byte* intPtr7 = ptr;
			ptr += 2;
			int num7 = GoodsList6.Serialize(ptr);
			ptr += num7;
			*(ushort*)intPtr7 = (ushort)num7;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (BuyInGoodsList != null)
		{
			byte* intPtr8 = ptr;
			ptr += 2;
			int num8 = BuyInGoodsList.Serialize(ptr);
			ptr += num8;
			*(ushort*)intPtr8 = (ushort)num8;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (BuyInGoodsList != null)
		{
			byte* intPtr9 = ptr;
			ptr += 2;
			int num9 = BuyInGoodsList.Serialize(ptr);
			ptr += num9;
			*(ushort*)intPtr9 = (ushort)num9;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (BuyInPrice != null)
		{
			*(ushort*)ptr = (ushort)BuyInPrice.Count;
			ptr += 2;
			foreach (KeyValuePair<ItemKey, int> item in BuyInPrice)
			{
				ptr += item.Key.Serialize(ptr);
				*(int*)ptr = item.Value;
				ptr += 4;
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (PriceChangeData != null)
		{
			*(ushort*)ptr = (ushort)PriceChangeData.Count;
			ptr += 2;
			foreach (KeyValuePair<ItemKey, int> priceChangeDatum in PriceChangeData)
			{
				ptr += priceChangeDatum.Key.Serialize(ptr);
				*(int*)ptr = priceChangeDatum.Value;
				ptr += 4;
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num10 = (int)(ptr - pData);
		if (num10 > 4)
		{
			return (num10 + 3) / 4 * 4;
		}
		return num10;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		CharId = *(int*)ptr;
		ptr += 4;
		MerchantTemplateId = (sbyte)(*ptr);
		ptr++;
		Money = *(int*)ptr;
		ptr += 4;
		GoodsKeepTime = (sbyte)(*ptr);
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (GoodsList0 == null)
			{
				GoodsList0 = new Inventory();
			}
			ptr += GoodsList0.Deserialize(ptr);
		}
		else
		{
			GoodsList0 = null;
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (GoodsList1 == null)
			{
				GoodsList1 = new Inventory();
			}
			ptr += GoodsList1.Deserialize(ptr);
		}
		else
		{
			GoodsList1 = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (GoodsList2 == null)
			{
				GoodsList2 = new Inventory();
			}
			ptr += GoodsList2.Deserialize(ptr);
		}
		else
		{
			GoodsList2 = null;
		}
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (GoodsList3 == null)
			{
				GoodsList3 = new Inventory();
			}
			ptr += GoodsList3.Deserialize(ptr);
		}
		else
		{
			GoodsList3 = null;
		}
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (GoodsList4 == null)
			{
				GoodsList4 = new Inventory();
			}
			ptr += GoodsList4.Deserialize(ptr);
		}
		else
		{
			GoodsList4 = null;
		}
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		if (num6 > 0)
		{
			if (GoodsList5 == null)
			{
				GoodsList5 = new Inventory();
			}
			ptr += GoodsList5.Deserialize(ptr);
		}
		else
		{
			GoodsList5 = null;
		}
		ushort num7 = *(ushort*)ptr;
		ptr += 2;
		if (num7 > 0)
		{
			if (GoodsList6 == null)
			{
				GoodsList6 = new Inventory();
			}
			ptr += GoodsList6.Deserialize(ptr);
		}
		else
		{
			GoodsList6 = null;
		}
		ushort num8 = *(ushort*)ptr;
		ptr += 2;
		if (num8 > 0)
		{
			if (BuyInGoodsList == null)
			{
				BuyInGoodsList = new Inventory();
			}
			ptr += BuyInGoodsList.Deserialize(ptr);
		}
		else
		{
			BuyInGoodsList = null;
		}
		ushort num9 = *(ushort*)ptr;
		ptr += 2;
		if (num9 > 0)
		{
			if (BuyInGoodsList == null)
			{
				BuyInGoodsList = new Inventory();
			}
			ptr += BuyInGoodsList.Deserialize(ptr);
		}
		else
		{
			BuyInGoodsList = null;
		}
		ushort num10 = *(ushort*)ptr;
		ptr += 2;
		BuyInPrice.Clear();
		if (num10 > 0)
		{
			for (int i = 0; i < num10; i++)
			{
				ItemKey key = default(ItemKey);
				ptr += key.Deserialize(ptr);
				int value = *(int*)ptr;
				ptr += 4;
				BuyInPrice.Add(key, value);
			}
		}
		ushort num11 = *(ushort*)ptr;
		ptr += 2;
		PriceChangeData.Clear();
		if (num11 > 0)
		{
			for (int j = 0; j < num11; j++)
			{
				ItemKey key2 = default(ItemKey);
				ptr += key2.Deserialize(ptr);
				int value2 = *(int*)ptr;
				ptr += 4;
				PriceChangeData.Add(key2, value2);
			}
		}
		int num12 = (int)(ptr - pData);
		if (num12 > 4)
		{
			return (num12 + 3) / 4 * 4;
		}
		return num12;
	}

	public static IList<PresetItemTemplateIdGroup> GetGoodsPreset(MerchantItem template, int index)
	{
		return index switch
		{
			0 => template.Goods0, 
			1 => template.Goods1, 
			2 => template.Goods2, 
			3 => template.Goods3, 
			4 => template.Goods4, 
			5 => template.Goods5, 
			6 => template.Goods6, 
			7 => template.Goods7, 
			8 => template.Goods8, 
			9 => template.Goods9, 
			10 => template.Goods10, 
			11 => template.Goods11, 
			12 => template.Goods12, 
			13 => template.Goods13, 
			_ => throw new IndexOutOfRangeException(), 
		};
	}

	public int GetPriceChangePercent(ItemKey itemKey)
	{
		if (!PriceChangeData.TryGetValue(itemKey, out var value))
		{
			return 0;
		}
		return value;
	}

	public static sbyte FindMerchantTemplateId(sbyte type, sbyte level)
	{
		for (int i = 0; i < Config.Merchant.Instance.Count; i++)
		{
			MerchantItem merchantItem = Config.Merchant.Instance[i];
			if (merchantItem.MerchantType == type && merchantItem.Level == level)
			{
				return merchantItem.TemplateId;
			}
		}
		return -1;
	}

	public static int GetItemSoldPrice(int srcPrice, int favorChangeRate, short itemSubType, short merchantLoveItemType, short merchantHateItemType, sbyte merchantBehaviorType, int durabilityRate)
	{
		int num = 0;
		if (itemSubType == merchantLoveItemType)
		{
			num = merchantBehaviorType switch
			{
				0 => 10, 
				1 => 5, 
				2 => 0, 
				3 => 5, 
				4 => 10, 
				_ => num, 
			};
		}
		else if (itemSubType == merchantHateItemType)
		{
			num = merchantBehaviorType switch
			{
				0 => -20, 
				1 => -10, 
				2 => 0, 
				3 => -10, 
				4 => -20, 
				_ => num, 
			};
		}
		int num2 = 50 + durabilityRate / 2;
		return Math.Max(0, srcPrice * (20 + 20 * favorChangeRate / 100 + num) / 100 * num2 / 100);
	}

	public Inventory GetGoodsList(int index)
	{
		switch (index)
		{
		case 0:
			if (GoodsList0 == null)
			{
				return GoodsList0 = new Inventory();
			}
			return GoodsList0;
		case 1:
			if (GoodsList1 == null)
			{
				return GoodsList1 = new Inventory();
			}
			return GoodsList1;
		case 2:
			if (GoodsList2 == null)
			{
				return GoodsList2 = new Inventory();
			}
			return GoodsList2;
		case 3:
			if (GoodsList3 == null)
			{
				return GoodsList3 = new Inventory();
			}
			return GoodsList3;
		case 4:
			if (GoodsList4 == null)
			{
				return GoodsList4 = new Inventory();
			}
			return GoodsList4;
		case 5:
			if (GoodsList5 == null)
			{
				return GoodsList5 = new Inventory();
			}
			return GoodsList5;
		case 6:
			if (GoodsList6 == null)
			{
				return GoodsList6 = new Inventory();
			}
			return GoodsList6;
		default:
			throw new IndexOutOfRangeException();
		}
	}
}
