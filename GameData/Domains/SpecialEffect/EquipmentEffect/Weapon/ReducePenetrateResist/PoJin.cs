using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReducePenetrateResist
{
	// Token: 0x02000182 RID: 386
	public class PoJin : ReducePenetrateResistBase
	{
		// Token: 0x06002B82 RID: 11138 RVA: 0x0020578C File Offset: 0x0020398C
		public PoJin()
		{
		}

		// Token: 0x06002B83 RID: 11139 RVA: 0x00205796 File Offset: 0x00203996
		public PoJin(int charId, ItemKey itemKey) : base(charId, itemKey, 30009)
		{
			this.RequireArmorResourceType = 2;
		}
	}
}
