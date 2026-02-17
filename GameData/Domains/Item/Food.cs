using System;
using System.Collections.Generic;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Serializer;
using Redzen.Random;

namespace GameData.Domains.Item
{
	// Token: 0x0200066B RID: 1643
	[SerializableGameData(NotForDisplayModule = true)]
	public class Food : ItemBase, ISerializableGameData
	{
		// Token: 0x060050F6 RID: 20726 RVA: 0x002C9C08 File Offset: 0x002C7E08
		public unsafe override void SetMaxDurability(short maxDurability, DataContext context)
		{
			this.MaxDurability = maxDurability;
			base.SetModifiedAndInvalidateInfluencedCache(2, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 6U, 2);
				*(short*)pData = this.MaxDurability;
				pData += 2;
			}
		}

		// Token: 0x060050F7 RID: 20727 RVA: 0x002C9C68 File Offset: 0x002C7E68
		public unsafe override void SetCurrDurability(short currDurability, DataContext context)
		{
			this.CurrDurability = currDurability;
			base.SetModifiedAndInvalidateInfluencedCache(3, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 8U, 2);
				*(short*)pData = this.CurrDurability;
				pData += 2;
			}
		}

		// Token: 0x060050F8 RID: 20728 RVA: 0x002C9CC8 File Offset: 0x002C7EC8
		public unsafe override void SetModificationState(byte modificationState, DataContext context)
		{
			this.ModificationState = modificationState;
			base.SetModifiedAndInvalidateInfluencedCache(4, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 10U, 1);
				*pData = this.ModificationState;
				pData++;
			}
		}

		// Token: 0x060050F9 RID: 20729 RVA: 0x002C9D28 File Offset: 0x002C7F28
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetName()
		{
			return Food.Instance[this.TemplateId].Name;
		}

		// Token: 0x060050FA RID: 20730 RVA: 0x002C9D50 File Offset: 0x002C7F50
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetItemType()
		{
			return Food.Instance[this.TemplateId].ItemType;
		}

		// Token: 0x060050FB RID: 20731 RVA: 0x002C9D78 File Offset: 0x002C7F78
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetItemSubType()
		{
			return Food.Instance[this.TemplateId].ItemSubType;
		}

		// Token: 0x060050FC RID: 20732 RVA: 0x002C9DA0 File Offset: 0x002C7FA0
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetGrade()
		{
			return Food.Instance[this.TemplateId].Grade;
		}

		// Token: 0x060050FD RID: 20733 RVA: 0x002C9DC8 File Offset: 0x002C7FC8
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetIcon()
		{
			return Food.Instance[this.TemplateId].Icon;
		}

		// Token: 0x060050FE RID: 20734 RVA: 0x002C9DF0 File Offset: 0x002C7FF0
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetDesc()
		{
			return Food.Instance[this.TemplateId].Desc;
		}

		// Token: 0x060050FF RID: 20735 RVA: 0x002C9E18 File Offset: 0x002C8018
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetTransferable()
		{
			return Food.Instance[this.TemplateId].Transferable;
		}

		// Token: 0x06005100 RID: 20736 RVA: 0x002C9E40 File Offset: 0x002C8040
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetStackable()
		{
			return Food.Instance[this.TemplateId].Stackable;
		}

		// Token: 0x06005101 RID: 20737 RVA: 0x002C9E68 File Offset: 0x002C8068
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetWagerable()
		{
			return Food.Instance[this.TemplateId].Wagerable;
		}

		// Token: 0x06005102 RID: 20738 RVA: 0x002C9E90 File Offset: 0x002C8090
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRefinable()
		{
			return Food.Instance[this.TemplateId].Refinable;
		}

		// Token: 0x06005103 RID: 20739 RVA: 0x002C9EB8 File Offset: 0x002C80B8
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetPoisonable()
		{
			return Food.Instance[this.TemplateId].Poisonable;
		}

		// Token: 0x06005104 RID: 20740 RVA: 0x002C9EE0 File Offset: 0x002C80E0
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRepairable()
		{
			return Food.Instance[this.TemplateId].Repairable;
		}

		// Token: 0x06005105 RID: 20741 RVA: 0x002C9F08 File Offset: 0x002C8108
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseWeight()
		{
			return Food.Instance[this.TemplateId].BaseWeight;
		}

		// Token: 0x06005106 RID: 20742 RVA: 0x002C9F30 File Offset: 0x002C8130
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseValue()
		{
			return Food.Instance[this.TemplateId].BaseValue;
		}

		// Token: 0x06005107 RID: 20743 RVA: 0x002C9F58 File Offset: 0x002C8158
		[CollectionObjectField(true, false, false, false, false)]
		public int GetBasePrice()
		{
			return Food.Instance[this.TemplateId].BasePrice;
		}

		// Token: 0x06005108 RID: 20744 RVA: 0x002C9F80 File Offset: 0x002C8180
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetDropRate()
		{
			return Food.Instance[this.TemplateId].DropRate;
		}

		// Token: 0x06005109 RID: 20745 RVA: 0x002C9FA8 File Offset: 0x002C81A8
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetResourceType()
		{
			return Food.Instance[this.TemplateId].ResourceType;
		}

		// Token: 0x0600510A RID: 20746 RVA: 0x002C9FD0 File Offset: 0x002C81D0
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetPreservationDuration()
		{
			return Food.Instance[this.TemplateId].PreservationDuration;
		}

		// Token: 0x0600510B RID: 20747 RVA: 0x002C9FF8 File Offset: 0x002C81F8
		[CollectionObjectField(true, false, false, false, false)]
		public short GetDuration()
		{
			return Food.Instance[this.TemplateId].Duration;
		}

		// Token: 0x0600510C RID: 20748 RVA: 0x002CA020 File Offset: 0x002C8220
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetConsumedFeatureMedals()
		{
			return Food.Instance[this.TemplateId].ConsumedFeatureMedals;
		}

		// Token: 0x0600510D RID: 20749 RVA: 0x002CA048 File Offset: 0x002C8248
		[CollectionObjectField(true, false, false, false, false)]
		public MainAttributes GetMainAttributesRegen()
		{
			return Food.Instance[this.TemplateId].MainAttributesRegen;
		}

		// Token: 0x0600510E RID: 20750 RVA: 0x002CA070 File Offset: 0x002C8270
		[CollectionObjectField(true, false, false, false, true)]
		public short GetStrength()
		{
			return Food.Instance[this.TemplateId].Strength;
		}

		// Token: 0x0600510F RID: 20751 RVA: 0x002CA098 File Offset: 0x002C8298
		[CollectionObjectField(true, false, false, false, true)]
		public short GetDexterity()
		{
			return Food.Instance[this.TemplateId].Dexterity;
		}

		// Token: 0x06005110 RID: 20752 RVA: 0x002CA0C0 File Offset: 0x002C82C0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetConcentration()
		{
			return Food.Instance[this.TemplateId].Concentration;
		}

		// Token: 0x06005111 RID: 20753 RVA: 0x002CA0E8 File Offset: 0x002C82E8
		[CollectionObjectField(true, false, false, false, true)]
		public short GetVitality()
		{
			return Food.Instance[this.TemplateId].Vitality;
		}

		// Token: 0x06005112 RID: 20754 RVA: 0x002CA110 File Offset: 0x002C8310
		[CollectionObjectField(true, false, false, false, true)]
		public short GetEnergy()
		{
			return Food.Instance[this.TemplateId].Energy;
		}

		// Token: 0x06005113 RID: 20755 RVA: 0x002CA138 File Offset: 0x002C8338
		[CollectionObjectField(true, false, false, false, true)]
		public short GetIntelligence()
		{
			return Food.Instance[this.TemplateId].Intelligence;
		}

		// Token: 0x06005114 RID: 20756 RVA: 0x002CA160 File Offset: 0x002C8360
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateStrength()
		{
			return Food.Instance[this.TemplateId].HitRateStrength;
		}

		// Token: 0x06005115 RID: 20757 RVA: 0x002CA188 File Offset: 0x002C8388
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateTechnique()
		{
			return Food.Instance[this.TemplateId].HitRateTechnique;
		}

		// Token: 0x06005116 RID: 20758 RVA: 0x002CA1B0 File Offset: 0x002C83B0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateSpeed()
		{
			return Food.Instance[this.TemplateId].HitRateSpeed;
		}

		// Token: 0x06005117 RID: 20759 RVA: 0x002CA1D8 File Offset: 0x002C83D8
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateMind()
		{
			return Food.Instance[this.TemplateId].HitRateMind;
		}

		// Token: 0x06005118 RID: 20760 RVA: 0x002CA200 File Offset: 0x002C8400
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateOfOuter()
		{
			return Food.Instance[this.TemplateId].PenetrateOfOuter;
		}

		// Token: 0x06005119 RID: 20761 RVA: 0x002CA228 File Offset: 0x002C8428
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateOfInner()
		{
			return Food.Instance[this.TemplateId].PenetrateOfInner;
		}

		// Token: 0x0600511A RID: 20762 RVA: 0x002CA250 File Offset: 0x002C8450
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateStrength()
		{
			return Food.Instance[this.TemplateId].AvoidRateStrength;
		}

		// Token: 0x0600511B RID: 20763 RVA: 0x002CA278 File Offset: 0x002C8478
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateTechnique()
		{
			return Food.Instance[this.TemplateId].AvoidRateTechnique;
		}

		// Token: 0x0600511C RID: 20764 RVA: 0x002CA2A0 File Offset: 0x002C84A0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateSpeed()
		{
			return Food.Instance[this.TemplateId].AvoidRateSpeed;
		}

		// Token: 0x0600511D RID: 20765 RVA: 0x002CA2C8 File Offset: 0x002C84C8
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateMind()
		{
			return Food.Instance[this.TemplateId].AvoidRateMind;
		}

		// Token: 0x0600511E RID: 20766 RVA: 0x002CA2F0 File Offset: 0x002C84F0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateResistOfOuter()
		{
			return Food.Instance[this.TemplateId].PenetrateResistOfOuter;
		}

		// Token: 0x0600511F RID: 20767 RVA: 0x002CA318 File Offset: 0x002C8518
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateResistOfInner()
		{
			return Food.Instance[this.TemplateId].PenetrateResistOfInner;
		}

		// Token: 0x06005120 RID: 20768 RVA: 0x002CA340 File Offset: 0x002C8540
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfStance()
		{
			return Food.Instance[this.TemplateId].RecoveryOfStance;
		}

		// Token: 0x06005121 RID: 20769 RVA: 0x002CA368 File Offset: 0x002C8568
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfBreath()
		{
			return Food.Instance[this.TemplateId].RecoveryOfBreath;
		}

		// Token: 0x06005122 RID: 20770 RVA: 0x002CA390 File Offset: 0x002C8590
		[CollectionObjectField(true, false, false, false, true)]
		public short GetMoveSpeed()
		{
			return Food.Instance[this.TemplateId].MoveSpeed;
		}

		// Token: 0x06005123 RID: 20771 RVA: 0x002CA3B8 File Offset: 0x002C85B8
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfFlaw()
		{
			return Food.Instance[this.TemplateId].RecoveryOfFlaw;
		}

		// Token: 0x06005124 RID: 20772 RVA: 0x002CA3E0 File Offset: 0x002C85E0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetCastSpeed()
		{
			return Food.Instance[this.TemplateId].CastSpeed;
		}

		// Token: 0x06005125 RID: 20773 RVA: 0x002CA408 File Offset: 0x002C8608
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfBlockedAcupoint()
		{
			return Food.Instance[this.TemplateId].RecoveryOfBlockedAcupoint;
		}

		// Token: 0x06005126 RID: 20774 RVA: 0x002CA430 File Offset: 0x002C8630
		[CollectionObjectField(true, false, false, false, true)]
		public short GetWeaponSwitchSpeed()
		{
			return Food.Instance[this.TemplateId].WeaponSwitchSpeed;
		}

		// Token: 0x06005127 RID: 20775 RVA: 0x002CA458 File Offset: 0x002C8658
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAttackSpeed()
		{
			return Food.Instance[this.TemplateId].AttackSpeed;
		}

		// Token: 0x06005128 RID: 20776 RVA: 0x002CA480 File Offset: 0x002C8680
		[CollectionObjectField(true, false, false, false, true)]
		public short GetInnerRatio()
		{
			return Food.Instance[this.TemplateId].InnerRatio;
		}

		// Token: 0x06005129 RID: 20777 RVA: 0x002CA4A8 File Offset: 0x002C86A8
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfQiDisorder()
		{
			return Food.Instance[this.TemplateId].RecoveryOfQiDisorder;
		}

		// Token: 0x0600512A RID: 20778 RVA: 0x002CA4D0 File Offset: 0x002C86D0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfHotPoison()
		{
			return Food.Instance[this.TemplateId].ResistOfHotPoison;
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x002CA4F8 File Offset: 0x002C86F8
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfGloomyPoison()
		{
			return Food.Instance[this.TemplateId].ResistOfGloomyPoison;
		}

		// Token: 0x0600512C RID: 20780 RVA: 0x002CA520 File Offset: 0x002C8720
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfColdPoison()
		{
			return Food.Instance[this.TemplateId].ResistOfColdPoison;
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x002CA548 File Offset: 0x002C8748
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfRedPoison()
		{
			return Food.Instance[this.TemplateId].ResistOfRedPoison;
		}

		// Token: 0x0600512E RID: 20782 RVA: 0x002CA570 File Offset: 0x002C8770
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfRottenPoison()
		{
			return Food.Instance[this.TemplateId].ResistOfRottenPoison;
		}

		// Token: 0x0600512F RID: 20783 RVA: 0x002CA598 File Offset: 0x002C8798
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfIllusoryPoison()
		{
			return Food.Instance[this.TemplateId].ResistOfIllusoryPoison;
		}

		// Token: 0x06005130 RID: 20784 RVA: 0x002CA5C0 File Offset: 0x002C87C0
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseFavorabilityChange()
		{
			return Food.Instance[this.TemplateId].BaseFavorabilityChange;
		}

		// Token: 0x06005131 RID: 20785 RVA: 0x002CA5E8 File Offset: 0x002C87E8
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetBaseHappinessChange()
		{
			return Food.Instance[this.TemplateId].BaseHappinessChange;
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x002CA610 File Offset: 0x002C8810
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetGiftLevel()
		{
			return Food.Instance[this.TemplateId].GiftLevel;
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x002CA638 File Offset: 0x002C8838
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetInheritable()
		{
			return Food.Instance[this.TemplateId].Inheritable;
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x002CA660 File Offset: 0x002C8860
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetIsSpecial()
		{
			return Food.Instance[this.TemplateId].IsSpecial;
		}

		// Token: 0x06005135 RID: 20789 RVA: 0x002CA688 File Offset: 0x002C8888
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetMerchantLevel()
		{
			return Food.Instance[this.TemplateId].MerchantLevel;
		}

		// Token: 0x06005136 RID: 20790 RVA: 0x002CA6B0 File Offset: 0x002C88B0
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowRandomCreate()
		{
			return Food.Instance[this.TemplateId].AllowRandomCreate;
		}

		// Token: 0x06005137 RID: 20791 RVA: 0x002CA6D8 File Offset: 0x002C88D8
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetBreakBonusEffect()
		{
			return Food.Instance[this.TemplateId].BreakBonusEffect;
		}

		// Token: 0x06005138 RID: 20792 RVA: 0x002CA700 File Offset: 0x002C8900
		[CollectionObjectField(true, false, false, false, false)]
		public short GetGroupId()
		{
			return Food.Instance[this.TemplateId].GroupId;
		}

		// Token: 0x06005139 RID: 20793 RVA: 0x002CA728 File Offset: 0x002C8928
		[CollectionObjectField(true, false, false, false, false)]
		public List<EFoodFoodType> GetFoodType()
		{
			return Food.Instance[this.TemplateId].FoodType;
		}

		// Token: 0x0600513A RID: 20794 RVA: 0x002CA750 File Offset: 0x002C8950
		[CollectionObjectField(true, false, false, false, false)]
		public string GetBigIcon()
		{
			return Food.Instance[this.TemplateId].BigIcon;
		}

		// Token: 0x0600513B RID: 20795 RVA: 0x002CA777 File Offset: 0x002C8977
		public Food()
		{
		}

		// Token: 0x0600513C RID: 20796 RVA: 0x002CA784 File Offset: 0x002C8984
		public Food(short templateId)
		{
			FoodItem template = Food.Instance[templateId];
			this.TemplateId = template.TemplateId;
			this.MaxDurability = template.MaxDurability;
		}

		// Token: 0x0600513D RID: 20797 RVA: 0x002CA7C0 File Offset: 0x002C89C0
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x0600513E RID: 20798 RVA: 0x002CA7D4 File Offset: 0x002C89D4
		public int GetSerializedSize()
		{
			return 11;
		}

		// Token: 0x0600513F RID: 20799 RVA: 0x002CA7EC File Offset: 0x002C89EC
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = this.Id;
			byte* pCurrData = pData + 4;
			*(short*)pCurrData = this.TemplateId;
			pCurrData += 2;
			*(short*)pCurrData = this.MaxDurability;
			pCurrData += 2;
			*(short*)pCurrData = this.CurrDurability;
			pCurrData += 2;
			*pCurrData = this.ModificationState;
			pCurrData++;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06005140 RID: 20800 RVA: 0x002CA844 File Offset: 0x002C8A44
		public unsafe int Deserialize(byte* pData)
		{
			this.Id = *(int*)pData;
			byte* pCurrData = pData + 4;
			this.TemplateId = *(short*)pCurrData;
			pCurrData += 2;
			this.MaxDurability = *(short*)pCurrData;
			pCurrData += 2;
			this.CurrDurability = *(short*)pCurrData;
			pCurrData += 2;
			this.ModificationState = *pCurrData;
			pCurrData++;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06005141 RID: 20801 RVA: 0x002CA89B File Offset: 0x002C8A9B
		public Food(IRandomSource random, short templateId, int itemId) : this(templateId)
		{
			this.Id = itemId;
			this.MaxDurability = ItemBase.GenerateMaxDurability(random, this.MaxDurability);
			this.CurrDurability = this.MaxDurability;
		}

		// Token: 0x06005142 RID: 20802 RVA: 0x002CA8CC File Offset: 0x002C8ACC
		public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
		{
			return Food.GetCharacterPropertyBonus(this.TemplateId, type);
		}

		// Token: 0x06005143 RID: 20803 RVA: 0x002CA8EC File Offset: 0x002C8AEC
		public static int GetCharacterPropertyBonus(short templateId, ECharacterPropertyReferencedType type)
		{
			FoodItem config = Food.Instance[templateId];
			return config.GetCharacterPropertyBonusInt(type);
		}

		// Token: 0x040015FD RID: 5629
		public const int FixedSize = 11;

		// Token: 0x040015FE RID: 5630
		public const int DynamicCount = 0;

		// Token: 0x02000ABE RID: 2750
		internal class FixedFieldInfos
		{
			// Token: 0x04002C77 RID: 11383
			public const uint Id_Offset = 0U;

			// Token: 0x04002C78 RID: 11384
			public const int Id_Size = 4;

			// Token: 0x04002C79 RID: 11385
			public const uint TemplateId_Offset = 4U;

			// Token: 0x04002C7A RID: 11386
			public const int TemplateId_Size = 2;

			// Token: 0x04002C7B RID: 11387
			public const uint MaxDurability_Offset = 6U;

			// Token: 0x04002C7C RID: 11388
			public const int MaxDurability_Size = 2;

			// Token: 0x04002C7D RID: 11389
			public const uint CurrDurability_Offset = 8U;

			// Token: 0x04002C7E RID: 11390
			public const int CurrDurability_Size = 2;

			// Token: 0x04002C7F RID: 11391
			public const uint ModificationState_Offset = 10U;

			// Token: 0x04002C80 RID: 11392
			public const int ModificationState_Size = 1;
		}
	}
}
