using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x0200032B RID: 811
	public class RanChenZiAssistSkillBase : AssistSkillBase
	{
		// Token: 0x06003468 RID: 13416 RVA: 0x00228A1B File Offset: 0x00226C1B
		protected RanChenZiAssistSkillBase()
		{
		}

		// Token: 0x06003469 RID: 13417 RVA: 0x00228A25 File Offset: 0x00226C25
		protected RanChenZiAssistSkillBase(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x0600346A RID: 13418 RVA: 0x00228A34 File Offset: 0x00226C34
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 217, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			this._bossPhaseUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 100U);
			GameDataBridge.AddPostDataModificationHandler(this._bossPhaseUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnBossPhaseChanged));
		}

		// Token: 0x0600346B RID: 13419 RVA: 0x00228AA8 File Offset: 0x00226CA8
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._bossPhaseUid, base.DataHandlerKey);
		}

		// Token: 0x0600346C RID: 13420 RVA: 0x00228AC0 File Offset: 0x00226CC0
		private void OnBossPhaseChanged(DataContext context, DataUid dataUid)
		{
			sbyte currPhase = base.CombatChar.GetBossPhase();
			bool flag = currPhase == this.RequireBossPhase;
			if (flag)
			{
				base.ShowSpecialEffectTips(0);
				base.CombatChar.SetXiangshuEffectId((short)base.EffectId, context);
				base.SetConstAffecting(context, true);
				this.ActivateEffect(context);
			}
			else
			{
				bool flag2 = currPhase > this.RequireBossPhase;
				if (flag2)
				{
					this.DeactivateEffect(context);
					base.SetConstAffecting(context, false);
				}
			}
		}

		// Token: 0x0600346D RID: 13421 RVA: 0x00228B38 File Offset: 0x00226D38
		protected virtual void ActivateEffect(DataContext context)
		{
		}

		// Token: 0x0600346E RID: 13422 RVA: 0x00228B3B File Offset: 0x00226D3B
		protected virtual void DeactivateEffect(DataContext context)
		{
		}

		// Token: 0x0600346F RID: 13423 RVA: 0x00228B40 File Offset: 0x00226D40
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 217;
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x04000F77 RID: 3959
		protected sbyte RequireBossPhase;

		// Token: 0x04000F78 RID: 3960
		private DataUid _bossPhaseUid;
	}
}
