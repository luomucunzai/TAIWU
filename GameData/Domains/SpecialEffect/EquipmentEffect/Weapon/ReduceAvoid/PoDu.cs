using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReduceAvoid
{
	// Token: 0x0200018C RID: 396
	public class PoDu : ReduceAvoidBase
	{
		// Token: 0x06002B9F RID: 11167 RVA: 0x00205AFF File Offset: 0x00203CFF
		public PoDu()
		{
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x00205B09 File Offset: 0x00203D09
		public PoDu(int charId, ItemKey itemKey) : base(charId, itemKey, 30003)
		{
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x00205B1C File Offset: 0x00203D1C
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType - 14 <= 1;
		}
	}
}
