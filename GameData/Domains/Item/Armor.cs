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

namespace GameData.Domains.Item
{
	// Token: 0x0200065A RID: 1626
	[SerializableGameData(NotForDisplayModule = true)]
	public class Armor : EquipmentBase, ISerializableGameData
	{
		// Token: 0x06004DF4 RID: 19956 RVA: 0x002AED7C File Offset: 0x002ACF7C
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

		// Token: 0x06004DF5 RID: 19957 RVA: 0x002AEDDC File Offset: 0x002ACFDC
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

		// Token: 0x06004DF6 RID: 19958 RVA: 0x002AEE3C File Offset: 0x002AD03C
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

		// Token: 0x06004DF7 RID: 19959 RVA: 0x002AEE9C File Offset: 0x002AD09C
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

		// Token: 0x06004DF8 RID: 19960 RVA: 0x002AEEFC File Offset: 0x002AD0FC
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

		// Token: 0x06004DF9 RID: 19961 RVA: 0x002AEF5C File Offset: 0x002AD15C
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

		// Token: 0x06004DFA RID: 19962 RVA: 0x002AEFC0 File Offset: 0x002AD1C0
		public OuterAndInnerShorts GetPenetrationResistFactors()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 8);
			OuterAndInnerShorts penetrationResistFactors;
			if (flag)
			{
				penetrationResistFactors = this._penetrationResistFactors;
			}
			else
			{
				OuterAndInnerShorts value = this.CalcPenetrationResistFactors();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._penetrationResistFactors = value;
					dataStates.SetCached(this.DataStatesOffset, 8);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				penetrationResistFactors = this._penetrationResistFactors;
			}
			return penetrationResistFactors;
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x002AF064 File Offset: 0x002AD264
		public short GetEquipmentAttack()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 9);
			short equipmentAttack;
			if (flag)
			{
				equipmentAttack = this._equipmentAttack;
			}
			else
			{
				short value = this.CalcEquipmentAttack();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._equipmentAttack = value;
					dataStates.SetCached(this.DataStatesOffset, 9);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				equipmentAttack = this._equipmentAttack;
			}
			return equipmentAttack;
		}

		// Token: 0x06004DFC RID: 19964 RVA: 0x002AF10C File Offset: 0x002AD30C
		public short GetEquipmentDefense()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 10);
			short equipmentDefense;
			if (flag)
			{
				equipmentDefense = this._equipmentDefense;
			}
			else
			{
				short value = this.CalcEquipmentDefense();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._equipmentDefense = value;
					dataStates.SetCached(this.DataStatesOffset, 10);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				equipmentDefense = this._equipmentDefense;
			}
			return equipmentDefense;
		}

		// Token: 0x06004DFD RID: 19965 RVA: 0x002AF1B4 File Offset: 0x002AD3B4
		public override int GetWeight()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 11);
			int weight;
			if (flag)
			{
				weight = this._weight;
			}
			else
			{
				int value = this.CalcWeight();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._weight = value;
					dataStates.SetCached(this.DataStatesOffset, 11);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				weight = this._weight;
			}
			return weight;
		}

		// Token: 0x06004DFE RID: 19966 RVA: 0x002AF25C File Offset: 0x002AD45C
		public OuterAndInnerShorts GetInjuryFactor()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 12);
			OuterAndInnerShorts injuryFactor;
			if (flag)
			{
				injuryFactor = this._injuryFactor;
			}
			else
			{
				OuterAndInnerShorts value = this.CalcInjuryFactor();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this._injuryFactor = value;
					dataStates.SetCached(this.DataStatesOffset, 12);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				injuryFactor = this._injuryFactor;
			}
			return injuryFactor;
		}

		// Token: 0x06004DFF RID: 19967 RVA: 0x002AF304 File Offset: 0x002AD504
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetName()
		{
			return Armor.Instance[this.TemplateId].Name;
		}

		// Token: 0x06004E00 RID: 19968 RVA: 0x002AF32C File Offset: 0x002AD52C
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetItemType()
		{
			return Armor.Instance[this.TemplateId].ItemType;
		}

		// Token: 0x06004E01 RID: 19969 RVA: 0x002AF354 File Offset: 0x002AD554
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetItemSubType()
		{
			return Armor.Instance[this.TemplateId].ItemSubType;
		}

		// Token: 0x06004E02 RID: 19970 RVA: 0x002AF37C File Offset: 0x002AD57C
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetGrade()
		{
			return Armor.Instance[this.TemplateId].Grade;
		}

		// Token: 0x06004E03 RID: 19971 RVA: 0x002AF3A4 File Offset: 0x002AD5A4
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetIcon()
		{
			return Armor.Instance[this.TemplateId].Icon;
		}

		// Token: 0x06004E04 RID: 19972 RVA: 0x002AF3CC File Offset: 0x002AD5CC
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetDesc()
		{
			return Armor.Instance[this.TemplateId].Desc;
		}

		// Token: 0x06004E05 RID: 19973 RVA: 0x002AF3F4 File Offset: 0x002AD5F4
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetTransferable()
		{
			return Armor.Instance[this.TemplateId].Transferable;
		}

		// Token: 0x06004E06 RID: 19974 RVA: 0x002AF41C File Offset: 0x002AD61C
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetStackable()
		{
			return Armor.Instance[this.TemplateId].Stackable;
		}

		// Token: 0x06004E07 RID: 19975 RVA: 0x002AF444 File Offset: 0x002AD644
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetWagerable()
		{
			return Armor.Instance[this.TemplateId].Wagerable;
		}

		// Token: 0x06004E08 RID: 19976 RVA: 0x002AF46C File Offset: 0x002AD66C
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRefinable()
		{
			return Armor.Instance[this.TemplateId].Refinable;
		}

		// Token: 0x06004E09 RID: 19977 RVA: 0x002AF494 File Offset: 0x002AD694
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetPoisonable()
		{
			return Armor.Instance[this.TemplateId].Poisonable;
		}

		// Token: 0x06004E0A RID: 19978 RVA: 0x002AF4BC File Offset: 0x002AD6BC
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRepairable()
		{
			return Armor.Instance[this.TemplateId].Repairable;
		}

		// Token: 0x06004E0B RID: 19979 RVA: 0x002AF4E4 File Offset: 0x002AD6E4
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseWeight()
		{
			return Armor.Instance[this.TemplateId].BaseWeight;
		}

		// Token: 0x06004E0C RID: 19980 RVA: 0x002AF50C File Offset: 0x002AD70C
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseValue()
		{
			return Armor.Instance[this.TemplateId].BaseValue;
		}

		// Token: 0x06004E0D RID: 19981 RVA: 0x002AF534 File Offset: 0x002AD734
		[CollectionObjectField(true, false, false, false, false)]
		public int GetBasePrice()
		{
			return Armor.Instance[this.TemplateId].BasePrice;
		}

		// Token: 0x06004E0E RID: 19982 RVA: 0x002AF55C File Offset: 0x002AD75C
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseFavorabilityChange()
		{
			return Armor.Instance[this.TemplateId].BaseFavorabilityChange;
		}

		// Token: 0x06004E0F RID: 19983 RVA: 0x002AF584 File Offset: 0x002AD784
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetDropRate()
		{
			return Armor.Instance[this.TemplateId].DropRate;
		}

		// Token: 0x06004E10 RID: 19984 RVA: 0x002AF5AC File Offset: 0x002AD7AC
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetResourceType()
		{
			return Armor.Instance[this.TemplateId].ResourceType;
		}

		// Token: 0x06004E11 RID: 19985 RVA: 0x002AF5D4 File Offset: 0x002AD7D4
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetPreservationDuration()
		{
			return Armor.Instance[this.TemplateId].PreservationDuration;
		}

		// Token: 0x06004E12 RID: 19986 RVA: 0x002AF5FC File Offset: 0x002AD7FC
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetEquipmentType()
		{
			return Armor.Instance[this.TemplateId].EquipmentType;
		}

		// Token: 0x06004E13 RID: 19987 RVA: 0x002AF624 File Offset: 0x002AD824
		[CollectionObjectField(true, false, false, false, false)]
		public short GetBaseEquipmentAttack()
		{
			return Armor.Instance[this.TemplateId].BaseEquipmentAttack;
		}

		// Token: 0x06004E14 RID: 19988 RVA: 0x002AF64C File Offset: 0x002AD84C
		[CollectionObjectField(true, false, false, false, false)]
		public short GetBaseEquipmentDefense()
		{
			return Armor.Instance[this.TemplateId].BaseEquipmentDefense;
		}

		// Token: 0x06004E15 RID: 19989 RVA: 0x002AF674 File Offset: 0x002AD874
		[CollectionObjectField(true, false, false, false, false)]
		public List<PropertyAndValue> GetRequiredCharacterProperties()
		{
			return Armor.Instance[this.TemplateId].RequiredCharacterProperties;
		}

		// Token: 0x06004E16 RID: 19990 RVA: 0x002AF69C File Offset: 0x002AD89C
		[CollectionObjectField(true, false, false, false, false)]
		public HitOrAvoidShorts GetBaseAvoidFactors()
		{
			return Armor.Instance[this.TemplateId].BaseAvoidFactors;
		}

		// Token: 0x06004E17 RID: 19991 RVA: 0x002AF6C4 File Offset: 0x002AD8C4
		[CollectionObjectField(true, false, false, false, false)]
		public OuterAndInnerShorts GetBasePenetrationResistFactors()
		{
			return Armor.Instance[this.TemplateId].BasePenetrationResistFactors;
		}

		// Token: 0x06004E18 RID: 19992 RVA: 0x002AF6EC File Offset: 0x002AD8EC
		[CollectionObjectField(true, false, false, false, false)]
		public short GetRelatedWeapon()
		{
			return Armor.Instance[this.TemplateId].RelatedWeapon;
		}

		// Token: 0x06004E19 RID: 19993 RVA: 0x002AF714 File Offset: 0x002AD914
		[CollectionObjectField(true, false, false, false, false)]
		public List<string> GetSkeletonSlotAndAttachment()
		{
			return Armor.Instance[this.TemplateId].SkeletonSlotAndAttachment;
		}

		// Token: 0x06004E1A RID: 19994 RVA: 0x002AF73C File Offset: 0x002AD93C
		[CollectionObjectField(true, false, false, false, false)]
		public short GetMakeItemSubType()
		{
			return Armor.Instance[this.TemplateId].MakeItemSubType;
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x002AF764 File Offset: 0x002AD964
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetGiftLevel()
		{
			return Armor.Instance[this.TemplateId].GiftLevel;
		}

		// Token: 0x06004E1C RID: 19996 RVA: 0x002AF78C File Offset: 0x002AD98C
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetBaseHappinessChange()
		{
			return Armor.Instance[this.TemplateId].BaseHappinessChange;
		}

		// Token: 0x06004E1D RID: 19997 RVA: 0x002AF7B4 File Offset: 0x002AD9B4
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetDetachable()
		{
			return Armor.Instance[this.TemplateId].Detachable;
		}

		// Token: 0x06004E1E RID: 19998 RVA: 0x002AF7DC File Offset: 0x002AD9DC
		[CollectionObjectField(true, false, false, false, false)]
		public OuterAndInnerShorts GetBaseInjuryFactors()
		{
			return Armor.Instance[this.TemplateId].BaseInjuryFactors;
		}

		// Token: 0x06004E1F RID: 19999 RVA: 0x002AF804 File Offset: 0x002ADA04
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetIsSpecial()
		{
			return Armor.Instance[this.TemplateId].IsSpecial;
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x002AF82C File Offset: 0x002ADA2C
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowRandomCreate()
		{
			return Armor.Instance[this.TemplateId].AllowRandomCreate;
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x002AF854 File Offset: 0x002ADA54
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetMerchantLevel()
		{
			return Armor.Instance[this.TemplateId].MerchantLevel;
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x002AF87C File Offset: 0x002ADA7C
		[CollectionObjectField(true, false, false, false, false)]
		public short GetGroupId()
		{
			return Armor.Instance[this.TemplateId].GroupId;
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x002AF8A4 File Offset: 0x002ADAA4
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowCrippledCreate()
		{
			return Armor.Instance[this.TemplateId].AllowCrippledCreate;
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x002AF8CC File Offset: 0x002ADACC
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetInheritable()
		{
			return Armor.Instance[this.TemplateId].Inheritable;
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x002AF8F4 File Offset: 0x002ADAF4
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowRawCreate()
		{
			return Armor.Instance[this.TemplateId].AllowRawCreate;
		}

		// Token: 0x06004E26 RID: 20006 RVA: 0x002AF91C File Offset: 0x002ADB1C
		[CollectionObjectField(true, false, false, false, false)]
		public short GetEquipmentCombatPowerValueFactor()
		{
			return Armor.Instance[this.TemplateId].EquipmentCombatPowerValueFactor;
		}

		// Token: 0x06004E27 RID: 20007 RVA: 0x002AF943 File Offset: 0x002ADB43
		public Armor()
		{
		}

		// Token: 0x06004E28 RID: 20008 RVA: 0x002AF95C File Offset: 0x002ADB5C
		public Armor(short templateId)
		{
			ArmorItem template = Armor.Instance[templateId];
			this.TemplateId = template.TemplateId;
			this.MaxDurability = template.MaxDurability;
			this.EquipmentEffectId = template.EquipmentEffectId;
		}

		// Token: 0x06004E29 RID: 20009 RVA: 0x002AF9B0 File Offset: 0x002ADBB0
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06004E2A RID: 20010 RVA: 0x002AF9C4 File Offset: 0x002ADBC4
		public int GetSerializedSize()
		{
			return 29;
		}

		// Token: 0x06004E2B RID: 20011 RVA: 0x002AF9DC File Offset: 0x002ADBDC
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

		// Token: 0x06004E2C RID: 20012 RVA: 0x002AFA5C File Offset: 0x002ADC5C
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

		// Token: 0x06004E2D RID: 20013 RVA: 0x002AFADC File Offset: 0x002ADCDC
		[ObjectCollectionDependency(6, 1, new ushort[]
		{
			5,
			3,
			7
		}, Scope = InfluenceScope.Self)]
		private OuterAndInnerShorts CalcPenetrationResistFactors()
		{
			OuterAndInnerInts value = this.GetBasePenetrationResistFactors();
			CValuePercent percent = base.GetMaterialResourceBonusValuePercentage(3);
			value.Outer *= percent;
			value.Inner *= percent;
			EquipmentEffectHelper.ValueSelector outerSelector = EquipmentEffectHelper.GetPenetrationResistFactorSelector(false);
			value.Outer = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(value.Outer, outerSelector);
			EquipmentEffectHelper.ValueSelector innerSelector = EquipmentEffectHelper.GetPenetrationResistFactorSelector(true);
			value.Inner = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(value.Inner, innerSelector);
			bool flag = ModificationStateHelper.IsActive(this.ModificationState, 2);
			if (flag)
			{
				int refineBonus = DomainManager.Item.GetRefinedEffects(base.GetItemKey()).GetArmorPropertyBonus(ERefiningEffectArmorType.PenetrationResist);
				refineBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(refineBonus, this.EquippedCharId);
				value.Inner += refineBonus;
				value.Outer += refineBonus;
			}
			return (OuterAndInnerShorts)value;
		}

		// Token: 0x06004E2E RID: 20014 RVA: 0x002AFBD4 File Offset: 0x002ADDD4
		[ObjectCollectionDependency(6, 1, new ushort[]
		{
			3
		}, Scope = InfluenceScope.Self)]
		private OuterAndInnerShorts CalcInjuryFactor()
		{
			OuterAndInnerInts value = this.GetBaseInjuryFactors();
			EquipmentEffectHelper.ValueSelector outerSelector = EquipmentEffectHelper.GetInjuryFactorSelector(false);
			value.Outer = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(value.Outer, outerSelector);
			EquipmentEffectHelper.ValueSelector innerSelector = EquipmentEffectHelper.GetInjuryFactorSelector(true);
			value.Inner = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(value.Inner, innerSelector);
			return (OuterAndInnerShorts)value;
		}

		// Token: 0x06004E2F RID: 20015 RVA: 0x002AFC44 File Offset: 0x002ADE44
		[ObjectCollectionDependency(6, 1, new ushort[]
		{
			5,
			3,
			7
		}, Scope = InfluenceScope.Self)]
		private short CalcEquipmentAttack()
		{
			int value = (int)this.GetBaseEquipmentAttack();
			value = value * base.GetMaterialResourceBonusValuePercentage(0) / 100;
			value = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(value, (EquipmentEffectItem x) => (int)x.EquipmentAttackChange);
			bool flag = ModificationStateHelper.IsActive(this.ModificationState, 2);
			if (flag)
			{
				int refineBonus = DomainManager.Item.GetRefinedEffects(base.GetItemKey()).GetArmorPropertyBonus(ERefiningEffectArmorType.EquipmentAttack);
				refineBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(refineBonus, this.EquippedCharId);
				value = value * (100 + refineBonus) / 100;
			}
			return (short)value;
		}

		// Token: 0x06004E30 RID: 20016 RVA: 0x002AFCE4 File Offset: 0x002ADEE4
		[ObjectCollectionDependency(6, 1, new ushort[]
		{
			5,
			3,
			7
		}, Scope = InfluenceScope.Self)]
		private short CalcEquipmentDefense()
		{
			int value = (int)this.GetBaseEquipmentDefense();
			value = value * base.GetMaterialResourceBonusValuePercentage(1) / 100;
			value = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(value, (EquipmentEffectItem x) => (int)x.EquipmentDefenseChange);
			bool flag = ModificationStateHelper.IsActive(this.ModificationState, 2);
			if (flag)
			{
				int refineBonus = DomainManager.Item.GetRefinedEffects(base.GetItemKey()).GetArmorPropertyBonus(ERefiningEffectArmorType.EquipmentDefense);
				refineBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(refineBonus, this.EquippedCharId);
				value = value * (100 + refineBonus) / 100;
			}
			return (short)value;
		}

		// Token: 0x06004E31 RID: 20017 RVA: 0x002AFD84 File Offset: 0x002ADF84
		[ObjectCollectionDependency(6, 1, new ushort[]
		{
			5,
			3,
			7
		}, Scope = InfluenceScope.Self)]
		private int CalcWeight()
		{
			int value = this.GetBaseWeight();
			value = value * (170 - base.GetMaterialResourceBonusValuePercentage(4)) / 100;
			value = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(value, (EquipmentEffectItem x) => (int)x.WeightChange);
			bool flag = ModificationStateHelper.IsActive(this.ModificationState, 2);
			if (flag)
			{
				int refineBonus = DomainManager.Item.GetRefinedEffects(base.GetItemKey()).GetArmorPropertyBonus(ERefiningEffectArmorType.Weight);
				refineBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(refineBonus, this.EquippedCharId);
				value = value * (100 + refineBonus) / 100;
			}
			return value;
		}

		// Token: 0x06004E32 RID: 20018 RVA: 0x002AFE27 File Offset: 0x002AE027
		public Armor(IRandomSource random, short templateId, int itemId) : this(templateId)
		{
			this.Id = itemId;
			this.MaxDurability = ItemBase.GenerateMaxDurability(random, this.MaxDurability);
			this.CurrDurability = this.MaxDurability;
		}

		// Token: 0x06004E33 RID: 20019 RVA: 0x002AFE58 File Offset: 0x002AE058
		public override int GetFavorabilityChange()
		{
			int value = Armor.Instance[this.TemplateId].BaseFavorabilityChange;
			bool flag = this.EquipmentEffectId >= 0;
			if (flag)
			{
				EquipmentEffectItem equipmentEffect = EquipmentEffect.Instance[this.EquipmentEffectId];
				value += value * (int)equipmentEffect.FavorChange / 100;
			}
			return value;
		}

		// Token: 0x06004E34 RID: 20020 RVA: 0x002AFEB4 File Offset: 0x002AE0B4
		public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
		{
			return 0;
		}

		// Token: 0x06004E35 RID: 20021 RVA: 0x002AFEC8 File Offset: 0x002AE0C8
		public unsafe HitOrAvoidShorts GetAvoidFactors()
		{
			HitOrAvoidInts value = this.GetBaseAvoidFactors();
			for (int i = 0; i < 4; i++)
			{
				EquipmentEffectHelper.ValueSelector selector = EquipmentEffectHelper.GetAvoidFactorSelector(i);
				value[i] = DomainManager.Item.GetEquipmentEffects(this).ModifyValue(value[i], selector);
			}
			bool flag = ModificationStateHelper.IsActive(this.ModificationState, 2);
			if (flag)
			{
				RefiningEffects refiningEffects = DomainManager.Item.GetRefinedEffects(base.GetItemKey());
				for (int hitType = 0; hitType < 4; hitType++)
				{
					int refineBonus = refiningEffects.GetArmorPropertyBonus(ERefiningEffectArmorType.AvoidRateStrength + hitType);
					refineBonus = ProfessionSkillHandle.GetRefineBonus_CraftSkill_2(refineBonus, this.EquippedCharId);
					*(ref value.Items.FixedElementField + (IntPtr)hitType * 4) += Math.Abs(*(ref value.Items.FixedElementField + (IntPtr)hitType * 4) * refineBonus / 100);
				}
			}
			return (HitOrAvoidShorts)value;
		}

		// Token: 0x06004E36 RID: 20022 RVA: 0x002AFFB8 File Offset: 0x002AE1B8
		public unsafe HitOrAvoidShorts GetAvoidFactors(int charId)
		{
			HitOrAvoidShorts avoidFactors = this.GetAvoidFactors();
			short usePower = DomainManager.Character.GetItemPower(charId, base.GetItemKey());
			for (int hitType = 0; hitType < 4; hitType++)
			{
				int avoidValue = (int)(*(ref avoidFactors.Items.FixedElementField + (IntPtr)hitType * 2));
				bool flag = avoidValue > 0;
				if (flag)
				{
					*(ref avoidFactors.Items.FixedElementField + (IntPtr)hitType * 2) = (short)(avoidValue * (int)usePower / 100);
				}
			}
			return avoidFactors;
		}

		// Token: 0x04001570 RID: 5488
		[CollectionObjectField(false, false, true, false, false)]
		private OuterAndInnerShorts _penetrationResistFactors;

		// Token: 0x04001571 RID: 5489
		[CollectionObjectField(false, false, true, false, false)]
		private short _equipmentAttack;

		// Token: 0x04001572 RID: 5490
		[CollectionObjectField(false, false, true, false, false)]
		private short _equipmentDefense;

		// Token: 0x04001573 RID: 5491
		[CollectionObjectField(false, false, true, false, false)]
		private int _weight;

		// Token: 0x04001574 RID: 5492
		[CollectionObjectField(false, false, true, false, false)]
		private OuterAndInnerShorts _injuryFactor;

		// Token: 0x04001575 RID: 5493
		public const int FixedSize = 29;

		// Token: 0x04001576 RID: 5494
		public const int DynamicCount = 0;

		// Token: 0x04001577 RID: 5495
		private SpinLock _spinLock = new SpinLock(false);

		// Token: 0x02000AA0 RID: 2720
		internal class FixedFieldInfos
		{
			// Token: 0x04002BA3 RID: 11171
			public const uint Id_Offset = 0U;

			// Token: 0x04002BA4 RID: 11172
			public const int Id_Size = 4;

			// Token: 0x04002BA5 RID: 11173
			public const uint TemplateId_Offset = 4U;

			// Token: 0x04002BA6 RID: 11174
			public const int TemplateId_Size = 2;

			// Token: 0x04002BA7 RID: 11175
			public const uint MaxDurability_Offset = 6U;

			// Token: 0x04002BA8 RID: 11176
			public const int MaxDurability_Size = 2;

			// Token: 0x04002BA9 RID: 11177
			public const uint EquipmentEffectId_Offset = 8U;

			// Token: 0x04002BAA RID: 11178
			public const int EquipmentEffectId_Size = 2;

			// Token: 0x04002BAB RID: 11179
			public const uint CurrDurability_Offset = 10U;

			// Token: 0x04002BAC RID: 11180
			public const int CurrDurability_Size = 2;

			// Token: 0x04002BAD RID: 11181
			public const uint ModificationState_Offset = 12U;

			// Token: 0x04002BAE RID: 11182
			public const int ModificationState_Size = 1;

			// Token: 0x04002BAF RID: 11183
			public const uint EquippedCharId_Offset = 13U;

			// Token: 0x04002BB0 RID: 11184
			public const int EquippedCharId_Size = 4;

			// Token: 0x04002BB1 RID: 11185
			public const uint MaterialResources_Offset = 17U;

			// Token: 0x04002BB2 RID: 11186
			public const int MaterialResources_Size = 12;
		}
	}
}
