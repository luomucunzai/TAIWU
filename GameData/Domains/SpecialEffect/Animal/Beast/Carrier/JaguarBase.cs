using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x0200062F RID: 1583
	public abstract class JaguarBase : CarrierEffectBase
	{
		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06004612 RID: 17938
		protected abstract int FightBackPower { get; }

		// Token: 0x06004613 RID: 17939 RVA: 0x00273F41 File Offset: 0x00272141
		protected JaguarBase()
		{
		}

		// Token: 0x06004614 RID: 17940 RVA: 0x00273F4B File Offset: 0x0027214B
		protected JaguarBase(int charId) : base(charId)
		{
		}

		// Token: 0x06004615 RID: 17941 RVA: 0x00273F56 File Offset: 0x00272156
		protected override void OnEnableSubClass(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 112, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06004616 RID: 17942 RVA: 0x00273F84 File Offset: 0x00272184
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 112;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this.FightBackPower;
			}
			return result;
		}
	}
}
