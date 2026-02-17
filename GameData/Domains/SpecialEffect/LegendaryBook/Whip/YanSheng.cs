using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.EquipmentEffect;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Whip
{
	// Token: 0x0200011D RID: 285
	public class YanSheng : EquipmentEffectBase
	{
		// Token: 0x06002A2C RID: 10796 RVA: 0x002020EB File Offset: 0x002002EB
		public YanSheng()
		{
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x002020F5 File Offset: 0x002002F5
		public YanSheng(int charId, ItemKey itemKey) : base(charId, itemKey, 41100, false)
		{
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x00202107 File Offset: 0x00200307
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 29, -1, -1, -1, -1), EDataModifyType.Add);
		}

		// Token: 0x06002A2F RID: 10799 RVA: 0x00202134 File Offset: 0x00200334
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CustomParam2 != this.EquipItemKey.Id;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 29;
				if (flag2)
				{
					result = 10;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000CEA RID: 3306
		private const sbyte AddAttackRange = 10;
	}
}
