using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

[SerializableGameData]
public class ShowSpecialEffectCollection : ISerializableGameData
{
	[SerializableGameDataField]
	public List<ShowSpecialEffectDisplayData> ShowEffectList = new List<ShowSpecialEffectDisplayData>();

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		if (ShowEffectList != null)
		{
			num += 2;
			int count = ShowEffectList.Count;
			for (int i = 0; i < count; i++)
			{
				num += ShowEffectList[i].GetSerializedSize();
			}
		}
		else
		{
			num += 2;
		}
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (ShowEffectList != null)
		{
			int count = ShowEffectList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				int num = ShowEffectList[i].Serialize(ptr);
				ptr += num;
				Tester.Assert(num <= 65535);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (ShowEffectList == null)
			{
				ShowEffectList = new List<ShowSpecialEffectDisplayData>(num);
			}
			else
			{
				ShowEffectList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ShowSpecialEffectDisplayData item = default(ShowSpecialEffectDisplayData);
				ptr += item.Deserialize(ptr);
				ShowEffectList.Add(item);
			}
		}
		else
		{
			ShowEffectList?.Clear();
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
