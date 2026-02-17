using System;
using Config;
using GameData.Common;
using GameData.Domains.Map;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Item
{
	// Token: 0x0200065B RID: 1627
	public abstract class EquipmentBase : ItemBase
	{
		// Token: 0x06004E37 RID: 20023
		[CollectionObjectField(true, false, false, false, false)]
		public abstract sbyte GetEquipmentType();

		// Token: 0x06004E38 RID: 20024
		[CollectionObjectField(true, false, false, false, false)]
		public abstract bool GetDetachable();

		// Token: 0x06004E39 RID: 20025 RVA: 0x002B0030 File Offset: 0x002AE230
		protected EquipmentBase()
		{
			this.EquippedCharId = -1;
		}

		// Token: 0x06004E3A RID: 20026 RVA: 0x002B0041 File Offset: 0x002AE241
		public void OfflineSetEquippedCharId(int charId)
		{
			this.EquippedCharId = charId;
		}

		// Token: 0x06004E3B RID: 20027 RVA: 0x002B004C File Offset: 0x002AE24C
		public override int GetWeight()
		{
			int value = this.GetBaseWeight();
			return DomainManager.Item.GetEquipmentEffects(this).ModifyValue(value, (EquipmentEffectItem x) => (int)x.WeightChange);
		}

		// Token: 0x06004E3C RID: 20028 RVA: 0x002B0098 File Offset: 0x002AE298
		public override int GetValue()
		{
			int value = base.GetValue();
			bool flag = this.EquipmentEffectId >= 0;
			if (flag)
			{
				EquipmentEffectItem equipmentEffect = EquipmentEffect.Instance[this.EquipmentEffectId];
				value += value * (int)equipmentEffect.ValueChange / 100;
			}
			return value;
		}

		// Token: 0x06004E3D RID: 20029 RVA: 0x002B00E4 File Offset: 0x002AE2E4
		public void OfflineGenerateEquipmentEffect(IRandomSource random)
		{
			short itemSubType = this.GetItemSubType();
			bool flag = itemSubType == 104 || itemSubType == 17 || itemSubType == 16;
			if (!flag)
			{
				bool flag2 = this.EquipmentEffectId < 0;
				if (flag2)
				{
					this.EquipmentEffectId = ItemDomain.GetRandomEquipmentEffect(random, this.GetItemType());
				}
				bool flag3 = this.EquipmentEffectId < 0;
				if (!flag3)
				{
					EquipmentEffectItem equipmentEffectCfg = EquipmentEffect.Instance[this.EquipmentEffectId];
					bool flag4 = equipmentEffectCfg.MaxDurabilityChange != 0;
					if (flag4)
					{
						this.MaxDurability = this.MaxDurability * (short)(100 + equipmentEffectCfg.MaxDurabilityChange) / 100;
						this.CurrDurability = this.MaxDurability;
					}
				}
			}
		}

		// Token: 0x06004E3E RID: 20030 RVA: 0x002B018C File Offset: 0x002AE38C
		public void ApplyDurabilityEquipmentEffectChange(DataContext context, int oldId, int newId)
		{
			int oldChange = 0;
			int newChange = 0;
			bool flag = oldId > -1;
			if (flag)
			{
				EquipmentEffectItem oldConfig = EquipmentEffect.Instance[oldId];
				oldChange = (int)oldConfig.MaxDurabilityChange;
			}
			bool flag2 = newId > -1;
			if (flag2)
			{
				EquipmentEffectItem newConfig = EquipmentEffect.Instance[newId];
				newChange = (int)newConfig.MaxDurabilityChange;
			}
			int delta = newChange - oldChange;
			bool flag3 = delta != 0;
			if (flag3)
			{
				int baseDurability = (int)(this.MaxDurability * 100) / (100 + oldChange);
				this.MaxDurability = (short)(baseDurability * (100 + newChange) / 100);
				this.CurrDurability = Math.Min(this.CurrDurability, this.MaxDurability);
				this.SetMaxDurability(this.MaxDurability, context);
				this.SetCurrDurability(this.CurrDurability, context);
			}
		}

		// Token: 0x06004E3F RID: 20031 RVA: 0x002B0244 File Offset: 0x002AE444
		public unsafe void OfflineGenerateMaterialResources(IRandomSource random)
		{
			short makeItemSubType = ItemTemplateHelper.GetEquipmentMakeItemSubType(this.GetItemType(), this.TemplateId);
			bool flag = makeItemSubType < 0;
			if (!flag)
			{
				MakeItemSubTypeItem makeItemSubTypeCfg = MakeItemSubType.Instance[makeItemSubType];
				short totalResourceCount = makeItemSubTypeCfg.ResourceTotalCount;
				MaterialResources remainingValidSlots = default(MaterialResources);
				remainingValidSlots.Initialize();
				sbyte* validResTypes = stackalloc sbyte[(UIntPtr)6];
				int validCount = 0;
				for (sbyte resourceType = 0; resourceType < 6; resourceType += 1)
				{
					short maxMaterialResourceCount = *(ref makeItemSubTypeCfg.MaxMaterialResources.Items.FixedElementField + (IntPtr)resourceType * 2);
					bool flag2 = maxMaterialResourceCount <= 0;
					if (!flag2)
					{
						short resCount = (short)random.Next((int)(Math.Min(maxMaterialResourceCount, totalResourceCount) + 1));
						totalResourceCount -= resCount;
						*(ref this.MaterialResources.Items.FixedElementField + (IntPtr)resourceType * 2) = resCount;
						*(ref remainingValidSlots.Items.FixedElementField + (IntPtr)resourceType * 2) = maxMaterialResourceCount - resCount;
						validResTypes[validCount] = resourceType;
						validCount++;
					}
				}
				bool flag3 = validCount > 0;
				if (flag3)
				{
					CollectionUtils.Shuffle<sbyte>(random, validResTypes, validCount);
					int i = 0;
					while (i < validCount && totalResourceCount > 0)
					{
						sbyte resType = validResTypes[i];
						short resCount2 = *(ref remainingValidSlots.Items.FixedElementField + (IntPtr)resType * 2);
						bool flag4 = resCount2 >= totalResourceCount;
						if (flag4)
						{
							ref short ptr = ref this.MaterialResources.Items.FixedElementField + (IntPtr)resType * 2;
							ptr += totalResourceCount;
						}
						else
						{
							ref short ptr2 = ref this.MaterialResources.Items.FixedElementField + (IntPtr)resType * 2;
							ptr2 += resCount2;
						}
						totalResourceCount -= resCount2;
						i++;
					}
				}
			}
		}

		// Token: 0x06004E40 RID: 20032 RVA: 0x002B03EC File Offset: 0x002AE5EC
		protected int GetMaterialResourceBonusValuePercentage(sbyte equipmentBonusType)
		{
			return ItemTemplateHelper.GetMaterialResourceBonusValuePercentage(this.GetItemType(), this.TemplateId, equipmentBonusType, this.MaterialResources);
		}

		// Token: 0x06004E41 RID: 20033 RVA: 0x002B0418 File Offset: 0x002AE618
		public short GetEquipmentEffectId()
		{
			return this.EquipmentEffectId;
		}

		// Token: 0x06004E42 RID: 20034
		public abstract void SetEquipmentEffectId(short equipmentEffectId, DataContext context);

		// Token: 0x06004E43 RID: 20035 RVA: 0x002B0430 File Offset: 0x002AE630
		public int GetEquippedCharId()
		{
			return this.EquippedCharId;
		}

		// Token: 0x06004E44 RID: 20036
		public abstract void SetEquippedCharId(int equippedCharId, DataContext context);

		// Token: 0x06004E45 RID: 20037 RVA: 0x002B0448 File Offset: 0x002AE648
		public MaterialResources GetMaterialResources()
		{
			return this.MaterialResources;
		}

		// Token: 0x06004E46 RID: 20038
		public abstract void SetMaterialResources(MaterialResources materialResources, DataContext context);

		// Token: 0x04001578 RID: 5496
		[CollectionObjectField(false, true, false, false, false)]
		protected int EquippedCharId;

		// Token: 0x04001579 RID: 5497
		[CollectionObjectField(true, true, false, false, false)]
		protected short EquipmentEffectId;

		// Token: 0x0400157A RID: 5498
		[CollectionObjectField(false, true, false, true, false)]
		protected MaterialResources MaterialResources;
	}
}
