using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.DefenseAndAssist
{
	// Token: 0x020001D4 RID: 468
	public class PiJiaZhenGangShiSiJue : AssistSkillBase
	{
		// Token: 0x06002D49 RID: 11593 RVA: 0x0020B15A File Offset: 0x0020935A
		public PiJiaZhenGangShiSiJue()
		{
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x0020B164 File Offset: 0x00209364
		public PiJiaZhenGangShiSiJue(CombatSkillKey skillKey) : base(skillKey, 9702)
		{
			this.SetConstAffectingOnCombatBegin = true;
		}

		// Token: 0x06002D4B RID: 11595 RVA: 0x0020B17C File Offset: 0x0020937C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 141 : 143, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 142 : 144, -1, -1, -1, -1), EDataModifyType.TotalPercent);
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x0020B1FD File Offset: 0x002093FD
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			base.SetConstAffecting(context, base.CanAffect);
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x0020B210 File Offset: 0x00209410
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = 50;
			}
			return result;
		}

		// Token: 0x04000D9B RID: 3483
		private const sbyte AddEquipValue = 50;
	}
}
