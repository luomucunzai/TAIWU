using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Building;

public class PossessionPreview : ISerializableGameData
{
	[SerializableGameDataField]
	public byte Result;

	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public short Age;

	[SerializableGameDataField]
	public int BirthDate;

	[SerializableGameDataField]
	public short Health;

	[SerializableGameDataField]
	public sbyte Gender;

	[SerializableGameDataField]
	public short BaseMorality;

	[SerializableGameDataField]
	public sbyte Happiness;

	[SerializableGameDataField]
	public sbyte Fame;

	[SerializableGameDataField]
	public Personalities Personalities;

	[SerializableGameDataField]
	public short BirthFeatureId;

	[SerializableGameDataField]
	public List<short> FeatureIds;

	[SerializableGameDataField]
	public MainAttributes BaseMainAttributes;

	[SerializableGameDataField]
	public LifeSkillShorts BaseLifeSkillQualifications;

	[SerializableGameDataField]
	public CombatSkillShorts BaseCombatSkillQualifications;

	[SerializableGameDataField]
	public sbyte ConsummateLevel;

	[SerializableGameDataField]
	public NeiliAllocation NeiliAllocation;

	[SerializableGameDataField]
	public int CurrNeili;

	[SerializableGameDataField]
	public NeiliProportionOfFiveElements BaseNeiliProportionOfFiveElements;

	[SerializableGameDataField]
	public CharacterSamsaraData CharacterSamsaraData;

	public PossessionPreview()
	{
	}

	public PossessionPreview(PossessionPreview other)
	{
		Result = other.Result;
		Id = other.Id;
		Age = other.Age;
		BirthDate = other.BirthDate;
		Health = other.Health;
		Gender = other.Gender;
		BaseMorality = other.BaseMorality;
		Happiness = other.Happiness;
		Fame = other.Fame;
		Personalities = other.Personalities;
		BirthFeatureId = other.BirthFeatureId;
		FeatureIds = ((other.FeatureIds == null) ? null : new List<short>(other.FeatureIds));
		BaseMainAttributes = other.BaseMainAttributes;
		BaseLifeSkillQualifications = other.BaseLifeSkillQualifications;
		BaseCombatSkillQualifications = other.BaseCombatSkillQualifications;
		ConsummateLevel = other.ConsummateLevel;
		NeiliAllocation = other.NeiliAllocation;
		CurrNeili = other.CurrNeili;
		BaseNeiliProportionOfFiveElements = other.BaseNeiliProportionOfFiveElements;
		CharacterSamsaraData = new CharacterSamsaraData(other.CharacterSamsaraData);
	}

	public void Assign(PossessionPreview other)
	{
		Result = other.Result;
		Id = other.Id;
		Age = other.Age;
		BirthDate = other.BirthDate;
		Health = other.Health;
		Gender = other.Gender;
		BaseMorality = other.BaseMorality;
		Happiness = other.Happiness;
		Fame = other.Fame;
		Personalities = other.Personalities;
		BirthFeatureId = other.BirthFeatureId;
		FeatureIds = ((other.FeatureIds == null) ? null : new List<short>(other.FeatureIds));
		BaseMainAttributes = other.BaseMainAttributes;
		BaseLifeSkillQualifications = other.BaseLifeSkillQualifications;
		BaseCombatSkillQualifications = other.BaseCombatSkillQualifications;
		ConsummateLevel = other.ConsummateLevel;
		NeiliAllocation = other.NeiliAllocation;
		CurrNeili = other.CurrNeili;
		BaseNeiliProportionOfFiveElements = other.BaseNeiliProportionOfFiveElements;
		CharacterSamsaraData = new CharacterSamsaraData(other.CharacterSamsaraData);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 121;
		num = ((FeatureIds == null) ? (num + 2) : (num + (2 + 2 * FeatureIds.Count)));
		num = ((CharacterSamsaraData == null) ? (num + 2) : (num + (2 + CharacterSamsaraData.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = Result;
		ptr++;
		*(int*)ptr = Id;
		ptr += 4;
		*(short*)ptr = Age;
		ptr += 2;
		*(int*)ptr = BirthDate;
		ptr += 4;
		*(short*)ptr = Health;
		ptr += 2;
		*ptr = (byte)Gender;
		ptr++;
		*(short*)ptr = BaseMorality;
		ptr += 2;
		*ptr = (byte)Happiness;
		ptr++;
		*ptr = (byte)Fame;
		ptr++;
		ptr += Personalities.Serialize(ptr);
		*(short*)ptr = BirthFeatureId;
		ptr += 2;
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
		ptr += BaseMainAttributes.Serialize(ptr);
		ptr += BaseLifeSkillQualifications.Serialize(ptr);
		ptr += BaseCombatSkillQualifications.Serialize(ptr);
		*ptr = (byte)ConsummateLevel;
		ptr++;
		ptr += NeiliAllocation.Serialize(ptr);
		*(int*)ptr = CurrNeili;
		ptr += 4;
		ptr += BaseNeiliProportionOfFiveElements.Serialize(ptr);
		if (CharacterSamsaraData != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = CharacterSamsaraData.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Result = *ptr;
		ptr++;
		Id = *(int*)ptr;
		ptr += 4;
		Age = *(short*)ptr;
		ptr += 2;
		BirthDate = *(int*)ptr;
		ptr += 4;
		Health = *(short*)ptr;
		ptr += 2;
		Gender = (sbyte)(*ptr);
		ptr++;
		BaseMorality = *(short*)ptr;
		ptr += 2;
		Happiness = (sbyte)(*ptr);
		ptr++;
		Fame = (sbyte)(*ptr);
		ptr++;
		ptr += Personalities.Deserialize(ptr);
		BirthFeatureId = *(short*)ptr;
		ptr += 2;
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
		ptr += BaseMainAttributes.Deserialize(ptr);
		ptr += BaseLifeSkillQualifications.Deserialize(ptr);
		ptr += BaseCombatSkillQualifications.Deserialize(ptr);
		ConsummateLevel = (sbyte)(*ptr);
		ptr++;
		ptr += NeiliAllocation.Deserialize(ptr);
		CurrNeili = *(int*)ptr;
		ptr += 4;
		ptr += BaseNeiliProportionOfFiveElements.Deserialize(ptr);
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (CharacterSamsaraData == null)
			{
				CharacterSamsaraData = new CharacterSamsaraData();
			}
			ptr += CharacterSamsaraData.Deserialize(ptr);
		}
		else
		{
			CharacterSamsaraData = null;
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
