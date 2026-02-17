using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.DefenseAndAssist
{
	// Token: 0x02000208 RID: 520
	public class TaiBuXuanDian : TrickAddHitOrAvoid
	{
		// Token: 0x06002ECA RID: 11978 RVA: 0x00210AD1 File Offset: 0x0020ECD1
		public TaiBuXuanDian()
		{
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x00210ADB File Offset: 0x0020ECDB
		public TaiBuXuanDian(CombatSkillKey skillKey) : base(skillKey, 5605)
		{
			this.RequireTrickTypes = new sbyte[]
			{
				3,
				5,
				4
			};
		}
	}
}
