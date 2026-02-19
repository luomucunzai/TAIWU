using GameData.Serializer;

namespace GameData.Domains.Taiwu.Display;

[SerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public class VillagerRoleCharacterSlimDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public short RoleTemplateId;

	[SerializableGameDataField]
	public short ArrangementTemplateId;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 8;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = Id;
		byte* num = pData + 4;
		*(short*)num = RoleTemplateId;
		byte* num2 = num + 2;
		*(short*)num2 = ArrangementTemplateId;
		int num3 = (int)(num2 + 2 - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Id = *(int*)ptr;
		ptr += 4;
		RoleTemplateId = *(short*)ptr;
		ptr += 2;
		ArrangementTemplateId = *(short*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
