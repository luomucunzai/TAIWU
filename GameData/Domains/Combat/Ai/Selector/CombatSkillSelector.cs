using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Selector
{
	// Token: 0x02000725 RID: 1829
	public class CombatSkillSelector
	{
		// Token: 0x060068AF RID: 26799 RVA: 0x003B7A9E File Offset: 0x003B5C9E
		private static bool IsValid(short skillId)
		{
			return skillId >= 0;
		}

		// Token: 0x060068B0 RID: 26800 RVA: 0x003B7AA7 File Offset: 0x003B5CA7
		public CombatSkillSelector(sbyte equipType) : this(equipType, null, null)
		{
		}

		// Token: 0x060068B1 RID: 26801 RVA: 0x003B7AB4 File Offset: 0x003B5CB4
		public CombatSkillSelector(sbyte equipType, CombatSkillSelectorPredicate predicate, CombatSkillSelectorComparison comparison)
		{
			this._equipType = equipType;
			this._predicate = predicate;
			this._comparison = comparison;
		}

		// Token: 0x060068B2 RID: 26802 RVA: 0x003B7AD4 File Offset: 0x003B5CD4
		private int Comparison(short skillA, short skillB)
		{
			EAiPriority priorityA = this._updatingMemory.GetPriority(skillA);
			EAiPriority priorityB = this._updatingMemory.GetPriority(skillB);
			bool flag = priorityA != priorityB;
			int result;
			if (flag)
			{
				int num = (int)priorityA;
				result = num.CompareTo((int)priorityB);
			}
			else
			{
				CombatSkillItem configA = Config.CombatSkill.Instance[skillA];
				CombatSkillItem configB = Config.CombatSkill.Instance[skillB];
				result = this.Comparison(configA, configB);
			}
			return result;
		}

		// Token: 0x060068B3 RID: 26803 RVA: 0x003B7B40 File Offset: 0x003B5D40
		private int Comparison(CombatSkillItem configA, CombatSkillItem configB)
		{
			CombatSkillSelectorComparison comparison = this._comparison;
			int result = (comparison != null) ? comparison(configA, configB) : 0;
			bool flag = result != 0;
			int result2;
			if (flag)
			{
				result2 = result;
			}
			else
			{
				bool flag2 = configA.Grade != configB.Grade;
				if (flag2)
				{
					result2 = configB.Grade.CompareTo(configA.Grade);
				}
				else
				{
					result2 = configA.TemplateId.CompareTo(configB.TemplateId);
				}
			}
			return result2;
		}

		// Token: 0x060068B4 RID: 26804 RVA: 0x003B7BAD File Offset: 0x003B5DAD
		private bool Predicate(short skillId)
		{
			return this.Predicate(Config.CombatSkill.Instance[skillId]);
		}

		// Token: 0x060068B5 RID: 26805 RVA: 0x003B7BC0 File Offset: 0x003B5DC0
		private bool Predicate(CombatSkillItem config)
		{
			CombatSkillSelectorPredicate predicate = this._predicate;
			return predicate == null || predicate(config);
		}

		// Token: 0x060068B6 RID: 26806 RVA: 0x003B7BD8 File Offset: 0x003B5DD8
		public short Select(AiMemoryNew memory, CombatCharacter combatChar)
		{
			this._updatingMemory = memory;
			IEnumerable<short> skillList = combatChar.GetCombatSkillList(this._equipType).Where(new Func<short, bool>(CombatSkillSelector.IsValid));
			sbyte equipType = this._equipType;
			if (!true)
			{
			}
			IEnumerable<short> enumerable;
			switch (equipType)
			{
			case 1:
				enumerable = skillList.Where(new Func<short, bool>(CombatSkillTemplateHelper.IsAttack));
				break;
			case 2:
				enumerable = skillList.Where(new Func<short, bool>(CombatSkillTemplateHelper.IsAgile));
				break;
			case 3:
				enumerable = skillList.Where(new Func<short, bool>(CombatSkillTemplateHelper.IsDefense));
				break;
			default:
				enumerable = skillList;
				break;
			}
			if (!true)
			{
			}
			skillList = enumerable;
			skillList = skillList.Where(new Func<short, bool>(combatChar.AiCanCast)).Where(new Func<short, bool>(this.Predicate));
			List<short> skillIds = ObjectPool<List<short>>.Instance.Get();
			skillIds.Clear();
			skillIds.AddRange(skillList);
			skillIds.Sort(new Comparison<short>(this.Comparison));
			short skillId = (skillIds.Count > 0) ? skillIds[0] : -1;
			ObjectPool<List<short>>.Instance.Return(skillIds);
			this._updatingMemory = null;
			return skillId;
		}

		// Token: 0x04001CB6 RID: 7350
		private readonly sbyte _equipType;

		// Token: 0x04001CB7 RID: 7351
		private readonly CombatSkillSelectorPredicate _predicate;

		// Token: 0x04001CB8 RID: 7352
		private readonly CombatSkillSelectorComparison _comparison;

		// Token: 0x04001CB9 RID: 7353
		private AiMemoryNew _updatingMemory;
	}
}
