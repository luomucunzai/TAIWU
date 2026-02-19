using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Extra;

[SerializableGameData(NotForArchive = true)]
public struct TreasureFindResult : ISerializableGameData
{
	[SerializableGameDataField]
	public bool RequestInvalid;

	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	public ItemKeyAndDate ItemKeyAndDate;

	[SerializableGameDataField]
	public uint ItemCount;

	[SerializableGameDataField]
	public short MaterialTemplateId;

	[SerializableGameDataField]
	public sbyte ResourceType;

	[SerializableGameDataField]
	public int ResourceCount;

	[SerializableGameDataField]
	public List<ItemKey> ExtraItems;

	[SerializableGameDataField]
	private int _extraItemTypeInternal;

	public static TreasureFindResult Invalid
	{
		get
		{
			TreasureFindResult result = new TreasureFindResult();
			result.RequestInvalid = true;
			return result;
		}
	}

	public ItemKey ItemKey => ItemKeyAndDate.ItemKey;

	public bool AnyItem
	{
		get
		{
			if (ItemKey.IsValid())
			{
				return ItemCount != 0;
			}
			return false;
		}
	}

	public bool AnyMaterial => MaterialTemplateId >= 0;

	public bool AnyResource
	{
		get
		{
			if (ResourceType != -1)
			{
				return ResourceCount > 0;
			}
			return false;
		}
	}

	public bool AnyExtraItem
	{
		get
		{
			if (ExtraItems != null)
			{
				return ExtraItems.Count > 0;
			}
			return false;
		}
	}

	public ETreasureExtraItemType ExtraItemType => (ETreasureExtraItemType)_extraItemTypeInternal;

	public bool Success
	{
		get
		{
			if (!AnyItem && !AnyMaterial)
			{
				return AnyExtraItem;
			}
			return true;
		}
	}

	public TreasureFindResult()
	{
		RequestInvalid = false;
		Location = Location.Invalid;
		ItemKeyAndDate = new ItemKeyAndDate(-1, ItemKey.Invalid);
		ItemCount = 0u;
		MaterialTemplateId = -1;
		ResourceType = -1;
		ResourceCount = 0;
		ExtraItems = null;
		_extraItemTypeInternal = 0;
	}

	public void SetExtraItemType(ETreasureExtraItemType extraItemType)
	{
		_extraItemTypeInternal = (int)extraItemType;
	}

	public TreasureFindResult(TreasureFindResult other)
	{
		RequestInvalid = other.RequestInvalid;
		Location = other.Location;
		ItemKeyAndDate = other.ItemKeyAndDate;
		ItemCount = other.ItemCount;
		MaterialTemplateId = other.MaterialTemplateId;
		ResourceType = other.ResourceType;
		ResourceCount = other.ResourceCount;
		ExtraItems = ((other.ExtraItems == null) ? null : new List<ItemKey>(other.ExtraItems));
		_extraItemTypeInternal = other._extraItemTypeInternal;
	}

	public void Assign(TreasureFindResult other)
	{
		RequestInvalid = other.RequestInvalid;
		Location = other.Location;
		ItemKeyAndDate = other.ItemKeyAndDate;
		ItemCount = other.ItemCount;
		MaterialTemplateId = other.MaterialTemplateId;
		ResourceType = other.ResourceType;
		ResourceCount = other.ResourceCount;
		ExtraItems = ((other.ExtraItems == null) ? null : new List<ItemKey>(other.ExtraItems));
		_extraItemTypeInternal = other._extraItemTypeInternal;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 32;
		num = ((ExtraItems == null) ? (num + 2) : (num + (2 + 8 * ExtraItems.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (RequestInvalid ? ((byte)1) : ((byte)0));
		ptr++;
		ptr += Location.Serialize(ptr);
		ptr += ItemKeyAndDate.Serialize(ptr);
		*(uint*)ptr = ItemCount;
		ptr += 4;
		*(short*)ptr = MaterialTemplateId;
		ptr += 2;
		*ptr = (byte)ResourceType;
		ptr++;
		*(int*)ptr = ResourceCount;
		ptr += 4;
		if (ExtraItems != null)
		{
			int count = ExtraItems.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += ExtraItems[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = _extraItemTypeInternal;
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
		RequestInvalid = *ptr != 0;
		ptr++;
		ptr += Location.Deserialize(ptr);
		ptr += ItemKeyAndDate.Deserialize(ptr);
		ItemCount = *(uint*)ptr;
		ptr += 4;
		MaterialTemplateId = *(short*)ptr;
		ptr += 2;
		ResourceType = (sbyte)(*ptr);
		ptr++;
		ResourceCount = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (ExtraItems == null)
			{
				ExtraItems = new List<ItemKey>(num);
			}
			else
			{
				ExtraItems.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ItemKey item = default(ItemKey);
				ptr += item.Deserialize(ptr);
				ExtraItems.Add(item);
			}
		}
		else
		{
			ExtraItems?.Clear();
		}
		_extraItemTypeInternal = *(int*)ptr;
		ptr += 4;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
