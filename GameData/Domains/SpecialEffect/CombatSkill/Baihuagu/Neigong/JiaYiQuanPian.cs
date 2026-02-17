using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Neigong
{
	// Token: 0x020005C0 RID: 1472
	public class JiaYiQuanPian : CombatSkillEffectBase
	{
		// Token: 0x060043A9 RID: 17321 RVA: 0x0026C3AA File Offset: 0x0026A5AA
		public JiaYiQuanPian()
		{
		}

		// Token: 0x060043AA RID: 17322 RVA: 0x0026C3D2 File Offset: 0x0026A5D2
		public JiaYiQuanPian(CombatSkillKey skillKey) : base(skillKey, 3002, -1)
		{
		}

		// Token: 0x060043AB RID: 17323 RVA: 0x0026C404 File Offset: 0x0026A604
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 130, -1, -1, -1, -1), EDataModifyType.Add);
			bool flag = !base.IsDirect;
			if (flag)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			}
		}

		// Token: 0x060043AC RID: 17324 RVA: 0x0026C46C File Offset: 0x0026A66C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 130;
			int result;
			if (flag)
			{
				result = (int)(base.IsDirect ? this.DirectReduceMaxAcupoint : this.ReverseReduceMaxAcupoint);
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69 && dataKey.CustomParam0 == 1 && base.CombatChar.GetDefeatMarkCollection().GetTotalAcupointCount() >= (int)this.ReverseReduceDamageAcupoint;
				if (flag2)
				{
					result = (int)this.ReverseReduceDamagePercent;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04001414 RID: 5140
		private sbyte DirectReduceMaxAcupoint = -1;

		// Token: 0x04001415 RID: 5141
		private sbyte ReverseReduceMaxAcupoint = -2;

		// Token: 0x04001416 RID: 5142
		private sbyte ReverseReduceDamageAcupoint = 3;

		// Token: 0x04001417 RID: 5143
		private sbyte ReverseReduceDamagePercent = -30;
	}
}
