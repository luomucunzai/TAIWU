using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Polearm
{
	// Token: 0x0200013F RID: 319
	public class JueDou : AddDamage
	{
		// Token: 0x06002A8E RID: 10894 RVA: 0x00202CA5 File Offset: 0x00200EA5
		public JueDou()
		{
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x00202CAF File Offset: 0x00200EAF
		public JueDou(CombatSkillKey skillKey) : base(skillKey, 40902)
		{
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x00202CC0 File Offset: 0x00200EC0
		protected override int GetAddDamagePercent()
		{
			int markCount = base.CombatChar.GetDefeatMarkCollection().GetTotalCount();
			int maxDamageMarkCount = (int)(GlobalConfig.NeedDefeatMarkCount[(int)DomainManager.Combat.GetCombatType()] / 2);
			return 180 * markCount / maxDamageMarkCount;
		}
	}
}
