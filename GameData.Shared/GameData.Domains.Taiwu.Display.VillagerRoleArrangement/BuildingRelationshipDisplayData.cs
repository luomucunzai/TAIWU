using GameData.Serializer;

namespace GameData.Domains.Taiwu.Display.VillagerRoleArrangement;

[SerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public class BuildingRelationshipDisplayData : IVillagerRoleArrangementDisplayData, ISerializableGameData
{
	[SerializableGameDataField]
	public int RelationshipChange;

	[SerializableGameDataField]
	public bool IsIncreaseRelationship;

	[SerializableGameDataField]
	public int AffectedPeopleCount;

	[SerializableGameDataField]
	public int SecretInformationGainChange;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 13;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = RelationshipChange;
		byte* num = pData + 4;
		*num = (IsIncreaseRelationship ? ((byte)1) : ((byte)0));
		byte* num2 = num + 1;
		*(int*)num2 = AffectedPeopleCount;
		byte* num3 = num2 + 4;
		*(int*)num3 = SecretInformationGainChange;
		int num4 = (int)(num3 + 4 - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		RelationshipChange = *(int*)ptr;
		ptr += 4;
		IsIncreaseRelationship = *ptr != 0;
		ptr++;
		AffectedPeopleCount = *(int*)ptr;
		ptr += 4;
		SecretInformationGainChange = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
