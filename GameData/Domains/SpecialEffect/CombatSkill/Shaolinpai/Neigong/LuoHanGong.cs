using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Neigong
{
	// Token: 0x0200041C RID: 1052
	public class LuoHanGong : StrengthenFiveElementsTypeSimple
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x0600394B RID: 14667 RVA: 0x0023E0B5 File Offset: 0x0023C2B5
		protected override sbyte FiveElementsType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600394C RID: 14668 RVA: 0x0023E0B8 File Offset: 0x0023C2B8
		public LuoHanGong()
		{
		}

		// Token: 0x0600394D RID: 14669 RVA: 0x0023E0C2 File Offset: 0x0023C2C2
		public LuoHanGong(CombatSkillKey skillKey) : base(skillKey, 1001)
		{
		}
	}
}
