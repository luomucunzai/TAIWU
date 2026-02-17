using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.FistAndPalm
{
	// Token: 0x020003D2 RID: 978
	public class WuDangMianZhang : IgnoreArmor
	{
		// Token: 0x060037A3 RID: 14243 RVA: 0x002367E6 File Offset: 0x002349E6
		public WuDangMianZhang()
		{
		}

		// Token: 0x060037A4 RID: 14244 RVA: 0x002367F0 File Offset: 0x002349F0
		public WuDangMianZhang(CombatSkillKey skillKey) : base(skillKey, 4102)
		{
			this.RequireMainAttributeType = 4;
		}
	}
}
