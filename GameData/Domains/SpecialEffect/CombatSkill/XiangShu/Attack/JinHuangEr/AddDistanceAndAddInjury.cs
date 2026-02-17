using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.JinHuangEr
{
	// Token: 0x02000308 RID: 776
	public class AddDistanceAndAddInjury : CombatSkillEffectBase
	{
		// Token: 0x060033D6 RID: 13270 RVA: 0x00226B8B File Offset: 0x00224D8B
		protected AddDistanceAndAddInjury()
		{
		}

		// Token: 0x060033D7 RID: 13271 RVA: 0x00226B95 File Offset: 0x00224D95
		protected AddDistanceAndAddInjury(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x00226BA2 File Offset: 0x00224DA2
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060033D9 RID: 13273 RVA: 0x00226BC9 File Offset: 0x00224DC9
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x00226BF0 File Offset: 0x00224DF0
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = index != 3 || context.SkillKey != this.SkillKey || !base.CombatCharPowerMatchAffectRequire(0);
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				DomainManager.Combat.ChangeDistance(context, enemyChar, 60, true);
				bool flag2 = (short)DomainManager.Combat.GetDistanceRange().Item2 - DomainManager.Combat.GetCurrentDistance() < 60;
				if (flag2)
				{
					base.ChangeMobilityValue(context, enemyChar, -enemyChar.GetMobilityValue());
					base.ClearAffectingAgileSkill(context, enemyChar);
					for (int i = 0; i < (int)this.InjuryCount; i++)
					{
						DomainManager.Combat.AddRandomInjury(context, enemyChar, context.Random.CheckPercentProb(50), 1, 1, true, -1);
					}
				}
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x00226CE8 File Offset: 0x00224EE8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000F53 RID: 3923
		private const sbyte AddDistance = 60;

		// Token: 0x04000F54 RID: 3924
		private const sbyte AffectRequireDistance = 60;

		// Token: 0x04000F55 RID: 3925
		protected sbyte InjuryCount;
	}
}
