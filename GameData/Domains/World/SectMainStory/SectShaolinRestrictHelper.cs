using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.World.SectMainStory
{
	// Token: 0x02000038 RID: 56
	public static class SectShaolinRestrictHelper
	{
		// Token: 0x06000F20 RID: 3872 RVA: 0x000FB7F0 File Offset: 0x000F99F0
		public static short GetCombatConfigId(this IEnumerable<DemonSlayerTrialRestrictItem> restricts)
		{
			short result = 211;
			List<short> validCombatConfigs = ObjectPool<List<short>>.Instance.Get();
			List<short> preferCombatConfigs = ObjectPool<List<short>>.Instance.Get();
			using (IEnumerator<DemonSlayerTrialRestrictItem> enumerator = restricts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DemonSlayerTrialRestrictItem restrict = enumerator.Current;
					bool flag = validCombatConfigs.Count == 0;
					if (flag)
					{
						validCombatConfigs.AddRange(restrict.EffectiveCombatConfigs);
					}
					else
					{
						validCombatConfigs.RemoveAll((short x) => !restrict.EffectiveCombatConfigs.Contains(x));
						bool flag2 = validCombatConfigs.Count == 0;
						if (flag2)
						{
							break;
						}
					}
					bool flag3 = !preferCombatConfigs.Contains(restrict.PreferCombatConfig);
					if (flag3)
					{
						preferCombatConfigs.Add(restrict.PreferCombatConfig);
					}
				}
			}
			bool flag4 = validCombatConfigs.Count > 0;
			if (flag4)
			{
				bool flag5 = preferCombatConfigs.Any(new Func<short, bool>(validCombatConfigs.Contains));
				if (flag5)
				{
					validCombatConfigs.RemoveAll((short x) => !preferCombatConfigs.Contains(x));
				}
				result = validCombatConfigs[0];
			}
			ObjectPool<List<short>>.Instance.Return(validCombatConfigs);
			ObjectPool<List<short>>.Instance.Return(preferCombatConfigs);
			return result;
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x000FB964 File Offset: 0x000F9B64
		public static bool Check(this DemonSlayerTrialRestrictItem restrict)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			CombatSkillEquipment combatSkillEquipment = taiwu.GetCombatSkillEquipment();
			bool flag = restrict.MinCombatSkillGrade >= 0 && combatSkillEquipment.GetMinGrade() < (int)restrict.MinCombatSkillGrade;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = restrict.MaxCombatSkillGrade >= 0 && combatSkillEquipment.GetMaxGrade() > (int)restrict.MaxCombatSkillGrade;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = restrict.MaxAttackSkillSlotCount >= 0 && taiwu.GetSlotCount(1) > (int)restrict.MaxAttackSkillSlotCount;
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = restrict.MaxAgileSkillSlotCount >= 0 && taiwu.GetSlotCount(2) > (int)restrict.MaxAgileSkillSlotCount;
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool flag5 = restrict.MaxDefenseSkillSlotCount >= 0 && taiwu.GetSlotCount(3) > (int)restrict.MaxDefenseSkillSlotCount;
							if (flag5)
							{
								result = false;
							}
							else
							{
								bool flag6 = restrict.MaxAssistSkillSlotCount >= 0 && taiwu.GetSlotCount(4) > (int)restrict.MaxAssistSkillSlotCount;
								result = (!flag6 && (restrict.MaxWeaponSlotCount < 0 || taiwu.GetWeaponCount() <= (int)restrict.MaxWeaponSlotCount));
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x000FBA88 File Offset: 0x000F9C88
		private static int GetMinGrade(this CombatSkillEquipment combatSkillEquipment)
		{
			sbyte minGrade = 8;
			foreach (short skillId in combatSkillEquipment)
			{
				minGrade = Math.Min(minGrade, CombatSkill.Instance[skillId].Grade);
			}
			return (int)minGrade;
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x000FBAEC File Offset: 0x000F9CEC
		private static int GetMaxGrade(this CombatSkillEquipment combatSkillEquipment)
		{
			sbyte maxGrade = 0;
			foreach (short skillId in combatSkillEquipment)
			{
				maxGrade = Math.Max(maxGrade, CombatSkill.Instance[skillId].Grade);
			}
			return (int)maxGrade;
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x000FBB50 File Offset: 0x000F9D50
		private static int GetSlotCount(this GameData.Domains.Character.Character character, sbyte equipType)
		{
			return character.GetCombatSkillTypeRequireGrid(equipType);
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x000FBB6C File Offset: 0x000F9D6C
		private static int GetWeaponCount(this GameData.Domains.Character.Character character)
		{
			ItemKey[] equipment = character.GetEquipment();
			int count = 0;
			for (sbyte i = 0; i <= 2; i += 1)
			{
				bool flag = equipment[(int)i].IsValid();
				if (flag)
				{
					count++;
				}
			}
			return count;
		}
	}
}
