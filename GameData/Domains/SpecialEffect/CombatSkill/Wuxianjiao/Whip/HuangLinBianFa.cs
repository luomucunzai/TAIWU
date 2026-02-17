using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Whip
{
	// Token: 0x0200037F RID: 895
	public class HuangLinBianFa : CombatSkillEffectBase
	{
		// Token: 0x060035E2 RID: 13794 RVA: 0x0022E422 File Offset: 0x0022C622
		public HuangLinBianFa()
		{
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x0022E42C File Offset: 0x0022C62C
		public HuangLinBianFa(CombatSkillKey skillKey) : base(skillKey, 12400, -1)
		{
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x0022E43D File Offset: 0x0022C63D
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x0022E476 File Offset: 0x0022C676
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x0022E4B0 File Offset: 0x0022C6B0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						this._affected = false;
						base.AddMaxEffectCount(true);
						base.AppendAffectedAllEnemyData(context, 165, EDataModifyType.TotalPercent, -1);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag4 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag4)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x0022E548 File Offset: 0x0022C748
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = !this._affected || mover.IsAlly == base.CombatChar.IsAlly || !isMove;
			if (!flag)
			{
				this._affected = false;
				base.ReduceEffectCount(1);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x0022E598 File Offset: 0x0022C798
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x0022E5E8 File Offset: 0x0022C7E8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 165;
			int result;
			if (flag)
			{
				this._affected = true;
				result = -50;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x04000FB5 RID: 4021
		private const sbyte ReduceDistancePercent = -50;

		// Token: 0x04000FB6 RID: 4022
		private bool _affected;
	}
}
