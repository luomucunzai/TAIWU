using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Whip
{
	// Token: 0x02000381 RID: 897
	public class TianSheSuo : CombatSkillEffectBase
	{
		// Token: 0x060035F7 RID: 13815 RVA: 0x0022E9B6 File Offset: 0x0022CBB6
		public TianSheSuo()
		{
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x0022E9C0 File Offset: 0x0022CBC0
		public TianSheSuo(CombatSkillKey skillKey) : base(skillKey, 12408, -1)
		{
		}

		// Token: 0x060035F9 RID: 13817 RVA: 0x0022E9D4 File Offset: 0x0022CBD4
		public override void OnEnable(DataContext context)
		{
			DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 0, base.MaxEffectCount, false);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x0022EA5F File Offset: 0x0022CC5F
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x0022EA88 File Offset: 0x0022CC88
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || combatSkillId == base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = (base.IsDirect ? outerMarkCount : innerMarkCount) <= 0 || base.EffectCount >= (int)base.MaxEffectCount;
				if (!flag2)
				{
					DomainManager.Combat.ChangeSkillEffectCount(DomainManager.Combat.Context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 1, true, false);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x0022EB14 File Offset: 0x0022CD14
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = index != 3 || base.EffectCount <= 0 || context.SkillKey != this.SkillKey || !base.CombatCharPowerMatchAffectRequire(0);
			if (!flag)
			{
				base.ReduceEffectCount(1);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x0022EB68 File Offset: 0x0022CD68
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
				bool flag2 = dataKey.FieldId == 69 && dataKey.CombatSkillId == base.SkillTemplateId && base.EffectCount > 0 && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1) && base.CombatChar.GetAttackSkillPower() >= 100;
				if (flag2)
				{
					result = 15 * base.EffectCount;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000FBA RID: 4026
		private const sbyte AddDamagePercent = 15;
	}
}
