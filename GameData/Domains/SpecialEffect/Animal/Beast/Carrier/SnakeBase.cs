using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x0200063B RID: 1595
	public abstract class SnakeBase : CarrierEffectBase
	{
		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06004647 RID: 17991
		protected abstract int ChangeHealEffect { get; }

		// Token: 0x06004648 RID: 17992 RVA: 0x0027436F File Offset: 0x0027256F
		protected SnakeBase()
		{
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x00274379 File Offset: 0x00272579
		protected SnakeBase(int charId) : base(charId)
		{
		}

		// Token: 0x0600464A RID: 17994 RVA: 0x00274384 File Offset: 0x00272584
		protected override void OnEnableSubClass(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 119, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 122, -1, -1, -1, -1), EDataModifyType.TotalPercent);
		}

		// Token: 0x0600464B RID: 17995 RVA: 0x002743D9 File Offset: 0x002725D9
		public override void OnDataAdded(DataContext context)
		{
			base.AppendAffectedAllEnemyData(context, 120, EDataModifyType.TotalPercent, -1);
			base.AppendAffectedAllEnemyData(context, 123, EDataModifyType.TotalPercent, -1);
		}

		// Token: 0x0600464C RID: 17996 RVA: 0x002743F4 File Offset: 0x002725F4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId == base.CharacterId;
			bool flag2 = flag;
			if (flag2)
			{
				ushort fieldId = dataKey.FieldId;
				bool flag3 = fieldId == 119 || fieldId == 122;
				flag2 = flag3;
			}
			bool flag4 = flag2;
			int result;
			if (flag4)
			{
				result = this.ChangeHealEffect;
			}
			else
			{
				bool flag5 = dataKey.CharId != base.CharacterId;
				bool flag6 = flag5;
				if (flag6)
				{
					ushort fieldId = dataKey.FieldId;
					bool flag3 = fieldId == 120 || fieldId == 123;
					flag6 = flag3;
				}
				bool flag7 = flag6;
				if (flag7)
				{
					result = -this.ChangeHealEffect;
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
