using System;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Throw
{
	// Token: 0x0200021C RID: 540
	public class ShiGuLanChangSha : AttackBodyPart
	{
		// Token: 0x06002F27 RID: 12071 RVA: 0x00212078 File Offset: 0x00210278
		public ShiGuLanChangSha()
		{
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x00212082 File Offset: 0x00210282
		public ShiGuLanChangSha(CombatSkillKey skillKey) : base(skillKey, 15402)
		{
			this.BodyParts = new sbyte[]
			{
				1
			};
			this.ReverseAddDamagePercent = 30;
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x002120AA File Offset: 0x002102AA
		protected override void OnCastAffectPower(DataContext context)
		{
			base.ShowSpecialEffectTips(1);
			base.ChangeStanceValue(context, base.CurrEnemyChar, -1200);
		}
	}
}
