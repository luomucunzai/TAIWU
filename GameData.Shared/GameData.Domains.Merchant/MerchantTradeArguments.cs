using System.Collections.Generic;
using GameData.Domains.Building;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Merchant;

[SerializableGameData(NoCopyConstructors = true, NotRestrictCollectionSerializedSize = true)]
public class MerchantTradeArguments : ISerializableGameData
{
	[SerializableGameDataField]
	public Dictionary<ItemKey, int> TradeMoneySources;

	[SerializableGameDataField]
	public int BuyMoney;

	[SerializableGameDataField]
	public List<ItemSourceChange> ItemChangeList;

	[SerializableGameDataField]
	public MerchantOverFavorData OverFavorData;

	[SerializableGameDataField]
	public OpenShopEventArguments OpenShopEventArguments;

	[SerializableGameDataField]
	public MerchantData MerchantData;

	[SerializableGameDataField]
	public MerchantBuyBackData MerchantBuyBackData;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 16;
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, int>(TradeMoneySources);
		if (ItemChangeList != null)
		{
			num += 2;
			int count = ItemChangeList.Count;
			for (int i = 0; i < count; i++)
			{
				ItemSourceChange itemSourceChange = ItemChangeList[i];
				num = ((itemSourceChange == null) ? (num + 2) : (num + (2 + itemSourceChange.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		num = ((OverFavorData == null) ? (num + 2) : (num + (2 + OverFavorData.GetSerializedSize())));
		num = ((MerchantData == null) ? (num + 2) : (num + (2 + MerchantData.GetSerializedSize())));
		num = ((MerchantBuyBackData == null) ? (num + 2) : (num + (2 + MerchantBuyBackData.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, int>(ptr, ref TradeMoneySources);
		*(int*)ptr = BuyMoney;
		ptr += 4;
		if (ItemChangeList != null)
		{
			int count = ItemChangeList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ItemSourceChange itemSourceChange = ItemChangeList[i];
				if (itemSourceChange != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = itemSourceChange.Serialize(ptr);
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
		if (OverFavorData != null)
		{
			byte* intPtr2 = ptr;
			ptr += 2;
			int num2 = OverFavorData.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr2 = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += OpenShopEventArguments.Serialize(ptr);
		if (MerchantData != null)
		{
			byte* intPtr3 = ptr;
			ptr += 2;
			int num3 = MerchantData.Serialize(ptr);
			ptr += num3;
			Tester.Assert(num3 <= 65535);
			*(ushort*)intPtr3 = (ushort)num3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (MerchantBuyBackData != null)
		{
			byte* intPtr4 = ptr;
			ptr += 2;
			int num4 = MerchantBuyBackData.Serialize(ptr);
			ptr += num4;
			Tester.Assert(num4 <= 65535);
			*(ushort*)intPtr4 = (ushort)num4;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, int>(ptr, ref TradeMoneySources);
		BuyMoney = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (ItemChangeList == null)
			{
				ItemChangeList = new List<ItemSourceChange>(num);
			}
			else
			{
				ItemChangeList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				if (num2 > 0)
				{
					ItemSourceChange itemSourceChange = new ItemSourceChange();
					ptr += itemSourceChange.Deserialize(ptr);
					ItemChangeList.Add(itemSourceChange);
				}
				else
				{
					ItemChangeList.Add(null);
				}
			}
		}
		else
		{
			ItemChangeList?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (OverFavorData == null)
			{
				OverFavorData = new MerchantOverFavorData();
			}
			ptr += OverFavorData.Deserialize(ptr);
		}
		else
		{
			OverFavorData = null;
		}
		if (OpenShopEventArguments == null)
		{
			OpenShopEventArguments = new OpenShopEventArguments();
		}
		ptr += OpenShopEventArguments.Deserialize(ptr);
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (MerchantData == null)
			{
				MerchantData = new MerchantData();
			}
			ptr += MerchantData.Deserialize(ptr);
		}
		else
		{
			MerchantData = null;
		}
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (MerchantBuyBackData == null)
			{
				MerchantBuyBackData = new MerchantBuyBackData();
			}
			ptr += MerchantBuyBackData.Deserialize(ptr);
		}
		else
		{
			MerchantBuyBackData = null;
		}
		int num6 = (int)(ptr - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}
}
