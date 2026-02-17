using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Agile
{
	// Token: 0x020003E5 RID: 997
	public class YunHuanBu : AttackChangeMobility
	{
		// Token: 0x06003819 RID: 14361 RVA: 0x00238D29 File Offset: 0x00236F29
		public YunHuanBu()
		{
		}

		// Token: 0x0600381A RID: 14362 RVA: 0x00238D33 File Offset: 0x00236F33
		public YunHuanBu(CombatSkillKey skillKey) : base(skillKey, 4402)
		{
			this.RequireWeaponSubType = 6;
		}
	}
}
