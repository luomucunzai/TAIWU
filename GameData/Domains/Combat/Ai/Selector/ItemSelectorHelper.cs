using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat.Ai.Selector
{
	// Token: 0x0200072C RID: 1836
	public static class ItemSelectorHelper
	{
		// Token: 0x060068CF RID: 26831 RVA: 0x003B7FD4 File Offset: 0x003B61D4
		public static ItemKey SelectBestGradeItem(IRandomSource random, IEnumerable<ItemKey> pool)
		{
			List<sbyte> gradeList = ObjectPool<List<sbyte>>.Instance.Get();
			gradeList.Clear();
			foreach (List<ItemKey> itemKeyList in ItemSelectorHelper.GradeCache.Values)
			{
				itemKeyList.Clear();
			}
			foreach (ItemKey key in pool)
			{
				sbyte grade = ItemTemplateHelper.GetGrade(key.ItemType, key.TemplateId);
				bool flag = !gradeList.Contains(grade);
				if (flag)
				{
					gradeList.Add(grade);
				}
				bool flag2 = !ItemSelectorHelper.GradeCache.ContainsKey(grade);
				if (flag2)
				{
					ItemSelectorHelper.GradeCache.Add(grade, new List<ItemKey>());
				}
				ItemSelectorHelper.GradeCache[grade].Add(key);
			}
			gradeList.Sort();
			sbyte b;
			if (DomainManager.Combat.GetCombatType() != 1)
			{
				List<sbyte> list = gradeList;
				b = list[list.Count - 1];
			}
			else
			{
				b = gradeList[Math.Max(gradeList.Count - 1, 0) / 2];
			}
			sbyte targetGrade = b;
			ObjectPool<List<sbyte>>.Instance.Return(gradeList);
			return ItemSelectorHelper.GradeCache[targetGrade].GetRandom(random);
		}

		// Token: 0x060068D0 RID: 26832 RVA: 0x003B8144 File Offset: 0x003B6344
		private static bool HealInjury(CombatCharacter combatChar, ItemKey itemKey)
		{
			bool flag = itemKey.ItemType != 8;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				MedicineItem config = Config.Medicine.Instance[itemKey.TemplateId];
				EMedicineEffectType effectType = config.EffectType;
				bool flag2 = effectType <= EMedicineEffectType.RecoverInnerInjury;
				bool flag3 = !flag2;
				if (flag3)
				{
					result = false;
				}
				else
				{
					bool inner = config.EffectType == EMedicineEffectType.RecoverInnerInjury;
					Injuries injuries = combatChar.GetInjuries();
					int partCount = 0;
					int maxInjuryValue = 0;
					for (sbyte i = 0; i < 7; i += 1)
					{
						sbyte value = injuries.Get(i, inner);
						bool flag4 = value <= 0 || (short)value > config.EffectThresholdValue;
						if (!flag4)
						{
							partCount++;
							maxInjuryValue = Math.Max(maxInjuryValue, (int)value);
						}
					}
					bool flag5 = DomainManager.Combat.IsCharacterHalfFallen(combatChar);
					if (flag5)
					{
						result = (partCount > 0);
					}
					else
					{
						result = (partCount >= (int)(config.InjuryRecoveryTimes / 2) && maxInjuryValue >= (int)(config.EffectValue / 2));
					}
				}
			}
			return result;
		}

		// Token: 0x060068D1 RID: 26833 RVA: 0x003B8248 File Offset: 0x003B6448
		private unsafe static bool HealPoison(CombatCharacter combatChar, ItemKey itemKey)
		{
			bool flag = itemKey.ItemType != 8;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				MedicineItem config = Config.Medicine.Instance[itemKey.TemplateId];
				bool flag2 = config.EffectType != EMedicineEffectType.DetoxPoison;
				if (flag2)
				{
					result = false;
				}
				else
				{
					sbyte detoxPoisonType = config.DetoxPoisonType;
					bool flag3 = detoxPoisonType < 0;
					if (flag3)
					{
						result = false;
					}
					else
					{
						PoisonInts poisoned = *combatChar.GetCharacter().GetPoisoned();
						sbyte level = PoisonsAndLevels.CalcPoisonedLevel(*poisoned[(int)detoxPoisonType]);
						result = ((short)level <= config.EffectThresholdValue);
					}
				}
			}
			return result;
		}

		// Token: 0x060068D2 RID: 26834 RVA: 0x003B82E0 File Offset: 0x003B64E0
		private static bool HealQiDisorder(CombatCharacter combatChar, ItemKey itemKey)
		{
			bool flag = itemKey.ItemType != 8;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				MedicineItem config = Config.Medicine.Instance[itemKey.TemplateId];
				result = (config.EffectType == EMedicineEffectType.ChangeDisorderOfQi);
			}
			return result;
		}

		// Token: 0x060068D3 RID: 26835 RVA: 0x003B8320 File Offset: 0x003B6520
		private static bool Buff(CombatCharacter combatChar, ItemKey itemKey)
		{
			bool flag = itemKey.ItemType != 8;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				MedicineItem config = Config.Medicine.Instance[itemKey.TemplateId];
				result = GameData.Domains.Character.Character.BonusPropertyTypes.Any((ECharacterPropertyReferencedType type) => config.GetCharacterPropertyBonusInt(type) > 0);
			}
			return result;
		}

		// Token: 0x060068D4 RID: 26836 RVA: 0x003B8378 File Offset: 0x003B6578
		private static bool ThrowPoison(CombatCharacter combatChar, ItemKey itemKey)
		{
			bool flag = itemKey.ItemType != 8;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				short currDistance = DomainManager.Combat.GetCurrentDistance();
				MedicineItem config = Config.Medicine.Instance[itemKey.TemplateId];
				bool flag2 = (short)config.MaxUseDistance < currDistance;
				if (flag2)
				{
					result = false;
				}
				else
				{
					GameData.Domains.Character.Character character = combatChar.GetCharacter();
					bool flag3 = EatingItems.IsWugKing(itemKey);
					if (flag3)
					{
						result = !character.GetLearnedCombatSkills().Contains(WugKing.Instance.First((WugKingItem x) => x.WugMedicine == itemKey.TemplateId).WugFinger);
					}
					else
					{
						result = (config.ItemSubType == 801 && config.EffectType == EMedicineEffectType.ApplyPoison);
					}
				}
			}
			return result;
		}

		// Token: 0x060068D5 RID: 26837 RVA: 0x003B844C File Offset: 0x003B664C
		private static bool Neili(CombatCharacter combatChar, ItemKey itemKey)
		{
			bool flag = itemKey.ItemType != 12;
			return !flag && Config.Misc.Instance[itemKey.TemplateId].Neili > 0;
		}

		// Token: 0x060068D6 RID: 26838 RVA: 0x003B848C File Offset: 0x003B668C
		private static bool Wine(CombatCharacter combatChar, ItemKey itemKey)
		{
			bool flag = itemKey.ItemType != 9;
			return !flag && Config.TeaWine.Instance[itemKey.TemplateId].ItemSubType == 901;
		}

		// Token: 0x04001CC5 RID: 7365
		private static readonly Dictionary<sbyte, List<ItemKey>> GradeCache = new Dictionary<sbyte, List<ItemKey>>();

		// Token: 0x04001CC6 RID: 7366
		public static readonly Dictionary<EItemSelectorType, ItemSelectorPredicate> Predicates = new Dictionary<EItemSelectorType, ItemSelectorPredicate>
		{
			{
				EItemSelectorType.HealInjury,
				new ItemSelectorPredicate(ItemSelectorHelper.HealInjury)
			},
			{
				EItemSelectorType.HealPoison,
				new ItemSelectorPredicate(ItemSelectorHelper.HealPoison)
			},
			{
				EItemSelectorType.HealQiDisorder,
				new ItemSelectorPredicate(ItemSelectorHelper.HealQiDisorder)
			},
			{
				EItemSelectorType.Buff,
				new ItemSelectorPredicate(ItemSelectorHelper.Buff)
			},
			{
				EItemSelectorType.ThrowPoison,
				new ItemSelectorPredicate(ItemSelectorHelper.ThrowPoison)
			},
			{
				EItemSelectorType.Neili,
				new ItemSelectorPredicate(ItemSelectorHelper.Neili)
			},
			{
				EItemSelectorType.Wine,
				new ItemSelectorPredicate(ItemSelectorHelper.Wine)
			}
		};
	}
}
