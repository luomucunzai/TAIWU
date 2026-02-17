using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReducePenetrate
{
	// Token: 0x0200019D RID: 413
	public class PiJin : ReducePenetrateBase
	{
		// Token: 0x06002BEB RID: 11243 RVA: 0x00206518 File Offset: 0x00204718
		public PiJin()
		{
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x00206522 File Offset: 0x00204722
		public PiJin(int charId, ItemKey itemKey) : base(charId, itemKey, 30109)
		{
			this.RequireWeaponResourceType = 2;
		}
	}
}
