using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Taiwu;

public struct SkillBreakPlateObsoleteList : ISerializableGameData
{
	public List<SkillBreakPlateObsolete> Items;

	public static SkillBreakPlateObsoleteList Create()
	{
		SkillBreakPlateObsoleteList result = default(SkillBreakPlateObsoleteList);
		result.Items = new List<SkillBreakPlateObsolete>();
		return result;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		int num2 = ((Items != null) ? Items.Count : 0);
		for (int i = 0; i < num2; i++)
		{
			num++;
			if (Items[i] != null)
			{
				num += Items[i].GetSerializedSize();
			}
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (Items != null)
		{
			int num = (*(int*)ptr = Items.Count);
			ptr += 4;
			for (int i = 0; i < num; i++)
			{
				*ptr = ((Items[i] != null) ? ((byte)1) : ((byte)0));
				ptr++;
				if (Items[i] != null)
				{
					ptr += Items[i].Serialize(ptr);
				}
			}
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		int num = *(int*)ptr;
		ptr += 4;
		if (num > 0)
		{
			if (Items == null)
			{
				Items = new List<SkillBreakPlateObsolete>(num);
			}
			else
			{
				Items.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				byte num2 = *ptr;
				ptr++;
				if (num2 != 0)
				{
					SkillBreakPlateObsolete skillBreakPlateObsolete = new SkillBreakPlateObsolete();
					ptr += skillBreakPlateObsolete.Deserialize(ptr);
					Items.Add(skillBreakPlateObsolete);
				}
				else
				{
					Items.Add(null);
				}
			}
		}
		else
		{
			Items?.Clear();
		}
		return (int)(ptr - pData);
	}
}
