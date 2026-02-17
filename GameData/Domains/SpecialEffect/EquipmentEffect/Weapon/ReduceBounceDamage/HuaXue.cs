using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReduceBounceDamage
{
	// Token: 0x02000188 RID: 392
	public class HuaXue : EquipmentEffectBase
	{
		// Token: 0x06002B92 RID: 11154 RVA: 0x002059B9 File Offset: 0x00203BB9
		public HuaXue()
		{
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x002059C3 File Offset: 0x00203BC3
		public HuaXue(int charId, ItemKey itemKey) : base(charId, itemKey, 30013)
		{
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x002059D4 File Offset: 0x00203BD4
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 103, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x00205A00 File Offset: 0x00203C00
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CustomParam0 != 0 || !base.IsCurrWeapon();
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

		// Token: 0x04000D48 RID: 3400
		private const sbyte ReducePercent = -20;
	}
}
