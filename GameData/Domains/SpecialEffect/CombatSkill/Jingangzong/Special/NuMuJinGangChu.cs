using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Special
{
	// Token: 0x020004AB RID: 1195
	public class NuMuJinGangChu : AddPestleEffect
	{
		// Token: 0x06003CA7 RID: 15527 RVA: 0x0024E58C File Offset: 0x0024C78C
		public NuMuJinGangChu()
		{
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x0024E596 File Offset: 0x0024C796
		public NuMuJinGangChu(CombatSkillKey skillKey) : base(skillKey, 11303)
		{
			this.PestleEffectName = "CombatSkill.Jingangzong.PestleEffect.NuMuJinGangChu";
		}
	}
}
