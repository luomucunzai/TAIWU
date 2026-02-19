using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Adventure;
using GameData.Domains.Item;
using GameData.Domains.World;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Map;

public class MapBlockData : ISerializableGameData
{
	[SerializableGameDataField]
	public short AreaId;

	[SerializableGameDataField]
	public short BlockId;

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public short BelongBlockId;

	[SerializableGameDataField]
	public short RootBlockId;

	[SerializableGameDataField]
	public bool Visible;

	[SerializableGameDataField]
	public HashSet<int> CharacterSet;

	[SerializableGameDataField]
	public HashSet<int> InfectedCharacterSet;

	[SerializableGameDataField]
	public HashSet<int> FixedCharacterSet;

	[SerializableGameDataField]
	public HashSet<int> GraveSet;

	[SerializableGameDataField]
	public List<MapTemplateEnemyInfo> TemplateEnemyList;

	[SerializableGameDataField]
	public HashSet<int> EnemyCharacterSet;

	[SerializableGameDataField]
	public short Malice;

	[SerializableGameDataField]
	public bool Destroyed;

	[SerializableGameDataField]
	public MaterialResources MaxResources;

	[SerializableGameDataField]
	public MaterialResources CurrResources;

	[SerializableGameDataField]
	public SortedList<ItemKeyAndDate, int> Items;

	private static readonly LocalObjectPool<HashSet<int>> IntHashSetPool = new LocalObjectPool<HashSet<int>>(6144, 30720);

	private static readonly LocalObjectPool<List<MapTemplateEnemyInfo>> RandomEnemyListPool = new LocalObjectPool<List<MapTemplateEnemyInfo>>(3072, 15360);

	private static readonly ObjectPool<SortedList<ItemKeyAndDate, int>> ItemCollectionPool = new ObjectPool<SortedList<ItemKeyAndDate, int>>(3072, 15360);

	public List<MapBlockData> GroupBlockList;

	public const int MaxBlockItemCount = 500;

	public bool ShowDestroyed
	{
		get
		{
			if (Destroyed)
			{
				return !GetConfig().IgnoreDestroyed;
			}
			return false;
		}
	}

	public sbyte MoveCost => GetConfig()?.MoveCost ?? (-1);

	public EMapBlockType BlockType => GetConfig()?.Type ?? EMapBlockType.Invalid;

	public EMapBlockSubType BlockSubType => GetConfig()?.SubType ?? EMapBlockSubType.Invalid;

	public MapBlockData(short areaId, short blockId, short templateId)
	{
		AreaId = areaId;
		BlockId = blockId;
		TemplateId = templateId;
		BelongBlockId = -1;
		RootBlockId = -1;
		Visible = false;
	}

	public static MapBlockData SimpleClone(MapBlockData other)
	{
		return new MapBlockData
		{
			AreaId = other.AreaId,
			BlockId = other.BlockId,
			TemplateId = other.TemplateId,
			BelongBlockId = other.BelongBlockId,
			RootBlockId = other.RootBlockId,
			Visible = other.Visible
		};
	}

	public MapBlockData()
	{
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public unsafe int GetSerializedSize()
	{
		int num = 38;
		num = ((CharacterSet == null) ? (num + 2) : (num + (2 + 4 * CharacterSet.Count)));
		num = ((InfectedCharacterSet == null) ? (num + 2) : (num + (2 + 4 * InfectedCharacterSet.Count)));
		num = ((FixedCharacterSet == null) ? (num + 2) : (num + (2 + 4 * FixedCharacterSet.Count)));
		num = ((GraveSet == null) ? (num + 2) : (num + (2 + 4 * GraveSet.Count)));
		num = ((TemplateEnemyList == null) ? (num + 2) : (num + (2 + 8 * TemplateEnemyList.Count)));
		num = ((EnemyCharacterSet == null) ? (num + 2) : (num + (2 + 4 * EnemyCharacterSet.Count)));
		num = ((Items == null) ? (num + 2) : (num + (2 + (4 + sizeof(ItemKey) + 4) * Items.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = AreaId;
		ptr += 2;
		*(short*)ptr = BlockId;
		ptr += 2;
		*(short*)ptr = TemplateId;
		ptr += 2;
		*(short*)ptr = BelongBlockId;
		ptr += 2;
		*(short*)ptr = RootBlockId;
		ptr += 2;
		*ptr = (Visible ? ((byte)1) : ((byte)0));
		ptr++;
		if (CharacterSet != null)
		{
			int count = CharacterSet.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			foreach (int item in CharacterSet)
			{
				*(int*)ptr = item;
				ptr += 4;
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (InfectedCharacterSet != null)
		{
			int count2 = InfectedCharacterSet.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			foreach (int item2 in InfectedCharacterSet)
			{
				*(int*)ptr = item2;
				ptr += 4;
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (FixedCharacterSet != null)
		{
			int count3 = FixedCharacterSet.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			foreach (int item3 in FixedCharacterSet)
			{
				*(int*)ptr = item3;
				ptr += 4;
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (GraveSet != null)
		{
			int count4 = GraveSet.Count;
			Tester.Assert(count4 <= 65535);
			*(ushort*)ptr = (ushort)count4;
			ptr += 2;
			foreach (int item4 in GraveSet)
			{
				*(int*)ptr = item4;
				ptr += 4;
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (TemplateEnemyList != null)
		{
			int count5 = TemplateEnemyList.Count;
			Tester.Assert(count5 <= 65535);
			*(ushort*)ptr = (ushort)count5;
			ptr += 2;
			for (int i = 0; i < count5; i++)
			{
				ptr += TemplateEnemyList[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (EnemyCharacterSet != null)
		{
			int count6 = EnemyCharacterSet.Count;
			Tester.Assert(count6 <= 65535);
			*(ushort*)ptr = (ushort)count6;
			ptr += 2;
			foreach (int item5 in EnemyCharacterSet)
			{
				*(int*)ptr = item5;
				ptr += 4;
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(short*)ptr = Malice;
		ptr += 2;
		*ptr = (Destroyed ? ((byte)1) : ((byte)0));
		ptr++;
		ptr += MaxResources.Serialize(ptr);
		ptr += CurrResources.Serialize(ptr);
		if (Items != null)
		{
			int count7 = Items.Count;
			Tester.Assert(count7 <= 65535);
			*(ushort*)ptr = (ushort)count7;
			ptr += 2;
			IList<ItemKeyAndDate> keys = Items.Keys;
			IList<int> values = Items.Values;
			for (int j = 0; j < count7; j++)
			{
				ptr += keys[j].Serialize(ptr);
				*(int*)ptr = values[j];
				ptr += 4;
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
		AreaId = *(short*)ptr;
		ptr += 2;
		BlockId = *(short*)ptr;
		ptr += 2;
		TemplateId = *(short*)ptr;
		ptr += 2;
		BelongBlockId = *(short*)ptr;
		ptr += 2;
		RootBlockId = *(short*)ptr;
		ptr += 2;
		Visible = *ptr != 0;
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (CharacterSet == null)
			{
				CharacterSet = IntHashSetPool.Get();
			}
			else
			{
				CharacterSet.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				int item = *(int*)ptr;
				ptr += 4;
				CharacterSet.Add(item);
			}
		}
		else
		{
			CharacterSet?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (InfectedCharacterSet == null)
			{
				InfectedCharacterSet = IntHashSetPool.Get();
			}
			else
			{
				InfectedCharacterSet.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				int item2 = *(int*)ptr;
				ptr += 4;
				InfectedCharacterSet.Add(item2);
			}
		}
		else
		{
			InfectedCharacterSet?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (FixedCharacterSet == null)
			{
				FixedCharacterSet = IntHashSetPool.Get();
			}
			else
			{
				FixedCharacterSet.Clear();
			}
			for (int k = 0; k < num3; k++)
			{
				int item3 = *(int*)ptr;
				ptr += 4;
				FixedCharacterSet.Add(item3);
			}
		}
		else
		{
			FixedCharacterSet?.Clear();
		}
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (GraveSet == null)
			{
				GraveSet = IntHashSetPool.Get();
			}
			else
			{
				GraveSet.Clear();
			}
			for (int l = 0; l < num4; l++)
			{
				int item4 = *(int*)ptr;
				ptr += 4;
				GraveSet.Add(item4);
			}
		}
		else
		{
			GraveSet?.Clear();
		}
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (TemplateEnemyList == null)
			{
				TemplateEnemyList = RandomEnemyListPool.Get();
			}
			else
			{
				TemplateEnemyList.Clear();
			}
			for (int m = 0; m < num5; m++)
			{
				MapTemplateEnemyInfo item5 = default(MapTemplateEnemyInfo);
				ptr += item5.Deserialize(ptr);
				TemplateEnemyList.Add(item5);
			}
		}
		else
		{
			TemplateEnemyList?.Clear();
		}
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		if (num6 > 0)
		{
			if (EnemyCharacterSet == null)
			{
				EnemyCharacterSet = IntHashSetPool.Get();
			}
			else
			{
				EnemyCharacterSet.Clear();
			}
			for (int n = 0; n < num6; n++)
			{
				int item6 = *(int*)ptr;
				ptr += 4;
				EnemyCharacterSet.Add(item6);
			}
		}
		else
		{
			EnemyCharacterSet?.Clear();
		}
		Malice = *(short*)ptr;
		ptr += 2;
		Destroyed = *ptr != 0;
		ptr++;
		ptr += MaxResources.Deserialize(ptr);
		ptr += CurrResources.Deserialize(ptr);
		ushort num7 = *(ushort*)ptr;
		ptr += 2;
		if (num7 > 0)
		{
			if (Items == null)
			{
				Items = ItemCollectionPool.Get();
			}
			else
			{
				Items.Clear();
			}
			for (int num8 = 0; num8 < num7; num8++)
			{
				ItemKeyAndDate key = default(ItemKeyAndDate);
				ptr += key.Deserialize(ptr);
				int value = *(int*)ptr;
				ptr += 4;
				Items.Add(key, value);
			}
		}
		else
		{
			Items?.Clear();
		}
		int num9 = (int)(ptr - pData);
		if (num9 > 4)
		{
			return (num9 + 3) / 4 * 4;
		}
		return num9;
	}

	public void AddCharacter(int charId)
	{
		if (CharacterSet == null)
		{
			CharacterSet = IntHashSetPool.Get();
		}
		CharacterSet.Add(charId);
	}

	public bool RemoveCharacter(int charId)
	{
		if (CharacterSet != null && CharacterSet.Remove(charId))
		{
			if (CharacterSet.Count <= 0)
			{
				IntHashSetPool.Return(CharacterSet);
				CharacterSet = null;
			}
			return true;
		}
		return false;
	}

	public void AddInfectedCharacter(int charId)
	{
		if (InfectedCharacterSet == null)
		{
			InfectedCharacterSet = IntHashSetPool.Get();
		}
		InfectedCharacterSet.Add(charId);
	}

	public bool RemoveInfectedCharacter(int charId)
	{
		if (InfectedCharacterSet != null && InfectedCharacterSet.Remove(charId))
		{
			if (InfectedCharacterSet.Count <= 0)
			{
				IntHashSetPool.Return(InfectedCharacterSet);
				InfectedCharacterSet = null;
			}
			return true;
		}
		return false;
	}

	public void AddFixedCharacter(int charId)
	{
		if (FixedCharacterSet == null)
		{
			FixedCharacterSet = IntHashSetPool.Get();
		}
		FixedCharacterSet.Add(charId);
	}

	public bool RemoveFixedCharacter(int charId)
	{
		if (FixedCharacterSet != null && FixedCharacterSet.Remove(charId))
		{
			if (FixedCharacterSet.Count <= 0)
			{
				IntHashSetPool.Return(FixedCharacterSet);
				FixedCharacterSet = null;
			}
			return true;
		}
		return false;
	}

	public void AddEnemyCharacter(int charId)
	{
		if (EnemyCharacterSet == null)
		{
			EnemyCharacterSet = IntHashSetPool.Get();
		}
		EnemyCharacterSet.Add(charId);
	}

	public bool RemoveEnemyCharacter(int charId)
	{
		if (EnemyCharacterSet != null && EnemyCharacterSet.Remove(charId))
		{
			if (EnemyCharacterSet.Count <= 0)
			{
				IntHashSetPool.Return(EnemyCharacterSet);
				EnemyCharacterSet = null;
			}
			return true;
		}
		return false;
	}

	public void AddGrave(int charId)
	{
		if (GraveSet == null)
		{
			GraveSet = IntHashSetPool.Get();
		}
		GraveSet.Add(charId);
	}

	public bool RemoveGrave(int charId)
	{
		if (GraveSet != null && GraveSet.Remove(charId))
		{
			if (GraveSet.Count <= 0)
			{
				IntHashSetPool.Return(GraveSet);
				GraveSet = null;
			}
			return true;
		}
		return false;
	}

	public bool AnyTemplateEnemy(short templateId)
	{
		if (TemplateEnemyList == null)
		{
			return false;
		}
		foreach (MapTemplateEnemyInfo templateEnemy in TemplateEnemyList)
		{
			if (templateEnemy.TemplateId == templateId)
			{
				return true;
			}
		}
		return false;
	}

	public bool AnyTemplateEnemy(short templateIdMin, short templateIdMax)
	{
		if (TemplateEnemyList == null)
		{
			return false;
		}
		foreach (MapTemplateEnemyInfo templateEnemy in TemplateEnemyList)
		{
			if (templateEnemy.TemplateId >= templateIdMin && templateEnemy.TemplateId <= templateIdMax)
			{
				return true;
			}
		}
		return false;
	}

	public void AddTemplateEnemy(MapTemplateEnemyInfo mapTemplateEnemyInfo)
	{
		if (TemplateEnemyList == null)
		{
			TemplateEnemyList = RandomEnemyListPool.Get();
		}
		TemplateEnemyList.Add(mapTemplateEnemyInfo);
	}

	public bool RemoveTemplateEnemy(MapTemplateEnemyInfo templateEnemyInfo)
	{
		if (TemplateEnemyList != null && TemplateEnemyList.Remove(templateEnemyInfo))
		{
			if (TemplateEnemyList.Count <= 0)
			{
				RandomEnemyListPool.Return(TemplateEnemyList);
				TemplateEnemyList = null;
			}
			return true;
		}
		return false;
	}

	public bool CountDown()
	{
		if (TemplateEnemyList == null)
		{
			return false;
		}
		bool result = false;
		int count = TemplateEnemyList.Count;
		while (count-- > 0)
		{
			if (TemplateEnemyList[count].Duration > 0)
			{
				MapTemplateEnemyInfo value = TemplateEnemyList[count];
				value.Duration--;
				if (value.Duration == 0)
				{
					TemplateEnemyList.RemoveAt(count);
				}
				else
				{
					TemplateEnemyList[count] = value;
				}
				result = true;
			}
		}
		return result;
	}

	public void AddItem(ItemKey itemKey, int amount)
	{
		if (Items == null)
		{
			Items = ItemCollectionPool.Get();
		}
		int currDate = ExternalDataBridge.Context.CurrDate;
		ItemKeyAndDate key = new ItemKeyAndDate(GetDestroyedDate(itemKey, currDate), itemKey);
		if (Items.TryGetValue(key, out var value))
		{
			Items[key] = value + amount;
		}
		else
		{
			Items.Add(key, amount);
		}
	}

	public void RemoveItem(ItemKeyAndDate itemKeyAndDate)
	{
		if (Items != null)
		{
			Items.Remove(itemKeyAndDate);
			if (Items.Count == 0)
			{
				ItemCollectionPool.Return(Items);
				Items = null;
			}
		}
	}

	public void RemoveItemByCount(ItemKeyAndDate itemKeyAndDate, int count)
	{
		if (Items[itemKeyAndDate] <= count)
		{
			RemoveItem(itemKeyAndDate);
		}
		else
		{
			Items[itemKeyAndDate] -= count;
		}
	}

	public void AddItems(List<(ItemKey itemKey, int amount)> items)
	{
		if (Items == null)
		{
			Items = ItemCollectionPool.Get();
		}
		int currDate = ExternalDataBridge.Context.CurrDate;
		int i = 0;
		for (int count = items.Count; i < count; i++)
		{
			(ItemKey itemKey, int amount) tuple = items[i];
			ItemKey item = tuple.itemKey;
			int item2 = tuple.amount;
			ItemKeyAndDate key = new ItemKeyAndDate(GetDestroyedDate(item, currDate), item);
			if (Items.TryGetValue(key, out var value))
			{
				Items[key] = value + item2;
			}
			else
			{
				Items.Add(key, item2);
			}
		}
	}

	private bool DestroyItemsByDate(int currDate, List<ItemKey> destroyedUniqueItems)
	{
		if (Items == null || Items.Count <= 0)
		{
			return false;
		}
		IList<ItemKeyAndDate> keys = Items.Keys;
		int count = Items.Count;
		int num = -1;
		for (int i = 0; i < count && keys[i].Date <= currDate; i++)
		{
			num = i;
		}
		if (num < 0)
		{
			return false;
		}
		for (int j = 0; j <= num; j++)
		{
			ItemKey itemKey = keys[j].ItemKey;
			if (!ItemTemplateHelper.IsPureStackable(itemKey))
			{
				destroyedUniqueItems.Add(itemKey);
			}
		}
		if (num < count - 1)
		{
			SortedList<ItemKeyAndDate, int> sortedList = ItemCollectionPool.Get();
			IList<int> values = Items.Values;
			for (int k = num + 1; k < count; k++)
			{
				sortedList.Add(keys[k], values[k]);
			}
			Items.Clear();
			ItemCollectionPool.Return(Items);
			Items = sortedList;
		}
		else
		{
			Items.Clear();
			ItemCollectionPool.Return(Items);
			Items = null;
		}
		return true;
	}

	public bool DestroyItems(List<ItemKey> destroyedUniqueItems)
	{
		int currDate = ExternalDataBridge.Context.CurrDate;
		return DestroyItemsByDate(currDate, destroyedUniqueItems);
	}

	public bool DestroyItemsDirect(List<ItemKey> destroyedUniqueItems)
	{
		return DestroyItemsByDate(2147483646, destroyedUniqueItems);
	}

	public static int GetDestroyedDate(ItemKey itemKey, int date)
	{
		short preservationDuration = ItemTemplateHelper.GetPreservationDuration(itemKey.ItemType, itemKey.TemplateId);
		if (preservationDuration < 0)
		{
			return int.MaxValue;
		}
		return date + preservationDuration;
	}

	public MapBlockItem GetConfig()
	{
		if (RootBlockId >= 0)
		{
			return GetRootBlock()?.GetConfig();
		}
		return MapBlock.Instance[TemplateId];
	}

	public ResourceCollectionItem GetResourceCollectionConfig()
	{
		return ResourceCollection.Instance[GetConfig().ResourceCollectionType];
	}

	public Location GetLocation()
	{
		return new Location(AreaId, BlockId);
	}

	public ByteCoordinate GetBlockPos()
	{
		byte areaSize = ExternalDataBridge.Context.GetAreaSize(AreaId);
		return ByteCoordinate.IndexToCoordinate(BlockId, areaSize);
	}

	public bool IsCityTown()
	{
		if (BlockType != EMapBlockType.City && BlockType != EMapBlockType.Sect)
		{
			return BlockType == EMapBlockType.Town;
		}
		return true;
	}

	public bool IsNonDeveloped()
	{
		EMapBlockType blockType = BlockType;
		if (blockType != EMapBlockType.Normal && blockType != EMapBlockType.Wild && blockType != EMapBlockType.Bad)
		{
			return blockType == EMapBlockType.scenery;
		}
		return true;
	}

	public bool IsPassable()
	{
		return MoveCost >= 0;
	}

	public bool CanChangeBlockType()
	{
		if (TemplateId != 126 && RootBlockId == -1)
		{
			return !IsCityTown();
		}
		return false;
	}

	public bool CanCollectResource(sbyte resourceType)
	{
		if (GetConfig().ResourceCollectionType >= 0)
		{
			return MaxResources[resourceType] > 0;
		}
		return false;
	}

	public short GetMaxMalice()
	{
		return GetConfig().MaxMalice;
	}

	public int GetAnimalBaseSpawnRate()
	{
		int sum = CurrResources.GetSum();
		int sum2 = MaxResources.GetSum();
		return sum * 10 / sum2;
	}

	public MapBlockData GetRootBlock()
	{
		if (RootBlockId >= 0)
		{
			return ExternalDataBridge.Context.GetBlockData(new Location(AreaId, RootBlockId));
		}
		return this;
	}

	public byte GetManhattanDistanceToPos(byte x, byte y)
	{
		if (RootBlockId >= 0)
		{
			return GetRootBlock().GetManhattanDistanceToPos(x, y);
		}
		ByteCoordinate byteCoordinate = new ByteCoordinate(x, y);
		int num = GetBlockPos().GetManhattanDistance(byteCoordinate);
		Location rootLocation = new Location(AreaId, BlockId);
		IEnumerable<short> groupBlockIds = ExternalDataBridge.Context.GetGroupBlockIds(rootLocation, this);
		byte areaSize = ExternalDataBridge.Context.GetAreaSize(AreaId);
		foreach (short item in groupBlockIds)
		{
			int manhattanDistance = ByteCoordinate.IndexToCoordinate(item, areaSize).GetManhattanDistance(byteCoordinate);
			if (manhattanDistance < num)
			{
				num = manhattanDistance;
			}
		}
		return (byte)num;
	}

	public byte GetManhattanDistanceToPosWithoutRoot(byte x, byte y)
	{
		ByteCoordinate byteCoordinate = new ByteCoordinate(x, y);
		return (byte)GetBlockPos().GetManhattanDistance(byteCoordinate);
	}

	public unsafe int GetCollectItemChance(sbyte resourceType)
	{
		if (MaxResources.Items[resourceType] <= 0)
		{
			return 0;
		}
		return CurrResources.Items[resourceType] * 100 / MaxResources.Items[resourceType] - 25;
	}

	public unsafe int GetCollectResourceAmount(sbyte resourceType)
	{
		return CurrResources.Items[resourceType] * GlobalConfig.Instance.CollectResourcePercent / 100 * GameData.Domains.World.SharedMethods.GetGainResourcePercent(10) / 100;
	}

	public int GetBlockIndexInBigBlock(byte areaSize)
	{
		MapBlockItem config = GetConfig();
		if (config != null && config.Size > 1)
		{
			if (RootBlockId < 0)
			{
				if (config.Size != 2)
				{
					return 6;
				}
				return 2;
			}
			int num = BlockId - RootBlockId;
			if (config.Size == 2)
			{
				if (num != 1)
				{
					return (num != areaSize) ? 1 : 0;
				}
				return 3;
			}
			switch (num)
			{
			default:
				if (num != areaSize)
				{
					if (num != areaSize + 1)
					{
						if (num != areaSize + 2)
						{
							if (num != areaSize * 2)
							{
								if (num != areaSize * 2 + 1)
								{
									return 2;
								}
								return 1;
							}
							return 0;
						}
						return 5;
					}
					return 4;
				}
				return 3;
			case 2:
				return 8;
			case 1:
				return 7;
			}
		}
		return -1;
	}

	public override string ToString()
	{
		return $"MapBlockData({AreaId},{BlockId})";
	}

	public unsafe void InitResources(IRandomSource random)
	{
		MapBlockItem config = GetConfig();
		if (config == null)
		{
			return;
		}
		for (sbyte b = 0; b < 6; b++)
		{
			short num = config.Resources[b];
			if (num < 0)
			{
				num = (short)(random.Next(Math.Abs(num) / 2, Math.Abs(num) + 1) * 5);
			}
			else if (num != 0 && random.CheckPercentProb(50))
			{
				num = (short)Math.Max(random.CheckPercentProb(35) ? (num + random.Next(1, 6) * 5) : (num - random.Next(1, 6) * 5), 0);
			}
			MaxResources.Items[b] = num;
			CurrResources.Items[b] = (short)(num * random.Next(50, 75) / 100 * GameData.Domains.World.SharedMethods.GetGainResourcePercent(12) / 100);
		}
	}

	public void ChangeTemplateId(short newPresetId, bool checkCanChange = true)
	{
		if (!checkCanChange || CanChangeBlockType())
		{
			TemplateId = newPresetId;
			return;
		}
		throw new Exception($"{BlockSubType} can not change PresetId!");
	}

	public void SetToSizeBlock(MapBlockData groupRoot)
	{
		if (groupRoot != null)
		{
			TemplateId = groupRoot.TemplateId;
			RootBlockId = groupRoot.BlockId;
			if (groupRoot.GroupBlockList == null)
			{
				groupRoot.GroupBlockList = new List<MapBlockData>();
			}
			if (!groupRoot.GroupBlockList.Contains(this))
			{
				groupRoot.GroupBlockList.Add(this);
			}
		}
	}

	public short GetCollectItemTemplateId(IRandomSource random, sbyte resourceType)
	{
		ResourceCollectionItem resourceCollectionConfig = GetResourceCollectionConfig();
		if (resourceCollectionConfig == null || resourceCollectionConfig.ItemIdList == null || !resourceCollectionConfig.ItemIdList.CheckIndex(resourceType))
		{
			return -1;
		}
		List<short> dataList = resourceCollectionConfig.ItemIdList[resourceType].DataList;
		if (resourceType == 5)
		{
			List<Config.ShortList> itemIdList = resourceCollectionConfig.ItemIdList;
			Config.ShortList shortList = itemIdList[itemIdList.Count - 1];
			if (shortList.DataList.Count > 0 && random.CheckPercentProb(40))
			{
				dataList = shortList.DataList;
			}
		}
		if (dataList.Count == 0)
		{
			return -1;
		}
		return dataList[random.Next(0, dataList.Count)];
	}

	public MapBlockData GetNearestBlockToTarget(ByteCoordinate pos)
	{
		if (RootBlockId >= 0)
		{
			return GetRootBlock().GetNearestBlockToTarget(pos);
		}
		MapBlockData mapBlockData = this;
		int num = mapBlockData.GetBlockPos().GetManhattanDistance(pos);
		if (GroupBlockList != null)
		{
			for (int i = 0; i < GroupBlockList.Count; i++)
			{
				MapBlockData mapBlockData2 = GroupBlockList[i];
				int manhattanDistance = mapBlockData2.GetBlockPos().GetManhattanDistance(pos);
				if (manhattanDistance < num)
				{
					num = manhattanDistance;
					mapBlockData = mapBlockData2;
				}
			}
		}
		return mapBlockData;
	}

	public int CalcFindTreasureChanceByItemsCount(sbyte luck, int itemsCount)
	{
		return itemsCount * (100 + luck * 3) / 100;
	}

	public int CalcFindTreasureChance(sbyte luck)
	{
		if (Items != null)
		{
			return CalcFindTreasureChanceByItemsCount(luck, Items.Count);
		}
		return 0;
	}
}
