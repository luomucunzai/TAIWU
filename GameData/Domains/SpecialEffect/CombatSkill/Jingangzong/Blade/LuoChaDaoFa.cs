using System;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Blade
{
	// Token: 0x020004D2 RID: 1234
	public class LuoChaDaoFa : AttackBodyPart
	{
		// Token: 0x06003D74 RID: 15732 RVA: 0x00251CA4 File Offset: 0x0024FEA4
		public LuoChaDaoFa()
		{
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x00251CAE File Offset: 0x0024FEAE
		public LuoChaDaoFa(CombatSkillKey skillKey) : base(skillKey, 11201)
		{
			this.BodyParts = new sbyte[1];
			this.ReverseAddDamagePercent = 30;
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x00251CD4 File Offset: 0x0024FED4
		protected override void OnCastAffectPower(DataContext context)
		{
			bool flag = base.CurrEnemyChar.WorsenRandomInjury(context, false);
			if (flag)
			{
				base.ShowSpecialEffectTips(1);
			}
		}
	}
}
