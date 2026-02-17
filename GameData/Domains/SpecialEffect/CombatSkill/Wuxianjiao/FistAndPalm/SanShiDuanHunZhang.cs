using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.FistAndPalm
{
	// Token: 0x02000397 RID: 919
	public class SanShiDuanHunZhang : ChangePoisonType
	{
		// Token: 0x0600366D RID: 13933 RVA: 0x00230E48 File Offset: 0x0022F048
		public SanShiDuanHunZhang()
		{
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x00230E52 File Offset: 0x0022F052
		public SanShiDuanHunZhang(CombatSkillKey skillKey) : base(skillKey, 12103)
		{
			this.CanChangePoisonType = new sbyte[]
			{
				0,
				3,
				4
			};
			this.AddPowerPoisonType = 4;
		}
	}
}
