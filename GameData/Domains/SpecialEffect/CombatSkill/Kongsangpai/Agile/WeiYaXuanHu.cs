using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Agile
{
	// Token: 0x020004A5 RID: 1189
	public class WeiYaXuanHu : AttackChangeMobility
	{
		// Token: 0x06003C97 RID: 15511 RVA: 0x0024E337 File Offset: 0x0024C537
		public WeiYaXuanHu()
		{
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x0024E341 File Offset: 0x0024C541
		public WeiYaXuanHu(CombatSkillKey skillKey) : base(skillKey, 10501)
		{
			this.RequireWeaponSubType = 14;
		}
	}
}
