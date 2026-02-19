using System;
using GameData.Serializer;

namespace GameData.Domains.Item.Display;

public class SkillBookPageDisplayData : ISerializableGameData
{
	public ItemKey ItemKey;

	public sbyte[] State;

	public sbyte[] ReadingProgress;

	public sbyte[] Type;

	public bool IsCombatBook => ItemTemplateHelper.GetItemSubType(ItemKey.ItemType, ItemKey.TemplateId) == 1001;

	public bool CanFix()
	{
		bool result = false;
		for (int i = 0; i < State.Length; i++)
		{
			if (State[i] == 1 || State[i] == 2)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public (sbyte pageNum, short needProgress) GetFixProgress()
	{
		sbyte grade = ItemTemplateHelper.GetGrade(ItemKey.ItemType, ItemKey.TemplateId);
		short num = GlobalConfig.Instance.FixBookTotalProgress[grade];
		sbyte item = -1;
		for (sbyte b = 0; b < State.Length; b++)
		{
			if (State[b] == 1)
			{
				item = b;
				break;
			}
			if (State[b] == 2)
			{
				item = b;
				num *= 3;
				break;
			}
		}
		num = Math.Min(short.MaxValue, num);
		return (pageNum: item, needProgress: num);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public unsafe int GetSerializedSize()
	{
		return sizeof(ItemKey) + 1 + (State.Length + ReadingProgress.Length + Type.Length);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(ItemKey*)ptr = ItemKey;
		ptr += sizeof(ItemKey);
		sbyte b = (sbyte)(*ptr = (byte)(sbyte)State.Length);
		ptr++;
		for (sbyte b2 = 0; b2 < b; b2++)
		{
			*ptr = (byte)State[b2];
			ptr++;
			*ptr = (byte)ReadingProgress[b2];
			ptr++;
			*ptr = (byte)Type[b2];
			ptr++;
		}
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ItemKey = *(ItemKey*)ptr;
		ptr += sizeof(ItemKey);
		sbyte b = (sbyte)(*ptr);
		ptr++;
		State = new sbyte[b];
		ReadingProgress = new sbyte[b];
		Type = new sbyte[b];
		for (sbyte b2 = 0; b2 < b; b2++)
		{
			State[b2] = (sbyte)(*ptr);
			ptr++;
			ReadingProgress[b2] = (sbyte)(*ptr);
			ptr++;
			Type[b2] = (sbyte)(*ptr);
			ptr++;
		}
		return (int)(ptr - pData);
	}
}
