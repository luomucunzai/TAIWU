using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Agile
{
	// Token: 0x02000505 RID: 1285
	public class QiXingFeiXuan : AgileSkillBase
	{
		// Token: 0x06003E9D RID: 16029 RVA: 0x00256865 File Offset: 0x00254A65
		private bool IsAffectChar(CombatCharacter attacker)
		{
			return base.IsCurrent && (base.IsDirect ? (attacker.GetId() == base.CharacterId) : (attacker.IsAlly != base.CombatChar.IsAlly));
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x002568A0 File Offset: 0x00254AA0
		public QiXingFeiXuan()
		{
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x002568AA File Offset: 0x00254AAA
		public QiXingFeiXuan(CombatSkillKey skillKey) : base(skillKey, 13406)
		{
		}

		// Token: 0x06003EA0 RID: 16032 RVA: 0x002568BC File Offset: 0x00254ABC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_AttackSkillAttackEndOfAll(new Events.OnAttackSkillAttackEndOfAll(this.OnAttackSkillAttackEndOfAll));
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
		}

		// Token: 0x06003EA1 RID: 16033 RVA: 0x00256908 File Offset: 0x00254B08
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_AttackSkillAttackEndOfAll(new Events.OnAttackSkillAttackEndOfAll(this.OnAttackSkillAttackEndOfAll));
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			base.OnDisable(context);
		}

		// Token: 0x06003EA2 RID: 16034 RVA: 0x00256954 File Offset: 0x00254B54
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = this.IsAffectChar(attacker) && base.CanAffect && !attacker.IsUnlockAttack;
			if (flag)
			{
				this.DoSettlement(context);
			}
		}

		// Token: 0x06003EA3 RID: 16035 RVA: 0x0025698C File Offset: 0x00254B8C
		private void OnAttackSkillAttackEndOfAll(DataContext context, CombatCharacter character, int index)
		{
			bool flag = this.IsAffectChar(character) && base.CanAffect;
			if (flag)
			{
				this.DoSettlement(context);
			}
		}

		// Token: 0x06003EA4 RID: 16036 RVA: 0x002569B8 File Offset: 0x00254BB8
		private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			CombatCharacter attacker = DomainManager.Combat.GetElement_CombatCharacterDict(attackerId);
			bool flag = this.IsAffectChar(attacker) && base.CanAffect && !attacker.IsUnlockAttack;
			if (flag)
			{
				this._catchingDamage += damageValue;
			}
		}

		// Token: 0x06003EA5 RID: 16037 RVA: 0x00256A04 File Offset: 0x00254C04
		private void DoSettlement(DataContext context)
		{
			this._attackCount++;
			bool flag = this._attackCount < 7;
			if (!flag)
			{
				DomainManager.Combat.AddFatalDamageValue(context, base.CurrEnemyChar, this._catchingDamage * QiXingFeiXuan.AddFatalDamagePercent, -1, -1, -1, EDamageType.None);
				this._attackCount = (this._catchingDamage = 0);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x0400127A RID: 4730
		private const int SettlementAttackCount = 7;

		// Token: 0x0400127B RID: 4731
		private static readonly CValuePercent AddFatalDamagePercent = 40;

		// Token: 0x0400127C RID: 4732
		private int _attackCount;

		// Token: 0x0400127D RID: 4733
		private int _catchingDamage;
	}
}
