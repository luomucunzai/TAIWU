using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Agile
{
	// Token: 0x020005E2 RID: 1506
	public class TiZhenFa : MakeTricksInterchangeable
	{
		// Token: 0x06004468 RID: 17512 RVA: 0x0026F728 File Offset: 0x0026D928
		public TiZhenFa()
		{
		}

		// Token: 0x06004469 RID: 17513 RVA: 0x0026F732 File Offset: 0x0026D932
		public TiZhenFa(CombatSkillKey skillKey) : base(skillKey, 3400)
		{
			this.AffectTrickTypes = new List<sbyte>
			{
				8,
				5,
				2
			};
		}
	}
}
