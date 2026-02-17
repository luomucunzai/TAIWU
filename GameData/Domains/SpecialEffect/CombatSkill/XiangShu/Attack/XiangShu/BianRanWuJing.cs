using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.XiangShu
{
	// Token: 0x020002D5 RID: 725
	public class BianRanWuJing : CombatSkillEffectBase
	{
		// Token: 0x060032BB RID: 12987 RVA: 0x00220A7B File Offset: 0x0021EC7B
		public BianRanWuJing()
		{
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x00220A85 File Offset: 0x0021EC85
		public BianRanWuJing(CombatSkillKey skillKey) : base(skillKey, 17095, -1)
		{
		}

		// Token: 0x060032BD RID: 12989 RVA: 0x00220A96 File Offset: 0x0021EC96
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060032BE RID: 12990 RVA: 0x00220ACF File Offset: 0x0021ECCF
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060032BF RID: 12991 RVA: 0x00220B08 File Offset: 0x0021ED08
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = base.CombatChar != combatChar || DomainManager.Combat.Pause;
			if (!flag)
			{
				this._frameCounter++;
				bool flag2 = this._frameCounter < 60 || !DomainManager.Combat.InAttackRange(base.CombatChar);
				if (!flag2)
				{
					this._frameCounter = 0;
					CombatCharacter enemyChar = base.CurrEnemyChar;
					bool flag3 = enemyChar.GetCharacter().GetXiangshuInfection() < 200;
					if (flag3)
					{
						enemyChar.GetCharacter().ChangeXiangshuInfection(context, 1 + enemyChar.OriginXiangshuInfection * 10 / 100);
					}
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x060032C0 RID: 12992 RVA: 0x00220BB0 File Offset: 0x0021EDB0
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.SkillKey != this.SkillKey || index != 2 || !base.CombatCharPowerMatchAffectRequire(0);
			if (!flag)
			{
				bool flag2 = context.Defender.GetCharacter().GetXiangshuInfection() >= 200 && !context.Defender.Immortal;
				if (flag2)
				{
					context.Defender.ForceDefeat = true;
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x060032C1 RID: 12993 RVA: 0x00220C30 File Offset: 0x0021EE30
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000F03 RID: 3843
		private const sbyte AffectFrameCount = 60;

		// Token: 0x04000F04 RID: 3844
		private int _frameCounter;
	}
}
