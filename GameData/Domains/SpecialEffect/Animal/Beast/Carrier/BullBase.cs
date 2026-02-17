using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x02000629 RID: 1577
	public abstract class BullBase : CarrierEffectBase
	{
		// Token: 0x170002FC RID: 764
		// (get) Token: 0x060045F8 RID: 17912
		protected abstract int BouncePowerAddPercent { get; }

		// Token: 0x060045F9 RID: 17913 RVA: 0x00273D9E File Offset: 0x00271F9E
		protected BullBase()
		{
		}

		// Token: 0x060045FA RID: 17914 RVA: 0x00273DA8 File Offset: 0x00271FA8
		protected BullBase(int charId) : base(charId)
		{
		}

		// Token: 0x060045FB RID: 17915 RVA: 0x00273DB3 File Offset: 0x00271FB3
		protected override void OnEnableSubClass(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 111, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x060045FC RID: 17916 RVA: 0x00273DE0 File Offset: 0x00271FE0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 111;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this.BouncePowerAddPercent;
			}
			return result;
		}
	}
}
