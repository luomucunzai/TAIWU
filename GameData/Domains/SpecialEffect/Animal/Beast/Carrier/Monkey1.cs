using System;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x02000634 RID: 1588
	public class Monkey1 : MonkeyBase
	{
		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06004629 RID: 17961 RVA: 0x0027414D File Offset: 0x0027234D
		protected override short CombatStateId
		{
			get
			{
				return 157;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x0600462A RID: 17962 RVA: 0x00274154 File Offset: 0x00272354
		protected override int PowerAddOrReduceRatio
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x0600462B RID: 17963 RVA: 0x00274158 File Offset: 0x00272358
		public Monkey1()
		{
		}

		// Token: 0x0600462C RID: 17964 RVA: 0x00274162 File Offset: 0x00272362
		public Monkey1(int charId) : base(charId)
		{
		}
	}
}
