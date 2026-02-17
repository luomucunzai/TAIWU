using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.EquipmentEffect;

namespace GameData.Domains.SpecialEffect.LegendaryBook.ControllableShot
{
	// Token: 0x0200016C RID: 364
	public class QiBian : EquipmentEffectBase
	{
		// Token: 0x06002B2F RID: 11055 RVA: 0x00204B5C File Offset: 0x00202D5C
		public QiBian()
		{
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x00204B66 File Offset: 0x00202D66
		public QiBian(int charId, ItemKey itemKey) : base(charId, itemKey, 41200, false)
		{
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x00204B78 File Offset: 0x00202D78
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 76, -1, -1, -1, -1), EDataModifyType.TotalPercent);
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x00204BA4 File Offset: 0x00202DA4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.IsCurrWeapon() || !base.CombatChar.GetChangeTrickAttack();
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 76;
				if (flag2)
				{
					result = 100;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000D37 RID: 3383
		private const short AddPursueOddsPercent = 100;
	}
}
