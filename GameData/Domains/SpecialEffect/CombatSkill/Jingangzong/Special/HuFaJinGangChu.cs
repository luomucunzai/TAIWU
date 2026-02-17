using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Special
{
	// Token: 0x020004A9 RID: 1193
	public class HuFaJinGangChu : AddPestleEffect
	{
		// Token: 0x06003C9F RID: 15519 RVA: 0x0024E3CD File Offset: 0x0024C5CD
		public HuFaJinGangChu()
		{
		}

		// Token: 0x06003CA0 RID: 15520 RVA: 0x0024E3D7 File Offset: 0x0024C5D7
		public HuFaJinGangChu(CombatSkillKey skillKey) : base(skillKey, 11301)
		{
			this.PestleEffectName = "CombatSkill.Jingangzong.PestleEffect.HuFaJinGangChu";
		}
	}
}
