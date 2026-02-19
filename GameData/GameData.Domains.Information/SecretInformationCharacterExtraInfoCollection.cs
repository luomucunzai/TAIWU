using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Information;

[SerializableGameData(NotForDisplayModule = true)]
public class SecretInformationCharacterExtraInfoCollection : ISerializableGameData
{
	public readonly IDictionary<int, SecretInformationCharacterExtraInfo> Collection;

	public SecretInformationCharacterExtraInfoCollection()
	{
		Collection = new Dictionary<int, SecretInformationCharacterExtraInfo>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		foreach (KeyValuePair<int, SecretInformationCharacterExtraInfo> item in Collection)
		{
			num += 4 + item.Value.GetSerializedSize();
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)pData = Collection.Count;
		pData += 4;
		foreach (KeyValuePair<int, SecretInformationCharacterExtraInfo> item in Collection)
		{
			*(int*)pData = item.Key;
			pData += 4;
			pData += item.Value.Serialize(pData);
		}
		return (int)(pData - ptr);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Collection.Clear();
		int num = *(int*)pData;
		pData += 4;
		for (int i = 0; i < num; i++)
		{
			int key = *(int*)pData;
			SecretInformationCharacterExtraInfo value = default(SecretInformationCharacterExtraInfo);
			pData += 4;
			pData += value.Deserialize(pData);
			Collection.Add(key, value);
		}
		return (int)(pData - ptr);
	}
}
