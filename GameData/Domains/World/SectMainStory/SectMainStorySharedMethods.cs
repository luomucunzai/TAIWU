using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using GameData.Combat.Math;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.World.SectMainStory
{
	// Token: 0x02000036 RID: 54
	public static class SectMainStorySharedMethods
	{
		// Token: 0x06000F04 RID: 3844 RVA: 0x000FA504 File Offset: 0x000F8704
		public static bool IsEmeiBonusFit(short bonusTypeTemplateId, ItemKey itemKey)
		{
			SkillBreakPlateGridBonusTypeItem config = SkillBreakPlateGridBonusType.Instance[bonusTypeTemplateId];
			short subType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
			short[] extraBonusFitItemSubTypes = config.ExtraBonusFitItemSubTypes;
			bool flag = extraBonusFitItemSubTypes != null && extraBonusFitItemSubTypes.Contains(subType);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = itemKey.ItemType != 10;
				if (flag2)
				{
					result = false;
				}
				else
				{
					SkillBookItem itemConfig = Config.SkillBook.Instance[itemKey.TemplateId];
					sbyte[] extraBonusFitCombatSkillTypes = config.ExtraBonusFitCombatSkillTypes;
					bool flag3;
					if (extraBonusFitCombatSkillTypes == null || !extraBonusFitCombatSkillTypes.Contains(itemConfig.CombatSkillType))
					{
						sbyte[] extraBonusFitLifeSkillTypes = config.ExtraBonusFitLifeSkillTypes;
						flag3 = (extraBonusFitLifeSkillTypes != null && extraBonusFitLifeSkillTypes.Contains(itemConfig.LifeSkillType));
					}
					else
					{
						flag3 = true;
					}
					result = flag3;
				}
			}
			return result;
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x000FA5B0 File Offset: 0x000F87B0
		public static int CalcEmeiBonusItemProgress(short bonusTypeTemplateId, ItemKey itemKey)
		{
			ItemBase itemBase = DomainManager.Item.GetBaseItem(itemKey);
			int value = itemBase.GetValue();
			int progress = SectMainStorySharedMethods.IsEmeiBonusFit(bonusTypeTemplateId, itemKey) ? value : (value * GlobalConfig.Instance.SectStoryEmeiBonusNotFitProgressPercent);
			return Math.Max(progress, GlobalConfig.Instance.SectStoryEmeiBonusMinProgress);
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x000FA608 File Offset: 0x000F8808
		public static int CalcEmeiBonusItemProgress(short bonusTypeTemplateId, IEnumerable<ItemKey> itemKeys)
		{
			return (itemKeys == null) ? 0 : (from x in itemKeys
			select SectMainStorySharedMethods.CalcEmeiBonusItemProgress(bonusTypeTemplateId, x)).Sum();
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x000FA644 File Offset: 0x000F8844
		public static int CalcWugJugRefiningCostPoisonValue(SectWuxianWugJugData jugData)
		{
			int baseValue = GlobalConfig.Instance.WugJugRefiningCostPoison;
			int currDate = DomainManager.World.GetCurrDate();
			int bonusPercent = GlobalConfig.Instance.WugJugRefiningCostPoisonBonusPercent;
			int monthPercent = GlobalConfig.Instance.WugJugRefiningCostPoisonMonthPercent;
			CValuePercentBonus bonus = (jugData.LastRefiningDate < 0) ? 0 : MathUtils.Max(bonusPercent + monthPercent * (currDate - jugData.LastRefiningDate), 0);
			return baseValue * bonus;
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x000FA6B4 File Offset: 0x000F88B4
		public static sbyte CalcWugKingType(List<int> costPoisons, SectWuxianWugJugData jugData)
		{
			SectMainStorySharedMethods.<>c__DisplayClass4_0 CS$<>8__locals1;
			CS$<>8__locals1.costPoisons = costPoisons;
			CS$<>8__locals1.costPoisons.Clear();
			for (int i = 0; i < 6; i++)
			{
				CS$<>8__locals1.costPoisons.Add(0);
			}
			int require = SectMainStorySharedMethods.CalcWugJugRefiningCostPoisonValue(jugData);
			int jugPoison = jugData.Poisons.Sum();
			bool flag = jugPoison < require;
			sbyte result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				for (int j = 0; j < 6; j++)
				{
					CS$<>8__locals1.costPoisons[j] = (int)MathUtils.Clamp((long)require * (long)jugData.Poisons[j] / (long)jugPoison, 0L, (long)require);
				}
				int costPoison = CS$<>8__locals1.costPoisons.Sum();
				int adjustPoison = costPoison - require;
				for (int k = 0; k < 6; k++)
				{
					bool flag2 = CS$<>8__locals1.costPoisons[k] <= 0;
					if (!flag2)
					{
						int adjust = MathUtils.Min(CS$<>8__locals1.costPoisons[k], adjustPoison);
						List<int> costPoisons2 = CS$<>8__locals1.costPoisons;
						int index = k;
						costPoisons2[index] -= adjust;
						adjustPoison -= adjust;
					}
				}
				foreach (WugKingItem wugKing in ((IEnumerable<WugKingItem>)WugKing.Instance))
				{
					SectMainStorySharedMethods.<>c__DisplayClass4_1 CS$<>8__locals2;
					CS$<>8__locals2.minPoison = require * (int)wugKing.PoisonMinPercent / 100;
					CS$<>8__locals2.maxPoison = require * (int)wugKing.PoisonMaxPercent / 100;
					bool match = true;
					for (sbyte l = 0; l < 6; l += 1)
					{
						bool flag3 = wugKing.RefiningPoisons.Contains(l);
						if (flag3)
						{
							match = (match && SectMainStorySharedMethods.<CalcWugKingType>g__Match|4_0(l, ref CS$<>8__locals1, ref CS$<>8__locals2));
						}
						else
						{
							bool poisonUnique = wugKing.PoisonUnique;
							if (poisonUnique)
							{
								match = (match && !SectMainStorySharedMethods.<CalcWugKingType>g__Match|4_0(l, ref CS$<>8__locals1, ref CS$<>8__locals2));
							}
						}
					}
					bool flag4 = match;
					if (flag4)
					{
						return wugKing.TemplateId;
					}
				}
				result = -1;
			}
			return result;
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x000FA8D8 File Offset: 0x000F8AD8
		public unsafe static PoisonInts CalcDropPoisonValue(ItemKey itemKey)
		{
			PoisonInts values = default(PoisonInts);
			values.Initialize();
			sbyte itemType = itemKey.ItemType;
			bool flag = itemType - 4 <= 1;
			bool flag2 = !flag;
			PoisonInts result;
			if (flag2)
			{
				result = values;
			}
			else
			{
				sbyte itemType2 = itemKey.ItemType;
				if (!true)
				{
				}
				PoisonsAndLevels innatePoisons;
				if (itemType2 != 4)
				{
					if (itemType2 != 5)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
						defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
						defaultInterpolatedStringHandler.AppendLiteral(" ");
						defaultInterpolatedStringHandler.AppendFormatted<sbyte>(itemKey.ItemType);
						throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					innatePoisons = Config.Material.Instance[itemKey.TemplateId].InnatePoisons;
				}
				else
				{
					innatePoisons = Config.Carrier.Instance[itemKey.TemplateId].InnatePoisons;
				}
				if (!true)
				{
				}
				PoisonsAndLevels poisons = innatePoisons;
				ItemBase itemBase = DomainManager.Item.GetBaseItem(itemKey);
				short currDurability = itemBase.GetCurrDurability();
				short maxDurability = itemBase.GetMaxDurability();
				short durability = currDurability;
				CValuePercent percent = (itemKey.ItemType == 4 && maxDurability > 0) ? ItemFormula.FormulaCalcDurabilityEffect((int)durability, (int)maxDurability) : 100;
				int ratio = GlobalConfig.Instance.WugJugPoisonDropRatio;
				for (sbyte i = 0; i < 6; i += 1)
				{
					ValueTuple<short, sbyte> valueAndLevel = poisons.GetValueAndLevel(i);
					*values[(int)i] = (int)(valueAndLevel.Item1 * (short)valueAndLevel.Item2) * percent * ratio;
				}
				result = values;
			}
			return result;
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x000FAA50 File Offset: 0x000F8C50
		public static PoisonInts CalcDropPoisonValue(SectWuxianWugJugData jugData, List<ItemKey> items)
		{
			PoisonInts addPoisons = default(PoisonInts);
			addPoisons.Initialize();
			foreach (ItemKey item in items)
			{
				PoisonInts poisons = SectMainStorySharedMethods.CalcDropPoisonValue(item);
				addPoisons.Add(ref poisons);
			}
			return addPoisons;
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x000FAAC4 File Offset: 0x000F8CC4
		[CompilerGenerated]
		internal static bool <CalcWugKingType>g__Match|4_0(sbyte type, ref SectMainStorySharedMethods.<>c__DisplayClass4_0 A_1, ref SectMainStorySharedMethods.<>c__DisplayClass4_1 A_2)
		{
			return A_1.costPoisons[(int)type] >= A_2.minPoison && A_1.costPoisons[(int)type] <= A_2.maxPoison;
		}
	}
}
