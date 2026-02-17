using System;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Domains.Map;
using GameData.Serializer;
using Redzen.Random;

namespace GameData.Domains.Item
{
	// Token: 0x02000660 RID: 1632
	[SerializableGameData(NotForDisplayModule = true)]
	public class Clothing : EquipmentBase, ISerializableGameData
	{
		// Token: 0x06004EC9 RID: 20169 RVA: 0x002B1F84 File Offset: 0x002B0184
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

		// Token: 0x06004ECA RID: 20170 RVA: 0x002B1FE4 File Offset: 0x002B01E4
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

		// Token: 0x06004ECB RID: 20171 RVA: 0x002B2044 File Offset: 0x002B0244
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

		// Token: 0x06004ECC RID: 20172 RVA: 0x002B20A4 File Offset: 0x002B02A4
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

		// Token: 0x06004ECD RID: 20173 RVA: 0x002B2104 File Offset: 0x002B0304
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

		// Token: 0x06004ECE RID: 20174 RVA: 0x002B2164 File Offset: 0x002B0364
		public sbyte GetGender()
		{
			return this._gender;
		}

		// Token: 0x06004ECF RID: 20175 RVA: 0x002B217C File Offset: 0x002B037C
		public unsafe void SetGender(sbyte gender, DataContext context)
		{
			this._gender = gender;
			base.SetModifiedAndInvalidateInfluencedCache(7, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 17U, 1);
				*pData = (byte)this._gender;
				pData++;
			}
		}

		// Token: 0x06004ED0 RID: 20176 RVA: 0x002B21DC File Offset: 0x002B03DC
		public unsafe override void SetMaterialResources(MaterialResources materialResources, DataContext context)
		{
			this.MaterialResources = materialResources;
			base.SetModifiedAndInvalidateInfluencedCache(8, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 18U, 12);
				pData += this.MaterialResources.Serialize(pData);
			}
		}

		// Token: 0x06004ED1 RID: 20177 RVA: 0x002B2240 File Offset: 0x002B0440
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetName()
		{
			return Clothing.Instance[this.TemplateId].Name;
		}

		// Token: 0x06004ED2 RID: 20178 RVA: 0x002B2268 File Offset: 0x002B0468
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetItemType()
		{
			return Clothing.Instance[this.TemplateId].ItemType;
		}

		// Token: 0x06004ED3 RID: 20179 RVA: 0x002B2290 File Offset: 0x002B0490
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetItemSubType()
		{
			return Clothing.Instance[this.TemplateId].ItemSubType;
		}

		// Token: 0x06004ED4 RID: 20180 RVA: 0x002B22B8 File Offset: 0x002B04B8
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetGrade()
		{
			return Clothing.Instance[this.TemplateId].Grade;
		}

		// Token: 0x06004ED5 RID: 20181 RVA: 0x002B22E0 File Offset: 0x002B04E0
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetIcon()
		{
			return Clothing.Instance[this.TemplateId].Icon;
		}

		// Token: 0x06004ED6 RID: 20182 RVA: 0x002B2308 File Offset: 0x002B0508
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetDesc()
		{
			return Clothing.Instance[this.TemplateId].Desc;
		}

		// Token: 0x06004ED7 RID: 20183 RVA: 0x002B2330 File Offset: 0x002B0530
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetTransferable()
		{
			return Clothing.Instance[this.TemplateId].Transferable;
		}

		// Token: 0x06004ED8 RID: 20184 RVA: 0x002B2358 File Offset: 0x002B0558
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetStackable()
		{
			return Clothing.Instance[this.TemplateId].Stackable;
		}

		// Token: 0x06004ED9 RID: 20185 RVA: 0x002B2380 File Offset: 0x002B0580
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetWagerable()
		{
			return Clothing.Instance[this.TemplateId].Wagerable;
		}

		// Token: 0x06004EDA RID: 20186 RVA: 0x002B23A8 File Offset: 0x002B05A8
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRefinable()
		{
			return Clothing.Instance[this.TemplateId].Refinable;
		}

		// Token: 0x06004EDB RID: 20187 RVA: 0x002B23D0 File Offset: 0x002B05D0
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetPoisonable()
		{
			return Clothing.Instance[this.TemplateId].Poisonable;
		}

		// Token: 0x06004EDC RID: 20188 RVA: 0x002B23F8 File Offset: 0x002B05F8
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRepairable()
		{
			return Clothing.Instance[this.TemplateId].Repairable;
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x002B2420 File Offset: 0x002B0620
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseWeight()
		{
			return Clothing.Instance[this.TemplateId].BaseWeight;
		}

		// Token: 0x06004EDE RID: 20190 RVA: 0x002B2448 File Offset: 0x002B0648
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseValue()
		{
			return Clothing.Instance[this.TemplateId].BaseValue;
		}

		// Token: 0x06004EDF RID: 20191 RVA: 0x002B2470 File Offset: 0x002B0670
		[CollectionObjectField(true, false, false, false, false)]
		public int GetBasePrice()
		{
			return Clothing.Instance[this.TemplateId].BasePrice;
		}

		// Token: 0x06004EE0 RID: 20192 RVA: 0x002B2498 File Offset: 0x002B0698
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetDropRate()
		{
			return Clothing.Instance[this.TemplateId].DropRate;
		}

		// Token: 0x06004EE1 RID: 20193 RVA: 0x002B24C0 File Offset: 0x002B06C0
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetResourceType()
		{
			return Clothing.Instance[this.TemplateId].ResourceType;
		}

		// Token: 0x06004EE2 RID: 20194 RVA: 0x002B24E8 File Offset: 0x002B06E8
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetPreservationDuration()
		{
			return Clothing.Instance[this.TemplateId].PreservationDuration;
		}

		// Token: 0x06004EE3 RID: 20195 RVA: 0x002B2510 File Offset: 0x002B0710
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetEquipmentType()
		{
			return Clothing.Instance[this.TemplateId].EquipmentType;
		}

		// Token: 0x06004EE4 RID: 20196 RVA: 0x002B2538 File Offset: 0x002B0738
		[CollectionObjectField(true, false, false, false, false)]
		public short GetDisplayId()
		{
			return Clothing.Instance[this.TemplateId].DisplayId;
		}

		// Token: 0x06004EE5 RID: 20197 RVA: 0x002B2560 File Offset: 0x002B0760
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetAgeGroup()
		{
			return Clothing.Instance[this.TemplateId].AgeGroup;
		}

		// Token: 0x06004EE6 RID: 20198 RVA: 0x002B2588 File Offset: 0x002B0788
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetKeepOnPassing()
		{
			return Clothing.Instance[this.TemplateId].KeepOnPassing;
		}

		// Token: 0x06004EE7 RID: 20199 RVA: 0x002B25B0 File Offset: 0x002B07B0
		[CollectionObjectField(true, false, false, false, false)]
		public short GetMakeItemSubType()
		{
			return Clothing.Instance[this.TemplateId].MakeItemSubType;
		}

		// Token: 0x06004EE8 RID: 20200 RVA: 0x002B25D8 File Offset: 0x002B07D8
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetGiftLevel()
		{
			return Clothing.Instance[this.TemplateId].GiftLevel;
		}

		// Token: 0x06004EE9 RID: 20201 RVA: 0x002B2600 File Offset: 0x002B0800
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseFavorabilityChange()
		{
			return Clothing.Instance[this.TemplateId].BaseFavorabilityChange;
		}

		// Token: 0x06004EEA RID: 20202 RVA: 0x002B2628 File Offset: 0x002B0828
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetBaseHappinessChange()
		{
			return Clothing.Instance[this.TemplateId].BaseHappinessChange;
		}

		// Token: 0x06004EEB RID: 20203 RVA: 0x002B2650 File Offset: 0x002B0850
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetDetachable()
		{
			return Clothing.Instance[this.TemplateId].Detachable;
		}

		// Token: 0x06004EEC RID: 20204 RVA: 0x002B2678 File Offset: 0x002B0878
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowRandomCreate()
		{
			return Clothing.Instance[this.TemplateId].AllowRandomCreate;
		}

		// Token: 0x06004EED RID: 20205 RVA: 0x002B26A0 File Offset: 0x002B08A0
		[CollectionObjectField(true, false, false, false, false)]
		public short GetWeaveNeedAttainment()
		{
			return Clothing.Instance[this.TemplateId].WeaveNeedAttainment;
		}

		// Token: 0x06004EEE RID: 20206 RVA: 0x002B26C8 File Offset: 0x002B08C8
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetWeaveType()
		{
			return Clothing.Instance[this.TemplateId].WeaveType;
		}

		// Token: 0x06004EEF RID: 20207 RVA: 0x002B26F0 File Offset: 0x002B08F0
		[CollectionObjectField(true, false, false, false, false)]
		public string GetDlcName()
		{
			return Clothing.Instance[this.TemplateId].DlcName;
		}

		// Token: 0x06004EF0 RID: 20208 RVA: 0x002B2718 File Offset: 0x002B0918
		[CollectionObjectField(true, false, false, false, false)]
		public string GetSmallVillageDesc()
		{
			return Clothing.Instance[this.TemplateId].SmallVillageDesc;
		}

		// Token: 0x06004EF1 RID: 20209 RVA: 0x002B2740 File Offset: 0x002B0940
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetMerchantLevel()
		{
			return Clothing.Instance[this.TemplateId].MerchantLevel;
		}

		// Token: 0x06004EF2 RID: 20210 RVA: 0x002B2768 File Offset: 0x002B0968
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetInheritable()
		{
			return Clothing.Instance[this.TemplateId].Inheritable;
		}

		// Token: 0x06004EF3 RID: 20211 RVA: 0x002B2790 File Offset: 0x002B0990
		[CollectionObjectField(true, false, false, false, false)]
		public short GetGroupId()
		{
			return Clothing.Instance[this.TemplateId].GroupId;
		}

		// Token: 0x06004EF4 RID: 20212 RVA: 0x002B27B8 File Offset: 0x002B09B8
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetIsSpecial()
		{
			return Clothing.Instance[this.TemplateId].IsSpecial;
		}

		// Token: 0x06004EF5 RID: 20213 RVA: 0x002B27E0 File Offset: 0x002B09E0
		[CollectionObjectField(true, false, false, false, false)]
		public short GetEquipmentCombatPowerValueFactor()
		{
			return Clothing.Instance[this.TemplateId].EquipmentCombatPowerValueFactor;
		}

		// Token: 0x06004EF6 RID: 20214 RVA: 0x002B2807 File Offset: 0x002B0A07
		public Clothing()
		{
		}

		// Token: 0x06004EF7 RID: 20215 RVA: 0x002B2814 File Offset: 0x002B0A14
		public Clothing(short templateId)
		{
			ClothingItem template = Clothing.Instance[templateId];
			this.TemplateId = template.TemplateId;
			this.MaxDurability = template.MaxDurability;
			this.EquipmentEffectId = template.EquipmentEffectId;
		}

		// Token: 0x06004EF8 RID: 20216 RVA: 0x002B285C File Offset: 0x002B0A5C
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06004EF9 RID: 20217 RVA: 0x002B2870 File Offset: 0x002B0A70
		public int GetSerializedSize()
		{
			return 30;
		}

		// Token: 0x06004EFA RID: 20218 RVA: 0x002B2888 File Offset: 0x002B0A88
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
			*pCurrData = (byte)this._gender;
			pCurrData++;
			pCurrData += this.MaterialResources.Serialize(pCurrData);
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06004EFB RID: 20219 RVA: 0x002B2914 File Offset: 0x002B0B14
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
			this._gender = *(sbyte*)pCurrData;
			pCurrData++;
			pCurrData += this.MaterialResources.Deserialize(pCurrData);
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x002B299E File Offset: 0x002B0B9E
		public Clothing(IRandomSource random, short templateId, int itemId, sbyte gender) : this(templateId)
		{
			this.Id = itemId;
			this.MaxDurability = ItemBase.GenerateMaxDurability(random, this.MaxDurability);
			this.CurrDurability = this.MaxDurability;
			this._gender = gender;
		}

		// Token: 0x06004EFD RID: 20221 RVA: 0x002B29D8 File Offset: 0x002B0BD8
		public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
		{
			return 0;
		}

		// Token: 0x0400158F RID: 5519
		[CollectionObjectField(false, true, false, false, false)]
		private sbyte _gender;

		// Token: 0x04001590 RID: 5520
		public const int FixedSize = 30;

		// Token: 0x04001591 RID: 5521
		public const int DynamicCount = 0;

		// Token: 0x02000AA5 RID: 2725
		internal class FixedFieldInfos
		{
			// Token: 0x04002BC9 RID: 11209
			public const uint Id_Offset = 0U;

			// Token: 0x04002BCA RID: 11210
			public const int Id_Size = 4;

			// Token: 0x04002BCB RID: 11211
			public const uint TemplateId_Offset = 4U;

			// Token: 0x04002BCC RID: 11212
			public const int TemplateId_Size = 2;

			// Token: 0x04002BCD RID: 11213
			public const uint MaxDurability_Offset = 6U;

			// Token: 0x04002BCE RID: 11214
			public const int MaxDurability_Size = 2;

			// Token: 0x04002BCF RID: 11215
			public const uint EquipmentEffectId_Offset = 8U;

			// Token: 0x04002BD0 RID: 11216
			public const int EquipmentEffectId_Size = 2;

			// Token: 0x04002BD1 RID: 11217
			public const uint CurrDurability_Offset = 10U;

			// Token: 0x04002BD2 RID: 11218
			public const int CurrDurability_Size = 2;

			// Token: 0x04002BD3 RID: 11219
			public const uint ModificationState_Offset = 12U;

			// Token: 0x04002BD4 RID: 11220
			public const int ModificationState_Size = 1;

			// Token: 0x04002BD5 RID: 11221
			public const uint EquippedCharId_Offset = 13U;

			// Token: 0x04002BD6 RID: 11222
			public const int EquippedCharId_Size = 4;

			// Token: 0x04002BD7 RID: 11223
			public const uint Gender_Offset = 17U;

			// Token: 0x04002BD8 RID: 11224
			public const int Gender_Size = 1;

			// Token: 0x04002BD9 RID: 11225
			public const uint MaterialResources_Offset = 18U;

			// Token: 0x04002BDA RID: 11226
			public const int MaterialResources_Size = 12;
		}
	}
}
