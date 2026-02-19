using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Information;

public class SecretInformationCharacterDataCollection : ISerializableGameData
{
	[SerializableGameDataField]
	public readonly IDictionary<int, SecretInformationCharacterData> Collection;

	public SecretInformationCharacterDataCollection()
	{
		Collection = new Dictionary<int, SecretInformationCharacterData>();
	}

	public SecretInformationCharacterDataCollection(SecretInformationCharacterDataCollection other)
		: this()
	{
		Assign(other);
	}

	public void Assign(SecretInformationCharacterDataCollection other)
	{
		Collection.Clear();
		foreach (KeyValuePair<int, SecretInformationCharacterData> item in other.Collection)
		{
			Collection.Add(item.Key, item.Value);
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		foreach (KeyValuePair<int, SecretInformationCharacterData> item in Collection)
		{
			num += 4;
			num += item.Value.GetSerializedSize();
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = Collection.Count;
		ptr += 4;
		foreach (KeyValuePair<int, SecretInformationCharacterData> item in Collection)
		{
			*(int*)ptr = item.Key;
			ptr += 4;
			ptr += item.Value.Serialize(ptr);
		}
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		int num = *(int*)ptr;
		ptr += 4;
		Collection.Clear();
		for (int i = 0; i < num; i++)
		{
			int key = *(int*)ptr;
			SecretInformationCharacterData secretInformationCharacterData = new SecretInformationCharacterData();
			ptr += 4;
			ptr += secretInformationCharacterData.Deserialize(ptr);
			Collection.Add(key, secretInformationCharacterData);
		}
		return (int)(ptr - pData);
	}
}
