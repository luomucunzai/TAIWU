using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReduceAvoid
{
	// Token: 0x02000189 RID: 393
	public class PoAnQi : ReduceAvoidBase
	{
		// Token: 0x06002B96 RID: 11158 RVA: 0x00205A50 File Offset: 0x00203C50
		public PoAnQi()
		{
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x00205A5A File Offset: 0x00203C5A
		public PoAnQi(int charId, ItemKey itemKey) : base(charId, itemKey, 30006)
		{
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x00205A6C File Offset: 0x00203C6C
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType == 0 || weaponSubType == 2 || weaponSubType == 12;
		}
	}
}
