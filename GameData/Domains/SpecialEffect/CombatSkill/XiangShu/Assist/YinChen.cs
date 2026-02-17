using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x02000333 RID: 819
	public class YinChen : RanChenZiAssistSkillBase
	{
		// Token: 0x06003484 RID: 13444 RVA: 0x00228E3B File Offset: 0x0022703B
		public YinChen()
		{
		}

		// Token: 0x06003485 RID: 13445 RVA: 0x00228E45 File Offset: 0x00227045
		public YinChen(CombatSkillKey skillKey) : base(skillKey, 16410)
		{
			this.RequireBossPhase = 0;
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x00228E5C File Offset: 0x0022705C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 157, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 175, -1, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06003487 RID: 13447 RVA: 0x00228EC6 File Offset: 0x002270C6
		protected override void DeactivateEffect(DataContext context)
		{
			base.ClearAffectedData(context);
		}

		// Token: 0x06003488 RID: 13448 RVA: 0x00228ED1 File Offset: 0x002270D1
		private void OnCombatBegin(DataContext context)
		{
			base.ShowSpecialEffectTips(0);
			base.CombatChar.SetXiangshuEffectId((short)base.EffectId, context);
			base.SetConstAffecting(context, true);
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x00228F0C File Offset: 0x0022710C
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
				bool flag2 = dataKey.FieldId == 157;
				result = (!flag2 && base.GetModifiedValue(dataKey, dataValue));
			}
			return result;
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x00228F5C File Offset: 0x0022715C
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 175;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}
	}
}
