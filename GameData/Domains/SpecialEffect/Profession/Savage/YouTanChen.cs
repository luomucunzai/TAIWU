using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Profession.Savage
{
	// Token: 0x02000115 RID: 277
	public class YouTanChen : ProfessionEffectBase
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06002A0A RID: 10762 RVA: 0x00201B4A File Offset: 0x001FFD4A
		protected override short CombatStateId
		{
			get
			{
				return 140;
			}
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x00201B51 File Offset: 0x001FFD51
		public YouTanChen()
		{
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x00201B5B File Offset: 0x001FFD5B
		public YouTanChen(int charId) : base(charId)
		{
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x00201B66 File Offset: 0x001FFD66
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(72, EDataModifyType.Add, -1);
			base.CreateAffectedData(73, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x00201B88 File Offset: 0x001FFD88
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
				bool flag2 = dataKey.FieldId == 72;
				if (flag2)
				{
					result = 1;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 73;
					if (flag3)
					{
						result = 50;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000CE3 RID: 3299
		private const sbyte AddPoisonLevel = 1;

		// Token: 0x04000CE4 RID: 3300
		private const sbyte AddPoisonPercent = 50;
	}
}
