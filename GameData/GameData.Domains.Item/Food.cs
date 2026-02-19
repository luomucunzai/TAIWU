using System.Collections.Generic;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Serializer;
using Redzen.Random;

namespace GameData.Domains.Item;

[SerializableGameData(NotForDisplayModule = true)]
public class Food : ItemBase, ISerializableGameData
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
		return Config.Food.Instance[TemplateId].Name;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetItemType()
	{
		return Config.Food.Instance[TemplateId].ItemType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetItemSubType()
	{
		return Config.Food.Instance[TemplateId].ItemSubType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetGrade()
	{
		return Config.Food.Instance[TemplateId].Grade;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetIcon()
	{
		return Config.Food.Instance[TemplateId].Icon;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override string GetDesc()
	{
		return Config.Food.Instance[TemplateId].Desc;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetTransferable()
	{
		return Config.Food.Instance[TemplateId].Transferable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetStackable()
	{
		return Config.Food.Instance[TemplateId].Stackable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetWagerable()
	{
		return Config.Food.Instance[TemplateId].Wagerable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRefinable()
	{
		return Config.Food.Instance[TemplateId].Refinable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetPoisonable()
	{
		return Config.Food.Instance[TemplateId].Poisonable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override bool GetRepairable()
	{
		return Config.Food.Instance[TemplateId].Repairable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseWeight()
	{
		return Config.Food.Instance[TemplateId].BaseWeight;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseValue()
	{
		return Config.Food.Instance[TemplateId].BaseValue;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetBasePrice()
	{
		return Config.Food.Instance[TemplateId].BasePrice;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetDropRate()
	{
		return Config.Food.Instance[TemplateId].DropRate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetResourceType()
	{
		return Config.Food.Instance[TemplateId].ResourceType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override short GetPreservationDuration()
	{
		return Config.Food.Instance[TemplateId].PreservationDuration;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetDuration()
	{
		return Config.Food.Instance[TemplateId].Duration;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetConsumedFeatureMedals()
	{
		return Config.Food.Instance[TemplateId].ConsumedFeatureMedals;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public MainAttributes GetMainAttributesRegen()
	{
		return Config.Food.Instance[TemplateId].MainAttributesRegen;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetStrength()
	{
		return Config.Food.Instance[TemplateId].Strength;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetDexterity()
	{
		return Config.Food.Instance[TemplateId].Dexterity;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetConcentration()
	{
		return Config.Food.Instance[TemplateId].Concentration;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetVitality()
	{
		return Config.Food.Instance[TemplateId].Vitality;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetEnergy()
	{
		return Config.Food.Instance[TemplateId].Energy;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetIntelligence()
	{
		return Config.Food.Instance[TemplateId].Intelligence;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetHitRateStrength()
	{
		return Config.Food.Instance[TemplateId].HitRateStrength;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetHitRateTechnique()
	{
		return Config.Food.Instance[TemplateId].HitRateTechnique;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetHitRateSpeed()
	{
		return Config.Food.Instance[TemplateId].HitRateSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetHitRateMind()
	{
		return Config.Food.Instance[TemplateId].HitRateMind;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetPenetrateOfOuter()
	{
		return Config.Food.Instance[TemplateId].PenetrateOfOuter;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetPenetrateOfInner()
	{
		return Config.Food.Instance[TemplateId].PenetrateOfInner;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAvoidRateStrength()
	{
		return Config.Food.Instance[TemplateId].AvoidRateStrength;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAvoidRateTechnique()
	{
		return Config.Food.Instance[TemplateId].AvoidRateTechnique;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAvoidRateSpeed()
	{
		return Config.Food.Instance[TemplateId].AvoidRateSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAvoidRateMind()
	{
		return Config.Food.Instance[TemplateId].AvoidRateMind;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetPenetrateResistOfOuter()
	{
		return Config.Food.Instance[TemplateId].PenetrateResistOfOuter;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetPenetrateResistOfInner()
	{
		return Config.Food.Instance[TemplateId].PenetrateResistOfInner;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetRecoveryOfStance()
	{
		return Config.Food.Instance[TemplateId].RecoveryOfStance;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetRecoveryOfBreath()
	{
		return Config.Food.Instance[TemplateId].RecoveryOfBreath;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetMoveSpeed()
	{
		return Config.Food.Instance[TemplateId].MoveSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetRecoveryOfFlaw()
	{
		return Config.Food.Instance[TemplateId].RecoveryOfFlaw;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetCastSpeed()
	{
		return Config.Food.Instance[TemplateId].CastSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetRecoveryOfBlockedAcupoint()
	{
		return Config.Food.Instance[TemplateId].RecoveryOfBlockedAcupoint;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetWeaponSwitchSpeed()
	{
		return Config.Food.Instance[TemplateId].WeaponSwitchSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetAttackSpeed()
	{
		return Config.Food.Instance[TemplateId].AttackSpeed;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetInnerRatio()
	{
		return Config.Food.Instance[TemplateId].InnerRatio;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetRecoveryOfQiDisorder()
	{
		return Config.Food.Instance[TemplateId].RecoveryOfQiDisorder;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfHotPoison()
	{
		return Config.Food.Instance[TemplateId].ResistOfHotPoison;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfGloomyPoison()
	{
		return Config.Food.Instance[TemplateId].ResistOfGloomyPoison;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfColdPoison()
	{
		return Config.Food.Instance[TemplateId].ResistOfColdPoison;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfRedPoison()
	{
		return Config.Food.Instance[TemplateId].ResistOfRedPoison;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfRottenPoison()
	{
		return Config.Food.Instance[TemplateId].ResistOfRottenPoison;
	}

	[CollectionObjectField(true, false, false, false, true)]
	public short GetResistOfIllusoryPoison()
	{
		return Config.Food.Instance[TemplateId].ResistOfIllusoryPoison;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override int GetBaseFavorabilityChange()
	{
		return Config.Food.Instance[TemplateId].BaseFavorabilityChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public override sbyte GetBaseHappinessChange()
	{
		return Config.Food.Instance[TemplateId].BaseHappinessChange;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetGiftLevel()
	{
		return Config.Food.Instance[TemplateId].GiftLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetInheritable()
	{
		return Config.Food.Instance[TemplateId].Inheritable;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetIsSpecial()
	{
		return Config.Food.Instance[TemplateId].IsSpecial;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetMerchantLevel()
	{
		return Config.Food.Instance[TemplateId].MerchantLevel;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowRandomCreate()
	{
		return Config.Food.Instance[TemplateId].AllowRandomCreate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetBreakBonusEffect()
	{
		return Config.Food.Instance[TemplateId].BreakBonusEffect;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetGroupId()
	{
		return Config.Food.Instance[TemplateId].GroupId;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<EFoodFoodType> GetFoodType()
	{
		return Config.Food.Instance[TemplateId].FoodType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetBigIcon()
	{
		return Config.Food.Instance[TemplateId].BigIcon;
	}

	public Food()
	{
	}

	public Food(short templateId)
	{
		FoodItem foodItem = Config.Food.Instance[templateId];
		TemplateId = foodItem.TemplateId;
		MaxDurability = foodItem.MaxDurability;
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

	public Food(IRandomSource random, short templateId, int itemId)
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
		FoodItem foodItem = Config.Food.Instance[templateId];
		return foodItem.GetCharacterPropertyBonusInt(type);
	}
}
