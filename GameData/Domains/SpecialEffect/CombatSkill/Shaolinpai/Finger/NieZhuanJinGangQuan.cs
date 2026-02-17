using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Finger
{
	// Token: 0x0200042D RID: 1069
	public class NieZhuanJinGangQuan : CombatSkillEffectBase
	{
		// Token: 0x0600399B RID: 14747 RVA: 0x0023F6A0 File Offset: 0x0023D8A0
		public NieZhuanJinGangQuan()
		{
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x0023F6AA File Offset: 0x0023D8AA
		public NieZhuanJinGangQuan(CombatSkillKey skillKey) : base(skillKey, 1204, -1)
		{
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x0023F6BC File Offset: 0x0023D8BC
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x0023F714 File Offset: 0x0023D914
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x0023F76C File Offset: 0x0023D96C
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = defender.GetId() != base.CharacterId || !this._affected;
			if (!flag)
			{
				this._affected = false;
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x0023F7AC File Offset: 0x0023D9AC
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = !this.IsSrcSkillPerformed || charId != base.CharacterId || skillId != base.SkillTemplateId || !base.CombatChar.GetAutoCastingSkill();
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x0023F818 File Offset: 0x0023DA18
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.IsSrcSkillPerformed;
			if (flag)
			{
				bool flag2 = charId != base.CharacterId || skillId != base.SkillTemplateId;
				if (!flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0) && !base.CombatChar.GetAutoCastingSkill();
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						this._affected = false;
						base.AppendAffectedData(context, base.CharacterId, 102, EDataModifyType.AddPercent, -1);
						base.AppendAffectedData(context, base.CharacterId, 111, EDataModifyType.Add, -1);
						base.AppendAffectedData(context, base.CharacterId, 177, EDataModifyType.Custom, -1);
						base.AddMaxEffectCount(true);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
			}
			else
			{
				bool flag4 = charId == base.CharacterId && skillId == base.SkillTemplateId && base.PowerMatchAffectRequire((int)power, 0);
				if (flag4)
				{
					base.RemoveSelf(context);
				}
				else
				{
					bool flag5 = this._affected && isAlly != base.CombatChar.IsAlly && DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar) && Config.CombatSkill.Instance[skillId].EquipType == 1;
					if (flag5)
					{
						this._affected = false;
						base.ReduceEffectCount(1);
					}
				}
			}
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x0023F964 File Offset: 0x0023DB64
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				bool flag2 = DomainManager.Combat.CanCastSkill(base.CombatChar, base.SkillTemplateId, true, false);
				if (flag2)
				{
					DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.Normal);
					base.ShowSpecialEffectTips(1);
				}
				else
				{
					base.RemoveSelf(context);
				}
			}
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x0023F9F8 File Offset: 0x0023DBF8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this.IsSrcSkillPerformed;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 102 && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1);
				if (flag2)
				{
					bool flag3 = !this._affected;
					if (flag3)
					{
						this._affected = true;
						base.ShowSpecialEffectTips(0);
					}
					result = -30;
				}
				else
				{
					bool flag4 = dataKey.FieldId == 111 && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1);
					if (flag4)
					{
						result = 80;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x0023FAA4 File Offset: 0x0023DCA4
		public override OuterAndInnerInts GetModifiedValue(AffectedDataKey dataKey, OuterAndInnerInts dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			OuterAndInnerInts result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 177;
				if (flag2)
				{
					OuterAndInnerInts skillRange = DomainManager.Combat.GetSkillAttackRange(base.CombatChar, base.SkillTemplateId);
					dataValue.Outer = Math.Min(dataValue.Outer, skillRange.Outer);
					dataValue.Inner = Math.Max(dataValue.Inner, skillRange.Inner);
				}
				result = dataValue;
			}
			return result;
		}

		// Token: 0x040010D9 RID: 4313
		private const sbyte ReduceDamagePercent = -30;

		// Token: 0x040010DA RID: 4314
		private const sbyte BouncePower = 80;

		// Token: 0x040010DB RID: 4315
		private const sbyte PrepareProgressPercent = 50;

		// Token: 0x040010DC RID: 4316
		private bool _affected;
	}
}
