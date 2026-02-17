using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Shot
{
	// Token: 0x020005B9 RID: 1465
	public class HuaMaiShenZhen : PoisonAddInjury
	{
		// Token: 0x0600438A RID: 17290 RVA: 0x0026BC3E File Offset: 0x00269E3E
		public HuaMaiShenZhen()
		{
		}

		// Token: 0x0600438B RID: 17291 RVA: 0x0026BC48 File Offset: 0x00269E48
		public HuaMaiShenZhen(CombatSkillKey skillKey) : base(skillKey, 3207)
		{
			this.RequirePoisonType = 1;
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x0026BC60 File Offset: 0x00269E60
		protected override void OnCastMaxPower(DataContext context)
		{
			CombatCharacter poisonChar = base.IsDirect ? base.EnemyChar : base.CombatChar;
			byte unit = poisonChar.GetDefeatMarkCollection().PoisonMarkList[(int)this.RequirePoisonType];
			bool flag = unit <= 0;
			if (!flag)
			{
				for (byte i = 0; i < 4; i += 1)
				{
					base.EnemyChar.ChangeNeiliAllocation(context, i, (int)unit * -10, true, true);
				}
				base.EnemyChar.SilenceNeiliAllocationAutoRecover(context, (int)unit * 400);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x0400140B RID: 5131
		private const int CostNeiliAllocationUnit = -10;

		// Token: 0x0400140C RID: 5132
		private const int SilenceNeiliAllocationUnit = 400;
	}
}
