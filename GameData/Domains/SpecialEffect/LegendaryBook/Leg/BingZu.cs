using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.EquipmentEffect;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Leg
{
	// Token: 0x02000157 RID: 343
	public class BingZu : EquipmentEffectBase
	{
		// Token: 0x06002AF9 RID: 11001 RVA: 0x002046EE File Offset: 0x002028EE
		public BingZu()
		{
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x002046F8 File Offset: 0x002028F8
		public BingZu(int charId, ItemKey itemKey) : base(charId, itemKey, 40500, false)
		{
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x0020470A File Offset: 0x0020290A
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 88, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x00204738 File Offset: 0x00202938
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
				bool flag2 = dataKey.FieldId == 88;
				result = (!flag2 && dataValue);
			}
			return result;
		}
	}
}
