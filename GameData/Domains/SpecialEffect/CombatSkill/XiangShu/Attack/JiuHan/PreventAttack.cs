using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.JiuHan
{
	// Token: 0x02000301 RID: 769
	public class PreventAttack : CombatSkillEffectBase
	{
		// Token: 0x060033BC RID: 13244 RVA: 0x00226858 File Offset: 0x00224A58
		protected PreventAttack()
		{
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x00226862 File Offset: 0x00224A62
		protected PreventAttack(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x0022686F File Offset: 0x00224A6F
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(107, EDataModifyType.TotalPercent, -1);
			base.CreateAffectedAllEnemyData(283, EDataModifyType.TotalPercent, -1);
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x002268A5 File Offset: 0x00224AA5
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			base.OnDisable(context);
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x002268C2 File Offset: 0x00224AC2
		public override bool IsOn(int counterType)
		{
			return base.EffectCount > 0;
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x002268CD File Offset: 0x00224ACD
		protected override IEnumerable<int> CalcFrameCounterPeriods()
		{
			yield return 120;
			yield break;
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x002268DD File Offset: 0x00224ADD
		public override void OnProcess(DataContext context, int counterType)
		{
			base.ReduceEffectCount(1);
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x002268E8 File Offset: 0x00224AE8
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.SkillKey != this.SkillKey;
			if (!flag)
			{
				base.AddMaxEffectCount(true);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x00226920 File Offset: 0x00224B20
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !dataKey.IsNormalAttack || base.EffectCount <= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				int num;
				if (fieldId != 107)
				{
					if (fieldId != 283)
					{
						num = 0;
					}
					else
					{
						num = 100;
					}
				}
				else
				{
					num = -50;
				}
				if (!true)
				{
				}
				result = num;
			}
			return result;
		}

		// Token: 0x04000F4F RID: 3919
		private const int EffectFrame = 120;

		// Token: 0x04000F50 RID: 3920
		private const int AttackHitOddsAddPercent = -50;

		// Token: 0x04000F51 RID: 3921
		private const int AttackPrepareFrameAddPercent = 100;
	}
}
