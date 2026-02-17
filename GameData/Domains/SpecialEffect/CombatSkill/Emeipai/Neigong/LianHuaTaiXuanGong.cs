using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Neigong
{
	// Token: 0x0200054A RID: 1354
	public class LianHuaTaiXuanGong : TransferFiveElementsNeili
	{
		// Token: 0x06004025 RID: 16421 RVA: 0x0025D10C File Offset: 0x0025B30C
		public LianHuaTaiXuanGong()
		{
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x0025D116 File Offset: 0x0025B316
		public LianHuaTaiXuanGong(CombatSkillKey skillKey) : base(skillKey, 2004)
		{
			this.SrcFiveElementsType = (base.IsDirect ? 3 : 0);
			this.DestFiveElementsType = 1;
		}
	}
}
