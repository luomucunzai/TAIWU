using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReduceHit
{
	// Token: 0x020001A4 RID: 420
	public class PiDao : ReduceHitBase
	{
		// Token: 0x06002BFD RID: 11261 RVA: 0x002066FB File Offset: 0x002048FB
		public PiDao()
		{
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x00206705 File Offset: 0x00204905
		public PiDao(int charId, ItemKey itemKey) : base(charId, itemKey, 30102)
		{
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x00206718 File Offset: 0x00204918
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType == 9;
		}
	}
}
