using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Extra;

public struct LifeSkillCombatReadBookPageData : ISerializableGameData
{
	[SerializableGameDataField]
	public List<byte> PageList;

	public LifeSkillCombatReadBookPageData(LifeSkillCombatReadBookPageData other)
	{
		PageList = new List<byte>(other.PageList);
	}

	public void Assign(LifeSkillCombatReadBookPageData other)
	{
		PageList = new List<byte>(other.PageList);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((PageList == null) ? (num + 2) : (num + (2 + PageList.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (PageList != null)
		{
			int count = PageList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr[i] = PageList[i];
			}
			ptr += count;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (PageList == null)
			{
				PageList = new List<byte>(num);
			}
			else
			{
				PageList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				PageList.Add(ptr[i]);
			}
			ptr += (int)num;
		}
		else
		{
			PageList?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
