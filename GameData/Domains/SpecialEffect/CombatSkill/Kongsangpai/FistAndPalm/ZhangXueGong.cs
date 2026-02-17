using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.FistAndPalm
{
	// Token: 0x02000491 RID: 1169
	public class ZhangXueGong : PowerUpByPoison
	{
		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06003C18 RID: 15384 RVA: 0x0024C0AD File Offset: 0x0024A2AD
		protected override sbyte RequirePoisonType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06003C19 RID: 15385 RVA: 0x0024C0B0 File Offset: 0x0024A2B0
		protected override short DirectStateId
		{
			get
			{
				return 216;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06003C1A RID: 15386 RVA: 0x0024C0B7 File Offset: 0x0024A2B7
		protected override short ReverseStateId
		{
			get
			{
				return 217;
			}
		}

		// Token: 0x06003C1B RID: 15387 RVA: 0x0024C0BE File Offset: 0x0024A2BE
		public ZhangXueGong()
		{
		}

		// Token: 0x06003C1C RID: 15388 RVA: 0x0024C0C8 File Offset: 0x0024A2C8
		public ZhangXueGong(CombatSkillKey skillKey) : base(skillKey, 10102)
		{
		}
	}
}
