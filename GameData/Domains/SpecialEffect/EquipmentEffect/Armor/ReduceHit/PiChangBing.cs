using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReduceHit
{
	// Token: 0x020001A3 RID: 419
	public class PiChangBing : ReduceHitBase
	{
		// Token: 0x06002BFA RID: 11258 RVA: 0x002066C7 File Offset: 0x002048C7
		public PiChangBing()
		{
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x002066D1 File Offset: 0x002048D1
		public PiChangBing(int charId, ItemKey itemKey) : base(charId, itemKey, 30104)
		{
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x002066E4 File Offset: 0x002048E4
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType == 10;
		}
	}
}
