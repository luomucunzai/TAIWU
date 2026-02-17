using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Special
{
	// Token: 0x020004A6 RID: 1190
	public class BuDongMingWangChu : AddPestleEffect
	{
		// Token: 0x06003C99 RID: 15513 RVA: 0x0024E359 File Offset: 0x0024C559
		public BuDongMingWangChu()
		{
		}

		// Token: 0x06003C9A RID: 15514 RVA: 0x0024E363 File Offset: 0x0024C563
		public BuDongMingWangChu(CombatSkillKey skillKey) : base(skillKey, 11307)
		{
			this.PestleEffectName = "CombatSkill.Jingangzong.PestleEffect.BuDongMingWangChu";
		}
	}
}
