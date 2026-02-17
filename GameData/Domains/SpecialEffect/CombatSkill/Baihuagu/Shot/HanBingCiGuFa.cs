using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Shot
{
	// Token: 0x020005B8 RID: 1464
	public class HanBingCiGuFa : PowerUpByPoison
	{
		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06004385 RID: 17285 RVA: 0x0026BC13 File Offset: 0x00269E13
		protected override sbyte RequirePoisonType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06004386 RID: 17286 RVA: 0x0026BC16 File Offset: 0x00269E16
		protected override short DirectStateId
		{
			get
			{
				return 214;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06004387 RID: 17287 RVA: 0x0026BC1D File Offset: 0x00269E1D
		protected override short ReverseStateId
		{
			get
			{
				return 215;
			}
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x0026BC24 File Offset: 0x00269E24
		public HanBingCiGuFa()
		{
		}

		// Token: 0x06004389 RID: 17289 RVA: 0x0026BC2E File Offset: 0x00269E2E
		public HanBingCiGuFa(CombatSkillKey skillKey) : base(skillKey, 3202)
		{
		}
	}
}
