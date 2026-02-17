using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Agile
{
	// Token: 0x020004D6 RID: 1238
	public class DaDingJiao : MakeTricksInterchangeable
	{
		// Token: 0x06003D8C RID: 15756 RVA: 0x0025257C File Offset: 0x0025077C
		public DaDingJiao()
		{
		}

		// Token: 0x06003D8D RID: 15757 RVA: 0x00252586 File Offset: 0x00250786
		public DaDingJiao(CombatSkillKey skillKey) : base(skillKey, 11501)
		{
			this.AffectTrickTypes = new List<sbyte>
			{
				6,
				3,
				0
			};
		}
	}
}
