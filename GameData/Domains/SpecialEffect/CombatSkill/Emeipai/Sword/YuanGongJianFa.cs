using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Sword
{
	// Token: 0x0200053C RID: 1340
	public class YuanGongJianFa : GetTrick
	{
		// Token: 0x06003FDC RID: 16348 RVA: 0x0025BD81 File Offset: 0x00259F81
		public YuanGongJianFa()
		{
		}

		// Token: 0x06003FDD RID: 16349 RVA: 0x0025BD8B File Offset: 0x00259F8B
		public YuanGongJianFa(CombatSkillKey skillKey) : base(skillKey, 2301)
		{
			this.GetTrickType = 4;
			this.DirectCanChangeTrickType = new sbyte[]
			{
				3,
				5
			};
		}
	}
}
