using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Stunt
{
	// Token: 0x0200012F RID: 303
	public class ZhouQuan : CombatSkillEffectBase
	{
		// Token: 0x06002A5C RID: 10844 RVA: 0x002025F7 File Offset: 0x002007F7
		public ZhouQuan()
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x00202608 File Offset: 0x00200808
		public ZhouQuan(CombatSkillKey skillKey) : base(skillKey, 40204, -1)
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x00202620 File Offset: 0x00200820
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 176, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x0020267C File Offset: 0x0020087C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 176;
				if (flag2)
				{
					result = 100;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 199;
					if (flag3)
					{
						result = -50;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000CF3 RID: 3315
		private const sbyte ChangeKeepFrames = 100;

		// Token: 0x04000CF4 RID: 3316
		private const sbyte ChangePower = -50;
	}
}
