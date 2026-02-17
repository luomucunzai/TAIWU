using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Sword
{
	// Token: 0x020003C0 RID: 960
	public class WuDangDanJian : ChangePowerByEquipType
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06003744 RID: 14148 RVA: 0x00234BAC File Offset: 0x00232DAC
		protected override sbyte ChangePowerUnitReverse
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06003745 RID: 14149 RVA: 0x00234BAF File Offset: 0x00232DAF
		public WuDangDanJian()
		{
		}

		// Token: 0x06003746 RID: 14150 RVA: 0x00234BB9 File Offset: 0x00232DB9
		public WuDangDanJian(CombatSkillKey skillKey) : base(skillKey, 4200)
		{
			this.AffectEquipType = 4;
		}
	}
}
