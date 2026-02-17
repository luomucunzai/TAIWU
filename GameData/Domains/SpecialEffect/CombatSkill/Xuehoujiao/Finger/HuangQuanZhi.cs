using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Finger
{
	// Token: 0x0200023B RID: 571
	public class HuangQuanZhi : PowerUpByPoison
	{
		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06002FA0 RID: 12192 RVA: 0x00213BCD File Offset: 0x00211DCD
		protected override sbyte RequirePoisonType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06002FA1 RID: 12193 RVA: 0x00213BD0 File Offset: 0x00211DD0
		protected override short DirectStateId
		{
			get
			{
				return 218;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06002FA2 RID: 12194 RVA: 0x00213BD7 File Offset: 0x00211DD7
		protected override short ReverseStateId
		{
			get
			{
				return 219;
			}
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x00213BDE File Offset: 0x00211DDE
		public HuangQuanZhi()
		{
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x00213BE8 File Offset: 0x00211DE8
		public HuangQuanZhi(CombatSkillKey skillKey) : base(skillKey, 15205)
		{
		}
	}
}
