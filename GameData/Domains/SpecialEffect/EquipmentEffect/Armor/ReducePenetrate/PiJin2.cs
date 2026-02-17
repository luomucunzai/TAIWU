using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReducePenetrate
{
	// Token: 0x0200019E RID: 414
	public class PiJin2 : ReducePenetrateBase
	{
		// Token: 0x06002BED RID: 11245 RVA: 0x0020653A File Offset: 0x0020473A
		public PiJin2()
		{
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x00206544 File Offset: 0x00204744
		public PiJin2(int charId, ItemKey itemKey) : base(charId, itemKey, 30112)
		{
			this.RequireWeaponResourceType = 4;
		}
	}
}
