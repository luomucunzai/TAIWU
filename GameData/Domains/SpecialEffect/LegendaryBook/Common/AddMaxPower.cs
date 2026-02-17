using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Common
{
	// Token: 0x02000170 RID: 368
	public class AddMaxPower : CombatSkillEffectBase
	{
		// Token: 0x06002B3E RID: 11070 RVA: 0x00204D61 File Offset: 0x00202F61
		protected AddMaxPower()
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x00204D72 File Offset: 0x00202F72
		protected AddMaxPower(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x00204D88 File Offset: 0x00202F88
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 200, -1, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 202, -1, -1, -1, -1), EDataModifyType.Add);
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x00204DE4 File Offset: 0x00202FE4
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
				bool flag2 = dataKey.FieldId == 200;
				if (flag2)
				{
					result = 50;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 202;
					if (flag3)
					{
						result = 25;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000D39 RID: 3385
		private const sbyte AddMaxPowerValue = 50;

		// Token: 0x04000D3A RID: 3386
		private const sbyte AddRequirementPercent = 25;
	}
}
