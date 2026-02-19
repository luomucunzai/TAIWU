using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Combat.Math;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.World.SectMainStory;

public static class SectMainStorySharedMethods
{
	public static bool IsEmeiBonusFit(short bonusTypeTemplateId, ItemKey itemKey)
	{
		SkillBreakPlateGridBonusTypeItem skillBreakPlateGridBonusTypeItem = SkillBreakPlateGridBonusType.Instance[bonusTypeTemplateId];
		short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
		short[] extraBonusFitItemSubTypes = skillBreakPlateGridBonusTypeItem.ExtraBonusFitItemSubTypes;
		if (extraBonusFitItemSubTypes != null && extraBonusFitItemSubTypes.Contains(itemSubType))
		{
			return true;
		}
		if (itemKey.ItemType != 10)
		{
			return false;
		}
		SkillBookItem skillBookItem = Config.SkillBook.Instance[itemKey.TemplateId];
		sbyte[] extraBonusFitCombatSkillTypes = skillBreakPlateGridBonusTypeItem.ExtraBonusFitCombatSkillTypes;
		return (extraBonusFitCombatSkillTypes != null && extraBonusFitCombatSkillTypes.Contains(skillBookItem.CombatSkillType)) || (skillBreakPlateGridBonusTypeItem.ExtraBonusFitLifeSkillTypes?.Contains(skillBookItem.LifeSkillType) ?? false);
	}

	public static int CalcEmeiBonusItemProgress(short bonusTypeTemplateId, ItemKey itemKey)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
		int value = baseItem.GetValue();
		int val = (IsEmeiBonusFit(bonusTypeTemplateId, itemKey) ? value : (value * CValuePercent.op_Implicit(GlobalConfig.Instance.SectStoryEmeiBonusNotFitProgressPercent)));
		return Math.Max(val, GlobalConfig.Instance.SectStoryEmeiBonusMinProgress);
	}

	public static int CalcEmeiBonusItemProgress(short bonusTypeTemplateId, IEnumerable<ItemKey> itemKeys)
	{
		return itemKeys?.Select((ItemKey x) => CalcEmeiBonusItemProgress(bonusTypeTemplateId, x)).Sum() ?? 0;
	}

	public static int CalcWugJugRefiningCostPoisonValue(SectWuxianWugJugData jugData)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		int wugJugRefiningCostPoison = GlobalConfig.Instance.WugJugRefiningCostPoison;
		int currDate = DomainManager.World.GetCurrDate();
		int wugJugRefiningCostPoisonBonusPercent = GlobalConfig.Instance.WugJugRefiningCostPoisonBonusPercent;
		int wugJugRefiningCostPoisonMonthPercent = GlobalConfig.Instance.WugJugRefiningCostPoisonMonthPercent;
		CValuePercentBonus val = CValuePercentBonus.op_Implicit((jugData.LastRefiningDate >= 0) ? MathUtils.Max(wugJugRefiningCostPoisonBonusPercent + wugJugRefiningCostPoisonMonthPercent * (currDate - jugData.LastRefiningDate), 0) : 0);
		return wugJugRefiningCostPoison * val;
	}

	public static sbyte CalcWugKingType(List<int> costPoisons, SectWuxianWugJugData jugData)
	{
		costPoisons.Clear();
		for (int i = 0; i < 6; i++)
		{
			costPoisons.Add(0);
		}
		int num = CalcWugJugRefiningCostPoisonValue(jugData);
		int num2 = jugData.Poisons.Sum();
		if (num2 < num)
		{
			return -1;
		}
		for (int j = 0; j < 6; j++)
		{
			costPoisons[j] = (int)MathUtils.Clamp((long)num * (long)jugData.Poisons[j] / num2, 0L, num);
		}
		int num3 = costPoisons.Sum();
		int num4 = num3 - num;
		for (int k = 0; k < 6; k++)
		{
			if (costPoisons[k] > 0)
			{
				int num5 = MathUtils.Min(costPoisons[k], num4);
				costPoisons[k] -= num5;
				num4 -= num5;
			}
		}
		foreach (WugKingItem item in (IEnumerable<WugKingItem>)WugKing.Instance)
		{
			int minPoison = num * item.PoisonMinPercent / 100;
			int maxPoison = num * item.PoisonMaxPercent / 100;
			bool flag = true;
			for (sbyte b = 0; b < 6; b++)
			{
				if (item.RefiningPoisons.Contains(b))
				{
					flag = flag && Match(b);
				}
				else if (item.PoisonUnique)
				{
					flag = flag && !Match(b);
				}
			}
			if (flag)
			{
				return item.TemplateId;
			}
			bool Match(sbyte type)
			{
				return costPoisons[type] >= minPoison && costPoisons[type] <= maxPoison;
			}
		}
		return -1;
	}

	public static PoisonInts CalcDropPoisonValue(ItemKey itemKey)
	{
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		PoisonInts result = default(PoisonInts);
		result.Initialize();
		sbyte itemType = itemKey.ItemType;
		if ((uint)(itemType - 4) > 1u)
		{
			return result;
		}
		sbyte itemType2 = itemKey.ItemType;
		if (1 == 0)
		{
		}
		PoisonsAndLevels poisonsAndLevels = itemType2 switch
		{
			5 => Config.Material.Instance[itemKey.TemplateId].InnatePoisons, 
			4 => Config.Carrier.Instance[itemKey.TemplateId].InnatePoisons, 
			_ => throw new ArgumentOutOfRangeException($"{itemKey} {itemKey.ItemType}"), 
		};
		if (1 == 0)
		{
		}
		PoisonsAndLevels poisonsAndLevels2 = poisonsAndLevels;
		ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
		short currDurability = baseItem.GetCurrDurability();
		short maxDurability = baseItem.GetMaxDurability();
		short currDurability2 = currDurability;
		CValuePercent val = ((itemKey.ItemType == 4 && maxDurability > 0) ? ItemFormula.FormulaCalcDurabilityEffect(currDurability2, maxDurability) : CValuePercent.op_Implicit(100));
		int wugJugPoisonDropRatio = GlobalConfig.Instance.WugJugPoisonDropRatio;
		for (sbyte b = 0; b < 6; b++)
		{
			(short, sbyte) valueAndLevel = poisonsAndLevels2.GetValueAndLevel(b);
			result[b] = valueAndLevel.Item1 * valueAndLevel.Item2 * val * wugJugPoisonDropRatio;
		}
		return result;
	}

	public static PoisonInts CalcDropPoisonValue(SectWuxianWugJugData jugData, List<ItemKey> items)
	{
		PoisonInts result = default(PoisonInts);
		result.Initialize();
		foreach (ItemKey item in items)
		{
			PoisonInts delta = CalcDropPoisonValue(item);
			result.Add(ref delta);
		}
		return result;
	}
}
