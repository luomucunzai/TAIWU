using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Special
{
	// Token: 0x02000542 RID: 1346
	public class LingSheCi : PowerUpByMainAttribute
	{
		// Token: 0x06003FFB RID: 16379 RVA: 0x0025C72E File Offset: 0x0025A92E
		public LingSheCi()
		{
		}

		// Token: 0x06003FFC RID: 16380 RVA: 0x0025C738 File Offset: 0x0025A938
		public LingSheCi(CombatSkillKey skillKey) : base(skillKey, 2403)
		{
			this.RequireMainAttributeType = 1;
		}
	}
}
