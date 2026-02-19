using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Merchant;

[SerializableGameData(NoCopyConstructors = true, NotRestrictCollectionSerializedSize = true)]
public class MerchantBuyBackData : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte MerchantType = -1;

	[SerializableGameDataField]
	public Inventory BuyInGoodsList = new Inventory();

	[SerializableGameDataField]
	public Dictionary<ItemKey, int> BuyInPrice = new Dictionary<ItemKey, int>();

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 1;
		num = ((BuyInGoodsList == null) ? (num + 2) : (num + (2 + BuyInGoodsList.GetSerializedSize())));
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, int>(BuyInPrice);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)MerchantType;
		ptr++;
		if (BuyInGoodsList != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = BuyInGoodsList.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, int>(ptr, ref BuyInPrice);
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
		MerchantType = (sbyte)(*ptr);
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
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
		ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, int>(ptr, ref BuyInPrice);
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
