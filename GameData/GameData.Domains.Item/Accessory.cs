using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Domains.Map;
using GameData.Domains.Taiwu.Profession;
using GameData.Serializer;
using Redzen.Random;

namespace GameData.Domains.Item;

[SerializableGameData(NotForDisplayModule = true)]
public class Accessory : EquipmentBase, ISerializableGameData, IExploreBonusRateItem
{
	internal class FixedFieldInfos
	{
		public const uint Id_Offset = 0u;

		public const int Id_Size = 4;

		public const uint TemplateId_Offset = 4u;

		public const int TemplateId_Size = 2;

		public const uint MaxDurability_Offset = 6u;

		public const int MaxDurability_Size = 2;

		public const uint EquipmentEffectId_Offset = 8u;

		public const int EquipmentEffectId_Size = 2;

		public const uint CurrDurability_Offset = 10u;

		public const int CurrDurability_Size = 2;

		public const uint ModificationState_Offset = 12u;

		public const int ModificationState_Size = 1;

		public const uint EquippedCharId_Offset = 13u;

		public const int EquippedCharId_Size = 4;

		public const uint MaterialResources_Offset = 17u;

		public const int MaterialResources_Size = 12;
	}

	public const int FixedSize = 29;

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

	public unsafe override void SetEquipmentEffectId(short equipmentEffectId, DataContext context)
	{
		EquipmentEffectId = equipmentEffectId;
		SetModifiedAndInvalidateInfluencedCache(3, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 8u, 2);
			*(short*)ptr = EquipmentEffectId;
			ptr += 2;
		}
	}

	public unsafe override void SetCurrDurability(short currDurability, DataContext context)
	{
		CurrDurability = currDurability;
		SetModifiedAndInvalidateInfluencedCache(4, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 10u, 2);
			*(short*)ptr = CurrDurability;
			ptr += 2;
		}
	}

	public unsafe override void SetModificationState(byte modificationState, DataContext context)
	{
		ModificationState = modificationState;
		SetModifiedAndInvalidateInfluencedCache(5, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 12u, 1);
			*ptr = ModificationState;
			ptr++;
		}
	}

	public unsafe override void SetEquippedCharId(int equippedCharId, DataContext context)
	{
		EquippedCharId = equippedCharId;
		SetModifiedAndInvalidateInfluencedCache(6, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 13u, 4);
			*(int*)ptr = EquippedCharId;
			ptr += 4;
		}
	}

	public unsafe override void SetMaterialResources(MaterialResources materialResources, DataContext context)
	{
		MaterialResources = materialResources;
		SetModifiedAndInvalidateInfluencedCache(7, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 17u, 12);
			ptr += MaterialResources.Serialize(ptr);
		}
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetName()
	{
		return Config.Accessory.Instance[TemplateId].Name;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetItemType()
	{
		return Config.Accessory.Instance[TemplateId].ItemType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetItemSubType()
	{
		return Config.Accessory.Instance[TemplateId].ItemSubType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetGrade()
	{
		return Config.Accessory.Instance[TemplateId].Grade;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetIcon()
	{
		return Config.Accessory.Instance[TemplateId].Icon;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetDesc()
	{
		return Config.Accessory.Instance[TemplateId].Desc;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetTransferable()
	{
		return Config.Accessory.Instance[TemplateId].Transferable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetStackable()
	{
		return Config.Accessory.Instance[TemplateId].Stackable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetWagerable()
	{
		return Config.Accessory.Instance[TemplateId].Wagerable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRefinable()
	{
		return Config.Accessory.Instance[TemplateId].Refinable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetPoisonable()
	{
		return Config.Accessory.Instance[TemplateId].Poisonable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRepairable()
	{
		return Config.Accessory.Instance[TemplateId].Repairable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseWeight()
	{
		return Config.Accessory.Instance[TemplateId].BaseWeight;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseValue()
	{
		return Config.Accessory.Instance[TemplateId].BaseValue;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetBasePrice()
	{
		return Config.Accessory.Instance[TemplateId].BasePrice;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseFavorabilityChange()
	{
		return Config.Accessory.Instance[TemplateId].BaseFavorabilityChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetDropRate()
	{
		return Config.Accessory.Instance[TemplateId].DropRate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetResourceType()
	{
		return Config.Accessory.Instance[TemplateId].ResourceType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetPreservationDuration()
	{
		return Config.Accessory.Instance[TemplateId].PreservationDuration;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetEquipmentType()
	{
		return Config.Accessory.Instance[TemplateId].EquipmentType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetDropRateBonus()
	{
		return Config.Accessory.Instance[TemplateId].DropRateBonus;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetMaxInventoryLoadBonus()
	{
		return Config.Accessory.Instance[TemplateId].MaxInventoryLoadBonus;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetStrength()
	{
		return Config.Accessory.Instance[TemplateId].Strength;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetDexterity()
	{
		return Config.Accessory.Instance[TemplateId].Dexterity;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetConcentration()
	{
		return Config.Accessory.Instance[TemplateId].Concentration;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetVitality()
	{
		return Config.Accessory.Instance[TemplateId].Vitality;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetEnergy()
	{
		return Config.Accessory.Instance[TemplateId].Energy;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetIntelligence()
	{
		return Config.Accessory.Instance[TemplateId].Intelligence;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetHitRateStrength()
	{
		return Config.Accessory.Instance[TemplateId].HitRateStrength;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetHitRateTechnique()
	{
		return Config.Accessory.Instance[TemplateId].HitRateTechnique;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetHitRateSpeed()
	{
		return Config.Accessory.Instance[TemplateId].HitRateSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetPenetrateOfOuter()
	{
		return Config.Accessory.Instance[TemplateId].PenetrateOfOuter;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetPenetrateOfInner()
	{
		return Config.Accessory.Instance[TemplateId].PenetrateOfInner;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAvoidRateStrength()
	{
		return Config.Accessory.Instance[TemplateId].AvoidRateStrength;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAvoidRateTechnique()
	{
		return Config.Accessory.Instance[TemplateId].AvoidRateTechnique;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAvoidRateSpeed()
	{
		return Config.Accessory.Instance[TemplateId].AvoidRateSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetPenetrateResistOfOuter()
	{
		return Config.Accessory.Instance[TemplateId].PenetrateResistOfOuter;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetPenetrateResistOfInner()
	{
		return Config.Accessory.Instance[TemplateId].PenetrateResistOfInner;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetRecoveryOfStance()
	{
		return Config.Accessory.Instance[TemplateId].RecoveryOfStance;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetRecoveryOfBreath()
	{
		return Config.Accessory.Instance[TemplateId].RecoveryOfBreath;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetMoveSpeed()
	{
		return Config.Accessory.Instance[TemplateId].MoveSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetRecoveryOfFlaw()
	{
		return Config.Accessory.Instance[TemplateId].RecoveryOfFlaw;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetCastSpeed()
	{
		return Config.Accessory.Instance[TemplateId].CastSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetRecoveryOfBlockedAcupoint()
	{
		return Config.Accessory.Instance[TemplateId].RecoveryOfBlockedAcupoint;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetWeaponSwitchSpeed()
	{
		return Config.Accessory.Instance[TemplateId].WeaponSwitchSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAttackSpeed()
	{
		return Config.Accessory.Instance[TemplateId].AttackSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetInnerRatio()
	{
		return Config.Accessory.Instance[TemplateId].InnerRatio;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetRecoveryOfQiDisorder()
	{
		return Config.Accessory.Instance[TemplateId].RecoveryOfQiDisorder;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfHotPoison()
	{
		return Config.Accessory.Instance[TemplateId].ResistOfHotPoison;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfGloomyPoison()
	{
		return Config.Accessory.Instance[TemplateId].ResistOfGloomyPoison;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfColdPoison()
	{
		return Config.Accessory.Instance[TemplateId].ResistOfColdPoison;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfRedPoison()
	{
		return Config.Accessory.Instance[TemplateId].ResistOfRedPoison;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfRottenPoison()
	{
		return Config.Accessory.Instance[TemplateId].ResistOfRottenPoison;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfIllusoryPoison()
	{
		return Config.Accessory.Instance[TemplateId].ResistOfIllusoryPoison;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetBonusCombatSkillSect()
	{
		return Config.Accessory.Instance[TemplateId].BonusCombatSkillSect;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetMakeItemSubType()
	{
		return Config.Accessory.Instance[TemplateId].MakeItemSubType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetGiftLevel()
	{
		return Config.Accessory.Instance[TemplateId].GiftLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetBaseHappinessChange()
	{
		return Config.Accessory.Instance[TemplateId].BaseHappinessChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetDetachable()
	{
		return Config.Accessory.Instance[TemplateId].Detachable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetGroupId()
	{
		return Config.Accessory.Instance[TemplateId].GroupId;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetIsSpecial()
	{
		return Config.Accessory.Instance[TemplateId].IsSpecial;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowRawCreate()
	{
		return Config.Accessory.Instance[TemplateId].AllowRawCreate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowRandomCreate()
	{
		return Config.Accessory.Instance[TemplateId].AllowRandomCreate;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAvoidRateMind()
	{
		return Config.Accessory.Instance[TemplateId].AvoidRateMind;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetCombatSkillAddMaxPower()
	{
		return Config.Accessory.Instance[TemplateId].CombatSkillAddMaxPower;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetMerchantLevel()
	{
		return Config.Accessory.Instance[TemplateId].MerchantLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetInheritable()
	{
		return Config.Accessory.Instance[TemplateId].Inheritable;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetHitRateMind()
	{
		return Config.Accessory.Instance[TemplateId].HitRateMind;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetEquipmentCombatPowerValueFactor()
	{
		return Config.Accessory.Instance[TemplateId].EquipmentCombatPowerValueFactor;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseCaptureRateBonus()
	{
		return Config.Accessory.Instance[TemplateId].BaseCaptureRateBonus;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseExploreBonusRate()
	{
		return Config.Accessory.Instance[TemplateId].BaseExploreBonusRate;
	}

	public Accessory()
	{
	}

	public Accessory(short templateId)
	{
		AccessoryItem accessoryItem = Config.Accessory.Instance[templateId];
		TemplateId = accessoryItem.TemplateId;
		MaxDurability = accessoryItem.MaxDurability;
		EquipmentEffectId = accessoryItem.EquipmentEffectId;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 29;
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
		*(short*)ptr = EquipmentEffectId;
		ptr += 2;
		*(short*)ptr = CurrDurability;
		ptr += 2;
		*ptr = ModificationState;
		ptr++;
		*(int*)ptr = EquippedCharId;
		ptr += 4;
		ptr += MaterialResources.Serialize(ptr);
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
		EquipmentEffectId = *(short*)ptr;
		ptr += 2;
		CurrDurability = *(short*)ptr;
		ptr += 2;
		ModificationState = *ptr;
		ptr++;
		EquippedCharId = *(int*)ptr;
		ptr += 4;
		ptr += MaterialResources.Deserialize(ptr);
		return (int)(ptr - pData);
	}

	public Accessory(IRandomSource random, short templateId, int itemId)
		: this(templateId)
	{
		Id = itemId;
		MaxDurability = ItemBase.GenerateMaxDurability(random, MaxDurability);
		CurrDurability = MaxDurability;
	}

	public override int GetFavorabilityChange()
	{
		int num = Config.Accessory.Instance[TemplateId].BaseFavorabilityChange;
		if (EquipmentEffectId >= 0)
		{
			EquipmentEffectItem equipmentEffectItem = EquipmentEffect.Instance[EquipmentEffectId];
			num += num * equipmentEffectItem.FavorChange / 100;
		}
		return num;
	}

	public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
	{
		AccessoryItem accessoryItem = Config.Accessory.Instance[TemplateId];
		int num = accessoryItem.GetCharacterPropertyBonusInt(type);
		if (ModificationStateHelper.IsActive(ModificationState, 2))
		{
			ERefiningEffectAccessoryType eRefiningEffectAccessoryType = RefiningEffects.CharPropertyTypeToRefiningEffectAccessoryType(type);
			if (eRefiningEffectAccessoryType != ERefiningEffectAccessoryType.Invalid)
			{
				int accessoryPropertyBonus = DomainManager.Item.GetRefinedEffects(GetItemKey()).GetAccessoryPropertyBonus(eRefiningEffectAccessoryType);
				accessoryPropertyBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(accessoryPropertyBonus, EquippedCharId);
				num += accessoryPropertyBonus;
			}
		}
		return num;
	}

	int IExploreBonusRateItem.GetExploreBonusRate()
	{
		return GetBaseExploreBonusRate();
	}
}
