using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReducePenetrate
{
	// Token: 0x020001A1 RID: 417
	public class ReducePenetrateBase : EquipmentEffectBase
	{
		// Token: 0x06002BF3 RID: 11251 RVA: 0x002065A0 File Offset: 0x002047A0
		protected ReducePenetrateBase()
		{
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x002065AA File Offset: 0x002047AA
		protected ReducePenetrateBase(int charId, ItemKey itemKey, int type) : base(charId, itemKey, type)
		{
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x002065B8 File Offset: 0x002047B8
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 98, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 99, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x00206610 File Offset: 0x00204810
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.IsCurrArmor((sbyte)dataKey.CustomParam1);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ItemKey enemyWeaponKey = DomainManager.Combat.GetUsingWeaponKey(base.CurrEnemyChar);
				bool flag2 = DomainManager.Item.GetBaseItem(enemyWeaponKey).GetResourceType() != this.RequireWeaponResourceType;
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

		// Token: 0x04000D54 RID: 3412
		private const sbyte ReducePercent = -20;

		// Token: 0x04000D55 RID: 3413
		protected sbyte RequireWeaponResourceType;
	}
}
