using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Agile
{
	// Token: 0x0200056D RID: 1389
	public class XingWuDingZong : AttackChangeMobility
	{
		// Token: 0x060040FF RID: 16639 RVA: 0x00260F55 File Offset: 0x0025F155
		public XingWuDingZong()
		{
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x00260F5F File Offset: 0x0025F15F
		public XingWuDingZong(CombatSkillKey skillKey) : base(skillKey, 2502)
		{
			this.RequireWeaponSubType = 1;
		}
	}
}
