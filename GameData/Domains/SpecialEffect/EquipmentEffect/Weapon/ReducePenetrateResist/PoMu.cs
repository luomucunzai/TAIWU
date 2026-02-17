using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReducePenetrateResist
{
	// Token: 0x02000184 RID: 388
	public class PoMu : ReducePenetrateResistBase
	{
		// Token: 0x06002B86 RID: 11142 RVA: 0x002057D0 File Offset: 0x002039D0
		public PoMu()
		{
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x002057DA File Offset: 0x002039DA
		public PoMu(int charId, ItemKey itemKey) : base(charId, itemKey, 30010)
		{
			this.RequireArmorResourceType = 1;
		}
	}
}
