using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.FistAndPalm
{
	// Token: 0x02000428 RID: 1064
	public class ShaoLinChangQuan : CombatSkillEffectBase
	{
		// Token: 0x0600397D RID: 14717 RVA: 0x0023EBF6 File Offset: 0x0023CDF6
		public ShaoLinChangQuan()
		{
		}

		// Token: 0x0600397E RID: 14718 RVA: 0x0023EC00 File Offset: 0x0023CE00
		public ShaoLinChangQuan(CombatSkillKey skillKey) : base(skillKey, 1100, -1)
		{
		}

		// Token: 0x0600397F RID: 14719 RVA: 0x0023EC14 File Offset: 0x0023CE14
		public override void OnEnable(DataContext context)
		{
			this._affected = false;
			DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 0, base.MaxEffectCount, false);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.RegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003980 RID: 14720 RVA: 0x0023ECBB File Offset: 0x0023CEBB
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.UnRegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003981 RID: 14721 RVA: 0x0023ECF4 File Offset: 0x0023CEF4
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = defenderId == base.CharacterId && (base.IsDirect ? outerMarkCount : innerMarkCount) > 0 && base.EffectCount < (int)base.MaxEffectCount;
			if (flag)
			{
				this.AddPower(context);
			}
		}

		// Token: 0x06003982 RID: 14722 RVA: 0x0023ED3C File Offset: 0x0023CF3C
		private void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
		{
			bool flag = defenderId == base.CharacterId && (base.IsDirect ? outerMarkCount : innerMarkCount) > 0 && base.EffectCount < (int)base.MaxEffectCount;
			if (flag)
			{
				this.AddPower(context);
			}
		}

		// Token: 0x06003983 RID: 14723 RVA: 0x0023ED84 File Offset: 0x0023CF84
		private void AddPower(DataContext context)
		{
			bool affected = this._affected;
			if (!affected)
			{
				DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 1, true, false);
				base.ShowSpecialEffectTips(0);
				this._affected = true;
				Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			}
		}

		// Token: 0x06003984 RID: 14724 RVA: 0x0023EDE5 File Offset: 0x0023CFE5
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			this._affected = false;
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x06003985 RID: 14725 RVA: 0x0023EE04 File Offset: 0x0023D004
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06003986 RID: 14726 RVA: 0x0023EE58 File Offset: 0x0023D058
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = base.EffectCount * 3;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040010CB RID: 4299
		private const sbyte AddPowerUnit = 3;

		// Token: 0x040010CC RID: 4300
		private bool _affected;
	}
}
