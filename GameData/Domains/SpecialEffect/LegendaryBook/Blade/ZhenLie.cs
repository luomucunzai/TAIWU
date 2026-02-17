using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.EquipmentEffect;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Blade
{
	// Token: 0x0200017E RID: 382
	public class ZhenLie : EquipmentEffectBase
	{
		// Token: 0x06002B6F RID: 11119 RVA: 0x002054E9 File Offset: 0x002036E9
		public ZhenLie()
		{
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x002054F3 File Offset: 0x002036F3
		public ZhenLie(int charId, ItemKey itemKey) : base(charId, itemKey, 40800, false)
		{
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x00205505 File Offset: 0x00203705
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(69, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x0020551C File Offset: 0x0020371C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 69 || !base.IsCurrWeapon();
			int result;
			if (flag)
			{
				result = base.GetModifyValue(dataKey, currModifyValue);
			}
			else
			{
				result = ((dataKey.CustomParam2 == 1) ? 33 : base.GetModifyValue(dataKey, currModifyValue));
			}
			return result;
		}

		// Token: 0x04000D41 RID: 3393
		private const int DirectDamageAddPercent = 33;
	}
}
