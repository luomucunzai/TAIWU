using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.DefenseAndAssist
{
	// Token: 0x020003A7 RID: 935
	public class MiChongXiang : AssistSkillBase
	{
		// Token: 0x060036AC RID: 13996 RVA: 0x00231898 File Offset: 0x0022FA98
		public MiChongXiang()
		{
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x002318A2 File Offset: 0x0022FAA2
		public MiChongXiang(CombatSkillKey skillKey) : base(skillKey, 12800)
		{
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x002318B2 File Offset: 0x0022FAB2
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x002318E4 File Offset: 0x0022FAE4
		public override void OnDataAdded(DataContext context)
		{
			base.OnDataAdded(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.AppendAffectedAllEnemyData(context, 259, EDataModifyType.AddPercent, -1);
			}
			else
			{
				base.AppendAffectedData(context, base.CharacterId, 259, EDataModifyType.AddPercent, -1);
			}
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x0023192A File Offset: 0x0022FB2A
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x0023195C File Offset: 0x0022FB5C
		private void OnCombatBegin(DataContext context)
		{
			CValuePercent attainmentReducePercent = (int)this.CharObj.GetLifeSkillAttainment(9) * MiChongXiang.AttainmentReducePercent;
			this._periodicFrames = 30 + Math.Max(0, 90 - 90 * attainmentReducePercent);
			this._frameCounter = 0;
		}

		// Token: 0x060036B2 RID: 14002 RVA: 0x002319A8 File Offset: 0x0022FBA8
		private void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = combatChar != base.CombatChar || DomainManager.Combat.Pause;
			if (!flag)
			{
				this._frameCounter++;
				bool flag2 = this._frameCounter < this._periodicFrames;
				if (!flag2)
				{
					this._frameCounter = 0;
					bool flag3 = !base.CombatChar.ChangeWugCount(context, 1);
					if (!flag3)
					{
						base.ShowSpecialEffectTips(0);
						base.ShowEffectTips(context);
					}
				}
			}
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x00231A24 File Offset: 0x0022FC24
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId != 259;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = base.IsDirect && !base.IsCurrent;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					short unit = base.CombatChar.GetWugCount();
					bool flag3 = unit <= 0;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						base.ShowSpecialEffectTips(1);
						result = (int)(base.IsDirect ? (unit * 2) : (-(int)unit));
					}
				}
			}
			return result;
		}

		// Token: 0x04000FEE RID: 4078
		private const int StaticPeriodicFrames = 30;

		// Token: 0x04000FEF RID: 4079
		private const int DynamicPeriodicFrames = 90;

		// Token: 0x04000FF0 RID: 4080
		private static readonly CValuePercent AttainmentReducePercent = 20;

		// Token: 0x04000FF1 RID: 4081
		private int _periodicFrames;

		// Token: 0x04000FF2 RID: 4082
		private int _frameCounter;
	}
}
