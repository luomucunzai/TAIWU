using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.FistAndPalm
{
	// Token: 0x020003D5 RID: 981
	public class ZhuiFengYunHuanZhang : GetTrick
	{
		// Token: 0x060037AF RID: 14255 RVA: 0x00236C8B File Offset: 0x00234E8B
		public ZhuiFengYunHuanZhang()
		{
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x00236C95 File Offset: 0x00234E95
		public ZhuiFengYunHuanZhang(CombatSkillKey skillKey) : base(skillKey, 4101)
		{
			this.GetTrickType = 8;
			this.DirectCanChangeTrickType = new sbyte[]
			{
				6,
				7
			};
		}
	}
}
