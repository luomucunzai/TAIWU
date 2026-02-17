using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.FistAndPalm
{
	// Token: 0x02000234 RID: 564
	public class QiShiErDiShaQuan : AddBreakBodyFeature
	{
		// Token: 0x06002F8E RID: 12174 RVA: 0x002138FA File Offset: 0x00211AFA
		public QiShiErDiShaQuan()
		{
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x00213904 File Offset: 0x00211B04
		public QiShiErDiShaQuan(CombatSkillKey skillKey) : base(skillKey, 15102)
		{
			this.AffectBodyParts = new sbyte[]
			{
				5,
				6
			};
		}
	}
}
