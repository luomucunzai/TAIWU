using System.Collections.Generic;
using GameData.Domains.Building;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Display;

[SerializableGameData(NoCopyConstructors = true)]
public class CharacterDisplayDataForTooltip : ISerializableGameData
{
	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public byte CreatingType;

	[SerializableGameDataField]
	public OrganizationInfo OrganizationInfo;

	[SerializableGameDataField]
	public short Age;

	[SerializableGameDataField]
	public FullName FullName;

	[SerializableGameDataField]
	public byte MonkType;

	[SerializableGameDataField]
	public MonasticTitle MonasticTitle;

	[SerializableGameDataField]
	public int CustomDisplayNameId;

	[SerializableGameDataField]
	public short Attraction;

	[SerializableGameDataField]
	public AvatarRelatedData AvatarRelatedData;

	[SerializableGameDataField]
	public sbyte BehaviorType;

	[SerializableGameDataField]
	public MainAttributes MainAttributes;

	[SerializableGameDataField]
	public List<short> FeatureIds;

	[SerializableGameDataField]
	public sbyte Gender;

	[SerializableGameDataField]
	public bool Transgender;

	[SerializableGameDataField]
	public CombatSkillShorts CombatSkillQualifications;

	[SerializableGameDataField]
	public sbyte CombatSkillQualificationGrowthType;

	[SerializableGameDataField]
	public LifeSkillShorts LifeSkillQualifications;

	[SerializableGameDataField]
	public sbyte LifeSkillQualificationGrowthType;

	[SerializableGameDataField]
	public Personalities Personalities;

	[SerializableGameDataField]
	public List<sbyte> TeammateCommands;

	[SerializableGameDataField]
	public int NickNameId;

	[SerializableGameDataField]
	public LifeSkillShorts LifeSkillAttainments;

	[SerializableGameDataField]
	public short FavorabilityToTaiwu;

	[SerializableGameDataField]
	public bool IsInteractedCharacter;

	public CharacterDisplayDataForTooltip()
	{
		NickNameId = -1;
	}

	public CharacterDisplayDataForTooltip(RecruitCharacterData recruitCharacter)
	{
		TemplateId = recruitCharacter.TemplateId;
		CreatingType = 1;
		OrganizationInfo = new OrganizationInfo(0, 0, principal: true, -1);
		Age = recruitCharacter.Age;
		FullName = recruitCharacter.FullName;
		MonkType = 0;
		MonasticTitle = new MonasticTitle(-1, -1);
		CustomDisplayNameId = -1;
		BehaviorType = GameData.Domains.Character.BehaviorType.GetBehaviorType(recruitCharacter.GetBaseMorality());
		Attraction = recruitCharacter.FinalAttraction;
		AvatarRelatedData = new AvatarRelatedData(recruitCharacter);
		MainAttributes = recruitCharacter.MainAttributes;
		FeatureIds = recruitCharacter.FeatureIds;
		Gender = recruitCharacter.Gender;
		Transgender = recruitCharacter.Transgender;
		CombatSkillQualifications = recruitCharacter.CombatSkillQualifications;
		CombatSkillQualificationGrowthType = recruitCharacter.CombatSkillQualificationGrowthType;
		LifeSkillQualifications = recruitCharacter.LifeSkillQualifications;
		LifeSkillQualificationGrowthType = recruitCharacter.LifeSkillQualificationGrowthType;
		Personalities = recruitCharacter.CalculatedPersonalities;
		TeammateCommands = recruitCharacter.TeammateCommands;
		NickNameId = -1;
		LifeSkillAttainments = default(LifeSkillShorts);
		FavorabilityToTaiwu = 0;
		IsInteractedCharacter = false;
	}

	public NameRelatedData GetNameRelatedData()
	{
		NameRelatedData result = new NameRelatedData();
		result.CharTemplateId = TemplateId;
		result.Gender = Gender;
		result.MonkType = MonkType;
		result.FullName = FullName;
		result.OrgTemplateId = OrganizationInfo.OrgTemplateId;
		result.OrgGrade = OrganizationInfo.Grade;
		result.MonasticTitle = MonasticTitle;
		result.CustomDisplayNameId = CustomDisplayNameId;
		result.NickNameId = NickNameId;
		result.ExtraNameTextTemplateId = -1;
		return result;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 246;
		num = ((FeatureIds == null) ? (num + 2) : (num + (2 + 2 * FeatureIds.Count)));
		num = ((TeammateCommands == null) ? (num + 2) : (num + (2 + TeammateCommands.Count)));
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
		*ptr = CreatingType;
		ptr++;
		ptr += OrganizationInfo.Serialize(ptr);
		*(short*)ptr = Age;
		ptr += 2;
		ptr += FullName.Serialize(ptr);
		*ptr = MonkType;
		ptr++;
		ptr += MonasticTitle.Serialize(ptr);
		*(int*)ptr = CustomDisplayNameId;
		ptr += 4;
		*(short*)ptr = Attraction;
		ptr += 2;
		ptr += AvatarRelatedData.Serialize(ptr);
		*ptr = (byte)BehaviorType;
		ptr++;
		ptr += MainAttributes.Serialize(ptr);
		if (FeatureIds != null)
		{
			int count = FeatureIds.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = FeatureIds[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)Gender;
		ptr++;
		*ptr = (Transgender ? ((byte)1) : ((byte)0));
		ptr++;
		ptr += CombatSkillQualifications.Serialize(ptr);
		*ptr = (byte)CombatSkillQualificationGrowthType;
		ptr++;
		ptr += LifeSkillQualifications.Serialize(ptr);
		*ptr = (byte)LifeSkillQualificationGrowthType;
		ptr++;
		ptr += Personalities.Serialize(ptr);
		if (TeammateCommands != null)
		{
			int count2 = TeammateCommands.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				ptr[j] = (byte)TeammateCommands[j];
			}
			ptr += count2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = NickNameId;
		ptr += 4;
		ptr += LifeSkillAttainments.Serialize(ptr);
		*(short*)ptr = FavorabilityToTaiwu;
		ptr += 2;
		*ptr = (IsInteractedCharacter ? ((byte)1) : ((byte)0));
		ptr++;
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
		CreatingType = *ptr;
		ptr++;
		ptr += OrganizationInfo.Deserialize(ptr);
		Age = *(short*)ptr;
		ptr += 2;
		ptr += FullName.Deserialize(ptr);
		MonkType = *ptr;
		ptr++;
		ptr += MonasticTitle.Deserialize(ptr);
		CustomDisplayNameId = *(int*)ptr;
		ptr += 4;
		Attraction = *(short*)ptr;
		ptr += 2;
		if (AvatarRelatedData == null)
		{
			AvatarRelatedData = new AvatarRelatedData();
		}
		ptr += AvatarRelatedData.Deserialize(ptr);
		BehaviorType = (sbyte)(*ptr);
		ptr++;
		ptr += MainAttributes.Deserialize(ptr);
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (FeatureIds == null)
			{
				FeatureIds = new List<short>(num);
			}
			else
			{
				FeatureIds.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				FeatureIds.Add(((short*)ptr)[i]);
			}
			ptr += 2 * num;
		}
		else
		{
			FeatureIds?.Clear();
		}
		Gender = (sbyte)(*ptr);
		ptr++;
		Transgender = *ptr != 0;
		ptr++;
		ptr += CombatSkillQualifications.Deserialize(ptr);
		CombatSkillQualificationGrowthType = (sbyte)(*ptr);
		ptr++;
		ptr += LifeSkillQualifications.Deserialize(ptr);
		LifeSkillQualificationGrowthType = (sbyte)(*ptr);
		ptr++;
		ptr += Personalities.Deserialize(ptr);
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (TeammateCommands == null)
			{
				TeammateCommands = new List<sbyte>(num2);
			}
			else
			{
				TeammateCommands.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				TeammateCommands.Add((sbyte)ptr[j]);
			}
			ptr += (int)num2;
		}
		else
		{
			TeammateCommands?.Clear();
		}
		NickNameId = *(int*)ptr;
		ptr += 4;
		ptr += LifeSkillAttainments.Deserialize(ptr);
		FavorabilityToTaiwu = *(short*)ptr;
		ptr += 2;
		IsInteractedCharacter = *ptr != 0;
		ptr++;
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
