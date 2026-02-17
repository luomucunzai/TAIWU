using System;
using System.Collections.Generic;
using Config;
using Config.ConfigCells.Character;
using GameData.ArchiveData;
using GameData.Common;
using GameData.DLC.FiveLoong;
using GameData.Serializer;
using Redzen.Random;

namespace GameData.Domains.Item
{
	// Token: 0x0200066E RID: 1646
	[SerializableGameData(NotForDisplayModule = true)]
	public class Material : ItemBase, ISerializableGameData
	{
		// Token: 0x0600514A RID: 20810 RVA: 0x002CAC90 File Offset: 0x002C8E90
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

		// Token: 0x0600514B RID: 20811 RVA: 0x002CACF0 File Offset: 0x002C8EF0
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

		// Token: 0x0600514C RID: 20812 RVA: 0x002CAD50 File Offset: 0x002C8F50
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

		// Token: 0x0600514D RID: 20813 RVA: 0x002CADB0 File Offset: 0x002C8FB0
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetName()
		{
			return Material.Instance[this.TemplateId].Name;
		}

		// Token: 0x0600514E RID: 20814 RVA: 0x002CADD8 File Offset: 0x002C8FD8
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetItemType()
		{
			return Material.Instance[this.TemplateId].ItemType;
		}

		// Token: 0x0600514F RID: 20815 RVA: 0x002CAE00 File Offset: 0x002C9000
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetItemSubType()
		{
			return Material.Instance[this.TemplateId].ItemSubType;
		}

		// Token: 0x06005150 RID: 20816 RVA: 0x002CAE28 File Offset: 0x002C9028
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetGrade()
		{
			return Material.Instance[this.TemplateId].Grade;
		}

		// Token: 0x06005151 RID: 20817 RVA: 0x002CAE50 File Offset: 0x002C9050
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetIcon()
		{
			return Material.Instance[this.TemplateId].Icon;
		}

		// Token: 0x06005152 RID: 20818 RVA: 0x002CAE78 File Offset: 0x002C9078
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetDesc()
		{
			return Material.Instance[this.TemplateId].Desc;
		}

		// Token: 0x06005153 RID: 20819 RVA: 0x002CAEA0 File Offset: 0x002C90A0
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetTransferable()
		{
			return Material.Instance[this.TemplateId].Transferable;
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x002CAEC8 File Offset: 0x002C90C8
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetStackable()
		{
			return Material.Instance[this.TemplateId].Stackable;
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x002CAEF0 File Offset: 0x002C90F0
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetWagerable()
		{
			return Material.Instance[this.TemplateId].Wagerable;
		}

		// Token: 0x06005156 RID: 20822 RVA: 0x002CAF18 File Offset: 0x002C9118
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRefinable()
		{
			return Material.Instance[this.TemplateId].Refinable;
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x002CAF40 File Offset: 0x002C9140
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetPoisonable()
		{
			return Material.Instance[this.TemplateId].Poisonable;
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x002CAF68 File Offset: 0x002C9168
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRepairable()
		{
			return Material.Instance[this.TemplateId].Repairable;
		}

		// Token: 0x06005159 RID: 20825 RVA: 0x002CAF90 File Offset: 0x002C9190
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseWeight()
		{
			return Material.Instance[this.TemplateId].BaseWeight;
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x002CAFB8 File Offset: 0x002C91B8
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseValue()
		{
			return Material.Instance[this.TemplateId].BaseValue;
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x002CAFE0 File Offset: 0x002C91E0
		[CollectionObjectField(true, false, false, false, false)]
		public int GetBasePrice()
		{
			return Material.Instance[this.TemplateId].BasePrice;
		}

		// Token: 0x0600515C RID: 20828 RVA: 0x002CB008 File Offset: 0x002C9208
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetDropRate()
		{
			return Material.Instance[this.TemplateId].DropRate;
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x002CB030 File Offset: 0x002C9230
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetResourceType()
		{
			return Material.Instance[this.TemplateId].ResourceType;
		}

		// Token: 0x0600515E RID: 20830 RVA: 0x002CB058 File Offset: 0x002C9258
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetPreservationDuration()
		{
			return Material.Instance[this.TemplateId].PreservationDuration;
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x002CB080 File Offset: 0x002C9280
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetRefiningEffect()
		{
			return Material.Instance[this.TemplateId].RefiningEffect;
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x002CB0A8 File Offset: 0x002C92A8
		[CollectionObjectField(true, false, false, false, false)]
		public short GetResourceAmount()
		{
			return Material.Instance[this.TemplateId].ResourceAmount;
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x002CB0D0 File Offset: 0x002C92D0
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetRequiredLifeSkillType()
		{
			return Material.Instance[this.TemplateId].RequiredLifeSkillType;
		}

		// Token: 0x06005162 RID: 20834 RVA: 0x002CB0F8 File Offset: 0x002C92F8
		[CollectionObjectField(true, false, false, false, false)]
		public short GetRequiredAttainment()
		{
			return Material.Instance[this.TemplateId].RequiredAttainment;
		}

		// Token: 0x06005163 RID: 20835 RVA: 0x002CB120 File Offset: 0x002C9320
		[CollectionObjectField(true, false, false, false, false)]
		public short GetRequiredResourceAmount()
		{
			return Material.Instance[this.TemplateId].RequiredResourceAmount;
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x002CB148 File Offset: 0x002C9348
		[CollectionObjectField(true, false, false, false, false)]
		public List<short> GetCraftableItemTypes()
		{
			return Material.Instance[this.TemplateId].CraftableItemTypes;
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x002CB170 File Offset: 0x002C9370
		[CollectionObjectField(true, false, false, false, false)]
		public ref readonly PoisonsAndLevels GetInnatePoisons()
		{
			return ref Material.Instance[this.TemplateId].InnatePoisons;
		}

		// Token: 0x06005166 RID: 20838 RVA: 0x002CB198 File Offset: 0x002C9398
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetBaseHappinessChange()
		{
			return Material.Instance[this.TemplateId].BaseHappinessChange;
		}

		// Token: 0x06005167 RID: 20839 RVA: 0x002CB1C0 File Offset: 0x002C93C0
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetGiftLevel()
		{
			return Material.Instance[this.TemplateId].GiftLevel;
		}

		// Token: 0x06005168 RID: 20840 RVA: 0x002CB1E8 File Offset: 0x002C93E8
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseFavorabilityChange()
		{
			return Material.Instance[this.TemplateId].BaseFavorabilityChange;
		}

		// Token: 0x06005169 RID: 20841 RVA: 0x002CB210 File Offset: 0x002C9410
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateResistOfInner()
		{
			return Material.Instance[this.TemplateId].PenetrateResistOfInner;
		}

		// Token: 0x0600516A RID: 20842 RVA: 0x002CB238 File Offset: 0x002C9438
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateMind()
		{
			return Material.Instance[this.TemplateId].AvoidRateMind;
		}

		// Token: 0x0600516B RID: 20843 RVA: 0x002CB260 File Offset: 0x002C9460
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateResistOfOuter()
		{
			return Material.Instance[this.TemplateId].PenetrateResistOfOuter;
		}

		// Token: 0x0600516C RID: 20844 RVA: 0x002CB288 File Offset: 0x002C9488
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateSpeed()
		{
			return Material.Instance[this.TemplateId].AvoidRateSpeed;
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x002CB2B0 File Offset: 0x002C94B0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfBreath()
		{
			return Material.Instance[this.TemplateId].RecoveryOfBreath;
		}

		// Token: 0x0600516E RID: 20846 RVA: 0x002CB2D8 File Offset: 0x002C94D8
		[CollectionObjectField(true, false, false, false, true)]
		public short GetMoveSpeed()
		{
			return Material.Instance[this.TemplateId].MoveSpeed;
		}

		// Token: 0x0600516F RID: 20847 RVA: 0x002CB300 File Offset: 0x002C9500
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfFlaw()
		{
			return Material.Instance[this.TemplateId].RecoveryOfFlaw;
		}

		// Token: 0x06005170 RID: 20848 RVA: 0x002CB328 File Offset: 0x002C9528
		[CollectionObjectField(true, false, false, false, true)]
		public short GetCastSpeed()
		{
			return Material.Instance[this.TemplateId].CastSpeed;
		}

		// Token: 0x06005171 RID: 20849 RVA: 0x002CB350 File Offset: 0x002C9550
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfBlockedAcupoint()
		{
			return Material.Instance[this.TemplateId].RecoveryOfBlockedAcupoint;
		}

		// Token: 0x06005172 RID: 20850 RVA: 0x002CB378 File Offset: 0x002C9578
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfStance()
		{
			return Material.Instance[this.TemplateId].RecoveryOfStance;
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x002CB3A0 File Offset: 0x002C95A0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateTechnique()
		{
			return Material.Instance[this.TemplateId].AvoidRateTechnique;
		}

		// Token: 0x06005174 RID: 20852 RVA: 0x002CB3C8 File Offset: 0x002C95C8
		[CollectionObjectField(true, false, false, false, false)]
		public short GetGroupId()
		{
			return Material.Instance[this.TemplateId].GroupId;
		}

		// Token: 0x06005175 RID: 20853 RVA: 0x002CB3F0 File Offset: 0x002C95F0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAttackSpeed()
		{
			return Material.Instance[this.TemplateId].AttackSpeed;
		}

		// Token: 0x06005176 RID: 20854 RVA: 0x002CB418 File Offset: 0x002C9618
		[CollectionObjectField(true, false, false, false, true)]
		public short GetInnerRatio()
		{
			return Material.Instance[this.TemplateId].InnerRatio;
		}

		// Token: 0x06005177 RID: 20855 RVA: 0x002CB440 File Offset: 0x002C9640
		[CollectionObjectField(true, false, false, false, true)]
		public short GetRecoveryOfQiDisorder()
		{
			return Material.Instance[this.TemplateId].RecoveryOfQiDisorder;
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x002CB468 File Offset: 0x002C9668
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfHotPoison()
		{
			return Material.Instance[this.TemplateId].ResistOfHotPoison;
		}

		// Token: 0x06005179 RID: 20857 RVA: 0x002CB490 File Offset: 0x002C9690
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfGloomyPoison()
		{
			return Material.Instance[this.TemplateId].ResistOfGloomyPoison;
		}

		// Token: 0x0600517A RID: 20858 RVA: 0x002CB4B8 File Offset: 0x002C96B8
		[CollectionObjectField(true, false, false, false, true)]
		public short GetAvoidRateStrength()
		{
			return Material.Instance[this.TemplateId].AvoidRateStrength;
		}

		// Token: 0x0600517B RID: 20859 RVA: 0x002CB4E0 File Offset: 0x002C96E0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfColdPoison()
		{
			return Material.Instance[this.TemplateId].ResistOfColdPoison;
		}

		// Token: 0x0600517C RID: 20860 RVA: 0x002CB508 File Offset: 0x002C9708
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfRedPoison()
		{
			return Material.Instance[this.TemplateId].ResistOfRedPoison;
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x002CB530 File Offset: 0x002C9730
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfRottenPoison()
		{
			return Material.Instance[this.TemplateId].ResistOfRottenPoison;
		}

		// Token: 0x0600517E RID: 20862 RVA: 0x002CB558 File Offset: 0x002C9758
		[CollectionObjectField(true, false, false, false, true)]
		public short GetResistOfIllusoryPoison()
		{
			return Material.Instance[this.TemplateId].ResistOfIllusoryPoison;
		}

		// Token: 0x0600517F RID: 20863 RVA: 0x002CB580 File Offset: 0x002C9780
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetConsumedFeatureMedals()
		{
			return Material.Instance[this.TemplateId].ConsumedFeatureMedals;
		}

		// Token: 0x06005180 RID: 20864 RVA: 0x002CB5A8 File Offset: 0x002C97A8
		[CollectionObjectField(true, false, false, false, true)]
		public short GetWeaponSwitchSpeed()
		{
			return Material.Instance[this.TemplateId].WeaponSwitchSpeed;
		}

		// Token: 0x06005181 RID: 20865 RVA: 0x002CB5D0 File Offset: 0x002C97D0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateOfInner()
		{
			return Material.Instance[this.TemplateId].PenetrateOfInner;
		}

		// Token: 0x06005182 RID: 20866 RVA: 0x002CB5F8 File Offset: 0x002C97F8
		[CollectionObjectField(true, false, false, false, false)]
		public short GetPrimaryEffectValue()
		{
			return Material.Instance[this.TemplateId].PrimaryEffectValue;
		}

		// Token: 0x06005183 RID: 20867 RVA: 0x002CB620 File Offset: 0x002C9820
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateMind()
		{
			return Material.Instance[this.TemplateId].HitRateMind;
		}

		// Token: 0x06005184 RID: 20868 RVA: 0x002CB648 File Offset: 0x002C9848
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetInheritable()
		{
			return Material.Instance[this.TemplateId].Inheritable;
		}

		// Token: 0x06005185 RID: 20869 RVA: 0x002CB670 File Offset: 0x002C9870
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetMerchantLevel()
		{
			return Material.Instance[this.TemplateId].MerchantLevel;
		}

		// Token: 0x06005186 RID: 20870 RVA: 0x002CB698 File Offset: 0x002C9898
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowRandomCreate()
		{
			return Material.Instance[this.TemplateId].AllowRandomCreate;
		}

		// Token: 0x06005187 RID: 20871 RVA: 0x002CB6C0 File Offset: 0x002C98C0
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetIsSpecial()
		{
			return Material.Instance[this.TemplateId].IsSpecial;
		}

		// Token: 0x06005188 RID: 20872 RVA: 0x002CB6E8 File Offset: 0x002C98E8
		[CollectionObjectField(true, false, false, false, false)]
		public EMaterialProperty GetProperty()
		{
			return Material.Instance[this.TemplateId].Property;
		}

		// Token: 0x06005189 RID: 20873 RVA: 0x002CB710 File Offset: 0x002C9910
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetBreakBonusEffect()
		{
			return Material.Instance[this.TemplateId].BreakBonusEffect;
		}

		// Token: 0x0600518A RID: 20874 RVA: 0x002CB738 File Offset: 0x002C9938
		[CollectionObjectField(true, false, false, false, false)]
		public List<PresetInventoryItem> GetDisassembleResultItemList()
		{
			return Material.Instance[this.TemplateId].DisassembleResultItemList;
		}

		// Token: 0x0600518B RID: 20875 RVA: 0x002CB760 File Offset: 0x002C9960
		[CollectionObjectField(true, false, false, false, false)]
		public short GetDisassembleResultCount()
		{
			return Material.Instance[this.TemplateId].DisassembleResultCount;
		}

		// Token: 0x0600518C RID: 20876 RVA: 0x002CB788 File Offset: 0x002C9988
		[CollectionObjectField(true, false, false, false, false)]
		public short GetDuration()
		{
			return Material.Instance[this.TemplateId].Duration;
		}

		// Token: 0x0600518D RID: 20877 RVA: 0x002CB7B0 File Offset: 0x002C99B0
		[CollectionObjectField(true, false, false, false, false)]
		public short GetBaseMaxHealthDelta()
		{
			return Material.Instance[this.TemplateId].BaseMaxHealthDelta;
		}

		// Token: 0x0600518E RID: 20878 RVA: 0x002CB7D8 File Offset: 0x002C99D8
		[CollectionObjectField(true, false, false, false, false)]
		public EMedicineEffectType GetPrimaryEffectType()
		{
			return Material.Instance[this.TemplateId].PrimaryEffectType;
		}

		// Token: 0x0600518F RID: 20879 RVA: 0x002CB800 File Offset: 0x002C9A00
		[CollectionObjectField(true, false, false, false, false)]
		public short GetPrimaryEffectThresholdValue()
		{
			return Material.Instance[this.TemplateId].PrimaryEffectThresholdValue;
		}

		// Token: 0x06005190 RID: 20880 RVA: 0x002CB828 File Offset: 0x002C9A28
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetPrimaryInjuryRecoveryTimes()
		{
			return Material.Instance[this.TemplateId].PrimaryInjuryRecoveryTimes;
		}

		// Token: 0x06005191 RID: 20881 RVA: 0x002CB850 File Offset: 0x002C9A50
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetPrimaryRecoverAllInjuries()
		{
			return Material.Instance[this.TemplateId].PrimaryRecoverAllInjuries;
		}

		// Token: 0x06005192 RID: 20882 RVA: 0x002CB878 File Offset: 0x002C9A78
		[CollectionObjectField(true, false, false, false, false)]
		public EMedicineEffectType GetSecondaryEffectType()
		{
			return Material.Instance[this.TemplateId].SecondaryEffectType;
		}

		// Token: 0x06005193 RID: 20883 RVA: 0x002CB8A0 File Offset: 0x002C9AA0
		[CollectionObjectField(true, false, false, false, false)]
		public EMedicineEffectSubType GetSecondaryEffectSubType()
		{
			return Material.Instance[this.TemplateId].SecondaryEffectSubType;
		}

		// Token: 0x06005194 RID: 20884 RVA: 0x002CB8C8 File Offset: 0x002C9AC8
		[CollectionObjectField(true, false, false, false, false)]
		public short GetSecondaryEffectThresholdValue()
		{
			return Material.Instance[this.TemplateId].SecondaryEffectThresholdValue;
		}

		// Token: 0x06005195 RID: 20885 RVA: 0x002CB8F0 File Offset: 0x002C9AF0
		[CollectionObjectField(true, false, false, false, false)]
		public short GetSecondaryEffectValue()
		{
			return Material.Instance[this.TemplateId].SecondaryEffectValue;
		}

		// Token: 0x06005196 RID: 20886 RVA: 0x002CB918 File Offset: 0x002C9B18
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetSecondaryInjuryRecoveryTimes()
		{
			return Material.Instance[this.TemplateId].SecondaryInjuryRecoveryTimes;
		}

		// Token: 0x06005197 RID: 20887 RVA: 0x002CB940 File Offset: 0x002C9B40
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetSecondaryRecoverAllInjuries()
		{
			return Material.Instance[this.TemplateId].SecondaryRecoverAllInjuries;
		}

		// Token: 0x06005198 RID: 20888 RVA: 0x002CB968 File Offset: 0x002C9B68
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateStrength()
		{
			return Material.Instance[this.TemplateId].HitRateStrength;
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x002CB990 File Offset: 0x002C9B90
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateTechnique()
		{
			return Material.Instance[this.TemplateId].HitRateTechnique;
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x002CB9B8 File Offset: 0x002C9BB8
		[CollectionObjectField(true, false, false, false, true)]
		public short GetHitRateSpeed()
		{
			return Material.Instance[this.TemplateId].HitRateSpeed;
		}

		// Token: 0x0600519B RID: 20891 RVA: 0x002CB9E0 File Offset: 0x002C9BE0
		[CollectionObjectField(true, false, false, false, true)]
		public short GetPenetrateOfOuter()
		{
			return Material.Instance[this.TemplateId].PenetrateOfOuter;
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x002CBA08 File Offset: 0x002C9C08
		[CollectionObjectField(true, false, false, false, false)]
		public EMedicineEffectSubType GetPrimaryEffectSubType()
		{
			return Material.Instance[this.TemplateId].PrimaryEffectSubType;
		}

		// Token: 0x0600519D RID: 20893 RVA: 0x002CBA30 File Offset: 0x002C9C30
		[CollectionObjectField(true, false, false, false, false)]
		public EMaterialFilterType GetFilterType()
		{
			return Material.Instance[this.TemplateId].FilterType;
		}

		// Token: 0x0600519E RID: 20894 RVA: 0x002CBA58 File Offset: 0x002C9C58
		[CollectionObjectField(true, false, false, false, false)]
		public EMaterialFilterHardness GetFilterHardness()
		{
			return Material.Instance[this.TemplateId].FilterHardness;
		}

		// Token: 0x0600519F RID: 20895 RVA: 0x002CBA7F File Offset: 0x002C9C7F
		public Material()
		{
		}

		// Token: 0x060051A0 RID: 20896 RVA: 0x002CBA8C File Offset: 0x002C9C8C
		public Material(short templateId)
		{
			MaterialItem template = Material.Instance[templateId];
			this.TemplateId = template.TemplateId;
			this.MaxDurability = template.MaxDurability;
		}

		// Token: 0x060051A1 RID: 20897 RVA: 0x002CBAC8 File Offset: 0x002C9CC8
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x060051A2 RID: 20898 RVA: 0x002CBADC File Offset: 0x002C9CDC
		public int GetSerializedSize()
		{
			return 11;
		}

		// Token: 0x060051A3 RID: 20899 RVA: 0x002CBAF4 File Offset: 0x002C9CF4
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

		// Token: 0x060051A4 RID: 20900 RVA: 0x002CBB4C File Offset: 0x002C9D4C
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

		// Token: 0x060051A5 RID: 20901 RVA: 0x002CBBA3 File Offset: 0x002C9DA3
		public Material(IRandomSource random, short templateId, int itemId) : this(templateId)
		{
			this.Id = itemId;
			this.MaxDurability = ItemBase.GenerateMaxDurability(random, this.MaxDurability);
			this.CurrDurability = this.MaxDurability;
		}

		// Token: 0x060051A6 RID: 20902 RVA: 0x002CBBD4 File Offset: 0x002C9DD4
		public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
		{
			return Material.GetCharacterPropertyBonus(this.TemplateId, type);
		}

		// Token: 0x060051A7 RID: 20903 RVA: 0x002CBBF4 File Offset: 0x002C9DF4
		public static int GetCharacterPropertyBonus(short templateId, ECharacterPropertyReferencedType type)
		{
			MaterialItem config = Material.Instance[templateId];
			return config.GetCharacterPropertyBonusInt(type);
		}

		// Token: 0x060051A8 RID: 20904 RVA: 0x002CBC1C File Offset: 0x002C9E1C
		public override int GetValue()
		{
			GameData.DLC.FiveLoong.Jiao jiao;
			return DomainManager.Extra.TryGetJiaoByItemKey(base.GetItemKey(), out jiao) ? jiao.Properties.Get(jiao.TemplateId, 6) : Material.Instance[this.TemplateId].BaseValue;
		}

		// Token: 0x060051A9 RID: 20905 RVA: 0x002CBC6C File Offset: 0x002C9E6C
		public override sbyte GetHappinessChange()
		{
			GameData.DLC.FiveLoong.Jiao jiao;
			return DomainManager.Extra.TryGetJiaoByItemKey(base.GetItemKey(), out jiao) ? ((sbyte)jiao.Properties.Get(jiao.TemplateId, 7)) : Material.Instance[this.TemplateId].BaseHappinessChange;
		}

		// Token: 0x060051AA RID: 20906 RVA: 0x002CBCBC File Offset: 0x002C9EBC
		public override int GetFavorabilityChange()
		{
			GameData.DLC.FiveLoong.Jiao jiao;
			return DomainManager.Extra.TryGetJiaoByItemKey(base.GetItemKey(), out jiao) ? jiao.Properties.Get(jiao.TemplateId, 8) : Material.Instance[this.TemplateId].BaseFavorabilityChange;
		}

		// Token: 0x0400160A RID: 5642
		public const int FixedSize = 11;

		// Token: 0x0400160B RID: 5643
		public const int DynamicCount = 0;

		// Token: 0x02000AC1 RID: 2753
		internal class FixedFieldInfos
		{
			// Token: 0x04002C8E RID: 11406
			public const uint Id_Offset = 0U;

			// Token: 0x04002C8F RID: 11407
			public const int Id_Size = 4;

			// Token: 0x04002C90 RID: 11408
			public const uint TemplateId_Offset = 4U;

			// Token: 0x04002C91 RID: 11409
			public const int TemplateId_Size = 2;

			// Token: 0x04002C92 RID: 11410
			public const uint MaxDurability_Offset = 6U;

			// Token: 0x04002C93 RID: 11411
			public const int MaxDurability_Size = 2;

			// Token: 0x04002C94 RID: 11412
			public const uint CurrDurability_Offset = 8U;

			// Token: 0x04002C95 RID: 11413
			public const int CurrDurability_Size = 2;

			// Token: 0x04002C96 RID: 11414
			public const uint ModificationState_Offset = 10U;

			// Token: 0x04002C97 RID: 11415
			public const int ModificationState_Size = 1;
		}
	}
}
