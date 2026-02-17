using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Neigong
{
	// Token: 0x02000420 RID: 1056
	public class XiSuiJing : GenderKeepYoung
	{
		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06003955 RID: 14677 RVA: 0x0023E15C File Offset: 0x0023C35C
		protected override sbyte ReduceFatalDamageValueType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06003956 RID: 14678 RVA: 0x0023E15F File Offset: 0x0023C35F
		public XiSuiJing()
		{
		}

		// Token: 0x06003957 RID: 14679 RVA: 0x0023E169 File Offset: 0x0023C369
		public XiSuiJing(CombatSkillKey skillKey) : base(skillKey, 1008)
		{
			this.RequireGender = 1;
		}
	}
}
