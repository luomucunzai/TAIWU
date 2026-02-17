using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Sword
{
	// Token: 0x020003BF RID: 959
	public class TaiYiXuanMenJian : PowerUpByMainAttribute
	{
		// Token: 0x06003742 RID: 14146 RVA: 0x00234B8B File Offset: 0x00232D8B
		public TaiYiXuanMenJian()
		{
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x00234B95 File Offset: 0x00232D95
		public TaiYiXuanMenJian(CombatSkillKey skillKey) : base(skillKey, 4203)
		{
			this.RequireMainAttributeType = 4;
		}
	}
}
