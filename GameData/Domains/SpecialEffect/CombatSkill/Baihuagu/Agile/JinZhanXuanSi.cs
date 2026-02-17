using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Agile
{
	// Token: 0x020005DF RID: 1503
	public class JinZhanXuanSi : CheckHitEffect
	{
		// Token: 0x0600445F RID: 17503 RVA: 0x0026F5AC File Offset: 0x0026D7AC
		public JinZhanXuanSi()
		{
		}

		// Token: 0x06004460 RID: 17504 RVA: 0x0026F5B6 File Offset: 0x0026D7B6
		public JinZhanXuanSi(CombatSkillKey skillKey) : base(skillKey, 3405)
		{
			this.CheckHitType = 1;
		}

		// Token: 0x06004461 RID: 17505 RVA: 0x0026F5D0 File Offset: 0x0026D7D0
		protected override bool HitEffect(DataContext context)
		{
			return base.CurrEnemyChar.WorsenRandomInjury(context, WorsenConstants.HighPercent);
		}
	}
}
