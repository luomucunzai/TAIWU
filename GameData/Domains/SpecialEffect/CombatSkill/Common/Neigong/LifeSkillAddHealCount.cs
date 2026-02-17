using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong
{
	// Token: 0x02000577 RID: 1399
	public class LifeSkillAddHealCount : CombatSkillEffectBase
	{
		// Token: 0x0600415C RID: 16732 RVA: 0x00262952 File Offset: 0x00260B52
		protected LifeSkillAddHealCount()
		{
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x0026295C File Offset: 0x00260B5C
		protected LifeSkillAddHealCount(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x0026296C File Offset: 0x00260B6C
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(base.IsDirect ? 119 : 122, EDataModifyType.AddPercent, -1);
			int attainment = (int)this.CharObj.GetLifeSkillAttainment(this.RequireLifeSkillType);
			this._addHealCountPercent = 25 + attainment / 10;
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x002629B0 File Offset: 0x00260BB0
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
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId == 119 || fieldId == 122;
				bool flag3 = flag2;
				if (flag3)
				{
					result = this._addHealCountPercent;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04001343 RID: 4931
		private const sbyte AddHealCountPercentBase = 25;

		// Token: 0x04001344 RID: 4932
		private const sbyte AttainmentAddRatio = 10;

		// Token: 0x04001345 RID: 4933
		protected sbyte RequireLifeSkillType;

		// Token: 0x04001346 RID: 4934
		private int _addHealCountPercent;
	}
}
