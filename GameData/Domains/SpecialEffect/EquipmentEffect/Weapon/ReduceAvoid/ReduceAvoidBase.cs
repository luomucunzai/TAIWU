using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReduceAvoid
{
	// Token: 0x02000192 RID: 402
	public abstract class ReduceAvoidBase : EquipmentEffectBase
	{
		// Token: 0x06002BB1 RID: 11185
		protected abstract bool IsRequireWeaponSubType(short weaponSubType);

		// Token: 0x06002BB2 RID: 11186 RVA: 0x00205C6E File Offset: 0x00203E6E
		protected ReduceAvoidBase()
		{
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x00205C78 File Offset: 0x00203E78
		protected ReduceAvoidBase(int charId, ItemKey itemKey, int type) : base(charId, itemKey, type)
		{
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x00205C88 File Offset: 0x00203E88
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			for (sbyte type = 0; type < 4; type += 1)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, (ushort)(60 + type), -1, -1, -1, -1), EDataModifyType.AddPercent);
			}
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x00205CD4 File Offset: 0x00203ED4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.IsCurrWeapon();
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

		// Token: 0x04000D49 RID: 3401
		private const sbyte ReducePercent = -20;
	}
}
