using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Sword
{
	// Token: 0x020001B5 RID: 437
	public class JuQueQianJunJian : SwordAddFatalEffectBase
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06002C5B RID: 11355 RVA: 0x002074C4 File Offset: 0x002056C4
		protected override IEnumerable<sbyte> RequirePersonalityTypes
		{
			get
			{
				yield return 2;
				yield return 3;
				yield break;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06002C5C RID: 11356 RVA: 0x002074E3 File Offset: 0x002056E3
		protected override int RequirePersonalityValue
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06002C5D RID: 11357 RVA: 0x002074E7 File Offset: 0x002056E7
		protected override CValueMultiplier FlawOrAcupointCount
		{
			get
			{
				return base.EnemyChar.GetDefeatMarkCollection().GetTotalFlawCount();
			}
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x002074FE File Offset: 0x002056FE
		public JuQueQianJunJian()
		{
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x00207508 File Offset: 0x00205708
		public JuQueQianJunJian(CombatSkillKey skillKey) : base(skillKey, 9103)
		{
		}
	}
}
