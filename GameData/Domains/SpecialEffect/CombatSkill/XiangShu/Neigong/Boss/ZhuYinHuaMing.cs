using System;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss
{
	// Token: 0x020002A8 RID: 680
	public class ZhuYinHuaMing : BossNeigongBase
	{
		// Token: 0x060031D4 RID: 12756 RVA: 0x0021C5B4 File Offset: 0x0021A7B4
		public ZhuYinHuaMing()
		{
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x0021C5BE File Offset: 0x0021A7BE
		public ZhuYinHuaMing(CombatSkillKey skillKey) : base(skillKey, 16105)
		{
		}

		// Token: 0x060031D6 RID: 12758 RVA: 0x0021C5CE File Offset: 0x0021A7CE
		protected override void ActivePhase2Effect(DataContext context)
		{
			base.CombatChar.OuterInjuryAutoHealSpeeds.Add(2);
			base.CombatChar.InnerInjuryAutoHealSpeeds.Add(2);
		}

		// Token: 0x04000EC5 RID: 3781
		private const sbyte AutoHealSpeed = 2;
	}
}
