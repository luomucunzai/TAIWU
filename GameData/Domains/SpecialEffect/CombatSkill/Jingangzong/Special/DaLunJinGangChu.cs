using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Special
{
	// Token: 0x020004A7 RID: 1191
	public class DaLunJinGangChu : ReverseNext
	{
		// Token: 0x06003C9B RID: 15515 RVA: 0x0024E37E File Offset: 0x0024C57E
		public DaLunJinGangChu()
		{
		}

		// Token: 0x06003C9C RID: 15516 RVA: 0x0024E388 File Offset: 0x0024C588
		public DaLunJinGangChu(CombatSkillKey skillKey) : base(skillKey, 11304)
		{
			this.AffectSectId = 11;
			this.AffectSkillType = 10;
		}
	}
}
