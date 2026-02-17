using System;
using System.Collections.Generic;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Domains.Taiwu.Profession;
using GameData.Serializer;
using Redzen.Random;

namespace GameData.Domains.Item
{
	// Token: 0x02000661 RID: 1633
	[SerializableGameData(NotForDisplayModule = true)]
	public class CraftTool : ItemBase, ISerializableGameData
	{
		// Token: 0x06004EFE RID: 20222 RVA: 0x002B29EC File Offset: 0x002B0BEC
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

		// Token: 0x06004EFF RID: 20223 RVA: 0x002B2A4C File Offset: 0x002B0C4C
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

		// Token: 0x06004F00 RID: 20224 RVA: 0x002B2AAC File Offset: 0x002B0CAC
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

		// Token: 0x06004F01 RID: 20225 RVA: 0x002B2B0C File Offset: 0x002B0D0C
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetName()
		{
			return CraftTool.Instance[this.TemplateId].Name;
		}

		// Token: 0x06004F02 RID: 20226 RVA: 0x002B2B34 File Offset: 0x002B0D34
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetItemType()
		{
			return CraftTool.Instance[this.TemplateId].ItemType;
		}

		// Token: 0x06004F03 RID: 20227 RVA: 0x002B2B5C File Offset: 0x002B0D5C
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetItemSubType()
		{
			return CraftTool.Instance[this.TemplateId].ItemSubType;
		}

		// Token: 0x06004F04 RID: 20228 RVA: 0x002B2B84 File Offset: 0x002B0D84
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetGrade()
		{
			return CraftTool.Instance[this.TemplateId].Grade;
		}

		// Token: 0x06004F05 RID: 20229 RVA: 0x002B2BAC File Offset: 0x002B0DAC
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetIcon()
		{
			return CraftTool.Instance[this.TemplateId].Icon;
		}

		// Token: 0x06004F06 RID: 20230 RVA: 0x002B2BD4 File Offset: 0x002B0DD4
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetDesc()
		{
			return CraftTool.Instance[this.TemplateId].Desc;
		}

		// Token: 0x06004F07 RID: 20231 RVA: 0x002B2BFC File Offset: 0x002B0DFC
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetTransferable()
		{
			return CraftTool.Instance[this.TemplateId].Transferable;
		}

		// Token: 0x06004F08 RID: 20232 RVA: 0x002B2C24 File Offset: 0x002B0E24
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetStackable()
		{
			return CraftTool.Instance[this.TemplateId].Stackable;
		}

		// Token: 0x06004F09 RID: 20233 RVA: 0x002B2C4C File Offset: 0x002B0E4C
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetWagerable()
		{
			return CraftTool.Instance[this.TemplateId].Wagerable;
		}

		// Token: 0x06004F0A RID: 20234 RVA: 0x002B2C74 File Offset: 0x002B0E74
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRefinable()
		{
			return CraftTool.Instance[this.TemplateId].Refinable;
		}

		// Token: 0x06004F0B RID: 20235 RVA: 0x002B2C9C File Offset: 0x002B0E9C
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetPoisonable()
		{
			return CraftTool.Instance[this.TemplateId].Poisonable;
		}

		// Token: 0x06004F0C RID: 20236 RVA: 0x002B2CC4 File Offset: 0x002B0EC4
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRepairable()
		{
			return CraftTool.Instance[this.TemplateId].Repairable;
		}

		// Token: 0x06004F0D RID: 20237 RVA: 0x002B2CEC File Offset: 0x002B0EEC
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseWeight()
		{
			return CraftTool.Instance[this.TemplateId].BaseWeight;
		}

		// Token: 0x06004F0E RID: 20238 RVA: 0x002B2D14 File Offset: 0x002B0F14
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseValue()
		{
			return CraftTool.Instance[this.TemplateId].BaseValue;
		}

		// Token: 0x06004F0F RID: 20239 RVA: 0x002B2D3C File Offset: 0x002B0F3C
		[CollectionObjectField(true, false, false, false, false)]
		public int GetBasePrice()
		{
			return CraftTool.Instance[this.TemplateId].BasePrice;
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x002B2D64 File Offset: 0x002B0F64
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetDropRate()
		{
			return CraftTool.Instance[this.TemplateId].DropRate;
		}

		// Token: 0x06004F11 RID: 20241 RVA: 0x002B2D8C File Offset: 0x002B0F8C
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetResourceType()
		{
			return CraftTool.Instance[this.TemplateId].ResourceType;
		}

		// Token: 0x06004F12 RID: 20242 RVA: 0x002B2DB4 File Offset: 0x002B0FB4
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetPreservationDuration()
		{
			return CraftTool.Instance[this.TemplateId].PreservationDuration;
		}

		// Token: 0x06004F13 RID: 20243 RVA: 0x002B2DDC File Offset: 0x002B0FDC
		[CollectionObjectField(true, false, false, false, false)]
		public List<sbyte> GetRequiredLifeSkillTypes()
		{
			return CraftTool.Instance[this.TemplateId].RequiredLifeSkillTypes;
		}

		// Token: 0x06004F14 RID: 20244 RVA: 0x002B2E04 File Offset: 0x002B1004
		[CollectionObjectField(true, false, false, false, false)]
		public short GetAttainmentBonus()
		{
			return CraftTool.Instance[this.TemplateId].AttainmentBonus;
		}

		// Token: 0x06004F15 RID: 20245 RVA: 0x002B2E2C File Offset: 0x002B102C
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetGiftLevel()
		{
			return CraftTool.Instance[this.TemplateId].GiftLevel;
		}

		// Token: 0x06004F16 RID: 20246 RVA: 0x002B2E54 File Offset: 0x002B1054
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseFavorabilityChange()
		{
			return CraftTool.Instance[this.TemplateId].BaseFavorabilityChange;
		}

		// Token: 0x06004F17 RID: 20247 RVA: 0x002B2E7C File Offset: 0x002B107C
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetBaseHappinessChange()
		{
			return CraftTool.Instance[this.TemplateId].BaseHappinessChange;
		}

		// Token: 0x06004F18 RID: 20248 RVA: 0x002B2EA4 File Offset: 0x002B10A4
		[CollectionObjectField(true, false, false, false, false, ArrayElementsCount = 9)]
		public short[] GetDurabilityCost()
		{
			return CraftTool.Instance[this.TemplateId].DurabilityCost;
		}

		// Token: 0x06004F19 RID: 20249 RVA: 0x002B2ECC File Offset: 0x002B10CC
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetMerchantLevel()
		{
			return CraftTool.Instance[this.TemplateId].MerchantLevel;
		}

		// Token: 0x06004F1A RID: 20250 RVA: 0x002B2EF4 File Offset: 0x002B10F4
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowRandomCreate()
		{
			return CraftTool.Instance[this.TemplateId].AllowRandomCreate;
		}

		// Token: 0x06004F1B RID: 20251 RVA: 0x002B2F1C File Offset: 0x002B111C
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetInheritable()
		{
			return CraftTool.Instance[this.TemplateId].Inheritable;
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x002B2F44 File Offset: 0x002B1144
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetIsSpecial()
		{
			return CraftTool.Instance[this.TemplateId].IsSpecial;
		}

		// Token: 0x06004F1D RID: 20253 RVA: 0x002B2F6C File Offset: 0x002B116C
		[CollectionObjectField(true, false, false, false, false)]
		public short GetGroupId()
		{
			return CraftTool.Instance[this.TemplateId].GroupId;
		}

		// Token: 0x06004F1E RID: 20254 RVA: 0x002B2F94 File Offset: 0x002B1194
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetConsumedFeatureMedals()
		{
			return CraftTool.Instance[this.TemplateId].ConsumedFeatureMedals;
		}

		// Token: 0x06004F1F RID: 20255 RVA: 0x002B2FBB File Offset: 0x002B11BB
		public CraftTool()
		{
		}

		// Token: 0x06004F20 RID: 20256 RVA: 0x002B2FC8 File Offset: 0x002B11C8
		public CraftTool(short templateId)
		{
			CraftToolItem template = CraftTool.Instance[templateId];
			this.TemplateId = template.TemplateId;
			this.MaxDurability = template.MaxDurability;
		}

		// Token: 0x06004F21 RID: 20257 RVA: 0x002B3004 File Offset: 0x002B1204
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06004F22 RID: 20258 RVA: 0x002B3018 File Offset: 0x002B1218
		public int GetSerializedSize()
		{
			return 11;
		}

		// Token: 0x06004F23 RID: 20259 RVA: 0x002B3030 File Offset: 0x002B1230
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

		// Token: 0x06004F24 RID: 20260 RVA: 0x002B3088 File Offset: 0x002B1288
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

		// Token: 0x06004F25 RID: 20261 RVA: 0x002B30DF File Offset: 0x002B12DF
		public CraftTool(IRandomSource random, short templateId, int itemId) : this(templateId)
		{
			this.Id = itemId;
			this.MaxDurability = ItemBase.GenerateMaxDurability(random, this.MaxDurability);
			this.CurrDurability = this.MaxDurability;
		}

		// Token: 0x06004F26 RID: 20262 RVA: 0x002B3110 File Offset: 0x002B1310
		public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
		{
			return 0;
		}

		// Token: 0x06004F27 RID: 20263 RVA: 0x002B3124 File Offset: 0x002B1324
		public short GetRealAttainmentBonus()
		{
			short baseValue = this.GetAttainmentBonus();
			int bonus = 0;
			List<sbyte> required = this.GetRequiredLifeSkillTypes();
			bool skillIsMeet = required.Contains(7) || required.Contains(6) || required.Contains(10) || required.Contains(11);
			bool flag = DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(7) && skillIsMeet;
			if (flag)
			{
				ProfessionData professionData = DomainManager.Extra.GetProfessionData(2);
				bonus = professionData.GetSeniorityAttainmentBonus();
			}
			return (short)((100 + bonus) * (int)baseValue / 100);
		}

		// Token: 0x04001592 RID: 5522
		public const int FixedSize = 11;

		// Token: 0x04001593 RID: 5523
		public const int DynamicCount = 0;

		// Token: 0x02000AA6 RID: 2726
		internal class FixedFieldInfos
		{
			// Token: 0x04002BDB RID: 11227
			public const uint Id_Offset = 0U;

			// Token: 0x04002BDC RID: 11228
			public const int Id_Size = 4;

			// Token: 0x04002BDD RID: 11229
			public const uint TemplateId_Offset = 4U;

			// Token: 0x04002BDE RID: 11230
			public const int TemplateId_Size = 2;

			// Token: 0x04002BDF RID: 11231
			public const uint MaxDurability_Offset = 6U;

			// Token: 0x04002BE0 RID: 11232
			public const int MaxDurability_Size = 2;

			// Token: 0x04002BE1 RID: 11233
			public const uint CurrDurability_Offset = 8U;

			// Token: 0x04002BE2 RID: 11234
			public const int CurrDurability_Size = 2;

			// Token: 0x04002BE3 RID: 11235
			public const uint ModificationState_Offset = 10U;

			// Token: 0x04002BE4 RID: 11236
			public const int ModificationState_Size = 1;
		}
	}
}
