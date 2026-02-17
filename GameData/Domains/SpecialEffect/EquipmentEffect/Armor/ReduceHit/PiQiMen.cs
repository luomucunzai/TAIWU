using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReduceHit
{
	// Token: 0x020001A8 RID: 424
	public class PiQiMen : ReduceHitBase
	{
		// Token: 0x06002C09 RID: 11273 RVA: 0x002067E6 File Offset: 0x002049E6
		public PiQiMen()
		{
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x002067F0 File Offset: 0x002049F0
		public PiQiMen(int charId, ItemKey itemKey) : base(charId, itemKey, 30107)
		{
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x00206804 File Offset: 0x00204A04
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType == 1 || weaponSubType == 5 || weaponSubType == 13;
		}
	}
}
