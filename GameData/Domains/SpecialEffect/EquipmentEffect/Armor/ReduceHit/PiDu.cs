using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReduceHit
{
	// Token: 0x020001A5 RID: 421
	public class PiDu : ReduceHitBase
	{
		// Token: 0x06002C00 RID: 11264 RVA: 0x0020672F File Offset: 0x0020492F
		public PiDu()
		{
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x00206739 File Offset: 0x00204939
		public PiDu(int charId, ItemKey itemKey) : base(charId, itemKey, 30103)
		{
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x0020674C File Offset: 0x0020494C
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType - 14 <= 1;
		}
	}
}
