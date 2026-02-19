using System.Collections.Generic;
using GameData.Domains.Character.Display;
using GameData.Serializer;

namespace GameData.Domains.Organization.Display;

[SerializableGameData(NoCopyConstructors = true)]
public class SettlementBountyDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public Dictionary<int, CharacterDisplayDataForSettlementBounty> BountyCharacterDisplayDataDict;

	[SerializableGameDataField]
	public int OrgTemplateId;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, CharacterDisplayDataForSettlementBounty>(BountyCharacterDisplayDataDict);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* num = pData + DictionaryOfBasicTypeCustomTypePair.Serialize<int, CharacterDisplayDataForSettlementBounty>(pData, ref BountyCharacterDisplayDataDict);
		*(int*)num = OrgTemplateId;
		int num2 = (int)(num + 4 - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, CharacterDisplayDataForSettlementBounty>(ptr, ref BountyCharacterDisplayDataDict);
		OrgTemplateId = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
