using System;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Finger
{
	// Token: 0x02000495 RID: 1173
	public class TieHuaFuGuZhua : AttackBodyPart
	{
		// Token: 0x06003C27 RID: 15399 RVA: 0x0024C272 File Offset: 0x0024A472
		public TieHuaFuGuZhua()
		{
		}

		// Token: 0x06003C28 RID: 15400 RVA: 0x0024C27C File Offset: 0x0024A47C
		public TieHuaFuGuZhua(CombatSkillKey skillKey) : base(skillKey, 10202)
		{
			this.BodyParts = new sbyte[1];
			this.ReverseAddDamagePercent = 30;
		}

		// Token: 0x06003C29 RID: 15401 RVA: 0x0024C2A0 File Offset: 0x0024A4A0
		protected override void OnCastAffectPower(DataContext context)
		{
			base.AbsorbBreathValue(context, base.CurrEnemyChar, 20);
			base.ShowSpecialEffectTips(1);
		}

		// Token: 0x040011B0 RID: 4528
		private const int AbsorbBreathPercent = 20;
	}
}
