using System.Collections.Generic;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.DLC.FiveLoong;
using GameData.Domains.Map;
using GameData.Serializer;
using Redzen.Random;

namespace GameData.Domains.Item;

[SerializableGameData(NotForDisplayModule = true)]
public class Carrier : EquipmentBase, ISerializableGameData, IExploreBonusRateItem
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
		return Config.Carrier.Instance[TemplateId].Name;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetItemType()
	{
		return Config.Carrier.Instance[TemplateId].ItemType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetItemSubType()
	{
		return Config.Carrier.Instance[TemplateId].ItemSubType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetGrade()
	{
		return Config.Carrier.Instance[TemplateId].Grade;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetIcon()
	{
		return Config.Carrier.Instance[TemplateId].Icon;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetDesc()
	{
		return Config.Carrier.Instance[TemplateId].Desc;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetTransferable()
	{
		return Config.Carrier.Instance[TemplateId].Transferable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetStackable()
	{
		return Config.Carrier.Instance[TemplateId].Stackable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetWagerable()
	{
		return Config.Carrier.Instance[TemplateId].Wagerable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRefinable()
	{
		return Config.Carrier.Instance[TemplateId].Refinable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetPoisonable()
	{
		return Config.Carrier.Instance[TemplateId].Poisonable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRepairable()
	{
		return Config.Carrier.Instance[TemplateId].Repairable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseWeight()
	{
		return Config.Carrier.Instance[TemplateId].BaseWeight;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseValue()
	{
		return Config.Carrier.Instance[TemplateId].BaseValue;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetBasePrice()
	{
		return Config.Carrier.Instance[TemplateId].BasePrice;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetDropRate()
	{
		return Config.Carrier.Instance[TemplateId].DropRate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetResourceType()
	{
		return Config.Carrier.Instance[TemplateId].ResourceType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetPreservationDuration()
	{
		return Config.Carrier.Instance[TemplateId].PreservationDuration;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetEquipmentType()
	{
		return Config.Carrier.Instance[TemplateId].EquipmentType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetIsFlying()
	{
		return Config.Carrier.Instance[TemplateId].IsFlying;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetMakeItemSubType()
	{
		return Config.Carrier.Instance[TemplateId].MakeItemSubType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetBaseHappinessChange()
	{
		return Config.Carrier.Instance[TemplateId].BaseHappinessChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseFavorabilityChange()
	{
		return Config.Carrier.Instance[TemplateId].BaseFavorabilityChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetGiftLevel()
	{
		return Config.Carrier.Instance[TemplateId].GiftLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<short> GetHateFoodType()
	{
		return Config.Carrier.Instance[TemplateId].HateFoodType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetTamePoint()
	{
		return Config.Carrier.Instance[TemplateId].TamePoint;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetCombatState()
	{
		return Config.Carrier.Instance[TemplateId].CombatState;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseCaptureRateBonus()
	{
		return Config.Carrier.Instance[TemplateId].BaseCaptureRateBonus;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseMaxKidnapSlotCountBonus()
	{
		return Config.Carrier.Instance[TemplateId].BaseMaxKidnapSlotCountBonus;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseMaxInventoryLoadBonus()
	{
		return Config.Carrier.Instance[TemplateId].BaseMaxInventoryLoadBonus;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetBaseTravelTimeReduction()
	{
		return Config.Carrier.Instance[TemplateId].BaseTravelTimeReduction;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<short> GetLoveFoodType()
	{
		return Config.Carrier.Instance[TemplateId].LoveFoodType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseDropRateBonus()
	{
		return Config.Carrier.Instance[TemplateId].BaseDropRateBonus;
	}

	[CollectionObjectField(true, false, false, false, false, ArrayElementsCount = 7)]
	public sbyte[] GetTamePersonalities()
	{
		return Config.Carrier.Instance[TemplateId].TamePersonalities;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetDetachable()
	{
		return Config.Carrier.Instance[TemplateId].Detachable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetCharacterIdInCombat()
	{
		return Config.Carrier.Instance[TemplateId].CharacterIdInCombat;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetMerchantLevel()
	{
		return Config.Carrier.Instance[TemplateId].MerchantLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowRandomCreate()
	{
		return Config.Carrier.Instance[TemplateId].AllowRandomCreate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetTravelSkeleton()
	{
		return Config.Carrier.Instance[TemplateId].TravelSkeleton;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetStandDisplay()
	{
		return Config.Carrier.Instance[TemplateId].StandDisplay;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetGroupId()
	{
		return Config.Carrier.Instance[TemplateId].GroupId;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public ref readonly PoisonsAndLevels GetInnatePoisons()
	{
		return ref Config.Carrier.Instance[TemplateId].InnatePoisons;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetIsSpecial()
	{
		return Config.Carrier.Instance[TemplateId].IsSpecial;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetInheritable()
	{
		return Config.Carrier.Instance[TemplateId].Inheritable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetEquipmentCombatPowerValueFactor()
	{
		return Config.Carrier.Instance[TemplateId].EquipmentCombatPowerValueFactor;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseExploreBonusRate()
	{
		return Config.Carrier.Instance[TemplateId].BaseExploreBonusRate;
	}

	public Carrier()
	{
	}

	public Carrier(short templateId)
	{
		CarrierItem carrierItem = Config.Carrier.Instance[templateId];
		TemplateId = carrierItem.TemplateId;
		MaxDurability = carrierItem.MaxDurability;
		EquipmentEffectId = carrierItem.EquipmentEffectId;
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

	public Carrier(IRandomSource random, short templateId, int itemId)
		: this(templateId)
	{
		Id = itemId;
		MaxDurability = ItemBase.GenerateMaxDurability(random, MaxDurability);
		CurrDurability = MaxDurability;
	}

	public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
	{
		if (type >= ECharacterPropertyReferencedType.PersonalityCalm && type <= ECharacterPropertyReferencedType.PersonalityPerceptive && DomainManager.Extra.IsCarrierFullTamePoint(GetItemKey()) && !IsDurabilityRunningOut())
		{
			int num = (int)(type - 94);
			return GetTamePersonalities()[num];
		}
		return 0;
	}

	public sbyte GetTravelTimeReduction()
	{
		if (IsDurabilityRunningOut())
		{
			return 0;
		}
		sbyte b = Config.Carrier.Instance[TemplateId].BaseTravelTimeReduction;
		ChildrenOfLoong childOfLoong;
		if (DomainManager.Extra.TryGetJiaoIdByItemKey(GetItemKey(), out var id) && DomainManager.Extra.TryGetJiao(id, out var jiao))
		{
			b += (sbyte)jiao.Properties.Get(jiao.TemplateId, 0);
		}
		else if (DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out childOfLoong))
		{
			b += (sbyte)childOfLoong.Properties.Get(childOfLoong.JiaoTemplateId, childOfLoong.LoongTemplateId, 0);
		}
		return b;
	}

	public short GetMaxInventoryLoadBonus()
	{
		if (IsDurabilityRunningOut())
		{
			return 0;
		}
		short num = Config.Carrier.Instance[TemplateId].BaseMaxInventoryLoadBonus;
		ChildrenOfLoong childOfLoong;
		if (DomainManager.Extra.TryGetJiaoIdByItemKey(GetItemKey(), out var id) && DomainManager.Extra.TryGetJiao(id, out var jiao))
		{
			num += (short)jiao.Properties.Get(jiao.TemplateId, 1);
		}
		else if (DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out childOfLoong))
		{
			num += (short)childOfLoong.Properties.Get(childOfLoong.JiaoTemplateId, childOfLoong.LoongTemplateId, 1);
		}
		return num;
	}

	public short GetDropRateBonus()
	{
		if (IsDurabilityRunningOut())
		{
			return 0;
		}
		short num = Config.Carrier.Instance[TemplateId].BaseDropRateBonus;
		ChildrenOfLoong childOfLoong;
		if (DomainManager.Extra.TryGetJiaoIdByItemKey(GetItemKey(), out var id) && DomainManager.Extra.TryGetJiao(id, out var jiao))
		{
			num += (short)jiao.Properties.Get(jiao.TemplateId, 2);
		}
		else if (DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out childOfLoong))
		{
			num += (short)childOfLoong.Properties.Get(childOfLoong.JiaoTemplateId, childOfLoong.LoongTemplateId, 2);
		}
		return num;
	}

	public short GetCaptureRateBonus()
	{
		if (IsDurabilityRunningOut())
		{
			return 0;
		}
		short num = Config.Carrier.Instance[TemplateId].BaseCaptureRateBonus;
		ChildrenOfLoong childOfLoong;
		if (DomainManager.Extra.TryGetJiaoIdByItemKey(GetItemKey(), out var id) && DomainManager.Extra.TryGetJiao(id, out var jiao))
		{
			num += (short)jiao.Properties.Get(jiao.TemplateId, 3);
		}
		else if (DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out childOfLoong))
		{
			num += (short)childOfLoong.Properties.Get(childOfLoong.JiaoTemplateId, childOfLoong.LoongTemplateId, 3);
		}
		return num;
	}

	public int GetExploreBonusRate()
	{
		if (IsDurabilityRunningOut())
		{
			return 0;
		}
		int num = GetBaseExploreBonusRate();
		ChildrenOfLoong childOfLoong;
		if (DomainManager.Extra.TryGetJiaoIdByItemKey(GetItemKey(), out var id) && DomainManager.Extra.TryGetJiao(id, out var jiao))
		{
			num += jiao.Properties.Get(jiao.TemplateId, 5);
		}
		else if (DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out childOfLoong))
		{
			num += childOfLoong.Properties.Get(childOfLoong.JiaoTemplateId, childOfLoong.LoongTemplateId, 5);
		}
		return num;
	}

	public short GetMaxKidnapSlotCountBonus()
	{
		if (IsDurabilityRunningOut())
		{
			return 0;
		}
		short num = Config.Carrier.Instance[TemplateId].BaseMaxKidnapSlotCountBonus;
		ChildrenOfLoong childOfLoong;
		if (DomainManager.Extra.TryGetJiaoIdByItemKey(GetItemKey(), out var id) && DomainManager.Extra.TryGetJiao(id, out var jiao))
		{
			num += (short)jiao.Properties.Get(jiao.TemplateId, 4);
		}
		else if (DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out childOfLoong))
		{
			num += (short)childOfLoong.Properties.Get(childOfLoong.JiaoTemplateId, childOfLoong.LoongTemplateId, 4);
		}
		return num;
	}

	public override int GetValue()
	{
		int num = Config.Carrier.Instance[TemplateId].BaseValue;
		ChildrenOfLoong childOfLoong;
		if (DomainManager.Extra.TryGetJiaoIdByItemKey(GetItemKey(), out var id) && DomainManager.Extra.TryGetJiao(id, out var jiao))
		{
			num += jiao.Properties.Get(jiao.TemplateId, 6);
		}
		else if (DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out childOfLoong))
		{
			num += childOfLoong.Properties.Get(childOfLoong.JiaoTemplateId, childOfLoong.LoongTemplateId, 6);
		}
		return ApplyDurabilityEffect(num);
	}

	public override sbyte GetHappinessChange()
	{
		sbyte b = Config.Carrier.Instance[TemplateId].BaseHappinessChange;
		ChildrenOfLoong childOfLoong;
		if (DomainManager.Extra.TryGetJiaoIdByItemKey(GetItemKey(), out var id) && DomainManager.Extra.TryGetJiao(id, out var jiao))
		{
			b += (sbyte)jiao.Properties.Get(jiao.TemplateId, 7);
		}
		else if (DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out childOfLoong))
		{
			b += (sbyte)childOfLoong.Properties.Get(childOfLoong.JiaoTemplateId, childOfLoong.LoongTemplateId, 7);
		}
		return b;
	}

	public override int GetFavorabilityChange()
	{
		int num = Config.Carrier.Instance[TemplateId].BaseFavorabilityChange;
		ChildrenOfLoong childOfLoong;
		if (DomainManager.Extra.TryGetJiaoIdByItemKey(GetItemKey(), out var id) && DomainManager.Extra.TryGetJiao(id, out var jiao))
		{
			num += jiao.Properties.Get(jiao.TemplateId, 8);
		}
		else if (DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out childOfLoong))
		{
			num += childOfLoong.Properties.Get(childOfLoong.JiaoTemplateId, childOfLoong.LoongTemplateId, 8);
		}
		return num;
	}
}
