using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.EquipmentEffect;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Throw
{
	// Token: 0x0200011F RID: 287
	public class AnSha : EquipmentEffectBase
	{
		// Token: 0x06002A32 RID: 10802 RVA: 0x002021A3 File Offset: 0x002003A3
		public AnSha()
		{
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x002021AD File Offset: 0x002003AD
		public AnSha(int charId, ItemKey itemKey) : base(charId, itemKey, 40600, false)
		{
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x002021BF File Offset: 0x002003BF
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 281, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x002021F0 File Offset: 0x002003F0
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.IsCurrWeapon();
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 281;
				result = (flag2 || dataValue);
			}
			return result;
		}
	}
}
