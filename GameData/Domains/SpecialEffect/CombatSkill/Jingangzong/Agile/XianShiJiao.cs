using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Agile
{
	// Token: 0x020004D9 RID: 1241
	public class XianShiJiao : ChangeAttackHitType
	{
		// Token: 0x06003D97 RID: 15767 RVA: 0x0025276E File Offset: 0x0025096E
		public XianShiJiao()
		{
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x00252778 File Offset: 0x00250978
		public XianShiJiao(CombatSkillKey skillKey) : base(skillKey, 11500)
		{
			this.HitType = 0;
		}
	}
}
