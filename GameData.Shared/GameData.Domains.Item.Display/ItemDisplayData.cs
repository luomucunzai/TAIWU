using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using Config.ConfigCells.Character;
using GameData.DLC;
using GameData.DLC.FiveLoong;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.Extra;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Item.Display;

[SerializableGameData(NotRestrictCollectionSerializedSize = true)]
public class ItemDisplayData : ISerializableGameData
{
	public enum ItemUsingType
	{
		Invalid = -1,
		Reading,
		EquipmentPlaned,
		Equiped,
		Breeding,
		Referring
	}

	public enum ItemUsingOperationType
	{
		Default,
		Sell,
		Store,
		Present,
		Bet,
		Give
	}

	public enum ItemUnavailableType
	{
		Valid,
		NoMeat,
		NoAlcohol,
		HighGrade
	}

	private static readonly LocalObjectPool<List<ItemKey>> LocalObjectPool = new LocalObjectPool<List<ItemKey>>(1, 4);

	[Obsolete("This field is obsolete, and will be removed in future. Use EquipmentEffectIds instead.")]
	public short EquipmentEffectId;

	[SerializableGameDataField]
	private ItemKey _key;

	[SerializableGameDataField]
	public int Amount;

	[SerializableGameDataField]
	public short Durability;

	[SerializableGameDataField]
	public short MaxDurability;

	[SerializableGameDataField]
	public int Weight;

	[SerializableGameDataField]
	public int Value;

	[SerializableGameDataField]
	public int SpecialArg;

	[SerializableGameDataField]
	public List<short> EquipmentEffectIds;

	[SerializableGameDataField]
	public ItemPowerInfo PowerInfo;

	[SerializableGameDataField]
	public List<(int type, int required, int actual)> Requirements;

	[SerializableGameDataField]
	public short EquipmentAttack;

	[SerializableGameDataField]
	public short EquipmentDefense;

	[SerializableGameDataField]
	public short WeavedClothingTemplateId;

	[SerializableGameDataField]
	public HitOrAvoidShorts HitAvoidFactor;

	[SerializableGameDataField]
	public (short, short) PenetrationInfo;

	[SerializableGameDataField]
	public OuterAndInnerShorts InjuryFactors;

	[SerializableGameDataField]
	public RefiningEffects RefiningEffects;

	[SerializableGameDataField]
	public FullPoisonEffects PoisonEffects;

	[SerializableGameDataField]
	public MaterialResources MaterialResources;

	[SerializableGameDataField]
	public Dictionary<ItemKey, int> MergedPoisonItemDict;

	[SerializableGameDataField]
	public Dictionary<ItemKey, int> MergedExtraGoodsItemDict;

	[SerializableGameDataField]
	public sbyte ItemSourceType;

	[SerializableGameDataField]
	private sbyte _usingType;

	[SerializableGameDataField]
	public LoveTokenDataItem LoveTokenDataItem;

	[SerializableGameDataField]
	public int OwnerCharId;

	[SerializableGameDataField]
	public bool IsReadingFinished;

	[SerializableGameDataField]
	public JiaoEggItemDisplayData JiaoEggItemDisplayData;

	[SerializableGameDataField]
	public int CarrierTamePoint;

	[SerializableGameDataField]
	public bool IsThreeCorpseKeepingLegendaryBook;

	[SerializableGameDataField]
	public sbyte ExtraGoodsType;

	[SerializableGameDataField]
	private sbyte _unavailableType;

	public ETreasuryOperation TreasuryOperation;

	public EItemDebtState ItemDebtState;

	public int ItemShopLevel;

	public bool Interactable;

	public ItemKey Key
	{
		get
		{
			Dictionary<ItemKey, int> mergedPoisonItemDict = MergedPoisonItemDict;
			if (mergedPoisonItemDict == null || mergedPoisonItemDict.Count <= 0)
			{
				mergedPoisonItemDict = MergedExtraGoodsItemDict;
				if (mergedPoisonItemDict == null || mergedPoisonItemDict.Count <= 0)
				{
					return _key;
				}
			}
			List<ItemKey> allItemKeysFromPool = GetAllItemKeysFromPool();
			int index = new Random().Next(0, allItemKeysFromPool.Count);
			ItemKey result = allItemKeysFromPool[index];
			ReturnItemKeyListToPool(allItemKeysFromPool);
			return result;
		}
		set
		{
			_key = value;
		}
	}

	public ItemKey RealKey => _key;

	public ItemSourceType ItemSourceTypeEnum => (ItemSourceType)ItemSourceType;

	public ItemUsingType UsingType
	{
		get
		{
			return (ItemUsingType)_usingType;
		}
		set
		{
			_usingType = (sbyte)value;
		}
	}

	public ItemUnavailableType UnavailableType
	{
		get
		{
			return (ItemUnavailableType)_unavailableType;
		}
		set
		{
			_unavailableType = (sbyte)value;
		}
	}

	public bool IsWeaved
	{
		get
		{
			if (_key.ItemType == 3 && WeavedClothingTemplateId >= 0)
			{
				return WeavedClothingTemplateId != _key.TemplateId;
			}
			return false;
		}
	}

	public bool HasAnyPoison
	{
		get
		{
			if (ModificationStateHelper.IsActive(_key.ModificationState, 1))
			{
				return true;
			}
			if (MergedPoisonItemDict != null && MergedPoisonItemDict.Count > 0)
			{
				return true;
			}
			return false;
		}
	}

	public bool HasAnyExtraGoods
	{
		get
		{
			if (ModificationStateHelper.IsActive(_key.ModificationState, 8))
			{
				return true;
			}
			if (MergedExtraGoodsItemDict != null && MergedExtraGoodsItemDict.Count > 0)
			{
				return true;
			}
			return false;
		}
	}

	public bool PoisonIsIdentified => PoisonEffects?.IsIdentified ?? false;

	public bool IsResource => ItemTemplateHelper.IsMiscResource(_key.ItemType, _key.TemplateId);

	public sbyte ResourceType
	{
		get
		{
			if (!IsResource)
			{
				return -1;
			}
			return ItemTemplateHelper.GetMiscResourceType(_key.ItemType, _key.TemplateId);
		}
	}

	public MerchantExtraGoodsType ExtraGoodsTypeEnum => (MerchantExtraGoodsType)ExtraGoodsType;

	public sbyte Grade => ItemTemplateHelper.GetGrade(RealKey.ItemType, RealKey.TemplateId);

	[Obsolete("This property is obsolete, use PowerInfo.Power instead.")]
	public short EquipCurrPower => PowerInfo.Power;

	[Obsolete("This property is obsolete, use PowerInfo.MaxPower instead.")]
	public short EquipMaxPower => PowerInfo.MaxPower;

	public short CricketColorId => (short)(SpecialArg >> 16);

	public short CricketPartId => (short)(SpecialArg & 0xFFFF);

	public static List<ItemKey> GetItemKeyListFromPool()
	{
		return LocalObjectPool.Get();
	}

	public static void ReturnItemKeyListToPool(List<ItemKey> list)
	{
		LocalObjectPool.Return(list);
	}

	public List<ItemKey> GetAllItemKeysFromPool()
	{
		List<ItemKey> itemKeyListFromPool = GetItemKeyListFromPool();
		int num = 0;
		if (IsResource)
		{
			itemKeyListFromPool.Add(_key);
		}
		else
		{
			if (MergedPoisonItemDict != null && MergedPoisonItemDict.Count > 0)
			{
				foreach (KeyValuePair<ItemKey, int> item in MergedPoisonItemDict)
				{
					for (int i = 0; i < item.Value; i++)
					{
						itemKeyListFromPool.Add(item.Key);
						num++;
					}
				}
			}
			if (MergedExtraGoodsItemDict != null && MergedExtraGoodsItemDict.Count > 0)
			{
				foreach (KeyValuePair<ItemKey, int> item2 in MergedExtraGoodsItemDict)
				{
					for (int j = 0; j < item2.Value; j++)
					{
						itemKeyListFromPool.Add(item2.Key);
						num++;
					}
				}
			}
			for (int k = 0; k < Amount - num; k++)
			{
				itemKeyListFromPool.Add(_key);
			}
		}
		return itemKeyListFromPool;
	}

	public List<ItemKey> GetOperationKeyListFromPool(int amount, bool preview = false)
	{
		Tester.Assert(amount <= Amount && amount > 0 && !IsResource);
		List<ItemKey> list = GetAllItemKeysFromPool();
		if (amount < Amount)
		{
			List<ItemKey> itemKeyListFromPool = GetItemKeyListFromPool();
			int num = amount;
			while (num > 0)
			{
				num--;
				int index = new Random().Next(0, list.Count);
				itemKeyListFromPool.Add(list[index]);
				list.RemoveAt(index);
			}
			ReturnItemKeyListToPool(list);
			list = itemKeyListFromPool;
		}
		if (!preview)
		{
			ChangePoisonData(list);
			ChangeExtraGoodData(list);
		}
		return list;
	}

	private void ChangePoisonData(List<ItemKey> keyList)
	{
		if (MergedPoisonItemDict == null || MergedPoisonItemDict.Count <= 0)
		{
			return;
		}
		foreach (ItemKey key in keyList)
		{
			if (MergedPoisonItemDict.TryGetValue(key, out var value))
			{
				value--;
				if (value <= 0)
				{
					MergedPoisonItemDict.Remove(key);
				}
				else
				{
					MergedPoisonItemDict[key] = value;
				}
			}
		}
	}

	private void ChangeExtraGoodData(List<ItemKey> keyList)
	{
		if (MergedExtraGoodsItemDict == null || MergedExtraGoodsItemDict.Count <= 0)
		{
			return;
		}
		foreach (ItemKey key in keyList)
		{
			if (MergedExtraGoodsItemDict.TryGetValue(key, out var value))
			{
				value--;
				if (value <= 0)
				{
					MergedExtraGoodsItemDict.Remove(key);
				}
				else
				{
					MergedExtraGoodsItemDict[key] = value;
				}
			}
		}
	}

	public bool PoisonEquals(ItemDisplayData other)
	{
		if (_key.TemplateEquals(other._key))
		{
			return PoisonEffects == other.PoisonEffects;
		}
		return false;
	}

	public bool ContainsItemKey(ItemKey targetKey)
	{
		if (_key.Equals(targetKey))
		{
			return true;
		}
		if (targetKey.IsValid() && (HasAnyPoison || HasAnyExtraGoods) && ItemTemplateHelper.IsStackable(targetKey.ItemType, targetKey.TemplateId))
		{
			List<ItemKey> allItemKeysFromPool = GetAllItemKeysFromPool();
			bool result = allItemKeysFromPool.Contains(targetKey);
			ReturnItemKeyListToPool(allItemKeysFromPool);
			return result;
		}
		return false;
	}

	public bool CanMerge(ItemDisplayData other)
	{
		if (!_key.TemplateEquals(other._key))
		{
			return false;
		}
		if (ItemTemplateHelper.IsMiscResource(_key.ItemType, _key.TemplateId))
		{
			return true;
		}
		if (!ItemTemplateHelper.IsStackable(other.Key.ItemType, other.Key.TemplateId))
		{
			return false;
		}
		if (ModificationStateHelper.IsActive(_key.ModificationState, 8) || ModificationStateHelper.IsActive(other._key.ModificationState, 8))
		{
			if (ExtraGoodsTypeEnum > MerchantExtraGoodsType.None)
			{
				return ExtraGoodsTypeEnum == other.ExtraGoodsTypeEnum;
			}
			return false;
		}
		if (PoisonIsIdentified && other.PoisonIsIdentified)
		{
			return PoisonEquals(other);
		}
		if (!PoisonIsIdentified && !other.PoisonIsIdentified)
		{
			return true;
		}
		return false;
	}

	public ItemDisplayData()
	{
		_key = ItemKey.Invalid;
		RefiningEffects.Initialize();
		UsingType = ItemUsingType.Invalid;
		LoveTokenDataItem = new LoveTokenDataItem();
		OwnerCharId = -1;
		WeavedClothingTemplateId = -1;
	}

	public ItemDisplayData Clone(int amount = -1)
	{
		ItemDisplayData itemDisplayData = new ItemDisplayData
		{
			_key = _key,
			Amount = ((amount == -1) ? Amount : amount),
			Durability = Durability,
			MaxDurability = MaxDurability,
			Weight = Weight,
			Value = Value,
			SpecialArg = SpecialArg,
			EquipmentEffectIds = ((EquipmentEffectIds == null) ? null : new List<short>(EquipmentEffectIds)),
			EquipmentAttack = EquipmentAttack,
			EquipmentDefense = EquipmentDefense,
			WeavedClothingTemplateId = WeavedClothingTemplateId,
			HitAvoidFactor = HitAvoidFactor,
			PenetrationInfo = PenetrationInfo,
			InjuryFactors = InjuryFactors,
			PowerInfo = PowerInfo,
			RefiningEffects = RefiningEffects,
			PoisonEffects = PoisonEffects,
			MaterialResources = MaterialResources,
			MergedPoisonItemDict = MergedPoisonItemDict,
			MergedExtraGoodsItemDict = MergedExtraGoodsItemDict,
			UsingType = UsingType,
			ItemSourceType = ItemSourceType,
			LoveTokenDataItem = LoveTokenDataItem,
			OwnerCharId = OwnerCharId,
			IsReadingFinished = IsReadingFinished,
			JiaoEggItemDisplayData = JiaoEggItemDisplayData,
			CarrierTamePoint = CarrierTamePoint,
			ExtraGoodsType = ExtraGoodsType
		};
		if (Requirements != null)
		{
			itemDisplayData.Requirements = new List<(int, int, int)>();
			itemDisplayData.Requirements.AddRange(Requirements);
		}
		return itemDisplayData;
	}

	public ItemDisplayData Clone(ItemKey itemKey, sbyte itemSourceType)
	{
		ItemDisplayData itemDisplayData = Clone(1);
		itemDisplayData._key = itemKey;
		itemDisplayData.ItemSourceType = itemSourceType;
		itemDisplayData.MergedPoisonItemDict = new Dictionary<ItemKey, int>();
		itemDisplayData.MergedExtraGoodsItemDict = new Dictionary<ItemKey, int>();
		itemDisplayData.PoisonEffects = ((PoisonEffects == null) ? null : new FullPoisonEffects(PoisonEffects));
		return itemDisplayData;
	}

	public ItemDisplayData Clone(List<ItemKey> keyList, sbyte itemSourceType)
	{
		ItemDisplayData itemDisplayData = Clone(keyList.Count);
		itemDisplayData.ItemSourceType = itemSourceType;
		itemDisplayData.MergedPoisonItemDict = new Dictionary<ItemKey, int>();
		itemDisplayData.MergedExtraGoodsItemDict = new Dictionary<ItemKey, int>();
		int num = keyList.FindIndex((ItemKey k) => !ModificationStateHelper.IsActive(k.ModificationState, 1));
		itemDisplayData._key = ((num >= 0) ? keyList[num] : keyList.First());
		if (keyList.Count > 1)
		{
			foreach (ItemKey key in keyList)
			{
				if (!itemDisplayData._key.Equals(key))
				{
					if (ModificationStateHelper.IsActive(key.ModificationState, 1))
					{
						itemDisplayData.MergedPoisonItemDict.TryGetValue(key, out var value);
						value++;
						itemDisplayData.MergedPoisonItemDict[key] = value;
					}
					if (ModificationStateHelper.IsActive(key.ModificationState, 8))
					{
						itemDisplayData.MergedExtraGoodsItemDict.TryGetValue(key, out var value2);
						value2++;
						itemDisplayData.MergedExtraGoodsItemDict[key] = value2;
					}
				}
			}
		}
		itemDisplayData.PoisonEffects = ((PoisonEffects == null) ? null : new FullPoisonEffects(PoisonEffects));
		return itemDisplayData;
	}

	public void ChangeAmount(List<ItemKey> keyList, bool isAdd)
	{
		foreach (ItemKey key in keyList)
		{
			ChangeAmount(key, isAdd);
		}
	}

	public void ChangeAmount(ItemKey itemKey, bool isAdd)
	{
		if (isAdd)
		{
			Amount++;
		}
		else
		{
			Amount--;
		}
		if (MergedExtraGoodsItemDict == null)
		{
			MergedExtraGoodsItemDict = new Dictionary<ItemKey, int>();
		}
		if (ModificationStateHelper.IsActive(itemKey.ModificationState, 8))
		{
			MergedExtraGoodsItemDict.TryGetValue(itemKey, out var value);
			value = ((!isAdd) ? (value - 1) : (value + 1));
			MergedExtraGoodsItemDict[itemKey] = value;
			if (value <= 0)
			{
				MergedExtraGoodsItemDict.Remove(itemKey);
			}
		}
		else if (ModificationStateHelper.IsActive(_key.ModificationState, 8))
		{
			MergedExtraGoodsItemDict[_key] = 1;
			_key = itemKey;
		}
		if (MergedPoisonItemDict == null)
		{
			MergedPoisonItemDict = new Dictionary<ItemKey, int>();
		}
		if (ModificationStateHelper.IsActive(itemKey.ModificationState, 1))
		{
			MergedPoisonItemDict.TryGetValue(itemKey, out var value2);
			value2 = ((!isAdd) ? (value2 - 1) : (value2 + 1));
			MergedPoisonItemDict[itemKey] = value2;
			if (value2 <= 0)
			{
				MergedPoisonItemDict.Remove(itemKey);
			}
		}
		else if (ModificationStateHelper.IsActive(_key.ModificationState, 1))
		{
			MergedPoisonItemDict[_key] = 1;
			_key = itemKey;
		}
	}

	public ItemDisplayData(ItemKey itemKey, int amount)
	{
		_key = itemKey;
		Amount = amount;
		MaterialResources.Initialize();
		RefiningEffects.Initialize();
		EquipmentEffectIds = null;
	}

	public ItemDisplayData(sbyte itemType, short templateId)
	{
		_key = new ItemKey(itemType, 0, templateId, -1);
		Amount = 1;
		Durability = (MaxDurability = ItemTemplateHelper.GetBaseMaxDurability(itemType, templateId));
		Weight = ItemTemplateHelper.GetBaseWeight(itemType, templateId);
		Value = ItemTemplateHelper.GetBaseValue(itemType, templateId);
		EquipmentEffectIds = null;
		MaterialResources.Initialize();
		WeavedClothingTemplateId = -1;
		switch (itemType)
		{
		case 0:
		{
			WeaponItem weaponItem = Weapon.Instance[templateId];
			EquipmentAttack = weaponItem.BaseEquipmentAttack;
			EquipmentDefense = weaponItem.BaseEquipmentDefense;
			HitAvoidFactor = weaponItem.BaseHitFactors;
			PenetrationInfo.Item1 = weaponItem.BasePenetrationFactor;
			break;
		}
		case 1:
		{
			ArmorItem armorItem = Armor.Instance[templateId];
			EquipmentAttack = armorItem.BaseEquipmentAttack;
			EquipmentDefense = armorItem.BaseEquipmentDefense;
			HitAvoidFactor = armorItem.BaseAvoidFactors;
			armorItem.BasePenetrationResistFactors.Deconstruct(out PenetrationInfo.Item1, out PenetrationInfo.Item2);
			InjuryFactors = armorItem.BaseInjuryFactors;
			break;
		}
		case 11:
			AdaptableLog.Warning("ItemDisplayData does not support cricket construction", appendWarningMessage: true);
			break;
		}
		RefiningEffects.Initialize();
		if ((uint)itemType <= 1u)
		{
			Requirements = new List<(int, int, int)>(((itemType == 0) ? Weapon.Instance[templateId].RequiredCharacterProperties : Armor.Instance[templateId].RequiredCharacterProperties).Select((PropertyAndValue x) => ((int, int, int))(x.PropertyId, x.Value, -1)));
			PowerInfo = ItemPowerInfo.Default;
		}
		else if (itemType == 12 && GameData.Domains.Combat.SharedConstValue.SwordFragment2BossId.ContainsKey(_key.TemplateId))
		{
			AdaptableLog.Warning("ItemDisplayData does not support sword fragment construction", appendWarningMessage: true);
		}
		ItemSourceType = -1;
		UsingType = ItemUsingType.Invalid;
		LoveTokenDataItem = new LoveTokenDataItem();
		OwnerCharId = -1;
	}

	public bool CanSetEquipmentEffect()
	{
		sbyte itemType = Key.ItemType;
		if (!ItemType.IsEquipmentItemType(itemType))
		{
			return false;
		}
		if (itemType == 3 || itemType == 4)
		{
			return false;
		}
		List<short> equipmentEffectIds = EquipmentEffectIds;
		if (equipmentEffectIds != null && equipmentEffectIds.Count > 0 && EquipmentEffect.Instance[EquipmentEffectIds[0]].Special)
		{
			return false;
		}
		return true;
	}

	public static void ClearItemUsingState(ItemDisplayData itemData, List<ItemDisplayData> itemList)
	{
		ItemDisplayData itemDisplayData = itemList?.Find((ItemDisplayData data) => data.ContainsItemKey(itemData.Key));
		if (itemDisplayData != null)
		{
			itemDisplayData.UsingType = ItemUsingType.Invalid;
		}
	}

	public override string ToString()
	{
		return $"ItemDisplayData({Key}, {Amount}, {Durability}/{MaxDurability})";
	}

	public ItemDisplayData(ItemDisplayData other)
	{
		_key = other._key;
		Amount = other.Amount;
		Durability = other.Durability;
		MaxDurability = other.MaxDurability;
		Weight = other.Weight;
		Value = other.Value;
		SpecialArg = other.SpecialArg;
		EquipmentEffectIds = ((other.EquipmentEffectIds == null) ? null : new List<short>(other.EquipmentEffectIds));
		PowerInfo = other.PowerInfo;
		Requirements = ((other.Requirements == null) ? null : new List<(int, int, int)>(other.Requirements));
		EquipmentAttack = other.EquipmentAttack;
		EquipmentDefense = other.EquipmentDefense;
		WeavedClothingTemplateId = other.WeavedClothingTemplateId;
		HitAvoidFactor = other.HitAvoidFactor;
		PenetrationInfo = other.PenetrationInfo;
		InjuryFactors = other.InjuryFactors;
		RefiningEffects = other.RefiningEffects;
		PoisonEffects = new FullPoisonEffects(other.PoisonEffects);
		MaterialResources = other.MaterialResources;
		MergedPoisonItemDict = ((other.MergedPoisonItemDict == null) ? null : new Dictionary<ItemKey, int>(other.MergedPoisonItemDict));
		MergedExtraGoodsItemDict = ((other.MergedExtraGoodsItemDict == null) ? null : new Dictionary<ItemKey, int>(other.MergedExtraGoodsItemDict));
		ItemSourceType = other.ItemSourceType;
		_usingType = other._usingType;
		LoveTokenDataItem = new LoveTokenDataItem(other.LoveTokenDataItem);
		OwnerCharId = other.OwnerCharId;
		IsReadingFinished = other.IsReadingFinished;
		JiaoEggItemDisplayData = other.JiaoEggItemDisplayData;
		CarrierTamePoint = other.CarrierTamePoint;
		IsThreeCorpseKeepingLegendaryBook = other.IsThreeCorpseKeepingLegendaryBook;
		ExtraGoodsType = other.ExtraGoodsType;
		_unavailableType = other._unavailableType;
	}

	public void Assign(ItemDisplayData other)
	{
		_key = other._key;
		Amount = other.Amount;
		Durability = other.Durability;
		MaxDurability = other.MaxDurability;
		Weight = other.Weight;
		Value = other.Value;
		SpecialArg = other.SpecialArg;
		EquipmentEffectIds = ((other.EquipmentEffectIds == null) ? null : new List<short>(other.EquipmentEffectIds));
		PowerInfo = other.PowerInfo;
		Requirements = ((other.Requirements == null) ? null : new List<(int, int, int)>(other.Requirements));
		EquipmentAttack = other.EquipmentAttack;
		EquipmentDefense = other.EquipmentDefense;
		WeavedClothingTemplateId = other.WeavedClothingTemplateId;
		HitAvoidFactor = other.HitAvoidFactor;
		PenetrationInfo = other.PenetrationInfo;
		InjuryFactors = other.InjuryFactors;
		RefiningEffects = other.RefiningEffects;
		PoisonEffects = new FullPoisonEffects(other.PoisonEffects);
		MaterialResources = other.MaterialResources;
		MergedPoisonItemDict = ((other.MergedPoisonItemDict == null) ? null : new Dictionary<ItemKey, int>(other.MergedPoisonItemDict));
		MergedExtraGoodsItemDict = ((other.MergedExtraGoodsItemDict == null) ? null : new Dictionary<ItemKey, int>(other.MergedExtraGoodsItemDict));
		ItemSourceType = other.ItemSourceType;
		_usingType = other._usingType;
		LoveTokenDataItem = new LoveTokenDataItem(other.LoveTokenDataItem);
		OwnerCharId = other.OwnerCharId;
		IsReadingFinished = other.IsReadingFinished;
		JiaoEggItemDisplayData = other.JiaoEggItemDisplayData;
		CarrierTamePoint = other.CarrierTamePoint;
		IsThreeCorpseKeepingLegendaryBook = other.IsThreeCorpseKeepingLegendaryBook;
		ExtraGoodsType = other.ExtraGoodsType;
		_unavailableType = other._unavailableType;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 126;
		num = ((EquipmentEffectIds == null) ? (num + 2) : (num + (2 + 2 * EquipmentEffectIds.Count)));
		if (Requirements != null)
		{
			num += 2;
			int count = Requirements.Count;
			for (int i = 0; i < count; i++)
			{
				(int, int, int) tuple = Requirements[i];
				num += SerializationHelper.GetSerializedSize<int, int, int>(tuple);
			}
		}
		else
		{
			num += 2;
		}
		num = ((PoisonEffects == null) ? (num + 2) : (num + (2 + PoisonEffects.GetSerializedSize())));
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, int>(MergedPoisonItemDict);
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, int>(MergedExtraGoodsItemDict);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += _key.Serialize(ptr);
		*(int*)ptr = Amount;
		ptr += 4;
		*(short*)ptr = Durability;
		ptr += 2;
		*(short*)ptr = MaxDurability;
		ptr += 2;
		*(int*)ptr = Weight;
		ptr += 4;
		*(int*)ptr = Value;
		ptr += 4;
		*(int*)ptr = SpecialArg;
		ptr += 4;
		if (EquipmentEffectIds != null)
		{
			int count = EquipmentEffectIds.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = EquipmentEffectIds[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += PowerInfo.Serialize(ptr);
		if (Requirements != null)
		{
			int count2 = Requirements.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				(int, int, int) tuple = Requirements[j];
				ptr += SerializationHelper.Serialize<int, int, int>(ptr, tuple);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(short*)ptr = EquipmentAttack;
		ptr += 2;
		*(short*)ptr = EquipmentDefense;
		ptr += 2;
		*(short*)ptr = WeavedClothingTemplateId;
		ptr += 2;
		ptr += HitAvoidFactor.Serialize(ptr);
		ptr += SerializationHelper.Serialize<short, short>(ptr, PenetrationInfo);
		ptr += InjuryFactors.Serialize(ptr);
		ptr += RefiningEffects.Serialize(ptr);
		if (PoisonEffects != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = PoisonEffects.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += MaterialResources.Serialize(ptr);
		ptr += DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, int>(ptr, ref MergedPoisonItemDict);
		ptr += DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, int>(ptr, ref MergedExtraGoodsItemDict);
		*ptr = (byte)ItemSourceType;
		ptr++;
		*ptr = (byte)_usingType;
		ptr++;
		ptr += LoveTokenDataItem.Serialize(ptr);
		*(int*)ptr = OwnerCharId;
		ptr += 4;
		*ptr = (IsReadingFinished ? ((byte)1) : ((byte)0));
		ptr++;
		ptr += JiaoEggItemDisplayData.Serialize(ptr);
		*(int*)ptr = CarrierTamePoint;
		ptr += 4;
		*ptr = (IsThreeCorpseKeepingLegendaryBook ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)ExtraGoodsType;
		ptr++;
		*ptr = (byte)_unavailableType;
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
		ptr += _key.Deserialize(ptr);
		Amount = *(int*)ptr;
		ptr += 4;
		Durability = *(short*)ptr;
		ptr += 2;
		MaxDurability = *(short*)ptr;
		ptr += 2;
		Weight = *(int*)ptr;
		ptr += 4;
		Value = *(int*)ptr;
		ptr += 4;
		SpecialArg = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (EquipmentEffectIds == null)
			{
				EquipmentEffectIds = new List<short>(num);
			}
			else
			{
				EquipmentEffectIds.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				EquipmentEffectIds.Add(((short*)ptr)[i]);
			}
			ptr += 2 * num;
		}
		else
		{
			EquipmentEffectIds?.Clear();
		}
		ptr += PowerInfo.Deserialize(ptr);
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (Requirements == null)
			{
				Requirements = new List<(int, int, int)>(num2);
			}
			else
			{
				Requirements.Clear();
			}
			(int, int, int) item = default((int, int, int));
			for (int j = 0; j < num2; j++)
			{
				ptr += SerializationHelper.Deserialize<int, int, int>(ptr, ref item);
				Requirements.Add(item);
			}
		}
		else
		{
			Requirements?.Clear();
		}
		EquipmentAttack = *(short*)ptr;
		ptr += 2;
		EquipmentDefense = *(short*)ptr;
		ptr += 2;
		WeavedClothingTemplateId = *(short*)ptr;
		ptr += 2;
		ptr += HitAvoidFactor.Deserialize(ptr);
		ptr += SerializationHelper.Deserialize<short, short>(ptr, ref PenetrationInfo);
		ptr += InjuryFactors.Deserialize(ptr);
		ptr += RefiningEffects.Deserialize(ptr);
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (PoisonEffects == null)
			{
				PoisonEffects = new FullPoisonEffects();
			}
			ptr += PoisonEffects.Deserialize(ptr);
		}
		else
		{
			PoisonEffects = null;
		}
		ptr += MaterialResources.Deserialize(ptr);
		ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, int>(ptr, ref MergedPoisonItemDict);
		ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, int>(ptr, ref MergedExtraGoodsItemDict);
		ItemSourceType = (sbyte)(*ptr);
		ptr++;
		_usingType = (sbyte)(*ptr);
		ptr++;
		if (LoveTokenDataItem == null)
		{
			LoveTokenDataItem = new LoveTokenDataItem();
		}
		ptr += LoveTokenDataItem.Deserialize(ptr);
		OwnerCharId = *(int*)ptr;
		ptr += 4;
		IsReadingFinished = *ptr != 0;
		ptr++;
		ptr += JiaoEggItemDisplayData.Deserialize(ptr);
		CarrierTamePoint = *(int*)ptr;
		ptr += 4;
		IsThreeCorpseKeepingLegendaryBook = *ptr != 0;
		ptr++;
		ExtraGoodsType = (sbyte)(*ptr);
		ptr++;
		_unavailableType = (sbyte)(*ptr);
		ptr++;
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
