using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReducePenetrateResist
{
	// Token: 0x02000183 RID: 387
	public class PoJin2 : ReducePenetrateResistBase
	{
		// Token: 0x06002B84 RID: 11140 RVA: 0x002057AE File Offset: 0x002039AE
		public PoJin2()
		{
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x002057B8 File Offset: 0x002039B8
		public PoJin2(int charId, ItemKey itemKey) : base(charId, itemKey, 30012)
		{
			this.RequireArmorResourceType = 4;
		}
	}
}
