using System;
using GameData.Domains.Character;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu;

[Obsolete]
[SerializableGameData(NoCopyConstructors = true)]
public class TaiwuVillageStorage : ISerializableGameData
{
	[SerializableGameDataField]
	public ResourceInts Resources;

	[SerializableGameDataField(SubDataMaxCount = int.MaxValue)]
	public Inventory[] Inventories;

	public bool NeedCommit;

	public TaiwuVillageStorage()
	{
		Resources.Initialize();
		Inventories = Array.Empty<Inventory>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 32;
		if (Inventories != null)
		{
			num += 2;
			int num2 = Inventories.Length;
			for (int i = 0; i < num2; i++)
			{
				Inventory inventory = Inventories[i];
				num = ((inventory == null) ? (num + 4) : (num + (4 + inventory.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += Resources.Serialize(ptr);
		if (Inventories != null)
		{
			int num = Inventories.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				Inventory inventory = Inventories[i];
				if (inventory != null)
				{
					byte* intPtr = ptr;
					ptr += 4;
					int num2 = inventory.Serialize(ptr);
					ptr += num2;
					Tester.Assert(num2 <= int.MaxValue);
					*(int*)intPtr = num2;
				}
				else
				{
					*(int*)ptr = 0;
					ptr += 4;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += Resources.Deserialize(ptr);
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Inventories == null || Inventories.Length != num)
			{
				Inventories = new Inventory[num];
			}
			for (int i = 0; i < num; i++)
			{
				int num2 = *(int*)ptr;
				ptr += 4;
				if (num2 > 0)
				{
					Inventory inventory = Inventories[i] ?? new Inventory();
					ptr += inventory.Deserialize(ptr);
					Inventories[i] = inventory;
				}
				else
				{
					Inventories[i] = null;
				}
			}
		}
		else
		{
			Inventories = null;
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
