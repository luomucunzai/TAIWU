using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x02000632 RID: 1586
	public abstract class LionBase : CarrierEffectBase
	{
		// Token: 0x1700030B RID: 779
		// (get) Token: 0x0600461F RID: 17951
		protected abstract int AddOrReduceCostPercent { get; }

		// Token: 0x06004620 RID: 17952 RVA: 0x00274002 File Offset: 0x00272202
		protected LionBase()
		{
		}

		// Token: 0x06004621 RID: 17953 RVA: 0x0027400C File Offset: 0x0027220C
		protected LionBase(int charId) : base(charId)
		{
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x00274018 File Offset: 0x00272218
		protected override void OnEnableSubClass(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 255, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 256, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 150, -1, -1, -1, -1), EDataModifyType.TotalPercent);
		}

		// Token: 0x06004623 RID: 17955 RVA: 0x00274094 File Offset: 0x00272294
		public override void OnDataAdded(DataContext context)
		{
			base.AppendAffectedAllEnemyData(context, 255, EDataModifyType.TotalPercent, -1);
			base.AppendAffectedAllEnemyData(context, 256, EDataModifyType.TotalPercent, -1);
			base.AppendAffectedAllEnemyData(context, 150, EDataModifyType.TotalPercent, -1);
		}

		// Token: 0x06004624 RID: 17956 RVA: 0x002740C4 File Offset: 0x002722C4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			ushort fieldId = dataKey.FieldId;
			bool flag = fieldId == 150 || fieldId - 255 <= 1;
			bool flag2 = !flag || !base.IsCurrent;
			int result;
			if (flag2)
			{
				result = 0;
			}
			else
			{
				result = ((dataKey.CharId == base.CharacterId) ? (-this.AddOrReduceCostPercent) : this.AddOrReduceCostPercent);
			}
			return result;
		}
	}
}
