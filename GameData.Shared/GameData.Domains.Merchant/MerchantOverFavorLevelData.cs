using System;
using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Merchant;

[SerializableGameData(IsExtensible = true, NoCopyConstructors = true)]
public class MerchantOverFavorLevelData : ISerializableGameData, ICloneable
{
	private static class FieldIds
	{
		public const ushort BuyCount = 0;

		public const ushort Inventory = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "BuyCount", "Inventory" };
	}

	[SerializableGameDataField]
	public short BuyCount;

	[SerializableGameDataField]
	public Inventory Inventory = new Inventory();

	public object Clone()
	{
		MerchantOverFavorLevelData merchantOverFavorLevelData = new MerchantOverFavorLevelData
		{
			BuyCount = BuyCount,
			Inventory = new Inventory()
		};
		foreach (var (key, value) in Inventory.Items)
		{
			merchantOverFavorLevelData.Inventory.Items.Add(key, value);
		}
		return merchantOverFavorLevelData;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num = ((Inventory == null) ? (num + 2) : (num + (2 + Inventory.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 2;
		ptr += 2;
		*(short*)ptr = BuyCount;
		ptr += 2;
		if (Inventory != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = Inventory.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
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
			BuyCount = *(short*)ptr;
			ptr += 2;
		}
		if (num > 1)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (Inventory == null)
				{
					Inventory = new Inventory();
				}
				ptr += Inventory.Deserialize(ptr);
			}
			else
			{
				Inventory = null;
			}
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
