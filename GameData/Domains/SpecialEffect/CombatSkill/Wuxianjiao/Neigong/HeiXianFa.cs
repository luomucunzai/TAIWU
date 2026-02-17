using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Neigong
{
	// Token: 0x0200038E RID: 910
	public class HeiXianFa : CombatSkillEffectBase
	{
		// Token: 0x06003640 RID: 13888 RVA: 0x002302BE File Offset: 0x0022E4BE
		public HeiXianFa()
		{
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x002302D0 File Offset: 0x0022E4D0
		public HeiXianFa(CombatSkillKey skillKey) : base(skillKey, 12003, -1)
		{
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x002302EC File Offset: 0x0022E4EC
		public override void OnEnable(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedData(122, EDataModifyType.AddPercent, -1);
			}
			else
			{
				base.CreateAffectedAllEnemyData(123, EDataModifyType.AddPercent, -1);
			}
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x0023031C File Offset: 0x0022E51C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			ushort fieldId = dataKey.FieldId;
			bool flag = fieldId - 122 <= 1;
			bool flag2 = !flag;
			int result;
			if (flag2)
			{
				result = 0;
			}
			else
			{
				bool flag3 = !base.IsDirect && !base.IsCurrent;
				if (flag3)
				{
					result = 0;
				}
				else
				{
					bool flag4 = dataKey.CustomParam0 == 0;
					if (flag4)
					{
						base.ShowSpecialEffectTips(0);
					}
					result = (int)((dataKey.FieldId == 122) ? this.HealCountChangePercent : (-(int)this.HealCountChangePercent));
				}
			}
			return result;
		}

		// Token: 0x04000FD3 RID: 4051
		private sbyte HealCountChangePercent = 50;
	}
}
