using System;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Polearm
{
	// Token: 0x02000417 RID: 1047
	public class ShiBaDianQiMeiGun : TrickBuffFlaw
	{
		// Token: 0x06003935 RID: 14645 RVA: 0x0023DB2C File Offset: 0x0023BD2C
		public ShiBaDianQiMeiGun()
		{
		}

		// Token: 0x06003936 RID: 14646 RVA: 0x0023DB36 File Offset: 0x0023BD36
		public ShiBaDianQiMeiGun(CombatSkillKey skillKey) : base(skillKey, 1302)
		{
			this.RequireTrickType = 5;
		}

		// Token: 0x06003937 RID: 14647 RVA: 0x0023DB50 File Offset: 0x0023BD50
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

		// Token: 0x040010BE RID: 4286
		private const sbyte ReverseTrickOdds = 60;
	}
}
