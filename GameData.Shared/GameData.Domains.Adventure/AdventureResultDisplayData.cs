using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Domains.Item.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Adventure;

public class AdventureResultDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public short ConfigId;

	[SerializableGameDataField]
	public bool IsFinished;

	[SerializableGameDataField]
	public int OriginExp;

	[SerializableGameDataField]
	public int ChangedExp;

	[SerializableGameDataField]
	public int OriginSpiritualDebt;

	[SerializableGameDataField]
	public int ChangedSpiritualDebt;

	[SerializableGameDataField]
	public List<ItemDisplayData> ItemList = new List<ItemDisplayData>();

	[SerializableGameDataField]
	public List<CharacterDisplayData> CharList = new List<CharacterDisplayData>();

	[SerializableGameDataField]
	public ResourceInts OriginResources;

	[SerializableGameDataField]
	public ResourceInts ChangedResources;

	public void Clear()
	{
		ConfigId = 0;
		IsFinished = false;
		OriginExp = 0;
		ChangedExp = 0;
		OriginSpiritualDebt = 0;
		ChangedSpiritualDebt = 0;
		ItemList.Clear();
		CharList.Clear();
		OriginResources = default(ResourceInts);
		ChangedResources = default(ResourceInts);
	}

	public void SetOrigin(short configId, int originExp, int originSpiritualDebt, ResourceInts originResources)
	{
		ConfigId = configId;
		OriginExp = originExp;
		OriginSpiritualDebt = originSpiritualDebt;
		OriginResources = originResources;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 19;
		if (ItemList != null)
		{
			num += 2;
			int count = ItemList.Count;
			for (int i = 0; i < count; i++)
			{
				ItemDisplayData itemDisplayData = ItemList[i];
				num = ((itemDisplayData == null) ? (num + 2) : (num + (2 + itemDisplayData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (CharList != null)
		{
			num += 2;
			int count2 = CharList.Count;
			for (int j = 0; j < count2; j++)
			{
				CharacterDisplayData characterDisplayData = CharList[j];
				num = ((characterDisplayData == null) ? (num + 2) : (num + (2 + characterDisplayData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		num += OriginResources.GetSerializedSize();
		num += ChangedResources.GetSerializedSize();
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = ConfigId;
		ptr += 2;
		*ptr = (IsFinished ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = OriginExp;
		ptr += 4;
		*(int*)ptr = ChangedExp;
		ptr += 4;
		*(int*)ptr = OriginSpiritualDebt;
		ptr += 4;
		*(int*)ptr = ChangedSpiritualDebt;
		ptr += 4;
		if (ItemList != null)
		{
			int count = ItemList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ItemDisplayData itemDisplayData = ItemList[i];
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
		if (CharList != null)
		{
			int count2 = CharList.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				CharacterDisplayData characterDisplayData = CharList[j];
				if (characterDisplayData != null)
				{
					byte* intPtr2 = ptr;
					ptr += 2;
					int num2 = characterDisplayData.Serialize(ptr);
					ptr += num2;
					Tester.Assert(num2 <= 65535);
					*(ushort*)intPtr2 = (ushort)num2;
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
		ptr += OriginResources.Serialize(ptr);
		ptr += ChangedResources.Serialize(ptr);
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
		ConfigId = *(short*)ptr;
		ptr += 2;
		IsFinished = *ptr != 0;
		ptr++;
		OriginExp = *(int*)ptr;
		ptr += 4;
		ChangedExp = *(int*)ptr;
		ptr += 4;
		OriginSpiritualDebt = *(int*)ptr;
		ptr += 4;
		ChangedSpiritualDebt = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (ItemList == null)
			{
				ItemList = new List<ItemDisplayData>(num);
			}
			else
			{
				ItemList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				if (num2 > 0)
				{
					ItemDisplayData itemDisplayData = new ItemDisplayData();
					ptr += itemDisplayData.Deserialize(ptr);
					ItemList.Add(itemDisplayData);
				}
				else
				{
					ItemList.Add(null);
				}
			}
		}
		else
		{
			ItemList?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (CharList == null)
			{
				CharList = new List<CharacterDisplayData>(num3);
			}
			else
			{
				CharList.Clear();
			}
			for (int j = 0; j < num3; j++)
			{
				ushort num4 = *(ushort*)ptr;
				ptr += 2;
				if (num4 > 0)
				{
					CharacterDisplayData characterDisplayData = new CharacterDisplayData();
					ptr += characterDisplayData.Deserialize(ptr);
					CharList.Add(characterDisplayData);
				}
				else
				{
					CharList.Add(null);
				}
			}
		}
		else
		{
			CharList?.Clear();
		}
		ptr += OriginResources.Deserialize(ptr);
		ptr += ChangedResources.Deserialize(ptr);
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}
}
