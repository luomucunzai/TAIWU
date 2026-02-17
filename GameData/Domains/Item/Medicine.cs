using System;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Serializer;
using Redzen.Random;

namespace GameData.Domains.Item
{
	// Token: 0x0200066F RID: 1647
	[SerializableGameData(NotForDisplayModule = true)]
	public class Medicine : ItemBase, ISerializableGameData
	{
		// Token: 0x060051AB RID: 20907 RVA: 0x002CBD0C File Offset: 0x002C9F0C
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

		// Token: 0x060051AC RID: 20908 RVA: 0x002CBD6C File Offset: 0x002C9F6C
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

		// Token: 0x060051AD RID: 20909 RVA: 0x002CBDCC File Offset: 0x002C9FCC
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

		// Token: 0x060051AE RID: 20910 RVA: 0x002CBE2C File Offset: 0x002CA02C
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetName()
		{
			return Medicine.Instance[this.TemplateId].Name;
		}

		// Token: 0x060051AF RID: 20911 RVA: 0x002CBE54 File Offset: 0x002CA054
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetItemType()
		{
			return Medicine.Instance[this.TemplateId].ItemType;
		}

		// Token: 0x060051B0 RID: 20912 RVA: 0x002CBE7C File Offset: 0x002CA07C
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetItemSubType()
		{
			return Medicine.Instance[this.TemplateId].ItemSubType;
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x002CBEA4 File Offset: 0x002CA0A4
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetGrade()
		{
			return Medicine.Instance[this.TemplateId].Grade;
		}

		// Token: 0x060051B2 RID: 20914 RVA: 0x002CBECC File Offset: 0x002CA0CC
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetIcon()
		{
			return Medicine.Instance[this.TemplateId].Icon;
		}

		// Token: 0x060051B3 RID: 20915 RVA: 0x002CBEF4 File Offset: 0x002CA0F4
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetDesc()
		{
			return Medicine.Instance[this.TemplateId].Desc;
		}

		// Token: 0x060051B4 RID: 20916 RVA: 0x002CBF1C File Offset: 0x002CA11C
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetTransferable()
		{
			return Medicine.Instance[this.TemplateId].Transferable;
		}

		// Token: 0x060051B5 RID: 20917 RVA: 0x002CBF44 File Offset: 0x002CA144
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetStackable()
		{
			return Medicine.Instance[this.TemplateId].Stackable;
		}

		// Token: 0x060051B6 RID: 20918 RVA: 0x002CBF6C File Offset: 0x002CA16C
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetWagerable()
		{
			return Medicine.Instance[this.TemplateId].Wagerable;
		}

		// Token: 0x060051B7 RID: 20919 RVA: 0x002CBF94 File Offset: 0x002CA194
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRefinable()
		{
			return Medicine.Instance[this.TemplateId].Refinable;
		}

		// Token: 0x060051B8 RID: 20920 RVA: 0x002CBFBC File Offset: 0x002CA1BC
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetPoisonable()
		{
			return Medicine.Instance[this.TemplateId].Poisonable;
		}

		// Token: 0x060051B9 RID: 20921 RVA: 0x002CBFE4 File Offset: 0x002CA1E4
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRepairable()
		{
			return Medicine.Instance[this.TemplateId].Repairable;
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x002CC00C File Offset: 0x002CA20C
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseWeight()
		{
			return Medicine.Instance[this.TemplateId].BaseWeight;
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x002CC034 File Offset: 0x002CA234
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseValue()
		{
			return Medicine.Instance[this.TemplateId].BaseValue;
		}

		// Token: 0x060051BC RID: 20924 RVA: 0x002CC05C File Offset: 0x002CA25C
		[CollectionObjectField(true, false, false, false, false)]
		public int GetBasePrice()
		{
			return Medicine.Instance[this.TemplateId].BasePrice;
		}

		// Token: 0x060051BD RID: 20925 RVA: 0x002CC084 File Offset: 0x002CA284
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetDropRate()
		{
			return Medicine.Instance[this.TemplateId].DropRate;
		}

		// Token: 0x060051BE RID: 20926 RVA: 0x002CC0AC File Offset: 0x002CA2AC
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetResourceType()
		{
			return Medicine.Instance[this.TemplateId].ResourceType;
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x002CC0D4 File Offset: 0x002CA2D4
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetPreservationDuration()
		{
			return Medicine.Instance[this.TemplateId].PreservationDuration;
		}

		// Token: 0x060051C0 RID: 20928 RVA: 0x002CC0FC File Offset: 0x002CA2FC
		[CollectionObjectField(true, false, false, false, false)]
		public short GetDuration()
		{
			return Medicine.Instance[this.TemplateId].Duration;
		}

		// Token: 0x060051C1 RID: 20929 RVA: 0x002CC124 File Offset: 0x002CA324
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetInjuryRecoveryTimes()
		{
			return Medicine.Instance[this.TemplateId].InjuryRecoveryTimes;
		}

		// Token: 0x060051C2 RID: 20930 RVA: 0x002CC14C File Offset: 0x002CA34C
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateStrength()
		{
			return Medicine.Instance[this.TemplateId].HitRateStrength;
		}

		// Token: 0x060051C3 RID: 20931 RVA: 0x002CC174 File Offset: 0x002CA374
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateTechnique()
		{
			return Medicine.Instance[this.TemplateId].HitRateTechnique;
		}

		// Token: 0x060051C4 RID: 20932 RVA: 0x002CC19C File Offset: 0x002CA39C
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateSpeed()
		{
			return Medicine.Instance[this.TemplateId].HitRateSpeed;
		}

		// Token: 0x060051C5 RID: 20933 RVA: 0x002CC1C4 File Offset: 0x002CA3C4
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateMind()
		{
			return Medicine.Instance[this.TemplateId].HitRateMind;
		}

		// Token: 0x060051C6 RID: 20934 RVA: 0x002CC1EC File Offset: 0x002CA3EC
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateOfOuter()
		{
			return Medicine.Instance[this.TemplateId].PenetrateOfOuter;
		}

		// Token: 0x060051C7 RID: 20935 RVA: 0x002CC214 File Offset: 0x002CA414
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateOfInner()
		{
			return Medicine.Instance[this.TemplateId].PenetrateOfInner;
		}

		// Token: 0x060051C8 RID: 20936 RVA: 0x002CC23C File Offset: 0x002CA43C
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateStrength()
		{
			return Medicine.Instance[this.TemplateId].AvoidRateStrength;
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x002CC264 File Offset: 0x002CA464
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateTechnique()
		{
			return Medicine.Instance[this.TemplateId].AvoidRateTechnique;
		}

		// Token: 0x060051CA RID: 20938 RVA: 0x002CC28C File Offset: 0x002CA48C
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateSpeed()
		{
			return Medicine.Instance[this.TemplateId].AvoidRateSpeed;
		}

		// Token: 0x060051CB RID: 20939 RVA: 0x002CC2B4 File Offset: 0x002CA4B4
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateMind()
		{
			return Medicine.Instance[this.TemplateId].AvoidRateMind;
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x002CC2DC File Offset: 0x002CA4DC
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateResistOfOuter()
		{
			return Medicine.Instance[this.TemplateId].PenetrateResistOfOuter;
		}

		// Token: 0x060051CD RID: 20941 RVA: 0x002CC304 File Offset: 0x002CA504
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateResistOfInner()
		{
			return Medicine.Instance[this.TemplateId].PenetrateResistOfInner;
		}

		// Token: 0x060051CE RID: 20942 RVA: 0x002CC32C File Offset: 0x002CA52C
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfStance()
		{
			return Medicine.Instance[this.TemplateId].RecoveryOfStance;
		}

		// Token: 0x060051CF RID: 20943 RVA: 0x002CC354 File Offset: 0x002CA554
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfBreath()
		{
			return Medicine.Instance[this.TemplateId].RecoveryOfBreath;
		}

		// Token: 0x060051D0 RID: 20944 RVA: 0x002CC37C File Offset: 0x002CA57C
		[CollectionObjectField(true, false, false, false, true)]
		public short GetMoveSpeed()
		{
			return Medicine.Instance[this.TemplateId].MoveSpeed;
		}

		// Token: 0x060051D1 RID: 20945 RVA: 0x002CC3A4 File Offset: 0x002CA5A4
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfFlaw()
		{
			return Medicine.Instance[this.TemplateId].RecoveryOfFlaw;
		}

		// Token: 0x060051D2 RID: 20946 RVA: 0x002CC3CC File Offset: 0x002CA5CC
		[CollectionObjectField(true, false, false, false, true)]
		public short GetCastSpeed()
		{
			return Medicine.Instance[this.TemplateId].CastSpeed;
		}

		// Token: 0x060051D3 RID: 20947 RVA: 0x002CC3F4 File Offset: 0x002CA5F4
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfBlockedAcupoint()
		{
			return Medicine.Instance[this.TemplateId].RecoveryOfBlockedAcupoint;
		}

		// Token: 0x060051D4 RID: 20948 RVA: 0x002CC41C File Offset: 0x002CA61C
		[CollectionObjectField(true, false, false, false, true)]
		public short GetWeaponSwitchSpeed()
		{
			return Medicine.Instance[this.TemplateId].WeaponSwitchSpeed;
		}

		// Token: 0x060051D5 RID: 20949 RVA: 0x002CC444 File Offset: 0x002CA644
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAttackSpeed()
		{
			return Medicine.Instance[this.TemplateId].AttackSpeed;
		}

		// Token: 0x060051D6 RID: 20950 RVA: 0x002CC46C File Offset: 0x002CA66C
		[CollectionObjectField(true, false, false, false, true)]
		public short GetInnerRatio()
		{
			return Medicine.Instance[this.TemplateId].InnerRatio;
		}

		// Token: 0x060051D7 RID: 20951 RVA: 0x002CC494 File Offset: 0x002CA694
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfQiDisorder()
		{
			return Medicine.Instance[this.TemplateId].RecoveryOfQiDisorder;
		}

		// Token: 0x060051D8 RID: 20952 RVA: 0x002CC4BC File Offset: 0x002CA6BC
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetWugType()
		{
			return Medicine.Instance[this.TemplateId].WugType;
		}

		// Token: 0x060051D9 RID: 20953 RVA: 0x002CC4E4 File Offset: 0x002CA6E4
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetWugGrowthType()
		{
			return Medicine.Instance[this.TemplateId].WugGrowthType;
		}

		// Token: 0x060051DA RID: 20954 RVA: 0x002CC50C File Offset: 0x002CA70C
		[CollectionObjectField(true, false, false, false, false)]
		public string GetSpecialEffectClass()
		{
			return Medicine.Instance[this.TemplateId].SpecialEffectClass;
		}

		// Token: 0x060051DB RID: 20955 RVA: 0x002CC534 File Offset: 0x002CA734
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetConsumedFeatureMedals()
		{
			return Medicine.Instance[this.TemplateId].ConsumedFeatureMedals;
		}

		// Token: 0x060051DC RID: 20956 RVA: 0x002CC55C File Offset: 0x002CA75C
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetMaxUseDistance()
		{
			return Medicine.Instance[this.TemplateId].MaxUseDistance;
		}

		// Token: 0x060051DD RID: 20957 RVA: 0x002CC584 File Offset: 0x002CA784
		[CollectionObjectField(true, false, false, false, false)]
		public string GetSpecialEffectDesc()
		{
			return Medicine.Instance[this.TemplateId].SpecialEffectDesc;
		}

		// Token: 0x060051DE RID: 20958 RVA: 0x002CC5AC File Offset: 0x002CA7AC
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetBuffAndOtherMedicine()
		{
			return Medicine.Instance[this.TemplateId].BuffAndOtherMedicine;
		}

		// Token: 0x060051DF RID: 20959 RVA: 0x002CC5D4 File Offset: 0x002CA7D4
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetBaseHappinessChange()
		{
			return Medicine.Instance[this.TemplateId].BaseHappinessChange;
		}

		// Token: 0x060051E0 RID: 20960 RVA: 0x002CC5FC File Offset: 0x002CA7FC
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetGiftLevel()
		{
			return Medicine.Instance[this.TemplateId].GiftLevel;
		}

		// Token: 0x060051E1 RID: 20961 RVA: 0x002CC624 File Offset: 0x002CA824
		[CollectionObjectField(true, false, false, false, false)]
		public short GetSpecialEffectId()
		{
			return Medicine.Instance[this.TemplateId].SpecialEffectId;
		}

		// Token: 0x060051E2 RID: 20962 RVA: 0x002CC64C File Offset: 0x002CA84C
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseFavorabilityChange()
		{
			return Medicine.Instance[this.TemplateId].BaseFavorabilityChange;
		}

		// Token: 0x060051E3 RID: 20963 RVA: 0x002CC674 File Offset: 0x002CA874
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfRedPoison()
		{
			return Medicine.Instance[this.TemplateId].ResistOfRedPoison;
		}

		// Token: 0x060051E4 RID: 20964 RVA: 0x002CC69C File Offset: 0x002CA89C
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfIllusoryPoison()
		{
			return Medicine.Instance[this.TemplateId].ResistOfIllusoryPoison;
		}

		// Token: 0x060051E5 RID: 20965 RVA: 0x002CC6C4 File Offset: 0x002CA8C4
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfRottenPoison()
		{
			return Medicine.Instance[this.TemplateId].ResistOfRottenPoison;
		}

		// Token: 0x060051E6 RID: 20966 RVA: 0x002CC6EC File Offset: 0x002CA8EC
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfColdPoison()
		{
			return Medicine.Instance[this.TemplateId].ResistOfColdPoison;
		}

		// Token: 0x060051E7 RID: 20967 RVA: 0x002CC714 File Offset: 0x002CA914
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfGloomyPoison()
		{
			return Medicine.Instance[this.TemplateId].ResistOfGloomyPoison;
		}

		// Token: 0x060051E8 RID: 20968 RVA: 0x002CC73C File Offset: 0x002CA93C
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetCanUseMultiple()
		{
			return Medicine.Instance[this.TemplateId].CanUseMultiple;
		}

		// Token: 0x060051E9 RID: 20969 RVA: 0x002CC764 File Offset: 0x002CA964
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetBreakBonusEffect()
		{
			return Medicine.Instance[this.TemplateId].BreakBonusEffect;
		}

		// Token: 0x060051EA RID: 20970 RVA: 0x002CC78C File Offset: 0x002CA98C
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetInheritable()
		{
			return Medicine.Instance[this.TemplateId].Inheritable;
		}

		// Token: 0x060051EB RID: 20971 RVA: 0x002CC7B4 File Offset: 0x002CA9B4
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetMerchantLevel()
		{
			return Medicine.Instance[this.TemplateId].MerchantLevel;
		}

		// Token: 0x060051EC RID: 20972 RVA: 0x002CC7DC File Offset: 0x002CA9DC
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowRandomCreate()
		{
			return Medicine.Instance[this.TemplateId].AllowRandomCreate;
		}

		// Token: 0x060051ED RID: 20973 RVA: 0x002CC804 File Offset: 0x002CAA04
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetIsSpecial()
		{
			return Medicine.Instance[this.TemplateId].IsSpecial;
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x002CC82C File Offset: 0x002CAA2C
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfHotPoison()
		{
			return Medicine.Instance[this.TemplateId].ResistOfHotPoison;
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x002CC854 File Offset: 0x002CAA54
		[CollectionObjectField(true, false, false, false, false)]
		public short GetGroupId()
		{
			return Medicine.Instance[this.TemplateId].GroupId;
		}

		// Token: 0x060051F0 RID: 20976 RVA: 0x002CC87C File Offset: 0x002CAA7C
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetDamageStepBonus()
		{
			return Medicine.Instance[this.TemplateId].DamageStepBonus;
		}

		// Token: 0x060051F1 RID: 20977 RVA: 0x002CC8A4 File Offset: 0x002CAAA4
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetRequiredMainAttributeValue()
		{
			return Medicine.Instance[this.TemplateId].RequiredMainAttributeValue;
		}

		// Token: 0x060051F2 RID: 20978 RVA: 0x002CC8CC File Offset: 0x002CAACC
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetRequiredMainAttributeType()
		{
			return Medicine.Instance[this.TemplateId].RequiredMainAttributeType;
		}

		// Token: 0x060051F3 RID: 20979 RVA: 0x002CC8F4 File Offset: 0x002CAAF4
		[CollectionObjectField(true, false, false, false, false)]
		public short GetSideEffectValue()
		{
			return Medicine.Instance[this.TemplateId].SideEffectValue;
		}

		// Token: 0x060051F4 RID: 20980 RVA: 0x002CC91C File Offset: 0x002CAB1C
		[CollectionObjectField(true, false, false, false, false)]
		public short GetEffectValue()
		{
			return Medicine.Instance[this.TemplateId].EffectValue;
		}

		// Token: 0x060051F5 RID: 20981 RVA: 0x002CC944 File Offset: 0x002CAB44
		[CollectionObjectField(true, false, false, false, false)]
		public short GetEffectThresholdValue()
		{
			return Medicine.Instance[this.TemplateId].EffectThresholdValue;
		}

		// Token: 0x060051F6 RID: 20982 RVA: 0x002CC96C File Offset: 0x002CAB6C
		[CollectionObjectField(true, false, false, false, false)]
		public EMedicineEffectSubType GetEffectSubType()
		{
			return Medicine.Instance[this.TemplateId].EffectSubType;
		}

		// Token: 0x060051F7 RID: 20983 RVA: 0x002CC994 File Offset: 0x002CAB94
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetHasNormalEatingEffect()
		{
			return Medicine.Instance[this.TemplateId].HasNormalEatingEffect;
		}

		// Token: 0x060051F8 RID: 20984 RVA: 0x002CC9BC File Offset: 0x002CABBC
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetInstantAffect()
		{
			return Medicine.Instance[this.TemplateId].InstantAffect;
		}

		// Token: 0x060051F9 RID: 20985 RVA: 0x002CC9E4 File Offset: 0x002CABE4
		[CollectionObjectField(true, false, false, false, false)]
		public short GetCombatUseEffect()
		{
			return Medicine.Instance[this.TemplateId].CombatUseEffect;
		}

		// Token: 0x060051FA RID: 20986 RVA: 0x002CCA0C File Offset: 0x002CAC0C
		[CollectionObjectField(true, false, false, false, false)]
		public short GetCombatPrepareUseEffect()
		{
			return Medicine.Instance[this.TemplateId].CombatPrepareUseEffect;
		}

		// Token: 0x060051FB RID: 20987 RVA: 0x002CCA34 File Offset: 0x002CAC34
		[CollectionObjectField(true, false, false, false, false)]
		public EMedicineEffectType GetEffectType()
		{
			return Medicine.Instance[this.TemplateId].EffectType;
		}

		// Token: 0x060051FC RID: 20988 RVA: 0x002CCA5B File Offset: 0x002CAC5B
		public Medicine()
		{
		}

		// Token: 0x060051FD RID: 20989 RVA: 0x002CCA68 File Offset: 0x002CAC68
		public Medicine(short templateId)
		{
			MedicineItem template = Medicine.Instance[templateId];
			this.TemplateId = template.TemplateId;
			this.MaxDurability = template.MaxDurability;
		}

		// Token: 0x060051FE RID: 20990 RVA: 0x002CCAA4 File Offset: 0x002CACA4
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x060051FF RID: 20991 RVA: 0x002CCAB8 File Offset: 0x002CACB8
		public int GetSerializedSize()
		{
			return 11;
		}

		// Token: 0x06005200 RID: 20992 RVA: 0x002CCAD0 File Offset: 0x002CACD0
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

		// Token: 0x06005201 RID: 20993 RVA: 0x002CCB28 File Offset: 0x002CAD28
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

		// Token: 0x06005202 RID: 20994 RVA: 0x002CCB7F File Offset: 0x002CAD7F
		public Medicine(IRandomSource random, short templateId, int itemId) : this(templateId)
		{
			this.Id = itemId;
			this.MaxDurability = ItemBase.GenerateMaxDurability(random, this.MaxDurability);
			this.CurrDurability = this.MaxDurability;
		}

		// Token: 0x06005203 RID: 20995 RVA: 0x002CCBAF File Offset: 0x002CADAF
		public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
		{
			return Medicine.GetCharacterPropertyBonusValue(this.TemplateId, type);
		}

		// Token: 0x06005204 RID: 20996 RVA: 0x002CCBBD File Offset: 0x002CADBD
		public static int GetCharacterPropertyBonusValue(short templateId, ECharacterPropertyReferencedType type)
		{
			return Medicine.Instance[templateId].GetCharacterPropertyBonusValue(type);
		}

		// Token: 0x06005205 RID: 20997 RVA: 0x002CCBD0 File Offset: 0x002CADD0
		public static int GetCharacterPropertyBonusPercentage(short templateId, ECharacterPropertyReferencedType type)
		{
			return Medicine.Instance[templateId].GetCharacterPropertyBonusPercentage(type);
		}

		// Token: 0x06005206 RID: 20998 RVA: 0x002CCBE4 File Offset: 0x002CADE4
		public static short GetDeltaWugDuration(sbyte medicineGrade)
		{
			return (short)(-(medicineGrade + 1) * 12);
		}

		// Token: 0x0400160C RID: 5644
		public const int FixedSize = 11;

		// Token: 0x0400160D RID: 5645
		public const int DynamicCount = 0;

		// Token: 0x02000AC2 RID: 2754
		internal class FixedFieldInfos
		{
			// Token: 0x04002C98 RID: 11416
			public const uint Id_Offset = 0U;

			// Token: 0x04002C99 RID: 11417
			public const int Id_Size = 4;

			// Token: 0x04002C9A RID: 11418
			public const uint TemplateId_Offset = 4U;

			// Token: 0x04002C9B RID: 11419
			public const int TemplateId_Size = 2;

			// Token: 0x04002C9C RID: 11420
			public const uint MaxDurability_Offset = 6U;

			// Token: 0x04002C9D RID: 11421
			public const int MaxDurability_Size = 2;

			// Token: 0x04002C9E RID: 11422
			public const uint CurrDurability_Offset = 8U;

			// Token: 0x04002C9F RID: 11423
			public const int CurrDurability_Size = 2;

			// Token: 0x04002CA0 RID: 11424
			public const uint ModificationState_Offset = 10U;

			// Token: 0x04002CA1 RID: 11425
			public const int ModificationState_Size = 1;
		}
	}
}
