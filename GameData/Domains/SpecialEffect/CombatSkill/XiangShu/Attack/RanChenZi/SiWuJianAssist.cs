using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.RanChenZi
{
	// Token: 0x020002EE RID: 750
	public class SiWuJianAssist : CombatSkillEffectBase
	{
		// Token: 0x0600335D RID: 13149 RVA: 0x00224AE7 File Offset: 0x00222CE7
		public SiWuJianAssist()
		{
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x00224AF1 File Offset: 0x00222CF1
		public SiWuJianAssist(CombatSkillKey skillKey, bool goodEnding) : base(skillKey, 17136, -1)
		{
			this._goodEnding = goodEnding;
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x00224B09 File Offset: 0x00222D09
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(199, EDataModifyType.TotalPercent, base.SkillTemplateId);
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x00224B1F File Offset: 0x00222D1F
		public override void OnDisable(DataContext context)
		{
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x00224B24 File Offset: 0x00222D24
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
					result = (this._goodEnding ? -50 : 50);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000F2F RID: 3887
		private const sbyte ChangePower = 50;

		// Token: 0x04000F30 RID: 3888
		private readonly bool _goodEnding;
	}
}
