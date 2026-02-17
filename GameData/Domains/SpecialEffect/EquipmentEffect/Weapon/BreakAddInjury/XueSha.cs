using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.BreakAddInjury
{
	// Token: 0x02000194 RID: 404
	public class XueSha : EquipmentEffectBase
	{
		// Token: 0x06002BBA RID: 11194 RVA: 0x00205DDC File Offset: 0x00203FDC
		public XueSha()
		{
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x00205DE6 File Offset: 0x00203FE6
		public XueSha(int charId, ItemKey itemKey) : base(charId, itemKey, 30015)
		{
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x00205DF7 File Offset: 0x00203FF7
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x00205E24 File Offset: 0x00204024
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !base.CombatChar.NeedReduceWeaponDurability || dataKey.CustomParam0 != 0 || !base.IsCurrWeapon();
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

		// Token: 0x04000D4B RID: 3403
		private const sbyte AddDamagePercent = 30;
	}
}
