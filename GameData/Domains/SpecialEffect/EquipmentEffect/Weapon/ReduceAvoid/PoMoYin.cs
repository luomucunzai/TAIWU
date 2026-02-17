using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReduceAvoid
{
	// Token: 0x0200018E RID: 398
	public class PoMoYin : ReduceAvoidBase
	{
		// Token: 0x06002BA5 RID: 11173 RVA: 0x00205B72 File Offset: 0x00203D72
		public PoMoYin()
		{
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x00205B7C File Offset: 0x00203D7C
		public PoMoYin(int charId, ItemKey itemKey) : base(charId, itemKey, 30008)
		{
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x00205B90 File Offset: 0x00203D90
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType == 3 || weaponSubType == 11;
		}
	}
}
