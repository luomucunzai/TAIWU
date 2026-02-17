using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon
{
	// Token: 0x02000181 RID: 385
	public class DuCi : EquipmentEffectBase
	{
		// Token: 0x06002B7E RID: 11134 RVA: 0x002056DA File Offset: 0x002038DA
		public DuCi()
		{
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x002056E4 File Offset: 0x002038E4
		public DuCi(int charId, ItemKey itemKey) : base(charId, itemKey, 30017)
		{
		}

		// Token: 0x06002B80 RID: 11136 RVA: 0x002056F5 File Offset: 0x002038F5
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 73, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06002B81 RID: 11137 RVA: 0x00205724 File Offset: 0x00203924
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

		// Token: 0x04000D44 RID: 3396
		private const sbyte AddPercent = 20;
	}
}
