using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReduceHit
{
	// Token: 0x020001A6 RID: 422
	public class PiJian : ReduceHitBase
	{
		// Token: 0x06002C03 RID: 11267 RVA: 0x0020676E File Offset: 0x0020496E
		public PiJian()
		{
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x00206778 File Offset: 0x00204978
		public PiJian(int charId, ItemKey itemKey) : base(charId, itemKey, 30101)
		{
		}

		// Token: 0x06002C05 RID: 11269 RVA: 0x0020678C File Offset: 0x0020498C
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType == 8;
		}
	}
}
