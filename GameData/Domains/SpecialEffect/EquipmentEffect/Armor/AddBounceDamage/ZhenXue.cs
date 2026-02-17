using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.AddBounceDamage
{
	// Token: 0x020001AF RID: 431
	public class ZhenXue : EquipmentEffectBase
	{
		// Token: 0x06002C23 RID: 11299 RVA: 0x00206B74 File Offset: 0x00204D74
		public ZhenXue()
		{
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x00206B7E File Offset: 0x00204D7E
		public ZhenXue(int charId, ItemKey itemKey) : base(charId, itemKey, 30113)
		{
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x00206B8F File Offset: 0x00204D8F
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 70, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x00206BBC File Offset: 0x00204DBC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CustomParam0 != 0 || !base.IsCurrArmor((sbyte)dataKey.CustomParam1);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 70;
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

		// Token: 0x04000D5A RID: 3418
		private const sbyte AddPercent = 20;
	}
}
