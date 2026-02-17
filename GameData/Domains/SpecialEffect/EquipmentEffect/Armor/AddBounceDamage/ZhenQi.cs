using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.AddBounceDamage
{
	// Token: 0x020001AE RID: 430
	public class ZhenQi : EquipmentEffectBase
	{
		// Token: 0x06002C1F RID: 11295 RVA: 0x00206AD4 File Offset: 0x00204CD4
		public ZhenQi()
		{
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x00206ADE File Offset: 0x00204CDE
		public ZhenQi(int charId, ItemKey itemKey) : base(charId, itemKey, 30114)
		{
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x00206AEF File Offset: 0x00204CEF
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 70, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x00206B1C File Offset: 0x00204D1C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CustomParam0 != 1 || !base.IsCurrArmor((sbyte)dataKey.CustomParam1);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 70;
				if (flag2)
				{
					result = 20;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000D59 RID: 3417
		private const sbyte AddPercent = 20;
	}
}
