using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReduceAvoid
{
	// Token: 0x02000191 RID: 401
	public class PoZhang : ReduceAvoidBase
	{
		// Token: 0x06002BAE RID: 11182 RVA: 0x00205C3D File Offset: 0x00203E3D
		public PoZhang()
		{
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x00205C47 File Offset: 0x00203E47
		public PoZhang(int charId, ItemKey itemKey) : base(charId, itemKey, 30000)
		{
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x00205C58 File Offset: 0x00203E58
		protected override bool IsRequireWeaponSubType(short weaponSubType)
		{
			return weaponSubType == 4;
		}
	}
}
