using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReducePenetrate
{
	// Token: 0x0200019F RID: 415
	public class PiMu : ReducePenetrateBase
	{
		// Token: 0x06002BEF RID: 11247 RVA: 0x0020655C File Offset: 0x0020475C
		public PiMu()
		{
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x00206566 File Offset: 0x00204766
		public PiMu(int charId, ItemKey itemKey) : base(charId, itemKey, 30110)
		{
			this.RequireWeaponResourceType = 1;
		}
	}
}
