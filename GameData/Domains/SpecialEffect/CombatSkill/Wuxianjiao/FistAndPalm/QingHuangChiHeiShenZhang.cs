using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.FistAndPalm
{
	// Token: 0x02000395 RID: 917
	public class QingHuangChiHeiShenZhang : StrengthenPoison
	{
		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600365C RID: 13916 RVA: 0x00230871 File Offset: 0x0022EA71
		protected override bool Variant1
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x00230874 File Offset: 0x0022EA74
		public QingHuangChiHeiShenZhang()
		{
		}

		// Token: 0x0600365E RID: 13918 RVA: 0x0023087E File Offset: 0x0022EA7E
		public QingHuangChiHeiShenZhang(CombatSkillKey skillKey) : base(skillKey, 12106)
		{
			this.AffectPoisonType = 4;
		}
	}
}
