using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Information;

[SerializableGameData(NotForDisplayModule = true)]
public class SecretInformationCharacterRelationshipSnapshotCollection : ISerializableGameData
{
	[SerializableGameDataField]
	public readonly IDictionary<int, SecretInformationCharacterRelationshipSnapshot> Collection;

	public SecretInformationCharacterRelationshipSnapshotCollection()
	{
		Collection = new Dictionary<int, SecretInformationCharacterRelationshipSnapshot>();
	}

	public SecretInformationCharacterRelationshipSnapshotCollection(SecretInformationCharacterRelationshipSnapshotCollection other)
		: this()
	{
		Assign(other);
	}

	public void Assign(SecretInformationCharacterRelationshipSnapshotCollection other)
	{
		Collection.Clear();
		foreach (KeyValuePair<int, SecretInformationCharacterRelationshipSnapshot> item in other.Collection)
		{
			Collection.Add(item.Key, item.Value);
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSizeActually()
	{
		int num = 4;
		foreach (KeyValuePair<int, SecretInformationCharacterRelationshipSnapshot> item in Collection)
		{
			num += 4 + item.Value.GetSerializedSize();
		}
		return num;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		foreach (KeyValuePair<int, SecretInformationCharacterRelationshipSnapshot> item in Collection)
		{
			num += 4 + item.Value.GetSerializedSize();
		}
		if (num >= 65535)
		{
			num = 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (GetSerializedSizeActually() >= 65535)
		{
			Collection.Clear();
		}
		*(int*)pData = Collection.Count;
		pData += 4;
		foreach (KeyValuePair<int, SecretInformationCharacterRelationshipSnapshot> item in Collection)
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
			SecretInformationCharacterRelationshipSnapshot secretInformationCharacterRelationshipSnapshot = new SecretInformationCharacterRelationshipSnapshot();
			pData += 4;
			pData += secretInformationCharacterRelationshipSnapshot.Deserialize(pData);
			Collection.Add(key, secretInformationCharacterRelationshipSnapshot);
		}
		return (int)(pData - ptr);
	}
}
