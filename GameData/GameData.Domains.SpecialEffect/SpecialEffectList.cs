using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect;

public class SpecialEffectList : ISerializableGameData
{
	public List<SpecialEffectBase> EffectList;

	public SpecialEffectList()
	{
		EffectList = new List<SpecialEffectBase>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += 2;
		if (EffectList != null)
		{
			for (int i = 0; i < EffectList.Count; i++)
			{
				num += 4;
				num += EffectList[i].GetSerializedSize();
			}
		}
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (EffectList != null)
		{
			int count = EffectList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				SpecialEffectBase specialEffectBase = EffectList[i];
				*(int*)ptr = specialEffectBase.Type;
				ptr += 4;
				ptr += EffectList[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (EffectList == null)
			{
				EffectList = new List<SpecialEffectBase>();
			}
			EffectList.Clear();
			for (int i = 0; i < num; i++)
			{
				int type = *(int*)ptr;
				ptr += 4;
				SpecialEffectBase specialEffectBase = SpecialEffectType.CreateEffectObj(type);
				ptr += specialEffectBase.Deserialize(ptr);
				EffectList.Add(specialEffectBase);
			}
			ptr += (int)num;
		}
		else
		{
			EffectList?.Clear();
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
