using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

public class SkillPowerChangeCollection : ISerializableGameData
{
	public Dictionary<SkillEffectKey, int> EffectDict = new Dictionary<SkillEffectKey, int>();

	public void Add(SkillEffectKey effectKey, int power)
	{
		if (!EffectDict.TryAdd(effectKey, power))
		{
			EffectDict[effectKey] += power;
		}
	}

	public int GetTotalChangeValue()
	{
		int num = 0;
		foreach (int value in EffectDict.Values)
		{
			num += value;
		}
		return num;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += 2;
		if (EffectDict != null)
		{
			num += (default(SkillEffectKey).GetSerializedSize() + 4) * EffectDict.Count;
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
		if (EffectDict != null)
		{
			int count = EffectDict.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			foreach (KeyValuePair<SkillEffectKey, int> item in EffectDict)
			{
				ptr += item.Key.Serialize(ptr);
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (EffectDict == null)
			{
				EffectDict = new Dictionary<SkillEffectKey, int>();
			}
			EffectDict.Clear();
			for (int i = 0; i < num; i++)
			{
				SkillEffectKey key = default(SkillEffectKey);
				ptr += key.Deserialize(ptr);
				int value = *(int*)ptr;
				ptr += 4;
				EffectDict.Add(key, value);
			}
			ptr += (int)num;
		}
		else
		{
			EffectDict?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
