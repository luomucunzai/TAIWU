using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Agile
{
	// Token: 0x020005B5 RID: 1461
	public class CheckHitEffect : AgileSkillBase
	{
		// Token: 0x0600436E RID: 17262 RVA: 0x0026B670 File Offset: 0x00269870
		protected CheckHitEffect()
		{
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x0026B67A File Offset: 0x0026987A
		protected CheckHitEffect(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x06004370 RID: 17264 RVA: 0x0026B686 File Offset: 0x00269886
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._hitPercent = 100;
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x0026B6BD File Offset: 0x002698BD
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
		}

		// Token: 0x06004372 RID: 17266 RVA: 0x0026B6EC File Offset: 0x002698EC
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = base.CombatChar != (base.IsDirect ? attacker : defender) || pursueIndex > 0 || this._hitPercent <= 0;
			if (!flag)
			{
				this.CheckHit(context);
			}
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x0026B730 File Offset: 0x00269930
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = base.CombatChar != (base.IsDirect ? attacker : defender) || this._hitPercent <= 0;
			if (!flag)
			{
				this.CheckHit(context);
			}
		}

		// Token: 0x06004374 RID: 17268 RVA: 0x0026B770 File Offset: 0x00269970
		private void CheckHit(DataContext context)
		{
			bool flag = base.CanAffect && DomainManager.Combat.CheckHit(context, base.CombatChar, this.CheckHitType, this._hitPercent);
			if (flag)
			{
				bool flag2 = this.HitEffect(context);
				if (flag2)
				{
					base.ShowSpecialEffectTips(0);
				}
			}
			this._hitPercent -= 30;
		}

		// Token: 0x06004375 RID: 17269 RVA: 0x0026B7D0 File Offset: 0x002699D0
		protected virtual bool HitEffect(DataContext context)
		{
			return false;
		}

		// Token: 0x04001404 RID: 5124
		private const sbyte ReducePercentUnit = 30;

		// Token: 0x04001405 RID: 5125
		protected sbyte CheckHitType;

		// Token: 0x04001406 RID: 5126
		private int _hitPercent;
	}
}
