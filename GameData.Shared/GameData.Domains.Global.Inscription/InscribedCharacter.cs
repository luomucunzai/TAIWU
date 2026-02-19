using System;
using System.Collections.Generic;
using System.Text;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Global.Inscription;

[Serializable]
public class InscribedCharacter : ISerializableGameData
{
	[SerializableGameDataField]
	public long Timestamp;

	[SerializableGameDataField]
	public string Surname;

	[SerializableGameDataField]
	public string GivenName;

	[SerializableGameDataField]
	public sbyte Gender;

	[SerializableGameDataField]
	public short ActualAge;

	[SerializableGameDataField]
	public short CurrAge;

	[SerializableGameDataField]
	public short BaseMaxHealth;

	[SerializableGameDataField]
	public short Morality;

	[SerializableGameDataField]
	public OrganizationInfo OrganizationInfo;

	[SerializableGameDataField]
	public AvatarData Avatar;

	[SerializableGameDataField]
	public short ClothingDisplayId;

	[SerializableGameDataField]
	public sbyte BirthMonth;

	[SerializableGameDataField]
	public List<short> FeatureIds = new List<short>();

	[SerializableGameDataField]
	public MainAttributes BaseMainAttributes;

	[SerializableGameDataField]
	public LifeSkillShorts BaseLifeSkillQualifications;

	[SerializableGameDataField]
	public sbyte LifeSkillQualificationGrowthType;

	[SerializableGameDataField]
	public CombatSkillShorts BaseCombatSkillQualifications;

	[SerializableGameDataField]
	public sbyte CombatSkillQualificationGrowthType;

	[SerializableGameDataField(ArrayElementsCount = 2)]
	public SkillQualificationBonus[] InnateSkillQualificationBonuses;

	public AvatarRelatedData GenerateAvatarRelatedData()
	{
		return new AvatarRelatedData
		{
			AvatarData = new AvatarData(Avatar),
			DisplayAge = CurrAge,
			ClothingDisplayId = ClothingDisplayId
		};
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 186;
		num = ((Surname == null) ? (num + 2) : (num + (2 + 2 * Surname.Length)));
		num = ((GivenName == null) ? (num + 2) : (num + (2 + 2 * GivenName.Length)));
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
		*(long*)ptr = Timestamp;
		ptr += 8;
		if (Surname != null)
		{
			int length = Surname.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* surname = Surname)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)surname[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (GivenName != null)
		{
			int length2 = GivenName.Length;
			Tester.Assert(length2 <= 65535);
			*(ushort*)ptr = (ushort)length2;
			ptr += 2;
			fixed (char* givenName = GivenName)
			{
				for (int j = 0; j < length2; j++)
				{
					((short*)ptr)[j] = (short)givenName[j];
				}
			}
			ptr += 2 * length2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)Gender;
		ptr++;
		*(short*)ptr = ActualAge;
		ptr += 2;
		*(short*)ptr = CurrAge;
		ptr += 2;
		*(short*)ptr = BaseMaxHealth;
		ptr += 2;
		*(short*)ptr = Morality;
		ptr += 2;
		ptr += OrganizationInfo.Serialize(ptr);
		ptr += Avatar.Serialize(ptr);
		*(short*)ptr = ClothingDisplayId;
		ptr += 2;
		*ptr = (byte)BirthMonth;
		ptr++;
		if (FeatureIds != null)
		{
			int count = FeatureIds.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int k = 0; k < count; k++)
			{
				((short*)ptr)[k] = FeatureIds[k];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += BaseMainAttributes.Serialize(ptr);
		ptr += BaseLifeSkillQualifications.Serialize(ptr);
		*ptr = (byte)LifeSkillQualificationGrowthType;
		ptr++;
		ptr += BaseCombatSkillQualifications.Serialize(ptr);
		*ptr = (byte)CombatSkillQualificationGrowthType;
		ptr++;
		Tester.Assert(InnateSkillQualificationBonuses.Length == 2);
		for (int l = 0; l < 2; l++)
		{
			ptr += InnateSkillQualificationBonuses[l].Serialize(ptr);
		}
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
		Timestamp = *(long*)ptr;
		ptr += 8;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			int num2 = 2 * num;
			Surname = Encoding.Unicode.GetString(ptr, num2);
			ptr += num2;
		}
		else
		{
			Surname = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			int num4 = 2 * num3;
			GivenName = Encoding.Unicode.GetString(ptr, num4);
			ptr += num4;
		}
		else
		{
			GivenName = null;
		}
		Gender = (sbyte)(*ptr);
		ptr++;
		ActualAge = *(short*)ptr;
		ptr += 2;
		CurrAge = *(short*)ptr;
		ptr += 2;
		BaseMaxHealth = *(short*)ptr;
		ptr += 2;
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
		BirthMonth = (sbyte)(*ptr);
		ptr++;
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (FeatureIds == null)
			{
				FeatureIds = new List<short>(num5);
			}
			else
			{
				FeatureIds.Clear();
			}
			for (int i = 0; i < num5; i++)
			{
				FeatureIds.Add(((short*)ptr)[i]);
			}
			ptr += 2 * num5;
		}
		else
		{
			FeatureIds?.Clear();
		}
		ptr += BaseMainAttributes.Deserialize(ptr);
		ptr += BaseLifeSkillQualifications.Deserialize(ptr);
		LifeSkillQualificationGrowthType = (sbyte)(*ptr);
		ptr++;
		ptr += BaseCombatSkillQualifications.Deserialize(ptr);
		CombatSkillQualificationGrowthType = (sbyte)(*ptr);
		ptr++;
		if (InnateSkillQualificationBonuses == null || InnateSkillQualificationBonuses.Length != 2)
		{
			InnateSkillQualificationBonuses = new SkillQualificationBonus[2];
		}
		for (int j = 0; j < 2; j++)
		{
			SkillQualificationBonus skillQualificationBonus = default(SkillQualificationBonus);
			ptr += skillQualificationBonus.Deserialize(ptr);
			InnateSkillQualificationBonuses[j] = skillQualificationBonus;
		}
		int num6 = (int)(ptr - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}

	public short CalcMaxHealth(short actualAge)
	{
		int num = 0;
		if (FeatureIds != null)
		{
			int i = 0;
			for (int count = FeatureIds.Count; i < count; i++)
			{
				short index = FeatureIds[i];
				num += CharacterFeature.Instance[index].MaxHealthPercentBonus;
			}
		}
		int num2 = BaseMaxHealth * (100 + num) / 100;
		return (short)((num2 >= 0) ? num2 : 0);
	}

	public short CalcAttraction(short actualAge, short clothingDisplayId)
	{
		if (CurrAge < 16)
		{
			return GlobalConfig.Instance.ImmaturityAttraction;
		}
		short characterAge = actualAge;
		int charm = Avatar.GetCharm(characterAge, (byte)clothingDisplayId);
		charm += GetCommonPropertyBonus(ECharacterPropertyReferencedType.Attraction);
		if (clothingDisplayId <= 0)
		{
			charm /= 2;
		}
		return (short)MathUtils.Clamp(charm, 0, 900);
	}

	public unsafe MainAttributes CalcMaxMainAttributes(short actualAge)
	{
		MainAttributes baseMainAttributes = BaseMainAttributes;
		for (int i = 0; i < 6; i++)
		{
			ref short reference = ref baseMainAttributes.Items[i];
			reference += (short)GetCommonPropertyBonus((ECharacterPropertyReferencedType)(0 + i));
		}
		short num = actualAge;
		int index = ((num <= 100) ? num : 100);
		MainAttributes mainAttributes = AgeEffect.Instance[index].MainAttributes;
		for (int j = 0; j < 6; j++)
		{
			baseMainAttributes.Items[j] = (short)(baseMainAttributes.Items[j] * mainAttributes.Items[j] / 100);
		}
		for (int k = 0; k < 6; k++)
		{
			baseMainAttributes.Items[k] = Math.Clamp(baseMainAttributes.Items[k], GlobalConfig.Instance.MinValueOfMaxMainAttributes, GlobalConfig.Instance.MaxValueOfMaxMainAttributes);
		}
		return baseMainAttributes;
	}

	public unsafe LifeSkillShorts CalcLifeSkillQualifications(short actualAge)
	{
		LifeSkillShorts baseLifeSkillQualifications = BaseLifeSkillQualifications;
		for (int i = 0; i < 16; i++)
		{
			ref short reference = ref baseLifeSkillQualifications.Items[i];
			reference += (short)GetCommonPropertyBonus((ECharacterPropertyReferencedType)(34 + i));
		}
		int j = 0;
		for (int num = InnateSkillQualificationBonuses.Length; j < num; j++)
		{
			SkillQualificationBonus skillQualificationBonus = InnateSkillQualificationBonuses[j];
			var (b, b2) = skillQualificationBonus.GetSkillGroupAndType();
			if (b == 0)
			{
				ref short reference2 = ref baseLifeSkillQualifications.Items[b2];
				reference2 += skillQualificationBonus.Bonus;
			}
		}
		int num2 = ((actualAge <= 100) ? actualAge : 100);
		AgeEffectItem ageEffectItem = AgeEffect.Instance[num2];
		sbyte b3 = LifeSkillQualificationGrowthType switch
		{
			0 => ageEffectItem.SkillQualificationAverage, 
			1 => ageEffectItem.SkillQualificationPrecocious, 
			2 => ageEffectItem.SkillQualificationLateBlooming, 
			_ => throw new Exception($"Unsupported LifeSkillQualificationGrowthType: {LifeSkillQualificationGrowthType}"), 
		};
		for (int k = 0; k < 16; k++)
		{
			ref short reference3 = ref baseLifeSkillQualifications.Items[k];
			reference3 += b3;
		}
		if (num2 < 16)
		{
			for (int l = 0; l < 16; l++)
			{
				baseLifeSkillQualifications.Items[l] = (short)(baseLifeSkillQualifications.Items[l] * num2 / 16);
			}
		}
		for (int m = 0; m < 16; m++)
		{
			if (baseLifeSkillQualifications.Items[m] < 0)
			{
				baseLifeSkillQualifications.Items[m] = 0;
			}
		}
		return baseLifeSkillQualifications;
	}

	public unsafe CombatSkillShorts CalcCombatSkillQualifications(short actualAge)
	{
		CombatSkillShorts baseCombatSkillQualifications = BaseCombatSkillQualifications;
		for (int i = 0; i < 14; i++)
		{
			ref short reference = ref baseCombatSkillQualifications.Items[i];
			reference += (short)GetCommonPropertyBonus((ECharacterPropertyReferencedType)(66 + i));
		}
		int j = 0;
		for (int num = InnateSkillQualificationBonuses.Length; j < num; j++)
		{
			SkillQualificationBonus skillQualificationBonus = InnateSkillQualificationBonuses[j];
			var (b, b2) = skillQualificationBonus.GetSkillGroupAndType();
			if (b == 1)
			{
				ref short reference2 = ref baseCombatSkillQualifications.Items[b2];
				reference2 += skillQualificationBonus.Bonus;
			}
		}
		int num2 = ((actualAge <= 100) ? actualAge : 100);
		AgeEffectItem ageEffectItem = AgeEffect.Instance[num2];
		sbyte b3 = CombatSkillQualificationGrowthType switch
		{
			0 => ageEffectItem.SkillQualificationAverage, 
			1 => ageEffectItem.SkillQualificationPrecocious, 
			2 => ageEffectItem.SkillQualificationLateBlooming, 
			_ => throw new Exception($"Unsupported CombatSkillQualificationGrowthType: {CombatSkillQualificationGrowthType}"), 
		};
		for (int k = 0; k < 14; k++)
		{
			ref short reference3 = ref baseCombatSkillQualifications.Items[k];
			reference3 += b3;
		}
		if (num2 < 16)
		{
			for (int l = 0; l < 14; l++)
			{
				baseCombatSkillQualifications.Items[l] = (short)(baseCombatSkillQualifications.Items[l] * num2 / 16);
			}
		}
		for (int m = 0; m < 14; m++)
		{
			if (baseCombatSkillQualifications.Items[m] < 0)
			{
				baseCombatSkillQualifications.Items[m] = 0;
			}
		}
		return baseCombatSkillQualifications;
	}

	private int GetCommonPropertyBonus(ECharacterPropertyReferencedType propertyType)
	{
		if (FeatureIds != null)
		{
			return CharacterFeature.GetCharacterPropertyBonus(FeatureIds, propertyType);
		}
		return 0;
	}
}
