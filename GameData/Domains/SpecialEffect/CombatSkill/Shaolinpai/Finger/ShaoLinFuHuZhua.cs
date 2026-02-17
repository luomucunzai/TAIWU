using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Finger
{
	// Token: 0x0200042E RID: 1070
	public class ShaoLinFuHuZhua : ChangePowerByEquipType
	{
		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060039A5 RID: 14757 RVA: 0x0023FB2A File Offset: 0x0023DD2A
		protected override sbyte ChangePowerUnitReverse
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x0023FB2D File Offset: 0x0023DD2D
		public ShaoLinFuHuZhua()
		{
		}

		// Token: 0x060039A7 RID: 14759 RVA: 0x0023FB37 File Offset: 0x0023DD37
		public ShaoLinFuHuZhua(CombatSkillKey skillKey) : base(skillKey, 1200)
		{
			this.AffectEquipType = 3;
		}
	}
}
