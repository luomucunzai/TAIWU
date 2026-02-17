using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReduceAvoid
{
	// Token: 0x02000190 RID: 400
	public class PoRuanBing : ReduceAvoidBase
	{
		// Token: 0x06002BAB RID: 11179 RVA: 0x00205C00 File Offset: 0x00203E00
		public PoRuanBing()
		{
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x00205C0A File Offset: 0x00203E0A
		public PoRuanBing(int charId, ItemKey itemKey) : base(charId, itemKey, 30005)
		{
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x00205C1C File Offset: 0x00203E1C
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType - 6 <= 1;
		}
	}
}
