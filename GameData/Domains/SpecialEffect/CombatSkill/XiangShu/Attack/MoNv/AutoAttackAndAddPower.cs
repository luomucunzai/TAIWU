using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.MoNv
{
	// Token: 0x020002F1 RID: 753
	public class AutoAttackAndAddPower : CombatSkillEffectBase
	{
		// Token: 0x06003371 RID: 13169 RVA: 0x00225091 File Offset: 0x00223291
		protected AutoAttackAndAddPower()
		{
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x0022509B File Offset: 0x0022329B
		protected AutoAttackAndAddPower(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x002250A8 File Offset: 0x002232A8
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x00225114 File Offset: 0x00223314
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x0022516C File Offset: 0x0022336C
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = this.SkillKey.IsMatch(charId, skillId);
			if (flag)
			{
				base.AddMaxEffectCount(true);
			}
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x00225194 File Offset: 0x00223394
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = defender == base.CombatChar && DomainManager.Combat.InAttackRange(attacker) && DomainManager.Combat.InAttackRange(base.CombatChar) && base.EffectCount > 0;
			if (flag)
			{
				base.CombatChar.CanNormalAttackInPrepareSkill = true;
				base.CombatChar.NormalAttackFree();
				base.CombatChar.NormalAttackRepeatIsFightBack = true;
				base.CombatChar.NormalAttackLeftRepeatTimes = this.AttackRepeatTimes;
				base.CombatChar.SetIsFightBack(true, context);
				base.ReduceEffectCount(1);
				base.ShowSpecialEffectTips(0);
			}
			else
			{
				bool flag2 = attacker == base.CombatChar;
				if (flag2)
				{
					base.CombatChar.CanNormalAttackInPrepareSkill = false;
				}
			}
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x0022524C File Offset: 0x0022344C
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId) || base.EffectCount <= 0;
			if (!flag)
			{
				this._addPower = (int)this.AddPowerUnit * base.EffectCount;
				base.InvalidateCache(context, 199);
				base.ReduceEffectCount(base.EffectCount);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x002252B4 File Offset: 0x002234B4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId);
			if (!flag)
			{
				this._addPower = 0;
				base.InvalidateCache(context, 199);
			}
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x002252F0 File Offset: 0x002234F0
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
					result = this._addPower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000F38 RID: 3896
		protected byte AttackRepeatTimes;

		// Token: 0x04000F39 RID: 3897
		protected sbyte AddPowerUnit;

		// Token: 0x04000F3A RID: 3898
		private int _addPower;
	}
}
