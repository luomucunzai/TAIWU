using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Serializer;
using Redzen.Random;

namespace GameData.Domains.Item;

[SerializableGameData(NotForDisplayModule = true)]
public class TeaWine : ItemBase, ISerializableGameData
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

	public TeaWine(IRandomSource random, short templateId, int itemId)
		: this(templateId)
	{
		Id = itemId;
		MaxDurability = ItemBase.GenerateMaxDurability(random, MaxDurability);
		CurrDurability = MaxDurability;
	}

	public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
	{
		return GetCharacterPropertyBonus(TemplateId, type);
	}

	public static int GetCharacterPropertyBonus(short templateId, ECharacterPropertyReferencedType type)
	{
		TeaWineItem teaWineItem = Config.TeaWine.Instance[templateId];
		return teaWineItem.GetCharacterPropertyBonusInt(type);
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
		return Config.TeaWine.Instance[TemplateId].Name;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetItemType()
	{
		return Config.TeaWine.Instance[TemplateId].ItemType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetItemSubType()
	{
		return Config.TeaWine.Instance[TemplateId].ItemSubType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetGrade()
	{
		return Config.TeaWine.Instance[TemplateId].Grade;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetIcon()
	{
		return Config.TeaWine.Instance[TemplateId].Icon;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetDesc()
	{
		return Config.TeaWine.Instance[TemplateId].Desc;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetTransferable()
	{
		return Config.TeaWine.Instance[TemplateId].Transferable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetStackable()
	{
		return Config.TeaWine.Instance[TemplateId].Stackable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetWagerable()
	{
		return Config.TeaWine.Instance[TemplateId].Wagerable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRefinable()
	{
		return Config.TeaWine.Instance[TemplateId].Refinable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetPoisonable()
	{
		return Config.TeaWine.Instance[TemplateId].Poisonable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRepairable()
	{
		return Config.TeaWine.Instance[TemplateId].Repairable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseWeight()
	{
		return Config.TeaWine.Instance[TemplateId].BaseWeight;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseValue()
	{
		return Config.TeaWine.Instance[TemplateId].BaseValue;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetBasePrice()
	{
		return Config.TeaWine.Instance[TemplateId].BasePrice;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetDropRate()
	{
		return Config.TeaWine.Instance[TemplateId].DropRate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetResourceType()
	{
		return Config.TeaWine.Instance[TemplateId].ResourceType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetPreservationDuration()
	{
		return Config.TeaWine.Instance[TemplateId].PreservationDuration;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetDuration()
	{
		return Config.TeaWine.Instance[TemplateId].Duration;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetDirectChangeOfQiDisorder()
	{
		return Config.TeaWine.Instance[TemplateId].DirectChangeOfQiDisorder;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetConsumedFeatureMedals()
	{
		return Config.TeaWine.Instance[TemplateId].ConsumedFeatureMedals;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetHitRateStrength()
	{
		return Config.TeaWine.Instance[TemplateId].HitRateStrength;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetHitRateTechnique()
	{
		return Config.TeaWine.Instance[TemplateId].HitRateTechnique;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetHitRateSpeed()
	{
		return Config.TeaWine.Instance[TemplateId].HitRateSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetHitRateMind()
	{
		return Config.TeaWine.Instance[TemplateId].HitRateMind;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetPenetrateOfOuter()
	{
		return Config.TeaWine.Instance[TemplateId].PenetrateOfOuter;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetPenetrateOfInner()
	{
		return Config.TeaWine.Instance[TemplateId].PenetrateOfInner;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAvoidRateStrength()
	{
		return Config.TeaWine.Instance[TemplateId].AvoidRateStrength;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAvoidRateTechnique()
	{
		return Config.TeaWine.Instance[TemplateId].AvoidRateTechnique;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAvoidRateSpeed()
	{
		return Config.TeaWine.Instance[TemplateId].AvoidRateSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAvoidRateMind()
	{
		return Config.TeaWine.Instance[TemplateId].AvoidRateMind;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetPenetrateResistOfOuter()
	{
		return Config.TeaWine.Instance[TemplateId].PenetrateResistOfOuter;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetPenetrateResistOfInner()
	{
		return Config.TeaWine.Instance[TemplateId].PenetrateResistOfInner;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetInnerRatio()
	{
		return Config.TeaWine.Instance[TemplateId].InnerRatio;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetRecoveryOfQiDisorder()
	{
		return Config.TeaWine.Instance[TemplateId].RecoveryOfQiDisorder;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseFavorabilityChange()
	{
		return Config.TeaWine.Instance[TemplateId].BaseFavorabilityChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetBaseHappinessChange()
	{
		return Config.TeaWine.Instance[TemplateId].BaseHappinessChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetGiftLevel()
	{
		return Config.TeaWine.Instance[TemplateId].GiftLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetSolarTermType()
	{
		return Config.TeaWine.Instance[TemplateId].SolarTermType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetEatHappinessChange()
	{
		return Config.TeaWine.Instance[TemplateId].EatHappinessChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetGroupId()
	{
		return Config.TeaWine.Instance[TemplateId].GroupId;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetBreakBonusEffect()
	{
		return Config.TeaWine.Instance[TemplateId].BreakBonusEffect;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowRandomCreate()
	{
		return Config.TeaWine.Instance[TemplateId].AllowRandomCreate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetMerchantLevel()
	{
		return Config.TeaWine.Instance[TemplateId].MerchantLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetInheritable()
	{
		return Config.TeaWine.Instance[TemplateId].Inheritable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetActionPointRecover()
	{
		return Config.TeaWine.Instance[TemplateId].ActionPointRecover;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetIsSpecial()
	{
		return Config.TeaWine.Instance[TemplateId].IsSpecial;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetBigIcon()
	{
		return Config.TeaWine.Instance[TemplateId].BigIcon;
	}

	public TeaWine()
	{
	}

	public TeaWine(short templateId)
	{
		TeaWineItem teaWineItem = Config.TeaWine.Instance[templateId];
		TemplateId = teaWineItem.TemplateId;
		MaxDurability = teaWineItem.MaxDurability;
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
}
