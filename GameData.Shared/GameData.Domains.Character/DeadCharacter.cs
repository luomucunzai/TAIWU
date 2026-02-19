using System.Collections.Generic;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character;

public class DeadCharacter : ISerializableGameData
{
	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public FullName FullName;

	[SerializableGameDataField]
	public MonasticTitle MonasticTitle;

	[SerializableGameDataField]
	public List<short> TitleIds;

	[SerializableGameDataField]
	public sbyte Gender;

	[SerializableGameDataField]
	public sbyte FameType;

	[SerializableGameDataField]
	public sbyte Happiness;

	[SerializableGameDataField]
	public short Morality;

	[SerializableGameDataField]
	public OrganizationInfo OrganizationInfo;

	[SerializableGameDataField]
	public AvatarData Avatar;

	[SerializableGameDataField]
	public short ClothingDisplayId;

	[SerializableGameDataField]
	public short Attraction;

	[SerializableGameDataField]
	public int BirthDate;

	[SerializableGameDataField]
	public int DeathDate;

	[SerializableGameDataField]
	public byte MonkType;

	[SerializableGameDataField]
	public List<short> FeatureIds;

	[SerializableGameDataField]
	public MainAttributes BaseMainAttributes;

	[SerializableGameDataField]
	public short CurrAge;

	[SerializableGameDataField]
	public LifeSkillShorts BaseLifeSkillQualifications;

	[SerializableGameDataField]
	public CombatSkillShorts BaseCombatSkillQualifications;

	[SerializableGameDataField]
	public PreexistenceCharIds PreexistenceCharIds;

	public DeadCharacter()
	{
	}

	public DeadCharacter(DeadCharacter other)
	{
		TemplateId = other.TemplateId;
		FullName = other.FullName;
		MonasticTitle = other.MonasticTitle;
		TitleIds = new List<short>(other.TitleIds);
		Gender = other.Gender;
		FameType = other.FameType;
		Happiness = other.Happiness;
		Morality = other.Morality;
		OrganizationInfo = other.OrganizationInfo;
		Avatar = new AvatarData(other.Avatar);
		ClothingDisplayId = other.ClothingDisplayId;
		Attraction = other.Attraction;
		BirthDate = other.BirthDate;
		DeathDate = other.DeathDate;
		MonkType = other.MonkType;
		FeatureIds = new List<short>(other.FeatureIds);
		BaseMainAttributes = other.BaseMainAttributes;
		CurrAge = other.CurrAge;
		BaseLifeSkillQualifications = other.BaseLifeSkillQualifications;
		BaseCombatSkillQualifications = other.BaseCombatSkillQualifications;
		PreexistenceCharIds = other.PreexistenceCharIds;
	}

	public void Assign(DeadCharacter other)
	{
		TemplateId = other.TemplateId;
		FullName = other.FullName;
		MonasticTitle = other.MonasticTitle;
		TitleIds = new List<short>(other.TitleIds);
		Gender = other.Gender;
		FameType = other.FameType;
		Happiness = other.Happiness;
		Morality = other.Morality;
		OrganizationInfo = other.OrganizationInfo;
		Avatar = new AvatarData(other.Avatar);
		ClothingDisplayId = other.ClothingDisplayId;
		Attraction = other.Attraction;
		BirthDate = other.BirthDate;
		DeathDate = other.DeathDate;
		MonkType = other.MonkType;
		FeatureIds = new List<short>(other.FeatureIds);
		BaseMainAttributes = other.BaseMainAttributes;
		CurrAge = other.CurrAge;
		BaseLifeSkillQualifications = other.BaseLifeSkillQualifications;
		BaseCombatSkillQualifications = other.BaseCombatSkillQualifications;
		PreexistenceCharIds = other.PreexistenceCharIds;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 244;
		num = ((TitleIds == null) ? (num + 2) : (num + (2 + 2 * TitleIds.Count)));
		num = ((FeatureIds == null) ? (num + 2) : (num + (2 + 2 * FeatureIds.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = TemplateId;
		ptr += 2;
		ptr += FullName.Serialize(ptr);
		ptr += MonasticTitle.Serialize(ptr);
		if (TitleIds != null)
		{
			int count = TitleIds.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = TitleIds[i];
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
		*ptr = (byte)FameType;
		ptr++;
		*ptr = (byte)Happiness;
		ptr++;
		*(short*)ptr = Morality;
		ptr += 2;
		ptr += OrganizationInfo.Serialize(ptr);
		ptr += Avatar.Serialize(ptr);
		*(short*)ptr = ClothingDisplayId;
		ptr += 2;
		*(short*)ptr = Attraction;
		ptr += 2;
		*(int*)ptr = BirthDate;
		ptr += 4;
		*(int*)ptr = DeathDate;
		ptr += 4;
		*ptr = MonkType;
		ptr++;
		if (FeatureIds != null)
		{
			int count2 = FeatureIds.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				((short*)ptr)[j] = FeatureIds[j];
			}
			ptr += 2 * count2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += BaseMainAttributes.Serialize(ptr);
		*(short*)ptr = CurrAge;
		ptr += 2;
		ptr += BaseLifeSkillQualifications.Serialize(ptr);
		ptr += BaseCombatSkillQualifications.Serialize(ptr);
		ptr += PreexistenceCharIds.Serialize(ptr);
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
		TemplateId = *(short*)ptr;
		ptr += 2;
		ptr += FullName.Deserialize(ptr);
		ptr += MonasticTitle.Deserialize(ptr);
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (TitleIds == null)
			{
				TitleIds = new List<short>(num);
			}
			else
			{
				TitleIds.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				TitleIds.Add(((short*)ptr)[i]);
			}
			ptr += 2 * num;
		}
		else
		{
			TitleIds?.Clear();
		}
		Gender = (sbyte)(*ptr);
		ptr++;
		FameType = (sbyte)(*ptr);
		ptr++;
		Happiness = (sbyte)(*ptr);
		ptr++;
		Morality = *(short*)ptr;
		ptr += 2;
		ptr += OrganizationInfo.Deserialize(ptr);
		if (Avatar == null)
		{
			Avatar = new AvatarData();
		}
		ptr += Avatar.Deserialize(ptr);
		ClothingDisplayId = *(short*)ptr;
		ptr += 2;
		Attraction = *(short*)ptr;
		ptr += 2;
		BirthDate = *(int*)ptr;
		ptr += 4;
		DeathDate = *(int*)ptr;
		ptr += 4;
		MonkType = *ptr;
		ptr++;
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (FeatureIds == null)
			{
				FeatureIds = new List<short>(num2);
			}
			else
			{
				FeatureIds.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				FeatureIds.Add(((short*)ptr)[j]);
			}
			ptr += 2 * num2;
		}
		else
		{
			FeatureIds?.Clear();
		}
		ptr += BaseMainAttributes.Deserialize(ptr);
		CurrAge = *(short*)ptr;
		ptr += 2;
		ptr += BaseLifeSkillQualifications.Deserialize(ptr);
		ptr += BaseCombatSkillQualifications.Deserialize(ptr);
		ptr += PreexistenceCharIds.Deserialize(ptr);
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public AvatarRelatedData GenerateAvatarRelatedData()
	{
		return new AvatarRelatedData
		{
			AvatarData = new AvatarData(Avatar),
			DisplayAge = CurrAge,
			ClothingDisplayId = ClothingDisplayId
		};
	}

	public bool OrgAndMonkTypeAllowMarriage()
	{
		if (MonkType == 0)
		{
			return OrganizationInfo.GetOrgMemberConfig().ChildGrade >= 0;
		}
		return false;
	}

	public sbyte GetConsummateLevel()
	{
		return OrganizationInfo.GetOrgMemberConfig().ConsummateLevel;
	}

	public short GetActualAge()
	{
		return (short)((DeathDate - BirthDate) / 12);
	}

	public bool IsCompletelyInfected()
	{
		return FeatureIds.Contains(218);
	}

	public NameRelatedData GetRawNameRelatedData()
	{
		NameRelatedData result = new NameRelatedData();
		result.CharTemplateId = TemplateId;
		result.Gender = Gender;
		result.MonkType = MonkType;
		result.FullName = FullName;
		result.OrgTemplateId = OrganizationInfo.OrgTemplateId;
		result.OrgGrade = OrganizationInfo.Grade;
		result.MonasticTitle = MonasticTitle;
		result.ExtraNameTextTemplateId = -1;
		return result;
	}

	public override string ToString()
	{
		(string, string) monasticTitleOrDisplayName = GetRawNameRelatedData().GetMonasticTitleOrDisplayName(isTaiwu: false);
		return $"{OrganizationInfo}-{monasticTitleOrDisplayName.Item1}{monasticTitleOrDisplayName.Item2}";
	}
}
