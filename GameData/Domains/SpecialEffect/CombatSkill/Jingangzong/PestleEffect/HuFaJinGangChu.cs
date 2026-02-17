using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.PestleEffect
{
	// Token: 0x020004B0 RID: 1200
	public class HuFaJinGangChu : PestleEffectBase
	{
		// Token: 0x06003CC9 RID: 15561 RVA: 0x0024F0D7 File Offset: 0x0024D2D7
		public HuFaJinGangChu()
		{
		}

		// Token: 0x06003CCA RID: 15562 RVA: 0x0024F0E1 File Offset: 0x0024D2E1
		public HuFaJinGangChu(int charId) : base(charId, 11401)
		{
		}

		// Token: 0x06003CCB RID: 15563 RVA: 0x0024F0F1 File Offset: 0x0024D2F1
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 116, -1, -1, -1, -1), EDataModifyType.Add);
			base.OnEnable(context);
		}

		// Token: 0x06003CCC RID: 15564 RVA: 0x0024F128 File Offset: 0x0024D328
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 116 && dataKey.CustomParam1 == (base.IsDirect ? 0 : 1);
				if (flag2)
				{
					result = ((dataKey.CustomParam2 > 1) ? -1 : 0);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}
	}
}
