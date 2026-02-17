using System;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.FistAndPalm
{
	// Token: 0x02000398 RID: 920
	public class XueZhiZhang : AttackBodyPart
	{
		// Token: 0x0600366F RID: 13935 RVA: 0x00230E7D File Offset: 0x0022F07D
		public XueZhiZhang()
		{
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x00230E87 File Offset: 0x0022F087
		public XueZhiZhang(CombatSkillKey skillKey) : base(skillKey, 12102)
		{
			this.BodyParts = new sbyte[]
			{
				1
			};
			this.ReverseAddDamagePercent = 30;
		}

		// Token: 0x06003671 RID: 13937 RVA: 0x00230EAF File Offset: 0x0022F0AF
		protected override void OnCastAffectPower(DataContext context)
		{
			base.AbsorbStanceValue(context, base.CurrEnemyChar, 20);
			base.ShowSpecialEffectTips(1);
		}

		// Token: 0x04000FE1 RID: 4065
		private const int AbsorbStancePercent = 20;
	}
}
