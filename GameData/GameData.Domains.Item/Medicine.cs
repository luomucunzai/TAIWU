using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Serializer;
using Redzen.Random;

namespace GameData.Domains.Item;

[SerializableGameData(NotForDisplayModule = true)]
public class Medicine : ItemBase, ISerializableGameData
{
	internal class FixedFieldInfos
	{
		public const uint Id_Offset = 0u;

		public const int Id_Size = 4;

		public const uint TemplateId_Offset = 4u;

		public const int TemplateId_Size = 2;

		public const uint MaxDurability_Offset = 6u;

		public const int MaxDurability_Size = 2;

		public const uint CurrDurability_Offset = 8u;

		public const int CurrDurability_Size = 2;

		public const uint ModificationState_Offset = 10u;

		public const int ModificationState_Size = 1;
	}

	public const int FixedSize = 11;

	public const int DynamicCount = 0;

	public unsafe override void SetMaxDurability(short maxDurability, DataContext context)
	{
		MaxDurability = maxDurability;
		SetModifiedAndInvalidateInfluencedCache(2, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 6u, 2);
			*(short*)ptr = MaxDurability;
			ptr += 2;
		}
	}

	public unsafe override void SetCurrDurability(short currDurability, DataContext context)
	{
		CurrDurability = currDurability;
		SetModifiedAndInvalidateInfluencedCache(3, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 8u, 2);
			*(short*)ptr = CurrDurability;
			ptr += 2;
		}
	}

	public unsafe override void SetModificationState(byte modificationState, DataContext context)
	{
		ModificationState = modificationState;
		SetModifiedAndInvalidateInfluencedCache(4, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 10u, 1);
			*ptr = ModificationState;
			ptr++;
		}
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetName()
	{
		return Config.Medicine.Instance[TemplateId].Name;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetItemType()
	{
		return Config.Medicine.Instance[TemplateId].ItemType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetItemSubType()
	{
		return Config.Medicine.Instance[TemplateId].ItemSubType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetGrade()
	{
		return Config.Medicine.Instance[TemplateId].Grade;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetIcon()
	{
		return Config.Medicine.Instance[TemplateId].Icon;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetDesc()
	{
		return Config.Medicine.Instance[TemplateId].Desc;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetTransferable()
	{
		return Config.Medicine.Instance[TemplateId].Transferable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetStackable()
	{
		return Config.Medicine.Instance[TemplateId].Stackable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetWagerable()
	{
		return Config.Medicine.Instance[TemplateId].Wagerable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRefinable()
	{
		return Config.Medicine.Instance[TemplateId].Refinable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetPoisonable()
	{
		return Config.Medicine.Instance[TemplateId].Poisonable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRepairable()
	{
		return Config.Medicine.Instance[TemplateId].Repairable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseWeight()
	{
		return Config.Medicine.Instance[TemplateId].BaseWeight;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseValue()
	{
		return Config.Medicine.Instance[TemplateId].BaseValue;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetBasePrice()
	{
		return Config.Medicine.Instance[TemplateId].BasePrice;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetDropRate()
	{
		return Config.Medicine.Instance[TemplateId].DropRate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetResourceType()
	{
		return Config.Medicine.Instance[TemplateId].ResourceType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetPreservationDuration()
	{
		return Config.Medicine.Instance[TemplateId].PreservationDuration;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetDuration()
	{
		return Config.Medicine.Instance[TemplateId].Duration;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetInjuryRecoveryTimes()
	{
		return Config.Medicine.Instance[TemplateId].InjuryRecoveryTimes;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetHitRateStrength()
	{
		return Config.Medicine.Instance[TemplateId].HitRateStrength;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetHitRateTechnique()
	{
		return Config.Medicine.Instance[TemplateId].HitRateTechnique;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetHitRateSpeed()
	{
		return Config.Medicine.Instance[TemplateId].HitRateSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetHitRateMind()
	{
		return Config.Medicine.Instance[TemplateId].HitRateMind;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetPenetrateOfOuter()
	{
		return Config.Medicine.Instance[TemplateId].PenetrateOfOuter;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetPenetrateOfInner()
	{
		return Config.Medicine.Instance[TemplateId].PenetrateOfInner;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAvoidRateStrength()
	{
		return Config.Medicine.Instance[TemplateId].AvoidRateStrength;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAvoidRateTechnique()
	{
		return Config.Medicine.Instance[TemplateId].AvoidRateTechnique;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAvoidRateSpeed()
	{
		return Config.Medicine.Instance[TemplateId].AvoidRateSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAvoidRateMind()
	{
		return Config.Medicine.Instance[TemplateId].AvoidRateMind;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetPenetrateResistOfOuter()
	{
		return Config.Medicine.Instance[TemplateId].PenetrateResistOfOuter;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetPenetrateResistOfInner()
	{
		return Config.Medicine.Instance[TemplateId].PenetrateResistOfInner;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetRecoveryOfStance()
	{
		return Config.Medicine.Instance[TemplateId].RecoveryOfStance;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetRecoveryOfBreath()
	{
		return Config.Medicine.Instance[TemplateId].RecoveryOfBreath;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetMoveSpeed()
	{
		return Config.Medicine.Instance[TemplateId].MoveSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetRecoveryOfFlaw()
	{
		return Config.Medicine.Instance[TemplateId].RecoveryOfFlaw;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetCastSpeed()
	{
		return Config.Medicine.Instance[TemplateId].CastSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetRecoveryOfBlockedAcupoint()
	{
		return Config.Medicine.Instance[TemplateId].RecoveryOfBlockedAcupoint;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetWeaponSwitchSpeed()
	{
		return Config.Medicine.Instance[TemplateId].WeaponSwitchSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAttackSpeed()
	{
		return Config.Medicine.Instance[TemplateId].AttackSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetInnerRatio()
	{
		return Config.Medicine.Instance[TemplateId].InnerRatio;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetRecoveryOfQiDisorder()
	{
		return Config.Medicine.Instance[TemplateId].RecoveryOfQiDisorder;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetWugType()
	{
		return Config.Medicine.Instance[TemplateId].WugType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetWugGrowthType()
	{
		return Config.Medicine.Instance[TemplateId].WugGrowthType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetSpecialEffectClass()
	{
		return Config.Medicine.Instance[TemplateId].SpecialEffectClass;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetConsumedFeatureMedals()
	{
		return Config.Medicine.Instance[TemplateId].ConsumedFeatureMedals;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetMaxUseDistance()
	{
		return Config.Medicine.Instance[TemplateId].MaxUseDistance;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetSpecialEffectDesc()
	{
		return Config.Medicine.Instance[TemplateId].SpecialEffectDesc;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetBuffAndOtherMedicine()
	{
		return Config.Medicine.Instance[TemplateId].BuffAndOtherMedicine;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetBaseHappinessChange()
	{
		return Config.Medicine.Instance[TemplateId].BaseHappinessChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetGiftLevel()
	{
		return Config.Medicine.Instance[TemplateId].GiftLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetSpecialEffectId()
	{
		return Config.Medicine.Instance[TemplateId].SpecialEffectId;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseFavorabilityChange()
	{
		return Config.Medicine.Instance[TemplateId].BaseFavorabilityChange;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfRedPoison()
	{
		return Config.Medicine.Instance[TemplateId].ResistOfRedPoison;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfIllusoryPoison()
	{
		return Config.Medicine.Instance[TemplateId].ResistOfIllusoryPoison;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfRottenPoison()
	{
		return Config.Medicine.Instance[TemplateId].ResistOfRottenPoison;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfColdPoison()
	{
		return Config.Medicine.Instance[TemplateId].ResistOfColdPoison;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfGloomyPoison()
	{
		return Config.Medicine.Instance[TemplateId].ResistOfGloomyPoison;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetCanUseMultiple()
	{
		return Config.Medicine.Instance[TemplateId].CanUseMultiple;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetBreakBonusEffect()
	{
		return Config.Medicine.Instance[TemplateId].BreakBonusEffect;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetInheritable()
	{
		return Config.Medicine.Instance[TemplateId].Inheritable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetMerchantLevel()
	{
		return Config.Medicine.Instance[TemplateId].MerchantLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowRandomCreate()
	{
		return Config.Medicine.Instance[TemplateId].AllowRandomCreate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetIsSpecial()
	{
		return Config.Medicine.Instance[TemplateId].IsSpecial;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfHotPoison()
	{
		return Config.Medicine.Instance[TemplateId].ResistOfHotPoison;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetGroupId()
	{
		return Config.Medicine.Instance[TemplateId].GroupId;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetDamageStepBonus()
	{
		return Config.Medicine.Instance[TemplateId].DamageStepBonus;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetRequiredMainAttributeValue()
	{
		return Config.Medicine.Instance[TemplateId].RequiredMainAttributeValue;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetRequiredMainAttributeType()
	{
		return Config.Medicine.Instance[TemplateId].RequiredMainAttributeType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetSideEffectValue()
	{
		return Config.Medicine.Instance[TemplateId].SideEffectValue;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetEffectValue()
	{
		return Config.Medicine.Instance[TemplateId].EffectValue;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetEffectThresholdValue()
	{
		return Config.Medicine.Instance[TemplateId].EffectThresholdValue;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public EMedicineEffectSubType GetEffectSubType()
	{
		return Config.Medicine.Instance[TemplateId].EffectSubType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetHasNormalEatingEffect()
	{
		return Config.Medicine.Instance[TemplateId].HasNormalEatingEffect;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetInstantAffect()
	{
		return Config.Medicine.Instance[TemplateId].InstantAffect;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetCombatUseEffect()
	{
		return Config.Medicine.Instance[TemplateId].CombatUseEffect;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetCombatPrepareUseEffect()
	{
		return Config.Medicine.Instance[TemplateId].CombatPrepareUseEffect;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public EMedicineEffectType GetEffectType()
	{
		return Config.Medicine.Instance[TemplateId].EffectType;
	}

	public Medicine()
	{
	}

	public Medicine(short templateId)
	{
		MedicineItem medicineItem = Config.Medicine.Instance[templateId];
		TemplateId = medicineItem.TemplateId;
		MaxDurability = medicineItem.MaxDurability;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 11;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = Id;
		ptr += 4;
		*(short*)ptr = TemplateId;
		ptr += 2;
		*(short*)ptr = MaxDurability;
		ptr += 2;
		*(short*)ptr = CurrDurability;
		ptr += 2;
		*ptr = ModificationState;
		ptr++;
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Id = *(int*)ptr;
		ptr += 4;
		TemplateId = *(short*)ptr;
		ptr += 2;
		MaxDurability = *(short*)ptr;
		ptr += 2;
		CurrDurability = *(short*)ptr;
		ptr += 2;
		ModificationState = *ptr;
		ptr++;
		return (int)(ptr - pData);
	}

	public Medicine(IRandomSource random, short templateId, int itemId)
		: this(templateId)
	{
		Id = itemId;
		MaxDurability = ItemBase.GenerateMaxDurability(random, MaxDurability);
		CurrDurability = MaxDurability;
	}

	public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
	{
		return GetCharacterPropertyBonusValue(TemplateId, type);
	}

	public static int GetCharacterPropertyBonusValue(short templateId, ECharacterPropertyReferencedType type)
	{
		return Config.Medicine.Instance[templateId].GetCharacterPropertyBonusValue(type);
	}

	public static int GetCharacterPropertyBonusPercentage(short templateId, ECharacterPropertyReferencedType type)
	{
		return Config.Medicine.Instance[templateId].GetCharacterPropertyBonusPercentage(type);
	}

	public static short GetDeltaWugDuration(sbyte medicineGrade)
	{
		return (short)(-(medicineGrade + 1) * 12);
	}
}
