using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.NoSect.Neigong
{
	// Token: 0x02000471 RID: 1137
	public class PeiRanJue : CombatSkillEffectBase
	{
		// Token: 0x06003B47 RID: 15175 RVA: 0x00247513 File Offset: 0x00245713
		public PeiRanJue()
		{
		}

		// Token: 0x06003B48 RID: 15176 RVA: 0x0024751D File Offset: 0x0024571D
		public PeiRanJue(CombatSkillKey skillKey) : base(skillKey, 0, -1)
		{
		}

		// Token: 0x06003B49 RID: 15177 RVA: 0x0024752A File Offset: 0x0024572A
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(117, EDataModifyType.Custom, -1);
		}

		// Token: 0x06003B4A RID: 15178 RVA: 0x00247538 File Offset: 0x00245738
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 117;
			int result;
			if (flag)
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			else
			{
				result = dataValue * PeiRanJue.ReduceGongMadInjury;
			}
			return result;
		}

		// Token: 0x0400115D RID: 4445
		private static readonly CValuePercentBonus ReduceGongMadInjury = -75;
	}
}
