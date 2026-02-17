using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Carrier
{
	// Token: 0x02000613 RID: 1555
	public class Chaofeng : CarrierEffectBase
	{
		// Token: 0x06004572 RID: 17778 RVA: 0x002725F7 File Offset: 0x002707F7
		public Chaofeng(int charId) : base(charId)
		{
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06004573 RID: 17779 RVA: 0x00272602 File Offset: 0x00270802
		protected override short CombatStateId
		{
			get
			{
				return 201;
			}
		}

		// Token: 0x06004574 RID: 17780 RVA: 0x00272609 File Offset: 0x00270809
		protected override void OnEnableSubClass(DataContext context)
		{
			base.CreateAffectedData(197, EDataModifyType.TotalPercent, -1);
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x0027261C File Offset: 0x0027081C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 197;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = 50;
			}
			return result;
		}

		// Token: 0x04001488 RID: 5256
		private const int MobilityRecoverSpeedAddPercent = 50;
	}
}
