using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiYiHou
{
	// Token: 0x020002C4 RID: 708
	public class SiXiangXueHuoShu : AttackExtraPart
	{
		// Token: 0x0600326D RID: 12909 RVA: 0x0021F743 File Offset: 0x0021D943
		public SiXiangXueHuoShu()
		{
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x0021F74D File Offset: 0x0021D94D
		public SiXiangXueHuoShu(CombatSkillKey skillKey) : base(skillKey, 17042)
		{
			this.AttackExtraPartCount = 1;
		}
	}
}
