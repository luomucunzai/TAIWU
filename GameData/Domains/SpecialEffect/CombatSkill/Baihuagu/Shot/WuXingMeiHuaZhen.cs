using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Shot
{
	// Token: 0x020005BD RID: 1469
	public class WuXingMeiHuaZhen : GetTrick
	{
		// Token: 0x0600439D RID: 17309 RVA: 0x0026C085 File Offset: 0x0026A285
		public WuXingMeiHuaZhen()
		{
		}

		// Token: 0x0600439E RID: 17310 RVA: 0x0026C08F File Offset: 0x0026A28F
		public WuXingMeiHuaZhen(CombatSkillKey skillKey) : base(skillKey, 3201)
		{
			this.GetTrickType = 2;
			this.DirectCanChangeTrickType = new sbyte[]
			{
				0,
				1
			};
		}
	}
}
