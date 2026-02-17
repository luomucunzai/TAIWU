using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Blade
{
	// Token: 0x02000404 RID: 1028
	public class BaWangDao : CombatSkillEffectBase
	{
		// Token: 0x060038CA RID: 14538 RVA: 0x0023BE98 File Offset: 0x0023A098
		public BaWangDao()
		{
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x0023BEA2 File Offset: 0x0023A0A2
		public BaWangDao(CombatSkillKey skillKey) : base(skillKey, 6205, -1)
		{
		}

		// Token: 0x060038CC RID: 14540 RVA: 0x0023BEB3 File Offset: 0x0023A0B3
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x0023BEEC File Offset: 0x0023A0EC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x0023BF28 File Offset: 0x0023A128
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed && base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					this.IsSrcSkillPerformed = true;
					base.AddMaxEffectCount(true);
					base.ShowSpecialEffectTips(0);
					base.AppendAffectedAllEnemyData(context, 151, EDataModifyType.Custom, -1);
				}
				else
				{
					base.RemoveSelf(context);
				}
			}
		}

		// Token: 0x060038CF RID: 14543 RVA: 0x0023BFA4 File Offset: 0x0023A1A4
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = this.IsSrcSkillPerformed && (outerMarkCount > 0 || innerMarkCount > 0);
			if (flag)
			{
				base.ReduceEffectCount((int)(outerMarkCount + innerMarkCount));
			}
		}

		// Token: 0x060038D0 RID: 14544 RVA: 0x0023BFDC File Offset: 0x0023A1DC
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060038D1 RID: 14545 RVA: 0x0023C02C File Offset: 0x0023A22C
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = !this.IsSrcSkillPerformed || !DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar);
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 151 && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1) && DomainManager.Combat.InAttackRange(DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false));
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
