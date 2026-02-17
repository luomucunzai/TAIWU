using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Neigong
{
	// Token: 0x0200038F RID: 911
	public class HuangXianFa : CombatSkillEffectBase
	{
		// Token: 0x06003644 RID: 13892 RVA: 0x0023039E File Offset: 0x0022E59E
		public HuangXianFa()
		{
		}

		// Token: 0x06003645 RID: 13893 RVA: 0x002303B0 File Offset: 0x0022E5B0
		public HuangXianFa(CombatSkillKey skillKey) : base(skillKey, 12001, -1)
		{
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x002303CC File Offset: 0x0022E5CC
		public override void OnEnable(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedData(121, EDataModifyType.AddPercent, -1);
			}
			else
			{
				base.CreateAffectedAllEnemyData(121, EDataModifyType.AddPercent, -1);
			}
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x002303FC File Offset: 0x0022E5FC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId != 121;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = !base.IsDirect && !base.IsCurrent;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					base.ShowSpecialEffectTips(0);
					result = (int)(base.IsDirect ? this.SpeedChangePercent : (-(int)this.SpeedChangePercent));
				}
			}
			return result;
		}

		// Token: 0x04000FD4 RID: 4052
		private sbyte SpeedChangePercent = 50;
	}
}
