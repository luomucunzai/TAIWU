using System;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.FistAndPalm
{
	// Token: 0x02000272 RID: 626
	public class HanYinZhang : AttackBodyPart
	{
		// Token: 0x0600309B RID: 12443 RVA: 0x00217DBC File Offset: 0x00215FBC
		public HanYinZhang()
		{
		}

		// Token: 0x0600309C RID: 12444 RVA: 0x00217DC6 File Offset: 0x00215FC6
		public HanYinZhang(CombatSkillKey skillKey) : base(skillKey, 8102)
		{
			this.BodyParts = new sbyte[]
			{
				1
			};
			this.ReverseAddDamagePercent = 30;
		}

		// Token: 0x0600309D RID: 12445 RVA: 0x00217DF0 File Offset: 0x00215FF0
		protected override void OnCastAffectPower(DataContext context)
		{
			bool flag = base.CurrEnemyChar.WorsenRandomInjury(context, true);
			if (flag)
			{
				base.ShowSpecialEffectTips(1);
			}
		}
	}
}
