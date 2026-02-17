using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.NoSect.FistAndPalm
{
	// Token: 0x02000473 RID: 1139
	public class TaiZuChangQuan : CombatSkillEffectBase
	{
		// Token: 0x06003B58 RID: 15192 RVA: 0x00247893 File Offset: 0x00245A93
		public TaiZuChangQuan()
		{
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x0024789D File Offset: 0x00245A9D
		public TaiZuChangQuan(CombatSkillKey skillKey) : base(skillKey, 100, -1)
		{
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x002478AB File Offset: 0x00245AAB
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x002478E4 File Offset: 0x00245AE4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x00247920 File Offset: 0x00245B20
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker != base.CombatChar || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this.IsSrcSkillPrepared = true;
			}
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x00247954 File Offset: 0x00245B54
		private void OnCombatSettlement(DataContext context, sbyte combatStatus)
		{
			bool flag = !this.IsSrcSkillPrepared || !base.CombatChar.IsAlly || combatStatus != CombatStatusType.EnemyFail;
			if (!flag)
			{
				DomainManager.Combat.AppendEvaluation(32);
				base.ShowSpecialEffectTipsByDisplayEvent(0);
			}
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x002479A0 File Offset: 0x00245BA0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetMainCharacter(!base.CombatChar.IsAlly);
				bool flag2 = DomainManager.Combat.IsCharacterFallen(enemyChar);
				if (!flag2)
				{
					base.RemoveSelf(context);
				}
			}
		}
	}
}
