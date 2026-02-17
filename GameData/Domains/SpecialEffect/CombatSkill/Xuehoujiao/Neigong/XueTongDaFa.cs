using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Neigong
{
	// Token: 0x02000227 RID: 551
	public class XueTongDaFa : ChangeNeiliAllocation
	{
		// Token: 0x06002F4F RID: 12111 RVA: 0x0021272A File Offset: 0x0021092A
		public XueTongDaFa()
		{
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x00212734 File Offset: 0x00210934
		public XueTongDaFa(CombatSkillKey skillKey) : base(skillKey, 15008)
		{
			this.AffectNeiliAllocationType = 0;
		}
	}
}
