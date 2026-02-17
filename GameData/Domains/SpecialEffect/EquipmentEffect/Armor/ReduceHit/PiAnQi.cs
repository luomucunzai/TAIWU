using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReduceHit
{
	// Token: 0x020001A2 RID: 418
	public class PiAnQi : ReduceHitBase
	{
		// Token: 0x06002BF7 RID: 11255 RVA: 0x00206681 File Offset: 0x00204881
		public PiAnQi()
		{
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x0020668B File Offset: 0x0020488B
		public PiAnQi(int charId, ItemKey itemKey) : base(charId, itemKey, 30106)
		{
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x0020669C File Offset: 0x0020489C
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType == 0 || weaponSubType == 2 || weaponSubType == 12;
		}
	}
}
