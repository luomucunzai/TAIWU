using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Agile
{
	// Token: 0x0200033C RID: 828
	public class MoWoZiZai : AgileSkillBase
	{
		// Token: 0x060034B6 RID: 13494 RVA: 0x002298ED File Offset: 0x00227AED
		public MoWoZiZai()
		{
		}

		// Token: 0x060034B7 RID: 13495 RVA: 0x002298F7 File Offset: 0x00227AF7
		public MoWoZiZai(CombatSkillKey skillKey) : base(skillKey, 16209)
		{
		}

		// Token: 0x060034B8 RID: 13496 RVA: 0x00229908 File Offset: 0x00227B08
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 145, -1, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 146, -1, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(-1, 244, -1, -1, -1, -1), EDataModifyType.Custom);
			short aiTargetDist = base.CombatChar.AiController.GetTargetDistance();
			bool flag = aiTargetDist >= 0;
			if (flag)
			{
				DomainManager.Combat.ChangeDistance(context, base.CombatChar, (int)(aiTargetDist - DomainManager.Combat.GetCurrentDistance()), false, false);
			}
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x002299CC File Offset: 0x00227BCC
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = base.CombatChar.GetAffectingMoveSkillId() != base.SkillTemplateId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 244;
				result = (flag2 || dataValue);
			}
			return result;
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x00229A14 File Offset: 0x00227C14
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
				bool flag2 = fieldId - 145 <= 1;
				bool flag3 = flag2;
				if (flag3)
				{
					result = 10000;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}
	}
}
