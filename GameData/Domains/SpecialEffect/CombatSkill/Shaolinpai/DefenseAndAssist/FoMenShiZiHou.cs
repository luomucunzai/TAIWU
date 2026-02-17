using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.DefenseAndAssist
{
	// Token: 0x02000432 RID: 1074
	public class FoMenShiZiHou : AssistSkillBase
	{
		// Token: 0x060039B3 RID: 14771 RVA: 0x0023FE89 File Offset: 0x0023E089
		public FoMenShiZiHou()
		{
		}

		// Token: 0x060039B4 RID: 14772 RVA: 0x0023FE93 File Offset: 0x0023E093
		public FoMenShiZiHou(CombatSkillKey skillKey) : base(skillKey, 1606)
		{
		}

		// Token: 0x060039B5 RID: 14773 RVA: 0x0023FEA3 File Offset: 0x0023E0A3
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x0023FEDC File Offset: 0x0023E0DC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x0023FF18 File Offset: 0x0023E118
		private void OnCombatBegin(DataContext context)
		{
			this._addingCostCharId = -1;
			this._addingCostSkillId = -1;
			base.AppendAffectedAllEnemyData(context, 207, EDataModifyType.AddPercent, -1);
			base.AppendAffectedAllEnemyData(context, 9, EDataModifyType.Custom, -1);
			base.AppendAffectedAllEnemyData(context, 14, EDataModifyType.Custom, -1);
			base.AppendAffectedAllEnemyData(context, 11, EDataModifyType.Custom, -1);
			base.AppendAffectedAllEnemyData(context, 204, EDataModifyType.AddPercent, -1);
			base.AppendAffectedAllEnemyData(context, 13, EDataModifyType.Custom, -1);
			base.AppendAffectedAllEnemyData(context, 10, EDataModifyType.Custom, -1);
			base.AppendAffectedAllEnemyData(context, 12, EDataModifyType.Custom, -1);
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x0023FF9C File Offset: 0x0023E19C
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || !base.CanAffect || !CombatSkillTemplateHelper.IsAttack(skillId) || !DomainManager.Combat.InAttackRange(base.CombatChar) || base.CombatChar.GetAutoCastingSkill();
			if (!flag)
			{
				this._addingCostCharId = base.EnemyChar.GetId();
				this._addingCostSkillId = skillId;
				this.DoClearSkill(context);
				this.DoMakeAttack();
				this.InvalidAllCache(context);
				base.ShowSpecialEffectTips(1);
				base.ShowSpecialEffectTips(2);
			}
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x0024002C File Offset: 0x0023E22C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || this._addingCostCharId < 0 || skillId != this._addingCostSkillId;
			if (!flag)
			{
				this._addingCostCharId = -1;
				this._addingCostSkillId = -1;
				this.InvalidAllCache(context);
			}
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x00240078 File Offset: 0x0023E278
		private void DoClearSkill(DataContext context)
		{
			bool flag = base.IsDirect ? (base.EnemyChar.GetAffectingMoveSkillId() < 0) : (base.EnemyChar.GetAffectingDefendSkillId() < 0);
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					base.ClearAffectingAgileSkill(context, base.EnemyChar);
				}
				else
				{
					DomainManager.Combat.ClearAffectingDefenseSkill(context, base.EnemyChar);
				}
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060039BB RID: 14779 RVA: 0x002400E8 File Offset: 0x0023E2E8
		private void DoMakeAttack()
		{
			Weapon weapon = DomainManager.Combat.GetUsingWeapon(base.CombatChar);
			bool flag = weapon.GetTemplateId() != 884;
			if (!flag)
			{
				CombatContext context = CombatContext.Create(base.CombatChar, null, -1, -1, -1, null);
				int hitOdds = context.CalcProperty(3).HitOdds;
				int damageValue = CFormula.FormulaCalcDamageValue((long)context.BaseDamage, (long)hitOdds, 100L, (long)context.AttackOdds);
				damageValue *= (int)base.SkillInstance.GetPower();
				DomainManager.Combat.AddMindDamage(context, damageValue);
				base.ShowSpecialEffectTips(3);
			}
		}

		// Token: 0x060039BC RID: 14780 RVA: 0x00240194 File Offset: 0x0023E394
		private void InvalidAllCache(DataContext context)
		{
			base.InvalidateAllEnemyCache(context, base.IsDirect ? 207 : 204);
			base.InvalidateAllEnemyCache(context, base.IsDirect ? 9 : 13);
			base.InvalidateAllEnemyCache(context, base.IsDirect ? 14 : 10);
			base.InvalidateAllEnemyCache(context, base.IsDirect ? 11 : 12);
		}

		// Token: 0x060039BD RID: 14781 RVA: 0x00240200 File Offset: 0x0023E400
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = this._addingCostCharId < 0 || dataKey.CharId != this._addingCostCharId;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool isDirect = base.IsDirect;
				bool flag2 = isDirect;
				if (flag2)
				{
					ushort fieldId = dataKey.FieldId;
					bool flag3 = fieldId == 9 || fieldId == 11 || fieldId == 14;
					flag2 = flag3;
				}
				bool flag4 = flag2;
				if (flag4)
				{
					result = 0;
				}
				else
				{
					bool flag5 = !base.IsDirect;
					bool flag6 = flag5;
					if (flag6)
					{
						ushort fieldId = dataKey.FieldId;
						bool flag3 = fieldId == 10 || fieldId - 12 <= 1;
						flag6 = flag3;
					}
					bool flag7 = flag6;
					if (flag7)
					{
						result = 0;
					}
					else
					{
						result = dataValue;
					}
				}
			}
			return result;
		}

		// Token: 0x060039BE RID: 14782 RVA: 0x002402C4 File Offset: 0x0023E4C4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.IsNormalAttack || dataKey.CharId != this._addingCostCharId || this._addingCostCharId < 0 || !base.CanAffect;
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
				if (fieldId != 204)
				{
					if (fieldId != 207)
					{
						num = 0;
					}
					else
					{
						num = ((base.IsDirect && CombatSkillTemplateHelper.IsAgile(dataKey.CombatSkillId)) ? 100 : 0);
					}
				}
				else
				{
					num = ((!base.IsDirect && CombatSkillTemplateHelper.IsDefense(dataKey.CombatSkillId)) ? 100 : 0);
				}
				if (!true)
				{
				}
				result = num;
			}
			return result;
		}

		// Token: 0x040010DF RID: 4319
		private const int AddCostPercent = 100;

		// Token: 0x040010E0 RID: 4320
		private int _addingCostCharId;

		// Token: 0x040010E1 RID: 4321
		private short _addingCostSkillId;
	}
}
