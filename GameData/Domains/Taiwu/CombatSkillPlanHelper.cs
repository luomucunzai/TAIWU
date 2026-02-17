using System;
using GameData.Domains.Character;

namespace GameData.Domains.Taiwu
{
	// Token: 0x0200003C RID: 60
	public static class CombatSkillPlanHelper
	{
		// Token: 0x06000F6B RID: 3947 RVA: 0x000FDF8C File Offset: 0x000FC18C
		public static void Record(this CombatSkillPlan plan, Character character)
		{
			CombatSkillEquipment combatSkillEquipment = character.GetCombatSkillEquipment();
			CombatSkillPlan srcPlan = combatSkillEquipment.GetSourceObject<CombatSkillPlan>();
			bool flag = srcPlan != null;
			if (flag)
			{
				plan.CopyFrom(srcPlan);
			}
			else
			{
				short[] skillIdList = character.GetEquippedCombatSkills();
				plan.Record(skillIdList);
			}
		}
	}
}
