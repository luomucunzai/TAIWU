using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor
{
	// Token: 0x0200019C RID: 412
	public class DuCi : EquipmentEffectBase
	{
		// Token: 0x06002BE7 RID: 11239 RVA: 0x00206468 File Offset: 0x00204668
		public DuCi()
		{
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x00206472 File Offset: 0x00204672
		public DuCi(int charId, ItemKey itemKey) : base(charId, itemKey, 30117)
		{
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x00206483 File Offset: 0x00204683
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 73, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x002064B0 File Offset: 0x002046B0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CustomParam1 != (int)this.EquipItemKey.ItemType || dataKey.CustomParam2 != this.EquipItemKey.Id;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 73;
				if (flag2)
				{
					result = 20;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000D53 RID: 3411
		private const sbyte AddPercent = 20;
	}
}
