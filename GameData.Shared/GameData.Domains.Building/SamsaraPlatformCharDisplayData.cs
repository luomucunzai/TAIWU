using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Serializer;

namespace GameData.Domains.Building;

public class SamsaraPlatformCharDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public NameRelatedData NameRelatedData;

	[SerializableGameDataField]
	public AvatarRelatedData AvatarRelatedData;

	[SerializableGameDataField]
	public MainAttributes MainAttributes;

	[SerializableGameDataField]
	public CombatSkillShorts CombatSkillQualifications;

	[SerializableGameDataField]
	public LifeSkillShorts LifeSkillQualifications;

	public SamsaraPlatformCharDisplayData()
	{
	}

	public SamsaraPlatformCharDisplayData(SamsaraPlatformCharDisplayData other)
	{
		Id = other.Id;
		TemplateId = other.TemplateId;
		NameRelatedData = other.NameRelatedData;
		AvatarRelatedData = new AvatarRelatedData(other.AvatarRelatedData);
		MainAttributes = other.MainAttributes;
		CombatSkillQualifications = other.CombatSkillQualifications;
		LifeSkillQualifications = other.LifeSkillQualifications;
	}

	public void Assign(SamsaraPlatformCharDisplayData other)
	{
		Id = other.Id;
		TemplateId = other.TemplateId;
		NameRelatedData = other.NameRelatedData;
		AvatarRelatedData = new AvatarRelatedData(other.AvatarRelatedData);
		MainAttributes = other.MainAttributes;
		CombatSkillQualifications = other.CombatSkillQualifications;
		LifeSkillQualifications = other.LifeSkillQualifications;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 194;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = Id;
		ptr += 4;
		*(short*)ptr = TemplateId;
		ptr += 2;
		ptr += NameRelatedData.Serialize(ptr);
		ptr += AvatarRelatedData.Serialize(ptr);
		ptr += MainAttributes.Serialize(ptr);
		ptr += CombatSkillQualifications.Serialize(ptr);
		ptr += LifeSkillQualifications.Serialize(ptr);
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
		Id = *(int*)ptr;
		ptr += 4;
		TemplateId = *(short*)ptr;
		ptr += 2;
		ptr += NameRelatedData.Deserialize(ptr);
		if (AvatarRelatedData == null)
		{
			AvatarRelatedData = new AvatarRelatedData();
		}
		ptr += AvatarRelatedData.Deserialize(ptr);
		ptr += MainAttributes.Deserialize(ptr);
		ptr += CombatSkillQualifications.Deserialize(ptr);
		ptr += LifeSkillQualifications.Deserialize(ptr);
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
