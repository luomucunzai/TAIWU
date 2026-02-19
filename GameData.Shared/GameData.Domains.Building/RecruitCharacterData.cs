using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Building;

[SerializableGameData(IsExtensible = true, NotRestrictCollectionSerializedSize = true)]
public class RecruitCharacterData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort TemplateId = 0;

		public const ushort PeopleLevel = 1;

		public const ushort Age = 2;

		public const ushort FullName = 3;

		public const ushort BaseAttraction = 4;

		public const ushort AvatarData = 5;

		public const ushort MainAttributes = 6;

		public const ushort FeatureIds = 7;

		public const ushort Gender = 8;

		public const ushort Transgender = 9;

		public const ushort CombatSkillQualifications = 10;

		public const ushort CombatSkillQualificationGrowthType = 11;

		public const ushort LifeSkillQualifications = 12;

		public const ushort LifeSkillQualificationGrowthType = 13;

		public const ushort CalculatedPersonalities = 14;

		public const ushort ClothingTemplateId = 15;

		public const ushort TeammateCommands = 16;

		public const ushort BirthMonth = 17;

		public const ushort AttractionBonus = 18;

		public const ushort Count = 19;

		public static readonly string[] FieldId2FieldName = new string[19]
		{
			"TemplateId", "PeopleLevel", "Age", "FullName", "BaseAttraction", "AvatarData", "MainAttributes", "FeatureIds", "Gender", "Transgender",
			"CombatSkillQualifications", "CombatSkillQualificationGrowthType", "LifeSkillQualifications", "LifeSkillQualificationGrowthType", "CalculatedPersonalities", "ClothingTemplateId", "TeammateCommands", "BirthMonth", "AttractionBonus"
		};
	}

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public sbyte PeopleLevel;

	[SerializableGameDataField]
	public short Age;

	[SerializableGameDataField]
	public FullName FullName;

	[SerializableGameDataField]
	public short BaseAttraction;

	[SerializableGameDataField]
	public AvatarData AvatarData;

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
	public Personalities CalculatedPersonalities;

	[SerializableGameDataField]
	public short ClothingTemplateId;

	[SerializableGameDataField]
	public List<sbyte> TeammateCommands;

	[SerializableGameDataField]
	public sbyte BirthMonth;

	[SerializableGameDataField]
	public short FinalAttraction;

	public unsafe void Recalculate()
	{
		for (int i = 0; i < 7; i++)
		{
			ECharacterPropertyReferencedType property = (ECharacterPropertyReferencedType)(94 + i);
			int num = 10;
			if (FeatureIds != null)
			{
				foreach (short featureId in FeatureIds)
				{
					int characterPropertyBonus = CharacterFeature.GetCharacterPropertyBonus(featureId, property);
					num += characterPropertyBonus;
				}
			}
			CalculatedPersonalities.Items[i] = (sbyte)Math.Clamp(num, 0, 100);
		}
	}

	public unsafe short GetBaseMorality()
	{
		long num = BaseAttraction % 10;
		for (int i = 0; i < 6; i++)
		{
			num += MainAttributes.Items[i];
		}
		num = (1664525 * num + 1013904223) % 4294967296L;
		return (short)Math.Clamp(num % 1001 + -500, -500L, 500L);
	}

	public AvatarRelatedData GenerateAvatarRelatedData()
	{
		ClothingItem item = Clothing.Instance.GetItem(ClothingTemplateId);
		return new AvatarRelatedData
		{
			AvatarData = AvatarData,
			ClothingDisplayId = (item?.DisplayId ?? 0),
			DisplayAge = Age,
			HasNewGoods = false
		};
	}

	public void GetTeammateCommands(IList<sbyte> receiver)
	{
		if (TeammateCommands == null)
		{
			return;
		}
		foreach (sbyte teammateCommand in TeammateCommands)
		{
			receiver.Add(teammateCommand);
		}
	}

	public RecruitCharacterData()
	{
	}

	public RecruitCharacterData(RecruitCharacterData other)
	{
		TemplateId = other.TemplateId;
		PeopleLevel = other.PeopleLevel;
		Age = other.Age;
		FullName = other.FullName;
		BaseAttraction = other.BaseAttraction;
		AvatarData = new AvatarData(other.AvatarData);
		MainAttributes = other.MainAttributes;
		FeatureIds = ((other.FeatureIds == null) ? null : new List<short>(other.FeatureIds));
		Gender = other.Gender;
		Transgender = other.Transgender;
		CombatSkillQualifications = other.CombatSkillQualifications;
		CombatSkillQualificationGrowthType = other.CombatSkillQualificationGrowthType;
		LifeSkillQualifications = other.LifeSkillQualifications;
		LifeSkillQualificationGrowthType = other.LifeSkillQualificationGrowthType;
		CalculatedPersonalities = other.CalculatedPersonalities;
		ClothingTemplateId = other.ClothingTemplateId;
		TeammateCommands = ((other.TeammateCommands == null) ? null : new List<sbyte>(other.TeammateCommands));
		BirthMonth = other.BirthMonth;
		FinalAttraction = other.FinalAttraction;
	}

	public void Assign(RecruitCharacterData other)
	{
		TemplateId = other.TemplateId;
		PeopleLevel = other.PeopleLevel;
		Age = other.Age;
		FullName = other.FullName;
		BaseAttraction = other.BaseAttraction;
		AvatarData = new AvatarData(other.AvatarData);
		MainAttributes = other.MainAttributes;
		FeatureIds = ((other.FeatureIds == null) ? null : new List<short>(other.FeatureIds));
		Gender = other.Gender;
		Transgender = other.Transgender;
		CombatSkillQualifications = other.CombatSkillQualifications;
		CombatSkillQualificationGrowthType = other.CombatSkillQualificationGrowthType;
		LifeSkillQualifications = other.LifeSkillQualifications;
		LifeSkillQualificationGrowthType = other.LifeSkillQualificationGrowthType;
		CalculatedPersonalities = other.CalculatedPersonalities;
		ClothingTemplateId = other.ClothingTemplateId;
		TeammateCommands = ((other.TeammateCommands == null) ? null : new List<sbyte>(other.TeammateCommands));
		BirthMonth = other.BirthMonth;
		FinalAttraction = other.FinalAttraction;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 184;
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
		*(short*)ptr = 19;
		ptr += 2;
		*(short*)ptr = TemplateId;
		ptr += 2;
		*ptr = (byte)PeopleLevel;
		ptr++;
		*(short*)ptr = Age;
		ptr += 2;
		ptr += FullName.Serialize(ptr);
		*(short*)ptr = BaseAttraction;
		ptr += 2;
		ptr += AvatarData.Serialize(ptr);
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
		ptr += CalculatedPersonalities.Serialize(ptr);
		*(short*)ptr = ClothingTemplateId;
		ptr += 2;
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
		*ptr = (byte)BirthMonth;
		ptr++;
		*(short*)ptr = FinalAttraction;
		ptr += 2;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			TemplateId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 1)
		{
			PeopleLevel = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 2)
		{
			Age = *(short*)ptr;
			ptr += 2;
		}
		if (num > 3)
		{
			ptr += FullName.Deserialize(ptr);
		}
		if (num > 4)
		{
			BaseAttraction = *(short*)ptr;
			ptr += 2;
		}
		if (num > 5)
		{
			if (AvatarData == null)
			{
				AvatarData = new AvatarData();
			}
			ptr += AvatarData.Deserialize(ptr);
		}
		if (num > 6)
		{
			ptr += MainAttributes.Deserialize(ptr);
		}
		if (num > 7)
		{
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
				for (int i = 0; i < num2; i++)
				{
					FeatureIds.Add(((short*)ptr)[i]);
				}
				ptr += 2 * num2;
			}
			else
			{
				FeatureIds?.Clear();
			}
		}
		if (num > 8)
		{
			Gender = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 9)
		{
			Transgender = *ptr != 0;
			ptr++;
		}
		if (num > 10)
		{
			ptr += CombatSkillQualifications.Deserialize(ptr);
		}
		if (num > 11)
		{
			CombatSkillQualificationGrowthType = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 12)
		{
			ptr += LifeSkillQualifications.Deserialize(ptr);
		}
		if (num > 13)
		{
			LifeSkillQualificationGrowthType = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 14)
		{
			ptr += CalculatedPersonalities.Deserialize(ptr);
		}
		if (num > 15)
		{
			ClothingTemplateId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 16)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				if (TeammateCommands == null)
				{
					TeammateCommands = new List<sbyte>(num3);
				}
				else
				{
					TeammateCommands.Clear();
				}
				for (int j = 0; j < num3; j++)
				{
					TeammateCommands.Add((sbyte)ptr[j]);
				}
				ptr += (int)num3;
			}
			else
			{
				TeammateCommands?.Clear();
			}
		}
		if (num > 17)
		{
			BirthMonth = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 18)
		{
			FinalAttraction = *(short*)ptr;
			ptr += 2;
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
