using GameData.Serializer;

namespace GameData.Domains.Information;

public class SecretInformationCharacterData : ISerializableGameData
{
	[SerializableGameDataField]
	public int SecretInformationMetaDataId;

	[SerializableGameDataField]
	public int SecretInformationDisseminationBranch;

	[SerializableGameDataField]
	public int SourceCharacterId;

	public SecretInformationCharacterData(int secretInformationMetaDataId, int secretInformationDisseminationBranch = -1)
	{
		SecretInformationMetaDataId = secretInformationMetaDataId;
		SecretInformationDisseminationBranch = secretInformationDisseminationBranch;
		SourceCharacterId = 0;
	}

	public SecretInformationCharacterData()
		: this(-1)
	{
	}

	public SecretInformationCharacterData(SecretInformationCharacterData other)
	{
		SecretInformationMetaDataId = other.SecretInformationMetaDataId;
		SecretInformationDisseminationBranch = other.SecretInformationDisseminationBranch;
		SourceCharacterId = other.SourceCharacterId;
	}

	public void Assign(SecretInformationCharacterData other)
	{
		SecretInformationMetaDataId = other.SecretInformationMetaDataId;
		SecretInformationDisseminationBranch = other.SecretInformationDisseminationBranch;
		SourceCharacterId = other.SourceCharacterId;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 12;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = SecretInformationMetaDataId;
		byte* num = pData + 4;
		*(int*)num = SecretInformationDisseminationBranch;
		byte* num2 = num + 4;
		*(int*)num2 = SourceCharacterId;
		int num3 = (int)(num2 + 4 - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		SecretInformationMetaDataId = *(int*)ptr;
		ptr += 4;
		SecretInformationDisseminationBranch = *(int*)ptr;
		ptr += 4;
		SourceCharacterId = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
