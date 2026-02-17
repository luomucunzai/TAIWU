using System;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.Taiwu
{
	// Token: 0x02000040 RID: 64
	public static class SkillBreakPlateBonusHelper
	{
		// Token: 0x06000F75 RID: 3957 RVA: 0x000FE3BC File Offset: 0x000FC5BC
		public static SkillBreakPlateBonus CreateItem(ItemKey key)
		{
			return SkillBreakPlateBonus.CreateItem(key);
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x000FE3D4 File Offset: 0x000FC5D4
		public static SkillBreakPlateBonus CreateExp(int level)
		{
			return SkillBreakPlateBonus.CreateExp(level);
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x000FE3EC File Offset: 0x000FC5EC
		public static SkillBreakPlateBonus CreateRelation(int charId, int relatedCharId, ushort relationType)
		{
			bool flag = relationType == 16384 || relationType == 32768;
			bool flag2 = !flag;
			SkillBreakPlateBonus result;
			if (flag2)
			{
				result = SkillBreakPlateBonus.Invalid;
			}
			else
			{
				bool flag3 = !DomainManager.Character.HasRelation(charId, relatedCharId, relationType) || !DomainManager.Character.HasRelation(relatedCharId, charId, relationType);
				if (flag3)
				{
					result = SkillBreakPlateBonus.Invalid;
				}
				else
				{
					RelationKey relationKey = new RelationKey(charId, relatedCharId);
					short favorability = DomainManager.Character.GetFavorability(charId, relatedCharId);
					result = SkillBreakPlateBonus.CreateRelation(relationKey, relationType, favorability);
				}
			}
			return result;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x000FE47C File Offset: 0x000FC67C
		public static SkillBreakPlateBonus CreateFriend(int charId, int relatedCharId, short skillId)
		{
			CombatSkillItem config = Config.CombatSkill.Instance[skillId];
			bool flag = config == null;
			SkillBreakPlateBonus result;
			if (flag)
			{
				result = SkillBreakPlateBonus.Invalid;
			}
			else
			{
				GameData.Domains.Character.Character character;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (flag2)
				{
					result = SkillBreakPlateBonus.Invalid;
				}
				else
				{
					bool flag3 = character.GetCreatingType() != 1;
					if (flag3)
					{
						result = SkillBreakPlateBonus.Invalid;
					}
					else
					{
						bool flag4 = !character.GetLearnedCombatSkills().Contains(skillId);
						if (flag4)
						{
							result = SkillBreakPlateBonus.Invalid;
						}
						else
						{
							GameData.Domains.CombatSkill.CombatSkill skill;
							bool flag5 = !DomainManager.CombatSkill.TryGetElement_CombatSkills(new ValueTuple<int, short>(charId, skillId), out skill);
							if (flag5)
							{
								result = SkillBreakPlateBonus.Invalid;
							}
							else
							{
								bool flag6 = !CombatSkillStateHelper.IsBrokenOut(skill.GetActivationState());
								if (flag6)
								{
									result = SkillBreakPlateBonus.Invalid;
								}
								else
								{
									RelationKey relationKey = new RelationKey(charId, relatedCharId);
									short attainment = character.GetCombatSkillAttainment(config.Type);
									short favorability = DomainManager.Character.GetFavorability(charId, relatedCharId);
									result = SkillBreakPlateBonus.CreateFriend(relationKey, attainment, favorability);
								}
							}
						}
					}
				}
			}
			return result;
		}
	}
}
