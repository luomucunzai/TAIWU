using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.RandomEnemy
{
	// Token: 0x02000293 RID: 659
	public abstract class MinionBase : CombatSkillEffectBase
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600314C RID: 12620 RVA: 0x0021A7BF File Offset: 0x002189BF
		protected static bool CanAffect
		{
			get
			{
				return !DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(41);
			}
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x0021A7D0 File Offset: 0x002189D0
		protected MinionBase()
		{
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x0021A7DA File Offset: 0x002189DA
		protected MinionBase(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}
	}
}
