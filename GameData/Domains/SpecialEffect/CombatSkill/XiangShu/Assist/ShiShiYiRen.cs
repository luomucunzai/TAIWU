using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x0200032E RID: 814
	public class ShiShiYiRen : RanChenZiAssistSkillBase
	{
		// Token: 0x06003474 RID: 13428 RVA: 0x00228BDB File Offset: 0x00226DDB
		public ShiShiYiRen()
		{
		}

		// Token: 0x06003475 RID: 13429 RVA: 0x00228BE5 File Offset: 0x00226DE5
		public ShiShiYiRen(CombatSkillKey skillKey) : base(skillKey, 16415)
		{
			this.RequireBossPhase = 5;
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x00228BFC File Offset: 0x00226DFC
		protected override void ActivateEffect(DataContext context)
		{
			base.CombatChar.Immortal = true;
			DomainManager.Combat.CastSkillFree(context, base.CombatChar, 861, ECombatCastFreePriority.Normal);
			DomainManager.Combat.SetBgmIndex(2, context);
		}
	}
}
