using System;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Finger
{
	// Token: 0x02000556 RID: 1366
	public class BoDaShiBaShi : TrickBuffFlaw
	{
		// Token: 0x0600406C RID: 16492 RVA: 0x0025E1B8 File Offset: 0x0025C3B8
		public BoDaShiBaShi()
		{
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x0025E1C2 File Offset: 0x0025C3C2
		public BoDaShiBaShi(CombatSkillKey skillKey) : base(skillKey, 2202)
		{
			this.RequireTrickType = 8;
		}

		// Token: 0x0600406E RID: 16494 RVA: 0x0025E1DC File Offset: 0x0025C3DC
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

		// Token: 0x040012EF RID: 4847
		private const sbyte ReverseTrickOdds = 60;
	}
}
