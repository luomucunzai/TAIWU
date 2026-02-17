using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReduceAvoid
{
	// Token: 0x0200018D RID: 397
	public class PoJian : ReduceAvoidBase
	{
		// Token: 0x06002BA2 RID: 11170 RVA: 0x00205B3E File Offset: 0x00203D3E
		public PoJian()
		{
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x00205B48 File Offset: 0x00203D48
		public PoJian(int charId, ItemKey itemKey) : base(charId, itemKey, 30001)
		{
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x00205B5C File Offset: 0x00203D5C
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType == 8;
		}
	}
}
