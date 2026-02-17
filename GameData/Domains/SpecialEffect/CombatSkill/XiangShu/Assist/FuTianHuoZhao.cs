using System;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x02000323 RID: 803
	public class FuTianHuoZhao : RanChenZiAssistSkillBase
	{
		// Token: 0x0600343E RID: 13374 RVA: 0x00228215 File Offset: 0x00226415
		public FuTianHuoZhao()
		{
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x0022821F File Offset: 0x0022641F
		public FuTianHuoZhao(CombatSkillKey skillKey) : base(skillKey, 16413)
		{
			this.RequireBossPhase = 3;
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x00228238 File Offset: 0x00226438
		protected override void ActivateEffect(DataContext context)
		{
			base.CombatChar.LockMaxBreath = true;
			base.CombatChar.LockMaxStance = true;
			base.ChangeBreathValue(context, base.CombatChar, 30000);
			base.ChangeStanceValue(context, base.CombatChar, 4000);
			DomainManager.Combat.SetBgmIndex(1, context);
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x00228291 File Offset: 0x00226491
		protected override void DeactivateEffect(DataContext context)
		{
			base.CombatChar.LockMaxBreath = false;
			base.CombatChar.LockMaxStance = false;
		}
	}
}
