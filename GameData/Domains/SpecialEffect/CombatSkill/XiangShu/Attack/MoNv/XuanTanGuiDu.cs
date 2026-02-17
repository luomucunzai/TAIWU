using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.MoNv
{
	// Token: 0x020002F7 RID: 759
	public class XuanTanGuiDu : AddPoison
	{
		// Token: 0x06003389 RID: 13193 RVA: 0x002255F7 File Offset: 0x002237F7
		public XuanTanGuiDu()
		{
		}

		// Token: 0x0600338A RID: 13194 RVA: 0x00225601 File Offset: 0x00223801
		public XuanTanGuiDu(CombatSkillKey skillKey) : base(skillKey, 17004)
		{
			this.PoisonTypeCount = 6;
		}
	}
}
