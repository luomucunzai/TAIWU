using System;
using System.Collections.Generic;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Serializer;

namespace GameData.Domains.Information;

[SerializableGameData(NotForDisplayModule = true)]
public class SecretInformationMetaData : BaseGameDataObject, ISerializableGameData
{
	internal class FixedFieldInfos
	{
		public const uint Id_Offset = 0u;

		public const int Id_Size = 4;

		public const uint Offset_Offset = 4u;

		public const int Offset_Size = 4;

		public const uint RelevanceSecretInformationMetaDataId_Offset = 8u;

		public const int RelevanceSecretInformationMetaDataId_Size = 4;
	}

	[CollectionObjectField(false, true, false, true, false)]
	private int _id;

	[CollectionObjectField(false, true, false, false, false)]
	private int _offset;

	[CollectionObjectField(false, true, false, false, false)]
	private SecretInformationDisseminationData _disseminationData;

	[CollectionObjectField(false, true, false, false, false)]
	private int _relevanceSecretInformationMetaDataId;

	[CollectionObjectField(false, true, false, false, false)]
	private SecretInformationCharacterRelationshipSnapshotCollection _secretInformationCharacterRelationshipSnapshotCollection;

	[CollectionObjectField(false, true, false, false, false)]
	private SecretInformationCharacterExtraInfoCollection _secretInformationCharacterExtraInfoCollection;

	public const int FixedSize = 12;

	public const int DynamicCount = 3;

	public SecretInformationCharacterRelationshipSnapshotCollection CharacterRelationshipSnapshotCollection => _secretInformationCharacterRelationshipSnapshotCollection;

	public SecretInformationCharacterExtraInfoCollection CharacterExtraInfoCollection => _secretInformationCharacterExtraInfoCollection;

	public SecretInformationMetaData(int id, int offset, int relevanceSecretInformationMetaDataId = -1)
		: this()
	{
		_id = id;
		_offset = offset;
		_relevanceSecretInformationMetaDataId = relevanceSecretInformationMetaDataId;
	}

	public void UpdateOffset(int delta)
	{
		_offset += delta;
	}

	public void IncreaseCharacterDisseminationCount(int characterId)
	{
		if (_disseminationData.DisseminationCounts.TryGetValue(characterId, out var value))
		{
			_disseminationData.DisseminationCounts[characterId] = value + 1;
		}
		else
		{
			_disseminationData.DisseminationCounts.Add(characterId, 1);
		}
	}

	public int GetCharacterDisseminationCount(int characterId)
	{
		int value;
		return _disseminationData.DisseminationCounts.TryGetValue(characterId, out value) ? value : 0;
	}

	public ICollection<int> GetDisseminationBranchCharacterIds()
	{
		return _disseminationData.DisseminationCounts.Keys;
	}

	public int GetId()
	{
		return _id;
	}

	public int GetOffset()
	{
		return _offset;
	}

	public unsafe void SetOffset(int offset, DataContext context)
	{
		_offset = offset;
		SetModifiedAndInvalidateInfluencedCache(1, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 4u, 4);
			*(int*)ptr = _offset;
			ptr += 4;
		}
	}

	public SecretInformationDisseminationData GetDisseminationData()
	{
		return _disseminationData;
	}

	public unsafe void SetDisseminationData(SecretInformationDisseminationData disseminationData, DataContext context)
	{
		_disseminationData = disseminationData;
		SetModifiedAndInvalidateInfluencedCache(2, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _disseminationData.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 0, serializedSize);
			ptr += _disseminationData.Serialize(ptr);
		}
	}

	public int GetRelevanceSecretInformationMetaDataId()
	{
		return _relevanceSecretInformationMetaDataId;
	}

	public unsafe void SetRelevanceSecretInformationMetaDataId(int relevanceSecretInformationMetaDataId, DataContext context)
	{
		_relevanceSecretInformationMetaDataId = relevanceSecretInformationMetaDataId;
		SetModifiedAndInvalidateInfluencedCache(3, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 8u, 4);
			*(int*)ptr = _relevanceSecretInformationMetaDataId;
			ptr += 4;
		}
	}

	public SecretInformationCharacterRelationshipSnapshotCollection GetSecretInformationCharacterRelationshipSnapshotCollection()
	{
		return _secretInformationCharacterRelationshipSnapshotCollection;
	}

	public unsafe void SetSecretInformationCharacterRelationshipSnapshotCollection(SecretInformationCharacterRelationshipSnapshotCollection secretInformationCharacterRelationshipSnapshotCollection, DataContext context)
	{
		_secretInformationCharacterRelationshipSnapshotCollection = secretInformationCharacterRelationshipSnapshotCollection;
		SetModifiedAndInvalidateInfluencedCache(4, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _secretInformationCharacterRelationshipSnapshotCollection.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 1, serializedSize);
			ptr += _secretInformationCharacterRelationshipSnapshotCollection.Serialize(ptr);
		}
	}

	public SecretInformationCharacterExtraInfoCollection GetSecretInformationCharacterExtraInfoCollection()
	{
		return _secretInformationCharacterExtraInfoCollection;
	}

	public unsafe void SetSecretInformationCharacterExtraInfoCollection(SecretInformationCharacterExtraInfoCollection secretInformationCharacterExtraInfoCollection, DataContext context)
	{
		_secretInformationCharacterExtraInfoCollection = secretInformationCharacterExtraInfoCollection;
		SetModifiedAndInvalidateInfluencedCache(5, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _secretInformationCharacterExtraInfoCollection.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 2, serializedSize);
			ptr += _secretInformationCharacterExtraInfoCollection.Serialize(ptr);
		}
	}

	public SecretInformationMetaData()
	{
		_disseminationData = new SecretInformationDisseminationData();
		_secretInformationCharacterRelationshipSnapshotCollection = new SecretInformationCharacterRelationshipSnapshotCollection();
		_secretInformationCharacterExtraInfoCollection = new SecretInformationCharacterExtraInfoCollection();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 24;
		int serializedSize = _disseminationData.GetSerializedSize();
		num += serializedSize;
		int serializedSize2 = _secretInformationCharacterRelationshipSnapshotCollection.GetSerializedSize();
		num += serializedSize2;
		int serializedSize3 = _secretInformationCharacterExtraInfoCollection.GetSerializedSize();
		return num + serializedSize3;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = _id;
		ptr += 4;
		*(int*)ptr = _offset;
		ptr += 4;
		*(int*)ptr = _relevanceSecretInformationMetaDataId;
		ptr += 4;
		byte* ptr2 = ptr;
		ptr += 4;
		ptr += _disseminationData.Serialize(ptr);
		int num = (int)(ptr - ptr2 - 4);
		if (num > 4194304)
		{
			throw new Exception($"Size of field {"_disseminationData"} must be less than {4096}KB");
		}
		*(int*)ptr2 = num;
		byte* ptr3 = ptr;
		ptr += 4;
		ptr += _secretInformationCharacterRelationshipSnapshotCollection.Serialize(ptr);
		int num2 = (int)(ptr - ptr3 - 4);
		if (num2 > 4194304)
		{
			throw new Exception($"Size of field {"_secretInformationCharacterRelationshipSnapshotCollection"} must be less than {4096}KB");
		}
		*(int*)ptr3 = num2;
		byte* ptr4 = ptr;
		ptr += 4;
		ptr += _secretInformationCharacterExtraInfoCollection.Serialize(ptr);
		int num3 = (int)(ptr - ptr4 - 4);
		if (num3 > 4194304)
		{
			throw new Exception($"Size of field {"_secretInformationCharacterExtraInfoCollection"} must be less than {4096}KB");
		}
		*(int*)ptr4 = num3;
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		_id = *(int*)ptr;
		ptr += 4;
		_offset = *(int*)ptr;
		ptr += 4;
		_relevanceSecretInformationMetaDataId = *(int*)ptr;
		ptr += 4;
		ptr += 4;
		ptr += _disseminationData.Deserialize(ptr);
		ptr += 4;
		ptr += _secretInformationCharacterRelationshipSnapshotCollection.Deserialize(ptr);
		ptr += 4;
		ptr += _secretInformationCharacterExtraInfoCollection.Deserialize(ptr);
		return (int)(ptr - pData);
	}
}
