using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReducePenetrateResist
{
	// Token: 0x02000185 RID: 389
	public class PoYu : ReducePenetrateResistBase
	{
		// Token: 0x06002B88 RID: 11144 RVA: 0x002057F2 File Offset: 0x002039F2
		public PoYu()
		{
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x002057FC File Offset: 0x002039FC
		public PoYu(int charId, ItemKey itemKey) : base(charId, itemKey, 30011)
		{
			this.RequireArmorResourceType = 3;
		}
	}
}
