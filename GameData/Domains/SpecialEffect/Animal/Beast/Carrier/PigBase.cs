using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x02000638 RID: 1592
	public abstract class PigBase : CarrierEffectBase
	{
		// Token: 0x17000315 RID: 789
		// (get) Token: 0x0600463A RID: 17978
		protected abstract CValuePercent AddCriticalOddsPercent { get; }

		// Token: 0x0600463B RID: 17979 RVA: 0x00274290 File Offset: 0x00272490
		protected PigBase()
		{
		}

		// Token: 0x0600463C RID: 17980 RVA: 0x0027429A File Offset: 0x0027249A
		protected PigBase(int charId) : base(charId)
		{
		}

		// Token: 0x0600463D RID: 17981 RVA: 0x002742A5 File Offset: 0x002724A5
		protected override void OnEnableSubClass(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 254, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x0600463E RID: 17982 RVA: 0x002742D4 File Offset: 0x002724D4
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
				bool flag2 = dataKey.FieldId == 254;
				if (flag2)
				{
					result = (int)(140 - DomainManager.Combat.GetCurrentDistance()) * this.AddCriticalOddsPercent;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}
	}
}
