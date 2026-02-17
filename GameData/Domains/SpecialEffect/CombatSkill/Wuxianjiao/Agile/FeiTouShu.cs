using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Agile
{
	// Token: 0x020003AF RID: 943
	public class FeiTouShu : AgileSkillBase
	{
		// Token: 0x060036E3 RID: 14051 RVA: 0x00232BF9 File Offset: 0x00230DF9
		public FeiTouShu()
		{
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x00232C03 File Offset: 0x00230E03
		public FeiTouShu(CombatSkillKey skillKey) : base(skillKey, 12603)
		{
		}

		// Token: 0x060036E5 RID: 14053 RVA: 0x00232C14 File Offset: 0x00230E14
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 126, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 131, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x00232C74 File Offset: 0x00230E74
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				short distance = DomainManager.Combat.GetCurrentDistance();
				bool flag2 = base.IsDirect ? (distance < 50) : (distance > 70);
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					ushort fieldId = dataKey.FieldId;
					bool flag3 = fieldId == 126 || fieldId == 131;
					bool flag4 = flag3;
					if (flag4)
					{
						base.ShowSpecialEffectTips(0);
						result = false;
					}
					else
					{
						result = dataValue;
					}
				}
			}
			return result;
		}

		// Token: 0x04001001 RID: 4097
		private const sbyte DirectRequireDistance = 50;

		// Token: 0x04001002 RID: 4098
		private const sbyte ReverseRequireDistance = 70;
	}
}
