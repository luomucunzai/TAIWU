using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

public class SkillEffectCollection : ISerializableGameData
{
	public Dictionary<SkillEffectKey, short> EffectDict;

	public Dictionary<SkillEffectKey, CombatSkillEffectDescriptionDisplayData> EffectDescriptionDict;

	public readonly Dictionary<SkillEffectKey, short> MaxEffectCountDict = new Dictionary<SkillEffectKey, short>();

	public readonly Dictionary<SkillEffectKey, bool> AutoRemoveOnNoCountDict = new Dictionary<SkillEffectKey, bool>();

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
			num += (default(SkillEffectKey).GetSerializedSize() + 2) * EffectDict.Count;
		}
		num += 2;
		if (EffectDescriptionDict != null)
		{
			foreach (KeyValuePair<SkillEffectKey, CombatSkillEffectDescriptionDisplayData> item in EffectDescriptionDict)
			{
				num += item.Key.GetSerializedSize() + item.Value.GetSerializedSize();
			}
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
			foreach (KeyValuePair<SkillEffectKey, short> item in EffectDict)
			{
				ptr += item.Key.Serialize(ptr);
				*(short*)ptr = item.Value;
				ptr += 2;
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (EffectDescriptionDict != null)
		{
			int count2 = EffectDescriptionDict.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			foreach (KeyValuePair<SkillEffectKey, CombatSkillEffectDescriptionDisplayData> item2 in EffectDescriptionDict)
			{
				ptr += item2.Key.Serialize(ptr);
				ptr += item2.Value.Serialize(ptr);
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
				EffectDict = new Dictionary<SkillEffectKey, short>();
			}
			EffectDict.Clear();
			for (int i = 0; i < num; i++)
			{
				SkillEffectKey key = default(SkillEffectKey);
				ptr += key.Deserialize(ptr);
				short value = *(short*)ptr;
				ptr += 2;
				EffectDict.Add(key, value);
			}
		}
		else
		{
			EffectDict?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (EffectDescriptionDict == null)
			{
				EffectDescriptionDict = new Dictionary<SkillEffectKey, CombatSkillEffectDescriptionDisplayData>();
			}
			EffectDescriptionDict.Clear();
			for (int j = 0; j < num2; j++)
			{
				SkillEffectKey key2 = default(SkillEffectKey);
				CombatSkillEffectDescriptionDisplayData value2 = default(CombatSkillEffectDescriptionDisplayData);
				ptr += key2.Deserialize(ptr);
				ptr += value2.Deserialize(ptr);
				EffectDescriptionDict.Add(key2, value2);
			}
		}
		else
		{
			EffectDescriptionDict?.Clear();
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
