using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Neigong
{
	// Token: 0x020003C7 RID: 967
	public class LuoTianZhenJueShiErZhuang : TransferFiveElementsNeili
	{
		// Token: 0x06003771 RID: 14193 RVA: 0x00235952 File Offset: 0x00233B52
		public LuoTianZhenJueShiErZhuang()
		{
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x0023595C File Offset: 0x00233B5C
		public LuoTianZhenJueShiErZhuang(CombatSkillKey skillKey) : base(skillKey, 4003)
		{
			this.SrcFiveElementsType = (base.IsDirect ? 1 : 2);
			this.DestFiveElementsType = 3;
		}
	}
}
