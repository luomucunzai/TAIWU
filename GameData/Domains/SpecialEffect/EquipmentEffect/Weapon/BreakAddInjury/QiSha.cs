using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.BreakAddInjury
{
	// Token: 0x02000193 RID: 403
	public class QiSha : EquipmentEffectBase
	{
		// Token: 0x06002BB6 RID: 11190 RVA: 0x00205D45 File Offset: 0x00203F45
		public QiSha()
		{
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x00205D4F File Offset: 0x00203F4F
		public QiSha(int charId, ItemKey itemKey) : base(charId, itemKey, 30016)
		{
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x00205D60 File Offset: 0x00203F60
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x00205D8C File Offset: 0x00203F8C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !base.CombatChar.NeedReduceWeaponDurability || dataKey.CustomParam0 != 1 || !base.IsCurrWeapon();
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69;
				if (flag2)
				{
					result = 30;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000D4A RID: 3402
		private const sbyte AddDamagePercent = 30;
	}
}
