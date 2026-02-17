using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x02000328 RID: 808
	public class LiDuanShenMo : RanChenZiAssistSkillBase
	{
		// Token: 0x06003453 RID: 13395 RVA: 0x002285B1 File Offset: 0x002267B1
		public LiDuanShenMo()
		{
		}

		// Token: 0x06003454 RID: 13396 RVA: 0x002285BB File Offset: 0x002267BB
		public LiDuanShenMo(CombatSkillKey skillKey) : base(skillKey, 16412)
		{
			this.RequireBossPhase = 2;
		}

		// Token: 0x06003455 RID: 13397 RVA: 0x002285D4 File Offset: 0x002267D4
		protected override void ActivateEffect(DataContext context)
		{
			base.AppendAffectedData(context, base.CharacterId, 148, EDataModifyType.Custom, -1);
			base.AppendAffectedData(context, base.CharacterId, 215, EDataModifyType.Custom, -1);
			base.AppendAffectedData(context, base.CharacterId, 126, EDataModifyType.Custom, -1);
			base.AppendAffectedData(context, base.CharacterId, 131, EDataModifyType.Custom, -1);
		}

		// Token: 0x06003456 RID: 13398 RVA: 0x00228633 File Offset: 0x00226833
		protected override void DeactivateEffect(DataContext context)
		{
			base.ClearAffectedData(context);
		}

		// Token: 0x06003457 RID: 13399 RVA: 0x00228640 File Offset: 0x00226840
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			bool result;
			if (flag)
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			else
			{
				bool flag2 = dataKey.CombatSkillId != base.SkillTemplateId && base.CombatChar.GetBossPhase() != this.RequireBossPhase;
				result = (flag2 && base.GetModifiedValue(dataKey, dataValue));
			}
			return result;
		}
	}
}
