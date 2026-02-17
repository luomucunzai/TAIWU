using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x02000330 RID: 816
	public class TianWaiZhenHuo : AddDamageByFiveElementsType
	{
		// Token: 0x0600347E RID: 13438 RVA: 0x00228DD1 File Offset: 0x00226FD1
		public TianWaiZhenHuo()
		{
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x00228DDB File Offset: 0x00226FDB
		public TianWaiZhenHuo(CombatSkillKey skillKey) : base(skillKey, 16403)
		{
			this.CounteringType = 0;
			this.CounteredType = 2;
		}
	}
}
