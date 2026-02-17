using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReducePenetrate
{
	// Token: 0x020001A0 RID: 416
	public class PiYu : ReducePenetrateBase
	{
		// Token: 0x06002BF1 RID: 11249 RVA: 0x0020657E File Offset: 0x0020477E
		public PiYu()
		{
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x00206588 File Offset: 0x00204788
		public PiYu(int charId, ItemKey itemKey) : base(charId, itemKey, 30111)
		{
			this.RequireWeaponResourceType = 3;
		}
	}
}
