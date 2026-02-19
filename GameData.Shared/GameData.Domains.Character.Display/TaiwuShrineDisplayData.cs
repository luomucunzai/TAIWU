using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Display;

public class TaiwuShrineDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public int Authority;

	[SerializableGameDataField]
	public List<int> CharIdList;

	public TaiwuShrineDisplayData()
	{
	}

	public TaiwuShrineDisplayData(TaiwuShrineDisplayData other)
	{
		Authority = other.Authority;
		List<int> charIdList = other.CharIdList;
		int count = charIdList.Count;
		CharIdList = new List<int>(count);
		for (int i = 0; i < count; i++)
		{
			CharIdList.Add(charIdList[i]);
		}
	}

	public void Assign(TaiwuShrineDisplayData other)
	{
		Authority = other.Authority;
		List<int> charIdList = other.CharIdList;
		int count = charIdList.Count;
		CharIdList = new List<int>(count);
		for (int i = 0; i < count; i++)
		{
			CharIdList.Add(charIdList[i]);
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num += 2;
		if (CharIdList != null && CharIdList.Count > 0)
		{
			num += 4 * CharIdList.Count;
		}
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = Authority;
		ptr += 4;
		if (CharIdList != null)
		{
			int count = CharIdList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				*(int*)ptr = CharIdList[i];
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
		Authority = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (CharIdList == null)
			{
				CharIdList = new List<int>(num);
			}
			else
			{
				CharIdList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				int item = *(int*)ptr;
				ptr += 4;
				CharIdList.Add(item);
			}
		}
		else
		{
			CharIdList?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
