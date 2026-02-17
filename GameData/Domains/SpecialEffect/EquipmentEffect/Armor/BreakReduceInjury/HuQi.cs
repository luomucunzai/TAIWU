using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.BreakReduceInjury
{
	// Token: 0x020001AC RID: 428
	public class HuQi : EquipmentEffectBase
	{
		// Token: 0x06002C17 RID: 11287 RVA: 0x0020697C File Offset: 0x00204B7C
		public HuQi()
		{
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x00206986 File Offset: 0x00204B86
		public HuQi(int charId, ItemKey itemKey) : base(charId, itemKey, 30116)
		{
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x00206997 File Offset: 0x00204B97
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x002069C4 File Offset: 0x00204BC4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CombatChar.NeedReduceArmorDurability || dataKey.CustomParam0 != 1 || !base.IsCurrArmor((sbyte)dataKey.CustomParam1);
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
					result = 30;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000D57 RID: 3415
		private const sbyte ReduceDamagePercent = -30;
	}
}
