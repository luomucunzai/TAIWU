using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.FistAndPalm
{
	// Token: 0x02000230 RID: 560
	public class BaiGuiHuaGuZhang : CastAgainOrPowerUp
	{
		// Token: 0x06002F7E RID: 12158 RVA: 0x002135FC File Offset: 0x002117FC
		public BaiGuiHuaGuZhang()
		{
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x00213606 File Offset: 0x00211806
		public BaiGuiHuaGuZhang(CombatSkillKey skillKey) : base(skillKey, 15105)
		{
			this.RequireTrickType = 8;
		}
	}
}
