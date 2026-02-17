using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReduceHit
{
	// Token: 0x020001AB RID: 427
	public abstract class ReduceHitBase : EquipmentEffectBase
	{
		// Token: 0x06002C12 RID: 11282
		protected abstract bool IsRequireWeaponSubType(short weaponSubType);

		// Token: 0x06002C13 RID: 11283 RVA: 0x0020689E File Offset: 0x00204A9E
		protected ReduceHitBase()
		{
		}

		// Token: 0x06002C14 RID: 11284 RVA: 0x002068A8 File Offset: 0x00204AA8
		protected ReduceHitBase(int charId, ItemKey itemKey, int type) : base(charId, itemKey, type)
		{
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x002068B8 File Offset: 0x00204AB8
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			for (sbyte type = 0; type < 4; type += 1)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, (ushort)(90 + type), -1, -1, -1, -1), EDataModifyType.AddPercent);
			}
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x00206904 File Offset: 0x00204B04
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.IsCurrArmor((sbyte)dataKey.CustomParam2);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ItemKey enemyWeaponKey = DomainManager.Combat.GetUsingWeaponKey(base.CurrEnemyChar);
				short enemyWeaponSubType = DomainManager.Item.GetElement_Weapons(enemyWeaponKey.Id).GetItemSubType();
				bool flag2 = !this.IsRequireWeaponSubType(enemyWeaponSubType);
				if (flag2)
				{
					result = 0;
				}
				else
				{
					result = -20;
				}
			}
			return result;
		}

		// Token: 0x04000D56 RID: 3414
		private const sbyte ReducePercent = -20;
	}
}
