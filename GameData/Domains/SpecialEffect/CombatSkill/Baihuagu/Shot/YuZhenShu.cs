using System;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Shot
{
	// Token: 0x020005BE RID: 1470
	public class YuZhenShu : TrickBuffFlaw
	{
		// Token: 0x0600439F RID: 17311 RVA: 0x0026C0B6 File Offset: 0x0026A2B6
		public YuZhenShu()
		{
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x0026C0C0 File Offset: 0x0026A2C0
		public YuZhenShu(CombatSkillKey skillKey) : base(skillKey, 3200)
		{
			this.RequireTrickType = 2;
		}

		// Token: 0x060043A1 RID: 17313 RVA: 0x0026C0D8 File Offset: 0x0026A2D8
		protected override bool OnReverseAffect(DataContext context, int trickCount)
		{
			bool anyAffect = false;
			for (int i = 0; i < trickCount; i++)
			{
				bool flag = !context.Random.CheckPercentProb(60);
				if (!flag)
				{
					DomainManager.Combat.ChangeChangeTrickCount(context, base.CombatChar, 1, false);
					anyAffect = true;
				}
			}
			return anyAffect;
		}

		// Token: 0x04001412 RID: 5138
		private const sbyte ReverseTrickOdds = 60;
	}
}
