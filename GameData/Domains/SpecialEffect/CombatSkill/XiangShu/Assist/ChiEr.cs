using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x02000320 RID: 800
	public class ChiEr : RanChenZiAssistSkillBase
	{
		// Token: 0x06003435 RID: 13365 RVA: 0x00228147 File Offset: 0x00226347
		public ChiEr()
		{
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x00228151 File Offset: 0x00226351
		public ChiEr(CombatSkillKey skillKey) : base(skillKey, 16414)
		{
			this.RequireBossPhase = 4;
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x00228168 File Offset: 0x00226368
		protected override void ActivateEffect(DataContext context)
		{
			base.AppendAffectedData(context, base.CharacterId, 288, EDataModifyType.Custom, -1);
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x00228180 File Offset: 0x00226380
		protected override void DeactivateEffect(DataContext context)
		{
			base.ClearAffectedData(context);
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x0022818C File Offset: 0x0022638C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId == base.CharacterId && dataKey.FieldId == 288;
			return flag || base.GetModifiedValue(dataKey, dataValue);
		}
	}
}
