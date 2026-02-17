using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Neigong
{
	// Token: 0x02000515 RID: 1301
	public class KuangLongTunXiangGong : TransferFiveElementsNeili
	{
		// Token: 0x06003EEA RID: 16106 RVA: 0x002577C6 File Offset: 0x002559C6
		public KuangLongTunXiangGong()
		{
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x002577D0 File Offset: 0x002559D0
		public KuangLongTunXiangGong(CombatSkillKey skillKey) : base(skillKey, 14004)
		{
			this.SrcFiveElementsType = (base.IsDirect ? 4 : 0);
			this.DestFiveElementsType = 3;
		}
	}
}
