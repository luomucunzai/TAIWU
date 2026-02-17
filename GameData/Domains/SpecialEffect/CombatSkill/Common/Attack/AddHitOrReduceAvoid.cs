using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x02000586 RID: 1414
	public class AddHitOrReduceAvoid : CombatSkillEffectBase
	{
		// Token: 0x060041DC RID: 16860 RVA: 0x00264657 File Offset: 0x00262857
		protected AddHitOrReduceAvoid()
		{
		}

		// Token: 0x060041DD RID: 16861 RVA: 0x00264661 File Offset: 0x00262861
		protected AddHitOrReduceAvoid(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060041DE RID: 16862 RVA: 0x00264670 File Offset: 0x00262870
		public override void OnEnable(DataContext context)
		{
			this._fieldId = (ushort)((base.IsDirect ? 56 : 90) + this.AffectHitType);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, this._fieldId, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060041DF RID: 16863 RVA: 0x00264724 File Offset: 0x00262924
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060041E0 RID: 16864 RVA: 0x0026478C File Offset: 0x0026298C
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = !this.CharacterIdMatch(attacker.GetId()) || !this.IsSrcSkillPerformed || pursueIndex > 0 || attacker.NormalAttackHitType != this.AffectHitType || !DomainManager.Combat.InAttackRange(base.IsDirect ? base.CombatChar : base.CurrEnemyChar);
			if (!flag)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060041E1 RID: 16865 RVA: 0x002647F8 File Offset: 0x002629F8
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = !this.IsSrcSkillPerformed || !this.CharacterIdMatch(attacker.GetId());
			if (!flag)
			{
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x060041E2 RID: 16866 RVA: 0x00264830 File Offset: 0x00262A30
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = !this.CharacterIdMatch(attacker.GetId()) || !this.IsSrcSkillPerformed || !base.CombatChar.SkillHitType.Exist(this.AffectHitType) || !DomainManager.Combat.InAttackRange(base.IsDirect ? base.CombatChar : base.CurrEnemyChar);
			if (!flag)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060041E3 RID: 16867 RVA: 0x002648A0 File Offset: 0x00262AA0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.IsSrcSkillPerformed;
			if (flag)
			{
				bool flag2 = skillId == base.SkillTemplateId && charId == base.CharacterId;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
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
				bool flag4 = Config.CombatSkill.Instance[skillId].EquipType == 1 && this.CharacterIdMatch(charId);
				if (flag4)
				{
					base.ReduceEffectCount(1);
				}
				bool flag5 = skillId == base.SkillTemplateId && base.PowerMatchAffectRequire((int)power, 0) && charId == base.CharacterId;
				if (flag5)
				{
					base.RemoveSelf(context);
				}
			}
		}

		// Token: 0x060041E4 RID: 16868 RVA: 0x00264960 File Offset: 0x00262B60
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060041E5 RID: 16869 RVA: 0x002649B0 File Offset: 0x00262BB0
		private bool CharacterIdMatch(int charId)
		{
			return charId == (base.IsDirect ? base.CharacterId : base.CurrEnemyChar.GetId());
		}

		// Token: 0x060041E6 RID: 16870 RVA: 0x002649E0 File Offset: 0x00262BE0
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
				bool flag2 = dataKey.FieldId == this._fieldId;
				if (flag2)
				{
					result = 3 * ((int)(base.MaxEffectCount + 1) - base.EffectCount) * (base.IsDirect ? 1 : -1);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0400136E RID: 4974
		private const sbyte UnitValue = 3;

		// Token: 0x0400136F RID: 4975
		protected sbyte AffectHitType;

		// Token: 0x04001370 RID: 4976
		private ushort _fieldId;
	}
}
