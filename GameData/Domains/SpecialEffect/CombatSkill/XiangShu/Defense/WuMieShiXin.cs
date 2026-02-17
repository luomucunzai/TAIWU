using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense
{
	// Token: 0x020002B1 RID: 689
	public class WuMieShiXin : DefenseSkillBase
	{
		// Token: 0x06003204 RID: 12804 RVA: 0x0021D809 File Offset: 0x0021BA09
		public WuMieShiXin()
		{
		}

		// Token: 0x06003205 RID: 12805 RVA: 0x0021D813 File Offset: 0x0021BA13
		public WuMieShiXin(CombatSkillKey skillKey) : base(skillKey, 16310)
		{
		}

		// Token: 0x06003206 RID: 12806 RVA: 0x0021D824 File Offset: 0x0021BA24
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 185, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 186, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 187, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 188, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 191, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 192, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x06003207 RID: 12807 RVA: 0x0021D90C File Offset: 0x0021BB0C
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
				result = 100;
			}
			return result;
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x0021D944 File Offset: 0x0021BB44
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 191 <= 1;
				bool flag3 = flag2;
				if (flag3)
				{
					dataValue = 0;
				}
				result = dataValue;
			}
			return result;
		}
	}
}
