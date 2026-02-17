using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon
{
	// Token: 0x020001E4 RID: 484
	public abstract class SwordAddFatalEffectBase : SwordUnlockEffectBase
	{
		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06002DDB RID: 11739 RVA: 0x0020D253 File Offset: 0x0020B453
		private int SelfFatalDamagePercent
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? 10 : 5;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06002DDC RID: 11740
		protected abstract CValueMultiplier FlawOrAcupointCount { get; }

		// Token: 0x06002DDD RID: 11741 RVA: 0x0020D262 File Offset: 0x0020B462
		protected SwordAddFatalEffectBase()
		{
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x0020D26C File Offset: 0x0020B46C
		protected SwordAddFatalEffectBase(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x0020D278 File Offset: 0x0020B478
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.AddDirectDamageValue));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x0020D2C4 File Offset: 0x0020B4C4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.AddDirectDamageValue));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x0020D310 File Offset: 0x0020B510
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId;
			if (!flag)
			{
				this._directDamageValue = 0;
			}
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x0020D33C File Offset: 0x0020B53C
		private void AddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId;
			if (!flag)
			{
				this._directDamageValue += damageValue;
			}
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x0020D36C File Offset: 0x0020B56C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || this._directDamageValue <= 0;
			if (!flag)
			{
				CValuePercent percent = 0;
				bool flag2 = skillId == base.SkillTemplateId && base.IsReverseOrUsingDirectWeapon;
				if (flag2)
				{
					percent += this.SelfFatalDamagePercent;
				}
				bool flag3 = base.EffectCount > 0;
				if (flag3)
				{
					percent += 5;
				}
				percent *= this.FlawOrAcupointCount;
				int fatalDamageValue = this._directDamageValue * percent;
				this._directDamageValue = 0;
				bool flag4 = fatalDamageValue <= 0;
				if (!flag4)
				{
					bool flag5 = base.EffectCount > 0;
					if (flag5)
					{
						base.ReduceEffectCount(1);
					}
					DomainManager.Combat.AddFatalDamageValue(context, base.CurrEnemyChar, fatalDamageValue, -1, -1, -1, EDamageType.None);
					base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
				}
			}
		}

		// Token: 0x04000DB5 RID: 3509
		private const int EffectFatalDamagePercent = 5;

		// Token: 0x04000DB6 RID: 3510
		private int _directDamageValue;
	}
}
