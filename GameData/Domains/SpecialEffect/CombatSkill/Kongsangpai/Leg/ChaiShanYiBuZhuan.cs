using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Leg
{
	// Token: 0x02000484 RID: 1156
	public class ChaiShanYiBuZhuan : CombatSkillEffectBase
	{
		// Token: 0x06003BB1 RID: 15281 RVA: 0x00249371 File Offset: 0x00247571
		public ChaiShanYiBuZhuan()
		{
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x0024937B File Offset: 0x0024757B
		public ChaiShanYiBuZhuan(CombatSkillKey skillKey) : base(skillKey, 10301, -1)
		{
		}

		// Token: 0x06003BB3 RID: 15283 RVA: 0x0024938C File Offset: 0x0024758C
		public override void OnEnable(DataContext context)
		{
			this._quickCastAffected = false;
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x002493CC File Offset: 0x002475CC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x00249408 File Offset: 0x00247608
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || this._quickCastAffected;
			if (!flag)
			{
				bool flag2 = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false).GetPreparingSkillId() >= 0;
				if (flag2)
				{
					this._quickCastAffected = true;
					DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x00249494 File Offset: 0x00247694
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = index != 3 || context.SkillKey != this.SkillKey || !base.CombatCharPowerMatchAffectRequire(0);
			if (!flag)
			{
				DomainManager.Combat.ChangeDistance(context, base.CombatChar, base.IsDirect ? -20 : 20);
				base.ShowSpecialEffectTips(1);
				DomainManager.Combat.SetDisplayPosition(context, base.CombatChar.IsAlly, base.IsDirect ? int.MinValue : DomainManager.Combat.GetDisplayPosition(base.CombatChar.IsAlly, DomainManager.Combat.GetCurrentDistance()));
				bool flag2 = !base.IsDirect;
				if (flag2)
				{
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					base.CombatChar.SetCurrentPosition(base.CombatChar.GetDisplayPosition(), context);
					enemyChar.SetCurrentPosition(enemyChar.GetDisplayPosition(), context);
				}
			}
		}

		// Token: 0x06003BB7 RID: 15287 RVA: 0x002495A4 File Offset: 0x002477A4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = this._quickCastAffected && isAlly != base.CombatChar.IsAlly;
			if (flag)
			{
				this._quickCastAffected = false;
			}
			bool flag2 = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag2)
			{
				bool flag3 = power > 0;
				if (flag3)
				{
				}
			}
		}

		// Token: 0x0400117C RID: 4476
		private const sbyte PrepareProgressPercent = 50;

		// Token: 0x0400117D RID: 4477
		private const sbyte ChangeDistance = 20;

		// Token: 0x0400117E RID: 4478
		private bool _quickCastAffected;
	}
}
