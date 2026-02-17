using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReduceHit
{
	// Token: 0x020001AA RID: 426
	public class PiZhang : ReduceHitBase
	{
		// Token: 0x06002C0F RID: 11279 RVA: 0x0020686D File Offset: 0x00204A6D
		public PiZhang()
		{
		}

		// Token: 0x06002C10 RID: 11280 RVA: 0x00206877 File Offset: 0x00204A77
		public PiZhang(int charId, ItemKey itemKey) : base(charId, itemKey, 30100)
		{
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x00206888 File Offset: 0x00204A88
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType == 4;
		}
	}
}
