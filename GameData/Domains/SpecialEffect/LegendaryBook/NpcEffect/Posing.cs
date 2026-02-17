using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.NpcEffect
{
	// Token: 0x0200014B RID: 331
	public class Posing : FeatureEffectBase
	{
		// Token: 0x06002AC3 RID: 10947 RVA: 0x00203986 File Offset: 0x00201B86
		public Posing()
		{
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x00203990 File Offset: 0x00201B90
		public Posing(int charId, short featureId) : base(charId, featureId, 41401)
		{
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x002039A1 File Offset: 0x00201BA1
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(179, EDataModifyType.TotalPercent, -1);
			base.CreateAffectedData(175, EDataModifyType.TotalPercent, -1);
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x002039C0 File Offset: 0x00201BC0
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
				bool flag2 = dataKey.FieldId == 179;
				if (flag2)
				{
					result = -50;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 175;
					if (flag3)
					{
						result = -50;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000D16 RID: 3350
		private const sbyte ChangeFrameCost = -50;

		// Token: 0x04000D17 RID: 3351
		private const sbyte ChangeMoveCost = -50;
	}
}
