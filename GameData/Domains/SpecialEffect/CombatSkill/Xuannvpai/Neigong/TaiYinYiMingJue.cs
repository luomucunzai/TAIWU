using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Neigong
{
	// Token: 0x02000261 RID: 609
	public class TaiYinYiMingJue : GenderKeepYoung
	{
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06003055 RID: 12373 RVA: 0x00216BEC File Offset: 0x00214DEC
		protected override sbyte ReduceFatalDamageValueType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x00216BEF File Offset: 0x00214DEF
		public TaiYinYiMingJue()
		{
		}

		// Token: 0x06003057 RID: 12375 RVA: 0x00216BF9 File Offset: 0x00214DF9
		public TaiYinYiMingJue(CombatSkillKey skillKey) : base(skillKey, 8008)
		{
			this.RequireGender = 0;
		}
	}
}
