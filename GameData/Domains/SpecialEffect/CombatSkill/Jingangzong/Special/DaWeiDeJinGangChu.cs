using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Special
{
	// Token: 0x020004A8 RID: 1192
	public class DaWeiDeJinGangChu : AddPestleEffect
	{
		// Token: 0x06003C9D RID: 15517 RVA: 0x0024E3A8 File Offset: 0x0024C5A8
		public DaWeiDeJinGangChu()
		{
		}

		// Token: 0x06003C9E RID: 15518 RVA: 0x0024E3B2 File Offset: 0x0024C5B2
		public DaWeiDeJinGangChu(CombatSkillKey skillKey) : base(skillKey, 11305)
		{
			this.PestleEffectName = "CombatSkill.Jingangzong.PestleEffect.DaWeiDeJinGangChu";
		}
	}
}
