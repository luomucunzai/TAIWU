using System;
using System.Collections.Generic;
using System.Text;
using Config;
using GameData.Domains.Character.Display;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

[SerializableGameData(NoCopyConstructors = true)]
public class EventSelectItemData : ISerializableGameData
{
	[SerializableGameDataField]
	public List<ItemDisplayData> CanSelectItemList = new List<ItemDisplayData>();

	[SerializableGameDataField]
	public List<SelectItemFilter> FilterList = new List<SelectItemFilter>();

	[SerializableGameDataField]
	public bool FilterWithOrOperate;

	[SerializableGameDataField]
	public sbyte ItemOperationType = 2;

	[SerializableGameDataField]
	public CharacterLoveAndHateItemInfo LoveAndHateItemInfo = new CharacterLoveAndHateItemInfo
	{
		CharacterId = -1,
		LovingItemSubType = -1,
		HatingItemSubType = -1
	};

	[SerializableGameDataField]
	public string ConfirmDisableTips;

	[SerializableGameDataField]
	public List<int> VisibleItemFilterTypes;

	[SerializableGameDataField]
	public bool IsSelectingItemReward;

	[SerializableGameDataField]
	public int ResourceMaxValue = -1;

	public Action OnSelectFinish;

	public bool IsAvailableSelectResult(List<ItemKey> selectResult)
	{
		if (selectResult == null)
		{
			return false;
		}
		if (FilterList == null || FilterList.Count <= 0)
		{
			return true;
		}
		if (!selectResult[0].IsValid())
		{
			return true;
		}
		short itemSubType = ItemTemplateHelper.GetItemSubType(selectResult[0].ItemType, selectResult[0].TemplateId);
		if (FilterList.Count == 1 && itemSubType == 1202)
		{
			return true;
		}
		Dictionary<ItemKey, ItemDisplayData> canSelectItemMap = new Dictionary<ItemKey, ItemDisplayData>(CanSelectItemList.Count);
		CanSelectItemList.ForEach(delegate(ItemDisplayData e)
		{
			canSelectItemMap[e.Key] = e;
		});
		List<ItemKey> list = new List<ItemKey>();
		for (int num = 0; num < FilterList.Count; num++)
		{
			SelectItemFilter selectItemFilter = FilterList[num];
			ItemFilterRulesItem item = ItemFilterRules.Instance.GetItem(selectItemFilter.FilterTemplateId);
			bool flag = false;
			foreach (ItemKey item2 in selectResult)
			{
				if (list.Contains(item2))
				{
					continue;
				}
				if (selectItemFilter.DisplayDataFilterId != 0)
				{
					Predicate<ItemDisplayData> filter = ItemDisplayDataFilters.GetFilter(selectItemFilter.DisplayDataFilterId);
					if (filter == null)
					{
						continue;
					}
					bool flag2 = ItemTemplateHelper.IsMiscResource(item2.ItemType, item2.TemplateId);
					bool flag3 = selectItemFilter.DisplayDataFilterId == 2;
					if (canSelectItemMap.TryGetValue(item2, out var value) && ((flag3 && flag2) || filter(value)))
					{
						flag = true;
						list.Add(item2);
						if (FilterWithOrOperate)
						{
							return true;
						}
					}
				}
				else if (ItemTemplateHelper.MatchItemFilterRule(item2.ItemType, item2.TemplateId, item))
				{
					flag = true;
					list.Add(item2);
					if (FilterWithOrOperate)
					{
						return true;
					}
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 27;
		if (CanSelectItemList != null)
		{
			num += 2;
			int count = CanSelectItemList.Count;
			for (int i = 0; i < count; i++)
			{
				ItemDisplayData itemDisplayData = CanSelectItemList[i];
				num = ((itemDisplayData == null) ? (num + 2) : (num + (2 + itemDisplayData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (FilterList != null)
		{
			num += 2;
			int count2 = FilterList.Count;
			for (int j = 0; j < count2; j++)
			{
				num += FilterList[j].GetSerializedSize();
			}
		}
		else
		{
			num += 2;
		}
		num = ((ConfirmDisableTips == null) ? (num + 2) : (num + (2 + 2 * ConfirmDisableTips.Length)));
		num = ((VisibleItemFilterTypes == null) ? (num + 2) : (num + (2 + 4 * VisibleItemFilterTypes.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (CanSelectItemList != null)
		{
			int count = CanSelectItemList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ItemDisplayData itemDisplayData = CanSelectItemList[i];
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
		if (FilterList != null)
		{
			int count2 = FilterList.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				int num2 = FilterList[j].Serialize(ptr);
				ptr += num2;
				Tester.Assert(num2 <= 65535);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (FilterWithOrOperate ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)ItemOperationType;
		ptr++;
		ptr += LoveAndHateItemInfo.Serialize(ptr);
		if (ConfirmDisableTips != null)
		{
			int length = ConfirmDisableTips.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* confirmDisableTips = ConfirmDisableTips)
			{
				for (int k = 0; k < length; k++)
				{
					((short*)ptr)[k] = (short)confirmDisableTips[k];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (VisibleItemFilterTypes != null)
		{
			int count3 = VisibleItemFilterTypes.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int l = 0; l < count3; l++)
			{
				((int*)ptr)[l] = VisibleItemFilterTypes[l];
			}
			ptr += 4 * count3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (IsSelectingItemReward ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = ResourceMaxValue;
		ptr += 4;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (CanSelectItemList == null)
			{
				CanSelectItemList = new List<ItemDisplayData>(num);
			}
			else
			{
				CanSelectItemList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				if (num2 > 0)
				{
					ItemDisplayData itemDisplayData = new ItemDisplayData();
					ptr += itemDisplayData.Deserialize(ptr);
					CanSelectItemList.Add(itemDisplayData);
				}
				else
				{
					CanSelectItemList.Add(null);
				}
			}
		}
		else
		{
			CanSelectItemList?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (FilterList == null)
			{
				FilterList = new List<SelectItemFilter>(num3);
			}
			else
			{
				FilterList.Clear();
			}
			for (int j = 0; j < num3; j++)
			{
				SelectItemFilter item = default(SelectItemFilter);
				ptr += item.Deserialize(ptr);
				FilterList.Add(item);
			}
		}
		else
		{
			FilterList?.Clear();
		}
		FilterWithOrOperate = *ptr != 0;
		ptr++;
		ItemOperationType = (sbyte)(*ptr);
		ptr++;
		if (LoveAndHateItemInfo == null)
		{
			LoveAndHateItemInfo = new CharacterLoveAndHateItemInfo();
		}
		ptr += LoveAndHateItemInfo.Deserialize(ptr);
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			int num5 = 2 * num4;
			ConfirmDisableTips = Encoding.Unicode.GetString(ptr, num5);
			ptr += num5;
		}
		else
		{
			ConfirmDisableTips = null;
		}
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		if (num6 > 0)
		{
			if (VisibleItemFilterTypes == null)
			{
				VisibleItemFilterTypes = new List<int>(num6);
			}
			else
			{
				VisibleItemFilterTypes.Clear();
			}
			for (int k = 0; k < num6; k++)
			{
				VisibleItemFilterTypes.Add(((int*)ptr)[k]);
			}
			ptr += 4 * num6;
		}
		else
		{
			VisibleItemFilterTypes?.Clear();
		}
		IsSelectingItemReward = *ptr != 0;
		ptr++;
		ResourceMaxValue = *(int*)ptr;
		ptr += 4;
		int num7 = (int)(ptr - pData);
		if (num7 > 4)
		{
			return (num7 + 3) / 4 * 4;
		}
		return num7;
	}
}
