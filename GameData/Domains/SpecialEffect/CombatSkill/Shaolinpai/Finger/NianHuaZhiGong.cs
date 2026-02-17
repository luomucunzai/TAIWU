using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Finger
{
	// Token: 0x0200042C RID: 1068
	public class NianHuaZhiGong : GameData.Domains.SpecialEffect.CombatSkill.Common.Attack.AttackHitType
	{
		// Token: 0x06003999 RID: 14745 RVA: 0x0023F67F File Offset: 0x0023D87F
		public NianHuaZhiGong()
		{
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x0023F689 File Offset: 0x0023D889
		public NianHuaZhiGong(CombatSkillKey skillKey) : base(skillKey, 1202)
		{
			this.AffectHitType = 1;
		}
	}
}
