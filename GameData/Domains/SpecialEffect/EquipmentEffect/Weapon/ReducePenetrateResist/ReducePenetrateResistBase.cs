using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReducePenetrateResist
{
	// Token: 0x02000186 RID: 390
	public class ReducePenetrateResistBase : EquipmentEffectBase
	{
		// Token: 0x06002B8A RID: 11146 RVA: 0x00205814 File Offset: 0x00203A14
		protected ReducePenetrateResistBase()
		{
		}

		// Token: 0x06002B8B RID: 11147 RVA: 0x0020581E File Offset: 0x00203A1E
		protected ReducePenetrateResistBase(int charId, ItemKey itemKey, int type) : base(charId, itemKey, type)
		{
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x0020582C File Offset: 0x00203A2C
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 66, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 67, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x00205884 File Offset: 0x00203A84
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.IsCurrWeapon() || dataKey.CustomParam1 < 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ItemKey armorKey = base.CurrEnemyChar.Armors[dataKey.CustomParam1];
				bool flag2 = !armorKey.IsValid();
				if (flag2)
				{
					result = 0;
				}
				else
				{
					ItemBase armor = DomainManager.Item.GetBaseItem(armorKey);
					bool flag3 = armor.GetCurrDurability() <= 0 || armor.GetResourceType() != this.RequireArmorResourceType;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						result = -20;
					}
				}
			}
			return result;
		}

		// Token: 0x04000D45 RID: 3397
		private const sbyte ReducePercent = -20;

		// Token: 0x04000D46 RID: 3398
		protected sbyte RequireArmorResourceType;
	}
}
