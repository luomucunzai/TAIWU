using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Stunt
{
	// Token: 0x0200012E RID: 302
	public class YuanLiu : CombatSkillEffectBase
	{
		// Token: 0x06002A58 RID: 10840 RVA: 0x00202549 File Offset: 0x00200749
		public YuanLiu()
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002A59 RID: 10841 RVA: 0x0020255A File Offset: 0x0020075A
		public YuanLiu(CombatSkillKey skillKey) : base(skillKey, 40202, -1)
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x00202572 File Offset: 0x00200772
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x002025A4 File Offset: 0x002007A4
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
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = 100;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000CF2 RID: 3314
		private const short AddPowerPercent = 100;
	}
}
