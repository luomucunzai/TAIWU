using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Display;

[SerializableGameData(NotRestrictCollectionSerializedSize = true)]
public class GroupCharDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public int CharacterId;

	[SerializableGameDataField]
	public short CharacterTemplateId;

	[SerializableGameDataField]
	public NameRelatedData NameData;

	[SerializableGameDataField]
	public short CurrAge;

	[SerializableGameDataField]
	public short ActualAge;

	[SerializableGameDataField]
	public int BirthDate;

	[SerializableGameDataField]
	public short Health;

	[SerializableGameDataField]
	public short MaxLeftHealth;

	[SerializableGameDataField]
	public sbyte DefeatMarkCount;

	[SerializableGameDataField]
	public short Charm;

	[SerializableGameDataField]
	public sbyte BehaviorType;

	[SerializableGameDataField]
	public sbyte Fame;

	[SerializableGameDataField]
	public sbyte Happiness;

	[SerializableGameDataField]
	public short FavorabilityToTaiwu;

	[SerializableGameDataField]
	public short PreexistenceCharCount;

	[SerializableGameDataField]
	public int AttackMedal;

	[SerializableGameDataField]
	public int DefenceMedal;

	[SerializableGameDataField]
	public int WisdomMedal;

	[SerializableGameDataField]
	public MainAttributes MaxMainAttributes;

	[SerializableGameDataField]
	public OuterAndInnerInts Penetrations;

	[SerializableGameDataField]
	public OuterAndInnerInts PenetrationResists;

	[SerializableGameDataField]
	public HitOrAvoidInts HitValues;

	[SerializableGameDataField]
	public HitOrAvoidInts AvoidValues;

	[SerializableGameDataField]
	public short DisorderOfQi;

	[SerializableGameDataField]
	public LifeSkillShorts LifeSkillQualifications;

	[SerializableGameDataField]
	public sbyte LifeSkillGrowthType;

	[SerializableGameDataField]
	public CombatSkillShorts CombatSkillQualifications;

	[SerializableGameDataField]
	public sbyte CombatSkillGrowthType;

	[SerializableGameDataField]
	public Personalities Personalities;

	[SerializableGameDataField]
	public ResourceInts Resources;

	[SerializableGameDataField]
	public int CurrInventoryLoad;

	[SerializableGameDataField]
	public int MaxInventoryLoad;

	[SerializableGameDataField]
	public sbyte KidnapCount;

	[SerializableGameDataField]
	public sbyte Gender;

	[SerializableGameDataField]
	public short PhysiologicalAge;

	[SerializableGameDataField]
	public short ClothDisplayId;

	[SerializableGameDataField]
	public bool FaceVisible;

	[SerializableGameDataField]
	public byte CreatingType;

	[SerializableGameDataField]
	public SByteList Command;

	[SerializableGameDataField]
	public SByteList AdvancedCommand;

	[SerializableGameDataField]
	public sbyte ConsummateLevel;

	[SerializableGameDataField]
	public bool IsSpecialGroupMember;

	[SerializableGameDataField]
	public bool IsInteractedWithTaiwu;

	public GroupCharDisplayData()
	{
	}

	public GroupCharDisplayData(GroupCharDisplayData other)
	{
		CharacterId = other.CharacterId;
		CharacterTemplateId = other.CharacterTemplateId;
		NameData = other.NameData;
		CurrAge = other.CurrAge;
		ActualAge = other.ActualAge;
		BirthDate = other.BirthDate;
		Health = other.Health;
		MaxLeftHealth = other.MaxLeftHealth;
		DefeatMarkCount = other.DefeatMarkCount;
		Charm = other.Charm;
		BehaviorType = other.BehaviorType;
		Fame = other.Fame;
		Happiness = other.Happiness;
		FavorabilityToTaiwu = other.FavorabilityToTaiwu;
		PreexistenceCharCount = other.PreexistenceCharCount;
		AttackMedal = other.AttackMedal;
		DefenceMedal = other.DefenceMedal;
		WisdomMedal = other.WisdomMedal;
		MaxMainAttributes = other.MaxMainAttributes;
		Penetrations = other.Penetrations;
		PenetrationResists = other.PenetrationResists;
		HitValues = other.HitValues;
		AvoidValues = other.AvoidValues;
		DisorderOfQi = other.DisorderOfQi;
		LifeSkillQualifications = other.LifeSkillQualifications;
		LifeSkillGrowthType = other.LifeSkillGrowthType;
		CombatSkillQualifications = other.CombatSkillQualifications;
		CombatSkillGrowthType = other.CombatSkillGrowthType;
		Personalities = other.Personalities;
		Resources = other.Resources;
		CurrInventoryLoad = other.CurrInventoryLoad;
		MaxInventoryLoad = other.MaxInventoryLoad;
		KidnapCount = other.KidnapCount;
		Gender = other.Gender;
		PhysiologicalAge = other.PhysiologicalAge;
		ClothDisplayId = other.ClothDisplayId;
		FaceVisible = other.FaceVisible;
		CreatingType = other.CreatingType;
		Command = new SByteList(other.Command);
		AdvancedCommand = new SByteList(other.AdvancedCommand);
		ConsummateLevel = other.ConsummateLevel;
		IsSpecialGroupMember = other.IsSpecialGroupMember;
		IsInteractedWithTaiwu = other.IsInteractedWithTaiwu;
	}

	public void Assign(GroupCharDisplayData other)
	{
		CharacterId = other.CharacterId;
		CharacterTemplateId = other.CharacterTemplateId;
		NameData = other.NameData;
		CurrAge = other.CurrAge;
		ActualAge = other.ActualAge;
		BirthDate = other.BirthDate;
		Health = other.Health;
		MaxLeftHealth = other.MaxLeftHealth;
		DefeatMarkCount = other.DefeatMarkCount;
		Charm = other.Charm;
		BehaviorType = other.BehaviorType;
		Fame = other.Fame;
		Happiness = other.Happiness;
		FavorabilityToTaiwu = other.FavorabilityToTaiwu;
		PreexistenceCharCount = other.PreexistenceCharCount;
		AttackMedal = other.AttackMedal;
		DefenceMedal = other.DefenceMedal;
		WisdomMedal = other.WisdomMedal;
		MaxMainAttributes = other.MaxMainAttributes;
		Penetrations = other.Penetrations;
		PenetrationResists = other.PenetrationResists;
		HitValues = other.HitValues;
		AvoidValues = other.AvoidValues;
		DisorderOfQi = other.DisorderOfQi;
		LifeSkillQualifications = other.LifeSkillQualifications;
		LifeSkillGrowthType = other.LifeSkillGrowthType;
		CombatSkillQualifications = other.CombatSkillQualifications;
		CombatSkillGrowthType = other.CombatSkillGrowthType;
		Personalities = other.Personalities;
		Resources = other.Resources;
		CurrInventoryLoad = other.CurrInventoryLoad;
		MaxInventoryLoad = other.MaxInventoryLoad;
		KidnapCount = other.KidnapCount;
		Gender = other.Gender;
		PhysiologicalAge = other.PhysiologicalAge;
		ClothDisplayId = other.ClothDisplayId;
		FaceVisible = other.FaceVisible;
		CreatingType = other.CreatingType;
		Command = new SByteList(other.Command);
		AdvancedCommand = new SByteList(other.AdvancedCommand);
		ConsummateLevel = other.ConsummateLevel;
		IsSpecialGroupMember = other.IsSpecialGroupMember;
		IsInteractedWithTaiwu = other.IsInteractedWithTaiwu;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 255;
		num += Command.GetSerializedSize();
		num += AdvancedCommand.GetSerializedSize();
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = CharacterId;
		ptr += 4;
		*(short*)ptr = CharacterTemplateId;
		ptr += 2;
		ptr += NameData.Serialize(ptr);
		*(short*)ptr = CurrAge;
		ptr += 2;
		*(short*)ptr = ActualAge;
		ptr += 2;
		*(int*)ptr = BirthDate;
		ptr += 4;
		*(short*)ptr = Health;
		ptr += 2;
		*(short*)ptr = MaxLeftHealth;
		ptr += 2;
		*ptr = (byte)DefeatMarkCount;
		ptr++;
		*(short*)ptr = Charm;
		ptr += 2;
		*ptr = (byte)BehaviorType;
		ptr++;
		*ptr = (byte)Fame;
		ptr++;
		*ptr = (byte)Happiness;
		ptr++;
		*(short*)ptr = FavorabilityToTaiwu;
		ptr += 2;
		*(short*)ptr = PreexistenceCharCount;
		ptr += 2;
		*(int*)ptr = AttackMedal;
		ptr += 4;
		*(int*)ptr = DefenceMedal;
		ptr += 4;
		*(int*)ptr = WisdomMedal;
		ptr += 4;
		ptr += MaxMainAttributes.Serialize(ptr);
		ptr += Penetrations.Serialize(ptr);
		ptr += PenetrationResists.Serialize(ptr);
		ptr += HitValues.Serialize(ptr);
		ptr += AvoidValues.Serialize(ptr);
		*(short*)ptr = DisorderOfQi;
		ptr += 2;
		ptr += LifeSkillQualifications.Serialize(ptr);
		*ptr = (byte)LifeSkillGrowthType;
		ptr++;
		ptr += CombatSkillQualifications.Serialize(ptr);
		*ptr = (byte)CombatSkillGrowthType;
		ptr++;
		ptr += Personalities.Serialize(ptr);
		ptr += Resources.Serialize(ptr);
		*(int*)ptr = CurrInventoryLoad;
		ptr += 4;
		*(int*)ptr = MaxInventoryLoad;
		ptr += 4;
		*ptr = (byte)KidnapCount;
		ptr++;
		*ptr = (byte)Gender;
		ptr++;
		*(short*)ptr = PhysiologicalAge;
		ptr += 2;
		*(short*)ptr = ClothDisplayId;
		ptr += 2;
		*ptr = (FaceVisible ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = CreatingType;
		ptr++;
		int num = Command.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		int num2 = AdvancedCommand.Serialize(ptr);
		ptr += num2;
		Tester.Assert(num2 <= 65535);
		*ptr = (byte)ConsummateLevel;
		ptr++;
		*ptr = (IsSpecialGroupMember ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (IsInteractedWithTaiwu ? ((byte)1) : ((byte)0));
		ptr++;
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		CharacterId = *(int*)ptr;
		ptr += 4;
		CharacterTemplateId = *(short*)ptr;
		ptr += 2;
		ptr += NameData.Deserialize(ptr);
		CurrAge = *(short*)ptr;
		ptr += 2;
		ActualAge = *(short*)ptr;
		ptr += 2;
		BirthDate = *(int*)ptr;
		ptr += 4;
		Health = *(short*)ptr;
		ptr += 2;
		MaxLeftHealth = *(short*)ptr;
		ptr += 2;
		DefeatMarkCount = (sbyte)(*ptr);
		ptr++;
		Charm = *(short*)ptr;
		ptr += 2;
		BehaviorType = (sbyte)(*ptr);
		ptr++;
		Fame = (sbyte)(*ptr);
		ptr++;
		Happiness = (sbyte)(*ptr);
		ptr++;
		FavorabilityToTaiwu = *(short*)ptr;
		ptr += 2;
		PreexistenceCharCount = *(short*)ptr;
		ptr += 2;
		AttackMedal = *(int*)ptr;
		ptr += 4;
		DefenceMedal = *(int*)ptr;
		ptr += 4;
		WisdomMedal = *(int*)ptr;
		ptr += 4;
		ptr += MaxMainAttributes.Deserialize(ptr);
		ptr += Penetrations.Deserialize(ptr);
		ptr += PenetrationResists.Deserialize(ptr);
		ptr += HitValues.Deserialize(ptr);
		ptr += AvoidValues.Deserialize(ptr);
		DisorderOfQi = *(short*)ptr;
		ptr += 2;
		ptr += LifeSkillQualifications.Deserialize(ptr);
		LifeSkillGrowthType = (sbyte)(*ptr);
		ptr++;
		ptr += CombatSkillQualifications.Deserialize(ptr);
		CombatSkillGrowthType = (sbyte)(*ptr);
		ptr++;
		ptr += Personalities.Deserialize(ptr);
		ptr += Resources.Deserialize(ptr);
		CurrInventoryLoad = *(int*)ptr;
		ptr += 4;
		MaxInventoryLoad = *(int*)ptr;
		ptr += 4;
		KidnapCount = (sbyte)(*ptr);
		ptr++;
		Gender = (sbyte)(*ptr);
		ptr++;
		PhysiologicalAge = *(short*)ptr;
		ptr += 2;
		ClothDisplayId = *(short*)ptr;
		ptr += 2;
		FaceVisible = *ptr != 0;
		ptr++;
		CreatingType = *ptr;
		ptr++;
		ptr += Command.Deserialize(ptr);
		ptr += AdvancedCommand.Deserialize(ptr);
		ConsummateLevel = (sbyte)(*ptr);
		ptr++;
		IsSpecialGroupMember = *ptr != 0;
		ptr++;
		IsInteractedWithTaiwu = *ptr != 0;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
