using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Building;

public class MakeItemData : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte ProductItemType;

	[SerializableGameDataField]
	public List<short> ProductItemIdList;

	[SerializableGameDataField]
	public short LeftTime;

	[SerializableGameDataField]
	public MaterialResources MaterialResources;

	[SerializableGameDataField]
	public ItemKey ToolKey;

	[SerializableGameDataField]
	public ItemKey MaterialKey;

	[SerializableGameDataField]
	public bool IsPerfect;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 32;
		num = ((ProductItemIdList == null) ? (num + 2) : (num + (2 + 2 * ProductItemIdList.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)ProductItemType;
		ptr++;
		if (ProductItemIdList != null)
		{
			int count = ProductItemIdList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = ProductItemIdList[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(short*)ptr = LeftTime;
		ptr += 2;
		ptr += MaterialResources.Serialize(ptr);
		ptr += ToolKey.Serialize(ptr);
		ptr += MaterialKey.Serialize(ptr);
		*ptr = (IsPerfect ? ((byte)1) : ((byte)0));
		ptr++;
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
		ProductItemType = (sbyte)(*ptr);
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (ProductItemIdList == null)
			{
				ProductItemIdList = new List<short>(num);
			}
			else
			{
				ProductItemIdList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ProductItemIdList.Add(((short*)ptr)[i]);
			}
			ptr += 2 * num;
		}
		else
		{
			ProductItemIdList?.Clear();
		}
		LeftTime = *(short*)ptr;
		ptr += 2;
		ptr += MaterialResources.Deserialize(ptr);
		ptr += ToolKey.Deserialize(ptr);
		ptr += MaterialKey.Deserialize(ptr);
		IsPerfect = *ptr != 0;
		ptr++;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
