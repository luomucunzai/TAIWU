using System.Collections.Generic;
using GameData.Serializer;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Combat;

[AutoGenerateSerializableGameData(NotForArchive = true)]
public class DefeatMarksCountOutOfCombatData : ISerializableGameData
{
	[SerializableGameDataField]
	public Dictionary<short, int> DefeatMarksDict = new Dictionary<short, int>();

	public DefeatMarksCountOutOfCombatData()
	{
	}

	public DefeatMarksCountOutOfCombatData(DefeatMarksCountOutOfCombatData other)
	{
		DefeatMarksDict = ((other.DefeatMarksDict == null) ? null : new Dictionary<short, int>(other.DefeatMarksDict));
	}

	public void Assign(DefeatMarksCountOutOfCombatData other)
	{
		DefeatMarksDict = ((other.DefeatMarksDict == null) ? null : new Dictionary<short, int>(other.DefeatMarksDict));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += 4;
		if (DefeatMarksDict != null)
		{
			foreach (KeyValuePair<short, int> item in DefeatMarksDict)
			{
				_ = item;
				num += 2;
				num += 4;
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
		if (DefeatMarksDict != null)
		{
			*(int*)ptr = DefeatMarksDict.Count;
			ptr += 4;
			foreach (KeyValuePair<short, int> item in DefeatMarksDict)
			{
				*(short*)ptr = item.Key;
				ptr += 2;
				*(int*)ptr = item.Value;
				ptr += 4;
			}
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
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
		int num = *(int*)ptr;
		ptr += 4;
		if (num > 0)
		{
			if (DefeatMarksDict == null)
			{
				DefeatMarksDict = new Dictionary<short, int>();
			}
			else
			{
				DefeatMarksDict.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				short key = *(short*)ptr;
				ptr += 2;
				int value = *(int*)ptr;
				ptr += 4;
				DefeatMarksDict.Add(key, value);
			}
		}
		else
		{
			DefeatMarksDict?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
