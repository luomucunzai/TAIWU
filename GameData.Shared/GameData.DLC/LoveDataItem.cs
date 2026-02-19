using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.DLC;

public class LoveDataItem : ISerializableGameData
{
	[SerializableGameDataField]
	public int TaiwuCharId;

	[SerializableGameDataField]
	public int LoverCharId;

	[SerializableGameDataField]
	public List<bool> ReincarnationLoveList;

	[SerializableGameDataField]
	public ItemKey TaiwuOwnedToken;

	[SerializableGameDataField]
	public ItemKey LoverOwnedToken;

	[SerializableGameDataField]
	public int DateCount;

	[SerializableGameDataField]
	public bool IsBindSamsara;

	[SerializableGameDataField]
	public int TaiwuNicknameId;

	[SerializableGameDataField]
	public int InteractTime;

	[SerializableGameDataField]
	public int BecomeLoverTime;

	[SerializableGameDataField]
	public Dictionary<sbyte, int> EventTimeDict;

	public bool CheckReincarnationLoveContinueThreeTimes()
	{
		if (ReincarnationLoveList.Count < 3)
		{
			return false;
		}
		int num = 0;
		foreach (bool reincarnationLove in ReincarnationLoveList)
		{
			num = (reincarnationLove ? (num + 1) : 0);
		}
		return num >= 3;
	}

	public LoveDataItem()
	{
		ReincarnationLoveList = new List<bool>();
		TaiwuOwnedToken = ItemKey.Invalid;
		LoverOwnedToken = ItemKey.Invalid;
		EventTimeDict = new Dictionary<sbyte, int>();
	}

	public LoveDataItem(LoveDataItem other)
	{
		TaiwuCharId = other.TaiwuCharId;
		LoverCharId = other.LoverCharId;
		ReincarnationLoveList = new List<bool>(other.ReincarnationLoveList);
		TaiwuOwnedToken = other.TaiwuOwnedToken;
		LoverOwnedToken = other.LoverOwnedToken;
		DateCount = other.DateCount;
		IsBindSamsara = other.IsBindSamsara;
		TaiwuNicknameId = other.TaiwuNicknameId;
		InteractTime = other.InteractTime;
		BecomeLoverTime = other.BecomeLoverTime;
		EventTimeDict = other.EventTimeDict;
	}

	public void Assign(LoveDataItem other)
	{
		TaiwuCharId = other.TaiwuCharId;
		LoverCharId = other.LoverCharId;
		ReincarnationLoveList = new List<bool>(other.ReincarnationLoveList);
		TaiwuOwnedToken = other.TaiwuOwnedToken;
		LoverOwnedToken = other.LoverOwnedToken;
		DateCount = other.DateCount;
		IsBindSamsara = other.IsBindSamsara;
		TaiwuNicknameId = other.TaiwuNicknameId;
		InteractTime = other.InteractTime;
		BecomeLoverTime = other.BecomeLoverTime;
		EventTimeDict = other.EventTimeDict;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 41;
		num = ((ReincarnationLoveList == null) ? (num + 2) : (num + (2 + ReincarnationLoveList.Count)));
		num = ((EventTimeDict == null || EventTimeDict.Count <= 0) ? (num + 2) : (num + (2 + 3 * EventTimeDict.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = TaiwuCharId;
		ptr += 4;
		*(int*)ptr = LoverCharId;
		ptr += 4;
		if (ReincarnationLoveList != null)
		{
			int count = ReincarnationLoveList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (ReincarnationLoveList[i] ? ((byte)1) : ((byte)0));
			}
			ptr += count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += TaiwuOwnedToken.Serialize(ptr);
		ptr += LoverOwnedToken.Serialize(ptr);
		*(int*)ptr = DateCount;
		ptr += 4;
		*ptr = (IsBindSamsara ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = TaiwuNicknameId;
		ptr += 4;
		*(int*)ptr = InteractTime;
		ptr += 4;
		*(int*)ptr = BecomeLoverTime;
		ptr += 4;
		if (EventTimeDict != null && EventTimeDict.Count > 0)
		{
			*(ushort*)ptr = (ushort)EventTimeDict.Count;
			ptr += 2;
			foreach (KeyValuePair<sbyte, int> item in EventTimeDict)
			{
				*ptr = (byte)item.Key;
				ptr++;
				*(int*)ptr = item.Value;
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
		TaiwuCharId = *(int*)ptr;
		ptr += 4;
		LoverCharId = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (ReincarnationLoveList == null)
			{
				ReincarnationLoveList = new List<bool>(num);
			}
			else
			{
				ReincarnationLoveList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ReincarnationLoveList.Add(ptr[i] != 0);
			}
			ptr += (int)num;
		}
		else
		{
			ReincarnationLoveList?.Clear();
		}
		ptr += TaiwuOwnedToken.Deserialize(ptr);
		ptr += LoverOwnedToken.Deserialize(ptr);
		DateCount = *(int*)ptr;
		ptr += 4;
		IsBindSamsara = *ptr != 0;
		ptr++;
		TaiwuNicknameId = *(int*)ptr;
		ptr += 4;
		InteractTime = *(int*)ptr;
		ptr += 4;
		BecomeLoverTime = *(int*)ptr;
		ptr += 4;
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (EventTimeDict != null)
			{
				EventTimeDict.Clear();
			}
			else
			{
				EventTimeDict = new Dictionary<sbyte, int>(num2);
			}
			for (int j = 0; j < num2; j++)
			{
				sbyte key = (sbyte)(*ptr);
				ptr++;
				int value = *(int*)ptr;
				ptr += 4;
				EventTimeDict.Add(key, value);
			}
		}
		else
		{
			EventTimeDict?.Clear();
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
