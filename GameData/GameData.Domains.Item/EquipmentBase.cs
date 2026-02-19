using System;
using Config;
using GameData.Common;
using GameData.Domains.Map;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Item;

public abstract class EquipmentBase : ItemBase
{
	[CollectionObjectField(false, true, false, false, false)]
	protected int EquippedCharId;

	[CollectionObjectField(true, true, false, false, false)]
	protected short EquipmentEffectId;

	[CollectionObjectField(false, true, false, true, false)]
	protected MaterialResources MaterialResources;

	[CollectionObjectField(true, false, false, false, false)]
	public abstract sbyte GetEquipmentType();

	[CollectionObjectField(true, false, false, false, false)]
	public abstract bool GetDetachable();

	protected EquipmentBase()
	{
		EquippedCharId = -1;
	}

	public void OfflineSetEquippedCharId(int charId)
	{
		EquippedCharId = charId;
	}

	public override int GetWeight()
	{
		int baseWeight = GetBaseWeight();
		return DomainManager.Item.GetEquipmentEffects(this).ModifyValue(baseWeight, (EquipmentEffectItem x) => x.WeightChange);
	}

	public override int GetValue()
	{
		int num = base.GetValue();
		if (EquipmentEffectId >= 0)
		{
			EquipmentEffectItem equipmentEffectItem = EquipmentEffect.Instance[EquipmentEffectId];
			num += num * equipmentEffectItem.ValueChange / 100;
		}
		return num;
	}

	public void OfflineGenerateEquipmentEffect(IRandomSource random)
	{
		short itemSubType = GetItemSubType();
		if (itemSubType == 104 || itemSubType == 17 || itemSubType == 16)
		{
			return;
		}
		if (EquipmentEffectId < 0)
		{
			EquipmentEffectId = ItemDomain.GetRandomEquipmentEffect(random, GetItemType());
		}
		if (EquipmentEffectId >= 0)
		{
			EquipmentEffectItem equipmentEffectItem = EquipmentEffect.Instance[EquipmentEffectId];
			if (equipmentEffectItem.MaxDurabilityChange != 0)
			{
				MaxDurability = (short)(MaxDurability * (100 + equipmentEffectItem.MaxDurabilityChange) / 100);
				CurrDurability = MaxDurability;
			}
		}
	}

	public void ApplyDurabilityEquipmentEffectChange(DataContext context, int oldId, int newId)
	{
		int num = 0;
		int num2 = 0;
		if (oldId > -1)
		{
			EquipmentEffectItem equipmentEffectItem = EquipmentEffect.Instance[oldId];
			num = equipmentEffectItem.MaxDurabilityChange;
		}
		if (newId > -1)
		{
			EquipmentEffectItem equipmentEffectItem2 = EquipmentEffect.Instance[newId];
			num2 = equipmentEffectItem2.MaxDurabilityChange;
		}
		if (num2 - num != 0)
		{
			int num3 = MaxDurability * 100 / (100 + num);
			MaxDurability = (short)(num3 * (100 + num2) / 100);
			CurrDurability = Math.Min(CurrDurability, MaxDurability);
			SetMaxDurability(MaxDurability, context);
			SetCurrDurability(CurrDurability, context);
		}
	}

	public unsafe void OfflineGenerateMaterialResources(IRandomSource random)
	{
		short equipmentMakeItemSubType = ItemTemplateHelper.GetEquipmentMakeItemSubType(GetItemType(), TemplateId);
		if (equipmentMakeItemSubType < 0)
		{
			return;
		}
		MakeItemSubTypeItem makeItemSubTypeItem = MakeItemSubType.Instance[equipmentMakeItemSubType];
		short num = makeItemSubTypeItem.ResourceTotalCount;
		MaterialResources materialResources = default(MaterialResources);
		materialResources.Initialize();
		sbyte* ptr = stackalloc sbyte[6];
		int num2 = 0;
		for (sbyte b = 0; b < 6; b++)
		{
			short num3 = makeItemSubTypeItem.MaxMaterialResources.Items[b];
			if (num3 > 0)
			{
				short num4 = (short)random.Next(Math.Min(num3, num) + 1);
				num -= num4;
				MaterialResources.Items[b] = num4;
				materialResources.Items[b] = (short)(num3 - num4);
				ptr[num2] = b;
				num2++;
			}
		}
		if (num2 <= 0)
		{
			return;
		}
		CollectionUtils.Shuffle(random, ptr, num2);
		for (int i = 0; i < num2; i++)
		{
			if (num <= 0)
			{
				break;
			}
			sbyte b2 = ptr[i];
			short num5 = materialResources.Items[b2];
			if (num5 >= num)
			{
				ref short reference = ref MaterialResources.Items[b2];
				reference += num;
			}
			else
			{
				ref short reference2 = ref MaterialResources.Items[b2];
				reference2 += num5;
			}
			num -= num5;
		}
	}

	protected int GetMaterialResourceBonusValuePercentage(sbyte equipmentBonusType)
	{
		return ItemTemplateHelper.GetMaterialResourceBonusValuePercentage(GetItemType(), TemplateId, equipmentBonusType, MaterialResources);
	}

	public short GetEquipmentEffectId()
	{
		return EquipmentEffectId;
	}

	public abstract void SetEquipmentEffectId(short equipmentEffectId, DataContext context);

	public int GetEquippedCharId()
	{
		return EquippedCharId;
	}

	public abstract void SetEquippedCharId(int equippedCharId, DataContext context);

	public MaterialResources GetMaterialResources()
	{
		return MaterialResources;
	}

	public abstract void SetMaterialResources(MaterialResources materialResources, DataContext context);
}
