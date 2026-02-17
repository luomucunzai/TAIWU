using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Finger
{
	// Token: 0x02000496 RID: 1174
	public class WuHuanShou : CombatSkillEffectBase
	{
		// Token: 0x06003C2A RID: 15402 RVA: 0x0024C2C0 File Offset: 0x0024A4C0
		public WuHuanShou()
		{
		}

		// Token: 0x06003C2B RID: 15403 RVA: 0x0024C2CA File Offset: 0x0024A4CA
		public WuHuanShou(CombatSkillKey skillKey) : base(skillKey, 10205, -1)
		{
		}

		// Token: 0x06003C2C RID: 15404 RVA: 0x0024C2DC File Offset: 0x0024A4DC
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_ChangeBossPhase(new Events.OnChangeBossPhase(this.OnChangeBossPhase));
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x0024C334 File Offset: 0x0024A534
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_ChangeBossPhase(new Events.OnChangeBossPhase(this.OnChangeBossPhase));
		}

		// Token: 0x06003C2E RID: 15406 RVA: 0x0024C38C File Offset: 0x0024A58C
		private void OnNormalAttackCalcHitEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, int pursueIndex, bool hit, bool isFightback, bool isMind)
		{
			bool flag = attacker.IsAlly == base.CombatChar.IsAlly && defender == this._affectEnemy;
			if (flag)
			{
				this.RemoveEffect(context);
			}
		}

		// Token: 0x06003C2F RID: 15407 RVA: 0x0024C3C8 File Offset: 0x0024A5C8
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.Attacker.IsAlly == base.CombatChar.IsAlly && context.Defender == this._affectEnemy;
			if (flag)
			{
				this.RemoveEffect(context);
			}
		}

		// Token: 0x06003C30 RID: 15408 RVA: 0x0024C414 File Offset: 0x0024A614
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = this.SkillKey.IsMatch(charId, skillId) && base.PowerMatchAffectRequire((int)power, 0) && base.CombatChar.SkillAttackBodyPart == 2;
			if (flag)
			{
				this.AddEffect(context);
			}
		}

		// Token: 0x06003C31 RID: 15409 RVA: 0x0024C45A File Offset: 0x0024A65A
		private void OnChangeBossPhase(DataContext context)
		{
			this.RemoveEffect(context);
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x0024C464 File Offset: 0x0024A664
		private void AddEffect(DataContext context)
		{
			bool flag = this._affectEnemy != null;
			if (!flag)
			{
				this._affectEnemy = base.CurrEnemyChar;
				this._affectEnemy.ForbidNormalAttackEffectCount++;
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					this._affectEnemy.CanCastSkillCostStance = false;
				}
				else
				{
					this._affectEnemy.CanCastSkillCostBreath = false;
				}
				this._affectEnemy.AiController.ClearMemories();
				DomainManager.Combat.UpdateCanChangeTrick(context, this._affectEnemy);
				DomainManager.Combat.UpdateSkillCanUse(context, this._affectEnemy);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003C33 RID: 15411 RVA: 0x0024C504 File Offset: 0x0024A704
		private void RemoveEffect(DataContext context)
		{
			bool flag = this._affectEnemy == null;
			if (!flag)
			{
				this._affectEnemy.ForbidNormalAttackEffectCount--;
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					this._affectEnemy.CanCastSkillCostStance = true;
				}
				else
				{
					this._affectEnemy.CanCastSkillCostBreath = true;
				}
				DomainManager.Combat.UpdateCanChangeTrick(context, this._affectEnemy);
				DomainManager.Combat.UpdateSkillCanUse(context, this._affectEnemy);
				this._affectEnemy = null;
			}
		}

		// Token: 0x040011B1 RID: 4529
		private CombatCharacter _affectEnemy;
	}
}
