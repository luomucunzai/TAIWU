using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Posing
{
	// Token: 0x0200013B RID: 315
	public class YunYong : CombatSkillEffectBase
	{
		// Token: 0x06002A80 RID: 10880 RVA: 0x00202B3F File Offset: 0x00200D3F
		public YunYong()
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x00202B50 File Offset: 0x00200D50
		public YunYong(CombatSkillKey skillKey) : base(skillKey, 40103, -1)
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002A82 RID: 10882 RVA: 0x00202B68 File Offset: 0x00200D68
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(175, EDataModifyType.TotalPercent, -1);
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x00202B7C File Offset: 0x00200D7C
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
				bool flag2 = dataKey.FieldId == 175;
				if (flag2)
				{
					result = -25;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000CF9 RID: 3321
		private const sbyte ChangeMoveCost = -25;
	}
}
