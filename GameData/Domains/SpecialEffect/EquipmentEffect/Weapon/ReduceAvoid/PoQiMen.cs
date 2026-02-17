using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReduceAvoid
{
	// Token: 0x0200018F RID: 399
	public class PoQiMen : ReduceAvoidBase
	{
		// Token: 0x06002BA8 RID: 11176 RVA: 0x00205BB6 File Offset: 0x00203DB6
		public PoQiMen()
		{
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x00205BC0 File Offset: 0x00203DC0
		public PoQiMen(int charId, ItemKey itemKey) : base(charId, itemKey, 30007)
		{
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x00205BD4 File Offset: 0x00203DD4
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType == 1 || weaponSubType == 5 || weaponSubType == 13;
		}
	}
}
