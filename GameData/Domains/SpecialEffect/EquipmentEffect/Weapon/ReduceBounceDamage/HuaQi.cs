using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReduceBounceDamage
{
	// Token: 0x02000187 RID: 391
	public class HuaQi : EquipmentEffectBase
	{
		// Token: 0x06002B8E RID: 11150 RVA: 0x00205920 File Offset: 0x00203B20
		public HuaQi()
		{
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x0020592A File Offset: 0x00203B2A
		public HuaQi(int charId, ItemKey itemKey) : base(charId, itemKey, 30014)
		{
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x0020593B File Offset: 0x00203B3B
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 103, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x00205968 File Offset: 0x00203B68
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CustomParam0 != 1 || !base.IsCurrWeapon();
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 103;
				if (flag2)
				{
					result = -20;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000D47 RID: 3399
		private const sbyte ReducePercent = -20;
	}
}
