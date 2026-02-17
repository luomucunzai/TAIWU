using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x0200062C RID: 1580
	public abstract class EagleBase : CarrierEffectBase
	{
		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06004605 RID: 17925
		protected abstract CValuePercent AddCriticalOddsPercent { get; }

		// Token: 0x06004606 RID: 17926 RVA: 0x00273E68 File Offset: 0x00272068
		protected EagleBase()
		{
		}

		// Token: 0x06004607 RID: 17927 RVA: 0x00273E72 File Offset: 0x00272072
		protected EagleBase(int charId) : base(charId)
		{
		}

		// Token: 0x06004608 RID: 17928 RVA: 0x00273E7D File Offset: 0x0027207D
		protected override void OnEnableSubClass(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 254, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06004609 RID: 17929 RVA: 0x00273EAC File Offset: 0x002720AC
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
					result = (int)DomainManager.Combat.GetCurrentDistance() * this.AddCriticalOddsPercent;
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
