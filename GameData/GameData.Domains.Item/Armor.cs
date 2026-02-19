using System;
using System.Collections.Generic;
using System.Threading;
using Config;
using Config.ConfigCells.Character;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Dependencies;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Domains.Taiwu.Profession;
using GameData.Serializer;
using Redzen.Random;

namespace GameData.Domains.Item;

[SerializableGameData(NotForDisplayModule = true)]
public class Armor : EquipmentBase, ISerializableGameData
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

	[CollectionObjectField(false, false, true, false, false)]
	private OuterAndInnerShorts _penetrationResistFactors;

	[CollectionObjectField(false, false, true, false, false)]
	private short _equipmentAttack;

	[CollectionObjectField(false, false, true, false, false)]
	private short _equipmentDefense;

	[CollectionObjectField(false, false, true, false, false)]
	private int _weight;

	[CollectionObjectField(false, false, true, false, false)]
	private OuterAndInnerShorts _injuryFactor;

	public const int FixedSize = 29;

	public const int DynamicCount = 0;

	private SpinLock _spinLock = new SpinLock(enableThreadOwnerTracking: false);

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

	public OuterAndInnerShorts GetPenetrationResistFactors()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 8))
		{
			return _penetrationResistFactors;
		}
		OuterAndInnerShorts penetrationResistFactors = CalcPenetrationResistFactors();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_penetrationResistFactors = penetrationResistFactors;
			dataStates.SetCached(DataStatesOffset, 8);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _penetrationResistFactors;
	}

	public short GetEquipmentAttack()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 9))
		{
			return _equipmentAttack;
		}
		short equipmentAttack = CalcEquipmentAttack();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_equipmentAttack = equipmentAttack;
			dataStates.SetCached(DataStatesOffset, 9);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _equipmentAttack;
	}

	public short GetEquipmentDefense()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 10))
		{
			return _equipmentDefense;
		}
		short equipmentDefense = CalcEquipmentDefense();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_equipmentDefense = equipmentDefense;
			dataStates.SetCached(DataStatesOffset, 10);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _equipmentDefense;
	}

	public override int GetWeight()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 11))
		{
			return _weight;
		}
		int weight = CalcWeight();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_weight = weight;
			dataStates.SetCached(DataStatesOffset, 11);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _weight;
	}

	public OuterAndInnerShorts GetInjuryFactor()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 12))
		{
			return _injuryFactor;
		}
		OuterAndInnerShorts injuryFactor = CalcInjuryFactor();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_injuryFactor = injuryFactor;
			dataStates.SetCached(DataStatesOffset, 12);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _injuryFactor;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetName()
	{
		return Config.Armor.Instance[TemplateId].Name;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetItemType()
	{
		return Config.Armor.Instance[TemplateId].ItemType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetItemSubType()
	{
		return Config.Armor.Instance[TemplateId].ItemSubType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetGrade()
	{
		return Config.Armor.Instance[TemplateId].Grade;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetIcon()
	{
		return Config.Armor.Instance[TemplateId].Icon;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetDesc()
	{
		return Config.Armor.Instance[TemplateId].Desc;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetTransferable()
	{
		return Config.Armor.Instance[TemplateId].Transferable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetStackable()
	{
		return Config.Armor.Instance[TemplateId].Stackable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetWagerable()
	{
		return Config.Armor.Instance[TemplateId].Wagerable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRefinable()
	{
		return Config.Armor.Instance[TemplateId].Refinable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetPoisonable()
	{
		return Config.Armor.Instance[TemplateId].Poisonable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRepairable()
	{
		return Config.Armor.Instance[TemplateId].Repairable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseWeight()
	{
		return Config.Armor.Instance[TemplateId].BaseWeight;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseValue()
	{
		return Config.Armor.Instance[TemplateId].BaseValue;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetBasePrice()
	{
		return Config.Armor.Instance[TemplateId].BasePrice;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseFavorabilityChange()
	{
		return Config.Armor.Instance[TemplateId].BaseFavorabilityChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetDropRate()
	{
		return Config.Armor.Instance[TemplateId].DropRate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetResourceType()
	{
		return Config.Armor.Instance[TemplateId].ResourceType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetPreservationDuration()
	{
		return Config.Armor.Instance[TemplateId].PreservationDuration;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetEquipmentType()
	{
		return Config.Armor.Instance[TemplateId].EquipmentType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseEquipmentAttack()
	{
		return Config.Armor.Instance[TemplateId].BaseEquipmentAttack;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseEquipmentDefense()
	{
		return Config.Armor.Instance[TemplateId].BaseEquipmentDefense;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<PropertyAndValue> GetRequiredCharacterProperties()
	{
		return Config.Armor.Instance[TemplateId].RequiredCharacterProperties;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public HitOrAvoidShorts GetBaseAvoidFactors()
	{
		return Config.Armor.Instance[TemplateId].BaseAvoidFactors;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public OuterAndInnerShorts GetBasePenetrationResistFactors()
	{
		return Config.Armor.Instance[TemplateId].BasePenetrationResistFactors;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetRelatedWeapon()
	{
		return Config.Armor.Instance[TemplateId].RelatedWeapon;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<string> GetSkeletonSlotAndAttachment()
	{
		return Config.Armor.Instance[TemplateId].SkeletonSlotAndAttachment;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetMakeItemSubType()
	{
		return Config.Armor.Instance[TemplateId].MakeItemSubType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetGiftLevel()
	{
		return Config.Armor.Instance[TemplateId].GiftLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetBaseHappinessChange()
	{
		return Config.Armor.Instance[TemplateId].BaseHappinessChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetDetachable()
	{
		return Config.Armor.Instance[TemplateId].Detachable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public OuterAndInnerShorts GetBaseInjuryFactors()
	{
		return Config.Armor.Instance[TemplateId].BaseInjuryFactors;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetIsSpecial()
	{
		return Config.Armor.Instance[TemplateId].IsSpecial;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowRandomCreate()
	{
		return Config.Armor.Instance[TemplateId].AllowRandomCreate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetMerchantLevel()
	{
		return Config.Armor.Instance[TemplateId].MerchantLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetGroupId()
	{
		return Config.Armor.Instance[TemplateId].GroupId;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowCrippledCreate()
	{
		return Config.Armor.Instance[TemplateId].AllowCrippledCreate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetInheritable()
	{
		return Config.Armor.Instance[TemplateId].Inheritable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowRawCreate()
	{
		return Config.Armor.Instance[TemplateId].AllowRawCreate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetEquipmentCombatPowerValueFactor()
	{
		return Config.Armor.Instance[TemplateId].EquipmentCombatPowerValueFactor;
	}

	public Armor()
	{
	}

	public Armor(short templateId)
	{
		ArmorItem armorItem = Config.Armor.Instance[templateId];
		TemplateId = armorItem.TemplateId;
		MaxDurability = armorItem.MaxDurability;
		EquipmentEffectId = armorItem.EquipmentEffectId;
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

	[ObjectCollectionDependency(6, 1, new ushort[] { 5, 3, 7 }, Scope = InfluenceScope.Self)]
	private OuterAndInnerShorts CalcPenetrationResistFactors()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		OuterAndInnerInts outerAndInnerInts = GetBasePenetrationResistFactors();
		CValuePercent val = CValuePercent.op_Implicit(GetMaterialResourceBonusValuePercentage(3));
		ref int outer = ref outerAndInnerInts.Outer;
		outer *= val;
		ref int inner = ref outerAndInnerInts.Inner;
		inner *= val;
		EquipmentEffectHelper.ValueSelector penetrationResistFactorSelector = EquipmentEffectHelper.GetPenetrationResistFactorSelector(inner: false);
		outerAndInnerInts.Outer = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(outerAndInnerInts.Outer, penetrationResistFactorSelector);
		EquipmentEffectHelper.ValueSelector penetrationResistFactorSelector2 = EquipmentEffectHelper.GetPenetrationResistFactorSelector(inner: true);
		outerAndInnerInts.Inner = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(outerAndInnerInts.Inner, penetrationResistFactorSelector2);
		if (ModificationStateHelper.IsActive(ModificationState, 2))
		{
			int armorPropertyBonus = DomainManager.Item.GetRefinedEffects(GetItemKey()).GetArmorPropertyBonus(ERefiningEffectArmorType.PenetrationResist);
			armorPropertyBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(armorPropertyBonus, EquippedCharId);
			outerAndInnerInts.Inner += armorPropertyBonus;
			outerAndInnerInts.Outer += armorPropertyBonus;
		}
		return (OuterAndInnerShorts)outerAndInnerInts;
	}

	[ObjectCollectionDependency(6, 1, new ushort[] { 3 }, Scope = InfluenceScope.Self)]
	private OuterAndInnerShorts CalcInjuryFactor()
	{
		OuterAndInnerInts outerAndInnerInts = GetBaseInjuryFactors();
		EquipmentEffectHelper.ValueSelector injuryFactorSelector = EquipmentEffectHelper.GetInjuryFactorSelector(inner: false);
		outerAndInnerInts.Outer = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(outerAndInnerInts.Outer, injuryFactorSelector);
		EquipmentEffectHelper.ValueSelector injuryFactorSelector2 = EquipmentEffectHelper.GetInjuryFactorSelector(inner: true);
		outerAndInnerInts.Inner = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(outerAndInnerInts.Inner, injuryFactorSelector2);
		return (OuterAndInnerShorts)outerAndInnerInts;
	}

	[ObjectCollectionDependency(6, 1, new ushort[] { 5, 3, 7 }, Scope = InfluenceScope.Self)]
	private short CalcEquipmentAttack()
	{
		int baseEquipmentAttack = GetBaseEquipmentAttack();
		baseEquipmentAttack = baseEquipmentAttack * GetMaterialResourceBonusValuePercentage(0) / 100;
		baseEquipmentAttack = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(baseEquipmentAttack, (EquipmentEffectItem x) => x.EquipmentAttackChange);
		if (ModificationStateHelper.IsActive(ModificationState, 2))
		{
			int armorPropertyBonus = DomainManager.Item.GetRefinedEffects(GetItemKey()).GetArmorPropertyBonus(ERefiningEffectArmorType.EquipmentAttack);
			armorPropertyBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(armorPropertyBonus, EquippedCharId);
			baseEquipmentAttack = baseEquipmentAttack * (100 + armorPropertyBonus) / 100;
		}
		return (short)baseEquipmentAttack;
	}

	[ObjectCollectionDependency(6, 1, new ushort[] { 5, 3, 7 }, Scope = InfluenceScope.Self)]
	private short CalcEquipmentDefense()
	{
		int baseEquipmentDefense = GetBaseEquipmentDefense();
		baseEquipmentDefense = baseEquipmentDefense * GetMaterialResourceBonusValuePercentage(1) / 100;
		baseEquipmentDefense = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(baseEquipmentDefense, (EquipmentEffectItem x) => x.EquipmentDefenseChange);
		if (ModificationStateHelper.IsActive(ModificationState, 2))
		{
			int armorPropertyBonus = DomainManager.Item.GetRefinedEffects(GetItemKey()).GetArmorPropertyBonus(ERefiningEffectArmorType.EquipmentDefense);
			armorPropertyBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(armorPropertyBonus, EquippedCharId);
			baseEquipmentDefense = baseEquipmentDefense * (100 + armorPropertyBonus) / 100;
		}
		return (short)baseEquipmentDefense;
	}

	[ObjectCollectionDependency(6, 1, new ushort[] { 5, 3, 7 }, Scope = InfluenceScope.Self)]
	private int CalcWeight()
	{
		int baseWeight = GetBaseWeight();
		baseWeight = baseWeight * (170 - GetMaterialResourceBonusValuePercentage(4)) / 100;
		baseWeight = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(baseWeight, (EquipmentEffectItem x) => x.WeightChange);
		if (ModificationStateHelper.IsActive(ModificationState, 2))
		{
			int armorPropertyBonus = DomainManager.Item.GetRefinedEffects(GetItemKey()).GetArmorPropertyBonus(ERefiningEffectArmorType.Weight);
			armorPropertyBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(armorPropertyBonus, EquippedCharId);
			baseWeight = baseWeight * (100 + armorPropertyBonus) / 100;
		}
		return baseWeight;
	}

	public Armor(IRandomSource random, short templateId, int itemId)
		: this(templateId)
	{
		Id = itemId;
		MaxDurability = ItemBase.GenerateMaxDurability(random, MaxDurability);
		CurrDurability = MaxDurability;
	}

	public override int GetFavorabilityChange()
	{
		int num = Config.Armor.Instance[TemplateId].BaseFavorabilityChange;
		if (EquipmentEffectId >= 0)
		{
			EquipmentEffectItem equipmentEffectItem = EquipmentEffect.Instance[EquipmentEffectId];
			num += num * equipmentEffectItem.FavorChange / 100;
		}
		return num;
	}

	public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
	{
		return 0;
	}

	public unsafe HitOrAvoidShorts GetAvoidFactors()
	{
		HitOrAvoidInts hitOrAvoidInts = GetBaseAvoidFactors();
		for (int i = 0; i < 4; i++)
		{
			EquipmentEffectHelper.ValueSelector avoidFactorSelector = EquipmentEffectHelper.GetAvoidFactorSelector(i);
			hitOrAvoidInts[i] = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(hitOrAvoidInts[i], avoidFactorSelector);
		}
		if (ModificationStateHelper.IsActive(ModificationState, 2))
		{
			RefiningEffects refinedEffects = DomainManager.Item.GetRefinedEffects(GetItemKey());
			for (int j = 0; j < 4; j++)
			{
				int armorPropertyBonus = refinedEffects.GetArmorPropertyBonus((ERefiningEffectArmorType)(0 + j));
				armorPropertyBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(armorPropertyBonus, EquippedCharId);
				ref int reference = ref hitOrAvoidInts.Items[j];
				reference += Math.Abs(hitOrAvoidInts.Items[j] * armorPropertyBonus / 100);
			}
		}
		return (HitOrAvoidShorts)hitOrAvoidInts;
	}

	public unsafe HitOrAvoidShorts GetAvoidFactors(int charId)
	{
		HitOrAvoidShorts avoidFactors = GetAvoidFactors();
		short itemPower = DomainManager.Character.GetItemPower(charId, GetItemKey());
		for (int i = 0; i < 4; i++)
		{
			int num = avoidFactors.Items[i];
			if (num > 0)
			{
				avoidFactors.Items[i] = (short)(num * itemPower / 100);
			}
		}
		return avoidFactors;
	}
}
