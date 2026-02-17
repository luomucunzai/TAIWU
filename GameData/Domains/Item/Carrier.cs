using System;
using System.Collections.Generic;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.DLC.FiveLoong;
using GameData.Domains.Map;
using GameData.Serializer;
using Redzen.Random;

namespace GameData.Domains.Item
{
	// Token: 0x0200065F RID: 1631
	[SerializableGameData(NotForDisplayModule = true)]
	public class Carrier : EquipmentBase, ISerializableGameData, IExploreBonusRateItem
	{
		// Token: 0x06004E84 RID: 20100 RVA: 0x002B0D34 File Offset: 0x002AEF34
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

		// Token: 0x06004E85 RID: 20101 RVA: 0x002B0D94 File Offset: 0x002AEF94
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

		// Token: 0x06004E86 RID: 20102 RVA: 0x002B0DF4 File Offset: 0x002AEFF4
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

		// Token: 0x06004E87 RID: 20103 RVA: 0x002B0E54 File Offset: 0x002AF054
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

		// Token: 0x06004E88 RID: 20104 RVA: 0x002B0EB4 File Offset: 0x002AF0B4
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

		// Token: 0x06004E89 RID: 20105 RVA: 0x002B0F14 File Offset: 0x002AF114
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

		// Token: 0x06004E8A RID: 20106 RVA: 0x002B0F78 File Offset: 0x002AF178
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetName()
		{
			return Carrier.Instance[this.TemplateId].Name;
		}

		// Token: 0x06004E8B RID: 20107 RVA: 0x002B0FA0 File Offset: 0x002AF1A0
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetItemType()
		{
			return Carrier.Instance[this.TemplateId].ItemType;
		}

		// Token: 0x06004E8C RID: 20108 RVA: 0x002B0FC8 File Offset: 0x002AF1C8
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetItemSubType()
		{
			return Carrier.Instance[this.TemplateId].ItemSubType;
		}

		// Token: 0x06004E8D RID: 20109 RVA: 0x002B0FF0 File Offset: 0x002AF1F0
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetGrade()
		{
			return Carrier.Instance[this.TemplateId].Grade;
		}

		// Token: 0x06004E8E RID: 20110 RVA: 0x002B1018 File Offset: 0x002AF218
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetIcon()
		{
			return Carrier.Instance[this.TemplateId].Icon;
		}

		// Token: 0x06004E8F RID: 20111 RVA: 0x002B1040 File Offset: 0x002AF240
		[CollectionObjectField(true, false, false, false, false)]
		public override string GetDesc()
		{
			return Carrier.Instance[this.TemplateId].Desc;
		}

		// Token: 0x06004E90 RID: 20112 RVA: 0x002B1068 File Offset: 0x002AF268
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetTransferable()
		{
			return Carrier.Instance[this.TemplateId].Transferable;
		}

		// Token: 0x06004E91 RID: 20113 RVA: 0x002B1090 File Offset: 0x002AF290
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetStackable()
		{
			return Carrier.Instance[this.TemplateId].Stackable;
		}

		// Token: 0x06004E92 RID: 20114 RVA: 0x002B10B8 File Offset: 0x002AF2B8
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetWagerable()
		{
			return Carrier.Instance[this.TemplateId].Wagerable;
		}

		// Token: 0x06004E93 RID: 20115 RVA: 0x002B10E0 File Offset: 0x002AF2E0
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRefinable()
		{
			return Carrier.Instance[this.TemplateId].Refinable;
		}

		// Token: 0x06004E94 RID: 20116 RVA: 0x002B1108 File Offset: 0x002AF308
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetPoisonable()
		{
			return Carrier.Instance[this.TemplateId].Poisonable;
		}

		// Token: 0x06004E95 RID: 20117 RVA: 0x002B1130 File Offset: 0x002AF330
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetRepairable()
		{
			return Carrier.Instance[this.TemplateId].Repairable;
		}

		// Token: 0x06004E96 RID: 20118 RVA: 0x002B1158 File Offset: 0x002AF358
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseWeight()
		{
			return Carrier.Instance[this.TemplateId].BaseWeight;
		}

		// Token: 0x06004E97 RID: 20119 RVA: 0x002B1180 File Offset: 0x002AF380
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseValue()
		{
			return Carrier.Instance[this.TemplateId].BaseValue;
		}

		// Token: 0x06004E98 RID: 20120 RVA: 0x002B11A8 File Offset: 0x002AF3A8
		[CollectionObjectField(true, false, false, false, false)]
		public int GetBasePrice()
		{
			return Carrier.Instance[this.TemplateId].BasePrice;
		}

		// Token: 0x06004E99 RID: 20121 RVA: 0x002B11D0 File Offset: 0x002AF3D0
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetDropRate()
		{
			return Carrier.Instance[this.TemplateId].DropRate;
		}

		// Token: 0x06004E9A RID: 20122 RVA: 0x002B11F8 File Offset: 0x002AF3F8
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetResourceType()
		{
			return Carrier.Instance[this.TemplateId].ResourceType;
		}

		// Token: 0x06004E9B RID: 20123 RVA: 0x002B1220 File Offset: 0x002AF420
		[CollectionObjectField(true, false, false, false, false)]
		public override short GetPreservationDuration()
		{
			return Carrier.Instance[this.TemplateId].PreservationDuration;
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x002B1248 File Offset: 0x002AF448
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetEquipmentType()
		{
			return Carrier.Instance[this.TemplateId].EquipmentType;
		}

		// Token: 0x06004E9D RID: 20125 RVA: 0x002B1270 File Offset: 0x002AF470
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetIsFlying()
		{
			return Carrier.Instance[this.TemplateId].IsFlying;
		}

		// Token: 0x06004E9E RID: 20126 RVA: 0x002B1298 File Offset: 0x002AF498
		[CollectionObjectField(true, false, false, false, false)]
		public short GetMakeItemSubType()
		{
			return Carrier.Instance[this.TemplateId].MakeItemSubType;
		}

		// Token: 0x06004E9F RID: 20127 RVA: 0x002B12C0 File Offset: 0x002AF4C0
		[CollectionObjectField(true, false, false, false, false)]
		public override sbyte GetBaseHappinessChange()
		{
			return Carrier.Instance[this.TemplateId].BaseHappinessChange;
		}

		// Token: 0x06004EA0 RID: 20128 RVA: 0x002B12E8 File Offset: 0x002AF4E8
		[CollectionObjectField(true, false, false, false, false)]
		public override int GetBaseFavorabilityChange()
		{
			return Carrier.Instance[this.TemplateId].BaseFavorabilityChange;
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x002B1310 File Offset: 0x002AF510
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetGiftLevel()
		{
			return Carrier.Instance[this.TemplateId].GiftLevel;
		}

		// Token: 0x06004EA2 RID: 20130 RVA: 0x002B1338 File Offset: 0x002AF538
		[CollectionObjectField(true, false, false, false, false)]
		public List<short> GetHateFoodType()
		{
			return Carrier.Instance[this.TemplateId].HateFoodType;
		}

		// Token: 0x06004EA3 RID: 20131 RVA: 0x002B1360 File Offset: 0x002AF560
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetTamePoint()
		{
			return Carrier.Instance[this.TemplateId].TamePoint;
		}

		// Token: 0x06004EA4 RID: 20132 RVA: 0x002B1388 File Offset: 0x002AF588
		[CollectionObjectField(true, false, false, false, false)]
		public short GetCombatState()
		{
			return Carrier.Instance[this.TemplateId].CombatState;
		}

		// Token: 0x06004EA5 RID: 20133 RVA: 0x002B13B0 File Offset: 0x002AF5B0
		[CollectionObjectField(true, false, false, false, false)]
		public short GetBaseCaptureRateBonus()
		{
			return Carrier.Instance[this.TemplateId].BaseCaptureRateBonus;
		}

		// Token: 0x06004EA6 RID: 20134 RVA: 0x002B13D8 File Offset: 0x002AF5D8
		[CollectionObjectField(true, false, false, false, false)]
		public short GetBaseMaxKidnapSlotCountBonus()
		{
			return Carrier.Instance[this.TemplateId].BaseMaxKidnapSlotCountBonus;
		}

		// Token: 0x06004EA7 RID: 20135 RVA: 0x002B1400 File Offset: 0x002AF600
		[CollectionObjectField(true, false, false, false, false)]
		public short GetBaseMaxInventoryLoadBonus()
		{
			return Carrier.Instance[this.TemplateId].BaseMaxInventoryLoadBonus;
		}

		// Token: 0x06004EA8 RID: 20136 RVA: 0x002B1428 File Offset: 0x002AF628
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetBaseTravelTimeReduction()
		{
			return Carrier.Instance[this.TemplateId].BaseTravelTimeReduction;
		}

		// Token: 0x06004EA9 RID: 20137 RVA: 0x002B1450 File Offset: 0x002AF650
		[CollectionObjectField(true, false, false, false, false)]
		public List<short> GetLoveFoodType()
		{
			return Carrier.Instance[this.TemplateId].LoveFoodType;
		}

		// Token: 0x06004EAA RID: 20138 RVA: 0x002B1478 File Offset: 0x002AF678
		[CollectionObjectField(true, false, false, false, false)]
		public short GetBaseDropRateBonus()
		{
			return Carrier.Instance[this.TemplateId].BaseDropRateBonus;
		}

		// Token: 0x06004EAB RID: 20139 RVA: 0x002B14A0 File Offset: 0x002AF6A0
		[CollectionObjectField(true, false, false, false, false, ArrayElementsCount = 7)]
		public sbyte[] GetTamePersonalities()
		{
			return Carrier.Instance[this.TemplateId].TamePersonalities;
		}

		// Token: 0x06004EAC RID: 20140 RVA: 0x002B14C8 File Offset: 0x002AF6C8
		[CollectionObjectField(true, false, false, false, false)]
		public override bool GetDetachable()
		{
			return Carrier.Instance[this.TemplateId].Detachable;
		}

		// Token: 0x06004EAD RID: 20141 RVA: 0x002B14F0 File Offset: 0x002AF6F0
		[CollectionObjectField(true, false, false, false, false)]
		public short GetCharacterIdInCombat()
		{
			return Carrier.Instance[this.TemplateId].CharacterIdInCombat;
		}

		// Token: 0x06004EAE RID: 20142 RVA: 0x002B1518 File Offset: 0x002AF718
		[CollectionObjectField(true, false, false, false, false)]
		public sbyte GetMerchantLevel()
		{
			return Carrier.Instance[this.TemplateId].MerchantLevel;
		}

		// Token: 0x06004EAF RID: 20143 RVA: 0x002B1540 File Offset: 0x002AF740
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetAllowRandomCreate()
		{
			return Carrier.Instance[this.TemplateId].AllowRandomCreate;
		}

		// Token: 0x06004EB0 RID: 20144 RVA: 0x002B1568 File Offset: 0x002AF768
		[CollectionObjectField(true, false, false, false, false)]
		public short GetTravelSkeleton()
		{
			return Carrier.Instance[this.TemplateId].TravelSkeleton;
		}

		// Token: 0x06004EB1 RID: 20145 RVA: 0x002B1590 File Offset: 0x002AF790
		[CollectionObjectField(true, false, false, false, false)]
		public string GetStandDisplay()
		{
			return Carrier.Instance[this.TemplateId].StandDisplay;
		}

		// Token: 0x06004EB2 RID: 20146 RVA: 0x002B15B8 File Offset: 0x002AF7B8
		[CollectionObjectField(true, false, false, false, false)]
		public short GetGroupId()
		{
			return Carrier.Instance[this.TemplateId].GroupId;
		}

		// Token: 0x06004EB3 RID: 20147 RVA: 0x002B15E0 File Offset: 0x002AF7E0
		[CollectionObjectField(true, false, false, false, false)]
		public ref readonly PoisonsAndLevels GetInnatePoisons()
		{
			return ref Carrier.Instance[this.TemplateId].InnatePoisons;
		}

		// Token: 0x06004EB4 RID: 20148 RVA: 0x002B1608 File Offset: 0x002AF808
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetIsSpecial()
		{
			return Carrier.Instance[this.TemplateId].IsSpecial;
		}

		// Token: 0x06004EB5 RID: 20149 RVA: 0x002B1630 File Offset: 0x002AF830
		[CollectionObjectField(true, false, false, false, false)]
		public bool GetInheritable()
		{
			return Carrier.Instance[this.TemplateId].Inheritable;
		}

		// Token: 0x06004EB6 RID: 20150 RVA: 0x002B1658 File Offset: 0x002AF858
		[CollectionObjectField(true, false, false, false, false)]
		public short GetEquipmentCombatPowerValueFactor()
		{
			return Carrier.Instance[this.TemplateId].EquipmentCombatPowerValueFactor;
		}

		// Token: 0x06004EB7 RID: 20151 RVA: 0x002B1680 File Offset: 0x002AF880
		[CollectionObjectField(true, false, false, false, false)]
		public short GetBaseExploreBonusRate()
		{
			return Carrier.Instance[this.TemplateId].BaseExploreBonusRate;
		}

		// Token: 0x06004EB8 RID: 20152 RVA: 0x002B16A7 File Offset: 0x002AF8A7
		public Carrier()
		{
		}

		// Token: 0x06004EB9 RID: 20153 RVA: 0x002B16B4 File Offset: 0x002AF8B4
		public Carrier(short templateId)
		{
			CarrierItem template = Carrier.Instance[templateId];
			this.TemplateId = template.TemplateId;
			this.MaxDurability = template.MaxDurability;
			this.EquipmentEffectId = template.EquipmentEffectId;
		}

		// Token: 0x06004EBA RID: 20154 RVA: 0x002B16FC File Offset: 0x002AF8FC
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06004EBB RID: 20155 RVA: 0x002B1710 File Offset: 0x002AF910
		public int GetSerializedSize()
		{
			return 29;
		}

		// Token: 0x06004EBC RID: 20156 RVA: 0x002B1728 File Offset: 0x002AF928
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

		// Token: 0x06004EBD RID: 20157 RVA: 0x002B17A8 File Offset: 0x002AF9A8
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

		// Token: 0x06004EBE RID: 20158 RVA: 0x002B1826 File Offset: 0x002AFA26
		public Carrier(IRandomSource random, short templateId, int itemId) : this(templateId)
		{
			this.Id = itemId;
			this.MaxDurability = ItemBase.GenerateMaxDurability(random, this.MaxDurability);
			this.CurrDurability = this.MaxDurability;
		}

		// Token: 0x06004EBF RID: 20159 RVA: 0x002B1858 File Offset: 0x002AFA58
		public override int GetCharacterPropertyBonus(ECharacterPropertyReferencedType type)
		{
			bool flag = type >= ECharacterPropertyReferencedType.PersonalityCalm && type <= ECharacterPropertyReferencedType.PersonalityPerceptive && DomainManager.Extra.IsCarrierFullTamePoint(base.GetItemKey()) && !base.IsDurabilityRunningOut();
			int result;
			if (flag)
			{
				int personalityIndex = type - ECharacterPropertyReferencedType.PersonalityCalm;
				result = (int)this.GetTamePersonalities()[personalityIndex];
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06004EC0 RID: 20160 RVA: 0x002B18A8 File Offset: 0x002AFAA8
		public sbyte GetTravelTimeReduction()
		{
			bool flag = base.IsDurabilityRunningOut();
			sbyte result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				sbyte res = Carrier.Instance[this.TemplateId].BaseTravelTimeReduction;
				int id;
				GameData.DLC.FiveLoong.Jiao jiao;
				bool flag2 = DomainManager.Extra.TryGetJiaoIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetJiao(id, out jiao);
				if (flag2)
				{
					res += (sbyte)jiao.Properties.Get(jiao.TemplateId, 0);
				}
				else
				{
					ChildrenOfLoong loong;
					bool flag3 = DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out loong);
					if (flag3)
					{
						res += (sbyte)loong.Properties.Get(loong.JiaoTemplateId, loong.LoongTemplateId, 0);
					}
				}
				result = res;
			}
			return result;
		}

		// Token: 0x06004EC1 RID: 20161 RVA: 0x002B1974 File Offset: 0x002AFB74
		public short GetMaxInventoryLoadBonus()
		{
			bool flag = base.IsDurabilityRunningOut();
			short result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				short res = Carrier.Instance[this.TemplateId].BaseMaxInventoryLoadBonus;
				int id;
				GameData.DLC.FiveLoong.Jiao jiao;
				bool flag2 = DomainManager.Extra.TryGetJiaoIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetJiao(id, out jiao);
				if (flag2)
				{
					res += (short)jiao.Properties.Get(jiao.TemplateId, 1);
				}
				else
				{
					ChildrenOfLoong loong;
					bool flag3 = DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out loong);
					if (flag3)
					{
						res += (short)loong.Properties.Get(loong.JiaoTemplateId, loong.LoongTemplateId, 1);
					}
				}
				result = res;
			}
			return result;
		}

		// Token: 0x06004EC2 RID: 20162 RVA: 0x002B1A40 File Offset: 0x002AFC40
		public short GetDropRateBonus()
		{
			bool flag = base.IsDurabilityRunningOut();
			short result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				short res = Carrier.Instance[this.TemplateId].BaseDropRateBonus;
				int id;
				GameData.DLC.FiveLoong.Jiao jiao;
				bool flag2 = DomainManager.Extra.TryGetJiaoIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetJiao(id, out jiao);
				if (flag2)
				{
					res += (short)jiao.Properties.Get(jiao.TemplateId, 2);
				}
				else
				{
					ChildrenOfLoong loong;
					bool flag3 = DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out loong);
					if (flag3)
					{
						res += (short)loong.Properties.Get(loong.JiaoTemplateId, loong.LoongTemplateId, 2);
					}
				}
				result = res;
			}
			return result;
		}

		// Token: 0x06004EC3 RID: 20163 RVA: 0x002B1B0C File Offset: 0x002AFD0C
		public short GetCaptureRateBonus()
		{
			bool flag = base.IsDurabilityRunningOut();
			short result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				short res = Carrier.Instance[this.TemplateId].BaseCaptureRateBonus;
				int id;
				GameData.DLC.FiveLoong.Jiao jiao;
				bool flag2 = DomainManager.Extra.TryGetJiaoIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetJiao(id, out jiao);
				if (flag2)
				{
					res += (short)jiao.Properties.Get(jiao.TemplateId, 3);
				}
				else
				{
					ChildrenOfLoong loong;
					bool flag3 = DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out loong);
					if (flag3)
					{
						res += (short)loong.Properties.Get(loong.JiaoTemplateId, loong.LoongTemplateId, 3);
					}
				}
				result = res;
			}
			return result;
		}

		// Token: 0x06004EC4 RID: 20164 RVA: 0x002B1BD8 File Offset: 0x002AFDD8
		public int GetExploreBonusRate()
		{
			bool flag = base.IsDurabilityRunningOut();
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int res = (int)this.GetBaseExploreBonusRate();
				int id;
				GameData.DLC.FiveLoong.Jiao jiao;
				bool flag2 = DomainManager.Extra.TryGetJiaoIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetJiao(id, out jiao);
				if (flag2)
				{
					res += jiao.Properties.Get(jiao.TemplateId, 5);
				}
				else
				{
					ChildrenOfLoong loong;
					bool flag3 = DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out loong);
					if (flag3)
					{
						res += loong.Properties.Get(loong.JiaoTemplateId, loong.LoongTemplateId, 5);
					}
				}
				result = res;
			}
			return result;
		}

		// Token: 0x06004EC5 RID: 20165 RVA: 0x002B1C90 File Offset: 0x002AFE90
		public short GetMaxKidnapSlotCountBonus()
		{
			bool flag = base.IsDurabilityRunningOut();
			short result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				short res = Carrier.Instance[this.TemplateId].BaseMaxKidnapSlotCountBonus;
				int id;
				GameData.DLC.FiveLoong.Jiao jiao;
				bool flag2 = DomainManager.Extra.TryGetJiaoIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetJiao(id, out jiao);
				if (flag2)
				{
					res += (short)jiao.Properties.Get(jiao.TemplateId, 4);
				}
				else
				{
					ChildrenOfLoong loong;
					bool flag3 = DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out loong);
					if (flag3)
					{
						res += (short)loong.Properties.Get(loong.JiaoTemplateId, loong.LoongTemplateId, 4);
					}
				}
				result = res;
			}
			return result;
		}

		// Token: 0x06004EC6 RID: 20166 RVA: 0x002B1D5C File Offset: 0x002AFF5C
		public override int GetValue()
		{
			int res = Carrier.Instance[this.TemplateId].BaseValue;
			int id;
			GameData.DLC.FiveLoong.Jiao jiao;
			bool flag = DomainManager.Extra.TryGetJiaoIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetJiao(id, out jiao);
			if (flag)
			{
				res += jiao.Properties.Get(jiao.TemplateId, 6);
			}
			else
			{
				ChildrenOfLoong loong;
				bool flag2 = DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out loong);
				if (flag2)
				{
					res += loong.Properties.Get(loong.JiaoTemplateId, loong.LoongTemplateId, 6);
				}
			}
			return base.ApplyDurabilityEffect(res);
		}

		// Token: 0x06004EC7 RID: 20167 RVA: 0x002B1E18 File Offset: 0x002B0018
		public override sbyte GetHappinessChange()
		{
			sbyte res = Carrier.Instance[this.TemplateId].BaseHappinessChange;
			int id;
			GameData.DLC.FiveLoong.Jiao jiao;
			bool flag = DomainManager.Extra.TryGetJiaoIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetJiao(id, out jiao);
			if (flag)
			{
				res += (sbyte)jiao.Properties.Get(jiao.TemplateId, 7);
			}
			else
			{
				ChildrenOfLoong loong;
				bool flag2 = DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out loong);
				if (flag2)
				{
					res += (sbyte)loong.Properties.Get(loong.JiaoTemplateId, loong.LoongTemplateId, 7);
				}
			}
			return res;
		}

		// Token: 0x06004EC8 RID: 20168 RVA: 0x002B1ED0 File Offset: 0x002B00D0
		public override int GetFavorabilityChange()
		{
			int res = Carrier.Instance[this.TemplateId].BaseFavorabilityChange;
			int id;
			GameData.DLC.FiveLoong.Jiao jiao;
			bool flag = DomainManager.Extra.TryGetJiaoIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetJiao(id, out jiao);
			if (flag)
			{
				res += jiao.Properties.Get(jiao.TemplateId, 8);
			}
			else
			{
				ChildrenOfLoong loong;
				bool flag2 = DomainManager.Extra.TryGetChildrenOfLoongIdByItemKey(base.GetItemKey(), out id) && DomainManager.Extra.TryGetLoong(id, out loong);
				if (flag2)
				{
					res += loong.Properties.Get(loong.JiaoTemplateId, loong.LoongTemplateId, 8);
				}
			}
			return res;
		}

		// Token: 0x0400158D RID: 5517
		public const int FixedSize = 29;

		// Token: 0x0400158E RID: 5518
		public const int DynamicCount = 0;

		// Token: 0x02000AA4 RID: 2724
		internal class FixedFieldInfos
		{
			// Token: 0x04002BB9 RID: 11193
			public const uint Id_Offset = 0U;

			// Token: 0x04002BBA RID: 11194
			public const int Id_Size = 4;

			// Token: 0x04002BBB RID: 11195
			public const uint TemplateId_Offset = 4U;

			// Token: 0x04002BBC RID: 11196
			public const int TemplateId_Size = 2;

			// Token: 0x04002BBD RID: 11197
			public const uint MaxDurability_Offset = 6U;

			// Token: 0x04002BBE RID: 11198
			public const int MaxDurability_Size = 2;

			// Token: 0x04002BBF RID: 11199
			public const uint EquipmentEffectId_Offset = 8U;

			// Token: 0x04002BC0 RID: 11200
			public const int EquipmentEffectId_Size = 2;

			// Token: 0x04002BC1 RID: 11201
			public const uint CurrDurability_Offset = 10U;

			// Token: 0x04002BC2 RID: 11202
			public const int CurrDurability_Size = 2;

			// Token: 0x04002BC3 RID: 11203
			public const uint ModificationState_Offset = 12U;

			// Token: 0x04002BC4 RID: 11204
			public const int ModificationState_Size = 1;

			// Token: 0x04002BC5 RID: 11205
			public const uint EquippedCharId_Offset = 13U;

			// Token: 0x04002BC6 RID: 11206
			public const int EquippedCharId_Size = 4;

			// Token: 0x04002BC7 RID: 11207
			public const uint MaterialResources_Offset = 17U;

			// Token: 0x04002BC8 RID: 11208
			public const int MaterialResources_Size = 12;
		}
	}
}
