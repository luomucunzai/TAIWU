using System;
using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Information;

[SerializableGameData(NotForDisplayModule = true)]
public class SecretInformationCharacterUsedCount : ISerializableGameData
{
	[SerializableGameDataField]
	public readonly IDictionary<int, sbyte> UsedCounts;

	public SecretInformationCharacterUsedCount()
	{
		UsedCounts = new Dictionary<int, sbyte>();
	}

	public SecretInformationCharacterUsedCount(SecretInformationCharacterUsedCount other)
		: this()
	{
		Assign(other);
	}

	public void Assign(SecretInformationCharacterUsedCount other)
	{
		UsedCounts.Clear();
		foreach (KeyValuePair<int, sbyte> usedCount in other.UsedCounts)
		{
			UsedCounts.Add(usedCount.Key, usedCount.Value);
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((UsedCounts == null) ? (num + 4) : (num + (4 + 5 * UsedCounts.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (UsedCounts != null)
		{
			int count = UsedCounts.Count;
			*(int*)ptr = count;
			ptr += 4;
			foreach (KeyValuePair<int, sbyte> usedCount in UsedCounts)
			{
				*(int*)ptr = usedCount.Key;
				ptr += 4;
				*ptr = (byte)usedCount.Value;
				ptr++;
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
		uint num = *(uint*)ptr;
		ptr += 4;
		if (num != 0)
		{
			if (UsedCounts == null)
			{
				throw new NotImplementedException();
			}
			UsedCounts.Clear();
			for (int i = 0; i < num; i++)
			{
				int key = *(int*)ptr;
				ptr += 4;
				sbyte value = (sbyte)(*ptr);
				ptr++;
				UsedCounts.Add(key, value);
			}
		}
		else
		{
			UsedCounts?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
