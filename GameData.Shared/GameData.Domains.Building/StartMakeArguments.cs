using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Domains.Item.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Building;

public struct StartMakeArguments : ISerializableGameData
{
	[SerializableGameDataField]
	public int CharId;

	[SerializableGameDataField]
	public BuildingBlockKey BuildingBlockKey;

	[SerializableGameDataField]
	public ItemDisplayData Tool;

	[SerializableGameDataField]
	public ItemDisplayData Material;

	[SerializableGameDataField]
	public sbyte ItemType;

	[SerializableGameDataField]
	public List<short> ItemList;

	[SerializableGameDataField]
	public short MakeItemSubTypeId;

	[SerializableGameDataField]
	public ResourceInts ResourceCount;

	[SerializableGameDataField]
	public ResourceInts NeedResource;

	[SerializableGameDataField]
	public bool IsPerfect;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 80;
		num = ((Tool == null) ? (num + 2) : (num + (2 + Tool.GetSerializedSize())));
		num = ((Material == null) ? (num + 2) : (num + (2 + Material.GetSerializedSize())));
		num = ((ItemList == null) ? (num + 2) : (num + (2 + 2 * ItemList.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = CharId;
		ptr += 4;
		ptr += BuildingBlockKey.Serialize(ptr);
		if (Tool != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = Tool.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (Material != null)
		{
			byte* intPtr2 = ptr;
			ptr += 2;
			int num2 = Material.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr2 = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)ItemType;
		ptr++;
		if (ItemList != null)
		{
			int count = ItemList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = ItemList[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(short*)ptr = MakeItemSubTypeId;
		ptr += 2;
		ptr += ResourceCount.Serialize(ptr);
		ptr += NeedResource.Serialize(ptr);
		*ptr = (IsPerfect ? ((byte)1) : ((byte)0));
		ptr++;
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
		CharId = *(int*)ptr;
		ptr += 4;
		ptr += BuildingBlockKey.Deserialize(ptr);
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Tool == null)
			{
				Tool = new ItemDisplayData();
			}
			ptr += Tool.Deserialize(ptr);
		}
		else
		{
			Tool = null;
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (Material == null)
			{
				Material = new ItemDisplayData();
			}
			ptr += Material.Deserialize(ptr);
		}
		else
		{
			Material = null;
		}
		ItemType = (sbyte)(*ptr);
		ptr++;
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (ItemList == null)
			{
				ItemList = new List<short>(num3);
			}
			else
			{
				ItemList.Clear();
			}
			for (int i = 0; i < num3; i++)
			{
				ItemList.Add(((short*)ptr)[i]);
			}
			ptr += 2 * num3;
		}
		else
		{
			ItemList?.Clear();
		}
		MakeItemSubTypeId = *(short*)ptr;
		ptr += 2;
		ptr += ResourceCount.Deserialize(ptr);
		ptr += NeedResource.Deserialize(ptr);
		IsPerfect = *ptr != 0;
		ptr++;
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
