using System.Collections.Generic;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Domains.Extra;
using GameData.Serializer;
using Redzen.Random;

namespace GameData.Domains.Item;

[SerializableGameData(NotForDisplayModule = true)]
public class Misc : ItemBase, ISerializableGameData
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
		return Config.Misc.Instance[TemplateId].Name;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetItemType()
	{
		return Config.Misc.Instance[TemplateId].ItemType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetItemSubType()
	{
		return Config.Misc.Instance[TemplateId].ItemSubType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetGrade()
	{
		return Config.Misc.Instance[TemplateId].Grade;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetIcon()
	{
		return Config.Misc.Instance[TemplateId].Icon;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetDesc()
	{
		return Config.Misc.Instance[TemplateId].Desc;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetTransferable()
	{
		return Config.Misc.Instance[TemplateId].Transferable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetStackable()
	{
		return Config.Misc.Instance[TemplateId].Stackable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetWagerable()
	{
		return Config.Misc.Instance[TemplateId].Wagerable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRefinable()
	{
		return Config.Misc.Instance[TemplateId].Refinable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetPoisonable()
	{
		return Config.Misc.Instance[TemplateId].Poisonable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRepairable()
	{
		return Config.Misc.Instance[TemplateId].Repairable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseWeight()
	{
		return Config.Misc.Instance[TemplateId].BaseWeight;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseValue()
	{
		return Config.Misc.Instance[TemplateId].BaseValue;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetBasePrice()
	{
		return Config.Misc.Instance[TemplateId].BasePrice;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetDropRate()
	{
		return Config.Misc.Instance[TemplateId].DropRate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetResourceType()
	{
		return Config.Misc.Instance[TemplateId].ResourceType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetPreservationDuration()
	{
		return Config.Misc.Instance[TemplateId].PreservationDuration;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetNeili()
	{
		return Config.Misc.Instance[TemplateId].Neili;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetCricketHealInjuryOdds()
	{
		return Config.Misc.Instance[TemplateId].CricketHealInjuryOdds;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetConsumedFeatureMedals()
	{
		return Config.Misc.Instance[TemplateId].ConsumedFeatureMedals;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetMaxUseDistance()
	{
		return Config.Misc.Instance[TemplateId].MaxUseDistance;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetBaseHappinessChange()
	{
		return Config.Misc.Instance[TemplateId].BaseHappinessChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetMakeItemSubType()
	{
		return Config.Misc.Instance[TemplateId].MakeItemSubType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<TreasureStateInfo> GetStateBuryAmount()
	{
		return Config.Misc.Instance[TemplateId].StateBuryAmount;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetConsumable()
	{
		return Config.Misc.Instance[TemplateId].Consumable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseFavorabilityChange()
	{
		return Config.Misc.Instance[TemplateId].BaseFavorabilityChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetGiftLevel()
	{
		return Config.Misc.Instance[TemplateId].GiftLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<short> GetRequireCombatConfig()
	{
		return Config.Misc.Instance[TemplateId].RequireCombatConfig;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetGroupId()
	{
		return Config.Misc.Instance[TemplateId].GroupId;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetIsSpecial()
	{
		return Config.Misc.Instance[TemplateId].IsSpecial;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowRandomCreate()
	{
		return Config.Misc.Instance[TemplateId].AllowRandomCreate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetInheritable()
	{
		return Config.Misc.Instance[TemplateId].Inheritable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetBreakBonusEffect()
	{
		return Config.Misc.Instance[TemplateId].BreakBonusEffect;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetMerchantLevel()
	{
		return Config.Misc.Instance[TemplateId].MerchantLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<int> GetAllowBrokenLevels()
	{
		return Config.Misc.Instance[TemplateId].AllowBrokenLevels;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public EMiscGenerateType GetGenerateType()
	{
		return Config.Misc.Instance[TemplateId].GenerateType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public EMiscFilterType GetFilterType()
	{
		return Config.Misc.Instance[TemplateId].FilterType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetReduceEscapeRate()
	{
		return Config.Misc.Instance[TemplateId].ReduceEscapeRate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetCombatUseEffect()
	{
		return Config.Misc.Instance[TemplateId].CombatUseEffect;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetCombatPrepareUseEffect()
	{
		return Config.Misc.Instance[TemplateId].CombatPrepareUseEffect;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetGainExp()
	{
		return Config.Misc.Instance[TemplateId].GainExp;
	}

	public Misc()
	{
	}

	public Misc(short templateId)
	{
		MiscItem miscItem = Config.Misc.Instance[templateId];
		TemplateId = miscItem.TemplateId;
		MaxDurability = miscItem.MaxDurability;
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

	public Misc(IRandomSource random, short templateId, int itemId)
		: this(templateId)
	{
		Id = itemId;
		MaxDurability = ItemBase.GenerateMaxDurability(random, MaxDurability);
		CurrDurability = MaxDurability;
	}

	public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
	{
		return 0;
	}
}
