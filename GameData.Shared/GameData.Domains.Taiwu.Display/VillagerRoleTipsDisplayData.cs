using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Display;

[SerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public class VillagerRoleTipsDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public short RoleTemplateId;

	[SerializableGameDataField]
	public List<int> RelatedBuildingClassList;

	[SerializableGameDataField]
	public List<IntPair> DetailList;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num = ((RelatedBuildingClassList == null) ? (num + 2) : (num + (2 + 4 * RelatedBuildingClassList.Count)));
		num = ((DetailList == null) ? (num + 2) : (num + (2 + 8 * DetailList.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = RoleTemplateId;
		ptr += 2;
		if (RelatedBuildingClassList != null)
		{
			int count = RelatedBuildingClassList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = RelatedBuildingClassList[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (DetailList != null)
		{
			int count2 = DetailList.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				ptr += DetailList[j].Serialize(ptr);
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
		RoleTemplateId = *(short*)ptr;
		ptr += 2;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (RelatedBuildingClassList == null)
			{
				RelatedBuildingClassList = new List<int>(num);
			}
			else
			{
				RelatedBuildingClassList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				RelatedBuildingClassList.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			RelatedBuildingClassList?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (DetailList == null)
			{
				DetailList = new List<IntPair>(num2);
			}
			else
			{
				DetailList.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				IntPair item = default(IntPair);
				ptr += item.Deserialize(ptr);
				DetailList.Add(item);
			}
		}
		else
		{
			DetailList?.Clear();
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
