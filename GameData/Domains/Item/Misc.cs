using System;
using System.Collections.Generic;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Domains.Extra;
using GameData.Serializer;
using Redzen.Random;

namespace GameData.Domains.Item
{
	// Token: 0x02000670 RID: 1648
	[SerializableGameData(NotForDisplayModule = true)]
	public class Misc : ItemBase, ISerializableGameData
	{
		// Token: 0x06005207 RID: 20999 RVA: 0x002CCC00 File Offset: 0x002CAE00
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

		// Token: 0x06005208 RID: 21000 RVA: 0x002CCC60 File Offset: 0x002CAE60
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

		// Token: 0x06005209 RID: 21001 RVA: 0x002CCCC0 File Offset: 0x002CAEC0
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

		// Token: 0x0600520A RID: 21002 RVA: 0x002CCD20 File Offset: 0x002CAF20
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetName()
		{
			return Misc.Instance[this.TemplateId].Name;
		}

		// Token: 0x0600520B RID: 21003 RVA: 0x002CCD48 File Offset: 0x002CAF48
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetItemType()
		{
			return Misc.Instance[this.TemplateId].ItemType;
		}

		// Token: 0x0600520C RID: 21004 RVA: 0x002CCD70 File Offset: 0x002CAF70
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetItemSubType()
		{
			return Misc.Instance[this.TemplateId].ItemSubType;
		}

		// Token: 0x0600520D RID: 21005 RVA: 0x002CCD98 File Offset: 0x002CAF98
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetGrade()
		{
			return Misc.Instance[this.TemplateId].Grade;
		}

		// Token: 0x0600520E RID: 21006 RVA: 0x002CCDC0 File Offset: 0x002CAFC0
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetIcon()
		{
			return Misc.Instance[this.TemplateId].Icon;
		}

		// Token: 0x0600520F RID: 21007 RVA: 0x002CCDE8 File Offset: 0x002CAFE8
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetDesc()
		{
			return Misc.Instance[this.TemplateId].Desc;
		}

		// Token: 0x06005210 RID: 21008 RVA: 0x002CCE10 File Offset: 0x002CB010
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetTransferable()
		{
			return Misc.Instance[this.TemplateId].Transferable;
		}

		// Token: 0x06005211 RID: 21009 RVA: 0x002CCE38 File Offset: 0x002CB038
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetStackable()
		{
			return Misc.Instance[this.TemplateId].Stackable;
		}

		// Token: 0x06005212 RID: 21010 RVA: 0x002CCE60 File Offset: 0x002CB060
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetWagerable()
		{
			return Misc.Instance[this.TemplateId].Wagerable;
		}

		// Token: 0x06005213 RID: 21011 RVA: 0x002CCE88 File Offset: 0x002CB088
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRefinable()
		{
			return Misc.Instance[this.TemplateId].Refinable;
		}

		// Token: 0x06005214 RID: 21012 RVA: 0x002CCEB0 File Offset: 0x002CB0B0
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetPoisonable()
		{
			return Misc.Instance[this.TemplateId].Poisonable;
		}

		// Token: 0x06005215 RID: 21013 RVA: 0x002CCED8 File Offset: 0x002CB0D8
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRepairable()
		{
			return Misc.Instance[this.TemplateId].Repairable;
		}

		// Token: 0x06005216 RID: 21014 RVA: 0x002CCF00 File Offset: 0x002CB100
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseWeight()
		{
			return Misc.Instance[this.TemplateId].BaseWeight;
		}

		// Token: 0x06005217 RID: 21015 RVA: 0x002CCF28 File Offset: 0x002CB128
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseValue()
		{
			return Misc.Instance[this.TemplateId].BaseValue;
		}

		// Token: 0x06005218 RID: 21016 RVA: 0x002CCF50 File Offset: 0x002CB150
		[CollectionObjectField(true, false, false, false, false)]
		public int GetBasePrice()
		{
			return Misc.Instance[this.TemplateId].BasePrice;
		}

		// Token: 0x06005219 RID: 21017 RVA: 0x002CCF78 File Offset: 0x002CB178
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetDropRate()
		{
			return Misc.Instance[this.TemplateId].DropRate;
		}

		// Token: 0x0600521A RID: 21018 RVA: 0x002CCFA0 File Offset: 0x002CB1A0
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetResourceType()
		{
			return Misc.Instance[this.TemplateId].ResourceType;
		}

		// Token: 0x0600521B RID: 21019 RVA: 0x002CCFC8 File Offset: 0x002CB1C8
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetPreservationDuration()
		{
			return Misc.Instance[this.TemplateId].PreservationDuration;
		}

		// Token: 0x0600521C RID: 21020 RVA: 0x002CCFF0 File Offset: 0x002CB1F0
		[CollectionObjectField(true, false, false, false, false)]
		public short GetNeili()
		{
			return Misc.Instance[this.TemplateId].Neili;
		}

		// Token: 0x0600521D RID: 21021 RVA: 0x002CD018 File Offset: 0x002CB218
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetCricketHealInjuryOdds()
		{
			return Misc.Instance[this.TemplateId].CricketHealInjuryOdds;
		}

		// Token: 0x0600521E RID: 21022 RVA: 0x002CD040 File Offset: 0x002CB240
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetConsumedFeatureMedals()
		{
			return Misc.Instance[this.TemplateId].ConsumedFeatureMedals;
		}

		// Token: 0x0600521F RID: 21023 RVA: 0x002CD068 File Offset: 0x002CB268
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetMaxUseDistance()
		{
			return Misc.Instance[this.TemplateId].MaxUseDistance;
		}

		// Token: 0x06005220 RID: 21024 RVA: 0x002CD090 File Offset: 0x002CB290
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetBaseHappinessChange()
		{
			return Misc.Instance[this.TemplateId].BaseHappinessChange;
		}

		// Token: 0x06005221 RID: 21025 RVA: 0x002CD0B8 File Offset: 0x002CB2B8
		[CollectionObjectField(true, false, false, false, false)]
		public short GetMakeItemSubType()
		{
			return Misc.Instance[this.TemplateId].MakeItemSubType;
		}

		// Token: 0x06005222 RID: 21026 RVA: 0x002CD0E0 File Offset: 0x002CB2E0
		[CollectionObjectField(true, false, false, false, false)]
		public List<TreasureStateInfo> GetStateBuryAmount()
		{
			return Misc.Instance[this.TemplateId].StateBuryAmount;
		}

		// Token: 0x06005223 RID: 21027 RVA: 0x002CD108 File Offset: 0x002CB308
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetConsumable()
		{
			return Misc.Instance[this.TemplateId].Consumable;
		}

		// Token: 0x06005224 RID: 21028 RVA: 0x002CD130 File Offset: 0x002CB330
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseFavorabilityChange()
		{
			return Misc.Instance[this.TemplateId].BaseFavorabilityChange;
		}

		// Token: 0x06005225 RID: 21029 RVA: 0x002CD158 File Offset: 0x002CB358
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetGiftLevel()
		{
			return Misc.Instance[this.TemplateId].GiftLevel;
		}

		// Token: 0x06005226 RID: 21030 RVA: 0x002CD180 File Offset: 0x002CB380
		[CollectionObjectField(true, false, false, false, false)]
		public List<short> GetRequireCombatConfig()
		{
			return Misc.Instance[this.TemplateId].RequireCombatConfig;
		}

		// Token: 0x06005227 RID: 21031 RVA: 0x002CD1A8 File Offset: 0x002CB3A8
		[CollectionObjectField(true, false, false, false, false)]
		public short GetGroupId()
		{
			return Misc.Instance[this.TemplateId].GroupId;
		}

		// Token: 0x06005228 RID: 21032 RVA: 0x002CD1D0 File Offset: 0x002CB3D0
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetIsSpecial()
		{
			return Misc.Instance[this.TemplateId].IsSpecial;
		}

		// Token: 0x06005229 RID: 21033 RVA: 0x002CD1F8 File Offset: 0x002CB3F8
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowRandomCreate()
		{
			return Misc.Instance[this.TemplateId].AllowRandomCreate;
		}

		// Token: 0x0600522A RID: 21034 RVA: 0x002CD220 File Offset: 0x002CB420
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetInheritable()
		{
			return Misc.Instance[this.TemplateId].Inheritable;
		}

		// Token: 0x0600522B RID: 21035 RVA: 0x002CD248 File Offset: 0x002CB448
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetBreakBonusEffect()
		{
			return Misc.Instance[this.TemplateId].BreakBonusEffect;
		}

		// Token: 0x0600522C RID: 21036 RVA: 0x002CD270 File Offset: 0x002CB470
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetMerchantLevel()
		{
			return Misc.Instance[this.TemplateId].MerchantLevel;
		}

		// Token: 0x0600522D RID: 21037 RVA: 0x002CD298 File Offset: 0x002CB498
		[CollectionObjectField(true, false, false, false, false)]
		public List<int> GetAllowBrokenLevels()
		{
			return Misc.Instance[this.TemplateId].AllowBrokenLevels;
		}

		// Token: 0x0600522E RID: 21038 RVA: 0x002CD2C0 File Offset: 0x002CB4C0
		[CollectionObjectField(true, false, false, false, false)]
		public EMiscGenerateType GetGenerateType()
		{
			return Misc.Instance[this.TemplateId].GenerateType;
		}

		// Token: 0x0600522F RID: 21039 RVA: 0x002CD2E8 File Offset: 0x002CB4E8
		[CollectionObjectField(true, false, false, false, false)]
		public EMiscFilterType GetFilterType()
		{
			return Misc.Instance[this.TemplateId].FilterType;
		}

		// Token: 0x06005230 RID: 21040 RVA: 0x002CD310 File Offset: 0x002CB510
		[CollectionObjectField(true, false, false, false, false)]
		public short GetReduceEscapeRate()
		{
			return Misc.Instance[this.TemplateId].ReduceEscapeRate;
		}

		// Token: 0x06005231 RID: 21041 RVA: 0x002CD338 File Offset: 0x002CB538
		[CollectionObjectField(true, false, false, false, false)]
		public short GetCombatUseEffect()
		{
			return Misc.Instance[this.TemplateId].CombatUseEffect;
		}

		// Token: 0x06005232 RID: 21042 RVA: 0x002CD360 File Offset: 0x002CB560
		[CollectionObjectField(true, false, false, false, false)]
		public short GetCombatPrepareUseEffect()
		{
			return Misc.Instance[this.TemplateId].CombatPrepareUseEffect;
		}

		// Token: 0x06005233 RID: 21043 RVA: 0x002CD388 File Offset: 0x002CB588
		[CollectionObjectField(true, false, false, false, false)]
		public int GetGainExp()
		{
			return Misc.Instance[this.TemplateId].GainExp;
		}

		// Token: 0x06005234 RID: 21044 RVA: 0x002CD3AF File Offset: 0x002CB5AF
		public Misc()
		{
		}

		// Token: 0x06005235 RID: 21045 RVA: 0x002CD3BC File Offset: 0x002CB5BC
		public Misc(short templateId)
		{
			MiscItem template = Misc.Instance[templateId];
			this.TemplateId = template.TemplateId;
			this.MaxDurability = template.MaxDurability;
		}

		// Token: 0x06005236 RID: 21046 RVA: 0x002CD3F8 File Offset: 0x002CB5F8
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06005237 RID: 21047 RVA: 0x002CD40C File Offset: 0x002CB60C
		public int GetSerializedSize()
		{
			return 11;
		}

		// Token: 0x06005238 RID: 21048 RVA: 0x002CD424 File Offset: 0x002CB624
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

		// Token: 0x06005239 RID: 21049 RVA: 0x002CD47C File Offset: 0x002CB67C
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

		// Token: 0x0600523A RID: 21050 RVA: 0x002CD4D3 File Offset: 0x002CB6D3
		public Misc(IRandomSource random, short templateId, int itemId) : this(templateId)
		{
			this.Id = itemId;
			this.MaxDurability = ItemBase.GenerateMaxDurability(random, this.MaxDurability);
			this.CurrDurability = this.MaxDurability;
		}

		// Token: 0x0600523B RID: 21051 RVA: 0x002CD504 File Offset: 0x002CB704
		public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
		{
			return 0;
		}

		// Token: 0x0400160E RID: 5646
		public const int FixedSize = 11;

		// Token: 0x0400160F RID: 5647
		public const int DynamicCount = 0;

		// Token: 0x02000AC3 RID: 2755
		internal class FixedFieldInfos
		{
			// Token: 0x04002CA2 RID: 11426
			public const uint Id_Offset = 0U;

			// Token: 0x04002CA3 RID: 11427
			public const int Id_Size = 4;

			// Token: 0x04002CA4 RID: 11428
			public const uint TemplateId_Offset = 4U;

			// Token: 0x04002CA5 RID: 11429
			public const int TemplateId_Size = 2;

			// Token: 0x04002CA6 RID: 11430
			public const uint MaxDurability_Offset = 6U;

			// Token: 0x04002CA7 RID: 11431
			public const int MaxDurability_Size = 2;

			// Token: 0x04002CA8 RID: 11432
			public const uint CurrDurability_Offset = 8U;

			// Token: 0x04002CA9 RID: 11433
			public const int CurrDurability_Size = 2;

			// Token: 0x04002CAA RID: 11434
			public const uint ModificationState_Offset = 10U;

			// Token: 0x04002CAB RID: 11435
			public const int ModificationState_Size = 1;
		}
	}
}
