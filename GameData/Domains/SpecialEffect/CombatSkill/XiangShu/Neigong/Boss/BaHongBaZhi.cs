using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss
{
	// Token: 0x0200029A RID: 666
	public class BaHongBaZhi : BossNeigongBase
	{
		// Token: 0x06003187 RID: 12679 RVA: 0x0021B50F File Offset: 0x0021970F
		public BaHongBaZhi()
		{
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x0021B519 File Offset: 0x00219719
		public BaHongBaZhi(CombatSkillKey skillKey) : base(skillKey, 16107)
		{
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x0021B529 File Offset: 0x00219729
		protected override void ActivePhase2Effect(DataContext context)
		{
			base.AppendAffectedData(context, base.CharacterId, 69, EDataModifyType.TotalPercent, -1);
			base.AppendAffectedData(context, base.CharacterId, 102, EDataModifyType.TotalPercent, -1);
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x0021B550 File Offset: 0x00219750
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = ((dataKey.FieldId == 69) ? 120 : -60);
			}
			return result;
		}

		// Token: 0x04000EB2 RID: 3762
		private const sbyte AddDamage = 120;

		// Token: 0x04000EB3 RID: 3763
		private const sbyte ReduceDamage = -60;
	}
}
