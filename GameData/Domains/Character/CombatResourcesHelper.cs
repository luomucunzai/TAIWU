using System;

namespace GameData.Domains.Character
{
	// Token: 0x0200080C RID: 2060
	public static class CombatResourcesHelper
	{
		// Token: 0x06007466 RID: 29798 RVA: 0x00443384 File Offset: 0x00441584
		public static CombatResources GetMax(Character character)
		{
			short medicineAttainment = character.GetLifeSkillAttainment(8);
			short toxicologyAttainment = character.GetLifeSkillAttainment(9);
			return new CombatResources
			{
				HealingCount = (sbyte)Math.Min((int)(1 + medicineAttainment / 60), 99),
				DetoxCount = (sbyte)Math.Min((int)(1 + toxicologyAttainment / 60), 99),
				BreathingCount = 1,
				RecoverCount = 1
			};
		}
	}
}
