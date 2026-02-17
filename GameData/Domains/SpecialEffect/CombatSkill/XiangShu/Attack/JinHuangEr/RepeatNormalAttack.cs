using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.JinHuangEr
{
	// Token: 0x0200030F RID: 783
	public class RepeatNormalAttack : CombatSkillEffectBase
	{
		// Token: 0x060033ED RID: 13293 RVA: 0x00226FFF File Offset: 0x002251FF
		protected RepeatNormalAttack()
		{
		}

		// Token: 0x060033EE RID: 13294 RVA: 0x00227009 File Offset: 0x00225209
		protected RepeatNormalAttack(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060033EF RID: 13295 RVA: 0x00227018 File Offset: 0x00225218
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x00227070 File Offset: 0x00225270
		public override void OnDisable(DataContext context)
		{
			base.CombatChar.NormalAttackLeftRepeatTimes = 0;
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060033F1 RID: 13297 RVA: 0x002270D4 File Offset: 0x002252D4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = !interrupted;
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						this._affected = false;
						base.AddMaxEffectCount(true);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag4 = !interrupted;
					if (flag4)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x00227154 File Offset: 0x00225354
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = !this.IsSrcSkillPerformed || attacker != base.CombatChar || this._affected;
			if (!flag)
			{
				CombatCharacter combatChar = base.CombatChar;
				combatChar.NormalAttackLeftRepeatTimes += this.RepeatTimes;
				this._affected = true;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x002271AC File Offset: 0x002253AC
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = !this.IsSrcSkillPerformed || attacker != base.CombatChar || base.CombatChar.NormalAttackLeftRepeatTimes > 0;
			if (!flag)
			{
				this._affected = false;
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x002271F4 File Offset: 0x002253F4
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000F5B RID: 3931
		protected byte RepeatTimes;

		// Token: 0x04000F5C RID: 3932
		private bool _affected;
	}
}
