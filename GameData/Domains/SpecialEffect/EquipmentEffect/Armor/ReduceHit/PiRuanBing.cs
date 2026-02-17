using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReduceHit
{
	// Token: 0x020001A9 RID: 425
	public class PiRuanBing : ReduceHitBase
	{
		// Token: 0x06002C0C RID: 11276 RVA: 0x00206830 File Offset: 0x00204A30
		public PiRuanBing()
		{
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x0020683A File Offset: 0x00204A3A
		public PiRuanBing(int charId, ItemKey itemKey) : base(charId, itemKey, 30105)
		{
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x0020684C File Offset: 0x00204A4C
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType - 6 <= 1;
		}
	}
}
