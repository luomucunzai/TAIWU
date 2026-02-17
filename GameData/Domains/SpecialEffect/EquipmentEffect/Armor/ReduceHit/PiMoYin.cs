using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReduceHit
{
	// Token: 0x020001A7 RID: 423
	public class PiMoYin : ReduceHitBase
	{
		// Token: 0x06002C06 RID: 11270 RVA: 0x002067A2 File Offset: 0x002049A2
		public PiMoYin()
		{
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x002067AC File Offset: 0x002049AC
		public PiMoYin(int charId, ItemKey itemKey) : base(charId, itemKey, 30108)
		{
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x002067C0 File Offset: 0x002049C0
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType == 3 || weaponSubType == 11;
		}
	}
}
