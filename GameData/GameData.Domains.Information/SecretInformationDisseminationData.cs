using System;
using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Information;

[SerializableGameData(NotForDisplayModule = true)]
public class SecretInformationDisseminationData : ISerializableGameData
{
	[SerializableGameDataField]
	public readonly IDictionary<int, int> DisseminationCounts;

	public SecretInformationDisseminationData()
	{
		DisseminationCounts = new Dictionary<int, int>();
	}

	public SecretInformationDisseminationData(SecretInformationDisseminationData other)
		: this()
	{
		Assign(other);
	}

	public void Assign(SecretInformationDisseminationData other)
	{
		DisseminationCounts.Clear();
		foreach (KeyValuePair<int, int> disseminationCount in other.DisseminationCounts)
		{
			DisseminationCounts.Add(disseminationCount.Key, disseminationCount.Value);
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((DisseminationCounts == null) ? (num + 4) : (num + (4 + 8 * DisseminationCounts.Count)));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (DisseminationCounts != null)
		{
			int count = DisseminationCounts.Count;
			*(int*)ptr = count;
			ptr += 4;
			foreach (KeyValuePair<int, int> disseminationCount in DisseminationCounts)
			{
				*(int*)ptr = disseminationCount.Key;
				ptr += 4;
				*(int*)ptr = disseminationCount.Value;
				ptr += 4;
			}
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		uint num = *(uint*)ptr;
		ptr += 4;
		if (num != 0)
		{
			if (DisseminationCounts == null)
			{
				throw new NotImplementedException();
			}
			DisseminationCounts.Clear();
			for (int i = 0; i < num; i++)
			{
				int key = *(int*)ptr;
				ptr += 4;
				int value = *(int*)ptr;
				ptr += 4;
				DisseminationCounts.Add(key, value);
			}
		}
		else
		{
			DisseminationCounts?.Clear();
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
