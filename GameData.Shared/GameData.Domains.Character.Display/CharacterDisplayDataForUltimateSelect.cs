using System;
using System.Collections.Generic;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Display;

[SerializableGameData(NotForArchive = true, NoCopyConstructors = true)]
public class CharacterDisplayDataForUltimateSelect : ISerializableGameData
{
	[SerializableGameDataField]
	public int CharacterId;

	[SerializableGameDataField]
	public byte CreatingType;

	[SerializableGameDataField]
	public NameRelatedData NameRelatedData;

	[SerializableGameDataField]
	public AvatarRelatedData AvatarRelatedData;

	[SerializableGameDataField]
	public short CurrAge;

	[SerializableGameDataField]
	public short Charm;

	[SerializableGameDataField]
	public sbyte Gender;

	[SerializableGameDataField]
	public sbyte HappinessType;

	[SerializableGameDataField]
	public sbyte BehaviorType;

	[SerializableGameDataField]
	public sbyte FameType;

	[SerializableGameDataField]
	public OrganizationInfo OrganizationInfo;

	[SerializableGameDataField]
	public bool IsReclusiveChar;

	[SerializableGameDataField]
	public FullBlockName LocationNameData;

	[SerializableGameDataField]
	public short Health;

	[SerializableGameDataField]
	public short LeftMaxHealth;

	[SerializableGameDataField]
	public short FavorabilityToTaiwu;

	[SerializableGameDataField]
	public bool IsInteractedWithTaiwu;

	[SerializableGameDataField]
	public int BirthDate;

	[SerializableGameDataField]
	public short PreexistenceCharCount;

	[SerializableGameDataField]
	public sbyte DefeatMarkCount;

	[SerializableGameDataField]
	public int AttackMedal;

	[SerializableGameDataField]
	public int DefenceMedal;

	[SerializableGameDataField]
	public int WisdomMedal;

	[SerializableGameDataField]
	public int ConsummateLevel;

	[SerializableGameDataField]
	public sbyte BirthMonth;

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
	public NeiliProportionOfFiveElements NeiliProportionOfFiveElements;

	[SerializableGameDataField]
	public sbyte VillagerNeedWaitTime;

	private Dictionary<int, (int, string)> _valuesCache;

	public (int, string) this[int sortingTypeId]
	{
		get
		{
			if (!_valuesCache.TryGetValue(sortingTypeId, out var value))
			{
				return (0, string.Empty);
			}
			return value;
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 351;
		num += LocationNameData.GetSerializedSize();
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
		*ptr = CreatingType;
		ptr++;
		ptr += NameRelatedData.Serialize(ptr);
		ptr += AvatarRelatedData.Serialize(ptr);
		*(short*)ptr = CurrAge;
		ptr += 2;
		*(short*)ptr = Charm;
		ptr += 2;
		*ptr = (byte)Gender;
		ptr++;
		*ptr = (byte)HappinessType;
		ptr++;
		*ptr = (byte)BehaviorType;
		ptr++;
		*ptr = (byte)FameType;
		ptr++;
		ptr += OrganizationInfo.Serialize(ptr);
		*ptr = (IsReclusiveChar ? ((byte)1) : ((byte)0));
		ptr++;
		int num = LocationNameData.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		*(short*)ptr = Health;
		ptr += 2;
		*(short*)ptr = LeftMaxHealth;
		ptr += 2;
		*(short*)ptr = FavorabilityToTaiwu;
		ptr += 2;
		*ptr = (IsInteractedWithTaiwu ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = BirthDate;
		ptr += 4;
		*(short*)ptr = PreexistenceCharCount;
		ptr += 2;
		*ptr = (byte)DefeatMarkCount;
		ptr++;
		*(int*)ptr = AttackMedal;
		ptr += 4;
		*(int*)ptr = DefenceMedal;
		ptr += 4;
		*(int*)ptr = WisdomMedal;
		ptr += 4;
		*(int*)ptr = ConsummateLevel;
		ptr += 4;
		*ptr = (byte)BirthMonth;
		ptr++;
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
		ptr += NeiliProportionOfFiveElements.Serialize(ptr);
		*ptr = (byte)VillagerNeedWaitTime;
		ptr++;
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
		CreatingType = *ptr;
		ptr++;
		ptr += NameRelatedData.Deserialize(ptr);
		if (AvatarRelatedData == null)
		{
			AvatarRelatedData = new AvatarRelatedData();
		}
		ptr += AvatarRelatedData.Deserialize(ptr);
		CurrAge = *(short*)ptr;
		ptr += 2;
		Charm = *(short*)ptr;
		ptr += 2;
		Gender = (sbyte)(*ptr);
		ptr++;
		HappinessType = (sbyte)(*ptr);
		ptr++;
		BehaviorType = (sbyte)(*ptr);
		ptr++;
		FameType = (sbyte)(*ptr);
		ptr++;
		ptr += OrganizationInfo.Deserialize(ptr);
		IsReclusiveChar = *ptr != 0;
		ptr++;
		ptr += LocationNameData.Deserialize(ptr);
		Health = *(short*)ptr;
		ptr += 2;
		LeftMaxHealth = *(short*)ptr;
		ptr += 2;
		FavorabilityToTaiwu = *(short*)ptr;
		ptr += 2;
		IsInteractedWithTaiwu = *ptr != 0;
		ptr++;
		BirthDate = *(int*)ptr;
		ptr += 4;
		PreexistenceCharCount = *(short*)ptr;
		ptr += 2;
		DefeatMarkCount = (sbyte)(*ptr);
		ptr++;
		AttackMedal = *(int*)ptr;
		ptr += 4;
		DefenceMedal = *(int*)ptr;
		ptr += 4;
		WisdomMedal = *(int*)ptr;
		ptr += 4;
		ConsummateLevel = *(int*)ptr;
		ptr += 4;
		BirthMonth = (sbyte)(*ptr);
		ptr++;
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
		ptr += NeiliProportionOfFiveElements.Deserialize(ptr);
		VillagerNeedWaitTime = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public void InitValuesCache(Action<CharacterDisplayDataForUltimateSelect, Dictionary<int, (int, string)>> initCache)
	{
		if (_valuesCache == null)
		{
			_valuesCache = new Dictionary<int, (int, string)>();
			initCache(this, _valuesCache);
		}
	}
}
