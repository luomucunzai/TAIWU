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
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Item;

[SerializableGameData(NotForDisplayModule = true)]
public class Weapon : EquipmentBase, ISerializableGameData
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

	[CollectionObjectField(true, true, false, false, false)]
	private List<sbyte> _tricks;

	[CollectionObjectField(false, false, true, false, false)]
	private short _penetrationFactor;

	[CollectionObjectField(false, false, true, false, false)]
	private short _equipmentAttack;

	[CollectionObjectField(false, false, true, false, false)]
	private short _equipmentDefense;

	[CollectionObjectField(false, false, true, false, false)]
	private int _weight;

	public const int FixedSize = 29;

	public const int DynamicCount = 1;

	private SpinLock _spinLock = new SpinLock(enableThreadOwnerTracking: false);

	[ObjectCollectionDependency(6, 0, new ushort[] { 6, 8 }, Scope = InfluenceScope.Self)]
	private short CalcPenetrationFactor()
	{
		int basePenetrationFactor = GetBasePenetrationFactor();
		basePenetrationFactor = basePenetrationFactor * GetMaterialResourceBonusValuePercentage(2) / 100;
		if (!ModificationStateHelper.IsActive(ModificationState, 2))
		{
			return (short)basePenetrationFactor;
		}
		int weaponPropertyBonus = DomainManager.Item.GetRefinedEffects(GetItemKey()).GetWeaponPropertyBonus(ERefiningEffectWeaponType.Penetration);
		weaponPropertyBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(weaponPropertyBonus, EquippedCharId);
		basePenetrationFactor += (short)weaponPropertyBonus;
		return (short)basePenetrationFactor;
	}

	[ObjectCollectionDependency(6, 0, new ushort[] { 6, 3, 8 }, Scope = InfluenceScope.Self)]
	private short CalcEquipmentAttack()
	{
		int baseEquipmentAttack = GetBaseEquipmentAttack();
		baseEquipmentAttack = baseEquipmentAttack * GetMaterialResourceBonusValuePercentage(0) / 100;
		baseEquipmentAttack = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(baseEquipmentAttack, (EquipmentEffectItem x) => x.EquipmentAttackChange);
		if (!ModificationStateHelper.IsActive(ModificationState, 2))
		{
			return (short)baseEquipmentAttack;
		}
		int weaponPropertyBonus = DomainManager.Item.GetRefinedEffects(GetItemKey()).GetWeaponPropertyBonus(ERefiningEffectWeaponType.EquipmentAttack);
		weaponPropertyBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(weaponPropertyBonus, EquippedCharId);
		baseEquipmentAttack = baseEquipmentAttack * (100 + weaponPropertyBonus) / 100;
		return (short)baseEquipmentAttack;
	}

	[ObjectCollectionDependency(6, 0, new ushort[] { 6, 3, 8 }, Scope = InfluenceScope.Self)]
	private short CalcEquipmentDefense()
	{
		int baseEquipmentDefense = GetBaseEquipmentDefense();
		baseEquipmentDefense = baseEquipmentDefense * GetMaterialResourceBonusValuePercentage(1) / 100;
		baseEquipmentDefense = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(baseEquipmentDefense, (EquipmentEffectItem x) => x.EquipmentDefenseChange);
		if (!ModificationStateHelper.IsActive(ModificationState, 2))
		{
			return (short)baseEquipmentDefense;
		}
		int weaponPropertyBonus = DomainManager.Item.GetRefinedEffects(GetItemKey()).GetWeaponPropertyBonus(ERefiningEffectWeaponType.EquipmentDefense);
		weaponPropertyBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(weaponPropertyBonus, EquippedCharId);
		baseEquipmentDefense = baseEquipmentDefense * (100 + weaponPropertyBonus) / 100;
		return (short)baseEquipmentDefense;
	}

	[ObjectCollectionDependency(6, 0, new ushort[] { 6, 3, 8 }, Scope = InfluenceScope.Self)]
	private int CalcWeight()
	{
		int baseWeight = GetBaseWeight();
		baseWeight = baseWeight * (170 - GetMaterialResourceBonusValuePercentage(4)) / 100;
		baseWeight = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(baseWeight, (EquipmentEffectItem x) => x.WeightChange);
		if (!ModificationStateHelper.IsActive(ModificationState, 2))
		{
			return baseWeight;
		}
		int weaponPropertyBonus = DomainManager.Item.GetRefinedEffects(GetItemKey()).GetWeaponPropertyBonus(ERefiningEffectWeaponType.Weight);
		weaponPropertyBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(weaponPropertyBonus, EquippedCharId);
		return baseWeight * (100 + weaponPropertyBonus) / 100;
	}

	public Weapon(IRandomSource random, short templateId, int itemId)
		: this(templateId)
	{
		Id = itemId;
		MaxDurability = ItemBase.GenerateMaxDurability(random, MaxDurability);
		CurrDurability = MaxDurability;
		if (GetRandomTrick())
		{
			CollectionUtils.Shuffle(random, _tricks);
		}
	}

	public override int GetFavorabilityChange()
	{
		int num = Config.Weapon.Instance[TemplateId].BaseFavorabilityChange;
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

	public unsafe HitOrAvoidShorts GetHitFactors()
	{
		HitOrAvoidShorts baseHitFactors = GetBaseHitFactors();
		foreach (EquipmentEffectItem equipmentEffect in DomainManager.Item.GetEquipmentEffects(this))
		{
			for (int i = 0; i < equipmentEffect.HitFactors.Length; i++)
			{
				baseHitFactors[i] += equipmentEffect.HitFactors[i];
			}
		}
		if (ModificationStateHelper.IsActive(ModificationState, 2))
		{
			RefiningEffects refinedEffects = DomainManager.Item.GetRefinedEffects(GetItemKey());
			for (int j = 0; j < 4; j++)
			{
				int weaponPropertyBonus = refinedEffects.GetWeaponPropertyBonus((ERefiningEffectWeaponType)(0 + j));
				weaponPropertyBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(weaponPropertyBonus, EquippedCharId);
				baseHitFactors.Items[j] = (short)(baseHitFactors.Items[j] + Math.Abs(baseHitFactors.Items[j] * weaponPropertyBonus / 100));
			}
		}
		return baseHitFactors;
	}

	public unsafe HitOrAvoidShorts GetHitFactors(int charId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		HitOrAvoidShorts hitFactors = GetHitFactors();
		CValuePercent val = CValuePercent.op_Implicit((int)DomainManager.Character.GetItemPower(charId, GetItemKey()));
		for (int i = 0; i < 4; i++)
		{
			int num = hitFactors.Items[i];
			if (num > 0)
			{
				hitFactors.Items[i] = (short)(num * val);
			}
		}
		return hitFactors;
	}

	public bool TricksMatchCombatSkill(CombatSkillItem combatSkillCfg)
	{
		foreach (NeedTrick item in combatSkillCfg.TrickCost)
		{
			int num = 0;
			foreach (sbyte trick in _tricks)
			{
				if (trick == item.TrickType)
				{
					num++;
				}
			}
			if (num < item.NeedCount)
			{
				return false;
			}
		}
		return true;
	}

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

	public List<sbyte> GetTricks()
	{
		return _tricks;
	}

	public unsafe void SetTricks(List<sbyte> tricks, DataContext context)
	{
		_tricks = tricks;
		SetModifiedAndInvalidateInfluencedCache(4, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _tricks.Count;
			int num = count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 0, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (byte)_tricks[i];
			}
			ptr += num;
		}
	}

	public unsafe override void SetCurrDurability(short currDurability, DataContext context)
	{
		CurrDurability = currDurability;
		SetModifiedAndInvalidateInfluencedCache(5, context);
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
		SetModifiedAndInvalidateInfluencedCache(6, context);
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
		SetModifiedAndInvalidateInfluencedCache(7, context);
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
		SetModifiedAndInvalidateInfluencedCache(8, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, Id, 17u, 12);
			ptr += MaterialResources.Serialize(ptr);
		}
	}

	public short GetPenetrationFactor()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 9))
		{
			return _penetrationFactor;
		}
		short penetrationFactor = CalcPenetrationFactor();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_penetrationFactor = penetrationFactor;
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
		return _penetrationFactor;
	}

	public short GetEquipmentAttack()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 10))
		{
			return _equipmentAttack;
		}
		short equipmentAttack = CalcEquipmentAttack();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_equipmentAttack = equipmentAttack;
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
		return _equipmentAttack;
	}

	public short GetEquipmentDefense()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 11))
		{
			return _equipmentDefense;
		}
		short equipmentDefense = CalcEquipmentDefense();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_equipmentDefense = equipmentDefense;
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
		return _equipmentDefense;
	}

	public override int GetWeight()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 12))
		{
			return _weight;
		}
		int weight = CalcWeight();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_weight = weight;
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
		return _weight;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetName()
	{
		return Config.Weapon.Instance[TemplateId].Name;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetItemType()
	{
		return Config.Weapon.Instance[TemplateId].ItemType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetItemSubType()
	{
		return Config.Weapon.Instance[TemplateId].ItemSubType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetGrade()
	{
		return Config.Weapon.Instance[TemplateId].Grade;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetIcon()
	{
		return Config.Weapon.Instance[TemplateId].Icon;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetDesc()
	{
		return Config.Weapon.Instance[TemplateId].Desc;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetTransferable()
	{
		return Config.Weapon.Instance[TemplateId].Transferable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetStackable()
	{
		return Config.Weapon.Instance[TemplateId].Stackable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetWagerable()
	{
		return Config.Weapon.Instance[TemplateId].Wagerable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRefinable()
	{
		return Config.Weapon.Instance[TemplateId].Refinable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetPoisonable()
	{
		return Config.Weapon.Instance[TemplateId].Poisonable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRepairable()
	{
		return Config.Weapon.Instance[TemplateId].Repairable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseWeight()
	{
		return Config.Weapon.Instance[TemplateId].BaseWeight;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseValue()
	{
		return Config.Weapon.Instance[TemplateId].BaseValue;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetBasePrice()
	{
		return Config.Weapon.Instance[TemplateId].BasePrice;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseFavorabilityChange()
	{
		return Config.Weapon.Instance[TemplateId].BaseFavorabilityChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetDropRate()
	{
		return Config.Weapon.Instance[TemplateId].DropRate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetResourceType()
	{
		return Config.Weapon.Instance[TemplateId].ResourceType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetPreservationDuration()
	{
		return Config.Weapon.Instance[TemplateId].PreservationDuration;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetEquipmentType()
	{
		return Config.Weapon.Instance[TemplateId].EquipmentType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseEquipmentAttack()
	{
		return Config.Weapon.Instance[TemplateId].BaseEquipmentAttack;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseEquipmentDefense()
	{
		return Config.Weapon.Instance[TemplateId].BaseEquipmentDefense;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public ref readonly PoisonsAndLevels GetInnatePoisons()
	{
		return ref Config.Weapon.Instance[TemplateId].InnatePoisons;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<PropertyAndValue> GetRequiredCharacterProperties()
	{
		return Config.Weapon.Instance[TemplateId].RequiredCharacterProperties;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetWeaponAction()
	{
		return Config.Weapon.Instance[TemplateId].WeaponAction;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetCombatPictureR()
	{
		return Config.Weapon.Instance[TemplateId].CombatPictureR;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetCombatPictureL()
	{
		return Config.Weapon.Instance[TemplateId].CombatPictureL;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<string> GetHitSounds()
	{
		return Config.Weapon.Instance[TemplateId].HitSounds;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetSwingSoundsSuffix()
	{
		return Config.Weapon.Instance[TemplateId].SwingSoundsSuffix;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetPlayArmorHitSound()
	{
		return Config.Weapon.Instance[TemplateId].PlayArmorHitSound;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<TrickDistanceAdjust> GetTrickDistanceAdjusts()
	{
		return Config.Weapon.Instance[TemplateId].TrickDistanceAdjusts;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetRandomTrick()
	{
		return Config.Weapon.Instance[TemplateId].RandomTrick;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetCanChangeTrick()
	{
		return Config.Weapon.Instance[TemplateId].CanChangeTrick;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetPursueAttackFactor()
	{
		return Config.Weapon.Instance[TemplateId].PursueAttackFactor;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetAttackPreparePointCost()
	{
		return Config.Weapon.Instance[TemplateId].AttackPreparePointCost;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetMinDistance()
	{
		return Config.Weapon.Instance[TemplateId].MinDistance;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetMaxDistance()
	{
		return Config.Weapon.Instance[TemplateId].MaxDistance;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public HitOrAvoidShorts GetBaseHitFactors()
	{
		return Config.Weapon.Instance[TemplateId].BaseHitFactors;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBasePenetrationFactor()
	{
		return Config.Weapon.Instance[TemplateId].BasePenetrationFactor;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetStanceIncrement()
	{
		return Config.Weapon.Instance[TemplateId].StanceIncrement;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetDefaultInnerRatio()
	{
		return Config.Weapon.Instance[TemplateId].DefaultInnerRatio;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetInnerRatioAdjustRange()
	{
		return Config.Weapon.Instance[TemplateId].InnerRatioAdjustRange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<string> GetBlockParticles()
	{
		return Config.Weapon.Instance[TemplateId].BlockParticles;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetBaseHappinessChange()
	{
		return Config.Weapon.Instance[TemplateId].BaseHappinessChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetGiftLevel()
	{
		return Config.Weapon.Instance[TemplateId].GiftLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetMakeItemSubType()
	{
		return Config.Weapon.Instance[TemplateId].MakeItemSubType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetIdleAni()
	{
		return Config.Weapon.Instance[TemplateId].IdleAni;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetForwardAni()
	{
		return Config.Weapon.Instance[TemplateId].ForwardAni;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetBackwardAni()
	{
		return Config.Weapon.Instance[TemplateId].BackwardAni;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetFastBackwardAni()
	{
		return Config.Weapon.Instance[TemplateId].FastBackwardAni;
	}

	[CollectionObjectField(true, false, false, false, false, ArrayElementsCount = 4)]
	public string[] GetAvoidAnis()
	{
		return Config.Weapon.Instance[TemplateId].AvoidAnis;
	}

	[CollectionObjectField(true, false, false, false, false, ArrayElementsCount = 3)]
	public string[] GetHittedAnis()
	{
		return Config.Weapon.Instance[TemplateId].HittedAnis;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetFatalParticle()
	{
		return Config.Weapon.Instance[TemplateId].FatalParticle;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetTeammateCmdAniPostfix()
	{
		return Config.Weapon.Instance[TemplateId].TeammateCmdAniPostfix;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<string> GetBlockAnis()
	{
		return Config.Weapon.Instance[TemplateId].BlockAnis;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<string> GetBlockSounds()
	{
		return Config.Weapon.Instance[TemplateId].BlockSounds;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetChangeTrickPercent()
	{
		return Config.Weapon.Instance[TemplateId].ChangeTrickPercent;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetFastForwardAni()
	{
		return Config.Weapon.Instance[TemplateId].FastForwardAni;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetDetachable()
	{
		return Config.Weapon.Instance[TemplateId].Detachable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetUnlockEffect()
	{
		return Config.Weapon.Instance[TemplateId].UnlockEffect;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetIsSpecial()
	{
		return Config.Weapon.Instance[TemplateId].IsSpecial;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowRawCreate()
	{
		return Config.Weapon.Instance[TemplateId].AllowRawCreate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetGroupId()
	{
		return Config.Weapon.Instance[TemplateId].GroupId;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowRandomCreate()
	{
		return Config.Weapon.Instance[TemplateId].AllowRandomCreate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetMerchantLevel()
	{
		return Config.Weapon.Instance[TemplateId].MerchantLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetInheritable()
	{
		return Config.Weapon.Instance[TemplateId].Inheritable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowCrippledCreate()
	{
		return Config.Weapon.Instance[TemplateId].AllowCrippledCreate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetEquipmentCombatPowerValueFactor()
	{
		return Config.Weapon.Instance[TemplateId].EquipmentCombatPowerValueFactor;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetBaseStartupFrames()
	{
		return Config.Weapon.Instance[TemplateId].BaseStartupFrames;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetBaseRecoveryFrames()
	{
		return Config.Weapon.Instance[TemplateId].BaseRecoveryFrames;
	}

	public Weapon()
	{
		_tricks = new List<sbyte>();
	}

	public Weapon(short templateId)
	{
		WeaponItem weaponItem = Config.Weapon.Instance[templateId];
		TemplateId = weaponItem.TemplateId;
		MaxDurability = weaponItem.MaxDurability;
		EquipmentEffectId = weaponItem.EquipmentEffectId;
		_tricks = new List<sbyte>(weaponItem.Tricks);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 33;
		int count = _tricks.Count;
		int num2 = count;
		int num3 = 2 + num2;
		return num + num3;
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
		int count = _tricks.Count;
		int num = count;
		if (num > 4194300)
		{
			throw new Exception($"Size of field {"_tricks"} must be less than {4096}KB");
		}
		*(int*)ptr = num + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int i = 0; i < count; i++)
		{
			ptr[i] = (byte)_tricks[i];
		}
		ptr += num;
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
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		_tricks.Clear();
		for (int i = 0; i < num; i++)
		{
			_tricks.Add((sbyte)ptr[i]);
		}
		ptr += (int)num;
		return (int)(ptr - pData);
	}
}
