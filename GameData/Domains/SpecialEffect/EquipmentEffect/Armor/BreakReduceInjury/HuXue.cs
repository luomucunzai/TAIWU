using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.BreakReduceInjury
{
	// Token: 0x020001AD RID: 429
	public class HuXue : EquipmentEffectBase
	{
		// Token: 0x06002C1B RID: 11291 RVA: 0x00206A29 File Offset: 0x00204C29
		public HuXue()
		{
		}

		// Token: 0x06002C1C RID: 11292 RVA: 0x00206A33 File Offset: 0x00204C33
		public HuXue(int charId, ItemKey itemKey) : base(charId, itemKey, 30115)
		{
		}

		// Token: 0x06002C1D RID: 11293 RVA: 0x00206A44 File Offset: 0x00204C44
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x00206A70 File Offset: 0x00204C70
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CombatChar.NeedReduceArmorDurability || dataKey.CustomParam0 != 0 || !base.IsCurrArmor((sbyte)dataKey.CustomParam1);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 102;
				if (flag2)
				{
					result = -30;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000D58 RID: 3416
		private const sbyte ReduceDamagePercent = -30;
	}
}
