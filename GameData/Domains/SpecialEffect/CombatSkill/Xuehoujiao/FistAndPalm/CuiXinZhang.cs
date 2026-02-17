using System;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.FistAndPalm
{
	// Token: 0x02000231 RID: 561
	public class CuiXinZhang : AttackBodyPart
	{
		// Token: 0x06002F80 RID: 12160 RVA: 0x0021361D File Offset: 0x0021181D
		public CuiXinZhang()
		{
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x00213627 File Offset: 0x00211827
		public CuiXinZhang(CombatSkillKey skillKey) : base(skillKey, 15101)
		{
			this.BodyParts = new sbyte[1];
			this.ReverseAddDamagePercent = 30;
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x0021364B File Offset: 0x0021184B
		protected override void OnCastAffectPower(DataContext context)
		{
			base.ShowSpecialEffectTips(1);
			base.ChangeBreathValue(context, base.CurrEnemyChar, -9000);
		}
	}
}
