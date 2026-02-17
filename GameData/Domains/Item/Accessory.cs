using System;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Domains.Map;
using GameData.Domains.Taiwu.Profession;
using GameData.Serializer;
using Redzen.Random;

namespace GameData.Domains.Item
{
	// Token: 0x02000659 RID: 1625
	[SerializableGameData(NotForDisplayModule = true)]
	public class Accessory : EquipmentBase, ISerializableGameData, IExploreBonusRateItem
	{
		// Token: 0x06004D9D RID: 19869 RVA: 0x002ADD84 File Offset: 0x002ABF84
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

		// Token: 0x06004D9E RID: 19870 RVA: 0x002ADDE4 File Offset: 0x002ABFE4
		public unsafe override void SetEquipmentEffectId(short equipmentEffectId, DataContext context)
		{
			this.EquipmentEffectId = equipmentEffectId;
			base.SetModifiedAndInvalidateInfluencedCache(3, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 8U, 2);
				*(short*)pData = this.EquipmentEffectId;
				pData += 2;
			}
		}

		// Token: 0x06004D9F RID: 19871 RVA: 0x002ADE44 File Offset: 0x002AC044
		public unsafe override void SetCurrDurability(short currDurability, DataContext context)
		{
			this.CurrDurability = currDurability;
			base.SetModifiedAndInvalidateInfluencedCache(4, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 10U, 2);
				*(short*)pData = this.CurrDurability;
				pData += 2;
			}
		}

		// Token: 0x06004DA0 RID: 19872 RVA: 0x002ADEA4 File Offset: 0x002AC0A4
		public unsafe override void SetModificationState(byte modificationState, DataContext context)
		{
			this.ModificationState = modificationState;
			base.SetModifiedAndInvalidateInfluencedCache(5, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 12U, 1);
				*pData = this.ModificationState;
				pData++;
			}
		}

		// Token: 0x06004DA1 RID: 19873 RVA: 0x002ADF04 File Offset: 0x002AC104
		public unsafe override void SetEquippedCharId(int equippedCharId, DataContext context)
		{
			this.EquippedCharId = equippedCharId;
			base.SetModifiedAndInvalidateInfluencedCache(6, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 13U, 4);
				*(int*)pData = this.EquippedCharId;
				pData += 4;
			}
		}

		// Token: 0x06004DA2 RID: 19874 RVA: 0x002ADF64 File Offset: 0x002AC164
		public unsafe override void SetMaterialResources(MaterialResources materialResources, DataContext context)
		{
			this.MaterialResources = materialResources;
			base.SetModifiedAndInvalidateInfluencedCache(7, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 17U, 12);
				pData += this.MaterialResources.Serialize(pData);
			}
		}

		// Token: 0x06004DA3 RID: 19875 RVA: 0x002ADFC8 File Offset: 0x002AC1C8
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetName()
		{
			return Accessory.Instance[this.TemplateId].Name;
		}

		// Token: 0x06004DA4 RID: 19876 RVA: 0x002ADFF0 File Offset: 0x002AC1F0
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetItemType()
		{
			return Accessory.Instance[this.TemplateId].ItemType;
		}

		// Token: 0x06004DA5 RID: 19877 RVA: 0x002AE018 File Offset: 0x002AC218
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetItemSubType()
		{
			return Accessory.Instance[this.TemplateId].ItemSubType;
		}

		// Token: 0x06004DA6 RID: 19878 RVA: 0x002AE040 File Offset: 0x002AC240
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetGrade()
		{
			return Accessory.Instance[this.TemplateId].Grade;
		}

		// Token: 0x06004DA7 RID: 19879 RVA: 0x002AE068 File Offset: 0x002AC268
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetIcon()
		{
			return Accessory.Instance[this.TemplateId].Icon;
		}

		// Token: 0x06004DA8 RID: 19880 RVA: 0x002AE090 File Offset: 0x002AC290
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetDesc()
		{
			return Accessory.Instance[this.TemplateId].Desc;
		}

		// Token: 0x06004DA9 RID: 19881 RVA: 0x002AE0B8 File Offset: 0x002AC2B8
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetTransferable()
		{
			return Accessory.Instance[this.TemplateId].Transferable;
		}

		// Token: 0x06004DAA RID: 19882 RVA: 0x002AE0E0 File Offset: 0x002AC2E0
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetStackable()
		{
			return Accessory.Instance[this.TemplateId].Stackable;
		}

		// Token: 0x06004DAB RID: 19883 RVA: 0x002AE108 File Offset: 0x002AC308
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetWagerable()
		{
			return Accessory.Instance[this.TemplateId].Wagerable;
		}

		// Token: 0x06004DAC RID: 19884 RVA: 0x002AE130 File Offset: 0x002AC330
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRefinable()
		{
			return Accessory.Instance[this.TemplateId].Refinable;
		}

		// Token: 0x06004DAD RID: 19885 RVA: 0x002AE158 File Offset: 0x002AC358
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetPoisonable()
		{
			return Accessory.Instance[this.TemplateId].Poisonable;
		}

		// Token: 0x06004DAE RID: 19886 RVA: 0x002AE180 File Offset: 0x002AC380
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRepairable()
		{
			return Accessory.Instance[this.TemplateId].Repairable;
		}

		// Token: 0x06004DAF RID: 19887 RVA: 0x002AE1A8 File Offset: 0x002AC3A8
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseWeight()
		{
			return Accessory.Instance[this.TemplateId].BaseWeight;
		}

		// Token: 0x06004DB0 RID: 19888 RVA: 0x002AE1D0 File Offset: 0x002AC3D0
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseValue()
		{
			return Accessory.Instance[this.TemplateId].BaseValue;
		}

		// Token: 0x06004DB1 RID: 19889 RVA: 0x002AE1F8 File Offset: 0x002AC3F8
		[CollectionObjectField(true, false, false, false, false)]
		public int GetBasePrice()
		{
			return Accessory.Instance[this.TemplateId].BasePrice;
		}

		// Token: 0x06004DB2 RID: 19890 RVA: 0x002AE220 File Offset: 0x002AC420
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseFavorabilityChange()
		{
			return Accessory.Instance[this.TemplateId].BaseFavorabilityChange;
		}

		// Token: 0x06004DB3 RID: 19891 RVA: 0x002AE248 File Offset: 0x002AC448
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetDropRate()
		{
			return Accessory.Instance[this.TemplateId].DropRate;
		}

		// Token: 0x06004DB4 RID: 19892 RVA: 0x002AE270 File Offset: 0x002AC470
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetResourceType()
		{
			return Accessory.Instance[this.TemplateId].ResourceType;
		}

		// Token: 0x06004DB5 RID: 19893 RVA: 0x002AE298 File Offset: 0x002AC498
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetPreservationDuration()
		{
			return Accessory.Instance[this.TemplateId].PreservationDuration;
		}

		// Token: 0x06004DB6 RID: 19894 RVA: 0x002AE2C0 File Offset: 0x002AC4C0
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetEquipmentType()
		{
			return Accessory.Instance[this.TemplateId].EquipmentType;
		}

		// Token: 0x06004DB7 RID: 19895 RVA: 0x002AE2E8 File Offset: 0x002AC4E8
		[CollectionObjectField(true, false, false, false, false)]
		public short GetDropRateBonus()
		{
			return Accessory.Instance[this.TemplateId].DropRateBonus;
		}

		// Token: 0x06004DB8 RID: 19896 RVA: 0x002AE310 File Offset: 0x002AC510
		[CollectionObjectField(true, false, false, false, false)]
		public short GetMaxInventoryLoadBonus()
		{
			return Accessory.Instance[this.TemplateId].MaxInventoryLoadBonus;
		}

		// Token: 0x06004DB9 RID: 19897 RVA: 0x002AE338 File Offset: 0x002AC538
		[CollectionObjectField(true, false, false, false, true)]
		public short GetStrength()
		{
			return Accessory.Instance[this.TemplateId].Strength;
		}

		// Token: 0x06004DBA RID: 19898 RVA: 0x002AE360 File Offset: 0x002AC560
		[CollectionObjectField(true, false, false, false, true)]
		public short GetDexterity()
		{
			return Accessory.Instance[this.TemplateId].Dexterity;
		}

		// Token: 0x06004DBB RID: 19899 RVA: 0x002AE388 File Offset: 0x002AC588
		[CollectionObjectField(true, false, false, false, true)]
		public short GetConcentration()
		{
			return Accessory.Instance[this.TemplateId].Concentration;
		}

		// Token: 0x06004DBC RID: 19900 RVA: 0x002AE3B0 File Offset: 0x002AC5B0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetVitality()
		{
			return Accessory.Instance[this.TemplateId].Vitality;
		}

		// Token: 0x06004DBD RID: 19901 RVA: 0x002AE3D8 File Offset: 0x002AC5D8
		[CollectionObjectField(true, false, false, false, true)]
		public short GetEnergy()
		{
			return Accessory.Instance[this.TemplateId].Energy;
		}

		// Token: 0x06004DBE RID: 19902 RVA: 0x002AE400 File Offset: 0x002AC600
		[CollectionObjectField(true, false, false, false, true)]
		public short GetIntelligence()
		{
			return Accessory.Instance[this.TemplateId].Intelligence;
		}

		// Token: 0x06004DBF RID: 19903 RVA: 0x002AE428 File Offset: 0x002AC628
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateStrength()
		{
			return Accessory.Instance[this.TemplateId].HitRateStrength;
		}

		// Token: 0x06004DC0 RID: 19904 RVA: 0x002AE450 File Offset: 0x002AC650
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateTechnique()
		{
			return Accessory.Instance[this.TemplateId].HitRateTechnique;
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x002AE478 File Offset: 0x002AC678
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateSpeed()
		{
			return Accessory.Instance[this.TemplateId].HitRateSpeed;
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x002AE4A0 File Offset: 0x002AC6A0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateOfOuter()
		{
			return Accessory.Instance[this.TemplateId].PenetrateOfOuter;
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x002AE4C8 File Offset: 0x002AC6C8
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateOfInner()
		{
			return Accessory.Instance[this.TemplateId].PenetrateOfInner;
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x002AE4F0 File Offset: 0x002AC6F0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateStrength()
		{
			return Accessory.Instance[this.TemplateId].AvoidRateStrength;
		}

		// Token: 0x06004DC5 RID: 19909 RVA: 0x002AE518 File Offset: 0x002AC718
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateTechnique()
		{
			return Accessory.Instance[this.TemplateId].AvoidRateTechnique;
		}

		// Token: 0x06004DC6 RID: 19910 RVA: 0x002AE540 File Offset: 0x002AC740
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateSpeed()
		{
			return Accessory.Instance[this.TemplateId].AvoidRateSpeed;
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x002AE568 File Offset: 0x002AC768
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateResistOfOuter()
		{
			return Accessory.Instance[this.TemplateId].PenetrateResistOfOuter;
		}

		// Token: 0x06004DC8 RID: 19912 RVA: 0x002AE590 File Offset: 0x002AC790
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateResistOfInner()
		{
			return Accessory.Instance[this.TemplateId].PenetrateResistOfInner;
		}

		// Token: 0x06004DC9 RID: 19913 RVA: 0x002AE5B8 File Offset: 0x002AC7B8
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfStance()
		{
			return Accessory.Instance[this.TemplateId].RecoveryOfStance;
		}

		// Token: 0x06004DCA RID: 19914 RVA: 0x002AE5E0 File Offset: 0x002AC7E0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfBreath()
		{
			return Accessory.Instance[this.TemplateId].RecoveryOfBreath;
		}

		// Token: 0x06004DCB RID: 19915 RVA: 0x002AE608 File Offset: 0x002AC808
		[CollectionObjectField(true, false, false, false, true)]
		public short GetMoveSpeed()
		{
			return Accessory.Instance[this.TemplateId].MoveSpeed;
		}

		// Token: 0x06004DCC RID: 19916 RVA: 0x002AE630 File Offset: 0x002AC830
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfFlaw()
		{
			return Accessory.Instance[this.TemplateId].RecoveryOfFlaw;
		}

		// Token: 0x06004DCD RID: 19917 RVA: 0x002AE658 File Offset: 0x002AC858
		[CollectionObjectField(true, false, false, false, true)]
		public short GetCastSpeed()
		{
			return Accessory.Instance[this.TemplateId].CastSpeed;
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x002AE680 File Offset: 0x002AC880
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfBlockedAcupoint()
		{
			return Accessory.Instance[this.TemplateId].RecoveryOfBlockedAcupoint;
		}

		// Token: 0x06004DCF RID: 19919 RVA: 0x002AE6A8 File Offset: 0x002AC8A8
		[CollectionObjectField(true, false, false, false, true)]
		public short GetWeaponSwitchSpeed()
		{
			return Accessory.Instance[this.TemplateId].WeaponSwitchSpeed;
		}

		// Token: 0x06004DD0 RID: 19920 RVA: 0x002AE6D0 File Offset: 0x002AC8D0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAttackSpeed()
		{
			return Accessory.Instance[this.TemplateId].AttackSpeed;
		}

		// Token: 0x06004DD1 RID: 19921 RVA: 0x002AE6F8 File Offset: 0x002AC8F8
		[CollectionObjectField(true, false, false, false, true)]
		public short GetInnerRatio()
		{
			return Accessory.Instance[this.TemplateId].InnerRatio;
		}

		// Token: 0x06004DD2 RID: 19922 RVA: 0x002AE720 File Offset: 0x002AC920
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfQiDisorder()
		{
			return Accessory.Instance[this.TemplateId].RecoveryOfQiDisorder;
		}

		// Token: 0x06004DD3 RID: 19923 RVA: 0x002AE748 File Offset: 0x002AC948
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfHotPoison()
		{
			return Accessory.Instance[this.TemplateId].ResistOfHotPoison;
		}

		// Token: 0x06004DD4 RID: 19924 RVA: 0x002AE770 File Offset: 0x002AC970
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfGloomyPoison()
		{
			return Accessory.Instance[this.TemplateId].ResistOfGloomyPoison;
		}

		// Token: 0x06004DD5 RID: 19925 RVA: 0x002AE798 File Offset: 0x002AC998
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfColdPoison()
		{
			return Accessory.Instance[this.TemplateId].ResistOfColdPoison;
		}

		// Token: 0x06004DD6 RID: 19926 RVA: 0x002AE7C0 File Offset: 0x002AC9C0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfRedPoison()
		{
			return Accessory.Instance[this.TemplateId].ResistOfRedPoison;
		}

		// Token: 0x06004DD7 RID: 19927 RVA: 0x002AE7E8 File Offset: 0x002AC9E8
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfRottenPoison()
		{
			return Accessory.Instance[this.TemplateId].ResistOfRottenPoison;
		}

		// Token: 0x06004DD8 RID: 19928 RVA: 0x002AE810 File Offset: 0x002ACA10
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfIllusoryPoison()
		{
			return Accessory.Instance[this.TemplateId].ResistOfIllusoryPoison;
		}

		// Token: 0x06004DD9 RID: 19929 RVA: 0x002AE838 File Offset: 0x002ACA38
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetBonusCombatSkillSect()
		{
			return Accessory.Instance[this.TemplateId].BonusCombatSkillSect;
		}

		// Token: 0x06004DDA RID: 19930 RVA: 0x002AE860 File Offset: 0x002ACA60
		[CollectionObjectField(true, false, false, false, false)]
		public short GetMakeItemSubType()
		{
			return Accessory.Instance[this.TemplateId].MakeItemSubType;
		}

		// Token: 0x06004DDB RID: 19931 RVA: 0x002AE888 File Offset: 0x002ACA88
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetGiftLevel()
		{
			return Accessory.Instance[this.TemplateId].GiftLevel;
		}

		// Token: 0x06004DDC RID: 19932 RVA: 0x002AE8B0 File Offset: 0x002ACAB0
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetBaseHappinessChange()
		{
			return Accessory.Instance[this.TemplateId].BaseHappinessChange;
		}

		// Token: 0x06004DDD RID: 19933 RVA: 0x002AE8D8 File Offset: 0x002ACAD8
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetDetachable()
		{
			return Accessory.Instance[this.TemplateId].Detachable;
		}

		// Token: 0x06004DDE RID: 19934 RVA: 0x002AE900 File Offset: 0x002ACB00
		[CollectionObjectField(true, false, false, false, false)]
		public short GetGroupId()
		{
			return Accessory.Instance[this.TemplateId].GroupId;
		}

		// Token: 0x06004DDF RID: 19935 RVA: 0x002AE928 File Offset: 0x002ACB28
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetIsSpecial()
		{
			return Accessory.Instance[this.TemplateId].IsSpecial;
		}

		// Token: 0x06004DE0 RID: 19936 RVA: 0x002AE950 File Offset: 0x002ACB50
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowRawCreate()
		{
			return Accessory.Instance[this.TemplateId].AllowRawCreate;
		}

		// Token: 0x06004DE1 RID: 19937 RVA: 0x002AE978 File Offset: 0x002ACB78
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowRandomCreate()
		{
			return Accessory.Instance[this.TemplateId].AllowRandomCreate;
		}

		// Token: 0x06004DE2 RID: 19938 RVA: 0x002AE9A0 File Offset: 0x002ACBA0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateMind()
		{
			return Accessory.Instance[this.TemplateId].AvoidRateMind;
		}

		// Token: 0x06004DE3 RID: 19939 RVA: 0x002AE9C8 File Offset: 0x002ACBC8
		[CollectionObjectField(true, false, false, false, false)]
		public int GetCombatSkillAddMaxPower()
		{
			return Accessory.Instance[this.TemplateId].CombatSkillAddMaxPower;
		}

		// Token: 0x06004DE4 RID: 19940 RVA: 0x002AE9F0 File Offset: 0x002ACBF0
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetMerchantLevel()
		{
			return Accessory.Instance[this.TemplateId].MerchantLevel;
		}

		// Token: 0x06004DE5 RID: 19941 RVA: 0x002AEA18 File Offset: 0x002ACC18
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetInheritable()
		{
			return Accessory.Instance[this.TemplateId].Inheritable;
		}

		// Token: 0x06004DE6 RID: 19942 RVA: 0x002AEA40 File Offset: 0x002ACC40
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateMind()
		{
			return Accessory.Instance[this.TemplateId].HitRateMind;
		}

		// Token: 0x06004DE7 RID: 19943 RVA: 0x002AEA68 File Offset: 0x002ACC68
		[CollectionObjectField(true, false, false, false, false)]
		public short GetEquipmentCombatPowerValueFactor()
		{
			return Accessory.Instance[this.TemplateId].EquipmentCombatPowerValueFactor;
		}

		// Token: 0x06004DE8 RID: 19944 RVA: 0x002AEA90 File Offset: 0x002ACC90
		[CollectionObjectField(true, false, false, false, false)]
		public short GetBaseCaptureRateBonus()
		{
			return Accessory.Instance[this.TemplateId].BaseCaptureRateBonus;
		}

		// Token: 0x06004DE9 RID: 19945 RVA: 0x002AEAB8 File Offset: 0x002ACCB8
		[CollectionObjectField(true, false, false, false, false)]
		public short GetBaseExploreBonusRate()
		{
			return Accessory.Instance[this.TemplateId].BaseExploreBonusRate;
		}

		// Token: 0x06004DEA RID: 19946 RVA: 0x002AEADF File Offset: 0x002ACCDF
		public Accessory()
		{
		}

		// Token: 0x06004DEB RID: 19947 RVA: 0x002AEAEC File Offset: 0x002ACCEC
		public Accessory(short templateId)
		{
			AccessoryItem template = Accessory.Instance[templateId];
			this.TemplateId = template.TemplateId;
			this.MaxDurability = template.MaxDurability;
			this.EquipmentEffectId = template.EquipmentEffectId;
		}

		// Token: 0x06004DEC RID: 19948 RVA: 0x002AEB34 File Offset: 0x002ACD34
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06004DED RID: 19949 RVA: 0x002AEB48 File Offset: 0x002ACD48
		public int GetSerializedSize()
		{
			return 29;
		}

		// Token: 0x06004DEE RID: 19950 RVA: 0x002AEB60 File Offset: 0x002ACD60
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = this.Id;
			byte* pCurrData = pData + 4;
			*(short*)pCurrData = this.TemplateId;
			pCurrData += 2;
			*(short*)pCurrData = this.MaxDurability;
			pCurrData += 2;
			*(short*)pCurrData = this.EquipmentEffectId;
			pCurrData += 2;
			*(short*)pCurrData = this.CurrDurability;
			pCurrData += 2;
			*pCurrData = this.ModificationState;
			pCurrData++;
			*(int*)pCurrData = this.EquippedCharId;
			pCurrData += 4;
			pCurrData += this.MaterialResources.Serialize(pCurrData);
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06004DEF RID: 19951 RVA: 0x002AEBE0 File Offset: 0x002ACDE0
		public unsafe int Deserialize(byte* pData)
		{
			this.Id = *(int*)pData;
			byte* pCurrData = pData + 4;
			this.TemplateId = *(short*)pCurrData;
			pCurrData += 2;
			this.MaxDurability = *(short*)pCurrData;
			pCurrData += 2;
			this.EquipmentEffectId = *(short*)pCurrData;
			pCurrData += 2;
			this.CurrDurability = *(short*)pCurrData;
			pCurrData += 2;
			this.ModificationState = *pCurrData;
			pCurrData++;
			this.EquippedCharId = *(int*)pCurrData;
			pCurrData += 4;
			pCurrData += this.MaterialResources.Deserialize(pCurrData);
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06004DF0 RID: 19952 RVA: 0x002AEC5E File Offset: 0x002ACE5E
		public Accessory(IRandomSource random, short templateId, int itemId) : this(templateId)
		{
			this.Id = itemId;
			this.MaxDurability = ItemBase.GenerateMaxDurability(random, this.MaxDurability);
			this.CurrDurability = this.MaxDurability;
		}

		// Token: 0x06004DF1 RID: 19953 RVA: 0x002AEC90 File Offset: 0x002ACE90
		public override int GetFavorabilityChange()
		{
			int value = Accessory.Instance[this.TemplateId].BaseFavorabilityChange;
			bool flag = this.EquipmentEffectId >= 0;
			if (flag)
			{
				EquipmentEffectItem equipmentEffect = EquipmentEffect.Instance[this.EquipmentEffectId];
				value += value * (int)equipmentEffect.FavorChange / 100;
			}
			return value;
		}

		// Token: 0x06004DF2 RID: 19954 RVA: 0x002AECEC File Offset: 0x002ACEEC
		public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
		{
			AccessoryItem config = Accessory.Instance[this.TemplateId];
			int bonus = config.GetCharacterPropertyBonusInt(type);
			bool flag = ModificationStateHelper.IsActive(this.ModificationState, 2);
			if (flag)
			{
				ERefiningEffectAccessoryType effectType = RefiningEffects.CharPropertyTypeToRefiningEffectAccessoryType(type);
				bool flag2 = effectType != ERefiningEffectAccessoryType.Invalid;
				if (flag2)
				{
					int refineBonus = DomainManager.Item.GetRefinedEffects(base.GetItemKey()).GetAccessoryPropertyBonus(effectType);
					refineBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(refineBonus, this.EquippedCharId);
					bonus += refineBonus;
				}
			}
			return bonus;
		}

		// Token: 0x06004DF3 RID: 19955 RVA: 0x002AED72 File Offset: 0x002ACF72
		int IExploreBonusRateItem.GetExploreBonusRate()
		{
			return (int)this.GetBaseExploreBonusRate();
		}

		// Token: 0x0400156E RID: 5486
		public const int FixedSize = 29;

		// Token: 0x0400156F RID: 5487
		public const int DynamicCount = 0;

		// Token: 0x02000A9F RID: 2719
		internal class FixedFieldInfos
		{
			// Token: 0x04002B93 RID: 11155
			public const uint Id_Offset = 0U;

			// Token: 0x04002B94 RID: 11156
			public const int Id_Size = 4;

			// Token: 0x04002B95 RID: 11157
			public const uint TemplateId_Offset = 4U;

			// Token: 0x04002B96 RID: 11158
			public const int TemplateId_Size = 2;

			// Token: 0x04002B97 RID: 11159
			public const uint MaxDurability_Offset = 6U;

			// Token: 0x04002B98 RID: 11160
			public const int MaxDurability_Size = 2;

			// Token: 0x04002B99 RID: 11161
			public const uint EquipmentEffectId_Offset = 8U;

			// Token: 0x04002B9A RID: 11162
			public const int EquipmentEffectId_Size = 2;

			// Token: 0x04002B9B RID: 11163
			public const uint CurrDurability_Offset = 10U;

			// Token: 0x04002B9C RID: 11164
			public const int CurrDurability_Size = 2;

			// Token: 0x04002B9D RID: 11165
			public const uint ModificationState_Offset = 12U;

			// Token: 0x04002B9E RID: 11166
			public const int ModificationState_Size = 1;

			// Token: 0x04002B9F RID: 11167
			public const uint EquippedCharId_Offset = 13U;

			// Token: 0x04002BA0 RID: 11168
			public const int EquippedCharId_Size = 4;

			// Token: 0x04002BA1 RID: 11169
			public const uint MaterialResources_Offset = 17U;

			// Token: 0x04002BA2 RID: 11170
			public const int MaterialResources_Size = 12;
		}
	}
}
