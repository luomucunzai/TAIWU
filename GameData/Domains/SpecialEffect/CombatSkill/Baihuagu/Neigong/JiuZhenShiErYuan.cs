using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Neigong
{
	// Token: 0x020005C2 RID: 1474
	public class JiuZhenShiErYuan : CombatSkillEffectBase
	{
		// Token: 0x060043B8 RID: 17336 RVA: 0x0026C5C0 File Offset: 0x0026A7C0
		public JiuZhenShiErYuan()
		{
		}

		// Token: 0x060043B9 RID: 17337 RVA: 0x0026C5CA File Offset: 0x0026A7CA
		public JiuZhenShiErYuan(CombatSkillKey skillKey) : base(skillKey, 3001, -1)
		{
		}

		// Token: 0x060043BA RID: 17338 RVA: 0x0026C5DC File Offset: 0x0026A7DC
		public override void OnEnable(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedData(118, EDataModifyType.AddPercent, -1);
			}
			else
			{
				base.CreateAffectedAllEnemyData(118, EDataModifyType.AddPercent, -1);
			}
		}

		// Token: 0x060043BB RID: 17339 RVA: 0x0026C60C File Offset: 0x0026A80C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId != 118;
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
					result = ((dataKey.CharId == base.CharacterId) ? 50 : -50);
				}
			}
			return result;
		}

		// Token: 0x0400141B RID: 5147
		private const sbyte SpeedChangePercent = 50;
	}
}
