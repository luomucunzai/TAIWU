using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss
{
	// Token: 0x020002A3 RID: 675
	public class ShenNvHuanJian : BossNeigongBase
	{
		// Token: 0x060031B9 RID: 12729 RVA: 0x0021BF2D File Offset: 0x0021A12D
		public ShenNvHuanJian()
		{
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x0021BF37 File Offset: 0x0021A137
		public ShenNvHuanJian(CombatSkillKey skillKey) : base(skillKey, 16100)
		{
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x0021BF47 File Offset: 0x0021A147
		protected override void ActivePhase2Effect(DataContext context)
		{
			base.AppendAffectedData(context, base.CharacterId, 76, EDataModifyType.Add, -1);
			base.AppendAffectedData(context, base.CharacterId, 69, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x0021BF70 File Offset: 0x0021A170
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 76;
			int result;
			if (flag)
			{
				result = (int)ShenNvHuanJian.AddPursueOdds[dataKey.CustomParam0];
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69 && base.CombatChar.PursueAttackCount > 0;
				if (flag2)
				{
					result = 200;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000EBD RID: 3773
		private static readonly sbyte[] AddPursueOdds = new sbyte[]
		{
			80,
			60,
			40,
			20,
			20
		};

		// Token: 0x04000EBE RID: 3774
		private const int AddDamagePercent = 200;
	}
}
