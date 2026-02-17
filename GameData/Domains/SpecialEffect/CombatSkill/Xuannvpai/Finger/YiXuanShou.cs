using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Finger
{
	// Token: 0x0200027E RID: 638
	public class YiXuanShou : CombatSkillEffectBase
	{
		// Token: 0x060030DA RID: 12506 RVA: 0x00218D00 File Offset: 0x00216F00
		public YiXuanShou()
		{
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x00218D0A File Offset: 0x00216F0A
		public YiXuanShou(CombatSkillKey skillKey) : base(skillKey, 8200, -1)
		{
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x00218D1B File Offset: 0x00216F1B
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060030DD RID: 12509 RVA: 0x00218D42 File Offset: 0x00216F42
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060030DE RID: 12510 RVA: 0x00218D6C File Offset: 0x00216F6C
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = index != 3 || context.SkillKey != this.SkillKey || !base.CombatCharPowerMatchAffectRequire(0);
			if (!flag)
			{
				DomainManager.Combat.ChangeDistance(context, base.CombatChar, (int)(base.IsDirect ? -20 : 20));
				base.ShowSpecialEffectTips(0);
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

		// Token: 0x060030DF RID: 12511 RVA: 0x00218E7C File Offset: 0x0021707C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000E7F RID: 3711
		private const sbyte ChangeDistance = 20;
	}
}
