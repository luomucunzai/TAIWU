using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Profession.Savage
{
	// Token: 0x02000114 RID: 276
	public class YiXianTian : ProfessionEffectBase
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06002A05 RID: 10757 RVA: 0x00201AD3 File Offset: 0x001FFCD3
		protected override short CombatStateId
		{
			get
			{
				return 132;
			}
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x00201ADA File Offset: 0x001FFCDA
		public YiXianTian()
		{
		}

		// Token: 0x06002A07 RID: 10759 RVA: 0x00201AE4 File Offset: 0x001FFCE4
		public YiXianTian(int charId) : base(charId)
		{
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x00201AEF File Offset: 0x001FFCEF
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(194, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x00201B08 File Offset: 0x001FFD08
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
				bool flag2 = dataKey.FieldId == 194;
				if (flag2)
				{
					result = 20;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000CE2 RID: 3298
		private const sbyte AddPercent = 20;
	}
}
