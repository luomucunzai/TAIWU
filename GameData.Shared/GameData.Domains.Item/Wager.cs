using System;
using System.Numerics;
using GameData.Serializer;

namespace GameData.Domains.Item;

public struct Wager : ISerializableGameData
{
	public static readonly sbyte[] ResourceRandomWeight = new sbyte[8] { 1, 1, 1, 1, 1, 1, 6, 1 };

	public const sbyte ItemRandomWeight = 3;

	public static readonly Vector2[] BehaviorValueRange = new Vector2[5]
	{
		new Vector2(0.9f, 1.1f),
		new Vector2(1f, 1.2f),
		new Vector2(0.8f, 1f),
		new Vector2(0.6f, 1.3f),
		new Vector2(0.7f, 0.9f)
	};

	public sbyte Type;

	public sbyte WagerResourceType;

	public ItemKey ItemKey;

	public int CharId;

	public int Count;

	public static readonly Wager Invalid = new Wager
	{
		Type = -1,
		WagerResourceType = -1,
		ItemKey = ItemKey.Invalid,
		CharId = -1,
		Count = 0
	};

	public sbyte Grade => CalcWagerGrade();

	public static Wager CreateResource(sbyte resourceType, int count)
	{
		return new Wager
		{
			Type = 0,
			WagerResourceType = resourceType,
			ItemKey = ItemKey.Invalid,
			CharId = -1,
			Count = count
		};
	}

	public static Wager CreateItem(ItemKey itemKey, int count)
	{
		return new Wager
		{
			Type = 1,
			WagerResourceType = -1,
			ItemKey = itemKey,
			CharId = -1,
			Count = count
		};
	}

	public static Wager CreateChar(int charId)
	{
		return new Wager
		{
			Type = 2,
			WagerResourceType = -1,
			ItemKey = ItemKey.Invalid,
			CharId = charId,
			Count = -1
		};
	}

	public static Wager CreateExp(int count)
	{
		return new Wager
		{
			Type = 3,
			WagerResourceType = -1,
			ItemKey = ItemKey.Invalid,
			CharId = -1,
			Count = count
		};
	}

	public long CalcWagerValue(int itemPrice = 0, sbyte fame = 0, short attraction = 0, short physiologicalAge = 0, sbyte displayGender = -1, sbyte charGrade = 0)
	{
		switch (Type)
		{
		case 0:
			return CricketSpecialConstants.ResourceToPrice(WagerResourceType, Count);
		case 1:
			return (long)itemPrice * (long)Count;
		case 2:
		{
			long num = 1000L;
			num += fame * 25;
			attraction /= 100;
			num += attraction * attraction * 50;
			num = num * (100 + charGrade * 10) / 100;
			if (displayGender == 0)
			{
				num = num * 150 / 100;
			}
			return num * Math.Max(0, 120 - physiologicalAge) / 100;
		}
		case 3:
			return CricketSpecialConstants.ExpToPrice(Count);
		default:
			throw new Exception($"Invalid wager type {Type}");
		}
	}

	private sbyte CalcWagerGrade()
	{
		return Type switch
		{
			1 => ItemTemplateHelper.GetGrade(ItemKey.ItemType, ItemKey.TemplateId), 
			0 => CricketSpecialConstants.ResourceToPriceGrade(WagerResourceType, Count), 
			3 => CricketSpecialConstants.ExpToPriceGrade(Count), 
			_ => -1, 
		};
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = ItemKey.GetSerializedSize() + 10;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)Type;
		ptr++;
		*ptr = (byte)WagerResourceType;
		ptr++;
		ptr += ItemKey.Serialize(ptr);
		*(int*)ptr = CharId;
		ptr += 4;
		*(int*)ptr = Count;
		ptr += 4;
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
		Type = (sbyte)(*ptr);
		ptr++;
		WagerResourceType = (sbyte)(*ptr);
		ptr++;
		ptr += ItemKey.Deserialize(ptr);
		CharId = *(int*)ptr;
		ptr += 4;
		Count = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
