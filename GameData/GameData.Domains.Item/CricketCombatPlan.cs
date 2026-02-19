using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Item;

[SerializableGameData(IsExtensible = true, NotForDisplayModule = true)]
public class CricketCombatPlan : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Crickets = 0;

		public const ushort Count = 1;

		public static readonly string[] FieldId2FieldName = new string[1] { "Crickets" };
	}

	[SerializableGameDataField]
	public List<ItemKey> Crickets;

	public CricketCombatPlan()
	{
	}

	public CricketCombatPlan(CricketCombatPlan other)
	{
		Crickets = ((other.Crickets == null) ? null : new List<ItemKey>(other.Crickets));
	}

	public void Assign(CricketCombatPlan other)
	{
		Crickets = ((other.Crickets == null) ? null : new List<ItemKey>(other.Crickets));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num = ((Crickets == null) ? (num + 2) : (num + (2 + 8 * Crickets.Count)));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 1;
		ptr += 2;
		if (Crickets != null)
		{
			int count = Crickets.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += Crickets[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (Crickets == null)
				{
					Crickets = new List<ItemKey>(num2);
				}
				else
				{
					Crickets.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					ItemKey item = default(ItemKey);
					ptr += item.Deserialize(ptr);
					Crickets.Add(item);
				}
			}
			else
			{
				Crickets?.Clear();
			}
		}
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}
}
