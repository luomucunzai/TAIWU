using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Sword
{
	// Token: 0x020001B3 RID: 435
	public class ChunJunJianQi : SwordAddFatalEffectBase
	{
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06002C50 RID: 11344 RVA: 0x00207428 File Offset: 0x00205628
		protected override IEnumerable<sbyte> RequirePersonalityTypes
		{
			get
			{
				yield return 0;
				yield return 1;
				yield break;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06002C51 RID: 11345 RVA: 0x00207447 File Offset: 0x00205647
		protected override int RequirePersonalityValue
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06002C52 RID: 11346 RVA: 0x0020744B File Offset: 0x0020564B
		protected override CValueMultiplier FlawOrAcupointCount
		{
			get
			{
				return base.EnemyChar.GetDefeatMarkCollection().GetTotalAcupointCount();
			}
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x00207462 File Offset: 0x00205662
		public ChunJunJianQi()
		{
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x0020746C File Offset: 0x0020566C
		public ChunJunJianQi(CombatSkillKey skillKey) : base(skillKey, 9106)
		{
		}
	}
}
