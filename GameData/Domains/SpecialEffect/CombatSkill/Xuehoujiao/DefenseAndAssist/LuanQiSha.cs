using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.DefenseAndAssist
{
	// Token: 0x02000241 RID: 577
	public class LuanQiSha : AssistSkillBase
	{
		// Token: 0x06002FC1 RID: 12225 RVA: 0x002144D5 File Offset: 0x002126D5
		public LuanQiSha()
		{
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x002144DF File Offset: 0x002126DF
		public LuanQiSha(CombatSkillKey skillKey) : base(skillKey, 15801)
		{
			this.SetConstAffectingOnCombatBegin = true;
		}

		// Token: 0x06002FC3 RID: 12227 RVA: 0x002144F6 File Offset: 0x002126F6
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x0021452A File Offset: 0x0021272A
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			base.SetConstAffecting(context, base.CanAffect);
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x0021453C File Offset: 0x0021273C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CustomParam0 != (base.IsDirect ? 0 : 1) || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69;
				if (flag2)
				{
					result = (int)(this.CharObj.GetDisorderOfQi() / 400);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E26 RID: 3622
		private const short RequireQiDisorderUnit = 400;
	}
}
