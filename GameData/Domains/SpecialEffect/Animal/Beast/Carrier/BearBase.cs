using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x02000626 RID: 1574
	public abstract class BearBase : CarrierEffectBase
	{
		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x060045EA RID: 17898
		protected abstract int AddOrReduceDirectFatalDamagePercent { get; }

		// Token: 0x060045EB RID: 17899 RVA: 0x00273CA2 File Offset: 0x00271EA2
		protected BearBase()
		{
		}

		// Token: 0x060045EC RID: 17900 RVA: 0x00273CAC File Offset: 0x00271EAC
		protected BearBase(int charId) : base(charId)
		{
		}

		// Token: 0x060045ED RID: 17901 RVA: 0x00273CB7 File Offset: 0x00271EB7
		protected override void OnEnableSubClass(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 191, -1, -1, -1, -1), EDataModifyType.TotalPercent);
		}

		// Token: 0x060045EE RID: 17902 RVA: 0x00273CE6 File Offset: 0x00271EE6
		public override void OnDataAdded(DataContext context)
		{
			base.AppendAffectedAllEnemyData(context, 191, EDataModifyType.TotalPercent, -1);
		}

		// Token: 0x060045EF RID: 17903 RVA: 0x00273CF8 File Offset: 0x00271EF8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId != 191 || !base.IsCurrent;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				EDamageType damageType = (EDamageType)dataKey.CustomParam1;
				bool flag2 = damageType != EDamageType.Direct;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					result = ((dataKey.CharId == base.CharacterId) ? (-this.AddOrReduceDirectFatalDamagePercent) : this.AddOrReduceDirectFatalDamagePercent);
				}
			}
			return result;
		}
	}
}
