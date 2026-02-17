using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Profession.Savage
{
	// Token: 0x0200010E RID: 270
	public class LingJueDing : ProfessionEffectBase
	{
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060029E8 RID: 10728 RVA: 0x00201811 File Offset: 0x001FFA11
		protected override short CombatStateId
		{
			get
			{
				return 131;
			}
		}

		// Token: 0x060029E9 RID: 10729 RVA: 0x00201818 File Offset: 0x001FFA18
		public LingJueDing()
		{
		}

		// Token: 0x060029EA RID: 10730 RVA: 0x00201822 File Offset: 0x001FFA22
		public LingJueDing(int charId) : base(charId)
		{
		}

		// Token: 0x060029EB RID: 10731 RVA: 0x0020182D File Offset: 0x001FFA2D
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(283, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x060029EC RID: 10732 RVA: 0x00201848 File Offset: 0x001FFA48
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 283;
				if (flag2)
				{
					result = -20;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000CDB RID: 3291
		private const sbyte AddPercent = -20;
	}
}
