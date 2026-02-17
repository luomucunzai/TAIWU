using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Agile
{
	// Token: 0x0200040F RID: 1039
	public class TieQiaoGong : AgileSkillBase
	{
		// Token: 0x06003904 RID: 14596 RVA: 0x0023CC9A File Offset: 0x0023AE9A
		public TieQiaoGong()
		{
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x0023CCA4 File Offset: 0x0023AEA4
		public TieQiaoGong(CombatSkillKey skillKey) : base(skillKey, 6401)
		{
		}

		// Token: 0x06003906 RID: 14598 RVA: 0x0023CCB4 File Offset: 0x0023AEB4
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 107, -1, -1, -1, -1), EDataModifyType.TotalPercent);
		}

		// Token: 0x06003907 RID: 14599 RVA: 0x0023CCE8 File Offset: 0x0023AEE8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				short currDistance = DomainManager.Combat.GetCurrentDistance();
				int unit = (int)(base.IsDirect ? ((currDistance - 40) / 10) : ((80 - currDistance) / 10));
				bool flag2 = unit <= 0;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 107;
					if (flag3)
					{
						base.ShowSpecialEffectTips(0);
						result = -10 * (Math.Abs((int)(currDistance - 40)) / 10 + 1);
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x040010AC RID: 4268
		private const sbyte DirectRequireDistance = 40;

		// Token: 0x040010AD RID: 4269
		private const sbyte ReverseRequireDistance = 80;

		// Token: 0x040010AE RID: 4270
		private const sbyte ReduceHitOddsUnit = -10;
	}
}
