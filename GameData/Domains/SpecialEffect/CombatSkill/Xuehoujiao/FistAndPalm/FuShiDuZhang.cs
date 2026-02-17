using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.FistAndPalm
{
	// Token: 0x02000233 RID: 563
	public class FuShiDuZhang : ChangePoisonLevelVariant1
	{
		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06002F8B RID: 12171 RVA: 0x002138DD File Offset: 0x00211ADD
		protected override sbyte AffectPoisonType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x002138E0 File Offset: 0x00211AE0
		public FuShiDuZhang()
		{
		}

		// Token: 0x06002F8D RID: 12173 RVA: 0x002138EA File Offset: 0x00211AEA
		public FuShiDuZhang(CombatSkillKey skillKey) : base(skillKey, 15103)
		{
		}
	}
}
