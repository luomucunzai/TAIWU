using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Domains.Taiwu.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu;

[SerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public struct VillagerStatusDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public int CharacterId;

	[SerializableGameDataField]
	public NameRelatedData Name;

	[SerializableGameDataField]
	public short CurrAge;

	[SerializableGameDataField]
	public short Health;

	[SerializableGameDataField]
	public short MaxLeftHealth;

	[SerializableGameDataField]
	public sbyte Gender;

	[SerializableGameDataField]
	public sbyte BehaviorType;

	[SerializableGameDataField]
	public short Happiness;

	[SerializableGameDataField]
	public short FavorabilityToTaiwu;

	[SerializableGameDataField]
	public sbyte Fame;

	[SerializableGameDataField]
	public byte LivingStatus;

	[SerializableGameDataField]
	public byte WorkStatus;

	[SerializableGameDataField]
	public short PhysiologicalAge;

	[SerializableGameDataField]
	public short ClothDisplayId;

	[SerializableGameDataField]
	public bool FaceVisible;

	[SerializableGameDataField]
	public byte CreatingType;

	[SerializableGameDataField]
	public int BirthDate;

	[SerializableGameDataField]
	public sbyte DefeatMarkCount;

	[SerializableGameDataField]
	public short Charm;

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
	public LifeSkillShorts LifeSkillAttainment;

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
	public OrganizationInfo OrgInfo;

	[SerializableGameDataField]
	public short RoleTemplateId;

	[SerializableGameDataField]
	public VillagerRoleArrangementDisplayDataWrapper ArrangementDisplayData;

	[SerializableGameDataField]
	public short TrappedAreaId;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 295;
		num = ((ArrangementDisplayData == null) ? (num + 2) : (num + (2 + ArrangementDisplayData.GetSerializedSize())));
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
		ptr += Name.Serialize(ptr);
		*(short*)ptr = CurrAge;
		ptr += 2;
		*(short*)ptr = Health;
		ptr += 2;
		*(short*)ptr = MaxLeftHealth;
		ptr += 2;
		*ptr = (byte)Gender;
		ptr++;
		*ptr = (byte)BehaviorType;
		ptr++;
		*(short*)ptr = Happiness;
		ptr += 2;
		*(short*)ptr = FavorabilityToTaiwu;
		ptr += 2;
		*ptr = (byte)Fame;
		ptr++;
		*ptr = LivingStatus;
		ptr++;
		*ptr = WorkStatus;
		ptr++;
		*(short*)ptr = PhysiologicalAge;
		ptr += 2;
		*(short*)ptr = ClothDisplayId;
		ptr += 2;
		*ptr = (FaceVisible ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = CreatingType;
		ptr++;
		*(int*)ptr = BirthDate;
		ptr += 4;
		*ptr = (byte)DefeatMarkCount;
		ptr++;
		*(short*)ptr = Charm;
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
		ptr += LifeSkillAttainment.Serialize(ptr);
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
		ptr += OrgInfo.Serialize(ptr);
		*(short*)ptr = RoleTemplateId;
		ptr += 2;
		if (ArrangementDisplayData != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = ArrangementDisplayData.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(short*)ptr = TrappedAreaId;
		ptr += 2;
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
		CharacterId = *(int*)ptr;
		ptr += 4;
		ptr += Name.Deserialize(ptr);
		CurrAge = *(short*)ptr;
		ptr += 2;
		Health = *(short*)ptr;
		ptr += 2;
		MaxLeftHealth = *(short*)ptr;
		ptr += 2;
		Gender = (sbyte)(*ptr);
		ptr++;
		BehaviorType = (sbyte)(*ptr);
		ptr++;
		Happiness = *(short*)ptr;
		ptr += 2;
		FavorabilityToTaiwu = *(short*)ptr;
		ptr += 2;
		Fame = (sbyte)(*ptr);
		ptr++;
		LivingStatus = *ptr;
		ptr++;
		WorkStatus = *ptr;
		ptr++;
		PhysiologicalAge = *(short*)ptr;
		ptr += 2;
		ClothDisplayId = *(short*)ptr;
		ptr += 2;
		FaceVisible = *ptr != 0;
		ptr++;
		CreatingType = *ptr;
		ptr++;
		BirthDate = *(int*)ptr;
		ptr += 4;
		DefeatMarkCount = (sbyte)(*ptr);
		ptr++;
		Charm = *(short*)ptr;
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
		ptr += LifeSkillAttainment.Deserialize(ptr);
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
		ptr += OrgInfo.Deserialize(ptr);
		RoleTemplateId = *(short*)ptr;
		ptr += 2;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (ArrangementDisplayData == null)
			{
				ArrangementDisplayData = new VillagerRoleArrangementDisplayDataWrapper();
			}
			ptr += ArrangementDisplayData.Deserialize(ptr);
		}
		else
		{
			ArrangementDisplayData = null;
		}
		TrappedAreaId = *(short*)ptr;
		ptr += 2;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
