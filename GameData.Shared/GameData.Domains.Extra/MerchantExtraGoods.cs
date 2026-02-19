using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Extra;

[SerializableGameData]
[Obsolete("use new archive data MerchantExtraGoodsData instead, do not delete this")]
public class MerchantExtraGoods : ISerializableGameData
{
	[SerializableGameDataField]
	public List<MerchantExtraGoodsItem> Items = new List<MerchantExtraGoodsItem>();

	public bool Check(int id, int index)
	{
		return Items?.Exists((MerchantExtraGoodsItem d) => d.Id == id && d.Index == index) ?? false;
	}

	public MerchantExtraGoods()
	{
	}

	public MerchantExtraGoods(MerchantExtraGoods other)
	{
		Items = new List<MerchantExtraGoodsItem>(other.Items);
	}

	public void Assign(MerchantExtraGoods other)
	{
		Items = new List<MerchantExtraGoodsItem>(other.Items);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((Items == null) ? (num + 2) : (num + (2 + 8 * Items.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (Items != null)
		{
			int count = Items.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += Items[i].Serialize(ptr);
			}
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Items == null)
			{
				Items = new List<MerchantExtraGoodsItem>(num);
			}
			else
			{
				Items.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				MerchantExtraGoodsItem item = default(MerchantExtraGoodsItem);
				ptr += item.Deserialize(ptr);
				Items.Add(item);
			}
		}
		else
		{
			Items?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
