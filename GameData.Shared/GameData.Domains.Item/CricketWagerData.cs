using System.Collections.Generic;
using GameData.Domains.Item.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Item;

[SerializableGameData(NotForArchive = true)]
public class CricketWagerData : ISerializableGameData
{
	[SerializableGameDataField]
	public Wager Wager;

	[SerializableGameDataField]
	public List<ItemDisplayData> Crickets;

	[SerializableGameDataField]
	public long MinWagerValue;

	[SerializableGameDataField]
	public byte PreRandomizedShowCricketIndex;

	public CricketWagerData()
	{
	}

	public CricketWagerData(CricketWagerData other)
	{
		Wager = other.Wager;
		if (other.Crickets != null)
		{
			List<ItemDisplayData> crickets = other.Crickets;
			int count = crickets.Count;
			Crickets = new List<ItemDisplayData>(count);
			for (int i = 0; i < count; i++)
			{
				Crickets.Add(new ItemDisplayData(crickets[i]));
			}
		}
		else
		{
			Crickets = null;
		}
		MinWagerValue = other.MinWagerValue;
		PreRandomizedShowCricketIndex = other.PreRandomizedShowCricketIndex;
	}

	public void Assign(CricketWagerData other)
	{
		Wager = other.Wager;
		if (other.Crickets != null)
		{
			List<ItemDisplayData> crickets = other.Crickets;
			int count = crickets.Count;
			Crickets = new List<ItemDisplayData>(count);
			for (int i = 0; i < count; i++)
			{
				Crickets.Add(new ItemDisplayData(crickets[i]));
			}
		}
		else
		{
			Crickets = null;
		}
		MinWagerValue = other.MinWagerValue;
		PreRandomizedShowCricketIndex = other.PreRandomizedShowCricketIndex;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 29;
		if (Crickets != null)
		{
			num += 2;
			int count = Crickets.Count;
			for (int i = 0; i < count; i++)
			{
				ItemDisplayData itemDisplayData = Crickets[i];
				num = ((itemDisplayData == null) ? (num + 2) : (num + (2 + itemDisplayData.GetSerializedSize())));
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
		ptr += Wager.Serialize(ptr);
		if (Crickets != null)
		{
			int count = Crickets.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ItemDisplayData itemDisplayData = Crickets[i];
				if (itemDisplayData != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = itemDisplayData.Serialize(ptr);
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
		*(long*)ptr = MinWagerValue;
		ptr += 8;
		*ptr = PreRandomizedShowCricketIndex;
		ptr++;
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
		ptr += Wager.Deserialize(ptr);
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Crickets == null)
			{
				Crickets = new List<ItemDisplayData>(num);
			}
			else
			{
				Crickets.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				if (num2 > 0)
				{
					ItemDisplayData itemDisplayData = new ItemDisplayData();
					ptr += itemDisplayData.Deserialize(ptr);
					Crickets.Add(itemDisplayData);
				}
				else
				{
					Crickets.Add(null);
				}
			}
		}
		else
		{
			Crickets?.Clear();
		}
		MinWagerValue = *(long*)ptr;
		ptr += 8;
		PreRandomizedShowCricketIndex = *ptr;
		ptr++;
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
