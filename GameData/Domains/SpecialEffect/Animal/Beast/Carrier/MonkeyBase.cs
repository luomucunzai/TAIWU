using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x02000635 RID: 1589
	public abstract class MonkeyBase : CarrierEffectBase
	{
		// Token: 0x17000310 RID: 784
		// (get) Token: 0x0600462D RID: 17965
		protected abstract int PowerAddOrReduceRatio { get; }

		// Token: 0x0600462E RID: 17966 RVA: 0x0027416D File Offset: 0x0027236D
		protected MonkeyBase()
		{
		}

		// Token: 0x0600462F RID: 17967 RVA: 0x00274177 File Offset: 0x00272377
		protected MonkeyBase(int charId) : base(charId)
		{
		}

		// Token: 0x06004630 RID: 17968 RVA: 0x00274184 File Offset: 0x00272384
		protected override void OnEnableSubClass(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 257, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 258, -1, -1, -1, -1), EDataModifyType.TotalPercent);
		}

		// Token: 0x06004631 RID: 17969 RVA: 0x002741E0 File Offset: 0x002723E0
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
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				int num;
				if (fieldId != 257)
				{
					if (fieldId != 258)
					{
						num = 0;
					}
					else
					{
						num = -this.PowerAddOrReduceRatio;
					}
				}
				else
				{
					num = this.PowerAddOrReduceRatio;
				}
				if (!true)
				{
				}
				result = num;
			}
			return result;
		}
	}
}
