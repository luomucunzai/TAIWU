using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Building;

public class BuildingEarningsData : ISerializableGameData
{
	[SerializableGameDataField]
	public List<ItemKey> CollectionItemList;

	[SerializableGameDataField]
	public List<IntPair> CollectionResourceList;

	[SerializableGameDataField]
	public List<ItemKey> ShopSoldItemList;

	[SerializableGameDataField]
	public List<IntPair> ShopSoldItemEarnList;

	[SerializableGameDataField]
	public List<IntPair> RecruitLevelList;

	[SerializableGameDataField]
	public List<ItemKey> FixBookInfoList;

	public BuildingEarningsData()
	{
		CollectionItemList = new List<ItemKey>();
		CollectionResourceList = new List<IntPair>();
		ShopSoldItemList = new List<ItemKey>();
		ShopSoldItemEarnList = new List<IntPair>();
		RecruitLevelList = new List<IntPair>();
		FixBookInfoList = new List<ItemKey>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((CollectionItemList == null) ? (num + 2) : (num + (2 + 8 * CollectionItemList.Count)));
		num = ((CollectionResourceList == null) ? (num + 2) : (num + (2 + 8 * CollectionResourceList.Count)));
		num = ((ShopSoldItemList == null) ? (num + 2) : (num + (2 + 8 * ShopSoldItemList.Count)));
		num = ((ShopSoldItemEarnList == null) ? (num + 2) : (num + (2 + 8 * ShopSoldItemEarnList.Count)));
		num = ((RecruitLevelList == null) ? (num + 2) : (num + (2 + 8 * RecruitLevelList.Count)));
		num = ((FixBookInfoList == null) ? (num + 2) : (num + (2 + 8 * FixBookInfoList.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (CollectionItemList != null)
		{
			int count = CollectionItemList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += CollectionItemList[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (CollectionResourceList != null)
		{
			int count2 = CollectionResourceList.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				ptr += CollectionResourceList[j].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (ShopSoldItemList != null)
		{
			int count3 = ShopSoldItemList.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				ptr += ShopSoldItemList[k].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (ShopSoldItemEarnList != null)
		{
			int count4 = ShopSoldItemEarnList.Count;
			Tester.Assert(count4 <= 65535);
			*(ushort*)ptr = (ushort)count4;
			ptr += 2;
			for (int l = 0; l < count4; l++)
			{
				ptr += ShopSoldItemEarnList[l].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (RecruitLevelList != null)
		{
			int count5 = RecruitLevelList.Count;
			Tester.Assert(count5 <= 65535);
			*(ushort*)ptr = (ushort)count5;
			ptr += 2;
			for (int m = 0; m < count5; m++)
			{
				ptr += RecruitLevelList[m].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (FixBookInfoList != null)
		{
			int count6 = FixBookInfoList.Count;
			Tester.Assert(count6 <= 65535);
			*(ushort*)ptr = (ushort)count6;
			ptr += 2;
			for (int n = 0; n < count6; n++)
			{
				ptr += FixBookInfoList[n].Serialize(ptr);
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
			if (CollectionItemList == null)
			{
				CollectionItemList = new List<ItemKey>(num);
			}
			else
			{
				CollectionItemList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ItemKey item = default(ItemKey);
				ptr += item.Deserialize(ptr);
				CollectionItemList.Add(item);
			}
		}
		else
		{
			CollectionItemList?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (CollectionResourceList == null)
			{
				CollectionResourceList = new List<IntPair>(num2);
			}
			else
			{
				CollectionResourceList.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				IntPair item2 = default(IntPair);
				ptr += item2.Deserialize(ptr);
				CollectionResourceList.Add(item2);
			}
		}
		else
		{
			CollectionResourceList?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (ShopSoldItemList == null)
			{
				ShopSoldItemList = new List<ItemKey>(num3);
			}
			else
			{
				ShopSoldItemList.Clear();
			}
			for (int k = 0; k < num3; k++)
			{
				ItemKey item3 = default(ItemKey);
				ptr += item3.Deserialize(ptr);
				ShopSoldItemList.Add(item3);
			}
		}
		else
		{
			ShopSoldItemList?.Clear();
		}
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (ShopSoldItemEarnList == null)
			{
				ShopSoldItemEarnList = new List<IntPair>(num4);
			}
			else
			{
				ShopSoldItemEarnList.Clear();
			}
			for (int l = 0; l < num4; l++)
			{
				IntPair item4 = default(IntPair);
				ptr += item4.Deserialize(ptr);
				ShopSoldItemEarnList.Add(item4);
			}
		}
		else
		{
			ShopSoldItemEarnList?.Clear();
		}
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (RecruitLevelList == null)
			{
				RecruitLevelList = new List<IntPair>(num5);
			}
			else
			{
				RecruitLevelList.Clear();
			}
			for (int m = 0; m < num5; m++)
			{
				IntPair item5 = default(IntPair);
				ptr += item5.Deserialize(ptr);
				RecruitLevelList.Add(item5);
			}
		}
		else
		{
			RecruitLevelList?.Clear();
		}
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		if (num6 > 0)
		{
			if (FixBookInfoList == null)
			{
				FixBookInfoList = new List<ItemKey>(num6);
			}
			else
			{
				FixBookInfoList.Clear();
			}
			for (int n = 0; n < num6; n++)
			{
				ItemKey item6 = default(ItemKey);
				ptr += item6.Deserialize(ptr);
				FixBookInfoList.Add(item6);
			}
		}
		else
		{
			FixBookInfoList?.Clear();
		}
		int num7 = (int)(ptr - pData);
		if (num7 > 4)
		{
			return (num7 + 3) / 4 * 4;
		}
		return num7;
	}
}
