using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

public class EventSelectCharacterData : ISerializableGameData
{
	[SerializableGameDataField(SubDataMaxCount = int.MaxValue)]
	public List<CharacterSelectFilter> FilterList;

	[SerializableGameDataField]
	public bool UseOrOperate;

	[SerializableGameDataField]
	public SelectApprovedTaiwu SelectApprovedTaiwu;

	public Action OnSelectComplete;

	public bool IsAvailableSelectResult(List<int> charIdList)
	{
		if (charIdList == null || charIdList.Count <= 0)
		{
			return false;
		}
		List<CharacterSelectFilter> list = new List<CharacterSelectFilter>(FilterList);
		foreach (int charId in charIdList)
		{
			bool flag = false;
			for (int num = list.Count - 1; num >= 0; num--)
			{
				if (list[num].AvailableCharactersDisplayDataList != null && list[num].AvailableCharactersDisplayDataList.Select((CharacterDisplayData element) => element.CharacterId).ToList().Contains(charId))
				{
					flag = true;
					list.RemoveAt(num);
					break;
				}
			}
			if (UseOrOperate)
			{
				if (flag)
				{
					return true;
				}
			}
			else if (!flag)
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
		int num = 1;
		if (FilterList != null)
		{
			num += 2;
			int count = FilterList.Count;
			for (int i = 0; i < count; i++)
			{
				num += FilterList[i].GetSerializedSize();
			}
		}
		else
		{
			num += 2;
		}
		num = ((SelectApprovedTaiwu == null) ? (num + 2) : (num + (2 + SelectApprovedTaiwu.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (FilterList != null)
		{
			int count = FilterList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				int num = FilterList[i].Serialize(ptr);
				ptr += num;
				Tester.Assert(num <= int.MaxValue);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (UseOrOperate ? ((byte)1) : ((byte)0));
		ptr++;
		if (SelectApprovedTaiwu != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num2 = SelectApprovedTaiwu.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr = (ushort)num2;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (FilterList == null)
			{
				FilterList = new List<CharacterSelectFilter>(num);
			}
			else
			{
				FilterList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				CharacterSelectFilter item = default(CharacterSelectFilter);
				ptr += item.Deserialize(ptr);
				FilterList.Add(item);
			}
		}
		else
		{
			FilterList?.Clear();
		}
		UseOrOperate = *ptr != 0;
		ptr++;
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (SelectApprovedTaiwu == null)
			{
				SelectApprovedTaiwu = new SelectApprovedTaiwu();
			}
			ptr += SelectApprovedTaiwu.Deserialize(ptr);
		}
		else
		{
			SelectApprovedTaiwu = null;
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
