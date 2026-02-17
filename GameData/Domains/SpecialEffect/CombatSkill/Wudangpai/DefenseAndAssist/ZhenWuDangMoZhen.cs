using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.DefenseAndAssist
{
	// Token: 0x020003DE RID: 990
	public class ZhenWuDangMoZhen : BuffTeammateCommand
	{
		// Token: 0x060037EC RID: 14316 RVA: 0x00237E78 File Offset: 0x00236078
		public ZhenWuDangMoZhen()
		{
		}

		// Token: 0x060037ED RID: 14317 RVA: 0x00237E82 File Offset: 0x00236082
		public ZhenWuDangMoZhen(CombatSkillKey skillKey) : base(skillKey, 4605)
		{
			this.AffectTeammateCommandType = new ETeammateCommandImplement[]
			{
				ETeammateCommandImplement.TransferNeiliAllocation
			};
			this.CommandPowerUpPercent = 100;
		}
	}
}
