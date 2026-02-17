using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Neigong
{
	// Token: 0x020001CE RID: 462
	public class JiHunShengWeiPian : CombatSkillEffectBase
	{
		// Token: 0x06002D21 RID: 11553 RVA: 0x0020A5B0 File Offset: 0x002087B0
		public JiHunShengWeiPian()
		{
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x0020A5BA File Offset: 0x002087BA
		public JiHunShengWeiPian(CombatSkillKey skillKey) : base(skillKey, 9004, -1)
		{
		}

		// Token: 0x06002D23 RID: 11555 RVA: 0x0020A5CC File Offset: 0x002087CC
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 27, -1, -1, -1, -1), EDataModifyType.Add);
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 28, -1, -1, -1, -1), EDataModifyType.Add);
			}
		}

		// Token: 0x06002D24 RID: 11556 RVA: 0x0020A630 File Offset: 0x00208830
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 27;
				if (flag2)
				{
					result = 30;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 28;
					if (flag3)
					{
						result = -20;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000D8E RID: 3470
		private const sbyte DirectAddMaxPower = 30;

		// Token: 0x04000D8F RID: 3471
		private const sbyte ReverseReduceRequirementPercent = -20;
	}
}
