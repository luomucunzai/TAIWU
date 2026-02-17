using System;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Serializer;
using Redzen.Random;

namespace GameData.Domains.Item
{
	// Token: 0x02000672 RID: 1650
	[SerializableGameData(NotForDisplayModule = true)]
	public class TeaWine : ItemBase, ISerializableGameData
	{
		// Token: 0x06005277 RID: 21111 RVA: 0x002CE47B File Offset: 0x002CC67B
		public TeaWine(IRandomSource random, short templateId, int itemId) : this(templateId)
		{
			this.Id = itemId;
			this.MaxDurability = ItemBase.GenerateMaxDurability(random, this.MaxDurability);
			this.CurrDurability = this.MaxDurability;
		}

		// Token: 0x06005278 RID: 21112 RVA: 0x002CE4AC File Offset: 0x002CC6AC
		public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
		{
			return TeaWine.GetCharacterPropertyBonus(this.TemplateId, type);
		}

		// Token: 0x06005279 RID: 21113 RVA: 0x002CE4CC File Offset: 0x002CC6CC
		public static int GetCharacterPropertyBonus(short templateId, ECharacterPropertyReferencedType type)
		{
			TeaWineItem config = TeaWine.Instance[templateId];
			return config.GetCharacterPropertyBonusInt(type);
		}

		// Token: 0x0600527A RID: 21114 RVA: 0x002CE4F4 File Offset: 0x002CC6F4
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

		// Token: 0x0600527B RID: 21115 RVA: 0x002CE554 File Offset: 0x002CC754
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

		// Token: 0x0600527C RID: 21116 RVA: 0x002CE5B4 File Offset: 0x002CC7B4
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

		// Token: 0x0600527D RID: 21117 RVA: 0x002CE614 File Offset: 0x002CC814
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetName()
		{
			return TeaWine.Instance[this.TemplateId].Name;
		}

		// Token: 0x0600527E RID: 21118 RVA: 0x002CE63C File Offset: 0x002CC83C
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetItemType()
		{
			return TeaWine.Instance[this.TemplateId].ItemType;
		}

		// Token: 0x0600527F RID: 21119 RVA: 0x002CE664 File Offset: 0x002CC864
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetItemSubType()
		{
			return TeaWine.Instance[this.TemplateId].ItemSubType;
		}

		// Token: 0x06005280 RID: 21120 RVA: 0x002CE68C File Offset: 0x002CC88C
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetGrade()
		{
			return TeaWine.Instance[this.TemplateId].Grade;
		}

		// Token: 0x06005281 RID: 21121 RVA: 0x002CE6B4 File Offset: 0x002CC8B4
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetIcon()
		{
			return TeaWine.Instance[this.TemplateId].Icon;
		}

		// Token: 0x06005282 RID: 21122 RVA: 0x002CE6DC File Offset: 0x002CC8DC
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetDesc()
		{
			return TeaWine.Instance[this.TemplateId].Desc;
		}

		// Token: 0x06005283 RID: 21123 RVA: 0x002CE704 File Offset: 0x002CC904
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetTransferable()
		{
			return TeaWine.Instance[this.TemplateId].Transferable;
		}

		// Token: 0x06005284 RID: 21124 RVA: 0x002CE72C File Offset: 0x002CC92C
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetStackable()
		{
			return TeaWine.Instance[this.TemplateId].Stackable;
		}

		// Token: 0x06005285 RID: 21125 RVA: 0x002CE754 File Offset: 0x002CC954
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetWagerable()
		{
			return TeaWine.Instance[this.TemplateId].Wagerable;
		}

		// Token: 0x06005286 RID: 21126 RVA: 0x002CE77C File Offset: 0x002CC97C
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRefinable()
		{
			return TeaWine.Instance[this.TemplateId].Refinable;
		}

		// Token: 0x06005287 RID: 21127 RVA: 0x002CE7A4 File Offset: 0x002CC9A4
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetPoisonable()
		{
			return TeaWine.Instance[this.TemplateId].Poisonable;
		}

		// Token: 0x06005288 RID: 21128 RVA: 0x002CE7CC File Offset: 0x002CC9CC
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRepairable()
		{
			return TeaWine.Instance[this.TemplateId].Repairable;
		}

		// Token: 0x06005289 RID: 21129 RVA: 0x002CE7F4 File Offset: 0x002CC9F4
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseWeight()
		{
			return TeaWine.Instance[this.TemplateId].BaseWeight;
		}

		// Token: 0x0600528A RID: 21130 RVA: 0x002CE81C File Offset: 0x002CCA1C
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseValue()
		{
			return TeaWine.Instance[this.TemplateId].BaseValue;
		}

		// Token: 0x0600528B RID: 21131 RVA: 0x002CE844 File Offset: 0x002CCA44
		[CollectionObjectField(true, false, false, false, false)]
		public int GetBasePrice()
		{
			return TeaWine.Instance[this.TemplateId].BasePrice;
		}

		// Token: 0x0600528C RID: 21132 RVA: 0x002CE86C File Offset: 0x002CCA6C
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetDropRate()
		{
			return TeaWine.Instance[this.TemplateId].DropRate;
		}

		// Token: 0x0600528D RID: 21133 RVA: 0x002CE894 File Offset: 0x002CCA94
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetResourceType()
		{
			return TeaWine.Instance[this.TemplateId].ResourceType;
		}

		// Token: 0x0600528E RID: 21134 RVA: 0x002CE8BC File Offset: 0x002CCABC
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetPreservationDuration()
		{
			return TeaWine.Instance[this.TemplateId].PreservationDuration;
		}

		// Token: 0x0600528F RID: 21135 RVA: 0x002CE8E4 File Offset: 0x002CCAE4
		[CollectionObjectField(true, false, false, false, false)]
		public short GetDuration()
		{
			return TeaWine.Instance[this.TemplateId].Duration;
		}

		// Token: 0x06005290 RID: 21136 RVA: 0x002CE90C File Offset: 0x002CCB0C
		[CollectionObjectField(true, false, false, false, false)]
		public short GetDirectChangeOfQiDisorder()
		{
			return TeaWine.Instance[this.TemplateId].DirectChangeOfQiDisorder;
		}

		// Token: 0x06005291 RID: 21137 RVA: 0x002CE934 File Offset: 0x002CCB34
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetConsumedFeatureMedals()
		{
			return TeaWine.Instance[this.TemplateId].ConsumedFeatureMedals;
		}

		// Token: 0x06005292 RID: 21138 RVA: 0x002CE95C File Offset: 0x002CCB5C
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateStrength()
		{
			return TeaWine.Instance[this.TemplateId].HitRateStrength;
		}

		// Token: 0x06005293 RID: 21139 RVA: 0x002CE984 File Offset: 0x002CCB84
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateTechnique()
		{
			return TeaWine.Instance[this.TemplateId].HitRateTechnique;
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x002CE9AC File Offset: 0x002CCBAC
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateSpeed()
		{
			return TeaWine.Instance[this.TemplateId].HitRateSpeed;
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x002CE9D4 File Offset: 0x002CCBD4
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateMind()
		{
			return TeaWine.Instance[this.TemplateId].HitRateMind;
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x002CE9FC File Offset: 0x002CCBFC
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateOfOuter()
		{
			return TeaWine.Instance[this.TemplateId].PenetrateOfOuter;
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x002CEA24 File Offset: 0x002CCC24
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateOfInner()
		{
			return TeaWine.Instance[this.TemplateId].PenetrateOfInner;
		}

		// Token: 0x06005298 RID: 21144 RVA: 0x002CEA4C File Offset: 0x002CCC4C
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateStrength()
		{
			return TeaWine.Instance[this.TemplateId].AvoidRateStrength;
		}

		// Token: 0x06005299 RID: 21145 RVA: 0x002CEA74 File Offset: 0x002CCC74
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateTechnique()
		{
			return TeaWine.Instance[this.TemplateId].AvoidRateTechnique;
		}

		// Token: 0x0600529A RID: 21146 RVA: 0x002CEA9C File Offset: 0x002CCC9C
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateSpeed()
		{
			return TeaWine.Instance[this.TemplateId].AvoidRateSpeed;
		}

		// Token: 0x0600529B RID: 21147 RVA: 0x002CEAC4 File Offset: 0x002CCCC4
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateMind()
		{
			return TeaWine.Instance[this.TemplateId].AvoidRateMind;
		}

		// Token: 0x0600529C RID: 21148 RVA: 0x002CEAEC File Offset: 0x002CCCEC
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateResistOfOuter()
		{
			return TeaWine.Instance[this.TemplateId].PenetrateResistOfOuter;
		}

		// Token: 0x0600529D RID: 21149 RVA: 0x002CEB14 File Offset: 0x002CCD14
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateResistOfInner()
		{
			return TeaWine.Instance[this.TemplateId].PenetrateResistOfInner;
		}

		// Token: 0x0600529E RID: 21150 RVA: 0x002CEB3C File Offset: 0x002CCD3C
		[CollectionObjectField(true, false, false, false, true)]
		public short GetInnerRatio()
		{
			return TeaWine.Instance[this.TemplateId].InnerRatio;
		}

		// Token: 0x0600529F RID: 21151 RVA: 0x002CEB64 File Offset: 0x002CCD64
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfQiDisorder()
		{
			return TeaWine.Instance[this.TemplateId].RecoveryOfQiDisorder;
		}

		// Token: 0x060052A0 RID: 21152 RVA: 0x002CEB8C File Offset: 0x002CCD8C
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseFavorabilityChange()
		{
			return TeaWine.Instance[this.TemplateId].BaseFavorabilityChange;
		}

		// Token: 0x060052A1 RID: 21153 RVA: 0x002CEBB4 File Offset: 0x002CCDB4
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetBaseHappinessChange()
		{
			return TeaWine.Instance[this.TemplateId].BaseHappinessChange;
		}

		// Token: 0x060052A2 RID: 21154 RVA: 0x002CEBDC File Offset: 0x002CCDDC
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetGiftLevel()
		{
			return TeaWine.Instance[this.TemplateId].GiftLevel;
		}

		// Token: 0x060052A3 RID: 21155 RVA: 0x002CEC04 File Offset: 0x002CCE04
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetSolarTermType()
		{
			return TeaWine.Instance[this.TemplateId].SolarTermType;
		}

		// Token: 0x060052A4 RID: 21156 RVA: 0x002CEC2C File Offset: 0x002CCE2C
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetEatHappinessChange()
		{
			return TeaWine.Instance[this.TemplateId].EatHappinessChange;
		}

		// Token: 0x060052A5 RID: 21157 RVA: 0x002CEC54 File Offset: 0x002CCE54
		[CollectionObjectField(true, false, false, false, false)]
		public short GetGroupId()
		{
			return TeaWine.Instance[this.TemplateId].GroupId;
		}

		// Token: 0x060052A6 RID: 21158 RVA: 0x002CEC7C File Offset: 0x002CCE7C
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetBreakBonusEffect()
		{
			return TeaWine.Instance[this.TemplateId].BreakBonusEffect;
		}

		// Token: 0x060052A7 RID: 21159 RVA: 0x002CECA4 File Offset: 0x002CCEA4
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowRandomCreate()
		{
			return TeaWine.Instance[this.TemplateId].AllowRandomCreate;
		}

		// Token: 0x060052A8 RID: 21160 RVA: 0x002CECCC File Offset: 0x002CCECC
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetMerchantLevel()
		{
			return TeaWine.Instance[this.TemplateId].MerchantLevel;
		}

		// Token: 0x060052A9 RID: 21161 RVA: 0x002CECF4 File Offset: 0x002CCEF4
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetInheritable()
		{
			return TeaWine.Instance[this.TemplateId].Inheritable;
		}

		// Token: 0x060052AA RID: 21162 RVA: 0x002CED1C File Offset: 0x002CCF1C
		[CollectionObjectField(true, false, false, false, false)]
		public int GetActionPointRecover()
		{
			return TeaWine.Instance[this.TemplateId].ActionPointRecover;
		}

		// Token: 0x060052AB RID: 21163 RVA: 0x002CED44 File Offset: 0x002CCF44
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetIsSpecial()
		{
			return TeaWine.Instance[this.TemplateId].IsSpecial;
		}

		// Token: 0x060052AC RID: 21164 RVA: 0x002CED6C File Offset: 0x002CCF6C
		[CollectionObjectField(true, false, false, false, false)]
		public string GetBigIcon()
		{
			return TeaWine.Instance[this.TemplateId].BigIcon;
		}

		// Token: 0x060052AD RID: 21165 RVA: 0x002CED93 File Offset: 0x002CCF93
		public TeaWine()
		{
		}

		// Token: 0x060052AE RID: 21166 RVA: 0x002CEDA0 File Offset: 0x002CCFA0
		public TeaWine(short templateId)
		{
			TeaWineItem template = TeaWine.Instance[templateId];
			this.TemplateId = template.TemplateId;
			this.MaxDurability = template.MaxDurability;
		}

		// Token: 0x060052AF RID: 21167 RVA: 0x002CEDDC File Offset: 0x002CCFDC
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x060052B0 RID: 21168 RVA: 0x002CEDF0 File Offset: 0x002CCFF0
		public int GetSerializedSize()
		{
			return 11;
		}

		// Token: 0x060052B1 RID: 21169 RVA: 0x002CEE08 File Offset: 0x002CD008
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

		// Token: 0x060052B2 RID: 21170 RVA: 0x002CEE60 File Offset: 0x002CD060
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

		// Token: 0x04001614 RID: 5652
		public const int FixedSize = 11;

		// Token: 0x04001615 RID: 5653
		public const int DynamicCount = 0;

		// Token: 0x02000AC5 RID: 2757
		internal class FixedFieldInfos
		{
			// Token: 0x04002CBA RID: 11450
			public const uint Id_Offset = 0U;

			// Token: 0x04002CBB RID: 11451
			public const int Id_Size = 4;

			// Token: 0x04002CBC RID: 11452
			public const uint TemplateId_Offset = 4U;

			// Token: 0x04002CBD RID: 11453
			public const int TemplateId_Size = 2;

			// Token: 0x04002CBE RID: 11454
			public const uint MaxDurability_Offset = 6U;

			// Token: 0x04002CBF RID: 11455
			public const int MaxDurability_Size = 2;

			// Token: 0x04002CC0 RID: 11456
			public const uint CurrDurability_Offset = 8U;

			// Token: 0x04002CC1 RID: 11457
			public const int CurrDurability_Size = 2;

			// Token: 0x04002CC2 RID: 11458
			public const uint ModificationState_Offset = 10U;

			// Token: 0x04002CC3 RID: 11459
			public const int ModificationState_Size = 1;
		}
	}
}
