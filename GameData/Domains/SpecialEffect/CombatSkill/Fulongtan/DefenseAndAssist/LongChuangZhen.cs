using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.DefenseAndAssist
{
	// Token: 0x02000523 RID: 1315
	public class LongChuangZhen : BuffTeammateCommand
	{
		// Token: 0x06003F32 RID: 16178 RVA: 0x00258E55 File Offset: 0x00257055
		public LongChuangZhen()
		{
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x00258E5F File Offset: 0x0025705F
		public LongChuangZhen(CombatSkillKey skillKey) : base(skillKey, 14603)
		{
			this.AffectTeammateCommandType = new ETeammateCommandImplement[]
			{
				ETeammateCommandImplement.AttackSkill
			};
			this.CommandPowerUpPercent = 50;
		}
	}
}
