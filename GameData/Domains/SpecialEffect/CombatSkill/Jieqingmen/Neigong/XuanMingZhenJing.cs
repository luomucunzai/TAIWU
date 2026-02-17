using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Neigong
{
	// Token: 0x020004F0 RID: 1264
	public class XuanMingZhenJing : TransferFiveElementsNeili
	{
		// Token: 0x06003E2A RID: 15914 RVA: 0x00254D90 File Offset: 0x00252F90
		public XuanMingZhenJing()
		{
		}

		// Token: 0x06003E2B RID: 15915 RVA: 0x00254D9A File Offset: 0x00252F9A
		public XuanMingZhenJing(CombatSkillKey skillKey) : base(skillKey, 13004)
		{
			this.SrcFiveElementsType = (base.IsDirect ? 0 : 4);
			this.DestFiveElementsType = 2;
		}
	}
}
