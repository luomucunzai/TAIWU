using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Whip
{
	// Token: 0x020003B7 RID: 951
	public class LiangYiFuChenGong : ReverseNext
	{
		// Token: 0x0600370F RID: 14095 RVA: 0x00233AC8 File Offset: 0x00231CC8
		public LiangYiFuChenGong()
		{
		}

		// Token: 0x06003710 RID: 14096 RVA: 0x00233AD2 File Offset: 0x00231CD2
		public LiangYiFuChenGong(CombatSkillKey skillKey) : base(skillKey, 4302)
		{
			this.AffectSectId = 4;
			this.AffectSkillType = 11;
		}
	}
}
