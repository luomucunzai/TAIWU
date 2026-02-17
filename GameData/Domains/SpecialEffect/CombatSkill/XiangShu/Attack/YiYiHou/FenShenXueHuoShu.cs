using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiYiHou
{
	// Token: 0x020002C2 RID: 706
	public class FenShenXueHuoShu : AttackExtraPart
	{
		// Token: 0x06003269 RID: 12905 RVA: 0x0021F701 File Offset: 0x0021D901
		public FenShenXueHuoShu()
		{
		}

		// Token: 0x0600326A RID: 12906 RVA: 0x0021F70B File Offset: 0x0021D90B
		public FenShenXueHuoShu(CombatSkillKey skillKey) : base(skillKey, 17045)
		{
			this.AttackExtraPartCount = 3;
		}
	}
}
