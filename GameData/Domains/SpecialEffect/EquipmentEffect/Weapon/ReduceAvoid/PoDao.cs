using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReduceAvoid
{
	// Token: 0x0200018B RID: 395
	public class PoDao : ReduceAvoidBase
	{
		// Token: 0x06002B9C RID: 11164 RVA: 0x00205ACB File Offset: 0x00203CCB
		public PoDao()
		{
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x00205AD5 File Offset: 0x00203CD5
		public PoDao(int charId, ItemKey itemKey) : base(charId, itemKey, 30002)
		{
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x00205AE8 File Offset: 0x00203CE8
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType == 9;
		}
	}
}
