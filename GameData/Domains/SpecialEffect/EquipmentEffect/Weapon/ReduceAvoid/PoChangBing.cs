using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReduceAvoid
{
	// Token: 0x0200018A RID: 394
	public class PoChangBing : ReduceAvoidBase
	{
		// Token: 0x06002B99 RID: 11161 RVA: 0x00205A97 File Offset: 0x00203C97
		public PoChangBing()
		{
		}

		// Token: 0x06002B9A RID: 11162 RVA: 0x00205AA1 File Offset: 0x00203CA1
		public PoChangBing(int charId, ItemKey itemKey) : base(charId, itemKey, 30004)
		{
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x00205AB4 File Offset: 0x00203CB4
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType == 10;
		}
	}
}
