using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Neigong
{
	// Token: 0x020001D1 RID: 465
	public class ZhangShiNaiFuPian : CombatSkillEffectBase
	{
		// Token: 0x06002D2F RID: 11567 RVA: 0x0020A846 File Offset: 0x00208A46
		public ZhangShiNaiFuPian()
		{
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x0020A850 File Offset: 0x00208A50
		public ZhangShiNaiFuPian(CombatSkillKey skillKey) : base(skillKey, 9001, -1)
		{
		}

		// Token: 0x06002D31 RID: 11569 RVA: 0x0020A864 File Offset: 0x00208A64
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 30, -1, -1, -1, -1), EDataModifyType.Add);
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 31, -1, -1, -1, -1), EDataModifyType.Add);
			}
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x0020A8C8 File Offset: 0x00208AC8
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
				bool flag2 = dataKey.FieldId == 30;
				if (flag2)
				{
					result = 30;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 31;
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

		// Token: 0x04000D92 RID: 3474
		private const sbyte DirectAddMaxPower = 30;

		// Token: 0x04000D93 RID: 3475
		private const sbyte ReverseReduceRequirementPercent = -20;
	}
}
