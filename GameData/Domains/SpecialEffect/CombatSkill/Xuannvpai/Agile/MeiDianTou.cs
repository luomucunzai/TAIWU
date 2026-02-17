using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Agile
{
	// Token: 0x0200028B RID: 651
	public class MeiDianTou : AttackChangeMobility
	{
		// Token: 0x06003121 RID: 12577 RVA: 0x00219D24 File Offset: 0x00217F24
		public MeiDianTou()
		{
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x00219D2E File Offset: 0x00217F2E
		public MeiDianTou(CombatSkillKey skillKey) : base(skillKey, 8400)
		{
			this.RequireWeaponSubType = 11;
		}
	}
}
