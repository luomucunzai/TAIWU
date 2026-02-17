using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.FistAndPalm
{
	// Token: 0x0200051F RID: 1311
	public class HuoLianZhang : IgnoreArmor
	{
		// Token: 0x06003F1F RID: 16159 RVA: 0x00258A59 File Offset: 0x00256C59
		public HuoLianZhang()
		{
		}

		// Token: 0x06003F20 RID: 16160 RVA: 0x00258A63 File Offset: 0x00256C63
		public HuoLianZhang(CombatSkillKey skillKey) : base(skillKey, 14102)
		{
			this.RequireMainAttributeType = 3;
		}
	}
}
