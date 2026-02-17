using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Finger
{
	// Token: 0x020005CE RID: 1486
	public class DaHuaManTuoLuoZhi : PowerUpByPoison
	{
		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060043FD RID: 17405 RVA: 0x0026DB30 File Offset: 0x0026BD30
		protected override sbyte RequirePoisonType
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060043FE RID: 17406 RVA: 0x0026DB33 File Offset: 0x0026BD33
		protected override short DirectStateId
		{
			get
			{
				return 220;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060043FF RID: 17407 RVA: 0x0026DB3A File Offset: 0x0026BD3A
		protected override short ReverseStateId
		{
			get
			{
				return 221;
			}
		}

		// Token: 0x06004400 RID: 17408 RVA: 0x0026DB41 File Offset: 0x0026BD41
		public DaHuaManTuoLuoZhi()
		{
		}

		// Token: 0x06004401 RID: 17409 RVA: 0x0026DB4B File Offset: 0x0026BD4B
		public DaHuaManTuoLuoZhi(CombatSkillKey skillKey) : base(skillKey, 3105)
		{
		}
	}
}
